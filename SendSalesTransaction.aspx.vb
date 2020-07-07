' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/SendSalesTransaction.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:41a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: SendSalesTransaction.aspx.vb $
'
' ********************************************************************************

Partial Public Class SendSalesTransaction
  Inherits System.Web.UI.Page
  Dim aircraftID As Long = 0
  Dim JournalID As Long = 0
  Dim ModelID As Long = 0
  Dim SendPrice As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load user session : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      Master.RemoveAllStyleElements(True)
      Master.SetPageTitle("Submit Transaction Data to JETNET")
      'We need the Aircraft ID:
      If Not IsNothing(Request.Item("acid")) Then
        If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
          aircraftID = CLng(Request.Item("acid").ToString.Trim)
        End If
      End If

      'We need the Journal ID:
      If Not IsNothing(Request.Item("jID")) Then
        If Not String.IsNullOrEmpty(Request.Item("jID").ToString) Then
          JournalID = CLng(Request.Item("jID").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("ModelID")) Then
        If Not String.IsNullOrEmpty(Request.Item("ModelID").ToString) Then
          ModelID = CLng(Request.Item("ModelID").ToString.Trim)
        End If
      End If


      'We need to check Send Sales to see if it's passed.
      If Not IsNothing(Request.Item("sendSales")) Then
        If Not String.IsNullOrEmpty(Request.Item("sendSales").ToString) Then
          SendPrice = True
        End If
      End If

      If SendPrice = True Then
        sales_pre_submittal_form.Visible = True

        'We need to verify the price range:
        If ModelID > 0 Then
          Dim ModelTable As New DataTable
          ModelTable = ReturnLowOrHighPriceRangeForModel(ModelID)
          If Not IsNothing(ModelTable) Then
            If ModelTable.Rows.Count > 0 Then
              salesPriceRange.MinimumValue = CDbl(ModelTable.Rows(0).Item("LOWRANGE"))
              salesPriceRange.MaximumValue = CDbl(ModelTable.Rows(0).Item("HIGHRANGE"))
              salesPriceRange.ErrorMessage = "Your sale price is outside of the range of pricing establed for this aircraft [" & FormatCurrency(ModelTable.Rows(0).Item("LOWRANGE"), 0).ToString & "-" & FormatCurrency(ModelTable.Rows(0).Item("HIGHRANGE"), 0).ToString & "]."
              salesPriceRange.Enabled = True

              askingPriceRange.MinimumValue = CDbl(ModelTable.Rows(0).Item("LOWRANGE"))
              askingPriceRange.MaximumValue = CDbl(ModelTable.Rows(0).Item("HIGHRANGE"))
              askingPriceRange.ErrorMessage = "Your asking price is outside of the range of pricing establed for this aircraft [" & FormatCurrency(ModelTable.Rows(0).Item("LOWRANGE"), 0).ToString & "-" & FormatCurrency(ModelTable.Rows(0).Item("HIGHRANGE"), 0).ToString & "]."
              askingPriceRange.Enabled = True
            End If
          End If
        End If

        If aircraftID > 0 Then
          Dim AircraftTable As New DataTable
          Dim JournalTable As New DataTable
          JournalTable = Master.aclsData_Temp.GetJETNET_Historical_Data(aircraftID, JournalID, Session.Item("localSubscription").crmAerodexFlag)
          AircraftTable = Master.aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(aircraftID, JournalID)
          journal_information.Text = "<strong>Aircraft Information:</strong> "


          If Not IsNothing(AircraftTable) Then
            If AircraftTable.Rows.Count > 0 Then
              If Not IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) Then
                journal_information.Text += AircraftTable.Rows(0).Item("amod_make_name").ToString
              End If

              If Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name")) Then
                journal_information.Text += " " & AircraftTable.Rows(0).Item("amod_model_name").ToString
              End If

              If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) Then
                journal_information.Text += " S/N: " & AircraftTable.Rows(0).Item("ac_ser_nbr").ToString
              End If
            End If
          End If

          journal_information.Text += "<br />"
          If Not IsNothing(JournalTable) Then
            If JournalTable.Rows.Count > 0 Then
              If Not IsDBNull(JournalTable.Rows(0).Item("journ_date")) Then
                journal_information.Text += "<strong>Date:</strong> " & crmWebClient.clsGeneral.clsGeneral.datenull(JournalTable.Rows(0).Item("journ_date"))
                SalesTransactionDate.Text = JournalTable.Rows(0).Item("journ_date")
                SalesTransactionDate.Enabled = False
              End If
              If Not IsDBNull(JournalTable.Rows(0).Item("journ_subject")) Then
                journal_information.Text += "<br /><strong>Subject:</strong> " & JournalTable.Rows(0).Item("journ_subject").ToString
              End If
            End If
          End If

        End If
      End If

    End If

  End Sub
  Public Sub checkCheckbox(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

    If agreeToTerms.Checked = False Then
      args.IsValid = False
      Exit Sub
    Else
      args.IsValid = True
    End If
  End Sub

  Public Sub HaveOnePrice(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

    If String.IsNullOrEmpty(sale_price.Text) And String.IsNullOrEmpty(asking_price.Text) Then
      args.IsValid = False
      Exit Sub
    Else
      args.IsValid = True
    End If
  End Sub

  Private Sub agreeToTerms_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles agreeToTerms.CheckedChanged
    If agreeToTerms.Checked Then
      SubmitTransactionData.Enabled = True
      SubmitTransactionData.CssClass = ""
    Else
      SubmitTransactionData.Enabled = False
      SubmitTransactionData.CssClass = "disabledSalesButton"
    End If
  End Sub

  Public Function Create_Aircraft_Value(ByVal acval_date As String, ByVal acval_sub_id As Integer, ByVal acval_login As String, ByVal acval_seq_no As Integer, ByVal acval_contact_name As String, ByVal acval_amod_id As Integer, ByVal acval_ac_id As Long, ByVal acval_journ_id As Long, ByVal acval_airframe_tot_hrs As Nullable(Of Integer), ByVal acval_airframe_tot_landings As Nullable(Of Integer), ByVal acval_asking_price As Double, ByVal acval_sale_price As Double, ByVal acval_notes As String, ByRef ErrorString As String, ByVal acval_comp_id As Long) As Boolean
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim ResponseCode As Boolean = False
    Try

      QueryFields = "insert into Aircraft_Value(acval_date, "
      QueryValues = " values (@acval_date,"


      QueryFields += " acval_type, "
      QueryValues += " @acval_type, "

      QueryFields += " acval_sub_id, "
      QueryValues += " @acval_sub_id, "

      QueryFields += " acval_login, "
      QueryValues += " @acval_login, "

      QueryFields += " acval_seq_no, "
      QueryValues += " @acval_seq_no, "

      QueryFields += " acval_contact_name, "
      QueryValues += " @acval_contact_name, "

      QueryFields += " acval_amod_id, "
      QueryValues += " @acval_amod_id, "

      QueryFields += " acval_ac_id, "
      QueryValues += " @acval_ac_id, "

      QueryFields += " acval_comp_id, "
      QueryValues += " @acval_comp_id, "

      QueryFields += " acval_journ_id, "
      QueryValues += " @acval_journ_id, "

      QueryFields += " acval_airframe_tot_hrs, "
      QueryValues += " @acval_airframe_tot_hrs, "

      QueryFields += " acval_airframe_tot_landings, "
      QueryValues += " @acval_airframe_tot_landings, "

      QueryFields += " acval_asking_price, "
      QueryValues += " @acval_asking_price, "

      QueryFields += " acval_sale_price, "
      QueryValues += " @acval_sale_price, "

      QueryFields += " acval_webaction_date, "
      QueryValues += " @acval_webaction_date, "

      QueryFields += " acval_notes) "
      QueryValues += " @acval_notes) "

      Query = QueryFields & QueryValues

      SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase")
      SqlConn.Open()


      Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)
      SqlCommand.Parameters.AddWithValue("@acval_date", acval_date)
      SqlCommand.Parameters.AddWithValue("@acval_type", "SOLD")
      SqlCommand.Parameters.AddWithValue("@acval_sub_id", acval_sub_id)
      SqlCommand.Parameters.AddWithValue("@acval_login", acval_login)
      SqlCommand.Parameters.AddWithValue("@acval_seq_no", acval_seq_no)
      SqlCommand.Parameters.AddWithValue("@acval_contact_name", acval_contact_name)
      SqlCommand.Parameters.AddWithValue("@acval_amod_id", acval_amod_id)
      SqlCommand.Parameters.AddWithValue("@acval_ac_id", acval_ac_id)
      SqlCommand.Parameters.AddWithValue("@acval_comp_id", acval_comp_id)
      SqlCommand.Parameters.AddWithValue("@acval_journ_id", acval_journ_id)

      If IsNothing(acval_airframe_tot_hrs) Then
        SqlCommand.Parameters.AddWithValue("@acval_airframe_tot_hrs", DBNull.Value)
      Else
        SqlCommand.Parameters.AddWithValue("@acval_airframe_tot_hrs", acval_airframe_tot_hrs)
      End If

      If IsNothing(acval_airframe_tot_landings) Then
        SqlCommand.Parameters.AddWithValue("@acval_airframe_tot_landings", DBNull.Value)
      Else
        SqlCommand.Parameters.AddWithValue("@acval_airframe_tot_landings", acval_airframe_tot_landings)
      End If

      SqlCommand.Parameters.AddWithValue("@acval_asking_price", acval_asking_price)
      SqlCommand.Parameters.AddWithValue("@acval_sale_price", acval_sale_price)
      SqlCommand.Parameters.AddWithValue("@acval_webaction_date", "1/1/1900")
      SqlCommand.Parameters.AddWithValue("@acval_notes", acval_notes)
      SqlCommand.ExecuteNonQuery()


      ResponseCode = True

      SqlCommand.Dispose()
      SqlCommand = Nothing


    Catch ex As Exception
      ErrorString = "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"
      Return False
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
    Return ResponseCode
  End Function

  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Method name: ReturnLowOrHighPriceRangeForModel
  ' Purpose: To Return Lowest/Highest Possible Price Range
  ' Parameters: amod_ID
  ' Return: 
  '       integer
  ' Change Log
  '           5/2/2016    - Created By: Amanda Vaughn
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function ReturnLowOrHighPriceRangeForModel(ByVal modelID As Long) As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable

    Try
      If modelID > 0 Then
        'Opening Connection
        SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase")
        SqlConn.Open()

        sql = "select (amod_start_price-(amod_start_price*.2)) as LOWRANGE, "
        sql += " (amod_end_price + (amod_end_price * .2)) as HIGHRANGE"
        sql += " from Aircraft_Model with (NOLOCK)"
        sql += " where (amod_id = @modelID)"

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)


        Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


        SqlCommand.Parameters.AddWithValue("@modelID", modelID)

        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing

      End If

      Return TempTable

    Catch ex As Exception
      ReturnLowOrHighPriceRangeForModel = Nothing
      'Me.class_error = "Error in ReturnLowOrHighPriceRangeForModel(ByVal modelID As Long) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing
    End Try

  End Function

  Private Sub SubmitTransactionData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubmitTransactionData.Click
    If Page.IsValid Then 'We need to make sure the page came back as valid
      Try
        Dim ReturnValue As Boolean = False
        Dim ErrorString As String = ""

        'What we looked up about the transaction:
        Dim acval_date As String = SalesTransactionDate.Text 'put in the date of the transaction on the page 

        'What we should know about the page:
        Dim acval_sub_id As Integer = HttpContext.Current.Session.Item("localUser").crmSubSubID 'put the users subscription id in this field
        Dim acval_login As String = HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString 'put the users subscription login in this field
        Dim acval_seq_no As Integer = HttpContext.Current.Session.Item("localUser").crmSubSeqNo 'put the users subscription sequence number in this field
        Dim acval_comp_id As Long = HttpContext.Current.Session.Item("localUser").crmUserCompanyID 'put the users company ID in this field
        Dim acval_contact_name As String = HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName 'put the first name &  last name into this field of the person logged in
        Dim acval_amod_id As Integer = ModelID 'put jetnet model id
        Dim acval_ac_id As Long = aircraftID 'put jetnet aircraft id
        Dim acval_journ_id As Long = JournalID 'put jetnet journal id

        'User Input
        Dim acval_airframe_tot_hrs As New Nullable(Of Integer) 'put the total hours from page
        Dim acval_airframe_tot_landings As New Nullable(Of Integer) 'put the total landings from page
        Dim acval_asking_price As Double = 0 'put the asking price from page, if any/otherwise ignore.
        Dim acval_sale_price As Double = 0 'put sold price here.
        Dim acval_notes As String = "" 'put in the value notes from above.

        'Filling up aftt, only if numeric/not empty. Otherwise a null is being passed.
        If Not String.IsNullOrEmpty(aftt.Text) Then
          If IsNumeric(aftt.Text) Then
            acval_airframe_tot_hrs = aftt.Text
          End If
        End If

        'Filling up landings, only if numeric/not empty. Otherwise a null is being passed.
        If Not String.IsNullOrEmpty(total_landings.Text) Then
          If IsNumeric(total_landings.Text) Then
            acval_airframe_tot_landings = total_landings.Text
          End If
        End If

        'Filling up sale price, only if not empty and numeric.
        If Not String.IsNullOrEmpty(sale_price.Text) Then
          If IsNumeric(sale_price.Text) Then
            acval_sale_price = sale_price.Text
          End If
        End If

        'Going on to fill up asking price, needs to be numeric
        If Not String.IsNullOrEmpty(asking_price.Text) Then
          If IsNumeric(asking_price.Text) Then
            acval_asking_price = asking_price.Text
          End If
        End If

        ReturnValue = Create_Aircraft_Value(acval_date, acval_sub_id, acval_login, acval_seq_no, acval_contact_name, acval_amod_id, acval_ac_id, acval_journ_id, acval_airframe_tot_hrs, acval_airframe_tot_landings, acval_asking_price, acval_sale_price, acval_notes, ErrorString, acval_comp_id)

        If ReturnValue = True Then 'This means it was inserted
          'Record a Subscription_Install record for the aircraft that says "Submitted Asking/Sold Transaction Data JETNET for use." – include the aircraft and journal Id on the log record if you can.
          Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Submitted Asking/Sold Transaction Data JETNET for use.", Nothing, 0, JournalID, 0, 0, 0, aircraftID, ModelID)

          'Display Thank you message:
          sales_pre_submittal_form.Visible = False
          post_submittal_form.Visible = True
        Else 'Error occurred.
          Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace("SendSalesTransaction : " & ErrorString, "'", "''"), Nothing, 0, 0, 0, 0, 0)
          'Display Error message:
          sales_pre_submittal_form.Visible = False
          error_submittal_form.Visible = True
        End If
      Catch ex As Exception
        Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace("SendSalesTransaction : " & ex.Message.ToString, "'", "''"), Nothing, 0, 0, 0, 0, 0)
        'Display Error message:
        sales_pre_submittal_form.Visible = False
        error_submittal_form.Visible = True
      End Try
    End If
  End Sub
End Class