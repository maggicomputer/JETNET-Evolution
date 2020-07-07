
Partial Public Class spec_sheet_aspx
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
  Public number_of_engine_types As Long = 0

  Public TYPE_OF_AC As String = ""
  Public PDF_Page_Flag As String = "N"
  Public ac_id As Integer = 0
  Public blind_report As String = "No"

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
      report_name += Session.Item("localUser").crmSubSubID.ToString.Trim + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString.Trim + "_" + commonEvo.GenerateFileName("PDF_SpecSheet", ".doc", False)
    Else
      report_name += Session.Item("localUser").crmSubSubID.ToString.Trim + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString.Trim + "_" + commonEvo.GenerateFileName("PDF_SpecSheet", ".html", False)
    End If

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs As System.Data.SqlClient.SqlDataReader : localAdoRs = Nothing

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn

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

      ViewToPDF = Build_PDF_CoverPage(ac_id)

      If SP.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Second_Page(ac_id)
      End If

      If Not BR.Checked Then
        If TP.Checked Then
          ViewToPDF = ViewToPDF & Insert_Page_Break()
          ViewToPDF = ViewToPDF & Build_PDF_Third_Page(ac_id)
        End If
      End If


      If PP.Checked Then
        ViewToPDF = ViewToPDF & Insert_Page_Break()
        ViewToPDF = ViewToPDF & Build_PDF_Pictures_Page(ac_id)
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
          Convert_To_PDF(amod_ID)
        End If


        new_pdf_name = Replace(report_name, "html", "pdf")
      Else
        Build_String_To_HTML(ViewToPDF)
        new_pdf_name = report_name
      End If

      Response.Redirect(Session.Item("MarketSummaryFolderVirtualPath").ToString + "/" + new_pdf_name)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in btnRunReport_Click: " & ex.Message
    End Try

  End Sub

  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Function name: Build_PDF_CoverPage
  ' Purpose: to build the cover page for the pdf file
  ' Parameters: none
  ' Return: 
  '       String - in html table row format
  ' Change Log
  '           05/27/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Build_PDF_CoverPage(ByVal ac_id As Integer) As String

    Dim Query As String : Query = ""
    Dim Query2 As String : Query2 = ""
    Dim font_size_for_address As String = ""

    Dim confidential As String = ""
    Dim airfram_tot_time As String = ""
    Dim cycles As String = ""
    Dim asking_price As String = ""
    Dim asking_type As String = ""
    Dim ac_status As String = ""
    Dim in_done_by_and_rating As String = ""
    Dim temp_moyear As String = ""
    Dim last_updated As String = ""
    Dim exclusive_flag As String = ""
    Dim list_date As String = ""
    Dim days_on_market As String = ""
    Dim ex_done_by_and_rating As String = ""
    Dim ex_date As String = ""
    Dim prev_owned As String = ""
    Dim times_of_date As String = ""

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
    Dim adoTemp As System.Data.SqlClient.SqlDataReader : adoTemp = Nothing
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Try

      ' THIS IS FOR AIRCRAFT QUERY

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

      Query = "SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address, "
      Query += "Company.comp_address1, Company.comp_address2, Company.comp_city, Company.comp_state, "
      Query += "Company.comp_zip_code FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0"
      Query += "WHERE(Subscription.sub_id = " + Session.Item("localSubscription").evoSubID.ToString.Trim + ")"

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = Query

      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        If Not IsDBNull(adoRSAircraft("comp_address1")) And Not IsDBNull(adoRSAircraft("comp_address2")) Then
          If (adoRSAircraft.Item("comp_address1").ToString.Length + adoRSAircraft.Item("comp_address2").ToString.Length) > 50 Then
            font_size_for_address = "-2"
          Else
            font_size_for_address = "-1"
          End If
        Else

        End If

        If Not IsDBNull(adoRSAircraft("comp_address1")) Then
          If Me.WD.SelectedValue.ToString = "Word" Then
            htmlOutput += "<tr valign='top'><td class='white_feat_header_text'><font color='white' size='" + font_size_for_address + "'>" + adoRSAircraft.Item("comp_address1").ToString.Trim
          Else
            htmlOutput += "<tr valign='top'><td class='CompInfo'><font color='white'>" + adoRSAircraft.Item("comp_address1").ToString.Trim + "</font>"
          End If
        Else
          If Me.WD.SelectedValue.ToString = "Word" Then
            htmlOutput += "<tr valign='top'><td class='white_feat_header_text'><font color='white' size='-1'>"
          Else
            htmlOutput += "<tr valign='top'><td class='CompInfo'>"
          End If

        End If

        If Not IsDBNull(adoRSAircraft("comp_address2")) Then
          htmlOutput += "<font color='white'> . " + adoRSAircraft.Item("comp_address2").ToString.Trim + "</font>"
        End If

        If font_size_for_address = "-2" Then
          htmlOutput += "<br>"
        End If

        If Not IsDBNull(adoRSAircraft("comp_city")) Then
          htmlOutput += "<font color='white'> " + adoRSAircraft.Item("comp_city").ToString.Trim + "</font>"
        End If
        If Not IsDBNull(adoRSAircraft("comp_state")) Then
          htmlOutput += "<font color='white'>, " + adoRSAircraft.Item("comp_state").ToString.Trim + "</font>"
        End If
        If Not IsDBNull(adoRSAircraft("comp_zip_code")) Then
          htmlOutput += "<font color='white'> " + adoRSAircraft.Item("comp_zip_code").ToString.Trim + "</font></td></tr>"
        Else
          htmlOutput += "</td></tr>"
        End If

        Dim phone_info_string As String = Phone_Info(Session.Item("localSubscription").evoSubID.ToString.Trim, "white")

        htmlOutput += phone_info_string

        If phone_info_string.Length = 0 Then
          htmlOutput += "<tr><td>"
        Else
          htmlOutput += "<font color='white'> &#8226; </font>"
        End If

        If Not IsDBNull(adoRSAircraft("comp_web_address")) Then
          htmlOutput += "<font color='white'><u>" + adoRSAircraft.Item("comp_web_address").ToString.Trim + "</u></font></td></tr>"
        Else
          htmlOutput += "</td></tr>"
        End If
      End If

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      htmlOutput = Build_PDF_Header("Aircraft Information (Page 1 of 2)", htmlOutput)

      'SECTION 2  - AIRCRAFT INFO  -----------------------------------------------------------------------------------------------------------------------------------

      htmlOutput += "<tr><td height='5'></td></tr>"

      Query2 = "SELECT * FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query2 = Query2 & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)

      SqlCommand.CommandText = Query2
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        htmlOutput += "<tr><td colspan='2' class='header_text'>Aircraft Identification</b></font></td></tr>"

        If Not IsDBNull(adoRSAircraft("ac_confidential_notes")) Then
          If Not String.IsNullOrEmpty(adoRSAircraft.Item("ac_confidential_notes").ToString) Then
            confidential += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Note: &nbsp;</font>"
            confidential += "<font class='text_text'>" + adoRSAircraft.Item("ac_confidential_notes").ToString.Trim + "&nbsp;</td></tr>"
          End If
        End If

        ' make
        If Not IsDBNull(adoRSAircraft("amod_make_name")) Then
          htmlOutput += "<tr><td width='2%'>&nbsp;</td><td NOWRAP><font class='small_header_text'>Model: </font><font class='text_text'> "
          htmlOutput += adoRSAircraft.Item("amod_make_name").ToString + "&nbsp;"
        End If
        ' model
        If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
          htmlOutput += adoRSAircraft.Item("amod_model_name") + "</font></td></tr>"
        End If

        ' year of manufacture
        If Not IsDBNull(adoRSAircraft("ac_mfr_year")) Then
          htmlOutput += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Year of Manufacture: </font><font class='text_text'>" + adoRSAircraft.Item("ac_mfr_year").ToString + "</font>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_year")) Then
          htmlOutput += "<font class='small_header_text'>, Delivery: </font><font class='text_text'>" + adoRSAircraft.Item("ac_year").ToString + "</font></td></tr>"
        Else
          htmlOutput += "</td></tr>"
        End If

        If Not BR.Checked Then
          ' serial number
          If Not IsDBNull(adoRSAircraft("ac_ser_no_full")) Then
            htmlOutput += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Serial #: </font><font class='text_text'>" + adoRSAircraft.Item("ac_ser_no_full").ToString + "</font></td></tr>"
          End If
          ' reg number
          If Not IsDBNull(adoRSAircraft("ac_reg_no")) Then
            htmlOutput += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Registration #: </font><font class='text_text'>" + adoRSAircraft.Item("ac_reg_no").ToString
            If Not IsDBNull(adoRSAircraft("ac_reg_no_expiration_date")) Then
              htmlOutput += " [Expires: " + adoRSAircraft.Item("ac_reg_no_expiration_date") + "]"
            End If
            htmlOutput += "</font></td></tr>"
          End If
          ' airframe total time
          If Not IsDBNull(adoRSAircraft("ac_purchase_date")) Then
            htmlOutput += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Purchase Date: </font><font class='text_text'>" + FormatDateTime(adoRSAircraft.Item("ac_purchase_date").ToString, vbShortDate) + "</font>"
          End If
        End If

        If Not IsDBNull(adoRSAircraft("ac_airframe_tot_hrs")) Then
          airfram_tot_time = "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Airframe Total Time(AFTT): </font><font class='text_text'>" + FormatNumber(adoRSAircraft.Item("ac_airframe_tot_hrs").ToString, 0) + "</font></td></tr>"
        End If

        ' landing cycles
        If Not IsDBNull(adoRSAircraft("ac_airframe_tot_landings")) Then
          cycles = "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text'>Landings/Cycles: </font><font class='text_text'>" + FormatNumber(adoRSAircraft.Item("ac_airframe_tot_landings").ToString, 0) + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_foreign_currency_price")) Then
          asking_price = "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'>"
          asking_price += "<font class='small_header_text'>Asking Amt (" + adoRSAircraft.Item("ac_foreign_currency_name").ToString.Trim
          asking_price += "): </font><font class='text_text'>" + FormatNumber(adoRSAircraft.Item("ac_foreign_currency_price"), 0) + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_asking")) Then
          asking_type = adoRSAircraft.Item("ac_asking").ToString

          If Not IsDBNull(adoRSAircraft("ac_status")) Then
            ac_status = adoRSAircraft.Item("ac_status").ToString.ToUpper
            If ac_status.ToUpper.Contains("FOR SALE/TRADE") And asking_type.ToUpper.Contains("SALE/TRADE") Then
              ac_status = ""
            Else
              ac_status = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Status: </font><font class='text_text'>"
              ac_status += adoRSAircraft.Item("ac_status").ToString.Trim & "</font></td></tr>"
            End If
          End If

          If (asking_type.ToUpper.Contains("PRICE")) Then
            asking_type = ""
            If Not IsDBNull(adoRSAircraft("ac_asking_price")) Then
              asking_price += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'>"
              asking_price += "<font class='small_header_text'>Asking Amt (USD): </font><font class='text_text'>"
              If Not String.IsNullOrEmpty(addAsking.Text) Then
                asking_price += FormatCurrency(CDbl(adoRSAircraft.Item("ac_asking_price").ToString) + CDbl(addAsking.Text), 0) & "</font></td></tr>"
              Else
                asking_price += FormatCurrency(CDbl(adoRSAircraft.Item("ac_asking_price").ToString), 0) & "</font></td></tr>"
              End If
            End If
          Else
            asking_type = "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'>"
            asking_type += "<font class='small_header_text'>Asking: </font><font class='text_text'>"
            If Me.addAsking.Text.Trim.ToString <> "" Then
              asking_type += Me.addAsking.Text.Trim.ToString
            Else
              asking_type += adoRSAircraft.Item("ac_asking").ToString
            End If
            asking_type += "</font></td></tr>"

          End If
        Else
          If Not IsDBNull(adoRSAircraft("ac_asking_price")) Then
            asking_price += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'>"
            asking_price += "<font class='small_header_text'>Asking Amt (USD): </font><font class='text_text'>"
            If Not String.IsNullOrEmpty(addAsking.Text) Then
              asking_price += FormatCurrency(CDbl(adoRSAircraft.Item("ac_asking_price").ToString) + CDbl(addAsking.Text), 0) & "</font></td></tr>"
            Else
              asking_price += FormatCurrency(CDbl(adoRSAircraft.Item("ac_asking_price").ToString), 0) & "</font></td></tr>"
            End If
          End If
        End If

        ' asking amt
        If Not IsDBNull(adoRSAircraft("ac_interior_doneby_name")) Then
          in_done_by_and_rating += "<tr><td width='2%'>&nbsp;</td><td nowrap='NOWRAP'><font class='small_header_text2'>Done By: </font><font class='text_text'>" + adoRSAircraft.Item("ac_interior_doneby_name").ToString.Trim + "</font></td></tr>"
        End If
        If Not IsDBNull(adoRSAircraft("ac_interior_rating")) Then
          in_done_by_and_rating += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Rating: </font><font class='text_text'>" + adoRSAircraft.Item("ac_interior_rating").ToString.Trim + "</font></td></tr>"
        End If
        If Not IsDBNull(adoRSAircraft("ac_passenger_count")) Then
          in_done_by_and_rating += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Number of Passengers: </font><font class='text_text'>" + adoRSAircraft.Item("ac_passenger_count").ToString.Trim + "</font></td></tr>"
        End If

        ' THIS IS FOR INTERIOR SECTION ------ 
        If Not IsDBNull(adoRSAircraft("ac_interior_moyear")) Then

          If adoRSAircraft.Item("ac_interior_moyear").ToString.Length > 4 Then
            temp_moyear = adoRSAircraft.Item("ac_interior_moyear")
            If temp_moyear.ToString.Length = 5 Then
              temp_moyear = Left(temp_moyear, 1) + "/" + Right(temp_moyear, 4)
            Else
              If temp_moyear.ToString.Length = 6 Then
                temp_moyear = Left(temp_moyear, 2) + "/" + Right(temp_moyear, 4)
              End If
            End If
            last_updated = " (<font class='small_header_text'>Updated: </font><font class='text_text'> " + temp_moyear + "</font>)"
          Else
            last_updated = " (<font class='small_header_text'>Updated: </font><font class='text_text'> " + adoRSAircraft.Item("ac_interior_moyear").ToString.Trim + "</font>)"
          End If
        End If
      End If
      ' THIS IS FOR INTERIOR SECTION ------ TP.Checked Or PP.Checked Or SP.Checked

      If Not BR.Checked And Not EB.Checked Then

        If Not IsDBNull(adoRSAircraft("ac_exclusive_flag")) Then

          If (adoRSAircraft.Item("ac_exclusive_flag").ToString.ToUpper = "Y") Then

            exclusive_flag = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>On Exclusive "

            Query = "select top 1 comp_id, actype_name, cref_owner_percent, comp_name, contact_id,"
            Query += " contact_email_address from aircraft_reference inner join company on cref_comp_id = comp_id and cref_journ_id = comp_journ_id inner join aircraft_contact_type on cref_contact_type = actype_code"
            Query += " left outer join contact on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_hide_flag='N'"
            Query += " where(cref_ac_id = '" & ac_id.ToString & "') and cref_journ_id = 0 and cref_contact_type <> '71' and actype_name = 'Exclusive Broker' order by cref_transmit_seq_no"

            SqlConn2.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
            SqlConn2.Open()

            SqlCommand2.Connection = SqlConn2
            SqlCommand2.CommandType = System.Data.CommandType.Text
            SqlCommand2.CommandTimeout = 60
            SqlCommand2.CommandText = Query
            adoTemp = SqlCommand2.ExecuteReader()

            If adoTemp.HasRows Then

              Do While adoTemp.Read

                If Not IsDBNull(adoTemp("actype_name")) Then
                  exclusive_flag += " with " + adoTemp.Item("comp_name").ToString.Trim
                End If

              Loop

            End If

            adoTemp.Close()
            adoTemp = Nothing

            exclusive_flag += "</font></td></tr>"

          End If

        End If

      End If

      If Not IsDBNull(adoRSAircraft("ac_list_date")) Then
        list_date = "Date Listed: " + adoRSAircraft.Item("ac_list_date").ToString.Trim
        days_on_market = DateDiff("d", adoRSAircraft.Item("ac_list_date"), Now())
      End If

      If Not IsDBNull(adoRSAircraft("ac_exterior_doneby_name")) Then
        ex_done_by_and_rating += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Done By: </font><font class='text_text'>" + adoRSAircraft.Item("ac_exterior_doneby_name").ToString.Trim + "</font></td></tr>"
      End If
      If Not IsDBNull(adoRSAircraft("ac_exterior_rating")) Then
        ex_done_by_and_rating += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Rating: </font><font class='text_text'>" + adoRSAircraft.Item("ac_exterior_rating").ToString.Trim + "</font></td></tr>"
      End If

      If adoRSAircraft.Item("ac_exterior_moyear").ToString.Trim.Length > 0 Then
        If adoRSAircraft.Item("ac_exterior_moyear").ToString.Trim.Length > 4 Then
          temp_moyear = adoRSAircraft.Item("ac_exterior_moyear")
          If temp_moyear.ToString.Trim.Length = 5 Then
            temp_moyear = Left(temp_moyear, 1) + "/" + Right(temp_moyear, 4)
          Else
            If temp_moyear.ToString.Trim.Length = 6 Then
              temp_moyear = Left(temp_moyear, 2) + "/" + Right(temp_moyear, 4)
            End If
          End If
          ex_date = " (<font class='small_header_text'>Updated: </font><font class='text_text'>" + temp_moyear + "</font>)</td>"
        Else
          ex_date = " (<font class='small_header_text'>Updated: </font><font class='text_text'>" + adoRSAircraft.Item("ac_exterior_moyear").ToString.Trim + "</font>)</td>"
        End If
      End If

      If Not IsDBNull(adoRSAircraft("ac_previously_owned_flag")) Then
        prev_owned = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Previously Owned</font></td></tr>"
      End If

      If Not IsDBNull(adoRSAircraft("ac_times_as_of_date")) Then
        times_of_date = "(Times as of " + adoRSAircraft.Item("ac_times_as_of_date").ToString.Trim + ")"
      End If

      'SECTION 2  - AIRCRAFT INFO  -----------------------------------------------------------------------------------------------------------------------------------
      If (adoRSAircraft.Item("ac_forsale_flag").ToString.ToUpper = "Y") Then

        htmlOutput += "<tr><td height='5'></td></tr>"
        htmlOutput += "<tr><td colspan='2' width='100%' class='header_text'>For Sale Information (" + list_date + ")</b></font>&nbsp;</td></tr>"
        htmlOutput += ac_status
        htmlOutput += asking_type

        htmlOutput += asking_price
        htmlOutput += confidential
        htmlOutput += exclusive_flag
        'added MSW 2/26/2013 - upon jason's customer request
        If Trim(days_on_market) <> "" Then
          htmlOutput += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>Days on Market: </font><font class='text_text'>" + days_on_market + "</font></td></tr>"
        End If
      End If

      If (Not String.IsNullOrEmpty(airfram_tot_time) Or Not String.IsNullOrEmpty(cycles)) Then
        htmlOutput += "<tr><td height='5'></td></tr>"
        htmlOutput += "<tr><td colspan='2' width='100%' class='header_text'>Usage</font>&nbsp;"
        htmlOutput += times_of_date
        htmlOutput += airfram_tot_time
        htmlOutput += cycles
        htmlOutput += prev_owned
      End If

      '----------------------------------------- END OF DETAILS SECTION----------------------------------------------------------------

      htmlOutput += Aircraft_Details("Interior", last_updated, in_done_by_and_rating)
      htmlOutput += Aircraft_Details("Exterior", ex_date, ex_done_by_and_rating)
      htmlOutput += Build_PDF_Format()
      htmlOutput += Build_PDF_First_Picture(ac_id)
      htmlOutput += Aircraft_Build_PDF_Features(ac_id)
      htmlOutput += Build_PDF_Airport_Information(ac_id)
      htmlOutput += End_Page()

      adoRSAircraft.Close()

    Catch SqlException

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      adoTemp.Close()
      adoTemp = Nothing

      SqlConn.Dispose()
      SqlConn2.Dispose()
      SqlCommand.Dispose()
      SqlCommand2.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlCommand2.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn2.Close()
      SqlConn2.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Phone_Company_Contact_Info(ByVal comp_id As Integer, ByVal contact_id As Integer) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim Query As String : Query = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "select distinct pnum_type, pnum_number_full from Phone_Numbers where pnum_contact_id = " & contact_id & " and pnum_comp_id = " & comp_id & "AND pnum_journ_id = 0 AND pnum_hide_customer = 'N'"
      Query = Query & " ORDER BY pnum_type desc"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then
        Do While adoRSAircraft.Read
          htmlOutput += adoRSAircraft.Item("pnum_type") & ": " & adoRSAircraft.Item("pnum_number_full") & "<br />"
        Loop
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Phone_Company_Contact_Info: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Phone_Info(ByVal Sub_ID As String, ByVal color As String) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String : Query = ""

    Try
      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      '------------------------------------------------------------- PHONE INFO-----------------
      Query = "SELECT top 1 pnum_type, pnum_number_full FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 INNER JOIN Phone_Numbers"
      ' Query = Query & " WITH(NOLOCK), Phone_Type WITH(NOLOCK)"
      Query = Query & " ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id"
      Query = Query & " INNER JOIN Phone_Type ON ptype_name = pnum_type "
      Query = Query & " WHERE Subscription.sub_id = " & Sub_ID
      Query = Query & " AND pnum_journ_id = 0"
      Query = Query & " AND pnum_hide_customer = 'N' AND pnum_contact_id = 0 " 'and pnum_type = 'Office'"
      Query = Query & " ORDER BY ptype_seq_no asc"  ' QUERY EDITED TO DISPLAY TOLL FREE THEN OFFICE THEN FAX

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        Do While adoRSAircraft.Read
          If color = "white" Then
            If Me.WD.SelectedValue.ToString = "Word" Then
              htmlOutput += "<tr><td><font class='white_feat_text' size='-1'>" & adoRSAircraft.Item("pnum_type") & ": </font><font class='white_feat_text' size='-1'>" & adoRSAircraft.Item("pnum_number_full") & "</font>"
            Else
              htmlOutput += "<tr><td><font color='white'>" & adoRSAircraft.Item("pnum_type") & ": </font><font color='white' size='+1'>" & adoRSAircraft.Item("pnum_number_full") & "</font>"
            End If
          Else

            htmlOutput += "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text'>" & adoRSAircraft.Item("pnum_type") & ": </font><font class='text_text'>" & adoRSAircraft.Item("pnum_number_full") & "</font></li></td></tr>"
          End If
        Loop
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Phone_Info: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Aircraft_Details(ByVal detail_type As String, ByVal update_date As String, ByVal done_by As String) As String

    Dim htmlOutput As String = ""
    Dim last_detail_type As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Dim Query As String : Query = ""

    Try

      Query = "SELECT * FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " & ac_id
      Query = Query & " AND adet_journ_id = '0' AND adet_data_type = '" & detail_type & "'"

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        htmlOutput = "<tr><td height='5'></td></tr>"

        htmlOutput += "<tr><td colspan='2' width='100%' class='header_text'>" & detail_type & " Details " & update_date & "</font>&nbsp;</td></tr>"
        htmlOutput += done_by
        htmlOutput += "<tr><td width='2%'>&nbsp;</td><td>"

        Do While adoRSAircraft.Read

          If Not IsDBNull(adoRSAircraft("adet_data_name")) Then

            If last_detail_type.ToUpper.Trim = adoRSAircraft.Item("adet_data_name").ToString.ToUpper.Trim Then
              htmlOutput += "<font class='text_text'> " + adoRSAircraft.Item("adet_data_description").ToString.Trim + "; </font>"
            Else
              htmlOutput += "<font class='small_header_text2'>" + adoRSAircraft.Item("adet_data_name").ToString.Trim + "</font><font class='text_text2'>: " + adoRSAircraft.Item("adet_data_description").ToString.Trim + "; </font>"
            End If
          Else
            htmlOutput += "<font class='small_header_text2'>" + adoRSAircraft.Item("adet_data_name").ToString.Trim + "</font><font class='text_text2'>: " + adoRSAircraft.Item("adet_data_description").ToString.Trim + "; </font>"
          End If

          last_detail_type = adoRSAircraft.Item("adet_data_name").ToString.Trim

        Loop

      End If

      htmlOutput += "</td></tr>"

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Aircraft_Details: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function End_Page() As String

    Dim sTmpStr As String = ""

    Try
      sTmpStr += "</td></tr></table>" ' this is for the end of right column
      sTmpStr += "</td></tr></table>" ' this is for the end of the entire apge
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in End_Page: " & ex.Message
    End Try

    Return sTmpStr

  End Function

  Public Function Build_PDF_Pictures_Page(ByVal ac_id As Integer) As String

    Dim sTmpStr As String = ""

    Try
      sTmpStr += Build_PDF_Header2("Additional Aircraft Pictures")
      sTmpStr += Build_PDF_Pictures_Page_Pics(ac_id)
      sTmpStr += "</td></tr></table>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Header: " & ex.Message
    End Try

    Return sTmpStr

  End Function

  Public Function Build_PDF_Third_Page(ByVal ac_id As Integer) As String

    Dim sTmpStr As String = ""

    Try
      sTmpStr += Build_PDF_Header2("Aircraft Contacts")
      sTmpStr += Aircraft_Contacts(ac_id)
      sTmpStr += End_Page()
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Third_Page: " & ex.Message
    End Try

    Return sTmpStr

  End Function

  Public Function Build_PDF_Second_Page(ByVal ac_id As Integer) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim ac_maintained As String = ""
    Dim ac_airframe_maint_tracking_prog_AMTP As String = ""
    Dim ac_airframe_maintenance_prog_AMP As String = ""
    Dim cert_information As String = ""
    Dim en_overhaul As String = ""
    Dim hot_inspec As String = ""
    Dim dam_history As String = ""
    Dim Query As String : Query = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "select ac_id, ac_ser_no_full, amp_program_name, amtp_program_name, ac_maint_hots_by_name, ac_maint_eoh_by_name, ac_damage_history_notes, ac_maintained, ac_apu_model_name, ac_apu_tot_hrs, accert_name from aircraft"
      Query = Query & " inner join Airframe_Maintenance_Program on ac_airframe_maintenance_prog_AMP=amp_id"
      Query = Query & " inner join Airframe_Maintenance_Tracking_Program on ac_airframe_maint_tracking_prog_AMTP=amtp_id"
      Query = Query & " Inner join Aircraft_Certified on accert_ac_id = " & ac_id & " and accert_ac_journ_id = '0'"
      Query = Query & " where(ac_id = " & ac_id & ") and ac_journ_id = 0"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        If Not IsDBNull(adoRSAircraft("ac_maintained")) Then
          ac_maintained = ac_maintained & "<tr><td width='2%'>&nbsp;</td>"
          ac_maintained = ac_maintained & "<td><font class='small_header_text2'>Maintained: </font><font class='text_text'>" + adoRSAircraft.Item("ac_maintained").ToString.Trim + "</font>" ' & "</td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("amp_program_name")) Then
          ac_airframe_maintenance_prog_AMP = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Airframe Maintenance Program: </font><font class='text_text2'>" + adoRSAircraft.Item("amp_program_name").ToString.Trim + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("amtp_program_name")) Then
          ac_airframe_maint_tracking_prog_AMTP = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Airframe Maintenance Tracking Program: </font><font class='text_text2'>" + adoRSAircraft.Item("amtp_program_name").ToString.Trim + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("accert_name")) Then
          cert_information = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Certification: </font><font class='text_text2'>" + adoRSAircraft.Item("accert_name").ToString.Trim + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_maint_hots_by_name")) Then
          hot_inspec = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Hot Inspection By: </font><font class='text_text2'>" + adoRSAircraft.Item("ac_maint_hots_by_name").ToString.Trim + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_maint_eoh_by_name")) Then
          en_overhaul = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Engine Overhaul By: </font><font class='text_text2'>" + adoRSAircraft.Item("ac_maint_eoh_by_name").ToString.Trim + "</font></td></tr>"
        End If

        If Not IsDBNull(adoRSAircraft("ac_damage_history_notes")) Then
          dam_history = "<tr><td width='2%'>&nbsp;</td><td><font class='small_header_text2'>Dam History Notes: </font><font class='text_text2'>" + adoRSAircraft.Item("ac_damage_history_notes").ToString.Trim + "</font></td></tr>"
        End If

      End If

      adoRSAircraft.Close()

      htmlOutput += Build_PDF_Header("Aircraft Information (Page 2 of 2)", "")
      htmlOutput += DisplayEngineInfo(ac_id)

      htmlOutput += heli_details(ac_id, 0)

      htmlOutput += Aircraft_APU(ac_id)
      htmlOutput += Aircraft_Details("Maintenance", ac_maintained, "")
      htmlOutput += ac_airframe_maintenance_prog_AMP & ac_airframe_maint_tracking_prog_AMTP & cert_information
      htmlOutput += hot_inspec & en_overhaul & dam_history
      htmlOutput += Aircraft_Details("Equipment", "", "")
      htmlOutput += Aircraft_Details("Addl Cockpit Equipment", "", "")
      htmlOutput += Aircraft_Avionics(ac_id)

      htmlOutput += Build_PDF_Format()
      htmlOutput += Build_PDF_Pictures(ac_id)
      htmlOutput += End_Page()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Second_Page: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Lease_Information(ByVal ac_id As Integer) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String = ""
    Dim htmlOutput As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "SELECT * FROM Aircraft_Lease WITH(NOLOCK) WHERE aclease_ac_id = " & ac_id & " AND aclease_expired <> 'Y'"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        htmlOutput += "<br /><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0' width='60%'><tr>" & vbCrLf
        htmlOutput += "<th colspan='4'>LEASE INFORMATION</th></tr><tr>" & vbCrLf

        htmlOutput += "<th class='Normal'><font class='Label'>Type</font></th>"
        htmlOutput += "<th class='Normal'><font class='Label'>Term</font></th>"
        htmlOutput += "<th class='Normal'><font class='Label'>Expiration Date</font></th>"
        htmlOutput += "<th class='Normal'><font class='Label'>Expiration Confirmed</font></th>"
        htmlOutput += "</tr><tr>"

        Do While Not adoRSAircraft.Read

          If Not (IsDBNull(adoRSAircraft("aclease_type"))) And adoRSAircraft.Item("aclease_type").ToString.Trim <> "" Then
            htmlOutput += adoRSAircraft.Item("aclease_type").ToString.Trim
          End If

          If Not (IsDBNull(adoRSAircraft("aclease_term"))) And adoRSAircraft.Item("aclease_term").ToString.Trim <> "" Then
            htmlOutput += adoRSAircraft.Item("aclease_term").ToString.Trim
          End If

          If Not (IsDBNull(adoRSAircraft("aclease_expiration_date"))) And adoRSAircraft.Item("aclease_expiration_date").ToString.Trim <> "" Then
            htmlOutput += FormatDateTime(adoRSAircraft.Item("aclease_expiration_date"), vbShortDate)
          End If

          If Not (IsDBNull(adoRSAircraft("aclease_exp_confirm_date"))) And adoRSAircraft.Item("aclease_exp_confirm_date").ToString.Trim <> "" Then
            htmlOutput += FormatDateTime(adoRSAircraft.Item("aclease_exp_confirm_date"), vbShortDate)
          End If


          htmlOutput += "Notes"

          If Not (IsDBNull(adoRSAircraft("aclease_note"))) And adoRSAircraft.Item("aclease_note").ToString.Trim <> "" Then
            htmlOutput += adoRSAircraft.Item("aclease_note").ToString.Trim
          End If

          htmlOutput += "</tr>"

        Loop

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Lease_Information: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Aircraft_Avionics(ByVal ac_id As Integer) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String = ""

    Try
      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      ' Start Avionics    
      Query = "SELECT * FROM Aircraft_Avionics WITH(NOLOCK) WHERE av_ac_id = " & ac_id & " AND av_ac_journ_id = '0'"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        htmlOutput += "<tr><td height='5'></td></tr>"
        htmlOutput += "<tr><td colspan='2' class='header_text'>Avionics</td></tr>"
        htmlOutput += "<tr><td with='2%'>&nbsp;</td><td>"

        Do While adoRSAircraft.Read
          htmlOutput += "<font class='small_header_text2'>" + adoRSAircraft.Item("av_name").ToString.Trim + ": </font><font class='text_text'>" + adoRSAircraft.Item("av_description").ToString.Trim + "; </font>"
        Loop
        htmlOutput += "</td></tr>"

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Aircraft_Avionics: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Build_PDF_Airport_Information(ByVal ac_id As Integer) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String : Query = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60


      Query = "SELECT ac_aport_iata_code, ac_aport_icao_code, ac_aport_name, ac_aport_city, ac_aport_state, ac_aport_country FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & CStr(ac_id)

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        htmlOutput += "<br /><table align='center'><tr><td align='center' class='white_feat_header_text'>Airport Information</td></tr><tr><td class='white_feat_text'>"

        If Not IsDBNull(adoRSAircraft("ac_aport_iata_code")) Then
          htmlOutput += adoRSAircraft.Item("ac_aport_iata_code").ToString.Trim
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_icao_code")) Then
          htmlOutput += " - " + adoRSAircraft.Item("ac_aport_icao_code").ToString.Trim
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_name")) Then
          htmlOutput += " - " + adoRSAircraft.Item("ac_aport_name").ToString.Trim
        End If


        If Not IsDBNull(adoRSAircraft("ac_aport_city")) Then
          htmlOutput += "<br />" + adoRSAircraft.Item("ac_aport_city").ToString.Trim + vbCrLf
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_state")) Then
          htmlOutput += " - " + adoRSAircraft.Item("ac_aport_state").ToString.Trim + vbCrLf
        End If

        If Not IsDBNull(adoRSAircraft("ac_aport_country")) Then
          htmlOutput += " - " + adoRSAircraft.Item("ac_aport_country").ToString.Trim + vbCrLf
        End If

        htmlOutput += "</td></tr></table>"

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Airport_Information: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput
  End Function

  Public Function Aircraft_APU(ByVal ac_id As Integer) As String
    Dim ac_apu_model_name As String = ""
    Dim ac_apu_tot_hrs As Integer
    Dim ac_apu_ser_no As String = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String : Query = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "SELECT * FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        adoRSAircraft.Read()

        If Not IsDBNull(adoRSAircraft("ac_apu_model_name")) Then
          ac_apu_model_name = adoRSAircraft.Item("ac_apu_model_name").ToString.Trim
        Else
          ac_apu_model_name = ""
        End If

        If Not IsDBNull(adoRSAircraft("ac_apu_tot_hrs")) Then
          ac_apu_tot_hrs = adoRSAircraft.Item("ac_apu_tot_hrs").ToString.Trim
        Else
          ac_apu_tot_hrs = 0
        End If

        If Not IsDBNull(adoRSAircraft("ac_apu_ser_no")) Then
          ac_apu_ser_no = adoRSAircraft.Item("ac_apu_ser_no").ToString.Trim
        Else
          ac_apu_ser_no = ""
        End If

        htmlOutput += "<tr><td height='5'></td></tr>"

        If ac_apu_model_name <> "" Or ac_apu_tot_hrs > 0 Or ac_apu_ser_no <> "" Then
          htmlOutput += "<tr><td colspan='2' class='header_text'>Auxiliary Power Unit (APU)</td></tr>"

          If ac_apu_model_name.Trim <> "" Then
            htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
            htmlOutput += "<td><font class='small_header_text'>Model: </font><font class='text_text'>" + ac_apu_model_name
          End If

          If ac_apu_ser_no <> "" Then
            If ac_apu_model_name.Trim = "" Then
              htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
              htmlOutput += "<td nowrap>"
            Else
              htmlOutput += ", "
            End If
            htmlOutput += "Serial #:&nbsp;"
            htmlOutput += "</font><font class='text_text'>" + adoRSAircraft.Item("ac_apu_ser_no").ToString.Trim + "</td></tr>"
          End If

          If ac_apu_model_name.Trim <> "" Then
            If ac_apu_tot_hrs > 0 Then
              htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
              htmlOutput += "<td nowrap><font class='small_header_text'>Total Time (Hours) Since New: </font><font class='text_text'>" + FormatNumber(CDbl(ac_apu_tot_hrs), 0, True, False, True) + "</td></tr>"
            End If
          End If
        End If

        If Not IsDBNull(adoRSAircraft("ac_apu_soh_hrs")) Then
          htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
          htmlOutput += "<td nowrap><font class='small_header_text'>Since Overhaul (SOH) Hours:&nbsp;"
          htmlOutput += "</font><font class='text_text'>" + FormatNumber(CDbl(adoRSAircraft.Item("ac_apu_soh_hrs").ToString.Trim), 0, True, False, True) + "&nbsp;</td></tr>"
        End If

        htmlOutput += "</tr>" + vbCrLf

        If Not IsDBNull(adoRSAircraft("ac_apu_shi_hrs")) Then
          htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
          htmlOutput += "<td nowrap><font class='small_header_text'>Since Hot Inspection (SHI) Hours:&nbsp;"


          htmlOutput += "</font><font class='text_text'>" + FormatNumber(CDbl(adoRSAircraft.Item("ac_apu_shi_hrs").ToString.Trim), 0, True, False, True) + "&nbsp;</td>"
          htmlOutput += "</tr>" + vbCrLf
        End If

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Aircraft_APU: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Aircraft_Contacts(ByVal ac_id As Integer) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim column_color As String = "white"
    Dim contact_counter As Integer = 1
    Dim sub_counter As Integer = 1
    Dim Query As String = ""
    Dim htmlOutput As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "select comp_id, actype_name, cref_owner_percent, comp_name, comp_name_alt, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country,"
      Query = Query & " comp_email_address, comp_web_address, contact_id, contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title,"
      Query = Query & " contact_email_address from aircraft_reference inner join company on cref_comp_id = comp_id and cref_journ_id = comp_journ_id inner join aircraft_contact_type on cref_contact_type = actype_code"
      Query = Query & " left outer join contact on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_hide_flag='N'"
      Query = Query & " where(cref_ac_id = '" & ac_id & "') and cref_journ_id = 0 "

      If Me.EB.Checked Then
        Query = Query & " and cref_contact_type <> '99' "
      End If

      Query = Query & " and cref_contact_type <> '71' "
      Query = Query & " order by cref_transmit_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        Do While adoRSAircraft.Read

          If (column_color = "white") Then
            If Me.WD.SelectedValue.ToString = "Word" Then
              htmlOutput += "<tr valign='top'><td width='60%' height='700' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
            Else
              htmlOutput += "<tr valign='top'><td width='60%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
            End If

          Else
            If Me.WD.SelectedValue.ToString = "Word" Then
              htmlOutput += "<tr valign='top' height='70'><td width='60%' height='700' bgcolor='#E6E6E6' class='text_text' cellspacing='0' cellpadding='5'><b>"
            Else
              htmlOutput += "<tr valign='top'><td width='60%' bgcolor='#E6E6E6' class='text_text' cellspacing='0' cellpadding='5'><b>"
            End If
          End If


          If Not IsDBNull(adoRSAircraft("actype_name")) Then
            htmlOutput += adoRSAircraft.Item("actype_name").ToString.Trim
          End If

          If Not IsDBNull(adoRSAircraft("cref_owner_percent")) Then
            If CLng(adoRSAircraft.Item("cref_owner_percent").ToString.Trim) > 0 And CLng(adoRSAircraft.Item("cref_owner_percent").ToString.Trim) < 100 Then
              htmlOutput += " [" & adoRSAircraft.Item("cref_owner_percent").ToString.Trim & "]"
            End If
          End If

          If Not IsDBNull(adoRSAircraft("comp_name")) Then
            htmlOutput += " - " & adoRSAircraft.Item("comp_name") & " "
          End If

          If Not IsDBNull(adoRSAircraft("comp_name_alt")) Then
            htmlOutput += adoRSAircraft.Item("comp_name_alt").ToString.Trim & "</b><br>"
          Else
            htmlOutput += "</b><br>"
          End If

          If Not IsDBNull(adoRSAircraft("comp_address1")) Then
            htmlOutput += adoRSAircraft.Item("comp_address1").ToString.Trim & "<br>"
          End If

          If Not IsDBNull(adoRSAircraft("comp_city")) Then
            htmlOutput += adoRSAircraft.Item("comp_city").ToString.Trim
          End If

          If Not IsDBNull(adoRSAircraft("comp_state")) Then
            htmlOutput += ", " & adoRSAircraft.Item("comp_state").ToString.Trim & " "
          End If

          If Not IsDBNull(adoRSAircraft("comp_zip_code")) Then
            htmlOutput += adoRSAircraft.Item("comp_zip_code").ToString.Trim & " "
          End If

          If Not IsDBNull(adoRSAircraft("comp_country")) Then
            htmlOutput += adoRSAircraft.Item("comp_country").ToString.Trim & "<br>"
          Else
            htmlOutput += "<br>"
          End If

          If Not IsDBNull(adoRSAircraft("comp_email_address")) Then
            If adoRSAircraft.Item("comp_email_address").ToString.Trim.Length > 0 Then
              htmlOutput += "<u>" & adoRSAircraft.Item("comp_email_address").ToString.Trim & "</u><br />"
            End If
          End If

          If Not IsDBNull(adoRSAircraft("comp_web_address")) Then
            If adoRSAircraft.Item("comp_web_address").ToString.Trim.Length > 0 Then
              htmlOutput += "<u>" & adoRSAircraft.Item("comp_web_address").ToString.Trim & "</u><br />"
            End If
          End If

          If Not IsDBNull(adoRSAircraft("comp_id")) Then
            htmlOutput += Phone_Company_Contact_Info(adoRSAircraft.Item("comp_id").ToString.Trim, 0)
          End If

          If (column_color = "white") Then
            If Me.WD.SelectedValue.ToString = "Word" Then
              htmlOutput += "&nbsp;</td><td width='40%' bgcolor='#E6E6E6' valign='top' height='70' class='text_text' cellspacing='0' cellpadding='5'><b>"
            Else
              htmlOutput += "&nbsp;</td><td width='40%' bgcolor='#E6E6E6' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
            End If

            column_color = "other"
          Else
            If Me.WD.SelectedValue.ToString = "Word" Then
              htmlOutput += "&nbsp;</td><td width='40%' bgcolor='white' height='70' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
            Else
              htmlOutput += "&nbsp;</td><td width='40%' bgcolor='white' valign='top' class='text_text' cellspacing='0' cellpadding='5'><b>"
            End If
            column_color = "white"
          End If

          If Not IsDBNull(adoRSAircraft("contact_sirname")) Then
            htmlOutput += adoRSAircraft.Item("contact_sirname").ToString.Trim & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_first_name")) Then
            htmlOutput += adoRSAircraft.Item("contact_first_name").ToString.Trim & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_middle_initial")) Then
            htmlOutput += adoRSAircraft.Item("contact_middle_initial").ToString.Trim & " "
          End If

          If Not IsDBNull(adoRSAircraft("contact_last_name")) Then
            htmlOutput += adoRSAircraft.Item("contact_last_name").ToString.Trim & "</b> "
          End If

          If Not IsDBNull(adoRSAircraft("contact_suffix")) Then
            htmlOutput += "<b>" & adoRSAircraft.Item("contact_suffix").ToString.Trim & "</b><br />"
          End If

          If Not IsDBNull(adoRSAircraft("contact_title")) Then
            htmlOutput += adoRSAircraft.Item("contact_title").ToString.Trim & "<br />"
          End If

          If Not IsDBNull(adoRSAircraft("contact_email_address")) Then
            htmlOutput += "<u>" & adoRSAircraft.Item("contact_email_address").ToString.Trim & "</u><br />"
          End If

          If Not IsDBNull(adoRSAircraft("comp_id")) And Not IsDBNull(adoRSAircraft("contact_id")) Then
            htmlOutput += Phone_Company_Contact_Info(CLng(adoRSAircraft.Item("comp_id").ToString), CLng(adoRSAircraft.Item("contact_id").ToString))
          End If

          htmlOutput += "</td></tr>"
          contact_counter = contact_counter + 1
        Loop

      End If

      If Me.WD.SelectedValue.ToString = "Word" Then
        If contact_counter < 10 Then
          sub_counter = 10 - contact_counter
          Do While sub_counter > 0
            htmlOutput += "<tr><td height='70'>&nbsp;</td></tr>"
            sub_counter = sub_counter - 1
          Loop
        End If
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Aircraft_Contacts: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Build_PDF_Pictures(ByVal ac_id As Integer) As String
    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Dim outString As String = ""

    ' This is right side column, should start with an open table already in td
    Dim imgFolder As String = HttpContext.Current.Server.MapPath(Session.Item("AircraftPicturesFolderVirtualPath"))
    Dim imgDisplayFolder As String = Application.Item("webHostObject").crmClientFullHostName + Session.Item("AircraftPicturesFolderVirtualPath")
    Dim imgFileName As String = ""
    Dim fApicSubject As String = ""

    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Integer = 0
    Dim pic_seq_num As Integer = 0

    Dim pic_size As Integer = 310

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String : Query = ""
    Dim skip_first As Integer = 0
    Dim something_shown As Boolean = False

    Try
      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      ' start AC_Pic
      Query = "SELECT TOP 2 * FROM Aircraft_Pictures WITH(NOLOCK)"
      Query = Query & " WHERE acpic_ac_id = " + ac_id.ToString
      Query = Query & " AND acpic_journ_id = '0'"
      Query = Query & " AND acpic_seq_no > '0'"
      Query = Query & " AND acpic_image_type = 'JPG'"
      Query = Query & " AND acpic_hide_flag = 'N'"
      Query = Query & " ORDER BY acpic_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then
        Do While adoRSAircraft.Read()
          If skip_first > 0 Then

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

            imgFileName = ac_id.ToString + crmWebClient.Constants.cHyphen + "0" + crmWebClient.Constants.cHyphen + fAcpic_id.ToString + crmWebClient.Constants.cDot + fAcpic_image_type.ToLower.Trim

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' setup the path for the pictures based on which site is running
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Me.WD.SelectedValue.ToString = "Word" Then
              pic_size = 250
            End If

            If Not String.IsNullOrEmpty(imgFileName) Then
              If Me.WD.SelectedValue.ToString = "Word" Then
                If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
                  outString += "<img Title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' height='200' width='" & pic_size & "' /><br /><br />"
                  something_shown = True
                End If
              Else
                If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
                  outString += "<img Title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" & pic_size & "' /><br /><br />"
                  something_shown = True
                End If
              End If
            End If

          Else
            skip_first = 1
          End If
        Loop
      Else
        imgFileName = ""
      End If

      If Not something_shown Then
        outString += " No Pictures Available "
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Pictures: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return outString

  End Function

  Public Function Build_PDF_First_Picture(ByVal ac_id As Integer) As String

    Dim outString As String = ""

    ' This is right side column, should start with an open table already in td
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Integer = 0
    Dim pic_seq_num As Integer = 0
    Dim something_shown As Boolean = False
    Dim pic_size As Integer = 310

    Dim fApicSubject As String = ""
    Dim imgFolder As String = HttpContext.Current.Server.MapPath(Session.Item("AircraftPicturesFolderVirtualPath"))
    Dim imgDisplayFolder As String = Application.Item("webHostObject").crmClientFullHostName + Session.Item("AircraftPicturesFolderVirtualPath")
    Dim imgFileName As String = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      ' start AC_Pic
      Query = "SELECT TOP 1 * FROM Aircraft_Pictures WITH(NOLOCK)"
      Query = Query & " WHERE acpic_ac_id = " + ac_id.ToString
      Query = Query & " AND acpic_journ_id = '0'"
      Query = Query & " AND acpic_seq_no > '0'"
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

        imgFileName = ac_id.ToString + crmWebClient.Constants.cHyphen + "0" + crmWebClient.Constants.cHyphen + fAcpic_id.ToString + crmWebClient.Constants.cDot + fAcpic_image_type.ToLower.Trim

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' setup the path for the pictures based on which site is running
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Me.WD.SelectedValue.ToString = "Word" Then
          pic_size = 250
        End If

        If Not String.IsNullOrEmpty(imgFileName) Then
          If Me.WD.SelectedValue.ToString = "Word" Then
            If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
              outString += "<img Title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' height='200' width='" & pic_size & "' /><br />&nbsp;<br />"
              something_shown = True
            End If
          Else
            If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
              outString += "<img Title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" & pic_size & "' /><br />&nbsp;<br />"
              something_shown = True
            End If
          End If
        End If

      Else
        imgFileName = ""
      End If

      If Not something_shown Then
        outString += "No Pictures Available"
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_First_Picture: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return outString

  End Function

  Public Function Build_PDF_Pictures_Page_Pics(ByVal ac_id As Integer) As String

    'LAST SECTION  IN COL 1- PICTURE -----------------------------------------------------------------------------------------------------------------------------------
    Dim outString As String = ""

    Dim imgFolder As String = HttpContext.Current.Server.MapPath(Session.Item("AircraftPicturesFolderVirtualPath"))
    Dim imgDisplayFolder As String = Application.Item("webHostObject").crmClientFullHostName + Session.Item("AircraftPicturesFolderVirtualPath")
    Dim imgFileName As String = ""

    ' This is right side column, should start with an open table already in td

    Dim fApicSubject As String = ""
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As Integer = 0
    Dim pic_seq_num As Integer = 0
    Dim row_color As String = "gray"
    Dim pic_counter As Integer = 1
    Dim pic_size As Integer = 250
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String : Query = ""
    Dim skip_first As Integer = 0
    Dim something_shown As Boolean = False

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      ' start AC_Pic
      Query = "SELECT * FROM Aircraft_Pictures WITH(NOLOCK)"   ' 1 on title - 3 on next - 6 on this
      Query = Query & " WHERE acpic_ac_id = " + ac_id.ToString
      Query = Query & " AND acpic_journ_id = '0'"
      Query = Query & " AND acpic_seq_no > '0'"
      Query = Query & " AND acpic_image_type = 'JPG'"
      Query = Query & " AND acpic_hide_flag = 'N'"
      Query = Query & " ORDER BY acpic_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        Do While adoRSAircraft.Read

          If skip_first > 1 Then

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

            imgFileName = ac_id.ToString + crmWebClient.Constants.cHyphen + "0" + crmWebClient.Constants.cHyphen + fAcpic_id.ToString + crmWebClient.Constants.cDot + fAcpic_image_type.ToLower.Trim

            If Me.WD.SelectedValue.ToString = "Word" Then
              outString += "<tr valign='top' bgcolor='F1F1F1'><td valign='top' class='text_text' cellspacing='0'>"
            Else
              If pic_counter = 1 Then
                If (row_color = "white") Then
                  outString += "<tr valign='top' bgcolor='F1F1F1'><td width='33%' valign='top' class='text_text' cellspacing='0'>"
                  row_color = "Gray"
                Else
                  outString += "<tr valign='top' bgcolor='#D0D0D0'><td width='33%' class='text_text' cellspacing='0'>"
                  row_color = "white"
                End If
              End If

            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' setup the path for the pictures based on which site is running
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Not String.IsNullOrEmpty(imgFileName) Then
              If Me.WD.SelectedValue.ToString = "Word" Then
                If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
                  outString += "<img Title='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' height='200' width='" & pic_size & "' /><br />&nbsp;<br />"
                  something_shown = True
                End If
              Else
                If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
                  outString += "<img Title='" + fApicSubject.Trim + "' alt='" + fApicSubject.Trim + "' src='" + imgDisplayFolder.Trim + "/" + imgFileName.Trim + "' width='" & pic_size & "' /><br />&nbsp;<br />"
                  something_shown = True
                End If
              End If
            End If

            skip_first = skip_first + 1

            If Me.WD.SelectedValue.ToString = "Word" Then
              outString += "</td></tr>"
            Else

              If pic_counter < 3 Then
                outString += "</td><td>"
                pic_counter = pic_counter + 1
              Else
                outString += "</td></tr>"
                pic_counter = 1
              End If
            End If

          Else
            skip_first = skip_first + 1
          End If

        Loop

        If pic_counter = 2 Then
          outString += "&nbsp;</td><td>&nbsp;</td></tr>"
        ElseIf pic_counter = 3 Then
          outString += "&nbsp;</td></tr>"
        End If

      Else
        imgFileName = ""
      End If

      If something_shown = 0 Then
        outString += " No Additional Pictures Currently Available "
      End If

      outString += "</table>"

      adoRSAircraft.Close()

    Catch SqlException

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Pictures_Page_Pics: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return outString

  End Function

  Public Function Build_PDF_Header(ByVal Title As String, ByVal address_info As String) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim company_name As String = ""
    Dim Query As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 
      Query = "SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 WHERE(Subscription.sub_id = " + Session.Item("localSubscription").evoSubID.ToString.Trim + ")"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()
      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        company_name = adoRSAircraft.Item("comp_name")
      End If

      adoRSAircraft.Close()

      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "<table cellspacing='0' cellpadding='0' width='650'><tr bgcolor='#736F6E'><td colspan='3'  cellpadding='0' cellspacing='0'>"
        htmlOutput += "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50'><tr><td>"
        htmlOutput += "<table width='650'><tr><td width='650' valign='top' class='white_feat_header_text'><font color='white' size='-1'>"
      Else
        htmlOutput += "<table cellspacing='0' cellpadding='0' width='100%'><tr bgcolor='#736F6E'><td colspan='3'  cellpadding='0' cellspacing='0'>"
        htmlOutput += "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr><td>"
        htmlOutput += "<table width='100%'><tr><td width='650'><font color='white' size='+1'>"
      End If

      htmlOutput += company_name & "</font><br>"

      If address_info <> "" Then
        htmlOutput += "<table>" & address_info
        htmlOutput += "</table>"
      End If

      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "</td><td width='40%' cellpadding='5' valign='top' class='white_feat_header_text'><font color='white' size='-1'>"
      Else
        htmlOutput += "</td><td width='40%' cellpadding='5' valign='top'><font color='white'>"
      End If

      Query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = '0' and ac_id = " & ac_id

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()
      adoRSAircraft.Read()

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        htmlOutput += adoRSAircraft.Item("amod_make_name") & " "
      End If

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        htmlOutput += adoRSAircraft.Item("amod_model_name")
      End If

      If Not BR.Checked Then
        If Not IsDBNull(adoRSAircraft("ac_ser_no_full")) Then
          htmlOutput += " SN # " & adoRSAircraft.Item("ac_ser_no_full").ToString & "</font>"
        End If
      End If

      'serial number

      htmlOutput += "</font>"

      htmlOutput += "<table align='left' valign='bottom'>"
      If address_info <> "" Then
        htmlOutput += "<tr><td align='center'>&nbsp;</td></tr>"
      End If
      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "<tr><td align='center' class='white_feat_header_text'><font color='white' size='-1'><i>" & Title & "</i></font></td></tr></table>"
      Else
        htmlOutput += "<tr><td align='center'><font color='white'><i>" & Title & "</i></font></td></tr></table>"
      End If

      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='400' valign='top'  height='500' cellpadding='0' cellspacing='0'><table valign='top'  cellpadding='0' cellspacing='0'>"
      Else
        htmlOutput += "</td></tr></table></td></tr></table></td></tr><tr valign='top'><td width='60%' height='900' valign='top'  cellpadding='0' cellspacing='0'><table valign='top'  cellpadding='0' cellspacing='0'>"
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Header: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Build_PDF_Header2(ByVal Title As String) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim company_name As String = ""
    Dim Query As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

      Query = "SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 WHERE(Subscription.sub_id = " + Session.Item("localSubscription").evoSubID.ToString.Trim + ")"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()
      adoRSAircraft.Read()

      If adoRSAircraft.HasRows Then
        company_name = adoRSAircraft.Item("comp_name")
      End If

      adoRSAircraft.Close()

      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "<table cellspacing='0' cellpadding='0' width='650'><tr bgcolor='#736F6E'><td colspan='3' valign='top'>"
        htmlOutput += "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr valign='top'><td valign='top'>"
        htmlOutput += "<table valign='top' width='650'><tr valign='top'><td width='650' valign='top' class='white_feat_header_text'><font color='white' size='-1'>"
      Else
        htmlOutput += "<table cellspacing='0' cellpadding='0' width='100%'><tr bgcolor='#736F6E'><td colspan='3' valign='top'>"
        htmlOutput += "<table border='1' cellspacing='0' cellpadding='4' bordercolor='black' width='100%' height='50' ><tr valign='top'><td valign='top'>"
        htmlOutput += "<table valign='top' width='100%'><tr valign='top'><td width='650' valign='top'><font color='white' size='+1'>"
      End If

      htmlOutput += company_name
      htmlOutput += "</font></td><td width='40%' cellpadding='5' class='white_feat_header_text'><font color='white' size='-1'>"

      Query = "SELECT amod_make_name, amod_model_name, ac_ser_no_full FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = '0' and ac_id = " & ac_id

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()
      adoRSAircraft.Read()

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        htmlOutput += adoRSAircraft.Item("amod_make_name") & " "
      End If

      If Not IsDBNull(adoRSAircraft("amod_model_name")) Then
        htmlOutput += adoRSAircraft.Item("amod_model_name")
      End If

      If Not BR.Checked Then
        If Not IsDBNull(adoRSAircraft("ac_ser_no_full")) Then
          htmlOutput += " SN # " & adoRSAircraft.Item("ac_ser_no_full") & "</font>"
        End If
      Else
        htmlOutput += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
      End If

      'serial number

      If Me.WD.SelectedValue = "Word" Then
        htmlOutput += "<table align='left' valign='bottom'><tr><td align='center' class='white_feat_header_text'><font color='white' size='-1'><i>" & Title & "</i></font></td></tr></table>"
        htmlOutput += "</font></td></tr></table></td></tr></table></td></tr><tr><td width='650' cellspacing='0'><table width='650' height='850' cellspacing='0' cellpadding='5'>"
      Else
        htmlOutput += "<table align='left' valign='bottom'><tr><td align='center'><font color='white'><i>" & Title & "</i></font></td></tr></table>"
        htmlOutput += "</font></td></tr></table></td></tr></table></td></tr><tr><td width='100%' cellspacing='0'><table width='100%' cellspacing='0' cellpadding='5'>"
      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_HEADER2: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Aircraft_Build_PDF_Features(ByVal ac_id As Integer) As String

    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim type_of_damage As String = ""
    Dim Query As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "SELECT Aircraft_Key_Feature.afeat_status_flag, Key_Feature.kfeat_name FROM Aircraft_Key_Feature INNER JOIN Key_Feature ON Aircraft_Key_Feature.afeat_feature_code = Key_Feature.kfeat_code "
      Query = Query & "WHERE (Aircraft_Key_Feature.afeat_ac_id = " & ac_id & ") AND (Aircraft_Key_Feature.afeat_journ_id = '0') ORDER BY Aircraft_Key_Feature.afeat_seq_no"

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then
        htmlOutput += "<table><tr><td class='white_feat_header_text' colspan='2'>Key Features</td></tr>"
        Do While adoRSAircraft.Read
          If adoRSAircraft.Item("afeat_status_flag").ToString = "Y" Then
            htmlOutput += "<tr valign='top'><td class='white_feat_text' valign='top'>"
            htmlOutput += "&#10003; "
            htmlOutput += "</td><td class='white_feat_text'>" & adoRSAircraft.Item("kfeat_name") & type_of_damage & "</td></tr>"
          Else
            type_of_damage = ""
          End If
        Loop

      End If

      htmlOutput += "</table>"

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Aircraft_Build_PDF_Features: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

  Function DisplayEngineInfo(ByVal ac_id As Integer) As String

    Dim xLoop, nloopCount
    Dim sAircraftType As String = ""
    Dim sAirframeType As String = ""
    Dim htmlOutput As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft2 As System.Data.SqlClient.SqlDataReader : adoRSAircraft2 = Nothing
    Dim type_of_damage As String = ""

    Dim Query2 As String : Query2 = ""
    Dim Query As String : Query = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      Query = "SELECT * FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id"
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        nloopCount = 0
        xLoop = 0
        adoRSAircraft.Read()

        If Not IsDBNull(adoRSAircraft("amod_airframe_type_code")) Then
          sAirframeType = adoRSAircraft.Item("amod_airframe_type_code").ToString.Trim.ToLower
        End If
        If Not IsDBNull(adoRSAircraft("amod_type_code")) Then
          sAircraftType = adoRSAircraft.Item("amod_type_code").ToString.Trim.ToLower
        End If

        htmlOutput += "<tr><td height='5'></td></tr>"

        htmlOutput += "<tr><td colspan='2' class='header_text'>Engine Information</th></tr>"
        htmlOutput += "<tr><td width='2%'>&nbsp;</td><td valign='middle'><font class='small_header_text'>Engine Model: "
        htmlOutput += "</font><font class='text_text'>" & adoRSAircraft.Item("ac_engine_name").ToString & "&nbsp;</font>"


        If Not IsDBNull(adoRSAircraft("ac_engine_tbo_oc_flag")) Then
          htmlOutput += "<font class='small_header_text'>, On&nbsp;Condition&nbsp;TBO: "
          If adoRSAircraft.Item("ac_engine_tbo_oc_flag").ToString.Trim.ToUpper = "Y" Then
            htmlOutput += "&nbsp;Yes</font></td>"
          Else
            htmlOutput += "&nbsp;No</font></td>"
          End If
        End If

        htmlOutput += "</tr>" & vbCrLf
        htmlOutput += "<tr><td width='2%'>&nbsp;</td><td valign='middle'  colspan='2' nowrap><font class='small_header_text'>Engine&nbsp;Maintenance&nbsp;Program:</font> "

        Query2 = "SELECT emp_name, emp_provider_name, emp_program_name FROM Engine_Maintenance_Program WITH(NOLOCK)"
        Query2 = Query2 & " WHERE emp_id = " & adoRSAircraft.Item("ac_engine_maintenance_prog_EMP").ToString

        SqlConn2.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
        SqlConn2.Open()
        SqlCommand2.Connection = SqlConn2
        SqlCommand2.CommandType = System.Data.CommandType.Text
        SqlCommand2.CommandTimeout = 60
        SqlCommand2.CommandText = Query2
        adoRSAircraft2 = SqlCommand2.ExecuteReader()

        If adoRSAircraft2.HasRows Then
          adoRSAircraft2.Read()
          htmlOutput += "<font class='text_text'>"
          htmlOutput += Trim(adoRSAircraft2.Item("emp_provider_name").ToString) & "&nbsp;-&nbsp;" & adoRSAircraft2.Item("emp_program_name").ToString & "&nbsp;</font>"
        End If
        adoRSAircraft2.Close()

        htmlOutput += "</td></tr>"

        If Me.WD.SelectedValue.ToString = "Word" Then
          htmlOutput += "<tr><td colspan='2'><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'><tr>"
          htmlOutput += "<tr><td class=Normal>&nbsp;</td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Serial Number</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Time Since New  Hours</font></td>" ' (TTSNEW)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Since Ovh Hours</font></td>" ' (SOH/SCOR)
          htmlOutput += "<th class='text_text'align='center'><font size='-2'>Since Hot Inspect Hours</font></td>" '(SHI/SMPI)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Time Between Ovh Hours</font></td>" '(TBO/TBCI)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since New</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Ovh</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Hot</font></td>"
          htmlOutput += "</tr>" & vbCrLf
        Else
          htmlOutput += "<tr><td colspan='2'><table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'><tr>"
          htmlOutput += "<tr><td class=Normal>&nbsp;</td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Serial Number</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Time Since New  Hours</font></td>" ' (TTSNEW)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Since Overhaul  Hours</font></td>" ' (SOH/SCOR)
          htmlOutput += "<th class='text_text'align='center'><font size='-2'>Since Hot Inspection  Hours</font></td>" '(SHI/SMPI)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Time Between Overhaul  Hours</font></td>" '(TBO/TBCI)
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since New</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Overhaul</font></td>"
          htmlOutput += "<th class='text_text' align='center'><font size='-2'>Total Cycles Since Hot</font></td>"
          htmlOutput += "</tr>" & vbCrLf
        End If

        If sAirframeType <> "R" Then
          nloopCount = 4
        Else
          nloopCount = 3
        End If

        For xLoop = 1 To nloopCount

          If (Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Or Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shs_cycles"))) Then

            If xLoop = 1 And sAirframeType <> "R" Then
              htmlOutput += "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 2 And sAirframeType <> "R" Then
              htmlOutput += "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            ElseIf xLoop = 3 And sAirframeType <> "R" Then
              htmlOutput += "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(L)&nbsp;</td>"
            ElseIf xLoop = 4 And sAirframeType <> "R" Then
              htmlOutput += "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;(R)&nbsp;</td>"
            Else
              htmlOutput += "<tr><td class='text_text' valign='middle' align='right' nowrap><font size='-2'>Eng&nbsp;" & CStr(xLoop) & "&nbsp;</td>"
            End If

            htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_ser_no").ToString & "</td>"


            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tot_hrs")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_tot_hrs").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_hrs")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_soh_hrs").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shi_hrs")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_shi_hrs").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_tbo_hrs")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_tbo_hrs").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_snew_cycles")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_snew_cycles").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_soh_cycles")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_soh_cycles").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("ac_engine_" & CStr(xLoop) & "_shs_cycles")) Then
              htmlOutput += "<td class='text_text' valign='middle' align='left'><font size='-2'>&nbsp;" & FormatNumber(CDbl(adoRSAircraft.Item("ac_engine_" & CStr(xLoop) & "_shs_cycles").ToString), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td class='text_text' valign='middle' align='left'>&nbsp;</td>"
            End If

            htmlOutput += "</tr>" & vbCrLf

          End If

        Next ' xLoop

        htmlOutput += "</table>" & vbCrLf
        htmlOutput += "</td></tr>"

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in DisplayEngineInfo: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      adoRSAircraft2.Close()
      adoRSAircraft2 = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()
      SqlConn2.Dispose()
      SqlCommand2.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()
      SqlCommand2.Dispose()
      SqlConn2.Close()
      SqlConn2.Dispose()

      adoRSAircraft = Nothing
      adoRSAircraft2 = Nothing

    End Try

    Return htmlOutput

  End Function

  Public Function Build_HTML_Page(ByVal viewToPDF As String) As String

    Return viewToPDF & "</body></html>"

  End Function

  Public Function Build_PDF_Template_Header() As String

    Dim sServerMapPath As String = ""
    Dim sSiteStyleSheet As String = "common\jetnet.css"
    sServerMapPath = HttpContext.Current.Server.MapPath(sSiteStyleSheet)
    Dim txtFile As New System.IO.StreamReader(sServerMapPath)
    Dim readStyle As String = ""
    Dim formatStyle As String = ""
    readStyle = "<style>"

    Try

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
        readStyle += formatStyle + vbCrLf
      Loop

      readStyle += ".break { page-break-before: always; }" + vbCrLf

      If Me.WD.SelectedValue.ToString = "Word" Then
        readStyle += ".header_text{font-family:Arial ;font-size: x-small; color: #736F6E; font-weight: bold;}" + vbCrLf
        readStyle += ".small_header_text{font-family:Arial ;font-size: xx-small; font-style: italic; color: #736F6E} " + vbCrLf
        readStyle += ".small_header_text2{font-family:Arial ;font-size: xx-small; font-style: italic; color: #736F6E; font-weight: bold;} " + vbCrLf
        readStyle += ".text_text{font-family:Arial;font-size: xx-small; color: #736F6E}" + vbCrLf
        readStyle += ".text_text2{font-family:Arial;font-size: xx-small; color: #736F6E}" + vbCrLf
        readStyle += ".white_feat_text{font-family:Arial;font-size: x-small; color: white}" + vbCrLf
        readStyle += ".white_feat_header_text{font-family:Arial;font-size: small; color: white; font-weight: bold;}" + vbCrLf
      Else
        readStyle += ".header_text{font-family:Arial ;font-size: medium; color: #736F6E; font-weight: bold;}" + vbCrLf
        readStyle += ".small_header_text{font-family:Arial ;font-size: smaller; font-style: italic; color: #736F6E} " + vbCrLf
        readStyle += ".small_header_text2{font-family:Arial ;font-size: x-small; font-style: italic; color: #736F6E; font-weight: bold;} " + vbCrLf
        readStyle += ".text_text{font-family:Arial;font-size: small; color: #736F6E}" + vbCrLf
        readStyle += ".text_text2{font-family:Arial;font-size: x-small; color: #736F6E}" + vbCrLf
        readStyle += ".white_feat_text{font-family:Arial;font-size: medium; color: white}" + vbCrLf
        readStyle += ".white_feat_header_text{font-family:Arial;font-size: large; color: white; font-weight: bold;}" + vbCrLf
      End If

      readStyle += ".table_specs{font-size:12px;}" + vbCrLf
      readStyle += "</style>" + vbCrLf

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Template_Header: " & ex.Message
    End Try

    Return "<html><head>" + vbCrLf + readStyle + "</head><body>" + vbCrLf

  End Function

  Public Function Build_PDF_Format() As String
    Dim sTmpStr As String = ""

    Try
      sTmpStr += "</table>"
      sTmpStr += "</td><td>&nbsp;&nbsp;&nbsp;"

      If Me.WD.SelectedValue.ToString = "Word" Then
        sTmpStr += "</td><td width='250' height='500' valign='top'>"
        sTmpStr += "<table bgcolor='#A4A4A4' height='850' width='250' valign='top'><tr height='850' valign='top'><td width='250' height='850' align='center'>"
      Else
        sTmpStr += "</td><td width='40%' height='900' valign='top'>"
        sTmpStr += "<table bgcolor='#A4A4A4' height='900' width='100%' valign='top'><tr height='900' valign='top'><td width='100%' height='900' align='center'>"
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Format: " & ex.Message
    End Try

    Return sTmpStr

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
    Dim sTmpStr As String = ""

    Try
      sTmpStr = "<table width='100%' align='center'><tr id='trMaintbl_Footer'><hr />"
      sTmpStr += "<td  id='tdMaintbl_Footer' align='center'>JETNET Evolution Model Market Summary Report"
      sTmpStr += "</td></tr></table>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_PDF_Template_Footer: " & ex.Message
    End Try

    Return sTmpStr

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

    Dim sTmpStr As String = ""
    Try
      sTmpStr = "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Insert_Page_Break: " & ex.Message
    End Try

    Return sTmpStr

  End Function

  Public Function Build_String_To_HTML(ByVal ViewToPDF As String) As Boolean

    Dim bResult As Boolean = False

    Try
      ' create a file to dump the PDF report to
      ' create a streamwriter variable
      Dim swPDF As System.IO.StreamWriter
      ' create the html file

      swPDF = IO.File.CreateText(HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + report_name)
      ' write to the file
      swPDF.WriteLine(ViewToPDF)
      'close the streamwriter
      swPDF.Close()
      ' call the webgrabber info
      Response.Write("Page:<br>" & ViewToPDF)

      bResult = True

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_String_To_HTML: " & ex.Message
    End Try

    Return bResult

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


  Public Function heli_details(ByVal nAircraftID, ByVal nAircraftJournalID)

    heli_details = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
    Dim Query As String = ""
    Dim htmlOutput As String = ""
    Dim bHasMainBlades As Boolean = False
    Dim bHasTailBlades As Boolean = False
    Dim sLabel As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60


      Query = "SELECT * FROM Helicopter_Detail_Times WITH(NOLOCK) WHERE heldt_ac_id = " & CStr(nAircraftID)
      Query = Query & " AND heldt_journ_id = " & CStr(nAircraftJournalID) & "  "

      SqlCommand.CommandText = Query
      adoRSAircraft = SqlCommand.ExecuteReader()

      If adoRSAircraft.HasRows Then

        htmlOutput = "<table>"
        htmlOutput += "<tr><td>&nbsp;</td></tr>"
        htmlOutput += "<tr><td width='2%'>&nbsp;</td>"
        htmlOutput += "<td valign='top'>"

        htmlOutput += "<table border='1' bordercolor='#949494' cellpadding='2' cellspacing='0'><tr>"
        htmlOutput += "<tr><th class='text_text'  colspan='8'><strong>GEARBOX/ROTOR BLADE INFORMATION</strong></td></tr>"
        htmlOutput += "<tr><td class='Norma'>&nbsp;</td><td class='Norma'>&nbsp;</td>"
        htmlOutput += "<th class='text_text' align='center'><font size='-2'>Serial Number</font></td>"
        htmlOutput += "<th class='text_text' align='center'><b>TTSN:</b></td>"
        htmlOutput += "<th class='text_text' align='center'><b>Time Remaining:</b></td>"
        htmlOutput += "<th class='text_text' align='center'><b>TSOH:</b></td></tr>"

        Do While adoRSAircraft.Read

          If Not IsDBNull(adoRSAircraft("heldt_category_type")) And Trim(adoRSAircraft("heldt_category_type")) <> "" Then
            Select Case UCase(Trim(adoRSAircraft("heldt_category_type")))
              Case "INTERMEDIATE GEARBOX"
                bHasMainBlades = False
                bHasTailBlades = False
                sLabel = "Intermediate&nbsp;Gearbox"
              Case "MAIN ROTOR #1 BLADES", "MAIN ROTOR #2 BLADES"
                ' Check for number of blades
                sLabel = "Main&nbsp;Blade&nbsp;"
                bHasMainBlades = True
                bHasTailBlades = False
                ' OK Get the Blade Number 1-10
                sLabel = sLabel & Right(Trim(adoRSAircraft("heldt_subcat_type")), 2)
              Case "MAIN ROTOR HUB #1", "MAIN ROTOR HUB #2"
                bHasMainBlades = False
                bHasTailBlades = False
                sLabel = "Main&nbsp;Rotor&nbsp;Hub"
              Case "MAIN TRANSMISSION #1", "MAIN TRANSMISSION #2"
                bHasMainBlades = False
                bHasTailBlades = False
                sLabel = "Main&nbsp;Transmission"
              Case "TAIL ROTOR BLADES"
                ' Check for number of blades
                sLabel = "Tail&nbsp;Blade&nbsp;"
                bHasMainBlades = False
                bHasTailBlades = True
                ' OK Get the Blade Number 1-10
                sLabel = sLabel & Right(Trim(adoRSAircraft("heldt_subcat_type")), 2)
              Case "TAIL ROTOR GEARBOX"
                bHasMainBlades = False
                bHasTailBlades = False
                sLabel = "Tail&nbsp;Rotor&nbsp;Gearbox"
              Case "TAIL ROTOR HUB"
                bHasMainBlades = False
                bHasTailBlades = False
                sLabel = "Tail&nbsp;Rotor&nbsp;Hub"
            End Select
          End If

          If Not IsDBNull(adoRSAircraft("heldt_ttsn")) Or Not IsDBNull(adoRSAircraft("heldt_remaining_hours")) Or Not IsDBNull(adoRSAircraft("heldt_soh")) Then
            htmlOutput += "<tr id='TR_Prop_Info'>" ' TR_Prop_Info
            htmlOutput += "<td nowrap>&nbsp;</td>"
            htmlOutput += "<th class='small_header_text'>&nbsp;" & sLabel & "&nbsp;&nbsp;</td>"

            If Not IsDBNull(adoRSAircraft("heldt_ser_no_full")) Then
              htmlOutput += "<td align='center' class='text_text'>" & adoRSAircraft("heldt_ser_no_full") & "</td>"
            Else
              htmlOutput += "<td>&nbsp;</td>"
            End If

            If Not IsDBNull(adoRSAircraft("heldt_ttsn")) Then
              htmlOutput += "<td align='center' class='text_text'>" & FormatNumber(CDbl(adoRSAircraft("heldt_ttsn")), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td>&nbsp;</td>"
            End If
            If Not IsDBNull(adoRSAircraft("heldt_remaining_hours")) Then
              htmlOutput += "<td align='center' class='text_text'>" & FormatNumber(CDbl(adoRSAircraft("heldt_remaining_hours")), 0, True, False, True) & "</td>"
            Else
              htmlOutput += "<td>&nbsp;</td>"
            End If
            If Not IsDBNull(adoRSAircraft("heldt_soh")) And (Not bHasMainBlades Or Not bHasTailBlades) Then
              htmlOutput += "<td align='center' class='text_text'>" & FormatNumber(CDbl(adoRSAircraft("heldt_soh")), 0, True, False, True) & "</td></tr>"
            Else
              htmlOutput += "<td>&nbsp;</td>"
            End If
            htmlOutput += "</tr>"
          End If

        Loop
        htmlOutput += "</table>" ' Prop_Information_Mini_Table

      End If

      adoRSAircraft.Close()

    Catch SqlException
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in heli_details: " & SqlException.Message

      adoRSAircraft.Close()
      adoRSAircraft = Nothing

      SqlConn.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

      adoRSAircraft = Nothing

    End Try

    Return htmlOutput

  End Function

End Class
