Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/flightDataFunctions.vb $
'$$Author: Matt $
'$$Date: 5/15/20 2:07p $
'$$Modtime: 5/15/20 1:19p $
'$$Revision: 14 $
'$$Workfile: flightDataFunctions.vb $
'
' ********************************************************************************


<System.Serializable()> Public Class flightDataFunctions

  Private aError As String

  Private clientConnectString As String

  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String
  Dim comp_functions As New CompanyFunctions
  Dim number_flights_orig As Double = 0.0
  Dim avg_dist_orig As Double = 0.0
  Dim total_dist_orig As Double = 0.0
  Dim avg_flight_orig As Double = 0.0
  Dim total_time_orig As Double = 0.0


  Sub New()

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

  End Sub

#Region "database_connection_strings"

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

  Public Property adminConnectStr() As String
    Get
      adminConnectStr = adminConnectString
    End Get
    Set(ByVal value As String)
      adminConnectString = value
    End Set
  End Property

  Public Property clientConnectStr() As String
    Get
      clientConnectStr = clientConnectString
    End Get
    Set(ByVal value As String)
      clientConnectString = value
    End Set
  End Property

  Public Property starConnectStr() As String
    Get
      starConnectStr = starConnectString
    End Get
    Set(ByVal value As String)
      starConnectString = value
    End Set
  End Property

  Public Property cloudConnectStr() As String
    Get
      cloudConnectStr = cloudConnectString
    End Get
    Set(ByVal value As String)
      cloudConnectString = value
    End Set
  End Property

  Public Property serverConnectStr() As String
    Get
      serverConnectStr = serverConnectString
    End Get
    Set(ByVal value As String)
      serverConnectString = value
    End Set
  End Property

#End Region




