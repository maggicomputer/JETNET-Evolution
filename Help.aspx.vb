Partial Public Class Help
  Inherits System.Web.UI.Page
  Dim TabSelected As Integer = 0
  Dim SelectTopic As String = ""
  Dim SelectedID As Long = 0
  Dim SelectedArticleTitle As String = ""
  Dim Clear As Boolean = False
  Dim selected_topic_section As String = ""
  Dim aclsData_Temp As New clsData_Manager_SQL

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Help")

    Dim is_for_export As Boolean = False

    If Not Page.IsPostBack Then
      close_window_only.Text += ("<a class=""underline cursor"" onclick=""javascript:window.close();return false;"" class=""close_button"" style=""padding-right:15px;""><img src='images/x.svg' alt='Close' /></a>")
    End If

    If Not IsNothing(Request.Item("t")) Then
      If Not String.IsNullOrEmpty(Request.Item("t").ToString) Then
        TabSelected = CLng(Request.Item("t").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("id")) Then
      If Not String.IsNullOrEmpty(Request.Item("id").ToString) Then
        SelectedID = CLng(Request.Item("id").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("clear")) Then
      If Not String.IsNullOrEmpty(Request.Item("clear").ToString) Then
        If (Request.Item("clear").ToString = "true") Then
          Clear = True
        End If
      End If
    End If

    If Trim(Request("export")) = "Y" Then
      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR And Session.Item("localUser").crmAllowExport_Flag = True Then
        export_notes.Visible = True
        is_for_export = True
      End If
    End If

    If Not is_for_export Then

      If Trim(Request("section")) <> "" Then
        selected_topic_section = Trim(Request("section"))
      Else

        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
          If Not IsNothing(Request.Item("s")) Then
            If Not String.IsNullOrEmpty(Request.Item("s").ToString) Then
              SelectTopic = CLng(Request.Item("s").ToString.Trim)
              Select Case CLng(Request.Item("s").ToString.Trim)
                Case 1
                  SelectTopic = "Yacht List"
                Case 2
                  SelectTopic = "Yacht Details"
                Case Else
                  SelectTopic = ""
              End Select
            End If
          End If
        Else
          If Not IsNothing(Request.Item("s")) Then
            If Not String.IsNullOrEmpty(Request.Item("s").ToString) Then
              SelectTopic = CLng(Request.Item("s").ToString.Trim)
              Select Case CLng(Request.Item("s").ToString.Trim)
                Case 1
                  SelectTopic = "Aircraft List"
                Case 2
                  SelectTopic = "Aircraft Details"
                Case Else
                  SelectTopic = ""
              End Select
            End If
          End If
        End If
      End If


      'Let's list what this page needs to do.
      If Not Page.IsPostBack Then
        'We need to clear Notification if clear is set
        If Clear Then
          'If clear is true
          ClearNotification()
        End If


        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
          Fill_Yacht_Bulletin_Board()
          Fill_Yacht_Help()
          Fill_Yacht_News()
          Fill_News()
        Else
          Fill_New_Features()
          Fill_Bulletin_Board()
          Fill_Help()
          Fill_News()
        End If

        If Session.Item("localSubscription").crmBusiness_Flag = True Then
          If Trim(Request("attribute")) = "Y" Then
            Fill_AC_Glossary_Attributes("")
          Else
            Fill_AC_Glossary("")
          End If

          ac_glossary_panel.Visible = True
        End If

        If Session.Item("localSubscription").crmYacht_Flag = True Then
          Fill_Yacht_Glossary("")
          yacht_glossary_panel.Visible = True
        End If


        If Not IsPostBack Then
          If Trim(Request("search_term")) <> "" Then
            ac_glossary_text.Text = Trim(Request("search_term"))
            search_text()
          End If
        End If

                'Get_FAQ_Events()
                Get_Model_Resources()
        Get_Calendar_Events()

        If SelectedID <> 0 Then
          FillIndividualTab()
          tab_container_ID.ActiveTab = individual_panel
        End If
      Else
        'Close open Tab Panel if a post back
        individual_label.Text = ""
        individual_panel.Visible = False
      End If

      If Not IsPostBack Then
        If SelectedID = 0 Then
          tab_container_ID.ActiveTabIndex = TabSelected
        End If
      End If

    End If

    DeterminePageTitle()

    If Not IsPostBack Then
      insert_help_function()
    End If

  End Sub

  Public Sub insert_help_function()
    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
      Select Case tab_container_ID.ActiveTab.ID.ToString
        Case "features_tab"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Bulletin Board", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "bulletin_tab"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Help", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "help_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed New Features", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "news_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed News", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "calendar_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed JETNET Calendar", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "faq_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed FAQs", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "resources_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Model Resources", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "ac_glossary_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Aircraft Glossary", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "yacht_glossary_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Yacht Glossary", Nothing, 0, 0, 0, 0, 0, 0, 0)
      End Select
    Else
      Select Case tab_container_ID.ActiveTab.ID.ToString
        Case "features_tab"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed New Features", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "bulletin_tab"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Bulletin Board", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "news_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed News", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "calendar_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed JETNET Calendar", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "faq_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed FAQs", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "resources_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Model Resources", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "ac_glossary_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Aircraft Glossary", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case "yacht_glossary_panel"
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Yacht Glossary", Nothing, 0, 0, 0, 0, 0, 0, 0)
        Case Else
          Call commonLogFunctions.Log_User_Event_Data("UserDisplayHelp", "User Displayed Help", Nothing, 0, 0, 0, 0, 0, 0, 0)
      End Select
    End If

  End Sub

  Private Sub ClearNotification()
    Dim strDate As String = Format(Now(), "yyyy-MM-dd H:mm:ss")

    masterPage.aclsData_Temp.InsertIntoSubscriptionNotifications(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo, SelectedID, strDate, "R")
    Response.Redirect("help.aspx?id=" & SelectedID)
  End Sub

  Private Sub DeterminePageTitle()


    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
      Select Case tab_container_ID.ActiveTab.ID.ToString
        Case "features_tab"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Bulletin Board")
        Case "bulletin_tab"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Help")
        Case "help_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "New Features")
        Case "news_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "News")
        Case "calendar_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "JETNET Calendar")
        Case "faq_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "FAQs")
        Case "resources_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Model Resources")
        Case "ac_glossary_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Aircraft Glossary")
        Case "yacht_glossary_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Yacht Glossary")
        Case Else
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "")
      End Select
    Else
      Select Case tab_container_ID.ActiveTab.ID.ToString
        Case "features_tab"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "New Features")
        Case "bulletin_tab"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Bulletin Board")
        Case "news_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "News")
        Case "calendar_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "JETNET Calendar")
        Case "faq_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "FAQs")
        Case "resources_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Model Resources")
        Case "ac_glossary_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Aircraft Glossary")
        Case "yacht_glossary_panel"
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Yacht Glossary")
        Case Else
          masterPage.SetPageTitle(IIf(SelectTopic <> "", SelectTopic & " ", "") & IIf(SelectedArticleTitle <> "", SelectedArticleTitle & " ", "") & "Help")
      End Select
    End If



  End Sub

  Private Sub FillIndividualTab()

    Dim IndDataT As New DataTable
    Dim htmlString As String = ""

    IndDataT = masterPage.aclsData_Temp.HelpData("", SelectTopic, SelectedID)

    If Not IsNothing(IndDataT) Then
      If IndDataT.Rows.Count > 0 Then
        individual_panel.Visible = True
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In IndDataT.Rows

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_title").ToString.Trim + " - " + r("evonot_announcement").ToString.Trim '.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span>"

          SelectedArticleTitle = r("evonot_title").ToString.Trim
          individual_panel.HeaderText = r("evonot_title").ToString.ToUpper.Trim

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then

              htmlString += "</td></tr><tr><td align=""left"" valign=""middle"">"
              htmlString += "<div id=""" + r("evonot_id").ToString + """><p align=""center"">"

              htmlString += r.Item("evonot_video").ToString.Trim

              htmlString += "</p></div><div class=""clear"">&nbsp;</div>"

            End If
          End If

          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    individual_label.Text = htmlString.Trim

    IndDataT = Nothing

  End Sub

  Private Sub Fill_New_Features()

    Dim New_FeaturesData As New DataTable
    Dim htmlString As String = ""
    New_FeaturesData = masterPage.aclsData_Temp.HelpData("G", "", 0)

    If Not IsNothing(New_FeaturesData) Then
      If New_FeaturesData.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In New_FeaturesData.Rows

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span>"

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
              htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
            End If
          End If

          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    features_label.Text = htmlString.Trim

    New_FeaturesData = Nothing

  End Sub

  Private Sub Fill_Bulletin_Board()

    Dim BulletinData As New DataTable
    Dim htmlString As String = ""

    BulletinData = masterPage.aclsData_Temp.HelpData("B", "", 0)

    If Not IsNothing(BulletinData) Then
      If BulletinData.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In BulletinData.Rows
          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span></td></tr>"

        Next

        htmlString += "</table>"

      End If
    End If

    bulletin_label.Text = htmlString.Trim

    BulletinData = Nothing

  End Sub

  Private Sub Fill_Help()

    Dim Last_Tab As String = ""
    Dim htmlString As String = ""
    Dim HelpDataT As New DataTable

    If Not String.IsNullOrEmpty(selected_topic_section.Trim) Then
      HelpDataT = masterPage.aclsData_Temp.HelpData("H", selected_topic_section.Trim, 0, True)
    Else
      HelpDataT = masterPage.aclsData_Temp.HelpData("H", SelectTopic, 0)
    End If

    If Not IsNothing(HelpDataT) Then
      If HelpDataT.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In HelpDataT.Rows

          If Not IsDBNull(r("evotop_name")) Then
            If Last_Tab = "" Or Last_Tab <> r("evotop_name").ToString Then
              Last_Tab = r("evotop_name").ToString
              htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">" + Last_Tab + "</strong></td></tr>"
            End If
          End If

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><span class=""help_indent"">&nbsp;"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Replace("http://www.jetnet.com", "https://www.jetnet.com").Replace("www.jetnetevo.com", "www.jetnetevolution.com").Trim
          htmlString += "</span>"

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
              htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
            End If
          End If

          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If

    End If

    help_label.Text = htmlString.Trim

    HelpDataT = Nothing

  End Sub

  Private Sub Fill_AC_Glossary_Attributes(ByVal search_term As String)

    Dim last_letter As String = ""
    Dim temp_topic As String = ""
    Dim htmlString As String = ""

    Dim acGlossaryDt As New DataTable
    acGlossaryDt = masterPage.aclsData_Temp.Build_AC_Glossary_Based_On_Attributes(search_term)

    If Not IsNothing(acGlossaryDt) Then
      If acGlossaryDt.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"

        For Each r As DataRow In acGlossaryDt.Rows

          If Not IsDBNull(r.Item("LETTER")) Then

            If Trim(last_letter) = "" Or Trim(last_letter) <> Trim(r.Item("LETTER").ToString) Then
              htmlString += "<tr><td style=""vertical-align: top; text-align: left;""><strong class=""padding_table upperCase"">" + r.Item("LETTER").ToString.Trim + "</strong></td></tr>"
            End If

            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">"
            htmlString += "<span class=""help_indent"">&nbsp;"
            htmlString += "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">&#149;&nbsp;"

            If Not String.IsNullOrEmpty(r.Item("TOPIC").ToString.Trim) Then
              htmlString += "<strong class=""padding_table upperCase"">" + r("TOPIC").ToString.Trim + "</strong> - "
            End If

            If Not String.IsNullOrEmpty(r.Item("RELATED_TOPICS").ToString.Trim) Then
              temp_topic = r.Item("RELATED_TOPICS").ToString.ToString.Replace("<actop_name>", ",").Replace("</actop_name>", "").Replace("actop_name>", "").Trim
              htmlString += " <i>Also Known As: " + temp_topic.Trim + " - </i>"
            End If

            If Not String.IsNullOrEmpty(r.Item("RELATED_TOPICS2").ToString.Trim) Then
              temp_topic = r.Item("RELATED_TOPICS2").ToString.Replace("<actop_name>", ",").Replace("</actop_name>", "").Replace("actop_name>", "").Trim
              htmlString += " <i>Also Known As: " + temp_topic.Trim + " - </i>"
            End If

            If Not String.IsNullOrEmpty(r.Item("DESCRIPTION").ToString.Trim) Then
              htmlString += r("DESCRIPTION").ToString.Trim
            End If

            htmlString += "</td></tr></table>"

          End If

          htmlString += "</span></td></tr>"
          last_letter = r.Item("LETTER").ToString.Trim

        Next

        htmlString += "</table>"

      End If
    End If

    ac_glossary_label.Text = htmlString.Trim

    acGlossaryDt = Nothing

  End Sub

  Private Sub Fill_AC_Glossary(ByVal search_term As String)

    Dim acGlossary As New DataTable
    acGlossary = masterPage.aclsData_Temp.Build_AC_Glossary(search_term)

    Dim last_letter As String = ""
    Dim temp_topic As String = ""
    Dim htmlString As String = ""

    If Not IsNothing(acGlossary) Then
      If acGlossary.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"

        For Each r As DataRow In acGlossary.Rows

          If Not IsDBNull(r.Item("LETTER")) Then

            If Trim(last_letter) = "" Or Trim(last_letter) <> Trim(r.Item("LETTER").ToString) Then
              htmlString += "<tr><td style=""vertical-align: top; text-align: left;""><strong class=""padding_table upperCase"">" + r.Item("LETTER").ToString.Trim + "</strong></td></tr>"
            End If

            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">"
            htmlString += "<span class=""help_indent"">&nbsp;"
            htmlString += "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">&#149;&nbsp;"

            If Not String.IsNullOrEmpty(r.Item("TOPIC").ToString.Trim) Then
              htmlString += "<strong class=""padding_table upperCase"">" + r("TOPIC").ToString.Trim + "</strong> - "
            End If

            If Not String.IsNullOrEmpty(r.Item("DESCRIPTION").ToString.Trim) Then
              htmlString += r("DESCRIPTION").ToString.Trim
            End If

            If Not String.IsNullOrEmpty(r.Item("RELATED_TOPICS").ToString.Trim) Then
              temp_topic = r.Item("RELATED_TOPICS").ToString.ToString.Replace("<actop_name>", ",").Replace("</actop_name>", "").Replace("actop_name>", "").Trim
              htmlString += " [Also Known As: " + temp_topic.Trim + "]"
            End If

            htmlString += "</td></tr></table>"

          End If

          htmlString += "</span></td></tr>"

          last_letter = r.Item("LETTER").ToString.Trim

        Next

        htmlString += "</table>"

      End If
    End If

    ac_glossary_label.Text = htmlString.Trim

    acGlossary = Nothing

  End Sub

  Private Sub Fill_News()

    Dim NewsData As New DataTable
    Dim htmlString As String = ""
    NewsData = masterPage.aclsData_Temp.HelpData("N','J", "", 0)

    If Not IsNothing(NewsData) Then
      If NewsData.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"

        For Each r As DataRow In NewsData.Rows
          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span></td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    news_label.Text = htmlString.Trim

    NewsData = Nothing

  End Sub

    'Private Sub Get_FAQ_Events()

    '  Dim HelpDataT As New DataTable
    '  Dim htmlString As String = ""

    '  HelpDataT = masterPage.aclsData_Temp.HelpData("EF", "", 0)

    '  faq_panel.HeaderText = "FAQs"

    '  If Not IsNothing(HelpDataT) Then
    '    If HelpDataT.Rows.Count > 0 Then
    '      htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
    '      For Each r As DataRow In HelpDataT.Rows

    '        htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

    '        If Not IsDBNull(r.Item("evonot_doc_link")) Then
    '          If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

    '            htmlString += "<a href="""

    '            If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

    '              If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

    '                If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
    '                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
    '                ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
    '                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
    '                Else
    '                  htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
    '                End If

    '              Else
    '                htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
    '              End If

    '            Else

    '              If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

    '                If Request.IsSecureConnection Then
    '                  htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
    '                Else
    '                  htmlString += r.Item("evonot_doc_link").ToString.Trim
    '                End If

    '              Else

    '                If Request.IsSecureConnection Then
    '                  htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
    '                End If

    '              End If

    '            End If

    '            htmlString += """ target=""_blank"" class=""underline"">"

    '          End If
    '        End If

    '        htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
    '        htmlString += r("evonot_title").ToString

    '        If Not IsDBNull(r.Item("evonot_doc_link")) Then
    '          If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
    '            htmlString += "</a>"
    '          End If
    '        End If

    '        htmlString += "</strong><span class=""help_indent"">&nbsp;"
    '        htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
    '        htmlString += "</span>" ' </td></tr>

    '        If Not IsDBNull(r("evonot_video")) Then
    '          If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
    '            htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
    '          End If
    '        End If
    '        htmlString += "</td></tr>"

    '      Next
    '      htmlString += "</table>"
    '    End If
    '  End If

    '  faq_label.Text = htmlString.Trim
    '  HelpDataT = Nothing

    'End Sub

    Private Sub Get_Model_Resources()

    Dim HelpDataT As New DataTable
    Dim htmlString As String = ""
    HelpDataT = masterPage.aclsData_Temp.HelpData("ML", "", 0)

    resources_panel.HeaderText = "Model Resources"

    If Not IsNothing(HelpDataT) Then
      If HelpDataT.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In HelpDataT.Rows

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span>" ' </td></tr>

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
              htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
            End If
          End If
          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    resources_label.Text = htmlString.Trim
    HelpDataT = Nothing

  End Sub

  Private Sub Get_Calendar_Events()

    Dim HelpDataT As New DataTable
    HelpDataT = masterPage.aclsData_Temp.HelpData("JC", "", 0)
    Dim htmlString As String = ""

    calendar_panel.HeaderText = "JETNET Calendar"

    If Not IsNothing(HelpDataT) Then
      If HelpDataT.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In HelpDataT.Rows

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Replace("training@jetnet.com", "<a href=""mailto:training@jetnet.com"" class=""underline"">training@jetnet.com</a>").Trim
          htmlString += "</span>" ' </td></tr>

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
              htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
            End If
          End If
          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    calendar_label.Text = htmlString.Trim
    HelpDataT = Nothing

  End Sub

  Private Sub Fill_Yacht_Help()

    Dim HelpDataT As New DataTable
    Dim htmlString As String = ""
    Dim Last_Tab As String = ""

    HelpDataT = masterPage.aclsData_Temp.HelpData("YH", "", 0)

    bulletin_tab.HeaderText = "YachtSpot Help"

    If Not IsNothing(HelpDataT) Then
      If HelpDataT.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In HelpDataT.Rows

          If Not IsDBNull(r("evotop_name")) Then
            If Last_Tab = "" Or Last_Tab <> r("evotop_name").ToString Then
              Last_Tab = r("evotop_name").ToString
              htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">" + Last_Tab + "</strong></td></tr>"
            End If
          End If

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><span class=""help_indent"">&nbsp;"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If


                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span>"

          If Not IsDBNull(r("evonot_video")) Then
            If Not String.IsNullOrEmpty(r("evonot_video").ToString.Trim) Then
              htmlString += "</td></tr><tr><td valign='middle'><a href=""Help.aspx?id=" + r("evonot_id").ToString + """ class=""video_reel_link"" id=""" + r("evonot_id").ToString + "_text"">Click Here To View This Video</a>"
            End If
          End If

          htmlString += "</td></tr>"

        Next
        htmlString += "</table>"
      End If

    End If

    bulletin_label.Text = htmlString.Trim
    HelpDataT = Nothing

  End Sub

  Private Sub Fill_Yacht_News()

    Dim NewsData As New DataTable
    Dim htmlString As String = ""
    NewsData = masterPage.aclsData_Temp.HelpData("YR','YG", "", 0)

    help_panel.HeaderText = "YachtSpot New Features"

    If Not IsNothing(NewsData) Then
      If NewsData.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"

        For Each r As DataRow In NewsData.Rows
          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If


                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span></td></tr>"

        Next
        htmlString += "</table>"
      End If
    End If

    help_label.Text = htmlString.Trim
    NewsData = Nothing

  End Sub

  Private Sub Fill_Yacht_Bulletin_Board()

    Dim BulletinData As New DataTable
    Dim htmlString As String = ""
    BulletinData = masterPage.aclsData_Temp.HelpData("YB", "", 0)

    features_tab.HeaderText = "YachtSpot Bulletin Board"

    If Not IsNothing(BulletinData) Then
      If BulletinData.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
        For Each r As DataRow In BulletinData.Rows

          htmlString += "<tr><td style=""vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;""><strong class=""padding_table upperCase"">"

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

              htmlString += "<a href="""

              If r.Item("evonot_doc_link").ToString.ToLower.Contains("/help") Or r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists") Then

                If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

                  If r.Item("evonot_doc_link").ToString.ToLower.Contains("/masterlists.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  ElseIf r.Item("evonot_doc_link").ToString.ToLower.Contains("/help.aspx") Then
                    htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "https://www.testjetnetevolution.com" + r.Item("evonot_doc_link").ToString.Trim
                  End If


                Else
                  htmlString += Session.Item("jetnetFullHostName").ToString.Substring(0, Session.Item("jetnetFullHostName").ToString.Length - 1) + r.Item("evonot_doc_link").ToString.Trim
                End If

              Else

                If (Not r.Item("evonot_doc_link").ToString.Contains("http://")) And (Not r.Item("evonot_doc_link").ToString.Contains("https://")) Then

                  If Request.IsSecureConnection Then
                    htmlString += "https://" + r.Item("evonot_doc_link").ToString.Trim
                  Else
                    htmlString += "http://" + r.Item("evonot_doc_link").ToString.Trim
                  End If

                Else

                  If Request.IsSecureConnection Then
                    htmlString += r.Item("evonot_doc_link").ToString.Replace("http://", "https://").Trim
                  Else
                    htmlString += r.Item("evonot_doc_link").ToString.Trim
                  End If

                End If

              End If

              htmlString += """ target=""_blank"" class=""underline"">"

            End If
          End If

          htmlString += FormatDateTime(r("evonot_release_date").ToString, DateFormat.ShortDate) + " - "
          htmlString += r("evonot_title").ToString

          If Not IsDBNull(r.Item("evonot_doc_link")) Then
            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              htmlString += "</a>"
            End If
          End If

          htmlString += "</strong><span class=""help_indent"">&nbsp;"
          htmlString += r("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
          htmlString += "</span></td></tr>" '

        Next
        htmlString += "</table>"

      End If
    End If

    features_label.Text = htmlString.Trim

    BulletinData = Nothing

  End Sub

  Private Sub Fill_Yacht_Glossary(ByVal search_term As String)

    Dim ytGlossary As New DataTable
    ytGlossary = masterPage.aclsData_Temp.Build_Yacht_Glossary(search_term)

    Dim last_letter As String = ""
    Dim temp_topic As String = ""
    Dim htmlString As String = ""

    If Not IsNothing(ytGlossary) Then
      If ytGlossary.Rows.Count > 0 Then
        htmlString = "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"

        For Each r As DataRow In ytGlossary.Rows

          If Not IsDBNull(r.Item("LETTER")) Then

            If Trim(last_letter) = "" Or Trim(last_letter) <> Trim(r.Item("LETTER").ToString) Then
              htmlString += "<tr><td style=""vertical-align: top; text-align: left;""><strong class=""padding_table upperCase"">" + r.Item("LETTER").ToString.Trim + "</strong></td></tr>"
            End If

            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">"
            htmlString += "<span class=""help_indent"">&nbsp;"
            htmlString += "<table border=""0"" style=""padding: 4px; border-spacing: 6px; text-align: left; width: 100%;"" class=""formatTable blue"">"
            htmlString += "<tr><td style=""vertical-align: top; text-align: left;"">&#149;&nbsp;"

            If Not String.IsNullOrEmpty(r.Item("TOPIC").ToString.Trim) Then
              htmlString += "<strong class=""padding_table upperCase"">" + r("TOPIC").ToString.Trim + "</strong> - "
            End If

            If Not String.IsNullOrEmpty(r.Item("DESCRIPTION").ToString.Trim) Then
              htmlString += r("DESCRIPTION").ToString.Trim
            End If

            If Not String.IsNullOrEmpty(r.Item("RELATED_TOPICS").ToString.Trim) Then
              temp_topic = r.Item("RELATED_TOPICS").ToString.ToString.Replace("<yttop_name>", ",").Replace("</yttop_name>", "").Replace("yttop_name>", "").Trim
              htmlString += " [Also Known As: " + temp_topic.Trim + "]"
            End If

            htmlString += "</td></tr></table>"

          End If

          htmlString += "</span></td></tr>"

          last_letter = r.Item("LETTER").ToString.Trim

        Next

        htmlString += "</table>"

      End If
    End If

    yacht_glossary_label.Text = htmlString.Trim

    ytGlossary = Nothing

  End Sub

  Private Sub tab_container_ID_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_container_ID.ActiveTabChanged

    DeterminePageTitle()

    insert_help_function()

  End Sub

  Private Sub search_ac_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_ac.Click

    Call search_text()

  End Sub

  Public Sub search_text()

    If Session.Item("localSubscription").crmBusiness_Flag = True Then
      If Trim(Request("attribute")) = "Y" Then
        Fill_AC_Glossary_Attributes(Me.ac_glossary_text.Text)
      Else
        Fill_AC_Glossary(Me.ac_glossary_text.Text)
      End If
      Me.ac_glossary_panel.Visible = True
    End If

  End Sub

  Private Sub search_yacht_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_yacht.Click

    If Session.Item("localSubscription").crmYacht_Flag = True Then
      Fill_Yacht_Glossary(Me.yacht_glossary_text.Text)
      Me.yacht_glossary_panel.Visible = True
    End If

  End Sub

  Private Sub export_notes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_notes.Click
    Dim return_table As New DataTable
    Dim count1 As Integer = 0
    Dim PDF_String As String = ""
    Dim report_name As String = "Export_All.xls"
    Dim tstring As String = ""

    If Me.export_notes.Visible = True Then


      aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
      aclsData_Temp.class_error = ""

      If HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
        ' aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localSubscription").crmCloudNotesDBName
        return_table = aclsData_Temp.Get_CloudNotes_GetByUserIDStatusLessThanDate(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, "", "A", True)


        'HttpContext.Current.Session.Item("localUser").crmUserCompanyID 
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
        If InStr(Trim(Server.MapPath("")), "C:\Users\Matt Wanner\Documents\Visual Studio 2008\Projects\newevo", CompareMethod.Text) > 0 Then
          aclsData_Temp.client_DB = Replace(aclsData_Temp.client_DB, "jetnetcrm2.jetnet.com", "192.69.4.165")
        End If
        return_table = aclsData_Temp.Dual_NotesOnlyOne(0, 0, "A", True, True, True)
      End If


      If HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
        PDF_String = DisplayFunctions.Display_Notes_For_All_Export(return_table, aclsData_Temp, True, True, True, True, True, False, True)

        If Not Build_String_To_HTML(report_name, PDF_String) Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "There was a problem generating your report"
        End If

        tstring = Session.Item("MarketSummaryFolderVirtualPath").ToString & "/" & report_name

        Response.Redirect(tstring)
      End If

    End If
  End Sub

  Public Function Build_String_To_HTML(ByVal report_name As String, ByVal ViewToPDF As String) As Boolean
    Build_String_To_HTML = False
    Try
      Build_String_To_HTML = True
      ' create a file to dump the PDF report to
      ' create a streamwriter variable
      Dim swPDF As System.IO.StreamWriter
      ' create the html file

      'Temp Hold MSW


      swPDF = IO.File.CreateText(HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + report_name)
      ' write to the file
      swPDF.WriteLine(ViewToPDF)
      'close the streamwriter
      swPDF.Close()
      ' call the webgrabber info
      Response.Write("Page:<br>" & ViewToPDF)




    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Build_String_To_HTML: " & ex.Message
    End Try
  End Function

End Class