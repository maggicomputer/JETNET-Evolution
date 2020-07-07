' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/resdiualMarketValue.aspx.vb $
'$$Author: Amanda $
'$$Date: 5/15/20 4:15p $
'$$Modtime: 5/15/20 1:04p $
'$$Revision: 7 $
'$$Workfile: resdiualMarketValue.aspx.vb $
'
' ********************************************************************************

Partial Public Class resdiualMarketValue
    Inherits System.Web.UI.Page


    Private localCriteria As New viewSelectionCriteriaClass
    Public Shared masterPage As New Object


    Public productCodeCount As Integer = 0
    Public isHeliOnlyProduct As Boolean = False

    Private sTypeMakeModelCtrlBaseName As String = "AircraftView"

    Private bHasHelicopterFilter As Boolean = False
    Private bHasBusinessFilter As Boolean = False
    Private bHasCommercialFilter As Boolean = False
    Private bHasRegionalFilter As Boolean = False
    Private bHasYachtFilter As Boolean = False

    Private bClearView As Boolean = False
    Dim comp_functions As New CompanyFunctions
    Public SharedModelTable As New DataTable
    Dim isValueChartRefresh As Boolean = False
    Dim stringToExclude As String = ""
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        load_view_session_variables()
    End Sub
    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
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

        Try
            isValueChartRefresh = HttpContext.Current.Request.Form(tabPanel1GraphButton.UniqueID) IsNot Nothing
            Dim sErrorString As String = ""

            If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                Response.Write("** Error in [View_Master.ascx.vb :  load preferences : " + sErrorString)
            End If

            For nloop As Integer = 0 To UBound(Session.Item("localPreferences").ProductCode)

                Select Case Session.Item("localPreferences").ProductCode(nloop)
                    Case eProductCodeTypes.H
                        productCodeCount += 1
                    Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
                        productCodeCount += 1
                    Case eProductCodeTypes.R
                    Case eProductCodeTypes.C
                        productCodeCount += 1
                    Case eProductCodeTypes.P
                    Case eProductCodeTypes.A
                    Case eProductCodeTypes.Y

                End Select

            Next

            isHeliOnlyProduct = HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Residual Market Forecast")
            masterPage.SetPageTitle("Residual Market Forecast")
            masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page
            Dim resdiual_functions As New resdiualMarketValueDataLayer
            resdiual_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            resdiual_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            resdiual_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            resdiual_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            resdiual_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            breadcrumbs1.Text = "<strong>Residual Market Forecast</strong>"

            ' buttons1.Text = "<a class=""underline cursor"" onclick=""javascript:window.close();return false;"" class=""close_button""><img src=""/images/x.svg"" alt=""Close"" /></a>"



            ViewTMMDropDowns.setIsView(True)
            ViewTMMDropDowns.setShowWeightClass(True)
            ViewTMMDropDowns.setListSize(6)
            ViewTMMDropDowns.setOverideDefaultModel(False)
            ViewTMMDropDowns.setOverideMultiSelect(True)
            ViewTMMDropDowns.setControlName(sTypeMakeModelCtrlBaseName)

            commonEvo.fillAirframeArray("")
            commonEvo.fillAircraftTypeLableArray("")

            Dim amod_string As String = ""
            Dim year_string As String = ""

            If Page.IsPostBack Then
                tab1Panel.Visible = True
                PanelCollapseEx1.Collapsed = True
                PanelCollapseEx1.ClientState = "True"
                load_page_variables()
                ' mainHeader.Visible = True
                main_tab_container.Visible = True
                If isValueChartRefresh = False Then
                    Me.table_label.Text = run_asset_model_summary()
                End If
            Else
                tab1Panel.Visible = False
                'mainHeader.Visible = False
                main_tab_container.Visible = False
                PanelCollapseEx1.Collapsed = False
                PanelCollapseEx1.ClientState = "False"
            End If




            If Not IsPostBack Then
                Dim i As Integer = 0
                Me.year_start.Items.Add(New ListItem("All", ""))
                Me.year_end.Items.Add(New ListItem("All", ""))
                For i = 1970 To 2018
                    Me.year_start.Items.Add(i)
                    Me.year_end.Items.Add(i)
                Next



                Call commonLogFunctions.Log_User_Event_Data("UserDisplayView", "User Entered View " & Replace(commonEvo.Get_Default_User_View(31), "&nbsp;", " "), Nothing, 31, localCriteria.ViewCriteriaJournalID, 0, localCriteria.ViewCriteriaAircraftID, 0, localCriteria.ViewCriteriaAircraftID, localCriteria.ViewCriteriaAmodID)

            End If

            Dim jsStr As String = ""

            jsStr = "$(document).ready(function(){" + vbCrLf
            ' jsStr += "setUpLinkHover();setUpAutoComplete();" & vbNewLine
            jsStr += "setTimeout(function(){"
            jsStr += BuildTable()
            jsStr += "; }, 1000);"


            jsStr += " if ($('#" & split_by_year.ClientID & "').is("":checked"")) {"
            jsStr += "$('#" & year_end.ClientID & "').removeClass(""display_none"");"
            jsStr += " } else { "
            jsStr += "$('#" & year_end.ClientID & "').addClass(""display_none"");"
            jsStr += " };"


            jsStr += "});" & vbNewLine

            System.Web.UI.ScriptManager.RegisterStartupScript(tabPanel1Update, tabPanel1Update.GetType(), "StartupScr", jsStr, True)

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_Load): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_Load): " + ex.Message.ToString.Trim
            End If
        End Try



    End Sub
    Public Sub load_page_variables()

        Dim sAirFrame As String = ""
        Dim sAirType As String = ""
        Dim sMake As String = ""
        Dim sModel As String = ""
        Dim sUsage As String = ""
        Dim amod_product As String = ""
        Dim multiProduct As Array = Nothing
        Dim type_search_variable_only As String = ""
        Dim type_search_string_variable_only As String = ""
        Dim StringMakeModelName As String = ""
        Dim FirstModel As Long = 0
        Try



            ' load type/make/model boxes
            If Not String.IsNullOrEmpty(Session.Item("viewAircraftModel").ToString.Trim) Then

                Dim modelArray() As String = Nothing
                Dim tmpModelArray() As String = Nothing

                ' Check and see if user selected more than one model
                If Session.Item("viewAircraftModel").ToString.Contains(Constants.cCommaDelim) Then
                    modelArray = Session.Item("viewAircraftModel").ToString.Split(Constants.cCommaDelim)

                    If IsArray(modelArray) And Not IsNothing(modelArray) Then
                        ' translate index into actual amod_id

                        ReDim tmpModelArray(UBound(modelArray))

                        For x As Integer = 0 To UBound(modelArray)
                            ' translate index into actual amod_id
                            tmpModelArray(x) = commonEvo.ReturnAmodIDForItemIndex(CLng(modelArray(x)))
                            If x = 0 Then
                                FirstModel = tmpModelArray(x)
                            Else
                                If StringMakeModelName <> "" Then
                                    StringMakeModelName += ", "
                                End If
                                StringMakeModelName += tmpModelArray(x)
                            End If
                            If commonEvo.ReturnModelDataFromIndex(CLng(modelArray(x)), sAirFrame, sAirType, sMake, sModel, sUsage) Then

                                If String.IsNullOrEmpty(localCriteria.ViewCriteriaAircraftModel.Trim) Then
                                    localCriteria.ViewCriteriaAircraftModel = sModel
                                    'StringMakeModelName = sMake & " " & sModel

                                Else
                                    localCriteria.ViewCriteriaAircraftModel += Constants.cCommaDelim + sModel
                                    'StringMakeModelName += Constants.cCommaDelim & " " & sMake & " " & sModel
                                End If
                            End If
                        Next

                        localCriteria.ViewCriteriaAmodIDArray = tmpModelArray
                        'mainHeader.InnerHtml = "<strong>" & StringMakeModelName & "</strong> Residual Values"

                        '   residualValueChartForm(FirstModel, StringMakeModelName)
                    End If

                Else

                    ' translate index into actual amod_id
                    localCriteria.ViewCriteriaAmodID = commonEvo.ReturnAmodIDForItemIndex(CLng(Session.Item("viewAircraftModel").ToString))

                    ' residualValueChartForm(localCriteria.ViewCriteriaAmodID, "")

                    If commonEvo.ReturnModelDataFromIndex(CLng(Session.Item("viewAircraftModel").ToString), sAirFrame, sAirType, sMake, sModel, sUsage) Then
                        localCriteria.ViewCriteriaAircraftModel = sModel
                        localCriteria.ViewCriteriaAircraftType = sAirType
                    End If

                End If

            Else
                localCriteria.ViewCriteriaAmodIDArray = Nothing
                localCriteria.ViewCriteriaAmodID = -1
                localCriteria.ViewCriteriaAircraftModel = ""
                Session.Item("viewAircraftModel") = ""
            End If

            If Not String.IsNullOrEmpty(Session.Item("viewAircraftMake").ToString.Trim) Then

                Dim makeArray() As String = Nothing
                Dim tmpMakeArray() As String = Nothing

                ' Check and see if user selected more than one make
                If Session.Item("viewAircraftMake").ToString.Contains(Constants.cCommaDelim) Then

                    makeArray = Session.Item("viewAircraftMake").ToString.Split(Constants.cCommaDelim)

                    If IsArray(makeArray) And Not IsNothing(makeArray) Then

                        ReDim tmpMakeArray(UBound(makeArray))

                        For x As Integer = 0 To UBound(makeArray)
                            ' translate index into actual amod_make_id
                            tmpMakeArray(x) = commonEvo.ReturnAmodIDForItemIndex(CLng(makeArray(x)))
                            If commonEvo.ReturnModelDataFromIndex(CLng(makeArray(x)), sAirFrame, sAirType, sMake, sModel, sUsage) Then
                                If String.IsNullOrEmpty(localCriteria.ViewCriteriaAircraftMake.Trim) Then
                                    localCriteria.ViewCriteriaAircraftMake = sMake
                                Else
                                    localCriteria.ViewCriteriaAircraftMake += Constants.cCommaDelim + sMake
                                End If
                            End If
                        Next

                        localCriteria.ViewCriteriaMakeIDArray = tmpMakeArray

                    End If

                Else
                    ' translate index into actual amod_make_id
                    localCriteria.ViewCriteriaMakeAmodID = commonEvo.ReturnAmodIDForItemIndex(CLng(Session.Item("viewAircraftMake")))

                    If commonEvo.ReturnModelDataFromIndex(CLng(Session.Item("viewAircraftMake").ToString), sAirFrame, sAirType, sMake, sModel, sUsage) Then
                        localCriteria.ViewCriteriaAircraftMake = sMake

                        Select Case (sAirType)
                            Case Constants.AMOD_TYPE_AIRLINER
                                localCriteria.ViewCriteriaAirframeType = Constants.VIEW_EXECUTIVE
                            Case Constants.AMOD_TYPE_JET
                                localCriteria.ViewCriteriaAirframeType = Constants.VIEW_JETS
                            Case Constants.AMOD_TYPE_TURBO
                                Select Case (sAirFrame)
                                    Case Constants.AMOD_FIXED_AIRFRAME
                                        localCriteria.ViewCriteriaAirframeType = Constants.VIEW_TURBOPROPS
                                    Case Constants.AMOD_ROTARY_AIRFRAME
                                        localCriteria.ViewCriteriaAirframeType = Constants.VIEW_HELICOPTERS

                                End Select
                            Case Constants.AMOD_TYPE_PISTON
                                Select Case (sAirFrame)
                                    Case Constants.AMOD_FIXED_AIRFRAME
                                        localCriteria.ViewCriteriaAirframeType = Constants.VIEW_PISTONS
                                    Case Constants.AMOD_ROTARY_AIRFRAME
                                        localCriteria.ViewCriteriaAirframeType = Constants.VIEW_HELICOPTERS
                                End Select
                        End Select

                    End If
                End If

            Else
                localCriteria.ViewCriteriaMakeIDArray = Nothing
                localCriteria.ViewCriteriaMakeAmodID = -1
                localCriteria.ViewCriteriaAircraftMake = ""
                localCriteria.ViewCriteriaAirframeType = Constants.VIEW_ALLAIRFRAME
                Session.Item("viewAircraftMake") = ""
            End If

            If String.IsNullOrEmpty(Session.Item("viewAircraftMake").ToString.Trim) Then
                If String.IsNullOrEmpty(Session.Item("viewAircraftModel").ToString.Trim) Then
                    If Trim(Session.Item("viewAircraftType")) <> "" Then

                        If commonEvo.ReturnModelDataFromIndex(CLng(Session.Item("viewAircraftType").ToString), sAirFrame, sAirType, sMake, sModel, sUsage) Then
                            ' localCriteria.ViewCriteriaAircraftType = sAirType
                            type_search_variable_only = sAirType
                            type_search_string_variable_only = sAirFrame

                            Select Case (type_search_variable_only)
                                Case Constants.AMOD_TYPE_AIRLINER
                                    localCriteria.ViewCriteriaAirframeType = Constants.VIEW_EXECUTIVE
                                Case Constants.AMOD_TYPE_JET
                                    localCriteria.ViewCriteriaAirframeType = Constants.VIEW_JETS
                                Case Constants.AMOD_TYPE_TURBO
                                    Select Case (sAirFrame)
                                        Case Constants.AMOD_FIXED_AIRFRAME
                                            localCriteria.ViewCriteriaAirframeType = Constants.VIEW_TURBOPROPS
                                        Case Constants.AMOD_ROTARY_AIRFRAME
                                            localCriteria.ViewCriteriaAirframeType = Constants.VIEW_HELICOPTERS

                                    End Select
                                Case Constants.AMOD_TYPE_PISTON
                                    Select Case (sAirFrame)
                                        Case Constants.AMOD_FIXED_AIRFRAME
                                            localCriteria.ViewCriteriaAirframeType = Constants.VIEW_PISTONS
                                        Case Constants.AMOD_ROTARY_AIRFRAME
                                            localCriteria.ViewCriteriaAirframeType = Constants.VIEW_HELICOPTERS

                                    End Select
                            End Select

                        End If
                    End If
                End If
            End If

            'If localCriteria.ViewCriteriaAmodID = -1 And localCriteria.ViewCriteriaMakeAmodID > -1 Then

            '  'localCriteria.ViewCriteriaAmodID = localCriteria.ViewCriteriaMakeAmodID
            '  'Session.Item("viewAircraftModel") = commonEvo.FindIndexForItemByAmodID(localCriteria.ViewCriteriaAmodID)
            '  'Session.Item("viewAircraftMake") = localCriteria.ViewCriteriaMakeAmodID ' Session.Item("viewAircraftModel")
            '  'Session.Item("viewAircraftType") = Session.Item("viewAircraftModel")

            '  If commonEvo.ReturnModelDataFromIndex(CLng(Session.Item("viewAircraftModel").ToString), sAirFrame, sAirType, sMake, sModel, sUsage) Then
            '    localCriteria.ViewCriteriaAirframeTypeStr = sAirFrame
            '    localCriteria.ViewCriteriaAircraftType = sAirType
            '    localCriteria.ViewCriteriaAircraftModel = sModel

            '  End If

            'End If


            If Not IsNothing(HttpContext.Current.Session.Item("localPreferences")) And localCriteria.ViewCriteriaAmodID = -1 And localCriteria.ViewCriteriaMakeAmodID = -1 Then

                '   commonEvo.fillAirframeArray("")
                '   commonEvo.fillAircraftTypeLableArray("")


                If IsNothing(localCriteria.ViewCriteriaAmodIDArray) And String.IsNullOrEmpty(localCriteria.ViewCriteriaAircraftMake) And IsNothing(localCriteria.ViewCriteriaTypeIDArray) Then


                    If IsNumeric(HttpContext.Current.Session.Item("localPreferences").DefaultModel) And CLng(HttpContext.Current.Session.Item("localPreferences").DefaultModel.ToString) > -1 Then
                        localCriteria.ViewCriteriaAmodID = HttpContext.Current.Session.Item("localPreferences").DefaultModel
                    Else
                        ' ADDED MSW - 8/4/15---------
                        ' CHECKS AND MAKES SURE YOU CAN SEE THE DEFAULT OR SELECTED MODEL
                        Call check_and_reset_model()
                    End If

                    Session.Item("viewAircraftModel") = commonEvo.FindIndexForItemByAmodID(localCriteria.ViewCriteriaAmodID)
                    Session.Item("viewAircraftMake") = Session.Item("viewAircraftModel")
                    Session.Item("viewAircraftType") = Session.Item("viewAircraftModel")


                    If commonEvo.ReturnModelDataFromIndex(CLng(Session.Item("viewAircraftModel").ToString), sAirFrame, sAirType, sMake, sModel, sUsage) Then
                        localCriteria.ViewCriteriaAirframeTypeStr = sAirFrame
                        localCriteria.ViewCriteriaAircraftType = sAirType
                        localCriteria.ViewCriteriaAircraftModel = sModel
                    End If

                End If
            End If




            If isValueChartRefresh = False Then
                residualValueChartForm(localCriteria, localCriteria.ViewCriteriaAmodID, "", True, tab1Update)

            End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub residualValueChartForm(ByVal localCriteria As viewSelectionCriteriaClass, ByVal modelID As Long, ByVal modelString As String, ByVal checkForDomLoad As Boolean, ByVal updatePanelToUpdate As UpdatePanel, Optional ByVal stringToExclude As String = "")
        Dim YearString As String = ""
        Dim residualMarketValue As New resdiualMarketValueDataLayer
        Dim google_map_string As String = ""
        residualValueChart.Visible = False
        residualMarketValue.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        residualMarketValue.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        residualMarketValue.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        residualMarketValue.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        residualMarketValue.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
        YearString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(year_end, True, 0, True)

        residualValueChart.Text = "<table class=""formatTable blue large"" width=""100%""><tr class=""noBorder""><td align=""left"" valign=""top"">"

        residualValueChart.Text += "<span class=""subHeader"">RESIDUAL VALUES " & IIf(split_by_year.Checked, "BY DLV YEAR", "") & "</span></td></tr><tr><td align=""left"" valign=""top"">"

        Call residualMarketValue.FillResidualGraph(split_by_year, residualValueChart, localCriteria, "RESIDUAL", modelID, residualValueChart.Text, updatePanelToUpdate, 1, 0, 0, 350, 0, True, True, True, "", "N", "", "", "", YearString, "", "", "", "", google_map_string, modelString, True, checkForDomLoad, False, stringToExclude)
        resizeScript(1)
        residualValueChart.Text += "</td></tr></table>"


        HttpContext.Current.Session.Item("Residual_Chart_Java") = ""
        HttpContext.Current.Session.Item("Residual_Chart_Java") = google_map_string

        ViewACResidualByMFR.Text = "<br/><a href=""#"" onclick=""javascript:load('largeGraphDisplay.aspx?&Residual=Y&graph_type=RESIDUAL&page_title=RESIDUAL MARKET VALUES','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">Enlarge Graph</a>"

    End Sub
    Public Function generateAvgGauge(ByVal minRes As Double, ByVal avgRes As Double, ByVal maxRes As Double) As StringBuilder
        Dim htmlOut As New StringBuilder
        Dim jsScr As New StringBuilder

        residualGaugeChart.Visible = True
        chartCSSResize.Attributes.Remove("class")
        chartCSSResize.Attributes.Add("class", "columns eight setUpLeftMargin")
        jsScr.Append(" function initGauge_AVG() { ")

        jsScr.Append(" var gauge = new RadialGauge({ renderTo:  'avgCount',")
        jsScr.Append(" width: 275, height: 275, units: false,")
        jsScr.Append(" fontTitleSize: ""34"",")
        jsScr.Append(" fontTitle:""Arial"",")
        jsScr.Append("colorTitle:  '#4f5050',")

        jsScr.Append(" title: """ & FormatNumber(avgRes, 1).ToString & "%"", ")
        jsScr.Append("  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, ")
        jsScr.Append("  minValue: " & minRes.ToString & ",  maxValue: " & maxRes.ToString & ",")
        jsScr.Append(" majorTicks: false, minorTicks: 0,strokeTicks: false,")
        jsScr.Append(" colorUnits: ""#000000"",")
        jsScr.Append(" fontUnitsSize: ""30"",")
        jsScr.Append("highlights: false,animation: false,")
        jsScr.Append("barWidth: 25,")
        jsScr.Append("barProgress: true,")
        jsScr.Append("colorBarProgress:  '#078fd7',")
        jsScr.Append("needle: false,")
        jsScr.Append("colorBar:  '#eee',")
        jsScr.Append("colorStrokeTicks: '#fff',")
        jsScr.Append("numbersMargin: -18,")
        jsScr.Append("  colorPlate: ""rgba(0,0,0,0)"",") 'Make background transparent.
        jsScr.Append("    borderShadowWidth: 0,")
        jsScr.Append("    borders: false,")
        jsScr.Append("    value: " & avgRes.ToString & ",")
        jsScr.Append("}).draw();")


        jsScr.Append(" };initGauge_AVG();")


        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "gaugeString", jsScr.ToString, True)


        Return htmlOut
    End Function
    Public Sub CheckSharedModelTable()
        'All that this does is check for the existence of the shared model table - basically so you don't use it if it doesn't exist.
        'It returns nothing but fills it up if it's not there.
        If Not IsNothing(SharedModelTable) Then
            If SharedModelTable.Rows.Count = 0 Then
                'We need to fill this up. If it has rows already, it already exists - so we're good to go.
                SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
            End If
        Else
            SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
        End If
    End Sub
    Public Sub check_and_reset_model()
        Dim ComparableID As Long = 0
        If Not IsNothing(Request.Item("ViewID")) Then
            If Not String.IsNullOrEmpty(Request.Item("ViewID").ToString) Then
                ComparableID = Trim(Request.Item("ViewID"))
            End If
        End If
        If ComparableID <> 28 Then
            CheckSharedModelTable()

            If IsNothing(SharedModelTable) Then 'We dont have a shared model table set a defaut
                If isHeliOnlyProduct Then
                    localCriteria.ViewCriteriaAmodID = 442
                    Exit Sub
                End If

                If Session.Item("localPreferences").Tierlevel = 1 Then   'jets exec
                    If Session.Item("localPreferences").UserBusinessFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 272   ' challenger 300 - business jet
                    ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 698 '  AIRBUS  A300B2K-200  -  commercial jet 
                    ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 442
                    End If
                ElseIf Session.Item("localPreferences").Tierlevel = 2 Then 'pistons turbos
                    If Session.Item("localPreferences").UserBusinessFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 180   ' caravan 208 - business turbo
                    ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 181 ' caravan 208B  -  commercial turbo 
                    ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 408 ' augusta westland aw139 - helicopter 
                    End If
                Else
                    If Session.Item("localPreferences").UserBusinessFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 272   ' challenger 300 - business jet
                    ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 698 ' boeng bbj -  commercial jet 
                    ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
                        localCriteria.ViewCriteriaAmodID = 408 ' augusta westland aw139 - helicopter 
                    End If
                End If
            End If
        End If
    End Sub
    Private Function BuildTable() As String
        Dim tableBuild As New StringBuilder
        Dim footerBuild As New StringBuilder
        Dim bottomNumber As Integer = 6
        Dim topNumber As Integer = 8


        tableBuild.Append("var cw = $('.aircraftContainer').width() - 20;")
        tableBuild.Append("$("".resizeDiv"").width(cw);")

        tableBuild.Append("$(window).resize(function() {")
        tableBuild.Append("var cw = $('.aircraftContainer').width() - 20;")
        tableBuild.Append("$("".resizeDiv"").width(cw);")
        tableBuild.Append("});")
        tableBuild.Append("var hideFromExport = [];var table = $('#residualForecast').DataTable({destroy:true,")
        tableBuild.Append("dom:        'Bitrp',")
        tableBuild.Append("scrollY:        530, ")
        tableBuild.Append("scrollX:        cw, ")
        tableBuild.Append("scrollCollapse: true, ")
        tableBuild.Append("scroller:       true, ")
        If split_by_year.Checked Then
            tableBuild.Append("""order"": [[ 3, ""desc"" ]],")
        Else
            tableBuild.Append("""order"": [[ 4, ""desc"" ]],")
        End If
        tableBuild.Append(" ""language"": {")
        tableBuild.Append("""emptyTable"": ""No Residual Market Forecast Values"",")
        tableBuild.Append("""info"": ""_TOTAL_ Residual Market Forecast Value(s)"",")
        tableBuild.Append("""infoEmpty"": ""0 Residual Market Forecast Values""")
        tableBuild.Append("},")


        tableBuild.Append("columnDefs: [")
        tableBuild.Append("{")
        tableBuild.Append(" className: 'display_none',")
        tableBuild.Append("""name"": 'id',")
        tableBuild.Append("""targets"": 1")
        tableBuild.Append("}, ")
        tableBuild.Append("{ orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }")
        tableBuild.Append("],")
        tableBuild.Append("select: { style: 'multi', selector: 'td:first-child' },")

        'tableBuild.Append("buttons: [")
        'tableBuild.Append("{ extend: 'csv', exportOptions: { columns: ':visible'} },")
        'tableBuild.Append("{ extend: 'excel', exportOptions: { columns: ':visible'} },")
        'tableBuild.Append("{ extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible'} },")
        'tableBuild.Append("{ extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },")


        'tableBuild.Append("]")

        'Residual Market Forecast Values
        tableBuild.Append(footerBuild)
        tableBuild.Append(BuildButtonString)
        'Remove Selected Button:
        tableBuild.Append("});")


        'tableBuild.Append("$('.formatTable').DataTable({destroy:true,")
        'tableBuild.Append("dom:        'Bfitrp',")
        'tableBuild.Append("scrollY:        530, ")
        'tableBuild.Append("scrollX:        cw, ")
        'tableBuild.Append("scrollCollapse: true, ")
        'tableBuild.Append("scroller:       true, ")
        'tableBuild.Append(footerBuild)
        'tableBuild.Append(BuildButtonString)
        ''Remove Selected Button:
        'tableBuild.Append("});")
        tableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().columns.adjust();")
        tableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().scroller.measure();")

        Return tableBuild.ToString
    End Function
    Private Function BuildButtonString() As String
        Dim buttonsString As New StringBuilder
        Dim excelButton As String = ""
        Dim exportOptions As String = ""

        exportOptions = "columns: [function ( idx, data, node ) {"
        exportOptions += "var isVisible = table.column( idx ).visible();"
        exportOptions += "var isNotForExport = $.inArray( idx, hideFromExport ) !== -1;"
        exportOptions += "return isVisible && !isNotForExport ? true : false; "
        'ExportOptions += "}"
        exportOptions += "}, 'colvis']"


        buttonsString.Append("buttons: [ ")
        'CSV Button:
        buttonsString.Append("{")
        buttonsString.Append("extend:  'csv',")
        buttonsString.Append("exportOptions: {")
        buttonsString.Append(exportOptions)
        buttonsString.Append("}")
        buttonsString.Append("}, ")
        'Excel Button
        buttonsString.Append(excelButton)
        'PDF Button

        buttonsString.Append(" {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', ")
        buttonsString.Append("exportOptions: {")
        buttonsString.Append(exportOptions)
        buttonsString.Append("}")
        buttonsString.Append("}, ")
        'Column Visibility Button
        buttonsString.Append("{")
        buttonsString.Append("extend: 'colvis', text: 'Columns',")
        buttonsString.Append("collectionLayout:  'fixed two-column',")
        buttonsString.Append("postfixButtons: [ 'colvisRestore' ]")
        buttonsString.Append("},")

        buttonsString.Append("{ text: 'Remove Selected Rows', className: 'RemoveRowsValue',")
        buttonsString.Append("action: function(e, dt, node, config) {")

        buttonsString.Append("$(""#" & filter_draw.ClientID & """).val('filter');")
        buttonsString.Append("$(""#" & acKeepRemove.ClientID & """).val('remove');")
        buttonsString.Append("dt.rows('.selected').nodes().to$().addClass('remove');")
        buttonsString.Append("dt.rows({ selected: true} ).deselect();dt.draw();$('#" & filter_draw.ClientID & "').val('');ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');$('#" & tabPanel1GraphButton.ClientID & "').click();")


        buttonsString.Append("}")
        buttonsString.Append("},")

        buttonsString.Append("{ text: 'Keep Selected Rows', className: 'KeepTableRow',")
        buttonsString.Append("action: function(e, dt, node, config) {")


        buttonsString.Append("$(""#" & filter_draw.ClientID & """).val('filter');")
        buttonsString.Append("$(""#" & acKeepRemove.ClientID & """).val('keep');")
        buttonsString.Append("dt.rows('.selected').nodes().to$().addClass('keep'); ")
        buttonsString.Append("dt.rows({ selected: true} ).deselect();")
        buttonsString.Append("dt.draw();$('#" & filter_draw.ClientID & "').val('');ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');$('#" & tabPanel1GraphButton.ClientID & "').click();")

        buttonsString.Append("}")
        buttonsString.Append("},")

        buttonsString.Append("{ text: 'Reload Table', className: 'RefreshTableValue',")
        buttonsString.Append("action: function(e, dt, node, config) {$('#" & rowIDs.ClientID & "').val('');")
        buttonsString.Append("$(""#" & filter_draw.ClientID & """).val('filter');")
        buttonsString.Append("$(""#" & acKeepRemove.ClientID & """).val('remove');")
        buttonsString.Append("dt.rows().nodes().to$().removeClass('gone');  ")
        buttonsString.Append("dt.rows('.selected').deselect(); dt.draw();$('#" & filter_draw.ClientID & "').val('');ChangeTheMouseCursorOnItemParentDocument('standalone_page cursor_wait');$('#" & tabPanel1GraphButton.ClientID & "').click();")

        buttonsString.Append("}")
        buttonsString.Append("}")

        buttonsString.Append("]")

        Return buttonsString.ToString
    End Function

    Public Function run_asset_model_summary() As String    ' ByVal amod_id_list As String, ByVal year_list As String
        run_asset_model_summary = ""

        Try

            Dim results_table As New DataTable
            Dim resdiual_layer As New resdiualMarketValueDataLayer
            Dim temp_data As String = ""
            Dim htmlOut As New StringBuilder
            Dim divide_by_year As Boolean = False
            Dim year_values As String = ""
            Dim temp_amod_id_string As String = ""
            Dim avg As Double = 0
            Dim max As Double = 0
            Dim min As Double = 0
            residualGaugeChart.Visible = False
            'chartCSSResize.Attributes.Remove("class")
            'chartCSSResize.Attributes.Add("class", "twelve setUpLeftMargin_Width")


            If Me.split_by_year.Checked = True Then
                divide_by_year = True
            End If

            resdiual_layer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            resdiual_layer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            resdiual_layer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            resdiual_layer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            resdiual_layer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            year_values = ""
            If Me.year_end.SelectedValue.ToString.Trim <> "All" Then
                Dim listOfIndices As List(Of Integer) = year_end.GetSelectedIndices().ToList()
                For Each indice As Integer In listOfIndices
                    If year_end.Items(indice).Value.ToString.Trim <> "All" Then
                        If Trim(year_values) <> "" Then
                            year_values &= ","
                        End If
                        year_values &= year_end.Items(indice).Value
                    End If
                Next indice
            End If


            If Not IsNothing(localCriteria.ViewCriteriaAmodIDArray) Then

                For i = 0 To UBound(localCriteria.ViewCriteriaAmodIDArray)
                    If Trim(temp_amod_id_string) <> "" Then
                        temp_amod_id_string &= ", "
                    End If
                    temp_amod_id_string &= localCriteria.ViewCriteriaAmodIDArray(i).ToString
                Next

                results_table = resdiual_layer.get_assett_summary(localCriteria, temp_amod_id_string, year_values, divide_by_year, stringToExclude)
            Else
                results_table = resdiual_layer.get_assett_summary(localCriteria, localCriteria.ViewCriteriaAmodID, year_values, divide_by_year, stringToExclude)
            End If

            'results_table = resdiual_layer.get_assett_summary("272, 278", "2003,2004,2005,2006,2007", divide_by_year)


            ' htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))

            htmlOut.Append("<div class=""Box"">")
            Dim resultsString As String = ""
            htmlOut.Append("<table cellpadding=""0"" cellspacing=""0"" width=""100%"" id =""residualForecast"" class=""formatTable blue""><thead>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<tr><th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>SEL</strong></th>")
                    htmlOut.Append("<th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>ID</strong></th>")
                    htmlOut.Append("<th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>MAKE/MODEL</strong></th>")
                    If divide_by_year = True Then
                        htmlOut.Append("<th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>YEAR DLV</strong></th>")
                    End If

                    htmlOut.Append("<th valign=""middle"" class=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>AVG VALUE</strong></th>")
                    htmlOut.Append("<th valign=""middle"" class=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>5-YEAR RESIDUAL VALUE </strong></th>")
                    htmlOut.Append("<th valign=""middle"" class=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>5-YEAR % OF VALUE RETAINED</strong></th>")

                    htmlOut.Append("</tr>")
                    htmlOut.Append("</thead><tbody>")



                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left"">" & r("amod_id").ToString & "</td><td valign=""middle"" align=""left"">")


                        If resultsString <> "" Then
                            resultsString += ","
                        End If

                        resultsString += "["
                        resultsString += "'"
                        If split_by_year.Checked Then
                            If Not IsDBNull(r("ac_year")) Then
                                resultsString += r("ac_year").ToString & " - "
                            End If
                        Else


                            If Not IsDBNull(r("amod_make_name")) Then
                                resultsString += r("amod_make_name") & " "
                                htmlOut.Append("" & r("amod_make_name") & " ")
                            End If
                        End If
                        If Not IsDBNull(r("amod_model_name")) Then
                            resultsString += r("amod_model_name") & " "
                            htmlOut.Append("" & r("amod_model_name") & " ")
                        End If


                        htmlOut.Append("&nbsp;</td>")


                        If divide_by_year = True Then
                            htmlOut.Append("<td valign=""middle"" align=""right"" data-sort=""" & r("ac_year").ToString & """>")

                            If Not IsDBNull(r("ac_year")) Then
                                htmlOut.Append("" & r("ac_year") & "")
                            End If


                            htmlOut.Append("&nbsp;</td>")
                        End If
                        resultsString += "',"
                        htmlOut.Append("<td valign=""middle"" align=""right"" data-sort=""" & r("avg_value").ToString & """>")

                        If Not IsDBNull(r("avg_value")) Then
                            htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("avg_value")))
                        End If

                        htmlOut.Append("&nbsp;</td>")


                        htmlOut.Append("<td valign=""middle"" align=""right"" data-sort=""" & r("avg_residual").ToString & """>")


                        If Not IsDBNull(r("avg_residual")) Then
                            htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("avg_residual")))
                        End If

                        htmlOut.Append("&nbsp;</td>")


                        htmlOut.Append("<td valign=""middle"" align=""right"" data-sort=""" & r("percent_of_orig_value").ToString & """>")

                        If Not IsDBNull(r("percent_of_orig_value")) Then
                            htmlOut.Append("" & FormatNumber(r("percent_of_orig_value"), 1) & "%")

                            If r("percent_of_orig_value") < min Or min = 0 Then
                                min = r("percent_of_orig_value")
                            End If
                            If r("percent_of_orig_value") > max Or max = 0 Then
                                max = r("percent_of_orig_value")
                            End If

                            avg += r("percent_of_orig_value")
                            resultsString += r("percent_of_orig_value").ToString
                        Else
                            resultsString += "0"
                        End If
                        resultsString += "]"

                        htmlOut.Append("&nbsp;</td>")

                        htmlOut.Append("</tr>")
                    Next



                    If results_table.Rows.Count > 1 Then
                        avg = avg / results_table.Rows.Count
                        generateAvgGauge(min, avg, max)
                    End If

                Else
                    chartCSSResize.Attributes.Remove("class")
                    chartCSSResize.Attributes.Add("class", "display_none")
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" class='table_specs'>No data available!</td></tr>")
                End If
            Else
                chartCSSResize.Attributes.Remove("class")
                chartCSSResize.Attributes.Add("class", "display_none")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"" class='table_specs'>No data available!</td></tr>")
            End If

            htmlOut.Append("</tbody></table>")

            htmlOut.Append("</div>")

            run_asset_model_summary = htmlOut.ToString
            BuildResBarChart(resultsString)
            tab1Update.Update()
        Catch ex As Exception

        End Try


    End Function

    Private Sub resizeScript(ByVal graphID As Long)
        Dim scriptOut As New StringBuilder

        scriptOut.Append("$(window).resize(function() {" + vbCrLf)
        scriptOut.Append("if(this.resizeTO) clearTimeout(this.resizeTO);" + vbCrLf)
        scriptOut.Append("this.resizeTO = setTimeout(function() {" + vbCrLf)
        scriptOut.Append("$(this).trigger('resizeEnd');" + vbCrLf)
        scriptOut.Append("}, 500);" + vbCrLf)
        scriptOut.Append("});" + vbCrLf)

        '//redraw graph when window resize is completed  
        scriptOut.Append("$(window).on('resizeEnd', function() {")
        scriptOut.Append("$('#visualization" + graphID.ToString + "').empty();" + vbCrLf)
        scriptOut.Append("   drawVisualization" + graphID.ToString + "();$('#chart_divRes').empty();drawBasic();" + vbCrLf)
        scriptOut.Append("});" + vbCrLf)

        System.Web.UI.ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "refreshGraph" & graphID.ToString, scriptOut.ToString, True)

    End Sub
    Private Sub BuildResBarChart(ByRef ResultsString As String)
        Dim jsStr As String = ""
        jsStr = " function drawBasic() {"

        jsStr += "var data = new google.visualization.DataTable();"
        jsStr += "data.addColumn('string', 'Model');"
        jsStr += "data.addColumn('number', 'Residual %');"

        jsStr += "  data.addRows(["
        jsStr += ResultsString
        jsStr += "   ]); "

        jsStr += " var options = {"
        jsStr += " legend: {position: 'none'}, "
        jsStr += "      hAxis: {"
        If split_by_year.Checked Then
            jsStr += "title:  'Year/Model'"
        Else
            jsStr += "title:  'Make/Model'"
        End If

        jsStr += "       },"
        jsStr += "       vAxis: {"
        jsStr += "title:  'Residual Value %'"
        jsStr += "       }"
        jsStr += "     };"

        jsStr += "     var chart = new google.visualization.ColumnChart("
        jsStr += "        document.getElementById('chart_divRes'));"
        jsStr += "     chart.draw(data, options);"
        jsStr += "   };setTimeout(function(){drawBasic(); }, 1000);"

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(tab1Update, Me.GetType, "barChartArray", jsStr, True)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Try

            'Dim JavascriptOnLoad As String = ""

            '      'JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"

            '      If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
            '  System.Web.UI.ScriptManager.RegisterStartupScript(Me.contentClass, Me.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
            'End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreRender): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub

    Public Sub load_view_session_variables()

        Try

            ' because these values are needed on this page they need to match the control names in the control
            ' so the request header pickes up the right values
            If Not IsNothing(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type")) Then
                If Not String.IsNullOrEmpty(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString) Then
                    If Not Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString.ToLower.Contains("all") Then
                        Session.Item("viewAircraftType") = Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Type").ToString.Trim
                    Else
                        Session.Item("viewAircraftModel") = ""
                        Session.Item("viewAircraftMake") = ""
                        Session.Item("viewAircraftType") = ""
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make")) Then
                If Not String.IsNullOrEmpty(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString) Then
                    If Not Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString.ToLower.Contains("all") Then
                        Session.Item("viewAircraftMake") = Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Make").ToString.Trim
                    Else
                        Session.Item("viewAircraftModel") = ""
                        Session.Item("viewAircraftMake") = ""
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model")) Then
                If Not String.IsNullOrEmpty(Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString) Then
                    If Not Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString.ToLower.Contains("all") Then
                        Session.Item("viewAircraftModel") = Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "Model").ToString.Trim
                    Else
                        Session.Item("viewAircraftModel") = ""
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("chkHelicopterFilter")) Then
                If Not String.IsNullOrEmpty(Request.Item("chkHelicopterFilter").ToString) Then
                    bHasHelicopterFilter = CBool(Request.Item("chkHelicopterFilter").ToString.Trim)
                End If
            End If

            If Not IsNothing(Request.Item("chkBusinessFilter")) Then
                If Not String.IsNullOrEmpty(Request.Item("chkBusinessFilter").ToString) Then
                    bHasBusinessFilter = CBool(Request.Item("chkBusinessFilter").ToString.Trim)
                End If
            End If

            If Not IsNothing(Request.Item("chkCommercialFilter")) Then
                If Not String.IsNullOrEmpty(Request.Item("chkCommercialFilter").ToString) Then
                    bHasCommercialFilter = CBool(Request.Item("chkCommercialFilter").ToString.Trim)
                End If
            End If

            If bClearView Then

                HttpContext.Current.Session.Item("hasModelFilter") = False
                HttpContext.Current.Session.Item("hasHelicopterFilter") = False
                HttpContext.Current.Session.Item("hasBusinessFilter") = False
                HttpContext.Current.Session.Item("hasCommercialFilter") = False

                HttpContext.Current.Session.Item("viewAircraftModel") = ""
                HttpContext.Current.Session.Item("viewAircraftMake") = ""
                HttpContext.Current.Session.Item("viewAircraftType") = ""

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in [View_Master.ascx.vb :  [load_view_session_variables] : " + ex.Message
        End Try

    End Sub

    Private Sub atGlanceClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles atGlanceClear.Click
        HttpContext.Current.Session.Item("hasModelFilter") = False
        HttpContext.Current.Session.Item("hasHelicopterFilter") = False
        HttpContext.Current.Session.Item("hasBusinessFilter") = False
        HttpContext.Current.Session.Item("hasCommercialFilter") = False

        HttpContext.Current.Session.Item("viewAircraftModel") = ""
        HttpContext.Current.Session.Item("viewAircraftMake") = ""
        HttpContext.Current.Session.Item("viewAircraftType") = ""
        Response.Redirect("/resdiualMarketValue.aspx")
    End Sub

    Private Sub tabPanel1GraphButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabPanel1GraphButton.Click

        If Not String.IsNullOrEmpty(rowIDs.Text) Then
            If split_by_year.Checked = False Then
                Dim splitID As Array = Split(rowIDs.Text, ",")
                For SelectionCount = 0 To UBound(splitID)
                    If SelectionCount > 0 Then
                        stringToExclude += " and "
                    End If
                    stringToExclude += " amod_id NOT IN (" & Trim(splitID(SelectionCount)) & ")"
                Next

            Else
                Dim splitID As Array = Split(rowIDs.Text, ",")
                For SelectionCount = 0 To UBound(splitID)

                    Dim splitYearID As Array = Split(Trim(splitID(SelectionCount)), "--")

                    If UBound(splitYearID) >= 1 Then
                        If SelectionCount > 0 Then
                            stringToExclude += " and "
                        End If
                        stringToExclude += " (amod_id = '" & Trim(splitYearID(0)) & "' and ac_year <> '" & Trim(splitYearID(1)) & "') "
                    End If


                Next
            End If
        End If

        'stringToExclude = stringToExclude
        residualValueChartForm(localCriteria, localCriteria.ViewCriteriaAmodID, "", False, tab1Update, stringToExclude)
        Me.table_label.Text = run_asset_model_summary()
    End Sub
End Class