#Region "FLIGHT ACTIVITY FAA"


  Public Sub get_title_for_time_period(ByRef title_add_on As String, ByVal action_date As String, ByVal is_graph As Boolean, ByVal timeframe_text As String)

    Dim text_temp As String = ""
    Dim title_original As String = ""

    title_add_on = LCase(title_add_on)

    If Trim(timeframe_text) <> "" And Trim(title_add_on) = "" Then
      title_add_on = Trim(timeframe_text)
    End If 
    title_original = title_add_on

    If Trim(title_add_on) <> "" Then
            If Trim(title_add_on) = "90_days" Then
                ' text_temp = "FLIGHT ACTIVITY FOR LAST 90 DAYS"
                text_temp = "LAST 90 DAYS"
                title_add_on = " (" & FormatDateTime(DateAdd(DateInterval.Day, -90, Date.Now.Date), DateFormat.ShortDate) & " - " & FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate) & ")"
            ElseIf Trim(title_add_on) = "last_year" Then
                text_temp = "LAST YEAR"
                ' text_temp = "FLIGHT ACTIVITY FOR LAST YEAR"
                title_add_on = " (" & FormatDateTime(DateAdd(DateInterval.Year, -1, Date.Now.Date), DateFormat.ShortDate) & " - " & FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate) & ")"
            ElseIf Trim(title_add_on) = "current" Then
                text_temp = "CURRENT OWNER"
                'text_temp = "FLIGHT ACTIVITY FOR CURRENT OWNER"
                title_add_on = " (" & action_date & " - " & FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate) & ")"
            ElseIf Trim(title_add_on) = "all" Then
                text_temp = "LIFETIME"
                'text_temp = "FLIGHT ACTIVITY FOR LIFETIME"
                title_add_on = ""
            ElseIf InStr(Trim(title_add_on), "date range") > 0 Or InStr(Trim(title_add_on), "date_search") > 0 Then
                text_temp = "DATE RANGE"
                'text_temp = "FLIGHT ACTIVITY FOR LAST YEAR"
                title_add_on = Replace(title_add_on, "date range", "")
            Else
        text_temp = "LAST YEAR"
        'text_temp = "FLIGHT ACTIVITY FOR LAST YEAR"
        title_add_on = " (" & FormatDateTime(DateAdd(DateInterval.Year, -1, Date.Now.Date), DateFormat.ShortDate) & " - " & FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate) & ")"
      End If
    Else
      text_temp = "LAST YEAR "
      'text_temp = "FLIGHT ACTIVITY FOR LAST YEAR "
      title_add_on = " (" & FormatDateTime(DateAdd(DateInterval.Year, -1, Date.Now.Date), DateFormat.ShortDate) & " - " & FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate) & ")"
    End If

    If is_graph = True Then
      title_add_on = text_temp & IIf(HttpContext.Current.Session.Item("isMobile"), "<br />", "") & title_add_on
    Else
            If Trim(title_original) <> "" Then
                If Trim(title_original) = "90_days" Then
                    title_add_on = "LAST 90 DAYS"
                ElseIf Trim(title_original) = "last_year" Then
                    title_add_on = "LAST YEAR"
                ElseIf Trim(title_original) = "current" Then
                    title_add_on = "CURRENT OWNER"
                ElseIf Trim(title_original) = "all" Then
                    title_add_on = "LIFETIME"
                ElseIf InStr(Trim(title_add_on), "DATE RANGE") > 0 Or InStr(Trim(title_add_on), "date_search") > 0 Then
                    title_add_on = "DATE RANGE"
                Else
                    title_add_on = "LAST YEAR"
                End If
            Else
                title_add_on = "LAST YEAR"
      End If
    End If


  End Sub
    Public Function Display_Operator_History_Function(ByRef final_table As DataTable, ByRef tab_title As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim toggleRowColor As Boolean = False
        Dim total_flights As Long = 0
        Dim total_distance_min As Long = 0
        Dim total_flight_time_min As Long = 0
        Dim temp_string As String = ""


        Try


            htmlOut.Append("<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border='1' class='formatTable blue large'>")


            If Not IsNothing(final_table) Then

                If final_table.Rows.Count > 0 Then


                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">Operator</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">START</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">END</strong></td>")
                    '  htmlOut.Append("<td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">ROLE</strong></td>")
                    htmlOut.Append("</tr>")


                    For Each r As DataRow In final_table.Rows


                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If



                        If Not IsDBNull(r("Operator")) Then
                            If Not String.IsNullOrEmpty(r.Item("Operator").ToString) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" & r.Item("Operator").ToString & "</td>")
                            Else
                                htmlOut.Append("<td>&nbsp;</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                        End If

                        If Not IsDBNull(r("STARTDATE")) Then
                            If Not IsDBNull(r("STARTDATE")) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" & Month(r.Item("STARTDATE").ToString) & "/" & Day(r.Item("STARTDATE")) & "/" & Right(Year(Trim(r.Item("STARTDATE"))), 2) & "</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                        End If


                        If Not IsDBNull(r("ENDDATE")) Then
                            If Not String.IsNullOrEmpty(r.Item("ENDDATE").ToString) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" & Month(r.Item("ENDDATE").ToString) & "/" & Day(r.Item("ENDDATE")) & "/" & Right(Year(Trim(r.Item("ENDDATE"))), 2) & "</td>")
                            Else
                                htmlOut.Append("<td>&nbsp;</td>")
                            End If
                        Else
                            htmlOut.Append("<td>&nbsp;</td>")
                        End If



                        'If Not IsDBNull(r("acomprole_notes")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("acomprole_notes").ToString) Then
                        '        htmlOut.Append("<td valign=""middle"" align=""left""><font size='-1'>" & r.Item("acomprole_notes").ToString & "</font></td>")
                        '    Else
                        '        htmlOut.Append("<td>&nbsp;</td>")
                        '    End If
                        'Else
                        '    htmlOut.Append("<td>&nbsp;</td>")
                        'End If


                        htmlOut.Append("</tr>")


                    Next


                Else
                    htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No " & DisplayFunctions.ConvertToTitleCase(tab_title) & "</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No " & DisplayFunctions.ConvertToTitleCase(tab_title) & "</td></tr>")
            End If

            htmlOut.Append("</table>")


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayFAAFlightData(ByVal dtFlightData As DataTable) As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function



    Public Function displayFAAFlightData(ByRef dtFlightData As DataTable, ByRef tab_title As String, ByVal title_add_on As String, ByVal action_date As String, ByVal ac_id As Long, ByVal greater_than_date As String, ByVal is_from_ac_details As Boolean, Optional ByVal reg_no As String = "", Optional ByVal IS_REG_BLOCKED As Boolean = True, Optional ByVal is_from As String = "", Optional ByVal product_code_selection As String = "", Optional ByVal comp_id As Long = 0) As String

    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim toggleRowColor As Boolean = False
    Dim total_flights As Long = 0
    Dim total_distance_min As Long = 0
    Dim total_flight_time_min As Long = 0
    Dim temp_string As String = ""

    Dim results_table2 As New DataTable

    Dim final_table As New DataTable

    Dim column As New DataColumn
    Dim column2 As New DataColumn
    Dim column3 As New DataColumn
    Dim column4 As New DataColumn
    Dim column5 As New DataColumn
    Dim column6 As New DataColumn
    Dim column7 As New DataColumn
    Dim column8 As New DataColumn
    Dim column9 As New DataColumn
    Dim column10 As New DataColumn
    Dim column11 As New DataColumn
    Dim column12 As New DataColumn
    Dim column13 As New DataColumn
    Dim column14 As New DataColumn
    Dim column15 As New DataColumn

    Dim is_journal As Boolean = False

    Try



      Call get_title_for_time_period(title_add_on, action_date, True, "")


      If Trim(is_from) = "flightdata" Or Trim(is_from) = "flightdatanoac" Then
        htmlOut.Append("<div class=""Box"">")
        htmlOut.Append("<table width=""100%"" cellpadding=""3"" cellspacing=""0""><tr><td>")

        If Trim(tab_title) = "" Then  ' if its for the flight page 
          htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"" " & IIf(HttpContext.Current.Session.Item("isMobile") = False, "nowrap='nowrap'", "class='mobileAlignLeft'") & "><font class='mainHeading'><strong>FLIGHTS</strong></font></td></tr>")
          htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")
        Else ' if its for another page , set the title
          tab_title = "" & title_add_on & ""
          htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""90%"" class='formatTable blue'>")
        End If
      Else
        If Trim(tab_title) = "" Then  ' if its for the flight page 
          htmlOut.Append("<table cellpadding=""5"" cellspacing=""0"" width=""99%"" border='1'>")
          htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""5"" " & IIf(HttpContext.Current.Session.Item("isMobile") = False, "nowrap='nowrap'", "class='mobileAlignLeft'") & "><span class=""label"">" & title_add_on & "</span></td></tr>")
        Else ' if its for another page , set the title
          If is_from_ac_details = True Then
            tab_title += " " & title_add_on & ""
          Else
            tab_title = "" & title_add_on & ""
          End If
          htmlOut.Append("<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border='1' class='formatTable blue large'>")
        End If
      End If





      column.DataType = System.Type.GetType("System.DateTime")
      column6.AllowDBNull = True
      column.Unique = False
      column.ColumnName = "flight_date"
      final_table.Columns.Add(column)

      column2.DataType = System.Type.GetType("System.String")
      column2.DefaultValue = ""
      column2.AllowDBNull = True
      column2.Unique = False
      column2.ColumnName = "origin_aport"
      final_table.Columns.Add(column2)

      column3.DataType = System.Type.GetType("System.String")
      column3.DefaultValue = ""
      column3.AllowDBNull = True
      column3.Unique = False
      column3.ColumnName = "dest_aport"
      final_table.Columns.Add(column3)

      column4.DataType = System.Type.GetType("System.String")
      column4.DefaultValue = ""
      column4.AllowDBNull = True
      column4.Unique = False
      column4.ColumnName = "aport_dest_lat"
      final_table.Columns.Add(column4)

      column5.DataType = System.Type.GetType("System.String")
      column5.DefaultValue = ""
      column5.AllowDBNull = True
      column5.Unique = False
      column5.ColumnName = "aport_dest_long"
      final_table.Columns.Add(column5)


      column6.DataType = System.Type.GetType("System.Double")
      column6.DefaultValue = 0
      column6.AllowDBNull = True
      column6.Unique = False
      column6.ColumnName = "distance"
      final_table.Columns.Add(column6)

      column7.DataType = System.Type.GetType("System.Double")
      column7.DefaultValue = 0
      column7.AllowDBNull = True
      column7.Unique = False
      column7.ColumnName = "flight_time"
      final_table.Columns.Add(column7)

      column8.DataType = System.Type.GetType("System.Double")
      column8.DefaultValue = 0
      column8.AllowDBNull = True
      column8.Unique = False
      column8.ColumnName = "flight_distance"
      final_table.Columns.Add(column8)


      column9.DataType = System.Type.GetType("System.String")
      column9.DefaultValue = ""
      column9.AllowDBNull = True
      column9.Unique = False
      column9.ColumnName = "journal_text"
      final_table.Columns.Add(column9)

      column10.DataType = System.Type.GetType("System.String")
      column10.DefaultValue = ""
      column10.AllowDBNull = True
      column10.Unique = False
      column10.ColumnName = "ffd_unique_flight_id"
      final_table.Columns.Add(column10)


      column11.DataType = System.Type.GetType("System.String")
      column11.DefaultValue = ""
      column11.AllowDBNull = True
      column11.Unique = False
      column11.ColumnName = "amod_make_name"
      final_table.Columns.Add(column11)

      column12.DataType = System.Type.GetType("System.String")
      column12.DefaultValue = ""
      column12.AllowDBNull = True
      column12.Unique = False
      column12.ColumnName = "amod_model_name"
      final_table.Columns.Add(column12)

      column13.DataType = System.Type.GetType("System.String")
      column13.DefaultValue = ""
      column13.AllowDBNull = True
      column13.Unique = False
      column13.ColumnName = "ac_ser_no_full"
      final_table.Columns.Add(column13)

      column14.DataType = System.Type.GetType("System.String")
      column14.DefaultValue = ""
      column14.AllowDBNull = True
      column14.Unique = False
      column14.ColumnName = "ac_reg_no"
      final_table.Columns.Add(column14)

      column15.DataType = System.Type.GetType("System.String")
      column15.DefaultValue = ""
      column15.AllowDBNull = True
      column15.Unique = False
      column15.ColumnName = "TotalFuelBurn"
      final_table.Columns.Add(column15)


      results_table2 = get_ac_journal_info(ac_id, greater_than_date, False)



      If Not IsNothing(dtFlightData) Then
        For Each drRow As DataRow In dtFlightData.Rows
          final_table.ImportRow(drRow)
        Next
      End If

      If Not IsNothing(results_table2) Then
        For Each drRow As DataRow In results_table2.Rows
          final_table.ImportRow(drRow)
        Next
      End If

      ' sort the table by date asc
      Dim Filtered_DV As New DataView(final_table)

      Filtered_DV.Sort = "flight_date desc, ffd_unique_flight_id desc, journal_text asc "
      final_table = Filtered_DV.ToTable


      If is_from_ac_details = True Then
        htmlOut.Append("<tr><td align='left' colspan=""5"" class=""tiny_text""><A href='FAAFlightData.aspx?acid=" & ac_id & "' target='_blank' class=""float_left text_underline"">View Flight Activity Map & Details</a>")
        htmlOut.Append("<span class=""float_right""><a href='/help/documents/589.pdf' target='_blank'>Flight Data</a> As of " & HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date & "</span></td></tr>")
      End If


      If Not IsNothing(final_table) Then

        If final_table.Rows.Count > 0 Then

          htmlOut.Append("XXYYZZ")
          If Trim(is_from) = "flightdatanoac" Then
            If ac_id = 0 Then
              htmlOut.Append("<tr><th valign=""middle"" align=""left"">Aircraft</th>")
              htmlOut.Append("<th valign=""middle"" align=""left"">Ser#</th>")
              htmlOut.Append("<th valign=""middle"" align=""left"">Reg#</th>")
            End If

            htmlOut.Append("<th valign=""middle"" align=""left"">Date</th>")

            If Trim(is_from) = "flightdatanoac" And ac_id = 0 Then  ' then it is a route analysis 
            Else
              htmlOut.Append("<th valign=""middle"" align=""left"" class=""mobileAlignBottom"">Route</th>")
            End If

            htmlOut.Append("<th valign=""middle"" class='right'>Dist.<br /><em>(nm)</em></th>")
            htmlOut.Append("<th valign=""middle"" class='right'>")
            ' htmlOut.Append("<A href='#' alt='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.' title='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.'>")
            htmlOut.Append("Flight<br />Time<em>(min)</em></th>") '</a>
            htmlOut.Append("<th valign=""middle"" class='right'>Est Fuel<br />Burn (GAL)</th></tr>")
            htmlOut.Append("</thead><tbody>")
          ElseIf Trim(is_from) = "flightdata" Then
            htmlOut.Append("<tr><th valign=""middle"" align=""left"">Date</th>")
            htmlOut.Append("<th valign=""middle"" align=""left"" class=""mobileAlignBottom"">Origin</th>")
            htmlOut.Append("<th valign=""middle"" align=""left"" class=""mobileAlignBottom"">" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "Destination", "Dest.") & "</th>")
            htmlOut.Append("<th valign=""middle"" class='right'>Dist.<br /><em>(nm)</em></th>")
            htmlOut.Append("<th valign=""middle"" class='right'>")
            ' htmlOut.Append("<A href='#' alt='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.' title='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.'>")
            htmlOut.Append("Flight<br />Time<em>(min)</em></th></tr>") '</a>
            htmlOut.Append("</thead><tbody>")
          Else
            htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">Date</strong></td>")
            htmlOut.Append("<td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">Origin</strong></td>")
            htmlOut.Append("<td valign=""top"" align=""left"" class=""mobileAlignBottom""><strong class=""featuresHeader"">" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "Destination", "Dest.") & "</strong></td>")
            htmlOut.Append("<td valign=""top"" align=""center"" class=""mobileAlignBottom""><strong class=""featuresHeader"">" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "Distance<br /></strong>", "Dist.<br />") & "<em>(nm)</em></td>")
            htmlOut.Append("<td valign=""top"" align=""center"" class=""mobileAlignBottom"">")
            ' htmlOut.Append("<A href='#' alt='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.' title='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.'>")
            htmlOut.Append("<strong class=""featuresHeader"">Time</strong><em>(min)</em></td></tr>") '</a>
          End If


          For Each r As DataRow In final_table.Rows

            is_journal = False


            If Not IsDBNull(r.Item("journal_text")) Then ' if its a journal, then write it out 
              If Trim(r.Item("journal_text")) <> "" Then
                is_journal = True
              End If
            End If

            If is_journal = True Then

              htmlOut.Append("<tr>")
              If Not IsDBNull(r.Item("flight_date")) Then
                If Not String.IsNullOrEmpty(r.Item("flight_date").ToString) Then
                  htmlOut.Append("<td valign=""middle"" align=""left"" bgcolor='#A9F5A9'>" + FormatDateTime(r.Item("flight_date").ToString, DateFormat.ShortDate) + "</td>")
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If
              Else
                htmlOut.Append("<td>&nbsp;</td>")
              End If

              htmlOut.Append("<td colspan='4' align='left' bgcolor='#A9F5A9'>")
              If Not IsDBNull(r.Item("journal_text")) Then
                If Not String.IsNullOrEmpty(r.Item("journal_text").ToString) Then
                  htmlOut.Append("" + r.Item("journal_text").ToString + "")
                End If
              End If
              htmlOut.Append("&nbsp;</td>")

            Else

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If


              If Trim(is_from) = "flightdatanoac" Then
                If ac_id = 0 Then
                  If Not IsDBNull(r("amod_make_name")) Then
                    If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                      htmlOut.Append("<td valign=""middle"" align=""left"">" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString & "</td>")
                    Else
                      htmlOut.Append("<td>&nbsp;</td>")
                    End If
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If

                  If Not IsDBNull(r("ac_ser_no_full")) Then
                    If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString) Then
                      htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("ac_ser_no_full").ToString + "</td>")
                    Else
                      htmlOut.Append("<td>&nbsp;</td>")
                    End If
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If


                  If Not IsDBNull(r("ac_reg_no")) Then
                    If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then
                      htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("ac_reg_no").ToString + "</td>")
                    Else
                      htmlOut.Append("<td>&nbsp;</td>")
                    End If
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                End If

                If Not IsDBNull(r.Item("flight_date")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_date").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""left"">" + FormatDateTime(r.Item("flight_date").ToString, DateFormat.ShortDate) + "</td>")
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Trim(is_from) = "flightdatanoac" And ac_id = 0 Then  ' then it is a route analysis 
                Else
                  If Not IsDBNull(r("origin_aport")) Then
                    If Not String.IsNullOrEmpty(r.Item("origin_aport").ToString) Then
                      htmlOut.Append("<td valign=""middle"" align=""left"">" & Replace(Replace(r.Item("origin_aport").ToString, " Airport", " "), "International", "Int.") & " To " & Replace(Replace(r.Item("dest_aport").ToString, " Airport", " "), "International", "Int.") & "</td>")
                    Else
                      htmlOut.Append("<td>&nbsp;</td>")
                    End If
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                End If


                If Not IsDBNull(r("flight_distance")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_distance").ToString) Then '
                    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(ConvertStatuteMileToNauticalMile(r.Item("flight_distance").ToString), 0, True, False, True) + "</td>")
                    total_distance_min = total_distance_min + FormatNumber(ConvertStatuteMileToNauticalMile(r.Item("flight_distance").ToString), 0, True, False, True)
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If


                If Not IsDBNull(r("flight_time")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_time").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True) + "</td>")
                    total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("TotalFuelBurn")) Then
                  If Not String.IsNullOrEmpty(r.Item("TotalFuelBurn").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFuelBurn").ToString, 0, True, False, True) + "</td>")
                    ' total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

              Else
                If Not IsDBNull(r.Item("flight_date")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_date").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""left"">" + clsGeneral.clsGeneral.TwoPlaceYear(r.Item("flight_date")) + "</td>")
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("origin_aport")) Then
                  If Not String.IsNullOrEmpty(r.Item("origin_aport").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""left"">" + Replace(Replace(r.Item("origin_aport").ToString, "Airport", ""), "International", "Int.") + "</td>")
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("dest_aport")) Then
                  If Not String.IsNullOrEmpty(r.Item("dest_aport").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""left"">" + Replace(Replace(r.Item("dest_aport").ToString, "Airport", ""), "International", "Int.") + "</td>")
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("flight_distance")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_distance").ToString) Then '
                    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(ConvertStatuteMileToNauticalMile(r.Item("flight_distance").ToString), 0, True, False, True) + "</td>")
                    total_distance_min = total_distance_min + FormatNumber(ConvertStatuteMileToNauticalMile(r.Item("flight_distance").ToString), 0, True, False, True)
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If


                If Not IsDBNull(r("flight_time")) Then
                  If Not String.IsNullOrEmpty(r.Item("flight_time").ToString) Then
                    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True) + "</td>")
                    total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                Else
                  htmlOut.Append("<td>&nbsp;</td>")
                End If

                If is_from_ac_details = False Then
                  If Not IsDBNull(r("TotalFuelBurn")) Then
                    If Not String.IsNullOrEmpty(r.Item("TotalFuelBurn").ToString) Then
                      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFuelBurn").ToString, 0, True, False, True) + "</td>")
                      ' total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                    Else
                      htmlOut.Append("<td>&nbsp;</td>")
                    End If
                  Else
                    htmlOut.Append("<td>&nbsp;</td>")
                  End If
                End If
              End If




              htmlOut.Append("</tr>")

              total_flights = total_flights + 1

            End If

          Next



          'temp_string &= ("<tr>")
          'temp_string &= ("<td valign=""middle"" align=""left"" colspan='2' nowrap='nowrap'><strong>Total Flights:</strong> " & FormatNumber(total_flights, 0) & "</td>")
          'temp_string &= ("<td valign=""middle"" align=""left"" colspan='2' nowrap='nowrap'><strong>Total Distance</strong>&nbsp;<em>(nm)</em>&nbsp;:&nbsp;" & FormatNumber(total_flight_time_min, 0) & "</td>")
          'temp_string &= ("<td valign=""middle"" align=""left"" colspan='2' nowrap='nowrap'><strong>Total Flight Time</strong>&nbsp;<em>(min)</em>&nbsp;:&nbsp; " & FormatNumber(total_distance_min, 0) & "</td>")
          'temp_string &= ("</tr>")


          temp_string &= ("<tr class=""noBorder"">")
          temp_string &= ("<td valign=""" & IIf(is_from_ac_details, "left""", "middle""") & " class=""smallText"" " & IIf(Trim(is_from) = "flightdatanoac", "align=""left""", "align=""center""") & " colspan='7' " & IIf(HttpContext.Current.Session.Item("isMobile") = False, "nowrap='nowrap'", "class='mobileAlignLeft'") & ">")
          temp_string &= ("<span class=""float_left padded_right""><strong>Total Flights:</strong> " & FormatNumber(total_flights, 0) & " </span>")
          If HttpContext.Current.Session.Item("isMobile") Then
            temp_string &= "<br />"
          End If
          temp_string &= ("<span class=""float_left padded_right""><strong>Total Distance</strong>&nbsp;<em>(nm)</em> " & FormatNumber(total_distance_min, 0) & " </span>")
          If HttpContext.Current.Session.Item("isMobile") Then 'Or is_from_ac_details Then
            temp_string &= "<br />"
          End If
          temp_string &= ("<span class=""float_left padded_right""><strong>Total Flight Time</strong>&nbsp;<em>(hrs)</em> " & FormatNumber((total_flight_time_min / 60), 0) & " </span>")
          If is_from_ac_details = False And is_from <> "flightdata" And is_from <> "flightdatanoac" Then
            'temp_string &= ("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong><A href='selection_listing.aspx?area=Flights&display=table' target='_blank'>VIEW IN GRID</a></strong>")
            ' temp_string &= ("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong><A href='WebSource.aspx?viewType=dynamic&display=table' target='_blank'>VIEW IN GRID</a></strong>")
            temp_string &= ("<span class=""float_right padded_left""><a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=FLIGHT ACTIVITY','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" >VIEW IN GRID</a></span>")
          End If
          temp_string &= ("</td>")
          temp_string &= ("</tr>")


          If IS_REG_BLOCKED = True Then
            temp_string = ("<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border='1'>") & temp_string
            temp_string &= ("<tr><td><b>Detailed flight data for this aircraft (REG#" & reg_no & ") is not available for public viewing based on the request of the owner/operator.</b></td></tr>")
            temp_string &= ("</table>")
          Else
            temp_string = Replace(htmlOut.ToString, "XXYYZZ", temp_string) 'if its on blocked list, then dont add list in  
          End If

          htmlOut.Length = 0   ' clear string
          htmlOut.Append(temp_string) ' set to previous string 

        Else
          htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No " & DisplayFunctions.ConvertToTitleCase(tab_title) & "</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No " & DisplayFunctions.ConvertToTitleCase(tab_title) & "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If Trim(is_from) = "flightdata" Then
        htmlOut.Append("</div>")
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayFAAFlightData(ByVal dtFlightData As DataTable) As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString

    htmlOut = Nothing

  End Function
  Public Function get_ac_journal_info(ByVal ac_id As Long, ByVal greater_than_date As String, ByVal is_for_graph As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If is_for_graph = True Then
        sQuery.Append(" select month(journ_date) as tmonth,  year(journ_date) as tyear, journ_subject as journal_text, 'journal' as ttype ")
      Else
        sQuery.Append(" select journ_date as flight_date, journ_subject as journal_text, 'journal' as ttype ")
      End If

      sQuery.Append(" from journal with (NOLOCK) ")
      sQuery.Append(" where journ_ac_id = " & ac_id & " ")
      sQuery.Append(" and journ_subcat_code_part1= 'WS'  ")
      sQuery.Append(" and journ_internal_trans_flag='N'  ")
      sQuery.Append(" and journ_subcat_code_part3 not in ('DB','RE') ")

      If Trim(greater_than_date) <> "" Then
        sQuery.Append(" and journ_date >= '" & greater_than_date & "'  ")
      End If

      sQuery.Append(" order by journ_date, journ_id ")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_ac_journal_info(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = serverConnectStr ' Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_ac_journal_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_ac_journal_info(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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

    Public Function make_ac_flights_comparisons(ByVal nAircraftID As Long, ByRef chart_label_text As String, ByVal DropDownList_owner As DropDownList, ByVal DropDownList_timeframe As DropDownList, ByVal temp_amod_id As Long, ByVal faa_start_date As TextBox, ByVal faa_end_date As TextBox, ByVal start_date As String, ByVal sRegNumber As String, ByVal aport_id1 As Long, ByVal aport_id2 As Long, ByVal show_one_way As Boolean, ByVal aport_name1 As String, ByVal aport_name2 As String, ByRef total_flights_count As Long, ByRef my_page As Page, ByRef chart_panel As System.Web.UI.UpdatePanel, ByRef map_panel As Panel, ByRef google_string As String, Optional ByRef disableTitle As Boolean = False, Optional ByVal comp_id As Long = 0, Optional ByVal drop_value As String = "") As String
        make_ac_flights_comparisons = ""
        Try

            Dim returnString As String = ""
            Dim tmpAircraftSummaryTable As DataTable = Nothing
            Dim tmpAircraftSummaryTable2 As DataTable = Nothing

            tmpAircraftSummaryTable = get_chart_data_activity_summary(nAircraftID, DropDownList_owner, New DropDownList, temp_amod_id, nAircraftID, faa_start_date.Text, faa_end_date.Text, aport_id1, aport_id2, show_one_way)
            tmpAircraftSummaryTable2 = get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, faa_start_date.Text, faa_end_date.Text, aport_id1, aport_id2, show_one_way)

            returnString = make_google_summary_chart(tmpAircraftSummaryTable, nAircraftID, start_date, tmpAircraftSummaryTable2, Nothing, aport_id1, aport_id2, aport_name1, aport_name2, total_flights_count, my_page, chart_panel, map_panel, DropDownList_timeframe, google_string, disableTitle)

            If Trim(drop_value) <> "" Then
                Call make_chart_text(tmpAircraftSummaryTable, tmpAircraftSummaryTable2, start_date, sRegNumber, Trim(drop_value), "flightdata", chart_label_text, nAircraftID) '  DropDownList_timeframe.SelectedValue
            Else
                Call make_chart_text(tmpAircraftSummaryTable, tmpAircraftSummaryTable2, start_date, sRegNumber, "last_year", "flightdata", chart_label_text, nAircraftID) '  DropDownList_timeframe.SelectedValue
            End If

            Return returnString
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Sub make_chart_text(ByVal tmpAircraftSummaryTable As DataTable, ByVal tmpAircraftSummaryTable2 As DataTable, ByVal start_date As String, ByVal reg_no As String, ByVal timeframe_text As String, Optional ByVal is_from As String = "", Optional ByRef chart_label_text As String = "", Optional ByVal nAircraftID As Long = 0)
        Dim temp_label_sting As String = ""
        Dim is_both As Boolean = False
        Dim top_count As Integer = 0
        Dim top_count2 As Integer = 0

        If Trim(is_from) = "flightdata" Then
            If nAircraftID > 0 Then
                chart_label_text &= ""
            Else
                chart_label_text = ""
            End If
            chart_label_text &= "<div class=""Box""><table cellspacing='0' cellpadding='0' border='0' align='center'  class='blue noPaddingFirstLevelTD' width=""90%""> " ' original top table"
            chart_label_text &= "<thead>"
            chart_label_text &= "<tr><td>"
            chart_label_text &= "<table cellspacing='0' cellpadding='0' align='center' width=""100%""><thead>"
            chart_label_text &= "<tr><th><strong>Utilization Summary</strong><br /><br /></th></tr></thead><tbody>"
            chart_label_text &= "<tr><td><strong>Number of Flights</strong></td></tr>"
            chart_label_text &= "<tr><td><strong>Average Distance(nm)</strong></td></tr>"
            chart_label_text &= "<tr><td><strong>Total Distance (nm)</strong></td></tr>"
            chart_label_text &= "<tr><td><strong>Average Flight Time (hrs)</strong></td></tr>"
            chart_label_text &= "<tr><td><strong>Total Flight Time (hrs)</strong></td></tr>"
            chart_label_text &= "</table>"
            chart_label_text &= "</td>"
        Else
            chart_label_text = "<div class=""mobileWidth mobileOverflowAuto""><table cellspacing='0' cellpadding='0' border='0' align='center' class=""adjustTableMobile""> " ' original top table"
            chart_label_text &= "<tr><td>"
            chart_label_text &= "<table cellspacing='0' cellpadding='3' border='1' align='center'>"
            chart_label_text &= "<tr><td><b>Utilization Summary<br/>&nbsp;</b></td></tr>"
            chart_label_text &= "<tr><td><b>Number of Flights</b></td></tr>"
            chart_label_text &= "<tr><td><b>Average Distance(nm)</b></td></tr>"
            chart_label_text &= "<tr><td><b>Total Distance (nm)</b></td></tr>"
            chart_label_text &= "<tr><td><b>Average Flight Time (hrs)</b></td></tr>"
            chart_label_text &= "<tr><td><b>Total Flight Time (hrs)</b></td></tr>"
            chart_label_text &= "</table>"
            chart_label_text &= "</td>"
        End If



        If Not IsNothing(tmpAircraftSummaryTable) And Not IsNothing(tmpAircraftSummaryTable2) Then
            If tmpAircraftSummaryTable.Rows.Count > 0 And tmpAircraftSummaryTable2.Rows.Count > 0 Then
                is_both = True
            End If
        End If

        If Not IsNothing(tmpAircraftSummaryTable) And Not IsNothing(tmpAircraftSummaryTable2) Then
            If tmpAircraftSummaryTable.Rows.Count > 0 And tmpAircraftSummaryTable2.Rows.Count > 0 Then
                is_both = True

                If Not IsNothing(tmpAircraftSummaryTable) Then
                    If tmpAircraftSummaryTable.Rows.Count > 0 Then
                        For Each r As DataRow In tmpAircraftSummaryTable.Rows
                            top_count = top_count + 1
                        Next
                    End If
                End If


                If Not IsNothing(tmpAircraftSummaryTable2) Then
                    If tmpAircraftSummaryTable2.Rows.Count > 0 Then
                        For Each r As DataRow In tmpAircraftSummaryTable2.Rows
                            top_count2 = top_count2 + 1
                        Next
                    End If
                End If

                ' find the most number of months that will be displayed
                If top_count2 > top_count Then
                    top_count = top_count2
                End If

            End If
        End If

        Call display_columns_faa_chart(tmpAircraftSummaryTable, temp_label_sting, False, start_date, reg_no, timeframe_text, top_count, is_from)
        chart_label_text &= temp_label_sting
        temp_label_sting = ""

        Call display_columns_faa_chart(tmpAircraftSummaryTable2, temp_label_sting, is_both, start_date, reg_no, timeframe_text, top_count, is_from)
        chart_label_text &= temp_label_sting
        temp_label_sting = ""

        chart_label_text &= "</table>"
        If Trim(is_from) = "flightdata" Then
            chart_label_text &= "</div>"
        End If
        'If nAircraftID > 0 Then
        '  chart_label_text &= ""
        'Else
        '  chart_label_text &= ""
        'End If

    End Sub
    Public Sub display_columns_faa_chart(ByRef temp_table As DataTable, ByRef string_to_print As String, ByVal show_variance As Boolean, ByVal start_date As String, ByVal reg_no As String, ByVal timeframe_text As String, ByVal top_count As Integer, Optional ByVal is_from As String = "")

        Dim tcounter As Integer = 0
        Dim temp_inner_string As String = ""
        Dim is_ac As Boolean = False
        Dim avg_count As Double = 0.0
        Dim top_numAircraft As Integer = 0
        Dim totDistance As Double = 0.0
        Dim avg_dist As Double = 0.0
        Dim totTime As Double = 0.0
        Dim avg_time As Double = 0.0
        Dim temp_label_string As String = ""


        If Not IsNothing(temp_table) Then
            If temp_table.Rows.Count > 0 Then


                For Each r As DataRow In temp_table.Rows

                    tcounter = tcounter + 1

                    If Trim(r("ttype")) = "ac" Then
                        is_ac = True
                    Else
                        is_ac = False
                    End If


                    If Not IsDBNull(r("numAircraft")) Then
                        If Not String.IsNullOrEmpty(r.Item("numAircraft").ToString) Then
                            If CDbl(top_numAircraft) < CDbl(r.Item("numAircraft")) Then
                                top_numAircraft = CDbl(r.Item("numAircraft"))
                            End If
                        End If
                    End If


                    If Not IsDBNull(r("avg_count")) Then
                        If Not String.IsNullOrEmpty(r.Item("avg_count").ToString) Then
                            avg_count = CDbl(avg_count + CDbl(r.Item("avg_count")))
                        End If
                    End If


                    If Not IsDBNull(r("totDistance")) Then
                        If Not String.IsNullOrEmpty(r.Item("totDistance").ToString) Then
                            totDistance = CDbl(totDistance + CDbl(r.Item("totDistance")))
                        End If
                    End If


                    If Not IsDBNull(r("totTime")) Then
                        If Not String.IsNullOrEmpty(r.Item("totTime").ToString) Then
                            totTime = CDbl(totTime + CDbl(r.Item("totTime")))
                        End If
                    End If


                Next


                totDistance = flightDataFunctions.ConvertStatuteMileToNauticalMile(totDistance)

                ' if there is less than the max, then must divide to get totals 
                If top_count > tcounter Then
                    tcounter = top_count
                End If

                ' avg_count = CDbl(CDbl(avg_count) / CDbl(top_numAircraft))  ' 117/1 or 35564/263
                totDistance = CDbl(CDbl(totDistance) / CDbl(top_numAircraft))
                totTime = CDbl(CDbl(totTime) / CDbl(top_numAircraft)) '
                totTime = CDbl(CDbl(totTime) / CDbl(60))

                Call get_title_for_time_period(temp_label_string, start_date, False, timeframe_text)


                temp_label_string = Replace(temp_label_string, Year(Now()), Right(Year(Now()), 2))
                temp_label_string = Replace(temp_label_string, Year(Now()) - 1, Right(Year(Now()) - 1, 2))
                temp_label_string = Replace(temp_label_string, Year(Now()) - 2, Right(Year(Now()) - 2, 2))
                temp_label_string = Replace(temp_label_string, Year(Now()) - 3, Right(Year(Now()) - 3, 2))
                temp_label_string = Replace(temp_label_string, Year(Now()) - 4, Right(Year(Now()) - 4, 2))

                '  If is_since_owner = "Y" Then
                '  temp_label_string = Replace(temp_label_string, "LAST YEAR", "SINCE</br>OWNER")
                ' End If


                string_to_print &= "<td>"
                If Trim(is_from) = "flightdata" Then
                    temp_label_string = Replace(temp_label_string, "LAST YEAR", "LAST</br>YEAR")
                    string_to_print &= "<table cellspacing='0' cellpadding='0' align='center' width=""100%""><thead>"
                Else
                    string_to_print &= "<table cellspacing='0' cellpadding='3' border='1' align='center'>"
                End If


                If Trim(is_from) = "flightdata" Then
                    If is_ac = True Then
                        string_to_print &= "<tr><th  class='right' nowrap='nowrap'><strong>" & Trim(reg_no) & "<br/>" & temp_label_string & "</strong></th></tr></thead><tbody>"
                    Else
                        string_to_print &= "<tr><th  class='right' nowrap='nowrap'><strong>Avg Model<br/>" & temp_label_string & "</strong></th></tr></thead><tbody>"
                    End If
                Else
                    If is_ac = True Then
                        string_to_print &= "<tr><td nowrap='nowrap'><b>" & Trim(reg_no) & "<br/>" & temp_label_string & "</b></td></tr></thead><tbody>"
                    Else
                        string_to_print &= "<tr><td nowrap='nowrap'><b>Average Model<br/>" & temp_label_string & "</b></td></tr></thead><tbody>"
                    End If
                End If



                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_count, 2) & "&nbsp;</td></tr>"

                avg_dist = CDbl(totDistance / avg_count)
                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_dist, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(totDistance, 2) & "&nbsp;</td></tr>"

                avg_time = CDbl(totTime / avg_count)
                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_time, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(totTime, 2) & "&nbsp;</td></tr>"
                string_to_print &= "</table>"
                string_to_print &= "</td>"

                string_to_print &= "<td>"
                If Trim(is_from) = "flightdata" Then
                    string_to_print &= "<table cellspacing='0' cellpadding='0' align='center' width=""100%""><thead>"
                    If is_ac = True Then
                        string_to_print &= "<tr><th class='right' nowrap='nowrap'><strong>" & Trim(reg_no) & "<br/>Monthly<br/>Avg</strong></th></tr></thead><tbody>"
                    Else
                        string_to_print &= "<tr><th class='right' nowrap='nowrap'><strong>Model<br/>Monthly</br>Average</strong></th></tr></thead><tbody>"
                    End If
                Else
                    string_to_print &= "<table cellspacing='0' cellpadding='3' border='1' align='center'>"
                    If is_ac = True Then
                        string_to_print &= "<tr><td align='left' nowrap='nowrap'><b>" & Trim(reg_no) & "<br/>Monthly Average</b></td></tr>"
                    Else
                        string_to_print &= "<tr><td align='left' nowrap='nowrap'><b>Model<br/>Monthly Average</b></td></tr>"
                    End If
                End If




                avg_count = CDbl(CDbl(avg_count) / tcounter)
                totDistance = CDbl(CDbl(totDistance) / tcounter)
                totTime = CDbl(CDbl(totTime) / tcounter)


                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_count, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_dist, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(totDistance, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(avg_time, 2) & "&nbsp;</td></tr>"
                string_to_print &= "<tr><td align='right'>" & FormatNumber(totTime, 2) & "&nbsp;</td></tr>"
                string_to_print &= "</table>"
                string_to_print &= "</td>"

                If is_ac = True Then
                    number_flights_orig = avg_count
                    avg_dist_orig = avg_dist
                    total_dist_orig = totDistance
                    avg_flight_orig = avg_time
                    total_time_orig = totTime
                ElseIf show_variance = True Then

                    avg_count = CDbl(number_flights_orig - avg_count)
                    avg_dist = CDbl(avg_dist_orig - avg_dist)
                    totDistance = CDbl(total_dist_orig - totDistance)
                    avg_time = CDbl(avg_flight_orig - avg_time)
                    totTime = CDbl(total_time_orig - totTime)

                    string_to_print &= "<td>"
                    If Trim(is_from) = "flightdata" Then
                        string_to_print &= "<table cellspacing='0' cellpadding='0'  align='center' width=""100%""><thead>"
                        string_to_print &= "<tr><th class='right'><strong>" & Trim(reg_no) & " vs. Model<br/>Monthly<br/>Variance</strong></th></tr></thead><tbody>"
                    Else
                        string_to_print &= "<table cellspacing='0' cellpadding='3' border='1' align='center'>"
                        string_to_print &= "<tr><td><b>" & Trim(reg_no) & " vs. Model<br/>Monthly Variance</b></td></tr>"
                    End If



                    If CDbl(avg_count) > 0 Then
                        string_to_print &= "<tr><td align='right'><font color='green'>" & FormatNumber(avg_count, 2) & "</font>&nbsp;</td></tr>"
                    ElseIf CDbl(avg_count) < 0 Then
                        string_to_print &= "<tr><td align='right'><font color='red'>" & FormatNumber(avg_count, 2) & "</font>&nbsp;</td></tr>"
                    Else
                        string_to_print &= "<tr><td align='right'><font color='black'>" & FormatNumber(avg_count, 2) & "</font>&nbsp;</td></tr>"
                    End If

                    If CDbl(avg_dist) > 0 Then
                        string_to_print &= "<tr><td align='right'><font color='green'>" & FormatNumber(avg_dist, 2) & "</font>&nbsp;</td></tr>"
                    ElseIf CDbl(avg_dist) < 0 Then
                        string_to_print &= "<tr><td align='right'><font color='red'>" & FormatNumber(avg_dist, 2) & "</font>&nbsp;</td></tr>"
                    Else
                        string_to_print &= "<tr><td align='right'><font color='black'>" & FormatNumber(avg_dist, 2) & "</font>&nbsp;</td></tr>"
                    End If

                    If CDbl(totDistance) > 0 Then
                        string_to_print &= "<tr><td align='right'><font color='green'>" & FormatNumber(totDistance, 2) & "</font>&nbsp;</td></tr>"
                    ElseIf CDbl(totDistance) < 0 Then
                        string_to_print &= "<tr><td align='right'><font color='red'>" & FormatNumber(totDistance, 2) & "</font>&nbsp;</td></tr>"
                    Else
                        string_to_print &= "<tr><td align='right'><font color='black'>" & FormatNumber(totDistance, 2) & "</font>&nbsp;</td></tr>"
                    End If

                    If CDbl(avg_time) > 0 Then
                        string_to_print &= "<tr><td align='right'><font color='green'>" & FormatNumber(avg_time, 2) & "</font>&nbsp;</td></tr>"
                    ElseIf CDbl(avg_time) < 0 Then
                        string_to_print &= "<tr><td align='right'><font color='red'>" & FormatNumber(avg_time, 2) & "</font>&nbsp;</td></tr>"
                    Else
                        string_to_print &= "<tr><td align='right'><font color='black'>" & FormatNumber(avg_time, 2) & "</font>&nbsp;</td></tr>"
                    End If

                    If CDbl(totTime) > 0 Then
                        string_to_print &= "<tr><td align='right'><font color='green'>" & FormatNumber(totTime, 2) & "</font>&nbsp;</td></tr>"
                    ElseIf CDbl(totTime) < 0 Then
                        string_to_print &= "<tr><td align='right'><font color='red'>" & FormatNumber(totTime, 2) & "</font>&nbsp;</td></tr>"
                    Else
                        string_to_print &= "<tr><td align='right'><font color='black'>" & FormatNumber(totTime, 2) & "</font>&nbsp;</td></tr>"
                    End If
                    string_to_print &= "</table>"
                    string_to_print &= "</td>"

                    number_flights_orig = 0
                    avg_dist_orig = 0
                    total_dist_orig = 0
                    avg_flight_orig = 0
                    total_time_orig = 0
                End If


                If Trim(is_from) = "flightdata" Then
                    string_to_print &= "</tbody>"
                End If
                'chart_label_text &= "<td><b>Model for Timeframe (1/1-12/31)/AC</b></td>"
                'chart_label_text &= "<td><b>	Avg Model</b></td>"
                'chart_label_text &= "<td><b>AC vs. Model Variance </b></td>"
                'chart_label_text &= "</tr>"

            End If
        End If

    End Sub
    Public Function make_google_summary_chart(ByRef tmpAircraftSummaryTable As DataTable, ByVal ac_id As Long, ByVal greater_than_date As String, ByRef tmpAircraftSummaryTable2 As DataTable, Optional ByVal tmpAircraftSummaryTable3 As DataTable = Nothing, Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal aport_name1 As String = "", Optional ByVal aport_name2 As String = "", Optional ByRef total_flights_count As String = "", Optional ByRef my_page As Page = Nothing, Optional ByRef chart_panel As System.Web.UI.UpdatePanel = Nothing, Optional ByRef map_panel As Panel = Nothing, Optional ByRef DropDownList_timeframe As DropDownList = Nothing, Optional ByRef google_map_string_ret As String = "", Optional ByRef disableTitle As Boolean = False, Optional ByVal DisplayOnlyFirstAirport As Boolean = False, Optional ByVal DisplayOnlySecondAirport As Boolean = False) As String


    Dim google_map_string As String = ""
    Dim temp_data As String = ""
    Dim row_added As Boolean = False
    Dim results_table2 As New DataTable

    Dim final_table As New DataTable

    Dim column1 As New DataColumn
    Dim column2 As New DataColumn
    Dim column3 As New DataColumn
    Dim column4 As New DataColumn
    Dim column5 As New DataColumn
    Dim column6 As New DataColumn
    Dim instr_spot As Integer = 0

    Dim journ_text As String = ""

    Dim is_journal As Boolean = False
    Dim temp_label_string As String = ""
    Dim google_map_stringB As New StringBuilder



    Try



      column1.DataType = System.Type.GetType("System.Int16")
      column1.DefaultValue = 0
      column1.AllowDBNull = True
      column1.Unique = False
      column1.ColumnName = "tyear"
      final_table.Columns.Add(column1)


      column2.DataType = System.Type.GetType("System.Int16")
      column2.DefaultValue = 0
      column2.DefaultValue = 0
      column2.AllowDBNull = True
      column2.Unique = False
      column2.ColumnName = "tmonth"
      final_table.Columns.Add(column2)


      column3.DataType = System.Type.GetType("System.String")
      column3.DefaultValue = ""
      column3.AllowDBNull = True
      column3.Unique = False
      column3.ColumnName = "avg_count"
      final_table.Columns.Add(column3)

      column4.DataType = System.Type.GetType("System.String")
      column4.DefaultValue = ""
      column4.AllowDBNull = True
      column4.Unique = False
      column4.ColumnName = "journal_text"
      final_table.Columns.Add(column4)

      column5.DataType = System.Type.GetType("System.String")
      column5.DefaultValue = ""
      column5.AllowDBNull = True
      column5.Unique = False
      column5.ColumnName = "ttype"
      final_table.Columns.Add(column5)

      If aport_id1 > 0 And aport_id2 > 0 And ac_id = 0 Then
        column6.DataType = System.Type.GetType("System.String")
        column6.DefaultValue = ""
        column6.AllowDBNull = True
        column6.Unique = False
        column6.ColumnName = "tcount"
        final_table.Columns.Add(column6)
      End If


      results_table2 = get_ac_journal_info(ac_id, greater_than_date, True)

      If Not IsNothing(tmpAircraftSummaryTable2) Then
        For Each drRow As DataRow In tmpAircraftSummaryTable2.Rows
          final_table.ImportRow(drRow)
        Next
      End If

      If Not IsNothing(tmpAircraftSummaryTable) Then
        For Each drRow As DataRow In tmpAircraftSummaryTable.Rows
          final_table.ImportRow(drRow)
        Next
      End If


      If Not IsNothing(tmpAircraftSummaryTable3) Then
        For Each drRow As DataRow In tmpAircraftSummaryTable3.Rows
          final_table.ImportRow(drRow)
        Next
      End If

      If Not IsNothing(results_table2) Then
        For Each drRow As DataRow In results_table2.Rows
          final_table.ImportRow(drRow)
        Next
      End If

      ' sort the table by date asc
      Dim Filtered_DV As New DataView(final_table)

      Filtered_DV.Sort = " tyear asc, tmonth asc, ttype asc, journal_text desc "
      final_table = Filtered_DV.ToTable


      If Not IsNothing(final_table) Then
        If final_table.Rows.Count > 0 Then

          google_map_string = " data1.addColumn('string', 'Date'); "

          If aport_id1 > 0 And aport_id2 > 0 And ac_id = 0 Then

            If DisplayOnlyFirstAirport = True Then

              google_map_string &= " data1.addColumn('number', 'Originating From " & Replace(aport_name1, "'", "") & "'); "
              'google_map_string &= " data1.addColumn('number', 'Flights per Month'); "
            ElseIf DisplayOnlySecondAirport = True Then
              google_map_string &= " data1.addColumn('number', 'Originating From " & Replace(aport_name2, "'", "") & "'); "
              'google_map_string &= " data1.addColumn('number', 'Flights per Month'); "
            Else
              google_map_string &= " data1.addColumn('number', 'Flights per Month'); "
              google_map_string &= " data1.addColumn('number', 'Originating From " & Replace(aport_name1, "'", "") & "'); "
              google_map_string &= " data1.addColumn('number', 'Originating From " & Replace(aport_name2, "'", "") & "'); "
            End If

          Else
            google_map_string &= " data1.addColumn('number', 'Flights per Month'); "
            google_map_string &= " data1.addColumn('number', 'AVG Model Flights/Mo'); "
            google_map_string &= " data1.addColumn('number', 'New Owner'); "
          End If

          google_map_string &= " data1.addRows(["

          google_map_stringB.Append(google_map_string)
          google_map_string = ""


          For Each r As DataRow In final_table.Rows
            If Not IsDBNull(r("tyear")) Then
              If Not String.IsNullOrEmpty(r.Item("tyear").ToString) Then

                If Not IsDBNull(r("tmonth")) Then
                  If Not String.IsNullOrEmpty(r.Item("tmonth").ToString) Then

                    is_journal = False

                    If Not IsDBNull(r("journal_text")) Then
                      If Not String.IsNullOrEmpty(r.Item("journal_text").ToString) Then
                        is_journal = True
                      End If
                    End If

                    If is_journal Then
                      If row_added = True Then
                        google_map_string &= ","
                      End If

                      journ_text = r.Item("journal_text").ToString
                      instr_spot = InStr(journ_text, " to ")
                      journ_text = "Sold to " & Replace(Right(journ_text, Len(journ_text) - instr_spot - 3), "'", "")


                      temp_data = r.Item("tmonth").ToString & "/" & r.Item("tyear").ToString & ": " & journ_text
                      google_map_string &= "['" & temp_data & "', null , null, 0  ]"
                      row_added = True
                    ElseIf Trim(r("ttype")) = "ac" Then
                      If row_added = True Then
                        google_map_string &= ","
                      End If

                      temp_data = r.Item("tmonth").ToString & "/" & r.Item("tyear").ToString
                      If aport_id1 > 0 And aport_id2 > 0 And ac_id = 0 Then
                        total_flights_count += r.Item("tcount")
                        google_map_string &= "['" & temp_data & "', " & r.Item("tcount").ToString & ", "

                        If DisplayOnlySecondAirport = False And DisplayOnlyFirstAirport = False Then
                          google_map_string &= " ORIG1, "

                          google_map_string &= " ORIG2"
                        End If

                        google_map_string &= "]"
                      Else
                        google_map_string &= "['" & temp_data & "', " & r.Item("avg_count").ToString & ",  AMOD_AVG,  null]"
                      End If


                      row_added = True
                    ElseIf Trim(r("ttype")) = "model" Then
                      'this should replace the last months data with the avg
                      ' if there is no month that was just put in for ac, then put it in for model only
                      If InStr(Trim(google_map_string), "AMOD_AVG") > 0 Then
                        google_map_string = Replace(google_map_string, "AMOD_AVG", r.Item("avg_count").ToString)
                      Else
                        If row_added = True Then
                          google_map_string &= ","
                        End If

                        temp_data = r.Item("tmonth").ToString & "/" & r.Item("tyear").ToString
                        google_map_string &= "['" & temp_data & "', null, " & r.Item("avg_count").ToString & ",  null]"
                        row_added = True
                      End If

                    ElseIf Trim(r("ttype")) = "orig1" Then
                      'this should replace the last months data with the avg
                      ' if there is no month that was just put in for ac, then put it in for model only 
                      google_map_string = Replace(google_map_string, "ORIG1", r.Item("tcount").ToString)
                    ElseIf Trim(r("ttype")) = "orig2" Then
                      'this should replace the last months data with the avg
                      ' if there is no month that was just put in for ac, then put it in for model only 
                      google_map_string = Replace(google_map_string, "ORIG2", r.Item("tcount").ToString)

                    End If


                  End If

                End If

              End If
            End If

            'google_map_stringB.Append(google_map_string)
            'google_map_string = ""

          Next
        End If
      End If


      'google_map_string = google_map_stringB.ToString

      google_map_string = Replace(google_map_string, "AMOD_AVG", "null")
      google_map_string = Replace(google_map_string, "ORIG1", "null")
      google_map_string = Replace(google_map_string, "ORIG2", "null")

      google_map_stringB.Append(google_map_string)


      If Not IsNothing(DropDownList_timeframe) Then
        temp_label_string = DropDownList_timeframe.SelectedValue
      End If
      Call get_title_for_time_period(temp_label_string, greater_than_date, True, "")

      'set it before it goes in and has other items done to it 
      google_map_string_ret = google_map_stringB.ToString
      google_map_string = google_map_stringB.ToString

      If Not IsNothing(DropDownList_timeframe) Then
        If aport_id1 > 0 And aport_id2 > 0 And ac_id = 0 Then
          DisplayFunctions.load_google_chart_faa(google_map_string, IIf(disableTitle, "", "ACTIVITY SUMMARY - " & temp_label_string), "Flights per Month", "chart_div_survey", IIf(HttpContext.Current.Session.Item("isMobile") = False, 870, 320), 420, my_page, chart_panel, aport_id1)
        Else
          DisplayFunctions.load_google_chart_faa(google_map_string, IIf(disableTitle, "", "ACTIVITY SUMMARY - " & temp_label_string), "Flights per Month", "chart_div_survey", IIf(HttpContext.Current.Session.Item("isMobile") = False, 870, 320), 420, my_page, chart_panel)
        End If
      End If


      If Not IsNothing(DropDownList_timeframe) Then
        If DropDownList_timeframe.SelectedValue <> "all" Then
          map_panel.Visible = True
        Else
          map_panel.Visible = False
        End If
      End If

    Catch ex As Exception
      Return "ACTIVITY SUMMARY - ERROR"
    End Try

    Return "ACTIVITY SUMMARY - " & temp_label_string

  End Function



    Public Function get_chart_data_activity_summary(ByVal sAircraftID As Long, ByRef DropDownList_owner As System.Web.UI.WebControls.DropDownList, ByRef DropDownList_timeframe As System.Web.UI.WebControls.DropDownList, ByVal amod_id As Integer, ByVal use_hidden_ac_id As Long, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal ttype_name As String = "", Optional ByVal product_code_selection As String = "", Optional ByVal comp_id As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try


            '' -- ACTIVITY SUMMARY
            'sQuery.Append(" SELECT distinct year(ffd_date) as tyear, month(ffd_date) as tmonth, count(*) as tcount ")

            'sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")
            'sQuery.Append(" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id and ffd_journ_id = ac_journ_id and ffd_hide_flag='N'")
            'sQuery.Append(" WHERE (ac_id = " & sAircraftID & ") AND (ac_journ_id = 0) ")

            'If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
            '  sQuery.Append(" AND (ffd_date >= ac_purchase_date)")
            'End If

            'If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
            '  sQuery.Append(" AND (DATEDIFF(day,ffd_date, GETDATE()) <= 90)")
            'End If

            'If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
            '  sQuery.Append(" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
            'End If

            'sQuery.Append(" group by year(ffd_date), month(ffd_date) ")
            'sQuery.Append(" ORDER BY year(ffd_date), month(ffd_date) ")  

            sQuery.Append(" SELECT distinct year(ffd_date) as tyear, month(ffd_date) as tmonth, count(*) as tcount, ")



            If Trim(sAircraftID) > 0 Then
                sQuery.Append(" 'ac' as ttype, ")
            ElseIf aport_id1 > 0 Then
                If Trim(ttype_name) <> "" Then
                    sQuery.Append(" '" & Trim(ttype_name) & "' as ttype, ")
                Else
                    sQuery.Append(" 'ac' as ttype, ")
                End If
            Else
                sQuery.Append(" 'model' as ttype, ")
            End If


            If comp_id > 0 Then
                sQuery.Append("  cast((cast(count(*) as decimal(8,2))/cast(COUNT(distinct ac_id) as decimal(8,2))) as decimal(8,2)) as avg_count, ")
                sQuery.Append(" COUNT(distinct ac_id) as numAircraft, ")
            Else
                sQuery.Append("  cast((cast(count(*) as decimal(8,2))/cast(COUNT(distinct ffd_ac_id) as decimal(8,2))) as decimal(8,2)) as avg_count, ")
                sQuery.Append(" COUNT(distinct ffd_ac_id) as numAircraft, ")
            End If

            sQuery.Append(" SUM(ffd_flight_time) as totTime, ")
            sQuery.Append(" SUM(ffd_distance) as totDistance ")


            If comp_id > 0 Then
                sQuery.Append(" FROM view_flights WITH (NOLOCK) ")
            Else
                sQuery.Append(" FROM FAA_Flight_Data WITH (NOLOCK) ")
                sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = ffd_ac_id and ac_journ_id = 0 ")
                sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on amod_id = ac_amod_id  ")
            End If


            If Trim(sAircraftID) > 0 Then
                sQuery.Append(" where ffd_ac_id = " & sAircraftID & " ")
            ElseIf aport_id1 > 0 Then ' aport_id1

                If comp_id > 0 Then
                    sQuery.Append(" WHERE  ac_id > 0 ")
                Else
                    sQuery.Append(" WHERE  ffd_journ_id = 0 ")
                End If

                If show_one_way = True Then
                    sQuery.Append("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                Else
                    sQuery.Append("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                    sQuery.Append("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
                End If
            Else
                sQuery.Append(" WHERE ffd_ac_id in (select distinct ac_id from Aircraft with (NOLOCK) ")
                sQuery.Append(" where ac_amod_id = " & amod_id & " and ac_journ_id = 0)  ")
            End If


            ' sQuery.Append(" AND (ac_journ_id = 0) ")
            If comp_id > 0 Then
                sQuery.Append(" and comp_id = " & comp_id & " ")
            Else
                sQuery.Append(" and ffd_hide_flag='N' ")
            End If

            If Not IsNothing(DropDownList_owner) Then
                If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                    If Trim(sAircraftID) > 0 Then
                        sQuery.Append("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = ffd_ac_id)) ")
                    Else
                        sQuery.Append("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = " & use_hidden_ac_id & ")) ")
                    End If
                End If
            End If

            If Not IsNothing(DropDownList_timeframe) Then
                If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                    sQuery.Append(" AND (DATEDIFF(day,ffd_date, GETDATE()) <= 90)")
                End If

                If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                    sQuery.Append(" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
                End If
            End If


            If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                If Trim(start_date) <> "" Then
                    sQuery.Append(" AND ffd_date >= '" & Trim(start_date) & "' ")
                End If
                If Trim(end_date) <> "" Then
                    sQuery.Append(" AND ffd_date <= '" & Trim(end_date) & "' ")
                End If
            End If

            If Trim(product_code_selection) <> "" Then
                sQuery.Append(" " & product_code_selection & " ")
            End If

            If comp_id > 0 Then
                sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " & Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), "amod", "Aircraft_Model.amod"))
            End If




            sQuery.Append(" group by year(ffd_date), month(ffd_date) ")
            sQuery.Append(" ORDER BY year(ffd_date), month(ffd_date) ")



            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_chart_data_activity_summary(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = serverConnectStr ' Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_chart_data_activity_summary load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_chart_data_activity_summary(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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

    Public Function get_Distances_Flights(ByVal airport_search As String, ByVal temp_airport_id As Long, ByVal origin_or_dest As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing


        Try

            sQuery.Append(" select ")

            If Trim(origin_or_dest) = "origin" Then   ' if its origin, a2 will be origin, if its dest, then a2 should be dest
                sQuery.Append("  Airport.aport_name as 'DEST NAME',  Airport.aport_latitude_decimal as 'DEST LAT', Airport.aport_longitude_decimal as 'DEST LONG', ")
                sQuery.Append("  a2.aport_name as 'ORIGIN NAME',  a2.aport_latitude_decimal as 'ORIGIN LAT', a2.aport_longitude_decimal as 'ORIGIN LONG',")
                sQuery.Append("  Airport.aport_name as 'DEST NAME', ")
                sQuery.Append("  Airport.aport_icao_code as dest_aport_icao_code, Airport.aport_iata_code as dest_aport_iata_code ")
                sQuery.Append(",  a2.aport_icao_code as origin_aport_icao_code, a2.aport_iata_code as origin_aport_iata_code,  ")
            Else
                sQuery.Append("  a2.aport_name as 'DEST NAME',  a2.aport_latitude_decimal as 'DEST LAT', a2.aport_longitude_decimal as 'DEST LONG', ")
                sQuery.Append("  Airport.aport_name as 'ORIGIN NAME',  Airport.aport_latitude_decimal as 'ORIGIN LAT', Airport.aport_longitude_decimal as 'ORIGIN LONG',")
                sQuery.Append("  a2.aport_icao_code as dest_aport_icao_code, a2.aport_iata_code as dest_aport_iata_code ")
                sQuery.Append(",  Airport.aport_icao_code as origin_aport_icao_code, Airport.aport_iata_code as origin_aport_iata_code,  ")
            End If


            sQuery.Append("  dbo.ConvertLatitudeLongitudeToMiles(Airport.aport_latitude_decimal, Airport.aport_longitude_decimal, a2.aport_latitude_decimal, a2.aport_longitude_decimal) as 'DISTANCE' ")
            sQuery.Append(" from Airport With (NOLOCK)  ")
            sQuery.Append(" inner join airport a2 On a2.aport_id = " & temp_airport_id & " ")

            If IsNumeric(airport_search) Then
                sQuery.Append(" where (Airport.aport_id = " & airport_search & ") ")
            Else
                sQuery.Append(" where (Airport.aport_iata_code = '" & airport_search & "' or Airport.aport_icao_code = '" & airport_search & "') ")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_Distances_Flights(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = serverConnectStr  ' Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_Distances_Flights load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_Distances_Flights(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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
    Public Function getFAAFlightData_View_Simple(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByRef DropDownList_owner As System.Web.UI.WebControls.DropDownList, ByRef DropDownList_timeframe As System.Web.UI.WebControls.DropDownList, Optional ByVal check_unclean As Boolean = False, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal activetab As Long = 0, Optional ByVal removeIATACode As Boolean = False, Optional ByVal product_code_selection As String = "", Optional ByVal company As Boolean = False, Optional ByVal aircraft As Boolean = False, Optional ByVal comp_id As Long = 0, Optional ByVal Flight_Id1 As String = "", Optional ByVal Flight_Id2 As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try


            If company Then
                sQuery.Append("SELECT ")
                sQuery.Append(" comp_id, comp_name, comp_city, comp_state, comp_country, comp_email_address,comp_off_phone,  comp_address1, cbus_name ")
                'sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone ,contact_mob_phone, comp_address1 ")

                sQuery.Append(", COUNT(ffd_unique_flight_id) as NbrFlights,")
                sQuery.Append(" SUM(convert(decimal(18,4),ffd_flight_time))/60 as TotalFlightTimeHrs, ")
                sQuery.Append(" SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn ")
            ElseIf aircraft Then
                sQuery.Append("SELECT ")
                sQuery.Append(" amod_make_name, amod_model_name, ac_ser_no_full as SERNO_NONDISPLAY, ac_ser_no_sort as SERNOSORT_NONDISPLAY,  ")
                sQuery.Append(" ac_reg_no as REGNO_NONDISPLAY, faablk_reg_no, ac_ser_no_full, ac_reg_no, base_aport_name, ac_id,  ")
                sQuery.Append(" comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY',  ")
                sQuery.Append(" comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',comp_off_phone AS 'OFFICE PHONE', ")
                sQuery.Append(" count(*) AS tflights ,  ")
                sQuery.Append(" COUNT(ffd_unique_flight_id) as NbrFlights, SUM(convert(decimal(18,4),ffd_flight_time))/60 as TotalFlightTimeHrs,  ")
                sQuery.Append(" SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn, comp_id, cbus_name  ")

            Else
                sQuery.Append("SELECT amod_make_name as MAKE, amod_model_name as MODEL, base_aport_name, base_aport_city, base_aport_state, base_aport_country, base_aport_iata_code, base_aport_icao_code, ac_mfr_year as 'MFR YEAR', SERNBR, REGNBR,")
                sQuery.Append("ffd_date as 'FLIGHT DATE', ffd_flight_time as 'FLIGHT TIME', ffd_distance as 'DISTANCE', ESTFUELBURN, ")
                sQuery.Append("ffd_origin_aport as 'ORIGIN CODE', origin_aport_name AS 'ORIGIN NAME', origin_aport_city AS 'ORIGIN CITY', origin_aport_state AS 'ORIGIN STATE', ")
                sQuery.Append("origin_aport_country as 'ORIGIN COUNTRY',  origin_aport_latitude AS 'ORIGIN LAT', origin_aport_longitude AS 'ORIGIN LONG',")
                sQuery.Append("ffd_dest_aport AS 'DEST CODE',dest_aport_name AS 'DEST NAME',dest_aport_city AS 'DEST CITY',  dest_aport_state AS 'DEST STATE',  dest_aport_country AS 'DEST COUNTRY', ")
                sQuery.Append("dest_aport_latitude AS 'DEST LAT', dest_aport_longitude AS 'DEST LONG',  comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', ")
                sQuery.Append("comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',")
                sQuery.Append("comp_off_phone AS 'OFFICE PHONE', ")
                sQuery.Append("contact_first_name AS 'FIRST NAME', contact_last_name AS 'LAST NAME', contact_title AS 'TITLE', contact_email_address AS 'CONTACT EMAIL',")
                sQuery.Append("contact_off_phone AS 'CONTACT OFFICE PHONE',")
                sQuery.Append("contact_mob_phone AS 'CONTACT MOBILE PHONE', ac_id as AC_ID, comp_id AS 'COMP_ID',contact_id AS 'CONTACT_ID',ffd_origin_aport_id AS 'ORIGIN_ID',ffd_dest_aport_id AS 'DEST_ID'")
                sQuery.Append(", origin_continent, dest_continent, amjiqs_cat_desc, cbus_name ")

                If (Trim(Flight_Id1) <> "" And Trim(Flight_Id2) <> "") Then
                    sQuery.Append(", dest_aport_icao_code, dest_aport_iata_code ")
                    sQuery.Append(", origin_aport_icao_code, origin_aport_iata_code ")
                End If

                If sAircraftID > 0 Then
                    sQuery.Append(", Convert(varchar(10), ffd_origin_date, 101) as 'DepartureDate',  convert (varchar(15), ffd_origin_date,8) as 'DepartureTime' ")
                    sQuery.Append(", Convert(varchar(10), ffd_dest_date, 101) as 'ArrivalDate' , convert (varchar(15), ffd_dest_date,8) as 'ArrivalTime' ")
                End If
            End If



            sQuery.Append(" FROM view_flights_new WITH(NOLOCK) ")   ' get operating company at time of flight data - MSW 5/15/20
            '  sQuery.Append(" FROM view_flights WITH(NOLOCK) ")


            sQuery.Append(" WHERE  ")

            If sAircraftID > 0 Then
                sQuery.Append(" (ac_id = " + sAircraftID.ToString.Trim + " )")
            End If

            If Trim(Flight_Id1) <> "" And Trim(Flight_Id2) <> "" Then
                If sAircraftID > 0 Then
                    sQuery.Append(" and ")
                End If
                sQuery.Append(" ffd_unique_flight_id in ('" & Flight_Id1 & "', '" & Flight_Id2 & "') ")



            ElseIf aport_id1 > 0 Or aport_id2 > 0 Then
                If show_one_way = True Then
                    If sAircraftID > 0 Then
                        sQuery.Append(" and ")
                    End If
                    sQuery.Append(" ( ")
                    If aport_id1 > 0 Then
                        sQuery.Append(" ffd_origin_aport_id =  " & aport_id1.ToString)
                    End If
                    If aport_id2 > 0 Then
                        If aport_id1 > 0 Then
                            sQuery.Append("and")
                        End If
                        sQuery.Append("  ffd_dest_aport_id= " & aport_id2.ToString)
                    End If
                    sQuery.Append(") ")
                ElseIf show_one_way = False And aport_id1 > 0 And aport_id2 > 0 Then
                    If sAircraftID > 0 Then
                        sQuery.Append(" and ")
                    End If
                    sQuery.Append(" (( ")
                    sQuery.Append(" ffd_origin_aport_id =  " & aport_id1.ToString)
                    sQuery.Append(" and ffd_dest_aport_id= " & aport_id2.ToString)
                    sQuery.Append(" ) or ( ")
                    sQuery.Append(" ffd_origin_aport_id =  " & aport_id2.ToString)
                    sQuery.Append(" and ffd_dest_aport_id= " & aport_id1.ToString)
                    sQuery.Append(" ) ) ")
                End If
            End If

            If comp_id > 0 Then
                sQuery.Append(" and comp_id = " & comp_id & "")
            End If

            If Trim(Flight_Id1) <> "" And Trim(Flight_Id2) <> "" Then
                ' then dont do the date because we have the flights already 
            ElseIf Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                If Trim(start_date) <> "" Then
                    sQuery.Append(" and ffd_date >= '" & Trim(start_date) & "' ")
                End If
                If Trim(end_date) <> "" Then
                    sQuery.Append(" AND ffd_date <= '" & Trim(end_date) & "' ")
                End If

            End If

            If Trim(product_code_selection) <> "" Then
                sQuery.Append(" " & product_code_selection & " ")
            End If

            If company Then
                sQuery.Append(" group by comp_id, comp_name, comp_city, comp_state, comp_country, comp_email_address,comp_off_phone,  comp_address1, cbus_name")
                'sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone ,contact_mob_phone, comp_address1 ")
                sQuery.Append(" order by NbrFlights desc ")
            ElseIf aircraft Then
                sQuery.Append(" group by amod_make_name, amod_model_name, ac_ser_no_full, base_aport_name, ac_reg_no, faablk_reg_no,ac_ser_no_sort, ac_id,")
                sQuery.Append(" comp_name, comp_address1, comp_city, comp_state, comp_country, ")
                sQuery.Append(" comp_web_address, comp_email_address, comp_off_phone, comp_id, cbus_name")

            Else
                If Trim(Flight_Id1) <> "" And Trim(Flight_Id2) <> "" Then
                    sQuery.Append(" ORDER BY ffd_date asc")
                Else
                    sQuery.Append(" ORDER BY ffd_date desc")
                End If


            End If



            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = serverConnectStr  ' Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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

    Public Function Get_Operator_History_Data(ByVal sAircraftID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()


        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try



            sQuery.Append(" Select   comp_name As Operator, acomprole_start_date As STARTDATE, acomprole_end_date As ENDDATE, acomprole_id, acomprole_notes  ")
            sQuery.Append(" From Aircraft_Company_Role with (NOLOCK) ")
            sQuery.Append(" inner Join Company with (NOLOCK) on comp_id = acomprole_comp_id And comp_journ_id = 0 ")
            sQuery.Append(" where acomprole_ac_id = " & sAircraftID & "")
            sQuery.Append(" order by acomprole_start_date desc  ")



            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Get_Operator_History_Data(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = serverConnectStr  ' Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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
    Public Function getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByRef DropDownList_owner As System.Web.UI.WebControls.DropDownList, ByRef DropDownList_timeframe As System.Web.UI.WebControls.DropDownList, Optional ByVal check_unclean As Boolean = False, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal activetab As Long = 0, Optional ByVal removeIATACode As Boolean = False, Optional ByVal product_code_selection As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()


    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Where") = " "
      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = ""


      If activetab = 7 Then
        sQuery.Append("SELECT  ffd_ac_id, amod_make_name, amod_model_name,  ac_ser_no_full, aircraft.ac_reg_no, comp_name, COUNT(*)  AS num_flights, ")
        sQuery.Append(" sum(ffd_flight_time) as flight_time,")
        sQuery.Append(" sum(ffd_distance) as flight_distance, ")
        sQuery.Append(" SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn ")


        HttpContext.Current.Session.Item("Selection_Listing_Fields") = sQuery.ToString

        HttpContext.Current.Session.Item("Selection_Listing_Table") = (" FROM Aircraft WITH (NOLOCK) ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= ("  inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id  ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner join Aircraft_Reference With (NOLOCK) On cref_ac_id = ac_id And cref_journ_id = ac_journ_id And cref_operator_flag In ('Y', 'O')  ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = ac_journ_id  ")
 
        sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table")) 

      ElseIf activetab = 8 Then

        sQuery.Append("SELECT comp_name, comp_id, count(*) as tcount, ")
        sQuery.Append(" sum(ffd_flight_time) as flight_time,")
        sQuery.Append(" sum(ffd_distance) as flight_distance, ")
        sQuery.Append(" SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn ")


        HttpContext.Current.Session.Item("Selection_Listing_Fields") = sQuery.ToString

        HttpContext.Current.Session.Item("Selection_Listing_Table") = (" FROM Aircraft WITH (NOLOCK) ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= ("  inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id  ")

        sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))

      Else


        sQuery.Append("SELECT ")
        If activetab = 0 Then
          sQuery.Append(" amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ")
        End If

        If aport_id1 > 0 And aport_id2 > 0 Then
          sQuery.Append(" ffd_date as flight_date, (o.aport_name) as origin_aport,")

          sQuery.Append(" o.aport_latitude_decimal as aport_origin_lat,")
          sQuery.Append(" o.aport_longitude_decimal as aport_origin_long, ")
          sQuery.Append(" o.aport_city as 'Orig Airport City',")
          sQuery.Append(" o.aport_state as 'Orig Airport State',")
          sQuery.Append(" o.aport_country as 'Orig Airport Country',")


          sQuery.Append(" (d.aport_name) as dest_aport,")
        Else
          If removeIATACode = True Then
            sQuery.Append(" ffd_date as flight_date, (o.aport_name) as origin_aport,")

            sQuery.Append(" o.aport_latitude_decimal as aport_origin_lat,")
            sQuery.Append(" o.aport_longitude_decimal as aport_origin_long, ")
            sQuery.Append(" o.aport_city as 'Orig Airport City',")
            sQuery.Append(" o.aport_state as 'Orig Airport State',")
            sQuery.Append(" o.aport_country as 'Orig Airport Country',")


            sQuery.Append(" (d.aport_name) as dest_aport,")
          Else
            sQuery.Append(" ffd_date as flight_date, (ffd_origin_aport + ' - ' + o.aport_name) as origin_aport,")

            sQuery.Append(" o.aport_latitude_decimal as aport_origin_lat,")
            sQuery.Append(" o.aport_longitude_decimal as aport_origin_long, ")
            sQuery.Append(" o.aport_city as 'Orig Airport City',")
            sQuery.Append(" o.aport_state as 'Orig Airport State',")
            sQuery.Append(" o.aport_country as 'Orig Airport Country',")


            sQuery.Append(" (ffd_dest_aport + ' - ' + d.aport_name) as dest_aport,")
          End If
        End If





        '  sQuery.Append(" (SELECT top 1 aport_latitude_decimal FROM Airport WHERE (aport_iata_code = ffd_origin_aport OR aport_icao_code = ffd_origin_aport) AND lower(aport_country) = 'united states') AS aport_origin_lat,")
        '  sQuery.Append(" (SELECT top 1 aport_longitude_decimal FROM Airport WHERE (aport_iata_code = ffd_origin_aport OR aport_icao_code = ffd_origin_aport) AND lower(aport_country) = 'united states') AS aport_origin_long,")




        '  sQuery.Append(" (SELECT aport_latitude_decimal FROM Airport WHERE (aport_iata_code = ffd_dest_aport OR aport_icao_code = ffd_dest_aport) AND lower(aport_country) = 'united states') AS aport_dest_lat,")
        ' sQuery.Append(" (SELECT aport_longitude_decimal FROM Airport WHERE (aport_iata_code = ffd_dest_aport OR aport_icao_code = ffd_dest_aport) AND lower(aport_country) = 'united states') AS aport_dest_long,")
        sQuery.Append(" d.aport_latitude_decimal as aport_dest_lat,")
        sQuery.Append(" d.aport_longitude_decimal as aport_dest_long, ")
        sQuery.Append(" d.aport_city as 'Dest Airport City',")
        sQuery.Append(" d.aport_state as 'Dest Airport State',")
        sQuery.Append(" d.aport_country as 'Dest Airport Country',")


        sQuery.Append(" ffd_flight_time as flight_time,")
        sQuery.Append(" ((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn, ")

        sQuery.Append(" ffd_distance as flight_distance, ffd_unique_flight_id as ffd_unique_flight_id")

        HttpContext.Current.Session.Item("Selection_Listing_Fields") = sQuery.ToString

        HttpContext.Current.Session.Item("Selection_Listing_Table") = "Aircraft with (NOLOCK)"

        sQuery.Append(" FROM Aircraft WITH (NOLOCK) ")
      End If



      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "SELECT ", "")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "amod_make_name", "amod_make_name as 'Model Name'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "amod_model_name", "amod_model_name as 'Make Name'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_ser_no_full", "ac_ser_no_full as 'Ser No'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_reg_no", "ac_reg_no as 'Reg No'")

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "num_flights", "'#Flights'")

 
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as flight_date", "as 'Flight Date'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as origin_aport", "as 'Origin Airport'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as dest_aport", "as 'Dest Airport'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as aport_origin_lat", "as 'Origin Lat'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as aport_origin_long", "as 'Origin Long'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as aport_dest_lat", "as 'Dest Lat'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as aport_dest_long", "as 'Dest Long'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as flight_time", "as 'Flight Time'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "as flight_distance", "as 'Flight Distance(nm)'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), ", ffd_unique_flight_id as ffd_unique_flight_id", "")  'get rid of it 


      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn", "cast(SUM((ffd_flight_time* amod_fuel_burn_rate)/60)AS decimal(10^1)) as 'Est Fuel Burn'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn", "cast(((ffd_flight_time* amod_fuel_burn_rate)/60)AS decimal(10^1)) as 'Est Fuel Burn'")



      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), ", ffd_unique_flight_id as ffd_unique_flight_id", "")  'get rid of it 

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name", "comp_name as 'Company Name'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_id", "comp_id as 'Company ID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "tcount", "'Total Count'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "TotalFuelBurn", "'Est Fuel Burn'")

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_ac_id", " ffd_ac_id as 'AC ID'")

      ' make sure the space is in the first line so that it doesnt replace the sums
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_distance ", " cast((ffd_distance * 0.86897624)AS decimal(10^0)) ") ' make sure these ")AS are capital so it will work 
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "sum(ffd_distance)", "  cast((sum(ffd_distance) * 0.86897624)AS decimal(10^0)) ")






      If activetab = 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= ("  inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id ")
        sQuery.Append("  inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id ")
      End If


      If activetab = 8 Then
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id  and  cref_operator_flag IN ('Y', 'O') ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = ac_journ_id ")
        sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id  and  cref_operator_flag IN ('Y', 'O') ")
        sQuery.Append(" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = ac_journ_id ")
      End If



      If check_unclean = True Then
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
        sQuery.Append(" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN View_FAA_Flight_Data_Clean WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
        sQuery.Append(" INNER JOIN View_FAA_Flight_Data_Clean WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
      End If

      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN airport o on  o.aport_id = ffd_origin_aport_id ")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN airport d on  d.aport_id = ffd_dest_aport_id ")

      sQuery.Append(" LEFT OUTER JOIN airport o on  o.aport_id = ffd_origin_aport_id ")
      sQuery.Append(" LEFT OUTER JOIN airport d on  d.aport_id = ffd_dest_aport_id ")
      'ffd_reg_no = '" + sRegNumber.Trim + "' AND 


      If sAircraftID = 0 Then ' aport_id1
        sQuery.Append(" WHERE ")

        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" ffd_journ_id = 0 ")
        sQuery.Append("  ffd_journ_id = 0 ")
        If show_one_way = True Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
          sQuery.Append("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
        Else
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")

          sQuery.Append("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
          sQuery.Append("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
        End If
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" (ffd_ac_id = " + sAircraftID.ToString.Trim + " and ffd_journ_id = 0)")
        sQuery.Append(" WHERE (ffd_ac_id = " + sAircraftID.ToString.Trim + " and ffd_journ_id = 0)")
      End If


      If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
        If Trim(start_date) <> "" Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date >= '" & Trim(start_date) & "' ")
          sQuery.Append(" AND ffd_date >= '" & Trim(start_date) & "' ")
        End If
        If Trim(end_date) <> "" Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date <= '" & Trim(end_date) & "' ")
          sQuery.Append(" AND ffd_date <= '" & Trim(end_date) & "' ")
        End If
      ElseIf Not IsNothing(DropDownList_owner) Then
        If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_date >= ac_purchase_date)")
          sQuery.Append(" AND (ffd_date >= ac_purchase_date)") ' changed to be <= , flight has to be greater than last purchase date
        End If

        If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (DATEDIFF(day, ffd_date, GETDATE()) <= 90)")
          sQuery.Append(" AND (DATEDIFF(day, ffd_date, GETDATE()) <= 90)")
        End If

        If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
          sQuery.Append(" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
        End If
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
        sQuery.Append(" AND (DATEDIFF(m, ffd_date, GETDATE()) <= 12)")
        'sQuery.Append(" AND (YEAR(ffd_date) = " & Year(Date.Now) & " )")

      End If

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(" " & product_code_selection & " ")
      End If


      sQuery.Append(" " & Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), "amod", "Aircraft_Model.amod"))



      If activetab = 7 Then
        HttpContext.Current.Session.Item("Selection_Listing_Group") = (" group by ffd_ac_id, amod_make_name, amod_model_name,  ac_ser_no_full, aircraft.ac_reg_no, comp_name  ")
        sQuery.Append(" group by ffd_ac_id, amod_make_name, amod_model_name,  ac_ser_no_full, aircraft.ac_reg_no, comp_name  ")
        HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY  COUNT(*)  desc ")
        sQuery.Append(" ORDER BY  COUNT(*)  desc ")
      ElseIf activetab = 8 Then
        HttpContext.Current.Session.Item("Selection_Listing_Group") = (" group by comp_name, comp_id  ")
        sQuery.Append(" group by comp_name, comp_id  ")
        HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY  COUNT(*)  desc ")
        sQuery.Append(" ORDER BY  COUNT(*)  desc ")
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY ffd_date")
        sQuery.Append(" ORDER BY ffd_date")
      End If


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = serverConnectStr  ' Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData load datatable " + constrExc.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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
  Public Function GetAirportName(ByRef aport_id As String) As String
    GetAirportName = ""
    Dim bResult As Boolean = False
    Dim sQuery As New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException = Nothing

    Try

      Try

        sQuery.Append("SELECT aport_name from Airport with (NOLOCK)  ")   'ac_reg_no = '" + sRegNumber.Trim + "'
        sQuery.Append(" WHERE aport_id = " & aport_id.ToString.Trim & " ")


        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean</b><br />" + sQuery.ToString

        SqlConn.ConnectionString = serverConnectStr
        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        If SqlReader.HasRows Then
          SqlReader.Read()
          If Not IsDBNull(SqlReader.Item("aport_name")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("aport_name").ToString.Trim) Then
              GetAirportName = SqlReader.Item("aport_name").ToString.Trim
              GetAirportName = Replace(GetAirportName, " Airport", " ")
              GetAirportName = Trim(GetAirportName)
            End If
          End If
        End If

      Catch SqlException

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData SQL " + SqlException.Message

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try


  End Function

    Public Function checkForFAAFlightData(ByRef sRegNumber As String, ByVal sAircraftID As Long, Optional ByVal check_unclean As Boolean = False, Optional ByVal include_faa_sub As Boolean = False) As Boolean

        Dim bResult As Boolean = False
        Dim sQuery As New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException = Nothing

        Try

            Try

                sQuery.Append("SELECT ac_reg_no FROM Aircraft WITH (NOLOCK) WHERE (")   'ac_reg_no = '" + sRegNumber.Trim + "'
                sQuery.Append(" ac_id = " + sAircraftID.ToString.Trim + " AND ac_journ_id = 0)")

                If include_faa_sub = True Then
                    If check_unclean = True Then
                        sQuery.Append(" AND (EXISTS (SELECT NULL FROM FAA_Flight_Data WITH (NOLOCK)")
                    Else
                        sQuery.Append(" AND (EXISTS (SELECT NULL FROM View_FAA_Flight_Data_Clean WITH (NOLOCK)")
                    End If


                    sQuery.Append(" WHERE (ffd_ac_id = ac_id) AND (ffd_journ_id = ac_journ_id)")

                    sQuery.Append(" ))")
                End If
                'sQuery.Append(" AND (DATEDIFF(day,ffd_date, GETDATE()) <= 90)")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean</b><br />" + sQuery.ToString

                SqlConn.ConnectionString = serverConnectStr
                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60

                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                If SqlReader.HasRows Then
                    SqlReader.Read()
                    If Not IsDBNull(SqlReader.Item("ac_reg_no")) Then
                        If Not String.IsNullOrEmpty(SqlReader.Item("ac_reg_no").ToString.Trim) Then
                            ' will also have to verify that this ac is not on the block list
                            bResult = True
                            sRegNumber = SqlReader.Item("ac_reg_no").ToString.Trim
                        End If
                    End If
                End If

            Catch SqlException

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData SQL " + SqlException.Message
                Return bResult

            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean " + ex.Message

            Return bResult

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bResult

    End Function

    Public Function checkForFAAFlightData_Last_Aport(ByRef sRegNumber As String, ByVal sAircraftID As Long, ByRef last_aport_id As Long, ByRef last_aport_lat As String, ByRef last_aport_long As String) As Boolean

        Dim bResult As Boolean = False
        Dim sQuery As New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException = Nothing

        Try

            Try

                sQuery.Append("SELECT top 1 ffd_dest_aport_id, aport_country, aport_latitude_decimal,  aport_longitude_decimal  FROM  View_FAA_Flight_Data_Clean  WITH (NOLOCK) inner join Airport with (NOLOCK) on aport_id = ffd_dest_aport_id WHERE (")   'ac_reg_no = '" + sRegNumber.Trim + "'
                sQuery.Append(" ffd_ac_id = " + sAircraftID.ToString.Trim + " )")
                sQuery.Append(" order by ffd_date desc ")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />checkForFAAFlightData_Last_Aport(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean</b><br />" + sQuery.ToString

                SqlConn.ConnectionString = serverConnectStr
                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60

                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                If SqlReader.HasRows Then
                    SqlReader.Read()


                    If Not IsDBNull(SqlReader.Item("aport_country")) Then

                        If Trim(SqlReader.Item("aport_country")) <> "United States" Then


                            If Not IsDBNull(SqlReader.Item("ffd_dest_aport_id")) Then
                                If Not String.IsNullOrEmpty(SqlReader.Item("ffd_dest_aport_id").ToString.Trim) Then
                                    ' will also have to verify that this ac is not on the block list
                                    bResult = True
                                    last_aport_id = SqlReader.Item("ffd_dest_aport_id")
                                End If

                                If Not String.IsNullOrEmpty(SqlReader.Item("aport_latitude_decimal").ToString.Trim) Then
                                    last_aport_lat = SqlReader.Item("aport_latitude_decimal")
                                End If

                                If Not String.IsNullOrEmpty(SqlReader.Item("aport_longitude_decimal").ToString.Trim) Then
                                    last_aport_long = SqlReader.Item("aport_longitude_decimal")
                                End If

                            End If

                        End If

                    End If

                End If


            Catch SqlException

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData_Last_Aport SQL " + SqlException.Message
                Return bResult

            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData_Last_Aport(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean " + ex.Message

            Return bResult

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bResult

    End Function

    Public Function IS_ON_BLOCKED_LIST(ByVal sRegNumber As String) As Boolean

    Dim bResult As Boolean = False
    Dim sQuery As New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException = Nothing

    Try

      Try

        sQuery.Append("SELECT faablk_reg_no FROM FAA_Blocked_Registration_Numbers WITH (NOLOCK)  ")   'ac_reg_no = '" + sRegNumber.Trim + "'
        sQuery.Append(" WHERE faablk_reg_no = '" & Replace(sRegNumber.ToString.Trim, "-", "") & "' ")

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />IS_ON_BLOCKED_LIST(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean</b><br />" + sQuery.ToString

        SqlConn.ConnectionString = serverConnectStr
        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        If SqlReader.HasRows Then
          SqlReader.Read()
          bResult = True
        End If

      Catch SqlException

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData SQL " + SqlException.Message
        Return bResult

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean " + ex.Message

      Return bResult

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bResult

  End Function



  Public Function displayAirframeTimesData(ByRef dtFlightData As DataTable, ByRef airframe_times_as_of As String, ByRef airframe_total_hours As String, ByRef airframe_total_landings As String, ByVal is_for_ac As Boolean, ByVal aircraftPreviouslyOwnedFlag As String, ByVal aircraftPurchaseDate As String, Optional ByVal delivery_year As Integer = 0, Optional ByVal bShowBlankAcFields As Boolean = False, Optional ByVal is_from As String = "", Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal editLink As String = "", Optional ByVal is_commercial_Ac As Boolean = False, Optional ByRef est_aftt As String = "", Optional ByRef est_landings As String = "", Optional ByRef est_as_of_date As String = "") As String

    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim toggleRowColor As Boolean = False
    Dim sSeparator As String = ""

    Dim totalMiles As Double = 0.0
    Dim totalFlightTime As Double = 0.0
    Dim averageMiles As Double = 0.0
    Dim averageFlightTime As Double = 0.0
    Dim totalFlights As Double = 0.0
    Dim last_flight_date As String = ""
    Dim af_hours_start As Long = 0
    Dim land_start As Long = 0
    Dim tmpAFTT As Double = 0
    Dim tmpLandings As Double = 0
    Dim nr_used As Boolean = False
    Dim bgcolor As String = "#F6CECE"
    Dim ShowEstimation As Boolean = True
    Dim ShowEstimationCSSClass As String = "emphasisColor"
    Dim PurchaseDateGreaterThan As Boolean = False
    Dim temp_faa As String = ""
    Dim hide_other_blocks As Boolean = False
    Dim DisplayField As String = ""
    Dim temp_string As String = ""

    Try


      '  htmlOut.Append("<table cellpadding=""3"" cellspacing=""0"" width=""100%"">")

      '11/11/2015 Changes.
      'There are rules to whether or not you show an estimation.
      'a.	Scenario 1 – we have times as of and airframe hours === Show Estimates
      'b.	Scenario 2 – we have (blank times as of date or airframe hours) and new aircraft and aircraft purchase date >= 1/1/2007  === Show Estimates. Example ID 114292
      'c.	Scenario 3 – otherwise if (blank times as of date or airframe hours)  === Do Not Show Estimates. Example IDs 29132, 6883

      'The bottom line is, the only time we actually care about/there's going to be a change is the third scenario. Otherwise the page will remain the same.
      'In Scenario 3, we are going to set the ShowEstimationCSSClass to be display_none. This will basically hide the estimation. Otherwise the class is blank, and everything
      'carries on as normal. We will also toggle off the ShowEstimation boolean - just so we can hide the text explaining the estimation as well.

      If IsDate(aircraftPurchaseDate) Then
        If DateTime.Parse(aircraftPurchaseDate) >= DateTime.Parse("1/1/2007") Then
          PurchaseDateGreaterThan = True
        End If
      End If

      'We need to set an actual long for this.
      Dim lngAirframeTotalHours As Long = 0

      If IsNumeric(airframe_total_hours) Then
        lngAirframeTotalHours = airframe_total_hours
      End If

      ' commented out MSW 12/7/2015  - showing estimates in all areas
      'If Not String.IsNullOrEmpty(Trim(airframe_times_as_of)) And lngAirframeTotalHours > 0 Then
      '  'We show estimates here.
      'ElseIf (String.IsNullOrEmpty(Trim(airframe_times_as_of)) Or lngAirframeTotalHours = 0) And aircraftPreviouslyOwnedFlag = "N" And PurchaseDateGreaterThan = True Then
      '  'We show estimates here.
      'ElseIf (String.IsNullOrEmpty(Trim(airframe_times_as_of)) Or lngAirframeTotalHours = 0) Then
      '  'We do not show estimates here.
      '  ShowEstimationCSSClass = "display_none"
      '  ShowEstimation = False
      'End If



      If Not String.IsNullOrEmpty(airframe_times_as_of) Then
        '  htmlOut.Append("<tr class=""alt_row""><td valign=""middle"" align=""left""><span class=""label"">&nbsp;&nbsp;Times/Values Current As Of&nbsp;:</span>" + FormatDateTime(IIf(Not String.IsNullOrEmpty(airframe_times_as_of), CDate(airframe_times_as_of), Now()), DateFormat.ShortDate).ToString + "</td>")
        '  htmlOut.Append("<td valign=""middle"" align=""center""><span class=""label"">Air Frame Total Time (AFTT)&nbsp;:</span>" + IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0") + " <em>(hrs)</em></td>")
        ' htmlOut.Append("<td valign=""middle"" align=""right""><span class=""label"">Landings/Cycles&nbsp;:</span>" + IIf(Not String.IsNullOrEmpty(airframe_total_landings), FormatNumber(CDbl(airframe_total_landings), 0, True, False, True), "0") + "&nbsp;&nbsp;</td></tr>")
        If Trim(af_hours_start) <> "" Then

          If Not IsNothing(airframe_total_hours) Then
            If Trim(airframe_total_hours) <> "" Then
              af_hours_start = FormatNumber(CDbl(airframe_total_hours), 0, True, False, True)
            Else
              af_hours_start = "0"
            End If
          Else
            af_hours_start = "0"
          End If


          'af_hours_start = IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0")
        Else
          af_hours_start = 0
        End If


        If Not IsNothing(airframe_total_landings) Then
          If Trim(airframe_total_landings) <> "" Then
            land_start = FormatNumber(CDbl(airframe_total_landings), 0, True, False, True)
          Else
            land_start = 0
          End If
        Else
          land_start = 0
        End If


        ' If Trim(airframe_total_landings) <> "" Then
        'land_start = IIf(Not String.IsNullOrEmpty(airframe_total_landings), FormatNumber(CDbl(airframe_total_landings), 0, True, False, True), "0")
        ' Else
        '    land_start = 0
        '  End If
      ElseIf Not IsNothing(airframe_total_hours) And Not IsNothing(dtFlightData) Then    ' ADDED IN MSw - if no flights or time, make sure you say the AFTT - 3/25/19
        If dtFlightData.Rows.Count = 0 And Trim(airframe_total_hours) <> "" And Trim(airframe_total_hours) <> "0" Then
          '------------ COPIED FROM THE PREVIOUS SECTION -- FOR RARE CASES - WITH NO DATE OR FLIGHT ACTIVITY, BUT AFTT
          If Trim(af_hours_start) <> "" Then

            If Not IsNothing(airframe_total_hours) Then
              If Trim(airframe_total_hours) <> "" Then
                af_hours_start = FormatNumber(CDbl(airframe_total_hours), 0, True, False, True)
              Else
                af_hours_start = "0"
              End If
            Else
              af_hours_start = "0"
            End If


            'af_hours_start = IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0")
          Else
            af_hours_start = 0
          End If


          If Not IsNothing(airframe_total_landings) Then
            If Trim(airframe_total_landings) <> "" Then
              land_start = FormatNumber(CDbl(airframe_total_landings), 0, True, False, True)
            Else
              land_start = 0
            End If
          Else
            land_start = 0
          End If
          '------------------------------
        End If
      End If

      '•	If aircraft year of delivery is prior to 2005 and no AFTT 
      If (Trim(airframe_times_as_of) = "" And delivery_year > 0 And delivery_year < 2005) Then
        hide_other_blocks = True
        '•	OR aftt is older than 6/2005 then do not display estimate blocks on the aircraft or the text below it.
      ElseIf Trim(airframe_times_as_of) <> "" Then
        If CDate(airframe_times_as_of) < CDate("6/1/2005") Then
          hide_other_blocks = True
        End If
      End If

      If Not IsNothing(dtFlightData) Then

        If dtFlightData.Rows.Count > 0 Then

          totalFlights = CDbl(dtFlightData.Rows.Count)

          For Each r As DataRow In dtFlightData.Rows

            If Not IsDBNull(r.Item("flight_time")) Then

              If Not String.IsNullOrEmpty(r.Item("flight_time").ToString) Then
                totalFlightTime += CDbl(r.Item("flight_time").ToString)
              End If
            End If

            If Not IsDBNull(r("flight_distance")) Then
              If Not String.IsNullOrEmpty(r.Item("flight_distance").ToString) Then
                totalMiles += CDbl(r.Item("flight_distance").ToString)
              End If
            End If

            If Not IsDBNull(r("flight_date")) Then
              If Not String.IsNullOrEmpty(r.Item("flight_date").ToString) Then
                If String.IsNullOrEmpty(last_flight_date) Then
                  last_flight_date = r.Item("flight_date").ToString
                Else
                  If CDate(r.Item("flight_date").ToString) > CDate(last_flight_date) Then
                    last_flight_date = r.Item("flight_date").ToString
                  End If
                End If
              End If
            End If

          Next

          '  htmlOut.Append("<tr bgcolor=""white""><td valign=""middle"" align=""center"" colspan=""3"">")

          ' htmlOut.Append("<table cellpadding=""0"" cellspacing=""0"" width=""100%""><tr>")

          If totalFlights > 0 Then

            averageMiles = totalMiles / totalFlights
            averageFlightTime = totalFlightTime / totalFlights

            '    htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Flights</strong>&nbsp;:&nbsp;" + FormatNumber(totalFlights, 0, True, False, True) + "</td>")
          Else
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Flights</strong>&nbsp;:&nbsp;0</td>")
          End If

          If totalMiles > 0 Then
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Distance</strong>&nbsp;<em>(nm)</em>&nbsp;:&nbsp;" + FormatNumber(totalMiles, 0, True, False, True) + "</td>")
          Else
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Distance</strong>&nbsp;<em>(nm)</em>&nbsp;:&nbsp;0</td>")
          End If

          If averageMiles > 0 Then
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Average Distance</strong>&nbsp;<em>(nm)</em>&nbsp;:&nbsp;" + FormatNumber(averageMiles, 0, True, False, True) + "</td>")
          Else
            '    htmlOut.Append("<td valign=""middle"" align=""left""><strong>Average Distance</strong>&nbsp;<em>(nm)</em>&nbsp;:&nbsp;0</td>")
          End If

          If totalFlightTime > 0 Then
            totalFlightTime = (totalFlightTime / 60)

            If is_commercial_Ac = True Then
              totalFlightTime = totalFlightTime + ((totalFlights * 12) / 60)
            Else
              totalFlightTime = totalFlightTime
            End If


            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Flight Time</strong>&nbsp;<em>(min)</em>&nbsp;:&nbsp;" + FormatNumber(totalFlightTime, 0, True, False, True) + "<br /></td>")
          Else
            '    htmlOut.Append("<td valign=""middle"" align=""left""><strong>Total Flight Time</strong>&nbsp;<em>(min)</em>&nbsp;:&nbsp;0</td>")
          End If

          If averageFlightTime > 0 Then
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Average Flight Time</strong>&nbsp;<em>(min)</em>&nbsp;:&nbsp;" + FormatNumber(averageFlightTime, 0, True, False, True) + "</td>")
          Else
            '   htmlOut.Append("<td valign=""middle"" align=""left""><strong>Average Flight Time</strong>&nbsp;<em>(min)</em>&nbsp;:&nbsp;0</td>")
          End If

          '  htmlOut.Append("</tr></table></td></tr>")

          ' add taxi start/stop times to total flight time then add to airframe_total_hours
          ' Dim tmpAFTT As Double = CDbl(airframe_total_hours)
          tmpAFTT = 0

          If Trim(airframe_total_hours) <> "" Then
            tmpAFTT = CDbl(airframe_total_hours)
          End If

          'tmpAFTT += (totalFlightTime + (totalFlights * 0.2))   ' (totalFlights * (.2(hrs){12/60}) =  
          If Trim(airframe_total_hours) <> "" Then
            tmpAFTT = airframe_total_hours + totalFlightTime
          Else
            tmpAFTT = totalFlightTime
          End If

          airframe_total_hours = tmpAFTT.ToString

          '  airframe_total_hours = (af_hours_start + totalFlightTime)


          tmpLandings = 0
          tmpLandings += totalFlights + land_start
          airframe_total_landings = tmpLandings.ToString

          If Not String.IsNullOrEmpty(airframe_times_as_of) Then
            '  htmlOut.Append("<tr class=""alt_row""><td valign=""top"" align=""left""><span class=""label"">&nbsp;&nbsp;Times/Values Current As Of&nbsp;:</span>" + FormatDateTime(IIf(Not String.IsNullOrEmpty(last_flight_date), CDate(last_flight_date), Now()), DateFormat.ShortDate).ToString + "</td>")
            '  htmlOut.Append("<td valign=""top"" align=""center""><span class=""label"">Air Frame Total Time (AFTT)&nbsp;:</span>" + IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0") + " <em>(hrs)</em><br /><em>includes taxi/startup times</em></td>")
            '  htmlOut.Append("<td valign=""top"" align=""right""><span class=""label"">Landings/Cycles&nbsp;:</span>" + IIf(Not String.IsNullOrEmpty(airframe_total_landings), FormatNumber(CDbl(airframe_total_landings), 0, True, False, True), "0") + "&nbsp;&nbsp;</td></tr>")
          End If

        Else
          'htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No airframe times data available!</td></tr>")
        End If



        If CInt(af_hours_start) = CInt(airframe_total_hours) Then
          hide_other_blocks = True
        End If


        htmlOut.Length = 0 ' clear it 
        '--------------------------------------------------------------------------------------------------------

        If Trim(is_from) = "flightdata" Then
          htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))
          htmlOut.Append("<div class=""Box"">")
          htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable large blue'><thead>")

          htmlOut.Append("<tr>")
          htmlOut.Append("<th valign=""middle"" align=""center"" colspan=""1"">&nbsp;</th>")
          htmlOut.Append("<th valign=""middle"" class='right' colspan=""1""><span class=""label"" nowrap='nowrap'>Current Values</span></th>")

          If hide_other_blocks = False Then
            htmlOut.Append("<th valign=""middle"" class='right' colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label"">Flight&nbsp;Activity</span></th>")
            htmlOut.Append("<th valign=""middle"" class='right' colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label"">Estimated" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "&nbsp;", "<br />") & "Values</span></th>")
          End If
          htmlOut.Append("</tr>")
          htmlOut.Append("</thead><tbody>")
        Else
          htmlOut.Append("<table cellpadding=""3"" cellspacing=""0"" width=""100%"" border=""1"">")
          If is_for_ac = True Then
            htmlOut.Append("<tr class=""noBorder"">")
            htmlOut.Append("<td valign=""middle"" align=""left"" colspan=""1""><span class=""acSpecShow display_none""><font class='sub_section_title_text subHeader'>Airframe</font></span></td>")
            htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"" class='header'><span nowrap='nowrap' class=""label gray"">Current" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "&nbsp;", "<br />") & "Values</span></td>")
            If hide_other_blocks = False Then
              htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label gray"">Flight&nbsp;Activity</span></td>")
              htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label gray"">Estimated" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "&nbsp;", "<br />") & "Values</span></td>")
            End If
            htmlOut.Append("</tr>")
          Else
            htmlOut.Append("<tr>")
            htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"">&nbsp;</td>")
            htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1""><span class=""label"" nowrap='nowrap'>Current Values</span></td>")

            If hide_other_blocks = False Then
              htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label"">Flight&nbsp;Activity</span></td>")
              htmlOut.Append("<td valign=""middle"" align=""center"" colspan=""1"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """ nowrap='nowrap'><span class=""label"">Estimated" & IIf(HttpContext.Current.Session.Item("isMobile") = False, "&nbsp;", "<br />") & "Values</span></td>")
            End If
            htmlOut.Append("</tr>")
          End If
        End If


        If Trim(is_from) = "flightdata" Then
          htmlOut.Append("<tr>")
        Else
          htmlOut.Append("<tr class=""alt_row"">")
        End If


        htmlOut.Append("<td valign=""middle"" align=""left"" class='header'><span><b>Data Valid As of</b></span></td>")
        htmlOut.Append("<td valign=""middle"" align=""right"">")  ' current
        est_as_of_date = ""
        If Trim(airframe_times_as_of) <> "" Then
          htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(IIf(Not String.IsNullOrEmpty(airframe_times_as_of), CDate(airframe_times_as_of), Now()), DateFormat.ShortDate).ToString()))
          est_as_of_date = FormatDateTime(IIf(Not String.IsNullOrEmpty(airframe_times_as_of), CDate(airframe_times_as_of), Now()), DateFormat.ShortDate).ToString()
        Else
          htmlOut.Append("N/R")
          nr_used = True
        End If

        htmlOut.Append("&nbsp;</td>")

        If hide_other_blocks = False Then
          htmlOut.Append("<td valign=""middle"" align=""right"" nowrap='nowrap' bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")  ' current

          If Not IsNothing(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
            temp_faa = HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date
          Else
            temp_faa = "1/1/2001"
          End If



          If Trim(airframe_times_as_of) = "" Then  ' if blank, then show the word beganning
            htmlOut.Append("Beginning - ")

            If Not IsNothing(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)))
            Else
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(Date.Now.Date, DateFormat.ShortDate)))
            End If

            htmlOut.Append("&nbsp;</td>")

            htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")  ' current

            ' If Trim(last_flight_date) <> "" Then
            '   htmlOut.Append(FormatDateTime(IIf(Not String.IsNullOrEmpty(last_flight_date), CDate(last_flight_date), Now()), DateFormat.ShortDate).ToString())
            '  End If 
            If Not IsNothing(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)))
              est_as_of_date = FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)
            Else
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(Date.Now.Date, DateFormat.ShortDate)))
            End If
            htmlOut.Append("&nbsp;</td>")
          ElseIf DateDiff(DateInterval.Day, CDate(airframe_times_as_of), CDate(temp_faa)) < 0 Then ' if not 
            htmlOut.Append(" - ")
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")  ' current
            If Trim(airframe_times_as_of) <> "" Then
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(IIf(Not String.IsNullOrEmpty(airframe_times_as_of), CDate(airframe_times_as_of), Now()), DateFormat.ShortDate).ToString()))
              est_as_of_date = FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)
            End If
            htmlOut.Append("&nbsp;</td>")
          Else
            htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(IIf(Not String.IsNullOrEmpty(airframe_times_as_of), CDate(airframe_times_as_of), Now()), DateFormat.ShortDate).ToString()))

            htmlOut.Append(" - ")
            ' If Trim(last_flight_date) <> "" Then
            '   htmlOut.Append(FormatDateTime(IIf(Not String.IsNullOrEmpty(last_flight_date), CDate(last_flight_date), Now()), DateFormat.ShortDate).ToString())
            ' End If  

            If Not IsNothing(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate))) 
            Else
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(Date.Now.Date, DateFormat.ShortDate)))
            End If

            htmlOut.Append("&nbsp;</td>")

            htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")  ' current

            ' If Trim(last_flight_date) <> "" Then
            '   htmlOut.Append(FormatDateTime(IIf(Not String.IsNullOrEmpty(last_flight_date), CDate(last_flight_date), Now()), DateFormat.ShortDate).ToString())
            '  End If 
            If Not IsNothing(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)))
              est_as_of_date = FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date, DateFormat.ShortDate)
            Else
              htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(Date.Now.Date, DateFormat.ShortDate)))
            End If
            htmlOut.Append("&nbsp;</td>")
          End If
        End If


        htmlOut.Append("</tr>")

        htmlOut.Append("<tr>")

        If is_for_ac = True Then
          htmlOut.Append("<td valign=""middle"" align=""left"" class='header'><span title=""Airframe Total Time"" alt=""Airframe Total Time"" class=""help_cursor text_underline""><b>AFTT</b></span> (hrs):</td>")
        Else
          htmlOut.Append("<td valign=""middle"" align=""left"" class='header'><span title=""Airframe Total Time"" alt=""Airframe Total Time"" class=""help_cursor text_underline""><b>AFTT</b></span> (hrs):</td>")
        End If


        htmlOut.Append("<td valign=""middle"" align=""right"">")
        If af_hours_start > 0 Then
          htmlOut.Append(FormatNumber(af_hours_start, 0))
          ' this should be set if we are hiding the other blocks
          If hide_other_blocks = True Then
            est_aftt = FormatNumber(af_hours_start, 0)
          End If
        Else
          htmlOut.Append("N/R")
          nr_used = True
        End If
        htmlOut.Append("&nbsp;</td>")

        If hide_other_blocks = False Then
          htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")
          '  htmlOut.Append("<A href='#' alt='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.' title='JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.'>")
          If totalFlightTime > 0 Then
            htmlOut.Append(FormatNumber(totalFlightTime, 0))
          Else
            htmlOut.Append("N/R")
            nr_used = True
          End If
          '  htmlOut.Append("</a>&nbsp;</td>")
          htmlOut.Append("&nbsp;</td>")
          htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")
          If Trim(airframe_total_hours) <> "" Then
            htmlOut.Append(IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0"))
            est_aftt = IIf(Not String.IsNullOrEmpty(airframe_total_hours), FormatNumber(CDbl(airframe_total_hours), 0, True, False, True), "0")
          Else
          End If
          htmlOut.Append("&nbsp;</td>")
        End If

        htmlOut.Append("</tr>")


        htmlOut.Append("<tr class=""alt_row"">")
        htmlOut.Append("<td valign=""middle"" align=""left"" class='header'><span><b>Landings/Cycles:</b></span></td>")

        htmlOut.Append("<td valign=""middle"" align=""right"">")
        If land_start > 0 Then
          htmlOut.Append(FormatNumber(land_start, 0))
          ' this should be set if we are hiding the other blocks
          If hide_other_blocks = True Then
            est_landings = FormatNumber(land_start, 0)
          End If
        Else
          htmlOut.Append("N/R")
          nr_used = True
        End If
        htmlOut.Append("&nbsp;</td>")

        If hide_other_blocks = False Then
          htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")

          If totalFlights > 0 Then
            htmlOut.Append(FormatNumber(totalFlights, 0))
          Else
            htmlOut.Append("N/R")
            nr_used = True
          End If

          htmlOut.Append("&nbsp;</td>")

          htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")
          If IsNumeric(airframe_total_landings) Then
            htmlOut.Append(IIf(Not String.IsNullOrEmpty(airframe_total_landings), FormatNumber(CDbl(airframe_total_landings), 0, True, False, True), "0"))
            est_landings = IIf(Not String.IsNullOrEmpty(airframe_total_landings), FormatNumber(CDbl(airframe_total_landings), 0, True, False, True), "0")
          End If
          htmlOut.Append("&nbsp;</td>")
        End If

        htmlOut.Append("</tr>")

        DisplayField = ""
        If hide_other_blocks = False Then
          If totalMiles > 0 Then
            DisplayField += FormatNumber(totalMiles, 0, True, False, True)
          Else
            DisplayField += "N/R"
            nr_used = True
          End If
        End If

        If DisplayField <> "" Or bShowBlankAcFields = True Then
          htmlOut.Append("<tr><td valign=""middle"" align=""left"" class='header'><span><b>Nautical Miles:</b></span></td><td valign=""middle"" align=""right"">&nbsp;</td>")
          If hide_other_blocks = False Then
            htmlOut.Append("<td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>")
            If nr_used = True Then
              htmlOut.Append(DisplayField)
            Else
              htmlOut.Append(FormatNumber(ConvertStatuteMileToNauticalMile(DisplayField), 0))
            End If
            htmlOut.Append("&nbsp;</td><td valign=""middle"" align=""right"" bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """>&nbsp;</td>")
          End If
          htmlOut.Append("</tr>")
        End If

        DisplayField = ""

        htmlOut.Append("<tr><td colspan='4' align='center' class='header'>")
        If nr_used = True Or hide_other_blocks = False Then
          If is_for_ac = True Then
            htmlOut.Append("<div class=""searchCriteria slideoutToolTip""><p title='Information' class='blue'>")
          End If
          If nr_used = True Then
            htmlOut.Append("<span class='label'><font size='-8'>N/R = Not Reported</font></span><br /><br />")
          End If

          If hide_other_blocks = False Then
            htmlOut.Append("<span class=""" & ShowEstimationCSSClass & " header""><font size='-9'><span class=""label"">Flight Activity <span title=""Airframe Total Time"" alt=""Airframe Total Time"" class=""help_cursor text_underline"">AFTT</span>:</span> For Commercial Aircraft, JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.</font></span><br /><br />")

            htmlOut.Append("<span class=""" & ShowEstimationCSSClass & " header""><font size='-9'><span class=""label"">Flight Activity Since Last Verified:</span> The red columns above represent data received from independent flight data sources and reported to clients for their interpretation and use.  JETNET is not responsible for any errors or omissions in the summarization or presentation of flight activity data. <br /><br /><a href='/help/documents/589.pdf' target='_blank' class=""display_block"">Flight Data As of " & HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date & "</a></font></span>")
          End If
          If is_for_ac = True Then
            htmlOut.Append("</p></div>")

          End If
        End If

        If is_for_ac Then
          If Not String.IsNullOrEmpty(editLink) Then
            htmlOut.Append(editLink)
          End If
        End If
        htmlOut.Append("</td></tr>")

        htmlOut.Append("</table>")

        If Trim(is_from) = "flightdata" Then
          htmlOut.Append("</body></div>")
        End If

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No airframe times data available!</td></tr>")
      End If


      If Trim(is_from) = "flightdata" Then
        'bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """
        temp_string = htmlOut.ToString
        htmlOut.Length = 0
        temp_string = Replace(temp_string, "bgcolor='" & bgcolor & "' class=""" & ShowEstimationCSSClass & """", "")
        htmlOut.Append(temp_string)
      End If

      ' htmlOut.Append("</table>")

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayAirframeTimesData(ByRef dtAirframeTimesData As DataTable) As String " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString

    htmlOut = Nothing

  End Function
  Public Shared Function ConvertStatuteMileToNauticalMile(ByVal dSMile As Double) As Double

    Dim dNMile As Double

    dNMile = 0.0#
    If CDbl(dSMile) > 0.0# Then
      dNMile = CDbl(dSMile) * 0.86897624
    End If
    ConvertStatuteMileToNauticalMile = dNMile

  End Function ' ConvertStatuteMileToNauticalMile



  Public Function getAllFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal date_of_current_times As String, Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ffd_date AS flight_date, ffd_origin_aport As origin_aport,")
      '    sQuery.Append(" (SELECT top 1 aport_latitude_decimal FROM Airport WHERE (aport_iata_code = ffd_origin_aport OR aport_icao_code = ffd_origin_aport) AND lower(aport_country) = 'united states') AS aport_origin_lat,")
      ' sQuery.Append(" (SELECT top 1 aport_longitude_decimal FROM Airport WHERE (aport_iata_code = ffd_origin_aport OR aport_icao_code = ffd_origin_aport) AND lower(aport_country) = 'united states') AS aport_origin_long,")

      ' sQuery.Append(" (SELECT aport_latitude_decimal FROM Airport WHERE (aport_iata_code = ffd_dest_aport OR aport_icao_code = ffd_dest_aport) AND lower(aport_country) = 'united states') AS aport_dest_lat,")
      '  sQuery.Append(" (SELECT aport_longitude_decimal FROM Airport WHERE (aport_iata_code = ffd_dest_aport OR aport_icao_code = ffd_dest_aport) AND lower(aport_country) = 'united states') AS aport_dest_long,")

      sQuery.Append(" o.aport_latitude_decimal AS aport_origin_lat,")
      sQuery.Append(" o.aport_longitude_decimal AS aport_origin_long, ")
      sQuery.Append(" (ffd_dest_aport + ' - ' + d.aport_name) AS dest_aport,")
      '  sQuery.Append(" ffd_dest_aport AS dest_aport,")
      sQuery.Append(" d.aport_latitude_decimal AS aport_dest_lat,")
      sQuery.Append(" d.aport_longitude_decimal AS aport_dest_long, ")
      sQuery.Append(" ffd_flight_time AS flight_time,")
      sQuery.Append(" ffd_distance AS flight_distance")
      sQuery.Append(" FROM Aircraft WITH (NOLOCK) INNER JOIN  FAA_Flight_Data WITH (NOLOCK) ON")
      sQuery.Append(" ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
      sQuery.Append(" inner join aircraft_model with (NOLOCK) on amod_id = ac_amod_id ")
      sQuery.Append(" LEFT OUTER JOIN airport o on  o.aport_id = ffd_origin_aport_id ")
      sQuery.Append(" LEFT OUTER JOIN airport d on  d.aport_id = ffd_dest_aport_id ")
      'ffd_reg_no = '" + sRegNumber.Trim + "' AND 
      If sAircraftID = 0 Then ' aport_id1
        sQuery.Append(" WHERE  ffd_journ_id = 0 ")
        If show_one_way = True Then
          sQuery.Append("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
        Else
          sQuery.Append("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
          sQuery.Append("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
        End If
      Else
        sQuery.Append(" WHERE (ffd_ac_id = " + sAircraftID.ToString.Trim + " and ffd_journ_id = 0) and ffd_hide_flag = 'N' ")
      End If


      sQuery.Append(" " & Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), "amod", "Aircraft_Model.amod"))



      If Trim(date_of_current_times) <> "" Then
        sQuery.Append(" and ffd_date > '" & date_of_current_times & "' ")
      End If
      sQuery.Append(" ORDER BY ffd_date")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getAllFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = serverConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getAllFAAFlightData load datatable " + constrExc.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getAllFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As DataTable " + ex.Message
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

#End Region




End Class
