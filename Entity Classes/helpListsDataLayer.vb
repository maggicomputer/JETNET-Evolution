Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/helpListsDataLayer.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: helpListsDataLayer.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class helpSelectionCriteriaClass

  Private _helpCriteriaStatusCode As eObjStatusCode
  Private _helpCriteriaDetailError As eObjDetailErrorCode
  Private _helpCriteriaShowList As String
  Private _helpCriteriaAirframeType As String
  Private _helpCriteriaMakeType As String
  Private _helpCriteriaMakeName As String
  Private _helpCriteriaModelID As Long
  Private _helpCriteriaTmpModelID As Long
  Private _helpCriteriaAvionicsType As String
  Private _helpCriteriaTransactionSendToWeb As String

  Sub New()

    _helpCriteriaStatusCode = eObjStatusCode.NULL
    _helpCriteriaDetailError = eObjDetailErrorCode.NULL
    _helpCriteriaShowList = ""
    _helpCriteriaAirframeType = ""
    _helpCriteriaMakeType = ""
    _helpCriteriaAvionicsType = ""
    _helpCriteriaMakeName = ""
    _helpCriteriaModelID = -1
    _helpCriteriaTmpModelID = -1
    _helpCriteriaTransactionSendToWeb = False

  End Sub

  Public Property HelpSelectionCriteriaStatusCode() As eObjStatusCode
    Get
      Return _helpCriteriaStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _helpCriteriaStatusCode = value
    End Set
  End Property

  Public Property HelpSelectionCriteriaDetailError() As eObjDetailErrorCode
    Get
      Return _helpCriteriaDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _helpCriteriaDetailError = value
    End Set
  End Property

  Public Property HelpCriteriaShowList() As String
    Get
      Return _helpCriteriaShowList
    End Get
    Set(ByVal value As String)
      _helpCriteriaShowList = value
    End Set
  End Property

  Public Property HelpCriteriaAirframeType() As String
    Get
      Return _helpCriteriaAirframeType
    End Get
    Set(ByVal value As String)
      _helpCriteriaAirframeType = value
    End Set
  End Property

  Public Property HelpCriteriaMakeType() As String
    Get
      Return _helpCriteriaMakeType
    End Get
    Set(ByVal value As String)
      _helpCriteriaMakeType = value
    End Set
  End Property

  Public Property HelpCriteriaAvionicsType() As String
    Get
      Return _helpCriteriaAvionicsType
    End Get
    Set(ByVal value As String)
      _helpCriteriaAvionicsType = value
    End Set
  End Property

  Public Property HelpCriteriaMakeName() As String
    Get
      Return _helpCriteriaMakeName
    End Get
    Set(ByVal value As String)
      _helpCriteriaMakeName = value
    End Set
  End Property

  Public Property HelpCriteriaModelID() As Long
    Get
      Return _helpCriteriaModelID
    End Get
    Set(ByVal value As Long)
      _helpCriteriaModelID = value
    End Set
  End Property

  Public Property HelpCriteriaTmpModelID() As Long
    Get
      Return _helpCriteriaTmpModelID
    End Get
    Set(ByVal value As Long)
      _helpCriteriaTmpModelID = value
    End Set
  End Property

  Public Property HelpCriteriaTransactionSendToWeb() As Boolean
    Get
      Return _helpCriteriaTransactionSendToWeb
    End Get
    Set(ByVal value As Boolean)
      _helpCriteriaTransactionSendToWeb = value
    End Set
  End Property

End Class  ' 

