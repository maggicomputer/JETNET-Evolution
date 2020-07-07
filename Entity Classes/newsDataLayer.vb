Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/newsDataLayer.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: newsDataLayer.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class newsSelectionCriteriaClass

  Private _newsCriteriaStatusCode As eObjStatusCode
  Private _newsCriteriaDetailError As eObjDetailErrorCode
  Private _newsCriteriaTopic As Integer
  Private _newsCriteriaTopicName As String
  Private _newsCriteriaMakeName As String
  Private _newsCriteriaModelID As Long
  Private _newsCriteriaDisplayRows As Integer
  Private _newsCriteriaSourceID As Integer

  Sub New()

    _newsCriteriaStatusCode = eObjStatusCode.NULL
    _newsCriteriaDetailError = eObjDetailErrorCode.NULL
    _newsCriteriaTopic = -1
    _newsCriteriaTopicName = ""
    _newsCriteriaMakeName = ""
    _newsCriteriaModelID = -1
    _newsCriteriaDisplayRows = 0
    _newsCriteriaSourceID = 0

  End Sub

  Public Property HelpSelectionCriteriaStatusCode() As eObjStatusCode
    Get
      Return _newsCriteriaStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _newsCriteriaStatusCode = value
    End Set
  End Property

  Public Property HelpSelectionCriteriaDetailError() As eObjDetailErrorCode
    Get
      Return _newsCriteriaDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _newsCriteriaDetailError = value
    End Set
  End Property

  Public Property NewsCriteriaTopic() As Integer
    Get
      Return _newsCriteriaTopic
    End Get
    Set(ByVal value As Integer)
      _newsCriteriaTopic = value
    End Set
  End Property

  Public Property NewsCriteriaTopicName() As String
    Get
      Return _newsCriteriaTopicName
    End Get
    Set(ByVal value As String)
      _newsCriteriaTopicName = value
    End Set
  End Property

  Public Property NewsCriteriaMakeName() As String
    Get
      Return _newsCriteriaMakeName
    End Get
    Set(ByVal value As String)
      _newsCriteriaMakeName = value
    End Set
  End Property

  Public Property NewsCriteriaModelID() As Long
    Get
      Return _newsCriteriaModelID
    End Get
    Set(ByVal value As Long)
      _newsCriteriaModelID = value
    End Set
  End Property

  Public Property NewsCriteriaDisplayRows() As Integer
    Get
      Return _newsCriteriaDisplayRows
    End Get
    Set(ByVal value As Integer)
      _newsCriteriaDisplayRows = value
    End Set
  End Property

  Public Property NewsCriteriaSourceID() As Integer
    Get
      Return _newsCriteriaSourceID
    End Get
    Set(ByVal value As Integer)
      _newsCriteriaSourceID = value
    End Set
  End Property

End Class  ' 

