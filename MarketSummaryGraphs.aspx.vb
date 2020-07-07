' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/MarketSummaryGraphs.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:40a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: MarketSummaryGraphs.aspx.vb $
'
' ********************************************************************************

Partial Public Class MarketSummaryGraphs

  Inherits System.Web.UI.Page

  Const graph_width = 490
  Const graph_height = 490

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim sGenerateFileName As String = ""
    Dim nGraphID As Integer = 0
    Dim graphData As marketGraphData = Nothing

    Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"
    Dim sReportTitle As String = "market_summary_graph"

    sGenerateFileName = commonEvo.GenerateFileName(subscriptionInfo + sReportTitle, ".jpg", False)

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load market summary graphs : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      If Not IsNothing(Request.Item("graphID")) Then
        If Not String.IsNullOrEmpty(Request.Item("graphID").ToString.Trim) Then
          nGraphID = CInt(Request("graphID").ToString.Trim)
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        graphData = Session.Item("marketGraphData")(nGraphID - 1)
      End If

      If Not IsNothing(graphData) Then

        Master.SetPageTitle("Market Summary - " + graphData.marketGraph_topTitle)  ' sets the page title

        market_summary_graph_chart.Titles.Clear()

        display_market_graph(graphData, Me.market_summary_graph_chart)

        market_summary_graph_chart.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
        market_summary_graph_chart.SaveImage(HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sGenerateFileName, DataVisualization.Charting.ChartImageFormat.Jpeg)

        graph_image.Text = "<div style=""text-align: center;""><img src=""TempFiles/" + sGenerateFileName + """ width=""" + graph_width.ToString + """ height=""" + graph_height.ToString + """></div>"

      Else
        Master.SetPageTitle("There was an error with Market Summary Graph Display")  ' sets the page title
      End If

    End If

  End Sub

  Public Sub display_market_graph(ByRef localGraphData As marketGraphData, ByRef LOCAL_GRAPH As DataVisualization.Charting.Chart)

    Dim high_number As Double = 0.0
    Dim low_number As Double = 0.0
    Dim starting_point As Integer = 0
    Dim ending_point As Integer = 0
    Dim interval_point As Integer = 1

    Dim current_month_to_show As Boolean = False

    Dim x_data() As String = Nothing
    Dim y_data() As String = Nothing

    Dim nCount As Integer = 0

    Try

      LOCAL_GRAPH.Series.Clear()
      LOCAL_GRAPH.Series.Add("PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
      LOCAL_GRAPH.Series("PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      LOCAL_GRAPH.Series("PER_MONTH").LabelForeColor = Drawing.Color.Blue
      LOCAL_GRAPH.ChartAreas("ChartArea1").AxisY.Title = localGraphData.marketGraph_Y_title
      LOCAL_GRAPH.Series("PER_MONTH").Color = Drawing.Color.Blue
      LOCAL_GRAPH.Series("PER_MONTH").BorderWidth = 1
      LOCAL_GRAPH.Series("PER_MONTH").MarkerSize = 5
      LOCAL_GRAPH.Series("PER_MONTH").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      LOCAL_GRAPH.Width = graph_width
      LOCAL_GRAPH.Height = graph_height

      If Not String.IsNullOrEmpty(localGraphData.marketGraph_Y_data.Trim) Then
        y_data = Split(localGraphData.marketGraph_Y_data, crmWebClient.Constants.cCommaDelim)
      End If

      If Not String.IsNullOrEmpty(localGraphData.marketGraph_X_data.Trim) Then
        x_data = Split(localGraphData.marketGraph_X_data, crmWebClient.Constants.cCommaDelim)
      End If

      If Not IsNothing(y_data) And IsArray(y_data) Then

        For x As Integer = 0 To y_data.Length - 1

          If Not String.IsNullOrEmpty(y_data(x).ToString.Trim) And IsNumeric(y_data(x)) Then

            'debug_output.Text += "x: " + x_data(x).ToString + " y: " + y_data(x).ToString + "<br />"

            If high_number = 0 Or CDbl(y_data(x)) > high_number Then
              high_number = CDbl(y_data(x))
            End If

            If low_number = 0 Or CDbl(y_data(x)) < low_number Then
              low_number = CDbl(y_data(x))
            End If

            LOCAL_GRAPH.Series("PER_MONTH").Points.AddXY(x_data(x), CDbl(y_data(x)))

            nCount += 1

          End If

        Next

      Else ' no y data must be all zeros

        For x As Integer = 0 To x_data.Length - 1

          If Not String.IsNullOrEmpty(x_data(x).ToString.Trim) Then

            'debug_output.Text += "x: " + x_data(x).ToString + " y: 0<br />"

            LOCAL_GRAPH.Series("PER_MONTH").Points.AddXY(x_data(x), 0)

            nCount += 1

          End If

        Next

      End If

      commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)

      If localGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_YEAR Or localGraphData.marketGraph_LinkType = eGraphLinkType.WS_AVG_YEAR Then
        interval_point = 5
        starting_point = ((low_number / interval_point) - 1) * interval_point
        ending_point = ((high_number / interval_point) + 1) * interval_point
      End If

      'debug_output.Text += "HI: " + high_number.ToString + " LO: " + low_number.ToString + "<br />"
      'debug_output.Text += "EP: " + ending_point.ToString + " SP: " + IIf(starting_point > 0, starting_point, 0).ToString + " IP: " + interval_point.ToString + "<br />"
      'debug_output.Text += "SZ: " + CInt((high_number.ToString.Length + low_number.ToString.Length) / 2).ToString + " RG: " + Math.Abs(high_number - low_number).ToString + "<br />"
      debug_output.Visible = False

      LOCAL_GRAPH.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
      LOCAL_GRAPH.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
      LOCAL_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = interval_point

    Catch ex As Exception

      debug_output.Text += "Error in display_market_graph(ByRef LOCAL_GRAPH As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

  End Sub

End Class