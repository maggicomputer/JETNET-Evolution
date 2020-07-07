' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/EvoStyles/CustomerAdminTheme.Master.vb $
'$$Author: Mike $
'$$Date: 6/14/20 1:48p $
'$$Modtime: 6/14/20 10:39a $
'$$Revision: 27 $
'$$Workfile: CustomerAdminTheme.Master.vb $
'
' ********************************************************************************

Partial Public Class CustomerAdminTheme
  Inherits System.Web.UI.MasterPage

  Public aclsData_temp As clsData_Manager_SQL = Nothing
  Public error_string As String = ""
  Dim temptable As New DataTable
  Protected localDatalayer As New admin_center_dataLayer
  Dim strPageAt As String = ""
  Public script_version As String = ""

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    aclsData_temp = New clsData_Manager_SQL
    aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase")
    aclsData_temp.client_DB = Session.Item("jetnetServerNotesDatabase")

    Session.Item("localUser").crmUser_DebugText = ""
    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")

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

    Dim link3 As HtmlLink = New HtmlLink()
    link3.Attributes.Add("type", "text/css")
    link3.Attributes.Add("rel", "stylesheet")
    link3.Attributes.Add("href", "/EvoStyles/stylesheets/adminStyles.css" + script_version)
    Page.Header.Controls.Add(link3)

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

  End Sub
  Public Sub RemoveLine()
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try


      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else
        localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


        Trace.Write("Start Page_load CustomerAdminTheme.Master.vb" + Now.ToString)

        'Clear this Automatic Refresh Time Integer: This clears every time the masterpage is hit in a refresh.
        Session.Item("AutomaticRefreshTime") = 0

        'Configures background
        'Configure_Background()

        If Not Page.IsPostBack Then
          Dim hostReference As String = Application.Item("crmClientSiteData").crmClientHostName

          If Request.IsSecureConnection Then
            hostReference = "https://" + hostReference
          Else

            hostReference = "http://" + hostReference
          End If


          strPageAt = Replace(LCase(Request.Url.ToString()), LCase(hostReference), "")

          If strPageAt = "/adminhome.aspx" Then
            BuildMenu("Main") 'Build temporary left hand menu on page
          Else
            Dim MenuURLCheck As New DataTable
            MenuURLCheck = localDatalayer.MenuFilter("", "", strPageAt, "ADMIN")
            If Not IsNothing(MenuURLCheck) Then
              If MenuURLCheck.Rows.Count > 0 Then
                BuildMenu(MenuURLCheck.Rows(0).Item("menutree_page_name"))
              Else
                BuildMenu("Main")
              End If
            End If
          End If
        End If

      End If

      Trace.Write("End Page_load CustomerAdminTheme.Master.vb" + Now.ToString)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString

    End Try

  End Sub

  Public Sub BuildMenu(pageName As String)

    Dim returnString As String = ""
    Dim parentTable As New DataTable

    parentTable = localDatalayer.MenuFilter(pageName, "", "", "ADMIN")

    If Not IsNothing(parentTable) Then
      If parentTable.Rows.Count > 0 Then
        returnString = "<ul class=""sf-menu sf-vertical"">"

        If pageName <> "Main" Then
          returnString += "<li><a href =""javascript:void(0);"" class=""js-back"">Back</a></li>"
          returnString += "<li><a href =""/adminHome.aspx"">Dashboard</a></li>"
          returnString += "<li><strong>" + pageName.ToString + "</strong></li>"
        End If
        For Each r As DataRow In parentTable.Rows
          If Not IsDBNull(r("menutree_display_name")) Then
            If Not IsDBNull(r("menutree_display_url")) Then

              If Not String.IsNullOrEmpty(r("menutree_display_url")) Then
                Dim PageCurrent As String = ""
                Dim activeCSS As String = ""
                PageCurrent = r("menutree_display_url")

                If PageCurrent.ToLower = strPageAt.ToLower Then
                  If UCase(r("menutree_page_name")) = "MAIN" Then
                    Page.Title = r("menutree_display_name")
                  Else
                    Page.Title = r("menutree_page_name") + " - " + r("menutree_display_name")
                    mainHeaderText.InnerHtml = r("menutree_page_name") + " - <strong>" + r("menutree_display_name") + "</strong>"
                    mainHeaderText.Visible = True
                  End If

                  activeCSS = "active"
                End If


                returnString += "<li class=""" & IIf(r("itemSubCount") > 0, "hasChildren", "") & " " + activeCSS + """>"

                returnString += "<a href=""" + PageCurrent + """>"
                returnString += r("menutree_display_name").ToString.Replace(" ", "&nbsp;")
                returnString += "</a>"
              Else
                returnString += "<li class=""" & IIf(r("itemSubCount") > 0, "hasChildren", "") & """>"
                returnString += r("menutree_display_name").ToString.Replace(" ", "&nbsp;")
              End If
            Else
              returnString += "<li class=""" & IIf(r("itemSubCount") > 0, "hasChildren", "") & """>"
              returnString += r("menutree_display_name").ToString.Replace(" ", "&nbsp;")
            End If

            If r("itemSubCount") > 0 Then 'If item has children, run query on them
              CreateChildren(returnString, r("menutree_item_name"), 0)
            End If
            returnString += "</li>"
          End If
        Next

      End If
    End If

    menutree_literal.Text = returnString

  End Sub

  Public Function CreateChildren(ByRef returnString As String, ByVal parentName As String, ByRef ChildCount As Integer) As String
    Dim ReturnTable As New DataTable
    Dim menutreeLink As String = ""
    returnString += "<ul>"
    ReturnTable = localDatalayer.MenuFilter(parentName, "", "", "ADMIN")
    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows
        menutreeLink = "<a href=""" + r("menutree_display_url") + """>"
        If Not IsDBNull(r("menutree_display_url")) Then
          returnString += "<li>"

          If Not IsDBNull(r("menutree_target")) Then
            If Not String.IsNullOrEmpty(r("menutree_target")) Then
              If r("menutree_target") = "NEW" Then
                menutreeLink = "<a href=""javascript:void(0);"" onclick=""javascript:load('" & r("menutree_display_url") & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
              End If
            End If
          End If
          returnString += menutreeLink
          returnString += r("menutree_display_name").ToString.Replace(" ", "&nbsp;")
          returnString += "</a>"
        Else
          returnString += "<li>"
          returnString += r("menutree_display_name").ToString.Replace(" ", "&nbsp;")
        End If

        'If r("itemSubCount") > 0 Then 'If item has children, run query on them
        '    CreateChildren(returnString, r("menutree_item_name"), 0)
        'End If
        returnString += "</li>"
      Next
    End If
    returnString += "</ul>"

    Return returnString
  End Function

  Public Sub SetDefaultButtion(ByVal buttonID As String)
    form1.DefaultButton = buttonID
  End Sub

  Public Sub SetStatusText(ByVal text As String, Optional ByRef aircraftSearch As Boolean = False)
    WelcomeUser1.SetStatusText(text)
  End Sub

  Public Function ReturnStatusText() As String
    Return WelcomeUser1.evo_message_text.Text
  End Function

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
    gray_bar_container.Visible = visibility
  End Sub
  Public Sub ToggleWelcomeMessage(ByVal visible As Boolean)
    WelcomeUser1.ToggleWelcomeMessage(visible)
  End Sub
  Public Sub RemoveWhiteBackground(ByVal remove As Boolean)
    If remove = True Then
      container_white_background_div.Attributes("class") = "sixteen columns"
      container_border.Attributes("class") = ""
    End If
  End Sub
  Public Sub UpdateHelpLink(ByVal url As String)
    WelcomeUser1.ChangeHelpLink(url)
  End Sub
  ''' <summary>
  ''' Configures background. With an ID of 0, it means they have a random picture background. This means
  ''' they need to look up the background (if they haven't already). So we look it up, then store it and update the picture
  ''' with the new background. Query isn't run on postback but update of picture is.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub Configure_Background()
    Trace.Write("Start Configure_Background CustomerAdminTheme.Master.vb" + Now.ToString)

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
        background_image.Text = "<img src="" images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
      End If
    End If

    Trace.Write("End Configure_Background CustomerAdminTheme.Master.vb" + Now.ToString)

  End Sub

  Public Sub Set_Active_Tab(ByVal active_tabIndex As Long)

  End Sub

End Class