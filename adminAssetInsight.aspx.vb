' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminAssetInsight.aspx.vb $
'$$Author: Mike $
'$$Date: 11/18/19 11:33a $
'$$Modtime: 11/18/19 11:32a $
'$$Revision: 4 $
'$$Workfile: adminAssetInsight.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminAssetInsight
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Public Shared masterPage As New Object

  Private sTask As String = ""
  Private sResultsItem As String = ""
  Public current_count As Integer = 0
  Public overall_count As Integer = 0
  Public temp_text As String = ""


  Private Sub adminAssetInsight_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, HomebaseTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreInit): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    Try

      If Session.Item("crmUserLogon") <> True Then

        Response.Redirect("Default.aspx", True)

      Else

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
          masterPage.Set_Active_Tab(11)
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution eValues Dashboard")
        ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
          masterPage.Set_Active_Tab(10)
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase eValues Dashboard")
        End If

        If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                                  HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                                  CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                                  CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
          Response.Redirect("Default.aspx", True)
        End If

        If Not IsNothing(Request.Item("task")) Then
          If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
            sTask = Request.Item("task").ToString.ToUpper.Trim
          End If
        End If

        If Not IsNothing(Request.Item("item")) Then
          If Not String.IsNullOrEmpty(Request.Item("item").ToString.Trim) Then
            sResultsItem = Request.Item("item").ToString.ToUpper.Trim
          End If
        End If

        localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        If Trim(Request("run_probation")) = "Y" And Trim(Request("run_model")) = "Y" Then

          If Trim(Request("mfr_year_start")) <> "" And Trim(Request("mfr_year_end")) <> "0" Then
            Call Run_Model_Evalues_Probation(Trim(Request("amod_make_name")), Trim(Request("amod_model_name")), Trim(Request("mfr_year_start")), Trim(Request("mfr_year_end")))
          Else
            Call Run_Model_Evalues_Probation(Trim(Request("amod_make_name")), Trim(Request("amod_model_name")), 0, 0)
          End If


        ElseIf Trim(Request("run_probation")) = "Y" Or Trim(Request("run_world")) = "Y" Or Trim(Request("run_all")) = "Y" Or Trim(Request("run_percent")) = "Y" Or Trim(Request("run_backwards")) = "Y" Then

          If Trim(Request("run_world")) = "Y" Then
            If Trim(Request("run_all")) = "Y" Then
              temp_text = "Running..."
              Response.Write(temp_text)
              probation_label.Text = temp_text
              Response.Flush()

              current_count = 0
              Call Run_Evalues_Probation(Trim(Request("amod_id")), True, False, False)
              temp_text = "<br>Standard Deviation Run: " & current_count
              Response.Write(temp_text)
              probation_label.Text &= temp_text
              overall_count = overall_count + current_count
              Response.Flush()
              Response.Flush()

              current_count = 0
              Call Run_Evalues_Probation(Trim(Request("amod_id")), True, True, False)
              temp_text = "<br>Backwards Run: " & current_count
              Response.Write(temp_text)
              probation_label.Text &= temp_text
              overall_count = overall_count + current_count
              Response.Flush()
              Response.Flush()

              current_count = 0
              Call Run_Evalues_Probation(Trim(Request("amod_id")), True, False, True)
              temp_text = "<br>Model Percent Run: " & current_count
              Response.Write(temp_text)
              probation_label.Text &= temp_text
              overall_count = overall_count + current_count
              Response.Flush()
              Response.Flush()

              Call Run_Evalues_Probation_Negative()
              temp_text = "<br>Negative EValues: " & current_count
              Response.Write(temp_text)
              probation_label.Text &= temp_text
              overall_count = overall_count + current_count
              Response.Flush()
              Response.Flush()

            Else
              Call Run_Evalues_Probation(Trim(Request("amod_id")), False, False, False)
              Call Run_Evalues_Probation(Trim(Request("amod_id")), False, True, False)
              Call Run_Evalues_Probation(Trim(Request("amod_id")), False, False, True)
            End If
          ElseIf Trim(Request("run_probation")) = "Y" Then
            If Trim(Request("run_percent")) = "Y" Then
              If Trim(Request("run_all")) = "Y" Then
                Call Run_Evalues_Probation(Trim(Request("amod_id")), True, False, True)
              Else
                Call Run_Evalues_Probation(Trim(Request("amod_id")), False, False, True)
              End If
            ElseIf Trim(Request("run_backwards")) = "Y" Then
              If Trim(Request("run_all")) = "Y" Then
                Call Run_Evalues_Probation(Trim(Request("amod_id")), True, True, False)
              Else
                Call Run_Evalues_Probation(Trim(Request("amod_id")), False, True, False)
              End If
            ElseIf Trim(Request("run_all")) = "Y" Then
              Call Run_Evalues_Probation(Trim(Request("amod_id")), True, False, False)
            Else
              Call Run_Evalues_Probation(Trim(Request("amod_id")), False, False, False)
            End If
          End If

          temp_text = "<br><br>Overall Count: " & overall_count
          probation_label.Text &= temp_text
          overall_count = overall_count + current_count
          Response.Flush()
          Response.Flush()


          probation_label.Text &= "<br><br><a href='adminAssetInsight.aspx?run_probation=Y&run_world=Y&amod_id=0&run_all=Y'>Run Probation Routine</a>"

        Else

          If sTask.ToUpper.Contains("RESULTS") Then

            Select Case sResultsItem

              Case "LATESTESTIMATES"
                localDatalayer.display_latest_estimates_table(acSearchResultsTable_tabPanel0.Text)
                evalue_tabContainer.Visible = True
                evalue_tabPanel0_Label1.Text = "Latest Estimates (top 500)"
              Case "ESTIMATESINQUEUE"
                localDatalayer.display_estimates_in_queue_table(acSearchResultsTable_tabPanel0.Text)
                evalue_tabContainer.Visible = True
                evalue_tabPanel0_Label1.Text = "Estimates in Queue"
              Case "MODELSNOTMAPPED"
                localDatalayer.display_models_not_mapped_table(acSearchResultsTable_tabPanel0.Text)
                evalue_tabContainer.Visible = True
                evalue_tabPanel0_Label1.Text = "Models not Mapped"
              Case "AIRCRAFTNOTMAPPED"
                localDatalayer.display_aircraft_not_mapped_table(acSearchResultsTable_tabPanel0.Text)
                evalue_tabContainer.Visible = True
                evalue_tabPanel0_Label1.Text = "Aircraft not Mapped"
              Case "AIRCRAFTONPROBATION"
                localDatalayer.display_aircraft_on_probation_table(acSearchResultsTable_tabPanel0.Text)
                evalue_tabContainer.Visible = True
                evalue_tabPanel0_Label1.Text = "Aircraft on Probation"
              Case Else
                evalue_tabContainer.Visible = False
                evalue_tabPanel0_Label1.Text = "No Results"

            End Select

          End If

          ' load evalues Panels

          Dim eValuesSummaryHtml As String = ""

          localDatalayer.display_asset_insight_summary_table(eValuesSummaryHtml)
          evalue_summary_table.Text = eValuesSummaryHtml

          Dim eValuesProcessingHtml As String = ""

          localDatalayer.display_asset_insight_processing_table(eValuesProcessingHtml)
          evalue_processing_table.Text = eValuesProcessingHtml

        End If

      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_Load): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_Load): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Private Sub adminAssetInsight_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    Try

      Dim JavascriptOnLoad As String = ""

      If sTask.ToUpper.Contains("RESULTS") Then

        Select Case sResultsItem

          Case "latestEstimates"
            '    JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
          Case "estimatesInQueue"
            '    JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
          Case "modelsNotMapped"
            '    JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
          Case "aircraftNotMapped"
            '    JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
          Case "aircraftOnProbation"
            '    JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"

        End Select

        JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"

      End If


      JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"

      If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me.evalue_tabContainer, Me.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreRender): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub


  '  -- ASSETT INSIGHT FUNCTIONS


  Public Function GetStandardDeviation(ByVal evalues_array() As Long, ByVal array_count As Integer, ByRef high_number As Long, ByRef low_number As Long, ByRef past_low_to_use As Long, ByRef run_backwards As Boolean) As Long
    GetStandardDeviation = 0

    Dim standardDeviation As Double = 0
    Dim standardDeviation_low_future As Double = 0
    '  Dim enumerable(100) As Long
    Dim array_of_within_1sd(100) As Long
    Dim size_of_array As Integer = 0
    Dim avg As Double = 0
    Dim sum As Double = 0
    Dim i As Integer = 0
    'Dim num_found As Integer = 0

    Try

      'enumerable(0) = 111
      'enumerable(1) = 222
      'enumerable(2) = 88
      'enumerable(3) = 177
      'enumerable(4) = 123
      'enumerable(5) = 155
      'enumerable(6) = 144
      'enumerable(7) = 188
      'enumerable(8) = 6

      'enumerable(0) = 10133
      'enumerable(1) = 2541
      'enumerable(2) = 7492
      'enumerable(3) = 10953
      'enumerable(4) = 7606
      'enumerable(5) = 9015
      'enumerable(6) = 7953
      'enumerable(7) = 9032
      'enumerable(8) = 10159
      'enumerable(9) = 9289


      'enumerable(0) = 680
      'enumerable(1) = 876
      'enumerable(2) = 784
      'enumerable(3) = 941
      'enumerable(4) = 800
      'enumerable(5) = 810

      ReDim Preserve evalues_array(array_count)

      size_of_array = UBound(evalues_array)

      If (size_of_array > 1) Then
        avg = evalues_array.Average

        For i = 0 To size_of_array - 1
          sum += ((evalues_array(i) - avg) * (evalues_array(i) - avg))
        Next
        '  sum = enumerable.Sum(d >= (d - avg) * (d - avg))

        standardDeviation = Math.Sqrt(sum / size_of_array)

        If run_backwards = True Then
          past_low_to_use = (avg * 0.3)
        Else
          standardDeviation_low_future = (standardDeviation * 2)
          past_low_to_use = avg - standardDeviation_low_future

          ' example , average 23 million
          ' standard dev is 1.1 mill, so 2 standard dev is 2.2 mill, so 23 mill minus 2.2 = 20.8 million 
          ' if u take 23 million times 80 percent = 18,400,000
          ' if the standard deviation number (20.8) is greater than the 80 percent of the average (18,400,000) 
          ' then our market is too tight to mess with pervious, so clear the number
          If past_low_to_use > (avg * 0.8) Then
            ' if we consider it too tight, then reduce the next year cutting down to 30% of the average 
            past_low_to_use = (avg * 0.4)
          Else
            past_low_to_use = past_low_to_use
          End If

          ' only go 2 down for future ones 
        End If





        'if 1 standard dev low is greater than 90 percent of the average then we are too tight 
        If (avg - standardDeviation) > (avg * 0.9) Then
          low_number = 0
          high_number = 0
        Else
          standardDeviation = (standardDeviation * 2.5)  ' go 2.5 for current 
          high_number = avg + standardDeviation
          low_number = avg - standardDeviation
        End If












        'For i = 0 To size_of_array - 1
        '  If enumerable(i) >= low_number And enumerable(i) <= high_number Then
        '    array_of_within_1sd(num_found) = enumerable(i)
        '    num_found += 1
        '  End If
        'Next

        'ReDim Preserve array_of_within_1sd(num_found - 1)

        'avg = array_of_within_1sd.Average

        'sum = 0
        'For i = 0 To num_found - 1
        '  sum += ((array_of_within_1sd(i) - avg) * (array_of_within_1sd(i) - avg))
        'Next

        'standardDeviation = Math.Sqrt(sum / size_of_array)


      End If

    Catch ex As Exception

    End Try




  End Function

  Public Sub Run_Evalues_Probation_Negative()
    Dim temp_table As New DataTable
    Dim ac_id As Long = 0
    Dim temp_make_name As String = ""
    Dim temp_model_name As String = ""
    Dim temp_reason As String = ""
    Dim temp_ser_no As String = ""
    Dim temp_year As Integer = 0
    Dim avg_evalue As Long = 0
    Dim temp_evalue As Long = 0
    Dim amod_id As Long = 0


    temp_table = GET_NEGATIVE_EVALUES()


    If Not IsNothing(temp_table) Then
      If temp_table.Rows.Count > 0 Then
        For Each k As DataRow In temp_table.Rows

          temp_evalue = k.Item("afmv_value")
          temp_make_name = k.Item("amod_make_name")
          temp_model_name = k.Item("amod_model_name")
          temp_ser_no = k.Item("ac_ser_no_full")
          amod_id = k.Item("amod_id")

          ac_id = k.Item("afmv_ac_id")
          temp_reason = "Error: Evalue Less Than 0 "
          Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, 0, 0, avg_evalue, temp_reason, amod_id)



        Next
      End If
    End If

  End Sub

  Public Sub Run_Model_Evalues_Probation(ByVal make_name As String, ByVal model_name As String, ByVal mfr_year_start As Integer, ByVal mfr_year_end As Integer)

    Dim evalues_table As New DataTable
    Dim year_table As New DataTable
    Dim evalues_array(500) As Long
    Dim evalues_count As Integer = 0
    Dim high_number As Long = 0
    Dim low_number As Long = 0
    Dim temp_evalue As Long = 0
    Dim amod_id As Long = 207 ' 207 king air b200 , 1200 citation x+ , 288 g-450 
    Dim temp_year As Integer = 0
    Dim i As Integer = 0
    Dim last_low_value As Long = 0
    Dim ac_id As Long = 0
    Dim temp_make_name As String = ""
    Dim temp_model_name As String = ""
    Dim temp_reason As String = ""
    Dim temp_ser_no As String = ""
    Dim avg_evalue As Double = 0
    Dim past_low_to_use As Long = 0
    Dim last_amod_id As Long = 0


    year_table = Get_DLV_Year_Evalues_For_Model_Mfr_Year(make_name, model_name, mfr_year_start, mfr_year_end)


    If Not IsNothing(year_table) Then
      If year_table.Rows.Count > 0 Then
        For Each k As DataRow In year_table.Rows

          temp_year = k.Item("ac_year")
          temp_make_name = k.Item("amod_make_name")
          temp_model_name = k.Item("amod_model_name")
          temp_ser_no = k.Item("ac_ser_no_full")
          temp_evalue = k.Item("afmv_value")
          amod_id = k.Item("amod_id")
          ac_id = k.Item("ac_id")

          Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, 0, 0, 0, "AUTOMATED: Model/Year Probation", amod_id)

        Next
      End If
    End If

  End Sub

  Public Sub Run_Evalues_Probation(ByVal amod_id_passed As String, ByVal run_all As Boolean, ByVal run_backwards As Boolean, ByVal run_percent As Boolean)

    Dim evalues_table As New DataTable
    Dim year_table As New DataTable
    Dim evalues_array(500) As Long
    Dim evalues_count As Integer = 0
    Dim high_number As Long = 0
    Dim low_number As Long = 0
    Dim temp_evalue As Long = 0
    Dim amod_id As Long = 207 ' 207 king air b200 , 1200 citation x+ , 288 g-450 
    Dim temp_year As Integer = 0
    Dim i As Integer = 0
    Dim last_low_value As Long = 0
    Dim ac_id As Long = 0
    Dim temp_make_name As String = ""
    Dim temp_model_name As String = ""
    Dim temp_reason As String = ""
    Dim temp_ser_no As String = ""
    Dim avg_evalue As Double = 0
    Dim past_low_to_use As Long = 0
    Dim last_amod_id As Long = 0

    If Trim(amod_id_passed) <> "" Then
      amod_id = CInt(amod_id_passed)
    End If

    If run_percent = True Then
      year_table = GET_MODEL_AVG_BY_PERCENT(amod_id, 0, run_all, run_backwards)
    Else
      year_table = Get_DLV_Year_Evalues_For_Model(amod_id, 0, run_all, run_backwards)
    End If



    If Not IsNothing(year_table) Then
      If year_table.Rows.Count > 0 Then
        For Each k As DataRow In year_table.Rows

          temp_year = k.Item("ac_Year")

          If run_all = True Then
            amod_id = k.Item("amod_id")
          End If


          If run_percent = True Then
            ' we do not need to do 
          Else
            ' if the amod has changed then we need to clear the last low value
            If last_amod_id <> amod_id Then
              last_low_value = 0
            End If

            last_amod_id = amod_id

            For i = 0 To evalues_count
              evalues_array(i) = 0
            Next
            evalues_count = 0
            evalues_table.Clear()


            evalues_table = Get_Evalues_For_Model(amod_id, temp_year)

            If Not IsNothing(evalues_table) Then
              If evalues_table.Rows.Count > 0 Then
                For Each r As DataRow In evalues_table.Rows
                  evalues_array(evalues_count) = r.Item("afmv_value")
                  evalues_count += 1
                Next
              End If
            End If

            evalues_count = evalues_count - 1
          End If


          If run_percent = True Then

            temp_evalue = k.Item("afmv_value")
            temp_make_name = k.Item("amod_make_name")
            temp_model_name = k.Item("amod_model_name")
            temp_ser_no = k.Item("ac_ser_no_full")
            avg_evalue = k.Item("modelavg")

            low_number = low_number
            ac_id = k.Item("afmv_ac_id")
            temp_evalue = temp_evalue
            temp_reason = "Error: lower than model percentage "
            Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, last_low_value, avg_evalue, temp_reason, amod_id)


          ElseIf run_backwards = True Then
            Call GetStandardDeviation(evalues_array, evalues_count, high_number, low_number, past_low_to_use, run_backwards)

            If high_number = 0 And low_number = 0 Then
              ' this means the current market is too tight to trim
            Else


              If Not IsNothing(evalues_table) Then
                If evalues_table.Rows.Count > 0 Then
                  For Each r As DataRow In evalues_table.Rows

                    temp_evalue = r.Item("afmv_value")
                    temp_make_name = r.Item("amod_make_name")
                    temp_model_name = r.Item("amod_model_name")
                    temp_ser_no = r.Item("ac_ser_no_full")

                    If (last_low_value > 0 And (temp_evalue < last_low_value)) Then
                      low_number = low_number
                      ac_id = r.Item("afmv_ac_id")
                      temp_evalue = temp_evalue
                      temp_reason = "Error: next low percent"
                      Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, last_low_value, avg_evalue, temp_reason, amod_id)
                    End If

                  Next
                End If
              End If

              'if we have enough values, then save the last low value, then 
              If evalues_count >= 5 Then
                last_low_value = past_low_to_use
              Else
                last_low_value = 0
              End If

            End If

          ElseIf evalues_count >= 5 Then
            Call GetStandardDeviation(evalues_array, evalues_count, high_number, low_number, past_low_to_use, run_backwards)

            If high_number = 0 And low_number = 0 Then
              ' this means the current market is too tight to trim
            Else
              If Not IsNothing(evalues_table) Then
                If evalues_table.Rows.Count > 0 Then
                  For Each r As DataRow In evalues_table.Rows

                    temp_evalue = r.Item("afmv_value")
                    temp_make_name = r.Item("amod_make_name")
                    temp_model_name = r.Item("amod_model_name")
                    temp_ser_no = r.Item("ac_ser_no_full")

                    If (temp_evalue > high_number) Then
                      high_number = high_number
                      ac_id = r.Item("afmv_ac_id")
                      temp_evalue = temp_evalue
                      ' insert this ac into the record 
                      temp_reason = "Error: high"
                      Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, low_number, avg_evalue, temp_reason, amod_id)
                    ElseIf (temp_evalue < low_number) Then
                      low_number = low_number
                      ac_id = r.Item("afmv_ac_id")
                      temp_evalue = temp_evalue
                      ' insert this ac into the record 
                      temp_reason = "Error: low"
                      Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, low_number, avg_evalue, temp_reason, amod_id)
                    ElseIf (last_low_value > 0 And (temp_evalue < last_low_value)) Then
                      low_number = low_number
                      ac_id = r.Item("afmv_ac_id")
                      temp_evalue = temp_evalue
                      temp_reason = "Error: prior low"
                      Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, last_low_value, avg_evalue, temp_reason, amod_id)
                    End If

                  Next
                End If
              End If
            End If

            last_low_value = past_low_to_use

          ElseIf last_low_value > 0 Then  'if there is not enough data to do, but there is a last value, then continue, to see if it has dropped 

            If Not IsNothing(evalues_table) Then
              If evalues_table.Rows.Count > 0 Then
                For Each r As DataRow In evalues_table.Rows

                  temp_evalue = r.Item("afmv_value")
                  temp_make_name = r.Item("amod_make_name")
                  temp_model_name = r.Item("amod_model_name")
                  temp_ser_no = r.Item("ac_ser_no_full")

                  If (last_low_value > 0 And (temp_evalue < last_low_value)) Then
                    low_number = low_number
                    ac_id = r.Item("afmv_ac_id")
                    temp_evalue = temp_evalue
                    temp_reason = "Error: prior low"
                    Call Insert_DoNot_Process(ac_id, temp_make_name, temp_model_name, temp_year, temp_ser_no, temp_evalue, high_number, last_low_value, avg_evalue, temp_reason, amod_id)
                  End If

                Next
              End If
            End If
          End If



        Next
      End If
    End If




  End Sub

  Public Function Check_If_DoNot_Process_exists(ByVal ac_id As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" SELECT * ")
      sQuery.Append(" from Asset_Insight_Do_Not_Process ")
      sQuery.Append(" where aidonot_ac_id = '" & ac_id & "' ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function
  Public Sub Insert_DoNot_Process(ByVal ac_id As Long, ByVal make_name As String, ByVal model_name As String, ByVal year_dlv As Integer, ByVal ser_no As String, ByVal evalue As String, ByVal high_number As Long, ByVal low_number As Long, ByVal avg As Long, ByVal reason As String, ByVal amod_id As Long)

    Dim Query As String = ""
    Dim results_table As New DataTable
    Dim temp_name As String = ""
    Dim temp_val As String = ""
    Dim found_spot As Boolean = False
    Dim insert_string_start As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    ' Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim temp_note As String = ""

    Try

      results_table = Check_If_DoNot_Process_exists(ac_id)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then
        Else
          insert_string_start = "INSERT INTO Asset_Insight_Do_Not_Process("
          insert_string_start &= " aidonot_make, aidonot_model, aidonot_ser_no, aidonot_ac_id, aidonot_process_status, aidonot_notes, aidonot_amod_id "
          insert_string_start &= ") VALUES ( "

          insert_string_start &= " '" & make_name & "', "
          insert_string_start &= " '" & model_name & "', "
          insert_string_start &= " '" & ser_no & "', "
          insert_string_start &= " '" & ac_id & "', "
          insert_string_start &= " 'N', "

          If InStr(reason, "percentage") > 0 Then
            temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k, lower than " & FormatNumber((avg / 1000), 0) & "k for percentage of model average"
          ElseIf InStr(reason, "high") > 0 Then
            If InStr(reason, "prior") > 0 Then
              temp_note = temp_note ' SHOULD NEVER GET HERE 
              ' temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k higher than " & FormatNumber((high_number / 1000), 0) & "k for Prior Year: " & year_dlv
            Else
              temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k, higher than " & FormatNumber((high_number / 1000), 0) & "k for " & year_dlv
            End If
          ElseIf InStr(reason, "low") > 0 Then
            If InStr(reason, "prior") > 0 Then
              temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k, lower than " & FormatNumber((low_number / 1000), 0) & "k for Prior Year: " & year_dlv
            ElseIf InStr(reason, "next") > 0 Then
              temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k, lower than " & FormatNumber((low_number / 1000), 0) & "k for Next Year: " & year_dlv
            Else
              temp_note = "AUTOMATED: evalue $" & FormatNumber((evalue / 1000), 0) & "k, lower than " & FormatNumber((low_number / 1000), 0) & "k for " & year_dlv
            End If
          Else
            temp_note = reason & " $" & FormatNumber((evalue / 1000), 0) & "k"
          End If

          insert_string_start &= " '" & temp_note & "', "
          insert_string_start &= " " & amod_id & " "
          insert_string_start &= ") "


          SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          SqlConn.Open()
          SqlCommand.Connection = SqlConn

          SqlCommand.CommandText = insert_string_start
          SqlCommand.ExecuteNonQuery()
          current_count = current_count + 1
        End If
      End If


    Catch ex As Exception
    Finally
      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Sub
  Public Function Get_Evalues_For_Model(ByVal amod_id As Long, ByVal temp_year As Integer) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM aircraft_fmv WITH(NOLOCK)  ")
      sQuery.Append(" inner join aircraft_flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0  ")
      sQuery.Append(" WHERE  amod_id = " & amod_id & " and afmv_value > 0  ")
      If temp_year > 0 Then
        sQuery.Append(" and ac_year = " & temp_year & " ")
      End If
      sQuery.Append(" and afmv_latest_flag = 'Y' and afmv_status = 'Y' ")


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function
  Public Function Get_DLV_Year_Evalues_For_Model_Mfr_Year(ByVal amod_make_name As String, ByVal amod_model_name As String, ByVal mfr_year_start As Long, ByVal mfr_year_end As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("SELECT  distinct ac_id, ac_year, amod_make_name, amod_model_name, ac_ser_no_full, afmv_value, amod_id FROM aircraft_fmv WITH(NOLOCK)  ")
      sQuery.Append(" inner join aircraft_flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0  ")
      sQuery.Append(" WHERE amod_make_name = '" & amod_make_name & "' and afmv_value > 0  ")

      If Trim(amod_model_name) <> "" Then
        sQuery.Append(" and amod_model_name = '" & amod_model_name & "'")
      End If

      If mfr_year_start > 0 Then
        sQuery.Append(" and ac_year >= " & mfr_year_start & " ")
      End If

      If mfr_year_end > 0 Then
        sQuery.Append(" and ac_year <= " & mfr_year_end & " ")
      End If




      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function
  Public Function Get_DLV_Year_Evalues_For_Model(ByVal amod_id As Long, ByVal temp_year As Integer, ByVal run_all As Boolean, ByVal run_backwards As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If amod_id = 0 And run_all = True Then
        sQuery.Append("SELECT  distinct amod_id, ac_year FROM aircraft_fmv WITH(NOLOCK)  ")
        sQuery.Append(" inner join aircraft_flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0  ")
        sQuery.Append(" WHERE afmv_value > 0  ")
        sQuery.Append(" and afmv_latest_flag = 'Y' and afmv_status = 'Y' ")
        sQuery.Append(" group by amod_id, ac_year ")
        If run_backwards = True Then
          sQuery.Append(" order by amod_id asc, ac_year desc ")
        Else
          sQuery.Append(" order by amod_id asc, ac_year asc ")
        End If
      Else
        sQuery.Append("SELECT  distinct ac_year FROM aircraft_fmv WITH(NOLOCK)  ")
        sQuery.Append(" inner join aircraft_flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0  ")
        sQuery.Append(" WHERE amod_id = " & amod_id & " and afmv_value > 0  ")
        If temp_year > 0 Then
          sQuery.Append(" and ac_year = " & temp_year & " ")
        End If
        sQuery.Append(" and afmv_latest_flag = 'Y' and afmv_status = 'Y' ")
        If run_backwards = True Then
          sQuery.Append(" order by ac_year desc ")
        Else
          sQuery.Append(" order by ac_year asc ")
        End If
      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function
  Public Function GET_NEGATIVE_EVALUES() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" SELECT distinct amod_make_name, amod_model_name, ac_ser_no_full,afmv_ac_id, afmv_value, ac_year, amod_id  FROM aircraft_fmv WITH(NOLOCK)    ")
      sQuery.Append(" inner join aircraft_flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
      sQuery.Append(" WHERE afmv_value < 0   and afmv_latest_flag = 'Y' and afmv_status = 'Y'    ")


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function
  Public Function GET_MODEL_AVG_BY_PERCENT(ByVal amod_id As Long, ByVal temp_year As Integer, ByVal run_all As Boolean, ByVal run_backwards As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If amod_id = 0 And run_all = True Then
        sQuery.Append(" select distinct amod_make_name, amod_model_name, ac_ser_no_full,afmv_ac_id, modelavg, ")
        sQuery.Append(" afmv_value, ac_year, amod_id  ")
        sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
        sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on ac_id = afmv_ac_id and ac_journ_id = 0 ")
        sQuery.Append(" inner join View_Asset_Insight_Model_Average on amod_id = View_Asset_Insight_Model_Average.MODELID ")
        sQuery.Append(" where afmv_latest_flag='Y' ")
        sQuery.Append(" and afmv_value > 0 ")
        sQuery.Append(" and afmv_value < (View_Asset_Insight_Model_Average.MODELAVG*.2) ")
        sQuery.Append(" order by amod_make_name, amod_model_name, ac_ser_no_full ")
      Else
        sQuery.Append(" select distinct amod_make_name AS MAKE, amod_model_name AS MODEL, ac_ser_no_full,ac_id, modelavg, ")
        sQuery.Append(" afmv_value, ac_year, amod_id  ")
        sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
        sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on ac_id = afmv_ac_id and ac_journ_id = 0 ")
        sQuery.Append(" inner join View_Asset_Insight_Model_Average on amod_id = View_Asset_Insight_Model_Average.MODELID ")
        sQuery.Append(" where afmv_latest_flag='Y' ")
        sQuery.Append(" and afmv_value > 0 ")
        sQuery.Append(" and amod_id > " & amod_id & " ")
        sQuery.Append(" and afmv_value < (View_Asset_Insight_Model_Average.MODELAVG*.2) ")
        sQuery.Append(" order by amod_make_name, amod_model_name, ac_ser_no_full ")
      End If



      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "user_portfolio_view.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  '--------------- END OF ASSET INSIGHT FUNCTIONS ---------------






End Class