Public Class YachtFunctions
    'A Set of reusable functions for yachts.
  Public Shared Function DisplaySummaryByYachtType(ByVal YachtTable As DataTable) As String

    Dim OutputTotal As Long = 0
    Dim OutputSaleTotal As Long = 0
    Dim OutputString As String = ""
    Dim OutputCharterTotal As Integer = 0

    DisplaySummaryByYachtType = ""
    If Not IsNothing(YachtTable) Then
      If YachtTable.Rows.Count > 0 Then
        OutputString += "<table width='100%' cellpadding='2' cellspacing='0' class='data_aircraft_grid'>"
        OutputString += "<tr class='header_row'>"
        OutputString += "<td align='left' valign='top'><b>Yacht Size</b></td>"
        OutputString += "<td align='right' valign='top'><b># of Yachts</b></td>"
        OutputString += "<td align='right' valign='top'><b># For Sale</b></td>"
        OutputString += "<td align='right' valign='top'><b># For Charter</b></td>"

        OutputString += "</tr>"
        For Each Row As DataRow In YachtTable.Rows
          OutputString += "<tr>"
          OutputString += "<td align='left' valign='top' nowrap='nowrap'><a href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" & Row("ym_category_size").ToString & "|" & Row("ym_motor_type").ToString & "','','');"">" & Row("ycs_description") & "</a></td>"
          OutputString += "<td align='right' valign='top'><a href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" & Row("ym_category_size").ToString & "|" & Row("ym_motor_type").ToString & "','','');"">" & Row("tcount") & "</a></td>"
          OutputString += "<td align='right' valign='top'><a href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" & Row("ym_category_size").ToString & "|" & Row("ym_motor_type").ToString & "','for_sale','true');"">" & Row("yforsale") & "</a></td>"
          OutputString += "<td align='right' valign='top'><a href='#' onclick=""javascript:PerformYachtSearch('ym_category_size','" & Row("ym_category_size").ToString & "|" & Row("ym_motor_type").ToString & "','for_charter','true');"">" & Row("yforcharter") & "</a></td>"
          OutputString += "</tr>"
          OutputSaleTotal += Row("yforsale")
          OutputTotal += Row("tcount")
          OutputCharterTotal += Row("yforcharter").ToString
        Next
        OutputString += "<tr class='alt_row'>"
        OutputString += "<td align='left' valign='top' class='blue_text'>Total:</td>"
        OutputString += "<td align='right' valign='top' class='blue_text'>" & OutputTotal & "</td>"
        OutputString += "<td align='right' valign='top' class='blue_text'>" & OutputSaleTotal & "</td>"
        OutputString += "<td align='right' valign='top' class='blue_text'>" & OutputCharterTotal & "</td>"
        OutputString += "</tr>"
        OutputString += "</table>"
      End If
    End If
    YachtTable.Dispose()
    Return OutputString

  End Function
    ''' <summary>
    ''' Displays Yacht Brand in Listbox
    ''' </summary>
    ''' <param name="master"></param>
    ''' <param name="lb"></param>
    ''' <remarks></remarks>
    Public Shared Sub Display_Yacht_Brand_In_Listbox(ByVal master As Object, ByVal lb As ListBox)
        Dim YachtBrand As New DataTable
        lb.Items.Clear()
        lb.Items.Add(New ListItem("ALL", ""))
        If Not IsNothing(master) Then
            YachtBrand = master.aclsdata_temp.ListOfYachtBrands()
            If Not IsNothing(YachtBrand) Then
                If YachtBrand.Rows.Count > 0 Then
                    For Each r As DataRow In YachtBrand.Rows
                        lb.Items.Add(New ListItem(r("ym_brand_name").ToString, r("ym_brand_name").ToString))
                    Next
                End If
            End If
        End If
        YachtBrand.Dispose()
    End Sub
    ''' <summary>
    ''' Yacht Category displayed in dropdown
    ''' </summary>
    ''' <param name="master"></param>
    ''' <param name="lb"></param>
    ''' <remarks></remarks>
    Public Shared Sub Display_Yacht_Category_In_Dropdown(ByVal master As Object, ByVal lb As DropDownList)
        Dim YachtCategory As New DataTable
        lb.Items.Clear()
        lb.Items.Add(New ListItem("ALL", ""))
        If Not IsNothing(master) Then
            YachtCategory = master.aclsdata_temp.ListOfYachtCategorySize()
            If Not IsNothing(YachtCategory) Then
                If YachtCategory.Rows.Count > 0 Then
                    For Each r As DataRow In YachtCategory.Rows
                        lb.Items.Add(New ListItem(r("ycs_description").ToString & " (" & r("ymt_description").ToString & ")", r("ym_category_size").ToString & "|" & r("ym_motor_type").ToString))
                    Next
                End If
            End If
        End If
        YachtCategory.Dispose()
    End Sub

  Public Shared Function GetYachtCharterLocations() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("select * from Yacht_Charter_Locations with (NOLOCK) order by ycloc_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetYachtCharterLocations() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtCharterLocations() As DataTable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtCharterLocations() As DataTable" + ex.Message

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

  Public Shared Function GetYachtComplianceTypes() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("select yct_type, yct_type_description, yct_id from Yacht_Compliance_Types with (NOLOCK) order by yct_type_description")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetYachtComplianceTypes() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtComplianceTypes() As DataTable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtComplianceTypes() As DataTable" + ex.Message

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
  Public Shared Function GetYachtEngineManufacturers() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("select distinct comp_name, comp_id from Yacht_Engine_Models with (NOLOCK) inner join Company with (NOLOCK) on yem_engine_mfr_comp_id = comp_id and comp_journ_id = 0 order by comp_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetYachtEngineManufacturers() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtEngineManufacturers() As DataTable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtEngineManufacturers() As DataTable" + ex.Message

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

  Public Shared Function GetEngineModelFromManufacturer(ByVal CompanyIDString As String) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try



      sql = "select distinct yem_engine_model, yem_engine_model_id "
      sql += " from Yacht_Engine_Models with (NOLOCK)"
      sql += " inner join Company with (NOLOCK) on yem_engine_mfr_comp_id = comp_id and comp_journ_id = 0"
      sql += " where comp_id in (" & CompanyIDString & ")"
      sql += " and yem_engine_model <> ''"
      sql += " order by yem_engine_model"


      'save to session query debug string.
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetEngineModelFromManufacturer(ByVal CompanyIDString As String) As DataTable</b><br />" & sql

      'Opening Connection
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn

      SqlCommand.CommandText = sql
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandType = CommandType.Text
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try




      Return atemptable

    Catch ex As Exception
      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetEngineModelFromManufacturer(ByVal CompanyIDString As String) As DataTable As DataTable: " + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

  Public Shared Function GetYachtBrandQuickSearch() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT ym_model_id, ycs_description, ym_motor_type, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_name, ycs_seqnbr")
      sQuery.Append(" FROM Yacht_Model INNER JOIN Yacht_Category_Size WITH (NOLOCK) ON ym_category_size = ycs_category_size AND ym_motor_type = ycs_motor_type")
      sQuery.Append(" WHERE ym_brand_name <> 'JETNET'")
      sQuery.Append(" GROUP BY ycs_seqnbr, ym_motor_type,ycs_description, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_id, ym_model_name")
      sQuery.Append(" ORDER BY ycs_seqnbr, ym_motor_type, ycs_description, ym_category_size, ym_brand_name, ym_brand_abbrev, ym_model_id, ym_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetYachtBrandQuickSearch() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtBrandQuickSearch() As DataTable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetYachtBrandQuickSearch() As DataTable" + ex.Message

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

  Public Shared Function DisplayYachtConfidentialNotes(ByVal yt_charter_flag As Object, ByVal yachtID As Long, ByVal cssClass As String, ByVal ReturnOnlyText As Boolean, ByVal yt_forSaleFlag As Object) As String
    Dim returnString As String = ""
    Dim ReturnTable As New DataTable

    Dim charterFlag As String = ""
    Dim forsaleFlag As String = ""

    If Not IsDBNull(yt_charter_flag) Then
      If Not String.IsNullOrEmpty(yt_charter_flag) Then
        charterFlag = yt_charter_flag
      End If
    End If
    If Not IsDBNull(yt_forSaleFlag) Then
      If Not String.IsNullOrEmpty(yt_forSaleFlag) Then
        forsaleFlag = yt_forSaleFlag
      End If
    End If


    If charterFlag.ToString = "Y" Or forsaleFlag.ToString = "Y" Then
      If yachtID > 0 Then
        ReturnTable = GetYachtConfidentialNotes(yachtID)
        If Not IsNothing(ReturnTable) Then
          If ReturnTable.Rows.Count > 0 Then
            If Not IsDBNull(ReturnTable.Rows(0).Item("yt_confidential_notes")) Then
              If Not String.IsNullOrEmpty(ReturnTable.Rows(0).Item("yt_confidential_notes")) Then
                If ReturnOnlyText = False Then
                  returnString = "<span class=""" & cssClass & """>"
                End If

                returnString += ReturnTable.Rows(0).Item("yt_confidential_notes")

                If ReturnOnlyText = False Then
                  returnString += "</span>"
                End If
              End If
            End If
          End If
        End If
      End If
    End If



    Return returnString
  End Function
  Public Shared Function GetYachtConfidentialNotes(ByVal YachtID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      'Opening Connection
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sql = "SELECT yt_confidential_notes "
      sql += " FROM yacht WITH(NOLOCK) "
      sql += " inner join yacht_model on ym_model_id = yt_model_id "
      sql += " inner join yacht_category_size on  ycs_motor_type = ym_motor_type and ycs_category_size = ym_category_size "
      sql += " inner join yacht_motor_type on  ymt_motor_type = ym_motor_type  "
      sql += " WHERE yt_id = @yachtID"
      sql += " AND yt_journ_id = 0 "

      'save to session query debug string.
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetYachtConfidentialNotes(ByVal YachtID As Long) As DataTable</b><br />" & sql

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      SqlCommand.Parameters.AddWithValue("yachtID", YachtID)


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
      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in GetYachtConfidentialNotes(ByVal YachtID As Long) As DataTable: " + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function


  Public Shared Function Get_Yacht_Brand_And_Manufacturer() As DataTable
    Dim tempTable As New DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      Dim sQuery As String = ""

      sQuery = " select distinct comp_name, comp_id, ym_brand_name from Yacht_model "
      sQuery += " inner join Company with (NOLOCK) on comp_id = ym_mfr_comp_id and comp_journ_id = 0  "
      sQuery += " where ym_brand_name <> 'JETNET' " 'and comp_id in (" & CompanyIDString & ")"
      sQuery += " order by comp_name asc, ym_brand_name asc "

      SqlCommand.CommandText = sQuery

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Yacht_Brand_And_Manufacturer() As DataTable</b><br />" & sQuery


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        tempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = tempTable.GetErrors()
      End Try
      Return tempTable
    Catch ex As Exception
      Get_Yacht_Brand_And_Manufacturer = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Yacht_Brand_And_Manufacturer() As DataTable: SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
End Class
