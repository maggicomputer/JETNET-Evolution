' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Aircraft_Listing.aspx.vb $
'$$Author: Amanda $
'$$Date: 6/22/20 3:09p $
'$$Modtime: 6/22/20 3:08p $
'$$Revision: 30 $
'$$Workfile: Aircraft_Listing.aspx.vb $
'
' ********************************************************************************


Partial Public Class Aircraft_Listing
    Inherits System.Web.UI.Page
    Public productCodeCount As Integer = 0
    Public isHeliOnlyProduct As Boolean = False

    Dim TempTable As New DataTable
    Dim TypeDataTable As New DataTable
    Dim TypeDataHold As New DataTable
    Dim PageNumber As Integer = 1
    Dim PageSort As String = ""
    Dim foundChild As New DropDownList
    Dim FieldCounter As Integer = 0
    Dim Query_Class_Array As New ArrayList()
    Dim History As Boolean = False
    Dim MarketEvent As Boolean = False
    Dim ErrorReportingTypeString As String = "Aircraft"
    Private sTypeMakeModelCtrlBaseName As String = "Aircraft"
    Dim StaticAIRCRAFTIDs As String = ""
    Public bUsernameExists As Boolean = False
    Private localCriteria As New viewSelectionCriteriaClass
    Public Shared masterPage As New Object
    Public displayEvalues As Boolean = False
    Dim LookupDataSet As New DataSet

    Private Sub ClearSavedSelection()
        Try
            'Clear out the Type/Make/Model Boxes Properly on Reset:
            HttpContext.Current.Session.Item("tabAircraftType") = ""
            HttpContext.Current.Session.Item("tabAircraftMake") = ""
            HttpContext.Current.Session.Item("tabAircraftModel") = ""
            HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
            HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
            HttpContext.Current.Session.Item("tabAircraftSize") = ""
            HttpContext.Current.Session.Item("hasModelFilter") = False

            HttpContext.Current.Session.Item("chkHelicopterFilter") = False
            HttpContext.Current.Session.Item("chkBusinessFilter") = False
            HttpContext.Current.Session.Item("chkCommercialFilter") = False

            HttpContext.Current.Session.Item("companyRegion") = ""
            HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent"
            HttpContext.Current.Session.Item("companyTimeZone") = ""
            HttpContext.Current.Session.Item("companyCountry") = ""
            HttpContext.Current.Session.Item("companyState") = ""

            HttpContext.Current.Session.Item("baseRegion") = ""
            HttpContext.Current.Session.Item("baseRegionOrContinent") = "continent"
            HttpContext.Current.Session.Item("baseCountry") = ""
            HttpContext.Current.Session.Item("baseState") = ""

            HttpContext.Current.Session.Item("eventCatType") = ""
            HttpContext.Current.Session.Item("eventCatCode") = ""
            HttpContext.Current.Session.Item("eventType") = "AIRCRAFT"

            'Clear the search class/reset it
            HttpContext.Current.Session.Item("searchCriteria") = New SearchSelectionCriteria

            HttpContext.Current.Session.Item("MasterAircraftWhere") = ""
            HttpContext.Current.Session.Item("MasterAircraftFrom") = ""
            HttpContext.Current.Session.Item("MasterAircraftSelect") = ""
            HttpContext.Current.Session.Item("MasterAircraftSort") = ""


            'This goes through and finds all the advanced search items and clears it.
            Dim I As Integer = 0
            Dim L As Integer = Session.Contents.Count
            Dim keyName As String

            For I = L - 1 To 0 Step -1
                If TypeOf (Session.Contents.Item(I)) Is String Then
                    If InStr(Session.Contents.Keys(I).ToString(), "Advanced-") > 0 Then

                        keyName = Session.Contents.Keys(I).ToString()
                        Session.Remove(keyName)
                    End If
                End If
            Next
        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function ShowDom(ByVal DOM As Object) As String
        If Not IsDBNull(DOM) Then
            Return "<br /><span class=""tiny_text"">(DOM: " & DateDiff(DateInterval.Day, DOM, Now()) & ")</span>"
        Else
            Return ""
        End If
    End Function

    Private Sub Aircraft_Listing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try


            If Not Page.IsPostBack Then
                retail_transaction_label.Text = "<a href=""#"" class=""help_cursor"" title=""What are Retail Transactions? Click to Learn More"" onClick=""javascript:load('help/helpexamples/454.pdf','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">Retail Transactions</a>"
                If Aircraft_Criteria.Visible = True Then

                    SwitchGalleryListing(Session.Item("localUser").crmACListingView)
                    'This needs to be done on load complete because otherwise the array of the models is not stored in session yet
                    'and the first time we complete a project or homepage search, it will not work (until the array is filled later on
                    'in page lifecycle)
                    If Page.Request.Form("complete_search") = "Y" Or Page.Request.Form("project_search") = "Y" Then
                        'if either of these variables is passed, then go ahead and complete this search\

                        acsearch_Click(acsearch, EventArgs.Empty)
                    End If
                End If
            End If

            Aircraft_Criteria.Focus()
        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try
    End Sub

    Public Shared Function DisplayClientAircraft(ByVal JetnetID As Object, ByVal jetnetAFTT As Object) As String
        Dim returnString As String = ""
        If clsGeneral.clsGeneral.isCrmDisplayMode() Then
            If IsNumeric(JetnetID) Then
                Dim clsDataTemp As New clsData_Manager_SQL
                Dim seperator As String = ""
                clsDataTemp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                If Not String.IsNullOrEmpty(clsDataTemp.client_DB) Then
                    Dim TemporaryTable As New DataTable
                    TemporaryTable = clsDataTemp.Get_Client_Aircraft_JETNET_AC(JetnetID)
                    If Not IsNothing(TemporaryTable) Then
                        If TemporaryTable.Rows.Count > 0 Then
                            returnString = "<span class=""label mediumText"">CLIENT RECORD:</span> S/N " & crmWebClient.DisplayFunctions.WriteDetailsLink(TemporaryTable.Rows(0).Item("cliaircraft_id"), 0, 0, 0, True, TemporaryTable.Rows(0).Item("cliaircraft_ser_nbr").ToString, "text_underline", "&SOURCE=CLIENT")
                            If TemporaryTable.Rows(0).Item("cliaircraft_forsale_flag") = "Y" Then
                                'Select Case TemporaryTable.Rows(0).Item("cliaircraft_status")
                                '  Case "For Sale"
                                '    returnString = "<strong>FOR SALE</strong>"
                                '    seperator = Constants.cCommaDelim
                                '  Case Else
                                returnString += "<br />" & TemporaryTable.Rows(0).Item("cliaircraft_status")
                                seperator += Constants.cCommaDelim
                                ' End Select


                                If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")) Then
                                    If TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage") <> "" Then
                                        If Trim(TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")) = "Price" Then
                                            If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_asking_price")) Then
                                                seperator = Constants.cCommaDelim
                                                returnString += seperator + " <span class=""label"">Asking:</span> " & FormatCurrency((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_asking_price").ToString) / 1000), 0) & "k"
                                            End If
                                        Else
                                            seperator = Constants.cCommaDelim
                                            returnString += seperator + " " & TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")
                                        End If
                                    End If
                                End If

                                If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_est_price")) Then
                                    If TemporaryTable.Rows(0).Item("cliaircraft_est_price") <> 0 Then
                                        returnString += seperator + " <span class=""label"">Take Price:</span> " & FormatCurrency((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_est_price").ToString) / 1000), 0) & "k"
                                    End If
                                End If
                            End If


                            If Not IsNumeric(jetnetAFTT) Then
                                jetnetAFTT = 0
                            End If

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_value_description")) Then
                                If TemporaryTable.Rows(0).Item("cliaircraft_value_description") <> "" Then
                                    returnString += "<br />" & TemporaryTable.Rows(0).Item("cliaircraft_value_description")
                                End If
                            End If

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours")) Then
                                If TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours") <> 0 And jetnetAFTT <> TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours") Then
                                    returnString += seperator + "<br /><span class=""label"">AFTT:</span> " & FormatNumber((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours").ToString)), 0)
                                End If
                            End If



                            If returnString <> "" Then
                                returnString = "<div class=""CLIENTCRMRow"">" & returnString & "</div>"
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return returnString
    End Function

    Public Function DisplayClientAircraftRow(ByVal JetnetID As Object, ByVal jetnetAFTT As Object, ByVal make As Object, ByVal model As Object, ByVal modelID As Object) As String
        Dim returnString As String = ""
        If clsGeneral.clsGeneral.isCrmDisplayMode() Then
            If IsNumeric(JetnetID) Then
                Dim clsDataTemp As New clsData_Manager_SQL
                Dim seperator As String = ""
                clsDataTemp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                If Not String.IsNullOrEmpty(clsDataTemp.client_DB) Then
                    Dim TemporaryTable As New DataTable
                    TemporaryTable = clsDataTemp.Get_Client_Aircraft_as_Jetnet_Fields_By_JetnetID(JetnetID)
                    If Not IsNothing(TemporaryTable) Then
                        If TemporaryTable.Rows.Count > 0 Then
                            returnString = "<tr class=""CLIENTCRMRow"">"
                            returnString += "<td align=""left"" valign=""top""></td>"
                            returnString += "<td align=""left"" valign=""top"">"

                            If Not IsDBNull(make) And Not IsDBNull(model) And Not IsDBNull(modelID) Then
                                returnString += DisplayFunctions.WriteModelLink(modelID, model, True)
                                returnString += " " & make
                            End If
                            returnString += "</td>"
                            returnString += "<td align=""left"" valign=""top"">"

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_year")) Then
                                returnString += TemporaryTable.Rows(0).Item("ac_year")
                            End If
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_mfr_year")) Then
                                returnString += "<br />" & TemporaryTable.Rows(0).Item("ac_mfr_year")
                            End If
                            returnString += "</td>"
                            returnString += "<td align=""left"" valign=""top"">" & crmWebClient.DisplayFunctions.WriteDetailsLink(TemporaryTable.Rows(0).Item("CLIENT_ID"), 0, 0, 0, True, TemporaryTable.Rows(0).Item("ac_ser_nbr").ToString, "text_underline", "&SOURCE=CLIENT") & "</td>"
                            returnString += "<td align=""left"" valign=""top"">"

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_reg_no")) Then
                                returnString += TemporaryTable.Rows(0).Item("ac_reg_no")
                            End If

                            returnString += "</td>"
                            If Session.Item("localSubscription").crmAerodexFlag = False Then
                                returnString += "<td align=""left"" valign=""top"">"

                                If TemporaryTable.Rows(0).Item("ac_forsale_flag") = "Y" Then
                                    returnString += "<span class='green_background'>"
                                    returnString += TemporaryTable.Rows(0).Item("ac_status")
                                    If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_asking_price")) Then
                                        If TemporaryTable.Rows(0).Item("ac_asking") = "Price" Then
                                            returnString += "<br /><span class=""emphasis_text"">" & crmWebClient.clsGeneral.clsGeneral.no_zero(TemporaryTable.Rows(0).Item("ac_asking_price"), "", True) & "</span>"
                                        End If
                                    Else
                                        returnString += "<br /><span class=""emphasis_text"">" & TemporaryTable.Rows(0).Item("ac_asking").ToString & "</span>"
                                    End If
                                    returnString += ShowDom(TemporaryTable.Rows(0).Item("ac_list_date"))
                                    returnString += "</span>"
                                Else
                                    returnString += "<span>" & TemporaryTable.Rows(0).Item("ac_status") & "</span>"
                                End If



                                If TemporaryTable.Rows(0).Item("ac_forsale_flag") = "Y" Then
                                    returnString += TemporaryTable.Rows(0).Item("ac_status")
                                    seperator += Constants.cCommaDelim
                                    If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_asking")) Then
                                        If TemporaryTable.Rows(0).Item("ac_asking") <> "" Then
                                            If Trim(TemporaryTable.Rows(0).Item("ac_asking")) = "Price" Then
                                                If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_asking_price")) Then
                                                    seperator = Constants.cCommaDelim
                                                    returnString += seperator + " <span class=""label"">Asking:</span> " & FormatCurrency((CDbl(TemporaryTable.Rows(0).Item("ac_asking_price").ToString) / 1000), 0) & "k"
                                                End If
                                            Else
                                                seperator = Constants.cCommaDelim
                                                returnString += seperator + " " & TemporaryTable.Rows(0).Item("ac_asking")
                                            End If
                                        End If
                                    End If
                                End If
                                returnString += "</td>"
                            End If

                            returnString += "<td align=""left"" valign=""top"">"

                            returnString += crmWebClient.CompanyFunctions.FindEvolutionACCompanies(masterPage.aclsData_Temp, TemporaryTable.Rows(0).Item("CLIENT_ID"), True, True)

                            returnString += "</td>"
                            returnString += "<td align=""left"" valign=""top"">"
                            If Not IsNumeric(jetnetAFTT) Then
                                jetnetAFTT = 0
                            End If
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_airframe_total_hours")) Then
                                If TemporaryTable.Rows(0).Item("ac_airframe_total_hours") <> 0 And jetnetAFTT <> TemporaryTable.Rows(0).Item("ac_airframe_total_hours") Then
                                    returnString += TemporaryTable.Rows(0).Item("ac_airframe_total_hours").ToString
                                End If
                            End If
                            returnString += "<br />"
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_1_tot_hrs")) Then
                                returnString += TemporaryTable.Rows(0).Item("ac_engine_1_tot_hrs").ToString
                            End If
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_2_tot_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_2_tot_hrs").ToString
                            End If
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_3_tot_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_3_tot_hrs").ToString
                            End If
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_4_tot_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_4_tot_hrs").ToString
                            End If
                            returnString += "<br />"
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_1_soh_hrs")) Then
                                returnString += "" & TemporaryTable.Rows(0).Item("ac_engine_1_soh_hrs").ToString
                            End If

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_2_soh_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_2_soh_hrs").ToString
                            End If

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_3_soh_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_3_soh_hrs").ToString
                            End If

                            If Not IsDBNull(TemporaryTable.Rows(0).Item("ac_engine_4_soh_hrs")) Then
                                returnString += " / " & TemporaryTable.Rows(0).Item("ac_engine_4_soh_hrs").ToString
                            End If
                            returnString += "<br />"

                            returnString += "</td>"

                            returnString += "<td align=""left"" valign=""top""></td>"
                            returnString += "<td align=""left"" valign=""top""></td>"

                            'returnString = "<span class=""label mediumText"">CLIENT RECORD:</span> S/N " & crmWebClient.DisplayFunctions.WriteDetailsLink(TemporaryTable.Rows(0).Item("cliaircraft_id"), 0, 0, 0, True, TemporaryTable.Rows(0).Item("cliaircraft_ser_nbr").ToString, "text_underline", "&SOURCE=CLIENT")
                            'If TemporaryTable.Rows(0).Item("cliaircraft_forsale_flag") = "Y" Then
                            '  'Select Case TemporaryTable.Rows(0).Item("cliaircraft_status")
                            '  '  Case "For Sale"
                            '  '    returnString = "<strong>FOR SALE</strong>"
                            '  '    seperator = Constants.cCommaDelim
                            '  '  Case Else
                            '  returnString += "<br />" & TemporaryTable.Rows(0).Item("cliaircraft_status")
                            '  seperator += Constants.cCommaDelim
                            '  ' End Select


                            '  If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")) Then
                            '    If TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage") <> "" Then
                            '      If Trim(TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")) = "Price" Then
                            '        If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_asking_price")) Then
                            '          seperator = Constants.cCommaDelim
                            '          returnString += seperator + " <span class=""label"">Asking:</span> " & FormatCurrency((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_asking_price").ToString) / 1000), 0) & "k"
                            '        End If
                            '      Else
                            '        seperator = Constants.cCommaDelim
                            '        returnString += seperator + " " & TemporaryTable.Rows(0).Item("cliaircraft_asking_wordage")
                            '      End If
                            '    End If
                            '  End If

                            '  If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_est_price")) Then
                            '    If TemporaryTable.Rows(0).Item("cliaircraft_est_price") <> 0 Then
                            '      returnString += seperator + " <span class=""label"">Take Price:</span> " & FormatCurrency((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_est_price").ToString) / 1000), 0) & "k"
                            '    End If
                            '  End If
                            'End If


                            'If Not IsNumeric(jetnetAFTT) Then
                            '  jetnetAFTT = 0
                            'End If

                            'If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_value_description")) Then
                            '  If TemporaryTable.Rows(0).Item("cliaircraft_value_description") <> "" Then
                            '    returnString += "<br />" & TemporaryTable.Rows(0).Item("cliaircraft_value_description")
                            '  End If
                            'End If
                            'If Not IsNumeric(jetnetAFTT) Then
                            '  jetnetAFTT = 0
                            'End If
                            'If Not IsDBNull(TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours")) Then
                            '  If TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours") <> 0 And jetnetAFTT <> TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours") Then
                            '    returnString += seperator + "<br /><span class=""label"">AFTT:</span> " & FormatNumber((CDbl(TemporaryTable.Rows(0).Item("cliaircraft_airframe_total_hours").ToString)), 0)
                            '  End If
                            'End If

                            returnString += "</tr>"

                            'If returnString <> "" Then
                            '  returnString = "<div class=""CLIENTCRMRow"">" & returnString & "</div>"
                            'End If
                        End If
                    End If
                End If
            End If
        End If
        Return returnString
    End Function

    Public Function ShowAttributes() As Boolean

        Dim returnBool As Boolean = False

        Try

            'This goes through and finds all the advanced search items and clears it.
            Dim I As Integer = 0
            Dim L As Integer = Session.Contents.Count

            For I = L - 1 To 0 Step -1
                If Not IsNothing(Session.Contents.Item(I)) Then
                    If TypeOf (Session.Contents.Item(I)) Is String Then
                        If InStr(Session.Contents.Keys(I).ToString(), "Advanced-Attribute") > 0 Then

                            returnBool = True
                        End If
                    End If
                End If
            Next
            If Trim(Request("att")) = "true" Then
                returnBool = True
            End If
            If Page.Request.Form("project_search") = "Y" Then
                For Each name As String In Request.Form.AllKeys
                    Dim value As String = Request.Form(name)
                    If InStr(name, "Attribute_") > 0 Then
                        returnBool = True
                    End If
                Next
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try


        Return returnBool

    End Function

    Private Sub Aircraft_Listing_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try

            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else

                If Not IsNothing(Session.Item("isMobile")) Then
                    If Session.Item("isMobile") Then
                        Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
                        masterPage = DirectCast(Page.Master, MobileTheme)
                    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
                        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
                    Else
                        Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
                        masterPage = DirectCast(Page.Master, EvoTheme)
                    End If
                ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                    Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
                    masterPage = DirectCast(Page.Master, CustomerAdminTheme)
                Else
                    Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
                    masterPage = DirectCast(Page.Master, EvoTheme)
                End If



                If Not IsNothing(Request.Item("h")) Then
                    If Not String.IsNullOrEmpty(Request.Item("h").ToString) Then
                        History = True
                        ErrorReportingTypeString = "History"

                    End If
                End If
                If Not IsNothing(Request.Item("e")) Then
                    If Not String.IsNullOrEmpty(Request.Item("e").ToString) Then
                        MarketEvent = True
                        ErrorReportingTypeString = "Events"

                    End If
                End If

                If Not IsNothing(Request.Item("restart")) Then
                    If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
                        If Request.Item("restart") = "1" Then
                            Reset_Form()
                        End If
                    End If
                End If


                productCodeCount = DisplayFunctions.ReturnProductCodeCount(productCodeCount)


                ''setting default button so enter submits form
                If MarketEvent Then
                    masterPage.SetDefaultButtion(Me.events_search.UniqueID)
                ElseIf History Then
                    masterPage.SetDefaultButtion(Me.transaction_search.UniqueID)
                ElseIf History = False And MarketEvent = False Then
                    masterPage.SetDefaultButtion(Me.acsearch.UniqueID)
                End If

                If Page.Request.Form("complete_search") = "Y" Then
                    ClearSavedSelection()
                    ac_lifecycle_stage.SelectedValue = ""
                ElseIf Page.Request.Form("project_search") = "Y" Then
                    If IsNumeric(Page.Request.Form("project_id")) Then
                        If Page.Request.Form("project_id") <> 0 Then
                            ClearSavedSelection()
                            ac_lifecycle_stage.SelectedValue = ""
                        Else
                            ac_lifecycle_stage.SelectedValue = ""
                            If Page.Request.Form("clearSelection") = "true" Then
                                ClearSavedSelection()
                            End If

                            If Page.Request.Form("fromMarketSummary") = "true" Then
                                TableCell2.Visible = True
                            End If

                        End If
                    End If
                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub BuildActionsDropdownMenu()

        Try

            actions_submenu_dropdown.Items.Clear()
            actions_submenu_dropdown.Items.Add(New ListItem("Save As - New Folder", "javascript:SubMenuDropAircraft(3,0, false);"))

            If (MarketEvent And (String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventType").ToString.Trim) Or HttpContext.Current.Session.Item("eventType").ToString.ToUpper.Contains("AIRCRAFT"))) Or MarketEvent = False Then
                actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDropAircraft(1,0, false);"))
            End If

            actions_submenu_dropdown.Items.Add(New ListItem("JETNET Export/Report", "javascript:SubMenuDropAircraft(5,0, false);"))
            actions_submenu_dropdown.Items.Add(New ListItem("Map Aircraft", "javascript:SubMenuDropAircraft(4,0, false);"))

            If MarketEvent = False Then
                actions_submenu_dropdown.Items.Add(New ListItem("Summary", "javascript:SubMenuDropAircraft(2,0, false);"))
            ElseIf MarketEvent = True Then
                If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                    If Page.Request.Form("complete_search") = "Y" Or Page.Request.Form("project_search") = "Y" Then
                    Else
                        actions_submenu_dropdown.Items.Add(New ListItem("Schedule Event Alert", "javascript:SubMenuDropAircraft(3, 0, true);"))
                    End If
                End If
            End If



            If MarketEvent = False And History = False Then
                'actions_submenu_dropdown.Items.Add(New ListItem("Portfolio Summary", "javascript:setPortfolioView('1');"))
                fleetAnalyzerContainer.Text = "<img src=""images/fleetAnalyzer.png"" alt=""Fleet Analyzer"" class=""cursor"" onclick=""setPortfolioView('1');""/>"
            End If
        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub Aircraft_Listing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim RelationshipTable As New DataTable
        Dim MaintenanceTable As New DataTable
        Dim LookupDataset As New DataSet

        Try

            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else

                Dim DateRangePicker As StringBuilder = New StringBuilder()
                Dim PressEnterandSubmit As StringBuilder = New StringBuilder()
                Dim SwapPageScript As StringBuilder = New StringBuilder()
                Dim CheckUncheckExcludeInternal As StringBuilder = New StringBuilder()
                'javascript added for history daterange picker

                Dim maintenanceItemValueDate As StringBuilder = New StringBuilder()

                'Caching fields if needed.
                masterPage.aclsdata_temp.FillCacheLookups()

                'Initializing Cache Dataset
                If Not IsNothing(Cache("CacheLookups")) Then
                    LookupDataset = Cache("CacheLookups")
                End If


                If Not Page.ClientScript.IsClientScriptBlockRegistered("MaintenanceItemValueDate") Then
                    If Session.Item("isMobile") = False Then
                        maintenanceItemValueDate.Append("<script type=""text/javascript"">")
                        maintenanceItemValueDate.Append("$(function(){")
                        maintenanceItemValueDate.Append("$('#" & acmaint_value.ClientID & "').daterangepicker();")
                        maintenanceItemValueDate.Append("$('#" & acmaint_value1.ClientID & "').daterangepicker();")
                        maintenanceItemValueDate.Append("});")
                        maintenanceItemValueDate.Append("</script>")

                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MaintenanceItemValueDate", maintenanceItemValueDate.ToString, False)
                    End If
                End If

                If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPostFolder") Then
                    If MarketEvent = False Then
                        Dim masonryStr As String = "function loadMasonry() { " & vbNewLine

                        masonryStr += " var grid = document.querySelector('.grid');" & vbNewLine
                        masonryStr += " var msnry = new Masonry(grid, {" & vbNewLine
                        masonryStr += " itemSelector: '.grid-item'," & vbNewLine
                        masonryStr += " columnWidth: '.grid-item'," & vbNewLine
                        masonryStr += " gutter: 10," & vbNewLine
                        masonryStr += " horizontalOrder: true," & vbNewLine
                        masonryStr += " percentPosition: true" & vbNewLine
                        masonryStr += " });" & vbNewLine


                        masonryStr += "setTimeout(function(){msnry.layout();},1100);"

                        masonryStr += " };" & vbNewLine



                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPostFolder", masonryStr, True)
                    End If
                End If

                If History Then

                    If Not Page.ClientScript.IsClientScriptBlockRegistered("DateRangePicker") Then

                        DateRangePicker.Append("<script type=""text/javascript"">")
                        DateRangePicker.Append("$(function(){")
                        DateRangePicker.Append("$('#" & journ_date.ClientID & "').daterangepicker();")
                        DateRangePicker.Append("});")
                        DateRangePicker.Append("</script>")
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DateRangePicker", DateRangePicker.ToString, False)

                    End If

                    'This script runs on the history side.
                    'It ties to the used ac sales/ new ac sales checkbox.
                    'If they're clicked, then it goes and checks the exclude internal transactions checkbox as well.
                    'Nothing happens (yet) if they're unchecked. Meaning it doesn't uncheck the exclude internal transactions checkbox.
                    'However - changing the exclude internal transaction checks to see if NEW or USED sales is checked. If they are, it makes sure the exclude internal transaction remains checks

                    If Not Page.ClientScript.IsClientScriptBlockRegistered("CheckUncheckExcludeInternal") Then
                        CheckUncheckExcludeInternal.Append("$(""#" & journ_newac_flag.ClientID & """).change(function() {")
                        CheckUncheckExcludeInternal.Append("if(this.checked) {")
                        CheckUncheckExcludeInternal.Append("$(""#" & journ_exclude_internal_transactions.ClientID & """).prop('checked', true);")
                        CheckUncheckExcludeInternal.Append("}")
                        CheckUncheckExcludeInternal.Append("});")

                        CheckUncheckExcludeInternal.Append("$(""#" & jcat_used_retail_sales_flag.ClientID & """).change(function() {")
                        CheckUncheckExcludeInternal.Append("if(this.checked) {")
                        CheckUncheckExcludeInternal.Append("$(""#" & journ_exclude_internal_transactions.ClientID & """).prop('checked', true);")
                        CheckUncheckExcludeInternal.Append("}")
                        CheckUncheckExcludeInternal.Append("});")

                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CheckUncheckExcludeInternal", CheckUncheckExcludeInternal.ToString, True)

                    End If
                End If


                If Not Page.ClientScript.IsStartupScriptRegistered("ToggleClickScript") Then
                    Dim ToggleClick As New StringBuilder

                    If Session.Item("isMobile") = True Then
                        ToggleClick.Append("$(function(){")
                        ToggleClick.Append(" $('#" & controlLink.ClientID & "').click(function() {")
                        ToggleClick.Append("if ($(""#" & Collapse_Panel.ClientID & """).is("":hidden"")) {")
                        ToggleClick.Append("$('#" & controlLink.ClientID & "').attr('src', '../images/search_collapse.jpg');")
                        ToggleClick.Append("} else {")
                        ToggleClick.Append("$('#" & controlLink.ClientID & "').attr('src', '../images/search_expand.jpg');")
                        ToggleClick.Append("}")
                        ToggleClick.Append("$(""#" & Collapse_Panel.ClientID & """).slideToggle();")
                        ToggleClick.Append("});")
                        ToggleClick.Append("});")
                        acsearch.OnClientClick = "javascript:FillStateHiddenValue(1);ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
                    Else
                        ToggleClick.Append("$(function(){")
                        ToggleClick.Append(" $('#" & ControlImage.ClientID & "').click(function() {")
                        ToggleClick.Append("if ($(""#" & Collapse_Panel.ClientID & """).is("":hidden"")) {")
                        ToggleClick.Append("$('#" & ControlImage.ClientID & "').attr('src', '../images/search_collapse.jpg');")
                        ToggleClick.Append("} else {")
                        ToggleClick.Append("$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');")
                        ToggleClick.Append("}")
                        ToggleClick.Append("$(""#" & Collapse_Panel.ClientID & """).slideToggle();")
                        ToggleClick.Append("});")
                        ToggleClick.Append("});")
                    End If
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleClickScript", ToggleClick.ToString, True)
                End If


                'Setting up Chosen Select Dropdown:
                If Session.Item("isMobile") = True Then
                    serText.Width = Unit.Pixel(45)

                    ac_reg_no.Width = Unit.Percentage(98)
                    ac_reg_no.CssClass = "float_left"
                    ac_ser_no_from.Width = Unit.Percentage(98)
                    'ac_ser_no_to.Width = Unit.Percentage(47.5)
                    aerodex_toggle.Visible = False
                    Dim dropdownString As New StringBuilder
                    dropdownString.Append("function swapChosenDropdowns() {")
                    dropdownString.Append("$("".chosen-select"").chosen(""destroy"");")
                    dropdownString.Append("$("".chosen-select"").chosen({ no_results_text: ""No results found."", disable_search_threshold: 10 });")
                    dropdownString.Append("}")
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("chosenDropdowns") Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chosenDropdowns", dropdownString.ToString, True)
                    End If

                    dropdownString = New StringBuilder
                    If Not Page.IsPostBack Then
                        dropdownString.Append(";swapChosenDropdowns();")
                    End If

                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateDropdown", dropdownString.ToString, True)
                End If

                'This javascript uses jquery to toggle on/off classes
                'and on and off input items based on what type of search the user has swapped to.

                If Not Page.ClientScript.IsClientScriptBlockRegistered("PressEnterSubmit") Then

                    PressEnterandSubmit.Append("<script type=""text/javascript"">")
                    '//Automatically submit on enter press
                    PressEnterandSubmit.Append("$(function(){")
                    PressEnterandSubmit.Append("$('textarea').on('keyup', function(e){")
                    PressEnterandSubmit.Append("if (e.keyCode == 13) {")

                    If MarketEvent Then
                        PressEnterandSubmit.Append("$(""#" & events_search.ClientID & """).click();")
                    ElseIf History Then
                        PressEnterandSubmit.Append("$(""#" & transaction_search.ClientID & """).click();")
                    Else
                        PressEnterandSubmit.Append("$(""#" & acsearch.ClientID & """).click();")
                    End If

                    PressEnterandSubmit.Append("}")
                    PressEnterandSubmit.Append("});")
                    PressEnterandSubmit.Append("});")
                    PressEnterandSubmit.Append("</script>")
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PressEnterSubmit", PressEnterandSubmit.ToString, False)

                End If

                If Not Page.ClientScript.IsClientScriptBlockRegistered("SwapPageDependingOnEventType") Then
                    SwapPageScript.Append("<script type=""text/javascript"">")

                    SwapPageScript.Append(vbCrLf & "function SwapPageDependingOnEventType(SwapPageType) {")
                    SwapPageScript.Append(vbCrLf & "switch(SwapPageType) {")
                    SwapPageScript.Append(vbCrLf & "case ""AIRCRAFT"":")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").prop(""disabled"", false);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").removeClass( ""display_disable"" )")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & """).removeClass(""display_disable"");")

                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").prop(""disabled"", false);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").removeClass( ""display_disable"" )")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & """).removeClass(""display_disable"");")
                    SwapPageScript.Append(vbCrLf & "break;")
                    SwapPageScript.Append(vbCrLf & "case ""WANTED"":")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").prop(""disabled"", false);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").removeClass( ""display_disable"" )")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & """).removeClass(""display_disable"");")

                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").prop(""disabled"", true);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").prop(""class"", ""display_disable"")")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & """).prop(""class"", ""display_disable"");")
                    SwapPageScript.Append(vbCrLf & "break;")
                    SwapPageScript.Append(vbCrLf & "case ""COMPANY"":")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").prop(""disabled"", true);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & " :input"").prop(""class"", ""display_disable"")")
                    SwapPageScript.Append(vbCrLf & "$(""#" & model_search_box.ClientID & """).prop(""class"", ""display_disable"");")

                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").prop(""disabled"", true);")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & " :input"").prop(""class"", ""display_disable"")")
                    SwapPageScript.Append(vbCrLf & "$(""#" & tableCellToggle.ClientID & """).prop(""class"", ""display_disable"");")
                    SwapPageScript.Append(vbCrLf & "break;")
                    SwapPageScript.Append(vbCrLf & "}")
                    SwapPageScript.Append(vbCrLf & "}")
                    SwapPageScript.Append("</script>")
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SwapPageDependingOnEventType", SwapPageScript.ToString, False)

                End If

                If Not Page.IsPostBack Then
                    BuildActionsDropdownMenu()

                    'This needs to be put in and loaded for now. Hopefully whenever the session variables are the same, this can go away.
                    If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                        Response.Write("error in load preferences : ")
                    End If

                    'The above tables are to be removed as soon as the newer tab is working and this will be moved out of here.
                    RelationshipTable = masterPage.aclsData_Temp.Get_Client_Aircraft_Contact_Type(History)
                    clsGeneral.clsGeneral.Populate_Listbox(RelationshipTable, cref_contact_type, "cliact_name", "cliact_type", True)

                    cref_contact_type.Items.RemoveAt(0)
                    cref_contact_type.Items.Insert(0, New ListItem("All", ""))
                    cref_contact_type.Items.Insert(1, New ListItem("All Owners", "'00','97','17','08','16'"))
                    cref_contact_type.Items.Insert(2, New ListItem("All Operating Companies", "'Y','O'"))

                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                        cref_contact_type.Items.Insert(3, New ListItem("All Dealers, Brokers, Reps", "'93','98','99','38','2X'"))
                        cref_contact_type.Items.Insert(4, New ListItem("All Owners as Individuals", "'I'"))
                    Else
                        cref_contact_type.Items.Insert(3, New ListItem("All Owners as Individuals", "'I'"))
                    End If

                    If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then
                        advanced_search_categories_table.Visible = True
                        Custom_MPM.Visible = True
                        ToggleCustomFields()
                    End If

                    cref_contact_type.SelectedValue = ""

                    If Not IsNothing(LookupDataset.Tables(0)) Then
                        clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(0), cref_business_type, "cbus_name", "cbus_type", False)
                    End If
                    If Not IsNothing(LookupDataset.Tables(1)) Then
                        clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(1), contact_title, "ctitleg_group_name", "ctitleg_group_name", False)
                    End If

                    Dim CompanyFolderTable As New DataTable
                    CompanyFolderTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 1, Nothing, "")
                    If Not IsNothing(CompanyFolderTable) Then
                        clsGeneral.clsGeneral.Populate_Dropdown(CompanyFolderTable, cref_comp_id, "cfolder_name", "cfolder_id", False)
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'US AC MAINTAINED/FOREIGN MAINTAINED
                    'One Query for both US and foreign.
                    'I really didn't want to run this query twice whenever once would be sufficient.
                    If Not IsNothing(LookupDataset.Tables(9)) Then
                        MaintenanceTable = LookupDataset.Tables(9)  ' done - msw - 5/5/19
                    End If

                    us_ac_maintained.Items.Clear()
                    foreign_ac_maintained.Items.Clear()
                    us_ac_maintained.Items.Add(New ListItem("", ""))
                    foreign_ac_maintained.Items.Add(New ListItem("", ""))
                    us_ac_maintained.Items.Add(New ListItem("Blank/Unknown", "IS NULL"))
                    us_ac_maintained.Items.Add(New ListItem("Blank", "BLANK"))

                    If Not IsNothing(MaintenanceTable) Then
                        If MaintenanceTable.Rows.Count > 0 Then
                            For Each r As DataRow In MaintenanceTable.Rows
                                If Not IsDBNull(r("certification_name")) Then
                                    If Not IsDBNull(r("certification_usa_flag")) Then
                                        If r("certification_usa_flag").ToString = "U" Then
                                            us_ac_maintained.Items.Add(New ListItem(CStr(r("certification_name")), "'" & CStr(r("certification_name") & "'")))
                                        ElseIf r("certification_usa_flag").ToString = "I" Then
                                            foreign_ac_maintained.Items.Add(New ListItem(CStr(r("certification_name")), "'" & CStr(r("certification_name") & "'")))
                                        ElseIf r("certification_usa_flag").ToString = "B" Then
                                            foreign_ac_maintained.Items.Add(New ListItem(CStr(r("certification_name")), "'" & CStr(r("certification_name") & "'")))
                                            us_ac_maintained.Items.Add(New ListItem(CStr(r("certification_name")), "'" & CStr(r("certification_name") & "'")))
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If

                    us_ac_maintained.SelectedValue = ""
                    foreign_ac_maintained.SelectedValue = ""

                    'Filling up the Fractional Program listbox.
                    If Not IsNothing(LookupDataset.Tables(3)) Then
                        clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(3), lbFractionalProgram, "prog_name", "prog_id", False)
                    End If

                    'Filling up the Maintenance Item listbox.
                    If Not IsNothing(LookupDataset.Tables(8)) Then
                        clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(8), maintenance_item, "mitem_name", "mitem_name", True)
                    End If

                    'Filling up the Maintenance Item listbox.
                    If Not IsNothing(LookupDataset.Tables(8)) Then
                        clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(8), maintenance_item1, "mitem_name", "mitem_name", True)
                    End If

                    MaintenanceTable = New DataTable
                    If Not IsNothing(LookupDataset.Tables(4)) Then
                        MaintenanceTable = LookupDataset.Tables(4)
                        FillDropdownsMaintenance(emp_provider_name, MaintenanceTable, "emp_provider_name")
                        FillDropdownsMaintenance(emp_program_name, MaintenanceTable, "emp_program_name")
                    End If

                    Dim HTMLstr As String = "var filledArray;" & vbNewLine
                    FillHTML(HTMLstr, MaintenanceTable, "emp_provider_name", "emp_program_name")
                    emp_provider_name.Attributes("onChange") = "javascript:" & HTMLstr & ";FilterDropDownBasedOnValue(this.value, filledArray, '" & emp_program_name.ClientID & "');return false;"


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not IsNothing(LookupDataset.Tables(5)) Then
                        MaintenanceTable = New DataTable
                        MaintenanceTable = LookupDataset.Tables(5)
                        FillDropdownsMaintenance(emgp_provider_name, MaintenanceTable, "emgp_provider_name")
                        FillDropdownsMaintenance(emgp_program_name, MaintenanceTable, "emgp_program_name")
                    End If

                    HTMLstr = "var filledArray;" & vbNewLine
                    FillHTML(HTMLstr, MaintenanceTable, "emgp_provider_name", "emgp_program_name")
                    emgp_provider_name.Attributes("onChange") = "javascript:" & HTMLstr & ";FilterDropDownBasedOnValue(this.value, filledArray, '" & emgp_program_name.ClientID & "');return false;"

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not IsNothing(LookupDataset.Tables(6)) Then
                        MaintenanceTable = New DataTable
                        MaintenanceTable = LookupDataset.Tables(6)
                        FillDropdownsMaintenance(amp_provider_name, MaintenanceTable, "amp_provider_name")
                        FillDropdownsMaintenance(amp_program_name, MaintenanceTable, "amp_program_name")
                    End If

                    HTMLstr = "var filledArray;" & vbNewLine
                    FillHTML(HTMLstr, MaintenanceTable, "amp_provider_name", "amp_program_name")
                    amp_provider_name.Attributes("onChange") = "javascript:" & HTMLstr & ";FilterDropDownBasedOnValue(this.value, filledArray, '" & amp_program_name.ClientID & "');return false;"


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not IsNothing(LookupDataset.Tables(7)) Then
                        MaintenanceTable = New DataTable
                        MaintenanceTable = LookupDataset.Tables(7)
                        FillDropdownsMaintenance(amtp_provider_name, MaintenanceTable, "amtp_provider_name")
                        FillDropdownsMaintenance(amtp_program_name, MaintenanceTable, "amtp_program_name")
                    End If

                    HTMLstr = "var filledArray;" & vbNewLine
                    FillHTML(HTMLstr, MaintenanceTable, "amtp_provider_name", "amtp_program_name")
                    amtp_provider_name.Attributes("onChange") = "javascript:" & HTMLstr & ";FilterDropDownBasedOnValue(this.value, filledArray, '" & amtp_program_name.ClientID & "');return false;"


                    MaintenanceTable = Nothing


                    If Aircraft_Criteria.Visible = True Then
                        'Fill Transaction Type/Transaction only dropdowns
                        If History = True Then
                            ac_lifecycle_stage.SelectedValue = "" 'Clearing selected lifecycle for history.
                            DisplayFunctions.Fill_Dropdown("Date", journ_date_operator, "")
                            journ_date.ToolTip = DisplayFunctions.DisplayFormatRules("Date")
                            transaction_retail_CheckedChanged(transaction_retail, EventArgs.Empty)
                        End If


                        'fill events only listboxes 
                        If MarketEvent = True Then
                            Aircraft_Criteria.CssClass = "fixPosition eventsOnly"
                            ac_lifecycle_stage.SelectedValue = "" 'Clearing selected lifecycle for market.
                            If actions_submenu_dropdown.Items.Count > 3 Then
                                actions_submenu_dropdown.Items.RemoveAt(3)
                            End If
                        End If

                    End If
                End If

                'This will go ahead and set up the javascript control array. Not needed unless you're going to need the array (such as to find an amod ID index) before the search button is clicked
                'Generally you won't, but on the ac listing page, you use folders and the home page market tab

                'This basically loads the array into session.
                commonEvo.fillAirframeArray("")
                commonEvo.fillAircraftTypeLableArray("")
                commonEvo.fillDefaultAirframeArray("")

                commonEvo.fillMfrNamesArray("")
                commonEvo.fillAircraftSizeArray("")

                If History = False And MarketEvent = False Then

                    If Session.Item("localSubscription").crmCloudNotes_Flag Or Session.Item("localSubscription").crmServerSideNotes_Flag Then
                        aircraftShowTable.Visible = True
                        Dim SwapPageScriptNotes As StringBuilder = New StringBuilder()
                        If Not Page.ClientScript.IsClientScriptBlockRegistered("Toggle") Then
                            SwapPageScriptNotes.Append("<script type=""text/javascript"">")

                            SwapPageScriptNotes.Append(vbCrLf & "function toggleNotesDateToggle(aircraftSearchDropdown) {")
                            SwapPageScriptNotes.Append(vbCrLf & " if (aircraftSearchDropdown.value != 0) { ")
                            SwapPageScriptNotes.Append(vbCrLf & "$(""#" & aircraftNotesDateToggle.ClientID & """).removeClass(""display_none"");")
                            SwapPageScriptNotes.Append(vbCrLf & "$(""#" & placerHold.ClientID & """).prop(""class"", ""display_none"");")
                            SwapPageScriptNotes.Append(vbCrLf & "} else {")
                            SwapPageScriptNotes.Append(vbCrLf & "$(""#" & aircraftNotesDateToggle.ClientID & """).prop(""class"", ""display_none"");")
                            SwapPageScriptNotes.Append(vbCrLf & "$(""#" & placerHold.ClientID & """).removeClass(""display_none"");")
                            SwapPageScriptNotes.Append(vbCrLf & "}")
                            SwapPageScriptNotes.Append(vbCrLf & "}")
                            SwapPageScriptNotes.Append("</script>")
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Toggle", SwapPageScriptNotes.ToString, False)

                        End If


                    End If

                    If clsGeneral.clsGeneral.isEValuesAvailable() Then
                        Dim ToggleCookie As HttpCookie = Request.Cookies("evalues")

                        If Not IsNothing(ToggleCookie) Then
                            If ToggleCookie.Value = "true" Then
                                displayEvalues = True
                            End If
                        Else
                            displayEvalues = False
                        End If
                    End If

                    If ShowAttributes() Then
                        attrBoolRan.Text = "true"
                    End If

                    Dim TabSwap As String = ""
                    ac_advanced_search.OnClientActiveTabChanged = "TabBottomSwap"
                    TabSwap = "function TabBottomSwap(sender, args) { if (sender.get_activeTabIndex() == 11) { if ($(""#" & attrBoolRan.ClientID & """).val() == '') { ChangeTheMouseCursorOnItemParentDocument('cursor_wait'); $(""#" & TestLoadAttributes.ClientID & """).click(); } }} "
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("TabSwap") Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "TabSwap", TabSwap.ToString, True)
                    End If

                End If

                'This will build all the controls.
                Build_Advanced_Search()

                If Not Page.IsPostBack Then

                    If Aircraft_Criteria.Visible Then

                        'This is going to fill up all the boxes if there's search criteria.
                        'This comes from the home page tab.
                        'it's going to send type/make/model with the actual IDs 
                        'So you'll need to find the respectable index for them.
                        If Page.Request.Form("complete_search") = "Y" Then
                            If Page.Request.Form("type_code") <> "" Then
                                Dim tempArray As Array = Split(Page.Request.Form("type_code"), "|")
                                If UBound(tempArray) > 0 Then
                                    HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(tempArray(0)), crmWebClient.Constants.AIRFRAME_TYPE, tempArray(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
                                Else
                                    HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(Page.Request.Form("type_code"), crmWebClient.Constants.AIRFRAME_TYPE)
                                End If
                                If Page.Request.Form("make") <> "" Then
                                    HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(Page.Request.Form("make")), crmWebClient.Constants.AIRFRAME_MAKE, tempArray(1), crmWebClient.Constants.AIRFRAME_FRAME)
                                    If Page.Request.Form("model") <> "" Then
                                        HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(Page.Request.Form("model")))
                                    End If
                                End If
                            End If

                            If Page.Request.Form("in_operation") = "Y" Then
                                ac_lifecycle_stage.SelectedValue = 3
                            End If
                            If Page.Request.Form("for_sale") = "Y" Then
                                market.SelectedValue = "For Sale"
                            End If
                            If Page.Request.Form("exclusive") = "Y" Then
                                market.SelectedValue = "For Sale on Exclusive"
                            End If
                            If IsNumeric(Page.Request.Form("lifecycle")) Then
                                ac_lifecycle_stage.SelectedValue = Page.Request.Form("lifecycle")
                            End If

                        End If


                        '''''Working yet needs to be rethought. Since
                        'This is just for a prototype to see whether we'll 
                        'use this or not, it will suffice for now
                        'However should be rethought if we go ahead and use this
                        'Most notably up above, where the different 'complete search' is used.
                        'Should definitely combine the two.
                        'And create a reasonable approach.  
                        'What I'm thinking is naming all the request variables after the html control
                        'This  way we can get rid of the stuff up top and deal with it in just one place.
                        'Another thing would be figuring out what the type of the control is. I don't
                        'Really like this way, even though I found it on 4guysfromrolla.com. It
                        'seems like there should be a better way.
                        'One more note. On the advanced search creation function, 
                        'If we go through with this, it would be best to remove the used keys from the request form, so 
                        'that way we don't waste time looking for them down below.
                        'Or we could remove the request form element and just pass a simple project ID, get everything from the database
                        'And loop it much in the same way. It really depends on which way we decide is the best way to deal with this.
                        'There may be limitations in passing request fields as well as security issues to talk about.
                        If Page.Request.Form("project_search") = "Y" Then
                            Dim folderID As Long = 0
                            Dim FoldersTableData As New DataTable
                            Dim cfolderData As String = ""
                            Dim FolderSource As String = "JETNET"
                            Dim FolderName As String = ""

                            FolderInformation.Text = ""
                            FolderInformation.Visible = False
                            folderID = Page.Request.Form("project_id")

                            If Not String.IsNullOrEmpty(Page.Request.Form("cfolder_source")) Then
                                FolderSource = Page.Request.Form("cfolder_source")
                            End If

                            If folderID <> 0 Then

                                If FolderSource = "JETNET" Then
                                    FoldersTableData = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
                                    If Not IsNothing(FoldersTableData) Then
                                        If FoldersTableData.Rows.Count > 0 Then
                                            cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString
                                            FolderName = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                                            If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                                                comp_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                                                static_folder.Text = "true"
                                            End If
                                        End If
                                    End If
                                Else
                                    cfolderData = Page.Request.Form("cfolder_data")
                                    If cfolderData <> "" Then
                                        Dim UserTableCheck As DataTable
                                        UserTableCheck = masterPage.aclsData_temp.Get_Client_User_By_Email_Address(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress)
                                        If Not IsNothing(UserTableCheck) Then
                                            If UserTableCheck.Rows.Count > 0 Then
                                                FoldersTableData = masterPage.aclsdata_temp.Get_Client_Folders_ByID(UserTableCheck.Rows(0).Item("cliuser_id"), folderID)
                                                If Not IsNothing(FoldersTableData) Then
                                                    If FoldersTableData.Rows.Count > 0 Then
                                                        FolderName = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                                                        If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                                                            comp_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                                                            static_folder.Text = "true"
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If

                                End If
                                If cfolderData <> "" Then

                                    If MarketEvent = False And History = False Then
                                        actions_submenu_dropdown.Items.Add(New ListItem("Portfolio View", "javascript:setPortfolioView('" & folderID.ToString & "');"))
                                        ' only show if you arent aerodex or u are areodex elite 
                                        If HttpContext.Current.Session.Item("localPreferences").AerodexElite = True Or HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                                            actions_submenu_dropdown.Items.Add(New ListItem("Flight Activity View", "javascript:setFlightActivityView('" & folderID.ToString & "', '" & FolderName & "');"))
                                        End If
                                    End If

                                    'Fills up the applicable folder Information pulled from the cfolder data field
                                    DisplayFunctions.FillUpFolderInformation(Table2, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, History, MarketEvent, False, False, Collapse_Panel, actions_submenu_dropdown, company_contact, StaticFolderNewSearchLabel, Control_Panel, StaticAIRCRAFTIDs, False, False, False, FolderSource)
                                    'This event only runs for events page listing.
                                    'It will make sure the selection selects for the type.

                                    If History Then
                                        If transaction_retail.Checked Then
                                            'Fill the Transaction Type.
                                            transaction_retail_CheckedChanged(transaction_retail, EventArgs.Empty)
                                        End If

                                    End If

                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, equip)
                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, location)
                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, AttributesPanel)

                                    static_folder_ac_ids.Text = StaticAIRCRAFTIDs

                                    If History Then
                                        'These are two special cases. We are in the process of removing the operators on the dates and replacing them with
                                        'a calendar entry control. However for prototype - we are only changing the journal date first.
                                        'This means that all the other dates still have operators yet we've gotten rid of the journal date operator.
                                        'Because of this - the recall on older folders that use a between or a greater than on a journal date
                                        'need to be handled in the new way. This means on this case - we have to actually fix and adjust the values.
                                        If journ_date_operator.SelectedValue = "Greater Than" Then
                                            'This needs to translate to:
                                            'currently stored in journ_date.text:now() + 15 years
                                            journ_date.Text = journ_date.Text + ":" + Format(DateAdd(DateInterval.Year, 15, Now()), "MM/dd/yyyy")
                                            journ_date_operator.SelectedValue = "Between"
                                        ElseIf journ_date_operator.SelectedValue = "Less Than" Then
                                            'This needs to translate to:
                                            '1/1/1960:currently stored in journ_date.text
                                            journ_date.Text = "1/1/1960:" + journ_date.Text
                                            journ_date_operator.SelectedValue = "Between"

                                        End If
                                    End If

                                End If

                            Else

                                'Summary Search
                                'We need to build the cData from the request object because there is technically no created folder.
                                For Each name As String In Request.Form.AllKeys 'This will loop through all the keys.

                                    If name <> "project_id" And name <> "project_search" And name <> "clearSelection" And name <> "sMarketAddToWhereClause" And Trim(name) <> "off_markets" And Trim(name) <> "on_markets" And Trim(name) <> "written_off" Then

                                        Dim value As String = Request.Form(name)

                                        If Not value.ToLower.Contains("undefined") Then

                                            If Not String.IsNullOrEmpty(cfolderData.Trim) Then
                                                cfolderData += "!~!" + name.Trim + "=" + value.Trim
                                            Else
                                                cfolderData = name.Trim + "=" + value.Trim
                                            End If

                                        End If

                                    End If

                                Next

                                '  If Not IsPostBack Then
                                '------------------------------------------------------
                                masterPage.aclsData_Temp.FillCacheLookups()

                                'Initializing Cache Dataset
                                If Not IsNothing(Cache("CacheLookups")) Then
                                    LookupDataset = Cache("CacheLookups")
                                End If
                                clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(0), journ_subcat_code_part2, "cbus_name", "cbus_type", False)
                                clsGeneral.clsGeneral.Populate_Listbox(LookupDataset.Tables(0), journ_subcat_code_part3, "cbus_name", "cbus_type", False)

                                If Not String.IsNullOrEmpty(cfolderData.Trim) Then

                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, company_contact)
                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, equip)
                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, location)
                                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, AttributesPanel)

                                End If
                            End If



                        End If
                    End If

                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub ToggleCustomFields()

        Dim aclsData_temp As New clsData_Manager_SQL
        Dim aTempTable As New DataTable
        ' Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try

            If Not IsNothing(HttpContext.Current.Session.Item("jetnetServerNotesDatabase")) Then

                Dim currentHeight As Double = advanced_search_categories_table.Height.Value

                '  aclsData_temp.client_DB = Application.Item("crmClientDatabase")
                aclsData_temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

                aTempTable = aclsData_temp.Get_Client_Preferences()
                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
                                If r("clipref_ac_custom_1_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name1.Visible = True
                                    custom_pref_text1.Visible = True
                                    custom_pref_name1.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), "")) & ":"
                                Else
                                    custom_pref_name1.Visible = False
                                    custom_pref_text1.Visible = False
                                    custom_pref_name1.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
                                If r("clipref_ac_custom_2_use") = "Y" Then
                                    custom_pref_name2.Visible = True
                                    custom_pref_text2.Visible = True
                                    custom_pref_name2.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), "")) & ":"
                                Else
                                    custom_pref_name2.Visible = False
                                    custom_pref_text2.Visible = False
                                    custom_pref_name2.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
                                If r("clipref_ac_custom_3_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name3.Visible = True
                                    custom_pref_text3.Visible = True
                                    custom_pref_name3.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), "")) & ":"
                                Else
                                    custom_pref_name3.Visible = False
                                    custom_pref_text3.Visible = False
                                    custom_pref_name3.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
                                If r("clipref_ac_custom_4_use") = "Y" Then
                                    custom_pref_name4.Visible = True
                                    custom_pref_text4.Visible = True
                                    custom_pref_name4.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), "")) & ":"
                                Else
                                    custom_pref_name4.Visible = False
                                    custom_pref_text4.Visible = False
                                    custom_pref_name4.Text = ""
                                End If
                            End If


                            If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
                                If r("clipref_ac_custom_5_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name5.Visible = True
                                    custom_pref_text5.Visible = True
                                    custom_pref_name5.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), "")) & ":"
                                Else
                                    custom_pref_name5.Visible = False
                                    custom_pref_text5.Visible = False
                                    custom_pref_name5.Text = ""
                                End If
                            End If


                            If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
                                If r("clipref_ac_custom_6_use") = "Y" Then
                                    custom_pref_name6.Visible = True
                                    custom_pref_text6.Visible = True
                                    custom_pref_name6.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), "")) & ":"
                                Else
                                    custom_pref_name6.Visible = False
                                    custom_pref_text6.Visible = False
                                    custom_pref_name6.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
                                If r("clipref_ac_custom_7_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name7.Visible = True
                                    custom_pref_text7.Visible = True
                                    custom_pref_name7.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), "")) & ":"
                                Else
                                    custom_pref_name7.Visible = False
                                    custom_pref_text7.Visible = False
                                    custom_pref_name7.Text = ""
                                End If
                            End If


                            If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
                                If r("clipref_ac_custom_8_use") = "Y" Then
                                    custom_pref_name8.Visible = True
                                    custom_pref_text8.Visible = True
                                    custom_pref_name8.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), "")) & ":"
                                Else
                                    custom_pref_name8.Visible = False
                                    custom_pref_text8.Visible = False
                                    custom_pref_name8.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
                                If r("clipref_ac_custom_9_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name9.Visible = True
                                    custom_pref_text9.Visible = True
                                    custom_pref_name9.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), "")) & ":"
                                Else
                                    custom_pref_name9.Visible = False
                                    custom_pref_text9.Visible = False
                                    custom_pref_name9.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
                                If r("clipref_ac_custom_10_use") = "Y" Then

                                    custom_pref_name10.Visible = True
                                    custom_pref_text10.Visible = True
                                    custom_pref_name10.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), "")) & ":"
                                Else
                                    custom_pref_name10.Visible = False
                                    custom_pref_text10.Visible = False
                                    custom_pref_name10.Text = ""
                                End If
                            End If

                        Next

                    Else
                        '  If aclsData_temp.class_error <> "" Then
                        '  error_string = masterpage.aclsData_Temp.class_error
                        ' masterpage.LogError("AircraftSearch.ascx.vb - ToggleCustomFields() - " & error_string)
                        'End If
                        ' masterPage.display_error()
                    End If

                    If advanced_search_categories_table.Height.Value <> currentHeight Then
                        currentHeight += 10 'buffer for custom fields header.
                        advanced_search_categories_table.Height = currentHeight
                    Else
                        advanced_search_categories_table.Visible = False 'toggle custom fields off
                    End If

                End If
            End If
        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub FillHTML(ByRef HTMLstr As String, ByRef MaintenanceTable As DataTable, ByVal fieldOne As String, ByVal FieldTwo As String)
        HTMLstr = "var filledArray;" & vbNewLine
        HTMLstr += "filledArray = [" & vbNewLine

        For Each r As DataRow In MaintenanceTable.Rows
            HTMLstr += "['" & Replace(r(fieldOne), "'", "\'").ToString & "','" & Replace(r(FieldTwo), "'", "\'").ToString & "'],"
        Next

        HTMLstr += "]" & vbNewLine
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Results_Table As New DataTable
        Dim TempRoundedPagingCount As Integer = 0
        Dim temp_string As String = ""
        Try

            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else


                ViewTMMDropDowns.setIsView(False)
                ViewTMMDropDowns.setShowWeightClass(True)

                'ViewTMMDropDowns.setOverideMultiSelect(True)
                'ViewTMMDropDowns.setOverideDefaultModel(True)

                If History = True Then
                    ViewTMMDropDowns.setListSize(16)
                    'Add help button text here: 7/20/15
                    ' help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("History")
                    masterPage.UpdateHelpLink(clsGeneral.clsGeneral.CreateEvoHelpLink("History", True))
                ElseIf MarketEvent = True Then
                    ViewTMMDropDowns.setListSize(14)
                    'Add help button text here: 7/20/15
                    ' help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Events")
                    masterPage.UpdateHelpLink(clsGeneral.clsGeneral.CreateEvoHelpLink("Events", True))
                Else
                    ViewTMMDropDowns.setListSize(14)
                    'Add help button text here: 7/20/15
                    'help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Aircraft Search")
                    masterPage.UpdateHelpLink(clsGeneral.clsGeneral.CreateEvoHelpLink("Aircraft Search", True))
                End If

                ViewTMMDropDowns.setShowMfrNames(True)
                ViewTMMDropDowns.setShowAcSize(True)

                ViewTMMDropDowns.setControlName(sTypeMakeModelCtrlBaseName)

                DisplayFunctions.FillUpSessionForMakeTypeModel(sTypeMakeModelCtrlBaseName, ViewTMMDropDowns)

                If Page.IsPostBack Then
                    'Setting up the project search
                    If Session.Item("isMobile") Then
                        If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
                            Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
                            If UBound(ModelData) = 3 Then
                                HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(ModelData(0)), crmWebClient.Constants.AIRFRAME_TYPE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
                                HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(ModelData(2)), crmWebClient.Constants.AIRFRAME_MAKE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME)
                                HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(ModelData(3)))
                            End If
                        End If
                    End If
                End If

                'Load Search Information:
                If Not Page.IsPostBack Then
                    If Session.Item("isMobile") Then
                        ac_advanced_search.CssClass = "display_none"
                        'wanted_make_model_panel.CssClass = "display_none"
                        MobileSearchVisible.Visible = True
                        DisplayFunctions.SingleModelLookupAndFill(makeModelDynamic, masterPage)
                    End If
                End If


                'set the control  up
                'Is base is for the aircraft base
                'Is view is for the view
                'Otherwise (if both our false) it defaults to company listing
                'Show inactive countries is useful on the history search bar, in case a historical record uses a defunct country.
                viewCCSTDropDowns.setIsBase(False)
                viewCCSTDropDowns.setIsView(False)
                viewCCSTDropDowns.setFirstControl(False)
                viewCCSTDropDowns.setListSize(6)
                viewCCSTDropDowns.setShowInactiveCountries(History) 'shows historical countries if on history


                viewCCSTDropDownsAirport.setIsBase(True)
                viewCCSTDropDownsAirport.setIsView(False)
                viewCCSTDropDownsAirport.setListSize(6)
                viewCCSTDropDownsAirport.setFirstControl(True)
                viewCCSTDropDownsAirport.setShowInactiveCountries(History) 'shows historical countries if on history

                ac_ser_no_from.Focus()

                eventCategoryTypeDropdowns.setListSize(4)
                eventCategoryTypeDropdowns.setControlName("Events")

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''Some neat functions that might help'''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 
                'Pass the tab index of what you want highlighted on the bar.
                If History = True Then
                    Aircraft_Search_Box_toggle.CssClass = "transaction_search_box"
                    event_search_box.Visible = False
                    market_search_box.Visible = False
                    transaction_box.Visible = True
                    masterPage.Set_Active_Tab(3)
                    'This will set page title.
                    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("History Search Results")
                    page_type.Text = "HISTORY"

                ElseIf MarketEvent = True Then
                    Aircraft_Search_Box_toggle.CssClass = "event_search_box"
                    event_search_box.Visible = True
                    market_search_box.Visible = False
                    transaction_box.Visible = False
                    masterPage.Set_Active_Tab(7)
                    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Event Search Results")
                    page_type.Text = "EVENTS"
                    mobileSearchRadioToggle.Visible = False
                Else
                    Aircraft_Search_Box_toggle.CssClass = "market_search_box"
                    event_search_box.Visible = False
                    market_search_box.Visible = True
                    transaction_box.Visible = False
                    masterPage.Set_Active_Tab(2)
                    'This will set page title.
                    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Aircraft Search Results")
                    page_type.Text = "AIRCRAFT"
                End If

                ToggleHigherLowerBar(False)

                If Not IsNothing(Request.Item("cboEventsCategories")) Then
                    If Not String.IsNullOrEmpty(Request.Item("cboEventsCategories").ToString.Trim) Then

                        If Not Request.Item("cboEventsCategories").ToString.ToLower.Contains("all") Then
                            HttpContext.Current.Session.Item("eventCatType") = Request.Item("cboEventsCategories").ToString.Trim
                        Else
                            HttpContext.Current.Session.Item("eventCatType") = ""
                        End If

                    End If
                End If

                If Not IsNothing(Request.Item("cboEventsTypeCodes")) Then
                    If Not String.IsNullOrEmpty(Request.Item("cboEventsTypeCodes").ToString.Trim) Then

                        If Not Request.Item("cboEventsTypeCodes").ToString.ToLower.Contains("all") Then
                            HttpContext.Current.Session.Item("eventCatCode") = Request.Item("cboEventsTypeCodes").ToString.Trim
                        Else
                            HttpContext.Current.Session.Item("eventCatCode") = ""
                        End If

                    End If
                End If

                If Not IsNothing(Request.Item("radEventsValue")) Then
                    If Not String.IsNullOrEmpty(Request.Item("radEventsValue").ToString.Trim) Then

                        HttpContext.Current.Session.Item("eventType") = Request.Item("radEventsValue").ToString.Trim

                    End If
                End If

                'Load Search Information:
                Dim TempFolderID As Integer = 0

                ''''''''''''''
                If Not Page.IsPostBack Then
                    Initial(True)


                    If Aircraft_Criteria.Visible = True Then

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'toggle aerodex
                        If Session.Item("localSubscription").crmAerodexFlag = True Then
                            aerodex_toggle.Visible = False
                            mobileStatus.Visible = False
                            mobileSearchRadioToggle.Visible = False
                        End If


                        If Page.Request.Form("project_search") = "Y" Then
                            If Page.Request.Form("project_id") <> 0 Then
                                TempFolderID = Page.Request.Form("project_id")
                            End If
                        End If

                        If TempFolderID = 0 Then
                            If Page.Request.Form("clearSelection") = "true" Then
                            Else
                                FillOutSearchParameters()
                            End If
                        End If

                        'Fill Up Folder Information
                        FillUpFolderInformation()

                        DisplayFunctions.SetPagingItem(per_page_dropdown)

                    End If


                Else

                    'Refill up Folder Information Based On
                    FillUpFolderInformation()

                    If sort_by_dropdown.Visible = True Then
                        SetPageSort(sort_dropdown.Items(0).Text)
                    End If
                    If go_to_dropdown.Visible = True Then
                        SetPageNumber(CInt(go_to_dropdown.Items(0).Text))
                    End If

                End If


                If journ_subcat_code_part2.Items.Count < 2 Then
                    '------------------------------------------------------
                    masterPage.aclsData_Temp.FillCacheLookups()

                    If Not IsNothing(Cache("CacheLookups")) Then
                        LookupDataSet = Cache("CacheLookups")
                    End If
                    clsGeneral.clsGeneral.Populate_Listbox(LookupDataSet.Tables(0), journ_subcat_code_part2, "cbus_name", "cbus_type", False)

                    clsGeneral.clsGeneral.Populate_Listbox(LookupDataSet.Tables(0), journ_subcat_code_part3, "cbus_name", "cbus_type", False)
                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub makeModelDynamic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles makeModelDynamic.SelectedIndexChanged
        Initial(True)

        If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
            Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
            If UBound(ModelData) = 3 Then
                HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(ModelData(0)), crmWebClient.Constants.AIRFRAME_TYPE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
                HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(ModelData(2)), crmWebClient.Constants.AIRFRAME_MAKE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME)
                HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(ModelData(3)))

            End If
        End If


    End Sub

    Private Sub FillUpFolderInformation()
        Try
            Dim FoldersTable As New DataTable
            Dim TypeOfFolder As Integer = 0
            'Fill up the Aircraft Folder List based on historical data or not

            If History = True Then
                TypeOfFolder = 8
                folders_submenu_dropdown.Items.Clear()
                FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", TypeOfFolder, Nothing, "")
            ElseIf MarketEvent = True Then
                TypeOfFolder = 5
                folders_submenu_dropdown.Items.Clear()
                FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", TypeOfFolder, Nothing, "A")
            Else
                TypeOfFolder = 3
                folders_submenu_dropdown.Items.Clear()
                FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", TypeOfFolder, Nothing, "")
            End If
            DisplayFunctions.AddEditFolderListOptionToFolderDropdown(folders_submenu_dropdown, TypeOfFolder)


            If Not IsNothing(FoldersTable) Then
                If FoldersTable.Rows.Count > 0 Then
                    For Each r As DataRow In FoldersTable.Rows
                        If Not IsDBNull(r("cfolder_data")) Then
                            Dim FolderDataString As Array
                            'this was added to parse out the real search query now that we're saving it
                            FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

                            If Replace(r("cfolder_data").ToString, "journ_id=", "") = "" And (History = True) Then
                                folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:alert('This folder contains no information.');"))
                            Else
                                folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseForm('" & r("cfolder_id").ToString & "', " & IIf(History = True, "true", "false") & ", " & IIf(MarketEvent = True, "true", "false") & ",false, false, false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
                            End If
                        End If
                    Next
                End If
            End If

            FoldersTable = Nothing

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub CheckListboxValue(ByVal value As String, ByVal lb As ListBox)
        Try
            Dim MultipleSelection As Array
            'We split the answer.
            MultipleSelection = Split(value, "##") 'value.Split("##")
            'We also need to account to make sure that the selection mode on the listbox is that of
            'multiple selections. If it is, we run through and select all the picked ones
            If lb.SelectionMode = ListSelectionMode.Multiple Then
                lb.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                'that the page defaults to.
                For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                    For ListBoxCount As Integer = 0 To lb.Items.Count() - 1
                        If UCase(lb.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                            lb.Items(ListBoxCount).Selected = True
                        End If
                    Next

                Next
            Else
                lb.SelectedValue = value
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Protected Sub FillTransactionType()

        Try

            journ_subcat_code_part1.Items.Clear()

            If transaction_retail.Checked = True Then
                journ_subcat_code_part1.Items.Add(New ListItem("All", "AllR"))
                journ_subcat_code_part1.Items.Add(New ListItem("Whole", "Whole"))
                journ_subcat_code_part1.Items.Add(New ListItem("Share", "Share"))
                journ_subcat_code_part1.Items.Add(New ListItem("Leases", "Leases"))
                journ_subcat_code_part1.SelectedValue = "AllR"
            Else
                journ_subcat_code_part1.Items.Add(New ListItem("All", ""))
                journ_subcat_code_part1.Items.Add(New ListItem("All Sales", "All Sales"))
                journ_subcat_code_part1.Items.Add(New ListItem("Whole", "Whole"))
                journ_subcat_code_part1.Items.Add(New ListItem("Share", "Share"))
                journ_subcat_code_part1.Items.Add(New ListItem("Fractional", "Fractional"))
                journ_subcat_code_part1.Items.Add(New ListItem("Leases", "Leases"))
                journ_subcat_code_part1.Items.Add(New ListItem("Delivery Position", "Delivery Position"))
                journ_subcat_code_part1.Items.Add(New ListItem("Foreclosures", "Foreclosures"))
                journ_subcat_code_part1.Items.Add(New ListItem("Seizures", "Seizures"))
                journ_subcat_code_part1.Items.Add(New ListItem("Written Off", "Written Off"))
                journ_subcat_code_part1.Items.Add(New ListItem("Withdrawn from Use", "Withdrawn from Use"))
                journ_subcat_code_part1.Items.Add(New ListItem("Withdrawn from Use-Stored", "Withdrawn from Use-Stored"))
                journ_subcat_code_part1.SelectedValue = ""
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub Build_Advanced_Search()

        Dim LoggingWhereThisStops As String = "0" 'This variable needs to be filled as it goes through this function.
        'It is throwing an error that I can't reproduce, so I need this to figure out what is going on.
        Try

            Dim MainContent As New ContentPlaceHolder
            'Added 7/15/2015.
            'This is going to set up the master page content holder so we can go ahead and reference it later on for the date range picker.
            If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
                MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
            End If


            Dim CountryTable As New DataTable
            Dim StateTable As New DataTable
            Dim RegionTable As New DataTable
            Dim ContinentTable As New DataTable
            Dim RelationshipTable As New DataTable

            Dim TemporaryTable As New DataTable
            Dim TemporaryFields As New DataTable
            Dim Counter As Integer = 1
            Dim SubCounter As Integer = 0

            If Not IsNothing(masterPage) Then
                If Not IsNothing(masterPage.aclsData_Temp) Then
                    'First let's go ahead and fill in the company tab with session information.
                    'Equipment/Maintenance Tab:
                    If Not IsNothing(Session.Item("Advanced-us_ac_maintained")) Then
                        DisplayFunctions.SelectInformation(us_ac_maintained, Session.Item("Advanced-us_ac_maintained"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-foreign_ac_maintained")) Then
                        DisplayFunctions.SelectInformation(foreign_ac_maintained, Session.Item("Advanced-foreign_ac_maintained"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-emp_provider_name")) Then
                        DisplayFunctions.SelectInformation(emp_provider_name, Session.Item("Advanced-emp_provider_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-emp_provider_name")) Then
                        DisplayFunctions.SelectInformation(emp_program_name, Session.Item("Advanced-emp_program_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-emgp_provider_name")) Then
                        DisplayFunctions.SelectInformation(emgp_provider_name, Session.Item("Advanced-emgp_provider_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-emgp_program_name")) Then
                        DisplayFunctions.SelectInformation(emgp_program_name, Session.Item("Advanced-emgp_program_name"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-amp_provider_name")) Then
                        DisplayFunctions.SelectInformation(amp_provider_name, Session.Item("Advanced-amp_provider_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-amp_program_name")) Then
                        DisplayFunctions.SelectInformation(amp_program_name, Session.Item("Advanced-amp_program_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-amtp_provider_name")) Then
                        DisplayFunctions.SelectInformation(amtp_provider_name, Session.Item("Advanced-amtp_provider_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-amtp_program_name")) Then
                        DisplayFunctions.SelectInformation(amtp_program_name, Session.Item("Advanced-amtp_program_name"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-maintenance_item")) Then
                        DisplayFunctions.SelectInformation(maintenance_item, Session.Item("Advanced-maintenance_item"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-maintenance_item1")) Then
                        DisplayFunctions.SelectInformation(maintenance_item1, Session.Item("Advanced-maintenance_item1"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-acmaint_date")) Then
                        DisplayFunctions.SelectInformation(acmaint_date, Session.Item("Advanced-acmaint_date"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-acmaint_date1")) Then
                        DisplayFunctions.SelectInformation(acmaint_date1, Session.Item("Advanced-acmaint_date1"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-acmaint_time")) Then
                        DisplayFunctions.SelectInformation(acmaint_time, Session.Item("Advanced-acmaint_time"))
                    End If
                    If Not IsNothing(Session.Item("Advanced-acmaint_time1")) Then
                        DisplayFunctions.SelectInformation(acmaint_time1, Session.Item("Advanced-acmaint_time1"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-acmaint_value")) Then
                        DisplayFunctions.SelectInformation(acmaint_value, Session.Item("Advanced-acmaint_value"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-acmaint_value1")) Then
                        DisplayFunctions.SelectInformation(acmaint_value1, Session.Item("Advanced-acmaint_value1"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-acmaint_chk")) Then
                        DisplayFunctions.SelectInformation(acmaint_chk, Session.Item("Advanced-acmaint_chk"))
                    End If

                    If Not IsNothing(Session.Item("Advanced-acmaint_chk1")) Then
                        DisplayFunctions.SelectInformation(acmaint_chk1, Session.Item("Advanced-acmaint_chk1"))
                    End If


                    'Contact Type
                    If Not IsNothing(Session.Item("Advanced-cref_contact_type")) Then
                        DisplayFunctions.SelectInformation(cref_contact_type, Session.Item("Advanced-cref_contact_type"))
                    End If

                    'Company folder id
                    If Not IsNothing(Session.Item("Advanced-cref_comp_id")) Then
                        DisplayFunctions.SelectInformation(cref_comp_id, Session.Item("Advanced-cref_comp_id"))
                    End If
                    'Business Type
                    If Not IsNothing(Session.Item("Advanced-cref_business_type")) Then
                        DisplayFunctions.SelectInformation(cref_business_type, Session.Item("Advanced-cref_business_type"))
                    End If
                    'Comp Not in selected relationship
                    If Not IsNothing(Session.Item("Advanced-comp_not_in_selected")) Then
                        DisplayFunctions.SelectInformation(comp_not_in_selected, Session.Item("Advanced-comp_not_in_selected"))
                    End If
                    'Comp Name
                    If Not IsNothing(Session.Item("Advanced-comp_name")) Then
                        DisplayFunctions.SelectInformation(comp_name, Session.Item("Advanced-comp_name"))
                    End If

                    'Contact Title
                    If Not IsNothing(Session.Item("Advanced-contact_title")) Then
                        DisplayFunctions.SelectInformation(contact_title, Session.Item("Advanced-contact_title"))
                    End If
                    'Comp Address
                    If Not IsNothing(Session.Item("Advanced-comp_address1")) Then
                        DisplayFunctions.SelectInformation(comp_address1, Session.Item("Advanced-comp_address1"))
                    End If

                    'Comp City
                    If Not IsNothing(Session.Item("Advanced-comp_city")) Then
                        DisplayFunctions.SelectInformation(comp_city, Session.Item("Advanced-comp_city"))
                    End If

                    'Comp postal code
                    If Not IsNothing(Session.Item("Advanced-comp_zip_code")) Then
                        DisplayFunctions.SelectInformation(comp_zip_code, Session.Item("Advanced-comp_zip_code"))
                    End If

                    'Contact First Name
                    If Not IsNothing(Session.Item("Advanced-contact_first_name")) Then
                        DisplayFunctions.SelectInformation(contact_first_name, Session.Item("Advanced-contact_first_name"))
                    End If

                    'Contact Last Name
                    If Not IsNothing(Session.Item("Advanced-contact_last_name")) Then
                        DisplayFunctions.SelectInformation(contact_last_name, Session.Item("Advanced-contact_last_name"))
                    End If

                    'Email
                    If Not IsNothing(Session.Item("Advanced-comp_email_address")) Then
                        DisplayFunctions.SelectInformation(comp_email_address, Session.Item("Advanced-comp_email_address"))
                    End If

                    'Phone
                    If Not IsNothing(Session.Item("Advanced-comp_phone_office")) Then
                        DisplayFunctions.SelectInformation(comp_phone_office, Session.Item("Advanced-comp_phone_office"))
                    End If

                    'Comp Agency Type
                    If Not IsNothing(Session.Item("Advanced-comp_agency_type")) Then
                        DisplayFunctions.SelectInformation(comp_agency_type, Session.Item("Advanced-comp_agency_type"))
                    End If

                    LoggingWhereThisStops = "1st"

                    TemporaryTable = masterPage.aclsData_Temp.ListofTabsForCustomSearch("Aircraft")
                    If Not IsNothing(TemporaryTable) Then
                        If TemporaryTable.Rows.Count > 0 Then
                            Dim TempPanel As New AjaxControlToolkit.TabPanel
                            For Each r As DataRow In TemporaryTable.Rows
                                If UCase(r("cefstab_sub_name").ToString) <> "COMPANY/CONTACT" Then

                                    TempPanel = New AjaxControlToolkit.TabPanel
                                    Dim Tab As New Table
                                    Dim TR As New TableRow
                                    Dim TD As New TableCell
                                    Dim TD_2 As New TableCell
                                    Dim TDTEXT As New TextBox
                                    Dim TDSELECT As New DropDownList
                                    Dim TDSELECTCOMPARISON As New DropDownList
                                    Dim TD_3 As New TableCell
                                    Dim TD_4 As New TableCell
                                    Dim TD_5 As New TableCell 'For Validation Message.
                                    Dim LB As New Label
                                    Dim LB_2 As New Label
                                    Dim Display_Block As String = ""
                                    Dim cssClass As String = ""
                                    Dim SetUpLinkOutStart As String = ""
                                    Dim SetUpLinkOutEnd As String = ""
                                    Dim ConditionFormatCSSClass As String = "" 'This only gets toggled on when you're on the features tab.

                                    'Validation Controls.
                                    Dim NumberCustom As New RegularExpressionValidator
                                    Dim CustomValidation As New CustomValidator

                                    Tab.Width = Unit.Percentage(100D)
                                    Tab.CssClass = "data_aircraft_grid"
                                    Tab.CellPadding = 5

                                    TempPanel.ID = "TAB" & r("cefstab_id")
                                    If UCase(r("cefstab_sub_name")) = "MARKET STATUS" Then
                                        TempPanel.HeaderText = "Market"
                                    Else
                                        TempPanel.HeaderText = r("cefstab_sub_name")
                                    End If

                                    TempPanel.Visible = True


                                    TemporaryFields = masterPage.aclsData_Temp.ListofTabsFieldsBasedonTabID(r("cefstab_id"))

                                    If UCase(r("cefstab_sub_name").ToString) = "PROPELLER" Then
                                        Tab = BuildEngineCustom(Tab, False)
                                    End If

                                    LoggingWhereThisStops = "2nd"

                                    If Not IsNothing(TemporaryFields) Then
                                        If TemporaryFields.Rows.Count > 0 Then
                                            If UCase(r("cefstab_sub_name").ToString) = "AIRFRAME/ENGINES/APU" Then
                                                Tab = BuildEngineCustom(Tab, True)
                                            End If

                                            TR.CssClass = "header_row"
                                            TD.Text = "<b>Field</b>"
                                            TD.Width = Unit.Percentage(20D)
                                            TD_2.Text = "<b>Condition</b>"
                                            TD_2.Width = Unit.Percentage(15D)
                                            TD_3.Text = "<b>Value</b>"
                                            TD_3.Width = Unit.Percentage(15D)
                                            TD_4.Text = "<b>Format</b>"

                                            'We need to override the widths for just the features tab.
                                            If UCase(r("cefstab_sub_name").ToString) = "FEATURES" Then
                                                'Adding a custom label above the table.
                                                LB_2 = New Label
                                                LB_2.Text = "<br /><p class=""small_to_medium_text"" align=""center"">The following features may not be applicable to all aircraft models. To see a list of aircraft models for each feature click on the link to the right labeled as View Applicable Models</p>"
                                                TempPanel.Controls.Add(LB_2)
                                                TD.Width = Unit.Percentage(27D)
                                                TD_2.Width = Unit.Percentage(8D)
                                                TD_3.Width = Unit.Percentage(10D)
                                                TD_4.Width = Unit.Percentage(13D)

                                                ConditionFormatCSSClass = "display_none"

                                                TD_2.CssClass = ConditionFormatCSSClass
                                                TD_4.CssClass = ConditionFormatCSSClass

                                                'We need to td_5 to take up all the rest of the available space, so not setting a width.

                                                TD_5.Text = "<b>Applies to Models</b> <a href='#' class=""float_right"" onclick=""javascript:load('MasterLists.aspx?helplist=featuremodel','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">View Model Features List</a>"
                                            Else
                                                TD_4.Width = Unit.Percentage(38D)
                                                TD_5.Width = Unit.Percentage(12D)
                                            End If

                                            TR.Controls.Add(TD)
                                            TR.Controls.Add(TD_2)
                                            TR.Controls.Add(TD_3)
                                            TR.Controls.Add(TD_4)
                                            TR.Controls.Add(TD_5)
                                            Tab.Controls.Add(TR)
                                            'End If
                                            SubCounter = 0
                                            Display_Block = "" ' TemporaryFields.Rows(0).Item("cefsblk_name").ToString

                                            LoggingWhereThisStops = "3rd"

                                            For Each q As DataRow In TemporaryFields.Rows

                                                'If q("cef_evo_field_name").ToString <> "comp_city" And q("cef_evo_field_name").ToString <> "ac_feat_htw" Then
                                                'This is the block that shows the group heading. 
                                                If Display_Block <> q("cefsblk_name").ToString Then
                                                    TR = New TableRow
                                                    TR.CssClass = "header_row"
                                                    TD = New TableCell

                                                    If cssClass = "" Then
                                                        cssClass = "alt_row"
                                                    Else
                                                        cssClass = ""
                                                    End If
                                                    Display_Block = q("cefsblk_name").ToString

                                                    TD = New TableCell
                                                    TD.CssClass = "data_aircraft_grid_cell light_seafoam_green_header_color"
                                                    'TD.BackColor = System.Drawing.ColorTranslator.FromHtml("#e7eeeb")
                                                    TD.ColumnSpan = 5
                                                    TD.Text = "<b>" & q("cefsblk_name").ToString & "</b>"

                                                    'If q("cefsblk_name").ToString = "Maintenance Regulation" Then
                                                    '    TD.Text &= "&nbsp;&nbsp;<a href='https://www.jetnetevolution.com/help/documents/1034.pdf' target='_blank'><img src='images/magnify_small.png' width='9' alt='" & q("cefsblk_name").ToString & "' title='" & q("cefsblk_name").ToString & "' /></a>"
                                                    'End If

                                                    TR.Controls.Add(TD)
                                                    Tab.Controls.Add(TR)
                                                End If

                                                LB = New Label
                                                LB_2 = New Label
                                                TR = New TableRow
                                                TR.CssClass = cssClass
                                                TD = New TableCell
                                                TD_2 = New TableCell
                                                TD_3 = New TableCell
                                                TD_4 = New TableCell
                                                TD_5 = New TableCell



                                                CustomValidation = New CustomValidator
                                                NumberCustom = New RegularExpressionValidator
                                                TDTEXT = New TextBox
                                                TDSELECT = New DropDownList
                                                TDSELECTCOMPARISON = New DropDownList
                                                SetUpLinkOutStart = ""
                                                SetUpLinkOutEnd = ""


                                                'If there's a definition, switch this table cell class to a help cursor
                                                'Also sets the tooltip.
                                                If (q("cef_definition").ToString <> "") Then
                                                    TD.CssClass = "help_cursor"
                                                    If UCase(r("cefstab_sub_name").ToString) = "FEATURES" Then
                                                    Else
                                                        TD.ToolTip = q("cef_definition").ToString
                                                    End If
                                                End If

                                                If Not IsDBNull(q("cef_link")) Then
                                                    If Not String.IsNullOrEmpty(Trim(q("cef_link"))) Then
                                                        SetUpLinkOutStart = "<a href=""#"" onclick=""javascript:load('" & q("cef_link") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                                        SetUpLinkOutEnd = "</a>"
                                                    End If
                                                End If
                                                'This displays the text in the first textbox, but also displays a magnifying glass (smaller than picture) if there is a definition (mouseover)
                                                If UCase(r("cefstab_sub_name").ToString) = "FEATURES" Then
                                                    TD.Text = SetUpLinkOutStart & q("cef_display").ToString & SetUpLinkOutEnd & ": " & IIf(q("cef_definition").ToString <> "", "&nbsp;", "")
                                                Else
                                                    TD.Text = SetUpLinkOutStart & q("cef_display").ToString & SetUpLinkOutEnd & ": " & IIf(q("cef_definition").ToString <> "", "&nbsp;<img src='images/magnify_small.png' width='9' alt='" & q("cef_definition").ToString & "' title='" & q("cef_definition").ToString & "' />", "")
                                                End If

                                                'Adds the tablecell
                                                TR.Controls.Add(TD)

                                                'Comparison Field
                                                If q("cef_display") = "Engine MFR Name" Then
                                                    TDSELECTCOMPARISON.ID = "COMPARE_engine_mfr_name_static"
                                                Else
                                                    TDSELECTCOMPARISON.ID = "COMPARE_" & AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString) ' q("cef_field_type").ToString & "-" & q("cef_id") & "-" & q("cef_evo_field_name").ToString
                                                End If
                                                TDSELECTCOMPARISON.Width = Unit.Percentage(100D)
                                                TDSELECTCOMPARISON = DisplayFunctions.Fill_Dropdown(q("cef_field_type").ToString, TDSELECTCOMPARISON, q("cef_values").ToString)

                                                If UCase(q("cef_field_type").ToString) = "DATE" Then
                                                    TDSELECTCOMPARISON.CssClass = "display_none"
                                                End If

                                                'This piece of code is going to check and see if the values are set for this form item.
                                                'If they are, it's going to set an attribute.
                                                'This is going to reference a javascript function
                                                'in common_functions.js.
                                                'This function is going to set an onchange event on a select dropdown.
                                                'It will then watch and if the select box comparison is changed to empty, 
                                                'It will go ahead and clear the associated textbox value.
                                                'You can pass the type of input on the third parameter of the javascript function.
                                                'In this case it is textarea because we're setting up multiline (text area) boxes.
                                                If String.IsNullOrEmpty(q("cef_values").ToString) Then
                                                    TDSELECTCOMPARISON.Attributes.Add("onChange", "javascript:ClearAssociatedBox($(this).find(':selected').val(),'" & AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString) & "', 'textarea');")
                                                End If

                                                'Setting up the display none toggle on features tab.
                                                TD_2.CssClass = ConditionFormatCSSClass


                                                'This check will see whether or not the dropdown only has equals. If it does, we're going to go ahead
                                                'And add a label that will basically say equals. The way we tell if the dropdown only has equals
                                                'is because the function up above sets the css class to display_none if it is equals.
                                                If TDSELECTCOMPARISON.CssClass = "display_none" Then
                                                    LB.Text = TDSELECTCOMPARISON.SelectedItem.Text
                                                    LB.CssClass = "lighter_gray_text"
                                                    TD_2.Controls.Add(LB)
                                                ElseIf TDSELECTCOMPARISON.CssClass = "display_none includes" Then
                                                    LB.Text = "Enter Search Term"
                                                    LB.CssClass = "lighter_gray_text"
                                                    TD_2.Controls.Add(LB)
                                                End If

                                                'Fill in the session saved value.
                                                DisplayFunctions.SelectInformation(TDSELECTCOMPARISON, Session.Item("Advanced-" & TDSELECTCOMPARISON.ID))

                                                LoggingWhereThisStops = "4th"

                                                TD_2.Controls.Add(TDSELECTCOMPARISON)
                                                TR.Controls.Add(TD_2)

                                                If Not IsDBNull(q("cef_values")) Then
                                                    If Not String.IsNullOrEmpty(q("cef_values").ToString.Trim) Then
                                                        Dim TempHold As Array = Nothing

                                                        If q("cef_display") = "Engine MFR Name" Then
                                                            TDSELECT.ID = "engine_mfr_name_static"
                                                        Else
                                                            TDSELECT.ID = AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString) 'q("cef_field_type").ToString & "-" & q("cef_id") & "-" & q("cef_evo_field_name").ToString
                                                        End If

                                                        TDSELECT.ValidationGroup = q("cef_field_type").ToString
                                                        TempHold = Split(q("cef_values"), ",")
                                                        TDSELECT.Items.Add(New ListItem("", ""))
                                                        'This tooltip is set as the display on purpose.
                                                        'When we are looping through them, instead of adding yet another pipe in the ID
                                                        'Using the tooltip is an easy way to get the Textual Display of this Field.
                                                        TDSELECT.ToolTip = q("cef_display").ToString

                                                        For x = 0 To UBound(TempHold)
                                                            TDSELECT.Items.Add(New ListItem(Replace(Trim(TempHold(x)), "&#44;", ","), IIf(UCase(q("cef_field_type").ToString) = "CHAR", Left(Trim(TempHold(x)), 1), Replace(Trim(TempHold(x)), "&#44;", ","))))
                                                        Next


                                                        TDSELECT.Width = Unit.Percentage(100D)

                                                        'Fill in the session saved value.
                                                        DisplayFunctions.SelectInformation(TDSELECT, Session.Item("Advanced-" & TDSELECT.ID))

                                                        'If just on the case of the project search
                                                        'Well, really only if we're doing a summary call back
                                                        'if the session is set, but there's a request variable
                                                        'we override it.
                                                        'On a summary call back - we go ahead and don't clear the session
                                                        'regular projects we do.
                                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                                            If Page.Request.Form("project_search") = "Y" Then
                                                                Dim temp As String = ""
                                                                If Not IsNothing(Request.Form(TDSELECT.ID)) Then
                                                                    temp = Request.Form(TDSELECT.ID)
                                                                    If Not String.IsNullOrEmpty(temp) Then
                                                                        TDSELECT.SelectedValue = Request.Form(TDSELECT.ID)
                                                                    End If
                                                                End If
                                                            End If
                                                        End If

                                                        TD_3.Controls.Add(TDSELECT)

                                                        TD_4.Text = DisplayFunctions.DisplayFormatRules("Dropdown")

                                                        'Only on features tab. We're going to add a label.
                                                        If UCase(r("cefstab_sub_name").ToString) = "FEATURES" Then
                                                            If (q("cef_definition").ToString <> "") Then 'But only if the definition (tooltip) isn't blank
                                                                LB_2 = New Label
                                                                Dim DisplayableText As String = ""
                                                                Dim TrimEndText As String = ""
                                                                ' LB_2.Text = "<img src=""images/alert.png"" alt=""Important Notice"" title=""Important Notice"" width=""11"" class=""alertImageAircraftListing"" />"
                                                                LB_2.Text += "<A href='MasterLists.aspx?helplist=featuremodel&fcode=" & Replace(q("cef_evo_field_name").ToString, "ac_feat_", "") & "' target='_blank'>View Applicable Models</a>"
                                                                ' If Len(q("cef_definition").ToString) > 83 Then
                                                                '  DisplayableText = Trim(Left(q("cef_definition").ToString, 83))
                                                                '   DisplayableText = DisplayableText.TrimEnd(",")
                                                                ' LB_2.Text += DisplayableText & "... <span title=""" & q("cef_definition").ToString & """ class=""help_cursor underline smaller_text""></span>"
                                                                '  Else
                                                                ' LB_2.Text += q("cef_definition").ToString.TrimEnd(",")
                                                                '   End If
                                                                '  TD_5.ToolTip = q("cef_definition").ToString.TrimEnd(",")
                                                                TD_5.Controls.Add(LB_2)
                                                            End If
                                                        End If


                                                    End If
                                                End If
                                                LoggingWhereThisStops = "5th"

                                                If TDSELECT.ID = "" Then 'use a textbox
                                                    TDTEXT.ID = AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString) 'q("cef_field_type").ToString & "-" & q("cef_id") & "-" & q("cef_evo_field_name").ToString
                                                    ' TDTEXT.MaxLength = IIf(Not IsDBNull(q("cef_field_length")), q("cef_field_length"), 0)
                                                    TDTEXT.Width = Unit.Percentage(99D)
                                                    TDTEXT.ValidationGroup = q("cef_field_type").ToString

                                                    'This tooltip is set as the display on purpose.
                                                    'When we are looping through them, instead of adding yet another pipe in the ID
                                                    'Using the tooltip is an easy way to get the Textual Display of this Field.
                                                    TDTEXT.ToolTip = q("cef_display").ToString
                                                    TDTEXT.TextMode = TextBoxMode.MultiLine
                                                    TDTEXT.Height = Unit.Pixel(12)
                                                    TDTEXT.Rows = 1

                                                    'Fill in the session saved value.
                                                    If Not IsNothing(Session.Item("Advanced-" & TDTEXT.ID)) Then
                                                        If Not String.IsNullOrEmpty(Session.Item("Advanced-" & TDTEXT.ID)) Then
                                                            TDTEXT.Text = Session.Item("Advanced-" & TDTEXT.ID)
                                                        End If
                                                    End If
                                                    'Fill in the session saved value.
                                                    DisplayFunctions.SelectInformation(TDTEXT, Session.Item("Advanced-" & TDTEXT.ID))


                                                    'If just on the case of the project search
                                                    'Well, really only if we're doing a summary call back
                                                    'if the session is set, but there's a request variable
                                                    'we override it.
                                                    'On a summary call back - we go ahead and don't clear the session
                                                    'regular projects we do.
                                                    If Not IsNothing(Page.Request.Form("project_search")) Then
                                                        If Page.Request.Form("project_search") = "Y" Then
                                                            Dim temp As String = ""
                                                            If Not IsNothing(Request.Form(TDTEXT.ID)) Then
                                                                temp = Request.Form(TDTEXT.ID)
                                                                If Not String.IsNullOrEmpty(temp) Then
                                                                    TDTEXT.Text = Request.Form(TDTEXT.ID)
                                                                End If
                                                            End If
                                                        End If
                                                    End If

                                                    'This was added on 7/15/2015.
                                                    'This is registering a javascript block - only if the field type is date and only on the history page (that second part is temporary).
                                                    'This javascript block goes ahead and initiates the rangepicker for the date field types.
                                                    'This code also adds an attribute to the textbox.
                                                    'This textbox attribute is called data. It's a way to store the comparable operator in a way so that none of the naming conventions (due to the master page/parent controls)
                                                    'have to be hard coded. Now at least if any of the panels names get changed, it will give you an error in code behind so you'll know that this
                                                    'will need to be updated as well.
                                                    'MainContent is the masterpage content control. If you go to the beginning of this function, it will
                                                    'Go ahead and declare a new control, then look for the one that's named correctly.
                                                    'If it finds it, this will be named right and it will initiate the rangepicker.
                                                    'if for some reason it doesn't find it - it will still work as a textbox date fill-in and there should be
                                                    'no error.
                                                    If UCase(q("cef_field_type").ToString) = "DATE" Then
                                                        'If History Then
                                                        If Not Page.ClientScript.IsClientScriptBlockRegistered("DateRangePicker_" & AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString)) Then
                                                            Dim DateRangePicker As StringBuilder = New StringBuilder()
                                                            If Session.Item("isMobile") = False Then
                                                                DateRangePicker.Append("<script type=""text/javascript"">")
                                                                DateRangePicker.Append("$(function(){")
                                                                DateRangePicker.Append("$('#" & MainContent.ClientID & "_" & ac_advanced_search.ID & "_" & TempPanel.ID & "_" & TDTEXT.ClientID & "').daterangepicker();")
                                                                TDTEXT.Attributes.Add("data", MainContent.ClientID & "_" & ac_advanced_search.ID & "_" & TempPanel.ID & "_COMPARE_" & TDTEXT.ClientID)
                                                                DateRangePicker.Append("});")
                                                                DateRangePicker.Append("</script>")
                                                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DateRangePicker_" & AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name")).ToString, DateRangePicker.ToString, False)
                                                            End If
                                                        End If
                                                        ' End If
                                                    End If

                                                    TD_3.Controls.Add(TDTEXT)

                                                    Dim bIsNullORBlank As Boolean = False
                                                    ' special case for items that are "blank" '' or "null" IS NULL
                                                    If TDTEXT.Text.ToUpper.Contains("IS NULL") Or TDTEXT.Text.ToUpper.Contains("BLANK") Then
                                                        bIsNullORBlank = True
                                                    End If

                                                    LoggingWhereThisStops = "6th"

                                                    'This validation is only going in for numeric fields for right now.
                                                    'This is a work in progress and a test, I don't want to add them all at once,
                                                    'but would rather work with one at a time.
                                                    If q("cef_field_type").ToString = "Numeric" Or q("cef_field_type").ToString = "Year" Then
                                                        ' If q("cef_display").ToString <> "Days on Market" And q("cef_display").ToString <> "Month Purchased" Then
                                                        NumberCustom.ID = "VALIDATE_" & q("cef_id").ToString
                                                        NumberCustom.ErrorMessage = "*Incorrect Format"
                                                        'NumberCustom.Font.Size = Unit.
                                                        NumberCustom.Font.Bold = True
                                                        NumberCustom.ValidationGroup = "Numeric"
                                                        NumberCustom.ControlToValidate = TDTEXT.ID
                                                        NumberCustom.SetFocusOnError = True
                                                        NumberCustom.ValidationExpression = "^[\d,:\s\n]+$"
                                                        NumberCustom.Text = "*Incorrect Format"
                                                        NumberCustom.Display = ValidatorDisplay.Dynamic
                                                        NumberCustom.Enabled = IIf(Not bIsNullORBlank, True, False)
                                                        TD_5.Controls.Add(NumberCustom)

                                                        CustomValidation = New CustomValidator
                                                        CustomValidation.ID = "VALIDATE_BET" & q("cef_id").ToString
                                                        CustomValidation.ErrorMessage = "*Incorrect Format Between Requires :"
                                                        'NumberCustom.Font.Size = Unit.
                                                        CustomValidation.Font.Bold = True
                                                        CustomValidation.ValidationGroup = "Numeric"
                                                        CustomValidation.ControlToValidate = TDTEXT.ID
                                                        CustomValidation.SetFocusOnError = True
                                                        CustomValidation.ClientValidationFunction = "validateBetween"
                                                        CustomValidation.Attributes.Add("data", AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q("cef_evo_field_name").ToString))
                                                        CustomValidation.Text = "*Incorrect Format"
                                                        CustomValidation.Display = ValidatorDisplay.Dynamic
                                                        CustomValidation.Enabled = IIf(Not bIsNullORBlank, True, False)
                                                        TD_5.Controls.Add(CustomValidation)
                                                        'End If
                                                    ElseIf q("cef_field_type").ToString = "Date" Then
                                                        CustomValidation.ID = "VALIDATE_" & q("cef_id").ToString
                                                        CustomValidation.ErrorMessage = "*Incorrect Format"
                                                        'NumberCustom.Font.Size = Unit.
                                                        CustomValidation.Font.Bold = True
                                                        CustomValidation.ValidationGroup = "Numeric"
                                                        CustomValidation.ControlToValidate = TDTEXT.ID
                                                        CustomValidation.SetFocusOnError = True
                                                        CustomValidation.ClientValidationFunction = "validateDate"
                                                        CustomValidation.Text = "*Incorrect Format"
                                                        CustomValidation.Display = ValidatorDisplay.Static
                                                        CustomValidation.Enabled = IIf(Not bIsNullORBlank, True, False)
                                                        TD_5.Controls.Add(CustomValidation)
                                                    End If

                                                    If LB.Text <> "Enter Search Term" Then
                                                        TD_4.Text = DisplayFunctions.DisplayFormatRules(q("cef_field_type").ToString)
                                                    End If



                                                End If

                                                TR.Controls.Add(TD_3)
                                                TD_4.CssClass = "lighter_gray_text " + ConditionFormatCSSClass
                                                TD_4.ToolTip = q("cef_definition").ToString
                                                TR.Controls.Add(TD_4)

                                                TR.Controls.Add(TD_5) 'Validator
                                                Tab.Controls.Add(TR)
                                                SubCounter += 1
                                                LoggingWhereThisStops = "7th"

                                            Next

                                        End If
                                    End If


                                    'This makes sure that the location only gets added to the location tab panel instead of a newly created panel
                                    If UCase(r("cefstab_sub_name").ToString) = "LOCATION" Then
                                        location_dynamic_panel.Controls.Add(Tab)
                                    ElseIf UCase(r("cefstab_sub_name").ToString) = "EQUIP/MAINT" Then
                                        equip_dynamic_panel.Controls.Add(Tab)
                                    Else
                                        TempPanel.Controls.Add(Tab)
                                        ac_advanced_search.Controls.AddAt(Counter - 1, TempPanel)
                                    End If

                                    Tab.Dispose()
                                    TempPanel.Dispose()



                                    Counter += 1

                                End If
                            Next
                            LoggingWhereThisStops = "8th"

                            'Addition of Financial Document Panel

                            If Not IsNothing(masterPage.aclsData_Temp) Then
                                TemporaryTable = masterPage.aclsData_Temp.Get_Financial_Institution_Primary_Group("")
                                If Not IsNothing(TemporaryTable) Then
                                    TempPanel = New AjaxControlToolkit.TabPanel

                                    If TemporaryTable.Rows.Count > 0 Then

                                        Dim Tab As New Table
                                        Tab.CellPadding = 5
                                        Tab.CssClass = "data_aircraft_grid"
                                        Tab.Width = Unit.Percentage(100D)

                                        Dim TR As New TableRow
                                        Dim TD As New TableCell
                                        Dim TD_2 As New TableCell
                                        Dim TD_3 As New TableCell
                                        Dim TD_4 As New TableCell
                                        Dim TD_5 As New TableCell

                                        Dim TDSELECT As New DropDownList
                                        Dim TDTEXT As New TextBox
                                        Dim TDSELECTLIST As New ListBox
                                        Dim TDSELECTCOMPARISON As New DropDownList
                                        Dim LB As New Label
                                        Dim CustomValidation As New CustomValidator

                                        TR.CssClass = "header_row"
                                        TD.Text = "<b>Field</b>"
                                        TD.Width = Unit.Percentage(20D)
                                        TD_2.Text = "<b>Condition</b>"
                                        TD_2.Width = Unit.Percentage(15D)
                                        TD_3.Text = "<b>Value</b>"
                                        TD_3.Width = Unit.Percentage(15D)
                                        TD_4.Text = "<b>Format</b>"
                                        TD_4.Width = Unit.Percentage(38D)
                                        TD_5.Width = Unit.Percentage(12D)
                                        TR.Controls.Add(TD)
                                        TR.Controls.Add(TD_2)
                                        TR.Controls.Add(TD_3)
                                        TR.Controls.Add(TD_4)
                                        TR.Controls.Add(TD_5)
                                        Tab.Controls.Add(TR)

                                        TR = New TableRow
                                        TD = New TableCell
                                        TD_2 = New TableCell
                                        TD_3 = New TableCell
                                        TD_4 = New TableCell
                                        TD_5 = New TableCell
                                        LB = New Label

                                        TD.Text = "Financial Institution: "
                                        TR.Controls.Add(TD)


                                        TDSELECTCOMPARISON.ID = "COMPARE_FinancialInstitution"
                                        TDSELECTCOMPARISON.Width = Unit.Percentage(100D)
                                        TDSELECTCOMPARISON = DisplayFunctions.Fill_Dropdown("String", TDSELECTCOMPARISON, "1")

                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                            If Page.Request.Form("project_search") = "Y" Then
                                                Dim temp As String = ""
                                                If Not IsNothing(Request.Form("COMPARE_FinancialInstitution")) Then
                                                    temp = Request.Form("COMPARE_FinancialInstitution")
                                                    If Not String.IsNullOrEmpty(temp) Then
                                                        TDSELECTCOMPARISON.SelectedValue = temp
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'fill session
                                        If Not IsNothing(Session.Item("Advanced-" & TDSELECTCOMPARISON.ID)) Then
                                            TDSELECTCOMPARISON.SelectedValue = Session.Item("Advanced-" & TDSELECTCOMPARISON.ID)
                                        End If

                                        'This one is equals, so we're going to go ahead and the select dropdown is inivisible, so the label is shown
                                        LB.Text = TDSELECTCOMPARISON.SelectedItem.Text
                                        LB.CssClass = "lighter_gray_text"

                                        TD_2.Controls.Add(LB)
                                        TD_2.Controls.Add(TDSELECTCOMPARISON)
                                        TDSELECT.ID = "FinancialInstitution"
                                        TDSELECT.Items.Add(New ListItem("", ""))
                                        If Not IsNothing(TemporaryTable) Then
                                            If TemporaryTable.Rows.Count > 0 Then
                                                For Each r As DataRow In TemporaryTable.Rows
                                                    TDSELECT.Items.Add(New ListItem(r("fipg_generic_name"), r("fipg_generic_name")))
                                                Next
                                            End If
                                        End If

                                        LoggingWhereThisStops = "7.25"
                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                            If Page.Request.Form("project_search") = "Y" Then
                                                If Not IsNothing(Request.Form("FinancialInstitution")) Then
                                                    Dim temp As String = Request.Form("FinancialInstitution")
                                                    If Not String.IsNullOrEmpty(temp) Then
                                                        TDSELECT.SelectedValue = temp
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'fill session
                                        If Not IsNothing(Session.Item("Advanced-" & TDSELECT.ID)) Then
                                            TDSELECT.SelectedValue = Session.Item("Advanced-" & TDSELECT.ID)
                                        End If

                                        TDSELECT.ToolTip = "Financial Institution"

                                        TDSELECT.Width = Unit.Percentage(100D)
                                        TDSELECT.ValidationGroup = "String"
                                        TD_3.Controls.Add(TDSELECT)
                                        TR.Controls.Add(TD_2)
                                        TR.Controls.Add(TD_3)
                                        TD_4.Text = "" 'DisplayFormatRules("String")
                                        TD_4.ColumnSpan = 2
                                        TR.Controls.Add(TD_3)
                                        TR.Controls.Add(TD_4)
                                        Tab.Controls.Add(TR)
                                        TempPanel.Controls.Add(Tab)



                                        TR = New TableRow
                                        TR.CssClass = "alt_row"
                                        TD = New TableCell
                                        TD_2 = New TableCell
                                        TD_3 = New TableCell
                                        TD_4 = New TableCell
                                        TDSELECT = New DropDownList
                                        TDTEXT = New TextBox
                                        TDSELECTCOMPARISON = New DropDownList

                                        TD.Text = "Document Date: "
                                        TR.Controls.Add(TD)

                                        TDSELECTCOMPARISON.ID = "COMPARE_adoc_doc_date"
                                        TDSELECTCOMPARISON.Width = Unit.Percentage(100D)
                                        TDSELECTCOMPARISON = DisplayFunctions.Fill_Dropdown("Date", TDSELECTCOMPARISON, "")

                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                            If Page.Request.Form("project_search") = "Y" Then
                                                If Not IsNothing(Request.Form(TDSELECTCOMPARISON.ID)) Then
                                                    Dim temp As String = Request.Form(TDSELECTCOMPARISON.ID)
                                                    If Not String.IsNullOrEmpty(temp) Then
                                                        TDSELECTCOMPARISON.SelectedValue = temp
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'fill session
                                        If Not IsNothing(Session.Item("Advanced-" & TDSELECTCOMPARISON.ID)) Then
                                            DisplayFunctions.SelectInformation(TDSELECTCOMPARISON, Session.Item("Advanced-" & TDSELECTCOMPARISON.ID))
                                        End If

                                        TDSELECTCOMPARISON.Attributes.Add("onChange", "javascript:ClearAssociatedBox($(this).find(':selected').val(),'adoc_doc_date', 'input');")

                                        TD_2.Controls.Add(TDSELECTCOMPARISON)
                                        TR.Controls.Add(TD_2)

                                        TDTEXT.ID = "adoc_doc_date"
                                        TDTEXT.ValidationGroup = "Date"

                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                            If Page.Request.Form("project_search") = "Y" Then
                                                If Not IsNothing(Request.Form(TDTEXT.ID)) Then
                                                    Dim temp As String = Request.Form(TDTEXT.ID)
                                                    If Not String.IsNullOrEmpty(temp) Then
                                                        TDTEXT.Text = temp
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'fill session
                                        If Not IsNothing(Session.Item("Advanced-" & TDTEXT.ID)) Then
                                            DisplayFunctions.SelectInformation(TDTEXT, Session.Item("Advanced-" & TDTEXT.ID))
                                        End If

                                        TDTEXT.ToolTip = "Financial Document Date"
                                        TD_3.Controls.Add(TDTEXT)
                                        TR.Controls.Add(TD_3)
                                        TD_4.Text = DisplayFunctions.DisplayFormatRules("Date")
                                        TD_4.CssClass = "lighter_gray_text"
                                        TR.Controls.Add(TD_4)

                                        CustomValidation.ID = "VALIDATE_FinancialDocumentDate"
                                        CustomValidation.ErrorMessage = "*Incorrect Format"
                                        CustomValidation.Font.Bold = True
                                        CustomValidation.ValidationGroup = "Numeric"
                                        CustomValidation.ControlToValidate = TDTEXT.ID
                                        CustomValidation.SetFocusOnError = True
                                        CustomValidation.ClientValidationFunction = "validateDate"
                                        CustomValidation.Text = "*Incorrect Format"
                                        CustomValidation.Display = ValidatorDisplay.Static
                                        CustomValidation.Enabled = True
                                        TD_5.Controls.Add(CustomValidation)
                                        TR.Controls.Add(TD_5)
                                        Tab.Controls.Add(TR)


                                        TR = New TableRow
                                        TD = New TableCell
                                        TD_2 = New TableCell
                                        TD_3 = New TableCell
                                        TD_4 = New TableCell
                                        TD_5 = New TableCell
                                        LB = New Label

                                        TDSELECTLIST = New ListBox
                                        TDTEXT = New TextBox
                                        TDSELECTCOMPARISON = New DropDownList

                                        TD.Text = "Document Type: "
                                        TR.Controls.Add(TD)

                                        TDSELECTCOMPARISON.ID = "COMPARE_adoc_doc_type"
                                        TDSELECTCOMPARISON.Width = Unit.Percentage(100D)
                                        TDSELECTCOMPARISON = DisplayFunctions.Fill_Dropdown("String", TDSELECTCOMPARISON, "1")

                                        If Not IsNothing(Page.Request.Form("project_search")) Then
                                            If Page.Request.Form("project_search") = "Y" Then
                                                Dim temp As String = ""
                                                If Not IsNothing(Request.Form("COMPARE_adoc_doc_type")) Then
                                                    temp = Request.Form("COMPAREadoc_doc_type")
                                                    If Not String.IsNullOrEmpty(temp) Then
                                                        TDSELECTCOMPARISON.SelectedValue = temp
                                                    End If
                                                End If
                                            End If
                                        End If

                                        If Not IsNothing(Session.Item("Advanced-" & TDSELECTCOMPARISON.ID)) Then
                                            TDSELECTCOMPARISON.SelectedValue = Session.Item("Advanced-" & TDSELECTCOMPARISON.ID)
                                        End If

                                        'This one is equals, so we're going to go ahead and the select dropdown is inivisible, so the label is shown
                                        LB.Text = TDSELECTCOMPARISON.SelectedItem.Text
                                        LB.CssClass = "lighter_gray_text"

                                        TD_2.Controls.Add(LB)
                                        TD_2.Controls.Add(TDSELECTCOMPARISON)
                                        TDSELECTLIST.ID = "adoc_doc_type"
                                        TDSELECTLIST.SelectionMode = ListSelectionMode.Multiple
                                        TDSELECTLIST.Items.Add(New ListItem("", ""))
                                        LoggingWhereThisStops = "7.5"
                                        If Not IsNothing(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
                                            Dim financial_documents_functions As New financial_view_functions
                                            financial_documents_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim


                                            Dim results_table As New DataTable


                                            results_table = financial_documents_functions.get_top_document_types_info(Nothing, True)

                                            If Not IsNothing(results_table) Then
                                                For Each r As DataRow In results_table.Rows
                                                    TDSELECTLIST.Items.Add(New ListItem(r.Item("adoc_doc_type").ToString, r.Item("adoc_doc_type").ToString))
                                                Next
                                            End If

                                            If Not IsNothing(Page.Request.Form("project_search")) Then
                                                If Page.Request.Form("project_search") = "Y" Then
                                                    If Not IsNothing(Request.Form("adoc_doc_type")) Then
                                                        Dim temp As String = Request.Form("adoc_doc_type")
                                                        If Not String.IsNullOrEmpty(temp.Trim) Then
                                                            DisplayFunctions.SelectInformation(TDSELECTLIST, temp)
                                                        End If
                                                    End If
                                                End If
                                            End If


                                            'fill session
                                            If Not IsNothing(Session.Item("Advanced-" + TDSELECTLIST.ID)) Then
                                                DisplayFunctions.SelectInformation(TDSELECTLIST, Session.Item("Advanced-" + TDSELECTLIST.ID))
                                            End If
                                        End If

                                        LoggingWhereThisStops = "7.75"
                                        TDSELECTLIST.ToolTip = "Document Type"

                                        TDSELECTLIST.Width = Unit.Percentage(100D)
                                        TDSELECTLIST.ValidationGroup = "String"
                                        TD_3.Controls.Add(TDSELECTLIST)
                                        TR.Controls.Add(TD_2)
                                        TR.Controls.Add(TD_3)
                                        TD_4.Text = "" 'DisplayFormatRules("String")
                                        TD_4.ColumnSpan = 2
                                        TR.Controls.Add(TD_3)
                                        TR.Controls.Add(TD_4)
                                        Tab.Controls.Add(TR)

                                    End If

                                    LoggingWhereThisStops = "9th"

                                    TempPanel.ID = "TABFINANCIAL"
                                    If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then
                                        TempPanel.HeaderText = "Docs"
                                    Else
                                        TempPanel.HeaderText = "Financial Docs"
                                    End If

                                    ac_advanced_search.Controls.Add(TempPanel)
                                    TempPanel.Visible = True


                                    '  If Not Page.IsPostBack Then
                                    'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                                    '  'Adding the attributes panel.
                                    '  AttrTab.Visible = True

                                    '  'This panel is completely custom, it does not come from the database.
                                    '  'It is a panel that allows the ability to integrate filtering into the aircraft search area.
                                    '  'It will display two topic lists. One organized by alphabet. One organized by area.
                                    '  'The important thing to note is that when this is built, you really need to have it rebuilt after the search.
                                    '  'Your search results could alter the list of topics.
                                    '  'TempPanel = New AjaxControlToolkit.TabPanel
                                    '  'TempPanel.HeaderText = "Attributes"
                                    '  'TempPanel.ID = "AttrTab"
                                    '  'TempPanel.Visible = True

                                    '  Dim TemporaryContainer As New Panel
                                    '  BuildAttributeTextAndDropdown(TemporaryContainer, MainContent.ClientID, ac_advanced_search.ID, AttrTab.ID)
                                    '  AttributesPanel.Controls.Add(TemporaryContainer)

                                    '  TemporaryContainer = New Panel
                                    '  TemporaryContainer.ID = "letter_display"
                                    '  BuildTopicAreaPanel(TemporaryContainer, True, False, "LETTER", "TOPIC", MainContent.ClientID, ac_advanced_search.ID, AttrTab.ID)
                                    '  AttributesPanel.Controls.Add(TemporaryContainer)

                                    '  TemporaryContainer = New Panel
                                    '  TemporaryContainer.ID = "area_display"
                                    '  TemporaryContainer.Attributes.Add("style", "display:none;")
                                    '  BuildTopicAreaPanel(TemporaryContainer, False, True, "AREA", "TOPIC", MainContent.ClientID, ac_advanced_search.ID, AttrTab.ID)
                                    '  AttributesPanel.Controls.Add(TemporaryContainer)
                                    '  AttrTab.Controls.Add(AttributesPanel)
                                    'End If
                                    'End If
                                    If History = False And MarketEvent = False Then
                                        LoggingWhereThisStops = "10th"
                                        AttrTab.Visible = True
                                        If attrBoolRan.Text = "true" Then
                                            DealWithAttributeTab(MainContent.ClientID, AttributesPanel)
                                        End If
                                        LoggingWhereThisStops = "11th"
                                    End If
                                End If
                            End If
                            If Trim(Request("att")) = "true" Then
                                ac_advanced_search.ActiveTabIndex = 11
                            Else
                                ac_advanced_search.ActiveTabIndex = 0
                            End If


                        End If
                    End If

                    TemporaryTable = Nothing

                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Stop Position: " + LoggingWhereThisStops + " " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): Stop Position: " + LoggingWhereThisStops + " " + ex.Message.ToString.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Stop Position: " + LoggingWhereThisStops + " Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub DealWithAttributeTab(ByVal MainContentClientID As String, ByVal ContainingPanel As Panel)
        Try
            AttrTab.Visible = True
            Dim DropDownSelection As Integer = 2
            Dim TempTable As New DataTable
            'This panel is completely custom, it does not come from the database.
            'It is a panel that allows the ability to integrate filtering into the aircraft search area.
            'It will display two topic lists. One organized by alphabet. One organized by area.
            'The important thing to note is that when this is built, you really need to have it rebuilt after the search.
            'Your search results could alter the list of topics.
            'TempPanel = New AjaxControlToolkit.TabPanel
            'TempPanel.HeaderText = "Attributes"
            'TempPanel.ID = "AttrTab"
            'TempPanel.Visible = True

            If aircraft_attention.Text = "" Then
                Dim TemporaryContainer As New Panel
                DropDownSelection = AdvancedQueryResults.BuildAttributeTextAndDropdown(TemporaryContainer, MainContentClientID, ac_advanced_search.ID, AttrTab.ID, Page, True)
                ContainingPanel.Controls.Add(TemporaryContainer)

                TemporaryContainer = New Panel
                TemporaryContainer.ID = "area_display"

                AdvancedQueryResults.BuildTopicAreaPanel(TemporaryContainer, False, True, "AREA", "TOPIC", MainContentClientID, ac_advanced_search.ID, AttrTab.ID, Page, True, masterPage.aclsData_Temp)
                ContainingPanel.Controls.Add(TemporaryContainer)
                AttrTab.Controls.Add(ContainingPanel)
            Else
                Dim newLabel As New Label
                newLabel.ForeColor = Drawing.Color.Red
                newLabel.Font.Bold = True
                newLabel.Text = "<br /><p align=""center"">No attributes were found for this search.</p>"
                AttrTab.Controls.Add(newLabel)
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub FillDropdownsMaintenance(ByRef Drop As DropDownList, ByRef MaintenanceTable As DataTable, ByVal FieldText As String)

        Dim distinct_table_view As New DataView
        Dim distinct_table As New DataTable

        Try

            'create the view to get the distinct values.
            distinct_table_view = MaintenanceTable.DefaultView
            'actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, FieldText)

            If Not IsNothing(distinct_table) Then
                Drop.Items.Clear()
                Drop.Items.Add(New ListItem("", ""))
                If Not IsNothing(distinct_table) Then
                    If distinct_table.Rows.Count > 0 Then
                        For Each r As DataRow In distinct_table.Rows
                            If Not IsDBNull(r(FieldText)) Then
                                Drop.Items.Add(New ListItem(CStr(r(FieldText)), "'" & Replace(CStr(r(FieldText)), "'", "&apos;") & "'"))
                            End If
                        Next
                    End If
                End If
                Drop.SelectedValue = ""
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub FillDropdowns(ByRef Drop As DropDownList, ByRef MaintenanceTable As DataTable, ByVal FieldText As String)

        Try

            If Not IsNothing(MaintenanceTable) Then
                Drop.Items.Clear()
                Drop.Items.Add(New ListItem("", ""))
                If Not IsNothing(MaintenanceTable) Then
                    If MaintenanceTable.Rows.Count > 0 Then
                        For Each r As DataRow In MaintenanceTable.Rows
                            If Not IsDBNull(r(FieldText)) Then
                                Drop.Items.Add(New ListItem(CStr(r(FieldText)), "'" & CStr(r(FieldText) & "'")))
                            End If
                        Next
                    End If
                End If
                Drop.SelectedValue = ""
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function BuildEngineCustom(ByRef Tab As Table, ByVal Engine As Boolean) As Table
        Try
            Dim TR As New TableRow
            Dim TD As New TableCell
            Dim InnerTab As New Table
            Dim InnerTR As New TableRow
            Dim InnerTD As New TableCell
            Dim InnerCompanyTable As New Table
            Dim InnerCompanyTR As New TableRow
            Dim InnerCompanyTD As New TableCell

            Dim InnerContactTable As New Table
            Dim InnerContactTR As New TableRow
            Dim InnerContactTD As New TableCell
            Dim TempText As New TextBox
            Dim TempCheck As New CheckBox
            Dim TempDrop As New DropDownList
            Dim TempTable As New DataTable
            Dim TempLabel As New Label
            Dim TempSelectComparison As New DropDownList
            Dim NumberCustom As New RegularExpressionValidator
            Dim DigitCustom As New CompareValidator

            Dim stringName As String = "Engine" 'this is going to be the type of the field in string, it can swap to propeller so we can use the same block
            'for each one

            InnerTab.CssClass = "override_borders"
            InnerTab.Width = Unit.Percentage(100D)
            InnerContactTable.Width = Unit.Percentage(100D)
            InnerContactTable.CellPadding = 3
            InnerContactTable.CssClass = "override_borders light_blue_background"

            InnerCompanyTable.Width = Unit.Percentage(100D)
            InnerCompanyTable.CellPadding = 3
            InnerCompanyTable.CssClass = "override_borders"

            TR.CssClass = "header_row"
            TD.CssClass = "data_aircraft_grid_cell" ' light_seafoam_green_header_color"
            TD.ColumnSpan = 5

            If Engine Then
                stringName = "Engine"
            Else
                stringName = "Propeller"
            End If


            TD.Text = "<b>" & stringName & "</span></b>"

            TR.Controls.Add(TD)
            Tab.Controls.Add(TR)

            TR = New TableRow

            'Setting up Model
            If Engine Then
                TD = DisplayFunctions.BuildTableCell(False, "<A href='MasterLists.aspx?helplist=engineprefix' target='_blank'>Model</a>:", VerticalAlign.Middle, HorizontalAlign.Left)
            Else
                TD = DisplayFunctions.BuildTableCell(False, "Propeller TTSN:", VerticalAlign.Middle, HorizontalAlign.Left)
            End If

            TR.Controls.Add(TD)

            'Model/TTSN Textbox.
            TD = New TableCell
            TempText = New TextBox
            TempText.TextMode = TextBoxMode.MultiLine
            TempText.Height = Unit.Pixel(12)
            TempText.Rows = 1

            'if this is an engine, it needs a model, otherwise we can ignore this
            If Engine Then
                FillInSetUpBox(TempText, "String", stringName & " Model", "ac_" & LCase(stringName) & "_name_search", TD, TempSelectComparison)
                TD.Controls.AddAt(1, TempText)
            Else
                FillInSetUpBox(TempText, "Numeric", stringName & " Model", LCase(stringName) & "_snew", TD, TempSelectComparison)
                TD.Controls.AddAt(1, TempText)
            End If

            TR.Controls.Add(TD)

            If Engine = False Then 'Validator but only for SNEW 
                TD = New TableCell
                NumberCustom = New RegularExpressionValidator

                NumberCustom.ID = "VALIDATE_" & stringName & "TTSN"
                NumberCustom.ErrorMessage = "*Incorrect Format"
                NumberCustom.Font.Bold = True
                NumberCustom.ValidationGroup = "Numeric"
                NumberCustom.ControlToValidate = TempText.ID
                NumberCustom.SetFocusOnError = True
                NumberCustom.ValidationExpression = "^[\d,:\s\n]+$"
                NumberCustom.Text = "*Incorrect Format"
                NumberCustom.Display = ValidatorDisplay.Static
                NumberCustom.Enabled = True
                TD.Controls.Add(NumberCustom)
                TR.Controls.Add(TD)
                'Else
                '    TD = New TableCell
                '    TR.Controls.Add(TD)
            End If

            'Serial # label
            TD = DisplayFunctions.BuildTableCell(False, stringName & " Serial Number:", VerticalAlign.Middle, HorizontalAlign.Left)
            TR.Controls.Add(TD)

            'Serial # Text.
            TempText = New TextBox
            TempText.CssClass = "float_left"


            TD = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)
            FillInSetUpBox(TempText, "String", stringName & " Serial Number", "ac_" & LCase(stringName) & "_prop_ser_from", TD, TempSelectComparison)
            TD.Controls.AddAt(1, TempText)


            'Serial # Text.
            TempLabel = New Label
            TempLabel.CssClass = "float_left padding_left"
            TempLabel.Text = " ""nnnn"", for Between use ""nnnn: nnnn"""
            TD.Controls.AddAt(2, TempLabel)
            TD.ColumnSpan = 2
            TR.Controls.Add(TD)
            Tab.Controls.Add(TR)


            'Set up 3rd Row

            TR = New TableRow
            If Engine Then
                TD = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Middle, HorizontalAlign.Left)
            Else
                TD = DisplayFunctions.BuildTableCell(False, "Propeller SOH:", VerticalAlign.Middle, HorizontalAlign.Left)
            End If

            TR.Controls.Add(TD)

            'do Not Include checkbox
            TD = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Middle, HorizontalAlign.Left)
            If Engine Then
                TempCheck = New CheckBox
                TempCheck.ID = stringName & "NoOverdue"
                TempCheck.Text = "Do Not Include Overdue"

                If Not IsNothing(Session.Item("Advanced-" & TempCheck.ID)) Then
                    If Not String.IsNullOrEmpty(Session.Item("Advanced-" & TempCheck.ID)) Then
                        TempCheck.Checked = Session.Item("Advanced-" & TempCheck.ID)
                    End If
                End If
                'Fill in the session saved value.
                DisplayFunctions.SelectInformation(TempCheck, Session.Item("Advanced-" & TempCheck.ID))
                TD.Controls.Add(TempCheck)
            Else
                TD = New TableCell
                TempText = New TextBox
                TempText.TextMode = TextBoxMode.MultiLine
                TempText.Height = Unit.Pixel(12)
                TempText.Rows = 1

                FillInSetUpBox(TempText, "Numeric", "Propeller SOH", LCase(stringName) & "_soh", TD, TempSelectComparison)
                DisplayFunctions.SelectInformation(TempText, Session.Item("Advanced-" & TempText.ID))
                TD.Controls.Add(TempText)
            End If



            TR.Controls.Add(TD)

            If Engine Then
                TD = DisplayFunctions.BuildTableCell(False, stringName & " Times Within", VerticalAlign.Top, HorizontalAlign.Left)
                TR.Controls.Add(TD)

                'Hours textbox
                TD = New TableCell
                TempText = New TextBox
                TempText.CssClass = "float_left"
                TempLabel = New Label

                FillInSetUpBox(TempText, "Numeric", stringName & "Times Within", LCase(stringName) & "_ac_hours", TD, TempSelectComparison)
                TD.Controls.AddAt(1, TempText)

                TempLabel = New Label
                TempLabel.CssClass = "float_left padding_left"
                TempLabel.Text = " Hours of Next Overhaul"
                TD.Controls.AddAt(2, TempLabel)

                TR.Controls.Add(TD)
                TD = New TableCell

                DigitCustom = New CompareValidator

                DigitCustom.ID = "VALIDATE_" & stringName & "HoursOfNextOverhaul"
                DigitCustom.ErrorMessage = "*Incorrect Format"
                DigitCustom.Font.Bold = True
                DigitCustom.ValidationGroup = "Numeric"
                DigitCustom.ControlToValidate = TempText.ID
                DigitCustom.SetFocusOnError = True
                DigitCustom.Operator = ValidationCompareOperator.DataTypeCheck
                DigitCustom.Type = ValidationDataType.Double
                DigitCustom.Text = "*Incorrect Format"
                DigitCustom.Display = ValidatorDisplay.Static
                DigitCustom.Enabled = True
                TD.Controls.Add(DigitCustom)
            Else
                TD = New TableCell
                NumberCustom = New RegularExpressionValidator

                NumberCustom.ID = "VALIDATE_" & stringName & "SOH"
                NumberCustom.ErrorMessage = "*Incorrect Format"
                NumberCustom.Font.Bold = True
                NumberCustom.ValidationGroup = "Numeric"
                NumberCustom.ControlToValidate = TempText.ID
                NumberCustom.SetFocusOnError = True
                NumberCustom.ValidationExpression = "^[\d,:\s\n]+$"
                NumberCustom.Text = "*Incorrect Format"
                NumberCustom.Display = ValidatorDisplay.Static
                NumberCustom.Enabled = True
                TD.Controls.Add(NumberCustom)
            End If




            TR.Controls.Add(TD)

            If Engine = False Then
                TD = New TableCell
                TD.ColumnSpan = 2
                TR.Controls.Add(TD)
            End If

            'Add Row
            Tab.Controls.Add(TR)








            '--------------------------------ADDED MSW 8-20-15-----------------------------------
            'do Not Include checkbox
            'TD = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Middle, HorizontalAlign.Left)
            If Engine Then
                TR = New TableRow
                TD = DisplayFunctions.BuildTableCell(False, "SOH Hours:", VerticalAlign.Middle, HorizontalAlign.Left)
                TR.Controls.Add(TD)



                'TD = New TableCell
                'TempDrop = New DropDownList
                'TempDrop.ID = "COMPARE_" & stringName & "_soh_hours"
                'TempDrop.Text = ""
                'TempDrop.Items.Add("")
                'TempDrop.Items.Add("Equals")
                'TempDrop.Items.Add("Less Than")
                'TempDrop.Items.Add("Greater Than")
                'TempDrop.Items.Add("Between")

                ''Fill in the session saved value.
                'DisplayFunctions.SelectInformation(TempDrop, Session.Item("Advanced-" & TempDrop.ID))
                'TD.Controls.Add(TempDrop)
                'TR.Controls.Add(TD)



                TD = New TableCell
                TempText = New TextBox
                TempText.TextMode = TextBoxMode.MultiLine
                TempText.Height = Unit.Pixel(12)
                TempText.Rows = 1
                FillInSetUpBox(TempText, "Numeric", "Engine SOH Hours", "" & LCase(stringName) & "_soh_hours", TD, TempSelectComparison)
                DisplayFunctions.SelectInformation(TempText, Session.Item("Advanced-" & TempText.ID))
                TR.Controls.Add(TD)
                TD = New TableCell
                TD.Controls.Add(TempText)
                TR.Controls.Add(TD)



                TD = New TableCell
                TD = DisplayFunctions.BuildTableCell(False, "'nnnn', for Between use 'nnnn: nnnn'", VerticalAlign.Middle, HorizontalAlign.Left)
                TR.Controls.Add(TD)

                Tab.Controls.Add(TR)
            End If






            If Engine Then
                TR = New TableRow
                TD = DisplayFunctions.BuildTableCell(False, "Since Hot Inspection:", VerticalAlign.Middle, HorizontalAlign.Left)
                TR.Controls.Add(TD)

                'TD = New TableCell
                'TempDrop = New DropDownList
                'TempDrop.ID = "COMPARE_" & stringName & "_shi_hours"
                'TempDrop.Text = ""
                'TempDrop.Items.Add("")
                'TempDrop.Items.Add("Equals")
                'TempDrop.Items.Add("Less Than")
                'TempDrop.Items.Add("Greater Than")
                'TempDrop.Items.Add("Between")
                'TD.Controls.Add(TempDrop)
                'TR.Controls.Add(TD)

                TD = New TableCell
                TempText = New TextBox
                TempText.CssClass = "float_left"
                TempLabel = New Label
                TempText.TextMode = TextBoxMode.MultiLine
                TempText.Height = Unit.Pixel(12)
                TempText.Rows = 1
                FillInSetUpBox(TempText, "Numeric", stringName & " SHI Hours", "" & LCase(stringName) & "_shi_hours", TD, TempSelectComparison)
                TR.Controls.Add(TD)
                TD = New TableCell
                TD.Controls.Add(TempText)
                TR.Controls.Add(TD)


                TD = New TableCell
                TD = DisplayFunctions.BuildTableCell(False, "'nnnn', for Between use 'nnnn: nnnn'", VerticalAlign.Middle, HorizontalAlign.Left)
                TR.Controls.Add(TD)

                Tab.Controls.Add(TR)
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return Tab

    End Function

    Private Sub FillInSetUpBox(ByRef tempText As TextBox, ByVal valgroup As String, ByVal tooltip As String, ByVal idString As String, ByRef td As TableCell, ByRef tempselectcomparison As DropDownList)

        Try

            'Comparison Field
            tempselectcomparison = New DropDownList
            tempselectcomparison.ID = "COMPARE_" & idString
            tempselectcomparison.Width = Unit.Percentage(100D)
            tempselectcomparison = DisplayFunctions.Fill_Dropdown(valgroup, tempselectcomparison, "")


            If Trim(idString) <> "engine_soh_hours" And Trim(idString) <> "engine_shi_hours" Then
                If valgroup = "Numeric" Then
                    If tempselectcomparison.SelectedValue = "" Then
                        'This function is ran by the engine tab, which means that these select boxes won't show up.
                        'So this goes ahead and toggles the display off, if the selected value is nothing, 
                        'and selects the value to be equals.
                        tempselectcomparison.CssClass = "display_none"
                        tempselectcomparison.SelectedValue = "Equals"
                    End If
                End If
            End If

            td.Controls.AddAt(0, tempselectcomparison)

            tempText.ID = idString
            tempText.ValidationGroup = valgroup
            'This tooltip is set as the display on purpose.
            'When we are looping through them, instead of adding yet another pipe in the ID
            'Using the tooltip is an easy way to get the Textual Display of this Field.
            tempText.ToolTip = tooltip

            'Fill in the session saved value.
            If Not IsNothing(Session.Item("Advanced-" & tempText.ID)) Then
                If Not String.IsNullOrEmpty(Session.Item("Advanced-" & tempText.ID)) Then
                    tempText.Text = Session.Item("Advanced-" & tempText.ID)
                End If
            End If
            'Fill in the session saved value.
            DisplayFunctions.SelectInformation(tempText, Session.Item("Advanced-" & tempText.ID))


            'If just on the case of the project search
            'Well, really only if we're doing a summary call back
            'if the session is set, but there's a request variable
            'we override it.
            'On a summary call back - we go ahead and don't clear the session
            'regular projects we do.


            If Page.Request.Form("project_search") = "Y" Then
                If Not IsNothing(Request.Form(tempText.ID)) Then
                    Dim temp As String = Request.Form(tempText.ID)
                    If Not String.IsNullOrEmpty(temp) Then
                        tempText.Text = Request.Form(tempText.ID)
                    End If
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function AircraftListingPageQuery(ByVal WeightClass As String, ByVal ManufacturerName As String,
                                             ByVal AcSize As String, ByVal model_string As String,
                                             ByVal ForSale_Flag As String, ByVal ForLease_Flag As String,
                                             ByVal onExclusive_Flag As String, ByVal SerialNo_Start As String,
                                             ByVal SerialNo_End As String, ByVal DoNotSearchAltSer As Boolean, ByVal RegistrationNo As String,
                                             ByVal RegistrationNo_Exact As Boolean, ByVal DoNotSearchPrevRegNo As Boolean,
                                             ByVal LifeCycleStage As String, ByVal Status As String,
                                             ByVal Ownership As String, ByVal PreviouslyOwned_Flag As String,
                                             ByVal Model_Type As String, ByVal Airframe_Type As String,
                                             ByVal CombinedAirframeTypeString As String, ByVal Make_String As String,
                                             ByVal AC_Status As String, ByVal financialInstitution As String,
                                             ByVal financial_doc_date As String, ByVal financial_doc_type As String,
                                             ByVal Journal_Date As String, ByVal Journal_Type As String,
                                             ByVal Journal_Retail_Only As Boolean, ByVal Journal_New_Aircraft As Boolean,
                                             ByVal Journal_Used_Aircraft As Boolean, ByVal Journal_Subcat_Part2 As String,
                                             ByVal Journal_Subcat_Part2_Operator As String, ByVal Journal_Subcat_Part3 As String,
                                             ByVal Journal_Subcat_Part3_Operator As String, ByVal DynamicStringGeneration As String,
                                             ByVal History As Boolean, ByVal sql_order As String, ByVal CompanyCountry As String,
                                             ByVal CompanyTimeZone As String, ByVal CompanyContinentString As String,
                                             ByVal CompanyRegionString As String, ByVal Business As Boolean,
                                             ByVal Helicopter As Boolean, ByVal Commercial As Boolean,
                                             ByVal BaseCountriesString As String, ByVal BaseContinentString As String,
                                             ByVal BaseRegionString As String, ByVal BaseStateName As String,
                                             ByVal CompanyStateName As String, ByVal journalIDs As String,
                                             ByVal onMarket As Boolean, ByVal offMarket As Boolean,
                                             ByVal writtenOff As Boolean) As DataTable
        Dim sql As String = ""
        'Dim FinancialInstitution As String = "1"
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim aTempTable As New DataTable
        Dim sql_where As String = ""
        Dim sql_company_where As String = ""
        Dim count As Integer = 1
        Dim useAlternate As Boolean = False
        Dim TableName As String = " from View_Aircraft_Flat with (NOLOCK) "
        Dim ac_count As DataColumn = New DataColumn("ac_count", Type.GetType("System.Int64"))
        Dim table_name As String = ""
        Dim AclsData_Temp As New clsData_Manager_SQL
        Dim sql_where_start As String = ""
        Dim sql_where_end As String = ""
        Dim sql_where_value As String = ""
        Dim sql_where_value2 As String = ""
        Dim field_name As String = ""
        Dim temp_string As String = ""
        Dim last_string As String = ""
        Dim temp_size As String = ""
        ac_count.AutoIncrement = True
        ac_count.AutoIncrementSeed = 1
        aTempTable.Columns.Add(ac_count)
        'atemptable.Columns.Add("ac_count", Type.GetType("System.Int64"))


        Dim ac_airframe_tot_col As DataColumn = aTempTable.Columns.Add("ac_airframe_tot_hrs", Type.GetType("System.Int64"))
        ac_airframe_tot_col.AllowDBNull = True

        Dim ac_engine_1_tot_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_1_tot_hrs", Type.GetType("System.Int64"))
        ac_engine_1_tot_hrs.AllowDBNull = True

        Dim ac_engine_2_tot_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_2_tot_hrs", Type.GetType("System.Int64"))
        ac_engine_2_tot_hrs.AllowDBNull = True

        Dim ac_engine_3_tot_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_3_tot_hrs", Type.GetType("System.Int64"))
        ac_engine_3_tot_hrs.AllowDBNull = True

        Dim ac_engine_4_tot_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_4_tot_hrs", Type.GetType("System.Int64"))
        ac_engine_4_tot_hrs.AllowDBNull = True

        Dim ac_engine_1_soh_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_1_soh_hrs", Type.GetType("System.Int64"))
        ac_engine_1_soh_hrs.AllowDBNull = True
        Dim ac_engine_2_soh_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_2_soh_hrs", Type.GetType("System.Int64"))
        ac_engine_2_soh_hrs.AllowDBNull = True
        Dim ac_engine_3_soh_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_3_soh_hrs", Type.GetType("System.Int64"))
        ac_engine_3_soh_hrs.AllowDBNull = True
        Dim ac_engine_4_soh_hrs As DataColumn = aTempTable.Columns.Add("ac_engine_4_soh_hrs", Type.GetType("System.Int64"))
        ac_engine_4_soh_hrs.AllowDBNull = True

        Dim journDate As DataColumn = aTempTable.Columns.Add("journ_date", Type.GetType("System.DateTime"))
        journDate.AllowDBNull = True

        'Upon every search, clear these.

        HttpContext.Current.Session.Item("MasterAircraft") = "" 'Whole Search
        HttpContext.Current.Session.Item("MasterAircraftSelect") = "" 'Select Only
        HttpContext.Current.Session.Item("MasterAircraftFrom") = "" 'From Only
        HttpContext.Current.Session.Item("MasterAircraftWhere") = "" 'Where Only
        HttpContext.Current.Session.Item("MasterAircraftSort") = "" 'Sort Only

        HttpContext.Current.Session.Item("MasterAircraftCompany") = HttpContext.Current.Session.Item("MasterAircraftCompany") 'Company Only

        aTempTable.Columns.Add("ac_id")
        aTempTable.Columns.Add("ac_picture_id")
        aTempTable.Columns.Add("amod_make_name")
        aTempTable.Columns.Add("amod_model_name")
        aTempTable.Columns.Add("amod_id")
        aTempTable.Columns.Add("amod_airframe_type_code")
        aTempTable.Columns.Add("ac_mfr_year")
        aTempTable.Columns.Add("ac_forsale_flag")
        aTempTable.Columns.Add("ac_year")
        aTempTable.Columns.Add("ac_ser_no_full")
        aTempTable.Columns.Add("ac_ser_no_sort")
        aTempTable.Columns.Add("ac_reg_no")
        aTempTable.Columns.Add("ac_flights_id")
        aTempTable.Columns.Add("ac_status")
        aTempTable.Columns.Add("ac_asking")
        aTempTable.Columns.Add("ac_asking_price")
        aTempTable.Columns.Add("ac_delivery")
        aTempTable.Columns.Add("ac_exclusive_flag")
        aTempTable.Columns.Add("ac_lease_flag")
        aTempTable.Columns.Add("ac_last_event")

        Dim ac_list_date As DataColumn = aTempTable.Columns.Add("ac_list_date", Type.GetType("System.DateTime"))
        ac_list_date.AllowDBNull = True

        aTempTable.Columns.Add("ac_aport_iata_code")
        aTempTable.Columns.Add("ac_aport_icao_code")
        aTempTable.Columns.Add("ac_reg_no_search")
        aTempTable.Columns.Add("ac_times_as_of_date")
        aTempTable.Columns.Add("ac_last_aerodex_event")
        aTempTable.Columns.Add("aport_longitude_decimal")
        aTempTable.Columns.Add("aport_latitude_decimal")
        aTempTable.Columns.Add("journ_subject")
        aTempTable.Columns.Add("journ_id")
        aTempTable.Columns.Add("jcat_subcategory_name")
        aTempTable.Columns.Add("journ_subcat_code_part1")
        aTempTable.Columns.Add("ac_aport_country")
        aTempTable.Columns.Add("ac_aport_state")

        Try
            ' If model_string <> "" Or Make_String <> "" Then
            sql = "select distinct ac_id, amod_airframe_type_code, amod_type_code,ac_est_airframe_hrs, ac_last_aerodex_event, ac_picture_id,ac_aport_icao_code,ac_aport_iata_code,aport_latitude_decimal,aport_longitude_decimal, ac_list_date, amod_make_name, amod_model_name,amod_id,  "
            sql = sql & " ac_mfr_year, ac_forsale_flag, ac_year, ac_ser_no_full,ac_ser_no_sort, ac_reg_no, ac_times_as_of_date,"
            sql = sql & " ac_flights_id, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, "
            sql = sql & " ac_status, ac_asking, ac_asking_price, ac_delivery,ac_reg_no_search, "
            sql = sql & " ac_exclusive_flag, ac_lease_flag, ac_engine_1_soh_hrs, ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, "
            sql = sql & " ac_last_event "

            If displayEvalues Then
                sql = sql & ", (select afmv_value from ReturnAssetInsighteValue(ac_id)) as AVGEvalue "
            Else
                sql = sql & ", '0' as AVGEvalue "
            End If


            If Session.Item("isMobile") = True Then
                sql = sql & " , ac_aport_country, ac_aport_state "
            End If
            If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True And History = True Then
                sql = sql & ", ac_sale_price_display_flag  "   ' 
                sql = sql & AclsData_Temp.add_in_case_for_ac_sale_price()
            Else
                sql = sql & ", NULL as ac_sale_price, 'N' as ac_sale_price_display_flag "    ' 
            End If

            If History Then
                sql = sql & " , journ_date, journ_subject, journ_id, jcat_subcategory_name, journ_customer_note, journ_subcat_code_part1  "
            End If

            HttpContext.Current.Session.Item("MasterAircraftSelect") = sql
            'This value is locked in.

            If CompanyStateName <> "" Or CompanyContinentString <> "" Or CompanyCountry <> "" Or CompanyTimeZone <> "" Or CompanyRegionString <> "" Or InStr(DynamicStringGeneration, "actype_") > 0 Or InStr(DynamicStringGeneration, "comp_") > 0 Or InStr(DynamicStringGeneration, "contact_") > 0 Or InStr(DynamicStringGeneration, "cref") > 0 Or InStr(DynamicStringGeneration, "non_exclusive") > 0 Then
                useAlternate = True
            End If


            If History Then
                If useAlternate = True Then
                    TableName = " from View_Aircraft_Company_History_Flat with (NOLOCK) "
                Else
                    TableName = " from View_Aircraft_History_Flat with (NOLOCK) "
                End If
            Else
                If useAlternate = True Then
                    TableName = " from View_Aircraft_Company_Flat with (NOLOCK) "
                Else
                    TableName = " from View_Aircraft_Flat with (NOLOCK) "
                End If
            End If

            sql = sql & TableName
            HttpContext.Current.Session.Item("MasterAircraftFrom") = TableName

            sql = sql & " where "

            If History Then

                If onMarket = True Then
                    sql_where = " journ_subcat_code_part1 not in ('OM','MS') "
                    sql_where += " and journ_subcat_code_part1 = 'MA' "
                ElseIf offMarket = True Then
                    sql_where = " journ_subcat_code_part1 not in ('MA','MS') "
                    sql_where += " and journ_subcat_code_part1 = 'OM' "
                ElseIf writtenOff = True Then
                    sql_where = " journ_subcat_code_part1 not in ('OM','MA','MS') "
                    sql_where += " and journ_subcat_code_part1 = 'WO' "
                Else
                    sql_where = " journ_subcat_code_part1 not in ('OM','MA','MS') "
                End If

            Else
                '  sql_where = " ac_journ_id = 0 "
            End If


            If Not String.IsNullOrEmpty(financialInstitution.Trim) Then

                If sql_where <> "" Then
                    sql_where += " AND "
                End If

                sql_where += " ( "
                sql_where += " EXISTS (SELECT NULL FROM "
                sql_where += " Aircraft_Document "
                sql_where += " WHERE (adoc_infavor_comp_id IN "
                sql_where += "(" + financialInstitution.Trim + ")) "
                sql_where += " AND (adoc_ac_id = ac_id) "

                'If this isn't a history search
                If Not History Then
                    sql_where += " AND (EXISTS (SELECT NULL FROM Aircraft_Reference "
                    sql_where += " WHERE (cref_comp_id = adoc_infavor_comp_id) "
                    sql_where += " AND (cref_ac_id = ac_id) AND (cref_journ_id = 0) "
                    sql_where += " AND (cref_contact_type in ('00','08','78','97'))))"
                Else 'otherwise it is a history search, so we need to add this.
                    sql_where += " AND (adoc_journ_id = ac_journ_id)"
                End If

                If Not String.IsNullOrEmpty(financial_doc_date.Trim) Then
                    sql_where += " AND (" + financial_doc_date.Trim + ")"
                End If

                If Not String.IsNullOrEmpty(financial_doc_type.Trim) Then
                    sql_where += " AND (" + financial_doc_type.Trim + ")"
                End If

                sql_where += " ))"

            ElseIf Not String.IsNullOrEmpty(financial_doc_date.Trim) Then

                If sql_where <> "" Then
                    sql_where += " AND "
                End If

                sql_where += " (EXISTS (SELECT NULL FROM Aircraft_Document WHERE"
                'If this isn't a history search
                If Not History Then
                    sql_where += " (adoc_ac_id = ac_id)"
                Else
                    sql_where += " (adoc_ac_id = ac_id AND adoc_journ_id = ac_journ_id)"
                End If

                sql_where += " AND (" + financial_doc_date.Trim + ")"

                If Not String.IsNullOrEmpty(financial_doc_type.Trim) Then
                    sql_where += " AND (" + financial_doc_type.Trim + ")"
                End If

                sql_where += " ))"

            ElseIf Not String.IsNullOrEmpty(financial_doc_type.Trim) Then

                If sql_where <> "" Then
                    sql_where += " AND "
                End If


                sql_where += " (EXISTS (SELECT NULL FROM Aircraft_Document WHERE"
                'If this isn't a history search
                If Not History Then
                    sql_where += " (adoc_ac_id = ac_id)"
                Else 'otherwise it is.
                    sql_where += " (adoc_ac_id = ac_id AND adoc_journ_id = ac_journ_id)"
                End If

                sql_where += " AND (" + financial_doc_type.Trim + ")"

                sql_where += " ))"

            End If

            If AC_Status <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " (" & AC_Status & ") "
            End If

            If Not String.IsNullOrEmpty(WeightClass.Trim) Then
                If Not String.IsNullOrEmpty(sql_where.Trim) Then
                    sql_where += " and "
                End If

                If WeightClass.Contains(Constants.cValueSeperator) Then
                    sql_where += " (amod_weight_class IN ('" + WeightClass.Trim + "')) "
                Else
                    sql_where += " (amod_weight_class = '" + WeightClass.Trim + "') "
                End If

            End If

            If Not String.IsNullOrEmpty(ManufacturerName.Trim) Then
                If Trim(ManufacturerName.Trim) <> "All" Then   ' added MSW - 12/4/2018
                    If Not String.IsNullOrEmpty(sql_where.Trim) Then
                        sql_where += " and "
                    End If

                    If ManufacturerName.Contains(Constants.cValueSeperator) Then
                        sql_where += " (amod_manufacturer_common_name IN ('" + ManufacturerName.Trim + "')) "
                    Else
                        sql_where += " (amod_manufacturer_common_name = '" + ManufacturerName.Trim + "') "
                    End If
                End If
            End If


            If Commercial = True And Business = True Then

                If Not String.IsNullOrEmpty(AcSize.Trim) Then
                    If Not String.IsNullOrEmpty(sql_where.Trim) Then
                        sql_where += " and "
                    End If
                    If AcSize.Contains(Constants.cValueSeperator) Then

                        If (AcSize.Contains("ALJ'") Or AcSize.Contains("ALTP'")) Then
                            temp_size = AcSize.Trim
                            temp_size = "'" & temp_size & "'"

                            sql_where += " ( "

                            If InStr(temp_size, "'ALJ'") > 0 Then
                                sql_where += " (amod_jniq_size = 'ALJ' and ac_product_business_flag = 'Y') "
                                temp_size = Replace(temp_size, "'ALJ'", "")
                                If Trim(temp_size) <> "" Then
                                    sql_where += "  or "
                                End If
                                If Left(Trim(temp_size), 1) = "," Then
                                    temp_size = Right(Trim(temp_size), Len(Trim(temp_size)) - 1)
                                End If
                                If Right(Trim(temp_size), 1) = "," Then
                                    temp_size = Left(Trim(temp_size), Len(Trim(temp_size)) - 1)
                                End If
                                temp_size = Replace(temp_size, ",,", ",")
                            End If

                            If InStr(temp_size, "'ALTP'") > 0 Then
                                sql_where += " (amod_jniq_size = 'ALTP' and ac_product_business_flag = 'Y') "
                                temp_size = Replace(temp_size, "'ALTP'", "")
                                If Trim(temp_size) <> "" Then
                                    sql_where += "  or "
                                End If
                                If Left(Trim(temp_size), 1) = "," Then
                                    temp_size = Right(Trim(temp_size), Len(Trim(temp_size)) - 1)
                                End If
                                If Right(Trim(temp_size), 1) = "," Then
                                    temp_size = Left(Trim(temp_size), Len(Trim(temp_size)) - 1)
                                End If
                                temp_size = Replace(temp_size, ",,", ",")
                            End If

                            If Trim(temp_size) <> "" Then
                                sql_where += " amod_jniq_size IN (" & temp_size & ") "
                            End If

                            sql_where += " ) "
                        Else
                            sql_where += " amod_jniq_size IN ('" & AcSize.Trim & "') "
                        End If

                    Else
                        If Trim(AcSize.Trim) = "ALJ" Or Trim(AcSize.Trim) = "ALTP" Then
                            sql_where += " amod_jniq_size = '" + AcSize.Trim + "' and ac_product_business_flag = 'Y' "
                        Else
                            sql_where += " amod_jniq_size = '" + AcSize.Trim + "' "
                        End If

                    End If

                End If
            Else
                If Not String.IsNullOrEmpty(AcSize.Trim) Then
                    If Not String.IsNullOrEmpty(sql_where.Trim) Then
                        sql_where += " and "
                    End If

                    If AcSize.Contains(Constants.cValueSeperator) Then
                        sql_where += " (amod_jniq_size IN ('" + AcSize.Trim + "')) "
                    Else
                        sql_where += " (amod_jniq_size = '" + AcSize.Trim + "') "
                    End If

                End If
            End If

            If model_string <> "" Then
                If Not String.IsNullOrEmpty(sql_where.Trim) Then
                    sql_where += " and "
                End If
                sql_where += " amod_id in (" & model_string & ") "
            Else
                'If Model_Type <> "" Then
                '    If sql_where <> "" Then
                '        sql_where += " and "
                '    End If
                '    sql_where += " amod_type_code in (" & Model_Type & ")"
                'End If

                'If Airframe_Type <> "" Then
                '    If sql_where <> "" Then
                '        sql_where += " and "
                '    End If
                '    sql_where += " amod_airframe_type_code in (" & Airframe_Type & ")"
                'End If
                If CombinedAirframeTypeString <> "" Then
                    Dim TemporaryAirframeWhere As String = ""
                    'The structure looks like this:
                    'AirType|AirframeType, 
                    'First let's add the and if we need it:

                    Dim BrokenApartTypes As Array = Split(CombinedAirframeTypeString, ",")
                    ' If UBound(BrokenApartTypes) > 0 Then
                    For MultipleSelectionCount = 0 To UBound(BrokenApartTypes)
                        Dim FinalSeperationType As Array = Split(BrokenApartTypes(MultipleSelectionCount), "|")
                        If UBound(FinalSeperationType) = 1 Then
                            'This means there's a type, airframe type
                            If TemporaryAirframeWhere <> "" Then
                                TemporaryAirframeWhere += " or "
                            End If
                            TemporaryAirframeWhere += " (amod_type_code in ('" & Trim(FinalSeperationType(0)) & "') and amod_airframe_type_code in ('" & Trim(FinalSeperationType(1)) & "')) "
                        End If
                    Next
                    'End If

                    If TemporaryAirframeWhere <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        TemporaryAirframeWhere = " ( " & TemporaryAirframeWhere & " ) "
                        sql_where += TemporaryAirframeWhere
                    End If

                End If

                If Make_String <> "" Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " amod_make_name in (" & Make_String & ")"
                End If
            End If

            If ForSale_Flag <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ac_forsale_flag = '" & ForSale_Flag & "' "
            End If
            If ForLease_Flag <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ac_lease_flag = '" & ForLease_Flag & "' "
            End If

            'SER NO RANGE
            'ac_ser_no_value BETWEEN 27 AND 33) OR (ac_alt_ser_no_value BETWEEN 27 AND 33

            'SER NO SINGLE
            '((ac_ser_no_full = '27') OR (ac_ser_no = '27') OR (ac_ser_no_value = 27) OR (ac_alt_ser_no_full = '27') OR (ac_alt_ser_no = '27') OR (ac_alt_ser_no_value = 27))

            Dim sHoldSerial As String = ""
            Dim nloop As Integer = 0
            Dim serNbrArray() As String = Nothing
            Dim sArrayItem As String = ""
            Dim nArrayItem As String = ""

            If Not String.IsNullOrEmpty(SerialNo_Start.Trim) And Not String.IsNullOrEmpty(SerialNo_End.Trim) Then

                If Not String.IsNullOrEmpty(sql_where.Trim) Then
                    sql_where += " AND "
                End If

                If IsNumeric(SerialNo_Start) And IsNumeric(SerialNo_End) Then

                    sql_where += "( (ac_ser_no_value BETWEEN " + SerialNo_Start + " AND " + SerialNo_End + ")"

                    If DoNotSearchAltSer Then
                        sql_where += ")"
                    Else
                        sql_where += " OR (ac_alt_ser_no_value BETWEEN " + SerialNo_Start + " AND " + SerialNo_End + ") )"
                    End If

                Else

                    sql_where += "( ac_ser_no_full BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End
                    sql_where += "' OR ac_ser_no BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End + "'"

                    If DoNotSearchAltSer Then
                        sql_where += ")"
                    Else
                        sql_where += " OR ac_alt_ser_no_full BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End
                        sql_where += "' OR ac_alt_ser_no BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End + "')"
                    End If

                End If ' IsNumeric(nSerialFrom) And IsNumeric(nSerialTo)

            ElseIf Not String.IsNullOrEmpty(SerialNo_Start.Trim) Then
                sHoldSerial = SerialNo_Start.Trim
            ElseIf Not String.IsNullOrEmpty(SerialNo_End.Trim) Then
                sHoldSerial = SerialNo_End.Trim
            End If

            If Not String.IsNullOrEmpty(sHoldSerial.Trim) Then ' Only Valid if a Single text box was filled in. Start Or End

                sHoldSerial = sHoldSerial.Replace(", ", ",") ' remove any spaces after (comma)

                sHoldSerial = "'" + sHoldSerial.Replace(",", "','") + "'"

                serNbrArray = sHoldSerial.Split(",")

                If Not String.IsNullOrEmpty(sql_where.Trim) Then
                    sql_where += " AND "
                End If

                sql_where += "("

                For nloop = 0 To UBound(serNbrArray)
                    If Not String.IsNullOrEmpty(serNbrArray(nloop)) Then
                        sArrayItem = serNbrArray(nloop).Trim
                        nArrayItem = sArrayItem.Replace("'", "").Trim ' Strip off any single quotes for numeric test

                        If IsNumeric(nArrayItem) And Not sArrayItem.Contains("-") Then ' if this array item is a number

                            sql_where += "ac_ser_no_full = " + sArrayItem
                            sql_where += " OR ac_ser_no = " + sArrayItem
                            sql_where += " OR ac_ser_no_value = " + nArrayItem

                            If Not DoNotSearchAltSer Then
                                sql_where += " OR ac_alt_ser_no_full = " + sArrayItem
                                sql_where += " OR ac_alt_ser_no = " + sArrayItem
                                sql_where += " OR ac_alt_ser_no_value = " + nArrayItem
                            End If

                        Else

                            sql_where += "ac_ser_no_full = " + sArrayItem
                            sql_where += " OR ac_ser_no = " + sArrayItem

                            If Not DoNotSearchAltSer Then
                                sql_where += " OR ac_alt_ser_no_full = " + sArrayItem
                                sql_where += " OR ac_alt_ser_no = " + sArrayItem
                            End If

                        End If

                        If UBound(serNbrArray) >= 1 And nloop < UBound(serNbrArray) Then
                            sql_where += " OR " ' add or clauses for each item
                        End If

                    End If

                Next ' nLoop

                sql_where += ")"

            End If

            'REG NO (not exact)
            '((ac_reg_no LIKE 'N415%') OR (ac_prev_reg_no LIKE 'N415%'))

            'REG NO (exact)
            '((ac_reg_no = 'N415CT') OR (ac_prev_reg_no = 'N415CT'))


            If RegistrationNo <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If

                sql_where += "( "

                If RegistrationNo_Exact = True Then
                    sql_where += " ac_reg_no_search = '" & Replace(RegistrationNo, "-", "") & "' "
                Else
                    sql_where += " ac_reg_no_search like '" & Replace(RegistrationNo, "-", "") & "%' "
                End If


                If DoNotSearchPrevRegNo = False Then
                    If RegistrationNo_Exact = True Then
                        sql_where += " or ac_prev_reg_no = '" & RegistrationNo & "' "
                    Else
                        sql_where += " or ac_prev_reg_no like '" & RegistrationNo & "%' "
                    End If
                End If
                sql_where += " )"
            End If

            If onExclusive_Flag <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ac_exclusive_flag = '" & onExclusive_Flag & "' "
            End If
            If PreviouslyOwned_Flag <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ac_previously_owned_flag = '" & PreviouslyOwned_Flag & "' "
            End If
            If Ownership <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ac_ownership_type in (" & Ownership & ") "
            End If

            'History Only Fields 
            If History Then
                If journalIDs <> "" Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " journ_id in (" & journalIDs & ")"
                End If

                If Journal_Date <> "" Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " journ_date " & Journal_Date & ""
                End If

                If Journal_Type <> "" Then
                    Dim specialWithdrawnString As String = ""
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    'This bit of code is setting up the history type to be able to multiple select.
                    'It takes the journal type array and splits it based on commas and then
                    'it goes ahead and builds a temporary hold variable (so we can append commas as needed by checking length) 
                    'and then inserts it in an in clause for the journ subcat part 1 field.
                    Dim TemporaryHolding As String = ""
                    Dim TemporaryJournalTypeArray As Array = Split(Journal_Type, ",")
                    For MultipleSelectionCount = 0 To UBound(TemporaryJournalTypeArray)
                        If TemporaryHolding <> "" Then
                            If TemporaryJournalTypeArray(MultipleSelectionCount) <> "WITHDRAWN FROM USE" Then
                                If TemporaryJournalTypeArray(MultipleSelectionCount) <> "WITHDRAWN FROM USE-STORED" Then
                                    TemporaryHolding += ","
                                End If
                            End If
                        End If
                        Select Case TemporaryJournalTypeArray(MultipleSelectionCount)
                            Case "ALLR"
                                TemporaryHolding += "'WS', 'SS', 'LA', 'LO', 'LT'"
                            Case "ALL SALES"
                                TemporaryHolding += "'WS','SS','FS'"
                            Case "LEASES"
                                TemporaryHolding += "'LA','LO','LT'"
                            Case "DELIVERY POSITION"
                                TemporaryHolding += "'DP'"
                            Case "FORECLOSURES"
                                TemporaryHolding += "'FC'"
                            Case "SEIZURES"
                                TemporaryHolding += "'SZ'"
                            Case "WRITTEN OFF"
                                TemporaryHolding += "'WO'"
                            Case "WITHDRAWN FROM USE"
                                If specialWithdrawnString <> "" Then
                                    specialWithdrawnString += " or "
                                End If
                                specialWithdrawnString += "(journ_subcat_code_part1 = 'WF' AND journ_subject LIKE '%Withdrawn from Use%') "
                            Case "WITHDRAWN FROM USE-STORED"

                                If specialWithdrawnString <> "" Then
                                    specialWithdrawnString += " or "
                                End If

                                specialWithdrawnString += "(journ_subcat_code_part1 = 'WF' AND journ_subject LIKE '%Withdrawn from Use%Stored%') "
                            Case "WHOLE"
                                TemporaryHolding += "'WS'"
                            Case "SHARE"
                                TemporaryHolding += "'SS'"
                            Case "FRACTIONAL"
                                TemporaryHolding += "'FS'"
                        End Select
                    Next
                    sql_where += "("
                    If TemporaryHolding <> "" Then
                        sql_where += " (journ_subcat_code_part1 IN (" & TemporaryHolding & "))"
                    End If

                    If specialWithdrawnString <> "" Then
                        If TemporaryHolding <> "" Then
                            sql_where += " or "
                        End If
                        sql_where += specialWithdrawnString
                    End If

                    sql_where += ")"
                End If

                If Journal_Retail_Only = True Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If

                    sql_where += " NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM'))"
                Else
                    'These options will only show up if retail only is false.
                    '•	The second 2 [3-4] characters of the subcategory_code match the journ_subcat_code_part2, represents the “from” with 2 chars as in “DB” for dealer broker
                    If Journal_Subcat_Part2 <> "" And Journal_Subcat_Part2_Operator <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        Select Case UCase(Journal_Subcat_Part2_Operator)
                            Case "NOT FROM"
                                sql_where += " journ_subcat_code_part2 NOT IN (" & Journal_Subcat_Part2 & ") "
                            Case Else
                                sql_where += " journ_subcat_code_part2 IN (" & Journal_Subcat_Part2 & ") "
                        End Select
                    End If
                    '•	The positions 5-6 characters of the subcategory_code match the journ_subcat_code_part3, represents to the “to” with 2 chars as in “EU” for end user
                    If Journal_Subcat_Part3 <> "" And Journal_Subcat_Part3_Operator <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        Select Case UCase(Journal_Subcat_Part3_Operator)
                            Case "NOT TO"
                                sql_where += " journ_subcat_code_part3 NOT IN (" & Journal_Subcat_Part3 & ") "
                            Case Else
                                sql_where += " journ_subcat_code_part3 IN (" & Journal_Subcat_Part3 & ") "
                        End Select
                    End If
                End If

                If Journal_New_Aircraft = True Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " journ_newac_flag = 'Y' "
                    If journ_exclude_internal_transactions.Checked = True Then
                        sql_where += " AND journ_internal_trans_flag = 'N' "
                    End If
                ElseIf Journal_Used_Aircraft = True Then


                    If InStr(sql, "View_Aircraft_History_Flat") > 0 Then
                        table_name = "View_Aircraft_History_Flat"
                    ElseIf InStr(sql, "View_Aircraft_Company_History_Flat") > 0 Then
                        table_name = "View_Aircraft_Company_History_Flat"
                    Else
                        table_name = "View_Aircraft_History_Flat"
                    End If


                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " journ_newac_flag = 'N' AND journ_internal_trans_flag = 'N' "

                    sql_where = sql_where & " and ( "

                    sql_where = sql_where & " (journ_date > "
                    sql_where = sql_where & " (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) "
                    sql_where = sql_where & " Where af2.ac_id = " & Trim(table_name) & ".ac_id "
                    sql_where = sql_where & " and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') "
                    sql_where = sql_where & " order by af2.journ_date asc))"

                    sql_where = sql_where & " or  "

                    sql_where = sql_where & " (journ_date = "
                    sql_where = sql_where & " (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) "
                    sql_where = sql_where & " Where af2.ac_id = " & Trim(table_name) & ".ac_id and " & Trim(table_name) & ".ac_journ_id  >  af2.ac_journ_id "
                    sql_where = sql_where & " and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') "
                    sql_where = sql_where & " order by af2.journ_date asc))"

                    sql_where = sql_where & " ) "
                ElseIf journ_exclude_internal_transactions.Checked = True Then
                    If sql_where <> "" Then
                        sql_where += " and "
                    End If
                    sql_where += " journ_internal_trans_flag = 'N' "
                End If


            End If




            If CompanyTimeZone <> "" Then
                If sql_company_where <> "" Then
                    sql_company_where += " and "
                End If

                sql_company_where += " comp_timezone in (SELECT tzone_name FROM Timezone where tzone_id in (" & CompanyTimeZone & ")) "
            End If

            'Continent
            If CompanyContinentString <> "" Then
                If sql_company_where <> "" Then
                    sql_company_where += " AND"
                End If
                CompanyContinentString = Replace(CompanyContinentString, "Australia &amp; Oceania", "Australia & Oceania") ' added in MSW to fix 2/19/15

                sql_company_where += " country_continent_name in (" & CompanyContinentString & ") "
            End If
            'Base Continent
            If BaseContinentString <> "" Then
                If sql_where <> "" Then
                    sql_where = sql_where & " AND"
                End If
                sql_where = sql_where & " ac_country_continent_name in (" & BaseContinentString & ") "
            End If

            ' check the state
            If CompanyStateName <> "" Then
                If sql_company_where <> "" Then
                    sql_company_where += " AND "
                End If
                sql_company_where += " state_name IN (" & CompanyStateName & ")"
            End If

            'base state
            If BaseStateName <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If

                sql_where += " ac_aport_state_name in (" & BaseStateName & ") "
            End If


            ' check the country
            If CompanyCountry <> "" And sql_company_where <> "" Then
                sql_company_where += " AND comp_country in (" & CompanyCountry & ") "
            ElseIf CompanyCountry <> "" Then
                sql_company_where += " comp_country in (" & CompanyCountry & ") "
            End If

            'Base Countries
            If BaseCountriesString <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where = sql_where & " ac_aport_country in (" & BaseCountriesString & ") "
            End If


            'regions
            If CompanyRegionString <> "" Then
                If sql_company_where <> "" Then
                    sql_company_where += " AND "
                End If
                sql_company_where += " comp_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & CompanyRegionString & ")) "

                If CompanyStateName <> "" Then
                    sql_company_where += " and state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & CompanyRegionString & ")) "
                End If
            End If

            'base regions
            If BaseRegionString <> "" Then
                If sql_where <> "" Then
                    sql_where = sql_where & " AND "
                End If
                sql_where = sql_where & " ac_aport_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & BaseRegionString & ")) "

                If BaseStateName <> "" Then
                    sql_where = sql_where & " and ac_aport_state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & BaseRegionString & ")) "
                End If
            End If


            If LifeCycleStage <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += " ( " & LifeCycleStage & " ) " '" ac_lifecycle_stage in (" & LifeCycleStage & ")"
                '    HttpContext.Current.Session.Item("SearchString") += " LIFECYCLE STAGE IN (" & LifeCycleStage & ")<br />"
            End If

            If DynamicStringGeneration <> "" Then
                If sql_where <> "" Then
                    sql_where += " and "
                End If
                sql_where += DynamicStringGeneration
                '  HttpContext.Current.Session.Item("SearchString") += DynamicStringGeneration & "<br />"
            End If

            field_name = "lease_start_date"
            If InStr(sql_where, field_name) > 0 Then

                sql_where_start = Left(Trim(sql_where), InStr(Trim(sql_where), field_name) - 1)

                sql_where_end = Right(Trim(sql_where), Len(Trim(sql_where)) - InStr(Trim(sql_where), field_name) + 1)
                ' this will get me rest

                sql_where_end = Replace(sql_where_end, field_name, "")

                If InStr(Left(Trim(sql_where_end), 10), "between") > 0 Then
                    'FIRST GET RID OF THE WORD BETWEEN,
                    temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'") - 1)
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(temp_string)))
                    ' THEN START OVER BUILDING THE TEMP STRING
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    temp_string = Replace(temp_string, "between", " >= ")
                    temp_string = Replace(temp_string, "and", " and journ_date <= ")
                    sql_where_value = temp_string
                Else ' take to the first two tick marks 
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    last_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), "'"))
                    temp_string &= last_string
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - Len(Trim(last_string)))
                    sql_where_value = temp_string
                End If


                'sql_where_value = ""


                sql_where = sql_where_start
                sql_where = sql_where & " exists (select journ_date from Journal with (NOLOCK) "
                sql_where = sql_where & " inner join Journal_Category with (NOLOCK) on jcat_subcategory_code  = journ_subcategory_code "
                sql_where = sql_where & " where journ_ac_id = ac_id "
                sql_where = sql_where & " and jcat_subcategory_transtype in ('Lease','Lease Internal', 'Helo Lease', 'Commercial A/C Lease')"
                sql_where = sql_where & " and journ_date "
                sql_where = sql_where & sql_where_value & ") " & sql_where_end
                If History = True Then
                    sql_where = sql_where & " and jcat_subcategory_transtype in ('Lease','Lease Internal', 'Helo Lease', 'Commercial A/C Lease') "
                    sql_where = sql_where & " and journ_date " & sql_where_value
                Else
                    sql_where = sql_where & " and ac_lease_flag = 'Y' "
                End If


            End If



            field_name = "non_exclusive"
            If InStr(sql_where, field_name) > 0 Then

                ' only use companies where it is the owner
                sql_where_value = " and cref_contact_type in ('00','97','08','78', '86','17') "
                sql_where_value &= " and cref_transmit_seq_no = 1 "
                sql_where_value &= " and ac_exclusive_flag = 'N' " ' and the current ac is not on exclusive

                If InStr(sql_where, "and non_exclusive = 'Y'") > 0 Then
                    sql_where_value &= " and not exists("
                ElseIf InStr(sql_where, "and non_exclusive = 'N'") > 0 Then
                    sql_where_value &= " and exists("
                ElseIf InStr(sql_where, "and non_exclusive = 'Y'") > 0 Then
                    sql_where_value &= " and not exists("
                ElseIf InStr(sql_where, "and non_exclusive = 'N'") > 0 Then
                    sql_where_value &= " and exists("
                End If

                sql_where_value &= " select cref_comp_id from aircraft_reference with (NOLOCK) "
                sql_where_value &= " inner join aircraft a2 with (NOLOCK) on cref_ac_id = a2.ac_id and a2.ac_journ_id = 0 "
                sql_where_value &= " where cref_comp_id = comp_id "
                sql_where_value &= " and cref_ac_id <> View_Aircraft_Company_Flat.ac_id and cref_journ_id = 0  "
                sql_where_value &= " and ac_exclusive_flag = 'Y'"
                sql_where_value &= " and cref_contact_type in ('00','97','08','78', '86','17') "
                sql_where_value &= " and cref_transmit_seq_no = 1 "
                sql_where_value &= " )"

                ' added extra space replace in since sometimes there is an extra space in for some reason and it is easier to just correct here 
                sql_where = Replace(sql_where, "and non_exclusive = 'Y'", sql_where_value)
                sql_where = Replace(sql_where, "and non_exclusive = 'N'", sql_where_value)
                sql_where = Replace(sql_where, "and non_exclusive = 'Y'", sql_where_value)
                sql_where = Replace(sql_where, "and non_exclusive = 'N'", sql_where_value)
            End If



            '------- CLIENT CUSTOM SEARCH ------------
            Dim query_where As String = ""
            If custom_pref_text1.Visible = True Then
                If Not IsNothing(HttpContext.Current.Session.Item("jetnetServerNotesDatabase")) Then
                    AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")


                    If Trim(custom_pref_text1.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & " ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(1, Trim(custom_pref_text1.Text)) & ") "
                    End If

                    If Trim(custom_pref_text2.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(2, Trim(custom_pref_text2.Text)) & ") "
                    End If

                    If Trim(custom_pref_text3.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(3, Trim(custom_pref_text3.Text)) & ") "
                    End If

                    If Trim(custom_pref_text4.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(4, Trim(custom_pref_text4.Text)) & ") "
                    End If

                    If Trim(custom_pref_text5.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(5, Trim(custom_pref_text5.Text)) & ") "
                    End If

                    If Trim(custom_pref_text6.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(6, Trim(custom_pref_text6.Text)) & ") "
                    End If

                    If Trim(custom_pref_text7.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & " and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(7, Trim(custom_pref_text7.Text)) & ") "
                    End If

                    If Trim(custom_pref_text8.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(8, Trim(custom_pref_text8.Text)) & ") "
                    End If

                    If Trim(custom_pref_text9.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(9, Trim(custom_pref_text9.Text)) & ") "
                    End If

                    If Trim(custom_pref_text10.Text) <> "" Then
                        If sql_where <> "" Then
                            sql_where += " and "
                        End If
                        sql_where = sql_where & "  and ac_id in (" & AclsData_Temp.Get_Client_AC_IDS_With_Custom_AC_FIELDS(10, Trim(custom_pref_text10.Text)) & ") "
                    End If

                End If
            End If
            '------- CLIENT CUSTOM SEARCH ------------













            field_name = "engine_total_time"
            If InStr(sql_where, field_name) > 0 Then

                sql_where_start = Left(Trim(sql_where), InStr(Trim(sql_where), field_name) - 1)

                sql_where_end = Right(Trim(sql_where), Len(Trim(sql_where)) - InStr(Trim(sql_where), field_name) + 1)
                ' this will get me rest

                sql_where_end = Replace(sql_where_end, field_name, "")

                If InStr(Left(Trim(sql_where_end), 10), "between") > 0 Then
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), "between") - 1)

                    If InStr(Trim(sql_where_end), " and") > 0 Then ' else we are at the end of the select 
                        temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), " and") - 1)
                        sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), " and") - 3)

                        temp_string = Replace(temp_string, "tween", "")
                        sql_where_value = temp_string

                        If InStr(Trim(sql_where_end), " and") > 0 Then ' else we are at the end of the select 
                            temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), " and") - 1)
                            sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), " and"))
                        Else
                            temp_string = sql_where_end ' then its the last item in the search 
                            sql_where_end = ""
                        End If

                        sql_where_value2 = temp_string

                        sql_where_value = " ((ac_engine_1_tot_hrs >= " & sql_where_value & " and ac_engine_1_tot_hrs <= " & sql_where_value2 & ") or  (ac_engine_2_tot_hrs >= " & sql_where_value & " and ac_engine_2_tot_hrs <= " & sql_where_value2 & ") or (ac_engine_3_tot_hrs >= " & sql_where_value & " and ac_engine_3_tot_hrs <= " & sql_where_value2 & ") or (ac_engine_4_tot_hrs >= " & sql_where_value & " and ac_engine_4_tot_hrs <= " & sql_where_value2 & ")) "

                    End If


                ElseIf InStr(Left(Trim(sql_where_end), 10), "<") > 0 Then
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), "<") - 1)

                    If InStr(Trim(sql_where_end), "and") > 0 Then ' else we are at the end of the select 
                        temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), " and") - 1)
                        sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), " and"))
                    Else
                        temp_string = sql_where_end ' then its the last item in the search 
                        sql_where_end = ""
                    End If

                    'should just have value now 
                    sql_where_value = " (ac_engine_1_tot_hrs < " & temp_string & " or  ac_engine_2_tot_hrs < " & temp_string & " or ac_engine_3_tot_hrs < " & temp_string & " or ac_engine_4_tot_hrs  < " & temp_string & ") "

                ElseIf InStr(Left(Trim(sql_where_end), 10), ">") > 0 Then
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), ">") - 1)

                    If InStr(Trim(sql_where_end), " and") > 0 Then ' else we are at the end of the select 
                        temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), " and") - 1)
                        sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), " and"))
                    Else
                        temp_string = sql_where_end ' then its the last item in the search 
                        sql_where_end = ""
                    End If

                    'should just have value now 
                    sql_where_value = " (ac_engine_1_tot_hrs < " & temp_string & " or  ac_engine_2_tot_hrs < " & temp_string & " or ac_engine_3_tot_hrs < " & temp_string & " or ac_engine_4_tot_hrs  < " & temp_string & ") "

                    sql_where_value = " (ac_engine_1_tot_hrs > " & temp_string & " or  ac_engine_2_tot_hrs > " & temp_string & " or ac_engine_3_tot_hrs > " & temp_string & " or ac_engine_4_tot_hrs  > " & temp_string & ") "
                ElseIf InStr(Left(Trim(sql_where_end), 10), "=") > 0 Then
                    sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), "=") - 1)

                    If InStr(Trim(sql_where_end), " and") > 0 Then ' else we are at the end of the select 
                        temp_string = Left(Trim(sql_where_end), InStr(Trim(sql_where_end), " and") - 1)
                        sql_where_end = Right(Trim(sql_where_end), Len(Trim(sql_where_end)) - InStr(Trim(sql_where_end), " and"))
                    Else
                        temp_string = sql_where_end ' then its the last item in the search 
                        sql_where_end = ""
                    End If

                    'should just have value now 
                    sql_where_value = " (ac_engine_1_tot_hrs < " & temp_string & " or  ac_engine_2_tot_hrs < " & temp_string & " or ac_engine_3_tot_hrs < " & temp_string & " or ac_engine_4_tot_hrs  < " & temp_string & ") "

                    sql_where_value = " (ac_engine_1_tot_hrs = " & temp_string & " or  ac_engine_2_tot_hrs = " & temp_string & " or ac_engine_3_tot_hrs = " & temp_string & " or ac_engine_4_tot_hrs  = " & temp_string & ") "
                End If


                sql_where = sql_where_start
                sql_where = sql_where & sql_where_value & sql_where_end
            End If

            'ADDED IN MSW - NEED TO REPLACE WITH THE HISTORY FLAT TABLE WHEN USING HISTORY - 10/12/18
            If History Then
                If InStr(sql_where, "(select amod_number_of_engines from aircraft_model with (NOLOCK) where aircraft_model.amod_id = View_Aircraft_Flat.amod_id)") > 0 Then
                    sql_where = Replace(sql_where, "(select amod_number_of_engines from aircraft_model with (NOLOCK) where aircraft_model.amod_id = View_Aircraft_Flat.amod_id)", "(select amod_number_of_engines from aircraft_model with (NOLOCK) where aircraft_model.amod_id = View_Aircraft_History_Flat.amod_id)")
                End If
            End If

            'This small piece of code is going to append the company where clause to the company session variable.
            'But first we check whether or not that variable is blank, if it isn't, and the company where clause is blank, we go ahead and 
            'append an 'and' to it.
            If HttpContext.Current.Session.Item("MasterAircraftCompany") <> "" And sql_company_where <> "" Then
                HttpContext.Current.Session.Item("MasterAircraftCompany") += " and "
            End If
            'Then we check for blank, and append it to the where clause.
            If sql_company_where <> "" Then
                HttpContext.Current.Session.Item("MasterAircraftCompany") += sql_company_where
            End If

            'We need to check whether an 'and' was needed
            If sql_where <> "" Then
                If sql_company_where <> "" Then
                    sql_where += " and "
                End If
            End If
            'Finally we append the where clause to the company where clause
            If sql_company_where <> "" Then
                sql_where += sql_company_where
            End If
            Dim HoldClsSubscription As New crmSubscriptionClass

            HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
            HoldClsSubscription.crmBusiness_Flag = Business
            HoldClsSubscription.crmCommercial_Flag = Commercial
            HoldClsSubscription.crmHelicopter_Flag = Helicopter
            HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
            HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
            HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag

            'Commented out for right now
            'Needs to be put on live and tested - also having a problem with serialization
            'This is a catch added specifically for the aircraft search.
            'because it's possible to run this particular search with no parameters,
            'and the cls function automatically adds an and, I need to check to see if there's any where parameters
            'and if there isn't, I need to remove the starting 'and'
            If sql_where = "" Then
                Dim TempVarProductCodeSelection As String = clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True)
                Dim MyChar() As Char = {"A", "N", "D"}
                sql_where += " " & TempVarProductCodeSelection.TrimStart(MyChar)
            Else
                sql_where += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True)
            End If

            sql = sql & sql_where


            HttpContext.Current.Session.Item("MasterAircraftWhere") = " where " & sql_where



            sql += " order by "

            If sql_order <> "" Then
                sql += sql_order
                HttpContext.Current.Session.Item("MasterAircraftSort") = " order by " & sql_order
            Else
                If History Then
                    sql += " amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort" '" amod_make_name, amod_model_name, ac_ser_no_sort"
                Else
                    sql += " ac_ser_no_sort"
                End If
                HttpContext.Current.Session.Item("MasterAircraftSort") = " order by  amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
            End If



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & IIf(History = True, "<em>(History Side)</em>", "<em>(Aircraft Side)</em>"), Me.GetType().FullName, sql)


            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            SqlCommand.CommandText = sql
            'This will use the entire query so we don't have to put it back together.
            'This will be used on the Folder Maintenance Page to save the query:
            HttpContext.Current.Session.Item("MasterAircraft") = sql

            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                aTempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
            End Try

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return aTempTable
        aTempTable = Nothing

    End Function

    Public Function DisplayMobileCompanies(ByVal acID As Long) As String
        Dim returnString As String = "<span class=""display_block"">"
        Dim CompTable As New DataTable
        CompTable = CompanyFunctions.MobileACLoadCompanies(acID, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag)

        If Not IsNothing(CompTable) Then
            If CompTable.Rows.Count > 0 Then
                For Each r As DataRow In CompTable.Rows
                    returnString += r("actype_name") & ": " & DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, r("comp_name").ToString, "compName", "")
                Next
            End If
        End If
        returnString += "</span>"
        Return returnString
    End Function

    Public Sub Aircraft_Search(ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, ByVal model_string As String,
                               ByVal forSale_Flag As String, ByVal forLease_Flag As String, ByVal onExclusive_Flag As String,
                               ByVal SerialNo_Start As String, ByVal SerialNo_End As String, ByVal DoNotUseAltSer As Boolean,
                               ByVal RegistrationNo As String, ByVal RegistrationNo_Exact As Boolean, ByVal DoNotSearchPrevRegNo As Boolean,
                               ByVal LifeCycleStage As String, ByVal Status As String, ByVal Ownership As String,
                               ByVal PreviouslyOwned_Flag As String, ByVal Model_Type As String, ByVal Airframe_Type As String,
                               ByVal CombinedAirframeTypeString As String, ByVal Make_String As String, ByVal PageNumber As Integer,
                               ByVal PageSort As String, ByVal bindFromSession As Boolean, ByVal AircraftTextStringDisplay As String,
                               ByVal Journal_Date As String, ByVal Journal_Type As String, ByVal Journal_Retail_Only As Boolean,
                               ByVal Journal_New_Aircraft As Boolean, ByVal Journal_Used_Aircraft As Boolean,
                               ByVal Journal_Subcat_Part2 As String, ByVal Journal_Subcat_Part2_Operator As String,
                               ByVal Journal_Subcat_Part3 As String, ByVal Journal_Subcat_Part3_Operator As String,
                               ByVal AC_Status As String, ByVal DynamicQueryString As String,
                               ByVal FinancialInstitution As String, ByVal FinancialDate As String, ByVal FinancialDocType As String,
                               ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String, ByVal CompanyContinentString As String,
                               ByVal CompanyRegionString As String,
                               ByVal Business As Boolean, ByVal Helicopter As Boolean, ByVal Commercial As Boolean,
                               ByVal BaseCountriesString As String, ByVal BaseContinentString As String,
                               ByVal BaseRegionString As String, ByVal BaseStateName As String, ByVal CompanyStateName As String,
                               ByVal JournalIDs As String, ByVal onMarket As Boolean, ByVal offMarket As Boolean, ByVal writtenOff As Boolean)


        Dim RecordsPerPage As Integer = 0
        Dim Results_Table As New DataTable
        Dim Paging_Table As New DataTable

        'Please note - The Aircraft and History Search are ran off of this page. 
        'They use seperate datagrid and datalists
        'The code basically creates two container datagrids and then
        'Binds them to the one that we're supposed to be binding them to.
        'So that we only really need one binding, paging, etc. 
        'Properly Dispose at the End.
        Dim Dynamically_Configured_Datagrid As New DataGrid
        Dim Dynamically_Configured_Repeated As New Repeater
        Dim Dynamically_Configured_DataList As New DataList


        'FolderInformation.Text = ""
        'FolderInformation.Visible = False

        'Swapping is here.
        Try

            If History Then
                Dynamically_Configured_Datagrid = TransactionSearchDataGrid
                Dynamically_Configured_DataList = TransactionSearchDataList
            Else
                help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Aircraft List")

                Dynamically_Configured_Datagrid = AircraftSearchDataGrid
                Dynamically_Configured_DataList = AircraftSearchDataList
                Dynamically_Configured_Repeated = ResultsSearchData

                If Session.Item("isMobile") Then
                    Dynamically_Configured_DataList = mobileDataList
                    Dynamically_Configured_DataList.RepeatColumns = 1
                End If

                If Session.Item("localSubscription").crmAerodexFlag = False Then
                    Dynamically_Configured_Datagrid.Columns(5).Visible = True
                    Dynamically_Configured_Datagrid.Columns(6).Visible = True
                End If

                ' shut off faa or argus, based on field
                If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then
                    Dynamically_Configured_Datagrid.Columns(4).Visible = False
                    Dynamically_Configured_Datagrid.Columns(5).Visible = True
                Else
                    Dynamically_Configured_Datagrid.Columns(4).Visible = True
                    Dynamically_Configured_Datagrid.Columns(5).Visible = False
                End If

                If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then
                    Dynamically_Configured_Datagrid.Columns(11).Visible = True
                End If
            End If


            If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
            End If

            Initial(False)
            aircraft_attention.Text = ""
            'Resetting the datagrid to show headers just in case they returned a 0 result search and it toggled it off.
            'This will not toggle the visibility of the datagrid at all, so if it's invisible, nothing will really happen.
            Dynamically_Configured_Datagrid.ShowHeader = True


            HttpContext.Current.Session.Item("SearchString") = AircraftTextStringDisplay

            If static_folder.Text = "true" And History = False And MarketEvent = False And DynamicQueryString = "" Then
            Else
                If bindFromSession = True And Not IsNothing(Session.Item("Aircraft_Master")) Then
                    Results_Table = Session.Item("Aircraft_Master")
                Else
                    Results_Table = AircraftListingPageQuery(WeightClass, ManufacturerName, AcSize, model_string,
                                                             forSale_Flag, forLease_Flag,
                                                             onExclusive_Flag, SerialNo_Start,
                                                             SerialNo_End, DoNotUseAltSer, RegistrationNo,
                                                             RegistrationNo_Exact, DoNotSearchPrevRegNo,
                                                             LifeCycleStage, Status, Ownership, PreviouslyOwned_Flag,
                                                             Model_Type, Airframe_Type, CombinedAirframeTypeString, Make_String,
                                                             AC_Status, FinancialInstitution, FinancialDate, FinancialDocType,
                                                             Journal_Date, Journal_Type, Journal_Retail_Only, Journal_New_Aircraft,
                                                             Journal_Used_Aircraft, Journal_Subcat_Part2, Journal_Subcat_Part2_Operator,
                                                             Journal_Subcat_Part3, Journal_Subcat_Part3_Operator, DynamicQueryString,
                                                             History, PageSort, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString,
                                                             Business, Helicopter, Commercial,
                                                             BaseCountriesString, BaseContinentString, BaseRegionString, BaseStateName, CompanyStateName,
                                                             JournalIDs, onMarket, offMarket, writtenOff)
                End If
            End If

            If Not IsNothing(Results_Table) Then

                Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
                If Results_Table.Rows.Count > 0 Then

                    next_.CommandArgument = "1"
                    previous.CommandArgument = "0"

                    Session.Item("Aircraft_Master") = Results_Table
                    'This is basically saying that if the datagrid isn't visible, don't fill it
                    If Dynamically_Configured_Datagrid.Visible = True Or ResultsSearchData.Visible = True Then

                        'Added this on 07/01/2015 - This is going to reset the current page index whenever the datagrid listing is active
                        'and a new search occurs.

                        If MarketEvent Or History Then
                            Dynamically_Configured_Datagrid.DataSource = Results_Table
                            Dynamically_Configured_Datagrid.PageSize = RecordsPerPage
                            Dynamically_Configured_Datagrid.CurrentPageIndex = 0 'PageNumber - 1
                            Dynamically_Configured_Datagrid.DataBind()
                        Else
                            ResultsSearchData.Visible = True
                            Paging_Table = Results_Table.Clone
                            Dim afiltered_Client As DataRow() = Results_Table.Select("ac_count <= " & RecordsPerPage, "")
                            For Each atmpDataRow_Client In afiltered_Client
                                Paging_Table.ImportRow(atmpDataRow_Client)
                            Next

                            'ResultsSearchData.
                            ResultsSearchData.DataSource = Paging_Table
                            ResultsSearchData.DataBind()
                        End If
                    End If



                    'This is basically saying that if the datagrid isn't visible, don't fill it
                    If Dynamically_Configured_DataList.Visible = True Then
                        If Session.Item("isMobile") = False Then
                            'We need to add the paging to this for now since the datalist doesn't natively support paging. 
                            'For right now, we clone the results table (getting the schema) then filter based on the ac_count field (added during query)
                            'This will allow us to bind based on the paging table.
                            Paging_Table = Results_Table.Clone
                            Dim afiltered_Client As DataRow() = Results_Table.Select("ac_count <= " & RecordsPerPage, "")
                            For Each atmpDataRow_Client In afiltered_Client
                                Paging_Table.ImportRow(atmpDataRow_Client)
                            Next
                        Else
                            Paging_Table = Results_Table 'no paging.
                        End If
                        Dynamically_Configured_DataList.DataSource = Paging_Table
                        Dynamically_Configured_DataList.DataBind()
                    End If

                    criteria_results.Text = Results_Table.Rows.Count & " Results"

                    If Session.Item("isMobile") = False Then
                        record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
                        bottom_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)


                        'This will fill up the dropdown bar with however many pages.
                        If Results_Table.Rows.Count > RecordsPerPage Then
                            Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                            'Criteria_Bar2.Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                            SetPagingButtons(False, True)
                            'Criteria_Bar2.SetPagingButtons(False, True)
                        Else
                            Fill_Page_To_To_Dropdown(1)
                            SetPagingButtons(False, False)
                            'Criteria_Bar2.SetPagingButtons(False, False)
                        End If
                    Else
                        SetPagingButtons(False, False)
                        record_count.Text = ""
                        bottom_record_count.Text = ""
                    End If


                    'PanelCollapseEx.Collapsed = True
                    Paging_Table = Nothing
                    Results_Table = Nothing

                Else
                    aircraft_attention.Text = "<br /><p class='padding'><b>No Aircraft Found. Please refine your search and try again.</b></p><br /><br />"
                    criteria_results.Text = "0 Results"
                    SetPagingButtons(False, False)
                    record_count.Text = ""
                    Aircraft_Bottom_Paging.Visible = False

                    'Changed on 07/01/2015
                    'Whenever a search occurs that returns no results, we reset the page index to 0. 
                    'This means the datagrid starts on the first page - which would be empty in this case, but 
                    'it still needs to be set at 0. Otherwise if you're paging through a recordset, get to a page that's not 0
                    'then do a search that returns nothing, you'll recieve an invalid CurrentPageIndex value error.
                    Dynamically_Configured_Datagrid.CurrentPageIndex = 0
                    Dynamically_Configured_Datagrid.ShowHeader = False
                    Dynamically_Configured_Datagrid.DataSource = New DataTable
                    Dynamically_Configured_Datagrid.DataBind()
                    Dynamically_Configured_DataList.DataSource = New DataTable
                    Dynamically_Configured_DataList.DataBind()
                End If
            Else 'this means that the datatable equals nothing
                'And that there was an error on the data side.
                If Not IsNothing(masterPage) Then
                    masterPage.LogError("Aircraft_Search() Aircraft_Listing.aspx.vb (" & ErrorReportingTypeString & "): " & masterPage.aclsData_Temp.class_error)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): MasterPageNotEstablished"
                End If

                Aircraft_Bottom_Paging.Visible = False
                'aircraft_attention.Text = "<br /><p class='padding'><b>No Aircraft Found. Please refine your search and try again.</b></p><br /><br />"

                aircraft_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
                If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
                    If Not IsNothing(masterPage) Then
                        aircraft_attention.Text += masterPage.aclsData_Temp.class_error
                    End If
                End If
                masterPage.aclsData_Temp.class_error = ""

                criteria_results.Text = "0 Results"
                SetPagingButtons(False, False)
                record_count.Text = ""

                'Changed on 07/01/2015
                'Whenever a search occurs that returns a datatable that equals nothing, we set the page index to 0.
                'This will just reset the paging so it stops another error from occuring (invalid page index error).
                Dynamically_Configured_Datagrid.CurrentPageIndex = 0
                Dynamically_Configured_Datagrid.ShowHeader = False
                Dynamically_Configured_Datagrid.DataSource = New DataTable
                Dynamically_Configured_Datagrid.DataBind()
                Dynamically_Configured_DataList.DataSource = New DataTable
                Dynamically_Configured_DataList.DataBind()

            End If


            If History = False Then
                If Page.IsPostBack Then 'Runs on all searches except initial folder.
                    If Session.Item("isMobile") = True Then
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.acSearchUpdate, Me.GetType(), "lazyLoad", "$("".lazy"").lazy({  onError: function(element) {console.log('error loading ' + element.data('data-src'));}});", True)
                    End If
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("collapsePanelJS") Then
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.acSearchUpdate, Me.GetType(), "collapsePanelJS", SetUpScriptsAfterSearch(True, True), True)
                    End If
                Else
                    'Runs only on folder 
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapsePanelJS", "$(""#" & Collapse_Panel.ClientID & """).hide();$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');SetUpSlider(); ", True)
                End If
            Else
                If Page.IsPostBack Then 'Runs on all searches except initial folder.
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("collapsePanelJS") Then
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.acHistSearchUpdate, Me.GetType(), "collapsePanelJS", SetUpScriptsAfterSearch(True, True), True)
                    End If
                Else
                    'Runs only on folder 
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapsePanelJS", "$(""#" & Collapse_Panel.ClientID & """).hide();$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');SetUpSlider(); ", True)
                End If
            End If


            If Page.IsPostBack And MarketEvent = False Then
                If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
                    If ResultsSearchData.Visible = True Or Dynamically_Configured_DataList.Visible = True Then
                        'Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetScrollEvent);
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "masonryPost", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadMasonry);", True)
                    End If
                End If
            Else
                If ResultsSearchData.Visible = True Or Dynamically_Configured_DataList.Visible = True Then
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPostFolderCall", "$(document).ready(function() {loadMasonry();});", True)
                End If
            End If


            If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                    masterPage.SetStatusText(HttpContext.Current.Session.Item("SearchString"), True)
                End If
            End If

            folderInformationUpdate.Update()
            listingUpdatePanel.Update()

        Catch ex As Exception
            'Error Logging in case this Aircraft Search Function Fails. 
            'This will allow us to know how and a little more about the error.
            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try

            Aircraft_Bottom_Paging.Visible = False
            aircraft_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
            If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
                aircraft_attention.Text += "Query: " & Session.Item("MasterAircraft").ToString & " " & ex.Message.ToString
            End If

            criteria_results.Text = "0 Results"
            SetPagingButtons(False, False)
            record_count.Text = ""
            Dynamically_Configured_Datagrid.DataSource = New DataTable
            Dynamically_Configured_Datagrid.DataBind()
            Dynamically_Configured_DataList.DataSource = New DataTable
            Dynamically_Configured_DataList.DataBind()
        Finally
            Dynamically_Configured_Datagrid = Nothing
            Dynamically_Configured_DataList = Nothing
        End Try

    End Sub

    Public Sub MoveRepeater(ByRef StartCount As Integer, ByRef EndCount As Integer, ByVal Dynamically_Configured_DataGrid As DataGrid, ByVal Dynamically_Configured_DataRepeater As Repeater, ByVal Dynamically_Configured_DataList As Object, ByVal HoldTable As DataTable, ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
        Dim RecordsPerPage As Integer = 0
        Dim CurrentPage As Integer = 0
        Dim CurrentRecord As Integer = 0
        ' Dim EndCount As Integer = 0
        'Dim StartCount As Integer = 0
        Dim Paging_Table As New DataTable
        Dim CountString As String = ""
        Dim TotalPageNumber As Integer = 0

        Try
            'Initial(False)
            If HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage
            End If

            If Not IsNothing(HoldTable) Then
                TotalPageNumber = Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage)

                CurrentPage = pageNumber 'company_next.CommandArgument ' - 1
                CurrentRecord = (RecordsPerPage * CurrentPage) - HoldTable.Rows.Count + HoldTable.Rows.Count
                If CurrentRecord = 0 Then
                    StartCount = 1
                Else
                    StartCount = CurrentRecord + 1
                End If

                If CurrentRecord + RecordsPerPage >= HoldTable.Rows.Count Then
                    CountString = StartCount & "-" & HoldTable.Rows.Count
                    EndCount = HoldTable.Rows.Count
                Else
                    CountString = StartCount & "-" & CurrentRecord + pageNumber
                    EndCount = CurrentRecord + RecordsPerPage
                End If

                Fill_Page_To_To_Dropdown(Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage))



                Paging_Table = HoldTable.Clone
                Dim afiltered_Client As DataRow() = HoldTable.Select("", "") '"ac_count >= " & StartCount & " and ac_count <= " & EndCount, PageSort) '"ac_count >= " & StartCount & " and ac_count <= " & EndCount
                'For Each atmpDataRow_Client In afiltered_Client
                For i = (StartCount - 1) To EndCount - 1 'RecordsPerPage - 1
                    Paging_Table.ImportRow(afiltered_Client(i))
                Next

                record_count.Text = "Showing " & StartCount & " - " & IIf(HoldTable.Rows.Count <= RecordsPerPage, HoldTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= HoldTable.Rows.Count, RecordsPerPage + StartCount, HoldTable.Rows.Count))
                bottom_record_count.Text = "Showing " & StartCount & " - " & IIf(HoldTable.Rows.Count <= RecordsPerPage, HoldTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= HoldTable.Rows.Count, RecordsPerPage + StartCount, HoldTable.Rows.Count))



                go_to_dropdown.Items.Clear()
                go_to_dropdown.Items.Add(New ListItem(pageNumber + 1, ""))

                go_to_dropdown_2.Items.Clear()
                go_to_dropdown_2.Items.Add(New ListItem(pageNumber + 1, ""))



                'only bind if results is visible.

                If Not IsNothing(Dynamically_Configured_DataRepeater) Then
                    If Dynamically_Configured_DataRepeater.Visible = True Then
                        Dynamically_Configured_DataRepeater.DataSource = Paging_Table
                        Dynamically_Configured_DataRepeater.DataBind()

                    End If
                End If


                If Not IsNothing(Dynamically_Configured_DataList) Then
                    If Dynamically_Configured_DataList.Visible = True Then
                        Dynamically_Configured_DataList.DataSource = Paging_Table
                        Dynamically_Configured_DataList.DataBind()
                        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPage") Then
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "masonryPageMove", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadMasonry);", True)
                        End If
                    End If
                End If


            End If

            Dynamically_Configured_DataGrid = Nothing

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub


    Public Function SetUpScriptsAfterSearch(ByVal afterSearch As Boolean, ByVal reinitializeHeader As Boolean) As String
        Dim jsStr As String = ""
        Try
            If afterSearch Then

                jsStr += "$('[id^=CRM_Logo_Text]').removeClass();"
                jsStr += "$('[id^=CRM_Logo_Text]').addClass(""current_status"");"
                jsStr += "$('[id^=searchCriteriaToggle]').removeClass();$('[id^=searchCriteriaToggle]').addClass(""searchCriteria slideoutToolTip"");$(function(){"
                'If Session.Item("isMobile") = True Then
                '    jsStr += "$('#" & divTabLoading.ClientID & "').addClass('display_none');"
                'End If


                jsStr += "$(""#" & Collapse_Panel.ClientID & """).hide();"
                'End If
                jsStr += "$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');"

                jsStr += "SetUpSlider();"
            Else
                Dim rgz As Regex = New Regex("[^a-zA-Z0-9:-]")
                Dim displaySearch As String = ""
                If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                        displaySearch = rgz.Replace(HttpContext.Current.Session.Item("SearchString"), "")
                    End If
                End If
                'If Session.Item("isMobile") = False Then
                jsStr += "$(""#" & Collapse_Panel.ClientID & """).hide();"
                'End If

                jsStr += "$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');"

                jsStr += "$(function(){"
                If reinitializeHeader Then
                    jsStr += " SetUpSlider();"
                End If
            End If

            jsStr += "});ChangeTheMouseCursorOnItemParentDocument('cursor_default" & IIf(Session.Item("isMobile"), " lowerLevel", "") & "');"
            'jsStr += "$find(""" & PanelCollapseEx.ClientID & """).collapsePanel(); });ChangeTheMouseCursorOnItemParentDocument('cursor_default');"

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return jsStr

    End Function

    Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)
        Try
            If Aircraft_Criteria.Visible = True Then

                previous_all.Visible = back_page
                bottom_previous_all.Visible = back_page
                previous.Visible = back_page
                bottom_previous.Visible = back_page

                next_all.Visible = next_page
                bottom_next_all.Visible = next_page

                next_.Visible = next_page
                bottom_next_.Visible = next_page
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function TrimName(ByVal acYear As Object, ByVal acMake As Object, ByRef acModel As Object, ByVal amodId As Long, ByVal serNo As Object, ByVal acID As Long)
        Dim returnString As String = ""

        Try

            If Not IsDBNull(acYear) Then
                If Not String.IsNullOrEmpty(acYear) Then
                    returnString += acYear
                End If
            End If

            If Not IsDBNull(acMake) Then
                If Not String.IsNullOrEmpty(acMake) Then
                    If returnString <> "" Then
                        returnString += " "
                    End If
                    returnString += acMake
                End If
            End If

            If Not IsDBNull(acModel) Then
                If Not String.IsNullOrEmpty(acModel) Then
                    If returnString <> "" Then
                        returnString += " "
                    End If
                    returnString += acModel.ToString
                End If
            End If

            If Not IsDBNull(serNo) Then
                If Not String.IsNullOrEmpty(serNo) Then
                    If returnString <> "" Then
                        returnString += " "
                    End If

                    'Dim total As Integer = Len(returnString) + Len(serNo.ToString)
                    'Dim difference As Integer = total - 31
                    'If Len(returnString) + Len(serNo.ToString) > 31 Then
                    '  returnString += DisplayFunctions.WriteDetailsLink(acID, 0, 0, 0, True, serNo.ToString.Substring(0, Len(serNo.ToString) - difference) & "...", "", "")
                    'Else
                    returnString += "SN: " & DisplayFunctions.WriteDetailsLink(acID, 0, 0, 0, True, serNo.ToString, "", "")
                    'End If
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return returnString

    End Function

    Public Sub MovePage(ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)

        Dim RecordsPerPage As Integer = 0
        Dim CurrentPage As Integer = 0
        Dim CurrentRecord As Integer = 0
        Dim EndCount As Integer = 0
        Dim StartCount As Integer = 0
        Dim HoldTable As New DataTable
        Dim Paging_Table As New DataTable
        Dim CountString As String = ""
        Dim TotalPageNumber As Integer = 0
        Dim Dynamically_Configured_Datagrid As New DataGrid
        Dim Dynamically_Configured_DataList As New DataList

        Try

            If History Then
                Dynamically_Configured_Datagrid = TransactionSearchDataGrid
                Dynamically_Configured_DataList = TransactionSearchDataList
            ElseIf MarketEvent Then
                Dynamically_Configured_Datagrid = EventsDataGrid
            Else
                Dynamically_Configured_Datagrid = AircraftSearchDataGrid
                Dynamically_Configured_DataList = AircraftSearchDataList
                If Session.Item("isMobile") Then
                    Dynamically_Configured_DataList = mobileDataList
                End If
            End If

            Initial(False)


            If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
            End If

            If Not IsNothing(Session.Item("Aircraft_Master")) Then
                HoldTable = Session.Item("Aircraft_Master")

                If History = False And MarketEvent = False Then
                    MoveRepeater(StartCount, EndCount, Dynamically_Configured_Datagrid, ResultsSearchData, Dynamically_Configured_DataList, HoldTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
                    SetPagingButtons(IIf(StartCount = 1, False, True), IIf(HoldTable.Rows.Count <= EndCount, False, True))

                Else

                    TotalPageNumber = Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage)
                    Dynamically_Configured_Datagrid.PageSize = RecordsPerPage


                    If next_ Then
                        Dynamically_Configured_Datagrid.CurrentPageIndex += 1
                    ElseIf prev_ Then
                        Dynamically_Configured_Datagrid.CurrentPageIndex -= 1
                    ElseIf prev_all Then
                        Dynamically_Configured_Datagrid.CurrentPageIndex = 0
                    ElseIf next_all Then
                        Dynamically_Configured_Datagrid.CurrentPageIndex = TotalPageNumber - 1 'TotalPageNumber 'Results.PageCount - 1
                    Else
                        Dynamically_Configured_Datagrid.CurrentPageIndex = pageNumber '- 1
                    End If

                    go_to_dropdown.Items.Clear()
                    go_to_dropdown.Items.Add(New ListItem(Dynamically_Configured_Datagrid.CurrentPageIndex + 1, ""))

                    go_to_dropdown_2.Items.Clear()
                    go_to_dropdown_2.Items.Add(New ListItem(Dynamically_Configured_Datagrid.CurrentPageIndex + 1, ""))



                    Dynamically_Configured_Datagrid.DataSource = HoldTable

                    'only bind if results is visible.
                    If Dynamically_Configured_Datagrid.Visible = True Then
                        Try
                            Dynamically_Configured_Datagrid.DataBind()
                        Catch
                            Dynamically_Configured_Datagrid.CurrentPageIndex = 0
                            Dynamically_Configured_Datagrid.DataBind()
                        End Try
                    End If

                    CurrentPage = Dynamically_Configured_Datagrid.CurrentPageIndex + 1
                    CurrentRecord = (Dynamically_Configured_Datagrid.PageSize * Dynamically_Configured_Datagrid.CurrentPageIndex) - HoldTable.Rows.Count + HoldTable.Rows.Count



                    Fill_Page_To_To_Dropdown(Math.Ceiling(TotalPageNumber))
                    If CurrentRecord = 0 Then
                        StartCount = 1
                    Else
                        StartCount = CurrentRecord + 1
                    End If

                    If CurrentRecord + Dynamically_Configured_Datagrid.PageSize >= HoldTable.Rows.Count Then
                        CountString = StartCount & "-" & HoldTable.Rows.Count
                        EndCount = HoldTable.Rows.Count
                    Else
                        CountString = StartCount & "-" & CurrentRecord + Dynamically_Configured_Datagrid.PageSize
                        EndCount = CurrentRecord + Dynamically_Configured_Datagrid.PageSize
                    End If

                    SetPagingButtons(IIf(StartCount = 1, False, True), IIf(HoldTable.Rows.Count = EndCount, False, True))
                    ' Criteria_Bar2.SetPagingButtons(IIf(StartCount = 1, False, True), IIf(HoldTable.Rows.Count = EndCount, False, True))

                    record_count.Text = "Showing " & CountString
                    bottom_record_count.Text = "Showing " & CountString
                    'Criteria_Bar2.record_count.Text = "Showing " & CountString

                    If MarketEvent = False Then
                        If Dynamically_Configured_DataList.Visible = True Then
                            Paging_Table = HoldTable.Clone
                            Dim afiltered_Client As DataRow() = HoldTable.Select("ac_count >= " & StartCount & " and ac_count <= " & EndCount, "")
                            For Each atmpDataRow_Client In afiltered_Client
                                Paging_Table.ImportRow(atmpDataRow_Client)
                            Next

                            Dynamically_Configured_DataList.DataSource = Paging_Table
                            Dynamically_Configured_DataList.DataBind()

                            If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPage") Then
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "masonryPageDataList", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadMasonry);", True)
                            End If
                        End If
                    End If

                End If
            End If

            Dynamically_Configured_DataList.Dispose()
            Dynamically_Configured_Datagrid.Dispose()

            folderInformationUpdate.Update()
            listingUpdatePanel.Update()
            If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                    masterPage.SetStatusText(HttpContext.Current.Session.Item("SearchString"), True)
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub Initial(ByVal initial_page_load As Boolean)
        Try
            If initial_page_load = True Then

                criteria_results.Visible = False
                sort_by_text.Visible = False
                sort_by_dropdown.Visible = False
                view_dropdown.Visible = False
                fleetAnalyzerContainer.Visible = False
                actions_dropdown.Visible = False
                paging.Visible = False
                per_page_text.Visible = False
                per_page_dropdown_.Visible = False
                go_to_text.Visible = False
                go_to_dropdown_.Visible = False
                go_to_dropdown_cell_2.Visible = False
                go_to_text_2.Visible = False
                'PanelCollapseEx.CollapseControlID = ""
                'PanelCollapseEx.Collapsed = False
                'PanelCollapseEx.ClientState = False

            Else

                If Session.Item("isMobile") = True Then
                    controlLink.Attributes.Remove("class")
                    controlLink.Attributes.Add("class", "newSearchLink mobile_display_on_cell")
                    ' mobileSpace.Visible = True
                End If

                Aircraft_Bottom_Paging.Visible = True
                'PanelCollapseEx.CollapseControlID = "Control_Panel"
                criteria_results.Visible = True
                sort_by_text.Visible = True
                sort_by_dropdown.Visible = True
                If MarketEvent = False Then
                    view_dropdown.Visible = True
                    If History = True Then
                        sort_dropdown.CssClass = "ul_top history_sort_width"
                        sort_submenu_dropdown.CssClass = "ul_bottom history_sort_dropdown"
                        If PageSort = "" Then
                            sort_submenu_dropdown.Items.Clear()
                            sort_dropdown.Items.Clear()

                            sort_submenu_dropdown.Items.Add(New ListItem("Year Dlv", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Year Mfr", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Ser #", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Model/Ser#", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("AFTT", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Trans Date ASC", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Trans Date DESC", ""))

                            sort_dropdown.Items.Add(New ListItem("Model/Ser#", ""))

                        End If
                    ElseIf History = False And MarketEvent = False Then
                        fleetAnalyzerContainer.Visible = True
                        If PageSort = "" Then
                            sort_submenu_dropdown.Items.Clear()
                            sort_dropdown.Items.Clear()
                            sort_submenu_dropdown.Items.Add(New ListItem("Year Dlv", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Year Mfr", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Ser #", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("Model/Ser#", ""))

                            sort_submenu_dropdown.Items.Add(New ListItem("AFTT", ""))
                            sort_submenu_dropdown.Items.Add(New ListItem("EST AFTT", ""))
                            If Session.Item("localSubscription").crmAerodexFlag = False Then
                                sort_submenu_dropdown.Items.Add(New ListItem("List Date", ""))
                                sort_submenu_dropdown.Items.Add(New ListItem("Status", ""))
                                sort_submenu_dropdown.Items.Add(New ListItem("Price", ""))
                            End If
                            sort_submenu_dropdown.Items.Add(New ListItem("Reg #", ""))
                            sort_dropdown.Items.Add(New ListItem("Ser #", ""))


                            If Session.Item("isMobile") = True Then
                                sort_dropdown.Items.Clear()
                                sort_dropdown.Items.Add(New ListItem("Sort", ""))
                            End If
                        End If
                    End If
                Else
                    If PageSort = "" Then
                        sort_submenu_dropdown.Items.Clear()


                        sort_dropdown.Items.Clear()
                        sort_dropdown.CssClass = "ul_top event_sort_width"
                        sort_submenu_dropdown.CssClass = "ul_bottom event_sort_dropdown"

                        sort_submenu_dropdown.Items.Add(New ListItem("Make/Model", ""))
                        sort_submenu_dropdown.Items.Add(New ListItem("Reg #", ""))
                        sort_submenu_dropdown.Items.Add(New ListItem("Event Date/Time", ""))

                        sort_dropdown.Items.Add(New ListItem("Make/Model", ""))
                    End If

                End If
                actions_dropdown.Visible = True
                paging.Visible = True
                'PanelCollapseEx.Collapsed = True
                'PanelCollapseEx.ClientState = True

                per_page_text.Visible = True
                per_page_dropdown_.Visible = True
                go_to_text.Visible = True
                go_to_dropdown_.Visible = True
                go_to_dropdown_cell_2.Visible = True
                go_to_text_2.Visible = True
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)
        Try
            If Aircraft_Criteria.Visible = True Then
                go_to_submenu_dropdown.Items.Clear()
                go_to_submenu_dropdown_2.Items.Clear()
                For x = 1 To pageNumber
                    go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
                    go_to_submenu_dropdown_2.Items.Add(New ListItem(x, x))
                Next
                next_all.CommandArgument = pageNumber.ToString
                previous_all.CommandArgument = "0"
                bottom_next_all.CommandArgument = pageNumber.ToString
                bottom_previous_all.CommandArgument = "0"
            End If



        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub AlterListing(ByVal TypeOfListing As Integer, ByVal RecordAmount As Integer)
        Try
            Dim Dynamically_Configured_Datagrid As New DataGrid
            Dim Dynamically_Configured_DataList As New DataList

            If History Then
                Dynamically_Configured_Datagrid = TransactionSearchDataGrid
                Dynamically_Configured_DataList = TransactionSearchDataList
            Else
                Dynamically_Configured_Datagrid = AircraftSearchDataGrid
                Dynamically_Configured_DataList = AircraftSearchDataList
                If Session.Item("isMobile") Then
                    Dynamically_Configured_DataList = mobileDataList
                End If
            End If

            Select Case TypeOfListing
                Case 0 'Listing Display
                    Dynamically_Configured_Datagrid.Visible = True
                    Dynamically_Configured_DataList.Visible = False
                    If History = False And MarketEvent = False Then
                        ResultsSearchData.Visible = True
                        Dynamically_Configured_Datagrid.Visible = False
                    End If
                Case 1 'Image Display
                    ResultsSearchData.Visible = False
                    Dynamically_Configured_Datagrid.Visible = False
                    Dynamically_Configured_DataList.Visible = True
            End Select

            Dynamically_Configured_DataList.Dispose()
            Dynamically_Configured_Datagrid.Dispose()


        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Sub SwitchGalleryListing(ByVal showtype As Integer)
        Try
            Select Case showtype
                Case 0
                    view_dropdown.Items.Clear()
                    view_dropdown.Items.Add(New ListItem("", ""))
                    view_dropdown.CssClass = "ul_top listing_view_bullet"
                    AlterListing(0, 0)
                    Session.Item("localUser").crmACListingView = eListingView.LISTING
                Case 1
                    view_dropdown.Items.Clear()
                    view_dropdown.Items.Add(New ListItem("", ""))
                    view_dropdown.CssClass = "ul_top thumnail_view_bullet"
                    AlterListing(1, 0)
                    Session.Item("localUser").crmACListingView = eListingView.GALLERY
            End Select

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
        Try
            Select Case selectedLI
                Case "Trans Date DESC"
                    PageSort = " journ_date desc"
                Case "Trans Date ASC"
                    PageSort = " journ_date asc"
                Case "List Date"
                    PageSort = " ac_list_date"
                Case "AFTT"
                    PageSort = " ac_airframe_tot_hrs" ', ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs"
                Case "EST AFTT"
                    PageSort = " ac_est_airframe_hrs"
                Case "Status"
                    PageSort = " ac_status"
                Case "Price"
                    PageSort = "ac_asking desc, ac_asking_price"
                Case "Make/Model"
                    PageSort = "amod_make_name, amod_model_name"
                Case "Reg #"
                    PageSort = "ac_reg_no"
                Case "Event Date/Time"
                    PageSort = "apev_entry_date"
                Case "Ser #", "Sort"
                    PageSort = "ac_ser_no_sort"
                Case "Year Dlv"
                    PageSort = "ac_year"
                Case "Year Mfr"
                    PageSort = "ac_mfr_year"
                Case Else
                    PageSort = " amod_make_name, amod_model_name, ac_ser_no_sort"
            End Select

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
        Try
            Dim selectedLI As New ListItem
            selectedLI = sender.Items(e.Index)
            If sender.id.ToString = "sort_submenu_dropdown" Then
                sort_dropdown.Items.Clear()
                sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
                SetPageSort(selectedLI.Text)
                acsearch_Click(acsearch, EventArgs.Empty)

            ElseIf sender.id.ToString = "view_submenu_dropdown" Then
                SwitchGalleryListing(e.Index)

                acsearch_Click(acsearch, EventArgs.Empty, True)
                'End Select

            ElseIf sender.id.ToString = "go_to_submenu_dropdown" Or sender.id.ToString = "go_to_submenu_dropdown_2" Then
                go_to_dropdown.Items.Clear()
                go_to_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))

                go_to_dropdown_2.Items.Clear()
                go_to_dropdown_2.Items.Add(New ListItem(selectedLI.Text, ""))


                If MarketEvent Or History Then
                    SetPageNumber(CInt(selectedLI.Text))
                Else
                    SetPageNumber(CInt(selectedLI.Text) - 1)
                End If

                next_.CommandArgument = PageNumber + 1
                bottom_next_.CommandArgument = PageNumber + 1

                MovePage(False, False, False, False, True, PageNumber)

            ElseIf sender.id.ToString = "per_page_submenu_dropdown" Then
                per_page_dropdown.Items.Clear()
                per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
                Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
                next_.CommandArgument = PageNumber.ToString
                bottom_next_.CommandArgument = PageNumber.ToString


                MovePage(False, False, False, False, False, 0)

            End If



            Dim jsStr As String = ""
            jsStr = SetUpScriptsAfterSearch(False, IIf(sender.id.ToString = "per_page_submenu_dropdown", True, True))
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "collapsePanelJSSubmenu", jsStr, True)
            If Not Page.ClientScript.IsClientScriptBlockRegistered("CursorNormalRemove") Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "CursorNormalRemove", "ChangeTheMouseCursorOnItemParentDocument('cursor_default" & IIf(Session.Item("isMobile"), " lowerLevel", "") & "');", True)
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function ShowHistoryLink(ByVal ModelID As Object, ByVal JournalID As Object, ByVal AircraftID As Object, ByVal AskingPrice As Object, ByVal ListingView As Boolean, ByVal sale_price As Object, ByVal sale_price_text As String, ByVal sale_price_displayable As String)
        Dim ReturnString As String = ""
        Dim ReturnLink As Boolean = True
        Dim temp_num As String = ""
        Dim sale_price_showing As Boolean = False
        Try


            If Not IsDBNull(sale_price) And sale_price_displayable = "Y" Then
                If Trim(sale_price) <> "0" Then

                    If Trim(sale_price_text) <> "" Then
                        ReturnString = "<table cellspacing='0' cellpadding='0' border='0'><tr><td>"
                        ReturnString &= Trim(sale_price_text) & "&nbsp;</td><td>"
                    End If

                    If Trim(sale_price.ToString) <> "" Then
                        temp_num = "$" & FormatNumber((CInt(sale_price.ToString) / 1000), 0) & "k"
                        ReturnString &= DisplayFunctions.TextToImage(temp_num, 9, "", "40", "Reported Sale Price Displayed with Permission from Source", "", True)
                        sale_price_showing = True
                    End If

                    If Trim(sale_price_text) <> "" Then
                        ReturnString &= "</td></tr></table>"
                    End If
                End If

            End If


            If sale_price_showing = False Then
                ReturnString = "<a href=""#"" class=""padding_right " & IIf(ListingView = False, "HistoryReportAskingPrice", "") & """ onclick=""javascript:load('SendSalesTransaction.aspx?sendSales=true"

                If Not IsDBNull(ModelID) Then
                    ReturnString += "&ModelID=" & ModelID.ToString
                Else
                    ReturnLink = False
                End If
                If Not IsDBNull(JournalID) Then
                    ReturnString += "&jID=" & JournalID.ToString
                Else
                    ReturnLink = False
                End If
                If Not IsDBNull(AircraftID) Then
                    ReturnString += "&acid=" & AircraftID.ToString
                Else
                    ReturnLink = False
                End If

                If ReturnLink Then
                    ReturnString += "','','scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no');return false;"" title=""Click this link to report asking/sale price on transactions for JETNET use"">" & IIf(ListingView, "<div class=""dollarSign"" title=""Report Asking/Sale Price""></div>", "Report Asking/Sale Price") & "</a>"
                Else
                    ReturnString = ""
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return ReturnString

    End Function

    Private Sub acsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal LoadFromSession As Boolean = False) Handles acsearch.Click, transaction_search.Click, events_search.Click

        Try

            Page.Validate("Numeric")

            If Page.IsValid Then
                'PanelCollapseEx.Collapsed = True
                'PanelCollapseEx.ClientState = True

                Dim AC_Market_Status As String = ""
                Dim DynamicQueryString As String = ""
                Dim ForSale_Flag As String = ""
                Dim ForLease_Flag As String = ""
                Dim OnExclusive_Flag As String = ""
                Dim SerialNo_Start As String = ""
                Dim SerialNo_End As String = ""
                Dim RegistrationNo As String = ""
                Dim LifeCycleStage_String As String = ""
                Dim Status As String = ""
                Dim Ownership_String As String = ""
                Dim PreviouslyOwned_Flag As String = ""
                Dim Journal_Date As String = ""
                Dim Journal_Date_Operator As String = ""
                Dim Journal_Type As String = ""
                Dim Journal_Retail_Sales As Boolean = False
                Dim Journal_New_Aircraft As Boolean = False
                Dim Journal_Used_Aircraft As Boolean = False
                Dim Journal_Subcat_Part2 As String = ""
                Dim Journal_Subcat_Part2_Operator As String = ""
                Dim Journal_Subcat_Part3 As String = ""
                Dim Journal_Subcat_Part3_Operator As String = ""
                Dim JournalIDs As String = ""
                Dim BuildSearchString As String = ""
                Dim MarketCategory As String = ""
                Dim MarketType As String = ""
                Dim StartDate As Date = Now()
                Dim Months As Integer = 0
                Dim Days As Integer = 0
                Dim Hours As Integer = 0
                Dim Minutes As Integer = 0
                Dim EventTypeOfSearch As String = ""
                Dim UseDefaultDate As Boolean = True
                Dim FinancialInstitution As New TextBox
                Dim FinancialDate As New TextBox
                Dim FinancialDocType As New TextBox
                Dim counter As Integer = 0
                Dim TemporaryTable As New DataTable
                Dim totalcounthold As Integer = 0

                'Continent/Region/State/Timezone
                Dim CompanyRegionString As String = ""
                Dim CompanyContinentString As String = ""
                Dim CompanyTimeZoneString As String = ""
                'Dim CompanyStatesString As String = ""
                Dim CompanyCountriesString As String = ""
                Dim CompanyStateName As String = ""

                Dim BaseRegionString As String = ""
                Dim BaseContinentString As String = ""
                Dim BaseTimeZoneString As String = ""
                'Dim BaseStatesString As String = ""
                Dim BaseCountriesString As String = ""
                Dim BaseStateName As String = ""

                'Model/Type/Make/WeightClass
                Dim ModelsString As String = ""
                Dim MakeString As String = ""
                Dim TypeString As String = ""

                Dim WeightClassDDL As New Object
                Dim WeightClass As String = ""

                Dim ManufacturerStr As String = ""

                Dim AcSizeStr As String = ""

                Dim AirframeTypeString As String = ""
                Dim CombinedAirframeTypeString As String = ""

                Dim Helicopter As Boolean = False
                Dim Business As Boolean = False
                Dim Commercial As Boolean = False

                Dim frac_program_string As String = ""
                Dim help_string As String = ""
                Dim help_string2 As String = ""
                Dim temp_spot As Integer = 0
                Dim amod_id_list As String = ""

                Dim NewSearchClass As New SearchSelectionCriteria

                If MarketEvent = True Then
                    'If it's events and on test/local and only on search
                    If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me.EventSearchUpdatePanel, Me.GetType(), "ToggleEventAlert", "$('#" & eventAlertMaintenanceButton.ClientID & "').show();", True)
                    End If
                End If

                If Not IsNothing(Trim(Request("flight"))) Then
                    'Only in the case of the redirect from the flight activity view.
                    If Not String.IsNullOrEmpty(Trim(Request("flight"))) Then
                        If Trim(Request("flight")) = "true" Then
                            NewSearchClass = Session.Item("searchCriteria")
                        End If
                    End If
                End If

                'Toggle for Project search
                If LoadFromSession = False Then
                    If Page.Request.Form("project_search") <> "Y" Then 'the search request variable isn't set, meaning this is a re-search.
                        If FolderInformation.Visible = True Then ' the folder information is visible, meaning there is a folder
                            FolderInformation.CssClass = "alertFolderNameBar help_cursor"
                            FolderInformation.ToolTip = "This Red Information Bar means that your screen varies from what's saved in your current selected folder. " & vbNewLine & vbNewLine & "Under the actions dropdown, please click, 'Save As - New Folder' to save this search as a new folder, or click 'Save Current Folder' to update your selected folder. " & vbNewLine & vbNewLine & "To deselect the folder currently chosen, please click the, 'Close Current Folder' link located on this information bar."
                        End If
                    End If
                End If

                If Not IsNothing(ViewTMMDropDowns.FindControl("ddlWeightClass")) Then
                    WeightClassDDL = ViewTMMDropDowns.FindControl("ddlWeightClass")
                End If

                'Model/Make/Type String Building
                If Session.Item("isMobile") = True Then
                    If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
                        Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
                        ModelsString = CLng(ModelData(3))
                        MakeString = UCase(ModelData(2))
                        AirframeTypeString = UCase(ModelData(1))
                        TypeString = UCase(ModelData(0))
                        CombinedAirframeTypeString = UCase(ModelData(0)) & "|" & UCase(ModelData(1))
                    End If
                Else
                    BuildSearchString += DisplayFunctions.GetMakeModelTypeFromCommonControl("", BuildSearchString,
                                                                                            ModelsString, MakeString,
                                                                                            TypeString, AirframeTypeString,
                                                                                            CombinedAirframeTypeString,
                                                                                            WeightClassDDL, WeightClass,
                                                                                            ManufacturerStr, AcSizeStr,
                                                                                            Business, Helicopter, Commercial, amod_id_list)
                End If



                'Display folder name
                If comp_folder_name.Text <> "" Then
                    BuildSearchString = DisplayFunctions.BuildSearchTextDisplay(comp_folder_name, "Folder")
                End If


                If Not String.IsNullOrEmpty(WeightClass.Trim) Then
                    'Setting up The Weight Class in Session
                    NewSearchClass.SearchCriteriaWeightClass = WeightClass
                End If

                If Not String.IsNullOrEmpty(ManufacturerStr.Trim) Then
                    'Setting up The mfr name in Session
                    NewSearchClass.SearchCriteriaManufacturerName = ManufacturerStr
                End If

                If Not String.IsNullOrEmpty(AcSizeStr.Trim) Then
                    'Setting up The ac size in Session
                    NewSearchClass.SearchCriteriaAcSize = AcSizeStr
                End If

                'Setting up Make in Session
                NewSearchClass.SearchCriteriaMake = MakeString
                'Setting up Model in Session
                NewSearchClass.SearchCriteriaModel = ModelsString
                'Setting up Type in Session
                NewSearchClass.SearchCriteriaType = TypeString
                'Setting up Business in Session
                NewSearchClass.SearchCriteriaBusinessFlag = Business
                'Setting up Helicopter in Session
                NewSearchClass.SearchCriteriaHelicopterFlag = Helicopter
                'Setting up Commercial in session
                NewSearchClass.SearchCriteriaCommercialFlag = Commercial


                For i = 0 To market.Items.Count - 1
                    If market.Items(i).Selected Then
                        If market.Items(i).Value <> "" Then
                            Select Case market.Items(i).Value
                                Case "For Sale"
                                    'ForSale_Flag = "Y"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += "  (ac_forsale_flag = 'Y') "
                                Case "For Sale/Lease"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += " ( ac_status = 'For Sale/Lease' and "
                                    AC_Market_Status += "  ac_forsale_flag = 'Y') "
                  'ForSale_Flag = "Y"
                                Case "For Sale/Trade"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += " (ac_status = 'For Sale/Trade' and "
                                    AC_Market_Status += " ac_forsale_flag = 'Y') "
                  'ForSale_Flag = "Y"
                                Case "For Sale on Exclusive"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += " ( ac_status LIKE 'For Sale%' and "
                                    AC_Market_Status += " ac_forsale_flag = 'Y' and "
                                    AC_Market_Status += " ac_exclusive_flag = 'Y') "
                  'ForSale_Flag = "Y"
                  'OnExclusive_Flag = "Y"
                                Case "For Sale Not on Exclusive"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += " ( ac_status LIKE 'For Sale%' and "
                                    AC_Market_Status += " ac_forsale_flag = 'Y' and "
                                    AC_Market_Status += " ac_exclusive_flag = 'N') "
                  'ForSale_Flag = "Y"
                  'OnExclusive_Flag = "N"
                                Case "Not For Sale"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    'ForSale_Flag = "N"
                                    AC_Market_Status += "  (ac_forsale_flag = 'N') "
                                Case "Lease"
                                    If AC_Market_Status <> "" Then
                                        AC_Market_Status += " or "
                                    End If
                                    AC_Market_Status += " (ac_status = 'Lease' OR ac_asking = 'Lease' and "
                                    AC_Market_Status += " ac_lease_flag = 'Y') "
                                    'ForLease_Flag = "Y"
                            End Select
                        End If
                    End If
                Next
                If AC_Market_Status <> "" Then
                    AC_Market_Status = "(" & AC_Market_Status & ")"
                End If

                NewSearchClass.SearchCriteriaMarketStatus = ""
                If market.SelectedValue <> "" Then
                    'Setting up the Market in Session to be called
                    NewSearchClass.SearchCriteriaMarketStatus = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(market, False, 0, True)

                    'Market String Building Textual Display
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(market, "Market Status")
                End If


                If Not String.IsNullOrEmpty(cref_comp_id.SelectedValue) Then
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(cref_comp_id, "Company Folder")
                End If

                If Session.Item("isMobile") = True And AC_Market_Status = "" Then

                    NewSearchClass.SearchCriteriaMarketStatus = ""
                    If UCase(mobileStatus.SelectedValue) = "FOR SALE" Then
                        AC_Market_Status += "  (ac_forsale_flag = 'Y') "
                        NewSearchClass.SearchCriteriaMarketStatus = mobileStatus.SelectedValue
                    ElseIf UCase(mobileStatus.SelectedValue) = "NOT FOR SALE" Then
                        AC_Market_Status += "  (ac_forsale_flag = 'N') "
                        NewSearchClass.SearchCriteriaMarketStatus = mobileStatus.SelectedValue
                    End If
                End If

                'grabbing the market event information.
                'Since these boxes exist in the same fashion on the yacht side, I just made one function to grab
                'all of the info and set up the build search screen so that way we don't have to do it twice.
                If MarketEvent Then
                    Aircraft_SearchToGrabTheEventOnlyInformation(MarketEvent, EventTypeOfSearch, MarketCategory,
                                                                 MarketType, Months, Days, Hours, Minutes,
                                                                 UseDefaultDate, StartDate,
                                                                 BuildSearchString, NewSearchClass,
                                                                 events_months, event_days, event_hours, event_minutes)
                End If

                If History Then
                    'JournalID, passed if we have a contact folder
                    If journ_id.Text <> "" Then
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(journ_id, "Journal IDs")
                        JournalIDs = clsGeneral.clsGeneral.StripChars(journ_id.Text, True)
                    End If

                    'Select Case UCase(transaction_new_used.SelectedValue)
                    '    Case "NEW"
                    NewSearchClass.SearchCriteriaSalesOfNewAircraftOnly = False
                    NewSearchClass.SearchCriteriaSalesOfUsedAircraftOnly = False
                    If journ_newac_flag.Checked = True Then
                        'Setting up New/Used Flag in Session
                        NewSearchClass.SearchCriteriaSalesOfNewAircraftOnly = True
                        NewSearchClass.SearchCriteriaSalesOfUsedAircraftOnly = False
                        Journal_New_Aircraft = True
                        Journal_Used_Aircraft = False
                        BuildSearchString += "New Aircraft: True<br />"
                        BuildSearchString += "Exclude Internal Transactions: True<br />"
                        journ_exclude_internal_transactions.Checked = True
                        NewSearchClass.SearchCriteriaExcludeInternalTransactions = True
                        'Case "USED"
                    End If
                    If jcat_used_retail_sales_flag.Checked = True Then
                        'Setting up New/Used Flag in Session
                        NewSearchClass.SearchCriteriaSalesOfNewAircraftOnly = False
                        NewSearchClass.SearchCriteriaSalesOfUsedAircraftOnly = True
                        Journal_New_Aircraft = False
                        Journal_Used_Aircraft = True
                        BuildSearchString += "Used Aircraft: True<br />"
                        BuildSearchString += "Exclude Internal Transactions: True<br />"
                        NewSearchClass.SearchCriteriaExcludeInternalTransactions = True
                        journ_exclude_internal_transactions.Checked = True
                        'End Select
                    End If

                    'Checkbox to exclude internal transactions on history
                    'What we get to do here, is ignore this if the used AC is true, or the NEW ac is true.
                    'The query already appends what is needed (basically that the internal transaction flag = N.
                    If Journal_New_Aircraft = False And Journal_Used_Aircraft = False Then
                        If journ_exclude_internal_transactions.Checked Then
                            NewSearchClass.SearchCriteriaExcludeInternalTransactions = True
                            BuildSearchString += "Exclude Internal Transactions: True<br />"
                            Dim QueryData As New AdvancedQueryResults
                            QueryData.FieldName = "journ_internal_trans_flag"
                            QueryData.OperatorChoice = "Equals"
                            QueryData.DataType = "String"
                            QueryData.SearchValue = "N"
                            QueryData.FieldDisplay = "Exclude Internal Transactions"
                            Query_Class_Array.Add(QueryData)

                        Else
                            NewSearchClass.SearchCriteriaExcludeInternalTransactions = False
                        End If
                    End If

                    NewSearchClass.SearchCriteriaHistoryDateOperator = ""
                    If Not String.IsNullOrEmpty(journ_date_operator.SelectedValue) Then
                        Journal_Date_Operator = journ_date_operator.SelectedValue
                        'Setting up Date Operator in Session
                        NewSearchClass.SearchCriteriaHistoryDateOperator = journ_date_operator.SelectedValue
                    End If

                    NewSearchClass.SearchCriteriaHistoryDate = ""
                    If Not String.IsNullOrEmpty(journ_date.Text) Then
                        'Setting up Date in Session
                        NewSearchClass.SearchCriteriaHistoryDate = journ_date.Text
                        If IsDate(journ_date.Text) Then
                            Journal_Date = Month(journ_date.Text) & "/" & Day(journ_date.Text) & "/" & Year(journ_date.Text)
                            If Not String.IsNullOrEmpty(journ_date_operator.SelectedValue) Then
                                Journal_Date = clsGeneral.clsGeneral.PrepQueryString(Journal_Date_Operator, Journal_Date, "Date", False, "", True)
                            Else
                                Journal_Date = ""
                            End If
                            BuildSearchString += "Journal Date " & Journal_Date_Operator & " " & journ_date.Text & "<br />"
                        ElseIf Journal_Date_Operator = "Between" And InStr(journ_date.Text, ":") Then
                            Journal_Date = clsGeneral.clsGeneral.PrepQueryString(Journal_Date_Operator, journ_date.Text, "Date", False, "", True)
                            BuildSearchString += "Journal Date " & Journal_Date_Operator & " " & Replace(journ_date.Text, ":", " and ") & "<br />"

                        End If
                    End If

                    'Journal Type String Building For Query
                    NewSearchClass.SearchCriteriaHistoryType = ""
                    Journal_Type = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(journ_subcat_code_part1, False, 0, True)
                    'Journal_Type = journ_subcat_code_part1.SelectedValue
                    'Journal_Type = ""
                    If Journal_Type <> "" Then
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(journ_subcat_code_part1, "Transaction Type")
                        'Setting up Type in session  
                        NewSearchClass.SearchCriteriaHistoryType = Journal_Type
                    End If

                    'Journal Retail Sales Building for Query
                    Journal_Retail_Sales = transaction_retail.Checked
                    'setting up retail sales in session
                    NewSearchClass.SearchCriteriaRetailActivity = transaction_retail.Checked

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(transaction_retail, "Retail Sales Only")

                    'Journal Subcat Part 2 Building for Query
                    NewSearchClass.SearchCriteriaHistoryFromAnswer = ""
                    Journal_Subcat_Part2 = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(journ_subcat_code_part2, True, 0, True)

                    'Journal Subcat Part 2 Textual Display
                    If Journal_Subcat_Part2 <> "" Then
                        'Setting up From in Session
                        NewSearchClass.SearchCriteriaHistoryFromAnswer = Journal_Subcat_Part2

                        'Journal Subcat Part 2 Operator, setting this isn't really important unless we have the other part, so ignore it unless there's a listbox selection
                        Journal_Subcat_Part2_Operator = journ_subcat_code_part2_operator.SelectedValue

                        'Setting up From Operator in Session
                        NewSearchClass.SearchCriteriaHistoryFromOperator = Journal_Subcat_Part2_Operator

                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(journ_subcat_code_part2, "Transaction " & Journal_Subcat_Part2_Operator)
                    End If

                    'Journal Subcat Part 3 Building for Query
                    NewSearchClass.SearchCriteriaHistoryToAnswer = ""
                    Journal_Subcat_Part3 = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(journ_subcat_code_part3, True, 0, True)

                    'Journal Subcat Part 2 Textual Display
                    If Journal_Subcat_Part3 <> "" Then
                        'Setting up To in Session
                        NewSearchClass.SearchCriteriaHistoryToAnswer = Journal_Subcat_Part3

                        'Journal Subcat Part 3 Operator, setting this isn't really important unless we have the other part, so ignore it unless there's a listbox selection
                        Journal_Subcat_Part3_Operator = journ_subcat_code_part3_operator.SelectedValue

                        'Setting up To Operator in Session
                        NewSearchClass.SearchCriteriaHistoryToOperator = Journal_Subcat_Part3_Operator

                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(journ_subcat_code_part3, "Transaction " & Journal_Subcat_Part3_Operator)
                    End If
                End If

                NewSearchClass.SearchCriteriaSerNoStart = ""
                If Not String.IsNullOrEmpty(ac_ser_no_from.Text.Trim) Then

                    'Saving Ser No Start in Session
                    NewSearchClass.SearchCriteriaSerNoStart = ac_ser_no_from.Text.Trim
                    SerialNo_Start = NewSearchClass.SearchCriteriaSerNoStart
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_ser_no_from.Text.Trim, "Serial # Start")

                End If

                NewSearchClass.SearchCriteriaSerNoEnd = ""
                If Not String.IsNullOrEmpty(ac_ser_no_to.Text.Trim) Then

                    'Saving Ser No End in Session
                    NewSearchClass.SearchCriteriaSerNoEnd = ac_ser_no_to.Text.Trim
                    SerialNo_End = NewSearchClass.SearchCriteriaSerNoEnd
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_ser_no_to.Text.Trim, "Serial # End")

                End If


                NewSearchClass.SearchCriteriaRegNo = ""

                If Not String.IsNullOrEmpty(ac_reg_no.Text.Trim) Then
                    'Saving Reg No Start in Session
                    ac_reg_no.Text = Replace(ac_reg_no.Text, "=", "-")
                    NewSearchClass.SearchCriteriaRegNo = ac_reg_no.Text

                    RegistrationNo = Trim(ac_reg_no.Text)
                    RegistrationNo = RegistrationNo.TrimStart() 'Added 10/19/2016. This removes the beginning enter character if one was accidently pasted in.
                    RegistrationNo = RegistrationNo.TrimEnd() 'added 10/10/2016 to circumvent people pressing the enter sign on this field in order to submit the form. This removes all trailing white space including enter.
                    ' RegistrationNo = Replace(RegistrationNo, "*", "%")
                    RegistrationNo = RegistrationNo.Replace(vbCr, "").Replace(vbLf, ",")
                    RegistrationNo = Replace(RegistrationNo, ";", ",")

                    RegistrationNo = clsGeneral.clsGeneral.CleanUserData(RegistrationNo, Constants.cEmptyString, Constants.cCommaDelim, True)

                    If InStr(RegistrationNo, ",") > 0 Then
                        Dim QueryData As New AdvancedQueryResults
                        QueryData.FieldName = "ac_reg_no_search"

                        If ac_reg_no_exact_match.Checked = True Then
                            QueryData.OperatorChoice = "Equals"
                        Else
                            QueryData.OperatorChoice = "Begins With"
                            RegistrationNo = Replace(RegistrationNo, ",", "*,")
                        End If

                        QueryData.DataType = "String"
                        QueryData.SearchValue = Trim(RegistrationNo)
                        QueryData.FieldDisplay = "Reg #"
                        Query_Class_Array.Add(QueryData)
                        RegistrationNo = ""
                    End If

                    'RegistrationNo String Building Textual Display
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_reg_no, "Reg #")
                End If


                'Life Cycle Building
                'LifeCycleStage_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ac_lifecycle_stage, True, 0, True)
                '4.	Aircraft search by retired and in storage lifecycles – slight modifications to criteria for these
                'a.	Retired Selection = ((ac_lifecycle_stage = '4' AND lower(ac_status) <> 'withdrawn from use - stored'))
                'b.	In Storage = ((ac_lifecycle_stage = '4' AND lower(ac_status) = 'withdrawn from use - stored'))
                'Because of above, the lifecycle stage has to be set up and extracted special. 

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'LIFECYCLE

                For i = 0 To ac_lifecycle_stage.Items.Count - 1
                    If ac_lifecycle_stage.Items(i).Selected Then
                        If ac_lifecycle_stage.Items(i).Value <> "" Then 'Here we check to see if there is a value, meaning there's no selection
                            If UCase(ac_lifecycle_stage.Items(i).Value) <> "ALL" Then 'Checking to make sure ALL isn't checked, if it is, we don't need to search
                                If LifeCycleStage_String <> "" Then
                                    LifeCycleStage_String += " or "
                                End If
                                If ac_lifecycle_stage.Items(i).Value = 4 Then
                                    LifeCycleStage_String += " (ac_lifecycle_stage = '4' AND lower(ac_status) <> 'withdrawn from use - stored') "
                                ElseIf ac_lifecycle_stage.Items(i).Value = 5 Then
                                    LifeCycleStage_String += " (ac_lifecycle_stage = '4' AND lower(ac_status) = 'withdrawn from use - stored') "
                                Else
                                    LifeCycleStage_String += " (ac_lifecycle_stage = '" & ac_lifecycle_stage.Items(i).Value & "') "
                                End If
                            End If
                        End If
                    End If
                Next


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                'Life Cycle String Building Textual Display
                NewSearchClass.SearchCriteriaLifeCycle = ""
                If LifeCycleStage_String <> "" Then
                    'Saving LifeCycle in Session
                    NewSearchClass.SearchCriteriaLifeCycle = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ac_lifecycle_stage, True, 0, True)

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_lifecycle_stage, "Lifecycle")
                End If

                'Ownership Building For Query
                Ownership_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ac_ownership_type, True, 0, True)

                'Life Cycle String Building Textual Display
                NewSearchClass.SearchCriteriaOwnership = ""
                If Ownership_String <> "" Then
                    'Saving LifeCycle in Session
                    NewSearchClass.SearchCriteriaOwnership = Ownership_String

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_ownership_type, "Ownership")
                End If

                'Lease Flag
                NewSearchClass.SearchCriteriaLeaseStatus = ""
                If lease_status.SelectedValue <> "" Then
                    'Saving LifeCycle in Session
                    NewSearchClass.SearchCriteriaLeaseStatus = lease_status.SelectedValue

                    ForLease_Flag = lease_status.SelectedValue
                    'Lease Flag String Building Textual Display
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(lease_status, "Lease Status")
                End If
                'Previously Owned Flag
                NewSearchClass.SearchCriteriaPreviouslyOwned = ""
                If ac_previously_owned_flag.SelectedValue <> "" Then
                    'Saving Previously Owned Flag in Session
                    NewSearchClass.SearchCriteriaPreviouslyOwned = ac_previously_owned_flag.SelectedValue

                    PreviouslyOwned_Flag = ac_previously_owned_flag.SelectedValue
                    'Previously Owned String Building Textual Display
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ac_previously_owned_flag, "Previously Owned")
                End If

                Session.Item("Frac_Percent") = ""
                'Set Up Advanced Search Dynamic Query String
                If static_folder.Text = "true" And static_folder_ac_ids.Text <> "" And History = False And MarketEvent = False Then
                    DynamicQueryString = " ac_id in (" & clsGeneral.clsGeneral.StripChars(static_folder_ac_ids.Text, False) & ") "
                Else
                    Dim srchMaintenanceItems As New searchMaintenanceItems

                    Dim tempDataManager As New clsData_Manager_SQL
                    tempDataManager.JETNET_DB = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn

                    DynamicQueryString = AdvancedQueryResults.BuildDynamicString(tempDataManager, Query_Class_Array,
                                                                       ac_advanced_search, BuildSearchString,
                                                                       FinancialInstitution, FinancialDate, static_folder_ac_ids,
                                                                       static_folder, totalcounthold, counter,
                                                                       do_not_search_ac_prev_reg_no.Checked, ErrorReportingTypeString, FinancialDocType, srchMaintenanceItems, False, amod_id_list)

                    'We are going to go ahead and grab this here so that we can access the relationship text - that way instead of values - we get the actual relationship name.
                    'Without looking it up in BuildDynamicString from the database.
                    'With this edit down below - storing this in the search string was also commented out in BuildDynamicString for the contact types.
                    If cref_contact_type.SelectedValue <> "" Then
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(cref_contact_type, IIf(comp_not_in_selected.Checked = False, "Relationships to Aircraft", "Not in Selected Relationship"))
                    End If

                    ' put in hard code for percent ownship search - MSW - 5/6/15

                    If Trim(DynamicQueryString) <> "" Then
                        If InStr(Trim(DynamicQueryString), "cref_owner_percent  <") > 0 Then
                            temp_spot = InStr(Trim(DynamicQueryString), "cref_owner_percent  <")
                            help_string = Trim(Right(Trim(DynamicQueryString), Len(Trim(DynamicQueryString)) - temp_spot - 21))
                            temp_spot = InStr(Trim(help_string), " ")
                            If temp_spot > 0 Then
                                help_string = Left(Trim(help_string), temp_spot - 1) ' should get you the value  
                            End If
                            Session.Item("Frac_Percent") = "cref_owner_percent > 0 and cref_owner_percent  <" & help_string
                            DynamicQueryString = Replace(DynamicQueryString, "cref_owner_percent  <", "cref_owner_percent > 0 and cref_owner_percent  <")

                        ElseIf InStr(Trim(DynamicQueryString), "cref_owner_percent  >") > 0 Then
                            temp_spot = InStr(Trim(DynamicQueryString), "cref_owner_percent  >")
                            help_string = Trim(Right(Trim(DynamicQueryString), Len(Trim(DynamicQueryString)) - temp_spot - 21))
                            temp_spot = InStr(Trim(help_string), " ")
                            If temp_spot > 0 Then
                                help_string = Left(Trim(help_string), temp_spot - 1) ' should get you the value 
                            End If
                            Session.Item("Frac_Percent") = "cref_owner_percent < 100 and cref_owner_percent  >" & help_string
                            DynamicQueryString = Replace(DynamicQueryString, "cref_owner_percent  >", "cref_owner_percent < 100 and cref_owner_percent  >")
                        ElseIf InStr(Trim(DynamicQueryString), "cref_owner_percent  =") > 0 Then
                            temp_spot = InStr(Trim(DynamicQueryString), "cref_owner_percent  =")
                            help_string = Trim(Right(Trim(DynamicQueryString), Len(Trim(DynamicQueryString)) - temp_spot - 21))
                            temp_spot = InStr(Trim(help_string), " ")
                            If temp_spot > 0 Then
                                help_string = Left(Trim(help_string), temp_spot - 1) ' should get you the value 
                            End If
                            Session.Item("Frac_Percent") = "cref_owner_percent =" & help_string
                        ElseIf InStr(Trim(DynamicQueryString), "cref_owner_percent between") > 0 Then
                            temp_spot = InStr(Trim(DynamicQueryString), "cref_owner_percent between")
                            help_string = Trim(Right(Trim(DynamicQueryString), Len(Trim(DynamicQueryString)) - temp_spot - 26))
                            temp_spot = InStr(Trim(help_string), " and ")
                            help_string2 = Trim(Left(Trim(help_string), temp_spot - 1)) ' should get you the first value 
                            help_string = Trim(Right(Trim(help_string), Len(Trim(help_string)) - 5 - Len(Trim(help_string2))))
                            temp_spot = InStr(Trim(help_string), " ")
                            If temp_spot > 0 Then
                                help_string = Trim(Left(Trim(help_string), temp_spot - 1)) ' should get you the second value 
                            End If
                            Session.Item("Frac_Percent") = "cref_owner_percent between " & help_string2 & " and " & help_string & " "
                        ElseIf InStr(Trim(DynamicQueryString), "external_lav ") > 0 Then
                            DynamicQueryString = Replace(DynamicQueryString, "external_lav ", "(case when  (select top 1 adet_data_description FROM Aircraft_Details with (NOLOCK) WHERE (((Aircraft_Details.adet_data_description LIKE '%external%service%') AND (Aircraft_Details.adet_data_name = 'Lavatory')) or  (Aircraft_Details.adet_data_description LIKE '%external lav service%') ) AND (Aircraft_Details.adet_journ_id = 0) and (Aircraft_Details.adet_ac_id = VIEW_AIRCRAFT_FLAT.ac_id)) is not null then 'Yes' else 'No' end)")
                        End If

                        If DynamicQueryString.ToLower.Contains("lbfractionalprogram") Then

                            frac_program_string = "ac_id IN (select distinct cref_ac_id from Aircraft_Reference"
                            frac_program_string += " WHERE cref_contact_type IN ('17') and cref_journ_id = 0"
                            frac_program_string += " AND cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK) WHERE "

                            If DynamicQueryString.Contains("=") Then

                                Dim frac_program_arr As Array = DynamicQueryString.Split("=")

                                frac_program_string += "pgref_prog_id IN (" + frac_program_arr(1).ToString.Trim + ") ))"

                            ElseIf DynamicQueryString.Contains("in (") Then

                                Dim findFirstParen As Integer = DynamicQueryString.IndexOf("(")
                                Dim findLastParen As Integer = DynamicQueryString.IndexOf(")")

                                Dim frac_program_values As String = DynamicQueryString.Substring(findFirstParen, (findLastParen - findFirstParen) + 1)

                                frac_program_string += "pgref_prog_id IN " + frac_program_values + "))"

                            End If

                            DynamicQueryString = "(" + frac_program_string + ")"

                        End If

                    End If

                    Dim tmpMaintenanceClause1 As New StringBuilder
                    Dim tmpMaintenanceClause2 As New StringBuilder

                    Dim bAsReportedChk1 As Boolean = False
                    Dim bAsReportedChk2 As Boolean = False


                    If Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_item1.Trim) Or Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_item2.Trim) Or srchMaintenanceItems.Maintenance_value1.Trim <> "" Or srchMaintenanceItems.Maintenance_value2.Trim <> "" Then

                        If Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_item1.Trim) Or Trim(srchMaintenanceItems.Maintenance_value1.Trim) <> "" Then

                            bAsReportedChk1 = CBool(IIf(Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_chk1.ToLower), srchMaintenanceItems.Maintenance_chk1.ToLower, "false"))

                            tmpMaintenanceClause1.Append("ac_id IN (SELECT DISTINCT acmaint_ac_id FROM Aircraft_Maintenance WITH(NOLOCK) WHERE (acmaint_journ_id = 0)")
                            If Trim(srchMaintenanceItems.Maintenance_item1.Trim) <> "" Then
                                tmpMaintenanceClause1.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_item1.Replace("maintenance_item", "acmaint_name"))
                            End If

                            If bAsReportedChk1 And srchMaintenanceItems.Maintenance_value1.Trim <> "" Then
                                tmpMaintenanceClause1.Append(Constants.cAndClause + Constants.cDoubleOpen)
                            End If

                            If srchMaintenanceItems.Maintenance_date1.ToUpper.Contains("CW") And srchMaintenanceItems.Maintenance_time1.ToUpper.Contains("DATE") Then

                                If bAsReportedChk1 And srchMaintenanceItems.Maintenance_value1.Trim <> "" Then

                                    Dim tmpValue As String = srchMaintenanceItems.Maintenance_value1.Trim

                                    tmpValue = tmpValue.Replace("acmaint_value", "acmaint_complied_date")
                                    tmpValue = tmpValue.Replace(">", ">=")

                                    tmpMaintenanceClause1.Append(tmpValue)
                                    tmpMaintenanceClause1.Append(Constants.cAndClause + "acmaint_date_type IN ('D','Y')" + Constants.cSingleClose)

                                    Dim firstTick As Integer = tmpValue.IndexOf(Constants.cSingleQuote)
                                    Dim secondTick As Integer = 0
                                    Dim tmpDate As Date = Today
                                    If tmpValue.ToLower.Contains("between") Then
                                        secondTick = tmpValue.IndexOf(Constants.cSingleQuote, firstTick + 1)
                                        tmpDate = CDate(tmpValue.Substring(firstTick, secondTick - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    Else
                                        tmpDate = CDate(tmpValue.Substring(firstTick, tmpValue.Length - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    End If

                                    Dim tmpMonth As String = Month(tmpDate).ToString
                                    Dim tmpYear As String = Year(tmpDate).ToString

                                    Dim tmpMonthDate As String = Constants.cSingleQuote + tmpMonth + "/01/" + tmpYear + Constants.cSingleQuote

                                    If tmpValue.ToLower.Contains("between") Then
                                        Dim tmpStr As String = tmpValue.Substring(0, firstTick)
                                        tmpStr += tmpMonthDate + tmpValue.Substring(secondTick + 1, tmpValue.Length - secondTick - 1)
                                        tmpValue = tmpStr
                                    Else
                                        tmpValue = tmpValue.Substring(0, firstTick)
                                        tmpValue += tmpMonthDate
                                    End If

                                    tmpMaintenanceClause1.Append(Constants.cOrClause + Constants.cSingleOpen + tmpValue)

                                    tmpMaintenanceClause1.Append(Constants.cAndClause + "acmaint_date_type = 'M'" + Constants.cDoubleClose)
                                ElseIf srchMaintenanceItems.Maintenance_value1.Trim <> "" Then
                                    tmpMaintenanceClause1.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value1.Replace("acmaint_value", "acmaint_complied_date"))
                                End If

                            End If

                            If srchMaintenanceItems.Maintenance_date1.ToUpper.Contains("DUE") And srchMaintenanceItems.Maintenance_time1.ToUpper.Contains("DATE") Then

                                If bAsReportedChk1 And srchMaintenanceItems.Maintenance_value1.Trim <> "" Then

                                    Dim tmpValue As String = srchMaintenanceItems.Maintenance_value1.Trim

                                    tmpValue = tmpValue.Replace("acmaint_value", "acmaint_due_date")
                                    tmpValue = tmpValue.Replace(">", ">=")

                                    tmpMaintenanceClause1.Append(tmpValue)
                                    tmpMaintenanceClause1.Append(Constants.cAndClause + "acmaint_date_type IN ('D','Y')" + Constants.cSingleClose)

                                    Dim firstTick As Integer = tmpValue.IndexOf(Constants.cSingleQuote)
                                    Dim secondTick As Integer = 0
                                    Dim tmpDate As Date = Today
                                    If Trim(tmpValue) <> "" Then
                                        If tmpValue.ToLower.Contains("between") Then
                                            secondTick = tmpValue.IndexOf(Constants.cSingleQuote, firstTick + 1)
                                            tmpDate = CDate(tmpValue.Substring(firstTick, secondTick - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                        Else
                                            tmpDate = CDate(tmpValue.Substring(firstTick, tmpValue.Length - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                        End If

                                        Dim tmpMonth As String = Month(tmpDate).ToString
                                        Dim tmpYear As String = Year(tmpDate).ToString

                                        Dim tmpMonthDate As String = Constants.cSingleQuote + tmpMonth + "/01/" + tmpYear + Constants.cSingleQuote

                                        If tmpValue.ToLower.Contains("between") Then
                                            Dim tmpStr As String = tmpValue.Substring(0, firstTick)
                                            tmpStr += tmpMonthDate + tmpValue.Substring(secondTick + 1, tmpValue.Length - secondTick - 1)
                                            tmpValue = tmpStr
                                        Else
                                            tmpValue = tmpValue.Substring(0, firstTick)
                                            tmpValue += tmpMonthDate
                                        End If

                                        tmpMaintenanceClause1.Append(Constants.cOrClause + Constants.cSingleOpen + tmpValue)

                                        tmpMaintenanceClause1.Append(Constants.cAndClause + "acmaint_date_type = 'M'" + Constants.cDoubleClose)

                                    End If

                                Else
                                    tmpMaintenanceClause1.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value1.Replace("acmaint_value", "acmaint_due_date"))
                                End If

                            End If

                            If srchMaintenanceItems.Maintenance_date1.ToUpper.Contains("CW") And srchMaintenanceItems.Maintenance_time1.ToUpper.Contains("HOURS") Then

                                tmpMaintenanceClause1.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value1.Replace("acmaint_value", "acmaint_complied_hrs"))

                            End If

                            If srchMaintenanceItems.Maintenance_date1.ToUpper.Contains("DUE") And srchMaintenanceItems.Maintenance_time1.ToUpper.Contains("HOURS") Then

                                tmpMaintenanceClause1.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value1.Replace("acmaint_value", "acmaint_due_hrs"))

                            End If

                            If Not bAsReportedChk1 Then
                                tmpMaintenanceClause1.Append(Constants.cAndClause + "acmaint_notes NOT LIKE '%as reported%'")
                            End If

                            tmpMaintenanceClause1.Append(Constants.cSingleClose)

                        End If

                        If Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_item2.Trim) Or Trim(srchMaintenanceItems.Maintenance_value2.Trim) <> "" Then

                            bAsReportedChk2 = CBool(IIf(Not String.IsNullOrEmpty(srchMaintenanceItems.Maintenance_chk2.ToLower), srchMaintenanceItems.Maintenance_chk2.ToLower, "false"))

                            tmpMaintenanceClause2.Append("ac_id IN (SELECT DISTINCT acmaint_ac_id FROM Aircraft_Maintenance WITH(NOLOCK) WHERE (acmaint_journ_id = 0)")
                            If Trim(srchMaintenanceItems.Maintenance_item2) <> "" Then
                                tmpMaintenanceClause2.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_item2.Replace("maintenance_item1", "acmaint_name"))
                            End If
                            If bAsReportedChk2 And srchMaintenanceItems.Maintenance_value2.Trim <> "" Then
                                tmpMaintenanceClause2.Append(Constants.cAndClause + Constants.cDoubleOpen)
                            End If

                            If srchMaintenanceItems.Maintenance_date2.ToUpper.Contains("CW") And srchMaintenanceItems.Maintenance_time2.ToUpper.Contains("DATE") Then

                                If bAsReportedChk2 And srchMaintenanceItems.Maintenance_value2.Trim <> "" Then

                                    Dim tmpValue As String = srchMaintenanceItems.Maintenance_value2.Trim

                                    tmpValue = tmpValue.Replace("acmaint_value1", "acmaint_complied_date")
                                    tmpValue = tmpValue.Replace(">", ">=")

                                    tmpMaintenanceClause2.Append(tmpValue)
                                    tmpMaintenanceClause2.Append(Constants.cAndClause + "acmaint_date_type IN ('D','Y')" + Constants.cSingleClose)

                                    Dim firstTick As Integer = tmpValue.IndexOf(Constants.cSingleQuote)
                                    Dim secondTick As Integer = 0
                                    Dim tmpDate As Date = Today

                                    If tmpValue.ToLower.Contains("between") Then
                                        secondTick = tmpValue.IndexOf(Constants.cSingleQuote, firstTick + 1)
                                        tmpDate = CDate(tmpValue.Substring(firstTick, secondTick - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    Else
                                        tmpDate = CDate(tmpValue.Substring(firstTick, tmpValue.Length - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    End If

                                    Dim tmpMonth As String = Month(tmpDate).ToString
                                    Dim tmpYear As String = Year(tmpDate).ToString

                                    Dim tmpMonthDate As String = Constants.cSingleQuote + tmpMonth + "/01/" + tmpYear + Constants.cSingleQuote

                                    If tmpValue.ToLower.Contains("between") Then
                                        Dim tmpStr As String = tmpValue.Substring(0, firstTick)
                                        tmpStr += tmpMonthDate + tmpValue.Substring(secondTick + 1, tmpValue.Length - secondTick - 1)
                                        tmpValue = tmpStr
                                    Else
                                        tmpValue = tmpValue.Substring(0, firstTick)
                                        tmpValue += tmpMonthDate
                                    End If

                                    tmpMaintenanceClause2.Append(Constants.cOrClause + Constants.cSingleOpen + tmpValue)

                                    tmpMaintenanceClause2.Append(Constants.cAndClause + "acmaint_date_type = 'M'" + Constants.cDoubleClose)

                                ElseIf srchMaintenanceItems.Maintenance_value2.Trim <> "" Then
                                    tmpMaintenanceClause2.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value2.Replace("acmaint_value1", "acmaint_complied_date"))
                                End If

                            End If

                            If srchMaintenanceItems.Maintenance_date2.ToUpper.Contains("DUE") And srchMaintenanceItems.Maintenance_time2.ToUpper.Contains("DATE") Then

                                If bAsReportedChk2 And srchMaintenanceItems.Maintenance_value2.Trim <> "" Then

                                    Dim tmpValue As String = srchMaintenanceItems.Maintenance_value2.Trim

                                    tmpValue = tmpValue.Replace("acmaint_value1", "acmaint_due_date")
                                    tmpValue = tmpValue.Replace(">", ">=")

                                    tmpMaintenanceClause2.Append(tmpValue)
                                    tmpMaintenanceClause2.Append(Constants.cAndClause + "acmaint_date_type IN ('D','Y')" + Constants.cSingleClose)

                                    Dim firstTick As Integer = tmpValue.IndexOf(Constants.cSingleQuote)
                                    Dim secondTick As Integer = 0
                                    Dim tmpDate As Date = Today

                                    If tmpValue.ToLower.Contains("between") Then
                                        secondTick = tmpValue.IndexOf(Constants.cSingleQuote, firstTick + 1)
                                        tmpDate = CDate(tmpValue.Substring(firstTick, secondTick - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    Else
                                        tmpDate = CDate(tmpValue.Substring(firstTick, tmpValue.Length - firstTick).Replace(Constants.cSingleQuote, Constants.cEmptyString))
                                    End If

                                    Dim tmpMonth As String = Month(tmpDate).ToString
                                    Dim tmpYear As String = Year(tmpDate).ToString

                                    Dim tmpMonthDate As String = Constants.cSingleQuote + tmpMonth + "/01/" + tmpYear + Constants.cSingleQuote

                                    If tmpValue.ToLower.Contains("between") Then
                                        Dim tmpStr As String = tmpValue.Substring(0, firstTick)
                                        tmpStr += tmpMonthDate + tmpValue.Substring(secondTick + 1, tmpValue.Length - secondTick - 1)
                                        tmpValue = tmpStr
                                    Else
                                        tmpValue = tmpValue.Substring(0, firstTick)
                                        tmpValue += tmpMonthDate
                                    End If

                                    tmpMaintenanceClause2.Append(Constants.cOrClause + Constants.cSingleOpen + tmpValue)

                                    tmpMaintenanceClause2.Append(Constants.cAndClause + "acmaint_date_type = 'M'" + Constants.cDoubleClose)

                                ElseIf srchMaintenanceItems.Maintenance_value2.Trim <> "" Then
                                    tmpMaintenanceClause2.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value2.Replace("acmaint_value1", "acmaint_due_date"))
                                End If

                            End If

                            If srchMaintenanceItems.Maintenance_date2.ToUpper.Contains("CW") And srchMaintenanceItems.Maintenance_time2.ToUpper.Contains("HOURS") Then
                                tmpMaintenanceClause2.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value2.Replace("acmaint_value1", "acmaint_complied_hrs"))
                            End If

                            If srchMaintenanceItems.Maintenance_date2.ToUpper.Contains("DUE") And srchMaintenanceItems.Maintenance_time2.ToUpper.Contains("HOURS") Then
                                tmpMaintenanceClause2.Append(Constants.cAndClause + srchMaintenanceItems.Maintenance_value2.Replace("acmaint_value1", "acmaint_due_hrs"))
                            End If

                            If Not bAsReportedChk2 Then
                                tmpMaintenanceClause2.Append(Constants.cAndClause + "acmaint_notes NOT LIKE '%as reported%'")
                            End If

                            tmpMaintenanceClause2.Append(Constants.cSingleClose)

                        End If

                        If Not String.IsNullOrEmpty(tmpMaintenanceClause1.ToString.Trim) Then

                            If Not String.IsNullOrEmpty(DynamicQueryString.Trim) Then
                                DynamicQueryString += Constants.cAndClause + tmpMaintenanceClause1.ToString
                            Else
                                DynamicQueryString = tmpMaintenanceClause1.ToString
                            End If

                        End If

                        If Not String.IsNullOrEmpty(tmpMaintenanceClause2.ToString.Trim) Then

                            If Not String.IsNullOrEmpty(DynamicQueryString.Trim) Then
                                DynamicQueryString += Constants.cAndClause + tmpMaintenanceClause2.ToString
                            Else
                                DynamicQueryString = tmpMaintenanceClause2.ToString
                            End If

                        End If

                    End If

                End If

                Dim onMarket As Boolean = False
                Dim offMarket As Boolean = False
                Dim writtenOff As Boolean = False

                If History Then
                    If Not IsNothing(Trim(Request("sMarketAddToWhereClause"))) Then
                        If Trim(Request("fromMarketSummary")) = "true" Or Trim(Request("fromHomePage")) = "true" Then
                            Dim AppendedString As String = ""
                            AppendedString = Trim(Request("sMarketAddToWhereClause"))
                            AppendedString = Replace(AppendedString, " equals ", "=")
                            AppendedString = Replace(AppendedString, "'", "''")
                            AppendedString = Replace(AppendedString, "?", "'")
                            If Trim(DynamicQueryString) <> "" Then
                                DynamicQueryString += " and " & AppendedString
                            Else
                                DynamicQueryString += AppendedString
                            End If

                            If Trim(Request("on_markets")) = "true" Then
                                onMarket = True
                            ElseIf Trim(Request("off_markets")) = "true" Then
                                offMarket = True
                            ElseIf Trim(Request("written_off")) = "true" Then
                                writtenOff = True
                            End If

                        End If
                    End If
                End If


                'Setting up Do Not Search Previous Registration  # in Session 
                NewSearchClass.SearchCriteriaDoNotSearchPrevRegNo = do_not_search_ac_prev_reg_no.Checked

                'Setting up Do Not Search Alt Ser # in Session
                NewSearchClass.SearchCriteriaSerDoNotSearchAlt = do_not_search_ac_alt_ser_no.Checked

                'Setting up Do Not Search Alt Ser # in Session
                NewSearchClass.SearchCriteriaRegExactMatch = ac_reg_no_exact_match.Checked


                'This function grabs the all the region information from the locaton control for Company Side
                DisplayFunctions.GetRegionInfoFromCommonControl("Company", BuildSearchString, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString, CompanyStateName)

                'This function grabs the all the region information from the locaton control for BaseSide
                DisplayFunctions.GetRegionInfoFromCommonControl("Base", BuildSearchString, BaseCountriesString, BaseTimeZoneString, BaseContinentString, BaseRegionString, BaseStateName)

                Dim BaseWhereString As String = ""
                Dim CompanyWhereString As String = ""

                'Drill through each state.
                If BaseRegionString <> "" Then
                    If BaseStateName <> "" Then
                        BaseWhereString = AdvancedQueryResults.BuildRegionWhereString("ac_aport_state_name", "ac_aport_country", Master.aclsData_Temp, BaseStateName, BaseCountriesString, BaseRegionString)

                        If DynamicQueryString <> "" Then
                            DynamicQueryString += " and (" & BaseWhereString & ")"
                        Else
                            DynamicQueryString += "(" & BaseWhereString & ")"
                        End If
                        BaseStateName = ""
                        BaseCountriesString = ""
                        BaseRegionString = ""
                    End If

                End If

                If CompanyRegionString <> "" Then

                    If CompanyStateName <> "" Then
                        CompanyWhereString = AdvancedQueryResults.BuildRegionWhereString("state_name", "comp_country", Master.aclsData_Temp, CompanyStateName, CompanyCountriesString, CompanyRegionString)

                        If DynamicQueryString <> "" Then
                            DynamicQueryString += " and (" & CompanyWhereString & ")"
                        Else
                            DynamicQueryString += "(" & CompanyWhereString & ")"
                        End If
                        CompanyStateName = ""
                        CompanyCountriesString = ""
                        CompanyRegionString = ""
                    End If

                End If

                'This is note filtering
                If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then
                    FilterWithWithoutNotes(DynamicQueryString, ModelsString)
                End If

                Session.Item("searchCriteria") = NewSearchClass

                Call commonLogFunctions.Log_User_Event_Data("UserSearch", ErrorReportingTypeString & " Search: " & clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(BuildSearchString, "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

                'Response.Write(DynamicQueryString & "!!!")
                If MarketEvent = False Then
                    Aircraft_Search(WeightClass, ManufacturerStr, AcSizeStr, ModelsString, ForSale_Flag, ForLease_Flag, OnExclusive_Flag,
                                    clsGeneral.clsGeneral.StripChars(SerialNo_Start, True), clsGeneral.clsGeneral.StripChars(SerialNo_End, True), do_not_search_ac_alt_ser_no.Checked,
                                    clsGeneral.clsGeneral.StripChars(RegistrationNo, True), ac_reg_no_exact_match.Checked, do_not_search_ac_prev_reg_no.Checked,
                                    LifeCycleStage_String, Status, Ownership_String, PreviouslyOwned_Flag,
                                    TypeString, AirframeTypeString, CombinedAirframeTypeString, MakeString,
                                    PageNumber, PageSort, LoadFromSession, BuildSearchString,
                                    Journal_Date, Journal_Type, Journal_Retail_Sales, Journal_New_Aircraft, Journal_Used_Aircraft, Journal_Subcat_Part2, Journal_Subcat_Part2_Operator, Journal_Subcat_Part3, Journal_Subcat_Part3_Operator,
                                    AC_Market_Status, DynamicQueryString, FinancialInstitution.Text, FinancialDate.Text, FinancialDocType.Text,
                                    CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString,
                                    Business, Helicopter, Commercial,
                                    BaseCountriesString, BaseContinentString, BaseRegionString, BaseStateName, CompanyStateName,
                                    JournalIDs, onMarket, offMarket, writtenOff)
                Else
                    MarketSearch(TypeString, WeightClass, ManufacturerStr, AcSizeStr, ModelsString, Format(CDate(StartDate), "MM/dd/yyyy hh:mm:ss tt"), "",
                                 clsGeneral.clsGeneral.StripChars(SerialNo_Start, True), clsGeneral.clsGeneral.StripChars(SerialNo_End, True), do_not_search_ac_alt_ser_no.Checked,
                                 clsGeneral.clsGeneral.StripChars(RegistrationNo, True), ac_reg_no_exact_match.Checked, do_not_search_ac_prev_reg_no.Checked,
                                 TypeString, AirframeTypeString, CombinedAirframeTypeString, MakeString, MarketCategory, MarketType,
                                 BuildSearchString, DynamicQueryString, LoadFromSession,
                                 FinancialInstitution.Text, FinancialDate.Text, FinancialDocType.Text,
                                 PageSort, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString,
                                 Business, Helicopter, Commercial,
                                 BaseCountriesString, BaseContinentString, BaseRegionString, BaseStateName, CompanyStateName,
                                 EventTypeOfSearch)
                End If

                'This has to be ran after the search to rebuild the attributes tab.
                'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then

                Dim MainContent As New ContentPlaceHolder
                If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
                    MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
                End If

                If History = False And MarketEvent = False Then
                    If attrBoolRan.Text = "true" Then
                        AttrTab.Controls.Remove(AttributesPanel)

                        Dim newPanel As New Panel

                        DealWithAttributeTab(MainContent.ClientID, newPanel)
                        AttrTab.Controls.Add(newPanel)
                        'End If
                    End If
                End If

            Else
                aircraft_attention.Text = "<br /><p align='center' class='padding'>Your search fields have invalid formatting. Please correct them and try again.</p><br /><br />"
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try
    End Sub

    Private Sub FilterWithWithoutNotes(ByRef AdvancedSearchQuery As String, ByVal modelsString As String)
        If MarketEvent = False And History = False Then
            If aircraftShowNotes.SelectedValue = "1" Or aircraftShowNotes.SelectedValue = "2" Then
                Dim jetnetIDs As New DataTable


                If Session.Item("localSubscription").crmServerSideNotes_Flag Then
                    Dim clientDate As String = ""
                    If IsDate(notesDate.Text) Then
                        clientDate = Year(notesDate.Text) & "-" & Month(notesDate.Text) & "-" & Day(notesDate.Text)
                    End If
                    jetnetIDs = Master.aclsData_Temp.SelectDistinctJetnetAircraftIDFromNotes(clientDate, "", modelsString)
                ElseIf Session.Item("localSubscription").crmCloudNotes_Flag Then
                    jetnetIDs = Master.aclsData_Temp.CloudNotesDetailsACDistinct(modelsString, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, clsGeneral.clsGeneral.StripChars(notesDate.Text, True))
                End If


                If Not IsNothing(jetnetIDs) Then
                    If jetnetIDs.Rows.Count > 0 Then
                        Dim filterString As String = ""
                        For Each r As DataRow In jetnetIDs.Rows
                            If filterString <> "" Then
                                filterString += ","
                            End If
                            filterString += r("lnote_jetnet_ac_id").ToString
                        Next

                        If filterString <> "" Then
                            If AdvancedSearchQuery <> "" Then
                                AdvancedSearchQuery += " and "
                            End If
                            AdvancedSearchQuery += " ac_id " & IIf(aircraftShowNotes.SelectedValue = "2", "NOT", "") & " in (" & filterString & ") "
                        End If
                    Else
                        If aircraftShowNotes.SelectedValue = "1" Then
                            If AdvancedSearchQuery <> "" Then
                                AdvancedSearchQuery += " and "
                            End If
                            AdvancedSearchQuery += " ac_id in (0) "
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
        PageNumber = selectedLI
    End Sub

    Public Sub MoveNext(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles next_.Click, previous.Click, next_all.Click, previous_all.Click
        Try
            If sender.id.ToString = "next_" Or sender.id.ToString = "bottom_next_" Then
                MovePage(True, False, False, False, False, next_.CommandArgument)
                next_.CommandArgument = next_.CommandArgument + 1
                previous.CommandArgument = next_.CommandArgument + 1
            ElseIf sender.id.ToString = "previous" Or sender.id.ToString = "bottom_previous" Then
                MovePage(False, True, False, False, False, previous.CommandArgument)
                next_.CommandArgument = next_.CommandArgument - 1
                previous.CommandArgument = next_.CommandArgument - 1
            ElseIf sender.id.ToString = "next_all" Or sender.id.ToString = "bottom_next_all" Then
                MovePage(False, False, True, False, False, next_all.CommandArgument - 1)
            ElseIf sender.id.ToString = "previous_all" Or sender.id.ToString = "bottom_previous_all" Then
                MovePage(False, False, False, True, False, 0)
            End If

            Dim jsStr As String = ""
            jsStr = SetUpScriptsAfterSearch(False, True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "collapsePanelJSMove", jsStr, True)
            If Not Page.ClientScript.IsClientScriptBlockRegistered("CursorNormalRemove") Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "CursorNormalRemove", "ChangeTheMouseCursorOnItemParentDocument('cursor_default" & IIf(Session.Item("isMobile"), " lowerLevel", "") & "');", True)
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
        Try
            'setting the javascript of the menus
            If Aircraft_Criteria.Visible = True Then
                'folders:
                folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
                folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

                folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
                folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")
                folders_submenu_dropdown.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")

                'sort
                sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
                sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

                sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
                sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")
                sort_submenu_dropdown.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")

                'page dropdown
                per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
                per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")

                per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
                per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")
                per_page_submenu_dropdown.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")

                'go to dropdown
                go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
                go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")

                go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
                go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")
                go_to_submenu_dropdown.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")

                'go to dropdown (top)
                go_to_dropdown_2.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown_2.ClientID & "', true);")
                go_to_dropdown_2.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown_2.ClientID & "', false);")

                go_to_submenu_dropdown_2.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown_2.ClientID & "', true);")
                go_to_submenu_dropdown_2.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown_2.ClientID & "', false);")
                go_to_submenu_dropdown_2.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")


                'view dropdown
                view_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
                view_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")

                view_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
                view_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")
                view_submenu_dropdown.Attributes.Add("onclick", "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');")

                'actions dropdown
                actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
                actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

                actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
                actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

                If lower_bar = True Then
                    'PanelCollapseEx.Enabled = False
                    Collapse_Panel.Visible = False
                    search_expand_text.Visible = False
                    help_text.Visible = False
                    sort_by_text.Visible = False
                    sort_by_dropdown.Visible = False
                    view_dropdown_.Visible = False

                Else
                    per_page_dropdown_.Visible = False
                    per_page_text.Visible = False
                    go_to_dropdown_.Visible = False
                    go_to_text.Visible = False
                    go_to_text_2.Visible = False
                    go_to_dropdown_cell_2.Visible = False
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub transaction_retail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles transaction_retail.CheckedChanged
        Try

            If transaction_retail.Checked Then
                journ_subcat_code_part2_operator.Visible = False
                journ_subcat_code_part3_operator.Visible = False
                journ_subcat_code_part2.Visible = False
                journ_subcat_code_part3.Visible = False

                journ_exclude_internal_transactions.Checked = True  ' added MSW - 7/29/19
                journ_exclude_internal_transactions.Enabled = False  ' added MSW - 7/29/19

            Else
                journ_subcat_code_part2_operator.Visible = True
                journ_subcat_code_part3_operator.Visible = True
                journ_subcat_code_part2.Visible = True
                journ_subcat_code_part3.Visible = True

                journ_exclude_internal_transactions.Checked = False  ' added MSW - 7/29/19
                journ_exclude_internal_transactions.Enabled = True  ' added MSW - 7/29/19
            End If

            FillTransactionType()

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub Reset_Form()
        ClearSavedSelection()

        If History = True Then
            Response.Redirect("Aircraft_Listing.aspx?h=1", False)
        ElseIf MarketEvent = True Then
            Response.Redirect("Aircraft_Listing.aspx?e=1", False)
        Else
            Response.Redirect("Aircraft_Listing.aspx", False)
        End If

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Function EventsListingPageQuery(ByVal models As String, ByVal start_date As String,
                                           ByVal end_date As String, ByVal aerodex_flag As Boolean,
                                           ByVal ac_id As Long, ByVal SerialNo_Start As String,
                                           ByVal SerialNo_End As String, ByVal DoNotSearchAltSer As Boolean,
                                           ByVal RegistrationNo As String, ByVal RegistrationNo_Exact As String,
                                           ByVal DoNotSearchPrevRegNo As Boolean, ByVal Make_String As String,
                                           ByVal Model_Type As String, ByVal Airframe_Type As String,
                                           ByVal CombinedAirframeTypeString As String,
                                           ByVal MarketCategory As String, ByVal MarketType As String,
                                           ByVal DynamicQueryString As String,
                                           ByVal financialInstitution As String, ByVal financial_doc_date As String,
                                           ByVal financial_doc_type As String, ByVal pageSort As String,
                                           ByVal CompanyCountry As String, ByVal CompanyState As String,
                                           ByVal CompanyTimeZone As String, ByVal CompanyContinentString As String,
                                           ByVal CompanyRegionString As String,
                                           ByVal Business As Boolean, ByVal Helicopter As Boolean, ByVal Commercial As Boolean,
                                           ByVal BaseCountriesString As String, ByVal BaseStatesString As String,
                                           ByVal BaseContinentString As String, ByVal BaseRegionString As String,
                                           ByVal EventTypeOfSearch As String, ByVal weight_class As String,
                                           ByVal ManufacturerName As String, ByVal AcSize As String)
        Dim sql As String = ""
        Dim market_sql As String = ""
        Dim market_sql_where As String = ""
        Dim i As Integer = 0
        Dim market_sql_from As String = ""
        Dim andQ As String = ""
        Dim aTempTable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            HttpContext.Current.Session.Item("MasterEvents") = "" 'Whole Search
            HttpContext.Current.Session.Item("MasterAircraftEventsWhere") = "" 'Where Only
            HttpContext.Current.Session.Item("MasterAircraftEventsFrom") = "" 'From Variable Only
            'This is what needs to happen.
            'If we're running a search that is not aircraft.
            'Not all of the search parameters apply.
            'Rather than go through and weed out throughout the query building process.
            'I thought it would be substantially easier to read/edit if we just went ahead and
            'Weeded them out by clearing the values that do not pertain at the very begining.

            Select Case UCase(EventTypeOfSearch)
                Case "AIRCRAFT"
          'All search values apply (currently) for this search.
                Case "WANTED"
                    'This means that any of the dynamic query string, as well as any of the individual AC fields do not apply
                    'For this search.

                    'models: This one is fine, Models pertain to the wanted.
                    'start_date As String: Start Date is Fine, this pertains to events
                    'end_date As String: Start Date is Fine, this pertains to events
                    'aerodex_flag As Boolean: This flag pertains to subscription information and should not be cleared.
                    ac_id = 0 'This can be cleared. Wanted are model based, AC Ids do not apply.
                    SerialNo_Start = "" 'individual AC parameter
                    SerialNo_End = "" 'individual AC parameter
                    'DoNotSearchAltSer As Boolean: Keeping this one as is is fine as it won't pertain if the top two are cleared.
                    RegistrationNo = ""
                    'RegistrationNo_Exact As String: Keeping this one as is is fine as it won't pertain if the top Reg# is cleared.
                    'DoNotSearchPrevRegNo As Boolean: Keeping this one as is is fine as it won't pertain if the top Reg# is cleared.
                    'Make_String As String: This can be kept around. Wanted use models.
                    'Model_Type As String: This can be kept around. Wanted use models.
                    'Airframe_Type As String: This can be kept around. Wanted use models.
                    'CombinedAirframeTypeString as string: This can be kept around. Wanted use models.
                    MarketCategory = "'COMPANY/CONTACT'" 'This is the only valid category for a wanted event search.
                    MarketType = "'NEWWA'" 'We might as well hardcode this to circumvent any possible way to return a different type on this type of search.
                    DynamicQueryString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    'pageSort As String: Page sorting variable, can be kept.
                    CompanyCountry = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyState = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyTimeZone = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyContinentString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyRegionString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    'Business As Boolean: These are subscription variables and are important to keep regardless of search.
                    'Helicopter As Boolean: These are subscription variables and are important to keep regardless of search. 
                    'Commercial As Boolean: These are subscription variables and are important to keep regardless of search. 
                    BaseCountriesString = "" 'This can be cleared. None of the dynamic search makes sense here. 
                    BaseStatesString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    BaseContinentString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    BaseRegionString = "" 'This can be cleared. None of the dynamic search makes sense here.
          'EventTypeOfSearch: This needs to persist. It's the type of search.
                Case "COMPANY"
                    'This means that any of the dynamic query string, as well as any of the individual AC/Model fields do not apply
                    'For this search.

                    models = "" 'This field needs to be cleared, models do not pertain to a company event search.
                    'start_date As String: Start Date is Fine, this pertains to events
                    'end_date As String: Start Date is Fine, this pertains to events
                    'aerodex_flag As Boolean: This flag pertains to subscription information and should not be cleared.
                    ac_id = 0 'This can be cleared. AC Ids do not apply to company events.
                    SerialNo_Start = "" 'individual AC parameter
                    SerialNo_End = "" 'individual AC parameter
                    'DoNotSearchAltSer As Boolean: Keeping this one as is is fine as it won't pertain if the top two are cleared.
                    RegistrationNo = ""
                    'RegistrationNo_Exact As String: Keeping this one as is is fine as it won't pertain if the top Reg# is cleared.
                    'DoNotSearchPrevRegNo As Boolean: Keeping this one as is is fine as it won't pertain if the top Reg# is cleared.
                    Make_String = "" 'This is cleared. Company events do not need make strings.
                    Model_Type = "" 'This is cleared. Company events do not need model strings.
                    CombinedAirframeTypeString = "" 'This can be cleared. Company Events do not need model strings.
                    Airframe_Type = "" 'This is cleared. Company events do not need airframe types strings.
                    MarketCategory = "'COMPANY/CONTACT'" 'This is the only valid category for a company event search.
                    MarketType = "'CFNC'" 'We might as well hardcode this to avoid issues. This is the only type they see on this search.
                    DynamicQueryString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    'pageSort As String: Page sorting variable, can be kept.
                    CompanyCountry = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyState = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyTimeZone = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyContinentString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    CompanyRegionString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    'Business As Boolean: These are subscription variables and are important to keep regardless of search.
                    'Helicopter As Boolean: These are subscription variables and are important to keep regardless of search. 
                    'Commercial As Boolean: These are subscription variables and are important to keep regardless of search. 
                    BaseCountriesString = "" 'This can be cleared. None of the dynamic search makes sense here. 
                    BaseStatesString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    BaseContinentString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    BaseRegionString = "" 'This can be cleared. None of the dynamic search makes sense here.
                    'EventTypeOfSearch: This needs to persist. It's the type of search.
            End Select



            market_sql = " select distinct priorev_id, priorev_journ_id, priorev_subject as apev_subject,  priorev_description as apev_description, priorev_comp_id, priorev_contact_id,  "
            market_sql += " priorev_entry_date as apev_action_date , priorev_entry_date as apev_entry_date, "


            If EventTypeOfSearch.ToUpper.Contains("WANTED") Then
                market_sql += " amod_airframe_type_code, amod_type_code, amod_id, amod_make_name, amod_model_name, "
                market_sql += " '0' as ac_ser_no_sort, '' as ac_reg_no, '0' as ac_id, '' as ac_year,  '' as ac_ser_no_full,   "
            ElseIf EventTypeOfSearch.ToUpper.Contains("COMPANY") Then
                market_sql += " '' as amod_airframe_type_code, '' as amod_type_code, '0' as amod_id, '' as amod_make_name, '' as amod_model_name, "
                market_sql += " '0' as ac_ser_no_sort, '' as ac_reg_no, '0' as ac_id, '' as ac_year,  '' as ac_ser_no_full,   "
            Else 'If UCase(EventTypeOfSearch) = "AIRCRAFT" or fail safe to always use this if this isn't sent
                market_sql += " amod_airframe_type_code, amod_type_code, amod_id, amod_make_name, amod_model_name, "
                market_sql += " ac_ser_no_sort, ac_reg_no, ac_id, ac_year,  ac_ser_no_full,   "
            End If

            market_sql = market_sql & " 0 as client_id "

            market_sql_from = "from priority_events with(NOLOCK) "


            market_sql_from = market_sql_from & " inner join Priority_Events_Category WITH(NOLOCK) on priorev_category_code=priorevcat_category_code"

            If EventTypeOfSearch.ToUpper.Contains("WANTED") Then
                market_sql_from = market_sql_from & " LEFT OUTER JOIN Aircraft_Model_Wanted ON amwant_id = priorev_amwant_id AND priorev_amwant_id > 0 "
                market_sql_from = market_sql_from & " LEFT OUTER JOIN Company ON comp_id = priorev_comp_id AND comp_journ_id = 0 AND priorev_comp_id > 0 "
                market_sql_from = market_sql_from & " LEFT OUTER JOIN aircraft_model WITH(NOLOCK) on amwant_amod_id = amod_id "
            ElseIf EventTypeOfSearch.ToUpper.Contains("COMPANY") Then
                market_sql_from = market_sql_from & " LEFT OUTER JOIN Company ON comp_id = priorev_comp_id AND comp_journ_id = 0 AND priorev_comp_id > 0"

            Else
                If CompanyState <> "" Or CompanyContinentString <> "" Or CompanyTimeZone <> "" Or CompanyCountry <> "" Or CompanyRegionString <> "" Or InStr(DynamicQueryString, "actype_") > 0 Or InStr(DynamicQueryString, "comp_") > 0 Or InStr(DynamicQueryString, "contact_") > 0 Or InStr(DynamicQueryString, "cref") > 0 Then
                    market_sql_from = market_sql_from & " LEFT OUTER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON (ac_id = priorev_ac_id AND ac_journ_id = 0 AND priorev_ac_id > 0) "
                Else
                    market_sql_from = market_sql_from & " LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (ac_id = priorev_ac_id AND ac_journ_id = 0 AND priorev_ac_id > 0) "
                End If
            End If

            HttpContext.Current.Session.Item("MasterAircraftEventsFrom") = market_sql_from
            market_sql += market_sql_from

            market_sql += " WHERE "

            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                market_sql_where = " priorevcat_category <> 'Market Status' "
                market_sql_where += " and priorev_category_code not IN ('CA','EXOFF','EXON','MA','OM','OMNS','SALEP','SC','SPTOIM') "
            End If

            If MarketCategory <> "" Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If



                If MarketType <> "" Then
                    If InStr(Trim(MarketType), "SC^") > 0 Then
                        market_sql_where += andQ & " priorevcat_category IN (" & MarketCategory & ",'Market Status') "
                    Else
                        market_sql_where += andQ & "( priorevcat_category IN (" & MarketCategory & ") "
                    End If
                Else
                    market_sql_where += andQ & " priorevcat_category IN (" & MarketCategory & ") "
                End If
            End If


            If MarketType <> "" Then

                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If


                If MarketCategory <> "" Then
                    If InStr(Trim(MarketType), "SC^") > 0 Then
                        market_sql_where += andQ & " ( priorevcat_category_code IN (" & MarketType & ")  "
                    Else
                        market_sql_where += andQ & " priorevcat_category_code IN (" & MarketType & ")) "
                    End If
                Else
                    market_sql_where += andQ & " priorevcat_category_code IN (" & MarketType & ") "
                End If

                If InStr(Trim(MarketType), "SC^") > 0 Then
                    Dim temp_market_types As Array = Split(Replace(MarketType, "'", ""), "SC^")

                    For i = 0 To UBound(temp_market_types)
                        If InStr(Trim(temp_market_types(i)), "Aircraft Back In Service") > 0 Then
                            market_sql_where += " OR (priorevcat_category_code IN ('SC') and priorev_description like '%Aircraft Back In Service%' )"
                        ElseIf InStr(Trim(temp_market_types(i)), "Written Off") > 0 Then
                            market_sql_where += " OR (priorevcat_category_code IN ('SC') and priorev_description like '%to: Written Off%' )"
                        ElseIf InStr(Trim(temp_market_types(i)), "Withdrawn From Use") > 0 Then
                            market_sql_where += " OR (priorevcat_category_code IN ('SC') and priorev_description like '%to: Withdrawn From Use%' )"
                        End If
                    Next


                    market_sql_where += " ) "

                End If


            End If


            If EventTypeOfSearch.ToUpper.Contains("AIRCRAFT") Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                'When doing an aircraft search, these results are not to be returned.
                'This is an extra catch to make sure that in the event they pick all - it's still taken into consideration.
                market_sql_where += andQ & " priorevcat_category_code NOT IN ('NEWWA','CFNC') "
            End If

            If Not String.IsNullOrEmpty(financialInstitution.Trim) Then

                If market_sql_where <> "" Then
                    market_sql_where += " and "
                End If

                market_sql_where += " ( "
                market_sql_where += " EXISTS (SELECT NULL FROM "
                market_sql_where += " Aircraft_Document "
                market_sql_where += " WHERE (adoc_infavor_comp_id IN "
                market_sql_where += "(" + financialInstitution.Trim + ")) "
                market_sql_where += " AND (adoc_ac_id = ac_id) "

                market_sql_where += " AND (EXISTS (SELECT NULL FROM Aircraft_Reference "
                market_sql_where += " WHERE (cref_comp_id = adoc_infavor_comp_id) "
                market_sql_where += " AND (cref_ac_id = ac_id) AND (cref_journ_id = 0) "
                market_sql_where += " AND (cref_contact_type in ('00','08','78','97'))))"

                If Not String.IsNullOrEmpty(financial_doc_date.Trim) Then
                    market_sql_where += " AND (" + financial_doc_date.Trim + ")"
                End If

                If Not String.IsNullOrEmpty(financial_doc_type.Trim) Then
                    market_sql_where += " AND (" + financial_doc_type.Trim + ")"
                End If

                market_sql_where += " ))"

            ElseIf Not String.IsNullOrEmpty(financial_doc_date.Trim) Then

                If market_sql_where <> "" Then
                    market_sql_where += " AND "
                End If

                market_sql_where += " (EXISTS (SELECT NULL FROM Aircraft_Document WHERE (adoc_ac_id = ac_id)"
                market_sql_where += " AND (" + financial_doc_date.Trim + ")"

                If Not String.IsNullOrEmpty(financial_doc_type.Trim) Then
                    market_sql_where += " AND (" + financial_doc_type.Trim + ")"
                End If

                market_sql_where += " ))"

            ElseIf Not String.IsNullOrEmpty(financial_doc_type.Trim) Then

                If market_sql_where <> "" Then
                    market_sql_where += " AND "
                End If

                market_sql_where += " (EXISTS (SELECT NULL FROM Aircraft_Document WHERE (adoc_ac_id = ac_id)"
                market_sql_where += " AND (" + financial_doc_type.Trim + ")"

                market_sql_where += " ))"

            End If

            'SER NO RANGE
            'ac_ser_no_value BETWEEN 27 AND 33) OR (ac_alt_ser_no_value BETWEEN 27 AND 33

            'SER NO SINGLE
            '((ac_ser_no_full = '27') OR (ac_ser_no = '27') OR (ac_ser_no_value = 27) OR (ac_alt_ser_no_full = '27') OR (ac_alt_ser_no = '27') OR (ac_alt_ser_no_value = 27))


            Dim sHoldSerial As String = ""
            Dim nloop As Integer = 0
            Dim serNbrArray() As String = Nothing
            Dim sArrayItem As String = ""
            Dim nArrayItem As String = ""

            If Not String.IsNullOrEmpty(SerialNo_Start.Trim) And Not String.IsNullOrEmpty(SerialNo_End.Trim) Then

                If Not String.IsNullOrEmpty(market_sql_where.Trim) Then
                    market_sql_where += " AND "
                End If

                If IsNumeric(SerialNo_Start) And IsNumeric(SerialNo_End) Then

                    market_sql_where += "( (ac_ser_no_value BETWEEN " + SerialNo_Start + " AND " + SerialNo_End + ")"

                    If DoNotSearchAltSer Then
                        market_sql_where += ")"
                    Else
                        market_sql_where += " OR (ac_alt_ser_no_value BETWEEN " + SerialNo_Start + " AND " + SerialNo_End + ") )"
                    End If

                Else

                    market_sql_where += "( ac_ser_no_full BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End
                    market_sql_where += "' OR ac_ser_no BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End + "'"

                    If DoNotSearchAltSer Then
                        market_sql_where += ")"
                    Else
                        market_sql_where += " OR ac_alt_ser_no_full BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End
                        market_sql_where += "' OR ac_alt_ser_no BETWEEN '" + SerialNo_Start + "' AND '" + SerialNo_End + "')"
                    End If

                End If ' IsNumeric(nSerialFrom) And IsNumeric(nSerialTo)

            ElseIf Not String.IsNullOrEmpty(SerialNo_Start.Trim) Then
                sHoldSerial = SerialNo_Start.Trim
            ElseIf Not String.IsNullOrEmpty(SerialNo_End.Trim) Then
                sHoldSerial = SerialNo_End.Trim
            End If

            If Not String.IsNullOrEmpty(sHoldSerial.Trim) Then ' Only Valid if a Single text box was filled in. Start Or End

                sHoldSerial = sHoldSerial.Replace(", ", ",") ' remove any spaces after (comma)
                sHoldSerial = "'" + sHoldSerial.Replace(",", "','") + "'"

                serNbrArray = sHoldSerial.Split(",")

                If Not String.IsNullOrEmpty(market_sql_where.Trim) Then
                    market_sql_where += " AND "
                End If

                market_sql_where += "("

                For nloop = 0 To UBound(serNbrArray)
                    If Not String.IsNullOrEmpty(serNbrArray(nloop)) Then
                        sArrayItem = serNbrArray(nloop).Trim
                        nArrayItem = sArrayItem.Replace("'", "").Trim ' Strip off any single quotes for numeric test

                        If IsNumeric(nArrayItem) And Not sArrayItem.Contains("-") Then ' if this array item is a number

                            market_sql_where += "ac_ser_no_full = " + sArrayItem
                            market_sql_where += " OR ac_ser_no = " + sArrayItem
                            market_sql_where += " OR ac_ser_no_value = " + nArrayItem

                            If Not DoNotSearchAltSer Then
                                market_sql_where += " OR ac_alt_ser_no_full = " + sArrayItem
                                market_sql_where += " OR ac_alt_ser_no = " + sArrayItem
                                market_sql_where += " OR ac_alt_ser_no_value = " + nArrayItem
                            End If

                        Else

                            market_sql_where += "ac_ser_no_full = " + sArrayItem
                            market_sql_where += " OR ac_ser_no = " + sArrayItem

                            If Not DoNotSearchAltSer Then
                                market_sql_where += " OR ac_alt_ser_no_full = " + sArrayItem
                                market_sql_where += " OR ac_alt_ser_no = " + sArrayItem
                            End If

                        End If

                        If UBound(serNbrArray) >= 1 And nloop < UBound(serNbrArray) Then
                            market_sql_where += " OR " ' add or clauses for each item
                        End If

                    End If

                Next ' nLoop

                market_sql_where += ")"

            End If


            'REG NO (not exact)
            '((ac_reg_no_search LIKE 'N415%') OR (ac_prev_reg_no LIKE 'N415%'))

            'REG NO (exact)
            '((ac_reg_no_search = 'N415CT') OR (ac_prev_reg_no = 'N415CT'))


            If RegistrationNo <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " and "
                End If

                market_sql_where += "( "

                If RegistrationNo_Exact = True Then
                    market_sql_where += " ac_reg_no_search = '" & Replace(RegistrationNo, "-", "") & "' "
                    HttpContext.Current.Session.Item("SearchString") += " REG NO = '" & RegistrationNo & "'<br />"
                Else
                    market_sql_where += " ac_reg_no_search like '%" & Replace(RegistrationNo, "-", "") & "%' "
                    HttpContext.Current.Session.Item("SearchString") += " REG NO LIKE '" & RegistrationNo & "%'<br />"
                End If


                If DoNotSearchPrevRegNo = False Then
                    If RegistrationNo_Exact = True Then
                        market_sql_where += " or ac_prev_reg_no = '" & RegistrationNo & "' "
                        HttpContext.Current.Session.Item("SearchString") += " PREV REG NO = '" & RegistrationNo & "'<br />"
                    Else
                        market_sql_where += " or ac_prev_reg_no like '" & RegistrationNo & "%' "
                        HttpContext.Current.Session.Item("SearchString") += " PREV REG NO LIKE '%" & RegistrationNo & "%'<br />"
                    End If
                End If
                market_sql_where += " )"
            End If


            If market_sql_where <> "" Then
                andQ = " and"
            Else
                andQ = ""
            End If
            market_sql_where += andQ & " (priorev_hide_flag = 'N') "

            Dim HoldClsSubscription As New crmSubscriptionClass

            HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
            HoldClsSubscription.crmBusiness_Flag = Business
            HoldClsSubscription.crmCommercial_Flag = Commercial
            HoldClsSubscription.crmHelicopter_Flag = Helicopter
            HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
            HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
            HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag


            If EventTypeOfSearch.ToUpper.Contains("WANTED") Then
                market_sql_where += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, False)
            ElseIf EventTypeOfSearch.ToUpper.Contains("COMPANY") Then
                market_sql_where += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, True, False)
            Else
                market_sql_where += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True)
            End If


            If models <> "" Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                market_sql_where += andQ & " amod_id in (" & models & ") "
            Else

                If CombinedAirframeTypeString <> "" Then
                    Dim TemporaryAirframeWhere As String = ""
                    'The structure looks like this:
                    'AirType|AirframeType, 
                    'First let's add the and if we need it:

                    Dim BrokenApartTypes As Array = Split(CombinedAirframeTypeString, ",")
                    'If UBound(BrokenApartTypes) > 0 Then
                    For MultipleSelectionCount = 0 To UBound(BrokenApartTypes)
                        Dim FinalSeperationType As Array = Split(BrokenApartTypes(MultipleSelectionCount), "|")
                        If UBound(FinalSeperationType) = 1 Then
                            'This means there's a type, airframe type
                            If TemporaryAirframeWhere <> "" Then
                                TemporaryAirframeWhere += " or "
                            End If
                            TemporaryAirframeWhere += " (amod_type_code in ('" & Trim(FinalSeperationType(0)) & "') and amod_airframe_type_code in ('" & Trim(FinalSeperationType(1)) & "')) "
                        End If
                    Next
                    'End If

                    If TemporaryAirframeWhere <> "" Then
                        If market_sql_where <> "" Then
                            market_sql_where += " and "
                        End If
                        TemporaryAirframeWhere = " ( " & TemporaryAirframeWhere & " ) "
                        market_sql_where += TemporaryAirframeWhere
                    End If

                End If

                If Make_String <> "" Then
                    If market_sql_where <> "" Then
                        market_sql_where += " and "
                    End If
                    market_sql_where += " amod_make_name in (" & Make_String & ")"
                    'HttpContext.Current.Session.Item("SearchString") += " MAKE NAME IN (" & Make_String & ")<br />"
                End If
            End If

            If ac_id <> 0 Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                market_sql_where += andQ & " ac_id = " & ac_id & " "
            End If

            ''''''''''''''''''''''''''''base location
            'Base Continent
            If BaseContinentString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " AND"
                End If
                market_sql_where += " ac_country_continent_name in (" & BaseContinentString & ") "
            End If

            'base state
            If BaseStatesString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " and "
                End If
                market_sql_where += " ac_aport_state_name in (" & BaseStatesString & ") "
            End If

            'Base Countries
            If BaseCountriesString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " and "
                End If
                market_sql_where += " ac_aport_country in (" & BaseCountriesString & ") "
            End If

            'base regions
            If BaseRegionString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " AND "
                End If
                market_sql_where += " ac_aport_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & BaseRegionString & ")) "

                If BaseStatesString <> "" Then
                    market_sql_where += " and ac_aport_state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & BaseRegionString & ")) "
                End If
            End If

            '''''''''''''''''''''''''company region/location'''''''''''''''''''''''''''''


            ' check the state
            If CompanyState <> "" And market_sql_where <> "" Then
                market_sql_where += " AND state_name IN (" & CompanyState & ")"
            ElseIf CompanyState <> "" Then
                market_sql_where += " state_name IN (" & CompanyState & ")"
            End If

            If CompanyTimeZone <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " and "
                End If
                market_sql_where += " comp_timezone in (SELECT tzone_name FROM Timezone where tzone_id in (" & CompanyTimeZone & ")) "
            End If

            'Continent
            If CompanyContinentString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " AND"
                End If
                market_sql_where += " country_continent_name in (" & CompanyContinentString & ") "
            End If

            ' check the country
            If CompanyCountry <> "" And market_sql_where <> "" Then
                market_sql_where += " AND comp_country in (" & CompanyCountry & ") "
            ElseIf CompanyCountry <> "" Then
                market_sql_where += " comp_country in (" & CompanyCountry & ") "
            End If
            'regions
            If CompanyRegionString <> "" Then
                If market_sql_where <> "" Then
                    market_sql_where += " AND "
                End If
                market_sql_where += " comp_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & CompanyRegionString & ")) "

                If CompanyState <> "" Then
                    market_sql_where += " and state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & CompanyRegionString & ")) "
                End If
            End If



            If start_date <> "" Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                market_sql_where += andQ & " priorev_entry_date >= '" & start_date & "'"
            End If

            If end_date <> "" Then
                If market_sql_where <> "" Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                market_sql_where += andQ & " priorev_entry_date <= '" & end_date & "'"
            End If

            If Not String.IsNullOrEmpty(weight_class.Trim) Then
                If Not String.IsNullOrEmpty(market_sql_where.Trim) Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                If weight_class.Contains(Constants.cValueSeperator) Then
                    market_sql_where += andQ + " amod_weight_class IN ('" + weight_class.Trim + "') "
                Else
                    market_sql_where += andQ + " amod_weight_class = '" + weight_class.Trim + "' "
                End If

            End If

            If Not String.IsNullOrEmpty(ManufacturerName.Trim) Then
                If Not String.IsNullOrEmpty(market_sql_where.Trim) Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                If ManufacturerName.Contains(Constants.cValueSeperator) Then
                    market_sql_where += andQ + " amod_manufacturer_common_name IN ('" + ManufacturerName.Trim + "') "
                Else
                    market_sql_where += andQ + " amod_manufacturer_common_name = '" + ManufacturerName.Trim + "' "
                End If

            End If

            If Not String.IsNullOrEmpty(AcSize.Trim) Then
                If Not String.IsNullOrEmpty(market_sql_where.Trim) Then
                    andQ = " and"
                Else
                    andQ = ""
                End If
                If AcSize.Contains(Constants.cValueSeperator) Then
                    market_sql_where += andQ + " amod_jniq_size IN ('" + AcSize.Trim + "') "
                Else
                    market_sql_where += andQ + " amod_jniq_size = '" + AcSize.Trim + "' "
                End If

            End If

            If UCase(EventTypeOfSearch) = "AIRCRAFT" Then
                If DynamicQueryString <> "" Then
                    If market_sql_where <> "" Then
                        market_sql_where += " and "
                    End If
                    market_sql_where += DynamicQueryString
                End If
            End If

            HttpContext.Current.Session.Item("MasterAircraftEventsWhere") = market_sql_where

            market_sql_where += " order by "

            If pageSort = "" Then
                market_sql_where += " amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort " 'by priorev_entry_date desc,amod_make_name, amod_model_name, ac_ser_no"
            Else
                market_sql_where += pageSort
            End If

            sql = market_sql.Trim + " " + market_sql_where.Trim

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b style='color:#ff0000;'>Market_Search_Evo(ByVal models As String, ByVal start_date As String, ByVal aerodex_flag As Boolean, ByVal ac_id As Integer, ByVal categories As String, ByVal types As String)</b><br />" & sql


            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            HttpContext.Current.Session.Item("MasterEvents") = sql
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            Try
                aTempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
            End Try


        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): (" + sql.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return aTempTable
        aTempTable = Nothing

    End Function

    Public Sub MarketSearch(ByVal TypeString As String, ByVal WeightClass As String, ByVal ManufacturerName As String,
                            ByVal AcSize As String, ByVal ModelsString As String,
                            ByVal start_date As String, ByVal end_date As String, ByVal events_serial_number_from As String,
                            ByVal events_serial_number_to As String, ByVal DoNotSearchAltSerNo As Boolean, ByVal events_registration_number As String,
                            ByVal events_registration_number_exact_match As Boolean, ByVal events_do_not_search_prev_reg As Boolean,
                            ByVal Model_Type_String As String, ByVal Airframe_Type_String As String, ByVal CombinedAirframeTypeString As String,
                            ByVal Make_String As String, ByVal MarketCategory As String, ByVal MarketType As String, ByVal TextStringDisplay As String,
                            ByVal DynamicQueryString As String, ByVal bindFromSession As Boolean,
                            ByVal FinancialInstitution As String, ByVal FinancialDate As String, ByVal FinancialDocType As String,
                            ByVal orderBy As String, ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String,
                            ByVal CompanyContinentString As String, ByVal CompanyRegionString As String,
                            ByVal Business As Boolean, ByVal Helicopter As Boolean, ByVal Commercial As Boolean,
                            ByVal BaseCountriesString As String, ByVal BaseContinentString As String, ByVal BaseRegionString As String,
                            ByVal BaseStateName As String, ByVal CompanyStateName As String, ByVal EventTypeOfSearch As String)
        Try
            Dim Results_Table As New DataTable
            Dim RecordsPerPage As Integer = 0

            If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
            End If
            ' If ModelsString <> "" Then
            HttpContext.Current.Session.Item("SearchString") = TextStringDisplay

            Initial(False)
            aircraft_attention.Text = ""

            If bindFromSession = True And Not IsNothing(Session.Item("Aircraft_Master")) Then
                Results_Table = Session.Item("Aircraft_Master")
            Else
                Results_Table = EventsListingPageQuery(ModelsString, start_date, end_date, False, 0,
                                                       events_serial_number_from, events_serial_number_to, DoNotSearchAltSerNo,
                                                       events_registration_number, events_registration_number_exact_match, events_do_not_search_prev_reg,
                                                       Make_String, Model_Type_String, Airframe_Type_String, CombinedAirframeTypeString, MarketCategory, MarketType,
                                                       DynamicQueryString, FinancialInstitution, FinancialDate, FinancialDocType, orderBy,
                                                       CompanyCountriesString, CompanyStateName, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString,
                                                       Business, Helicopter, Commercial,
                                                       BaseCountriesString, BaseStateName, BaseContinentString, BaseRegionString,
                                                       EventTypeOfSearch, WeightClass, ManufacturerName, AcSize)
            End If



            If Not IsNothing(Results_Table) Then
                Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
                If Results_Table.Rows.Count > 0 Then
                    next_.CommandArgument = "1"
                    previous.CommandArgument = "0"

                    Select Case UCase(EventTypeOfSearch)
                        Case "WANTED" 'This happens if a wanted event search is ran.
                            EventsDataGrid.Columns(0).Visible = True 'Enable the Make Row. Row 0.
                            EventsDataGrid.Columns(1).Visible = False 'Disable Year Row. Row 1
                            EventsDataGrid.Columns(2).Visible = False 'Disable Ser # Row. Row 2
                            EventsDataGrid.Columns(3).Visible = False 'Disable Reg # Row. Row 3
                        Case "COMPANY" 'This happens if a company event search is ran.
                            EventsDataGrid.Columns(0).Visible = False 'Disable Make Row. Row 0
                            EventsDataGrid.Columns(1).Visible = False 'Disable Year Row. Row 1
                            EventsDataGrid.Columns(2).Visible = False 'Disable Ser # Row. Row 2
                            EventsDataGrid.Columns(3).Visible = False 'Disable Reg # Row. Row 3
                        Case "AIRCRAFT" 'This basically just resets the datagrid to normal if an AC Event search is ran.
                            EventsDataGrid.Columns(0).Visible = True 'Disable Make Row. Row 0
                            EventsDataGrid.Columns(1).Visible = True 'Disable Year Row. Row 1
                            EventsDataGrid.Columns(2).Visible = True 'Disable Ser # Row. Row 2
                            EventsDataGrid.Columns(3).Visible = True 'Disable Reg # Row. Row 3
                    End Select
                    Session.Item("Aircraft_Master") = Results_Table

                    'Added this on 07/01/2015 - This is going to reset the current page index whenever the datagrid listing is active
                    'and a new search occurs.
                    EventsDataGrid.CurrentPageIndex = 0

                    If Session.Item("isMobile") = False Then
                        EventsDataGrid.PageSize = RecordsPerPage
                    Else
                        EventsDataGrid.AllowPaging = False
                        SetPagingButtons(False, False)
                        record_count.Text = ""
                        bottom_record_count.Text = ""
                    End If
                    EventsDataGrid.Visible = True
                    EventsDataGrid.DataSource = Results_Table
                    EventsDataGrid.DataBind()

                    criteria_results.Text = Results_Table.Rows.Count & " Results"
                    ' Criteria_Bar2.criteria_results.Text = Results_Table.Rows.Count & " Results"
                    If Session.Item("isMobile") = False Then
                        record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
                        bottom_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)

                        'This will fill up the dropdown bar with however many pages.
                        If Results_Table.Rows.Count > RecordsPerPage Then
                            Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                            'Criteria_Bar2.Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                            SetPagingButtons(False, True)
                            'Criteria_Bar2.SetPagingButtons(False, True)
                        Else
                            Fill_Page_To_To_Dropdown(1)
                            SetPagingButtons(False, False)
                            'Criteria_Bar2.SetPagingButtons(False, False)
                        End If
                    End If


                    'PanelCollapseEx.Collapsed = True
                    Results_Table = Nothing
                    'Criteria_Bar2.events_PanelCollapseEx.Collapsed = True
                    Results_Table = Nothing
                Else
                    Aircraft_Bottom_Paging.Visible = False
                    aircraft_attention.Text = "<br /><p align='center'>Your search did not return any results.</p><br />"
                    criteria_results.Text = Results_Table.Rows.Count & " Results"

                    record_count.Text = "Showing 0 Results"


                    EventsDataGrid.Visible = False
                End If
            Else
                'And that there was an error on the data side.
                If Not IsNothing(masterPage) Then
                    masterPage.LogError("Market_Search() Aircraft_Listing.aspx.vb (" & ErrorReportingTypeString & "): " & masterPage.aclsData_Temp.class_error)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): MasterPageNotEstablished"
                End If

                Aircraft_Bottom_Paging.Visible = False
                ' aircraft_attention.Text = "<br /><p class='padding'><b>No Results Found. Please refine your search and try again.</b></p><br /><br />"
                aircraft_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
                If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
                    aircraft_attention.Text += masterPage.aclsData_Temp.class_error
                End If
                masterPage.aclsData_Temp.class_error = ""
                criteria_results.Text = "0 Results"
                SetPagingButtons(False, False)
                record_count.Text = ""
            End If

            If Page.IsPostBack Then 'Running on every search except folders.
                If Not Page.ClientScript.IsClientScriptBlockRegistered("collapsePanelJS") Then
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(EventSearchUpdatePanel, Me.GetType(), "collapsePanelJS", SetUpScriptsAfterSearch(True, True), True)
                End If
            Else 'Run on folders.
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "collapsePanelJS", "$(""#" & Collapse_Panel.ClientID & """).hide();$('#" & ControlImage.ClientID & "').attr('src', '../images/search_expand.jpg');SetUpSlider();", True)
            End If

            If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                    Dim displaySearch As String = ""
                    If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                            displaySearch = HttpContext.Current.Session.Item("SearchString").Replace(vbNewLine, " ")
                            displaySearch = displaySearch.Replace(vbCrLf, " ")
                            displaySearch = displaySearch.Replace(vbEmpty, " ")

                        End If
                    End If

                    masterPage.SetStatusText(displaySearch, True)
                End If
            End If

            folderInformationUpdate.Update()
            listingUpdatePanel.Update()

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try

            aircraft_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"

            If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
                aircraft_attention.Text += "Query: " & Session.Item("MasterEvents").ToString & " " & ex.Message.ToString
            End If

        End Try



    End Sub

    Public Function LinkOutEventsCompanies(ByVal Description As String, ByVal CompID As Long, ByVal ContactID As Long) As String
        Dim returnString As String = ""

        Try

            Dim tempTable As New DataTable
            Dim compName As String = ""
            Dim compLink As String = ""

            'This is going to look for the company ID, then perform a search on the company Name.
            If CompID > 0 Then

                tempTable = masterPage.aclsData_Temp.GetCompanyInfo_ID(CompID, "JETNET", 0)

                If Not IsNothing(tempTable) Then
                    If tempTable.Rows.Count > 0 Then
                        If Not IsDBNull(tempTable.Rows(0).Item("comp_name")) Then
                            compName = tempTable.Rows(0).Item("comp_name").ToString
                        End If
                    End If
                End If


                If Not String.IsNullOrEmpty(Description.Trim) Then

                    If Not String.IsNullOrEmpty(compName.Trim) Then
                        compLink = crmWebClient.DisplayFunctions.WriteDetailsLink(0, CompID, 0, 0, True, compName, "", "")
                    End If

                    If Not String.IsNullOrEmpty(compLink.Trim) Then
                        returnString = "<span class=""tiny"">[" + Description.Replace(compName, compLink).Trim + "]</span>"
                    Else
                        returnString = "<span class=""tiny"">[" + Description.Trim + "]</span>"
                    End If

                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception


                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return returnString

    End Function

    Public Function DisplayEvalueIcon(ByVal acID As Object, ByVal modelID As Object, ByVal eValues As Object) As String
        Dim displayString As String = ""
        If displayEvalues Then
            If IsNumeric(acID) And IsNumeric(modelID) Then
                If Not IsDBNull(eValues) Then
                    If eValues > 0 Then
                        displayString = "<a href=""javascript:void(0);"" class='no_text_underline' onclick=""javascript:load('view_template.aspx?ViewID=27&ViewName=Value&amod_id=" & modelID.ToString & "&acid=" & acID.ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Valuation View'><i class=""fa fa-usd"" alt='Valuation View' /></i></a>"
                    End If
                End If
            End If
        End If

        Return displayString
    End Function

    Public Function DisplayEValuesData(ByVal eValues As Object) As String
        Dim displayString As String = ""
        If displayEvalues Then
            If Not IsDBNull(eValues) Then
                If eValues > 0 Then
                    displayString = "<br /><a href=""javascript:void(0)"" class=""text_underline " & Session.Item("localUser").crmUser_Evalues_CSS & """  onclick='javascript:openSmallWindowJS(""/help/documents/809.pdf"",""HelpWindow"");'>" & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</a>: " & clsGeneral.clsGeneral.ConvertIntoThousands(eValues)
                End If
            End If
        End If
        Return displayString
    End Function

    Private Sub FillOutSearchParameters()
        Try
            'Filling Back in the Search Criteria.

            'All Pages

            'Serial No Start Search Criteria.
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaSerNoStart) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaSerNoStart) Then
                    ac_ser_no_from.Text = Session.Item("searchCriteria").SearchCriteriaSerNoStart.ToString
                End If
            End If

            'Serial No End Search Criteria.
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaSerNoEnd) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaSerNoEnd) Then
                    ac_ser_no_to.Text = Session.Item("searchCriteria").SearchCriteriaSerNoEnd.ToString
                End If
            End If

            'Reg No Search Criteria.
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaRegNo) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaRegNo) Then
                    ac_reg_no.Text = Session.Item("searchCriteria").SearchCriteriaRegNo.ToString
                End If
            End If

            'Do Not Search Alt Ser #
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaSerDoNotSearchAlt) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaSerDoNotSearchAlt) Then
                    do_not_search_ac_alt_ser_no.Checked = Session.Item("searchCriteria").SearchCriteriaSerDoNotSearchAlt
                End If
            End If

            'Do not Search Prev Reg #
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaDoNotSearchPrevRegNo) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaDoNotSearchPrevRegNo) Then
                    do_not_search_ac_prev_reg_no.Checked = Session.Item("searchCriteria").SearchCriteriaDoNotSearchPrevRegNo
                End If
            End If

            'Reg # Exact Match
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaRegExactMatch) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaRegExactMatch) Then
                    ac_reg_no_exact_match.Checked = Session.Item("searchCriteria").SearchCriteriaRegExactMatch
                End If
            End If

            If MarketEvent = True Then
                'Only event page.

                'Event Months Only.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventMonths) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventMonths) Then
                        If Session.Item("searchCriteria").SearchCriteriaEventMonths <> 0 Then
                            events_months.Text = Session.Item("searchCriteria").SearchCriteriaEventMonths
                        End If
                    End If
                End If
                'Event Days Only.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventDays) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventDays) Then
                        If Session.Item("searchCriteria").SearchCriteriaEventDays <> 0 Then
                            event_days.Text = Session.Item("searchCriteria").SearchCriteriaEventDays
                        Else
                            'This is a special case added for what happens when your event session day isn't there and the textbox defaults to 1.
                            'We need to run a check against all other variables. If there's anything in any of those other months/minutes/hours besides 0, then
                            'we need to clear this out and make it 0.
                            If (Session.Item("searchCriteria").SearchCriteriaEventMonths <> 0 Or Session.Item("searchCriteria").SearchCriteriaEventHours <> 0 Or Session.Item("searchCriteria").SearchCriteriaEventMinutes <> 0) Then
                                event_days.Text = "0"
                            End If
                        End If
                    End If
                End If
                'Event Hours Only.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventHours) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventHours) Then
                        If Session.Item("searchCriteria").SearchCriteriaEventHours <> 0 Then
                            event_hours.Text = Session.Item("searchCriteria").SearchCriteriaEventHours
                        End If
                    End If
                End If
                'Event Minutes Only.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventMinutes) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventMinutes) Then
                        If Session.Item("searchCriteria").SearchCriteriaEventMinutes <> 0 Then
                            event_minutes.Text = Session.Item("searchCriteria").SearchCriteriaEventMinutes
                        End If
                    End If
                End If

                'Event Type Of Search. Aircraft, Wanted or Company.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventSearchType) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventSearchType) Then
                        HttpContext.Current.Session.Item("eventType") = Session.Item("searchCriteria").SearchCriteriaEventSearchType.ToString
                    End If
                End If

                'Event Category
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventCategory) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventCategory) Then

                        Dim EventCategorySelection As Array = Split(Replace(Session.Item("searchCriteria").SearchCriteriaEventCategory, "'", ""), ",")
                        'that the page defaults to.
                        For EventCategorySelectionCount = 0 To UBound(EventCategorySelection)

                            If String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventCatType").ToString.Trim) Then
                                HttpContext.Current.Session.Item("eventCatType") = EventCategorySelection(EventCategorySelectionCount)
                            Else
                                HttpContext.Current.Session.Item("eventCatType") += Constants.cCommaDelim + EventCategorySelection(EventCategorySelectionCount)
                            End If

                        Next
                    Else
                        HttpContext.Current.Session.Item("eventCatType") = ""
                    End If
                Else
                    HttpContext.Current.Session.Item("eventCatType") = ""
                End If

                'Event Types
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaEventType) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaEventType) Then

                        Dim EventTypeSelection As Array = Split(Replace(Session.Item("searchCriteria").SearchCriteriaEventType, "'", ""), ",")

                        'that the page defaults to.
                        For EventTypeSelectionCount = 0 To UBound(EventTypeSelection)
                            If String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventCatCode").ToString.Trim) Then
                                HttpContext.Current.Session.Item("eventCatCode") = EventTypeSelection(EventTypeSelectionCount)
                            Else
                                HttpContext.Current.Session.Item("eventCatCode") += Constants.cCommaDelim + EventTypeSelection(EventTypeSelectionCount)
                            End If
                        Next

                    Else
                        HttpContext.Current.Session.Item("eventCatCode") = ""
                    End If
                Else
                    HttpContext.Current.Session.Item("eventCatCode") = ""
                End If

            End If

            If History = True Then
                'Historical Fields.
                'Retail Activity
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaRetailActivity) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaRetailActivity) Then
                        transaction_retail.Checked = Session.Item("searchCriteria").SearchCriteriaRetailActivity
                    End If
                End If

                'Fill the Transaction Type.
                transaction_retail_CheckedChanged(transaction_retail, EventArgs.Empty)

                'New AC Flag
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaSalesOfNewAircraftOnly) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaSalesOfNewAircraftOnly) Then
                        journ_newac_flag.Checked = Session.Item("searchCriteria").SearchCriteriaSalesOfNewAircraftOnly
                    End If
                End If
                'Used AC Flag
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaSalesOfUsedAircraftOnly) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaSalesOfUsedAircraftOnly) Then
                        jcat_used_retail_sales_flag.Checked = Session.Item("searchCriteria").SearchCriteriaSalesOfUsedAircraftOnly
                    End If
                End If

                'History Exclude Internal Transactions
                'NewSearchClass.SearchCriteriaExcludeInternalTransactions
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaExcludeInternalTransactions) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaExcludeInternalTransactions) Then
                        journ_exclude_internal_transactions.Checked = Session.Item("searchCriteria").SearchCriteriaExcludeInternalTransactions
                    End If
                End If

                'History Type
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryType) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryType) Then
                        'journ_subcat_code_part1.SelectedValue = Session.Item("searchCriteria").SearchCriteriaHistoryType.ToString
                        Dim part1Selection As Array
                        part1Selection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaHistoryType, "'", ""), ",")
                        journ_subcat_code_part1.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For part1SelectionCount = 0 To UBound(part1Selection)
                            For ListBoxCount As Integer = 0 To journ_subcat_code_part1.Items.Count() - 1
                                If UCase(journ_subcat_code_part1.Items(ListBoxCount).Value) = UCase(part1Selection(part1SelectionCount)) Then
                                    journ_subcat_code_part1.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If

                'History From Operator
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryFromOperator) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryFromOperator) Then
                        journ_subcat_code_part2_operator.SelectedValue = Session.Item("searchCriteria").SearchCriteriaHistoryFromOperator.ToString
                    End If
                End If

                'History From 
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryFromAnswer) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryFromAnswer) Then
                        Dim part2Selection As Array
                        part2Selection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaHistoryFromAnswer, "'", ""), ",")
                        journ_subcat_code_part2.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For part2SelectionCount = 0 To UBound(part2Selection)
                            For ListBoxCount As Integer = 0 To journ_subcat_code_part2.Items.Count() - 1
                                If UCase(journ_subcat_code_part2.Items(ListBoxCount).Value) = UCase(part2Selection(part2SelectionCount)) Then
                                    journ_subcat_code_part2.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If

                'History To Operator
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryToOperator) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryToOperator) Then
                        journ_subcat_code_part3_operator.SelectedValue = Session.Item("searchCriteria").SearchCriteriaHistoryToOperator.ToString
                    End If
                End If

                'History To 
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryToAnswer) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryToAnswer) Then
                        Dim part3Selection As Array
                        part3Selection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaHistoryToAnswer, "'", ""), ",")
                        journ_subcat_code_part3.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For part3SelectionCount = 0 To UBound(part3Selection)
                            For ListBoxCount As Integer = 0 To journ_subcat_code_part3.Items.Count() - 1
                                If UCase(journ_subcat_code_part3.Items(ListBoxCount).Value) = UCase(part3Selection(part3SelectionCount)) Then
                                    journ_subcat_code_part3.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If

                'History Date Operator
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryDateOperator) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryDateOperator) Then
                        journ_date_operator.SelectedValue = Session.Item("searchCriteria").SearchCriteriaHistoryDateOperator.ToString
                    End If
                End If

                'History Date
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaHistoryDate) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaHistoryDate) Then
                        journ_date.Text = Session.Item("searchCriteria").SearchCriteriaHistoryDate.ToString
                    End If
                End If
            End If

            '---------------------------------------------------------------------------------------------
            If MarketEvent = False And History = False Then
                'Aircraft Page Only
                'Previously Owned Search Criteria.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPreviouslyOwned) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPreviouslyOwned) Then
                        ac_previously_owned_flag.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPreviouslyOwned.ToString
                    End If
                End If

                'Lease Status Search Criteria.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaLeaseStatus) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaLeaseStatus) Then
                        lease_status.SelectedValue = Session.Item("searchCriteria").SearchCriteriaLeaseStatus.ToString
                    End If
                End If

                'Market Status Search Criteria.
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaMarketStatus) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaMarketStatus) Then
                        If Session.Item("isMobile") = True Then
                            mobileStatus.SelectedValue = Session.Item("searchCriteria").SearchCriteriaMarketStatus
                        Else
                            Dim MarketSelection As Array
                            MarketSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaMarketStatus, "'", ""), ",")
                            market.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                            'that the page defaults to.
                            For MarketSelectionCount = 0 To UBound(MarketSelection)
                                For ListBoxCount As Integer = 0 To market.Items.Count() - 1
                                    If UCase(market.Items(ListBoxCount).Value) = UCase(MarketSelection(MarketSelectionCount)) Then
                                        market.Items(ListBoxCount).Selected = True
                                    End If
                                Next
                            Next
                        End If
                    End If
                End If

                'Life Cycle
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaLifeCycle) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaLifeCycle) Then
                        Dim LifeCycleSelection As Array
                        LifeCycleSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaLifeCycle, "'", ""), ",")
                        ac_lifecycle_stage.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For LifeCycleSelectionCount = 0 To UBound(LifeCycleSelection)
                            For ListBoxCount As Integer = 0 To ac_lifecycle_stage.Items.Count() - 1
                                If UCase(ac_lifecycle_stage.Items(ListBoxCount).Value) = UCase(LifeCycleSelection(LifeCycleSelectionCount)) Then
                                    ac_lifecycle_stage.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If

                'Ownership
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOwnership) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOwnership) Then
                        Dim OwnershipSelection As Array
                        OwnershipSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaOwnership, "'", ""), ",")
                        ac_ownership_type.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For OwnershipSelectionCount = 0 To UBound(OwnershipSelection)
                            For ListBoxCount As Integer = 0 To ac_ownership_type.Items.Count() - 1
                                If UCase(ac_ownership_type.Items(ListBoxCount).Value) = UCase(OwnershipSelection(OwnershipSelectionCount)) Then
                                    ac_ownership_type.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If
            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Protected Sub IsValidDateEntry(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim input As String = args.Value
        Dim pattern As String = ":"
        Dim replacement As String = ","
        Dim rgx As New Regex(pattern)
        Dim pattern2 As String = "\n"
        Dim rgx2 As New Regex(pattern2)
        Dim result As String = rgx.Replace(input, replacement)
        result = rgx2.Replace(result, replacement)

        Dim DateArray As Array = Split(result, ",")


        For x = 0 To UBound(DateArray)
            If Not IsDate(DateArray(x)) Then
                args.IsValid = False
            End If

        Next


    End Sub

    Public Function AircraftBuildNote(ByVal ID As Long, ByVal typeOfNote As String) As String

        Dim ResultsTable As New DataTable
        Dim ReturnString As String = ""

        Dim yacht As Boolean = False
        Dim aircraft As Boolean = False
        Dim company As Boolean = False

        Try

            If Not String.IsNullOrEmpty(typeOfNote.Trim) Then

                If typeOfNote.ToUpper.Contains("YACHT") Then
                    yacht = True
                ElseIf typeOfNote.ToUpper.Contains("AC") Then
                    aircraft = True
                Else
                    company = True
                End If

                If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag Then 'make sure the display is correct on the listing page
                    If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then

                        If HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = True Then

                            ResultsTable = masterPage.aclsData_Temp.AIRCRAFT_LISTING_DUAL_Notes_LIMIT(typeOfNote, ID, "A", "JETNET", Month(Now()).ToString + "/" + Day(Now()).ToString + "/" + Year(Now()).ToString)

                            If Not IsNothing(ResultsTable) Then
                                If ResultsTable.Rows.Count > 0 Then
                                    ReturnString = "<i class=""fa-thumb-tack"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'></i>"
                                End If
                            End If
                        End If

                    ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then

                        If aircraft Or yacht Then

                            ResultsTable = masterPage.aclsData_Temp.CloudNotesDetailsNoteListingQuery(ID, "A", aircraft, company, yacht, True)

                            If Not IsNothing(ResultsTable) Then
                                If ResultsTable.Rows.Count > 0 Then
                                    ReturnString = "<i class=""fa-thumb-tack"" alt='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "' title='" & IIf(Not IsDBNull(ResultsTable.Rows(0).Item("lnote_entry_date")), Format(CDate(ResultsTable.Rows(0).Item("lnote_entry_date")), "MM/dd/yyyy") & " - ", "") & ResultsTable.Rows(0).Item("lnote_note") & "'></i>"
                                End If
                            End If
                        End If
                    End If

                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

        Return ReturnString

    End Function

    Public Function showEstAFTT(ByVal ac_airframe_tot_hrs As String, ByVal ac_est_airframe_hrs As String, ByVal ac_year As String, ByVal ac_times_as_of_date As String, ByVal bShowOnListing As Boolean, ByVal bShowOnTableHTML As Boolean) As String

        Dim htmlOutStr As String = ""

        Dim BASEACYEAR As Long = 2005
        Dim BASEACTIMES As Date = CDate("06/01/2005")

        Dim bShowEstAFTT As Boolean = True

        Dim nAcAFTT As Long = 0
        Dim nAcEstAFTT As Long = 0
        Dim nAcYear As Long = 0
        Dim dtAcTimesOfDate As Date = Now()
        Dim show_est_w_zero As Boolean = False

        If Not String.IsNullOrEmpty(ac_year.Trim) Then
            If IsNumeric(ac_year) Then
                nAcYear = CLng(ac_year.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_times_as_of_date.Trim) Then
            If IsDate(ac_times_as_of_date) Then
                dtAcTimesOfDate = CDate(ac_times_as_of_date.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_airframe_tot_hrs.Trim) Then
            If IsNumeric(ac_airframe_tot_hrs) Then
                nAcAFTT = CLng(ac_airframe_tot_hrs.Trim)
            End If
        End If

        If Not String.IsNullOrEmpty(ac_est_airframe_hrs.Trim) Then
            If IsNumeric(ac_est_airframe_hrs) Then
                nAcEstAFTT = CLng(ac_est_airframe_hrs.Trim)
            End If
        End If

        ' MSW - CREATED show_est_w_zero so that newer ac with  nAcAFTT = 0 can show
        ' IF the MFR Year (nAcYear) is > BASEACYEAR which is currently set to 2005, and we have flight hours, nAcEstAFTT
        ' Then we should skip the other ids, and show it 

        show_est_w_zero = False
        If (nAcAFTT = 0 And nAcYear > BASEACYEAR And nAcEstAFTT > 0) Then
            show_est_w_zero = True
        ElseIf nAcAFTT = 0 And nAcYear < BASEACYEAR Then
            bShowEstAFTT = False
        ElseIf dtAcTimesOfDate < BASEACTIMES Then
            bShowEstAFTT = False
        ElseIf nAcAFTT = nAcEstAFTT Then
            bShowEstAFTT = False
        End If

        If bShowOnListing Then
            If nAcAFTT > 0 Or show_est_w_zero = True Then ' MSW - ADDED IN show_est_w_zero so that in those circumstances it can show
                If Session.Item("isMobile") = True Then
                    If bShowEstAFTT Then
                        If nAcEstAFTT > 0 Then
                            htmlOutStr += "<span class=""float_right ""><span class=""help_cursor"" title=""Estimated AFTT based on flight hours."" class=""text_underline"">" & FormatNumber(nAcEstAFTT, 0).ToString & " hrs</span></span>"
                        End If
                    Else
                        If nAcAFTT > 0 Then
                            htmlOutStr += "<span class=""float_right "">" & FormatNumber(nAcAFTT, 0).ToString + " hrs</span>"
                        End If
                    End If
                Else
                    htmlOutStr = "<span class=""""><span class=""label"">AFTT"
                    htmlOutStr += IIf(bShowEstAFTT, " / <a href=""javascript:void();"" onclick=""openEstAFTTHelp();"" class=""text_underline"">EST AFTT</a>", "")
                    htmlOutStr += ":</span>&nbsp;" + nAcAFTT.ToString + ""
                    htmlOutStr += IIf(bShowEstAFTT, " / <span>" + nAcEstAFTT.ToString + "</span>", "") + "</span><br />"
                End If
            End If
        End If

        If bShowOnTableHTML Then
            If nAcAFTT > 0 Then
                htmlOutStr += " " + nAcAFTT.ToString + " "
                htmlOutStr += IIf(bShowEstAFTT, " / <span>" + nAcEstAFTT.ToString + "</span>", "") + "<br />"
            End If
        End If

        Return htmlOutStr

    End Function

    Public Function DisplayBaseInfo(ByVal baseCountry As Object, ByVal baseState As Object) As String
        Dim returnString As String = ""

        If Not IsDBNull(baseCountry) Then
            If Not String.IsNullOrEmpty(baseCountry) Then
                returnString += baseCountry
            End If
        End If
        If Not IsDBNull(baseState) Then
            If Not String.IsNullOrEmpty(baseState) Then
                If returnString <> "" Then
                    returnString += ", "
                End If
                returnString += baseState
            End If
        End If

        If returnString <> "" Then
            returnString = "<span class=""display_block div_clear"">" & returnString & "</span>"
        End If
        Return returnString
    End Function

    Public Sub Aircraft_SearchToGrabTheEventOnlyInformation(ByVal MarketEvent As Boolean, ByRef EventTypeOfSearch As String, ByRef MarketCategory As String,
                                                                   ByRef MarketType As String, ByRef Months As Integer, ByRef Days As Integer, ByRef Hours As Integer,
                                                                   ByRef Minutes As Integer, ByRef UseDefaultDate As Boolean, ByRef StartDate As Date,
                                                                   ByRef BuildSearchString As String, ByRef NewSearchClass As SearchSelectionCriteria,
                                                                   ByRef events_months As TextBox, ByRef event_days As TextBox, ByRef event_hours As TextBox, ByRef event_minutes As TextBox)

        Try
            Dim tmpArray() As String = Nothing

            If MarketEvent Then

                NewSearchClass.SearchCriteriaEventSearchType = ""

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventType").ToString.Trim) Then
                    HttpContext.Current.Session.Item("eventType") = "AIRCRAFT"
                Else
                    EventTypeOfSearch = HttpContext.Current.Session.Item("eventType").ToString.Trim
                End If

                NewSearchClass.SearchCriteriaEventSearchType = HttpContext.Current.Session.Item("eventType").ToString.Trim

                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventCatType").ToString.Trim) Then

                    ' Check and see if user selected more than one item
                    If HttpContext.Current.Session.Item("eventCatType").ToString.Contains(Constants.cCommaDelim) Then

                        tmpArray = HttpContext.Current.Session.Item("eventCatType").ToString.Split(Constants.cCommaDelim)

                        If IsArray(tmpArray) And Not IsNothing(tmpArray) Then

                            For x As Integer = 0 To UBound(tmpArray)
                                ' translate index into actual amod_id
                                If String.IsNullOrEmpty(MarketCategory.Trim) Then
                                    MarketCategory = Constants.cSingleQuote + tmpArray(x).ToString + Constants.cSingleQuote
                                Else
                                    MarketCategory += Constants.cCommaDelim + Constants.cSingleQuote + tmpArray(x).ToString + Constants.cSingleQuote
                                End If
                            Next

                        End If

                    Else

                        MarketCategory = Constants.cSingleQuote + HttpContext.Current.Session.Item("eventCatType").ToString + Constants.cSingleQuote

                    End If
                End If

                NewSearchClass.SearchCriteriaEventCategory = ""
                If Not String.IsNullOrEmpty(MarketCategory.Trim) Then
                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(MarketCategory.Replace(Constants.cSingleQuote, ""), "Event Categories")
                    'Setting up Event Category in session
                    NewSearchClass.SearchCriteriaEventCategory = MarketCategory
                End If

                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("eventCatCode").ToString.Trim) Then

                    ' Check and see if user selected more than one item
                    If HttpContext.Current.Session.Item("eventCatCode").ToString.Contains(Constants.cCommaDelim) Then

                        tmpArray = HttpContext.Current.Session.Item("eventCatCode").ToString.Split(Constants.cCommaDelim)

                        If IsArray(tmpArray) And Not IsNothing(tmpArray) Then

                            For x As Integer = 0 To UBound(tmpArray)
                                ' translate index into actual amod_id
                                If String.IsNullOrEmpty(MarketType.Trim) Then
                                    MarketType = Constants.cSingleQuote + tmpArray(x).ToString + Constants.cSingleQuote
                                Else
                                    MarketType += Constants.cCommaDelim + Constants.cSingleQuote + tmpArray(x).ToString + Constants.cSingleQuote
                                End If
                            Next

                        End If

                    Else

                        MarketType = Constants.cSingleQuote + HttpContext.Current.Session.Item("eventCatCode").ToString + Constants.cSingleQuote

                    End If
                End If

                NewSearchClass.SearchCriteriaEventType = ""
                If Not String.IsNullOrEmpty(event_type_text.Text.Trim) Then
                    If UCase(event_type_text.Text.Trim) <> "ALL" Then
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(event_type_text.Text.Replace(Constants.cSingleQuote, ""), "Event Types")
                    End If
                End If

                If Not String.IsNullOrEmpty(MarketType) Then
                    'Setting up Event Type in session
                    If String.IsNullOrEmpty(event_type_text.Text.Trim) Then
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(MarketType.Replace(Constants.cSingleQuote, ""), "Event Types")
                    End If
                    NewSearchClass.SearchCriteriaEventType = MarketType
                End If

                NewSearchClass.SearchCriteriaEventMonths = 0
                If events_months.Text <> "" Then
                    If IsNumeric(events_months.Text) Then
                        Months = events_months.Text
                        UseDefaultDate = False
                        'Setting up Event Months in session
                        NewSearchClass.SearchCriteriaEventMonths = Months
                    End If
                End If

                NewSearchClass.SearchCriteriaEventDays = 1
                If event_days.Text <> "" Then
                    If IsNumeric(event_days.Text) Then
                        Days = event_days.Text
                        UseDefaultDate = False
                        'Setting up Event Days in session
                        NewSearchClass.SearchCriteriaEventDays = Days
                    End If
                End If
                NewSearchClass.SearchCriteriaEventHours = 0
                If event_hours.Text <> "" Then
                    If IsNumeric(event_hours.Text) Then
                        Hours = event_hours.Text
                        UseDefaultDate = False
                        'Setting up Event Hours in session
                        NewSearchClass.SearchCriteriaEventHours = Hours
                    End If
                End If
                NewSearchClass.SearchCriteriaEventMinutes = 0
                If event_minutes.Text <> "" Then
                    If IsNumeric(event_minutes.Text) Then
                        Minutes = event_minutes.Text
                        UseDefaultDate = False
                        'Setting up Event Minutes in session
                        NewSearchClass.SearchCriteriaEventMinutes = Minutes
                    End If
                End If

                If UseDefaultDate = False Then
                    StartDate = DateAdd(DateInterval.Month, -Months, Now())
                    StartDate = DateAdd(DateInterval.Day, -Days, StartDate)
                    StartDate = DateAdd(DateInterval.Hour, -Hours, StartDate)
                    StartDate = DateAdd(DateInterval.Minute, -Minutes, StartDate)
                Else
                    NewSearchClass.SearchCriteriaEventDays = 1
                    StartDate = DateAdd(DateInterval.Day, -1, Now())
                    event_days.Text = 1
                End If

                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Format(CDate(StartDate), "MM/dd/yyyy hh:mm:ss tt"), "Start Date")
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Format(CDate(Now()), "MM/dd/yyyy hh:mm:ss tt"), "End Date")

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim
                End If
            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Function DisplayHistoryDataGridAsking(ByVal ac_forsale_flag As Object, ByVal ac_status As Object, ByVal ac_asking_price As Object, ByVal acAskingWordage As Object) As String
        Dim returnString As String = ""


        ' If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
        If ac_forsale_flag = "Y" Then
            If Not IsDBNull(acAskingWordage) Then
                If acAskingWordage.ToString.ToUpper = "PRICE" Then
                    If Not IsDBNull(ac_asking_price) Then
                        returnString += crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(ac_asking_price)
                    End If
                Else
                    If acAskingWordage.ToString.ToUpper = "MAKE OFFER" Then
                        returnString += "<span class=""help_cursor"" title=""MAKE OFFER"">M/O</span>"
                    End If
                End If
            End If
        End If
        'End If
        Return returnString
    End Function

    Private Sub TestLoadAttributes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestLoadAttributes.Click
        Response.Redirect("Aircraft_Listing.aspx?att=true", False)
    End Sub

End Class