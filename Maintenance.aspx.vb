Partial Public Class Maintenance

  Inherits System.Web.UI.Page
  Dim AircraftID As Long = 0
  Dim Client_AircraftID As Long = 0
  Dim JournalID As Long = 0
  Dim MaintenanceItemTable As New DataTable 
  Dim Make_name As String = ""
  Dim model_name As String = ""
  Dim amod_id As Long = 0
  Dim Model_Items_List As String = ""


  Private Sub Maintenance_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    If Not IsNothing(Request.Item("acid")) Then
      If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
        AircraftID = CLng(Request.Item("acid").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("cliacid")) Then
      If Not String.IsNullOrEmpty(Request.Item("cliacid").ToString) Then
        Client_AircraftID = CLng(Request.Item("cliacid").ToString.Trim)
      End If
    End If

    'Fills Journal ID
    If Not IsNothing(Request.Item("jid")) Then
      If Not String.IsNullOrEmpty(Request.Item("jid").ToString) Then
        JournalID = CLng(Request.Item("jid").ToString.Trim)
      End If
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y" Then
      Response.Redirect("Default.aspx", False)
    Else

      If Trim(Request.Item("avionics")) = "Y" Then
        Call build_avionics_page()

      Else
        Me.maint_panel.Visible = True
        Me.avionics_panel.Visible = False

        Dim AircraftTextString As String = ""
        If AircraftID > 0 Then
          If Not Page.IsPostBack Then

            If Trim(Request("homebase")) = "Y" Then
              If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Then
                Master.aclsData_Temp.JETNET_DB = My.Settings.LIVE_INHOUSE_MSSQL.ToString
                Session.Item("jetnetClientDatabase") = My.Settings.LIVE_INHOUSE_MSSQL.ToString
              End If
            End If

            'Call Get_AC_MAKE_MODEL(AircraftID, Make_name, model_name, amod_id, "", "", "", "")
            If Trim(Request("all_maint")) = "" Or Trim(Request("all_maint")) = "N" Then
              Model_Items_List = Get_Inspection_List(AircraftID)
              Me.Model_Items_List_Label.Text = Model_Items_List
              view_all_maint.Text = "<a href='maintenance.aspx?acID=" & AircraftID & "&jID=" & JournalID & "&maint_row=" & Trim(Request("maint_row")) & "&homebase=Y&all_maint=Y'>View All Maintenance Items Available</a>"
            Else
              Model_Items_List = ""
              Me.Model_Items_List_Label.Text = ""
              view_all_maint.Text = "<a href='maintenance.aspx?acID=" & AircraftID & "&jID=" & JournalID & "&maint_row=" & Trim(Request("maint_row")) & "&homebase=Y&all_maint=N'>View Model Specific Maintenance Items</a>"
            End If


            'JETNET_DB '
            If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
              closeButton.Attributes.Remove("onclick")
              closeButton.Attributes.Add("onclick", "window.opener.location.href = window.opener.location.href; self.close();")
              Dim AircraftTable As New DataTable
              AircraftTable = Master.aclsData_Temp.GetJETNET_AC_NAME(AircraftID, "")
              AircraftTextString = CommonAircraftFunctions.Display_Aircraft_Information_For_Link(AircraftTable, False, 0)

              Master.SetPageTitle(AircraftTextString & " Maintenance/Inspections Details")
              Master.SetPageText("Maintenance/Inspections Details")
              AircraftInfo.InnerText = AircraftTextString

              LoadGridView_Client()

            ElseIf (HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST) Then
              If (HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com")) Or Trim(Request("homebase")) = "Y" Then
                Dim AircraftTable As New DataTable
                AircraftTable = Master.aclsData_Temp.GetJETNET_AC_NAME(AircraftID, "")
                AircraftTextString = CommonAircraftFunctions.Display_Aircraft_Information_For_Link(AircraftTable, False, 0)

                Master.SetPageTitle(AircraftTextString & " Maintenance/Inspections Details")
                Master.SetPageText("Maintenance/Inspections Details")
                AircraftInfo.InnerText = AircraftTextString

                LoadGridView()
              End If
            End If


            If (HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST) Then
              If (HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com")) Or Trim(Request("homebase")) = "Y" Then
                Me.model_specs.Text = Build_Maintenance_Schedules(AircraftID)
              End If
            End If


            If Trim(Request("maint_row")) <> "" Then
              edit_this_row(CInt(Trim(Request("maint_row"))))
            End If

          End If
        End If

      End If

      End If




  End Sub
  Public Function build_avionics_page() As String

    build_avionics_page = ""

    Try 

      Me.maint_panel.Visible = False
      Me.avionics_panel.Visible = True

      If Trim(Request("homebase")) = "Y" Then
        If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
          Master.aclsData_Temp.JETNET_DB = My.Settings.LIVE_INHOUSE_MSSQL.ToString
          Session.Item("jetnetClientDatabase") = My.Settings.LIVE_INHOUSE_MSSQL.ToString
        End If
      End If

      Dim data_Table As New DataTable

      data_Table = Master.aclsData_Temp.Get_Avionics_Mfr_Names()
      Me.avitem_mfr_name.Items.Clear()
      Me.avitem_mfr_name.Items.Add("")
      If Not IsNothing(data_Table) Then
        If data_Table.Rows.Count > 0 Then
          For Each r As DataRow In data_Table.Rows
            Me.avitem_mfr_name.Items.Add(r.Item("avitem_mfr_name"))
          Next
        End If
      End If

      If Trim(Request("id")) <> "" Then

        data_Table = Nothing
        data_Table = Master.aclsData_Temp.Get_Avionics_Item_By_ID(Trim(Request("id")))

        If Not IsNothing(data_Table) Then
          If data_Table.Rows.Count > 0 Then
            For Each r As DataRow In data_Table.Rows

              If Not IsDBNull(r.Item("avitem_id")) Then
                Me.avitem_id.Text = Trim(r.Item("avitem_id"))
              Else
                Me.avitem_id.Text = "0"
              End If

              If Not IsDBNull(r.Item("avitem_name")) Then
                Me.avitem_name.Text = Trim(r.Item("avitem_name"))
              Else
                Me.avitem_name.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_mfr_name")) Then
                Me.avitem_mfr_name.SelectedValue = Trim(r.Item("avitem_mfr_name"))
              Else
                Me.avitem_mfr_name.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_item_name")) Then
                Me.avitem_item_name.Text = Trim(r.Item("avitem_item_name"))
              Else
                Me.avitem_item_name.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_Description")) Then
                Me.avitem_Description.Text = Trim(r.Item("avitem_Description"))
              Else
                Me.avitem_Description.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_web_address")) Then
                Me.avitem_web_address.Text = Trim(r.Item("avitem_web_address"))
              Else
                Me.avitem_web_address.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_research_description")) Then
                Me.avitem_research_description.Text = Trim(r.Item("avitem_research_description"))
              Else
                Me.avitem_research_description.Text = ""
              End If
              If Not IsDBNull(r.Item("avitem_upgrade_cost")) Then
                Me.avitem_upgrade_cost.Text = Trim(r.Item("avitem_upgrade_cost"))
              Else
                Me.avitem_upgrade_cost.Text = ""
              End If

              If Not IsDBNull(r.Item("avitem_upgrade_downtime")) Then
                Me.avitem_upgrade_downtime.Text = Trim(r.Item("avitem_upgrade_downtime"))
              Else
                Me.avitem_upgrade_downtime.Text = ""
              End If

            Next
          End If
        End If

      End If

    Catch ex As Exception

    End Try

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
  Public Function get_amod_id(ByVal ac_id As Long) As Long
    get_amod_id = 0

    Dim tmpQuery3 As String = ""
    Dim counter111 As Integer = 0
    Dim SqlConn2 As New System.Data.SqlClient.SqlConnection 
    Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand 
    Dim adoTempRS2 As System.Data.SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing

    Try
      SqlConn2.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn2.Open()
      SqlCommand2.Connection = SqlConn2 

      tmpQuery3 = " Select distinct amod_id from aircraft_model with (NOLOCK) inner join aircraft with (NOLOCK) on ac_amod_id = amod_id and ac_journ_id = 0 where ac_id = " & ac_id
 
    
      SqlCommand2.CommandText = tmpQuery3
      SqlCommand2.CommandType = CommandType.Text
      SqlCommand2.CommandTimeout = 60
      adoTempRS2 = SqlCommand2.ExecuteReader(CommandBehavior.CloseConnection)

      If adoTempRS2.HasRows Then

        Do While adoTempRS2.Read

          get_amod_id = adoTempRS2("amod_id")

        Loop
      End If

    Catch ex As Exception
    Finally
      adoTempRS2.Close()
      SqlConn2.Close()
      SqlConn2.Dispose()
    End Try
  End Function
  Public Function Build_Maintenance_Schedules(ByVal ac_id As Long) As String

    Dim htmlOut_TopInspections As StringBuilder = New StringBuilder()
    Dim htmlOut_TopHowMaintained As StringBuilder = New StringBuilder()
    Dim htmlOut_InspectionSchedule As StringBuilder = New StringBuilder()
    Dim htmlOut_TopProgramNames As StringBuilder = New StringBuilder()
    Dim htmlOut As StringBuilder = New StringBuilder()

    Dim tempTable As New DataTable
    Dim toggleRowColor As Boolean = False
    Dim ModelID As Long = 0

    ModelID = get_amod_id(ac_id)
    tempTable = GetMaintenanceDetailsInspectionSchedule(ModelID)

    Dim afiltered_TimeUsage As DataRow() = Nothing
    Dim afiltered_General As DataRow() = Nothing

    If Not IsNothing(tempTable) Then

      afiltered_TimeUsage = tempTable.Select("amitem_increment IN('Time','Usage')", "amitem_increment, amitem_sort, amitem_name")

      If afiltered_TimeUsage.Length > 0 Then

        htmlOut_InspectionSchedule.Append("<table id=""modelMaintenanceDetailsInspectionScheduleTable"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
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

        htmlOut_InspectionSchedule.Append("<table id=""modelMaintenanceDetailsInspectionScheduleTable2"" cellpadding=""2"" cellspacing=""2"" width=""100%"" class=""data_aircraft_grid darker_blue_border""><tr>")
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

    Build_Maintenance_Schedules = htmlOut_InspectionSchedule.ToString

  End Function
  Public Function Get_Inspection_List(ByVal AircraftID As Long) As String
    Get_Inspection_List = ""

    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim fleetinfo As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim mitem_id As String = ""

    Try

      Query = "SELECT distinct mitem_id from Maintenance_Item with (NOLOCK) "
      Query = Query & " inner join aircraft_maintenance with (NOLOCK) on aircraft_maintenance.acmaint_name = maintenance_item.mitem_name "
      Query = Query & " inner join Aircraft_Flat with (NOLOCK) on acmaint_ac_id=ac_id and acmaint_journ_id=ac_journ_id"
      Query = Query & " where mitem_active_flag='Y'"
      Query = Query & " and amod_id in ( select distinct amod_id from Aircraft_Flat with (NOLOCK) where ac_id = " & AircraftID & " and ac_journ_id = 0 )"
      'Query = Query & " and amod_id in (" & amod_id & ") "
      Query = Query & " order by mitem_id"

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = Query.ToString
      fleetinfo = SqlCommand.ExecuteReader()

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

      If fleetinfo.HasRows Then

        Do While fleetinfo.Read
          If Not IsDBNull(fleetinfo("mitem_id")) Then
            If Trim(mitem_id) = "" Then
              mitem_id = mitem_id & fleetinfo("mitem_id")
            Else
              mitem_id = mitem_id & "," & fleetinfo("mitem_id")
            End If

          End If

        Loop

      End If

      Get_Inspection_List = mitem_id

    Catch
    Finally
      fleetinfo = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function
  Public Function Get_AC_MAKE_MODEL(ByVal ac_id As Long, ByRef make_name As String, ByRef model_name As String, ByRef amod_id As Long, ByRef rest_of As String, ByRef ac_ser_no As String, Optional ByRef year_of As String = "", Optional ByRef aftt_of As String = "") As String
    Get_AC_MAKE_MODEL = ""

    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim fleetinfo As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      Query = "SELECT distinct amod_make_name, amod_model_name, amod_id,  ac_ser_no, ac_reg_no, ac_year, ac_airframe_tot_hrs "
      Query = Query & " FROM Aircraft WITH(NOLOCK)"
      Query = Query & " inner join aircraft_model WITH(NOLOCK) on amod_id = ac_amod_id "
      Query = Query & " WHERE ac_journ_id = 0 and ac_id = " & ac_id

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = Query.ToString
      fleetinfo = SqlCommand.ExecuteReader()

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

      If fleetinfo.HasRows Then

        Do While fleetinfo.Read
          make_name = fleetinfo("amod_make_name")
          model_name = fleetinfo("amod_model_name")
          amod_id = fleetinfo("amod_id")
          If Not IsDBNull(fleetinfo("ac_year")) Then
            rest_of = fleetinfo("ac_year") & " "
            year_of = fleetinfo("ac_year")
          End If

          If Not IsDBNull(fleetinfo("ac_airframe_tot_hrs")) Then
            aftt_of = fleetinfo("ac_airframe_tot_hrs")
          End If


          rest_of = rest_of & make_name & " " & model_name

          If Not IsDBNull(fleetinfo("ac_ser_no")) Then
            rest_of = rest_of & " S/N " & fleetinfo("ac_ser_no")
            ac_ser_no = fleetinfo("ac_ser_no")
          End If
 
        Loop

      End If

    Catch
    Finally
      fleetinfo = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function
  Public Sub LoadGridView()
    attention.Text = ""
    TryToAddRowBelow.Visible = True
    TryToAddRow.Visible = True
    Dim MaintenanceTable As New DataTable
    MaintenanceTable = Get_Maintenance_By_ID(AircraftID, JournalID)
    'maintenanceTableLiteral.Text = DisplayMaintenanceTable(MaintenanceTable)
    If Not IsNothing(MaintenanceTable) Then
      If MaintenanceTable.Rows.Count > 0 Then
        maintenanceInfo.DataSource = MaintenanceTable
        maintenanceInfo.DataBind()
        Me.check_auto.Visible = True
      Else
        MaintenanceTable = New DataTable
        MaintenanceTable.Columns.Add("acmaint_name")
        MaintenanceTable.Columns.Add("mitem_duration")
        MaintenanceTable.Columns.Add("acmaint_complied_date")
        MaintenanceTable.Columns.Add("acmaint_date_type")
        MaintenanceTable.Columns.Add("acmaint_complied_hrs")
        MaintenanceTable.Columns.Add("acmaint_due_hrs")
        MaintenanceTable.Columns.Add("acmaint_due_date")
        MaintenanceTable.Columns.Add("acmaint_notes")
        MaintenanceTable.Rows.Add(MaintenanceTable.NewRow)
        MaintenanceTable.AcceptChanges()
        maintenanceInfo.DataSource = MaintenanceTable
        maintenanceInfo.DataBind()
        maintenanceInfo.Rows(0).Visible = False
      End If
    End If
  End Sub
  Public Sub LoadGridView_Client()
    attention.Text = ""
    TryToAddRowBelow.Visible = True
    TryToAddRow.Visible = True
    Dim MaintenanceTable As New DataTable
    MaintenanceTable = clsGeneral.clsGeneral.Get_Maintenance_By_ID_Client(Client_AircraftID)
    'maintenanceTableLiteral.Text = DisplayMaintenanceTable(MaintenanceTable)
    If Not IsNothing(MaintenanceTable) Then
      If MaintenanceTable.Rows.Count > 0 Then
        maintenanceInfo.DataSource = MaintenanceTable
        maintenanceInfo.DataBind()
      Else
        MaintenanceTable = New DataTable
        MaintenanceTable.Columns.Add("acmaint_name")
        MaintenanceTable.Columns.Add("mitem_duration")
        MaintenanceTable.Columns.Add("acmaint_complied_date")
        MaintenanceTable.Columns.Add("acmaint_date_type")
        MaintenanceTable.Columns.Add("acmaint_complied_hrs")
        MaintenanceTable.Columns.Add("acmaint_due_hrs")
        MaintenanceTable.Columns.Add("acmaint_due_date")
        MaintenanceTable.Columns.Add("acmaint_notes")
        MaintenanceTable.Rows.Add(MaintenanceTable.NewRow)
        MaintenanceTable.AcceptChanges()
        maintenanceInfo.DataSource = MaintenanceTable
        maintenanceInfo.DataBind()
        maintenanceInfo.Rows(0).Visible = False
      End If
    End If
  End Sub


  Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles maintenanceInfo.RowDataBound

    If (e.Row.RowType = DataControlRowType.DataRow AndAlso maintenanceInfo.EditIndex = e.Row.RowIndex) Or (e.Row.RowType = DataControlRowType.Footer AndAlso maintenanceInfo.ShowFooter = True) Then
      Dim DropDownListFilled As New DropDownList
      Dim CompliedDate As New TextBox
      Dim DueDate As New TextBox
      Dim MaintenanceName As New TextBox
      Dim selectedItem As String = ""
      Dim MaintenanceDateType As New TextBox
      Dim MaintenanceByDate As New DropDownList
      Dim CompliedFormat As New Label
      Dim DueFormat As New Label
      Dim selected_index As Long = 0

      If Not IsNothing(e.Row.FindControl("acmaint_name")) Then
        DropDownListFilled = DirectCast(e.Row.FindControl("acmaint_name"), DropDownList)
      End If


      If Not IsNothing(e.Row.FindControl("acmaint_complied_date")) Then
        CompliedDate = TryCast(e.Row.FindControl("acmaint_complied_date"), TextBox)
      End If



      If Not IsNothing(e.Row.FindControl("acmaint_due_date")) Then
        DueDate = TryCast(e.Row.FindControl("acmaint_due_date"), TextBox)
      End If


      If Not IsNothing(e.Row.FindControl("acmaint_name_textbox")) Then
        MaintenanceName = TryCast(e.Row.FindControl("acmaint_name_textbox"), TextBox)
        selectedItem = TryCast(e.Row.FindControl("acmaint_name_textbox"), TextBox).Text
      End If



      If Not IsNothing(e.Row.FindControl("acmaint_date_type")) Then
        MaintenanceDateType = TryCast(e.Row.FindControl("acmaint_date_type"), TextBox)
      End If


      If Not IsNothing(e.Row.FindControl("acmaint_by_date")) Then
        MaintenanceByDate = TryCast(e.Row.FindControl("acmaint_by_date"), DropDownList)
      End If


      If Not IsNothing(e.Row.FindControl("acmaint_complied_date_format")) Then
        CompliedFormat = TryCast(e.Row.FindControl("acmaint_complied_date_format"), Label)
      End If

      If Not IsNothing(e.Row.FindControl("acmaint_due_date_format")) Then
        DueFormat = TryCast(e.Row.FindControl("acmaint_due_date_format"), Label)
      End If

      MaintenanceByDate.SelectedValue = MaintenanceDateType.Text

      If check_auto.Checked = True Then
        CompliedDate.Attributes.Add("onChange", "FigureOutNewDate(1,this.value,$(""#" & DueDate.ClientID & """),$(""#" & MaintenanceName.ClientID & """).val(),$(""#" & MaintenanceByDate.ClientID & """).val());")
        DueDate.Attributes.Add("onChange", "FigureOutNewDate(2,this.value,$(""#" & CompliedDate.ClientID & """),$(""#" & MaintenanceName.ClientID & """).val(),$(""#" & MaintenanceByDate.ClientID & """).val());")
      End If
      MaintenanceByDate.Attributes.Add("onChange", "UpdateFormat(this.value, $(""#" & CompliedFormat.ClientID & """), $(""#" & DueFormat.ClientID & """));")



      'Fill Up Dropdowns
      If Not IsNothing(MaintenanceItemTable) Then
        If MaintenanceItemTable.Rows.Count = 0 Then 'refil
          MaintenanceItemTable = Get_Maintenance_Item(0, Me.Model_Items_List_Label.Text)
        End If
        If MaintenanceItemTable.Rows.Count > 0 Then
          For Each r As DataRow In MaintenanceItemTable.Rows
            DropDownListFilled.Items.Add(New ListItem(r("mitem_name"), r("mitem_name") & "|" & r("mitem_duration")))

            If r("mitem_name") = selectedItem Then
              selectedItem = r("mitem_name") & "|" & r("mitem_duration")
            End If
          Next

          ' added msw, for client versions, go back thro 
          If Right(Trim(selectedItem), 1) = "0" Then
            For Each r As DataRow In MaintenanceItemTable.Rows
              DropDownListFilled.Items.Add(New ListItem(r("mitem_name"), r("mitem_name") & "|" & r("mitem_duration")))

              ' If Left(Trim(r("mitem_name")), 8) = Left(Trim(selectedItem), 8) Then
              If Trim(LCase(r("mitem_name") & "|" & r("mitem_duration"))) = Trim(LCase(selectedItem)) Then
                selectedItem = r("mitem_name") & "|" & r("mitem_duration")
                selected_index = DropDownListFilled.Items.Count - 1
              End If
            Next
          End If
        End If
      End If

      If selected_index > 0 Then
        DropDownListFilled.SelectedIndex = selected_index
      ElseIf selectedItem <> "" Then
        DropDownListFilled.SelectedValue = selectedItem
      End If

      DropDownListFilled.Attributes.Add("onChange", "$(""#" & MaintenanceName.ClientID & """).val(this.value);")
    End If
  End Sub

  Public Function FormattingMaintenanceDate(ByVal acmaint_date As Object, ByVal acmaint_date_type As Object)
    Dim returnString As String = ""
    If Not IsDBNull(acmaint_date) Then
      returnString = Format(acmaint_date, ReturnCalendarFormat(acmaint_date_type))
    End If
    Return returnString
  End Function
  Public Function ReturnCalendarFormat(ByVal acmaint_date_type As Object) As String
    Dim DateToFormatAs As String = "MM/dd/yyyy"

    If Not IsDBNull(acmaint_date_type) And Not String.IsNullOrEmpty(acmaint_date_type) Then
      Select Case UCase(acmaint_date_type)
        Case "D"
          DateToFormatAs = "MM/dd/yyyy"
        Case "M"
          DateToFormatAs = "MM/yyyy"
        Case "Y"
          DateToFormatAs = "yyyy"
      End Select
    End If

    Return DateToFormatAs
  End Function

  Private Sub maintenanceInfo_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles maintenanceInfo.RowCancelingEdit
    Try

      maintenanceInfo.EditIndex = -1
      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
          LoadGridView_Client()
      Else
        LoadGridView()
      End If
      maintenanceInfo.DataBind()
    Catch ex As Exception

    End Try
  End Sub
  Private Sub delete_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delete_all_maint_click.Click
    Dim AircraftID As Long = 0

    If Not IsNothing(Request.Item("acid")) Then
      If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
        AircraftID = CLng(Request.Item("acid").ToString.Trim)
      End If
    End If

    If AircraftID > 0 Then
      Me.delete_label.Visible = True
      Me.delete_no.Visible = True
      Me.delete_yes.Visible = True 
      Me.delete_all_maint_click.Visible = False
    End If


  End Sub

  Private Sub delete_no_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delete_no.Click
    Me.delete_label.Visible = False
    Me.delete_no.Visible = False
    Me.delete_yes.Visible = False
    Me.delete_all_maint_click.Visible = True
  End Sub

  Private Sub delete_yes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delete_yes.Click


    If Not IsNothing(Request.Item("acid")) Then
      If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
        AircraftID = CLng(Request.Item("acid").ToString.Trim)
      End If
    End If

    If AircraftID > 0 Then
      Call Remove_All_Aircraft_Maintenance_Items(AircraftID, 0)

      Call re_load_page()
    End If
    Me.delete_label.Visible = False
    Me.delete_no.Visible = False
    Me.delete_yes.Visible = False
    Me.delete_all_maint_click.Visible = True
  End Sub



  Private Sub autoaddclick(ByVal sender As Object, ByVal e As System.EventArgs) Handles auto_add_multiple.Click
    Dim ResponseCode As Boolean = False
    Dim acmaint_name As String = ""
    Dim acmaint_complied_date As String = ""
    Dim acmaint_complied_hrs As Long = 0
    Dim acmaint_due_date As String = ""
    Dim acmaint_due_hrs As Long = 0
    Dim acmaint_notes As String = ""
    Dim acmaint_date_type As String = "D"
    Dim acmaint_id As Long = 0
    Dim acmaint_ac_id As Long = AircraftID
    Dim acmaint_journ_id As Long = JournalID

    GetTheFieldsOut(maintenanceInfo.FooterRow, acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)


    For i = 1 To 4  
      acmaint_name = "Phase " & i & " Inspection"

      ' If String.IsNullOrEmpty(acmaint_complied_date) And String.IsNullOrEmpty(acmaint_due_date) Then
      '  attention.Text = "<p align=""center"">The Complied Date or Due Date must be filled in.</p>"
      '  Else

      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        ResponseCode = Insert_Aircraft_Maintenance_Client(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
      Else
        ResponseCode = Insert_Aircraft_Maintenance(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
      End If
      ' End If
    Next

    If ResponseCode = True Then
      maintenanceInfo.ShowFooter = False
      maintenanceInfo.EditIndex = -1
      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        LoadGridView_Client()
      Else
        LoadGridView()
      End If
      attention.Text = "<p align=""center"">Your record has been added.</p>"

      'refresh
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener && window.opener.document) {window.opener.location.href = window.opener.location.href;}", True)
    Else
      attention.Text = "<p align=""center"">An error has occurred in added.</p>"
    End If

    auto_add_multiple.Visible = False

  End Sub

  Private Sub maintenanceInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles maintenanceInfo.RowCommand

    Dim ResponseCode As Boolean = False
    Dim acmaint_name As String = ""
    Dim acmaint_complied_date As String = ""
    Dim acmaint_complied_hrs As Long = 0
    Dim acmaint_due_date As String = ""
    Dim acmaint_due_hrs As Long = 0
    Dim acmaint_notes As String = ""
    Dim acmaint_date_type As String = "D"
    Dim acmaint_id As Long = 0
    Dim acmaint_ac_id As Long = AircraftID
    Dim acmaint_journ_id As Long = JournalID



    If e.CommandName = "CancelAdd" Then
      maintenanceInfo.EditIndex = -1
      maintenanceInfo.ShowFooter = False
      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        LoadGridView_Client()
      Else
        LoadGridView()
      End If
    ElseIf e.CommandName = "Remove" Then

      If Trim(invis_maint_row.Text) <> "" Then

        GetTheFieldsOut(maintenanceInfo.Rows(invis_maint_row.Text), acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)


        If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
          ResponseCode = Remove_Aircraft_Maintenanc_Client(acmaint_id, acmaint_ac_id, acmaint_journ_id)
        Else
          ResponseCode = Remove_Aircraft_Maintenance(acmaint_id, acmaint_ac_id, acmaint_journ_id)
        End If


        If ResponseCode = True Then
          maintenanceInfo.ShowFooter = False
          maintenanceInfo.EditIndex = -1
          If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            LoadGridView_Client()
          Else
            LoadGridView()
          End If
          attention.Text = "<p align=""center"">Your record has been removed.</p>"

          'refresh
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener && window.opener.document) {window.opener.location.href = window.opener.location.href;}", True)
        Else
          attention.Text = "<p align=""center"">An error has occurred in added.</p>"
        End If
      End If

    ElseIf e.CommandName = "Insert" Then


      GetTheFieldsOut(maintenanceInfo.FooterRow, acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)

      If Trim(acmaint_name) = "Phase 1 Inspection" And acmaint_ac_id > 0 And auto_add_multiple.Visible = False Then
        attention.Text &= "<p align=""center"">Would You like to Automatically Add Phases 2-4 as well? If Yes, select the 'Auto' Button, If No, Select the Normal 'Save' Button</p>"
        auto_add_multiple.Visible = True
      Else

        If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
          ResponseCode = Insert_Aircraft_Maintenance_Client(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
        Else
          ResponseCode = Insert_Aircraft_Maintenance(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
        End If


        If ResponseCode = True Then
          maintenanceInfo.ShowFooter = False
          maintenanceInfo.EditIndex = -1
          If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            LoadGridView_Client()
          Else
            LoadGridView()
          End If
          attention.Text = "<p align=""center"">Your record has been added.</p>"

          'refresh
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener && window.opener.document) {window.opener.location.href = window.opener.location.href;}", True)
        Else
          attention.Text = "<p align=""center"">An error has occurred in added.</p>"
        End If
      End If




    End If
  End Sub



  Public Function IgnoreZero(ByVal Hrs As Object) As String
    Dim returnString As String = ""

    If Not IsDBNull(Hrs) Then
      If IsNumeric(Hrs) Then
        If Hrs > 0 Then
          returnString = Hrs.ToString
        End If
      End If
    End If

    Return returnString
  End Function
  Private Sub maintenanceInfo_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles maintenanceInfo.RowEditing

    Call edit_this_row(e.NewEditIndex)

    invis_maint_row.Text = e.NewEditIndex

  End Sub
  Private Sub edit_this_row(ByVal this_row As Integer)

    Try
      maintenanceInfo.ShowFooter = False
      maintenanceInfo.EditIndex = CInt(this_row)
      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        LoadGridView_Client()
      Else
        LoadGridView()
      End If
      maintenanceInfo.DataBind()
    Catch ex As Exception

    End Try
  End Sub

  Private Sub TryToAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TryToAddRow.Click, TryToAddRowBelow.Click

    Call re_load_page() 

  End Sub

  Public Sub re_load_page()

    maintenanceInfo.ShowFooter = True
    maintenanceInfo.EditIndex = -1
    If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
      LoadGridView_Client()
    Else
      LoadGridView()
    End If
    TryToAddRowBelow.Visible = False
    TryToAddRow.Visible = False 

  End Sub


  ''''' <summary>
  ''''' This removes the Aircraft Maintenance Record
  ''''' </summary>
  ''''' <returns></returns>
  ''''' <remarks></remarks>
  Public Function Remove_Aircraft_Maintenance(ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      If acmaint_ac_id > 0 And acmaint_id > 0 Then

        Query = "delete from aircraft_maintenance "
        Query += " where acmaint_id = @acmaint_id and "
        Query += " acmaint_ac_id = @acmaint_ac_id and "
        Query += " acmaint_journ_id = @acmaint_journ_id "

        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
        SqlConn.Open()


        Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)

        SqlCommand.Parameters.AddWithValue("@acmaint_id", acmaint_id)
        SqlCommand.Parameters.AddWithValue("@acmaint_ac_id", acmaint_ac_id)
        SqlCommand.Parameters.AddWithValue("@acmaint_journ_id", acmaint_journ_id)

        SqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        SqlCommand.Dispose()
        SqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function
  Public Function Remove_All_Aircraft_Maintenance_Items(ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      If acmaint_ac_id > 0 Then

        Query = "delete from aircraft_maintenance where acmaint_ac_id = " & acmaint_ac_id & " and acmaint_journ_id = 0 "

        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
        SqlConn.Open()


        Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)

        SqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        SqlCommand.Dispose()
        SqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function

  Public Function Remove_Aircraft_Maintenanc_Client(ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing


    Try
      If acmaint_ac_id > 0 And acmaint_id > 0 Then

        Query = "delete from client_aircraft_maintenance "
        Query += " where cliacmaint_id = @acmaint_id and "
        Query += " cliacmaint_cliac_id = " & Client_AircraftID & " and "
        Query += " cliacmaint_jetnet_ac_id = " & AircraftID & "   "


        MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
        MySqlConn.Open()


        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(Query, MySqlConn)

        MySqlCommand.Parameters.AddWithValue("@acmaint_id", acmaint_id)

        MySqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        MySqlCommand.Dispose()
        MySqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      MySqlConn.Close()
      MySqlConn.Dispose()
      MySqlConn = Nothing

    End Try
  End Function


  '''' <summary>
  '''' This updates the Aircraft Maintenance Record
  '''' </summary>
  '''' <returns></returns>
  '''' <remarks></remarks>
  Public Function Update_Aircraft_Maintenance(ByVal acmaint_name As String, ByVal acmaint_complied_date As String, ByVal acmaint_complied_hrs As String, ByVal acmaint_due_date As String, ByVal acmaint_due_hrs As String, ByVal acmaint_notes As String, ByVal acmaint_date_type As String, ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      If acmaint_ac_id > 0 And acmaint_id > 0 Then

        Query = "update aircraft_maintenance set "
        Query += " acmaint_name = @acmaint_name, "
        Query += " acmaint_complied_date = @acmaint_complied_date, "
        Query += " acmaint_complied_hrs = @acmaint_complied_hrs, "
        Query += " acmaint_due_date = @acmaint_due_date, "
        Query += " acmaint_due_hrs = @acmaint_due_hrs, "
        Query += " acmaint_notes = @acmaint_notes, "
        Query += " acmaint_date_type = @acmaint_date_type "

        Query += " where acmaint_id = @acmaint_id and "
        Query += " acmaint_ac_id = @acmaint_ac_id and "
        Query += " acmaint_journ_id = @acmaint_journ_id "

        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
        SqlConn.Open()


        Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)

        SqlCommand.Parameters.AddWithValue("@acmaint_name", Left(acmaint_name, 100))

        If String.IsNullOrEmpty(acmaint_complied_date) Then
          SqlCommand.Parameters.AddWithValue("@acmaint_complied_date", DBNull.Value)
        Else
          SqlCommand.Parameters.AddWithValue("@acmaint_complied_date", acmaint_complied_date)
        End If

        SqlCommand.Parameters.AddWithValue("@acmaint_complied_hrs", acmaint_complied_hrs)

        If String.IsNullOrEmpty(acmaint_due_date) Then
          SqlCommand.Parameters.AddWithValue("@acmaint_due_date", DBNull.Value)
        Else
          SqlCommand.Parameters.AddWithValue("@acmaint_due_date", acmaint_due_date)
        End If


        SqlCommand.Parameters.AddWithValue("@acmaint_due_hrs", acmaint_due_hrs)
        SqlCommand.Parameters.AddWithValue("@acmaint_notes", Left(acmaint_notes, 300))
        SqlCommand.Parameters.AddWithValue("@acmaint_date_type", Left(acmaint_date_type, 1))
        SqlCommand.Parameters.AddWithValue("@acmaint_id", acmaint_id)
        SqlCommand.Parameters.AddWithValue("@acmaint_ac_id", acmaint_ac_id)
        SqlCommand.Parameters.AddWithValue("@acmaint_journ_id", acmaint_journ_id)

        SqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        SqlCommand.Dispose()
        SqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function
  Public Function Update_Aircraft_Maintenance_Client(ByVal acmaint_name As String, ByVal acmaint_complied_date As String, ByVal acmaint_complied_hrs As String, ByVal acmaint_due_date As String, ByVal acmaint_due_hrs As String, ByVal acmaint_notes As String, ByVal acmaint_date_type As String, ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Try
      If acmaint_ac_id > 0 And acmaint_id > 0 Then

        Query = "update client_aircraft_maintenance set "
        Query += " cliacmaint_name = @acmaint_name, "
        Query += " cliacmaint_complied_date = @acmaint_complied_date, "
        Query += " cliacmaint_complied_hrs = @acmaint_complied_hrs, "
        Query += " cliacmaint_due_date = @acmaint_due_date, "
        Query += " cliacmaint_due_hrs = @acmaint_due_hrs, "
        Query += " cliacmaint_notes = @acmaint_notes, "
        Query += " cliacmaint_date_type = @acmaint_date_type "

        Query += " where cliacmaint_id = @acmaint_id and "
        Query += " cliacmaint_cliac_id = @acmaint_ac_id "

        MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
        MySqlConn.Open()


        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(Query, MySqlConn)

        MySqlCommand.Parameters.AddWithValue("@acmaint_name", Left(acmaint_name, 100))

        If Trim(acmaint_complied_date) <> "" Then
          acmaint_complied_date = Year(acmaint_complied_date) & "-" & Month(acmaint_complied_date) & "-" & Day(acmaint_complied_date)
        Else
          acmaint_complied_date = "NULL"
        End If

        MySqlCommand.Parameters.AddWithValue("@acmaint_complied_date", acmaint_complied_date)
        MySqlCommand.Parameters.AddWithValue("@acmaint_complied_hrs", acmaint_complied_hrs)
        If Trim(acmaint_due_date) <> "" Then
          acmaint_due_date = Year(acmaint_due_date) & "-" & Month(acmaint_due_date) & "-" & Day(acmaint_due_date)
        Else
          acmaint_due_date = "NULL"
        End If

        If Trim(acmaint_due_date) = "NULL" Then
          MySqlCommand.Parameters.AddWithValue("@acmaint_due_date", System.DBNull.Value)
        Else
          MySqlCommand.Parameters.AddWithValue("@acmaint_due_date", acmaint_due_date)
        End If 

        MySqlCommand.Parameters.AddWithValue("@acmaint_due_hrs", acmaint_due_hrs)
        MySqlCommand.Parameters.AddWithValue("@acmaint_notes", Left(acmaint_notes, 300))
        MySqlCommand.Parameters.AddWithValue("@acmaint_date_type", Left(acmaint_date_type, 1))
        MySqlCommand.Parameters.AddWithValue("@acmaint_id", acmaint_id)
        MySqlCommand.Parameters.AddWithValue("@acmaint_ac_id", acmaint_ac_id)

        MySqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        MySqlCommand.Dispose()
        MySqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      MySqlConn.Close()
      MySqlConn.Dispose()
      MySqlConn = Nothing

    End Try
  End Function
  '''' <summary>
  '''' This updates the Aircraft Maintenance Record
  '''' </summary>
  '''' <returns></returns>
  '''' <remarks></remarks> 
  Public Function Insert_Aircraft_Maintenance(ByVal acmaint_name As String, ByVal acmaint_complied_date As String, ByVal acmaint_complied_hrs As String, ByVal acmaint_due_date As String, ByVal acmaint_due_hrs As String, ByVal acmaint_notes As String, ByVal acmaint_date_type As String, ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If acmaint_ac_id > 0 Then
        QueryFields = "insert into aircraft_maintenance(acmaint_name, "
        QueryValues = " values (@acmaint_name, "

        QueryFields += " acmaint_complied_date, "
        QueryValues += " @acmaint_complied_date, "

        QueryFields += " acmaint_complied_hrs, "
        QueryValues += " @acmaint_complied_hrs, "

        QueryFields += " acmaint_due_date, "
        QueryValues += " @acmaint_due_date, "

        QueryFields += " acmaint_due_hrs, "
        QueryValues += " @acmaint_due_hrs, "

        QueryFields += " acmaint_notes, "
        QueryValues += " @acmaint_notes, "

        QueryFields += " acmaint_date_type, "
        QueryValues += " @acmaint_date_type, "

        QueryFields += " acmaint_ac_id, "
        QueryValues += " @acmaint_ac_id, "

        QueryFields += " acmaint_journ_id) "
        QueryValues += " @acmaint_journ_id) "

        Query = QueryFields & QueryValues

        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
        SqlConn.Open()


        Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)

        SqlCommand.Parameters.AddWithValue("@acmaint_name", Left(acmaint_name, 100))

        If Not String.IsNullOrEmpty(acmaint_complied_date) Then
          SqlCommand.Parameters.AddWithValue("@acmaint_complied_date", acmaint_complied_date)
        Else
          SqlCommand.Parameters.AddWithValue("@acmaint_complied_date", DBNull.Value)
        End If

        SqlCommand.Parameters.AddWithValue("@acmaint_complied_hrs", acmaint_complied_hrs)

        If Not String.IsNullOrEmpty(acmaint_due_date) Then
          SqlCommand.Parameters.AddWithValue("@acmaint_due_date", acmaint_due_date)
        Else
          SqlCommand.Parameters.AddWithValue("@acmaint_due_date", DBNull.Value)
        End If

        SqlCommand.Parameters.AddWithValue("@acmaint_due_hrs", acmaint_due_hrs)
        SqlCommand.Parameters.AddWithValue("@acmaint_notes", Left(acmaint_notes, 300))
        SqlCommand.Parameters.AddWithValue("@acmaint_date_type", Left(acmaint_date_type, 1))
        SqlCommand.Parameters.AddWithValue("@acmaint_ac_id", acmaint_ac_id)
        SqlCommand.Parameters.AddWithValue("@acmaint_journ_id", acmaint_journ_id)

        SqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        SqlCommand.Dispose()
        SqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function
  Public Function Insert_Aircraft_Maintenance_Client(ByVal acmaint_name As String, ByVal acmaint_complied_date As String, ByVal acmaint_complied_hrs As String, ByVal acmaint_due_date As String, ByVal acmaint_due_hrs As String, ByVal acmaint_notes As String, ByVal acmaint_date_type As String, ByVal acmaint_id As Long, ByVal acmaint_ac_id As Long, ByVal acmaint_journ_id As Long) As Boolean
    Dim Query As String = ""
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Try
      If acmaint_ac_id > 0 Then
        QueryFields = "insert into client_aircraft_maintenance(cliacmaint_name, "
        QueryValues = " values (@acmaint_name, "


        QueryFields += " cliacmaint_complied_date, "
        QueryValues += " @acmaint_complied_date, "

        QueryFields += " cliacmaint_complied_hrs, "
        QueryValues += " @acmaint_complied_hrs, "

        QueryFields += " cliacmaint_due_date, "
        QueryValues += " @acmaint_due_date, "

        QueryFields += " cliacmaint_due_hrs, "
        QueryValues += " @acmaint_due_hrs, "

        QueryFields += " cliacmaint_notes, "
        QueryValues += " @acmaint_notes, "

        QueryFields += " cliacmaint_date_type, "
        QueryValues += " @acmaint_date_type, "

        QueryFields += " cliacmaint_jetnet_ac_id, "
        QueryValues += " " & AircraftID & ", "

        QueryFields += " cliacmaint_cliac_id "
        QueryValues += " " & Client_AircraftID & " "

        QueryFields += " ) "
        QueryValues += " ) "

        Query = QueryFields & QueryValues

        MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
        MySqlConn.Open()






        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(Query, MySqlConn)

        MySqlCommand.Parameters.AddWithValue("@acmaint_name", Left(acmaint_name, 100))

        If Trim(acmaint_complied_date) <> "" Then
          acmaint_complied_date = Year(acmaint_complied_date) & "-" & Month(acmaint_complied_date) & "-" & Day(acmaint_complied_date)
        End If

        If Not String.IsNullOrEmpty(acmaint_complied_date) Then
          MySqlCommand.Parameters.AddWithValue("@acmaint_complied_date", acmaint_complied_date)
        Else
          MySqlCommand.Parameters.AddWithValue("@acmaint_complied_date", DBNull.Value)
        End If

        MySqlCommand.Parameters.AddWithValue("@acmaint_complied_hrs", acmaint_complied_hrs)

        If Trim(acmaint_due_date) <> "" Then
          acmaint_due_date = Year(acmaint_due_date) & "-" & Month(acmaint_due_date) & "-" & Day(acmaint_due_date)
        End If

        If Not String.IsNullOrEmpty(acmaint_due_date) Then
          MySqlCommand.Parameters.AddWithValue("@acmaint_due_date", acmaint_due_date)
        Else
          MySqlCommand.Parameters.AddWithValue("@acmaint_due_date", DBNull.Value)
        End If

        MySqlCommand.Parameters.AddWithValue("@acmaint_due_hrs", acmaint_due_hrs)
        MySqlCommand.Parameters.AddWithValue("@acmaint_notes", Left(acmaint_notes, 300))
        MySqlCommand.Parameters.AddWithValue("@acmaint_date_type", Left(acmaint_date_type, 1))
        MySqlCommand.Parameters.AddWithValue("@acmaint_ac_id", acmaint_ac_id)
        MySqlCommand.Parameters.AddWithValue("@acmaint_journ_id", acmaint_journ_id)

        MySqlCommand.ExecuteNonQuery()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

        MySqlCommand.Dispose()
        MySqlCommand = Nothing

        Return True
      End If
    Catch ex As Exception
      Return False
    Finally
      'kill everything
      MySqlConn.Close()
      MySqlConn.Dispose()
      MySqlConn = Nothing

    End Try
  End Function

  Public Function Get_Maintenance_Item(ByVal mitem_id As Long, ByVal Model_Items_List As String) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
      SqlConn.Open()

      sql = "SELECT mitem_id, mitem_name, mitem_type, mitem_duration from Maintenance_Item WITH(NOLOCK) WHERE mitem_active_flag = 'Y' "

      If mitem_id > 0 Then
        sql += " and mitem_id = @mitem_id "
      End If

      If Trim(Model_Items_List) <> "" Then
        sql += " and mitem_id in (" & Model_Items_List & ") "
      End If
      sql += " order by mitem_sort asc  "
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      If mitem_id > 0 Then
        SqlCommand.Parameters.AddWithValue("mitem_id", mitem_id)
      End If


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try

      Get_Maintenance_Item = TempTable

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Get_Maintenance_Item = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function

  Public Function Get_Maintenance_By_ID(ByVal acID As Long, ByVal journID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
      SqlConn.Open()

      sql = "SELECT *, mitem_duration from aircraft_maintenance with (NOLOCK)  "
      sql = sql & " INNER JOIN Maintenance_Item with (NOLOCK) ON aircraft_maintenance.acmaint_name = maintenance_item.mitem_name"
      sql = sql & " WHERE "
      sql = sql & " (aircraft_maintenance.acmaint_ac_id = @acID)  AND (aircraft_maintenance.acmaint_journ_id = @journID) "
      sql = sql & " order by acmaint_complied_date asc "


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

      SqlCommand.Parameters.AddWithValue("acID", acID)
      SqlCommand.Parameters.AddWithValue("journID", journID)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try

      Get_Maintenance_By_ID = TempTable

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Get_Maintenance_By_ID = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function
  Private Sub GetTheFieldsOut(ByRef SelectedRow As GridViewRow, ByRef acmaint_name As String, ByRef acmaint_complied_date As String, ByRef acmaint_complied_hrs As Long, ByRef acmaint_due_date As String, ByRef acmaint_due_hrs As Long, ByRef acmaint_notes As String, ByRef acmaint_date_type As String, ByRef acmaint_id As Long, ByRef acmaint_ac_id As Long, ByRef acmaint_journ_id As Long)
    'ID #
    If Not IsNothing(SelectedRow.FindControl("acmaint_id")) Then
      acmaint_id = CLng(TryCast(SelectedRow.FindControl("acmaint_id"), Label).Text)
    End If

    'Name
    If Not IsNothing(SelectedRow.FindControl("acmaint_name")) Then
      acmaint_name = TryCast(SelectedRow.FindControl("acmaint_name"), DropDownList).SelectedItem.Text
    End If

    'Complied Date
    If Not IsNothing(SelectedRow.FindControl("acmaint_complied_date")) Then
      acmaint_complied_date = TryCast(SelectedRow.FindControl("acmaint_complied_date"), TextBox).Text
    End If

    'Complied Hrs
    If Not IsNothing(SelectedRow.FindControl("acmaint_complied_hrs")) Then
      If IsNumeric(TryCast(SelectedRow.FindControl("acmaint_complied_hrs"), TextBox).Text) Then
        acmaint_complied_hrs = TryCast(SelectedRow.FindControl("acmaint_complied_hrs"), TextBox).Text
      End If
    End If

    'Due Date
    If Not IsNothing(SelectedRow.FindControl("acmaint_due_date")) Then
      acmaint_due_date = TryCast(SelectedRow.FindControl("acmaint_due_date"), TextBox).Text
    End If

    'Due Hrs
    If Not IsNothing(SelectedRow.FindControl("acmaint_due_hrs")) Then
      If IsNumeric(TryCast(SelectedRow.FindControl("acmaint_due_hrs"), TextBox).Text) Then
        acmaint_due_hrs = TryCast(SelectedRow.FindControl("acmaint_due_hrs"), TextBox).Text
      End If
    End If

    'Notes
    If Not IsNothing(SelectedRow.FindControl("acmaint_notes")) Then
      acmaint_notes = TryCast(SelectedRow.FindControl("acmaint_notes"), TextBox).Text
    End If

    'Date Type
    If Not IsNothing(SelectedRow.FindControl("acmaint_by_date")) Then
      acmaint_date_type = TryCast(SelectedRow.FindControl("acmaint_by_date"), DropDownList).SelectedValue
    End If

    'We need to add some valid info to the cases of by month/by year
    Select Case acmaint_date_type
      Case "M"
        Dim SplitMonth As String()
        Dim SplitMonthComplied As String()

        If Not String.IsNullOrEmpty(acmaint_due_date) Then
          SplitMonth = Split(acmaint_due_date, "/")
          If UBound(SplitMonth) = 1 Then
            acmaint_due_date = SplitMonth(0) & "/01/" & SplitMonth(1)
          End If
        End If
        If Not String.IsNullOrEmpty(acmaint_complied_date) Then
          SplitMonthComplied = Split(acmaint_complied_date, "/")
          If UBound(SplitMonthComplied) = 1 Then
            acmaint_complied_date = SplitMonthComplied(0) & "/01/" & SplitMonthComplied(1)
          End If
        End If

      Case "Y"
        If Not String.IsNullOrEmpty(acmaint_due_date) Then
          acmaint_due_date = "01/01/" & acmaint_due_date
        End If
        If Not String.IsNullOrEmpty(acmaint_complied_date) Then
          acmaint_complied_date = "01/01/" & acmaint_complied_date
        End If
    End Select

  End Sub
  Private Sub maintenanceInfo_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles maintenanceInfo.RowUpdating
    Dim acmaint_name As String = ""
    Dim acmaint_complied_date As String = ""
    Dim acmaint_complied_hrs As Long = 0
    Dim acmaint_due_date As String = ""
    Dim acmaint_due_hrs As Long = 0
    Dim acmaint_notes As String = ""
    Dim acmaint_date_type As String = "D"
    Dim acmaint_id As Long = 0
    Dim acmaint_ac_id As Long = AircraftID
    Dim acmaint_journ_id As Long = JournalID
    Dim ResponseCode As Boolean = False


    GetTheFieldsOut(maintenanceInfo.Rows(e.RowIndex), acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)

    If Client_AircraftID > 0 Then
      acmaint_ac_id = Client_AircraftID
      ResponseCode = Update_Aircraft_Maintenance_Client(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
    Else
      ResponseCode = Update_Aircraft_Maintenance(acmaint_name, acmaint_complied_date, acmaint_complied_hrs, acmaint_due_date, acmaint_due_hrs, acmaint_notes, acmaint_date_type, acmaint_id, acmaint_ac_id, acmaint_journ_id)
    End If

    If ResponseCode = True Then
      maintenanceInfo.ShowFooter = False
      maintenanceInfo.EditIndex = -1
      If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        LoadGridView_Client()
      Else
        LoadGridView()
      End If
      attention.Text = "<p align=""center"">Your record has been updated.</p>"
    Else
      attention.Text = "<p align=""center"">An error has occurred in updating.</p>"
    End If

  End Sub
End Class