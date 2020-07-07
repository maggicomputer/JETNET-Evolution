Partial Public Class YachtTheme
  Inherits System.Web.UI.MasterPage

  Public aclsData_Temp As New clsData_Manager_SQL
  Public error_string As String = ""
  Dim temptable As New DataTable
  Public bEnableChat As Boolean
  Public script_version As String = ""

  Public Sub UpdateHelpLink(ByVal url As String)
    WelcomeUser1.ChangeHelpLink(url)
  End Sub
  Public Sub SetContainerClass(ByVal ClassName As String)
    pageSizing.Attributes.Remove("class")
    pageSizing.Attributes.Add("class", ClassName)

  End Sub
  Public Sub SetDefaultButtion(ByVal buttonID As String)
    form1.DefaultButton = buttonID
  End Sub
  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    aclsData_Temp = New clsData_Manager_SQL
    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
    aclsData_Temp.client_DB = ""

    Session.Item("localUser").crmUser_DebugText = ""
    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")

    Dim bHasMaster As Boolean = True

    If Not IsNothing(Request.Item("noMaster")) Then
      If Not String.IsNullOrEmpty(Request.Item("noMaster").ToString) Then
        bHasMaster = CBool(Request.Item("noMaster").ToString.Trim)
      End If
    End If

    Dim SMgr As ScriptManager
    If ScriptManager.GetCurrent(Page) Is Nothing Then
      Throw New Exception("ScriptManager not found.")
    Else
      SMgr = ScriptManager.GetCurrent(Page)
    End If

    script_version = My.Settings.SCRIPT_VERSION.ToString

    Dim link As HtmlLink = New HtmlLink()
    link.Attributes.Add("type", "text/css")
    link.Attributes.Add("rel", "stylesheet")
    link.Attributes.Add("href", "/EvoStyles/stylesheets/additional_styles.css" + script_version)
    Page.Header.Controls.Add(link)

    Dim link1 As HtmlLink = New HtmlLink()
    link1.Attributes.Add("type", "text/css")
    link1.Attributes.Add("rel", "stylesheet")
    link1.Attributes.Add("href", "/EvoStyles/stylesheets/tableThemes.css" + script_version)
    Page.Header.Controls.Add(link1)

    Dim link2 As HtmlLink = New HtmlLink()
    link2.Attributes.Add("type", "text/css")
    link2.Attributes.Add("rel", "stylesheet")
    link2.Attributes.Add("href", "/EvoStyles/stylesheets/header_styles.css" + script_version)
    Page.Header.Controls.Add(link2)

    Dim SRef As ScriptReference = New ScriptReference()
    SRef.Path = "http://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
    SMgr.Scripts.Add(SRef)

    Dim SRef1 As ScriptReference = New ScriptReference()
    SRef1.Path = "http://code.jquery.com/ui/1.12.1/jquery-ui.js"
    SMgr.Scripts.Add(SRef1)

    Dim SRef2 As ScriptReference = New ScriptReference()
    SRef2.Path = "http://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
    SMgr.Scripts.Add(SRef2)

    Dim SRef3 As ScriptReference = New ScriptReference()
    SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
    SMgr.Scripts.Add(SRef3)

    Dim SRef4 As ScriptReference = New ScriptReference()
    SRef4.Path = "~/common/common_functions.js" + script_version
    SMgr.Scripts.Add(SRef4)

    Dim SRef5 As ScriptReference = New ScriptReference()
    SRef5.Path = "~/common/jquery.number.min.js"
    SMgr.Scripts.Add(SRef5)

    Dim SRef6 As ScriptReference = New ScriptReference()
    SRef6.Path = "~/common/date.js"
    SMgr.Scripts.Add(SRef6)

    Dim SRef7 As ScriptReference = New ScriptReference()
    SRef7.Path = "~/common/daterangepicker.jQuery.js" + script_version
    SMgr.Scripts.Add(SRef7)

    Dim SRef8 As ScriptReference = New ScriptReference()
    SRef8.Path = "~/common/header_scripts.js" + script_version
    SMgr.Scripts.Add(SRef8)

    Dim SRef9 As ScriptReference = New ScriptReference()
    SRef9.Path = "~/common/chosen.jquery.min.js"
    SMgr.Scripts.Add(SRef9)

    Dim SRef10 As ScriptReference = New ScriptReference()
    SRef10.Path = "~/common/jQDateRangeSlider-min.js"
    SMgr.Scripts.Add(SRef10)

    Dim SRef11 As ScriptReference = New ScriptReference()
    SRef11.Path = "~/abiFiles/js/superfish.min.js"
    SMgr.Scripts.Add(SRef11)

    Dim SRef12 As ScriptReference = New ScriptReference()
    SRef12.Path = "https://www.gstatic.com/charts/loader.js"
    SMgr.Scripts.Add(SRef12)

    Dim SRef13 = New ScriptReference()
    SRef13.Path = "~/common/jquery.slicknav.min.js"
    SMgr.Scripts.Add(SRef13)

    If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.EVOLUTION Then

      If bHasMaster And (CBool(My.Settings.enableChat)) Then

        ChatManager.CheckAndInitChat(False, bEnableChat)

        notifyChatUserPanel.Visible = False

        If bEnableChat Then

          Dim SvcRef As ServiceReference = New ServiceReference()
          SvcRef.Path = "~/chat/Services/chatServices.svc"

          SMgr.Services.Add(SvcRef)

          notifyChatUserPanel.Visible = True

        End If
      End If

    End If

  End Sub
  Public Sub RemoveLine()
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      'Clear this Automatic Refresh Time Integer: This clears every time the masterpage is hit in a refresh.
      Session.Item("AutomaticRefreshTime") = 0

      'Special change for just safari, making the dropdowns work 
      If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
        Request.Browser.Adapters.Clear()
      End If


      'querying to display the view information for the menu item for Views
      If Not IsNothing(Session.Item("ViewDataTable")) Then
        temptable = Session.Item("ViewDataTable")
      Else
        temptable = aclsData_Temp.Display_View_Information(0)
        Session.Item("ViewDataTable") = temptable
      End If

      'Filling that menu item.
      Menu1.Items(1).ChildItems.Clear()
      If Not IsNothing(temptable) Then
        If temptable.Rows.Count > 0 Then
          For Each r As DataRow In temptable.Rows
            Dim TempMenu As New MenuItem
            TempMenu.Text = r("evoview_title").ToString
            TempMenu.Value = r("evoview_id").ToString
            'TempMenu.NavigateUrl = "../view_template.aspx?amod_id=300"
            Menu1.Items(1).ChildItems.Add(TempMenu)
          Next
        End If
      Else
        LogError(aclsData_Temp.class_error)
      End If

      'If Not Page.IsPostBack Then
      '    'Configures background
      '    Configure_Background()
      'End If

    End If

    If Not Page.IsPostBack Then
      If Session.Item("localSubscription").crmBusiness_Flag = False Then
        Menu1.Items(Menu1.Items.Count - 1).ChildItems.RemoveAt(5)
      End If
    End If

  End Sub
  Private Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
    Dim _context As HttpContext = HttpContext.Current
    Dim ViewID As Long = 0
    Dim ViewName As String = ""

    If Not IsNothing(e.Item.Parent) Then
      Select Case e.Item.Parent.Value
        Case "Views"
          ViewID = e.Item.Value
          ViewName = e.Item.Text
      End Select

      Response.Redirect("Yacht_View_Template.aspx?ViewID=" + ViewID.ToString + "&ViewName=" + ViewName)

    End If
  End Sub
  Public Sub Set_Active_Tab(ByVal active_tabIndex As Long)
    Menu1.Items(active_tabIndex).Selected = True
  End Sub
  Public Sub SetStatusText(ByVal text As String, Optional ByRef aircraftSearch As Boolean = False)
    WelcomeUser1.SetStatusText(text)
  End Sub
  ''' <summary>
  ''' Just a temporary function to remove status notification
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub RemoveStatusNotification()
    Response.Write("this closed the notification")
  End Sub
  ''' <summary>
  ''' This is blank until the logging is put in
  ''' </summary>
  ''' <param name="ex"></param>
  ''' <remarks></remarks>
  Public Sub LogError(ByVal ex As String)
    'Error Logging Function
    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex, "'", "''"), Nothing, 0, 0, 0, 0, 0)
  End Sub
  ''' <summary>
  ''' Fills up Gray Menu Bar with a title text
  ''' </summary>
  ''' <param name="title_string">Acceptable title</param>
  ''' <remarks></remarks>
  Public Sub Gray_Title_Bar(ByVal title_string As String)
    gray_menu_title.Text = title_string
  End Sub
  ''' <summary>
  ''' Fills up Gray Menu Bar Buttons
  ''' </summary>
  ''' <param name="title_string"></param>
  ''' <remarks></remarks>
  Public Sub Gray_Title_Buttons(ByVal title_string As String)
    gray_menu_buttons.Text = title_string
  End Sub
  ''' <summary>
  ''' Needs true if the view is being viewed in the main template, false if a standalone.
  ''' </summary>
  ''' <param name="visibility"></param>
  ''' <remarks></remarks>
  Public Sub MenuBarVisibility(ByVal visibility As Boolean)
    ' WelcomeUser1.Visible = visibility
    Menu1.Visible = visibility
    gray_bar_container.Visible = visibility
  End Sub
  Public Sub ToggleWelcomeHeader(ByVal visibility As Boolean)
    'WelcomeUser1.Visible = visibility
  End Sub
  Public Sub RemoveWhiteBackground(ByVal remove As Boolean)
    If remove = True Then
      container_white_background_div.Attributes("class") = "sixteen columns"
      container_border.Attributes("class") = ""
      content_clear.Attributes("class") = "display_none"
    End If
  End Sub
  ''' <summary>
  ''' Configures background. With an ID of 0, it means they have a random picture background. This means
  ''' they need to look up the background (if they haven't already). So we look it up, then store it and update the picture
  ''' with the new background. Query isn't run on postback but update of picture is.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub Configure_Background()
    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
      If Session.Item("localUser").crmLocalUser_Background_ID = 0 Then 'this means the background is random, this needs to be looked up once, and then set.
        If Not Page.IsPostBack Then
          temptable = aclsData_Temp.GetBackgroundImages()
          If Not IsNothing(temptable) Then
            If temptable.Rows.Count > 0 Then
              Session.Item("localUser").crmLocalUser_Background_ID = temptable.Rows(0).Item("evoback_id")
            End If
          End If
        End If
      End If
    End If
    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
      If Session.Item("localUser").crmLocalUser_Background_ID <> 0 Then
        background_image.Text = "<img src=""images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
      End If
    End If
  End Sub
End Class