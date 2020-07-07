Partial Public Class defaultABI
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Master.Set_Meta_Information("Aircraft for sale, planes for sale, helicopters for sale, including: Cessna, Gulfstream, Challenger, Hawker, and Learjet aircraft by Aircraft Dealers & Brokers.", "aircraft for sale, jets for sale, turbo props for sale, helicopters for sale, aircraft wanteds, business jets, used aircraft, used planes, aircraft sale, JETNET Global, aviation, aircraft, fbo, dealer, news, aviation links, aviation events, aviation products, plane, airplane, Cessna, gulfstream, hawker, learjet, lear jet, jetnet")
    Master.Set_Page_Title("Aircraft for Sale, Aircraft Sales, Used Aircraft for Sale at JETNET Global")

    'Filling up the Dropdown Model is for Aircraft For Sale Block.
    FillAircraftForSaleModels()

    'Fills up ABI dealers in for sale block dropdown
    FillABIDealers()


    If Not Page.IsPostBack Then
      'Fill Year Ranges
      clsGeneral.clsGeneral.Year_Range_DropDownFill(year_start, 1975, Year(Now()))
      year_start.SelectedValue = ""

      clsGeneral.clsGeneral.Year_Range_DropDownFill(year_end, 1975, Year(Now()))
      year_end.SelectedValue = ""
    End If

    'Filling up the latest Aviation Articles (the main two blocks with pictures).
    FillLatestAviationArticles()

    'Filling up the aviation news. This is the latest without the pictures.
    FillAviationNews()

    'Filling up featured aircraft list.
    FillFeaturedAircraft()

    'Filling up Jetnet News (both pictures and latest)
    FillJetnetNews()

    'Fills up latest 3 events
    FillEventsList()
  End Sub


  ''' <summary>
  ''' 
  ''' A routine that fills up the model dropdown in the aircraft for sale box.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillAircraftForSaleModels()

    Dim AircraftTable As New DataTable
    AircraftTable = Master.AbiDataManager.GetABIAircraftModelList()

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then
        For Each r As DataRow In AircraftTable.Rows
          Dim ModelName As String = ""
          If Not IsDBNull(r("amod_make_name")) Then
            ModelName = r("amod_make_name").ToString & " "
          End If

          If Not IsDBNull(r("amod_model_name")) Then
            ModelName += r("amod_model_name").ToString
          End If

          searchMakeModel.Items.Add(New ListItem(ModelName, r("amod_id")))
        Next
      End If
    End If

  End Sub

  ''' <summary>
  ''' Fills up Dealer Dropdown List.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillABIDealers()

    Dim DealersTable As New DataTable
    DealersTable = Master.AbiDataManager.GetABIDealers(True, "")

    If Not IsNothing(DealersTable) Then
      If DealersTable.Rows.Count > 0 Then
        For Each r As DataRow In DealersTable.Rows
          If Not IsDBNull(r("comp_name")) Then
            searchDealers.Items.Add(New ListItem(r("comp_name"), r("comp_id")))
          End If
        Next
      End If
    End If

  End Sub

  ''' <summary>
  ''' Fills up the Event List underneath the aviation news
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillEventsList()
    Dim eventTable As New DataTable
    eventTable = Master.AbiDataManager.GetABIEventList(3)

    eventRepeater.DataSource = eventTable
    eventRepeater.DataBind()

  End Sub

  ''' <summary>
  ''' A routine that fills up the latest aviation news repeater (aviation news without pictures)
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillAviationNews()
    Dim ArticleTable As New DataTable
    ArticleTable = Master.AbiDataManager.GetAviationArticles(4, 0, "")

    If Not IsNothing(ArticleTable) Then
      latest_aviation_news_repeater.DataSource = ArticleTable
      latest_aviation_news_repeater.DataBind()
    End If
  End Sub

  ''' <summary>
  ''' Fills up latest jetnet news. Fills up both the article with the picture and the ones below without it.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillJetnetNews()
    Dim ArticleTable As New DataTable
    Dim topTable As New DataTable
    Dim latestTable As New DataTable
    ArticleTable = Master.AbiDataManager.GetLatestJetnetNews(5)

    SeperateJetnetNewsTables(ArticleTable, topTable, latestTable)

    If Not IsNothing(ArticleTable) Then
      jetnetNewsRepeater.DataSource = latestTable
      jetnetNewsRepeater.DataBind()
      topJetnetArticle.DataSource = topTable
      topJetnetArticle.DataBind()
    End If
  End Sub

  ''' <summary>
  ''' This splits the original article table into two so that you only have to query once to get all the jetnet news.
  ''' </summary>
  ''' <param name="ArticleTable"></param>
  ''' <param name="TopTable"></param>
  ''' <param name="latestTable"></param>
  ''' <remarks></remarks>
  Private Sub SeperateJetnetNewsTables(ByVal ArticleTable As DataTable, ByRef TopTable As DataTable, ByRef latestTable As DataTable)
    Dim count As Integer = 0
    TopTable = ArticleTable.Clone
    latestTable = ArticleTable.Clone


    Dim afiltered As DataRow() = ArticleTable.Select("", "evonot_release_date DESC")

    For Each atmpDataRow In afiltered
      If count = 0 Then
        TopTable.ImportRow(atmpDataRow)
      Else
        latestTable.ImportRow(atmpDataRow)
      End If
      count += 1
    Next

    TopTable = AddPicturesToJetnetTable(TopTable)

  End Sub

  ''' <summary>
  ''' Adds a picture to the jetnet news article table that needs it (top 1)
  ''' </summary>
  ''' <param name="ArticleTable"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function AddPicturesToJetnetTable(ByVal ArticleTable As DataTable) As DataTable
    Dim returnTable As New DataTable
    Dim pictureTable As New DataTable
    Dim column As New DataColumn 'Column to add Picture
    Dim column2 As New DataColumn 'Column to add Formatted date
    Dim rowCount As Integer = 0
    returnTable = ArticleTable.Clone

    column.DataType = System.Type.GetType("System.String")
    column.DefaultValue = ""
    column.Unique = False
    column.ColumnName = "picture"
    returnTable.Columns.Add(column)

    column2.DataType = System.Type.GetType("System.String")
    column2.DefaultValue = ""
    column2.Unique = False
    column2.ColumnName = "dateWithoutTime"
    returnTable.Columns.Add(column2)

    'getting picture table
    pictureTable = Master.AbiDataManager.GetRandomEvolutionBackgrounds(1)

    If Not IsNothing(pictureTable) Then
      If pictureTable.Rows.Count > 0 Then

        For Each r As DataRow In ArticleTable.Rows
          Dim newCustomersRow As DataRow = returnTable.NewRow()
          newCustomersRow("evonot_id") = r("evonot_id")
          newCustomersRow("evonot_title") = r("evonot_title")

          If Not IsDBNull(r("evonot_release_date")) Then
            newCustomersRow("dateWithoutTime") = Format(r("evonot_release_date"), "MM/dd/yyyy")
          End If

          If Not IsDBNull(r("evonot_description")) Then
            newCustomersRow("evonot_description") = Left(r("evonot_description"), 50)
            If Len(r("evonot_description")) > 50 Then
              newCustomersRow("evonot_description") += "..."
            End If
          End If


          newCustomersRow("evonot_doc_link") = r("evonot_doc_link")
          newCustomersRow("picture") = "/images/background/" & pictureTable.Rows(rowCount).Item("evoback_id") & ".jpg"

          returnTable.Rows.Add(newCustomersRow)
          returnTable.AcceptChanges()
          rowCount += 1
        Next

      End If
    End If
    Return returnTable

  End Function

  ''' <summary>
  ''' This fills the Latest Aviation Articles (the top two with pictures)
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillLatestAviationArticles()
    Dim ArticleTable As New DataTable
    ArticleTable = Master.AbiDataManager.GetAviationArticles(2, 0, "NEWID()")

    If Not IsNothing(ArticleTable) Then
      ArticleTable = AddPicturesToAviationArticleTable(ArticleTable)

      latest_articles_holder_repeater.DataSource = ArticleTable
      latest_articles_holder_repeater.DataBind()
    End If
  End Sub

  ''' <summary>
  ''' Fills featured Aircraft repeater
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillFeaturedAircraft()
    Dim AircraftTable As New DataTable()

    AircraftTable = Master.AbiDataManager.GetFeaturedAircraft(5)

    If Not IsNothing(AircraftTable) Then
      If AircraftTable.Rows.Count > 0 Then
        featuredAircraftRepeater.DataSource = AircraftTable
        featuredAircraftRepeater.DataBind()
      End If
    End If
  End Sub

  ''' <summary>
  ''' This loops through the Aviation Table and adds evolution background pictures.
  ''' </summary>
  ''' <param name="EditTable"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function AddPicturesToAviationArticleTable(ByVal EditTable As DataTable) As DataTable
    Dim returnTable As New DataTable
    Dim pictureTable As New DataTable
    Dim column As New DataColumn 'Column to add Picture
    Dim column2 As New DataColumn 'Column to add Formatted date
    Dim rowCount As Integer = 0
    returnTable = EditTable.Clone

    column.DataType = System.Type.GetType("System.String")
    column.DefaultValue = ""
    column.Unique = False
    column.ColumnName = "picture"
    returnTable.Columns.Add(column)

    column2.DataType = System.Type.GetType("System.String")
    column2.DefaultValue = ""
    column2.Unique = False
    column2.ColumnName = "dateWithoutTime"
    returnTable.Columns.Add(column2)

    'getting picture table
    pictureTable = Master.AbiDataManager.GetRandomEvolutionBackgrounds(2)

    If Not IsNothing(pictureTable) Then
      If pictureTable.Rows.Count > 0 Then

        For Each r As DataRow In EditTable.Rows
          Dim newCustomersRow As DataRow = returnTable.NewRow()
          newCustomersRow("abinewssrc_name") = r("abinewssrc_name")
          newCustomersRow("abinewslnk_title") = r("abinewslnk_title")

          If Not IsDBNull(r("abinewslnk_date")) Then
            newCustomersRow("dateWithoutTime") = Format(r("abinewslnk_date"), "MM/dd/yyyy")
          End If

          If Not IsDBNull(r("abinewslnk_description")) Then
            newCustomersRow("abinewslnk_description") = Left(r("abinewslnk_description"), 50)
            If Len(r("abinewslnk_description")) > 50 Then
              newCustomersRow("abinewslnk_description") += "..."
            End If
          End If


          newCustomersRow("abinewslnk_web_address") = r("abinewslnk_web_address")
          newCustomersRow("abinewslnk_source_id") = r("abinewslnk_source_id")
          newCustomersRow("picture") = "/images/background/" & pictureTable.Rows(rowCount).Item("evoback_id") & ".jpg"

          returnTable.Rows.Add(newCustomersRow)
          returnTable.AcceptChanges()
          rowCount += 1
        Next

      End If
    End If
    Return returnTable

  End Function

  Private Sub findAircraft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles findAircraft.Click
    Dim pageURL As String = ""
    pageURL = "abiForsale.aspx?"
    If searchMakeModel.SelectedValue > 0 Then
      pageURL += "ID=" + searchMakeModel.SelectedValue
    End If

    If searchDealers.SelectedValue > 0 Then
      pageURL += "&Dealer=" + searchDealers.SelectedValue
    End If

    If Not String.IsNullOrEmpty(year_start.SelectedValue) Then
      pageURL += "&start=" & year_start.SelectedValue
    End If

    If Not String.IsNullOrEmpty(year_end.SelectedValue) Then
      pageURL += "&end=" & year_end.SelectedValue
    End If

    Response.Redirect(pageURL)
  End Sub
End Class