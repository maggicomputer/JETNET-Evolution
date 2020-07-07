' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/aircraftFinder.aspx.vb $
'$$Author: Amanda $
'$$Date: 5/15/20 4:29p $
'$$Modtime: 4/23/20 3:00p $
'$$Revision: 7 $
'$$Workfile: aircraftFinder.aspx.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class finderCriteriaClass

    Private _finderCriteriaStatusCode As eObjStatusCode
    Private _finderCriteriaDetailError As eObjDetailErrorCode

    Private _finderCriteria_StartYear As Integer
    Private _finderCriteria_EndYear As Integer
    Private _finderCriteria_StartRange As Long
    Private _finderCriteria_EndRange As Long
    Private _finderCriteria_StartPAX As Long
    Private _finderCriteria_EndPAX As Long
    Private _finderCriteria_StartPrice As Long
    Private _finderCriteria_EndPrice As Long
    Private _finderCriteria_StartAge As Long
    Private _finderCriteria_EndAge As Long
    Private _finderCriteria_StartAFTT As Long
    Private _finderCriteria_EndAFTT As Long

    Private _finderCriteria_JustForSale As Boolean
    Private _finderCriteria_Location As String
    Private _finderCriteria_WeightClass As String
    Private _finderCriteria_Features As Array
    Private _finderCriteria_Airframe_Maintenance_Program As Boolean
    Private _finderCriteria_Engine_Maintenance_Program As Boolean
    Private _finderCriteria_Engine_Maintenance_Program_Type As String

    Sub New()

        _finderCriteriaStatusCode = eObjStatusCode.NULL
        _finderCriteriaDetailError = eObjDetailErrorCode.NULL

        _finderCriteria_StartYear = 0
        _finderCriteria_EndYear = 0

        _finderCriteria_StartRange = 0
        _finderCriteria_EndRange = 0

        _finderCriteria_StartPAX = 0
        _finderCriteria_EndPAX = 0

        _finderCriteria_StartPrice = 0
        _finderCriteria_EndPrice = 0

        _finderCriteria_StartAge = 0
        _finderCriteria_EndAge = 0

        _finderCriteria_StartAFTT = 0
        _finderCriteria_EndAFTT = 0

        _finderCriteria_JustForSale = False
        _finderCriteria_Location = ""
        _finderCriteria_WeightClass = ""
        _finderCriteria_Features = Nothing

        _finderCriteria_Airframe_Maintenance_Program = False
        _finderCriteria_Engine_Maintenance_Program = False
        _finderCriteria_Engine_Maintenance_Program_Type = ""


    End Sub

    Public Property FinderCriteriaStatusCode() As eObjStatusCode
        Get
            Return _finderCriteriaStatusCode
        End Get
        Set(ByVal value As eObjStatusCode)
            _finderCriteriaStatusCode = value
        End Set
    End Property

    Public Property FinderCriteriaDetailError() As eObjDetailErrorCode
        Get
            Return _finderCriteriaDetailError
        End Get
        Set(ByVal value As eObjDetailErrorCode)
            _finderCriteriaDetailError = value
        End Set
    End Property

    Public Property FinderAirframe_Maintenance_Program() As Boolean
        Get
            Return _finderCriteria_Airframe_Maintenance_Program
        End Get
        Set(ByVal value As Boolean)
            _finderCriteria_Airframe_Maintenance_Program = value
        End Set
    End Property
    Public Property FinderEngine_Maintenance_Program() As Boolean
        Get
            Return _finderCriteria_Engine_Maintenance_Program
        End Get
        Set(ByVal value As Boolean)
            _finderCriteria_Engine_Maintenance_Program = value
        End Set
    End Property
    Public Property FinderEngine_Maintenance_Program_Type() As String
        Get
            Return _finderCriteria_Engine_Maintenance_Program_Type
        End Get
        Set(ByVal value As String)
            _finderCriteria_Engine_Maintenance_Program_Type = value
        End Set
    End Property


    Public Property FinderCriteriaStartYear() As Integer
        Get
            Return _finderCriteria_StartYear
        End Get
        Set(ByVal value As Integer)
            _finderCriteria_StartYear = value
        End Set
    End Property

    Public Property FinderCriteriaEndYear() As Integer
        Get
            Return _finderCriteria_EndYear
        End Get
        Set(ByVal value As Integer)
            _finderCriteria_EndYear = value
        End Set
    End Property

    Public Property FinderCriteriaStartRange() As Long
        Get
            Return _finderCriteria_StartRange
        End Get
        Set(ByVal value As Long)
            _finderCriteria_StartRange = value
        End Set
    End Property

    Public Property FinderCriteriaEndRange() As Long
        Get
            Return _finderCriteria_EndRange
        End Get
        Set(ByVal value As Long)
            _finderCriteria_EndRange = value
        End Set
    End Property

    Public Property FinderCriteriaStartPAX() As Long
        Get
            Return _finderCriteria_StartPAX
        End Get
        Set(ByVal value As Long)
            _finderCriteria_StartPAX = value
        End Set
    End Property

    Public Property FinderCriteriaEndPAX() As Long
        Get
            Return _finderCriteria_EndPAX
        End Get
        Set(ByVal value As Long)
            _finderCriteria_EndPAX = value
        End Set
    End Property

    Public Property FinderCriteriaStartPrice() As Long
        Get
            Return _finderCriteria_StartPrice
        End Get
        Set(ByVal value As Long)
            _finderCriteria_StartPrice = value
        End Set
    End Property

    Public Property FinderCriteriaEndPrice() As Long
        Get
            Return _finderCriteria_EndPrice
        End Get
        Set(ByVal value As Long)
            _finderCriteria_EndPrice = value
        End Set
    End Property

    Public Property FinderCriteriaStartAge() As Long
        Get
            Return _finderCriteria_StartAge
        End Get
        Set(ByVal value As Long)
            _finderCriteria_StartAge = value
        End Set
    End Property

    Public Property FinderCriteriaEndAge() As Long
        Get
            Return _finderCriteria_EndAge
        End Get
        Set(ByVal value As Long)
            _finderCriteria_EndAge = value
        End Set
    End Property

    Public Property FinderCriteriaStartAFTT() As Long
        Get
            Return _finderCriteria_StartAFTT
        End Get
        Set(ByVal value As Long)
            _finderCriteria_StartAFTT = value
        End Set
    End Property

    Public Property FinderCriteriaEndAFTT() As Long
        Get
            Return _finderCriteria_EndAFTT
        End Get
        Set(ByVal value As Long)
            _finderCriteria_EndAFTT = value
        End Set
    End Property

    Public Property FinderCriteriaLocation() As String
        Get
            Return _finderCriteria_Location
        End Get
        Set(ByVal value As String)
            _finderCriteria_Location = value
        End Set
    End Property

    Public Property FinderCriteriaFeatures() As Array
        Get
            Return _finderCriteria_Features
        End Get
        Set(ByVal value As Array)
            _finderCriteria_Features = value
        End Set
    End Property

    Public Property FinderCriteriaWeightClass() As String
        Get
            Return _finderCriteria_WeightClass
        End Get
        Set(ByVal value As String)
            _finderCriteria_WeightClass = value
        End Set
    End Property

    Public Property FinderCriteriaJustForSale() As Boolean
        Get
            Return _finderCriteria_JustForSale
        End Get
        Set(ByVal value As Boolean)
            _finderCriteria_JustForSale = value
        End Set
    End Property