<System.Serializable()> Public Class helpListsDataLayer

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

  Public Function get_weight_class_list_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT acwgtcls_airframe_type_code AS AirframeType, acwgtcls_maketype AS MakeType,")
      sQuery.Append(" (SELECT TOP 1 afmt_description FROM Airframe_Make_Type WITH (NOLOCK)")
      sQuery.Append(" WHERE (afmt_airframetype = acwgtcls_airframe_type_code)")
      sQuery.Append(" AND (afmt_code = acwgtcls_maketype)")
      sQuery.Append(" ) AS MakeTypeDesc,")
      sQuery.Append(" acwgtcls_name AS WeightClassName, acwgtcls_weight_from AS WeightFrom, acwgtcls_weight_to AS WeightTo")
      sQuery.Append(" FROM Aircraft_Weight_Class WHERE (acwgtcls_airframe_type_code IN ('F','R'))")
      sQuery.Append(" AND (EXISTS (SELECT NULL FROM Aircraft_Model WITH (NOLOCK)")
      sQuery.Append(" WHERE (amod_airframe_type_code = acwgtcls_airframe_type_code)")
      sQuery.Append(" AND (amod_type_code = acwgtcls_maketype)")
      sQuery.Append(" AND (amod_product_business_flag = 'Y' OR amod_product_helicopter_flag = 'Y' OR amod_product_commercial_flag = 'Y')))")
      sQuery.Append(" ORDER BY acwgtcls_airframe_type_code, acwgtcls_maketype, acwgtcls_weight_from ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_weight_class_list_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_weight_class_list_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_weight_class_list_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_weight_class_list(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable

    Dim filtered_table As New DataTable

    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim rememberAcType As String = ""

    Try

      results_table = get_weight_class_list_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")
          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>[airframe] - Type</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Class</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Weight<br>From (lbs)</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>To (lbs)</strong></td></tr>")

          ' display weight class based on product code?
          If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then

            filtered_table = results_table.Clone 'gets a blank copy of the results schema

            Dim afileterd As DataRow() = results_table.Select("AirframeType = 'R'", "AirframeType, MakeType, WeightTo ASC")

            For Each atmpDataRow As DataRow In afileterd
              filtered_table.ImportRow(atmpDataRow)
            Next

            results_table = filtered_table

          End If

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If rememberAcType.ToLower <> r.Item("MakeTypeDesc").ToString.ToLower.Trim Then

              htmlOut.Append("<td align='left' valign='middle'>[" + r.Item("AirframeType").ToString.Trim + "] - " + r.Item("MakeTypeDesc").ToString.Trim)
              htmlOut.Append("</td>")

              rememberAcType = r.Item("MakeTypeDesc").ToString.Trim
            Else
              htmlOut.Append("<td align='left' valign='middle'>&nbsp;</td>")
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("WeightClassName").ToString.Trim)
            htmlOut.Append("</td>")

            If r.Item("WeightFrom").ToString = "0" Then
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>0")
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + FormatNumber(r.Item("WeightFrom").ToString, 0, False, False, True).ToString)
            End If

            htmlOut.Append("</td>")
            If r.Item("WeightTo").ToString = "999999" Then
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>up")
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + FormatNumber(r.Item("WeightTo").ToString, 0, False, False, True).ToString)
            End If
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_weight_class_list(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_weight_class_list_model_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT aftype_name As AirframeName, afmt_description As MakeTypeName,")
      sQuery.Append(" amod_make_name As Make, amod_model_name As Model, acwgtcls_name AS WeightClass, acwgtcls_weight_from As WeightFrom, acwgtcls_weight_to As WeightTo")
      sQuery.Append(" FROM Aircraft_Model WITH (NOLOCK)")

      sQuery.Append(" INNER JOIN Aircraft_Weight_Class ON amod_type_code = acwgtcls_maketype AND amod_airframe_type_code = acwgtcls_airframe_type_code AND amod_weight_class = acwgtcls_code")
      sQuery.Append(" INNER JOIN Airframe_Make_Type ON amod_type_code = afmt_code AND amod_airframe_type_code = afmt_airframetype")
      sQuery.Append(" INNER JOIN Airframe_Type ON amod_airframe_type_code = aftype_code")
      sQuery.Append(" WHERE (amod_customer_flag = 'Y')")

      ' only display weight class for models available
      If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_product_helicopter_flag = 'Y'")
      End If

      If HttpContext.Current.Session.Item("localPreferences").isBusinessOnlyProduct Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_product_business_flag = 'Y'")
      End If

      If HttpContext.Current.Session.Item("localPreferences").isCommercialOnlyProduct Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_product_commercial_flag = 'Y'")
      End If

      sQuery.Append(" ORDER BY acwgtcls_airframe_type_code, acwgtcls_maketype, acwgtcls_weight_from ASC, amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_weight_class_list_model_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_weight_class_list_model_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_weight_class_list_model_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_weight_class_list_model(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim rememberAcType As String = ""

    Try

      results_table = get_weight_class_list_model_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Make</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Model</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Weight Class</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>From (lbs)</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>To (lbs)</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If rememberAcType.ToLower <> r.Item("MakeTypeName").ToString.ToLower.Trim Then

              htmlOut.Append("<td align='center' valign='middle' colspan='5' class='seperator'><strong>" + r.Item("AirframeName").ToString.ToUpper.Trim + " - " + r.Item("MakeTypeName").ToString.ToUpper.Trim)
              htmlOut.Append("</strong></td></tr>")

              rememberAcType = r.Item("MakeTypeName").ToString.Trim

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

            End If


            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("Make").ToString.Trim)
            htmlOut.Append("</td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("Model").ToString.Trim)
            htmlOut.Append("</td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("WeightClass").ToString.Trim)
            htmlOut.Append("</td>")

            If r.Item("WeightFrom").ToString = "0" Then
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>0")
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + FormatNumber(r.Item("WeightFrom").ToString, 0, False, False, True).ToString)
            End If

            htmlOut.Append("</td>")
            If r.Item("WeightTo").ToString = "999999" Then
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>up")
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + FormatNumber(r.Item("WeightTo").ToString, 0, False, False, True).ToString)
            End If

            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_weight_class_list_model(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_lifecycle_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Aircraft_Stage WITH (NOLOCK) ORDER BY acs_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_lifecycle_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_lifecycle_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_lifecycle_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_lifecycle(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_aircraft_lifecycle_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Lifecycle</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Description</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("acs_name").ToString.Replace("(", "<br />(").Trim)
            htmlOut.Append("</td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("acs_desc").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          If Not toggleRowColor Then
            htmlOut.Append("<tr class='alt_row'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor='white'>")
            toggleRowColor = False
          End If

          htmlOut.Append("<td align='center' valign='middle' class='seperator'>Retired-In Storage</td>")
          htmlOut.Append("<td align='left' valign='middle' class='seperator'>This describes aircraft that are in storage and have retained a registration number. These aircraft may be returned to active operation.</td></tr>")

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_weight_class_list_model(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_serial_number_format_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_ser_no_prefix, amod_serno_hyphen_flag, amod_ser_no_suffix, amod_ser_no_start ")
      sQuery.Append("FROM Aircraft_Model WITH(NOLOCK) WHERE (amod_customer_flag = 'Y') ORDER BY amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_serial_number_format_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_serial_number_format_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_serial_number_format_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_serial_number_format(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_serial_number_format_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Make</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Model</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Prefix</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Hyphen</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Suffix</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>SerNbr</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Example</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_make_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_model_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_ser_no_prefix").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + IIf(r.Item("amod_serno_hyphen_flag").ToString.ToUpper.Trim = "Y", crmWebClient.Constants.cHyphen, ""))
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_ser_no_suffix").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_ser_no_start").ToString.Trim)
            htmlOut.Append("</td>")

            If Not String.IsNullOrEmpty(r.Item("amod_ser_no_prefix").ToString.Trim) Then
              strExample = r.Item("amod_ser_no_prefix").ToString.Trim
            End If

            If Not String.IsNullOrEmpty(r.Item("amod_serno_hyphen_flag").ToString.Trim) Then
              If r.Item("amod_serno_hyphen_flag").ToString.ToUpper.Trim = "Y" Then
                strExample += crmWebClient.Constants.cHyphen
              End If
            End If

            If Not String.IsNullOrEmpty(r.Item("amod_ser_no_start").ToString.Trim) Then
              strExample += r.Item("amod_ser_no_start").ToString.Trim
            End If

            If Not String.IsNullOrEmpty(r.Item("amod_ser_no_suffix").ToString.Trim) Then
              strExample += r.Item("amod_ser_no_suffix").ToString.Trim
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + strExample.Trim)
            htmlOut.Append("</td></tr>")

            strExample = ""

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_serial_number_format(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_registration_number_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT country_name, country_regnbr_prefix FROM Country WITH(NOLOCK) WHERE (country_regnbr_prefix IS NOT NULL) ORDER BY country_name, country_regnbr_prefix")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_registration_number_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_registration_number_prefix_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_registration_number_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_registration_number_prefix(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim columnCount As Integer = 0

    Try

      results_table = get_aircraft_registration_number_prefix_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Country</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>RegNbr</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom' width='1'></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Country</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>RegNbr</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If columnCount = 0 Then
              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator' nowrap='nowrap'>" + r.Item("country_name").ToString.Trim)
            htmlOut.Append("</td>")
            htmlOut.Append("<td align='left' valign='middle' class='seperator' nowrap='nowrap'>" + r.Item("country_regnbr_prefix").ToString.Trim)
            htmlOut.Append("</td>")

            columnCount += 2

            If columnCount = 2 Then
              htmlOut.Append("<td align='left' valign='bottom' class='seperator' width='1'></td>")
            End If

            If columnCount = 4 Then
              columnCount = 0
              htmlOut.Append("</tr>")
            End If

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_registration_number_prefix(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_features_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT kfeat_code, kfeat_name")
      sQuery.Append(" FROM Key_Feature WITH(NOLOCK) WHERE (kfeat_inactive_date IS NULL) ORDER BY kfeat_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_features_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_features_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_features_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_features(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_features_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Code</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Description</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("kfeat_code").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("kfeat_name").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_features(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_model_features_info(ByRef searchCriteria As helpSelectionCriteriaClass, Optional ByVal fcode As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_make_name, amod_model_name, amfeat_seq_no, kfeat_code, kfeat_name, amfeat_standard_equip")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model_Key_Feature ON amod_id = amfeat_amod_id")
      sQuery.Append(" INNER JOIN Key_Feature ON amfeat_feature_code = kfeat_code")
      sQuery.Append(" WHERE (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (amfeat_inactive_date IS NULL)")

      If Trim(fcode) <> "" Then
        sQuery.Append(" AND amfeat_feature_code = '" & UCase(fcode) & "' ")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaAirframeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_airframe_type_code = '" + searchCriteria.HelpCriteriaAirframeType.Trim + "')")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
      End If

      sQuery.Append(" AND kfeat_inactive_date IS NULL ORDER BY amod_make_name, amod_model_name, amfeat_seq_no")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_model_features_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_model_features_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_model_features_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_model_features(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal fcode As String = "")

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_model_features_info(searchCriteria, fcode)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Make</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Model</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>SeqNbr</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Code</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Description</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_make_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_model_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amfeat_seq_no").ToString.Trim)
            htmlOut.Append("</td>")

            If Not String.IsNullOrEmpty(r.Item("amfeat_standard_equip").ToString.Trim) Then
              If r.Item("amfeat_standard_equip").ToString.ToUpper.Trim = "Y" Then
                htmlOut.Append("<td align='left' valign='middle' class='seperator' title='Standard Equipment'><font color='blue'>" + r.Item("kfeat_code").ToString.Trim + "</font>")
              Else
                htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("kfeat_code").ToString.Trim)
              End If
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("kfeat_code").ToString.Trim)
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("kfeat_name").ToString.Trim)
            htmlOut.Append("</td></tr>")

            strExample = ""

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_model_features(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_avionics_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If String.IsNullOrEmpty(searchCriteria.HelpCriteriaAvionicsType) Then
        sQuery.Append("SELECT av_name AS AVType, Count(DISTINCT av_description) AS TotRec, avion_notes As AVDesc")
        sQuery.Append(" FROM Aircraft_Avionics WITH (NOLOCK)")
        sQuery.Append(" INNER JOIN Avionics WITH (NOLOCK) ON av_name = avion_name")
        sQuery.Append(" WHERE (av_ac_journ_id = 0)")
        sQuery.Append(" GROUP BY av_name, avion_notes ORDER BY av_name")
      Else
        sQuery.Append("SELECT DISTINCT av_name AS AVType, av_description As AVName")
        sQuery.Append(" FROM Aircraft_Avionics WITH (NOLOCK)")
        sQuery.Append(" WHERE (av_ac_journ_id = 0) AND (av_name = '" + searchCriteria.HelpCriteriaAvionicsType.Trim + "')")
        sQuery.Append(" ORDER BY av_name, av_description")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_avionics_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_avionics_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_avionics_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_avionics(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_avionics_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          If String.IsNullOrEmpty(searchCriteria.HelpCriteriaAvionicsType.Trim) Then
            htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Avionics Type</strong></td>")
            htmlOut.Append("<td align='center' valign='bottom'><strong>Total Records</strong></td>")
            htmlOut.Append("<td align='center' valign='bottom' width='300'><strong>Description</strong></td></tr>")
          Else
            htmlOut.Append("<tr><td align='center' valign='bottom'><a href='masterLists.aspx?helplist=avionics' target='_self' title='Click To View Avionics Type List'>Avionics Type List</a><br/><br/>")
            htmlOut.Append("<strong>Avionics Name</strong></td></tr>")
          End If

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If String.IsNullOrEmpty(searchCriteria.HelpCriteriaAvionicsType.Trim) Then
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("AVType").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='center' valign='middle' class='seperator'>")
              htmlOut.Append("<a href='masterLists.aspx?helplist=avionics&helpAvType=" + HttpContext.Current.Server.UrlEncode(r.Item("AVType").ToString.Trim) + "' target='_self' title='Click To View Details'>" + r.Item("TotRec").ToString.Trim + "</a>")
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("AVDesc").ToString.Trim)
              htmlOut.Append("</td>")
            Else
              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("AVName").ToString.Trim)
            End If

            htmlOut.Append("</td></tr>")

            strExample = ""

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_avionics(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_make_model_engine_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_airframe_type_code As AirframeType, aftype_name As AirframeTypeName,")
      sQuery.Append(" amod_id As ModelId, amod_type_code AS MakeType, afmt_description AS MakeTypeName,")
      sQuery.Append(" amod_make_name AS Make, amod_model_name AS Model")
      sQuery.Append(" FROM Aircraft_Model WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Airframe_Type WITH (NOLOCK) ON amod_airframe_type_code = aftype_code")
      sQuery.Append(" INNER JOIN Airframe_Make_Type WITH (NOLOCK) ON amod_airframe_type_code = afmt_airframetype AND amod_type_code = afmt_code")
      sQuery.Append(" WHERE (amod_customer_flag = 'Y')")

      If searchCriteria.HelpCriteriaModelID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_id = " + searchCriteria.HelpCriteriaModelID.ToString + ")")
      Else

        If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_airframe_type_code = 'R')")
        Else
          If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaAirframeType.Trim) Then
            sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_airframe_type_code = '" + searchCriteria.HelpCriteriaAirframeType.Trim + "')")
          End If
        End If

        If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeName.Trim) Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_make_name = '" + searchCriteria.HelpCriteriaMakeName.Trim + "')")
        End If

      End If

      sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_make_model_engine_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_make_model_engine_prefix_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_make_model_engine_prefix_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_aircraft_make_model_engine_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ameng_engine_prefix AS EnginePrefix, ameng_engine_core AS EngineCore, ameng_engine_suffix1 AS EngineSuffix1")
      sQuery.Append(" FROM Aircraft_Model_Engine WITH (NOLOCK)")
      sQuery.Append(" WHERE (ameng_amod_id = " + searchCriteria.HelpCriteriaTmpModelID.ToString + ")")
      sQuery.Append(" ORDER BY ameng_seq_no")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_make_model_engine_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_make_model_engine_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_make_model_engine_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_make_model_engine_prefix(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim results_table2 As New DataTable

    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strLastAirframe As String = ""
    Dim strLastAirType As String = ""
    Dim strLastMake As String = ""

    Dim strEngine As String = ""
    Dim strLastEngine As String = ""
    Dim strSeperator As String = ""

    Try

      results_table = get_aircraft_make_model_engine_prefix_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          If searchCriteria.HelpCriteriaModelID > -1 Then
            htmlOut.Append("<tr bgcolor='white'>")
            htmlOut.Append("<td align='right' valign='middle' colspan='3'><strong>")
            htmlOut.Append("<a href='masterLists.aspx?helplist=engineprefix&helpAirframe=&helpAirtype=&helpMake=&helpModel=' target='_self' title='Click to view master engine model prefix list'>back to master list</a>")
            htmlOut.Append("</strong></td></tr>")
          End If

          htmlOut.Append("<tr bgcolor='white'><td align='center' valign='bottom' class='seperator' colspan='3'><strong>")

          If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaAirframeType.Trim) Then
            Select Case searchCriteria.HelpCriteriaAirframeType.Trim
              Case crmWebClient.Constants.AMOD_FIXED_AIRFRAME
                htmlOut.Append("FIXED WING")
              Case crmWebClient.Constants.AMOD_ROTARY_AIRFRAME
                htmlOut.Append("ROTARY")
            End Select
            strSeperator = " - "
          End If

          If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
            Select Case searchCriteria.HelpCriteriaMakeType.Trim
              Case crmWebClient.Constants.AMOD_TYPE_AIRLINER
                htmlOut.Append(strSeperator + "JET AIRLINER")
              Case crmWebClient.Constants.AMOD_TYPE_JET
                htmlOut.Append(strSeperator + "BUSINESS JET")
              Case crmWebClient.Constants.AMOD_TYPE_PISTON
                htmlOut.Append(strSeperator + "PISTON")
              Case crmWebClient.Constants.AMOD_TYPE_TURBO
                Select Case searchCriteria.HelpCriteriaAirframeType.Trim
                  Case crmWebClient.Constants.AMOD_FIXED_AIRFRAME
                    htmlOut.Append(strSeperator + "TURBOPROP")
                  Case crmWebClient.Constants.AMOD_ROTARY_AIRFRAME
                    htmlOut.Append(strSeperator + "TURBINE")
                End Select
            End Select
            strSeperator = " - "
          End If

          If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeName.Trim) Then
            htmlOut.Append(strSeperator + searchCriteria.HelpCriteriaMakeName.Trim)
          End If

          htmlOut.Append("</strong></td></tr>")

          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Make</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Model</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Engine Model Prefix</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If r.Item("AirframeType").ToString.ToUpper.Trim <> strLastAirframe.ToUpper.Trim And searchCriteria.HelpCriteriaModelID = -1 Then
              If String.IsNullOrEmpty(searchCriteria.HelpCriteriaAirframeType.Trim) Then
                If Not toggleRowColor Then
                  htmlOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  htmlOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If
                htmlOut.Append("<td align='left' valign='middle' class='seperator' colspan='3'><strong>")
                htmlOut.Append("<a href='masterLists.aspx?helplist=engineprefix&helpAirframe=" + r.Item("AirframeType").ToString.ToUpper.Trim + "&helpAirtype=&helpMake=&helpModel=' target='_self' title='View By Airframe Type'>" + r.Item("AirframeTypeName").ToString.ToUpper.Trim + "</a>")
                htmlOut.Append("</strong></td></tr>")
              End If
            End If

            If r.Item("MakeType").ToString.ToUpper.Trim <> strLastAirType.ToUpper.Trim And searchCriteria.HelpCriteriaModelID = -1 Then
              If String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
                If Not toggleRowColor Then
                  htmlOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  htmlOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If
                htmlOut.Append("<td align='left' valign='middle' class='seperator' colspan='3'><strong>")
                htmlOut.Append("<a href='masterLists.aspx?helplist=engineprefix&helpAirframe=" + r.Item("AirframeType").ToString.ToUpper.Trim + "&helpAirtype=" + r.Item("MakeType").ToString.ToUpper.Trim + "&helpMake=&helpModel=' target='_self' title='View By Make Type'>" + r.Item("MakeTypeName").ToString.ToUpper.Trim + "</a>")
                htmlOut.Append("</strong></td></tr>")
              End If
            End If

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row' valign='top'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white' valign='top'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator'>")
            If r.Item("Make").ToString.ToUpper.Trim <> strLastMake.ToUpper.Trim And searchCriteria.HelpCriteriaModelID = -1 Then
              If String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeName.Trim) Then
                htmlOut.Append("<a href='masterLists.aspx?helplist=engineprefix&helpAirframe=" + r.Item("AirframeType").ToString.ToUpper.Trim + "&helpAirtype=" + r.Item("MakeType").ToString.ToUpper.Trim + "&helpMake=" + r.Item("Make").ToString.ToUpper.Trim + "&helpModel=' target='_self' title='View By Make Name'><strong>" + r.Item("Make").ToString.ToUpper.Trim + "<strong></a>")
              Else
                htmlOut.Append(r.Item("Make").ToString.Trim)
              End If
            Else
              htmlOut.Append(r.Item("Make").ToString.Trim)
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator'>")
            If searchCriteria.HelpCriteriaModelID = -1 Then
              htmlOut.Append("<a href='masterLists.aspx?helplist=engineprefix&helpAirframe=" + r.Item("AirframeType").ToString.ToUpper.Trim + "&helpAirtype=" + r.Item("MakeType").ToString.ToUpper.Trim + "&helpMake=" + r.Item("Make").ToString.ToUpper.Trim + "&helpModel=" + r.Item("ModelId").ToString.Trim + "' target='_self' title='View By Model'>" + r.Item("Model").ToString.Trim + "</a>")
            Else
              htmlOut.Append(r.Item("Model").ToString.Trim)
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='top' class='seperator'>")

            searchCriteria.HelpCriteriaTmpModelID = CLng(r.Item("ModelId").ToString)

            results_table2 = get_aircraft_make_model_engine_info(searchCriteria)

            If Not IsNothing(results_table2) Then

              If results_table2.Rows.Count > 0 Then

                For Each r2 As DataRow In results_table2.Rows

                  strEngine = ""

                  If Not IsDBNull(r2.Item("EnginePrefix")) Then
                    If Not String.IsNullOrEmpty(r2.Item("EnginePrefix").ToString.Trim) Then
                      strEngine = r2.Item("EnginePrefix").ToString.Trim
                    End If
                  End If

                  If Not IsDBNull(r2.Item("EngineCore")) Then
                    If Not String.IsNullOrEmpty(r2.Item("EngineCore").ToString.Trim) Then
                      strEngine += " " + r2.Item("EngineCore").ToString.Trim
                    End If
                  End If

                  If Not IsDBNull(r2.Item("EngineSuffix1")) Then
                    If Not String.IsNullOrEmpty(r2.Item("EngineSuffix1").ToString.Trim) Then
                      If r2.Item("EngineSuffix1").ToString.StartsWith(crmWebClient.Constants.cHyphen) Then
                        ' strEngine += crmWebClient.Constants.cHyphen
                        strEngine += " " + r2.Item("EngineSuffix1").ToString.Trim
                      End If
                    End If
                  End If

                  If Not String.IsNullOrEmpty(strEngine) And strLastEngine <> strEngine Then   ' And Not strEngine.Contains(crmWebClient.Constants.cHyphen)
                    htmlOut.Append(strEngine + "<br/>")
                  End If

                  strLastEngine = strEngine

                Next
              Else
                htmlOut.Append("&nbsp;")
              End If
            Else
              htmlOut.Append("&nbsp;")
            End If

            searchCriteria.HelpCriteriaTmpModelID = -1

            strLastAirframe = r.Item("AirframeType").ToString.ToUpper.Trim
            strLastAirType = r.Item("MakeType").ToString.ToUpper.Trim
            strLastMake = r.Item("Make").ToString.ToUpper.Trim

            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_make_model_engine_prefix(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing
    results_table2 = Nothing

  End Sub

  Public Function get_aircraft_emp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT emp_provider_name, emp_program_name, COUNT(*) AS tcount")
      sQuery.Append(" FROM Engine_Maintenance_Program WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft ON emp_id = ac_engine_maintenance_prog_EMP")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(" INNER JOIN Aircraft_Model ON ac_amod_id = amod_id")
      End If

      sQuery.Append(" WHERE (ac_journ_id = 0) AND (emp_provider_name <> 'Unknown')")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
      End If

      sQuery.Append(" GROUP BY emp_provider_name, emp_program_name ORDER BY tcount DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_emp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_emp_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_emp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_emp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_emp_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='middle' class='seperator' colspan='2'><strong>")

          Select Case searchCriteria.HelpCriteriaMakeType.Trim
            Case crmWebClient.Constants.AMOD_TYPE_AIRLINER
              htmlOut.Append("JET AIRLINER")
            Case crmWebClient.Constants.AMOD_TYPE_JET
              htmlOut.Append("BUSINESS JET")
            Case crmWebClient.Constants.AMOD_TYPE_PISTON
              htmlOut.Append("PISTON")
            Case crmWebClient.Constants.AMOD_TYPE_TURBO
              htmlOut.Append("TURBOPROP/TURBINE")
            Case Else
              htmlOut.Append("ALL AIRCRAFT")
          End Select

          htmlOut.Append("</td></tr>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Provider/Program</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Count</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("emp_provider_name").ToString.Trim)
            htmlOut.Append(" / " + r.Item("emp_program_name").ToString.Trim + "</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("tcount").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_emp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_emgp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT emgp_provider_name, emgp_program_name, COUNT(*) AS tcount")
      sQuery.Append(" FROM Engine_Management_Program WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft ON emgp_id = ac_engine_Management_prog_EMGP")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(" INNER JOIN Aircraft_Model ON ac_amod_id = amod_id")
      End If

      sQuery.Append(" WHERE (ac_journ_id = 0) AND (emgp_provider_name <> 'Unknown')")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
      End If

      sQuery.Append(" GROUP BY emgp_provider_name, emgp_program_name ORDER BY tcount DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_emgp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_emgp_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_emgp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_emgp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_emgp_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='middle' class='seperator' colspan='2'><strong>")

          Select Case searchCriteria.HelpCriteriaMakeType.Trim
            Case crmWebClient.Constants.AMOD_TYPE_AIRLINER
              htmlOut.Append("JET AIRLINER")
            Case crmWebClient.Constants.AMOD_TYPE_JET
              htmlOut.Append("BUSINESS JET")
            Case crmWebClient.Constants.AMOD_TYPE_PISTON
              htmlOut.Append("PISTON")
            Case crmWebClient.Constants.AMOD_TYPE_TURBO
              htmlOut.Append("TURBOPROP/TURBINE")
            Case Else
              htmlOut.Append("ALL AIRCRAFT")
          End Select

          htmlOut.Append("</td></tr>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Provider/Program</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Count</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("emgp_provider_name").ToString.Trim)
            htmlOut.Append(" / " + r.Item("emgp_program_name").ToString.Trim + "</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("tcount").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_emgp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_amp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amp_provider_name, amp_program_name, COUNT(*) AS tcount")
      sQuery.Append(" FROM Airframe_Maintenance_Program WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft ON amp_id = ac_airframe_maintenance_prog_amp")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(" INNER JOIN Aircraft_Model ON ac_amod_id = amod_id")
      End If

      sQuery.Append(" WHERE (ac_journ_id = 0) AND (amp_provider_name <> 'Unknown')")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
      End If

      sQuery.Append(" GROUP BY amp_provider_name, amp_program_name ORDER BY tcount DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_amp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_amp_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_amp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_amp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_amp_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='middle' class='seperator' colspan='2'><strong>")

          Select Case searchCriteria.HelpCriteriaMakeType.Trim
            Case crmWebClient.Constants.AMOD_TYPE_AIRLINER
              htmlOut.Append("JET AIRLINER")
            Case crmWebClient.Constants.AMOD_TYPE_JET
              htmlOut.Append("BUSINESS JET")
            Case crmWebClient.Constants.AMOD_TYPE_PISTON
              htmlOut.Append("PISTON")
            Case crmWebClient.Constants.AMOD_TYPE_TURBO
              htmlOut.Append("TURBOPROP/TURBINE")
            Case Else
              htmlOut.Append("ALL AIRCRAFT")
          End Select

          htmlOut.Append("</td></tr>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Provider/Program</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Count</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amp_provider_name").ToString.Trim)
            htmlOut.Append(" / " + r.Item("amp_program_name").ToString.Trim + "</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("tcount").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_amp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_amtp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amtp_provider_name, amtp_program_name, COUNT(*) AS tcount")
      sQuery.Append(" FROM Airframe_Maintenance_Tracking_Program WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft ON amtp_id = ac_airframe_maint_tracking_prog_amtp")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(" INNER JOIN Aircraft_Model ON ac_amod_id = amod_id")
      End If

      sQuery.Append(" WHERE (ac_journ_id = 0) AND (amtp_provider_name <> 'Unknown')")

      If Not String.IsNullOrEmpty(searchCriteria.HelpCriteriaMakeType.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "(amod_type_code = '" + searchCriteria.HelpCriteriaMakeType.Trim + "')")
      End If

      sQuery.Append(" GROUP BY amtp_provider_name, amtp_program_name ORDER BY tcount DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_amtp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_amtp_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_amtp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_aircraft_model_business_types(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" select distinct amod_make_name, amod_model_name, amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, afmt_description   ")
      sQuery.Append(" from aircraft_model with (NOLOCK)  ")
      sQuery.Append(" inner join aircraft with (NOLOCK) on ac_amod_id = amod_id and ac_journ_id = 0  ")
      sQuery.Append(" inner join Airframe_Make_type with (NOLOCK) on afmt_code = amod_type_code and afmt_airframetype  = amod_airframe_type_code   ")
      sQuery.Append(" where ac_journ_id = 0 ")
      sQuery.Append(" order by afmt_description asc, amod_make_name asc, amod_product_business_flag, amod_product_commercial_flag, amod_product_helicopter_flag, amod_model_name asc  ")

 
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_amtp_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_model_business_types load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_model_business_types(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_amtp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_amtp_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='left' valign='middle' class='seperator' colspan='2'><strong>")

          Select Case searchCriteria.HelpCriteriaMakeType.Trim
            Case crmWebClient.Constants.AMOD_TYPE_AIRLINER
              htmlOut.Append("JET AIRLINER")
            Case crmWebClient.Constants.AMOD_TYPE_JET
              htmlOut.Append("BUSINESS JET")
            Case crmWebClient.Constants.AMOD_TYPE_PISTON
              htmlOut.Append("PISTON")
            Case crmWebClient.Constants.AMOD_TYPE_TURBO
              htmlOut.Append("TURBOPROP/TURBINE")
            Case Else
              htmlOut.Append("ALL AIRCRAFT")
          End Select

          htmlOut.Append("</td></tr>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Provider / Program</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Count</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amtp_provider_name").ToString.Trim)
            htmlOut.Append(" / " + r.Item("amtp_program_name").ToString.Trim + "</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("tcount").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_amtp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub help_display_aircraft_model_types(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_model_business_types(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")
 
          htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Aircraft Model Name</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Business<br/>Product</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Commercial<br/>Product</strong></td>")
          htmlOut.Append("<td align='center' valign='bottom'><strong>Helicopter<br/>Product</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Description</strong></td>")
          htmlOut.Append("</tr>") 

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If 

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_make_name").ToString.Trim & " " & r.Item("amod_model_name"))
            htmlOut.Append("</td>")  

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amod_product_business_flag").ToString.Trim)
            htmlOut.Append("</td>") 

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amod_product_commercial_flag").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amod_product_helicopter_flag").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("afmt_description").ToString.Trim)
            htmlOut.Append("</td>") 

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_amtp(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_transaction_codes_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.HelpCriteriaTransactionSendToWeb Then
        sQuery.Append("SELECT jcat_subcategory_code, jcat_subcategory_transtype, jcat_subcategory_transtofrom")
        sQuery.Append(" FROM Journal_Category WITH (NOLOCK)")
        sQuery.Append(" WHERE (jcat_category_code = 'AH')")
        sQuery.Append(" AND (jcat_subcategory_code Not Like '%CORR%')")
        sQuery.Append(" AND (jcat_subcategory_transtofrom IS NOT NULL)")
        sQuery.Append(" AND (jcat_subcategory_transtofrom <> '')")
        sQuery.Append(" AND (jcat_send_to_website = 'Y')")
        sQuery.Append(" ORDER BY jcat_subcategory_code")
      Else
        sQuery.Append("SELECT jcat_category_code, jcat_category_name, jcat_subcategory_code, jcat_subcategory_name")
        sQuery.Append(" FROM Journal_Category WITH (NOLOCK)")
        sQuery.Append(" WHERE (jcat_send_to_website = 'N')")
        sQuery.Append(" OR ((jcat_category_code = 'AH')")
        sQuery.Append(" AND (jcat_subcategory_transtofrom IS NOT NULL OR jcat_subcategory_transtofrom <> '')")
        sQuery.Append(" AND (jcat_send_to_website = 'Y'))")
        sQuery.Append(" ORDER BY jcat_category_code, jcat_subcategory_code")
      End If


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_transaction_codes_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_transaction_codes_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_transaction_codes_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_transaction_codes(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_transaction_codes_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          If searchCriteria.HelpCriteriaTransactionSendToWeb Then
            htmlOut.Append("<tr><td align='left' valign='bottom'><strong>Trans Code</strong></td>")
            htmlOut.Append("<td align='left' valign='bottom'><strong>Trans Type</strong></td>")
            htmlOut.Append("<td align='left' valign='bottom'><strong>To-From</strong></td></tr>")
          Else
            htmlOut.Append("<tr><td colspan='4' align='center'><strong>Misc Transaction Codes List By Code</strong></td></tr>")
            htmlOut.Append("<tr><td align='left' valign='bottom'><strong>JCat Code</strong></td>")
            htmlOut.Append("<td align='left' valign='bottom'><strong>JCat Name</strong></td>")
            htmlOut.Append("<td align='left' valign='bottom'><strong>JCat Type</strong></td>")
            htmlOut.Append("<td align='left' valign='bottom'><strong>JCat Type Name</strong></td></tr>")
          End If

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If searchCriteria.HelpCriteriaTransactionSendToWeb Then

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_subcategory_code").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_subcategory_transtype").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_subcategory_transtofrom").ToString.Trim)
              htmlOut.Append("</td></tr>")

            Else

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_category_code").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_category_name").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_subcategory_code").ToString.Trim)
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("jcat_subcategory_name").ToString.Trim)
              htmlOut.Append("</td></tr>")

            End If

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If

    Catch ex As Exception

      aError = "Error in help_display_transaction_codes(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_company_business_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      ' this is strictly evo site, yacht types will show up on the yacht site
      sQuery.Append("SELECT cbus_type, cbus_name, cbus_description")
      sQuery.Append(" FROM Company_Business_Type WITH (NOLOCK) WHERE")

      If Not HttpContext.Current.Session.Item("localPreferences").isYachtOnlyProduct Then
        sQuery.Append(" cbus_aircraft_flag = 'Y'")
      Else
        sQuery.Append(" cbus_yacht_flag = 'Y'")
      End If

      sQuery.Append(" ORDER BY cbus_type")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_company_business_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_company_business_type_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_company_business_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_company_business_type(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_company_business_type_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Code</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Name</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Description</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("cbus_type").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("cbus_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("cbus_description").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_company_business_type(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_aircraft_contact_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT actype_code, actype_name")
      sQuery.Append(" FROM Aircraft_Contact_Type WITH (NOLOCK)")
      sQuery.Append(" WHERE (actype_name_code IS NULL OR actype_name_code = '')")
      sQuery.Append(" AND (actype_compref_flag = 'N') AND (actype_compref_twoway_flag = 'N')")
      sQuery.Append(" AND (actype_code <> '71')") ' Research Only
      sQuery.Append(" ORDER BY actype_code")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_aircraft_contact_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_aircraft_contact_type_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_aircraft_contact_type_info(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_aircraft_contact_type(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_aircraft_contact_type_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Code</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Name</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("actype_code").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("actype_name").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_aircraft_contact_type(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_business_aircraft_sizes(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amjiqs_cat_desc, amod_make_name, amod_model_name")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model_JIQ_Size WITH(NOLOCK) ON amod_jniq_size = amjiqs_cat_code")
      sQuery.Append(" ORDER BY amjiqs_cat_desc, amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_business_aircraft_sizes(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_business_aircraft_sizes load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_business_aircraft_sizes(ByRef searchCriteria As helpSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub help_display_business_aircraft_sizes(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False
    Dim strExample As String = ""

    Try

      results_table = get_business_aircraft_sizes(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")

          htmlOut.Append("<tr><td align='center' valign='bottom'><strong>Size</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Make</strong></td>")
          htmlOut.Append("<td align='left' valign='bottom'><strong>Model</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amjiqs_cat_desc").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='center' valign='middle' class='seperator'>" + r.Item("amod_make_name").ToString.Trim)
            htmlOut.Append("</td>")

            htmlOut.Append("<td align='left' valign='middle' class='seperator'>" + r.Item("amod_model_name").ToString.Trim)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='helpListingDataTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in help_display_business_aircraft_sizes(ByRef searchCriteria As helpSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function GetHelpTopicBySection(ByVal evotopName As String) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try


      'Opening Connection
      SqlConn.ConnectionString = adminConnectStr
      SqlConn.Open()

      sql = "select distinct evotop_id "
      sql += "from Evolution_Notifications with (NOLOCK)"
      sql += "inner join Evolution_Topic_Index with (NOLOCK) on evonot_id= evotopind_evonot_id "
      sql += "inner join Evolution_Topics with (NOLOCK) on evotop_id= evotopind_evotop_id "
      sql += "where evotop_name = @evotop_name "
      sql += "and evonot_release_type = 'H' "
      sql += "and evonot_evo_dotnet_flag = 'Y' "
      sql += "and evonot_active_flag='Y' "

      'save to session query debug string.
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      SqlCommand.Parameters.AddWithValue("evotop_name", evotopName)

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
      GetHelpTopicBySection = Nothing
      Me.class_error = "Error in GetHelpTopicBySection(ByVal evotopName As String) As DataTable: SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing



    End Try

  End Function

End Class
