
Partial Public Class short_spec_sheet_aspx

  Inherits System.Web.UI.Page

  Dim aCommonEvo As New commonEVO

  Public report_name As String ' Used for defining name of pdf report
  Public make_model_name As String = ""
  Public amod_ID As Integer
  Public avgyear As Integer = 0
  Public totalcount As Integer = 0
  Public totalInOpcount As Integer = 0
  Public ac_for_sale As Integer = 0
  Public ac_exclusive_sale As Integer = 0
  Public ac_lease As Integer = 0
  Public w_owner As Integer = 0
  Public s_owner As Integer = 0
  Public f_owner As Integer = 0
  Public o_stage As Integer = 0
  Public t_stage As Integer = 0
  Public th_stage As Integer = 0
  Public f_stage As Integer = 0
  Public daysonmarket As Integer = 0
  Public daysonmarket2 As Integer = 0
  Public forsaleavghigh As String = ""
  Public forsaleavlow As String = "199999999999999999999999999999999999999999999999999999"
  Public allhigh As Integer = 0
  Public alllow As Integer = 0
  Public per As Double = 0.0
  Public per2 As Double = 0.0
  Public per3 As Double = 0.0
  Public days As Long = 0

  Public TYPE_OF_AC As String = ""
  Public PDF_Page_Flag As String = "N"
  Public ac_id As Integer

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ' If Not IsPostBack Then

    If Not IsNothing(Request("ac_id")) Then
      If Not String.IsNullOrEmpty(Request("ac_id").ToString) Then
        ac_id = CLng(Request("ac_id").ToString)
      End If
    End If

    If Session("show_cost_values") = "" Then
      Session("show_cost_values") = "Yes"
    End If

    Dim fSubins_platform_os As String = commonEVO.getBrowserCapabilities(Request.Browser)


    Dim sErrorString As String = ""

    If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
      Response.Write("error in load preferences : " + sErrorString)
    End If

    report_name = ""
    If Me.WD.SelectedValue.ToString = "Word" Then
      report_name += Session.Item("localUser").crmSubSubID.ToString.Trim + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString.Trim + "_" + commonEvo.GenerateFileName("PDF_ShortSpecSheet", ".doc", False)
    Else
      report_name += Session.Item("localUser").crmSubSubID.ToString.Trim + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString.Trim + "_" + commonEvo.GenerateFileName("PDF_ShortSpecSheet", ".html", False)
    End If

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs As System.Data.SqlClient.SqlDataReader : localAdoRs = Nothing

    Try

      SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      Dim Query As String : Query = ""

      Query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " + ac_id.ToString

      SqlCommand.CommandText = Query
      localAdoRs = SqlCommand.ExecuteReader()

      If localAdoRs.HasRows Then
        localAdoRs.Read()
        TYPE_OF_AC = localAdoRs.Item("amod_make_name") & " " & localAdoRs.Item("amod_model_name")
        makemodelname.Text = TYPE_OF_AC
      End If

      localAdoRs.Close()
      localAdoRs.Dispose()

    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

  End Sub

  Protected Sub btnRunReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRunReport.Click
    Try

      ' crate a string to hold the PDF output info
      Dim ViewToPDF As String = ""

      'ViewToPDF = ViewToPDF & Short_Header(ac_id)

      Dim SqlException2 As System.Data.SqlClient.SqlException : SqlException2 = Nothing
      Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
      Dim adoRSAircraft2 As System.Data.SqlClient.SqlDataReader : adoRSAircraft2 = Nothing
      SqlConn2.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      'end Select
      SqlConn2.Open()

      SqlCommand2.Connection = SqlConn2
      SqlCommand2.CommandType = System.Data.CommandType.Text
      SqlCommand2.CommandTimeout = 60
      Dim Query2 As String : Query2 = ""
      '-----------------------------


      Dim htmlOutput As String = ""
      Dim ac_apu_model_name As String = ""
      Dim ac_apu_tot_hrs As String = ""
      Dim ac_apu_ser_no As String = ""
      Dim xLoop As Integer
      Dim nloopCount As Integer = 4
      Dim sAirframeType As String = ""
      Dim other_engine_info As String = ""
      Dim ac_airframe_tot_hrs As String = ""
      Dim engine_col_counter As Integer = 1
      Dim temp_string As String = ""
      Dim temp_string2 As String = ""
      Dim temp_moyear As String = ""
      Dim temp_ex_moyear As String = ""
      htmlOutput = htmlOutput & Draw_Black_Line()


      'SECTION 2  - AIRCRAFT INFO  -----------------------------------------------------------------------------------------------------------------------------------


      Query2 = "SELECT amod_make_name, ac_reg_no_expiration_date, ac_interior_doneby_name, ac_interior_rating, ac_passenger_count, ac_asking_price, ac_purchase_date, amod_model_name, ac_year, ac_asking, ac_status, ac_ser_no_full, ac_mfr_year, ac_list_date, ac_reg_no, ac_maintained, ac_forsale_flag, ac_damage_history_notes, ac_confidential_notes, ac_airframe_tot_landings, ac_upd_date, ac_exclusive_flag, ac_times_as_of_date, ac_lease_flag, ac_interior_moyear, ac_exterior_doneby_name, ac_exterior_moyear, ac_exterior_rating"
      Query2 = Query2 & ", ac_apu_model_name, ac_foreign_currency_name, ac_foreign_currency_price, ac_apu_tot_hrs, ac_apu_ser_no, ac_apu_soh_hrs, ac_apu_shi_hrs, ac_main_eoh_moyear, ac_maint_eoh_by_name" ' for apu section

      ' for engine section-------------------
      Query2 = Query2 & ", ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_engine_1_soh_hrs, ac_engine_2_soh_hrs, ac_engine_3_soh_hrs, ac_engine_4_soh_hrs"
      Query2 = Query2 & ", ac_engine_1_shi_hrs, ac_engine_2_shi_hrs, ac_engine_3_shi_hrs, ac_engine_4_shi_hrs, ac_engine_1_tbo_hrs, ac_engine_2_tbo_hrs, ac_engine_3_tbo_hrs, ac_engine_4_tbo_hrs"
      Query2 = Query2 & ", ac_engine_1_snew_cycles, ac_engine_2_snew_cycles, ac_engine_3_snew_cycles, ac_engine_4_snew_cycles, ac_engine_1_soh_cycles, ac_engine_2_soh_cycles, ac_engine_3_soh_cycles, ac_engine_4_soh_cycles"
      Query2 = Query2 & ", ac_engine_1_shs_cycles, ac_engine_2_shs_cycles, ac_engine_3_shs_cycles, ac_engine_4_shs_cycles, ac_engine_1_ser_no, ac_engine_2_ser_no, ac_engine_3_ser_no, ac_engine_4_ser_no, ac_engine_name, ac_airframe_tot_hrs"
      ' for engine section-------------------


      Query2 = Query2 & " FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query2 = Query2 & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)
      ' Query = Query & GenerateProductCodeSelectionQuery(session("Product_Code"), session("UserTierLevel"), False, False, False)

      SqlCommand2.CommandText = Query2
      adoRSAircraft2 = SqlCommand2.ExecuteReader()
      adoRSAircraft2.Read()

      If adoRSAircraft2.HasRows Then
        htmlOutput = htmlOutput & "<table width='100%'>"
        ''''''''''''''''''''''''''''''''''''''''''''
        ' start the Aircraft Identification Status

        ''''''''''''''''''''''''''''''''''''''''''''

        ' ------------------------------ THIS IS FIRST ROW OF BLOCK ------------------------
        ' make
        If Not IsDBNull(adoRSAircraft2("amod_make_name")) Or Not IsDBNull(adoRSAircraft2("amod_make_name")) Then
          htmlOutput = htmlOutput & "<tr valign='top'><td class='small_bold_text' width='10%'>Model</td><td class='small_text' width='15%'>: " & adoRSAircraft2("amod_make_name").ToString & " " & adoRSAircraft2("amod_model_name").ToString & "</td>"
        Else
          htmlOutput = htmlOutput & "<tr valign='top'><td class='small_bold_text'>Model</td><td class='small_bold_text'>:&nbsp;</td>"
        End If



        'If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_name")) Then
        'htmlOutput = htmlOutput & "<td class='small_bold_text'>Ask Amt (" & adoRSAircraft2("ac_foreign_currency_name") & "): </td><td class='small_text'>" & FormatCurrency(adoRSAircraft2("ac_foreign_currency_price"), 0) & "</td>"
        'Else
        htmlOutput = htmlOutput & "<td class='small_bold_text'>&nbsp;</td><td class='small_bold_text'>&nbsp;</td>" ' leave blank
        'End If

        If Me.WD.SelectedValue = "Word" Then
          If Not IsDBNull(adoRSAircraft2("ac_asking_price")) Or Not IsDBNull(adoRSAircraft2("ac_asking")) Then
            If Not IsDBNull(adoRSAircraft2("ac_asking")) Then
              If adoRSAircraft2("ac_asking") = "Price" Then
                htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>"

                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_name")) Then
                  htmlOutput = htmlOutput & "Ask Amt (" & adoRSAircraft2("ac_foreign_currency_name") & ") "
                  htmlOutput = htmlOutput & "<br>Ask Amt (USD)"
                Else
                  htmlOutput = htmlOutput & "Ask Amt (USD)"
                End If
                htmlOutput = htmlOutput & "</td><td class='small_text' width='17%'>"

                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_price")) Then
                  htmlOutput = htmlOutput & ": " & FormatNumber(adoRSAircraft2("ac_foreign_currency_price"), 0)
                  htmlOutput = htmlOutput & "<br>: " & FormatCurrency(adoRSAircraft2("ac_asking_price"), 0).ToString
                Else
                  htmlOutput = htmlOutput & ": " & FormatCurrency(adoRSAircraft2("ac_asking_price"), 0).ToString
                End If
              Else
                htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Ask Amt (USD)"
                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_name")) Then
                  htmlOutput = htmlOutput & "<br>Ask Amt (" & adoRSAircraft2("ac_foreign_currency_name") & ") "
                End If
                htmlOutput = htmlOutput & "</td>"
                htmlOutput = htmlOutput & "<td class='small_text' width='17%'>: " & adoRSAircraft2("ac_asking").ToString
                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_price")) Then
                  htmlOutput = htmlOutput & "<br>: " & FormatNumber(adoRSAircraft2("ac_foreign_currency_price"), 0)
                End If
              End If

              htmlOutput = htmlOutput & "</td>"
            End If
          Else
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Ask Amt (USD)</td><td class='small_text' width='17%'>: &nbsp;</td>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_purchase_date")) Then
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Purch Date</td><td class='small_text' width='10%'>: " & FormatDateTime(adoRSAircraft2("ac_purchase_date"), vbShortDate).ToString & "</td></tr>"
          Else
            htmlOutput = htmlOutput & "<td class='small_bold_text'>&nbsp;</td><td class='small_bold_text'>&nbsp;</td></tr>"
          End If
        Else

          If Not IsDBNull(adoRSAircraft2("ac_asking_price")) Or Not IsDBNull(adoRSAircraft2("ac_asking")) Then
            If Not IsDBNull(adoRSAircraft2("ac_asking")) Then

              If adoRSAircraft2("ac_asking") = "Price" Then
                htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>"
                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_name")) Then
                  htmlOutput = htmlOutput & "Ask Amt (" & adoRSAircraft2("ac_foreign_currency_name") & ") "
                  htmlOutput = htmlOutput & "<br>Asking Amt (USD)"
                Else
                  htmlOutput = htmlOutput & "Asking Amt (USD)"
                End If
                htmlOutput = htmlOutput & "</td><td class='small_text' width='17%'>"

                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_price")) Then
                  htmlOutput = htmlOutput & ": " & FormatNumber(adoRSAircraft2("ac_foreign_currency_price"), 0)
                  htmlOutput = htmlOutput & "<br>: " & FormatCurrency(adoRSAircraft2("ac_asking_price"), 0).ToString
                Else
                  htmlOutput = htmlOutput & ": " & FormatCurrency(adoRSAircraft2("ac_asking_price"), 0).ToString
                End If


                htmlOutput = htmlOutput & "</td>"
              Else

                htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Asking Amt (USD)"
                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_name")) Then
                  htmlOutput = htmlOutput & "<br>Ask Amt (" & adoRSAircraft2("ac_foreign_currency_name") & ") "
                End If
                htmlOutput = htmlOutput & "</td>"
                htmlOutput = htmlOutput & "<td class='small_text' width='17%'>: " & adoRSAircraft2("ac_asking").ToString

                If Not IsDBNull(adoRSAircraft2("ac_foreign_currency_price")) Then
                  htmlOutput = htmlOutput & "<br>: " & FormatNumber(adoRSAircraft2("ac_foreign_currency_price"), 0)
                End If
                htmlOutput = htmlOutput & "</td>"

              End If
            End If



          Else
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Asking Amt (USD)</td><td class='small_text' width='17%'>: &nbsp;</td>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_purchase_date")) Then
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Purchase Date</td><td class='small_text' width='10%'>: " & FormatDateTime(adoRSAircraft2("ac_purchase_date"), vbShortDate).ToString & "</td></tr>"
          Else
            htmlOutput = htmlOutput & "<td class='small_bold_text'>&nbsp;</td><td class='small_bold_text'>&nbsp;</td></tr>"
          End If
        End If

        ' ------------------------------ THIS IS FIRST ROW OF BLOCK ------------------------




        ' ------------------------------ THIS IS SECOND ROW OF BLOCK ------------------------


        If Not Me.BR.Checked Then
          If Not IsDBNull(adoRSAircraft2("ac_ser_no_full")) Then
            If Not String.IsNullOrEmpty(adoRSAircraft2("ac_ser_no_full").ToString) Then
              htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Serial #</td><td class='small_text' width='15%'>: " & adoRSAircraft2("ac_ser_no_full").ToString & "</font></td>"
            Else
              htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Serial #</td><td class='small_text' width='15%'>: &nbsp;</font></td>"
            End If
          End If
        Else
          htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'></td><td class='small_text' width='15%'>&nbsp;</font></td>"
        End If


        ' serial number

        If Not IsDBNull(adoRSAircraft2("ac_year")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='7%'>YR DLV</td><td class='small_text' width='13%'>: " & adoRSAircraft2("ac_year").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='7%'>YR DLV</td><td class='small_text' width='13%'>: &nbsp;</font></td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_status")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Status</td><td class='small_text' width='17%'>: " & adoRSAircraft2("ac_status".ToString) & "</td>"
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>Status</td><td class='small_text' width='17%'>&nbsp;</td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_upd_date")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Date Edited</td><td class='small_text' width='10%'>: " & FormatDateTime(adoRSAircraft2("ac_upd_date"), vbShortDate).ToString & "</td></tr>" ' Date Edited ac_upd_date
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Date Edited</td><td class='small_text' width='10%'>:&nbsp;</td></tr>" ' Date Edited ac_upd_date
        End If

        ' ------------------------------ THIS IS SECOND ROW OF BLOCK ------------------------



        ' ------------------------------ THIS IS THIRD ROW OF BLOCK ------------------------
        'If Not Me.BR.Checked Then
        '  If Not IsDBNull(adoRSAircraft2("ac_reg_no")) Then
        '    If String.IsNullOrEmpty(adoRSAircraft2("ac_ser_no_full").ToString) Then
        '      htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Registration #</td><td class='small_text' width='15%'>: " & adoRSAircraft2("ac_reg_no").ToString & "</td>"
        '    Else
        '      htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Registration #</td><td class='small_text' width='15%'>:&nbsp;</td>"
        '    End If

        '  End If
        'Else
        '  htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'></td><td class='small_text' width='15%'>&nbsp;</font></td>"
        'End If

        If Me.WD.SelectedValue = "Word" Then
          If Not Me.BR.Checked Then
            If Not IsDBNull(adoRSAircraft2("ac_reg_no")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft2("ac_ser_no_full").ToString) Then
                htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Reg #</td><td class='small_text' width='20%'>: " & adoRSAircraft2("ac_reg_no").ToString
                If Not IsDBNull(adoRSAircraft2("ac_reg_no_expiration_date")) Then
                  htmlOutput = htmlOutput & "  <font class='small_bold_text'> Exp:  </font>" & FormatDateTime(adoRSAircraft2("ac_reg_no_expiration_date"), vbShortDate).ToString
                End If
                htmlOutput = htmlOutput & "</td>"
              Else
                htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Reg #</td><td class='small_text' width='20%'>: &nbsp;</td>"
              End If
            Else
              htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Reg #</td><td class='small_text' width='20%'>: &nbsp;</td>"
            End If
          Else
            htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'></td><td class='small_text' width='15%'>&nbsp;</font></td>"
          End If
        Else
          If Not Me.BR.Checked Then
            If Not IsDBNull(adoRSAircraft2("ac_reg_no")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft2("ac_ser_no_full").ToString) Then
                htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Registration #</td><td class='small_text' width='20%'>: " & adoRSAircraft2("ac_reg_no").ToString
                If Not IsDBNull(adoRSAircraft2("ac_reg_no_expiration_date")) Then
                  htmlOutput = htmlOutput & "  <font class='small_bold_text'> Exp:  </font>" & FormatDateTime(adoRSAircraft2("ac_reg_no_expiration_date"), vbShortDate).ToString
                End If
                htmlOutput = htmlOutput & "</td>"
              Else
                htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Registration #</td><td class='small_text' width='20%'>: &nbsp;</td>"
              End If
            Else
              htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'>Registration #</td><td class='small_text' width='20%'>: &nbsp;</td>"
            End If
          Else
            htmlOutput = htmlOutput & "<tr><td class='small_bold_text' width='10%'></td><td class='small_text' width='15%'>&nbsp;</font></td>"
          End If
        End If



        ' year of manufacture
        If Not IsDBNull(adoRSAircraft2("ac_mfr_year")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='7%'>YR MFT</td><td class='small_text' width='13%'>: " & adoRSAircraft2("ac_mfr_year").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='7%'>YR MFT</td><td class='small_text' width='13%'>:&nbsp;</font></td>"
        End If

        htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%'>For Sale (Y/N)&nbsp;</td><td class='small_text' width='17%'>:&nbsp;" & adoRSAircraft2("ac_forsale_flag").ToString & "</td>" ' FOR SALE (Y/N)

        If Not IsDBNull(adoRSAircraft2("ac_list_date")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Date Listed</td><td class='small_text' width='10%'>: " & FormatDateTime(adoRSAircraft2("ac_list_date"), vbShortDate).ToString & "</font></td></tr>"
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Date Listed</td><td class='small_text' width='10%'>:&nbsp;</font></td></tr>"
        End If

        ' ------------------------------ THIS IS THIRD ROW OF BLOCK ------------------------

        ' ------------------------------ THIS IS FOURTH ROW OF BLOCK ------------------------

        htmlOutput = htmlOutput & "<td class='small_bold_text' valign='top'>AC/BASE</td><td colspan='3' valign='top'>" & Short_Airport_Information(ac_id) & "</td>"

        '  htmlOutput = htmlOutput & "<td class='small_bold_text'>&nbsp;</td><td class='small_bold_text'>&nbsp;</td>" ' leave blank

        If Not IsDBNull(adoRSAircraft2("ac_maintained")) Then
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%' valign='top'>Maintained</td><td class='small_text' width='17%' valign='top'>: " & adoRSAircraft2("ac_maintained").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='13%' valign='top'>Maintained</td><td class='small_text' width='17%' valign='top'>: &nbsp;</font></td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_lease_flag")) Then
          If adoRSAircraft2("ac_lease_flag").ToString.ToUpper.Trim = "Y" Then
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Leased</td><td class='small_text' width='15%' valign='top'>: Yes</font></td>"
          Else
            htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Leased</td><td class='small_text' width='15%' valign='top'>: No</font></td>"
          End If
        Else
          htmlOutput = htmlOutput & "<td class='small_bold_text' width='10%' valign='top'>Leased</td><td class='small_text' width='15%' valign='top'>: No</font></td>"
        End If

        ' ------------------------------ THIS IS FOURTH ROW OF BLOCK ------------------------

      End If

      If Not IsDBNull(adoRSAircraft2("ac_confidential_notes")) Then
        If Not String.IsNullOrEmpty(adoRSAircraft2("ac_confidential_notes")) Then
          htmlOutput = htmlOutput & "<tr><td colspan='8'><font class='small_bold_text'>Note: &nbsp;"
          htmlOutput = htmlOutput & "</font><font class='small_text'>" & adoRSAircraft2("ac_confidential_notes").ToString & "&nbsp;</td></tr>"
          'htmlOutput = htmlOutput & Draw_Black_Line()
        End If
      End If

      htmlOutput = htmlOutput & "</table>"

      ViewToPDF = ViewToPDF & htmlOutput
      ViewToPDF = ViewToPDF & Draw_Black_Line()


      ViewToPDF = ViewToPDF & Short_Aircraft_Contacts(ac_id, "Normal")
      ViewToPDF = ViewToPDF & Draw_Black_Line()

      If Not IsDBNull(adoRSAircraft2("ac_exclusive_flag")) Then
        If adoRSAircraft2("ac_exclusive_flag") = "Y" Then
          temp_string = Short_Aircraft_Contacts(ac_id, "Broker")
          If Not String.IsNullOrEmpty(temp_string) Then
            ViewToPDF = ViewToPDF & temp_string
            ViewToPDF = ViewToPDF & Draw_Black_Line()
          End If
        End If
      End If

      ViewToPDF = ViewToPDF & "<table width='100%'><tr><td width='50%'>" ' This starts next section

      If Not IsDBNull(adoRSAircraft2("ac_times_as_of_date")) Then
        ViewToPDF = ViewToPDF & "<table width='50%'><tr><td valign='middle' align='left' width='100%'><font  class='small_bold_text'>Times Current:</font><font  class='small_text'>&nbsp;" & FormatDateTime(adoRSAircraft2("ac_times_as_of_date"), vbShortDate).ToString & "</td></tr></table>"
      End If
      '----------------------------------------- ENGINE INFORMATION--------------------------------------------------------------------
      ViewToPDF = ViewToPDF & "<table width='50%'><tr><td>"
            ViewToPDF = ViewToPDF & "<table><tr><td class='small_bold_text'> ENG TT</td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>SMOH/CORE</td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>SHOT/MPI </td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>TBO </td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>TCSN</td><td class='small_bold_text'>: </td></tr></table>"
      ViewToPDF = ViewToPDF & "</td>"
      For xLoop = 1 To nloopCount


        If (Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Or Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shs_cycles"))) Then

          ViewToPDF = ViewToPDF & "<td><table>"

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Then
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tot_hrs")), 0, True, False, True) & "</td></tr>"
          Else
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;</td></tr>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Then
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_hrs")), 0, True, False, True) & "</td></tr>"
          Else
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;</td></tr>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Then
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shi_hrs")), 0, True, False, True) & "</td></tr>"
          Else
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;</td></tr>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Then
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_tbo_hrs")), 0, True, False, True) & "</td></tr>"
          Else
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;</td></tr>"
          End If

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Then
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_snew_cycles")), 0, True, False, True) & "</td></tr>"
          Else
            ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left' class='small_text'>&nbsp;</td></tr>"
          End If

          'If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Then
          '  ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_soh_cycles")), 0, True, False, True) & "</td></tr>"
          'Else
          '  ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left'>&nbsp;</td></tr>"
          'End If

          'If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shs_cycles")) Then
          '  ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_shs_cycles")), 0, True, False, True) & "</td></tr>"
          'Else
          '  ViewToPDF = ViewToPDF & "<tr><td valign='middle' align='left'>&nbsp;</td></tr>"
          'End If

          ViewToPDF = ViewToPDF & "</table></td>" & vbCrLf

          If Not IsDBNull(adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_ser_no")) Then
            If engine_col_counter = 1 Then
              other_engine_info = other_engine_info & "<tr><td  width='25%' valign='middle' align='left' class='small_text'>&nbsp;" & adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_ser_no")
              engine_col_counter = engine_col_counter + 1
            Else
              other_engine_info = other_engine_info & "</td><td  width='25%' valign='middle' align='left' class='small_text'>&nbsp;" & adoRSAircraft2("ac_engine_" & CStr(xLoop) & "_ser_no") & "</td></tr>"
              engine_col_counter = 1
            End If
          Else
            If engine_col_counter = 2 Then
              other_engine_info = other_engine_info & "</td></tr>"
            End If
          End If

        End If

      Next ' xLoop
      '----------------------------------------- ENGINE INFORMATION--------------------------------------------------------------------

      ViewToPDF = ViewToPDF & "</table>"

      '  If engine_col_counter = 2 Then
      'ViewToPDF = ViewToPDF & "</td></tr></table>"
      ' End If

      ViewToPDF = ViewToPDF & "</td><td width='50%' valign='top'>"  ' this is for right side 

      ViewToPDF = ViewToPDF & "<table valign='top' width='100%'><tr><td valign='top' width='15%'>"

      ViewToPDF = ViewToPDF & "<table valign='top' width='100%'><tr><td class='small_bold_text' valign='top'>AIRFRM&nbsp;TT</td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>ENG&nbsp;MODEL</td><td class='small_bold_text'>: </td></tr><tr><td class='small_bold_text'>ENG&nbsp;SER#</td><td class='small_bold_text'>: </td></tr></table>"

      If Not IsDBNull(adoRSAircraft2("ac_airframe_tot_hrs")) Then
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<tr><td width='25%' valign='top' align='left' class='small_text'>&nbsp;" & adoRSAircraft2("ac_airframe_tot_hrs").ToString & "</td>"
      Else
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<tr><td width='25%' valign='top' align='left' class='small_text'></td>"
      End If

      If Not IsDBNull(adoRSAircraft2("ac_airframe_tot_landings")) Then
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<td valign='middle' align='left' width='35%'><font  class='small_bold_text'>Landings: </font><font class='small_text'>&nbsp;" & adoRSAircraft2("ac_airframe_tot_landings").ToString & "</td></tr>"
      Else
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<td valign='middle' align='left' class='small_text' width='35%'></td></tr>"
      End If

      If Not IsDBNull(adoRSAircraft2("ac_engine_name")) Then
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<tr><td valign='middle' align='left' class='small_text' colspan='2'>&nbsp;" & adoRSAircraft2("ac_engine_name").ToString & "</td></tr>"
      Else
        ac_airframe_tot_hrs = ac_airframe_tot_hrs & "<tr><td valign='middle' align='left' class='small_text'></td></tr>"
      End If

      ViewToPDF = ViewToPDF & "</td><td width='50%' valign='top'><table valign='top' width='100%'>" & ac_airframe_tot_hrs & other_engine_info & "</table>"

      ViewToPDF = ViewToPDF & "</td></tr></table>"

      ViewToPDF = ViewToPDF & "</td></tr></table>"

      '------------------------------------------------------ APU INFORMATION------------------------------------------------
      If Not IsDBNull(adoRSAircraft2("ac_apu_model_name")) Then
        ac_apu_model_name = adoRSAircraft2("ac_apu_model_name").ToString
      Else
        ac_apu_model_name = ""
      End If

      If Not IsDBNull(adoRSAircraft2("ac_apu_tot_hrs")) Then
        ac_apu_tot_hrs = adoRSAircraft2("ac_apu_tot_hrs").ToString
      Else
        ac_apu_tot_hrs = 0
      End If

      If Not IsDBNull(adoRSAircraft2("ac_apu_ser_no")) Then
        ac_apu_ser_no = adoRSAircraft2("ac_apu_ser_no").ToString
      Else
        ac_apu_ser_no = ""
      End If


      ViewToPDF = ViewToPDF & "<table width='100%'><tr><td width='50%'>"
      ViewToPDF = ViewToPDF & "<table width='100%'><tr><td colspan='5' class='small_bold_text'>Auxiliary Power Unit (APU)</td></tr>"


      If Not String.IsNullOrEmpty(ac_apu_model_name.ToString) Then
        ViewToPDF = ViewToPDF & "<tr><td width='50%'><font class='small_bold_text'>Model: </font><font class='small_text'>" & ac_apu_model_name
      End If

      If ac_apu_model_name.ToString = "" Then
        ViewToPDF = ViewToPDF & "<tr><td nowrap width='50%'>"
      Else
        ViewToPDF = ViewToPDF & "</td><td width='50%'>"
      End If

      If Not String.IsNullOrEmpty(ac_apu_ser_no) Then
        ViewToPDF = ViewToPDF & "<font class='small_bold_text'>Serial #:&nbsp;"
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>" & adoRSAircraft2("ac_apu_ser_no").ToString & "</td></tr>"
      Else
        ViewToPDF = ViewToPDF & "<font class='small_bold_text'>Serial #:&nbsp;</td></tr>"
      End If

      ViewToPDF = ViewToPDF & "<tr><td nowrap><font class='small_bold_text'>Total Time (Hours) Since New: "
      If Not String.IsNullOrEmpty(ac_apu_model_name.Trim) Then
        If ac_apu_tot_hrs > 0 Then
          ViewToPDF = ViewToPDF & "</font><font  class='small_text'>" & FormatNumber(CDbl(ac_apu_tot_hrs), 0, True, False, True) & "</td></tr>"
        Else
          ViewToPDF = ViewToPDF & "</font><font class='small_text'>&nbsp;</td></tr>"
        End If
      End If


      ViewToPDF = ViewToPDF & "<tr><td nowrap><font class='small_bold_text'>Since Overhaul (SOH) Hours:&nbsp;"
      If Not IsDBNull(adoRSAircraft2("ac_apu_soh_hrs")) Then
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>" & FormatNumber(CDbl(adoRSAircraft2("ac_apu_soh_hrs")), 0, True, False, True) & "&nbsp;</td></tr>"
      Else
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>&nbsp;</td></tr>"
      End If

      ViewToPDF = ViewToPDF & "</tr>" & vbCrLf


      ViewToPDF = ViewToPDF & "<tr><td nowrap><font class='small_bold_text'>Since Hot Inspection (SHI) Hours:&nbsp;"
      If Not IsDBNull(adoRSAircraft2("ac_apu_shi_hrs")) Then
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>" & FormatNumber(CDbl(adoRSAircraft2("ac_apu_shi_hrs")), 0, True, False, True) & "&nbsp;</td>"
        ViewToPDF = ViewToPDF & "</tr>" & vbCrLf
      Else
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>&nbsp;</td></tr>"

      End If

      ViewToPDF = ViewToPDF & "<tr><td width='50%'><font class='small_bold_text'>Engine OH:&nbsp;"
      If Not IsDBNull(adoRSAircraft2("ac_maint_eoh_by_name")) Then
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>" & adoRSAircraft2("ac_maint_eoh_by_name").ToString & "&nbsp;"
      End If

      ViewToPDF = ViewToPDF & "</td><td width='50%'><font class='small_bold_text'>Engine OH Date:&nbsp;"
      If Not IsDBNull(adoRSAircraft2("ac_main_eoh_moyear")) Then
        ViewToPDF = ViewToPDF & "</font><font class='small_text'>" & adoRSAircraft2("ac_main_eoh_moyear").ToString & "&nbsp;</td></tr>"
      Else
        ViewToPDF = ViewToPDF & "&nbsp;</td></tr>"
      End If





      ViewToPDF = ViewToPDF & "</table>"
      '  APU INFORMATION------------------------------------------------




      ViewToPDF = ViewToPDF & "</td><td width='50%' valign='top'>"
      ViewToPDF = ViewToPDF & Short_Aircraft_Features(ac_id)

      ViewToPDF = ViewToPDF & "</td></tr></table>"


      ViewToPDF = ViewToPDF & Draw_Black_Line()

      '---------------------------- PAGE BREAK--------------------------------
      ViewToPDF = ViewToPDF & Insert_Page_Break()
      ViewToPDF = ViewToPDF & htmlOutput
      ViewToPDF = ViewToPDF & Draw_Black_Line()
      '---------------------------- PAGE BREAK--------------------------------



      temp_string = Short_Aircraft_Details("Maintenance")




      temp_string = temp_string & temp_string2
      temp_string2 = ""
      If Not IsDBNull(adoRSAircraft2("ac_damage_history_notes")) Then
        temp_string = temp_string & "<tr><td></td><td><font class='small_bold_text'>Damage History: &nbsp;"
        temp_string = temp_string & " </font><font class='small_text'>" & adoRSAircraft2("ac_damage_history_notes").ToString & "&nbsp;</td></tr>"
      End If

      temp_string = temp_string & Short_Cert_Info()

      temp_string = temp_string & Short_Aircraft_Details("Interior")


      temp_string = temp_string & "<tr><td width='1%' valign='top' class='small_bold_text'></td><td valign='top'>"
      If Not IsDBNull(adoRSAircraft2("ac_interior_rating")) Then
        temp_string = temp_string & "<font class='small_bold_text'> Rating: </font><font class='small_text'>" & adoRSAircraft2("ac_interior_rating").ToString & "</font>"
      End If

      If Not IsDBNull(adoRSAircraft2("ac_interior_moyear")) Then
        'htmlOutput = htmlOutput & "<tr><td width='2%'>&nbsp;</td>"
        If Not String.IsNullOrEmpty(adoRSAircraft2("ac_interior_moyear").ToString) Then
          temp_moyear = adoRSAircraft2("ac_interior_moyear")
          If temp_moyear.ToString.Length = 5 Then
            temp_moyear = Left(temp_moyear, 1) & "/" & Right(temp_moyear, 4)
          Else
            If temp_moyear.ToString.Length = 6 Then
              temp_moyear = Left(temp_moyear, 2) & "/" & Right(temp_moyear, 4)
            End If
          End If
          temp_string = temp_string & "<font class='small_bold_text'> Updated: </font><font class='small_text'> " & temp_moyear & "</font>"
        Else
          temp_string = temp_string & "<font class='small_bold_text'> Updated: </font><font class='small_text'> " & adoRSAircraft2("ac_interior_moyear").ToString & "</font>"
        End If
      End If

      If Not IsDBNull(adoRSAircraft2("ac_interior_doneby_name")) Then
        temp_string = temp_string & "<font class='small_bold_text'> Done By: </font><font class='small_text'>" & adoRSAircraft2("ac_interior_doneby_name").ToString & "</font>"
      End If

      If Not IsDBNull(adoRSAircraft2("ac_passenger_count")) Then
        temp_string = temp_string & "<font class='small_bold_text'> Number of Passengers: </font><font class='small_text'>" & adoRSAircraft2("ac_passenger_count").ToString & "</font>"
      End If
      temp_string = temp_string & "</tr>"



      temp_string = temp_string & Short_Aircraft_Details("Exterior")

      temp_string = temp_string & "<tr><td width='1%'></td><td>"

      If Not IsDBNull(adoRSAircraft2("ac_exterior_rating")) Then
        temp_string = temp_string & "<font class='small_bold_text'>Rating: </font><font class='small_text'>" & adoRSAircraft2("ac_exterior_rating").ToString & " </font>"
      End If

      If Not String.IsNullOrEmpty(adoRSAircraft2("ac_exterior_moyear").ToString) Then
        If adoRSAircraft2("ac_exterior_moyear").ToString.Trim.Length > 4 Then
          temp_ex_moyear = adoRSAircraft2("ac_exterior_moyear")
          If temp_ex_moyear.ToString.Trim.Length = 5 Then
            temp_ex_moyear = Left(temp_ex_moyear, 1) & "/" & Right(temp_ex_moyear, 4)
          Else
            If temp_ex_moyear.ToString.Trim.Length = 6 Then
              temp_ex_moyear = Left(temp_ex_moyear, 2) & "/" & Right(temp_ex_moyear, 4)
            End If
          End If
          temp_string = temp_string & "<font class='small_bold_text'>Updated: </font><font class='small_text'>" & temp_ex_moyear & " </font>"
        Else
          temp_string = temp_string & "<font class='small_bold_text'>Updated: </font><font class='small_text'>" & adoRSAircraft2("ac_exterior_moyear").ToString & " </font>"
        End If
      End If

      If Not IsDBNull(adoRSAircraft2("ac_exterior_doneby_name")) Then
        temp_string = temp_string & "<font class='small_bold_text'>Done By: </font><font class='small_text'>" & adoRSAircraft2("ac_exterior_doneby_name").ToString & " </font>"
      End If

      temp_string = temp_string & "</td></tr>"

      temp_string = temp_string & Short_Aircraft_Details("Equipment")

      If Not String.IsNullOrEmpty(temp_string.ToString) Then
        temp_string = temp_string & Draw_Black_Line()
      End If


      temp_string2 = temp_string2 & Short_Aircraft_Avionics(ac_id)
      temp_string2 = temp_string2 & Short_Aircraft_Details("Addl Cockpit Equipment")

      If Not String.IsNullOrEmpty(temp_string2) Then
        temp_string2 = temp_string2 & Draw_Black_Line()
        temp_string = temp_string & temp_string2
        temp_string2 = ""
      End If

      'If Not IsDBNull(adoRSAircraft2("ac_confidential_notes")) Then
      '  If Not String.IsNullOrEmpty(adoRSAircraft2("ac_confidential_notes")) Then
      '    temp_string2 = temp_string2 & "<tr><td><font class='small_bold_text'>Confidential: &nbsp;"
      '    temp_string2 = temp_string2 & "</font><font class='small_text'>" & adoRSAircraft2("ac_confidential_notes").ToString & "&nbsp;</td></tr>"
      '    If temp_string2.Trim <> "" Then
      '      temp_string2 = temp_string2 & Draw_Black_Line()
      '    End If
      '  End If
      'End If
      ' temp_string = temp_string & temp_string2
      ' temp_string2 = ""

      If Not IsDBNull(adoRSAircraft2("ac_lease_flag")) Then
        If adoRSAircraft2("ac_lease_flag").ToString.ToString.ToUpper.Trim = "Y" Then
          temp_string2 = temp_string2 & Short_Lease_Info(ac_id)
          If temp_string2.Trim <> "" Then
            temp_string2 = temp_string2 & Draw_Black_Line()
          End If
        End If
      End If


      temp_string = temp_string & temp_string2
      If Not String.IsNullOrEmpty(temp_string) Then
        ViewToPDF = ViewToPDF & temp_string
      End If


      temp_string = Short_Aircraft_Contacts(ac_id, "Other")
      If Not String.IsNullOrEmpty(temp_string) Then
        '---------------------------- PAGE BREAK--------------------------------
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & htmlOutput
        ViewToPDF = ViewToPDF & Draw_Black_Line()
        '---------------------------- PAGE BREAK--------------------------------
        ViewToPDF = ViewToPDF & temp_string
        ViewToPDF = ViewToPDF & Draw_Black_Line()
      End If


      ' call the build header function
      ViewToPDF = Build_PDF_Template_Header() & ViewToPDF

      ' call the Build HTML Page
      ViewToPDF = Build_HTML_Page(ViewToPDF)
      ' call the Output String to HTML file

      Dim new_pdf_name As String



      If Not Me.WD.SelectedValue.ToString = "Word" Then

        If Not Build_String_To_HTML(ViewToPDF) Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "There was a problem generating your report"
        Else
          convert_to_pdf(report_name)
        End If


        new_pdf_name = Replace(report_name, "html", "pdf")
      Else
        Build_String_To_HTML(ViewToPDF)

        new_pdf_name = report_name
      End If


      ' Convert_To_Word(ViewToPDF)
      Response.Redirect(Session.Item("MarketSummaryFolderVirtualPath").ToString + "/" + new_pdf_name)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in btnRunReport_Click: " & ex.Message
    End Try
  End Sub
  Function Draw_Black_Line() As String
    Return "</td></tr></table><table width='100%' height='1'><tr><td width='100%' height='1px' bgcolor='black'></td></tr></table>" ' This is Line for Black Spacer----------
  End Function
  Function Short_Lease_Info(ByVal ac_id As Integer) As String
    Short_Lease_Info = ""
    Try
      ' THIS IS FOR AIRCRAFT QUERY
      Dim SqlException2 As System.Data.SqlClient.SqlException : SqlException2 = Nothing
      Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
      Dim adoRSAircraft2 As System.Data.SqlClient.SqlDataReader : adoRSAircraft2 = Nothing
      SqlConn2.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      'end Select
      SqlConn2.Open()

      SqlCommand2.Connection = SqlConn2
      SqlCommand2.CommandType = System.Data.CommandType.Text
      SqlCommand2.CommandTimeout = 60
      Dim Query2 As String : Query2 = ""



      '  Query2 = "SELECT * FROM Aircraft_Lease WHERE aclease_ac_id = '" & ac_id & "' and aclease_expired='N'"
      Query2 = "SELECT * FROM Aircraft_Lease WHERE aclease_ac_id = '" & ac_id & "' and aclease_expired='N'"

      SqlCommand2.CommandText = Query2
      adoRSAircraft2 = SqlCommand2.ExecuteReader()
      adoRSAircraft2.Read()

      If adoRSAircraft2.HasRows Then
        If Not IsDBNull(adoRSAircraft2("aclease_expired")) Then

        End If
        '    Short_Lease_Info = Short_Lease_Info & "LEASE TYPE - LEASE TERM - EXPIRES displayed as mm/dd/yyyy - NOTES"
        Short_Lease_Info = Short_Lease_Info & "<table valign='top'><tr><td valign='top' class='small_bold_text' colspan='10'>Lease Information</td></tr>"

        Short_Lease_Info = Short_Lease_Info & "<tr><td class='small_bold_text' width='5%'>Type:</td>"
        If Not IsDBNull(adoRSAircraft2("aclease_type")) Then
          If Not String.IsNullOrEmpty(adoRSAircraft2("aclease_type").ToString) Then
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'>" & adoRSAircraft2("aclease_type").ToString.Trim & "</td>"
          Else
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'></td></tr>"
          End If
        Else
          Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'></td>"
        End If

        Short_Lease_Info = Short_Lease_Info & "<td class='small_bold_text' width='5%'>Term:</td>"
        If Not IsDBNull(adoRSAircraft2("aclease_term")) Then
          If Not String.IsNullOrEmpty(adoRSAircraft2("aclease_term").ToString) Then
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'>" & adoRSAircraft2("aclease_term").ToString.Trim & "</td>"
          Else
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'></td></tr>"
          End If
        Else
          Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'></td>"
        End If

        Short_Lease_Info = Short_Lease_Info & "<td class='small_bold_text' width='10%'>Expiration Date:</td>"
        If Not IsDBNull(adoRSAircraft2("aclease_expiration_date")) Then
          If Not String.IsNullOrEmpty(adoRSAircraft2("aclease_expiration_date").ToString) Then
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='15%'>" & adoRSAircraft2("aclease_expiration_date") & "</td></tr>"
          Else
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='15%'></td></tr>"
          End If
        Else
          Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='15%'></td></tr>"
        End If

        Short_Lease_Info = Short_Lease_Info & "<tr><td class='small_bold_text' width='5%' colspan='10'>Notes: </td>"
        If Not IsDBNull(adoRSAircraft2("aclease_note")) Then
          If Not String.IsNullOrEmpty(adoRSAircraft2("aclease_note").ToString) Then
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'>" & adoRSAircraft2("aclease_note").ToString.Trim & "</td></tr>"
          Else
            Short_Lease_Info = Short_Lease_Info & "<td class='small_text' width='20%'></td></tr>"
          End If
        Else

          Short_Lease_Info = Short_Lease_Info & "</tr>"
        End If




      End If


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_CoverPage: " & ex.Message
    End Try
  End Function

  Function Short_Header(ByVal ac_id As Integer) As String
    Short_Header = ""
    Try
      ' THIS IS FOR AIRCRAFT QUERY
      Dim SqlException2 As System.Data.SqlClient.SqlException : SqlException2 = Nothing
      Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
      Dim adoRSAircraft2 As System.Data.SqlClient.SqlDataReader : adoRSAircraft2 = Nothing
      SqlConn2.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      'end Select
      SqlConn2.Open()

      SqlCommand2.Connection = SqlConn2
      SqlCommand2.CommandType = System.Data.CommandType.Text
      SqlCommand2.CommandTimeout = 60
      Dim Query2 As String : Query2 = ""
      '-----------------------------


      Dim htmlOutput As String = ""

      'SECTION 2  - AIRCRAFT INFO  -----------------------------------------------------------------------------------------------------------------------------------

      htmlOutput = htmlOutput & "<table>"
      Query2 = "SELECT * FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query2 = Query2 & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)
      ' Query = Query & GenerateProductCodeSelectionQuery(session("Product_Code"), session("UserTierLevel"), False, False, False)

      SqlCommand2.CommandText = Query2
      adoRSAircraft2 = SqlCommand2.ExecuteReader()
      adoRSAircraft2.Read()

      If adoRSAircraft2.HasRows Then

        ''''''''''''''''''''''''''''''''''''''''''''
        ' start the Aircraft Identification Status

        ''''''''''''''''''''''''''''''''''''''''''''

        ' ------------------------------ THIS IS FIRST ROW OF BLOCK ------------------------
        ' make
        If Not IsDBNull(adoRSAircraft2("amod_make_name")) Then
          htmlOutput = htmlOutput & "<tr><td NOWRAP><font class='small_header_text'>Make</font></td><td><font class='small_header_text'>: " & adoRSAircraft2("amod_make_name").ToString & "</td>"
        Else
          htmlOutput = htmlOutput & "<tr><td>&nbsp;</td><td>&nbsp;</td>"
        End If

        htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>" ' leave blank

        If Not IsDBNull(adoRSAircraft2("ac_asking_price")) Then
          htmlOutput = htmlOutput & "<td NOWRAP><font class='small_header_text'>Asking Amt (USD)</td><td></font><font class='text_text'>: " & FormatCurrency(adoRSAircraft2("ac_asking_price"), 0) & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_purchase_date")) Then
          htmlOutput = htmlOutput & "<td NOWRAP><font class='small_header_text'>Purchase Date</td><td></font><font class='text_text'>: " & FormatDateTime(adoRSAircraft2("ac_purchase_date"), vbShortDate) & "</font></td></tr>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td></tr>"
        End If
        ' ------------------------------ THIS IS FIRST ROW OF BLOCK ------------------------




        ' ------------------------------ THIS IS SECOND ROW OF BLOCK ------------------------
        If Not IsDBNull(adoRSAircraft2("amod_model_name")) Then
          htmlOutput = htmlOutput & "<tr><td NOWRAP><font class='small_header_text'>Model</td><td></font><font class='text_text'>: " & adoRSAircraft2("amod_model_name").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_year")) Then
          htmlOutput = htmlOutput & "<td NOWRAP><font class='small_header_text'>YR DLV</td><td><font class='small_header_text'>: " & adoRSAircraft2("ac_year").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        If Not IsDBNull(adoRSAircraft2("ac_asking")) Or Not IsDBNull(adoRSAircraft2("ac_status")) Then
          htmlOutput = htmlOutput & "<td><font class='small_header_text'>Status</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_status").ToString & adoRSAircraft2("ac_asking").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td></tr>" ' Date Edited

        ' ------------------------------ THIS IS SECOND ROW OF BLOCK ------------------------



        ' ------------------------------ THIS IS THIRD ROW OF BLOCK ------------------------
        ' serial number
        If Not IsDBNull(adoRSAircraft2("ac_ser_no_full")) Then
          htmlOutput = htmlOutput & "<tr><td NOWRAP><font class='small_header_text'>Serial #</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_ser_no_full").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<tr><td>&nbsp;</td>"
        End If
        ' year of manufacture
        If Not IsDBNull(adoRSAircraft2("ac_mfr_year")) Then
          htmlOutput = htmlOutput & "<td NOWRAP><font class='small_header_text'>YR MFT</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_mfr_year").ToString & "</font></td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>" ' FOR SALE (Y/N)

        If Not IsDBNull(adoRSAircraft2("ac_list_date")) Then
          htmlOutput = htmlOutput & "<td><font class='small_header_text'>Date Listed</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_list_date").ToString & "</font></td></tr>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td></tr>"
        End If

        ' ------------------------------ THIS IS THIRD ROW OF BLOCK ------------------------




        ' ------------------------------ THIS IS FOURTH ROW OF BLOCK ------------------------
        If Not IsDBNull(adoRSAircraft2("ac_reg_no")) Then
          htmlOutput = htmlOutput & "<tr><td NOWRAP><font class='small_header_text'>Registration #</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_reg_no").ToString & "</td>"
        Else
          htmlOutput = htmlOutput & "<tr><td>&nbsp;</td><td>&nbsp;</td>"
        End If

        htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>" ' leave blank

        If Not IsDBNull(adoRSAircraft2("ac_maintained")) Then
          htmlOutput = htmlOutput & "<td><font class='small_header_text'>Maintained</td><td></font><font class='text_text'>: " & adoRSAircraft2("ac_maintained").ToString & "</font>" & "</td>"
        Else
          htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td>"
        End If

        htmlOutput = htmlOutput & "<td>&nbsp;</td><td>&nbsp;</td></tr>" ' follow up
        htmlOutput = htmlOutput & "</table>"
        ' ------------------------------ THIS IS FOURTH ROW OF BLOCK ------------------------

      End If

      Short_Header = Short_Header & htmlOutput
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_CoverPage: " & ex.Message
    End Try
  End Function



  Public Function Short_Phone_Company_Contact_Info(ByVal comp_id As Integer, ByVal contact_id As Integer) As String
    Short_Phone_Company_Contact_Info = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim num_phones As Integer = 1
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try

      '------------------------------------------------------------- PHONE INFO-----------------
      'Query = "SELECT distinct pnum_type, pnum_number_full FROM  Phone_Numbers"
      'Query = Query & " INNER JOIN Company ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id"
      'Query = Query & " Inner Join Contact On Contact.contact_id = Phone_Numbers.pnum_contact_id"
      'Query = Query & " WHERE Company.comp_id = " & comp_id & " and Contact.contact_id = " & contact_id & " AND pnum_journ_id = 0 AND pnum_hide_customer = 'N'"

      Query = "select distinct top 2 pnum_type, pnum_number_full from Phone_Numbers where pnum_contact_id = " & contact_id & " and pnum_comp_id = " & comp_id & "AND pnum_journ_id = 0AND pnum_hide_customer = 'N'"
      Query = Query & " ORDER BY pnum_type desc"



      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      ' adoRSAircraft.Read()


      If adoRSAircraft.HasRows Then
        Do While adoRSAircraft.Read
          If Me.WD.SelectedValue = "Word" Then
            Short_Phone_Company_Contact_Info = Short_Phone_Company_Contact_Info & "<tr><td><font  class='small_text'>" & adoRSAircraft("pnum_type").ToString & ":</font><font class='small_text'> " & adoRSAircraft("pnum_number_full").ToString & "</td></tr>"
          Else

            If num_phones = 1 Then
              Short_Phone_Company_Contact_Info = Short_Phone_Company_Contact_Info & "<tr><td><font  class='small_text'>" & adoRSAircraft("pnum_type").ToString & ":</font><font class='small_text'> " & adoRSAircraft("pnum_number_full").ToString & "</td>"
              num_phones = num_phones + 1
            Else
              Short_Phone_Company_Contact_Info = Short_Phone_Company_Contact_Info & "<td><font  class='small_text'>" & adoRSAircraft("pnum_type").ToString & ":</font><font class='small_text'> " & adoRSAircraft("pnum_number_full").ToString & "</td></tr>"
              num_phones = 1
            End If
          End If
        Loop
        If num_phones = 2 Then
          Short_Phone_Company_Contact_Info = Short_Phone_Company_Contact_Info & "</tr>"
        End If

      End If


      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Phone Info: " & ex.Message
    End Try
  End Function
  Public Function Phone_Info(ByVal Sub_ID As String, ByVal color As String) As String
    Phone_Info = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try

      '------------------------------------------------------------- PHONE INFO-----------------
      Query = "SELECT pnum_type, pnum_number_full FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 INNER JOIN Phone_Numbers"
      ' Query = Query & " WITH(NOLOCK), Phone_Type WITH(NOLOCK)"
      Query = Query & " ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id"
      Query = Query & " WHERE Subscription.sub_id = " & Sub_ID
      Query = Query & " AND pnum_journ_id = 0"
      Query = Query & " AND pnum_hide_customer = 'N' AND pnum_contact_id = 0"
      Query = Query & " ORDER BY pnum_type desc"  ' QUERY EDITED TO DISPLAY TOLL FREE THEN OFFICE THEN FAX
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      ' adoRSAircraft.Read()


      If adoRSAircraft.HasRows Then

        Do While adoRSAircraft.Read
          If adoRSAircraft("pnum_type") = "Office" Then
            If color = "white" Then
              Phone_Info = Phone_Info & "<tr><td><font color='white' size='+1'>" & adoRSAircraft("pnum_type").ToString & ": </font><font color='white' size='+1'>" & adoRSAircraft("pnum_number_full").ToString & "</font>"
            Else
              Phone_Info = Phone_Info & "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>" & adoRSAircraft("pnum_type").ToString & ": </font><font class='text_text'>" & adoRSAircraft("pnum_number_full").ToString & "</font></li></td></tr>"
            End If

          End If
        Loop
      End If


      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Phone Info: " & ex.Message
    End Try
  End Function
  Public Function Short_Cert_Info() As String
    Short_Cert_Info = ""
    Dim htmlOutput As String = ""
    Dim last_detail_type As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim first_count As Integer = 1
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try
      ' Start Interior Details    -----------------------------------------------------------------
      ' Start Interior Details    
      Query = "select * from aircraft_certified where(accert_ac_journ_id = 0) and accert_ac_id= " & ac_id


      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      'adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        Short_Cert_Info = Short_Cert_Info & "<tr><td></td><td><font class='small_bold_text'>Certifications: </font><font class='small_text'>"
        Do While adoRSAircraft.Read
          If first_count = 2 Then
            Short_Cert_Info = Short_Cert_Info & ",&nbsp;"
          Else
            first_count = 2
          End If
          Short_Cert_Info = Short_Cert_Info & adoRSAircraft("accert_name").ToString
        Loop
        Short_Cert_Info = Short_Cert_Info & "</td></tr>"
      End If


      adoRSAircraft = Nothing
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_CoverPage: " & ex.Message
    End Try
  End Function

  Public Function Short_Aircraft_Details(ByVal detail_type As String) As String
    Short_Aircraft_Details = ""
    Dim htmlOutput As String = ""
    Dim last_detail_type As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim first_count As Integer = 1
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try
      ' Start Interior Details    -----------------------------------------------------------------
      ' Start Interior Details    
      Query = "SELECT * FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " & ac_id
      Query = Query & " AND adet_journ_id = '0' AND adet_data_type = '" & detail_type & "'"


      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      'adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then

        Short_Aircraft_Details = Short_Aircraft_Details & "<tr><td><table><tr><td width='99%' class='small_bold_text' colspan='2'>" & detail_type & " Details</font>&nbsp;</td></tr>"


        Do While adoRSAircraft.Read
          If Not IsDBNull(adoRSAircraft("adet_data_name")) Then
            If last_detail_type = adoRSAircraft("adet_data_name").ToString Then
              Short_Aircraft_Details = Short_Aircraft_Details & "<font class='small_text'>; " & adoRSAircraft("adet_data_description").ToString & "</font>"
            Else
              If first_count = 2 Then
                Short_Aircraft_Details = Short_Aircraft_Details & "</td></tr>"
              Else
                first_count = 2
              End If
              Short_Aircraft_Details = Short_Aircraft_Details & "<tr><td width='1%'></td><td><font class='small_bold_text'>" & adoRSAircraft("adet_data_name").ToString & "</font><font class='small_text'>: " & adoRSAircraft("adet_data_description").ToString & "</font>"
            End If
          Else
            Short_Aircraft_Details = Short_Aircraft_Details & "<tr><td width='1%'></td><td><font class='small_bold_text'>" & adoRSAircraft("adet_data_name").ToString & "</font><font class='small_text'>: " & adoRSAircraft("adet_data_description").ToString & "</font></td></tr>"
          End If
          last_detail_type = adoRSAircraft("adet_data_name").ToString

        Loop
        Short_Aircraft_Details = Short_Aircraft_Details & "</td></tr></table></td></tr>"
        adoRSAircraft.Close()
      End If
      adoRSAircraft = Nothing


      adoRSAircraft = Nothing
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_CoverPage: " & ex.Message
    End Try
  End Function

  Public Function Short_Aircraft_Avionics(ByVal ac_id As Integer) As String
    Short_Aircraft_Avionics = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim av_count As Integer = 1
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try
      ' Start Avionics    
      Query = "SELECT * FROM Aircraft_Avionics WITH(NOLOCK) WHERE av_ac_id = " & ac_id & " AND av_ac_journ_id = '0'" '  AND av_name  IN ('Avioncs Package', 'FMS', 'GPS', 'TAWS', 'TCAS', 'SATCOM', 'EFIS', 'CVR', 'FDR')"
      'response.write(Query)
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      'adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        Short_Aircraft_Avionics = Short_Aircraft_Avionics & "<table width='99%'><tr><td class='small_bold_text' colspan='2'>Avionics</td></tr>"

        Do While adoRSAircraft.Read
          If av_count = 1 Then
            Short_Aircraft_Avionics = Short_Aircraft_Avionics & "<tr><td width='1%'></td>"
          End If
          Short_Aircraft_Avionics = Short_Aircraft_Avionics & "<td width='49%'><font class='small_bold_text'>" & adoRSAircraft("av_name").ToString & ": </font><font class='small_text'>" & adoRSAircraft("av_description").ToString & "; </font></td>"
          av_count = av_count + 1
          If av_count = 3 Then
            Short_Aircraft_Avionics = Short_Aircraft_Avionics & "</tr>"
            av_count = 1
          End If
        Loop
        Short_Aircraft_Avionics = Short_Aircraft_Avionics & "</table>"
      End If
      adoRSAircraft = Nothing

      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Avionics: " & ex.Message
    End Try
  End Function
  Public Function Short_Airport_Information(ByVal ac_id As Integer) As String
    Short_Airport_Information = ""
    Try
      Dim htmlOutput As String = ""
      Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
      Dim SqlConn As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand As New System.Data.SqlClient.SqlCommand
      Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
      Dim Query As String : Query = ""


      SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60


      Query = "SELECT ac_aport_iata_code, ac_aport_icao_code, ac_aport_name, ac_aport_city, ac_aport_state, ac_aport_country FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)
      'response.write(Query)
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        Short_Airport_Information = Short_Airport_Information & "<table align='left' width='100%'><tr><td valign='top' align='left' width='100%' class='small_text'>: "

        If Not IsDBNull(adoRSAircraft("ac_aport_iata_code")) Then
          Short_Airport_Information = Short_Airport_Information & adoRSAircraft("ac_aport_iata_code").ToString.Trim
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_icao_code")) Then
          Short_Airport_Information = Short_Airport_Information & " - " & adoRSAircraft("ac_aport_icao_code").ToString.Trim
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_name")) Then
          Short_Airport_Information = Short_Airport_Information & " - " & adoRSAircraft("ac_aport_name").ToString.Trim
        End If


        If Not IsDBNull(adoRSAircraft("ac_aport_city")) Then
          Short_Airport_Information = Short_Airport_Information & "<br>&nbsp;&nbsp;" & adoRSAircraft("ac_aport_city").ToString.Trim & vbCrLf
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_state")) Then
          Short_Airport_Information = Short_Airport_Information & " - " & adoRSAircraft("ac_aport_state").ToString.Trim & vbCrLf
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_country")) Then
          Short_Airport_Information = Short_Airport_Information & " - " & adoRSAircraft("ac_aport_country").ToString.Trim & vbCrLf
        End If
        Short_Airport_Information = Short_Airport_Information & "</td></tr></table>"




      End If
      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Airport_Information: " & ex.Message
    End Try
  End Function
  Public Function Set_up_function(ByVal ac_id As Integer) As String
    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Set_up_function = ""

    ' This is right side column, should start with an open table already in td

    Try
      Dim htmlOutput As String = ""
      Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
      Dim SqlConn As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand As New System.Data.SqlClient.SqlCommand
      Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
      SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      'end Select
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      Dim Query As String : Query = ""


      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then

      End If
      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in SeupFnction: " & ex.Message
    End Try
  End Function

  Public Function Short_Aircraft_Contacts(ByVal ac_id As Integer, ByVal type As String) As String
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim contact_counter As Integer = 1
    Dim column_color As String = "white"
    Dim temp_phone As String = ""
    Dim email_web_count As Integer = 1
    Short_Aircraft_Contacts = ""
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try

      Query = "select comp_id, actype_name, cref_owner_percent, comp_name, comp_name_alt, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country,"
      Query = Query & " comp_email_address, comp_web_address, contact_id, contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title,"
      Query = Query & " contact_email_address from aircraft_reference inner join company on cref_comp_id = comp_id and cref_journ_id = comp_journ_id inner join aircraft_contact_type on cref_contact_type = actype_code"
      Query = Query & " left outer join contact on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_hide_flag='N'"
      If type = "Normal" Then
        Query = Query & " where(cref_ac_id = '" & ac_id & "') and cref_journ_id = 0 and cref_contact_type NOT IN('71', '99', '93') and cref_transmit_seq_no <> 99 order by cref_transmit_seq_no"
      ElseIf type = "Broker" Then
        Query = Query & " where(cref_ac_id = '" & ac_id & "') and cref_journ_id = 0 and cref_contact_type IN('99','93') order by cref_transmit_seq_no"
      ElseIf type = "Other" Then
        Query = Query & " where(cref_ac_id = '" & ac_id & "') and cref_journ_id = 0 and cref_contact_type NOT IN('71', '99', '93') and cref_transmit_seq_no = 99 order by cref_transmit_seq_no"
      End If
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()



      If adoRSAircraft.HasRows Then
        'Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td>&nbsp;</td></tr>"
        Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<table width='100%' valign='top'>"
        If type = "Normal" Then
          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td class='small_bold_text' colspan='2'>Primary Contacts</td></tr>"
        ElseIf type = "Broker" Then
          Short_Aircraft_Contacts = Short_Aircraft_Contacts & ""
        ElseIf type = "Other" Then
          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td class='small_bold_text' colspan='2'>Additional Contacts</td></tr>"
        End If

        Do While adoRSAircraft.Read



          If contact_counter = 1 Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr>"
          End If

          'If (column_color = "white") Then
          '  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr valign='top'><td width='60%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          'Else
          '  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr valign='top'><td width='60%' bgcolor='#E6E6E6' class='text_text' cellspacing='0' cellpadding='5'><b>"
          'End If


          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<td valign='top' width='50%'><table valign='top' width='100%'><tr><td valign='top' align='left' class='small_bold_text' width='50%'>"

          If Not IsDBNull(adoRSAircraft("actype_name")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("actype_name")
          End If


          If Not IsDBNull(adoRSAircraft("cref_owner_percent")) Then
            If adoRSAircraft("cref_owner_percent") > 0 And adoRSAircraft("cref_owner_percent") < 100 Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & " [" & adoRSAircraft("cref_owner_percent").ToString & "%] </td></tr><tr><td class='small_text'>"
            Else
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</td></tr><tr><td class='small_text'>"
            End If
          Else
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</td></tr><tr><td class='small_text'>"
          End If

          If Not IsDBNull(adoRSAircraft("comp_name")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_name").ToString & "</td></tr><tr><td class='small_text'>"
          End If



          If Not IsDBNull(adoRSAircraft("comp_name_alt")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_name_alt").ToString & "</td></tr><tr><td class='small_text'>"
          Else
            ' Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</td></tr><tr><td class='small_text'>"
          End If

          '------------------------ PRIM CONTACT----------------
          If Not IsDBNull(adoRSAircraft("contact_sirname")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_sirname").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_first_name")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_first_name").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_middle_initial")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_middle_initial").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_last_name")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_last_name").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_suffix")) Then
            If Not String.IsNullOrEmpty(adoRSAircraft("contact_suffix").ToString) Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_suffix").ToString & "</td></tr><tr><td class='small_text'>"
            End If
          End If

          If Not IsDBNull(adoRSAircraft("contact_title")) Then
            If Not String.IsNullOrEmpty(adoRSAircraft("contact_title")) Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("contact_title").ToString & "</td></tr><tr><td class='small_text'>"
            End If
          End If

          '------------------------ PRIM CONTACT----------------




          If Not IsDBNull(adoRSAircraft("comp_address1")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_address1").ToString & "</td></tr><tr><td class='small_text'>"
          End If

          'If Not IsDBNull(adoRSAircraft("comp_address2")) Then
          '  Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_address2") & "<br>"
          ' End If

          If Not IsDBNull(adoRSAircraft("comp_city")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_city").ToString
          End If

          If Not IsDBNull(adoRSAircraft("comp_state")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & ", " & adoRSAircraft("comp_state").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("comp_zip_code")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_zip_code").ToString & " "
          End If

          If Not IsDBNull(adoRSAircraft("comp_country")) Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & adoRSAircraft("comp_country").ToString & "</td></tr>"
          Else
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</td></tr>"
          End If

          If Me.WD.SelectedValue = "Word" Then
            '-------------------------------------------------------------------------------------------------------------------------------------------------
            If Not IsDBNull(adoRSAircraft("comp_web_address")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft("comp_web_address").ToString) Then
                Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td class='small_text' width='50%'><u>" & adoRSAircraft("comp_web_address").ToString & "</u>" & "</td></tr>"
              End If
            End If
            If Not IsDBNull(adoRSAircraft("contact_email_address")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft("contact_email_address").ToString.ToString) Then
                Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td class='small_text' width='50%'><u>" & adoRSAircraft("contact_email_address").ToString & "</u>" & "</td></tr>"
              End If
            End If
            '-------------------------------------------------------------------------------------------------------------------------------------------------
          Else
            '-------------------------------------------------------------------------------------------------------------------------------------------------
            If Not IsDBNull(adoRSAircraft("comp_web_address")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft("comp_web_address").ToString) Then
                Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td class='small_text' width='50%'><u>" & adoRSAircraft("comp_web_address").ToString & "</u>" & "</td>"
                email_web_count = 2
              End If
            End If


            If Not IsDBNull(adoRSAircraft("contact_email_address")) Then
              If Not String.IsNullOrEmpty(adoRSAircraft("contact_email_address").ToString.ToString) Then
                If email_web_count = 1 Then
                  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr>"
                End If
                Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<td class='small_text' width='50%'><u>" & adoRSAircraft("contact_email_address").ToString & "</u>" & "</td>"
                email_web_count = 2
              Else
                If Not IsDBNull(adoRSAircraft("comp_email_address")) Then
                  If Not String.IsNullOrEmpty(adoRSAircraft("comp_email_address").ToString) Then
                    If email_web_count = 1 Then
                      Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr>"
                    End If
                    Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<td class='small_text' width='50%'><u>" & adoRSAircraft("comp_email_address").ToString & "</u>" & "</td>"
                    email_web_count = 2
                  End If
                End If
              End If
            Else
              If Not IsDBNull(adoRSAircraft("comp_email_address")) Then
                If Not String.IsNullOrEmpty(adoRSAircraft("comp_email_address").ToString) Then
                  If email_web_count = 1 Then
                    Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr>"
                  End If
                  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<td class='small_text' width='50%'><u>" & adoRSAircraft("comp_email_address").ToString & "</u>" & "</td>"
                  email_web_count = 2
                End If
              End If
            End If

            If email_web_count = 2 Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</tr>"
            End If
            '-------------------------------------------------------------------------------------------------------------------------------------------------
          End If



          If Not IsDBNull(adoRSAircraft("comp_id")) And Not IsDBNull(adoRSAircraft("contact_id")) Then
            temp_phone = Short_Phone_Company_Contact_Info(adoRSAircraft("comp_id"), adoRSAircraft("contact_id"))
            If temp_phone = "" Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & Short_Phone_Company_Contact_Info(adoRSAircraft("comp_id"), 0)
            Else
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & temp_phone
            End If

          Else
            If Not IsDBNull(adoRSAircraft("comp_id")) Then
              Short_Aircraft_Contacts = Short_Aircraft_Contacts & Short_Phone_Company_Contact_Info(adoRSAircraft("comp_id"), 0)
            End If
          End If



          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<tr><td height='2'></td></tr>"

          'If (column_color = "white") Then
          '  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "&nbsp;</td><td width='40%' bgcolor='#E6E6E6' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          '  column_color = "other"
          'Else
          '  Short_Aircraft_Contacts = Short_Aircraft_Contacts & "&nbsp;</td><td width='40%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
          '  column_color = "white"
          'End If






          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</table></td>"


          If contact_counter = 2 Then
            Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</tr>"
            contact_counter = 1
          Else
            contact_counter = contact_counter + 1
          End If


          email_web_count = 1
        Loop

        If contact_counter = 1 Then
          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "</table>"
        Else
          Short_Aircraft_Contacts = Short_Aircraft_Contacts & "<td width='100%'>&nbsp;</td></tr></table>"
        End If


      End If
      adoRSAircraft = Nothing



      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Avionics: " & ex.Message
    End Try
  End Function

  Public Function Build_PDF_Header(ByVal Title As String, ByVal address_info As String) As String
    Build_PDF_Header = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim company_name As String = ""
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try




      'htmlOutput = htmlOutput & "<table valign='top'>"


      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

      'Query = "SELECT DISTINCT * FROM Company WITH(NOLOCK) WHERE (comp_journ_id =  0 AND  = " & Session("SubID") & " AND comp_hide_flag = 'N')"
      Query = "SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 WHERE(Subscription.sub_id = " & Session.Item("localSubscription").evoSubID.ToString.Trim & ")"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        company_name = adoRSAircraft("comp_name")
      End If
      adoRSAircraft.Close()

      Build_PDF_Header = Build_PDF_Header & "<table cellspacing='0' cellpadding='0' width='800'><tr bgcolor='#736F6E'><td colspan='3'  cellpadding='0' cellspacing='0'>"
      Build_PDF_Header = Build_PDF_Header & "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr><td>"
      Build_PDF_Header = Build_PDF_Header & "<table><tr><td width='650'><font color='white' size='+1'>"
      Build_PDF_Header = Build_PDF_Header & company_name & "<br><table>" & address_info
      Build_PDF_Header = Build_PDF_Header & "</table></font>"

      ' Build_PDF_Header = Build_PDF_Header & "<table align='center' valign='bottom'><tr><td align='center'>&nbsp;</td></tr></table>"
      Build_PDF_Header = Build_PDF_Header & "</td><td width='40%' cellpadding='5'><font color='white'>"




      Query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = '0' and ac_id = " & ac_id
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

      End If

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        Build_PDF_Header = Build_PDF_Header & adoRSAircraft.Item("amod_make_name").ToString & " "
      End If

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then

        Build_PDF_Header = Build_PDF_Header & adoRSAircraft.Item("amod_model_name").ToString
      End If

      If Not Me.BR.Checked Then
        If Not IsDBNull(adoRSAircraft("ac_ser_no_full")) Then
          Build_PDF_Header = Build_PDF_Header & " SN # " & adoRSAircraft.Item("ac_ser_no_full").ToString & "</font>"
        End If
      End If
      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      'serial number

      Build_PDF_Header = Build_PDF_Header & "</font>"

      Build_PDF_Header = Build_PDF_Header & "<table align='left' valign='bottom'>"
      If address_info <> "" Then
        Build_PDF_Header = Build_PDF_Header & "<tr><td align='center'>&nbsp;</td></tr>"
      End If
      Build_PDF_Header = Build_PDF_Header & "<tr><td align='center'><font color='white'><i>" & Title & "</i></font></td></tr></table>"
      Build_PDF_Header = Build_PDF_Header & "</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='60%' height='950' valign='top'  cellpadding='0' cellspacing='0'><table valign='top'  cellpadding='0' cellspacing='0'>"

      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Header: " & ex.Message
    End Try
  End Function
  Public Function Build_PDF_Header2(ByVal Title As String) As String
    Build_PDF_Header2 = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim company_name As String = ""
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try




      'htmlOutput = htmlOutput & "<table valign='top'>"


      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

      'Query = "SELECT DISTINCT * FROM Company WITH(NOLOCK) WHERE (comp_journ_id =  0 AND  = " & Session("SubID") & " AND comp_hide_flag = 'N')"
      Query = "SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 WHERE(Subscription.sub_id = " & Session.Item("localSubscription").evoSubID.ToString.Trim & ")"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        company_name = adoRSAircraft("comp_name")
      End If
      adoRSAircraft.Close()

      Build_PDF_Header2 = Build_PDF_Header2 & "<table cellspacing='0' cellpadding='0' width='800'><tr bgcolor='#736F6E'><td colspan='3' valign='top'>"
      Build_PDF_Header2 = Build_PDF_Header2 & "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr valign='top'><td valign='top'>"
      Build_PDF_Header2 = Build_PDF_Header2 & "<table valign='top'><tr valign='top'><td width='650' valign='top'><font color='white' size='+1'>"
      Build_PDF_Header2 = Build_PDF_Header2 & company_name
      Build_PDF_Header2 = Build_PDF_Header2 & "</font></td><td width='40%' cellpadding='5'><font color='white'>"




      Query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = '0' and ac_id = " & ac_id
      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      adoRSAircraft.Read()


      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        Build_PDF_Header2 = Build_PDF_Header2 & adoRSAircraft("amod_make_name") & " "
      End If

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        Build_PDF_Header2 = Build_PDF_Header2 & adoRSAircraft("amod_model_name")
      End If

      If Not Me.BR.Checked Then
        If Not IsDBNull(adoRSAircraft("ac_ser_no_full")) Then
          Build_PDF_Header2 = Build_PDF_Header2 & " SN # " & adoRSAircraft("ac_ser_no_full") & "</font>"
        End If
      Else
        Build_PDF_Header2 = Build_PDF_Header2 & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
      End If

      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      'serial number
      Build_PDF_Header2 = Build_PDF_Header2 & "<table align='left' valign='bottom'><tr><td align='center'><font color='white'><i>" & Title & "</i></font></td></tr></table>"
      Build_PDF_Header2 = Build_PDF_Header2 & "</font></td></tr></table></td></tr></table></td></tr><tr><td width='100%'  cellspacing='0'><table width='100%' cellspacing='0' cellpadding='5'>"

      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Header2: " & ex.Message
    End Try
  End Function
  Public Function Build_PDF_Format() As String
    Build_PDF_Format = ""
    Try
      Build_PDF_Format = Build_PDF_Format & "</table>"
      Build_PDF_Format = Build_PDF_Format & "</td><td>&nbsp;&nbsp;&nbsp;"
      Build_PDF_Format = Build_PDF_Format & "</td><td width='40%' height='950' valign='top'>"
      Build_PDF_Format = Build_PDF_Format & "<table bgcolor='#A4A4A4' height='950' width='100%' valign='top'><tr height='950' valign='top'><td width='100%' height='950' align='center'>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Format: " & ex.Message
    End Try
  End Function
  Public Function Short_Aircraft_Features(ByVal ac_id As Integer) As String
    Short_Aircraft_Features = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim type_of_damage As String = ""
    Dim feat_count As Integer = 1
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60
    Dim Query As String : Query = ""
    Try

      Query = "SELECT Aircraft_Key_Feature.afeat_status_flag, Key_Feature.kfeat_code FROM Aircraft_Key_Feature INNER JOIN Key_Feature ON Aircraft_Key_Feature.afeat_feature_code = Key_Feature.kfeat_code "
      Query = Query & "WHERE (Aircraft_Key_Feature.afeat_ac_id = " + ac_id.ToString + ") AND (Aircraft_Key_Feature.afeat_journ_id = '0') ORDER BY Aircraft_Key_Feature.afeat_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then
        Short_Aircraft_Features = Short_Aircraft_Features & "<table valign='top'><tr><td valign='top' colspan='20' class='small_bold_text'>Key Features</td></tr>"
        Do While adoRSAircraft.Read
          '  Aircraft_Build_PDF_Features = Aircraft_Build_PDF_Features & "<tr valign='top'><td class='white_feat_text' valign='top'>"
          If feat_count = 1 Then
            Short_Aircraft_Features = Short_Aircraft_Features & "<tr valign='top'>"
          End If
          Short_Aircraft_Features = Short_Aircraft_Features & "<td width='10%' valign='top' class='small_bold_text'>"
          If adoRSAircraft("afeat_status_flag").ToString = "Y" Then
            Short_Aircraft_Features = Short_Aircraft_Features & adoRSAircraft("kfeat_code") & "</td><td class='small_bold_text' width='5%'>: </td><td class='small_text' width='10%'>"
            Short_Aircraft_Features = Short_Aircraft_Features & " Y " & "</td>"
          ElseIf adoRSAircraft("afeat_status_flag").ToString = "N" Then
            Short_Aircraft_Features = Short_Aircraft_Features & adoRSAircraft("kfeat_code") & "</td><td class='small_bold_text' width='5%'>: </td><td class='small_text' width='10%'>"
            Short_Aircraft_Features = Short_Aircraft_Features & " N " & "</td>"
          ElseIf adoRSAircraft("afeat_status_flag").ToString = "U" Then
            Short_Aircraft_Features = Short_Aircraft_Features & adoRSAircraft("kfeat_code") & "</td><td class='small_bold_text' width='5%'>: </td><td class='small_text' width='10%'>"
            Short_Aircraft_Features = Short_Aircraft_Features & " U " & "</td>"
          ElseIf adoRSAircraft("afeat_status_flag").ToString = "I" Then
            Short_Aircraft_Features = Short_Aircraft_Features & adoRSAircraft("kfeat_code") & "</td><td class='small_bold_text' width='5%'>: </td><td class='small_text' width='10%'>"
            Short_Aircraft_Features = Short_Aircraft_Features & " I " & "</td>"
          ElseIf adoRSAircraft("afeat_status_flag").ToString = "A" Then
            Short_Aircraft_Features = Short_Aircraft_Features & adoRSAircraft("kfeat_code") & "</td><td class='small_bold_text' width='5%'>: </td><td class='small_text' width='10%'>"
            Short_Aircraft_Features = Short_Aircraft_Features & " A " & "</td>"
          Else
            type_of_damage = ""
          End If

          feat_count = feat_count + 1
          If feat_count = 5 Then
            Short_Aircraft_Features = Short_Aircraft_Features & "</tr>"
            feat_count = 1
          End If

        Loop
      End If
      Short_Aircraft_Features = Short_Aircraft_Features & "</table>"


      adoRSAircraft.Close()
      adoRSAircraft = Nothing
      'serial number
      SqlConn.Close()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Features: " & ex.Message
    End Try
  End Function
  Function DisplayEngineInfo(ByVal ac_id As Integer) As String
    DisplayEngineInfo = ""
    Dim xLoop, nloopCount
    Dim sAircraftType As String = ""
    Dim sAirframeType As String = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim SqlException2 As System.Data.SqlClient.SqlException : SqlException2 = Nothing
    Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft2 As System.Data.SqlClient.SqlDataReader : adoRSAircraft2 = Nothing
    Dim type_of_damage As String = ""
    SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    SqlConn2.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    'end Select
    SqlConn.Open()
    SqlConn2.Open()


    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = System.Data.CommandType.Text
    SqlCommand.CommandTimeout = 60

    SqlCommand2.Connection = SqlConn2
    SqlCommand2.CommandType = System.Data.CommandType.Text
    SqlCommand2.CommandTimeout = 60
    Dim Query2 As String : Query2 = ""
    Dim Query As String : Query = ""
    Try


      Query = "SELECT * FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then


        DisplayEngineInfo = ""

        nloopCount = 0
        xLoop = 0
        adoRSAircraft.Read()

        If Not IsDBNull(adoRSAircraft("amod_airframe_type_code")) Then
          sAirframeType = adoRSAircraft("amod_airframe_type_code").ToString.Trim.ToLower
        End If
        If Not IsDBNull(adoRSAircraft("amod_type_code")) Then
          sAircraftType = adoRSAircraft("amod_type_code").ToString.Trim.ToLower
        End If

        DisplayEngineInfo = DisplayEngineInfo & "<tr><td height='5'></td></tr>"

        DisplayEngineInfo = DisplayEngineInfo & "<tr><td colspan='2' class='header_text'>Engine Information</th></tr>"
        DisplayEngineInfo = DisplayEngineInfo & "<tr><td width='2%'>&nbsp;</td><td valign='middle'><font class='small_header_text'>Engine Model: "
        DisplayEngineInfo = DisplayEngineInfo & "</font><font class='text_text'>" & adoRSAircraft("ac_engine_name") & "&nbsp;</font>"


        If Not IsDBNull(adoRSAircraft("ac_engine_tbo_oc_flag")) Then
          DisplayEngineInfo = DisplayEngineInfo & ", <font class='small_header_text'>On&nbsp;Condition&nbsp;TBO: "
          If adoRSAircraft("ac_engine_tbo_oc_flag").ToString.Trim.ToUpper = "Y" Then
            DisplayEngineInfo = DisplayEngineInfo & "&nbsp;Yes</font></td>"
          Else
            DisplayEngineInfo = DisplayEngineInfo & "&nbsp;No</font></td>"
          End If
        End If
        DisplayEngineInfo = DisplayEngineInfo & "</tr>" & vbCrLf


        DisplayEngineInfo = DisplayEngineInfo & "<tr><td width='2%'>&nbsp;</td><td valign='middle'  colspan='2' nowrap><font class='small_header_text'>Engine&nbsp;Maintenance&nbsp;Program:</font> "

        'DisplayEngineInfo = DisplayEngineInfo & GetEngineMaintenanceInfo(tmpAdoRs, True, False)


        Query2 = "SELECT emp_name, emp_provider_name, emp_program_name FROM Engine_Maintenance_Program WITH(NOLOCK)"
        Query2 = Query2 & " WHERE emp_id = " & adoRSAircraft("ac_engine_maintenance_prog_EMP")

        'ac_engine_maintenance_prog_EMP
        SqlCommand2.CommandText = Query2
        adoRSAircraft2 = SqlCommand2.ExecuteReader()
        adoRSAircraft2.Read()
        If adoRSAircraft2.HasRows Then
          DisplayEngineInfo = DisplayEngineInfo & "<font class='text_text'>"
          DisplayEngineInfo = DisplayEngineInfo & Trim(adoRSAircraft2("emp_provider_name")) & "&nbsp;-&nbsp;" & adoRSAircraft2("emp_program_name") & "&nbsp;</font>"
          adoRSAircraft2.Close()
        End If

        adoRSAircraft2 = Nothing


        '  DisplayEngineInfo = DisplayEngineInfo & "</td></tr><tr><td width='2%'></td><td valign='middle' colspan='2' nowrap><font  class='text_text'>Engine&nbsp;Management&nbsp;Program: </font>"



        '   DisplayEngineInfo = DisplayEngineInfo & GetEngineManagementInfo(tmpAdoRs)



        DisplayEngineInfo = DisplayEngineInfo & "</td></tr>"




        DisplayEngineInfo = DisplayEngineInfo & "<tr><td colspan='2'><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'><tr>"
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

        If sAirframeType <> "R" Then
          nloopCount = 4
        Else
          nloopCount = 3
        End If

        For xLoop = 1 To nloopCount

          If (Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shs_cycles"))) Then

            If xLoop = 1 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Engine&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 2 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Engine&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            ElseIf xLoop = 3 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Engine&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 4 And sAirframeType <> "R" Then
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Engine&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Engine&nbsp;" & CStr(xLoop) & "&nbsp;</td>"
            End If

            DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & adoRSAircraft("ac_engine_" & CStr(xLoop) & "_ser_no") & "</td>"


            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tot_hrs")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_hrs")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shi_hrs")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tbo_hrs")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_snew_cycles")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_cycles")), 0, True, False, True) & "</td>"
            Else
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shs_cycles")) Then
              DisplayEngineInfo = DisplayEngineInfo & "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shs_cycles")), 0, True, False, True) & "</td>"
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
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Engines: " & ex.Message
    End Try
  End Function


  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Function name: Build_HTML_Page
  ' Purpose: to build html open/close tags
  ' Parameters: viewToPdf - the table content
  ' Return: 
  '       String - in html table row format
  ' Change Log
  '           05/27/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Build_HTML_Page(ByVal viewToPDF As String) As String
    Build_HTML_Page = ""
    Try

      viewToPDF = viewToPDF & "</body></html>"
      Build_HTML_Page = viewToPDF
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_HTML_Page: " & ex.Message
    End Try
  End Function
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Function name: Build_PDF_Template_Header
  ' Purpose: to build html head info for the PDF Template page
  ' Parameters: none
  ' Return: 
  '       String - in html table row format
  ' Change Log
  '           05/27/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Build_PDF_Template_Header() As String
    Build_PDF_Template_Header = ""
    Try
      Dim sServerMapPath As String = ""
      Dim sSiteStyleSheet As String = "common\jetnet.css"
      sServerMapPath = Server.MapPath(sSiteStyleSheet)
      Dim txtFile As New System.IO.StreamReader(sServerMapPath)
      Dim readStyle As String = ""
      Dim formatStyle As String = ""
      readStyle = "<style>"
      Do While txtFile.EndOfStream <> True
        formatStyle = txtFile.ReadLine()
        If Trim(formatStyle.StartsWith(".border_bottom")) Then
          formatStyle = formatStyle.Replace("0px", "1px")
        ElseIf Trim(formatStyle.StartsWith(".border_bottom_right")) Then
          formatStyle = formatStyle.Replace("0px", "1px")
        ElseIf Trim(formatStyle.StartsWith(".leftside")) Then
          formatStyle = formatStyle.Replace("0px", "1px")
        ElseIf Trim(formatStyle.StartsWith(".rightside")) Then
          formatStyle = formatStyle.Replace("0px", "1px")
        ElseIf Trim(readStyle.EndsWith("TD{")) Then
          formatStyle = formatStyle.Replace("8pt", "12pt")
        End If
        readStyle = readStyle & formatStyle
      Loop
      readStyle = readStyle & ".break { page-break-before: always; }" & vbCrLf


      If Me.WD.SelectedValue = "Word" Then
        readStyle = readStyle & ".small_text{font-family:Arial;font-size: xx-small}" & vbCrLf
        readStyle = readStyle & ".small_bold_text{font-family:Arial;font-size: xx-small; font-weight: bold;}" & vbCrLf
      Else
        readStyle = readStyle & ".small_text{font-family:Arial;font-size: x-small}" & vbCrLf
        readStyle = readStyle & ".small_bold_text{font-family:Arial;font-size: x-small; font-weight: bold;}" & vbCrLf
      End If



      readStyle = readStyle & ".table_specs{font-size:12px;}" & vbCrLf
      readStyle = readStyle & "</style>" & vbCrLf
      Build_PDF_Template_Header = "<html><head>" & vbCrLf & readStyle & "</head><body>" & vbCrLf

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Template_Header: " & ex.Message
    End Try
  End Function
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Function name: Build_PDF_Template_Footer
  ' Purpose: to build html footer info for the PDF Template page
  ' Parameters: none
  ' Return: 
  '       String - in html table row format
  ' Change Log
  '           05/27/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Build_PDF_Template_Footer() As String
    Build_PDF_Template_Footer = ""
    Try
      Build_PDF_Template_Footer = "<table width='100%' align='center'><tr id='trMaintbl_Footer'><hr />"
      Build_PDF_Template_Footer = Build_PDF_Template_Footer & "<td  id='tdMaintbl_Footer' align='center'>JETNET Evolution Model Market Summary Report"
      Build_PDF_Template_Footer = Build_PDF_Template_Footer & "</td></tr></table>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Template_Footer: " & ex.Message
    End Try
  End Function
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Function name: Insert_Page_Break
  ' Purpose: to insert a page break into the html page
  ' Parameters: none
  ' Return: 
  '       String - in html table row format
  ' Change Log
  '           06/29/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Insert_Page_Break() As String
    Insert_Page_Break = ""
    Try
      Insert_Page_Break = "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Insert_Page_Break: " & ex.Message
    End Try
  End Function


  Public Function Build_String_To_HTML(ByVal ViewToPDF As String) As Boolean
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
  Function GetContactTypeForContactID(ByVal inContactType)
    Try
      Dim adors As System.Data.SqlClient.SqlDataReader : adors = Nothing
      Dim SqlConn As New System.Data.SqlClient.SqlConnection
      Dim SqlCommand As New System.Data.SqlClient.SqlCommand
      Dim Query As String = ""

      Query = "SELECT actype_name FROM Aircraft_Contact_Type WITH(NOLOCK) WHERE (actype_code = '" & inContactType & "'"

      ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users
      If Session("Aerodex") Then
        Query = Query & " AND actype_code NOT IN ('93','98','99','67','68','02'))"
      Else
        Query = Query & " AND actype_code NOT IN ('67','68','02'))"
      End If
      ' setup connection
      'Select Case My.Settings.whichDatabase
      ' Case "LOCAL"
      'SqlConn.ConnectionString = My.Settings.LOCAL
      '  Case "LIVE"
      SqlConn.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
      ' End Select
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
      SqlConn.Close()
    Catch ex As Exception
      GetContactTypeForContactID = ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetContactTypeForContactID: " & ex.Message
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

End Class
