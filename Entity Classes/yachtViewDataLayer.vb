Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/yachtViewDataLayer.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:50a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: yachtViewDataLayer.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class yachtViewDataLayer

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

#Region "misc_functions"

  Public Function check_if_yacht_picture_exists(ByRef searchCriteria As yachtViewSelectionCriteria) As Boolean

    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append("SELECT ym_picture_exists_flag FROM yacht_model WITH(NOLOCK) WHERE amod_id =" + searchCriteria.YachtViewCriteriaYmodID.ToString)
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />check_pic_exists(ByRef searchCriteria As yachtViewSelectionCriteria) As Boolean </b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          If SqlReader.Item("ym_picture_exists_flag").ToString.ToUpper = "Y" Then
            bResult = True
          End If
        End If

      Catch SqlException
        aError = "Error in check_if_yacht_picture_exists ExecuteReader : " & SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in check_if_yacht_picture_exists(ByRef searchCriteria As yachtViewSelectionCriteria) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bResult

  End Function

  Public Sub yacht_view_display_model_pic(ByRef searchCriteria As yachtViewSelectionCriteria, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim imgDisplayFolder As String = ""

    imgDisplayFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("ModelPicturesFolderVirtualPath")

    Try

      If check_if_yacht_picture_exists(searchCriteria) Then

        If searchCriteria.YachtViewCriteriaYmodID > -1 Then
          htmlOut.Append("<br /><div class=""picture""><img src=""" + imgDisplayFolder.Trim + "/" + searchCriteria.YachtViewCriteriaYmodID.ToString + ".jpg"" alt=""" + commonEvo.Get_Yacht_Model_Info(searchCriteria.YachtViewCriteriaYmodID, False) + """  title=""" + commonEvo.Get_Yacht_Model_Info(searchCriteria.YachtViewCriteriaYmodID, False) + """ height=""205"" width=""300"" border=""1"" style=""height: 205px; width: 300px;""/></div>")
        End If

      Else
        htmlOut.Append("<br /><div class=""picture""><b>&nbsp;Image&nbsp;or&nbsp;Video&nbsp;not&nbsp;Available&nbsp;</b></div>")
      End If

    Catch ex As Exception

      aError = "Error in yacht_view_display_model_pic(ByRef searchCriteria As yachtViewSelectionCriteria, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing

  End Sub

#End Region

#Region "yacht_view_16_functions"
  ''' <summary>
  ''' 
  ''' </summary>
  ''' <param name="inTable"></param>
  ''' <param name="ColumnOneString"></param>
  ''' <param name="ColumnOneName"></param>
  ''' <param name="ColumnTwoString"></param>
  ''' <param name="ColumnTwoName"></param>
  ''' <param name="DisplayTotal"></param>
  ''' <param name="RequestVariableName"></param>
  ''' <param name="use_RequestVariableName_as_val"></param>
  ''' <param name="RequestVariableName2">This is an optional parameter to send a second request variable name to the performYachtSearch js function. If you don't need to use it - just leave it empty.</param>
  ''' <param name="OptionalRequestVariableNameOperator2">This is an optional parameter to send a second request variable value to the performYachtSearch js function. If you don't need to use it - just leave it empty.</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function display_two_column_view16(ByVal inTable As DataTable, ByVal ColumnOneString As String, ByVal ColumnOneName As String, ByVal ColumnTwoString As String, ByVal ColumnTwoName As String, ByVal DisplayTotal As Boolean, ByVal RequestVariableName As String, ByVal use_RequestVariableName_as_val As Boolean, ByVal RequestVariableName2 As String, ByVal OptionalRequestVariableNameOperator2 As String) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim ColumnTotal As Long = 0

    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>" + ColumnOneString.Trim + "</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b>" + ColumnTwoString.Trim + "</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            If use_RequestVariableName_as_val Then
              htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(RequestVariableName).ToString + "','" + RequestVariableName2 + "','" + IIf(Not String.IsNullOrEmpty(OptionalRequestVariableNameOperator2), OptionalRequestVariableNameOperator2, "") + "');""", "") + ">" + Row.Item(ColumnOneName).ToString + "</a></td>")
              htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(RequestVariableName).ToString + "','" + RequestVariableName2 + "','" + IIf(Not String.IsNullOrEmpty(OptionalRequestVariableNameOperator2), OptionalRequestVariableNameOperator2, "") + "');""", "") + ">" + Row.Item(ColumnTwoName).ToString + "</a></td>")
            Else
              htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(ColumnOneName).ToString + "','" + RequestVariableName2 + "','" + IIf(Not String.IsNullOrEmpty(OptionalRequestVariableNameOperator2), OptionalRequestVariableNameOperator2, "") + "');""", "") + ">" + Row.Item(ColumnOneName).ToString + "</a></td>")
              htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(ColumnOneName).ToString + "','" + RequestVariableName2 + "','" + IIf(Not String.IsNullOrEmpty(OptionalRequestVariableNameOperator2), OptionalRequestVariableNameOperator2, "") + "');""", "") + ">" + Row.Item(ColumnTwoName).ToString + "</a></td>")
            End If
          Else
            If use_RequestVariableName_as_val Then
              htmlOut.Append("<td align='left' valign='top'>" & Row.Item(ColumnOneName).ToString & "</td>")
              htmlOut.Append("<td align='right' valign='top'>" & Row.Item(ColumnTwoName).ToString & "</td>")
            Else
              htmlOut.Append("<td align='left' valign='top'>" & Row.Item(ColumnOneName).ToString & "</td>")
              htmlOut.Append("<td align='right' valign='top'>" & Row.Item(ColumnTwoName).ToString & "</td>")
            End If
          End If


          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item(ColumnTwoName).ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function
  Public Function display_two_column_view16_lifecycle(ByRef LookupTable As DataTable, ByVal inTable As DataTable, ByVal DisplayTotal As Boolean) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim ColumnTotal As Long = 0

    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>Life Cycle Stage</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b># of Yachts</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' onclick=""javascript:PerformYachtSearch('yt_lifecycle_id','" & LinkLifecycleCorrectly(LookupTable, Row.Item("yt_lifecycle_id")) & "','','');"">" + Row.Item("yl_lifecycle_name").ToString + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' onclick=""javascript:PerformYachtSearch('yt_lifecycle_id','" & LinkLifecycleCorrectly(LookupTable, Row.Item("yt_lifecycle_id")) & "','','');"">" + Row.Item("tcount").ToString + "</a></td>")
          Else
            htmlOut.Append("<td align='left' valign='top'>" & Row.Item("yl_lifecycle_name").ToString & "</td>")
            htmlOut.Append("<td align='right' valign='top'>" & Row.Item("tcount").ToString & "</td>")
          End If


          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item("tcount").ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function
  Public Function display_two_column_view16_brand(ByVal inTable As DataTable, ByVal ColumnOneString As String, ByVal ColumnOneName As String, ByVal ColumnTwoString As String, ByVal ColumnTwoName As String, ByVal DisplayTotal As Boolean, ByVal RequestVariableName As String, ByVal box_combo As String, ByVal size_combo As String) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim seperated_sections As String = ""
    Dim cat_array(8) As String
    Dim i As Integer = 0

    Dim ColumnTotal As Long = 0



    cat_array = Split(size_combo, "##")




    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>" + ColumnOneString.Trim + "</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b>" + ColumnTwoString.Trim + "</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          seperated_sections = ""
          For i = 0 To 6
            seperated_sections = seperated_sections & Row.Item(ColumnOneName).ToString() & "|" & cat_array(i) & "##"
          Next
          seperated_sections = Left(seperated_sections, Len(seperated_sections) - 2)

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('cboYachtCategoryID','" & box_combo & "','cboYachtBrandID', '" & seperated_sections & "');""", "") + ">" + Row.Item(ColumnOneName).ToString + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('cboYachtCategoryID','" & box_combo & "','cboYachtBrandID','" & seperated_sections & "');""", "") + ">" + Row.Item(ColumnTwoName).ToString + "</a></td>")
          Else
            htmlOut.Append("<td align='left' valign='top'>" + Row.Item(ColumnOneName).ToString + "</td>")
            htmlOut.Append("<td align='right' valign='top'>" + Row.Item(ColumnTwoName).ToString + "</td>")
          End If

          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item(ColumnTwoName).ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function
  Public Function LinkLifecycleCorrectly(ByVal lookupTable As DataTable, ByVal lifecycleID As Long) As String
    Dim returnString As String = ""
    Dim filtered As DataRow()
    Dim FilteredTable As New DataTable

    If Not IsNothing(lookupTable) Then
      If lookupTable.Rows.Count > 0 Then
        FilteredTable = lookupTable.Clone

        Dim selectQuery As String = ""
        filtered = lookupTable.Select("yl_lifecyle_id = " & lifecycleID.ToString, "")

        For Each atmpDataRow In filtered
          FilteredTable.ImportRow(atmpDataRow)
        Next

        For Each r As DataRow In FilteredTable.Rows
          If returnString <> "" Then
            returnString += "##"
          End If
          returnString += "" & r("yl_lifecyle_id") & "|" & r("yls_lifecycle_status") & ""
        Next

      End If
    End If

    Return returnString
  End Function
  Public Function display_summary_by_yacht_motorType(ByVal inTable As DataTable) As String

    Dim OutputTotal As Integer = 0
    Dim OutputSaleTotal As Integer = 0
    Dim OutputCharterTotal As Integer = 0

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    If Not IsNothing(inTable) Then

      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'><p>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='2' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>Yacht Size</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b># of Yachts</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b># For Sale</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b># For Charter</b></td>")

        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap'><a class=""underline"" href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" + Row.Item("ym_category_size").ToString + "|" + Row.Item("ym_motor_type").ToString + "','','');"">" + Row.Item("ycs_description").ToString.Trim + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" + Row.Item("ym_category_size").ToString + "|" + Row.Item("ym_motor_type").ToString + "','','');"">" + Row.Item("tcount").ToString.Trim + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" + Row.Item("ym_category_size").ToString + "|" + Row.Item("ym_motor_type").ToString + "','for_sale','true');"">" + Row.Item("yforsale").ToString.Trim + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" & Row("ym_category_size").ToString & "|" & Row("ym_motor_type").ToString & "','for_charter','true');"">" & Row("yforcharter") & "</a></td>")
          Else
            htmlOut.Append("<td align='left' valign='top' nowrap='nowrap'>" + Row.Item("ycs_description").ToString.Trim + "</td>")
            htmlOut.Append("<td align='right' valign='top'>" + Row.Item("tcount").ToString.Trim + "</td>")
            htmlOut.Append("<td align='right' valign='top'>" + Row.Item("yforsale").ToString.Trim + "</td>")
            htmlOut.Append("<td align='right' valign='top'>" & Row("yforcharter") & "</td>")
          End If

          htmlOut.Append("</tr>")

          OutputCharterTotal += CInt(Row.Item("yforcharter").ToString)
          OutputSaleTotal += CInt(Row.Item("yforsale").ToString)
          OutputTotal += CInt(Row.Item("tcount").ToString)

        Next

        If Not toggleRowColor Then
          htmlOut.Append("<tr class='alt_row'>")
          toggleRowColor = True
        Else
          htmlOut.Append("<tr bgcolor='white'>")
          toggleRowColor = False
        End If

        htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
        htmlOut.Append("<td align='right' valign='top' class='blue_text'>" + OutputTotal.ToString + "</td>")
        htmlOut.Append("<td align='right' valign='top' class='blue_text'>" + OutputSaleTotal.ToString + "</td>")
        htmlOut.Append("<td align='right' valign='top' class='blue_text'>" & OutputCharterTotal & "</td>")
        htmlOut.Append("</tr>")
        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</p></div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts of this type to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts of this type to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function
  Public Function get_yacht_all_mfr_years(ByVal mfr_comp_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select yt_year_mfr, COUNT(*) as tcount ")
      sQuery.Append(" from Yacht with (NOLOCK)  ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id  ")
      sQuery.Append(" where yt_journ_id = 0  ")
      If mfr_comp_id > 0 Then
        sQuery.Append(" And ym_mfr_comp_id = " & mfr_comp_id & "  ")
      End If
      sQuery.Append(" group by yt_year_mfr ")
      sQuery.Append(" order by yt_year_mfr ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_all_mfr_years() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_for_mfr(ByVal mfr_comp_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select ym_brand_name, ym_model_name, yt_yacht_name, ")
      sQuery.Append(" yt_year_mfr, yt_hull_mfr_nbr, yt_forsale_flag, yt_forsale_status, yt_asking_price, ")
      sQuery.Append(" yt_central_agent_flag, yt_id, yt_for_charter_flag, yt_for_lease_flag ")
      sQuery.Append(" from Yacht with (NOLOCK)  ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id  ")
      sQuery.Append(" where yt_journ_id = 0  ")
      sQuery.Append(" And ym_mfr_comp_id = " & mfr_comp_id & "  ")
      sQuery.Append(" order by yt_year_mfr desc, yt_yacht_name asc, ym_brand_name, ym_model_name ")



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_for_mfr() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_crossover_companies_section3() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select distinct comp_name, comp_city, comp_state, comp_country, comp_id ")
      sQuery.Append(" from Company with (NOLOCK)  ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_comp_id = comp_id and yr_journ_id = comp_journ_id ")
      sQuery.Append("  where comp_journ_id = 0 ")
      sQuery.Append(" and comp_active_flag='Y'  ")
      sQuery.Append(" AND yr_contact_type in ('00','08') ")


      'If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
      '  sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      'ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
      '  sQuery.Append(" and (comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      'ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
      '  sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      'ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
      '  sQuery.Append(" and comp_product_commercial_flag = 'Y'")
      'ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
      '  sQuery.Append(" and comp_product_helicopter_flag = 'Y'")
      'ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
      '  sQuery.Append(" and comp_product_business_flag = 'Y'")
      'End If

      sQuery.Append(" group by comp_name, comp_city, comp_state, comp_country, comp_id  ")
      sQuery.Append(" having (select COUNT(distinct cref_ac_id) from Aircraft_Reference with (NOLOCK) where cref_comp_id = comp_id  ")
      sQuery.Append(" and cref_journ_id = 0 AND cref_contact_type in ('00','08','97'))= 0  ")
      sQuery.Append(" and comp_name <> 'AWAITING DOCUMENTATION' ")
      sQuery.Append(" order by comp_name, comp_city, comp_state, comp_country ")



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_all_mfr() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_crossover_companies_section2() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      '      sQuery.Append(" select distinct comp_name, comp_city, comp_state, comp_country, comp_id, ")
      '      sQuery.Append(" (select COUNT(distinct cref_ac_id) from Aircraft_Reference with (NOLOCK) where cref_comp_id = comp_id and ")
      '      sQuery.Append(" cref_journ_id = 0) as AircraftRel, ")
      '      sQuery.Append(" (select COUNT(distinct yr_yt_id) from Yacht_Reference with (NOLOCK) where yr_comp_id = comp_id and ")
      '      sQuery.Append(" yr_journ_id = 0) as YachtRel ")
      '      sQuery.Append(" from Company with (NOLOCK)  ")
      '      '--inner join Contact  c with (NOLOCK) on comp_id = contact_comp_id and comp_journ_id = contact_journ_id
      '      sQuery.Append(" where comp_journ_id = 0  ")
      '      '--and comp_product_yacht_flag='Y'
      '      sQuery.Append(" and comp_active_flag='Y' ")
      '      sQuery.Append(" AND comp_id in (select distinct bustypref_comp_id from Business_Type_Reference with (NOLOCK) ")
      '      sQuery.Append(" where bustypref_journ_id = 0 and bustypref_type in ('XX','YY','XY')) ")
      '      sQuery.Append(" group by comp_name, comp_city, comp_state, comp_country, comp_id ")
      '      sQuery.Append(" having (select COUNT(*) from Yacht_Reference with (NOLOCK) where yr_comp_id = comp_id and ")
      '      sQuery.Append(" yr_journ_id = 0) > 0 and ")
      '      sQuery.Append(" (select COUNT(*) from Aircraft_Reference with (NOLOCK) where cref_comp_id = comp_id and ")
      '      sQuery.Append(" cref_journ_id = 0) > 0 ")
      'sQuery.Append(" order by comp_name, comp_city, comp_state, comp_country ")


      sQuery.Append(" select distinct comp_name, comp_city, comp_state, comp_country, comp_id, ")
      sQuery.Append(" (select COUNT(distinct cref_ac_id) from Aircraft_Reference with (NOLOCK) where cref_comp_id = comp_id  ")
      sQuery.Append(" and cref_journ_id = 0 AND cref_contact_type in ('00','08','97')) as AircraftRel,  ")
      sQuery.Append(" (select COUNT(distinct yr_yt_id) from Yacht_Reference with (NOLOCK)  ")
      sQuery.Append(" where yr_comp_id = comp_id and yr_journ_id = 0 AND yr_contact_type in ('00','08','97')) as YachtRel  ")
      sQuery.Append(" from Aircraft_Company_Flat with (NOLOCK)  ")
      sQuery.Append(" where cref_journ_id = 0  ")
      sQuery.Append(" and comp_active_flag='Y'  ")
      sQuery.Append(" AND cref_contact_type in ('00','08','97') ")
      sQuery.Append(" and comp_name <> 'AWAITING DOCUMENTATION' ")


      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and comp_product_commercial_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and comp_product_helicopter_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and comp_product_business_flag = 'Y'")
      End If


      sQuery.Append(" group by comp_name, comp_city, comp_state, comp_country, comp_id  ")
      sQuery.Append(" having (select COUNT(*) from Yacht_Reference with (NOLOCK)  ")
      sQuery.Append(" where yr_comp_id = comp_id and yr_journ_id = 0 and yr_contact_type in ('00','08')) > 0  ")
      sQuery.Append(" order by comp_name, comp_city, comp_state, comp_country ")




      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_all_mfr() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_crossover_companies_section2_new(ByVal amod_id As Long, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select comp_id, comp_name, comp_city, comp_state, comp_country, cross_Yacht_Count, cross_CompID, cross_Company, cross_city, cross_State, cross_Country,  ")
      sQuery.Append(" cross_BFlag, cross_CFlag, cross_HFlag, cross_YFlag, cross_aircraft_count ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on comp_id = cross_Yacht_CompID and comp_journ_id = 0 ")
      sQuery.Append(" where cross_aircraft_count > 0 ")

      If amod_id > 0 Or Trim(make_name) <> "" Then
        sQuery.Append(" and cross_CompID in ( select distinct cross_compid from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" where cross_Yacht_CompId in ( select distinct cross_Yacht_CompId from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id ")
        sQuery.Append(" and cref_journ_id = 0 and cref_contact_type in ('00','08','97') ")

        If amod_id > 0 Then
          sQuery.Append(" where amod_id = " & amod_id & " ")
        ElseIf Trim(make_name) <> "" Then
          sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
        End If

        sQuery.Append(" )) ")
      End If

      sQuery.Append(" order by comp_name ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies_section2_new() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_crossover_companies(ByVal has_ac As Boolean, ByVal type_string As String, ByVal amod_id As Integer, ByVal make_name As String, ByVal yacht_size As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" SELECT distinct CompId, ")
      sQuery.Append(" Company, ")
      sQuery.Append(" ContactId, ")
      sQuery.Append(" ContactFullName, ")
      sQuery.Append(" Country, ")
      sQuery.Append(" State, ")
      sQuery.Append(" City, ")
      sQuery.Append(" BFlag, ")
      sQuery.Append(" YFlag,   ")
      sQuery.Append(" ContactGroup, ")
      sQuery.Append(" ContactTitle, ")
      sQuery.Append(" num_prevaircraft, ")
      sQuery.Append(" num_aircraft, ")
      sQuery.Append(" num_yachts, ")
      sQuery.Append(" contactlname ")

      sQuery.Append(" FROM Yacht_Crossover_Table WITH (NOLOCK) ")

      If amod_id > 0 Or Trim(make_name) <> "" Then
        sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0  and cref_contact_type in ('00','08','97') ")
      ElseIf Trim(yacht_size) <> "" Then
        sQuery.Append(" inner join View_Yacht_Company_Flat with (NOLOCK) on ContactId = contact_id and yr_journ_id = 0  and yr_contact_type in ('00','08','97')   ")
        sQuery.Append(" inner join Yacht_Category_Size with (NOLOCK) on ycs_category_size = ym_category_size ")
        If Trim(yacht_size) <> "" Then
          sQuery.Append("  and ycs_category_size = '" & Trim(yacht_size) & "' ")
        End If
      End If


      'sQuery.Append(" FROM Company WITH (NOLOCK) ")
      'sQuery.Append(" INNER JOIN Contact WITH (NOLOCK) ON contact_comp_id = comp_id AND contact_journ_id = comp_journ_id ")
      'sQuery.Append(" WHERE(comp_journ_id = 0) ")
      'sQuery.Append("AND (comp_active_flag = 'Y') ")
      ''--AND (comp_product_yacht_flag = 'Y')
      'sQuery.Append("AND (contact_active_flag = 'Y') ")
      'sQuery.Append("AND (contact_hide_flag = 'N') ")
      'sQuery.Append("AND (EXISTS (SELECT NULL FROM Business_Type_Reference WITH (NOLOCK) ")
      'sQuery.Append(" WHERE bustypref_comp_id = comp_id  ")
      'sQuery.Append("AND (bustypref_journ_id = comp_journ_id) ")
      'sQuery.Append("AND (bustypref_type IN ('XX','XY','YY'))  ")
      'sQuery.Append(") ")
      'sQuery.Append(")         ")

      ''-- Does This Contact Or Any Contact/Related Company Have A Yacht
      'sQuery.Append("AND (EXISTS (SELECT NULL FROM Yacht_Reference WITH (NOLOCK) ")
      'sQuery.Append("WHERE (yr_contact_id IN (SELECT ContactId FROM ReturnContactIdRelationshipsByContactId(contact_id))) ")
      'sQuery.Append("AND (yr_journ_id = 0) ")
      'sQuery.Append(") ")
      'sQuery.Append("OR ")
      'sQuery.Append("EXISTS (SELECT NULL FROM Yacht_Reference WITH (NOLOCK) ")
      'sQuery.Append(" WHERE(yr_contact_id = contact_id) ")
      'sQuery.Append("AND (yr_journ_id = 0) ")
      'sQuery.Append(")        ")
      'sQuery.Append(") ")

      ''-- Does This Contact Or Any Contact/Related Company Have An Aircraft
      'sQuery.Append("AND (EXISTS (SELECT NULL FROM Aircraft_Reference WITH (NOLOCK) ")
      'sQuery.Append("WHERE (cref_contact_id IN (SELECT ContactId FROM ReturnContactIdRelationshipsByContactId(contact_id))) ")
      'sQuery.Append("AND (cref_journ_id = 0) ")
      'sQuery.Append(") ")
      'sQuery.Append("OR ")
      'sQuery.Append("EXISTS (SELECT NULL FROM Aircraft_Reference WITH (NOLOCK) ")
      'sQuery.Append("WHERE(cref_contact_id = contact_id) ")
      'sQuery.Append("AND (cref_journ_id = 0) ")
      'sQuery.Append(")                    ")
      'sQuery.Append(") ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" Where (BFlag = 'Y' or HFlag = 'Y' or CFlag = 'Y' or YFlag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" Where (HFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" Where (BFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" Where CFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" Where HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" Where BFlag = 'Y' or HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" Where BFlag = 'Y'")
      End If

      If has_ac = True Then
        sQuery.Append(" and num_aircraft > 0 ")
      Else
        sQuery.Append(" and num_aircraft = 0 ")
      End If

      If Trim(type_string) = "P" Then
        sQuery.Append(" and num_prevaircraft > 0 ")
      End If


      If amod_id > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & "")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & Trim(make_name) & "' ")
      End If

      'comp_product_helicopter_flag
      'comp_product_commercial_flag 

      sQuery.Append(" ORDER BY contactlname, contactgroup, Company ")
      'sQuery.Append("ORDER BY ContactFullName, company,country, state, city ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function




  Public Function get_summary_jets_by_weight_class_company() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct acwgtcls_name, count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" and amod_type_code in ('J','E') ")
      sQuery.Append(" INNER JOIN Aircraft_Weight_Class with (NOLOCK) on acwgtcls_code = amod_weight_class ")
      sQuery.Append(" where(cross_aircraft_count > 0) ")
      sQuery.Append(" group by acwgtcls_name ")
      sQuery.Append(" order by count(distinct ac_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_jets_by_weight_class() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct acwgtcls_name, count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" and amod_type_code in ('J','E') ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and ac_product_commercial_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_helicopter_flag = 'Y' ")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and ac_product_business_flag  = 'Y'")
      End If

      sQuery.Append(" INNER JOIN Aircraft_Weight_Class with (NOLOCK) on acwgtcls_code = amod_weight_class ")
      sQuery.Append(" where Num_Aircraft > 0   ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or HFlag = 'Y' or CFlag = 'Y' or YFlag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (HFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and CFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and BFlag = 'Y' or HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and BFlag = 'Y'")
      End If


      sQuery.Append(" group by acwgtcls_name ")
      sQuery.Append(" order by count(distinct ac_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_yachts_comp_by(ByVal sum_by As String, ByVal type_string As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct " & sum_by & ", count(distinct yt_id) as acount ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on cross_Yacht_Compid = comp_id and comp_journ_id = 0 ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_comp_id = comp_id and yr_journ_id = comp_journ_id and yr_contact_type in ('00') ")
      sQuery.Append(" inner join Yacht with (NOLOCK) on yt_id = yr_yt_id and yt_journ_id = yr_journ_id  ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on ym_model_id = yt_model_id ")
      sQuery.Append(" inner join Yacht_Category_Size with (NOLOCK) on ycs_category_size = ym_category_size ")
      sQuery.Append(" where cross_aircraft_count = 0 ")

      If Trim(type_string) = "P" Then
        sQuery.Append(" and cross_prevaircraft_count > 0 ")
      End If

      sQuery.Append(" group by " & sum_by & "   ")
      sQuery.Append(" order by count(distinct yt_id) desc ")
 

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_yachts_by(ByVal sum_by As String, ByVal type_string As String, ByVal yacht_size As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct " & sum_by & ", count(distinct yt_id) as acount ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" inner join View_Yacht_Company_Flat with (NOLOCK) on ContactId = contact_id and yt_journ_id = 0  ")
      sQuery.Append(" and yct_code  in ('00','08','97')  ")
      sQuery.Append(" inner join Yacht_Category_Size on ycs_category_size = ym_category_size ")
      If Trim(yacht_size) <> "" Then
        sQuery.Append("  and ycs_category_size = '" & Trim(yacht_size) & "' ")
      End If
      sQuery.Append(" where Num_Aircraft = 0 ")

      If Trim(type_string) = "P" Then
        sQuery.Append(" and num_prevaircraft > 0 ")
      End If



      sQuery.Append(" group by " & sum_by & "   ")
      sQuery.Append(" order by count(distinct yt_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_ac_makes_owned_company() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select distinct amod_make_name, ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" where cross_aircraft_count > 0 ")
      sQuery.Append(" group by amod_make_name ")
      sQuery.Append(" order by count(distinct ac_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_ac_makes_owned() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct amod_make_name,  ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" where Num_Aircraft > 0  ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or HFlag = 'Y' or CFlag = 'Y' or YFlag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (HFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and CFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and BFlag = 'Y' or HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and BFlag = 'Y'")
      End If

      sQuery.Append(" group by amod_make_name  ")
      sQuery.Append(" order by count(distinct ac_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_whole_vs_fractional(ByVal type_of As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
        sQuery.Append(" select distinct acot_name as ac_own_type, count(distinct comp_id) as tcount from View_Aircraft_Company_Flat with (NOLOCK) where cref_journ_id = 0 and cref_contact_type in (" & type_of & ") ")
        sQuery.Append(" and comp_id in (" & HttpContext.Current.Session.Item("MasterCompanyWhere") & ")")
        sQuery.Append(" group by acot_name ")
        sQuery.Append(" order by acot_name desc ")

        SqlConn.ConnectionString = clientConnectString
        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 180

        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

      End If


    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_whole_vs_fractional() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_ac_business_types() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select bustypref_type, cbus_name, COUNT(distinct comp_id) as tcount from company with (NOLOCK) ")
      sQuery.Append(" inner join Business_Type_Reference with (NOLOCK) on comp_id = bustypref_comp_id  ")
      sQuery.Append(" and comp_journ_id = bustypref_journ_id  ")
      sQuery.Append(" inner join Company_Business_Type with (NOLOCK) on bustypref_type = cbus_type ")
      sQuery.Append(" where comp_journ_id = 0 ")
      sQuery.Append(" and comp_id in ( ")
      sQuery.Append(" select distinct comp_id from company with (NOLOCK) ")
      sQuery.Append(" inner join Business_Type_Reference with (NOLOCK) on comp_id = bustypref_comp_id  ")
      sQuery.Append(" and comp_journ_id = bustypref_journ_id where comp_journ_id = 0  ")
      sQuery.Append(" and bustypref_type='YY' and (comp_product_business_flag = 'Y'  ")
      sQuery.Append(" or comp_product_helicopter_flag = 'Y'  ")
      sQuery.Append(" or comp_product_commercial_flag = 'Y')  ")
      sQuery.Append(" ) ")
      sQuery.Append(" and bustypref_type not in ('YY','XX','XY') ")
      sQuery.Append(" and cbus_aircraft_flag = 'Y' ")
      sQuery.Append(" group by bustypref_type, cbus_name ")
      sQuery.Append(" order by COUNT(distinct comp_id) desc ")



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_ac_business_types() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_ac_models_owned_by_ind(ByVal type_sum As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If Trim(type_sum) = "Make" Then
        sQuery.Append(" select distinct amod_make_name, ")
      Else
        sQuery.Append(" select distinct amod_make_name, amod_model_name, amod_id, ")
      End If


      sQuery.Append(" count(distinct cref_ac_id) as acount ")
      sQuery.Append(" from View_Aircraft_Company_Flat with (NOLOCK) ")
      sQuery.Append(" where cref_journ_id = 0 and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" and comp_id in (" & HttpContext.Current.Session.Item("MasterCompanyWhere") & ")")


      If Trim(type_sum) = "Make" Then
        sQuery.Append(" group by amod_make_name ")
      Else
        sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
      End If
      sQuery.Append(" order by count(distinct cref_ac_id) desc ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function




  Public Function get_summary_ac_models_owned_company() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("   select distinct amod_make_name, amod_model_name, amod_id, ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" where cross_aircraft_count > 0 ")
      sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
      sQuery.Append(" order by count(distinct ac_id) desc ")



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_ac_models_owned_company() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_ac_models_owned() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct amod_make_name, amod_model_name, amod_id, ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and ac_product_commercial_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_helicopter_flag = 'Y' ")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and ac_product_business_flag  = 'Y'")
      End If

      sQuery.Append(" where Num_Aircraft > 0  ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or HFlag = 'Y' or CFlag = 'Y' or YFlag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (HFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and CFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and BFlag = 'Y' or HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and BFlag = 'Y'")
      End If


      sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
      sQuery.Append(" order by count(distinct ac_id) desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_yachts_owned_by_ac_model_company(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      ' -- GET A LIST OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL
      sQuery.Append(" select distinct yt_hull_mfr_nbr, yt_year_mfr, ym_model_name, ym_brand_name, yt_yacht_name, yt_id, case ym_category_size when 'G' then 'GIGA' when 'M' then 'MEGA' when 'L' then 'LUXURY' else 'SUPER' end as YachtClass  ")
      sQuery.Append(" from Yacht with (NOLOCK)  ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id  ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id  ")
      sQuery.Append(" where yt_journ_id = 0  ")
      sQuery.Append(" and yr_comp_id in ( select distinct cross_compid from Yacht_Company_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" where cross_Yacht_CompId in ( select distinct cross_Yacht_CompId from Yacht_Company_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id  ")
      sQuery.Append(" and cref_journ_id = 0 and cref_contact_type in ('00','08','97')  ")
  
      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If

      sQuery.Append(" )) ")
      sQuery.Append(" order by YachtClass, yt_id ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_ac_models_owned() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_ac_models_owned() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_yachts_owned_by_ac_model(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      ' -- GET A LIST OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL
      sQuery.Append(" select distinct yt_hull_mfr_nbr, yt_year_mfr, ym_model_name, ym_brand_name, yt_yacht_name, yt_id, ")
      sQuery.Append(" case ym_category_size when 'G' then 'GIGA' when 'M' then 'MEGA' when 'L' then 'LUXURY' else 'SUPER' end as YachtClass ")
      sQuery.Append(" from Yacht with (NOLOCK) ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id ")
      sQuery.Append(" where yt_journ_id = 0  ")
      sQuery.Append(" and yr_contact_id in ( ")
      sQuery.Append(" select distinct contactid from Yacht_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" where contactgroup in ( ")
      sQuery.Append(" select distinct ContactGroup  ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ") 
      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If

      sQuery.Append(" )) ")
      sQuery.Append(" order by YachtClass, yt_id ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_ac_models_owned() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_ac_models_owned() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function



  Public Function get_summary_by_ac_type_company() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct case amod_airframe_type_code when 'R' then 'Helicopter' else '' end as amod_airframe_type_code, case atype_name when 'Jet Airliner' then 'Business Jet' else atype_name end as atype_name, ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      sQuery.Append(" where cross_aircraft_count > 0  ")
      sQuery.Append(" group by amod_airframe_type_code,case atype_name when 'Jet Airliner' then 'Business Jet' else atype_name end ")
      sQuery.Append(" order by  amod_airframe_type_code, atype_name ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_by_ac_type_company() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_by_ac_type() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct case amod_airframe_type_code when 'R' then 'Helicopter' else '' end as amod_airframe_type_code, case atype_name when 'Jet Airliner' then 'Business Jet' else atype_name end as atype_name, ")
      sQuery.Append(" count(distinct ac_id) as acount ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_helicopter_flag = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (ac_product_business_flag  = 'Y' or ac_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and ac_product_commercial_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_helicopter_flag = 'Y' ")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and ac_product_business_flag  = 'Y' or ac_product_helicopter_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and ac_product_business_flag  = 'Y'")
      End If

      sQuery.Append(" where(Num_Aircraft > 0) ")

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or HFlag = 'Y' or CFlag = 'Y' or YFlag = 'Y')") ' this should allow all to be seen
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (HFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (BFlag = 'Y' or CFlag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and CFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and BFlag = 'Y' or HFlag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
        sQuery.Append(" and BFlag = 'Y'")
      End If
 

      sQuery.Append(" group by amod_airframe_type_code,case atype_name when 'Jet Airliner' then 'Business Jet' else atype_name end  ")
      sQuery.Append(" order by  amod_airframe_type_code,atype_name ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_yacht_class_company(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      ' -- GET A SUMMARY OF THE CLASS OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL

      sQuery.Append(" select ycs_description as YachtClass, ycs_feet_from, COUNT(distinct yt_id) as ycount ")
      sQuery.Append(" from Yacht with (NOLOCK) ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id ")
      sQuery.Append(" inner join Yacht_Category_Size on ycs_motor_type=ycs_motor_type and ycs_category_size=ym_category_size ")
      sQuery.Append(" where yt_journ_id = 0 ")
      sQuery.Append(" and yr_comp_id in ( select distinct cross_compid from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" where cross_Yacht_CompId in ( select distinct cross_Yacht_CompId from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id ")
      sQuery.Append(" and cref_journ_id = 0 and cref_contact_type in ('00','08','97') ")
 
      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If

      sQuery.Append(" )) ")
      sQuery.Append(" group by ycs_description, ycs_feet_from order by ycs_feet_from desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_yacht_class() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_yacht_class_company() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_summary_yacht_class(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      ' -- GET A SUMMARY OF THE CLASS OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL

      'sQuery.Append(" select distinct case ym_category_size when 'G' then 'GIGA' when 'M' then 'MEGA' when 'L' then 'LUXURY' else 'SUPER' end as YachtClass,  ")
      sQuery.Append(" select ycs_description as YachtClass, ycs_feet_from, ")
      sQuery.Append(" COUNT(distinct yt_id) as ycount ")
      sQuery.Append(" from Yacht with (NOLOCK) ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id ")
      sQuery.Append(" inner join Yacht_Category_Size on ycs_motor_type=ycs_motor_type and ycs_category_size=ym_category_size ")
      sQuery.Append(" where yt_journ_id = 0 ")
      sQuery.Append(" and yr_contact_id in ( ")
      sQuery.Append(" select distinct contactid from Yacht_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" where contactgroup in ( ")
      sQuery.Append(" select distinct ContactGroup  ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If

      sQuery.Append(" )) ")
      sQuery.Append(" group by ycs_description, ycs_feet_from order by ycs_feet_from desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_yacht_class() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_yacht_class() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_summary_yacht_by_brand_company(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      '  -- GET A SUMMARY OF THE BRANDS OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL
      sQuery.Append(" select distinct ym_brand_name,COUNT(distinct yt_id) as ycount ")
      sQuery.Append(" from Yacht with (NOLOCK) inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id ")
      sQuery.Append(" where yt_journ_id = 0 ")
      sQuery.Append(" and yr_comp_id in ( select distinct cross_compid from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" where cross_Yacht_CompId in ( select distinct cross_Yacht_CompId from Yacht_Company_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on cross_compid = comp_id ")
      sQuery.Append(" and cref_journ_id = 0 and cref_contact_type in ('00','08','97') ")

      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If
      sQuery.Append(" )) ")
      sQuery.Append(" group by ym_brand_name ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_yacht_class() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_yacht_class() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function


  Public Function get_summary_yacht_by_brand(ByVal amod_id As Integer, ByVal make_name As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      '  -- GET A SUMMARY OF THE BRANDS OF YACHTS OWNED BY OWNERS OF A SPECIFIC AIRCRAFT MODEL
      sQuery.Append(" select distinct ym_brand_name,COUNT(distinct yt_id) as ycount ")
      sQuery.Append(" from Yacht with (NOLOCK) ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on yt_model_id = ym_model_id ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id ")
      sQuery.Append(" where yt_journ_id = 0  ")
      sQuery.Append(" and yr_contact_id in ( ")
      sQuery.Append(" select distinct contactid from Yacht_Crossover_Table with (NOLOCK)  ")
      sQuery.Append(" where contactgroup in ( ")
      sQuery.Append(" select distinct ContactGroup  ")
      sQuery.Append(" from Yacht_Crossover_Table with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('00','08','97') ")
      If amod_id > 0 Then
        sQuery.Append(" where amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" where amod_make_name = '" & Trim(make_name) & "' ")
      End If
      sQuery.Append(" )) ")
      sQuery.Append(" group by ym_brand_name ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_summary_yacht_class() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_summary_yacht_class() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_crossover_table_count(ByVal type_of As Integer, ByVal type_string As String, ByVal amod_id As Integer, ByVal make_name As String, ByVal yacht_size As String) As Integer

    get_yacht_crossover_table_count = 0

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()
    Dim count_rows As Integer = 0

    Try



      If Trim(type_of) = 1 Then
        '- TABLE 1 - YACHTS AND AIRCRAFT
        '-- GET INDIVIDUALS OWNING YACHTS AND AIRCRAFT COUNT  
        sQuery.Append(" SELECT count(distinct contactgroup) as tcount from Yacht_Crossover_Table with (NOLOCK) ")

        If amod_id > 0 Or Trim(make_name) <> "" Then
          sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = contact_id and cref_journ_id = 0  and cref_contact_type in ('00','08','97') ") 
        End If

        sQuery.Append(" where num_aircraft > 0 ")

        If amod_id > 0 Then
          sQuery.Append(" and amod_id = " & amod_id & "")
        ElseIf Trim(make_name) <> "" Then
          sQuery.Append(" and amod_make_name = '" & Trim(make_name) & "'")
        End If

      ElseIf Trim(type_of) = 2 Then
        '- GET COMPANIES OWNING YACHTS AND AIRCRAFT COUNT 
        sQuery.Append(" select COUNT(distinct cross_Yacht_CompId) as tcount from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" where cross_aircraft_count>0 ")
      ElseIf Trim(type_of) = 3 Then
        '- GET COMPANIES OWNING YACHTS AND AIRCRAFT COUNT   
        sQuery.Append("  select COUNT(distinct comp_id) as tcount ")
        sQuery.Append(" from company with (NOLOCK) ")
        sQuery.Append(" inner join Business_Type_Reference with (NOLOCK) on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id  ")
        sQuery.Append(" where comp_journ_id = 0 and bustypref_type='YY' ")

        If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
          sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
          sQuery.Append(" and (comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
          sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_commercial_flag = 'Y')")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
          sQuery.Append(" and comp_product_commercial_flag = 'Y'")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
          sQuery.Append(" and comp_product_helicopter_flag = 'Y'")
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
          sQuery.Append(" and comp_product_business_flag = 'Y'")
        End If

      ElseIf Trim(type_of) = 4 Then
        '-- COUNT THE NUMBER OF COMPANIES OWNING YACHTS NOT AIRCRAFT.
        sQuery.Append(" select COUNT(distinct cross_yacht_compid)  from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" where cross_aircraft_count = 0  ")
      ElseIf Trim(type_of) = 11 Then
        '-- GET INDIVIDUALS OWNING YACHTS AND AIRCRAFT COUNT  
        sQuery.Append(" SELECT count(distinct contactgroup) as tcount from Yacht_Crossover_Table with (NOLOCK) ")

        If Trim(yacht_size) <> "" Then
          sQuery.Append(" inner join View_Yacht_Company_Flat with (NOLOCK) on ContactId = contact_id and yr_journ_id = 0  and yr_contact_type in ('00','08','97')   ")
          sQuery.Append(" inner join Yacht_Category_Size with (NOLOCK) on ycs_category_size = ym_category_size ")
          If Trim(yacht_size) <> "" Then
            sQuery.Append("  and ycs_category_size = '" & Trim(yacht_size) & "' ")
          End If
        End If
        sQuery.Append(" where num_aircraft = 0 ")

        If Trim(type_string) = "P" Then
          sQuery.Append(" and num_prevaircraft > 0 ")
        End If

        Else
          sQuery.Append(" where num_aircraft > 0 ")
        End If

        SqlConn.ConnectionString = clientConnectString
        SqlConn.Open()
        SqlCommand.Connection = SqlConn

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_crossover_companies() As DataTable</b><br />" + sQuery.ToString

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 180

        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try

        If temptable.Rows.Count > 0 Then
          For Each r In temptable.Rows
            count_rows = r("tcount")
          Next
        End If

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return count_rows

  End Function
  Public Function get_latest_mfr_news(ByVal comp_id As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" SELECT top 10 * ")
      sQuery.Append(" FROM yacht_news WITH (NOLOCK)  ")
      sQuery.Append(" INNER JOIN Yacht_News_Source WITH (NOLOCK) ON ytnewssrc_id = ytnews_source_id   ")
      sQuery.Append(" INNER JOIN yacht WITH (NOLOCK) ON yt_id = ytnews_yt_id   ")
      sQuery.Append(" INNER JOIN yacht_model WITH (NOLOCK) ON ym_model_id = yt_model_id  ")
      sQuery.Append(" left outer join Company WITH (NOLOCK) ON ym_mfr_comp_id = comp_id and comp_journ_id = 0 ")
      sQuery.Append(" WHERE yt_journ_id = 0  ")
      sQuery.Append(" and ytnews_comp_id = " & comp_id & "")
      sQuery.Append(" ORDER BY ytnews_date desc ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_all_mfr() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_latest_mfr_news() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_all_mfr(ByVal order_by As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" SELECT DISTINCT comp_name,comp_city, comp_state, comp_country, comp_id, COUNT(*) AS tcount ")
      sQuery.Append(" FROM yacht WITH (NOLOCK)  ")
      sQuery.Append(" INNER JOIN yacht_model WITH (NOLOCK) ON ym_model_id = yt_model_id  ")
      sQuery.Append(" inner join Company WITH (NOLOCK) ON ym_mfr_comp_id = comp_id and comp_journ_id = 0 ")
      sQuery.Append(" WHERE yt_journ_id = 0  ")
      sQuery.Append(" GROUP BY comp_name,comp_city, comp_state, comp_country, comp_id ")

      If Trim(order_by) = "countasc" Then
        sQuery.Append(" ORDER BY COUNT(*) asc, comp_name asc,comp_city, comp_state, comp_country, comp_id  ")
      ElseIf Trim(order_by) = "countdesc" Then
        sQuery.Append(" ORDER BY COUNT(*) desc, comp_name asc,comp_city, comp_state, comp_country, comp_id  ")
      ElseIf Trim(order_by) = "compnameasc" Then
        sQuery.Append(" ORDER BY comp_name asc,comp_city, comp_state, comp_country, comp_id  ")
      ElseIf Trim(order_by) = "compnamedesc" Then
        sQuery.Append(" ORDER BY comp_name desc,comp_city, comp_state, comp_country, comp_id  ")
      Else
        sQuery.Append(" ORDER BY comp_name asc,comp_city, comp_state, comp_country, comp_id  ")
      End If



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_all_mfr() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_all_brands() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("select distinct ycs_motor_type, ycs_category_size, ycs_seqnbr from Yacht_Category_Size order by ycs_seqnbr asc")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_all_central_agents(ByVal temp_country As String, ByVal order_by As String, ByVal main_comp_id As Long, ByVal temp_brand_name As String, ByVal type_of As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      'sQuery.Append("SELECT DISTINCT comp_id, comp_name, comp_country, comp_city, comp_state, COUNT(yt_id) as tcount ")
      'sQuery.Append("From Company WITH(NOLOCK) ")
      'sQuery.Append("inner join Yacht_Reference with (NOLOCK) on comp_id = yr_comp_id  ")
      'sQuery.Append("and comp_journ_id = yr_journ_id  ")
      'sQuery.Append("inner join Yacht with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id  ")
      'sQuery.Append("LEFT OUTER JOIN State WITH(NOLOCK) on state_code = comp_state  ")
      'sQuery.Append("and state_country=comp_country  ")
      'sQuery.Append("WHERE comp_journ_id = 0 and comp_active_flag='Y'  ")
      'sQuery.Append("AND comp_hide_flag = 'N' AND upper(comp_name) <> 'AWAITING DOCUMENTATION' and (yr_contact_type in ('99','C1','C2','C3','C4','C5','C6') ) AND comp_active_flag = 'Y'  ")
      'sQuery.Append("AND ( comp_product_yacht_flag = 'Y')  ")


      'If Trim(temp_country) <> "" Then
      '  sQuery.Append("AND ( comp_country = '" & temp_country & "' ) ")
      'End If


      'sQuery.Append("group by comp_id, comp_name, comp_country, comp_city, comp_state ")
      'sQuery.Append("order by comp_name, comp_country, comp_city, comp_state ")

      If main_comp_id > 0 Or Trim(temp_country) <> "" Then
        sQuery.Append("select broker_main_comp_id, Company.comp_name, c3.comp_id, c3.comp_name as lower_comp_name, c3.comp_city, c3.comp_state, c3.comp_country, COUNT(yr_yt_id) as tcount ")
      Else
        sQuery.Append("select broker_main_comp_id, comp_name, count(distinct broker_comp_id) as sub_count, COUNT(yr_yt_id) as tcount ")
      End If

      'sQuery.Append(" from ReturnYachtCentralAgents()  ")
      sQuery.Append(" from Yacht_Central_Agent with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_main_comp_id = comp_id and comp_journ_id =0 ")
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on broker_comp_id = yr_comp_id and yr_journ_id = 0 ")

      If Trim(temp_brand_name) <> "" Then
        sQuery.Append(" inner join Yacht with (NOLOCK) on yr_yt_id = yt_id and yt_journ_id = 0 ")
        sQuery.Append(" inner join Yacht_Model with (NOLOCK) on ym_model_id = yt_model_id ")
      End If

      If main_comp_id > 0 Or Trim(temp_country) <> "" Then
        sQuery.Append(" inner join company c3  with (NOLOCK) on c3.comp_id = broker_comp_id and c3.comp_journ_id = 0 ")
      End If

      If Trim(type_of) = "" Then
        sQuery.Append(" where yr_contact_type in ('99','C1','C2','C3','C4','C5','C6') ")
      ElseIf Trim(type_of) = "FS" Then ' for sale
        sQuery.Append(" where yr_contact_type in ('C1','C2','C5','C6') ")  'c1 ca for sale, c2, jca for sale, c5, ca for s/c, c6 jca for s/c
      ElseIf Trim(type_of) = "FC" Then ' for charter
        sQuery.Append(" where yr_contact_type in ('C3','C4','C5','C6') ") 'c3 ca for c, c4 jca for c, c5, ca for s/c, c6 jca for s/c
      End If



      If Trim(temp_brand_name) <> "" Then
        sQuery.Append(" and ym_brand_name = '" & Trim(temp_brand_name) & "' ")
      End If

      If Trim(temp_country) <> "" Then
        '  If Trim(temp_country) = "Monaco" Then
        sQuery.Append("AND ( c3.comp_country like '%" & temp_country & "%' ) ")
        'Else
        '  sQuery.Append("AND ( comp_country = '" & temp_country & "' ) ")
        ' End If
      End If

      If main_comp_id > 0 Or Trim(temp_country) <> "" Then
        If main_comp_id > 0 Then
          sQuery.Append(" and broker_main_comp_id = " & main_comp_id & " ")
        End If
        sQuery.Append(" group by broker_main_comp_id, Company.comp_name, c3.comp_id, c3.comp_name, c3.comp_city, c3.comp_state, c3.comp_country ")
      Else
        sQuery.Append(" group by broker_main_comp_id, comp_name ")
      End If

      If main_comp_id > 0 Or Trim(temp_country) <> "" Then
        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(yr_yt_id) desc ")
        Else
          sQuery.Append(" order by c3.comp_name ")
        End If
      Else
        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(yr_yt_id) desc ")
        Else
          sQuery.Append(" order by Company.comp_name ")
        End If
      End If

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function


  Public Function get_Yacht_Owners_Not_Owning_AC(ByVal type_of As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

     
      If Trim(type_of) = "P" Then
        sQuery.Append(" select distinct comp_id, comp_name, comp_city, comp_state, comp_country, cross_yacht_count, cross_prevaircraft_count  ")
        sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" inner join Company with (NOLOCK) on cross_Yacht_Compid = comp_id and comp_journ_id = 0 ")
        sQuery.Append(" where cross_aircraft_count = 0 ")
        ' to make sure we only get the mains 
        sQuery.Append(" and cross_Yacht_Compid = cross_CompId ")

        '-- and none of our children have ac 
        sQuery.Append(" and not exists ( ")
        sQuery.Append(" select top 1 * from Yacht_Company_Crossover_Table y2 with (NOLOCK)  ")
        sQuery.Append(" where y2.cross_Yacht_CompId = Yacht_Company_Crossover_Table.cross_Yacht_Compid  ")
        sQuery.Append(" and (y2.cross_aircraft_count > 0) ")
        sQuery.Append(" ) ")


        '-- and we either have previous ac counts
        sQuery.Append(" and ((cross_aircraft_count = 0 and cross_prevaircraft_count > 0) ")
        sQuery.Append(" or ")
        '-- or one of us has previous ac counts
        sQuery.Append(" exists ( ")
        sQuery.Append(" select top 1 * from Yacht_Company_Crossover_Table y2 with (NOLOCK)  ")
        sQuery.Append("  where y2.cross_Yacht_CompId = Yacht_Company_Crossover_Table.cross_Yacht_Compid  ")
        sQuery.Append(" and (y2.cross_aircraft_count = 0 and y2.cross_prevaircraft_count > 0) ")
        sQuery.Append(" ) ")

        sQuery.Append(" ) ")
      Else
        sQuery.Append(" select distinct comp_id, comp_name, comp_city, comp_state, comp_country, cross_yacht_count, cross_prevaircraft_count  ")
        sQuery.Append(" from Yacht_Company_Crossover_Table with (NOLOCK) ")
        sQuery.Append(" inner join Company with (NOLOCK) on cross_Yacht_Compid = comp_id and comp_journ_id = 0 ")
        sQuery.Append(" where cross_aircraft_count = 0 ")

      End If

      sQuery.Append(" order by comp_name, comp_city, comp_state ")

      'sQuery.Append(" select * from View_Private_Owners_of_Jets_With_No_Yachts ")
      ' sQuery.Append(" order by comp_name, comp_city, comp_state, comp_country ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_Yacht_Owners_Not_Owning_AC() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_from_View_Private_Owners_of_Jets_With_No_Yachts(ByVal temp_country As String, ByVal type_of As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()
    Dim squery_items As String = ""
    Dim squery_from As String = ""
    Dim squery_where As String = ""
    Dim squery_order_by As String = ""

    Try


      'select distinct acot_name, count(comp_id)
      'from View_Aircraft_Company_Flat with (NOLOCK)
      'group by acot_name

      squery_items = " select distinct comp_name, contact_id, comp_city, comp_state, comp_zip_code, comp_country, comp_id "
      sQuery.Append(squery_items)
      squery_from = " from View_Aircraft_Owners_Without_Yachts with (NOLOCK) "
      sQuery.Append(squery_from)

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        squery_where &= "  where (comp_product_business_flag = 'Y' or comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')"
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        squery_where &= "  where (comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')"
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        squery_where &= "  where (comp_product_business_flag = 'Y' or comp_product_commercial_flag = 'Y')"
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        squery_where &= "  where comp_product_commercial_flag = 'Y'"
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        squery_where &= "  where comp_product_helicopter_flag = 'Y'"
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        squery_where &= "  where comp_product_business_flag = 'Y'"
      End If

      If Trim(type_of) = "" Then
        '   sQuery.Append(" and amod_airframe_type_code = 'F' ")
      ElseIf Trim(type_of) = "J" Then
        squery_where &= "  and View_Aircraft_Owners_Without_Yachts.amod_airframe_type_code = 'F' and View_Aircraft_Owners_Without_Yachts.amod_type_code in ('J','E') "
      ElseIf Trim(type_of) = "T" Then
        squery_where &= "  and View_Aircraft_Owners_Without_Yachts.amod_airframe_type_code = 'F' and View_Aircraft_Owners_Without_Yachts.amod_type_code in ('T') "
      ElseIf Trim(type_of) = "H" Then
        squery_where &= " and View_Aircraft_Owners_Without_Yachts.amod_airframe_type_code = 'R' "
      End If

      sQuery.Append(squery_where)

      squery_order_by = " order by comp_name "
      sQuery.Append(squery_order_by)


      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") = ""
      squery_items = Replace(squery_items, ", contact_id", "")
      squery_items = Replace(squery_items, ", comp_id", "")
      squery_items = Replace(squery_items, "comp_name", "comp_name as NAME")
      squery_items = Replace(squery_items, ", comp_address1", ", comp_address1 as Address")

      squery_items = Replace(squery_items, ", comp_state", ", comp_state as State")
      squery_items = Replace(squery_items, ", comp_country", ", comp_country as Country")
      squery_items = Replace(squery_items, ", comp_zip_code", ", comp_zip_code as ZipCode")

      squery_items = Replace(squery_items, ", comp_city", ", (select top 1 comp_address1 from company c2 with (NOLOCK) where c2.comp_id = View_Aircraft_Owners_Without_Yachts.comp_id and comp_journ_id = 0 ) as Address, comp_city as City ")


      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= squery_items

     
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= ", (select top 1 contact_email_address from contact with (NOLOCK) where contact.contact_id = View_Aircraft_Owners_Without_Yachts.contact_id and contact_journ_id = 0 ) as Contact_Email "
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= ", (select top 1 pnum_number_full from Phone_Numbers with (NOLOCK) where pnum_contact_id = contact_id and pnum_journ_id = 0 ) as Contact_Phone " 
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= ", (select top 1 pnum_number_full from Phone_Numbers with (NOLOCK) where pnum_comp_id = comp_id and pnum_journ_id = 0) as Comp_Phone "
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " , amod_make_name as Make, amod_model_name as Model, ac_ser_no_full as SerNo,"
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " case ac_ownership_type when 'W' then 'Whole Owner' when 'S' then 'Shared Owner' else 'Fractional' end as Ownership, "
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " case when cref_owner_percent=0 then 100 when cref_owner_percent > 0 then cref_owner_percent else 100 end as PercentOwned"

      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= squery_from

      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = 0 and cref_contact_type in  ('00','08','97')"
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " inner join Aircraft with (NOLOCK) on ac_id = cref_ac_id and ac_journ_id = cref_journ_id  "
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id "


      If Trim(type_of) = "" Then
        '   sQuery.Append(" and amod_airframe_type_code = 'F' ")
      ElseIf Trim(type_of) = "J" Then
        squery_where &= "  and Aircraft_Model.amod_airframe_type_code = 'F' and Aircraft_Model.amod_type_code in ('J','E') "
      ElseIf Trim(type_of) = "T" Then
        squery_where &= "  and Aircraft_Model.amod_airframe_type_code = 'F' and Aircraft_Model.amod_type_code in ('T') "
      ElseIf Trim(type_of) = "H" Then
        squery_where &= " and Aircraft_Model.amod_airframe_type_code = 'R' "
      End If


      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= squery_where
      ' HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= " and comp_id in (XXXCOMP_IDXXX) "
      HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select") &= squery_order_by

      'sQuery.Append(" select * from View_Private_Owners_of_Jets_With_No_Yachts ")
      ' sQuery.Append(" order by comp_name, comp_city, comp_state, comp_country ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn 

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_from_View_Private_Owners_of_Jets_With_No_Yachts() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_central_agents_no_ac(ByVal temp_country As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select comp_name, comp_country, comp_city, comp_state, comp_id, comp_product_business_flag,  ")
      sQuery.Append(" comp_product_helicopter_flag, comp_product_yacht_flag ")
      sQuery.Append(" from company ")
      sQuery.Append(" inner join Business_Type_Reference on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id ")
      sQuery.Append(" where comp_journ_id = 0  ")
      sQuery.Append(" and bustypref_type='YY' ")


      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_helicopter_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and (comp_product_business_flag = 'Y' or comp_product_commercial_flag = 'Y')")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        sQuery.Append(" and comp_product_commercial_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and comp_product_helicopter_flag = 'Y'")
      ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
        sQuery.Append(" and comp_product_business_flag = 'Y'")
      End If



      If Trim(temp_country) <> "" Then
        sQuery.Append("AND ( comp_country = '" & temp_country & "' ) ")
      End If

      sQuery.Append(" order by comp_name, comp_country ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_Individual_Owners_Without_Aircraft() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select * from View_Yacht_New_Individual_Owners_Without_Aircraft with (NOLOCK) order by ype_entered_date desc")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_Yacht_New_Company_Owners_Without_Aircraft() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("select * from View_Yacht_New_Company_Owners_Without_Aircraft with (NOLOCK) order by ype_entered_date desc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_all_central_agents_by_section(ByVal temp_country As String, ByVal order_by As String, ByVal field_to_group As String, ByVal main_comp_id As Long, ByVal temp_brand_name As String, ByVal is_for_graph As String, ByVal type_of As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If main_comp_id > 0 And Trim(field_to_group) = "comp_country" Then
        sQuery.Append("select distinct c3." & Trim(field_to_group) & ", COUNT(distinct c3.comp_id) as tcount ")
      ElseIf Trim(field_to_group) = "comp_country" Then
        sQuery.Append("select distinct c3.comp_country, COUNT(distinct c3.comp_id) as tcount ")
      ElseIf Trim(field_to_group) = "ym_brand_name" Then

        ' if we have a brand and a idfferent one, show locations
        If Trim(temp_brand_name) <> "" And (main_comp_id > 0 Or Trim(temp_country) <> "") And is_for_graph = "N" Then
          sQuery.Append("select distinct " & field_to_group & ", COUNT(distinct c3.comp_id) as tcount ")
        ElseIf Trim(temp_brand_name) <> "" And is_for_graph = "N" Then
          sQuery.Append("select distinct " & field_to_group & ", COUNT(distinct broker_main_comp_id) as tcount ")
        ElseIf main_comp_id > 0 Or Trim(temp_country) <> "" Then
          sQuery.Append("select distinct " & field_to_group & ", COUNT(distinct yr_yt_id) as tcount ")
        Else
          sQuery.Append("select distinct " & field_to_group & ", COUNT(distinct broker_main_comp_id) as tcount ")
        End If


      Else
        sQuery.Append("select distinct " & field_to_group & ", COUNT(distinct yr_yt_id) as tcount ")
      End If


      ' sQuery.Append(" from ReturnYachtCentralAgents()  ")
      sQuery.Append(" from Yacht_Central_Agent with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_main_comp_id = comp_id and comp_journ_id = 0 ")
      If main_comp_id > 0 Or Trim(field_to_group) = "comp_country" Or Trim(temp_country) <> "" Then
        sQuery.Append(" inner join company c3  with (NOLOCK) on c3.comp_id = broker_comp_id and c3.comp_journ_id= 0 ")
      End If
      sQuery.Append(" inner join Yacht_Reference with (NOLOCK) on broker_comp_id = yr_comp_id and yr_journ_id = 0 ")
      sQuery.Append(" inner join Yacht with (NOLOCK) on yr_yt_id = yt_id and yt_journ_id = 0 ")
      sQuery.Append(" inner join Yacht_Model with (NOLOCK) on ym_model_id = yt_model_id ")
      sQuery.Append(" inner join Yacht_Category_Size with (NOLOCK) on ycs_category_size = ym_category_size and ycs_motor_type = 'M' ")
   

      If Trim(type_of) = "" Then
        sQuery.Append(" where yr_contact_type in ('99','C1','C2','C3','C4','C5','C6') ")
      ElseIf Trim(type_of) = "FS" Then ' for sale
        sQuery.Append(" where yr_contact_type in ('C1','C2','C5','C6') ")  'c1 ca for sale, c2, jca for sale, c5, ca for s/c, c6 jca for s/c
      ElseIf Trim(type_of) = "FC" Then ' for charter
        sQuery.Append(" where yr_contact_type in ('C3','C4','C5','C6') ") 'c3 ca for c, c4 jca for c, c5, ca for s/c, c6 jca for s/c
      End If



      If Trim(temp_brand_name) <> "" Then
        sQuery.Append(" and ym_brand_name = '" & Trim(temp_brand_name) & "' ")
      End If

      If main_comp_id > 0 Then
        sQuery.Append(" and broker_main_comp_id = " & main_comp_id & " ")
      End If

      If Trim(temp_country) <> "" Then
        sQuery.Append("AND ( c3.comp_country like '%" & temp_country & "%' ) ")
      End If



      If main_comp_id > 0 And Trim(field_to_group) = "comp_country" Then

        sQuery.Append(" group by c3." & Trim(field_to_group) & " ")
        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(distinct c3.comp_id) desc ")
        Else
          sQuery.Append(" order by c3." & Trim(field_to_group) & " asc ")
        End If
      ElseIf Trim(field_to_group) = "comp_country" Then
        sQuery.Append(" group by c3.comp_country ")
        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(distinct c3.comp_id) desc ")
        Else
          sQuery.Append(" order by c3.comp_country asc ")
        End If
      ElseIf Trim(field_to_group) = "ym_brand_name" Then

        sQuery.Append(" group by " & field_to_group & " ")

        If Trim(temp_brand_name) <> "" And (main_comp_id > 0 Or Trim(temp_country) <> "") And is_for_graph = "N" Then
          If Trim(order_by) = "count" Then
            sQuery.Append(" order by COUNT(distinct c3.comp_id) desc ")
          Else
            sQuery.Append(" order by " & field_to_group & " asc ")
          End If
        ElseIf Trim(temp_brand_name) <> "" And is_for_graph = "N" Then
          If Trim(order_by) = "count" Then
            sQuery.Append(" order by COUNT(distinct broker_main_comp_id) desc ")
          Else
            sQuery.Append(" order by " & field_to_group & " asc ")
          End If
        ElseIf main_comp_id > 0 Or Trim(temp_country) <> "" Then
          If Trim(order_by) = "count" Then
            sQuery.Append(" order by COUNT(distinct yr_yt_id) desc ")
          Else
            sQuery.Append(" order by " & field_to_group & " asc ")
          End If
        Else
          If Trim(order_by) = "count" Then
            sQuery.Append(" order by COUNT(distinct broker_main_comp_id) desc ")
          Else
            sQuery.Append(" order by " & field_to_group & " asc ")
          End If
        End If





        'if its multiple
      ElseIf InStr(Trim(field_to_group), "ym_brand_name") > 0 Then
        sQuery.Append(" group by " & field_to_group & " ")

        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(distinct yr_yt_id) desc ")
        Else
          sQuery.Append(" order by " & field_to_group & " asc ")
        End If

      Else

        sQuery.Append(" group by " & field_to_group & " ")
        If Trim(order_by) = "count" Then
          sQuery.Append(" order by COUNT(distinct yr_yt_id) desc ")
        Else
          sQuery.Append(" order by " & field_to_group & " asc ")
        End If
      End If


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, "Selection Over")


    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_all_central_agents_top_25(ByVal temp_Country As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT TOP 25  comp_id, comp_name, comp_country, comp_city, comp_state, COUNT(yt_id) as tcount ")
      sQuery.Append("From Company WITH(NOLOCK) ")
      sQuery.Append("inner join Yacht_Reference with (NOLOCK) on comp_id = yr_comp_id  ")
      sQuery.Append("and comp_journ_id = yr_journ_id  ")
      sQuery.Append("inner join Yacht with (NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id  ")
      sQuery.Append("LEFT OUTER JOIN State WITH(NOLOCK) on state_code = comp_state  ")
      sQuery.Append("and state_country=comp_country  ")
      sQuery.Append("WHERE comp_journ_id = 0 and comp_active_flag='Y'  ")
      sQuery.Append("AND comp_hide_flag = 'N' AND upper(comp_name) <> 'AWAITING DOCUMENTATION' and (yr_contact_type in ('99','C1','C2','C3','C4','C5','C6') ) AND comp_active_flag = 'Y' ")
      sQuery.Append("AND ( comp_product_yacht_flag = 'Y')  ")

      If Trim(temp_Country) <> "" Then
        sQuery.Append("AND ( comp_country = '" & temp_Country & "' ) ")
      End If

      sQuery.Append("group by comp_id, comp_name, comp_country, comp_city, comp_state ")
      sQuery.Append("order by COUNT(yt_id) desc ")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_yacht_fleet_summary(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT yl_lifecycle_name, yt_lifecycle_id, count(*) AS tcount")
      sQuery.Append(" FROM yacht WITH (NOLOCK) INNER JOIN Yacht_Model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" INNER JOIN Yacht_Lifecycle WITH (NOLOCK) ON yt_lifecycle_id = yl_lifecyle_id")
      sQuery.Append(" WHERE yt_journ_id = 0")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY yl_lifecycle_name, yt_lifecycle_id")
      sQuery.Append(" ORDER BY yt_lifecycle_id")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_fleet_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_fleet_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_year_summary(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT yt_year_mfr, count(*) AS tcount")
      sQuery.Append(" FROM yacht WITH (NOLOCK) INNER JOIN Yacht_Model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" WHERE yt_journ_id = 0")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY yt_year_mfr ORDER BY yt_year_mfr")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_year_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in get_yacht_year_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yacht_brand_summary(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT ym_brand_name, COUNT(*) AS tcount")
      sQuery.Append(" FROM yacht WITH (NOLOCK) INNER JOIN yacht_model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" WHERE yt_journ_id = 0")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY ym_brand_name ORDER BY ym_brand_name")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yacht_brand_summary() As DataTable</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing
      aError = "Error in get_yacht_brand_summary() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_yachts_by_type(ByRef searchCriteria As yachtViewSelectionCriteria, ByVal hullType As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("SELECT DISTINCT ymt_description, ym_motor_type, ycs_seqnbr, ycs_description, ym_category_size, COUNT(*) AS tcount,")

      sQuery.Append(" (SELECT COUNT(*) FROM Yacht WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Yacht_Model WITH (NOLOCK) ON yt_model_id = ym_model_id")
      sQuery.Append(" WHERE ym_motor_type = m.ym_motor_type AND ym_category_size = m.ym_category_size AND yt_forsale_flag='Y' and yt_journ_id = 0) AS yforsale")

      sQuery.Append(",(SELECT COUNT(*) FROM Yacht WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Yacht_Model WITH (NOLOCK) ON yt_model_id = ym_model_id")
      sQuery.Append(" WHERE ym_motor_type = m.ym_motor_type AND ym_category_size = m.ym_category_size AND yt_for_charter_flag='Y' and yt_journ_id = 0) AS yforcharter")

      sQuery.Append(" FROM yacht INNER JOIN Yacht_Model m WITH (NOLOCK) ON yt_model_id = ym_model_id")
      sQuery.Append(" INNER JOIN Yacht_Category_size WITH (NOLOCK) ON ym_category_size = ycs_category_size AND ym_motor_type = ycs_motor_type")
      sQuery.Append(" INNER JOIN Yacht_Motor_Type WITH (NOLOCK) ON ymt_motor_type = ym_motor_type")
      sQuery.Append(" WHERE yt_journ_id = 0")
      sQuery.Append(" AND ym_motor_type = '" + hullType + "'")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY ymt_description, ym_motor_type, ycs_seqnbr, ycs_description, ym_category_size")
      sQuery.Append(" ORDER BY ymt_description, ym_motor_type, ycs_seqnbr, ycs_description, ym_category_size")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_yachts_by_type(ByVal sMotorType As String) As DataTable:</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing
      aError = "Error in get_yachts_by_type(ByVal sMotorType As String) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

#End Region

#Region "yacht_view_17_functions"

  Public Function get_yacht_naval_architects_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT comp_name, comp_city, comp_state, comp_country, comp_id, COUNT(distinct yr_yt_id) AS tcount")
      sQuery.Append(" FROM Yacht WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Yacht_Model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" INNER JOIN Yacht_Reference WITH (NOLOCK) ON yr_yt_id = yt_id AND yr_journ_id = yt_journ_id")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON yr_comp_id = comp_id AND YR_JOURN_ID = comp_journ_id")
      sQuery.Append(" WHERE yt_journ_id = 0 AND comp_active_flag = 'Y'")
      sQuery.Append(" AND yr_contact_type IN ('Y4','YA')")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY comp_name, comp_city, comp_state, comp_country, comp_id")
      sQuery.Append(" ORDER BY COUNT(distinct yr_yt_id) desc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_naval_architects_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_yacht_naval_architects_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_yacht_naval_architects_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable " + ex.Message

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

  Public Function get_yacht_interior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT comp_name, comp_city, comp_state, comp_country, comp_id, COUNT(distinct yr_yt_id) AS tcount")
      sQuery.Append(" FROM Yacht WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Yacht_Model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" INNER JOIN Yacht_Reference WITH (NOLOCK) ON yr_yt_id = yt_id AND yr_journ_id = yt_journ_id")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON yr_comp_id = comp_id AND YR_JOURN_ID = comp_journ_id")
      sQuery.Append(" WHERE yt_journ_id = 0 AND comp_active_flag = 'Y'")
      sQuery.Append(" AND yr_contact_type IN ('Y2','Y0')")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY comp_name, comp_city, comp_state, comp_country, comp_id")
      sQuery.Append(" ORDER BY COUNT(distinct yr_yt_id) desc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_interior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_yacht_interior_designers_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_yacht_interior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable " + ex.Message

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

  Public Function get_yacht_exterior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT comp_name, comp_city, comp_state, comp_country, comp_id, COUNT(distinct yr_yt_id) AS tcount")
      sQuery.Append(" FROM Yacht WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Yacht_Model WITH (NOLOCK) ON ym_model_id = yt_model_id")
      sQuery.Append(" INNER JOIN Yacht_Reference WITH (NOLOCK) ON yr_yt_id = yt_id AND yr_journ_id = yt_journ_id")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON yr_comp_id = comp_id AND YR_JOURN_ID = comp_journ_id")
      sQuery.Append(" WHERE yt_journ_id = 0 AND comp_active_flag = 'Y'")
      sQuery.Append(" AND yr_contact_type IN ('Y3','Y9')")

      If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
          If String.IsNullOrEmpty(tmpStr.Trim) Then
            tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
          Else
            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
          End If
        Next

        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "yt_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name IN ('" + searchCriteria.YachtViewCriteriaYachtBrand.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_brand_name = '" + searchCriteria.YachtViewCriteriaYachtBrand.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtCategory.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ym_category_size = '" + searchCriteria.YachtViewCriteriaYachtCategory.Trim + "'")
      End If

      sQuery.Append(" GROUP BY comp_name, comp_city, comp_state, comp_country, comp_id")
      sQuery.Append(" ORDER BY COUNT(distinct yr_yt_id) desc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_yacht_exterior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_yacht_exterior_designers_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_yacht_exterior_designers_info(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable " + ex.Message

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

  Public Function display_two_column_view17(ByVal inTable As DataTable, ByVal ColumnOneString As String, ByVal ColumnOneName As String, ByVal ColumnTwoString As String, ByVal ColumnTwoName As String, ByVal DisplayTotal As Boolean) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim ColumnTotal As Long = 0

    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>" + ColumnOneString.Trim + "</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b>" + ColumnTwoString.Trim + "</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
          If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
            HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("comp_id").ToString
          Else
            HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("comp_id").ToString
          End If

          htmlOut.Append("<td align='left' valign='top'><a class='underline' target='_blank' href='DisplayCompanyDetail.aspx?compid=" + Row.Item("comp_id").ToString + "' title='Display Company Details'>" + Row.Item(ColumnOneName).ToString + "</a></td>")
          htmlOut.Append("<td align='right' valign='top'>" + Row.Item(ColumnTwoName).ToString + "</a></td>")
          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item(ColumnTwoName).ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function

#End Region

#Region "yacht_view_20_functions"

  Public Function display_two_column_view20_yacht(ByVal inTable As DataTable, ByVal ColumnOneString As String, ByVal ColumnOneName As String, ByVal ColumnTwoString As String, ByVal ColumnTwoName As String, ByVal DisplayTotal As Boolean, ByVal RequestVariableName As String, ByVal box_combo As String, ByVal size_combo As String, ByVal ColumnThreeName As String) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim seperated_sections As String = ""
    Dim cat_array(8) As String
    Dim i As Integer = 0

    Dim ColumnTotal As Long = 0



    cat_array = Split(size_combo, "##")




    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>Brand</b></td>")
        htmlOut.Append("<td align='left' valign='top'><b>Model</b></td>")
        htmlOut.Append("<td align='left' valign='top'><b>Name</b></td>")
        htmlOut.Append("<td align='left' valign='top'><b>Hull ID Nbr.</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b>Mfr. Year</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          ' seperated_sections = ""
          ' For i = 0 To 6
          '     seperated_sections = seperated_sections & Row.Item(ColumnOneName).ToString() & "|" & cat_array(i) & "##"
          ' Next
          ' seperated_sections = Left(seperated_sections, Len(seperated_sections) - 2)

          '  htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('cboYachtCategoryID','" & box_combo & "','cboYachtBrandID', '" & seperated_sections & "');""", "") + ">" + Row.Item(ColumnOneName).ToString + "</a></td>")
          '  htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('cboYachtCategoryID','" & box_combo & "','cboYachtBrandID','" & seperated_sections & "');""", "") + ">" + Row.Item(ColumnTwoName).ToString + "</a></td>")


          ' htmlOut.Append(Display_Yacht_Information_For_Link(Row.Item(ColumnThreeName), Row.Item(ColumnOneName), Row.Item(ColumnTwoName)))

          htmlOut.Append("<td align='left'>" & Row.Item("ym_brand_name") & "</td>")
          htmlOut.Append("<td align='left'>" & Row.Item("ym_model_name") & "</td>")
          htmlOut.Append("<td align='left'><a " & DisplayFunctions.WriteYachtDetailsLink(Row.Item("yt_id"), False, "", "", "") & ">" & Row.Item("yt_yacht_name") & "</a></td>")
          htmlOut.Append("<td align='left'>" & Row.Item("yt_hull_mfr_nbr") & "</td>")
          htmlOut.Append("<td align='right'>" & Row.Item("yt_year_mfr") & "</td>")

          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item(ColumnTwoName).ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function
  Public Shared Function Display_Yacht_Information_For_Link(ByVal yt_id As Long, ByVal yt_name As String, ByVal yt_year_mfr As String)
    Dim ReturnString As String = ""

    ReturnString = "<td align='left'><a " & DisplayFunctions.WriteYachtDetailsLink(yt_id, False, "", "", "") & ">" & yt_name & "</a></td>"
    'Hull # " & atempTable.Rows(0).Item("yt_hull_mfr_nbr").ToString

    '  ReturnString += "<td align='right'><a " & DisplayFunctions.WriteYachtDetailsLink(yt_id, False, "", "", "") & ">" & yt_year_mfr & "</a></td>"

    ReturnString += "<td align='right'>" & yt_year_mfr & "</td>"


    Return ReturnString
  End Function

  Public Function get_comp_id_by_name(ByVal comp_id As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery = "Select company.*, "
      sQuery = sQuery & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
      sQuery = sQuery & " and pnum_type='Office') as comp_phone_office,"
      sQuery = sQuery & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
      sQuery = sQuery & " and pnum_type='Fax') as comp_phone_fax"
      sQuery = sQuery & " from company "
      sQuery = sQuery & " where comp_journ_id = 0 and comp_id = " & comp_id.ToString & " "

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_comp_id_by_name As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_ac_based_on_location load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_ac_based_on_location(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Function get_company_name_by_id(ByVal comp_id As Long, ByRef this_company_name As String) As String
    get_company_name_by_id = ""

    Dim CompanyLocation As String = ""
    Dim CompanyTitle As String = ""
    Dim results_table As New DataTable

    CompanyLocation = ""
    CompanyTitle = ""

    results_table = get_comp_id_by_name(comp_id)

    If Not IsNothing(results_table) Then
      If results_table.Rows.Count > 0 Then



        For Each r As DataRow In results_table.Rows

          this_company_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name") & "<Br>", "<Br>")

          '  CompanyTitle = IIf(Not IsDBNull(r("comp_name")), r("comp_name") & "<Br>", "<Br>")
          CompanyTitle += IIf(Not IsDBNull(r("comp_address1")), r("comp_address1") & " ", "")
          CompanyTitle += IIf(Not IsDBNull(r("comp_address2")), r("comp_address2") & "<Br>", "<Br>")
          CompanyLocation += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
          CompanyLocation += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")
          CompanyLocation += IIf(Not IsDBNull(r("comp_country")), r("comp_country") & " ", " ")

          CompanyLocation = Replace(CompanyLocation, "United States", "U.S.")
          CompanyTitle += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
          CompanyTitle += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")

          CompanyTitle += IIf(Not IsDBNull(r("comp_country")), r("comp_country") & "<Br>" & "<Br>", "<Br>" & "<Br>")
          CompanyTitle += IIf(Not IsDBNull(r("comp_phone_office")), "Office: " & r("comp_phone_office") & "<Br>", "")
          CompanyTitle += IIf(Not IsDBNull(r("comp_phone_fax")), "Fax: " & r("comp_phone_fax") & "<Br>" & "<Br>", "<Br>" & "<Br>")
          CompanyTitle += IIf(Not IsDBNull(r("comp_email_address")), "Email: " & r("comp_email_address") & "<Br>", "")
          CompanyTitle += IIf(Not IsDBNull(r("comp_web_address")), "Website: " & r("comp_web_address") & "<Br>", "")



          If Not IsDBNull(r("comp_name")) Then
            get_company_name_by_id += "<span> " & DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", "") & "<br><span class='tiny'>" & CompanyTitle & "</span>"
          End If

        Next

      Else
        get_company_name_by_id += ""
      End If
    Else
      get_company_name_by_id += ""
    End If







  End Function

  Public Function display_two_column_view20(ByVal inTable As DataTable, ByVal ColumnOneString As String, ByVal ColumnOneName As String, ByVal ColumnTwoString As String, ByVal ColumnTwoName As String, ByVal DisplayTotal As Boolean, ByVal RequestVariableName As String, ByVal view_link As String, ByVal temp_height As Integer, ByVal use_search_click As Boolean) As String

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim ColumnTotal As Long = 0

    If Not IsNothing(inTable) Then
      If inTable.Rows.Count > 0 Then

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")
        End If

        htmlOut.Append("<table width='97%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>" + ColumnOneString.Trim + "</b></td>")
        htmlOut.Append("<td align='right' valign='top'><b>" + ColumnTwoString.Trim + "</b></td>")
        htmlOut.Append("</tr>")

        For Each Row As DataRow In inTable.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          If Trim(ColumnOneName) = "comp_name" Then
            HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
            If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
              HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item(RequestVariableName).ToString
            Else
              HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item(RequestVariableName).ToString
            End If
          End If

          If use_search_click Then
            htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(RequestVariableName).ToString + "','','');""", "") + ">" + Row.Item(ColumnOneName).ToString + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='#' " + IIf(Not String.IsNullOrEmpty(RequestVariableName), "onclick=""javascript:PerformYachtSearch('" + RequestVariableName + "','" + Row.Item(RequestVariableName).ToString + "','','');""", "") + ">" + Row.Item(ColumnTwoName).ToString + "</a></td>")
          ElseIf Trim(view_link) <> "" Then
            htmlOut.Append("<td align='left' valign='top'><a class=""underline"" href='" & view_link & "&comp_id=" & Row.Item(RequestVariableName).ToString & "'>" + Row.Item(ColumnOneName).ToString + "</a></td>")
            htmlOut.Append("<td align='right' valign='top'><a class=""underline"" href='" & view_link & "&comp_id=" & Row.Item(RequestVariableName).ToString & "'>" + Row.Item(ColumnTwoName).ToString + "</a></td>")
          Else
            htmlOut.Append("<td align='left' valign='top'>" + Row.Item(ColumnOneName).ToString + "</td>")
            htmlOut.Append("<td align='right' valign='top'>" + Row.Item(ColumnTwoName).ToString + "</td>")
          End If


          htmlOut.Append("</tr>")

          If DisplayTotal Then
            ColumnTotal += CInt(Row.Item(ColumnTwoName).ToString)
          End If

        Next

        If DisplayTotal Then
          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If
          htmlOut.Append("<td align='left' valign='top' class='blue_text'>Total:</td>")
          htmlOut.Append("<td align='right' valign='top' class='blue_text'><strong>" + ColumnTotal.ToString + "</strong></td>")
          htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        If inTable.Rows.Count > 15 Then
          htmlOut.Append("</div>")
        End If

      Else

        htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
        htmlOut.Append("<tr class='header_row'>")
        htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
        htmlOut.Append("</tr></table>")

      End If

    Else

      htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left' valign='top'><b>No yachts to display</b></td>")
      htmlOut.Append("</tr></table>")

    End If

    Return htmlOut.ToString

  End Function

#End Region


#Region "yacht_view_21_functions"
  Public Function Display_Crossover_Formatted_Table(ByRef DisplayTable As DataTable, ByVal tableWidth As String, ByVal tableHeader As String, ByVal tableIcon As String, ByVal secondTableIcon As String, ByVal DisplayItemField As String, ByVal DisplayCountField As String, ByVal DisplayTabField As String, ByVal DisplayTypeOFField As String, ByVal strikeThrough As Boolean) As String
    Dim ResultsString As String = ""

    If Not IsNothing(DisplayTable) Then
      If DisplayTable.Rows.Count > 0 Then
        ResultsString = "<table width='318' class=""crossoverTable"""
        If tableWidth <> "" Then
          ResultsString &= " width=""" & tableWidth & """"
        End If
        ResultsString &= " cellpadding=""3"" cellspacing=""0"">"
        ResultsString &= "<tr class=""crossoverHeaderRow"">"

        ResultsString &= "<td align=""right"" valign=""top"" width=""40"">"
        If tableIcon <> "" Then
          ResultsString &= "<img src=""" & tableIcon & """ alt="""" />"
        End If
        ResultsString &= "</td>"

        ResultsString &= "<td align=""center"" valign=""top"">"
        ResultsString &= tableHeader
        ResultsString &= "</td>"

        ResultsString &= "<td align=""left"" valign=""top"" width=""40"" >"
        If secondTableIcon <> "" Then
          If strikeThrough Then
            ResultsString &= " <div class=""strikethrough"">"
          End If

          ResultsString &= "<img src=""" & secondTableIcon & """ alt=""""  />"

          If strikeThrough Then
            ResultsString &= " </div>"
          End If
        End If
        ResultsString &= "</td>"

        ResultsString &= "</tr>"
        ResultsString &= "<tr>"
        ResultsString &= "<td align=""left"" valign=""top"" colspan=""3"" >"
        ResultsString &= "<ul>"

        For Each r As DataRow In DisplayTable.Rows
          If Not IsDBNull(r(DisplayItemField)) Then

            If InStr(r(DisplayItemField), "&nbsp;&nbsp;&nbsp;") > 0 Then
              ResultsString &= "<table cellpadding='0' cellspacing='0' border='0'><tr><td>&nbsp;&nbsp;&nbsp;</td><td><ul>"
            End If

            ResultsString &= "<li>"
            'ResultsString &= "<a href=""#"">" 
            ResultsString &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=" & r(DisplayTabField) & "&type_of=" & r(DisplayTypeOFField) & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
            ResultsString &= Replace(r(DisplayItemField), "&nbsp;&nbsp;&nbsp;", "")
            ' If Not IsDBNull(r(DisplayCountField)) Then
            '   ResultsString &= " (" & r(DisplayCountField) & ")"
            'End If
            ResultsString &= "</a></li>"

            If InStr(r(DisplayItemField), "&nbsp;&nbsp;&nbsp;") > 0 Then
              ResultsString &= "</ul></td></tr></table>"
            End If

          End If
        Next

        ResultsString &= "</ul>"
        ResultsString &= "</td>"
        ResultsString &= "</tr>"
      End If
    End If
    ResultsString &= "</table>"

    Return ResultsString
  End Function
#End Region

End Class
