' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiAircraftDetails.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:42a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: abiAircraftDetails.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiAircraftDetails

  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim acID As Long = 0
    Dim aircraftData As New DataTable

    If Not IsNothing(Trim(Request("ID"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("ID"))) Then
        acID = Trim(Request("ID"))
        acID = Server.UrlDecode(acID)
      End If
    End If

    Master.ToggleMarketTrendsColumn(False)

    aircraftData = Master.AbiDataManager.GetABIACDetails(acID, 0, 0, "")
    DisplayAircraftInformation(aircraftData)

  End Sub

  Private Sub DisplayAircraftInformation(ByVal aircraftData As DataTable)
    Dim DisplayStr As String = ""
    Dim sliderJS As StringBuilder = New StringBuilder()
    Dim PictureTable As New DataTable
    Dim FeaturesTable As New DataTable
    Dim AvionicsTable As New DataTable
    Dim EngineTable As New datatable
    Dim OtherTables As New DataTable
    Dim InteriorText As String = ""
    Dim ExteriorText As String = ""
    Dim CockpitText As String = ""
    Dim EquipmentText As String = ""
    Dim FeaturesText As String = ""
    Dim MaintenanceText As String = ""
    Dim ABIAircraftStat As String = ""

    If Not IsNothing(aircraftData) Then
      If aircraftData.Rows.Count > 0 Then
        ac_header.InnerText = ""

        If Not Page.IsPostBack Then

          If Not IsDBNull(aircraftData.Rows(0).Item("amod_make_name")) Then
            ABIAircraftStat = aircraftData.Rows(0).Item("amod_make_name").ToString & "_"
          End If
          If Not IsDBNull(aircraftData.Rows(0).Item("amod_model_name")) Then
            ABIAircraftStat += aircraftData.Rows(0).Item("amod_model_name").ToString & "_"
          End If
          If Not IsDBNull(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
            ABIAircraftStat += aircraftData.Rows(0).Item("ac_ser_no_full").ToString
          End If

          Master.AbiDataManager.Create_ABI_Stats(aircraftData.Rows(0).Item("comp_id"), aircraftData.Rows(0).Item("ac_id"), ABIAircraftStat, 0)
        End If
        DisplayStr += "<div class=""items-row col-2 row-fluid""><span class=""span8"">"

        PictureTable = Master.AbiDataManager.GetABIPictures(aircraftData.Rows(0).Item("ac_id"), 0)
        Dim counter As Integer = 0

        If Not IsNothing(PictureTable) Then
          If PictureTable.Rows.Count > 0 Then
            DisplayStr += "<ul class=""bxslider2"">"
            sliderJS.Append("jQuery(function($) {" & vbNewLine)
            sliderJS.Append("$('.bxslider2').bxSlider({" & vbNewLine)
            sliderJS.Append("adaptiveHeight:false," & vbNewLine)
            sliderJS.Append("buildPager: function(slideIndex) {" & vbNewLine)
            sliderJS.Append("switch (slideIndex) {" & vbNewLine)

            For Each r As DataRow In PictureTable.Rows
              DisplayStr += "<li><img src=""" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & aircraftData.Rows(0).Item("ac_id") & "-0-" & r("acpic_id") & ".jpg"" class=""pull-left"" width=""100%""/></li>"

              sliderJS.Append("case " & counter & ":" & vbNewLine)
              sliderJS.Append("return '<img src=""" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & aircraftData.Rows(0).Item("ac_id") & "-0-" & r("acpic_id") & ".jpg"">';" & vbNewLine)

              counter += 1
            Next

            DisplayStr += "</ul>"
            sliderJS.Append("}" & vbNewLine)
            sliderJS.Append("}" & vbNewLine)
            sliderJS.Append("});" & vbNewLine)
            sliderJS.Append(" });" & vbNewLine)

            If Not Page.ClientScript.IsClientScriptBlockRegistered("bxSliderScript") Then
              Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "bxSliderScript", sliderJS.ToString, True)
            End If


          End If
        End If


        DisplayStr += "</span>"

        DisplayStr += Master.AbiDataManager.DisplayRightHandColumn(aircraftData, True)
        DisplayStr += "</div>"


        DisplayStr += "<div class=""clear_left""></div><div class=""clear_left""></div><br /><hr /><div class=""clear_left""></div>"
        DisplayStr += "<div class=""items-row col-2 row-fluid"">"
        'Build Tables
        OtherTables = Master.aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID(aircraftData.Rows(0).Item("ac_id"))
        BuildOtherTableStrings(OtherTables, InteriorText, ExteriorText, CockpitText, EquipmentText, MaintenanceText)

        FeaturesTable = Master.aclsData_Temp.GetJETNET_Aircraft_Details_Key_Features_AC_ID(aircraftData.Rows(0).Item("ac_id"), 0)
        BuildFeaturesTab(FeaturesTable, FeaturesText)

        EngineTable = Master.AbiDataManager.GetABIACEngine(aircraftData.Rows(0).Item("ac_id"), 0)


        'AVIONICS Table.
        DisplayStr += "<span class=""span6 removePaddingLeft pull-left clear_left"">"
        AvionicsTable = Master.aclsData_Temp.GetJETNET_Aircraft_Avionics_AC_ID(aircraftData.Rows(0).Item("ac_id"), 0)
        If Not IsNothing(AvionicsTable) Then
          If AvionicsTable.Rows.Count > 0 Then
            DisplayStr += "<h4 class=""bold"">Avionics</h4>"
            For Each r As DataRow In AvionicsTable.Rows
              If Not IsDBNull(r("av_name")) Then
                DisplayStr += "<b>" & r("av_name") & "</b> - "
              End If
              If Not IsDBNull(r("av_description")) Then
                DisplayStr += r("av_description")
              End If
              DisplayStr += "<br />"
            Next
            DisplayStr += "<br />"
          End If
        End If


        If Not String.IsNullOrEmpty(CockpitText) Then
          DisplayStr += "<h4 class=""bold"">Additional Cockpit Equipment</h4>"
          DisplayStr += CockpitText
          DisplayStr += "<br />"
        End If

        If Not String.IsNullOrEmpty(EquipmentText) Then
          DisplayStr += "<h4 class=""bold"">Equipment</h4>"
          DisplayStr += EquipmentText
          DisplayStr += "<br />"
        End If

        If Not String.IsNullOrEmpty(MaintenanceText) Then
          DisplayStr += "<h4 class=""bold"">Maintenance</h4>"
          DisplayStr += MaintenanceText
          DisplayStr += "<br />"
        End If

        DisplayStr += "</span>"


        DisplayStr += "<span class=""span6 removePaddingLeft pull-right clear_right"">"

        If Not String.IsNullOrEmpty(InteriorText) Then
          DisplayStr += "<h4 class=""bold"">Interior</h4>"
          DisplayStr += InteriorText
          DisplayStr += "<br />"
        End If

        If Not String.IsNullOrEmpty(ExteriorText) Then
          DisplayStr += "<h4 class=""bold"">Exterior "
          DisplayStr += "</h4>"
          DisplayStr += ExteriorText
          DisplayStr += "<br />"
        End If

        If Not String.IsNullOrEmpty(FeaturesText) Then
          DisplayStr += "<h4 class=""bold"">Aircraft Features</h4>"
          DisplayStr += FeaturesText
          DisplayStr += "<br />"
        End If
        DisplayStr += "</span>"


        DisplayStr += "<span class=""span12 removePaddingLeft""><br /><h4 class=""bold"">Engine Information</h4>"
        DisplayStr += (CommonAircraftFunctions.DisplayEngineInfo_Vertical(Me.Session, EngineTable, Nothing))
        DisplayStr += "</span>"

        'DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Type:</strong></span>"
        'DisplayStr += "<span class=""span4"">"

        'If (aircraftData.Rows(0).Item("amod_airframe_type_code") = "R") Then
        '  DisplayStr += "Helicopter"
        'ElseIf (aircraftData.Rows(0).Item("amod_type_code") = "J") Then
        '  DisplayStr += "Jet"
        'ElseIf (aircraftData.Rows(0).Item("amod_type_code") = "T") Then
        '  DisplayStr += "TurboProp"
        'ElseIf (aircraftData.Rows(0).Item("amod_type_code") = "P") Then
        '  DisplayStr += "Piston"
        'ElseIf (aircraftData.Rows(0).Item("amod_type_code") = "E") Then
        '  DisplayStr += "Executive"
        'End If

        'DisplayStr += "</span>"


        'Year
        'DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Year Mfr/Dlv:</strong></span>"
        'DisplayStr += "<span class=""span4"">"
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_mfr_year")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_mfr_year")) Then
        '    DisplayStr += aircraftData.Rows(0).Item("ac_mfr_year").ToString
        '  End If
        'End If

        'DisplayStr += " / "
        If Not IsDBNull(aircraftData.Rows(0).Item("ac_year")) Then
          If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_year")) Then
            'DisplayStr += aircraftData.Rows(0).Item("ac_year").ToString
            ac_header.InnerText += aircraftData.Rows(0).Item("ac_year").ToString & " "
          End If
        End If

        'DisplayStr += "</span>"

        'Make
        If Not IsDBNull(aircraftData.Rows(0).Item("amod_make_name")) Then
          If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_make_name")) Then
            'DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Make:</strong></span>"
            'DisplayStr += "<span class=""span4"">"
            'DisplayStr += aircraftData.Rows(0).Item("amod_make_name").ToString
            'DisplayStr += "</span>"
            ac_header.InnerText += aircraftData.Rows(0).Item("amod_make_name").ToString & " "
          End If
        End If

        'Model
        If Not IsDBNull(aircraftData.Rows(0).Item("amod_model_name")) Then
          If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_model_name")) Then
            'DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Model:</strong></span>"
            'DisplayStr += "<span class=""span4"">"
            'DisplayStr += aircraftData.Rows(0).Item("amod_model_name").ToString
            'DisplayStr += "</span>"
            ac_header.InnerText += aircraftData.Rows(0).Item("amod_model_name").ToString
          End If
        End If

        ac_header.InnerText += " For Sale "

        'Company
        If Not IsDBNull(aircraftData.Rows(0).Item("comp_name")) Then
          If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("comp_name")) Then
            ac_header.InnerText += "by " & aircraftData.Rows(0).Item("comp_name").ToString
          End If
        End If

        ''Ser #
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Serial #:</strong></span>"
        '    DisplayStr += "<span class=""span4"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_ser_no_full").ToString
        '    DisplayStr += "</span>"
        '  End If
        'End If

        ''Reg #
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_reg_no")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_reg_no")) Then
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Reg #:</strong></span>"
        '    DisplayStr += "<span class=""span4"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_reg_no").ToString
        '    DisplayStr += "</span>"
        '  End If
        'End If

        'Status
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_status")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_status")) Then
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Status:</strong></span>"
        '    DisplayStr += "<span class=""span4"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_status")
        '    DisplayStr += "</span>"
        '  End If
        'End If

        ''Asking
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_asking")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_asking")) Then

        '    If UCase(aircraftData.Rows(0).Item("ac_asking")) = "PRICE" Then
        '      DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Asking Price:</strong></span>"
        '      DisplayStr += "<span class=""span4"">"
        '      DisplayStr += FormatCurrency(aircraftData.Rows(0).Item("ac_asking_price"), 0)
        '      DisplayStr += "</span>"
        '    Else
        '      DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Asking:</strong></span>"
        '      DisplayStr += "<span class=""span4"">"
        '      DisplayStr += aircraftData.Rows(0).Item("ac_asking")
        '      DisplayStr += "</span>"
        '    End If

        '  End If
        'End If


        'Date Listed
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_list_date")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_list_date")) Then
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Date Listed:</strong></span>"
        '    DisplayStr += "<span class=""span4"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_list_date")
        '    DisplayStr += "</span>"
        '  End If
        'End If


        'AFTT
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_airframe_tot_hrs")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_airframe_tot_hrs")) Then
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>AFTT:</strong></span>"
        '    DisplayStr += "<span class=""span4"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_airframe_tot_hrs").ToString
        '    DisplayStr += "</span>"
        '  End If
        'End If

        ''Delivery Position/Date
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_lifecycle_stage")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_lifecycle_stage")) Then
        '    If aircraftData.Rows(0).Item("ac_lifecycle_stage") = "1" Then
        '      DisplayStr += "<span class=""span2 removePaddingLeft""><strong>DELIVERY POSITION:</strong></span>"
        '      DisplayStr += "<span class=""span4"">"
        '      DisplayStr += "</span>"

        '      If Not IsDBNull(aircraftData.Rows(0).Item("ac_delivery_date")) Then
        '        If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_delivery_date")) Then
        '          DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Delivery Date:</strong></span>"
        '          DisplayStr += "<span class=""span4"">"
        '          DisplayStr += aircraftData.Rows(0).Item("ac_delivery_date")
        '          DisplayStr += "</span>"
        '        End If
        '      End If

        '    End If
        '  End If
        'End If

        ''Note
        'If Not IsDBNull(aircraftData.Rows(0).Item("ac_confidential_notes")) Then
        '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_confidential_notes")) Then
        '    DisplayStr += "<div class=""clear_left""></div><br />"
        '    DisplayStr += "<span class=""span2 removePaddingLeft""><strong>Notes:</strong></span>"
        '    DisplayStr += "<span class=""span8"">"
        '    DisplayStr += aircraftData.Rows(0).Item("ac_confidential_notes")
        '    DisplayStr += "</span>"
        '  End If
        'End If
        DisplayStr += "</div>"

        acInformation.Text = DisplayStr
        Master.Set_Page_Title(ac_header.InnerText)
      End If
    End If
  End Sub
  Private Sub BuildOtherTableStrings(ByVal EquipmentTable As DataTable, ByRef InteriorText As String, ByRef ExteriorText As String, ByRef CockpitText As String, ByRef EquipmentText As String, ByRef MaintenanceText As String)
    For Each r As DataRow In EquipmentTable.Rows
      If Trim(r("adet_data_type")) = "Interior" Then
        InteriorText = InteriorText & "<b>" & r("adet_data_name") & "</b> - "
        InteriorText = InteriorText & r("adet_data_description") & "<br />"
      ElseIf Trim(r("adet_data_type")) = "Exterior" Then
        ExteriorText = ExteriorText & "<b>" & r("adet_data_name") & "</b> - "
        ExteriorText = ExteriorText & r("adet_data_description") & "<br />"
      ElseIf Trim(r("adet_data_type")) = "Addl Cockpit Equipment" Then
        CockpitText = CockpitText & "<b>" & r("adet_data_name") & "</b> - "
        CockpitText = CockpitText & r("adet_data_description") & "<br />"
      ElseIf Trim(r("adet_data_type")) = "Equipment" Then
        EquipmentText = EquipmentText & "<b>" & r("adet_data_name") & "</b> - "
        EquipmentText = EquipmentText & r("adet_data_description") & "<br />"
      ElseIf Trim(r("adet_data_type")) = "Maintenance" Then
        MaintenanceText = MaintenanceText & "<b>" & r("adet_data_name") & "</b> - "
        MaintenanceText = MaintenanceText & r("adet_data_description") & ""
      End If
    Next
  End Sub

  Private Sub BuildFeaturesTab(ByVal FeaturesTable As DataTable, ByRef FeaturesText As String)
    For Each r As DataRow In FeaturesTable.Rows

      If r("kff_name") = "Yes" Or r("kff_name") = "Y" Then
        FeaturesText += "<span class='em' title='Yes'><img src='/images/green_check.gif' alt='Yes' /></span> "
     

        FeaturesText += r("kfeat_name")
        'FeaturesText += " <b class=""tiny"">(" & r("kfeat_type") & ")</b> "
        FeaturesText += "<br />"
      End If
    Next
  End Sub
  
End Class