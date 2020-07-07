Partial Public Class DisplayYachtDetail
  Inherits System.Web.UI.Page

  Public yacht_id As Long = 0
  Public journalID As Long = 0
  Public currentRecord As Long = 0

  'This is either going to stay here, or become a session variable, but either way, this is gearing up to be
  'basically a toggle variable that will disable, change or remove some of the links to various items on this page 
  'depending on where they're coming from.
  Private CRMView As Boolean = False
  Private securityTokenLocal As String = ""

  Private bExtraJFWAFW As Boolean = False
  Private bFromJFWAFW As Boolean = False
  Private bFromView As Boolean = False
  Private bShowReminder As Boolean = False
  Private bShowNote As Boolean = False

  Private Aircraft_Display_String As String = ""

  Private SqlJournalReader As System.Data.SqlClient.SqlDataReader = Nothing
  Private SqlAircraftReader As System.Data.SqlClient.SqlDataReader = Nothing

  'Public dsAircraftBrowse As DataSet = Nothing
  Public dsYachtBrowse As New DataTable
  Public SqlCommand As New System.Data.SqlClient.SqlCommand
  Public SqlConnection As New System.Data.SqlClient.SqlConnection
  Public SqlConnection2 As New System.Data.SqlClient.SqlConnection
  Public SqlCommand_inner As New System.Data.SqlClient.SqlCommand
  Public interior_redone As String = ""
  Public exterior_redone As String = ""
  Dim helipad_string As String = ""
  Public build_column_number As Integer = 1
  Public is_on_machine_for_testing As Boolean = True
  Public pictures_directory As String = ""
  Public engine_info As String = ""
  Public aclsData_Temp As New clsData_Manager_SQL
  Dim cSingleSpace As String = " "
  Dim cHTMLnbsp As String = "&nbsp;"
  Dim cMultiDelim = ", "
  Dim cDot = "."
  Dim cHyphen = "-"
  Dim cCommaDelim = ","
  Dim cstext1 As String = ""
  Dim cstext2 As String = ""
  Dim DisplayNotes As Boolean = False
  Dim yacht_journal_date As String = ""
  Dim ValidatePermissions As Boolean = False
  Dim interior_extra As String = ""

  Private Sub DisplayYachtDetail_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    'Setting up the Yacht ID on initialization.
    If Not IsNothing(Request.Item("yid")) Then
      If Not String.IsNullOrEmpty(Request.Item("yid").ToString) Then
        yacht_id = CLng(Request.Item("yid").ToString.Trim)
      End If
    End If

    If Trim(Request("homebase")) = "Y" Then
      ValidatePermissions = True
    ElseIf Session.Item("localSubscription").crmYacht_Flag = True Then
      ValidatePermissions = True
    End If

    If Not Page.ClientScript.IsClientScriptBlockRegistered("Toggle") Then
      Dim ToggleChangedScript As StringBuilder = New StringBuilder()

      ToggleChangedScript.Append(vbCrLf & " function ToggleButtons(class_name) {")
      ToggleChangedScript.Append(vbCrLf & " if (document.getElementById(""prev_button_slide"") != null) {")
      ToggleChangedScript.Append(vbCrLf & " document.getElementById(""prev_button_slide"").className = class_name;")
      ToggleChangedScript.Append(vbCrLf & " }")
      ToggleChangedScript.Append(vbCrLf & " if (document.getElementById(""next_button_slide"") != null) {")
      ToggleChangedScript.Append(vbCrLf & " document.getElementById(""next_button_slide"").className = class_name;")
      ToggleChangedScript.Append(vbCrLf & " }")
      ToggleChangedScript.Append(vbCrLf & " }")
      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Toggle", ToggleChangedScript.ToString, True)
    End If

    Build_Dynamic_Folder_Table()
  End Sub

  ''' <summary>
  ''' This function is running to build the dynamic folder list to allow adding to static folders.
  ''' It's built dynamically in page init
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub Build_Dynamic_Folder_Table()
    'Dim FoldersTable As New DataTable
    Dim ContainerTable As New Table
    Dim TR As New TableRow
    Dim TDHold As New TableCell
    Dim SubmitButton As New LinkButton


    ContainerTable = DisplayFunctions.CreateStaticFoldersTable(0, 0, journalID, 0, 0, aclsData_Temp, yacht_id)
    TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)

    SubmitButton.Text = "Save Folders"
    SubmitButton.ID = "SaveStaticFoldersButton"
    AddHandler SubmitButton.Click, AddressOf SaveStaticFolders

    TDHold.Controls.Add(SubmitButton)
    TR.Controls.Add(TDHold)

    ContainerTable.Controls.Add(TR)

    folders_label.Controls.Clear()
    folders_label.Controls.Add(ContainerTable)

    folders_update_panel.Update()
  End Sub

  ''' <summary>
  ''' This function allows saving of static folders.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub SaveStaticFolders()
    folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, aclsData_Temp, 0, 0, 0, 0, 0, yacht_id)
    folders_update_panel.Update()
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'Since we're not in a master page anymore, I went ahead
    'and added this catch. This makes sure you're logged in
    Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)


    'so that no code executes without proper session.

    If Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y" Then
      Response.Redirect("Default.aspx", False)
    ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
      ' if we arent on homebase.com, but have passed homebasee, then bad
      Response.Redirect("Default.aspx", False)
    Else
      aclsData_Temp = New clsData_Manager_SQL


      If Trim(Request("homebase")) = "Y" Then

        If useBackupSQL Then
          aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
        Else
          aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
        End If
      Else
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
      End If

      If Not IsNothing(Request.Item("jid")) Then
        If Not String.IsNullOrEmpty(Request.Item("jid").ToString) Then
          journalID = CLng(Request.Item("jid").ToString.Trim)
        End If
      End If


      'Set Export Links:
      single_spec_link.Text = "<a href='#' onclick=""javascript:load('PDF_Creator.aspx?area=yacht&r_id=39&yacht_id=" & yacht_id & "&jid=" & journalID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Single Spec</a>"
      single_spec_link.Text = ""
      Me.single_spec_link.Visible = False

      full_spec_link.Text = "<a href='#' onclick=""javascript:load('PDF_Creator.aspx?area=yacht&r_id=43&yacht_id=" & yacht_id & "&jid=" & journalID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Full Spec</a>"

      condensed_spec_link.Text = "<a href='#' onclick=""javascript:load('PDF_Creator.aspx?area=yacht&r_id=40&yacht_id=" & yacht_id & "&jid=" & journalID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Condensed Spec</a>"
      condensed_spec_link.Text = ""
      Me.condensed_spec_link.Visible = False

      'Set Cookies
      clsGeneral.clsGeneral.Recent_Cookies("yachts", yacht_id, "JETNET")


      Master.SetPageTitle(clsGeneral.clsGeneral.Set_Page_Title("Yacht Details Page")) 'This common function will return the header and set it, just pass what you want the page to be called.


      If Not IsNothing(Request.Item("securityToken")) Then
        If Not String.IsNullOrEmpty(Request.Item("securityToken").ToString) Then
          securityTokenLocal = Request.Item("securityToken").Trim
        End If
      End If




      If Not IsNothing(Request.Item("fromView")) Then
        If Not String.IsNullOrEmpty(Request.Item("fromView").ToString) Then
          If Request.Item("fromView").ToString.Trim.ToUpper = "Y" Then
            bFromView = True
          Else
            bFromView = False
          End If
        End If
      End If

      If Not IsNothing(Request.Item("ShowNotes")) Then
        If Not String.IsNullOrEmpty(Request.Item("ShowNotes").ToString) Then
          If Request.Item("ShowNotes").ToString.Trim.ToUpper = "Y" Then
            Session.Item("localSubscription").evoShowNotes = True
          Else
            Session.Item("localSubscription").evoShowNotes = False
          End If
        End If
      End If

      If Not IsNothing(Request.Item("ShowReminder")) Then
        If Not String.IsNullOrEmpty(Request.Item("ShowReminder").ToString) Then
          If Request.Item("ShowReminder").ToString.Trim.ToUpper = "Y" Then
            Session.Item("localSubscription").evoShowReminders = True
          Else
            Session.Item("localSubscription").evoShowReminders = False
          End If
        End If
      End If

      ' bShowReminder = Session.Item("localSubscription").evoShowReminders
      '  bShowNote = Session.Item("localSubscription").evoShowNotes

      If String.IsNullOrEmpty(securityTokenLocal) Then
        ' must be coming from EVO pick up subscription values
        If Not IsNothing(Request.Item("sub")) Then
          If Not String.IsNullOrEmpty(Request.Item("sub").ToString) Then
            Session.Item("localSubscription").evoSubID = CLng(Request.Item("sub").ToString.Trim)
          End If
        End If

        If Not IsNothing(Request.Item("log")) Then
          If Not String.IsNullOrEmpty(Request.Item("log").ToString) Then
            Session.Item("localSubscription").evoUserID = Request.Item("log").ToString.Trim
          End If
        End If

        If Not IsNothing(Request.Item("seq")) Then
          If Not String.IsNullOrEmpty(Request.Item("seq").ToString) Then
            Session.Item("localSubscription").evoSeqNo = CInt(Request.Item("seq").ToString.Trim)
          End If
        End If

        If Not IsNothing(Request.Item("ExtraJFWAFW")) Then
          If Not String.IsNullOrEmpty(Request.Item("ExtraJFWAFW").ToString) Then
            bExtraJFWAFW = CBool(Request.Item("ExtraJFWAFW").ToString.Trim)
          End If
        End If

        'If Session.Item("localSubscription").evoSubID = 0 And _
        '   String.IsNullOrEmpty(Session.Item("localSubscription").evoUserID) And _
        '   Session.Item("localSubscription").evoSeqNo = 0 Then
        '    Response.Write("Error in Request(EVO) : No Subscription Info Entered")
        '    Response.End()
        'End If

      Else
        ' must be coming from CRM pick up security token value
        'At least for right now
        CRMView = True
        Try

          If Not String.IsNullOrEmpty(securityTokenLocal.ToString) Then
            'This is where we're going to change the CRM Validatio n a little bit.
            'First of all, we really need to validate the GUID.
            'This also sets the tier type and the product types
            If Not aclsData_Temp.CRM_GUID_VALIDATION(securityTokenLocal.ToString, Me.Application) Then
              ' invalid user display error message
              Response.Write("Unauthorized Page Access: Access Denied")
              Response.End()
            End If
          Else
            Response.Write("Unauthorized Page Access: Request Denied")
            Response.End()
          End If


          Session.Item("localSubscription").evoUserDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Session.Item("localSubscription").evoEnableNotesFlag = False

        Catch ex As Exception

          Response.Write("Error in Request(CRM) : Request Denied | " + ex.Message.Trim)
          Response.End()

        End Try

      End If

      Dim fSubins_platform_os As String = aclsData_Temp.parseUserAgentString(HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT"))

      If String.IsNullOrEmpty(securityTokenLocal) And (Not Session.Item("loadedSubscription")) Then

        Dim sErrorString As String = ""

        'If Not loadUserSubscription(Me.Application, Me.Session, sErrorString) Then
        'Response.Write("Error in load evo subscription : " + sErrorString.Trim)
        '    Response.End()
        ' Else
        Session.Item("loadedSubscription") = True
        ' End If
        '
      End If 'String.IsNullOrEmpty(securityTokenLocal) Then

      If Not IsPostBack Then

        If journalID = 0 Then
          'We need to figure out if the yachts are supposed to display notes:
          If (Session.Item("localSubscription").crmCloudNotes_Flag) Then 'If either Server Side Notes or Cloud Notes are on.
            If (Not String.IsNullOrEmpty(Session.Item("jetnetServerNotesDatabase"))) Or (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localSubscription").crmCloudNotesDBName)) Then 'If either the server note db string, or the cloud note db name isn't empty
              DisplayNotes = True
            End If
          End If

          If DisplayNotes = True Then
            Reminders.Visible = True
            Notes.Visible = True
            closeNotes.Visible = True
            DisplayFunctions.DisplayLocalItems(aclsData_Temp, 0, 0, yacht_id, notes_label, action_label, False, False, True)
            notes_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, 0, yacht_id, True, "&n=1", "Add New Note") & "</p>"
            action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, 0, yacht_id, True, "", "Add New Action") & "</p>"
            view_notes.Visible = True

          Else
            view_notes.Visible = False
            Reminders.Visible = False
            Notes.Visible = False
          End If
        Else
          Me.notes_panel.Visible = False
          Me.action_panel.Visible = False
        End If


        If journalID > 0 Then
          Me.view_yacht_events.Visible = False
          Me.view_analytics.Visible = False
        End If


        Call build_yacht_page(yacht_id, journalID)

      End If

      '   Me.page_body.Style.Add("background-image", "http://jetnet12/images/background/" & find_background_image())  ' todo


      '  Me.page_body.Attributes("ImageURL") = "http://jetnet12/pictures/yacht_bg/" & find_background_image()
      ' Page.Master.FindControl("body").
      '  Page.Master.FindControl("body").attributes["ImageURL"] = "http://jetnet12/pictures/yacht_bg/" & find_background_image()
    End If

  End Sub

  Public Function build_yacht_page(ByVal yacht_id As Long, ByVal journalID As Long)
    build_yacht_page = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim pic_reference As String = ""
    Dim temp_details As String = ""
    Dim temp_string As String = ""
    Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

    Try



      '  Select Case Application.Item("webHostObject").evoWebHostType
      '      Case eWebSiteTypes.LOCAL
      '   SqlConnection.ConnectionString = My.Settings.TEST_INHOUSE_MSSQL
      '       Case Else
      '   SqlConnection.ConnectionString = My.Settings.DEFAULT_LIVE_MSSQL
      '   End Select


      ' TEMP HOLD  

      'If InStr(Server.MapPath(""), "C:\inetpub\wwwroot\Evolution\JetnetWeb", CompareMethod.Text) > 0 Then
      '    ' SqlConnection.ConnectionString = My.Settings.TEST_INHOUSE_MSSQL
      '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"

      'ElseIf InStr(Server.MapPath(""), "jetnet12", CompareMethod.Text) > 0 Then
      '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"
      'Else
      '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"
      'End If
      If Trim(Request("homebase")) = "Y" Then
        If useBackupSQL Then
          SqlConnection2.ConnectionString = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=crmexport;Password=d4gpt9f8"
          SqlConnection.ConnectionString = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=crmexport;Password=d4gpt9f8"
        Else
          SqlConnection2.ConnectionString = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=evolution;Password=vbs73az8"
          SqlConnection.ConnectionString = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=evolution;Password=vbs73az8"
        End If
        Session.Item("jetnetClientDatabase") = SqlConnection2.ConnectionString
      Else
        SqlConnection2.ConnectionString = Session.Item("jetnetClientDatabase")
        SqlConnection.ConnectionString = Session.Item("jetnetClientDatabase")
      End If

      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("Before Conn Opens: " & SqlConnection.ConnectionString & "<br>")
      End If

      SqlConnection.Open()
      SqlConnection2.Open()

      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("Conn Opened: " & SqlConnection.ConnectionString & "<br>")
      End If

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000


      SqlCommand_inner.Connection = SqlConnection2
      SqlCommand_inner.CommandTimeout = 1000



      aircraft_information_label.Text = DisplayYachrSpecifications(yacht_id, journalID)

      'Let's set up the browse records bar:
      If Not IsNothing(Session.Item("Yacht_Master")) Then
        dsYachtBrowse = CType(Session.Item("Yacht_Master"), DataTable)
        UpdateBrowseButtons(dsYachtBrowse)
      End If



      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("DisplayYachrSpecifications Completed<br>")
      End If

      temp_string = DisplayYacht_Engines(yacht_id, engine_info, journalID)
      If Trim(temp_string) <> "" Then
        engine_tab_label.Text = temp_string
        Me.engine_tab_container.Visible = True
      Else
        Me.engine_tab_container.Visible = False
      End If

      temp_details = DisplayYachrDetails(yacht_id, "power", journalID)
      If Trim(temp_details) <> "" Then
        engine_tab_label.Text = engine_tab_label.Text & "<table cellpadding='0' cellspacing='0' width='100%'>" & temp_details
        engine_tab_container.Visible = True
      End If



      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("DisplayYacht_Engines Completed<br>")
      End If

      temp_string = get_yacht_compliance_certs(yacht_id, journalID)
      If Trim(temp_string) <> "" Then
        compliance_label.Text = temp_string
        Me.compliance_cert.Visible = True
      Else
        Me.compliance_cert.Visible = False
      End If

      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("get_yacht_compliance_certs Completed<br>")
      End If



      maint_label.Text = "maint_label"
      temp_details = "" 
      temp_details = DisplayYachrDetails(yacht_id, "interior", journalID)



      If Trim(temp_details) <> "" Then
        interior_tab_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & interior_extra & interior_redone & temp_details
        interior_tab_container.Visible = True
      ElseIf Trim(interior_extra) <> "" Then
        interior_tab_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & interior_extra & "</table>"
      Else
        If Trim(interior_redone) <> "" Then
          interior_tab_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & interior_redone & "</table>"
          interior_tab_container.Visible = True
        Else
          interior_tab_container.Visible = False
        End If
      End If

      temp_details = DisplayYachrDetails(yacht_id, "exterior", journalID)
      If Trim(temp_details) <> "" Then
        exterior_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & exterior_redone & temp_details
        exterior_tab_container.Visible = True
      Else
        If Trim(exterior_redone) <> "" Then
          exterior_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & exterior_redone & "</table>"
          exterior_tab_container.Visible = True
        Else
          exterior_tab_container.Visible = False
        End If
      End If

      temp_details = DisplayYachrDetails(yacht_id, "equipment", journalID)
      If Trim(temp_details) <> "" Then
        equip_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & DisplayYachrDetails(yacht_id, "equipment", journalID)
        equipment_tab.Visible = True
      Else
        equipment_tab.Visible = False
      End If

      temp_details = DisplayYachrDetails(yacht_id, "maintenance", journalID)
      If Trim(temp_details) <> "" Then
        maint_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & temp_details
        maintenance_tab_container.Visible = True
      Else
        maintenance_tab_container.Visible = False
      End If

      temp_details = DisplayYachrDetails(yacht_id, "bridge", journalID)
      If Trim(temp_details) <> "" Then
        bridge_tab_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & temp_details
        bridge_tab_container.Visible = True
      Else
        bridge_tab_container.Visible = False
      End If


      temp_details = DisplayYachrDetails(yacht_id, "systems", journalID)
      If Trim(temp_details) <> "" Then
        systems_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & temp_details
        systems_tab_container.Visible = True
      Else
        systems_tab_container.Visible = False
      End If

      temp_details = DisplayYachrDetails(yacht_id, "amenities", journalID)
      If Trim(temp_details) <> "" Then
        amenities_label.Text = "<table cellpadding='0' cellspacing='0' width='100%'>" & temp_details
        amenities_tab_container.Visible = True
      Else
        amenities_tab_container.Visible = False
      End If



      temp_details = GetCompanies_DisplayYachtsDetails(yacht_id, journalID)
      If Trim(temp_details) <> "" Then
        yacht_contacts_label.Text = temp_details
      End If


      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("GetCompanies_DisplayYachtsDetails Completed<br>")
      End If



      temp_details = DisplayYacht_Previous_Names_Text(yacht_id, journalID)

      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("DisplayYacht_Previous_Names_Text Completed<br>")
      End If

      If Trim(temp_details) <> "" Then
        previous_name_label.Text = temp_details
      End If


      usage_tab_container.Visible = False



      If journalID = 0 Then
        Me.history_label.Text = aclsData_Temp.Get_Yacht_History(yacht_id, 0, 0, journalID, CRMView, "")
        If Trim(Me.history_label.Text) = "" Then
          Me.history_Tab.Visible = False
          Me.history_container.Visible = False
        End If
      Else
        Me.history_information.Visible = True
        Me.history_information_label.Visible = True
        Me.history_information_panel.HeaderText = "HISTORY INFORMATION AS OF: " & yacht_journal_date
        Me.history_information_label.Text = aclsData_Temp.Get_Yacht_History(yacht_id, 0, 0, journalID, CRMView, "")


        Me.history_Tab.Visible = False
        Me.history_container.Visible = False
      End If

      pic_reference = DisplayYacht_Previous_Names_Evo()


      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("DisplayYacht_Previous_Names_Evo Completed<br>")
      End If


      aircraft_picture_slideshow.Text = GetYachtPictures(yacht_id, pic_reference, journalID)

      If Trim(aircraft_picture_slideshow.Text) <> "" Then
        aircraft_picture_slideshow.Text = aircraft_picture_slideshow.Text & "<br class='div_clear' />"
      End If

      If aircraft_picture_slideshow.Visible = True Then
        all_pics.Visible = True
        all_pics.Text = "<a href='yachtpicture.aspx?yt_id=" & yacht_id & "&journalID=0' target='_blank'>View All Pictures</a>"
      End If

      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("GetAircraftPictures Completed<br>")
      End If


      aircraft_picture_slideshow.Text = aircraft_picture_slideshow.Text


      If Trim(Request.Item("debug")) = "1" Then
        Response.Write("aircraft_picture_slideshow Completed<br>")
      End If




      '          ([yt_id]
      '         ,[yt_journ_id]
      '         ,[yt_model_id]






      'block 1 
      '    ,[yt_superstructure_material]
      '    ,[yt_launch_date])


      'status 



      '          ,[yt_refitted_by_company] 
      'maint




      '          ,[yt_security_system_flag]


      '          ,[yt_class_id]
      '          ,[yt_nbr_decks]












      '    ,[yt_nbr_engines]
      '    ,[yt_engine_fuel_type]
      '    ,[yt_engine_fuel_capacity_gals]
      '    ,[yt_engine_emp]


      '    ,[yt_max_speed_knots]
      '    ,[yt_cruise_speed_knots]
      '    ,[yt_range_miles]


      '    ,[yt_confidential_notes]
      '    ,[yt_common_notes]











      'USED ALREADY-------------------------------------------------------------
      '         ,[yt_forsale_flag]
      '         ,[yt_forsale_status]
      '         ,[yt_asking_price]
      '         ,[yt_forsale_list_date]
      '         ,[yt_purchased_date]
      '         ,[yt_year_mfr]
      '         ,[yt_year_refitted]
      '         ,[yt_yacht_name]
      '    ,[yt_interior_redone_date]
      '    ,[yt_exterior_redone_date]
      '         ,[yt_lifecycle_id]
      '         ,[yt_ownership_type]
      '         ,[yt_foreign_asking_price]
      '          ,[yt_foreign_currency_name]
      '          ,[yt_vat_amount_paid]
      '          ,[yt_lease_flag]
      '          ,[yt_charter_flag]
      '          ,[yt_central_agent_flag]

      '         ,[yt_hull_mfr_nbr]
      '         ,[yt_hull_material]
      '         ,[yt_imo_nbr]
      '         ,[yt_hull_id_nbr]
      '         ,[yt_official_nbr]
      '         ,[yt_lying_port_id]
      '         ,[yt_port_registered_id]
      '          ,[yt_mmsi_mobile_nbr]
      '          ,[yt_radio_call_sign]
      '          ,[yt_registered_country_flag] 

      '     ,[yt_length_overall_meters]
      '     ,[yt_extended_version_hull_length_meters]
      '    ,[yt_length_water_line_meters]
      '    ,[yt_beam_water_line_meters]
      '    ,[yt_draft_water_line_meters]
      '    ,[yt_displacement_tons]
      '    ,[yt_gross_tons] 
      '    ,[yt_nbr_staterooms]
      'USED ALREADY-------------------------------------------------------------





      ' PROBABLY WONT BE USED ----------------------------------------------------
      '         ,[yt_entered_date]
      '         ,[yt_update_date]
      '         ,[yt_action_date]
      '         ,[yt_user_id]
      '    ,[yt_last_call_date]
      '    ,[yt_next_call_date]
      '         ,[yt_yacht_name_search]
      ' PROBABLY WONT BE USED ----------------------------------------------------




    Catch SqlException




    Finally

      SqlCommand.Dispose()
      SqlCommand_inner.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()


      SqlConnection2.Close()
      SqlConnection2.Dispose()
    End Try

  End Function


  Public Function DisplayYachrSpecifications(ByVal yacht_id As Long, ByVal journalID As Long) As String
    DisplayYachrSpecifications = ""

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut2 As StringBuilder = New StringBuilder()
    Dim htmlOut3 As StringBuilder = New StringBuilder()
    Dim htmlOut4 As StringBuilder = New StringBuilder()
    Dim model_info As String = ""
    Dim sQuery As String = ""
    Dim temp_text As String = ""
    Dim market_change As String = ""
    Dim temp_news As String = ""
    Dim news_link As String = ""
    Dim make_left As Integer = 0
    Dim temp_text_left As String = ""
    Dim temp_text_right As String = ""
    Dim temp_text_mid As String = ""


    sQuery = "SELECT *, (select yp_port_name from yacht_port where yp_id = yt_port_registered_id) as reg_port "
    sQuery &= " , (select yp_port_name from yacht_port where yp_id = yt_lying_port_id) as ly_port "
    sQuery &= " , (select yp_port_name from yacht_port where yp_id = yt_home_port_id) as home_port "
    sQuery &= " , (select yp_port_name from yacht_port where yp_id = yt_home_port_id) as heli_port "

    sQuery &= " , (select top 1 yd_description  FROM Yacht_Details WITH (NOLOCK) where (yd_type = 'Exterior')  AND (yd_name = 'Helipad')  and yd_yt_id = yt_id  and yd_journ_id = 0) as heli_details "
    sQuery &= " , (select top 1 yd_description FROM Yacht_Details WITH (NOLOCK) where (yd_type = 'Exterior')  AND (yd_name = 'Hangar') and yd_yt_id = yt_id   and yd_journ_id = 0) as hanger_details "

    sQuery &= " FROM yacht WITH(NOLOCK) "
    sQuery &= " inner join yacht_model on ym_model_id = yt_model_id "
    sQuery &= " inner join yacht_category_size on  ycs_motor_type = ym_motor_type and ycs_category_size = ym_category_size "
    sQuery &= " inner join yacht_motor_type on  ymt_motor_type = ym_motor_type  "
    sQuery &= " left outer join yacht_classification_society_types on ycst_code = yt_class_id  "

    If journalID > 0 Then
      sQuery &= " inner join  journal  on  journ_yacht_id  = yt_id and journ_id = yt_journ_id  "
    End If

    sQuery &= " WHERE yt_id = " & yacht_id

    sQuery &= " AND yt_journ_id = " & journalID & " "

    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("DisplayYachrSpecifications: " & sQuery & "<br>")
    End If

    Try

      SqlCommand.CommandText = sQuery.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then


        htmlOut2.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut3.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut4.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        ' model_info = model_info & ("<table cellpadding='0' cellspacing='0' width='100%'>")

        Do While lDataReader.Read()


          stats_tab.HeaderText = "<b>"
          ' -----------------------------------------------------------------------------------------------------------------
          If Not IsDBNull(lDataReader.Item("ym_brand_name")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_brand_name").ToString) Then
            'htmlOut.Append(build_column_string(lDataReader.Item("ym_brand_name").ToString, "Brand", build_column_number))
            stats_tab.HeaderText = stats_tab.HeaderText & lDataReader.Item("ym_brand_name") & " "
          End If

          If Not IsDBNull(lDataReader.Item("ym_model_name")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_model_name").ToString) Then
            'htmlOut.Append(build_column_string(lDataReader.Item("ym_model_name").ToString, "Model", build_column_number))
            stats_tab.HeaderText = stats_tab.HeaderText & lDataReader.Item("ym_model_name") & " "
          End If

          Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Yacht Display: " & lDataReader.Item("yt_yacht_name"), Nothing, 0, 0, 0, 0, 0, 0, 0, CInt(lDataReader.Item("yt_id")))

          If Not IsDBNull(lDataReader.Item("yt_yacht_name")) Then
            Master.SetPageTitle(UCase(Replace(stats_tab.HeaderText, "<b>", "") & " """ & lDataReader.Item("yt_yacht_name") & """"))
          Else
            Master.SetPageTitle(UCase(Replace(stats_tab.HeaderText, "<b>", "")))
          End If


          stats_tab.HeaderText = stats_tab.HeaderText & """" & "<i>" & lDataReader.Item("yt_yacht_name") & "</i>" & """" & " "
          stats_tab.HeaderText = stats_tab.HeaderText & "</b>"
          stats_tab.HeaderText = UCase(stats_tab.HeaderText)



          If journalID > 0 Then
            If Not IsDBNull(lDataReader.Item("journ_date")) Then
              yacht_journal_date = lDataReader.Item("journ_date")
            End If
          End If

          '---------------------- SECTION II---------------------------------------------------------------------------------------------------


          'Section II.
          'Year Built			Year Launched
          'Flag()
          'Class				Hull#
          'Reg. Port			Official #
          'Home Port			IMO
          'Lying				HIN#
          'MMSI				Call Sign
          'Put in Class Details
          build_column_number = 1
          If Not IsDBNull(lDataReader.Item("yt_year_mfr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_year_mfr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_year_mfr").ToString, "Year Mfr.", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_launch_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_launch_date").ToString) Then
            If Year(lDataReader.Item("yt_launch_date")) <> 1900 Then
              htmlOut.Append(build_column_string(Year(lDataReader.Item("yt_launch_date")).ToString, "Year Dlv.", build_column_number, 0))
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_hull_mfr_nbr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_hull_mfr_nbr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_hull_mfr_nbr").ToString, "Hull #", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_imo_nbr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_imo_nbr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_imo_nbr").ToString, "IMO", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_hull_id_nbr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_hull_id_nbr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_hull_id_nbr").ToString, "Hull ID Number", build_column_number, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_official_nbr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_official_nbr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_official_nbr").ToString, "Official Nbr", build_column_number, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_mmsi_mobile_nbr")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_mmsi_mobile_nbr").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_mmsi_mobile_nbr").ToString, "MMSI", build_column_number, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_radio_call_sign")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_radio_call_sign").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_radio_call_sign").ToString, "Call Sign", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("ycst_society_name")) And Not String.IsNullOrEmpty(lDataReader.Item("ycst_society_name").ToString) Then
            If Trim(lDataReader.Item("ycst_society_name").ToString) <> "Unknown" Then
              htmlOut.Append(build_column_string(lDataReader.Item("ycst_society_name").ToString, "Class", build_column_number, 0))
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_registered_country_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_registered_country_flag").ToString) Then
            If Trim(lDataReader.Item("yt_registered_country_flag").ToString) <> "Unknown" Then
              htmlOut.Append(build_column_string(lDataReader.Item("yt_registered_country_flag").ToString, "Flag", build_column_number, 0))
            End If
          End If

          If Not IsDBNull(lDataReader.Item("ly_port")) And Not String.IsNullOrEmpty(lDataReader.Item("ly_port").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("ly_port").ToString, "Lying Port", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("home_port")) And Not String.IsNullOrEmpty(lDataReader.Item("home_port").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("home_port").ToString, "Home Port", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("reg_port")) And Not String.IsNullOrEmpty(lDataReader.Item("reg_port").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("reg_port").ToString, "Reg Port", build_column_number, 0))
          Else
            htmlOut.Append(build_column_string("Unknown", "Reg Port", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_year_refitted")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_year_refitted").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_year_refitted").ToString, "Year Refitted", build_column_number, 0), 0)
          End If

          If Not IsDBNull(lDataReader.Item("yt_submotor_type")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_submotor_type").ToString) Then
            htmlOut.Append(build_column_string(lDataReader.Item("yt_submotor_type").ToString, "Sub Type", build_column_number, 0))
          End If

          ' MISSING CLASS
          ' MISSING CLASS DETAILS
          ' MISSING HIN NUMBEr - Might not need
          '---------------------- SECTION II---------------------------------------------------------------------------------------------------





          '---------------------- SECTION III---------------------------------------------------------------------------------------------------
          'Section III:   Should be the dimensions and size box
          '                    LOA()
          '                    Beam()
          '                    LWL()
          '                    Draft()
          '                    Gross(Tons)
          '                    Displacement(Tons)
          'Hull Material			Number of Decks
          'Superstructure		Number of Staterooms
          build_column_number = 1

          If (Not IsDBNull(lDataReader.Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_overall_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_beam_water_line_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_water_line_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_draft_water_line_meters").ToString)) Then
            htmlOut3.Append("<tr><td>&nbsp;</td><td><b>Dimensions (metrics)</b></td><td>&nbsp;</td><td><b>Dimensions (US Standard)</b></td></tr>")
            htmlOut3.Append("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
          End If

          If Not IsDBNull(lDataReader.Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_overall_meters").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_length_overall_meters").ToString, "LOA (m)", build_column_number, 2))
            htmlOut3.Append(build_column_string(convert_metric_to_us(lDataReader.Item("yt_length_overall_meters").ToString), "LOA (ft)", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_beam_water_line_meters").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_beam_water_line_meters").ToString, "Beam (m)", build_column_number, 2))
            If Trim(lDataReader.Item("yt_beam_water_line_meters").ToString) <> "0" Then
              htmlOut3.Append(build_column_string(convert_metric_to_us(lDataReader.Item("yt_beam_water_line_meters").ToString), "Beam (ft)", build_column_number, 0))
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_water_line_meters").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_length_water_line_meters").ToString, "LWL (m)", build_column_number, 2))
            If Trim(lDataReader.Item("yt_length_water_line_meters").ToString) <> "0" Then
              htmlOut3.Append(build_column_string(convert_metric_to_us(lDataReader.Item("yt_length_water_line_meters").ToString), "LWL (ft)", build_column_number, 0))
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_draft_water_line_meters").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_draft_water_line_meters").ToString, "Draft (m)", build_column_number, 2))
            If Trim(lDataReader.Item("yt_draft_water_line_meters").ToString) <> "0" Then
              htmlOut3.Append(build_column_string(convert_metric_to_us(lDataReader.Item("yt_draft_water_line_meters").ToString), "Draft (ft)", build_column_number, 0))
            End If
          End If

          If (Not IsDBNull(lDataReader.Item("yt_length_overall_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_overall_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_beam_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_beam_water_line_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_length_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_length_water_line_meters").ToString)) Or (Not IsDBNull(lDataReader.Item("yt_draft_water_line_meters")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_draft_water_line_meters").ToString)) Then
            htmlOut3.Append("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
          End If

          If Not IsDBNull(lDataReader.Item("yt_gross_tons")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_gross_tons").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_gross_tons").ToString, "Gross Tons", build_column_number, 2))
          End If

          If Not IsDBNull(lDataReader.Item("yt_displacement_tons")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_displacement_tons").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_displacement_tons").ToString, "Displacement Tons", build_column_number, 2))
          End If

          If Not IsDBNull(lDataReader.Item("yt_nbr_decks")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_nbr_decks").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_nbr_decks").ToString, "Number Of Decks", build_column_number, 2))
          End If


          If Not IsDBNull(lDataReader.Item("yt_hull_material")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_hull_material").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_hull_material").ToString, "Hull Material", build_column_number, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_superstructure_material")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_superstructure_material").ToString) Then
            htmlOut3.Append(build_column_string(lDataReader.Item("yt_superstructure_material").ToString, "Superstructure Material", build_column_number, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_nbr_staterooms")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_nbr_staterooms").ToString) Then
            interior_extra &= (build_column_string(lDataReader.Item("yt_nbr_staterooms").ToString, "Number of Staterooms/Cabins", 1, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_nbr_crew")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_nbr_crew").ToString) Then
            interior_extra &= (build_column_string(lDataReader.Item("yt_nbr_crew").ToString, "Number of Crew", 1, 0))
          End If


          If Not IsDBNull(lDataReader.Item("yt_nbr_guests")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_nbr_guests").ToString) Then
            interior_extra &= (build_column_string(lDataReader.Item("yt_nbr_guests").ToString, "Number of Guests", 1, 0))
          End If

          If Trim(interior_extra) <> "" Then
            interior_extra = Replace(interior_extra, "width='50%'", "width='100%'")
          End If
          '---------------------- SECTION III---------------------------------------------------------------------------------------------------






          '---------------------- SECTION V---------------------------------------------------------------------------------------------------
          'Section V: Status For Sale or Charter or both
          '                    Asking Price			VAT
          'Wholly Owned		In Operation
          '                    Central(Agent)
          'Notes: change to Season availability
          '            price per week/month etc.
          'Not available for sale or charter.......etc.

          'Put central agent contact here
          'and owner if applicable

          build_column_number = 1
          If Not IsNothing(status_tab_container) Then
            If lDataReader.Item("yt_forsale_flag").ToString.ToUpper = "Y" Or lDataReader.Item("yt_for_lease_flag") = "Y" Or lDataReader.Item("yt_for_charter_flag") = "Y" Then
              status_tab_container.CssClass = "green-theme"
              usage_tab_container.CssClass = "dark-theme"
              ' notes_tab_container.cssclass = "ajax__tab_blue_gray-theme"
            Else
              status_tab_container.CssClass = "dark-theme"
              usage_tab_container.CssClass = "dark-theme"
            End If
          End If



          If Not IsDBNull(lDataReader.Item("yt_forsale_status")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_forsale_status").ToString) Then
            'htmlOut2.Append(build_column_string(lDataReader.Item("yt_forsale_status").ToString, "Status", build_column_number))


            If Trim(lDataReader.Item("yt_forsale_flag")) = "Y" And Trim(lDataReader.Item("yt_for_lease_flag")) = "Y" And Trim(lDataReader.Item("yt_for_charter_flag")) = "Y" Then
              market_change = "For Sale/Lease/Charter"
            ElseIf Trim(lDataReader.Item("yt_forsale_flag")) = "Y" And Trim(lDataReader.Item("yt_for_lease_flag")) = "Y" And Trim(lDataReader.Item("yt_for_charter_flag")) = "N" Then
              market_change = "For Sale/Lease"
            ElseIf Trim(lDataReader.Item("yt_forsale_flag")) = "Y" And Trim(lDataReader.Item("yt_for_lease_flag")) = "N" And Trim(lDataReader.Item("yt_for_charter_flag")) = "Y" Then
              market_change = "For Sale/Charter"
            ElseIf Trim(lDataReader.Item("yt_forsale_flag")) = "N" And Trim(lDataReader.Item("yt_for_lease_flag")) = "Y" And Trim(lDataReader.Item("yt_for_charter_flag")) = "Y" Then
              market_change = "For Lease/Charter"
            ElseIf Trim(lDataReader.Item("yt_forsale_flag")) = "Y" Then
              market_change = "For Sale"
            ElseIf Trim(lDataReader.Item("yt_for_lease_flag")) = "Y" Then
              market_change = "For Lease"
            ElseIf Trim(lDataReader.Item("yt_for_charter_flag")) = "Y" Then
              market_change = "For Charter"
            End If

            status_tab.HeaderText = "STATUS: " & UCase(lDataReader.Item("yt_forsale_status").ToString) & " " & market_change
          End If



          'If Not IsDBNull(lDataReader.Item("yt_for_lease_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_lease_flag").ToString) Then
          '    If Trim(lDataReader.Item("yt_for_lease_flag")) = "Y" Then
          '        htmlOut2.Append(build_column_string(yn_to_yes_no(lDataReader.Item("yt_for_lease_flag").ToString), "Avail For Lease", build_column_number, 0))
          '    End If
          'End If


          'If Not IsDBNull(lDataReader.Item("yt_for_charter_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_for_charter_flag").ToString) Then
          '    If Trim(lDataReader.Item("yt_for_charter_flag")) = "Y" Then
          '        htmlOut2.Append(build_column_string(yn_to_yes_no(lDataReader.Item("yt_for_charter_flag").ToString), "Avail for Charter?", build_column_number, 0))
          '    End If
          'End If


          If Trim(lDataReader.Item("yt_asking_price_wordage")) = "Inquire" Then

            If Not IsDBNull(lDataReader.Item("yt_asking_price_wordage")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_asking_price_wordage").ToString) Then
              htmlOut2.Append(build_column_string(lDataReader.Item("yt_asking_price_wordage").ToString, "Status", build_column_number, 0))
            End If

          Else

            If Not IsDBNull(lDataReader.Item("yt_forsale_status")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_forsale_status").ToString) Then
              If Trim(lDataReader.Item("yt_forsale_status")) = "For Sale" Then
                If Not IsDBNull(lDataReader.Item("yt_forsale_list_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_forsale_list_date").ToString) Then
                  If Year(lDataReader.Item("yt_forsale_list_date")) <> "1900" Then
                    htmlOut2.Append(build_column_string(FormatDateTime(lDataReader.Item("yt_forsale_list_date").ToString, DateFormat.ShortDate), "Date Listed", build_column_number, 0))
                  End If
                End If
              End If
            End If



            If Not IsDBNull(lDataReader.Item("yt_asking_price")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_asking_price").ToString) Then
              htmlOut2.Append(build_column_string(lDataReader.Item("yt_asking_price").ToString, "Asking Price", build_column_number, 0))
            End If

            If Not IsDBNull(lDataReader.Item("yt_foreign_asking_price")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_foreign_asking_price").ToString) Then
              If CDbl(lDataReader.Item("yt_foreign_asking_price")) <> 0 Then
                If Not IsDBNull(lDataReader.Item("yt_foreign_currency_name")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_foreign_currency_name").ToString) Then
                  If Trim(lDataReader.Item("yt_foreign_currency_name").ToString) <> "Dollar" Then
                    htmlOut2.Append(build_column_string(lDataReader.Item("yt_foreign_asking_price").ToString & " (" & lDataReader.Item("yt_foreign_currency_name").ToString & ")", "Foreign Asking Price", build_column_number, 0))
                  Else
                    htmlOut2.Append(build_column_string(lDataReader.Item("yt_foreign_asking_price").ToString, "Foreign Asking Price", build_column_number, 0))
                  End If
                Else
                  htmlOut2.Append(build_column_string(lDataReader.Item("yt_foreign_asking_price").ToString, "Foreign Asking Price", build_column_number, 0))
                End If
              End If

            End If

          End If



          If Not IsDBNull(lDataReader.Item("yt_purchased_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_purchased_date").ToString) Then
            htmlOut2.Append(build_column_string(FormatDateTime(lDataReader.Item("yt_purchased_date").ToString, DateFormat.ShortDate), "Date Purchased", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_lifecycle_id")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_lifecycle_id").ToString) Then
            htmlOut2.Append(build_column_string(life_cycle_stage(lDataReader.Item("yt_lifecycle_id").ToString), "", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_lifecycle_status")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_lifecycle_status").ToString) Then
            htmlOut2.Append(build_column_string(lDataReader.Item("yt_lifecycle_status").ToString, "", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("yt_ownership_type")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_ownership_type").ToString) Then
            htmlOut2.Append(build_column_string(ownership_type(lDataReader.Item("yt_ownership_type").ToString), "", build_column_number, 0))
          End If



          If Not IsDBNull(lDataReader.Item("yt_vat_status")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_vat_status").ToString) Then
            htmlOut2.Append(build_column_string(lDataReader.Item("yt_vat_status").ToString, "VAT", build_column_number, 0))
            If Trim(lDataReader.Item("yt_vat_status").ToString) = "Paid" Then
              ' If Not IsDBNull(lDataReader.Item("yt_vat_amount_paid")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_vat_amount_paid").ToString) Then
              'htmlOut2.Append(build_column_string(lDataReader.Item("yt_vat_amount_paid").ToString, "VAT Amount Paid", build_column_number))
              'End If
            End If
          End If



          If Not IsDBNull(lDataReader.Item("yt_central_agent_flag")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_central_agent_flag").ToString) Then
            If Trim(lDataReader.Item("yt_central_agent_flag")) = "Y" Then
              htmlOut2.Append(build_column_string(yn_to_yes_no(lDataReader.Item("yt_central_agent_flag").ToString), "Central Agent?", build_column_number, 0))
            End If
          End If


          If Not IsDBNull(lDataReader.Item("yt_forsale_list_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_forsale_list_date").ToString) Then
            If Trim(lDataReader.Item("yt_forsale_flag")) = "Y" Then
              htmlOut2.Append(build_column_string(FormatDateTime(lDataReader.Item("yt_forsale_list_date").ToString, DateFormat.ShortDate), "Date Listed", build_column_number, 0))

              htmlOut2.Append(build_column_string(DateDiff(DateInterval.Day, CDate(lDataReader.Item("yt_forsale_list_date").ToString), Date.Now()), "Days On Market", build_column_number, 0))
            End If
          End If


          If build_column_number = 2 Then
            htmlOut2.Append("</tr>")
          End If


          If Not IsDBNull(lDataReader.Item("yt_not_in_usa_water")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_not_in_usa_water").ToString) Then
            If Trim(lDataReader.Item("yt_not_in_usa_water").ToString) = "Y" Then
              htmlOut2.Append("<tr>")
              htmlOut2.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
              htmlOut2.Append("<td valign='top' align='left' width='100%' colspan='4'><span class='li'>")
              htmlOut2.Append("Not available for sale or charter to US residents while in US waters")
              htmlOut2.Append("</span></td></tr>")
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_confidential_notes")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_confidential_notes").ToString) Then
            htmlOut2.Append("<tr>")
            htmlOut2.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            htmlOut2.Append("<td valign='top' align='left' width='100%' colspan='4'><span class='li'>")
            htmlOut2.Append("<span class='label'>Notes: </span>")
            htmlOut2.Append(lDataReader.Item("yt_confidential_notes").ToString)
            htmlOut2.Append("</span></td></tr>")
          End If
          '---------------------- SECTION V---------------------------------------------------------------------------------------------------


          '---------------------- SECTION VI---------------------------------------------------------------------------------------------------
          'Section(VI) : Maintenance()
          'Class Details
          'Refit(Details)


          '---------------------- SECTION VI---------------------------------------------------------------------------------------------------


          '---------------------- SECTION VI---------------------------------------------------------------------------------------------------
          ' Section(VII) : Power()
          'Number of Engines:		Fuel Type:

          'Engines Frame--------------------------------
          build_column_number = 1
          If Not IsDBNull(lDataReader.Item("yt_nbr_engines")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_nbr_engines").ToString) Then
            engine_info = engine_info & build_column_string(FormatNumber(lDataReader.Item("yt_nbr_engines"), 0), "Number of Engines", 1, 0)
          End If

          If Not IsDBNull(lDataReader.Item("yt_engine_fuel_type")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_engine_fuel_type").ToString) Then
            engine_info = engine_info & build_column_string(lDataReader.Item("yt_engine_fuel_type"), "Fuel Type", 2, 0)
          End If

          If Not IsDBNull(lDataReader.Item("yt_engine_fuel_capacity_gals")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_engine_fuel_capacity_gals").ToString) Then
            engine_info = engine_info & build_column_string(FormatNumber(lDataReader.Item("yt_engine_fuel_capacity_gals"), 2), "Fuel Capacity/Gal", 1, 2)
          End If

          If Not IsDBNull(lDataReader.Item("yt_engine_emp")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_engine_emp").ToString) Then
            If Trim(lDataReader.Item("yt_engine_emp")) <> "" Then
              engine_info = engine_info & build_column_string(lDataReader.Item("yt_engine_emp"), "EMP", 2, 0)
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_engine_times_current")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_engine_times_current").ToString) Then
            If Trim(lDataReader.Item("yt_engine_times_current")) <> "" Then 
              engine_tab.HeaderText &= ": Times Current as Of " & FormatDateTime(lDataReader.Item("yt_engine_times_current").ToString, DateFormat.ShortDate)
            End If
          End If
          'Engines Frame--------------------------------
          '---------------------- SECTION VI---------------------------------------------------------------------------------------------------





          ' Model Box?----------------------------
          build_column_number = 1
          'If Not IsDBNull(lDataReader.Item("ym_motor_type")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_motor_type").ToString) And Not IsDBNull(lDataReader.Item("ym_nbr_hulls")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_nbr_hulls").ToString) Then
          'model_info = model_info & (build_column_string(lDataReader.Item("ymt_description").ToString & "/" & lDataReader.Item("ym_nbr_hulls").ToString, "Type", build_column_number, 0))
          ' End If

          If Not IsDBNull(lDataReader.Item("ym_motor_type")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_motor_type").ToString) Then
            model_info = model_info & (build_column_string(lDataReader.Item("ymt_description").ToString, "Type", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("ym_nbr_hulls")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_nbr_hulls").ToString) Then
            model_info = model_info & (build_column_string(lDataReader.Item("ym_nbr_hulls").ToString, "Nbr Hulls", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("ym_category_size")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_category_size").ToString) Then
            model_info = model_info & (build_column_string(lDataReader.Item("ycs_description").ToString, "Size", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("ym_hull_configuration")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_hull_configuration").ToString) Then
            model_info = model_info & (build_column_string(lDataReader.Item("ym_hull_configuration").ToString, "Hull Config", build_column_number, 0))
          End If

          If Not IsDBNull(lDataReader.Item("ym_mfr_comp_id")) And Not String.IsNullOrEmpty(lDataReader.Item("ym_mfr_comp_id").ToString) Then
            model_info = model_info & (build_column_string(Find_Company_Name_MFR(lDataReader.Item("ym_mfr_comp_id").ToString), "Manufacturer", build_column_number, 0))
          End If

          ' -----------------------------------------------------------------------------------------------------------------












          ' -----------------------------------------------------------------------------------------------------------------
          build_column_number = 1
          'If Not IsDBNull(lDataReader.Item("yt_interior_redone_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_interior_redone_date").ToString) Then
          '    interior_redone = interior_redone & build_column_string(lDataReader.Item("yt_interior_redone_date").ToString, "Interior Redone Date", build_column_number, 0)
          '    interior_redone = interior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
          '    interior_redone = interior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
          '    interior_redone = interior_redone & "</td>"
          '    interior_redone = interior_redone & "</tr>"
          'End If
          If Not IsDBNull(lDataReader.Item("yt_interior_redone_year")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_interior_redone_year").ToString) Then
            If Not IsDBNull(lDataReader.Item("yt_interior_redone_month")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_interior_redone_month").ToString) Then
              interior_redone = interior_redone & build_column_string(lDataReader.Item("yt_interior_redone_month").ToString & "/" & lDataReader.Item("yt_interior_redone_year").ToString, "Interior Refit Date", build_column_number, 0)
              interior_redone = interior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
              interior_redone = interior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
              interior_redone = interior_redone & "</td>"
              interior_redone = interior_redone & "</tr>"
            Else
              interior_redone = interior_redone & build_column_string(lDataReader.Item("yt_interior_redone_year").ToString, "Interior Refit Date", build_column_number, 0)
              interior_redone = interior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
              interior_redone = interior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
              interior_redone = interior_redone & "</td>"
              interior_redone = interior_redone & "</tr>"
            End If
          End If

          ' -----------------------------------------------------------------------------------------------------------------




          ' ----------------------------------------------------------------------------------------------------------------- 
          build_column_number = 1
          'If Not IsDBNull(lDataReader.Item("yt_exterior_redone_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_exterior_redone_date").ToString) Then
          '    exterior_redone = exterior_redone & build_column_string(lDataReader.Item("yt_exterior_redone_date").ToString, "Exterior Refit Date", build_column_number, 0)
          '    exterior_redone = exterior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
          '    exterior_redone = exterior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
          '    exterior_redone = exterior_redone & "</td>"
          '    exterior_redone = exterior_redone & "</tr>"
          'End If


          If Not IsDBNull(lDataReader.Item("yt_exterior_redone_year")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_exterior_redone_year").ToString) Then
            If Not IsDBNull(lDataReader.Item("yt_exterior_redone_month")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_exterior_redone_month").ToString) Then
              exterior_redone = exterior_redone & build_column_string(lDataReader.Item("yt_exterior_redone_month").ToString & "/" & lDataReader.Item("yt_exterior_redone_year").ToString, "Exterior Refit Date", build_column_number, 0)
              exterior_redone = exterior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
              exterior_redone = exterior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
              exterior_redone = exterior_redone & "</td>"
              exterior_redone = exterior_redone & "</tr>"
            Else
              exterior_redone = exterior_redone & build_column_string(lDataReader.Item("yt_exterior_redone_year").ToString, "Exterior Refit Date", build_column_number, 0)
              exterior_redone = exterior_redone & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
              exterior_redone = exterior_redone & "<td valign='top' align='left' width='50%'>&nbsp;"
              exterior_redone = exterior_redone & "</td>"
              exterior_redone = exterior_redone & "</tr>"
            End If
          End If

          ' -----------------------------------------------------------------------------------------------------------------








          ' -----------------------------------------------------------------------------------------------------------------
          build_column_number = 1
          make_left = 0

          If Not IsDBNull(lDataReader.Item("yt_cruise_speed_knots")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_cruise_speed_knots").ToString) Then
            htmlOut4.Append(build_column_string(lDataReader.Item("yt_cruise_speed_knots").ToString, "Cruise Speed KN", build_column_number, 2))
            If Trim(lDataReader.Item("yt_cruise_speed_knots")) <> 0.0 Then
              make_left = make_left + 1
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yt_max_speed_knots")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_max_speed_knots").ToString) Then
            htmlOut4.Append(build_column_string(lDataReader.Item("yt_max_speed_knots").ToString, "Max Speed KN", build_column_number, 2))
            If Trim(lDataReader.Item("yt_max_speed_knots")) <> 0.0 Then
              make_left = make_left + 1
            End If
          End If


          If Not IsDBNull(lDataReader.Item("yt_range_miles")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_range_miles").ToString) Then
            htmlOut4.Append(build_column_string(lDataReader.Item("yt_range_miles").ToString, "Range(NM)", build_column_number, 2))
            If Trim(lDataReader.Item("yt_range_miles")) <> 0.0 Then
              make_left = make_left + 1
            End If
          End If

          If make_left = 1 Then
            htmlOut4.Append("<td>&nbsp;</td><td>&nbsp;</td></tr>")
          End If
          ' -----------------------------------------------------------------------------------------------------------------







          helipad_string = helipad_string & "<table cellpadding='0' cellspacing='0' width='100%'>"

          helipad_string = helipad_string & "<tr>"
          helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
          helipad_string = helipad_string & "<td valign='top' align='left' width='50%'>"

          helipad_string = helipad_string & "<span class='li'><span class='label'>Helipad?: </span>"

          If Not IsDBNull(lDataReader.Item("yt_helipad")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_helipad").ToString) Then
            If Trim(lDataReader.Item("yt_helipad").ToString) = "N" Then
              helipad_string = helipad_string & "No"
            ElseIf Trim(lDataReader.Item("yt_helipad").ToString) = "U" Then
              helipad_string = helipad_string & "Unknown"
            Else
              helipad_string = helipad_string & "" & yn_to_yes_no(lDataReader.Item("yt_helipad").ToString) & ""
            End If
          End If


          helipad_string = helipad_string & "</span>"

          helipad_string = helipad_string & "</td>"
          helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
          helipad_string = helipad_string & "<td valign='top' align='left' width='50%'>"

          helipad_string = helipad_string & "<span class='li'><span class='label'>Helipad Hangar?:</span> "


          If Not IsDBNull(lDataReader.Item("yt_helipad_hangar")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_helipad_hangar").ToString) Then
            If Trim(lDataReader.Item("yt_helipad_hangar").ToString) = "N" Then
              helipad_string = helipad_string & "No"
            ElseIf Trim(lDataReader.Item("yt_helipad_hangar").ToString) = "U" Then
              helipad_string = helipad_string & "Unknown"
            Else
              helipad_string = helipad_string & "" & yn_to_yes_no(lDataReader.Item("yt_helipad_hangar").ToString) & ""
            End If
          End If

          helipad_string = helipad_string & "</span>"

          helipad_string = helipad_string & "</td>"
          helipad_string = helipad_string & "</tr>"


          If Not IsDBNull(lDataReader.Item("yt_helipad")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_helipad").ToString) Then
            If Trim(lDataReader.Item("yt_helipad")) = "Y" Then



              If Not IsDBNull(lDataReader.Item("yt_helipad_approved_for_lbs")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_helipad_approved_for_lbs").ToString) Then
                If Trim(lDataReader.Item("yt_helipad_approved_for_lbs")) <> "0" Then

                  helipad_string = helipad_string & "<tr>"

                  helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                  helipad_string = helipad_string & "<td valign='top' align='left' width='50%'>"

                  helipad_string = helipad_string & "<span class='li'><span class='label'>Approved Lbs: </span>"

                  ' really pounds is saved in kg 

                  helipad_string = helipad_string & FormatNumber(convert_kg_to_lbs(lDataReader.Item("yt_helipad_approved_for_lbs").ToString), 2) & ""

                  helipad_string = helipad_string & "</span></td>"
                  helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                  helipad_string = helipad_string & "<td width='50%'>"
                  helipad_string = helipad_string & "<span class='li'><span class='label'>Approved KGs: </span>"
                  helipad_string = helipad_string & "" & FormatNumber(lDataReader.Item("yt_helipad_approved_for_lbs").ToString, 2) & ""

                  helipad_string = helipad_string & "</span>"

                  helipad_string = helipad_string & "</td>"
                  helipad_string = helipad_string & "</tr>"

                End If
              End If

              If Not IsDBNull(lDataReader.Item("yt_helipad_radius")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_helipad_radius").ToString) Then
                If Trim(lDataReader.Item("yt_helipad_radius")) <> "0" Then

                  helipad_string = helipad_string & "<tr>"
                  helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                  helipad_string = helipad_string & "<td valign='top' align='left' width='50%'>"

                  helipad_string = helipad_string & "<span class='li'><span class='label'>Radius (ft):</span> "

                  helipad_string = helipad_string & convert_metric_to_us(lDataReader.Item("yt_helipad_radius").ToString) & ""

                  helipad_string = helipad_string & "</span></td>"
                  helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
                  helipad_string = helipad_string & "<td width='50%'>"

                  helipad_string = helipad_string & "<span class='li'><span class='label'>Radius (m):</span> "
                  helipad_string = helipad_string & "" & FormatNumber(lDataReader.Item("yt_helipad_radius").ToString, 2) & ""
                  helipad_string = helipad_string & "</span>"

                  helipad_string = helipad_string & "</td>"
                  helipad_string = helipad_string & "</tr>"
                End If
              End If

              helipad_string = helipad_string & "</tr>"
            End If
          End If


          If Not IsDBNull(lDataReader.Item("heli_details")) And Not String.IsNullOrEmpty(lDataReader.Item("heli_details").ToString) Then
            helipad_string = helipad_string & "<tr>"
            helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
            helipad_string = helipad_string & "<td valign='top' align='left' width='92%' colspan='4'><span class='li'>"
            helipad_string = helipad_string & "" & lDataReader.Item("heli_details").ToString & ""
            helipad_string = helipad_string & "</span></td>"
            helipad_string = helipad_string & "</tr>"
          End If

          If Not IsDBNull(lDataReader.Item("hanger_details")) And Not String.IsNullOrEmpty(lDataReader.Item("hanger_details").ToString) Then
            helipad_string = helipad_string & "<tr>"
            helipad_string = helipad_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
            helipad_string = helipad_string & "<td valign='top' align='left' width='92%' colspan='4'><span class='li'>"
            helipad_string = helipad_string & "" & lDataReader.Item("hanger_details").ToString & ""
            helipad_string = helipad_string & "</span></td>"
            helipad_string = helipad_string & "</tr>"
          End If

          helipad_string = helipad_string & "</table>"


          If Not IsDBNull(lDataReader.Item("yt_common_notes")) And Not String.IsNullOrEmpty(lDataReader.Item("yt_common_notes").ToString) Then
            temp_text = lDataReader.Item("yt_common_notes").ToString
            temp_text = Replace(temp_text, vbCrLf, "<br>")
            '  temp_text_right
            '  temp_text_left 
            '<p><span style="color: #003366">View details about<em><strong> VivieRae </strong></em>at </span><a href="http://www.vivierae.com/">www.vivierae.com/</a></p>

            If InStr(Trim(temp_text), "href=") > 0 Then
              '<p><span style="color: #003366">View details about<em><strong> VivieRae </strong></em>at </span><a href=
              '"http://www.vivierae.com/">www.vivierae.com/</a></p>

              temp_text_left = Left(Trim(temp_text), InStr(Trim(temp_text), "href") - 1)
              temp_text_right = Right(Trim(temp_text), Len(Trim(temp_text)) - InStr(Trim(temp_text), "href") + 2)

              '"http://www.vivierae.com/"
              temp_text_mid = Left(Trim(temp_text_right), InStr(Trim(temp_text_right), ">") - 1)

              '>www.vivierae.com/</a></p>
              temp_text_right = Replace(temp_text_right, temp_text_mid, "")

              temp_text = Trim(temp_text_left) & " " & Trim(temp_text_mid) & " target='_blank' " & Trim(temp_text_right)

            End If


            description_label.Text = "<table cellpadding='5' cellspacing='0' width='100%'><tr><td width='100%'>" & temp_text & "</td></tr></table>"
            Me.description_container.Visible = True
          Else
            Me.description_container.Visible = False
          End If







          'Helipad Frame--------------------------------

          'If lDataReader.Item("yt_helipad") = "Y" Then
          '     chk_helipad.Value = 1
          'Else
          '     chk_helipad.Value = 0
          ' End If

          '  If lDataReader.Item("yt_helipad_hangar") = "Y" Then
          '      chk_helipad_hanger.Value = 1
          '  Else
          '      chk_helipad_hanger.Value = 0
          '  End If


          ' txt_helipad_lbs.Text = lDataReader.Item("yt_helipad_approved_for_lbs")
          ' text_helipad_radius.Text = lDataReader.Item("yt_helipad_radius")

          'Helipad Frame--------------------------------

          ' Notes---------------------------------------
          'If Not IsNull(lDataReader.Item("yt_confidential_notes")) Then
          '     txt_confidential_notes.Text = lDataReader.Item("yt_confidential_notes")
          ' Else
          '     txt_confidential_notes.Text = ""
          ' End If

          ' txt_common_notes.Text = lDataReader.Item("yt_common_notes")
          ' Notes---------------------------------------




          '----------------------------- STATUS SECTION-------------------------------------------------



        Loop


        htmlOut2.Append("</table>")
        htmlOut3.Append("</table>")
        htmlOut4.Append("</table>")
        '  model_info = model_info & "</table>"
      End If

      lDataReader.Close()


      If journalID = 0 Then
        sQuery = ""
        sQuery = "select * from View_Yacht_News where yt_id = " & yacht_id & "  order by ytnews_date desc"
        SqlCommand.CommandText = sQuery.ToString
        lDataReader = SqlCommand.ExecuteReader()

        If lDataReader.HasRows Then
          temp_news = "<table cellpadding='5' cellspacing='0' width='100%'>"
          Do While lDataReader.Read
            If Not IsDBNull(lDataReader("ytnews_web_address")) Then
              news_link = lDataReader("ytnews_web_address")
              If InStr(news_link, "http://") = 0 And InStr(news_link, "https://") = 0 And Trim(news_link) <> "" Then
                news_link = "http://" & news_link
              End If
            End If
            temp_news += "<tr><td><span class='li'>" & lDataReader("ytnews_date") & "-<A href='" & news_link & "' target='_blank'>" & lDataReader("ytnews_title") & "</a>:<br> " & Left(lDataReader("ytnews_description"), 300) & " ... <i><u>More At <A href='" & news_link & "' target='_blank'>" & lDataReader("ytnewssrc_name") & "</a> </u></i></span></td></tr>"
          Loop
          temp_news += "</table>"
          Me.news_container.Visible = True
        Else
          Me.news_container.Visible = False
        End If

        Me.news_label.Text = temp_news

        lDataReader.Close()
        lDataReader = Nothing
      Else
        Me.news_container.Visible = False
      End If

      DisplayYachrSpecifications = htmlOut.ToString
      DisplayYachrSpecifications = DisplayYachrSpecifications & ("<tr><td colspan='6'><hr style='margin-top:5px;margin-bottom:5px'></td></tr>")
      DisplayYachrSpecifications = DisplayYachrSpecifications & model_info.ToString()



      aircraft_status_label.Text = htmlOut2.ToString

      features_label.Text = htmlOut3.ToString

      performance_label.Text = htmlOut4.ToString

      helipad_label.Text = helipad_string.ToString


      ' model_tab_label.Text =  model_info.ToString()

    Catch SqlException

      Response.Write(SqlException)

    Finally

      SqlCommand.Dispose()

    End Try



    Return DisplayYachrSpecifications

  End Function

  Public Function DisplayYachrDetails(ByVal yacht_id As Long, ByVal int_or_ext As String, ByVal journalID As Long) As String


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut2 As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0

    Query = "SELECT distinct yd_id, yd_name, yd_description, ydc_seq_no  FROM yacht WITH(NOLOCK) "
    Query = Query & "  inner join yacht_details on yd_yt_id = yt_id and yd_journ_id = yt_journ_id "
    Query = Query & " inner join yacht_details_category on ydc_name = yd_name "
    Query = Query & "  Where yd_yt_id = " & yacht_id
    Query = Query & " AND yd_journ_id = " & journalID & " "
    Query = Query & " AND yd_type = '" & int_or_ext & "' "
    Query = Query & " AND yd_description <> '' "
    Query = Query & " and (yd_name <> 'Hanger' and yd_name <> 'Helipad') " 
    Query = Query & "  ORDER BY ydc_seq_no asc, yd_name asc, yd_description, yd_id desc "


    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("DisplayYachrDetails: " & Query & "<br>")
    End If


    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then


        Do While lDataReader.Read()


          ' If counter = 0 Then
          htmlOut.Append("<tr>")
          ' End If

          htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
          htmlOut.Append("<td valign='top' align='left' width='100%'>")

          htmlOut.Append("<span class='li'><span class='label'>" & lDataReader.Item("yd_name") & ": </span>")

          If Not IsDBNull(lDataReader.Item("yd_description")) And Not String.IsNullOrEmpty(lDataReader.Item("yd_description").ToString) Then
            htmlOut.Append("" + lDataReader.Item("yd_description").ToString + "")
          End If

          htmlOut.Append("</span>")

          htmlOut.Append("</td>")


          ' If counter = 1 Then
          htmlOut.Append("</tr>")
          'counter = 0
          '  Else
          ' counter = counter + 1
          'End If



        Loop

        htmlOut.Append("</table>")
      End If




      lDataReader.Close()

      DisplayYachrDetails = htmlOut.ToString


    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try



    Return htmlOut.ToString.Trim

  End Function
  Public Function DisplayYacht_Previous_Names_Text(ByVal yacht_id As Long, ByVal journalID As Long) As String
    DisplayYacht_Previous_Names_Text = ""

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut2 As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0

    Query = "SELECT * FROM Yacht_Previous_Names WITH(NOLOCK) "
    Query = Query & "  Where ypn_yt_id = " & yacht_id
    If journalID > 0 Then
      Query = Query & " and (ypn_date_name_changed <= '" & CDate(yacht_journal_date) & "' or ypn_date_name_changed is null)"
    End If


    Query = Query & " order by ypn_seq_no desc, ypn_date_name_changed desc, ypn_id desc  "


    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("DisplayYacht_Previous_Names_Text: " & Query & "<br>")
    End If


    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")

        Do While lDataReader.Read()

          'If counter = 0 Then
          htmlOut.Append("<tr>")
          'ElseIf counter = 2 Then
          ' htmlOut.Append("</tr><tr>")
          ' counter = 0
          '  End If

          htmlOut.Append("<td valign='top' align='left' width='50%'>")


          '   htmlOut.Append("<span class='li'><span class='label'>Previous Name: </span>")
          htmlOut.Append("<span class='li'>")

          If Not IsDBNull(lDataReader.Item("ypn_previous_name")) And Not String.IsNullOrEmpty(lDataReader.Item("ypn_previous_name").ToString) Then
            htmlOut.Append("" + lDataReader.Item("ypn_previous_name").ToString + "")
          End If

          '  htmlOut.Append("</td>")

          '  htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
          ' htmlOut.Append("<td valign='top' align='left' width='50%'>")


          ' htmlOut.Append("<span class='label'>Date of Change: </span>")

          If Not IsDBNull(lDataReader.Item("ypn_date_name_changed")) And Not String.IsNullOrEmpty(lDataReader.Item("ypn_date_name_changed").ToString) Then

            '  htmlOut.Append("" + FormatDateTime(lDataReader.Item("ypn_date_name_changed").ToString, DateFormat.ShortDate) + "")
            htmlOut.Append(" (" + Year(lDataReader.Item("ypn_date_name_changed")).ToString & ")")
          End If


          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

          counter = counter + 1

        Loop

        ' htmlOut.Append("</tr>")

        htmlOut.Append("</table>")
        DisplayYacht_Previous_Names_Text = htmlOut.ToString
      Else
        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut.Append("<tr>")

        htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
        htmlOut.Append("<td valign='top' align='left' width='100%'>")

        htmlOut.Append("<span class='li'>No Previous Names Found</span>")
        htmlOut.Append("</td></tr>")
        htmlOut.Append("</table>")
        DisplayYacht_Previous_Names_Text = htmlOut.ToString
      End If

      lDataReader.Close()




    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try



  End Function

  Public Function GetCompanies_DisplayYachtsDetails(ByVal yacht_id As String, ByVal inJournalID As Long) As String
    GetCompanies_DisplayYachtsDetails = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing

    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""
    Dim bHadCity As Boolean = False
    Dim class_color As String = ""
    Dim strCompanyTypeName As String = ""
    Dim nContactCount As Integer = 0
    Dim last_comp_name As String = ""
    Dim last_contact_type As String = ""
    Dim inactive_text As String = ""
    Dim last_contact_id As Long = 0
    Dim contact_id As Long = 0


    GetCompanies_DisplayYachtsDetails &= "<table class='company_table' cellspacing='0' cellpadding='0' width='100%'>"
    'htmlOut.Append("<tr><th align='left' class='details_header' width='50%'>COMPANY</th>")
    'htmlOut.Append("<th align='left' class='details_header'>CONTACT</th></tr>")
    'htmlOut.Append("<tr><th class='bottom'>Company</th><th class='bottom'>Contact</th></tr>")




    ' Query = "SELECT distinct comp_id, comp_name, yr_contact_type, yr_id, yct_name, comp_city, comp_state, comp_country "
    ' Query = Query & ", (Select distinct top 1 contact_first_name + ' ' + contact_last_name from yacht_reference inner join contact on contact_id = yr_contact_id where yr_comp_id = comp_id and yr_contact_id > 0 and yr_yt_id = " & yacht_id & " and yr_id = a.yr_id) as contact "
    ' Query = Query & "  FROM company WITH(NOLOCK) "
    ' Query = Query & "  inner join yacht_reference a on a.yr_comp_id = comp_id and a.yr_journ_id = comp_journ_id "
    ' Query = Query & "  inner join yacht_contact_Type on yct_code = a.yr_contact_type "
    ' Query = Query & "  Where yr_yt_id = " & yacht_id
    ' Query = Query & " AND a.yr_journ_id = 0 "
    '
    'Query = Query & "  ORDER BY  comp_id, comp_name, yr_contact_type, yr_id  desc "
    '

    sQuery = "SELECT * FROM Yacht_Reference WITH(NOLOCK)"
    sQuery &= " INNER JOIN Company WITH(NOLOCK) ON (comp_id = yr_comp_id AND comp_journ_id = yr_journ_id) "

    sQuery &= " INNER JOIN yacht_contact_Type WITH(NOLOCK) ON (yr_contact_type = yct_code)"
    sQuery &= " LEFT OUTER JOIN contact WITH(NOLOCK) on contact_id = yr_contact_id and contact_journ_id = yr_journ_id "

    sQuery &= " WHERE (yr_yt_id = " + yacht_id + " AND yr_journ_id = " & inJournalID & " "

    '  If CLng(inJournalID) = 0 Then
    ' sQuery &= " AND comp_active_flag = 'Y'"
    ' End If

    ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users

    ' If MySesState.Item("localSubscription").evoAerodexFlag Then
    'sQuery &= " AND cref_contact_type NOT IN ('93','98','99','71')"
    ' Else
    sQuery &= " AND yr_contact_type NOT IN ('71')"
    ' End If

    sQuery &= " AND comp_hide_flag = 'N')"

    sQuery &= " ORDER BY  yr_seq_no, yct_seq_no, yr_contact_type, comp_name "

    '  If MySesState.Item("debug") Then
    ' debugQuery = "<b>GetCompanies_DisplayAircraftDetails : " + sQuery + "</b><br /><br />"
    '   End If

    Try

      SqlCommand.CommandText = sQuery.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        GetCompanies_DisplayYachtsDetails &= "<tr><td valign='top' align='left' colspan='2'>"
        class_color = "light_blue"
        Do While lDataReader.Read()

          '-----------------------------------------
          '-----------------------------------------

          If Not IsDBNull(lDataReader.Item("contact_id")) Then
            contact_id = lDataReader.Item("contact_id")
          Else
            contact_id = 0
          End If

          If Trim(lDataReader.Item("comp_name").ToString) <> Trim(last_comp_name) And Trim(last_comp_name) <> "" Then
            If (Trim(last_contact_id) <> Trim(contact_id)) Or (contact_id = 0 And last_contact_id = 0) Then
              GetCompanies_DisplayYachtsDetails &= htmlOut.ToString
              last_contact_type = ""

              If class_color = "light_blue" Then
                class_color = "light_gray"
              Else
                class_color = "light_blue"
              End If
            Else
              last_comp_name = last_comp_name
            End If
          ElseIf Trim(lDataReader.Item("comp_name").ToString) = Trim(last_comp_name) And Trim(last_comp_name) <> "" Then
            ' if company is the same, yet contact has changed
            If (Trim(last_contact_id) <> Trim(contact_id)) Or (contact_id = 0 And last_contact_id = 0) Then
              GetCompanies_DisplayYachtsDetails &= htmlOut.ToString
              last_contact_type = ""

              If class_color = "light_blue" Then
                class_color = "light_gray"
              Else
                class_color = "light_blue"
              End If
            Else
              last_comp_name = last_comp_name
            End If
          Else
            last_comp_name = last_comp_name
          End If

            htmlOut.Length = 0





            strCompanyTypeName = lDataReader.Item("yct_name").ToString

            If Trim(last_contact_type) <> "" Then
              strCompanyTypeName = last_contact_type & ", " & strCompanyTypeName
            End If

            If Not String.IsNullOrEmpty(strCompanyTypeName) Then
              strCompanyTypeName = strCompanyTypeName
            Else
              strCompanyTypeName = "Additional Company"
            End If


            Dim sCompanyName As String = lDataReader.Item("comp_name").ToString.Replace(cSingleSpace, cHTMLnbsp)

            If Trim(strCompanyTypeName) = "Shipyard" Then

              shipyard_company_name.Text = "<tr>"
              shipyard_company_name.Text &= "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
              shipyard_company_name.Text &= "<td valign='top' align='left' width='100%' colspan='4'><span class='li'><span class='label'>Shipyard: </span>"
              shipyard_company_name.Text &= lDataReader.Item("comp_name")
              shipyard_company_name.Text &= "</span></td></tr>"


            End If



            strCompanyTypeName = "<h1 class='company_title'>" & strCompanyTypeName

            If lDataReader.Item("comp_active_flag").ToString = "N" And journalID = 0 Then
              strCompanyTypeName &= " <font size='-1'>(No Longer Active)</font>"
            End If

            strCompanyTypeName &= "</h1>"

            nContactCount = 0



            If nContactCount = 0 Then
              htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' border='0' class=" & class_color & ">")
              htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>" & strCompanyTypeName & "</td></tr>")
              htmlOut.Append("<tr><td valign='top' align='left' rowspan='1'  width='50%'>")
            Else
              htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' border='0' class=" & class_color & ">")
              htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>" & strCompanyTypeName & "</td></tr>")
              htmlOut.Append("<tr><td valign='top' align='left' rowspan='" + nContactCount.ToString + "'  width='50%'>")
            End If

            'If Not String.IsNullOrEmpty(inTypes) Then
            'inTypes = inTypes.Replace(cCommaDelim, cSingleForwardSlash)
            '  htmlOut.Append(inTypes.Replace("Additional Contact1", "/Additional Company") + " - ")
            '  End' If

            ' If isDisplay Then
            '<a href="#" onclick="javascript:load('DisplayCompanyDetail.aspx?compid=<%#DataBinder.Eval(Container.DataItem, "comp_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
            'htmlOut.Append("<a class='underline'  href='#' onclick='javascript:openSmallWindowJS(" & """" & "DisplayCompanyDetails.asp?CompID=" + lDataReader.Item("comp_id").ToString & "&JournID=" & inJournalID.ToString & """" & "," & """" & "CompanyDetailsWindow" & """" & ");'>" & sCompanyName)

            If journalID > 0 Then
              htmlOut.Append("<a href='#' onclick=" & """" & "javascript:load('DisplayCompanyDetail.aspx?compid=" & lDataReader.Item("comp_id").ToString & "&jid=" & lDataReader.Item("comp_journ_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;" & """" & ">" & sCompanyName)
            Else
              htmlOut.Append("<a href='#' onclick=" & """" & "javascript:load('DisplayCompanyDetail.aspx?compid=" & lDataReader.Item("comp_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;" & """" & ">" & sCompanyName)
            End If



            htmlOut.Append("</a><br />")
            'Else
            '   htmlOut.Append(sCompanyName + "<br />")
            '  End If

            If Not IsDBNull(lDataReader.Item("comp_name_alt_type")) And Not IsDBNull(lDataReader.Item("comp_name_alt")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt_type").ToString) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt").ToString) Then
                htmlOut.Append(lDataReader.Item("comp_name_alt_type").ToString.Trim + cSingleSpace + lDataReader.Item("comp_name_alt").ToString.Trim + "<br />")
              End If
            Else
              If Not IsDBNull(lDataReader.Item("comp_name_alt")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name_alt").ToString) Then
                htmlOut.Append(lDataReader.Item("comp_name_alt").ToString.Trim + "<br />")
              End If
            End If

            If Not IsDBNull(lDataReader.Item("comp_address1")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_address1").ToString) Then
              htmlOut.Append(lDataReader.Item("comp_address1").ToString.Trim + "<br />")
            End If

            If Not IsDBNull(lDataReader.Item("comp_address2")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_address2").ToString) Then
              htmlOut.Append(lDataReader.Item("comp_address2").ToString.Trim + "<br />")
            End If

            If Not IsDBNull(lDataReader.Item("comp_city")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString) Then
              htmlOut.Append(lDataReader.Item("comp_city").ToString.Trim)
              bHadCity = True
            End If

            If Not IsDBNull(lDataReader.Item("comp_state")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_state").ToString) Then
              If bHadCity Then
                htmlOut.Append(cMultiDelim + lDataReader.Item("comp_state").ToString.Trim)
              Else
                htmlOut.Append(lDataReader.Item("comp_state").ToString.Trim)
              End If
            End If

            If Not IsDBNull(lDataReader.Item("comp_zip_code")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_zip_code").ToString) Then
              htmlOut.Append("&nbsp;" + lDataReader.Item("comp_zip_code").ToString.Trim)
            End If

            If Not IsDBNull(lDataReader.Item("comp_country")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_country").ToString) Then
              htmlOut.Append("&nbsp;" + lDataReader.Item("comp_country").ToString.Trim)
            End If

            If lDataReader.Item("comp_active_flag").ToString = "N" Then

              htmlOut.Append("<br />")


              If Not IsDBNull(lDataReader.Item("comp_email_address")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_email_address").ToString) Then
                '  If isDisplay Then
                htmlOut.Append("<a href='mailto:" + lDataReader.Item("comp_email_address").ToString.Trim + "'>" + lDataReader.Item("comp_email_address").ToString.Trim + "</a><br />")
                'Else
                '  If isJFWAFW Then
                'htmlOut.Append("<a href='mailto:" + lDataReader.Item("comp_email_address").ToString.Trim + "'>" + lDataReader.Item("comp_email_address").ToString.Trim + "</a><br />")
                ' Else
                '      htmlOut.Append(lDataReader.Item("comp_email_address").ToString.Trim + "<br />")
                '  End If
                '   End If
              End If

              If Not IsDBNull(lDataReader.Item("comp_web_address")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_web_address").ToString) Then
                '  If isDisplay Then
                htmlOut.Append("<a href='http://" + lDataReader.Item("comp_web_address").ToString.Trim + "' target=_new>" + lDataReader.Item("comp_web_address").ToString.Trim + "</a><br />")
                'Else
                '   If isJFWAFW Then
                'htmlOut.Append("<a href='http://" + lDataReader.Item("comp_web_address").ToString.Trim + "' target=_new>" + lDataReader.Item("comp_web_address").ToString.Trim + "</a><br />")
                ' Else
                '     htmlOut.Append(lDataReader.Item("comp_web_address").ToString.Trim + "<br />")
                ' End If
                'End If
              End If

              ' If b_showPhone Then
              'htmlOut.Append(DisplayPhoneInfoCompany(MySesState, inCompanyID, inJournalID, False))
              ' End If

              If Not IsDBNull(lDataReader.Item("comp_fractowr_notes")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_fractowr_notes").ToString) Then
                htmlOut.Append(lDataReader.Item("comp_fractowr_notes").ToString.Trim + "<br />")
              End If

            End If



            If Not IsDBNull(lDataReader.Item("contact_id")) Then
              htmlOut.Append("</td><td valign='top' align='left'>")

              '   If Trim(last_contact_name) <> "" Then
              '     htmlOut.Append(last_contact_name & "<br>")
              '   End If

              htmlOut.Append(CommonAircraftFunctions.GetContactInfoCompany(Me.Session, CLng(lDataReader.Item("contact_id").ToString), inJournalID, False, True, False, False, aclsData_Temp, ""))
              htmlOut.Append("</td></tr>")

              last_contact_id = CLng(lDataReader.Item("contact_id").ToString)
            Else
              last_contact_id = 0
              htmlOut.Append("</td><td valign='top'>&nbsp;</td></tr>")
            End If




            htmlOut.Append("</td></tr></table>")

            last_contact_type = Replace(Replace(strCompanyTypeName, "<h1 class='company_title'>", ""), "</h1>", "")

            ' last_contact_type = lDataReader.Item("yct_name").ToString
            last_comp_name = Trim(lDataReader.Item("comp_name").ToString)


        Loop
        GetCompanies_DisplayYachtsDetails &= htmlOut.ToString

        GetCompanies_DisplayYachtsDetails &= "</td></tr>"
      End If

      lDataReader.Close()

      GetCompanies_DisplayYachtsDetails &= "</table>"


    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try


  End Function
  Public Function DisplayYacht_Previous_Names_Evo() As String
    DisplayYacht_Previous_Names_Evo = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut2 As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0
    Dim model_pic_directory_c As String


    Query = "SELECT * from evolution_configuration where lngid = 1 "

    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("evolution_configuration: " & Query & "<br>")
    End If

    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then
        Do While lDataReader.Read()


          model_pic_directory_c = lDataReader("evo_config_yacht_model_pictures_dir")
          DisplayYacht_Previous_Names_Evo = lDataReader("evo_config_yacht_pictures_dir")


        Loop

      End If

      lDataReader.Close()


    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try



    Return DisplayYacht_Previous_Names_Evo.ToString.Trim

  End Function
  Public Function DisplayYacht_Previous_Names() As String
    DisplayYacht_Previous_Names = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim htmlOut2 As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0
    Dim model_pic_directory_c As String


    Query = "SELECT * from application_configuration where aconfig_id = 1 "

    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then
        Do While lDataReader.Read()


          model_pic_directory_c = lDataReader("aconfig_yacht_model_pictures_dir")
          DisplayYacht_Previous_Names = lDataReader("aconfig_yacht_pictures_dir")


        Loop

      End If

      lDataReader.Close()


    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try



    Return DisplayYacht_Previous_Names.ToString.Trim

  End Function
  Public Function get_yacht_compliance_certs(ByVal yacht_id As Long, ByVal journalID As Long) As String
    get_yacht_compliance_certs = ""
    Dim Query As String = ""
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim counter1 As Integer = 0
    Dim total_counter As Integer = 0

    Query = " Select * from yacht_compliance "
    Query = Query & " inner join yacht_compliance_types on yct_id = yc_cert_id "
    Query = Query & " Where yc_yt_id = " & yacht_id
    Query = Query & " and yc_journ_id = " & journalID

    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")

        Do While lDataReader.Read()


          If counter1 = 0 Then
            htmlOut.Append("<tr>")
          End If

          htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
          htmlOut.Append("<td valign='top' align='left' width='65%'>")
          htmlOut.Append("<span class='li'>")

          If Not IsDBNull(lDataReader.Item("yc_cert_date")) And Not String.IsNullOrEmpty(lDataReader.Item("yc_cert_date").ToString) Then
            If Year(lDataReader.Item("yc_cert_date")) > 1900 Then
              htmlOut.Append("" + FormatDateTime(lDataReader.Item("yc_cert_date").ToString, DateFormat.ShortDate) + ": ")
            End If
          End If

          If Not IsDBNull(lDataReader.Item("yct_type")) And Not String.IsNullOrEmpty(lDataReader.Item("yct_type").ToString) Then
            htmlOut.Append("" + lDataReader.Item("yct_type").ToString + "")
          End If

          If Not IsDBNull(lDataReader.Item("yc_cert_note")) And Not String.IsNullOrEmpty(lDataReader.Item("yc_cert_note").ToString) Then
            htmlOut.Append(", " + lDataReader.Item("yc_cert_note").ToString + "")
          End If

          htmlOut.Append("</span>")

          htmlOut.Append("</td>")

          If counter1 = 1 Then
            htmlOut.Append("</tr>")
            counter1 = -1
          End If

          counter1 = counter1 + 1
          total_counter = total_counter + 1
          '  htmlOut.Append("<tr>")

          '  htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
          '  htmlOut.Append("<td valign='top' align='left' width='95%' colspan='3'>")

          '   htmlOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")

          '  If Not IsDBNull(lDataReader.Item("yct_type_description")) And Not String.IsNullOrEmpty(lDataReader.Item("yct_type_description").ToString) Then
          'htmlOut.Append("" + lDataReader.Item("yct_type_description").ToString + "")
          '   End If


          '   htmlOut.Append("</td>")
          '   htmlOut.Append("</tr>")


        Loop

        If total_counter = 1 Then
          htmlOut.Append("<td>&nbsp;</td><td>&nbsp;</td>")
        End If
        If counter1 <> 0 Then
          htmlOut.Append("</tr>")
        End If
        htmlOut.Append("</table>")

      End If

      lDataReader.Close()

    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try


    get_yacht_compliance_certs = htmlOut.ToString


  End Function

  ''' <summary>
  ''' Function to update browse button. Tried to make this as simple as possible. Sent a datatable. Only care about previous/next ac. 
  ''' Creates text link.
  ''' </summary>
  ''' <param name="dsBrowse"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function UpdateBrowseButtons(ByRef dsBrowse As DataTable) As Boolean
    Dim FilterTable As New DataTable
    Dim nTotalRecordCount As Long = 0
    Dim yachtIDNext As Long = 0
    Dim yachtIDPrev As Long = 0

    If yacht_id > 0 Then
      ' we must be browsing records
      ' find the yacht record to display 
      If Not IsNothing(dsBrowse) Then
        If dsBrowse.Rows.Count > 0 Then
          Me.currentRecord = 1
          If dsBrowse.Rows.Count > 1 Then
            For a As Integer = 0 To dsBrowse.Rows.Count - 1

              If CLng(dsBrowse.Rows(a).Item("yt_id").ToString.Trim) = yacht_id Then

                If a + 1 = dsBrowse.Rows.Count Then
                ElseIf a + 1 <= dsBrowse.Rows.Count Then
                  yachtIDNext = dsBrowse.Rows(a + 1).Item("yt_id").ToString.Trim
                End If

                If a >= 1 Then
                  currentRecord = a + 1
                  yachtIDPrev = dsBrowse.Rows(a - 1).Item("yt_id").ToString.Trim
                End If

                Exit For
              End If
            Next
          End If
        Else
          ' browseTable.Visible = False
          PreviousYachtSwap.Visible = False
          browse_label.Visible = False
          NextYachtSwap.Visible = False
          recordsOf.Visible = False
        End If

      End If

    End If

    If yachtIDPrev > 0 Then
      PreviousYachtSwap.Text = "<a href=""#"" id=""previousAC"" type=""button"" class='gray_button float_left noBefore' value="" < Previous Yacht"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayYachtDetail.aspx?yid=" & yachtIDPrev & "';"" tooltip = ""Click to View the Previous Yacht"">&#9668; <strong>Previous</strong></a>"

      PreviousYachtSwap.Visible = True
    Else
      PreviousYachtSwap.Visible = False
    End If

    currentRecLabel.Text = Me.currentRecord.ToString
    totalRecLabel.Text = dsBrowse.Rows.Count 'nTotalRecordCount.ToString
    recordsOf.Visible = True
    If dsBrowse.Rows.Count = 1 Then
      browse_label.Visible = False
      browseTableTitle.Text = ""
      recordsOf.Visible = False
    ElseIf dsBrowse.Rows.Count = 0 Then
      recordsOf.Visible = False
    End If
    If yachtIDNext > 0 Then
      NextYachtSwap.Text = "<a href=""#"" id=""nextAC"" class='gray_button' onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayYachtDetail.aspx?yid=" & yachtIDNext & "';"" value=""Next Yacht &#9658 "" tooltip = ""Click to View the Next Yacht""><strong>Next</strong> &#9658</a>"
      NextYachtSwap.Visible = True
    Else
      NextYachtSwap.Visible = False
    End If

    Return True

  End Function

  Public Function DisplayYacht_Engines(ByVal yacht_id As Long, ByVal engine_start As String, ByVal journalID As Long) As String
    DisplayYacht_Engines = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0
    Dim temp_string As String = ""
    Dim found_port_star As Boolean = False
    Dim no_total As Boolean = True

    Query = "SELECT distinct ye_engine_sernbr, ye_engine_ttsn, yem_engine_model, ye_id, yc_engine_location, ye_engine_maintenance, comp_name  FROM yacht_engines WITH(NOLOCK) "
    Query = Query & "  inner join yacht_engine_models on yem_engine_model_id = ye_engine_model_id "
    Query = Query & "  left outer join company on comp_id = yem_engine_mfr_comp_id and comp_journ_id = 0  "
    Query = Query & "  Where ye_yt_id = " & yacht_id
    Query = Query & " and ye_journ_id = " & journalID & " "

    Query = Query & "  ORDER BY ye_engine_sernbr, ye_engine_ttsn, yem_engine_model desc "

    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("DisplayYacht_Engines: " & Query & "<br>")
    End If

    Try

      SqlCommand.CommandText = Query.ToString
      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")


        htmlOut.Append(engine_start)

        htmlOut.Append("<tr><td>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td colspan='4' width='100%'>")

        htmlOut.Append("<table cellpadding='4' cellspacing='0' width='100%' border='1' bordercolor='#E8E8E8' >")



        htmlOut.Append("<tr>")
        htmlOut.Append("<td>Model Name</td>")
        htmlOut.Append("<td>Mfr Comp</td>")
        htmlOut.Append("<td>Location</td>")
        htmlOut.Append("<td>Total Time</td>")
        htmlOut.Append("<td>Ser Nbr</td>")
        htmlOut.Append("<td>Notes</td>")




        Do While lDataReader.Read()


          htmlOut.Append("<tr>")

          htmlOut.Append("<td valign='top' align='left'>")


          If Not IsDBNull(lDataReader.Item("yem_engine_model")) And Not String.IsNullOrEmpty(lDataReader.Item("yem_engine_model").ToString) Then
            htmlOut.Append("" + lDataReader.Item("yem_engine_model").ToString + "")
          End If


          htmlOut.Append("&nbsp;</td>")

          '   htmlOut.Append("<td valign='top' align='left'>")

          '   If Not IsDBNull(lDataReader.Item("yem_engine_horsepower")) And Not String.IsNullOrEmpty(lDataReader.Item("yem_engine_horsepower").ToString) Then
          'htmlOut.Append("" + lDataReader.Item("yem_engine_horsepower").ToString + "")
          '   End If

          'htmlOut.Append("&nbsp;</td>")

          htmlOut.Append("<td valign='top' align='left'>")

          If Not IsDBNull(lDataReader.Item("comp_name")) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name").ToString) Then
            htmlOut.Append("" + lDataReader.Item("comp_name").ToString + "")
          End If

          htmlOut.Append("&nbsp;</td>")


          htmlOut.Append("<td valign='top' align='left'>")

          If Not IsDBNull(lDataReader.Item("yc_engine_location")) And Not String.IsNullOrEmpty(lDataReader.Item("yc_engine_location").ToString) Then
            htmlOut.Append("" + lDataReader.Item("yc_engine_location").ToString + "")
            If Not IsNumeric(lDataReader.Item("yc_engine_location").ToString) Then
              found_port_star = True
            End If
          End If


          htmlOut.Append("&nbsp;</td>")

          htmlOut.Append("<td valign='top' align='right'>")

          If Not IsDBNull(lDataReader.Item("ye_engine_ttsn")) And Not String.IsNullOrEmpty(lDataReader.Item("ye_engine_ttsn").ToString) Then
            If CInt(lDataReader.Item("ye_engine_ttsn")) > 0 Then
              htmlOut.Append("" + lDataReader.Item("ye_engine_ttsn").ToString + "")
              no_total = False
            End If
          End If

          htmlOut.Append("&nbsp;</td>")

          htmlOut.Append("<td valign='top' align='left'>")

          If Not IsDBNull(lDataReader.Item("ye_engine_sernbr")) And Not String.IsNullOrEmpty(lDataReader.Item("ye_engine_sernbr").ToString) Then
            htmlOut.Append("" + lDataReader.Item("ye_engine_sernbr").ToString + "")
          End If

          htmlOut.Append("&nbsp;</td>")

          htmlOut.Append("<td valign='top' align='left'>")

          If Not IsDBNull(lDataReader.Item("ye_engine_maintenance")) And Not String.IsNullOrEmpty(lDataReader.Item("ye_engine_maintenance").ToString) Then
            temp_string = lDataReader.Item("ye_engine_maintenance").ToString
            temp_string = Replace(temp_string, " KW:", "<br>KW:")
            htmlOut.Append("" + temp_string + "")
          End If


          htmlOut.Append("&nbsp;</td>")


          htmlOut.Append("</tr>")


        Loop

        htmlOut.Append("</table>")
        htmlOut.Append("</td></tr>")

        htmlOut.Append("</table>")

      End If

      lDataReader.Close()


      DisplayYacht_Engines = htmlOut.ToString

      If Trim(DisplayYacht_Engines) <> "" Then
        If found_port_star = True Then ' do nothing 
        Else
          DisplayYacht_Engines = Replace(DisplayYacht_Engines, "<td>Location</td>", "<td><A href='' title='Location' name='Location'>Loc</a></td>")
        End If

        If no_total = True Then
          DisplayYacht_Engines = Replace(DisplayYacht_Engines, "<td>Total Time</td>", "<td><A href='' title='Total Time' name='Total Time'>TT</a></td>")
        End If
      End If

    Catch SqlException

      Response.Write(SqlException)

    Finally

      lDataReader.Close()
      lDataReader = Nothing

    End Try



    Return DisplayYacht_Engines.ToString.Trim

  End Function

  Public Sub ViewYachtAnalytics(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_analytics.Click
    Call DisplayAnalyticInformation()
  End Sub

  Private Sub DisplayAnalyticInformation()
    Dim ResultsTable As New DataTable
    Dim TotalRunning As Long = 0
    Dim has_stats As Boolean = False

    If InStr(view_analytics.CssClass, "blue_button") > 0 Then

      Toggle_Tabs_Visibility(False, False, False, False, False)

    Else
      Toggle_Tabs_Visibility(False, False, False, False, True)


      ResultsTable = aclsData_Temp.DisplayAnalyticInformationSummarizedByDate(Session.Item("localUser").crmUserCompanyID, 0, yacht_id, has_stats)

      If Not IsNothing(ResultsTable) Then
        If ResultsTable.Rows.Count > 0 Then
          analytic_container.Visible = True

          ' Check to see if the startup script is already registered.
          cstext1 = "data = google.visualization.arrayToDataTable([" & vbNewLine
          cstext1 += "   ['x', 'Clicks']," & vbNewLine
          For Each r As DataRow In ResultsTable.Rows
            cstext1 += "['" & MonthName(r("YTMONTH")) & "',   " & r("tcount").ToString & "]," & vbNewLine
            TotalRunning += r("tcount")
          Next
          cstext1 += "]);" & vbNewLine

          System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "FillDataAnalytics", cstext1, True)
          analytic_label.Text = crmWebClient.DisplayFunctions.CreateAnalyticsSummaryByDate(ResultsTable, Master, "", "100", False)


        Else
          analytic_label.Text = "No analytic Data"
          analytic_container.Visible = False
          view_analytics.Visible = False
        End If

        ''if the ac is for sale only, and has a DOM #
        'If status_tab_container.CssClass = "green-theme" Then
        '    If IsNumeric(DOM.Text) Then
        '        If DOM.Text > 0 Then
        '            ResultsTable = New DataTable
        '            ResultsTable = aclsData_Temp.DisplayAnalyticInformationComparingModel(CLng(aircraft_model.Text), 0, Session.Item("localUser").crmUserCompanyID, DOM.Text)
        '            If Not IsNothing(ResultsTable) Then
        '                If ResultsTable.Rows.Count > 0 Then

        '                    cstext2 = "data_bar = google.visualization.arrayToDataTable([" & vbNewLine
        '                    cstext2 += "[' ', 'My AC', 'Other AC']," & vbNewLine
        '                    For Each r As DataRow In ResultsTable.Rows
        '                        cstext2 += vbNewLine & "[' ',  " & TotalRunning & ", " & r("avgclick") & "],"
        '                    Next

        '                    cstext2 = cstext2.TrimEnd(",")

        '                    cstext2 += "]);"

        '                    System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "FillBarAnalytics", cstext2, True)
        '                End If
        '            End If
        '            System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalyticsBar", "drawBarVisualization();", True)
        '        Else
        '            toggle_for_sale_analytics.Visible = False
        '        End If
        '    Else
        '        toggle_for_sale_analytics.Visible = False
        '    End If
        'Else
        '    toggle_for_sale_analytics.Visible = False
        'End If

        '  System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalytics", "drawVisualization();", True)

      Else
        'prep for error
        'LogError("DisplayAircraftDetail.aspx.vb - DisplayAnalyticInformationSummarizedByDate() - " & " " & aclsData_Temp.class_error)
        'clear error for data layer class
        aclsData_Temp.class_error = ""
      End If
      'End Display Analytic Information
      ResultsTable.Dispose()
    End If


    System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalytics", "drawVisualization();", True)
    'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)


    analytic_update_panel.Update()

  End Sub
  Public Sub ViewYachtEvents(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_yacht_events.Click
    Dim EventsTable As New DataTable
    Dim css_string As String = ""


    If InStr(view_yacht_events.CssClass, "blue_button") > 0 Then
      Toggle_Tabs_Visibility(False, False, False, False, False)
    Else
      Toggle_Tabs_Visibility(False, True, False, False, False)

      EventsTable = aclsData_Temp.YACHT_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, yacht_id, "", "")

      If Not IsNothing(EventsTable) Then
        If EventsTable.Rows.Count > 0 Then
          events_label.Text = "<table width='100%' cellspacing='3' cellpadding='3' class='data_aircraft_grid'>"
          events_label.Text += "<tr class='header_row'>"
          events_label.Text += "<td align='left' valign='top'><b class='title' width='350'>ACTIVITY DATE/TIME</b></td>"
          events_label.Text += "<td align='left' valign='top'><b class='title'>DESCRIPTION</b></td></tr>"

          For Each r As DataRow In EventsTable.Rows
            If css_string = "alt_row" Then
              css_string = ""
            Else
              css_string = "alt_row"
            End If
            events_label.Text += "<tr class='" & css_string & "'><td align='left' valign='top'>" & r("apev_action_date") & "</td>"
            events_label.Text += "<td align='left' valign='top'>" & r("apev_subject")
            If Not IsDBNull(r("apev_description")) Then
              If Not String.IsNullOrEmpty(r("apev_description")) Then
                events_label.Text += " [" & r("apev_description") & "]"
              End If
            End If
            events_label.Text += "</td></tr>"
          Next
          events_label.Text += "</table>"
        End If
      End If
    End If
    events_update_panel.Update()
    '   picture_update_panel.Update()
  End Sub

  Public Function GetYachtPictures(ByVal yacht_id As Long, ByVal image_folder As String, ByVal journalID As Long) As String

    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As String = ""
    Dim fAcpic_subject As String = ""
    Dim imgFolder As String = ""
    Dim theImgFile As String = ""
    Dim picture_counter As Integer = 0
    Dim javascript_slideshow_begining As String = ""
    Dim javascript_slideshow_ending As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader = Nothing
    Dim SqlException As SqlClient.SqlException = Nothing

    Dim sQuery As String = ""
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim view_all_pictures As String = ""
    Dim first_picture As String = ""
    Dim temp_count As Integer = 0

    Dim yt_image_file As String = ""
    Dim temp_height As Integer = 0
    Dim temp_width As Integer = 0
    Dim zimage2 As System.Drawing.Image
    ' Dim zimage3 As System.Drawing.Image
    Dim desired_width As Integer = 500
    Dim desired_height As Integer = 142
    Dim temp_percent1 As Double = 0.0
    Dim temp_percent2 As Double = 0.0
    Dim total_width As Integer = 0
    Dim width_size_total As Integer = 740
    Dim add_pic As String = "Y"
    Dim blow_up As Boolean = False
    Dim temp_calc As Double = 0.0
    Dim atemptable As New DataTable


    sQuery = "SELECT * FROM Yacht_Pictures WITH(NOLOCK) WHERE ytpic_yt_id = " + yacht_id.ToString
    sQuery &= " AND ytpic_journ_id = " + journalID.ToString + " AND ytpic_hide_flag = 'N' ORDER BY ytpic_seq_no asc"

    If Trim(Request.Item("debug")) = "1" Then
      Response.Write("GetAircraftPictures: " & sQuery & "<br>")
    End If

    slideshow_script.Visible = False
    step_script.Visible = False

    SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    SqlConn.Open()

    SqlCommand.Connection = SqlConn
    SqlCommand.CommandType = CommandType.Text
    SqlCommand.CommandTimeout = 60

    SqlCommand.CommandText = sQuery
    SqlReader = SqlCommand.ExecuteReader()

    Try
      atemptable.Load(SqlReader)
    Catch constrExc As System.Data.ConstraintException
      Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
    End Try

    htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'>")
    htmlOut.Append("<tr><td align='left' valign='top'>")

    If atemptable.Rows.Count > 0 Then

      aircraft_picture_slideshow.Visible = True
      slideshow_script.Visible = True
      step_script.Visible = True

      If Not (IsDBNull(atemptable.Rows(0).Item("ytpic_image_type"))) Then
        If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("ytpic_image_type").ToString) Then
          fAcpic_image_type = atemptable.Rows(0).Item("ytpic_image_type").ToString.ToLower.Trim
        End If
      End If

      If Not (IsDBNull(atemptable.Rows(0).Item("ytpic_id"))) Then
        If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("ytpic_id").ToString) Then
          fAcpic_id = atemptable.Rows(0).Item("ytpic_id").ToString.Trim
        End If
      End If

      If Not (IsDBNull(atemptable.Rows(0).Item("ytpic_subject"))) Then
        If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("ytpic_subject").ToString) Then
          fAcpic_subject = atemptable.Rows(0).Item("ytpic_subject").ToString.Trim
        End If
      End If

      If fAcpic_image_type.Contains("jpg") Then

        If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
          imgFolder = "https://www.testjetnetevolution.com/pictures/yacht"
        Else
          imgFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("YachtPicturesFolderVirtualPath")
        End If

        theImgFile = imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id.Trim + Constants.cDot + fAcpic_image_type.Trim

        picture_counter = 1

        javascript_slideshow_begining = ("<div id=""slider1"" class=""sliderwrapper"">")
        javascript_slideshow_begining = javascript_slideshow_begining & ("<div id=""paginate-slider1"" class=""pagination""></div>")
        javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='contentdiv' align='center'>")

        Try

          yt_image_file = HttpContext.Current.Server.MapPath("pictures\yacht\") + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type
          zimage2 = System.Drawing.Image.FromFile(yt_image_file)
          temp_width = zimage2.Width
          temp_height = zimage2.Height
          desired_width = 510
          desired_height = 350

          Call CommonAircraftFunctions.find_image_resize_to_fit(temp_width, temp_height, desired_width, desired_height, javascript_slideshow_begining, atemptable.Rows(0), imgFolder, fAcpic_id, fAcpic_image_type, fAcpic_subject, "Yacht", yacht_id, journalID)

          javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
          javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")


          temp_width = zimage2.Width
          temp_height = zimage2.Height
          desired_height = 100

          If (temp_height > desired_height) And (temp_height > temp_width) Then
            temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
            first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
          Else
            first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
          End If


        Catch ex As Exception
          javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='350' />")
          javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
          javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")
          first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")

        End Try

      End If

      javascript_slideshow_ending = ("</td>")
      javascript_slideshow_ending = javascript_slideshow_ending & ("</tr><tr>")

      If atemptable.Rows.Count > 0 Then

        javascript_slideshow_ending = javascript_slideshow_ending & ("<td align='left' valign='top'>") '&nbsp;Additional Images ...<br />")

        javascript_slideshow_ending = javascript_slideshow_ending & ("<table cellpadding='1' cellspacing='0' bgcolor='#030b18'>")
        javascript_slideshow_ending = javascript_slideshow_ending & ("<tr><td align='left' valign='top'><div id=""mygallery"" class=""stepcarousel""><div class=""belt"">")
        javascript_slideshow_ending = javascript_slideshow_ending & first_picture

        For Each r As DataRow In atemptable.Rows
          If picture_counter = 1 Then
          Else


            fAcpic_image_type = ""
            fAcpic_id = ""
            fAcpic_subject = ""

            If Not (IsDBNull(r.Item("ytpic_image_type"))) Then
              If Not String.IsNullOrEmpty(r.Item("ytpic_image_type").ToString) Then
                fAcpic_image_type = r.Item("ytpic_image_type").ToString.ToLower.Trim
              End If
            End If

            If Not (IsDBNull(r.Item("ytpic_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("ytpic_id").ToString) Then
                fAcpic_id = r.Item("ytpic_id").ToString.Trim
              End If
            End If

            If Not (IsDBNull(r.Item("ytpic_subject"))) Then
              If Not String.IsNullOrEmpty(r.Item("ytpic_subject").ToString) Then
                fAcpic_subject = r.Item("ytpic_subject").ToString.Trim
              End If
            End If

            theImgFile = imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type

            javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='contentdiv' align='center'>")

            Try

              yt_image_file = HttpContext.Current.Server.MapPath("pictures\yacht\") + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type
              zimage2 = System.Drawing.Image.FromFile(yt_image_file)
              temp_width = zimage2.Width
              temp_height = zimage2.Height
              desired_width = 510
              desired_height = 350


              Call CommonAircraftFunctions.find_image_resize_to_fit(temp_width, temp_height, desired_width, desired_height, javascript_slideshow_begining, r, imgFolder, fAcpic_id, fAcpic_image_type, fAcpic_subject, "Yacht", yacht_id, journalID)

              javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
              javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")

              temp_width = zimage2.Width
              temp_height = zimage2.Height
              desired_height = 100

              If (temp_height > desired_height) And (temp_height > temp_width) Then
                temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" & fAcpic_subject & "' alt='" & fAcpic_subject & "' height='100' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
              Else
                javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" & fAcpic_subject & "' alt='" & fAcpic_subject & "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
              End If

            Catch ex As Exception
              javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" & fAcpic_subject & "' alt='" & fAcpic_subject & "'  height='350' />")
              javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" & fAcpic_subject & "</div>")
              javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")
              javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journalID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" & fAcpic_subject & "' alt='" & fAcpic_subject & "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")

            End Try

          End If
          picture_counter += 1
        Next

        javascript_slideshow_ending = javascript_slideshow_ending & "</div></div></td>"
        javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")
        htmlOut.Append(javascript_slideshow_begining)
        htmlOut.Append(javascript_slideshow_ending)
        htmlOut.Append("</tr>")
        htmlOut.Append("</table>")

      End If
    End If

    htmlOut.Append("</td></tr>")
    htmlOut.Append("</table>")

    Return htmlOut.ToString.Trim

  End Function

  Public Function build_column_string(ByVal value, ByVal label, ByRef col_num, ByVal format_too) As String
    build_column_string = ""
    Dim temp_string As String = ""
    Dim temp_string2 As String = ""

    If Trim(value) <> "0" Then


      If col_num = 1 Then
        build_column_string = build_column_string & "<tr>"
      End If

      build_column_string = build_column_string & "<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>"
      build_column_string = build_column_string & "<td valign='top' align='left' width='50%'><span class='li'>"

      If Trim(label) <> "" Then
        If Trim(label) = "Foreign Asking Price" Then
          build_column_string = build_column_string & "<span class='label'>Asking Price: </span>"
        Else
          build_column_string = build_column_string & "<span class='label'>" & label & ": </span>"
        End If
      End If



      If Trim(label) = "Asking Price" Then
        build_column_string = build_column_string & "$"
      End If


      If InStr(value, "(") > 0 And Trim(label) = "Foreign Asking Price" Then
        temp_string = Right(value, value.ToString.Length - InStr(value, "(") + 1)
        temp_string2 = Trim(Left(value, InStr(value, "(") - 1))
        build_column_string = build_column_string & FormatNumber(temp_string2, 0) & temp_string
      ElseIf IsNumeric(value) And Trim(label) <> "Year Mfr." And Trim(label) <> "Hull #" And Trim(label) <> "MMSI" And Trim(label) <> "IMO" And Trim(label) <> "Year Dlv." And Trim(label) <> "Official Nbr" And Trim(label) <> "Interior Refit Date" And Trim(label) <> "Exterior Refit Date" Then
        build_column_string = build_column_string & FormatNumber(value, format_too)
      ElseIf Trim(label) = "Size" Then
        If InStr(value, "-") > 0 Then
          temp_string = Left(value, InStr(value, "-") - 1)
          build_column_string = build_column_string & temp_string
        Else
          build_column_string = build_column_string & value
        End If
      Else
        build_column_string = build_column_string & value
      End If


      build_column_string = build_column_string & "</span>"
      build_column_string = build_column_string & "</td>"

      If col_num = 2 Then
        build_column_string = build_column_string & "</tr>"
      End If


      If col_num = 1 Then
        col_num = 2
      Else
        col_num = 1
      End If


    End If

  End Function

  Public Function life_cycle_stage(ByVal value As Integer) As String
    life_cycle_stage = ""

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0



    Query = " select * from yacht_lifecycle where yl_lifecyle_id = '" & value & "' "


    Try

      SqlCommand_inner.CommandText = Query.ToString
      lDataReader = SqlCommand_inner.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          life_cycle_stage = lDataReader("yl_lifecycle_name")

        Loop

      End If

      lDataReader.Close()
      lDataReader = Nothing



    Catch SqlException

      Response.Write(SqlException)

    Finally

    End Try


  End Function
  'Public Function find_background_image() As String
  '    'find_background_image = ""

  '    'Dim SqlException As System.Data.SqlClient.SqlException = Nothing
  '    'Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
  '    'Dim htmlOut As StringBuilder = New StringBuilder()
  '    'Dim Query As String = ""
  '    'Dim counter As Integer = 0

  '    'If InStr(Server.MapPath(""), "C:\inetpub\wwwroot\Evolution\JetnetWeb", CompareMethod.Text) > 0 Then
  '    '    ' SqlConnection.ConnectionString = My.Settings.TEST_INHOUSE_MSSQL
  '    '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"

  '    'ElseIf InStr(Server.MapPath(""), "jetnet12", CompareMethod.Text) > 0 Then
  '    '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"
  '    'Else
  '    '    SqlConnection.ConnectionString = "Data Source=128.1.21.200;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=sa;Password=moejive;MultipleActiveResultSets=True"
  '    'End If

  '    'SqlConnection.Open()

  '    'Query = " select top 1 evoback_id from evolution_backgrounds where evoback_product_yacht_flag = 'Y' and evoback_active_flag = 'Y' order by newid() "


  '    'Try

  '    '    SqlCommand_inner.CommandText = Query.ToString
  '    '    lDataReader = SqlCommand_inner.ExecuteReader()

  '    '    If lDataReader.HasRows Then

  '    '        Do While lDataReader.Read()

  '    '            find_background_image = lDataReader("evoback_id") & ".jpg"

  '    '        Loop

  '    '    End If

  '    '    lDataReader.Close()
  '    '    lDataReader = Nothing



  '    'Catch SqlException

  '    '    Response.Write(SqlException)

  '    'Finally

  '    '    SqlConnection.Close()
  '    '    SqlConnection.Dispose()
  '    'End Try


  'End Function


  Public Function yn_to_yes_no(ByVal temp_yn As String) As String
    yn_to_yes_no = ""

    If temp_yn = "y" Or temp_yn = "Y" Then
      yn_to_yes_no = "Yes"
    Else
      yn_to_yes_no = "No"
    End If

  End Function

  Public Function ownership_type(ByVal temp_type As String) As String
    ownership_type = ""

    If temp_type = "w" Or temp_type = "W" Then
      ownership_type = "Wholly Owned"
    ElseIf temp_type = "f" Or temp_type = "F" Then
      ownership_type = "Fractional Ownership"
    ElseIf temp_type = "s" Or temp_type = "S" Then
      ownership_type = "Shared Ownership"
    End If

  End Function
  Public Function convert_metric_to_us(ByVal metric As Double) As String
    convert_metric_to_us = ""

    Dim english As Double
    Dim feet As Integer
    Dim inches As Integer


    english = (metric * 3.28084)
    feet = Int(english)
    inches = (english - feet) * 12
    inches = FormatNumber(inches, 0)

    convert_metric_to_us = feet & "' " & inches & "'' "
  End Function

  Public Function convert_kg_to_lbs(ByVal kg As Double) As String
    convert_kg_to_lbs = ""

    Dim english As Double  

    english = (kg * 2.20462262)  
    convert_kg_to_lbs = english
  End Function



  Private Sub Toggle_Tabs_Visibility(ByVal MapVis As Boolean, ByVal EventsVis As Boolean, ByVal FoldersVis As Boolean, ByVal NotesVis As Boolean, ByVal analytics As Boolean)
    If MapVis Then
      'Toggle on
    Else
      'Toggle off
    End If

    If NotesVis Then
      'toggle on
      'view_notes.Text = "Close Notes/Actions"
      closeNotes.Visible = True
      view_notes.CssClass = "blue_button float_left noBefore"
      Notes.CssClass = "blue-theme"
      Notes.Visible = True
      Reminders.CssClass = "blue-theme"
      Reminders.Visible = True
      notes_update_panel.Update()
    Else 'toggle off
      closeNotes.Visible = False
      view_notes.CssClass = "gray_button float_left noBefore"
      'view_notes.Text = "View Notes/Actions"
      Notes.CssClass = "dark-theme"
      Notes.Visible = False
      Reminders.CssClass = "dark-theme"
      Reminders.Visible = False
      notes_update_panel.Update()
    End If

    If EventsVis = True Then
      'Set Folders since they're opened.
      closeEvents.Visible = True
      'view_yacht_events.Text = "Close Events"
      view_yacht_events.CssClass = "blue_button float_left"
      events_container.CssClass = "blue-theme"
      events_container.Visible = True
    Else
      'Events Closed.
      closeEvents.Visible = False
      'view_yacht_events.Text = "View Events"
      view_yacht_events.CssClass = "gray_button float_left"
      events_container.CssClass = "dark-theme"
      events_container.Visible = False
    End If

    If FoldersVis Then
      'Set Folders since they're opened.
      'view_folders.Text = "Close Folders"
      closeFolders.Visible = True
      aircraft_picture_slideshow.Visible = False
      view_folders.CssClass = "blue_button float_left"
      folders_container.CssClass = "blue-theme"
      folders_container.Visible = True
    Else
      'Folders Closed.
      closeFolders.Visible = False
      view_folders.CssClass = "gray_button float_left"
      folders_container.CssClass = "dark-theme"
      'view_folders.Text = "View Folders"
      folders_container.Visible = False
      aircraft_picture_slideshow.Visible = True
    End If


    If analytics Then
      closeAnalytics.Visible = True
      'view_analytics.Text = "Close Analytics"
      view_analytics.CssClass = "blue_button float_left"
      analytic_container.CssClass = "blue-theme"
      analytic_container.Visible = True
    Else
      closeAnalytics.Visible = False
      'view_analytics.Text = "View Analytics"
      view_analytics.CssClass = "gray_button float_left"
      analytic_container.CssClass = "dark-theme"
      analytic_container.Visible = False
    End If



  End Sub

  Public Function Find_Company_Name_MFR(ByVal comp_id As Long) As String
    Find_Company_Name_MFR = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0



    Query = "SELECT comp_name, comp_id FROM company WITH(NOLOCK) "
    Query = Query & "  Where comp_id = " & comp_id & " and comp_journ_id = 0 "


    Try

      SqlCommand_inner.CommandText = Query.ToString
      lDataReader = SqlCommand_inner.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          Find_Company_Name_MFR = "<a href='#' onclick=" & """" & "javascript:load('DisplayCompanyDetail.aspx?compid=" & lDataReader("comp_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;" & """" & ">" & lDataReader("comp_name")

        Loop

      End If

      lDataReader.Close()
      lDataReader = Nothing


    Catch SqlException

      Response.Write(SqlException)

    Finally


    End Try

  End Function

  Public Function Find_Company_Name(ByVal comp_id As Long) As String
    Find_Company_Name = ""


    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim Query As String = ""
    Dim counter As Integer = 0



    Query = "SELECT comp_name FROM company WITH(NOLOCK) "
    Query = Query & "  Where comp_id = " & comp_id & " and comp_journ_id = 0 "


    Try

      SqlCommand_inner.CommandText = Query.ToString
      lDataReader = SqlCommand_inner.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          Find_Company_Name = lDataReader("comp_name")

        Loop

      End If

      lDataReader.Close()
      lDataReader = Nothing


    Catch SqlException

      Response.Write(SqlException)

    Finally


    End Try

  End Function

  Public Sub ViewYachtFolders(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_folders.Click
    If InStr(view_folders.CssClass, "blue_button") > 0 Then
      Toggle_Tabs_Visibility(False, False, False, False, False)
    Else
      Toggle_Tabs_Visibility(False, False, True, False, False)
    End If

  End Sub


  Public Sub ViewYachtNotes(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_notes.Click
    If InStr(view_notes.CssClass, "blue_button") > 0 Then
      Toggle_Tabs_Visibility(False, False, False, False, False)
    Else
      Toggle_Tabs_Visibility(False, False, False, True, False)
    End If

  End Sub
End Class