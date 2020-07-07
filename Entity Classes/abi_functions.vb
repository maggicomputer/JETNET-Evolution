Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/abi_functions.vb $
'$$Author: Mike $
'$$Date: 5/04/20 2:34p $
'$$Modtime: 5/04/20 2:28p $
'$$Revision: 3 $
'$$Workfile: abi_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class abi_functions

  Private aError As String
  Private clientConnectString As String
  Public Shared adminConnectString As String

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

#Region "database_connection_strings"

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

#End Region

#Region "News"
  Public Function GetNewsCategories() As DataTable
    '-- ***********************
    '-- LIST OF News Categories


    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "SELECT  ABI_News.abinews_topic, ABI_News.abinews_id, COUNT(*) as tcount FROM ABI_News_Links WITH(NOLOCK) INNER JOIN"
      sqlQuery += " ABI_News_Source WITH(NOLOCK) ON ABI_News_Links.abinewslnk_source_id = ABI_News_Source.abinewssrc_id LEFT OUTER JOIN"
      sqlQuery += " ABI_News_index WITH(NOLOCK) ON ABI_News_Links.abinewslnk_id = ABI_News_index.abinewsind_link_id INNER JOIN"
      sqlQuery += " ABI_News WITH(NOLOCK) ON ABI_News_index.abinewsind_cat_id = ABI_News.abinews_id"
      sqlQuery += " where (abinewslnk_date >= (getdate() - 14)) "
      sqlQuery += " GROUP BY ABI_News.abinews_topic, abinews_id"
      sqlQuery += " ORDER BY ABI_News.abinews_topic, abinews_id"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetNewsCategories() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetNewsCategories = atemptable
    Catch ex As Exception
      GetNewsCategories = Nothing
      Me.class_error = "Error in GetNewsCategories() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
  Public Function GetAviationArticlesByModel(ByVal amod_id As Long) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT TOP 15 abinewssrc_name, abinewslnk_title, abinewslnk_date, abinewslnk_description, abinewslnk_web_address, "
      sqlQuery += " abinewslnk_source_id FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) "
      sqlQuery += " ON abinewslnk_source_id = abinewssrc_id INNER JOIN aircraft_model WITH(NOLOCK) on "
      sqlQuery += " ABI_News_Links.abinewslnk_amod_id = amod_id WHERE amod_id = @amod_id "
      sqlQuery += " AND (abinewslnk_date >= (getdate()-60)) ORDER BY abinewslnk_date DESC"



      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetAviationArticlesByModel(ByVal amod_id As Long) As DataTable</b><br />" & sqlQuery

      If amod_id > 0 Then
        SqlCommand.Parameters.AddWithValue("amod_id", amod_id)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetAviationArticlesByModel = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetAviationArticlesByModel = Nothing
      Me.class_error = "Error in GetAviationArticlesByModel(ByVal amod_id As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function
  Public Function GetAviationArticlesByMake(ByVal amod_make_name As String) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT TOP 15 abinewssrc_name, abinewslnk_title, abinewslnk_date, abinewslnk_description, abinewslnk_web_address, "
      sqlQuery += " abinewslnk_source_id FROM ABI_News_Links WITH(NOLOCK) INNER JOIN ABI_News_Source WITH(NOLOCK) "
      sqlQuery += " ON abinewslnk_source_id = abinewssrc_id INNER JOIN aircraft_model WITH(NOLOCK) on "
      sqlQuery += " ABI_News_Links.abinewslnk_amod_id = amod_id WHERE (upper(amod_make_name) = @amod_make_name) "
      sqlQuery += " AND (abinewslnk_date >= (getdate()-60)) ORDER BY abinewslnk_date DESC"



      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetAviationArticlesByMake(ByVal amod_make_name As String) As DataTable</b><br />" & sqlQuery

      If amod_make_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_make_name", amod_make_name.ToUpper)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetAviationArticlesByMake = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetAviationArticlesByMake = Nothing
      Me.class_error = "Error in GetAviationArticlesByMake(ByVal amod_make_name As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function
  Public Function GetAviationArticles(ByVal articleCount As Integer, ByVal topicID As Long, ByVal orderBy As String) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT "

      If articleCount > 0 Then
        sqlQuery += " TOP " & articleCount
      End If

      sqlQuery += " abinewssrc_name, upper(abinewslnk_title) as abinewslnk_title, abinewslnk_date, abinewslnk_description, "
      sqlQuery += " abinewslnk_web_address, abinewslnk_source_id "

      If topicID > 0 Then
        sqlQuery += " , abinews_topic "
      End If

      sqlQuery += " FROM ABI_News_Links WITH(NOLOCK) "
      sqlQuery += " INNER JOIN ABI_News_Source WITH(NOLOCK) ON abinewslnk_source_id = abinewssrc_id "

      If topicID > 0 Then
        sqlQuery += " INNER JOIN ABI_News_Index WITH(NOLOCK) on ABI_News_Index.abinewsind_link_id = abinewslnk_id "
        sqlQuery += " INNER JOIN ABI_News WITH(NOLOCK) on ABI_News.abinews_id = abinewsind_cat_id "
      End If

      sqlQuery += " WHERE "

      If topicID > 0 Then
        sqlQuery += " abinews_id = @topicID "
      Else
        sqlQuery += " (abinewslnk_date >= (getdate() - 7))"
      End If

      sqlQuery += " ORDER BY "

      If orderBy = "" Then
        sqlQuery += " abinewslnk_date DESC"
      Else
        sqlQuery += orderBy
      End If

      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetAviationArticles(ByVal articleCount As Integer) As DataTable</b><br />" & sqlQuery

      If topicID > 0 Then
        SqlCommand.Parameters.AddWithValue("topicID", topicID)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetAviationArticles = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetAviationArticles = Nothing
      Me.class_error = "Error in GetAviationArticles(ByVal articleCount As Integer) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function
#End Region
#Region "Jetnet News"


  ''' <summary>
  ''' Returns latest jetnet news. Parameter is how many articles to return.
  ''' </summary>
  ''' <param name="articleCount"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetLatestJetnetNews(ByVal articleCount As Integer) As DataTable
    '-- ********************* BLOCK 4 - TOP JETNET NEWS ARTICLES ********************************
    '-- GET TOP LATEST JETNET NEWS ARTICLES

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "SELECT top " & articleCount & " evonot_id, evonot_release_date,"
      sqlQuery += " evonot_title, evonot_doc_link, evonot_announcement, evonot_description "
      sqlQuery += " FROM evolution_notifications"
      sqlQuery += " WHERE evonot_release_type in ('N', 'J') AND NOT evonot_product_crm_flag = 'Y' "
      sqlQuery += " AND NOT evonot_evo_dotnet_only_flag = 'Y' "
      sqlQuery += " ORDER BY evonot_release_date DESC"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetLatestJetnetNews(ByVal articleCount As Integer) As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetLatestJetnetNews = atemptable
    Catch ex As Exception
      GetLatestJetnetNews = Nothing
      Me.class_error = "Error in GetLatestJetnetNews(ByVal articleCount As Integer) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