<System.Serializable()> Public Class newsDataLayer

  Private aError As String

  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String

  Sub New()

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

  End Sub

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

  Public Property adminConnectStr() As String
    Get
      adminConnectStr = adminConnectString
    End Get
    Set(ByVal value As String)
      adminConnectString = value
    End Set
  End Property

  Public Property clientConnectStr() As String
    Get
      clientConnectStr = clientConnectString
    End Get
    Set(ByVal value As String)
      clientConnectString = value
    End Set
  End Property

  Public Property starConnectStr() As String
    Get
      starConnectStr = starConnectString
    End Get
    Set(ByVal value As String)
      starConnectString = value
    End Set
  End Property

  Public Property cloudConnectStr() As String
    Get
      cloudConnectStr = cloudConnectString
    End Get
    Set(ByVal value As String)
      cloudConnectString = value
    End Set
  End Property

  Public Property serverConnectStr() As String
    Get
      serverConnectStr = serverConnectString
    End Get
    Set(ByVal value As String)
      serverConnectString = value
    End Set
  End Property

  Public Function get_news_display_topics_info(ByRef searchCriteria As newsSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT ABI_News.abinews_topic, ABI_News.abinews_id FROM ABI_News_Links WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN ABI_News_Source WITH(NOLOCK) ON ABI_News_Links.abinewslnk_source_id = ABI_News_Source.abinewssrc_id")
      sQuery.Append(" LEFT OUTER JOIN ABI_News_index WITH(NOLOCK) ON ABI_News_Links.abinewslnk_id = ABI_News_index.abinewsind_link_id")
      sQuery.Append(" INNER JOIN ABI_News WITH(NOLOCK) ON ABI_News_index.abinewsind_cat_id = ABI_News.abinews_id")

      If Not String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) Or searchCriteria.NewsCriteriaModelID > -1 Then
        sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) on ABI_News_Links.abinewslnk_amod_id = amod_id")
      End If

      sQuery.Append(" WHERE ")

      If Not String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) Then
        sQuery.Append("(amod_make_name = '" + searchCriteria.NewsCriteriaMakeName.Trim + "')")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      If searchCriteria.NewsCriteriaModelID > -1 Then
        sQuery.Append(sSeperator + "(amod_id = " + searchCriteria.NewsCriteriaModelID.ToString + ")")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      If searchCriteria.NewsCriteriaTopic > -1 Then
        sQuery.Append(sSeperator + "(abinews_id = " + searchCriteria.NewsCriteriaTopic.ToString + ")")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      sQuery.Append(sSeperator + "(abinewslnk_date >= (getdate()-14))")

      sQuery.Append(" GROUP BY ABI_News.abinews_topic, abinews_id")
      sQuery.Append(" ORDER BY ABI_News.abinews_topic, abinews_id")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_news_display_topics_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_news_display_topics_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_news_display_topics_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub news_display_topics(ByRef searchCriteria As newsSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_news_display_topics_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='newsTopicsDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td width='2%' align='center' valign='middle' class='seperator'><img src='images/ch_red.jpg'></td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator'>")

            If Not IsDBNull(r.Item("abinews_topic")) Then
              If Not String.IsNullOrEmpty(r.Item("abinews_topic").ToString.Trim) Then
                If searchCriteria.NewsCriteriaTopic > -1 Then
                  htmlOut.Append("<strong>" + r.Item("abinews_topic").ToString.Trim + "</strong>")
                Else
                  htmlOut.Append("<a href='evoNews.aspx?newsMake=" + searchCriteria.NewsCriteriaMakeName.Trim + "&newsModel=" + searchCriteria.NewsCriteriaModelID.ToString + "&newsTopic=" + r.Item("abinews_id").ToString.Trim + "' target='_self' title='Click to view news topic'>" + r.Item("abinews_topic").ToString.Trim + "</a>")
                End If

                If CInt(r.Item("abinews_id").ToString) = searchCriteria.NewsCriteriaTopic Then
                  searchCriteria.NewsCriteriaTopicName = r.Item("abinews_topic").ToString.Trim
                End If

              End If
            End If

            htmlOut.Append("</td></tr>")

          Next

          If searchCriteria.NewsCriteriaTopic > -1 Then
            htmlOut.Append("<tr><td colspan='2' align='center' valign='middle' class='seperator'>&nbsp;</td></tr>")
            htmlOut.Append("<tr><td colspan='2' align='right' valign='middle' class='seperator'>")
            htmlOut.Append("<a href='evoNews.aspx?newsMake=" + searchCriteria.NewsCriteriaMakeName.Trim + "&newsModel=" + searchCriteria.NewsCriteriaModelID.ToString + "&newsTopic=-1' target='_self' title='Click to clear news topic'>Clear news topic</a>")
            htmlOut.Append("</td></tr>")
          End If

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='newsTopicsDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='newsTopicsDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in news_display_topics(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_news_topics_by_make_model_info(ByRef searchCriteria As newsSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) And searchCriteria.NewsCriteriaModelID = -1 Then

        sQuery.Append("SELECT DISTINCT TOP 50 amod_make_name, amod_id FROM ABI_News_Links WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN ABI_News_Source WITH(NOLOCK) ON ABI_News_Links.abinewslnk_source_id = ABI_News_Source.abinewssrc_id")
        sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON abinewslnk_amod_id = amod_id")

        If searchCriteria.NewsCriteriaTopic > -1 Then
          sQuery.Append(" INNER JOIN ABI_News_index WITH(NOLOCK) ON ABI_News_Links.abinewslnk_id = ABI_News_index.abinewsind_link_id")
          sQuery.Append(" INNER JOIN ABI_News WITH(NOLOCK) ON ABI_News_index.abinewsind_cat_id = ABI_News.abinews_id")
        End If

        sQuery.Append(" WHERE (abinewslnk_make_name IS NOT NULL AND abinewslnk_make_name <> '0') AND abinewslnk_date >= (getdate()-14)")

        If searchCriteria.NewsCriteriaTopic > -1 Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "(abinews_id = " + searchCriteria.NewsCriteriaTopic.ToString + ")")
        End If

        sQuery.Append(" GROUP BY amod_make_name, amod_id")
        sQuery.Append(" ORDER BY amod_make_name, amod_id")

      Else

        sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id FROM ABI_News_Links WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN ABI_News_Source WITH(NOLOCK) ON ABI_News_Links.abinewslnk_source_id = ABI_News_Source.abinewssrc_id")
        sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON abinewslnk_amod_id = amod_id")

        If searchCriteria.NewsCriteriaTopic > -1 Then
          sQuery.Append(" INNER JOIN ABI_News_index WITH(NOLOCK) ON ABI_News_Links.abinewslnk_id = ABI_News_index.abinewsind_link_id")
          sQuery.Append(" INNER JOIN ABI_News WITH(NOLOCK) ON ABI_News_index.abinewsind_cat_id = ABI_News.abinews_id")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) Then
          sQuery.Append(" WHERE (amod_make_name = '" + searchCriteria.NewsCriteriaMakeName.Trim + "')")
          sQuery.Append(crmWebClient.Constants.cAndClause + "(abinewslnk_date >= (getdate()-14))")
        ElseIf searchCriteria.NewsCriteriaModelID > -1 Then
          sQuery.Append(" WHERE (amod_id = " + searchCriteria.NewsCriteriaModelID.ToString + ")")
        End If

        If searchCriteria.NewsCriteriaTopic > -1 Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "(abinews_id = " + searchCriteria.NewsCriteriaTopic.ToString + ")")
        End If

        sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
        sQuery.Append(" ORDER BY amod_make_name, amod_model_name")

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_news_topics_by_make_model_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_news_topics_by_make_model_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_news_topics_by_make_model_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub news_display_topics_by_make_model(ByRef searchCriteria As newsSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strLastMake As String = ""

    Try

      results_table = get_news_topics_by_make_model_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) And searchCriteria.NewsCriteriaModelID = -1 Then

            htmlOut.Append("<table id='newsTopicsMakeModelDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

            For Each r As DataRow In results_table.Rows

              If strLastMake.ToLower.Trim <> r.Item("amod_make_name").ToString.ToLower.Trim Then

                If Not toggleRowColor Then
                  htmlOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  htmlOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If

                htmlOut.Append("<td width='2%' align='center' valign='middle' class='seperator'><img src='images/ch_red.jpg'></td>")
                htmlOut.Append("<td align='left' valign='middle' class='seperator'>")
                htmlOut.Append("<a href='evoNews.aspx?newsMake=" + HttpContext.Current.Server.UrlEncode(r.Item("amod_make_name").ToString.Trim) + "&newsModel=-1&newsTopic=" + searchCriteria.NewsCriteriaTopic.ToString + "' target='_self' title='Click to view news by aircraft make'>" + r.Item("amod_make_name").ToString.Trim + "</a>")
                htmlOut.Append("</td></tr>")

                strLastMake = r.Item("amod_make_name").ToString.Trim

              End If

            Next

            htmlOut.Append("</table>")

          Else

            htmlOut.Append("<table id='newsTopicsMakeModelDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

            For Each r As DataRow In results_table.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td width='2%' align='center' valign='middle' class='seperator'><img src='images/ch_red.jpg'></td>")
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>")
              If searchCriteria.NewsCriteriaModelID > -1 Then
                htmlOut.Append("<strong>" + r.Item("amod_make_name").ToString.Trim + " / " + r.Item("amod_model_name").ToString.Trim + "</strong>")
              Else
                htmlOut.Append("<a href='evoNews.aspx?newsMake=&newsModel=" + r.Item("amod_id").ToString.Trim + "&newsTopic=" + searchCriteria.NewsCriteriaTopic.ToString + "' target='_self' title='Click to view news by aircraft model'>" + r.Item("amod_make_name").ToString.Trim + " / " + r.Item("amod_model_name").ToString.Trim + "</a>")
              End If
              htmlOut.Append("</td></tr>")

            Next

            If searchCriteria.NewsCriteriaModelID > -1 Then
              htmlOut.Append("<tr><td colspan='2' align='center' valign='middle' class='seperator'>&nbsp;</td></tr>")
              htmlOut.Append("<tr><td colspan='2' align='right' valign='middle' class='seperator'>")
              htmlOut.Append("<a href='evoNews.aspx?newsMake=" + searchCriteria.NewsCriteriaMakeName.Trim + "&newsModel=-1&newsTopic=" + searchCriteria.NewsCriteriaTopic.ToString + "' target='_self' title='Click to clear news model'>Clear model</a>")
              htmlOut.Append("</td></tr>")
            Else
              htmlOut.Append("<tr><td colspan='2' align='center' valign='middle' class='seperator'>&nbsp;</td></tr>")
              htmlOut.Append("<tr><td colspan='2' align='right' valign='middle' class='seperator'>")
              htmlOut.Append("<a href='evoNews.aspx?newsMake=&newsModel=-1&newsTopic=" + searchCriteria.NewsCriteriaTopic.ToString + "' target='_self' title='Click to clear news make'>Clear make</a>")
              htmlOut.Append("</td></tr>")

            End If

            htmlOut.Append("</table>")

          End If

        Else
          htmlOut.Append("<table id='newsTopicsMakeModelDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='newsTopicsMakeModelDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in news_display_topics_by_make_model(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_news_evolution_notifications_info(ByRef searchCriteria As newsSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM evolution_notifications WHERE (evonot_release_type IN ('N','J')")
      sQuery.Append(" AND evonot_evo_dotnet_flag = 'Y' AND evonot_active_flag = 'Y')")
      sQuery.Append(" OR (evonot_release_type in ('N', 'J') AND NOT evonot_product_crm_flag = 'Y' AND NOT evonot_evo_dotnet_only_flag = 'Y')")
      sQuery.Append(" ORDER BY evonot_release_date DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_news_evolution_notifications_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_news_evolution_notifications_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_news_evolution_notifications_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub news_display_evolution_notifications(ByRef searchCriteria As newsSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_news_evolution_notifications_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='newsEvolutionNotificationsDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td width='2%' align='center' valign='top' class='seperator'><img src='images/ch_red.jpg'></td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator'><a href='")

            If Not IsDBNull(r.Item("evonot_doc_link")) Then
              If Not r.Item("evonot_doc_link").ToString.ToLower.Contains("http") Then
                htmlOut.Append("http://")
              End If
              htmlOut.Append(r.Item("Evonot_doc_link").ToString.Trim)
            End If

            htmlOut.Append("' target='_new' title='Click to viewnews article'><font color='black'>" + FormatDateTime(r.Item("evonot_release_date").ToString, DateFormat.ShortDate).Trim + "</font> - ")
            htmlOut.Append(r.Item("evonot_title").ToString.Trim + "</a><br />")

            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
              Dim tmpDescription As String = r.Item("evonot_announcement").ToString.Trim
              htmlOut.Append(Replace(Replace(tmpDescription, "<p>", ""), "</p>", ""))
            End If

            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='newsEvolutionNotificationsDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='newsEvolutionNotificationsDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in news_display_evolution_notifications(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_news_display_main_block_info(ByRef searchCriteria As newsSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT TOP " + CInt(searchCriteria.NewsCriteriaDisplayRows * 3).ToString + " abinewssrc_name, abinewslnk_title, abinewslnk_date, abinewslnk_description, abinewslnk_web_address, abinewslnk_source_id")
      sQuery.Append(" FROM ABI_News_Links WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN ABI_News_Source WITH(NOLOCK) ON abinewslnk_source_id = abinewssrc_id")

      If Not String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) Or searchCriteria.NewsCriteriaModelID > -1 Then
        sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) on ABI_News_Links.abinewslnk_amod_id = amod_id")
      End If

      If searchCriteria.NewsCriteriaTopic > -1 Then
        sQuery.Append(" INNER JOIN ABI_News_Index WITH(NOLOCK) on ABI_News_Index.abinewsind_link_id = abinewslnk_id")
        sQuery.Append(" INNER JOIN ABI_News WITH(NOLOCK) on ABI_News.abinews_id = abinewsind_cat_id")
      End If

      sQuery.Append(" WHERE ")

      If Not String.IsNullOrEmpty(searchCriteria.NewsCriteriaMakeName.Trim) Then
        sQuery.Append("(amod_make_name = '" + searchCriteria.NewsCriteriaMakeName.Trim + "')")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      If searchCriteria.NewsCriteriaModelID > -1 Then
        sQuery.Append(sSeperator + "(amod_id = " + searchCriteria.NewsCriteriaModelID.ToString + ")")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      If searchCriteria.NewsCriteriaTopic > -1 Then
        sQuery.Append(sSeperator + "(abinews_id = " + searchCriteria.NewsCriteriaTopic.ToString + ")")
        sSeperator = crmWebClient.Constants.cAndClause
      End If

      If searchCriteria.NewsCriteriaModelID = -1 Then
        sQuery.Append(sSeperator + "(abinewslnk_date >= (getdate()-14))")
      End If

      sQuery.Append(" ORDER BY abinewslnk_date DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_news_display_main_block_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_news_display_main_block_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_news_display_main_block_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub news_display_main_block(ByRef searchCriteria As newsSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim columnCount As Integer = 0
    Dim sNewsLink As String = ""

    Try

      results_table = get_news_display_main_block_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='newsMainBlockDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'>")

          For Each r As DataRow In results_table.Rows

            If columnCount = 0 Then
              htmlOut.Append("<tr bgcolor='white'>")
            End If

            If columnCount < 3 Then
              htmlOut.Append("<td width='33%' align='center' valign='left' class='seperator'>")

              htmlOut.Append("<table id='newsBlockItemTable' cellpadding='2' cellspacing='0'><tr>") ' table item block

              htmlOut.Append("<td width='2%' align='center' valign='top'><img src='images/ch_red.jpg'></td>")
              htmlOut.Append("<td align='left' valign='middle'>")

              If Not IsDBNull(r.Item("abinewslnk_web_address")) Then
                If Not r.Item("abinewslnk_web_address").ToString.ToLower.Contains("http") Then
                  sNewsLink = "http://" + r.Item("abinewslnk_web_address").ToString.Trim
                End If
                sNewsLink = r.Item("abinewslnk_web_address").ToString.Trim
              End If

              If Not IsDBNull(r.Item("abinewslnk_title")) Then
                If Not String.IsNullOrEmpty(r.Item("abinewslnk_title").ToString.Trim) Then
                  htmlOut.Append("<a href='" + sNewsLink + "' target='_blank'>")
                  htmlOut.Append("<strong>" + r.Item("abinewslnk_title").ToString.Trim + "</strong></a>")
                End If
              End If

              If Not IsDBNull(r.Item("abinewslnk_date")) Then
                If Not String.IsNullOrEmpty(r.Item("abinewslnk_date").ToString.Trim) Then
                  htmlOut.Append("<br /><font color='blue'>" + FormatDateTime(r.Item("abinewslnk_date").ToString.Trim, DateFormat.ShortDate) + "</font>")
                End If
              End If

              If Not IsDBNull(r.Item("abinewslnk_description")) Then
                If Not String.IsNullOrEmpty(r.Item("abinewslnk_description").ToString) Then
                  If r.Item("abinewslnk_description").ToString.Length > 149 Then
                    htmlOut.Append("<br />" + r.Item("abinewslnk_description").ToString.Substring(0, 150).Trim + "...")
                  Else
                    htmlOut.Append("<br />" + r.Item("abinewslnk_description").ToString.Trim)
                  End If
                End If
              End If

              If Not IsDBNull(r.Item("abinewssrc_name")) Then
                If Not String.IsNullOrEmpty(r.Item("abinewssrc_name").ToString) Then
                  htmlOut.Append("<br />[<em><a href='" + sNewsLink + "' target='_blank'>More at ")
                  htmlOut.Append(r.Item("abinewssrc_name").ToString.Trim)
                  htmlOut.Append("</a></em>]")
                Else
                  htmlOut.Append("<br />[<em>no source available</em>]")
                End If
              Else
                htmlOut.Append("<br />[<em>no source available</em>]")
              End If

              htmlOut.Append("</td></tr></table>") ' end table item block
              htmlOut.Append("</td>")

              columnCount += 1
            End If

            If columnCount = 3 Then
              columnCount = 0
              htmlOut.Append("</tr>")
            End If

          Next

          ' if data is less than 3 items fill in the extra "items" for the row
          If columnCount = 2 Then
            htmlOut.Append("<td width='33%' align='center' valign='middle' class='seperator'>&nbsp;</td>")
            htmlOut.Append("</tr>")
          ElseIf columnCount = 1 Then
            htmlOut.Append("<td width='33%' align='center' valign='middle' class='seperator'>&nbsp;</td>")
            htmlOut.Append("<td width='33%' align='center' valign='middle' class='seperator'>&nbsp;</td>")
            htmlOut.Append("</tr>")
          End If

          htmlOut.Append("</table>") ' end table main block

        Else
          htmlOut.Append("<table id='newsMainBlockDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='newsMainBlockDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No News Topics Found</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in news_display_main_block(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

End Class
