' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiForsale.aspx.vb $
'$$Author: Mike $
'$$Date: 5/04/20 2:33p $
'$$Modtime: 5/04/20 2:31p $
'$$Revision: 4 $
'$$Workfile: abiForsale.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiForsale
  Inherits System.Web.UI.Page
  Dim CompanyName As String = ""

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim AircraftTable As New DataTable
    Dim AirframeType As String = ""
    Dim ListingType As String = ""
    Dim AirType As String = ""
    Dim DealerID As Long = 0
    Dim ModelID As Long = 0
    Dim ModelString As String = ""
    Dim MakeString As String = ""
    Dim YearStart As String = ""
    Dim YearEnd As String = ""

    FillABIDealers()

    Master.Set_Meta_Information("Aircraft for sale, planes for sale, helicopters for sale, including: Cessna, Gulfstream, Challenger, Hawker, and Learjet aircraft by Aircraft Dealers & Brokers.", "aircraft for sale, jets for sale, turbo props for sale, helicopters for sale, aircraft wanteds, business jets, used aircraft, used planes, aircraft sale, JETNET global, aviation, aircraft, fbo, dealer, news, aviation links, aviation events, aviation products, plane, airplane, Cessna, gulfstream, hawker, learjet, lear jet, jetnet")
    Master.Set_Page_Title("Aircraft for Sale, Aircraft Sales, Used Aircraft for Sale at JETNET Global")

    'Type to filter type of aircraft
    If Not IsNothing(Trim(Request("AirframeType"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("AirframeType"))) Then
        AirframeType = Trim(Request("AirframeType"))
        AirframeType = Server.UrlDecode(AirframeType)
        viewAllDiv.Visible = True
      End If
    End If

    If Not IsNothing(Trim(Request("Make"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("Make"))) Then
        MakeString = Trim(Request("Make"))
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If


    If Not IsNothing(Trim(Request("start"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("start"))) Then
        YearStart = Trim(Request("start"))
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If

    If Not IsNothing(Trim(Request("end"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("end"))) Then
        YearEnd = Trim(Request("end"))
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If

    If Not IsNothing(Trim(Request("Dealer"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("Dealer"))) Then
        If IsNumeric(Trim(Request("Dealer"))) Then
          Dim DealerTable As New DataTable
          DealerID = Trim(Request("Dealer"))
          DealerID = Server.UrlDecode(DealerID)
          Master.ToggleMarketTrendsColumn(False)
          aside_right.Attributes.Remove("class")
          aside_right.Attributes.Add("class", "span3 span3withBorder  gradient")

          component.Attributes.Remove("class")
          component.Attributes.Add("class", "span9")

          viewAllDiv.Visible = True

          leftCategory.Visible = False
          leftHeaderText.Visible = False

          'Let's get the Company Name for future reference:
          DealerTable = Master.AbiDataManager.GetCompanyInformation(DealerID, 0)
          If Not IsNothing(DealerTable) Then
            If DealerTable.Rows.Count > 0 Then
              If Not IsDBNull(DealerTable.Rows(0).Item("comp_name")) Then
                CompanyName = DealerTable.Rows(0).Item("comp_name")
              End If

              If Not Page.IsPostBack Then
                'Go ahead and add the content stats in this.
                Master.AbiDataManager.Create_ABI_Stats(DealerID, 0, "", 0)
              End If
            End If
          End If
        End If
      End If
    End If

    If Not IsNothing(Trim(Request("ID"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("ID"))) Then
        If IsNumeric(Trim(Request("ID"))) Then
          ModelID = Trim(Request("ID"))
          ModelID = Server.UrlDecode(ModelID)
          viewAllDiv.Visible = True
          aside_right.Visible = False
          component.Attributes.Remove("class")
          component.Attributes.Add("class", "span9")
        End If
      End If
    End If

    If Not IsNothing(Trim(Request("Model"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("Model"))) Then
        ModelString = Trim(Request("Model"))
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If

    If Not IsNothing(Trim(Request("AirType"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("AirType"))) Then
        AirType = Trim(Request("AirType"))
        AirType = Server.UrlDecode(AirType)
        viewAllDiv.Visible = True
      End If
    End If

    If Not IsNothing(Trim(Request("type"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("type"))) Then
        ListingType = Trim(Request("type"))
        ListingType = Server.UrlDecode(ListingType)
        viewAllDiv.Visible = True
        aside_right.Visible = False
        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
      End If
    End If


    If DealerID > 0 Or ModelID > 0 Or MakeString <> "" Or YearStart <> "" Or YearEnd <> "" Or ModelString <> "" Then
      Dim HeaderStr As String = ""

      AircraftTable = Master.AbiDataManager.GetABIACForSaleDetailedList(AirframeType, AirType, DealerID, ModelID, MakeString, YearStart, YearEnd, ModelString)

      If DealerID > 0 Then
        If Not IsNothing(AircraftTable) Then
          If AircraftTable.Rows.Count > 0 Then
            'Master.Set_Page_Title("" & AircraftTable.Rows(0).Item("comp_name") & " Aircraft for Sale at JETNET Global")
            'If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
            '  companyInformation.Text = "<img src=""images/blank.gif"" class=""lazy"" data-src=""http://www.jetnetGlobal.com/photos/company/" & AircraftTable.Rows(0).Item("comp_id") & ".jpg"" alt=""" & AircraftTable.Rows(0).Item("comp_name") & """ />"
            'Else
            '  companyInformation.Text = "<img src=""images/blank.gif"" class=""lazy"" data-src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & AircraftTable.Rows(0).Item("comp_id") & ".jpg"" alt=""" & AircraftTable.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
            'End If 

            If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
              companyInformation.Text = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "abiFiles/images/blank.gif"" class=""lazy"" data-src=""" & clsData_Manager_SQL.get_site_name & "/pictures/company/" & AircraftTable.Rows(0).Item("comp_id") & ".jpg"" alt=""" & AircraftTable.Rows(0).Item("comp_name") & """ />"
            Else
              companyInformation.Text = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "abiFiles/images/blank.gif"" class=""lazy"" data-src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & AircraftTable.Rows(0).Item("comp_id") & ".jpg"" alt=""" & AircraftTable.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
            End If


            companyInformation.Text += "<br /><br /><h4 class='size15'>" & AircraftTable.Rows(0).Item("comp_name") & "</h4>"
            companyInformation.Text += "<p>" & abi_functions.DisplayCompanyInformation(AircraftTable.Rows(0).Item("comp_id"), AircraftTable.Rows(0).Item("comp_address1"), AircraftTable.Rows(0).Item("comp_address2"), AircraftTable.Rows(0).Item("comp_city"), AircraftTable.Rows(0).Item("comp_state"), AircraftTable.Rows(0).Item("comp_zip_code"), AircraftTable.Rows(0).Item("comp_country"), AircraftTable.Rows(0).Item("comp_web_address")) & "</p>"
          Else
            Dim CompanyData As New DataTable
            CompanyData = Master.AbiDataManager.GetABIDealerInformation(DealerID)

            If Not IsNothing(CompanyData) Then
              If CompanyData.Rows.Count > 0 Then
                'Still display the dealer information:
                'If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                '  companyInformation.Text = "<img src=""http://www.jetnetGlobal.com/photos/company/" & CompanyData.Rows(0).Item("comp_id") & ".jpg"" alt=""" & CompanyData.Rows(0).Item("comp_name") & """ />"
                'Else
                '  companyInformation.Text = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & CompanyData.Rows(0).Item("comp_id") & ".jpg"" alt=""" & CompanyData.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
                'End If

                If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                  companyInformation.Text = "<img src=""" & clsData_Manager_SQL.get_site_name & "/pictures/company/" & CompanyData.Rows(0).Item("comp_id") & ".jpg"" alt=""" & CompanyData.Rows(0).Item("comp_name") & """ />"
                Else
                  companyInformation.Text = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & CompanyData.Rows(0).Item("comp_id") & ".jpg"" alt=""" & CompanyData.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
                End If



                companyInformation.Text += "<br /><br /><h4 class='size15'>" & CompanyData.Rows(0).Item("comp_name") & "</h4>"
                companyInformation.Text += "<p>" & abi_functions.DisplayCompanyInformation(CompanyData.Rows(0).Item("comp_id"), CompanyData.Rows(0).Item("comp_address1"), CompanyData.Rows(0).Item("comp_address2"), CompanyData.Rows(0).Item("comp_city"), CompanyData.Rows(0).Item("comp_state"), CompanyData.Rows(0).Item("comp_zip_code"), CompanyData.Rows(0).Item("comp_country"), CompanyData.Rows(0).Item("comp_web_address")) & "</p>"

                'Display warning that no results were found.
                acListLiteral.Text = "<p align='center'>No results found for " & CompanyName & IIf(ModelID > 0, " and this model", "") & ".</p>"
              End If
            End If
          End If
        End If
      ElseIf ModelID > 0 Or ModelString <> "" Then
        If Not IsNothing(AircraftTable) Then
          If AircraftTable.Rows.Count > 0 Then
            ModelID = AircraftTable.Rows(0).Item("amod_id")
            HeaderStr = AircraftTable.Rows(0).Item("amod_make_name") & " " & AircraftTable.Rows(0).Item("amod_model_name")
            'Master.Set_Page_Title(HeaderStr & " Aircraft for Sale at JETNET Global")
          End If
        End If
      ElseIf MakeString <> "" Then
        HeaderStr = DisplayFunctions.ConvertToTitleCase(MakeString) & " "
        'Master.Set_Page_Title(HeaderStr & IIf(ListingType <> "", ListingType & " ", "") & "Aircraft for Sale at JETNET Global")

        newsByMake.Text = FillNewsByMake(MakeString, 0)
      End If

      If ModelID = 0 Then
        HeaderStr += IIf(DealerID > 0, CompanyName & " ", "") & ListingType & " Aircraft"
      Else
        newsByMake.Text = FillNewsByMake(MakeString, ModelID)
        Dim ModelData As New DataTable
        ModelData = Master.AbiDataManager.Get_Model_By_ID(ModelID)
        MoreModelInformation(ModelData)
      End If

      If DealerID = 0 Then
        FillForSaleAircraftPerListingType(AircraftTable, "", DealerID)
      ElseIf DealerID > 0 Then
        'Split the original table into types
        Dim SplitTable As New DataTable

        'Filter for Executive
        SplitTable = FilterTypeTable("F", "E", AircraftTable)
        FillForSaleAircraftPerListingType(SplitTable, "Executive Jets", DealerID)

        'Filter for Jet Aircraft
        SplitTable = FilterTypeTable("F", "J", AircraftTable)
        FillForSaleAircraftPerListingType(SplitTable, "Jet Aircraft", DealerID)

        'Filter for Turbo
        SplitTable = FilterTypeTable("F", "T", AircraftTable)
        FillForSaleAircraftPerListingType(SplitTable, "Turboprop Aircraft", DealerID)

        'Filter for Helicopter
        SplitTable = FilterTypeTable("R", "", AircraftTable)
        FillForSaleAircraftPerListingType(SplitTable, "Helicopters", DealerID)

        FillCompanyWanted(DealerID)
        viewAllDiv.Visible = False
      End If
      ac_header.InnerHtml = HeaderStr & " For Sale"
    Else
      Dim DisplayMakeOnly As Boolean = True

      Select Case ListingType.ToLower
        Case "executive"
          'Fill Executive Aircraft
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "E", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Executive Aircraft", ListingType, DisplayMakeOnly)
        Case "jets"
          'Fill Jet Aircraft
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "J", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Jet Aircraft", ListingType, DisplayMakeOnly)
        Case "turboprops"
          'Fill Turbo
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "T", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Turboprop Aircraft", ListingType, DisplayMakeOnly)
        Case "pistons"
          'Fill Piston
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "P", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Piston Aircraft", ListingType, DisplayMakeOnly)
        Case "helicopters"
          'Fill Helicopters
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("R", "", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Helicopters", ListingType, DisplayMakeOnly)
        Case Else
          DisplayMakeOnly = False
          acIncludedText.Visible = True
          'Fill Executive Aircraft
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "E", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Executive Aircraft", ListingType, DisplayMakeOnly)
          'Fill Jet Aircraft
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "J", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Jet Aircraft", ListingType, DisplayMakeOnly)
          'Fill Turbo
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "T", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Turboprop Aircraft", ListingType, DisplayMakeOnly)
          'Fill Piston
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("F", "P", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Piston Aircraft", ListingType, DisplayMakeOnly)
          'Fill Helicopters
          AircraftTable = Master.AbiDataManager.GetABIACForSaleList("R", "", DisplayMakeOnly, MakeString)
          FillForSaleAC(AircraftTable, "Helicopters", ListingType, DisplayMakeOnly)

      End Select
    End If
    If LCase(ListingType) = "helicopters" Then
      ac_header.InnerHtml = Replace(ac_header.InnerHtml, " Aircraft", "")
    End If

    Master.Set_Page_Title(stripBrackets(ac_header.InnerHtml) & " at JETNET Global")
  End Sub


  Private Function stripBrackets(ByRef Str As String) As String
    Dim pattern As String = "\(\d+\)"
    Dim rgx As Regex = New Regex(pattern, RegexOptions.IgnoreCase)

    Return rgx.Replace(Str, "")
  End Function
  Private Sub FillABIDealers()

    Dim DealersTable As New DataTable
    DealersTable = Master.AbiDataManager.GetABIDealers(False, "")

    If Not IsNothing(DealersTable) Then
      dealersRepeater.DataSource = DealersTable
      dealersRepeater.DataBind()
    End If

  End Sub

  Private Function FilterTypeTable(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String, ByVal aircraftTable As DataTable) As DataTable
    Dim filtered As DataRow()
    Dim FilteredTable As New DataTable
    Dim selectQuery As String = ""
    FilteredTable = aircraftTable.Clone 'clone aircraft table

    If amod_airframe_type_code <> "" Then
      selectQuery = "amod_airframe_type_code = '" & amod_airframe_type_code & "'"
    End If

    If amod_type_code <> "" Then
      If amod_airframe_type_code <> "" Then
        selectQuery += " and "
      End If
      selectQuery += " amod_type_code = '" & amod_type_code & "' "
    End If

    filtered = aircraftTable.Select(selectQuery, "")

    For Each atmpDataRow In filtered
      FilteredTable.ImportRow(atmpDataRow)
    Next

    Return FilteredTable
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


        DisplayStr += FillOtherModels(ModelDetails.Rows(0).Item("amod_make_name"))

      End If
    End If


    moreModelInformationLiteral.Text = DisplayStr
  End Sub
  Private Function FillNewsByMake(ByVal MakeString As String, ByVal modelID As Long)
    Dim returnString As String = ""
    Dim ArticleTable As New DataTable
    If modelID = 0 Then
      ArticleTable = Master.AbiDataManager.GetAviationArticlesByMake(MakeString)
    Else
      ArticleTable = Master.AbiDataManager.GetAviationArticlesByModel(modelID)
    End If
    If Not IsNothing(ArticleTable) Then
      If ArticleTable.Rows.Count > 0 Then
        returnString = "<hr /><h4>Latest " & DisplayFunctions.ConvertToTitleCase(MakeString) & " News</h4>"
        newsRepeater.DataSource = ArticleTable
        newsRepeater.DataBind()
      End If
    End If
    Return returnString
  End Function
  Private Function FillOtherModels(ByRef makeName As String) As String
    Dim ReturnStr As String = ""
    Dim OtherModelTable As New DataTable

    OtherModelTable = Master.AbiDataManager.Get_Model_By_Make(makeName)
    If Not IsNothing(OtherModelTable) Then
      If OtherModelTable.Rows.Count > 0 Then
        ReturnStr = "<br /><br /><hr /><span class=""30pxLeftBuffer""><h4>Other " & OtherModelTable.Rows(0).Item("amod_make_name") & " Aircraft Models for Sale</h4></span><div class=""clear_left""></div><div class=""items-row cols-4 row-fluid"">"

        For Each r As DataRow In OtherModelTable.Rows

          ReturnStr += "<span class=""span2""><a href=""" & abi_functions.AircraftModelForSaleURL(r("amod_id"), r("amod_make_name"), r("amod_model_name")) & """>" & r("amod_make_name") & " " & r("amod_model_name") & " (" & r("tcount") & ")</a></span>"

        Next
        ReturnStr += "</div>"
      End If
    End If
    Return ReturnStr
  End Function
  Private Sub FillForSaleAC(ByRef AircraftTable As DataTable, ByRef HeaderTitle As String, ByRef ListingType As String, ByRef DisplayMakeOnly As Boolean)
    Dim DisplayString As String = ""
    Dim KeepingCount As Long = 0
    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then

        If ListingType = "" Then
          DisplayString = "<span class=""span8 30pxLeftBuffer""><h4>" & HeaderTitle & " (####)</h4></span><div class='clearfix'></div>"
        End If

        For Each r As DataRow In AircraftTable.Rows
          DisplayString += "<span class=""span2  30pxLeftBuffer""><a href="""

          If DisplayMakeOnly = False Then
            DisplayString += abi_functions.AircraftModelForSaleURL(r("amod_id"), r("amod_make_name"), r("amod_model_name"))
            DisplayString += """>" & r("amod_make_name") & " "
            DisplayString += r("amod_model_name")
          Else
            DisplayString += "/abiFiles/abiForsale.aspx?type=" & ListingType & "&AirframeType=" & r("amod_airframe_type_code") & "&AirType=" & r("amod_type_code") & "&Make=" & r("amod_make_name") & """>"
            DisplayString += r("amod_make_name") & " "
          End If

          DisplayString += " (" & r("tcount") & ")</a></span>"
          KeepingCount += r("tcount")
        Next
      End If
      DisplayString += "<div class=""clearfix""></div><br />"
      If ListingType = "" Then
        DisplayString = Replace(DisplayString, "####", KeepingCount)
      Else
        ac_header.InnerHtml = DisplayFunctions.ConvertToTitleCase(ListingType) & " " & ac_header.InnerHtml & " (" & KeepingCount & ")"

      End If
    End If

    acListLiteral.Text += DisplayString
  End Sub
  Private Sub FillCompanyWanted(ByVal CompanyID As Long)
    Dim WantedTable As New DataTable
    Dim DisplayString As String = ""


    WantedTable = Master.AbiDataManager.GetABIWantedList("", "", CompanyID)
    If WantedTable.Rows.Count > 0 Then
      DisplayString = "<div class=""items-row row-0 row-fluid"">"
      DisplayString += "<span class=""span10 30pxLeftBuffer""><h4>AIRCRAFT WANTED BY " & CompanyName & ".</h4></span><div class='clearfix'></div>"
      DisplayString += "</div>"
      DisplayString += "<div class=""items-row row-0 row-fluid"">"

      abi_functions.DisplayWanted(WantedTable, "", "", companyWantedLiteral, True)

      DisplayString += companyWantedLiteral.Text
      DisplayString += "</div>"
    End If

    companyWantedLiteral.Text = DisplayString

  End Sub
  Private Sub FillForSaleAircraftPerListingType(ByRef AircraftTable As DataTable, ByRef headerTitle As String, ByRef DealerID As Long)
    Dim DisplayString As String = ""
    Dim css As String = "gray"

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then
        If headerTitle <> "" Then
          DisplayString += "<span class=""span10 30pxLeftBuffer""><h4>" & headerTitle & "</h4></span>"
        End If

        DisplayString += "<div class=""LeftBuffer size11""><span class=""span3 removePaddingLeft""><strong>&nbsp;</strong></span>"
        DisplayString += "<span class=""span4 tiny_header removePaddingLeft"">Year MFR/DLV | Make/Model</span>"
        DisplayString += "<span class=""span2 tiny_header"">Country Of Reg</span>"
        DisplayString += "<span class=""span1 tiny_header cursor"" title=""Total Hours"">Tot.Hrs</span>"
        DisplayString += "<span class=""span2 tiny_header"">Price</span><div class='clearfix'></div></div>"

        For Each r As DataRow In AircraftTable.Rows
          DisplayString += "<div class=""" & css & " LeftBuffer RowSeperator size11""><div class='clearfix'></div>"
          Dim picture As String = ""
          'Aircraft Pic

          If Not IsDBNull(r("ac_picture_id")) Then
            picture = "" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & r("ac_id") & "-0-" & r("ac_picture_id") & ".jpg"
          Else
            If r("amod_airframe_type_code").ToString = "F" Then
              picture = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "abiFiles/images/jet_no_image.jpg"
            Else
              picture = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "abiFiles/images/helo_no_image.jpg"
            End If
          End If

          DisplayString += "<span class=""span3 removePaddingLeft " & css & """ ><img  width=""250"" src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "abiFiles/images/blank.gif"" class=""lazy"" data-src=""" & picture & """ title=""Executive Airliners For Sale, Executive Airliner Aircraft For Sale, Business Executive at JETNET Global"" />"

          DisplayString += "</span>"

          'Year MFR/DLV/Make/Model  , ac_country_of_registration, ac_days_on_market
          DisplayString += "<span class=""span4 removePaddingLeft"">"
          DisplayString += "<figcaption><a href=""" & crmWebClient.abi_functions.AircraftDetailsURL(r("ac_id"), r("ac_year"), r("amod_make_name"), r("amod_model_name"), r("ac_reg_no")) & "/"">" & r("ac_mfr_year").ToString & "/" & r("ac_year").ToString & " | " & r("amod_make_name").ToString & " " & r("amod_model_name") & "</a></figcaption>"
          If DealerID = 0 Then
            DisplayString += "<br /><strong><em>" & r("comp_name") & "</em></strong><br />" & abi_functions.DisplayCompanyInformation(r("comp_id"), r("comp_address1"), r("comp_address2"), r("comp_city"), r("comp_state"), r("comp_zip_code"), r("comp_country"), r("comp_web_address"))
          End If
          DisplayString += "</span>"

          'Reg
          DisplayString += "<span class=""span2"">" & r("ac_country_of_registration").ToString

          DisplayString += "</span>"

          'Total Hours
          DisplayString += "<span class=""span1"">"

          'AFTT
          If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
            If Not String.IsNullOrEmpty(r("ac_airframe_tot_hrs")) Then
              DisplayString += r("ac_airframe_tot_hrs").ToString
            End If
          End If

          DisplayString += "</span>"

          'Price
          DisplayString += "<span class=""span2"">"

          If Not IsDBNull(r("ac_asking")) Then
            If Not String.IsNullOrEmpty(r("ac_asking")) Then

              If UCase(r("ac_asking")) = "PRICE" Then
                DisplayString += FormatCurrency(r("ac_asking_price"), 0)
              Else
                DisplayString += r("ac_asking")
              End If

            End If
          End If


          DisplayString += "</span>"

          DisplayString += "<div class='clearfix'></div></div>"



          If css = "gray" Then
            css = ""
          Else
            css = "gray"
          End If
        Next
      End If
    End If
    DisplayString += "<div class='clearfix'></div><br />"
    acDetailedList.Text += DisplayString
  End Sub
End Class