' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Appraisal.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:37a $
'$$Modtime: 6/18/19 6:11p $
'$$Revision: 2 $
'$$Workfile: Appraisal.aspx.vb $
'
' ********************************************************************************

Partial Public Class Appraisal
  Inherits System.Web.UI.Page 

  'Public cancleAndClose As Boolean = False
  'Public currentBrowseRec As Long = 0
  'Public fromView As String = ""
  Private AircraftID As Long = 0
  Private CompanyID As Long = 0
  Private YachtID As Long = 0

  Private journalID As Long = 0
  Private reminderID As Long = 0
  Public aclsData_Temp As New clsData_Manager_SQL
  Private JournalTable As New DataTable
  Private AircraftTable As New DataTable

  Private TypeOfNote As String = "Aircraft"
  Public bIsNote As Boolean = False
  Public bIsProspect As Boolean = False
  Public bIsUpdate As Boolean = False
  Dim NoteTitle As String = ""


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load user session : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      Master.SetPageTitle("Appraisal")  ' sets the page title and page.text


      Dim aTempTable As New DataTable
      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
      aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")


      'what Aircraft is it attached to?
      If Not IsNothing(Request.Item("acid")) Then
        If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
          If IsNumeric(Request.Item("acid").Trim) Then
            AircraftID = Request.Item("acid").Trim 
          End If
        End If
      End If
      'What yt is it attached to?
      If Not IsNothing(Request.Item("ytid")) Then
        If Not String.IsNullOrEmpty(Request.Item("ytid").ToString) Then
          If IsNumeric(Request.Item("ytid").Trim) Then
            YachtID = Request.Item("ytid").Trim
            TypeOfNote = "Yacht"
          End If
        End If
      End If


      'what company is it attached to?
      If Not IsNothing(Request.Item("compid")) Then
        If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
          If IsNumeric(Request.Item("compid").Trim) Then
            CompanyID = Request.Item("compid").Trim
          End If
        End If
      End If

      If Not IsPostBack Then
        If Trim(Request("ID")) <> "" Then
          Me.save_button.Visible = False
          Me.remove_app.Visible = True
          Me.update_app.Visible = True
          Call Load_Appraisal(Trim(Request("ID")), AircraftID)
        Else
          Me.save_button.Visible = True
          Me.remove_app.Visible = False
          Me.update_app.Visible = False
        End If
      End If


      generateItemDetails(Master)

      End If

  End Sub
  Public Sub Load_Appraisal(ByVal temp_id As Long, ByRef ac_id As Long)

    Dim results_table As New DataTable
    Dim sqlconn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sql As String = ""


    Try


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>DeletePhoneNumbers_contactID(ByVal contact_id As Integer, ByVal comp_id As Integer) As Integer</b><br />" & sql

      sqlconn.ConnectionString = Session.Item("jetnetClientDatabase")
      sqlconn.Open()
      SqlCommand.Connection = sqlconn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60



      sql = " Select * from Aircraft_Appraisal where acappr_id = " & temp_id

      SqlCommand.CommandText = sql
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        results_table.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
        ' results_table = "Error in Get_CRM_VIEW_Prospects load datatable" + constrExc.Message
      End Try


      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            'acappr_source
            Me.type_of.SelectedValue = r.Item("acappr_type")

            'acappr_entry_date
            Me.date_of_appraisal.Text = r.Item("acappr_date")
            ' acappr_subid
            'acappr_login
            'acappr_seq_no
            'acappr_comp_id
            'acappr_contact_id
            'acappr_amod_id 

            ac_id = r.Item("acappr_ac_id")

            If Not IsDBNull(r.Item("acappr_airframe_tot_hrs")) Then
              Me.aftt.Text = r.Item("acappr_airframe_tot_hrs")
            End If

            If Not IsDBNull(r.Item("acappr_airframe_tot_landings")) Then
              Me.cycles.Text = r.Item("acappr_airframe_tot_landings")
            End If

            If Not IsDBNull(r.Item("acappr_asking_price")) Then
              Me.asking_price.Text = FormatNumber(r.Item("acappr_asking_price"), 0)
            End If

            If Not IsDBNull(r.Item("acappr_take_price")) Then
              Me.take_price.Text = FormatNumber(r.Item("acappr_take_price"), 0)
            End If

            If Not IsDBNull(r.Item("acappr_est_value")) Then
              Me.est_value.Text = FormatNumber(r.Item("acappr_est_value"), 0)
            End If

            If Not IsDBNull(r.Item("acappr_notes")) Then
              Me.notes_text.Text = r.Item("acappr_notes")
            End If


            'acappr_webaction_date

          Next


        End If
      End If


    Catch ex As Exception
    Finally
      sqlconn.Dispose()
      sqlconn.Close()
      sqlconn = Nothing
      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub
  Public Function Update_Appraisal(ByVal appraisal_id As Long) As Integer
    Dim sqlconn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sql As String = ""

    Update_Appraisal = 0

    Try

      If appraisal_id > 0 Then

        sql = " Update Aircraft_Appraisal set "
        sql &= " acappr_source = 'Subscriber' "
        sql &= ", acappr_type = '" & Me.type_of.SelectedValue & "' "
        sql &= ", acappr_entry_date = '" & DateTime.Now & "' "
        sql &= ", acappr_date = '" & Me.date_of_appraisal.Text & "' "
        sql &= ", acappr_ac_id = "

        If Trim(Me.ac_id.Text) <> "" Then
          sql &= "" & Trim(Me.ac_id.Text) & ""
        Else
          sql &= "0"
        End If

        sql &= ", acappr_airframe_tot_hrs = "

        If Me.aftt.Text <> "" Then
          If IsNumeric(Me.aftt.Text) = True Then
            sql &= "" & Me.aftt.Text & ""
          Else
            sql &= "NULL"
          End If
        Else
          sql &= "NULL"
        End If

        sql &= ", acappr_airframe_tot_landings = "

        If Me.cycles.Text <> "" Then
          If IsNumeric(Me.cycles.Text) = True Then
            sql &= "" & Me.cycles.Text & ""
          Else
            sql &= "NULL"
          End If
        Else
          sql &= "NULL"
        End If

        sql &= ", acappr_asking_price = "
 
        If Me.asking_price.Text <> "" Then
          If IsNumeric(Me.asking_price.Text) = True Then
            sql &= "" & replace_numbers(Me.asking_price.Text) & ""
          Else
            sql &= "NULL"
          End If
        Else
          sql &= "NULL"
        End If


        sql &= ", acappr_take_price = "

        If Me.take_price.Text <> "" Then
          If IsNumeric(Me.take_price.Text) = True Then
            sql &= "" & replace_numbers(Me.take_price.Text) & ""
          Else
            sql &= "NULL"
          End If
        Else
          sql &= "NULL"
        End If


        sql &= ", acappr_est_value = "

        If Me.est_value.Text <> "" Then
          If IsNumeric(Me.est_value.Text) = True Then
            sql &= "" & replace_numbers(Me.est_value.Text) & ""
          Else
            sql &= "NULL"
          End If
        Else
          sql &= "NULL"
        End If

        sql &= ", acappr_notes = '"
 
        sql &= "" & replace_notes(Me.notes_text.Text) & ""

        sql &= "'"

        sql &= ", acappr_webaction_date = '1900-01-01 00:00:00.000' "

        sql &= " where acappr_id = " & appraisal_id

        sql = sql

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>DeletePhoneNumbers_contactID(ByVal contact_id As Integer, ByVal comp_id As Integer) As Integer</b><br />" & sql

        sqlconn.ConnectionString = Session.Item("jetnetClientDatabase")
        sqlconn.Open()
        SqlCommand.Connection = sqlconn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sql
        SqlCommand.ExecuteNonQuery()
        Update_Appraisal = 1

        Me.action_text.Text = "Your Appraisal Has Been Updated"
        Me.action_text.Visible = True

      End If


    Catch ex As Exception
      Update_Appraisal = 0
    Finally
      sqlconn.Dispose()
      sqlconn.Close()
      sqlconn = Nothing
      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function
  Public Function Remove_Appraisal(ByVal appraisal_id As Long) As Integer
    Dim sqlconn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sql As String = ""

    Remove_Appraisal = 0

    Try

      If appraisal_id > 0 Then

        sql = " Delete from Aircraft_Appraisal "
        sql &= " where acappr_id = " & appraisal_id

        sql = sql

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>DeletePhoneNumbers_contactID(ByVal contact_id As Integer, ByVal comp_id As Integer) As Integer</b><br />" & sql

        sqlconn.ConnectionString = Session.Item("jetnetClientDatabase")
        sqlconn.Open()
        SqlCommand.Connection = sqlconn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sql
        SqlCommand.ExecuteNonQuery()
        Remove_Appraisal = 1
      End If


    Catch ex As Exception
      Remove_Appraisal = 0
    Finally
      sqlconn.Dispose()
      sqlconn.Close()
      sqlconn = Nothing
      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function
  Public Function Insert_Into_Appraisal() As Integer
    Dim sqlconn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sql As String = ""

    Insert_Into_Appraisal = 0

    Try


      sql = " INSERT INTO Aircraft_Appraisal "
      sql &= " (acappr_source"
      sql &= " ,acappr_type"
      sql &= " ,acappr_entry_date"
      sql &= " ,acappr_date"
      sql &= "  ,acappr_subid"
      sql &= " ,acappr_login"
      sql &= ",acappr_seq_no"
      sql &= " ,acappr_comp_id"
      sql &= ",acappr_contact_id"
      sql &= ",acappr_amod_id"
      sql &= ",acappr_ac_id"
      sql &= ",acappr_airframe_tot_hrs"
      sql &= ",acappr_airframe_tot_landings"
      sql &= ",acappr_asking_price"
      sql &= ",acappr_take_price"
      sql &= ",acappr_est_value"
      sql &= ",acappr_notes"
      sql &= ",acappr_webaction_date)"
      sql &= " VALUES( "
      sql &= "  'Subscriber'"
      sql &= " ,'" & Me.type_of.SelectedValue & "'"
      sql &= " ,'" & DateTime.Now & "'"
      sql &= ",'" & Me.date_of_appraisal.Text & "'"
      sql &= "," & Session.Item("localPreferences").SubID & ""
      sql &= ",'" & Session.Item("localPreferences").UserID & "'"
      'Session.Item("localPreferences").Login As String: mattwanner-matt777
      'Session.Item("localPreferences").UserID As String: mattwanner
      sql &= "," & Session.Item("localPreferences").SeqNo & ""
      sql &= "," & Session.Item("localPreferences").UserCompanyID & ""
      sql &= "," & Session.Item("localPreferences").UserContactID & ""
      sql &= "," & Me.ModelID.Text & " "

      If Trim(Me.ac_id.Text) <> "" Then
        sql &= "," & Trim(Me.ac_id.Text) & ""
      Else
        sql &= ",0"
      End If

      If Me.aftt.Text <> "" Then
        If IsNumeric(Me.aftt.Text) = True Then
          sql &= "," & Me.aftt.Text & ""
        Else
          sql &= ",NULL"
        End If
      Else
        sql &= ",NULL"
      End If

      If Me.cycles.Text <> "" Then
        If IsNumeric(Me.cycles.Text) = True Then
          sql &= "," & Me.cycles.Text & ""
        Else
          sql &= ",NULL"
        End If
      Else
        sql &= ",NULL"
      End If

      If Me.asking_price.Text <> "" Then
        If IsNumeric(Me.asking_price.Text) = True Then
          sql &= "," & replace_numbers(Me.asking_price.Text) & ""
        Else
          sql &= ",NULL"
        End If
      Else
        sql &= ",NULL"
      End If

      If Me.take_price.Text <> "" Then
        If IsNumeric(Me.take_price.Text) = True Then
          sql &= "," & replace_numbers(Me.take_price.Text) & ""
        Else
          sql &= ",NULL"
        End If
      Else
        sql &= ",NULL"
      End If

      If Me.est_value.Text <> "" Then
        If IsNumeric(Me.est_value.Text) = True Then
          sql &= "," & replace_numbers(Me.est_value.Text) & ""
        Else
          sql &= ",NULL"
        End If
      Else
        sql &= ",NULL"
      End If


      sql &= ", '" & replace_notes(Me.notes_text.Text) & "'"

      sql &= ",'1900-01-01 00:00:00.000')"   ' acappr_webaction_date, datetime,

      sql = sql

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>DeletePhoneNumbers_contactID(ByVal contact_id As Integer, ByVal comp_id As Integer) As Integer</b><br />" & sql

      sqlconn.ConnectionString = Session.Item("jetnetClientDatabase")
      sqlconn.Open()
      SqlCommand.Connection = sqlconn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sql
      SqlCommand.ExecuteNonQuery()
      Insert_Into_Appraisal = 1

    Catch ex As Exception
      Insert_Into_Appraisal = 0
    Finally
      sqlconn.Dispose()
      sqlconn.Close()
      sqlconn = Nothing
      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function
  Public Function replace_numbers(ByVal temp_number As String) As String
    replace_numbers = ""

    temp_number = Replace(temp_number, ",", "")
    temp_number = Replace(temp_number, "$", "")

    replace_numbers = temp_number

  End Function
  Public Function replace_notes(ByVal temp_note As String) As String
    replace_notes = ""

    temp_note = Replace(temp_note, "'", "''") 
    temp_note = Left(Trim(temp_note), 1499)

    replace_notes = temp_note

  End Function
  Public Sub clicked_save(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

    Dim return_from As Integer = 0
    Dim passed_check As Boolean = False

    passed_check = call_check_form()
    If passed_check = True Then
      return_from = Insert_Into_Appraisal()

      If return_from = 0 Then
        Me.action_text.Text = "There was an error inserting your Appraisal"
        Me.action_text.Visible = True
      ElseIf return_from = 1 Then
        Me.action_text.Text = "Your Appraisal Has Been Saved"
        Me.action_text.Visible = True
      End If
    End If

  End Sub
  Public Sub clicked_update(ByVal sender As Object, ByVal e As System.EventArgs) Handles update_app.Click

    Dim return_from As Integer = 0
    Dim passed_check As Boolean = False

    passed_check = call_check_form()
    If passed_check = True Then
      return_from = Update_Appraisal(Trim(Request("id")))

      If return_from = 0 Then
        Me.action_text.Text = "There was an error Updating your Appraisal"
        Me.action_text.Visible = True
      ElseIf return_from = 1 Then
        Me.action_text.Text = "Your Appraisal Has Been Updated"
        Me.action_text.Visible = True
      End If
    End If

  End Sub
  Public Sub clicked_remove(ByVal sender As Object, ByVal e As System.EventArgs) Handles remove_app.Click

    Dim return_from As Integer = 0

    return_from = Remove_Appraisal(Trim(Request("id")))

    If return_from = 0 Then
      Me.action_text.Text = "There was an error Removing your Appraisal"
      Me.action_text.Visible = True
    ElseIf return_from = 1 Then
      Me.action_text.Text = "Your Appraisal Has Been Removed"
      Me.action_text.Visible = True
    End If

  End Sub

  Public Sub generateItemDetails(ByVal masterPage As EmptyEvoTheme)
    If AircraftID <> 0 Then
      container_tab.Visible = True
      aircraft_information.Text = CommonAircraftFunctions.DisplayAircraftDetailsBlock(aclsData_Temp, AircraftID, journalID, False, True, AircraftTable, JournalTable, Me.Session, Nothing, Nothing, features_tab, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True, 0, 0, False, Nothing, "")
      ModelID.Text = AircraftTable.Rows(0).Item("ac_amod_id")
      ac_id.text = AircraftTable.Rows(0).Item("ac_id")
    End If
    If YachtID <> 0 Then
      yacht_container_tab.Visible = True
      yacht_information.Text = crmWebClient.DisplayFunctions.BuildYachtInformationTab(YachtID, yacht_features_tab, masterPage.aclsData_Temp, YachtModelID, Nothing, Nothing, "", "", "", Nothing, "", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True) ', , masterPage, YachtID, 0, True, YachtModelID)
    End If

    If CompanyID <> 0 Then
      company_container_tab.Visible = True
      crmWebClient.CompanyFunctions.Fill_Information_Tab(company_features_tab, company_information, masterPage, CompanyID, journalID, "", New Label, New AjaxControlToolkit.TabContainer, New Label, New Label, True)
    End If
  End Sub

  Public Function call_check_form() As Boolean

    call_check_form = True

    Dim error_text As String = ""


    If IsDate(Me.date_of_appraisal.Text) = False Then
      call_check_form = False
      error_text = "Appraisal Date Must be a Date"
    End If 

    If Trim(Me.aftt.Text) <> "" Then
      If IsNumeric(Me.aftt.Text) = False Then
        call_check_form = False
        error_text = "AFTT must be Numeric"
      End If
    End If

    If Trim(Me.cycles.Text) <> "" Then
      If IsNumeric(Me.cycles.Text) = False Then
        call_check_form = False
        error_text = "Cycles must be Numeric"
      End If
    End If

    If Trim(Me.asking_price.Text) <> "" Then
      If IsNumeric(Me.asking_price.Text) = False Then
        call_check_form = False
        error_text = "Asking Price must be Numeric"
      End If
    End If

    If Trim(Me.take_price.Text) <> "" Then
      If IsNumeric(Me.take_price.Text) = False Then
        call_check_form = False
        error_text = "Take Price must be Numeric"
      End If
    End If

    If Trim(Me.est_value.Text) <> "" Then
      If IsNumeric(Me.est_value.Text) = False Then
        call_check_form = False
        error_text = "Estimated Value must be Numeric"
      End If
    End If

    If call_check_form = False Then
      Me.action_text.Text = "There was an error in your Appraisal, " & error_text & ", Appraisal NOT Updated"
      Me.action_text.Visible = True
    End If

  End Function

End Class