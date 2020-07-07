Partial Public Class DisplayModelDetail

  Inherits System.Web.UI.Page
  Dim ModelID As Long = 0
  Dim ModelDataTable As New DataTable
  Dim bAdminModelDisplay As Boolean = False

  Public Shared masterPage As New Object

  Private Sub DisplayModelDetail_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try

      bAdminModelDisplay = IIf((Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE), True, False)

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (DisplayModelDetail_PreInit): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (DisplayModelDetail_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Private Sub DisplayModelDetail_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    If Not IsNothing(Request.Item("id")) Then
      If Not String.IsNullOrEmpty(Request.Item("id").ToString) Then
        ModelID = CLng(Request.Item("id").ToString.Trim)
      End If
    End If

  End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ModelTable As New DataTable
        Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

        Dim isHomebase As Boolean = False
        Dim displayType As String = ""
        Dim is_postback_change_model As Boolean = False

        Try

            If IsPostBack Then
                If model_id_admin.SelectedValue <> ModelID Then
                    ' THEN WE POSTBACK AND CHANGED MODEL, SO SKIP A BUNCH OF FUCNTIONS 
                    is_postback_change_model = True
                End If
            End If


            If Not IsNothing(HttpContext.Current.Request("homebase")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request("homebase").ToString.Trim) Then
                    isHomebase = IIf(HttpContext.Current.Request("homebase").ToString.Trim.Contains("Y"), True, False)
                End If
            End If

            If Not IsNothing(HttpContext.Current.Request("type")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request("type").ToString.Trim) Then
                    displayType = HttpContext.Current.Request("type").ToString.Trim
                End If
            End If

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Or bAdminModelDisplay Then
                Me.know_more.Visible = False
            Else
                Me.know_more.HRef = "view_template.aspx?nomasterPage=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" + ModelID.ToString
            End If

            If ModelID <> 0 Then

                ' if not logged in and not from homebase then kick out user
                If Not CBool(Session.Item("crmUserLogon").ToString) And Not isHomebase Then

                    Response.Redirect("Default.aspx", True)

                Else

                    If isHomebase Then

                        sale.Visible = False
                        dealer.Visible = False
                        userInterest.Visible = False
                        maintenanceDetails.Visible = False
                        internalNotes.Visible = False
                        topics.Visible = False
                        operational.Visible = False
                        utilization.Visible = False

                        If useBackupSQL Then
                            masterPage.aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                        Else
                            masterPage.aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                        End If

                        FillModelInformation()

                        If ModelDataTable.Rows.Count > 0 Then

                            maintenance.Visible = False
                            features.Visible = False

                            If displayType.ToLower.Contains("performance") Then

                                costs.Visible = False
                                FillModelEngine()
                                FillBasicConfiguration()

                            Else

                                engine.Visible = False
                                basic.Visible = False
                                FillCostsBudget()

                            End If

                        End If

                    Else
                        If is_postback_change_model = True Then
                        Else
                            FillModelInformation()
                        End If


                        If ModelDataTable.Rows.Count > 0 Then

                            If is_postback_change_model = True Then
                            Else
                                FillModelPictureAndVideo()
                                FillModelEngine()
                                FillModelCodes()
                                FillMaintenance()
                                FillBasicConfiguration()
                                FillCostsBudget()

                                Build_Model_Resources_Tab(ModelID)
                            End If

                            If bAdminModelDisplay Then

                                If model_id_admin.Items.Count > 1 Then
                                Else
                                    commonEvo.fillMakeModelDropDown(model_id_admin, Nothing, 300, "", -1, False, False, False, True, False, False) ' fill list with models
                                    Me.model_id_admin.Visible = True
                                    Me.model_name_admin.Visible = True

                                    Me.model_id_admin.SelectedValue = ModelID
                                End If


                                ' select the item in the line here 



                                If is_postback_change_model = True Then
                                Else
                                    FillSalePrices()
                                    FillDealer()
                                    FillUserInterest()
                                    FillMaintDetails()
                                    FillTopics()
                                    FillUtilizationGraph()
                                End If


                                'ADDED IN MSW 
                                ' FillAssettInsightGraphs() 
                                Try
                                        Dim utilization_functions As New utilization_view_functions
                                        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                                        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                                        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                                        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                                        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                                        Call utilization_functions.FillAssettInsightGraphs("MFRYEAR", ModelID, assett_label.Text, sale, 7, 0, 0, 295, 0)
                                        Call utilization_functions.FillAssettInsightGraphs("ASKSOLD", ModelID, assett_label2.Text, sale, 777, 0, 0, 295, 0, True, True, True, "", "", "MODEL")


                                    Catch ex As Exception

                                    End Try


                                    FillOperationalTrends()

                                    Dim sTmpText As String = model_internal_notes(ModelID)

                                    If Not String.IsNullOrEmpty(sTmpText) Then
                                        internalNotes_label.Text = sTmpText
                                    Else
                                        internalNotes_label.Text = "<table id=""modelInternalNotesTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">"
                                        internalNotes_label.Text += "<tr><td valign=""top"" align=""left""><strong><em>No Internal Model Notes to Display</em></strong></td></tr>"
                                        internalNotes_label.Text += "</table>"
                                    End If

                                    internalNotes.Visible = True

                                Else

                                    sale.Visible = False
                                dealer.Visible = False
                                userInterest.Visible = False
                                maintenanceDetails.Visible = False
                                internalNotes.Visible = False
                                topics.Visible = False
                                operational.Visible = False
                                utilization.Visible = False

                            End If

                        Else

                            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("This aircraft model is not viewable by your current subscription.")
                            masterPage.SetPageTitle("This aircraft model is not viewable by your current subscription.")
                            no_model_text.Text = "<p align='center'>This aircraft model is not viewable by your subscription.</p>"

                            information.Visible = False
                            engine.Visible = False
                            maintenance.Visible = False
                            features.Visible = False
                            basic.Visible = False
                            costs.Visible = False
                            resources.Visible = False

                            sale.Visible = False
                            dealer.Visible = False
                            userInterest.Visible = False
                            maintenanceDetails.Visible = False
                            internalNotes.Visible = False
                            topics.Visible = False
                            operational.Visible = False
                            utilization.Visible = False

                        End If

                        ModelDataTable = Nothing

                        If Not Page.IsPostBack Then
                            'Insert into Content Stat Table
                            '  masterPage.aclsData_Temp.Insert_Content_Stat(Now(), 0, 0, ModelID, 0, 0, 0, 0, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)
                            Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayModelDetail: Model" & ModelID, Nothing, 0, 0, 0, 0, 0, 0, ModelID)
                        End If
                    End If

                End If

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load: " + ex.Message
        End Try

    End Sub


    Sub model_id_admin_clicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Response.Redirect("DisplayModelDetail.aspx?id=" & model_id_admin.SelectedValue)

    End Sub

    Private Sub Build_Model_Resources_Tab(ByVal amod_id As Integer)
    Dim ResourcesDataT As New DataTable
    Dim aclsData_Temp As New clsData_Manager_SQL
    Dim css_string As String = ""
    Try


      aclsData_Temp.JETNET_DB = Application.Item("crmClientSiteData").AdminDatabaseConn
      aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
      aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

      ResourcesDataT = aclsData_Temp.HelpData("ML", "", 0, False, amod_id)

      If Not IsNothing(ResourcesDataT) Then
        If ResourcesDataT.Rows.Count > 0 Then

          Me.resources.Visible = True
          Me.resources_label.Text = ""
          Me.resources_label.Text += "<table width='100%' cellpadding='0' cellspacing='0'>"
          For Each r As DataRow In ResourcesDataT.Rows


            Me.resources_label.Text += "<tr valign='top'><td align='left' valign='top' " & IIf(css_string <> "", "class='" & css_string & "'", "") & "><span class='help_indent'><span class='help_subtitle'>"

            If Not IsDBNull(r.Item("evonot_doc_link")) Then

              If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

                Me.resources_label.Text += "<a href='"

                If Not r.Item("evonot_doc_link").ToString.ToLower.Contains("http") And r.Item("evonot_doc_link").ToString.ToLower.Contains(".com") Then
                  Me.resources_label.Text += "http://"
                End If

                Me.resources_label.Text += r.Item("evonot_doc_link").ToString.Trim
                Me.resources_label.Text += "' target='_blank'>"

              End If
            End If

            Me.resources_label.Text += r("evonot_title") & "</a></span>- " & Replace(Replace(r("evonot_announcement"), "<p>", ""), "</p>", "") & "</span>"
            If Not IsDBNull(r("evonot_video")) Then
              If Trim(r("evonot_video")) <> "" Then
                Me.resources_label.Text += "</td></tr><tr><td valign='middle' " & IIf(css_string <> "", "class='" & css_string & "'", "") & "><a href='Help.aspx?id=" & r("evonot_id") & "'  class='video_reel_link' id='" & r("evonot_id") & "_text'>Click Here To View This Video</a>"
              End If
            End If
            'help_label.Text += "<div id='" & r("evonot_id") & "' class='display_none'><p align='center'>" & r("evonot_video").ToString & "</p></div><div class='clear'>&nbsp;</div>"

            Me.resources_label.Text += "</td></tr>"
            If css_string = "" Then
              css_string = "alt_row"
            Else
              css_string = ""
            End If
          Next
          Me.resources_label.Text += "</table>"
        Else
          Me.resources.Visible = False
        End If
      End If
      ResourcesDataT = New DataTable

    Catch ex As Exception

    End Try

  End Sub

  Public Sub FillModelInformation()
    Dim RightSideString As String = ""
    Dim LeftSideString As String = ""
    Dim tmpString As String = ""

    'Years
    Dim StartYear As String = ""
    Dim EndYear As String = ""
    'Serial Number
    Dim SerPrefix As String = ""
    Dim SerStart As String = ""
    Dim SerEnd As String = ""
    Dim SerSuffix As String = ""
    'Model Type
    Dim ModelTypeCode As String = ""
    Dim AirframeTypeCode As String = ""

    'Weight 
    Dim WeightClass As String = ""
    'Price 
    Dim StartPrice As Double = 0
    Dim EndPrice As Double = 0

    Try

      ModelDataTable = masterPage.aclsData_Temp.GetJetnetModelInfo(ModelID, True, "DisplayModelDetail.aspx.vb")

      If Not IsNothing(ModelDataTable) Then
        If ModelDataTable.Rows.Count > 0 Then

          'Make/Model
          If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(ModelDataTable.Rows(0).Item("amod_model_name")) Then

            information_tab.HeaderText = IIf(bAdminModelDisplay, "Model Intelligence for ", "") + ModelDataTable.Rows(0).Item("amod_make_name").ToString + " " + ModelDataTable.Rows(0).Item("amod_model_name").ToString
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(IIf(bAdminModelDisplay, "Model Intelligence for ", "") + ModelDataTable.Rows(0).Item("amod_make_name").ToString + " " + ModelDataTable.Rows(0).Item("amod_model_name").ToString)
            masterPage.SetPageTitle(IIf(bAdminModelDisplay, "Model Intelligence for ", "") + ModelDataTable.Rows(0).Item("amod_make_name").ToString + " " + ModelDataTable.Rows(0).Item("amod_model_name").ToString + " Model Details")

          End If

          LeftSideString = "<span class='li'><span class='label'>Make/Model:</span> " & ModelDataTable.Rows(0).Item("amod_make_name").ToString & " " & ModelDataTable.Rows(0).Item("amod_model_name").ToString & "</span>"

          'Manufacturer
          RightSideString = "<span class='li'><span class='label'>Manufacturer:</span> " & ModelDataTable.Rows(0).Item("amod_manufacturer").ToString & "</span>"

          'Years Build Range
          LeftSideString += "<span class='li'><span class='label'>Years Built:</span> "

          StartYear = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_start_year")), Trim(ModelDataTable.Rows(0).Item("amod_start_year").ToString), "")
          EndYear = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_end_year")), Trim(ModelDataTable.Rows(0).Item("amod_end_year").ToString), "")

          LeftSideString += StartYear

          If EndYear <> "" Then
            LeftSideString += " - " & EndYear & "&nbsp;"
          ElseIf StartYear <> "" Then
            LeftSideString += " - Present&nbsp;"
          End If
          LeftSideString += "</span>"

          'Serial Number Range String.
          RightSideString += "<span class='li'><span class='label'>Serial Number Range:</span> "

          SerPrefix = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_ser_no_prefix")), Trim(ModelDataTable.Rows(0).Item("amod_ser_no_prefix").ToString), "")
          SerStart = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_ser_no_start")), Trim(ModelDataTable.Rows(0).Item("amod_ser_no_start").ToString), "")
          SerEnd = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_ser_no_end")), Trim(ModelDataTable.Rows(0).Item("amod_ser_no_end").ToString), "")
          SerSuffix = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_ser_no_suffix")), Trim(ModelDataTable.Rows(0).Item("amod_ser_no_suffix").ToString), "")

          RightSideString += SerPrefix & SerStart & SerSuffix & "&nbsp;"

          If SerEnd <> "" Then
            RightSideString += " - " & SerPrefix & SerEnd & SerSuffix & "&nbsp;"
          ElseIf SerStart <> "" Then
            RightSideString += "&amp; Up&nbsp;"
          Else
            RightSideString += "&nbsp;"
          End If

          RightSideString += "</span>"

          'Type
          LeftSideString += "<span class='li'><span class='label'>Type:</span> "
          ModelTypeCode = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("atype_name")), Trim(ModelDataTable.Rows(0).Item("atype_name").ToString), "")
          AirframeTypeCode = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("aftype_name")), Trim(ModelDataTable.Rows(0).Item("aftype_name").ToString), "")


          LeftSideString += "" & AirframeTypeCode & " " & ModelTypeCode & "</span>"

          'Weight
          RightSideString += "<span class='li'><span class='label'>Weight Class:</span> "
          Select Case IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_weight_class")), UCase(Trim(ModelDataTable.Rows(0).Item("amod_weight_class").ToString)), "")
            Case "V"
              RightSideString += "Very Light Jet"
            Case "L"
              RightSideString += "Light"
            Case "M"
              RightSideString += "Medium"
            Case "H"
              RightSideString += "Heavy"
            Case Else
          End Select

          RightSideString += "</span>"

          If Not IsDBNull(ModelDataTable.Rows(0).Item("ambc_name")) Then
            If Not String.IsNullOrEmpty(ModelDataTable.Rows(0).Item("ambc_name").ToString) Then
              If Not ModelDataTable.Rows(0).Item("ambc_name").ToString.ToLower.Contains("unknown") And (ModelDataTable.Rows(0).Item("amod_product_helicopter_flag").ToString.ToUpper.Trim.Contains("Y") Or ModelDataTable.Rows(0).Item("amod_product_commercial_flag").ToString.ToUpper.Trim.Contains("Y")) Then
                LeftSideString += "<span class='li'><span class='label'>Body Configuration&nbsp;:&nbsp;</span> " & ModelDataTable.Rows(0).Item("ambc_name").ToString & "</span>"
              End If
            End If
          End If

          information_label.Text = "<table width='100%' cellpadding='3' cellspacing='0'>"
          information_label.Text += "<tr>"
          information_label.Text += "<td align='left' valign='top' width='50%'>" + LeftSideString + "</td>"
          information_label.Text += "<td align='left' valign='top' width='50%'>" + RightSideString + "</td>"
          information_label.Text += "</tr>"


          tmpString = "<span class=""li""><span class=""label"">" + IIf(bAdminModelDisplay, "Spec Asking Price Range&nbsp;:&nbsp;", "Price Range&nbsp;:&nbsp;").ToString.Trim + "</span>"

          StartPrice = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_start_price")), Trim(ModelDataTable.Rows(0).Item("amod_start_price").ToString), 0)
          EndPrice = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_end_price")), Trim(ModelDataTable.Rows(0).Item("amod_end_price").ToString), 0)

          If StartPrice <> 0 Then
            tmpString += FormatCurrency((StartPrice / 1000), 0, False, True, True) + "k"
          Else
            tmpString += "&nbsp;"
          End If

          If EndPrice <> 0 Then
            tmpString += " - " + FormatCurrency((EndPrice / 1000), 0, False, True, True) + "k"
          End If

          tmpString += "</span>"

          If bAdminModelDisplay Then

            Dim getSPIValues As New viewsDataLayer
            Dim getMarketValues As New market_model_functions
            Dim searchCriteria As New viewSelectionCriteriaClass
            Dim results_table As New DataTable

            Dim Asking_High As Double = 0.0
            Dim Asking_Low As Double = 0.0

            Dim Actual_Asking_High As Double = 0.0
            Dim Actual_Asking_Low As Double = 0.0

            Dim askingPriceString As String = ""

            getSPIValues.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            getSPIValues.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            getSPIValues.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            getSPIValues.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            getSPIValues.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            getMarketValues.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            getMarketValues.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            getMarketValues.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            getMarketValues.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            getMarketValues.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            searchCriteria.ViewCriteriaAmodID = ModelID
            searchCriteria.ViewCriteriaTimeSpan = 12

            getMarketValues.views_display_fleet_market_summary(searchCriteria, "", "", askingPriceString)

            results_table = getSPIValues.Create_Run_Price_History_SPI(searchCriteria)

            If Not IsNothing(results_table) Then

              If results_table.Rows.Count > 0 Then

                For Each r As DataRow In results_table.Rows

                  If Not IsDBNull(r.Item("LOWASKINGPRICE")) Then
                    If Not String.IsNullOrEmpty(r.Item("LOWASKINGPRICE").ToString.Trim) Then
                      If IsNumeric(r.Item("LOWASKINGPRICE").ToString) Then

                        If Asking_Low = 0 Or (CDbl(r.Item("LOWASKINGPRICE").ToString) < Asking_Low) Then
                          Asking_Low = CDbl(r.Item("LOWASKINGPRICE").ToString)
                        End If

                      End If
                    End If
                  End If

                  If Not IsDBNull(r.Item("HIGHASKINGPRICE")) Then
                    If Not String.IsNullOrEmpty(r.Item("HIGHASKINGPRICE").ToString.Trim) Then
                      If IsNumeric(r.Item("HIGHASKINGPRICE").ToString) Then

                        If Asking_High = 0 Or CDbl(r.Item("HIGHASKINGPRICE").ToString) > Asking_High Then
                          Asking_High = CDbl(r.Item("HIGHASKINGPRICE").ToString)
                        End If

                      End If
                    End If
                  End If

                  If Not IsDBNull(r.Item("LOWSALEPRICE")) Then
                    If Not String.IsNullOrEmpty(r.Item("LOWSALEPRICE").ToString.Trim) Then
                      If IsNumeric(r.Item("LOWSALEPRICE").ToString) Then

                        If Actual_Asking_Low = 0 Or (CDbl(r.Item("LOWSALEPRICE").ToString) < Actual_Asking_Low) Then
                          Actual_Asking_Low = CDbl(r.Item("LOWSALEPRICE").ToString)
                        End If

                      End If
                    End If
                  End If

                  If Not IsDBNull(r.Item("HIGHSALEPRICE")) Then
                    If Not String.IsNullOrEmpty(r.Item("HIGHSALEPRICE").ToString.Trim) Then
                      If IsNumeric(r.Item("HIGHSALEPRICE").ToString) Then

                        If Actual_Asking_High = 0 Or CDbl(r.Item("HIGHSALEPRICE").ToString) > Actual_Asking_High Then
                          Actual_Asking_High = CDbl(r.Item("HIGHSALEPRICE").ToString)
                        End If

                      End If
                    End If
                  End If

                Next

              End If

            End If

            If Not String.IsNullOrEmpty(askingPriceString.Trim) Then
              tmpString += askingPriceString.Trim
            End If

            tmpString += "<span class=""li""><span class=""label"">Sales Asking Price Range<br/>(Last 12 Months)&nbsp;:&nbsp;</span>"

            If Asking_Low > 0 Or Asking_High > 0 Then

              If Asking_Low > 0 Then
                tmpString += FormatCurrency((Asking_Low / 1000), 0, False, True, True) + "k"
              Else
                tmpString += "&nbsp;"
              End If

              If Asking_High > 0 Then
                tmpString += " - " + FormatCurrency((Asking_High / 1000), 0, False, True, True) + "k"
              End If

            Else
              tmpString += "No Sales Asking Price Range to Display"
            End If

            tmpString += "</span>"

            tmpString += "<span class=""li""><span class=""label"">Actual Sale Price Range<br/>(Last 12 Months)&nbsp;:&nbsp;</span>"

            If Actual_Asking_Low > 0 Or Actual_Asking_High > 0 Then

              If Actual_Asking_Low > 0 Then
                tmpString += FormatCurrency((Actual_Asking_Low / 1000), 0, False, True, True) + "k"
              Else
                tmpString += "&nbsp;"
              End If

              If Actual_Asking_High > 0 Then
                tmpString += " - " + FormatCurrency((Actual_Asking_High / 1000), 0, False, True, True) + "k"
              End If

            Else
              tmpString += "No Actual Sale Price Range to Display"
            End If

            tmpString += "</span>"

          End If


          tmpString += "<span class=""li""><span class=""label"">Description&nbsp;:&nbsp;</span>" + IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_description")), ModelDataTable.Rows(0).Item("amod_description").ToString, "").ToString + "</span>"

          information_label.Text += "<tr><td align=""left"" valign=""middle"" colspan=""2"">" + tmpString.Trim + "</td></tr>"
          information_label.Text += "</table>"

        End If
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in FillModelInformation: " + ex.Message
    End Try

  End Sub

  Public Sub FillModelPictureAndVideo()

    If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
      picture_label.Text = "<img src=""https://www.testjetnetevolution.com/pictures/model/" + ModelID.ToString + ".jpg"" width=""505"" height=""325"" class=""border padding"" />"
    Else
      picture_label.Text = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ModelPicturesFolderVirtualPath") + "/" + ModelID.ToString + ".jpg"" width=""505"" height=""325"" class=""border padding"" />" '"http://www.jetnettest.com/pictures/model"
    End If

    'video 
    'Session("ModelVideosFolderVirtualPath") & "/" & ModelID & ".mpeg"
  End Sub

  Public Sub FillModelEngine()
    Dim NumberOfEngines As Integer = 0
    Dim EngineTBOHrs As Long = 0
    Dim EngineThrustLbs As Long = 0
    Dim EngineShaft As Long = 0
    Dim EngineHSI As Long = 0

    Dim PropShaft As Long = 0
    Dim PropHSI As Long = 0
    Dim NumberOfProps As Integer = 0
    Dim PropTBOHrs As Long = 0
    Dim PropMFRName As String = ""
    Dim PropModName As String = ""

    Dim ModelString As String = ""

    Dim AirframeTypeCode As String = ""
    Dim TypeCode As String = ""
    Dim MainRotor1Blade As Long = 0
    Dim MainRotor2Blade As Long = 0
    Dim MainRotor1BladeDiameter As Long = 0
    Dim MainRotor2BladeDiameter As Long = 0
    Dim TailBlade As Long = 0
    Dim TailBladeDiameter As Long = 0
    Dim AntiTorq As String = ""

    engine_label.Text = ("<table class='engine_tab' cellpadding='3' cellspacing='0' width='100%'>")

    engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'>&nbsp;</td><td valign='top' align='left' class='engine_1'><span class='label'>ENGINE</span></td>")

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_type_code"))) Then
      TypeCode = UCase(Trim(ModelDataTable.Rows(0).Item("amod_type_code")))
    Else
      TypeCode = ""
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_airframe_type_code"))) Then
      AirframeTypeCode = UCase(Trim(ModelDataTable.Rows(0).Item("amod_airframe_type_code")))
    Else
      AirframeTypeCode = ""
    End If


    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_number_of_engines"))) Then
      NumberOfEngines = CInt(Trim(ModelDataTable.Rows(0).Item("amod_number_of_engines")))
    Else
      NumberOfEngines = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_number_of_props"))) Then
      NumberOfProps = CInt(Trim(ModelDataTable.Rows(0).Item("amod_number_of_props")))
    Else
      NumberOfProps = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_mfr_name"))) Then
      PropMFRName = Trim(ModelDataTable.Rows(0).Item("amod_prop_mfr_name"))
    Else
      PropMFRName = "&nbsp;"
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_model_name"))) Then
      PropModName = Trim(ModelDataTable.Rows(0).Item("amod_prop_model_name"))
    Else
      PropModName = "&nbsp;"
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_engine_com_tbo_hrs"))) Then
      EngineTBOHrs = CInt(Trim(ModelDataTable.Rows(0).Item("amod_engine_com_tbo_hrs")))
    Else
      EngineTBOHrs = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_com_tbo_hrs"))) Then
      PropTBOHrs = CInt(Trim(ModelDataTable.Rows(0).Item("amod_prop_com_tbo_hrs")))
    Else
      PropTBOHrs = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_engine_hsi"))) Then
      EngineHSI = CInt(Trim(ModelDataTable.Rows(0).Item("amod_engine_hsi")))
    Else
      EngineHSI = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_hsi"))) Then
      PropHSI = CInt(Trim(ModelDataTable.Rows(0).Item("amod_prop_hsi")))
    Else
      PropHSI = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_engine_shaft"))) Then
      EngineShaft = CLng(Trim(ModelDataTable.Rows(0).Item("amod_engine_shaft")))
    Else
      EngineShaft = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_shaft"))) Then
      PropShaft = CLng(Trim(ModelDataTable.Rows(0).Item("amod_prop_shaft")))
    Else
      PropShaft = 0
    End If

    If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_engine_thrust_lbs"))) Then
      EngineThrustLbs = CLng(Trim(ModelDataTable.Rows(0).Item("amod_engine_thrust_lbs")))
    Else
      EngineThrustLbs = 0
    End If

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      ' row 1 col 3
      engine_label.Text += "<td valign='top' align='left' class='engine_1'><span class='label'>PROPELLER(S)</span></td>"
    ElseIf AirframeTypeCode = "R" Then
      ' row 1 col 3 , 4
      engine_label.Text += "<td valign='top' align='left' class='engine_1'><span class='label'>TO&nbsp;Power (SHP)</span></td><td valign='top' align='left' class='engine_1'><span class='label'>Max&nbsp;Continuous (SHP)</span></td>"
    End If

    engine_label.Text += ("</tr>")

    ' row 2 col 1   
    engine_label.Text += "<tr><td valign='top' align='left' class='engine_1'><span class='label'>NUMBER OF</span></td>"
    engine_label.Text += "<td valign='top' align='left'>&nbsp;" & NumberOfEngines & "</td>"

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      ' row 2 col 2   
      engine_label.Text += "<td valign='top' align='left'>&nbsp;" & NumberOfProps & "</td>"
    ElseIf AirframeTypeCode = "R" Then
      ' row 2 col 2 , 3
      engine_label.Text += "<td valign='top' align='left' colspan='2'>&nbsp;</td>"
    End If


    '' RETRIEVE AND DISPLAY ENGINE STUFF -------------------------------


    Dim EngineName As String = ""
    Dim EngineCount As Integer = 0
    Dim TakeoffPower As Long = 0
    Dim ContinuousPower As Long = 0
    Dim EngineModelDetails As New DataTable
    Dim tmpString As String = ""


    EngineModelDetails = masterPage.aclsData_Temp.Aircraft_Model_Engine(ModelID)

    If Not IsNothing(EngineModelDetails) Then

      If (EngineModelDetails.Rows.Count > 0) Then

        tmpString = ""

        EngineCount = EngineModelDetails.Rows.Count

        For Each r As DataRow In EngineModelDetails.Rows

          If Not (IsDBNull(r("ameng_takeoff_power"))) Then
            TakeoffPower = r("ameng_takeoff_power")
          Else
            TakeoffPower = 0
          End If

          If Not (IsDBNull(r("ameng_max_continuous_power"))) Then
            ContinuousPower = r("ameng_max_continuous_power")
          Else
            ContinuousPower = 0
          End If

          If Not (IsDBNull(r("ameng_engine_name"))) Then
            EngineName = r("ameng_engine_name")
          Else
            EngineName = "&nbsp;"
          End If


          If EngineName <> "" Then
            ' row 3 col 2
            If AirframeTypeCode = "F" Then
              If tmpString = "" Then
                tmpString = "&nbsp;" & EngineName
              Else
                tmpString = tmpString & "<br />&nbsp;" & EngineName
              End If
            ElseIf AirframeTypeCode = "R" Then
              ' row 3 col 2,3,4   
              If tmpString = "" Then
                tmpString = "&nbsp;" & EngineName
                tmpString = tmpString & "<td class='Normal' align='left'>&nbsp;" & TakeoffPower
                tmpString = tmpString & "<td class='Normal' align='left'>&nbsp;" & ContinuousPower
              Else
                tmpString = tmpString & "</td></tr><td class='Normal' align='left' nowrap>&nbsp" & EngineName
                tmpString = tmpString & "<td class='Normal' align='left'>&nbsp;" & TakeoffPower
                tmpString = tmpString & "<td class='Normal' align='left'>&nbsp;" & ContinuousPower
              End If

            End If

          End If

        Next


      End If
    End If
    EngineModelDetails.Dispose()


    ' row 3 col 1                                                
    If (TypeCode = "J" Or TypeCode = "E") And AirframeTypeCode = "F" Then
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>MODEL</span></td>")
      engine_label.Text += ("<td valign='top' align='left'>" & tmpString & "&nbsp;</td>")
    ElseIf (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>MODEL</span></td>")
      engine_label.Text += ("<td valign='top' align='left'>" & tmpString & "&nbsp;</td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & Trim(PropMFRName) & " - " & Trim(PropModName) & "</td>")
    ElseIf AirframeTypeCode = "R" Then
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1' rowspan='" & CStr(EngineCount) & "'><span class='label'>MODEL</span></td>")
      engine_label.Text += ("<td valign='top' align='left'>" & tmpString & "&nbsp;</td>")
    End If

    engine_label.Text += ("</tr>")

    ' row 4 col 1   
    engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>TBO</span></td>" & vbCrLf)

    If EngineTBOHrs <> 0 Then
      ' row 4 col 2   
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(EngineTBOHrs, False, False, True) & "</td>" & vbCrLf)
    Else
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>")
    End If

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      If PropTBOHrs <> 0 Then
        ' row 4 col 3   
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(PropTBOHrs, False, False, True) & "</td>" & vbCrLf)
      Else
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>")
      End If
    ElseIf AirframeTypeCode = "R" Then
      ' row 4 col 2 , 3
      engine_label.Text += ("<td valign='top' align='left' colspan='2'>&nbsp;</td>")
    End If

    engine_label.Text += ("</tr>")

    ' row 5 col 1   
    engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>HSI</span></td>")

    If EngineHSI <> 0 Then
      ' row 5 col 2   
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(EngineHSI, False, False, True) & "</td>")
    Else
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>")
    End If

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_prop_hsi")) Then
        ' row 5 col 3   
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(PropHSI, False, False, True) & "</td>")
      Else
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>")
      End If
    ElseIf AirframeTypeCode = "R" Then
      ' row 4 col 2 , 3
      engine_label.Text += ("<td valign='top' align='left' colspan='2'>&nbsp;</td>")
    End If

    engine_label.Text += ("</tr>")

    ' row 6 col 1   
    engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>SHAFT</span></td>" & vbCrLf)

    If EngineShaft <> 0 Then
      ' row 6 col 2   
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(EngineShaft, False, False, True) & "</td>" & vbCrLf)
    Else
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>" & vbCrLf)
    End If

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      If PropShaft <> 0 Then
        ' row 6 col 3   
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(PropShaft, False, False, True) & "</td>" & vbCrLf)
      Else
        engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>" & vbCrLf)
      End If
    ElseIf AirframeTypeCode = "R" Then
      ' row 4 col 2 , 3
      engine_label.Text += ("<td valign='top' align='left' colspan='2'>&nbsp;</td>")
    End If

    engine_label.Text += ("</tr>")

    ' row 7 col 1   
    engine_label.Text += ("<tr><td valign='top' align='left' class='engine_1'><span class='label'>THRUST</span></td>" & vbCrLf)

    If EngineThrustLbs <> 0 Then
      ' row 7 col 2   
      engine_label.Text += ("<td valign='top'  align='left'>&nbsp;" & FormatNumber(EngineThrustLbs, False, False, True) & "</td>" & vbCrLf)
    Else
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;</td>" & vbCrLf)
    End If

    If (TypeCode = "T" Or TypeCode = "P") And AirframeTypeCode = "F" Then
      ' row 7 col 3   
      engine_label.Text += ("<td>&nbsp;</td>")
    ElseIf AirframeTypeCode = "R" Then
      ' row 4 col 2 , 3
      engine_label.Text += ("<td valign='top' align='left' colspan='2'>&nbsp;</td>")
    End If

    engine_label.Text += ("</tr></table>")

    If AirframeTypeCode = "R" Then
      ' engine_label.Text += ("</td><td align='center' valign='middle'>")
      engine_label.Text += ("<table class='engine_tab' cellpadding='3' cellspacing='0' width='100%'>")

      ' row 1 col 1,2,3,4
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_2'>&nbsp;</td><td valign='top' align='left' class='engine_2'><span class='label'>&nbsp;MAIN&nbsp;1&nbsp;</span></td><td valign='top' align='left' class='engine_2'><span class='label'>&nbsp;MAIN&nbsp;2&nbsp;</span></td><td valign='top' align='left' class='engine_2'><span class='label'>&nbsp;&nbsp;TAIL&nbsp;&nbsp;</span></td></tr>")

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_main_rotor_1_blade_count"))) Then
        MainRotor1Blade = ModelDataTable.Rows(0).Item("amod_main_rotor_1_blade_count")
      Else
        MainRotor1Blade = 0
      End If

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_main_rotor_2_blade_count"))) Then
        MainRotor2Blade = ModelDataTable.Rows(0).Item("amod_main_rotor_2_blade_count")
      Else
        MainRotor2Blade = 0
      End If

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_main_rotor_1_blade_diameter"))) Then
        MainRotor1BladeDiameter = ModelDataTable.Rows(0).Item("amod_main_rotor_1_blade_diameter")
      Else
        MainRotor1BladeDiameter = 0
      End If

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_main_rotor_2_blade_diameter"))) Then
        MainRotor2BladeDiameter = ModelDataTable.Rows(0).Item("amod_main_rotor_2_blade_diameter")
      Else
        MainRotor2BladeDiameter = 0
      End If



      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_tail_rotor_blade_count"))) Then
        TailBlade = ModelDataTable.Rows(0).Item("amod_tail_rotor_blade_count")
      Else
        TailBlade = 0
      End If

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_tail_rotor_blade_diameter"))) Then
        TailBladeDiameter = ModelDataTable.Rows(0).Item("amod_tail_rotor_blade_diameter")
      Else
        TailBladeDiameter = 0
      End If

      If Not (IsDBNull(ModelDataTable.Rows(0).Item("amod_rotor_anti_torque_system"))) Then
        AntiTorq = ModelDataTable.Rows(0).Item("amod_rotor_anti_torque_system")
      Else
        AntiTorq = ""
      End If

      ' row 2 col 1,2,3,4   
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_2'><span class='label'>NUMBER OF BLADES</span></td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(MainRotor1Blade, False, False, True) & "</td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(MainRotor2Blade, False, False, True) & "</td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(TailBlade, False, False, True) & "</td>")
      engine_label.Text += ("</tr>")

      ' row 3 col 1,2,3,4   
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_2'><span class='label'>BLADE DIAMETER</span></td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(MainRotor1BladeDiameter, False, False, True) & "</td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(MainRotor2BladeDiameter, False, False, True) & "</td>")
      engine_label.Text += ("<td valign='top' align='left'>&nbsp;" & FormatNumber(TailBladeDiameter, False, False, True) & "</td>")
      engine_label.Text += ("</tr>")

      ' row 3 col 1,2,3,4   
      engine_label.Text += ("<tr><td valign='top' align='left' class='engine_2'><span class='label'>ANTI TORQUE SYSTEM</span></td>")
      engine_label.Text += ("<td valign='top' align='left' nowrap>&nbsp;" & AntiTorq & "</td>")
      engine_label.Text += ("<td valign='top' align='left' colspan='2'>&nbsp;</td>")

      engine_label.Text += ("</td></tr></table>")
    End If


    'engine_label.Text += "</tr>"
    'engine_label.Text += "</table>"
  End Sub

  Public Sub FillModelCodes()

    Dim ModelCodes As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      ModelCodes = GetModelStandardFeatures(ModelID)

      If Not IsNothing(ModelCodes) Then

        If ModelCodes.Rows.Count > 0 Then

          If Not bAdminModelDisplay Then

            htmlOut.Append("<table width=""100%"" cellpadding=""2"" cellspacing=""2"">")

            For Each r As DataRow In ModelCodes.Rows

              ' if its standard, make it blue, otherwise, dont worry about it 
              If Not IsDBNull(r.Item("amfeat_standard_equip")) Then
                If r.Item("amfeat_standard_equip").ToString.ToUpper.Contains("Y") Then
                  htmlOut.Append("<tr><td align=""left"" valign=""top"" class=""blue_text""><span class=""li padding"">")
                Else
                  htmlOut.Append("<tr><td align=""left"" valign=""top""><span class=""li padding"">")
                End If
              Else
                htmlOut.Append("<tr><td align=""left"" valign=""top"" ><span class=""li padding"">")
              End If

              If Not IsDBNull(r.Item("amfeat_feature_code")) Then
                If Not String.IsNullOrEmpty(r.Item("amfeat_feature_code").ToString.Trim) Then
                  htmlOut.Append("<span class=""label"">" + r.Item("amfeat_feature_code").ToString.Trim + "&nbsp;:&nbsp;</span>")
                End If
              End If

              If Not IsDBNull(r.Item("kfeat_name")) Then
                If Not String.IsNullOrEmpty(r.Item("kfeat_name").ToString.Trim) Then
                  htmlOut.Append("<span class=""label"">" + r.Item("kfeat_name").ToString.Trim)
                End If
              End If

              If IIf(Not IsDBNull(r.Item("amfeat_feature_code")), r.Item("amfeat_feature_code").ToString.ToUpper, "") <> "DAM" Then
                If Not IsDBNull(r.Item("kfeat_description")) Then
                  If Not String.IsNullOrEmpty(r.Item("kfeat_description").ToString.Trim) Then
                    htmlOut.Append(" - " + r.Item("kfeat_description").ToString.Trim)
                  End If
                End If
              End If

              htmlOut.Append("</span></td></tr>")

            Next

            htmlOut.Append("</table>")

          Else

            htmlOut.Append("<table width=""100%"" cellpadding=""2"" cellspacing=""2"" class=""data_aircraft_grid darker_blue_border""><tr>")
            htmlOut.Append("<th class=""header_row cell_border_top"" width=""85%"">FEATURE</th>")
            htmlOut.Append("<th class=""header_row cell_border_top"">YES</th>")
            htmlOut.Append("<th class=""header_row cell_border_top"">NO</th>")
            htmlOut.Append("<th class=""header_row cell_border_top"">UNK</th>")
            htmlOut.Append("</tr>")

            For Each r As DataRow In ModelCodes.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              ' if its standard, make it blue, otherwise, dont worry about it  
              If Not IsDBNull(r.Item("amfeat_standard_equip")) Then
                If r.Item("amfeat_standard_equip").ToString.ToUpper.Contains("Y") Then
                  htmlOut.Append("<td align=""left"" valign=""middle"" title=""Standard Equipment""><font color=""blue"" size=""+1""><b>")
                Else
                  htmlOut.Append("<td align=""left"" valign=""middle""><strong>")
                End If
              Else
                htmlOut.Append("<td align=""left"" valign=""middle""><strong>")
              End If

              If Not IsDBNull(r.Item("amfeat_feature_code")) Then
                If Not String.IsNullOrEmpty(r.Item("amfeat_feature_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("amfeat_feature_code").ToString.Trim + "&nbsp;:&nbsp;")
                End If
              End If

              If Not IsDBNull(r.Item("kfeat_name")) Then
                If Not String.IsNullOrEmpty(r.Item("kfeat_name").ToString.Trim) Then
                  htmlOut.Append(r.Item("kfeat_name").ToString.Trim + IIf(r.Item("amfeat_standard_equip").ToString.ToUpper.Contains("Y"), "</b></font>", "</strong>"))
                End If
              End If

              If IIf(Not IsDBNull(r.Item("amfeat_feature_code")), r.Item("amfeat_feature_code").ToString.ToUpper, "") <> "DAM" Then
                If Not IsDBNull(r.Item("kfeat_description")) Then
                  If Not String.IsNullOrEmpty(r.Item("kfeat_description").ToString.Trim) Then
                    htmlOut.Append("<br/>" + r.Item("kfeat_description").ToString.Trim)
                  End If
                End If
              End If

              htmlOut.Append("</td><td align=""right"" valign=""middle"">")

              If Not IsDBNull(r.Item("ACFEATYES")) Then
                If Not String.IsNullOrEmpty(r.Item("ACFEATYES").ToString.Trim) Then
                  htmlOut.Append(r.Item("ACFEATYES").ToString.Trim)
                End If
              End If

              htmlOut.Append("</td><td align=""right"" valign=""middle"">")

              If Not IsDBNull(r.Item("ACFEATNO")) Then
                If Not String.IsNullOrEmpty(r.Item("ACFEATNO").ToString.Trim) Then
                  htmlOut.Append(r.Item("ACFEATNO").ToString.Trim)
                End If
              End If

              htmlOut.Append("</td><td align=""right"" valign=""middle"">")

              If Not IsDBNull(r.Item("ACFEATUNK")) Then
                If Not String.IsNullOrEmpty(r.Item("ACFEATUNK").ToString.Trim) Then
                  htmlOut.Append(r.Item("ACFEATUNK").ToString.Trim)
                End If
              End If

              htmlOut.Append("</td></tr>")

            Next

            htmlOut.Append("</table>")

          End If

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in FillModelCodes(): " + ex.Message
    End Try

    features_label.Text = htmlOut.ToString

    htmlOut = Nothing
    ModelCodes = Nothing

  End Sub

  Public Sub FillMaintenance()
    Dim MaintNote As String = ""
    Dim InspectionNote As String = ""
    maintenance_label.Text = ""

    If ModelDataTable.Rows.Count > 0 Then
      maintenance_label.Text = "<table width='100%' cellpadding='3' cellspacing='0'>"
      MaintNote = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_maint_note")), Trim(ModelDataTable.Rows(0).Item("amod_maint_note").ToString), "")
      InspectionNote = IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_inspection_note")), Trim(ModelDataTable.Rows(0).Item("amod_inspection_note").ToString), "")

      If MaintNote <> "" Then
        maintenance_label.Text += "<tr><td align='left' valign='top'><span class='li'><span class='label'>Maintenance Program:</span> " & MaintNote & "</span></td></tr>"
      End If

      If MaintNote <> "" Then
        maintenance_label.Text += "<tr><td align='left' valign='top'><span class='li'><span class='label'>Inspections:</span> " & InspectionNote & "</span></td></tr>"
      End If
      maintenance_label.Text += "</table>"
    End If
  End Sub

  Public Sub FillBasicConfiguration()
    Dim airframeType As String = "F"

    If ModelDataTable.Rows.Count > 0 Then
      airframeType = ModelDataTable.Rows(0).Item("amod_airframe_type_code")

      basic_label.Text = "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid darker_blue_border'>"
      basic_label.Text += "<tr><td align='left' valign='top' class='header_row cell_border_top'><b>Fuselage Dimensions</b></td>"
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Weight</b></td>"
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Climb</b></td></tr>"
      '''''''''''1st row
      basic_label.Text += "<tr>"
      'Fuse Length
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Length:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuselage_length")), ModelDataTable.Rows(0).Item("amod_fuselage_length").ToString, "") & "</span></td>"
      'Max Ramp
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Max Ramp (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_max_ramp_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_max_ramp_weight")), False, False, True), "") & "</span></td>"
      'Normal
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Normal (fpm):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_climb_normal_feet")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_climb_normal_feet")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"
      '''''''''''2nd row
      basic_label.Text += "<tr>"
      'Fuse Height
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Height:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuselage_height")), ModelDataTable.Rows(0).Item("amod_fuselage_height").ToString, "") & "</span></td>"
      'Max Takeoff
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Max Takeoff (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_max_takeoff_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_max_takeoff_weight")), False, False, True), "") & "</span></td>"
      'Engine Out
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Engine Out (fpm):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_climb_engout_feet")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_climb_engout_feet")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"

      '''''''''''3rd row
      basic_label.Text += "<tr>"
      'Wing Span
      If airframeType = "F" Then
        basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Wing Span:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuselage_wingspan")), ModelDataTable.Rows(0).Item("amod_fuselage_wingspan").ToString, "") & "</span></td>"
      ElseIf airframeType = "R" Then
        basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Width:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuselage_width")), ModelDataTable.Rows(0).Item("amod_fuselage_width").ToString, "") & "</span></td>"
      End If

      'Zero Fuel
      If airframeType = "F" Then
        basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Zero Fuel (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_zero_fuel_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_zero_fuel_weight")), False, False, True), "") & "</span></td>"
      ElseIf airframeType = "R" Then
        basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Empty Operating Weight (EOW):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_weight_eow")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_weight_eow")), False, False, True), "") & "</span></td>"
      End If

      'Ceiling
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Ceiling (fpm):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_ceiling_feet")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_ceiling_feet")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"

      '''''''''''4th row
      basic_label.Text += "<tr>"
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"

      'Basic Operating
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Basic Operating (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_basic_op_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_basic_op_weight")), False, False, True), "") & "</span></td>"

      If airframeType = "F" Then
        basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      ElseIf airframeType = "R" Then
        basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>HOGE - Out of Ground Effect:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_climb_hoge")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_climb_hoge")), False, False, True), "") & "</span></td>"
      End If

      basic_label.Text += "</tr>"

      ''''''''''Optional row.
      'basic_label.Text += "<tr>"
      'basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"

      'basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Basic Operating (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_basic_op_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_basic_op_weight")), False, False, True), "") & "</span></td>"

      'If airframeType = "F" Then
      '    basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      'ElseIf airframeType = "R" Then
      '    basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>HOGE - Out of Ground Effect:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_climb_hoge")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_climb_hoge")), False, False, True), "") & "</span></td>"
      'End If

      'basic_label.Text += "</tr>"

      '''''''''''5th row
      basic_label.Text += "<tr>"
      'Typical Header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Typical Configuration</b></td>"
      'Max Landing
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Max Landing (lbs):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_max_landing_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_max_landing_weight")), False, False, True), "") & "</span></td>"
      'Landing Header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Landing Performance</b></td>"
      basic_label.Text += "</tr>"

      '''''''''''6th row
      basic_label.Text += "<tr>"
      'crew
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Crew:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_number_of_crew")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_number_of_crew")), False, False, True), "") & "</span></td>"
      'Basic Operating
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      'Field length
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>FAA Field Length:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_field_length")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_field_length")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"

      '''''''''''7th row
      basic_label.Text += "<tr>"
      'Passengers
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Passengers:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_number_of_passengers")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_number_of_passengers")), False, False, True), "") & "</span></td>"
      'Basic Operating header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Speed (Knots)</b></td>"
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      basic_label.Text += "</tr>"

      '''''''''''8th row
      basic_label.Text += "<tr>"
      'Pressurization
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Pressurization (PSI):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_pressure")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_pressure")), 1, False, True), "") & "</span></td>"
      'VS
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>VS Clean:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_stall_vs")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_stall_vs")), False, False, True), "") & "</span></td>"
      'Takeoff header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Takeoff Performance</b></td>"
      basic_label.Text += "</tr>"

      '''''''''''9th row
      basic_label.Text += "<tr>"
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      'VSO
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>VSO Landing:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_stall_vso")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_stall_vso")), False, False, True), "") & "</span></td>"
      'SL ISA BFL
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>SL ISA BFL:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_takeoff_ali")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_takeoff_ali")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"

      '''''''''''9th row
      basic_label.Text += "<tr>"
      'Fuel Capacity header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Fuel Capacity</b></td>"
      'Normal
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Normal Cruise TAS:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_cruis_speed")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_cruis_speed")), False, False, True), "") & "</span></td>"
      '5000' +20C BFL
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>5000' +25C BLF:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_takeoff_500")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_takeoff_500")), False, False, True), "") & "</span></td>"
      basic_label.Text += "</tr>"

      '''''''''''10th row
      basic_label.Text += "<tr>"
      'Standard
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Standard:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_std_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_fuel_cap_std_weight")), False, False, True), "")

      If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_std_gal")) Then
        basic_label.Text += "&nbsp;lbs&nbsp;" & FormatNumber(CDbl(0 & ModelDataTable.Rows(0).Item("amod_fuel_cap_std_gal")), False, False, True) & "&nbsp;gal</span></td>"
      Else
        basic_label.Text += "&nbsp;lbs&nbsp;gal</span></td>"
      End If



      'VMO 
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Vmo (Max Op) IAS:</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_max_speed")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_max_speed")), False, False, True), "") & "</span></td>" ' 
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      basic_label.Text += "</tr>"

      '''''''''''11th row
      basic_label.Text += "<tr>"
      'Optional

      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Optional:</span> "

      If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_gal")) And Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_weight")) Then



        If CDbl(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_gal")) <> 0.0 Then
          basic_label.Text += IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_weight")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_weight")), False, False, True), "")
          basic_label.Text += "&nbsp;lbs"
        Else
          basic_label.Text += "&nbsp;lbs"
        End If

        If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_gal")) Then
          If CDbl(ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_gal")) <> 0.0 Then
            basic_label.Text += "&nbsp;" & FormatNumber(CDbl(0 & ModelDataTable.Rows(0).Item("amod_fuel_cap_opt_gal")), False, False, True) & "&nbsp;gal</span></td>"
          Else
            basic_label.Text += "&nbsp;</span></td>"
          End If
        Else
          basic_label.Text += "&nbsp;gal</span></td>"
        End If


      Else
        basic_label.Text += "&nbsp;</span></td>"
      End If

      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"

      'Range header
      basic_label.Text += "<td align='left' valign='top' class='header_row cell_border_top'><b>Range (Nautical Miles)</b>&nbsp;</td>"
      basic_label.Text += "</tr>"

      '''''''''''12th row
      basic_label.Text += "<tr>"
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      basic_label.Text += "<td align='left' valign='top'>&nbsp;</td>"
      'range
      basic_label.Text += "<td align='left' valign='top'><span class='li'><span class='label'>Range (nm):</span> " & IIf(Not IsDBNull(ModelDataTable.Rows(0).Item("amod_max_range_miles")), FormatNumber(CDbl(ModelDataTable.Rows(0).Item("amod_max_range_miles")), False, False, True), "") & "</span></td>"
      'amod_range_tanks_full could be this ? 
      basic_label.Text += "</tr>"


      If Not IsDBNull(ModelDataTable.Rows(0).Item("amod_other_config_note")) Then
        basic_label.Text += "<tr><td class='Normal' align='left' valign='middle' colspan='3'><b>Configuration Notes :</b> " & ModelDataTable.Rows(0).Item("amod_other_config_note") & "</td></tr>"
      End If


      basic_label.Text += "</table>"
    End If
  End Sub

  Public Sub FillCostsBudget()
    costs_label.Text += "<table width='100%' cellpadding='3' cellspacing='0'>"
    costs_label.Text += "<tr><td align='left' valign='top' width='33%'>"
    'DIRECT COSTS PER HOUR TABLE
    costs_label.Text += clsGeneral.clsGeneral.Build_Operating_Costs(ModelDataTable, masterPage.aclsData_Temp, 0, 0, False, True, False, False, True, False)
    costs_label.Text += "</td>"
    costs_label.Text += "<td align='left' valign='top' width='33%'>"
    'ANNUAL FIXED COSTS TABLE
    costs_label.Text += clsGeneral.clsGeneral.Build_Operating_Costs(ModelDataTable, masterPage.aclsData_Temp, 0, 0, False, False, True, False, True, False)
    costs_label.Text += "</td>"
    costs_label.Text += "<td align='left' valign='top' width='33%'>"
    'ANNUAL BUDGET TABLE
    costs_label.Text += clsGeneral.clsGeneral.Build_Operating_Costs(ModelDataTable, masterPage.aclsData_Temp, 0, 0, False, False, False, True, True, False)
    costs_label.Text += "</td>"
    costs_label.Text += "</tr>"
    costs_label.Text += "</table>"
  End Sub

  Public Function GetModelStandardFeatures(ByVal model_id As Long) As DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Try

      If bAdminModelDisplay Then

        sQuery.Append("SELECT DISTINCT amfeat_standard_equip, amfeat_feature_code, kfeat_name, kfeat_description,")
        sQuery.Append(" SUM(case when afeat_status_flag='Y' then 1 else 0 end) as ACFEATYES,")
        sQuery.Append(" SUM(case when afeat_status_flag='N' then 1 else 0 end) as ACFEATNO,")
        sQuery.Append(" SUM(case when afeat_status_flag='U' then 1 else 0 end) as ACFEATUNK")
        sQuery.Append(" FROM Aircraft_Model_Key_Feature WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Key_Feature WITH(NOLOCK) ON amfeat_feature_code = kfeat_code")
        sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_amod_id = amfeat_amod_id AND ac_journ_id = 0")
        sQuery.Append(" LEFT OUTER JOIN Aircraft_Key_Feature WITH(NOLOCK) ON afeat_feature_code = kfeat_code AND afeat_ac_id = ac_id AND afeat_journ_id = ac_journ_id")
        sQuery.Append(" WHERE kfeat_inactive_date IS NULL AND amfeat_amod_id = " + model_id.ToString)
        sQuery.Append(" GROUP BY amfeat_feature_code, amfeat_standard_equip, kfeat_name, kfeat_description")
        sQuery.Append(" ORDER BY amfeat_standard_equip DESC, amfeat_feature_code ASC")

      Else

        sQuery.Append("SELECT * FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK) WHERE kfeat_inactive_date IS NULL")
        sQuery.Append(" AND amfeat_feature_code = kfeat_code AND amfeat_amod_id = " + model_id.ToString)
        sQuery.Append(" ORDER BY amfeat_standard_equip DESC, amfeat_feature_code ASC")

      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
      End Try

    Catch ex As Exception
      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest(ByVal amod_id As Long) As DataTable: " + ex.Message
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

  Public Sub FillDealer()

    Dim comp_link As String = ""
    Dim ac_rank As Integer = 0

    Dim htmlOut_Dealers_AC As StringBuilder = New StringBuilder()
    Dim htmlOut_Dealers_Trans As StringBuilder = New StringBuilder()
    Dim htmlOut As StringBuilder = New StringBuilder()

    Dim acdealer_view_function As New aircraft_dealer_functions
    Dim tempTable As New DataTable
    Dim toggleRowColor As Boolean = False

    acdealer_view_function.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    acdealer_view_function.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    acdealer_view_function.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    acdealer_view_function.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    acdealer_view_function.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    tempTable = acdealer_view_function.ac_dealer_function_ac_count(0, ModelID, "", "", Nothing)

    Dim model_name As String = ""

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        model_name = commonEvo.Get_Aircraft_Model_Info(ModelID, False, "")

        htmlOut_Dealers_AC.Append("<table id=""modelACDealersTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_Dealers_AC.Append("<th class=""header_row cell_border_top"">RANK</th>")
        htmlOut_Dealers_AC.Append("<th class=""header_row cell_border_top"" width=""290"">CURRENT " + model_name.Trim + " DEALERS</th>")
        htmlOut_Dealers_AC.Append("<th class=""header_row cell_border_top"">AC</th>")
        htmlOut_Dealers_AC.Append("</tr>")

        ac_rank = 0

        For Each r As DataRow In tempTable.Rows

          ac_rank += 1

          If Not IsDBNull(r("comp_name")) Then
            comp_link = r("comp_name").ToString
          End If

          If Not toggleRowColor Then
            htmlOut_Dealers_AC.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_Dealers_AC.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_Dealers_AC.Append("<td class=""text_align_right"" nowrap=""nowrap"">" + ac_rank.ToString + "</td>")

          If Not String.IsNullOrEmpty(comp_link.Trim) Then

            If comp_link.Length > 72 Then
              comp_link = Left(comp_link, 72) + "..."
            End If

                        comp_link = "<a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("broker_main_comp_id").ToString + "&journid=0&amod_id=" + ModelID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" + comp_link.Trim + "</a>"

                        htmlOut_Dealers_AC.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""290"">" + comp_link)

            If Not IsDBNull(r("NUMLOCATIONS")) Then
              If r("NUMLOCATIONS") > 1 Then
                htmlOut_Dealers_AC.Append(" (" + r.Item("NUMLOCATIONS").ToString + " Locations)")
              Else
                htmlOut_Dealers_AC.Append(" (1 Location)")
              End If
            End If

            htmlOut_Dealers_AC.Append("</td>")

          Else
            htmlOut_Dealers_AC.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""290"">&nbsp;</td>")
          End If

          If Not IsDBNull(r("ACCOUNT")) Then
            htmlOut_Dealers_AC.Append("<td class=""text_align_right"" nowrap=""nowrap"">" + r.Item("ACCOUNT").ToString + "</td>")
          Else
            htmlOut_Dealers_AC.Append("<td class=""text_align_right"" nowrap=""nowrap"">0</td>")
          End If

          htmlOut_Dealers_AC.Append("</tr>")

        Next

        htmlOut_Dealers_AC.Append("</table>")

      End If

    End If

    If Not IsNothing(tempTable) Then
      tempTable.Clear()
    End If

    tempTable = acdealer_view_function.ac_dealer_function_ac_sales("", ModelID, "", Nothing)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlOut_Dealers_Trans.Append("<table id=""modelDealersTransTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_Dealers_Trans.Append("<th class=""header_row cell_border_top"">RANK</th>")
        htmlOut_Dealers_Trans.Append("<th class=""header_row cell_border_top"" width=""270"">" + model_name.Trim + "  DEALER SALES (" + (Year(Date.Now()) - 1).ToString + ")</th>")
        htmlOut_Dealers_Trans.Append("<th class=""header_row cell_border_top"">TRANS</th>")
        htmlOut_Dealers_Trans.Append("</tr>")

        ac_rank = 0
        For Each r As DataRow In tempTable.Rows

          ac_rank += 1

          If Not toggleRowColor Then
            htmlOut_Dealers_Trans.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_Dealers_Trans.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_Dealers_Trans.Append("<td class=""text_align_right"" nowrap=""nowrap"">" + ac_rank.ToString + "</td>")

          If Not IsDBNull(r("comp_name")) Then
            comp_link = r("comp_name").ToString
          End If

          If Not String.IsNullOrEmpty(comp_link.Trim) Then

            If comp_link.Length > 72 Then
              comp_link = Left(comp_link, 72) + "..."
            End If

                        comp_link = "<a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("broker_main_comp_id").ToString + "&journid=0&amod_id=" + ModelID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" + comp_link.Trim + "</a>"


                        htmlOut_Dealers_Trans.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""270"">" + comp_link)
            htmlOut_Dealers_Trans.Append("</td>")

          Else
            htmlOut_Dealers_Trans.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""270"">&nbsp;</td>")
          End If

          If Not IsDBNull(r("numtrans")) Then
            htmlOut_Dealers_Trans.Append("<td class=""text_align_right"" nowrap=""nowrap"">" + r.Item("numtrans").ToString + "</td>")
          Else
            htmlOut_Dealers_Trans.Append("<td class=""text_align_right"" nowrap=""nowrap"">0</td>")
          End If

          htmlOut_Dealers_Trans.Append("</tr>")

        Next

        htmlOut_Dealers_Trans.Append("</table>")

      End If

    End If


    htmlOut.Append("<table id=""modelDealersTabOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""2"">")
    htmlOut.Append("<tr><td align=""left"" valign=""top"" width=""50%"">")
    htmlOut.Append(htmlOut_Dealers_AC.ToString)
    htmlOut.Append("</td><td align=""left"" valign=""top"">")
    htmlOut.Append(htmlOut_Dealers_Trans.ToString)
    htmlOut.Append("</td></tr></table>")

    dealer_label.Text = htmlOut.ToString
    dealer.Visible = True

  End Sub

  Public Sub FillUserInterest()

    Dim htmlUserInterestGraph As String = ""
    Dim htmlUserInterestGraphScript As String = ""
    Dim htmlUserInterestFunctionScript As String = ""

    Dim htmlOut As New StringBuilder

    Dim graphID As Integer = 3

    model_user_interest_graph(ModelID, htmlUserInterestFunctionScript, htmlUserInterestGraph, graphID)

    If Not String.IsNullOrEmpty(htmlUserInterestFunctionScript.Trim) Then

      htmlUserInterestGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlUserInterestGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlUserInterestGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlUserInterestGraphScript += "});" + vbCrLf
      htmlUserInterestGraphScript += htmlUserInterestFunctionScript.Trim
      htmlUserInterestGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(userInterest, userInterest.GetType(), "showInterestGraph" + graphID.ToString, htmlUserInterestGraphScript, False)

    End If

    htmlOut.Append("<table id=""modelRankingTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
    htmlOut.Append("<tr><td valign=""top"" align=""left"">" + model_user_interest_rank(ModelID).Trim + "</td></tr>")
    htmlOut.Append("</table><br/>" + vbCrLf)

    userInterest_label.Text = htmlOut.ToString ' add ranking

    userInterest_label.Text += htmlUserInterestGraph.ToString

    userInterest.Visible = True

  End Sub

  Public Function model_user_interest_rank(ByVal amod_id As Long) As String

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim nModelRank As Long = 0
    Dim sAirframeModelType As String = ""
    Dim sAirTypeArray As String() = Nothing
    Dim htmlOut As New StringBuilder

    Try

      Dim modelInfo As String = commonEvo.Get_Aircraft_Model_Info(amod_id, False, "", sAirframeModelType)

      sAirTypeArray = sAirframeModelType.Split(":")

            'sQuery.Append(" SELECT DISTINCT amod_id, COUNT(*) AS tcount FROM Subscription_Install_Log WITH(NOLOCK)")
            'sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON subislog_amod_id = amod_id")
            'sQuery.Append(" WHERE subislog_date >= GETDATE()-30")
            'sQuery.Append(" AND amod_airframe_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(0).ToString), sAirTypeArray(0).ToString, "F") + "'")
            'sQuery.Append(" AND amod_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(1).ToString), sAirTypeArray(1).ToString, "J") + "'")
            'sQuery.Append(" AND subislog_subid NOT IN(888, 777, 9)")
            'sQuery.Append(" GROUP BY amod_id")
            'sQuery.Append(" ORDER BY COUNT(*) desc")



            sQuery.Append(" Select distinct amodrank_amod_id, amodrank_count ")
            sQuery.Append(" From Aircraft_Model_Rank with (NOLOCK) ")
            sQuery.Append(" inner Join aircraft_model with (NOLOCK) on amod_id = amodrank_amod_id And amod_airframe_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(0).ToString), sAirTypeArray(0).ToString, "F") + "'  AND amod_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(1).ToString), sAirTypeArray(1).ToString, "J") + "'   ")
            sQuery.Append(" where amodrank_year = Year(getdate()) And amodrank_month = (Month(getdate()) - 1) ")
            ' sQuery.Append(" And amodrank_amod_id  Not In (" & amod_id & ") ")
            sQuery.Append(" Group BY amodrank_amod_id, amodrank_count ")
            sQuery.Append(" ORDER BY amodrank_count desc  ")

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If temptable.Rows.Count > 0 Then

        For Each r As DataRow In temptable.Rows
          nModelRank += 1

                    If CLng(r("amodrank_amod_id").ToString) = amod_id Then
                        Exit For
                    End If

                Next

      End If

      htmlOut.Append("Ranks <strong>" + nModelRank.ToString + "</strong> of <strong>" + temptable.Rows.Count.ToString + "</strong> for Models of Same Type")

    Catch ex As Exception

      Return ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest_rank(ByVal amod_id As Long) As Long : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

    htmlOut = Nothing

  End Function

  Public Function model_user_interest_other_clicks(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim sAirframeModelType As String = ""
    Dim sAirTypeArray As String() = Nothing

    Try

      Dim modelInfo As String = commonEvo.Get_Aircraft_Model_Info(amod_id, False, "", sAirframeModelType)

      sAirTypeArray = sAirframeModelType.Split(":")

            'sQuery.Append(" SELECT YEAR(subislog_date) AS tyear, MONTH(subislog_date) AS tmonth, (COUNT(*)/COUNT(DISTINCT subislog_amod_id)) AS tcount")
            'sQuery.Append(" FROM Subscription_Install_Log WITH(NOLOCK)")
            'sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON subislog_amod_id = amod_id")
            'sQuery.Append(" WHERE subislog_amod_id <> " + amod_id.ToString)
            'sQuery.Append(" AND YEAR(subislog_date) = YEAR(getdate())")
            'sQuery.Append(" AND amod_airframe_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(0).ToString), sAirTypeArray(0).ToString, "F") + "'")
            'sQuery.Append(" AND amod_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(1).ToString), sAirTypeArray(1).ToString, "J") + "'")
            'sQuery.Append(" AND subislog_subid NOT IN(888,777,9)")
            'sQuery.Append(" GROUP BY YEAR(subislog_date),MONTH(subislog_date)")
            'sQuery.Append(" ORDER BY YEAR(subislog_date),MONTH(subislog_date)")

            sQuery.Append("  Select ")
            sQuery.Append(" amodrank_year  AS tyear, amodrank_month  As tmonth, ")
            sQuery.Append(" (sum(amodrank_count)/COUNT(DISTINCT amodrank_amod_id)) AS tcount , ")
            sQuery.Append(" COUNT(DISTINCT amodrank_amod_id), ")
            sQuery.Append(" sum(amodrank_count) ")
            sQuery.Append(" From Aircraft_Model_Rank with (NOLOCK) ")
            sQuery.Append(" inner Join aircraft_model with (NOLOCK) on amod_id = amodrank_amod_id And amod_airframe_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(0).ToString), sAirTypeArray(0).ToString, "F") + "'  AND amod_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(1).ToString), sAirTypeArray(1).ToString, "J") + "'   ")
            sQuery.Append(" where amodrank_year = year(getdate()) ")
            sQuery.Append(" And amodrank_amod_id  Not in (" & amod_id & ") ")

            sQuery.Append(" Group BY amodrank_year, amodrank_month ")
            sQuery.Append(" ORDER BY amodrank_year, amodrank_month ")



            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest_rank(ByVal amod_id As Long) As Long : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function model_user_interest(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Dim sAirframeModelType As String = ""
        Dim sAirTypeArray As String() = Nothing

        Try


            'sQuery.Append(" SELECT YEAR(subislog_date) AS tyear, MONTH(subislog_date) AS tmonth, COUNT(*) AS tcount FROM Subscription_Install_Log WITH(NOLOCK)")
            'sQuery.Append(" WHERE subislog_amod_id = " + amod_id.ToString)
            'sQuery.Append(" AND subislog_date >= GETDATE()-730")
            'sQuery.Append(" AND subislog_subid NOT IN(888, 777, 9)")
            'sQuery.Append(" GROUP BY YEAR(subislog_date), MONTH(subislog_date)")
            'sQuery.Append(" ORDER BY YEAR(subislog_date), MONTH(subislog_date)") 

            Dim modelInfo As String = commonEvo.Get_Aircraft_Model_Info(amod_id, False, "", sAirframeModelType)

            sAirTypeArray = sAirframeModelType.Split(":")

            sQuery.Append("  Select ")
            sQuery.Append(" amodrank_year AS tyear, amodrank_month  As tmonth, ")
            sQuery.Append(" (sum(amodrank_count)/COUNT(DISTINCT amodrank_amod_id)) AS tcount , ")
            '  sQuery.Append(" COUNT(DISTINCT amodrank_amod_id), ")
            sQuery.Append(" sum(amodrank_count) ")
            sQuery.Append(" From Aircraft_Model_Rank with (NOLOCK) ")
            sQuery.Append(" inner Join aircraft_model with (NOLOCK) on amod_id = amodrank_amod_id And amod_airframe_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(0).ToString), sAirTypeArray(0).ToString, "F") + "'  AND amod_type_code = '" + IIf(Not String.IsNullOrEmpty(sAirTypeArray(1).ToString), sAirTypeArray(1).ToString, "J") + "'   ")
            sQuery.Append(" where amodrank_year = year(getdate()) and  amodrank_month = month(getdate()) ")
            sQuery.Append(" And amodrank_amod_id =" & amod_id & " ")

            sQuery.Append(" Group BY amodrank_year, amodrank_month ")
            sQuery.Append(" ORDER BY amodrank_year, amodrank_month ")



            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Sub model_user_interest_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable
    Dim other_clicks_table As New DataTable

    Dim sYear As String = ""

    Dim sMonthArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12", ",")
    Dim sYearArray() As String = Nothing

    Dim afiltered_Rows As DataRow() = Nothing

    Dim bHadValue As Boolean = False

    Try

      results_table = model_user_interest(amod_id)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('number', 'Month');" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not commonEvo.inMyArray(sYear.Split(","), r.Item("tyear").ToString) Then

              If String.IsNullOrEmpty(sYear.Trim) Then
                sYear = r.Item("tyear").ToString
              Else
                sYear += "," + r.Item("tyear").ToString
              End If

              scriptOut.Append(" data.addColumn('number', 'Clicks " + r.Item("tyear").ToString.Trim + "');" + vbCrLf)

            End If

          Next

          ' add the other clicks column
          scriptOut.Append(" data.addColumn('number', 'Average Clicks Per Model');" + vbCrLf)
          other_clicks_table = model_user_interest_other_clicks(amod_id)

          sYearArray = sYear.Split(",")

          scriptOut.Append(" data.addRows([")

          For Each strMO As String In sMonthArray

            scriptOut.Append(IIf(CInt(strMO.Trim) > 1, ", [" + strMO.Trim, " [" + strMO.Trim))

            For Each strYR As String In sYearArray

              afiltered_Rows = results_table.Select("tmonth = " + strMO.Trim + " AND tyear = " + strYR.Trim, "")

              If afiltered_Rows.Count > 0 Then

                For Each r As DataRow In afiltered_Rows

                  If Not IsDBNull(r.Item("tcount")) Then
                    If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                      If CLng(r.Item("tcount").ToString) > 0 Then
                        scriptOut.Append("," + r.Item("tcount").ToString)
                      Else
                        scriptOut.Append(",0")
                      End If

                      Exit For

                    Else
                      scriptOut.Append(",0")
                      Exit For
                    End If

                  Else
                    scriptOut.Append(",0")
                    Exit For
                  End If

                Next

              Else
                scriptOut.Append(",0")
              End If

            Next

            bHadValue = False

            If other_clicks_table.Rows.Count > 0 Then

              For Each r As DataRow In other_clicks_table.Rows

                If Not IsDBNull(r.Item("tcount")) Then
                  If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                    If CInt(r.Item("tmonth").ToString) = CInt(strMO) Then

                      bHadValue = True

                      If CLng(r.Item("tcount").ToString) > 0 Then
                        scriptOut.Append("," + r.Item("tcount").ToString)
                      Else
                        scriptOut.Append(",0")
                      End If

                      Exit For

                    End If

                  Else
                    Exit For
                  End If

                Else
                  Exit For
                End If

              Next

              If Not bHadValue Then
                scriptOut.Append(",0")
              End If

            Else
              scriptOut.Append(",0")
            End If

            scriptOut.Append("]")

          Next


          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Month'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'Clicks'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:true," + vbCrLf)
          scriptOut.Append("  legend:'top'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modelUserInterestTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modelUserInterestTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No User Interest Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub FillMaintDetails()

    Dim htmlOut_TopInspections As StringBuilder = New StringBuilder()
    Dim htmlOut_TopHowMaintained As StringBuilder = New StringBuilder()
    Dim htmlOut_InspectionSchedule As StringBuilder = New StringBuilder()
    Dim htmlOut_TopProgramNames As StringBuilder = New StringBuilder()
    Dim htmlOut As StringBuilder = New StringBuilder()

    Dim tempTable As New DataTable
    Dim toggleRowColor As Boolean = False

    tempTable = GetMaintenanceDetailsTopInspections(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlOut_TopInspections.Append("<table id=""modelMaintenanceDetailsTopInspectionsTable"" cellpadding=""2"" cellspacing=""2"" width=""98%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_TopInspections.Append("<th class=""header_row cell_border_top"">TOP MAINTENANCE / INSPECTIONS</th>")
        htmlOut_TopInspections.Append("<th class=""header_row cell_border_top"">COUNT</th>")
        htmlOut_TopInspections.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          If Not toggleRowColor Then
            htmlOut_TopInspections.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_TopInspections.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_TopInspections.Append("<td class=""text_align_left"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("MITEMNAME")) Then
            If Not String.IsNullOrEmpty(r.Item("MITEMNAME").ToString.Trim) Then
              htmlOut_TopInspections.Append(r.Item("MITEMNAME").ToString.Trim)
            End If
          End If

          htmlOut_TopInspections.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tcount")) Then
            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

              If CLng(r.Item("tcount").ToString) > 0 Then
                htmlOut_TopInspections.Append(r.Item("tcount").ToString)
              Else
                htmlOut_TopInspections.Append("0")
              End If

            End If
          End If

          htmlOut_TopInspections.Append("</td></tr>")

        Next

        htmlOut_TopInspections.Append("</table>")

      End If

    End If

    If Not IsNothing(tempTable) Then
      tempTable.Clear()
    End If

    tempTable = GetMaintenanceDetailsInspectionSchedule(ModelID)

    Dim afiltered_TimeUsage As DataRow() = Nothing
    Dim afiltered_General As DataRow() = Nothing

    If Not IsNothing(tempTable) Then

      afiltered_TimeUsage = tempTable.Select("amitem_increment IN('Time','Usage')", "amitem_increment, amitem_sort, amitem_name")

      If afiltered_TimeUsage.Length > 0 Then

        htmlOut_InspectionSchedule.Append("<table id=""modelMaintenanceDetailsInspectionScheduleTable"" cellpadding=""2"" cellspacing=""2"" width=""98%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_InspectionSchedule.Append("<th class=""header_row cell_border_top"">COMMON ITEM / INSPECTION</th>")
        htmlOut_InspectionSchedule.Append("<th class=""header_row cell_border_top"">DESCRIPTION / NOTES</th>")
        htmlOut_InspectionSchedule.Append("</tr>")

        For Each r As DataRow In afiltered_TimeUsage

          If Not toggleRowColor Then
            htmlOut_InspectionSchedule.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_InspectionSchedule.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_InspectionSchedule.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""40%"">")

          If Not IsDBNull(r.Item("amitem_name")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_name").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append(r.Item("amitem_name").ToString.Trim)
            End If
          End If

          If Not IsDBNull(r.Item("amitem_alias")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_alias").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append("&nbsp;/&nbsp;" + r.Item("amitem_alias").ToString.Trim)
            End If
          End If

          htmlOut_InspectionSchedule.Append("&nbsp;</td><td class=""text_align_left"">")

          If Not IsDBNull(r.Item("amitem_description")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_description").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append(r.Item("amitem_description").ToString.Trim)
            End If
          End If

          If Not IsDBNull(r.Item("amitem_internal_notes")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_internal_notes").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append("&nbsp;/&nbsp;" + r.Item("amitem_internal_notes").ToString.Trim)
            End If
          End If

          htmlOut_InspectionSchedule.Append("&nbsp;</td></tr>")

        Next

        htmlOut_InspectionSchedule.Append("</table>")

      End If

      afiltered_General = tempTable.Select("amitem_increment IN('General')", "amitem_increment, amitem_sort, amitem_name")

      If afiltered_General.Length > 0 Then

        htmlOut_InspectionSchedule.Append("<br/><table id=""modelMaintenanceDetailsInspectionScheduleTable2"" cellpadding=""2"" cellspacing=""2"" width=""98%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_InspectionSchedule.Append("<th class=""header_row cell_border_top"">EQUIPMENT ITEMS / INSPECTIONS</th>")
        htmlOut_InspectionSchedule.Append("<th class=""header_row cell_border_top"">DESCRIPTION / NOTES</th>")
        htmlOut_InspectionSchedule.Append("</tr>")

        For Each r As DataRow In afiltered_General

          If Not toggleRowColor Then
            htmlOut_InspectionSchedule.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_InspectionSchedule.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_InspectionSchedule.Append("<td class=""text_align_left"" nowrap=""nowrap"" width=""40%"">")

          If Not IsDBNull(r.Item("amitem_name")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_name").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append(r.Item("amitem_name").ToString.Trim)
            End If
          End If

          If Not IsDBNull(r.Item("amitem_alias")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_alias").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append("&nbsp;/&nbsp;" + r.Item("amitem_alias").ToString.Trim)
            End If
          End If

          htmlOut_InspectionSchedule.Append("&nbsp;</td><td class=""text_align_left"">")

          If Not IsDBNull(r.Item("amitem_description")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_description").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append(r.Item("amitem_description").ToString.Trim)
            End If
          End If

          If Not IsDBNull(r.Item("amitem_internal_notes")) Then
            If Not String.IsNullOrEmpty(r.Item("amitem_internal_notes").ToString.Trim) Then
              htmlOut_InspectionSchedule.Append("&nbsp;/&nbsp;" + r.Item("amitem_internal_notes").ToString.Trim)
            End If
          End If

          htmlOut_InspectionSchedule.Append("&nbsp;</td></tr>")

        Next

        htmlOut_InspectionSchedule.Append("</table>")

      End If

    End If

    afiltered_TimeUsage = Nothing
    afiltered_General = Nothing

    If Not IsNothing(tempTable) Then
      tempTable.Clear()
    End If

    tempTable = GetMaintenanceDetailsTopHowMaintained(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlOut_TopHowMaintained.Append("<table id=""modelMaintenanceDetailsTopHowMaintainedTable"" cellpadding=""2"" cellspacing=""2"" width=""98%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_TopHowMaintained.Append("<th class=""header_row cell_border_top"">TOP HOW MAINTAINED</th>")
        htmlOut_TopHowMaintained.Append("<th class=""header_row cell_border_top"">COUNT</th>")
        htmlOut_TopHowMaintained.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          If Not toggleRowColor Then
            htmlOut_TopHowMaintained.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_TopHowMaintained.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_TopHowMaintained.Append("<td class=""text_align_left"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("MAINTAINED")) Then
            If Not String.IsNullOrEmpty(r.Item("MAINTAINED").ToString.Trim) Then
              htmlOut_TopHowMaintained.Append(r.Item("MAINTAINED").ToString.Trim)
            End If
          End If

          htmlOut_TopHowMaintained.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tcount")) Then
            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

              If CLng(r.Item("tcount").ToString) > 0 Then
                htmlOut_TopHowMaintained.Append(r.Item("tcount").ToString)
              Else
                htmlOut_TopHowMaintained.Append("0")
              End If

            End If
          End If

          htmlOut_TopHowMaintained.Append("</td></tr>")

        Next

        htmlOut_TopHowMaintained.Append("</table>")

      End If

    End If

    If Not IsNothing(tempTable) Then
      tempTable.Clear()
    End If

    tempTable = GetMaintenanceDetailsTopProgramNames(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlOut_TopProgramNames.Append("<table id=""modelMaintenanceDetailsTopProgramNamesTable"" cellpadding=""2"" cellspacing=""2"" width=""98%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_TopProgramNames.Append("<th class=""header_row cell_border_top"">TOP MAINTENANCE PROGRAM NAMES</th>")
        htmlOut_TopProgramNames.Append("<th class=""header_row cell_border_top"">COUNT</th>")
        htmlOut_TopProgramNames.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          If Not toggleRowColor Then
            htmlOut_TopProgramNames.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut_TopProgramNames.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut_TopProgramNames.Append("<td class=""text_align_left"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("ENGPROGNAME")) Then
            If Not String.IsNullOrEmpty(r.Item("ENGPROGNAME").ToString.Trim) Then
              htmlOut_TopProgramNames.Append(r.Item("ENGPROGNAME").ToString.Trim)
            End If
          End If

          htmlOut_TopProgramNames.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tcount")) Then
            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

              If CLng(r.Item("tcount").ToString) > 0 Then
                htmlOut_TopProgramNames.Append(r.Item("tcount").ToString)
              Else
                htmlOut_TopProgramNames.Append("0")
              End If

            End If
          End If

          htmlOut_TopProgramNames.Append("</td></tr>")

        Next

        htmlOut_TopProgramNames.Append("</table>")

      End If

    End If

    htmlOut.Append("<table id=""modelMaintenanceDetailsOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""2"">")
    htmlOut.Append("<tr><td align=""left"" valign=""top"" width=""50%"">")
    htmlOut.Append(htmlOut_TopInspections.ToString)
    htmlOut.Append("</td><td align=""left"" valign=""top"">")
    htmlOut.Append(htmlOut_InspectionSchedule.ToString)
    htmlOut.Append("</td><tr><td align=""left"" valign=""top"" width=""50%"">")
    htmlOut.Append(htmlOut_TopHowMaintained.ToString)
    htmlOut.Append("</td><td align=""left"" valign=""top"">")
    htmlOut.Append(htmlOut_TopProgramNames.ToString)
    htmlOut.Append("</td></tr></table>")

    maintenanceDetails_label.Text = htmlOut.ToString
    maintenanceDetails_label.Visible = True

    tempTable = Nothing
    htmlOut = Nothing

    htmlOut_TopInspections = Nothing
    htmlOut_TopHowMaintained = Nothing
    htmlOut_TopProgramNames = Nothing

  End Sub

  Public Function GetMaintenanceDetailsTopInspections(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT acmaint_name AS MITEMNAME, COUNT(*) AS tcount")
      sQuery.Append(" FROM Aircraft_Maintenance WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON acmaint_ac_id = ac_id AND acmaint_journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE acmaint_journ_id = 0 AND amod_id = " + amod_id.ToString)
      sQuery.Append(" GROUP BY acmaint_name")
      sQuery.Append(" ORDER BY COUNT(*) desc")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetMaintenanceDetailsTopInspections(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function GetMaintenanceDetailsInspectionSchedule(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT amitem_name, amitem_alias, amitem_description, amitem_duration, amitem_increment, amitem_sort, amitem_internal_notes")
      sQuery.Append(" FROM Aircraft_Model_Maintenance_Item WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = amitem_amod_id")
      sQuery.Append(" WHERE amitem_active_flag = 'Y' AND amitem_amod_id = " + amod_id.ToString)
      sQuery.Append(" ORDER BY amitem_increment, amitem_sort, amitem_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetMaintenanceDetailsInspectionSchedule(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function GetMaintenanceDetailsTopHowMaintained(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT (CASE WHEN ac_maintained IS NULL THEN 'Unknown' ELSE ac_maintained END) AS  MAINTAINED, COUNT(*) AS tcount")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE ac_journ_id = 0 AND amod_id = " + amod_id.ToString)
      sQuery.Append(" GROUP BY ac_maintained")
      sQuery.Append(" ORDER BY COUNT(*) desc")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetMaintenanceDetailsTopHowMaintained(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function GetMaintenanceDetailsTopProgramNames(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT emp_program_name AS ENGPROGNAME, COUNT(*) AS tcount")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" INNER JOIN Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = emp_id")
      sQuery.Append(" WHERE ac_journ_id = 0 AND amod_id = " + amod_id.ToString)
      sQuery.Append(" GROUP BY emp_program_name")
      sQuery.Append(" ORDER BY COUNT(*) desc")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetMaintenanceDetailsTopProgramNames(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Sub FillSalePrices()

    Dim htmlRetailSalePrices As StringBuilder = New StringBuilder()

    Dim htmlSalePricesBarChart As String = ""
    Dim htmlSalePricesBarChartScript As String = ""
    Dim htmlSalePricesBarChartFunctionScript As String = ""

    Dim htmlSalePricesByCompany As StringBuilder = New StringBuilder()

    Dim htmlSaleVsAskLineGraph As String = ""
    Dim htmlSaleVsAskLineGraphScript As String = ""
    Dim htmlSaleVsAskLineFunctionScript As String = ""

    Dim graphID As Integer = 1

    Dim htmlOut As StringBuilder = New StringBuilder()

    Dim tempTable As New DataTable

    Dim nTransCount As Long = 0
    Dim nPriceCount As Long = 0

    Dim toggleRowColor As Boolean = False

    tempTable = GetRetailSalePricesByYearOfSale(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlRetailSalePrices.Append("<table id=""modelRetailSalePricesByYearTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlRetailSalePrices.Append("<th class=""header_row cell_border_top"">YEAR</th>")
        htmlRetailSalePrices.Append("<th class=""header_row cell_border_top"">TRANSACTIONS</th>")
        htmlRetailSalePrices.Append("<th class=""header_row cell_border_top"">SALE PRICES</th>")
        htmlRetailSalePrices.Append("<th class=""header_row cell_border_top"">% PRICES</th>")
        htmlRetailSalePrices.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          nTransCount = 0
          nPriceCount = 0

          If Not toggleRowColor Then
            htmlRetailSalePrices.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlRetailSalePrices.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlRetailSalePrices.Append("<td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tyear")) Then
            If Not String.IsNullOrEmpty(r.Item("tyear").ToString.Trim) Then

              If CLng(r.Item("tyear").ToString) > 0 Then
                htmlRetailSalePrices.Append(r.Item("tyear").ToString)
              Else
                htmlRetailSalePrices.Append("&nbsp;")
              End If

            End If
          End If

          htmlRetailSalePrices.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("transcount")) Then
            If Not String.IsNullOrEmpty(r.Item("transcount").ToString.Trim) Then

              If CLng(r.Item("transcount").ToString) > 0 Then
                nTransCount = CLng(r.Item("transcount").ToString)
                htmlRetailSalePrices.Append(r.Item("transcount").ToString)
              Else
                htmlRetailSalePrices.Append("0")
              End If

            End If
          End If

          htmlRetailSalePrices.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("saleprices")) Then
            If Not String.IsNullOrEmpty(r.Item("saleprices").ToString.Trim) Then

              If CLng(r.Item("saleprices").ToString) > 0 Then
                nPriceCount = CLng(r.Item("saleprices").ToString)
                htmlRetailSalePrices.Append(r.Item("saleprices").ToString)
              Else
                htmlRetailSalePrices.Append("0")
              End If

            End If
          End If

          htmlRetailSalePrices.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If nTransCount > 0 Then
            htmlRetailSalePrices.Append(FormatNumber(System.Math.Round(CDbl((nPriceCount / nTransCount) * 100), 2), 1, False, False, False) + "%")
          Else
            htmlRetailSalePrices.Append("0")
          End If

          htmlRetailSalePrices.Append("</td></tr>")

        Next

        htmlRetailSalePrices.Append("</table>")

      End If

    End If

    If Not IsNothing(tempTable) Then
      tempTable.Clear()
    End If

    tempTable = GetSalePricesReceivedByCompany(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlSalePricesByCompany.Append("<table id=""modelSalePricesByCompanyTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlSalePricesByCompany.Append("<th class=""header_row cell_border_top"">COMPANY NAME</th>")
        htmlSalePricesByCompany.Append("<th class=""header_row cell_border_top"">COUNT</th>")
        htmlSalePricesByCompany.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          If Not toggleRowColor Then
            htmlSalePricesByCompany.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlSalePricesByCompany.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlSalePricesByCompany.Append("<td class=""text_align_left"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("comp_name")) Then
            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                            htmlSalePricesByCompany.Append("<a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0&amod_id=" + ModelID.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            htmlSalePricesByCompany.Append(r.Item("comp_name").ToString.Trim + "</a>")
            Else
              htmlSalePricesByCompany.Append("&nbsp;")
            End If
          Else
            htmlSalePricesByCompany.Append("&nbsp;")
          End If

          htmlSalePricesByCompany.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tcount")) Then
            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

              If CLng(r.Item("tcount").ToString) > 0 Then
                htmlSalePricesByCompany.Append(r.Item("tcount").ToString)
              Else
                htmlSalePricesByCompany.Append("0")
              End If

            End If
          End If

          htmlSalePricesByCompany.Append("</td></tr>")

        Next

        htmlSalePricesByCompany.Append("</table>")

      End If

    End If

    sales_vs_ask_line_graph(ModelID, htmlSaleVsAskLineFunctionScript, htmlSaleVsAskLineGraph, graphID)

    If Not String.IsNullOrEmpty(htmlSaleVsAskLineFunctionScript.Trim) Then

      htmlSaleVsAskLineGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlSaleVsAskLineGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlSaleVsAskLineGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlSaleVsAskLineGraphScript += "});" + vbCrLf
      htmlSaleVsAskLineGraphScript += htmlSaleVsAskLineFunctionScript.Trim
      htmlSaleVsAskLineGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(sale, sale.GetType(), "showSaleVsAskLineGraph" + graphID.ToString, htmlSaleVsAskLineGraphScript, False)

    End If

    graphID += 1

    sales_bar_chart(ModelID, htmlSalePricesBarChartFunctionScript, htmlSalePricesBarChart, graphID)

    If Not String.IsNullOrEmpty(htmlSalePricesBarChartFunctionScript.Trim) Then

      htmlSalePricesBarChartScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlSalePricesBarChartScript += "$(document).ready(function(){" + vbCrLf
      htmlSalePricesBarChartScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlSalePricesBarChartScript += "});" + vbCrLf
      htmlSalePricesBarChartScript += htmlSalePricesBarChartFunctionScript.Trim
      htmlSalePricesBarChartScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(sale, sale.GetType(), "showSalePricesBarChart" + graphID.ToString, htmlSalePricesBarChartScript, False)

    End If

    htmlOut.Append("<table id=""modelSalePricesOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""2"">")
    htmlOut.Append("<tr><td align=""left"" valign=""top"" width=""50%""><span class='label'>SALE PRICES COLLECTED BY YEAR OF SALE</span><br/>")
    htmlOut.Append(IIf(Not String.IsNullOrEmpty(htmlRetailSalePrices.ToString.Trim), htmlRetailSalePrices.ToString, "<strong> No Sale Prices Collected By Year Of Sale Data for this Model </strong>"))
    htmlOut.Append("</td><td align=""left"" valign=""top""><span class='label'>SALE PRICE SOURCES/DATA PROVIDERS</span><br/>")
    htmlOut.Append(IIf(Not String.IsNullOrEmpty(htmlSalePricesByCompany.ToString.Trim), htmlSalePricesByCompany.ToString, "<strong> No Sale Price Sources/Data Providers for this Model </strong>"))
    htmlOut.Append("</td></tr>")
    htmlOut.Append("<tr><td align=""left"" valign=""top"" width=""50%"">")
    htmlOut.Append(htmlSaleVsAskLineGraph)
    htmlOut.Append("</td><td align=""left"" valign=""top"">")
    htmlOut.Append(htmlSalePricesBarChart)
    htmlOut.Append("</td></tr></table>")

    sale_label.Text = htmlOut.ToString
    sale_label.Visible = True

    tempTable = Nothing
    htmlOut = Nothing

    htmlRetailSalePrices = Nothing
    htmlSalePricesByCompany = Nothing


  End Sub

  Public Function GetRetailSalePricesByYearOfSale(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT YEAR(journ_date) AS tyear, count(distinct journ_id) AS transcount, SUM(case when ac_sale_price > 0 then 1 else 0 end) AS SALEPRICES")
      sQuery.Append(" FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON journ_id = ac_journ_id and ac_id = journ_ac_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE amod_id = " + amod_id.ToString + " AND journ_subcat_code_part1 = 'WS'")
      sQuery.Append(" AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')")
      sQuery.Append(" GROUP BY YEAR(journ_date)")
      sQuery.Append(" ORDER BY YEAR(journ_date) desc")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetRetailSalePricesByYearOfSale(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function GetSalePricesReceivedByCompany(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT comp_name, comp_id, count(distinct journ_id) as tcount")
      sQuery.Append(" FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON acval_journ_id = ac_journ_id AND ac_id = acval_ac_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON journ_id = acval_journ_id AND journ_ac_id = acval_ac_id")
      sQuery.Append(" LEFT OUTER JOIN Subscription WITH(NOLOCK) ON acval_sub_id = sub_id")
      sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON acval_comp_id = comp_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE acval_sale_price > 0 AND ac_sale_price > 0")
      sQuery.Append(" AND ac_sale_price_display_flag = 'Y' AND amod_id = " + amod_id.ToString)
      sQuery.Append(" GROUP BY comp_name, comp_id")
      sQuery.Append(" ORDER BY COUNT(distinct journ_id) desc, comp_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetSalePricesReceivedByCompany(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Sub sales_vs_ask_line_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable
    Dim column As New DataColumn
    Dim column2 As New DataColumn
    Dim column3 As New DataColumn
    Dim column4 As New DataColumn
    Dim column5 As New DataColumn

    column.DataType = System.Type.GetType("System.Double")
    column.DefaultValue = 0
    column.Unique = False
    column.ColumnName = "asking_price"
    results_table.Columns.Add(column)

    column2.DataType = System.Type.GetType("System.Double")
    column2.DefaultValue = 0
    column2.Unique = False
    column2.ColumnName = "take_price"
    results_table.Columns.Add(column2)

    column3.DataType = System.Type.GetType("System.Double")
    column3.DefaultValue = 0
    column3.AllowDBNull = True
    column3.Unique = False
    column3.ColumnName = "sold_price"
    results_table.Columns.Add(column3)

    column4.DataType = System.Type.GetType("System.DateTime")
    column4.AllowDBNull = True
    column4.Unique = False
    column4.ColumnName = "date_of"
    results_table.Columns.Add(column4)

    column5.DataType = System.Type.GetType("System.String")
    column5.AllowDBNull = True
    column5.Unique = False
    column5.ColumnName = "ac_details"
    results_table.Columns.Add(column5)

    Dim viewsDataLayerObj As New viewsDataLayer

    Dim searchCriteria As New viewSelectionCriteriaClass

    Dim tString1 As String = ""
    Dim tString2 As String = ""
    Dim tString3 As String = ""
    Dim tString4 As String = ""
    Dim tString5 As String = ""
    Dim tString6 As String = "Y"
    Dim tString7 As String = "Y"
    Dim tString8 As String = "Y"

    Dim tGraph As String = ""

    Try

      viewsDataLayerObj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      viewsDataLayerObj.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      viewsDataLayerObj.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      viewsDataLayerObj.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      viewsDataLayerObj.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      searchCriteria.ViewCriteriaAmodID = amod_id
      searchCriteria.ViewCriteriaTimeSpan = 12

      crmViewDataLayer.Combined_views_display_recent_retail_sales(searchCriteria, "", viewsDataLayerObj, _
                                                                  IIf(Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM, True, False), _
                                                                  True, results_table, "N", "N", False, 0, 0, "", _
                                                                  0, 0, "", True, "", "", "", "", "", "", "", "", "", _
                                                                  searchCriteria.ViewCriteriaTimeSpan, "", 0, 0, "", "", "", "", tString1, _
                                                                  tString2, tString3, tString4, tString5, tString6, tString7, False, True, "", "", True, tString8)
      tString1 = ""
      tString2 = ""
      tString3 = ""
      tString4 = ""
      tString5 = ""
      tString6 = ""
      tString7 = ""

      crmViewDataLayer.Combined_views_display_recent_retail_sales(searchCriteria, "", viewsDataLayerObj, _
                                                            IIf(Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM, True, False), _
                                                            True, results_table, "N", "N", False, 0, 0, "", _
                                                            0, 0, "", True, "", "", "", "", "", "", "", "", "", _
                                                            searchCriteria.ViewCriteriaTimeSpan, "", 0, 0, "", "", "", "", tString1, _
                                                            tString2, tString3, tString4, tString5, tString6, tString7, False, True, "", "", True, tString8)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)


          scriptOut.Append(tString1.Replace("data1", "data") + vbCrLf)

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'65%',height:'80%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: ''," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 9, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 9, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: ''," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 9, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 9, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  curveType:'function'," + vbCrLf)
          scriptOut.Append("  legend: { position: 'right', textStyle:{fontSize:'11'}}," + vbCrLf)
          scriptOut.Append("  series: { " + vbCrLf)
          scriptOut.Append("   0: { lineWidth: 2, pointSize: 3 }, " + vbCrLf)
          scriptOut.Append("   1: { lineWidth: 0, pointSize: 3, visibleInLegend: false }, " + vbCrLf)
          scriptOut.Append("   2: { lineWidth: 2, pointSize: 3 }, " + vbCrLf)
          scriptOut.Append("   3: { lineWidth: 0, pointSize: 3, visibleInLegend: false }, " + vbCrLf)
          scriptOut.Append("   4: { lineWidth: 0, pointSize: 3, visibleInLegend: false }, " + vbCrLf)
          scriptOut.Append("   5: { lineWidth: 0, pointSize: 3, visibleInLegend: false } " + vbCrLf)
          scriptOut.Append("}, " + vbCrLf)

          scriptOut.Append("  colors: ['blue', 'red', 'green','blue', 'red', 'green']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modelSalesVsAskLineChart"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>Avg Asking vs Selling Price ($k) - (All Asking/Sold Prices))<br/><em>(Based on Last Year of Sales)</em></strong>")
        htmlOut.Append("<div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modelSalesVsAskLineChart"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>Avg Asking vs Selling Price ($k) - (All Asking/Sold Prices))<br/><em>(Based on Last Year of Sales)</em></strong></td></tr>")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in sales_vs_ask_line_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub sales_bar_chart(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim avg_sale As Long = 0
    Dim avg_sale_string As String = ""

    Dim getSPIValues As New viewsDataLayer

    Dim searchCriteria As New viewSelectionCriteriaClass

    getSPIValues.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    getSPIValues.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    getSPIValues.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    getSPIValues.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    getSPIValues.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    Try

      searchCriteria.ViewCriteriaAmodID = amod_id
      searchCriteria.ViewCriteriaTimeSpan = 12

      results_table = getSPIValues.Create_Run_Price_History_SPI(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
          scriptOut.Append(" data.addColumn('string', 'AC Year');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'Avg Sale Price ($k)');" + vbCrLf)
          scriptOut.Append(" data.addRows([" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("AVGSALEPRICE")) Then
              If Not String.IsNullOrEmpty(r.Item("AVGSALEPRICE").ToString.Trim) Then
                If IsNumeric(r.Item("AVGSALEPRICE").ToString) Then

                  avg_sale = CLng(r.Item("AVGSALEPRICE").ToString)
                  avg_sale = (avg_sale / 1000)

                End If
              End If
            End If

            If String.IsNullOrEmpty(avg_sale_string.Trim) Then
              avg_sale_string = "['" + r.Item("ac_year").ToString.Trim + "', " + avg_sale.ToString + "]"
            Else
              avg_sale_string += ",['" + r.Item("ac_year").ToString.Trim + "', " + avg_sale.ToString + "]"
            End If

            avg_sale = 0

          Next

          scriptOut.Append(avg_sale_string + "]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'65%',height:'80%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: ''," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 9, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 9, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: ''," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 9, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 9, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:true," + vbCrLf)
          scriptOut.Append("  legend: { position: 'right', textStyle:{fontSize:'11'}}," + vbCrLf)
          scriptOut.Append("  bar: {groupWidth: '75%'}," + vbCrLf)
          scriptOut.Append("  colors: ['green','blue','red','blue','red','green']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)

          scriptOut.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modelSalesBarChart"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>Avg Selling Price By Year Mfr($k)<br/><em>(Based on Last Year of Sales)</em></strong>")
        htmlOut.Append("<div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modelSalesBarChart"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>Avg Selling Price By Year Mfr($k)<br/><em>(Based on Last Year of Sales)</em></strong></td></tr>")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in sales_bar_chart(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function model_internal_notes(ByVal amod_id As Long) As String

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim htmlOut As New StringBuilder

    Dim bHadValue As Boolean = False

    Try

      sQuery.Append("SELECT amod_internal_note FROM Aircraft_Model WITH(NOLOCK) WHERE (amod_id = " + amod_id.ToString + ")")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try


      If Not IsNothing(temptable) Then

        If temptable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""modelInternalNotesTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")

          For Each r As DataRow In temptable.Rows

            If Not IsDBNull(r("amod_internal_note")) Then

              If Not String.IsNullOrEmpty(r("amod_internal_note").ToString.Trim) Then

                bHadValue = True
                htmlOut.Append("<tr><td valign=""top"" align=""left"">" + r("amod_internal_note").ToString.Replace(vbCrLf, "<br/>").Trim + "</td></tr>")

              End If

            End If

          Next

          htmlOut.Append("</table>" + vbCrLf)

        End If

      End If

      If Not bHadValue Then
        htmlOut = New StringBuilder
      End If

    Catch ex As Exception

      Return ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_internal_notes(ByVal amod_id As Long) As string : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

    htmlOut = Nothing

  End Function

  Public Sub FillTopics()

    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut_ModelTopics As StringBuilder = New StringBuilder()

    Dim tempTable As New DataTable

    Dim nColumnCount As Integer = 1
    Dim toggleRowColor As Boolean = False

    tempTable = GetModelTopics(ModelID)

    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then

        htmlOut_ModelTopics.Append("<table id=""modelTopicsTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">ATTRIBUTES</th>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">QTY</th>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">ATTRIBUTES</th>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">QTY</th>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">ATTRIBUTES</th>")
        htmlOut_ModelTopics.Append("<th class=""header_row cell_border_top"">QTY</th>")
        htmlOut_ModelTopics.Append("</tr>")

        For Each r As DataRow In tempTable.Rows

          If nColumnCount = 1 Then
            If Not toggleRowColor Then
              htmlOut_ModelTopics.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut_ModelTopics.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If
          End If

          htmlOut_ModelTopics.Append("<td class=""text_align_left"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("topicname")) Then
            If Not String.IsNullOrEmpty(r.Item("topicname").ToString.Trim) Then
              htmlOut_ModelTopics.Append(r.Item("topicname").ToString.Trim)
            End If
          End If

          htmlOut_ModelTopics.Append("</td><td class=""text_align_right"" nowrap=""nowrap"">")

          If Not IsDBNull(r.Item("tcount")) Then
            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

              If CLng(r.Item("tcount").ToString) > 0 Then
                htmlOut_ModelTopics.Append(r.Item("tcount").ToString)
              Else
                htmlOut_ModelTopics.Append("0")
              End If

            End If
          End If

          htmlOut_ModelTopics.Append("</td>")

          If nColumnCount = 3 Then
            htmlOut_ModelTopics.Append("</tr>")
            nColumnCount = 0
          End If

          nColumnCount += 1

        Next

        ' back fill missing columns
        Select Case nColumnCount
          Case 2
            htmlOut_ModelTopics.Append("<td class=""text_align_left"" nowrap=""nowrap""></td><td class=""text_align_right"" nowrap=""nowrap""></td>")
            htmlOut_ModelTopics.Append("<td class=""text_align_left"" nowrap=""nowrap""></td><td class=""text_align_right"" nowrap=""nowrap""></td></tr>")

          Case 3
            htmlOut_ModelTopics.Append("<td class=""text_align_left"" nowrap=""nowrap""></td><td class=""text_align_right"" nowrap=""nowrap""></td></tr>")

        End Select

        htmlOut_ModelTopics.Append("</table>")

      End If

    End If


    htmlOut.Append("<table id=""modelTopicsTabOuterTable"" width=""100%"" cellpadding=""2"" cellspacing="""">")
    htmlOut.Append("<tr><td align=""left"" valign=""top"">")
    htmlOut.Append(htmlOut_ModelTopics.ToString)
    htmlOut.Append("</td></tr></table>")

    topics_label.Text = htmlOut.ToString
    topics.Visible = True

  End Sub

  Public Function GetModelTopics(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT actop_name AS topicname, COUNT(*) AS tcount")
      sQuery.Append(" FROM Aircraft_Topic WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Topic_Index WITH(NOLOCK) ON actopind_journ_id = 0 AND actopind_actop_id = actop_id")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = 0 AND ac_id = actopind_ac_id")
      sQuery.Append(" WHERE actopind_journ_id = 0 AND ac_amod_id = " + amod_id.ToString)
      sQuery.Append(" GROUP BY actop_name")
      sQuery.Append(" ORDER BY actop_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetModelTopics(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  'Public Sub FillAssettInsightGraphs()

  '  Dim htmlUtilizationGraph As String = ""
  '  Dim htmlUtilizationGraphScript As String = ""
  '  Dim htmlUtilizationFunctionScript As String = ""

  '  Dim htmlOut As New StringBuilder

  '  Dim graphID As Integer = 7

  '  Dim utilization_functions As New utilization_view_functions
  '  Dim searchCriteria As New viewSelectionCriteriaClass

  '  searchCriteria.ViewCriteriaAmodID = ModelID

  '  utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
  '  utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
  '  utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
  '  utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
  '  utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

  '  utilization_functions.views_display_assett_prices_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False)


  '  If Not String.IsNullOrEmpty(htmlUtilizationFunctionScript.Trim) Then

  '    htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
  '    htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
  '    htmlUtilizationGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
  '    htmlUtilizationGraphScript += "});" + vbCrLf
  '    htmlUtilizationGraphScript += htmlUtilizationFunctionScript.Trim
  '    htmlUtilizationGraphScript += "</script>" + vbCrLf

  '    System.Web.UI.ScriptManager.RegisterStartupScript(sale, sale.GetType(), "showUtilizationGraph" + graphID.ToString, htmlUtilizationGraphScript, False)

  '  End If

  '  assett_label.Text += htmlUtilizationGraph.ToString




  '  graphID = 777
  '  utilization_functions.views_display_asking_with_sold_over_assett_prices_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False)

  '  If Not String.IsNullOrEmpty(htmlUtilizationFunctionScript.Trim) Then

  '    htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
  '    htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
  '    htmlUtilizationGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
  '    htmlUtilizationGraphScript += "});" + vbCrLf
  '    htmlUtilizationGraphScript += htmlUtilizationFunctionScript.Trim
  '    htmlUtilizationGraphScript += "</script>" + vbCrLf

  '    System.Web.UI.ScriptManager.RegisterStartupScript(sale, sale.GetType(), "showUtilizationGraph" + graphID.ToString, htmlUtilizationGraphScript, False)

  '  End If

  '  assett_label2.Text += htmlUtilizationGraph.ToString



  '  ' assett_label.Text = htmlOut.ToString


  '  ' utilization.Visible = True

  'End Sub
  Public Sub FillUtilizationGraph()

    Dim htmlUtilizationGraph As String = ""
    Dim htmlUtilizationGraphScript As String = ""
    Dim htmlUtilizationFunctionScript As String = ""

    Dim htmlOut As New StringBuilder

    Dim graphID As Integer = 4

    Dim utilization_functions As New utilization_view_functions
    Dim searchCriteria As New viewSelectionCriteriaClass

    searchCriteria.ViewCriteriaAmodID = ModelID

    utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        utilization_functions.views_display_flight_utilization_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False)

        htmlUtilizationFunctionScript = Replace(htmlUtilizationFunctionScript, "chartArea:{width:'80%'", "chartArea:{width:'70%'")

        If Not String.IsNullOrEmpty(htmlUtilizationFunctionScript.Trim) Then

      htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlUtilizationGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlUtilizationGraphScript += "});" + vbCrLf
      htmlUtilizationGraphScript += htmlUtilizationFunctionScript.Trim
      htmlUtilizationGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(utilization, utilization.GetType(), "showUtilizationGraph" + graphID.ToString, htmlUtilizationGraphScript, False)

    End If

    htmlOut.Append("<table id=""extraOperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
    htmlOut.Append("<tr><td valign=""top"" align=""left"">" + commonEvo.model_utilization_percentage(ModelID).Trim + "</td></tr>")
    htmlOut.Append("</table><br/>" + vbCrLf)

    utilization_label.Text = htmlOut.ToString

    utilization_label.Text += htmlUtilizationGraph.ToString

    utilization.Visible = True

  End Sub

  Public Sub FillOperationalTrends()

    Dim htmlOperationalTrendsGraph As String = ""
    Dim htmlOperationalTrendsGraphScript As String = ""
    Dim htmlOperationalTrendsFunctionScript As String = ""

    Dim htmlOut As New StringBuilder

    Dim graphID As Integer = 5

    model_operational_trends_graph(ModelID, htmlOperationalTrendsFunctionScript, htmlOperationalTrendsGraph, graphID)

    If Not String.IsNullOrEmpty(htmlOperationalTrendsFunctionScript.Trim) Then

      htmlOperationalTrendsGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlOperationalTrendsGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlOperationalTrendsGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlOperationalTrendsGraphScript += "});" + vbCrLf
      htmlOperationalTrendsGraphScript += htmlOperationalTrendsFunctionScript.Trim
      htmlOperationalTrendsGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(operational, operational.GetType(), "showOperationalTrendsGraph" + graphID.ToString, htmlOperationalTrendsGraphScript, False)

    End If

    htmlOut.Append("<table id=""extraModelUtilizationTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
    htmlOut.Append("<tr><td valign=""top"" align=""left"">" + model_aircraft_awaiting_docs(ModelID).Trim + "</td></tr>")
    htmlOut.Append("<tr><td valign=""top"" align=""left"">" + model_aircraft_with_no_base(ModelID).Trim + "</td></tr>")
    htmlOut.Append("</table><br/>" + vbCrLf)

    operational_label.Text = htmlOut.ToString

    operational_label.Text += htmlOperationalTrendsGraph.ToString

    operational.Visible = True

  End Sub

  Public Sub model_operational_trends_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Try

      results_table = model_operational_trends(amod_id)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('string', 'Year');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'In Operation');" + vbCrLf)

          scriptOut.Append(" data.addRows([" + vbCrLf)


          For Each r As DataRow In results_table.Rows

            For x As Integer = 1 To 5

              scriptOut.Append(IIf(x > 1, ",['", "['"))

              If Not IsDBNull(r.Item("YEAR" + x.ToString + "YEAR")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "YEAR").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "YEAR").ToString) > 0 Then
                    scriptOut.Append(r.Item("YEAR" + x.ToString + "YEAR").ToString + "'")
                  Else
                    scriptOut.Append("0'")
                  End If

                Else
                  scriptOut.Append("0'")
                End If

              Else
                scriptOut.Append("0'")
              End If

              If Not IsDBNull(r.Item("YEAR" + x.ToString + "VAL")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "VAL").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "VAL").ToString) > 0 Then
                    scriptOut.Append("," + r.Item("YEAR" + x.ToString + "VAL").ToString + "]")
                  Else
                    scriptOut.Append(",0]")
                  End If

                Else
                  scriptOut.Append(",0]")
                End If

              Else
                scriptOut.Append(",0]")
              End If

            Next

          Next

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
                    scriptOut.Append("  chartArea:{width:'74%',height:'75%'}," + vbCrLf)
                    scriptOut.Append("  hAxis: { title: 'Year'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'In Operation'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:true," + vbCrLf)
          scriptOut.Append("  legend:'none'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No In Operation Aircraft Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_operational_trends_graph(ByVal amod_id As Long, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function model_operational_trends(ByVal amod_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id,")
      sQuery.Append(" YEAR(getdate())-4 as YEAR1YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-4 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR1VAL,")
      sQuery.Append(" YEAR(getdate())-3 as YEAR2YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-3 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR2VAL,")
      sQuery.Append(" YEAR(getdate())-2 as YEAR3YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-2 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR3VAL,")
      sQuery.Append(" YEAR(getdate())-1 as YEAR4YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-1 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR4VAL,")
      sQuery.Append(" YEAR(getdate()) as YEAR5YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate()) AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR5VAL")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
      sQuery.Append(" WHERE amod_id = " + amod_id.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_user_interest(ByVal amod_id As Long) As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function



  Public Function model_aircraft_awaiting_docs(ByVal amod_id As Long) As String

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim nAc_count As Long = 0

    Dim htmlOut As New StringBuilder

    Try

      sQuery.Append(" SELECT COUNT(distinct ac_id) AS OWNERAWAITING FROM View_Aircraft_Company_Flat WITH(NOLOCK)")
      sQuery.Append(" WHERE ac_journ_id = 0 AND comp_name LIKE 'Awaiting Documentation' AND amod_id = " + amod_id.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If temptable.Rows.Count > 0 Then

        For Each r As DataRow In temptable.Rows

          If CLng(r.Item("OWNERAWAITING").ToString) > 0 Then

            nAc_count = CLng(r.Item("OWNERAWAITING").ToString)

          End If

        Next

      End If  '  XX% of in operation aircraft.

      htmlOut.Append("[<strong>" + nAc_count.ToString + "</strong>] Aircraft with Awaiting Documentation Owners.")

    Catch ex As Exception

      Return ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_aircraft_awaiting_docs(ByVal amod_id As Long) As Long : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

    htmlOut = Nothing

  End Function

  Public Function model_aircraft_with_no_base(ByVal amod_id As Long) As String

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Dim nAc_count As Long = 0

    Dim htmlOut As New StringBuilder

    Try

      sQuery.Append(" SELECT COUNT(distinct ac_id) AS NOAIRPORT FROM Aircraft_Flat WITH(NOLOCK)")
      sQuery.Append(" WHERE ac_journ_id = 0 AND ac_aport_id = 0 AND amod_id = " + amod_id.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If temptable.Rows.Count > 0 Then

        For Each r As DataRow In temptable.Rows

          If CLng(r.Item("NOAIRPORT").ToString) > 0 Then

            nAc_count = CLng(r.Item("NOAIRPORT").ToString)

          End If

        Next

      End If  '  XX% of in operation aircraft.

      htmlOut.Append("[<strong>" + nAc_count.ToString + "</strong>] Aircraft with Unknown Bases.")

    Catch ex As Exception

      Return ""
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in model_aircraft_with_no_base(ByVal amod_id As Long) As Long : " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

    htmlOut = Nothing

  End Function

End Class