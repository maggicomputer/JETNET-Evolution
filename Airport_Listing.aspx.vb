

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Airport_Listing.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:37a $
'$$Modtime: 6/18/19 6:11p $
'$$Revision: 2 $
'$$Workfile: Airport_Listing.aspx.vb $
'
' ********************************************************************************

Partial Public Class Airport_Listing
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not Page.IsPostBack Then
      SetUpToggle(True)
    End If
    airportSearchButton.OnClientClick = "$('#" & divTabLoading.ClientID & "').removeClass('display_none');"
  End Sub
  Private Sub SetUpClick()
    Dim ToggleClick As New StringBuilder
    If Not Page.ClientScript.IsStartupScriptRegistered("mGrid") Then

      ToggleClick.Append("$("".swipeToMove"").on(""swipeleft"", grabIDandRunButtonClick);")
      ToggleClick.Append("$(""#" & ResultsSearchDataList.ClientID & " a"").on(""click"", grabIDandRunButtonClick);")
      ToggleClick.Append("function grabIDandRunButtonClick(event) {")
      ToggleClick.Append("$('#" & divTabLoading.ClientID & "').removeClass('display_none');")
      ToggleClick.Append("$('#" & airportSearchID.ClientID & "').val('');")
      ToggleClick.Append("$('#" & airportSearchID.ClientID & "').val(event.currentTarget.id.replace('_',''));")
      'ToggleClick.Append("alert($('#" & airportSearchID.ClientID & "').val());")
      ToggleClick.Append("$('#" & ResultsSearchDataList.ClientID & "').toggle();")
      ToggleClick.Append("$('#" & airportSearchButton.ClientID & "').click();")
      ToggleClick.Append("};")
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(SearchUpdate, Me.GetType(), "mGrid", ToggleClick.ToString, True)
    End If

  End Sub

  Private Sub SetUpToggle(ByVal initial As Boolean)

    Dim ToggleClick As New StringBuilder

    ToggleClick.Append("function swipeBack() {")
    ToggleClick.Append("$(""#" & ResultsSearchDataList.ClientID & """).show();")
    ToggleClick.Append("$(""#" & mobileDataList.ClientID & """).hide();")
    ToggleClick.Append("$(""#" & ac_attention.ClientID & """).hide();")
    ToggleClick.Append("$("".airportName"").hide();")
    ToggleClick.Append("$(""#" & airportSearchID.ClientID & """).val('');")
    ToggleClick.Append("};")
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SwipeRight", ToggleClick.ToString, True)

    ToggleClick = New StringBuilder

    If initial = True Then
      If Not Page.ClientScript.IsStartupScriptRegistered("ToggleClickScript") Then
        ToggleClick.Append("$(function(){")
        ToggleClick.Append(" $('#" & controlLink.ClientID & "').click(function() {")


        'What I need to do right here..
        'Check and see if the first datagrid is hidden. If it is, we're going to morph the back button to unhide it, hide the second datagrid. So that it goes back.
        'Then once it's back, the back button will toggle the search.
        ToggleClick.Append("if ($(""#" & ResultsSearchDataList.ClientID & """).is("":hidden"")) {")
        ToggleClick.Append("swipeBack();")
        ToggleClick.Append("} else {")

        ToggleClick.Append("if ($(""#" & Collapse_Panel.ClientID & """).is("":hidden"")) {")
        ToggleClick.Append("$('#" & controlLink.ClientID & "').html('<img src=""../images/spacer.gif"" width=""13"" />');")
        ToggleClick.Append("$(""#" & Collapse_Panel.ClientID & """).show();")
        ToggleClick.Append("} else {")
        ToggleClick.Append("$(""#" & Collapse_Panel.ClientID & """).hide();")
        ToggleClick.Append("$('#" & controlLink.ClientID & "').html('<i class=""fa fa-chevron-left"" aria-hidden=""true""></i>');")
        ToggleClick.Append("}")

        ToggleClick.Append("}")
        ToggleClick.Append("});")
        ToggleClick.Append("});")
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleClickScript", ToggleClick.ToString, True)
      End If
    Else
      ToggleClick.Append("$("".airportName"").on(""swiperight click"", swipeBack);")
      ToggleClick.Append("$('#" & mobileDataList.ClientID & " .swipeToMoveBack').on(""swiperight"", swipeBack);")
      ToggleClick.Append("$('#" & controlLink.ClientID & "').html('<i class=""fa fa-chevron-left"" aria-hidden=""true""></i>');")
      ToggleClick.Append("$(""#" & Collapse_Panel.ClientID & """).hide();")
      ToggleClick.Append("$('#" & divTabLoading.ClientID & "').addClass('display_none');")
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(SearchUpdate, Me.GetType(), "ToggleOn", ToggleClick.ToString, True)
    End If

  End Sub
  Private Sub HideDatagrid()
    Dim toggleClick As New StringBuilder
    toggleClick.Append("$(""#" & ResultsSearchDataList.ClientID & """).hide();")
    System.Web.UI.ScriptManager.RegisterClientScriptBlock(SearchUpdate, Me.GetType(), "HideOldSearch", toggleClick.ToString, True)
  End Sub
  Public Function DisplayIataIcao(ByVal iata As Object, ByVal icao As Object) As String
    Dim returnstring As String = ""
    If Not IsDBNull(iata) Then
      If Not String.IsNullOrEmpty(iata) Then
        returnstring = iata.ToString
      End If
    End If

    If Not IsDBNull(iata) Or Not IsDBNull(icao) Then
      If Not String.IsNullOrEmpty(iata) And Not String.IsNullOrEmpty(icao) Then
        returnstring += "/"
      End If
    End If

    If Not IsDBNull(icao) Then
      returnstring += icao.ToString
    End If


    Return returnstring
  End Function

  Private Sub airportSearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles airportSearchButton.Click
    Dim ResultsTable As New DataTable

    If Not IsNumeric(airportSearchID.Text) Then
      ResultsTable = AirportQuerySearch(airportSearchTxt.Text, airportCitytxt.Text, airportIATACodetxt.Text, airportIATACodetxt.Text)
      mobileDataList.Visible = False
      ResultsSearchDataList.Visible = True

      If Not IsNothing(ResultsTable) Then
        If ResultsTable.Rows.Count > 0 Then
          ResultsSearchDataList.DataSource = ResultsTable
          ResultsSearchDataList.DataBind()
          ap_attention.Text = ""
          criteria_results.Text = ResultsTable.Rows.Count.ToString & " results"
        Else
          ResultsSearchDataList.DataSource = New DataTable
          ResultsSearchDataList.DataBind()
          criteria_results.Text = "0 results"
          ap_attention.Text = "<p align=""center"">No associated airports for this search term. Please click < and try another search.</p>"
        End If
      End If
      SetUpClick()
      listingUpdatePanel.Update()
    Else
      ResultsTable = AircraftByID(airportSearchID.Text)
      mobileDataList.Visible = True
      ResultsSearchDataList.Visible = False

      If Not IsNothing(ResultsTable) Then
        If ResultsTable.Rows.Count > 0 Then
          mobileDataList.DataSource = ResultsTable
          mobileDataList.DataBind()
          ac_attention.Text = ""
          airportName.Visible = True
          criteria_results.Text = ResultsTable.Rows.Count.ToString & " results"
          airportName.Text = "<h1 class=""airportName"">" & ResultsTable.Rows(0).Item("ac_aport_name").ToString & "</h1>" ' <strong>" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("ac_aport_city")), ResultsTable.Rows(0).Item("ac_aport_city").ToString & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("ac_aport_country")), ", ", ""), "") & ResultsTable.Rows(0).Item("ac_aport_country").ToString & "</strong></h1>"
        Else

          mobileDataList.DataSource = New DataTable
          mobileDataList.DataBind()
          ac_attention.Visible = True
          airportName.Text = ""
          criteria_results.Text = "0 results"
          ac_attention.Text = "<p align=""center"">No associated aircraft with this airport. Please click the < and pick another airport.</p>"
        End If
      End If
      HideDatagrid()
      mobileUpdate.Update()
    End If
    criteriaUpdatePanel.update()
    SetUpToggle(False)


  End Sub

  Public Function showEstAFTT(ByVal ac_airframe_tot_hrs As String, ByVal ac_est_airframe_hrs As String, ByVal ac_year As String, ByVal ac_times_as_of_date As String, ByVal bShowOnListing As Boolean, ByVal bShowOnTableHTML As Boolean) As String

    Dim htmlOutStr As String = ""

    Dim BASEACYEAR As Long = 2005
    Dim BASEACTIMES As Date = CDate("06/01/2005")

    Dim bShowEstAFTT As Boolean = True

    Dim nAcAFTT As Long = 0
    Dim nAcEstAFTT As Long = 0
    Dim nAcYear As Long = 0
    Dim dtAcTimesOfDate As Date = Now()

    If Not String.IsNullOrEmpty(ac_year.Trim) Then
      If IsNumeric(ac_year) Then
        nAcYear = CLng(ac_year.Trim)
      End If
    End If

    If Not String.IsNullOrEmpty(ac_times_as_of_date.Trim) Then
      If IsDate(ac_times_as_of_date) Then
        dtAcTimesOfDate = CDate(ac_times_as_of_date.Trim)
      End If
    End If

    If Not String.IsNullOrEmpty(ac_airframe_tot_hrs.Trim) Then
      If IsNumeric(ac_airframe_tot_hrs) Then
        nAcAFTT = CLng(ac_airframe_tot_hrs.Trim)
      End If
    End If

    If Not String.IsNullOrEmpty(ac_est_airframe_hrs.Trim) Then
      If IsNumeric(ac_est_airframe_hrs) Then
        nAcEstAFTT = CLng(ac_est_airframe_hrs.Trim)
      End If
    End If

    If nAcAFTT = 0 And nAcYear < BASEACYEAR Then
      bShowEstAFTT = False
    ElseIf dtAcTimesOfDate < BASEACTIMES Then
      bShowEstAFTT = False
    ElseIf nAcAFTT = nAcEstAFTT Then
      bShowEstAFTT = False
    End If

    If bShowOnListing Then
      If nAcAFTT > 0 Then
        If Session.Item("isMobile") = True Then
          If bShowEstAFTT Then
            If nAcEstAFTT > 0 Then
              htmlOutStr += "<span class=""float_right ""><span class=""help_cursor"" title=""Estimated AFTT based on flight hours."" style=""color:rgb(164, 86, 86);"">" & FormatNumber(nAcEstAFTT, 0).ToString & " hrs</span></span>"
            End If
          Else
            If nAcAFTT > 0 Then
              htmlOutStr += "<span class=""float_right "">" & FormatNumber(nAcAFTT, 0).ToString + " hrs</span>"
            End If
          End If
        Else
          htmlOutStr = "<span class=""""><span class=""label"">AFTT"
          htmlOutStr += IIf(bShowEstAFTT, " / <a href=""javascript:void();"" onclick=""openEstAFTTHelp();"" style=""color: rgb(164, 86, 86);"">EST AFTT</a>", "")
          htmlOutStr += "</span>:[" + nAcAFTT.ToString + "]"
          htmlOutStr += IIf(bShowEstAFTT, " / <span style=""color:rgb(164, 86, 86);"">[" + nAcEstAFTT.ToString + "]</span>", "") + "</span><br />"
        End If
      End If
    End If

    If bShowOnTableHTML Then
      If nAcAFTT > 0 Then
        htmlOutStr += "[" + nAcAFTT.ToString + "]"
        htmlOutStr += IIf(bShowEstAFTT, " / <span style=""color:rgb(164, 86, 86);"">[" + nAcEstAFTT.ToString + "]</span>", "") + "<br />"
      End If
    End If

    Return htmlOutStr

  End Function

  Private Function AircraftByID(ByVal id As Long) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      If Not String.IsNullOrEmpty(id) Then
        If id > 0 Then
          'Opening Connection
          SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
          SqlConn.Open()

          sqlQuery = "select distinct ac_id, amod_airframe_type_code, amod_type_code,ac_est_airframe_hrs, ac_last_aerodex_event, ac_picture_id,ac_aport_icao_code,ac_aport_iata_code,aport_latitude_decimal,aport_longitude_decimal, ac_list_date, amod_make_name, amod_model_name,amod_id, ac_mfr_year, ac_forsale_flag, "
          sqlQuery += " ac_year, ac_ser_no_full,ac_ser_no_sort, ac_reg_no, ac_times_as_of_date, ac_flights_id, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, "
          sqlQuery += " ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_status, ac_asking, ac_asking_price, ac_delivery,ac_reg_no_search, ac_exclusive_flag, ac_lease_flag, ac_engine_1_soh_hrs, "
          sqlQuery += " ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, ac_last_event , ac_aport_city, ac_aport_country, ac_aport_state ,ac_aport_name , NULL as ac_sale_price, 'N' as ac_sale_price_display_flag "
          sqlQuery += " from View_Aircraft_Flat with (NOLOCK) "
          sqlQuery += " where   (ac_lifecycle_stage = '3') and ac_aport_id = @aircraftID"
          sqlQuery += " order by ac_ser_no_sort"

          clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sqlQuery.ToString)

          Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

          SqlCommand.Parameters.AddWithValue("@aircraftID", id)

          SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

          Try
            atemptable.Load(SqlReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
          End Try



          SqlCommand.Dispose()
          SqlCommand = Nothing
        End If
      End If
      AircraftByID = atemptable
    Catch ex As Exception
      AircraftByID = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try
  End Function

  Private Function AirportQuerySearch(ByVal airportName As String, ByVal airportCity As String, ByVal airportIATACODE As String, ByVal airportICAOCODE As String) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim sqlWhere As String = ""
    Try

      'Opening Connection
      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sqlQuery = "SELECT aport_name, aport_city, aport_state, aport_country, aport_iata_code, aport_icao_code, aport_id FROM Airport with (NOLOCK) "
      sqlQuery += " WHERE aport_active_flag = 'Y' "
      sqlQuery += " AND ("

      If airportName <> "" Then
        sqlWhere += "aport_name LIKE @name "
      End If

      'These now come from the same textbox so they should be an OR. We can also technically use the same variable (either one) but I'd still
      'rather keep them separate for future changes. For now I will check the existence of both even though it's not necessary. If one is there, they both are.
      If airportIATACODE <> "" And airportICAOCODE <> "" Then
        Dim IATAString As String = ""
        If sqlWhere <> "" Then
          sqlWhere += " and ("
        Else
          sqlWhere += " ( "
        End If

        If airportIATACODE <> "" Then
          IATAString += " aport_iata_code LIKE @iata "
        End If

        If airportICAOCODE <> "" Then
          If IATAString <> "" Then
            IATAString += " or "
          End If
          IATAString += " aport_icao_code LIKE @icao "
        End If

        sqlWhere += IATAString & " )"
   
      End If


      If airportCity <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " aport_city LIKE @city"
      End If



      sqlQuery += sqlWhere
      sqlQuery += ") "
      sqlQuery += " ORDER BY aport_name ASC "

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sqlQuery.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If airportName <> "" Then
        SqlCommand.Parameters.AddWithValue("@name", "%" & airportName & "%")
      End If

      If airportCity <> "" Then
        SqlCommand.Parameters.AddWithValue("@city", "%" & airportCity & "%")
      End If

      If airportIATACODE <> "" Then
        SqlCommand.Parameters.AddWithValue("@iata", "%" & airportIATACODE & "%")
      End If

      If airportICAOCODE <> "" Then
        SqlCommand.Parameters.AddWithValue("@icao", "%" & airportICAOCODE & "%")
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try



      SqlCommand.Dispose()
      SqlCommand = Nothing

      AirportQuerySearch = atemptable
    Catch ex As Exception
      AirportQuerySearch = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try
  End Function

End Class