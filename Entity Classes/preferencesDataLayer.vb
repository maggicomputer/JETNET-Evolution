Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/preferencesDataLayer.vb $
'$$Author: Mike $
'$$Date: 4/08/20 11:58a $
'$$Modtime: 4/08/20 11:55a $
'$$Revision: 6 $
'$$Workfile: preferencesDataLayer.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class preferencesDataLayer
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

#Region "password_functions"

  Public Function VerifyPassword(ByVal userID As String, ByVal sub_id As Long, ByVal oldPassword As String) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT sublogin_password FROM Subscription_Login WITH(NOLOCK)")
      sQuery.Append(" WHERE (sublogin_sub_id = " + sub_id.ToString + " AND sublogin_login = '" + userID.Trim + "'")
      sQuery.Append(" AND lower(sublogin_password) = '" + oldPassword.ToLower.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />VerifyPassword(ByVal userID As String, ByVal sub_id As Long, ByVal oldPassword As String) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in VerifyPassword load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "ERROR in " + Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim

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

  Public Function UpdatePassword(ByVal userID As String, ByVal sub_id As Long, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Login SET sublogin_web_action_date = NULL, sublogin_password = '" + newPassword.Trim + "'")
      sQuery.Append(" WHERE (sublogin_sub_id = " + sub_id.ToString)
      sQuery.Append(" AND sublogin_login = '" + userID + "'")
      sQuery.Append(" AND lower(sublogin_password) = '" + oldPassword.ToLower.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdatePassword(ByVal userID As String, ByVal sub_id As Long, ByVal oldPassword As String, ByVal newPassword As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdatePassword ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "ERROR in " + Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Password", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

#End Region

#Region "update_default_values_functions"

  Public Function UpdateDefaultModel(ByVal defaultModelID As Long, ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If defaultModelID > -1 Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_default_amod_id = " + defaultModelID.ToString)
        sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
        sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
        sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
        sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_default_amod_id = NULL")
        sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
        sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
        sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
        sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateDefaultModel(ByVal defaultModelID As Long, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateDefaultModel ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateDefaultModel(ByVal defaultModelID As Long, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Default Model GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateDefaultBackground(ByVal defaultBackgroundID As Long, ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_background_image_id = " + defaultBackgroundID.ToString)
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateDefaultBackground(ByVal defaultBackgroundID As Long, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateDefaultBackground ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateDefaultBackground(ByVal defaultBackgroundID As Long ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Default Background GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateDefaultRelationship(ByVal defaultRelationships As String, ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If bEnable Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_aircraft_tab_relationship_to_ac_default = '" + defaultRelationships + "'")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_aircraft_tab_relationship_to_ac_default = NULL")
      End If

      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateDefaultRelationship(ByVal defaultRelationships As String, ByVal userGUID As String)</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateDefaultRelationship ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateDefaultRelationship(ByVal defaultRelationships As String, ByVal userGUID As String) " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Default Relationships GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function ResetDefaultView(ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_evoview_id = 0")
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />ResetDefaultView(ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in ResetDefaultView ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in ResetDefaultView(ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Reset Default View GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateDefaultModelMarket(ByRef MyListBoxControl As ListBox, ByVal userGUID As String, ByRef newModelString As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      For i As Integer = 0 To MyListBoxControl.Items.Count - 1
        If Not String.IsNullOrEmpty(MyListBoxControl.Items(i).Value) Then
          If String.IsNullOrEmpty(newModelString.Trim) Then
            newModelString = MyListBoxControl.Items(i).Value
          Else
            newModelString += Constants.cCommaDelim + MyListBoxControl.Items(i).Value
          End If
        End If
      Next

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_default_models = '" + newModelString.Trim + "'")
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateDefaultModelMarket(ByRef MyListBoxControl As ListBox, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateDefaultModelMarket ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateDefaultModelMarket(ByRef MyListBoxControl As ListBox, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Default Models GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

#End Region

#Region "subscriber_image_functions"

  Public Function ReturnUserDetailsAndImage(ByVal contactID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Contact WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN contact_pictures WITH(NOLOCK) ON contact_id = conpic_contact_id AND conpic_hide_flag='N'")
      sQuery.Append(" WHERE contact_id = " + contactID.ToString + " AND contact_journ_id = 0 AND contact_active_flag = 'Y'")
      sQuery.Append(" ORDER BY contact_acpros_seq_no, contact_last_name, contact_first_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>ReturnUserDetailsAndImage(ByVal contactID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in ReturnUserDetailsAndImage(ByVal contactID As Long) As DataTable load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in ReturnUserDetailsAndImage(ByVal contactID As Long) As DataTable" + ex.Message

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

  Public Function InsertUserImage(ByVal contactID As Long) As Boolean

    Dim bReturnValue As Boolean = False

    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("INSERT INTO contact_pictures (conpic_contact_id, conpic_action_date, conpic_hide_flag, conpic_image_type)")
      sQuery.Append(" VALUES (" + contactID.ToString + ",'" + FormatDateTime(Now(), vbGeneralDate) + "', 'N', 'jpg')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>InsertUserImage(ByVal contactID As Long) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bReturnValue = True
      Catch SqlException
        aError = "Error in InsertUserImage(ByVal contactID As Long) ExecuteNonQuery :" + SqlException.Message
      End Try

    Catch ex As Exception
      aError = "Error in InsertUserImage(ByVal contactID As Long) As Boolean" + ex.Message
    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function RemoveUserImage(ByVal contactID As Long, ByVal conpicID As Long) As Boolean

    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim bReturnValue As Boolean = False

    Try

      If contactID > 0 And conpicID > 0 Then

        sQuery.Append("DELETE FROM contact_pictures WHERE conpic_contact_id = " + contactID.ToString + " AND conpic_id = " + conpicID.ToString)

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>RemoveUserImage(ByVal contactID As Long, ByVal conpicID As Long) As Boolean</b><br />" + sQuery.ToString

        SqlConn.ConnectionString = adminConnectString
        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        Try
          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()
          bReturnValue = True
        Catch SqlException
          aError = "Error in RemoveUserImage(ByVal contactID As Long, ByVal conpicID As Long) As Integer ExecuteNonQuery :" + SqlException.Message
        End Try

      End If

    Catch ex As Exception
      aError = "Error in RemoveUserImage(ByVal contactID As Long, ByVal conpicID As Long) As Boolean" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function CheckForExistingUserImageRow(ByVal contactID As Long) As Long
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim contactPicID As Long = 0

    Try

      sQuery.Append("SELECT TOP 1 * FROM contact_pictures WITH(NOLOCK) ")
      sQuery.Append("WHERE conpic_contact_id = " + contactID.ToString)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>CheckForExistingUserImageRow(ByVal contactID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try

        atemptable.Load(SqlReader)

        If atemptable.Rows.Count > 0 Then
          contactPicID = CLng(atemptable.Rows(0).Item("conpic_id").ToString)
        End If

      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in CheckForExistingUserImageRow(ByVal contactID As Long) As DataTable load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in CheckForExistingUserImageRow(ByVal contactID As Long) As DataTable" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return contactPicID

  End Function

#End Region

#Region "fill_tab_tables_functions"

  Public Sub fillFoldersDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelFolder As String, ByVal inSelFolderType As String)

    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sFolderTabName As String = "" 'FolderID

    Dim results_table As New DataTable

    Dim bIsAdmin As Boolean = False

    MyDropDownControl.Items.Clear()

    Try

      MyDropDownControl.Items.Add(New ListItem("All", ""))

      If inSelFolderType.ToLower.Contains("usf") Then
        bIsAdmin = True
      End If

      results_table = commonEvo.returnUserFolders(True, bIsAdmin)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not (IsDBNull(r("cfttpe_name"))) Then
              sFolderTabName = r.Item("cfttpe_name").ToString
            End If

            If (sFolderTabName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
              maxWidth = (sFolderTabName.Length * Constants._STARTCHARWIDTH)
            End If

            MyDropDownControl.Items.Add(New ListItem(sFolderTabName, sFolderTabName))

          Next

        End If

      End If

      If Not String.IsNullOrEmpty(inSelFolder.Trim) And Not inSelFolder.ToLower.Contains("all") Then
        MyDropDownControl.SelectedValue = inSelFolder.Trim
      Else
        MyDropDownControl.SelectedIndex = 0
      End If

      If maxWidth = 0 Then
        maxWidth = (("  All  ").Length * Constants._STARTCHARWIDTH)
      End If

      MyDropDownControl.Width = (maxWidth)

    Catch ex As Exception

      aError = "Error in fillFoldersDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelFolder As String) " + ex.Message

    Finally


    End Try

  End Sub

  Public Sub fillTemplatesDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelTemplate As String, ByVal inSelTemplateType As String)

    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sTemplateName As String = ""
    Dim bIsAdmin As Boolean = False

    Dim results_table As New DataTable

    MyDropDownControl.Items.Clear()

    Try

      MyDropDownControl.Items.Add(New ListItem("All", ""))

      If inSelTemplateType.ToLower.Contains("ust") Then
        bIsAdmin = True
      End If

      results_table = commonEvo.returnUserTemplates(True, bIsAdmin)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not (IsDBNull(r("sise_tab"))) Then
              sTemplateName = r.Item("sise_tab").ToString
            End If

            If (sTemplateName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
              maxWidth = (sTemplateName.Length * Constants._STARTCHARWIDTH)
            End If

            MyDropDownControl.Items.Add(New ListItem(sTemplateName, sTemplateName))

          Next

        End If

      End If

      If Not String.IsNullOrEmpty(inSelTemplate.Trim) And Not inSelTemplate.ToLower.Contains("all") Then
        MyDropDownControl.SelectedValue = inSelTemplate.Trim
      Else
        MyDropDownControl.SelectedIndex = 0
      End If

      If maxWidth = 0 Then
        maxWidth = (("  All  ").Length * Constants._STARTCHARWIDTH)
      End If

      MyDropDownControl.Width = (maxWidth)

    Catch ex As Exception

      aError = "fillTemplatesDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelFolder As String) " + ex.Message

    Finally


    End Try

  End Sub

  Public Sub fillDefaultAirportsDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelAirport As String)

    Dim sQuery As StringBuilder = New StringBuilder()

    Dim sAirportFolderName As String = ""
    Dim nAirportFolderID As Long = 0

    Dim bIsAdmin As Boolean = False
    Dim bIsDefault As Boolean = False

    Dim results_table As New DataTable

    MyDropDownControl.Items.Clear()

    Try

      MyDropDownControl.Items.Add(New ListItem("No Default", "0"))

      results_table = commonEvo.returnAirportFolderName(False, False, 17)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not (IsDBNull(r.Item("cfolder_default_flag"))) Then
              bIsDefault = IIf(r.Item("cfolder_default_flag").ToString = "Y", True, False)
            End If

            If Not (IsDBNull(r.Item("cfolder_name"))) Then
              sAirportFolderName = r.Item("cfolder_name").ToString
            End If

            If Not (IsDBNull(r.Item("cfolder_id"))) Then
              If IsNumeric(r.Item("cfolder_id").ToString) Then
                nAirportFolderID = CLng(r.Item("cfolder_id").ToString)
              End If
            End If

            If (sAirportFolderName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
              maxWidth = (sAirportFolderName.Length * Constants._STARTCHARWIDTH)
            End If

            MyDropDownControl.Items.Add(New ListItem(sAirportFolderName, nAirportFolderID.ToString))

            If bIsDefault Then
              HttpContext.Current.Session.Item("currentDefaultAirportFolderID") = nAirportFolderID.ToString
            End If

            bIsDefault = False

          Next

        End If

      End If

      If Not String.IsNullOrEmpty(inSelAirport.Trim) Then
        If CLng(inSelAirport.Trim) = 0 Then
          MyDropDownControl.SelectedValue = "0"
        Else
          MyDropDownControl.SelectedValue = inSelAirport.Trim
        End If
      Else
        MyDropDownControl.SelectedValue = HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString
      End If

      If maxWidth = 0 Then
        maxWidth = (("  No Default  ").Length * Constants._STARTCHARWIDTH)
      End If

      MyDropDownControl.Width = (maxWidth)

    Catch ex As Exception

      aError = "fillDefaultAirportsDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inSelAirport As String) " + ex.Message

    Finally


    End Try

  End Sub

#End Region

#Region "fill_dropdown_functions"

  Public Sub fillSMSEventsDropDown(ByRef MyListBoxControl As ListBox, ByRef maxWidth As Long, ByRef htmlOutput As String, ByVal smsEvents As String, ByVal bIsSubscriber As Boolean)

    Dim tmpEventArr() As String = Nothing
    Dim bFoundValue As Boolean = False

    If Not bIsSubscriber Then
      MyListBoxControl.Items.Clear()
    End If

    tmpEventArr = smsEvents.Split(Constants.cCommaDelim)

    If commonEvo.inMyArray(tmpEventArr, "MA") Then
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Newly Available", "MA"))
      Else
        htmlOutput &= "<em>Newly Available</em><br />" & vbCrLf
      End If
    Else
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Newly Available", "MA"))
      End If
    End If

    If commonEvo.inMyArray(tmpEventArr, "CA") Then
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Change in Asking Price", "CA"))
      Else
        htmlOutput &= "<em>Change in Asking Price</em><br />" & vbCrLf
      End If
    Else
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Change in Asking Price", "CA"))
      End If
    End If

    If commonEvo.inMyArray(tmpEventArr, "OM") Then
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Off Market", "OM"))
      Else
        htmlOutput &= "<em>Off Market</em><br />" & vbCrLf
      End If
    Else
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Off Market", "OM"))
      End If
    End If

    If commonEvo.inMyArray(tmpEventArr, "OMNS") Then
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Off Market Due To Sale", "OMNS"))
      Else
        htmlOutput &= "<em>Off Market Due To Sale</em><br />" & vbCrLf
      End If
    Else
      If Not bIsSubscriber Then
        MyListBoxControl.Items.Add(New ListItem("Off Market Due To Sale", "OMNS"))
      End If
    End If

    If Not bIsSubscriber Then

      If Not String.IsNullOrEmpty(smsEvents) Then

        ' set selected values
        For i As Integer = 0 To MyListBoxControl.Items.Count - 1

          If commonEvo.inMyArray(tmpEventArr, MyListBoxControl.Items(i).Value.ToUpper) Then
            MyListBoxControl.Items(i).Selected = True
          End If

        Next

      End If

      maxWidth = (CStr("Off Market Due To Sale").Length * crmWebClient.Constants._STARTCHARWIDTH)
      MyListBoxControl.Width = (maxWidth)

    End If

  End Sub

  Public Sub fillSMSProviderDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inCarrierID As String)

    Dim sQuery As StringBuilder = New StringBuilder()

    Dim fSmstxtcar_carrier As String = ""
    Dim fSmstxtcar_country As String = ""
    Dim fSmstxtcar_id As String = ""
    Dim sCarrierName As String = ""

    Dim results_table As New DataTable
    Dim combined_table As New DataTable

    MyDropDownControl.Items.Clear()

    Try

      MyDropDownControl.Items.Add(New ListItem("Please Select One", ""))

      results_table = commonEvo.Get_SMS_Providers()

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          'This is what we have to fill the SMS dropdown up with. Interestingly enough, we need to create a filter though, so we
          'only have to poll the database once for this information.
          'First we need the united States carriers - ordered by carrier

          combined_table = results_table.Clone 'gets a blank copy of the results schema
          Dim afileterd As DataRow() = results_table.Select("smstxtcar_country LIKE 'United States'", "smstxtcar_carrier")

          For Each atmpDataRow As DataRow In afileterd
            combined_table.ImportRow(atmpDataRow)
          Next

          'Next we need the foreign carriers, ordered by country, carrier
          afileterd = results_table.Select("smstxtcar_country NOT LIKE 'United States'", "smstxtcar_country, smstxtcar_carrier")
          For Each atmpDataRow As DataRow In afileterd
            combined_table.ImportRow(atmpDataRow)
          Next

          If combined_table.Rows.Count > 0 Then

            For Each r As DataRow In combined_table.Rows

              If Not (IsDBNull(r("smstxtcar_carrier"))) Then
                fSmstxtcar_carrier = r.Item("smstxtcar_carrier").ToString
              End If

              If Not (IsDBNull(r("smstxtcar_country"))) Then
                fSmstxtcar_country = r.Item("smstxtcar_country").ToString
              End If

              If Not (IsDBNull(r("smstxtcar_id"))) Then
                fSmstxtcar_id = r.Item("smstxtcar_id").ToString
              End If

              sCarrierName = fSmstxtcar_country.Trim + " - " + fSmstxtcar_carrier.Trim

              If (sCarrierName.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (sCarrierName.Length * crmWebClient.Constants._STARTCHARWIDTH)
              End If

              MyDropDownControl.Items.Add(New ListItem(sCarrierName, fSmstxtcar_id))

            Next

          End If

        End If

      End If

      MyDropDownControl.SelectedValue = inCarrierID.ToString.Trim
      MyDropDownControl.Width = (maxWidth)

    Catch ex As Exception

      aError = "Error in fillSMSProviderDropDown(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inCarrierID As String) " + ex.Message

    Finally


    End Try

  End Sub

#End Region

#Region "display_tab_tables_functions"

  Public Sub display_folder_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim compString As String = ""
    Dim arrCompanyID() As String = Nothing
    Dim nArrCount As Integer = 0
    Dim folderType As Integer = 0
    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""folderDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th data-priority=""1"">AREA</th>")
          htmlOut.Append("<th>TITLE</th>")
          htmlOut.Append("<th>USER</th>")

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

            htmlOut.Append("<tr>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            htmlOut.Append(Replace(r.Item("cfttpe_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            htmlOut.Append("<img src=""" + DisplayFunctions.ReturnFolderImage(r.Item("cfolder_method").ToString.ToUpper, "", r.Item("cfolder_share").ToString.ToUpper) + """ alt="""" title="""" />&nbsp;")

            Select Case r.Item("cfttpe_name").ToString.ToUpper.Trim
              Case "HISTORY"
                folderType = 8
              Case "EVENTS"
                folderType = 5
              Case "COMPANY"
                folderType = 1
              Case "WANTED"
                folderType = 9
              Case "YACHT"
                folderType = 10
              Case "YACHT HISTORY"
                folderType = 14
              Case "YACHT EVENTS"
                folderType = 15
              Case "PERFORMANCE SPECS"
                folderType = 12
              Case "OPERATING COSTS"
                folderType = 11
              Case "MARKET SUMMARIES"
                folderType = 13
              Case "VALUE"
                folderType = 16
              Case "AIRPORT"
                folderType = 17
              Case Else
                folderType = 3
            End Select

            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""FolderMaintenance.aspx?fromPreferences=true&type=MY&REPORT_ID=" + r.Item("cfolder_id").ToString + "&TYPE_OF_FOLDER=" + r.Item("cfttpe_name").ToString + "&t=" + folderType.ToString + """,""EditFolder"");' title=""Edit Folder"">")

            If Not (IsDBNull(r("cfolder_name"))) And Not String.IsNullOrEmpty(r.Item("cfolder_name").ToString.Trim) Then
              htmlOut.Append(r.Item("cfolder_name").ToString.Trim)
            Else
              htmlOut.Append(" blank name ")
            End If
            htmlOut.Append("</a></td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
            If Not (IsDBNull(r("contact_first_name"))) And Not String.IsNullOrEmpty(r.Item("contact_first_name").ToString.Trim) Then
              htmlOut.Append(r.Item("contact_first_name").ToString.Trim)
            End If

            If Not (IsDBNull(r("contact_last_name"))) And Not String.IsNullOrEmpty(r.Item("contact_last_name").ToString.Trim) Then
              htmlOut.Append(Constants.cSingleSpace + r.Item("contact_last_name").ToString.Trim)
            End If
            htmlOut.Append("</td>")

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""folderLabel"" class="""" style=""padding:2px;""><strong>" + searchTable.Rows.Count.ToString + " Folder(s)</strong></div>")
          htmlOut.Append("<div id=""folderInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_company_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Public Sub display_template_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False


    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""templateDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th data-priority=""1"">AREA</th>")
          htmlOut.Append("<th>TITLE</th>")
          htmlOut.Append("<th>USER</th>")
          htmlOut.Append("<th>USAGE</th>")

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

            htmlOut.Append("<tr>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            htmlOut.Append(Replace(r.Item("sise_tab").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""evo_exporter.aspx?fromPreferences=true&export_type=" + r.Item("sise_tab").ToString.Trim + "&id=" + r.Item("sise_id").ToString + "&type=MY|" + r.Item("sise_id").ToString + """,""EditTemplate"");' title=""Edit Template"">")

            If Not (IsDBNull(r("sise_subject"))) And Not String.IsNullOrEmpty(r.Item("sise_subject").ToString.Trim) Then
              htmlOut.Append(r.Item("sise_subject").ToString.Trim)
            Else
              htmlOut.Append(" blank name ")
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            If Not (IsDBNull(r("contact_first_name"))) And Not String.IsNullOrEmpty(r.Item("contact_first_name").ToString.Trim) Then
              htmlOut.Append(r.Item("contact_first_name").ToString.Trim)
            End If

            If Not (IsDBNull(r("contact_last_name"))) And Not String.IsNullOrEmpty(r.Item("contact_last_name").ToString.Trim) Then
              htmlOut.Append(Constants.cSingleSpace + r.Item("contact_last_name").ToString.Trim)
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

            If Not (IsDBNull(r("sise_share_flag"))) And Not String.IsNullOrEmpty(r.Item("sise_share_flag").ToString.Trim) Then
              htmlOut.Append(IIf(r.Item("sise_share_flag").ToString.ToUpper.Trim.Contains("N"), "<img src=""images/regular_folder.png"" alt=""Personal"" title=""Personal"" />", "<img src=""images/shared_folder.png"" alt=""Shared"" title=""Shared"" />"))
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""templateLabel"" class="""" style=""padding:2px;""><strong>" + searchTable.Rows.Count.ToString + " Template(s)</strong></div>")
          htmlOut.Append("<div id=""templateInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_template_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Public Sub display_airport_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim airportString As String = ""
    Dim arrAirportID() As String = Nothing
    Dim nArrCount As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""airportDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th data-priority=""1"">NAME</th>")
          htmlOut.Append("<th>CITY</th>")
          htmlOut.Append("<th>STATE</th>")
          htmlOut.Append("<th data-priority=""2"">COUNTRY</th>")
          htmlOut.Append("<th>IATA</th>")
          htmlOut.Append("<th>ICAO</th>")

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

            airportString = r.Item("aport_id").ToString

            If Not commonEvo.inMyArray(arrAirportID, airportString) Then

              If Not IsArray(arrAirportID) And IsNothing(arrAirportID) Then
                ReDim arrAirportID(nArrCount)
              Else
                ReDim Preserve arrAirportID(nArrCount)
              End If

              ' Add CompId To Array
              arrAirportID(nArrCount) = airportString
              nArrCount += 1

              htmlOut.Append("<tr>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              If Not (IsDBNull(r("aport_city"))) And Not String.IsNullOrEmpty(r.Item("aport_city").ToString.Trim) Then
                htmlOut.Append(r.Item("aport_city").ToString.Trim)
              End If
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              If Not (IsDBNull(r("aport_state"))) And Not String.IsNullOrEmpty(r.Item("aport_state").ToString.Trim) Then
                htmlOut.Append(r.Item("aport_state").ToString.Trim)
              End If
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              If Not (IsDBNull(r("aport_country"))) And Not String.IsNullOrEmpty(r.Item("aport_country").ToString.Trim) Then
                htmlOut.Append(r.Item("aport_country").ToString.Trim)
              End If
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              If Not (IsDBNull(r("aport_iata_code"))) And Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString.Trim) Then
                htmlOut.Append(r.Item("aport_iata_code").ToString.Trim)
              End If
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
              If Not (IsDBNull(r("aport_icao_code"))) And Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString.Trim) Then
                htmlOut.Append(r.Item("aport_icao_code").ToString.Trim)
              End If

              htmlOut.Append("</td>")

              htmlOut.Append("</tr>")

            End If

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""airportLabel"" class="""" style=""padding:2px;""><strong>" + arrAirportID.Length.ToString + " Airport(s)</strong></div>")
          htmlOut.Append("<div id=""airportInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If


      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_airport_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

#End Region

#Region "update_preferences_functions"

  Public Function UpdateRecordsPerPage(ByVal recordsPerPageID As Long, ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_nbr_rec_per_page = " + recordsPerPageID.ToString)
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateRecordsPerPage(ByVal recordsPerPageID As Long, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateRecordsPerPage ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateRecordsPerPage(ByVal recordsPerPageID As Long, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Records Per Page GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateBusinessSegment(ByVal businessSegment As String, ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_business_type_code = '" + businessSegment.Trim + "'")
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateBusinessSegment(ByVal businessSegment As String, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateBusinessSegment ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateBusinessSegment(ByVal businessSegment As String, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Business Segment GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateAnalysisTimeframe(ByVal monthsValue As Long, ByVal userGUID As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_default_analysis_months = " + monthsValue.ToString)
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />UpdateAnalysisTimeframe(ByVal monthsValue As Long, ByVal userGUID As String) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateAnalysisTimeframe ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateAnalysisTimeframe(ByVal monthsValue As Long, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Analysis Timeframe GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateShowBlankFields(ByVal displayBlankAcVal As String, ByVal userGUID As String) As Boolean

    Dim bResult As Boolean = False

    Try

      If displayBlankAcVal.ToUpper.Contains("EF") Then
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Value = "Y"
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Expires = DateTime.Now.AddDays(300)
      Else
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Value = "N"
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Expires = DateTime.Now.AddDays(300)
      End If

      bResult = True

    Catch ex As Exception

      aError = "Error in UpdateShowBlankFields(ByVal displayBlankAcVal As String, ByVal userGUID As String) As Boolean " + ex.Message

    Finally
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Show Blank AC Fields GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateNotesIndicatorOnListing(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If bEnable Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_display_note_tag_on_aclist_flag = 'Y'")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_display_note_tag_on_aclist_flag = 'N'")
      End If
      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateNotesIndicatorOnListing(ByVal userGUID As String, ByRef bDisable As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateNotesIndicatorOnListing ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateNotesIndicatorOnListing(ByVal userGUID As String, ByRef bDisable As Boolean) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Notes Indicator On AC Listings GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateSubscriptionCloudNotes(ByVal bEnable As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlEVOConn As New SqlClient.SqlConnection
    Dim SqlCloudConn As New SqlClient.SqlConnection
    Dim SqlEVOCommand As New SqlClient.SqlCommand
    Dim SqlCloudCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False
    Dim rowsAffected As Integer

    Try

      SqlCloudConn.ConnectionString = cloudConnectStr
      SqlCloudConn.Open()

      SqlEVOConn.ConnectionString = adminConnectStr
      SqlEVOConn.Open()

      If bEnable Then

        ' run stored procedure to create cloud notes database
        SqlCloudCommand.Connection = SqlCloudConn
        SqlCloudCommand.CommandTimeout = 80
        SqlCloudCommand.CommandText = "Create_Company_Cloud_Notes_Table"
        SqlCloudCommand.CommandType = CommandType.StoredProcedure
        SqlCloudCommand.Parameters.AddWithValue("CNCompID", HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString)

        Try
          rowsAffected = SqlCloudCommand.ExecuteNonQuery()
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "RUN Stored Procedure Create_Company_Cloud_Notes_Table :" + rowsAffected.ToString
        Catch SqlException
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in RUN Create_Company_Cloud_Notes_Table Stored Procedure :" + SqlException.Message
        End Try

        sQuery.Append("UPDATE Subscription SET sub_web_action_date = NULL, sub_cloud_notes_flag = 'Y', sub_cloud_notes_database = 'cloud_notes_" + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + "'")
      Else
        sQuery.Append("UPDATE Subscription SET sub_web_action_date = NULL, sub_cloud_notes_flag = 'N', sub_cloud_notes_database = NULL")
      End If

      sQuery.Append(" WHERE (sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + " AND sub_comp_id = " + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + ")")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b> UpdateSubscriptionCloudNotes(ByRef bEnable As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlEVOCommand.Connection = SqlEVOConn
      SqlEVOCommand.CommandTimeout = 60
      SqlEVOCommand.CommandType = CommandType.Text

      Try
        SqlEVOCommand.CommandText = sQuery.ToString
        SqlEVOCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateSubscriptionCloudNotes ExecuteNonQuery :" + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateSubscriptionCloudNotes(ByVal bEnable As Boolean) " + ex.Message

    Finally

      SqlEVOConn.Dispose()
      SqlEVOConn.Close()
      SqlEVOConn = Nothing

      SqlEVOCommand.Dispose()
      SqlEVOCommand = Nothing


      SqlCloudConn.Dispose()
      SqlCloudConn.Close()
      SqlCloudConn = Nothing

      SqlCloudCommand.Dispose()
      SqlCloudCommand = Nothing

    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Subscription Cloud Notes", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateMobileFlag(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If bEnable Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL,")
        sQuery.Append(" subins_mobile_active_date = '" + FormatDateTime(DateAdd("d", 7, Now()), DateFormat.GeneralDate).ToString + "',")
        sQuery.Append(" subins_evo_mobile_flag = 'Y'")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_mobile_active_date = NULL,")
        sQuery.Append(" subins_evo_mobile_flag = 'N'")
      End If

      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateMobileFlag(ByVal userGUID As String, ByVal sSaveNumber As String, ByVal bDisable As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateMobileFlag ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateMobileFlag(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Mobile Flag GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateEmailRequest(ByVal userGUID As String, ByVal sEmailName As String, ByVal sEmailAddress As String, ByVal bHtmlFormat As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      If bHtmlFormat Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_email_replyname = '" + sEmailName.Trim + "',")
        sQuery.Append(" subins_email_replyaddress = '" + sEmailAddress.Trim + "', ")
        sQuery.Append(" subins_email_default_format = 'HTML'")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_email_replyname = '" + sEmailName.Trim + "',")
        sQuery.Append(" subins_email_replyaddress = '" + sEmailAddress.Trim + "', ")
        sQuery.Append(" subins_email_default_format = 'TEXT'")
      End If

      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateEmailRequest(ByVal userGUID As String, ByVal sEmailName As String, ByVal sEmailAddress As String, ByVal bHtmlFormat As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateEmailRequest ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateEmailRequest(ByVal userGUID As String, ByVal sEmailName As String, ByVal sEmailAddress As String, ByVal bHtmlFormat As Boolean) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Email Request Info GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateSMSActivation(ByVal userGUID As String, _
                  ByVal sSaveNumber As String, _
                  ByVal nCarrierID As Integer, _
                  ByVal sCarrierName As String, _
                  ByVal sSMSEvents As String, _
                  ByVal sSMSModels As String, _
                  ByVal bIsPhoneUnique As Boolean, _
                  ByVal sSMSActivationStatus As String) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      Select Case sSMSActivationStatus

        Case Constants.SMS_ACTIVATE_NO

          sQuery.Append("UPDATE Subscription_Install SET subins_smstxt_active_flag = 'A',")

          If Not String.IsNullOrEmpty(sSaveNumber) And Not bIsPhoneUnique Then
            sQuery.Append(" subins_cell_number = '" + sSaveNumber + "',")
          ElseIf String.IsNullOrEmpty(sSaveNumber) Then
            sQuery.Append(" subins_cell_number = NULL,")
          End If

          If nCarrierID > 0 Then
            sQuery.Append(" subins_cell_carrier_id = " + nCarrierID.ToString + ",")
          Else
            sQuery.Append(" subins_cell_carrier_id = NULL,")
          End If

          If Not String.IsNullOrEmpty(sCarrierName) Then
            sQuery.Append(" subins_cell_service = '" + sCarrierName.Trim + "',")
          Else
            sQuery.Append(" subins_cell_service = NULL,")
          End If

          If Not String.IsNullOrEmpty(sSMSModels) Then
            sQuery.Append(" subins_smstxt_models = '" + sSMSModels + "',")
          Else
            sQuery.Append(" subins_smstxt_models = NULL,")
          End If

          If Not String.IsNullOrEmpty(sSMSEvents) Then
            sQuery.Append(" subins_sms_events = '" + sSMSEvents + "',")
          Else
            sQuery.Append(" subins_sms_events = NULL,")
          End If

          sQuery.Append("subins_web_action_date = NULL")

          sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
          sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
          sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

        Case Constants.SMS_ACTIVATE_YES

          sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_smstxt_active_flag = 'N'")
          sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
          sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
          sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

        Case Else

          sQuery.Append("UPDATE Subscription_Install SET")

          If nCarrierID > 0 Then
            sQuery.Append(" subins_cell_carrier_id = " + nCarrierID.ToString + ",")
          Else
            sQuery.Append(" subins_cell_carrier_id = NULL,")
          End If

          If Not String.IsNullOrEmpty(sCarrierName) Then
            sQuery.Append(" subins_cell_service = '" + sCarrierName.Trim + "',")
          Else
            sQuery.Append(" subins_cell_service = NULL,")
          End If

          If Not String.IsNullOrEmpty(sSMSModels) Then
            sQuery.Append(" subins_smstxt_models = '" + sSMSModels + "',")
          Else
            sQuery.Append(" subins_smstxt_models = NULL,")
          End If

          If Not String.IsNullOrEmpty(sSMSEvents) Then
            sQuery.Append(" subins_sms_events = '" + sSMSEvents + "',")
          Else
            sQuery.Append(" subins_sms_events = NULL,")
          End If

          sQuery.Append("subins_web_action_date = NULL")

          sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
          sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
          sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      End Select

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateSMSActivation(ByVal userGUID As String, ByVal sSaveNumber As String, ByVal sMobileNumber As String,  ByVal nCarrierID As Integer, ByVal sCarrierName As String, ByVal sSMSEvents As String, ByVal sSMSModels As String, ByVal bDisable As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        aError = "Error in UpdateSMSActivation ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in UpdateSMSActivation(ByVal userGUID As String, ByVal sSaveNumber As String, ByVal sMobileNumber As String,  ByVal nCarrierID As Integer, ByVal sCarrierName As String, ByVal sSMSEvents As String, ByVal sSMSModels As String, ByVal bDisable As Boolean) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated SMS Activation  Status GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateSubscriptionABIFlag(ByVal bEnable As Boolean) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlEVOConn As New SqlClient.SqlConnection
    Dim SqlCloudConn As New SqlClient.SqlConnection
    Dim SqlEVOCommand As New SqlClient.SqlCommand
    Dim SqlCloudCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False
    '  Dim rowsAffected As Integer

    Try
      SqlEVOConn.ConnectionString = adminConnectStr
      SqlEVOConn.Open()

      If bEnable Then
        sQuery.Append("UPDATE Subscription SET sub_web_action_date = NULL, sub_abi_flag = 'Y'  ")
      Else
        sQuery.Append("UPDATE Subscription SET sub_web_action_date = NULL, sub_abi_flag = 'N' ")
      End If

      sQuery.Append(" WHERE (sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + " AND sub_comp_id = " + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + ")")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b> UpdateSubscriptionCloudNotes(ByRef bEnable As Boolean) As Boolean</b><br />" + sQuery.ToString

      SqlEVOCommand.Connection = SqlEVOConn
      SqlEVOCommand.CommandTimeout = 60
      SqlEVOCommand.CommandType = CommandType.Text

      Try
        SqlEVOCommand.CommandText = sQuery.ToString
        SqlEVOCommand.ExecuteNonQuery()
        bResult = True
      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateSubscriptionCloudNotes ExecuteNonQuery :" + SqlException.Message
      End Try


      If bEnable Then
        HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = True
      Else
        HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = False
      End If


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateSubscriptionCloudNotes(ByVal bEnable As Boolean) " + ex.Message

    Finally

      SqlEVOConn.Dispose()
      SqlEVOConn.Close()
      SqlEVOConn = Nothing

      SqlEVOCommand.Dispose()
      SqlEVOCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated Subscription Cloud Notes", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function UpdateGLOBALListing(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean
    'Dim sQuery = New StringBuilder()
    'Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    'Dim SqlReader As SqlClient.SqlDataReader
    'Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    '  Try


    bResult = UpdateSubscriptionABIFlag(bEnable)

    'If bEnable Then
    '  If HttpContext.Current.Session.Item("localPreferences").HasGlobalRecord Then
    '    sQuery.Append("UPDATE ABI_Company_service SET abicserv_end_date = NULL")
    '    sQuery.Append(" WHERE abicserv_serv_code = 'ACLIST' AND abicserv_status = 'A' AND abicserv_start_date <= GETDATE()")
    '    sQuery.Append(" AND abicserv_comp_id = " + HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString)
    '  Else
    '    sQuery.Append("INSERT INTO ABI_Company_service (abicserv_serv_code, abicserv_status, abicserv_comp_id, abicserv_start_date, abicserv_end_date)")
    '    sQuery.Append(" VALUES ('ACLIST', 'A', " + HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString + ", '" + DateTime.Now + "', NULL)")
    '  End If
    'Else
    '  If HttpContext.Current.Session.Item("localPreferences").HasGlobalRecord Then
    '    sQuery.Append("UPDATE ABI_Company_service SET abicserv_end_date = '" + DateTime.Now + "'")
    '    sQuery.Append(" WHERE abicserv_serv_code = 'ACLIST' AND abicserv_status = 'A' AND abicserv_start_date <= GETDATE()")
    '    sQuery.Append(" AND abicserv_comp_id = " + HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString)
    '  Else
    '    sQuery.Append("INSERT INTO ABI_Company_service (abicserv_serv_code, abicserv_status, abicserv_comp_id, abicserv_start_date, abicserv_end_date)")
    '    sQuery.Append(" VALUES ('ACLIST', 'A', " + HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString + ", '" + DateTime.Now + "', '" + DateTime.Now + "')")
    '  End If
    'End If

    'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateGLOBALListing(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean</b><br />" + sQuery.ToString

    'SqlConn.ConnectionString = adminConnectString
    'SqlConn.Open()

    'SqlCommand.Connection = SqlConn
    'SqlCommand.CommandType = CommandType.Text
    'SqlCommand.CommandTimeout = 60

    'Try
    '  SqlCommand.CommandText = sQuery.ToString
    '  SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
    '  bResult = True
    'Catch SqlException
    '  aError = "Error in UpdateGLOBALListing ExecuteNonQuery : " + SqlException.Message
    'End Try

    'Catch ex As Exception

    '  aError = "Error in UpdateGLOBALListing(ByVal userGUID As String, ByVal bEnable As Boolean) As Boolean " + ex.Message

    'Finally
    '  SqlReader = Nothing

    '  SqlConn.Dispose()
    '  SqlConn.Close()
    '  SqlConn = Nothing

    '  SqlCommand.Dispose()
    '  SqlCommand = Nothing
    'End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Updated GLOBAL Listing Flag GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Function InsertMPMClientPreferences(ByVal ac_category1_name As String, ByVal ac_category2_name As String, _
                                            ByVal ac_category3_name As String, ByVal ac_category4_name As String, _
                                            ByVal ac_category5_name As String, ByVal ac_category6_name As String, _
                                            ByVal ac_category7_name As String, ByVal ac_category8_name As String, _
                                            ByVal ac_category9_name As String, ByVal ac_category10_name As String, _
                                            ByVal ac_category1_use_name As String, ByVal ac_category2_use_name As String, _
                                            ByVal ac_category3_use_name As String, ByVal ac_category4_use_name As String, _
                                            ByVal ac_category5_use_name As String, ByVal ac_category6_use_name As String, _
                                            ByVal ac_category7_use_name As String, ByVal ac_category8_use_name As String, _
                                            ByVal ac_category9_use_name As String, ByVal ac_category10_use_name As String, _
                                            ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, _
                                            ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, _
                                            ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, _
                                            ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, _
                                            ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, _
                                            ByVal clipref_max_client_export As Long) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder
    Dim bReturnValue As Boolean = False

    Try

      sQuery.Append("INSERT INTO client_preference (clipref_ac_custom_1, clipref_ac_custom_2, clipref_ac_custom_3, clipref_ac_custom_4, clipref_ac_custom_5,")
      sQuery.Append(" clipref_ac_custom_6, clipref_ac_custom_7, clipref_ac_custom_8, clipref_ac_custom_9, clipref_ac_custom_10,")
      sQuery.Append(" clipref_ac_custom_1_use, clipref_ac_custom_2_use, clipref_ac_custom_3_use, clipref_ac_custom_4_use, clipref_ac_custom_5_use,")
      sQuery.Append(" clipref_ac_custom_6_use, clipref_ac_custom_7_use, clipref_ac_custom_8_use, clipref_ac_custom_9_use, clipref_ac_custom_10_use,")

      sQuery.Append(" clipref_category1_name, clipref_category2_name, clipref_category3_name, clipref_category4_name, clipref_category5_name,")
      sQuery.Append(" clipref_category1_use, clipref_category2_use, clipref_category3_use, clipref_category4_use, clipref_category5_use,")

      sQuery.Append(" clipref_max_client_export)  VALUES  (")

      sQuery.Append("'" + Left(ac_category1_name.Trim, 60) + "','" + Left(ac_category2_name.Trim, 60) + "','" + Left(ac_category3_name.Trim, 60) + "','" + Left(ac_category4_name.Trim, 60) + "','" + Left(ac_category5_name.Trim, 60) + "',")
      sQuery.Append("'" + Left(ac_category6_name.Trim, 60) + "','" + Left(ac_category7_name.Trim, 60) + "','" + Left(ac_category8_name.Trim, 60) + "','" + Left(ac_category9_name.Trim, 60) + "','" + Left(ac_category10_name.Trim, 60) + "',")

      sQuery.Append("'" + Left(ac_category1_use_name.Trim, 1) + "','" + Left(ac_category2_use_name.Trim, 1) + "','" + Left(ac_category3_use_name.Trim, 1) + "','" + Left(ac_category4_use_name.Trim, 1) + "','" + Left(ac_category5_use_name.Trim, 1) + "',")
      sQuery.Append("'" + Left(ac_category6_use_name.Trim, 1) + "','" + Left(ac_category7_use_name.Trim, 1) + "','" + Left(ac_category8_use_name.Trim, 1) + "','" + Left(ac_category9_use_name.Trim, 1) + "','" + Left(ac_category10_use_name.Trim, 1) + "',")

      sQuery.Append("'" + Left(clipref_category1_name.Trim, 60) + "','" + Left(clipref_category2_name.Trim, 60) + "','" + Left(clipref_category3_name.Trim, 60) + "','" + Left(clipref_category4_name.Trim, 60) + "','" + Left(clipref_category5_name.Trim, 60) + "',")
      sQuery.Append("'" + Left(clipref_category1_use.Trim, 1) + "','" + Left(clipref_category2_use.Trim, 1) + "','" + Left(clipref_category3_use.Trim, 1) + "','" + Left(clipref_category4_use.Trim, 1) + "','" + Left(clipref_category5_use.Trim, 1) + "', " + clipref_max_client_export.ToString + ")")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>InsertMPMClientPreferences(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String, clipref_max_client_export as long) As Integer</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()
      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in SQL InsertMPMClientPreferences(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String,  clipref_max_client_export as long) As Integer " + ex.Message

    Finally

      MySqlConn.Close()
      MySqlConn.Dispose()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing
      sQuery = Nothing

    End Try

    Return bReturnValue

  End Function

  Public Function UpdateMPMClientPreferences(ByVal ac_category1_name As String, ByVal ac_category2_name As String, _
                                            ByVal ac_category3_name As String, ByVal ac_category4_name As String, _
                                            ByVal ac_category5_name As String, ByVal ac_category6_name As String, _
                                            ByVal ac_category7_name As String, ByVal ac_category8_name As String, _
                                            ByVal ac_category9_name As String, ByVal ac_category10_name As String, _
                                            ByVal ac_category1_use_name As String, ByVal ac_category2_use_name As String, _
                                            ByVal ac_category3_use_name As String, ByVal ac_category4_use_name As String, _
                                            ByVal ac_category5_use_name As String, ByVal ac_category6_use_name As String, _
                                            ByVal ac_category7_use_name As String, ByVal ac_category8_use_name As String, _
                                            ByVal ac_category9_use_name As String, ByVal ac_category10_use_name As String, _
                                            ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, _
                                            ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, _
                                            ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, _
                                            ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, _
                                            ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, _
                                            ByVal clipref_max_client_export As Long, ByVal Original_clipref_id As Long) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try
      sQuery.Append("UPDATE client_preference")
      sQuery.Append(" SET clipref_category1_name = '" + Left(clipref_category1_name.Trim, 60) + "', clipref_category1_use = '" + Left(clipref_category1_use.Trim, 1) + "', ")
      sQuery.Append(" clipref_category2_name = '" + Left(clipref_category2_name.Trim, 60) + "', clipref_category2_use = '" + Left(clipref_category2_use.Trim, 1) + "', ")
      sQuery.Append(" clipref_category3_name = '" + Left(clipref_category3_name.Trim, 60) + "', clipref_category3_use = '" + Left(clipref_category3_use.Trim, 1) + "', ")
      sQuery.Append(" clipref_category4_name = '" + Left(clipref_category4_name.Trim, 60) + "', clipref_category4_use = '" + Left(clipref_category4_use.Trim, 1) + "', ")
      sQuery.Append(" clipref_category5_name = '" + Left(clipref_category5_name.Trim, 60) + "', clipref_category5_use = '" + Left(clipref_category5_use.Trim, 1) + "', ")

      'aircraft preferences: 
      sQuery.Append(" clipref_ac_custom_1  = '" + Replace(Left(ac_category1_name.Trim, 60), "'", "''") + "', clipref_ac_custom_1_use = '" + Left(ac_category1_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_2  = '" + Replace(Left(ac_category2_name.Trim, 60), "'", "''") + "', clipref_ac_custom_2_use = '" + Left(ac_category2_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_3  = '" + Replace(Left(ac_category3_name.Trim, 60), "'", "''") + "', clipref_ac_custom_3_use = '" + Left(ac_category3_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_4  = '" + Replace(Left(ac_category4_name.Trim, 60), "'", "''") + "', clipref_ac_custom_4_use = '" + Left(ac_category4_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_5  = '" + Replace(Left(ac_category5_name.Trim, 60), "'", "''") + "', clipref_ac_custom_5_use = '" + Left(ac_category5_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_6  = '" + Replace(Left(ac_category6_name.Trim, 60), "'", "''") + "', clipref_ac_custom_6_use = '" + Left(ac_category6_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_7  = '" + Replace(Left(ac_category7_name.Trim, 60), "'", "''") + "', clipref_ac_custom_7_use = '" + Left(ac_category7_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_8  = '" + Replace(Left(ac_category8_name.Trim, 60), "'", "''") + "', clipref_ac_custom_8_use = '" + Left(ac_category8_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_9  = '" + Replace(Left(ac_category9_name.Trim, 60), "'", "''") + "', clipref_ac_custom_9_use = '" + Left(ac_category9_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_ac_custom_10  = '" + Replace(Left(ac_category10_name.Trim, 60), "'", "''") + "', clipref_ac_custom_10_use = '" + Left(ac_category10_use_name.Trim, 1) + "',")
      sQuery.Append(" clipref_max_client_export = " + clipref_max_client_export.ToString)

      sQuery.Append(" WHERE (clipref_id = " + Original_clipref_id.ToString + ") limit 1")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>UpdateMPMClientPreferences(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String, clipref_max_client_export As Double, ByVal Original_clipref_id As Integer</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in SQL UpdateMPMClientPreferences(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String, clipref_max_client_export As Double, ByVal Original_clipref_id As Integer " + ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing
      sQuery = Nothing

    End Try

    Return bReturnValue

  End Function

  Public Function UpdateMPMSingleClientPreference(ByVal ac_category_name As String, ByVal ac_category_use_name As String, ByVal number_of As String, ByVal Original_clipref_id As Long) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try

      sQuery.Append("UPDATE client_preference")
      sQuery.Append(" SET clipref_ac_custom_" + number_of.Trim + "  = '" + Replace(Left(ac_category_name.Trim, 60), "'", "''") + "', clipref_ac_custom_" + number_of.Trim + "_use = '" + Left(ac_category_use_name.Trim, 1) + "'")

      sQuery.Append(" WHERE (clipref_id = " + Original_clipref_id.ToString + ") limit 1")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>UpdateMPMSingleClientPreference(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String, clipref_max_client_export As Double, ByVal Original_clipref_id As Integer</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in SQL UpdateMPMSingleClientPreference(ByVal clipref_category1_name As String, ByVal clipref_category2_name As String, ByVal clipref_category3_name As String, ByVal clipref_category4_name As String, ByVal clipref_category5_name As String, ByVal clipref_category1_use As String, ByVal clipref_category2_use As String, ByVal clipref_category3_use As String, ByVal clipref_category4_use As String, ByVal clipref_category5_use As String, ByVal clipref_activity_default_days As String, clipref_max_client_export As Double, ByVal Original_clipref_id As Integer " + ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function InsertMPMCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try

      sQuery.Append("INSERT INTO client_custom_export (clicexp_type, clicexp_display, clicexp_client_db_name, clicexp_jetnet_db_name, clicexp_sort, clicexp_aerodex_flag")
      sQuery.Append(" ,clicexp_header_field_name, clicexp_field_type, clicexp_field_length) ")
      sQuery.Append(" VALUES ( ")
      sQuery.Append("'" + exportType.Trim + "', ")
      sQuery.Append("'" + exportText.Trim + "', ")
      sQuery.Append("'clicomp_category" + number_of.Trim + " AS ''" + exportText.Trim + "''', ")
      sQuery.Append("''''' AS ''" + exportText.Trim + "''', ")
      sQuery.Append("'10" + number_of.Trim + "', ")
      sQuery.Append("'Y', ")
      sQuery.Append("'" + exportText.Trim + "', ")
      sQuery.Append("'String', ")
      sQuery.Append("'150' ")
      sQuery.Append(" )")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>InsertMPMCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String) As Boolean</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in InsertMPMCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String) As Boolean" + ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function UpdateMPMSingleCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String, ByVal exportID As Long) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try

      sQuery.Append("UPDATE client_custom_export SET")
      sQuery.Append(" clicexp_display = '" + exportText.Trim + "' ")
      sQuery.Append(", clicexp_header_field_name = '" + exportText.Trim + "' ")
      sQuery.Append(", clicexp_client_db_name = 'clicomp_category" + number_of.Trim + " AS ''" + exportText.Trim + "''' ")
      sQuery.Append(", clicexp_jetnet_db_name = ''''' AS ''" + exportText.Trim + "''' ")
      sQuery.Append(" where clicexp_id = '" + exportID.ToString + "'")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>UpdateMPMSingleCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String, ByVal exportID As Long) As Boolean</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in UpdateMPMSingleCustomExportItem(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String, ByVal exportID As Long) As Boolean" + ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function DeleteMPMCustomExportItem(ByVal type_of_export As String, Optional ByVal sort_no As String = "") As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      sQuery.Append("DELETE FROM client_custom_export WHERE clicexp_type = '" + type_of_export.Trim + "'")

      If Not String.IsNullOrEmpty(sort_no.Trim) Then
        sQuery.Append(" AND clicexp_sort = '10" + sort_no.Trim + "'")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>DeleteMPMCustomExport(ByVal type_of_export As String, Optional ByVal sort_no As String = "") As Boolean</b><br />" + sQuery.ToString

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in DeleteMPMCustomExport(ByVal type_of_export As String, Optional ByVal sort_no As String = "") As Boolean " & ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

  Public Function DeleteMPMClientProjectReference(ByVal exportID As Long) As Boolean

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Boolean = False

    Try

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      sQuery.Append("DELETE FROM client_project_reference WHERE clipref_exp_id = '" + exportID.ToString + "' AND clipref_source = 'CLIENT'")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>DeleteMPMClientProjectReference(ByVal exportID As Long) As Boolean</b><br />" + sQuery.ToString

      MySqlCommand.CommandText = sQuery.ToString
      MySqlCommand.ExecuteNonQuery()

      bReturnValue = True

    Catch ex As Exception

      Me.class_error = "Error in DeleteMPMClientProjectReference(ByVal exportID As Long) As Boolean " & ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

  Function FindMPMCustomExportID(ByVal type_of_export As String, ByVal sort_no As String) As Long

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
    Dim sQuery As New StringBuilder

    Dim bReturnValue As Long = 0

    Try
      sQuery.Append("SELECT clicexp_id FROM client_custom_export WHERE clicexp_type = '" + type_of_export.Trim + "' AND clicexp_sort = '10" + sort_no.Trim + "'")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>FindMPMCustomExportID(ByVal type_of_export As String, ByVal sort_no As String) As Long</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sQuery.ToString
      MySqlReader = MySqlCommand.ExecuteReader()

      If MySqlReader.HasRows Then

        MySqlReader.Read()

        If Not (IsDBNull(MySqlReader("clicexp_id"))) Then
          bReturnValue = CLng(MySqlReader.Item("clicexp_id").ToString)
        End If

        MySqlReader.Close()

      End If 'MySqlReader.HasRows 

      MySqlReader.Dispose()

    Catch ex As Exception

      Me.class_error = "Error in UpdateMPMSingleCustomExport(ByVal exportText As String, ByVal exportType As String, ByVal number_of As String, ByVal exportID As Long) As Boolean" + ex.Message

    Finally

      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing

      sQuery = Nothing
    End Try

    Return bReturnValue

  End Function

#End Region

#Region "subscriber_admin_functions"

  Public Function DisplayAdminUserList(ByVal bShareByComp As Boolean, ByVal bShareBySub As Boolean, ByVal sub_id As Long, ByVal parent_sub_id As Long, ByVal sub_comp_id As Long) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT distinct contact_sirname, contact_first_name, contact_last_name, contact_email_address, sublogin_password,")
      sQuery.Append(" comp_city, comp_state, contact_id, sub_id, sub_parent_sub_id, comp_id, sub_server_side_notes_flag,")
      sQuery.Append(" sub_cloud_notes_flag, subins_admin_flag")
      sQuery.Append(" FROM Subscription WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND (sublogin_login=subins_login)")
      sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
      sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")
      sQuery.Append(" WHERE subins_active_flag='Y'  and sublogin_active_flag = 'Y' AND sub_start_date <= GETDATE()")
      sQuery.Append(" AND (sub_end_date IS NULL OR sub_end_date > GETDATE())")


      If bShareByComp Then
        sQuery.Append(Constants.cAndClause + "sub_comp_id = " + sub_comp_id.ToString)
      ElseIf bShareBySub Then
        sQuery.Append(Constants.cAndClause + "sub_parent_sub_id = " + parent_sub_id.ToString)
      Else
        sQuery.Append(Constants.cAndClause + "sub_id = " + sub_id.ToString)
      End If

      sQuery.Append(" AND contact_active_flag = 'Y'")
      sQuery.Append(" ORDER BY contact_last_name, contact_first_name")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>DisplayAdminUserList(ByVal sub_id As Long, ByVal parent_sub_id As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in DisplayAdminUserList load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in DisplayAdminUserList(ByVal sub_id As Long, ByVal parent_sub_id As Long) As DataTable" + ex.Message

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

  Public Function DisplayTotalUserLicenses(ByVal bShareByComp As Boolean, ByVal bShareBySub As Boolean, ByVal sub_id As Long, ByVal parent_sub_id As Long, ByVal sub_comp_id As Long) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT sum(sub_nbr_of_installs) AS totlicenses")
      sQuery.Append(" FROM Subscription WITH(NOLOCK)")
      sQuery.Append(" WHERE sub_start_date <= GETDATE()")
      sQuery.Append(Constants.cAndClause + "(sub_end_date IS NULL OR sub_end_date > GETDATE())")

      If bShareByComp Then
        sQuery.Append(Constants.cAndClause + "sub_comp_id = " + sub_comp_id.ToString)
      ElseIf bShareBySub Then
        sQuery.Append(Constants.cAndClause + "sub_parent_sub_id = " + parent_sub_id.ToString)
      Else
        sQuery.Append(Constants.cAndClause + "sub_id = " + sub_id.ToString)
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>DisplayTotalUserLicenses(ByVal sub_id As Long, ByVal parent_sub_id As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in DisplayTotalUserLicenses load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in DisplayTotalUserLicenses(ByVal sub_id As Long, ByVal parent_sub_id As Long) As DataTable" + ex.Message

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

#End Region

#Region "general_functions"

  Public Function getUserSubscriptionInfo() As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT distinct contact_sirname, contact_first_name, contact_last_name, contact_email_address, sublogin_password,")
      sQuery.Append(" comp_city, comp_state, contact_id, sub_id, sub_parent_sub_id, comp_id, sub_server_side_notes_flag,")
      sQuery.Append(" sub_cloud_notes_flag, subins_admin_flag")
      sQuery.Append(" FROM Subscription WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND (sublogin_login=subins_login)")
      sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
      sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")
      sQuery.Append(" WHERE subins_active_flag='Y' AND sub_start_date <= GETDATE()")
      sQuery.Append(" AND (sub_end_date is NULL or sub_end_date > GETDATE() )")
      'sQuery.Append(" AND (sub_id = " + sub_id.ToString + " OR sub_parent_sub_id=" + parent_sub_id.ToString + ")")
      sQuery.Append(" AND contact_active_flag='Y'")
      sQuery.Append(" ORDER BY contact_last_name, contact_first_name")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getUserSubscriptionInfo() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in getUserSubscriptionInfo load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in getUserSubscriptionInfo() As DataTable" + ex.Message

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

  Public Function saveAsDefaultAirportFolder(ByVal oldAportFolderID As Long, ByVal newAportFolderID As Long) As Boolean
    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try


      SqlConn.ConnectionString = adminConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try

                If oldAportFolderID > 0 Then

                    sQuery.Append("UPDATE Client_Folder SET cfolder_default_flag = 'N' WHERE cfolder_id = " + oldAportFolderID.ToString)
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />saveAsDefaultAirportFolder(ByVal oldAportFolderID As Long, ByVal newAportFolderID As Long) As Boolean</b><br />" + sQuery.ToString
                    SqlCommand.CommandText = sQuery.ToString
                    SqlCommand.ExecuteNonQuery()
                Else
                    HttpContext.Current.Session.Item("currentDefaultAirportFolderID") = 0
                End If


                If newAportFolderID <> 0 Then

                    sQuery = New StringBuilder()
                    sQuery.Append("UPDATE Client_Folder SET cfolder_default_flag = 'Y' WHERE cfolder_id = " + newAportFolderID.ToString)
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />saveAsDefaultAirportFolder(ByVal oldAportFolderID As Long, ByVal newAportFolderID As Long) As Boolean</b><br />" + sQuery.ToString
                    SqlCommand.CommandText = sQuery.ToString
                    SqlCommand.ExecuteNonQuery()

                    HttpContext.Current.Session.Item("currentDefaultAirportFolderID") = newAportFolderID
                Else
                    HttpContext.Current.Session.Item("currentDefaultAirportFolderID") = 0
                End If


                bResult = True
      Catch SqlException
        aError = "Error in saveAsDefaultAirportFolder ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      aError = "Error in saveAsDefaultAirportFolder(ByVal AportFolderID As Long)) As Boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Saved Default Airport Folder ( old FID: " + oldAportFolderID.ToString + " new FID: " + newAportFolderID.ToString + " )", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

#End Region

End Class
