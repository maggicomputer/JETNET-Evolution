Partial Public Class print_spec
  Inherits System.Web.UI.Page
  Public error_string As String = ""
  Public aclsData_Temp As New clsData_Manager_SQL
  Public report_name As String ' Used for defining name of pdf report
  Public make_model_name As String = ""
  Public ac_id As Integer
  Public TYPE_OF_AC As String = ""
  Public Jetnet As Boolean = True
  Public Client As Boolean = False
  Public Client_DB As String = ""
  Public Ref_DB As String = ""
  Private localDatalayer As viewsDataLayer
  Private searchCriteria As New viewSelectionCriteriaClass
  Dim COMPLETED_OR_OPEN As String = "O"
  Dim LAST_SAVE_DATE As String = ""
  Dim current_model_id As Long = 0
  Dim current_make_model As String = ""
  Dim current_ac_name As String = ""

  Private Sub print_spec_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Client_DB = CStr(Application.Item("crmClientDatabase"))
    Ref_DB = CStr(Application.Item("crmJetnetDatabase"))
  End Sub


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '-------------------------------------------Database Connections--------------------------------------------------------------
    Dim tempTable As New DataTable
    Dim NOTE_ID As Long = 0

    aclsData_Temp = New clsData_Manager_SQL

    If Session.Item("crmUserLogon") <> True Or Ref_DB = "" Then
      'error_string = "print_spec.aspx.vb - Page Init() - " & Request.ServerVariables("SCRIPT_NAME").ToString() & " - Session Timeout"
      'clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      'aclsData_Temp.Insert_CRM_Event("Session", Application.Item("crmClientSiteData").crmClientHostName, error_string)
      Response.Redirect("Default.aspx", False)
    End If
    ' setup the connection info
    'Test 
    aclsData_Temp.client_DB = Application.Item("crmClientDatabase")
    aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
    aclsData_Temp.class_error = ""


    If Not IsNothing(Trim(Request("ac_ID"))) Then
      If Trim(Request("ac_id")) <> "" Then
        ac_id = Trim(Request("ac_ID"))
      Else
        ac_id = 0 ' default chall 300
      End If ' If Trim(Request("amod_id")) <> "" Then
    Else
      ac_id = 0 ' default chall 300
    End If

    If Session("show_cost_values") = "" Then
      Session("show_cost_values") = "Yes"
    End If
    Session("SubID") = "777"
    If Not IsNothing(Request.Item("sub_id")) Then
      If Not String.IsNullOrEmpty(Request.Item("sub_id").ToString) Then
        Session.Item("SubID") = Request.Item("sub_id").Trim
      End If
    End If
    If Not IsNothing(Request.Item("type")) Then
      If Not String.IsNullOrEmpty(Request.Item("type").ToString) Then
        If Request.Item("type").ToString = "JETNET" Then
          Jetnet = True
          Client = False
        Else
          Jetnet = False
          Client = True
        End If
      End If
    End If
    Session("UserID") = "jetnet"
    If Not IsNothing(Request.Item("UserID")) Then
      If Not String.IsNullOrEmpty(Request.Item("UserID").ToString) Then
        Session.Item("UserID") = Request.Item("UserID").Trim
      End If
    End If
    ' THIS SECTION CURRENTLY IS NOT USED
    Session.Item("SeqNo") = "1"
    If Not IsNothing(Request.Item("SeqNo")) Then
      If Not String.IsNullOrEmpty(Request.Item("SeqNo").ToString) Then
        Session.Item("SeqNo") = Request.Item("SeqNo").Trim
      End If
    End If

    Dim cur_date_string As String
    cur_date_string = Replace(Now.Date, "/", "_") & "_"
    cur_date_string = cur_date_string & Replace(Now.Hour, "/", "_") & "_"
    cur_date_string = cur_date_string & Replace(Now.Minute, "/", "_") & "_"
    cur_date_string = cur_date_string & Replace(Now.Second, "/", "_")

    report_name = ""
    report_name = report_name & Session("SubID") & "_" & Session("UserID") & "_" & Session.Item("SeqNo") & "_" & cur_date_string & "_PDF_ModelMarketSummary.doc"


    If Trim(Request("note_id")) <> "" Then
      NOTE_ID = Trim(Request("note_id"))
    Else
      If Trim(Request("noteID")) <> "" Then
        NOTE_ID = Trim(Request("noteID"))
      End If
    End If


    'This is a request variable that gets sent whenever you're closing a valuation and 
    'it needs to automatically create a PDF for the valuation.
    If Not String.IsNullOrEmpty(Trim(Request("fromClose"))) Then
      If Trim(Request("fromClose")) = "true" Then
        'default selection to PDF.
        WD.SelectedValue = "PDF"
        'Now we run this button click automatically
        btnRunReport_Click(btnRunReport, System.EventArgs.Empty)
      End If
    End If




    If NOTE_ID > 0 Then
      Me.SP.Visible = False
      Me.TP.Visible = False
      Me.NP.Visible = False
      Me.PR.Visible = False
      Me.prospect_type.Visible = False
      Me.PP.Visible = False
      Me.BR.Visible = False
      Me.sales_format.Visible = False

      Me.MTR.Visible = True
      Me.SP.Visible = True
      Me.CP.Visible = True
      Me.WD.Visible = True
      Me.CMC.Visible = True
      Me.SC.Visible = True
      Me.MSV.Visible = True
      Me.MSA.Visible = True
      Me.RS.Visible = True
      Me.MVA.Visible = True
      Me.mva_label.Visible = True
      Me.mva_months.Visible = True
      Me.chkPP_Large.Visible = False
      Me.pic_break.Visible = False
    Else
      Me.SP.Visible = True
      Me.TP.Visible = True
      Me.NP.Visible = True
      Me.PR.Visible = True
      Me.prospect_type.Visible = True

      Me.pic_break.Visible = False
      Me.PP.Visible = True
      Me.BR.Visible = True
      Me.sales_format.Visible = True
      Me.break_point_label.Visible = True

      Me.MTR.Visible = False
      Me.CP.Visible = False
      Me.CMC.Visible = False
      Me.SC.Visible = False
      Me.MSV.Visible = False
      Me.MSA.Visible = False
      Me.RS.Visible = False
      Me.MVA.Visible = False
      Me.mva_label.Visible = False
      Me.mva_months.Visible = False

      If Client = True Then
        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim Client_Query As String : Client_Query = ""

        Client_Query = "Select cliamod_make_name as amod_make_name, cliamod_model_name as amod_model_name, cliaircraft_ser_nbr as ac_ser_nbr from client_aircraft inner join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id"
        Client_Query = Client_Query & " WHERE cliaircraft_id = " & ac_id

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query



        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


        Try
          tempTable.Load(MySqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
        End Try

        MySqlReader.Close()
        MySqlConn.Close()
        '-----------------------------------
      ElseIf Jetnet = True Then
        Dim Jetnet_Query As String : Jetnet_Query = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing


        Jetnet_Query = "SELECT amod_make_name, amod_model_name, ac_ser_no as ac_ser_nbr FROM Aircraft with (NOLOCK) INNER JOIN Aircraft_Model with (NOLOCK) ON ac_amod_id = amod_id"
        Jetnet_Query = Jetnet_Query & " WHERE ac_id = " & ac_id & " and ac_journ_id = 0"

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          tempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
        End Try

        SqlReader.Close()
        SqlConn.Close()
      End If
      If tempTable.Rows.Count > 0 Then
        'End If
        TYPE_OF_AC = tempTable.Rows(0).Item("amod_make_name") & " " & tempTable.Rows(0).Item("amod_model_name") & " Ser#" & tempTable.Rows(0).Item("ac_ser_nbr")
        ac_mod_name_ser.Text = TYPE_OF_AC
      End If


    End If

  End Sub

  Private Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
    ' crate a string to hold the PDF output info
    Dim ViewToPDF As String = ""
    Dim NOTE_ID As Long = 0
    Dim internal As String = ""
    Dim retail As String = ""
    Dim report_name As String = ""
    Dim temp_header_text As String = ""
    Dim spec_bottom_text As String = ""
    Dim sHtmlMarketTrends As String = ""
    Dim comp_info As String = ""
    Dim temp_string As String = ""
    Dim rep_id As Long = 0
    Dim sales_within_years As String = ""
    Dim timeframe As String = ""
    Dim sales_within_Aftt As String = ""
    Dim use_only_used As String = ""
    Dim use_jetnet_data As String = ""
    Dim current_aftt As String = ""
    Dim current_year As String = ""
    Dim extra_sold_criteria As String = ""
    Dim extra_client_sold_criteria As String = ""
    Dim YearDateVariable As String = ""
    Dim internal_flag As String = ""
    Dim retail_flag As String = ""
    Dim temp_Asking_chart As String = ""
    Dim amod_id_no_ac_id As Long = 0



    If Trim(Request("internal")) <> "" Then
      internal = Trim(Request("internal"))
    End If

    If Trim(Request("retail")) <> "" Then
      retail = Trim(Request("retail"))
    End If


    If Trim(Request("amod_id")) <> "" Then
      amod_id_no_ac_id = Trim(Request("amod_id"))
    End If


    If Trim(Request("note_id")) <> "" Then
      NOTE_ID = Trim(Request("note_id"))
    Else
      If Trim(Request("noteID")) <> "" Then
        NOTE_ID = Trim(Request("noteID"))
      End If
    End If




    If Not IsNothing(Trim(Request("ac_ID"))) Then
      If Trim(Request("ac_id")) <> "" Then
        ac_id = Trim(Request("ac_ID"))
      Else
        ac_id = 0 ' default chall 300
      End If ' If Trim(Request("amod_id")) <> "" Then
    Else
      ac_id = 0 ' default chall 300
    End If



    Dim location_string As String = HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim()

    'If InStr(location_string, "jetnetcrmtest") > 0 Then
    '  location_string = "www.jetnetevolution.com"
    'End If

    ' Dim create footer variables
    '--------------------------------- header variables
    ' Dim page_total As Integer

    HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = Application.Item("crmClientDatabase")

    localDatalayer = New viewsDataLayer
    localDatalayer.clientConnectStr = Session.Item("localPreferences").UserDatabaseConn
    localDatalayer.starConnectStr = Session.Item("localPreferences").STARDatabaseConn
    localDatalayer.serverConnectStr = Session.Item("localPreferences").ServerNotesDatabaseConn


    timeframe = Trim(Request("timeframe"))
    If Trim(timeframe) <> "" Then
      searchCriteria.ViewCriteriaTimeSpan = timeframe
    End If


    If NOTE_ID > 0 Then


      report_name = Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID & "_COMPARISON_VIEW_PDF.html"

      ' run this ahead of time to get the client ac id correct 

      localDatalayer = New viewsDataLayer
      localDatalayer.adminConnectStr = Application.Item("crmClientSiteData").AdminDatabaseConn
      localDatalayer.clientConnectStr = Session.Item("localPreferences").UserDatabaseConn
      localDatalayer.starConnectStr = Session.Item("localPreferences").STARDatabaseConn
      localDatalayer.serverConnectStr = Session.Item("localPreferences").ServerNotesDatabaseConn


      temp_string = crmViewDataLayer.get_valuation_details(NOTE_ID, localDatalayer, Session("CLIENT_AC_ID"), Session("JETNET_AC_ID"), LAST_SAVE_DATE, COMPLETED_OR_OPEN, True, spec_bottom_text, amod_id_no_ac_id)

      ViewToPDF = ViewToPDF & Build_PDF_Template_Header()


      temp_header_text = build_full_spec_page_header(Session("JETNET_AC_ID"), "Market Value Analysis", "", "", comp_info)



      ViewToPDF = ViewToPDF & "<table width='100%'>"
      ViewToPDF = ViewToPDF & "<tr><Td>"
      ViewToPDF = ViewToPDF & temp_string
      ViewToPDF = ViewToPDF & "</td></tr>"
      ViewToPDF = ViewToPDF & get_ac_image(Session("JETNET_AC_ID"), 0)
      ViewToPDF = ViewToPDF & "<tr><Td>"
      ViewToPDF = ViewToPDF & Replace(spec_bottom_text, "Company/Customer", "PREPARED FOR")
      ViewToPDF = ViewToPDF & "</td></tr>"
      ViewToPDF = ViewToPDF & "<tr><td align='center' width='100%'><center><font class='header_text'><b>PREPARED BY: </b></font></center></td></tr><tr><Td class='small_header_text' align='center'><font class='small_header_text'><table align='center'>" & comp_info & "</table></font></td></tr>"
      ViewToPDF = ViewToPDF & "</table>"

      ' - -- try at bring it in ---------------
      Call localDatalayer.run_view_19_actions(NOTE_ID, 0, "", "", searchCriteria.ViewCriteriaAmodID, "", "", Session.Item("CLIENT_AC_ID"), "", COMPLETED_OR_OPEN, searchCriteria, "", "", Me.tabcontainer1, "", Me.Page.Title, Me.LAST_SAVE_DATE, Session.Item("JETNET_AC_ID"), "", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "", "")


      If amod_id_no_ac_id > 0 Then
      Else
        If Me.SP.Checked Then
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          ' ---- this is from other spec, not the cover page for this one --- 
          ViewToPDF = ViewToPDF & Build_PDF_CoverPage(ac_id, temp_header_text)
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          ViewToPDF = ViewToPDF & Build_PDF_Second_Page(ac_id, temp_header_text)
          ' ---- this is from other spec, not the cover page for this one ---
        End If


        If Me.MVA.Checked = True Then
          '----------------MARKET VALUE ANALYSIS-------------------------------------------------------
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          If Me.WD.SelectedValue = "Word" Then
            ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, True, 1, False, "", "", Me.mva_months.SelectedValue)
          Else
            ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, False, 1, False, "", "", Me.mva_months.SelectedValue)
          End If
          '----------------MARKET VALUE ANALYSIS-------------------------------------------------------
        End If
      End If


      '----------------MARKET SURVEY------------------------------------------------------
      'crmViewDataLayer.Combined_views_display_fleet_market_summary(searchCriteria, Build_FleetMarketSummary_text, chart_htmlString, localDatalayer, "", True, False, 0, 0, 0)
      rep_id = 0
      Call aclsData_Temp.Fill_Open_Box(Nothing, Session.Item("localUser").crmLocalUserID, 3, rep_id, searchCriteria.ViewCriteriaAmodID, "3", "", True)

      If Trim(Request("extra")) = "" Then
        crmViewDataLayer.Build_For_sale_tab(searchCriteria, Me.use_this_label.Text, NOTE_ID, Session("ForSale_File_EXCEL"), 19, True, False, Session.Item("localUser").crmAllowExport_Flag, "", localDatalayer, LAST_SAVE_DATE, "", Session.Item("CLIENT_AC_ID"), False, 0, "", True, rep_id, aclsData_Temp)
      Else
        crmViewDataLayer.Build_For_sale_tab(searchCriteria, Me.use_this_label.Text, NOTE_ID, Session("ForSale_File_EXCEL"), 19, True, Trim(Request("extra")), Session.Item("localUser").crmAllowExport_Flag, "", localDatalayer, LAST_SAVE_DATE, "", Session.Item("CLIENT_AC_ID"), False, 0, "", True, rep_id, aclsData_Temp)
      End If

      ' - -- try at bring it in --------------- 
      If Me.WD.SelectedValue = "Word" Then
        ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, True, 5)
      Else
        ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, False, 5)
      End If

      If Me.use_this_label.Text <> "" Then
        temp_string = Me.use_this_label.Text
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Market Survey")
        ViewToPDF = ViewToPDF & temp_string
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
      End If
      '----------------MARKET SURVEY------------------------------------------------------



      If amod_id_no_ac_id > 0 Then
      Else
        '----------------CURRENT MARKET-------------------------------------------------------
        rep_id = 0
        Me.tabcontainer1.ActiveTabIndex = 1
        Call aclsData_Temp.Fill_Open_Box(Nothing, Session.Item("localUser").crmLocalUserID, 3, rep_id, searchCriteria.ViewCriteriaAmodID, "3", 0, True)
        Call localDatalayer.Build_Compare_View(searchCriteria.ViewCriteriaAmodID, searchCriteria.ViewCriteriaAircraftMake, searchCriteria.ViewCriteriaAircraftModel, aclsData_Temp, rep_id, NOTE_ID, Me.tabcontainer1, Me.tabcontainer1, searchCriteria, Session.Item("CLIENT_AC_ID"), "", use_this_label, Me.dummy_label, "", "", Session.Item("CLIENT_AC_ID"), "", Me.dummy_label.Text, Me.LAST_SAVE_DATE, False, "", Session("JETNET_AC_ID"), "", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "", "")

        'If Me.use_this_label.Text <> "" Then
        '  temp_string = Me.use_this_label.Text

        '  ViewToPDF = ViewToPDF & Insert_Page_Break()
        ' ViewToPDF = ViewToPDF & Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Market Comparables")
        '  ViewToPDF = ViewToPDF & temp_string
        '  ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        'End If

        ' current market comparables
        If Me.WD.SelectedValue = "Word" Then
          ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, True, 2)
        Else
          ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, False, 2)
        End If
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        '----------------CURRENT MARKET------------------------------------------------------
      End If


      '----------------MARKET STATUS------------------------------------------------------
      If MSA.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, False, False, MSA.Checked, False, False, False, temp_header_text, False, 4, False, "", "", 0, amod_id_no_ac_id)
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
      End If
      '----------------MARKET STATUS------------------------------------------------------





      '----------------MARKET TRENDS------------------------------------------------------
      If MTR.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Market Trends")
        Call Build_Market_trends_tab(sHtmlMarketTrends, location_string)
        ViewToPDF = ViewToPDF & sHtmlMarketTrends
        ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"

        Call make_value_graphs_just_chart(DateAdd(DateInterval.Year, -1, Date.Now()), searchCriteria.ViewCriteriaAmodID, temp_Asking_chart)

        If Trim(temp_Asking_chart) <> "" Then
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          ViewToPDF = ViewToPDF & Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Market Trends")
          ViewToPDF = ViewToPDF & temp_Asking_chart
          ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        End If


      End If
      '----------------MARKET TRENDS------------------------------------------------------


      '----------------RECENT SALES----------------------------------------------------- 
      sales_within_years = Trim(Request("sales_within_years"))
      sales_within_Aftt = Trim(Request("sales_within_Aftt"))
      use_only_used = Trim(Request("use_only_used"))
      use_jetnet_data = Trim(Request("use_jetnet_data"))
      current_aftt = Trim(Request("current_aftt"))
      current_year = Trim(Request("current_year"))
      If Trim(current_year) = "" Then
        current_year = Year(Now())
      End If

      internal_flag = Trim(Request("internal_flag"))
      retail_flag = Trim(Request("retail_flag"))

      extra_sold_criteria = ""
      extra_client_sold_criteria = ""


      If Trim(timeframe) <> "" And Trim(timeframe) <> "0" Then
        YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(timeframe), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(timeframe), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(timeframe), Now()))

        extra_sold_criteria &= " AND journ_date >= '" & YearDateVariable & "' "
        extra_client_sold_criteria &= " AND clitrans_date >= '" & YearDateVariable & "'"
      End If


      If Trim(sales_within_years) <> "" And Trim(sales_within_years) <> "0" Then
        extra_sold_criteria &= " and (ac_mfr_year >= " & (current_year - sales_within_years) & " and ac_mfr_year <= " & (current_year + sales_within_years) & " ) "
        extra_client_sold_criteria &= " and (clitrans_year_mfr >= " & (current_year - sales_within_years) & " and clitrans_year_mfr <= " & (current_year + sales_within_years) & " ) "
      End If

      If Trim(sales_within_Aftt) <> "" And Trim(sales_within_Aftt) <> "0" Then
        extra_sold_criteria &= " and (ac_airframe_tot_hrs >= " & (current_aftt - sales_within_Aftt) & " and ac_airframe_tot_hrs <= " & (current_aftt + sales_within_Aftt) & " ) "
        extra_client_sold_criteria &= " and (clitrans_airframe_total_hours >= " & (current_aftt - sales_within_Aftt) & " and clitrans_airframe_total_hours <= " & (current_aftt + sales_within_Aftt) & " ) "
      End If

      If Trim(use_only_used) = "Y" Then
        extra_sold_criteria &= " and journ_newac_flag = 'N' "
        extra_client_sold_criteria &= " and clitrans_newac_flag = 'N' "
      End If

      If Trim(use_jetnet_data) <> "" Then
        extra_sold_criteria &= " and journ_newac_flag = 'N' "
        extra_client_sold_criteria &= " and clitrans_newac_flag = 'N' "
      End If

      If Trim(internal_flag) = "N" Then
        extra_sold_criteria &= " AND  journ_internal_trans_flag = 'N' "
        extra_client_sold_criteria &= " AND  clitrans_internal_trans_flag = 'N' "
      End If

      If Trim(retail_flag) = "Y" Then
        extra_sold_criteria &= " AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS')) "
        '  extra_client_sold_criteria &= " AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS')) "
      End If


      If Me.WD.SelectedValue = "Word" Then
        ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, False, False, False, False, False, temp_header_text, True, 6, True, extra_sold_criteria, extra_client_sold_criteria, 0, amod_id_no_ac_id)
      Else
        ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, False, False, False, False, False, temp_header_text, False, 6, True, extra_sold_criteria, extra_client_sold_criteria, amod_id_no_ac_id)
      End If
      '----------------RECENT SALES-----------------------------------------------------




      If amod_id_no_ac_id > 0 Then
      Else
        '----------------SOLD COMPARABLES------------------------------------------------------
        rep_id = 0
        Me.tabcontainer1.ActiveTabIndex = 2
        Call aclsData_Temp.Fill_Open_Box(Nothing, Session.Item("localUser").crmLocalUserID, 8, rep_id, searchCriteria.ViewCriteriaAmodID, "8", 0, True)
        Call localDatalayer.Build_Compare_View(searchCriteria.ViewCriteriaAmodID, searchCriteria.ViewCriteriaAircraftMake, searchCriteria.ViewCriteriaAircraftModel, aclsData_Temp, rep_id, NOTE_ID, Me.tabcontainer1, Me.tabcontainer1, searchCriteria, Session.Item("CLIENT_AC_ID"), "", Me.dummy_label, use_this_label, "", "", Session.Item("CLIENT_AC_ID"), "", Me.dummy_label.Text, Me.LAST_SAVE_DATE, False, "", Session("JETNET_AC_ID"), "", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        'If Me.use_this_label.Text <> "" Then
        '  temp_string = Me.use_this_label.Text

        '  ViewToPDF = ViewToPDF & Insert_Page_Break()
        '  ViewToPDF = ViewToPDF & Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Sold Comparables")
        '  ViewToPDF = ViewToPDF & temp_string
        '  ViewToPDF = ViewToPDF & "</td></tr></table></td></tr></table>"
        'End If

        If Me.WD.SelectedValue = "Word" Then
          ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, True, 3)
        Else
          ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(ac_id, NOTE_ID, False, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, temp_header_text, False, 3)
        End If
        '----------------SOLD COMPARABLES------------------------------------------------------
      End If













    Else

      report_name = Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_PRINT_SPEC.DOC"

      ' page_total = 0
      ViewToPDF = Build_PDF_Template_Header()
      ViewToPDF = ViewToPDF & Build_PDF_CoverPage(ac_id, "")

      If Me.SP.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Second_Page(ac_id, "")
      End If

      If Not BR.Checked Then
        If Me.TP.Checked Then
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          ViewToPDF = ViewToPDF & Build_PDF_Third_Page(ac_id)
        End If
      End If

      If NP.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Fourth_Page(ac_id)
      End If

      If PR.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Fifth_Page(ac_id)
      End If

      If PP.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Pictures_Page(ac_id)
      End If


      If PH.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Sixth_Page(ac_id)
      End If

    End If

    If NOTE_ID = 0 Then
      Response.AppendHeader("Content-Type", "application/msword")

      Response.AppendHeader("Content-disposition", "attachment; filename=spec.doc")
    End If

    ' call the Build HTML Page
    ViewToPDF = Build_HTML_Page(ViewToPDF)



    If NOTE_ID > 0 Then

      If Trim(Request("pic")) = "Y" Then


        ViewToPDF = Build_PDF_Template_Header()
        ViewToPDF &= "<img src='http://www.jetnetcrmtest.com/TempFiles/66_ANALYTICS_HISTORY.jpg'>"
        ViewToPDF &= "<table><tr><td>1</td></tr><table>"
        ViewToPDF &= Insert_Page_Break()
        ViewToPDF &= "<img src='http://www.jetnetevolution.com/TempFiles/27_FOR_SALE_6_MONTHS.jpg'>"
        ViewToPDF &= "<table><tr><td>6---</td></tr><table>"
        ViewToPDF &= Insert_Page_Break()
        ViewToPDF &= "<img src='" & Server.MapPath("TempFiles") & "/27_FOR_SALE_6_MONTHS.jpg'>"
        ViewToPDF &= "<table><tr><td>8--</td></tr><table>"




        report_name = "test123.html"

        If Not Build_String_To_HTML(report_name, ViewToPDF) Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "There was a problem generating your report"
        Else
          If Not WD.SelectedValue = "Word" Then
            convert_to_pdf(report_name)
            report_name = Replace(report_name, "html", "pdf")
            report_name = Session.Item("MarketSummaryFolderVirtualPath").ToString & "/" & report_name
          End If
        End If

        form1.Visible = False
        Response.Redirect(report_name)





      Else
        If WD.SelectedValue = "Word" Then
          Response.AppendHeader("Content-Type", "application/msword")

          Response.AppendHeader("Content-disposition", "attachment; filename=spec.doc")
          form1.Visible = False
          Response.Write(ViewToPDF)
        Else

          If Not Build_String_To_HTML(report_name, ViewToPDF) Then
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "There was a problem generating your report"
          Else
            If Not WD.SelectedValue = "Word" Then
              convert_to_pdf(report_name)
              report_name = Replace(report_name, "html", "pdf")
              report_name = Session.Item("MarketSummaryFolderVirtualPath").ToString & "/" & report_name
            End If
          End If



          'We're going to save this somewhere special if we pass the fromClose variable as true.
          'This basically means you're on a valuation and closing it and now a pdf must be made.
          If Not String.IsNullOrEmpty(Trim(Request("fromClose"))) Then
            If Trim(Request("fromClose")) = "true" Then
              'Since the pdf is made, we're going ahead and closing this form. We don't need any redirection.
              If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                System.IO.File.Move(Server.MapPath(report_name), Server.MapPath("\Documents\") & NOTE_ID & "_COMPARISON_VIEW_PDF.pdf")
              Else
                System.IO.File.Move(Server.MapPath(report_name), "C:\inetpub\vhosts\jetnetcrm.com\private\documents\" & Replace(LCase(Application.Item("crmClientSiteData").crmClientHostName()), "www.", "") & "\" & NOTE_ID & "_COMPARISON_VIEW_PDF.pdf")
              End If

              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='view_template.aspx?ViewID=19&noteID=" & NOTE_ID & "&noMaster=false';", True)
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

            Else
              form1.Visible = False
              Response.Redirect(report_name)
            End If
          Else
            form1.Visible = False
            Response.Redirect(report_name)
          End If


        End If
      End If

    Else
      form1.Visible = False
      Response.Write(ViewToPDF)
    End If



    ' call the Output String to HTML file



    ' call the Output String to HTML file





  End Sub

  Public Function Build_String_To_HTML(ByVal report_name As String, ByVal ViewToPDF As String) As Boolean
    Build_String_To_HTML = False
    Try
      Build_String_To_HTML = True
      ' create a file to dump the PDF report to
      ' create a streamwriter variable
      Dim swPDF As System.IO.StreamWriter
      ' create the html file

      'Temp Hold MSW


      swPDF = IO.File.CreateText(HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + report_name)
      ' write to the file
      swPDF.WriteLine(ViewToPDF)
      'close the streamwriter
      swPDF.Close()
      ' call the webgrabber info
      Response.Write("Page:<br>" & ViewToPDF)




    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_String_To_HTML: " & ex.Message
    End Try


  End Function

  Private Function convert_to_pdf(ByVal report_name As String) As Boolean

    Dim reportFolder As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString)
    Dim bReturnValue As Boolean = False
    Dim varURL As String = ""
    Dim varTimeout As Integer = 0

    Dim htmlToPdfConverter As New EvoPdf.HtmlToPdfConverter()

    Try

      varURL = reportFolder + "\" + report_name
      varTimeout = 60

      ' Set license key received after purchase to use the converter in licensed mode
      ' Leave it not set to use the converter in demo mode
      ' htmlToPdfConverter.LicenseKey = "" '"4W9+bn19bn5ue2B+bn1/YH98YHd3d3c="
      htmlToPdfConverter.LicenseKey = "9Xtoem9qemp6bXRqemlrdGtodGNjY2N6ag=="

      ' Set HTML Viewer width in pixels which is the equivalent in converter of the browser window width
      htmlToPdfConverter.HtmlViewerWidth = 1024

      ' Set HTML viewer height in pixels to convert the top part of a HTML page 
      ' Leave it not set to convert the entire HTML
      htmlToPdfConverter.HtmlViewerHeight = 0

      ' Set PDF page size which can be a predefined size like A4 or a custom size in points 
      ' Leave it not set to have a default A4 PDF page
      htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = EvoPdf.PdfPageSize.A4

      ' Set PDF page orientation to Portrait or Landscape
      ' Leave it not set to have a default Portrait orientation for PDF page
      htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = EvoPdf.PdfPageOrientation.Portrait

      ' Set the maximum time in seconds to wait for HTML page to be loaded 
      ' Leave it not set for a default 60 seconds maximum wait time
      htmlToPdfConverter.NavigationTimeout = varTimeout

      ' Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
      ' Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
      htmlToPdfConverter.ConversionDelay = 0

      htmlToPdfConverter.ConvertUrlToFile(varURL, reportFolder + "\" + commonEvo.GenerateFileName(report_name, ".pdf", True))

      bReturnValue = True

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in convert_to_pdf: " + ex.Message

    Finally

      ' Clear Objects
      htmlToPdfConverter = Nothing

    End Try

    Return bReturnValue

  End Function

  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '' Function name: Build_PDF_CoverPage
  '' Purpose: to build the cover page for the pdf file
  '' Parameters: none
  '' Return: 
  ''       String - in html table row format
  '' Change Log
  ''           05/27/2010    - Created By: Tom Jones
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Build_PDF_CoverPage(ByVal ac_id As Integer, ByVal temp_header_Text As String) As String
    Dim airfram_tot_time As String = ""
    Dim cycles As String = ""
    Dim asking_price As String = ""
    Dim company_name As String = ""
    Dim temp_moyear As String = ""
    Dim temp_ex_moyear As String = ""
    Dim last_updated As String = ""
    Dim exclusive_flag As String = ""
    Dim list_date As String = ""
    Dim tmp_list_date As String = ""
    Dim asking_type As String = ""
    Dim ex_date As String = ""
    Dim times_of_date As String = ""
    Dim ac_maintained As String = ""
    Dim ac_status As String = ""
    Dim prev_owned As String = ""
    Dim ex_done_by_and_rating As String = ""
    Dim in_done_by_and_rating As String = ""
    Dim address_info As String = ""
    Build_PDF_CoverPage = ""
    Dim phone_info_test As String = ""
    Dim confidential As String = ""
    Dim tempTable As New DataTable
    Dim temptable2 As New DataTable
    Dim MySqlConn2 As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft2 As MySql.Data.MySqlClient.MySqlDataReader
    Dim MySqlException2 As MySql.Data.MySqlClient.MySqlException : MySqlException2 = Nothing
    Dim Client_Query As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim SqlConn2 As New SqlClient.SqlConnection
    Dim SqlCommand2 As New SqlClient.SqlCommand
    Dim SqlReader2 As SqlClient.SqlDataReader : SqlReader2 = Nothing
    Dim SqlException2 As SqlClient.SqlException : SqlException2 = Nothing
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim Jetnet_Query As String = ""


    Try

      SqlConn.ConnectionString = Ref_DB

      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      Dim htmlOutput As String = ""

      If Trim(temp_header_Text) <> "" Then
        Build_PDF_CoverPage = Trim(Replace(temp_header_Text, "Market Value Analysis", "Value Analysis - Aircraft Info (1)"))
      Else
        Build_PDF_CoverPage = Build_PDF_Header("Aircraft Information (Page 1 of 2)", address_info)
      End If



      htmlOutput = htmlOutput & "<tr><td height='5'></td></tr>"

      If Client = True Then


        Dim client_fields As String = ""
        client_fields = "cliaircraft_confidential_notes as ac_confidential_notes, cliamod_make_name as amod_make_name,"
        client_fields = client_fields & "cliamod_model_name as amod_model_name, cliaircraft_year_mfr as ac_year_mfr, "
        client_fields = client_fields & "cliaircraft_year_dlv as ac_year_dlv, cliaircraft_ser_nbr as ac_ser_nbr, cliaircraft_reg_nbr "
        client_fields = client_fields & "as ac_reg_nbr, cliaircraft_airframe_total_hours as ac_airframe_total_hours, "
        client_fields = client_fields & " cliaircraft_date_purchased as ac_date_purchased, cliaircraft_airframe_total_landings as "
        client_fields = client_fields & " ac_airframe_total_landings, cliaircraft_asking_wordage as ac_asking_wordage, "
        client_fields = client_fields & " cliaircraft_asking_price as ac_asking_price, cliaircraft_status as ac_status, "
        client_fields = client_fields & " cliaircraft_interior_doneby_name as ac_interior_doneby_name, cliaircraft_interior_rating "
        client_fields = client_fields & "as ac_interior_rating, cliaircraft_passenger_count as ac_passenger_count, cliaircraft_interior_month_year "
        client_fields = client_fields & " as ac_interior_month_year, cliaircraft_exclusive_flag as ac_exclusive_flag, cliaircraft_date_listed "
        client_fields = client_fields & " as ac_date_listed, cliaircraft_exterior_doneby_name as ac_exterior_doneby_name, cliaircraft_exterior_rating as ac_exterior_rating, "
        client_fields = client_fields & " cliaircraft_exterior_month_year as ac_exterior_month_year, cliaircraft_forsale_flag as ac_forsale_flag"

        Client_Query = "Select " & client_fields & " from client_aircraft inner join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id"
        Client_Query = Client_Query & " WHERE cliaircraft_id = " & ac_id

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        Try


          MySqlConn.Open()
          MySqlCommand.Connection = MySqlConn
          MySqlCommand.CommandType = CommandType.Text
          MySqlCommand.CommandTimeout = 60

          adoRSAircraft2 = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

          Try
            tempTable.Load(adoRSAircraft2)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
          End Try

          adoRSAircraft2.Close()

        Catch ex As Exception
        Finally
          adoRSAircraft2 = Nothing
          MySqlConn.Close()
          MySqlConn = Nothing
        End Try



      ElseIf Jetnet = True Then


        Try
          Jetnet_Query = "SELECT aircraft.ac_confidential_notes, aircraft_model.amod_make_name, aircraft_model.amod_model_name, "
          Jetnet_Query = Jetnet_Query & "aircraft.ac_mfr_year as ac_year_mfr, aircraft.ac_delivery as ac_year_dlv, aircraft.ac_exclusive_flag "
          Jetnet_Query = Jetnet_Query & ", aircraft.ac_ser_no as ac_ser_nbr, aircraft.ac_reg_no as ac_reg_nbr, aircraft.ac_airframe_tot_hrs as ac_airframe_total_hours, "
          Jetnet_Query = Jetnet_Query & " aircraft.ac_airframe_tot_landings as ac_airframe_total_landings, aircraft.ac_purchase_date as ac_date_purchased "
          Jetnet_Query = Jetnet_Query & " , aircraft.ac_asking as ac_asking_wordage, aircraft.ac_asking_price, aircraft.ac_status, aircraft.ac_interior_doneby_name "
          Jetnet_Query = Jetnet_Query & " , aircraft.ac_interior_rating, aircraft.ac_passenger_count, ac_interior_moyear as ac_interior_month_year "
          Jetnet_Query = Jetnet_Query & " , aircraft.ac_list_date as ac_date_listed, aircraft.ac_exterior_rating, ac_exterior_moyear as ac_exterior_month_year "
          Jetnet_Query = Jetnet_Query & ", aircraft.ac_exterior_doneby_name, aircraft.ac_forsale_flag "
          Jetnet_Query = Jetnet_Query & " FROM Aircraft with (NOLOCK) INNER JOIN Aircraft_Model with (NOLOCK) ON ac_amod_id = amod_id"
          Jetnet_Query = Jetnet_Query & " WHERE ac_id = " & ac_id & " and ac_journ_id = 0"


          SqlCommand.CommandText = Jetnet_Query
          SqlReader = SqlCommand.ExecuteReader()
          SqlCommand.CommandType = CommandType.Text
          SqlCommand.CommandTimeout = 60

          Try
            tempTable.Load(SqlReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
          End Try

          SqlReader.Close()

        Catch ex As Exception
        Finally

        End Try




      End If



      If tempTable.Rows.Count > 0 Then

        htmlOutput = htmlOutput & "<tr><td colspan='2' class='header_text'>Aircraft Identification</b></font></td></tr>"
        ' start the Aircraft Identification Status
        ''''''''''''''''''''''''''''''''''''''''''''
        If Not IsDBNull(tempTable.Rows(0).Item("ac_confidential_notes")) Then
          If Not String.IsNullOrEmpty(tempTable.Rows(0).Item("ac_confidential_notes")) Then
            confidential = confidential & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Note: &nbsp;</font>"
            confidential = confidential & "<font class='text_text'>" & tempTable.Rows(0).Item("ac_confidential_notes").ToString & "&nbsp;</td></tr>"
            If confidential.Trim <> "" Then
              confidential = confidential
            End If
          End If
        End If
        ' make
        If Not IsDBNull(tempTable.Rows(0).Item("amod_make_name")) Then
          htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Model: </font><font class='text_text'> "
          '<b><i>Make: </i></b> 
          htmlOutput = htmlOutput & tempTable.Rows(0).Item("amod_make_name").ToString & "&nbsp;"
        End If
        ' model
        If Not IsDBNull(tempTable.Rows(0).Item("amod_model_name")) Then
          '<b><i>Model: </i></b>
          htmlOutput = htmlOutput & tempTable.Rows(0).Item("amod_model_name") & "</font></td></tr>"
        End If
        ' year of manufacture
        If Not IsDBNull(tempTable.Rows(0).Item("ac_year_mfr")) Then
          htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Year of Manufacture: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_year_mfr").ToString & "</font>"
        End If

        If Not IsDBNull(tempTable.Rows(0).Item("ac_year_dlv")) Then
          htmlOutput = htmlOutput & "<font class='small_header_text'>, Delivery: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_year_dlv").ToString & "</font></td></tr>"
        Else
          htmlOutput = htmlOutput & "</td></tr>"
        End If

        If Not BR.Checked Then
          ' serial number
          If Not IsDBNull(tempTable.Rows(0).Item("ac_ser_nbr")) Then
            htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Serial #: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_ser_nbr").ToString & "</font></td></tr>"
          End If
          ' reg number
          If Not IsDBNull(tempTable.Rows(0).Item("ac_reg_nbr")) Then
            htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Registration #: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_reg_nbr").ToString & ""
            htmlOutput = htmlOutput & "</font></td></tr>"
          End If
          ' airframe total time
          If Not IsDBNull(tempTable.Rows(0).Item("ac_date_purchased")) Then
            htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Purchase Date: </font><font class='text_text'>" & FormatDateTime(tempTable.Rows(0).Item("ac_date_purchased"), vbShortDate) & "</font>"
          End If
        End If

        If Not IsDBNull(tempTable.Rows(0).Item("ac_airframe_total_hours")) Then
          airfram_tot_time = "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Airframe Total Time(AFTT): </font><font class='text_text'>" & FormatNumber(tempTable.Rows(0).Item("ac_airframe_total_hours"), 0) & "</font></td></tr>"
        End If
        ' landing cycles
        If Not IsDBNull(tempTable.Rows(0).Item("ac_airframe_total_landings")) Then
          cycles = "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Landings/Cycles: </font><font class='text_text'>" & FormatNumber(tempTable.Rows(0).Item("ac_airframe_total_landings"), 0) & "</font></td></tr>"
        End If

        If Not IsDBNull(tempTable.Rows(0).Item("ac_asking_wordage")) Then
          asking_type = tempTable.Rows(0).Item("ac_asking_wordage")
          If Not IsDBNull(tempTable.Rows(0).Item("ac_status")) Then
            ac_status = tempTable.Rows(0).Item("ac_status")
            If ac_status = "For Sale/Trade" And asking_type = "Sale/Trade" Then
              ac_status = ""
            Else
              ac_status = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Status: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_status") & "</font></td></tr>"
            End If
          End If

          If (asking_type = "Price") Then
            asking_type = ""
            If Not IsDBNull(tempTable.Rows(0).Item("ac_asking_price")) Then
              asking_price += "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Asking Amt (USD): </font><font class='text_text'>" & FormatCurrency(tempTable.Rows(0).Item("ac_asking_price"), 0) & "</font></td></tr>"
            End If
          Else
            asking_type = "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Asking: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_asking_wordage") & "</font></td></tr>"

          End If
        Else
          If Not IsDBNull(tempTable.Rows(0).Item("ac_asking_price")) Then
            asking_price = "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Asking Amt (USD): </font><font class='text_text'>" & FormatCurrency(tempTable.Rows(0).Item("ac_asking_price"), 0) & "</font></td></tr>"
          End If
        End If

        ' asking amt
        If Not IsDBNull(tempTable.Rows(0).Item("ac_interior_doneby_name")) Then
          in_done_by_and_rating = in_done_by_and_rating & "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text2'>Done By: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_interior_doneby_name") & "</font></td></tr>"
        End If
        If Not IsDBNull(tempTable.Rows(0).Item("ac_interior_rating")) Then
          in_done_by_and_rating = in_done_by_and_rating & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Rating: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_interior_rating") & "</font></td></tr>"
        End If
        If Not IsDBNull(tempTable.Rows(0).Item("ac_passenger_count")) Then
          in_done_by_and_rating = in_done_by_and_rating & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Number of Passengers: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_passenger_count") & "</font></td></tr>"
        End If
        ' THIS IS FOR INTERIOR SECTION ------ 
        If Not IsDBNull(tempTable.Rows(0).Item("ac_interior_month_year")) Then
          'htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td>"
          If tempTable.Rows(0).Item("ac_interior_month_year").ToString.Length > 4 Then
            temp_moyear = tempTable.Rows(0).Item("ac_interior_month_year")
            If temp_moyear.ToString.Length = 5 Then
              temp_moyear = Left(temp_moyear, 1) & "/" & Right(temp_moyear, 4)
            Else
              If temp_moyear.ToString.Length = 6 Then
                temp_moyear = Left(temp_moyear, 2) & "/" & Right(temp_moyear, 4)
              End If
            End If
            last_updated = " (<font class='small_header_text'>Updated: </font><font class='text_text'> " & temp_moyear & "</font>)"
          Else
            last_updated = " (<font class='small_header_text'>Updated: </font><font class='text_text'> " & tempTable.Rows(0).Item("ac_interior_month_year") & "</font>)"
          End If
        End If
      End If

      ' THIS IS FOR INTERIOR SECTION ------ 
      If Not BR.Checked Then
        If Not IsDBNull(tempTable.Rows(0).Item("ac_exclusive_flag")) Then
          If (tempTable.Rows(0).Item("ac_exclusive_flag") = "Y") Then
            exclusive_flag = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>On Exclusive "


            If Client = True Then

              Client_Query = "select clicomp_id as comp_id, cliact_name as act_name, clicomp_name as comp_name, clicontact_id as contact_id,"
              Client_Query = Client_Query & " clicontact_email_address as contact_email_address from client_aircraft_reference inner join client_company on cliacref_comp_id = clicomp_id  "
              Client_Query = Client_Query & " inner join client_aircraft_contact_type on cliacref_contact_type = cliact_type"
              Client_Query = Client_Query & " left outer join client_contact on cliacref_contact_id = clicontact_id "
              Client_Query = Client_Query & " where cliacref_cliac_id = '" & ac_id & "' and cliacref_contact_type <> '71' and cliact_name = 'Exclusive Broker' "

              MySqlConn2.ConnectionString = Client_DB
              MySqlCommand2.CommandText = Client_Query

              MySqlConn2.Open()
              MySqlCommand2.Connection = MySqlConn2
              MySqlCommand2.CommandType = CommandType.Text
              MySqlCommand2.CommandTimeout = 60
              adoRSAircraft2 = MySqlCommand2.ExecuteReader(CommandBehavior.CloseConnection)

              Try
                temptable2.Load(adoRSAircraft2)
              Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable2.GetErrors()
              End Try

            ElseIf Jetnet = True Then

              Jetnet_Query = "select comp_id, actype_name as act_name, comp_name, contact_id,"
              Jetnet_Query = Jetnet_Query & " contact_email_address from aircraft_reference with (NOLOCK) inner join company on cref_comp_id = comp_id "
              Jetnet_Query = Jetnet_Query & " inner join aircraft_contact_type with (NOLOCK) on actype_code = cref_contact_type"
              Jetnet_Query = Jetnet_Query & " left outer join contact with (NOLOCK) on cref_contact_id = contact_id and cref_journ_id = contact_journ_id "
              Jetnet_Query = Jetnet_Query & " where cref_ac_id = '" & ac_id & "' and cref_journ_id = 0 and cref_contact_type <> '71' and actype_name = 'Exclusive Broker' "

              SqlConn2.ConnectionString = Ref_DB

              SqlConn2.Open()
              SqlCommand2.Connection = SqlConn2

              SqlCommand2.CommandText = Jetnet_Query
              SqlReader2 = SqlCommand2.ExecuteReader(CommandBehavior.CloseConnection)
              SqlCommand2.CommandType = CommandType.Text
              SqlCommand2.CommandTimeout = 60

              Try
                temptable2.Load(SqlReader2)
              Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable2.GetErrors()
              End Try

              SqlReader2.Close()
              SqlReader2 = Nothing
              SqlConn2.Close()
              SqlConn2 = Nothing


            End If

            If temptable2.Rows.Count > 0 Then
              'Aircraft_Contacts = Aircraft_Contacts & "<tr><td>&nbsp;</td></tr>"
              ' For Each t As DataRow In temptable2.Rows
              If Not IsDBNull(temptable2.Rows(0).Item("act_name")) Then
                exclusive_flag = exclusive_flag & " with " & temptable2.Rows(0).Item("comp_name")
              End If
              'Next
            End If

            exclusive_flag = exclusive_flag & "</font></td></tr>"
          End If
        End If
      End If


      If Not IsDBNull(tempTable.Rows(0).Item("ac_date_listed")) Then
        '  list_date = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Date Listed: </font><font class='text_text'>" & adoRSAircraft2("ac_list_date") & "</font></td></tr>"
        list_date = "Date Listed: " & tempTable.Rows(0).Item("ac_date_listed")
      End If

      If Not IsDBNull(tempTable.Rows(0).Item("ac_exterior_doneby_name")) Then
        ex_done_by_and_rating = ex_done_by_and_rating & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Done By: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_exterior_doneby_name") & "</font></td></tr>"
      End If
      If Not IsDBNull(tempTable.Rows(0).Item("ac_exterior_rating")) Then
        ex_done_by_and_rating = ex_done_by_and_rating & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Rating: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_exterior_rating") & "</font></td></tr>"
      End If

      If tempTable.Rows(0).Item("ac_exterior_month_year").ToString.Trim.Length > 0 Then
        If tempTable.Rows(0).Item("ac_exterior_month_year").ToString.Trim.Length > 4 Then
          temp_ex_moyear = tempTable.Rows(0).Item("ac_exterior_month_year")
          If temp_ex_moyear.ToString.Trim.Length = 5 Then
            temp_ex_moyear = Left(temp_ex_moyear, 1) & "/" & Right(temp_ex_moyear, 4)
          Else
            If temp_ex_moyear.ToString.Trim.Length = 6 Then
              temp_ex_moyear = Left(temp_ex_moyear, 2) & "/" & Right(temp_ex_moyear, 4)
            End If
          End If
          ex_date = " (<font class='small_header_text'>Updated: </font><font class='text_text'>" & temp_ex_moyear & "</font>)</td>"
        Else
          ex_date = " (<font class='small_header_text'>Updated: </font><font class='text_text'>" & tempTable.Rows(0).Item("ac_exterior_month_year") & "</font>)</td>"
        End If
      End If

      'If Not IsDBNull(adoRSAircraft2("ac_previously_owned_flag")) Then
      '    prev_owned = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Previously Owned</font></td></tr>"
      'End If

      'If Not IsDBNull(adoRSAircraft2("ac_times_as_of_date")) Then
      '    'times_of_date = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Times as of </font><font class='text_text'>" & adoRSAircraft2("ac_times_as_of_date") & "</font></td></tr>"
      '    times_of_date = "(Times as of " & adoRSAircraft2("ac_times_as_of_date") & ")"
      'End If

      'SECTION 2  - AIRCRAFT INFO  -----------------------------------------------------------------------------------------------------------------------------------

      If (tempTable.Rows(0).Item("ac_forsale_flag") = "Y") Then
        'If Not IsDBNull(asking_type) Or Not IsDBNull(list_date) Or Not IsDBNull(asking_type) Or Not IsDBNull(asking_price) Then
        htmlOutput = htmlOutput & "<tr><td height='5'></td></tr>"
        htmlOutput = htmlOutput & "<tr><td colspan='2' width='100%' class='header_text'>For Sale Information (" & list_date & ")</b></font>&nbsp;</td></tr>"
        htmlOutput = htmlOutput & ac_status
        htmlOutput = htmlOutput & asking_type
        '      htmlOutput = htmlOutput & list_date
        htmlOutput = htmlOutput & asking_price
        htmlOutput = htmlOutput & confidential
        htmlOutput = htmlOutput & exclusive_flag
      End If


      '  If Not IsDBNull(times_of_date) Or IsDBNull(airfram_tot_time) Or IsDBNull(cycles) Then
      If (airfram_tot_time <> "" Or cycles <> "") Then
        htmlOutput = htmlOutput & "<tr><td height='5'></td></tr>"
        htmlOutput = htmlOutput & "<tr><td colspan='2' width='100%' class='header_text'>Usage</font>&nbsp;"
        htmlOutput = htmlOutput & times_of_date
        htmlOutput = htmlOutput & airfram_tot_time
        htmlOutput = htmlOutput & cycles
        htmlOutput = htmlOutput & prev_owned
      End If


      ''----------------------------------------- END OF DETAILS SECTION----------------------------------------------------------------

      htmlOutput = htmlOutput & Aircraft_Details("Interior", last_updated, in_done_by_and_rating)
      htmlOutput = htmlOutput & Aircraft_Details("Exterior", ex_date, ex_done_by_and_rating)
      htmlOutput = htmlOutput & Build_PDF_Format()
      htmlOutput = htmlOutput & build_full_spec_first_picture(ac_id, SqlCommand, SqlReader)
      htmlOutput = htmlOutput & Aircraft_Build_PDF_Features(ac_id)
      htmlOutput = htmlOutput & Build_PDF_Airport_Information(ac_id)
      htmlOutput = htmlOutput & End_Page()


      Build_PDF_CoverPage = Build_PDF_CoverPage & htmlOutput
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_CoverPage(ByVal ac_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_CoverPage(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally
      SqlConn.Close()
      SqlCommand.Dispose()
    End Try
  End Function

  Public Function build_full_spec_first_picture(ByVal nAircraftID As Long, ByRef SqlCommand As SqlClient.SqlCommand, ByRef adoRSAircraft As SqlClient.SqlDataReader) As String
    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Dim outString As String = ""

    ' This is right side column, should start with an open table already in td
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Integer = 0
    Dim pic_seq_num As Integer = 0
    Dim something_shown As Integer = 0
    Dim pic_size As Integer = 310
    Dim full_image_path As String = ""

    Dim fApicSubject As String = ""
    Dim pic_height_size As Integer = 310
    Dim pic_width As Integer = 310

    Dim imgDisplayFolder As String = ""
    imgDisplayFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath")

    Dim imgFileName As String = ""
    Dim htmlOutput As String = ""
    Dim div_by As Double = 0
    Dim Query As String : Query = ""

    Dim ac_image_file As String = ""
    Dim temp_height As Integer = 0
    Dim temp_width As Integer = 0
    Dim zimage2 As System.Drawing.Image
    Dim desired_width As Integer = 310
    Dim temp_percent1 As Double = 0.0
    Dim temp_percent2 As Double = 0.0


    Try

      ' start AC_Pic
      Query = "SELECT TOP 1 * FROM Aircraft_Pictures WITH(NOLOCK)"

      If Jetnet = True Then
        Query = Query & " WHERE acpic_ac_id = " & nAircraftID.ToString
      Else
        nAircraftID = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(nAircraftID.ToString, False)
        Query = Query & " WHERE acpic_ac_id = " & nAircraftID.ToString
      End If

      Query = Query & " AND acpic_journ_id = 0"
      Query = Query & " AND acpic_seq_no > 0"
      Query = Query & " AND acpic_image_type = 'JPG'"
      Query = Query & " AND acpic_hide_flag = 'N'"
      Query = Query & " ORDER BY acpic_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        pic_seq_num = CInt(adoRSAircraft.Item("acpic_seq_no").ToString)

        If Not IsDBNull(adoRSAircraft("acpic_image_type")) Then
          fAcpic_image_type = adoRSAircraft.Item("acpic_image_type").ToString.ToLower.Trim
        End If

        If Not IsDBNull(adoRSAircraft("acpic_id")) Then
          fAcpic_id = adoRSAircraft.Item("acpic_id").ToString.Trim
        End If

        If Not (IsDBNull(adoRSAircraft("acpic_subject"))) Then
          fApicSubject = adoRSAircraft.Item("acpic_subject").ToString.Trim
        End If

        imgFileName = nAircraftID.ToString & Constants.cHyphen & "0" & Constants.cHyphen & fAcpic_id.ToString & Constants.cDot & fAcpic_image_type.ToLower.Trim

        full_image_path = imgDisplayFolder.Trim + "/" + imgFileName.Trim

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' setup the path for the pictures based on which site is running
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try


          ac_image_file = HttpContext.Current.Server.MapPath("pictures\aircraft\") & imgFileName
          zimage2 = System.Drawing.Image.FromFile(ac_image_file)
          temp_width = zimage2.Width
          temp_height = zimage2.Height

          ' if the image is wider then the desired image width, then shirnk down to size.
          If temp_width > desired_width Then
            temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
          End If

          'assuming generally that a square is fine
          If temp_height > temp_width Then
            temp_percent1 = CDbl(CDbl(temp_width) / CDbl(temp_height))
            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
          End If

          If Me.WD.SelectedValue.ToString = "Word" Then
            outString &= "<img Title=""" & fApicSubject.Trim & """ alt=""" & fApicSubject.Trim & """ src=""" & full_image_path & """ width=""" & temp_width & """ height=""" & temp_height & """ /><br /><br />"
          Else
            outString &= "<img Title=""" & fApicSubject.Trim & """ alt=""" & fApicSubject.Trim & """ src=""" & full_image_path & """ width=""" & temp_width & """  /><br /><br />"
          End If
        Catch ex As Exception

          If Me.WD.SelectedValue.ToString = "Word" Then
            pic_width = 250
            pic_height_size = 250
          End If

          outString &= "<img Title=""" & fApicSubject.Trim & """ alt=""" & fApicSubject.Trim & """ src=""" & full_image_path & """ width=""" & pic_width & """ height=""" & pic_height_size & """ /><br /><br />"
        End Try



        something_shown = 1

      End If

      adoRSAircraft.Close()

      If something_shown = 0 Then
        outString = "No Pictures Available"
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_first_picture()" + ex.Message
    End Try

    Return outString

  End Function


  Public Function Build_PDF_Second_Page(ByVal ac_id As Integer, ByVal temp_header_text As String) As String
    Build_PDF_Second_Page = ""
    Dim temptable As New DataTable
    Dim htmlOutput As String = ""
    Dim ac_maintained As String = ""
    Dim ac_airframe_maint_tracking_prog_AMTP As String = ""
    Dim ac_airframe_maintenance_prog_AMP As String = ""
    Dim cert_information As String = ""
    Dim en_overhaul As String = ""
    Dim hot_inspec As String = ""
    Dim dam_history As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""

    Try

      If Client = True Then

        Client_Query = "select cliaircraft_id as ac_id, cliaircraft_ser_nbr as ac_ser_nbr, amp_program_name, amtp_program_name, cliaircraft_damage_history_notes as ac_damage_history_notes from client_aircraft"
        Client_Query = Client_Query & " inner join Airframe_Maintenance_Program on cliaircraft_airframe_maintenance_program=amp_id"
        Client_Query = Client_Query & " inner join Airframe_Maintenance_Tracking_Program on cliaircraft_airframe_maintenance_tracking_program=amtp_id"
        Client_Query = Client_Query & " where(cliaircraft_id = " & ac_id & ")"

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then

        Jetnet_Query = "select ac_id, ac_ser_no as ac_ser_nbr, amp_program_name, amtp_program_name, ac_damage_history_notes from aircraft with (NOLOCK) "
        Jetnet_Query = Jetnet_Query & " inner join Airframe_Maintenance_Program  with (NOLOCK) on ac_airframe_maintenance_prog_AMP=amp_id"
        Jetnet_Query = Jetnet_Query & " inner join Airframe_Maintenance_Tracking_Program  with (NOLOCK) on aircraft.ac_airframe_maint_tracking_prog_AMTP=amtp_id"
        Jetnet_Query = Jetnet_Query & " where(ac_id = " & ac_id & ") and ac_journ_id = 0 "
        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If


      If temptable.Rows.Count > 0 Then

        If Not IsDBNull(temptable.Rows(0).Item("amp_program_name")) Then
          ac_airframe_maintenance_prog_AMP = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Airframe Maintenance Program: </font><font class='text_text2'>" & temptable.Rows(0).Item("amp_program_name") & "</font></td></tr>"

        End If
        If Not IsDBNull(temptable.Rows(0).Item("amtp_program_name")) Then
          ac_airframe_maint_tracking_prog_AMTP = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Airframe Maintenance Tracking Program: </font><font class='text_text2'>" & temptable.Rows(0).Item("amtp_program_name") & "</font></td></tr>"

        End If
        If Not IsDBNull(temptable.Rows(0).Item("ac_damage_history_notes")) Then
          dam_history = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Dam History Notes: </font><font class='text_text2'>" & temptable.Rows(0).Item("ac_damage_history_notes") & "</font></td></tr>"
        End If
      End If


      If Trim(temp_header_text) <> "" Then
        Build_PDF_Second_Page = Build_PDF_Second_Page & Trim(Replace(temp_header_text, "Market Value Analysis", "Value Analysis - Aircraft Info (2)"))
      Else
        Build_PDF_Second_Page = Build_PDF_Second_Page & Build_PDF_Header("Aircraft Information (Page 2 of 2)", "")
      End If

      Build_PDF_Second_Page = Build_PDF_Second_Page & DisplayEngineInfo(ac_id)
      Build_PDF_Second_Page = Build_PDF_Second_Page & Aircraft_APU(ac_id)
      Build_PDF_Second_Page = Build_PDF_Second_Page & Aircraft_Details("Maintenance", ac_maintained, "")
      Build_PDF_Second_Page = Build_PDF_Second_Page & ac_airframe_maintenance_prog_AMP & ac_airframe_maint_tracking_prog_AMTP & cert_information
      Build_PDF_Second_Page = Build_PDF_Second_Page & hot_inspec & en_overhaul & dam_history
      Build_PDF_Second_Page = Build_PDF_Second_Page & Aircraft_Details("Equipment", "", "")
      Build_PDF_Second_Page = Build_PDF_Second_Page & Aircraft_Details("Addl Cockpit Equipment", "", "")
      Build_PDF_Second_Page = Build_PDF_Second_Page & Aircraft_Avionics(ac_id)
      Build_PDF_Second_Page = Build_PDF_Second_Page & check_custom_fields(ac_id)
      ' 'Build_PDF_Second_Page = Build_PDF_Second_Page & Build_PDF_Format()
      Build_PDF_Second_Page = Build_PDF_Second_Page & End_Page()

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Second_Page(ByVal ac_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Second_Page(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally

      adoRSAircraft = Nothing
      MySqlConn.Close()

      SqlReader = Nothing
      SqlConn.Close()
    End Try
  End Function
  Public Function Build_PDF_Third_Page(ByVal ac_id As Integer) As String
    Build_PDF_Third_Page = ""
    Try
      Build_PDF_Third_Page = Build_PDF_Third_Page & Build_PDF_Header("Aircraft Contacts", "")
      Build_PDF_Third_Page = Build_PDF_Third_Page & Aircraft_Contacts(ac_id)
      ' Third_Page = Third_Page & Build_PDF_Format()
      'Build_PDF_Third_Page = Build_PDF_Third_Page & "</td></tr></table>"
      Build_PDF_Third_Page = Build_PDF_Third_Page & End_Page()
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Third_Page As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Third_Page() As String", aclsData_Temp)
    End Try
  End Function

  Public Function Build_PDF_Fourth_Page(ByVal ac_id As Integer) As String
    Build_PDF_Fourth_Page = ""
    Try
      Build_PDF_Fourth_Page = Build_PDF_Fourth_Page & Build_PDF_Header("Aircraft Notes", "")
      Build_PDF_Fourth_Page = Build_PDF_Fourth_Page & Aircraft_Notes(ac_id, "A", "", "")
      Build_PDF_Fourth_Page = Build_PDF_Fourth_Page & End_Page()
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Fourth_Page As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Fourth_Page() As String", aclsData_Temp)

    End Try
  End Function

  Public Function Build_PDF_Pictures_Page(ByVal ac_id As Integer) As String
    Build_PDF_Pictures_Page = ""
    Dim MyAircraftSearch As Boolean = False
    Dim ModelSearchVariable As Boolean = False
    Dim added_title As String = ""
    Dim added_string_query As String = ""

    Try


      Build_PDF_Pictures_Page = Build_PDF_Pictures_Page & Build_PDF_Header("Aircraft Pictures: " & added_title, "")
      Build_PDF_Pictures_Page = Build_PDF_Pictures_Page & build_full_spec_pictures_page(ac_id)
      Build_PDF_Pictures_Page = Build_PDF_Pictures_Page & End_Page()
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Fifth_Page As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Fifth_Page() As String", aclsData_Temp)
    End Try
  End Function

  Public Function Build_PDF_Fifth_Page(ByVal ac_id As Integer) As String
    Build_PDF_Fifth_Page = ""
    Dim MyAircraftSearch As Boolean = False
    Dim ModelSearchVariable As Boolean = False
    Dim added_title As String = ""
    Dim added_string_query As String = ""

    Try


      If prospect_type.SelectedValue = "AC" Then
        MyAircraftSearch = True

        If Jetnet = True Then
          added_string_query = " and lnote_jetnet_ac_id = " & ac_id & " "
        Else
          added_string_query = " and lnote_client_ac_id = " & ac_id & " "
        End If


        added_title = "My Aircraft"
      ElseIf prospect_type.SelectedValue = "ACMODEL" Then
        MyAircraftSearch = True
        ModelSearchVariable = current_model_id
        added_title = current_make_model & " or My Aircraft"

        If Jetnet = True Then
          added_string_query = " and ((lnote_jetnet_amod_id = " & current_model_id & " and lnote_jetnet_ac_id = 0) or (lnote_jetnet_ac_id = " & ac_id & ")) "
        Else
          added_string_query = " and ((lnote_client_amod_id = " & current_model_id & " and lnote_client_ac_id = 0) or (lnote_client_ac_id = " & ac_id & ")) "
        End If


      ElseIf prospect_type.SelectedValue = "MODEL" Then
        MyAircraftSearch = False
        ModelSearchVariable = current_model_id
        added_title = current_make_model

        If Jetnet = True Then
          added_string_query = " and (lnote_jetnet_amod_id = " & current_model_id & " or lnote_jetnet_ac_id = " & ac_id & ") "
        Else
          added_string_query = " and (lnote_client_amod_id = " & current_model_id & " or lnote_client_ac_id = " & ac_id & ") "
        End If


      End If





      Build_PDF_Fifth_Page = Build_PDF_Fifth_Page & Build_PDF_Header("Aircraft Prospects: " & added_title, "")
      Build_PDF_Fifth_Page = Build_PDF_Fifth_Page & Aircraft_Notes(ac_id, "B", prospect_type.Text.ToString, added_string_query)
      Build_PDF_Fifth_Page = Build_PDF_Fifth_Page & End_Page()
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Fifth_Page As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Fifth_Page() As String", aclsData_Temp)
    End Try
  End Function

  Public Function Build_PDF_Sixth_Page(ByVal ac_id As Integer) As String
    Build_PDF_Sixth_Page = ""
    Dim internal As String = ""
    Dim retail As String = ""
    Dim client_Ac_id As Long = 0
    Try

      If Trim(Request("internal")) <> "" Then
        internal = Trim(Request("internal"))
      End If

      If Trim(Request("retail")) <> "" Then
        retail = Trim(Request("retail"))
      End If

      Build_PDF_Sixth_Page = Build_PDF_Sixth_Page & Build_PDF_Header("Price History", "")

      ' this should, for this example, pass in the client ac id 
      If Jetnet Then
        client_Ac_id = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(ac_id.ToString, True)
        Build_PDF_Sixth_Page = Build_PDF_Sixth_Page & crmViewDataLayer.Build_Compare_Graphs(client_Ac_id, 0, True, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, "")
      Else
        Build_PDF_Sixth_Page = Build_PDF_Sixth_Page & crmViewDataLayer.Build_Compare_Graphs(ac_id, 0, True, internal, retail, localDatalayer, Me.ANALYTICS_HISTORY, Server.MapPath("TempFiles"), COMPLETED_OR_OPEN, searchCriteria, Me.Page, Me.bottom_tab_update_panel, RS.Checked, MSV.Checked, MSA.Checked, SC.Checked, CMC.Checked, False, "")
      End If



      Build_PDF_Sixth_Page = Build_PDF_Sixth_Page & End_Page()
    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Fifth_Page As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Fifth_Page() As String", aclsData_Temp)
    End Try
  End Function




  Public Function build_full_spec_pictures_page(ByVal nAircraftID As Long) As String

    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Dim outString As String = ""

    Dim imgDisplayFolder As String = ""
    imgDisplayFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath")

    Dim imgFileName As String = ""

    ' This is right side column, should start with an open table already in td

    Dim fApicSubject As String = ""
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Integer = 0
    Dim pic_seq_num As Integer = 0
    Dim row_color As String = "gray"
    Dim pic_counter As Integer = 0
    Dim pic_size As Integer = 250
    Dim pic_height_size As Integer = 200
    Dim nCount As Integer = 0
    Dim something_shown As Boolean = False

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader = Nothing
    Dim SqlException As SqlClient.SqlException = Nothing

    Dim ac_image_file As String = ""
    Dim temp_height As Integer = 0
    Dim temp_width As Integer = 0
    Dim zimage2 As System.Drawing.Image
    Dim desired_width As Integer = 250
    Dim temp_percent1 As Double = 0.0
    Dim temp_percent2 As Double = 0.0
    Dim add_pic As String = "N"
    Dim height_size_total As Integer = 1000
    Dim total_height As Integer = 0



    Try

      sQuery.Append("SELECT * FROM Aircraft_Pictures WITH(NOLOCK)")   ' 1 on title - 3 on next - 6 on this

      If Jetnet = True Then
        sQuery.Append("  WHERE acpic_ac_id = " & nAircraftID.ToString & "")
      Else
        sQuery.Append("  WHERE acpic_ac_id = " & localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(nAircraftID.ToString, False) & "")
      End If

      sQuery.Append(" AND acpic_journ_id = 0")
      sQuery.Append(" AND acpic_seq_no > 0")
      sQuery.Append(" AND acpic_image_type = 'JPG'")
      sQuery.Append(" AND acpic_hide_flag = 'N'")
      sQuery.Append(" ORDER BY acpic_seq_no")

      SqlConn.ConnectionString = Ref_DB
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      outString = "<table width='100%' align='center' cellspacing='0' cellpadding='4'>"

      If SqlReader.HasRows Then

        Do While SqlReader.Read

          If nCount > 1 Then

            pic_seq_num = CInt(SqlReader.Item("acpic_seq_no").ToString)

            If Not IsDBNull(SqlReader("acpic_image_type")) Then
              fAcpic_image_type = SqlReader.Item("acpic_image_type").ToString.ToLower.Trim
            End If

            If Not IsDBNull(SqlReader("acpic_id")) Then
              fAcpic_id = SqlReader.Item("acpic_id").ToString.Trim
            End If

            If Not (IsDBNull(SqlReader("acpic_subject"))) Then
              fApicSubject = SqlReader.Item("acpic_subject").ToString.Trim
            End If

            imgFileName = nAircraftID.ToString + Constants.cHyphen + "0" + Constants.cHyphen + fAcpic_id.ToString + Constants.cDot + fAcpic_image_type.ToLower.Trim

            If chkPP_Large.Checked = True Then
              desired_width = 800
            Else
              desired_width = 250
            End If

            If chkPP_Large.Checked = True Then

              If Me.WD.SelectedValue.ToString = "Word" Then
                'outString += "<tr bgcolor='#F1F1F1'><td valign='middle' align='center' class='text_text'>"
                If pic_counter = 0 Then
                  If row_color = "white" Then
                    outString += "<tr bgcolor='#F1F1F1'><td width='90%' valign='middle' align='center' class='text_text'>"
                    row_color = "gray"
                  Else
                    outString += "<tr bgcolor='#D0D0D0'><td width='90%' valign='middle' align='center' class='text_text'>"
                    row_color = "white"
                  End If
                End If
              Else
                If pic_counter = 0 Then
                  If row_color = "white" Then
                    outString += "<tr bgcolor='#F1F1F1'><td width='90%' valign='middle' align='center' class='text_text'>"
                    row_color = "gray"
                  Else
                    outString += "<tr bgcolor='#D0D0D0'><td width='90%' valign='middle' align='center' class='text_text'>"
                    row_color = "white"
                  End If
                End If
              End If

            Else

              If Me.WD.SelectedValue.ToString = "Word" Then
                'outString += "<tr bgcolor='#F1F1F1'><td valign='middle' align='center' class='text_text'>"
                If pic_counter = 0 Then
                  If row_color = "white" Then
                    outString += "<tr bgcolor='#F1F1F1'><td width='50%' valign='middle' align='center' class='text_text'>"
                    row_color = "gray"
                  Else
                    outString += "<tr bgcolor='#D0D0D0'><td width='50%' valign='middle' align='center' class='text_text'>"
                    row_color = "white"
                  End If
                End If
              Else
                If pic_counter = 0 Then
                  If row_color = "white" Then
                    outString += "<tr bgcolor='#F1F1F1'><td width='33%' valign='middle' align='center' class='text_text'>"
                    row_color = "gray"
                  Else
                    outString += "<tr bgcolor='#D0D0D0'><td width='33%' valign='middle' align='center' class='text_text'>"
                    row_color = "white"
                  End If
                End If
              End If

            End If



            Try


              ac_image_file = HttpContext.Current.Server.MapPath("pictures\aircraft\") & imgFileName
              zimage2 = System.Drawing.Image.FromFile(ac_image_file)
              temp_width = zimage2.Width
              temp_height = zimage2.Height

              If chkPP_Large.Checked = True Then

                'if width > height, then shrink the width down 
                If temp_width > temp_height Then
                  If temp_width > desired_width Then
                    temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                  End If
                ElseIf temp_height > temp_width Then
                  ' if taller than it is wide, then as long as not too wide, its ok till height check
                  If temp_width > desired_width Then
                    temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                  End If
                End If

                ' make sure not too tall for a given page size
                If temp_height > height_size_total Then
                  temp_percent1 = CDbl(CDbl(height_size_total) / CDbl(temp_height))
                  temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                  temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If



              Else
                ' if the image is wider then the desired image width, then shirnk down to size.
                If temp_width > desired_width Then
                  temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                  temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                  temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If

                'assuming generally that a square is fine
                If temp_height > temp_width Then
                  temp_percent1 = CDbl(CDbl(temp_width) / CDbl(temp_height))
                  temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                  temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If
              End If


              ' Call CommonAircraftFunctions.find_image_resize_to_fit(temp_width, temp_height, desired_width, desired_height, javascript_slideshow_begining, r, image_folder_display, fAcpic_id, fAcpic_image_type, fAcpic_subject, "Yacht", yacht_id, journalID)

              If Trim(Request("pic_test")) = "Y" Then
                If chkPP_Large.Checked = True Then
                  total_height = total_height + temp_height

                  add_pic = "Y"
                  If total_height > height_size_total Then
                    add_pic = "N"
                  End If

                  If add_pic = "N" Then
                    outString += "</td></tr>"
                    outString += Insert_Page_Break()
                    total_height = 0
                  End If
                End If
              End If


              If Me.WD.SelectedValue.ToString = "Word" Then
                outString += "<img title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" + temp_width.ToString + "'  height='" + temp_height.ToString + "'/><br /><br />"
              Else
                outString += "<img title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" + temp_width.ToString + "' /><br /><br />"
              End If


            Catch ex As Exception

              If Me.WD.SelectedValue.ToString = "Word" Then
                outString += "<img title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" + desired_width.ToString + "'  height='" + desired_width.ToString + "'/><br /><br />"
              Else
                outString += "<img title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" + desired_width.ToString + "' /><br /><br />"
              End If
            End Try


            If chkPP_Large.Checked = True Then

              outString += "</td></tr>"
              something_shown = True

            Else


              something_shown = True

              If Me.WD.SelectedValue.ToString = "Word" Then
                If pic_counter < 1 Then
                  outString += "</td><td width='33%' valign='middle' align='center' class='text_text'>"
                  pic_counter += 1
                Else
                  outString += "</td></tr>"
                  pic_counter = 0
                End If
              Else
                If pic_counter < 2 Then
                  outString += "</td><td width='33%' valign='middle' align='center' class='text_text'>"
                  pic_counter += 1
                Else
                  outString += "</td></tr>"
                  pic_counter = 0
                End If
              End If

            End If


          End If

          nCount += 1


        Loop

        If pic_counter = 0 Then
          outString += "</td>"
          outString += "<td width='33%' valign='middle' align='center' class='text_text'>&nbsp;</td></tr>"
        ElseIf pic_counter = 1 Then
          outString += "</td><td width='33%' valign='middle' align='center' class='text_text'>&nbsp;</td></tr>"
        End If

      End If

      If Not something_shown Then
        outString += "<tr><td valign='middle' align='center' class='text_text'> No Additional Pictures Currently Available </td></tr>"
      End If

      outString += "</table>"


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_pictures_page_pics()" + ex.Message
    Finally

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

      SqlReader.Close()


    End Try

    Return outString

  End Function
  Public Function Aircraft_Notes(ByVal ac_id As Integer, ByVal LNOTE_STATUS As String, ByVal show_type As String, ByVal added_string As String) As String
    Dim daystr As String = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now())
    Dim lnote_order As String = "lnote_schedule_start_date asc "
    Dim Note_Array As New ArrayList
    Dim aTempTable As New DataTable
    Dim TYPE As String = "note" 'Default Note View 
    Dim URL_STRING As String = ""
    Dim CAT_KEY As Integer = 0
    Dim DEFAULT_WIDTH As Integer = 300
    Dim UL_CSS_CLASS As String = "notes_list"
    Dim DIV_CSS_CLASS As String = "notes_list_div"
    Dim NOTES_STRING As String = "<tr valign='top'><td bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'>"
    Dim TYPE_OF_LISTING As Integer = 1
    Dim USED_ID As Integer = 0
    Dim USED_SOURCE As String = ""
    Dim COLOR As String = "#E6E6E6"
    Dim sExtraCompanyInfo As String = ""
    NOTES_STRING = ""
    'Going to create an array of notes for display purposes. First we need the query.  
    'First I need to run a query to get the notes. 
    'This will be the limited query. It only queries for 5 notes.

    If Trim(show_type) <> "" Then
      show_type = show_type
    Else
      show_type = "AC"
    End If

    If Jetnet = True Then
      aTempTable = aclsData_Temp.DUAL_Notes_LIMIT(show_type, ac_id, LNOTE_STATUS, "JETNET", daystr, lnote_order, 5000, added_string)
    Else
      aTempTable = aclsData_Temp.DUAL_Notes_LIMIT(show_type, ac_id, LNOTE_STATUS, "CLIENT", daystr, lnote_order, 5000, added_string)
    End If



    If aTempTable.Rows.Count > 0 Then
      Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(aTempTable)

      If COLOR = "#E6E6E6" Then
        COLOR = "#FFFFFF"
      Else
        COLOR = "#E6E6E6"
      End If

      'For some parameters.. let's set them up.
      Select Case TYPE
        Case "note"
          ' LNOTE_STATUS = "A"
        Case "action"
          'LNOTE_STATUS = "P"
          URL_STRING = "&opp=true"
        Case "opportunity"
          ' LNOTE_STATUS = "O"
          URL_STRING = "action"
      End Select

      NOTES_STRING = NOTES_STRING & "<table class='" & DIV_CSS_CLASS & "' bgcolor='" & COLOR & "' bordercolor='grey' width='750' border='1' cellspacing='0' cellpadding='3'>"
      NOTES_STRING = NOTES_STRING & "<tr valign='top'>"
      NOTES_STRING = NOTES_STRING & "<td align='left' nowrap='nowrap' class='text_text'><b>Date</b></td>"
      If sales_format.Checked = False Then
        NOTES_STRING = NOTES_STRING & "<td align='left' nowrap='nowrap' class='text_text'><b>Entered By:</b></td>"
      End If
      NOTES_STRING = NOTES_STRING & "<td align='left' nowrap='nowrap' class='text_text'><b>Note</b></td>"
      NOTES_STRING = NOTES_STRING & "<td align='left' nowrap='nowrap' class='text_text'><b>Company</b></td>"
      NOTES_STRING = NOTES_STRING & "</tr>"

      For Each Note_Data As clsLocal_Notes In Note_Array

        'Special consideration if the listing is a full notes listing. Meaning the width has to be wider on the note views.
        If CAT_KEY = 0 Then
          DEFAULT_WIDTH = 800
          UL_CSS_CLASS = "notes_list_no_width"
          DIV_CSS_CLASS = "notes_list_div_main"
        End If

        If Note_Data.lnote_notecat_key = CAT_KEY Or CAT_KEY = 0 Then 'If the notes category is equal to the category we're looking at, show the note. 
          NOTES_STRING = NOTES_STRING & "<tr valign='top'><td align='left' nowrap='nowrap' class='text_text'>"
          'NOTES_STRING = NOTES_STRING & "<b>"


          If IsDate(Note_Data.lnote_entry_date) And Note_Data.lnote_status <> "P" Then 'This means it's not an action.

            NOTES_STRING = NOTES_STRING & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_entry_date).Date


            If sales_format.Checked = False Then
              NOTES_STRING = NOTES_STRING & "</td><td align='left' nowrap='nowrap' class='text_text'>"
              ' NOTES_STRING = NOTES_STRING & " (<em>Entered by: " & Note_Data.lnote_user_name & ")</em> </b> - "
              ' NOTES_STRING = NOTES_STRING & " <em>" & Note_Data.lnote_user_name & "</em>  "
              NOTES_STRING = NOTES_STRING & "" & Note_Data.lnote_user_name & ""
              'NOTES_STRING = NOTES_STRING & "</b>"
            End If
          Else
            If Note_Data.lnote_status <> "P" And Note_Data.lnote_status <> "O" Then 'This means it's an action.
              NOTES_STRING = NOTES_STRING & " " & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_schedule_start_date).Date & "</b>"
            Else
              NOTES_STRING = NOTES_STRING & " " & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_schedule_start_date).Date & "</b>"
            End If
          End If


          NOTES_STRING = NOTES_STRING & "</td><td align='left' class='text_text'>"

          'Just displaying the notes text field
          ' If Len(Note_Data.lnote_note) > 100 Then
          'NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Left(Note_Data.lnote_note, 100) & "...")
          'Else
          NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Note_Data.lnote_note)
          ' End If

          NOTES_STRING = NOTES_STRING & "</td><td align='left' width='200' class='text_text'>"

          sExtraCompanyInfo = ""
          'clicomp_name, clicomp_city, clicomp_state, clicomp_country
          If Note_Data.lnote_client_comp_id.ToString > "0" Then
            NOTES_STRING = NOTES_STRING & "" & Server.HtmlEncode(aclsData_Temp.get_company_name_fromID_CRM(Note_Data.lnote_client_comp_id, 0, False, True, sExtraCompanyInfo)) & " "
            NOTES_STRING = NOTES_STRING & "<br>" & sExtraCompanyInfo & ""


            If Note_Data.lnote_client_contact_id.ToString > "0" Then
              NOTES_STRING = NOTES_STRING & "<Br>" & Server.HtmlEncode(aclsData_Temp.get_contact_name_fromID_CRM(Note_Data.lnote_client_contact_id, 0))
            End If

            ' NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Note_Data.clicomp_name)
            'NOTES_STRING = NOTES_STRING & " - (" & Server.HtmlEncode(commonEvo.get_company_name_fromID(Note_Data.lnote_jetnet_comp_id, 0, False, True, sExtraCompanyInfo)) & " "
            ' NOTES_STRING = NOTES_STRING & sExtraCompanyInfo & ")" 
            '    NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Note_Data.lnote_jetnet_comp_id)
          End If



          'NOTES_STRING = NOTES_STRING & "<br /><hr /><br />"
        End If

        NOTES_STRING = NOTES_STRING & "</td></tr>"

      Next

      NOTES_STRING = NOTES_STRING & "</table>"
      ' Aircraft_Notes = NOTES_STRING 'Server.HtmlEncode(NOTES_STRING)
    End If
    Aircraft_Notes = NOTES_STRING
  End Function

  Public Function Aircraft_Contacts(ByVal ac_id As Integer) As String
    Dim temptable As New DataTable
    Dim ac_maintained As String = ""
    Dim column_color As String = "white"
    Dim contact_counter As Integer = 1
    Dim sub_counter As Integer = 1
    Aircraft_Contacts = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""
    Try



      If Client = True Then

        Client_Query = "select clicomp_id as comp_id, cliact_name as act_name, cliacref_owner_percentage as acref_owner_percentage, clicomp_name as comp_name, clicomp_alternate_name as comp_alternate_name, clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, clicomp_city as comp_city, clicomp_state as comp_state, clicomp_zip_code as comp_zip_code, clicomp_country as comp_country,"
        Client_Query = Client_Query & " clicomp_email_address as comp_email_address, clicomp_web_address as comp_web_address, clicontact_id as contact_id, clicontact_sirname as contact_sirname, clicontact_first_name as contact_first_name, clicontact_middle_initial as contact_middle_initial, clicontact_last_name as contact_last_name, clicontact_suffix as contact_suffix, clicontact_title as contact_title,"
        Client_Query = Client_Query & " clicontact_email_address as contact_email_address from client_aircraft_reference inner join client_company on cliacref_comp_id = clicomp_id inner join client_aircraft_contact_type on cliacref_contact_type = cliact_type"
        Client_Query = Client_Query & " left outer join client_contact on cliacref_contact_id = clicontact_id "
        Client_Query = Client_Query & " where(cliacref_cliac_id = '" & ac_id & "') and cliacref_contact_type <> '71' "

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then

        Jetnet_Query = "select comp_id, actype_name as act_name, cref_owner_percent as acref_owner_percentage , comp_name, comp_name_alt as comp_alternate_name, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country,"
        Jetnet_Query = Jetnet_Query & " comp_email_address, comp_web_address, contact_id, contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title,"
        Jetnet_Query = Jetnet_Query & " contact_email_address from aircraft_reference  with (NOLOCK) inner join company  with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id inner join aircraft_contact_type with (NOLOCK) on cref_contact_type = actype_code "
        Jetnet_Query = Jetnet_Query & " left outer join contact with (NOLOCK) on cref_contact_id = contact_id and cref_journ_id = contact_journ_id "
        Jetnet_Query = Jetnet_Query & " where(cref_ac_id = '" & ac_id & "') and cref_journ_id = 0 and cref_contact_type <> '71' "

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If

      For Each r As DataRow In temptable.Rows

        If (column_color = "white") Then
          If Me.WD.SelectedValue.ToString = "Word" Then
            Aircraft_Contacts = Aircraft_Contacts & "<tr valign='top'><td width='400' height='700' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          Else
            Aircraft_Contacts = Aircraft_Contacts & "<tr valign='top'><td width='60%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          End If

        Else
          If Me.WD.SelectedValue.ToString = "Word" Then
            Aircraft_Contacts = Aircraft_Contacts & "<tr valign='top' height='70'><td width='400' height='700' bgcolor='#E6E6E6' class='text_text' cellspacing='0' cellpadding='5'><b>"
          Else
            Aircraft_Contacts = Aircraft_Contacts & "<tr valign='top'><td width='60%' bgcolor='#E6E6E6' class='text_text' cellspacing='0' cellpadding='5'><b>"
          End If
        End If


        If Not IsDBNull(r("act_name")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("act_name")
        End If

        If Not IsDBNull(r("acref_owner_percentage")) Then
          If r("acref_owner_percentage") > 0 And r("acref_owner_percentage") < 100 Then
            Aircraft_Contacts = Aircraft_Contacts & " [" & r("acref_owner_percentage") & "]"
          End If
        End If

        If Not IsDBNull(r("comp_name")) Then
          Aircraft_Contacts = Aircraft_Contacts & " - " & r("comp_name") & " "
        End If

        If Not IsDBNull(r("comp_alternate_name")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("comp_alternate_name") & "</b><br>"
        Else
          Aircraft_Contacts = Aircraft_Contacts & "</b><br>"
        End If
        If Not IsDBNull(r("comp_address1")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("comp_address1") & "<br>"
        End If
        If Not IsDBNull(r("comp_city")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("comp_city")
        End If

        If Not IsDBNull(r("comp_state")) Then
          Aircraft_Contacts = Aircraft_Contacts & ", " & r("comp_state") & " "
        End If

        If Not IsDBNull(r("comp_zip_code")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("comp_zip_code") & " "
        End If

        If Not IsDBNull(r("comp_country")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("comp_country") & "<br>"
        Else
          Aircraft_Contacts = Aircraft_Contacts & "<br>"
        End If

        If Not IsDBNull(r("comp_email_address")) Then
          If r("comp_email_address").ToString.Trim.Length > 0 Then
            Aircraft_Contacts = Aircraft_Contacts & "<u>" & r("comp_email_address") & "</u>" & "<br>"
          End If
        End If

        If Not IsDBNull(r("comp_web_address")) Then
          If r("comp_web_address").ToString.Trim.Length > 0 Then
            Aircraft_Contacts = Aircraft_Contacts & "<u>" & r("comp_web_address") & "</u>" & "<br>"
          End If
        End If
        If Not IsDBNull(r("comp_id")) Then
          Aircraft_Contacts = Aircraft_Contacts & Phone_Company_Contact_Info(r("comp_id"), 0)
        End If
        If (column_color = "white") Then
          If Me.WD.SelectedValue.ToString = "Word" Then
            Aircraft_Contacts = Aircraft_Contacts & "&nbsp;</td><td width='410' bgcolor='#E6E6E6' valign='top' height='70' class='text_text' cellspacing='0' cellpadding='5'><b>"
          Else
            Aircraft_Contacts = Aircraft_Contacts & "&nbsp;</td><td width='40%' bgcolor='#E6E6E6' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          End If
          column_color = "other"
        Else
          If Me.WD.SelectedValue.ToString = "Word" Then
            Aircraft_Contacts = Aircraft_Contacts & "&nbsp;</td><td width='410' bgcolor='white' height='70' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          Else
            Aircraft_Contacts = Aircraft_Contacts & "&nbsp;</td><td width='40%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          End If
          column_color = "white"
        End If
        If Not IsDBNull(r("contact_sirname")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("contact_sirname") & " "
        End If
        If Not IsDBNull(r("contact_first_name")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("contact_first_name") & " "
        End If
        If Not IsDBNull(r("contact_middle_initial")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("contact_middle_initial") & " "
        End If
        If Not IsDBNull(r("contact_last_name")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("contact_last_name") & "</b> "
        End If
        If Not IsDBNull(r("contact_suffix")) Then
          Aircraft_Contacts = "<b>" & Aircraft_Contacts & r("contact_suffix") & "</b><br>"
        End If
        If Not IsDBNull(r("contact_title")) Then
          Aircraft_Contacts = Aircraft_Contacts & r("contact_title") & "<br>"
        End If
        If Not IsDBNull(r("contact_email_address")) Then
          Aircraft_Contacts = Aircraft_Contacts & "<u>" & r("contact_email_address") & "</u>" & "<br>"
        End If
        If Not IsDBNull(r("comp_id")) And Not IsDBNull(r("contact_id")) Then
          '    Aircraft_Contacts = Aircraft_Contacts & Phone_Company_Contact_Info(r("comp_id"), r("contact_id"))
        End If
        Aircraft_Contacts = Aircraft_Contacts & "</td></tr>"
        contact_counter = contact_counter + 1
      Next


      If Me.WD.SelectedValue.ToString = "Word" Then
        If contact_counter < 10 Then
          sub_counter = 10 - contact_counter
          Do While sub_counter > 0
            Aircraft_Contacts = Aircraft_Contacts & "<tr><td height='70'>&nbsp;</td></tr>"
            sub_counter = sub_counter - 1
          Loop
        End If
      End If

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Aircraft_Contacts(ByVal ac_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Aircraft_Contacts(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally

      adoRSAircraft = Nothing
      MySqlConn.Close()

      SqlReader = Nothing
      SqlConn.Close()

    End Try
  End Function
  Public Function Phone_Company_Contact_Info(ByVal comp_id As Integer, ByVal contact_id As Integer) As String
    Phone_Company_Contact_Info = ""
    Dim temptable As New DataTable
    Dim htmlOutput As String = ""
    Dim ac_maintained As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing


    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""

    Try

      If Client = True Then

        Client_Query = "select distinct clipnum_type as pnum_type, clipnum_number as pnum_number from client_Phone_Numbers where clipnum_contact_id = " & contact_id & " and clipnum_comp_id = " & comp_id & ""
        Client_Query = Client_Query & " ORDER BY clipnum_type desc"

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then
        Jetnet_Query = "select distinct pnum_type, pnum_number_full  as pnum_number from Phone_Numbers with (NOLOCK) where pnum_contact_id = " & contact_id & " and pnum_comp_id = " & comp_id & " and pnum_journ_id = 0 "
        Jetnet_Query = Jetnet_Query & " ORDER BY pnum_type desc"

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If


      If temptable.Rows.Count > 0 Then
        For Each q As DataRow In temptable.Rows
          Phone_Company_Contact_Info = Phone_Company_Contact_Info & q("pnum_type") & ": " & q("pnum_number") & "<br>"
        Next
      End If



    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Phone_Company_Contact_Info(ByVal comp_id As Integer, ByVal contact_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Phone_Company_Contact_Info(ByVal comp_id As Integer, ByVal contact_id As Integer) As String", aclsData_Temp)
    Finally

      adoRSAircraft = Nothing
      MySqlConn.Close()

      SqlReader = Nothing
      SqlConn.Close()
    End Try
  End Function
  Public Function Aircraft_Details(ByVal detail_type As String, ByVal update_date As String, ByVal done_by As String) As String
    Aircraft_Details = ""
    Dim htmlOutput As String = ""
    Dim last_detail_type As String = ""
    Dim temptable As New DataTable

    If Client = True Then
      Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
      Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
      Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader
      Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
      Dim Client_Query As String = ""

      Client_Query = "SELECT cliadet_data_description as adet_data_description, cliadet_data_name as adet_data_name FROM client_Aircraft_Details WHERE cliadet_cliac_id = " & ac_id
      Client_Query = Client_Query & " AND cliadet_data_type = '" & detail_type & "'"

      MySqlConn.ConnectionString = Client_DB
      MySqlCommand.CommandText = Client_Query

      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(adoRSAircraft)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      MySqlConn.Close()
      MySqlConn = Nothing
    ElseIf Jetnet = True Then
      Dim Jetnet_Query As String = ""
      Dim SqlConn As New SqlClient.SqlConnection
      Dim SqlCommand As New SqlClient.SqlCommand
      Dim SqlReader As SqlClient.SqlDataReader
      Dim SqlException As SqlClient.SqlException : SqlException = Nothing

      Jetnet_Query = "SELECT * FROM Aircraft_Details with (NOLOCK) WHERE adet_ac_id = " & ac_id
      Jetnet_Query = Jetnet_Query & " AND adet_data_type = '" & detail_type & "' and adet_journ_id = 0 "

      SqlConn.ConnectionString = Ref_DB

      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      SqlCommand.CommandText = Jetnet_Query
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      SqlReader.Close()
      SqlReader = Nothing
      SqlConn.Close()
      SqlConn = Nothing
    End If

    Try
      ' Start Interior Details    -----------------------------------------------------------------

      If temptable.Rows.Count > 0 Then
        Aircraft_Details = Aircraft_Details & "<tr><td height='5'></td></tr>"

        Aircraft_Details = Aircraft_Details & "<tr><td colspan='2' width='100%' class='header_text'>" & detail_type & " Details " & update_date & "</font>&nbsp;</td></tr>"
        Aircraft_Details = Aircraft_Details & done_by
        Aircraft_Details = Aircraft_Details & "<tr><td width='2%'>&nbsp;</td><td>"

        For Each r As DataRow In temptable.Rows
          If Not IsDBNull(r("adet_data_name")) Then

            If last_detail_type = r("adet_data_name") Then
              Aircraft_Details = Aircraft_Details & "<font class='text_text'> " & r("adet_data_description") & "; </font>"
            Else
              Aircraft_Details = Aircraft_Details & "<font class='small_header_text2'>" & r("adet_data_name") & "</font><font class='text_text2'>: " & r("adet_data_description") & "; </font>"
            End If
          Else
            Aircraft_Details = Aircraft_Details & "<font class='small_header_text2'>" & r("adet_data_name") & "</font><font class='text_text2'>: " & r("adet_data_description") & "; </font>"
          End If
          last_detail_type = r("adet_data_name")

        Next
        Aircraft_Details = Aircraft_Details & "</td></tr>"

      End If

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Aircraft_Details(ByVal detail_type As String, ByVal update_date As String, ByVal done_by As String) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Aircraft_Details(ByVal detail_type As String, ByVal update_date As String, ByVal done_by As String) As String", aclsData_Temp)
    End Try
  End Function
  Public Function Aircraft_Build_PDF_Features(ByVal ac_id As Integer) As String
    Aircraft_Build_PDF_Features = ""
    Dim htmlOutput As String = ""
    Dim temptable As New DataTable

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""

    Try

      If Client = True Then


        Client_Query = "Select Client_Aircraft_Key_Features.cliafeat_flag as afeat_flag, client_key_features.clikfeat_name as kfeat_name from client_aircraft_key_features "
        Client_Query = Client_Query & "inner join client_key_features on Client_Aircraft_Key_Features.cliafeat_type = client_key_features.clikfeat_type "
        Client_Query = Client_Query & " where (client_aircraft_key_features.cliafeat_cliac_id = " & ac_id & ") order by client_aircraft_key_features.cliafeat_seq_nbr"

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()
      ElseIf Jetnet = True Then
        Jetnet_Query = " SELECT kfeat_name, Aircraft_Key_Feature.afeat_status_flag as afeat_flag "
        Jetnet_Query = Jetnet_Query & " FROM Aircraft_Key_Feature with (NOLOCK) "
        Jetnet_Query = Jetnet_Query & " INNER JOIN Key_Feature with (NOLOCK) ON Aircraft_Key_Feature.afeat_feature_code = Key_Feature.kfeat_code"
        Jetnet_Query = Jetnet_Query & " WHERE Aircraft_Key_Feature.afeat_ac_id = " & ac_id
        Jetnet_Query = Jetnet_Query & " AND afeat_journ_id = 0"
        Jetnet_Query = Jetnet_Query & " ORDER BY Aircraft_Key_Feature.afeat_seq_no"

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If



      ' adoRSAircraft.Read()
      If temptable.Rows.Count > 0 Then
        Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "<table><tr><td class='white_feat_header_text' colspan='2'>Key Features</td></tr>"
        For Each q As DataRow In temptable.Rows
          If q("afeat_flag").ToString = "Y" Then
            Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "<tr valign='top'><td class='white_feat_text' valign='top'>"
            Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "&#10003; "
            Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "</td><td class='white_feat_text'>" & q("kfeat_name") & "</td></tr>"
          Else
          End If
        Next
      End If
      Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "</table>"

    Catch ex As Exception
      Response.Write("Error in Aircraft_Build_PDF_Features(ByVal ac_id As Integer) As Str: " & ex.Message)

      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Aircraft_Build_PDF_Features(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally
      adoRSAircraft = Nothing
      MySqlConn.Close()


      SqlReader = Nothing
      SqlConn.Close()
      SqlConn = Nothing

    End Try
  End Function
  Public Function Build_PDF_Airport_Information(ByVal ac_id As Integer) As String
    Build_PDF_Airport_Information = ""
    Dim temptable As New DataTable
    Dim htmlOutput As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing


    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""
    Try

      If Client = True Then

        Client_Query = "SELECT cliaircraft_aport_iata_code as ac_aport_iata_code, cliaircraft_aport_icao_code as ac_aport_icao_code, "
        Client_Query = Client_Query & "cliaircraft_aport_name as ac_aport_name, cliaircraft_aport_city as ac_aport_city, cliaircraft_aport_state "
        Client_Query = Client_Query & "as ac_aport_state, cliaircraft_aport_country as ac_aport_country FROM client_Aircraft INNER JOIN client_Aircraft_Model ON cliaircraft_cliamod_id = cliamod_id"
        Client_Query = Client_Query & " WHERE cliaircraft_id = " & CStr(ac_id)

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()
      ElseIf Jetnet = True Then

        Jetnet_Query = "SELECT ac_aport_iata_code, ac_aport_icao_code, ac_aport_name, ac_aport_city, ac_aport_state, ac_aport_country FROM Aircraft with (NOLOCK) INNER JOIN Aircraft_Model with (NOLOCK) ON ac_amod_id = amod_id"
        Jetnet_Query = Jetnet_Query & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)


        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If

      If temptable.Rows.Count > 0 Then
        Build_PDF_Airport_Information = Build_PDF_Airport_Information & "<br><table align='center'><tr><td align='center' class='white_feat_header_text'>Airport Information</td></tr><tr><td class='white_feat_text'>"
        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_iata_code")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & Trim(temptable.Rows(0).Item("ac_aport_iata_code"))
        End If
        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_icao_code")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & " - " & Trim(temptable.Rows(0).Item("ac_aport_icao_code"))
        End If
        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_name")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & " - " & Trim(temptable.Rows(0).Item("ac_aport_name"))
        End If
        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_city")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & "<br>" & Trim(temptable.Rows(0).Item("ac_aport_city")) & vbCrLf
        End If

        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_state")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & " - " & Trim(temptable.Rows(0).Item("ac_aport_state")) & vbCrLf
        End If
        If Not IsDBNull(temptable.Rows(0).Item("ac_aport_country")) Then
          Build_PDF_Airport_Information = Build_PDF_Airport_Information & " - " & Trim(temptable.Rows(0).Item("ac_aport_country")) & vbCrLf
        End If
        Build_PDF_Airport_Information = Build_PDF_Airport_Information & "</td></tr></table>"
      End If

    Catch ex As Exception
      Response.Write("Error in Build_PDF_Airport_Information: " & ex.Message)
      clsGeneral.clsGeneral.LogError("Error in Build_PDF_Airport_Information: " & ex.Message, aclsData_Temp)
    Finally
      MySqlConn.Close()
      adoRSAircraft = Nothing

      SqlConn.Close()
      SqlReader = Nothing
    End Try
  End Function
  Function DisplayEngineInfo(ByVal ac_id As Integer) As String
    DisplayEngineInfo = ""
    Dim TempTable As New DataTable
    Dim xLoop, nloopCount
    Dim sAircraftType As String = ""
    Dim sAirframeType As String = ""
    Dim htmlOutput As String = ""
    Dim type_of_damage As String = ""
    'mysql
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim MySqlConn2 As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft2 As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft2 = Nothing
    Dim MySqlException2 As MySql.Data.MySqlClient.MySqlException : MySqlException2 = Nothing
2:
    'sql

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing



    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""
    Try
      If Client = True Then
        Client_Query = "select cliacep_engine_name as acep_engine_name, cliacep_engine_maintenance_program as acep_engine_maintenance_program, "
        Client_Query = Client_Query & " cliacep_engine_1_ttsn_hours as acep_engine_1_ttsn_hours, cliacep_engine_2_ttsn_hours as "
        Client_Query = Client_Query & "acep_engine_2_ttsn_hours, cliacep_engine_3_ttsn_hours as acep_engine_3_ttsn_hours, cliacep_engine_4_ttsn_hours as acep_engine_4_ttsn_hours, "
        Client_Query = Client_Query & " cliacep_engine_1_tsoh_hours as acep_engine_1_tsoh_hours, cliacep_engine_2_tsoh_hours as "
        Client_Query = Client_Query & "acep_engine_2_tsoh_hours,cliacep_engine_3_tsoh_hours as acep_engine_3_tsoh_hours, cliacep_engine_4_tsoh_hours as acep_engine_4_tsoh_hours, "
        Client_Query = Client_Query & " cliacep_engine_1_tshi_hours as acep_engine_1_tshi_hours, cliacep_engine_2_tshi_hours as "
        Client_Query = Client_Query & "acep_engine_2_tshi_hours,cliacep_engine_3_tshi_hours as acep_engine_3_tshi_hours, cliacep_engine_4_tshi_hours as acep_engine_4_tshi_hours, "
        Client_Query = Client_Query & " cliacep_engine_1_tbo_hours as acep_engine_1_tbo_hours, cliacep_engine_2_tbo_hours as "
        Client_Query = Client_Query & "acep_engine_2_tbo_hours, cliacep_engine_3_tbo_hours as acep_engine_3_tbo_hours, cliacep_engine_4_tbo_hours as acep_engine_4_tbo_hours, "
        Client_Query = Client_Query & " cliacep_engine_1_tsn_cycle as acep_engine_1_tsn_cycle, cliacep_engine_2_tsn_cycle as "
        Client_Query = Client_Query & "acep_engine_2_tsn_cycle, cliacep_engine_3_tsn_cycle as acep_engine_3_tsn_cycle,cliacep_engine_4_tsn_cycle as acep_engine_4_tsn_cycle, "
        Client_Query = Client_Query & " cliacep_engine_1_tsoh_cycle as acep_engine_1_tsoh_cycle, cliacep_engine_2_tsoh_cycle as "
        Client_Query = Client_Query & "acep_engine_2_tsoh_cycle, cliacep_engine_3_tsoh_cycle as acep_engine_3_tsoh_cycle, cliacep_engine_4_tsoh_cycle as acep_engine_4_tsoh_cycle, "
        Client_Query = Client_Query & " cliacep_engine_1_tshi_cycle as acep_engine_1_tshi_cycle, cliacep_engine_2_tshi_cycle as "
        Client_Query = Client_Query & "acep_engine_2_tshi_cycle, cliacep_engine_3_tshi_cycle as acep_engine_3_tshi_cycle,cliacep_engine_4_tshi_cycle as acep_engine_4_tshi_cycle, "
        Client_Query = Client_Query & " cliacep_engine_1_ser_nbr as acep_engine_1_ser_nbr, cliacep_engine_2_ser_nbr as "
        Client_Query = Client_Query & "acep_engine_2_ser_nbr, cliacep_engine_3_ser_nbr as acep_engine_3_ser_nbr,cliacep_engine_4_ser_nbr as acep_engine_4_ser_nbr from client_aircraft "
        Client_Query = Client_Query & "inner join client_aircraft_engine on cliacep_cliac_id = cliaircraft_id "
        Client_Query = Client_Query & " where cliaircraft_id = " & ac_id


        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          TempTable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then
        Jetnet_Query = "SELECT ac_id, ac_engine_name as acep_engine_name, ac_engine_maintenance_prog_EMP as acep_engine_maintenance_program, "
        Jetnet_Query = Jetnet_Query & " ac_engine_management_prog_EMGP as acep_engine_management_program, "
        Jetnet_Query = Jetnet_Query & " ac_engine_1_ser_no as acep_engine_1_ser_nbr, ac_engine_2_ser_no as acep_engine_2_ser_nbr, ac_engine_3_ser_no as acep_engine_3_ser_nbr, "
        Jetnet_Query = Jetnet_Query & " ac_engine_4_ser_no as acep_engine_4_ser_nbr , ac_engine_1_tot_hrs as acep_engine_1_ttsn_hours, ac_engine_2_tot_hrs as acep_engine_2_ttsn_hours, ac_engine_3_tot_hrs as acep_engine_3_ttsn_hours, "
        Jetnet_Query = Jetnet_Query & "  ac_engine_4_tot_hrs as acep_engine_4_ttsn_hours, ac_engine_1_soh_hrs as acep_engine_1_tsoh_hours, ac_engine_2_soh_hrs as acep_engine_2_tsoh_hours, ac_engine_3_soh_hrs as acep_engine_3_tsoh_hours, "
        Jetnet_Query = Jetnet_Query & " ac_engine_4_soh_hrs as acep_engine_4_tsoh_hours, ac_engine_1_shi_hrs as acep_engine_1_tshi_hours, ac_engine_2_shi_hrs as acep_engine_2_tshi_hours, ac_engine_3_shi_hrs as acep_engine_3_tshi_hours, "
        Jetnet_Query = Jetnet_Query & " ac_engine_4_shi_hrs as acep_engine_4_tshi_hours, ac_engine_1_tbo_hrs as acep_engine_1_tbo_hours, ac_engine_2_tbo_hrs as acep_engine_2_tbo_hours, ac_engine_3_tbo_hrs as acep_engine_3_tbo_hours, "
        Jetnet_Query = Jetnet_Query & " ac_engine_4_tbo_hrs as acep_engine_4_tbo_hours, ac_engine_1_snew_cycles as acep_engine_1_tsn_cycle, ac_engine_2_snew_cycles as acep_engine_2_tsn_cycle, ac_engine_3_snew_cycles as acep_engine_3_tsn_cycle, "
        Jetnet_Query = Jetnet_Query & "  ac_engine_4_snew_cycles as acep_engine_4_tsn_cycle, ac_engine_1_soh_cycles as acep_engine_1_tsoh_cycle, ac_engine_2_soh_cycles as acep_engine_2_tsoh_cycle, ac_engine_3_soh_cycles as acep_engine_3_tsoh_cycle, "
        Jetnet_Query = Jetnet_Query & " ac_engine_4_soh_cycles as acep_engine_4_tsoh_cycle,ac_engine_1_shs_cycles as acep_engine_1_tshi_cycle,ac_engine_2_shs_cycles as acep_engine_2_tshi_cycle,ac_engine_3_shs_cycles as acep_engine_3_tshi_cycle, "
        Jetnet_Query = Jetnet_Query & "  ac_engine_4_shs_cycles as acep_engine_4_tshi_cycle, '' as emp_provider_name, "
        Jetnet_Query = Jetnet_Query & "  '' as emp_program_name, '' as emgp_provider_name, "
        Jetnet_Query = Jetnet_Query & "  '' as emgp_program_name"
        Jetnet_Query = Jetnet_Query & " FROM ((Aircraft with (NOLOCK) INNER JOIN"
        Jetnet_Query = Jetnet_Query & "  Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id) INNER JOIN"
        Jetnet_Query = Jetnet_Query & "  Engine_Management_Program WITH(NOLOCK) ON ac_engine_management_prog_EMGP = Engine_Management_Program.emgp_id) "
        Jetnet_Query = Jetnet_Query & " WHERE   ac_id = " & ac_id
        Jetnet_Query = Jetnet_Query & " and ac_journ_id = 0 ORDER BY   ac_id"

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try
        SqlReader.Close()
      End If


      If TempTable.Rows.Count > 0 Then
        DisplayEngineInfo = ""
        nloopCount = 0
        xLoop = 0

        DisplayEngineInfo = DisplayEngineInfo & "<tr><td height='5'></td></tr>"

        DisplayEngineInfo = DisplayEngineInfo & "<tr><td colspan='2' class='header_text'>Engine Information</td></tr>"

        DisplayEngineInfo = DisplayEngineInfo & "<tr><td width='2%'>&nbsp;</td><td valign='middle'  colspan='2' nowrap><font class='small_header_text'>Engine&nbsp;Maintenance&nbsp;Program:</font> "

        Client_Query = "SELECT emp_provider_name, emp_program_name FROM Engine_Maintenance_Program "
        Client_Query = Client_Query & " WHERE emp_id = " & TempTable.Rows(0).Item("acep_engine_maintenance_program")

        MySqlConn2.ConnectionString = Client_DB
        MySqlCommand2.CommandText = Client_Query
        MySqlConn2.Open()
        MySqlCommand2.Connection = MySqlConn2
        MySqlCommand2.CommandType = CommandType.Text
        MySqlCommand2.CommandTimeout = 60

        adoRSAircraft2 = MySqlCommand2.ExecuteReader(CommandBehavior.CloseConnection)
        adoRSAircraft2.Read()

        If adoRSAircraft2.HasRows Then
          DisplayEngineInfo = DisplayEngineInfo & "<font class='text_text'>"
          DisplayEngineInfo = DisplayEngineInfo & Trim(adoRSAircraft2("emp_provider_name")) & "&nbsp;-&nbsp;" & adoRSAircraft2("emp_program_name") & "&nbsp;</font>"
          adoRSAircraft2.Close()
        End If

        adoRSAircraft2 = Nothing
        MySqlConn2.Close()

        DisplayEngineInfo = DisplayEngineInfo & "</td></tr>"


        If Me.WD.SelectedValue.ToString = "Word" Then
          DisplayEngineInfo = DisplayEngineInfo & "<tr><td colspan='2'><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'>"
          DisplayEngineInfo = DisplayEngineInfo & "<tr><th class='text_text' align='center'><font size='-2'>&nbsp;</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Serial Number</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Time Since New  Hours</font></td>" ' (TTSNEW)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Since Ovh Hours</font></td>" ' (SOH/SCOR)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text'align='center'><font size='-2'>Since Hot Inspect Hours</font></td>" '(SHI/SMPI)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Time Between Ovh Hours</font></td>" '(TBO/TBCI)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since New</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Ovh</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Hot</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "</tr>" & vbCrLf
        Else

          DisplayEngineInfo = DisplayEngineInfo & "<tr><td colspan='2'><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'>"
          DisplayEngineInfo = DisplayEngineInfo & "<tr><td class=Normal>&nbsp;</td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Serial Number</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Time Since New  Hours</font></td>" ' (TTSNEW)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Since Overhaul  Hours</font></td>" ' (SOH/SCOR)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text'align='center'><font size='-2'>Since Hot Inspection  Hours</font></td>" '(SHI/SMPI)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Time Between Overhaul  Hours</font></td>" '(TBO/TBCI)
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since New</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Overhaul</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Hot</font></td>"
          DisplayEngineInfo = DisplayEngineInfo & "</tr>" & vbCrLf
        End If

        If sAirframeType <> "R" Then
          nloopCount = 4
        Else
          nloopCount = 3
        End If

        For xLoop = 1 To nloopCount

          If (Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_ttsn_hours")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_hours")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_hours")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tbo_hours")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsn_cycle")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_cycle")) Or Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_cycle"))) Then

            If xLoop = 1 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 2 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            ElseIf xLoop = 3 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 4 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;</td>"
            End If

            DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_ser_nbr") & "</td>"


            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_ttsn_hours")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_ttsn_hours")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_hours")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_hours")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_hours")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_hours")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tbo_hours")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tbo_hours")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsn_cycle")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsn_cycle")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_cycle")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tsoh_cycle")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_cycle")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(TempTable.Rows(0).Item("acep_engine_" & CStr(xLoop) & "_tshi_cycle")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            DisplayEngineInfo = DisplayEngineInfo & "</tr>" & vbCrLf

          End If

        Next ' xLoop

        DisplayEngineInfo = DisplayEngineInfo & "</table>" & vbCrLf
        DisplayEngineInfo = DisplayEngineInfo & "</td></tr>"

      End If

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in DisplayEngineInfo(ByVal ac_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in DisplayEngineInfo(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally


      adoRSAircraft = Nothing
      adoRSAircraft2 = Nothing
      SqlReader = Nothing
      MySqlConn.Close()
      SqlConn.Close()
      MySqlConn2.Close()
    End Try
  End Function
  Public Function Aircraft_APU(ByVal ac_id As Integer) As String
    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Aircraft_APU = ""
    Dim temptable As New DataTable

    Dim ac_apu_model_name As String = ""
    Dim ac_apu_tot_hrs As Integer
    Dim ac_apu_ser_no As String = ""
    Dim htmlOutput As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""

    Try

      If Client = True Then
        Client_Query = "SELECT cliaircraft_apu_model_name as ac_apu_model_name, cliaircraft_apu_ttsn_hours as ac_apu_ttsn_hours, "
        Client_Query = Client_Query & "cliaircraft_apu_ser_nbr as ac_apu_ser_nbr, cliaircraft_apu_tsoh_hours as ac_apu_tsoh_hours, "
        Client_Query = Client_Query & "cliaircraft_apu_tshi_hours as ac_apu_tshi_hours FROM client_Aircraft INNER JOIN "
        Client_Query = Client_Query & "client_Aircraft_Model ON cliaircraft_cliamod_id = cliamod_id"
        Client_Query = Client_Query & " WHERE cliaircraft_id = " & ac_id


        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then
        Jetnet_Query = "SELECT aircraft.ac_apu_ser_no as ac_apu_ser_nbr, ac_apu_tot_hrs as ac_apu_ttsn_hours, ac_apu_soh_hrs as ac_apu_tsoh_hours, aircraft.ac_apu_model_name, "
        Jetnet_Query = Jetnet_Query & " ac_apu_shi_hrs as ac_apu_tshi_hours, ac_apu_maint_prog as ac_apu_maintance_program FROM Aircraft with (NOLOCK) INNER JOIN Aircraft_Model with (NOLOCK) ON ac_amod_id = amod_id"
        Jetnet_Query = Jetnet_Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If


      If temptable.Rows.Count > 0 Then
        If Not IsDBNull(temptable.Rows(0).Item("ac_apu_model_name")) Then
          ac_apu_model_name = temptable.Rows(0).Item("ac_apu_model_name")
        Else
          ac_apu_model_name = ""
        End If

        If Not IsDBNull(temptable.Rows(0).Item("ac_apu_ttsn_hours")) Then
          ac_apu_tot_hrs = temptable.Rows(0).Item("ac_apu_ttsn_hours")
        Else
          ac_apu_tot_hrs = 0
        End If

        If Not IsDBNull(temptable.Rows(0).Item("ac_apu_ser_nbr")) Then
          ac_apu_ser_no = temptable.Rows(0).Item("ac_apu_ser_nbr")
        Else
          ac_apu_ser_no = ""
        End If


        Aircraft_APU = Aircraft_APU & "<tr><td height='5'></td></tr>"

        If ac_apu_tot_hrs > 0 Or ac_apu_ser_no <> "" Then
          Aircraft_APU = Aircraft_APU & "<tr><td colspan='2' class='header_text'>Auxiliary Power Unit (APU)</td></tr>"

          If ac_apu_model_name.Trim <> "" Then
            Aircraft_APU = Aircraft_APU & "<tr><td width='2%'>&nbsp;</td>"
            Aircraft_APU = Aircraft_APU & "<td><font class='small_header_text'>Model: </font><font class='text_text'>" & ac_apu_model_name
          End If

          If ac_apu_ser_no <> "" Then
            If ac_apu_model_name.Trim = "" Then
              Aircraft_APU = Aircraft_APU & "<tr><td width='2%'>&nbsp;</td>"
              Aircraft_APU = Aircraft_APU & "<td nowrap>"
            Else
              Aircraft_APU = Aircraft_APU & ", "
            End If
            Aircraft_APU = Aircraft_APU & "Serial #:&nbsp;"
            Aircraft_APU = Aircraft_APU & "</font><font class='text_text'>" & temptable.Rows(0).Item("ac_apu_ser_nbr") & "</td></tr>"
          End If

          If ac_apu_model_name.Trim <> "" Then
            If ac_apu_tot_hrs > 0 Then
              Aircraft_APU = Aircraft_APU & "<tr><td width='2%'>&nbsp;</td>"
              Aircraft_APU = Aircraft_APU & "<td nowrap><font class='small_header_text'>Total Time (Hours) Since New: </font><font class='text_text'>" & FormatNumber(CDbl(ac_apu_tot_hrs), 0, True, False, True) & "</td></tr>"
            End If
          End If
        End If

        If Not IsDBNull(temptable.Rows(0).Item("ac_apu_tsoh_hours")) Then
          Aircraft_APU = Aircraft_APU & "<tr><td width='2%'>&nbsp;</td>"
          Aircraft_APU = Aircraft_APU & "<td nowrap><font class='small_header_text'>Since Overhaul (SOH) Hours:&nbsp;"
          Aircraft_APU = Aircraft_APU & "</font><font class='text_text'>" & FormatNumber(CDbl(temptable.Rows(0).Item("ac_apu_tsoh_hours")), 0, True, False, True) & "&nbsp;</td></tr>"
        End If

        Aircraft_APU = Aircraft_APU & "</tr>" & vbCrLf
        If Not IsDBNull(temptable.Rows(0).Item("ac_apu_tshi_hours")) Then
          Aircraft_APU = Aircraft_APU & "<tr><td width='2%'>&nbsp;</td>"
          Aircraft_APU = Aircraft_APU & "<td nowrap><font class='small_header_text'>Since Hot Inspection (SHI) Hours:&nbsp;"


          Aircraft_APU = Aircraft_APU & "</font><font class='text_text'>" & FormatNumber(CDbl(temptable.Rows(0).Item("ac_apu_tshi_hours")), 0, True, False, True) & "&nbsp;</td>"
          Aircraft_APU = Aircraft_APU & "</tr>" & vbCrLf
        Else
        End If
      End If

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Aircraft_APU(ByVal ac_id As Integer) As String")

      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Aircraft_APU(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally


      adoRSAircraft = Nothing
      MySqlConn.Close()

      SqlReader = Nothing
      SqlConn.Close()

    End Try
  End Function
  Public Function Aircraft_Avionics(ByVal ac_id As Integer) As String
    Aircraft_Avionics = ""
    Dim temptable As New DataTable
    Dim htmlOutput As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim Jetnet_Query As String = ""
    Dim Client_Query As String = ""
    Try
      Aircraft_Avionics = ""
      ' Start Avionics    

      If Client = True Then
        Client_Query = "SELECT cliav_name as av_name, cliav_description as av_description FROM client_Aircraft_Avionics WHERE cliav_cliac_id = " & ac_id & " " '  AND av_name  IN ('Avioncs Package', 'FMS', 'GPS', 'TAWS', 'TCAS', 'SATCOM', 'EFIS', 'CVR', 'FDR')"

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60

        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          temptable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        adoRSAircraft.Close()

      ElseIf Jetnet = True Then
        Jetnet_Query = "SELECT * FROM Aircraft_Avionics with (NOLOCK) WHERE av_ac_id = " & ac_id & " and av_ac_journ_id = 0 " '  AND av_name  IN ('Avioncs Package', 'FMS', 'GPS', 'TAWS', 'TCAS', 'SATCOM', 'EFIS', 'CVR', 'FDR')"

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
        SqlReader.Close()
      End If

      If temptable.Rows.Count > 0 Then
        Aircraft_Avionics = Aircraft_Avionics & "<tr><td height='5'></td></tr>"
        Aircraft_Avionics = Aircraft_Avionics & "<tr><td colspan='2' class='header_text'>Avionics</td></tr>"
        Aircraft_Avionics = Aircraft_Avionics & "<tr><td with='2%'>&nbsp;</td><td>"

        For Each q As DataRow In temptable.Rows
          Aircraft_Avionics = Aircraft_Avionics & "<font class='small_header_text2'>" & q("av_name") & ": </font><font class='text_text'>" & q("av_description") & "; </font>"
        Next
        Aircraft_Avionics = Aircraft_Avionics & "</td></tr>"
      End If

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Aircraft_Avionics(ByVal ac_id As Integer) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Aircraft_Avionics(ByVal ac_id As Integer) As String", aclsData_Temp)
    Finally

      adoRSAircraft = Nothing
      MySqlConn.Close()
      SqlReader = Nothing
      SqlConn.Close()

    End Try
  End Function

  Public Function Insert_Page_Break() As String
    Insert_Page_Break = ""
    Try

      If Me.WD.SelectedValue = "Word" Then
        Insert_Page_Break = "<br style=""page-break-before: always"">"
      Else
        Insert_Page_Break = "</td></tr></table></td></tr></table></td></tr></table></td></tr></table>"
        Insert_Page_Break &= "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
      End If


    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Insert_Page_Break() As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Insert_Page_Break() As String", aclsData_Temp)
    End Try
  End Function
  Public Function Build_PDF_Header(ByVal Title As String, ByVal address_info As String) As String
    Dim tempTable As New DataTable
    Build_PDF_Header = ""
    Dim company_name As String = ""
    Dim htmlOutput As String = ""

    company_name = ""

    If Me.WD.SelectedValue = "Word" Then
      Build_PDF_Header = Build_PDF_Header & "<table cellspacing='0' cellpadding='0' width='750'><tr bgcolor='#736F6E'><td colspan='3'  cellpadding='0' cellspacing='0'>"
      Build_PDF_Header = Build_PDF_Header & "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50'><tr><td>"
      Build_PDF_Header = Build_PDF_Header & "<table width='750'><tr><td width='700' valign='top' class='white_feat_header_text'><font color='white' size='-1'>"
    Else
      Build_PDF_Header = Build_PDF_Header & "<table cellspacing='0' cellpadding='0' width='100%'><tr bgcolor='#736F6E'><td colspan='3'  cellpadding='0' cellspacing='0'>"
      Build_PDF_Header = Build_PDF_Header & "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr><td>"
      Build_PDF_Header = Build_PDF_Header & "<table width='100%'><tr><td width='650'><font color='white' size='+1'>"
    End If


    Build_PDF_Header = Build_PDF_Header & company_name & "</font><br>"
    If address_info <> "" Then
      Build_PDF_Header = Build_PDF_Header & "<table>" & address_info
      Build_PDF_Header = Build_PDF_Header & "</table>"
    End If

    If Me.WD.SelectedValue = "Word" Then
      Build_PDF_Header = Build_PDF_Header & "</td><td width='40%' cellpadding='5' valign='top' class='white_feat_header_text'><font color='white' size='-1'>"
    Else
      Build_PDF_Header = Build_PDF_Header & "</td><td width='40%' cellpadding='5' valign='top'><font color='white'>"
    End If



    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim adoRSAircraft As MySql.Data.MySqlClient.MySqlDataReader : adoRSAircraft = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing


    Try
      If Client = True Then

        Dim Client_Query As String : Client_Query = ""


        Client_Query = "Select cliamod_make_name as amod_make_name, cliamod_model_name as amod_model_name, cliamod_id as amod_id, cliaircraft_ser_nbr as ac_ser_nbr from client_aircraft inner join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id"
        Client_Query = Client_Query & " WHERE cliaircraft_id = " & ac_id

        MySqlConn.ConnectionString = Client_DB
        MySqlCommand.CommandText = Client_Query

        MySqlConn.Open()
        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 60
        adoRSAircraft = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Try
          tempTable.Load(adoRSAircraft)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
        End Try


      ElseIf Jetnet = True Then
        Dim Jetnet_Query As String : Jetnet_Query = ""
        Jetnet_Query = "SELECT amod_make_name, amod_model_name, amod_id, ac_ser_no as ac_ser_nbr FROM Aircraft with (NOLOCK) INNER JOIN Aircraft_Model with (NOLOCK) ON ac_amod_id = amod_id"
        Jetnet_Query = Jetnet_Query & " WHERE ac_id = " & ac_id & " and ac_journ_id = 0 "

        SqlConn.ConnectionString = Ref_DB

        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        SqlCommand.CommandText = Jetnet_Query
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          tempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
        End Try

      End If

      If tempTable.Rows.Count > 0 Then
        For Each r As DataRow In tempTable.Rows
          If Not IsDBNull(r("amod_model_name")) Then
            Build_PDF_Header = Build_PDF_Header & r("amod_make_name") & " "
            current_make_model = r("amod_make_name")
          End If

          If Not IsDBNull(r("amod_model_name")) Then
            Build_PDF_Header = Build_PDF_Header & r("amod_model_name")
            current_make_model = current_make_model & " " & r("amod_model_name")
          End If


          If Not IsDBNull(r("amod_id")) Then
            current_model_id = r("amod_id")
          End If



          If Not BR.Checked Then
            If Not IsDBNull(r("ac_ser_nbr")) Then
              Build_PDF_Header = Build_PDF_Header & " SN # " & r("ac_ser_nbr").ToString & "</font>"
              current_ac_name = current_make_model & " "
              current_ac_name &= ", SN # " & r("ac_ser_nbr").ToString
            End If
          End If


        Next
      End If


      Build_PDF_Header = Build_PDF_Header & "</font>"

      Build_PDF_Header = Build_PDF_Header & "<table align='left' valign='bottom'>"
      If address_info <> "" Then
        Build_PDF_Header = Build_PDF_Header & "<tr><td align='center'>&nbsp;</td></tr>"
      End If
      If Me.WD.SelectedValue = "Word" Then
        Build_PDF_Header = Build_PDF_Header & "<tr><td align='center' class='white_feat_header_text'><font color='white' size='-1'><i>" & Title & "</i></font></td></tr></table>"
      Else
        Build_PDF_Header = Build_PDF_Header & "<tr><td align='center'><font color='white'><i>" & Title & "</i></font></td></tr></table>"
      End If

      If Me.WD.SelectedValue = "Word" Then
        Build_PDF_Header = Build_PDF_Header & "</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='680' valign='top'  height='500' cellpadding='0' cellspacing='0'><table valign='top'  cellpadding='0' cellspacing='0'>"
      Else
        Build_PDF_Header = Build_PDF_Header & "</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='60%' height='900' valign='top'  cellpadding='0' cellspacing='0'><table valign='top'  cellpadding='0' cellspacing='0'>"
      End If


    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Header(ByVal Title As String, ByVal address_info As String) As String")
      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Header(ByVal Title As String, ByVal address_info As String) As String", aclsData_Temp)
    Finally
      If Client = True Then
        adoRSAircraft.Close()
        adoRSAircraft = Nothing
        'serial number
        MySqlConn.Close()
      ElseIf Jetnet = True Then
        SqlReader.Close()
        SqlReader = Nothing
        SqlConn.Close()
      End If
    End Try
  End Function
  Public Function Build_HTML_Page(ByVal viewToPDF As String) As String

    Return viewToPDF & "</body></html>"

  End Function
  Public Function Build_PDF_Format() As String
    Build_PDF_Format = ""
    Try
      Build_PDF_Format = Build_PDF_Format & "</table>"
      Build_PDF_Format = Build_PDF_Format & "</td><td>&nbsp;&nbsp;&nbsp;"

      Build_PDF_Format = Build_PDF_Format & "</td><td width='250' height='500' valign='top'>"
      Build_PDF_Format = Build_PDF_Format & "<table bgcolor='#A4A4A4' height='850' width='250' valign='top'><tr height='850' valign='top'><td width='250' height='850' align='center'>"

    Catch ex As Exception
      Response.Write("Error in Build_PDF_Format: " & ex.Message)
    End Try
  End Function
  Public Function End_Page() As String
    End_Page = ""
    Try
      End_Page = End_Page & "</td></tr></table>" ' this is for the end of right column
      End_Page = End_Page & "</td></tr></table>" ' this is for the end of the entire apge
    Catch ex As Exception
      Response.Write("Error in End_Page: " & ex.Message)
    End Try
  End Function
  Public Function Build_PDF_Template_Header() As String

    Dim readStyle As String = ""
    Dim formatStyle As String = ""
    readStyle = "<style>"

    Try

      readStyle = readStyle & "body {font-size:12px;} " & vbCrLf
      If Me.WD.SelectedValue.ToString = "Word" Then
        readStyle = readStyle & ".header_text{font-family:Arial ;font-size: x-small; color: #736F6E; font-weight: bold;}" & vbCrLf
        readStyle = readStyle & ".small_header_text{font-family:Arial ;font-size: xx-small; font-style: italic; color: #736F6E} " & vbCrLf
        readStyle = readStyle & ".small_header_text2{font-family:Arial ;font-size: xx-small; font-style: italic; color: #736F6E; font-weight: bold;} " & vbCrLf
        readStyle = readStyle & ".text_text{font-family:Arial;font-size: xx-small; color: #736F6E;padding:8px;}" & vbCrLf
        readStyle = readStyle & ".text_text2{font-family:Arial;font-size: xx-small; color: #736F6E}" & vbCrLf
        readStyle = readStyle & ".white_feat_text{font-family:Arial;font-size: x-small; color: white}" & vbCrLf
        readStyle = readStyle & ".white_feat_header_text{font-family:Arial;font-size: small; color: white; font-weight: bold;}" & vbCrLf
      Else
        readStyle = readStyle & ".header_text{font-family:Arial ;font-size: medium; color: #736F6E; font-weight: bold;}" & vbCrLf
        readStyle = readStyle & ".small_header_text{font-family:Arial ;font-size: smaller; font-style: italic; color: #736F6E} " & vbCrLf
        readStyle = readStyle & ".small_header_text2{font-family:Arial ;font-size: x-small; font-style: italic; color: #736F6E; font-weight: bold;} " & vbCrLf
        readStyle = readStyle & ".text_text{font-family:Arial;font-size: small; color: #736F6E}" & vbCrLf
        readStyle = readStyle & ".text_text2{font-family:Arial;font-size: x-small; color: #736F6E}" & vbCrLf
        readStyle = readStyle & ".white_feat_text{font-family:Arial;font-size: medium; color: white}" & vbCrLf
        readStyle = readStyle & ".white_feat_header_text{font-family:Arial;font-size: large; color: white; font-weight: bold;}" & vbCrLf
      End If
      readStyle = readStyle & ".break { page-break-before: always; }" & vbCrLf
      readStyle = readStyle & ".table_specs{font-size:12px;}" & vbCrLf
      readStyle = readStyle & "</style>" & vbCrLf

    Catch ex As Exception
      Response.Write("Error " & ex.Message & " in Build_PDF_Template_Header() As String")

      clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Build_PDF_Template_Header() As String", aclsData_Temp)
    End Try

    Return "<html><head>" & vbCrLf & readStyle & "</head><body>" & vbCrLf

  End Function

  Public Function check_custom_fields(ByVal ac_id As Long) As String
    check_custom_fields = ""

    Dim Query As String = ""
    Dim results_table As New DataTable
    Dim temp_name As String = ""
    Dim temp_val As String = ""
    Dim found_spot As Boolean = False
    Dim insert_string_start As String = ""
    Dim insert_string As String = ""
    Dim i As Integer = 0

    Try

      Query = " SELECT * "
      Query &= " from client_preference "
      Query &= " where clipref_ac_custom_1_use = 'Y' "

      HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = Application.Item("crmClientDatabase")


      results_table = localDatalayer.Get_Compare_Query(Query, "find_custom_fields")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then


          For Each r As DataRow In results_table.Rows

            For i = 1 To 10
              If Not IsDBNull(r("clipref_ac_custom_" & i & "_use")) Then
                If Trim(r("clipref_ac_custom_" & i & "_use")) = "Y" Then
                  If Not IsDBNull(r("clipref_ac_custom_" & i & "")) Then
                    If Trim(r("clipref_ac_custom_" & i & "")) <> "" Then
                      check_custom_fields &= get_fields_info_from_spot(i, ac_id)
                    End If
                  End If
                End If
              End If
            Next

          Next
        End If
      End If


    Catch ex As Exception
    Finally

    End Try
  End Function

  Public Function get_fields_info_from_spot(ByVal field_spot As Integer, ByVal ac_id As Long) As String
    get_fields_info_from_spot = ""

    Dim results_table As New DataTable
    Dim temp_name As String = ""
    Dim temp_val As String = ""
    Dim Query As String = ""


    Query = " SELECT clivalch_name, cliaircraft_custom_" & field_spot & " "
    Query &= " from client_aircraft "
    Query &= " inner join client_value_field_choice on clivalch_db_name = 'cliaircraft_custom_" & field_spot & "'"
    Query &= " where cliaircraft_id = " & ac_id & " "

    results_table = localDatalayer.Get_Compare_Query(Query, "get_fields_info_from_spot")

    If Not IsNothing(results_table) Then

      If results_table.Rows.Count > 0 Then
        For Each r As DataRow In results_table.Rows
          If Not IsDBNull(r("cliaircraft_custom_" & field_spot & "")) Then
            If Trim(r("cliaircraft_custom_" & field_spot & "")) <> "" Then
              get_fields_info_from_spot &= "<tr><td height='5'></td></tr>"
              get_fields_info_from_spot &= "<tr><td colspan='2' class='header_text'>" & r("clivalch_name") & "</td></tr>"
              get_fields_info_from_spot &= "<tr><td with='2%'>&nbsp;</td><td>"
              get_fields_info_from_spot &= "<font class='text_text'>"
              get_fields_info_from_spot &= r("cliaircraft_custom_" & field_spot & "")
              get_fields_info_from_spot &= "</font></td></tr>"
            End If
          End If
        Next
      End If
    End If


  End Function

  Public Function get_ac_image(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String
    get_ac_image = ""

    Dim imgFolder As String = HttpContext.Current.Server.MapPath(Session.Item("AircraftPicturesFolderVirtualPath"))
    Dim imgDisplayFolder As String = Application.Item("crmClientSiteData").ClientFullHostName & Session.Item("AircraftPicturesFolderVirtualPath")

    Dim imgFileName As String = ""
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim adoTempRS2 As SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim htmlOutput As String = ""
    Dim pic_seq_num As Integer = 0
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Long = 0
    Dim fApicSubject As String = ""


    Try



      ' start AC_Pic
      Query = "SELECT TOP 1 * FROM Aircraft_Pictures WITH(NOLOCK)"
      Query = Query & " WHERE acpic_ac_id = " & nAircraftID
      Query = Query & " AND acpic_journ_id = " & nAircraftJournalID
      Query = Query & " AND acpic_seq_no > '0'"
      Query = Query & " AND acpic_image_type = 'JPG'"
      Query = Query & " AND acpic_hide_flag = 'N'"
      Query = Query & " ORDER BY acpic_seq_no"

      SqlConn.ConnectionString = Ref_DB

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query
      adoTempRS2 = SqlCommand.ExecuteReader()

      If adoTempRS2.HasRows Then

        adoTempRS2.Read()

        pic_seq_num = CInt(adoTempRS2.Item("acpic_seq_no").ToString)

        If Not IsDBNull(adoTempRS2("acpic_image_type")) Then
          fAcpic_image_type = adoTempRS2.Item("acpic_image_type").ToString.ToLower.Trim
        End If

        If Not IsDBNull(adoTempRS2("acpic_id")) Then
          fAcpic_id = adoTempRS2.Item("acpic_id").ToString.Trim
        End If

        If Not (IsDBNull(adoTempRS2("acpic_subject"))) Then
          fApicSubject = adoTempRS2.Item("acpic_subject").ToString.Trim
        End If

        imgFileName = nAircraftID.ToString & crmWebClient.Constants.cHyphen & nAircraftJournalID.ToString & crmWebClient.Constants.cHyphen & fAcpic_id.ToString & crmWebClient.Constants.cDot & fAcpic_image_type.ToLower.Trim

      Else
        imgFileName = ""
      End If

      ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
      ' setup the path for the pictures based on which site is running
      ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
      htmlOutput = htmlOutput & "<tr>"

      If Not String.IsNullOrEmpty(imgFileName) Then
        If Me.WD.SelectedValue.ToString = "Word" Then
          ' If System.IO.File.Exists(imgFolder.Trim & "\" & imgFileName.Trim) Then
          htmlOutput = htmlOutput & "<td width='90%' id='AC_Pic' align='center' valign='top'>"
          htmlOutput = htmlOutput & "<img Title='" & fApicSubject.Trim & "' src='http://www.jetnetevolution.com/pictures/aircraft/" & imgFileName.Trim & "' width='700' /></td>"
          'End If
        Else
          'If System.IO.File.Exists(imgFolder.Trim & "\" & imgFileName.Trim) Then 
          htmlOutput = htmlOutput & "<td width='490' id='AC_Pic' align='center' valign='top'>"
          htmlOutput = htmlOutput & "<img Title='" & fApicSubject.Trim & "' alt='" & fApicSubject.Trim & "' src='" & HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") + "/" + imgFileName.Trim & "' width='490'/></td>"
          'end If
        End If

      Else
        htmlOutput = htmlOutput & "<td><img src='' width='250'/></td>"
      End If

      htmlOutput = htmlOutput & "</tr><tr><td>&nbsp;</td></tr>"

      adoTempRS2.Close()
    Catch ex As Exception
    Finally
      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn = Nothing
    End Try

    get_ac_image = htmlOutput

  End Function

  Public Function build_full_spec_page_header(ByVal nAircraftID As Long, ByVal Title As String, ByVal address_info As String, ByVal image_ref As String, ByRef comp_info As String) As String

    Dim company_name As String = ""

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim font_size_for_address As String = ""
    Dim logo_link As String = ""

    Try

      sQuery.Append("SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_web_address")
      sQuery.Append(" FROM Company")
      sQuery.Append(" INNER JOIN Subscription ON sub_comp_id = comp_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE Subscription.sub_id = " & Session.Item("localSubscription").crmSubscriptionID.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then
        SqlReader.Read()
        company_name = SqlReader.Item("comp_name").ToString.Trim



        If Not IsDBNull(SqlReader.Item("comp_address1")) And Not IsDBNull(SqlReader.Item("comp_address2")) Then
          If (SqlReader.Item("comp_address1").ToString.Length + SqlReader.Item("comp_address2").ToString.Length) > 50 Then
            font_size_for_address = "-2"
          Else
            font_size_for_address = "-1"
          End If
        Else
          font_size_for_address = "-1"
        End If

        If Not IsDBNull(SqlReader.Item("comp_address1")) Then
          If Me.WD.SelectedValue = "Word" Then
            address_info = "<tr valign='top'><td class='white_feat_header_text' align='left'><font color='white' size='" + font_size_for_address + "'>" + SqlReader.Item("comp_address1").ToString
          Else
            address_info = "<tr valign='top'><td class='CompInfo' align='left'><font color='white'>" + SqlReader.Item("comp_address1").ToString + "</font>"
          End If
        Else
          If Me.WD.SelectedValue = "Word" Then
            address_info = "<tr valign='top'><td class='white_feat_header_text' align='left'><font color='white' size='-1'>"
          Else
            address_info = "<tr valign='top'><td class='CompInfo' align='left'>"
          End If
        End If



        'If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
        '    If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
        '        logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
        '        logo_link = "<img src='" + logo_link + "/" + SqlReader.Item("comp_id").ToString + ".jpg' width='150'>"
        '    End If
        'End If

        If Not IsDBNull(SqlReader.Item("comp_address2")) Then
          address_info += "<font color='white'> . " + SqlReader.Item("comp_address2").ToString + "</font>"
        End If

        If font_size_for_address = "-2" Or Not String.IsNullOrEmpty(logo_link) Then
          address_info += "<br />"
        End If

        If Not IsDBNull(SqlReader.Item("comp_city")) Then
          address_info += "<font color='white'> " + SqlReader.Item("comp_city").ToString + "</font>"
        End If

        If Not IsDBNull(SqlReader.Item("comp_state")) Then
          address_info += "<font color='white'>, " + SqlReader.Item("comp_state").ToString + "</font>"
        End If

        If Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
          address_info += "<font color='white'> " + SqlReader.Item("comp_zip_code").ToString + "</font>"
        End If

        address_info += "</td></tr>"

        address_info += build_phone_info_full_spec(CLng(Session.Item("localSubscription").crmSubscriptionID.ToString), "white")

        If Not IsDBNull(SqlReader.Item("comp_web_address")) Then
          address_info += "<tr><td style='color: white;' align='left'> &#8226; "

          If SqlReader.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
            address_info += "<a href='http://" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' style='color: white;'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
          Else
            address_info += "<a href='" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' style='color: white;'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
          End If

          address_info += "</td></tr>"

        Else
          address_info += "<tr><td>&nbsp;</td></tr>"
        End If



        comp_info = "<tr><td class='small_header_text' align='left'>" & company_name & "</td></tr>"
        comp_info &= address_info
        comp_info = Replace(comp_info, "color='white'", "class='small_header_text'")
        comp_info = Replace(comp_info, "class='white_feat_text'", "class='small_header_text'")
        comp_info = Replace(comp_info, "style='color: white;'", "class='small_header_text'")
        comp_info = Replace(comp_info, "white_feat_header_text", "small_header_text")
        comp_info = Replace(comp_info, "white", "black")
        comp_info = Replace(comp_info, "align='left'", "align='center'")
      End If

      SqlReader.Close()

      If Me.WD.SelectedValue = "Word" Then
        sOutString.Append("<table cellspacing='0' cellpadding='0' width='750'><tr bgcolor='#736F6E'><td colspan='3' cellpadding='0' cellspacing='0'>")
        sOutString.Append("<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50'><tr><td>")
        sOutString.Append("<table width='750'><tr>")

        If Not String.IsNullOrEmpty(image_ref.Trim) Then
          sOutString.Append("<td width='150'>" + image_ref.Trim + "</td><td width='400' valign='top' class='white_feat_header_text'><font color='white' size='-1'>")
        Else
          sOutString.Append("<td width='400' valign='top' class='white_feat_header_text'><font color='white' size='-1'>")
        End If

      Else
        sOutString.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr bgcolor='#736F6E'><td colspan='3' cellpadding='0' cellspacing='0'>")
        sOutString.Append("<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50'><tr><td>")
        sOutString.Append("<table width='100%'><tr>")

        If Not String.IsNullOrEmpty(image_ref.Trim) Then
          sOutString.Append("<td width='150'>" + image_ref.Trim + "</td><td width='400'><font color='white' size='+1'>")
        Else
          sOutString.Append("<td width='400'><font color='white' size='+1'>")
        End If

      End If

      sOutString.Append(company_name + "</font><br>")

      If Not String.IsNullOrEmpty(address_info.Trim) Then
        sOutString.Append("<table>" + address_info.Trim)
        sOutString.Append("</table>")
      End If

      If Me.WD.SelectedValue = "Word" Then
        sOutString.Append("</td><td width='300' cellpadding='5' valign='top' class='white_feat_header_text'><font color='white' size='-1'>")
      Else
        sOutString.Append("</td><td width='40%' cellpadding='5' valign='top'><font color='white'>")
      End If

      Dim acInfoArray() As String = Split(commonEvo.GetAircraftInfo(nAircraftID, False), crmWebClient.Constants.cSvrDataSeperator)

      If Not String.IsNullOrEmpty(acInfoArray(0).ToString) Then
        sOutString.Append(acInfoArray(0).ToString & " ")
      End If

      If Not String.IsNullOrEmpty(acInfoArray(1).ToString) Then
        sOutString.Append(acInfoArray(1).ToString)
      End If

      ' If Not chkBlindReport.Checked Then
      If Not String.IsNullOrEmpty(acInfoArray(2).ToString) Then
        sOutString.Append(" SN #" + acInfoArray(2).ToString)
      End If
      '   End If

      sOutString.Append("</font>")

      sOutString.Append("<table align='left' valign='bottom' width='100%'>")

      sOutString.Append("<tr><td align='center'>&nbsp;</td></tr>")

      If Me.WD.SelectedValue = "Word" Then
        sOutString.Append("<tr><td align='center' class='white_feat_header_text'><font color='white' size='-1'><i>" + Title.Trim + "</i></font></td></tr></table>")
      Else
        sOutString.Append("<tr><td align='center'><font color='white'><i>" + Title.Trim + "</i></font></td></tr></table>")
      End If

      If Me.WD.SelectedValue = "Word" Then
        sOutString.Append("</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='400' valign='top' height='500'><table valign='top' cellpadding='0' cellspacing='0'>")
      Else
        sOutString.Append("</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='60%' height='900' valign='top'><table valign='top' cellpadding='0' cellspacing='0'>")
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_page_header " + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return (sOutString.ToString)

  End Function

  Public Function build_phone_info_full_spec(ByVal Sub_ID As Long, ByVal color As String) As String

    Dim sQuery = New StringBuilder()
    Dim sOutString As StringBuilder = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT top 1 pnum_type, pnum_number_full FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 INNER JOIN Phone_Numbers")
      sQuery.Append(" ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id")
      sQuery.Append(" INNER JOIN Phone_Type ON ptype_name = pnum_type ")
      sQuery.Append(" WHERE Subscription.sub_id = " + Sub_ID.ToString)
      sQuery.Append(" AND pnum_journ_id = 0")
      sQuery.Append(" AND pnum_hide_customer = 'N' AND pnum_contact_id = 0 ")
      sQuery.Append(" ORDER BY ptype_seq_no ASC")  ' QUERY EDITED TO DISPLAY TOLL FREE THEN OFFICE THEN FAX 

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        Do While SqlReader.Read
          If color = "white" Then
            If Me.WD.SelectedValue = "Word" Then
              sOutString.Append("<tr><td align='left'><font color='" & color & "' size='-1'>" + SqlReader.Item("pnum_type").ToString + ": </font><font class='white_feat_text' size='-1'>" + SqlReader.Item("pnum_number_full").ToString + "</font></td></tr>")
            Else
              sOutString.Append("<tr><td align='left'><font color='white'>" + SqlReader.Item("pnum_type").ToString + ": </font><font color='white' size='+1'>" + SqlReader.Item("pnum_number_full").ToString + "</font></td></tr>")
            End If
          Else
            sOutString.Append("<tr><td width='2%' align='left'>&nbsp;</td><td><font class='small_header_text'>" + SqlReader.Item("pnum_type").ToString + ": </font><font class='text_text'>" + SqlReader.Item("pnum_number_full").ToString + "</font></td></tr>")
          End If
        Loop

      End If

      SqlReader.Close()

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_phone_info_full_spec " + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sOutString.ToString

  End Function
  Private Sub Build_Market_trends_tab(ByRef out_htmlString As String, ByVal location_string As String)
    Dim htmlUpDown As String = ""
    Dim htmlOut As New StringBuilder


    Dim market_functions As New market_model_functions

    Try

      market_functions.adminConnectStr = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn.ToString.Trim
      market_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      market_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      market_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      market_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      If searchCriteria.ViewCriteriaTimeSpan = 0 Then
        searchCriteria.ViewCriteriaTimeSpan = 6
      End If



      ' set the title and load the data into the chart control 
      ANALYTICS_HISTORY.Titles.Clear()
      ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      ANALYTICS_HISTORY.Titles.Add("Avg Price By Month (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_avg_price_by_month_graph(searchCriteria, Me.ANALYTICS_HISTORY)

      If Trim(Request("noteid")) <> "" Then
        ANALYTICS_HISTORY.Width = 325
        ANALYTICS_HISTORY.Height = 325
      End If
      ANALYTICS_HISTORY.SaveImage(Server.MapPath("TempFiles") + "\" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_PRICE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)



      ANALYTICS_HISTORY.Titles.Clear()
      ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      ANALYTICS_HISTORY.Titles.Add("Sold Per Month (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_sold_per_month_graph(searchCriteria, True, Me.ANALYTICS_HISTORY)

      If Trim(Request("noteid")) <> "" Then
        ANALYTICS_HISTORY.Width = 325
        ANALYTICS_HISTORY.Height = 325
      End If
      ANALYTICS_HISTORY.SaveImage(Server.MapPath("TempFiles") + "\" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_PER_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)




      ANALYTICS_HISTORY.Titles.Clear()
      ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      ANALYTICS_HISTORY.Titles.Add("Avg Days on Market (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_average_days_on_market_graph(searchCriteria, Me.ANALYTICS_HISTORY)

      If Trim(Request("noteid")) <> "" Then
        ANALYTICS_HISTORY.Width = 325
        ANALYTICS_HISTORY.Height = 325
      End If
      ANALYTICS_HISTORY.SaveImage(Server.MapPath("TempFiles") + "\" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_DAYS_ON_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)


      '   htmlOut.Append("<div style='height:370px; width:970px; overflow: auto;'>")


      '   htmlOut.Append(JetnetSourceText)



      htmlOut.Append("<table width='100%' cellpadding='0' cellspacing='0' align='center'>")
      htmlOut.Append("<tr><td align='center' valign='top' width='100%'>")
      htmlOut.Append("<table cellpadding='0' cellspacing='0' align='center' width='100%'>")

      ''Moved this to the trends tab on 4/24/2014
      If Not Session.Item("localPreferences").AerodexFlag Then
        htmlOut.Append("<tr><td align='center' valign='top' colspan='4'><table width='100%' cellpadding='0' cellspacing='0' align='center'><tr><td valign='top' align='center'>")
        market_functions.views_display_market_up_down_one_model(searchCriteria, htmlUpDown, True)
        htmlUpDown = Replace(htmlUpDown, "class='seperator'", "class='small_header_text'")

        htmlUpDown = Replace(htmlUpDown, "images/gain_loss_down.jpg", HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + "/images/gain_loss_down.jpg")
        htmlUpDown = Replace(htmlUpDown, "images/gain_loss_none.jpg", HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + "/images/gain_loss_none.jpg")
        htmlUpDown = Replace(htmlUpDown, "images/gain_loss_up.jpg", HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + "/images/gain_loss_up.jpg")

        htmlOut.Append(htmlUpDown)

        htmlOut.Append("</td></tr></table></td></tr>")
      End If

      htmlOut.Append("<tr><td valign='top' align='center'>") ' need to append user_sub_seq to filename so it gets deleted when user logs in next time

      ''Moved this to the trends tab on 4/24/2014
      If Not Session.Item("localPreferences").AerodexFlag Then
        ANALYTICS_HISTORY.Titles.Clear()
        ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
        ANALYTICS_HISTORY.Titles.Add("For Sale By Month (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)")
        market_functions.views_display_for_sale_by_month_graph(searchCriteria, Me.ANALYTICS_HISTORY)
        If Trim(Request("noteid")) <> "" Then
          ANALYTICS_HISTORY.Width = 325
          ANALYTICS_HISTORY.Height = 325
        End If
        ANALYTICS_HISTORY.SaveImage(Server.MapPath("TempFiles") + "\" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_FORSALE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

        If Trim(Request("noteid")) <> "" Then
          htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_FORSALE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='325' height='325'>")
        Else
          htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_FORSALE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='300' height='300'>")
        End If
      End If

      htmlOut.Append("</td><td valign='top' align='center'>")

      If Trim(Request("noteid")) <> "" Then
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_PRICE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='325' height='325'>")
        htmlOut.Append("</td></tr><tr><td valign='top' align='center'>")
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_PER_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='325' height='325'>")
        htmlOut.Append("</td><td valign='top' align='center'>")
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_DAYS_ON_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='325' height='325'>")
      Else
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_PRICE_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='300' height='300'>")
        htmlOut.Append("</td></tr><tr><td valign='top' align='center'>")
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_PER_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='300' height='300'>")
        htmlOut.Append("</td><td valign='top' align='center'>")
        htmlOut.Append("<img src='http://" & location_string & "/TempFiles/" + Session.Item("localUser").crmUserTemporaryFilePrefix + searchCriteria.ViewCriteriaAmodID.ToString + "_AVG_DAYS_ON_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_MONTHS.jpg' width='300' height='300'>")
      End If


      htmlOut.Append("</td></tr></table>")
      htmlOut.Append("</td></tr></table>")
      '   htmlOut.Append("</div>")


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [Build_Market_trends_tab] : " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString

    htmlOut = Nothing
    market_functions = Nothing

  End Sub
  Public Function get_comp_logo_info(ByVal bWordReport As Boolean)
    get_comp_logo_info = ""

    Dim address_info As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader = Nothing
    Dim SqlException As SqlClient.SqlException = Nothing
    Dim htmlOutput As New StringBuilder
    Dim logo_link As String = ""
    Dim sQuery = New StringBuilder()
    Dim font_size_for_address As String = ""



    Try
      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

      sQuery.Append("SELECT TOP 1 * FROM Company INNER JOIN Subscription ON comp_id = sub_comp_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE sub_id = " + Session.Item("localUser").crmSubSubID.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader.Item("comp_address1")) And Not IsDBNull(SqlReader.Item("comp_address2")) Then
          If (SqlReader.Item("comp_address1").ToString.Length + SqlReader.Item("comp_address2").ToString.Length) > 50 Then
            font_size_for_address = "-2"
          Else
            font_size_for_address = "-1"
          End If
        Else

        End If

        If Not IsDBNull(SqlReader.Item("comp_address1")) Then
          If bWordReport Then
            address_info = "<tr valign='top'><td class='white_feat_header_text'><font color='white' size='" + font_size_for_address + "'>" + SqlReader.Item("comp_address1").ToString
          Else
            address_info = "<tr valign='top'><td class='CompInfo'><font color='white'>" + SqlReader.Item("comp_address1").ToString + "</font>"
          End If
        Else
          If bWordReport Then
            address_info = "<tr valign='top'><td class='white_feat_header_text'><font color='white' size='-1'>"
          Else
            address_info = "<tr valign='top'><td class='CompInfo'>"
          End If

        End If

        If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
          If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
            logo_link = "<img src='" + logo_link + "/" + SqlReader.Item("comp_id").ToString + ".jpg' width='150'>"
          End If
        End If

        If Not IsDBNull(SqlReader.Item("comp_address2")) Then
          address_info += "<font color='white'> . " + SqlReader.Item("comp_address2").ToString + "</font>"
        End If

        If font_size_for_address = "-2" Or Not String.IsNullOrEmpty(logo_link) Then
          address_info += "<br />"
        End If

        If Not IsDBNull(SqlReader.Item("comp_city")) Then
          address_info += "<font color='white'> " + SqlReader.Item("comp_city").ToString + "</font>"
        End If

        If Not IsDBNull(SqlReader.Item("comp_state")) Then
          address_info += "<font color='white'>, " + SqlReader.Item("comp_state").ToString + "</font>"
        End If

        If Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
          address_info += "<font color='white'> " + SqlReader.Item("comp_zip_code").ToString + "</font>"
        End If

        address_info += "</td></tr>"

        address_info += build_phone_info_full_spec(CLng(Session.Item("localUser").crmSubSubID.ToString), "white")

        If Not IsDBNull(SqlReader.Item("comp_web_address")) Then
          address_info += "<tr><td style='color: white;'> &#8226; "

          If SqlReader.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
            address_info += "<a href='http://" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' style='color: white;'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
          Else
            address_info += "<a href='" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' style='color: white;'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
          End If

          address_info += "</td></tr>"

        Else
          address_info += "<tr><td>&nbsp;</td></tr>"
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_coverpage()" + ex.Message
    Finally

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

      SqlReader.Dispose()
    End Try

  End Function

  Public Sub make_value_graphs_just_chart(ByVal past_date As String, ByVal temp_model As String, ByRef chart_html_string As String)
    Dim results_table As New DataTable
    Dim temp_string_value As String = ""
    Dim charting_string As String = ""
    Dim temp_data As String = ""
    Dim temp_header As String = ""
    Dim asking_count As Integer = 0
    Dim asking_total As Double = 0
    Dim take_total As Double = 0
    Dim take_count As Integer = 0
    Dim sold_total As Double = 0
    Dim sold_count As Integer = 0
    Dim last_header As String = ""
    Dim row_Added As Boolean = False
    Dim temp_count As Integer = 0
    Dim i As Integer = 0
    Dim temp_ask As Double = 0
    Dim temp_take As Double = 0
    Dim temp_sold As Double = 0
    Dim row_string As String = ""
    Dim distinct_ser_num_list(100) As String
    Dim ser_num_row_string(100) As String
    Dim date_string(100) As String
    Dim date_count As Integer = 0
    Dim d As Integer = 0
    Dim date_value_array(100) As Double
    Dim avg_asking As Double = 0

    past_date = Year(past_date) & "-" & Month(past_date) & "-" & Day(past_date)

    results_table = crmViewDataLayer.Get_Market_Snapshot_Datatable(past_date, temp_model)



    For Each r As DataRow In results_table.Rows
      row_Added = False
      If Not IsDBNull(r("clival_ser_nbr")) Then

        If temp_count > 0 Then
          For i = 0 To temp_count - 1
            If Trim(distinct_ser_num_list(i)) = Trim(r("clival_ser_nbr")) Then
              row_Added = True
              i = temp_count
            End If
          Next
        End If

        ' then there is no match, or its the first one
        If row_Added = False Then
          distinct_ser_num_list(temp_count) = Trim(r("clival_ser_nbr"))
          temp_count = temp_count + 1
        End If

      End If
    Next



    row_Added = False
    For Each r As DataRow In results_table.Rows

      temp_header = ""
      temp_data = ""
      temp_ask = 0
      temp_take = 0
      temp_sold = 0

      If Not IsDBNull(r("tdate")) Then
        temp_header = r("tdate")
      End If

      ' then its changed dates
      If Trim(temp_header) <> Trim(last_header) And Trim(last_header) <> "" Then

        date_string(date_count) = last_header
        date_count = date_count + 1

        avg_asking = Replace(FormatNumber(CDbl((asking_total / asking_count) / 1000), 0), ",", "")


        take_total = 0
        take_count = 0
        sold_count = 0
        sold_total = 0
        asking_count = 0
        asking_total = 0
        row_Added = True
      End If

      If Not IsDBNull(r("clival_asking_price")) Then
        If CDbl(r("clival_asking_price")) > 0 Then
          asking_total = asking_total + CDbl(r("clival_asking_price"))
          asking_count = asking_count + 1
          temp_ask = CDbl(r("clival_asking_price"))
        End If
      End If

      If Not IsDBNull(r("clival_est_price")) Then
        If CDbl(r("clival_est_price")) > 0 Then
          take_total = take_total + CDbl(r("clival_est_price"))
          take_count = take_count + 1
          temp_take = CDbl(r("clival_est_price"))
        End If
      End If


      If Not IsDBNull(r("clival_broker_price")) Then
        If CDbl(r("clival_broker_price")) > 0 Then
          sold_total = sold_total + CDbl(r("clival_broker_price"))
          sold_count = sold_count + 1
          temp_sold = CDbl(r("clival_broker_price"))
        End If
      End If

      last_header = Trim(temp_header)
    Next


    date_string(date_count) = last_header
    date_count = date_count + 1

    avg_asking = Replace(FormatNumber(CDbl((asking_total / asking_count) / 1000), 0), ",", "")




    row_string = row_string & "<table cellspacing='0' cellpadding='4' border='1' align='center' >"

    row_string = row_string & "<tr><td colspan='" & date_count + 1 & "' align='center'>"
    row_string = row_string & "<b>Asking Price Summary</b>"
    row_string = row_string & "</td></tr>"

    row_string = row_string & "<tr><td><b>Serial #</b></td>"

    For d = 0 To date_count - 1
      row_string = row_string & "<td align='right'><b>"
      row_string = row_string & Month(date_string(d)) & "/" & Day(date_string(d)) & "/" & Right(Year(Trim(date_string(d))), 2)
      row_string = row_string & "</b>&nbsp;</td>"
    Next

    row_string = row_string & "</tr>"

    For i = 0 To temp_count - 1
      row_Added = False

      For d = 0 To 100
        date_value_array(d) = 0
      Next

      For Each r As DataRow In results_table.Rows '--------------------- DATA ROWS ------------

        ' if serial number matches this line
        If Not IsDBNull(r("clival_ser_nbr")) Then
          If Trim(distinct_ser_num_list(i)) = Trim(r("clival_ser_nbr")) Then

            ' and date matches any line 
            If Not IsDBNull(r("tdate")) Then

              For d = 0 To date_count - 1
                If Trim(r("tdate")) = date_string(d) Then
                  ' if there is an asking price

                  date_value_array(d) = 1 ' set it to one to know a record exists

                  If Not IsDBNull(r("clival_asking_price")) Then
                    If CDbl(r("clival_asking_price")) > 0 Then
                      date_value_array(d) = (r("clival_asking_price") / 1000)
                    End If
                  End If

                End If
              Next


            End If
          End If
        End If
      Next          '--------------------- DATA ROWS ------------

      row_string = row_string & "<tr><td align='left'>" & distinct_ser_num_list(i) & "</td>"


      For d = 0 To date_count - 1

        If CDbl(date_value_array(d)) = 0 Then
          row_string = row_string & "<td align='right' bgcolor='#d9d9d9'>"
        ElseIf CDbl(date_value_array(d)) = 1 Then
          row_string = row_string & "<td align='right' bgcolor='#8ee388'>"
        Else
          row_string = row_string & "<td align='right' bgcolor='#8ee388'>"
          row_string = row_string & FormatNumber(date_value_array(d), 0) & "k"
        End If

        row_string = row_string & "</td>"


      Next

      row_string = row_string & "</tr>"

    Next

    row_string = row_string & "</table>"

    chart_html_string = row_string

  End Sub
End Class