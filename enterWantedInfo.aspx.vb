
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/enterWantedInfo.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:38a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: enterWantedInfo.aspx.vb $
'
' ********************************************************************************

Partial Public Class enterWantedInfo
  Inherits System.Web.UI.Page

  Private nMaxWidth As Long = 0

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim sErrorString As String = ""
    Try
      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else
        Master.SetPageTitle("Submit Wanted Information") 'Page title that can be set to whatever is necessary.
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load preferences : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      commonEvo.fillMakeModelDropDown(wantedModelList, Nothing, nMaxWidth, "", wantedModelList.SelectedValue, False, True, False, False, False, False) ' fill dropdownlist with models

    Catch ex As Exception

    End Try

  End Sub

  Private Function GetNextWantedID() As Long

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As StringBuilder = New StringBuilder()

    Dim nNextWantedID As Long = 0

    sQuery.Append("SELECT MAX(amwant_id) AS nMaxWantedID FROM Aircraft_Model_Wanted WITH(NOLOCK)")

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("nMaxWantedID"))) Then
          If Not String.IsNullOrEmpty(lDataReader.Item("nMaxWantedID").ToString) Then
            nNextWantedID = CLng(lDataReader("nMaxWantedID").ToString) + 1000 ' add 1000 to max id
          End If
        End If

        lDataReader.Close()

      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

      Return nNextWantedID

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    sQuery = Nothing
    Return nNextWantedID

  End Function

  Private Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection

    Dim sQuery As StringBuilder = New StringBuilder()

    Try

      If Not IsNothing(wantedModelList.SelectedValue) Then

        sQuery.Append("INSERT INTO Aircraft_Model_Wanted (amwant_id, amwant_amod_id, amwant_journ_id, ")
        sQuery.Append(" amwant_listed_date, amwant_start_year, amwant_end_year, amwant_year_note,")
        sQuery.Append(" amwant_max_price, amwant_amount_note, amwant_max_aftt, amwant_accept_dam_cur, amwant_comp_id,")
        sQuery.Append(" amwant_contact_id, amwant_notes, amwant_entry_date, amwant_entry_user_id) VALUES (" + GetNextWantedID.ToString + ", ")

        If CLng(wantedModelList.SelectedValue) <> -1 Then
          sQuery.Append(wantedModelList.SelectedValue + ", ") ' amwant_amod_id
        End If

        sQuery.Append("0, '" + Now.ToShortDateString + "', ") ' amwant_journ_id, amwant_listed_date

        If Not IsNothing(Request.Item("txtYearRange1")) Then
          If Not String.IsNullOrEmpty(Request.Item("txtYearRange1").ToString.Trim) Then
            sQuery.Append("'" + Request.Item("txtYearRange1").ToString.Trim + "', ") ' amwant_start_year
          Else
            sQuery.Append("NULL, ")
          End If
        Else
          sQuery.Append("NULL, ")
        End If

        If Not IsNothing(Request.Item("txtYearRange2")) Then
          If Not String.IsNullOrEmpty(Request.Item("txtYearRange2").ToString.Trim) Then
            sQuery.Append("'" + Request.Item("txtYearRange2").ToString.Trim + "', ") ' amwant_end_year
          Else
            sQuery.Append("NULL, ")
          End If
        Else
          sQuery.Append("NULL, ")
        End If

        If Not String.IsNullOrEmpty(Request.Item("txtYearNote").ToString.Trim) Then
          sQuery.Append("'" + Request.Item("txtYearNote").ToString.Trim + "', ") ' amwant_year_note
        Else
          If Not String.IsNullOrEmpty(Request.Item("txtYearRange1").ToString.Trim) And Not String.IsNullOrEmpty(Request.Item("txtYearRange2").ToString.Trim) Then
            sQuery.Append("'Range', ") ' amwant_year_note
          Else
            sQuery.Append("NULL, ")
          End If

        End If

        If Not IsNothing(Request.Item("txtMaxPrice")) Then
          If Not String.IsNullOrEmpty(Request.Item("txtMaxPrice").ToString.Trim) Then
            sQuery.Append("'" + Replace(Replace(Request.Item("txtMaxPrice").ToString.Trim, "$", ""), ",", "") + "', ") ' amwant_max_price
          Else
            sQuery.Append("NULL, ")
          End If
        Else
          sQuery.Append("NULL, ")
        End If

        If Not String.IsNullOrEmpty(Request.Item("txtPriceNote").ToString.Trim) Then
          sQuery.Append("'" + Request.Item("txtPriceNote").ToString.Trim + "', ") ' amwant_amount_note
        Else
          If Not String.IsNullOrEmpty(Request.Item("txtMaxPrice").ToString.Trim) Then
            sQuery.Append("'Price', ") ' amwant_amount_note
          Else
            sQuery.Append("NULL, ")
          End If

        End If

        If Not IsNothing(Request.Item("txtMaxAFTT")) Then
          If Not String.IsNullOrEmpty(Request.Item("txtMaxAFTT").ToString.Trim) Then
            sQuery.Append("'" + Request.Item("txtMaxAFTT").ToString.Trim + "', ") ' amwant_max_aftt
          Else
            sQuery.Append("NULL, ")
          End If
        Else
          sQuery.Append("NULL, ")
        End If

        sQuery.Append("'" + wantedDamageList.SelectedValue + "', ") ' amwant_accept_dam_cur

        sQuery.Append(Session.Item("localUser").crmUserCompanyID.ToString + ", ") ' amwant_comp_id

        sQuery.Append(Session.Item("localUser").crmUserContactID.ToString + ", ") ' amwant_contact_id

        If Not IsNothing(Request.Item("txtNotes")) Then
          If Not String.IsNullOrEmpty(Request.Item("txtNotes").ToString.Trim) Then
            sQuery.Append("'" + Replace(Replace(Replace(Request.Item("txtNotes").ToString.Trim, "'", "''"), "", ""), "", "") + "', ") ' amwant_notes
          Else
            sQuery.Append("NULL, ")
          End If
        Else
          sQuery.Append("NULL, ")
        End If

        sQuery.Append("'" + Now().ToString + "', ") ' amwant_entry_date

        sQuery.Append("'CUST')") 'amwant_entry_user_id

        SqlConnection.ConnectionString = Session.Item("jetnetAdminDatabase").ToString.Trim
        SqlConnection.Open()

        SqlCommand.Connection = SqlConnection
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()
          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubmitWantedSuccess", "submitWantedSuccess();", True)
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />SubmitWanted() As Boolean<br />" + sQuery.ToString
        Catch SqlException
          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubmitWantedFailed", "submitWantedFailure();", True)
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in save_button_Click ExecuteNonQuery :" + SqlException.Message
        End Try

      Else
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubmitWantedFailed", "submitWantedFailure();", True)
      End If

    Catch ex As Exception
      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SubmitWantedFailed", "submitWantedFailure();", True)
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in save_button_Click " + ex.Message

    Finally

      SqlConnection.Dispose()
      SqlConnection.Close()
      SqlConnection = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try


  End Sub

End Class