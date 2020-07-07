' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiRegIndex.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:43a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: abiRegIndex.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiRegIndex
  Inherits System.Web.UI.Page
  Public mailToSubject As String = ""

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim AircraftTable As New DataTable
    Dim ModelID As Long = 0
    Dim ACID As Long = 0
    Dim ModelString As String = ""
    Dim MakeString As String = ""

    Master.Set_Meta_Information("Aircraft for sale, planes for sale, helicopters for sale, including: Cessna, Gulfstream, Challenger, Hawker, and Learjet aircraft by Aircraft Dealers & Brokers.", "aircraft for sale, jets for sale, turbo props for sale, helicopters for sale, aircraft wanteds, business jets, used aircraft, used planes, aircraft sale, JETNET global, aviation, aircraft, fbo, dealer, news, aviation links, aviation events, aviation products, plane, airplane, Cessna, gulfstream, hawker, learjet, lear jet, jetnet")
    Master.Set_Page_Title("Aircraft Registry at JETNET Global")

    If Not IsNothing(HttpContext.Current.Request("Make")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("Make").ToString.Trim) Then
        MakeString = HttpContext.Current.Request("Make").ToString.Trim
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request("ID")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("ID").ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Request("ID").ToString) Then
          ModelID = CLng(HttpContext.Current.Request("ID").ToString.Trim)
          viewAllDiv.Visible = True
          aside_right.Visible = False
          component.Attributes.Remove("class")
          component.Attributes.Add("class", "span9")
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request("Model")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("Model").ToString.Trim) Then
        ModelString = HttpContext.Current.Request("Model").ToString.Trim
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request("ACID")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("ACID").ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Request("ACID").ToString) Then
          ACID = CLng(HttpContext.Current.Request("ACID").ToString.Trim)
          viewAllDiv.Visible = False
          aside_right.Visible = False
          component.Attributes.Remove("class")
          component.Attributes.Add("class", "span9")
        End If
      End If
    End If

    If ModelID > 0 Then

      acModelIncludedText.Visible = True

      Dim HeaderStr As String = ""
      Dim ModelData As New DataTable

      AircraftTable = commonEvo.Get_MakesModels_ByType(True, ModelID, 0, "")

      If Not IsNothing(AircraftTable) Then
        If AircraftTable.Rows.Count > 0 Then
          HeaderStr = AircraftTable.Rows(0).Item("amod_make_name").ToString.Trim + " " + AircraftTable.Rows(0).Item("amod_model_name").ToString.Trim
        End If
      End If

      FillSerialRegNumberList(AircraftTable)

      ModelData = Master.AbiDataManager.Get_Model_By_ID(ModelID)
      MoreModelInformation(ModelData)

      ac_header.InnerHtml = HeaderStr + " Aircraft Registry"

    ElseIf ACID > 0 Then

      acDetailInfoIncludedText.Visible = True

      Dim HeaderStr As String = ""
      Dim RegString As String = ""

      AircraftTable = commonEvo.GetAllAircraftInfo_dataTable(ACID, 0, False)

      If Not IsNothing(AircraftTable) Then
        If AircraftTable.Rows.Count > 0 Then

          HeaderStr = AircraftTable.Rows(0).Item("ac_mfr_year").ToString.Trim + " " + AircraftTable.Rows(0).Item("amod_make_name").ToString.Trim + " " + AircraftTable.Rows(0).Item("amod_model_name").ToString.Trim
          RegString = AircraftTable.Rows(0).Item("ac_reg_no").ToString.Trim

        End If
      End If

      DisplayACDetails(AircraftTable, HeaderStr)

      ac_header.InnerHtml = RegString + " - " + HeaderStr + " Aircraft Registry"

      mailToSubject = Server.HtmlEncode("JETNET Global Inquiry on " + RegString + " - " + HeaderStr + " Aircraft Registry")

      mailtoHref.Text = "<a class=""pointer"" href=""mailto:info@jetnetnews.com?subject=" + mailToSubject + """>info@jetnetnews.com</a>"

      Master.Set_Meta_Information("Registration number lookup for tail number " + RegString, "reg no, make, model, ser no, year, registration number, tail number, n number, registry, aircraft")

      Master.AbiDataManager.Create_ABI_Stats(0, 0, RegString, 0, ACID)

    Else

      acIncludedText.Visible = True

      'Fill Executive Aircraft
      AircraftTable = commonEvo.Get_MakesModels_ByType(False, 0, Constants.VIEW_EXECUTIVE, MakeString)
      DisplayACModel(AircraftTable, "Executive Aircraft")
      'Fill Jet Aircraft
      AircraftTable = commonEvo.Get_MakesModels_ByType(False, 0, Constants.VIEW_JETS, MakeString)
      DisplayACModel(AircraftTable, "Jet Aircraft")
      'Fill Turbo
      AircraftTable = commonEvo.Get_MakesModels_ByType(False, 0, Constants.VIEW_TURBOPROPS, MakeString)
      DisplayACModel(AircraftTable, "Turboprop Aircraft")
      'Fill Piston
      AircraftTable = commonEvo.Get_MakesModels_ByType(False, 0, Constants.VIEW_PISTONS, MakeString)
      DisplayACModel(AircraftTable, "Piston Aircraft")
      'Fill Helicopters
      AircraftTable = commonEvo.Get_MakesModels_ByType(False, 0, Constants.VIEW_HELICOPTERS, MakeString)
      DisplayACModel(AircraftTable, "Helicopters")

    End If

    Master.Set_Page_Title(stripBrackets(ac_header.InnerHtml) + " at JETNET Global")

  End Sub

  Private Function stripBrackets(ByRef Str As String) As String
    Dim pattern As String = "\(\d+\)"
    Dim rgx As Regex = New Regex(pattern, RegexOptions.IgnoreCase)

    Return rgx.Replace(Str, "")
  End Function

  Private Sub MoreModelInformation(ByRef ModelDetails As DataTable)
    Dim DisplayStr As String = ""
    Dim EngineTable As New DataTable
    If Not IsNothing(ModelDetails) Then
      If ModelDetails.Rows.Count > 0 Then
        Dim FieldDisplay As String = ""

        DisplayStr = "<hr /><div class=""items-row col-2 row-fluid""><span class=""span7""><h4>More Information about the " & ModelDetails.Rows(0).Item("amod_make_name") & " " & ModelDetails.Rows(0).Item("amod_model_name") & " Aircraft</h4></span></div>"
        DisplayStr += "<div class=""items-row col-2 row-fluid""><img width=""34%"" src=""" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session("ModelPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/model/") & ModelDetails.Rows(0).Item("amod_id") & ".jpg"" class=""pull-right"" />"

        If Not IsDBNull(ModelDetails.Rows(0).Item("amod_manufacturer")) Then
          If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_manufacturer"))) Then
            DisplayStr += "<span class=""span2"">Manufacturer:</span>"
            DisplayStr += "<span class=""span4"">" & ModelDetails.Rows(0).Item("amod_manufacturer") & "</span><div class=""clear_left""></div>"
          End If
        End If


        If Not IsDBNull(ModelDetails.Rows(0).Item("amod_start_year")) Then
          If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_start_year"))) Then
            FieldDisplay = ModelDetails.Rows(0).Item("amod_start_year")
            If Not IsDBNull(ModelDetails.Rows(0).Item("amod_end_year")) Then
              If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_end_year"))) Then
                FieldDisplay += " - "
              End If
            End If
          End If
        End If

        If Not IsDBNull(ModelDetails.Rows(0).Item("amod_end_year")) Then
          If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_end_year"))) Then

            FieldDisplay += "" & ModelDetails.Rows(0).Item("amod_end_year")
          End If
        End If

        If FieldDisplay <> "" Then
          DisplayStr += "<span class=""span2"">Year(s) Built:</span>"
          DisplayStr += "<span class=""span4"">"
          DisplayStr += FieldDisplay
          DisplayStr += "</span><div class=""clear_left""></div>"
        End If

        FieldDisplay = ""


        If Not (IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_start")) And IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_end"))) Then
          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_prefix")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_prefix"))) Then
              FieldDisplay = ModelDetails.Rows(0).Item("amod_ser_no_prefix")

            End If
          End If

          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_start")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_start"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_start") & " - "
            End If
          End If

          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_prefix")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_prefix"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_prefix")
            End If
          End If
          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_end")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_end"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_end")
            End If
          End If

          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_suffix")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_suffix"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_suffix")
            End If
          End If

        ElseIf Not (IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_prefix")) And IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_start"))) Then
          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_prefix")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_prefix"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_prefix")
            End If
          End If
          If Not IsDBNull(ModelDetails.Rows(0).Item("amod_ser_no_start")) Then
            If Not String.IsNullOrEmpty(Trim(ModelDetails.Rows(0).Item("amod_ser_no_start"))) Then
              FieldDisplay += ModelDetails.Rows(0).Item("amod_ser_no_start") & " & Up"
            End If
          End If

        End If

        If FieldDisplay <> "" Then
          DisplayStr += "<span class=""span2"">Serial # Range:</span>"
          DisplayStr += "<span class=""span4"">"
          DisplayStr += FieldDisplay
          DisplayStr += "</span>"
        End If
        FieldDisplay = ""

        DisplayStr += "<span class=""span4""></span><div class=""clear_left""></div>"
        DisplayStr += "<span class=""span2"">Weight Class/Type:</span>"
        DisplayStr += "<span class=""span4"">"
        If (ModelDetails.Rows(0).Item("amod_weight_class") = "L") Then
          DisplayStr += "Light"
        ElseIf (ModelDetails.Rows(0).Item("amod_weight_class") = "H") Then
          DisplayStr += "Heavy"
        ElseIf (ModelDetails.Rows(0).Item("amod_weight_class") = "M") Then
          DisplayStr += "Medium"
        ElseIf (ModelDetails.Rows(0).Item("amod_weight_class") = "V") Then
          DisplayStr += "Very Light"
        Else
          DisplayStr += "N/A"
        End If

        If (ModelDetails.Rows(0).Item("amod_airframe_type_code") = "R") Then
          DisplayStr += " - Helicopter"
        ElseIf (ModelDetails.Rows(0).Item("amod_type_code") = "J") Then
          DisplayStr += " - Jet"
        ElseIf (ModelDetails.Rows(0).Item("amod_type_code") = "T") Then
          DisplayStr += " - TurboProp"
        ElseIf (ModelDetails.Rows(0).Item("amod_type_code") = "P") Then
          DisplayStr += " - Piston"
        ElseIf (ModelDetails.Rows(0).Item("amod_type_code") = "E") Then
          DisplayStr += " - Executive"
        End If

        DisplayStr += "</span><div class=""clear_left""></div><br />"
        DisplayStr += "<span class=""span2""><strong>Engine Details:</strong></span>"
        DisplayStr += "<span class=""span4""></span><div class=""clear_left""></div>"

        EngineTable = Master.AbiDataManager.Get_Engine_Info_By_ID(ModelDetails.Rows(0).Item("amod_id"))

        If Not IsNothing(EngineTable) Then
          If EngineTable.Rows.Count > 0 Then
            For Each r As DataRow In EngineTable.Rows
              If Not IsDBNull(r("ameng_engine_name")) Then
                If Not String.IsNullOrEmpty(r("ameng_engine_name")) Then
                  If FieldDisplay <> "" Then
                    FieldDisplay += ", "
                  End If
                  FieldDisplay += r("ameng_engine_name")
                End If
              End If
            Next
          End If
        End If

        If FieldDisplay <> "" Then
          DisplayStr += "<span class=""span2"">Engines:</span>"
          DisplayStr += "<span class=""span5"">"
          DisplayStr += FieldDisplay
          DisplayStr += "</span><div class=""clear_left""></div>"
        End If

        DisplayStr += "<span class=""span2"">Number Of:</span><span class=""span4"">"
        If (Not IsDBNull(ModelDetails.Rows(0).Item("amod_number_of_engines"))) Then
          DisplayStr += ModelDetails.Rows(0).Item("amod_number_of_engines").ToString
        Else
          DisplayStr += "0"
        End If

        DisplayStr += "</span><div class=""clear_left""></div>"
        DisplayStr += "<span class=""span2"">TBO:</span>"
        DisplayStr += "<span class=""span4"">"

        If Not String.IsNullOrEmpty(ModelDetails.Rows(0).Item("amod_engine_com_tbo_hrs") And ModelDetails.Rows(0).Item("amod_engine_com_tbo_hrs") <> "0") Then
          DisplayStr += FormatNumber(ModelDetails.Rows(0).Item("amod_engine_com_tbo_hrs"), 0).ToString
        Else
          DisplayStr += "0"
        End If

        DisplayStr += "</span><div class=""clear_left""></div>"
        DisplayStr += "<span class=""span2"">HSI:</span>"
        DisplayStr += "<span class=""span4"">"

        If Not String.IsNullOrEmpty(ModelDetails.Rows(0).Item("amod_engine_hsi") And ModelDetails.Rows(0).Item("amod_engine_hsi") <> "0") Then
          DisplayStr += FormatNumber(ModelDetails.Rows(0).Item("amod_engine_hsi"), 0).ToString
        Else
          DisplayStr += "0"
        End If

        DisplayStr += "</span><div class=""clear_left""></div>"
        DisplayStr += "<span class=""span2"">Shaft:</span>"
        DisplayStr += "<span class=""span4"">"

        If Not String.IsNullOrEmpty(ModelDetails.Rows(0).Item("amod_engine_shaft")) And ModelDetails.Rows(0).Item("amod_engine_shaft") <> "0" Then
          DisplayStr += FormatNumber(ModelDetails.Rows(0).Item("amod_engine_shaft"), 0).ToString
        Else
          DisplayStr += "0"
        End If
        DisplayStr += "</span><div class=""clear_left""></div>"

        DisplayStr += "<span class=""span2"">Thrust:</span>"
        DisplayStr += "<span class=""span4"">"
        If Not String.IsNullOrEmpty(ModelDetails.Rows(0).Item("amod_engine_thrust_lbs")) And ModelDetails.Rows(0).Item("amod_engine_thrust_lbs") <> "0" Then
          DisplayStr += FormatNumber(ModelDetails.Rows(0).Item("amod_engine_thrust_lbs"), 0).ToString
        Else
          DisplayStr += "0"
        End If
        DisplayStr += "</span><div class=""clear_left""></div></div>"


        DisplayStr += FillOtherModels(ModelDetails.Rows(0).Item("amod_make_name"), ModelDetails.Rows(0).Item("amod_id"))

      End If
    End If


    moreModelInformationLiteral.Text = DisplayStr
  End Sub

  Private Function FillOtherModels(ByRef makeName As String, ByVal nAmodID As Long) As String
    Dim ReturnStr As String = ""
    Dim OtherModelTable As New DataTable

    OtherModelTable = commonEvo.Get_MakesModels_ByType(False, 0, 0, makeName)
    If Not IsNothing(OtherModelTable) Then
      If OtherModelTable.Rows.Count > 0 Then
        ReturnStr = "<br /><hr /><span class=""30pxLeftBuffer""><h4>Other " + OtherModelTable.Rows(0).Item("amod_make_name").ToString.Trim + " Aircraft Models</h4></span><div class=""clear_left""></div><div class=""items-row cols-4 row-fluid"">"

        For Each r As DataRow In OtherModelTable.Rows

          If CLng(r.Item("amod_id").ToString) <> nAmodID Then
            ReturnStr += "<span class=""span2""><a href=""abiRegIndex.aspx?ID=" + r.Item("amod_id").ToString.Trim + """ title=""Display Aircraft Model Registry"">" + r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + "</a></span>"
          End If

        Next
        ReturnStr += "</div>"
      End If
    End If
    Return ReturnStr
  End Function

  Private Sub FillSerialRegNumberList(ByRef AircraftTable As DataTable)

    Dim DisplayString As String = ""

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then
        DisplayString = "<div class=""clearfix""></div>"  ' "KING-AIR-300-N694JB

        For Each r As DataRow In AircraftTable.Rows

          DisplayString += "<span class=""span2  30pxLeftBuffer""><a href=""/listings/aircraft/registry/" + r.Item("ac_id").ToString.Trim + "/" + r.Item("ac_mfr_year").ToString.Trim + "-" + r.Item("amod_make_name").ToString.Replace(Constants.cSingleSpace, Constants.cHyphen).Trim + "-" + r.Item("amod_model_name").ToString.Replace(Constants.cSingleSpace, Constants.cHyphen).Replace("+", "PLUS").Trim + "-" + r.Item("ac_reg_no").ToString.Replace("+", "PLUS").Trim
          DisplayString += """ title=""Display Aircraft Info"">" + r.Item("ac_ser_no_full").ToString.Trim + " / " + r.Item("ac_reg_no").ToString.Trim
          DisplayString += "</a></span>"

        Next

        DisplayString += "<div class=""clearfix""></div><br />"

      End If
    End If

    serialRegNumberlist.Text += DisplayString

  End Sub

  Private Sub DisplayACModel(ByRef AircraftTable As DataTable, ByRef HeaderTitle As String)

    Dim DisplayString As String = ""

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then

        DisplayString = "<span class=""span8 30pxLeftBuffer""><h4>" + HeaderTitle + "</h4></span><div class=""clearfix""></div>"

        For Each r As DataRow In AircraftTable.Rows
          DisplayString += "<span class=""span2  30pxLeftBuffer""><a href=""abiRegIndex.aspx?"
          DisplayString += "ID=" + r.Item("amod_id").ToString.Trim + """ title=""Display Aircraft Model Registry"">" + r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim
          DisplayString += "</a></span>"
        Next
      End If
      DisplayString += "<div class=""clearfix""></div><br />"

    End If

    acListLiteral.Text += DisplayString
  End Sub

  Private Sub DisplayACDetails(ByRef AircraftTable As DataTable, ByRef HeaderTitle As String)

    Dim DisplayString As String = ""
    Dim EngineTable As New DataTable
    Dim engineName As String = ""
    Dim engineMfr As String = ""
    Dim engineMfrAbbrev As String = ""

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then

        DisplayString = "<span class=""span8 30pxLeftBuffer""><h4>" + HeaderTitle + "</h4></span><div class='clearfix'></div>"

        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_year")) Then
          If Not String.IsNullOrEmpty(Trim(AircraftTable.Rows(0).Item("ac_year"))) Then
            DisplayString += "<span class=""span2"">YEAR DELIVERED:</span>"
            DisplayString += "<span class=""span4"">" + AircraftTable.Rows(0).Item("ac_year").ToString.Trim + "</span><div class=""clear_left""></div>"
          End If
        End If

        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_no_full")) Then
          If Not String.IsNullOrEmpty(Trim(AircraftTable.Rows(0).Item("ac_ser_no_full"))) Then
            DisplayString += "<span class=""span2"">SERIAL:</span>"
            DisplayString += "<span class=""span4"">" + AircraftTable.Rows(0).Item("ac_ser_no_full").ToString.Trim + "</span><div class=""clear_left""></div>"
          End If
        End If

        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_no")) Then
          If Not String.IsNullOrEmpty(Trim(AircraftTable.Rows(0).Item("ac_reg_no"))) Then
            DisplayString += "<span class=""span2"">REGISTRAION:</span>"
            DisplayString += "<span class=""span4"">" + AircraftTable.Rows(0).Item("ac_reg_no").ToString.Trim + "</span><div class=""clear_left""></div>"
          End If
        End If

        If Not IsDBNull(AircraftTable.Rows(0).Item("amod_airframe_type_code")) Then
          If Not String.IsNullOrEmpty(Trim(AircraftTable.Rows(0).Item("amod_airframe_type_code"))) Then
            DisplayString += "<span class=""span2"">AIRFRAME TYPE:</span>"
            DisplayString += "<span class=""span4"">" + IIf(AircraftTable.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper.Contains("F"), "Fixed Wing", "Roatary") + "</span><div class=""clear_left""></div>"
          End If
        End If

        If Not IsDBNull(AircraftTable.Rows(0).Item("amod_manufacturer")) Then
          If Not String.IsNullOrEmpty(Trim(AircraftTable.Rows(0).Item("amod_manufacturer"))) Then
            DisplayString += "<span class=""span2"">MANUFACTURER:</span>"
            DisplayString += "<span class=""span4"">" + AircraftTable.Rows(0).Item("amod_manufacturer").ToString.Trim + "</span><div class=""clear_left""></div>"
          End If
        End If

        EngineTable = Master.AbiDataManager.Get_Engine_Info_By_ID(CLng(AircraftTable.Rows(0).Item("amod_id").ToString))

        If Not IsNothing(EngineTable) Then
          If EngineTable.Rows.Count > 0 Then

            For Each r As DataRow In EngineTable.Rows


              If Not IsDBNull(r.Item("ameng_engine_name")) Then
                If Not String.IsNullOrEmpty(r.Item("ameng_engine_name").ToString.Trim) Then

                  If Not IsDBNull(r.Item("ameng_mfr_name_abbrev")) Then
                    If Not String.IsNullOrEmpty(r.Item("ameng_mfr_name_abbrev").ToString.Trim) Then
                      engineMfrAbbrev = r.Item("ameng_mfr_name_abbrev").ToString.Trim
                    End If
                  End If

                  If String.IsNullOrEmpty(engineName.Trim) Then
                    engineName = r.Item("ameng_engine_name").ToString.Trim + Constants.cSingleSpace + "[" + engineMfrAbbrev + "]"
                  Else
                    engineName += Constants.cCommaDelim + Constants.cSingleSpace + r.Item("ameng_engine_name").ToString.Trim + Constants.cSingleSpace + "[" + engineMfrAbbrev + "]"
                  End If

                  engineMfrAbbrev = ""

                End If
              End If

              If Not IsDBNull(r.Item("ameng_mfr_name")) Then
                If Not String.IsNullOrEmpty(r.Item("ameng_mfr_name").ToString.Trim) Then

                  If String.IsNullOrEmpty(engineMfr.Trim) Then
                    engineMfr = r.Item("ameng_mfr_name").ToString.Trim
                  Else ' only add unique mfr names
                    If Not engineMfr.ToLower.Contains(r.Item("ameng_mfr_name").ToString.ToLower.Trim) Then
                      engineMfr += Constants.cCommaDelim + Constants.cSingleSpace + r.Item("ameng_mfr_name").ToString.Trim
                    End If
                  End If

                End If
              End If

            Next
          End If
        End If

        If Not String.IsNullOrEmpty(engineName) Then
          DisplayString += "<span class=""span2"">ENGINES "

          If (Not IsDBNull(AircraftTable.Rows(0).Item("amod_number_of_engines"))) Then
            DisplayString += "(" + AircraftTable.Rows(0).Item("amod_number_of_engines").ToString.Trim + ")"
          Else
            DisplayString += "(0)"
          End If

          DisplayString += ":</span><span class=""span8"">" + engineName + "</span><div class=""clear_left""></div>"
        End If

        If Not String.IsNullOrEmpty(engineMfr) Then
          DisplayString += "<span class=""span2"">ENGINE MFRS:</span>"
          DisplayString += "<span class=""span8"">" + engineMfr + "</span>"
        End If


      End If

      DisplayString += "<div class=""clearfix""></div><br />"

    End If

    acDetailedList.Text += DisplayString
  End Sub
End Class