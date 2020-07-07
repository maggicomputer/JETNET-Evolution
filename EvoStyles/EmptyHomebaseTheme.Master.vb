' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/EvoStyles/EmptyHomebaseTheme.Master.vb $
'$$Author: Amanda $
'$$Date: 6/17/20 2:57p $
'$$Modtime: 6/17/20 2:55p $
'$$Revision: 9 $
'$$Workfile: EmptyHomebaseTheme.Master.vb $
'
' ********************************************************************************

Partial Public Class EmptyHomebaseTheme
  Inherits System.Web.UI.MasterPage

  Public aclsData_temp As clsData_Manager_SQL = Nothing
  Public error_string As String = ""
  Dim temptable As New DataTable
  Public script_version As String = ""

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    aclsData_temp = New clsData_Manager_SQL
    aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase")
    aclsData_temp.client_DB = Session.Item("jetnetServerNotesDatabase")

    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")

    Dim SMgr As ScriptManager
    If ScriptManager.GetCurrent(Page) Is Nothing Then
      Throw New Exception("ScriptManager not found.")
    Else
      SMgr = ScriptManager.GetCurrent(Page)
    End If

        'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        SetContainerClass("container MaxWidthRemove") 'set full width page
        'End If

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
    SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
    SMgr.Scripts.Add(SRef)

    Dim SRef1 As ScriptReference = New ScriptReference()
    SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
    SMgr.Scripts.Add(SRef1)

    Dim SRef2 As ScriptReference = New ScriptReference()
    SRef2.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
    SMgr.Scripts.Add(SRef2)

    Dim SRef3 As ScriptReference = New ScriptReference()
    SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
    SMgr.Scripts.Add(SRef3)

    Dim SRef4 As ScriptReference = New ScriptReference()
    SRef4.Path = "~/common/common_functions.js" + script_version
    SMgr.Scripts.Add(SRef4)

    Dim SRef5 As ScriptReference = New ScriptReference()
    SRef5.Path = "~/common/date.js"
    SMgr.Scripts.Add(SRef5)

    Dim SRef6 As ScriptReference = New ScriptReference()
    SRef6.Path = "~/common/daterangepicker.jQuery.js" + script_version
    SMgr.Scripts.Add(SRef6)

    Dim SRef7 = New ScriptReference()
    SRef7.Path = "~/common/header_scripts.js" + script_version
    SMgr.Scripts.Add(SRef7)

    Dim SRef8 = New ScriptReference()
    SRef8.Path = "~/common/chosen.jquery.min.js"
    SMgr.Scripts.Add(SRef8)

    Dim SRef9 = New ScriptReference()
    SRef9.Path = "~/common/jquery.slicknav.min.js"
    SMgr.Scripts.Add(SRef9)

    Dim SRef10 As ScriptReference = New ScriptReference()
    SRef10.Path = "~/common/jquery.number.min.js"
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

  End Sub
    Public Sub UpdateHelpLink(ByVal url As String)
        updateHelp.Text = "<a href=""" & url & """ target=""blank""><img src=""images/help-circle.svg"" alt=""Help"" /></a>"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Trace.Write("Start Page_load EmptyHomebaseTheme.Master.vb" + Now.ToString)

    logo.ImageUrl = "~/images/homebase.png"

    Session.Item("localUser").crmLocalUser_Background_ID = 11
    'Configures background
    Configure_Background()

    If Not Page.IsPostBack Then

    End If

    Trace.Write("End Page_load EmptyHomebaseTheme.Master.vb" + Now.ToString)

  End Sub
    Public Sub RemoveLine()
    End Sub
    Public Sub Swap_Logo_Image(ByVal url As String)
    logo.ImageUrl = url
  End Sub

  Public Sub SetDefaultButtion(ByVal buttonID As String)
    form1.DefaultButton = buttonID
  End Sub

  Public Sub LogError(ByVal ex As String)
    'Error Logging Function
    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex, "'", "''"), Nothing, 0, 0, 0, 0, 0)
  End Sub

  Public Sub SetPageText(ByVal text As String)
    PageText.Text = text
  End Sub

  Public Sub SetPageTitle(ByVal title As String)
    PageText.Text = title
    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(title)
  End Sub

  'This is going to remove the background image.
  'If you ever need a very basic page with no added styles
  Public Sub RemoveBackgroundImage(ByVal removeImage As Boolean)
    If removeImage Then
      background_image.Text = ""
    Else
      If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
        If Session.Item("localUser").crmLocalUser_Background_ID <> 0 Then
          background_image.Text = "<img src=""images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
        End If
      End If
    End If
  End Sub

  Public Sub SetContainerClass(ByVal ClassName As String)
    pageSizing.Attributes.Remove("class")
    pageSizing.Attributes.Add("class", "container MaxWidthRemove")
    contentContainer.Attributes.Remove("class")
    contentContainer.Attributes.Add("class", "contentContainer sixteen columns")
    footer.Attributes.Remove("class")
    footer.Attributes.Add("class", "sixteen columns footer")
  End Sub

  'If you need a quick function to allow the masterpage to just be a blank screen
  'Use this and pass true. If you need to reset it, pass false.
  Public Sub RemoveAllStyleElements(ByVal removeStyle As Boolean)
    RemoveBackgroundImage(removeStyle)
    RemoveSizes(removeStyle)
    removeFooter(removeStyle)
    RemoveBackgroundColor(removeStyle)
    RemoveLogo(removeStyle)
    TurnOffPageHeader(removeStyle)
    WelcomeContainer.Attributes.Remove("class")
    belowWelcomeContainer.Attributes.Remove("class")
    fixedBar.Visible = False
  End Sub

  'This function allows you to remove the logo from this master page.
  'Or put it back, if for some reason you need to do so.
  Public Sub RemoveLogo(ByVal removeItem As Boolean)
    If removeItem Then
      logo.Visible = False
    Else
      logo.Visible = False
    End If
  End Sub
  'This will toggle the page header if it isn't needed.
  Public Sub TurnOffPageHeader(ByVal removeItem As Boolean)
    If removeItem Then
      PageText.Visible = False
    Else
      PageText.Visible = False
    End If
  End Sub
  'This function will allow you to toggle off/on the footer.
  Public Sub removeFooter(ByVal removeItem As Boolean)
    footer.Attributes.Remove("class")
    footer.Attributes.Add("class", IIf(removeItem, "display_none", "sixteen columns footer"))
  End Sub

  'This footer will get rid of the white background color of the page.
  'or put it back on depending on parameter
  Public Sub RemoveBackgroundColor(ByRef removeColor As Boolean)
    bodyTag.Attributes.Remove("class")
    bodyTag.Attributes.Add("class", IIf(removeColor, "noBackgroundColor", ""))
  End Sub

  'This function will get rid of the css grid styling that's used on this page
  'Or put it back, allowing you to toggle.
  Public Sub RemoveSizes(ByVal removeClass As Boolean)
    If removeClass Then
      container_white_background_div.Attributes.Remove("class")
      pageSizing.Attributes.Remove("class")
    Else
      container_white_background_div.Attributes.Add("class", "sixteen columns white_background_color content_border")
      pageSizing.Attributes.Add("class", "container")
    End If
  End Sub

  ''' <summary>
  ''' Configures background. With an ID of 0, it means they have a random picture background. This means
  ''' they need to look up the background (if they haven't already). So we look it up, then store it and update the picture
  ''' with the new background. Query isn't run on postback but update of picture is.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub Configure_Background()
    Trace.Write("Start Configure_Background HomebaseTheme.Master.vb" + Now.ToString)

    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
      If Session.Item("localUser").crmLocalUser_Background_ID = 0 Then 'this means the background is random, this needs to be looked up once, and then set.
        If Not Page.IsPostBack Then
          temptable = aclsData_temp.GetBackgroundImages()
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

    Trace.Write("End Configure_Background HomebaseTheme.Master.vb" + Now.ToString)

  End Sub

End Class