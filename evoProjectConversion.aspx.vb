
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/evoProjectConversion.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:39a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: evoProjectConversion.aspx.vb $
'
' ********************************************************************************

Partial Public Class evoProjectConversion

  Inherits System.Web.UI.Page

  Public good_results As Boolean = True
  Public known_bad_results As String = ""
  Public good_results_reason As String = ""
  Public type_code As String = ""
  Public airframe_type_code As String = ""
  Public amod_id_2 As String = ""
  Public amod_make_name As String = ""
  Public amod_model_name As String = ""
  Public market_status As String = ""


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    If Not Page.IsPostBack Then
      Call get_project_by_sub(Session.Item("localUser").crmSubSubID)
    End If

  End Sub

  Public Sub get_project_by_sub(ByVal sub_id As Long)

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoTempRS As System.Data.SqlClient.SqlDataReader : adoTempRS = Nothing
    Dim tmpQuery As String = ""
    Dim count_of_unconverted_ac As Integer = 0
    Dim count_of_unconverted_events As Integer = 0
    Dim count_of_unconverted_market_summary As Integer = 0

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      count_of_unconverted_ac = 0
      count_of_unconverted_ac = Get_Project_Counts(SqlCommand, adoTempRS, sub_id, "AircraftCriteria")


      If count_of_unconverted_ac = 0 Then
        Me.run_ac_export.Visible = False
      Else
        Me.run_ac_export.Visible = True
        Me.run_ac_export.Text = "Convert " & count_of_unconverted_ac.ToString & " Aircraft Projects"
      End If

      count_of_unconverted_events = 0
      count_of_unconverted_events = Get_Project_Counts(SqlCommand, adoTempRS, sub_id, "EventsCriteria")


      If count_of_unconverted_events = 0 Then
        Me.run_event_export.Visible = False
      Else
        Me.run_event_export.Visible = True
        Me.run_event_export.Text = "Convert " & count_of_unconverted_events.ToString & " Event Projects"
      End If

      count_of_unconverted_market_summary = 0
      count_of_unconverted_market_summary = Get_Project_Counts(SqlCommand, adoTempRS, sub_id, "MarketCriteria")
 
      If count_of_unconverted_market_summary = 0 Then
        Me.run_market_summary_export.Visible = False
      Else
        Me.run_market_summary_export.Visible = True
        Me.run_market_summary_export.Text = "Convert " & count_of_unconverted_market_summary.ToString & " Market Summary Projects"
      End If



      Me.count_line.Text = "You currently have " & (count_of_unconverted_ac + count_of_unconverted_events + count_of_unconverted_market_summary) & " Projects available to convert from the previous version of Evolution."




      adoTempRS = Nothing

    Catch ex As Exception
      ' aCommonEvo.DisplayAlert("Error in btnRunReport_Click: " & ex.Message)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing
    End Try

  End Sub
  Public Sub run_ac_report_clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles run_ac_export.Click
    run_project_page(Session.Item("localUser").crmSubSubID, "AircraftCriteria")
    Me.run_ac_export.Visible = False
  End Sub

  Public Sub run_event_report_clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles run_event_export.Click
    run_project_page(Session.Item("localUser").crmSubSubID, "EventsCriteria")
    Me.run_event_export.Visible = False
  End Sub
 
  Public Sub run_market_summary_export_clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles run_market_summary_export.Click
    run_project_page(Session.Item("localUser").crmSubSubID, "MarketCriteria")
    Me.run_event_export.Visible = False
  End Sub



  Public Function run_project_page(ByVal sub_id As Long, ByVal type_of_project As String) As String
    run_project_page = ""

    Dim tmpQuery As String = ""
    Dim tmpQuery2 As String = ""
    Dim tmpSelect As String = ""
    Dim tmpFrom As String = ""
    Dim tmpWhere As String = ""
    Dim tmpOrderby As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
    Dim adoTempRS As System.Data.SqlClient.SqlDataReader : adoTempRS = Nothing
    Dim adoTempRS2 As System.Data.SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim i As Integer = 0
    Dim k As Integer = 0
    Dim temp_string As String = ""
    Dim temp_string2 As String = ""
    Dim split_string(1000) As String
    Dim split_string2(1000) As String
    Dim temp_subject As String = ""
    Dim temp_desc As String = ""
    Dim temp_insert As String = ""
    Dim temp_seq As String = ""
    Dim temp_login As String = ""
    Dim temp_edate As String = ""
    Dim temp_udate As String = ""
    Dim temp_sub As String = ""
    Dim converted_where As String = ""
    Dim spot_1 As Integer = 0
    Dim spot_2 As Integer = 0
    Dim spot_3 As Integer = 0
    Dim evo_field_names As String
    Dim original_temp_string As String = ""
    Dim temp_val1 As String = ""
    Dim original_to_replace As String = ""
    Dim records_count As Integer = 0
    Dim string_for_trim As String = ""
    Dim special_replace_keeling As Integer = 0
    Dim special_replace_US As Integer = 0
    Dim special_replace_Brit As Integer = 0
    Dim good_count As Integer = 0
    Dim bad_count As Integer = 0
    Dim split_amod(100) As String
    Dim split_model(100) As String
    Dim split_make(100) As String
    Dim split_type(100) As String
    Dim split_aftype(100) As String
    Dim count_of_report As Integer = 0
    Dim select_count_report As String = ""
    Dim created_amod_id_list As String = ""
    Dim temp_sub_select_section As String = ""
    Dim new_sub_select_string As String = ""
    Dim temp_select_hold As String = ""
    Dim what_selecting As String = ""
    Dim project_id As Integer = 0
    Dim contact_type As String = ""
    Dim temp_temp_hold As String = ""
    Dim temp_Select_string As String = ""
    Dim sub_select_count As Long = 0
    Dim temp_states_hold As String = ""
    Dim us_ac_maintainted As String = ""
    Dim projects_not_converted As String = ""
    Dim run_flag As String = "N"
    Dim run_user_name As String = ""
    Dim run_user_email As String = ""
    Dim event_added_string As String = ""
    Dim event_added_final As String = ""
    Dim temp_event_string As String = ""
    Dim event_cat_name As String = ""
    Dim aport_code_temp As String = ""
    Dim temp_event_inner As String = ""
    Dim run_date_time_minutes As Long = 0
    Dim small_cpc_fix As String = ""

    Try

      ' SqlConn.ConnectionString = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=evolution;Password=vbs73az8"
      ' SqlConn2.ConnectionString = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=evolution;Password=vbs73az8"
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn2.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlConn2.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand2.Connection = SqlConn2
      SqlCommand2.CommandType = System.Data.CommandType.Text
      SqlCommand2.CommandTimeout = 60




      ' get list of all evo field names where they are advanced search
      evo_field_names = get_evo_field_names(SqlCommand2, adoTempRS2)

      ' split it on the commas so each name has its own array spot 
      split_string2 = Split(evo_field_names, ",")




      '-- delete folders for accounts 
      '-- don't reun this until you have insert ready to go in
      '        tmpQuery = "delete from Client_Folder where clifolder_sub_id in (9,777, 666,888)"


      '-- Project Query
      '-- Gets list of all active Aircraft projects that are from jetnet accounts
      'tmpQuery = " select top 100 * "
      'tmpQuery = tmpQuery & " from Subscription_Install_Saved_Search_Criteria "
      'tmpQuery = tmpQuery & " inner join Subscription with (NOLOCK) on sissc_sub_id=sub_id "
      'tmpQuery = tmpQuery & " WHERE sub_start_date <= GETDATE()"
      'tmpQuery = tmpQuery & " and (sub_end_date > GETDATE() or sub_end_date is NULL) "
      ''  tmpQuery = tmpQuery & " AND sissc_sub_id in (9,777, 666,888) "
      'tmpQuery = tmpQuery & " and sissc_tab='AircraftCriteria' "

      tmpQuery = " select * "
      tmpQuery = tmpQuery & " from Subscription_Install_Saved_Search_Criteria  with (nolock) "
      tmpQuery = tmpQuery & " inner join Subscription with (NOLOCK) on sissc_sub_id=sub_id "
      ' tmpQuery = tmpQuery & " WHERE sissc_sub_id in (select distinct sub_id from View_JETNET_Customers "
      '
      tmpQuery = tmpQuery & " where sissc_tab='" & type_of_project & "' "
      ' tmpQuery = tmpQuery & " where sissc_tab='AircraftCriteria' "
 


      ' tmpQuery = tmpQuery & " and sissc_sub_id = '777' "
      ' tmpQuery = tmpQuery & " and sissc_login = 'therese' "

      tmpQuery = tmpQuery & " and sissc_sub_id  = " & Session.Item("localUser").crmSubSubID.ToString.Trim & " "
      tmpQuery = tmpQuery & " and sissc_login = '" & Session.Item("localUser").crmUserLogin.ToString.Trim & "' "
      tmpQuery = tmpQuery & " and sissc_seq_no = '" & Session.Item("localUser").crmSubSeqNo.ToString.Trim & "' "
      tmpQuery = tmpQuery & " and sissc_convert_to_dotnet_flag = 'N' "

      ' tmpQuery = tmpQuery & " order by sissc_sub_id asc "



      SqlCommand.CommandText = tmpQuery
      adoTempRS = SqlCommand.ExecuteReader()


      If adoTempRS.HasRows Then

        Do While adoTempRS.Read

          good_results = True
          good_results_reason = ""
          known_bad_results = ""
          event_added_final = ""
          event_cat_name = ""
          temp_event_string = ""



          If Trim(type_of_project) = "EventsCriteria" Then

            tmpSelect = "select distinct priorev_id, priorev_journ_id, priorev_subject as apev_subject,  priorev_description as apev_description, priorev_comp_id, priorev_contact_id,   priorev_entry_date as apev_action_date , priorev_entry_date as apev_entry_date,  amod_airframe_type_code, amod_type_code, amod_id, amod_make_name, amod_model_name,  ac_ser_no_sort, ac_reg_no, ac_id, ac_year,  ac_ser_no_full,    0 as client_id "
            tmpFrom = "priority_events"
            tmpWhere = ""
            tmpOrderby = " order by  amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
            temp_event_inner = "inner join Priority_Events_Category WITH(NOLOCK) on priorev_category_code=priorevcat_category_code "
            temp_event_inner &= " LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (ac_id = priorev_ac_id AND ac_journ_id = 0 AND priorev_ac_id > 0)  "

          Else

            tmpSelect = "select distinct ac_id, ac_last_aerodex_event, ac_picture_id,ac_aport_icao_code,ac_aport_iata_code,aport_latitude_decimal,aport_longitude_decimal, ac_list_date, amod_make_name, amod_model_name,amod_id, amod_airframe_type_code, ac_mfr_year, ac_forsale_flag, ac_year, ac_ser_no_full,ac_ser_no_sort, ac_reg_no, ac_flights_id, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_status, ac_asking, ac_asking_price, ac_delivery,ac_reg_no_search, ac_exclusive_flag, ac_lease_flag, ac_engine_1_soh_hrs, ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, ac_last_event "
            tmpFrom = "View_Aircraft_Flat "
            tmpWhere = ""
            tmpOrderby = "order by amod_make_name, amod_model_name, ac_ser_no_sort"

          End If










          '  -- Get the where clause from the sissc_data field
          '  -- split by "~"
          '  -- find string that starts with "theWhere"
          temp_string = adoTempRS("sissc_data")
          split_string = Split(temp_string, "~")
          temp_string = ""

          run_flag = "N"
          run_user_name = ""
          If Not IsDBNull(adoTempRS("sissc_reply_username")) Then
            If Trim(adoTempRS("sissc_reply_username")) <> "" Then
              run_user_name = adoTempRS("sissc_reply_username")
            End If
          End If

          run_user_email = ""
          If Not IsDBNull(adoTempRS("sissc_reply_email")) Then
            If Trim(adoTempRS("sissc_reply_email")) <> "" Then
              run_user_email = adoTempRS("sissc_reply_email")
            End If
          End If

          If Not IsDBNull(adoTempRS("sissc_reply_email")) And Not IsDBNull(adoTempRS("sissc_reply_username")) Then
            If Trim(adoTempRS("sissc_reply_email")) <> "" And Trim(adoTempRS("sissc_reply_username")) <> "" Then
              run_flag = "Y"
            End If
          End If


          project_id = adoTempRS("sissc_id")

          If project_id = 1166 Then
            project_id = project_id
          End If


          For i = 0 To split_string.Length - 1
            If InStr(split_string(i), "theWhere=") Then
              temp_string = split_string(i)
              temp_string = Replace(temp_string, "!theWhere=", "")
              temp_string = Replace(temp_string, "theWhere=", "")
            End If
          Next


          '-----------EVENT INFORMATION----------------------------------
          '
          ' if its events get the events selection items from the sections they are in not from the where clause
          ' this should product event_added_final
          If Trim(type_of_project) = "EventsCriteria" Then
            event_added_string = ""

            event_added_final = event_added_final & "!~!events_type_of_search=Aircraft"
            event_added_final = event_added_final & "!~!events_type_of_search_1=false"
            event_added_final = event_added_final & "!~!events_type_of_search_2=false"

            run_date_time_minutes = 0

            For i = 0 To split_string.Length - 1
              If InStr(split_string(i), "EventDays=") Then
                spot_1 = InStr(split_string(i), "EventDays")
                If spot_1 > 0 Then 
                  run_date_time_minutes = run_date_time_minutes + (1440 * Replace(Replace(split_string(i), "EventDays=", ""), "!", "")) ' minutes in a day 
                  event_added_final = event_added_final & find_data_for_field(split_string(i), "eventdays", "event_days")
                End If
              ElseIf InStr(split_string(i), "EventMonths=") Then
                spot_1 = InStr(split_string(i), "EventMonths")
                If spot_1 > 0 Then
                  run_date_time_minutes = run_date_time_minutes + (43200 * Replace(Replace(split_string(i), "EventMonths=", ""), "!", "")) ' minutes in a month 
                  event_added_final = event_added_final & find_data_for_field(split_string(i), "eventmonths", "events_months")
                End If
              ElseIf InStr(split_string(i), "EventHours=") Then
                spot_1 = InStr(split_string(i), "EventHours")
                If spot_1 > 0 Then
                  run_date_time_minutes = run_date_time_minutes + (60 * Replace(Replace(split_string(i), "EventHours=", ""), "!", "")) ' minutes in an hour 
                  event_added_final = event_added_final & find_data_for_field(split_string(i), "eventhours", "event_hours")
                End If
              ElseIf InStr(split_string(i), "EventMinutes=") Then
                spot_1 = InStr(split_string(i), "EventMinutes")
                If spot_1 > 0 Then
                  run_date_time_minutes = run_date_time_minutes + (1 * Replace(Replace(split_string(i), "EventMinutes=", ""), "!", ""))
                  event_added_final = event_added_final & find_data_for_field(split_string(i), "eventminutes", "event_minutes")
                End If
              ElseIf InStr(split_string(i), "EventCat=") Then
                spot_1 = InStr(split_string(i), "EventCat")
                If spot_1 > 0 Then
                  ' temp_event_string = find_data_for_field(split_string(i), "eventcat", "events_market_types")
                  temp_event_string = LCase(split_string(i))
                  temp_event_string = Replace(temp_event_string, "eventcat=", "")
                  temp_event_string = Replace(temp_event_string, "!", "")
                  temp_event_string = "'" & Trim(Replace(Replace(temp_event_string, ",", "','"), " ", "")) & "'"
                  temp_event_string = get_prior_event_cat(SqlCommand2, adoTempRS2, Trim(temp_event_string), event_cat_name)

                  event_added_final = event_added_final & "!~!events_market_types=" & event_cat_name
                  event_added_final = event_added_final & "!~!events_market_categories=" & temp_event_string
                End If
              ElseIf InStr(split_string(i), "EventType=") Then

              End If
            Next

            ' if there is no type but there might be a category 
            If InStr(event_added_final, "events_market_categories") = 0 Then
              For i = 0 To split_string.Length - 1
                spot_1 = InStr(split_string(i), "EventType")
                If spot_1 > 0 Then
                  event_added_final = event_added_final & find_data_for_field(split_string(i), "eventtype", "events_market_categories")
                End If
              Next
            End If

            event_added_string = ""
            For i = 0 To split_string.Length - 1
              spot_1 = InStr(split_string(i), "AircraftType")
              If spot_1 > 0 Then
                'cboAircraftTypeID=E######T##P##T
                event_added_string = find_data_for_field(split_string(i), "aircrafttype", "cboaircrafttypeid")

                event_added_string = Replace(event_added_string, ",", "##")
                event_added_string = Replace(event_added_string, " ", "")
                event_added_string = Replace(event_added_string, "cboaircrafttypeid", "cboAircraftTypeID")

                event_added_string = Replace(event_added_string, "698", "E|F")    'jet(airliner = 698)
                event_added_string = Replace(event_added_string, "640", "J|F")    'bus(jet = 640) 
                event_added_string = Replace(event_added_string, "300", "P|F")    ' piston(f = 300) 
                event_added_string = Replace(event_added_string, "735", "T|F")    'turbo(prop = 735)


                event_added_string = Replace(event_added_string, "460", "P|R")    ' piston(r = 460) 
                event_added_string = Replace(event_added_string, "365", "T|R")    'turbine = 365

              End If
            Next

          End If




          '-----------EVENT INFORMATION----------------------------------

          '  -- recommend print of old where and new where - the rest should be all set

          '  -- Fields to insert
          '  -- sissc_subject = name of folder
          '  -- sissc_description = folder description
          temp_subject = adoTempRS("sissc_subject")
          If Not IsDBNull(adoTempRS("sissc_description")) Then
            temp_desc = adoTempRS("sissc_description")
          Else
            temp_desc = ""
          End If
          temp_sub = adoTempRS("sissc_sub_id")
          temp_login = adoTempRS("sissc_login")
          temp_seq = adoTempRS("sissc_seq_no")
          temp_edate = adoTempRS("sissc_entry_date")
          temp_udate = adoTempRS("sissc_update_date")


          If has_been_converted(SqlCommand2, adoTempRS2, project_id) Then '---------------------------------------------------------------


          Else





            If Right(Trim(temp_string), 1) <> "!" Then
              temp_string = temp_string & "!"
            End If


            original_temp_string = temp_string
            original_temp_string = Left(original_temp_string, Len(original_temp_string) - 1) ' trim off the last !

            converted_where = converted_where & "!~!ddlWeightClass=All"

            '--------------- SPECIAL CASE FOR THE THE ( LOCATION ) --------------------=
            temp_string = Replace(temp_string, "u.s.", "U.S.")
            temp_string = Replace(temp_string, "british", "British")
            temp_string = Replace(temp_string, "keeling", "Keeling")
            temp_string = Replace(temp_string, "(former)", "(Former)")

            special_replace_keeling = InStr(temp_string, "(Keeling)")
            If special_replace_keeling > 0 Then
              temp_string = Replace(temp_string, "(Keeling)", "Keeling")
            End If
            special_replace_US = InStr(temp_string, "(U.S.)")
            If special_replace_US > 0 Then
              temp_string = Replace(temp_string, "(U.S.)", "U.S.")
            End If
            special_replace_Brit = InStr(temp_string, "(British)")
            If special_replace_Brit > 0 Then
              temp_string = Replace(temp_string, "(British)", "British")
            End If
            special_replace_Brit = InStr(temp_string, "USSR (Former)")
            If special_replace_Brit > 0 Then
              temp_string = Replace(temp_string, "USSR (Former)", "USSR Former")
            End If
            '--------------- SPECIAL CASE FOR THE THE ( LOCATION ) --------------------=



            If InStr(temp_string, "EXISTS") > 0 Then
              ' CLIP_EXISTS(temp_string)
            End If





            '----------- THIS SECTION IS TO GET RID OF THE ITEMS THAT ARE SUB SELECTED------------------------------------
            spot_1 = InStr(temp_string, "ac_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_id", "")
            End If


            spot_1 = InStr(temp_string, "ac_engine_maintenance_prog_EMP")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_engine_maintenance_prog_EMP", "")
            End If


            '----------- THIS SECTION IS TO GET RID OF THE ITEMS THAT ARE SUB SELECTED------------------------------------





            If Trim(event_added_string) <> "" Then
              converted_where = converted_where & event_added_string
            End If










            '--------------- SPECIAL CASE FOR THE WORD LOWER--------------------=
            spot_1 = InStr(temp_string, "lower(")
            If spot_1 > 0 Then
              spot_2 = InStr(spot_1, temp_string, ")")
              string_for_trim = Mid(temp_string, spot_1, (spot_2 - spot_1))
              string_for_trim = Replace(string_for_trim, "lower(", "")
              string_for_trim = Replace(string_for_trim, ")", "")
              string_for_trim = Trim(string_for_trim)
              temp_string = Replace(temp_string, "lower(" & string_for_trim & ")", string_for_trim)

            End If

            '--------------- SPECIAL CASE FOR THE WORD LOWER--------------------=






            temp_val1 = ""

            ' go through each of the fields in the database 
            ' if there is one of those in the where clause then go through and replace it 
            For i = 0 To split_string2.Length - 1






              spot_1 = InStr(temp_string, split_string2(i))
              If Len(Trim(split_string2(i))) > 2 Then
                If spot_1 > 0 Then
                  temp_string2 = Right(temp_string, Len(temp_string) - spot_1 + 1)
                  spot_2 = InStr(temp_string2, ")")
                  spot_3 = InStr(temp_string2, "AND")


                  ' if there is a ) but there is a AND before that 
                  ' then see if there is a between 
                  ' if there is get it 
                  If (spot_2 > spot_3) And spot_3 > 0 Then
                    temp_val1 = Left(temp_string2, spot_2 - 1)
                    spot_1 = InStr(temp_val1, "BETWEEN")
                    If spot_1 > 0 Then
                      temp_val1 = Right(temp_val1, Len(temp_val1) - spot_1 - 6)
                      temp_val1 = Replace(temp_val1, "AND", ";")
                      temp_val1 = Replace(temp_val1, " ", "")
                    Else
                      temp_val1 = ""
                    End If
                  Else
                    temp_val1 = ""
                  End If

                  If Trim(temp_val1) = "" Then
                    ' check to see if there is an and rather than an end paren
                    ' if there is, make spot 2 be the and spot
                    If (spot_3 = 0) And (spot_2 = 0) Then
                      spot_2 = InStr(temp_string2, "!")
                    ElseIf (spot_3 < spot_2 And (spot_3 > 0)) Or (spot_2 = 0) Then
                      spot_2 = spot_3
                    End If



                    temp_string2 = Left(temp_string2, spot_2 - 1)


                    If InStr(UCase(temp_string2), " NOT IN (") > 0 Then
                      temp_string2 = temp_string2
                      temp_string2 = Replace(temp_string2, " NOT IN (", "")
                      temp_string2 = Replace(temp_string2, " not in (", "")
                      temp_string2 = Replace(temp_string2, split_string2(i), "")
                      temp_string2 = Trim(temp_string2)
                      temp_val1 = "BADRECORD(" & temp_string2 & ")"
                      temp_string2 = "NOT IN"
                    ElseIf InStr(UCase(temp_string2), " IN (") > 0 Then
                      temp_string2 = temp_string2
                      temp_string2 = Replace(temp_string2, " IN (", "")
                      temp_string2 = Replace(temp_string2, " in (", "")
                      temp_string2 = Replace(temp_string2, split_string2(i), "")
                      temp_string2 = Trim(temp_string2)
                      temp_val1 = "BADRECORD(" & temp_string2 & ")"
                      temp_string2 = "IN"
                    End If





                    ' if there is no between or instring 
                    If Trim(temp_val1) = "" Then

                      spot_1 = InStr(temp_string2, "=")
                      If spot_1 > 0 Then
                        temp_val1 = Right(temp_string2, Len(temp_string2) - spot_1)
                        temp_string2 = "Equals"
                      Else
                        spot_1 = InStr(temp_string2, "<")
                        If spot_1 > 0 Then
                          temp_val1 = Right(temp_string2, Len(temp_string2) - spot_1)
                          temp_string2 = "Less Than"

                        Else
                          spot_1 = InStr(temp_string2, ">")
                          If spot_1 > 0 Then
                            temp_val1 = Right(temp_string2, Len(temp_string2) - spot_1)
                            temp_string2 = "Greater Than"
                          Else
                            ' between already done
                          End If
                        End If
                      End If

                    End If

                  Else
                    string_for_trim = string_for_trim
                  End If
                  'get the spot of the initial find
                  string_for_trim = temp_string



                  spot_1 = InStr(string_for_trim, split_string2(i))
                  spot_2 = InStr(spot_1, string_for_trim, ")")
                  spot_3 = InStr(spot_1, string_for_trim, "AND")

                  If InStr(temp_string2, "BETWEEN") > 0 Then
                    good_results = False
                    good_results_reason = good_results_reason & "BETWEEN, "
                    If spot_2 = 0 Then
                      spot_2 = InStr(spot_1, string_for_trim, "!")
                    End If
                  Else
                    If (spot_3 = 0) And (spot_2 = 0) Then
                      spot_2 = InStr(string_for_trim, "!")
                    ElseIf (spot_3 < spot_2 And (spot_3 > 0)) Or (spot_2 = 0) Then
                      spot_2 = spot_3
                    End If

                  End If






                  Try



                    original_to_replace = Left(string_for_trim, spot_2 - 1)
                    original_to_replace = Right(string_for_trim, Len(string_for_trim) - spot_1 + 1)

                    ' string_for_trim added in case of multiple dynamic

                    temp_string = Left(string_for_trim, spot_1 - 1) ' = everything before that section 
                    ' then starting in that section add  everything to the right 


                    temp_string = temp_string & Right(string_for_trim, Len(string_for_trim) - spot_2)  '  - spot_1 

                  Catch ex As Exception
                  Finally

                  End Try


                  If InStr(temp_val1, "CONVERT (DATETIME,") > 0 Then
                    temp_val1 = Replace(temp_val1, "CONVERT (DATETIME,", "")
                    temp_val1 = Replace(temp_val1, ",102", "")
                  End If



                  If InStr(split_string2(i), "ac_aport_iata_code") > 0 Then
                  Else
                    converted_where = converted_where & get_control_name(SqlCommand2, adoTempRS2, split_string2(i), Trim(temp_string2), Trim(temp_val1))
                  End If


                  ' replace the other sections of that original 
                  temp_string = Replace(temp_string, original_to_replace, "")
                  original_to_replace = Replace(original_to_replace, " ", "")
                  temp_string = Replace(temp_string, original_to_replace, "")


                  converted_where = Replace(converted_where, (" OR " & split_string2(i) & " = "), "##")

                  If Trim(split_string2(i)) = "comp_country" Or Trim(split_string2(i)) = "ac_aport_country" Then
                    If special_replace_keeling > 0 Then
                      temp_string = Replace(temp_string, "Keeling ", "(Keeling) ")
                    End If
                    If special_replace_US > 0 Then
                      temp_string = Replace(temp_string, "U.S. ", "(U.S.) ")
                    End If
                    If special_replace_Brit > 0 Then
                      temp_string = Replace(temp_string, "British ", "(British) ")
                    End If
                    If special_replace_Brit > 0 Then
                      temp_string = Replace(temp_string, "USSR Former ", "USSR (Former) ")
                    End If
                  End If
                End If
              End If 
            Next






            ' ------'------------------- DO REPLACES ON ITEMS THAT CAN BE REPLACED AND BUILT---------------
            '

            If InStr(temp_string, "amod_customer_flag='Y'") > 0 Then
              temp_string = Replace(temp_string, "amod_customer_flag='Y'", "")
            End If

            If InStr(temp_string, "amod_customer_flag = 'Y'") > 0 Then
              temp_string = Replace(temp_string, "amod_customer_flag = 'Y'", "")
            End If

            ' If InStr(temp_string, "AND (comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y')") > 0 Then
            '     temp_string = Replace(temp_string, "AND (comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' OR comp_product_commercial_flag = 'Y')", "")
            '     converted_where = converted_where & "comp_product_helicopter_flag=True!~!comp_product_business_flag=True!~!comp_product_commercial_flag=True!~!"
            ' End If




            '--------------------------- BRILLIANCE - CUTTING EACH FUNCTION------------------
            spot_1 = 0



            '--------------- MODEL SECTION---------------------------------

            ' clear all variables to create
            airframe_type_code = ""
            amod_id_2 = ""
            amod_make_name = ""
            amod_model_name = ""
            type_code = ""
            created_amod_id_list = ""

            ' --- this area sets the above values while still chopping out the values from the text string
            spot_1 = InStr(temp_string, "amod_id")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "amod_id", "")
            End If

            spot_1 = InStr(temp_string, "amod_make_name")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "amod_make_name", "")
            End If

            spot_1 = InStr(temp_string, "amod_model_name")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "amod_model_name", "")
            End If

            If Trim(amod_id_2) <> "" And InStr(amod_id_2, "##") = 0 And InStr(amod_id_2, "OR") = 0 Then
              If Trim(amod_make_name) = "" Or Trim(amod_model_name) = "" Then
                Call get_make_model(SqlCommand2, adoTempRS2, amod_make_name, amod_model_name, amod_id_2)
              End If
            End If

            If Trim(amod_id_2) = "" Then
              created_amod_id_list = get_amod_id_model_name(SqlCommand2, adoTempRS2, amod_make_name, amod_model_name)
            End If

            spot_1 = InStr(LCase(temp_string), "amod_type_code")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "amod_type_code", "")
            End If

            spot_1 = InStr(temp_string, "amod_airframe_type_code")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "amod_airframe_type_code", "")
            End If

            airframe_type_code = Replace(airframe_type_code, " OR amod_airframe_type_code = ", "##")
            amod_id_2 = Replace(amod_id_2, " OR amod_id = ", "##")
            amod_make_name = Replace(amod_make_name, " OR amod_make_name = ", "##")
            amod_model_name = Replace(amod_model_name, " OR amod_model_name = ", "##")
            type_code = Replace(type_code, " OR amod_type_code = ", "##")



            ' build special strings

            If records_count = 185 Then
              records_count = records_count
            End If

            ReDim split_amod(1000)
            ReDim split_make(1000)
            ReDim split_model(1000)
            ReDim split_type(1000)
            ReDim split_aftype(1000)

            For k = 0 To 999
              split_amod(k) = ""
              split_make(k) = ""
              split_model(k) = ""
              split_type(k) = ""
              split_aftype(k) = ""
            Next

            If InStr(amod_id_2, "##") > 0 Then
              If Left(amod_id_2, 2) = "##" Then
                amod_id_2 = Right(amod_id_2, Len(amod_id_2) - 2)
              End If
              split_amod = Split(amod_id_2, "##")
            End If

            If InStr(amod_make_name, "##") > 0 Then
              If Left(amod_make_name, 2) = "##" Then
                amod_make_name = Right(amod_make_name, Len(amod_make_name) - 2)
              End If
              split_make = Split(amod_make_name, "##")
            End If

            If InStr(amod_model_name, "##") > 0 Then
              If Left(amod_model_name, 2) = "##" Then
                amod_model_name = Right(amod_model_name, Len(amod_model_name) - 2)
              End If
              split_model = Split(amod_model_name, "##")
            End If

            If InStr(type_code, "##") > 0 Then
              If Left(type_code, 2) = "##" Then
                type_code = Right(type_code, Len(type_code) - 2)
              End If
              split_type = Split(type_code, "##")
            End If

            If InStr(airframe_type_code, "##") > 0 Then
              If Left(airframe_type_code, 2) = "##" Then
                airframe_type_code = Right(airframe_type_code, Len(airframe_type_code) - 2)
              End If
              split_aftype = Split(airframe_type_code, "##")
            End If

            If split_amod.Length = 1001 And Trim(split_amod(0)) = "" Then
              ReDim Preserve split_amod(0)
            End If

            If split_make.Length = 1001 And Trim(split_make(0)) = "" Then
              ReDim Preserve split_make(0)
            End If
            If split_model.Length = 1001 And Trim(split_model(0)) = "" Then
              ReDim Preserve split_model(0)
            End If




            ' if there is no make and models yet there is amod ids then go and get the make models 
            'If (split_amod.Length > split_make.Length) Or (split_amod.Length > split_model.Length) Then
            ReDim split_make(1000)
            ReDim split_model(1000)
            If split_amod.Length > 1 Or Trim(split_amod(0)) <> "" Then


              For k = 0 To split_amod.Length - 1
                If Trim(split_amod(k)) <> "" Then
                  Call get_make_model(SqlCommand2, adoTempRS2, split_make(k), split_model(k), split_amod(k))
                End If
              Next

              If split_model.Length = 1001 And Trim(split_model(0)) <> "" Then
                For k = 0 To 1000
                  If Trim(split_model(k)) = "" Then
                    ReDim Preserve split_model(k - 1)
                    k = 1001
                  End If
                Next
              End If

              If split_make.Length = 1001 And Trim(split_make(0)) <> "" Then
                For k = 0 To 1000
                  If Trim(split_make(k)) = "" Then
                    ReDim Preserve split_make(k - 1)
                    k = 1001
                  End If
                Next
              End If
              ' ElseIf split_amod.Length > 0 And split_make.Length > 0 Then
              'temp_string = temp_string
              'ElseIf split_amod.Length > 0 And split_model.Length > 0 Then
              'temp_string = temp_string
              'End If



              If split_type.Length = 1001 And Trim(split_type(0)) = "" Then
                ReDim Preserve split_type(0)
              End If
              If split_aftype.Length = 1001 And Trim(split_aftype(0)) = "" Then
                ReDim Preserve split_aftype(0)
              End If




              converted_where = converted_where & "!~!cboBaseRegionID=All"
              converted_where = converted_where & "!~!chkRegionalFilterID=true"
              converted_where = converted_where & "!~!chkDefaultFilterID=true"
              converted_where = converted_where & "!~!radBaseContinentRegionID=true"
              converted_where = converted_where & "!~!radBaseContinentRegionID1=true"
              converted_where = converted_where & "!~!NoOverdue=true"
              converted_where = converted_where & "!~!comp_not_in_selected=true"
              converted_where = converted_where & "!~!radContinentRegionID=true"
              converted_where = converted_where & "!~!radContinentRegionID1=true"


              ' and currently all the lengths are the same 
              ' If split_type.Length > 1 And split_type.Length = split_aftype.Length Then  ' will figure out different way around later 

              converted_where = converted_where & "!~!cboAircraftTypeID="

              For k = 0 To split_type.Length - 1
                If k = 0 Then
                  converted_where = converted_where & split_type(k)
                Else
                  converted_where = converted_where & "##" & split_type(k)
                End If
              Next

              'Else
              '    ' if there is more than one but they are not all equal, it is currently just putting in one - so it is wrong 
              '    If split_type.Length > 1 Then
              '        good_results = False
              '        If Not IsNothing(airframe_type_code) Then
              '            good_results_reason = good_results_reason & "type_code and af type code dont match size,"
              '        Else
              '            good_results_reason = good_results_reason & "type_code and af type code dont match size (no af type),"
              '        End If
              '    End If

              '    If Trim(type_code) = "" And Trim(airframe_type_code) = "" Then
              '        converted_where = converted_where & "!~!cboAircraftTypeID=All"
              '    Else
              '        converted_where = converted_where & "!~!cboAircraftTypeID=" & type_code
              '    End If


              'End If

              ' and currently all the lengths are the same 
              ' If split_type.Length > 1 And split_type.Length = split_make.Length Then

              converted_where = converted_where & "!~!cboAircraftMakeID="
              For k = 0 To split_make.Length - 1
                If k = 0 Then
                  converted_where = converted_where & split_make(k)
                Else
                  converted_where = converted_where & "##" & split_make(k)
                End If
              Next
              'ElseIf split_type.Length = 1 And split_make.Length > 1 Then
              '' if there is only one type but multiple makes, then the type goes will all of the makes 
              'For k = 0 To split_make.Length - 1
              '    If k = 0 Then
              '        converted_where = converted_where & split_make(k)
              '    Else
              '        converted_where = converted_where & "##" & split_make(k)
              '    End If
              'Next
              'Else


              'If split_type.Length > 1 And split_make.Length = 1 Then
              '    If IsNothing(amod_make_name) Then
              '        converted_where = converted_where & "!~!cboAircraftMakeID="
              '        For k = 0 To split_make.Length - 1
              '            If k > 0 Then
              '                converted_where = converted_where & "##"
              '            End If
              '            converted_where = converted_where & amod_make_name
              '        Next
              '        good_results = False
              '        good_results_reason = good_results_reason & "type_code and amod_make_name dont match size (amod_make name is nothing),"
              '    Else
              '        converted_where = converted_where & "!~!cboAircraftMakeID=" & amod_make_name
              '    End If
              'Else
              '    If split_type.Length = 1 And split_make.Length = 1 Then
              '        converted_where = converted_where & "!~!cboAircraftMakeID=" & amod_make_name
              '    Else
              '        converted_where = converted_where & "!~!cboAircraftMakeID=" & amod_make_name
              '        good_results = False
              '        good_results_reason = good_results_reason & "type_code and amod_make_name dont match size,"
              '    End If
              'End If



              '  End If

              '' and currently all the lengths are the same 
              'If split_amod.Length > 1 And (split_amod.Length = split_make.Length) And (split_amod.Length = split_model.Length) And (split_make.Length = split_model.Length) Then

              '    converted_where = converted_where & "!~!cboAircraftModelID="

              '    For k = 0 To split_amod.Length - 1
              '        If k = 0 Then
              '            converted_where = converted_where & split_amod(k) & "|" & split_make(k) & "|" & split_model(k) & "|JETNET|0"
              '        Else
              '            converted_where = converted_where & "##" & split_amod(k) & "|" & split_make(k) & "|" & split_model(k) & "|JETNET|0"
              '        End If
              '    Next
              'Else
              '    If split_amod.Length > 1 Then
              '        good_results = False
              '        good_results_reason = "Multiple Models ( ONLY DISPLAYING ONE)"
              '    End If

              '    If InStr(Trim(amod_id_2), "##") > 0 Or InStr(Trim(amod_id_2), " OR ") > 0 Then
              '        good_results = False
              '        good_results_reason = "Multiple Models ( NOT MULTIPLE NAMES)"
              '    End If

              '    If Trim(amod_id_2) <> "" Then
              '        converted_where = converted_where & "!~!cboAircraftModelID=" & amod_id_2 & "|" & amod_make_name & "|" & amod_model_name & "|JETNET|0"
              '    Else
              '        converted_where = converted_where & "!~!cboAircraftModelID=All"
              '    End If

              'End If


              converted_where = converted_where & "!~!cboAircraftModelID="
              For k = 0 To split_amod.Length - 1
                If k = 0 Then
                  converted_where = converted_where & split_amod(k)
                Else
                  converted_where = converted_where & "##" & split_amod(k)
                End If
              Next
            Else
              If Trim(amod_model_name) <> "" Then
                If Trim(amod_id_2) <> "" And Trim(created_amod_id_list) = "" Then
                  converted_where = converted_where & "!~!cboAircraftModelID=" & amod_id_2
                ElseIf Trim(created_amod_id_list) <> "" Then
                  converted_where = converted_where & "!~!cboAircraftModelID=" & created_amod_id_list
                Else
                  'converted_where = converted_where & "!~!cboAircraftModelID=All"
                End If
              Else
                'converted_where = converted_where & "!~!cboAircraftModelID=All"
              End If

              If Trim(amod_make_name) <> "" Then
                converted_where = converted_where & "!~!cboAircraftMakeID=" & amod_make_name
              Else
                ' converted_where = converted_where & "!~!cboAircraftMakeID=All"
              End If

              If Trim(amod_model_name) <> "" Then
                converted_where = converted_where & "!~!cboAircraftTypeID=" & type_code
              Else
                ' converted_where = converted_where & "!~!cboAircraftTypeID=All"
              End If
            End If

            '--------------- MODEL SECTION---------------------------------



            '--------------- MARKET SUMMARY SECTION---------------------------------
            If Trim(type_of_project) = "MarketCriteria" Then 

              For i = 0 To split_string.Length - 1
                If InStr(split_string(i), "TypeCode=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "typecode", "transaction_type_lb")
                End If

                If InStr(split_string(i), "TypeLblFrom=True") Then
                  converted_where = converted_where & "!~!transaction_from=from"
                End If

                If InStr(split_string(i), "TypeCodeFrom=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "typecodefrom", "transaction_from_lb")
                End If

                If InStr(split_string(i), "TypeLblTo=True") Then
                  converted_where = converted_where & "!~!transaction_to=to"
                End If

                If InStr(split_string(i), "TypeCodeTo=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "typecodeto", "transaction_to_lb")
                End If



                If InStr(split_string(i), "MktStartDate=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "mktstartdate", "cboStartDateID")
                End If

                'If InStr(split_string(i), "MktEndDate=") Then
                'converted_where = converted_where & find_data_for_field(split_string(i), "MktEndDate", "cboStartDateID")
                ' End If

                If InStr(split_string(i), "TimeScale=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "timescale", "cboTimeScaleID")
                End If

                If InStr(split_string(i), "ScaleSets=") Then
                  converted_where = converted_where & find_data_for_field(split_string(i), "scalesets", "cboRangeSpanID")
                End If

   


 

              Next

 

              If InStr(temp_string, "journ_newac_flag") Then
                converted_where = converted_where & find_data_for_field(temp_string, "journ_newac_flag", "chkNewToMarketID")
              End If

              spot_1 = InStr(temp_string, "journ_internal_trans_flag")
              If spot_1 > 0 Then
                find_data_for_field(temp_string, "journ_internal_trans_flag", "")
              End If

              spot_1 = InStr(temp_string, "journ_subcategory_code")
              If spot_1 > 0 Then
                find_data_for_field(temp_string, "journ_subcategory_code", "")
              End If

              spot_1 = InStr(temp_string, "journ_subcat_code_part2")
              If spot_1 > 0 Then
                find_data_for_field(temp_string, "journ_subcat_code_part2", "")
              End If

              spot_1 = InStr(temp_string, "journ_subcat_code_part3")
              If spot_1 > 0 Then
                find_data_for_field(temp_string, "journ_subcat_code_part3", "")
              End If

            End If
            '--------------- MARKET SUMMARY SECTION---------------------------------





            '--------------- AIRCRAFT SECTION---------------------------------
            spot_1 = InStr(temp_string, "ac_product_business_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_product_business_flag", "chkBusinessFilterID")
            End If


            spot_1 = InStr(temp_string, "ac_product_commercial_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_product_commercial_flag", "chkCommercialFilterID")
            End If

            spot_1 = InStr(temp_string, "ac_product_helicopter_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_product_helicopter_flag", "chkHelicopterFilterID")
            End If

            ' chkHelicopterFilterID: true
            'chkBusinessFilterID: true
            'chkCommericalFilterID: true
            'chkDefaultFilterID: false
            'cboWeightClassID:
            'cboAircraftTypeID: 290
            'cboAircraftMakeID: 303
            'cboAircraftModelID: 308



            market_status = ""

            If InStr(UCase(temp_subject), "NFS") > 0 Then
              converted_where = converted_where
            End If

            spot_1 = InStr(temp_string, "ac_forsale_flag")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "ac_forsale_flag", "")
              ' run this and set the value = the temp status, if there is a status then it will put it in, otherwise it will put in the flag
            End If

            If Trim(market_status) <> "" Then
              If Trim(market_status) = "For Sale" Then
                converted_where = converted_where & "!~!market=For Sale"
                spot_1 = InStr(temp_string, "ac_exclusive_flag")
                If spot_1 > 0 Then
                  temp_temp_hold = find_data_for_field(temp_string, "ac_exclusive_flag", "")
                  temp_temp_hold = Replace(temp_temp_hold, "!~!", "")
                  temp_temp_hold = Replace(temp_temp_hold, "=", "")
                  temp_temp_hold = Replace(temp_temp_hold, "ac_exclusive_flag", "")
                  temp_temp_hold = Replace(temp_temp_hold, "'", "")
                  If Trim(UCase(temp_temp_hold)) = "Y" Then
                    converted_where = converted_where & " on Exclusive"
                  ElseIf Trim(UCase(temp_temp_hold)) = "N" Then
                    converted_where = converted_where & " Not on Exclusive"
                  End If
                End If
              Else
                converted_where = converted_where & "!~!market=" & market_status
              End If
            End If





            spot_1 = InStr(temp_string, "ac_lease_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_lease_flag", "lease_status")
            End If





            spot_1 = InStr(temp_string, "ac_lifecycle_stage")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_lifecycle_stage", "")
            End If

            temp_states_hold = ""
            spot_1 = InStr(temp_string, "ac_aport_state")
            If spot_1 > 0 Then
              temp_states_hold = find_data_for_field(temp_string, "ac_aport_state", "cboBaseStateID")
              temp_states_hold = Replace(UCase(temp_states_hold), "!~!CBOBASESTATEID=", "")
              Try

                temp_Select_string = " select distinct state_code, state_name from state where state_active_flag = 'Y' and state_country = 'United States' "

                SqlCommand2.CommandText = temp_Select_string
                adoTempRS2 = SqlCommand2.ExecuteReader()

                If adoTempRS2.HasRows Then
                  Do While adoTempRS2.Read
                    temp_states_hold = Replace(temp_states_hold, Trim(adoTempRS2("state_code")), Trim(adoTempRS2("state_name")))
                  Loop
                End If

                temp_states_hold = "!~!cboBaseStateID=" & temp_states_hold
                converted_where = converted_where & temp_states_hold

              Catch ex As Exception

              Finally
                adoTempRS2.Close()
              End Try

              ' good_results = False
              ' good_results_reason = good_results_reason & " STATES - HAVE TO MAKE FULL NAME, "
            Else
              converted_where = converted_where & "!~!cboBaseStateID=All"
            End If


            spot_1 = InStr(temp_string, "ac_aport_iata_code")
            If spot_1 > 0 Then
              aport_code_temp = find_data_for_field(temp_string, "ac_aport_iata_code", "")
              converted_where = converted_where & aport_code_temp
            End If

            spot_1 = InStr(temp_string, "ac_aport_icao_code")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_aport_icao_code", "")
            End If

            spot_1 = InStr(temp_string, "ac_ownership_type")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_ownership_type", "")
            End If


            spot_1 = InStr(temp_string, "ac_airframe_tot_hrs")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_airframe_tot_hrs", "")
            End If



            spot_1 = InStr(temp_string, "ac_exclusive_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_exclusive_flag", "")
            End If


            spot_1 = InStr(temp_string, "ac_status")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_status", "")
            End If

            spot_1 = InStr(temp_string, "ac_aport_country")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_aport_country", "cboBaseCountryID")
            Else
              converted_where = converted_where & "!~!cboBaseCountryID=All"
            End If

            us_ac_maintainted = ""
            spot_1 = InStr(temp_string, "ac_maintained")
            If spot_1 > 0 Then
              us_ac_maintainted = find_data_for_field(temp_string, "ac_maintained", "us_ac_maintained")
              us_ac_maintainted = Replace(us_ac_maintainted, "'", "SINGLE_TICK")
              converted_where = converted_where & us_ac_maintainted
              ' might need to do europe ac maintained differently ? 
            End If


            spot_1 = InStr(temp_string, "ac_previously_owned_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_previously_owned_flag", "")
            End If



            spot_1 = InStr(temp_string, "acfeat_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "acfeat_id", "")
            End If

            spot_1 = InStr(temp_string, "ac_purchase_date")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_purchase_date", "")
            End If

            spot_1 = InStr(temp_string, "ac_year")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_year", "")
            End If

            spot_1 = InStr(temp_string, "ac_passenger_count")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_passenger_count", "")
            End If

            spot_1 = InStr(temp_string, "ac_aport_city")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_aport_city", "")
            End If

            spot_1 = InStr(temp_string, "ac_ser_no_full")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_ser_no_full", "")
            End If


            spot_1 = InStr(temp_string, "ac_ser_no")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_ser_no", "")
            End If

            spot_1 = InStr(temp_string, "ac_maintained")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_maintained", "")
            End If

            spot_1 = InStr(temp_string, "ac_reg_no")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_reg_no", "")
            Else
              converted_where = converted_where & "!~!ac_reg_no_exact_match=false"
            End If

            spot_1 = InStr(temp_string, "ac_prev_reg_no")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_prev_reg_no", "")
              converted_where = converted_where & "!~!do_not_search_ac_prev_reg_no=true"
            Else
              converted_where = converted_where & "!~!do_not_search_ac_prev_reg_no=false"
            End If


            spot_1 = InStr(temp_string, "ac_alt_ser_no")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_alt_ser_no", "")
              converted_where = converted_where & "!~!do_not_search_ac_alt_ser_no=true"
            Else
              converted_where = converted_where & "!~!do_not_search_ac_alt_ser_no=false"
            End If


            spot_1 = InStr(temp_string, "ac_alt_ser_no_full")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_alt_ser_no_full", "")
              ' converted_where = converted_where & "!~!do_not_search_ac_alt_ser_no=true"
              'Else
              ' converted_where = converted_where & "!~!do_not_search_ac_alt_ser_no=false"
            End If


            spot_1 = InStr(temp_string, "ac_engine_management_prog_EMPG")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_engine_management_prog_EMPG", "")
            End If

            spot_1 = InStr(temp_string, "ac_engine_name_search")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "ac_engine_name_search", "")
            End If


            spot_1 = InStr(temp_string, "cpc_prod_code")
            If spot_1 > 0 Then
              small_cpc_fix = find_data_for_field(temp_string, "cpc_prod_code", "")

              small_cpc_fix = Replace(Trim(small_cpc_fix), " ", "")
              small_cpc_fix = Replace(Trim(small_cpc_fix), "cpc_prod_code='", "")
              small_cpc_fix = Replace(Trim(small_cpc_fix), "'", "")


              If InStr(Trim(small_cpc_fix), "B") > 0 Then
                small_cpc_fix = " comp_product_business_flag = 'Y' "
                converted_where = converted_where & "!~!comp_product_business_flag='Y' "
              ElseIf InStr(Trim(small_cpc_fix), "H") > 0 Then
                small_cpc_fix = " comp_product_helicopter_flag = 'Y' "
                converted_where = converted_where & "!~!comp_product_helicopter_flag='Y' "
              ElseIf InStr(Trim(small_cpc_fix), "C") > 0 Then
                small_cpc_fix = " comp_product_commercial_flag = 'Y' "
                converted_where = converted_where & "!~!comp_product_commercial_flag='Y' "
              End If

            End If
            ' 

            '--------------- AIRCRAFT SECTION---------------------------------


            '----------------- AV SECTION-------------------------------

            spot_1 = InStr(temp_string, "av_ac_journ_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "av_ac_journ_id", "")
            End If


            spot_1 = InStr(temp_string, "av_description")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "av_description", "")
            End If
            '----------------- AV SECTION-------------------------------


            '----------------- AC LEASE SECTION-------------------------------

            spot_1 = InStr(temp_string, "aclease_expiration_date")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "aclease_expiration_date", "")
            End If


            spot_1 = InStr(temp_string, "aclease_expired")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "aclease_expired", "")
            End If

            '----------------- AC LEASE SECTION-------------------------------




            '--------------- CREF SECTION---------------------------------
            spot_1 = InStr(temp_string, "cref_contact_type")
            If spot_1 > 0 Then
              contact_type = find_data_for_field(temp_string, "cref_contact_type", "")
              converted_where = converted_where & contact_type
            End If
            '--------------- CREF SECTION---------------------------------






            '--------------- COMPANY SECTION---------------------------------
            spot_1 = InStr(temp_string, "comp_product_helicopter_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_product_helicopter_flag", "")
            End If


            spot_1 = InStr(temp_string, "comp_product_business_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_product_business_flag", "")
            End If

            spot_1 = InStr(temp_string, "comp_product_commercial_flag")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_product_commercial_flag", "")
            End If

            ' this seems like it needs both above and below
            spot_1 = InStr(temp_string, "comp_country")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_country", "cboCompanyCountryID")
            End If

            temp_states_hold = ""
            spot_1 = InStr(temp_string, "comp_state")
            If spot_1 > 0 Then
              ' converted_where = converted_where & find_data_for_field(temp_string, "comp_state", "cboCompanyStateID")
              temp_states_hold = find_data_for_field(temp_string, "comp_state", "cboCompanyStateID")
              temp_states_hold = Replace(UCase(temp_states_hold), "!~!CBOCOMPANYSTATEID=", "")
              Try

                temp_Select_string = " select distinct state_code, state_name from state where state_active_flag = 'Y' and state_country = 'United States' "

                SqlCommand2.CommandText = temp_Select_string
                adoTempRS2 = SqlCommand2.ExecuteReader()

                If adoTempRS2.HasRows Then
                  Do While adoTempRS2.Read
                    temp_states_hold = Replace(temp_states_hold, Trim(adoTempRS2("state_code")), Trim(adoTempRS2("state_name")))
                  Loop
                End If

                temp_states_hold = "!~!cboCompanyStateID=" & temp_states_hold
                converted_where = converted_where & temp_states_hold

              Catch ex As Exception

              Finally
                adoTempRS2.Close()
              End Try
            End If

            spot_1 = InStr(temp_string, "comp_agency_type")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_agency_type", "")
            End If


            spot_1 = InStr(temp_string, "comp_timezone")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "comp_timezone", "")
            End If
            '--------------- COMPANY SECTION---------------------------------




            '------------- Feature Code Section--------------
            spot_1 = InStr(temp_string, "afeat_status_flag")
            If spot_1 > 0 Then
              If records_count = 56 Then
                records_count = records_count
              End If
              converted_where = converted_where & find_data_for_field(temp_string, "afeat_status_flag", "")
            End If

            spot_1 = InStr(temp_string, "afeat_journ_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "afeat_journ_id", "")
            End If
            '------------- Feature Code Section--------------




            '--------------- OTHER INCLUDED SECTION-----------
            spot_1 = InStr(temp_string, "emp_provider_name")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "emp_provider_name", "")
            End If

            spot_1 = InStr(temp_string, "bustypref_type")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "bustypref_type", "")
            End If

            spot_1 = InStr(temp_string, "adet_deta_description")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "adet_deta_description", "")
            End If


            spot_1 = InStr(temp_string, "adet_data_description")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "adet_data_description", "")
            End If


            spot_1 = InStr(temp_string, "adet_journ_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "adet_journ_id", "")
            End If

            spot_1 = InStr(temp_string, "priorev_category_code")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "priorev_category_code", "") ' do not add in , just remove
            End If

            spot_1 = InStr(temp_string, "priorev_entry_date")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "priorev_entry_date", "")
            End If

            spot_1 = InStr(temp_string, "adoc_infavor_comp_id")
            If spot_1 > 0 Then
              converted_where = converted_where & find_data_for_field(temp_string, "adoc_infavor_comp_id", "")
            End If




            spot_1 = InStr(temp_string, "ac_use_code")
            If spot_1 > 0 Then
              find_data_for_field(temp_string, "ac_use_code", "")
            End If
            '--------------- OTHER INCLUDED SECTION-----------





            '------------------------- IF IT IS AN EVENTS SEARCH - ADD IN THE STUFF FOR DAYS AND TIMES---------------
            If Trim(type_of_project) = "EventsCriteria" Then
              converted_where = converted_where & event_added_final
            End If
            '------------------------- IF IT IS AN EVENTS SEARCH - ADD IN THE STUFF FOR DAYS AND TIMES---------------

            '------------- To Do - Maybe By Hand-----------------------------
            'Serial # from: ac_ser_no_from
            'Serial # to: ac_ser_no_to
            'Do Not Search Alt Ser #: do_not_search_ac_alt_ser_no
            'Don't Search Prev. Reg #: do_not_search_ac_prev_reg_no
            'Reg #: ac_reg_no
            'Exact Match Reg #: ac_reg_no_exact_match


            'History Fields Only: 
            '                    Retail(Activity) : transaction_retail()
            'Type:               journ_subcat_code_part1()
            '(from) Operator: journ_subcat_code_part2_operator
            '(from): journ_subcat_code_part2
            '(to) Operator: journ_subcat_code_part3_operator
            '(to): journ_subcat_code_part3

            'Sale of New Aircraft Only: journ_newac_flag
            'Sale of Used Aircraft Only: jcat_used_retail_sales_flag

            'Date Operator: journ_date_operator
            'Date: journ_date

            'Ac Fields Only:
            '                    market(Status) : market()
            'Lifecycle:          ac_lifecycle_stage()
            'Ownership:          ac_ownership_type()
            '                    Previously(Owned) : ac_previously_owned_flag()
            '                    Lease(Status) : lease_status()


            '------------- To Do - Maybe By Hand-----------------------------





            '--------------------------- BRILLIANCE - CUTTING EACH FUNCTION------------------


            '----------- GET RID OF ALL LEFT OVER ( ) AND and OR !-------------
            temp_string = Replace(temp_string, ")", "")
            temp_string = Replace(temp_string, "(", "")
            temp_string = Replace(temp_string, "AND", "")
            temp_string = Replace(temp_string, "OR", "")
            temp_string = Replace(temp_string, "!", "")
            temp_string = Replace(temp_string, " ", "")
            temp_string = Replace(temp_string, "ND", "")
            converted_where = Replace(converted_where, "'", "")

            ' ANyWHERE IT NEEDS TO BE A TICK IN THERE == PUT IN SINGLE_TICK AND THEN THIS
            converted_where = Replace(converted_where, "SINGLE_TICK", "''")
            '----------- GET RID OF ALL LEFT OVER ( ) AND and OR -------------





            If InStr(Trim(temp_string), "comp_name_searchLIKE'") > 0 Then
              temp_string = Trim(temp_string)
              temp_string = Replace(Trim(temp_string), "comp_name_searchLIKE'", "")
              spot_1 = InStr(temp_string, "'")
              temp_string2 = Left(temp_string, spot_1 - 1)
              temp_string2 = Replace(temp_string2, "%", "")


              temp_string = Replace(temp_string, """", "")
              temp_string = Replace(temp_string, "'", "")
              temp_string = Replace(temp_string, "%", "")
              temp_string = Replace(temp_string, temp_string2, "")

              converted_where = converted_where & "!~!comp_name=" & temp_string2
            End If



            ' this is to trim the leading !~! off of the insert
            If Left(Trim(converted_where), 3) = "!~!" Then
              converted_where = "ZZZZZ" & Trim(converted_where)
              converted_where = Replace(Trim(converted_where), "ZZZZZ!~!", "")
            End If

            records_count = records_count + 1

            temp_string = Replace(temp_string, "NOT", "")  ' replace the leftover nots

            If Trim(temp_string) <> "" Then
              good_results = False
              good_results_reason = good_results_reason & " ITEMS LEFT IN TEMP STRING, "
            End If


            If InStr(UCase(converted_where), "LIKE") > 0 Then
              good_results = False
              good_results_reason = good_results_reason & " LIKE STATEMENT, "
            End If

            ' ignore this for event searches due to "in status" items 
            If Trim(type_of_project) = "AircraftCriteria" Then
              If InStr(UCase(converted_where), " IN ") > 0 Then
                good_results = False
                good_results_reason = good_results_reason & " IN CLAUSE2, "
                'ElseIf InStr(airframe_type_code, "##") > 0 Then
                '    good_results = True
              End If
            End If
            '   tmpQuery2 = tmpSelect & " " & tmpFrom & " " & temp_string & " " & tmpOrderby

            If Trim(converted_where) <> "" Then



              ' BLOCKS THAT WILL LATER BE REMOVED------------------

              ' currently anything with multiple anything would not work 

              ' If InStr(converted_where, "##") > 0 Then
              '  good_results = False
              ' good_results_reason = good_results_reason & " END MULTIPLE CHECK, "
              'End If

              ' currently anything with soemthing left in the string is not right 


              '' if there is no amod id in the list then its currently not ready
              '   If Trim(amod_id_2) = "" Then
              'good_results = False
              '    good_results_reason = good_results_reason & " NO AMOD ID, "
              'ElseIf InStr(amod_id_2, "##") > 0 Then
              '    good_results = True
              '   End If


              'If Trim(airframe_type_code) = "" Then
              '    good_results = False
              '    good_results_reason = good_results_reason & " airframe_type_code, "
              '    'ElseIf InStr(airframe_type_code, "##") > 0 Then
              '    '    good_results = True
              'End If



              'If Trim(amod_make_name) = "" Then
              '    good_results = False
              '    good_results_reason = good_results_reason & " amod_make_name, "
              'ElseIf InStr(amod_make_name, "##") > 0 Then
              '    good_results = True
              'End If


              'If Trim(amod_model_name) = "" Then
              '    good_results = False
              '    good_results_reason = good_results_reason & " amod_model_name, "
              'ElseIf InStr(amod_model_name, "##") > 0 Then
              '    good_results = True
              'End If

              ' BLOCKS THAT WILL LATER BE REMOVED------------------

              If InStr(UCase(temp_subject), "MIDDLE EAST") > 0 Then
                converted_where = converted_where
              End If

              If InStr(UCase(temp_subject), "MIDWEST") > 0 Then
                converted_where = converted_where
              End If

              If InStr(UCase(temp_subject), "EURO") > 0 Then
                converted_where = converted_where
              End If


              If InStr(UCase(converted_where), "BETWEEN") > 0 Then
                good_results = False
                good_results_reason = good_results_reason & " BETWEEN STATEMENT, "
              End If




              If good_results = True Then
                ' Response.Write(temp_insert & "<br><br>")
                'Response.Write(converted_where & "<br><br>")


                'If InStr(temp_insert, "Dan Rich") > 0 Then
                '    temp_insert = temp_insert
                'End If

                'If InStr(temp_insert, "gIV and gIVSP for sale and lear") > 0 Then
                '    temp_insert = temp_insert
                'End If

                'If InStr(temp_insert, "TRB Class I Jets") > 0 Then
                '    temp_insert = temp_insert
                'End If


                good_count = good_count + 1


                If Trim(type_of_project) = "AircraftCriteria" Then


                  If Trim(Replace(original_temp_string, "!", "")) <> "" Then

                    Try

                      count_of_report = 0
                      select_count_report = tmpSelect
                      If InStr(UCase(original_temp_string), "COMP") = 0 And InStr(UCase(original_temp_string), "CREF_CONTACT_TYPE") = 0 Then
                        select_count_report = select_count_report & "From " & tmpFrom & "  with (nolock) WHERE ac_id > 0 "
                      Else
                        select_count_report = select_count_report & "From View_Aircraft_Company_Flat  with (nolock) WHERE ac_id > 0 "
                      End If

                      original_temp_string = Replace(original_temp_string, "cpc_prod_code = 'B'", small_cpc_fix)
                      original_temp_string = Replace(original_temp_string, "cpc_prod_code = 'C'", small_cpc_fix)
                      original_temp_string = Replace(original_temp_string, "cpc_prod_code = 'H'", small_cpc_fix)


                      select_count_report = select_count_report & Replace(original_temp_string, "!", "")
                      select_count_report = select_count_report & tmpOrderby

                      SqlCommand2.CommandText = select_count_report
                      adoTempRS2 = SqlCommand2.ExecuteReader()

                      If adoTempRS2.HasRows Then
                        Do While adoTempRS2.Read
                          count_of_report = count_of_report + 1
                        Loop
                      End If



                    Catch ex As Exception
                      count_of_report = 1111111
                    Finally
                      adoTempRS2.Close()
                    End Try
                  Else
                    count_of_report = 15001
                    ' make the report to big, if there is no criteria to select 
                  End If

                  If count_of_report > 15000 Then
                    count_of_report = count_of_report
                    good_results = False
                    good_results_reason = good_results_reason & ", TOO MANY RECORDS - SOMETHING PROBABLY WRONG "
                  ElseIf count_of_report = 0 Then
                    good_results = False
                    good_results_reason = good_results_reason & ", 0 Reports  "
                  End If

                ElseIf Trim(type_of_project) = "EventsCriteria" Then

                  Try

                    count_of_report = 0
                    select_count_report = tmpSelect

                    select_count_report = select_count_report & "From " & tmpFrom & "  with (nolock) "
                    select_count_report = select_count_report & temp_event_inner
                    select_count_report = select_count_report & " WHERE amod_id > 0 "

                    select_count_report = select_count_report & Replace(original_temp_string, "!", "")
                    select_count_report = select_count_report & tmpOrderby

                    ' SqlCommand2.CommandText = select_count_report
                    'adoTempRS2 = SqlCommand2.ExecuteReader()

                    'If adoTempRS2.HasRows Then
                    '    Do While adoTempRS2.Read
                    '        count_of_report = count_of_report + 1
                    '    Loop
                    'End If


                  Catch ex As Exception
                    count_of_report = 1111111
                  Finally
                    ' adoTempRS2.Close()
                  End Try

                  'If count_of_report > 15000 Then
                  '    count_of_report = count_of_report
                  '    good_results = False
                  '    good_results_reason = good_results_reason & ", TOO MANY RECORDS - SOMETHING PROBABLY WRONG "
                  'ElseIf count_of_report = 0 Then
                  '    good_results = False
                  '    good_results_reason = good_results_reason & ", 0 Reports  "
                  'End If
                End If



                ' If count_of_report = 0 Then
                'Response.Write("<br><br>REPORT WITH 0: " & select_count_report & "<br>")
                '  End If

                If InStr(UCase(select_count_report), "IN (SELECT") > 0 Then
                  temp_sub_select_section = select_count_report

                  spot_1 = InStr(UCase(temp_sub_select_section), "IN (SELECT")
                  ' get everything to the right of it 
                  temp_sub_select_section = Right(temp_sub_select_section, Len(temp_sub_select_section) - spot_1)

                  ' get everything to the right of it 
                  spot_1 = InStr(UCase(temp_sub_select_section), "SELECT")
                  temp_sub_select_section = Right(temp_sub_select_section, Len(temp_sub_select_section) - spot_1 + 1)


                  spot_1 = InStr(UCase(temp_sub_select_section), "(")
                  spot_2 = InStr(UCase(temp_sub_select_section), ")")
                  If spot_1 = 0 Or (spot_1 > spot_2) Then
                    ' if there is no more ( or there is one but it is after the close ) then we are done  
                    temp_sub_select_section = Left(temp_sub_select_section, spot_2 - 1)
                  Else
                    ' otherwise, needs more chopping 
                    temp_select_hold = Left(temp_sub_select_section, spot_1) ' get everything from the left to the next )
                    temp_sub_select_section = Right(temp_sub_select_section, Len(temp_sub_select_section) - spot_1) ' cut down to everything to the right of (
                    spot_1 = InStr(UCase(temp_sub_select_section), "(")
                    spot_2 = InStr(UCase(temp_sub_select_section), ")")

                    'if there is no end ) or there is another ( before the next ) then too much to do for now 
                    If spot_2 = 0 Or ((spot_1 < spot_2) And spot_1 > 0) Then
                      temp_sub_select_section = temp_sub_select_section ' too many ()()()() for now 
                    Else
                      temp_select_hold = temp_select_hold & Left(temp_sub_select_section, spot_2 - 1) ' get from next ) 
                      temp_sub_select_section = Right(temp_sub_select_section, Len(temp_sub_select_section) - spot_2 + 1)
                      spot_2 = InStr(UCase(temp_sub_select_section), ")")
                      If spot_2 > 0 Then
                        temp_select_hold = temp_select_hold & Left(temp_sub_select_section, spot_2)
                      Else
                        temp_sub_select_section = temp_sub_select_section ' too many ()()()() for now  
                      End If

                      temp_sub_select_section = temp_select_hold
                    End If
                  End If

                  'use the query to get results -------------------------------------------------------------------
                  If Trim(temp_sub_select_section) <> "" Then
                    Try


                      spot_1 = InStr(UCase(temp_sub_select_section), "FROM")
                      what_selecting = Left(temp_sub_select_section, spot_1 - 1)
                      what_selecting = Replace(UCase(what_selecting), "SELECT", "")
                      what_selecting = Replace(UCase(what_selecting), "DISTINCT", "")
                      what_selecting = Trim(what_selecting)

                      If InStr(UCase(temp_sub_select_section), "DISTINCT") = 0 Then
                        temp_Select_string = Replace(UCase(temp_sub_select_section), "SELECT", "SELECT DISTINCT ") ' just not to get duplicates
                        SqlCommand2.CommandText = Replace(UCase(temp_Select_string), ("DISTINCT " & UCase(what_selecting)), "COUNT(DISTINCT " & what_selecting & ") as tcount ")
                      Else
                        temp_Select_string = temp_sub_select_section
                        SqlCommand2.CommandText = Replace(UCase(temp_Select_string), ("DISTINCT " & UCase(what_selecting)), "COUNT(DISTINCT " & what_selecting & ") as tcount ")
                      End If

                      adoTempRS2 = SqlCommand2.ExecuteReader()

                      If adoTempRS2.HasRows Then
                        adoTempRS2.Read()
                        sub_select_count = adoTempRS2("tcount")
                      End If
                      adoTempRS2.Close()

                      SqlCommand2.CommandText = temp_Select_string

                      ' added in - in case huge select
                      If sub_select_count < 5000 Then
                        new_sub_select_string = ""
                        spot_3 = 0 ' use spot3 as temp counter 
                        If adoTempRS2.HasRows Then
                          Do While adoTempRS2.Read

                            If spot_3 = 0 Then
                              new_sub_select_string = new_sub_select_string & adoTempRS2("" & what_selecting & "")
                            Else
                              new_sub_select_string = new_sub_select_string & "##" & adoTempRS2("" & what_selecting & "")
                            End If

                            spot_3 = spot_3 + 1
                          Loop
                        End If
                      End If


                    Catch ex As Exception
                    Finally
                      adoTempRS2.Close()
                    End Try
                    If Trim(new_sub_select_string) <> "" Then
                      Replace_in_not(temp_sub_select_section) ' this should make it match select in converted where
                      If InStr(converted_where, temp_sub_select_section) > 0 Then
                        converted_where = Replace(converted_where, temp_sub_select_section, Replace(new_sub_select_string, "'", ""))
                      Else
                        good_results = False
                        good_results_reason = good_results_reason & ", (SUB SELECT THAT ISNT COUNTRY OR DOESNT REPLACE CORRECT)"
                      End If
                    End If
                  End If
                  'use the query to get results -------------------------------------------------------------------

                End If


                If good_results = True Then


                  '  -- if enough data
                  '  -- insert the folder
                  temp_insert = ""
                  temp_insert = temp_insert & " insert into Client_Folder(cfolder_cftype_id, cfolder_method, cfolder_name "
                  temp_insert = temp_insert & ", cfolder_description, cfolder_data, cfolder_sub_id, cfolder_login, cfolder_seq_no, cfolder_cliuser_id "
                  temp_insert = temp_insert & ", cfolder_share, cfolder_hide_flag, cfolder_entry_date, cfolder_update_date, cfolder_sort, cfolder_sissc_id "
                  temp_insert = temp_insert & ", cfolder_jetnet_run_flag, cfolder_jetnet_run_reply_username, cfolder_jetnet_run_reply_email, cfolder_jetnet_run_freq_in_mins) "

                  temp_insert = temp_insert & " VALUES ("
                  '               [cfolder_id] [int] IDENTITY(1,1) NOT NULL,

                  If Trim(type_of_project) = "EventsCriteria" Then
                    temp_insert = temp_insert & " 5 " '               [cfolder_cftype_id] [int] NULL, ' 3 = Aircraft
                  ElseIf Trim(type_of_project) = "MarketCriteria" Then
                    temp_insert = temp_insert & " 13 "
                  Else
                    temp_insert = temp_insert & " 3 " '               [cfolder_cftype_id] [int] NULL, ' 3 = Aircraft
                  End If




                  temp_insert = temp_insert & ", 'A' " '               [cfolder_method] [char](1) NULL,  ' A = Active
                  If good_results = True Then
                    '  If count_of_report > 0 Then
                    'temp_insert = temp_insert & ", '" & Replace_Bad(temp_subject) & " (" & count_of_report & ")' " '               [cfolder_name] [varchar](250) NULL, ' sissc_subject 
                    ' Else
                    temp_insert = temp_insert & ", '" & Replace_Bad(temp_subject) & " ' " '               [cfolder_name] [varchar](250) NULL, ' sissc_subject 
                    '  End If
                  Else
                    temp_insert = temp_insert & ", '(BAD REPORT - " & Replace_Bad(temp_subject) & " ): " & Replace_Bad(good_results_reason) & " ; " & Replace_Bad(known_bad_results) & "' " ' 
                    good_results = False
                    good_results_reason = good_results_reason & " BAD REPORT, "
                  End If
                  temp_insert = temp_insert & ", '" & Replace_Bad(temp_desc) & "' " '               [cfolder_description] [varchar](1500) NULL, ' sissc_description

                  ' this is for replacing starting space in ## location##location2##location3 
                  If InStr(converted_where, "## ") > 0 Then
                    converted_where = Replace(Trim(converted_where), "## ", "##")
                  End If


                  temp_insert = temp_insert & ", '" & converted_where & "!~!THEREALSEARCHQUERY=" & Replace_Bad_Tick_with_tick(select_count_report) & "' " '              [cfolder_data] [text] NULL, ' converted stuff

                  temp_insert = temp_insert & ",'" & Session.Item("localUser").crmSubSubID.ToString.Trim & "' " '    temp_sub            [cfolder_sub_id] [int] NULL, ' sissc_sub_id
                  temp_insert = temp_insert & ",'" & Session.Item("localUser").crmUserLogin.ToString.Trim & "' " '    temp_login            [cfolder_login] [char](15) NULL, ' sissc_login
                  temp_insert = temp_insert & ",'" & Session.Item("localUser").crmSubSeqNo.ToString.Trim & "' " '    temp_seq           [cfolder_seq_no] [smallint] NULL, ' sissc_seq_no
                  temp_insert = temp_insert & ", 0 " '               [cfolder_cliuser_id] [int] NULL, ' 0 - unknown
                  temp_insert = temp_insert & ", 'N' " '               [cfolder_share] [char](1) NULL, ' N = No
                  temp_insert = temp_insert & ", 'N' " '               [cfolder_hide_flag] [char](1) NOT NULL, ' N= No
                  temp_insert = temp_insert & ",'" & temp_edate & "' " '              [cfolder_entry_date] [datetime] NULL,  ' sissc_entry_date
                  temp_insert = temp_insert & ",'" & temp_udate & "' " '               [cfolder_update_date] [datetime] NULL, ' sissc_update_date
                  temp_insert = temp_insert & ", 1 " '               [cfolder_sort] [smallint] NOT NULL ' 1 = default
                  temp_insert = temp_insert & "," & project_id & " " '
                  temp_insert = temp_insert & ",'" & run_flag & "' " ' run flag
                  temp_insert = temp_insert & ",'" & run_user_name & "' " ' run user name 
                  temp_insert = temp_insert & ",'" & run_user_email & "' " '  run email
                  temp_insert = temp_insert & "," & run_date_time_minutes & " " '  run minutes 
                  temp_insert = temp_insert & " )"


                  '    Response.Write("ORIGINAL SELECT: " & original_temp_string & "<br><br>")
                  '    Response.Write(temp_insert & "<br><br>")
                  SqlCommand2.CommandText = temp_insert
                  SqlCommand2.ExecuteNonQuery()




                  tmpQuery = " Update Subscription_Install_Saved_Search_Criteria "
                  tmpQuery = tmpQuery & "  set sissc_convert_to_dotnet_flag = 'Y' "
                  tmpQuery = tmpQuery & " where sissc_id = '" & project_id & "' "
                  tmpQuery = tmpQuery & " and sissc_sub_id  = " & Session.Item("localUser").crmSubSubID.ToString.Trim & ""
                  tmpQuery = tmpQuery & " and sissc_login = '" & Session.Item("localUser").crmUserLogin.ToString.Trim & "' "
                  tmpQuery = tmpQuery & " and sissc_seq_no = '" & Session.Item("localUser").crmSubSeqNo.ToString.Trim & "' "
                  tmpQuery = tmpQuery & " and sissc_convert_to_dotnet_flag = 'N' "

                  tmpQuery = tmpQuery

                  SqlCommand2.CommandText = tmpQuery
                  SqlCommand2.ExecuteNonQuery()



                Else
                  ' this will only go here is there is a fail in count number
                  '     Response.Write("BAD BECAUSE: " & good_results_reason & "<br>")
                  '    Response.Write("KNOW BAD - CANT FIX CURRENTLY: " & known_bad_results & "<br><br>")
                  '     Response.Write("LEFT TO DO: " & temp_string & "<br>")
                  '     Response.Write("CONVERTED: " & converted_where & "<br>")
                  '    Response.Write("ORIGINAL SELECT: " & original_temp_string & "<br><br>")
                  bad_count = bad_count + 1
                  good_count = good_count - 1
                  projects_not_converted = projects_not_converted & Trim(temp_subject) & "<br>"
                End If


              Else

                ' If InStr(UCase(original_temp_string), "NOT") Then
                ' Response.Write("ORIGINAL SELECT: " & original_temp_string & "<br><br>")
                'End If

                '  Response.Write("BAD BECAUSE: " & good_results_reason & "<br>")
                ' Response.Write("KNOW BAD - CANT FIX CURRENTLY: " & known_bad_results & "<br><br>")
                '  Response.Write("LEFT TO DO: " & temp_string & "<br>")
                '  Response.Write("CONVERTED: " & converted_where & "<br>")
                '  Response.Write("ORIGINAL SELECT: " & original_temp_string & "<br><br>")
                bad_count = bad_count + 1
                projects_not_converted = projects_not_converted & Trim(temp_subject) & "<br>"
              End If





            Else
              '  Response.Write("NOTHING<br><br>")
            End If

            ' SqlCommand2.CommandText = temp_insert
            ' SqlCommand2.ExecuteNonQuery()


            'If Trim(temp_string) <> "" Then
            '    Response.Write(records_count & "-")
            '    Response.Write("LEFT TO DO: " & temp_string & "<br>")
            'End If


            'If Trim(temp_string) <> "" Or Trim(converted_where) <> "" Then
            '    Response.Write(records_count & "-")
            '    Response.Write("LEFT TO DO: " & temp_string & "<br>")

            '    Response.Write("CONVERTED: " & converted_where)
            '    Response.Write("<Br><br>")
            'End If


            converted_where = ""

          End If  ' for has been converted


        Loop
      End If


      Me.results_label.Text = "PROJECTS CONVERTED: " & good_count & "<Br>"
      Me.results_label.Text += "PROJECTS NOT CONVERTED: " & bad_count & "<Br>"
      Me.results_label.Text += projects_not_converted & "<Br>"
      Me.results_label.Visible = True

      'Response.Write("TOTAL RECORDS: " & records_count & "<BR>")


      'adoRSAircraft.Dispose()
      'adoTempRS.Dispose()
      'adoTempRS2.Dispose()
      adoRSAircraft = Nothing
      adoTempRS = Nothing
      adoTempRS2 = Nothing



    Catch ex As Exception
      ' aCommonEvo.DisplayAlert("Error in btnRunReport_Click: " & ex.Message)
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlConn2.Close()
      SqlConn2.Dispose()
      SqlConn2 = Nothing
    End Try

  End Function


  Public Function get_evo_field_names(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader) As String
    get_evo_field_names = ""

    Dim tmpQuery3 As String = ""

    Try


      tmpQuery3 = " Select distinct cef_evo_field_name from custom_Export_fields with (nolock) where cef_advanced_search_flag = 'Y' "

      SqlCommand2.CommandText = tmpQuery3
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read

          If Not IsDBNull(adoTempRS2("cef_evo_field_name")) Then
            If Trim(adoTempRS2("cef_evo_field_name")) <> "" Then
              If Trim(get_evo_field_names) = "" Then
                get_evo_field_names = adoTempRS2("cef_evo_field_name")
              Else
                get_evo_field_names = get_evo_field_names & "," & adoTempRS2("cef_evo_field_name")
              End If
            End If
          End If

        Loop
      End If

    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function
  Public Function get_control_name(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByVal field_name As String, ByVal compares As String, ByVal value1 As String) As String
    get_control_name = ""

    Dim tmpQuery3 As String = ""

    Try


      tmpQuery3 = " Select * from custom_Export_fields  with (nolock) where cef_evo_field_name = '" & field_name & "' and cef_advanced_search_flag = 'Y' "

      SqlCommand2.CommandText = tmpQuery3
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read

          '  get_control_name = "!~!COMPARE-" & adoTempRS2("cef_field_type") & "-" & adoTempRS2("cef_id") & "-" & adoTempRS2("cef_evo_field_name") & "=" & compares
          '  get_control_name = get_control_name & "!~!" & adoTempRS2("cef_field_type") & "-" & adoTempRS2("cef_id") & "-" & adoTempRS2("cef_evo_field_name") & "=" & value1 & ""
          get_control_name = "!~!COMPARE_" & adoTempRS2("cef_evo_field_name") & "=" & Trim(compares)
          get_control_name = get_control_name & "!~!" & adoTempRS2("cef_evo_field_name") & "=" & Trim(value1) & ""


        Loop
      End If

    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function

  Public Function find_data_for_field(ByRef temp_string_to_change As String, ByVal string_to_look_for As String, ByVal control_name As String)
    find_data_for_field = ""
    Dim spot1 As Integer = 0
    Dim spot2 As Integer = 0
    Dim spot3 As Integer = 0
    Dim spot4 As Integer = 0
    Dim local_temp_string As String = ""
    Dim original_temp_string As String = ""
    Dim i As Integer = 0
    Dim value_of As String = ""
    Dim odd_testing As String = ""
    Dim before_chop_string As String = ""
    Dim last_value_of As String = ""
    Dim yes_no_switch As Boolean = False
    Dim sub_select_addition As String = ""


    original_temp_string = temp_string_to_change

    Try




      For i = 0 To 500

        If i > 25 Then
          i = i
        End If
        If Trim(local_temp_string) <> "" Then
          temp_string_to_change = local_temp_string
        End If

        spot1 = InStr(LCase(temp_string_to_change), string_to_look_for)

        If spot1 > 0 Then

          before_chop_string = temp_string_to_change

          spot2 = InStr(spot1, temp_string_to_change, ")")
          spot3 = InStr(spot1, temp_string_to_change, "AND ")
          spot4 = InStr(spot1, temp_string_to_change, " OR ")

          If (spot3 = 0) And (spot2 = 0) And (spot4 = 0) Then
            spot2 = InStr(spot1, temp_string_to_change, "!")
          ElseIf (spot3 < spot2 And (spot3 > 0)) Or (spot2 = 0) Then
            spot2 = spot3
          ElseIf (spot4 < spot2 And (spot4 > 0)) Or (spot2 = 0) Then
            spot2 = spot4
          End If


          local_temp_string = Left(temp_string_to_change, spot2 - 1)
          local_temp_string = Right(local_temp_string, Len(local_temp_string) - spot1 + 1)
          spot3 = InStr(local_temp_string, "=")
          If spot3 = 0 Then
            spot3 = InStr(UCase(local_temp_string), " IN ")
            If spot3 > 0 Then
              spot3 = spot3 - 1
              ' good_results = False
              'good_results_reason = good_results_reason & " IN STATEMENT, "
            Else
              spot3 = InStr(UCase(local_temp_string), " LIKE ")
              If spot3 > 0 Then
                spot3 = spot3 + 5
              End If
            End If
          End If

          value_of = local_temp_string


          If InStr(value_of, "SELECT") > 0 Then

            value_of = decode_value_of(before_chop_string, value_of, string_to_look_for, "SELECT", spot1, spot2)


            sub_select_addition = ""

            If InStr(LCase(value_of), "key_feature") > 0 Or InStr(LCase(value_of), "aircraft_avionics") > 0 Or InStr(LCase(value_of), "aircraft_details") > 0 Then
              value_of = dissect_special_selects(value_of, sub_select_addition, spot2, before_chop_string)
              'spot2 = spot2 + spot1 + Len(string_to_look_for)
            End If

            ' everything to the left of where it starts
            local_temp_string = Left(temp_string_to_change, spot1 - 1)

            local_temp_string = local_temp_string & Right(temp_string_to_change, Len(temp_string_to_change) - spot2 + 1)
            ' good_results = False
            ' good_results_reason = good_results_reason & " SELECT STATEMENT, "


          ElseIf InStr(value_of, "BETWEEN") > 0 Then

            value_of = decode_value_of(before_chop_string, value_of, string_to_look_for, "BETWEEN", spot1, spot2)

            ' everything to the left of where it starts
            local_temp_string = Left(temp_string_to_change, spot1 - 1)

            local_temp_string = local_temp_string & Right(temp_string_to_change, Len(temp_string_to_change) - spot2 + 1)
            good_results = False
            good_results_reason = good_results_reason & " BETWEEN STATEMENT, "
          Else
            local_temp_string = Right(local_temp_string, Len(local_temp_string) - spot3)
            value_of = local_temp_string
            ' everything to the left of where it starts
            local_temp_string = Left(temp_string_to_change, spot1 - 1)

            local_temp_string = local_temp_string & Right(temp_string_to_change, Len(temp_string_to_change) - spot2 + 1)

          End If


          Replace_in_not(value_of)




          '------------- SPECIAL CASES----------------------------------------------------

          yes_no_switch = False

          '---------- fOR YES NO REPLACES TO TRUE FALSE -----------------
          '


          If (Trim(control_name) = "ac_product_helicopter_flag" Or Trim(string_to_look_for) = "ac_product_helicopter_flag") Then
            yes_no_switch = True
          ElseIf (Trim(control_name) = "ac_product_business_flag" Or Trim(string_to_look_for) = "ac_product_business_flag") Then
            yes_no_switch = True
          ElseIf (Trim(control_name) = "ac_product_commercial_flag" Or Trim(string_to_look_for) = "ac_product_commercial_flag") Then
            yes_no_switch = True
          End If

          If yes_no_switch = True Then
            If Trim(value_of) = "'Y'" Then
              value_of = "true"
            ElseIf Trim(value_of) = "'F'" Then
              value_of = "false"
            End If
          End If


          If (Trim(control_name) = "ac_forsale_flag" Or Trim(string_to_look_for) = "ac_forsale_flag") Then
            If Trim(value_of) = "'Y'" Then
              market_status = "For Sale"
            ElseIf Trim(value_of) = "'N'" Then
              market_status = "Not For Sale"
            End If
          End If
          '---------- fOR YES NO REPLACES TO TRUE FALSE -----------------






          '------------- SPECIAL CASES----------------------------------------------------


          ' if it is a sub select, then add that to the converted list and not the other
          If Trim(sub_select_addition) <> "" Then
            find_data_for_field = find_data_for_field & sub_select_addition
          Else

            If (Trim(value_of) <> Trim(last_value_of)) And Trim(last_value_of) <> "" Then
              find_data_for_field = find_data_for_field & "##" & value_of

              ' will need to change for it to work for multiples
              If Trim(LCase(string_to_look_for)) = "amod_model_name" Then
                amod_model_name = amod_model_name & "##" & Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_make_name" Then
                amod_make_name = amod_make_name & "##" & Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_id" Then
                amod_id_2 = amod_id_2 & "##" & Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_type_code" Then
                type_code = type_code & "##" & Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_airframe_type_code" Then
                airframe_type_code = airframe_type_code & "##" & Trim(value_of)
              End If

            Else


              If Trim(control_name) <> "" Then
                find_data_for_field = "!~!" & Trim(control_name) & "=" & Trim(value_of)
              Else
                find_data_for_field = "!~!" & Trim(string_to_look_for) & "=" & Trim(value_of)
              End If

              ' if its reg no, then check for %
              If (Trim(control_name) = "ac_reg_no" Or Trim(string_to_look_for) = "ac_reg_no") Then
                If InStr(value_of, "%") > 0 Then
                  find_data_for_field = find_data_for_field & "!~!ac_reg_no_exact_match=false"
                  find_data_for_field = Replace(find_data_for_field, "%", "")
                Else
                  find_data_for_field = find_data_for_field & "!~!ac_reg_no_exact_match=true"
                End If
              End If



              If Trim(LCase(string_to_look_for)) = "amod_model_name" Then
                amod_model_name = Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_make_name" Then
                amod_make_name = Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_id" Then
                amod_id_2 = Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_type_code" Then
                type_code = Trim(value_of)
              ElseIf Trim(LCase(string_to_look_for)) = "amod_airframe_type_code" Then
                airframe_type_code = Trim(value_of)
              End If
            End If
          End If


          last_value_of = Trim(value_of)


        Else
          i = i + 550
        End If

      Next



      ' if it has gotten through the cycle and there is still more to go 
      If i < 150 Then

      End If





      If InStr(control_name, "comp_country") > 0 Or InStr(string_to_look_for, "comp_country") > 0 Then
        control_name = control_name
      End If


      If Trim(sub_select_addition) <> "" Then
        ' nothing to do if its a sub select, ut no neeed for replace
      Else
        If Trim(control_name) <> "" Then
          find_data_for_field = Replace(find_data_for_field, (" OR " & control_name & " = "), "##")
        Else
          find_data_for_field = Replace(find_data_for_field, (" OR " & string_to_look_for & " = "), "##")
        End If
      End If



      If Trim(local_temp_string) <> "" Then
        temp_string_to_change = local_temp_string
      Else
        temp_string_to_change = original_temp_string
      End If





      'Response.Write("<br>--" & local_temp_string & "<br>")
    Catch ex As Exception

    End Try

  End Function
  Public Sub Replace_in_not(ByRef value_of As String)

    If InStr(UCase(value_of), "NOT IN") > 0 Then
      value_of = Replace(value_of, "NOT IN (", "")
      value_of = Replace(value_of, "NOT IN(", "")
      value_of = Replace(value_of, "not in (", "")
      value_of = Replace(value_of, "not in(", "")
      value_of = Replace(value_of, ",", "##")
      value_of = Replace(value_of, "(", "")
      value_of = Replace(value_of, "'", "")
      value_of = Trim(value_of)
      good_results = False
      good_results_reason = good_results_reason & " NOT IN CLAUSE, "
    ElseIf InStr(UCase(value_of), "IN") > 0 Then
      value_of = Replace(value_of, "IN (", "")
      value_of = Replace(value_of, "IN(", "")
      value_of = Replace(value_of, "in (", "")
      value_of = Replace(value_of, "in(", "")
      value_of = Replace(value_of, "In (", "")
      value_of = Replace(value_of, "In(", "")
      value_of = Replace(value_of, "iN (", "")
      value_of = Replace(value_of, "iN(", "")
      value_of = Replace(value_of, ",", "##")
      value_of = Replace(value_of, "(", "")
      value_of = Replace(value_of, "'", "")
      value_of = Trim(value_of)
      'good_results = False
      ' good_results_reason = good_results_reason & " IN CLAUSE, "
    End If

  End Sub
  Public Function Replace_Bad_Tick_with_tick(ByRef temp_string11 As String) As String
    temp_string11 = Replace(temp_string11, "'", "''")
    Return temp_string11
  End Function
  Public Function Replace_Bad(ByRef temp_string11 As String) As String
    temp_string11 = Replace(temp_string11, "'", "")
    Return temp_string11
  End Function


  Public Function dissect_special_selects(ByVal temp_string1 As String, ByRef conv_where As String, ByVal spot_2 As Integer, ByVal original_string As String) As String
    dissect_special_selects = ""

    Dim spot_1 As Integer = 0
    Dim temp_s2 As String = ""
    Dim y_or_n As String = "N"
    Dim field_name As String = ""

    Try



      temp_s2 = temp_string1

      spot_1 = InStr(LCase(temp_string1), "from aircraft_key_feature")
      If spot_1 > 0 Then
        temp_string1 = LCase(temp_string1)
        spot_1 = InStr(LCase(temp_string1), "afeat_feature_code")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        field_name = Left(temp_string1, spot_1)

        ' set original
        temp_string1 = original_string
        spot_1 = InStr(LCase(temp_string1), "afeat_status_flag")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        y_or_n = Left(temp_string1, spot_1)


        conv_where = conv_where & "!~!ac_feat_" & field_name & "='" & y_or_n & "'"
        ' spot_2 = InStr(LCase(temp_s2), temp_string1) + 1
      End If

      spot_1 = InStr(LCase(temp_string1), "from aircraft_avionics")
      If spot_1 > 0 Then
        temp_string1 = LCase(temp_string1)
        spot_1 = InStr(LCase(temp_string1), "av_name")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        temp_string1 = Right(temp_string1, Len(temp_string1) - spot_1)
        spot_1 = InStr(LCase(temp_string1), "'")
        field_name = Left(temp_string1, spot_1)

        conv_where = conv_where & "!~!av_avionics_" & field_name & "='" & y_or_n & "'"
        'spot_2 = InStr(LCase(temp_s2), temp_string1) + 1
      End If


      spot_1 = InStr(LCase(temp_string1), "from aircraft_details")
      If spot_1 > 0 Then
        spot_1 = spot_1
      End If


      'spot_1 = InStr(LCase(temp_string1), "from aircraft_key_feature ejrtr afeat_feature_code = 'rvs' ")
      'If spot_1 > 0 Then
      '    conv_where = conv_where & "!~!ac_feat_rvs='Y'"
      '    spot_2 = spot_2 + InStr(LCase(temp_string1), "rvs'") - 1
      'End If

      'spot_1 = InStr(LCase(temp_string1), "from aircraft_avionics where av_name = 'adf'")
      'If spot_1 > 0 Then
      '    conv_where = conv_where & "!~!av_avionics_adf='Y'"
      '    spot_2 = spot_2 + InStr(LCase(temp_string1), "adf'") - 1
      'End If

      'spot_1 = InStr(LCase(temp_string1), "from aircraft_avionics where av_name = 'afis'")
      'If spot_1 > 0 Then
      '    conv_where = conv_where & "!~!ac_avionics_afis='Y'"
      '    spot_2 = spot_2 + InStr(LCase(temp_string1), "afis'") - 1
      'End If

      ''spot_1 = InStr(LCase(temp_string1), "from aircraft_avionics where av_name = 'afis'")
      ''If spot_1 > 0 Then
      ''    conv_where = conv_where & "!~!ac_avionics_package='Y'" 
      ''End If



      ''spot_1 = InStr(LCase(temp_string1), "av_description")
      ''If spot_1 > 0 Then
      ''    conv_where = conv_where & "!~!ac_avionics_package='Y'"
      ''End If

      'If InStr(LCase(temp_string1), "from aircraft_details where afeat_feature_code = 'rvs' and afeat_status_flag = 'y'") > 0 Then
      '    conv_where = conv_where & "!~!ac_avionics_package='Y'"
      '    spot_2 = spot_2 + InStr(LCase(temp_string1), "'y''") - 1
      'End If

      'If InStr(LCase(temp_string1), "from aircraft_details where adet_data_type = 'interior' and adet_data_name = 'lavatory'") > 0 Then
      '    conv_where = conv_where & "!~!ac_det_int_lavatory='Y'"
      '    spot_2 = spot_2 + InStr(LCase(temp_string1), "tory'") - 1
      'End If

      dissect_special_selects = temp_string1


    Catch ex As Exception

    End Try


  End Function
  Public Function decode_value_of(ByVal original_string As String, ByVal temp_val As String, ByVal search_string As String, ByVal odd_type As String, ByRef spot1 As Integer, ByRef spot2 As Integer) As String
    decode_value_of = ""


    Dim spot3 As Integer = 0
    Dim local_string As String = ""
    Dim real_value As String = ""
    Dim temp_add_nolock As String = ""

    If Trim(odd_type) = "SELECT" Then
      spot1 = InStr(original_string, search_string)

      spot3 = InStr(original_string, "(NOLOCK)")
      If spot3 > 0 Then
        original_string = Replace(original_string, "(NOLOCK)", "NOLOCK")
      End If
      spot2 = InStr(spot1, original_string, ")")

      If spot2 > 0 Then
        local_string = Left(original_string, spot2)
        local_string = Right(local_string, Len(local_string) - spot1 + 1)
        local_string = Replace(local_string, search_string, "")
        If spot3 > 0 Then
          local_string = Replace(local_string, "NOLOCK", "(NOLOCK)")
          spot2 = spot2 + 2
        End If
      Else
        spot2 = spot2
      End If

    ElseIf Trim(odd_type) = "BETWEEN" Then
      spot1 = InStr(original_string, search_string)
      spot2 = InStr(spot1, original_string, ")")
      If spot2 > 0 Then
        local_string = Left(original_string, spot2 - 1)
        local_string = Right(local_string, Len(local_string) - spot1 + 1)
        local_string = Replace(local_string, search_string, "")
        local_string = Replace(local_string, odd_type, "")
        local_string = Replace(local_string, "AND", ";")
      Else
        local_string = local_string
      End If
    End If

    real_value = local_string

    decode_value_of = real_value
  End Function

  Public Function get_amod_id_model_name(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByVal amod_make2 As String, ByVal amod_model2 As String) As String
    get_amod_id_model_name = ""

    Dim tmpQuery3 As String = ""
    Dim counter111 As Integer = 0

    Try

      ' if there is ticks in there already then dont replace with ticks
      If InStr(amod_model2, "'") > 0 Then
        amod_model2 = Replace(amod_model2, "'", "")
      End If

      If InStr(amod_make2, "'") > 0 Then
        amod_make2 = Replace(amod_make2, "'", "")
      End If

      amod_model2 = Replace(amod_model2, "##", "','")
      amod_make2 = Replace(amod_make2, "##", "','")

      tmpQuery3 = " Select distinct amod_id from aircraft_model where amod_id > 0 "
      If Trim(amod_model2) <> "" Then
        tmpQuery3 = tmpQuery3 & " and amod_model_name in ('" & amod_model2 & "') "
      End If
      If Trim(amod_make2) <> "" Then
        tmpQuery3 = tmpQuery3 & " and amod_make_name in('" & amod_make2 & "') "
      End If

      SqlCommand2.CommandText = tmpQuery3
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read

          If counter111 = 0 Then
            get_amod_id_model_name = adoTempRS2("amod_id")
          Else
            get_amod_id_model_name = get_amod_id_model_name & "##" & adoTempRS2("amod_id")
          End If

          counter111 = counter111 + 1
        Loop
      End If

    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function
  Public Function get_make_model(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByRef amod_make2 As String, ByRef amod_model2 As String, ByVal amod_temp As Integer) As String
    get_make_model = ""

    Dim tmpQuery3 As String = ""

    Try


      tmpQuery3 = " Select amod_make_name, amod_model_name from aircraft_model with (nolock)  where amod_id = " & amod_temp

      SqlCommand2.CommandText = tmpQuery3
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read


          amod_make2 = adoTempRS2("amod_make_name")
          amod_model2 = adoTempRS2("amod_model_name")

        Loop
      End If


    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function
  Public Function has_been_converted(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByVal folder_id As Long) As Boolean
    has_been_converted = False

    Dim tmpQuery3 As String = ""

    Try


      tmpQuery3 = " Select * from client_folder where cfolder_sissc_id = " & folder_id

      SqlCommand2.CommandText = tmpQuery3
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read
          has_been_converted = True
        Loop
      End If


    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function
  Public Function get_prior_event_cat(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByVal cat_code As String, ByRef cat_name As String) As String
    get_prior_event_cat = ""
    Dim select_count_report As String = ""
    Dim temp_count As Integer = 0

    Try

      select_count_report = "Select * from Priority_Events_Category with (nolock) where  priorevcat_category_code in (" & cat_code & ") "

      SqlCommand2.CommandText = select_count_report
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then
        Do While adoTempRS2.Read

          If temp_count > 0 Then
            get_prior_event_cat = get_prior_event_cat & "##"
            cat_name = cat_name & "##"
          End If

          get_prior_event_cat = get_prior_event_cat & adoTempRS2("priorevcat_category")
          cat_name = cat_name & adoTempRS2("priorevcat_category_name")

          temp_count = temp_count + 1

        Loop
      End If


    Catch ex As Exception
      get_prior_event_cat = ""
    Finally
      adoTempRS2.Close()
    End Try
  End Function

  Public Function Get_Project_Counts(ByRef SqlCommand2 As SqlClient.SqlCommand, ByRef adoTempRS2 As SqlClient.SqlDataReader, ByVal sub_id As Long, ByVal type_of As String) As Integer
    Get_Project_Counts = 0

    Dim tmpQuery As String = ""
    Dim folder_id As Long = 0
    Dim folder_array(200) As Long
    Dim current_count As Integer = 0
    Dim i As Integer = 0

    Try

      tmpQuery = " select * "
      tmpQuery = tmpQuery & " from Subscription_Install_Saved_Search_Criteria  with (nolock) "
      tmpQuery = tmpQuery & " inner join Subscription with (NOLOCK) on sissc_sub_id=sub_id "
      tmpQuery = tmpQuery & " where sissc_tab='" & type_of & "' "
      tmpQuery = tmpQuery & " and sissc_sub_id  = " & Session.Item("localUser").crmSubSubID.ToString.Trim & " "
      tmpQuery = tmpQuery & " and sissc_login = '" & Session.Item("localUser").crmUserLogin.ToString.Trim & "' "
      tmpQuery = tmpQuery & " and sissc_seq_no = '" & Session.Item("localUser").crmSubSeqNo.ToString.Trim & "' "
      tmpQuery = tmpQuery & " and sissc_convert_to_dotnet_flag = 'N' "


      SqlCommand2.CommandText = tmpQuery
      adoTempRS2 = SqlCommand2.ExecuteReader()

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read

          folder_id = adoTempRS2("sissc_id")
          folder_array(current_count) = folder_id

          current_count = current_count + 1
        Loop
      End If

      adoTempRS2.Close()


      For i = 0 To current_count - 1
        If has_been_converted(SqlCommand2, adoTempRS2, folder_array(i)) Then

        Else
          Get_Project_Counts = Get_Project_Counts + 1
        End If
      Next

    Catch ex As Exception
    Finally
      adoTempRS2.Close()
    End Try
  End Function


End Class