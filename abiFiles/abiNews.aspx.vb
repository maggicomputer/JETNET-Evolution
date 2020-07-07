
Partial Public Class abiNews
  Inherits System.Web.UI.Page
  Dim TopicID As Long = 0

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    If Not IsNothing(Trim(Request("topic"))) Then
      If IsNumeric(Trim(Request("topic"))) Then
        TopicID = Trim(Request("topic"))
      End If
    End If

    FillNewsCategories()
    FillNews(TopicID)



  End Sub
  Private Sub FillNewsCategories()
    Dim CategoriesTable As New DataTable
    CategoriesTable = Master.AbiDataManager.GetNewsCategories()

    If Not IsNothing(CategoriesTable) Then

      newsCategories.DataSource = CategoriesTable
      newsCategories.DataBind()
    End If
  End Sub
  Private Sub FillNews(ByRef topicID As Long)
    Dim NewsTable As New DataTable
    NewsTable = Master.AbiDataManager.GetAviationArticles(15, topicID, " abinewslnk_date DESC")

    If Not IsNothing(NewsTable) Then
      If NewsTable.Rows.Count > 0 Then
        If topicID > 0 Then
          If Not IsDBNull(NewsTable.Rows(0).Item("abinews_topic")) Then
            news_header.InnerHtml = "Latest " & NewsTable.Rows(0).Item("abinews_topic") & " News"
            Master.Set_Page_Title(NewsTable.Rows(0).Item("abinews_topic") & " News at JETNET Global")
          End If
        End If
      End If

      newsRepeater.DataSource = NewsTable
      newsRepeater.DataBind()
    End If
  End Sub


  Private Sub abiNews_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    Master.Set_Meta_Information("Latest aviation news on Aviation Industry News topics such as aircraft safety, finance, avionics, manufacturing, maintenance, and training on jets, turboprops, piston aircraft, and helicopters. News covering aircraft makes such as Agusta, Airbus, Baron, Beechjet, Bell, Cessna, Citation, Eclipse, Eurocopter, Falcon, Gulfstream, Hawker, King Air, Learjet, Sikorsky, and more.", "Aviation Industry News, Air Transport news, Aircraft Accidents news, Aircraft Avionics news, Aircraft Interior news, Aircraft Investment news, Aircraft Market Research news, Aircraft Research news, Airline news, Airport news, Aviation Finance news, Aviation Maintenance news, Aviation Manufacturing news, Aviation Safety news, Aviation Training news, Federal Aviation Administration news, Fixed Base Operator (FBO) news, Fractional Aircraft Program news, Helicopter news, Military Aircraft news, National Transportation Safety Board news, pilot news")
    If TopicID = 0 Then
      Master.Set_Page_Title("Aviation Industry News at JETNET Global")
    End If
  End Sub
End Class