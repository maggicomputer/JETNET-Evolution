' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiWanteds.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:43a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: abiWanteds.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiWanteds
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim WantedType As String = ""
    Dim WantedTable As New DataTable
    Dim WantedID As Long = 0

    'Type to filter type of wanteds.
    If Not IsNothing(Trim(Request("type"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("type"))) Then
        WantedType = Trim(Request("type"))
        WantedType = Server.UrlDecode(WantedType)
        viewAllDiv.Visible = True
      End If
    End If

    'ID to display details
    If Not IsNothing(Trim(Request("id"))) Then
      If IsNumeric(Trim(Request("id"))) Then
        WantedID = Trim(Request("id"))
        moduleTitleHeader.InnerHtml = "Interested Party"
        typesOfWantedList.Visible = False
        wanted_header.InnerHtml = "WANTED DETAILS"
        viewAllDiv.Visible = True

        aside_right.Attributes.Remove("class")
        aside_right.Attributes.Add("class", "span3")

        component.Attributes.Remove("class")
        component.Attributes.Add("class", "span9")
        Master.ToggleMarketTrendsColumn(False)
      End If
    End If

    Master.Set_Meta_Information("Aircraft and planes wanted including Jets Wanted, Turboprops Wanted, and Helicopters Wanted from aircraft dealers and brokers world-wide.", "Aircraft, Jetnet Global, business directory, aviation, aircraft, mail list, email list, business list, aircraft for sale, aircraft classified, purchase mail list, fbo, dealer, dealers, news, aviation links, aviation events, aviation products, plane, airplane, airline, airport, pilot, pilots, sale, transportation, charter, jetnet")


    If WantedID > 0 Then
      'Display Wanted Details
      DisplayWantedDetails(WantedID)

    ElseIf WantedID = 0 Then 'Display Wanted List.
      Master.Set_Page_Title(IIf(WantedType <> "", WantedType & " ", "") & "Aircraft Wanted at JETNET Global")

      If WantedType <> "" Then


        Select Case LCase(WantedType)
          Case "executive"
            WantedTable = Master.AbiDataManager.GetABIWantedList("F", "E", 0)
            abi_functions.DisplayWanted(WantedTable, "Executive", WantedType, wantedListLiteral, False)
            wanted_header.InnerHtml = "Executive Wanteds"
          Case "turboprops"
            WantedTable = Master.AbiDataManager.GetABIWantedList("F", "T", 0)
            abi_functions.DisplayWanted(WantedTable, "Turboprops", WantedType, wantedListLiteral, False)
            wanted_header.InnerHtml = "Turboprops Wanteds"
          Case "pistons"
            WantedTable = Master.AbiDataManager.GetABIWantedList("F", "P", 0)
            abi_functions.DisplayWanted(WantedTable, "Pistons", WantedType, wantedListLiteral, False)
            wanted_header.InnerHtml = "Pistons Wanteds"
          Case "helicopters"
            WantedTable = Master.AbiDataManager.GetABIWantedList("R", "", 0)
            abi_functions.DisplayWanted(WantedTable, "Helicopters", WantedType, wantedListLiteral, False)
            wanted_header.InnerHtml = "Helicopters Wanteds"
          Case Else 'Jets
            WantedTable = Master.AbiDataManager.GetABIWantedList("F", "J", 0)
            abi_functions.DisplayWanted(WantedTable, "Jets", WantedType, wantedListLiteral, False)
            wanted_header.InnerHtml = "Jets Wanteds"
        End Select
      Else

        'Display Executive
        WantedTable = Master.AbiDataManager.GetABIWantedList("F", "E", 0)
        abi_functions.DisplayWanted(WantedTable, "Executive", WantedType, wantedListLiteral, False)

        'Display Jets
        WantedTable = Master.AbiDataManager.GetABIWantedList("F", "J", 0)
        abi_functions.DisplayWanted(WantedTable, "Jets", WantedType, wantedListLiteral, False)

        'Display Turboprops
        WantedTable = Master.AbiDataManager.GetABIWantedList("F", "T", 0)
        abi_functions.DisplayWanted(WantedTable, "Turboprops", WantedType, wantedListLiteral, False)

        'Display Pistons
        WantedTable = Master.AbiDataManager.GetABIWantedList("F", "P", 0)
        abi_functions.DisplayWanted(WantedTable, "Pistons", WantedType, wantedListLiteral, False)

        'Display Helicopters
        WantedTable = Master.AbiDataManager.GetABIWantedList("R", "", 0)
        abi_functions.DisplayWanted(WantedTable, "Helicopters", WantedType, wantedListLiteral, False)
      End If
    End If

  End Sub
  Private Sub DisplayWantedDetails(ByRef WantedID As Long)
    Dim DisplayString As String = ""
    Dim DisplayDetails As New DataTable

    'Temporarily grabbing random wanted 
    DisplayDetails = Master.AbiDataManager.GetABIWantedDetails(WantedID)

    If Not IsNothing(DisplayDetails) Then
      If DisplayDetails.Rows.Count > 0 Then
        DisplayString = "<span class=""span8"">"
        'Setting up the main header.
        wanted_header.InnerHtml = ""

        'Display Make Name:
        If Not IsDBNull(DisplayDetails.Rows(0).Item("amod_make_name")) Then
          wanted_header.InnerHtml += DisplayDetails.Rows(0).Item("amod_make_name")
          DisplayString += "<strong>Make:</strong> " & DisplayDetails.Rows(0).Item("amod_make_name") & "<br />"
        End If

        'Display Model Name:
        If Not IsDBNull(DisplayDetails.Rows(0).Item("amod_model_name")) Then
          wanted_header.InnerHtml += " " & DisplayDetails.Rows(0).Item("amod_model_name")
          DisplayString += "<strong>Model:</strong> " & DisplayDetails.Rows(0).Item("amod_model_name") & "<br />"
        End If


        Master.Set_Page_Title(wanted_header.InnerHtml & " Wanted at JETNET Global")

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_listed_date")) Then
          DisplayString += "<strong>Date Verified:</strong> " & Format(DisplayDetails.Rows(0).Item("amwant_listed_date"), "MM/dd/yyyy") & "<br />"
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_start_year")) Or Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_end_year")) Then
          DisplayString += "<strong>Year(s) Wanted:</strong> "
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_start_year")) Then
          DisplayString += DisplayDetails.Rows(0).Item("amwant_start_year")
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_end_year")) Then
          DisplayString += "-" & DisplayDetails.Rows(0).Item("amwant_end_year")
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_start_year")) Or Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_end_year")) Then
          DisplayString += "<br /> "
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_max_price")) Then
          DisplayString += "<strong>Max Price:</strong> " & FormatCurrency(DisplayDetails.Rows(0).Item("amwant_max_price"), 0).ToString & " US<br />"
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_accept_damage_hist")) Then
          DisplayString += "<strong>Accept Historical Damage:</strong> " & DisplayAccept(DisplayDetails.Rows(0).Item("amwant_accept_damage_hist")) & "<br />"
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_accept_dam_cur")) Then
          DisplayString += "<strong>Accept Current Damage:</strong> " & DisplayAccept(DisplayDetails.Rows(0).Item("amwant_accept_dam_cur")) & "<br />"
        End If

        If Not IsDBNull(DisplayDetails.Rows(0).Item("amwant_notes")) Then
          DisplayString += "<strong>Notes:</strong> " & DisplayDetails.Rows(0).Item("amwant_notes")
        End If

        DisplayString += "</span>"

        If Not IsDBNull(DisplayDetails.Rows(0).Item("comp_name")) Then
          companyInformation.Text += "<h4 class='size15'>" & DisplayDetails.Rows(0).Item("comp_name") & "</h4>"
        End If

        'Dim EmailContactDisplay As String = ""
        'EmailContactDisplay = Master.AbiDataManager.getCompanyEmail(DisplayDetails.Rows(0).Item("comp_id"))

        'If EmailContactDisplay <> "" Then
        '  companyInformation.Text = "email found"
        'End If

        companyInformation.Text += "<p>" & abi_functions.DisplayCompanyInformation(DisplayDetails.Rows(0).Item("comp_id"), DisplayDetails.Rows(0).Item("comp_address1"), DisplayDetails.Rows(0).Item("comp_address2"), DisplayDetails.Rows(0).Item("comp_city"), DisplayDetails.Rows(0).Item("comp_state"), DisplayDetails.Rows(0).Item("comp_zip_code"), DisplayDetails.Rows(0).Item("comp_country"), DisplayDetails.Rows(0).Item("comp_web_address")) & "</p>"

        'If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
        '  companyInformation.Text += "<img src=""http://www.jetnetGlobal.com/photos/company/" & DisplayDetails.Rows(0).Item("comp_id") & ".jpg"" alt=""" & DisplayDetails.Rows(0).Item("comp_name") & """ />"
        'Else
        '  companyInformation.Text += "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & DisplayDetails.Rows(0).Item("comp_id") & ".jpg"" alt=""" & DisplayDetails.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
        'End If

        '  If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
        companyInformation.Text += "<img src=""" & clsData_Manager_SQL.get_site_name & "/pictures/company/" & DisplayDetails.Rows(0).Item("comp_id") & ".jpg"" alt=""" & DisplayDetails.Rows(0).Item("comp_name") & """ />"
        '  Else
        '    companyInformation.Text += "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" & DisplayDetails.Rows(0).Item("comp_id") & ".jpg"" alt=""" & DisplayDetails.Rows(0).Item("comp_name") & """ />" '"http://www.jetnetGlobal.com/photos/company"
        '  End If


        DisplayString += "<div class=""sayYouSaw"">Say you saw this aircraft on JETNET Global!</div>"


        If Not Page.IsPostBack Then
          'Insert into content stats
          Master.AbiDataManager.Create_ABI_Stats(DisplayDetails.Rows(0).Item("comp_id"), 0, "", WantedID)
        End If

      End If
    End If

    wantedListLiteral.Text += DisplayString

  End Sub

  Private Function DisplayAccept(ByRef acceptVariable As String) As String
    Select Case acceptVariable
      Case "Y"
        DisplayAccept = "Yes"
      Case "N"
        DisplayAccept = "No"
      Case Else
        DisplayAccept = "Unknown"
    End Select
  End Function
  'Private Sub DisplayWanted(ByRef WantedTable As DataTable, ByRef headerTitle As String, ByRef wantedType As String)
  '  Dim DisplayString As String = ""
  '  Dim css As String = "gray"
  '  Dim WantedLink As String = ""
  '  If Not IsNothing(WantedTable) Then
  '    If WantedTable.Rows.Count > 0 Then

  '      If wantedType = "" Then 'if there is no type selected, display the heading
  '        DisplayString += "<span class=""span8 30pxLeftBuffer""><h4>" & headerTitle & "</h4></span><div class='clearfix'></div>"
  '      Else
  '        DisplayString += ""
  '      End If

  '      DisplayString += "<span class=""span3 LeftBuffer""><strong><u>Make/Model</u></strong></span>"
  '      DisplayString += "<span class=""span2""><strong><u>Date Verified</u></strong></span>"
  '      DisplayString += "<span class=""span4""><strong><u>Interested Party</u></strong></span>"
  '      DisplayString += "<span class=""span2""><strong><u>Max Price</strong></u></span>"

  '      For Each r As DataRow In WantedTable.Rows
  '        WantedLink = "?id=" & r("amwant_id")
  '        DisplayString += "<div class='clearfix'></div>"

  '        'Make/Model Column
  '        DisplayString += "<span class=""LeftBuffer span3 " & css & """>"

  '        DisplayString += "<a href=""" & WantedLink & """>"
  '        'Make
  '        If Not IsDBNull(r("amod_make_name")) Then
  '          DisplayString += r("amod_make_name")
  '          DisplayString += " "
  '        End If

  '        'Model
  '        If Not IsDBNull(r("amod_model_name")) Then
  '          DisplayString += r("amod_model_name")
  '        End If

  '        DisplayString += "</a>"
  '        DisplayString += "</span>"

  '        'Date Verified Column
  '        DisplayString += "<span class=""span2 " & css & """>"

  '        If Not IsDBNull(r("amwant_listed_date")) Then
  '          DisplayString += Format(r("amwant_listed_date"), "MM/dd/yyyy")
  '        Else
  '          DisplayString += "&nbsp;"
  '        End If

  '        DisplayString += "</span>"

  '        'Interested Party Column
  '        DisplayString += "<span class=""span4 " & css & """>"

  '        If Not IsDBNull(r("comp_name")) Then
  '          DisplayString += r("comp_name")
  '        Else
  '          DisplayString += "&nbsp;"
  '        End If

  '        DisplayString += "</span>"

  '        'Max Price Column
  '        DisplayString += "<span class=""span2 " & css & """>"

  '        If Not IsDBNull(r("amwant_max_price")) Then
  '          DisplayString += FormatCurrency(r("amwant_max_price"), 0).ToString & " US"
  '        Else
  '          DisplayString += "&nbsp;"
  '        End If

  '        DisplayString += "</span>"

  '        If css = "gray" Then
  '          css = ""
  '        Else
  '          css = "gray"
  '        End If

  '        DisplayString += "<div class='clearfix'></div>"

  '      Next
  '    End If
  '    End If
  '  DisplayString += "<div class='clearfix'></div><br />"

  '    wantedListLiteral.Text += DisplayString
  'End Sub
End Class