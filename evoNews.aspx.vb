
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/evoNews.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:38a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: evoNews.aspx.vb $
'
' ********************************************************************************

Partial Public Class evoNews

  Inherits System.Web.UI.Page

  Dim localCriteria As New newsSelectionCriteriaClass
  Dim localDataLayer As New newsDataLayer

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    Try

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else
        Master.SetPageTitle("Aviation Industry News") 'Page title that can be set to whatever is necessary.
      End If

      If Not IsNothing(Request.Item("newsMake")) Then
        If Not String.IsNullOrEmpty(Request.Item("newsMake").ToString.Trim) Then
          localCriteria.NewsCriteriaMakeName = Request.Item("newsMake").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("newsModel")) Then
        If Not String.IsNullOrEmpty(Request.Item("newsModel").ToString.Trim) Then
          localCriteria.NewsCriteriaModelID = CLng(Request.Item("newsModel").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("newsTopic")) Then
        If Not String.IsNullOrEmpty(Request.Item("newsTopic").ToString.Trim) Then
          localCriteria.NewsCriteriaTopic = CLng(Request.Item("newsTopic").ToString.Trim)
        End If
      End If

      localDataLayer.adminConnectStr = Session.Item("jetnetAdminDatabase").ToString.Trim

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load preferences : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      localDataLayer.clientConnectStr = Session.Item("jetnetClientDatabase").ToString.Trim
      localDataLayer.starConnectStr = Session.Item("jetnetStarDatabase").ToString.Trim
      localDataLayer.serverConnectStr = Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDataLayer.cloudConnectStr = Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      masterNews.HeaderText = "<strong>Aviation Industry News</strong>"

      build_news_html()

      'add_ChangeTopActiveTab_Script(tab_container_ID)
    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [build_news_html] : " + ex.Message

    Finally

    End Try

  End Sub

  Public Sub add_ChangeTopActiveTab_Script(ByVal tcSource As AjaxControlToolkit.TabContainer)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("cht-top-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function changeTopTab(num) {")
      sScptStr.Append(vbCrLf & "    var container = $find(""" + tcSource.ClientID.ToString + """);")
      sScptStr.Append(vbCrLf & "    container.set_activeTabIndex(num);")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cht-top-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Private Sub build_news_html()

    Dim htmlOut As New StringBuilder

    Dim htmlNewsTopics As String = ""
    Dim htmlNewsTopicsMakeModel As String = ""
    Dim htmlNewsNotifications As String = ""
    Dim htmlNewsMainBlock As String = ""

    Try

      htmlOut.Append("<table id='newsTopicDataTable' width='100%' cellpadding='2' cellspacing='0' border='0'>")
      htmlOut.Append("<tr><td width='10%' align='left' valign='top'>") ' start first column

      htmlOut.Append("<table id='newsTableLeft' width='100%' cellpadding='2' cellspacing='0' class='module'>")

      htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>&nbsp;LATEST NEWS BY TOPIC&nbsp;</td></tr>")

      If localCriteria.NewsCriteriaModelID > -1 Then
        htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>&nbsp;FOR&nbsp;" + commonEvo.Get_Aircraft_Model_Info(localCriteria.NewsCriteriaModelID, False, "") + "&nbsp;</td></tr>")
      ElseIf Not String.IsNullOrEmpty(localCriteria.NewsCriteriaMakeName.Trim) Then
        htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>&nbsp;FOR&nbsp;" + localCriteria.NewsCriteriaMakeName.Trim + "&nbsp;</td></tr>")
      End If

      htmlOut.Append("<tr><td align='center'>")

      localDataLayer.news_display_topics(localCriteria, htmlNewsTopics)
      htmlOut.Append(htmlNewsTopics)

      htmlOut.Append("</td></tr>")
      htmlOut.Append("<tr><td style='height: 10px;'>&nbsp;</td></tr>")

      htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>")

      If localCriteria.NewsCriteriaModelID > -1 Then
        htmlOut.Append("&nbsp;LATEST NEWS TOPICS&nbsp;")
      ElseIf Not String.IsNullOrEmpty(localCriteria.NewsCriteriaMakeName.Trim) Then
        htmlOut.Append("&nbsp;LATEST NEWS TOPICS BY MODEL&nbsp;")
      Else
        htmlOut.Append("&nbsp;LATEST NEWS TOPICS BY MAKE&nbsp;")
      End If

      htmlOut.Append("</td></tr>")

      If Not String.IsNullOrEmpty(localCriteria.NewsCriteriaMakeName.Trim) Then
        htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>&nbsp;FOR&nbsp;" + localCriteria.NewsCriteriaMakeName.Trim + "&nbsp;</td></tr>")
      End If

      htmlOut.Append("<tr><td align='center'>")

      localDataLayer.news_display_topics_by_make_model(localCriteria, htmlNewsTopicsMakeModel)
      htmlOut.Append(htmlNewsTopicsMakeModel)

      htmlOut.Append("</td></tr></table>")
      htmlOut.Append("</td>") ' end first column 

      htmlOut.Append("<td align='left' valign='top'>") ' start second column
      htmlOut.Append("<table id='newsTableMiddle' width='100%' cellpadding='0' cellspacing='0' class='module'>")
      htmlOut.Append("<tr><td align='center' valign='top'>")

      htmlOut.Append("<table id='newsTableMiddleTitle' cellspacing='0' cellpadding='2' width='100%'>")
      htmlOut.Append("<tr><td width='33%' align='left' valign='top' class='header'>&nbsp;</td>")
      htmlOut.Append("<td width='33%' align='center' valign='top' class='header' nowrap='nowrap'>LATEST NEWS")

      If localCriteria.NewsCriteriaModelID > -1 Then
        htmlOut.Append(" FOR " + commonEvo.Get_Aircraft_Model_Info(localCriteria.NewsCriteriaModelID, False, ""))
      ElseIf Not String.IsNullOrEmpty(localCriteria.NewsCriteriaMakeName.Trim) Then
        htmlOut.Append(" FOR " + localCriteria.NewsCriteriaMakeName.Trim)
      End If

      If localCriteria.NewsCriteriaTopic > -1 Then
        htmlOut.Append(" TOPIC " + localCriteria.NewsCriteriaTopicName.ToUpper.Trim)
      End If

      htmlOut.Append("</td><td width='33%' align='right' valign='top' class='header' style='padding-right:5px;'>")
      htmlOut.Append("<a href='evoNews.aspx?newsMake=&newsModel=-1&newsTopic=-1' target='_self' title='Show All News' class='White'><em>all news</em></a>")
      htmlOut.Append("</td></tr></table>") ' end newsTableMiddleTitle

      htmlOut.Append("</td></tr>")
      htmlOut.Append("<tr><td align='center' valign='top'>")

      localCriteria.NewsCriteriaDisplayRows = 5
      localDataLayer.news_display_main_block(localCriteria, htmlNewsMainBlock)
      htmlOut.Append(htmlNewsMainBlock)

      htmlOut.Append("</td></tr></table>") ' end newsTableMiddle
      htmlOut.Append("</td>") ' end second column

      htmlOut.Append("<td align='left' valign='top'>") ' start third column
      htmlOut.Append("<table id='newsTableRight' width='100%' cellpadding='2' cellspacing='0' class='module'>")
      htmlOut.Append("<tr><td class='header' align='center' valign='top' nowrap='nowrap'>&nbsp;LATEST JETNET NEWS&nbsp;</td></tr>")
      htmlOut.Append("<tr><td align='center' valign='top'>")

      localDataLayer.news_display_evolution_notifications(localCriteria, htmlNewsNotifications)
      htmlOut.Append(htmlNewsNotifications)

      htmlOut.Append("</td></tr></table>") ' end newsTableRight
      htmlOut.Append("</td></tr></table>") ' end outer table

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [build_news_html] : " + ex.Message

    Finally

    End Try

    newsContent.Text = htmlOut.ToString()
    htmlOut = Nothing

  End Sub
End Class