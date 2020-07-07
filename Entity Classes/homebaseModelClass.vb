' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/homebaseModelClass.vb $
'$$Author: Mike $
'$$Date: 1/10/20 4:42p $
'$$Modtime: 1/10/20 1:30p $
'$$Revision: 11 $
'$$Workfile: homebaseModelClass.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class homebaseModelInfoClass

  Public Property amod_id() As Long
  Public Property amod_make_name() As String
  Public Property amod_model_name() As String
  Public Property amod_manufacturer() As String
  Public Property amod_manufacturer_comp_id() As Long
  Public Property amod_make_abbrev() As String
  Public Property amod_model_abbrev() As String
  Public Property amod_manufacturer_common_name() As String
  Public Property amod_airframe_type_code() As String
  Public Property amod_faa_model_id() As String
  Public Property amjiqs_cat_desc() As String
  Public Property amod_jniq_size() As String
  Public Property amod_type_code() As String
  Public Property amod_class_code() As String
  Public Property amod_weight_class() As String
  Public Property amod_start_year() As String
  Public Property amod_end_year() As String
  Public Property amod_start_price() As String
  Public Property amod_end_price() As String
  Public Property amod_description() As String
  Public Property amod_product_business_flag() As Boolean
  Public Property amod_product_commercial_flag() As Boolean
  Public Property amod_product_airbp_flag() As Boolean
  Public Property amod_product_helicopter_flag() As Boolean
  Public Property amod_ser_no_prefix() As String
  Public Property amod_ser_no_start() As String
  Public Property amod_ser_no_end() As String
  Public Property amod_ser_no_suffix() As String
  Public Property amod_serno_hyphen_flag() As Boolean

  Public Property amod_body_config() As String

  Sub New()

    amod_id = 0

    amod_make_name = ""
    amod_model_name = ""
    amod_manufacturer = ""
    amod_manufacturer_comp_id = 0
    amod_make_abbrev = ""
    amod_model_abbrev = ""
    amod_manufacturer_common_name = ""
    amod_airframe_type_code = ""
    amod_faa_model_id = ""
    amod_jniq_size = ""
    amjiqs_cat_desc = ""
    amod_type_code = ""
    amod_class_code = ""
    amod_weight_class = ""
    amod_start_year = ""
    amod_end_year = ""
    amod_start_price = ""
    amod_end_price = ""
    amod_description = ""

    amod_product_business_flag = False
    amod_product_commercial_flag = False
    amod_product_airbp_flag = False
    amod_product_helicopter_flag = False

    amod_ser_no_prefix = ""
    amod_ser_no_start = ""
    amod_ser_no_end = ""
    amod_ser_no_suffix = ""
    amod_serno_hyphen_flag = False

    amod_body_config = ""

  End Sub

  Sub New(ByVal nModelID As Long)

    amod_id = nModelID

    amod_make_name = ""
    amod_model_name = ""
    amod_manufacturer = ""
    amod_manufacturer_comp_id = 0
    amod_make_abbrev = ""
    amod_model_abbrev = ""
    amod_manufacturer_common_name = ""
    amod_airframe_type_code = ""
    amod_faa_model_id = ""
    amod_jniq_size = ""
    amjiqs_cat_desc = ""
    amod_type_code = ""
    amod_class_code = ""
    amod_weight_class = ""
    amod_start_year = ""
    amod_end_year = ""
    amod_start_price = ""
    amod_end_price = ""
    amod_description = ""

    amod_product_business_flag = False
    amod_product_commercial_flag = False
    amod_product_airbp_flag = False
    amod_product_helicopter_flag = False

    amod_ser_no_prefix = ""
    amod_ser_no_start = ""
    amod_ser_no_end = ""
    amod_ser_no_suffix = ""
    amod_serno_hyphen_flag = False

    amod_body_config = ""
  End Sub

  Public Function getModelDataTable(ByVal inModelID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      modelQuery.Append("SELECT Aircraft_Model.*, acwgtcls_name, amjiqs_cat_desc, amjiqs_cat_code FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH(NOLOCK)")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model_JIQ_Size WITH(NOLOCK) ON amod_jniq_size = amjiqs_cat_code")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Weight_Class WITH(NOLOCK) ON amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code")
      modelQuery.Append(" WHERE amod_id = @amod_id")

      SqlCommand.Parameters.AddWithValue("@amod_id", inModelID.ToString.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = modelQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub fillModelInfoClass()

    Dim resultsTable As New DataTable

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      resultsTable = getModelDataTable(amod_id)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("amod_make_name"))) Then
              amod_make_name = r.Item("amod_make_name").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_model_name"))) Then
              amod_model_name = r.Item("amod_model_name").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_manufacturer"))) Then
              amod_manufacturer = r.Item("amod_manufacturer").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_manufacturer_comp_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("amod_manufacturer_comp_id").ToString.Trim) Then
                If IsNumeric(r.Item("amod_manufacturer_comp_id").ToString.Trim) Then
                  amod_manufacturer_comp_id = CLng(r.Item("amod_manufacturer_comp_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("amod_make_abbrev"))) Then
              amod_make_abbrev = r.Item("amod_make_abbrev").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_model_abbrev"))) Then
              amod_model_abbrev = r.Item("amod_model_abbrev").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_manufacturer_common_name"))) Then
              amod_manufacturer_common_name = r.Item("amod_manufacturer_common_name").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_airframe_type_code"))) Then
              amod_airframe_type_code = r.Item("amod_airframe_type_code").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_faa_model_id"))) Then
              amod_faa_model_id = r.Item("amod_faa_model_id").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_jniq_size"))) Then
              amod_jniq_size = r.Item("amod_jniq_size").ToString.Trim
            End If

            If Not (IsDBNull(r("amjiqs_cat_desc"))) Then
              amjiqs_cat_desc = r.Item("amjiqs_cat_desc").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_type_code"))) Then
              amod_type_code = r.Item("amod_type_code").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_class_code"))) Then
              amod_class_code = r.Item("amod_class_code").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_weight_class"))) Then
              amod_weight_class = r.Item("amod_weight_class").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_start_year"))) Then
              amod_start_year = r.Item("amod_start_year").ToString.Trim.Trim
            End If

            If Not (IsDBNull(r("amod_end_year"))) Then
              amod_end_year = r.Item("amod_end_year").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_start_price"))) Then
              amod_start_price = r.Item("amod_start_price").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_end_price"))) Then
              amod_end_price = r.Item("amod_end_price").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_description"))) Then
              amod_description = r.Item("amod_description").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_product_business_flag"))) Then
              amod_product_business_flag = IIf(r.Item("amod_product_business_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("amod_product_commercial_flag"))) Then
              amod_product_commercial_flag = IIf(r.Item("amod_product_commercial_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("amod_product_airbp_flag"))) Then
              amod_product_airbp_flag = IIf(r.Item("amod_product_airbp_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("amod_product_helicopter_flag"))) Then
              amod_product_helicopter_flag = IIf(r.Item("amod_product_helicopter_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("amod_ser_no_prefix"))) Then
              amod_ser_no_prefix = r.Item("amod_ser_no_prefix").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_ser_no_start"))) Then
              amod_ser_no_start = r.Item("amod_ser_no_start").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_ser_no_end"))) Then
              amod_ser_no_end = r.Item("amod_ser_no_end").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_ser_no_suffix"))) Then
              amod_ser_no_suffix = r.Item("amod_ser_no_suffix").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_serno_hyphen_flag"))) Then
              amod_serno_hyphen_flag = IIf(r.Item("amod_serno_hyphen_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("amod_body_config"))) Then
              amod_body_config = r.Item("amod_body_config").ToString.Trim
            End If

          Next

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
    End Try
  End Sub

  Public Sub updateModelInfoClass()
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("UPDATE Aircraft_Model SET")

      If Not String.IsNullOrEmpty(amod_make_name.Trim) Then

        modelQuery.Append(" amod_make_name = @amod_make_name")
        SqlCommand.Parameters.AddWithValue("@amod_make_name", amod_make_name.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_model_name.Trim) Then

        modelQuery.Append(sSeperator + " amod_model_name = @amod_model_name")
        SqlCommand.Parameters.AddWithValue("@amod_model_name", amod_model_name.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_manufacturer.Trim) Then

        modelQuery.Append(sSeperator + " amod_manufacturer = @amod_manufacturer")
        SqlCommand.Parameters.AddWithValue("@amod_manufacturer", amod_manufacturer.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_make_abbrev.Trim) Then

        modelQuery.Append(sSeperator + " amod_make_abbrev = @amod_make_abbrev")
        SqlCommand.Parameters.AddWithValue("@amod_make_abbrev", amod_make_abbrev.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_model_abbrev.Trim) Then

        modelQuery.Append(sSeperator + " amod_model_abbrev = @amod_model_abbrev")
        SqlCommand.Parameters.AddWithValue("@amod_model_abbrev", amod_model_abbrev.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_manufacturer_common_name.Trim) Then

        modelQuery.Append(sSeperator + " amod_manufacturer_common_name = @amod_manufacturer_common_name")
        SqlCommand.Parameters.AddWithValue("@amod_manufacturer_common_name", amod_manufacturer_common_name.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_airframe_type_code.Trim) Then

        modelQuery.Append(sSeperator + " amod_airframe_type_code = @amod_airframe_type_code")
        SqlCommand.Parameters.AddWithValue("@amod_airframe_type_code", amod_airframe_type_code.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_faa_model_id.Trim) Then

        modelQuery.Append(sSeperator + " amod_faa_model_id = @amod_faa_model_id")
        SqlCommand.Parameters.AddWithValue("@amod_faa_model_id", amod_faa_model_id.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_jniq_size.Trim) Then

        modelQuery.Append(sSeperator + " amod_jniq_size = @amod_jniq_size")
        SqlCommand.Parameters.AddWithValue("@amod_jniq_size", amod_jniq_size.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_type_code.Trim) Then

        modelQuery.Append(sSeperator + " amod_type_code = @amod_type_code")
        SqlCommand.Parameters.AddWithValue("@amod_type_code", amod_type_code.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_class_code.Trim) Then

        modelQuery.Append(sSeperator + " amod_class_code = @amod_class_code")
        SqlCommand.Parameters.AddWithValue("@amod_class_code", amod_class_code.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_weight_class.Trim) Then

        modelQuery.Append(sSeperator + " amod_weight_class = @amod_weight_class")
        SqlCommand.Parameters.AddWithValue("@amod_weight_class", amod_weight_class.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_start_year.Trim) Then

        modelQuery.Append(sSeperator + " amod_start_year = @amod_start_year")
        SqlCommand.Parameters.AddWithValue("@amod_start_year", amod_start_year.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_start_year = @amod_start_year")
        SqlCommand.Parameters.AddWithValue("@amod_start_year", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_end_year.Trim) Then

        modelQuery.Append(sSeperator + " amod_end_year = @amod_end_year")
        SqlCommand.Parameters.AddWithValue("@amod_end_year", amod_end_year.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_end_year = @amod_end_year")
        SqlCommand.Parameters.AddWithValue("@amod_end_year", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_start_price.Trim) Then

        modelQuery.Append(sSeperator + " amod_start_price = @amod_start_price")
        SqlCommand.Parameters.AddWithValue("@amod_start_price", amod_start_price.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_start_price = @amod_start_price")
        SqlCommand.Parameters.AddWithValue("@amod_start_price", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_end_price.Trim) Then

        modelQuery.Append(sSeperator + " amod_end_price = @amod_end_price")
        SqlCommand.Parameters.AddWithValue("@amod_end_price", amod_end_price.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_end_price = @amod_end_price")
        SqlCommand.Parameters.AddWithValue("@amod_end_price", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_description.Trim) Then

        modelQuery.Append(sSeperator + " amod_description = @amod_description")
        SqlCommand.Parameters.AddWithValue("@amod_description", amod_description.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_ser_no_prefix.Trim) Then

        modelQuery.Append(sSeperator + " amod_ser_no_prefix = @amod_ser_no_prefix")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_prefix", amod_ser_no_prefix.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_ser_no_prefix = @amod_ser_no_prefix")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_prefix", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_ser_no_start.Trim) Then

        modelQuery.Append(sSeperator + " amod_ser_no_start = @amod_ser_no_start")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_start", amod_ser_no_start.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_ser_no_start = @amod_ser_no_start")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_start", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_ser_no_end.Trim) Then

        modelQuery.Append(sSeperator + " amod_ser_no_end = @amod_ser_no_end")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_end", amod_ser_no_end.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_ser_no_end = @amod_ser_no_end")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_end", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_ser_no_suffix.Trim) Then

        modelQuery.Append(sSeperator + " amod_ser_no_suffix = @amod_ser_no_suffix")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_suffix", amod_ser_no_suffix.Trim)
        sSeperator = ","

      Else

        modelQuery.Append(sSeperator + " amod_ser_no_suffix = @amod_ser_no_suffix")
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_suffix", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_body_config.Trim) Then
        modelQuery.Append(sSeperator + " amod_body_config = @amod_body_config")
        SqlCommand.Parameters.AddWithValue("@amod_body_config", amod_body_config.Trim)
        sSeperator = ","
      End If

      modelQuery.Append(sSeperator + " amod_product_business_flag = @amod_product_business_flag")
      SqlCommand.Parameters.AddWithValue("@amod_product_business_flag", IIf(amod_product_business_flag, "Y", "N"))

      modelQuery.Append(", amod_product_commercial_flag = @amod_product_commercial_flag")
      SqlCommand.Parameters.AddWithValue("@amod_product_commercial_flag", IIf(amod_product_commercial_flag, "Y", "N"))

      modelQuery.Append(", amod_product_helicopter_flag = @amod_product_helicopter_flag")
      SqlCommand.Parameters.AddWithValue("@amod_product_helicopter_flag", IIf(amod_product_helicopter_flag, "Y", "N"))

      modelQuery.Append(", amod_product_airbp_flag = @amod_product_airbp_flag")
      SqlCommand.Parameters.AddWithValue("@amod_product_airbp_flag", IIf(amod_product_airbp_flag, "Y", "N"))

      modelQuery.Append(", amod_serno_hyphen_flag = @amod_serno_hyphen_flag")
      SqlCommand.Parameters.AddWithValue("@amod_serno_hyphen_flag", IIf(amod_serno_hyphen_flag, "Y", "N"))

      modelQuery.Append(" WHERE amod_id = @amod_id")
      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertModelInfoClass()

    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("INSERT INTO Aircraft_Model (amod_make_name, amod_model_name, amod_manufacturer, amod_manufacturer_comp_id, amod_make_abbrev,")
      modelQuery.Append(" amod_model_abbrev, amod_manufacturer_common_name, amod_airframe_type_code, amod_faa_model_id, amod_jniq_size, amod_type_code,")
      modelQuery.Append(" amod_class_code, amod_weight_class, amod_start_year, amod_end_year, amod_start_price, amod_end_price,")
      modelQuery.Append(" amod_description, amod_ser_no_prefix, amod_ser_no_start, amod_ser_no_end, amod_ser_no_suffix, amod_serno_hyphen_flag,")
      modelQuery.Append(" amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, amod_product_airbp_flag, amod_body_config")
      modelQuery.Append(") VALUES (@amod_make_name, @amod_model_name, @amod_manufacturer, @amod_manufacturer_comp_id, @amod_make_abbrev,")
      modelQuery.Append(" @amod_model_abbrev, @amod_manufacturer_common_name, @amod_airframe_type_code, @amod_faa_model_id, @amod_jniq_size, @amod_type_code,")
      modelQuery.Append(" @amod_class_code, @amod_weight_class, @amod_start_year, @amod_end_year, @amod_start_price, @amod_end_price,")
      modelQuery.Append(" @amod_description, @amod_ser_no_prefix, @amod_ser_no_start, @amod_ser_no_end, @amod_ser_no_suffix, @amod_serno_hyphen_flag")
      modelQuery.Append(" @amod_product_business_flag, @amod_product_commercial_flag, @amod_product_helicopter_flag, @amod_product_airbp_flag, @amod_body_config")
      modelQuery.Append(") WHERE amod_id = @amod_id")

      If Not String.IsNullOrEmpty(amod_make_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_make_name", amod_make_name.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_model_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_model_name", amod_model_name.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_manufacturer.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_manufacturer", amod_manufacturer.Trim)
      End If

      If amod_manufacturer_comp_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_manufacturer_comp_id", amod_manufacturer_comp_id.ToString)
      End If

      If Not String.IsNullOrEmpty(amod_make_abbrev.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_make_abbrev", amod_make_abbrev.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_model_abbrev.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_model_abbrev", amod_model_abbrev.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_manufacturer_common_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_manufacturer_common_name", amod_manufacturer_common_name.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_airframe_type_code.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_airframe_type_code", amod_airframe_type_code.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_faa_model_id.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_faa_model_id", amod_faa_model_id.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_jniq_size.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_jniq_size", amod_jniq_size.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_type_code.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_type_code", amod_type_code.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_class_code.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_class_code", amod_class_code.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_weight_class.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_weight_class", amod_weight_class.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_start_year.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_start_year", amod_start_year.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_end_year.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_end_year", amod_end_year.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_start_price.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_start_price", amod_start_price.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_end_price.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_end_price", amod_end_price.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_description.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_description", amod_description.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_ser_no_prefix.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_prefix", amod_ser_no_prefix.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_ser_no_start.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_start", amod_ser_no_start.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_ser_no_end.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_end", amod_ser_no_end.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_ser_no_suffix.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_ser_no_suffix", amod_ser_no_suffix.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_body_config.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_body_config", amod_body_config.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@amod_product_business_flag", IIf(amod_product_business_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@amod_product_commercial_flag", IIf(amod_product_commercial_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@amod_product_helicopter_flag", IIf(amod_product_helicopter_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@amod_product_airbp_flag", IIf(amod_product_airbp_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@amod_serno_hyphen_flag", IIf(amod_serno_hyphen_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Overrides Function Equals(obj As Object) As Boolean
    Dim [class] = TryCast(obj, homebaseModelInfoClass)
    Return [class] IsNot Nothing AndAlso
           amod_id = [class].amod_id AndAlso
           amod_make_name = [class].amod_make_name AndAlso
           amod_model_name = [class].amod_model_name AndAlso
           amod_manufacturer = [class].amod_manufacturer AndAlso
           amod_manufacturer_comp_id = [class].amod_manufacturer_comp_id AndAlso
           amod_make_abbrev = [class].amod_make_abbrev AndAlso
           amod_model_abbrev = [class].amod_model_abbrev AndAlso
           amod_manufacturer_common_name = [class].amod_manufacturer_common_name AndAlso
           amod_airframe_type_code = [class].amod_airframe_type_code AndAlso
           amod_faa_model_id = [class].amod_faa_model_id AndAlso
           amjiqs_cat_desc = [class].amjiqs_cat_desc AndAlso
           amod_jniq_size = [class].amod_jniq_size AndAlso
           amod_type_code = [class].amod_type_code AndAlso
           amod_class_code = [class].amod_class_code AndAlso
           amod_weight_class = [class].amod_weight_class AndAlso
           amod_start_year = [class].amod_start_year AndAlso
           amod_end_year = [class].amod_end_year AndAlso
           amod_start_price = [class].amod_start_price AndAlso
           amod_end_price = [class].amod_end_price AndAlso
           amod_description = [class].amod_description AndAlso
           amod_product_business_flag = [class].amod_product_business_flag AndAlso
           amod_product_commercial_flag = [class].amod_product_commercial_flag AndAlso
           amod_product_airbp_flag = [class].amod_product_airbp_flag AndAlso
           amod_product_helicopter_flag = [class].amod_product_helicopter_flag AndAlso
           amod_ser_no_prefix = [class].amod_ser_no_prefix AndAlso
           amod_ser_no_start = [class].amod_ser_no_start AndAlso
           amod_ser_no_end = [class].amod_ser_no_end AndAlso
           amod_ser_no_suffix = [class].amod_ser_no_suffix AndAlso
           amod_serno_hyphen_flag = [class].amod_serno_hyphen_flag AndAlso
           amod_body_config = [class].amod_body_config
  End Function

  Public Shared Operator =(class1 As homebaseModelInfoClass, class2 As homebaseModelInfoClass) As Boolean
    Return EqualityComparer(Of homebaseModelInfoClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseModelInfoClass, class2 As homebaseModelInfoClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class

<System.Serializable()> Public Class homebaseModelPerfSpecsClass

  Public Property amod_id() As Long
  Public Property amod_airframe_type_code() As String
  Public Property amod_fuselage_length() As Decimal
  Public Property amod_fuselage_height() As Decimal
  Public Property amod_fuselage_wingspan() As Decimal
  Public Property amod_fuselage_width() As Decimal
  Public Property amod_number_of_crew() As Integer
  Public Property amod_number_of_passengers() As Integer
  Public Property amod_pressure() As Decimal
  Public Property amod_max_ramp_weight() As Decimal
  Public Property amod_max_takeoff_weight() As Decimal
  Public Property amod_zero_fuel_weight() As Decimal
  Public Property amod_weight_eow() As Decimal
  Public Property amod_basic_op_weight() As Decimal
  Public Property amod_max_landing_weight() As Decimal
  Public Property amod_ifr_certification() As String
  Public Property amod_climb_normal_feet() As Decimal
  Public Property amod_climb_engout_feet() As Decimal
  Public Property amod_ceiling_feet() As Decimal
  Public Property amod_climb_hoge() As Decimal
  Public Property amod_climb_hige() As Decimal
  Public Property amod_max_range_miles() As Decimal
  Public Property amod_range_tanks_full() As Decimal
  Public Property amod_range_seats_full() As Decimal
  Public Property amod_range_4_passenger() As Decimal
  Public Property amod_range_8_passenger() As Decimal
  Public Property amod_number_of_props() As Integer
  Public Property amod_prop_model_name() As String
  Public Property amod_prop_mfr_name() As String
  Public Property amod_prop_com_tbo_hrs() As Decimal
  Public Property amod_other_config_note() As String
  Public Property amod_cabinsize_height_feet() As Integer
  Public Property amod_cabinsize_height_inches() As Integer
  Public Property amod_cabinsize_width_feet() As Integer
  Public Property amod_cabinsize_width_inches() As Integer
  Public Property amod_cabinsize_length_feet() As Integer
  Public Property amod_cabinsize_length_inches() As Integer
  Public Property amod_cabin_volume() As Decimal
  Public Property amod_baggage_volume() As Decimal
  Public Property amod_fuel_cap_std_weight() As Decimal
  Public Property amod_fuel_cap_std_gal() As Decimal
  Public Property amod_fuel_cap_opt_weight() As Decimal
  Public Property amod_fuel_cap_opt_gal() As Decimal
  Public Property amod_stall_vs() As Decimal
  Public Property amod_stall_vso() As Decimal
  Public Property amod_cruis_speed() As Decimal
  Public Property amod_max_speed() As Decimal
  Public Property amod_vne_maxop_speed() As Decimal
  Public Property amod_v1_takeoff_speed() As Decimal
  Public Property amod_vfe_max_flap_extended_speed() As Decimal
  Public Property amod_vle_max_landing_gear_ext_speed() As Decimal
  Public Property amod_field_length() As Decimal
  Public Property amod_takeoff_ali() As Decimal
  Public Property amod_takeoff_500() As Decimal
  Public Property amod_number_of_engines() As Integer
  Public Property amod_engine_thrust_lbs() As Decimal
  Public Property amod_engine_shaft() As Decimal
  Public Property amod_engine_com_tbo_hrs() As Decimal
  Public Property amod_main_rotor_1_blade_count() As Integer
  Public Property amod_main_rotor_1_blade_diameter() As Decimal
  Public Property amod_main_rotor_2_blade_count() As Integer
  Public Property amod_main_rotor_2_blade_diameter() As Decimal
  Public Property amod_tail_rotor_blade_count() As Integer
  Public Property amod_tail_rotor_blade_diameter() As Decimal
  Public Property amod_rotor_anti_torque_system() As String

  Sub New()

    amod_id = 0
    amod_airframe_type_code = ""

    amod_fuselage_length = 0.0
    amod_fuselage_height = 0.0
    amod_fuselage_wingspan = 0.0
    amod_fuselage_width = 0.0

    amod_number_of_crew = 0
    amod_number_of_passengers = 0

    amod_pressure = 0.0

    amod_max_ramp_weight = 0.0
    amod_max_takeoff_weight = 0.0
    amod_zero_fuel_weight = 0.0
    amod_weight_eow = 0.0
    amod_basic_op_weight = 0.0
    amod_max_landing_weight = 0.0

    amod_ifr_certification = ""

    amod_climb_normal_feet = 0.0
    amod_climb_engout_feet = 0.0
    amod_ceiling_feet = 0.0
    amod_climb_hoge = 0.0
    amod_climb_hige = 0.0

    amod_max_range_miles = 0.0
    amod_range_tanks_full = 0.0
    amod_range_seats_full = 0.0
    amod_range_4_passenger = 0.0
    amod_range_8_passenger = 0.0

    amod_number_of_props = 0
    amod_prop_model_name = ""
    amod_prop_mfr_name = ""
    amod_prop_com_tbo_hrs = 0.0

    amod_other_config_note = ""

    amod_cabinsize_height_feet = 0
    amod_cabinsize_height_inches = 0
    amod_cabinsize_width_feet = 0
    amod_cabinsize_width_inches = 0
    amod_cabinsize_length_feet = 0
    amod_cabinsize_length_inches = 0

    amod_cabin_volume = 0.0
    amod_baggage_volume = 0.0

    amod_fuel_cap_std_weight = 0.0
    amod_fuel_cap_std_gal = 0.0
    amod_fuel_cap_opt_weight = 0.0
    amod_fuel_cap_opt_gal = 0.0

    amod_stall_vs = 0.0
    amod_stall_vso = 0.0
    amod_cruis_speed = 0.0
    amod_max_speed = 0.0
    amod_vne_maxop_speed = 0.0
    amod_v1_takeoff_speed = 0.0
    amod_vfe_max_flap_extended_speed = 0.0
    amod_vle_max_landing_gear_ext_speed = 0.0

    amod_field_length = 0.0

    amod_takeoff_ali = 0.0
    amod_takeoff_500 = 0.0

    amod_number_of_engines = 0

    amod_engine_thrust_lbs = 0.0
    amod_engine_shaft = 0.0
    amod_engine_com_tbo_hrs = 0.0

    amod_main_rotor_1_blade_count = 0
    amod_main_rotor_1_blade_diameter = 0.0
    amod_main_rotor_2_blade_count = 0
    amod_main_rotor_2_blade_diameter = 0.0
    amod_tail_rotor_blade_count = 0
    amod_tail_rotor_blade_diameter = 0.0

    amod_rotor_anti_torque_system = ""

  End Sub

  Sub New(ByVal nModelID As Long)

    amod_id = nModelID
    amod_airframe_type_code = ""

    amod_fuselage_length = 0.0
    amod_fuselage_height = 0.0
    amod_fuselage_wingspan = 0.0
    amod_fuselage_width = 0.0

    amod_number_of_crew = 0
    amod_number_of_passengers = 0

    amod_pressure = 0.0

    amod_max_ramp_weight = 0.0
    amod_max_takeoff_weight = 0.0
    amod_zero_fuel_weight = 0.0
    amod_weight_eow = 0.0
    amod_basic_op_weight = 0.0
    amod_max_landing_weight = 0.0

    amod_ifr_certification = ""

    amod_climb_normal_feet = 0.0
    amod_climb_engout_feet = 0.0
    amod_ceiling_feet = 0.0
    amod_climb_hoge = 0.0
    amod_climb_hige = 0.0

    amod_max_range_miles = 0.0
    amod_range_tanks_full = 0.0
    amod_range_seats_full = 0.0
    amod_range_4_passenger = 0.0
    amod_range_8_passenger = 0.0

    amod_number_of_props = 0
    amod_prop_model_name = ""
    amod_prop_mfr_name = ""
    amod_prop_com_tbo_hrs = 0.0

    amod_other_config_note = ""

    amod_cabinsize_height_feet = 0
    amod_cabinsize_height_inches = 0
    amod_cabinsize_width_feet = 0
    amod_cabinsize_width_inches = 0
    amod_cabinsize_length_feet = 0
    amod_cabinsize_length_inches = 0

    amod_cabin_volume = 0.0
    amod_baggage_volume = 0.0

    amod_fuel_cap_std_weight = 0.0
    amod_fuel_cap_std_gal = 0.0
    amod_fuel_cap_opt_weight = 0.0
    amod_fuel_cap_opt_gal = 0.0

    amod_stall_vs = 0.0
    amod_stall_vso = 0.0
    amod_cruis_speed = 0.0
    amod_max_speed = 0.0
    amod_vne_maxop_speed = 0.0
    amod_v1_takeoff_speed = 0.0
    amod_vfe_max_flap_extended_speed = 0.0
    amod_vle_max_landing_gear_ext_speed = 0.0

    amod_field_length = 0.0

    amod_takeoff_ali = 0.0
    amod_takeoff_500 = 0.0

    amod_number_of_engines = 0

    amod_engine_thrust_lbs = 0.0
    amod_engine_shaft = 0.0
    amod_engine_com_tbo_hrs = 0.0

    amod_main_rotor_1_blade_count = 0
    amod_main_rotor_1_blade_diameter = 0.0
    amod_main_rotor_2_blade_count = 0
    amod_main_rotor_2_blade_diameter = 0.0
    amod_tail_rotor_blade_count = 0
    amod_tail_rotor_blade_diameter = 0.0

    amod_rotor_anti_torque_system = ""

  End Sub

  Public Function getModelDataTable(ByVal inModelID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      modelQuery.Append("SELECT Aircraft_Model.*, acwgtcls_name, amjiqs_cat_desc, amjiqs_cat_code FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH(NOLOCK)")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model_JIQ_Size WITH(NOLOCK) ON amod_jniq_size = amjiqs_cat_code")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Weight_Class WITH(NOLOCK) ON amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code")
      modelQuery.Append(" WHERE amod_id = @amod_id")

      SqlCommand.Parameters.AddWithValue("@amod_id", inModelID.ToString.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = modelQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub fillModelPerfSpecsClass()

    Dim resultsTable As New DataTable

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      resultsTable = getModelDataTable(amod_id)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("amod_airframe_type_code"))) Then
              amod_airframe_type_code = r.Item("amod_airframe_type_code").ToString.Trim
            End If

            ' FUSELAGE DIMENSIONS
            If Not (IsDBNull(r("amod_fuselage_length"))) Then
              amod_fuselage_length = CDec(r.Item("amod_fuselage_length").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuselage_height"))) Then
              amod_fuselage_height = CDec(r.Item("amod_fuselage_height").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuselage_wingspan"))) Then
              amod_fuselage_wingspan = CDec(r.Item("amod_fuselage_wingspan").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuselage_width"))) Then
              amod_fuselage_width = CDec(r.Item("amod_fuselage_width").ToString.Trim)
            End If

            ' TYPICAL CONFIGURATION
            If Not (IsDBNull(r("amod_number_of_crew"))) Then
              amod_number_of_crew = CInt(r.Item("amod_number_of_crew").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_number_of_passengers"))) Then
              amod_number_of_passengers = CInt(r.Item("amod_number_of_passengers").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_pressure"))) Then
              amod_pressure = CDec(r.Item("amod_pressure").ToString.Trim)
            End If

            ' WEIGHT
            If Not (IsDBNull(r("amod_max_ramp_weight"))) Then
              amod_max_ramp_weight = CDec(r.Item("amod_max_ramp_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_max_takeoff_weight"))) Then
              amod_max_takeoff_weight = CDec(r.Item("amod_max_takeoff_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_zero_fuel_weight"))) Then
              amod_zero_fuel_weight = CDec(r.Item("amod_zero_fuel_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_weight_eow"))) Then
              amod_weight_eow = CDec(r.Item("amod_weight_eow").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_basic_op_weight"))) Then
              amod_basic_op_weight = CDec(r.Item("amod_basic_op_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_max_landing_weight"))) Then
              amod_max_landing_weight = CDec(r.Item("amod_max_landing_weight").ToString.Trim)
            End If

            ' IFR Certification
            If Not (IsDBNull(r("amod_ifr_certification"))) Then
              amod_ifr_certification = r.Item("amod_ifr_certification").ToString.Trim
            End If

            ' CLIMB
            If Not (IsDBNull(r("amod_climb_normal_feet"))) Then
              amod_climb_normal_feet = CDec(r.Item("amod_climb_normal_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_climb_engout_feet"))) Then
              amod_climb_engout_feet = CDec(r.Item("amod_climb_engout_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_ceiling_feet"))) Then
              amod_ceiling_feet = CDec(r.Item("amod_ceiling_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_climb_hoge"))) Then
              amod_climb_hoge = CDec(r.Item("amod_climb_hoge").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_climb_hige"))) Then
              amod_climb_hige = CDec(r.Item("amod_climb_hige").ToString.Trim)
            End If

            ' RANGE
            If Not (IsDBNull(r("amod_max_range_miles"))) Then
              amod_max_range_miles = CDec(r.Item("amod_max_range_miles").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_range_tanks_full"))) Then
              amod_range_tanks_full = CDec(r.Item("amod_range_tanks_full").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_range_seats_full"))) Then
              amod_range_seats_full = CDec(r.Item("amod_range_seats_full").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_range_4_passenger"))) Then
              amod_range_4_passenger = CDec(r.Item("amod_range_4_passenger").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_range_8_passenger"))) Then
              amod_range_8_passenger = CDec(r.Item("amod_range_8_passenger").ToString.Trim)
            End If

            ' PROPELLERS
            If Not (IsDBNull(r("amod_number_of_props"))) Then
              amod_number_of_props = CInt(r.Item("amod_number_of_props").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_prop_model_name"))) Then
              amod_prop_model_name = r.Item("amod_prop_model_name").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_prop_mfr_name"))) Then
              amod_prop_mfr_name = r.Item("amod_prop_mfr_name").ToString.Trim
            End If

            If Not (IsDBNull(r("amod_prop_com_tbo_hrs"))) Then
              amod_prop_com_tbo_hrs = CDec(r.Item("amod_prop_com_tbo_hrs").ToString.Trim)
            End If

            ' CONFIG NOTE
            If Not (IsDBNull(r("amod_other_config_note"))) Then
              amod_other_config_note = r.Item("amod_other_config_note").ToString.Trim
            End If

            ' CABIN DIMENSIONS
            If Not (IsDBNull(r("amod_cabinsize_height_feet"))) Then
              amod_cabinsize_height_feet = CInt(r.Item("amod_cabinsize_height_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabinsize_height_inches"))) Then
              amod_cabinsize_height_inches = CInt(r.Item("amod_cabinsize_height_inches").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabinsize_width_feet"))) Then
              amod_cabinsize_width_feet = CInt(r.Item("amod_cabinsize_width_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabinsize_width_inches"))) Then
              amod_cabinsize_width_inches = CInt(r.Item("amod_cabinsize_width_inches").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabinsize_length_feet"))) Then
              amod_cabinsize_length_feet = CInt(r.Item("amod_cabinsize_length_feet").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabinsize_length_inches"))) Then
              amod_cabinsize_length_inches = CInt(r.Item("amod_cabinsize_length_inches").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cabin_volume"))) Then
              amod_cabin_volume = CDec(r.Item("amod_cabin_volume").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_baggage_volume"))) Then
              amod_baggage_volume = CDec(r.Item("amod_baggage_volume").ToString.Trim)
            End If

            ' FUEL CAPACITY
            If Not (IsDBNull(r("amod_fuel_cap_std_weight"))) Then
              amod_fuel_cap_std_weight = CDec(r.Item("amod_fuel_cap_std_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_cap_std_gal"))) Then
              amod_fuel_cap_std_gal = CDec(r.Item("amod_fuel_cap_std_gal").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_cap_opt_weight"))) Then
              amod_fuel_cap_opt_weight = CDec(r.Item("amod_fuel_cap_opt_weight").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_cap_opt_gal"))) Then
              amod_fuel_cap_opt_gal = CDec(r.Item("amod_fuel_cap_opt_gal").ToString.Trim)
            End If

            ' SPEED

            If Not (IsDBNull(r("amod_stall_vs"))) Then
              amod_stall_vs = CDec(r.Item("amod_stall_vs").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_stall_vso"))) Then
              amod_stall_vso = CDec(r.Item("amod_stall_vso").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cruis_speed"))) Then
              amod_cruis_speed = CDec(r.Item("amod_cruis_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_max_speed"))) Then
              amod_max_speed = CDec(r.Item("amod_max_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_vne_maxop_speed"))) Then
              amod_vne_maxop_speed = CDec(r.Item("amod_vne_maxop_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_v1_takeoff_speed"))) Then
              amod_v1_takeoff_speed = CDec(r.Item("amod_v1_takeoff_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_vfe_max_flap_extended_speed"))) Then
              amod_vfe_max_flap_extended_speed = CDec(r.Item("amod_vfe_max_flap_extended_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_vle_max_landing_gear_ext_speed"))) Then
              amod_vle_max_landing_gear_ext_speed = CDec(r.Item("amod_vle_max_landing_gear_ext_speed").ToString.Trim)
            End If

            ' LANDING PERFORMANCE
            If Not (IsDBNull(r("amod_field_length"))) Then
              amod_field_length = CDec(r.Item("amod_field_length").ToString.Trim)
            End If

            ' TAKEOFF PERFORMANCE
            If Not (IsDBNull(r("amod_takeoff_ali"))) Then
              amod_takeoff_ali = CDec(r.Item("amod_takeoff_ali").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_takeoff_500"))) Then
              amod_takeoff_500 = CDec(r.Item("amod_takeoff_500").ToString.Trim)
            End If

            ' ENGINES
            If Not (IsDBNull(r("amod_number_of_engines"))) Then
              amod_number_of_engines = CInt(r.Item("amod_number_of_engines").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_engine_thrust_lbs"))) Then
              amod_engine_thrust_lbs = CDec(r.Item("amod_engine_thrust_lbs").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_engine_shaft"))) Then
              amod_engine_shaft = CDec(r.Item("amod_engine_shaft").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_engine_com_tbo_hrs"))) Then
              amod_engine_com_tbo_hrs = CDec(r.Item("amod_engine_com_tbo_hrs").ToString.Trim)
            End If

            ' ROTORS
            If Not (IsDBNull(r("amod_main_rotor_1_blade_count"))) Then
              amod_main_rotor_1_blade_count = CInt(r.Item("amod_main_rotor_1_blade_count").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_main_rotor_1_blade_diameter"))) Then
              amod_main_rotor_1_blade_diameter = CDec(r.Item("amod_main_rotor_1_blade_diameter").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_main_rotor_2_blade_count"))) Then
              amod_main_rotor_2_blade_count = CInt(r.Item("amod_main_rotor_2_blade_count").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_main_rotor_2_blade_diameter"))) Then
              amod_main_rotor_2_blade_diameter = CDec(r.Item("amod_main_rotor_2_blade_diameter").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tail_rotor_blade_count"))) Then
              amod_tail_rotor_blade_count = CInt(r.Item("amod_tail_rotor_blade_count").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tail_rotor_blade_diameter"))) Then
              amod_tail_rotor_blade_diameter = CDec(r.Item("amod_tail_rotor_blade_diameter").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_rotor_anti_torque_system"))) Then
              amod_rotor_anti_torque_system = r.Item("amod_rotor_anti_torque_system").ToString.Trim
            End If

          Next

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
    End Try
  End Sub

  Public Sub updateModelPerfSpecsClass()
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("UPDATE Aircraft_Model SET")

      If amod_fuselage_length >= 0 Then

        modelQuery.Append(" amod_fuselage_length = @amod_fuselage_length")
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_length", amod_fuselage_length.ToString)
        sSeperator = ","

      End If

      If amod_fuselage_height >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuselage_height = @amod_fuselage_height")
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_height", amod_fuselage_height.ToString)
        sSeperator = ","

      End If

      If amod_fuselage_wingspan >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuselage_wingspan = @amod_fuselage_wingspan")
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_wingspan", amod_fuselage_wingspan.ToString)
        sSeperator = ","

      End If

      If amod_fuselage_width >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuselage_width = @amod_fuselage_width")
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_width", amod_fuselage_width.ToString)
        sSeperator = ","

      End If

      If amod_number_of_crew >= 0 Then

        modelQuery.Append(sSeperator + " amod_number_of_crew = @amod_number_of_crew")
        SqlCommand.Parameters.AddWithValue("@amod_number_of_crew", amod_number_of_crew.ToString)
        sSeperator = ","

      End If

      If amod_number_of_passengers >= 0 Then

        modelQuery.Append(sSeperator + " amod_number_of_passengers = @amod_number_of_passengers")
        SqlCommand.Parameters.AddWithValue("@amod_number_of_passengers", amod_number_of_passengers.ToString)
        sSeperator = ","

      End If

      If amod_pressure >= 0 Then

        modelQuery.Append(sSeperator + " amod_pressure = @amod_pressure")
        SqlCommand.Parameters.AddWithValue("@amod_pressure", amod_pressure.ToString)
        sSeperator = ","

      End If

      If amod_max_ramp_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_max_ramp_weight = @amod_max_ramp_weight")
        SqlCommand.Parameters.AddWithValue("@amod_max_ramp_weight", amod_max_ramp_weight.ToString)
        sSeperator = ","

      End If

      If amod_max_takeoff_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_max_takeoff_weight = @amod_max_takeoff_weight")
        SqlCommand.Parameters.AddWithValue("@amod_max_takeoff_weight", amod_max_takeoff_weight.ToString)
        sSeperator = ","

      End If

      If amod_zero_fuel_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_zero_fuel_weight = @amod_zero_fuel_weight")
        SqlCommand.Parameters.AddWithValue("@amod_zero_fuel_weight", amod_zero_fuel_weight.ToString)
        sSeperator = ","

      End If

      If amod_weight_eow >= 0 Then

        modelQuery.Append(sSeperator + " amod_weight_eow = @amod_weight_eow")
        SqlCommand.Parameters.AddWithValue("@amod_weight_eow", amod_weight_eow.ToString)
        sSeperator = ","

      End If

      If amod_basic_op_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_basic_op_weight = @amod_basic_op_weight")
        SqlCommand.Parameters.AddWithValue("@amod_basic_op_weight", amod_basic_op_weight.ToString)
        sSeperator = ","

      End If

      If amod_max_landing_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_max_landing_weight = @amod_max_landing_weight")
        SqlCommand.Parameters.AddWithValue("@amod_max_landing_weight", amod_max_landing_weight.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_ifr_certification.Trim) Then

        modelQuery.Append(sSeperator + " amod_ifr_certification = @amod_ifr_certification")
        SqlCommand.Parameters.AddWithValue("@amod_ifr_certification", amod_ifr_certification.Trim)
        sSeperator = ","

      End If

      If amod_climb_normal_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_climb_normal_feet = @amod_climb_normal_feet")
        SqlCommand.Parameters.AddWithValue("@amod_climb_normal_feet", amod_climb_normal_feet.ToString)
        sSeperator = ","

      End If

      If amod_climb_engout_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_climb_engout_feet = @amod_climb_engout_feet")
        SqlCommand.Parameters.AddWithValue("@amod_climb_engout_feet", amod_climb_engout_feet.ToString)
        sSeperator = ","

      End If

      If amod_ceiling_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_ceiling_feet = @amod_ceiling_feet")
        SqlCommand.Parameters.AddWithValue("@amod_ceiling_feet", amod_ceiling_feet.ToString)
        sSeperator = ","

      End If

      If amod_climb_hoge >= 0 Then

        modelQuery.Append(sSeperator + " amod_climb_hoge = @amod_climb_hoge")
        SqlCommand.Parameters.AddWithValue("@amod_climb_hoge", amod_climb_hoge.ToString)
        sSeperator = ","

      End If

      If amod_climb_hige >= 0 Then

        modelQuery.Append(sSeperator + " amod_climb_hige = @amod_climb_hige")
        SqlCommand.Parameters.AddWithValue("@amod_climb_hige", amod_climb_hige.ToString)
        sSeperator = ","

      End If

      If amod_max_range_miles >= 0 Then

        modelQuery.Append(sSeperator + " amod_max_range_miles = @amod_max_range_miles")
        SqlCommand.Parameters.AddWithValue("@amod_max_range_miles", amod_max_range_miles.ToString)
        sSeperator = ","

      End If

      If amod_range_tanks_full >= 0 Then

        modelQuery.Append(sSeperator + " amod_range_tanks_full = @amod_range_tanks_full")
        SqlCommand.Parameters.AddWithValue("@amod_range_tanks_full", amod_range_tanks_full.ToString)
        sSeperator = ","

      End If

      If amod_range_seats_full >= 0 Then

        modelQuery.Append(sSeperator + " amod_range_seats_full = @amod_range_seats_full")
        SqlCommand.Parameters.AddWithValue("@amod_range_seats_full", amod_range_seats_full.ToString)
        sSeperator = ","

      End If

      If amod_range_4_passenger >= 0 Then

        modelQuery.Append(sSeperator + " amod_range_4_passenger = @amod_range_4_passenger")
        SqlCommand.Parameters.AddWithValue("@amod_range_4_passenger", amod_range_4_passenger.ToString)
        sSeperator = ","

      End If

      If amod_range_8_passenger >= 0 Then

        modelQuery.Append(sSeperator + " amod_range_8_passenger = @amod_range_8_passenger")
        SqlCommand.Parameters.AddWithValue("@amod_range_8_passenger", amod_range_8_passenger.ToString)
        sSeperator = ","

      End If

      If amod_number_of_props >= 0 Then

        modelQuery.Append(sSeperator + " amod_number_of_props = @amod_number_of_props")
        SqlCommand.Parameters.AddWithValue("@amod_number_of_props", amod_number_of_props.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_prop_model_name.Trim) Then

        modelQuery.Append(sSeperator + " amod_prop_model_name = @amod_prop_model_name")
        SqlCommand.Parameters.AddWithValue("@amod_prop_model_name", amod_prop_model_name.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_prop_mfr_name.Trim) Then

        modelQuery.Append(sSeperator + " amod_prop_mfr_name = @amod_prop_mfr_name")
        SqlCommand.Parameters.AddWithValue("@amod_prop_mfr_name", amod_prop_mfr_name.Trim)
        sSeperator = ","

      End If

      If amod_prop_com_tbo_hrs >= 0 Then

        modelQuery.Append(sSeperator + " amod_prop_com_tbo_hrs = @amod_prop_com_tbo_hrs")
        SqlCommand.Parameters.AddWithValue("@amod_prop_com_tbo_hrs", amod_prop_com_tbo_hrs.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_other_config_note.Trim) Then

        modelQuery.Append(sSeperator + " amod_other_config_note = @amod_other_config_note")
        SqlCommand.Parameters.AddWithValue("@amod_other_config_note", amod_other_config_note.Trim)
        sSeperator = ","

      End If

      If amod_cabinsize_height_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_height_feet = @amod_cabinsize_height_feet")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_height_feet", amod_cabinsize_height_feet.ToString)
        sSeperator = ","

      End If

      If amod_cabinsize_height_inches >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_height_inches = @amod_cabinsize_height_inches")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_height_inches", amod_cabinsize_height_inches.ToString)
        sSeperator = ","

      End If

      If amod_cabinsize_width_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_width_feet = @amod_cabinsize_width_feet")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_width_feet", amod_cabinsize_width_feet.ToString)
        sSeperator = ","

      End If

      If amod_cabinsize_width_inches >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_width_inches = @amod_cabinsize_width_inches")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_width_inches", amod_cabinsize_width_inches.ToString)
        sSeperator = ","

      End If

      If amod_cabinsize_length_feet >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_length_feet = @amod_cabinsize_length_feet")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_length_feet", amod_cabinsize_length_feet.ToString)
        sSeperator = ","

      End If

      If amod_cabinsize_length_inches >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabinsize_length_inches = @amod_cabinsize_length_inches")
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_length_inches", amod_cabinsize_length_inches.ToString)
        sSeperator = ","

      End If

      If amod_cabin_volume >= 0 Then

        modelQuery.Append(sSeperator + " amod_cabin_volume = @amod_cabin_volume")
        SqlCommand.Parameters.AddWithValue("@amod_cabin_volume", amod_cabin_volume.ToString)
        sSeperator = ","

      End If

      If amod_baggage_volume >= 0 Then

        modelQuery.Append(sSeperator + " amod_baggage_volume = @amod_baggage_volume")
        SqlCommand.Parameters.AddWithValue("@amod_baggage_volume", amod_baggage_volume.ToString)
        sSeperator = ","

      End If

      If amod_fuel_cap_std_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuel_cap_std_weight = @amod_fuel_cap_std_weight")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_std_weight", amod_fuel_cap_std_weight.ToString)
        sSeperator = ","

      End If

      If amod_fuel_cap_std_gal >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuel_cap_std_gal = @amod_fuel_cap_std_gal")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_std_gal", amod_fuel_cap_std_gal.ToString)
        sSeperator = ","

      End If

      If amod_fuel_cap_opt_weight >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuel_cap_opt_weight = @amod_fuel_cap_opt_weight")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_opt_weight", amod_fuel_cap_opt_weight.ToString)
        sSeperator = ","

      End If

      If amod_fuel_cap_opt_gal >= 0 Then

        modelQuery.Append(sSeperator + " amod_fuel_cap_opt_gal = @amod_fuel_cap_opt_gal")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_opt_gal", amod_fuel_cap_opt_gal.ToString)
        sSeperator = ","

      End If

      If amod_stall_vs >= 0 Then

        modelQuery.Append(sSeperator + " amod_stall_vs = @amod_stall_vs")
        SqlCommand.Parameters.AddWithValue("@amod_stall_vs", amod_stall_vs.ToString)
        sSeperator = ","

      End If

      If amod_stall_vso >= 0 Then

        modelQuery.Append(sSeperator + " amod_stall_vso = @amod_stall_vso")
        SqlCommand.Parameters.AddWithValue("@amod_stall_vso", amod_stall_vso.ToString)
        sSeperator = ","

      End If

      If amod_cruis_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_cruis_speed = @amod_cruis_speed")
        SqlCommand.Parameters.AddWithValue("@amod_cruis_speed", amod_cruis_speed.ToString)
        sSeperator = ","

      End If

      If amod_max_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_max_speed = @amod_max_speed")
        SqlCommand.Parameters.AddWithValue("@amod_max_speed", amod_max_speed.ToString)
        sSeperator = ","

      End If

      If amod_vne_maxop_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_vne_maxop_speed = @amod_vne_maxop_speed")
        SqlCommand.Parameters.AddWithValue("@amod_vne_maxop_speed", amod_vne_maxop_speed.ToString)
        sSeperator = ","

      End If

      If amod_v1_takeoff_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_v1_takeoff_speed = @amod_v1_takeoff_speed")
        SqlCommand.Parameters.AddWithValue("@amod_v1_takeoff_speed", amod_v1_takeoff_speed.ToString)
        sSeperator = ","

      End If

      If amod_vfe_max_flap_extended_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_vfe_max_flap_extended_speed = @amod_vfe_max_flap_extended_speed")
        SqlCommand.Parameters.AddWithValue("@amod_vfe_max_flap_extended_speed", amod_vfe_max_flap_extended_speed.ToString)
        sSeperator = ","

      End If

      If amod_vle_max_landing_gear_ext_speed >= 0 Then

        modelQuery.Append(sSeperator + " amod_vle_max_landing_gear_ext_speed = @amod_vle_max_landing_gear_ext_speed")
        SqlCommand.Parameters.AddWithValue("@amod_vle_max_landing_gear_ext_speed", amod_vle_max_landing_gear_ext_speed.ToString)
        sSeperator = ","

      End If

      If amod_field_length >= 0 Then

        modelQuery.Append(sSeperator + " amod_field_length = @amod_field_length")
        SqlCommand.Parameters.AddWithValue("@amod_field_length", amod_field_length.ToString)
        sSeperator = ","

      End If

      If amod_takeoff_ali >= 0 Then

        modelQuery.Append(sSeperator + " amod_takeoff_ali = @amod_takeoff_ali")
        SqlCommand.Parameters.AddWithValue("@amod_takeoff_ali", amod_takeoff_ali.ToString)
        sSeperator = ","

      End If

      If amod_takeoff_500 >= 0 Then

        modelQuery.Append(sSeperator + " amod_takeoff_500 = @amod_takeoff_500")
        SqlCommand.Parameters.AddWithValue("@amod_takeoff_500", amod_takeoff_500.ToString)
        sSeperator = ","

      End If

      If amod_number_of_engines >= 0 Then

        modelQuery.Append(sSeperator + " amod_number_of_engines = @amod_number_of_engines")
        SqlCommand.Parameters.AddWithValue("@amod_number_of_engines", amod_number_of_engines.ToString)
        sSeperator = ","

      End If

      If amod_engine_thrust_lbs >= 0 Then

        modelQuery.Append(sSeperator + " amod_engine_thrust_lbs = @amod_engine_thrust_lbs")
        SqlCommand.Parameters.AddWithValue("@amod_engine_thrust_lbs", amod_engine_thrust_lbs.ToString)
        sSeperator = ","

      End If

      If amod_engine_shaft >= 0 Then

        modelQuery.Append(sSeperator + " amod_engine_shaft = @amod_engine_shaft")
        SqlCommand.Parameters.AddWithValue("@amod_engine_shaft", amod_engine_shaft.ToString)
        sSeperator = ","

      End If

      If amod_engine_com_tbo_hrs >= 0 Then

        modelQuery.Append(sSeperator + " amod_engine_com_tbo_hrs = @amod_engine_com_tbo_hrs")
        SqlCommand.Parameters.AddWithValue("@amod_engine_com_tbo_hrs", amod_engine_com_tbo_hrs.ToString)
        sSeperator = ","

      End If

      If amod_main_rotor_1_blade_count >= 0 Then

        modelQuery.Append(sSeperator + " amod_main_rotor_1_blade_count = @amod_main_rotor_1_blade_count")
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_1_blade_count", amod_main_rotor_1_blade_count.ToString)
        sSeperator = ","

      End If

      If amod_main_rotor_1_blade_diameter >= 0 Then

        modelQuery.Append(sSeperator + " amod_main_rotor_1_blade_diameter = @amod_main_rotor_1_blade_diameter")
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_1_blade_diameter", amod_main_rotor_1_blade_diameter.ToString)
        sSeperator = ","

      End If

      If amod_main_rotor_2_blade_count >= 0 Then

        modelQuery.Append(sSeperator + " amod_main_rotor_2_blade_count = @amod_main_rotor_2_blade_count")
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_2_blade_count", amod_main_rotor_2_blade_count.ToString)
        sSeperator = ","

      End If

      If amod_main_rotor_2_blade_diameter >= 0 Then

        modelQuery.Append(sSeperator + " amod_main_rotor_2_blade_diameter = @amod_main_rotor_2_blade_diameter")
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_2_blade_diameter", amod_main_rotor_2_blade_diameter.ToString)
        sSeperator = ","

      End If

      If amod_tail_rotor_blade_count >= 0 Then

        modelQuery.Append(sSeperator + " amod_tail_rotor_blade_count = @amod_tail_rotor_blade_count")
        SqlCommand.Parameters.AddWithValue("@amod_tail_rotor_blade_count", amod_tail_rotor_blade_count.ToString)
        sSeperator = ","

      End If

      If amod_tail_rotor_blade_diameter >= 0 Then

        modelQuery.Append(sSeperator + " amod_tail_rotor_blade_diameter = @amod_tail_rotor_blade_diameter")
        SqlCommand.Parameters.AddWithValue("@amod_tail_rotor_blade_diameter", amod_tail_rotor_blade_diameter.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(amod_rotor_anti_torque_system.Trim) Then

        modelQuery.Append(sSeperator + " amod_rotor_anti_torque_system = @amod_rotor_anti_torque_system")
        SqlCommand.Parameters.AddWithValue("@amod_rotor_anti_torque_system", amod_rotor_anti_torque_system.Trim)
        sSeperator = ","

      End If

      modelQuery.Append(" WHERE amod_id = @amod_id")
      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertModelPerfSpecsClass()

    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("INSERT INTO Aircraft_Model (amod_fuselage_length, amod_fuselage_height, amod_fuselage_wingspan, amod_fuselage_width, amod_number_of_crew,")
      modelQuery.Append(" amod_number_of_passengers, amod_pressure, amod_max_ramp_weight, amod_max_takeoff_weight, amod_zero_fuel_weight, amod_weight_eow,")
      modelQuery.Append(" amod_basic_op_weight, amod_max_landing_weight, amod_ifr_certification, amod_climb_normal_feet, amod_climb_engout_feet, amod_ceiling_feet,")
      modelQuery.Append(" amod_climb_hoge, amod_climb_hige, amod_max_range_miles, amod_range_tanks_full, amod_range_seats_full, amod_range_4_passenger,")
      modelQuery.Append(" amod_range_8_passenger, amod_number_of_props, amod_prop_model_name, amod_prop_mfr_name, amod_prop_com_tbo_hrs,")
      modelQuery.Append(" amod_other_config_note, amod_cabinsize_height_feet, amod_cabinsize_height_inches, amod_cabinsize_width_feet, amod_cabinsize_width_inches,")
      modelQuery.Append(" amod_cabinsize_length_feet, amod_cabinsize_length_inches, amod_cabin_volume, amod_baggage_volume, amod_fuel_cap_std_weight,")
      modelQuery.Append(" amod_fuel_cap_std_gal, amod_fuel_cap_opt_weight, amod_fuel_cap_opt_gal, amod_stall_vs, amod_stall_vso,")
      modelQuery.Append(" amod_cruis_speed, amod_max_speed, amod_vne_maxop_speed, amod_v1_takeoff_speed, amod_vfe_max_flap_extended_speed,")
      modelQuery.Append(" amod_vle_max_landing_gear_ext_speed, amod_field_length, amod_takeoff_ali, amod_takeoff_500, amod_number_of_engines,")
      modelQuery.Append(" amod_engine_thrust_lbs, amod_engine_shaft, amod_engine_com_tbo_hrs, amod_main_rotor_1_blade_count, amod_main_rotor_1_blade_diameter,")
      modelQuery.Append(" amod_main_rotor_2_blade_count, amod_main_rotor_2_blade_diameter, amod_tail_rotor_blade_count, amod_tail_rotor_blade_diameter, amod_rotor_anti_torque_system")
      modelQuery.Append(") VALUES (@amod_fuselage_length, @amod_fuselage_height, @amod_fuselage_wingspan, @amod_fuselage_width, @amod_number_of_crew,")
      modelQuery.Append(" @amod_number_of_passengers, @amod_pressure, @amod_max_ramp_weight, @amod_max_takeoff_weight, @amod_zero_fuel_weight, @amod_weight_eow,")
      modelQuery.Append(" @amod_basic_op_weight, @amod_max_landing_weight, @amod_ifr_certification, @amod_climb_normal_feet, @amod_climb_engout_feet, @amod_ceiling_feet,")
      modelQuery.Append(" @amod_climb_hoge, @amod_climb_hige, @amod_max_range_miles, @amod_range_tanks_full, @amod_range_seats_full, @amod_range_4_passenger,")
      modelQuery.Append(" @amod_range_8_passenger, @amod_number_of_props, @amod_prop_model_name, @amod_prop_mfr_name, @amod_prop_com_tbo_hrs,")
      modelQuery.Append(" @amod_other_config_note, @amod_cabinsize_height_feet, @amod_cabinsize_height_inches, @amod_cabinsize_width_feet, @amod_cabinsize_width_inches,")
      modelQuery.Append(" @amod_cabinsize_length_feet, @amod_cabinsize_length_inches, @amod_cabin_volume, @amod_baggage_volume, @amod_fuel_cap_std_weight,")
      modelQuery.Append(" @amod_fuel_cap_std_gal, @amod_fuel_cap_opt_weight, @amod_fuel_cap_opt_gal, @amod_stall_vs, @amod_stall_vso,")
      modelQuery.Append(" @amod_cruis_speed, @amod_max_speed, @amod_vne_maxop_speed, @amod_v1_takeoff_speed, @amod_vfe_max_flap_extended_speed,")
      modelQuery.Append(" @amod_vle_max_landing_gear_ext_speed, @amod_field_length, @amod_takeoff_ali, @amod_takeoff_500, @amod_number_of_engines,")
      modelQuery.Append(" @amod_engine_thrust_lbs, @amod_engine_shaft, @amod_engine_com_tbo_hrs, @amod_main_rotor_1_blade_count, @amod_main_rotor_1_blade_diameter,")
      modelQuery.Append(" @amod_main_rotor_2_blade_count, @amod_main_rotor_2_blade_diameter, @amod_tail_rotor_blade_count, @amod_tail_rotor_blade_diameter, @amod_rotor_anti_torque_system")
      modelQuery.Append(") WHERE amod_id = @amod_id")


      If amod_fuselage_length >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_length", amod_fuselage_length.ToString)
      End If

      If amod_fuselage_height >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_height", amod_fuselage_height.ToString)
      End If

      If amod_fuselage_wingspan >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_wingspan", amod_fuselage_wingspan.ToString)
      End If

      If amod_fuselage_width >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuselage_width", amod_fuselage_width.ToString)
      End If

      If amod_number_of_crew >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_number_of_crew", amod_number_of_crew.ToString)
      End If

      If amod_number_of_passengers >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_number_of_passengers", amod_number_of_passengers.ToString)
      End If

      If amod_pressure >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_pressure", amod_pressure.ToString)
      End If

      If amod_max_ramp_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_max_ramp_weight", amod_max_ramp_weight.ToString)
      End If

      If amod_max_takeoff_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_max_takeoff_weight", amod_max_takeoff_weight.ToString)
      End If

      If amod_zero_fuel_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_zero_fuel_weight", amod_zero_fuel_weight.ToString)
      End If

      If amod_weight_eow >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_weight_eow", amod_weight_eow.ToString)
      End If

      If amod_basic_op_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_basic_op_weight", amod_basic_op_weight.ToString)
      End If

      If amod_max_landing_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_max_landing_weight", amod_max_landing_weight.ToString)
      End If

      If Not String.IsNullOrEmpty(amod_ifr_certification.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_ifr_certification", amod_ifr_certification.Trim)
      End If

      If amod_climb_normal_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_climb_normal_feet", amod_climb_normal_feet.ToString)
      End If

      If amod_climb_engout_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_climb_engout_feet", amod_climb_engout_feet.ToString)
      End If

      If amod_ceiling_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_ceiling_feet", amod_ceiling_feet.ToString)
      End If

      If amod_climb_hoge >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_climb_hoge", amod_climb_hoge.ToString)
      End If

      If amod_climb_hige >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_climb_hige", amod_climb_hige.ToString)
      End If

      If amod_max_range_miles >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_max_range_miles", amod_max_range_miles.ToString)
      End If

      If amod_range_tanks_full >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_range_tanks_full", amod_range_tanks_full.ToString)
      End If

      If amod_range_seats_full >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_range_seats_full", amod_range_seats_full.ToString)
      End If

      If amod_range_4_passenger >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_range_4_passenger", amod_range_4_passenger.ToString)
      End If

      If amod_range_8_passenger >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_range_8_passenger", amod_range_8_passenger.ToString)
      End If

      If amod_number_of_props >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_number_of_props", amod_number_of_props.ToString)
      End If

      If Not String.IsNullOrEmpty(amod_prop_model_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_prop_model_name", amod_prop_model_name.Trim)
      End If

      If Not String.IsNullOrEmpty(amod_prop_mfr_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_prop_mfr_name", amod_prop_mfr_name.Trim)
      End If

      If amod_prop_com_tbo_hrs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_prop_com_tbo_hrs", amod_prop_com_tbo_hrs.ToString)
      End If

      If Not String.IsNullOrEmpty(amod_other_config_note.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_other_config_note", amod_other_config_note.Trim)
      End If

      If amod_cabinsize_height_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_height_feet", amod_cabinsize_height_feet.ToString)
      End If

      If amod_cabinsize_height_inches >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_height_inches", amod_cabinsize_height_inches.ToString)
      End If

      If amod_cabinsize_width_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_width_feet", amod_cabinsize_width_feet.ToString)
      End If

      If amod_cabinsize_width_inches >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_width_inches", amod_cabinsize_width_inches.ToString)
      End If

      If amod_cabinsize_length_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_length_feet", amod_cabinsize_length_feet.ToString)
      End If

      If amod_cabinsize_length_inches >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_length_inches", amod_cabinsize_length_inches.ToString)
      End If

      If amod_cabin_volume >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabin_volume", amod_cabin_volume.ToString)
      End If

      If amod_baggage_volume >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_baggage_volume", amod_baggage_volume.ToString)
      End If

      If amod_fuel_cap_std_weight >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_std_weight", amod_fuel_cap_std_weight.ToString)
      End If

      If amod_cabinsize_width_inches >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_std_gal", amod_cabinsize_width_inches.ToString)
      End If

      If amod_cabinsize_length_feet >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_opt_weight", amod_cabinsize_length_feet.ToString)
      End If

      If amod_cabinsize_length_inches >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cabinsize_length_inches", amod_cabinsize_length_inches.ToString)
      End If

      If amod_fuel_cap_opt_gal >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_cap_opt_gal", amod_fuel_cap_opt_gal.ToString)
      End If

      If amod_stall_vs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_stall_vs", amod_stall_vs.ToString)
      End If

      If amod_stall_vso >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_stall_vso", amod_stall_vso.ToString)
      End If

      If amod_cruis_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cruis_speed", amod_cruis_speed.ToString)
      End If

      If amod_max_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_max_speed", amod_max_speed.ToString)
      End If

      If amod_vne_maxop_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_vne_maxop_speed", amod_vne_maxop_speed.ToString)
      End If

      If amod_v1_takeoff_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_v1_takeoff_speed", amod_v1_takeoff_speed.ToString)
      End If

      If amod_vfe_max_flap_extended_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_vfe_max_flap_extended_speed", amod_vfe_max_flap_extended_speed.ToString)
      End If

      If amod_vle_max_landing_gear_ext_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_vle_max_landing_gear_ext_speed", amod_vle_max_landing_gear_ext_speed.ToString)
      End If

      If amod_field_length >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_field_length", amod_field_length.ToString)
      End If

      If amod_takeoff_ali >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_takeoff_ali", amod_takeoff_ali.ToString)
      End If

      If amod_takeoff_500 >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_takeoff_500", amod_takeoff_500.ToString)
      End If

      If amod_number_of_engines >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_number_of_engines", amod_number_of_engines.ToString)
      End If

      If amod_engine_thrust_lbs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_engine_thrust_lbs", amod_engine_thrust_lbs.ToString)
      End If

      If amod_engine_shaft >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_engine_shaft", amod_engine_shaft.ToString)
      End If

      If amod_engine_com_tbo_hrs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_engine_com_tbo_hrs", amod_engine_com_tbo_hrs.ToString)
      End If

      If amod_main_rotor_1_blade_count >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_1_blade_count", amod_main_rotor_1_blade_count.ToString)
      End If

      If amod_main_rotor_1_blade_diameter >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_1_blade_diameter", amod_main_rotor_1_blade_diameter.ToString)
      End If

      If amod_main_rotor_2_blade_count >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_2_blade_count", amod_main_rotor_2_blade_count.ToString)
      End If

      If amod_main_rotor_2_blade_diameter >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_main_rotor_2_blade_diameter", amod_main_rotor_2_blade_diameter.ToString)
      End If

      If amod_tail_rotor_blade_count >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tail_rotor_blade_count", amod_tail_rotor_blade_count.ToString)
      End If

      If amod_tail_rotor_blade_diameter >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tail_rotor_blade_diameter", amod_tail_rotor_blade_diameter.ToString)
      End If

      If Not String.IsNullOrEmpty(amod_rotor_anti_torque_system.Trim) Then
        SqlCommand.Parameters.AddWithValue("@amod_rotor_anti_torque_system", amod_rotor_anti_torque_system.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Overrides Function Equals(obj As Object) As Boolean
    Dim [class] = TryCast(obj, homebaseModelPerfSpecsClass)
    Return [class] IsNot Nothing AndAlso
           amod_id = [class].amod_id AndAlso
           amod_airframe_type_code = [class].amod_airframe_type_code AndAlso
           amod_fuselage_length = [class].amod_fuselage_length AndAlso
           amod_fuselage_height = [class].amod_fuselage_height AndAlso
           amod_fuselage_wingspan = [class].amod_fuselage_wingspan AndAlso
           amod_fuselage_width = [class].amod_fuselage_width AndAlso
           amod_number_of_crew = [class].amod_number_of_crew AndAlso
           amod_number_of_passengers = [class].amod_number_of_passengers AndAlso
           amod_pressure = [class].amod_pressure AndAlso
           amod_max_ramp_weight = [class].amod_max_ramp_weight AndAlso
           amod_max_takeoff_weight = [class].amod_max_takeoff_weight AndAlso
           amod_zero_fuel_weight = [class].amod_zero_fuel_weight AndAlso
           amod_weight_eow = [class].amod_weight_eow AndAlso
           amod_basic_op_weight = [class].amod_basic_op_weight AndAlso
           amod_max_landing_weight = [class].amod_max_landing_weight AndAlso
           amod_ifr_certification = [class].amod_ifr_certification AndAlso
           amod_climb_normal_feet = [class].amod_climb_normal_feet AndAlso
           amod_climb_engout_feet = [class].amod_climb_engout_feet AndAlso
           amod_ceiling_feet = [class].amod_ceiling_feet AndAlso
           amod_climb_hoge = [class].amod_climb_hoge AndAlso
           amod_climb_hige = [class].amod_climb_hige AndAlso
           amod_max_range_miles = [class].amod_max_range_miles AndAlso
           amod_range_tanks_full = [class].amod_range_tanks_full AndAlso
           amod_range_seats_full = [class].amod_range_seats_full AndAlso
           amod_range_4_passenger = [class].amod_range_4_passenger AndAlso
           amod_range_8_passenger = [class].amod_range_8_passenger AndAlso
           amod_number_of_props = [class].amod_number_of_props AndAlso
           amod_prop_model_name = [class].amod_prop_model_name AndAlso
           amod_prop_mfr_name = [class].amod_prop_mfr_name AndAlso
           amod_prop_com_tbo_hrs = [class].amod_prop_com_tbo_hrs AndAlso
           amod_other_config_note = [class].amod_other_config_note AndAlso
           amod_cabinsize_height_feet = [class].amod_cabinsize_height_feet AndAlso
           amod_cabinsize_height_inches = [class].amod_cabinsize_height_inches AndAlso
           amod_cabinsize_width_feet = [class].amod_cabinsize_width_feet AndAlso
           amod_cabinsize_width_inches = [class].amod_cabinsize_width_inches AndAlso
           amod_cabinsize_length_feet = [class].amod_cabinsize_length_feet AndAlso
           amod_cabinsize_length_inches = [class].amod_cabinsize_length_inches AndAlso
           amod_cabin_volume = [class].amod_cabin_volume AndAlso
           amod_baggage_volume = [class].amod_baggage_volume AndAlso
           amod_fuel_cap_std_weight = [class].amod_fuel_cap_std_weight AndAlso
           amod_fuel_cap_std_gal = [class].amod_fuel_cap_std_gal AndAlso
           amod_fuel_cap_opt_weight = [class].amod_fuel_cap_opt_weight AndAlso
           amod_fuel_cap_opt_gal = [class].amod_fuel_cap_opt_gal AndAlso
           amod_stall_vs = [class].amod_stall_vs AndAlso
           amod_stall_vso = [class].amod_stall_vso AndAlso
           amod_cruis_speed = [class].amod_cruis_speed AndAlso
           amod_max_speed = [class].amod_max_speed AndAlso
           amod_vne_maxop_speed = [class].amod_vne_maxop_speed AndAlso
           amod_v1_takeoff_speed = [class].amod_v1_takeoff_speed AndAlso
           amod_vfe_max_flap_extended_speed = [class].amod_vfe_max_flap_extended_speed AndAlso
           amod_vle_max_landing_gear_ext_speed = [class].amod_vle_max_landing_gear_ext_speed AndAlso
           amod_field_length = [class].amod_field_length AndAlso
           amod_takeoff_ali = [class].amod_takeoff_ali AndAlso
           amod_takeoff_500 = [class].amod_takeoff_500 AndAlso
           amod_number_of_engines = [class].amod_number_of_engines AndAlso
           amod_engine_thrust_lbs = [class].amod_engine_thrust_lbs AndAlso
           amod_engine_shaft = [class].amod_engine_shaft AndAlso
           amod_engine_com_tbo_hrs = [class].amod_engine_com_tbo_hrs AndAlso
           amod_main_rotor_1_blade_count = [class].amod_main_rotor_1_blade_count AndAlso
           amod_main_rotor_1_blade_diameter = [class].amod_main_rotor_1_blade_diameter AndAlso
           amod_main_rotor_2_blade_count = [class].amod_main_rotor_2_blade_count AndAlso
           amod_main_rotor_2_blade_diameter = [class].amod_main_rotor_2_blade_diameter AndAlso
           amod_tail_rotor_blade_count = [class].amod_tail_rotor_blade_count AndAlso
           amod_tail_rotor_blade_diameter = [class].amod_tail_rotor_blade_diameter AndAlso
           amod_rotor_anti_torque_system = [class].amod_rotor_anti_torque_system
  End Function

  Public Shared Operator =(class1 As homebaseModelPerfSpecsClass, class2 As homebaseModelPerfSpecsClass) As Boolean
    Return EqualityComparer(Of homebaseModelPerfSpecsClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseModelPerfSpecsClass, class2 As homebaseModelPerfSpecsClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class

<System.Serializable()> Public Class homebaseModelOpCostsClass

  Public Property amod_id() As Long
  Public Property amod_airframe_type_code() As String
  Public Property amod_fuel_gal_cost() As Double
  Public Property amod_fuel_add_cost() As Double
  Public Property amod_fuel_burn_rate() As Double
  Public Property amod_fuel_tot_cost() As Double
  Public Property amod_maint_lab_cost() As Double
  Public Property amod_maint_parts_cost() As Double
  Public Property amod_maint_labor_cost_man_hours_multiplier() As Double
  Public Property amod_maint_parts_cost_man_hours_multiplier() As Double
  Public Property amod_maint_tot_cost() As Double
  Public Property amod_engine_ovh_cost() As Double
  Public Property amod_thrust_rev_ovh_cost() As Double
  Public Property amod_land_park_cost() As Double
  Public Property amod_crew_exp_cost() As Double
  Public Property amod_supplies_cost() As Double
  Public Property amod_misc_flight_cost() As Double
  Public Property amod_tot_hour_direct_cost() As Double
  Public Property amod_avg_block_speed() As Double
  Public Property amod_tot_stat_mile_cost() As Double
  Public Property amod_capt_salary_cost() As Double
  Public Property amod_cpilot_salary_cost() As Double
  Public Property amod_crew_benefit_cost() As Double
  Public Property amod_tot_crew_salary_cost() As Double
  Public Property amod_hangar_cost() As Double
  Public Property amod_hull_insurance_cost As Double
  Public Property amod_liability_insurance_cost() As Double
  Public Property amod_insurance_cost() As Double
  Public Property amod_misc_train_cost() As Double
  Public Property amod_misc_modern_cost() As Double
  Public Property amod_misc_naveq_cost() As Double
  Public Property amod_tot_misc_ovh_cost() As Double
  Public Property amod_deprec_cost() As Double
  Public Property amod_tot_fixed_cost() As Double
  Public Property amod_variable_costs() As Double
  Public Property amod_number_of_seats() As Integer
  Public Property amod_annual_miles() As Integer
  Public Property amod_annual_hours() As Integer
  Public Property amod_tot_direct_cost() As Double
  Public Property amod_tot_df_annual_cost() As Double
  Public Property amod_tot_df_hour_cost() As Double
  Public Property amod_tot_df_statmile_cost() As Double
  Public Property amod_tot_df_seat_cost() As Double
  Public Property amod_tot_nd_annual_cost() As Double
  Public Property amod_tot_nd_hour_cost() As Double
  Public Property amod_tot_nd_statmile_cost() As Double
  Public Property amod_tot_nd_seat_cost() As Double

  Sub New()

    amod_id = 0
    amod_airframe_type_code = ""

    amod_fuel_gal_cost = 0.0
    amod_fuel_add_cost = 0.0
    amod_fuel_burn_rate = 0.0
    amod_fuel_tot_cost = 0.0

    amod_maint_lab_cost = 0.0
    amod_maint_parts_cost = 0.0
    amod_maint_labor_cost_man_hours_multiplier = 0.0
    amod_maint_parts_cost_man_hours_multiplier = 0.0

    amod_maint_tot_cost = 0.0

    amod_engine_ovh_cost = 0.0
    amod_thrust_rev_ovh_cost = 0.0

    amod_land_park_cost = 0.0
    amod_crew_exp_cost = 0.0
    amod_supplies_cost = 0.0
    amod_misc_flight_cost = 0.0

    amod_tot_hour_direct_cost = 0.0
    amod_avg_block_speed = 0.0
    amod_tot_stat_mile_cost = 0.0

    amod_capt_salary_cost = 0.0
    amod_cpilot_salary_cost = 0.0
    amod_crew_benefit_cost = 0.0
    amod_tot_crew_salary_cost = 0.0

    amod_hangar_cost = 0.0

    amod_hull_insurance_cost = 0.0
    amod_liability_insurance_cost = 0.0
    amod_insurance_cost = 0.0

    amod_misc_train_cost = 0.0
    amod_misc_modern_cost = 0.0
    amod_misc_naveq_cost = 0.0
    amod_tot_misc_ovh_cost = 0.0

    amod_deprec_cost = 0.0
    amod_tot_fixed_cost = 0.0
    amod_variable_costs = 0.0

    amod_number_of_seats = 0
    amod_annual_miles = 0
    amod_annual_hours = 0

    amod_tot_direct_cost = 0.0
    amod_tot_df_annual_cost = 0.0

    amod_tot_df_hour_cost = 0.0
    amod_tot_df_statmile_cost = 0.0
    amod_tot_df_seat_cost = 0.0

    amod_tot_nd_annual_cost = 0.0
    amod_tot_nd_hour_cost = 0.0
    amod_tot_nd_statmile_cost = 0.0
    amod_tot_nd_seat_cost = 0.0

  End Sub

  Sub New(ByVal nModelID As Long)

    amod_id = nModelID
    amod_airframe_type_code = ""

    amod_fuel_gal_cost = 0.0
    amod_fuel_add_cost = 0.0
    amod_fuel_burn_rate = 0.0
    amod_fuel_tot_cost = 0.0

    amod_maint_lab_cost = 0.0
    amod_maint_parts_cost = 0.0
    amod_maint_labor_cost_man_hours_multiplier = 0.0
    amod_maint_parts_cost_man_hours_multiplier = 0.0

    amod_maint_tot_cost = 0.0

    amod_engine_ovh_cost = 0.0
    amod_thrust_rev_ovh_cost = 0.0

    amod_land_park_cost = 0.0
    amod_crew_exp_cost = 0.0
    amod_supplies_cost = 0.0
    amod_misc_flight_cost = 0.0

    amod_tot_hour_direct_cost = 0.0
    amod_avg_block_speed = 0.0
    amod_tot_stat_mile_cost = 0.0

    amod_capt_salary_cost = 0.0
    amod_cpilot_salary_cost = 0.0
    amod_crew_benefit_cost = 0.0
    amod_tot_crew_salary_cost = 0.0

    amod_hangar_cost = 0.0

    amod_hull_insurance_cost = 0.0
    amod_liability_insurance_cost = 0.0
    amod_insurance_cost = 0.0

    amod_misc_train_cost = 0.0
    amod_misc_modern_cost = 0.0
    amod_misc_naveq_cost = 0.0
    amod_tot_misc_ovh_cost = 0.0

    amod_deprec_cost = 0.0
    amod_tot_fixed_cost = 0.0
    amod_variable_costs = 0.0

    amod_number_of_seats = 0
    amod_annual_miles = 0
    amod_annual_hours = 0

    amod_tot_direct_cost = 0.0
    amod_tot_df_annual_cost = 0.0

    amod_tot_df_hour_cost = 0.0
    amod_tot_df_statmile_cost = 0.0
    amod_tot_df_seat_cost = 0.0

    amod_tot_nd_annual_cost = 0.0
    amod_tot_nd_hour_cost = 0.0
    amod_tot_nd_statmile_cost = 0.0
    amod_tot_nd_seat_cost = 0.0

  End Sub

  Public Function getModelDataTable(ByVal inModelID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      modelQuery.Append("SELECT Aircraft_Model.*, acwgtcls_name, amjiqs_cat_desc, amjiqs_cat_code FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH(NOLOCK)")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model_JIQ_Size WITH(NOLOCK) ON amod_jniq_size = amjiqs_cat_code")
      modelQuery.Append(" LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Weight_Class WITH(NOLOCK) ON amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code")
      modelQuery.Append(" WHERE amod_id = @amod_id")

      SqlCommand.Parameters.AddWithValue("@amod_id", inModelID.ToString.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = modelQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub fillModelOpCostsClass()

    Dim resultsTable As New DataTable

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      resultsTable = getModelDataTable(amod_id)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("amod_airframe_type_code"))) Then
              amod_airframe_type_code = r.Item("amod_airframe_type_code").ToString.Trim
            End If

            ' DIRECT COSTS/HOUR
            ' FUEL
            If Not (IsDBNull(r("amod_fuel_tot_cost"))) Then
              amod_fuel_tot_cost = CDbl(r.Item("amod_fuel_tot_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_gal_cost"))) Then
              amod_fuel_gal_cost = CDbl(r.Item("amod_fuel_gal_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_add_cost"))) Then
              amod_fuel_add_cost = CDbl(r.Item("amod_fuel_add_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_fuel_burn_rate"))) Then
              amod_fuel_burn_rate = CDbl(r.Item("amod_fuel_burn_rate").ToString.Trim)
            End If

            ' MAINTENANCE
            If Not (IsDBNull(r("amod_maint_tot_cost"))) Then
              amod_maint_tot_cost = CDbl(r.Item("amod_maint_tot_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_maint_lab_cost"))) Then
              amod_maint_lab_cost = CDbl(r.Item("amod_maint_lab_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_maint_labor_cost_man_hours_multiplier"))) Then
              amod_maint_labor_cost_man_hours_multiplier = CDbl(r.Item("amod_maint_labor_cost_man_hours_multiplier").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_maint_parts_cost"))) Then
              amod_maint_parts_cost = CDbl(r.Item("amod_maint_parts_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_maint_parts_cost_man_hours_multiplier"))) Then
              amod_maint_parts_cost_man_hours_multiplier = CDbl(r.Item("amod_maint_parts_cost_man_hours_multiplier").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_engine_ovh_cost"))) Then
              amod_engine_ovh_cost = CDbl(r.Item("amod_engine_ovh_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_thrust_rev_ovh_cost"))) Then
              amod_thrust_rev_ovh_cost = CDbl(r.Item("amod_thrust_rev_ovh_cost").ToString.Trim)
            End If

            ' MISC. FLIGHT EXP.
            If Not (IsDBNull(r("amod_misc_flight_cost"))) Then
              amod_misc_flight_cost = CDbl(r.Item("amod_misc_flight_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_land_park_cost"))) Then
              amod_land_park_cost = CDbl(r.Item("amod_land_park_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_crew_exp_cost"))) Then
              amod_crew_exp_cost = CDbl(r.Item("amod_crew_exp_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_supplies_cost"))) Then
              amod_supplies_cost = CDbl(r.Item("amod_supplies_cost").ToString.Trim)
            End If

            ' TOTAL DIRECT COSTS
            If Not (IsDBNull(r("amod_tot_hour_direct_cost"))) Then
              amod_tot_hour_direct_cost = CDbl(r.Item("amod_tot_hour_direct_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_avg_block_speed"))) Then
              amod_avg_block_speed = CDbl(r.Item("amod_avg_block_speed").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_stat_mile_cost"))) Then
              amod_tot_stat_mile_cost = CDbl(r.Item("amod_tot_stat_mile_cost").ToString.Trim)
            End If

            ' ANNUAL FIXED COSTS
            ' CREW SALARIES
            If Not (IsDBNull(r("amod_tot_crew_salary_cost"))) Then
              amod_tot_crew_salary_cost = CDbl(r.Item("amod_tot_crew_salary_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_capt_salary_cost"))) Then
              amod_capt_salary_cost = CDbl(r.Item("amod_capt_salary_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_cpilot_salary_cost"))) Then
              amod_cpilot_salary_cost = CDbl(r.Item("amod_cpilot_salary_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_crew_benefit_cost"))) Then
              amod_crew_benefit_cost = CDbl(r.Item("amod_crew_benefit_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_hangar_cost"))) Then
              amod_hangar_cost = CDbl(r.Item("amod_hangar_cost").ToString.Trim)
            End If

            ' INSURANCE
            If Not (IsDBNull(r("amod_insurance_cost"))) Then
              amod_insurance_cost = CDbl(r.Item("amod_insurance_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_hull_insurance_cost"))) Then
              amod_hull_insurance_cost = CDbl(r.Item("amod_hull_insurance_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_liability_insurance_cost"))) Then
              amod_liability_insurance_cost = CDbl(r.Item("amod_liability_insurance_cost").ToString.Trim)
            End If

            ' MISC. OVERHEAD
            If Not (IsDBNull(r("amod_tot_misc_ovh_cost"))) Then
              amod_tot_misc_ovh_cost = CDbl(r.Item("amod_tot_misc_ovh_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_misc_train_cost"))) Then
              amod_misc_train_cost = CDbl(r.Item("amod_misc_train_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_misc_modern_cost"))) Then
              amod_misc_modern_cost = CDbl(r.Item("amod_misc_modern_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_misc_naveq_cost"))) Then
              amod_misc_naveq_cost = CDbl(r.Item("amod_misc_naveq_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_deprec_cost"))) Then
              amod_deprec_cost = CDbl(r.Item("amod_deprec_cost").ToString.Trim)
            End If

            ' TOTAL FIXED COSTS
            If Not (IsDBNull(r("amod_tot_fixed_cost"))) Then
              amod_tot_fixed_cost = CDbl(r.Item("amod_tot_fixed_cost").ToString.Trim)
            End If

            ' TOTAL VARIABLE COSTS
            If Not (IsDBNull(r("amod_variable_costs"))) Then
              amod_variable_costs = CDbl(r.Item("amod_variable_costs").ToString.Trim)
            End If

            ' ANNUAL BUDGET
            If Not (IsDBNull(r("amod_number_of_seats"))) Then
              amod_number_of_seats = CDbl(r.Item("amod_number_of_seats").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_annual_miles"))) Then
              amod_annual_miles = CDbl(r.Item("amod_annual_miles").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_annual_hours"))) Then
              amod_annual_hours = CDbl(r.Item("amod_annual_hours").ToString.Trim)
            End If

            ' TOTAL DIRECT COSTS
            If Not (IsDBNull(r("amod_tot_direct_cost"))) Then
              amod_tot_direct_cost = CDbl(r.Item("amod_tot_direct_cost").ToString.Trim)
            End If

            ' TOTAL FIXED + DIRECT COSTS
            If Not (IsDBNull(r("amod_tot_df_annual_cost"))) Then
              amod_tot_df_annual_cost = CDbl(r.Item("amod_tot_df_annual_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_df_hour_cost"))) Then
              amod_tot_df_hour_cost = CDbl(r.Item("amod_tot_df_hour_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_df_statmile_cost"))) Then
              amod_tot_df_statmile_cost = CDbl(r.Item("amod_tot_df_statmile_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_df_seat_cost"))) Then
              amod_tot_df_seat_cost = CDbl(r.Item("amod_tot_df_seat_cost").ToString.Trim)
            End If

            ' TOTAL COSTS no depreciation
            If Not (IsDBNull(r("amod_tot_nd_annual_cost"))) Then
              amod_tot_nd_annual_cost = CDbl(r.Item("amod_tot_nd_annual_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_nd_hour_cost"))) Then
              amod_tot_nd_hour_cost = CDbl(r.Item("amod_tot_nd_hour_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_nd_statmile_cost"))) Then
              amod_tot_nd_statmile_cost = CDbl(r.Item("amod_tot_nd_statmile_cost").ToString.Trim)
            End If

            If Not (IsDBNull(r("amod_tot_nd_seat_cost"))) Then
              amod_tot_nd_seat_cost = CDbl(r.Item("amod_tot_nd_seat_cost").ToString.Trim)
            End If

          Next

        End If

      End If


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
    End Try
  End Sub

  Public Sub updateModelOpCostsClass()
    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("UPDATE Aircraft_Model SET")

      ' DIRECT COSTS/HOUR
      ' FUEL
      If amod_fuel_gal_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_fuel_gal_cost = @amod_fuel_gal_cost")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_gal_cost", amod_fuel_gal_cost.ToString)
        sSeperator = ","
      End If

      If amod_fuel_add_cost >= 0 Then
        modelQuery.Append(" amod_fuel add_cost = @amod_fuel add_cost")
        SqlCommand.Parameters.AddWithValue("@amod_fuel add_cost", amod_fuel_add_cost.ToString)
        sSeperator = ","
      End If

      If amod_fuel_burn_rate >= 0 Then
        modelQuery.Append(sSeperator + " amod_fuel_burn_rate = @amod_fuel_burn_rate")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_burn_rate", amod_fuel_burn_rate.ToString)
        sSeperator = ","
      End If

      ' recalculate total 
      amod_fuel_tot_cost = (amod_fuel_gal_cost + amod_fuel_add_cost) * amod_fuel_burn_rate

      If amod_fuel_tot_cost >= 0 Then
        modelQuery.Append(" amod_fuel_tot_cost = @amod_fuel_tot_cost")
        SqlCommand.Parameters.AddWithValue("@amod_fuel_tot_cost", amod_fuel_tot_cost.ToString)
        sSeperator = ","
      End If

      ' MAINTENANCE
      If amod_maint_lab_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_maint_lab_cost = @amod_maint_lab_cost")
        SqlCommand.Parameters.AddWithValue("@amod_maint_lab_cost", amod_maint_lab_cost.ToString)
        sSeperator = ","
      End If

      If amod_maint_labor_cost_man_hours_multiplier >= 0 Then
        modelQuery.Append(" amod_maint_labor_cost_man_hours_multiplier = @amod_maint_labor_cost_man_hours_multiplier")
        SqlCommand.Parameters.AddWithValue("@amod_maint_labor_cost_man_hours_multiplier", amod_maint_labor_cost_man_hours_multiplier.ToString)
        sSeperator = ","
      End If

      If amod_maint_parts_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_maint_parts_cost = @amod_maint_parts_cost")
        SqlCommand.Parameters.AddWithValue("@amod_maint_parts_cost", amod_maint_parts_cost.ToString)
        sSeperator = ","
      End If

      If amod_maint_parts_cost_man_hours_multiplier >= 0 Then
        modelQuery.Append(" amod_maint_parts_cost_man_hours_multiplier = @amod_maint_parts_cost_man_hours_multiplier")
        SqlCommand.Parameters.AddWithValue("@amod_maint_parts_cost_man_hours_multiplier", amod_maint_parts_cost_man_hours_multiplier.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      amod_maint_tot_cost = (amod_maint_lab_cost * amod_maint_labor_cost_man_hours_multiplier) + (amod_maint_parts_cost * amod_maint_parts_cost_man_hours_multiplier)

      If amod_maint_tot_cost >= 0 Then
        modelQuery.Append(" amod_maint_tot_cost = @amod_maint_tot_cost")
        SqlCommand.Parameters.AddWithValue("@amod_maint_tot_cost", amod_maint_tot_cost.ToString)
        sSeperator = ","
      End If

      If amod_engine_ovh_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_engine_ovh_cost = @amod_engine_ovh_cost")
        SqlCommand.Parameters.AddWithValue("@amod_engine_ovh_cost", amod_engine_ovh_cost.ToString)
        sSeperator = ","
      End If

      If amod_thrust_rev_ovh_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_thrust_rev_ovh_cost = @amod_thrust_rev_ovh_cost")
        SqlCommand.Parameters.AddWithValue("@amod_thrust_rev_ovh_cost", amod_thrust_rev_ovh_cost.ToString)
        sSeperator = ","
      End If

      ' MISC. FLIGHT EXP.
      If amod_land_park_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_land_park_cost = @amod_land_park_cost")
        SqlCommand.Parameters.AddWithValue("@amod_land_park_cost", amod_land_park_cost.ToString)
        sSeperator = ","
      End If

      If amod_crew_exp_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_crew_exp_cost = @amod_crew_exp_cost")
        SqlCommand.Parameters.AddWithValue("@amod_crew_exp_cost", amod_crew_exp_cost.ToString)
        sSeperator = ","
      End If

      If amod_supplies_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_supplies_cost = @amod_supplies_cost")
        SqlCommand.Parameters.AddWithValue("@amod_supplies_cost", amod_supplies_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      amod_misc_flight_cost = amod_land_park_cost + amod_crew_exp_cost + amod_supplies_cost

      If amod_misc_flight_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_misc_flight_cost = @amod_misc_flight_cost")
        SqlCommand.Parameters.AddWithValue("@amod_misc_flight_cost", amod_misc_flight_cost.ToString)
        sSeperator = ","
      End If

      ' TOTAL DIRECT COSTS
      ' recalculate total
      amod_tot_hour_direct_cost = amod_fuel_tot_cost + amod_maint_tot_cost + amod_misc_flight_cost + amod_engine_ovh_cost + amod_thrust_rev_ovh_cost

      If amod_tot_hour_direct_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_hour_direct_cost = @amod_tot_hour_direct_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_hour_direct_cost", amod_tot_hour_direct_cost.ToString)
        sSeperator = ","
      End If

      If amod_avg_block_speed >= 0 Then
        modelQuery.Append(sSeperator + " amod_avg_block_speed = @amod_avg_block_speed")
        SqlCommand.Parameters.AddWithValue("@amod_avg_block_speed", amod_avg_block_speed.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_avg_block_speed > 0 Then
        amod_tot_stat_mile_cost = amod_tot_hour_direct_cost / amod_avg_block_speed
      End If

      If amod_tot_stat_mile_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_stat_mile_cost = @amod_tot_stat_mile_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_stat_mile_cost", amod_tot_stat_mile_cost.ToString)
        sSeperator = ","
      End If

      ' ANNUAL FIXED COSTS
      ' CREW SALARIES
      If amod_capt_salary_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_capt_salary_cost = @amod_capt_salary_cost")
        SqlCommand.Parameters.AddWithValue("@amod_capt_salary_cost", amod_capt_salary_cost.ToString)
        sSeperator = ","
      End If

      If amod_cpilot_salary_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_cpilot_salary_cost = @amod_cpilot_salary_cost")
        SqlCommand.Parameters.AddWithValue("@amod_cpilot_salary_cost", amod_cpilot_salary_cost.ToString)
        sSeperator = ","
      End If

      If amod_crew_benefit_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_crew_benefit_cost = @amod_crew_benefit_cost")
        SqlCommand.Parameters.AddWithValue("@amod_crew_benefit_cost", amod_crew_benefit_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      amod_tot_crew_salary_cost = amod_capt_salary_cost + amod_cpilot_salary_cost + amod_crew_benefit_cost

      If amod_tot_crew_salary_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_crew_salary_cost = @amod_tot_crew_salary_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_crew_salary_cost", amod_tot_crew_salary_cost.ToString)
        sSeperator = ","
      End If

      If amod_hangar_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_hangar_cost = @amod_hangar_cost")
        SqlCommand.Parameters.AddWithValue("@amod_hangar_cost", amod_hangar_cost.ToString)
        sSeperator = ","
      End If

      ' INSURANCE
      If amod_hull_insurance_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_hull_insurance_cost = @amod_hull_insurance_cost")
        SqlCommand.Parameters.AddWithValue("@amod_hull_insurance_cost", amod_hull_insurance_cost.ToString)
        sSeperator = ","
      End If

      If amod_liability_insurance_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_liability_insurance_cost = @amod_liability_insurance_cost")
        SqlCommand.Parameters.AddWithValue("@amod_liability_insurance_cost", amod_liability_insurance_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      amod_insurance_cost = amod_hull_insurance_cost + amod_liability_insurance_cost

      If amod_insurance_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_insurance_cost = @amod_insurance_cost")
        SqlCommand.Parameters.AddWithValue("@amod_insurance_cost", amod_insurance_cost.ToString)
        sSeperator = ","
      End If

      ' MISC. OVERHEAD
      If amod_misc_train_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_misc_train_cost = @amod_misc_train_cost")
        SqlCommand.Parameters.AddWithValue("@amod_misc_train_cost", amod_misc_train_cost.ToString)
        sSeperator = ","
      End If

      If amod_misc_modern_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_misc_modern_cost = @amod_misc_modern_cost")
        SqlCommand.Parameters.AddWithValue("@amod_misc_modern_cost", amod_misc_modern_cost.ToString)
        sSeperator = ","
      End If

      If amod_misc_naveq_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_misc_naveq_cost = @amod_misc_naveq_cost")
        SqlCommand.Parameters.AddWithValue("@amod_misc_naveq_cost", amod_misc_naveq_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      amod_tot_misc_ovh_cost = amod_misc_train_cost + amod_misc_modern_cost + amod_misc_naveq_cost

      If amod_tot_misc_ovh_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_misc_ovh_cost = @amod_tot_misc_ovh_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_misc_ovh_cost", amod_tot_misc_ovh_cost.ToString)
        sSeperator = ","
      End If

      If amod_deprec_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_deprec_cost = @amod_deprec_cost")
        SqlCommand.Parameters.AddWithValue("@amod_deprec_cost", amod_deprec_cost.ToString)
        sSeperator = ","
      End If

      ' TOTAL FIXED COSTS
      ' recalculate total
      amod_tot_fixed_cost = amod_tot_crew_salary_cost + amod_insurance_cost + amod_tot_misc_ovh_cost + amod_hangar_cost + amod_deprec_cost

      If amod_tot_fixed_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_fixed_cost = @amod_tot_fixed_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_fixed_cost", amod_tot_fixed_cost.ToString)
        sSeperator = ","
      End If

      ' TOTAL VARIABLE COSTS
      If amod_variable_costs >= 0 Then
        modelQuery.Append(sSeperator + " amod_variable_costs = @amod_variable_costs")
        SqlCommand.Parameters.AddWithValue("@amod_variable_costs", amod_variable_costs.ToString)
        sSeperator = ","
      End If

      ' ANNUAL BUDGET
      If amod_number_of_seats >= 0 Then
        modelQuery.Append(sSeperator + " amod_number_of_seats = @amod_number_of_seats")
        SqlCommand.Parameters.AddWithValue("@amod_number_of_seats", amod_number_of_seats.ToString)
        sSeperator = ","
      End If

      If amod_annual_miles >= 0 Then
        modelQuery.Append(sSeperator + " amod_annual_miles = @amod_annual_miles")
        SqlCommand.Parameters.AddWithValue("@amod_annual_miles", amod_annual_miles.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_avg_block_speed > 0 Then
        amod_annual_hours = Math.Round(amod_annual_miles / amod_avg_block_speed, 0)
      End If

      If amod_annual_hours >= 0 Then
        modelQuery.Append(sSeperator + " amod_annual_hours = @amod_annual_hours")
        SqlCommand.Parameters.AddWithValue("@amod_annual_hours", amod_annual_hours.ToString)
        sSeperator = ","
      End If

      ' TOTAL DIRECT COSTS
      ' recalculate total
      If amod_annual_hours > 0 Then
        amod_tot_direct_cost = amod_annual_hours * amod_tot_hour_direct_cost
      ElseIf amod_airframe_type_code = "R" Then ' rotary aircraft dont multiply
        amod_tot_direct_cost = amod_tot_hour_direct_cost
      End If

      If amod_tot_direct_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_direct_cost = @amod_tot_direct_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_direct_cost", amod_tot_direct_cost.ToString)
        sSeperator = ","
      End If

      ' TOTAL FIXED + DIRECT COSTS
      ' recalculate total
      If amod_tot_direct_cost > 0 Then
        amod_tot_df_annual_cost = amod_tot_fixed_cost + amod_tot_direct_cost
      End If

      If amod_tot_df_annual_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_df_annual_cost = @amod_tot_df_annual_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_annual_cost", amod_tot_df_annual_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_annual_hours > 0 Then
        amod_tot_df_hour_cost = amod_tot_df_annual_cost / amod_annual_hours
      End If

      If amod_tot_df_hour_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_df_hour_cost = @amod_tot_df_hour_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_hour_cost", amod_tot_df_hour_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_annual_miles > 0 Then
        amod_tot_df_statmile_cost = amod_tot_df_annual_cost / amod_annual_miles
      End If

      If amod_tot_df_statmile_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_df_statmile_cost = @amod_tot_df_statmile_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_statmile_cost", amod_tot_df_statmile_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_number_of_seats > 0 Then
        amod_tot_df_seat_cost = amod_tot_df_statmile_cost / amod_number_of_seats
      End If

      If amod_tot_df_seat_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_df_seat_cost = @amod_tot_df_seat_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_seat_cost", amod_tot_df_seat_cost.ToString)
        sSeperator = ","
      End If

      ' TOTAL COSTS no depreciation
      ' recalculate total
      amod_tot_nd_annual_cost = amod_tot_df_annual_cost - amod_deprec_cost

      If amod_tot_nd_annual_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_nd_annual_cost = @amod_tot_nd_annual_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_annual_cost", amod_tot_nd_annual_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_annual_hours > 0 Then
        amod_tot_nd_hour_cost = amod_tot_nd_annual_cost / amod_annual_hours
      End If

      If amod_tot_nd_hour_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_nd_hour_cost = @amod_tot_nd_hour_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_hour_cost", amod_tot_nd_hour_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_annual_miles > 0 Then
        amod_tot_nd_statmile_cost = amod_tot_nd_annual_cost / amod_annual_miles
      End If

      If amod_tot_nd_statmile_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_nd_statmile_cost = @amod_tot_nd_statmile_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_statmile_cost", amod_tot_nd_statmile_cost.ToString)
        sSeperator = ","
      End If

      ' recalculate total
      If amod_number_of_seats > 0 Then
        amod_tot_nd_seat_cost = amod_tot_nd_statmile_cost / amod_number_of_seats
      End If

      If amod_tot_nd_seat_cost >= 0 Then
        modelQuery.Append(sSeperator + " amod_tot_nd_seat_cost = @amod_tot_nd_seat_cost")
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_seat_cost", amod_tot_nd_seat_cost.ToString)
        sSeperator = ","
      End If

      modelQuery.Append(" WHERE amod_id = @amod_id")
      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertModelOpCostsClass()

    Dim modelQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If amod_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      modelQuery.Append("INSERT INTO Aircraft_Model (amod_fuel_tot_cost, amod_fuel_gal_cost, amod_fuel_add_cost, amod_fuel_burn_rate, amod_maint_tot_cost,")
      modelQuery.Append(" amod_maint_lab_cost, amod_maint_labor_cost_man_hours_multiplier, amod_maint_parts_cost, amod_maint_parts_cost_man_hours_multiplier,")
      modelQuery.Append(" amod_engine_ovh_cost, amod_thrust_rev_ovh_cost, amod_misc_flight_cost, amod_land_park_cost, amod_crew_exp_cost, amod_supplies_cost,")
      modelQuery.Append(" amod_tot_hour_direct_cost, amod_avg_block_speed, amod_tot_stat_mile_cost, amod_tot_crew_salary_cost, amod_capt_salary_cost, amod_cpilot_salary_cost,")
      modelQuery.Append(" amod_crew_benefit_cost, amod_hangar_cost, amod_insurance_cost, amod_hull_insurance_cost, amod_liability_insurance_cost,")
      modelQuery.Append(" amod_tot_misc_ovh_cost, amod_misc_train_cost, amod_misc_modern_cost, amod_misc_naveq_cost, amod_deprec_cost,")
      modelQuery.Append(" amod_tot_fixed_cost, amod_variable_costs, amod_number_of_seats, amod_annual_miles, amod_annual_hours,")
      modelQuery.Append(" amod_tot_direct_cost, amod_tot_fixed_cost, amod_tot_df_annual_cost, amod_tot_df_hour_cost, amod_tot_df_statmile_cost, amod_tot_df_seat_cost,")
      modelQuery.Append(" amod_tot_nd_annual_cost, amod_tot_nd_hour_cost, amod_tot_nd_statmile_cost, amod_tot_nd_seat_cost")
      modelQuery.Append(") VALUES (@amod_fuel_tot_cost, @amod_fuel_gal_cost, @amod_fuel_add_cost, @amod_fuel_burn_rate, @amod_maint_tot_cost,")
      modelQuery.Append(" @amod_maint_lab_cost, @amod_maint_labor_cost_man_hours_multiplier, @amod_maint_parts_cost, @amod_maint_parts_cost_man_hours_multiplier,")
      modelQuery.Append(" @amod_engine_ovh_cost, @amod_thrust_rev_ovh_cost, @amod_misc_flight_cost, @amod_land_park_cost, @amod_crew_exp_cost, @amod_supplies_cost,")
      modelQuery.Append(" @amod_tot_hour_direct_cost, @amod_avg_block_speed, @amod_tot_stat_mile_cost, @amod_tot_crew_salary_cost, @amod_capt_salary_cost, @amod_cpilot_salary_cost,")
      modelQuery.Append(" @amod_crew_benefit_cost, @amod_hangar_cost, @amod_insurance_cost, @amod_hull_insurance_cost, @amod_liability_insurance_cost,")
      modelQuery.Append(" @amod_tot_misc_ovh_cost, @amod_misc_train_cost, @amod_misc_modern_cost, @amod_misc_naveq_cost, @amod_deprec_cost,")
      modelQuery.Append(" @amod_tot_fixed_cost, @amod_variable_costs, @amod_number_of_seats, @amod_annual_miles, @amod_annual_hours,")
      modelQuery.Append(" @amod_tot_direct_cost, @amod_tot_fixed_cost, @amod_tot_df_annual_cost, @amod_tot_df_hour_cost, @amod_tot_df_statmile_cost, @amod_tot_df_seat_cost,")
      modelQuery.Append(" @amod_tot_nd_annual_cost, @amod_tot_nd_hour_cost, @amod_tot_nd_statmile_cost, @amod_tot_nd_seat_cost")
      modelQuery.Append(") WHERE amod_id = @amod_id")

      ' DIRECT COSTS/HOUR
      ' FUEL

      ' calculate total 
      amod_fuel_tot_cost = (amod_fuel_gal_cost + amod_fuel_add_cost) * amod_fuel_burn_rate

      If amod_fuel_tot_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_tot_cost", amod_fuel_tot_cost.ToString)
      End If

      If amod_fuel_gal_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_gal_cost", amod_fuel_gal_cost.ToString)
      End If

      If amod_fuel_add_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel add_cost", amod_fuel_add_cost.ToString)
      End If

      If amod_fuel_burn_rate >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_fuel_burn_rate", amod_fuel_burn_rate.ToString)
      End If

      ' MAINTENANCE
      ' calculate total
      amod_maint_tot_cost = (amod_maint_lab_cost * amod_maint_labor_cost_man_hours_multiplier) + (amod_maint_parts_cost * amod_maint_parts_cost_man_hours_multiplier)

      If amod_maint_tot_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_maint_tot_cost", amod_maint_tot_cost.ToString)
      End If

      If amod_maint_lab_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_maint_lab_cost", amod_maint_lab_cost.ToString)
      End If

      If amod_maint_labor_cost_man_hours_multiplier >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_maint_labor_cost_man_hours_multiplier", amod_maint_labor_cost_man_hours_multiplier.ToString)
      End If

      If amod_maint_parts_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_maint_parts_cost", amod_maint_parts_cost.ToString)
      End If

      If amod_maint_parts_cost_man_hours_multiplier >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_maint_parts_cost_man_hours_multiplier", amod_maint_parts_cost_man_hours_multiplier.ToString)
      End If

      If amod_engine_ovh_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_engine_ovh_cost", amod_engine_ovh_cost.ToString)
      End If

      If amod_thrust_rev_ovh_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_thrust_rev_ovh_cost", amod_thrust_rev_ovh_cost.ToString)
      End If

      ' MISC. FLIGHT EXP.
      ' calculate total
      amod_misc_flight_cost = amod_land_park_cost + amod_crew_exp_cost + amod_supplies_cost

      If amod_misc_flight_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_misc_flight_cost", amod_misc_flight_cost.ToString)
      End If

      If amod_land_park_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_land_park_cost", amod_land_park_cost.ToString)
      End If

      If amod_crew_exp_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_crew_exp_cost", amod_crew_exp_cost.ToString)
      End If

      If amod_supplies_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_supplies_cost", amod_supplies_cost.ToString)
      End If

      ' TOTAL DIRECT COSTS
      ' calculate total
      amod_tot_hour_direct_cost = amod_fuel_tot_cost + amod_maint_tot_cost + amod_misc_flight_cost + amod_engine_ovh_cost + amod_thrust_rev_ovh_cost

      If amod_tot_hour_direct_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_hour_direct_cost", amod_tot_hour_direct_cost.ToString)
      End If

      If amod_avg_block_speed >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_avg_block_speed", amod_avg_block_speed.ToString)
      End If

      ' calculate total
      If amod_avg_block_speed > 0 Then
        amod_tot_stat_mile_cost = amod_tot_hour_direct_cost / amod_avg_block_speed
      End If

      If amod_tot_stat_mile_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_stat_mile_cost", amod_tot_stat_mile_cost.ToString)
      End If

      ' ANNUAL FIXED COSTS
      ' CREW SALARIES
      ' calculate total
      amod_tot_crew_salary_cost = amod_capt_salary_cost + amod_cpilot_salary_cost + amod_crew_benefit_cost

      If amod_tot_crew_salary_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_crew_salary_cost", amod_tot_crew_salary_cost.ToString)
      End If

      If amod_capt_salary_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_capt_salary_cost", amod_capt_salary_cost.ToString)
      End If

      If amod_cpilot_salary_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_cpilot_salary_cost", amod_cpilot_salary_cost.ToString)
      End If

      If amod_crew_benefit_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_crew_benefit_cost", amod_crew_benefit_cost.ToString)
      End If

      If amod_hangar_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_hangar_cost", amod_hangar_cost.ToString)
      End If

      ' INSURANCE
      ' calculate total
      amod_insurance_cost = amod_hull_insurance_cost + amod_liability_insurance_cost

      If amod_insurance_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_insurance_cost", amod_insurance_cost.ToString)
      End If

      If amod_hull_insurance_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_hull_insurance_cost", amod_hull_insurance_cost.ToString)
      End If

      If amod_liability_insurance_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_liability_insurance_cost", amod_liability_insurance_cost.ToString)
      End If

      ' MISC. OVERHEAD
      ' calculate total
      amod_tot_misc_ovh_cost = amod_misc_train_cost + amod_misc_modern_cost + amod_misc_naveq_cost

      If amod_tot_misc_ovh_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_misc_ovh_cost", amod_tot_misc_ovh_cost.ToString)
      End If

      If amod_misc_train_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_misc_train_cost", amod_misc_train_cost.ToString)
      End If

      If amod_misc_modern_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_misc_modern_cost", amod_misc_modern_cost.ToString)
      End If

      If amod_misc_naveq_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_misc_naveq_cost", amod_misc_naveq_cost.ToString)
      End If

      If amod_deprec_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_deprec_cost", amod_deprec_cost.ToString)
      End If

      ' TOTAL FIXED COSTS
      ' calculate total
      amod_tot_fixed_cost = amod_tot_crew_salary_cost + amod_insurance_cost + amod_tot_misc_ovh_cost + amod_hangar_cost + amod_deprec_cost

      If amod_tot_fixed_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_fixed_cost", amod_tot_fixed_cost.ToString)
      End If

      ' TOTAL VARIABLE COSTS
      If amod_variable_costs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_variable_costs", amod_variable_costs.ToString)
      End If

      ' ANNUAL BUDGET
      If amod_number_of_seats >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_number_of_seats", amod_number_of_seats.ToString)
      End If

      If amod_annual_miles >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_annual_miles", amod_annual_miles.ToString)
      End If

      ' calculate total
      If amod_avg_block_speed > 0 Then
        amod_annual_hours = Math.Round(amod_annual_miles / amod_avg_block_speed, 0)
      End If

      If amod_annual_hours >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_annual_hours", amod_annual_hours.ToString)
      End If

      ' TOTAL DIRECT COSTS
      If amod_annual_hours > 0 Then
        amod_tot_direct_cost = amod_annual_hours * amod_tot_hour_direct_cost
      ElseIf amod_airframe_type_code = "R" Then ' rotary aircraft dont multiply
        amod_tot_direct_cost = amod_tot_hour_direct_cost
      End If

      If amod_tot_direct_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_direct_cost", amod_tot_direct_cost.ToString)
      End If

      ' TOTAL FIXED + DIRECT COSTS
      ' calculate total
      If amod_tot_direct_cost > 0 Then
        amod_tot_df_annual_cost = amod_tot_fixed_cost + amod_tot_direct_cost
      End If

      If amod_tot_df_annual_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_annual_cost", amod_tot_df_annual_cost.ToString)
      End If

      ' calculate total
      If amod_annual_hours > 0 Then
        amod_tot_df_hour_cost = amod_tot_df_annual_cost / amod_annual_hours
      End If

      If amod_tot_df_hour_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_hour_cost", amod_tot_df_hour_cost.ToString)
      End If

      ' calculate total
      If amod_annual_miles > 0 Then
        amod_tot_df_statmile_cost = amod_tot_df_annual_cost / amod_annual_miles
      End If

      If amod_tot_df_statmile_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_statmile_cost", amod_tot_df_statmile_cost.ToString)
      End If

      ' calculate total
      If amod_number_of_seats > 0 Then
        amod_tot_df_seat_cost = amod_tot_df_statmile_cost / amod_number_of_seats
      End If

      If amod_tot_df_seat_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_df_seat_cost", amod_tot_df_seat_cost.ToString)
      End If

      ' TOTAL COSTS no depreciation
      ' calculate total
      amod_tot_nd_annual_cost = amod_tot_df_annual_cost - amod_deprec_cost

      If amod_tot_nd_annual_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_annual_cost", amod_tot_nd_annual_cost.ToString)
      End If

      ' calculate total
      If amod_annual_hours > 0 Then
        amod_tot_nd_hour_cost = amod_tot_nd_annual_cost / amod_annual_hours
      End If

      If amod_tot_nd_hour_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_hour_cost", amod_tot_nd_hour_cost.ToString)
      End If

      ' calculate total
      If amod_annual_miles > 0 Then
        amod_tot_nd_statmile_cost = amod_tot_nd_annual_cost / amod_annual_miles
      End If

      If amod_tot_nd_statmile_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_statmile_cost", amod_tot_nd_statmile_cost.ToString)
      End If

      ' calculate total
      If amod_number_of_seats > 0 Then
        amod_tot_nd_seat_cost = amod_tot_nd_statmile_cost / amod_number_of_seats
      End If

      If amod_tot_nd_seat_cost >= 0 Then
        SqlCommand.Parameters.AddWithValue("@amod_tot_nd_seat_cost", amod_tot_nd_seat_cost.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@amod_id", amod_id.ToString)

      SqlCommand.CommandText = modelQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Overrides Function Equals(obj As Object) As Boolean
    Dim [class] = TryCast(obj, homebaseModelOpCostsClass)
    Return [class] IsNot Nothing AndAlso
           amod_id = [class].amod_id AndAlso
           amod_airframe_type_code = [class].amod_airframe_type_code AndAlso
           amod_fuel_gal_cost = [class].amod_fuel_gal_cost AndAlso
           amod_fuel_add_cost = [class].amod_fuel_add_cost AndAlso
           amod_fuel_burn_rate = [class].amod_fuel_burn_rate AndAlso
           amod_fuel_tot_cost = [class].amod_fuel_tot_cost AndAlso
           amod_maint_lab_cost = [class].amod_maint_lab_cost AndAlso
           amod_maint_parts_cost = [class].amod_maint_parts_cost AndAlso
           amod_maint_labor_cost_man_hours_multiplier = [class].amod_maint_labor_cost_man_hours_multiplier AndAlso
           amod_maint_parts_cost_man_hours_multiplier = [class].amod_maint_parts_cost_man_hours_multiplier AndAlso
           amod_maint_tot_cost = [class].amod_maint_tot_cost AndAlso
           amod_engine_ovh_cost = [class].amod_engine_ovh_cost AndAlso
           amod_thrust_rev_ovh_cost = [class].amod_thrust_rev_ovh_cost AndAlso
           amod_land_park_cost = [class].amod_land_park_cost AndAlso
           amod_crew_exp_cost = [class].amod_crew_exp_cost AndAlso
           amod_supplies_cost = [class].amod_supplies_cost AndAlso
           amod_misc_flight_cost = [class].amod_misc_flight_cost AndAlso
           amod_tot_hour_direct_cost = [class].amod_tot_hour_direct_cost AndAlso
           amod_avg_block_speed = [class].amod_avg_block_speed AndAlso
           amod_tot_stat_mile_cost = [class].amod_tot_stat_mile_cost AndAlso
           amod_capt_salary_cost = [class].amod_capt_salary_cost AndAlso
           amod_cpilot_salary_cost = [class].amod_cpilot_salary_cost AndAlso
           amod_crew_benefit_cost = [class].amod_crew_benefit_cost AndAlso
           amod_tot_crew_salary_cost = [class].amod_tot_crew_salary_cost AndAlso
           amod_hangar_cost = [class].amod_hangar_cost AndAlso
           amod_hull_insurance_cost = [class].amod_hull_insurance_cost AndAlso
           amod_liability_insurance_cost = [class].amod_liability_insurance_cost AndAlso
           amod_insurance_cost = [class].amod_insurance_cost AndAlso
           amod_misc_train_cost = [class].amod_misc_train_cost AndAlso
           amod_misc_modern_cost = [class].amod_misc_modern_cost AndAlso
           amod_misc_naveq_cost = [class].amod_misc_naveq_cost AndAlso
           amod_tot_misc_ovh_cost = [class].amod_tot_misc_ovh_cost AndAlso
           amod_deprec_cost = [class].amod_deprec_cost AndAlso
           amod_tot_fixed_cost = [class].amod_tot_fixed_cost AndAlso
           amod_variable_costs = [class].amod_variable_costs AndAlso
           amod_number_of_seats = [class].amod_number_of_seats AndAlso
           amod_annual_miles = [class].amod_annual_miles AndAlso
           amod_annual_hours = [class].amod_annual_hours AndAlso
           amod_tot_direct_cost = [class].amod_tot_direct_cost AndAlso
           amod_tot_df_annual_cost = [class].amod_tot_df_annual_cost AndAlso
           amod_tot_df_hour_cost = [class].amod_tot_df_hour_cost AndAlso
           amod_tot_df_statmile_cost = [class].amod_tot_df_statmile_cost AndAlso
           amod_tot_df_seat_cost = [class].amod_tot_df_seat_cost AndAlso
           amod_tot_nd_annual_cost = [class].amod_tot_nd_annual_cost AndAlso
           amod_tot_nd_hour_cost = [class].amod_tot_nd_hour_cost AndAlso
           amod_tot_nd_statmile_cost = [class].amod_tot_nd_statmile_cost AndAlso
           amod_tot_nd_seat_cost = [class].amod_tot_nd_seat_cost
  End Function

  Public Shared Operator =(class1 As homebaseModelOpCostsClass, class2 As homebaseModelOpCostsClass) As Boolean
    Return EqualityComparer(Of homebaseModelOpCostsClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseModelOpCostsClass, class2 As homebaseModelOpCostsClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class