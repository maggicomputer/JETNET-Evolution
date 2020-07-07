Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class _listing
  Inherits System.Web.UI.Page
  Dim table, atemptable2, aTempTable As DataTable
  Dim error_string As String = ""
#Region "Page Events"


  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Try
      AddHandler Master.BringResults, AddressOf Fill_Listing
      AddHandler Master.Swap_Columns, AddressOf Swap_Columns
      AddHandler Master.ClearResults, AddressOf Clear_Listing
      AddHandler Master.NextButton_Listing, AddressOf Next_Listing
      AddHandler Master.PreviousButton_Listing, AddressOf Previous_Listing
      AddHandler Master.SetPagerButtons, AddressOf SetPagerButtons

      AddHandler Master.resultsVisible, AddressOf resultsVisible
      AddHandler Master.resultsInvisible, AddressOf resultsInvisible
      'Session.Item("Subnode") = ""
      'Session.Remove("Subnode")

      Master.ListingID = 0
      Master.Listing_ContactID = 0
      Master.ListingSource = ""
      Master.Listing_IsJob = False
      Session("export_info") = ""
      If Not Page.IsPostBack Then
        If Session.Item("FromTypeOfListing") <> Master.TypeOfListing Then
          If Session.Item("FromTypeOfListing") <> 0 Then
            Master.TypeOfListing = Session.Item("FromTypeOfListing")
          End If
        End If
      End If


    Catch ex As Exception
      error_string = "listing.aspx.vb - page init() - " & ex.Message
      ' 
      Master.LogError(error_string)
    End Try
  End Sub
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'If Not Page.IsPostBack Then
    Try
      If Not Page.IsPostBack Then
        Master.ListingID = 0
        Master.Listing_ContactID = 0
        Master.ListingSource = ""
        Master.OtherID = 0
        Master.Table_List = Nothing
      End If


      Master.aclsData_Temp.class_error = ""

      Dim cookieExists As Boolean = False 'whether or not a cookie is here
      Dim _acmarked As HttpCookie = Request.Cookies("testing")
      Select Case Master.TypeOfListing
        Case 1
          _acmarked = Request.Cookies("companies_marked")
        Case 2
          _acmarked = Request.Cookies("contacts_marked")
        Case 3
          _acmarked = Request.Cookies("aircraft_marked")
      End Select

      If _acmarked IsNot Nothing Then
        Dim _acmarked_val As String = ""
        Select Case Master.TypeOfListing
          Case 1
            _acmarked_val = Request.Cookies("companies_marked").Value
          Case 2
            _acmarked_val = Request.Cookies("contacts_marked").Value
          Case 3
            _acmarked_val = Request.Cookies("aircraft_marked").Value
        End Select

        If _acmarked_val <> "" Then
          cookieExists = True
        Else
          cookieExists = False
        End If
      Else
        cookieExists = False
      End If


      If Not IsNothing(Session("Results")) Then
        Master.Table_List = Session("Results")
        Session("FromDetails") = False
      End If
      If Not Page.IsPostBack Then '3/7/2012 - found a bug that changes calendar view to day view if you're searching on action items with a weekly view and click next. Will switch to day view 
        If Not (String.IsNullOrEmpty(Session.Item("DayPilotCalendar1_startDate"))) Then
          Master.DateOfActionItem = Session.Item("DayPilotCalendar1_startDate")
          'Session.Item("DayPilotCalendar1_startDate") = ""
          'this is to catch for the calendar action items.
          Master.Fill_DayPilotCalendar1("Day")
          Results.Visible = False
          'Session.Item("DayPilotCalendar1_startDate") = ""
        End If
      End If

      If Not (String.IsNullOrEmpty(Session.Item("isSubnode"))) Then
        Master.IsSubNode = Session.Item("isSubnode")
      Else
        Master.IsSubNode = False
      End If
      If (String.IsNullOrEmpty(Session.Item("Listing"))) Then
        'This sets the subnodes property. If you click on a subfolder, you need to save the ID of the subfolder as well as the parent folder.
        If (String.IsNullOrEmpty(Session.Item("Subnode"))) Then
          Master.SubNodeOfListing = 1
        Else
          Master.SubNodeOfListing = Session.Item("Subnode")
          Master.NameOfSubnode = Session.Item("SubnodeName")
          Master.Subnode_Method = Session.Item("SubnodeMethod")
        End If
        Master.TypeOfListing = 1
        ' Master.fill_CBO()
      Else
        'This sets the subnodes property. If you click on a subfolder, you need to save the ID of the subfolder as well as the parent folder.
        If (String.IsNullOrEmpty(Session.Item("Subnode"))) Then
          Master.SubNodeOfListing = Session.Item("Listing")
        Else
          Master.SubNodeOfListing = Session.Item("Subnode")
          Master.NameOfSubnode = Session.Item("SubnodeName")
          Master.Subnode_Method = Session.Item("SubnodeMethod")
        End If
        Master.TypeOfListing = Session.Item("Listing")
        ' Master.fill_CBO()
      End If

      If Not Page.IsPostBack Then
        If Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
          If Master.TypeOfListing = 4 Then 'Action Items Default View
            If Trim(Request("redo_search")) <> "true" Then
              If Not (String.IsNullOrEmpty(Session.Item("DayPilotCalendar1_startDate"))) Then
              Else
                'Master.Search = 1
                Master.Fill_Action("", "", 2, "B", Session.Item("localUser").crmLocalUserID, "Date Scheduled", "", "")
                'Master.Search = 2
                Dim btnDayPilotCalendar_Previous As Button = Master.FindControl("btnDayPilotCalendar_Previous")
                Dim btnDayPilotCalendar_Next As Button = Master.FindControl("btnDayPilotCalendar_Next")
                btnDayPilotCalendar_Previous.Visible = False
                btnDayPilotCalendar_Next.Visible = False
                'Dim dayPilot As DayPilot.Web.Ui.DayPilotCalendar = Master.FindControl("DayPilotCalendar1")
                'dayPilot.Visible = False
              End If
            End If
          ElseIf Master.TypeOfListing = 11 Then 'Opportunities Default View
            If Trim(Request("redo_search")) <> "true" Then
              Session("search_opportunity") = "@2@7@0@" & Session.Item("localUser").crmLocalUserID.ToString & "@0@@"
              Master.Fill_Opportunities("", 2, Session.Item("localUser").crmLocalUserID, 0, "", "", "O")
            End If
          ElseIf Master.TypeOfListing = 5 Then 'Job Default View
            If Master.SubNodeOfListing = 5 Then
              Master.Fill_Jobs(4)
            Else
              Master.Fill_Jobs(Master.SubNodeOfListing)
            End If
          ElseIf Master.TypeOfListing = 16 Then
            If Trim(Request("redo_search")) <> "true" Then
              Master.Fill_Notes("", "2", "A", IIf(HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly, 999, 0), "amod_make_name, amod_model_name, ac_ser_nbr, comp_name", "", "", "", "", 0, 0, "", 0, False, False, 3)
            End If

          ElseIf Master.TypeOfListing = 10 Then 'Market default
            ''''''''''''''' 
            Dim models As String = ""
            If Not IsDBNull(HttpContext.Current.Session.Item("localUser").crmUserDefaultModels) Then
              models = HttpContext.Current.Session.Item("localUser").crmUserDefaultModels
            End If

            aTempTable = Master.aclsData_Temp.Get_Client_Preferences()
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                Dim start_date As Integer = 0

                If Not IsDBNull(aTempTable.Rows(0).Item("clipref_activity_default_days")) Then
                  start_date = CInt(aTempTable.Rows(0).Item("clipref_activity_default_days"))
                End If

                Dim start As DropDownList = Master.FindControl("market_search").FindControl("market_time")
                Dim model_cbo As ListBox = Master.FindControl("market_search").FindControl("model_cbo")

                Dim model_list As ListBox = model_cbo
                Try
                  clsGeneral.clsGeneral.populate_models(model_cbo, True, Master.FindControl("market_search"), Nothing, Master, True)
                Catch ex As Exception
                  error_string = "listing.aspx.vb - market Default() Error in Aircraft Dropdown Filling - " & ex.Message
                  Master.LogError(error_string)
                End Try

                start.SelectedValue = start_date
                Dim noth As New ListBox
                If start_date <> 0 And models <> "" Then
                  Master.Fill_Market(model_cbo, start.SelectedValue, noth, noth, "", "")
                End If
              End If
            End If

          End If
        End If
        Session.Item("Table_List") = ""
      End If
    Catch ex As Exception
      error_string = "listing.aspx.vb - Page Load() - " & ex.Message
      Master.LogError(error_string)
    End Try
    'End If
  End Sub
#End Region
#Region "Deals with Results Listings - Next Previous, Pager Buttons"
  Private Sub resultsInvisible()
    Results.Visible = False
  End Sub
  Private Sub resultsVisible()
    Results.Visible = True
  End Sub
  Private Sub Search_Buttons()
    'Response.Write("search buttons clicked")
  End Sub
  Private Sub ToggleAircraftListingDataBinding()
    If Master.TypeOfListing = 3 Then
      If Session.Item("AircraftSort_Company") = True Then
        AddHandler Results.ItemDataBound, AddressOf Transaction_Bind
      Else
        AddHandler Results.ItemDataBound, AddressOf Aircraft_Item_Databound
      End If
    End If
  End Sub
  Private Sub Fill_Listing()

    Try
      Master.FillTreeView()
      table = Master.Table_List
      If Not IsNothing(table) Then
        Session("Results") = Master.Table_List

        If table.Rows.Count >= 10 Then
          Master.SetRecordCount = "1-" & Session.Item("localUser").crmUserRecsPerPage & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"

        Else
          Master.SetRecordCount = table.Rows.Count & " " & Master.NameOfListingType & " Records"

        End If

        If table.Rows.Count > 0 Then
          If Master.TypeOfListing = 8 Then
            Dim DisplayTransactionEdit As New Label
            If Not IsNothing(Master.FindControl("label_edit_display")) Then
              DisplayTransactionEdit = Master.FindControl("label_edit_display")
              DisplayTransactionEdit.Visible = True
            End If
          End If
        End If
        Results.DataSource = table

        Master.keep = 0
        Master.change = False

        ''Setting the visibility of the buttons
        SetPagerButtons()
        'This right here is for the export to excel stuff.
        Dim stringwrite As System.IO.StringWriter = New System.IO.StringWriter
        Dim htmlwrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringwrite)

        If IsNumeric(Session.Item("localUser").crmUserRecsPerPage) Then
          Results.PageSize = Session.Item("localUser").crmUserRecsPerPage
        End If

        Results.AllowPaging = True
        Master.keep = 0
        Master.change = False
        Select Case Master.TypeOfListing
          Case 3

            If Session.Item("localSubscription").crmAerodexFlag = True Then
              Results.Columns(14).Visible = False 'AC Status
              Results.Columns(15).Visible = False 'AC Flags
              Results.Columns(13).Visible = False 'Take
              Results.Columns(12).Visible = False 'Asking
              Results.Columns(11).Visible = False
            End If
            ToggleAircraftListingDataBinding()
          Case 16
            Results.Columns(1).Visible = False
            Results.Columns(2).Visible = False
            Results.Columns(3).Visible = False
            Results.Columns(4).Visible = False
            Results.Columns(5).Visible = False
            Results.Columns(6).Visible = True
            Results.Columns(7).Visible = True
            Results.Columns(8).Visible = False
            Results.Columns(9).Visible = True
        End Select

        Try

          'Response.Write("<br />" & Session("AtPage") & " listing page!!")
          'Dim test As Integer = IIf(Not IsNumeric(Session("AtPage")), Session("AtPage"), 0)
          If IsNumeric(Session("AtPage")) And Trim(Request("redo_search")) = "true" Then

            If CLng(Session("AtPage")) <> 0 Then

              Results.CurrentPageIndex = CLng(Session("AtPage"))
              'Results.CurrentPageIndex = 0 'added 4/3/12
              Results.DataBind()

              SetPagerButtons()
              Session("AtPage") = 0
              Dim currentrecord, realcount As Integer
              currentrecord = (Results.PageSize * Results.CurrentPageIndex) - table.Rows.Count + table.Rows.Count
              'reset record #'s if the page is saved
              If currentrecord = 0 Then
                realcount = 1
              Else
                realcount = currentrecord + 1
              End If

              If currentrecord + Results.PageSize >= table.Rows.Count Then
                Master.SetRecordCount = realcount & "-" & table.Rows.Count & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
              Else
                Master.SetRecordCount = realcount & "-" & currentrecord + Results.PageSize & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
              End If
            Else
              Results.CurrentPageIndex = 0 'added 4/3/12
              Results.DataBind()

            End If

          Else
            'SetPagerButtons()
            Results.CurrentPageIndex = 0 'added 4/3/12
            Results.DataBind()


            Dim lab As Label = Master.FindControl("opportunity_summary")
            If Master.TypeOfListing = 11 Then

              If Not IsNothing(lab) Then
                lab.Text = ""
              End If
              Dim subtotal_percent As Integer = 0
              Dim old_cat As Integer = 0
              Dim subtotal As Integer = 0
              Dim str As String = ""
              Dim looped As Integer = 0
              Dim full_total As Integer = 0
              Dim full_percent As Integer = 0
              Dim full_count As Integer = 0
              str = "<tr><td align=""left"" valign=""top""><u>Category Name</u></td><td align=""left"" valign=""top""><u>Qty</u></td>"
              str = str & "<td align=""left"" valign=""top""><u>Value</u></td><td align=""left"" valign=""top""><u>Rated Value</u></td></tr>"
              Dim c As Integer = 0
              Dim distinctTable As DataTable = table.DefaultView.ToTable(True, "lnote_notecat_key")
              Dim dalTable As New DataTable

              For Each r As DataRow In distinctTable.Rows
                dalTable = table.Clone
                Dim afileterd As DataRow() = table.Select("lnote_notecat_key = '" & r("lnote_notecat_key") & "'", "lnote_schedule_start_date desc")
                ' a single data row for importing to the dalTable
                Dim atmpDataRow As DataRow
                ' extract and import
                For Each atmpDataRow In afileterd
                  dalTable.ImportRow(atmpDataRow)
                Next

                subtotal = 0
                subtotal_percent = 0

                c = 0


                For Each q As DataRow In dalTable.Rows

                  '    '    If (old_cat = r("lnote_notecat_key") Or looped = 0) And looped <> table.Rows.Count - 1 Then
                  old_cat = q("lnote_notecat_key")
                  subtotal_percent = subtotal_percent + (q("lnote_cash_value") * (q("lnote_capture_percentage") / 100))
                  subtotal = subtotal + q("lnote_cash_value")
                  c = c + 1

                Next
                full_count = full_count + c
                full_percent = full_percent + subtotal_percent
                full_total = full_total + subtotal
                str = str & "<tr><td align=""left"" valign=""top"">" & Master.what_opportunity_cat(old_cat, False) & "</td><td align=""left"" valign=""top"">" & c & "</td><td align=""left"" valign=""top"">$" & FormatNumber(subtotal, 0) & "</td><td align=""left"" valign=""top"">$" & FormatNumber(subtotal_percent, 0) & "</td></tr>"

              Next
              str = str & "<tr><td align=""left"" valign=""top"" colspan='4'><hr /></td></tr>"
              str = str & "<tr><td align=""left"" valign=""top"">Totals: </td><td align=""left"" valign=""top"">" & full_count & "</td><td align=""left"" valign=""top"">$" & FormatNumber(full_total, 0) & "</td><td align=""left"" valign=""top"">$" & FormatNumber(full_percent, 0) & "</td></tr>"
              'Dim lab As Label = Master.FindControl("opportunity_summary")
              If Not IsNothing(lab) Then
                lab.Text = "<div><Table width='80%' cellpadding='0' cellspacing='0' align='right' style='border:1px solid #20578e;background-color:#dcedff;padding:5px;'><tr><td align='left' valign='top' colspan='4'><h3 style='margin:0px;padding:4px; float:right;color:#0e61b3;font-size:13px;text-decoration:none;border-bottom:1px solid #76bbff;'>Summary of Opportunities</h3></td></tr>" & str & "<tr><td align='right' valign='top'></td></tr></table></div>" '& FormatNumber(sum, 2) & "</td></tr></table>"
              End If
            ElseIf Master.TypeOfListing <> 11 Then
              If Not IsNothing(lab) Then
                lab.Visible = False
              End If
            End If
            'test = Results.PageCount
          End If
        Catch
          Results.CurrentPageIndex = 0
          Results.DataBind()
        End Try


        If Master.TypeOfListing = 7 Or Master.TypeOfListing = 5 Then 'Or Master.TypeOfListing = 12 Then ' Or Master.TypeOfListing = 6 Then
          Results.RenderControl(htmlwrite)
          Session("export_info") = htmlwrite
          Session("export_info") = Replace(stringwrite.ToString(), "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding' title='JETNET RECORD' />", "JETNET")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/client.png' alt='CLIENT RECORD' title='CLIENT RECORD' class='ico_padding'/>", "CLIENT")
        Else
          Session("export_info") = "."
        End If
        'Next and Previous Button Array
        atemptable2 = table
        Dim arrayed() As String
        ReDim arrayed(0)
        arrayed(0) = ""

        Select Case Master.TypeOfListing
          Case 1
            For count As Integer = 0 To atemptable2.Rows.Count - 1
              ReDim Preserve arrayed(count)
              arrayed(count) = atemptable2.Rows(count).Item(0) & "|" & atemptable2.Rows(count).Item("source")
            Next
            Session("my_ids") = arrayed
          Case 2
            For count As Integer = 0 To atemptable2.Rows.Count - 1
              ReDim Preserve arrayed(count)
              arrayed(count) = atemptable2.Rows(count).Item(0) & "|" & UCase(atemptable2.Rows(count).Item("contact_type")) & "|comp:" & UCase(atemptable2.Rows(count).Item("contact_comp_id"))
            Next
            Session("my_ids") = arrayed
          Case 3
            Dim jump_to As DropDownList = Master.FindControl("jump_to")
            jump_to.Visible = True
            jump_to.Attributes.Add("onchange", "On_Change('" + jump_to.ClientID + "');")
            Dim seperate_counter As Integer = 0 ' seperate counting. Since the ac data is combined now.
            'they'll be more in the array than in the datatable.
            For count As Integer = 0 To atemptable2.Rows.Count - 1

              If Not (IsDBNull(atemptable2.Rows(count).Item("other_ac_id"))) And Not (IsDBNull(atemptable2.Rows(count).Item("other_source"))) Then
                ReDim Preserve arrayed(seperate_counter)
                arrayed(seperate_counter) = atemptable2.Rows(count).Item("other_ac_id") & "|" & atemptable2.Rows(count).Item("other_source")
                seperate_counter = seperate_counter + 1
              End If

              ReDim Preserve arrayed(seperate_counter)
              If Not (IsDBNull(atemptable2.Rows(count).Item("ac_id"))) And Not (IsDBNull(atemptable2.Rows(count).Item("source"))) Then
                arrayed(seperate_counter) = atemptable2.Rows(count).Item("ac_id") & "|" & atemptable2.Rows(count).Item("source")
                seperate_counter = seperate_counter + 1
              End If

              jump_to.Items.Add(New ListItem(atemptable2.Rows(count).Item("amod_make_name") & " " & atemptable2.Rows(count).Item("amod_model_name") & " " & IIf(Not IsDBNull(atemptable2.Rows(count).Item("ac_ser_nbr")), "Ser # " & clsGeneral.clsGeneral.RemoveHTML(atemptable2.Rows(count).Item("ac_ser_nbr")), IIf(Not IsDBNull(atemptable2.Rows(count).Item("other_ac_ser_nbr")), "Ser # " & clsGeneral.clsGeneral.RemoveHTML(atemptable2.Rows(count).Item("other_ac_ser_nbr")), "")), "details.aspx?source=" & atemptable2.Rows(count).Item("source") & "&ac_ID=" & atemptable2.Rows(count).Item("ac_id") & "&type=3&order=" & count.ToString & ""))
            Next
            Session("my_ids") = arrayed
        End Select

        Master.fill_bar()

        table = Nothing
        atemptable2 = Nothing
      End If
    Catch ex As Exception
      Dim ref As String = ""
      If Not Request.UrlReferrer Is Nothing Then
        ref = Request.ServerVariables("HTTP_REFERER").ToString()
      End If
      Dim pa As String = Request.ServerVariables("SCRIPT_NAME").ToString()
      error_string = "listing.aspx.vb - Fill_Listing() - Referrer - " & ref & " Current - " & pa & " - " & ex.Message
      Master.aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try

  End Sub
  Private Sub Clear_Listing()
    Try
      table = New DataTable
      table.Clear()
      Results.DataSource = table
      Results.DataBind()
      Results.Visible = False
      Session("export_info") = ""
    Catch ex As Exception
      error_string = "listing.aspx.vb - Clear_Listing() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
  Private Sub Next_Listing()
    Try
      Master.PerformDatabaseAction = True
      If (Results.CurrentPageIndex < (Results.PageCount - 1)) Then
        Results.DataSource = Session("Results")
        Results.CurrentPageIndex = Results.CurrentPageIndex + 1
        Master.Previous_Button_Visible = True
        Master.Next_Button_Visible = True
        If Results.CurrentPageIndex = Results.PageCount - 1 Then
          Master.Next_Button_Visible = False
        End If
        table = Session("Results")
        Dim currentrecord, realcount As Integer
        currentrecord = (Results.PageSize * Results.CurrentPageIndex) - table.Rows.Count + table.Rows.Count
        If currentrecord = 0 Then
          realcount = 1
        Else
          realcount = currentrecord + 1
        End If

        If currentrecord + Results.PageSize >= table.Rows.Count Then
          Master.SetRecordCount = realcount & "-" & table.Rows.Count & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
        Else
          Master.SetRecordCount = realcount & "-" & currentrecord + Results.PageSize & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
        End If



      End If
      ToggleAircraftListingDataBinding()

      Results.DataBind()

      Session("AtPage") = Results.CurrentPageIndex
      Results.Visible = True
      Master.PerformDatabaseAction = False
    Catch ex As Exception
      error_string = "listing.aspx.vb - Next_Listing() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
  Private Sub Previous_Listing()
    Try
      Master.PerformDatabaseAction = True
      If (Results.CurrentPageIndex >= 1) Then
        Results.DataSource = Session("Results")
        Results.CurrentPageIndex = Results.CurrentPageIndex - 1
        Master.Next_Button_Visible = True
        Master.Previous_Button_Visible = True
        If (Results.CurrentPageIndex = 0) Then
          Master.Previous_Button_Visible = False
        End If
        table = Session("Results")
        Dim currentrecord, realcount As Integer
        currentrecord = (Results.PageSize * Results.CurrentPageIndex) - table.Rows.Count + table.Rows.Count
        If currentrecord = 0 Then
          realcount = 1
        Else
          realcount = currentrecord + 1
        End If

        If currentrecord + Results.PageSize >= table.Rows.Count Then
          Master.SetRecordCount = realcount & "-" & table.Rows.Count & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
        Else
          Master.SetRecordCount = realcount & "-" & currentrecord + Results.PageSize & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
        End If
      End If
      ToggleAircraftListingDataBinding()
      Results.DataBind()

      Session("AtPage") = Results.CurrentPageIndex
      Results.Visible = True
      Master.PerformDatabaseAction = False
    Catch ex As Exception
      error_string = "listing.aspx.vb - Previous_Listing() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
  Private Sub SetPagerButtons()
    Try
      If Results.CurrentPageIndex = (0) Then
        Master.Previous_Button_Visible = False
      Else
        Master.Previous_Button_Visible = True
      End If
      If Results.CurrentPageIndex = (Results.PageCount - 1) Then
        If Master.Table_List.Rows.Count > 10 Then
          Master.Next_Button_Visible = True
        Else
          Master.Next_Button_Visible = False
        End If
      Else
        If Master.Table_List.Rows.Count > 10 Then
          Master.Next_Button_Visible = True
        Else
          Master.Next_Button_Visible = False
        End If
      End If
    Catch ex As Exception
      error_string = "listing.aspx.vb - SetPagerButtons() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
  Private Sub Swap_Columns()
    Dim view_cat As Control = Master.FindControl("CompanySearch")
    Dim viewc As CheckBox = view_cat.FindControl("special_field_view")
    Dim column_name As DropDownList = view_cat.FindControl("special_field_cbo")
    'Response.Write(column_name.Text)

    Select Case column_name.Text
      Case "clicomp_category1"

        Results.Columns(10).HeaderText = column_name.SelectedItem.Text
        Results.Columns(10).Visible = True
        Results.Columns(9).Visible = True
        Results.Columns(8).Visible = False
        Results.Columns(7).Visible = False
        Results.Columns(6).Visible = False

        'make sure other categories go invisible 
        Results.Columns(11).Visible = False
        Results.Columns(12).Visible = False
        Results.Columns(13).Visible = False
        Results.Columns(14).Visible = False
      Case "clicomp_category2"
        Results.Columns(11).HeaderText = column_name.SelectedItem.Text
        Results.Columns(11).Visible = True
        Results.Columns(9).Visible = True
        Results.Columns(8).Visible = False
        Results.Columns(7).Visible = False
        Results.Columns(6).Visible = False

        'make sure other categories go invisible 
        Results.Columns(10).Visible = False
        Results.Columns(12).Visible = False
        Results.Columns(13).Visible = False
        Results.Columns(14).Visible = False
      Case "clicomp_category3"
        Results.Columns(12).HeaderText = column_name.SelectedItem.Text
        Results.Columns(12).Visible = True
        Results.Columns(9).Visible = True
        Results.Columns(8).Visible = False
        Results.Columns(7).Visible = False
        Results.Columns(6).Visible = False

        'make sure other categories go invisible 
        Results.Columns(10).Visible = False
        Results.Columns(11).Visible = False
        Results.Columns(13).Visible = False
        Results.Columns(14).Visible = False
      Case "clicomp_category4"
        Results.Columns(13).HeaderText = column_name.SelectedItem.Text
        Results.Columns(13).Visible = True
        Results.Columns(9).Visible = True
        Results.Columns(8).Visible = False
        Results.Columns(7).Visible = False
        Results.Columns(6).Visible = False

        'make sure other categories go invisible 
        Results.Columns(10).Visible = False
        Results.Columns(12).Visible = False
        Results.Columns(11).Visible = False
        Results.Columns(14).Visible = False
      Case "clicomp_category5"
        Results.Columns(14).HeaderText = column_name.SelectedItem.Text
        Results.Columns(14).Visible = True
        Results.Columns(9).Visible = True
        Results.Columns(8).Visible = False
        Results.Columns(7).Visible = False
        Results.Columns(6).Visible = False

        'make sure other categories go invisible 
        Results.Columns(10).Visible = False
        Results.Columns(12).Visible = False
        Results.Columns(11).Visible = False
        Results.Columns(13).Visible = False

    End Select
  End Sub
  Dim count As Integer = 0
  Public color As String = "container_grid"
  Dim linked As New Label
  Dim lab As New Label
  Dim cont As New Label
  Dim but As New ImageButton
  Dim fly As New OboutInc.Flyout2.Flyout
  Dim container As New Panel
  Sub Transaction_Bind(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 'Handles Results.ItemDataBound

    'If Not IsNothing(e.Item.Cells(23)) Then
    '    'e.Item.Cells(23).Text = "Relationships"
    'End If

    Dim text_string As String = ""
    Dim text_string2 As String = ""
    Dim text_string3 As String = ""
    Dim id As String() = Split("", "|")
    ''Response.Write(e.Item.Cells(2).Text & "<br />")
    Dim act_name As String() = Split("", "|")
    Dim act_name_id As String() = Split("", "|")
    Dim perc As String() = Split("", "|")
    Dim cont_id As String() = Split("", "|")
    Dim td As New TableCell
    Dim maxrow As Integer = 0
    Dim display As Integer = 0
    Dim rowadd As Integer = 0
    If InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_TRANSACTION.ASPX") > 0 Then
      rowadd = 15
      maxrow = 32
      id = Split(e.Item.Cells(0).Text, "|")
      act_name = Split(e.Item.Cells(2).Text, "|")
      act_name_id = Split(e.Item.Cells(33).Text, "|")
      perc = Split("", "|")
      cont_id = Split(e.Item.Cells(1).Text, "|")
      display = 1 'transaction
    ElseIf InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_AIR.ASPX") > 0 Then
      rowadd = 10
      maxrow = 39
      id = Split(e.Item.Cells(37).Text, "|")
      act_name = Split(e.Item.Cells(36).Text, "|")
      perc = Split(e.Item.Cells(39).Text, "|")
      cont_id = Split(e.Item.Cells(38).Text, "|")
      display = 2 'aircraft
    End If


    but.ImageUrl = "~/images/magnify.png"
    but.OnClientClick = "return false;"

    Dim text As New Label
    Dim fly_text As String = ""


    If InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_TRANSACTION.ASPX") > 0 Or InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_AIR.ASPX") Then

      If Not IsNothing(e.Item.Cells(maxrow)) Then

        If Trim(e.Item.Cells(3).Text) <> "&nbsp;" Then
          For j = 0 To UBound(id)

            'account sorting
            Dim run_through As Boolean = True
            Dim show_broker As Boolean = False
            If display = 2 Then
              'If Session.Item("types_of_owners") = "" Then
              '    Session.Item("types_of_owners") = "all"
              'End If
              'Select Case Session.Item("types_of_owners")
              '    Case "all"
              '        Select Case act_name(j)
              '            Case "Previous Owner", "Fractional Owner", "Owner", "Co-Owner", "Program Holder", "Exclusive Broker"
              '                run_through = True
              '            Case Else
              '                run_through = False
              '        End Select

              '    Case "whole"
              '        Select Case act_name(j)
              '            Case "Owner", "Previous Owner", "Exclusive Broker"
              '                run_through = True
              '            Case Else
              '                run_through = False
              '        End Select
              '    Case "operators"
              '        Select Case act_name(j)
              '            Case "Aircraft Management Company", "Charter Company", "Flight Department", "Hangar", "Lessee", "Managing Company", "Operator", "Program Manager", "Sublesee", "Exclusive Broker"
              '                run_through = True
              '            Case Else
              '                run_through = False
              '        End Select
              '    Case Else
              '        show_broker = True
              run_through = True
              'End Select
            ElseIf display = 1 Then

              Dim ar As String() = Split(Session.Item("transaction_owners"), ",")
              run_through = False


              For t = 0 To UBound(ar)
                If run_through = False Then
                  If ar(t) = act_name_id(j) Then
                    run_through = True
                  Else
                    run_through = False
                  End If
                End If
              Next


            End If

            If cont_id(j) = "" Then
              cont_id(j) = 0
            End If
            If id(j) = "" Then
              id(j) = 0
            End If
            Dim add_me As Boolean = True
            If show_broker = False And act_name(j) = "Exclusive Broker" Then
              add_me = False
            End If


            Dim r As String = "JETNET"
            'If r = "JETNET" Then

            If run_through = True Then
              Dim ac As String = e.Item.Cells(5).Text
              'If ac = "10427" Or ac = "10426" Then
              '    ac = "here!!!!!!"
              'End If
              If e.Item.Cells(3).Text = "JETNET" Or e.Item.Cells(3).Text = "CLIENT" Then
                'counter = counter + 1
                'Response.Write(counter & "!!!!!")
                If id(j) <> 0 Then

                  Dim comp_name As Array = Split("", "|")
                  Dim comp_address As Array = Split("", "|")
                  Dim comp_address2 As Array = Split("", "|")
                  Dim comp_city As Array = Split("", "|")
                  Dim comp_state As Array = Split("", "|")
                  Dim comp_country As Array = Split("", "|")
                  Dim comp_zip_code As Array = Split("", "|")
                  Dim comp_email_address As Array = Split("", "|")
                  Dim comp_web_address As Array = Split("", "|")

                  Dim contact_first_name As Array = Split("", "|")
                  Dim contact_last_name As Array = Split("", "|")
                  Dim contact_middle_initial As Array = Split("", "|")
                  Dim contact_title As Array = Split("", "|")
                  Dim contact_preferred_name As Array = Split("", "|")
                  Dim contact_notes As Array = Split("", "|")
                  Dim contact_email_address As Array = Split("", "|")
                  Dim contact_type_id As Array = Split("", "|")
                  Dim comp_source As Array = Split("", "|")
                  Dim client_exists = False
                  Dim source As String = ""

                  If display = 2 Then
                    Dim startRow As Integer = 19
                    'ac starts at 19
                    comp_name = Split(e.Item.Cells(startRow).Text, "|")
                    comp_address = Split(e.Item.Cells(startRow + 1).Text, "|")
                    comp_address2 = Split(e.Item.Cells(startRow + 2).Text, "|")
                    comp_city = Split(e.Item.Cells(startRow + 3).Text, "|")
                    comp_state = Split(e.Item.Cells(startRow + 4).Text, "|")
                    comp_country = Split(e.Item.Cells(startRow + 5).Text, "|")
                    comp_zip_code = Split(e.Item.Cells(startRow + 6).Text, "|")
                    comp_email_address = Split(e.Item.Cells(startRow + 7).Text, "|")
                    comp_web_address = Split(e.Item.Cells(startRow + 8).Text, "|")

                    contact_first_name = Split(e.Item.Cells(startRow + 9).Text, "|")
                    contact_last_name = Split(e.Item.Cells(startRow + 10).Text, "|")
                    contact_middle_initial = Split(e.Item.Cells(startRow + 11).Text, "|")
                    contact_title = Split(e.Item.Cells(startRow + 12).Text, "|")
                    contact_preferred_name = Split(e.Item.Cells(startRow + 13).Text, "|")
                    contact_notes = Split(e.Item.Cells(startRow + 14).Text, "|")
                    contact_email_address = Split(e.Item.Cells(startRow + 15).Text, "|")


                    comp_source = Split(e.Item.Cells(startRow + 16).Text, "|")
                    If InStr(e.Item.Cells(35).Text, "CLIENT") > 0 Then
                      client_exists = True
                    End If
                  Else
                    'trans starts 17
                    comp_name = Split(e.Item.Cells(17).Text, "|")
                    comp_address = Split(e.Item.Cells(18).Text, "|")
                    comp_address2 = Split(e.Item.Cells(19).Text, "|")
                    comp_city = Split(e.Item.Cells(20).Text, "|")
                    comp_state = Split(e.Item.Cells(21).Text, "|")
                    comp_country = Split(e.Item.Cells(22).Text, "|")
                    comp_zip_code = Split(e.Item.Cells(23).Text, "|")
                    comp_email_address = Split(e.Item.Cells(24).Text, "|")
                    comp_web_address = Split(e.Item.Cells(25).Text, "|")

                    contact_first_name = Split(e.Item.Cells(26).Text, "|")
                    contact_last_name = Split(e.Item.Cells(27).Text, "|")
                    contact_middle_initial = Split(e.Item.Cells(28).Text, "|")
                    contact_title = Split(e.Item.Cells(29).Text, "|")
                    contact_preferred_name = Split(e.Item.Cells(30).Text, "|")
                    contact_notes = Split(e.Item.Cells(31).Text, "|")
                    contact_email_address = Split(e.Item.Cells(32).Text, "|")
                    contact_type_id = Split(e.Item.Cells(33).Text, "|")
                  End If
                  If comp_name(j) <> "" Then
                    Dim address_string As String = ""
                    Dim lng_address_string As String = ""
                    Dim phone_text As String = ""
                    Dim contact_phone_text As String = ""
                    Dim font_color As String = ""
                    fly = New OboutInc.Flyout2.Flyout
                    linked = New Label
                    lab = New Label
                    but = New ImageButton
                    address_string = ""
                    text = New Label
                    cont = New Label
                    container = New Panel
                    'container.BorderColor = Drawing.Color.Red
                    'container.BorderWidth = 1
                    ' Response.Write(comp_name(j) & "1<br /><br />")

                    'linked.CommandName = "comp_details_from_ac"
                    'linked.ID = "details_view_com" & j & CInt(id(j))
                    'AddHandler linked.Click, AddressOf dispDetails_link
                    'linked.CommandArgument = CInt(id(j)) & "|" & e.Item.Cells(3).Text

                    text_string3 = comp_name(j)
                    If display = 2 Then
                      If comp_source(j) = "JETNET" Then
                        If color = "container_grid" Then
                          color = "container_grid_alt"
                        Else
                          color = "container_grid"
                        End If
                        font_color = "#023657"
                        source = "JETNET"
                      ElseIf comp_source(j) = "CJETNET" Then
                        If color = "container_grid" Then
                          color = "container_grid_alt"
                        Else
                          color = "container_grid"
                        End If
                        font_color = "#023657"
                        client_exists = True
                        source = "CLIENT"
                      ElseIf comp_source(j) = "JCLIENT" Then
                        If color = "container_grid_client" Then
                          color = "container_grid_alt_client"
                        Else
                          color = "container_grid_client"
                        End If
                        client_exists = True
                        source = "CLIENT"
                        font_color = "#7a3733"
                      ElseIf comp_source(j) = "CLIENT" Then
                        If color = "container_grid_client" Then
                          color = "container_grid_alt_client"
                        Else
                          color = "container_grid_client"
                        End If
                        source = "CLIENT"
                        font_color = "#7a3733"
                      End If
                    Else

                      If color = "container_grid" Then
                        color = "container_grid_alt"
                      Else
                        color = "container_grid"
                      End If
                      font_color = "#023657"
                    End If

                    lng_address_string = ""
                    lng_address_string = lng_address_string & "<strong style='font-size:14px;color:#" & font_color & ";'>" & comp_name(j) & "</strong><br />"
                    If comp_address(j) <> "" Then
                      lng_address_string = lng_address_string & comp_address(j) & "<br />"
                    End If
                    If comp_address2(j) <> "" Then
                      lng_address_string = lng_address_string & " " & comp_address2(j) & "<br />"
                    End If
                    If comp_city(j) <> "" Then
                      address_string = address_string & comp_city(j) & ","
                      lng_address_string = lng_address_string & comp_city(j) & ","
                    End If
                    If comp_state(j) <> "" Then
                      address_string = address_string & " " & comp_state(j)
                      lng_address_string = lng_address_string & " " & comp_state(j) & "<br />"
                    End If
                    If comp_zip_code(j) <> "" Then
                      lng_address_string = lng_address_string & " " & comp_zip_code(j) & "<br />"
                    End If
                    If comp_country(j) <> "" Then
                      address_string = address_string & " " & comp_country(j)
                      lng_address_string = lng_address_string & " " & comp_country(j) & "<br />"
                    End If
                    If comp_email_address(j) <> "" Then
                      lng_address_string = lng_address_string & "<br /><a href='mailto:" & comp_email_address(j) & "'>" & comp_email_address(j) & "</a>"
                    End If
                    If comp_web_address(j) <> "" Then
                      If InStr(comp_web_address(j), "http://") = 0 Then
                        lng_address_string = lng_address_string & "<br /><a href='http://" & comp_web_address(j) & "' target='_new'>" & comp_web_address(j) & "</a>"
                      Else
                        lng_address_string = lng_address_string & "<br /><a href='" & comp_web_address(j) & "' target='_new'>" & comp_web_address(j) & "</a>"
                      End If
                    End If
                    'linked.CommandArgument = CInt(id(j)) & "|" & e.Item.Cells(3).Text

                    If client_exists = True Then
                      comp_source(j) = "JETNET"
                    End If

                    container.CssClass = color
                    If display = 2 Then
                      linked.Text = "<span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id(j)) & "&source=" & source & "&type=1'>" & text_string3 & " (<em>" & act_name(j) & clsGeneral.clsGeneral.showpercent(perc(j), act_name(j)) & "</em>)</a></span>"
                    Else
                      linked.Text = "<span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id(j)) & "&source=" & e.Item.Cells(3).Text & "&type=1'>" & text_string3 & " (<em>" & act_name(j) & "</em>)</a></span>"
                    End If
                    text_string2 = "<span style='font-size:9px;'><i>" & address_string & "</i></span>"
                    ' lab.Text = "<br />" & text_string2
                    If display = 2 Then
                      cont.Text = "<br clear='all' /><span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id(j)) & "&contact_ID=" & CInt(cont_id(j)) & "&source=" & source & "&type=1'>" & contact_first_name(j) & " " & contact_last_name(j) & "</a></span>"
                    Else
                      cont.Text = "<br clear='all' /><span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id(j)) & "&contact_ID=" & CInt(cont_id(j)) & "&source=" & e.Item.Cells(3).Text & "&type=1'>" & contact_first_name(j) & " " & contact_last_name(j) & "</a></span>"
                    End If
                    'cont.CommandName = "cont_details_from_ac"

                    'cont.CommandArgument = CInt(cont_id(j)) & "|" & e.Item.Cells(3).Text & "|" & CInt(id(j))
                    'AddHandler cont.Click, AddressOf dispDetails_link

                    Dim contact_text As String = ""
                    'set up contact mouseover display

                    If Not contact_first_name(j) = "" Then
                      contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & contact_first_name(j)
                    End If
                    If Not contact_middle_initial(j) = "" Then
                      contact_text = contact_text & " " & contact_middle_initial(j)
                    End If
                    If Not contact_last_name(j) = "" Then
                      contact_text = contact_text & " " & contact_last_name(j) & "</strong><br />"
                    End If
                    If Not contact_title(j) = "" Then
                      contact_text = contact_text & contact_title(j) & " <br />"
                    End If
                    If Not contact_email_address(j) = "" Then
                      contact_text = contact_text & "<a href='mailto:" & contact_email_address(j) & "' class='non_special_link'>" & contact_email_address(j) & "</a>"
                    End If

                    If text_string3 <> "" Then
                      'If Not Page.IsPostBack Then

                      ' If Master.PerformDatabaseAction = True Then
                      count = count + 1
                      'Response.Write("I am touching database! " & Master.Search & "<br />")


                      ''Query for phone numbers. 
                      'aTempTable = Master.aclsData_Temp.GetPhoneNumbers(id(j), 0, e.Item.Cells(3).Text, 0)
                      'If Not IsNothing(aTempTable) Then

                      '    If aTempTable.Rows.Count > 0 Then
                      '        For Each q As DataRow In aTempTable.Rows
                      '            If q("pnum_contact_id") <> 0 Then
                      '                contact_phone_text = contact_phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                      '            Else
                      '                phone_text = phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                      '            End If
                      '        Next
                      '    End If
                      '    'End If

                      '    ' Session.Item("NotRealPostBack") = false 'do not rework database connections
                      'End If
                    End If
                    If contact_phone_text <> "" Then
                      contact_phone_text = "<br /><br /><strong style='font-size:14px;color:#67A0D9;'>CONTACT PHONE NUMBERS</strong><br />" & contact_phone_text
                    End If

                    If phone_text <> "" Then
                      phone_text = "<br /><br /><strong style='font-size:14px;color:#67A0D9;'>COMPANY PHONE NUMBERS</strong><br />" & phone_text
                    End If

                    If add_me = True Then
                      container.Controls.Add(linked)
                      'e.Item.Cells(rowadd).Controls.Add(linked)
                    End If
                    but.ID = "Button" & j & id(j)
                    but.ImageUrl = "~/images/magnify.png"
                    but.OnClientClick = "return false;"

                    fly.Align = OboutInc.Flyout2.AlignStyle.TOP
                    fly.Position = OboutInc.Flyout2.PositionStyle.TOP_RIGHT
                    fly.FlyingEffect = OboutInc.Flyout2.FlyingEffectStyle.TOP_RIGHT
                    fly.FadingEffect = True
                    fly_text = clsGeneral.clsGeneral.MouseOverTextStart()
                    fly_text = fly_text & UCase(lng_address_string)

                    ' If cont_id(j) = 0 Then
                    'phone now
                    fly_text = fly_text & UCase(phone_text)
                    'End If
                    If contact_text <> "" Then
                      fly_text = fly_text & "<br /><br />" & UCase(contact_text)
                    End If
                    ' If cont_id(j) <> 0 Then
                    'phone now
                    fly_text = fly_text & UCase(contact_phone_text)
                    'End If


                    fly_text = fly_text & clsGeneral.clsGeneral.MouseOverTextEnd()
                    text.Text = fly_text
                    fly.AttachTo = "Button" & j & id(j)
                    fly.Controls.Add(text)
                    If add_me = True Then
                      container.Controls.Add(but)
                      container.Controls.Add(lab)
                      ' e.Item.Cells(rowadd).Controls.Add(but)
                      ' e.Item.Cells(rowadd).Controls.Add(lab)
                    End If
                    If display = 2 Then
                      If act_name(j) = "Exclusive Broker" Then
                        Dim ex As Label = e.Item.Cells(15).FindControl("popup_ex")
                        Dim flyout1 As OboutInc.Flyout2.Flyout = e.Item.Cells(16).FindControl("Flyout1")
                        Dim str As String = ex.Text
                        ' ex.Text = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>"
                        flyout1.Controls.Clear()
                        flyout1.Controls.Add(text)
                      End If
                    End If
                  Else
                    add_me = False ' no company to add :(
                    'change add me to false so a company doesn't get added.
                    'fixed 6/14/2011
                  End If

                  If add_me = True Then
                    container.Controls.Add(cont)
                    ' e.Item.Cells(rowadd).Controls.Add(cont)
                  End If
                  If add_me = True Then
                    container.Controls.Add(fly)
                    'e.Item.Cells(rowadd).Controls.Add(fly)
                    Dim pan As Panel = e.Item.Cells(rowadd).FindControl("company_hold")
                    pan.Controls.Add(container)
                    'e.Item.Cells(rowadd).Controls.Add(container)
                  End If

                  'Next

                End If
              End If
            End If
          Next
        End If
      End If
    End If

  End Sub



  Private Sub Results_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Results.PageIndexChanged
    'Paging for gridview
    Master.PerformDatabaseAction = True
    Try
      Results.DataSource = Session("Results")
      table = Session("Results")
      Results.CurrentPageIndex = e.NewPageIndex
      ToggleAircraftListingDataBinding()

      Results.DataBind()
      Master.Next_Button_Visible = True
      Master.Previous_Button_Visible = True
      If Results.CurrentPageIndex = Results.PageCount - 1 Then
        Master.Next_Button_Visible = False
      End If
      If (Results.CurrentPageIndex = 0) Then
        Master.Previous_Button_Visible = False
      End If


      Dim currentrecord, realcount As Integer
      currentrecord = (Results.PageSize * Results.CurrentPageIndex) - table.Rows.Count + table.Rows.Count
      If currentrecord = 0 Then
        realcount = 1
      Else
        realcount = currentrecord + 1
      End If

      If currentrecord + Results.PageSize >= table.Rows.Count Then
        Master.SetRecordCount = realcount & "-" & table.Rows.Count & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
      Else
        Master.SetRecordCount = realcount & "-" & currentrecord + Results.PageSize & " of " & table.Rows.Count & " " & Master.NameOfListingType & " Records"
      End If

      Session("AtPage") = Results.CurrentPageIndex
      Master.PerformDatabaseAction = False
    Catch ex As Exception
      error_string = "listing.aspx.vb - Results_PageIndexChanged() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Display Details if someone clicks on something and needs more info"

  Private Sub dispDetails_link(ByVal s As LinkButton, ByVal e As EventArgs)
    Try
      Select Case (s.CommandName)
        Case "details"
          Dim ar As String() = Split(s.CommandArgument, "|")
          Master.ListingID = ar(0)
          Master.ListingSource = ar(1)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_contact"
          Dim ar As String() = Split(s.CommandArgument, "|")
          Master.ListingID = ar(2)
          Master.ListingSource = ar(1)
          Master.Listing_ContactID = ar(0)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
      End Select
    Catch ex As Exception
      error_string = "listing.aspx.vb - dispDetails_link() - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
  Sub ChangeAction(ByVal sender As Object, ByVal e As EventArgs)
    Dim status As RadioButtonList = sender.Item.FindControl("change_status")

    If status.SelectedValue <> "C" And status.SelectedValue <> "D" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "New Window", "javascript:alert('Please select either Completed or Dismissed to change the status of this action item.');", True)
    Else
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "New Window", "javascript:load('edit_note.aspx?type=UPDATE_STATUS&status=" & status.SelectedValue & "&id=" & Convert.ToInt32(sender.Item.Cells(6).Text) & "','scrollbars=no,menubar=no,height=30,width=400,resizable=yes,toolbar=no,location=no,status=no');", True)
    End If
  End Sub
  Sub dispDetails(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Select Case (e.CommandName)
        Case "cont_details_from_ac"
          Dim x As Array = Split(e.CommandArgument, "|")
          Master.ListingID = x(2)
          Master.ListingSource = x(1)
          Master.Listing_ContactID = x(0)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "comp_details_from_ac"
          Dim x As Array = Split(e.CommandArgument, "|")
          Master.ListingID = x(0)
          Master.ListingSource = x(1)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(2).Text)
          Master.ListingSource = (e.Item.Cells(3).Text)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_contact"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(2).Text)
          Master.ListingSource = (e.Item.Cells(3).Text)
          Master.Listing_ContactID = Convert.ToInt32(e.Item.Cells(4).Text)
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_ac"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(5).Text)
          Master.ListingSource = (e.Item.Cells(3).Text)
          Master.Listing_ContactID = 0
          Master.TypeOfListing = 3
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_ac_market"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(0).Text)
          Master.ListingSource = "JETNET"
          Master.Listing_ContactID = 0
          Master.TypeOfListing = 3
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_ac_market_client"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(1).Text)
          Master.ListingSource = "CLIENT"
          Master.Listing_ContactID = 0
          Master.TypeOfListing = 3
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_ac_trans"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(5).Text)
          Master.ListingSource = (e.Item.Cells(3).Text)
          Master.Listing_ContactID = 0
          Master.TypeOfListing = 3
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_notes_com"
          If Convert.ToInt32(e.Item.Cells(2).Text) <> 0 Then
            Master.ListingID = Convert.ToInt32(e.Item.Cells(2).Text)
            Master.ListingSource = "JETNET"
          Else
            Master.ListingID = Convert.ToInt32(e.Item.Cells(3).Text)
            Master.ListingSource = "CLIENT"
          End If
          Dim arrayed() As String
          ReDim arrayed(0)
          arrayed(0) = ""
          Session("my_ids") = arrayed
          Master.TypeOfListing = 1
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_notes_ac"
          If Convert.ToInt32(e.Item.Cells(4).Text) <> 0 Then
            Master.ListingID = Convert.ToInt32(e.Item.Cells(4).Text)
            Master.ListingSource = "JETNET"
          Else
            Master.ListingID = Convert.ToInt32(e.Item.Cells(5).Text)
            Master.ListingSource = "CLIENT"
          End If
          Dim arrayed() As String
          ReDim arrayed(0)
          arrayed(0) = ""
          Session("my_ids") = arrayed
          Master.TypeOfListing = 3
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "details_job"
          Master.ListingID = Convert.ToInt32(e.Item.Cells(2).Text)
          Master.ListingSource = "CLIENT"
          Master.Listing_ContactID = Convert.ToInt32(e.Item.Cells(3).Text)
          Master.TypeOfListing = 1
          Master.Listing_IsJob = True
          Response.Redirect("details.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          Master.m_bIsTerminating = True
        Case "complete_action"
          Dim status As RadioButtonList = e.Item.FindControl("change_status")

          If status.SelectedValue <> "C" And status.SelectedValue <> "D" Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "New Window", "javascript:alert('Please select either Completed or Dismissed to change the status of this action item.');", True)
          Else
            'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "New Window", "javascript:load('edit_note.aspx?type=UPDATE_STATUS&status=" & status.SelectedValue & "&id=" & Convert.ToInt32(e.Item.Cells(6).Text) & "','scrollbars=no,menubar=no,height=30,width=400,resizable=yes,toolbar=no,location=no,status=no');", True)

            Dim aclsLocal_Notes As New clsLocal_Notes
            Dim aTempTable2 As DataTable

            aTempTable2 = Master.aclsData_Temp.Get_Local_Notes_Client_NoteID(Convert.ToInt32(e.Item.Cells(6).Text))

            ' check the state of the DataTable
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable2.Rows
                  aclsLocal_Notes.lnote_jetnet_ac_id = r("lnote_jetnet_ac_id")
                  aclsLocal_Notes.lnote_client_ac_id = r("lnote_client_ac_id")
                  aclsLocal_Notes.lnote_jetnet_comp_id = r("lnote_jetnet_comp_id")
                  aclsLocal_Notes.lnote_client_comp_id = r("lnote_client_comp_id")
                  aclsLocal_Notes.lnote_client_contact_id = r("lnote_client_contact_id")
                  aclsLocal_Notes.lnote_jetnet_contact_id = r("lnote_jetnet_contact_id")
                  aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")
                  aclsLocal_Notes.lnote_document_flag = r("lnote_document_flag")
                  aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")


                  If Not IsDBNull(r("lnote_note")) Then
                    If r("lnote_note") <> "" Then
                      aclsLocal_Notes.lnote_status = "A"
                      If status.SelectedValue = "C" Then
                        aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(r("lnote_note") & " ** Completed **")
                      Else
                        aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(r("lnote_note") & " ** Dismissed **")
                      End If
                    Else
                      aclsLocal_Notes.lnote_status = status.SelectedValue
                      aclsLocal_Notes.lnote_note = ""
                    End If
                  Else
                    aclsLocal_Notes.lnote_status = status.SelectedValue
                    aclsLocal_Notes.lnote_note = ""
                  End If

                  aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")

                  aclsLocal_Notes.lnote_id = r("lnote_id")
                  aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
                  aclsLocal_Notes.lnote_action_date = Now() ' DB requires some value
                  aclsLocal_Notes.lnote_user_login = r("lnote_user_login") ' DB requires a string value greater than 0
                  aclsLocal_Notes.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)
                  aclsLocal_Notes.lnote_notecat_key = r("lnote_notecat_key")
                  aclsLocal_Notes.lnote_user_id = r("lnote_user_id")
                  aclsLocal_Notes.lnote_schedule_start_date = r("lnote_schedule_start_date")
                  aclsLocal_Notes.lnote_schedule_end_date = r("lnote_schedule_end_date")


                  If Master.aclsData_Temp.update_localNote(aclsLocal_Notes) = True Then
                    Dim url As String = "listing_action.aspx"
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = '" & url & "';", True)
                  Else
                    If Master.aclsData_Temp.class_error <> "" Then
                      error_string = Master.aclsData_Temp.class_error
                      Master.LogError("listing.aspx.vb - update note status() - " & error_string)
                    End If
                    Master.display_error()
                  End If

                Next
              End If
            End If

          End If
      End Select
    Catch ex As Exception
      error_string = "listing.aspx.vb - dispDetails(" & e.CommandName & " - " & Request.ServerVariables("SCRIPT_NAME").ToString() & ") - " & ex.Message
      Master.LogError(error_string)
    End Try
  End Sub
#End Region
  Function Aircraft_Item_Databound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) As String

    Aircraft_Item_Databound = ""
    Dim lng_address_string As String = ""
    Dim comp_name As String = ""
    Dim comp_source As String = ""
    Dim comp_address As String = ""
    Dim comp_address2 As String = ""
    Dim comp_zip_code As String = ""
    Dim comp_country As String = ""
    Dim comp_city As String = ""
    Dim comp_state As String = ""
    Dim comp_web_address As String = ""
    Dim comp_email_address As String = ""
    Dim comp_id As Integer = 0
    Dim office_phone As String = ""
    Dim office_fax As String = ""

    Dim contact_email_address As String = ""
    Dim contact_first_name As String = ""
    Dim contact_last_name As String = ""
    Dim contact_middle_initial As String = ""
    Dim contact_title As String = ""

    Dim perc As String = ""
    Dim act_name As String = ""
    Dim text_string2 As String = ""
    Dim address_string As String = ""
    Dim text_string3 As String = ""
    Dim phone_text As String = ""
    Dim contact_text As String = ""
    Dim short_contact_text As String = ""
    Dim font_color As String = ""
    Dim source As String = ""
    Dim fly_text As String = ""
    Dim client_exists As Boolean = True
    Dim text As New Label
    Dim operator_string As String = Session.Item("types_of_owners")

    'If operator_string = "" Then
    '    operator_string = "all"
    'End If

    Dim combined_table As New DataTable
    Dim first_table As New DataTable
    Dim second_table As New DataTable

    Dim counter As Integer = 0
    If Trim(e.Item.Cells(3).Text) <> "&nbsp;" Then
      If Not IsNothing(e.Item.Cells(16)) Then
        'Response.Write(e.Item.Cells(3).Text & " source<br />")
        'Response.Write(e.Item.Cells(2).Text & " other source<br />")
        'Response.Write(e.Item.Cells(4).Text & " ac id<br />")
        'Response.Write(e.Item.Cells(5).Text & " other ac ID<br />")

        If Not IsNothing(e.Item.Cells(4).Text) And Not IsNothing(e.Item.Cells(3).Text) Then
          first_table = Master.aclsData_Temp.Aircraft_Listing_Company_Display(e.Item.Cells(4).Text, e.Item.Cells(3).Text, operator_string)

          If IsNothing(first_table) Then
            If Master.aclsData_Temp.class_error <> "" Then
              error_string = Master.aclsData_Temp.class_error
              Master.LogError("listing.aspx.vb - 1. Generation of Company Listings on Aircraft Search() - " & error_string)
            End If
            Master.display_error()
          End If

        End If


        If Not IsNothing(e.Item.Cells(2).Text) And Not IsNothing(e.Item.Cells(5).Text) Then
          If (IsNumeric(e.Item.Cells(5).Text)) And ((e.Item.Cells(2).Text = "CLIENT" Or e.Item.Cells(2).Text = "JETNET")) Then
            second_table = Master.aclsData_Temp.Aircraft_Listing_Company_Display(e.Item.Cells(5).Text, e.Item.Cells(2).Text, operator_string)
            If IsNothing(second_table) Then
              If Master.aclsData_Temp.class_error <> "" Then
                error_string = Master.aclsData_Temp.class_error
                Master.LogError("listing.aspx.vb - 2. Generation of Company Listings on Aircraft Search() - " & error_string)
              End If
              Master.display_error()
            End If

          End If
        End If

        If first_table.Rows.Count > 0 Then 'if the first table has rows..
          combined_table = first_table 'we default to setting the display table as the first table, just in case there is no second table.
          If second_table.Rows.Count > 0 Then 'if the second table has rows
            combined_table = Master.aclsData_Temp.Combine_Jetnet_Client_Company_Listing_Display(first_table, second_table) 'then we send to a combining and distinct function
          End If
        ElseIf second_table.Rows.Count > 0 Then 'if the first table doesn't have rows, does the second?
          combined_table = second_table 'yes, it has rows.
        End If

        If Not IsNothing(combined_table) Then
          If combined_table.Rows.Count > 0 Then
            For Each company As DataRow In combined_table.Rows
              'fly = New OboutInc.Flyout2.Flyout
              linked = New Label
              lab = New Label
              'but = New ImageButton
              address_string = ""
              lng_address_string = ""
              short_contact_text = ""
              contact_text = ""
              phone_text = ""
              text = New Label
              cont = New Label
              container = New Panel


              counter = counter + 1
              comp_name = IIf(Not IsDBNull(company("comp_name")), company("comp_name"), "")
              comp_source = IIf(Not IsDBNull(company("source")), company("source"), "")
              comp_address = IIf(Not IsDBNull(company("comp_address")), company("comp_address"), "")
              comp_address2 = IIf(Not IsDBNull(company("comp_address2")), company("comp_address2"), "")
              comp_zip_code = IIf(Not IsDBNull(company("comp_zip_code")), company("comp_zip_code"), "")
              comp_country = IIf(Not IsDBNull(company("comp_country")), company("comp_country"), "")
              comp_city = IIf(Not IsDBNull(company("comp_city")), company("comp_city"), "")
              comp_state = IIf(Not IsDBNull(company("comp_state")), company("comp_state"), "")
              comp_web_address = IIf(Not IsDBNull(company("comp_web_address")), company("comp_web_address"), "")
              comp_email_address = IIf(Not IsDBNull(company("comp_email_address")), company("comp_email_address"), "")
              comp_id = IIf(Not IsDBNull(company("comp_id")), company("comp_id"), 0)
              perc = IIf(Not IsDBNull(company("percentage")), company("percentage"), "")
              act_name = IIf(Not IsDBNull(company("actype_name")), company("actype_name"), "")
              office_phone = IIf(Not IsDBNull(company("comp_office_phone")), company("comp_office_phone"), "")
              office_fax = IIf(Not IsDBNull(company("comp_fax_phone")), company("comp_fax_phone"), "")

              contact_email_address = IIf(Not IsDBNull(company("contact_email_address")), company("contact_email_address"), "")
              contact_first_name = IIf(Not IsDBNull(company("contact_first_name")), company("contact_first_name"), "")
              contact_last_name = IIf(Not IsDBNull(company("contact_last_name")), company("contact_last_name"), "")
              contact_middle_initial = IIf(Not IsDBNull(company("contact_middle_initial")), company("contact_middle_initial"), "")
              contact_title = IIf(Not IsDBNull(company("contact_title")), company("contact_title"), "")



              text_string3 = comp_name

              If comp_source = "JETNET" Then
                If color = "container_grid" Then
                  color = "container_grid_alt"
                Else
                  color = "container_grid"
                End If
                font_color = "#023657"
                source = "JETNET"
              ElseIf comp_source = "CJETNET" Then
                If color = "container_grid" Then
                  color = "container_grid_alt"
                Else
                  color = "container_grid"
                End If
                font_color = "#023657"
                client_exists = True
                source = "CLIENT"
              ElseIf comp_source = "JCLIENT" Then
                If color = "container_grid_client" Then
                  color = "container_grid_alt_client"
                Else
                  color = "container_grid_client"
                End If
                client_exists = True
                source = "CLIENT"
                font_color = "#7a3733"
              ElseIf comp_source = "CLIENT" Then
                If color = "container_grid_client" Then
                  color = "container_grid_alt_client"
                Else
                  color = "container_grid_client"
                End If
                source = "CLIENT"
                font_color = "#7a3733"
              End If

              lng_address_string = ""
              lng_address_string = lng_address_string & "<strong style='font-size:14px;color:#" & font_color & ";'>" & comp_name & "</strong>" & vbNewLine
              If comp_address <> "" Then
                lng_address_string = lng_address_string & comp_address & vbNewLine
              End If
              If comp_address2 <> "" Then
                lng_address_string = lng_address_string & " " & comp_address2 & vbNewLine
              End If
              If comp_city <> "" Then
                address_string = address_string & comp_city & ","
                lng_address_string = lng_address_string & comp_city & ","
              End If
              If comp_state <> "" Then
                address_string = address_string & " " & comp_state
                lng_address_string = lng_address_string & " " & comp_state & " "
              End If
              If comp_zip_code <> "" Then
                lng_address_string = lng_address_string & " " & comp_zip_code & vbNewLine
              End If
              If comp_country <> "" Then
                address_string = address_string & " " & comp_country
                lng_address_string = lng_address_string & comp_country & vbNewLine
              End If
              If comp_email_address <> "" Then
                lng_address_string = lng_address_string & "<a href='mailto:" & comp_email_address & "'>" & comp_email_address & "</a>" & vbNewLine
              End If
              If comp_web_address <> "" Then
                If InStr(comp_web_address, "http://") = 0 Then
                  lng_address_string = lng_address_string & "<a href='http://" & comp_web_address & "' target='_new'>" & comp_web_address & "</a>"
                Else
                  lng_address_string = lng_address_string & "<a href='" & comp_web_address & "' target='_new'>" & comp_web_address & "</a>"
                End If
              End If

              If client_exists = True Then
                comp_source = "JETNET"
              End If
              contact_text = ""

              If Not contact_first_name = "" Then
                contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & contact_first_name
                short_contact_text = vbNewLine & contact_first_name
              End If
              If Not contact_middle_initial = "" Then
                contact_text = contact_text & " " & contact_middle_initial
                short_contact_text = short_contact_text & " " & contact_middle_initial
              End If
              If Not contact_last_name = "" Then
                contact_text = contact_text & " " & contact_last_name & "</strong>" & vbNewLine
                short_contact_text = short_contact_text & " " & contact_last_name
              End If


              If Not contact_title = "" Then
                contact_text = contact_text & contact_title & vbNewLine

              End If


              If Not contact_email_address = "" Then
                contact_text = contact_text & "<a href='mailto:" & contact_email_address & "' class='non_special_link'>" & contact_email_address & "</a>"
              End If

              phone_text = ""
              If office_phone <> "" Then
                phone_text = phone_text & "Office: " & office_phone
              End If

              If office_fax <> "" Then
                phone_text = phone_text & "Fax: " & office_fax
              End If

              If phone_text <> "" Then
                phone_text = vbNewLine & vbNewLine & "<strong style='font-size:14px;color:#67A0D9;'>COMPANY PHONE NUMBERS</strong>" & vbNewLine & phone_text
              End If

              container.CssClass = color


              'e.Item.Cells(rowadd).Controls.Add(linked)

              'but.ID = "Button" & counter & comp_id
              'but.ImageUrl = "~/images/magnify.png"
              'but.OnClientClick = "return false;"

              'fly.Align = OboutInc.Flyout2.AlignStyle.TOP
              'fly.Position = OboutInc.Flyout2.PositionStyle.TOP_RIGHT
              'fly.FlyingEffect = OboutInc.Flyout2.FlyingEffectStyle.TOP_RIGHT
              'fly.FadingEffect = True
              'fly_text = clsGeneral.clsGeneral.MouseOverTextStart()
              fly_text = ""
              fly_text = lng_address_string & phone_text

              If contact_text <> "" Then
                fly_text = fly_text & vbNewLine & vbNewLine & contact_text
              End If

              'fly_text = fly_text & clsGeneral.clsGeneral.MouseOverTextEnd()

              'text.Text = fly_text
              'fly.AttachTo = "Button" & counter & comp_id
              'fly.Controls.Add(text)
              fly_text = Replace(fly_text, vbNewLine & vbNewLine & vbNewLine, vbNewLine & vbNewLine)
              text_string2 = "<span style='font-size:9px;'><i>" & address_string & "</i></span>"
              linked.Text = "<span style='font-size:10px;'><a title=""" & clsGeneral.clsGeneral.stripHTML(fly_text) & """ " & IIf(act_name = "Exclusive Broker", "class = ""purple_text""", "") & " href='details.aspx?comp_ID=" & comp_id & "&source=" & source & "&type=1'>" & text_string3 & " (<em>" & act_name & clsGeneral.clsGeneral.showpercent(perc, act_name) & "</em>)</a> " & short_contact_text & " </span>"
              container.Controls.Add(linked)

              container.Controls.Add(linked)


              If act_name = "Exclusive Broker" Then
                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                  'container.Controls.Add(but)
                  container.Controls.Add(lab)

                  container.Controls.Add(cont)

                  'container.Controls.Add(fly)

                  Dim pan As Panel = e.Item.Cells(11).FindControl("company_hold")
                  If IsNothing(pan.FindControl("Button" & counter & comp_id)) Then
                    pan.Controls.Add(container)
                  End If
                End If
              Else
                'container.Controls.Add(but)
                container.Controls.Add(lab)

                container.Controls.Add(cont)

                'container.Controls.Add(fly)

                Dim pan As Panel = e.Item.Cells(11).FindControl("company_hold")
                If IsNothing(pan.FindControl("Button" & counter & comp_id)) Then
                  pan.Controls.Add(container)
                End If
              End If



            Next
          End If
        End If
      End If
    End If

  End Function


End Class