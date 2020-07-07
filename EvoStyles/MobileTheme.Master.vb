Partial Public Class MobileTheme
  Inherits System.Web.UI.MasterPage
  Public aclsData_Temp As New clsData_Manager_SQL
  Public error_string As String = ""

  Dim TempTable As New DataTable
  Public script_version As String = ""

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    aclsData_Temp = New clsData_Manager_SQL
    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
    aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

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
    link1.Attributes.Add("href", "/EvoStyles/stylesheets/header_styles.css" + script_version)
    Page.Header.Controls.Add(link1)

    Dim link2 As HtmlLink = New HtmlLink()
    link2.Attributes.Add("type", "text/css")
    link2.Attributes.Add("rel", "stylesheet")
    link2.Attributes.Add("href", "/EvoStyles/stylesheets/tableThemes.css" + script_version)
    Page.Header.Controls.Add(link2)

    Dim link3 As HtmlLink = New HtmlLink()
    link3.Attributes.Add("type", "text/css")
    link3.Attributes.Add("rel", "stylesheet")
    link3.Attributes.Add("href", "/EvoStyles/stylesheets/additional_mobile_styles.css" + script_version)
    Page.Header.Controls.Add(link3)

    Dim SRef As ScriptReference = New ScriptReference()
    SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
    SMgr.Scripts.Add(SRef)


    Dim SRef1 As ScriptReference = New ScriptReference()
    SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
    SMgr.Scripts.Add(SRef1)

    SRef = New ScriptReference()
    SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
    SMgr.Scripts.Add(SRef)

    Dim SRef3 As ScriptReference = New ScriptReference()
    SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
    SMgr.Scripts.Add(SRef3)

    Dim SRef4 As ScriptReference = New ScriptReference()
    SRef4.Path = "~/common/common_functions.js" + script_version
    SMgr.Scripts.Add(SRef4)

    Dim SRef5 As ScriptReference = New ScriptReference()
    SRef5.Path = "~/abiFiles/js/superfish.min.js"
    SMgr.Scripts.Add(SRef5)

    Dim SRef6 = New ScriptReference()
    SRef6.Path = "~/common/jquery.slicknav.min.js"
    SMgr.Scripts.Add(SRef6)

    Dim SRef7 = New ScriptReference()
    SRef7.Path = "~/common/chosen.jquery.min.js"
    SMgr.Scripts.Add(SRef7)

    Dim SRef8 = New ScriptReference()
    SRef8.Path = "~/common/jQDateRangeSlider-min.js"
    SMgr.Scripts.Add(SRef8)

    Dim SRef9 = New ScriptReference()
    SRef9.Path = "~/common/header_scripts.js" + script_version
    SMgr.Scripts.Add(SRef9)

    Dim SRef10 = New ScriptReference()
    SRef10.Path = "https://use.fontawesome.com/52d48867c2.js"
    SMgr.Scripts.Add(SRef10)

    Dim SRef11 = New ScriptReference()
    SRef11.Path = "/abiFiles/js/tmlazyload.js"
    SMgr.Scripts.Add(SRef11)

    Dim SRef12 As ScriptReference = New ScriptReference()
    SRef12.Path = "https://www.gstatic.com/charts/loader.js"
    SMgr.Scripts.Add(SRef12)

    Dim SRef13 As ScriptReference = New ScriptReference()
    SRef13.Path = "~/common/jquery.number.min.js"
    SMgr.Scripts.Add(SRef13)

    Dim SRef14 As ScriptReference = New ScriptReference()
    SRef14.Path = "~/common/date.js"
    SMgr.Scripts.Add(SRef14)

    Dim SRef15 As ScriptReference = New ScriptReference()
    SRef15.Path = "~/common/daterangepicker.jQuery.js" + script_version
    SMgr.Scripts.Add(SRef15)

  End Sub
  Public Sub RemoveLine()
  End Sub
  Public Sub SetPageTitle(ByVal title As String)
    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(title)
  End Sub
  Public Sub SetContainerClass(ByVal ClassName As String)
    pageSizing.Attributes.Remove("class")
    pageSizing.Attributes.Add("class", "container MaxWidthRemove")
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")
      'Configures background
      Configure_Background()
      If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
        modelValuesLink.Visible = False
        eventActivityLink.Attributes.Clear()
      End If

      If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
        modelValuesLink.Visible = True
        ' eventActivityLink.Attributes.Add("class", "dividerRight")
      End If

      If UCase(Request.RawUrl.ToString()).Contains("/VIEW_TEMPLATE.ASPX") Then
        If Not String.IsNullOrEmpty(Trim(Request("ViewID"))) Then
          If IsNumeric(Trim(Request("ViewID"))) Then
            If Trim(Request("ViewID")) = "1" Then
              isView.Text = "true"
            End If
          End If
        End If
        bodyTag.Attributes.Clear()
        bodyTag.Attributes.Add("class", "lowerLevel")
      ElseIf UCase(Request.RawUrl.ToString()).Contains("/HOME.ASPX") Then
        container_white_background_div.Visible = False
        bodyTag.Attributes.Clear()
        bodyTag.Attributes.Add("class", "homePage")
      End If

      If UCase(Request.RawUrl.ToString()).Contains("/HOME.ASPX") Then
        SetUpSuperFishMenu(False)
      Else
        SetUpSuperFishMenu(True)
      End If

    End If

  End Sub
  Public Sub ToggleWelcomeHeader(ByVal visibility As Boolean)
  End Sub
  Public Sub Set_Active_Tab(ByVal active_tabIndex As Long)
    'Menu1.Items(active_tabIndex).Selected = True
  End Sub
  Public Sub SetDefaultButtion(ByVal buttonID As String)
    form1.DefaultButton = buttonID
  End Sub
  Public Sub SetStatusText(ByVal text As String, Optional ByRef aircraftSearch As Boolean = False)
    WelcomeUser1.SetStatusText(text, aircraftSearch)
  End Sub
  Public Function ReturnStatusText() As String
    Return WelcomeUser1.evo_message_text.Text
  End Function

  ''' <summary>
  ''' Remove status notification
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub RemoveStatusNotification()
    Response.Write("this closed the notification")
  End Sub
  '''' <summary>
  '''' Configures background. With an ID of 0, it means they have a random picture background. This means
  '''' they need to look up the background (if they haven't already). So we look it up, then store it and update the picture
  '''' with the new background. Query isn't run on postback but update of picture is.
  '''' </summary>
  '''' <remarks></remarks>
  Private Sub Configure_Background()
    Trace.Write("Start Configure_Background EvoTheme.Master.vb" + Now.ToString)

    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
      If Session.Item("localUser").crmLocalUser_Background_ID = 0 Then 'this means the background is random, this needs to be looked up once, and then set.
        If Not Page.IsPostBack Then
          TempTable = aclsData_Temp.GetBackgroundImages()
          If Not IsNothing(TempTable) Then
            If TempTable.Rows.Count > 0 Then
              Session.Item("localUser").crmLocalUser_Background_ID = TempTable.Rows(0).Item("evoback_id")
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
    Trace.Write("End Configure_Background EvoTheme.Master.vb" + Now.ToString)

  End Sub
  Public Sub UpdateHelpLink(ByVal url As String)
    WelcomeUser1.ChangeHelpLink(url)
  End Sub

  Public Sub AddExtraButtons(ByVal ExtraButtonsString As String)
    WelcomeUser1.SetExtraButtons(ExtraButtonsString)
  End Sub
  ''' <summary>
  ''' Error logging.
  ''' </summary>
  ''' <param name="ex"></param>
  ''' <remarks></remarks>
  Public Sub LogError(ByVal ex As String)
    'Error Logging Function
    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex, "'", "''"), Nothing, 0, 0, 0, 0, 0)
  End Sub

  ''' <summary>
  ''' Needs true if the view is being viewed in the main template, false if a standalone.
  ''' </summary>
  ''' <param name="visibility"></param>
  ''' <remarks></remarks>
  Public Sub MenuBarVisibility(ByVal visibility As Boolean)
    gray_bar_container.Visible = visibility
    'WelcomeUser1.ToggleLogoutButton(visibility)
    WelcomeUser1.ToggleStandalone(visibility)
  End Sub

  Public Sub RemoveWhiteBackground(ByVal remove As Boolean)
    If remove = True Then
      container_white_background_div.Attributes("class") = "sixteen columns"
      container_border.Attributes("class") = ""
      content_clear.Attributes("class") = "display_none"
      gray_bar_container.Attributes.Remove("class")
      gray_bar_container.Attributes("class") = "sixteen columns"
    End If
  End Sub

  Public Sub SetUpSuperFishMenu(ByRef Visible As Boolean)
    If Visible Then
      Dim jsStr As String = ""
      '<!--Initializing Superfish Menu-->

      'jsStr += vbCrLf + " jQuery(function($) {"
      'jsStr += vbCrLf + "$('#module-93')"

      'jsStr += vbCrLf + ".superfish({"
      'jsStr += vbCrLf + "hoverClass:  'sfHover',"
      'jsStr += vbCrLf + "pathClass:  'overideThisToUse',"
      'jsStr += vbCrLf + "pathLevels: 1,"
      'jsStr += vbCrLf + "delay: 500,"
      'jsStr += vbCrLf + "animation: { opacity: 'show', height: 'show' },"
      'jsStr += vbCrLf + "speed:  'normal',"
      'jsStr += vbCrLf + "speedOut:  'fast',"
      'jsStr += vbCrLf + "autoArrows: false,"
      'jsStr += vbCrLf + " disableHI: false,"
      'jsStr += vbCrLf + "useClick: 0,"
      'jsStr += vbCrLf + "easing: ""swing"","
      'jsStr += vbCrLf + "onInit: function() { },"
      'jsStr += vbCrLf + "onBeforeShow: function() { },"
      'jsStr += vbCrLf + "onShow: function() { },"
      'jsStr += vbCrLf + "onHide: function() { },"
      'jsStr += vbCrLf + "onIdle: function() { }"
      'jsStr += vbCrLf + "})"
      'jsStr += vbCrLf + ".mobileMenu({"
      'jsStr += vbCrLf + "defaultText: ""Navigate to..."","
      'jsStr += vbCrLf + "className: ""select-menu"","
      'jsStr += vbCrLf + "subMenuClass: ""sub-menu"""
      'jsStr += vbCrLf + "});"

      'jsStr += vbCrLf + "var ismobile = navigator.userAgent.match(/(iPhone)|(iPod)|(android)|(webOS)/i);"
      'jsStr += vbCrLf + "if (ismobile) {"
      'jsStr += vbCrLf + "$('#module-93').sftouchscreen();"
      'jsStr += vbCrLf + "}"
      'jsStr += vbCrLf + "$('.btn-sf-menu').click(function() {"
      'jsStr += vbCrLf + "$('#module-93').toggleClass('in');"
      'jsStr += vbCrLf + "});"
      'jsStr += vbCrLf + "$('#module-93').parents('[id*=""-row""]').scrollToFixed({ minWidth: 768 });"
      ''jsStr += vbCrLf + "});"
      ''<!--End Superfish Initialization-->
      'If Not Page.ClientScript.IsStartupScriptRegistered("jqSuperFish") Then
      '  System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jqSuperFish", jsStr.ToString, True)
      'End If
      ClosenavBG.Visible = False
    Else
      ClosenavBG.Attributes.Remove("class")
      ClosenavBG.Attributes.Add("class", "KeepOpennavBG")
      clickme.Attributes.Add("class", "display_none")
      container_border.Attributes.Remove("class")
      container_border.Attributes.Add("class", "content_padding")

    End If
  End Sub
End Class