End Class  ' 

Partial Public Class aircraftFinder
    Inherits System.Web.UI.Page

    Dim minYear As Integer = Year(DateAdd(DateInterval.Year, -65, Now()))
    Dim maxYear As Integer = Year(DateAdd(DateInterval.Year, +10, Now()))
    Dim startBaseYear As Integer = 2000
    Dim endBaseYear As Integer = Year(Now())

    Dim minRange As Long = 500
    Dim maxRange As Long = 8000
    Dim startBaseRange As Long = 0
    Dim endBaseRange As Long = 2500

    Dim minPAX As Long = 1
    Dim maxPAX As Long = 30
    Dim startBasePAX As Long = 0
    Dim endBasePAX As Long = 8

    Dim minPrice As Long = 1000
    Dim maxPrice As Long = 60000
    Dim startBasePrice As Long = 0
    Dim endBasePrice As Long = 0

    Dim minAge As Long = 1954
    Dim maxAge As Long = Year(Now())
    Dim startBaseAge As Long = 0
    Dim endBaseAge As Long = 0

    Dim minAFTT As Long = 500
    Dim maxAFTT As Long = 15000
    Dim startBaseAFTT As Long = 5000
    Dim endBaseAFTT As Long = 5000

    Dim bIsSearch As Boolean = False

    Dim arrWeightTypeSelection(,) As String = Nothing

    Dim objSearchCriteria As New finderCriteriaClass

    Public Shared masterPage As New Object

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
            masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page
        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreInit): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreInit): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sAircraftTypeHTMLtable As String = ""

        Try

            HttpContext.Current.Session.Item("crmUserLogon") = True

            If Not IsNothing(HttpContext.Current.Session.Item("acFinderCriteria")) Then
                objSearchCriteria = HttpContext.Current.Session.Item("acFinderCriteria")
            End If

            get_airctaft_type_HTMLtable(sAircraftTypeHTMLtable)
            'Keep these comments until we're positive we're not using sliders anymore, and then remove.
            'all_aircraft.Attributes.Add("onclick", "ToggleAllAircraft();")
            ' forSale_aircraft.Attributes.Add("onclick", "ToggleForsaleAircraft();")
            engine_maintenance_program.Attributes.Add("onclick", "ToggleAllMaintenance();")
            add_ToggleEngineMaintenance_Script(engine_maintenance_dropdown, engine_maintenance_program, maintenancePanel.ClientID)
            'add_ToggleAllorForsale_RadioButtons_Script(all_aircraft, forSale_aircraft, "price_range_div")

            If Not IsPostBack Then

                fill_comfort_features_checkboxlist(0, comfortFeatursCBL)

                aircraftTypeTable.Text = sAircraftTypeHTMLtable.Trim

                'range_start.Text = IIf(startBaseRange > 0, startBaseRange.ToString, minRange.ToString)
                'range_end.Text = 0 'IIf(endBaseRange > 0, FormatNumber(endBaseRange.ToString, 0, False, False, True), FormatNumber(maxRange.ToString, 0, False, False, True))
                'hiddenRange_start.Text = range_start.Text
                'hiddenRange_end.Text = range_end.Text

                For i As Integer = minRange To maxRange Step 500
                    minimumRangeDropdown.Items.Add(New ListItem(i, i))
                Next

                For i As Integer = minAFTT To maxAFTT Step 500
                    maximumAFTTDropdown.Items.Add(New ListItem(i, i))
                Next

                For i As Integer = minPAX To maxPAX Step 1
                    minimumPassengersDropdown.Items.Add(New ListItem(i, i))
                Next

                'pax_start.Text = IIf(startBasePAX > 0, startBasePAX.ToString, minPAX.ToString)
                'pax_end.Text = 0 'IIf(endBasePAX > 0, FormatNumber(endBasePAX.ToString, 0, False, False, True), FormatNumber(maxPAX.ToString, 0, False, False, True))
                'hiddenPax_start.Text = pax_start.Text
                'hiddenPax_end.Text = pax_end.Text

                'price_start.Text = IIf(startBasePrice > 0, startBasePrice.ToString, minPrice.ToString)
                'price_end.Text = 0 'IIf(endBasePrice > 0, FormatNumber(endBasePrice.ToString, 0, False, False, True), FormatNumber(maxPrice.ToString, 0, False, False, True))
                'hiddenPrice_start.Text = price_start.Text
                'hiddenPrice_end.Text = price_end.Text

                'age_start.Text = IIf(startBaseAge > 0, startBaseAge.ToString, minAge.ToString)
                'age_end.Text = IIf(endBaseAge > 0, FormatNumber(endBaseAge.ToString, 0, False, False, True), FormatNumber(maxAge.ToString, 0, False, False, True))
                'hiddenAge_start.Text = age_start.Text
                'hiddenAge_end.Text = age_end.Text

                For i As Integer = minPrice To maxPrice Step 1000
                    maximumPriceDropdown.Items.Add(New ListItem(i, i * 1000))
                Next

                'age_start.Text = IIf(startBaseYear > 0, startBaseYear.ToString, minYear.ToString)
                'age_end.Text = IIf(endBaseYear > 0, FormatNumber(endBaseYear.ToString, 0, False, False, False), FormatNumber(maxYear.ToString, 0, False, False, False))
                'hiddenAge_start.Text = age_start.Text
                'hiddenAge_end.Text = age_end.Text


                For i As Integer = maxYear To minYear Step -1
                    yearDropdownStart.Items.Add(New ListItem(i, i))
                Next

                For i As Integer = maxYear To minYear Step -1
                    yearDropdownEnd.Items.Add(New ListItem(i, i))
                Next

                'aftt_start.Text = IIf(startBaseAFTT > 0, startBaseAFTT.ToString, minAFTT.ToString)
                'aftt_end.Text = IIf(endBaseAFTT > 0, FormatNumber(endBaseAFTT.ToString, 0, False, False, True), FormatNumber(maxAFTT.ToString, 0, False, False, True))
                'hiddenAftt_start.Text = aftt_start.Text
                'hiddenAftt_end.Text = aftt_end.Text

                'all_aircraft.Checked = True
                market_status.SelectedValue = ""
                FillEngineMaintenanceDropdown()
            Else

                objSearchCriteria.FinderCriteriaStartYear = CInt(yearDropdownStart.SelectedValue)
                objSearchCriteria.FinderCriteriaEndYear = CInt(yearDropdownEnd.SelectedValue)

                'objSearchCriteria.FinderCriteriaStartRange = CInt(range_start.Text)
                objSearchCriteria.FinderCriteriaEndRange = CInt(minimumRangeDropdown.SelectedValue)

                'objSearchCriteria.FinderCriteriaStartPAX = CInt(pax_start.Text)
                objSearchCriteria.FinderCriteriaEndPAX = CInt(minimumPassengersDropdown.SelectedValue)

                'objSearchCriteria.FinderCriteriaStartPrice = CInt(price_start.Text)
                objSearchCriteria.FinderCriteriaEndPrice = CInt(maximumPriceDropdown.SelectedValue)

                'objSearchCriteria.FinderCriteriaStartAFTT = CInt(aftt_start.Text)
                objSearchCriteria.FinderCriteriaEndAFTT = CInt(maximumAFTTDropdown.SelectedValue) 'CInt(aftt_end.Text)

                objSearchCriteria.FinderCriteriaLocation = aircraft_registration.SelectedValue

                Dim GenericFeatures As New List(Of String)
                For Each l As ListItem In comfortFeatursCBL.Items
                    If l.Selected Then
                        GenericFeatures.Add(l.Value)
                    End If
                Next


                objSearchCriteria.FinderCriteriaFeatures = GenericFeatures.ToArray

                objSearchCriteria.FinderCriteriaWeightClass = selected_type_rows.Text

                objSearchCriteria.FinderCriteriaJustForSale = IIf(market_status.SelectedValue = "Y", True, False) 'forSale_aircraft.Checked

                HttpContext.Current.Session.Item("acFinderCriteria") = objSearchCriteria

            End If

            objSearchCriteria.FinderAirframe_Maintenance_Program = IIf(airframe_maintenance_program.Checked, True, False)
            objSearchCriteria.FinderEngine_Maintenance_Program = IIf(engine_maintenance_program.Checked, True, False)

            objSearchCriteria.FinderEngine_Maintenance_Program_Type = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(engine_maintenance_dropdown, False, 0, True)

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_Load): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_Load): " + ex.Message.ToString.Trim
            End If
        End Try

        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Aircraft Acquisition View")
        masterPage.SetPageTitle("Aircraft Acquisition View")

    End Sub
    Private Sub FillEngineMaintenanceDropdown()
        Dim engineTable As New DataTable
        Dim distinct_table_view As New DataView
        Dim distinct_table As New DataTable

        Try
            engineTable = masterPage.aclsData_Temp.EngineMaintenanceProgramProviderOrName(True, False, "")

            ''create the view to get the distinct values.
            distinct_table_view = engineTable.DefaultView
            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "emp_program_name")

            If Not IsNothing(distinct_table) Then
                engine_maintenance_dropdown.Items.Clear()
                engine_maintenance_dropdown.Items.Add(New ListItem("Any", ""))
                If Not IsNothing(distinct_table) Then
                    If distinct_table.Rows.Count > 0 Then
                        For Each r As DataRow In distinct_table.Rows
                            If Not IsDBNull(r("emp_program_name")) Then
                                engine_maintenance_dropdown.Items.Add(New ListItem(CStr(r("emp_program_name")), Replace(CStr(r("emp_program_name")), "'", "&apos;")))
                            End If
                        Next
                    End If
                End If
                engine_maintenance_dropdown.SelectedValue = ""
            End If
        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (FillEngineMaintenanceDropdown): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (FillEngineMaintenanceDropdown): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Try

            Dim JavascriptOnLoad As String = ""

            'JavascriptOnLoad += vbCrLf + BuildJqueryDropdownJavascript()

            'JavascriptOnLoad += vbCrLf + BuildSliderJavascript("slider-range", range_start, range_end, 0, True, False, True, 200, minRange, maxRange, startBaseRange, endBaseRange)

            'JavascriptOnLoad += vbCrLf + BuildSliderJavascript("pax-range", pax_start, pax_end, 0, True, False, False, 0, minPAX, maxPAX, startBasePAX, endBasePAX)

            ' JavascriptOnLoad += vbCrLf + BuildSliderJavascript("price-range", price_start, price_end, 0, True, False, True, 10000, minPrice, maxPrice, startBasePrice, endBasePrice)

            '  JavascriptOnLoad += vbCrLf + BuildSliderJavascript("age-range", age_start, age_end, 0, False, True, False, 0, minYear, maxYear, startBaseYear, endBaseYear)

            'JavascriptOnLoad += vbCrLf + BuildSliderJavascript("aftt-range", aftt_start, aftt_end, 0, True, False, True, 1000, minAFTT, maxAFTT, startBaseAFTT, endBaseAFTT)

            JavascriptOnLoad += vbCrLf + "CreateTheDatatable('aircraftInnerTable','aircraftTypeTable','aircraftjQueryTable');"

            JavascriptOnLoad += vbCrLf + BuildJavascriptClickEventForSummary()



            'If all_aircraft.Checked Then
            '  JavascriptOnLoad += vbCrLf + "ToggleAllMaintenance();"
            'End If
            JavascriptOnLoad += vbCrLf + "setUpSliderInitial();"
            'JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"
            JavascriptOnLoad += vbCrLf + "BuildSummary();" 'Javascript BuildSummary function builds summarized UL based off of form inputs.
            JavascriptOnLoad += WindowResizeFunction()



            If Not Page.IsPostBack Then
                If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
                End If
            ElseIf bIsSearch Then

            Else 'No data in table. Just close loading screen.
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabContainerBottomUpdate, Me.GetType(), "onLoadPostbackNoData", "CloseLoadingMessage(""DivLoadingMessage"");", True)
            End If
        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreRender): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub
    Private Function WindowResizeFunction() As String
        Dim returnString As String = "$(window).resize(function() {"
        returnString += " var cw = $('.mainHolder').width() - 40;"
        returnString += " $(""#searchInnerTable"").width(cw);"
        returnString += " $($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
        returnString += " $($.fn.dataTable.tables(true)).DataTable().scroller.measure();"
        returnString += " });"
        Return returnString
    End Function
    Private Function BuildJavascriptClickEventForSummary() As String
        Dim returnString As String = ""
        returnString = "$(""input, select"").change(function() {"
        returnString += "BuildSummary();"
        returnString += "});"
        Return returnString
    End Function
    Private Function BuildSliderJavascript(ByVal slidername As String, ByVal startVal As TextBox, ByVal endVal As TextBox, ByVal decimalPlace As Long,
                                           ByVal showDecimal As Boolean, ByVal showRange As Boolean, ByVal addStep As Boolean, ByVal stepIncriment As Long,
                                           ByVal minVal As Long, ByVal maxVal As Long, ByVal baseMin As Long, ByVal baseMax As Long) As String

        Dim sliderString As New StringBuilder

        sliderString.Append(vbCrLf + "$(""#" + slidername.ToLower + """).slider({")

        If showRange Then
            sliderString.Append(vbCrLf + "range: true,")
        End If

        If addStep Then
            sliderString.Append(vbCrLf + "step: " + stepIncriment.ToString + ",")
        End If

        sliderString.Append(vbCrLf + "min: " + minVal.ToString + ",")
        sliderString.Append(vbCrLf + "max: " + maxVal.ToString + ",")

        If showRange Then
            sliderString.Append(vbCrLf + "values: [ " + IIf(baseMin > 0, baseMin.ToString, minVal.ToString) + ", " + IIf(baseMax > 0, baseMax.ToString, maxVal.ToString) + " ],")
            sliderString.Append(vbCrLf + "slide: function( event, ui ) {")

            If showDecimal Then
                sliderString.Append(vbCrLf + "  $( ""#" + startVal.ClientID.ToString + """).val($.number( ui.values[ 0 ], " + decimalPlace.ToString + " ));")
                sliderString.Append(vbCrLf + "  $( ""#" + endVal.ClientID.ToString + """).val($.number( ui.values[ 1 ], " + decimalPlace.ToString + " ));")
            Else
                sliderString.Append(vbCrLf + "  $( ""#" + startVal.ClientID.ToString + """).val(ui.values[ 0 ]);")
                sliderString.Append(vbCrLf + "  $( ""#" + endVal.ClientID.ToString + """).val(ui.values[ 1 ]);")
            End If
            sliderString.Append(vbCrLf + "BuildSummary();}") 'BuildSummary() added as a function which builds a summary of picks. Needs to be triggered to update on slide
        Else

            sliderString.Append(vbCrLf + "value: [ " + IIf(baseMin > 0, baseMin.ToString, minVal.ToString) + " ],")
            sliderString.Append(vbCrLf + "slide: function( event, ui ) {")
            If showDecimal Then
                sliderString.Append(vbCrLf + "  $( ""#" + endVal.ClientID.ToString + """).val($.number( ui.value, " + decimalPlace.ToString + " )); ")
            Else
                sliderString.Append(vbCrLf + "  $( ""#" + endVal.ClientID.ToString + """).val(ui.value); ")
            End If
            sliderString.Append(vbCrLf + "BuildSummary();}") 'BuildSummary() added as a function which builds a summary of picks. Needs to be triggered to update on slide

        End If

        sliderString.Append(vbCrLf + "});")

        Return sliderString.ToString

    End Function

    'Private Function BuildJqueryDropdownJavascript() As String

    '  Dim dropdownString As New StringBuilder

    '  dropdownString.Append(vbCrLf + "swapChosenDropdowns();")

    '  Return dropdownString.ToString

    'End Function

    'Public Sub add_ToggleAllorForsale_RadioButtons_Script(ByVal rbSource1 As RadioButton, ByVal rbSource2 As RadioButton, ByVal dvSource1 As String)

    '  'Register the script block
    '  Dim sScptStr As StringBuilder = New StringBuilder()

    '  If Not Page.ClientScript.IsClientScriptBlockRegistered("cde-rb-onclick") Then

    '    sScptStr.Append("<script type=""text/javascript"">")
    '    sScptStr.Append(vbCrLf & "  function ToggleAllAircraft() {")
    '    sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource1.ClientID.ToString + """).checked == true) {")
    '    sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).checked = false;")
    '    sScptStr.Append(vbCrLf & "      document.getElementById(""" + dvSource1.Trim + """).style.visibility = ""hidden"";")
    '    sScptStr.Append(vbCrLf & "    }")
    '    sScptStr.Append(vbCrLf & "  }")
    '    sScptStr.Append(vbCrLf & "  function ToggleForsaleAircraft() {")
    '    sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource2.ClientID.ToString + """).checked == true) {")
    '    sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).checked = false;")
    '    sScptStr.Append(vbCrLf & "      document.getElementById(""" + dvSource1.Trim + """).style.visibility = ""visible"";")
    '    sScptStr.Append(vbCrLf & "    }")
    '    sScptStr.Append(vbCrLf & "  }")
    '    sScptStr.Append(vbCrLf & "</script>")

    '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "cde-rb-onclick", sScptStr.ToString, False)

    '  End If

    '  sScptStr = Nothing

    'End Sub
    Public Sub add_ToggleEngineMaintenance_Script(ByVal maintenanceDropdown As DropDownList, ByVal engineCheckbox As CheckBox, ByVal dvSource1 As String)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("cde-main-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function ToggleAllMaintenance() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + engineCheckbox.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + dvSource1.Trim + """).style.visibility = ""visible"";")
            sScptStr.Append(vbCrLf & "    } else { document.getElementById(""" + dvSource1.Trim + """).style.visibility = ""hidden"";}")
            sScptStr.Append(vbCrLf & "  }")

            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "cde-main-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub
    Public Function getAircraftFinderComfortFeaturesDataTable(Optional ByVal sFeatureList As Array = Nothing) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("select acatt_name, acatt_id from Aircraft_Attribute with (NOLOCK) ")
            sQuery.Append(" where acatt_area='Interior' and acatt_summary_level_flag='Y' and acatt_status='Y' ")
            sQuery.Append(" order by acatt_name ")


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAircraftFinderComfortFeaturesDataTable(Optional ByVal sFeatureList As String = "") As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderComfortFeaturesDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderComfortFeaturesDataTable(Optional ByVal sFeatureList As String = "") As DataTable</b><br />" + ex.Message

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

    Public Sub fill_comfort_features_checkboxlist(ByRef maxWidth As Long, ByRef cblComfortFeatures As CheckBoxList)

        Dim results_table As New DataTable

        Try

            cblComfortFeatures.Items.Clear()
            results_table = getAircraftFinderComfortFeaturesDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows
                        'acatt_name, acatt_id
                        If Not IsDBNull(r.Item("acatt_name")) And Not String.IsNullOrEmpty(r.Item("acatt_id").ToString.Trim) Then

                            If (r.Item("acatt_name").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                maxWidth = (r.Item("acatt_name").ToString.Length * Constants._STARTCHARWIDTH)
                            End If

                            cblComfortFeatures.Items.Add(New ListItem(r.Item("acatt_name").ToString, r.Item("acatt_id").ToString))

                            ' to select "checkbox" 
                            'Dim currentCheckBox As ListItem = cblComfortFeatures.Items.FindByValue(r.Item("kfeat_code").ToString)
                            'currentCheckBox.Selected = True

                        End If

                    Next

                End If
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in fill_comfort_features_checkboxlist(ByRef maxWidth As Long, ByRef cblComfortFeatures As CheckBoxList)</b><br />" + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Function getAircraftFinderTypeDataTable(ByRef evoSubScriptionCls As clsSubscriptionClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Try

            sQuery.Append("SELECT DISTINCT acwgtcls_display_name, acwgtcls_display_range, acwgtcls_display_passengers, acwgtcls_sort_order, amod_weight_class, amod_airframe_type_code")
            sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Weight_Class WITH(NOLOCK) ON amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code")

            sQuery.Append(" WHERE amod_number_of_passengers > 0 AND amod_max_range_miles > 0")

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(evoSubScriptionCls, False, True, True))

            sQuery.Append(" GROUP BY acwgtcls_display_name, acwgtcls_sort_order, acwgtcls_display_range, acwgtcls_display_passengers, amod_weight_class, amod_airframe_type_code")

            sQuery.Append(" ORDER BY acwgtcls_sort_order")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAircraftFinderTypeDataTable(ByRef evoSubScriptionCls As clsSubscriptionClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderTypeDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderTypeDataTable(ByRef evoSubScriptionCls As clsSubscriptionClass) As DataTable</b><br />" + ex.Message

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

    Public Sub get_airctaft_type_HTMLtable(ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = getAircraftFinderTypeDataTable(HttpContext.Current.Session.Item("localPreferences"))

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    ReDim arrWeightTypeSelection(results_table.Rows.Count - 1, 3)

                    htmlOut.Append("<table id=""aircraftTypeTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")
                    htmlOut.Append("<th width=""10""><span class=""help_cursor"" title=""Used to select and remove airctaft TYPES from the list"">SEL</span></th>")
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th data-priority=""1"">TYPE</th>")
                    htmlOut.Append("<th>MAX RANGE</th>")
                    htmlOut.Append("<th>MAX PASSENGERS</th>")

                    htmlOut.Append("</tr></thead><tbody>")

                    Dim nCount As Integer = 0

                    For Each r As DataRow In results_table.Rows

                        arrWeightTypeSelection(nCount, 0) = nCount.ToString

                        If Not IsDBNull(r.Item("amod_weight_class")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_weight_class").ToString.Trim) Then
                                arrWeightTypeSelection(nCount, 1) = r.Item("amod_weight_class").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("amod_airframe_type_code")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_airframe_type_code").ToString.Trim) Then
                                arrWeightTypeSelection(nCount, 2) = r.Item("amod_airframe_type_code").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("acwgtcls_display_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("acwgtcls_display_name").ToString.Trim) Then
                                arrWeightTypeSelection(nCount, 3) = r.Item("acwgtcls_display_name").ToString.Trim
                            End If
                        End If

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + nCount.ToString + "</td>")

                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                        If Not IsDBNull(r.Item("acwgtcls_display_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("acwgtcls_display_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("acwgtcls_display_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")

                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                        If Not IsDBNull(r.Item("acwgtcls_display_range")) Then
                            If Not String.IsNullOrEmpty(r.Item("acwgtcls_display_range").ToString.Trim) Then
                                htmlOut.Append(r.Item("acwgtcls_display_range").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")

                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" & r.Item("acwgtcls_sort_order").ToString & """>")

                        If Not IsDBNull(r.Item("acwgtcls_display_passengers")) Then
                            If Not String.IsNullOrEmpty(r.Item("acwgtcls_display_passengers").ToString.Trim) Then
                                htmlOut.Append(r.Item("acwgtcls_display_passengers").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")

                        htmlOut.Append("</tr>")

                        nCount += 1

                    Next

                    htmlOut.Append("</tbody></table>")
                    htmlOut.Append("<div id=""aircraftLabel"" class="""" style=""padding:2px;""><strong>Check the box of all desired aircraft types</strong></div>")
                    htmlOut.Append("<div id=""aircraftInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

                End If

            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_airctaft_type_HTMLtable(ByRef out_htmlString As String)</b><br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAircraftFinderDataTable(ByRef criteria As finderCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Try

            sQuery.Append("SELECT DISTINCT ac_id, amod_make_name, amod_model_name, amod_number_of_passengers, amod_max_range_miles, amod_range_tanks_full,")
            sQuery.Append(" ac_ser_no_full, ac_reg_no, ac_forsale_flag, ac_status, ac_asking, ac_asking_price, ac_mfr_year, ac_airframe_tot_hrs")
            sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Weight_Class WITH(NOLOCK) ON amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code")
            sQuery.Append(" INNER JOIN Aircraft_Key_Feature WITH(NOLOCK) ON ac_id = afeat_ac_id AND ac_journ_id = afeat_journ_id")

            If criteria.FinderEngine_Maintenance_Program = True Then
                If Not String.IsNullOrEmpty(criteria.FinderEngine_Maintenance_Program_Type) Then
                    sQuery.Append(" INNER JOIN Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id")
                End If
            End If

            sQuery.Append(" WHERE (ac_journ_id = 0)")

            sQuery.Append(Constants.cAndClause + "  (ac_lifecycle_stage = '3')  ")

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False, True))

            If criteria.FinderAirframe_Maintenance_Program = True Then
                sQuery.Append(Constants.cAndClause + "( ac_id in (select distinct acattind_ac_id from Aircraft_Attribute_Index with (NOLOCK) where acattind_acatt_id = 264)) ")
            End If

            ' FROM Engine_Maintenance_Program WITH(NOLOCK) WHERE emp_id =

            If criteria.FinderEngine_Maintenance_Program = True Then
                sQuery.Append(Constants.cAndClause + "( ac_id in (select distinct acattind_ac_id from Aircraft_Attribute_Index with (NOLOCK) where acattind_acatt_id = 223)) ")

                If Not String.IsNullOrEmpty(criteria.FinderEngine_Maintenance_Program_Type) Then
                    sQuery.Append(Constants.cAndClause + " ( emp_program_name = '" & criteria.FinderEngine_Maintenance_Program_Type.ToString & "')")
                End If
            End If

            If criteria.FinderCriteriaStartAFTT > 0 Then
                sQuery.Append(Constants.cAndClause + "((ac_airframe_tot_hrs >= " + criteria.FinderCriteriaStartAFTT.ToString + ") OR (ac_airframe_tot_hrs IS NULL))")
            End If

            If criteria.FinderCriteriaEndAFTT > 0 Then
                sQuery.Append(Constants.cAndClause + "((ac_airframe_tot_hrs <= " + criteria.FinderCriteriaEndAFTT.ToString + ") OR (ac_airframe_tot_hrs IS NULL))")
            End If

            If criteria.FinderCriteriaStartYear > 0 Then
                sQuery.Append(Constants.cAndClause + "ac_mfr_year >= " + criteria.FinderCriteriaStartYear.ToString)
            End If

            If criteria.FinderCriteriaEndYear > 0 Then
                sQuery.Append(Constants.cAndClause + "ac_mfr_year <= " + criteria.FinderCriteriaEndYear.ToString)
            End If

            If criteria.FinderCriteriaStartPAX > 0 Then
                sQuery.Append(Constants.cAndClause + "amod_number_of_passengers <= " + criteria.FinderCriteriaStartPAX.ToString)
            End If

            If criteria.FinderCriteriaEndPAX > 0 Then
                sQuery.Append(Constants.cAndClause + "amod_number_of_passengers >= " + criteria.FinderCriteriaEndPAX.ToString)
                sQuery.Append(Constants.cAndClause + "amod_number_of_passengers <= " + maxPAX.ToString)
            End If

            If criteria.FinderCriteriaStartRange > 0 Then
                sQuery.Append(Constants.cAndClause + "amod_max_range_miles <= " + criteria.FinderCriteriaStartRange.ToString)
                'sQuery.Append(Constants.cOrClause + "amod_range_tanks_full >= " + criteria.FinderCriteriaStartRange.ToString + " )")
            End If

            If criteria.FinderCriteriaEndRange > 0 Then
                sQuery.Append(Constants.cAndClause + "amod_max_range_miles >= " + criteria.FinderCriteriaEndRange.ToString)
                'sQuery.Append(Constants.cOrClause + "amod_range_tanks_full > 0 AND amod_range_tanks_full <= " + criteria.FinderCriteriaEndRange.ToString + " )")
            End If


            If criteria.FinderCriteriaJustForSale Then

                sQuery.Append(Constants.cAndClause + "ac_forsale_flag = 'Y'")

            End If

            If criteria.FinderCriteriaStartPrice > 0 Or criteria.FinderCriteriaEndPrice > 0 Then
                sQuery.Append(Constants.cAndClause + " (( ")

                'If criteria.FinderCriteriaStartPrice > 0 Then
                sQuery.Append("ac_asking_price > " + criteria.FinderCriteriaStartPrice.ToString)
                'End If

                If criteria.FinderCriteriaEndPrice > 0 Then
                    sQuery.Append(Constants.cAndClause + "ac_asking_price < " + criteria.FinderCriteriaEndPrice.ToString)
                End If

                sQuery.Append(")" & Constants.cOrClause + " ( ")

                sQuery.Append(" ac_id in (Select distinct afmv_ac_id ")
                sQuery.Append("	from Aircraft_FMV with (NOLOCK) ")
                sQuery.Append("	where afmv_latest_flag='Y' ")

                'If criteria.FinderCriteriaStartPrice > 0 Then
                sQuery.Append("	and afmv_value > " + criteria.FinderCriteriaStartPrice.ToString)
                'End If

                If criteria.FinderCriteriaEndPrice > 0 Then
                    sQuery.Append(" and afmv_value < " + criteria.FinderCriteriaEndPrice.ToString + ")  ")
                End If

                sQuery.Append(" )) ")
            End If



            If Not String.IsNullOrEmpty(criteria.FinderCriteriaLocation) Then
                If Not criteria.FinderCriteriaLocation.ToUpper.Contains("WORLDWIDE") Then
                    If criteria.FinderCriteriaLocation.ToUpper.Contains("N") Then
                        sQuery.Append(Constants.cAndClause + "ac_reg_no LIKE 'N%'")
                    ElseIf criteria.FinderCriteriaLocation.ToUpper.Contains("I") Then
                        sQuery.Append(Constants.cAndClause + "ac_reg_no NOT LIKE 'N%'")
                    End If
                End If
            End If

            Dim wtcls_class As String = ""
            Dim wtcls_airframe As String = ""

            If Not String.IsNullOrEmpty(criteria.FinderCriteriaWeightClass.Trim) Then
                Dim selectedWeightType() As String = Split(criteria.FinderCriteriaWeightClass, Constants.cMultiDelim)

                ' loop through each "selected row" data will be the index of that item in the array ..
                For Each wct As String In selectedWeightType


                    If String.IsNullOrEmpty(wtcls_class.Trim) Then
                        wtcls_class = arrWeightTypeSelection(CInt(wct), 1)
                    Else
                        If Not wtcls_class.ToUpper.Contains(arrWeightTypeSelection(CInt(wct), 1).ToUpper) Then
                            wtcls_class += Constants.cCommaDelim + arrWeightTypeSelection(CInt(wct), 1)
                        End If
                    End If

                    If String.IsNullOrEmpty(wtcls_airframe.Trim) Then
                        wtcls_airframe = arrWeightTypeSelection(CInt(wct), 2)
                    Else
                        If Not wtcls_airframe.ToUpper.Contains(arrWeightTypeSelection(CInt(wct), 2).ToUpper) Then
                            wtcls_airframe += Constants.cCommaDelim + arrWeightTypeSelection(CInt(wct), 2)
                        End If
                    End If

                Next

                sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + wtcls_class.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
                sQuery.Append(Constants.cAndClause + "amod_airframe_type_code IN ('" + wtcls_airframe.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")

            End If

            If Not IsNothing(criteria.FinderCriteriaFeatures) Then
                Dim featureString As String = ""
                For Each featName In criteria.FinderCriteriaFeatures
                    If featureString <> "" Then
                        featureString += Constants.cAndClause
                    End If

                    featureString += " ac_id in ( select distinct acattind_ac_id from Aircraft_Attribute_Index with (NOLOCK) where acattind_acatt_id = '" + featName + "')"
                Next

                If Not String.IsNullOrEmpty(featureString) Then
                    sQuery.Append(Constants.cAndClause + " " + featureString + " ")
                End If
            End If

            sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAircraftFinderDataTable(ByRef criteria As finderCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAircraftFinderDataTable(ByRef criteria As finderCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Sub get_airctaft_search_Array(ByRef searchCriteria As finderCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim ReturnString As String = ""
        'Const RECORD_LIMIT As Integer = 1000

        Try

            results_table = getAircraftFinderDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim nCount As Integer = 0

                    For Each r As DataRow In results_table.Rows
                        If nCount > 0 Then
                            htmlOut.Append(",")
                        End If
                        'If nCount <= RECORD_LIMIT Then
                        nCount += 1
                        'Else
                        '  Exit For
                        'End If


                        htmlOut.Append("{")
                        htmlOut.Append("""SEL"": """",")
                        htmlOut.Append("""MAKE"": """)
                        If Not IsDBNull(r.Item("amod_make_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                                htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(r.Item("amod_make_name").ToString.Trim))
                            End If
                        End If


                        htmlOut.Append(""",")

                        htmlOut.Append("""MODEL"": """)



                        If Not IsDBNull(r.Item("amod_model_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                                htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(r.Item("amod_model_name").ToString.Trim))
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""SERNO"": """)


                        If Not IsDBNull(r.Item("ac_ser_no_full")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString.Trim) Then
                                htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, True, r.Item("ac_ser_no_full").ToString.Trim, "text_underline", "")))

                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""REGNO"": """)


                        If Not IsDBNull(r.Item("ac_reg_no")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
                                htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(r.Item("ac_reg_no").ToString.Trim))
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""STATUS"": """)
                        If Not IsDBNull(r.Item("ac_forsale_flag")) Then

                            If r.Item("ac_forsale_flag").ToString.Trim.ToUpper.Contains("Y") Then

                                htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(r.Item("ac_status").ToString.Trim) + """,")

                                If Not IsDBNull(r.Item("ac_asking")) Then
                                    If r.Item("ac_asking").ToString.Trim.ToLower.Contains("price") Then
                                        If Not IsDBNull(r.Item("ac_asking_price")) Then

                                            Dim tmpPrice As Long = 0
                                            If IsNumeric(r.Item("ac_asking_price").ToString) Then
                                                If CLng(r.Item("ac_asking_price").ToString) > 0 Then
                                                    tmpPrice = CLng(r.Item("ac_asking_price").ToString) / 1000
                                                End If
                                            End If

                                            htmlOut.Append("""ASKING"": ""$" + FormatNumber(tmpPrice, 0, False, False, True) + "k"",")
                                        Else
                                            htmlOut.Append("""ASKING"":"""",")
                                        End If
                                    Else
                                        htmlOut.Append("""ASKING"":""" + r.Item("ac_asking").ToString.Trim + """,")
                                    End If
                                Else
                                    htmlOut.Append("""ASKING"":"""",")
                                End If

                            Else
                                If Not IsDBNull(r.Item("ac_status")) Then
                                    htmlOut.Append(clsGeneral.clsGeneral.PrepForJS(r.Item("ac_status").ToString.Trim) + """,")
                                    htmlOut.Append("""ASKING"":"""",")
                                Else
                                    htmlOut.Append(""",")
                                    htmlOut.Append("""ASKING"":"""",")
                                End If
                            End If

                        Else
                            If Not IsDBNull(r.Item("ac_status")) Then
                                htmlOut.Append(r.Item("ac_status").ToString.Trim + """,")
                                htmlOut.Append("""ASKING"":"""",")
                            Else
                                htmlOut.Append(""",")
                                htmlOut.Append("""ASKING"":"""",")
                            End If
                        End If

                        htmlOut.Append("""YEARMFG"":""")

                        If Not IsDBNull(r.Item("ac_mfr_year")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_mfr_year").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_mfr_year").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""PAX"":""")

                        If Not IsDBNull(r.Item("amod_number_of_passengers")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_number_of_passengers").ToString.Trim) Then
                                htmlOut.Append(r.Item("amod_number_of_passengers").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(""",")

                        htmlOut.Append("""RANGE"":""")

                        Dim maxRange As Long = 0

                        If Not IsDBNull(r.Item("amod_max_range_miles")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_max_range_miles").ToString.Trim) Then
                                If IsNumeric(r.Item("amod_max_range_miles").ToString.Trim) Then
                                    maxRange = CLng(r.Item("amod_max_range_miles").ToString)
                                End If
                            End If
                        End If

                        'If maxRange = 0 Then
                        '  If Not IsDBNull(r.Item("amod_range_tanks_full")) Then
                        '    If Not String.IsNullOrEmpty(r.Item("amod_range_tanks_full").ToString.Trim) Then
                        '      If IsNumeric(r.Item("amod_range_tanks_full").ToString.Trim) Then
                        '        maxRange = CLng(r.Item("amod_range_tanks_full").ToString)
                        '      End If
                        '    End If
                        '  End If
                        'End If

                        htmlOut.Append(maxRange.ToString.Trim)

                        htmlOut.Append(""",")

                        htmlOut.Append("""AFTT"":""")

                        If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_hrs").ToString.Trim) Then
                                htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("""")

                        htmlOut.Append("}")

                    Next


                    'htmlOut.Append("</tbody></table></div>")
                    'htmlOut.Append("<div id=""searchLabel"" class="""" style=""padding:2px;""><strong>" + results_table.Rows.Count.ToString + " aircraft</strong></div>")
                    'htmlOut.Append("<div id=""searchInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

                End If

            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_airctaft_search_HTMLtable(ByRef out_htmlString As String)</b><br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    'Public Sub get_airctaft_search_HTMLtable(ByRef searchCriteria As finderCriteriaClass, ByRef out_htmlString As String)

    '  Dim results_table As New DataTable
    '  Dim htmlOut As New StringBuilder

    '  Const RECORD_LIMIT As Integer = 1000

    '  Try

    '    results_table = getAircraftFinderDataTable(searchCriteria)

    '    If Not IsNothing(results_table) Then

    '      If results_table.Rows.Count > 0 Then

    '        htmlOut.Append("<div id=""acSearchResultsContainer""><table id=""searchDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
    '        htmlOut.Append("<thead><tr>")
    '        htmlOut.Append("<th width=""10""><span class=""help_cursor"" title=""Used to select and remove airctaft from the list"">SEL</span></th>")
    '        htmlOut.Append("<th></th>")
    '        htmlOut.Append("<th data-priority=""1"">MAKE</th>")
    '        htmlOut.Append("<th>MODEL</th>")
    '        htmlOut.Append("<th>SERNO</th>")
    '        htmlOut.Append("<th>REGNO</th>")
    '        htmlOut.Append("<th>STATUS</th>")
    '        htmlOut.Append("<th>ASKING</th>")
    '        htmlOut.Append("<th>YEARMFG</th>")
    '        htmlOut.Append("<th>PAX</th>")
    '        htmlOut.Append("<th>RANGE</th>")

    '        htmlOut.Append("<th>AFTT</th>")

    '        htmlOut.Append("</tr></thead><tbody>")

    '        Dim nCount As Integer = 0

    '        For Each r As DataRow In results_table.Rows

    '          If nCount <= RECORD_LIMIT Then
    '            nCount += 1
    '          Else
    '            Exit For
    '          End If

    '          htmlOut.Append("<tr>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '          htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")

    '          htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("amod_make_name")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
    '              htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("amod_model_name")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
    '              htmlOut.Append(r.Item("amod_model_name").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_ser_no_full")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString.Trim) Then

    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_reg_no")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ac_reg_no").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          If Not IsDBNull(r.Item("ac_forsale_flag")) Then

    '            If r.Item("ac_forsale_flag").ToString.Trim.ToUpper.Contains("Y") Then

    '              htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")

    '              If Not IsDBNull(r.Item("ac_asking")) Then
    '                If r.Item("ac_asking").ToString.Trim.ToLower.Contains("price") Then
    '                  If Not IsDBNull(r.Item("ac_asking_price")) Then

    '                    Dim tmpPrice As Long = 0
    '                    If IsNumeric(r.Item("ac_asking_price").ToString) Then
    '                      If CLng(r.Item("ac_asking_price").ToString) > 0 Then
    '                        tmpPrice = CLng(r.Item("ac_asking_price").ToString) / 1000
    '                      End If
    '                    End If

    '                    htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">$" + FormatNumber(tmpPrice, 0, False, False, True) + "k</td>")
    '                  Else
    '                    htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '                  End If
    '                Else
    '                  htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_asking").ToString.Trim + "</td>")
    '                End If
    '              Else
    '                htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              End If

    '            Else
    '              If Not IsDBNull(r.Item("ac_status")) Then
    '                htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")
    '                htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              Else
    '                htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '                htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              End If
    '            End If

    '          Else
    '            If Not IsDBNull(r.Item("ac_status")) Then
    '              htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_status").ToString.Trim + "</td>")
    '              htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '            Else
    '              htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '              htmlOut.Append("<td align=""center"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
    '            End If
    '          End If

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_mfr_year")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_mfr_year").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ac_mfr_year").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("amod_number_of_passengers")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_number_of_passengers").ToString.Trim) Then
    '              htmlOut.Append(r.Item("amod_number_of_passengers").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          Dim maxRange As Long = 0

    '          If Not IsDBNull(r.Item("amod_max_range_miles")) Then
    '            If Not String.IsNullOrEmpty(r.Item("amod_max_range_miles").ToString.Trim) Then
    '              If IsNumeric(r.Item("amod_max_range_miles").ToString.Trim) Then
    '                maxRange = CLng(r.Item("amod_max_range_miles").ToString)
    '              End If
    '            End If
    '          End If

    '          'If maxRange = 0 Then
    '          '  If Not IsDBNull(r.Item("amod_range_tanks_full")) Then
    '          '    If Not String.IsNullOrEmpty(r.Item("amod_range_tanks_full").ToString.Trim) Then
    '          '      If IsNumeric(r.Item("amod_range_tanks_full").ToString.Trim) Then
    '          '        maxRange = CLng(r.Item("amod_range_tanks_full").ToString)
    '          '      End If
    '          '    End If
    '          '  End If
    '          'End If

    '          htmlOut.Append(maxRange.ToString.Trim)

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"">")

    '          If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
    '            If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_hrs").ToString.Trim) Then
    '              htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString.Trim)
    '            End If
    '          End If

    '          htmlOut.Append("</td>")

    '          htmlOut.Append("</tr>")

    '        Next


    '        htmlOut.Append("</tbody></table></div>")
    '        htmlOut.Append("<div id=""searchLabel"" class="""" style=""padding:2px;""><strong>" + results_table.Rows.Count.ToString + " aircraft</strong></div>")
    '        htmlOut.Append("<div id=""searchInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

    '      End If

    '    End If

    '  Catch ex As Exception

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_airctaft_search_HTMLtable(ByRef out_htmlString As String)</b><br />" + ex.Message

    '  Finally

    '  End Try

    '  'return resulting html string
    '  out_htmlString = htmlOut.ToString
    '  htmlOut = Nothing
    '  results_table = Nothing

    'End Sub


    Private Sub findAC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles findAC.Click

        Dim sAircraftSearchHTMLtable As String = ""
        Dim sEnglishSummary As String = ""

        get_airctaft_search_Array(objSearchCriteria, sAircraftSearchHTMLtable)
        'tab_container_bottom.Visible = True
        'tab_container_top.ActiveTabIndex = 1

        fill_english_summary(objSearchCriteria, sEnglishSummary)

        If Not String.IsNullOrEmpty(sEnglishSummary.Trim) Then
            english_summary.Text = sEnglishSummary
        End If

        'If Not String.IsNullOrEmpty(sAircraftSearchHTMLtable.Trim) Then
        '  bIsSearch = True
        '  acSearchResultsTable.Font.Bold = False
        '  acSearchResultsTable.ForeColor = System.Drawing.SystemColors.WindowText
        'acSearchResultsTable.Text = " var dataSetAC = [" & sAircraftSearchHTMLtable & "];"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.tabContainerBottomUpdate, Me.GetType(), "AcFinderArray", " var dataSetAC = [" & sAircraftSearchHTMLtable & "];", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(tabContainerBottomUpdate, Me.GetType(), "onLoadPostback", "CreateSearchTable('searchInnerTable','searchDataTable','searchjQueryTable');CloseLoadingMessage(""DivLoadingMessage"");", True)

        'Else
        'acSearchResultsTable.Text = "<p align=""center"">No Aircraft matched the selected critera</p>"
        'acSearchResultsTable.CssClass = "padding"
        'acSearchResultsTable.Font.Bold = True
        'acSearchResultsTable.ForeColor = System.Drawing.Color.Red
        'End If

    End Sub

    Sub fill_english_summary(ByRef criteria As finderCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Try

            htmlOut.Append("Current Preferences : <br/> Select Aircraft where")

            Dim wtcls_type_name As String = ""

            If Not String.IsNullOrEmpty(criteria.FinderCriteriaWeightClass.Trim) Then
                Dim selectedWeightType() As String = Split(criteria.FinderCriteriaWeightClass, Constants.cMultiDelim)

                ' loop through each "selected row" data will be the index of that item in the array ..
                For Each wct As String In selectedWeightType

                    If String.IsNullOrEmpty(wtcls_type_name.Trim) Then
                        wtcls_type_name = arrWeightTypeSelection(CInt(wct), 3)
                    Else
                        If Not wtcls_type_name.ToUpper.Contains(arrWeightTypeSelection(CInt(wct), 3).ToUpper) Then
                            wtcls_type_name += Constants.cMultiDelim + arrWeightTypeSelection(CInt(wct), 3)
                        End If
                    End If

                Next

                htmlOut.Append(" the aircraft type(s) " + wtcls_type_name.Trim)

            End If

            If criteria.FinderCriteriaStartAFTT > 0 Then
                If Not String.IsNullOrEmpty(criteria.FinderCriteriaWeightClass.Trim) Then
                    htmlOut.Append(" and AFTT")
                Else
                    htmlOut.Append(" AFTT")
                End If
                htmlOut.Append(" is greater than or equal to " + criteria.FinderCriteriaStartAFTT.ToString + " hours")
            End If

            If criteria.FinderCriteriaEndAFTT > 0 Then
                If criteria.FinderCriteriaStartAFTT > 0 Then
                    htmlOut.Append(" and")
                Else
                    If Not String.IsNullOrEmpty(criteria.FinderCriteriaWeightClass.Trim) Then
                        htmlOut.Append(" and AFTT")
                    Else
                        htmlOut.Append(" AFTT")
                    End If
                End If

                htmlOut.Append(" is less than or equal to " + criteria.FinderCriteriaEndAFTT.ToString + " hours")
            End If

            If criteria.FinderCriteriaStartYear > 0 Then
                If criteria.FinderCriteriaStartAFTT > 0 Or criteria.FinderCriteriaEndAFTT > 0 Then
                    htmlOut.Append(", manufactured year")
                Else
                    htmlOut.Append(" manufactured year")
                End If
                htmlOut.Append(" is greater than or equal to " + criteria.FinderCriteriaStartYear.ToString)
            End If

            If criteria.FinderCriteriaEndYear > 0 Then
                If criteria.FinderCriteriaStartYear > 0 Then
                    htmlOut.Append(" and")
                Else
                    htmlOut.Append(", manufactured year")
                End If
                htmlOut.Append(" is less than or equal to " + criteria.FinderCriteriaEndYear.ToString)
            End If

            If criteria.FinderCriteriaStartPAX > 0 Then
                htmlOut.Append(", maximum number of passengers is less than " + criteria.FinderCriteriaStartPAX.ToString)
            End If

            If criteria.FinderCriteriaEndPAX > 0 Then
                If criteria.FinderCriteriaStartPAX > 0 Then
                    htmlOut.Append(" and")
                Else
                    htmlOut.Append(", number of passengers")
                End If

                htmlOut.Append(" is between " + criteria.FinderCriteriaEndPAX.ToString + " and " + maxPAX.ToString)

            End If

            If criteria.FinderCriteriaStartRange > 0 Then
                htmlOut.Append(", flight range is less than or equal to " + criteria.FinderCriteriaStartRange.ToString + "(statue miles)")
            End If

            If criteria.FinderCriteriaEndRange > 0 Then
                If criteria.FinderCriteriaStartRange > 0 Then
                    htmlOut.Append(" and")
                Else
                    htmlOut.Append(", flight range")
                End If
                htmlOut.Append(" is greater than or equal to " + criteria.FinderCriteriaEndRange.ToString + "(statue miles)")
            End If

            If criteria.FinderCriteriaJustForSale Then

                htmlOut.Append(", aircraft For Sale and")

                If criteria.FinderCriteriaStartPrice > 0 Then
                    htmlOut.Append(" asking price is greater than or equal to " + criteria.FinderCriteriaStartPrice.ToString)
                End If

                If criteria.FinderCriteriaEndPrice > 0 Then
                    If criteria.FinderCriteriaStartPrice > 0 Then
                        htmlOut.Append(" and")
                    End If
                    htmlOut.Append(" asking price is less than or equal to " + criteria.FinderCriteriaEndPrice.ToString)
                End If

            End If

            If Not IsNothing(criteria.FinderCriteriaFeatures) Then

                Dim results_table As New DataTable
                Dim feature_name As String = ""

                results_table = getAircraftFinderComfortFeaturesDataTable(criteria.FinderCriteriaFeatures)

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            If Not IsDBNull(r.Item("kfeat_name")) Then
                                If String.IsNullOrEmpty(feature_name.Trim) Then
                                    feature_name = r.Item("kfeat_name").ToString.Trim
                                Else
                                    If Not feature_name.ToUpper.Trim.Contains(r.Item("kfeat_name").ToString.ToUpper.Trim) Then
                                        feature_name += Constants.cMultiDelim + r.Item("kfeat_name").ToString.Trim
                                    End If
                                End If
                            End If

                        Next

                    End If
                End If

                htmlOut.Append(", also including these feature(s) " + feature_name.Trim)

            End If

            If Not String.IsNullOrEmpty(criteria.FinderCriteriaLocation) Then
                If Not criteria.FinderCriteriaLocation.ToUpper.Contains("WORLDWIDE") Then
                    If criteria.FinderCriteriaLocation.ToUpper.Contains("N") Then
                        htmlOut.Append(" while looking at all domestic aircraft")
                    ElseIf criteria.FinderCriteriaLocation.ToUpper.Contains("I") Then
                        htmlOut.Append(" while looking at all international aircraft")
                    End If
                Else
                    htmlOut.Append(" while looking at all aircraft worldwide")
                End If
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in fill_english_summary(ByRef searchCriteria As finderCriteriaClass, ByRef out_htmlString As String)</b><br />" + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

End Class