#End Region
#Region "Models"
  ''' <summary>
  ''' Dataquery to return model list for ABI
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIAircraftModelList() As DataTable
    '-- THERE IS CURRENTLY A BLOCK ON THE OLD ABI SITE LABELED AS AIRCRAFT FOR SALE WHERE
    '-- THE USER CAN TYPE CLICK ON SOME LINKS OR DO A QUICK SEARCH BY SELECTING EITHER A MODEL OR
    '-- A DEALER TO SEARCH ON.  WE WOULD LIKE TO KEEP THIS IN THE SPOT WHERE THE PIMARY PICTURE IS 
    '-- IF IT LOOKS GOOD THERE.
    '-- BELOW IS QUERY FOR DROP DOWN FOR THE MODEL LIST

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = " SELECT DISTINCT amod_make_name, amod_model_name, amod_id "
      sqlQuery += " from ABI_Company_Service WITH(NOLOCK) "
      sqlQuery += " INNER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON abicserv_comp_id = comp_id "
      sqlQuery += " WHERE (ac_forsale_flag = 'Y') AND (amod_customer_flag = 'Y') "
      sqlQuery += " AND (abicserv_serv_code = 'ACLIST' AND (abicserv_status = 'A')) "
      sqlQuery += " AND ((abicserv_end_date >= GETDATE() - 1) AND (abicserv_start_date <= GETDATE())) "
      sqlQuery += " AND ( (ac_lifecycle_stage IN (2, 3) AND cref_contact_type IN ('99', '00', '38')) "
      sqlQuery += " OR (ac_lifecycle_stage = 1 "
      sqlQuery += " AND cref_contact_type IN ('99', '38')) ) "
      sqlQuery += " ORDER BY amod_make_name, amod_model_name "

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIAircraftModelList() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIAircraftModelList = atemptable
    Catch ex As Exception
      GetABIAircraftModelList = Nothing
      Me.class_error = "Error in GetABIAircraftModelList() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function DisplayRightHandColumn(ByVal aircraftData As DataTable, ByVal inquireLink As Boolean) As String
    Dim DisplayStr As String = ""

    If inquireLink Then
      DisplayStr = "<span class=""span4""><h4 class=""uppercase"">"
    Else
      DisplayStr = "<span class=""span6 seperatorRight""><h4 class=""uppercase"">"
    End If


    If Not IsDBNull(aircraftData.Rows(0).Item("ac_mfr_year")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_mfr_year")) Then
        DisplayStr += aircraftData.Rows(0).Item("ac_mfr_year").ToString & " "
      End If
    End If

    If Not IsDBNull(aircraftData.Rows(0).Item("amod_make_name")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_make_name")) Then
        DisplayStr += aircraftData.Rows(0).Item("amod_make_name").ToString & " "
      End If
    End If

    If Not IsDBNull(aircraftData.Rows(0).Item("amod_model_name")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_model_name")) Then
        DisplayStr += aircraftData.Rows(0).Item("amod_model_name").ToString
      End If
    End If

    DisplayStr += "</h4>"

    If inquireLink Then
      DisplayStr += "</span>"
      DisplayStr += "<span class=""span4"">"
    End If

    'Ser #
    'If Not IsDBNull(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
    '  If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
    '    DisplayStr += "<strong class=""blue"">SERIAL:</strong> "
    '    DisplayStr += aircraftData.Rows(0).Item("ac_ser_no_full").ToString & "&nbsp;&nbsp;&nbsp;"
    '  End If
    'End If

    'Reg #
    If Not IsDBNull(aircraftData.Rows(0).Item("ac_country_of_registration")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_country_of_registration")) Then
        DisplayStr += "<strong class=""blue"">Country Of REG:</strong> "

        DisplayStr += aircraftData.Rows(0).Item("ac_country_of_registration").ToString & "&nbsp;&nbsp;&nbsp;"

      End If
    End If



    'Status
    If Not IsDBNull(aircraftData.Rows(0).Item("ac_status")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_status")) Then
        DisplayStr += "<strong class=""blue"">STATUS:</strong> "
        DisplayStr += aircraftData.Rows(0).Item("ac_status") & "&nbsp;&nbsp;&nbsp;"
      End If
    End If

    DisplayStr += "<br />"

    'Asking
    If Not IsDBNull(aircraftData.Rows(0).Item("ac_asking")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_asking")) Then

        If UCase(aircraftData.Rows(0).Item("ac_asking")) = "PRICE" Then
          DisplayStr += "<strong class=""blue"">ASKING PRICE:</strong> "
          DisplayStr += FormatCurrency(aircraftData.Rows(0).Item("ac_asking_price"), 0) & "&nbsp;&nbsp;&nbsp;"
        Else
          DisplayStr += "<strong class=""blue"">ASKING:</strong> "
          DisplayStr += aircraftData.Rows(0).Item("ac_asking") & "&nbsp;&nbsp;&nbsp;"
        End If

      End If
    End If


    'Date Listed
    If Not IsDBNull(aircraftData.Rows(0).Item("ac_days_on_market")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_days_on_market")) Then
        DisplayStr += "<strong class=""blue"">DAYS ON MARKET:</strong> "
        DisplayStr += aircraftData.Rows(0).Item("ac_days_on_market") & "&nbsp;&nbsp;&nbsp;"
      End If
    End If


    'AFTT
    If Not IsDBNull(aircraftData.Rows(0).Item("ac_airframe_tot_hrs")) Then
      If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_airframe_tot_hrs")) Then
        DisplayStr += "<br /><strong class=""blue"">AFTT:</strong> "
        DisplayStr += aircraftData.Rows(0).Item("ac_airframe_tot_hrs").ToString & "&nbsp;&nbsp;&nbsp;"
      End If
    End If

    DisplayStr += "<br />"

    If inquireLink Then
      DisplayStr += "&#10149; <a href=""/abiFiles/abiContact.aspx?acID=" & aircraftData.Rows(0).Item("ac_id") & """ class=""underline"">INQUIRE ABOUT THIS AIRCRAFT</a>"
      DisplayStr += "<br /><hr />"
    Else
      DisplayStr += "</span>"
      DisplayStr += "<span class=""span6"">"
    End If

    'Set up company information
    If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
      DisplayStr += "<img src=""http://www.jetnetevolution.com/pictures/company/" + aircraftData.Rows(0).Item("comp_id").ToString + ".jpg"" alt=""" & aircraftData.Rows(0).Item("comp_name") & """ " & IIf(inquireLink, "width=""100%""", " style='max-height:100px !important;'") & " />"
    Else
      DisplayStr += "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" + aircraftData.Rows(0).Item("comp_id").ToString + ".jpg"" alt=""" & aircraftData.Rows(0).Item("comp_name") & """ " & IIf(inquireLink, "width=""100%""", " style='max-height:100px !important;'") & " />" '"http://www.jetnetGlobal.com/photos/company"
    End If


    DisplayStr += "<h4 class='size15 margin_top'>" & aircraftData.Rows(0).Item("comp_name") & "</h4>"
    DisplayStr += "<p>" & abi_functions.DisplayCompanyInformation(aircraftData.Rows(0).Item("comp_id"), aircraftData.Rows(0).Item("comp_address1"), aircraftData.Rows(0).Item("comp_address2"), aircraftData.Rows(0).Item("comp_city"), aircraftData.Rows(0).Item("comp_state"), aircraftData.Rows(0).Item("comp_zip_code"), aircraftData.Rows(0).Item("comp_country"), aircraftData.Rows(0).Item("comp_web_address")) & "</p>"

    DisplayStr += "</span>"
    Return DisplayStr

  End Function

  ''' <summary>
  ''' An important note on this - this will grab the distinct list (whatever fields you pass it). Don't use this unless you're 
  ''' Passing those fields yourself. Don't run this from a query string (for instance) because it is not parameterized.
  ''' It's really just meant for the Model Footer List (plus click into), 
  ''' but I wanted to leave an opening in case we could use it somewhere else as well.
  ''' </summary>
  ''' <param name="SelectDistinctList"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIAircraftDistinctList(ByVal SelectDistinctList As String) As DataTable
    '-- On the old site we put a series of make links in the footer. See below.  
    'Here is a query that will get you those. This is not something that we need to 
    'rerun on every page – it would be fine if it ran once per day but could always
    'be displayed in the footer.  Clicking on the footer links would take you to a 
    'list of aircraft using the second query.

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = " SELECT distinct " & SelectDistinctList
      sqlQuery += " FROM View_ABI_Aircraft_For_Sale "
      sqlQuery += " order by " & SelectDistinctList


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIAircraftDistinctList(ByVal SelectDistinctList As String) As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIAircraftDistinctList = atemptable
    Catch ex As Exception
      GetABIAircraftDistinctList = Nothing
      Me.class_error = "Error in GetABIAircraftDistinctList(ByVal SelectDistinctList As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function Get_Model_By_ID(ByVal amod_ID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "SELECT * from aircraft_model WITH(NOLOCK) "

      If amod_ID > 0 Then
        sql += " WHERE (amod_id = @amod_ID)"
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Model_By_ID(ByVal amod_ID As Long) As DataTable</b><br />" & sql



      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

      If amod_ID > 0 Then
        SqlCommand.Parameters.AddWithValue("amod_ID", amod_ID)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try

      Get_Model_By_ID = TempTable

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Get_Model_By_ID = Nothing
      Me.class_error = "Error in Get_Model_By_ID(ByVal amod_ID As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function

  Public Function Get_Engine_Info_By_ID(ByVal amod_ID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "SELECT ameng_engine_name, ameng_mfr_name, ameng_mfr_name_abbrev FROM Aircraft_Model_Engine"

      If amod_ID > 0 Then
        sql += " WHERE (ameng_amod_id = @ameng_amod_id)"
      End If

      sql += " ORDER BY ameng_engine_name"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Public Function Get_Engine_Info_By_ID(ByVal amod_ID As Long) As DataTable</b><br />" & sql



      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

      If amod_ID > 0 Then
        SqlCommand.Parameters.AddWithValue("ameng_amod_id", amod_ID)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try

      Get_Engine_Info_By_ID = TempTable

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Get_Engine_Info_By_ID = Nothing
      Me.class_error = "Error in Get_Engine_Info_By_ID(ByVal amod_ID As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function

  Public Function Get_Model_By_Make(ByVal amod_make_name As String) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "SELECT DISTINCT amod_id, amod_make_name, amod_model_name, count(distinct ac_id) as tcount from View_ABI_Aircraft_For_Sale  "

      If amod_make_name <> "" Then
        sql += " WHERE (amod_make_name = @amod_make_name)"
      End If

      sql += " group by amod_id, amod_make_name, amod_model_name"
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Model_By_Make(ByVal amod_make_name As String) As DataTable</b><br />" & sql


      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

      If amod_make_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_make_name", amod_make_name)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try

      Get_Model_By_Make = TempTable

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Get_Model_By_Make = Nothing
      Me.class_error = "Error in Get_Model_By_Make(ByVal amod_make_name As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

  End Function

#End Region
#Region "Aircraft"
  ''' <summary>
  ''' Returns list of featured aircraft. Parameter is how many aircraft to return.
  ''' </summary>
  ''' <param name="acCount"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetFeaturedAircraft(ByVal acCount As Integer) As DataTable
    '-- **************************** BLOCK 2 - FEATURED AIRCRAFT ***********************************
    '-- FEATURED AIRCRAFT - GET 5 RANDOM AIRCRAFT TO DISPLAY AS FEATURED AIRCRAFT
    '-- DISPLAY IMAGE TO LEFT - 3 LINES TO RIGHT OF IMAGE
    '-- LINE 1. IN BLACK - MAKE MODEL
    '-- LINE 2. BOLD - DEALER NAME (NOTE THAT THIS COULD WRAP TO TAKE 2 LINES WITH A LONG DEALER NAME)
    '-- LINE 3. YEAR AND SERIAL NUMBER
    '-- EXAMPLES:
    '---- FALCON 900
    '---- By Dassault Aviation
    '---- 1998 - S/N: 900-171
    '--
    '---- KING AIR F90
    '---- By International Aviation Marketing, Inc.
    '---- 1981 - S/N: LA-88

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "SELECT top " & acCount & " acpic_id, ac_id, ac_year,amod_make_name, ac_reg_no, amod_model_name,  ac_ser_no_full, comp_name, comp_id, ac_country_of_registration "
      sqlQuery += " FROM View_ABI_Featured_Aircraft WITH(NOLOCK)  "
      sqlQuery += " ORDER BY NEWID()"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetFeaturedAircraft(ByVal acCount As Integer) As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetFeaturedAircraft = atemptable
    Catch ex As Exception
      GetFeaturedAircraft = Nothing
      Me.class_error = "Error in GetFeaturedAircraft(ByVal acCount As Integer) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  ''' <summary>
  ''' Query that runs the scroller stats in header.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function ScrollerStatsQuery() As DataTable
    'Scroller Stats Query
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = " SELECT distinct top 25 amod_make_name, count(*) as currentforsale,"
      sqlQuery += " (select SUM(mtrend_total_aircraft_for_sale) FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id WHERE amod_make_name = a.amod_make_name AND (mtrend_year = 2014) AND (mtrend_month = 1)) AS pastyearforsale, (select SUM(mtrend_total_aircraft_for_sale) "
      sqlQuery += " FROM Aircraft_Model_Trend WITH(NOLOCK) "
      sqlQuery += " INNER JOIN Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id "
      sqlQuery += " WHERE amod_make_name = a.amod_make_name AND (mtrend_year = 2015) AND (mtrend_month = 1)) AS pastmonthforsale "
      sqlQuery += " FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model AS A WITH(NOLOCK) ON ac_amod_id = amod_id "
      sqlQuery += " WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0 "
      sqlQuery += " AND (ac_product_business_flag ='Y' or ac_product_helicopter_flag ='Y')"
      sqlQuery += " and amod_type_code in ('E','J','T','P')"
      sqlQuery += " GROUP BY amod_make_name "
      sqlQuery += " ORDER by count(*) desc"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>ScrollerStatsQuery() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      ScrollerStatsQuery = atemptable
    Catch ex As Exception
      ScrollerStatsQuery = Nothing
      Me.class_error = "Error in ScrollerStatsQuery() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function


  Public Function GetABIACForSaleList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String, ByVal DisplayMakeOnly As Boolean, ByVal amod_make_name As String) As DataTable
    '    -- **************************************************
    '-- EXECS
    'where amod_airframe_type_code='F' and amod_type_code='E'
    '-- JETS
    'where amod_airframe_type_code='F' and amod_type_code='J'
    '-- TURBO PROPS
    'where amod_airframe_type_code='F' and amod_type_code='T'
    '-- PISTONS
    'where amod_airframe_type_code='F' and amod_type_code='P'
    '-- HELICOPTERS
    'where amod_airframe_type_code='R'

    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT DISTINCT "

      If DisplayMakeOnly = False Then
        sqlQuery += " amod_id, amod_model_name, "
      End If

      sqlQuery += " amod_airframe_type_code, amod_type_code, amod_make_name,  count(distinct ac_id) as tcount "
      sqlQuery += " from View_ABI_Aircraft_For_Sale "
      'sqlQuery += " FROM ABI_Company_Service WITH(NOLOCK) "
      'sqlQuery += " INNER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON abicserv_comp_id = comp_id and cref_journ_id = 0 "

      sqlQuery += " WHERE "


      If amod_airframe_type_code <> "" Then
        sqlWhere += " amod_airframe_type_code = @amod_airframe_type_code "

        'If amod_airframe_type_code = "R" Then
        '  sqlWhere += " and ac_product_helicopter_flag='Y' "
        'End If
      End If

      If amod_type_code <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_type_code = @amod_type_code "

      End If

      If amod_make_name <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " lower(amod_make_name) = @amod_make_name "

      End If

      If sqlWhere <> "" Then
        sqlQuery += sqlWhere
      End If

      'sqlQuery += " (ac_forsale_flag = 'Y') AND (amod_customer_flag = 'Y') "
      'sqlQuery += " AND (abicserv_serv_code = 'ACLIST' AND (abicserv_status = 'A')) "
      'sqlQuery += " AND ((abicserv_end_date >= GETDATE() - 1) AND (abicserv_start_date <= GETDATE())) "
      'sqlQuery += " AND ( (ac_lifecycle_stage IN (2, 3) AND cref_contact_type IN ('99', '00', '38')) "
      ' sqlQuery += " OR (ac_lifecycle_stage = 1 AND cref_contact_type IN ('99', '38')) ) "
      sqlQuery += " group by amod_airframe_type_code, amod_type_code, amod_make_name "

      If DisplayMakeOnly = False Then
        sqlQuery += ", amod_model_name, amod_id"
      End If

      sqlQuery += " ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name "
      If DisplayMakeOnly = False Then
        sqlQuery += ", amod_model_name"
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIACForSaleList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable</b><br />" & sqlQuery & "<br />Parameters: amod_type_code: " & amod_type_code & "<br />" & "amod_airframe_type_code: " & amod_airframe_type_code


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If amod_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_type_code", amod_type_code)
      End If

      If amod_make_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_make_name", amod_make_name.ToLower)
      End If

      If amod_airframe_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_airframe_type_code", amod_airframe_type_code)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIACForSaleList = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIACForSaleList = Nothing
      Me.class_error = "Error in GetABIACForSaleList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIACForSaleListForRSS(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable
    '    -- **************************************************
    '-- EXECS
    'where amod_airframe_type_code='F' and amod_type_code='E'
    '-- JETS
    'where amod_airframe_type_code='F' and amod_type_code='J'
    '-- TURBO PROPS
    'where amod_airframe_type_code='F' and amod_type_code='T'
    '-- PISTONS
    'where amod_airframe_type_code='F' and amod_type_code='P'
    '-- HELICOPTERS
    'where amod_airframe_type_code='R'

    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT DISTINCT "

      sqlQuery += " amod_id, amod_model_name, amod_manufacturer, amod_description, ac_list_date,  "

      sqlQuery += " amod_airframe_type_code, amod_type_code, amod_make_name,  count(distinct ac_id) as tcount "
      sqlQuery += " from View_ABI_Aircraft_For_Sale "

      sqlQuery += " WHERE ac_list_date >= '" & FormatDateTime(DateAdd(DateInterval.Day, -30, Now()), 2) & "'"


      If amod_airframe_type_code <> "" Then
        sqlWhere += " and amod_airframe_type_code = @amod_airframe_type_code "
      End If

      If amod_type_code <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_type_code = @amod_type_code "

      End If


      If sqlWhere <> "" Then
        sqlQuery += sqlWhere
      End If

      sqlQuery += " group by amod_airframe_type_code, amod_type_code, amod_make_name, amod_manufacturer, ac_list_date, amod_description "
      sqlQuery += ", amod_model_name, amod_id"


      sqlQuery += " ORDER BY ac_list_date desc, amod_airframe_type_code, amod_type_code, amod_make_name, amod_manufacturer, amod_description "
      sqlQuery += ", amod_model_name"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetABIACForSaleListForRSS(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable</b><br />" & sqlQuery & "<br />Parameters: amod_type_code: " & amod_type_code & "<br />" & "amod_airframe_type_code: " & amod_airframe_type_code


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If amod_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_type_code", amod_type_code)
      End If


      If amod_airframe_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_airframe_type_code", amod_airframe_type_code)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIACForSaleListForRSS = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIACForSaleListForRSS = Nothing
      Me.class_error = "Error in GetABIACForSaleListForRSS(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIACForSaleDetailedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String, ByVal DealerID As Long, ByVal ModelID As Long, ByVal amod_make_name As String, ByVal yearStart As String, ByVal yearEnd As String, ByVal amod_model_name As String) As DataTable
    '    -- **************************************************
    '-- EXECS
    'where amod_airframe_type_code='F' and amod_type_code='E'
    '-- JETS
    'where amod_airframe_type_code='F' and amod_type_code='J'
    '-- TURBO PROPS
    'where amod_airframe_type_code='F' and amod_type_code='T'
    '-- PISTONS
    'where amod_airframe_type_code='F' and amod_type_code='P'
    '-- HELICOPTERS
    'where amod_airframe_type_code='R'

    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT DISTINCT  ac_asking, ac_asking_price, ac_airframe_tot_hrs, ac_picture_id, comp_id, ac_id, amod_id, ac_ser_no_full, ac_reg_no, amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_mfr_year, ac_year, comp_name, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, comp_web_address, ac_country_of_registration, ac_days_on_market "
      ' sqlQuery += " FROM ABI_Company_Service WITH(NOLOCK) "
      ' sqlQuery += " INNER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON abicserv_comp_id = comp_id and cref_journ_id = 0 "

      sqlQuery += " FROM View_JETNET_Global_Aircraft_For_Sale WITH(NOLOCK)"
      sqlQuery += " INNER JOIN company WITH(NOLOCK) ON comp_journ_id = 0 and comp_id = cref_comp_id"

      sqlQuery += " WHERE "


      If amod_airframe_type_code <> "" Then
        sqlWhere += " amod_airframe_type_code = @amod_airframe_type_code "

        If amod_airframe_type_code = "R" Then
          sqlWhere += " and ac_product_helicopter_flag='Y' "
        End If
      End If

      If amod_type_code <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_type_code = @amod_type_code "

      End If

      If yearStart <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " ac_year > @yearStart "

      End If

      If yearEnd <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " ac_year < @yearEnd "

      End If

      If DealerID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " comp_id  = @comp_id  "

      End If


      If amod_make_name <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " lower(amod_make_name) = @amod_make_name "

      End If

      If amod_model_name <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " lower(amod_model_name) = @amod_model_name "

      End If


      If ModelID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_id  = @amod_id  "

      End If

      If sqlWhere <> "" Then
        sqlWhere += " and "
        sqlQuery += sqlWhere
      End If


      ' sqlQuery += " (ac_forsale_flag = 'Y')  AND (amod_customer_flag = 'Y') "
      sqlQuery += " ac_status <> 'Lease' "
      '  sqlQuery += " AND ac_id NOT IN (SELECT DISTINCT aadns_ac_id FROM ABI_Aircraft_Do_Not_Show WITH(NOLOCK))"
      '  sqlQuery += " AND (abicserv_serv_code = 'ACLIST' AND (abicserv_status = 'A')) "
      '   sqlQuery += " AND ((abicserv_end_date >= GETDATE() - 1) AND (abicserv_start_date <= GETDATE())) "
      '  sqlQuery += " AND ( (ac_lifecycle_stage IN (2, 3) AND cref_contact_type IN ('99', '00', '38')) "
      ' sqlQuery += " OR (ac_lifecycle_stage = 1 AND cref_contact_type IN ('99', '38')) ) "
      sqlQuery += " group by ac_asking, ac_asking_price, ac_airframe_tot_hrs, comp_id, ac_id, ac_picture_id, amod_id, ac_ser_no_full, ac_reg_no, amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_mfr_year, ac_year, comp_name, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, comp_web_address, ac_country_of_registration, ac_days_on_market "
      sqlQuery += " ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIACForSaleDetailedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable</b><br />" & sqlQuery & "<br />Parameters: amod_type_code: " & amod_type_code & "<br />" & "amod_airframe_type_code: " & amod_airframe_type_code


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If DealerID > 0 Then
        SqlCommand.Parameters.AddWithValue("comp_id", DealerID)
      End If

      If ModelID > 0 Then
        SqlCommand.Parameters.AddWithValue("amod_id", ModelID)
      End If

      If yearStart <> "" Then
        SqlCommand.Parameters.AddWithValue("yearStart", yearStart)
      End If

      If yearEnd <> "" Then
        SqlCommand.Parameters.AddWithValue("yearEnd", yearEnd)
      End If


      If amod_make_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_make_name", amod_make_name.ToLower)
      End If

      If amod_model_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_model_name", amod_model_name.ToLower)
      End If

      If amod_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_type_code", amod_type_code)
      End If

      If amod_airframe_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_airframe_type_code", amod_airframe_type_code)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIACForSaleDetailedList = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIACForSaleDetailedList = Nothing
      Me.class_error = "Error in GetABIACForSaleDetailedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIACDetails(ByVal acID As Long, ByVal DealerID As Long, ByVal ModelID As Long, ByVal amod_make_name As String) As DataTable
    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT DISTINCT ac_picture_id, ac_status, ac_asking, ac_asking_price, '' as ac_interior_month_year, '' as ac_exterior_month_year,"
      'sqlQuery += " ac_confidential_notes, "
      sqlQuery += " ac_list_date, ac_airframe_tot_hrs, ac_lifecycle_stage, ac_delivery_date,"
      sqlQuery += " comp_id, ac_id, amod_id, ac_ser_no_full, ac_reg_no, amod_airframe_type_code, amod_type_code, amod_make_name,"
      sqlQuery += " amod_model_name, ac_mfr_year, ac_year, comp_name, comp_address1, comp_address2, comp_city, comp_state,"
      sqlQuery += " comp_zip_code, comp_country, comp_web_address, ac_country_of_registration, ac_days_on_market"


      'sqlQuery += " FROM ABI_Company_Service WITH(NOLOCK) "
      'sqlQuery += " INNER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON abicserv_comp_id = comp_id and cref_journ_id = 0 "

      sqlQuery += " FROM View_JETNET_Global_Aircraft_For_Sale WITH (NOLOCK)"
      sqlQuery += " INNER JOIN company on comp_journ_id = 0 and comp_id = cref_comp_id"


      sqlQuery += " WHERE "


      If DealerID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " comp_id  = @comp_id  "
      End If

      If acID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " ac_id  = @ac_id  "
      End If

      If amod_make_name <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " lower(amod_make_name) = @amod_make_name "
      End If

      If ModelID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_id  = @amod_id  "
      End If

      If sqlWhere <> "" Then
        '  sqlWhere += " and "
        sqlQuery += sqlWhere
      End If

      '  sqlQuery += " (ac_forsale_flag = 'Y') AND (amod_customer_flag = 'Y') "
      '  sqlQuery += " AND ac_id NOT IN (SELECT DISTINCT aadns_ac_id FROM ABI_Aircraft_Do_Not_Show WITH(NOLOCK))"
      ' sqlQuery += " AND (abicserv_serv_code = 'ACLIST' AND (abicserv_status = 'A')) "
      '  sqlQuery += " AND ((abicserv_end_date >= GETDATE() - 1) AND (abicserv_start_date <= GETDATE())) "
      ' sqlQuery += " AND ( (ac_lifecycle_stage IN (2, 3) AND cref_contact_type IN ('99', '00', '38')) "
      ' sqlQuery += " OR (ac_lifecycle_stage = 1 AND cref_contact_type IN ('99', '38')) ) "
      sqlQuery += " group by comp_id, ac_id, ac_picture_id, amod_id, ac_ser_no_full, ac_reg_no, amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_mfr_year, ac_year, comp_name, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, comp_web_address, ac_status, ac_asking, ac_asking_price"
      ', ac_confidential_notes
      sqlQuery += " , ac_list_date, ac_airframe_tot_hrs, ac_lifecycle_stage, ac_delivery_date, ac_country_of_registration, ac_days_on_market "
      sqlQuery += " ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIACDetails(ByVal AmodID As Long, ByVal DealerID As Long, ByVal ModelID As Long, ByVal amod_make_name As String) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If DealerID > 0 Then
        SqlCommand.Parameters.AddWithValue("comp_id", DealerID)
      End If

      If acID > 0 Then
        SqlCommand.Parameters.AddWithValue("ac_id", acID)
      End If

      If ModelID > 0 Then
        SqlCommand.Parameters.AddWithValue("amod_id", ModelID)
      End If

      If amod_make_name <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_make_name", amod_make_name.ToLower)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIACDetails = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIACDetails = Nothing
      Me.class_error = "Error in GetABIACDetails(ByVal AmodID As Long, ByVal DealerID As Long, ByVal ModelID As Long, ByVal amod_make_name As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIACEngine(ByVal acID As Long, ByVal journalID As Long) As DataTable
    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT ac_id, ac_engine_name, amod_airframe_type_code as amod_airframe_type, amod_type_code as amod_make_type, ac_engine_maintenance_prog_EMP, ac_engine_management_prog_EMGP, ac_engine_tbo_oc_flag, "
      sqlQuery += " ac_engine_noise_rating, ac_model_config, ac_maint_eoh_by_name, ac_main_eoh_moyear, ac_maint_hots_by_name, "
      sqlQuery += " ac_maint_hots_moyear, ac_engine_1_ser_no, ac_engine_2_ser_no, ac_engine_3_ser_no, amod_number_of_engines, "
      sqlQuery += " ac_engine_4_ser_no, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, "
      sqlQuery += "  ac_engine_4_tot_hrs, ac_engine_1_soh_hrs, ac_engine_2_soh_hrs, ac_engine_3_soh_hrs, "
      sqlQuery += " ac_engine_4_soh_hrs, ac_engine_1_shi_hrs, ac_engine_2_shi_hrs, ac_engine_3_shi_hrs, "
      sqlQuery += " ac_engine_4_shi_hrs, ac_engine_1_tbo_hrs, ac_engine_2_tbo_hrs, ac_engine_3_tbo_hrs, "
      sqlQuery += " ac_engine_4_tbo_hrs, ac_engine_1_snew_cycles, ac_engine_2_snew_cycles, ac_engine_3_snew_cycles, "
      sqlQuery += "  ac_engine_4_snew_cycles, ac_engine_1_soh_cycles, ac_engine_2_soh_cycles, ac_engine_3_soh_cycles, "
      sqlQuery += " ac_engine_4_soh_cycles,ac_engine_1_shs_cycles,ac_engine_2_shs_cycles,ac_engine_3_shs_cycles, "
      sqlQuery += "  ac_engine_4_shs_cycles, emp_provider_name, "
      sqlQuery += "  emp_program_name, emgp_provider_name, "
      sqlQuery += "  emgp_program_name"
      sqlQuery += " FROM Aircraft with (NOLOCK) INNER JOIN "
      sqlQuery += " aircraft_model WITH(NOLOCK) ON aircraft.ac_amod_id = aircraft_model.amod_id INNER JOIN "
      sqlQuery += "  Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id INNER JOIN"
      sqlQuery += "  Engine_Management_Program WITH(NOLOCK) ON ac_engine_management_prog_EMGP = Engine_Management_Program.emgp_id "


      sqlQuery += " WHERE "



      sqlQuery += " ac_id  = @ac_id  "

      sqlQuery += " and "
      sqlQuery += " ac_journ_id = @ac_journ_id "

      sqlQuery += " ORDER BY ac_id"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIACEngine(ByVal acID As Long, ByVal journalID As Long) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)



      SqlCommand.Parameters.AddWithValue("ac_id", acID)
      SqlCommand.Parameters.AddWithValue("ac_journ_id", journalID)


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIACEngine = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIACEngine = Nothing
      Me.class_error = "Error in GetABIACEngine(ByVal acID As Long, ByVal journalID As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIPictures(ByVal acID As Long, ByVal jID As Long) As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "SELECT * FROM Aircraft_Pictures WITH(NOLOCK) "
      sqlQuery += " WHERE "

      sqlQuery += "  acpic_seq_no > '0'"
      sqlQuery += " AND acpic_image_type = 'JPG'"
      sqlQuery += " AND acpic_hide_flag = 'N'"

      'Aircraft ID
      sqlQuery += " and "
      sqlQuery += " acpic_ac_id  = @acpic_ac_id  "

      'Journal ID
      sqlQuery += " and "
      sqlQuery += " acpic_journ_id  = @acpic_journ_id  "


      sqlQuery += " ORDER BY acpic_seq_no"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIPictures(ByVal acID As Long, ByVal jID As Long) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)


      SqlCommand.Parameters.AddWithValue("acpic_ac_id", acID)
      SqlCommand.Parameters.AddWithValue("acpic_journ_id", jID)


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIPictures = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIPictures = Nothing
      Me.class_error = "Error in GetABIPictures(ByVal acID As Long, ByVal jID As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function
#End Region
#Region "Dealers"
  ''' <summary>
  ''' Standard display of info for dealers
  ''' </summary>
  ''' <param name="CompanyAddress1"></param>
  ''' <param name="CompanyAddress2"></param>
  ''' <param name="CompanyCity"></param>
  ''' <param name="CompanyState"></param>
  ''' <param name="CompanyZip"></param>
  ''' <param name="CompanyCountry"></param>
  ''' <param name="CompanyWeb"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function DisplayCompanyInformation(ByVal companyID As Long, ByVal CompanyAddress1 As Object, ByVal CompanyAddress2 As Object, ByVal CompanyCity As Object, ByVal CompanyState As Object, ByVal CompanyZip As Object, ByVal CompanyCountry As Object, ByVal CompanyWeb As Object)
    Dim DisplayString As String = ""
    Dim Seperator As String = ""
    Dim PhoneTable As New DataTable

    If Not IsDBNull(CompanyAddress1) Then
      DisplayString = CompanyAddress1
      Seperator = "<br />"
    End If

    If Not IsDBNull(CompanyAddress2) Then
      DisplayString += " " & CompanyAddress2
      Seperator = "<br />"
    End If

    DisplayString += Seperator
    Seperator = ""

    If Not IsDBNull(CompanyCity) Then
      DisplayString += CompanyCity & ", "
      Seperator = "<br />"
    End If

    If Not IsDBNull(CompanyState) Then
      DisplayString += CompanyState & " "
      Seperator = "<br />"
    End If

    If Not IsDBNull(CompanyZip) Then
      DisplayString += CompanyZip & " "
      Seperator = "<br />"
    End If


    If Not IsDBNull(CompanyCountry) Then
      DisplayString += CompanyCountry
      Seperator = "<br />"
    End If

    DisplayString += Seperator
    Seperator = ""

    If Not IsDBNull(CompanyWeb) Then
      DisplayString += "<a href="""
      If InStr("http://", CompanyWeb) = 0 Then
        DisplayString += "http://" & CompanyWeb
      Else
        DisplayString += CompanyWeb
      End If
      DisplayString += """ target=""blank"">" & CompanyWeb & "</a>"

    End If

    DisplayString += "<br />"
    PhoneTable = GetABIDealersPhoneNumber(companyID, 0, 0)
    If Not IsNothing(PhoneTable) Then
      If PhoneTable.Rows.Count > 0 Then

        For Each r As DataRow In PhoneTable.Rows
          If Not IsDBNull(r("pnum_type")) Then
            Select Case LCase(r("pnum_type"))
              Case "office"
                DisplayString += "<br /><strong>" & r("pnum_type") & "</strong>: " & r("pnum_number_full").ToString
              Case "fax"
                DisplayString += "<br /><strong>" & r("pnum_type") & "</strong>: " & r("pnum_number_full").ToString
            End Select
          End If
        Next
      End If
    End If

    If InStr(DisplayString.ToLower, "office") > 0 Or InStr(DisplayString.ToLower, "fax") > 0 Then
      DisplayString = Replace(DisplayString, "<br /><br />", "<div class=""seperatorHR"">&nbsp;</div>")
    End If
    Return DisplayString

  End Function


  Public Function GetCompanyEmailAddress(ByVal companyID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try


      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "SELECT comp_email_address, abicserv_alternate_email FROM View_JETNET_Global_Dealers WITH(NOLOCK) WHERE comp_id = @companyID"

      'save to session query debug string.
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetCompanyEmailAddress(ByVal companyID As Long) As DataTable</b><br />" & sql

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      SqlCommand.Parameters.AddWithValue("companyID", companyID)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing


      Return atemptable
    Catch ex As Exception
      GetCompanyEmailAddress = Nothing
      Me.class_error = "Error in GetCompanyEmailAddress(ByVal companyID As Long) As DataTable: SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing



    End Try

  End Function

  Public Shared Function GetABIDealersPhoneNumber(ByVal companyID As Long, ByVal journalID As Long, ByVal contactID As Long) As DataTable

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase")
      SqlConn.Open()

      sqlQuery = "SELECT * FROM Phone_Numbers WITH(NOLOCK), Phone_Type WITH(NOLOCK) WHERE pnum_comp_id=@companyID"
      sqlQuery = sqlQuery & " and pnum_journ_id= @journalID"
      sqlQuery = sqlQuery & " and pnum_contact_id = @contactID"
      sqlQuery = sqlQuery & " AND pnum_hide_customer <> 'Y'"
      sqlQuery = sqlQuery & " AND pnum_type = ptype_name"
      sqlQuery = sqlQuery & " ORDER BY ptype_seq_no"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIDealersPhoneNumber(ByVal companyID As Long, ByVal journalID As Long, ByVal contactID As Long) As DataTable</b><br />" & sqlQuery

      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      SqlCommand.Parameters.AddWithValue("companyID", companyID)
      SqlCommand.Parameters.AddWithValue("journalID", journalID)
      SqlCommand.Parameters.AddWithValue("contactID", contactID)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIDealersPhoneNumber = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetABIDealersPhoneNumber = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetABIDealerInformation(ByVal compID As Long) As DataTable
    '-- ***********************
    '-- Grabbing ABI Dealer information
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "select "

      sqlQuery += " * "

      sqlQuery += " from View_JETNET_Global_Dealers "

      If compID > 0 Then
        sqlQuery += " where comp_id = @compID "
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Public Function GetABIDealerInformation(ByVal compID As Long) As DataTable</b><br />" & sqlQuery

      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If compID > 0 Then
        SqlCommand.Parameters.AddWithValue("compID", compID)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIDealerInformation = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetABIDealerInformation = Nothing
      Me.class_error = "Error in Public Function GetABIDealerInformation(ByVal compID As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  ''' <summary>
  ''' Gets list of dealers based on country. Return limited fields only returns enough fields for dropdown list
  ''' </summary>
  ''' <param name="returnLimitedFields"></param>
  ''' <param name="country"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIDealers(ByVal returnLimitedFields As Boolean, ByVal country As String) As DataTable
    '-- ***********************
    '-- LIST OF ABI DEALERS


    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = "select "

      If returnLimitedFields = False Then
        sqlQuery += " * "
      Else
        sqlQuery += " comp_name , comp_id "
      End If

      sqlQuery += " from View_JETNET_Global_Dealers "

      If country <> "" Then
        sqlQuery += " where comp_country = @countryName "
      End If

      sqlQuery += " order by comp_name asc "

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIDealers() As DataTable</b><br />" & sqlQuery

      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If country <> "" Then
        SqlCommand.Parameters.AddWithValue("countryName", country)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIDealers = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing
    Catch ex As Exception
      GetABIDealers = Nothing
      Me.class_error = "Error in GetABIDealers() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Function GetCompanyInformation(ByVal companyID As Long, ByVal journalID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try


      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "select comp_action_date, comp_customer_notes as clicomp_description, comp_address1, comp_address2, comp_agency_type, comp_name_alt as comp_alternate_name, "
      sql = sql & " comp_name_alt_type as comp_alternate_name_type, comp_product_helicopter_flag, comp_product_business_flag, "
      sql = sql & " comp_product_commercial_flag, '' as clicomp_category2, '' as clicomp_category3, '' as clicomp_category4, '' as clicomp_category5, "
      sql = sql & " comp_city, comp_country, comp_email_address, comp_id, comp_id as jetnet_comp_id, comp_name, comp_state, "
      sql = sql & " comp_active_flag as comp_status, 0 as comp_user_id, comp_logo_flag, comp_web_address, comp_zip_code, 'JETNET' as source from company WITH (NOLOCK) where comp_journ_id = @journalID"
      sql = sql & " and comp_id = @companyID"

      'save to session query debug string.
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetCompanyInformation(ByVal companyID As Long, ByVal journalID As Long) As DataTable</b><br />" & sql

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      SqlCommand.Parameters.AddWithValue("companyID", companyID)
      SqlCommand.Parameters.AddWithValue("journalID", journalID)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing


      Return atemptable
    Catch ex As Exception
      GetCompanyInformation = Nothing
      Me.class_error = "Error in GetCompanyInformation(ByVal companyID As Long, ByVal journalID As Long) As DataTable: SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing



    End Try

  End Function
  ''' <summary>
  ''' Returns dealers countries on left hand side of page.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIDealersCountry() As DataTable
    '-- ***********************
    '-- LIST OF ABI DEALERS Country


    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "select distinct comp_country "

      sqlQuery += " from View_JETNET_Global_Dealers order by comp_country"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIDealersCountry() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIDealersCountry = atemptable
    Catch ex As Exception
      GetABIDealersCountry = Nothing
      Me.class_error = "Error in GetABIDealersCountry() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
#End Region
#Region "Backgrounds"
  ''' <summary>
  ''' Function to get random Evo backgrounds (parameter is how many to return)
  ''' </summary>
  ''' <param name="pictureCount"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetRandomEvolutionBackgrounds(ByVal pictureCount As Integer) As DataTable
    '-- FEATURED ARTICLE IMAGE 
    '-- GET 2 RANDOM IMAGES TO GO BEHIND THE FEATURED NEWS ARTICLE ABOVE

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "SELECT top " & pictureCount & " * from Evolution_Backgrounds WITH (NOLOCK) where evoback_active_flag = 'Y' "
      sqlQuery += " and ( evoback_product_helicopter_flag = 'Y' or evoback_product_business_flag = 'Y') "
      sqlQuery += " order by newid()"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetRandomEvolutionBackgrounds(ByVal pictureCount As Integer) As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetRandomEvolutionBackgrounds = atemptable
    Catch ex As Exception
      GetRandomEvolutionBackgrounds = Nothing
      Me.class_error = "Error in GetRandomEvolutionBackgrounds(ByVal pictureCount As Integer) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
#End Region
#Region "Events"
  ''' <summary>
  ''' Dataquery to return events for ABI
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIEventList(ByVal eventCount As Integer) As DataTable
    '-- Returns Events for homepage, latest 3

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = " select "
      If eventCount > 0 Then
        sqlQuery += " top " & eventCount.ToString
      End If
      sqlQuery += " * from ABI_Event"
      sqlQuery += " where(abievent_start_date >= getdate())"
      sqlQuery += " order by abievent_start_date asc"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIEventList() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIEventList = atemptable
    Catch ex As Exception
      GetABIEventList = Nothing
      Me.class_error = "Error in GetABIEventList() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
#End Region
#Region "Products"
  ''' <summary>
  ''' Get list of ABI products based on subgroup
  ''' </summary>
  ''' <param name="subGroup"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIProducts(ByVal subGroup As String) As DataTable
    '-- ****************************************************
    '-- ABI PRODUCTS PAGE QUERY
    'SELECT * from View_ABI_Products
    'order by abiserv_subgroup, abiserv_name


    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()


      sqlQuery = "SELECT * from View_ABI_Products "

      If subGroup <> "" Then
        sqlQuery += " where abiserv_subgroup = @subGroup "
      End If

      sqlQuery += " order by abiserv_subgroup, abiserv_name"



      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIProducts(ByVal subGroup As Integer) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If subGroup <> "" Then
        SqlCommand.Parameters.AddWithValue("subGroup", subGroup)
      End If


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIProducts = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIProducts = Nothing
      Me.class_error = "Error in GetABIProducts(ByVal subGroup As Integer) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function
  ''' <summary>
  ''' Get list of ABI products category list.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIProductsCategoriesList() As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "SELECT distinct abiserv_subgroup from View_ABI_Products "
      sqlQuery += " order by abiserv_subgroup"



      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIProductsCategoriesList() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      SqlCommand.CommandText = sqlQuery
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIProductsCategoriesList = atemptable

    Catch ex As Exception
      GetABIProductsCategoriesList = Nothing
      Me.class_error = "Error in GetABIProductsCategoriesList() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function
#End Region
#Region "Links"
  ''' <summary>
  ''' Get list of ABI links.
  ''' </summary>
  ''' <param name="topicName"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABILinkList(ByVal topicName As String) As DataTable
    '    -- **************************************************
    '-- AVIATION LINKS PAGE
    '-- RETURNS cbus_name, comp_name, comp_web_address
    '-- NEW HEADING BEFORE EACH cbus_name
    '-- DISPLAY COMPANY NAME WITH LINK TO COMPANY WEB ADDRESS

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = " select cbus_name, comp_name, comp_web_address from View_ABI_Aviation_Links"

      If topicName <> "" Then
        sqlQuery += " where cbus_name = @topicName "
      End If

      sqlQuery += " order by cbus_name, comp_name"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABILinkList(ByVal topicName As String) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If topicName <> "" Then
        SqlCommand.Parameters.AddWithValue("topicName", topicName)
      End If


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABILinkList = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABILinkList = Nothing
      Me.class_error = "Error in GetABILinkList(ByVal topicName As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function
  ''' <summary>
  ''' Get list of ABI topic categories.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABILinkTopicList() As DataTable
    '    -- **************************************************
    '-- AVIATION LINKS PAGE
    '-- RETURNS cbus_name

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = " select distinct cbus_name from View_ABI_Aviation_Links"
      sqlQuery += " order by cbus_name"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABILinkTopicList() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      SqlCommand.CommandText = sqlQuery
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABILinkTopicList = atemptable

    Catch ex As Exception
      GetABILinkTopicList = Nothing
      Me.class_error = "Error in GetABILinkTopicList() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function
#End Region
#Region "Wanteds"
  ''' <summary>
  ''' Get list of wanted based on type code/airframe type code.
  ''' </summary>
  ''' <param name="amod_airframe_type_code"></param>
  ''' <param name="amod_type_code"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIWantedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String, ByVal dealerID As Long) As DataTable
    '    -- **************************************************
    '-- AVIATION WANTED PAGE
    '  select * from View_ABI_Wanteds
    '-- EXECS
    'where amod_airframe_type_code='F' and amod_type_code='E'
    '-- JETS
    'where amod_airframe_type_code='F' and amod_type_code='J'
    '-- TURBO PROPS
    'where amod_airframe_type_code='F' and amod_type_code='T'
    '-- PISTONS
    'where amod_airframe_type_code='F' and amod_type_code='P'
    '-- HELICOPTERS
    'where amod_airframe_type_code='R'

    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = " select * from View_ABI_Wanteds "

      If amod_airframe_type_code <> "" Then
        sqlWhere += " amod_airframe_type_code = @amod_airframe_type_code "
      End If

      If amod_type_code <> "" Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " amod_type_code = @amod_type_code "
      End If

      If dealerID > 0 Then
        If sqlWhere <> "" Then
          sqlWhere += " and "
        End If
        sqlWhere += " comp_id = @comp_id "
      End If

      If sqlWhere <> "" Then
        sqlQuery += " where " & sqlWhere
      End If

      sqlQuery += " order by amod_make_name, amod_model_name "
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIWantedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable</b><br />" & sqlQuery & "<br />Parameters: amod_type_code: " & amod_type_code & "<br />" & "amod_airframe_type_code: " & amod_airframe_type_code


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If amod_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_type_code", amod_type_code)
      End If

      If dealerID > 0 Then
        SqlCommand.Parameters.AddWithValue("comp_id", dealerID)
      End If

      If amod_airframe_type_code <> "" Then
        SqlCommand.Parameters.AddWithValue("amod_airframe_type_code", amod_airframe_type_code)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIWantedList = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIWantedList = Nothing
      Me.class_error = "Error in GetABIWantedList(ByVal amod_airframe_type_code As String, ByVal amod_type_code As String) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function
  ''' <summary>
  ''' Get wanted details.
  ''' </summary>
  ''' <param name="amwant_id"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetABIWantedDetails(ByVal amwant_id As Long) As DataTable
    '    -- **************************************************
    '-- AVIATION WANTED PAGE
    Dim sqlQuery As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try
      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sqlQuery = " select * from View_ABI_Wanteds "

      If amwant_id > 0 Then
        sqlQuery += " where amwant_id = @amwant_id "
      End If


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetABIWantedDetails(ByVal amwant_id As Long) As DataTable</b><br />" & sqlQuery


      Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

      If amwant_id > 0 Then
        SqlCommand.Parameters.AddWithValue("amwant_id", amwant_id)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetABIWantedDetails = atemptable

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      GetABIWantedDetails = Nothing
      Me.class_error = "Error in GetABIWantedDetails(ByVal amwant_id As Long) As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Shared Sub DisplayWanted(ByRef WantedTable As DataTable, ByRef headerTitle As String, ByRef wantedType As String, ByRef wantedListLiteral As Literal, ByVal CompanyPage As Boolean)
    Dim DisplayString As String = ""
    Dim css As String = "gray"
    Dim WantedLink As String = ""
    If Not IsNothing(WantedTable) Then
      If WantedTable.Rows.Count > 0 Then

        If wantedType = "" Then 'if there is no type selected, display the heading
          DisplayString += "<span class=""span8 LeftBuffer""><h4>" & headerTitle & "</h4></span><div class='clearfix'></div>"
        Else
          DisplayString += ""
        End If

        DisplayString += "<div class=""removePaddingLeft RowSeperator size11"">"
        DisplayString += "<span class=""span3""><strong><u>Make/Model</u></strong></span>"
        DisplayString += "<span class=""span2""><strong><u>Date Verified</u></strong></span>"

        If CompanyPage = False Then
          DisplayString += "<span class=""span4""><strong><u>Interested Party</u></strong></span>"
          DisplayString += "<span class=""span2""><strong><u>Max Price</strong></u></span>"
        Else
          DisplayString += "<span class=""span6""><strong><u>Notes</u></strong></span>"
        End If
        DisplayString += "</div><div class='clearfix'></div>"

        For Each r As DataRow In WantedTable.Rows
          DisplayString += "<div class=""" & css & " LeftBuffer RowSeperator size11""><div class='clearfix'></div>"
          WantedLink = "?id=" & r("amwant_id")
          'DisplayString += "<div class='clearfix'></div>"

          'Make/Model Column
          DisplayString += "<span class=""removePaddingLeft  span3 " & css & """>"

          DisplayString += "<a href=""" & WantedLink & """>"
          'Make
          If Not IsDBNull(r("amod_make_name")) Then
            DisplayString += r("amod_make_name")
            DisplayString += " "
          End If

          'Model
          If Not IsDBNull(r("amod_model_name")) Then
            DisplayString += r("amod_model_name")
          End If

          DisplayString += "</a>"
          DisplayString += "</span>"

          'Date Verified Column
          DisplayString += "<span class=""span2 " & css & """>"

          If Not IsDBNull(r("amwant_listed_date")) Then
            DisplayString += Format(r("amwant_listed_date"), "MM/dd/yyyy")
          Else
            DisplayString += "&nbsp;"
          End If

          DisplayString += "</span>"

          If CompanyPage = False Then
            'Interested Party Column
            DisplayString += "<span class=""span4 " & css & """>"

            If Not IsDBNull(r("comp_name")) Then
              DisplayString += r("comp_name")
            Else
              DisplayString += "&nbsp;"
            End If

            DisplayString += "</span>"

            'Max Price Column
            DisplayString += "<span class=""span2 " & css & """>"

            If Not IsDBNull(r("amwant_max_price")) Then
              DisplayString += FormatCurrency(r("amwant_max_price"), 0).ToString & " US"
            Else
              DisplayString += "&nbsp;"
            End If

            DisplayString += "</span>"

          Else
            'Notes Column
            DisplayString += "<span class=""span6 " & css & """>"

            If Not IsDBNull(r("amwant_notes")) Then
              DisplayString += r("amwant_notes")
            Else
              DisplayString += "&nbsp;"
            End If

            DisplayString += "</span>"
          End If


          If css = "gray" Then
            css = ""
          Else
            css = "gray"
          End If

          DisplayString += "<div class='clearfix'></div>"
          DisplayString += "</div>"
        Next
      End If
    End If
    DisplayString += "<div class='clearfix'></div><br />"

    wantedListLiteral.Text += DisplayString
  End Sub
#End Region


#Region "Ads"
  Public Function GetAds() As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "select abicserv_id,abicserv_web_address from View_ABI_Tile_Ads ORDER BY NEWID()"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetAds() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetAds = atemptable

    Catch ex As Exception
      GetAds = Nothing
      Me.class_error = "Error in GetAds() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function
#End Region
#Region "Email"
  Public Function Create_Email_Record(ByVal abiemail_category As String, ByVal abiemail_comp_name As String, ByVal abiemail_first_name As String, ByVal abiemail_last_name As String, ByVal abiemail_address1 As String, ByVal abiemail_address2 As String, ByVal abiemail_city As String, ByVal abiemail_state As String, ByVal abiemail_zip_code As String, ByVal abiemail_phone As String, ByVal abiemail_email_address As String, ByVal abiemail_notes As String, ByVal abiemail_comp_id As Long, ByVal abiemail_ac_id As Long, ByVal abiemail_want_id As Long, ByVal abiemail_to As String, ByVal abiemail_from As String, ByVal abiemail_subject As String, ByVal abiemail_body As String) As Boolean
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim ResponseCode As Boolean = False
    Try

      QueryFields = "insert into ABI_Email(abiemail_category, "
      QueryValues = " values (@abiemail_category,"

      QueryFields += "abiemail_comp_name, "
      QueryValues += "@abiemail_comp_name, "

      QueryFields += "abiemail_first_name, "
      QueryValues += "@abiemail_first_name, "

      QueryFields += "abiemail_last_name, "
      QueryValues += "@abiemail_last_name, "

      QueryFields += "abiemail_address1, "
      QueryValues += "@abiemail_address1, "

      QueryFields += "abiemail_address2, "
      QueryValues += "@abiemail_address2, "

      QueryFields += "abiemail_city, "
      QueryValues += "@abiemail_city, "

      QueryFields += "abiemail_state, "
      QueryValues += "@abiemail_state, "

      QueryFields += "abiemail_zip_code, "
      QueryValues += "@abiemail_zip_code, "

      QueryFields += "abiemail_phone, "
      QueryValues += "@abiemail_phone, "

      QueryFields += "abiemail_email_address, "
      QueryValues += "@abiemail_email_address, "

      QueryFields += "abiemail_notes, "
      QueryValues += "@abiemail_notes, "

      QueryFields += "abiemail_date, "
      QueryValues += "@abiemail_date, "

      QueryFields += "abiemail_comp_id, "
      QueryValues += "@abiemail_comp_id, "

      QueryFields += "abiemail_ac_id, "
      QueryValues += "@abiemail_ac_id, "

      QueryFields += "abiemail_type, "
      QueryValues += "@abiemail_type, "

      QueryFields += "abiemail_want_id, "
      QueryValues += "@abiemail_want_id, "

      QueryFields += "abiemail_to, "
      QueryValues += "@abiemail_to, "

      QueryFields += "abiemail_from, "
      QueryValues += "@abiemail_from, "

      QueryFields += "abiemail_subject, "
      QueryValues += "@abiemail_subject, "

      QueryFields += "abiemail_body, "
      QueryValues += "@abiemail_body, "

      QueryFields += "abiemail_onhold_flag, "
      QueryValues += "@abiemail_onhold_flag, "

      QueryFields += "abiemail_html_flag, "
      QueryValues += "@abiemail_html_flag, "

      QueryFields += "abiemail_job_key) "
      QueryValues += "@abiemail_job_key) "

      Query = QueryFields & QueryValues

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()


      Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)
      SqlCommand.Parameters.AddWithValue("@abiemail_category", Left(abiemail_category, 40))
      SqlCommand.Parameters.AddWithValue("@abiemail_comp_name", Left(abiemail_comp_name, 100))
      SqlCommand.Parameters.AddWithValue("@abiemail_first_name", Left(abiemail_first_name, 50))
      SqlCommand.Parameters.AddWithValue("@abiemail_last_name", Left(abiemail_last_name, 50))
      SqlCommand.Parameters.AddWithValue("@abiemail_address1", Left(abiemail_address1, 35))
      SqlCommand.Parameters.AddWithValue("@abiemail_address2", Left(abiemail_address2, 35))
      SqlCommand.Parameters.AddWithValue("@abiemail_city", Left(abiemail_city, 40))
      SqlCommand.Parameters.AddWithValue("@abiemail_state", Left(abiemail_state, 4))
      SqlCommand.Parameters.AddWithValue("@abiemail_zip_code", Left(abiemail_zip_code, 50))
      SqlCommand.Parameters.AddWithValue("@abiemail_phone", Left(abiemail_phone, 20))
      SqlCommand.Parameters.AddWithValue("@abiemail_email_address", Left(abiemail_email_address, 150))
      SqlCommand.Parameters.AddWithValue("@abiemail_notes", abiemail_notes)
      SqlCommand.Parameters.AddWithValue("@abiemail_date", FormatDateTime(Now(), vbGeneralDate))
      SqlCommand.Parameters.AddWithValue("@abiemail_comp_id", abiemail_comp_id)
      SqlCommand.Parameters.AddWithValue("@abiemail_ac_id", abiemail_ac_id)
      SqlCommand.Parameters.AddWithValue("@abiemail_type", "J")
      SqlCommand.Parameters.AddWithValue("@abiemail_want_id", abiemail_want_id)
      SqlCommand.Parameters.AddWithValue("@abiemail_to", Left(abiemail_to, 300))
      SqlCommand.Parameters.AddWithValue("@abiemail_from", Left(abiemail_from, 150))
      SqlCommand.Parameters.AddWithValue("@abiemail_subject", Left(abiemail_subject, 200))
      SqlCommand.Parameters.AddWithValue("@abiemail_body", abiemail_body)

      SqlCommand.Parameters.AddWithValue("@abiemail_onhold_flag", "N")
      SqlCommand.Parameters.AddWithValue("@abiemail_html_flag", "Y")
      SqlCommand.Parameters.AddWithValue("@abiemail_job_key", 0)

      SqlCommand.ExecuteNonQuery()

      ResponseCode = True

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Me.class_error = Me.class_error & "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"
      Return Nothing
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
    Return ResponseCode
  End Function
  Public Function getCompanyEmail(ByVal companyID As Long)
    Dim emailTable As New DataTable
    emailTable = GetCompanyEmailAddress(companyID)

    Dim EmailAddress As String = "jetnetglobal@jetnet.com"

    If Not IsNothing(emailTable) Then
      If emailTable.Rows.Count > 0 Then
        If Not IsDBNull(emailTable.Rows(0).Item("abicserv_alternate_email")) Then
          If Trim(emailTable.Rows(0).Item("abicserv_alternate_email")) <> "" Then
            EmailAddress = Trim(emailTable.Rows(0).Item("abicserv_alternate_email"))
          ElseIf Not IsDBNull(emailTable.Rows(0).Item("comp_email_address")) Then
            If Trim(emailTable.Rows(0).Item("comp_email_address")) <> "" Then
              EmailAddress = Trim(emailTable.Rows(0).Item("comp_email_address"))
            End If
          End If
        ElseIf Not IsDBNull(emailTable.Rows(0).Item("comp_email_address")) Then
          If Trim(emailTable.Rows(0).Item("comp_email_address")) <> "" Then
            EmailAddress = Trim(emailTable.Rows(0).Item("comp_email_address"))
          End If
        End If

      End If
    End If

    Return EmailAddress
  End Function
#End Region
#Region "RSS Feed"
  Public Shared Sub BuildAircraftFeed(ByVal titleVar As String, ByVal descVar As String, ByVal abiDataQueries As abi_functions, ByVal amod_airframe_type_code As String, ByVal amod_type_code As String)
    Dim AircraftTable As New DataTable

    AircraftTable = abiDataQueries.GetABIACForSaleListForRSS(amod_airframe_type_code, amod_type_code)

    HttpContext.Current.Response.Buffer = True
    HttpContext.Current.Response.CacheControl = "no-cache"
    HttpContext.Current.Response.ContentType = "text/xml"

    HttpContext.Current.Response.Write(vbCrLf & "<rss version=" & Chr(34) & "2.0" & Chr(34) & ">")
    HttpContext.Current.Response.Write(vbCrLf & "  <channel>")
    HttpContext.Current.Response.Write(vbCrLf & "    <title>" & titleVar & "</title>")
    HttpContext.Current.Response.Write(vbCrLf & "    <link>http://www.jetnetglobal.com/</link>")
    HttpContext.Current.Response.Write(vbCrLf & "    <description>" & descVar & "</description>")
    HttpContext.Current.Response.Write(vbCrLf & "    <language>en-us</language>")
    HttpContext.Current.Response.Write(vbCrLf & "    <copyright>" & Year(Now()) & " JETNET Global - Jetnet (All Rights Reserved)</copyright>")
    HttpContext.Current.Response.Write(vbCrLf & "    <lastBuildDate>" & Now() & "</lastBuildDate>")


    If Not IsNothing(AircraftTable) Then
      For Each q As DataRow In AircraftTable.Rows

        'Build the title
        Dim theTitle As String = ""

        theTitle = Trim(q("amod_make_name")) & " "
        theTitle += Trim(q("amod_model_name")) & " "
        theTitle += " (" & q("tcount") & ") For Sale"

        'Build the description:
        Dim theDescription As String = ""

        theDescription = Trim("<img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session("ModelPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/model/") & q("amod_id") & ".jpg' border='0' width='75' /><br>")

        If Not IsDBNull(q("amod_manufacturer")) Then
          If Not String.IsNullOrEmpty(Trim(q("amod_manufacturer"))) Then
            theDescription += "<b>Manufacturer:</b> " & Trim(q("amod_manufacturer")) & "<br />"
          End If
        End If

        If Not IsDBNull(q("amod_description")) Then
          If Not String.IsNullOrEmpty(Trim(q("amod_description"))) Then
            theDescription += Left(Trim(q("amod_description")), 255) & "..."
          End If
        End If

        'Build the date
        Dim CurrHour As String = ""
        Dim CurrMin As String = ""
        Dim CurrSec As String = ""
        Dim CurrDateT As String = ""

        CurrHour = Hour(q("ac_list_date"))
        If CurrHour < 10 Then CurrHour = "0" & CurrHour

        CurrMin = Minute(q("ac_list_date"))
        If CurrMin < 10 Then CurrMin = "0" & CurrMin

        CurrSec = Second(q("ac_list_date"))
        If CurrSec < 10 Then CurrSec = "0" & CurrSec

        CurrDateT = WeekdayName(Weekday(q("ac_list_date")), True) & ", " & Day(q("ac_list_date")) & " " & _
        MonthName(Month(q("ac_list_date")), True) & " " & Year(q("ac_list_date")) & " " & _
        CurrHour & ":" & CurrMin & ":" & CurrSec & " GMT"

        HttpContext.Current.Response.Write(vbCrLf & "    <item>")
        HttpContext.Current.Response.Write(vbCrLf & "      <title>" & DisplayFunctions.ApplyXMLFormatting(theTitle) & "</title>")
        HttpContext.Current.Response.Write(vbCrLf & "      <link>http://www.jetnetglobal.com" & abi_functions.AircraftModelForSaleURL(q("amod_id"), Trim(q("amod_make_name")), Trim(q("amod_model_name"))) & "</link>")
        HttpContext.Current.Response.Write(vbCrLf & "      <description><![CDATA[" & theDescription & "]]></description>")
        HttpContext.Current.Response.Write(vbCrLf & "      <pubDate>" & CurrDateT & "</pubDate>")
        HttpContext.Current.Response.Write(vbCrLf & "    </item>")

      Next
    End If


    HttpContext.Current.Response.Write(vbCrLf & "     </channel>")
    HttpContext.Current.Response.Write(vbCrLf & "  </rss>")
  End Sub
#End Region
#Region "Simple Lookups"
  Public Function GetCountry() As DataTable
    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "select country_name from country with (NOLOCK) where country_active_flag = 'Y' order by country_name asc"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetCountry() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetCountry = atemptable

    Catch ex As Exception
      GetCountry = Nothing
      Me.class_error = "Error in GetCountry() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function
#End Region

#Region "Redirect"
  Public Shared Sub ABI_Redirect()
    Dim redirectSite As String = "/abiFiles/"
    Dim callingFile As String = ""

    callingFile = Replace(HttpContext.Current.Request.ServerVariables("SCRIPT_NAME"), ".asp", "")
    callingFile = Replace(callingFile, "/", "")
    callingFile = UCase(callingFile)


    Select Case callingFile
      Case "AIRCRAFT_DETAILS"
        redirectSite += "abiAircraftDetails.aspx"

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item("aircraft_id"))) Then
          If IsNumeric(Trim(HttpContext.Current.Request.Item(("aircraft_id")))) Then
            redirectSite += "ID=" & Trim(HttpContext.Current.Request.Item(("aircraft_id"))) & "&"
          End If
        End If

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item(("comp_id")))) Then
          If IsNumeric(Trim(HttpContext.Current.Request.Item(("comp_id")))) Then
            redirectSite += "Dealer=" & Trim(HttpContext.Current.Request.Item(("comp_id")))
          End If
        End If

      Case "LISTINGS"
        redirectSite += "abiForSale.aspx?"

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item(("comp_id")))) Then
          If IsNumeric(Trim(HttpContext.Current.Request.Item(("comp_id")))) Then
            redirectSite += "Dealer=" & Trim(HttpContext.Current.Request.Item(("comp_id"))) & "&"
          End If
        End If

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item(("airframe")))) Then
          redirectSite += "AirframeType=" & Trim(HttpContext.Current.Request.Item(("airframe"))) & "&"
        End If

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item(("make")))) Then
          redirectSite += "Make=" & Trim(HttpContext.Current.Request.Item(("make"))) & "&"
        End If

        If Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request.Item(("model")))) Then
          redirectSite += "Model=" & Trim(HttpContext.Current.Request.Item(("model")))
        End If

      Case Else
        If HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("HELICOPTERBUSINESSINDEX") Then
          redirectSite += "abiForsale.aspx?type=Helicopters&AirframeType=R"
        ElseIf HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("AVIATIONDEALERINDEX") Then
          redirectSite += "abiDealer.aspx"
        Else
          redirectSite += "defaultABI.aspx"
        End If
    End Select

    HttpContext.Current.Response.Redirect(redirectSite, True)
  End Sub
  Public Shared Function URLFriendly(ByVal startingText As String) As String
    Dim returnString As String = ""

    returnString = HttpUtility.UrlEncode(clsGeneral.clsGeneral.StripChars(Replace(Replace(Replace(startingText.ToString.ToLower, "+", "PLUS"), " ", "-"), "&", "_"), False))

    Return returnString
  End Function
  Public Shared Function AircraftModelForSaleURL(ByVal ModelID As Long, ByVal MakeName As String, ByVal ModelName As String) As String
    Dim returnString As String = ""
    returnString = MakeName + "-" + ModelName
    returnString = Replace(returnString, ",", "")
    returnString = Replace(returnString, ".", "")
    returnString = UCase(abi_functions.URLFriendly(returnString))
    returnString = "/listings/aircraft/model-for-sale/" & ModelID.ToString & "/" & returnString & "/"
    Return returnString
  End Function
  Public Shared Function AircraftMakeForSaleURL(ByVal makeName As String, ByVal airframeTypeCode As String, ByVal TypeCode As String) As String
    Dim returnString As String = "" '
    returnString = Replace(makeName.ToString, ",", "")
    returnString = Replace(returnString, ".", "")
    returnString = UCase(abi_functions.URLFriendly(makeName))
    returnString = "/listings/aircraft/make-for-sale/" & airframeTypeCode & "/" & TypeCode & "/" & returnString & "/"
    Return returnString
  End Function
  Public Shared Function AircraftDealerURL(ByVal companyID As Long, ByVal companyName As Object) As String
    Dim returnString As String = "" '
    returnString = Replace(companyName.ToString, ",", "")
    returnString = Replace(returnString, ".", "")
    returnString = UCase(abi_functions.URLFriendly(returnString))
    returnString = "/aircraftdealers/" + companyID.ToString + "/" + returnString + "/"
    Return returnString
  End Function
  Public Shared Function AircraftDetailsURL(ByVal aircraftID As Long, ByVal aircraftYear As Object, ByVal aircraftMake As String, ByVal aircraftModel As String, ByVal aircraftRegNo As Object) As String
    Dim returnString As String = "" '

    returnString += aircraftYear.ToString + "-"
    returnString += aircraftMake.ToString + "-"
    returnString += aircraftModel.ToString
    returnString = UCase(abi_functions.URLFriendly(returnString))
    returnString = "/listings/aircraft/for-sale/" + aircraftID.ToString + "/" + returnString

    Return returnString

  End Function
#End Region

#Region "Content Stats"
  Public Shared Function ReturnIPAddress() As String
    Dim StringIP As String = ""
    StringIP = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")

    If String.IsNullOrEmpty(StringIP) Then
      StringIP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
    Else 'This uses the xForwardedFor
      StringIP = StringIP.Split(",").Last().Trim()
    End If

    Return StringIP
  End Function

  Public Shared Function getABIBotExclusionList() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM BotList")

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      Else
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      End If

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

        Return Nothing

      End Try

    Catch ex As Exception

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Function insertBotData(ByVal botIPAddress As String, ByVal botName As String) As Boolean

    Dim bReturnValue As Boolean = False
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM BotIP WITH(NOLOCK) WHERE botip_address = '" + botIPAddress + "'")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>insertBotData(ByVal botIPAddress As String, ByVal botName As String) As Boolean</b><br />" + sQuery.ToString

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      Else
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in insertBotData(ByVal botIPAddress As String, ByVal botName As String) As Boolean load datatable" + constrExc.Message
      End Try

      If atemptable.Rows.Count = 0 Then

        sQuery = New StringBuilder()

        sQuery.Append("INSERT INTO BotIP (botip_address, botip_shortName)")
        sQuery.Append(" VALUES ('" + botIPAddress + "','" + botName.Trim + "')")

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>insertBotData(ByVal botIPAddress As String, ByVal botName As String) As Boolean</b><br />" + sQuery.ToString

        Try
          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()
          bReturnValue = True
        Catch SqlException
          aError = "Error in insertBotData(ByVal botIPAddress As String, ByVal botName As String) ExecuteNonQuery :" + SqlException.Message
        End Try

      End If

    Catch ex As Exception
      aError = "Error in insertBotData(ByVal botIPAddress As String, ByVal botName As String) As Boolean" + ex.Message
    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function isCrawlerOrBot(ByRef clientBrowser As HttpBrowserCapabilities) As Boolean

    Dim resultsTable As DataTable = Nothing
    Dim bIsBot As Boolean = False

    If clientBrowser.Crawler Then bIsBot = True

    resultsTable = getABIBotExclusionList()

    If Not IsNothing(resultsTable) Then

      If resultsTable.Rows.Count > 0 Then

        For Each r As DataRow In resultsTable.Rows

          If Not IsDBNull(r.Item("botlist_phrase")) Then
            If Not String.IsNullOrEmpty(r.Item("botlist_phrase").ToString) Then
              If (HttpContext.Current.Request.UserAgent.ToLower.Contains(r.Item("botlist_phrase").ToString.ToLower.Trim)) Then
                bIsBot = True
                Exit For
              End If
            End If
          End If

        Next

      End If

    End If

    If bIsBot Then
      insertBotData(ReturnIPAddress(), HttpContext.Current.Request.UserAgent.ToLower)
    End If

    Return bIsBot
    resultsTable = Nothing

  End Function

  Public Function Create_ABI_Stats(ByVal abistat_comp_id As Long, ByVal abistat_ac_id As Long, ByVal abistat_aircraft As String, ByVal abistat_wanted_id As Long, Optional ByVal abistat_registry_id As Long = 0) As Boolean
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim strDate As New System.DateTime
    strDate = FormatDateTime(Now(), vbGeneralDate)

    Dim ResponseCode As Boolean = False
    Try
      'This function inserts in the following places:
      'ABIFORSALE.aspx page. Whenever a DealerID is passed to the page. It will insert the abistat_comp_id
      'ABIAIRCRAFTDETAILS.aspx. This should insert the companyID, AircraftID and Aircraft fields.
      'ABIWANTEDS.aspx. This should insert both the company ID and the wanted ID.  abistat_user_agent

      QueryFields = "insert into ABI_Content_Stats(abistat_comp_id, "
      QueryValues = " values (@abistat_comp_id, "

      QueryFields += " abistat_ac_id, "
      QueryValues += " @abistat_ac_id, "

      QueryFields += " abistat_aircraft, "
      QueryValues += " @abistat_aircraft, "

      QueryFields += " abistat_ip_address, "
      QueryValues += " @abistat_ip_address, "

      QueryFields += " abistat_datetime, "
      QueryValues += " @abistat_datetime, "

      QueryFields += "abistat_wanted_id, "
      QueryValues += "@abistat_wanted_id, "

      QueryFields += "abistat_reg_ac_id, "
      QueryValues += "@abistat_reg_ac_id, "

      QueryFields += "abistat_user_agent) "
      QueryValues += "@abistat_user_agent) "

      Query = QueryFields & QueryValues

      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)

      SqlCommand.Parameters.AddWithValue("@abistat_datetime", strDate)
      SqlCommand.Parameters.AddWithValue("@abistat_ac_id", abistat_ac_id)
      SqlCommand.Parameters.AddWithValue("@abistat_comp_id", abistat_comp_id)
      SqlCommand.Parameters.AddWithValue("@abistat_ip_address", ReturnIPAddress())
      SqlCommand.Parameters.AddWithValue("@abistat_aircraft", abistat_aircraft)
      SqlCommand.Parameters.AddWithValue("@abistat_wanted_id", abistat_wanted_id)
      SqlCommand.Parameters.AddWithValue("@abistat_reg_ac_id", abistat_registry_id)
      SqlCommand.Parameters.AddWithValue("@abistat_user_agent", HttpContext.Current.Request.UserAgent.ToLower)

      If Not isCrawlerOrBot(HttpContext.Current.Request.Browser) Then
        SqlCommand.ExecuteNonQuery()
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Create_ABI_Stats(ByVal abistat_comp_id As Long, ByVal abistat_ac_id As Long, ByVal abistat_aircraft As String, ByVal abistat_wanted_id As Long) As Boolean</b><br />" & Query

      ResponseCode = True

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      Me.class_error = Me.class_error & "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"
      Return Nothing
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
    Return ResponseCode
  End Function

#End Region
End Class
