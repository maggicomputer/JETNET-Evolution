
Partial Public Class View

  Inherits System.Web.UI.Page



  Public Jetnet As Boolean = True
  Public Client As Boolean = False
  Public Client_DB As String = ""
  Public Ref_DB As String = ""
  Public direct_costs As String = ""
  Public annual_costs As String = ""
  Public budget_costs As String = ""
  Public total_costs As String = ""
  Public GetMarketStatus As String = ""
  Public Build_FleetMarketSummary_text As String = ""
  'Public Class evoSubscriptionClass
  '    Public evoProductCode As Array
  '    Public evoTierlevel As Integer
  'End Class
  'Public Enum eProductCodeTypes As Integer

  '    NULL = 0
  '    B = 1 ' Business     ** check tier level for make/model selection
  '    H = 2 ' Helicopters  ** ignore tier level
  '    C = 4 ' Commerical   ** ignore tier level
  '    R = 8 ' Regional     ** ignore tier level
  '    A = 16 ' Aviation Business Index     ** ignore tier level
  '    P = 32 ' AirBP     ** ignore tier level
  '    S = 64 ' STAR Reports     ** ignore tier level
  '    I = 128 ' SPI View     ** ignore tier level
  'End Enum
  'Public Enum eTierLevelTypes As Integer

  '    NULL = 0
  '    JETS = 1
  '    TURBOS = 2
  '    ALL = 4

  'End Enum
  Dim make_model_name As String = ""
  Dim amod_id As Integer = 272
  Dim months_count As Integer = 6
  Dim string_for_op_percentage As String = ""
  Dim QUOTE As String = "&quot;"
  Dim make_name As String = ""
  Dim sub_info As String = ""
  Dim sub_type As String = ""
  Dim weight_class As String = ""
  Dim weight_class_name As String = ""
  Dim spi_year As Integer = 2005
  Dim spi_year2 As Integer = 2011
  Dim range_constant As Long = 0
  Dim airframe_type_num As Integer = 2
  Dim airframe_type As String = ""
  Dim quarter As Integer = 1
  Dim string_for_spi_start As String = ""
  Dim Amod_manufacturer As String = ""
  Dim type_code As String = ""
  Dim Amod_description As String = ""
  Dim start_end_years As String = ""
  Dim ser_nbr_range As String = ""
  Dim amod_type_name As String = ""
  Dim amod_price_range As String = ""
  Dim model_name As String = ""
  '   Dim selected_product_code_from_drop_down As String = "B"
  Dim comp_id As Integer = 0
  Dim real_company_name As String = ""
  Dim string_from_op_costs_for_range As String = ""
  Dim temp_op_cost_string As String = ""
  Dim avg_days_on_market As Integer = 0
  ' Public crmSubScriptionCls As evoSubscriptionClass
  Public Const cAndClause = " AND "
  Public Const cOrClause = " OR "
  Public Const cLikeClause = " LIKE "
  Public Const cInClause = " IN "
  Public Const cBetweenClause = " BETWEEN "
  Public Const cConvertClause = " CONVERT "
  Public Const cSingleOpen = "("
  Public Const cDoubleOpen = "(("
  Public Const cSingleClose = ")"
  Public Const cDoubleClose = "))"
  Public Const cEmptyString = ""
  Public Const cSingleSpace = " "
  Public Const cSingleQuote = "'"
  Public Const cDoubbleSingleQuote = "''"
  Public Const cValueSeperator = "','"
  Public for_sale_flag As Boolean = False
  Public field_length As Double = 0





  Private Sub print_spec_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not Me.IsPostBack Then
      Client_DB = CStr(Application.Item("crmClientDatabase"))
      Ref_DB = CStr(Application.Item("crmActiveDatabase"))

    End If

    Session.Item("fuelPriceBase") = Get_Fuel_Price()
  End Sub
  'I made a change on 5-3-2012.
  'This change changed the database connection to use the 
  'Application.Item("crmJetnetDatabase")
  'This is set up in the master page
  'To use whatever database the client is supposed to be viewing. 
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    '        Rules()
    'Aerodex vs Marketplace
    '----Aerodex users can not see the following tabs:
    '-----------Market Status [validate this on evo]
    '-----------For sale 
    '-----------SPI

    'If Session.Item("localSubscription").crmBusiness_Flag = True Then
    '    selected_product_code_from_drop_down = "B"
    'ElseIf Session.Item("localSubscription").crmCommercial_Flag = True Then
    '    selected_product_code_from_drop_down = "C"
    'ElseIf Session.Item("localSubscription").crmHelicopter_Flag = True Then
    '    selected_product_code_from_drop_down = "H"
    'End If


    If Trim(Request("amod_id")) <> "" Then
      amod_id = Trim(Request("amod_id"))
      Session.Item("localUser").crmUserSelectedModel = amod_id
    Else
      amod_id = Session.Item("localUser").crmUserSelectedModel
      If amod_id = 0 Then
        If Session.Item("localSubscription").crmBusiness_Flag = True Then
          If Session.Item("localSubscription").crmJets_Flag = False Then
            amod_id = 193
          Else
            amod_id = 272
          End If
        ElseIf Session.Item("localSubscription").crmCommercial_Flag = True Then
          amod_id = 717
        ElseIf Session.Item("localSubscription").crmHelicopter_Flag = True Then
          amod_id = 646
        End If
      End If
    End If

    If Session.Item("localSubscription").crmAerodexFlag = True Then
      market_status_tab.Visible = False
      for_sale_tab.Visible = False
      Me.tabs_container.ActiveTabIndex = 1
    ElseIf Not Session.Item("localSubscription").crmCommercial_Flag = True And Session.Item("localSubscription").crmBusiness_Flag = False And Session.Item("localSubscription").crmHelicopter_Flag = False Then
      ' if its not commercial conly
      market_status_tab.Visible = True
      for_sale_tab.Visible = True
    End If


    If Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
      spi_tab.Visible = True
      Me.spi_label.Text = "Please Wait a Moment for Sales Price Index Data..."
    Else
      spi_tab.Visible = False
    End If





    range_tab.Visible = False

    Get_Model_Info(amod_id)

    Master.ListingID = 0
    Master.Listing_ContactID = 0
    Master.ListingSource = ""



    If Not Me.IsPostBack Then

      create_make_model_list()

      Dim pic_exists As Boolean = False
      pic_exists = check_pic_exists()


      If pic_exists Then
        Me.aircraft_image.ImageUrl = "/pictures/model/" & amod_id & ".jpg"
        Me.label_behind_pic.Text = ""
      Else
        Me.aircraft_image.Visible = False
        Me.label_behind_pic.Text = "No Picture Available"
      End If


      If Trim(make_model_name) <> "" Then
        Me.make_model_name_label.Text = make_model_name
      End If

      ' ------------- these are the top tabs, that will load the first time the page is loaded---------
      If Session.Item("localSubscription").crmCommercial_Flag = True And Session.Item("localSubscription").crmBusiness_Flag = False And Session.Item("localSubscription").crmHelicopter_Flag = False Then
        ' if its commercial conly
        Me.market_status_tab.Visible = False
        Me.fleet_tab.Visible = False
        Me.specs_tab.Visible = False
        Me.operating_costs_tab.Visible = False


        description_tab_label_direct_clicked()
        reports_label_clicked()
        tabs_container.ActiveTabIndex = 4
      Else
        market_summary_clicked(Session.Item("localSubscription").crmAerodexFlag) ' market summary needs to run either way for the fleet tab information
        fleet_tab_clicked()
        description_tab_label_direct_clicked()
        specs_tab_label_clicked()
        operating_costs_tab_label_clicked()
        reports_label_clicked()
      End If


      Me.retail_sales_label.Text = "Please Wait a Moment for Recent Retail Sales..."
      Me.market_activity_label.Text = "Please Wait a Moment for Maket Activity Data..."
      Me.news_label.Text = "Please Wait a Moment for News Data..."
      Me.range_label.Text = "Please Wait a Moment for Range Data..."
      Me.wanteds_label.Text = "Please Wait a Moment for Wanteds Data..."
      Me.documents_label.Text = "Please Wait a Moment for Documents Data..."
      Me.operators_label.Text = "Please Wait a Moment for Operator Data..."
      Me.charter_label.Text = "Please Wait a Moment for Charter Data..."
      Me.lease_label.Text = "Please Wait a Moment for Lease Data..."
      Me.flights_label.Text = "Please Wait a Moment for Flight Data..."
      Me.for_sale_label.Text = "Please Wait a Moment for For Sale Data..."


      ' ------------- these are the top tabs, that will load the first time the page is loaded---------


      ' ------------- this is the bottom tab, that will load the first time the page is loaded---------
      market_trends_label_clicked()
      ' ------------- this is the bottom tab, that will load the first time the page is loaded---------



    End If
  End Sub
  Function on_click_load_bottom() Handles Me.PreRender
    on_click_load_bottom = ""

    Dim wanted_index As Integer = 0
    ' no need to double load 0 
    If Not IsNothing(Session.Item("localSubscription")) Then


      Dim crmSubScriptionCls2 As crmSubscriptionClass = Session.Item("localSubscription")


      If crmSubScriptionCls2.crmAerodexFlag = False Then
        If TabContainer1.ActiveTabIndex = 1 Then
          If Len(Me.for_sale_label.Text) < 55 Then
            for_sale_label_clicked()
          End If
        End If
        wanted_index = 0
      Else
        wanted_index = -1
      End If


      If TabContainer1.ActiveTabIndex = wanted_index + 2 Then
        If Len(Me.retail_sales_label.Text) < 55 Then
          retail_sales_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 2 ' 2
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 3 Then
        If Len(Me.market_activity_label.Text) < 55 Then
          market_activity_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 3 '3
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 4 Then
        If Len(Me.news_label.Text) < 55 Then
          news_label_clicked()
        End If
        TabContainer1.ActiveTabIndex = wanted_index + 4
        'ElseIf TabContainer1.ActiveTabIndex = wanted_index + 5 Then
        '    If Len(Me.range_label.Text) < 55 Then
        '        range_label_clicked()
        '    End If
        '    TabContainer1.ActiveTabIndex = 12 '5
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 5 Then
        If Len(Me.wanteds_label.Text) < 55 Then
          wanteds_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 6
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 6 Then
        If Len(Me.documents_label.Text) < 55 Then
          documents_label_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 7
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 7 Then
        If Len(Me.operators_label.Text) < 55 Then
          operators_label_clicked()
        End If
        ' TabContainer1.ActiveTabIndex = wanted_index + 8
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 8 Then
        If Len(Me.charter_label.Text) < 55 Then
          charter_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 9
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 9 Then
        If Len(Me.lease_label.Text) < 55 Then
          lease_label_clicked()
        End If
        ' TabContainer1.ActiveTabIndex = wanted_index + 10
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 10 Then
        If Len(Me.flights_label.Text) < 55 Then
          flights_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 11
      ElseIf TabContainer1.ActiveTabIndex = wanted_index + 12 Then
        If Len(Me.spi_label.Text) < 55 Then
          spi_label_clicked()
        End If
        'TabContainer1.ActiveTabIndex = wanted_index + 11
      End If

    End If

  End Function

  Function market_summary_clicked(ByVal aero As Boolean)    ' dont want it really on load   Handles market_status_tab.Load
    market_summary_clicked = ""
    If Not Me.IsPostBack Then

      Build_FleetMarketSummary(amod_id, False, "Challenger", "Fleet")

      If aero = False Then
        Me.FOR_SALE.Titles.Add(display_for_sale_by_month_graph(amod_id, FOR_SALE))
        Me.FOR_SALE.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_FOR_SALE.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)




        Me.market_status_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
        Me.market_status_tab_label.Text += "<table width='100%' valign='top' cellspacing='0' cellpadding='0'><tr valign='top'><td width='100%' valign='top' colspan='2'>"
        Me.market_status_tab_label.Text += display_market_up_down_one_model(50, make_model_name, amod_id, months_count, "F", "")
        Me.market_status_tab_label.Text += "</td></tr><tr><td colspan='2' width='100%' height='1'  bgcolor='#67A0D9'>"
        Me.market_status_tab_label.Text += "</td></tr>"
        Me.market_status_tab_label.Text += "</td></tr><tr><td width='60%' valign='top'>"
        Me.market_status_tab_label.Text += GetMarketStatus
        Me.market_status_tab_label.Text += "</td><td width='40%' valign='top'>"
        Me.market_status_tab_label.Text += "<img src='TempFiles/" & amod_id & "_FOR_SALE.jpg'>"
        Me.market_status_tab_label.Text += "</td></tr></table>"
        Me.market_status_tab_label.Text += "</div></td></tr></table>"
      End If

    End If
  End Function
  Function fleet_tab_clicked()   ' dont want it really on load Handles fleet_tab_label.Load
    fleet_tab_clicked = ""
    If Not Me.IsPostBack Then
      Me.fleet_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
      Me.fleet_tab_label.Text += Build_FleetMarketSummary_text
      Me.fleet_tab_label.Text += "</div></td></tr></table>"
    End If
  End Function
  Function specs_tab_label_clicked() ' dont want it really on load     Handles specs_tab_label.Load 

    specs_tab_label_clicked = ""
    Me.specs_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
    Me.specs_tab_label.Text += Build_PerformanceSpecifications(False, "", False, "", amod_id, make_model_name)
    Me.specs_tab_label.Text += "</div></td></tr></table>"


  End Function
  Function documents_label_label_clicked() ' dont want it really on load    Handles documents_label.Load 

    documents_label_label_clicked = ""
    Me.documents_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
    Me.documents_label.Text = display_transaction_documents(amod_id, 500, "", "", "", 0, make_model_name, 6, "", "", "", "", "", "", "J")
    Me.documents_label.Text += "</div></td></tr></table>"

  End Function
  Function description_tab_label_direct_clicked()  ' dont want it really on load   Handles description_tab_label_direct.Load 

    description_tab_label_direct_clicked = ""
    Me.description_tab_label_direct.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left'  valign='top'><div class='tab_container_div2'>"
    Me.description_tab_label_direct.Text += "<table width='100%' cellspacing='0' cellpadding='0'>" '  bgcolor='#CCCCCC'
    Me.description_tab_label_direct.Text += "<tr><td><b>MANUFACTURER:&nbsp;</b></td><td>" & Amod_manufacturer & "</td>"
    Me.description_tab_label_direct.Text += "<td><b>YEARS BUILT:&nbsp;</b></td><td>" & start_end_years & "</td></tr>"
    Me.description_tab_label_direct.Text += "<tr><td><b>SER # RANGE:&nbsp;</b></td><td>" & ser_nbr_range & "</td>"
    Me.description_tab_label_direct.Text += "<td><b>TYPE:&nbsp;</b></td><td>" & amod_type_name & "</td></tr>"
    Me.description_tab_label_direct.Text += "<tr valign='top'><td><b>WEIGHT CLASS:&nbsp;</b></td><td>" & weight_class_name & "</td>"
    Me.description_tab_label_direct.Text += "<td><b>PRICE RANGE:&nbsp;</b></td><td>" & amod_price_range & "<br><br></td></tr>"
    Me.description_tab_label_direct.Text += "<tr><td colspan='4' width='100%' height='1'  bgcolor='#67A0D9'></td></tr>"
    Me.description_tab_label_direct.Text += "<tr><td colspan='4'><br>"
    Me.description_tab_label_direct.Text += Amod_description
    Me.description_tab_label_direct.Text += "</td></tr></table>"
    Me.description_tab_label_direct.Text += "</div></td></tr></table>"

  End Function '

  Function charter_label_clicked() ' dont want it really on load Handles charter_label.Load  

    charter_label_clicked = ""
    Me.charter_label.Text = "<table width='100%' cellpadding='3' height='200' cellspacing='0'><tr><td align='left' valign='top'>"
    Me.charter_label.Text += "<table width='100%' cellspacing='0' cellpadding='0'><tr><td width='40%'><div class='tab_container_div'>"
    Me.charter_label.Text += display_acContactType_location_piechart_city(0, amod_id, airframe_type, "", " ORDER BY comp_city, comp_state asc ", sub_info, make_model_name, "", sub_type)
    Me.charter_label.Text += "</div></td><td width='60%'><div class='tab_container_div'>"
    Me.charter_label.Text += acContactType_companies(amod_id, make_model_name, 0, airframe_type, "", "", sub_info, "", sub_type)
    Me.charter_label.Text += "</div></td></tr></table>"
    Me.charter_label.Text += "</td></tr></table>"

  End Function '
  Function operating_costs_tab_label_clicked()  ' dont want it really on load    Handles specs_tab_label.Load
    operating_costs_tab_label_clicked = ""
    Dim to_run As String = ""


    Me.operating_costs_tab_label_direct.Text = "<table width='100%' cellpadding='3'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
    Me.operating_costs_tab_label_direct.Text += Build_OperatingCosts(amod_id, make_model_name)
    Me.operating_costs_tab_label_direct.Text += "</div></td></tr></table>"


  End Function
  Function range_label_clicked() ' dont want it really on load  Handles range_label.Load 

    range_label_clicked = ""
    Build_PerformanceSpecifications(False, "", False, "", amod_id, make_model_name)
    '  Me.range_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
    '  Me.range_label.Text += "<table width='100%'><tr><td width='25%'><table width='100%'>"

    Me.range_label.Text = "<table><tr><td><table>"
    Me.range_label.Text += string_from_op_costs_for_range
    Me.range_label.Text += "</table></td></tr></table>"

    '  Me.range_label.Text += "</table>"
    '  Me.range_label.Text += "</td><td width='60%' align='center'>"
    ' Me.range_label.Text += 
    get_map_from_text()
    ' Me.range_label.Text += "</td></tr></table>"
    ' Me.range_label.Text += "</div></td></tr></table>"

  End Function
  Function retail_sales_label_clicked()   ' dont want it really on load  Handles retail_sales_label.Load

    'added here on 5-3-2012
    'We determined that the sold per month graph is pretty slow
    'So we moved it from the market trends tab to the retail sales tab

    Me.PER_MONTH.Titles.Add(display_sold_per_month_graph(False, False, amod_id, PER_MONTH, 6, True))
    Me.PER_MONTH.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_PER_MONTH.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)


    retail_sales_label_clicked = ""
    Me.retail_sales_label.Text = "<table width='100%' cellpadding='0' height='200' cellspacing='0'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.retail_sales_label.Text += Build_RecentRetailSales(amod_id)
    Me.retail_sales_label.Text += "</div></td><td>"
    Me.retail_sales_label.Text += "<img src='TempFiles/" & amod_id & "_PER_MONTH.jpg' width='260' height='260'>"
    Me.retail_sales_label.Text += "</td></tr></table>"


  End Function
  Function market_activity_label_clicked()   ' dont want it really on load  Handles market_activity_label.Load

    market_activity_label_clicked = ""
    Me.market_activity_label.Text = "<table width='100%' cellpadding='0' height='200' cellspacing='0' valign='top'><tr valign='top'><td align='left' valign='top'><div class='tab_container_div'>"
    Me.market_activity_label.Text += Build_RecentMarketActivity(amod_id)
    Me.market_activity_label.Text += "</div></td></tr></table>"


  End Function
  Function lease_label_clicked() ' dont want it really on load Handles lease_label.Load 
    Dim text_for_graph_title As String = ""

    lease_label_clicked = ""

    Me.lease_label.Text = "<table width='100%' cellpadding='0' height='200' cellspacing='0' valign='top'><tr valign='top'><td align='left' valign='top'>"
    Me.lease_label.Text += "<table width='100%' cellpadding='0' cellspacing='0' valign='top'><tr><td width='50%'><div class='tab_container_div'>"
    text_for_graph_title = display_leases_sold_by_month("", 195, "", make_model_name, amod_id, 0, airframe_type, "")

    If text_for_graph_title <> "" Then
      Me.AVG_SOLD_PER_MONTH.Titles.Add(text_for_graph_title)
      Me.AVG_SOLD_PER_MONTH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      Me.AVG_SOLD_PER_MONTH.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "AVG_SOLD_PER_MONTH.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
      Me.lease_label.Text += "<table align='center' valign='top'><tr valign='top'><td align='center'><img src='TempFiles\" & amod_id & "AVG_SOLD_PER_MONTH.jpg'></td></tr></table>"
    Else
      Me.lease_label.Text += "There Are No Leases Sold In The Last 6 Months"
    End If

    Me.lease_label.Text += display_leases_expired("", "", make_model_name, amod_id, 0, False, airframe_type, sub_info, "", "", 6, "", make_model_name)
    Me.lease_label.Text += display_leases_due_to_expire("", "", make_model_name, amod_id, 0, False, airframe_type, "", 1, sub_info, sub_type, "", make_model_name)
    Me.lease_label.Text += "</div></td><td width='50%'><div class='tab_container_div'>"
    Me.lease_label.Text += displayLeasedAircraft(amod_id, comp_id, sub_info, make_model_name, real_company_name, sub_type, airframe_type, "", "")

    Me.lease_label.Text += "</div></td></tr></table>"
    Me.lease_label.Text += "</td></tr></table>"

  End Function

  Function for_sale_label_clicked()  ' dont want it really on load   Handles for_sale_label.Load

    for_sale_label_clicked = ""
    Me.for_sale_label.Text = "<table width='100%' cellpadding='0' height='200' cellspacing='0'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.for_sale_label.Text += Build_AircraftForSale(amod_id, "", False, "")
    Me.for_sale_label.Text += "</div></td></tr></table>"

  End Function
  Function flights_label_clicked() ' dont want it really on load   Handles flights_label.Load 

    flights_label_clicked = ""
    Me.flights_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.flights_label.Text += GetFlightActivity(amod_id)
    Me.flights_label.Text += "</div></td></tr></table>"

  End Function
  Function wanteds_label_clicked()  ' dont want it really on load   Handles wanteds_label.Load

    wanteds_label_clicked = ""
    Me.wanteds_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.wanteds_label.Text += GetWantedInfo(amod_id)
    Me.wanteds_label.Text += "</div></td></tr></table>"

  End Function
  Function news_label_clicked() ' dont want it really on load    Handles news_label.Load 

    news_label_clicked = ""
    Me.news_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.news_label.Text += GetNewsInfo(amod_id)
    Me.news_label.Text += "</div></td></tr></table>"
  End Function
  Function spi_label_clicked()   ' dont want it really on load   Handles spi_label.Load

    spi_label_clicked = ""
    Me.spi_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
    Me.spi_label.Text += "<table width='100%'><Tr><td>"
    Me.spi_label.Text += Build_SPI(make_model_name, amod_id, sub_info, sub_type, weight_class, weight_class_name, spi_year, spi_year2, airframe_type_num, quarter, True)
    Me.spi_label.Text += "</td></Tr></table>"
    Me.spi_label.Text += "</div></td></tr></table>"

  End Function
  Function reports_label_clicked() ' dont want it really on load   Handles reports_label.Load 

    reports_label_clicked = ""
    If Not Me.IsPostBack Then
      Me.reports_label.Text = "<table width='100%' cellpadding='3' cellspacing='0' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
      Me.reports_label.Text += "<table width='100%'><tr class='aircraft_list'>"
      Me.reports_label.Text += "<td colspan='3'>WORD/PDF</td>"
      Me.reports_label.Text += "</tr><tr class='alt_row'><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>"
      Me.reports_label.Text += "<td>CHARTER INTELLIGGENCE</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"
      Me.reports_label.Text += "</tr><tr bgcolor='white'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>LEASED AIRCRAFT</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"
      Me.reports_label.Text += "</tr><tr class='alt_row'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>OPERATOR/AIRCRAFT SUMMARY</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"
      Me.reports_label.Text += "</tr><tr bgcolor='white'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>MODEL MARKET SUMMARY VIEW</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"
      Me.reports_label.Text += "</tr><tr class='alt_row'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>SALES PRICE INDEX (SPI)</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"
      Me.reports_label.Text += "</tr><tr bgcolor='white'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>FINANCIAL/MARKET LIST</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"

      Me.reports_label.Text += "</tr><tr class='aircraft_list'><td colspan='3'>EXCEL</td>"
      Me.reports_label.Text += "</tr><tr bgcolor='white'><td>&nbsp;</td>"
      Me.reports_label.Text += "<td>MODEL MARKET LIST</td>"
      Me.reports_label.Text += "<td><a href=''>Example</a></td>"

      Me.reports_label.Text += "</td></Tr></table>"
      Me.reports_label.Text += "</div></td></tr></table>"
    End If

  End Function





  Function market_trends_label_clicked()   ' dont want it really on load Handles market_trends_label.Load 
    market_trends_label_clicked = ""

    If Not Me.IsPostBack Then
      Me.AVG_PRICE_MONTH.Titles.Add(display_avg_price_by_month_graph(amod_id, 6))
      ' Me.PER_MONTH.Titles.Add(display_sold_per_month_graph(False, False, amod_id, PER_MONTH, 6, True))
      Me.AVG_DAYS_ON.Titles.Add(display_average_days_on_market_graph(amod_id, AVG_DAYS_ON))


      Me.AVG_PRICE_MONTH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      Me.AVG_PRICE_MONTH.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_AVG_PRICE_MONTH.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
      'Me.PER_MONTH.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_PER_MONTH.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
      Me.AVG_DAYS_ON.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_AVG_DAYS_ON.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)


      Me.market_trends_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div'>"
      Me.market_trends_label.Text += "<table><tr><td>"
      Me.market_trends_label.Text += "<img src='TempFiles/" & amod_id & "_AVG_PRICE_MONTH.jpg' width='390' height='260'>"
      ' Me.market_trends_label.Text += "</td><td>"
      '  Me.market_trends_label.Text += "<img src='TempFiles/" & amod_id & "_FOR_SALE.jpg'>"
      Me.market_trends_label.Text += "</td><td>"
      ' Me.market_trends_label.Text += "<img src='TempFiles/" & amod_id & "_PER_MONTH.jpg' width='260' height='260'>"
      Me.market_trends_label.Text += "</td><td>"
      Me.market_trends_label.Text += "<img src='TempFiles/" & amod_id & "_AVG_DAYS_ON.jpg' width='390' height='260'>"
      Me.market_trends_label.Text += "</td></tr></table>"
      Me.market_trends_label.Text += "</div></td></tr></table>"

    End If


  End Function

  Function operators_label_clicked()  ' dont want it really on load  Handles operators_label.Load
    Dim temp_returned As String = ""
    Dim temp_title As String = ""
    Dim selected_dropdown_code As String = ""
    Dim pie_title_section As String = ""
    Dim comp_id As Integer = 0

    operators_label_clicked = ""
    Me.operators_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top' width='50%'><div class='tab_container_div'>"


    Me.operators_label.Text += displayOperatorCompanies("", make_model_name, "B", "", amod_id, 0, "", "")

    Me.operators_label.Text += "</div></td><td width='50%'><div class='tab_container_div'>"

    temp_returned = display_OperatorView_PieChart(make_model_name, "", 2, selected_dropdown_code, comp_id, amod_id)
    temp_title = "COUNTRY SUMMARY"

    'If selected_dropdown_code = "B" Then
    '    pie_title_section = "BUSINESS AIRCRAFT "
    'ElseIf selected_dropdown_code = "C" Then
    '    pie_title_section = "COMMERCIAL AIRCRAFT"
    'ElseIf selected_dropdown_code = "H" Then
    '    pie_title_section = "HELICOPTERS "
    'Else
    '    pie_title_section = "BUSINESS AIRCRAFT "
    'End If

    '  Me.OP_COUNTRY_CHART.Titles.Add(temp_title)
    Me.OP_COUNTRY_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
    Me.OP_COUNTRY_CHART.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "_" & comp_id & "_" & "OP_COUNTRY_CHART.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
    '' the temp returned will hold the list of countries and numbers while the graph will display


    Me.operators_label.Text += "<table id='_outer_table' width='100%'  cellpadding='1' cellspacing='0' valign='top'>" & vbCrLf
    Me.operators_label.Text += "<tr><td align='center'>"
    Me.operators_label.Text += temp_title & "<br><br>"
    Me.operators_label.Text += "<img src='TempFiles/" & amod_id & "_" & comp_id & "_" & "OP_COUNTRY_CHART.jpg'></td></tr><tr><td align='left' valign='top'>"
    Me.operators_label.Text += "<table width='100%' valign='top' cellpadding='0' border='0' cellspacing='0'><tr valign='top' class='aircraft_list'><td align='left' width='50%'><strong>Country</strong></td><td  width='50%' align='right'><strong># of Operators&nbsp;&nbsp;</strong></td></tr>"
    Me.operators_label.Text += "<tr valign='top'><td colspan='2' valign='top'>"
    Me.operators_label.Text += temp_returned
    Me.operators_label.Text += "</td></tr></table>"
    Me.operators_label.Text += "</td></tr></table></div>"
    Me.operators_label.Text += "</td></tr></table>"

    ' Me.operators_label.Text += "</td></tr></table>"


  End Function
  Function submit_location_click() Handles submit_location.ServerClick
    submit_location_click = ""

    get_map_from_text()


  End Function
  Function new_model_selected() Handles new_model.Click
    new_model_selected = ""
    Dim model_selected As String = ""
    'If Not IsPostBack Then
    model_selected = Me.model_cbo.SelectedValue
    Dim model_info As Array = Split(model_selected, "|")
    If Not IsNothing(model_info(0)) Then
      Response.Redirect("view.aspX?amod_id=" & model_info(0))
    End If


    'End If
  End Function

  Function check_pic_exists() As Boolean
    check_pic_exists = False
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    Dim query As String = ""
    Dim counter As Integer = 0
    Try


      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      query = " select amod_picture_exists_flag from aircraft_model where amod_id =" & amod_id
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function check_pic_exists() As Boolean</b><br />" & query

      SqlCommand.CommandText = query
      rs = SqlCommand.ExecuteReader()
      If rs.HasRows Then
        rs.Read()
        If rs("amod_picture_exists_flag") = "Y" Then
          check_pic_exists = True
        Else
          check_pic_exists = False
        End If
      Else
        check_pic_exists = False
      End If
      rs.Close()
      rs = Nothing


    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function


  Function create_make_model_list()
    create_make_model_list = ""
    'I commented this out on 5-4-2012 
    ' with the following instructions
    'Amanda,
    'One change to the model view if possible.
    'Could you make the model selection similar to what we do on the other pages?  Not sure since on the other pages we select multiples and here we only want to select one.
    'Anyways – we maybe could atleast do the following - when you click the question mark we would have the list default to their preference models and when not show all.


    'replacement code.
    clsGeneral.clsGeneral.populate_models(model_cbo, True, Me, Nothing, Master, False)
    model_cbo.Items.Remove(model_cbo.Items.FindByValue("All"))

    'Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    'Dim SqlConn As New System.Data.SqlClient.SqlConnection
    'Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    'Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    'Dim query As String = ""
    'Dim counter As Integer = 0
    'Try


    '    SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

    '    SqlConn.Open()

    '    SqlCommand.Connection = SqlConn
    '    SqlCommand.CommandType = System.Data.CommandType.Text
    '    SqlCommand.CommandTimeout = 60

    '    query = " select distinct amod_make_name, amod_model_name, amod_id from aircraft_model where amod_make_name <> '' "
    '    query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
    '    query = query & " order by amod_make_name asc, amod_model_name asc"

    '    SqlCommand.CommandText = query
    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function create_make_model_list()</b><br />" & query

    '    rs = SqlCommand.ExecuteReader()
    '    If rs.HasRows Then
    '        Do While rs.Read
    '            ' Me.make_model_list.Items.Add("<asp:ListItem Value='" & rs("amod_id") & "' Text='" & rs("amod_make_name") & " / " & rs("amod_model_name") & "'></asp:ListItem>")
    '            Me.make_model_list.Items.Add(New ListItem(rs("amod_make_name") & " / " & rs("amod_model_name"), rs("amod_id")))
    '            ' Me.make_model_list.Items.Add(rs("amod_id"))
    '            '  Me.make_model_list.Attributes.Add(counter, rs("amod_id"))
    '            'Me.make_model_list.Items.Add(System.Web.UI.WebControls.ListItem.FromString("<asp:ListItem Value='" & rs("amod_id") & "' Text='" & rs("amod_make_name") & " / " & rs("amod_model_name") & "'></asp:ListItem>"))
    '            ' Me.make_model_list.Items.Add(System.Web.UI.WebControls.ListItem.FromString("value='7' text='model'"))

    '            counter = counter + 1
    '        Loop
    '    End If
    '    rs.Close()
    '    rs = Nothing


    'Catch ex As Exception
    'Finally
    '    SqlConn.Close()
    '    SqlConn.Dispose()
    'End Try

  End Function

  Function get_map_from_text()
    get_map_from_text = ""
    'Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    'Dim SqlConn As New System.Data.SqlClient.SqlConnection
    'Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    'Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    'Dim query As String = ""
    'Dim airport_name As String = ""
    'Dim city_name As String = ""
    'Dim state_name As String = ""
    'Dim country_name As String = ""
    'Dim lat As String = ""
    'Dim longit As String = ""
    'Dim iata_code As String = ""
    'Dim icao_code As String = ""
    'Dim selected_iata_code As String = ""
    'Dim selected_icao_code As String = ""
    'Dim selected_lat As String = ""
    'Dim selected_long As String = ""
    'Dim temp_ampersand As String = "&"
    'Dim selected_airport_name As String = ""
    'Dim selected_airport_name_long As String = ""
    'Dim counter1 As Integer = 0


    'Try
    '    Me.GMap2.resetMarkers()

    '    If aport_list_drop_down.SelectedIndex > 0 Then
    '        Me.Location.Value = aport_list_drop_down.SelectedItem.Value
    '    End If

    '    SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") ' My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

    '    SqlConn.Open()

    '    SqlCommand.Connection = SqlConn
    '    SqlCommand.CommandType = System.Data.CommandType.Text
    '    SqlCommand.CommandTimeout = 60


    '    query = "select aport_name, aport_city, aport_state, aport_country, aport_latitude_decimal, aport_longitude_decimal, aport_iata_code, aport_icao_code "
    '    query += " from airport "
    '    query += " where aport_name <> '' "
    '    query += " and aport_country <> '' "
    '    query += " and aport_latitude_full <> '' "
    '    query += " and aport_longitude_full <> '' "
    '    query += " and aport_max_runway_length is not null "
    '    query += " and aport_max_runway_length > " & field_length
    '    query += " and aport_country = 'United States' "
    '    '  query += " and aport_iata_code = '" & Me.Location.Value & "'"

    '    query += " order by aport_name asc"


    '    SqlCommand.CommandText = query
    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function get_map_from_text()</b><br />" & query

    '    rs = SqlCommand.ExecuteReader()
    '    If rs.HasRows Then
    '        Do While rs.Read

    '            If Not IsDBNull(rs("aport_name")) And Not IsDBNull(rs("aport_country")) And Not IsDBNull(rs("aport_iata_code")) Then
    '                Me.aport_list_drop_down.Items.Add(New ListItem(rs("aport_name") & "(" & rs("aport_country") & ")", rs("aport_iata_code")))
    '            End If

    '            If Not IsDBNull(rs("aport_name")) Then
    '                airport_name = rs("aport_name")
    '            End If

    '            If Not IsDBNull(rs("aport_city")) Then
    '                city_name = rs("aport_city")
    '            End If

    '            If Not IsDBNull(rs("aport_state")) Then
    '                state_name = rs("aport_state")
    '            End If

    '            If Not IsDBNull(rs("aport_country")) Then
    '                country_name = rs("aport_country")
    '            End If

    '            If Not IsDBNull(rs("aport_latitude_decimal")) Then
    '                lat = rs("aport_latitude_decimal")
    '            End If

    '            If Not IsDBNull(rs("aport_longitude_decimal")) Then
    '                longit = rs("aport_longitude_decimal")
    '            End If

    '            If Not IsDBNull(rs("aport_iata_code")) Then
    '                If rs("aport_iata_code") = Me.Location.Value Then

    '                    selected_iata_code = rs("aport_iata_code")
    '                    If Not IsDBNull(rs("aport_latitude_decimal")) Then
    '                        selected_lat = rs("aport_latitude_decimal")
    '                        selected_airport_name = airport_name
    '                        selected_airport_name_long = airport_name
    '                    End If
    '                    If Not IsDBNull(rs("aport_longitude_decimal")) Then
    '                        selected_long = rs("aport_longitude_decimal")
    '                        selected_airport_name = airport_name
    '                        selected_airport_name_long = airport_name
    '                    End If

    '                    If Not IsDBNull(rs("aport_city")) Then
    '                        selected_airport_name_long += "<br>" & rs("aport_city")
    '                    End If

    '                    If Not IsDBNull(rs("aport_state")) Then
    '                        selected_airport_name_long += "," & rs("aport_state")
    '                    End If

    '                    If Not IsDBNull(rs("aport_country")) Then
    '                        selected_airport_name_long += "<br>" & rs("aport_country")
    '                    End If


    '                Else
    '                    iata_code = rs("aport_iata_code")
    '                    '  map_each_point(iata_code)
    '                End If
    '            End If

    '            If Not IsDBNull(rs("aport_icao_code")) Then
    '                If rs("aport_icao_code") = Me.Location.Value Then
    '                    selected_icao_code = rs("aport_icao_code")
    '                    If Not IsDBNull(rs("aport_latitude_decimal")) Then
    '                        selected_lat = rs("aport_latitude_decimal")
    '                        selected_airport_name = airport_name
    '                        selected_airport_name_long = airport_name
    '                    End If
    '                    If Not IsDBNull(rs("aport_longitude_decimal")) Then
    '                        selected_long = rs("aport_longitude_decimal")
    '                        selected_airport_name = airport_name
    '                        selected_airport_name_long = airport_name
    '                    End If

    '                    If Not IsDBNull(rs("aport_city")) Then
    '                        selected_airport_name_long += "<br>" & rs("aport_city")
    '                    End If

    '                    If Not IsDBNull(rs("aport_state")) Then
    '                        selected_airport_name_long += "," & rs("aport_state")
    '                    End If

    '                    If Not IsDBNull(rs("aport_country")) Then
    '                        selected_airport_name_long += "<br>" & rs("aport_country")
    '                    End If

    '                Else
    '                    icao_code = rs("aport_icao_code")
    '                End If

    '            End If


    '            counter1 += 1

    '        Loop
    '    End If
    '    rs.Close()
    '    rs = Nothing


    '    If selected_lat <> "" Or selected_long <> "" Then
    '        Dim sMapKey As String = "ABQIAAAAaDQFN4EUkGBPHcvvi9xYphQxsRTReCzalNJnrTGVEk6OBfqksBRqk-cj1l4naoUFcfco6c6BSbLIAg"

    '        Dim lat_and_long As New Subgurim.Controles.GLatLng

    '        lat_and_long.lat = CDbl(selected_lat)
    '        lat_and_long.lng = CDbl(selected_long)


    '        Dim link As String = "http://maps.google.com/maps/api/geocode/xml?address=1600+Pennsylvania+Ave,+Washington+D.C.&sensor=false&radius=1000"
    '        ' link = "https://maps.googleapis.com/maps/api/geocode/json?latlng="
    '        ' link += selected_lat & "," & selected_long
    '        '  link += temp_ampersand & "sensor=false"
    '        '  link += temp_ampersand & "key=ABQIAAAAaDQFN4EUkGBPHcvvi9xYphQxsRTReCzalNJnrTGVEk6OBfqksBRqk-cj1l4naoUFcfco6c6BSbLIAg"
    '        ' link += temp_ampersand & "radius=10000"

    '        'Dim GeoCode As New Subgurim.Controles.GeoCode
    '        ' GeoCode = Subgurim.Controles.GMap.geoCodeRequest(link, sMapKey)
    '        ' GeoCode.Placemark.coordinates.lat = selected_lat
    '        '  GeoCode.Placemark.coordinates.lng = selected_long


    '        ' Dim geo_xml As New Subgurim.Controles.GGeoXml
    '        ' GMap2.addGGeoXML(New Subgurim.Controles.GGeoXml("http://maps.google.com/maps/api/geocode/xml?address=1600+Pennsylvania+Ave,+Washington+D.C.&sensor=false&radius=1000"))
    '        ''geo_xml.Url = link 
    '        ''   GMap2.addGGeoXML(geo_xml)

    '        'Dim Str As System.IO.Stream
    '        'Dim srRead As System.IO.StreamReader
    '        'Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(link)
    '        'Dim resp As System.Net.WebResponse = req.GetResponse
    '        'Dim string_text As String = ""
    '        'Dim spot_to_find As Integer = 0
    '        'Dim spot_to_find2 As Integer = 0
    '        'Dim array_split() As String
    '        'Dim i As Integer = 0


    '        'Str = resp.GetResponseStream
    '        'srRead = New System.IO.StreamReader(Str)
    '        '' read all the text 
    '        'string_text = srRead.ReadToEnd().ToString
    '        'string_text = string_text
    '        'geo_xml.Url = string_text

    '        'GMap2.Add(geo_xml)
    '        'link = link






    '        'Dim marker1 As New Subgurim.Controles.GMarker
    '        'Dim geo_code As New Subgurim.Controles.GeoCode

    '        'geo_code.Placemark.coordinates.lat = CDbl(selected_lat)
    '        'geo_code.Placemark.coordinates.lng = CDbl(selected_long)

    '        'Dim temp_string = GeoCode.Placemark.coordinates.optionalStringValue



    '        '' --------- THIS SETS THE CENTER FOR THE MAP---------------------------
    '        'Dim gLatLng As New Subgurim.Controles.GLatLng(GeoCode.Placemark.coordinates.lat, GeoCode.Placemark.coordinates.lng)
    '        'GMap2.setCenter(gLatLng)

    '        Dim gLatLng As New Subgurim.Controles.GLatLng(selected_lat, selected_long)
    '        Dim gpoint As New Subgurim.Controles.GPoint(selected_lat, selected_long)


    '        'Dim radius_circle As New CircleHotSpot
    '        'radius_circle.Radius = 4000
    '        'radius_circle.X = selected_lat
    '        'radius_circle.Y = selected_long




    '        'Dim yy As New Subgurim.Controles.GIcon
    '        'Dim zz As New Subgurim.Controles.GSize
    '        'zz.height = 1000
    '        'zz.width = 1000
    '        '' yy.flatIconOptions.flatIconShapeEnum = Subgurim.Controles.FlatIconOptions.flatIconShapeEnum.circle()
    '        ''  Subgurim.Controles.FlatIconOptions.flatIconShapeEnum.circle()
    '        'yy.iconAnchor = gpoint 
    '        'yy.iconSize = zz
    '        'GMap2.Add(xxx)





    '        ''''''------------------------------------------------------------- SQUARE using points--------------------------------
    '        'Dim point11 As New Subgurim.Controles.GLatLng(selected_lat - 5, selected_long)
    '        'Dim point22 As New Subgurim.Controles.GLatLng(selected_lat, selected_long - 7)
    '        'Dim point33 As New Subgurim.Controles.GLatLng(selected_lat + 5, selected_long)
    '        'Dim point44 As New Subgurim.Controles.GLatLng(selected_lat, selected_long + 7)
    '        'Dim range As Integer = 3000


    '        'Dim point_poly As New Subgurim.Controles.GPolygon
    '        ''point_poly.Add(point11)
    '        ''point_poly.Add(point22)
    '        ''point_poly.Add(point33)
    '        ''point_poly.Add(point44)

    '        'Dim collect As New System.Collections.Generic.List(Of Subgurim.Controles.GLatLng)
    '        'collect.Add(point11)
    '        'collect.Add(point22)
    '        'collect.Add(point33)
    '        'collect.Add(point44)

    '        'point_poly.points.AddRange(collect)

    '        'point_poly.close()

    '        'GMap2.addPolygon(point_poly)
    '        ''''''------------------------------------------------------------- SQUARE using points--------------------------------

    '        ''''---- some points that didnt work-----------
    '        ''''   Dim glantlong2test As New Subgurim.Controles.GLatLng
    '        'glantlong2test.lat = (selected_lat + range / 2 * 90 / 10000000)
    '        'glantlong2test.lng = (selected_long + range / 2 * 90 / 10000000 / Math.Cos(selected_lat))
    '        'collect.Add(glantlong2test)
    '        'glantlong2test.lat = (selected_lat - range / 2 * 90 / 10000000)
    '        'glantlong2test.lng = (selected_long + range / 2 * 90 / 10000000 / Math.Cos(selected_lat))
    '        'collect.Add(glantlong2test)
    '        'glantlong2test.lat = (selected_lat - range / 2 * 90 / 10000000)
    '        'glantlong2test.lng = (selected_long - range / 2 * 90 / 10000000 / Math.Cos(selected_lat))
    '        'collect.Add(glantlong2test)
    '        'glantlong2test.lat = (selected_lat + range / 2 * 90 / 10000000)
    '        'glantlong2test.lng = (selected_long - range / 2 * 90 / 10000000 / Math.Cos(selected_lat))
    '        'collect.Add(glantlong2test)
    '        'glantlong2test.lat = (selected_lat + range / 2 * 90 / 10000000)
    '        'glantlong2test.lng = (selected_long + range / 2 * 90 / 10000000 / Math.Cos(selected_lat))
    '        'collect.Add(glantlong2test)
    '        ''''---- some points that didnt work-----------



    '        ' '------------------------------------------------------------- from site--------------------------------
    '        'Dim poligono As New Subgurim.Controles.GPolygon
    '        'Dim puntos As New System.Collections.Generic.List(Of Subgurim.Controles.GLatLng)
    '        'puntos.Add(gLatLng + New Subgurim.Controles.GLatLng(0, 8))
    '        'puntos.Add(gLatLng + New Subgurim.Controles.GLatLng(-0.5, 4.2))
    '        'puntos.Add(gLatLng)
    '        'puntos.Add(gLatLng + New Subgurim.Controles.GLatLng(3.5, -4))
    '        'puntos.Add(gLatLng + New Subgurim.Controles.GLatLng(4.79, +2.6))
    '        'poligono = New Subgurim.Controles.GPolygon(puntos, "557799", 3, 0.5, "237464", 0.5)
    '        'poligono.close()
    '        'GMap2.Add(poligono)
    '        ' '-------------------------------------------------------------  from site--------------------------------





    '        'Dim micon As New Subgurim.Controles.GMarker

    '        'Dim tester As New Subgurim.Controles.MarkerIconOptions
    '        'tester.width = 1000
    '        'tester.height = 1000
    '        'tester.strokeColor = Drawing.Color.AliceBlue
    '        'tester.primaryColor = Drawing.Color.AliceBlue
    '        'tester.cornerColor = Drawing.Color.AliceBlue

    '        'micon.options. = tester
    '        'micon.point = gLatLng
    '        'GMap2.Add(micon)

    '        ' '-------------------------------------------------------------  nice polyine with small angle--------------------------------
    '        'Dim polyline As New Subgurim.Controles.GPolyline 
    '        'Dim glat2 As New Subgurim.Controles.GLatLng
    '        'glat2.lat = gLatLng.lat + 10
    '        'glat2.lng = gLatLng.lng + 10

    '        'Dim glat3 As New Subgurim.Controles.GLatLng
    '        'glat3.lat = gLatLng.lat - 10
    '        'glat3.lng = gLatLng.lng - 10

    '        'polyline.geodesic = True
    '        'polyline.points.Add(glat2)
    '        'polyline.points.Add(glat3) 

    '        'polyline.weight = 3
    '        'GMap2.addPolyline(polyline) 
    '        ' '------------------------------------------------------------- nice polyine with small angle--------------------------------



    '        ' '-------------------------------------------------------------  nice polygon--------------------------------

    '        'Dim poly As New Subgurim.Controles.GPolygon

    '        'poly.createPolygon(gLatLng, 360, 4)

    '        'poly.fillColor = "#2E2EFE"
    '        'poly.strokeColor = "#2E2EFE"
    '        'poly.fillOpacity = 0.3
    '        'poly.strokeOpacity = 0.3
    '        'poly.strokeWeight = 0

    '        'GMap2.addPolygon(poly)


    '        'Dim poly2 As New Subgurim.Controles.GPolygon
    '        'poly2.createPolygon(gLatLng, 360, 3)
    '        'poly2.fillColor = "#FA5858"
    '        'poly2.strokeColor = "#FA5858"
    '        'poly2.fillOpacity = 0.3
    '        'poly2.strokeOpacity = 0.3
    '        'GMap2.addPolygon(poly2)
    '        Dim radius As Double = range_constant / 100 ' 9


    '        Dim l As New Subgurim.Controles.GLatLng
    '        l = GMap2.GCenter
    '        Dim sw As New Subgurim.Controles.GLatLng
    '        Dim ne As New Subgurim.Controles.GLatLng
    '        ne = New Subgurim.Controles.GLatLng(l.lat, l.lng)
    '        sw = New Subgurim.Controles.GLatLng(l.lat, l.lng)
    '        'range_label.Text = range_constant

    '        Dim latlngbounds As New Subgurim.Controles.GLatLngBounds
    '        latlngbounds = New Subgurim.Controles.GLatLngBounds(ne, sw)



    '        Dim latlng As New Subgurim.Controles.GLatLng(selected_lat, selected_long)

    '        Dim Mar As Double = Math.PI / 180
    '        Dim puntos As New List(Of Subgurim.Controles.GLatLng)()
    '        'this has to be changed to the bounds.getnortheast - southwest/ northeast - southwest but I'm not sure how to get the bounds marker of the map
    '        ' Dim circleSquish As Double = 0.2 'selected_lat - selected_long / selected_lat - selected_long '(latlngbounds.getNorthEast() - latlngbounds.getSouthWest()) / (latlngbounds.getNorthEast() - latlngbounds.getSouthWest())

    '        For x As Integer = 0 To 360
    '            Dim q As Double = selected_lat + (radius * Math.Sin(x * Mar))
    '            Dim z As Double = selected_long + (radius * Math.Cos(x * Mar))
    '            puntos.Add(New Subgurim.Controles.GLatLng(selected_lat + (radius * Math.Sin(x * Mar)), selected_long + (radius * Math.Cos(x * Mar))))
    '        Next

    '        Dim poligono As New Subgurim.Controles.GPolygon(puntos, "ff0000", 3, 0.5, "ff0000", 0.5)

    '        poligono.close()
    '        'GMap2.Add(poligono)

    '        ' Me.GMap2.GCenter.lat = CDbl(selected_lat)
    '        'Me.GMap2.GCenter.lng = CDbl(selected_long)

    '        ' '-------------------------------------------------------------   nice polygon -------------------------------


    '        '' '-------------------------------------------------------------   nice Circle icon section -------------------------------
    '        'Dim flat_icon_options As New Subgurim.Controles.FlatIconOptions
    '        'Dim flat_icon As New Subgurim.Controles.GIcon
    '        'Dim flat_icon_marker As New Subgurim.Controles.GMarker
    '        ''    Dim flat_icon_g_size As New Subgurim.Controles.GSize
    '        'Dim gmarker_options As New Subgurim.Controles.GMarkerOptions


    '        ''   flat_icon_g_size.height = 100
    '        ''  flat_icon_g_size.width = 100


    '        'flat_icon_options.shape = Subgurim.Controles.FlatIconOptions.flatIconShapeEnum.circle
    '        'flat_icon_options.width = 100
    '        'flat_icon_options.height = 100

    '        ''  flat_icon.iconSize = flat_icon_g_size
    '        'flat_icon.flatIconOptions = flat_icon_options
    '        'flat_icon.iconAnchor = gpoint


    '        'gmarker_options.icon = flat_icon

    '        'flat_icon_marker.options = gmarker_options

    '        'GMap2.Add(flat_icon_marker)
    '        '' '-------------------------------------------------------------    nice Circle icon section -------------------------------

    '        ' Dim test1 As New CircleHotSpot
    '        ' test1.X = selected_lat
    '        ' test1.Y = selected_long
    '        ' test1.Radius = 5 
    '        'Dim overlay As New Subgurim.Controles.GIcon
    '        'Dim overlay_options As New Subgurim.Controles.FlatIconOptions
    '        'Dim point As New Subgurim.Controles.GPoint(selected_lat, selected_long)

    '        'overlay_options.shape = Subgurim.Controles.FlatIconOptions.flatIconShapeEnum.circle
    '        'overlay_options.width = 500
    '        'overlay_options.height = 500
    '        'overlay.iconAnchor = point
    '        'overlay.flatIconOptions = overlay_options
    '        'overlay.ID = "C1"



    '        ' --------- THIS SETS THE CENTER FOR THE MAP---------------------------




    '        ' --------- THIS SETS UP THE MARKER--------------------------


    '        'Dim oOption2 As New Subgurim.Controles.GMarkerOptions
    '        'oOption2.clickable = True
    '        'oOption2.title = selected_airport_name
    '        'oOption2.draggable = False

    '        'Dim info_w As New Subgurim.Controles.GInfoWindow(gLatLng, selected_airport_name_long)

    '        'Dim oMarker2 As New Subgurim.Controles.GMarker(gLatLng, oOption2)
    '        'GMap2.addGMarker(oMarker2)

    '        GMap2.setCenter(gLatLng)

    '        'GMap2.addInfoWindow(info_w)

    '        'Dim oOption As New Subgurim.Controles.LabeledMarkerOptions
    '        'oOption.clickable = True
    '        'oOption.title = selected_airport_name
    '        'oOption.draggable = False


    '        'Dim oMarker As New Subgurim.Controles.LabeledMarker(gLatLng, oOption)
    '        'GMap2.addGMarker(oMarker)
    '        ' --------- THIS SETS UP THE MARKER--------------------------
    '        '  Me.GMap2.GCenter.lat = CDbl(selected_lat)
    '        '  Me.GMap2.GCenter.lng = CDbl(selected_long)
    '    End If

    '    Me.GMap2.addGMapUI(New Subgurim.Controles.GMapUI)
    '    Me.GMap2.enableHookMouseWheelToZoom = True
    '    Me.GMap2.Key = "ABQIAAAAaDQFN4EUkGBPHcvvi9xYphQxsRTReCzalNJnrTGVEk6OBfqksBRqk-cj1l4naoUFcfco6c6BSbLIAg"

    '    '  Me.GMap2.addGMarker(marker1)


    '    'Me.GMap2.addMapType(Subgurim.Controles.GMapType.GTypes.Hybrid)
    '    '  Me.GMap2.Attributes.Add(1, "here")
    '    ' Me.GMap2.addGroundOverlay(New Subgurim.Controles.GMarker(New Subgurim.Controles.GLatLng(CDbl(selected_lat), CDbl(selected_long)))) 

    ''Catch ex As Exception
    ''    ex = ex
    ''Finally
    ''SqlConn.Close()
    ''SqlConn.Dispose()
    'End Try

  End Function

  'Function map_each_point(ByVal Temp_Location As String)
  '    map_each_point = ""
  '    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
  '    Dim SqlConn As New System.Data.SqlClient.SqlConnection
  '    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
  '    Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
  '    Dim query As String = ""
  '    Dim airport_name As String = ""
  '    Dim city_name As String = ""
  '    Dim state_name As String = ""
  '    Dim country_name As String = ""
  '    Dim lat As String = ""
  '    Dim longit As String = ""
  '    Dim iata_code As String = ""
  '    Dim icao_code As String = ""
  '    Dim selected_iata_code As String = ""
  '    Dim selected_icao_code As String = ""
  '    Dim selected_lat As String = ""
  '    Dim selected_long As String = ""
  '    Dim temp_ampersand As String = "&"
  '    Dim selected_airport_name As String = ""

  '    Try

  '        If aport_list_drop_down.SelectedIndex > 0 Then
  '            Me.Location.Value = aport_list_drop_down.SelectedItem.Value
  '        End If

  '        SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

  '        SqlConn.Open()

  '        SqlCommand.Connection = SqlConn
  '        SqlCommand.CommandType = System.Data.CommandType.Text
  '        SqlCommand.CommandTimeout = 60


  '        query = "select aport_name, aport_city, aport_state, aport_country, aport_latitude_decimal, aport_longitude_decimal, aport_iata_code, aport_icao_code "
  '        query += " from airport "
  '        query += " where aport_name <> '' "
  '        query += " and aport_country <> '' "
  '        query += " and aport_latitude_full <> '' "
  '        query += " and aport_longitude_full <> '' "
  '        query += " and aport_max_runway_length is not null "
  '        ' query += " and aport_max_runway_length > " & field_length
  '        query += " and aport_iata_code = '" & Temp_Location & "'"

  '        query += " order by aport_name asc"

  '        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function map_each_point(ByVal Temp_Location As String)</b><br />" & query

  '        SqlCommand.CommandText = query
  '        rs = SqlCommand.ExecuteReader()
  '        If rs.HasRows Then
  '            Do While rs.Read


  '                If Not IsDBNull(rs("aport_iata_code")) Then
  '                    If rs("aport_iata_code") = Temp_Location Then

  '                        selected_iata_code = rs("aport_iata_code")
  '                        If Not IsDBNull(rs("aport_latitude_decimal")) Then
  '                            selected_lat = rs("aport_latitude_decimal")
  '                            selected_airport_name = airport_name
  '                        End If
  '                        If Not IsDBNull(rs("aport_longitude_decimal")) Then
  '                            selected_long = rs("aport_longitude_decimal")
  '                            selected_airport_name = airport_name
  '                        End If

  '                    Else
  '                        iata_code = rs("aport_iata_code")
  '                    End If
  '                End If

  '                If Not IsDBNull(rs("aport_icao_code")) Then
  '                    If rs("aport_icao_code") = Temp_Location Then
  '                        selected_icao_code = rs("aport_icao_code")
  '                        If Not IsDBNull(rs("aport_latitude_decimal")) Then
  '                            selected_lat = rs("aport_latitude_decimal")
  '                            selected_airport_name = airport_name
  '                        End If
  '                        If Not IsDBNull(rs("aport_longitude_decimal")) Then
  '                            selected_long = rs("aport_longitude_decimal")
  '                            selected_airport_name = airport_name
  '                        End If

  '                    Else
  '                        icao_code = rs("aport_icao_code")
  '                    End If

  '                End If

  '            Loop
  '        End If
  '        rs.Close()
  '        rs = Nothing


  '        If selected_lat <> "" Or selected_long <> "" Then
  '            Dim sMapKey As String = "ABQIAAAAaDQFN4EUkGBPHcvvi9xYphQxsRTReCzalNJnrTGVEk6OBfqksBRqk-cj1l4naoUFcfco6c6BSbLIAg"
  '            Dim gLatLng As New Subgurim.Controles.GLatLng(selected_lat, selected_long)
  '            Dim oOption2 As New Subgurim.Controles.GMarkerOptions
  '            oOption2.title = selected_airport_name
  '            oOption2.draggable = False 

  '            Dim oMarker2 As New Subgurim.Controles.GMarker(gLatLng, oOption2)
  '            ' GMap2.addGMarker(oMarker2)


  '            '  Dim glistener As New Subgurim.Controles.GListener(oMarker2.ID, Subgurim.Controles.GListener.Event.click, labeler2())
  '            '  GMap2.addListener(glistener)

  '        End If

  '    Catch ex As Exception
  '        ex = ex
  '    Finally
  '        SqlConn.Close()
  '        SqlConn.Dispose()
  '    End Try

  'End Function

  'Function labeler(ByVal marker As Subgurim.Controles.GMarker, ByVal text As String, ByVal lat_long As Subgurim.Controles.GLatLng)
  '    labeler = ""
  '    marker.options.title = ""
  '    Dim info_w As New Subgurim.Controles.GInfoWindow(lat_long, text)

  '    'GMap2.addInfoWindow(info_w)
  'End Function



  Function display_market_up_down_one_model(ByVal table_height As Integer, ByVal make_model_name As String, ByVal amod_id As Integer, ByVal marketViewTimeSpan As Integer, ByVal airframe_type As String, ByVal product_codes As String)

    Dim last_year_diff As Integer = 0
    Dim last_month_diff As Integer = 0
    Dim last_year_percentage As Double = 0
    Dim last_month_percentage As Double = 0
    Dim nMonthOffset As Integer = 0
    Dim query As String = ""
    Dim outstring As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    Dim currentDate_Month As Integer = 0
    Dim currentDate_Year As Integer = 0
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      nMonthOffset = -1
      display_market_up_down_one_model = ""

      ' check for missing data

      query = "SELECT SUM(mtrend_total_aircraft_for_sale) AS pastmonthforsale FROM Aircraft_Model_Trend WITH(NOLOCK)"
      query = query & " INNER JOIN Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id"
      query = query & " WHERE (mtrend_year = " & Year(Now) & " AND mtrend_month = " & Month(DateAdd("m", -1, Now)) & ")"

      SqlCommand.CommandText = query
      rs = SqlCommand.ExecuteReader()
      If rs.HasRows Then
        rs.Read()
        If IsDBNull(rs("pastmonthforsale")) Then
          nMonthOffset = -2
        End If
      End If
      rs.Close()
      rs = Nothing

      query = "SELECT amod_make_name, amod_model_name, amod_id, count(*) as currentforsale,"
      query = query & "(select SUM(mtrend_total_aircraft_for_sale) FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN"
      query = query & " Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id"

      If amod_id > 0 Then
        query = query & " AND amod_id = " & amod_id
      ElseIf Trim(airframe_type) <> "" Then
        query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
      End If

      'If product_codes.ToString.Trim <> "" Then
      '    If product_codes.ToString.Trim = "B" Then
      '        query = query & " AND amod_product_business_flag='Y' "
      '    ElseIf product_codes.ToString.Trim = "C" Then
      '        query = query & " AND amod_product_commercial_flag='Y' "
      '    ElseIf product_codes.ToString.Trim = "H" Then
      '        query = query & " AND amod_product_helicopter_flag='Y' "
      '    End If
      'End If


      currentDate_Month = Month(DateAdd("m", (-1) * marketViewTimeSpan, Now))
      currentDate_Year = Year(DateAdd("m", (-1) * marketViewTimeSpan, Now))

      query = query & " WHERE (amod_make_name = a.amod_make_name AND amod_model_name = a.amod_model_name) AND (mtrend_year = " & currentDate_Year & ") AND (mtrend_month = " & currentDate_Month & ")) AS pastyearforsale,"

      query = query & " (select SUM(mtrend_total_aircraft_for_sale) FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN"
      query = query & " Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id"


      If amod_id > 0 Then
        query = query & " AND amod_id = " & amod_id
      ElseIf Trim(airframe_type) <> "" Then
        query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
      End If


      'If product_codes.ToString.Trim <> "" Then
      '    If product_codes.ToString.Trim = "B" Then
      '        query = query & " AND amod_airframe_type_code='F' "
      '    ElseIf product_codes.ToString.Trim = "H" Then
      '        query = query & " AND amod_airframe_type_code='R' "
      '    End If
      'End If


      currentDate_Month = Month(DateAdd("m", nMonthOffset, Now))
      currentDate_Year = Year(DateAdd("m", nMonthOffset, Now))

      query = query & " WHERE (amod_make_name = a.amod_make_name AND amod_model_name = a.amod_model_name) AND (mtrend_year = " & currentDate_Year & ") AND (mtrend_month = " & currentDate_Month & ")) AS pastmonthforsale"

      query = query & " FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model AS A WITH(NOLOCK) ON ac_amod_id = amod_id"
      query = query & " WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0"

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      If amod_id > 0 Then
        query = query & " AND amod_id = " & amod_id
      ElseIf Trim(airframe_type) <> "" Then
        query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
      End If


      'If product_codes.ToString.Trim <> "" Then
      '    If product_codes.ToString.Trim = "B" Then
      '        query = query & " AND amod_airframe_type_code='F' "
      '    ElseIf product_codes.ToString.Trim = "H" Then
      '        query = query & " AND amod_airframe_type_code='R' "
      '    End If
      'End If


      query = query & " GROUP BY amod_make_name, amod_model_name, amod_id ORDER BY amod_make_name, amod_model_name, amod_id"


      'If Session("debug") Then
      'Session.Item("localUser").crmUser_DebugText += "<b>display_market_up_down_one_model:" & Server.HtmlEncode(query) & "</b><br /><br />"
      '  End If

      SqlCommand.CommandText = query

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_market_up_down_one_model(ByVal table_height As Integer, ByVal make_model_name As String, ByVal amod_id As Integer, ByVal marketViewTimeSpan As Integer, ByVal airframe_type As String, ByVal product_codes As String)</b><br />" & query

      rs = SqlCommand.ExecuteReader()


      outstring = "<table width='100%' align='left' cellpadding='1' cellspacing='0' >" & vbCrLf
      ' outstring = outstring & "<tr class='aircraft_list'><td valign='middle' align='left' class='header' style='padding-left:3px; height:24px;'>AIRCRAFT FOR SALE MARKET TRENDS FOR MODEL</td>"
      ' outstring = outstring & "<td valign='middle' align='right' class='header' style='padding-left:3px; height:24px;'>" & marketViewTimeSpan & " MONTHS&nbsp;</td></tr>" & vbCrLf

      outstring = outstring & "<tr><td valign='top' align='left' colspan='2'><table width='100%' cellpadding='1' cellspacing='0'><tr>" & vbCrLf
      ' outstring = outstring & "<tr><td valign='top' align='left' class='seperator' width='40%'><strong>Make</strong></td>" & vbCrLf
      outstring = outstring & "<td valign='top' align='right'>&nbsp;</td><td>&nbsp;</td>"
      outstring = outstring & "<td valign='top' align='right' class='seperator' width='27%'><strong>Last Month +/-</strong></td>" & vbCrLf

      If CInt(marketViewTimeSpan) = 6 Then
        outstring = outstring & "<td valign='bottom' align='center'  width='30%'><strong>Last Six Months +/-</strong></td><tr>" & vbCrLf
      End If

      If CInt(marketViewTimeSpan) = 12 Then
        outstring = outstring & "<td valign='bottom' align='center' width='30%'><strong>Last Year +/-</strong></td><tr>" & vbCrLf
      End If

      outstring = outstring & "<tr><td colspan='4' class='rightside'>" & vbCrLf
      outstring = outstring & "<table width='100%' cellpadding='4' cellspacing='0'>" & vbCrLf

      If rs.HasRows Then

        Do While rs.Read


          Session("sum_current_for_sale") = FormatNumber(rs("currentforsale"), 0, True, False, True)
          outstring = outstring & "<tr>"
          'outstring = outstring & "<td valign='top' align='left' class='seperator' title='" & Trim(rs("amod_make_name")) & "&nbsp;/&nbsp;" & Trim(rs("amod_model_name")) & "' width='40%'>"
          ' outstring = outstring & "" & Trim(rs("amod_make_name")) & "&nbsp;/&nbsp;" & Trim(rs("amod_model_name")) & "</td>" & vbCrLf
          ' outstring = outstring & "<td valign='top' align='right' class='seperator' title='" & Trim(rs("amod_make_name")) & "&nbsp;/&nbsp;" & Trim(rs("amod_model_name")) & "' width='40%'>" & CStr(FormatNumber(rs("currentforsale"), 0, True, False, True)) & "</td>" & vbCrLf
          outstring = outstring & "<td valign='top' align='left' class='seperator'><strong>For Sale</strong></td><td align='center''>" & CStr(FormatNumber(rs("currentforsale"), 0, True, False, True)) & string_for_op_percentage & "</td>" & vbCrLf
          last_year_diff = 0
          last_year_percentage = 0

          If Not IsDBNull(rs("pastyearforsale")) Then
            If CDbl(rs("pastyearforsale")) > 0 Then
              last_year_diff = CDbl(CDbl(rs("currentforsale")) - CDbl(rs("pastyearforsale")))
              last_year_percentage = CDbl(last_year_diff / CDbl(rs("pastyearforsale")))
              last_year_percentage = CDbl(last_year_percentage * 100)
              last_year_percentage = CDbl(last_year_percentage) ' was a round , 1 
            End If
          End If

          last_month_diff = 0
          last_month_percentage = 0

          If Not IsDBNull(rs("pastmonthforsale")) Then
            If CDbl(rs("pastmonthforsale")) > 0 Then
              last_month_diff = CDbl(CDbl(rs("currentforsale")) - CDbl(rs("pastmonthforsale")))
              last_month_percentage = CDbl(last_month_diff / CDbl(rs("pastmonthforsale")))
              last_month_percentage = CDbl(last_month_percentage * 100)
              last_month_percentage = CDbl(last_month_percentage) ' was a round , 1 
            End If
          End If

          If last_month_diff = 0 Then
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='No Change' width='27%'><img align='center' src='../images/gain_loss_none.jpg'>&nbsp;&nbsp;" & last_month_diff & " (" & FormatNumber(last_month_percentage, 1) & "%)</td>" & vbCrLf
          ElseIf last_month_diff < 0 Then
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='Net Loss' width='27%'><img align='center' src='../images/gain_loss_down.jpg'>&nbsp;&nbsp;" & last_month_diff & " (" & FormatNumber(last_month_percentage, 1) & "%)</td>" & vbCrLf
          Else
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='Net Gain' width='27%'><img align='center' src='../images/gain_loss_up.jpg'>&nbsp;&nbsp;" & last_month_diff & " (" & FormatNumber(last_month_percentage, 1) & "%)</td>" & vbCrLf
          End If

          If last_year_diff = 0 Then
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='No Change'  width='30%'><img align='center' src='../images/gain_loss_none.jpg'>&nbsp;&nbsp;" & last_year_diff & " (" & FormatNumber(last_year_percentage, 1) & "%)</td></tr>" & vbCrLf
          ElseIf last_year_diff < 0 Then
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='Net Loss' width='30%'><img align='center' src='../images/gain_loss_down.jpg'>&nbsp;&nbsp;" & last_year_diff & " (" & FormatNumber(last_year_percentage, 1) & "%)</td></tr>" & vbCrLf
          Else
            outstring = outstring & "<td valign='top' align='right' class='seperator' title='Net Gain'  width='30%'><img align='center' src='../images/gain_loss_up.jpg'>&nbsp;&nbsp;" & last_year_diff & " (" & FormatNumber(last_year_percentage, 1) & "%)</td></tr>" & vbCrLf
          End If


        Loop
        rs.Close()
      Else
        outstring = outstring & "<tr><td valign='top' align='left' class='seperator'>No data matches for your search criteria</td></tr>"
      End If

      outstring = outstring & "</table></td></tr></table>"

      outstring = outstring & "</td></tr></table>"
      display_market_up_down_one_model = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  Function display_transaction_documents(ByVal model_id As Integer, ByVal table_height As Integer, ByVal city As String, ByVal country As String, ByVal in_HeaderText As String, ByVal comp_id As Integer, ByVal make_model_name As String, ByVal months_count As Integer, ByVal sub_info As String, ByVal real_company_name As String, ByVal sub_type As String, ByVal make_name As String, ByVal make_model_or_make_for_title As String, ByVal product_codes As String, ByVal airframe_type As String)
    display_transaction_documents = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim query As String = ""
    Dim outstring As String = ""
    Dim total_counter As Integer = 0
    Dim QUOTE As String = "&quot;"
    Dim sTmpStr As String = ""
    Dim first_output_section As String = ""
    Dim break_counter As Integer = 0
    Dim temp_first As String = ""
    Dim cut_spot_for_page_break As Integer = 15
    Dim display_for_date_range As String = ""
    Dim row_count As Integer = 0
    Dim record_count As Integer = 0
    Try
      display_for_date_range = "From: " & Month(DateAdd("m", (-1) * months_count, Now)) & "/01/" & Year(DateAdd("m", (-1) * months_count, Now)) & " Up to: " & Month(Now) & "/01/" & Year(Now)


      first_output_section = "<table id='displayTransactionDocumentsOuterTable' width='100%'  cellpadding='1' cellspacing='0'>"
      first_output_section = first_output_section & "<tr class='aircraft_list'><td valign='top' align='left' style='padding-left:3px;'><table width='100%'><tr><td width='50%'>LATEST FINANCIAL DOCUMENTS (TOTALCOUNTER)</td><td align='right' width='50%'>" & display_for_date_range & "</td></tr></table></td></tr>"
      first_output_section = first_output_section & "<tr><td valign='top' align='left'><table id='displayTransactionDocumentsInnerTable' width='100%' cellpadding='1' cellspacing='0' >"
      temp_first = first_output_section

      If model_id > 0 Or country <> "" Then
        cut_spot_for_page_break = 10
        'first_output_section = first_output_section & "<tr><td valign='top' align='left' class='tabheader'><strong>"
        ' first_output_section = first_output_section & "<br>" & display_for_date_range & "<br>" & in_HeaderText & "</strong></td><td class='border_bottom' width='20%'>&nbsp;</td></tr>"
      ElseIf make_name <> "" Then
        cut_spot_for_page_break = 10
        first_output_section = first_output_section & "<tr><td valign='top' align='left' class='tabheader'><strong>"
        If amod_id > 0 Then
          first_output_section = first_output_section & make_model_name
        ElseIf Trim(make_name) <> "" Then
          first_output_section = first_output_section & make_name.ToString.ToUpper
        Else
          first_output_section = first_output_section & "ALL MAKES/MODELS"
        End If
        first_output_section = first_output_section & "<br>TOTALCOUNTER Most Recent Financial Documents<br>" & in_HeaderText & "</strong></td><td class='border_bottom' width='20%'>&nbsp;</td></tr>"
      Else
        cut_spot_for_page_break = 10
        first_output_section = first_output_section & "<tr><td valign='top' align='left' class='tabheader'><strong>"
        If amod_id > 0 Then
          first_output_section = first_output_section & make_model_name
        ElseIf Trim(make_name) <> "" Then
          first_output_section = first_output_section & make_name.ToString.ToUpper
        Else
          first_output_section = first_output_section & "ALL MAKES/MODELS"
        End If
        first_output_section = first_output_section & "<br>50 Most Recent Financial Documents<br>" & in_HeaderText & "</strong></td><td class='border_bottom' width='20%'>&nbsp;</td></tr>"
      End If

      first_output_section = first_output_section & "<tr><td colspan='2' class='rightside'>"
      first_output_section = first_output_section & "<table id='displayTransactionDocumentsDataTable' width='100%' cellpadding='4' cellspacing='0'>"


      If model_id > 0 Or country <> "" Then
        query = "SELECT adoc_doc_date, adoc_doc_type, adoc_journ_id AS journ_id, adoc_journ_seq_no, journ_subject, journ_date, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no, comp_name, comp_city, comp_state, comp_id"
      ElseIf airframe_type <> "" Then
        cut_spot_for_page_break = 15
        query = "SELECT TOP 50 adoc_doc_date, adoc_doc_type, adoc_journ_id AS journ_id, adoc_journ_seq_no, journ_subject, journ_date, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no, comp_name, comp_city, comp_state, comp_id"
      Else
        cut_spot_for_page_break = 17
        query = "SELECT TOP 50 adoc_doc_date, adoc_doc_type, adoc_journ_id AS journ_id, adoc_journ_seq_no, journ_subject, journ_date, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no, comp_name, comp_city, comp_state, comp_id"
      End If

      query = query & " FROM Aircraft_Document WITH(NOLOCK)"
      query = query & " INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id"
      query = query & " INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      query = query & " INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id"

      query = query & " LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id"
      query = query & " LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id"
      query = query & " INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0"
      '     query = query & " WHERE (adoc_doc_date >= '"

      '    query = query & DateAdd(DateInterval.Month, -months_count, DateTime.Now) & " ')"

      '   query = query & "  AND (adoc_doc_date < '" & DateTime.Now & "')"

      query = query & " WHERE (adoc_doc_date >= '" & Month(DateAdd("m", (-1) * months_count, Now)) & "/01/" & Year(DateAdd("m", (-1) * months_count, Now)) & "')"
      query = query & " AND (adoc_doc_date < '" & Month(Now) & "/01/" & Year(Now) & "')"



      If Trim(airframe_type) <> "" Then
        query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
      End If

      'If product_codes.ToString.Trim <> "" Then
      '    If product_codes.ToString.Trim = "B" Then
      '        query = query & " and ac_product_business_flag = 'Y' "
      '    ElseIf product_codes.ToString.Trim = "H" Then
      '        query = query & " and ac_product_helicopter_flag = 'Y' "
      '    ElseIf product_codes.ToString.Trim = "C" Then
      '        query = query & " and ac_product_commercial_flag = 'Y' "
      '    End If
      'End If


      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      ' updated from fipg_main_comp_id
      If CLng(comp_id) > 0 Then
        query = query & " AND ficr_main_comp_id = " & comp_id
      End If

      If Trim(make_name) <> "" Then
        query = query & " AND amod_make_name = '" & make_name & "' "
      ElseIf CLng(model_id) > 0 Then
        query = query & "AND amod_id = " & Trim(model_id)
      End If

      query = query & " ORDER BY adoc_doc_date desc"

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>display_transaction_documents:" & Server.HtmlEncode(query) & "</b><br /><br />"
      'End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      adoRs = SqlCommand.ExecuteReader()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>display_transaction_documents(ByVal model_id As Integer, ByVal table_height As Integer, ByVal city As String, ByVal country As String, ByVal in_HeaderText As String, ByVal comp_id As Integer, ByVal make_model_name As String, ByVal months_count As Integer, ByVal sub_info As String, ByVal real_company_name As String, ByVal sub_type As String, ByVal make_name As String, ByVal make_model_or_make_for_title As String, ByVal product_codes As String, ByVal airframe_type As String)</b><br />" & query

      If adoRs.HasRows Then
        Do While adoRs.Read

          If row_count = 1 Then
            outstring = outstring & "<tr class='alt_row'>" & vbCrLf
            row_count = 0
          Else
            outstring = outstring & "<tr bgcolor='white'>" & vbCrLf
            row_count = 1
          End If
          outstring = outstring & "<td align='left' valign='top'><img src='images/ch_red.jpg' class='bullet'/></td>"
          ' outstring = outstring & "<td valign='top' align='left'></td>" ' & f_displayTransactionDocuments(adoRs("ac_id"), adoRs("journ_id"), adoRs("adoc_journ_seq_no"), False, False, False, True) & "</td>"
          outstring = outstring & "<td valign='top' align='left' class='seperator'><em>" & adoRs("adoc_doc_date") & "</em>, " & adoRs("adoc_doc_type") & sTmpStr & " in favor of " & adoRs("comp_name") & ", " & adoRs("amod_make_name") & "&nbsp;" & adoRs("amod_model_name") & " Ser# " & adoRs("ac_ser_no_full") & " Reg# " & adoRs("ac_reg_no")
          outstring = outstring & "," & adoRs("journ_subject") & " on " & FormatDateTime(adoRs("journ_date"), vbShortDate) & "</td></tr>"

          record_count = record_count + 1
        Loop
      Else
        outstring = outstring & "<tr><td valign='top' align='left' class='seperator'>No data matches for your search criteria.</td></tr>"
      End If

      '    outstring = outstring & "</table></p></div></td></tr></table></td></tr></table>"
      outstring = outstring & "</table></td></tr></table>"


      first_output_section = Replace(first_output_section, "TOTALCOUNTER", record_count.ToString)

      display_transaction_documents = Trim(first_output_section) & Trim(outstring)
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function display_average_days_on_market_graph(ByVal inModelID, ByVal graphID) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Dim outstring, query, x
    Dim nRememberSQLTimeout As Integer = 0
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    outstring = ""
    display_average_days_on_market_graph = ""
    x = 0
    Dim adoRs = Nothing
    Dim counter1 As Integer = 1
    Dim high_number As Integer = 0
    Dim low_number As Integer = 100000000
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1

    Try
      query = "SELECT DISTINCT mtrend_year, mtrend_month, mtrend_avg_market_days"
      query = query & " FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_amod_id = " & CStr(inModelID)
      query = query & " AND (( mtrend_year = year(getdate()-182) AND mtrend_month >= month(getdate()-182) ) OR "
      query = query & " ( mtrend_year = year(getdate()) AND mtrend_month <= month(getdate()) ))"

      Dim type_of_subscription As String = ""
      If Session.Item("localSubscription").crmBusiness_Flag = True Then
        type_of_subscription = "B"
      ElseIf Session.Item("localSubscription").crmCommercial_Flag = True Then
        type_of_subscription = "C"
      ElseIf Session.Item("localSubscription").crmHelicopter_Flag = True Then
        type_of_subscription = "H"
      Else
        type_of_subscription = "B"
      End If

      query = query & " and mtrend_product_type = '" & type_of_subscription & "' "


      query = query & " ORDER BY mtrend_year ASC, mtrend_month ASC"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      ' End Select
      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_average_days_on_market_graph(ByVal inModelID, ByVal graphID) As String</b><br />" & query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adoRs = SqlCommand.ExecuteReader()

      If adoRs.HasRows Then

        Me.AVG_DAYS_ON.Series.Add("AVG_DAYS").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
        Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Title = "Days On Market"

        Me.AVG_DAYS_ON.Series("AVG_DAYS").Color = Drawing.Color.Blue
        Me.AVG_DAYS_ON.Series("AVG_DAYS").BorderWidth = 1
        Me.AVG_DAYS_ON.Series("AVG_DAYS").MarkerSize = 5
        Me.AVG_DAYS_ON.Series("AVG_DAYS").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
        Me.AVG_DAYS_ON.BorderlineWidth = 10
        Me.AVG_DAYS_ON.Series("AVG_DAYS").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        Me.AVG_DAYS_ON.Width = 390
        Me.AVG_DAYS_ON.Height = 260

        Do While adoRs.Read
          If Not IsDBNull(adoRs("mtrend_year")) Then
            If adoRs("mtrend_year").ToString <> "" Then
              If Not IsDBNull(adoRs("mtrend_month")) Then
                If adoRs("mtrend_month").ToString <> "" Then
                  ' outstring = outstring & "data.setValue(" & x & ", 0, '" & CStr(adoRs("mtrend_month")) & "-" & CStr(adoRs("mtrend_year")) & "');" & vbCrLf
                  counter1 = adoRs("mtrend_year")
                  If Not IsDBNull(adoRs("mtrend_avg_market_days")) Then
                    If CDbl(adoRs("mtrend_avg_market_days")) >= 0 Then

                      If CDbl(adoRs("mtrend_avg_market_days")) > high_number Then
                        high_number = adoRs("mtrend_avg_market_days")
                      End If

                      If CDbl(adoRs("mtrend_avg_market_days")) < low_number Then
                        low_number = adoRs("mtrend_avg_market_days")
                      End If



                      Me.AVG_DAYS_ON.Series("AVG_DAYS").Points.AddXY((adoRs("mtrend_month") & "-" & adoRs("mtrend_year")), adoRs("mtrend_avg_market_days"))
                      'outstring = outstring & "data.setValue(" & x & ", 1, " & Format(adoRs("mtrend_avg_market_days"), 0) & ");" & vbCrLf
                    Else
                      'outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                    End If
                  Else
                    'outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                  End If
                  x = x + 1
                End If
              End If
            End If
          End If

        Loop

        If avg_days_on_market > high_number Then
          high_number = avg_days_on_market
        End If

        If avg_days_on_market < low_number Then
          low_number = avg_days_on_market
        End If

        Me.AVG_DAYS_ON.Series("AVG_DAYS").Points.AddXY(Date.Now.Month & "-" & Date.Now.Year, avg_days_on_market)


        Me.AVG_DAYS_ON.Series("AVG_DAYS").Points.Last.Color = Drawing.Color.Black
        Me.AVG_DAYS_ON.Series("AVG_DAYS").Points.Last.BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

        If low_number > 200 Then
          starting_point = (low_number / 200) - 1
          starting_point = starting_point * 200
        Else
          starting_point = 0
        End If

        If low_number < 400 Then
          interval_point = 100
        Else
          interval_point = 200
        End If


        Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Maximum = high_number + 100
        Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
        Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Interval = interval_point


        'Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Maximum = high_number
        'Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Minimum = low_number
        'Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Minimum = 0

        adoRs.close()
        adoRs = Nothing
        'outstring = outstring & "var chart = new google.visualization.LineChart(document.getElementById('visualization" & graphID & "'));" & vbCrLf
        'outstring = outstring & "chart.draw(data, {titleY: 'Days on Market', smoothLine: true, legend: 'none'});" & vbCrLf
        'outstring = outstring & "}" & vbCrLf
        'outstring = outstring & "</script>" & vbCrLf
      Else
        outstring = ""
      End If

      outstring = "Average Days on Market (past 6 months)"

      display_average_days_on_market_graph = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function display_sold_per_month_graph(ByVal bIsAcContactTypeView, ByVal isMarketView, ByVal inModelID, ByVal graphID, ByVal months, ByVal show_future) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Dim outstring, query, x, monthHeader
    monthHeader = ""
    Dim nRememberSQLTimeout As Integer = 0
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    outstring = ""
    display_sold_per_month_graph = ""
    x = 0
    Dim adoRs = Nothing
    Dim high_number As Integer = 0
    Dim low_number As Integer = 100000000
    Dim current_month_to_show As Boolean = False

    Try
      If bIsAcContactTypeView Then   ' go back to the first of the month we are in then subtract amount of days

        query = "SELECT year(journ_date) as tyear, month(journ_date) as tmonth, count(*) as tcount FROM Journal WITH(NOLOCK)"
        query = query & " INNER JOIN aircraft WITH(NOLOCK) ON journ_ac_id = ac_id INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id AND journ_id = ac_journ_id"
        query = query & " WHERE ac_amod_id = " & inModelID & " AND ac_lifecycle_stage < 4"
        query = query & " AND ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WHERE (cref_contact_type IN ('94','33') OR cref_business_type in ('CH')))"

        query = query & " AND ((journ_date >= '" & Month(DateAdd("m", (-1) * months, Now)) & "/01/" & Year(DateAdd("m", (-1) * months, Now)) & "')"
        query = query & " AND (journ_date < '" & Month(Now)
        query = query & "/01/"
        query = query & Year(Now) & "'))"
        monthHeader = months

      ElseIf isMarketView Then

        query = "SELECT year(journ_date) as tyear, month(journ_date) as tmonth, count(*) as tcount FROM Journal WITH(NOLOCK)"
        query = query & " INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id  WHERE ac_amod_id = " & CStr(inModelID)

        If CInt(Session.Item("marketViewTimeSpan")) = 6 Then
          query = query & " AND ((journ_date >= '" & Month(DateAdd("m", (-1) * months, Now)) & "/01/" & Year(DateAdd("m", (-1) * months, Now)) & "')"
          query = query & " AND (journ_date < '" & Month(Now) & "/01/" & Year(Now) & "'))"
          monthHeader = months
        End If

        If CInt(Session.Item("marketViewTimeSpan")) = 6 Then
          query = query & " AND ((journ_date >= '" & Month(DateAdd("m", (-1) * months, Now)) & "/01/" & Year(DateAdd("m", (-1) * months, Now)) & "')"
          query = query & " AND (journ_date < '" & Month(Now) & "/01/" & Year(Now) & "'))"
          monthHeader = months
        End If

      Else

        query = "SELECT year(journ_date) as tyear, month(journ_date) as tmonth, count(*) as tcount FROM Journal WITH(NOLOCK)"
        query = query & " INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id  INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id WHERE ac_amod_id = " & CStr(inModelID)

        query = query & " AND ((journ_date >= '" & Month(DateAdd("m", (-1) * months, Now)) & "/01/" & Year(DateAdd("m", (-1) * months, Now)) & "')"
        query = query & " AND (journ_date < '" & Month(Now)
        If show_future = True Then
          query = query & "/" & Day(Now) & "/"
        Else
          query = query & " /01/"
        End If
        query = query & Year(Now) & "'))"

        monthHeader = months

      End If

      query = query & " AND journ_subcategory_code like 'WS%' AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'"


      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      query = query & " GROUP BY year(journ_date), month(journ_date) ORDER BY year(journ_date), month(journ_date)"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      ' End Select
      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_sold_per_month_graph(ByVal bIsAcContactTypeView, ByVal isMarketView, ByVal inModelID, ByVal graphID, ByVal months, ByVal show_future) As String</b><br />" & query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adoRs = SqlCommand.ExecuteReader()

      If adoRs.HasRows Then
        Me.PER_MONTH.Series.Add("PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
        Me.PER_MONTH.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Sold"


        Me.PER_MONTH.Series("PER_MONTH").Color = Drawing.Color.Blue
        Me.PER_MONTH.Series("PER_MONTH").BorderWidth = 1
        Me.PER_MONTH.Series("PER_MONTH").MarkerSize = 5
        Me.PER_MONTH.Series("PER_MONTH").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
        Me.PER_MONTH.BorderlineWidth = 10
        Me.PER_MONTH.Series("PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        Me.PER_MONTH.Width = 260
        Me.PER_MONTH.Height = 260


        'Me.FOR_SALE.Series("PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Line
        Do While adoRs.Read
          If Not IsDBNull(adoRs("tyear")) Then
            If adoRs("tyear").ToString <> "" Then
              If Not IsDBNull(adoRs("tmonth")) Then
                If adoRs("tmonth").ToString <> "" Then
                  'outstring = outstring & "data.setValue(" & x & ", 0, '" & CStr(adoRs("tmonth")) & "-" & CStr(adoRs("tyear")) & "');" & vbCrLf
                  If Not IsDBNull(adoRs("tcount")) Then
                    If CDbl(adoRs("tcount")) >= 0 Then


                      If CDbl(adoRs("tcount")) > high_number Then
                        high_number = adoRs("tcount")
                      End If
                      If CDbl(adoRs("tcount")) < low_number Then
                        low_number = adoRs("tcount")
                      End If

                      If adoRs("tmonth") = Month(Now) Then
                        current_month_to_show = True
                      End If
                      Me.PER_MONTH.Series("PER_MONTH").Points.AddXY((adoRs("tmonth") & "-" & adoRs("tyear")), adoRs("tcount"))

                      'Me.AVG_PER_MONTH.Series("PER_MONTH").Points.AddXY(x, adoRs("tcount"))
                      '  outstring = outstring & "data.setValue(" & x & ", 1, " & Format(adoRs("tcount"), 0) & ");" & vbCrLf
                    Else
                      ' outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                    End If
                  Else
                    ' outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                  End If
                  x = x + 1
                End If
              End If
            End If
          End If
        Loop

        If show_future = True And current_month_to_show Then
          Me.PER_MONTH.Series("PER_MONTH").Points.Last.Color = Drawing.Color.Black
          Me.PER_MONTH.Series("PER_MONTH").Points.Last.BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

        End If


        Me.PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = high_number + 1
        Me.AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Minimum = low_number - 1
        'Me.PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0


      Else
        outstring = ""
      End If


      outstring = "Sold Per Month (past " & monthHeader & " months)"


      display_sold_per_month_graph = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function display_for_sale_by_month_graph(ByVal inModelID, ByVal graphID) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim outstring, query, x
    Dim nRememberSQLTimeout As Integer = 0
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    Dim adoRs = Nothing
    Dim high_number As Integer = 0
    Dim low_number As Integer = 100000000
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1
    Me.FOR_SALE.Series.Clear()
    Me.FOR_SALE.Series.Add("FOR_SALE")
    outstring = ""
    display_for_sale_by_month_graph = ""
    x = 0
    Try
      query = "SELECT DISTINCT mtrend_year, mtrend_month, mtrend_total_aircraft_for_sale"
      query = query & " FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_amod_id = " & CStr(inModelID)
      query = query & " AND ( (mtrend_year = year(getdate()-182) AND mtrend_month >= month(getdate()-182)) OR"
      query = query & " (mtrend_year = year(getdate()) AND mtrend_month <= month(getdate())) )"

      Dim type_of_subscription As String = ""
      If Session.Item("localSubscription").crmBusiness_Flag = True Then
        type_of_subscription = "B"
      ElseIf Session.Item("localSubscription").crmCommercial_Flag = True Then
        type_of_subscription = "C"
      ElseIf Session.Item("localSubscription").crmHelicopter_Flag = True Then
        type_of_subscription = "H"
      Else
        type_of_subscription = "B"
      End If

      query = query & " and mtrend_product_type = '" & type_of_subscription & "' "


      query = query & " ORDER BY mtrend_year ASC, mtrend_month ASC"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      ' End Select
      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_for_sale_by_month_graph(ByVal inModelID, ByVal graphID) As String</b><br />" & query

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adoRs = SqlCommand.ExecuteReader()


      If adoRs.HasRows Then

        Me.FOR_SALE.Series("FOR_SALE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
        Me.FOR_SALE.ChartAreas("ChartArea1").AxisY.Title = "Aircraft For Sale"

        Me.FOR_SALE.Series("FOR_SALE").Color = Drawing.Color.Blue
        Me.FOR_SALE.Series("FOR_SALE").BorderWidth = 1
        Me.FOR_SALE.Series("FOR_SALE").MarkerSize = 5
        Me.FOR_SALE.Series("FOR_SALE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
        Me.FOR_SALE.BorderlineWidth = 10
        Me.FOR_SALE.Series("FOR_SALE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        Me.FOR_SALE.Width = 185
        Me.FOR_SALE.Height = 185


        Do While adoRs.Read
          If Not IsDBNull(adoRs("mtrend_year")) Then
            If adoRs("mtrend_year").ToString <> "" Then
              If Not IsDBNull(adoRs("mtrend_month")) Then
                If adoRs("mtrend_month").ToString <> "" Then

                  ' outstring = outstring & "data.setValue(" & x & ", 0, '" & CStr(adoRs("mtrend_month")) & "-" & CStr(adoRs("mtrend_year")) & "');" & vbCrLf
                  If Not IsDBNull(adoRs("mtrend_total_aircraft_for_sale")) Then
                    If CDbl(adoRs("mtrend_total_aircraft_for_sale")) >= 0 Then

                      If CDbl(adoRs("mtrend_total_aircraft_for_sale")) > high_number Then
                        high_number = adoRs("mtrend_total_aircraft_for_sale")
                      End If
                      If CDbl(adoRs("mtrend_total_aircraft_for_sale")) < low_number Then
                        low_number = adoRs("mtrend_total_aircraft_for_sale")
                      End If

                      ' Me.FOR_SALE.Series("FOR_SALE").Points.AddXY(adoRs("mtrend_year"), adoRs("mtrend_total_aircraft_for_sale"))
                      Me.FOR_SALE.Series("FOR_SALE").Points.AddXY((adoRs("mtrend_month") & "-" & adoRs("mtrend_year")), adoRs("mtrend_total_aircraft_for_sale"))
                      '  outstring = outstring & "data.setValue(" & x & ", 1, " & Format(adoRs("mtrend_total_aircraft_for_sale")) & ");" & vbCrLf
                    Else
                      'outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                    End If
                  Else
                    ' outstring = outstring & "data.setValue(" & x & ", 1, 0);" & vbCrLf
                  End If
                  x = x + 1
                End If
              End If
            End If
          End If

        Loop


      Else
        outstring = ""
      End If

      adoRs.close()
      adoRs.dispose()

      query = "SELECT count(distinct ac_id) as tcount from aircraft WITH(NOLOCK) INNER JOIN aircraft_model on amod_id = ac_amod_id WHERE ac_amod_id = " & CStr(inModelID)
      query = query & " and ac_forsale_flag = 'Y' and ac_journ_id = 0 "
      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      SqlCommand.CommandText = query
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_for_sale_by_month_graph(ByVal inModelID, ByVal graphID) As String</b><br />" & query
      adoRs = SqlCommand.ExecuteReader()
      If adoRs.HasRows Then
        adoRs.read()
        If CDbl(adoRs("tcount")) > high_number Then
          high_number = adoRs("tcount")
        End If
        If CDbl(adoRs("tcount")) < low_number Then
          low_number = adoRs("tcount")
        End If

        Me.FOR_SALE.Series("FOR_SALE").Points.AddXY(Date.Now.Month & "-" & Date.Now.Year(), adoRs("tcount"))

        Me.FOR_SALE.Series("FOR_SALE").Points.Last.Color = Drawing.Color.Black
        Me.FOR_SALE.Series("FOR_SALE").Points.Last.BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
      End If


      If low_number > 5 Then
        starting_point = (low_number / 5) - 1
        starting_point = starting_point * 5
      End If



      Me.FOR_SALE.ChartAreas("ChartArea1").AxisY.Maximum = high_number + 1
      Me.FOR_SALE.ChartAreas("ChartArea1").AxisY.Minimum = low_number - 1
      Me.FOR_SALE.ChartAreas("ChartArea1").AxisY.Interval = interval_point
      'Me.FOR_SALE.ChartAreas("ChartArea1").AxisY.Minimum = 0




      outstring = "For Sale By Month (past 6 months)"

      display_for_sale_by_month_graph = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------

  'Function load_selected() Handles tabs_container.ActiveTabChanged

  '    If tabs_container.ActiveTabIndex = 0 Then
  '        Build_FleetMarketSummary(272, False, "Challenger", "Fleet")
  '        Me.market_status_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
  '        Me.market_status_tab_label.Text += GetMarketStatus
  '        Me.market_status_tab_label.Text += "</div></td></tr></table>"
  '    ElseIf tabs_container.ActiveTabIndex = 1 Then
  '        Build_FleetMarketSummary(272, False, "Challenger", "Fleet")
  '        Me.fleet_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
  '        Me.fleet_tab_label.Text += Build_FleetMarketSummary_text
  '        Me.fleet_tab_label.Text += "</div></td></tr></table>"
  '    ElseIf tabs_container.ActiveTabIndex = 2 Then
  '        Me.specs_tab_label.Text = "<table width='100%' cellpadding='3' height='200'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
  '        Me.specs_tab_label.Text += Build_PerformanceSpecifications(False, "", False, "", 272, "Challenger 300")
  '        Me.specs_tab_label.Text += "</div></td></tr></table>"
  '    ElseIf tabs_container.ActiveTabIndex = 3 Then
  '        Me.operating_costs_tab_label_direct.Text = "<table width='100%' cellpadding='3'><tr><td align='left' valign='top'><div class='tab_container_div2'>"
  '        Me.operating_costs_tab_label_direct.Text += Build_OperatingCosts(272, "Challenger 300")
  '        Me.operating_costs_tab_label_direct.Text += "</div></td></tr></table>"
  '    End If


  'End Function
  Public Function Build_FleetMarketSummary(ByVal inModelID As Long, ByVal bIsAcContactTypeView As Boolean, ByVal inMakeName As String, ByVal type As String) As String
    Build_FleetMarketSummary = ""

    Dim strHTML

    strHTML = ""
    Build_FleetMarketSummary = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim fleetinfo As System.Data.SqlClient.SqlDataReader : fleetinfo = Nothing
    Dim readOnce As Boolean = False
    Dim avgyear As Integer = 0
    Dim totalcount As Integer = 0
    Dim totalInOpcount As Integer = 0
    Dim ac_for_sale As Integer = 0
    Dim ac_exclusive_sale As Integer = 0
    Dim ac_lease As Integer = 0
    Dim w_owner As Integer = 0
    Dim s_owner As Integer = 0
    Dim f_owner As Integer = 0
    Dim o_stage As Integer = 0
    Dim t_stage As Integer = 0
    Dim th_stage As Integer = 0
    Dim f_stage As Integer = 0
    Dim daysonmarket As Integer = 0
    Dim daysonmarket2 As Integer = 0
    Dim days As Integer = 0
    Dim allhigh As Integer = 0
    Dim alllow As Integer = 0
    Dim forsaleavghigh As String = "0"
    Dim forsaleavlow As String = "199999999999999999999999999999999999999999999999999999"
    Dim per As Integer = 0
    Dim per2 As Integer = 0
    Dim per3 As Integer = 0


    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG


      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60




      Dim num, count, i, Query

      num = True
      count = 0

      If Not bIsAcContactTypeView Then

        Query = "SELECT ac_id, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag,"
        Query = Query & " ac_lease_flag, ac_asking, ac_asking_price, ac_list_date, ac_mfr_year, datediff(day, ac_list_date, '" & Date.Now.Date & "') AS daysonmarket"

        Query = Query & " FROM Aircraft INNER JOIN aircraft_model on amod_id = ac_amod_id  WHERE ac_amod_id = " & inModelID & " AND ac_journ_id = 0" '  WITH(NOLOCK)

      Else

        Query = "SELECT ac_id, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag,"
        Query = Query & " ac_lease_flag, ac_asking, ac_asking_price, ac_list_date, ac_mfr_year, datediff(day, ac_list_date, '" & Date.Now.Date & "') AS daysonmarket"

        Query = Query & " FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model on amod_id = ac_amod_id WHERE ac_amod_id = " & inModelID & " AND ac_journ_id = 0"
        Query = Query & " AND EXISTS (SELECT NULL FROM aircraft_reference WITH(NOLOCK)"
        Query = Query & " WHERE cref_ac_id = ac_id AND cref_journ_id = ac_journ_id"
        Query = Query & " AND (cref_contact_type IN ('94','33') OR cref_business_type = 'CH'))"

      End If


      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = Query
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Build_FleetMarketSummary(ByVal inModelID As Long, ByVal bIsAcContactTypeView As Boolean, ByVal inMakeName As String, ByVal type As String) As String</b><br />" & Query
      fleetinfo = SqlCommand.ExecuteReader()


      If fleetinfo.HasRows Then


        Do While fleetinfo.Read()
          If readOnce = False Then
            If Not IsDBNull(fleetinfo.Item("ac_mfr_year")) Then
              If IsNumeric(fleetinfo.Item("ac_mfr_year")) Then
                allhigh = CInt(fleetinfo.Item("ac_mfr_year"))
                alllow = CInt(fleetinfo.Item("ac_mfr_year"))
              End If
            Else
              allhigh = 0
              alllow = CInt(Year(Now()))
            End If
            readOnce = True
          End If

          If Not IsDBNull(fleetinfo.Item("daysonmarket")) Then
            If CLng(fleetinfo.Item("daysonmarket")) > 0 Then
              daysonmarket = daysonmarket + 1
              daysonmarket2 = daysonmarket2 + CLng(fleetinfo("daysonmarket"))
            End If
          End If

          If Not IsDBNull(fleetinfo.Item("ac_mfr_year")) Then
            If IsNumeric(fleetinfo.Item("ac_mfr_year")) Then

              If CInt(fleetinfo.Item("ac_mfr_year")) > CInt(allhigh) Then
                allhigh = CInt(fleetinfo.Item("ac_mfr_year"))
              End If

              If CInt(fleetinfo.Item("ac_mfr_year")) < CInt(alllow) Then
                alllow = CInt(fleetinfo.Item("ac_mfr_year"))
              End If

            End If
          End If

          totalcount = totalcount + 1

          If fleetinfo.Item("ac_lifecycle_stage") = "3" And
            (fleetinfo.Item("ac_ownership_type") = "S" Or fleetinfo.Item("ac_ownership_type") = "F" Or fleetinfo.Item("ac_ownership_type") = "W") Then
            totalInOpcount = totalInOpcount + 1
          End If

          If fleetinfo.Item("ac_ownership_type") = "W" And fleetinfo.Item("ac_lifecycle_stage") = "3" Then
            w_owner = w_owner + 1
          End If

          If fleetinfo.Item("ac_ownership_type") = "F" And fleetinfo.Item("ac_lifecycle_stage") = "3" Then
            f_owner = f_owner + 1
          End If

          If fleetinfo.Item("ac_ownership_type") = "S" And fleetinfo.Item("ac_lifecycle_stage") = "3" Then
            s_owner = s_owner + 1
          End If

          If fleetinfo.Item("ac_lifecycle_stage") = "1" Then
            o_stage = o_stage + 1
          End If

          If fleetinfo.Item("ac_lifecycle_stage") = "2" Then
            t_stage = t_stage + 1
          End If

          If fleetinfo.Item("ac_lifecycle_stage") = "3" And
            (fleetinfo.Item("ac_ownership_type") = "S" Or fleetinfo.Item("ac_ownership_type") = "F" Or fleetinfo.Item("ac_ownership_type") = "W") Then
            th_stage = th_stage + 1
          End If

          If fleetinfo.Item("ac_lifecycle_stage") = "4" Then
            f_stage = f_stage + 1
          End If

          If fleetinfo.Item("ac_forsale_flag") = "Y" Then
            ac_for_sale = ac_for_sale + 1

            If Not IsDBNull(fleetinfo.Item("ac_asking_price")) Then


              If CStr(fleetinfo.Item("ac_asking_price")) <> "" Then

                If CDbl(fleetinfo.Item("ac_asking_price")) > CDbl(forsaleavghigh) Then
                  forsaleavghigh = fleetinfo.Item("ac_asking_price")
                End If

                If CDbl(fleetinfo.Item("ac_asking_price")) < CDbl(forsaleavlow) Then
                  forsaleavlow = fleetinfo.Item("ac_asking_price")
                  num = False
                End If

              End If
            End If
          End If

          If fleetinfo.Item("ac_exclusive_flag") = "Y" Then
            ac_exclusive_sale = ac_exclusive_sale + 1
          End If

          If Not IsDBNull(fleetinfo.Item("ac_lease_flag")) Then
            If fleetinfo.Item("ac_lease_flag") = "Y" Then
              ac_lease = ac_lease + 1
            End If
          End If
          'fleetinfo.Item.moveNext()
        Loop

      End If

      fleetinfo.Close()
      SqlConn.Close()
      If num = True Then
        forsaleavlow = forsaleavghigh
      End If

      If (forsaleavlow > 0) Then
        forsaleavlow = CDbl(forsaleavlow / 1000)
        forsaleavlow = FormatCurrency(forsaleavlow, 0)
        forsaleavlow = forsaleavlow & "k"
      End If

      If (forsaleavghigh > 0) Then
        forsaleavghigh = CDbl(forsaleavghigh / 1000)
        forsaleavghigh = FormatCurrency(forsaleavghigh, 0)
        forsaleavghigh = forsaleavghigh & "k"
      End If

      If (ac_for_sale > 0 And th_stage > 0) Then

        per = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
        ' per2 = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100))
        per2 = ac_exclusive_sale / ac_for_sale * 100
        per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

        If daysonmarket > 0 Then
          days = System.Math.Round(CLng(daysonmarket2) / CLng(daysonmarket))
        Else
          days = System.Math.Round(CLng(daysonmarket2))
        End If

      End If

      If (alllow >= 0 And allhigh > 0) Then
        For i = alllow To allhigh
          avgyear = avgyear + i
          count = count + 1
        Next
      End If

      If avgyear > 0 And count > 0 Then
        avgyear = CLng(avgyear / count)
        avgyear = FormatNumber(avgyear, 0, False, True, False)
      End If

      'Response.write " ALLHIGH:" & allhigh
      'Response.write " ALLLOW:" & alllow
      'Response.write " FORSALEAVLOW:" & forsaleavlow
      'Response.write " FORSALEAVGHIGH:" & forsaleavghigh
      'Response.write " AVGMFRYEAR:" & avgyear
      'Response.write " COUNT:" & count


      fleetinfo = Nothing


      strHTML = strHTML & "<table align='center' id='fleetTable'  cellpadding='1' cellspacing='0' width='100%'>" & vbCrLf


      ' tr start for Logo
      '   strHTML = strHTML & "<tr id='trInner_Content_AC_PIC'>" & vbCrLf
      ' strHTML = strHTML & "<td  id='tdInner_Content_AC_PIC' align='center' colspan='3'>" & vbCrLf
      ' get the AC pic

      ' strHTML = strHTML & "</td>" & vbCrLf
      ' strHTML = strHTML & "</tr>" & vbCrLf


      '    strHTML = strHTML & "<tr><td colspan='3' align='center' valign='middle'><strong>" & UCase(make_model_name) & " Fleet/Market Summary</strong></td></tr>" & vbCrLf '<tr><td colspan='3'>&nbsp;</td></tr>
      strHTML = strHTML & "<tr>" & vbCrLf

      ' Ownership table
      strHTML = strHTML & "<td align='right' valign='top' class='FleetMarket_Left_TD' width='50%'><table id='lifeCycleTable'  cellspacing='0' cellpadding='4'>" & vbCrLf
      strHTML = strHTML & "<tr class='aircraft_list'><td valign='middle' align='center' colspan='2'><strong>Ownership (In Operation)</strong></td></tr>" & vbCrLf ' <tr><td>&nbsp;</td></tr>

      If CLng(w_owner) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >Whole:&nbsp;</td><td align='right'>" & FormatNumber(w_owner, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >Whole:&nbsp;</td><td align='right'>&nbsp;0</td><td>&nbsp;</td></tr>" & vbCrLf
      End If

      If CLng(s_owner) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(s_owner, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;0</td><td>&nbsp;</td></tr>" & vbCrLf
      End If

      If CLng(f_owner) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >Fractional:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(f_owner, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >Fractional:&nbsp;</td><td align='right'>&nbsp;0</td><td>&nbsp;</td></tr>" & vbCrLf
      End If

      If CLng(totalInOpcount) > 0 Then
        strHTML = strHTML & "<tr><td bgcolor='#F8F8F8' valign='top' align='left'  nowrap='nowrap'>Total Aircraft:&nbsp;</td><td  bgcolor='#F8F8F8' align='right'>&nbsp;" & FormatNumber(totalInOpcount, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td bgcolor='#F8F8F8' valign='top' align='left'  nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td><td>&nbsp;</td></tr>" & vbCrLf
      End If

      If CLng(alllow) > 0 And CLng(allhigh) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' class='border_bottom' nowrap='nowrap'>MFR Year Range:&nbsp;</td><td align='right'>&nbsp;" & alllow & " - " & allhigh & "</td><td>&nbsp;</td></tr>" & vbCrLf
      ElseIf CLng(alllow) > 0 And CLng(allhigh) = 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' class='border_bottom' nowrap='nowrap'>MFR Year Range:&nbsp;</td><td align='right'>&nbsp;" & alllow & " - To Present</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' class='border_bottom' nowrap='nowrap'>MFR Year Range:&nbsp;</td><td align='right'>&nbsp;N/A</td></tr>" & vbCrLf
      End If

      strHTML = strHTML & "<tr><td>&nbsp;</td><td>&nbsp;</td></tr></table></td>" & vbCrLf
      ' spacer
      strHTML = strHTML & "<td rowspan='7' class='FleetMarket_Right_TD'>&nbsp;</td>" & vbCrLf

      ' Fleet Info
      strHTML = strHTML & "<td align='left' width='50%'>" & vbCrLf
      strHTML = strHTML & "<table id='lifeCycleTable' width='175' cellspacing='0' cellpadding='4'>" & vbCrLf
      strHTML = strHTML & "<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong>Life Cycle</strong></td></tr>" & vbCrLf ' <tr><td>&nbsp;</td></tr>

      If CLng(o_stage) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >In Production:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(o_stage, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >In Production:&nbsp;</td><td align='right'>&nbsp;0</td></tr>" & vbCrLf
      End If

      If CLng(t_stage) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(t_stage, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;0</td></tr>" & vbCrLf
      End If

      If CLng(th_stage) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(th_stage, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;0</td></tr>" & vbCrLf
      End If

      If CLng(f_stage) > 0 Then
        strHTML = strHTML & "<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(f_stage, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;0</td></tr>" & vbCrLf
      End If

      If CLng(totalcount) > 0 Then
        strHTML = strHTML & "<tr><td valign='top'  bgcolor='#F8F8F8' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" & FormatNumber(totalcount, 0, True, False, True) & "</td></tr>" & vbCrLf
      Else
        strHTML = strHTML & "<tr bgcolor='#F8F8F8'><td valign='top' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td  class='border_bottom' align='right' bgcolor='#F8F8F8'>&nbsp;0</td></tr>" & vbCrLf
      End If

      strHTML = strHTML & "<tr><td>&nbsp;</td></tr></table>" & vbCrLf

      strHTML = strHTML & "</td></tr></table>" & vbCrLf ' changed from original

      Build_FleetMarketSummary = Trim(strHTML)
      Build_FleetMarketSummary_text = Build_FleetMarketSummary
      '  GetMarketStatus = "<td align='center'  width='15%' class='FleetMarket_Bottom_TD' colspan='3'>" ' changed from original

      GetMarketStatus = GetMarketStatus & "<table width='100%' cellspacing='0' cellpadding='4' valign='top'>" & vbCrLf '<tr>&nbsp;</tr>
      ' GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='center' colspan='2'><strong>Market Status</strong></td></tr>" & vbCrLf

      'If CLng(ac_for_sale) > 0 Then
      '    If Not Session.Item("Aerodex") Then
      '        GetMarketStatus = GetMarketStatus & "<tr valign='top'><td valign='top' align='left' >For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ac_for_sale, 0, True, False, True) & "&nbsp;<span class='tiny'>(" & FormatNumber(per, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
      '    Else
      '        GetMarketStatus = GetMarketStatus & "<tr valign='top'><td valign='top' align='left' >For Sale:&nbsp;</td><td align='left'>" & FormatNumber(ac_for_sale, 0, True, False, True) & " &nbsp;<span class='tiny'>(" & FormatNumber(per, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
      '    End If
      'Else
      '    GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >For Sale:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
      'End If

      string_for_op_percentage = "&nbsp;<span class='tiny'>(" & FormatNumber(per, 1) & "% of In Operation)"

      If Trim(forsaleavlow) <> "0" And Trim(forsaleavghigh) <> "0" Then
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Asking Price Range:&nbsp;</td><td align='left'>" & forsaleavlow & " - " & forsaleavghigh & "</td></tr>" & vbCrLf
      Else
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Asking Price Range:&nbsp;</td><td align='left'>No Asking Prices</td></tr>" & vbCrLf
      End If

      If Not Session.Item("Aerodex") Then
        ' THIS IS FOR ON EXCLUSIVE %
        If CLng(ac_exclusive_sale) > 0 Then
          GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >On Exclusive:&nbsp;</td><td align='left'>" & FormatNumber(ac_exclusive_sale, 0, True, False, True) & " <span class='tiny'>(" & FormatNumber(per2, 1) & "% of For Sale on Exclusive)</span></td></tr>" & vbCrLf
        Else
          GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >On Exclusive:&nbsp;</td><td align='left'>(0% of For Sale on Exclusive)</span></td></tr>" & vbCrLf
        End If

      End If

      If Trim(avgyear) <> "0" Then
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Avg MFG Year:&nbsp;</td><td align='left'>" & avgyear & "</td></tr>" & vbCrLf
      Else
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Avg MFG Year:&nbsp;</td><td align='v'>N/A</td></tr>" & vbCrLf
      End If


      avg_days_on_market = days
      If Trim(days) <> "" Then
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Avg Days on Market:&nbsp;</td><td align='left'>" & days & "</td></tr>" & vbCrLf
      Else
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Avg Days on Market:&nbsp;</td><td align='left'>N/A</td></tr>" & vbCrLf
      End If

      If CLng(ac_lease) > 0 Then
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Leased:&nbsp;</td><td align='left'>" & FormatNumber(ac_lease, 0, True, False, True) & "&nbsp;<span class='tiny'>(" & FormatNumber(per3, 1) & "% of In Operation)</span></td></tr>" & vbCrLf
      Else
        GetMarketStatus = GetMarketStatus & "<tr><td valign='top' align='left' >Leased:&nbsp;</td><td align='left'>0 <span class='tiny'>(0% of In Operation)</span></td></tr>" & vbCrLf
      End If

      GetMarketStatus = GetMarketStatus & "<tr><td>&nbsp;</td></tr></table> "

      ' GetMarketStatus = ""
      If type = "Fleet" Then
        Build_FleetMarketSummary = Build_FleetMarketSummary
      Else
        Build_FleetMarketSummary = GetMarketStatus
      End If

    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function Build_PerformanceSpecifications(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bHasManyAirFrames As Boolean, ByVal sAirframeType As String, ByVal amod_id As Integer, ByVal make_model_name As String) As String
    Build_PerformanceSpecifications = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim l_AdoRs As System.Data.SqlClient.SqlDataReader : l_AdoRs = Nothing
    Dim bIsFirstTime As Boolean = True
    Dim Query As String : Query = ""
    Dim number_of_engine_types As Integer = 0
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString
      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
      SqlCommand.CommandText = Query
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Build_PerformanceSpecifications(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bHasManyAirFrames As Boolean, ByVal sAirframeType As String, ByVal amod_id As Integer, ByVal make_model_name As String) As String</b><br />" & Query

      Dim l_objBuilder As New StringBuilder
      Dim nNumberOfEngines As Integer = 0
      Dim nRememberModelID As Long = 0
      Dim nMAXEngines As Long = 0

      l_AdoRs = SqlCommand.ExecuteReader()
      Dim iLoop
      iLoop = 0
      l_AdoRs.Read()
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


      l_objBuilder.Append("<table align='center' cellpadding='2' cellspacing='0' width='100%'>" & vbCrLf)
      ' tr start for Logo
      ' l_objBuilder.Append("<tr id='trInner_Content_AC_PIC'>" & vbCrLf)
      '  l_objBuilder.Append("<td  id='tdInner_Content_AC_PIC' align='center' colspan='5'>" & vbCrLf)
      ' get the AC pic

      ' l_objBuilder.Append("</td>")
      ' l_objBuilder.Append("</tr>")

      '  l_objBuilder.Append("<tr><td align='center' colspan='5'><strong>" & make_model_name & "&nbsp;Performance Specifications</strong></td></tr><tr>" & vbCrLf)


      For x As Integer = 0 To 1


        If bIsFirstTime Then
          bIsFirstTime = False
          'l_objBuilder.Append("<tr><th nowrap Colspan='5'>MODEL&nbsp;NAME</th></tr>" & vbCrLf)
          l_objBuilder.Append("<tr>")
          l_objBuilder.Append("<td valign='top' align='left' class='Performance_Specs_Left_TD_1'>" & vbCrLf)
          l_objBuilder.Append("<table cellpadding='2' cellspacing='0'>" & vbCrLf)
          l_objBuilder.Append("<tr><td nowrap><strong>Fuselage&nbsp;Dimensions</strong></td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)

            If sAirframeType = "F" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Wing Span (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Wing Span (" & TranslateUSMetricUnitsShort("FT") & ") / [R] Width (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (ft):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (ft):</td></tr>" & vbCrLf)

            If sAirframeType = "F" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Wing Span (ft):</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Width (ft):</td></tr>" & vbCrLf)
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[F] Wing Span (ft) / [R] Width (ft):</td></tr>" & vbCrLf)
            End If
          End If

          l_objBuilder.Append("<tr><td nowrap><strong>Cabin&nbsp;Dimensions</strong></td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (ft)(inches):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (ft)(inches):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (ft)(inches):</td></tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<tr><td><strong>Typical&nbsp;Configuration</strong></td></tr><tr>" & vbCrLf)

          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Crew:</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Passengers:</td></tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            If sAirframeType = "F" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Pressurization&nbsp;(" & TranslateUSMetricUnitsShort("PSI") & "):</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              ' do nothing
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[F] Pressurization&nbsp;(" & TranslateUSMetricUnitsShort("PSI") & "):</td></tr>" & vbCrLf)
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

          l_objBuilder.Append("<tr><td><strong>Fuel Capacity</strong></td></tr><tr>")

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(" & TranslateUSMetricUnitsShort("gal") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(" & TranslateUSMetricUnitsShort("gal") & "):</td></tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(gal):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(gal):</td></tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<tr><td><strong>Weight</strong></td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Ramp&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Takeoff&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)

            If sAirframeType = "F" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Zero&nbsp;Fuel&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Empty&nbsp;Operating&nbsp;Weight&nbsp;(EOW):</td></tr><tr>" & vbCrLf)
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>[F] Zero&nbsp;Fuel&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & ") / [R] Empty&nbsp;Operating&nbsp;Weight&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            End If
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Basic&nbsp;Operating&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Landing&nbsp;(" & TranslateUSMetricUnitsShort("lbs") & "):</td></tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Ramp&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Takeoff&nbsp;(lbs):</td></tr><tr>" & vbCrLf)

            If sAirframeType = "F" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Zero&nbsp;Fuel&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Empty&nbsp;Operating&nbsp;Weight&nbsp;(EOW):</td></tr><tr>" & vbCrLf)
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>[F] Zero&nbsp;Fuel&nbsp;(lbs) / [R] Empty&nbsp;Operating&nbsp;Weight&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            End If
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Basic&nbsp;Operating&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Landing&nbsp;(lbs):</td></tr>" & vbCrLf)
          End If
          l_objBuilder.Append("</table></td>" & vbCrLf)



          'Do While Not l_AdoRs.Read
          l_objBuilder.Append("<td valign='top' align='left'  class='Performance_Specs_Left_TD_2'>" & vbCrLf)
          l_objBuilder.Append("<table cellpadding='2' cellspacing='0'>" & vbCrLf)

          '''''''''''''''''''''''''''''''''''''''''''''''''''
          ' Start left side
          '''''''''''''''''''''''''''''''''''''''''''''''''''
          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_length"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_height"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)

            If sAirframeType = "F" Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_wingspan"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_width"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_length")), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_height")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)

            If sAirframeType = "F" Then
              l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_wingspan")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" Then
              l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_width")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            End If
          End If

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)



          ' amod_cabinsize_height_feet , amod_cabinsize_height_inches
          ' amod_cabinsize_width_feet , amod_cabinsize_width_inches
          ' amod_cabinsize_length_feet , amod_cabinsize_length_inches

          ' THIS IS FOR CABIN DIMENSIONS
          If Session.Item("useMetricValues") Then

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_length_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_length_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_length_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_height_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_height_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_height_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_width_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_width_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_width_feet"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr>" & vbCrLf)
            End If

          Else

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_length_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_length_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_length_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_length_inches") & "'&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_height_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_height_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_height_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_height_inches") & "'&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='left'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_width_feet")) Then
              If CDbl(l_AdoRs.Item("amod_cabinsize_width_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_width_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_width_inches") & "'&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr>" & vbCrLf)
            End If

          End If

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_number_of_crew") & "&nbsp;</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_number_of_passengers") & "&nbsp;</td></tr>" & vbCrLf)


          ' THIS IS FOR PRESSURIZATION SECTION
          If sAirframeType = "F" Then
            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("PSI", CDbl(l_AdoRs.Item("amod_pressure"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            Else
              'l_objBuilder.Append("<tr><td valign='middle' align='left'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_pressure")), 0, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)

              l_objBuilder.Append("<tr><td valign='middle' align='right'>" & CDbl(l_AdoRs.Item("amod_pressure")) & "&nbsp;</td></tr>" & vbCrLf)

            End If
          ElseIf sAirframeType = "R" And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_std_weight")) Then
            If CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight")) > 0 Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_std_gal")) Then
            If CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal")) > 0 Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("GAL", CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_opt_weight")) Then
            If CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight")) > 0 Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_opt_gal")) Then
            If CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal")) > 0 Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("GAL", CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_ramp_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_ramp_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_takeoff_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_takeoff_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If sAirframeType = "F" Then
            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_zero_fuel_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_zero_fuel_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          ElseIf sAirframeType = "R" Then
            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_weight_eow"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_weight_eow")), False, True, False) & "&nbsp;</td></tr><tr>")
            End If
          End If

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_basic_op_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_basic_op_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_landing_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_landing_weight")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
          End If
          '''''''''''''''''''''''''''''''''''''''''''''''''''
          ' End the left column
          '''''''''''''''''''''''''''''''''''''''''''''''''''
          l_objBuilder.Append("</table></td>" & vbCrLf)




        Else

          ''''''''''''''''''''''''''''''''''''''''''''
          ' Break goes here
          ''''''''''''''''''''''''''''''''''''''''''''
          l_objBuilder.Append("<td  valign='top' align='left' class='Performance_Specs_Filler_TD_Middle'>&nbsp;" & vbCrLf)

          l_objBuilder.Append("</td>" & vbCrLf)
          l_objBuilder.Append("<td valign='top' align='left' class='Performance_Specs_Right_TD_1'>" & vbCrLf)
          l_objBuilder.Append("<table cellpadding='2' cellspacing='0'>" & vbCrLf)
          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<tr><td><strong>Speed&nbsp;" & TranslateUSMetricUnitsLong("KN") & "</strong></td></tr><tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<tr><td><strong>Speed&nbsp;Knots</strong></td></tr><tr>" & vbCrLf)
          End If

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vs&nbsp;Clean:</td></tr><tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Vs&nbsp;Clean:</td></tr><tr>" & vbCrLf)
          End If

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vso&nbsp;Landing:</td></tr><tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Vso&nbsp;Landing:</td></tr><tr>" & vbCrLf)
          End If
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;Cruise&nbsp;TAS:</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vmo&nbsp;(Max&nbsp;Op)&nbsp;IAS:</td></tr>" & vbCrLf)

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><th nowrap>IFR Certification:</th></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>(IFR):</td></tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><th nowrap>[R] IFR Certification:</th></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[R] (IFR):</td></tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<tr><td><strong>Climb</strong></td></tr><tr>")
          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;(" & TranslateUSMetricUnitsShort("FPM") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Engine&nbsp;Out&nbsp;(" & TranslateUSMetricUnitsShort("FPM") & "):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Ceiling (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;(fpm):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Engine&nbsp;Out&nbsp;(fpm):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Ceiling (ft):</td></tr>" & vbCrLf)
          End If


          If sAirframeType = "F" And Not bHasManyAirFrames Then
            'do nothing
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>(HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>(HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[R] (HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[R] (HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
          End If

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            temp_op_cost_string = temp_op_cost_string & "<tr><td><strong>Landing Performance</strong></td></tr><tr>"
            If Session.Item("useMetricValues") Then
              temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>FAA&nbsp;Field&nbsp;Length (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>"
            Else
              temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>FAA&nbsp;Field&nbsp;Length (ft):</td></tr>"
            End If
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            temp_op_cost_string = temp_op_cost_string & "<tr><td><strong>[F] Landing Performance<strong></td></tr><tr>"
            If Session.Item("useMetricValues") Then
              temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>[F] FAA&nbsp;Field&nbsp;Length (" & TranslateUSMetricUnitsShort("FT") & "):</td></tr>"
            Else
              temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>[F] FAA&nbsp;Field&nbsp;Length (ft):</td></tr>"
            End If
          End If
          'amod_field_length, amod_max_range_miles
          temp_op_cost_string = temp_op_cost_string & "<tr><td><strong>Takeoff Performance</strong></td></tr><tr>"
          temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>SL&nbsp;ISA&nbsp;BFL:</td></tr><tr>"
          temp_op_cost_string = temp_op_cost_string & "<td class='Label' valign='middle' align='right'>5000'&nbsp;+20C&nbsp;BFL:</td></tr>"

          If Session.Item("useMetricValues") Then
            temp_op_cost_string = temp_op_cost_string & "<tr><td nowrap><strong>Range (" & TranslateUSMetricUnitsLong("NM") & ")</strong></td></tr>"


            If sAirframeType = "F" And Not bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(" & TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Tanks&nbsp;Full&nbsp;(" & TranslateUSMetricUnitsShort("NM") & "):</td></tr><tr>"
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Seats&nbsp;Full&nbsp;(" & TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>[F] Range&nbsp;(" & TranslateUSMetricUnitsShort("NM") & ") / [R] Tanks&nbsp;Full&nbsp;(" & TranslateUSMetricUnitsShort("NM") & "):</td></tr><tr>"
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>[R] Seats&nbsp;Full&nbsp;(" & TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
            End If
          Else
            ' l_objBuilder.Append("<tr><td nowrap><strong>Landing Field Length (ft)</strong></td></tr>" & vbCrLf)
            temp_op_cost_string = temp_op_cost_string & "<tr><td nowrap><strong>Range (Nautical Miles)</strong></td></tr>"

            If sAirframeType = "F" And Not bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(nm):</td></tr>"
            ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Tanks&nbsp;Full&nbsp;(nm):</td></tr>"
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>Seats&nbsp;Full&nbsp;(nm):</td></tr>"
            ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>[F] Range&nbsp;(nm) / [R] Tanks&nbsp;Full&nbsp;(nm):</td></tr>"
              temp_op_cost_string = temp_op_cost_string & "<tr><td class='Label' valign='middle' align='right'>[R] Seats&nbsp;Full&nbsp;(nm):</td></tr>"
            End If
          End If
          l_objBuilder.Append(temp_op_cost_string)
          string_from_op_costs_for_range = string_from_op_costs_for_range & temp_op_cost_string & "</table></td><td width='15%'><table width='100%'>"
          temp_op_cost_string = ""
          l_objBuilder.Append("<tr><td nowrap><strong>Engines</strong></td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Number&nbsp;of:</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='top' align='right'>Model(s):" & vbCrLf)

          number_of_engine_types = GetEnginesNumberForSpace(amod_id, nMAXEngines)

          For iLoop = 1 To number_of_engine_types - 1
            l_objBuilder.Append("<br>&nbsp;" & vbCrLf)
          Next

          l_objBuilder.Append("</td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Thrust&nbsp;(" & TranslateUSMetricUnitsShort("LBS") & "&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Shaft&nbsp;(" & TranslateUSMetricUnitsShort("HP") & "&nbsp;per&nbsp;Engine):</td></tr><tr>")
          Else
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Thrust&nbsp;(lbs&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Shaft&nbsp;(hp&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
          End If

          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Common&nbsp;TBO&nbsp;Hours:</td></tr>" & vbCrLf)
          ''''''''''''''''''''''''''''''''''''''''''''''''''''
          ' End the break here for the other column
          ''''''''''''''''''''''''''''''''''''''''''''''''''''

          l_objBuilder.Append("</table></td>" & vbCrLf)

          '''''''''''''''''''''''''''''''''''''''''''''''''''
          ' Start the right column
          '''''''''''''''''''''''''''''''''''''''''''''''''''
          l_objBuilder.Append("<td valign='top' align='left' class='Performance_Specs_Right_TD_2'>")
          l_objBuilder.Append("<table cellpadding='2' cellspacing='0'>" & vbCrLf)

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          If sAirframeType = "F" Then
            If Session.Item("useMetricValues") Then
              If Not IsDBNull(l_AdoRs.Item("amod_stall_vs")) Then
                If CDbl(l_AdoRs.Item("amod_stall_vs")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_stall_vs"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              If Not IsDBNull(l_AdoRs.Item("amod_stall_vs")) Then
                If CDbl(l_AdoRs.Item("amod_stall_vs")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_stall_vs")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            End If
          ElseIf sAirframeType = "R" And bHasManyAirFrames Then
            l_objBuilder.Append("<td valign='middle' align='right'>&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If sAirframeType = "F" Then
            If Session.Item("useMetricValues") Then
              If Not IsDBNull(l_AdoRs.Item("amod_stall_vso")) Then
                If CDbl(l_AdoRs.Item("amod_stall_vso")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_stall_vso"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              If Not IsDBNull(l_AdoRs.Item("amod_stall_vso")) Then
                If CDbl(l_AdoRs.Item("amod_stall_vso")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_stall_vso")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            End If
          ElseIf sAirframeType = "R" And bHasManyAirFrames Then
            l_objBuilder.Append("<td valign='middle' align='right'>&nbsp;</td></tr><tr>" & vbCrLf)
          End If

          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_cruis_speed"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_max_speed"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_cruis_speed")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_speed")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
          End If

          If sAirframeType = "F" And bHasManyAirFrames Then

            l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" Then

            l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>")

            If Not IsDBNull(l_AdoRs.Item("amod_ifr_certification")) Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_ifr_certification") & "&nbsp;</td></tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>Unknown</td></tr>" & vbCrLf)
            End If
          End If

          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          If Session.Item("useMetricValues") Then
            If CDbl(l_AdoRs.Item("amod_climb_normal_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FPM", CDbl(l_AdoRs.Item("amod_climb_normal_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If CDbl(l_AdoRs.Item("amod_climb_engout_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FPM", CDbl(l_AdoRs.Item("amod_climb_engout_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If CDbl(l_AdoRs.Item("amod_ceiling_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_ceiling_feet"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr>" & vbCrLf)
            End If

          Else
            If CDbl(l_AdoRs.Item("amod_climb_normal_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_normal_feet")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If CDbl(l_AdoRs.Item("amod_climb_engout_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_engout_feet")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If CDbl(l_AdoRs.Item("amod_ceiling_feet")) > 0 Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_ceiling_feet")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr>" & vbCrLf)
            End If
          End If

          If sAirframeType = "F" And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
            l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" Then

            If Not IsDBNull(l_AdoRs.Item("amod_climb_hoge")) Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_climb_hoge"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_hoge")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_climb_hige")) Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_climb_hige"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_hige")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
            End If

          End If

          If sAirframeType = "F" Then

            temp_op_cost_string = temp_op_cost_string & "<tr><td>&nbsp;</td></tr>" & vbCrLf
            If Session.Item("useMetricValues") Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_field_length"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
              field_length = CDbl(l_AdoRs.Item("amod_field_length"))
            Else
              temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_field_length")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
              field_length = CDbl(l_AdoRs.Item("amod_field_length"))
            End If
          ElseIf sAirframeType = "R" And bHasManyAirFrames Then

            temp_op_cost_string = temp_op_cost_string & "<tr><th>&nbsp;</th></tr>"
            temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf
          End If

          temp_op_cost_string = temp_op_cost_string & "<tr><td>&nbsp;</td></tr>" & vbCrLf

          temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_takeoff_ali")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
          temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_takeoff_500")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf

          '  l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          'If sAirframeType = "F" Then
          '  If Session.item("useMetricValues") Then
          '    l_objBuilder.Append("<td valign='middle' align='left'>" & FormatNumber(ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_max_range_miles"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
          '  Else
          '    l_objBuilder.Append("<td valign='middle' align='left'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_range_miles")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
          '  End If
          'ElseIf sAirframeType = "R" Then
          '  If Session.item("useMetricValues") Then
          '    l_objBuilder.Append("<td valign='middle' align='left'>" & FormatNumber(ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_tanks_full"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
          '  Else
          '    l_objBuilder.Append("<td valign='middle' align='left'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_range_tanks_full")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
          '  End If
          'End If

          If sAirframeType = "F" And bHasManyAirFrames Then
            temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf
          ElseIf sAirframeType = "R" Then
            If Session.Item("useMetricValues") Then
              temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_seats_full"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
            Else
              temp_op_cost_string = temp_op_cost_string & "<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_range_seats_full")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
            End If
          End If

          ' THIS IS FOR LANDING FIELD LENGTH SECTION - OTHER SECTIONS ABOVE - THIS IS ALSO IN THEORY THE RANGE SECTION
          '   l_objBuilder.Append("<tr><td>&nbsp;" & CLng(l_AdoRs.Item("amod_field_length")) & "</td></tr>" & vbCrLf)
          temp_op_cost_string = temp_op_cost_string & "<tr><td>&nbsp;</td></tr>" & vbCrLf
          range_constant = CLng(l_AdoRs.Item("amod_max_range_miles"))
          temp_op_cost_string = temp_op_cost_string & "<tr><td align='right'>&nbsp;" & CLng(l_AdoRs.Item("amod_max_range_miles")) & "</td></tr>" & vbCrLf

          l_objBuilder.Append(temp_op_cost_string)
          string_from_op_costs_for_range = string_from_op_costs_for_range & temp_op_cost_string
          temp_op_cost_string = ""
          l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr>" & vbCrLf)

          l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_number_of_engines")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td valign='middle' align='right' nowrap>" & GetEngines(l_AdoRs.Item("amod_id"), nMAXEngines) & "</td></tr><tr>" & vbCrLf)


          If Session.Item("useMetricValues") Then
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(ConvertUSToMetricValue("LBS", l_AdoRs.Item("amod_engine_thrust_lbs"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            If Not IsDBNull(l_AdoRs.Item("amod_engine_shaft")) Then
              If CDbl(l_AdoRs.Item("amod_engine_shaft")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(ConvertUSToMetricValue("HP", l_AdoRs.Item("amod_engine_shaft"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          Else
            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_thrust_lbs")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            If Not IsDBNull(l_AdoRs.Item("amod_engine_shaft")) Then
              If CDbl(l_AdoRs.Item("amod_engine_shaft")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_shaft")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(0, False, False, True) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If
          End If

          l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_com_tbo_hrs")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
          '''''''''''''''''''''''''''''''''''''''''''''''''''
          ' End the left column
          '''''''''''''''''''''''''''''''''''''''''''''''''''

          l_objBuilder.Append("</table></td>" & vbCrLf)

        End If
      Next
      ' add the spacer to move the footer down
      l_objBuilder.Append("</tr><tr>" & vbCrLf)
      l_objBuilder.Append("<td  align='center'  id='tdInnerTableSetup' colspan='1' >" & vbCrLf)
      l_objBuilder.Append("&nbsp;" & vbCrLf)
      l_objBuilder.Append("</td>" & vbCrLf)
      l_objBuilder.Append("</tr>" & vbCrLf)

      l_objBuilder.Append("</table>" & vbCrLf)

      l_AdoRs.Close()
      l_AdoRs = Nothing
      Build_PerformanceSpecifications = l_objBuilder.ToString()


    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  Public Function Build_OperatingCosts(ByVal amod_id As Integer, ByVal make_model_name As String) As String
    Build_OperatingCosts = ""


    Dim DCRecCount, AFRecCount, ABRecCount, TCND, TCSM, TCST, ANCH, nCount, sTitle
    Dim bFirstOne As Boolean = True
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
    Dim fuelTotCost As Double = 0
    Dim sCurrencyDate = ""
    Dim nRememberSessionTimeout As Long = 0


    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs As System.Data.SqlClient.SqlDataReader : localAdoRs = Nothing

    Dim Query As String : Query = ""
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString
      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
      SqlCommand.CommandText = Query
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Build_OperatingCosts(ByVal amod_id As Integer, ByVal make_model_name As String) As String</b><br />" & Query
      Dim l_objBuilder As New StringBuilder
      Dim nNumberOfEngines As Integer = 0
      Dim nRememberModelID As Long = 0
      Dim nMAXEngines As Long = 0
      Dim nRows As Long = 0
      localAdoRs = SqlCommand.ExecuteReader()

      If localAdoRs.HasRows Then
        ' EXTRA QUERY TAKEN OUT
        localAdoRs.Read()
        'End If
      Else
        Build_OperatingCosts = ""
        Exit Function
      End If
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


      ' BEGINNING SECTION HERE PICTURE AND TITLE-----------------------------------------------------------------------------
      '     Build_OperatingCosts = Build_OperatingCosts & "<table align='center' border='0' cellpadding='2' cellspacing='0' >"
      '     Build_OperatingCosts = Build_OperatingCosts & "<tr><td colspan='5' align='center'><b>" & make_model_name & "&nbsp;Operating Costs (" & Trim(sTitle) & ")</b></td></tr>"
      ' BEGINNING SECTION HERE PICTURE AND TITLE-----------------------------------------------------------------------------


      ' THIS IS BEGGINNING OF LEFT COLUMN -----------------------------------------------------------------------------
      '    Build_OperatingCosts = Build_OperatingCosts & "<tr>"
      '    Build_OperatingCosts = Build_OperatingCosts & "<td colspan='2' width='49%' align='left' class='Operating_Costs_TD_Top_1'>"

      ' THIS IS BEGGINNING OF LEFT COLUMN -----------------------------------------------------------------------------

      '   ------------------------------------------------------- THIS IS THE BEGGINING OF THE LEFT SIDE INFO -----------------------------------------------------------------
      ''''''''''''''''''''''''''''''''''''''''
      ' Starts the DIRECT COSTS PER HOUR 
      ''''''''''''''''''''''''''''''''''''''''
      '   Build_OperatingCosts = Build_OperatingCosts & "<td valign='top' align='left'>"
      Build_OperatingCosts = Build_OperatingCosts & "<table border='0' cellpadding='2' width='90%' cellspacing='0' align='center'><tr valign='top' align='center' class='aircraft_list'><td align=left colspan='3'>" & vbCrLf
      Build_OperatingCosts = Build_OperatingCosts & "<b>DIRECT COSTS PER HOUR</b></td></tr><tr>"

      ' Build_OperatingCosts = Build_OperatingCosts & "<td></td>"


      ' Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td align='left' width='50%'><u>Fuel</u></td>" & vbCrLf

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
      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Fuel Cost Per " & TranslateUSMetricUnitsLong("GAL") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Fuel Cost Per Gallon</td>" & vbCrLf
      End If

      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

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


          Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(fuelGalCost, 2, True, False, True) & "</td>" & vbCrLf
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
          Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(fuelGalCost, 2, True, False, True) & "</td>" & vbCrLf
          ' ok update the excell report with our item and value
          'Call UpdateExcelReport(rngDirectCosts, "FuelCost", fuelGalCost)
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
          ' ok update the excell report with our item and value
          'Call UpdateExcelReport(rngDirectCosts, "FuelCost", 0)
        End If
      End If


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;" & TranslateUSMetricUnitsLong("GAL") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;Gallon</td>" & vbCrLf
      End If

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

      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Burn Rate (" & TranslateUSMetricUnitsLong("GAL") & "s Per Hour)</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Burn Rate (Gallons Per Hour)</td>" & vbCrLf
      End If

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td><u>Maintenance</u></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Labor Cost Per Hour</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Parts Per Hour Cost</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>Engine Overhaul</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>Thrust Reverse Overhaul</td>" & vbCrLf


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


      Build_OperatingCosts = Build_OperatingCosts & "<td><u>Miscellaneous&nbsp;Flight&nbsp;Expenses</u></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td> &nbsp;&nbsp;&nbsp;&nbsp;Landing-Parking Fee</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Crew Expenses</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Supplies-Catering</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td><b>Total Direct Costs</b></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td><br>Block&nbsp;Speed&nbsp;" & TranslateUSMetricUnitsLong("SM") & "s&nbsp;Per&nbsp;Hour</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td><br>Block&nbsp;Speed&nbsp;Statute&nbsp;Miles&nbsp;Per&nbsp;Hour</td>" & vbCrLf
      End If

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


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>Total Cost Per " & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>Total Cost Per Statute Mile</td>" & vbCrLf
      End If

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
      '   Build_OperatingCosts = Build_OperatingCosts & "<table border='0' cellpadding='2' width='90%' cellspacing='0' align='center'>"
      Build_OperatingCosts = Build_OperatingCosts & "<tr><td>&nbsp;</td></tr><tr class='aircraft_list'><td align='left' colspan='3'><b>ANNUAL FIXED COSTS</b></td></tr><tr>"
      ' EXTRA QUERY TAKEN OUT RTW/MSW 8/23
      bFirstOne = True
      nCount = 3


      'Build_OperatingCosts = Build_OperatingCosts & "<td valign='top' align='left' class='Operating_Costs_TD_Bottom'>"
      '  Build_OperatingCosts = Build_OperatingCosts & "<td width='50%' colspan='3'><table border='0' cellpadding='2' cellspacing='0'><tr>" & vbCrLf


      'Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"


      '   Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'><u>Crew Salaries</u></td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Capt. Salary</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Co-pilot Salary</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Benefits</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>Hangar Cost</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td><u>Insurance</u></td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Hull</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Legal Liability</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td><u>Misc. Overhead</u></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Training</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td> &nbsp;&nbsp;&nbsp;&nbsp;Modernization</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Nav. Equipment</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>Depreciation</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td><b>Total Fixed Costs</b></td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "<tr class='aircraft_list'>" & vbCrLf


      ' Build_OperatingCosts = Build_OperatingCosts & "<td></td>"

      Build_OperatingCosts = Build_OperatingCosts & "<td colspan='3'>&nbsp;&nbsp;<b>ANNUAL BUDGET</b></td></tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Number of Seats</td>" & vbCrLf

      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
      If Not IsDBNull(localAdoRs("amod_number_of_seats")) Then
        Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & FormatNumber(System.Math.Round(CDbl(localAdoRs("amod_number_of_seats")), 0), 0, True, False, True) & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
      End If

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;" & TranslateUSMetricUnitsLong("M") & "s</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Miles</td>" & vbCrLf
      End If

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Hours</td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b>Total Direct Costs</b></td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b>Total Fixed Costs</b></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b><u>Total Cost (Fixed & Direct)</u></b></td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Hour</td>" & vbCrLf

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


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/" & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Statute Mile</td>" & vbCrLf
      End If

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

      Build_OperatingCosts = Build_OperatingCosts & "  </tr><tr>" & vbCrLf


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat " & TranslateUSMetricUnitsLong("M") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat Mile</td>" & vbCrLf
      End If

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
      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b><u>Total Cost (No Depreciation)</u></b></td>" & vbCrLf

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


      Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Hour</td>" & vbCrLf

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

      Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/" & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Statute Mile</td>" & vbCrLf
      End If

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


      If Session.Item("useMetricValues") Then
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat " & TranslateUSMetricUnitsLong("M") & "</td>" & vbCrLf
      Else
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat Mile</td>" & vbCrLf
      End If

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

      ' THIS ENDS THE INITIAL ROW CONTAINING ALL INFO AND THEN STARTS NEW ROW-------------------------------------------------------
      '  Build_OperatingCosts = Build_OperatingCosts & "</tr>"
      '  Build_OperatingCosts = Build_OperatingCosts & "<tr>"
      '-----------------------------------------------------------------------------------------------------------------------------

      ' THIS IS THE LAST COLUMN -------------------------------------------------------------------
      ' Build_OperatingCosts = Build_OperatingCosts & "<td colspan='5' class='Operating_Costs_TD_Bottom'>&nbsp;</td></tr></table>"
      'Build_OperatingCosts = Build_OperatingCosts & "<td colspan='5' class='Operating_Costs_TD_Top_1'>&nbsp;</td></tr></table>"
      ' THIS IS THE LAST COLUMN -------------------------------------------------------------------

      bFirstOne = False

    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY------------------------------



  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  Public Function Build_RecentRetailSales(ByVal inModelID As Integer) As String
    Build_RecentRetailSales = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim Query As String : Query = ""
    Dim row_count As Integer = 0

    Dim nRememberTimeout As Integer = 0
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    Dim GetRetailSalesInfo As String = ""
    Try
      GetRetailSalesInfo = "No Sales at this time, for this Make/Model ..."

      Query = "SELECT TOP 30 journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, amod_make_name"
      Query = Query & " FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id"
      Query = Query & " INNER JOIN journal_category WITH(NOLOCK) ON journ_subcategory_code = jcat_subcategory_code WHERE amod_id = " & inModelID
      Query = Query & " AND (jcat_category_code = 'AH') and (journ_subcat_code_part1='WS') "

      Query = Query & " and (journ_subcat_code_part3 NOT IN ('DB','DS','FI','MF','FY','RE','IT','RR'))"
      Query = Query & " and journ_date >='" & DateAdd(DateInterval.Month, -6, Date.Now) & "' "
      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)
      Query = Query & " ORDER BY journ_date DESC"


      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Build_RecentRetailSales(ByVal inModelID As Integer) As String</b><br />" & Query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query

      adors = SqlCommand.ExecuteReader()

      If adors.HasRows Then

        GetRetailSalesInfo = "<table width='100%' align='center' cellpadding='0' cellspacing='0' valign='top'><tr class='aircraft_list'><td align='center' colspan='6'><b>Recent Retail Sales (Last 30 Records)</b></td></tr>" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "<tr><td><table align='center' border='0' id='forSaleInnerTable' width='100%' cellpadding=0' cellspacing='0'><tr class='aircraft_list'><td><strong>Date</strong></td><td class='table_specs'><strong>Transaction Info</strong></td></tr>" & vbCrLf '"<td class='table_specs'><strong>Serial #</strong></td><td class='table_specs'><strong>Reg #</strong></td><td class='table_specs'><strong>Year MFR</strong></td></tr>" & vbCrLf

        Do While adors.Read

          If row_count = 1 Then
            GetRetailSalesInfo = GetRetailSalesInfo & "<tr class='alt_row'>" & vbCrLf
            row_count = 0
          Else
            GetRetailSalesInfo = GetRetailSalesInfo & "<tr bgcolor='white'>" & vbCrLf
            row_count = 1
          End If
          GetRetailSalesInfo = GetRetailSalesInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'><em>" & adors.Item("journ_date") & "</em></td>" & vbCrLf
          GetRetailSalesInfo = GetRetailSalesInfo & "<td nowrap='nowrap' class='table_specs'>" & Left(adors.Item("journ_subject"), 70) & vbCrLf

          GetRetailSalesInfo = GetRetailSalesInfo & "<div class='aircraft_name_format'>"
          'added on 5-3-2012
          'This is basically to condense the listing on the recent (retail) sales tab.
          If Not IsDBNull(adors.Item("ac_ser_no_full")) Then
            If Not String.IsNullOrEmpty((adors.Item("ac_ser_no_full").ToString)) Then
              GetRetailSalesInfo = GetRetailSalesInfo & "<span>Ser #</span> <a href='/details.aspx?ac_ID=" & adors.Item("ac_id") & "&source=JETNET&type=3'>" & adors.Item("ac_ser_no_full") & "</a>"
            End If
          End If

          If Not IsDBNull(adors.Item("ac_reg_no")) Then
            If Not String.IsNullOrEmpty((adors.Item("ac_reg_no").ToString)) Then
              GetRetailSalesInfo = GetRetailSalesInfo & " Reg # " & adors.Item("ac_reg_no")
            End If
          End If
          If Not IsDBNull(adors.Item("ac_mfr_year")) Then
            If Not String.IsNullOrEmpty((adors.Item("ac_mfr_year").ToString)) Then
              GetRetailSalesInfo = GetRetailSalesInfo & " Year: " & adors.Item("ac_mfr_year")
            End If
          End If

          GetRetailSalesInfo = GetRetailSalesInfo & "</div></td>" & vbCrLf

          'GetRetailSalesInfo = GetRetailSalesInfo & "<td nowrap='nowrap' class='table_specs' align='center'><a href='/details.aspx?ac_ID=" & adors.Item("ac_id") & "&source=JETNET&type=3'>" & adors.Item("ac_ser_no_full") & "</a></td>" & vbCrLf
          'GetRetailSalesInfo = GetRetailSalesInfo & "<td nowrap='nowrap' class='table_specs'>" & adors.Item("ac_reg_no") & "</td>" & vbCrLf
          'GetRetailSalesInfo = GetRetailSalesInfo & "<td nowrap='nowrap' class='table_specs' align='center'>" & adors.Item("ac_mfr_year") & "</td>" & vbCrLf
          GetRetailSalesInfo = GetRetailSalesInfo & "</tr>" & vbCrLf
        Loop

        ' add the spacer to move the footer down
        GetRetailSalesInfo = GetRetailSalesInfo & "</table></td></tr><tr>" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "<td  align='center'  id='tdInnerTableSetup' colspan='1' >" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "&nbsp;" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "</td>" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "</tr>" & vbCrLf

        GetRetailSalesInfo = GetRetailSalesInfo & "</table>" & vbCrLf
      Else

        GetRetailSalesInfo = "<table width='90%' align='center' cellpadding='1' cellpadding='1'><tr><td align='center' colspan='6'></td></tr><tr><td align='center' colspan='6'><b>" & make_model_name & " Recent Retail Sales</b></td></tr><tr><td align='center' colspan='6'>&nbsp;</td></tr>" & vbCrLf
        ' GetRetailSalesInfo = GetRetailSalesInfo & "<tr><td><table align='center' border='1' id='forSaleInnerTable' width='90%' cellpadding='1' cellspacing='0' align='center'><tr><td class='table_specs'><strong>Date</strong></td><td class='table_specs'><strong>Transaction Info</strong></td><td class='table_specs'><strong>Serial #</strong></td><td class='table_specs'><strong>Reg #</strong></td><td class='table_specs'><strong>Year MFR</strong></td></tr>" & vbCrLf
        ' GetRetailSalesInfo = GetRetailSalesInfo & "</table></td></tr><tr><td align='center'>No " & make_model_name & " Recent Retail Sales Available" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "<tr><td align='center'>No " & make_model_name & " Recent Retail Sales Available" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "<td  align='center'  id='tdInnerTableSetup' colspan='1' >" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "&nbsp;" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "</td>" & vbCrLf
        GetRetailSalesInfo = GetRetailSalesInfo & "</tr>" & vbCrLf

        GetRetailSalesInfo = GetRetailSalesInfo & "</table>" & vbCrLf
      End If

      adors = Nothing
      Build_RecentRetailSales = GetRetailSalesInfo
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function Build_RecentMarketActivity(ByVal singleViewModelID As Integer) As String
    Build_RecentMarketActivity = ""
    Try
      Dim objBuilder As New StringBuilder
      objBuilder.Append("<table align='center' border='0'  id='forSaleInnerTable' width='100%' cellpadding='0' cellspacing='0' valign='top'>")
      objBuilder.Append("<tr valign='top'><td valign='top' class='aircraft_list' align='center' colspan='7' width='100%'><strong>Recent Market Activity (Last 30 Records)</strong> </td></tr>" & vbCrLf) '<em>(last 20 events)</em>

      objBuilder.Append("<tr valign='top'><td align='center' colspan='7' width='100%'>" & vbCrLf & GetModelEventsInfo(singleViewModelID) & vbCrLf)

      objBuilder.Append("</td></tr>" & vbCrLf)
      objBuilder.Append("</table>" & vbCrLf)
      Build_RecentMarketActivity = objBuilder.ToString
    Catch ex As Exception

    End Try
  End Function
  Function GetModelEventsInfo(ByVal inModelID)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim query As String : query = ""
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    Dim row_count As Integer = 0
    Try
      GetModelEventsInfo = "No Market Status Events at this time, for this Make/Model ..."

      query = "SELECT TOP 30 priorev_entry_date, ac_mfr_year, ac_ser_no_full, ac_reg_no, ac_id, priorev_subject, priorev_description, amod_make_name FROM Priority_Events WITH(NOLOCK)"
      query = query & " INNER JOIN Priority_Events_category WITH(NOLOCK) ON priorevcat_category_code = priorev_category_code INNER JOIN aircraft WITH(NOLOCK) ON"
      query = query & " priorev_ac_id = ac_id AND ac_journ_id = 0 INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      query = query & " WHERE amod_id = " & inModelID & " AND priorevcat_category = 'Market Status'"

      If Session.Item("Aerodex") Then ' NOTE : need to update clause if new market status types are added
        query = query & " AND priorev_category_code NOT IN ('CA','EXOFF','EXON','MA','OM','OMNS','SALEP','SC','SPTOIM')"
      End If

      query = query & " and priorev_entry_date > '" & DateAdd(DateInterval.Month, -6, Date.Now) & "' "


      ' if line is taken out, seems to have no effect, but leaving in 
      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)
      query = query & " ORDER BY priorev_id DESC"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetModelEventsInfo(ByVal inModelID)</b><br />" & query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adors = SqlCommand.ExecuteReader()
      'Dim temp_trim_desc As String

      If adors.HasRows Then
        '
        GetModelEventsInfo = vbCrLf & "<table  align='center' border='0' id='forSaleInnerTable' width='100%' cellpadding='0' cellspacing='0' valign='top'> "
        GetModelEventsInfo = GetModelEventsInfo & vbCrLf & "<tr class='aircraft_list'><td class='table_specs'><b>Date:</b></td><td class='table_specs'><b>Status</b></td><td class='table_specs'><b>Serial#</b></td><td class='table_specs'><b>Reg#</b></td><td class='table_specs'><b>Year MFR</b></td><td class='table_specs'><b>Event Desc</b></td></tr>" & vbCrLf

        Do While adors.Read

          If row_count = 1 Then
            GetModelEventsInfo = GetModelEventsInfo & "<tr class='alt_row'>" & vbCrLf
            row_count = 0
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<tr bgcolor='white'>" & vbCrLf
            row_count = 1
          End If


          If Not IsDBNull(adors.Item("priorev_entry_date")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'><em>" & FormatDateTime(adors.Item("priorev_entry_date"), 2) & "</em></td>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'><em>&nbsp;</em></td>" & vbCrLf
          End If
          If Not IsDBNull(adors.Item("priorev_subject")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>" & adors.Item("priorev_subject") & "</td>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td>" & vbCrLf
          End If
          If Not IsDBNull(adors.Item("ac_ser_no_full")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='center' valign='top' nowrap='nowrap' class='table_specs'><a href='/details.aspx?ac_ID=" & adors.Item("ac_id") & "&source=JETNET&type=3'>" & adors.Item("ac_ser_no_full") & "</a></td>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='center' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td>" & vbCrLf
          End If
          If Not IsDBNull(adors.Item("ac_reg_no")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>" & adors.Item("ac_reg_no") & "</td>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td>" & vbCrLf
          End If
          If Not IsDBNull(adors.Item("ac_mfr_year")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='center' valign='top' nowrap='nowrap' class='table_specs'>" & adors.Item("ac_mfr_year") & "</td>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='center' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td>" & vbCrLf
          End If
          If Not IsDBNull(adors.Item("priorev_description")) Then
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>" & Left(adors.Item("priorev_description").ToString, 45).ToString & "</td></tr>" & vbCrLf
            'GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td></tr>" & vbCrLf
          Else
            GetModelEventsInfo = GetModelEventsInfo & "<td align='left' valign='top' nowrap='nowrap' class='table_specs'>&nbsp;</td></tr>" & vbCrLf
          End If

        Loop
        adors = Nothing
        GetModelEventsInfo = GetModelEventsInfo & "</table>"

      End If
    Catch ex As Exception
      GetModelEventsInfo = ""
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function Build_AircraftForSale(ByVal inModelID As Integer, ByVal InSortBy As String, ByVal bUseHeight As Boolean, ByVal table_height As String) As String
    Build_AircraftForSale = ""
    Dim outStr As String = ""
    Dim forSaleCount As Long = 0
    Dim bHadStatus As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim adoOwnerRs As System.Data.DataSet
    Dim query As String : query = ""

    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    Dim row_count As Integer = 0
    outStr = ""
    forSaleCount = 0
    Dim GetForSaleInfo As String = ""
    GetForSaleInfo = "No ForSale Information at this time, for this Make/Model ..."
    Try


      query = "SELECT ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_mfr_year, ac_airframe_tot_hrs, ac_status, ac_asking, ac_asking_price, ac_id, ac_journ_id, amod_make_name"
      query = query & " FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id WHERE amod_id = " & CStr(inModelID) & " AND ac_journ_id = 0"
      'query = query & commonEVO.GenerateProductCodeSelectionQuery("", "", False, False, False)
      query = query & " AND ac_forsale_flag = 'Y'"
      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)
      Select Case (InSortBy)
        Case "serno"
          query = query & " ORDER BY ac_ser_no_sort, ac_reg_no, ac_airframe_tot_hrs, ac_mfr_year"

        Case "regno"
          query = query & " ORDER BY ac_reg_no, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year"

        Case "aftt"
          query = query & " ORDER BY ac_airframe_tot_hrs, ac_ser_no_sort, ac_reg_no, ac_mfr_year"

        Case "mfryear"
          query = query & " ORDER BY ac_mfr_year, ac_ser_no_sort, ac_reg_no, ac_airframe_tot_hrs"

        Case Else
          query = query & " ORDER BY ac_ser_no_sort, ac_reg_no, ac_airframe_tot_hrs, ac_mfr_year"

      End Select
      GetForSaleInfo = ""
      If Not bUseHeight Then
        GetForSaleInfo = GetForSaleInfo & "<table id='forSaleOuterTable' width='100%' cellspacing='0' cellpadding='0'>"
      Else
        GetForSaleInfo = GetForSaleInfo & "<table id='forSaleOuterTable' width='100%' height='" & table_height & "' cellspacing='0' cellpadding='0' >"
      End If

      ' GetForSaleInfo = GetForSaleInfo & "<tr><td valign='top' align='center'><strong>" & make_model_name & " Aircraft For Sale</strong></td></tr><tr><td valign='top' align='center'>&nbsp;</td></tr>"
      'GetForSaleInfo = GetForSaleInfo & "<td class='border_bottom' width='20%' align='center'>&nbsp;</td>"

      'If Session.Item("debug") And Session.Item("convertToPDFPath") = "" Then
      'Session.Item("localUser").crmUser_DebugText += "<b>getForSaleInfo : " & Server.HtmlEncode(query) & "</b><br /><br />"
      'End If
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Build_AircraftForSale(ByVal inModelID As Integer, ByVal InSortBy As String, ByVal bUseHeight As Boolean, ByVal table_height As String) As String</b><br />" & query
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adors = SqlCommand.ExecuteReader()


      If adors.HasRows Then
        If adors.HasRows Then
          Do While adors.Read
            forSaleCount = forSaleCount + 1
          Loop
          If forSaleCount = 0 Then
            Exit Function
          Else
            adors.Close()
            SqlCommand.CommandText = query
            adors = SqlCommand.ExecuteReader()
            'adors.Read()
          End If
        Else
          Exit Function
        End If

        outStr = "<table border='0' calign='top' id='forSaleInnerTable' width='100%' cellpadding='0' cellspacing='0' align='center'><tr class='aircraft_list' valign='top'><td class='table_specs' width='15%'><strong>Serial#</strong></td><td class='table_specs'><strong>Reg#</strong></td><td class='table_specs' width='4%'><strong>Year MFR</strong></td><td class='table_specs'><strong>For Sale Status</strong></td><td class='table_specs' width='4%'><strong>Hours</strong></td><td class='table_specs'><strong>Owner/Exclusive Broker </strong></td></tr>" & vbCrLf
        Dim current_row As Integer = 0
        ' approximately limit 50 cells per page
        Do While adors.Read

          bHadStatus = False

          If row_count = 1 Then
            outStr = outStr & "<tr class='alt_row'>" & vbCrLf
            row_count = 0
          Else
            outStr = outStr & "<tr bgcolor='white'>" & vbCrLf
            row_count = 1
          End If
          outStr = outStr & "<td align='left' valign='top' class='table_specs'>" & vbCrLf
          outStr = outStr & "<a href='/details.aspx?ac_ID=" & adors.Item("ac_id") & "&source=JETNET&type=3'>"
          If Not IsDBNull(adors.Item("ac_ser_no_full")) Then

            If adors.Item("ac_ser_no_full") <> "" Then
              'outStr = outStr & "<font size='-1'>"
              outStr = outStr & adors.Item("ac_ser_no_full") & "</a></td><td class='table_specs'>"
            Else
              outStr = outStr & "&nbsp;</td><td class='table_specs'>"
            End If
          Else
            outStr = outStr & "&nbsp;</td><td class='table_specs'>"
          End If

          If Not IsDBNull(adors.Item("ac_reg_no")) Then
            If adors.Item("ac_reg_no") <> "" Then
              outStr = outStr & adors.Item("ac_reg_no") & "</td><td class='table_specs'>" & vbCrLf
            Else
              outStr = outStr & "&nbsp;</td><td class='table_specs'>" & vbCrLf
            End If
          Else
            outStr = outStr & "&nbsp;</td><td class='table_specs'>" & vbCrLf
          End If

          'outStr = outStr & adors.Item("ac_ser_no_full") & "</td><td>" & adors.Item("ac_reg_no") & "</td><td>" & vbCrLf

          If Not IsDBNull(adors.Item("ac_mfr_year")) Then
            If CDbl(adors.Item("ac_mfr_year")) = 0 Then
              outStr = outStr & "&nbsp;</td><td class='table_specs'>" & vbCrLf
            Else
              outStr = outStr & CStr(adors.Item("ac_mfr_year")) & "</td><td class='table_specs'>" & vbCrLf
            End If
          Else
            outStr = outStr & "&nbsp;</td><td class='table_specs'>" & vbCrLf
          End If

          If Not IsDBNull(adors.Item("ac_Status")) Then
            If Trim(adors.Item("ac_Status")) <> "" Then
              If LCase(adors.Item("ac_Status")) <> LCase("For Sale") Then
                outStr = outStr & Trim(adors.Item("ac_Status")) & " "
                bHadStatus = True
              End If
            End If
          End If

          If Not IsDBNull(adors.Item("ac_asking")) Then
            If Trim(adors.Item("ac_asking")) <> "Price" Then
              If bHadStatus Then
                outStr = outStr & Trim(adors.Item("ac_asking")) & "</td><td class='table_specs'>" & vbCrLf
              Else
                outStr = outStr & Trim(adors.Item("ac_asking")) & "</td><td class='table_specs'>" & vbCrLf
              End If
            Else
              If Not IsDBNull(adors.Item("ac_asking_price")) Then
                If CDbl(adors.Item("ac_asking_price")) > 0 Then
                  If bHadStatus Then
                    outStr = outStr & "$" & FormatNumber((adors.Item("ac_asking_price") / 1000), 0) & "k" & "</td><td class='table_specs'>" & vbCrLf
                  Else
                    outStr = outStr & "$" & FormatNumber((adors.Item("ac_asking_price") / 1000), 0) & "k" & "</td><td class='table_specs'>" & vbCrLf
                  End If
                End If
              End If
            End If
          Else
            outStr = outStr & "&nbsp;</td><td class='table_specs'>" & vbCrLf
          End If

          If Not IsDBNull(adors("ac_airframe_tot_hrs")) Then
            If CDbl(adors.Item("ac_airframe_tot_hrs")) = 0 Then
              outStr = outStr & "<em>0&nbsp;</em>" & "</td><td class='table_specs'>" & vbCrLf
            Else
              outStr = outStr & FormatNumber(adors.Item("ac_airframe_tot_hrs"), 0) & "&nbsp;" & "</td><td class='table_specs'>" & vbCrLf
            End If
          Else
            outStr = outStr & "<em>-&nbsp;</em>" & "</td><td class='table_specs'>" & vbCrLf
          End If
          Dim tmp_type As String
          ' exclusive info or show sequence 1 info
          adoOwnerRs = GetOwnerInfo(adors.Item("ac_id"), adors.Item("ac_journ_id"), True)
          'adoOwnerRs.Read()
          If Not IsDBNull(adoOwnerRs) Then
            If adoOwnerRs.Tables(0).Rows.Count > 0 Then
              tmp_type = GetContactTypeForContactID(adoOwnerRs.Tables(0).Rows(0).Item("cref_contact_type")).ToString.ToLower
              tmp_type = Char.ToUpper(tmp_type) + tmp_type.ToString.Substring(1)
              tmp_type = Replace(tmp_type, "b", "B")
              outStr = outStr & tmp_type & ": "
              outStr = outStr & "<a href='details.aspx?comp_id=" & adoOwnerRs.Tables(0).Rows(0).Item("comp_id") & "&source=JETNET&type=1'>"
              outStr = outStr & Trim(adoOwnerRs.Tables(0).Rows(0).Item("comp_name")) & vbCrLf
              outStr = outStr & "</a>"
            Else
              adoOwnerRs = GetOwnerInfo(adors.Item("ac_id"), adors.Item("ac_journ_id"), False)

              If Not IsDBNull(adoOwnerRs) Then
                If adoOwnerRs.Tables(0).Rows.Count > 0 Then
                  tmp_type = GetContactTypeForContactID(adoOwnerRs.Tables(0).Rows(0).Item("cref_contact_type")).ToString.ToLower
                  tmp_type = Char.ToUpper(tmp_type) + tmp_type.ToString.Substring(1)
                  tmp_type = Replace(tmp_type, "b", "B")
                  outStr = outStr & tmp_type & ": "
                  outStr = outStr & "<a href='details.aspx?comp_id=" & adoOwnerRs.Tables(0).Rows(0).Item("comp_id") & "&source=JETNET&type=1'>"
                  outStr = outStr & Trim(adoOwnerRs.Tables(0).Rows(0).Item("comp_name")) & vbCrLf
                  outStr = outStr & "</a>"

                End If
              End If
            End If
          Else
            adoOwnerRs = GetOwnerInfo(adors.Item("ac_id"), adors.Item("ac_journ_id"), False)

            If Not IsDBNull(adoOwnerRs) Then
              If adoOwnerRs.Tables(0).Rows.Count > 0 Then
                tmp_type = GetContactTypeForContactID(adoOwnerRs.Tables(0).Rows(0).Item("cref_contact_type")).ToString.ToLower
                tmp_type = Char.ToUpper(tmp_type) + tmp_type.ToString.Substring(1)
                tmp_type = Replace(tmp_type, "b", "B")
                outStr = outStr & tmp_type & ": "
                outStr = outStr & "<a href='details.aspx?comp_id=" & adoOwnerRs.Tables(0).Rows(0).Item("comp_id") & "&source=JETNET&type=1'>"
                outStr = outStr & Trim(adoOwnerRs.Tables(0).Rows(0).Item("comp_name")) & vbCrLf
                outStr = outStr & "</a>"
              End If
            End If
          End If

          adoOwnerRs = Nothing

          outStr = outStr & "</td></tr>" & vbCrLf



          current_row = current_row + 1
          '------------------------------------------------- THIS SECTION IS FOR ENDING OF PAGE---------------------

        Loop



        outStr = outStr & "</table>"

        'foot_space_ac_for_sale
        ' add the spacer to move the footer down
        'outStr = outStr & "</td></tr><tr>" & vbCrLf
        ' outStr = outStr & "<td  align='center'  id='tdInnerTableSetup' colspan='1'>" & vbCrLf
        'outStr = outStr & "&nbsp;" & vbCrLf

        'outStr = outStr & "</td>" & vbCrLf
        ' outStr = outStr & "</tr>" & vbCrLf
        ' outStr = outStr & "</table>" & vbCrLf


      Else
        outStr = "<table align='center' border='1' id='forSaleInnerTable' width='100%' cellpadding='1' cellspacing='0'><tr><td class='table_specs'><strong>Serial#</strong></td><td class='table_specs'><strong>Reg#</strong></td><td class='table_specs'><strong>Year MFR</strong></td><td class='table_specs'><strong>For Sale Status</strong></td><td class='table_specs' width='4%'><strong>Hours</strong></td><td class='table_specs'><strong>Owner</strong></td>" & vbCrLf
        outStr = outStr & "</tr><tr><td align='center'>No " & make_model_name & " Aircraft For Sale" & vbCrLf
        outStr = outStr & "</td>" & vbCrLf
        outStr = outStr & "</tr></table>" & vbCrLf
      End If

      If Not bUseHeight Then
        ' if more than 20 use big table
        If forSaleCount > 42 Then
          GetForSaleInfo = GetForSaleInfo & "<tr class='table_specs' valign='top'><td align='center' colspan='3' class='table_specs'>" & outStr & "</td></tr></table>" & vbCrLf
        Else
          GetForSaleInfo = GetForSaleInfo & "<tr class='table_specs' valign='top'><td align='center' colspan='3' class='table_specs'>" & outStr & "</td></tr></table>" & vbCrLf
        End If
      Else
        GetForSaleInfo = GetForSaleInfo & "<tr class='table_specs' valign='top'><td align='center' colspan='3' class='table_specs'>" & outStr & "</td></tr></table>" & vbCrLf
      End If

      ' GetForSaleInfo = GetForSaleInfo & "</td></tr></table>"

      Build_AircraftForSale = GetForSaleInfo
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function GetOwnerInfo(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal bGetExclusive As Boolean) As System.Data.DataSet

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim adoOwnerRs As New System.Data.SqlClient.SqlDataAdapter ': adoOwnerRs = Nothing
    Dim Query As String : Query = ""
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query

      GetOwnerInfo = Nothing

      Query = "SELECT * FROM Company WITH(NOLOCK)"
      Query = Query & " INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)"
      Query = Query & " LEFT OUTER JOIN Contact WITH(NOLOCK) ON (cref_contact_id = contact_id AND cref_journ_id = contact_journ_id)"
      Query = Query & " WHERE (cref_ac_id = " & CStr(nAircraftID) & " AND cref_journ_id = " & CStr(nAircraftJournalID)

      If Not bGetExclusive Then
        Query = Query & " AND cref_transmit_seq_no = 1 AND cref_contact_type <> '71'"
      Else
        Query = Query & " AND ((cref_contact_type = '99') OR (cref_contact_type = '93') OR (cref_transmit_seq_no = 4))"
      End If

      If nAircraftJournalID = 0 Then
        Query = Query & " AND comp_active_flag = 'Y'"
      End If

      Query = Query & " AND comp_hide_flag = 'N')"
      ' Query = Query & MakeCompanyProductCodeClause(False)  ' does nothing ? 
      SqlCommand.CommandText = Query
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetOwnerInfo(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal bGetExclusive As Boolean) As System.Data.DataSet</b><br />" & Query
      Dim x As New System.Data.DataSet
      adoOwnerRs.SelectCommand = SqlCommand
      adoOwnerRs.Fill(x) 'SqlCommand.BeginExecuteNonQuery
      GetOwnerInfo = x
      x.Dispose()
      x = Nothing
    Catch ex As Exception
      GetOwnerInfo = Nothing
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function GetContactTypeForContactID(ByVal inContactType)

    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim Query As String = ""
    Try
      Query = "SELECT actype_name FROM Aircraft_Contact_Type WITH(NOLOCK) WHERE (actype_code = '" & inContactType & "'"

      ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users
      If Session.Item("Aerodex") Then
        Query = Query & " AND actype_code NOT IN ('93','98','99','67','68','02'))"
      Else
        Query = Query & " AND actype_code NOT IN ('67','68','02'))"
      End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetContactTypeForContactID(ByVal inContactType)</b><br />" & Query
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query

      adors = SqlCommand.ExecuteReader()

      If adors.HasRows Then
        adors.Read()
        GetContactTypeForContactID = Replace(Trim(adors.Item("actype_name")), "Additional Contact1", "Additional Company")
        adors.Close()
      Else
        GetContactTypeForContactID = ""
      End If

      adors = Nothing
    Catch ex As Exception
      GetContactTypeForContactID = ""
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  Public Function display_avg_price_by_month_graph(ByVal inModelID, ByVal graphID) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim outstring, query, x
    Dim nRememberSQLTimeout As Integer = 0
    Dim adUseClient As Integer = 0
    Dim adUseServer As Integer = 0
    Dim adoRs = Nothing
    Dim this_counter As Integer = 1
    Dim high_number As Integer = 0
    Dim low_number As Integer = 100000000
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1
    outstring = ""
    display_avg_price_by_month_graph = ""
    x = 0
    Try


      query = "SELECT DISTINCT mtrend_year, mtrend_month, mtrend_total_aircraft_for_sale, mtrend_avg_asking_price"
      query = query & " FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_amod_id = " & CStr(inModelID)
      query = query & " AND (( mtrend_year = year(getdate()-182) AND mtrend_month >= month(getdate()-182) ) OR"
      query = query & " ( mtrend_year = year(getdate()) AND mtrend_month <= month(getdate()) ))"

      Dim type_of_subscription As String = ""
      If Session.Item("localSubscription").crmBusiness_Flag = True Then
        type_of_subscription = "B"
      ElseIf Session.Item("localSubscription").crmCommercial_Flag = True Then
        type_of_subscription = "C"
      ElseIf Session.Item("localSubscription").crmHelicopter_Flag = True Then
        type_of_subscription = "H"
      Else
        type_of_subscription = "B"
      End If

      query = query & " and mtrend_product_type = '" & type_of_subscription & "' "


      query = query & " ORDER BY mtrend_year ASC, mtrend_month ASC"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      ' End Select
      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_avg_price_by_month_graph(ByVal inModelID, ByVal graphID) As String</b><br />" & query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query

      adoRs = SqlCommand.ExecuteReader()


      If adoRs.HasRows Then
        Me.AVG_PRICE_MONTH.Series.Clear()
        Me.AVG_PRICE_MONTH.Series.Add("AVG_PRICE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").LabelForeColor = Drawing.Color.Blue
        Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Title = "Avg Asking Price-In Thousands US$"
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Color = Drawing.Color.Blue
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").BorderWidth = 1
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").MarkerSize = 5
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
        '  Me.AVG_PRICE_MONTH.BorderlineWidth = 10
        Me.AVG_PRICE_MONTH.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").SmartLabelStyle.Enabled = False
        'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").LabelAngle = 45
        Me.AVG_PRICE_MONTH.Width = 390
        Me.AVG_PRICE_MONTH.Height = 260

        Do While adoRs.Read
          If Not IsDBNull(adoRs("mtrend_year")) Then
            If adoRs("mtrend_year").ToString <> "" Then
              If Not IsDBNull(adoRs("mtrend_month")) Then
                If adoRs("mtrend_month").ToString <> "" Then
                  this_counter = adoRs("mtrend_year")
                  'outstring = outstring & " data.setValue(" & x & ", 0, '" & CStr(adoRs("mtrend_month")) & "-" & CStr(adoRs("mtrend_year")) & "');" & vbCrLf
                  If Not IsDBNull(adoRs("mtrend_avg_asking_price")) Then
                    If CDbl(adoRs("mtrend_avg_asking_price")) >= 0 Then

                      If CDbl(adoRs("mtrend_avg_asking_price")) > high_number Then
                        high_number = adoRs("mtrend_avg_asking_price")
                      End If
                      If CDbl(adoRs("mtrend_avg_asking_price")) < low_number Then
                        low_number = adoRs("mtrend_avg_asking_price")
                      End If


                      Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Points.AddXY((adoRs("mtrend_month") & "-" & adoRs("mtrend_year")), (adoRs("mtrend_avg_asking_price") / 1000))
                      '   Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Points.Item(x).Label = adoRs("mtrend_avg_asking_price")

                      'outstring = outstring & " data.setValue(" & x & ", 1, " & Format(adoRs("mtrend_avg_asking_price"), 2) & ");" & vbCrLf
                    Else
                      'outstring = outstring & " data.setValue(" & x & ", 1, 0);" & vbCrLf
                    End If
                  Else
                    'outstring = outstring & " data.setValue(" & x & ", 1, 0);" & vbCrLf
                  End If
                  x = x + 1
                End If
              End If
            End If
          End If

        Loop




        adoRs.close()
        adoRs.dispose()

        Dim ac_asking_price As Integer = 0
        Dim ac_asking_price_count As Integer = 0
        query = "SELECT ac_asking_price from aircraft WITH(NOLOCK) INNER JOIN aircraft_model on amod_id = ac_amod_id WHERE ac_amod_id = " & CStr(inModelID)
        query = query & " and ac_forsale_flag = 'Y' and ac_journ_id = 0 and ac_asking_price <> '' "
        query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)



        SqlCommand.CommandText = query
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_avg_price_by_month_graph(ByVal inModelID, ByVal graphID) As String</b><br />" & query

        adoRs = SqlCommand.ExecuteReader()
        If adoRs.HasRows Then

          Do While adoRs.read
            ac_asking_price = ac_asking_price + adoRs("ac_asking_price")
            ac_asking_price_count = ac_asking_price_count + 1
          Loop

          ac_asking_price = (ac_asking_price / ac_asking_price_count)

          Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Points.AddXY(Date.Now.Month & "-" & Date.Now.Year(), (ac_asking_price / 1000))

          Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Points.Last.Color = Drawing.Color.Black
          Me.AVG_PRICE_MONTH.Series("AVG_PRICE").Points.Last.BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

          If CDbl(ac_asking_price) > high_number Then
            high_number = ac_asking_price
          End If
          If CDbl(ac_asking_price) < low_number Then
            low_number = ac_asking_price
          End If


        End If


        If low_number > 5 Then
          starting_point = (low_number / 5) - 1
          starting_point = starting_point * 5
        End If




        If high_number - low_number > 1000000 And high_number - low_number < 10000000 Then ' one million - count by 500 thousand
          interval_point = 500000
          starting_point = (low_number / interval_point) - 1
          starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 10000000 And high_number - low_number < 50000000 Then ' ten million to 50 million count by 5 million
          interval_point = 5000000
          starting_point = (low_number / interval_point) - 1
          starting_point = starting_point * interval_point
        ElseIf high_number - low_number > 50000000 Then ' gap greater then 50 million count by 10 million
          interval_point = 10000000
          starting_point = (low_number / interval_point) - 1
          starting_point = starting_point * interval_point
        ElseIf high_number - low_number < 100000 Then ' one hundred thousand - count by 10 thousand 
          interval_point = 10000
          starting_point = (low_number / interval_point) - 1
          starting_point = starting_point * interval_point
        Else
          interval_point = 200000   ' inbetween 100 thousand and one million gap  - to be determined later
          starting_point = (low_number / interval_point) - 1
          starting_point = starting_point * interval_point
        End If

        interval_point = FormatNumber(interval_point / 1000, 0)
        starting_point = FormatNumber(starting_point / 1000, 0)
        high_number = FormatNumber(high_number / 1000, 0)


        Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = high_number + interval_point
        If low_number = 0 Then
          Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0
        Else
          Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
        End If
        Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Interval = interval_point

        'Me.AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0

      Else
        outstring = ""
      End If

      outstring = "Avg Price By Month (past 6 months)"


      display_avg_price_by_month_graph = Trim(outstring)
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function

  Function displayOperatorCompanies(ByVal real_company_name, ByVal real_model_name, ByVal product_code, ByVal engine_name, ByVal amod_id, ByVal comp_id, ByVal sub_info, ByVal sub_type)
    displayOperatorCompanies = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim query As String = ""
    Dim final_totals As Integer = 0
    Dim company_information As String = ""
    Dim type_of_search As String = ""
    Dim tmp_title_holder As String = ""
    Dim strBuilder As New StringBuilder
    Dim totaltot As Integer = 0
    Dim inservicetot As Integer = 0
    Dim onordertot As Integer = 0
    Dim leasedtota As Integer = 0
    Dim retiredtot As Integer = 0
    Dim inserviceshowtot As Integer = 0
    Dim onordershowtot As Integer = 0
    Dim leasedshowtot As Integer = 0
    Dim retiredshowtot As Integer = 0
    Dim retiredshowtot2 As Integer = 0
    Dim retiredtot2 As Integer = 0
    Dim total_total_line As Integer = 0
    Dim leasedtot As Integer = 0
    Dim nColspan As Integer = 0
    Dim countcompany As Integer = 0
    Dim bgcolor As String = ""
    Dim total As Integer = 0
    Dim runningtotal As Integer = 0
    Dim table_name As String = ""
    Dim title As String = ""
    Dim sub_title As String = ""
    Dim hidden_counter As Integer = 0
    Dim row_count As Integer = 0

    Try


      If CLng(amod_id) > 0 Or Trim(engine_name) <> "" And Trim(engine_name) <> "ALL" Then
        query = "SELECT distinct top 100 comp_name, comp_country, comp_id, count(distinct ac_id) as account"
      Else
        query = "SELECT distinct top 100 comp_name, comp_country, comp_id, count(distinct ac_id) as account"
      End If

      query = query & " from Aircraft_Summary a WITH(NOLOCK)  "

      If product_code = "B" Then
        query = query & " WHERE  ac_product_business_flag = 'Y'"
      ElseIf product_code = "C" Then
        query = query & " WHERE ac_product_commercial_flag = 'Y'"
      ElseIf product_code = "H" Then
        query = query & " WHERE  ac_product_helicopter_flag = 'Y'"
      Else
        query = query & " WHERE ac_product_business_flag = 'Y'"
      End If

      'query = query & " AND cref_operator_flag IN ('Y', 'O')"

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      query = query & " and ac_lifecycle_stage = '3' and cref_operator_flag IN ('Y', 'O')   "

      If CLng(comp_id) > 0 Then
        query = query & " and comp_id = " & CLng(comp_id)
      End If

      If CLng(amod_id) > 0 Then
        query = query & " and amod_id = " & CLng(amod_id)
      End If

      If Trim(engine_name) <> "" And UCase(Trim(engine_name)) <> "ALL" Then
        query = query & " and ac_engine_name = '" & Trim(engine_name) & "'"
      End If

      query = query & " GROUP BY comp_name, comp_country, comp_id"
      query = query & " ORDER BY count(distinct ac_id) desc, comp_name asc"

      'If Session("debug") Then
      '   Session.Item("localUser").crmUser_DebugText += "<b>displayOperatorCompanies : " & Server.HtmlEncode(query) & "</b><br /><br />"
      ' End If


      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function displayOperatorCompanies(ByVal real_company_name, ByVal real_model_name, ByVal product_code, ByVal engine_name, ByVal amod_id, ByVal comp_id, ByVal sub_info, ByVal sub_type) As String</b><br />" & query

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      adors = SqlCommand.ExecuteReader()


      If adors.HasRows Then


        If CLng(comp_id) > 0 Then
          If Trim(tmp_title_holder) = "" Then
            tmp_title_holder = real_company_name
          Else
            tmp_title_holder = tmp_title_holder & " - " & real_company_name
          End If
        End If

        If CLng(amod_id) > 0 Then
          If Trim(tmp_title_holder) = "" Then
            tmp_title_holder = real_model_name
          Else
            tmp_title_holder = tmp_title_holder & " - " & real_model_name
          End If
        End If

        If Trim(engine_name) <> "" And Trim(engine_name) <> "ALL" Then

          title = "OPERATOR SUMMARY : " & tmp_title_holder & Trim(engine_name) & " Engine"

        Else

          If CLng(amod_id) = 0 And CLng(comp_id) = 0 Then
            title = "OPERATOR SUMMARY : TOP 100 OPERATORS"
          Else
            title = "OPERATOR SUMMARY : " & tmp_title_holder
          End If

        End If



        If product_code = "C" Then
          sub_title = sub_title & "<table cellspacing='0' cellpadding='0' border='0' valign='top' width='100%'><tr class='aircraft_list'><td valign='middle' valign='top' align='left' width='60%'>"
        Else
          sub_title = sub_title & "<table cellspacing='0' cellpadding='0' border='0' valign='top' width='100%'><tr class='aircraft_list'><td valign='middle' valign='top' align='left' width='60%'>"
        End If
        sub_title = sub_title & "<strong>Company&nbsp;Name(<em>Country<em>)</strong></td>" & vbCrLf



        If product_code = "C" Then
          sub_title = sub_title & "<td valign='bottom' align='center' width='5%'><strong>On<br />Order</strong></td>"
        End If

        sub_title = sub_title & "<td valign='bottom' align='center'><strong>In<br />Operation</strong></td>"
        sub_title = sub_title & "<td valign='bottom' align='center'><strong>Leased</strong></td>"

        If product_code = "C" Then
          sub_title = sub_title & "<td valign='bottom' align='center' width='7%'><strong>In<br />Storage</strong></td>"
          sub_title = sub_title & "<td valign='bottom' align='center' width='6%'><strong>Retired</strong></td>"
          sub_title = sub_title & "<td valign='bottom' align='right' width='7%'><strong>Total</strong></td>"
        End If

        sub_title = sub_title & "</tr>"

        bgcolor = "#F6F6F6"

        Do While adors.Read
          countcompany = countcompany + 1

          If row_count = 1 Then
            strBuilder.Append("<tr class='alt_row'>")
            row_count = 0
          Else
            strBuilder.Append("<tr bgcolor='white'>")
            row_count = 1
          End If

          strBuilder.Append("<td valign='middle' align='left' width='60%' class='border_bottom_right'>")
          strBuilder.Append("<a href='details.aspx?comp_id=" & adors("comp_id") & "&source=JETNET&type=1'>")
          strBuilder.Append(Replace(adors("comp_name"), " ", "&nbsp;") & " (<em>" & Trim(adors("comp_country") & "") & ")")
          strBuilder.Append("</a>")
          strBuilder.Append("</em></td>")

          inservicetot = adors("account")
          leasedtot = count_totals_ac_table(adors("comp_id"), CLng(amod_id), "leased", 0, product_code)

          ' ******** COUNTING FUNCTION*******      
          If product_code = "C" Then
            retiredtot2 = count_totals_ac_table(adors("comp_id"), CLng(amod_id), "storage", 0, product_code) ' MSW
            retiredtot = count_totals_ac_table(adors("comp_id"), CLng(amod_id), "retired", 0, product_code)
            onordertot = count_totals_ac_table(adors("comp_id"), CLng(amod_id), "order", 0, product_code)
            retiredtot = retiredtot - retiredtot2
            retiredtot = FormatNumber(retiredtot, 0)
            retiredtot2 = FormatNumber(retiredtot2, 0)
            onordertot = FormatNumber(onordertot, 0)
          End If

          ' ******** COUNTING FUNCTION*******	       
          total = inservicetot + onordertot + retiredtot + retiredtot2
          inservicetot = FormatNumber(inservicetot, 0)
          leasedtot = FormatNumber(leasedtot, 0)
          total = FormatNumber(total, 0)

          If product_code = "C" Then
            strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & onordertot & "&nbsp;</td>")
          End If

          strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & inservicetot & "&nbsp;</td>")
          strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & leasedtot & "&nbsp;</td>")

          If product_code = "C" Then
            strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & retiredtot2 & "&nbsp;</td>")
            strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & retiredtot & "&nbsp;</td>")
            strBuilder.Append("<td valign='middle' align='right' class='border_bottom_right'>" & total & "&nbsp;</td>")
          End If

          strBuilder.Append("</tr>")

          runningtotal = runningtotal + total

          hidden_counter = hidden_counter + 1
        Loop


        adors.Close()


      Else



        If product_code = "B" Then
          type_of_search = " Business "
        ElseIf product_code = "C" Then
          type_of_search = " Commercial "
        ElseIf product_code = "H" Then
          type_of_search = " Helicopter "
        Else
          type_of_search = " Business "
        End If


        displayOperatorCompanies = displayOperatorCompanies & "<tr><td valign='top' align='left' class='border_bottom_right'><br>No Data Available "
        displayOperatorCompanies = displayOperatorCompanies & real_model_name & "</td></tr>"


      End If

      adors = Nothing
      SqlConn.Close()

      If CLng(comp_id) = 0 And (Trim(engine_name) = "" Or
         UCase(Trim(engine_name)) = "ALL") Then

        inservicetot = count_totals_ac_table(CLng(comp_id), CLng(amod_id), "operation", 1, product_code)

        If product_code = "C" Then
          onordertot = count_totals_ac_table(CLng(comp_id), CLng(amod_id), "order", 1, product_code)
          retiredtot2 = count_totals_ac_table(CLng(comp_id), CLng(amod_id), "storage", 1, product_code) ' MSW
          retiredtot = count_totals_ac_table(CLng(comp_id), CLng(amod_id), "retired", 1, product_code)
          retiredtot = retiredtot - retiredtot2
        End If

        leasedtot = count_totals_ac_table(CLng(comp_id), CLng(amod_id), "leased", 1, product_code)

      Else

        inservicetot = inserviceshowtot
        inservicetot = FormatNumber(inservicetot, 0)

        query = "SELECT * FROM company WITH(NOLOCK) WHERE comp_id = " & CLng(comp_id)

        SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL
        '  End Select

        ' End Select
        SqlConn.Open()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function displayOperatorCompanies(ByVal real_company_name, ByVal real_model_name, ByVal product_code, ByVal engine_name, ByVal amod_id, ByVal comp_id, ByVal sub_info, ByVal sub_type) As String</b><br />" & query

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = System.Data.CommandType.Text
        SqlCommand.CommandTimeout = 60
        SqlCommand.CommandText = query
        adors = SqlCommand.ExecuteReader()


        If adors.HasRows Then
          adors.Read()
          company_information = "<br /><table width='100%' cellspacing='0' cellpadding='2'>"
          company_information = company_information & "<tr><td align='left' valign='middle' class='header' style='padding-left:3px;'>OPERATOR DETAILS : " & tmp_title_holder & "</td></tr>"
          company_information = company_information & "<tr><td>Name : " & adors("comp_name") & "</td></tr>"
          company_information = company_information & "<tr><td>City : " & adors("comp_city") & "</td></tr>"
          company_information = company_information & "<tr><td>Country : " & adors("comp_country") & "</td></tr>"
          company_information = company_information & "</table>"

          adors.Close()

        End If

        adors = Nothing

      End If

      totaltot = inservicetot + onordertot + retiredtot + retiredtot2
      totaltot = FormatNumber(totaltot, 0)
      retiredtot = FormatNumber(retiredtot, 0)
      retiredtot2 = FormatNumber(retiredtot2, 0)
      onordertot = FormatNumber(onordertot, 0)
      inservicetot = FormatNumber(inservicetot, 0)
      leasedtot = FormatNumber(leasedtot, 0)
      total = FormatNumber(total, 0)



      '    If Trim(company_information) = "" Then
      '   displayOperatorCompanies = displayOperatorCompanies & displayOperatorViewSummary(totaltot, inservicetot, onordertot, leasedtot, retiredtot, retiredtot2, "op", 1)
      '  Else
      If CLng(comp_id) > 0 Then
        displayOperatorCompanies = displayOperatorCompanies & company_information
      End If
      ' End If

      If Trim(sub_title) <> "" Then
        displayOperatorCompanies = sub_title & strBuilder.ToString() & displayOperatorCompanies & "</table>" & vbCrLf
      Else
        displayOperatorCompanies = "<table>" & displayOperatorCompanies & "</table>" & vbCrLf
      End If

      strBuilder = Nothing
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function count_totals_ac_table(ByVal company_id, ByVal model_id, ByVal current_to_total, ByVal is_bottom, ByVal product_code)
    count_totals_ac_table = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim current_total As Integer = 0
    Dim query As String = ""
    Try

      query = "SELECT count(distinct ac_id) as account2"
      query = query & " FROM Aircraft_Summary"

      '   query = query & " INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      '   query = query & " INNER JOIN Aircraft_Reference WITH(NOLOCK) ON ac_journ_id = cref_journ_id AND ac_id = cref_ac_id"

      query = query & " WHERE "

      If current_to_total = "leased" Then
        query = query & " ac_lease_flag='Y'"
        If is_bottom = 0 Then
          query = query & " AND cref_operator_flag IN ('Y', 'O')"
        End If
      ElseIf current_to_total = "storage" Then
        query = query & " (ac_lifecycle_stage IN ('4'))"
        query = query & " AND ((cref_operator_flag IN ('Y', 'O')) or cref_contact_type in ('42','56'))"
        query = query & " AND (ac_status = 'Withdrawn from Use - Stored')"
      ElseIf current_to_total = "retired" Then
        query = query & " (ac_lifecycle_stage IN ('4'))"
        query = query & " AND (cref_operator_flag IN ('Y', 'O') or cref_contact_type in ('42','56'))"
      ElseIf current_to_total = "operation" Then
        query = query & " ac_lifecycle_stage IN ('3')"
        If is_bottom = 0 Then
          query = query & " AND cref_operator_flag IN ('Y', 'O')"
        End If
      ElseIf current_to_total = "order" Then
        query = query & " (ac_lifecycle_stage IN ('1','2') and (cref_contact_type in ('42') or cref_operator_flag IN ('Y', 'O'))) "  ' 
      End If

      '   query = query & " " + commonEVO.GenerateProductCodeSelectionQuery(Session.Item("localSubscription"), False, False)

      If model_id > 0 Then
        query = query & " AND amod_id = " & model_id
      End If

      If company_id > 0 Then
        query = query & " AND comp_id =" & company_id
      End If

      '     query = query & " AND (ac_journ_id = 0)"


      If product_code = "B" Then
        query = query & " AND ac_product_business_flag = 'Y'"
      ElseIf product_code = "C" Then
        query = query & " AND ac_product_commercial_flag = 'Y'"
      ElseIf product_code = "H" Then
        query = query & " AND ac_product_helicopter_flag = 'Y'"
      Else
        query = query & " AND ac_product_business_flag = 'Y'"
      End If


      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      adors = SqlCommand.ExecuteReader()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function count_totals_ac_table(ByVal company_id, ByVal model_id, ByVal current_to_total, ByVal is_bottom, ByVal product_code) As String</b><br />" & query

      If adors.HasRows Then
        Do While adors.Read
          current_total = current_total + adors("account2")
        Loop
        adors.Close()
      End If

      adors.Close()
      adors = Nothing
      SqlCommand.Dispose()

      count_totals_ac_table = current_total
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function display_OperatorView_PieChart(ByVal real_model_name, ByVal height_top_middle_table, ByVal graphID, ByVal product_code, ByVal comp_id, ByVal amod_id)
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    Dim current_total As Integer = 0
    Dim query As String = ""
    Dim pie_title_section As String = ""
    Dim record_count As Integer = 0
    Dim x As Integer = 0
    Dim strBuilder As New StringBuilder
    Dim high_number As Integer = 0
    Dim low_number As Integer = 20000000
    Dim temp_title As String = ""
    Dim bgcolor As String = ""
    Dim counter_for_break As Integer = 2
    display_OperatorView_PieChart = ""


    Try

      query = "SELECT Case ISNULL(comp_country,'') When '' then 'unknown' ELSE comp_country END AS comp_country, COUNT(*) AS tcount"
      query = query & " FROM Aircraft_Summary WITH(NOLOCK) "
      'query = query & " INNER JOIN Company WITH(NOLOCK) INNER JOIN Aircraft_Reference WITH(NOLOCK) ON comp_id = cref_comp_id AND"
      ' query = query & " comp_journ_id = cref_journ_id ON ac_journ_id = cref_journ_id AND ac_id = cref_ac_id INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"



      If product_code = "B" Then
        query = query & " WHERE ac_product_business_flag = 'Y' and ac_lifecycle_stage = 3"
      ElseIf product_code = "C" Then
        query = query & " WHERE ac_product_commercial_flag = 'Y' and ac_lifecycle_stage = 3"
      ElseIf product_code = "H" Then
        query = query & " WHERE ac_product_helicopter_flag = 'Y' and ac_lifecycle_stage = 3"
      Else
        query = query & " WHERE ac_product_business_flag = 'Y' and ac_lifecycle_stage = 3"
      End If



      query = query & " AND (cref_operator_flag IN ('Y', 'O'))"

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      If CLng(comp_id) > 0 Then
        query = query & " AND comp_id = " & CStr(comp_id)
      End If

      If CLng(amod_id) > 0 Then
        query = query & " AND amod_id = " & CStr(amod_id)
      End If

      query = query & " GROUP BY comp_country ORDER BY tcount DESC"


      'If Session("debug") Then
      '     Session.Item("localUser").crmUser_DebugText += "<b>display_OperatorView_PieChart : " & Server.HtmlEncode(query) & "</b><br /><br />"
      'End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_OperatorView_PieChart(ByVal real_model_name, ByVal height_top_middle_table, ByVal graphID, ByVal product_code, ByVal comp_id, ByVal amod_id)</b><br />" & query

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      rs = SqlCommand.ExecuteReader()


      If rs.HasRows Then
        Me.OP_COUNTRY_CHART.Series.Clear()
        Me.OP_COUNTRY_CHART.Series.Add("OP_COUNTRY_CHART").ChartType = UI.DataVisualization.Charting.SeriesChartType.Pie
        Me.OP_COUNTRY_CHART.ChartAreas("ChartArea1").AxisY.Title = "title1"

        Me.OP_COUNTRY_CHART.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        'Me.SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Perspective = 5
        Me.OP_COUNTRY_CHART.ChartAreas("ChartArea1").Area3DStyle.Rotation = 10

        Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").Color = Drawing.Color.Blue
        '     Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").BorderWidth = 1
        '     Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").MarkerSize = 5
        '     Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
        '     Me.OP_COUNTRY_CHART.BorderlineWidth = 10
        Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32


        Me.OP_COUNTRY_CHART.Width = 200
        Me.OP_COUNTRY_CHART.Height = 200



        '     display_OperatorView_PieChart = display_OperatorView_PieChart & "<table width='100%'>"
        display_OperatorView_PieChart = display_OperatorView_PieChart & "<table valign='top' width='100%'><tr valign='top'><td valign='top' width='33%'><table valign='top' width='100%'>"

        Do While rs.Read
          counter_for_break = counter_for_break + 1

          If Not IsDBNull(rs("comp_country")) Then
            If Trim(rs("comp_country")) <> "" Then

              If bgcolor = "#ffffff" Then
                bgcolor = "#F6F6F6"
              Else
                bgcolor = "#ffffff"
              End If

              Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").Points.Add(rs("tcount"))

              display_OperatorView_PieChart = display_OperatorView_PieChart & "<tr bgcolor='" & bgcolor & "' valign='top'><td align='left' valign='top' width='100%'>"

              If x < 5 Then
                Me.OP_COUNTRY_CHART.Series("OP_COUNTRY_CHART").Points.Last.Label = x + 1
                display_OperatorView_PieChart = display_OperatorView_PieChart & "<font size='2px'>" & (x + 1) & ". "
                display_OperatorView_PieChart = display_OperatorView_PieChart & rs("comp_country") & "</font>"
                display_OperatorView_PieChart = display_OperatorView_PieChart & "</td><td align='right'><font size='2px'>" & rs("tcount") & "</font></td></tr>"
              Else
                display_OperatorView_PieChart = display_OperatorView_PieChart & "<font size='2px'>" & rs("comp_country") & "</font>"
                display_OperatorView_PieChart = display_OperatorView_PieChart & "</td><td align='right'><font size='1px'>" & rs("tcount") & "</font></td></tr>"
              End If


              x = x + 1
            End If
          End If



          current_total = current_total + 1
          record_count = record_count + 1
        Loop


        display_OperatorView_PieChart = display_OperatorView_PieChart & "</table></td></tr></table>"

        rs.Close()
        '
        '        display_OperatorView_PieChart = display_OperatorView_PieChart & "</table>"

        '     Me.PER_MONTH.ChartAreas("ChartArea2").AxisY.Maximum = high_number + 1
        '    Me.AVG_DAYS_ON.ChartAreas("ChartArea2").AxisY.Minimum = low_number - 1



      Else
        strBuilder.Append("")
      End If


      rs = Nothing


      strBuilder = Nothing

    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function GetWantedInfo(ByVal inModelID)
    GetWantedInfo = ""
    Dim query, fAmwant_listed_date, fInterested_party, fAmwant_start_year, fAmwant_end_year, fAmwant_notes, fAmwant_id, fComp_id
    Dim strBuilder
    strBuilder = New StringBuilder
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim row_count As Integer = 0
    Try


      query = "SELECT TOP 20 Aircraft_Model_Wanted.*, comp_id, comp_name AS interested_party"
      query = query & " FROM Aircraft_Model_Wanted WITH(NOLOCK), Aircraft_Model WITH(NOLOCK), Company WITH(NOLOCK)"
      query = query & " WHERE (amwant_amod_id > 0) AND (amwant_amod_id = amod_id) AND (amwant_comp_id = comp_id)"
      query = query & " AND (amwant_journ_id = comp_journ_id) AND (amwant_journ_id = 0)"
      query = query & " AND (amwant_verified_date IS NOT NULL) AND (amwant_amod_id = " & inModelID & ")"
      query = query & " AND (amod_customer_flag = 'Y')"


      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), True, False)


      query = query & "ORDER BY amwant_listed_date DESC, amod_make_name, amod_model_name"

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>getWantedInfo : " & Server.HtmlEncode(query) & "</b><br /><br />"
      'End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      adors = SqlCommand.ExecuteReader()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetWantedInfo(ByVal inModelID)</b><br />" & query
      If adors.HasRows Then

        strBuilder.Append("<table id='wantedInfoTable' width='100%' cellpadding='2' cellspacing='0'>")

        Do While adors.Read

          If Not (IsDBNull(adors("amwant_listed_date"))) Then
            fAmwant_listed_date = Trim(adors("amwant_listed_date"))
          Else
            fAmwant_listed_date = ""
          End If

          If Not (IsDBNull(adors("interested_party"))) Then
            fInterested_party = Trim(adors("interested_party"))
          Else
            fInterested_party = ""
          End If

          If Not (IsDBNull(adors("amwant_start_year"))) Then
            fAmwant_start_year = Trim(adors("amwant_start_year"))
          Else
            fAmwant_start_year = ""
          End If

          If Not (IsDBNull(adors("amwant_end_year"))) Then
            fAmwant_end_year = Trim(adors("amwant_end_year"))
          Else
            fAmwant_end_year = ""
          End If

          If Not (IsDBNull(adors("amwant_notes"))) Then
            fAmwant_notes = Trim(adors("amwant_notes"))
          Else
            fAmwant_notes = ""
          End If

          If Not (IsDBNull(adors("amwant_id"))) Then
            fAmwant_id = Trim(adors("amwant_id"))
          Else
            fAmwant_id = ""
          End If

          If Not (IsDBNull(adors("comp_id"))) Then
            fComp_id = Trim(adors("comp_id"))
          Else
            fComp_id = ""
          End If


          If row_count = 1 Then
            strBuilder.Append("<tr class='alt_row'>")
            row_count = 0
          Else
            strBuilder.Append("<tr bgcolor='white'>")
            row_count = 1
          End If

          strBuilder.Append("<td align='left' valign='top'><img src='images/ch_red.jpg' class='bullet'/></td>")
          strBuilder.Append("<td align='left' valign='top'><em><a href='JavaScript:OpenSmallWindow(" & QUOTE & "DisplayWantedDetails.asp?id=" & Server.UrlEncode(fAmwant_id) & QUOTE & "," & QUOTE & "WantedDetail" & QUOTE & ")'>" & fAmwant_listed_date & "</a></em> | <a href='details.aspx?comp_id=" & fComp_id & "&source=JETNET&type=1'><strong>" & fInterested_party & "</strong></a><br />")

          If Trim(fAmwant_start_year) <> "" And Trim(fAmwant_end_year) <> "" Then
            strBuilder.Append("Year: " & fAmwant_start_year)
            strBuilder.Append(" - " & fAmwant_end_year)
          ElseIf Trim(fAmwant_start_year) <> "" And Trim(fAmwant_end_year) = "" Then
            strBuilder.Append("Year: " & fAmwant_start_year)
          ElseIf Trim(fAmwant_start_year) = "" And Trim(fAmwant_end_year) <> "" Then
            strBuilder.Append("End Year: " & fAmwant_end_year)
          ElseIf Trim(fAmwant_start_year) = "" And Trim(fAmwant_end_year) = "" Then
            strBuilder.Append("Year: Open")
          End If

          If Trim(fAmwant_notes) <> "" Then
            strBuilder.Append(" " & Trim(Left(fAmwant_notes, 250)))
          End If

          strBuilder.Append("<hr /></td></tr>")

        Loop

        adors.Close()

        strBuilder.Append("</table>")

      Else
        strBuilder.Append("No Wanteds at this time, for this Make/Model ...")
      End If

      adors = Nothing

      GetWantedInfo = strBuilder.ToString()

      strBuilder = Nothing
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  Function GetNewsInfo(ByVal inModelID)
    GetNewsInfo = ""
    Dim query, fAbinewslnk_web_address
    Dim strBuilder
    strBuilder = New StringBuilder
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
    Dim adors2 As System.Data.SqlClient.SqlDataReader : adors2 = Nothing
    Dim row_count As Integer = 0

    Try

      query = "SELECT TOP 20 abinewslnk_date, abinewslnk_title, abinewslnk_description, abinewssrc_name, abinewslnk_web_address"
      query = query & " FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) ON abinewslnk_source_id = abinewssrc_id WHERE abinewslnk_amod_id = " & inModelID
      query = query & " ORDER BY abinewslnk_date desc"

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      adors = SqlCommand.ExecuteReader()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetNewsInfo(ByVal inModelID)</b><br />" & query


      If adors.HasRows Then
        adors.Close()
        adors = Nothing
        'check the make name

        query = "SELECT TOP 20 abinewslnk_date, abinewslnk_title, abinewslnk_description, abinewssrc_name, abinewslnk_web_address"
        query = query & " FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) on abinewslnk_source_id = abinewssrc_id"
        query = query & " WHERE (abinewslnk_make_name = '" & make_name & "')"
        query = query & " ORDER BY abinewslnk_date desc"
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetNewsInfo(ByVal inModelID)</b><br />" & query

      End If

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>getNewsInfo : " & Server.HtmlEncode(query) & "</b><br /><br />"
      ' End If

      SqlCommand.CommandText = query
      adors = SqlCommand.ExecuteReader()


      If adors.HasRows Then

        strBuilder.Append("<table id='newsInfoTable' width='100%' cellpadding='2' cellspacing='0'>")

        Do While adors.Read

          If Not (IsDBNull(adors("abinewslnk_web_address"))) Then
            If Trim(adors("abinewslnk_web_address")) <> "" Then
              fAbinewslnk_web_address = Trim(adors("abinewslnk_web_address"))
              If (Left(LCase(fAbinewslnk_web_address), 3) = "www") Then
                fAbinewslnk_web_address = "<strong><a href='http://" & fAbinewslnk_web_address & "' target='new'>" & adors("abinewslnk_title") & "</a></strong>"
              Else
                fAbinewslnk_web_address = "<strong><a href='" & fAbinewslnk_web_address & "' target='new'>" & adors("abinewslnk_title") & "</a></strong>"
              End If
            Else
              fAbinewslnk_web_address = "<strong>" & adors("abinewslnk_title") & "</strong>"
            End If
          Else
            fAbinewslnk_web_address = "<strong>" & adors("abinewslnk_title") & "</strong>"
          End If


          If row_count = 1 Then
            strBuilder.Append("<tr class='alt_row'>")
            row_count = 0
          Else
            strBuilder.Append("<tr bgcolor='white'>")
            row_count = 1
          End If

          strBuilder.Append("<td align='left' valign='top'><img src='images/ch_red.jpg' class='bullet'/></td>")
          strBuilder.Append("<td align='left' valign='top'><em>" & adors("abinewslnk_date") & "</em> | " & fAbinewslnk_web_address & "<br />")
          strBuilder.Append(Left(adors("abinewslnk_description"), 250) & "... <hr /></td></tr>")

        Loop

        strBuilder.Append("</table>")

        adors.Close()
      Else
        strBuilder.Append("No News to display at this time for this Make/Model")
      End If

      adors = Nothing

      GetNewsInfo = strBuilder.ToString()

      strBuilder = Nothing
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function Build_SPI(ByVal real_make_model_name, ByVal amod_id, ByVal sub_info, ByVal sub_type, ByVal weight_class, ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal quarter, ByVal first_model)
    Build_SPI = ""
    Dim page_count As Integer = 0
    Dim space_spot As Integer = 0
    Dim text_from_first As String = ""
    Dim color As String = "Blue"
    Dim temp_string As String = ""

    color = "Blue"
    Session("salesPriceViewWtCls") = weight_class
    Session("salesPriceViewWtClsName") = weight_class_name
    Session("SPSingleGraphType") = 4
    Session("salesPriceViewAirframeType") = airframe_type_num



    If first_model = True Then
      Session("SPYearSld1") = spi_year
    ElseIf amod_id > 0 Then
      Session("SPYearSld1") = 2005
    Else
      Session("SPYearSld1") = spi_year
    End If

    Session("SPYearSld2") = spi_year2
    Session("salesPriceViewMake") = make_name
    Session("salesPriceViewModel") = model_name
    Session("salesPriceViewAirframeType") = airframe_type
    Session("SPYearQtr1") = quarter



    space_spot = InStr(real_make_model_name, " ")

    If space_spot > 0 Then
      Session("salesPriceViewMake") = Left(real_make_model_name, space_spot - 1)
      Session("salesPriceViewModel") = Right(real_make_model_name, (real_make_model_name.ToString.Length - space_spot))
    End If


    Build_SPI = ""

    temp_string = ReturnQuarterlyByModel(amod_id, "", text_from_first, sub_info, real_make_model_name, sub_type, weight_class, weight_class_name, spi_year, spi_year2, airframe_type, color)
    Build_SPI += string_for_spi_start

    Build_SPI += ReturnPreviousFullQuarterlyByWeightClass("", amod_id, "", "", "", weight_class, weight_class_name, spi_year, spi_year2, airframe_type, sub_info, real_make_model_name, sub_type, color)

    Build_SPI += temp_string

  End Function
  Private Function ReturnQuarterlyByModel(ByVal lModelId, ByRef strHTMLData1, ByRef strGraphVarianceAsking,
             ByVal sub_info, ByVal real_make_model_name, ByVal sub_type, ByVal weight_class, ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal color) As String


    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rstRec1 As System.Data.SqlClient.SqlDataReader : rstRec1 = Nothing

    ReturnQuarterlyByModel = ""
    Dim query As String = ""
    Dim strQuery1 As String = ""

    Dim strYearSld As String = ""
    Dim strQuarterSld As String = ""
    Dim strYearQtrName As String = ""
    Dim strAvgYearMfr As String = ""
    Dim strAvgYearDlv As String = ""
    Dim strAvgAsking As String = ""
    Dim strAvgAskingHidden As String = ""
    Dim strAvgSelling As String = ""
    Dim strPercent As String = ""
    Dim strPercentHidden As String = ""
    Dim strVariance As String = ""
    Dim strVarianceHidden As String = ""
    Dim strAvgAFTT As String = ""
    Dim strAvgDOM As String = ""

    Dim lYearSld As String = ""
    Dim lQuarterSld As String = ""
    Dim dAvgYearMfr As Double = 0
    Dim dAvgYearDlv As Double = 0
    Dim dAvgAsking As Double = 0
    Dim dAvgAskingHidden As Double = 0
    Dim dAvgSelling As Double = 0
    Dim dPercent As Double = 0
    Dim dPercentHidden As Double = 0
    Dim dVariance As Double = 0
    Dim dVarianceHidden As Double = 0
    Dim dAvgAFTT As Double = 0
    Dim dAvgDOM As Double = 0

    Dim strHRef1 As String = ""

    Dim lColSpan As String = ""
    Dim lTotRec As Double = 0

    Dim objGraph As String = ""
    Dim lGraphType As String = ""
    Dim strGraphImage As String = ""
    Dim lMaxSets As Double = 0
    Dim lThisSet As Double = 0


    Dim strHTMLData1_2 As String = ""
    Dim strHTMLData1_2_final As String = ""


    ' Percentage Of Asking Price     
    Dim strTitle1 As String = ""
    Dim strBottomTitle1 As String = ""
    Dim strLeftTitle1 As String = ""
    Dim lCnt1 As Integer = 0
    Dim aData1()
    Dim aLabels1()

    ' Variance Of Asking Price     
    Dim strTitle2 As String = ""
    Dim strBottomTitle2 As String = ""
    Dim strLeftTitle2 As String = ""
    Dim lCnt2 As Integer = 0
    Dim aData2()
    Dim aLabels2()

    ' Asking Price     
    Dim strTitle3 As String = ""
    Dim strBottomTitle3 As String = ""
    Dim strLeftTitle3 As String = ""
    Dim lCnt3 As Integer = 0
    Dim aData3()
    Dim aLabels3()

    ' Selling Price     
    Dim strTitle4 As String = ""
    Dim strBottomTitle4 As String = ""
    Dim strLeftTitle4 As String = ""
    Dim lCnt4 As Integer = 0
    Dim aData4()
    Dim aLabels4()

    ' Asking vs Selling Price     
    Dim strTitle5 As String = ""
    Dim strBottomTitle5 As String = ""
    Dim strLeftTitle5 As String = ""
    Dim strRightTitle5 As String = ""
    Dim lCnt5a As Integer = 0
    Dim lCnt5b As Integer = 0
    Dim aData5a()  ' Asking
    Dim aData5b()  ' Selling
    Dim aLabels5a()
    Dim aLabels5b()

    ' AFTT (Airframe Total Time)
    Dim strTitle6 As String = ""
    Dim strBottomTitle6 As String = ""
    Dim strLeftTitle6 As String = ""
    Dim lCnt6 As Integer = 0
    Dim aData6()
    Dim aLabels6()

    ' DOM (Days On Market)
    Dim strTitle7 As String = ""
    Dim strBottomTitle7 As String = ""
    Dim strLeftTitle7 As String = ""
    Dim lCnt7 As Integer = 0
    Dim aData7()
    Dim aLabels7()

    ' AFTT vs Variance
    Dim strTitle8 As String = ""
    Dim strBottomTitle8 As String = ""
    Dim strLeftTitle8 As String = ""
    Dim lCnt8 As Integer = 0
    Dim aData8()
    Dim aLabels8()

    ' DOM vs Variance
    Dim strTitle9 As String = ""
    Dim strBottomTitle9 As String = ""
    Dim strLeftTitle9 As String = ""
    Dim lCnt9 As Integer = 0
    Dim aData9()
    Dim aLabels9()

    ' AFTT vs Selling Price
    Dim strTitle10 As String = ""
    Dim strBottomTitle10 As String = ""
    Dim strLeftTitle10 As String = ""
    Dim lCnt10 As Integer = 0
    Dim aData10()
    Dim aLabels10()

    ' DOM vs Selling Price
    Dim strTitle11 As String = ""
    Dim strBottomTitle11 As String = ""
    Dim strLeftTitle11 As String = ""
    Dim lCnt11 As Integer = 0
    Dim aData11()
    Dim aLabels11()

    Dim tmpGraph As String = ""
    Dim cHyphen As String = "-"

    Dim min As Integer = 0
    Dim max As Integer = 0
    Dim min_max As String = ""


    Try



      strQuery1 = "SELECT DATEPART(year, journ_date) As YearSld, DATEPART(quarter, journ_date) As QuarterSld,"
      strQuery1 = strQuery1 & " AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr,"
      strQuery1 = strQuery1 & " AVG(CAST(ac_year AS INT)) As dAvgYearDlv,"
      strQuery1 = strQuery1 & " AVG(ac_asking_price) As dAvgAsking,"
      strQuery1 = strQuery1 & " AVG(ac_hidden_asking_price) As dAvgAskingHidden,"
      strQuery1 = strQuery1 & " AVG(ac_sale_price) As dAvgSelling,"
      strQuery1 = strQuery1 & " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent,"
      strQuery1 = strQuery1 & " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance,"
      strQuery1 = strQuery1 & " ((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden,"
      strQuery1 = strQuery1 & " ((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden,"
      strQuery1 = strQuery1 & " AVG(ac_airframe_tot_hrs) As dAvgAFTT,"
      strQuery1 = strQuery1 & " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM"

      strQuery1 = strQuery1 & " FROM Aircraft_Summary_SPI WITH (NOLOCK)"
      '  strQuery1 = strQuery1 & " FROM Aircraft WITH (NOLOCK, INDEX(ix_ac_sale_price_ac_id_journ_id_key))"
      '  strQuery1 = strQuery1 & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id"
      '  strQuery1 = strQuery1 & " INNER JOIN Journal WITH (NOLOCK) ON ac_id = journ_ac_id AND ac_journ_id = journ_id"
      '  strQuery1 = strQuery1 & " INNER JOIN Journal_Category WITH (NOLOCK) ON journ_subcategory_code = jcat_subcategory_code "


      strQuery1 = strQuery1 & " WHERE (ac_journ_id > 0)"
      strQuery1 = strQuery1 & " AND (ac_lifecycle_stage = 3)"                   '-- In Operation Only
      strQuery1 = strQuery1 & " AND (jcat_used_retail_sales_flag = 'Y')"        '-- Retail Only    
      strQuery1 = strQuery1 & " AND (journ_newac_flag = 'N')"                   '-- Used Sales Only
      strQuery1 = strQuery1 & " AND (journ_subcategory_code LIKE 'WS%')"        '-- Whole Sales Only
      strQuery1 = strQuery1 & " AND (journ_subcategory_code NOT LIKE '%IT%')"   '-- No Internals
      strQuery1 = strQuery1 & " AND (journ_internal_trans_flag = 'N')"          '-- No Internals 

      If lModelId > 0 Then
        strQuery1 = strQuery1 & " AND (amod_id = " & lModelId & ")"
      End If

      strQuery1 = strQuery1 & " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0)"


      strQuery1 = strQuery1 & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      If Trim(Session("SPYearSld1")) > 0 Then
        strQuery1 = strQuery1 & " AND (DATEPART(year,journ_date) >= " & Session("SPYearSld1") & ")"
      End If

      If Trim(Session("SPYearSld2")) > 0 Then
        strQuery1 = strQuery1 & " AND (DATEPART(year,journ_date) <= " & Session("SPYearSld2") & ")"
      End If

      strQuery1 = strQuery1 & " GROUP BY DATEPART(year, journ_date), DATEPART(quarter, journ_date)"
      strQuery1 = strQuery1 & " ORDER BY DATEPART(year, journ_date) ASC, DATEPART(quarter, journ_date) ASC"

      'If Session("debug") Then
      '   Session.Item("localUser").crmUser_DebugText += "<b>ReturnQuarterlyByModel : " & Server.HtmlEncode(strQuery1) & "</b><br /><br />"
      ' End If

      '  Case Else
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>ReturnQuarterlyByModel(ByVal lModelId, ByRef strHTMLData1, ByRef strGraphVarianceAsking, ByVal sub_info, ByVal real_make_model_name, ByVal sub_type, ByVal weight_class, ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal color) As String</b><br />" & strQuery1
      ' End Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = strQuery1
      rstRec1 = SqlCommand.ExecuteReader()



      strHTMLData1 = "<table id='quarterlyModelDataTable' cellpadding='2' cellspacing='0' width='100%'>"
      strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' colspan='9'>" & Session("salesPriceViewMake") & "&nbsp;/&nbsp;" & Session("salesPriceViewModel") & "</td></tr>"

      strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' rowspan='2'>Year<br />Quarter</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Year Of</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Price (k)</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Percent</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Variance</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Average</td></tr>"

      strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center'>Mftr</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center'>Delivery</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center'>Asking</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center'>Selling</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center'>AFTT</td>"
      strHTMLData1 = strHTMLData1 & "<td align='center'>Days<br />On<br />Mrkt</td></tr>"

      If rstRec1.HasRows Then


        lTotRec = 1000

        lCnt1 = 0
        lCnt2 = 0
        lCnt3 = 0
        lCnt4 = 0
        lCnt5a = 0
        lCnt5b = 0
        lCnt6 = 0
        lCnt7 = 0
        lCnt8 = 0
        lCnt9 = 0
        lCnt10 = 0
        lCnt11 = 0

        ReDim aData1(lTotRec)      ' Percentage Of Asking Price
        ReDim aLabels1(lTotRec)

        ReDim aData2(lTotRec)      ' Variance Of Asking Price
        ReDim aLabels2(lTotRec)

        ReDim aData3(lTotRec)      ' Asking Price
        ReDim aLabels3(lTotRec)

        ReDim aData4(lTotRec)      ' Selling Price
        ReDim aLabels4(lTotRec)

        ReDim aData5a(lTotRec)     ' Asking Price
        ReDim aLabels5a(lTotRec)

        ReDim aData5b(lTotRec)      ' Selling Price
        ReDim aLabels5b(lTotRec)

        ReDim aData6(lTotRec)      ' Avg AFTT
        ReDim aLabels6(lTotRec)

        ReDim aData7(lTotRec)      ' Avg DOM
        ReDim aLabels7(lTotRec)

        ReDim aData8(lTotRec)      ' Avg AFTT vs Variance
        ReDim aLabels8(lTotRec)

        ReDim aData9(lTotRec)      ' Avg DOM vs Variance
        ReDim aLabels9(lTotRec)

        ReDim aData10(lTotRec)     ' Avg AFTT vs Selling Price
        ReDim aLabels10(lTotRec)

        ReDim aData11(lTotRec)     ' Avg DOM vs Selling Price
        ReDim aLabels11(lTotRec)


        strHTMLData1 = "<table id='quarterlyModelDataTable' cellpadding='2' cellspacing='0' width='100%' border='1'>"
        strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' colspan='9'>" & Session("salesPriceViewMake") & "&nbsp;/&nbsp;" & Session("salesPriceViewModel") & "</td></tr>"

        strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center' rowspan='2'>Year<br />Quarter</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Year Of</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Avg Price (k)</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Percent</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center' rowspan='2'>Variance</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center' colspan='2'>Average</td></tr>"

        strHTMLData1 = strHTMLData1 & "<tr  class='aircraft_list'><td align='center'>Mftr</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center'>Delivery</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center'>Asking</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center'>Selling</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center'>AFTT</td>"
        strHTMLData1 = strHTMLData1 & "<td align='center'>Days<br />On<br />Mrkt</td></tr>"

        Do While rstRec1.Read

          strYearSld = ""
          strQuarterSld = ""
          strYearQtrName = ""

          strAvgYearMfr = ""
          strAvgYearDlv = ""
          strAvgAsking = ""
          strAvgAskingHidden = ""
          strAvgSelling = ""
          strPercent = ""
          strPercentHidden = ""
          strVariance = ""
          strVarianceHidden = ""
          strAvgAFTT = ""
          strAvgDOM = ""

          lYearSld = 0
          lQuarterSld = 0
          dAvgYearMfr = 0.0
          dAvgYearDlv = 0.0
          dAvgAsking = 0.0
          dAvgAskingHidden = 0.0
          dAvgSelling = 0.0
          dPercent = 0.0
          dPercentHidden = 0.0
          dVariance = 0.0
          dVarianceHidden = 0.0
          dAvgAFTT = 0.0
          dAvgDOM = 0.0

          If Not String.IsNullOrEmpty(rstRec1("YearSld")) Then
            lYearSld = rstRec1("YearSld")
          Else
            lYearSld = Year(Now())
          End If

          If Not String.IsNullOrEmpty(rstRec1("QuarterSld")) Then
            lQuarterSld = rstRec1("QuarterSld")
          Else
            '     lQuarterSld = Right(Get_Quarter_For_Month_Server(Month(Now())), 1)
          End If

          strYearSld = CStr(lYearSld)
          strQuarterSld = CStr(lQuarterSld)

          strYearQtrName = strYearSld & cHyphen & "Q" & strQuarterSld

          If Not IsDBNull(rstRec1("dAvgYearMfr")) Then
            dAvgYearMfr = rstRec1("dAvgYearMfr")
          Else
            dAvgYearMfr = 0
          End If

          If Not IsDBNull(rstRec1("dAvgYearDlv")) Then
            dAvgYearDlv = rstRec1("dAvgYearDlv")
          Else
            dAvgYearDlv = 0
          End If

          If Not IsDBNull(rstRec1("dAvgAsking")) Then
            dAvgAsking = rstRec1("dAvgAsking")
          Else
            dAvgAsking = 0
          End If

          If Not IsDBNull(rstRec1("dAvgAskingHidden")) Then
            dAvgAskingHidden = rstRec1("dAvgAskingHidden")
          Else
            dAvgAskingHidden = 0
          End If


          If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
            dAvgAsking = dAvgAskingHidden
          End If

          If Not IsDBNull(rstRec1("dAvgSelling")) Then
            dAvgSelling = rstRec1("dAvgSelling")
          Else
            dAvgSelling = 0
          End If

          If Not IsDBNull(rstRec1("dPercent")) Then
            dPercent = rstRec1("dPercent")
          Else
            dPercent = 0
          End If

          If Not IsDBNull(rstRec1("dPercentHidden")) Then
            dPercentHidden = rstRec1("dPercentHidden")
          Else
            dPercentHidden = 0
          End If

          If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
            dPercent = dPercentHidden
          End If

          If Not IsDBNull(rstRec1("dVariance")) Then
            dVariance = rstRec1("dVariance")
          Else
            dVariance = 0
          End If

          If Not IsDBNull(rstRec1("dVarianceHidden")) Then
            dVarianceHidden = rstRec1("dVarianceHidden")
          Else
            dVarianceHidden = 0
          End If

          If dAvgAsking = 0 And dAvgAskingHidden > 0 Then
            dVariance = dVarianceHidden
          End If

          If Not IsDBNull(rstRec1("dAvgAFTT")) Then
            dAvgAFTT = rstRec1("dAvgAFTT")
          Else
            dAvgAFTT = 0
          End If

          If Not IsDBNull(rstRec1("dAvgDOM")) Then
            dAvgDOM = rstRec1("dAvgDOM")
          Else
            dAvgDOM = 0
          End If

          '----------------------------- all changed to strHTMLData1_2 to mimick order of table - msw - 8/30/2011

          strHTMLData1_2 = "<tr><td align='left' nowrap='nowrap'>" & strYearQtrName & "</td>"

          strHTMLData1_2 = strHTMLData1_2 & "<td align='center'>"
          If dAvgYearMfr > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & CStr(dAvgYearMfr) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='center'>"
          If dAvgYearDlv > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & CStr(dAvgYearDlv) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dAvgAsking > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & "$" & FormatNumber(dAvgAsking / 1000, 0, True) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dAvgSelling > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & "$" & FormatNumber(dAvgSelling / 1000, 0, True) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dPercent > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dPercent, 1, True) & "%</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dAvgAsking > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dVariance, 1, True) & "%</td>"
          ElseIf dAvgAsking = dAvgSelling Then
            strHTMLData1_2 = strHTMLData1_2 & "0.0%</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dAvgAFTT > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dAvgAFTT, 0, True) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "<td align='right'>"
          If dAvgDOM > 0 Then
            strHTMLData1_2 = strHTMLData1_2 & FormatNumber(dAvgDOM, 0, True) & "</td>"
          Else
            strHTMLData1_2 = strHTMLData1_2 & "&nbsp;</td>"
          End If

          strHTMLData1_2 = strHTMLData1_2 & "</tr>"

          '--------------------------------------

          strHTMLData1_2_final = strHTMLData1_2 & strHTMLData1_2_final


          ' Percentage Of Asking Price  
          If dAvgAsking > 0 Then
            lCnt1 = lCnt1 + 1
            aLabels1(lCnt1 - 1) = strYearQtrName
            aData1(lCnt1 - 1) = CDbl(FormatNumber(dPercent, 1, True))
          End If

          ' Variance Of Asking Price  
          If dAvgAsking > 0 Then
            lCnt2 = lCnt2 + 1
            aLabels2(lCnt2 - 1) = strYearQtrName
            aData2(lCnt2 - 1) = CDbl(FormatNumber(dVariance, 1, True))
          End If

          ' Asking Price        
          If dAvgAsking > 0 Then
            lCnt3 = lCnt3 + 1
            aLabels3(lCnt3 - 1) = strYearQtrName
            aData3(lCnt3 - 1) = CDbl(FormatNumber(dAvgAsking / 1000, 1, True))
          End If

          ' Selling Price        
          If dAvgSelling > 0 Then
            lCnt4 = lCnt4 + 1
            aLabels4(lCnt4 - 1) = strYearQtrName
            aData4(lCnt4 - 1) = CDbl(FormatNumber(dAvgSelling / 1000, 1, True))
          End If

          ' Asking Price        
          lCnt5a = lCnt5a + 1
          aLabels5a(lCnt5a - 1) = strYearQtrName
          aData5a(lCnt5a - 1) = CDbl(FormatNumber(dAvgAsking / 1000, 1, True))

          ' Selling Price        
          lCnt5b = lCnt5b + 1
          aLabels5b(lCnt5b - 1) = strYearQtrName
          aData5b(lCnt5b - 1) = CDbl(FormatNumber(dAvgSelling / 1000, 1, True))

          ' Avg AFTT
          If dAvgAFTT > 0 Then
            lCnt6 = lCnt6 + 1
            aLabels6(lCnt6 - 1) = strYearQtrName
            aData6(lCnt6 - 1) = CLng(FormatNumber(dAvgAFTT, 0, True))
          End If

          ' Avg DOM
          If dAvgDOM > 0 Then
            lCnt7 = lCnt7 + 1
            aLabels7(lCnt7 - 1) = strYearQtrName
            aData7(lCnt7 - 1) = CLng(FormatNumber(dAvgDOM, 0, True))
          End If

          ' Avg AFTT vs Variance
          If dAvgAFTT > 0 And dAvgAsking > 0 Then
            lCnt8 = lCnt8 + 1
            aLabels8(lCnt8 - 1) = FormatNumber(dAvgAFTT, 0, True)
            aData8(lCnt8 - 1) = CDbl(FormatNumber(dVariance, 1, True))
          End If

          ' Avg DOM vs Variance
          If dAvgDOM > 0 And dAvgAsking > 0 Then
            lCnt9 = lCnt9 + 1
            aLabels9(lCnt9 - 1) = FormatNumber(dAvgDOM, 0, True)
            aData9(lCnt9 - 1) = CDbl(FormatNumber(dVariance, 1, True))
          End If

          ' Avg AFTT vs Selling Price
          If dAvgAFTT > 0 And dAvgSelling > 0 Then
            lCnt10 = lCnt10 + 1
            aLabels10(lCnt10 - 1) = FormatNumber(dAvgAFTT, 0, True)
            aData10(lCnt10 - 1) = CDbl(FormatNumber((dAvgSelling / 1000), 0, True))
          End If

          ' Avg DOM vs Selling Price
          If dAvgDOM > 0 And dAvgSelling > 0 Then
            lCnt11 = lCnt11 + 1
            aLabels11(lCnt11 - 1) = FormatNumber(dAvgDOM, 0, True)
            aData11(lCnt11 - 1) = CDbl(FormatNumber((dAvgSelling / 1000), 0, True))
          End If



        Loop

        're_order_arrays(aLabels2, aData2)



        '  Graph Types
        '  1=2D-Pie,        2=3D Pie
        '  3=2D Bar,        4=3D Bar
        '  6=Line,          7=Line 
        '  8=Area,          9=Speckle
        ' 10=Circle Line,  13=3D Ribbon 
        ' 14=3D-Area,      15=Line 
        ' 16=Line,         17=+/- Bar







        If lCnt1 > 1 Then

          Session("SPImageWidth") = Session("lgImageWidth")
          Session("SPImageHeight") = Session("l gImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle1 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Percentage of Asking Price (%)"
          strBottomTitle1 = "Year/Quarter Sold"    ' Y      
          strLeftTitle1 = "Percentage (%)"

          ReDim Preserve aData1(lCnt1)
          ReDim Preserve aLabels1(lCnt1)

          '       SortLabelsValue(aLabels1, aData1, True)

          tmpGraph = CreateAndGraphData(strTitle1, strBottomTitle1, strLeftTitle1, lGraphType, aLabels1, aData1, 0, "", "##.0", 0, 0, color)
          If tmpGraph.ToString.Length > 2 Then
            ' If Trim(tmpGraph) <> "" Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle1)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_1.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_1.jpg'></td>")
            Me.SPI_QUARTER.Series.Clear()
            'End If

          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
          End If
        Else
          ReturnQuarterlyByModel += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
        End If ' If lCnt1 > 1 Then

        If lCnt2 > 1 Then

          Session("SPImageWidth") = Session("lgImageWidth")
          Session("SPImageHeight") = Session("lgImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle2 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Variance of Asking Price (%)"
          strBottomTitle2 = "Year/Quarter Sold"    ' Y
          strLeftTitle2 = "Variance (%)"

          ReDim Preserve aData2(lCnt2)
          ReDim Preserve aLabels2(lCnt2)

          '    SortLabelsValue(aLabels2, aData2, True)

          tmpGraph = CreateAndGraphData(strTitle2, strBottomTitle2, strLeftTitle2, lGraphType, aLabels2, aData2, 0, "", "##.0", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle2)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_2.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_2.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt2 > 1 Then




        If lCnt5a > 1 And lCnt5b > 1 Then

          Session("SPImageWidth") = Session("lgImageWidth")
          Session("SPImageHeight") = Session("lgImageHeight")

          lGraphType = 7
          strTitle5 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Asking vs Selling Price (k)"

          strBottomTitle5 = "Year Sold-Quarter"    ' Y
          strRightTitle5 = "Price ($)"
          strLeftTitle5 = "Price ($)"

          ReDim Preserve aData5a(lCnt5a)
          ReDim Preserve aData5b(lCnt5b)
          ReDim Preserve aLabels5a(lCnt5a)
          ReDim Preserve aLabels5b(lCnt5b)

          If Not Session("localMachine") Then

            '  objGraph = Server.CreateObject("GSSERVER.GSServerProp")

            ' If lMaxSets = 2 Then

            ' SortLabelsWithTwoDataValues(aLabels5a, aLabels5b, aData5a, aData5b, True, 1)

            '  SetMultiLineGraphTitleStyleLabels(objGraph, lMaxSets, lGraphType, strTitle5, strBottomTitle5, strLeftTitle5, strRightTitle5, aLabels5a, "", "#,###", 2)
            '   End If


            '    SetMultiLineGraphAddData(objGraph, lThisSet, aData5a, 5, 0, "Avg Asking Price")
            min_max = CreateAndGraphData(strTitle5, strBottomTitle5, strLeftTitle5, lGraphType, aLabels5b, aData5b, 0, "", "", min, max, color)


            min = Left(min_max, (InStr(min_max, ",") - 1))
            max = Right(min_max, (min_max.ToString.Length - InStr(min_max, ",")))
            '  SetMultiLineGraphAddData(objGraph, lThisSet, aData5b, 1, 0, "Avg Selling Price")
            tmpGraph = CreateAndGraphData(strTitle5 & "&nbsp;", strBottomTitle5, strLeftTitle5, lGraphType, aLabels5a, aData5a, 0, "", "", min, max, color)

            '       strGraphAskingVsSelling = DrawMultiLineGraphs(objGraph)

            If tmpGraph.ToString.Length > 2 Then
              Me.SPI_QUARTER.Titles.Clear()
              Me.SPI_QUARTER.Titles.Add(strTitle5)
              Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
              Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_5.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
              ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_5.jpg'><br>")
              ReturnQuarterlyByModel += tmpGraph
              ReturnQuarterlyByModel += "</td>"
              Me.SPI_QUARTER.Series.Clear()
            Else
              ReturnQuarterlyByModel += "<tr><td align='center'>Asking vs selling Price (k)<br />Not Enough Data Availabl</td>"
            End If
          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Asking vs Selling Price (k)<br />Not Enough Data Availabl</td>"

            Me.SPI_QUARTER.Series.Clear()
          End If ' If lCnt5a > 1 And lCnt5b > 1 Then      


        End If








        If lCnt3 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle3 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Asking Price (k)"
          strBottomTitle3 = "Year/Quarter Sold"    ' Y
          strLeftTitle3 = "Price ($)"

          ReDim Preserve aData3(lCnt3)
          ReDim Preserve aLabels3(lCnt3)

          '   SortLabelsValue(aLabels3, aData3, True)

          tmpGraph = CreateAndGraphData(strTitle3, strBottomTitle3, strLeftTitle3, lGraphType, aLabels3, aData3, 0, "", "#,###", 0, 0, color)
          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle3)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_3.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_3.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<td align='center'>Asking Price (k)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<td align='center'>Asking Price (k)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt3 > 1 Then



        If lCnt4 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle4 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Selling Price (k)"
          strBottomTitle4 = "Year/Quarter Sold"    ' Y
          strLeftTitle4 = "Price ($)"
          ReDim Preserve aData4(lCnt4)
          ReDim Preserve aLabels4(lCnt4)

          '  SortLabelsValue(aLabels4, aData4, True)

          tmpGraph = CreateAndGraphData(strTitle4, strBottomTitle4, strLeftTitle4, lGraphType, aLabels4, aData4, 0, "", "#,###", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle4)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_4.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_4.jpg'></td>")
            Me.SPI_QUARTER.Series.Clear()

          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Selling Price (k)<br />Not Enough Data Available</td>"
          End If
        Else
          ReturnQuarterlyByModel += "<tr><td align='center'>Selling Price (k)<br />Not Enough Data Available</td>"
        End If ' If lCnt4 > 1 Then




        If lCnt6 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle6 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Average Airframe Total Time"
          strBottomTitle6 = "Year/Quarter Sold"    ' Y


          ReDim Preserve aData6(lCnt6)
          ReDim Preserve aLabels6(lCnt6)

          '  SortLabelsValue(aLabels6, aData6, True)

          tmpGraph = CreateAndGraphData(strTitle6, strBottomTitle6, strLeftTitle6, lGraphType, aLabels6, aData6, 0, "", "#,###", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle6)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_6.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_6.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<td align='center'>Average Airframe Total Time<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<td align='center'>Average Airframe Total Time<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt6 > 1 Then


        If lCnt7 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle7 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Average Days On Market"
          strBottomTitle7 = "Year/Quarter Sold"    ' Y


          ReDim Preserve aData7(lCnt7)
          ReDim Preserve aLabels7(lCnt7)

          ' SortLabelsValue(aLabels7, aData7, True)

          tmpGraph = CreateAndGraphData(strTitle7, strBottomTitle7, strLeftTitle7, lGraphType, aLabels7, aData7, 0, "", "#,###", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle7)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_7.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_7.jpg'></td>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Average Days On Market<br />Not Enough Data Available</td>"
          End If
        Else
          ReturnQuarterlyByModel += "<tr><td align='center'>Average Days On Market<br />Not Enough Data Available</td>"
        End If ' If lCnt7 > 1 Then


        If lCnt8 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle8 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Avg AFTT vs Variance (%)"
          strBottomTitle8 = "Average Airframe Total Time"     ' Y
          ' X    

          ReDim Preserve aData8(lCnt8)
          ReDim Preserve aLabels8(lCnt8)

          SortLabelsValue(aLabels8, aData8, lCnt8 - 1, 1)

          ReDim Preserve aData8(lCnt8)
          ReDim Preserve aLabels8(lCnt8)
          '   SortLabelsValue(aLabels8, aData8, True)

          tmpGraph = CreateAndGraphData(strTitle8, strBottomTitle8, strLeftTitle8, lGraphType, aLabels8, aData8, 0, "#,###", "##.0", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle8)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_8.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_8.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<td align='center'>Avg AFTT vs Variance (%)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<td align='center'>Avg AFTT vs Variance (%)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt8 > 1 Then


        If lCnt9 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle9 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Avg DOM vs Variance (%)"
          strBottomTitle9 = "Average Days on Market"     ' Y
          ' X    

          ReDim Preserve aData9(lCnt9 - 1)
          ReDim Preserve aLabels9(lCnt9 - 1)

          SortLabelsValue(aLabels9, aData9, lCnt9 - 1, 1)

          ReDim Preserve aData9(lCnt9)
          ReDim Preserve aLabels9(lCnt9)
          '  SortLabelsValue(aLabels9, aData9, True)

          tmpGraph = CreateAndGraphData(strTitle9, strBottomTitle9, strLeftTitle9, lGraphType, aLabels9, aData9, 0, "#,###", "##.0", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle9)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_9.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_9.jpg'></td>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Avg DOM vs Variance (%)<br />Not Enough Data Available</td>"
          End If
        Else
          ReturnQuarterlyByModel += "<tr><td align='center'>Avg DOM vs Variance (%)<br />Not Enough Data Available</td>"
        End If ' If lCnt9 > 1 Then


        If lCnt10 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle10 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Avg AFTT vs Selling Price (k)"
          strBottomTitle10 = "Average Airframe Total Time"     ' Y


          ReDim Preserve aData10(lCnt10 - 1)
          ReDim Preserve aLabels10(lCnt10 - 1)

          SortLabelsValue(aLabels10, aData10, lCnt10 - 1, 1)

          ReDim Preserve aData10(lCnt10)
          ReDim Preserve aLabels10(lCnt10)
          'SortLabelsValue(aLabels10, aData10)



          tmpGraph = CreateAndGraphData(strTitle10, strBottomTitle10, strLeftTitle10, lGraphType, aLabels10, aData10, 0, "#,###", "#,###", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle10)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_10.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_10.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<td align='center'>AFTT vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<td align='center'>AFTT vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt10 > 1 Then

        If lCnt11 > 1 Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle11 = Session("salesPriceViewMake") & "/" & Session("salesPriceViewModel") & " - Avg DOM vs Selling Price (k)"
          strBottomTitle11 = "Average Days on Market"     ' Y



          ReDim Preserve aData11(lCnt11 - 1)
          ReDim Preserve aLabels11(lCnt11 - 1)

          SortLabelsValue(aLabels11, aData11, lCnt11 - 1, 1)

          ReDim Preserve aData11(lCnt11)
          ReDim Preserve aLabels11(lCnt11)

          ' Array.Sort(aData11, aLabels11)
          '  Array.Sort(aLabels11)

          tmpGraph = CreateAndGraphData(strTitle11, strBottomTitle11, strLeftTitle11, lGraphType, aLabels11, aData11, 0, "#,###", "#,###", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle11)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_11.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            ReturnQuarterlyByModel += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_11.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            ReturnQuarterlyByModel += "<tr><td align='center'>Avg DOM vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          ReturnQuarterlyByModel += "<tr><td align='center'>Avg DOM vs Selling Price (k)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt11 > 1 Then

        rstRec1.Close()

      Else

        strHTMLData1 = "<tr><td align='center' colspan='9'>No Records Found</td></tr>"

      End If





      strHTMLData1 = strHTMLData1 & strHTMLData1_2_final

      strHTMLData1 = strHTMLData1 & "</table>"


      ReturnQuarterlyByModel = "<table width='100%' cellspacing='0' cellpadding='0'>" & ReturnQuarterlyByModel & "</table>"
      string_for_spi_start = strHTMLData1
      rstRec1 = Nothing



    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function ' ReturnQuarterlyByModel
  Function CreateAndGraphData(ByVal strTitle,
                                  ByVal strBottomTitle,
                                  ByVal strLeftTitle,
                                  ByVal lGraphType,
                                  ByRef aLabels,
                                  ByRef aData,
                                  ByVal lDivBy,
                                  ByVal strXFormatString,
                                  ByVal strYFormatString,
                                  ByVal current_min,
                                  ByVal current_max,
                                    ByVal color) As String
    CreateAndGraphData = ""
    Dim series_title As String = ""
    Dim text_legend As String = ""
    Dim temp_color As String = ""

    series_title = strTitle ' for the series to be consistent, gets replaced later on 



    If lGraphType = 1 Then '  1=2D-Pie
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Pie
    ElseIf lGraphType = 2 Then '  2=3D Pie
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Pie
      'rotate
    ElseIf lGraphType = 3 Then '  3=2D Bar
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
    ElseIf lGraphType = 4 Then '  4=3D Bar
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
      'rotate
    ElseIf lGraphType = 5 Then  '  5=Gantt
      '   Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.
    ElseIf lGraphType = 6 Then '  6=Line 
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Line
    ElseIf lGraphType = 7 Then '  7=Log/lin 
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Line

    ElseIf lGraphType = 8 Then '  8=2D Area 
      Me.SPI_QUARTER.Series.Add(series_title).ChartType = UI.DataVisualization.Charting.SeriesChartType.Area
    End If



    Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Title = strLeftTitle  ' passed in 
    Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.Title = strBottomTitle



    Me.SPI_QUARTER.Series(series_title).YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
    Me.SPI_QUARTER.Series(series_title).XValueType = UI.DataVisualization.Charting.ChartValueType.String



    '-------------------------------------------------
    '  Graph Types
    '  1=2D-Pie
    '  2=3D Pie
    '  3=2D Bar
    '  4=3D Bar
    '  5=Gantt
    '  6=Line 
    '  7=Log/lin 
    '  8=2D Area 
    '  9=2D Scatter
    ' 10=Polar/Circle Line
    ' 11=High-low-close
    ' 12=Bubble
    ' 13=3D Ribbon 
    ' 14=3D Area 
    ' 15=Log/log
    ' 16=Lin/log
    ' 17=Box-whisker +/- Bar 
    ' 18=Open-high-low-close
    ' 19=Candlestick
    ' 20=3D Survace
    ' 21=3D Scatter

    '
    '
    '
    '
    '
    '  Y Axis (Left)
    '    .
    '   .
    '  .
    ' .               X Axis (Bottom)
    '----------------------------------
    Try


      If lGraphType = 7 Then

        Me.SPI_QUARTER.Series(series_title).BorderWidth = 2


        If Right(series_title, 1) = ";" Then

          If color = "Blue" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
          ElseIf color = "Navy" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Navy
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Navy
          ElseIf color = "Light Gray" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.LightSlateGray
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.LightSlateGray
          ElseIf color = "Gray" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Gray
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Gray
          ElseIf color = "Dark Gray" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.DarkSlateGray
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.DarkSlateGray
          ElseIf color = "Black" Then
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Black
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Black
          Else
            Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue ' set to blue for default 
            Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
          End If

          If color = "Blue" Then
            temp_color = "Blue"
          ElseIf color = "Navy" Then
            temp_color = "#0000A0"
          ElseIf color = "Light Gray" Then
            temp_color = "#A0A0A0"
          ElseIf color = "Gray" Then
            temp_color = "Gray"
          ElseIf color = "Dark Gray" Then
            temp_color = "#25383C"
          ElseIf color = "Black" Then
            temp_color = "Black"
          Else
            temp_color = "Blue"
          End If


          text_legend = "<table align='center' valign='top' width='50%' border='1'>"
          text_legend += "<tr><td align='left' bgcolor='red'><font size='-2' color='white'>Avg Selling Price:</font></td><td align='left' bgcolor='red'> <font size='-2' color='white'>Red</font></td></tr>"
          text_legend += "<tr><td align='left' bgcolor='" & temp_color & "'><font size='-2' color='white'>Avg Asking Price:</font></td><td align='left' bgcolor='" & temp_color & "'> <font size='-2' color='white'>" & color & "</font></td></tr>"
          text_legend += "</table>"
        Else

          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Red  ' set to blue for default 
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Red


        End If





      Else
        If color = "Blue" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
        ElseIf color = "Navy" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Navy
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Navy
        ElseIf color = "Light Gray" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.LightSlateGray
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.LightSlateGray
        ElseIf color = "Gray" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Gray
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Gray
        ElseIf color = "Dark Gray" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.DarkSlateGray
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.DarkSlateGray
        ElseIf color = "Black" Then
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Black
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Black
        Else
          Me.SPI_QUARTER.Series(series_title).LabelForeColor = Drawing.Color.Blue ' set to blue for default 
          Me.SPI_QUARTER.Series(series_title).Color = Drawing.Color.Blue
        End If
      End If
      '--------------------------
      ' Color
      '  1=Dark Red
      '  2=Red
      '  3=Green
      '  4=Purple
      '  5=Dk Blue
      '  6=Blue
      '  7=Lt Grey
      '  8=Dk Grey
      '  9=Lt Purple
      ' 10=Lt Bue
      ' 11=Lt Green
      ' 12=Lime Green
      ' 13=Yellow
      ' 14=Gold
      ' 15=White 
      ' 16=Blank




      Dim strGraphPath As String = ""
      Dim strGraphURL As String = ""
      Dim lData As Integer = 0
      Dim strLabel As String = ""


      Dim lMax As Double = 0
      Dim lMin As Double = 0

      Dim lCnt1 As Integer = 0

      Dim strHTML As String = ""

      Dim strImageWidth As Integer = 0
      Dim strImageHeight As Integer = 0

      Dim lWidth As Integer = 0
      Dim lHeight As Integer = 0



      If CLng(Session("SPGraphWidth")) > 0 Then
        lWidth = CInt(Session("SPGraphWidth"))
      End If

      If CLng(Session("SPGraphHeight")) > 0 Then
        lHeight = CInt(Session("SPGraphHeight"))
      End If

      strHTML = ""

      If UBound(aData) >= 1 And Not Session("localMachine") Then


        If UBound(aData) > 1 Then
          If (UBound(aData) / 2) > 100 Then
            '   objGraph.YAxisTicks = 100
          Else
            '  objGraph.YAxisTicks = (UBound(aData) / 2)
          End If
        Else
          '  objGraph.YAxisTicks = 1
        End If


        If current_min > 0 Or current_max > 0 Then
          lMin = current_min
          lMax = current_max
        Else
          lMin = 999999999
          lMax = -999999999
        End If




        For lCnt1 = 1 To UBound(aData)

          If Not IsNothing(aData(lCnt1 - 1)) Then

            Me.SPI_QUARTER.Series(series_title).Points.AddXY(aLabels(lCnt1 - 1), aData(lCnt1 - 1))
            '  strLabel = aLabels(lCnt1 - 1)
            '  objGraph.Label(lCnt1) = strLabel & "   "

            lData = CDbl(aData(lCnt1 - 1))

            If lDivBy > 0 Then
              lData = lData \ lDivBy
            End If



            If lData > lMax Then
              lMax = lData
            End If
            If lData < lMin Then
              lMin = lData
            End If
          End If
        Next ' lCnt1




        Dim temp_counter As Integer = 0
        Dim temp_ten As Double = 10
        Dim temp_max As Double = 0
        Dim temp_min As Integer = 0
        Dim i As Integer = 1
        Dim label_count As Integer = 0
        Dim found As Integer = 0
        Dim found2 As Integer = 0
        label_count = UBound(aLabels)

        If lMax <= 0 Then
          temp_counter = -10
        ElseIf lMax <= 100 Then ' 100 
          temp_counter = 10
        ElseIf lMax <= 1000 Then  ' 1 thousand 
          temp_counter = 100
        ElseIf lMax <= 10000 Then ' ten thousand 
          temp_counter = 1000
        ElseIf lMax <= 100000 Then  ' 100 thousand 
          temp_counter = 10000
        ElseIf lMax <= 1000000 Then ' 1 mill 
          temp_counter = 100000
        ElseIf lMax <= 10000000 Then ' ten mill 
          temp_counter = 1000000
        ElseIf lMax <= 100000000 Then ' one hundred mill
          temp_counter = 10000000
        End If

        temp_max = lMax
        If lMax >= 0 Then
          For i = 1 To 20
            If lMax >= (temp_counter * i) Then
              temp_max = i + 1
            Else
              i = 20
            End If
          Next
        Else
          For i = 1 To 20
            If lMax >= (temp_counter * i * -1) Then
              temp_max = (i - 1) * -1
              i = 20
            End If

          Next
        End If

        temp_min = lMin

        If lMin >= 0 Then
          For i = 1 To 20
            If lMin <= (temp_counter * i) Then
              temp_min = i - 1
              i = 20
            End If
          Next
        Else
          For i = 1 To 20
            If lMin >= (temp_counter * i * -1) Then
              temp_min = (i) * -1
              i = 20
            End If

          Next
        End If




        ' even more precision 
        If lMax >= 0 Then
          For i = 1 To 10
            If (temp_counter + (i * (temp_counter / 10))) >= lMax Then
              found = (temp_counter + (i * (temp_counter / 10)))
              i = 10
            End If
          Next
        End If


        If lMin < 0 Then
          For i = 1 To 10
            If ((temp_counter * -1) - (i * (temp_counter / 10))) <= lMin Then
              found2 = ((temp_counter * -1) - (i * (temp_counter / 10)))
              i = 10
            End If
          Next
        End If




        ' 10,000    *       2 =         20,000
        If found > 0 Then
          lMax = found
        Else
          lMax = (temp_counter * temp_max)
        End If

        If found2 < 0 Then
          lMin = found2
        Else
          lMin = (temp_counter * temp_min)
        End If



        '  Me.SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        '  Me.SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Rotation = 10

        Me.SPI_QUARTER.Series(series_title).MarkerSize = 5
        Me.SPI_QUARTER.Series(series_title).MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle





        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.Angle = -90
        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.Interval = 1
        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelStyle.IsEndLabelVisible = False
        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.IsLabelAutoFit = True
        '  Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitStyle = DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont
        ' Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitMinFontSize = 5
        'Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisX.LabelAutoFitMaxFontSize = 5







        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Minimum = lMin
        Me.SPI_QUARTER.ChartAreas("ChartArea1").AxisY.Maximum = lMax
        '   Me.SPI_QUARTER.ChartAreas("ChartArea1").Area3DStyle.Rotation = 100




        Me.SPI_QUARTER.Width = 250
        Me.SPI_QUARTER.Height = 250



      End If ' If UBound(aData) >= 1 Then

      If text_legend.ToString.Trim <> "" Then
        CreateAndGraphData = text_legend.ToString.Trim    ' if there is a legend ( for the one line graph) return the legend 
      Else
        CreateAndGraphData = lMin & "," & lMax
      End If

    Catch ex As Exception
    Finally

    End Try
  End Function ' CreateAndGraphData
  Public Function ReturnPreviousFullQuarterlyByWeightClass(ByRef cntConn, ByVal lModelId, ByRef strHTMLData2, ByRef strGraphPercentAsking, ByRef strGraphVarianceAsking, ByVal weight_class, ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal sub_info, ByVal real_make_model_name, ByVal sub_type, ByVal color) As String
    ReturnPreviousFullQuarterlyByWeightClass = ""



    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rstRec1 As System.Data.SqlClient.SqlDataReader : rstRec1 = Nothing

    Dim strQuery1 As String = ""

    Dim strYearSld As String = ""
    Dim strQuarterSld As String = ""
    Dim strYearQtrName As String = ""

    Dim strMakeAbbrev As String = ""
    Dim lAModID As Integer = 0
    Dim strAModId As String = ""
    Dim strWeightClass As String = ""
    Dim strWeightClassName As String = ""

    Dim strAvgYearMfr As String = ""
    Dim strAvgYearDlv As String = ""
    Dim strAvgAsking As String = ""
    Dim strAvgSelling As String = ""
    Dim strPercent As String = ""
    Dim strVariance As String = ""
    Dim strAvgAFTT As String = ""
    Dim strAvgDOM As String = ""

    Dim dAvgYearMfr As Double = 0
    Dim dAvgYearDlv As Double = 0
    Dim dAvgAsking As Double = 0
    Dim dAvgSelling As Double = 0
    Dim dPercent As Double = 0
    Dim dVariance As Double = 0
    Dim dAvgAFTT As Double = 0
    Dim dAvgDOM As Double = 0

    Dim lRec1 As String = ""
    Dim strHRef As String = ""

    Dim lColSpan As Integer = 0
    Dim lTotRec As Double = 0

    Dim lGraphType As Integer = 0
    Dim strGraphImage As String = ""

    ' Percentage Of Asking Price     
    Dim strTitle1 As String = ""
    Dim strBottomTitle1 As String = ""
    Dim strLeftTitle1 As String = ""
    Dim lCnt1 As Integer = 0
    Dim aData1()
    Dim aLabels1()

    ' Variance Of Asking Price     
    Dim strTitle2 As String = ""
    Dim strBottomTitle2 As String = ""
    Dim strLeftTitle2 As String = ""
    Dim lCnt2 As Integer = 0
    Dim aData2()
    Dim aLabels2()

    Dim strModel As String = ""
    Dim strMake As String = ""
    Dim lYearSld As Integer = 0
    Dim lQuarterSld As Integer = 0

    Dim tmpGraph As String = ""

    Try
      strGraphPercentAsking = "Percentage of Asking Price (%)<br />Not Enough Data Available"
      strGraphVarianceAsking = "Variance of Asking Price (%)<br />Not Enough Data Available"

      ' Clear All Variables Passed By Reference

      strHTMLData2 = ""

      strQuery1 = "SELECT amod_id As AModId, amod_make_name As Make, amod_make_abbrev As MakeAbbrev, amod_model_name As Model,"
      strQuery1 = strQuery1 & " AVG(CAST(ac_mfr_year AS INT)) As dAvgYearMfr,"
      strQuery1 = strQuery1 & " AVG(CAST(ac_year AS INT)) As dAvgYearDlv,"
      strQuery1 = strQuery1 & " AVG(ac_asking_price) As dAvgAsking,"
      strQuery1 = strQuery1 & " AVG(ac_hidden_asking_price) As dAvgAskingHidden,"
      strQuery1 = strQuery1 & " AVG(ac_sale_price) As dAvgSelling,"
      strQuery1 = strQuery1 & " ((AVG(ac_sale_price)/AVG(ac_asking_price)) * 100) As dPercent,"
      strQuery1 = strQuery1 & " ((1-(AVG(ac_sale_price)/AVG(ac_asking_price))) * 100) As dVariance,"
      strQuery1 = strQuery1 & " ((AVG(ac_sale_price)/AVG(ac_hidden_asking_price)) * 100) As dPercentHidden,"
      strQuery1 = strQuery1 & " ((1-(AVG(ac_sale_price)/AVG(ac_hidden_asking_price))) * 100) As dVarianceHidden,"
      strQuery1 = strQuery1 & " AVG(ac_airframe_tot_hrs) As dAvgAFTT,"
      strQuery1 = strQuery1 & " AVG(DateDiff(day,ac_list_date, journ_date)) As dAvgDOM"

      strQuery1 = strQuery1 & " FROM Aircraft_Summary_SPI WITH (NOLOCK)"
      'strQuery1 = strQuery1 & " FROM Aircraft WITH (NOLOCK, INDEX(ix_ac_sale_price_ac_id_journ_id_key))"
      'strQuery1 = strQuery1 & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id"
      'strQuery1 = strQuery1 & " INNER JOIN Journal WITH (NOLOCK) ON ac_id = journ_ac_id AND ac_journ_id = journ_id"
      'strQuery1 = strQuery1 & " INNER JOIN Journal_Category WITH (NOLOCK) ON journ_subcategory_code = jcat_subcategory_code"



      strQuery1 = strQuery1 & " WHERE (ac_journ_id > 0)"
      strQuery1 = strQuery1 & " AND (ac_lifecycle_stage = 3)"                   '-- In Operation Only
      strQuery1 = strQuery1 & " AND (jcat_used_retail_sales_flag = 'Y')"        '-- Retail Only    
      strQuery1 = strQuery1 & " AND (journ_newac_flag = 'N')"                   '-- Used Sales Only
      strQuery1 = strQuery1 & " AND (journ_subcategory_code LIKE 'WS%')"        '-- Whole Sales Only
      strQuery1 = strQuery1 & " AND (journ_subcategory_code NOT LIKE '%IT%')"   '-- No Internals
      strQuery1 = strQuery1 & " AND (journ_internal_trans_flag = 'N')"          '-- No Internals

      If lModelId > 0 Then
        strQuery1 = strQuery1 & " AND (amod_id <> " & CStr(lModelId) & ")"
      End If

      Select Case CLng(Session("salesPriceViewAirframeType"))
        Case Is = 1
          strQuery1 = strQuery1 & " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')"
        Case Is = 2
          strQuery1 = strQuery1 & " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')"
        Case Is = 3
          strQuery1 = strQuery1 & " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')"
        Case Is = 4
          strQuery1 = strQuery1 & " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')"
        Case Is = 5
          strQuery1 = strQuery1 & " AND (amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))"
      End Select

      If Trim(Session("salesPriceViewWtCls")) <> "" Then

        If InStr(1, Session("salesPriceViewWtCls"), ",") = 0 Then
          strQuery1 = strQuery1 & " AND (amod_weight_class = '" & Session("salesPriceViewWtCls") & "')"
        Else
          strQuery1 = strQuery1 & " AND (amod_weight_class IN ('" & Replace(Session("salesPriceViewWtCls"), ",", "','") & "'))"
        End If

      End If

      strQuery1 = strQuery1 & " AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0)"

      If Trim(Session("SPYearSld2")) > 0 Then
        strQuery1 = strQuery1 & " AND (DATEPART(year,journ_date) >= " & Session("SPYearSld2") & ")"
        strQuery1 = strQuery1 & " AND (DATEPART(quarter,journ_date) = 1)"
      End If


      strQuery1 = strQuery1 & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      strQuery1 = strQuery1 & " GROUP BY amod_id, amod_make_name, amod_make_abbrev, amod_model_name"
      strQuery1 = strQuery1 & " ORDER BY amod_make_name, amod_model_name asc"

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>ReturnPreviousFullQuarterlyByWeightClass : " & Server.HtmlEncode(strQuery1) & "</b><br /><br />"
      '  End If
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function ReturnPreviousFullQuarterlyByWeightClass(ByRef cntConn, ByVal lModelId, ByRef strHTMLData2, ByRef strGraphPercentAsking, ByRef strGraphVarianceAsking, ByVal weight_class, ByVal weight_class_name, ByVal spi_year, ByVal spi_year2, ByVal airframe_type, ByVal sub_info, ByVal real_make_model_name, ByVal sub_type, ByVal color) As String</b><br />" & strQuery1

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") ' My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = strQuery1
      rstRec1 = SqlCommand.ExecuteReader()


      strHTMLData2 = "<table id='weightClassDataTable' cellpadding='2' cellspacing='0' border='1' width='100%'>"
      strHTMLData2 = strHTMLData2 & "<tr class='aircraft_list'><td align='center' colspan='9'>"
      strHTMLData2 = strHTMLData2 & "Weight Class Similar To " & Session("salesPriceViewMake") & "&nbsp;/&nbsp;" & Session("salesPriceViewModel") & "&nbsp;&nbsp;(" & Session("salesPriceViewWtClsName") & ")<br />"

      strHTMLData2 = strHTMLData2 & "Year/Quarter Sold " & Session("SPYearSld2") & " - " & ReturnYearQuarterName_PDF(Session("SPYearSld2"), 1)

      strHTMLData2 = strHTMLData2 & "</td></tr>"

      strHTMLData2 = strHTMLData2 & "<tr class='aircraft_list'><td align='center' rowspan='2'>Make<br />Model</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center' colspan='2'>Avg Year Of</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center' colspan='2'>Avg Price (k)</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center' rowspan='2'>Percent</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center' rowspan='2'>Variance</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center' colspan='2'>Average</td></tr>"

      strHTMLData2 = strHTMLData2 & "<tr class='aircraft_list'><td align='center'>Mftr</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center'>Delivery</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center'>Asking</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center'>Selling</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center'>AFTT</td>"
      strHTMLData2 = strHTMLData2 & "<td align='center'>Days<br />On<br />Mrkt</td></tr>"

      If rstRec1.HasRows Then
        lTotRec = 1000

        lCnt1 = 0
        lCnt2 = 0

        ReDim aData1(lTotRec)      ' Percentage Of Asking Price
        ReDim aLabels1(lTotRec)

        ReDim aData2(lTotRec)      ' Variance Of Asking Price
        ReDim aLabels2(lTotRec)

        Do While rstRec1.Read

          strYearSld = ""
          strQuarterSld = ""
          strYearQtrName = ""

          strModel = ""
          strMake = ""
          strMakeAbbrev = ""
          strAModId = ""

          strAvgYearMfr = ""
          strAvgYearDlv = ""
          strAvgAsking = ""
          strAvgSelling = ""
          strPercent = ""
          strVariance = ""
          strAvgAFTT = ""
          strAvgDOM = ""

          lAModID = 0
          lYearSld = 0
          lQuarterSld = 0
          dAvgYearMfr = 0.0
          dAvgYearDlv = 0.0
          dAvgAsking = 0.0
          dAvgSelling = 0.0
          dPercent = 0.0
          dVariance = 0.0
          dAvgAFTT = 0.0
          dAvgDOM = 0.0

          strMake = Trim(rstRec1("Make"))
          strMakeAbbrev = "(" & Trim(rstRec1("MakeAbbrev")) & ")"
          strModel = Trim(rstRec1("Model"))
          lAModID = rstRec1("AModId")
          strAModId = CStr(lAModID)

          If Not IsDBNull(rstRec1("dAvgYearMfr")) Then
            dAvgYearMfr = rstRec1("dAvgYearMfr")
          Else
            dAvgYearMfr = 0
          End If

          If Not IsDBNull(rstRec1("dAvgYearDlv")) Then
            dAvgYearDlv = rstRec1("dAvgYearDlv")
          Else
            dAvgYearDlv = 0
          End If

          If Not IsDBNull(rstRec1("dAvgAsking")) Then
            dAvgAsking = rstRec1("dAvgAsking")
          ElseIf Not IsDBNull(rstRec1("dAvgAskingHidden")) Then
            dAvgAsking = rstRec1("dAvgAskingHidden")
          Else
            dAvgAsking = 0
          End If

          If Not IsDBNull(rstRec1("dAvgSelling")) Then
            dAvgSelling = rstRec1("dAvgSelling")
          Else
            dAvgSelling = 0
          End If

          If Not IsDBNull(rstRec1("dPercent")) Then
            dPercent = rstRec1("dPercent")
          ElseIf Not IsDBNull(rstRec1("dPercentHidden")) Then
            dPercent = rstRec1("dPercentHidden")
          Else
            dPercent = 0
          End If

          If Not IsDBNull(rstRec1("dVariance")) Then
            dVariance = rstRec1("dVariance")
          ElseIf Not IsDBNull(rstRec1("dVarianceHidden")) Then
            dVariance = rstRec1("dVarianceHidden")
          Else
            dVariance = 0
          End If

          If Not IsDBNull(rstRec1("dAvgAFTT")) Then
            dAvgAFTT = rstRec1("dAvgAFTT")
          Else
            dAvgAFTT = 0
          End If

          If Not IsDBNull(rstRec1("dAvgDOM")) Then
            dAvgDOM = rstRec1("dAvgDOM")
          Else
            dAvgDOM = 0
          End If

          strHRef = strMakeAbbrev & " " & strModel
          strHTMLData2 = strHTMLData2 & "<tr><td align='left' nowrap='nowrap'>" & strHRef & "</td>"

          strHTMLData2 = strHTMLData2 & "<td align='center'>"
          If dAvgYearMfr > 0 Then
            strHTMLData2 = strHTMLData2 & CStr(dAvgYearMfr) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='center'>"
          If dAvgYearDlv > 0 Then
            strHTMLData2 = strHTMLData2 & CStr(dAvgYearDlv) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dAvgAsking > 0 Then
            strHTMLData2 = strHTMLData2 & "$" & FormatNumber(dAvgAsking / 1000, 0, True) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dAvgSelling > 0 Then
            strHTMLData2 = strHTMLData2 & "$" & FormatNumber(dAvgSelling / 1000, 0, True) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dPercent > 0 Then
            strHTMLData2 = strHTMLData2 & FormatNumber(dPercent, 1, True) & "%</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dAvgAsking > 0 Then
            strHTMLData2 = strHTMLData2 & FormatNumber(dVariance, 1, True) & "%</td>"
          ElseIf dAvgAsking = dAvgSelling Then
            strHTMLData2 = strHTMLData2 & "0.0%</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dAvgAFTT > 0 Then
            strHTMLData2 = strHTMLData2 & FormatNumber(dAvgAFTT, 0, True) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "<td align='right'>"
          If dAvgDOM > 0 Then
            strHTMLData2 = strHTMLData2 & FormatNumber(dAvgDOM, 0, True) & "</td>"
          Else
            strHTMLData2 = strHTMLData2 & "&nbsp;</td>"
          End If

          strHTMLData2 = strHTMLData2 & "</tr>"

          ' Percentage Of Asking Price  
          If dAvgAsking > 0 Then
            lCnt1 = lCnt1 + 1
            aLabels1(lCnt1 - 1) = strMakeAbbrev & " " & strModel
            aData1(lCnt1 - 1) = CDbl(FormatNumber(dPercent, 1, True))
          End If

          ' Variance Of Asking Price  
          If dAvgAsking > 0 Then
            lCnt2 = lCnt2 + 1
            aLabels2(lCnt2 - 1) = strMakeAbbrev & " " & strModel
            aData2(lCnt2 - 1) = CDbl(FormatNumber(dVariance, 1, True))
          End If


        Loop

        'Graph(Types)
        '  1=2D-Pie,        2=3D Pie
        '  3=2D Bar,        4=3D Bar
        '  6=Line,          7=Line 
        '  8=Area,          9=Speckle
        ' 10=Circle Line,  13=3D Ribbon 
        ' 14=3D-Area,      15=Line 
        ' 16=Line,         17=+/- Bar


        strHTMLData2 += "<tr><td colspan='10'><table cellpadding='0' cellspacing='0' width='100%'>"

        If lCnt1 > 1 And Not Session("localMachine") Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle1 = "Weight Class (" & Session("salesPriceViewWtClsName") & ") - Percentage of Asking Price (%)"
          strBottomTitle1 = "Make/Model(s)"    ' Y      


          '      lCnt1 = delete_bad_data_from_graphs(aLabels1, aData1, lCnt1)


          ReDim Preserve aData1(lCnt1)
          ReDim Preserve aLabels1(lCnt1)

          SortLabelsValue(aLabels1, aData1, lCnt1 - 1, 2)


          tmpGraph = CreateAndGraphData(strTitle1, strBottomTitle1, strLeftTitle1, lGraphType, aLabels1, aData1, 0, "", "##.0", 0, 0, color)

          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle1)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_W.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            strHTMLData2 += ("<tr><td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_W.jpg'></td>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            strHTMLData2 += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
          End If
        Else
          strHTMLData2 += "<tr><td align='center'>Percentage of Asking Price (%)<br />Not Enough Data Available</td>"
        End If ' If lCnt1 > 1 Then


        If lCnt2 > 1 And Not Session("localMachine") Then

          Session("SPImageWidth") = Session("smImageWidth")
          Session("SPImageHeight") = Session("smImageHeight")

          lGraphType = Session("SPSingleGraphType")
          strTitle2 = "Weight Class (" & Session("salesPriceViewWtClsName") & ") - Variance of Asking Price (%)"
          strBottomTitle2 = "Make/Model(s)"    ' Y

          ReDim Preserve aData2(lCnt2)
          ReDim Preserve aLabels2(lCnt2)


          SortLabelsValue(aLabels2, aData2, lCnt2 - 1, 2)

          tmpGraph = CreateAndGraphData(strTitle2, strBottomTitle2, strLeftTitle2, lGraphType, aLabels2, aData2, 0, "", "##.0", 0, 0, color)
          If tmpGraph.ToString.Length > 2 Then
            Me.SPI_QUARTER.Titles.Clear()
            Me.SPI_QUARTER.Titles.Add(strTitle2)
            Me.SPI_QUARTER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            Me.SPI_QUARTER.SaveImage(Server.MapPath("TempFiles") & "\" & amod_id & "SPI_QUARTER_W_2.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            strHTMLData2 += ("<td align='center'><img src='TempFiles/" & amod_id & "SPI_QUARTER_W_2.jpg'></td></tr>")
            Me.SPI_QUARTER.Series.Clear()
          Else
            strHTMLData2 += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
          End If
        Else
          strHTMLData2 += "<td align='center'>Variance of Asking Price (%)<br />Not Enough Data Available</td></tr>"
        End If ' If lCnt2 > 1 Then

        rstRec1.Close()

      Else

        strHTMLData2 = strHTMLData2 & "<tr><td align='center' colspan='9'>No Records Found</td></tr>"

      End If

      strHTMLData2 += "</table></td></tr>"

      strHTMLData2 = strHTMLData2 & "</table>"

      rstRec1 = Nothing

      ReturnPreviousFullQuarterlyByWeightClass = strHTMLData2.ToString

    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function ' ReturnPreviousFullQuarterlyByWeightClass
  Function acContactType_companies(ByVal inAmodID, ByVal make_model_name, ByVal inCompanyID, ByVal airframe_type, ByVal country, ByVal City, ByVal sub_info, ByVal real_company_name, ByVal sub_type)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim htmlOutput As String = ""
    Dim query As String = ""
    Dim counter1 As Integer = 0
    Dim starter_string As String = ""
    Dim title As String = ""
    Dim sub_title As String = ""
    Dim page_title As String = ""
    Dim counter_show As Integer = 0
    Dim text_for_counter_to_show As String = ""
    Dim row_count As Integer = 0

    If Trim(inAmodID) = "" Then inAmodID = 0
    If Trim(inCompanyID) = "" Then inCompanyID = 0

    acContactType_companies = ""
    Try

      If CLng(inCompanyID) > 0 Then
        query = "SELECT DISTINCT comp_id, comp_address1, count(distinct ac_id) AS ac_count, comp_address2, comp_city, comp_state, comp_zip_code, comp_country"
      ElseIf CLng(inAmodID) > 0 And CLng(inCompanyID) = 0 Then
        query = "SELECT DISTINCT TOP 250 comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, comp_address2, comp_city, comp_state, comp_zip_code, comp_country"
      ElseIf Trim(country) <> "" Or Trim(City) <> "" Then
        query = "SELECT DISTINCT TOP 250 comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, comp_address2, comp_city, comp_state, comp_zip_code, comp_country"
      Else
        query = "SELECT DISTINCT TOP 50 comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, comp_address2, comp_city, comp_state, comp_zip_code, comp_country"
      End If

      query = query & " FROM aircraft_summary WITH(NOLOCK)"
      query = query & " WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))"

      If CLng(inCompanyID) > 0 Then
        query = query & " AND comp_id = " & CStr(inCompanyID)
      End If

      If CLng(inAmodID) > 0 Then
        query = query & " AND amod_id = " & CStr(inAmodID)
      End If

      If Trim(country) <> "" Then
        query = query & " and comp_country='" & country & "' "
      End If

      If Trim(City) <> "" Then
        query = query & " and comp_city='" & City & "' "
      End If

      If Trim(airframe_type) <> "" Then
        query = query & " AND amod_airframe_type_code = '" & Trim(airframe_type) & "'"
      End If

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)


      query = query & " GROUP BY comp_name, comp_id, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country"
      query = query & " ORDER BY ac_count DESC, comp_name asc"

      'If Session("debug") Then
      If CLng(inCompanyID) > 0 Then
        Session.Item("localUser").crmUser_DebugText += "<b>acContactType_companies (companyId > 0) : " & Server.HtmlEncode(query) & "</b><br /><br />"
      ElseIf CLng(inAmodID) > 0 And CLng(inCompanyID) = 0 Then
        Session.Item("localUser").crmUser_DebugText += "<b>acContactType_companies (aModId > 0 and companyId = 0) : " & Server.HtmlEncode(query) & "</b><br /><br />"
      Else
        Session.Item("localUser").crmUser_DebugText += "<b>acContactType_companies (TOP 50) : " & Server.HtmlEncode(query) & "</b><br /><br />"
      End If
      '   End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function acContactType_companies(ByVal inAmodID, ByVal make_model_name, ByVal inCompanyID, ByVal airframe_type, ByVal country, ByVal City, ByVal sub_info, ByVal real_company_name, ByVal sub_type)</b><br />" & query

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      localAdoRs2 = SqlCommand.ExecuteReader()

      If CLng(inCompanyID) > 0 Then
        title = title & "CHARTER&nbsp;OPERATOR&nbsp;INFO</em>" & vbCrLf
      ElseIf CLng(inAmodID) > 0 And CLng(inCompanyID) = 0 Then
        title = title & "CHARTER OPERATORS "
        If Trim(make_model_name) <> "" Then
          title = title & " for " & make_model_name
        End If

        If Trim(City) <> "" Then
          title = title & " in " & City
        ElseIf Trim(country) <> "" Then
          title = title & " in " & country
        End If

        title = title & " <em> by number of aircraft</em>" & vbCrLf
      ElseIf Trim(country) <> "" Or Trim(City) <> "" Then



        If Trim(airframe_type) <> "" Then
          If Trim(airframe_type) = "F" Then
            title = title & "Fixed Airframe - "
          ElseIf Trim(airframe_type) = "R" Then
            title = title & "Rotary Airframe - "
          End If
        End If
        title = title & "CHARTER OPERATORS "
        If Trim(make_model_name) <> "" Then
          title = title & " for " & make_model_name
        End If

        If Trim(City) <> "" Then
          title = title & " in " & City
        ElseIf Trim(country) <> "" Then
          title = title & " in " & country
        End If

        title = title & " <em> by number of aircraft</em>" & vbCrLf
      Else

        If Trim(airframe_type) <> "" Then
          If Trim(airframe_type) = "F" Then
            title = title & "Fixed Airframe - "
          ElseIf Trim(airframe_type) = "R" Then
            title = title & "Rotary Airframe - "
          End If
        End If
        title = title & "CHARTER OPERATORS<em> "
        If Trim(airframe_type) <> "" Then
          title = title & "<br>"
        End If
        title = title & "(Top 50) by number of aircraft</em>" & vbCrLf
      End If


      sub_title = sub_title & "<table cellspacing='0' cellpadding='0' border='0' width='100%'><tr class='aircraft_list'><td valign='middle' align='left' width='60%' colspan='2'><strong>Operator&nbsp;Name</strong></td>" & vbCrLf
      sub_title = sub_title & "<td valign='middle' align='right' colspan='2'><strong># of Aircraft&nbsp;&nbsp;</strong></td></tr>" & vbCrLf

      page_title = "charter_operators"

      If localAdoRs2.HasRows Then


        If CLng(inCompanyID) = 0 Then

          Do While localAdoRs2.Read ' selectAcContactCompany



            If CLng(localAdoRs2("ac_count").ToString) > 0 Then


              If row_count = 1 Then
                htmlOutput = htmlOutput & "<tr class='alt_row'  height='59'>" & vbCrLf
                row_count = 0
              Else
                htmlOutput = htmlOutput & "<tr bgcolor='white'  height='59'>" & vbCrLf
                row_count = 1
              End If

              htmlOutput = htmlOutput & "<td align='left' valign='top' class='seperator'><img src='../images/ch_red.jpg' class='bullet'/>&nbsp;&nbsp;</td>" & vbCrLf
              htmlOutput = htmlOutput & "<td align='left' valign='top' class='seperator'>"
              htmlOutput = htmlOutput & "<a href='details.aspx?comp_id=" & localAdoRs2("comp_id") & "&source=JETNET&type=1'>"
              htmlOutput = htmlOutput & localAdoRs2("comp_name") & "</a><br>"

              If Not IsDBNull(localAdoRs2("comp_address1")) Then
                If localAdoRs2("comp_address1") <> "" Then
                  htmlOutput = htmlOutput & localAdoRs2("comp_address1") & "<br>"
                End If
              End If

              If Not IsDBNull(localAdoRs2("comp_address2")) Then
                If localAdoRs2("comp_address2") <> "" Then
                  htmlOutput = htmlOutput & localAdoRs2("comp_address2") & "<br>"
                End If
              End If

              htmlOutput = htmlOutput & localAdoRs2("comp_city") & ","
              htmlOutput = htmlOutput & localAdoRs2("comp_state") & " "
              htmlOutput = htmlOutput & localAdoRs2("comp_zip_code") & " "
              htmlOutput = htmlOutput & localAdoRs2("comp_country")


              htmlOutput = htmlOutput & "</font></td>" & vbCrLf
              htmlOutput = htmlOutput & "<td align='left' valign='top' class='seperator'>"
              ' htmlOutput = htmlOutput & get_company_cert_images(CStr(localAdoRs2("comp_id").ToString), False)
              htmlOutput = htmlOutput & "</td>"
              htmlOutput = htmlOutput & "<td align='right' valign='top' class='seperator'>" & CStr(localAdoRs2("ac_count").ToString) & "&nbsp;&nbsp;</font></td></tr>" & vbCrLf

            End If

            counter1 = counter1 + 1
            counter_show = counter_show + 1
          Loop

        Else

          htmlOutput = htmlOutput & "<tr><td align='left' valign='top' class='seperator' colspan='3'>"
          htmlOutput = htmlOutput & "<td align='left' valign='top' class='seperator'>" & CStr(localAdoRs2("ac_count").ToString) & "</td></tr>" & vbCrLf
          htmlOutput = htmlOutput & "</td></tr>"

        End If

      Else

        sub_title += "<tr><td>No data matches for your search criteria</td></tr>"

      End If

      htmlOutput = htmlOutput & "</table>"




      If counter_show = 250 Then
        title = "TOP 250 " & title
        text_for_counter_to_show = ""
      ElseIf counter_show = 30 And CLng(inAmodID) = 0 And CLng(inCompanyID) = 0 And country.ToString.Trim = "" Or City.ToString.Trim = "" Then
        text_for_counter_to_show = ""
      Else
        text_for_counter_to_show = counter_show.ToString
      End If


      acContactType_companies = sub_title & Trim(htmlOutput)
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function display_acContactType_location_piechart_city(ByVal inCompanyID, ByVal inAmodID, ByVal airframe, ByVal Country_Name, ByVal group_by_statement, ByVal sub_info, ByVal make_model_name, ByVal real_company_name, ByVal sub_type)
    display_acContactType_location_piechart_city = ""
    Dim query As String = ""
    Dim total_Aircraft As Integer = 0
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim htmlOutput As String = ""
    Dim record_count As Integer = 0
    Dim x As Integer = 0
    Dim string_for_locations As String = ""
    Dim title As String = ""
    Dim sub_title As String = ""
    Dim page_title As String = ""
    Dim string_for_link As String = ""
    Dim string_for_city_names As String = ""
    Dim hidden_counter As Integer = 0
    Dim bgcolor As String = ""
    Dim row_count As Integer = 0

    query = " SELECT DISTINCT comp_city, comp_state, count(distinct comp_id) AS comp_count "
    query = query & " FROM aircraft_summary WITH(NOLOCK) "
    query = query & " WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH')) "
    query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

    If Trim(Country_Name) <> "" Then
      query = query & " AND comp_country='" & Country_Name & "'"
    End If

    If CLng(inAmodID) > 0 Then
      query = query & " AND amod_id = " & CStr(inAmodID)
    End If

    If Trim(airframe) <> "" Then
      query = query & " AND amod_airframe_type_code = '" & Trim(airframe) & "'"
    End If


    query = query & " GROUP BY comp_city, comp_state"
    query = query & " " & group_by_statement



    'If Session("debug") Then
    Session.Item("localUser").crmUser_DebugText += "<b>display_acContactType_location_piechart_city : " & Server.HtmlEncode(query) & "</b><br /><br />"
    '  End If
    SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
    '  End Select

    ' End Select
    SqlConn.Open()
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_acContactType_location_piechart_city(ByVal inCompanyID, ByVal inAmodID, ByVal airframe, ByVal Country_Name, ByVal group_by_statement, ByVal sub_info, ByVal make_model_name, ByVal real_company_name, ByVal sub_type)</b><br />" & query
    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    SqlCommand.CommandText = query
    localAdoRs2 = SqlCommand.ExecuteReader()


    page_title = "Charter_Cities"

    title = "CHARTER&nbsp;OPERATOR&nbsp;CITIES"

    If Trim(Country_Name) <> "" Then
      title = title & " in " & Country_Name
    End If

    sub_title = sub_title & "<table cellspacing='0' cellpadding='0' border='0' width='100%'><tr class='aircraft_list'><td valign='middle' align='left' width='60%'><strong>City</strong></td>" & vbCrLf
    sub_title = sub_title & "<td valign='middle' align='right'><strong>#&nbsp;of&nbsp;Operators&nbsp;&nbsp;</strong></td></tr>" & vbCrLf

    Try


      If localAdoRs2.HasRows Then


        Do While localAdoRs2.Read

          If Not IsDBNull(localAdoRs2("comp_city")) Then
            If Trim(localAdoRs2("comp_city").ToString.Trim) <> "" Then

              If row_count = 1 Then
                string_for_city_names = string_for_city_names & "<tr class='alt_row'>" & vbCrLf
                row_count = 0
              Else
                string_for_city_names = string_for_city_names & "<tr bgcolor='white'>" & vbCrLf
                row_count = 1
              End If

              string_for_city_names = string_for_city_names & "<td align='left'>" & string_for_link
              string_for_city_names = string_for_city_names & Replace(localAdoRs2("comp_city").ToString, "'", " ")

              If Not IsDBNull(localAdoRs2("comp_state")) Then
                If Trim(localAdoRs2("comp_state").ToString.Trim) <> "" Then
                  string_for_city_names = string_for_city_names & ", " & Replace(localAdoRs2("comp_state").ToString, "'", " ")
                End If
              End If

              string_for_city_names = string_for_city_names & "</a>"
              string_for_city_names = string_for_city_names & "</td><td align='right'>"
              string_for_city_names = string_for_city_names & FormatNumber(CStr(localAdoRs2("comp_count").ToString), 0)
              string_for_city_names = string_for_city_names & "&nbsp;&nbsp;</td></tr>"

              x = x + localAdoRs2("comp_count")
              hidden_counter = hidden_counter + 1
            End If
          End If


        Loop



      Else

      End If

      localAdoRs2.Close()

      display_acContactType_location_piechart_city = sub_title & string_for_city_names & "</table>"

    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try


  End Function
  Function displayLeasedAircraft(ByVal lease_model_id, ByVal lease_comp_id, ByVal sub_info, ByVal real_make_model_name, ByVal real_company_name, ByVal sub_type, ByVal airframe_type, ByVal temp_business_type, ByVal business_type)
    Dim type_of_product_view As String = ""
    Dim Query, tmp_title_variable, htmlOut
    Dim acCount As Integer = 0
    Dim QUOTE As String = "&quot;"
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim strBuilder
    Dim count1 As Integer = 1
    Dim hidden_counter As Integer = 1
    Dim row_count As Integer = 0
    Try
      strBuilder = New StringBuilder
      tmp_title_variable = ""
      htmlOut = ""
      displayLeasedAircraft = ""

      Query = "SELECT DISTINCT ac_ser_no_full, amod_make_name, amod_model_name, ac_id, amod_id, ac_reg_no, comp_name as lessor,comp_id, comp_country, "
      Query = Query & "(select distinct top 1 comp_name from aircraft_summary b where a.ac_id= b.ac_id and cref_contact_type='12') as lessee, cref_contact_type "
      Query = Query & "FROM Aircraft_Summary a WITH(NOLOCK) "

      Query = Query & "WHERE ac_lifecycle_stage=3 and ac_lease_flag='Y' "

      If Trim(airframe_type) <> "" Then
        Query = Query & " AND amod_airframe_type_code = '" & Trim(airframe_type) & "'"
      End If

      'If business_type.ToString.Trim <> "" Then
      '    If business_type.ToString.Trim = "B" Then
      '        Query = Query & " and ac_product_business_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "H" Then
      '        Query = Query & " and ac_product_helicopter_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "C" Then
      '        Query = Query & " and ac_product_commercial_flag = 'Y' "
      '    End If
      'End If

      Query = Query & " and cref_contact_type in ('13', '57')"

      If CLng(lease_comp_id) > 0 Then
        Query = Query & " and comp_id = " & CLng(lease_comp_id)
      End If

      If CLng(lease_model_id) > 0 Then
        Query = Query & " and amod_id=" & CLng(lease_model_id)
      End If

      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)
      Query = Query & " ORDER BY ac_ser_no_full"

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>displayLeasedAircraft : " & Server.HtmlEncode(Query) & "</b><br /><br />"
      '   End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function displayLeasedAircraft(ByVal lease_model_id, ByVal lease_comp_id, ByVal sub_info, ByVal real_make_model_name, ByVal real_company_name, ByVal sub_type, ByVal airframe_type, ByVal temp_business_type, ByVal business_type)</b><br />" & Query

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG


      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query
      localAdoRs2 = SqlCommand.ExecuteReader()

      htmlOut = "<table id='leasedAircraftOuterTable' width='100%' cellspacing='0' cellpadding='0'>"

      If localAdoRs2.HasRows Then


        strBuilder.Append("<table id='leasedAircraftInnerTable' width='100%' cellpadding='0' cellspacing='0' border='0' valign='top'>")
        strBuilder.Append("<tr valign='top' class='aircraft_list'>")
        strBuilder.Append("<td valign='top'colspan='2' align='left'><strong>")



        Do While localAdoRs2.Read





          If count1 = 1 Then
            If CLng(lease_model_id) > 0 And CLng(lease_comp_id) > 0 Then
              strBuilder.Append("<a href='details.aspx?comp_id=" & localAdoRs2("comp_id") & "&source=JETNET&type=1'>" & Trim(localAdoRs2("lessor")) & "</a>: <br> " & Trim(localAdoRs2("amod_make_name")) & " " & Trim(localAdoRs2("amod_model_name")) & tmp_title_variable)
            ElseIf CLng(lease_comp_id) > 0 Then
              strBuilder.Append("<a href='details.aspx?comp_id=" & localAdoRs2("comp_id") & "&source=JETNET&type=1'>" & Trim(localAdoRs2("lessor")) & "</a>" & tmp_title_variable)
            ElseIf CLng(lease_model_id) > 0 Then
              strBuilder.Append(Trim(localAdoRs2("amod_make_name")) & " " & Trim(localAdoRs2("amod_model_name")) & tmp_title_variable)
            End If

            strBuilder.Append("</td></tr>")
            count1 = count1 + 1
          End If

          If row_count = 1 Then
            strBuilder.Append("<tr class='alt_row'  height='59'>")
            row_count = 0
          Else
            strBuilder.Append("<tr bgcolor='white'  height='59'>")
            row_count = 1
          End If


          strBuilder.Append("<td align='left' valign='top'><img src='../images/ch_red.jpg' class='bullet'/></td><td align='left' valign='top'>")
          ' strBuilder.Append("<font size='-4'>")
          strBuilder.Append("Serial# <a href='/details.aspx?ac_ID=" & localAdoRs2.Item("ac_id") & "&source=JETNET&type=3'>" & localAdoRs2("ac_ser_no_full") & "</a>, Reg# " & localAdoRs2("ac_reg_no"))
          strBuilder.Append(" " & localAdoRs2("amod_make_name") & "/" & localAdoRs2("amod_model_name") & ", ")

          strBuilder.Append("<br />")
          If Trim(localAdoRs2("cref_contact_type")) = "57" Then
            strBuilder.Append(" Sub")
          End If

          strBuilder.Append(" Leased From: ")
          strBuilder.Append("<a href='details.aspx?comp_id=" & localAdoRs2("comp_id") & "&source=JETNET&type=1'>")
          strBuilder.Append(Trim(localAdoRs2("lessor").ToString) & "</a> (" & Trim(localAdoRs2("comp_country").ToString) & ")")
          strBuilder.Append(" To: " & Trim(localAdoRs2("lessee").ToString) & "<hr /></td></tr>")

          hidden_counter = hidden_counter + 1
          acCount = acCount + 1
        Loop

        strBuilder.Append("</table>")

        localAdoRs2.Close()

        htmlOut = htmlOut & "<tr class='aircraft_list'><td align='left' valign='middle' style='padding-left:3px;'>ACTIVE LEASES&nbsp;<em>(" & FormatNumber(acCount, 0, True, False, True) & ")</em>"
        htmlOut = htmlOut & "<br>(Single Aircraft May Have Multiple Active Leases)</td></tr>"

      Else
        htmlOut = htmlOut & "<tr class='aircraft_list'><td align='left' valign='middle' style='padding-left:3px;'>ACTIVE LEASES&nbsp;<em>(" & FormatNumber(acCount, 0, True, False, True) & ")</em>"
        htmlOut = htmlOut & "<br>(Single Aircraft May Have Multiple Active Leases)</td></tr>"

        htmlOut = htmlOut & "<tr class='aircraft_list'><td align='left' valign='middle' style='padding-left:3px;'>NO LEASED AIRCRAFT&nbsp;<em>(0)</em></td></tr>"
      End If

      localAdoRs2 = Nothing


      htmlOut = htmlOut & "<tr valign='top'><td valign='top'>" & strBuilder.ToString() & "</td></tr></table>"


      strBuilder = Nothing

      displayLeasedAircraft = htmlOut
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function
  Function display_leases_due_to_expire(ByVal in_HeaderText, ByVal real_company_name, ByVal real_make_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal IsGetNextExpiring, ByVal airframe_type, ByVal business_type, ByVal months_count, ByVal sub_info, ByVal sub_type, ByVal make_name, ByVal make_item_to_pass_to_header)


    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim query, outstring, record_count
    Dim QUOTE As String = "&quot;"
    Dim VIEW_6MONTHS As Integer = 6
    Dim counter1 As Integer = 0
    Dim hidden_counter As Integer = 0
    Dim type_of_product_view As String = ""
    Dim row_count As Integer = 0
    Try
      outstring = ""
      display_leases_due_to_expire = ""
      record_count = 0

      If IsGetNextExpiring Then
        query = "SELECT top 1 journ_subject, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_id, comp_name, comp_city, comp_state, comp_id, cref_contact_type"
      Else
        query = "SELECT journ_subject, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_id, comp_name, comp_city, comp_state, comp_id, cref_contact_type"
      End If


      query = query & " FROM aircraft WITH(NOLOCK) INNER JOIN Aircraft_Lease WITH(NOLOCK) ON ac_id = aclease_ac_id AND ac_journ_id = aclease_journ_id"
      query = query & " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id"
      query = query & " INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id = cref_ac_id and ac_journ_id = cref_journ_id"
      query = query & " LEFT OUTER JOIN company WITH(NOLOCK) ON cref_comp_id = comp_id and cref_journ_id = comp_journ_id"


      query = query & " INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id "
      query = query & " WHERE ac_journ_id > 0 AND aclease_expired = 'N' AND cref_contact_type in ('13', '57')"


      If IsGetNextExpiring Then
        query = query & " AND aclease_expiration_date > '" & Month(Now) & "/01/" & Year(Now) & "'"
      Else
        query = query & " AND (aclease_expiration_date >= '" & Month(Now) & "/01/" & Year(Now) & "'"
        query = query & " AND aclease_expiration_date < '" & Month(DateAdd("m", months_count, Now)) & "/01/" & Year(DateAdd("m", months_count, Now)) & "') "
      End If

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      If CLng(lease_comp_id) > 0 Then
        query = query & " AND comp_id = " & CLng(lease_comp_id)
      End If

      If CLng(lease_model_id) > 0 Then
        query = query & " AND amod_id = " & CStr(lease_model_id)
      End If

      If Trim(airframe_type) <> "" Then
        If Trim(airframe_type) = "F" Or Trim(airframe_type) = "R" Then
          query = query & " AND amod_airframe_type_code = '" & Trim(airframe_type) & "'"
        Else
          query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
        End If
      End If

      If Trim(make_name) <> "" Then
        query = query & " and amod_make_name = '" & make_name & "' "
      End If

      'If business_type.ToString.Trim <> "" Then
      '    If business_type.ToString.Trim = "B" Then
      '        query = query & " and ac_product_business_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "H" Then
      '        query = query & " and ac_product_helicopter_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "C" Then
      '        query = query & " and ac_product_commercial_flag = 'Y' "
      '    End If
      'End If

      If IsGetNextExpiring Then
        If amod_id > 0 Then
          in_HeaderText = in_HeaderText & real_make_model_name
        ElseIf Trim(make_name) <> "" Then
          in_HeaderText = in_HeaderText & make_name.ToString.ToUpper
        Else
          in_HeaderText = in_HeaderText & "ALL MAKES/MODELS"
        End If
        in_HeaderText = in_HeaderText & "<br />No Leases Expiring in Next " & months_count & " Month(s). Next Lease To Expire Is:"
        query = query & " ORDER BY aclease_expiration_date ASC, amod_make_name, amod_model_name"
      Else
        If amod_id > 0 Then
          in_HeaderText = in_HeaderText & real_make_model_name
        ElseIf Trim(make_name) <> "" Then
          in_HeaderText = in_HeaderText & make_name.ToString.ToUpper
        Else
          in_HeaderText = in_HeaderText & "ALL MAKES/MODELS"
        End If
        in_HeaderText = in_HeaderText & "<br />From: " & Month(Now) & "/01/" & Year(Now) & " Up to: " & Month(DateAdd("m", months_count, Now)) & "/01/" & Year(DateAdd("m", months_count, Now))
        query = query & " ORDER BY aclease_expiration_date ASC, amod_make_name, amod_model_name"
      End If



      'If Session("debug") Then
      '   Session.Item("localUser").crmUser_DebugText += "<b>display_leases_due_to_expire:" & Server.HtmlEncode(query) & "</b><br /><br />"
      'End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_leases_due_to_expire(ByVal in_HeaderText, ByVal real_company_name, ByVal real_make_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal IsGetNextExpiring, ByVal airframe_type, ByVal business_type, ByVal months_count, ByVal sub_info, ByVal sub_type, ByVal make_name, ByVal make_item_to_pass_to_header)</b><br />" & query
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      localAdoRs2 = SqlCommand.ExecuteReader()


      outstring = outstring & "<tr><td valign='top' align='left'><table id='leasesDueToExpireInnerTable' width='100%' cellpadding='1' cellspacing='0'>"
      outstring = outstring & "<tr class='aircraft_list'><td valign='top' align='left' class='tabheader'><strong>" & in_HeaderText & "</strong></td><td class='border_bottom' width='20%'>&nbsp;</td></tr>"
      outstring = outstring & "<tr><td colspan='2' class='rightside'>"
      outstring = outstring & "<table id='leasesDueToExpireDataTable' width='100%' cellpadding='4' cellspacing='0'>"

      If localAdoRs2.HasRows Then
        Do While localAdoRs2.Read


          If row_count = 1 Then
            outstring = outstring & "<tr class='alt_row'  height='59'>" & vbCrLf
            row_count = 0
          Else
            outstring = outstring & "<tr bgcolor='white'  height='59'>" & vbCrLf
            row_count = 1
          End If

          outstring = outstring & "<td valign='top' align='left' width='5%' class='seperator'><img src='../images/ch_red.jpg' class='bullet'/></td>"
          outstring = outstring & "<td valign='top' align='left' class='seperator'><em>" & localAdoRs2("amod_make_name") & " " & localAdoRs2("amod_model_name") & "</em>, "
          outstring = outstring & " Serial# " & localAdoRs2("ac_ser_no_full") & ", Reg# " & localAdoRs2("ac_reg_no") & " &nbsp;-&nbsp;<b>Expires on: " & FormatDateTime(localAdoRs2("aclease_expiration_date"), 2) & "</b><br />"
          outstring = outstring & localAdoRs2("journ_subject") & "</font></td></tr>"

          counter1 = counter1 + 1
          hidden_counter = hidden_counter + 1
        Loop
      Else

        If IsGetNextExpiring Then
          outstring = outstring & "<tr><td valign='top' align='left' class='seperator'>No leases could be found that meet your search criteria</td></tr>"
        Else
          outstring = display_leases_due_to_expire("", real_company_name, real_make_model_name, lease_model_id, lease_comp_id, True, airframe_type, business_type, months_count, sub_info, sub_type, make_name, make_item_to_pass_to_header)
        End If

      End If


      If Not IsGetNextExpiring Then

        outstring = outstring & "</table></td></tr></table></td></tr></table>"

        record_count = counter1

        If record_count > 0 Then
          outstring = "<tr class='aircraft_list'><td valign='top' align='left' style='padding-left:3px;'>LEASES DUE TO EXPIRE (<em>next " & months_count & " month(s)</em> (" & CStr(record_count) & "))</td></tr>" & outstring
        Else
          outstring = "<tr class='aircraft_list'><td valign='top' align='left' style='padding-left:3px;'>NO LEASES DUE TO EXPIRE (<em>next " & months_count & " month(s)</em>)</td></tr>" & outstring
        End If

        outstring = "<table id='leasesDueToExpireOuterTable' width='100%' height='' cellpadding='0' cellspacing='0'>" & outstring
      End If

      display_leases_due_to_expire = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function
  Function display_leases_sold_by_month(ByVal in_HeaderText, ByVal table_height, ByVal real_company_name, ByVal real_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal airframe_type, ByVal business_type)

    Dim type_of_product_view As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim outstring, query, x, record_count
    Dim VIEW_6MONTHS As Integer = 24
    Dim high_count As Integer = 0
    Dim low_count As Integer = 10000
    Try
      x = 0
      record_count = 0
      outstring = ""
      display_leases_sold_by_month = ""


      query = "SELECT YEAR(journ_date) AS tyear, MONTH(journ_date) AS tmonth, count(*) AS tcount"
      query = query & " FROM Journal WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id and journ_id = ac_journ_id"
      query = query & " INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      'query = query & " INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id=cref_ac_id and ac_journ_id = cref_journ_id"

      '   query = query & " WHERE journ_subcategory_code like 'L%' and right(journ_subcategory_code,4) <> 'CORR' and right(journ_subcategory_code,2) <> 'IT'"
      query = query & "  Where journ_subcat_code_part1 like 'L%' and journ_subcat_code_part3 not in ('IT', 'RR') "

      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      '  query = query & " AND cref_contact_type in ('13', '57')"

      If Trim(airframe_type) <> "" Then
        query = query & " AND amod_airframe_type_code = '" & Trim(airframe_type) & "'"
      End If

      'If business_type.ToString.Trim <> "" Then
      '    If business_type.ToString.Trim = "B" Then
      '        query = query & " and ac_product_business_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "H" Then
      '        query = query & " and ac_product_helicopter_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "C" Then
      '        query = query & " and ac_product_commercial_flag = 'Y' "
      '    End If
      'End If

      If CLng(lease_comp_id) > 0 Then
        query = query & " AND cref_comp_id = " & CLng(lease_comp_id)
      End If

      If CLng(lease_model_id) > 0 Then
        query = query & " AND amod_id = " & CStr(lease_model_id)
      End If

      query = query & " AND (journ_date >= '" & Month(DateAdd("m", (-1) * CInt(VIEW_6MONTHS), Now)) & "/01/" & Year(DateAdd("m", (-1) * CInt(VIEW_6MONTHS), Now)) & "')"
      query = query & " AND (journ_date < '" & Month(Now) & "/01/" & Year(Now) & "')"



      query = query & " GROUP BY YEAR(journ_date), month(journ_date) ORDER BY YEAR(journ_date), month(journ_date)"

      'If Session("debug") Then
      Session.Item("localUser").crmUser_DebugText += "<b>display_leases_sold_by_month:" & Server.HtmlEncode(query) & "</b><br /><br />"
      '  End If

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_leases_sold_by_month(ByVal in_HeaderText, ByVal table_height, ByVal real_company_name, ByVal real_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal airframe_type, ByVal business_type)</b><br />" & query
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      localAdoRs2 = SqlCommand.ExecuteReader()

      If localAdoRs2.HasRows Then



        Me.AVG_SOLD_PER_MONTH.Series.Add("AVG").ChartType = UI.DataVisualization.Charting.SeriesChartType.SplineArea
        Me.AVG_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Title = "Leasses"
        Me.AVG_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisX.Title = "Month"
        Me.AVG_SOLD_PER_MONTH.Series("AVG").Color = Drawing.Color.Blue
        Me.AVG_SOLD_PER_MONTH.Series("AVG").BorderWidth = 1
        Me.AVG_SOLD_PER_MONTH.Series("AVG").MarkerSize = 5
        Me.AVG_SOLD_PER_MONTH.Series("AVG").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle

        Me.AVG_SOLD_PER_MONTH.Series("AVG").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
        Me.AVG_SOLD_PER_MONTH.Width = 220
        Me.AVG_SOLD_PER_MONTH.Height = 220

        Do While localAdoRs2.Read

          If localAdoRs2("tcount") > 0 Then

            Me.AVG_SOLD_PER_MONTH.Series("AVG").Points.AddXY((localAdoRs2("tmonth") & "-" & localAdoRs2("tYear")), localAdoRs2("tcount"))

            If localAdoRs2("tcount") > high_count Then
              high_count = localAdoRs2("tcount")
            End If

            If localAdoRs2("tcount") < low_count Then
              low_count = localAdoRs2("tcount")
            End If


            x = x + 1
          End If

        Loop
        localAdoRs2.Close()
        localAdoRs2 = Nothing

      Else
        outstring = ""
      End If



      If high_count > 0 Then
        outstring = outstring & "Leases Per Month Last 24 Months"
      End If


      If high_count > 0 Then

        If high_count > 50 Then
          high_count = 100
        ElseIf high_count > 20 And high_count < 50 Then
          high_count = 50
        ElseIf high_count < 20 And high_count > 10 Then
          high_count = 20
        Else
          high_count = 10
        End If


        Me.AVG_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = high_count 'high_number + interval_point
        Me.AVG_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0
        Me.AVG_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = (high_count / 10) 'interval_point

      End If

      display_leases_sold_by_month = Trim(outstring)

    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function display_leases_expired(ByVal in_HeaderText, ByVal real_company_name, ByVal real_make_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal IsGetLastExpired, ByVal airframe_type, ByVal sub_info, ByVal airframe_limitations_for_header, ByVal business_type, ByVal months_amount, ByVal make_name, ByVal make_item_to_pass_to_header)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim query, outstring, record_count
    Dim QUOTE As String = "&quot;"
    Dim VIEW_6MONTHS As Integer = 6
    Dim counter1 As Integer = 0
    Dim hidden_counter As Integer = 1
    Dim type_of_product_view As String = ""
    Dim row_count As Integer = 0
    outstring = ""
    display_leases_expired = ""
    record_count = 0
    in_HeaderText = ""
    Try

      If IsGetLastExpired Then
        query = "SELECT Top 1 amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id"
      Else
        query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id"
      End If


      query = query & " FROM aircraft WITH(NOLOCK) INNER JOIN Aircraft_Lease WITH(NOLOCK) ON ac_id = aclease_ac_id and ac_journ_id = aclease_journ_id"
      query = query & " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id"
      query = query & " INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id=cref_ac_id and ac_journ_id = cref_journ_id"
      query = query & " LEFT OUTER JOIN company WITH(NOLOCK) ON cref_comp_id = comp_id and cref_journ_id = comp_journ_id"


      query = query & " INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id "
      query = query & " WHERE ac_journ_id > 0 AND aclease_expired = 'Y' AND cref_contact_type in ('13', '57')"
      query = query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      If IsGetLastExpired Then
        query = query & " AND (aclease_expiration_date < '" & Month(Now) & "/01/" & Year(Now) & "')"
      Else
        query = query & " AND (aclease_expiration_date >= '" & Month(DateAdd("m", (-1) * months_amount, Now)) & "/01/" & Year(DateAdd("m", (-1) * months_amount, Now)) & "')"
        query = query & " AND (aclease_expiration_date < '" & Month(Now) & "/01/" & Year(Now) & "')"
      End If

      If CLng(lease_comp_id) > 0 Then
        query = query & " AND comp_id = " & CLng(lease_comp_id)
      End If

      If CLng(lease_model_id) > 0 Then
        query = query & " AND amod_id = " & CStr(lease_model_id)
      End If

      If Trim(airframe_type) <> "" Then
        If Trim(airframe_type) = "F" Or Trim(airframe_type) = "R" Then
          query = query & " AND amod_airframe_type_code = '" & Trim(airframe_type) & "'"
        Else
          query = query & " AND amod_type_code = '" & Trim(airframe_type) & "'"
        End If
      End If

      If Trim(make_name) <> "" Then
        query = query & " and amod_make_name = '" & make_name & "' "
      End If

      'If business_type.ToString.Trim <> "" Then
      '    If business_type.ToString.Trim = "B" Then
      '        query = query & " and ac_product_business_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "H" Then
      '        query = query & " and ac_product_helicopter_flag = 'Y' "
      '    ElseIf business_type.ToString.Trim = "C" Then
      '        query = query & " and ac_product_commercial_flag = 'Y' "
      '    End If
      'End If

      If IsGetLastExpired Then
        in_HeaderText = "No Leases Expired In Last " & months_amount & " Months. Last Lease To Expire Was:"
        query = query & " ORDER BY aclease_expiration_date desc, amod_make_name, amod_model_name "
      Else
        If amod_id > 0 Then
          in_HeaderText = in_HeaderText & real_make_model_name
        ElseIf Trim(make_name) <> "" Then
          in_HeaderText = in_HeaderText & make_name.ToString.ToUpper
        Else
          in_HeaderText = in_HeaderText & "ALL MAKES/MODELS"
        End If
        in_HeaderText = in_HeaderText & "<br />From: " & Month(DateAdd("m", (-1) * months_amount, Now)) & "/01/" & Year(DateAdd("m", (-1) * months_amount, Now)) & " Up to: " & Month(Now) & "/01/" & Year(Now)
        query = query & " ORDER BY aclease_expiration_date ASC, ac_ser_no_full, amod_make_name, amod_model_name "
      End If




      'If Session("debug") Then
      '   Session.Item("localUser").crmUser_DebugText += "<b>display_leases_expired:" & Server.HtmlEncode(query) & "</b><br /><br />"
      '   End If
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function display_leases_expired(ByVal in_HeaderText, ByVal real_company_name, ByVal real_make_model_name, ByVal lease_model_id, ByVal lease_comp_id, ByVal IsGetLastExpired, ByVal airframe_type, ByVal sub_info, ByVal airframe_limitations_for_header, ByVal business_type, ByVal months_amount, ByVal make_name, ByVal make_item_to_pass_to_header)</b><br />" & query

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query
      localAdoRs2 = SqlCommand.ExecuteReader()


      outstring = outstring & "<tr><td valign='top' align='left'><table id='displayExpiredLeasesInnerTable' width='100%' cellpadding='1' cellspacing='0'>"
      outstring = outstring & "<tr class='aircraft_list'><td valign='top' align='left' class='tabheader'><strong>" & in_HeaderText & "</strong></td><td class='border_bottom' width='20%'>&nbsp;</td></tr>"
      outstring = outstring & "<tr><td class='rightside' colspan='2'>"
      outstring = outstring & "<table id='displayExpiredLeasesDataTable' width='100%' cellpadding='4' cellspacing='0'>"

      If localAdoRs2.HasRows Then



        Do While localAdoRs2.Read


          If row_count = 1 Then
            outstring = outstring & "<tr class='alt_row'  height='59'>" & vbCrLf
            row_count = 0
          Else
            outstring = outstring & "<tr bgcolor='white'  height='59'>" & vbCrLf
            row_count = 1
          End If


          If IsGetLastExpired Then
            outstring = outstring & "<td valign='top' align='left' width='5%' class='seperator'><img src='../images/ch_red.jpg' class='bullet'/></td>"
            outstring = outstring & "<td valign='top' align='left' class='seperator'><em>" & localAdoRs2("amod_make_name") & " " & localAdoRs2("amod_model_name") & "</em>, "
            outstring = outstring & " Serial# " & localAdoRs2("ac_ser_no_full") & ", Reg# " & localAdoRs2("ac_reg_no") & " &nbsp;-&nbsp;<b>Expired on: " & FormatDateTime(localAdoRs2("aclease_expiration_date"), 2) & "</b>"
            outstring = outstring & "<br />"
            outstring = outstring & localAdoRs2("journ_subject") & "</font></td></tr>"
          Else
            outstring = outstring & "<td valign='top' align='left' width='5%' class='seperator'><img src='../images/ch_red.jpg' class='bullet'/></td>"
            outstring = outstring & "<td valign='top' align='left' class='seperator'><em>" & localAdoRs2("amod_make_name") & " " & localAdoRs2("amod_model_name") & "</em>, "
            outstring = outstring & " Serial# " & localAdoRs2("ac_ser_no_full") & ", Reg# " & localAdoRs2("ac_reg_no") & " &nbsp;-&nbsp;<b>Expired on: " & FormatDateTime(localAdoRs2("aclease_expiration_date"), 2) & "</b>"
            outstring = outstring & "<br />"
            outstring = outstring & localAdoRs2("journ_subject") & "</font></td></tr>"

            hidden_counter = hidden_counter + 1
            counter1 = counter1 + 1
          End If


        Loop



      Else

        If IsGetLastExpired Then
          outstring = outstring & "<tr><td valign='top' align='left' class='seperator'>No leases could be found that meet your search criteria</td></tr>"
        Else
          outstring = display_leases_expired("", real_company_name, make_item_to_pass_to_header, lease_model_id, lease_comp_id, True, airframe_type, sub_info, airframe_limitations_for_header, business_type, months_amount, "", make_item_to_pass_to_header)
        End If



      End If

      If Not IsGetLastExpired Then
        outstring = outstring & "</table></td></tr></table></td></tr></table>"

        record_count = counter1


        If record_count > 0 Then
          outstring = "<tr class='aircraft_list'><td valign='top' align='left' colspan='2'  style='padding-left:3px;'>LEASES EXPIRED (<em>last " & months_amount & " months</em> (" & CStr(record_count) & "))</td></tr>" & outstring
        Else
          outstring = "<tr class='aircraft_list'><td valign='top' align='left' colspan='2' style='padding-left:3px;'>NO LEASES HAVE EXPIRED (<em>last " & months_amount & " months</em>)</td></tr>" & outstring
        End If

        outstring = "<table id='displayExpiredLeasesOuterTable' width='100%' height='' cellpadding='0' cellspacing='0'>" & outstring
      End If

      display_leases_expired = Trim(outstring)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------
  '-------------------------------- BLOCK OF FUNCTIONS FOR MODEL MARKET SUMMARY - LOWER SECTION------------------------------



  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------
  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------
  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------

  Public Function TranslateUSMetricUnitsShort(ByVal in_StrToTranslate)

    Select Case (UCase(in_StrToTranslate))

      Case "FT"
        TranslateUSMetricUnitsShort = "m"
      Case "NM"
        TranslateUSMetricUnitsShort = "km"
      Case "M"
        TranslateUSMetricUnitsShort = "km"
      Case "SM"
        TranslateUSMetricUnitsShort = "km"
      Case "KN"
        TranslateUSMetricUnitsShort = "kph"
      Case "FPM"
        TranslateUSMetricUnitsShort = "mps"
      Case "PSI"
        TranslateUSMetricUnitsShort = "torr"
      Case "LBS"
        TranslateUSMetricUnitsShort = "kg"
      Case "GAL"
        TranslateUSMetricUnitsShort = "l"
      Case "HP"
        TranslateUSMetricUnitsShort = "mhp"
      Case Else
        TranslateUSMetricUnitsShort = UCase(in_StrToTranslate)

    End Select

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

  Public Function Get_Fuel_Price() As Double
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim sQuery As String = ""
    Dim tempprice As Double = 0.0


    Try

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60


      sQuery = "SELECT evo_config_fuel_cost FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'LIVE'"
      SqlCommand.CommandText = sQuery
      adoRs = SqlCommand.ExecuteReader()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Get_Fuel_Price() as Double</b><br />" & sQuery
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
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetEngines(ByVal inModelID, ByVal nMAXEngines) As String</b><br />" & Query
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
      'SqlConn.Close()
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
      Dim number_of_engine_types As Integer = 0

      Query = "SELECT ameng_engine_name, ameng_seq_no"
      Query = Query & " FROM Aircraft_Model_Engine WITH(NOLOCK) WHERE ameng_amod_id = " & CStr(inModelID)
      Query = Query & " GROUP BY ameng_seq_no, ameng_engine_name ORDER BY ameng_seq_no, ameng_engine_name"

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = Query

      adoRs = SqlCommand.ExecuteReader()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetEnginesNumberForSpace(ByVal inModelID, ByVal nMAXEngines) As Integer</b><br />" & Query
      If (adoRs.HasRows) Then
        Do While adoRs.Read
          number_of_engine_types = number_of_engine_types + 1
        Loop
        adoRs.Close()
      End If

      adoRs = Nothing
      ' SqlConn.Close()

      GetEnginesNumberForSpace = number_of_engine_types
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try

  End Function
  Public Function ConvertUSToForeignCurrency(ByVal in_ExchangeRate, ByVal in_valToConvert)
    ConvertUSToForeignCurrency = CDbl(CDbl(in_ExchangeRate) * CDbl(in_valToConvert))
  End Function
  Public Function GetForeignExchangeRate(ByVal in_CurrencyID, ByRef out_CurrencyName, ByRef out_CurrencyDate) As Double

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim Query As String : Query = ""
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

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
      'SqlConn.Close()
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
    Return GetForeignExchangeRate
  End Function
  Function GetFlightActivity(ByVal inModelID) As String
    Dim strBuilder
    strBuilder = New StringBuilder
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim Query As String : Query = ""
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60




      Query = "SELECT DISTINCT count(*) AS tflights, (sum(aractivity_distance)/count(*)) AS avgdistance, sum(aractivity_distance) AS tdistance,"
      Query = Query & " (sum(aractivity_flight_time)/count(*)) AS avgflighttime, sum(aractivity_flight_time) AS tflighttime"
      Query = Query & " FROM ARGUS_Activity_Data WITH(NOLOCK) INNER JOIN aircraft WITH(NOLOCK) ON (aractivity_reg_no = ac_reg_no_search) AND ac_journ_id = 0"
      Query = Query & " INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_amod_id = " & CStr(inModelID) & " AND aractivity_date_depart >= (getdate()-90)"


      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True)

      'If Session("debug") Then
      '    Session.Item("localUser").crmUser_DebugText += "<b>getFlightActivity : " & Server.HtmlEncode(Query) & "</b><br /><br />"
      '   End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetFlightActivity(ByVal inModelID) As String</b><br />" & Query

      SqlCommand.CommandText = Query
      adoRs = SqlCommand.ExecuteReader()
      strBuilder.Append("<table id='flightActivityTable' width='100%' cellspacing='0' cellpadding='4'>")

      If adoRs.HasRows Then
        adoRs.Read()
        strBuilder.Append("<tr class='aircraft_list'><td valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></td><td width='40%' class='border_bottom'>&nbsp;</td></tr>")

        strBuilder.Append("<tr><td width='50%'><table valign='top' width='100%'>")
        If Not IsDBNull(adoRs("tflights")) Then
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Number of Flights</td><td valign='top' align='left' class='rightside'>" & FormatNumber(adoRs("tflights"), 0) & "</td></tr>")
        Else
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Number of Flights</td><td valign='top' align='left' class='rightside'>" & FormatNumber(0, 0) & "</td></tr>")
        End If

        If Not IsDBNull(adoRs("avgdistance")) Then
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Average Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(adoRs("avgdistance"), 0) & " <em>(nm)</em></td></tr>")
        Else
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Average Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(0, 0) & " <em>(nm)</em></td></tr>")
        End If

        If Not IsDBNull(adoRs("tdistance")) Then
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Total Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(adoRs("tdistance"), 0) & " <em>(nm)</em></td></tr>")
        Else
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Total Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(0, 0) & " <em>(nm)</em></td></tr>")
        End If

        If Not IsDBNull(adoRs("avgflighttime")) Then
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Average Flight Time</td><td valign='top' align='left' class='rightside'>" & FormatNumber(adoRs("avgflighttime"), 1) & " <em>(min)</em></td></tr>")
        Else
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Average Flight Time</td><td valign='top' align='left' class='rightside'>" & FormatNumber(0, 1) & " <em>(min)</em></td></tr>")
        End If

        If Not IsDBNull(adoRs("tflighttime")) Then
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Total Flight Time</td><td valign='top' align='left' class='rightside'>" & FormatNumber(adoRs("tflighttime"), 1) & " <em>(min)</em></td></tr>")
        Else
          strBuilder.Append("<tr><td valign='middle' align='left' class='seperator'>Total Flight Time</td><td valign='top' align='left' class='rightside'>" & FormatNumber(0, 1) & " <em>(min)</em></td></tr>")
        End If

        strBuilder.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2' width='100%'><b>Powered by ARGUS/TRAQPak</b></td></tr>")

        adoRs.Close()
      Else
        strBuilder.Append("<tr><td valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></td><td width='40%' class='border_bottom'>&nbsp;</td></tr>")
        strBuilder.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2'>No Flight Activity at this time, for this Make/Model ...</td></tr>")
      End If

      strBuilder.Append("</table></td></tr>")

      strBuilder.Append("</table>")

      adoRs = Nothing

      GetFlightActivity = strBuilder.ToString()

      strBuilder = Nothing

    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try


  End Function
  Function ReturnYearQuarterName_PDF(ByVal strYear, ByVal strQuarter)

    Dim strResults

    strResults = ""

    Select Case strQuarter
      Case "1"
        strResults = "1st Quarter (Jan-Feb-Mar), " & strYear
      Case "2"
        strResults = "2nd Quarter (Apr-May-Jun), " & strYear
      Case "3"
        strResults = "3rd Quarter (Jul-Aug-Sep), " & strYear
      Case "4"
        strResults = "4th Quarter (Oct-Nov-Dec), " & strYear
    End Select

    ReturnYearQuarterName_PDF = strResults

  End Function ' ReturnYearQuarterName
  Function SortLabelsValue(ByRef labels, ByRef data, ByVal count, ByVal direction) ' 1 direction is label is used for sort 2 is that data is 
    SortLabelsValue = ""
    Dim i As Integer = 0
    Dim j As Integer = 0
    Dim temp_string As String = ""
    Dim temp_int As Integer = 0
    Dim temp_int_array(count) As Double


    For i = 0 To count
      If direction = 1 Then
        temp_int_array(i) = CDbl(labels(i))
      Else
        temp_int_array(i) = data(i)
      End If
    Next


    For j = 0 To count - 1
      For i = 0 To count - 1
        If temp_int_array(i) > temp_int_array(i + 1) Then

          temp_int = data(i + 1)
          data(i + 1) = data(i)
          data(i) = temp_int

          temp_string = labels(i + 1)
          labels(i + 1) = labels(i)
          labels(i) = temp_string

          temp_int = temp_int_array(i + 1)
          temp_int_array(i + 1) = temp_int_array(i)
          temp_int_array(i) = temp_int

        End If
      Next
    Next

    For j = 0 To count - 1
      If labels(j) = Nothing Then
        labels(j) = ""
      End If
    Next


  End Function
  Function get_company_cert_images(ByVal comp_id, ByVal IsCompanySelected)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs2 As System.Data.SqlClient.SqlDataReader : localAdoRs2 = Nothing
    Dim counter1
    Dim query2 As String = ""
    counter1 = 1
    get_company_cert_images = ""
    Try
      query2 = "select  ccerttype_id, ccerttype_type, ccerttype_logo_image from company_certification "
      query2 = query2 & " inner join company_certification_type on ccert_type_id = ccerttype_id"
      query2 = query2 & " where ccert_journ_id = 0 and ccert_comp_id = " & comp_id
      query2 = query2 & " and ccerttype_logo_image <> '' "
      '  query2 = query2 & " and ccerttype_type in ('IS-BOA', 'AOC', 'ARG/US Gold', 'ARG/US Platnum') "



      ' Select Case Application.Item("webHostObject").evoWebHostType
      '   Case eWebSiteTypes.LOCAL
      '  SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LOCAL_MSSQL
      '  Case Else
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG
      '  End Select

      ' End Select
      SqlConn.Open()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function get_company_cert_images(ByVal comp_id, ByVal IsCompanySelected)</b><br />" & query2
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = query2
      localAdoRs2 = SqlCommand.ExecuteReader()


      If IsCompanySelected Then

        get_company_cert_images = get_company_cert_images & "<table id='acContactTypeCompaniesOuterTable' width='100%' height=' & table_height & ' cellpadding='0' cellspacing='0'>" & vbCrLf
        get_company_cert_images = get_company_cert_images & "<tr><td valign='top' align='left' class='header'>CHARTER&nbsp;OPERATOR&nbsp;CERTIFICATES</em></td></tr>" & vbCrLf
        get_company_cert_images = get_company_cert_images & "<tr><td valign='top' align='left'>"
        get_company_cert_images = get_company_cert_images & "<table id='acContactTypeCountiesInnerTable' width='100%' cellpadding='0' cellspacing='0'><tr><Td>" & vbCrLf

        If localAdoRs2.HasRows Then

          Do While localAdoRs2.Read
            get_company_cert_images = get_company_cert_images & "<img width='21' src='../images/" & localAdoRs2("ccerttype_logo_image") & "' alt='" & localAdoRs2("ccerttype_type") & "'>"


          Loop

        Else
          get_company_cert_images = get_company_cert_images & "No Certificate Information Available"
        End If

        get_company_cert_images = get_company_cert_images & "</td></tr></table></td></tr></table>"

      Else



        If localAdoRs2.HasRows Then

          get_company_cert_images = get_company_cert_images & "<table><tr>"


          Do While localAdoRs2.Read
            If counter1 > 2 Then
              get_company_cert_images = get_company_cert_images & "</tr><tr><td>"
              counter1 = 1
            Else
              get_company_cert_images = get_company_cert_images & "<td>"
            End If

            get_company_cert_images = get_company_cert_images & "<img width='40' src='../images/" & localAdoRs2("ccerttype_logo_image") & "' alt='" & localAdoRs2("ccerttype_type") & "'>"

            'get_company_cert_images = get_company_cert_images & Cstr(adors("ccerttype_id").ToString)

            get_company_cert_images = get_company_cert_images & "</td>"
            counter1 = counter1 + 1
          Loop

          get_company_cert_images = get_company_cert_images & "</table>"

        Else

        End If


      End If


      localAdoRs2.Close()
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Function GetAircraftAirframeLabelServer(ByVal inAirframeType, ByVal inAircraftMakeType, ByVal bSingleAirframeSelected)


    GetAircraftAirframeLabelServer = "Business Jet"


  End Function
  Function Get_Model_Info(ByVal this_model)
    Get_Model_Info = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim rs As System.Data.SqlClient.SqlDataReader : rs = Nothing
    Dim Query As String = ""
    Dim fAmod_start_year As String = ""
    Dim fAmod_end_year As String = ""
    Dim fAmod_ser_no_prefix As String = ""
    Dim fAmod_ser_no_start As String = ""
    Dim fAmod_ser_no_end As String = ""
    Dim fAmod_ser_no_suffix As String = ""
    Dim fAmod_start_price As Integer = 0
    Dim fAmod_end_price As Integer = 0
    Try

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "SELECT amod_make_name, amod_model_name, amod_manufacturer, amod_airframe_type_code, amod_weight_class, amod_start_year, amod_end_year, amod_ser_no_prefix, amod_ser_no_start, "
      Query = Query & " amod_ser_no_end, amod_ser_no_suffix, amod_start_price, amod_end_price, amod_description, amod_type_code, atype_name "
      Query = Query & " FROM Aircraft_Model WITH(NOLOCK) "
      Query = Query & " INNER JOIN Aircraft_Type on amod_type_code=atype_code"
      Query = Query & " WHERE amod_id = " & this_model
      Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Get_Model_Info(ByVal this_model)</b><br />" & Query
      SqlCommand.CommandText = Query
      rs = SqlCommand.ExecuteReader()
      If rs.HasRows Then
        rs.Read()




        If Not IsDBNull(rs("amod_make_name")) Then
          make_name = rs("amod_make_name")
        End If

        If Not IsDBNull(rs("amod_model_name")) Then
          model_name = Trim(rs("amod_model_name"))
          make_model_name = Trim(make_name) & " " & model_name

        End If

        If Not IsDBNull(rs("amod_manufacturer")) Then
          Amod_manufacturer = Trim(rs("amod_manufacturer"))
        End If

        If Not IsDBNull(rs("amod_type_code")) Then
          type_code = UCase(Trim(rs("amod_type_code")))
        End If

        If Not IsDBNull(rs("amod_airframe_type_code")) Then
          airframe_type = UCase(Trim(rs("amod_airframe_type_code")))
          ' airframe_type_num = UCase(Trim(rs("amod_airframe_type_code")))
        End If

        If Not IsDBNull(rs("amod_weight_class")) Then
          weight_class = UCase(Trim(rs("amod_weight_class")))
        End If

        Select Case weight_class
          Case "V"
            weight_class_name = "Very Light Jet"
          Case "L"
            weight_class_name = "Light"
          Case "M"
            weight_class_name = "Medium"
          Case "H"
            weight_class_name = "Heavy"
        End Select


        If Not IsDBNull(rs("amod_start_year")) Then
          fAmod_start_year = Trim(rs("amod_start_year"))
        Else
          fAmod_start_year = ""
        End If

        If Not IsDBNull(rs("amod_end_year")) Then
          fAmod_end_year = Trim(rs("amod_end_year"))
        Else
          fAmod_end_year = ""
        End If

        start_end_years = fAmod_start_year

        If fAmod_end_year <> "" Then
          start_end_years += " - " & fAmod_end_year & "&nbsp;"
        ElseIf fAmod_start_year <> "" Then
          start_end_years += " - Present&nbsp;"
        Else
          start_end_years += "&nbsp;"
        End If



        If Not IsDBNull(rs("amod_ser_no_prefix")) Then
          fAmod_ser_no_prefix = Trim(rs("amod_ser_no_prefix"))
        Else
          fAmod_ser_no_prefix = ""
        End If

        If Not IsDBNull(rs("amod_ser_no_start")) Then
          fAmod_ser_no_start = Trim(rs("amod_ser_no_start"))
        Else
          fAmod_ser_no_start = ""
        End If

        If Not IsDBNull(rs("amod_ser_no_end")) Then
          fAmod_ser_no_end = Trim(rs("amod_ser_no_end"))
        Else
          fAmod_ser_no_end = ""
        End If

        If Not IsDBNull(rs("amod_ser_no_suffix")) Then
          fAmod_ser_no_suffix = Trim(rs("amod_ser_no_suffix"))
        Else
          fAmod_ser_no_suffix = ""
        End If


        ser_nbr_range = fAmod_ser_no_prefix & fAmod_ser_no_start & fAmod_ser_no_suffix

        If fAmod_ser_no_end <> "" Then
          ser_nbr_range += " - " & fAmod_ser_no_prefix & fAmod_ser_no_end & fAmod_ser_no_suffix & "&nbsp;"
        ElseIf fAmod_ser_no_start <> "" Then
          ser_nbr_range += " &amp; Up&nbsp;"
        Else
          ser_nbr_range += "&nbsp;"
        End If



        If Not IsDBNull(rs("atype_name")) Then
          amod_type_name = Trim(rs("atype_name"))
        Else
          amod_type_name = ""
        End If


        amod_price_range = ""

        If Not IsDBNull(rs("amod_start_price")) Then
          fAmod_start_price = Trim(rs("amod_start_price"))
        Else
          fAmod_start_price = 0
        End If

        If Not IsDBNull(rs("amod_end_price")) Then
          fAmod_end_price = Trim(rs("amod_end_price"))
        Else
          fAmod_end_price = 0
        End If

        If fAmod_start_price <> 0 Then
          amod_price_range += "$" & FormatNumber(fAmod_start_price, 0, False, False, True)
        Else
          amod_price_range += "&nbsp;"
        End If

        If fAmod_end_price <> 0 Then
          amod_price_range += " - $ " & FormatNumber(fAmod_end_price, 0, False, False, True) & "&nbsp;"
        Else
          amod_price_range += "&nbsp;"
        End If












        If Not IsDBNull(rs("amod_description")) Then
          Amod_description = Trim(rs("amod_description"))
        End If

      End If

      rs.Close()
      rs = Nothing
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  '    Public Shared Function clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(ByRef crmSubScriptionCls As crmSubscriptionClass, ByVal Is_Operator_Flag As Boolean, ByVal Is_Aircraft_Flag As Boolean)
  '        '------------------------------------------
  '        ' Function: clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM
  '        '
  '        ' This function takes in the local subscription class. 
  '        'If then names that crm subscription class and references it that way
  '        '
  '        '
  '        '
  '        ' This function take in two flags. One for Operator and one for Aircraft.
  '        ' Currently there is no way to do both a model/aircraft selection and an operator selection.
  '        '
  '        ' If Operator flag is true, then it runs the operator section query
  '        ' If Operator Flag is flase, it runs the model selection. It then checks the aircraft flag.
  '        ' If the aircraft flag is true, and the operator flag is false it will then run model and aircraft.
  '        '
  '        '  clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False) - This would run Just Model Selection Code 
  '        '  clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True) -  This would run The Model Selection and the Aircraft Selection Code
  '        '  clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), True, False) -  This would run Just the Operator Selection Code 
  '        '
  '        '
  '        '------------------------------------------
  '        Dim sSelectionClause As String = ""
  '        Dim nloop As Integer = 0
  '        Dim bSingleProduct As Boolean = True
  '        Dim string_for_type As String = ""




  '        If Is_Operator_Flag = False Then ' if it is not an operator, then run the model

  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------

  '            sSelectionClause &= "AND amod_customer_flag = 'Y' "

  '            sSelectionClause &= cAndClause

  '            sSelectionClause &= cSingleOpen

  '            If crmSubScriptionCls.crmBusiness_Flag = True Then
  '                sSelectionClause &= "( amod_product_business_flag = 'Y'"

  '                If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
  '                ElseIf crmSubScriptionCls.crmJets_Flag = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
  '                ElseIf crmSubScriptionCls.crmTurboprops = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
  '                Else
  '                    sSelectionClause &= ")"
  '                End If
  '            End If

  '            If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
  '                sSelectionClause &= cOrClause
  '            End If

  '            If crmSubScriptionCls.crmCommercial_Flag = True Then
  '                sSelectionClause &= "( amod_product_commercial_flag = 'Y'"


  '                If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
  '                ElseIf crmSubScriptionCls.crmJets_Flag = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
  '                ElseIf crmSubScriptionCls.crmTurboprops = True Then
  '                    sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
  '                Else
  '                    sSelectionClause &= ")"
  '                End If
  '            End If


  '            If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
  '                sSelectionClause &= cOrClause
  '            End If


  '            If crmSubScriptionCls.crmHelicopter_Flag = True Then
  '                sSelectionClause &= "(amod_type_code IN ('T','P') and amod_product_helicopter_flag = 'Y')"
  '            End If




  '            sSelectionClause &= cSingleClose

  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------




  '            If Is_Aircraft_Flag = True Then ' if it is aircraf, then run aircraft 

  '                '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
  '                '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
  '                sSelectionClause &= cAndClause

  '                sSelectionClause &= cSingleOpen

  '                If crmSubScriptionCls.crmBusiness_Flag = True Then
  '                    sSelectionClause &= " ac_product_business_flag = 'Y' "
  '                End If

  '                If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
  '                    sSelectionClause &= cOrClause
  '                End If

  '                If crmSubScriptionCls.crmCommercial_Flag = True Then
  '                    sSelectionClause &= " ac_product_commercial_flag = 'Y' "
  '                End If

  '                If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
  '                    sSelectionClause &= cOrClause
  '                End If

  '                If crmSubScriptionCls.crmHelicopter_Flag = True Then
  '                    sSelectionClause &= " ac_product_helicopter_flag = 'Y'"
  '                End If

  '                sSelectionClause &= cSingleClose

  '                '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
  '                '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
  '            End If

  '        ElseIf Is_Operator_Flag = True Then  ' if operator is true, then run it
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
  '            sSelectionClause &= "AND comp_active_flag = 'Y' "

  '            sSelectionClause &= cAndClause

  '            sSelectionClause &= cSingleOpen

  '            If crmSubScriptionCls.crmBusiness_Flag = True Then
  '                sSelectionClause &= " comp_product_business_flag = 'Y' "
  '            End If

  '            If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
  '                sSelectionClause &= cOrClause
  '            End If

  '            If crmSubScriptionCls.crmCommercial_Flag = True Then
  '                sSelectionClause &= " comp_product_commercial_flag = 'Y' "
  '            End If

  '            If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
  '                sSelectionClause &= cOrClause
  '            End If

  '            If crmSubScriptionCls.crmHelicopter_Flag = True Then
  '                sSelectionClause &= " comp_product_helicopter_flag = 'Y'"
  '            End If

  '            sSelectionClause &= cSingleClose
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
  '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
  '        End If





  '        Return sSelectionClause.Trim

  '    End Function


  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------
  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------
  '--------CONVERSION FUNCTIONS----------------CONVERSION FUNCTIONS---------CONVERSION FUNCTIONS------------
  Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
    Master.default_models_check_changed(main_pnl)
    model_cbo.Items.Remove(model_cbo.Items.FindByValue("All"))

  End Sub
End Class