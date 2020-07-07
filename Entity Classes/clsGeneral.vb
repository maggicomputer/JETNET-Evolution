Imports System.Net.Mail
Namespace clsGeneral
    Public Class clsGeneral
        Inherits System.Web.UI.Page

        'This handles Jetnet/Client Image Source Display on Listing Pages and anywhere that ICON is needed.
        Public Shared Function WhatAmI(ByVal x As Object) As String

            'Handles Jetnet/Client Display Image
            WhatAmI = ""
            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                Try
                    If Not IsDBNull(x) Then
                        x = x.ToString
                        If UCase(x) = "CLIENT" Then
                            WhatAmI = "<img src='images/client.png' alt='CLIENT RECORD' title='CLIENT RECORD' class='ico_padding'/>"
                            Return WhatAmI
                        Else
                            WhatAmI = "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding' title='JETNET RECORD' />"
                            Return WhatAmI
                        End If

                    End If
                Catch ex As Exception
                    'error_string = "clsGeneral.vb - WhatAmI() - " & ex.Message
                    'LogError(error_string)
                End Try
            End If
        End Function


        Public Shared Function PrepForJS(ByVal Text As Object) As String
            PrepForJS = ""
            If Not IsNothing(Text) Then
                Dim illegalChars As Char() = "&/\#,+()$~%..'"":*?<>{}"
                Dim str As String = Text.ToString
                Dim first As New System.Text.StringBuilder

                For Each ch As Char In str
                    If Array.IndexOf(illegalChars, ch) = -1 Then
                        first.Append(ch)
                    Else
                        first.Append("\" + ch)
                    End If
                Next
                Return Replace(first.ToString, Environment.NewLine, "")
            End If
        End Function


        '  [dbo].[Aircraft_Attribute_Model](
        '[attmod_id] [int] IDENTITY(1,1) NOT NULL,
        '[attmod_att_id] [int] NULL,
        '[attmod_amod_id] [int] NULL,
        '[attmod_seq_no] [smallint] NULL,
        '[attmod_standard_equip] [varchar](1) NULL,
        '[attmod_stdeq_start_ser_no_value] [int] NULL,
        '[attmod_stdeq_end_ser_no_value] [int] NULL,
        '[attmod_value] [float] NULL
        ') ON [PRIMARY]



        Public Sub SaveModelAttribute(ByVal attmod_att_id As Long, ByVal attmod_amod_id As Long, ByVal attmod_seq_no As Long, ByVal attmod_standard_equip As String, ByVal attmod_value As String, ByVal attmod_stdeq_start_ser_no_value As String, ByVal attmod_stdeq_end_ser_no_value As String)

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim update_string As String = ""
            Dim temp_id As Long = 0


            Try

                If IsNumeric(attmod_att_id) Then
                    If attmod_att_id > 0 Then

                        update_string = " Insert into " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model (attmod_att_id, attmod_amod_id, attmod_seq_no, attmod_standard_equip "

                        If Not String.IsNullOrEmpty(attmod_stdeq_start_ser_no_value) Then
                            update_string += " , attmod_stdeq_start_ser_no_value "
                        End If

                        If Not String.IsNullOrEmpty(attmod_stdeq_end_ser_no_value) Then
                            update_string += " , attmod_stdeq_end_ser_no_value "
                        End If

                        If Not String.IsNullOrEmpty(attmod_value) Then
                            update_string += " , attmod_value "
                        End If

                        update_string += ") VALUES (@attmod_att_id, @attmod_amod_id, @attmod_seq_no, @attmod_standard_equip "

                        If Not String.IsNullOrEmpty(attmod_stdeq_start_ser_no_value) Then
                            update_string += " , @attmod_stdeq_start_ser_no_value "
                        End If

                        If Not String.IsNullOrEmpty(attmod_stdeq_end_ser_no_value) Then
                            update_string += " , @attmod_stdeq_end_ser_no_value "
                        End If

                        If Not String.IsNullOrEmpty(attmod_value) Then
                            update_string += " , @attmod_value "
                        End If

                        update_string += ")"

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("attmod_att_id", attmod_att_id)
                        SqlCommand.Parameters.AddWithValue("attmod_amod_id", attmod_amod_id)
                        SqlCommand.Parameters.AddWithValue("attmod_seq_no", attmod_seq_no)
                        SqlCommand.Parameters.AddWithValue("attmod_standard_equip", attmod_standard_equip)

                        If Not String.IsNullOrEmpty(attmod_stdeq_start_ser_no_value) Then
                            SqlCommand.Parameters.AddWithValue("attmod_stdeq_start_ser_no_value", attmod_stdeq_start_ser_no_value)
                        End If

                        If Not String.IsNullOrEmpty(attmod_stdeq_end_ser_no_value) Then
                            SqlCommand.Parameters.AddWithValue("attmod_stdeq_end_ser_no_value", attmod_stdeq_end_ser_no_value)
                        End If

                        If Not String.IsNullOrEmpty(attmod_value) Then
                            SqlCommand.Parameters.AddWithValue("attmod_value", attmod_value)
                        End If

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, update_string.ToString)
                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing


            End Try

        End Sub

        Public Sub DeleteModelAttribute(ByVal attmod_amod_id As Long)

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim update_string As String = ""
            Dim temp_id As Long = 0


            Try

                If IsNumeric(attmod_amod_id) Then
                    If attmod_amod_id > 0 Then

                        update_string = " Delete FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model where attmod_amod_id = @attmod_amod_id "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)
                        SqlCommand.Parameters.AddWithValue("attmod_amod_id", attmod_amod_id)


                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, update_string.ToString)
                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing


            End Try

        End Sub

        Public Function SelectModelAttribute(ByVal attmod_amod_id As Long) As DataTable
            Dim SqlReader As SqlClient.SqlDataReader

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim select_string As String = ""
            Dim temp_id As Long = 0
            Dim aTempTable As New DataTable

            Try

                If IsNumeric(attmod_amod_id) Then
                    If attmod_amod_id > 0 Then

                        select_string = " Select * from " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model with (NOLOCK) where attmod_amod_id = @attmod_amod_id "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(select_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("attmod_amod_id", attmod_amod_id)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, select_string.ToString)
                        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                        Try
                            aTempTable.Load(SqlReader)
                        Catch constrExc As System.Data.ConstraintException
                            Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                            Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, "Error in SelectModelAttribute() load datatable " + constrExc.Message)
                        End Try

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing

            End Try
            Return aTempTable
        End Function
        Public Shared Function SelectModelAttributeByID(ByVal attmod_id As Long) As DataTable
            Dim SqlReader As SqlClient.SqlDataReader

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim select_string As String = ""
            Dim temp_id As Long = 0
            Dim aTempTable As New DataTable

            Try

                If IsNumeric(attmod_id) Then
                    If attmod_id > 0 Then

                        select_string = " Select * from " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model with (NOLOCK) "
                        select_string += " left outer join " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "aircraft_attribute with (NOLOCK) on acatt_id = attmod_att_id "
                        select_string += " where attmod_id = @attmod_id "
                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(select_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("attmod_id", attmod_id)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", select_string.ToString)
                        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                        Try
                            aTempTable.Load(SqlReader)
                        Catch constrExc As System.Data.ConstraintException
                            Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                            Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", "Error in SelectModelAttribute() load datatable " + constrExc.Message)
                        End Try

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing

            End Try
            Return aTempTable
        End Function
        Public Shared Sub UpdateModelAttribute(ByVal attmod_standard_equip As String, ByVal attmod_value As String, ByVal attmod_id As Long, ByVal attmod_amod_id As Long, attmod_notes As String, attmod_ser_start As String, attmod_ser_end As String)

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim update_string As String = ""
            Dim temp_id As Long = 0


            Try

                If IsNumeric(attmod_id) Then
                    If attmod_id > 0 Then

                        update_string = " Update " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model set attmod_standard_equip = @attmod_standard_equip "

                        If Not String.IsNullOrEmpty(attmod_value) Then
                            update_string += ", attmod_value = @attmod_value "
                        Else
                            update_string += ", attmod_value = NULL "
                        End If

                        If Not String.IsNullOrEmpty(attmod_notes) Then
                            update_string += ", attmod_notes = @attmod_notes "
                        Else
                            update_string += ", attmod_notes = NULL "
                        End If

                        If Not String.IsNullOrEmpty(attmod_ser_start) Then
                            update_string += ", attmod_stdeq_start_ser_no_value = @attmod_stdeq_start_ser_no_value "
                        Else
                            update_string += ", attmod_stdeq_start_ser_no_value = NULL "
                        End If


                        If Not String.IsNullOrEmpty(attmod_ser_end) Then
                            update_string += ", attmod_stdeq_end_ser_no_value = @attmod_stdeq_end_ser_no_value "
                        Else
                            update_string += ", attmod_stdeq_end_ser_no_value = NULL "
                        End If


                        update_string += " where attmod_id = @attmod_id and attmod_amod_id = @attmod_amod_id "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("attmod_standard_equip", attmod_standard_equip)
                        If Not String.IsNullOrEmpty(attmod_value) Then
                            SqlCommand.Parameters.AddWithValue("attmod_value", attmod_value)
                        End If

                        If Not String.IsNullOrEmpty(attmod_notes) Then
                            SqlCommand.Parameters.AddWithValue("attmod_notes", attmod_notes)
                        End If

                        If Not String.IsNullOrEmpty(attmod_ser_start) Then
                            SqlCommand.Parameters.AddWithValue("attmod_stdeq_start_ser_no_value", attmod_ser_start)
                        End If

                        If Not String.IsNullOrEmpty(attmod_ser_end) Then
                            SqlCommand.Parameters.AddWithValue("attmod_stdeq_end_ser_no_value", attmod_ser_end)
                        End If

                        SqlCommand.Parameters.AddWithValue("attmod_id", attmod_id)
                        SqlCommand.Parameters.AddWithValue("attmod_amod_id", attmod_amod_id)


                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsgeneral", update_string.ToString)
                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing


            End Try

        End Sub
        Public Shared Sub FillPriorityDropdown(ByRef crmProspectActionTaken As Object)
            Dim TempPriority As DataTable = Get_Local_Notes_Priority()
            If Not IsNothing(TempPriority) Then
                If TempPriority.Rows.Count > 0 Then
                    For Each r As DataRow In TempPriority.Rows
                        crmProspectActionTaken.Items.Add(New ListItem(r("clipri_name").ToString, r("clipri_id").ToString))
                    Next
                End If
            End If


            TempPriority.Dispose()
        End Sub
        Public Shared Function Get_Local_Notes_Priority() As DataTable
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim aTempTable As New DataTable

            Try

                sql = "SELECT clipri_ID, clipri_name FROM client_priority "


                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Local_Notes_Priority() As DataTable</b><br />" & sql


                MySqlCommand.CommandText = sql
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                Return aTempTable
            Catch ex As Exception
                Get_Local_Notes_Priority = Nothing
            Finally
                MySqlReader.Close()
                MySqlReader = Nothing

                MySqlConn.Close()
                MySqlConn.Dispose()
                MySqlConn = Nothing

                MySqlCommand.Dispose()
                MySqlCommand = Nothing
            End Try

        End Function
        Public Shared Function CheckAircraftAlertFolderExistence(subID As Long, userLogin As String) As DataTable
            Dim SqlReader As SqlClient.SqlDataReader

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim select_string As String = ""
            Dim temp_id As Long = 0
            Dim aTempTable As New DataTable

            Try

                If IsNumeric(subID) Then
                    If subID > 0 Then

                        select_string = " Select top 1 cfolder_id from Client_Folder with (NOLOCK) "
                        'select_string += " INNER join Client_Folder_Index with (NOLOCK) on cfolder_id=cfoldind_cfolder_id "
                        select_string += " where cfolder_cftype_id = 3 and cfolder_jetnet_run_flag = 'Y' and cfolder_method='S' "
                        select_string += " And cfolder_sub_id = @subID And cfolder_login = @userLogin "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(select_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("@subID", subID)
                        SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", select_string.ToString)
                        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                        Try
                            aTempTable.Load(SqlReader)
                        Catch constrExc As System.Data.ConstraintException
                            Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                            Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", "Error in CheckAircraftAlertFolderExistence(subID As Long, userLogin As String) As DataTable load datatable " + constrExc.Message)
                        End Try

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing

            End Try
            Return aTempTable
        End Function
        Public Shared Function CheckAircraftAlertsOn(ByVal ac_id As Long, subID As Long, userLogin As String) As DataTable
            Dim SqlReader As SqlClient.SqlDataReader

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim select_string As String = ""
            Dim temp_id As Long = 0
            Dim aTempTable As New DataTable

            Try

                If IsNumeric(ac_id) Then
                    If ac_id > 0 Then

                        select_string = " Select top 1 cfoldind_id from Client_Folder with (NOLOCK) "
                        select_string += " INNER join Client_Folder_Index with (NOLOCK) on cfolder_id=cfoldind_cfolder_id "
                        select_string += " where cfolder_cftype_id = 3 and cfolder_jetnet_run_flag = 'Y' "
                        select_string += " and cfoldind_jetnet_ac_id = @acID"
                        select_string += " And cfolder_sub_id = @subID And cfolder_login = @userLogin "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        SqlConn.Open()


                        Dim SqlCommand As New SqlClient.SqlCommand(select_string, SqlConn)

                        SqlCommand.Parameters.AddWithValue("@acID", ac_id)
                        SqlCommand.Parameters.AddWithValue("@subID", subID)
                        SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", select_string.ToString)
                        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                        Try
                            aTempTable.Load(SqlReader)
                        Catch constrExc As System.Data.ConstraintException
                            Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                            Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral", "Error in CheckAircraftAlertsOn(ByVal ac_id As Long) load datatable " + constrExc.Message)
                        End Try

                        SqlCommand.Dispose()
                        SqlCommand = Nothing
                    End If
                End If

            Catch ex As Exception
            Finally
                SqlConn.Dispose()
                SqlConn.Close()
                SqlConn = Nothing

            End Try
            Return aTempTable
        End Function
        Public Shared Function IsDataTypeNumeric(ByVal col As DataColumn) As Boolean
            If col Is Nothing Then Return False
            Dim numericTypes As Type() = {GetType(Byte), GetType(Decimal), GetType(Double), GetType(Int16), GetType(Int32), GetType(Int64), GetType(SByte), GetType(Single), GetType(UInt16), GetType(UInt32), GetType(UInt64)}
            Dim answer As Boolean = numericTypes.Contains(col.DataType)
            Return answer
        End Function
        Public Shared Function isEValuesAvailable() As Boolean
            Dim returnFlag As Boolean = False
            If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                ' If (HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Or HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.TEST) Then
                returnFlag = True
                'End If
            End If
            Return returnFlag
        End Function
        Public Shared Function isShowingEvalues() As Boolean

            Dim returnFlag As Boolean = False
            Dim NoteCookieName As String = ""

            NoteCookieName = "evalues"
            Dim _NoteCookies As HttpCookie = HttpContext.Current.Request.Cookies(NoteCookieName)
            Dim aCookie As New HttpCookie(NoteCookieName)

            If _NoteCookies Is Nothing Then
                HttpContext.Current.Response.Cookies(NoteCookieName).Value = "true"
                HttpContext.Current.Response.Cookies(NoteCookieName).Expires = DateTime.Now.AddDays(365)
                aCookie.Value = "true"
                aCookie.Expires = DateTime.Now.AddDays(365)
                HttpContext.Current.Response.Cookies.Add(aCookie)
            Else
                If Trim(_NoteCookies.Value) = "true" Then
                    returnFlag = True
                Else
                    returnFlag = False
                End If
            End If




            'Dim ToggleCookie As HttpCookie = HttpContext.Current.Response.Cookies("evalues")
            'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
            '  If Not IsNothing(ToggleCookie) Then
            '    If ToggleCookie.Value = "true" Then
            '      returnFlag = True
            '    End If
            '  End If
            'End If

            Return returnFlag
        End Function
        Public Shared Function UpdateEvalues(ByVal update_to As String) As Boolean

            Dim NoteCookieName As String = ""
            Dim returnFlag As Boolean = False

            NoteCookieName = "evalues"
            Dim _NoteCookies As HttpCookie = HttpContext.Current.Request.Cookies(NoteCookieName)

            If Trim(update_to) = "Y" Then
                HttpContext.Current.Response.Cookies(NoteCookieName).Value = "true"
            Else
                HttpContext.Current.Response.Cookies(NoteCookieName).Value = "false"
            End If

            HttpContext.Current.Response.Cookies(NoteCookieName).Expires = DateTime.Now.AddDays(365)
            HttpContext.Current.Response.Cookies.Add(HttpContext.Current.Response.Cookies(NoteCookieName))

            Dim temp_link As String = ""
            temp_link = HttpContext.Current.Request.Url.AbsoluteUri.ToString
            ' then replace whatever it was 
            If InStr(temp_link, "evalues_update") > 0 Then
                temp_link = Replace(temp_link, "&evalues_update=N", "")
                temp_link = Replace(temp_link, "&evalues_update=Y", "")
            End If

            HttpContext.Current.Response.Redirect(temp_link)

            'Dim returnFlag As Boolean = False
            'Dim ToggleCookie As HttpCookie = HttpContext.Current.Response.Cookies("evalues")
            'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
            '  If Not IsNothing(ToggleCookie) Then
            '    If Trim(update_to) = "Y" Then
            '      HttpContext.Current.Response.Cookies("evalues").Value = "true"
            '      HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
            '    ElseIf Trim(update_to) = "N" Then
            '      HttpContext.Current.Response.Cookies("evalues").Value = "false"
            '      HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
            '    End If
            '  Else
            '    If Trim(update_to) = "Y" Then
            '      HttpContext.Current.Response.Cookies("evalues").Value = "true"
            '      HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
            '    Else
            '      HttpContext.Current.Response.Cookies("evalues").Value = "false"
            '      HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
            '    End If 
            '  End If 
            'End If

            Return returnFlag
        End Function
        Public Shared Function isCrmDisplayMode() As Boolean
            Dim returnFlag As Boolean = False
            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Or HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                returnFlag = True
            End If

            Return returnFlag
        End Function
        Public Shared Sub fillRangeDropDowns(ByRef MyDropDownControl As DropDownList, ByRef maxWidth As Long, ByVal inAmodID As String, ByVal bIsForSale As Boolean, Optional ByVal modelPlaceholderText As String = "")

            Dim results_table As New DataTable

            Dim fAmod_make_name As String = ""
            Dim fAmod_model_name As String = ""
            Dim fAmod_id As String = ""
            Dim fAmod_max_range_miles As String = ""
            Dim fAmod_range_tanks_full As String = ""
            Dim fAmod_range_seats_full As String = ""
            Dim fAmod_airframe_type_code As String = ""

            Dim sMakeModelName As String = ""

            Dim dTmpRange As Double = 0
            'Dim sTmpRange As String = ""
            Dim sTmpValue As String = ""

            MyDropDownControl.Items.Clear()

            If String.IsNullOrEmpty(inAmodID) Then
                inAmodID = "-1"
            End If

            Try

                results_table = commonEvo.Get_MakesModels_ByProductCode(bIsForSale)

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        MyDropDownControl.Items.Add(New ListItem(IIf(modelPlaceholderText <> "", modelPlaceholderText, "")))

                        For Each r As DataRow In results_table.Rows

                            If Not (IsDBNull(r("amod_make_name"))) Then
                                fAmod_make_name = r.Item("amod_make_name").ToString
                            End If

                            If Not (IsDBNull(r("amod_model_name"))) Then
                                fAmod_model_name = r.Item("amod_model_name").ToString
                            End If

                            If Not (IsDBNull(r("amod_id"))) Then
                                fAmod_id = r.Item("amod_id").ToString
                            End If

                            If Not (IsDBNull(r("amod_max_range_miles"))) Then
                                fAmod_max_range_miles = r.Item("amod_max_range_miles").ToString
                            End If

                            If Not (IsDBNull(r("amod_range_tanks_full"))) Then
                                fAmod_range_tanks_full = r.Item("amod_range_tanks_full").ToString
                            End If

                            If Not (IsDBNull(r("amod_range_seats_full"))) Then
                                fAmod_range_seats_full = r.Item("amod_range_seats_full").ToString
                            End If

                            If Not (IsDBNull(r("amod_airframe_type_code"))) Then
                                fAmod_airframe_type_code = r.Item("amod_airframe_type_code").ToString.ToUpper
                            End If

                            sMakeModelName = fAmod_make_name.Trim + " " + fAmod_model_name.Trim

                            If (sMakeModelName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                maxWidth = (sMakeModelName.Length * Constants._STARTCHARWIDTH)
                            End If

                            Select Case fAmod_airframe_type_code
                                Case Constants.AMOD_ROTARY_AIRFRAME
                                    dTmpRange = ConversionFunctions.ConvertNauticalMileToMeter(CDbl(fAmod_range_tanks_full)).ToString
                                Case Else
                                    dTmpRange = ConversionFunctions.ConvertNauticalMileToMeter(CDbl(fAmod_max_range_miles)).ToString
                            End Select

                            sTmpValue = fAmod_id.Trim + "|" + dTmpRange.ToString.Trim

                            If dTmpRange > 0 Then
                                MyDropDownControl.Items.Add(New ListItem(sMakeModelName, sTmpValue))
                                If (CLng(fAmod_id) = CLng(inAmodID)) Then
                                    MyDropDownControl.SelectedValue = sTmpValue
                                End If
                            End If



                        Next

                        If maxWidth > 0 Then
                            MyDropDownControl.Width = (maxWidth)
                        Else
                            MyDropDownControl.Width = Unit.Percentage(100)

                        End If
                    End If
                End If

            Catch ex As Exception

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in [clsGeneral.vb :  [fillRangeDropDowns] : " + ex.Message

            Finally


            End Try


        End Sub
        Public Shared Function translateClientAircraftToJetnet(ByVal dataIn As String) As String
            If InStr(dataIn, "search_field=1") > 0 Then 'search reg/ser
                dataIn = Replace(dataIn, "search_for_txt=", "ac_ser_no_from=")
            ElseIf InStr(dataIn, "search_field=2") > 0 Then 'ser
                dataIn = Replace(dataIn, "search_for_txt=", "ac_ser_no_from=")
            ElseIf InStr(dataIn, "search_field=3") > 0 Then 'reg
                dataIn = Replace(dataIn, "search_for_txt=", "ac_reg_no=")
            ElseIf InStr(dataIn, "search_field=4") > 0 Then 'id
                dataIn = Replace(dataIn, "search_for_txt=", "ac_id=")
            End If

            dataIn = Replace(dataIn, "subset=JC", "searchTypeDropdown=JC")
            dataIn = Replace(dataIn, "subset=C", "searchTypeDropdown=C")
            dataIn = Replace(dataIn, "subset=J", "searchTypeDropdown=J")
            dataIn = Replace(dataIn, "country", "cboCompanyCountryID")
            dataIn = Replace(dataIn, "airport_name=", "ac_aport_name=")
            dataIn = Replace(dataIn, "city=", "comp_city=")
            dataIn = Replace(dataIn, "iata_code=", "ac_aport_iata_code=")
            dataIn = Replace(dataIn, "icao_code=", "ac_aport_icao_code=")
            dataIn = Replace(dataIn, "ac_lifecycle_dropdown=", "ac_lifecycle_stage=")
            dataIn = Replace(dataIn, "on_exclusive=", "lease_status=")
            dataIn = Replace(dataIn, "types_of_owners=whole", "ac_ownership_type=W")
            dataIn = Replace(dataIn, "types_of_owners=fractional", "ac_ownership_type=F")
            dataIn = Replace(dataIn, "types_of_owners=shared", "ac_ownership_type=S")

            If InStr(dataIn, "year_end") > 0 Then
                dataIn = Replace(dataIn, "year_start=", "COMPARE_ac_year=Between!~!ac_year=")
                dataIn = Replace(dataIn, "!~!year_end=", ":")
                'COMPARE_ac_mfr_year=Between!~!ac_mfr_year=2000:2015
            Else
                dataIn = Replace(dataIn, "year_start=", "COMPARE_ac_year=Equals!~!ac_year=")
            End If

            'COMPARE_ac_mfr_year=Between!~!ac_mfr_year=2000:2011

            'model_cbo=272|CHALLENGER|300|Both|4 
            'cboAircraftMakeID=CHALLENGER|29
            'cboAircraftModelID=
            If InStr(dataIn, "model_cbo=") > 0 Then
                Dim dataSpl As Array = Split(dataIn, "model_cbo=")
                Dim restofArray As Array = Split(dataSpl(1), "!~!")

                Dim StrModel As String = "model_cbo=" & restofArray(0)
                Dim ModelToSendBack As String = ""
                Dim IDToSendBack As String = ""
                If InStr(restofArray(0), "##") > 0 Then 'More than 1 model
                    'Split models
                    Dim LastModel As String = ""
                    Dim individualModels As Array = Split(restofArray(0), "##")
                    For MultipleSelectionCount = 0 To UBound(individualModels)
                        Dim ModelsToLookUp As Array = Split(individualModels(MultipleSelectionCount), "|")
                        LastModel = ModelsToLookUp(1)
                        If MultipleSelectionCount = 0 Then
                            ModelToSendBack = "cboAircraftMakeID=" & ModelsToLookUp(1) & "|" & commonEvo.ReturnAmodIDForItemIndex(commonEvo.FindIndexForFirstItem(UCase(ModelsToLookUp(1)), crmWebClient.Constants.AIRFRAME_MAKE))
                            IDToSendBack = "cboAircraftModelID=" & ModelsToLookUp(0)
                        Else
                            If LastModel <> ModelsToLookUp(1) Then
                                ModelToSendBack += "##" & ModelsToLookUp(1) & "|" & commonEvo.ReturnAmodIDForItemIndex(commonEvo.FindIndexForFirstItem(UCase(ModelsToLookUp(1)), crmWebClient.Constants.AIRFRAME_MAKE))
                            End If

                            IDToSendBack += "##" & ModelsToLookUp(0)
                        End If
                        LastModel = ModelsToLookUp(1)
                    Next
                Else
                    Dim ModelsToLookUp As Array = Split(restofArray(0), "|")
                    ModelToSendBack = "cboAircraftMakeID=" & ModelsToLookUp(1) & "|" & commonEvo.ReturnAmodIDForItemIndex(commonEvo.FindIndexForFirstItem(UCase(ModelsToLookUp(1)), crmWebClient.Constants.AIRFRAME_MAKE))
                    IDToSendBack = "cboAircraftModelID=" & ModelsToLookUp(0)

                End If

                dataIn = Replace(dataIn, StrModel, ModelToSendBack & "!~!" & IDToSendBack)

            End If

            Return dataIn
        End Function
        Public Shared Sub CreateExcelButton(ByRef ExcelButton As String, ByVal PanelName As String)
            Dim PlaceholderString As String = ""

            ExcelButton = "var panel = document.getElementById(""" & PanelName & """);"
            ExcelButton += "my_form = document.createElement('FORM');"
            ExcelButton += "my_form.name = 'myForm';"
            ExcelButton += "my_form.method = 'POST';"
            ExcelButton += "my_form.action = 'MacShell.aspx';"
            ExcelButton += "my_form.target = '_new';"
            ExcelButton += " my_tb = document.createElement('INPUT');"
            ExcelButton += "my_tb.type = 'HIDDEN';"
            ExcelButton += "my_tb.name = 'MacExport';"
            ExcelButton += "my_tb.value = true;"
            ExcelButton += "my_form.appendChild(my_tb);"

            ExcelButton += " my_tb = document.createElement('INPUT');"
            ExcelButton += "my_tb.type = 'HIDDEN';"
            ExcelButton += "my_tb.name = 'data';"
            ExcelButton += "my_tb.value = escape(panel.innerHTML);"
            ExcelButton += "my_form.appendChild(my_tb);"
            ExcelButton += " document.body.appendChild(my_form);"
            ExcelButton += "  my_form.submit();"



            If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmPlatformOS) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmPlatformOS) Then
                    If InStr(HttpContext.Current.Session.Item("localUser").crmPlatformOS, "mac") > 0 Then
                        PlaceholderString += ", { text:'Excel', "
                        PlaceholderString += " action: function( e, dt, node, config) {" & ExcelButton & "}},"
                    Else
                        PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
                    End If
                Else
                    PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
                End If
            Else
                PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
            End If
            ExcelButton = PlaceholderString
        End Sub
        'Small Function used in data functions to build the debug text.
        Public Shared Sub Build_Debug_Text(ByVal queryCall As String, ByVal ClassName As String, ByVal sql As String)
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><span class='red_text'>" & Now.ToString & "</span>: <b class='blue_text'>" & queryCall & "</b> <span class=""green size_11"">(" & Replace(ClassName, "crmWebClient.", "") & ")</span><br />" & sql
        End Sub
        'task = the customer requested that they have the ability to enter/update dollar values using a $ and an M - the dollor sign we could just strip off, where them M is meant to indicate millions so we would essentially have to translate the entry. I think that they want to enter something like $4.334M - if they did then this would be acceptable. - while we are doing this we might as well allow a K as well where we would replace with 3 0''s 
        Public Shared Function FormatMKDollarValue(ByVal x As Object) As String
            FormatMKDollarValue = ""
            Try
                If Not IsDBNull(x) Then
                    If InStr(x, "K") > 0 Then
                        x = Replace(x, "K", "000")
                    ElseIf InStr(x, "M") > 0 Then
                        If InStr(x, ".") > 0 Then
                            Dim len = Split(x, ".")
                            Dim length As Integer = len(1).Length
                            Dim zeroes As Integer = 6 - length
                            x = Replace(x, ".", ",")
                            Dim ending As String = ""
                            For i = 0 To zeroes
                                ending = ending & "0"
                            Next
                            x = Replace(x, "M", ending)
                        Else
                            x = Replace(x, "M", "000,000")
                        End If
                    End If
                End If
                FormatMKDollarValue = x
            Catch ex As Exception

            End Try
        End Function

        Public Shared Function Format_Currency(ByVal x As Object) As String
            Format_Currency = ""
            x = x.ToString
            If Not IsDBNull(x) Then
                If x <> "" Then
                    Format_Currency = "$" & FormatNumber(CDbl(x), 2)
                End If
            End If
        End Function
        'This Takes the Percentage 
        Public Shared Function showpercent(ByVal x As String, ByVal y As String)
            showpercent = ""
            Try
                'Displays co-owner or fractional owner percentage.
                If y = "Co-Owner" Or y = "Fractional Owner" Then
                    showpercent = " - " & FormatNumber(x, 2) & "%"
                Else
                    showpercent = ""
                End If
            Catch ex As Exception
                'error_string = "main_site.Master.vb - showpercent() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function

        Public Shared Sub Year_Range_DropDownFill(ByVal year As DropDownList, ByVal start_ As Integer, ByVal end_ As Integer)
            year.Items.Add(New ListItem("All", ""))
            For i As Integer = end_ To start_ Step -1
                year.Items.Add(New ListItem(i, i))
            Next
            year.SelectedValue = ""
        End Sub
        Public Shared Function Show_Document_AC_Listing(ByVal x As Object) As String
            Show_Document_AC_Listing = ""
            If Not IsDBNull(x) Then
                If x = "Y" Then
                    Show_Document_AC_Listing = "<img src='images/final.jpg' alt='' border='0' />&nbsp;&nbsp;"
                End If
            End If
        End Function
        Public Shared Function trans_date_diff(ByVal x As Object, ByVal y As Object, ByVal z As Integer) As String
            trans_date_diff = ""
            Try
                Dim answer As String = ""
                If Not IsDBNull(x) And Not IsDBNull(y) Then
                    answer = CStr(FormatDateTime(CDate(y.ToString), DateFormat.ShortDate))
                    If answer = "1/1/0001" Or answer = "1/1/1900" Then
                        answer = ""
                    End If
                    If answer <> "" Then
                        If z = 1 Then
                            trans_date_diff = DateDiff(DateInterval.Day, y, x) & " Days"
                        Else
                            trans_date_diff = "DOM: " & DateDiff(DateInterval.Day, y, x)
                        End If
                    Else
                        trans_date_diff = ""
                    End If
                Else
                    trans_date_diff = ""
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - trans_date_diff() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        Public Shared Function Category_Belongs_To(ByVal x As Integer, ByVal reverse As Boolean, ByVal y As String, ByVal temptable As DataTable)
            Category_Belongs_To = x
            Try
                If Not IsDBNull(x) Then

                    If reverse <> True Then
                        If Not IsNothing(temptable) Then
                            If temptable.Rows.Count > 0 Then
                                For Each R As DataRow In temptable.Rows
                                    'notecat_key, notecat_name
                                    If x = R("notecat_key") Then
                                        Category_Belongs_To = R("notecat_name")
                                    End If
                                Next
                            Else
                            End If
                        End If
                    Else
                        If Not IsNothing(temptable) Then
                            If temptable.Rows.Count > 0 Then
                                For Each R As DataRow In temptable.Rows
                                    'notecat_key, notecat_name
                                    If UCase(y) = UCase(R("notecat_name")) Then
                                        Category_Belongs_To = R("notecat_key")
                                    End If
                                Next
                            Else
                            End If
                        End If
                    End If
                End If

                Category_Belongs_To = UCase(Category_Belongs_To)
            Catch ex As Exception
                Category_Belongs_To = ""
            End Try

        End Function
        'This double checks and makes sure the date isn't a bad date/null date. We input 1/1/0001 or 1/1/1900 if the date is null in the adapters. 
        Public Shared Function datenull(ByVal x As Object, Optional ByVal GeneralDate As Boolean = False) As String
            datenull = ""
            Try
                Dim answer As String = ""
                If Not IsDBNull(x) Then
                    If IsDate(x) Then
                        If CStr(x) <> "" Then
                            If GeneralDate Then
                                answer = CStr(FormatDateTime(CDate(x.ToString), DateFormat.GeneralDate))
                            Else
                                answer = CStr(FormatDateTime(CDate(x.ToString), DateFormat.ShortDate))
                            End If

                            If answer = "1/1/0001" Or answer = "1/1/1900" Then
                                answer = ""
                            End If
                            datenull = answer
                        End If
                    Else
                        datenull = answer
                    End If
                Else
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - datenull() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'This function formats the price to the 100k prices
        Public Shared Function no_zero(ByVal x As Object, ByVal y As Object, ByVal format As Boolean) As String
            no_zero = ""
            Try
                If Not IsDBNull(x) Then
                    If x.ToString <> "0" And x.ToString <> "0.00" Then
                        If Left(x.ToString, 1) <> "0" And x.ToString <> "" Then
                            Dim integered As Double = CDbl(x.ToString)
                            Dim following As String = ""
                            If Not IsDBNull(y) Then
                                following = CStr(y)
                            End If
                            If following = "Price" Then
                                following = ""
                            End If
                            If format = True Then

                                no_zero = "$" & FormatNumber(integered, 0)

                                'integered = integered / 1000
                                'If integered <> 0 Then
                                '  no_zero = "$" & integered.ToString("#,##0") & "k" & IIf(following <> "", " " & following, "")
                                'Else
                                '  ' no_zero = integered & " " & following
                                'End If


                            Else
                                no_zero = "$" & integered & " " & following
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - no_zero() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function

        Public Shared Function no_zero_sold(ByVal x As Object, ByVal y As Object, ByVal format As Boolean, ByVal source As String, ByVal type_of As String, ByVal asking_status As String) As String
            no_zero_sold = ""
            Try
                If Not IsDBNull(x) Then
                    If x.ToString <> "0" And x.ToString <> "0.00" Then
                        If Left(x.ToString, 1) <> "0" And x.ToString <> "" Then
                            Dim integered As Double = CDbl(x.ToString)
                            Dim following As String = ""
                            If Not IsDBNull(y) Then
                                following = CStr(y)
                            End If
                            If following = "Price" Then
                                following = ""
                            End If
                            If format = True Then


                                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                    If Trim(source) = "JETNET" Then
                                        If Trim(type_of) = "Sold" Then
                                            no_zero_sold = "<A href='' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'><p unselectable='on' style='display:inline'>"
                                        ElseIf Trim(type_of) = "Asking" And Trim(asking_status) = "Make Offer" Or Trim(asking_status) = "" Then
                                            no_zero_sold = "<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'><p unselectable='on' style='display:inline'>"
                                        End If
                                    End If
                                End If

                                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                    integered = (integered / 1000)
                                    'no_zero_sold &= "$" & FormatNumber(integered, 0) & ""

                                    If Trim(source) = "JETNET" And Trim(type_of) = "Asking" And Trim(asking_status) = "Make Offer" Or Trim(asking_status) = "" Then
                                        no_zero_sold &= DisplayFunctions.TextToImage(integered, 7, "", "40", "Reported Sale Price Displayed with Permission from Source")
                                    ElseIf Trim(source) = "JETNET" And Trim(type_of) = "Sold" Then
                                        no_zero_sold &= DisplayFunctions.TextToImage(integered, 7, "", "40", "Reported Sale Price Displayed with Permission from Source")
                                    Else
                                        no_zero_sold &= "$" & FormatNumber(integered, 0) & ""
                                    End If


                                ElseIf Trim(source) = "JETNET" And Trim(type_of) = "Asking" Then
                                    ' make sure
                                    If Trim(asking_status) = "Make Offer" Or Trim(asking_status) = "" Then
                                        'integered = (integered / 1000)
                                        'no_zero_sold &= DisplayFunctions.TextToImage(integered, 7, "", "40", "Reported Asking Price Displayed with Permission from Source")
                                    Else
                                        integered = (integered / 1000)
                                        no_zero_sold &= "$" & FormatNumber(integered, 0) & ""
                                        ' no_zero_sold &= DisplayFunctions.TextToImage(integered, 7, "", "40", "Reported Asking Price Displayed with Permission from Source")
                                    End If
                                Else
                                    integered = (integered / 1000)
                                    no_zero_sold &= "$" & FormatNumber(integered, 0) & ""
                                End If


                                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                    If Trim(source) = "JETNET" Then
                                        If Trim(type_of) = "Sold" Then
                                            no_zero_sold &= "</p></a>"
                                        ElseIf Trim(type_of) = "Asking" And Trim(asking_status) = "Make Offer" Or Trim(asking_status) = "" Then
                                            no_zero_sold &= "</p></a>"
                                        End If
                                    End If
                                End If
                                'integered = integered / 1000
                                'If integered <> 0 Then
                                '  no_zero = "$" & integered.ToString("#,##0") & "k" & IIf(following <> "", " " & following, "")
                                'Else
                                '  ' no_zero = integered & " " & following
                                'End If


                            Else
                                no_zero_sold = "$" & integered & " " & following
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - no_zero() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function


        Public Shared Function ConvertIntoThousands(ByVal priceValue As Object) As String
            Dim returnString As String = ""
            Dim Jetnet As Boolean = False
            Dim Client As Boolean = False
            If Not IsDBNull(priceValue) Then

                If InStr(priceValue, "<span class='client_row'>") > 0 Then
                    Client = True
                ElseIf InStr(priceValue, "<span class='jetnet_row'>") > 0 Then
                    Jetnet = True
                End If

                priceValue = Replace(priceValue, "<span class='client_row'>", "")
                priceValue = Replace(priceValue, "<span class='jetnet_row'>", "")
                priceValue = Replace(priceValue, "</span>", "")

                If IsNumeric(priceValue) Then
                    If priceValue > 0 Then
                        priceValue = priceValue / 1000
                        If priceValue <> 0 Then
                            returnString = "$" & FormatNumber(priceValue, 0) & "k"
                        Else
                            returnString = priceValue
                        End If
                    End If
                End If

                If Client Then
                    returnString = "<span class='client_row'>" & returnString & "</span>"
                ElseIf Jetnet Then
                    returnString = "<span class='jetnet_row'>" & returnString & "</span>"
                End If
            End If


            Return returnString
        End Function
        'This strips illegal characters in the searches. 
        Public Shared Function StripChars(ByVal text As String, ByVal backslash As Boolean) As String
            Dim illegalChars As Char() = "$^{}[]""+<>?'".ToCharArray()
            Dim str As String = ""
            Dim first As New System.Text.StringBuilder

            Try
                If Not IsNothing(text) Then
                    str = text
                    For Each ch As Char In str
                        If Array.IndexOf(illegalChars, ch) = -1 Then
                            first.Append(ch)
                        Else
                            'If backslash = True Then
                            'first.Append("\" & ch)
                            'Else
                            first.Append("")
                            ' End If
                        End If
                    Next
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - StripChars() - " & ex.Message
                'LogError(error_string)
            End Try
            Return first.ToString
        End Function

        ''''WORK IN PROGRESS
        'This strips illegal characters in the folder names. Slightly different than the search function above.
        Public Shared Function PrepFolderNameForSave(ByVal text As String, ByVal backslash As Boolean) As String
            Dim illegalChars As Char() = "$^(){}[]""<>?'".ToCharArray()
            Dim str As String = ""
            Dim first As New System.Text.StringBuilder

            Try
                If Not IsNothing(text) Then
                    str = text
                    For Each ch As Char In str
                        If Array.IndexOf(illegalChars, ch) = -1 Then
                            first.Append(ch)
                        Else
                            'If backslash = True Then
                            'first.Append("\" & ch)
                            'Else
                            first.Append("")
                            ' End If
                        End If
                    Next
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - StripChars() - " & ex.Message
                'LogError(error_string)
            End Try
            Return first.ToString
        End Function


        Public Shared Function Build_Operating_Costs(ByVal TempTable As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal currencyExchangeRate As Double, ByVal FuelBaseRate As Double, ByVal ShowModelName As Boolean, ByVal ShowDirectCost As Boolean, ByVal ShowAnnualFixed As Boolean, ByVal ShowAnnualBudget As Boolean, ByVal ShowSubHeaders As Boolean, ByVal UseMetric As Boolean) As String

            'Dim UseMetric As Boolean = True
            Dim CurrencySymbol As String = "$"
            Dim str As String = ""
            Dim Direct_Cost_Per_Header As String = ""
            Dim Direct_Cost_Per_Text As String = ""
            Dim ModelNameHeader As String = ""
            Dim ModelNameText As String = ""

            'Fuel Block
            Dim Fuel_Header As String = ""
            Dim Fuel_Text As String = ""
            Dim Fuel_Cost_Per_Gallon_Header As String = ""
            Dim Fuel_Cost_Per_Gallon_Text As String = ""
            Dim Fuel_Burn_Rate_Header As String = ""
            Dim Fuel_Burn_Rate_Text As String = ""

            'Maintenance Block
            Dim Maintenance_Header As String = ""
            Dim Maintenance_Text As String = ""
            Dim Maintenance_Labor_Cost_Header As String = ""
            Dim Maintenance_Labor_Cost_Text As String = ""
            Dim Maintenance_Parts_Per_Header As String = ""
            Dim Maintenance_Parts_Per_Text As String = ""
            Dim Maintenance_Engine_Overhaul_Header As String = ""
            Dim Maintenance_Engine_Overhaul_Text As String = ""
            Dim Maintenance_Thrust_Reverse_Header As String = ""
            Dim Maintenance_Thrust_Reverse_Text As String = ""

            'Misc Block
            Dim Misc_Header As String = ""
            Dim Misc_Text As String = ""
            Dim Misc_Landing_Fee_Header As String = ""
            Dim Misc_Landing_Fee_Text As String = ""
            Dim Misc_Crew_Expenses_Header As String = ""
            Dim Misc_Crew_Expenses_Text As String = ""
            Dim Misc_Supplies_Header As String = ""
            Dim Misc_Supplies_Text As String = ""

            Dim Total_Direct_Costs_Header As String = ""
            Dim Total_Direct_Costs_Text As String = ""
            Dim Block_Speed_Statute_Header As String = ""
            Dim Block_Speed_Statute_Text As String = ""
            Dim Total_Cost_Per_Statute_Header As String = ""
            Dim Total_Cost_Per_Statute_Text As String = ""

            Dim Annual_Header As String = ""

            'Crew Salary Block
            Dim Crew_Salaries_Header As String = ""
            Dim Crew_Salaries_Text As String = ""
            Dim Cap_Salary_Header As String = ""
            Dim Cap_Salary_Text As String = ""
            Dim Co_Salary_Header As String = ""
            Dim Co_Salary_Text As String = ""
            Dim Benefits_Header As String = ""
            Dim Benefits_Text As String = ""
            Dim Hangar_Header As String = ""
            Dim Hangar_Text As String = ""

            'Insurance Block
            Dim Insurance_Header As String = ""
            Dim Insurance_Text As String = ""
            Dim Hull_Header As String = ""
            Dim Hull_Text As String = ""
            Dim Legal_Liability_Header As String = ""
            Dim Legal_Liability_Text As String = ""

            'Misc. Overhead
            Dim Misc_Overhead_Header As String = ""
            Dim Misc_Overhead_Text As String = ""
            Dim Training_Header As String = ""
            Dim Training_Text As String = ""
            Dim Modernization_Header As String = ""
            Dim Modernization_Text As String = ""
            Dim Nav_Equipment_Header As String = ""
            Dim Nav_Equipment_Text As String = ""
            Dim Depreciation_Header As String = ""
            Dim Depreciation_Text As String = ""
            Dim Total_Fixed_Header As String = ""
            Dim Total_Fixed_Text As String = ""

            Dim Budget_Header As String = ""
            Dim Number_Seats_Header As String = ""
            Dim Number_Seats_Text As String = ""
            Dim Miles_Header As String = ""
            Dim Miles_Text As String = ""
            Dim Hours_Header As String = ""
            Dim Hours_Text As String = ""

            Dim Total_Direct_Header As String = ""
            Dim Total_Direct_Text As String = ""
            Dim Total_Fixed_Cost_Header As String = ""
            Dim Total_Fixed_Cost_Text As String = ""
            Dim Total_Fixed_And_Direct_Header As String = ""
            Dim Total_Fixed_And_Direct_Text As String = ""

            'Total Cost Fixed and Direct Block
            Dim Cost_Hour_Header As String = ""
            Dim Cost_Hour_Text As String = ""
            Dim Cost_Statute_Header As String = ""
            Dim Cost_Statute_Text As String = ""
            Dim Cost_Seat_Header As String = ""
            Dim Cost_Seat_Text As String = ""

            'Total Cost No Depreciation
            Dim Deprec_Header As String = ""
            Dim Deprec_Text As String = ""
            Dim Deprec_Cost_Hour_Header As String = ""
            Dim Deprec_Cost_Hour_Text As String = ""
            Dim Deprec_Cost_Statute_Header As String = ""
            Dim Deprec_Cost_Statute_Text As String = ""
            Dim Deprec_Cost_Seat_Mile_Header As String = ""
            Dim Deprec_Cost_Seat_Mile_Text As String = ""
            Dim add_make_model_list As Boolean = False

            Direct_Cost_Per_Header = "<td align='left' valign='top'><b class='title'>Direct Costs Per Hour (US Standard)</b></td>"
            ModelNameHeader = "<td align='left' valign='top'><b class='title'>Model Name</b></td>"


            'Fuel Block
            Fuel_Header = "<td align='left' valign='top'><b class='title'>Fuel</b></td>"
            Fuel_Cost_Per_Gallon_Header = "<td align='left' valign='top'>Fuel Cost Per " & IIf(UseMetric = True, ConversionFunctions.TranslateUSMetricUnitsLong("GAL"), "Gallon") & "</td>"
            Fuel_Burn_Rate_Text = "<td align='left' valign='top'>Burn Rate (" & IIf(UseMetric = True, ConversionFunctions.TranslateUSMetricUnitsLong("GAL"), "Gallon") & " Per Hour)</td>"

            'Maintenance Block
            Maintenance_Header = "<td align='left' valign='top'><b class='title'>Maintenance</b></td>"
            Maintenance_Labor_Cost_Header = "<td align='left' valign='top'>Labor Cost Per Hour</td>"
            Maintenance_Parts_Per_Header = "<td align='left' valign='top'> Parts Per Hour Cost</td>"
            Maintenance_Engine_Overhaul_Header = "<td align='left' valign='top'>Engine Overhaul</td>"
            Maintenance_Thrust_Reverse_Header = "<td align='left' valign='top'>Thrust Reverse Overhaul</td>"

            'Misc Block
            Misc_Header = "<td align='left' valign='top'><b class='title'>Miscellaneous Flight Expenses</b></td>"
            Misc_Landing_Fee_Header = "<td align='left' valign='top'>Landing-Parking Fee</td>"
            Misc_Crew_Expenses_Header = "<td align='left' valign='top'>Crew Expenses</td>"
            Misc_Supplies_Header = "<td align='left' valign='top'>Supplies-Catering</td>"

            Total_Direct_Costs_Header = "<td align='left' valign='top'>Total Direct Costs</td>"
            Block_Speed_Statute_Header = "<td align='left' valign='top'>Block Speed Statute Miles Per Hour</td>"
            Total_Cost_Per_Statute_Header = "<td align='left' valign='top'>Total Cost Per Statute Mile</td>"

            Annual_Header = "<td align='left' valign='top' colspan='" & TempTable.Rows.Count + 1 & "'><b class='title'>ANNUAL FIXED COSTS (US Standard)</b></td>"

            'Crew Salary Block
            Crew_Salaries_Header = "<td align='left' valign='top'><b class='title'>Crew Salaries</b></td>"
            Cap_Salary_Header = "<td align='left' valign='top'>Capt. Salary</td>"
            Co_Salary_Header = "<td align='left' valign='top'>Co-pilot. Salary</td>"
            Benefits_Header = "<td align='left' valign='top'>Benefits</td>"
            Hangar_Header = "<td align='left' valign='top'>Hangar Cost</td>"

            'Insurance Block
            Insurance_Header = "<td align='left' valign='top'><b class='title'>Insurance</b></td>"
            Hull_Header = "<td align='left' valign='top'>Hull</td>"
            Legal_Liability_Header = "<td align='left' valign='top'>Legal Liability</td>"

            'Misc. Overhead
            Misc_Overhead_Header = "<td align='left' valign='top'><b class='title'>Misc. Overhead</b></td>"
            Training_Header = "<td align='left' valign='top'>Training</td>"
            Modernization_Header = "<td align='left' valign='top'>Modernization</td>"
            Nav_Equipment_Header = "<td align='left' valign='top'>Nav. Equipment</td>"
            Depreciation_Header = "<td align='left' valign='top'>Depreciation</td>"
            Total_Fixed_Header = "<td align='left' valign='top'>Total Fixed Costs</td>"



            Budget_Header = "<td align='left' valign='top' colspan='" & TempTable.Rows.Count + 1 & "'><b class='title'>ANNUAL BUDGET (US Standard)</b></td>"

            Number_Seats_Header = "<td align='left' valign='top'>Number of Seats</td>"
            Miles_Header = "<td align='left' valign='top'>Miles</td>"
            Hours_Header = "<td align='left' valign='top'>Hours</td>"

            Total_Direct_Header = "<td align='left' valign='top'>Total Direct Costs</td>"
            Total_Fixed_Cost_Header = "<td align='left' valign='top'>Total Fixed Costs</td>"

            Total_Fixed_And_Direct_Header = "<td align='left' valign='top' nowrap='nowrap'><b class='title'>Total Cost (Fixed &amp; Direct w/Depreciation)</b></td>"

            'Total Cost Fixed and Direct Block
            Cost_Hour_Header = "<td align='left' valign='top'>Cost/Hour</td>"
            Cost_Statute_Header = "<td align='left' valign='top'>Cost/Statute Mile</td>"
            Cost_Seat_Header = "<td align='left' valign='top'>Cost/Seat Mile</td>"


            'Total Cost No Depreciation
            Deprec_Header = "<td align='left' valign='top'><b class='title'>Total Cost (No Depreciation)</b></b></td>"
            Deprec_Cost_Hour_Header = "<td align='left' valign='top'>Cost/Hour</td>"
            Deprec_Cost_Statute_Header = "<td align='left' valign='top'>Cost/Statute Mile</td>"
            Deprec_Cost_Seat_Mile_Header = "<td align='left' valign='top'>  Cost/Seat Mile</td>"

            If HttpContext.Current.Session.Item("OpCostsModelID") = 0 And HttpContext.Current.Session.Item("OpCostsModelList").ToString.Trim = "" Then
                add_make_model_list = True
            End If

            For Each r As DataRow In TempTable.Rows

                If add_make_model_list Then
                    If Trim(HttpContext.Current.Session.Item("OpCostsModelList")) <> "" Then
                        HttpContext.Current.Session.Item("OpCostsModelList") = HttpContext.Current.Session.Item("OpCostsModelList") & ","
                    End If
                    HttpContext.Current.Session.Item("OpCostsModelList") = HttpContext.Current.Session.Item("OpCostsModelList") & r("amod_id").ToString
                End If


                'Model Name Text
                ModelNameText += "<td align='right' valign='top'>" & r("amod_make_name").ToString & " <a href=""#"" " & DisplayFunctions.WriteModelLink(r("amod_id"), "", False) & ">" & r("amod_model_name").ToString & "</a></td>"
                Direct_Cost_Per_Text += "<td align='right' valign='top'><a href=""#"" class='float_right emphasis_text tiny_text' " & DisplayFunctions.WriteModelLink(r("amod_id"), "", False) & " class='emphasis_text'>Know More</a></td>"

                '----Fuel Block
                Dim FuelAddCost As Double = 0
                Dim FuelTotCost As Double = 0
                Dim FuelBurnRate As Double = 0
                Dim FuelGalCost As Double = 0

                If UseMetric Then
                    'Fuel Gallon Cost
                    If Not IsDBNull(r("amod_fuel_gal_cost")) And CDbl(FuelBaseRate) = 0 Then
                        If CDbl(r("amod_fuel_gal_cost")) Then
                            FuelGalCost = ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(r("amod_fuel_gal_cost")))
                        End If
                    Else
                        If CDbl(FuelBaseRate) > 0 Then
                            FuelGalCost = ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(FuelBaseRate))
                        End If
                    End If
                    If CDbl(currencyExchangeRate) > 0 Then
                        FuelGalCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, FuelGalCost)
                    End If
                    FuelGalCost = System.Math.Round(FuelGalCost, 2)

                    'Fuel Additive
                    FuelAddCost = ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(r("amod_fuel_add_cost")))
                    If CDbl(currencyExchangeRate) > 0 Then
                        FuelAddCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, FuelAddCost)
                    End If
                    FuelAddCost = System.Math.Round(FuelAddCost, 2)

                    'Fuel Burn Rate
                    FuelBurnRate = ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(r("amod_fuel_burn_rate")))
                    FuelBurnRate = System.Math.Round(FuelBurnRate, 2)

                    'Fuel Total Cost.
                    FuelTotCost = CDbl((FuelGalCost + FuelAddCost) * FuelBurnRate)


                Else
                    'Fuel Gallon Cost
                    If Not IsDBNull(r("amod_fuel_gal_cost")) And CDbl(FuelBaseRate) = 0 Then
                        If CDbl(r("amod_fuel_gal_cost")) Then
                            FuelGalCost = CDbl(r("amod_fuel_gal_cost"))
                        End If
                    Else
                        If CDbl(FuelBaseRate) > 0 Then
                            FuelGalCost = CDbl(FuelBaseRate)
                        End If
                    End If
                    If CDbl(currencyExchangeRate) > 0 Then
                        ' exchange rate should always be set ? why always change 
                        FuelGalCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, FuelGalCost)
                    End If
                    FuelGalCost = System.Math.Round(FuelGalCost, 2)

                    'Fuel Additive Cost
                    If Not IsDBNull(r("amod_fuel_add_cost")) Then
                        FuelAddCost = CDbl(r("amod_fuel_add_cost"))
                    Else
                        FuelAddCost = CDbl(0)
                    End If
                    If CDbl(currencyExchangeRate) > 0 Then
                        FuelAddCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, FuelAddCost)
                    End If
                    FuelAddCost = System.Math.Round(FuelAddCost, 2)

                    'Fuel Burn Rate
                    If Not IsDBNull(r("amod_fuel_burn_rate")) Then
                        FuelBurnRate = CDbl(r("amod_fuel_burn_rate"))
                    Else
                        FuelBurnRate = CDbl(0)
                    End If
                    FuelBurnRate = System.Math.Round(FuelBurnRate, 2)

                    'Fuel Total Cost
                    FuelTotCost = (FuelGalCost + FuelAddCost) * FuelBurnRate

                End If




                FuelTotCost = System.Math.Round(FuelTotCost, 2)

                Fuel_Text += "<td align='right' valign='top'>" & IIf(Not IsDBNull(r("amod_fuel_tot_cost")), CurrencySymbol & FormatNumber(FuelTotCost, 2, True, False, True), "") & "</td>"
                Fuel_Cost_Per_Gallon_Text += "<td align='right' valign='top'>" & IIf(Not IsDBNull(r("amod_fuel_gal_cost")), CurrencySymbol & FormatNumber(FuelGalCost, 2, True, False, True), "") & "</td>"
                Fuel_Burn_Rate_Text += "<td align='right' valign='top'>" & FuelBurnRate & "</td>"

                '----Maintenance Block
                Dim TotalMaintCost As Double = 0
                Dim MaintLabCost As Double = 0
                Dim MaintPartsCost As Double = 0
                Dim OverhaulCost As Double = 0
                Dim RevOverhaulCost As Double = 0

                'Total Maintenance Cost.
                If Not IsDBNull(r("amod_maint_tot_cost")) Then
                    TotalMaintCost = System.Math.Round(CDbl(r("amod_maint_lab_cost")), 2) + System.Math.Round(CDbl(r("amod_maint_parts_cost")), 2)
                    TotalMaintCost = System.Math.Round(TotalMaintCost, 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        TotalMaintCost = ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("currencyExchangeRate"), TotalMaintCost)
                        TotalMaintCost = System.Math.Round(TotalMaintCost, 2)
                    End If
                    Maintenance_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalMaintCost, 2, True, False, True) & "</td>"
                Else
                    Maintenance_Text += "<td align='right' valign='top'>&nbsp</td>"
                End If
                'Maintenance Labor Cost
                If Not IsDBNull(r("amod_maint_lab_cost")) Then
                    MaintLabCost = System.Math.Round(CDbl(r("amod_maint_lab_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        MaintLabCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, MaintLabCost)
                        MaintLabCost = System.Math.Round(MaintLabCost, 2)
                    End If
                    Maintenance_Labor_Cost_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(MaintLabCost, 2, True, False, True) & "</td>"
                Else
                    Maintenance_Labor_Cost_Text += "<td align='right' valign='top'>&nbsp</td>"
                End If
                'Maintenance Parts Per Cost
                If Not IsDBNull(r("amod_maint_parts_cost")) Then
                    MaintPartsCost = System.Math.Round(CDbl(r("amod_maint_parts_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        MaintPartsCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, MaintPartsCost)
                        MaintPartsCost = System.Math.Round(MaintPartsCost, 2)
                    End If
                    Maintenance_Parts_Per_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(MaintPartsCost, 2, True, False, True) & "</td>"
                Else
                    Maintenance_Parts_Per_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Engine Overhaul Cost
                If Not IsDBNull(r("amod_engine_ovh_cost")) Then
                    OverhaulCost = System.Math.Round(CDbl(r("amod_engine_ovh_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        OverhaulCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, OverhaulCost)
                        OverhaulCost = System.Math.Round(OverhaulCost, 2)
                    End If

                    Maintenance_Engine_Overhaul_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(OverhaulCost, 2, True, False, True) & "</td>"
                Else
                    Maintenance_Engine_Overhaul_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Rev Overhaul Cost
                If Not IsDBNull(r("amod_thrust_rev_ovh_cost")) Then
                    RevOverhaulCost = System.Math.Round(CDbl(r("amod_thrust_rev_ovh_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        RevOverhaulCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, RevOverhaulCost)
                        RevOverhaulCost = System.Math.Round(RevOverhaulCost, 2)
                    End If
                    Maintenance_Thrust_Reverse_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(RevOverhaulCost, 2, True, False, True) & "</td>"
                Else
                    Maintenance_Thrust_Reverse_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If


                'Misc Block
                Dim MiscFlightCosts As Double = 0
                Dim LandParkCost As Double = 0
                Dim CrewExpCost As Double = 0
                Dim MiscSupplies As Double = 0
                Dim totalDirCostHR As Double = 0
                Dim avgBlockSpeed As Double = 0
                Dim TotalCostPer As Double = 0
                'Misc Total
                If Not IsDBNull(r("amod_misc_flight_cost")) Then
                    MiscFlightCosts = System.Math.Round(CDbl(r("amod_land_park_cost")), 2) + System.Math.Round(CDbl(r("amod_crew_exp_cost")), 2) + System.Math.Round(CDbl(r("amod_supplies_cost")), 2)
                    MiscFlightCosts = System.Math.Round(MiscFlightCosts, 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        MiscFlightCosts = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, MiscFlightCosts)
                        MiscFlightCosts = System.Math.Round(MiscFlightCosts, 2)
                    End If
                    Misc_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(MiscFlightCosts, 2, True, False, True) & "</td>"
                Else
                    Misc_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Landing Fee
                If Not IsDBNull(r("amod_land_park_cost")) Then
                    LandParkCost = System.Math.Round(CDbl(r("amod_land_park_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        LandParkCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, LandParkCost)
                        LandParkCost = System.Math.Round(LandParkCost, 2)
                    End If
                    Misc_Landing_Fee_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(LandParkCost, 2, True, False, True) & "</td>"
                Else
                    Misc_Landing_Fee_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Crew expenses cost.
                If Not IsDBNull(r("amod_crew_exp_cost")) Then
                    CrewExpCost = System.Math.Round(CDbl(r("amod_crew_exp_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        CrewExpCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, CrewExpCost)
                        CrewExpCost = System.Math.Round(CrewExpCost, 2)
                    End If
                    Misc_Crew_Expenses_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(CrewExpCost, 2, True, False, True) & "</td>"
                Else
                    Misc_Crew_Expenses_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Misc Supplies
                If Not IsDBNull(r("amod_supplies_cost")) Then
                    MiscSupplies = System.Math.Round(CDbl(r("amod_supplies_cost")), 2)
                    If CDbl(currencyExchangeRate) > 0 Then
                        MiscSupplies = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, MiscSupplies)
                        MiscSupplies = System.Math.Round(MiscSupplies, 2)
                    End If
                    Misc_Supplies_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(MiscSupplies, 2, True, False, True) & "</td>"
                Else
                    Misc_Supplies_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Total Direct Cost
                If Not IsDBNull(r("amod_tot_hour_direct_cost")) Then
                    totalDirCostHR = System.Math.Round(CDbl(FuelTotCost), 2) + TotalMaintCost + MiscFlightCosts + OverhaulCost + RevOverhaulCost
                    Total_Direct_Costs_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(totalDirCostHR, 2, True, False, True) & "</td>"
                Else
                    Total_Direct_Costs_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If
                'Average Block speed
                If Not IsDBNull(r("amod_avg_block_speed")) Then
                    If UseMetric Then
                        avgBlockSpeed = System.Math.Round(ConversionFunctions.ConvertUSToMetricValue("SM", CDbl(r("amod_avg_block_speed"))), 0)
                    Else
                        avgBlockSpeed = System.Math.Round(CDbl(r("amod_avg_block_speed")), 0)
                    End If
                    Block_Speed_Statute_Text += "<td align='right' valign='top'>" & FormatNumber(avgBlockSpeed, 0, True, False, True) & "</td>" & vbCrLf
                Else
                    Block_Speed_Statute_Text += "<td align='right' valign='top'>&nbsp;</td>" & vbCrLf
                End If

                'Total Cost per statute mile
                If totalDirCostHR > 0 And avgBlockSpeed > 0 Then
                    If HttpContext.Current.Session.Item("useMetricValues") Then
                        TotalCostPer = CDbl(CDbl(totalDirCostHR) / CDbl(avgBlockSpeed))
                    Else
                        TotalCostPer = CDbl(CDbl(totalDirCostHR) / CDbl(avgBlockSpeed))
                    End If

                    TotalCostPer = System.Math.Round(TotalCostPer, 2)
                Else
                    TotalCostPer = 0
                End If
                If Not IsDBNull(r("amod_tot_stat_mile_cost")) Then
                    Total_Cost_Per_Statute_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalCostPer, 2, True, False, True) & "</td>"
                Else
                    Total_Cost_Per_Statute_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                'Crew Salary Block
                Dim CrewSalary As Double = 0
                Dim CaptSalary As Double = 0
                Dim CoPilot As Double = 0
                Dim Benefits As Double = 0
                Dim Hangar As Double = 0
                If Not IsDBNull(r("amod_tot_crew_salary_cost")) Then
                    CrewSalary = System.Math.Round(CDbl(r("amod_capt_salary_cost")), 0) + System.Math.Round(CDbl(r("amod_cpilot_salary_cost")), 0) + System.Math.Round(CDbl(r("amod_crew_benefit_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        CrewSalary = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, CrewSalary)
                        CrewSalary = System.Math.Round(CrewSalary, 0)
                    End If
                    CrewSalary = System.Math.Round(CrewSalary, 0)
                    Crew_Salaries_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(CrewSalary, 0, True, False, True) & "</td>"
                Else
                    Crew_Salaries_Text += "<td align='right' valign='top'></td>"
                End If

                If Not IsDBNull(r("amod_capt_salary_cost")) Then
                    CaptSalary = System.Math.Round(CDbl(r("amod_capt_salary_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        CaptSalary = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, CaptSalary)
                        CaptSalary = System.Math.Round(CaptSalary, 0)
                    End If
                    Cap_Salary_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(CaptSalary, 0, True, False, True) & "</td>"
                Else
                    Cap_Salary_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_cpilot_salary_cost")) Then
                    CoPilot = System.Math.Round(CDbl(r("amod_cpilot_salary_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        CoPilot = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, CoPilot)
                        CoPilot = System.Math.Round(CoPilot, 0)
                    End If
                    Co_Salary_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(CoPilot, 0, True, False, True) & "</td>"
                Else
                    Co_Salary_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_crew_benefit_cost")) Then
                    Benefits = System.Math.Round(CDbl(r("amod_crew_benefit_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Benefits = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Benefits)
                        Benefits = System.Math.Round(Benefits, 0)
                    End If
                    Benefits_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Benefits, 0, True, False, True) & "</td>"
                Else
                    Benefits_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_hangar_cost")) Then
                    Hangar = System.Math.Round(CDbl(r("amod_hangar_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Hangar = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Hangar)
                    End If
                    Hangar = System.Math.Round(Hangar, 0)

                    Hangar_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Hangar, 0, True, False, True) & "</td>"
                Else
                    Hangar_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                'Insurance Block
                Dim Insurance As Double = 0
                Dim Hull As Double = 0
                Dim Liability As Double = 0
                If Not IsDBNull(r("amod_insurance_cost")) Then
                    Insurance = System.Math.Round(CDbl(r("amod_hull_insurance_cost")), 0) + System.Math.Round(CDbl(r("amod_liability_insurance_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Insurance = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Insurance)
                    End If
                    Insurance = System.Math.Round(Insurance, 0)
                    Insurance_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Insurance, 0, True, False, True) & "</td>"
                Else
                    Insurance_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If


                If Not IsDBNull(r("amod_hull_insurance_cost")) Then
                    Hull = System.Math.Round(CDbl(r("amod_hull_insurance_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Hull = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Hull)
                        Hull = System.Math.Round(Hull, 0)
                    End If
                    Hull_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Hull, 0, True, False, True) & "</td>"
                Else
                    Hull_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If


                If Not IsDBNull(r("amod_liability_insurance_cost")) Then
                    Liability = System.Math.Round(CDbl(r("amod_liability_insurance_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Liability = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Liability)
                    End If
                    Legal_Liability_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Liability, 0, True, False, True) & "</td>"
                Else
                    Legal_Liability_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If


                'Misc. Overhead
                Dim Misc As Double = 0
                Dim Training As Double = 0
                Dim Modern As Double = 0
                Dim Nav As Double = 0
                Dim Deprec As Double = 0
                If Not IsDBNull(r("amod_tot_misc_ovh_cost")) Then
                    Misc = System.Math.Round(CDbl(r("amod_misc_train_cost")), 0) + System.Math.Round(CDbl(r("amod_misc_modern_cost")), 0) + System.Math.Round(CDbl(r("amod_misc_naveq_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Misc = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Misc)
                        Misc = System.Math.Round(Misc, 0)
                    End If
                    Misc_Overhead_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Misc, 0, True, False, True) & "</td>"
                Else
                    Misc_Overhead_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_misc_train_cost")) Then
                    Training = System.Math.Round(CDbl(r("amod_misc_train_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Training = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Training)
                        Training = System.Math.Round(Training, 0)
                    End If
                    Training_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Training, 0, True, False, True) & "</td>"
                Else
                    Training_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_misc_modern_cost")) Then
                    Modern = System.Math.Round(CDbl(r("amod_misc_modern_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Modern = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Modern)
                        Modern = System.Math.Round(Modern, 0)
                    End If
                    Modernization_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Modern, 0, True, False, True) & "</td>"
                Else
                    Modernization_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_misc_naveq_cost")) Then
                    Nav = System.Math.Round(CDbl(r("amod_misc_naveq_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Nav = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Nav)
                        Nav = System.Math.Round(Nav, 0)
                    End If
                    Nav_Equipment_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Nav, 0, True, False, True) & "</td>"
                Else
                    Nav_Equipment_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_deprec_cost")) Then
                    Deprec = System.Math.Round(CDbl(r("amod_deprec_cost")), 0)
                    If CDbl(currencyExchangeRate) > 0 Then
                        Deprec = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, Deprec)
                    End If
                    Deprec = System.Math.Round(Deprec, 0)
                    Depreciation_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(Deprec, 0, True, False, True) & "</td>"
                Else
                    Depreciation_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                Dim TotalFixed As Double = 0
                Dim Annual As Double = 0
                Dim AnHours As Double = 0
                Dim TotalDirCost As Double = 0
                Dim TotalFixedCost As Double = 0
                Dim TotalFixedDirect As Double = 0

                If Not IsDBNull(r("amod_tot_fixed_cost")) Then
                    TotalFixed = CrewSalary + Hangar + Misc + Deprec + Insurance
                    TotalFixed = System.Math.Round(TotalFixed, 0)
                    Total_Fixed_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalFixed, 0, True, False, True) & "</td>"
                Else
                    Total_Fixed_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_number_of_seats")) Then
                    Number_Seats_Text += "<td align='right' valign='top'>" & FormatNumber(System.Math.Round(CDbl(r("amod_number_of_seats")), 0), 0, True, False, True) & "</td>"
                Else
                    Number_Seats_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_annual_miles")) Then
                    If UseMetric Then
                        Annual = ConversionFunctions.ConvertUSToMetricValue("M", CDbl(r("amod_annual_miles")))
                    Else
                        Annual = CDbl(r("amod_annual_miles"))
                    End If
                    Annual = System.Math.Round(Annual, 0)
                    Miles_Text += "<td align='right' valign='top'>" & FormatNumber(Annual, 0, True, False, True) & "</td>"
                Else
                    Miles_Text += ("<td align='right' valign='top'>&nbsp;</td>")
                End If

                If Not IsDBNull(r("amod_annual_hours")) And Annual > 0 And avgBlockSpeed > 0 Then
                    AnHours = CDbl(Annual) / CDbl(avgBlockSpeed)
                    AnHours = System.Math.Round(AnHours, 0)
                    Hours_Text += "<td align='right' valign='top'>" & FormatNumber(AnHours, 0, True, False, True) & "</td>"
                Else
                    Hours_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_tot_direct_cost")) Then
                    TotalDirCost = AnHours * totalDirCostHR
                    TotalDirCost = System.Math.Round(TotalDirCost, 0)
                    Total_Direct_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalDirCost, 0, True, False, True) & "</td>"
                Else
                    Total_Direct_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_tot_fixed_cost")) Then
                    TotalFixedCost = r("amod_tot_fixed_cost")
                    If CDbl(currencyExchangeRate) > 0 Then
                        TotalFixedCost = ConversionFunctions.ConvertUSToForeignCurrency(currencyExchangeRate, TotalFixedCost)
                    End If
                    Total_Fixed_Cost_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalFixedCost, 0, True, False, True) & "</td>"
                Else
                    Total_Fixed_Cost_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_tot_df_annual_cost")) Then
                    TotalFixedDirect = TotalDirCost + TotalFixedCost
                    Total_Fixed_And_Direct_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalFixedDirect, 0, True, False, True) & "</td>"
                Else
                    Total_Fixed_And_Direct_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                'Total Cost Fixed and Direct Block
                Dim DFHourCost As Double = 0
                Dim DFStatMile As Double = 0
                Dim DFSeatCost As Double = 0

                If Not IsDBNull(r("amod_tot_df_hour_cost")) And AnHours > 0 And TotalFixedDirect > 0 Then
                    If UseMetric Then
                        DFHourCost = TotalFixedDirect / AnHours
                    Else
                        DFHourCost = TotalFixedDirect / AnHours
                    End If
                    DFHourCost = System.Math.Round(DFHourCost, 0)
                    Cost_Hour_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(DFHourCost, 0, True, False, True) & "</td>"
                Else
                    Cost_Hour_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_tot_df_statmile_cost")) And Annual > 0 And TotalFixedDirect > 0 Then
                    If UseMetric Then
                        DFStatMile = TotalFixedDirect / Annual
                    Else
                        DFStatMile = TotalFixedDirect / Annual
                    End If
                    DFStatMile = System.Math.Round(DFStatMile, 2)
                    Cost_Statute_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(DFStatMile, 2, True, False, True) & "</td>"
                Else
                    Cost_Statute_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("amod_tot_df_seat_cost")) And DFStatMile > 0 And r("amod_number_of_seats") > 0 Then
                    DFSeatCost = DFStatMile / System.Math.Round(r("amod_number_of_seats"), 0)
                    DFSeatCost = System.Math.Round(DFSeatCost, 2)
                    Cost_Seat_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(DFSeatCost, 2, True, False, True) & "</td>"
                Else
                    Cost_Seat_Text += "<td align='right' valign='top'>&nbsp;</td>"
                End If


                'Total Cost No Depreciation
                Dim TotalCostNoDepreciation As Double = 0
                Dim ANCH As Double = 0
                Dim TCSM As Double = 0
                Dim TCST As Double = 0

                TotalCostNoDepreciation = TotalFixedDirect - Deprec
                TotalCostNoDepreciation = System.Math.Round(TotalCostNoDepreciation, 0)
                If TotalCostNoDepreciation > 0 Then
                    Deprec_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TotalCostNoDepreciation, 0, True, False, True) & "</td>"
                Else
                    Deprec_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                End If

                If Not IsDBNull(r("amod_annual_hours")) Then
                    If r("amod_annual_hours") > 0 Then
                        ANCH = TotalCostNoDepreciation / AnHours
                        ANCH = System.Math.Round(ANCH, 0)
                        Deprec_Cost_Hour_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(ANCH, 0, True, False, True) & "</td>"
                    Else
                        Deprec_Cost_Hour_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                    End If
                Else
                    Deprec_Cost_Hour_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                End If

                If Not IsDBNull(r("amod_annual_miles")) Then
                    If r("amod_annual_miles") > 0 Then
                        TCSM = TotalCostNoDepreciation / Annual
                        TCSM = System.Math.Round(TCSM, 3)

                        Deprec_Cost_Statute_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TCSM, 2, True, False, True) & "</td>"
                    Else
                        Deprec_Cost_Statute_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                    End If
                Else
                    Deprec_Cost_Statute_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                End If

                If Not IsDBNull(r("amod_number_of_seats")) Then
                    If r("amod_number_of_seats") > 0 Then
                        TCST = TCSM / System.Math.Round(r("amod_number_of_seats"), 0)
                        TCST = System.Math.Round(TCST, 2)

                        Deprec_Cost_Seat_Mile_Text += "<td align='right' valign='top'>" & CurrencySymbol & FormatNumber(TCST, 2, True, False, True) & "</td>"
                    Else
                        Deprec_Cost_Seat_Mile_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                    End If
                Else
                    Deprec_Cost_Seat_Mile_Text += "<td align='right' valign='top'>" & CurrencySymbol & "0.00</td>"
                End If

            Next

            str = "<table width='100%' cellspacing='3' cellpadding='3' class='data_aircraft_grid cell_right'>"

            If ShowModelName = True Then
                str += "<tr class='header_row'>"
                str += ModelNameHeader
                str += ModelNameText
                str += "</tr>"
            End If

            If ShowSubHeaders = True And ShowDirectCost = True Then
                str += "<tr class='header_row'>"
                str += Direct_Cost_Per_Header
                str += Direct_Cost_Per_Text
                str += "</tr>"
            End If




            If ShowDirectCost = True Then
                'Fuel Block
                str += "<tr class='seperator_row'>"
                str += Fuel_Header
                str += Fuel_Text
                str += "</tr>"
                str += "<tr>"
                str += Fuel_Cost_Per_Gallon_Header
                str += Fuel_Cost_Per_Gallon_Text
                str += "</tr>"
                str += "<tr>"
                str += Fuel_Burn_Rate_Header
                str += Fuel_Burn_Rate_Text
                str += "</tr>"


                'Maintenance Block
                str += "<tr class='seperator_row'>"
                str += Maintenance_Header
                str += Maintenance_Text
                str += "</tr>"
                str += "<tr>"
                str += Maintenance_Labor_Cost_Header
                str += Maintenance_Labor_Cost_Text
                str += "</tr>"
                str += "<tr>"
                str += Maintenance_Parts_Per_Header
                str += Maintenance_Parts_Per_Text
                str += "</tr>"
                str += "<tr>"
                str += Maintenance_Engine_Overhaul_Header
                str += Maintenance_Engine_Overhaul_Text
                str += "</tr>"
                str += "<tr>"
                str += Maintenance_Thrust_Reverse_Header
                str += Maintenance_Thrust_Reverse_Text
                str += "</tr>"

                'Misc Block
                str += "<tr class='seperator_row'>"
                str += Misc_Header
                str += Misc_Text
                str += "</tr>"
                str += "<tr>"
                str += Misc_Landing_Fee_Header
                str += Misc_Landing_Fee_Text
                str += "</tr>"
                str += "<tr>"
                str += Misc_Crew_Expenses_Header
                str += Misc_Crew_Expenses_Text
                str += "</tr>"
                str += "<tr>"
                str += Misc_Supplies_Header
                str += Misc_Supplies_Text
                str += "</tr>"
                str += "<tr>"
                str += Total_Direct_Costs_Header
                str += Total_Direct_Costs_Text
                str += "</tr>"
                str += "<tr>"
                str += Block_Speed_Statute_Header
                str += Block_Speed_Statute_Text
                str += "</tr>"
                str += "<tr>"
                str += Total_Cost_Per_Statute_Header
                str += Total_Cost_Per_Statute_Text
                str += "</tr>"

            End If

            If ShowAnnualFixed = True Then
                If ShowSubHeaders = True Then
                    str += "<tr class='header_row'>"
                    str += Annual_Header
                    str += "</tr>"
                End If


                'Crew Block
                str += "<tr class='seperator_row'>"
                str += Crew_Salaries_Header
                str += Crew_Salaries_Text
                str += "</tr>"
                str += "<tr>"
                str += Cap_Salary_Header
                str += Cap_Salary_Text
                str += "</tr>"
                str += "<tr>"
                str += Co_Salary_Header
                str += Co_Salary_Text
                str += "</tr>"
                str += "<tr>"
                str += Benefits_Header
                str += Benefits_Text
                str += "</tr>"
                str += "<tr>"
                str += Hangar_Header
                str += Hangar_Text
                str += "</tr>"

                'Insurance Block

                str += "<tr class='seperator_row'>"
                str += Insurance_Header
                str += Insurance_Text
                str += "</tr>"
                str += "<tr>"
                str += Hull_Header
                str += Hull_Text
                str += "</tr>"
                str += "<tr>"
                str += Legal_Liability_Header
                str += Legal_Liability_Text
                str += "</tr>"

                'Misc. Overhead
                str += "<tr class='seperator_row'>"
                str += Misc_Overhead_Header
                str += Misc_Overhead_Text
                str += "</tr>"
                str += "<tr>"
                str += Training_Header
                str += Training_Text
                str += "</tr>"
                str += "<tr>"
                str += Modernization_Header
                str += Modernization_Text
                str += "</tr>"
                str += "<tr>"
                str += Nav_Equipment_Header
                str += Nav_Equipment_Text
                str += "</tr>"
                str += "<tr>"
                str += Depreciation_Header
                str += Depreciation_Text
                str += "</tr>"
                str += "<tr class='seperator_row'>"
                str += Total_Fixed_Header
                str += Total_Fixed_Text
                str += "</tr>"
            End If

            If ShowAnnualBudget = True Then
                If ShowSubHeaders = True Then
                    str += "<tr class='header_row'>"
                    str += Budget_Header
                    str += "</tr>"
                End If

                str += "<tr>"
                str += Number_Seats_Header
                str += Number_Seats_Text
                str += "</tr>"
                str += "<tr>"
                str += Miles_Header
                str += Miles_Text
                str += "</tr>"
                str += "<tr>"
                str += Hours_Header
                str += Hours_Text
                str += "</tr>"
                str += "<tr>"
                str += Total_Direct_Header
                str += Total_Direct_Text
                str += "</tr>"
                str += "<tr>"
                str += Total_Fixed_Cost_Header
                str += Total_Fixed_Cost_Text
                str += "</tr>"

                'Total Cost Fixed and Direct Block
                str += "<tr class='seperator_row'>"
                str += Total_Fixed_And_Direct_Header
                str += Total_Fixed_And_Direct_Text
                str += "</tr>"

                str += "<tr>"
                str += Cost_Hour_Header
                str += Cost_Hour_Text
                str += "</tr>"
                str += "<tr>"
                str += Cost_Statute_Header
                str += Cost_Statute_Text
                str += "</tr>"
                str += "<tr>"
                str += Cost_Seat_Header
                str += Cost_Seat_Text
                str += "</tr>"

                'Total Cost No Depreciation
                str += "<tr class='seperator_row'>"
                str += Deprec_Header
                str += Deprec_Text
                str += "</tr>"
                str += "<tr>"
                str += Deprec_Cost_Hour_Header
                str += Deprec_Cost_Hour_Text
                str += "</tr>"
                str += "<tr>"
                str += Deprec_Cost_Statute_Header
                str += Deprec_Cost_Statute_Text
                str += "</tr>"
                str += "<tr>"
                str += Deprec_Cost_Seat_Mile_Header
                str += Deprec_Cost_Seat_Mile_Text
                str += "</tr>"
            End If

            str += "</table>"
            Return str
        End Function

        Public Shared Function ReturnASource(ByVal source As Object) As String
            Dim returnString As String = ""
            returnString = "<span class=""jetnet_row"">"
            If Not IsDBNull(source) Then
                If Not String.IsNullOrEmpty(source) Then
                    If source = "CLIENT" Then
                        returnString = "<span class=""client_row"">"
                    Else
                        returnString = "<span class=""jetnet_row"">"
                    End If
                End If
            End If
            Return returnString
        End Function
        Public Shared Function stripHTML(ByVal strHTML) As String
            'Strips the HTML tags from strHTML using split and join

            'Ensure that strHTML contains something
            If Len(strHTML) = 0 Then
                stripHTML = strHTML
                Exit Function
            End If

            Dim arysplit, i, j, strOutput

            arysplit = Split(strHTML, "<")

            'Assuming strHTML is nonempty, we want to start iterating
            'from the 2nd array postition
            If Len(arysplit(0)) > 0 Then j = 1 Else j = 0

            'Loop through each instance of the array
            For i = j To UBound(arysplit)
                'Do we find a matching > sign?
                If InStr(arysplit(i), ">") Then
                    'If so, snip out all the text between the start of the string
                    'and the > sign
                    arysplit(i) = Mid(arysplit(i), InStr(arysplit(i), ">") + 1)
                Else
                    'Ah, the < was was nonmatching
                    arysplit(i) = "<" & arysplit(i)
                End If
            Next

            'Rejoin the array into a single string
            strOutput = Join(arysplit, "")

            'Snip out the first <
            strOutput = Mid(strOutput, 2 - j)

            'Convert < and > to &lt; and &gt;
            strOutput = Replace(strOutput, ">", "&gt;")
            strOutput = Replace(strOutput, "<", "&lt;")

            stripHTML = strOutput
        End Function
        Public Shared Function Configure_Company_History_Documents(ByVal make As String, ByVal model As String, ByVal ser As String, ByVal ACID As Long, ByVal JournalID As Long, ByVal aclsDataTemp As clsData_Manager_SQL) As String
            Dim ReturnTable As New DataTable
            Dim DocumentFile As String = ""
            Dim returnString As String = ""
            Dim fDoctype_subdir_name As String = ""
            Dim fDoctype_file_extension As String = ""
            Dim fAdoc_journ_seq_no As Integer = 0
            ReturnTable = aclsDataTemp.Get_JETNET_TransactionDocuments(ACID, JournalID, 0)
            If Not IsNothing(ReturnTable) Then
                If ReturnTable.Rows.Count > 0 Then
                    For Each r As DataRow In ReturnTable.Rows
                        If r("adoc_hide_flag").ToString <> "Y" Then

                            If Not IsDBNull(r("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(r("adoc_journ_seq_no").ToString) Then
                                fAdoc_journ_seq_no = CInt(r("adoc_journ_seq_no").ToString.Trim)
                            Else
                                fAdoc_journ_seq_no = 0
                            End If
                            If Not IsDBNull(r("doctype_file_extension")) And Not String.IsNullOrEmpty(r("doctype_file_extension").ToString) Then
                                fDoctype_file_extension = r("doctype_file_extension").ToString.Trim
                            Else
                                fDoctype_file_extension = ""
                            End If

                            If Not IsDBNull(r("doctype_subdir_name")) And Not String.IsNullOrEmpty(r("doctype_subdir_name").ToString) Then
                                fDoctype_subdir_name = r("doctype_subdir_name").ToString.Trim
                            Else
                                fDoctype_subdir_name = ""
                            End If

                            DocumentFile = Get_Document_File_Name(ACID, JournalID, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension, HttpContext.Current.Application, HttpContext.Current.Session)

                            returnString += "<span class='li_document'><a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & make & "','" & model & "','" & ser & "'," & ACID.ToString & "," & JournalID.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & r("adoc_doc_type").ToString & "</a></span>"
                        End If
                    Next
                End If
            End If
            Return returnString
        End Function
        Public Shared Sub Fill_Wanteds_Tab(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal wanted_label As Label, ByVal wanted_dg As DataGrid, ByVal companyID As Integer, ByVal CompanySource As String, ByVal OtherID As Integer)
            Dim aTempTable As New DataTable
            Try

                aTempTable = aclsData_Temp.Return_Wanted(companyID, CompanySource, OtherID, "", "", "", "", "JC", 0)
                If Not IsNothing(aTempTable) Then
                    If (aTempTable.Rows.Count > 0) Then
                        wanted_dg.DataSource = aTempTable
                        If HttpContext.Current.Session.Item("localUser").crmEvo = True Then
                            '  wanted_dg.Columns(0).Visible = False
                        End If
                        wanted_dg.DataBind()
                    Else
                        wanted_dg.Visible = False
                        wanted_label.Text = "<p align='center'>No Current Wanted(s) for This Company.</p>"
                        'wanted_tab.Visible = False 'if no wanted, tab goes away
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, "Clsgeneral.vb - Fill_Wanteds_Tab() - " & aclsData_Temp.class_error, Now())
                    End If
                End If
            Catch ex As Exception
                aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, "Clsgeneral.vb - Fill_Wanteds_Tab() " & ex.Message, Now())
            End Try
        End Sub

        'This returns whether or not a jobseeker is active or inactive. 
        Public Shared Function active(ByVal x As String)
            active = ""
            Try
                'Displays whether or not the job seeker is active or inactive. 
                If x = "P" Then
                    active = "<span class='em' title='No'>&#10006;</span>"
                ElseIf x = "A" Then
                    active = "<span class='em' title='Yes'>&#10004;</span>"
                Else
                    active = "<span class='em' title='Inactive'>&mdash;</span>"
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - active() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'Shows a corresponding image based on whether the job seeker is a pilot or mechanic. 
        Public Shared Function pilot_mechanic(ByVal x As Object)
            'All this does is deal with whether the job seeker is a pilot or mechanic and shows corresponding image. 
            pilot_mechanic = ""
            Try
                If Not IsDBNull(x) Then
                    x = x.ToString
                    If x = "P" Then
                        pilot_mechanic = "<img src='images/pilot.png' alt='PILOT' width='20' align='center' />"
                    Else
                        pilot_mechanic = "<img src='images/wrench.png' alt='MECHANIC' align='center'/>"
                    End If
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - pilot_mechanic() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        '        'Used on listing page. Checks if a db field is null.
        Public Shared Function isitnull(ByVal x As Object) As String
            isitnull = ""
            Try
                If Not IsDBNull(x) Then
                    isitnull = x & " "
                Else
                    x = ""
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - isitnull() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'Function to color an aircraft with the for sale image.
        Public Shared Function colorme(ByVal x As Object) As String
            'Small function to color an ac with a for sale image. 
            colorme = ""
            Try
                If Not IsDBNull(x) Then
                    x = x.ToString
                    If UCase(x) = "Y" Then
                        colorme = "<img src='images/red_arrow.gif' alt='For Sale' width='25'/>"
                        Return colorme
                    Else
                        colorme = ""
                        Return colorme
                    End If
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - colorme() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'Function to color an aircraft with the lease image. 
        Public Shared Function colormelease(ByVal x As Object, ByVal client As Boolean) As String
            'Small function to color an ac with a lease image. 
            colormelease = ""
            Try
                If Not IsDBNull(x) Then
                    x = x.ToString
                    If UCase(x) = "Y" Then
                        If client = False Then
                            colormelease = "<img src='images/orange_arrow.gif' alt='Lease' width='25' />"
                        Else
                            colormelease = "<img src='images/orange_arrow.gif' alt='Lease' width='25'/>"
                        End If
                        Return colormelease
                    Else
                        colormelease = ""
                        Return colormelease
                    End If
                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - colormelease() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'This returns the flag based on what priority the note is. 
        Public Shared Function what_flag(ByVal x As Object) As String
            what_flag = ""
            Try
                If Not IsDBNull(x) Then
                    x = x.ToString
                    Select Case UCase(x)
                        Case "HIGH"
                            what_flag = x & "<br /><img src=""images/flag.gif"" alt=""Flag"" />"
                        Case "MEDIUM"
                            what_flag = x & "<br /><img src=""images/flag_medium.gif"" alt=""Flag"" />"
                        Case "LOW"
                            what_flag = x & "<br /><img src=""images/flag_low.gif"" alt=""Flag"" />"
                    End Select

                End If
            Catch ex As Exception
                'error_string = "clsGeneral.vb - what_flag() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        'This function sends back the exclusive flag image if its true.
        Public Shared Function colormeex(ByVal x As Object, ByVal client As Boolean) As String
            'Small function to color an ac with an exclusive image. 
            colormeex = ""
            Try
                If Not IsDBNull(x) Then
                    x = x.ToString
                    If UCase(x) = "Y" Then
                        If client = False Then
                            colormeex = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25' />"
                        Else
                            colormeex = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25' />"
                        End If
                        Return colormeex
                    Else
                        colormeex = ""
                        Return colormeex
                    End If
                End If
            Catch ex As Exception
                '    error_string = "main_site.Master.vb - colormeex() - " & ex.Message
                '    LogError(error_string)
            End Try
        End Function
        'This function sends back the exclusive flag image if its true.
        Public Shared Function colormeex_ac_listing(ByVal first As Object, ByVal second As Object, ByVal client As Boolean) As String
            'Small function to color an ac with an exclusive image. 
            colormeex_ac_listing = ""
            first = IIf(Not IsDBNull(first), first, "")
            second = IIf(Not IsDBNull(second), second, "")
            Try
                If UCase(first) = UCase(second) Then
                    If first = "Y" Then
                        colormeex_ac_listing = "<img src='images/exclusive_jetnet.gif' alt='Exclusive' width='14'/>"
                    End If
                    Return colormeex_ac_listing
                ElseIf UCase(first) <> UCase(second) Then
                    If first = "Y" Then
                        If second = "Y" Then
                            colormeex_ac_listing = "<img src='images/exclusive_jetnet.gif' alt='Exclusive' width='14' title='On Exclusive'/>"
                        ElseIf second = "N" Then
                            colormeex_ac_listing = "<img src='images/remove_exclusive.gif' alt='Exclusive' width='14' title='Removed From Exclusive'/>"
                        Else
                            colormeex_ac_listing = "<img src='images/exclusive_jetnet.gif' alt='Exclusive' width='14' title='On Exclusive'/>"
                        End If
                    ElseIf second = "Y" Then
                        colormeex_ac_listing = "<img src='images/exclusive_client.gif' alt='Exclusive' width='14' title='On Exclusive'/>"
                    End If

                Else
                    colormeex_ac_listing = ""
                    Return colormeex_ac_listing
                End If
            Catch ex As Exception
                '    error_string = "main_site.Master.vb - colormeex() - " & ex.Message
                '    LogError(error_string)
            End Try
        End Function
        Public Shared Function colormelease_ac_listing(ByVal first As Object, ByVal second As Object, ByVal client As Boolean) As String
            'Small function to color an ac with an exclusive image. 
            colormelease_ac_listing = ""
            first = IIf(Not IsDBNull(first), first, "")
            second = IIf(Not IsDBNull(second), second, "")
            Try
                If UCase(first) = UCase(second) Then
                    If first = "Y" Then
                        colormelease_ac_listing = "<img src='images/lease_jetnet.gif' alt='Exclusive' width='14' title='Leased'/>"
                    End If
                    Return colormelease_ac_listing
                ElseIf UCase(first) <> UCase(second) Then
                    If first = "Y" Then
                        If second = "Y" Then
                            colormelease_ac_listing = "<img src='images/lease_jetnet.gif' alt='Lease' width='14' title='Leased'/>"
                        ElseIf second = "N" Then
                            colormelease_ac_listing = "<img src='images/remove_lease.gif' alt='Lease' width='14' title='Removed From Leased'/>"
                        Else
                            colormelease_ac_listing = "<img src='images/lease_jetnet.gif' alt='Exclusive' width='14' title='Leased'/>"
                        End If
                    ElseIf second = "Y" Then
                        colormelease_ac_listing = "<img src='images/lease_client.gif' alt='Lease' title='Leased' width='14'/>"
                    End If

                Else
                    colormelease_ac_listing = ""
                    Return colormelease_ac_listing
                End If
            Catch ex As Exception
                '    error_string = "main_site.Master.vb - colormeex() - " & ex.Message
                '    LogError(error_string)
            End Try
        End Function
        'This will turn the text green if the AC status is for sale. 
        Public Shared Function colormestatus(ByVal x As Object, ByVal y As Object) As String
            colormestatus = ""
            Try
                If Not IsDBNull(x) And Not IsDBNull(y) Then
                    x = x.ToString
                    y = y.ToString
                    'This colors the AC status field. 
                    colormestatus = ""
                    If UCase(x) = "Y" Then
                        If InStr(UCase(y), "SALE PENDING") > 0 Then
                            colormestatus = "<span class='red'>" & y & "</span>"
                            Return colormestatus
                        Else
                            colormestatus = "<span class='green'>" & y & "</span>"
                            Return colormestatus
                        End If
                    Else
                        If InStr(y, "SALE PENDING") > 0 Then
                            colormestatus = "<span class='red'>" & y & "</span>"
                            Return colormestatus
                        Else
                            colormestatus = y
                            Return colormestatus
                        End If
                    End If
                End If
            Catch ex As Exception
                'error_string = "main_site.Master.vb - colormestatus() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        Public Shared Function difference_ac_listing_AskingPrice(ByVal status As Object, ByVal status_2 As Object, ByVal asking As Object, ByVal source As Object, ByVal asking_2 As Object, ByVal source_2 As Object, ByVal asking_wordage As Object, ByVal asking_wordage_2 As Object, ByVal css As String, ByVal title As String) As String
            'difference_ac_listing = ""
            Dim first_value As String = ""
            Dim second_value As String = ""

            asking = IIf(Not IsDBNull(asking), asking, "") 'asking price 1
            source = IIf(Not IsDBNull(source), source, "") 'asking price source 1

            asking_2 = IIf(Not IsDBNull(asking_2), asking_2, "") 'asking price 2
            source_2 = IIf(Not IsDBNull(source_2), source_2, "") 'asking price source 2

            asking_wordage = IIf(Not IsDBNull(asking_wordage), asking_wordage, "") 'asking wordage 1
            asking_wordage_2 = IIf(Not IsDBNull(asking_wordage_2), asking_wordage_2, "") 'asking wordage 2

            status = IIf(Not IsDBNull(status), status, "") 'status 1
            status_2 = IIf(Not IsDBNull(status_2), status_2, "") 'status 2

            'Here are the makeshift rules.
            'For figuring out what actually gets displayed in the asking price.
            'First we need to know if the value 1 is for sale.
            If status = "Y" Then
                'If this first value is for sale, then we basically need to figure out if the wordage is Price.
                If Trim(asking_wordage) = "Price" Then
                    'if the asking wordage is price, then the answer for the first value, is price.
                    first_value = asking
                ElseIf Trim(asking_wordage) = "Make Offer" Then
                    first_value = "<span title='MAKE OFFER' class='help_cursor'>MO</span>"
                Else
                    'if this is not price, then we need to return asking wordage
                    first_value = asking_wordage
                End If
            End If

            'Second we need to know if the value 2 is for sale.
            If status_2 = "Y" Then
                'If this first value is for sale, then we basically need to figure out if the wordage is price.
                If Trim(asking_wordage_2) = "Price" Then
                    'if this is price, return the asking price
                    second_value = asking_2
                ElseIf Trim(asking_wordage_2) = "Make Offer" Then
                    second_value = "<span title='MAKE OFFER' class='help_cursor'>MO</span>"
                Else
                    'if this is not price, return asking wordage
                    second_value = asking_wordage_2
                End If
            End If


            Dim starting As String = ""
            Dim ending As String = ""
            If HttpContext.Current.Session.Item("isMobile") = True Then
                starting = "<div class='" & css & "' align='right'><h3>" & title & "</h3> "
                ending = "</div>"
            End If

            If source = "" Then
                source = source_2
            ElseIf source_2 = "" Then
                source_2 = source
            End If

            If LCase(first_value) = LCase(second_value) Then
                second_value = ""
            Else
                second_value = "<span class='" & LCase(source_2) & "_row'>" & second_value & "</span>"
            End If

            If second_value = "" And first_value = "" Then
                starting = ""
                ending = ""
            End If

            first_value = "<span class='" & LCase(source) & "_row'>" & first_value & "</span>"
            difference_ac_listing_AskingPrice = starting & first_value & "<br />" & second_value & ending
        End Function

        Public Shared Function difference_ac_listing(ByVal year_one As Object, ByVal one_source As Object, ByVal year_two As Object, ByVal two_source As Object, ByVal css As String, ByVal title As String) As String
            difference_ac_listing = ""

            year_one = IIf(Not IsDBNull(year_one), year_one.ToString, "")
            year_two = IIf(Not IsDBNull(year_two), year_two.ToString, "")
            one_source = IIf(Not IsDBNull(one_source), one_source, "")
            two_source = IIf(Not IsDBNull(two_source), two_source, "")

            'year_one = datenull(year_one)
            'year_two = datenull(year_two)

            Dim starting As String = ""
            Dim ending As String = ""
            If HttpContext.Current.Session.Item("isMobile") = True Then
                starting = "<div class='" & css & "' align='right'><h3>" & title & "</h3> "
                ending = "</div>"
            End If

            If one_source = "" Then
                one_source = two_source
            ElseIf two_source = "" Then
                two_source = one_source
            End If
            If LCase(year_one) = LCase(year_two) Then
                year_two = ""
            Else
                If year_two <> "" Then
                    year_two = FormatDateShorthand(year_two)
                    year_two = "<span class='" & LCase(two_source) & "_row'>" & year_two & "</span>"
                End If
            End If

            If year_one = "" And year_two = "" Then
                starting = ""
                ending = ""
            End If

            If year_one <> "" Then
                year_one = FormatDateShorthand(year_one)
                year_one = "<span class='" & LCase(one_source) & "_row'>" & year_one & "</span>"
            End If

            difference_ac_listing = starting & year_one & IIf(year_two <> "", IIf(year_two <> "" And year_one <> "", "<br />", "") & year_two & ending & "<br>", "<br>")
        End Function

        Public Shared Function ShowEngineLabel(ByVal LabelText As String, ByVal acep_engine_tsoh_hours As Object, ByVal other_acep_engine_tsoh_hours As Object, ByVal acep_engine_ttsn_hours As Object, ByVal other_acep_engine_ttsn_hours As Object, ByVal acep_engine_shi_hours As Object, ByVal other_acep_engine_shi_hours As Object)
            Dim returnString As String = ""

            If (Not IsDBNull(acep_engine_ttsn_hours) And Not IsNothing(acep_engine_ttsn_hours)) Or (Not IsDBNull(other_acep_engine_ttsn_hours) And Not IsNothing(other_acep_engine_ttsn_hours)) Then
                'Show header
                returnString = LabelText
            ElseIf (Not IsDBNull(acep_engine_tsoh_hours) And Not IsNothing(acep_engine_tsoh_hours)) Or (Not IsDBNull(other_acep_engine_tsoh_hours) And Not IsNothing(other_acep_engine_tsoh_hours)) Then
                returnString = LabelText
            ElseIf (Not IsDBNull(acep_engine_shi_hours) And Not IsNothing(acep_engine_shi_hours)) Or (Not IsDBNull(other_acep_engine_shi_hours) And Not IsNothing(other_acep_engine_shi_hours)) Then
                returnString = LabelText
            End If



            Return "<strong>" & returnString & "</strong>"
        End Function

        Public Shared Function FormatDateShorthand(ByVal dateFormat As Object) As String
            Dim returnString As String = ""
            Dim DisplayDate As New Date
            If IsDate(dateFormat) Then
                DisplayDate = dateFormat
                returnString = DisplayDate.ToString("MM/dd/yy")
            Else
                If Not IsDBNull(dateFormat) Then
                    returnString = dateFormat
                End If
            End If
            Return returnString
        End Function
        Public Shared Function DisplayTextShorthand(ByVal text As Object) As String
            Dim returnString As String = ""
            If Not IsDBNull(text) Then
                If Not String.IsNullOrEmpty(text) Then
                    If Len(text) >= 25 Then
                        returnString = "<span title=""Value/Price Description: " & text & """ class=""help_cursor"">" & Left(text, 25) & "...</span>"
                    Else
                        returnString = "<span title=""Value/Price Description: " & text & """ class=""help_cursor"">" & text & "</span>"
                    End If
                End If
            End If

            Return returnString
        End Function
        Public Shared Function price_difference_ac_listing(ByVal year_one As Object, ByVal one_source As Object, ByVal year_two As Object, ByVal two_source As Object, ByVal forsale_flag As Object, ByVal other_forsale_flag As Object, ByVal css As String, ByVal title As String) As String
            price_difference_ac_listing = ""

            year_one = IIf(Not IsDBNull(year_one), year_one, "")
            year_two = IIf(Not IsDBNull(year_two), year_two, "")
            one_source = IIf(Not IsDBNull(one_source), one_source, "")
            two_source = IIf(Not IsDBNull(two_source), two_source, "")
            forsale_flag = IIf(Not IsDBNull(forsale_flag), forsale_flag, "")
            other_forsale_flag = IIf(Not IsDBNull(other_forsale_flag), other_forsale_flag, "")
            Dim starting As String = ""
            Dim ending As String = ""
            Dim style As String = "style='background-color:#caffb2;border:1px solid #3e6c0c;padding:4px;margin:0px;' class='smaller' "
            If HttpContext.Current.Session.Item("isMobile") = True Then

                If forsale_flag = "Y" Then
                    starting = "<div " & style & " align='right'><h3 style='color:#999999 !important;margin:0px;padding:0px;float:left;'>" & title & "</h3>"
                Else
                    starting = "<div class='" & css & "' align='right'><h3>" & title & "</h3> "
                End If

                ending = "</div>"
                style = ""
            End If
            If one_source = "" Then
                one_source = two_source
            ElseIf two_source = "" Then
                two_source = one_source
            End If
            If LCase(year_one) = LCase(year_two) Then
                year_two = ""
            Else
                If other_forsale_flag = "Y" Then

                    year_two = "<div " & style & " class='" & LCase(two_source) & "_row'>" & year_two & "</div>"

                Else
                    year_two = "<div class='" & LCase(two_source) & "_row' style='display:block;'>" & year_two & "</div>"
                End If
            End If

            If forsale_flag = "Y" Then
                year_one = "<div " & style & " class='" & LCase(one_source) & "_row'>" & year_one & "</div>"
            Else
                year_one = "<div class='" & LCase(one_source) & "_row' style='display:block;'>" & year_one & "</div>"
            End If
            price_difference_ac_listing = starting & year_one & "<br clear='all'/>" & year_two & ending
        End Function

        Public Shared Function String_Special_BlackList_Words(ByVal parameter As String, ByVal isEmail As Boolean) As Boolean
            Dim answer As Boolean = True

            If System.Text.RegularExpressions.Regex.IsMatch(parameter, "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") = False Or isEmail = False Then
                'This function is used in two spots outside of the login of the applications. 
                'This is on the LogonUser.ascx.vb control. It functions in both the "forgot password" and user login process.
                'The two parameters passed are a) The word being checked (in either case would be the username or password) and b) whether it's an email address
                'or not. The second parameter is important because the first thing this function does is check to see if it's going to pass email address regex validation.
                'What it does is take a reserved list of words that could be considered harmful and search for them.
                'If one is found, it stops the login and logs the error.
                Dim blackList As String() = {"--", ";--", ";", "/*", "*/", "@@",
                                               "varchar", "nvarchar", "alter",
                                               "begin", "create", "cursor", "declare", "delete",
                                               "drop", "execute", "fetch", "insert",
                                               "kill", "open", "select", "sysobjects", "syscolumns",
                                               "table", "update", "<script", "</script>"}

                For i As Integer = 0 To blackList.Length - 1
                    If (parameter.IndexOf(blackList(i), StringComparison.OrdinalIgnoreCase) >= 0) Then
                        answer = False
                        LogError("Login Failed With: " & blackList(i), New clsData_Manager_SQL)
                    End If
                Next

            End If

            Return answer
        End Function

        '''' <summary>
        '''' Sends Email from the Email Part of the CRM
        '''' </summary>
        '''' <param name="from">Person Sending, CRM User</param>
        '''' <param name="recepient">Person receiving</param>
        '''' <param name="bcc">Person blind copied</param>
        '''' <param name="cc">Person copied</param>
        '''' <param name="subject">Email Subject</param>
        '''' <param name="body">Email Body</param>
        '''' <remarks></remarks>
        'Public Shared Sub Send_Email_Queue(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, ByVal Email1 As Object)
        '    Try
        '        ' Instantiate a new instance of MailMessage
        '        Dim mMailMessage As New MailMessage()

        '        ' Set the sender address of the mail message
        '        mMailMessage.From = New MailAddress(from)
        '        ' Set the recepient address of the mail message
        '        mMailMessage.To.Add(New MailAddress(recepient))

        '        ' Check if the bcc value is nothing or an empty string
        '        If Not bcc Is Nothing And bcc <> String.Empty Then
        '            ' Set the Bcc address of the mail message
        '            mMailMessage.Bcc.Add(New MailAddress(bcc))
        '        End If

        '        ' Check if the cc value is nothing or an empty value
        '        If Not cc Is Nothing And cc <> String.Empty Then
        '            ' Set the CC address of the mail message
        '            mMailMessage.CC.Add(New MailAddress(cc))
        '        End If

        '        ' Set the subject of the mail message
        '        mMailMessage.Subject = subject
        '        ' Set the body of the mail message
        '        mMailMessage.Body = body

        '        'fileupload

        '        'Dim attach As System.Net.Mail.Attachment
        '        'Here's where we attach the file that they uploaded, if they upload a file.
        '        Dim FileUpload1 As New FileUpload
        '        If Not IsNothing(Email1) Then
        '            If Not IsNothing(Email1.FindControl("FileUpload1")) Then
        '                If Email1.Visible = True Then
        '                    FileUpload1 = Email1.FindControl("FileUpload1")
        '                    If Not IsNothing(FileUpload1) Then
        '                        If FileUpload1.PostedFile.FileName <> "" Then
        '                            mMailMessage.Attachments.Add(New Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName))
        '                        End If
        '                    End If
        '                End If
        '            End If
        '        End If
        '        'mMailMessage.Attachments.Add(FileUpload1)
        '        ' Set the format of the mail message body as HTML
        '        mMailMessage.IsBodyHtml = True
        '        ' Set the priority of the mail message to normal
        '        mMailMessage.Priority = MailPriority.Normal

        '        ' Instantiate a new instance of SmtpClient
        '        Dim mSmtpClient As New SmtpClient("localhost", 25)
        '        ' Send the mail message
        '        mSmtpClient.Send(mMailMessage)

        '    Catch ex As Exception
        '        'error_string = "edit_note.aspx.vb - SendEmail() - " & ex.Message
        '        'LogError(error_string)
        '    End Try
        'End Sub

        'This will turn the text green if the AC status is for sale. 
        Public Shared Function colormestatus_new(ByVal x As Object, ByVal y As Object, ByVal client As Boolean) As String
            colormestatus_new = ""
            Dim beginning As String = ""
            Dim ending As String = ""
            Try
                If Not IsDBNull(x) And Not IsDBNull(y) Then
                    If client = True Then
                        beginning = "<span class='client_row'>"
                    Else
                        beginning = "<span class='jetnet_row'>"
                    End If

                    ending = "</span>"

                    x = x.ToString
                    y = y.ToString
                    'This colors the AC status field. 
                    'colormestatus = ""
                    If UCase(x) = "Y" Then
                        If InStr(UCase(y), "SALE PENDING") > 0 Then
                            colormestatus_new = beginning & y & ending
                        Else
                            colormestatus_new = beginning & y & ending
                        End If
                    Else
                        If y <> "" Then
                            If InStr(y, "SALE PENDING") > 0 Then
                                colormestatus_new = beginning & y & ending
                            Else
                                colormestatus_new = beginning & y & ending
                            End If
                        End If
                    End If
                End If

                Return colormestatus_new
            Catch ex As Exception
                'error_string = "main_site.Master.vb - colormestatus() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        '        'This colors based on Previous/Exclusive/Leased
        Public Shared Function yes_no(ByVal x As String, ByVal y As String) As String
            yes_no = ""
            Try
                If Not IsDBNull(y) And Not IsDBNull(x) Then
                    x = x.ToString
                    y = y.ToString
                    Select Case y
                        Case "exclusive"
                            If x = "Y" Then
                                yes_no = "<span class='purple'>On Exclusive</span><br />"
                            Else
                                yes_no = ""
                            End If
                        Case "previous"
                            If x = "Y" Then
                                yes_no = "Previously Owned<br />"
                            Else
                                yes_no = ""
                            End If
                        Case "leased"
                            If x = "Y" Then
                                yes_no = "<span class='orange'>Leased"
                            Else
                                yes_no = ""
                            End If
                        Case Else
                            If x = "Y" Then
                                yes_no = "Yes"
                            Else
                                yes_no = "No"
                            End If
                    End Select
                End If
            Catch ex As Exception
                'error_string = "main_site.Master.vb - yes_no() - " & ex.Message
                'LogError(error_string)
            End Try
        End Function
        Public Shared showPopout As Boolean = False
        Public Shared Company_Popout_Text As String = ""
        Public Shared Function Company_Popout(ByVal comp_email_address As Object, ByVal comp_web_address As Object, ByVal id As Object, ByVal source As Object, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            Company_Popout_Text = ""
            showPopout = False
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Company_Popout = ""
            Try
                Dim address As String = ""
                Dim description As String = ""
                source = UCase(source)


                If Not IsDBNull(comp_email_address) Then
                    If Trim(comp_email_address) <> "" Then
                        showPopout = True
                        address = address & "<a href='mailto:" & comp_email_address & "' class='non_special_link'>" & comp_email_address & "</a>"
                    End If
                End If
                If Not IsDBNull(comp_web_address) Then
                    If Trim(comp_web_address) <> "" Then
                        showPopout = True
                        If Not IsDBNull(comp_email_address) Then
                            If Trim(comp_email_address) <> "" Then
                                address = address & "<br />"
                            End If
                        End If
                        address = address & "<a href='mailto:" & comp_web_address & "' class='non_special_link'>" & comp_web_address & "</a>"
                    End If
                End If
                'If source = "CLIENT" Then
                '    If Not IsDBNull(comp_description) Then
                '        If Trim(comp_description) <> "" Then
                '            showPopout = True
                '            If Len(comp_description) > 255 Then
                '                description = "<strong>Description:</strong> " & Left(comp_description, 255) & "...<br />"
                '            Else
                '                description = "<strong>Description:</strong> " & comp_description & "...<br />"
                '            End If

                '        End If
                '    End If
                'End If

                '------Phone Company Information Left Card Display----------------------------------------------------------------------
                Try

                    masterpage.aTempTable = masterpage.aclsData_temp.GetPhoneNumbers(id, 0, source, 0)
                    '' check the state of the DataTable
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            showPopout = True
                            address = address & "<br /><strong style='font-size:12px;color:#4d7997;'>Phone Numbers</strong><br />"
                            ' set it to the datagrid 
                            For Each q As DataRow In masterpage.aTempTable.Rows
                                address = address & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                            Next
                        Else
                            'rows = 0
                            address = address & ""
                        End If
                    Else
                        If masterpage.aclsData_temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_temp.class_error
                            masterpage.LogError("clsgeneral.vb - company_Popout() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - company_Popout() - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try

                If address <> "" Then
                    address = UCase(address.TrimEnd("<br />"))
                End If
                If showPopout = True Then
                    If description <> "" Then
                        Company_Popout_Text = address & "<br /><br />" & description
                    Else
                        Company_Popout_Text = address
                    End If
                Else
                    Company_Popout_Text = ""
                End If

            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - company_Popout() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Function
        Public Shared Function What_Aircraft_Name(ByVal Aircraft_Table As DataTable) As String
            Dim AIRCRAFT_TEXT As String = ""
            Dim AC_YEAR_MFR As String = ""
            Dim AC_REG_NBR As String = ""
            Dim AMOD_MAKE_NAME As String = ""
            Dim AMOD_MODEL_NAME As String = ""
            Dim AC_SER_NBR As String = ""

            If Not IsNothing(Aircraft_Table) Then
                If Aircraft_Table.Rows.Count > 0 Then
                    For Each R As DataRow In Aircraft_Table.Rows
                        'AC_YEAR_MFR = IIf(R("ac_year")
                        AIRCRAFT_TEXT = " (<em>"
                        If Not IsDBNull(R("ac_year_mfr")) Then
                            If R("ac_year_mfr") <> "" Then
                                AIRCRAFT_TEXT = AIRCRAFT_TEXT & R("ac_year_mfr") & " "
                            End If
                        End If
                        AIRCRAFT_TEXT = AIRCRAFT_TEXT & R("amod_make_name") & " " & R("amod_model_name") & " - "
                        If Not IsDBNull(R("ac_reg_nbr")) Then
                            If R("ac_reg_nbr") <> "" Then
                                AIRCRAFT_TEXT = AIRCRAFT_TEXT & "Reg #: " & R("ac_reg_nbr") & " - "
                            End If
                        End If
                        If Not IsDBNull(R("ac_ser_nbr")) Then
                            If R("ac_ser_nbr") <> "" Then
                                AIRCRAFT_TEXT = AIRCRAFT_TEXT & "Ser #:" & R("ac_ser_nbr") & "</em>)"
                            End If
                        End If
                    Next
                End If
            End If
            What_Aircraft_Name = AIRCRAFT_TEXT
        End Function
        Public Shared Function MouseOverTextStart() As String
            MouseOverTextStart = "<div style='width: 300px; padding: 8px;border:0px;'>"
            MouseOverTextStart = MouseOverTextStart & "<table cellspacing='0' cellpadding='0' class='flyout'>"
            MouseOverTextStart = MouseOverTextStart & "<tr>"
            MouseOverTextStart = MouseOverTextStart & " <td align='left' valign='top' width='19' height='17'><img src='images/rounded_popup_tlc.gif' alt='' /></td>"
            MouseOverTextStart = MouseOverTextStart & "<td align='left' valign='top' class='rounded_top'>"
            MouseOverTextStart = MouseOverTextStart & "&nbsp;"
            MouseOverTextStart = MouseOverTextStart & "</td>"
            MouseOverTextStart = MouseOverTextStart & "<td align='left' valign='top' width='19' height='17'><img src='images/rounded_popup_trc.gif' alt='' /></td>"
            MouseOverTextStart = MouseOverTextStart & "</tr>"
            MouseOverTextStart = MouseOverTextStart & "<tr>"
            MouseOverTextStart = MouseOverTextStart & "<td align='left' valign='top' class='rounded_left'>"
            MouseOverTextStart = MouseOverTextStart & "</td>"
            MouseOverTextStart = MouseOverTextStart & "<td align='left' bgcolor='#e3edf8'>"
        End Function
        Public Shared Function MouseOverTextEnd() As String
            MouseOverTextEnd = "</td>"
            MouseOverTextEnd = MouseOverTextEnd & "<td align='left' valign='top' class='rounded_right'>"
            MouseOverTextEnd = MouseOverTextEnd & "</td>"
            MouseOverTextEnd = MouseOverTextEnd & "</tr>"
            MouseOverTextEnd = MouseOverTextEnd & "<tr>"
            MouseOverTextEnd = MouseOverTextEnd & "<td align='left' valign='bottom' width='19' height='17'><img src='images/rounded_popup_blc.gif' alt='' /></td>"
            MouseOverTextEnd = MouseOverTextEnd & "<td align='left' valign='top' class='rounded_bottom'>"
            MouseOverTextEnd = MouseOverTextEnd & "&nbsp;"
            MouseOverTextEnd = MouseOverTextEnd & "</td>"
            MouseOverTextEnd = MouseOverTextEnd & "<td align='left' valign='bottom' width='19' height='17'><img src='images/rounded_popup_brc.gif' alt='' /></td>"
            MouseOverTextEnd = MouseOverTextEnd & "</tr>"
            MouseOverTextEnd = MouseOverTextEnd & " </table>"
            MouseOverTextEnd = MouseOverTextEnd & "</div>"
        End Function
        'Function to Display Data from Company Class!
        Public Shared Function Show_Company_Display(ByVal Company_Data As clsClient_Company, ByVal show_name As Boolean) As String
            Dim contact_text As String = ""
            Dim comp_email_address As String = ""
            Dim comp_web_address As String = ""

            If show_name = True Then
                If Company_Data.clicomp_name <> "" Then
                    contact_text = "<strong>" & UCase(Company_Data.clicomp_name) & "</strong><br />"
                End If
            End If
            If Company_Data.clicomp_alternate_name_type <> "" Then
                contact_text = contact_text & "<em>(" & Company_Data.clicomp_alternate_name_type & " "

                If Company_Data.clicomp_alternate_name <> "" Then
                    contact_text = contact_text & ": "
                End If
            End If

            If Company_Data.clicomp_alternate_name <> "" Then
                If Company_Data.clicomp_alternate_name = "" Then
                    contact_text = contact_text & "<em>"
                End If
                contact_text = contact_text & Company_Data.clicomp_alternate_name
                If Company_Data.clicomp_alternate_name_type <> "" Then
                    contact_text = contact_text & " )</em><br />"
                Else
                    contact_text = contact_text & "</em><br />"
                End If
            ElseIf Company_Data.clicomp_alternate_name_type <> "" Then
                contact_text = contact_text & "</em><br />"
            End If


            If Company_Data.clicomp_address1 <> "" Then
                contact_text = contact_text & Company_Data.clicomp_address1 & "<br />"
            End If
            If Company_Data.clicomp_address2 <> "" Then
                contact_text = contact_text & Company_Data.clicomp_address2 & "<br />"
            End If


            contact_text = contact_text & Company_Data.clicomp_city

            If Not IsDBNull(Company_Data.clicomp_state) Then
                If Not String.IsNullOrEmpty(Company_Data.clicomp_state) Then
                    contact_text = contact_text & ", " & Company_Data.clicomp_state & " "
                End If
            End If

            If Not IsDBNull(Company_Data.clicomp_zip_code) Then
                If Not String.IsNullOrEmpty(Company_Data.clicomp_zip_code) Then
                    contact_text = contact_text & " " & Company_Data.clicomp_zip_code
                End If
            End If

            contact_text = contact_text & " " & Replace(Company_Data.clicomp_country, "United States", "USA")

            'Set up both the Email Address and Company Web Address for Display!
            If Not IsDBNull(Company_Data.clicomp_email_address) Then
                comp_email_address = IIf(Not String.IsNullOrEmpty(Company_Data.clicomp_email_address), "<br /><a href='mailto:" & Company_Data.clicomp_email_address & "' class='non_special_link'>" & Company_Data.clicomp_email_address & "</a>", "")
            End If
            If Not IsDBNull(Company_Data.clicomp_web_address) Then
                If Not String.IsNullOrEmpty(Company_Data.clicomp_web_address) Then
                    comp_web_address = IIf((InStr(Company_Data.clicomp_web_address, "http") < 0), "<br /><a href='" & Company_Data.clicomp_web_address & "' class='non_special_link' target='new'>" & Company_Data.clicomp_web_address & "</a>", "<br /><a href='http://" & Company_Data.clicomp_web_address & "' class='non_special_link' target='new'>" & Company_Data.clicomp_web_address & "</a>")
                End If
            End If

            contact_text = IIf(Company_Data.clicomp_email_address <> "", contact_text & comp_email_address, contact_text)
            contact_text = IIf(Company_Data.clicomp_web_address <> "", contact_text & comp_web_address, contact_text)

            'What about that description? 
            If Company_Data.clicomp_description <> "" Then
                contact_text = contact_text & "<br /><em>" & Company_Data.clicomp_description & "</em><br />"
            End If

            'What about all of the company category fields?
            If Company_Data.clicomp_category1 <> "" Then
                contact_text = contact_text & IIf((Company_Data.clicomp_category1 <> ""), Company_Data.clicomp_category1 & "<br />", "")
            End If
            If Company_Data.clicomp_category2 <> "" Then
                contact_text = contact_text & IIf((Company_Data.clicomp_category2 <> ""), Company_Data.clicomp_category2 & "<br />", "")
            End If
            If Company_Data.clicomp_category3 <> "" Then
                contact_text = contact_text & IIf((Company_Data.clicomp_category3 <> ""), Company_Data.clicomp_category3 & "<br />", "")
            End If
            If Company_Data.clicomp_category4 <> "" Then
                contact_text = contact_text & IIf((Company_Data.clicomp_category4 <> ""), Company_Data.clicomp_category4 & "<br />", "")
            End If
            If Company_Data.clicomp_category5 <> "" Then
                contact_text = contact_text & IIf((Company_Data.clicomp_category5 <> ""), Company_Data.clicomp_category5 & "<br />", "")
            End If

            Show_Company_Display = contact_text
        End Function

        Public Shared Function Create_Aircraft_Class(ByVal Aircraft_Table As DataTable, ByVal extension As String) As clsClient_Aircraft
            Dim Aircraft_Data As New clsClient_Aircraft
            If Not IsNothing(Aircraft_Table) Then
                For Each r As DataRow In Aircraft_Table.Rows
                    Aircraft_Data.cliaircraft_action_date = IIf(Not IsDBNull(r(extension & "_action_date")), r(extension & "_action_date"), Now())
                    Aircraft_Data.cliaircraft_airframe_maintenance_program = IIf(Not IsDBNull(r(extension & "_airframe_maintenance_program")), r(extension & "_airframe_maintenance_program"), 0)
                    Aircraft_Data.cliaircraft_airframe_maintenance_tracking_program = IIf(Not IsDBNull(r(extension & "_airframe_maintenance_tracking_program")), r(extension & "_airframe_maintenance_tracking_program"), 0)

                    If Not IsDBNull(r(extension & "_airframe_total_hours")) Then
                        Aircraft_Data.cliaircraft_airframe_total_hours = IIf(Not IsDBNull(r(extension & "_airframe_total_hours")), r(extension & "_airframe_total_hours"), 0)
                    End If
                    If Not IsDBNull(r(extension & "_airframe_total_landings")) Then
                        Aircraft_Data.cliaircraft_airframe_total_landings = IIf(Not IsDBNull(r(extension & "_airframe_total_landings")), r(extension & "_airframe_total_landings"), 0)
                    End If
                    Aircraft_Data.cliaircraft_alt_ser_nbr = IIf(Not IsDBNull(r(extension & "_alt_ser_nbr")), r(extension & "_alt_ser_nbr"), "")
                    Aircraft_Data.cliaircraft_aport_city = IIf(Not IsDBNull(r(extension & "_alt_ser_nbr")), r(extension & "_alt_ser_nbr"), "")
                    Aircraft_Data.cliaircraft_aport_country = IIf(Not IsDBNull(r(extension & "_aport_country")), r(extension & "_aport_country"), "")
                    Aircraft_Data.cliaircraft_aport_iata_code = IIf(Not IsDBNull(r(extension & "_aport_iata_code")), r(extension & "_aport_iata_code"), "")
                    Aircraft_Data.cliaircraft_aport_name = IIf(Not IsDBNull(r(extension & "_aport_name")), r(extension & "_aport_name"), "")
                    Aircraft_Data.cliaircraft_aport_private = IIf(Not IsDBNull(r(extension & "_aport_private")), r(extension & "_aport_private"), "")
                    Aircraft_Data.cliaircraft_aport_state = IIf(Not IsDBNull(r(extension & "_aport_state")), r(extension & "_aport_state"), "")
                    Aircraft_Data.cliaircraft_apu_maintance_program = IIf(Not IsDBNull(r(extension & "_apu_maintance_program")), r(extension & "_apu_maintance_program"), "")
                    Aircraft_Data.cliaircraft_apu_model_name = IIf(Not IsDBNull(r(extension & "_apu_model_name")), r(extension & "_apu_model_name"), "")
                    Aircraft_Data.cliaircraft_apu_ser_nbr = IIf(Not IsDBNull(r(extension & "_apu_ser_nbr")), r(extension & "_apu_ser_nbr"), "")


                    Aircraft_Data.cliaircraft_apu_tshi_hours = IIf(Not IsDBNull(r(extension & "_apu_tshi_hours")), r(extension & "_apu_tshi_hours"), 0)
                    Aircraft_Data.cliaircraft_apu_tsoh_hours = IIf(Not IsDBNull(r(extension & "_apu_tsoh_hours")), r(extension & "_apu_tsoh_hours"), 0)
                    Aircraft_Data.cliaircraft_apu_ttsn_hours = IIf(Not IsDBNull(r(extension & "_apu_ttsn_hours")), r(extension & "_apu_ttsn_hours"), 0)


                    Aircraft_Data.cliaircraft_asking_price = IIf(Not IsDBNull(r(extension & "_asking_price")), r(extension & "_asking_price"), 0)
                    Aircraft_Data.cliaircraft_asking_wordage = IIf(Not IsDBNull(r(extension & "_asking_wordage")), r(extension & "_asking_wordage"), "")

                    If extension = "cliaircraft" Then
                        Aircraft_Data.cliaircraft_cliamod_id = IIf(Not IsDBNull(r(extension & "_cliamod_id")), r(extension & "_cliamod_id"), 0)
                    Else
                        Aircraft_Data.cliaircraft_cliamod_id = IIf(Not IsDBNull(r(extension & "_amod_id")), r(extension & "_amod_id"), 0)
                    End If
                    Aircraft_Data.cliaircraft_confidential_notes = IIf(Not IsDBNull(r(extension & "_confidential_notes")), r(extension & "_confidential_notes"), "")
                    Aircraft_Data.cliaircraft_country_of_registration = IIf(Not IsDBNull(r(extension & "_country_of_registration")), r(extension & "_country_of_registration"), "")
                    Aircraft_Data.cliaircraft_damage_flag = IIf(Not IsDBNull(r(extension & "_damage_flag")), r(extension & "_damage_flag"), "")
                    Aircraft_Data.cliaircraft_damage_history_notes = IIf(Not IsDBNull(r(extension & "_damage_history_notes")), r(extension & "_damage_history_notes"), "")
                    If Not IsDBNull(r(extension & "_date_engine_times_as_of")) Then
                        Aircraft_Data.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(r(extension & "_date_engine_times_as_of")), r(extension & "_date_engine_times_as_of"), Now())
                    End If
                    If Not IsDBNull(r(extension & "_date_listed")) Then
                        Aircraft_Data.cliaircraft_date_listed = IIf(Not IsDBNull(r(extension & "_date_listed")), r(extension & "_date_listed"), Now())
                    End If
                    If Not IsDBNull(r(extension & "_date_purchased")) Then
                        Aircraft_Data.cliaircraft_date_purchased = IIf(Not IsDBNull(r(extension & "_date_purchased")), r(extension & "_date_purchased"), Now())
                    End If
                    Aircraft_Data.cliaircraft_delivery = IIf(Not IsDBNull(r(extension & "_delivery")), r(extension & "_delivery"), "")

                    If extension = "cliaircraft" Then
                        Aircraft_Data.cliaircraft_est_price = IIf(Not IsDBNull(r(extension & "_est_price")), r(extension & "_est_price"), 0)
                        Aircraft_Data.cliaircraft_ac_maintained = IIf(Not IsDBNull(r(extension & "_ac_maintained")), r(extension & "_ac_maintained"), "")
                    Else
                        Aircraft_Data.cliaircraft_ac_maintained = IIf(Not IsDBNull(r(extension & "_maintained")), r(extension & "_maintained"), "")
                    End If

                    Aircraft_Data.cliaircraft_exclusive_flag = IIf(Not IsDBNull(r(extension & "_exclusive_flag")), r(extension & "_exclusive_flag"), "N")
                    Aircraft_Data.cliaircraft_exterior_doneby_name = IIf(Not IsDBNull(r(extension & "_exterior_doneby_name")), r(extension & "_exterior_doneby_name"), "")
                    Aircraft_Data.cliaircraft_exterior_month_year = IIf(Not IsDBNull(r(extension & "_exterior_month_year")), r(extension & "_exterior_month_year"), "")
                    Aircraft_Data.cliaircraft_exterior_rating = IIf(Not IsDBNull(r(extension & "_exterior_rating")), r(extension & "_exterior_rating"), 0)
                    Aircraft_Data.cliaircraft_forsale_flag = IIf(Not IsDBNull(r(extension & "_forsale_flag")), r(extension & "_forsale_flag"), "N")
                    Aircraft_Data.cliaircraft_id = IIf(Not IsDBNull(r(extension & "_id")), r(extension & "_id"), 0)
                    Aircraft_Data.cliaircraft_interior_config_name = IIf(Not IsDBNull(r(extension & "_interior_config_name")), r(extension & "_interior_config_name"), "")
                    Aircraft_Data.cliaircraft_interior_doneby_name = IIf(Not IsDBNull(r(extension & "_interior_doneby_name")), r(extension & "_interior_doneby_name"), "")
                    Aircraft_Data.cliaircraft_interior_month_year = IIf(Not IsDBNull(r(extension & "_interior_month_year")), r(extension & "_interior_month_year"), "")
                    Aircraft_Data.cliaircraft_interior_rating = IIf(Not IsDBNull(r(extension & "_interior_rating")), r(extension & "_interior_rating"), 0)

                    If extension = "cliaircraft" Then
                        Aircraft_Data.cliaircraft_jetnet_ac_id = IIf(Not IsDBNull(r(extension & "_jetnet_ac_id")), r(extension & "_jetnet_ac_id"), 0)
                        Aircraft_Data.cliaircraft_custom_1 = IIf(Not IsDBNull(r(extension & "_custom_1")), r(extension & "_custom_1"), "")
                        Aircraft_Data.cliaircraft_custom_2 = IIf(Not IsDBNull(r(extension & "_custom_2")), r(extension & "_custom_2"), "")
                        Aircraft_Data.cliaircraft_custom_3 = IIf(Not IsDBNull(r(extension & "_custom_3")), r(extension & "_custom_3"), "")
                        Aircraft_Data.cliaircraft_custom_4 = IIf(Not IsDBNull(r(extension & "_custom_4")), r(extension & "_custom_4"), "")
                        Aircraft_Data.cliaircraft_custom_5 = IIf(Not IsDBNull(r(extension & "_custom_5")), r(extension & "_custom_5"), "")
                        Aircraft_Data.cliaircraft_custom_6 = IIf(Not IsDBNull(r(extension & "_custom_6")), r(extension & "_custom_6"), "")
                        Aircraft_Data.cliaircraft_custom_7 = IIf(Not IsDBNull(r(extension & "_custom_7")), r(extension & "_custom_7"), "")
                        Aircraft_Data.cliaircraft_custom_8 = IIf(Not IsDBNull(r(extension & "_custom_8")), r(extension & "_custom_8"), "")
                        Aircraft_Data.cliaircraft_custom_9 = IIf(Not IsDBNull(r(extension & "_custom_9")), r(extension & "_custom_9"), "")
                        Aircraft_Data.cliaircraft_custom_10 = IIf(Not IsDBNull(r(extension & "_custom_10")), r(extension & "_custom_10"), "")
                        Aircraft_Data.cliaircraft_value_description = IIf(Not IsDBNull(r(extension & "_value_description")), r(extension & "_value_description"), "")
                        Aircraft_Data.cliaircraft_broker_price = IIf(Not IsDBNull(r(extension & "_broker_price")), r(extension & "_broker_price"), 0)

                    End If


                    If extension = "ac" Then
                        Aircraft_Data.cliaircraft_picture_exist_flag = IIf(Not IsDBNull(r(extension & "_picture_exist_flag")), r(extension & "_picture_exist_flag"), "N")
                    End If

                    Aircraft_Data.cliaircraft_lease_flag = IIf(Not IsDBNull(r(extension & "_lease_flag")), r(extension & "_lease_flag"), "N")
                    Aircraft_Data.cliaircraft_lifecycle = IIf(Not IsDBNull(r(extension & "_lifecycle")), r(extension & "_lifecycle"), 0)
                    Aircraft_Data.cliaircraft_new_flag = IIf(Not IsDBNull(r(extension & "_new_flag")), r(extension & "_new_flag"), "N")
                    Aircraft_Data.cliaircraft_ownership = IIf(Not IsDBNull(r(extension & "_ownership")), r(extension & "_ownership"), "")
                    Aircraft_Data.cliaircraft_passenger_count = IIf(Not IsDBNull(r(extension & "_passenger_count")), r(extension & "_passenger_count"), 0)
                    Aircraft_Data.cliaircraft_prev_reg_nbr = IIf(Not IsDBNull(r(extension & "_prev_reg_nbr")), r(extension & "_prev_reg_nbr"), "")
                    Aircraft_Data.cliaircraft_reg_nbr = IIf(Not IsDBNull(r(extension & "_reg_nbr")), r(extension & "_reg_nbr"), "")
                    Aircraft_Data.cliaircraft_ser_nbr = IIf(Not IsDBNull(r(extension & "_ser_nbr")), r(extension & "_ser_nbr"), "")
                    Aircraft_Data.cliaircraft_status = IIf(Not IsDBNull(r(extension & "_status")), r(extension & "_status"), "")
                    Aircraft_Data.cliaircraft_usage = IIf(Not IsDBNull(r(extension & "_usage")), r(extension & "_usage"), "")

                    If extension = "cliaircraft" Then
                        Aircraft_Data.cliaircraft_user_id = IIf(Not IsDBNull(r(extension & "_user_id")), r(extension & "_user_id"), 0)
                    End If

                    Aircraft_Data.cliaircraft_year_dlv = IIf(Not IsDBNull(r(extension & "_year_dlv")), r(extension & "_year_dlv"), "")
                    Aircraft_Data.cliaircraft_year_mfr = IIf(Not IsDBNull(r(extension & "_year_mfr")), r(extension & "_year_mfr"), "")
                Next
            End If

            Create_Aircraft_Class = Aircraft_Data
        End Function
        Public Shared Function Create_Aircraft_Model_Class(ByVal Aircraft_Model_Table As DataTable, ByVal extension As String) As clsClient_Aircraft_Model
            Dim Aircraft_Model As New clsClient_Aircraft_Model
            If Not IsNothing(Aircraft_Model_Table) Then
                If Aircraft_Model_Table.Rows.Count > 0 Then
                    Aircraft_Model.cliamod_airframe_type = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_airframe_type")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_airframe_type"), "")
                    Aircraft_Model.cliamod_id = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_id")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_id"), 0)
                    If extension = "cli" Then
                        Aircraft_Model.cliamod_jetnet_amod_id = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_jetnet_amod_id")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_jetnet_amod_id"), 0)
                    End If
                    Aircraft_Model.cliamod_make_name = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_make_name")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_make_name"), "")
                    Aircraft_Model.cliamod_make_type = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_make_type")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_make_type"), "")
                    Aircraft_Model.cliamod_manufacturer_name = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_manufacturer_name")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_manufacturer_name"), "")
                    Aircraft_Model.cliamod_model_name = IIf(Not IsDBNull(Aircraft_Model_Table.Rows(0).Item(extension & "amod_model_name")), Aircraft_Model_Table.Rows(0).Item(extension & "amod_model_name"), "")
                End If
            End If
            Create_Aircraft_Model_Class = Aircraft_Model
        End Function
        Public Shared Function Build_Aircraft_Display(ByVal Aircraft_Data As clsClient_Aircraft, ByVal basic As Boolean, ByVal advanced As Boolean, ByVal CondensedNoteDisplay As Boolean) As String
            'Basic = Aircraft Card, left side of Data. All the serial number, reg number, etc.
            'Advanced = Aircraft Card, Right side of Data. Airport, etc.

            Dim Aircraft_Text As String = ""

            If basic = True Then
                If Aircraft_Data.cliaircraft_ser_nbr <> "" Then
                    If CondensedNoteDisplay Then
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Ser #"
                        If Not IsDBNull(Aircraft_Data.cliaircraft_date_purchased) Then
                            Dim date_purchased As String = datenull(Aircraft_Data.cliaircraft_date_purchased)
                            If date_purchased <> "" Then
                                Aircraft_Text += "/Purchased:</span> " & Aircraft_Data.cliaircraft_ser_nbr & ""
                                Aircraft_Text = Aircraft_Text & " / " & Aircraft_Data.cliaircraft_date_purchased
                            Else
                                Aircraft_Text += ":</span><b> " & Aircraft_Data.cliaircraft_ser_nbr & "</b>"
                            End If
                        End If
                    Else
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Ser #:</span> <b>" & Aircraft_Data.cliaircraft_ser_nbr & "</b>"
                    End If

                    Aircraft_Text += "</span>"
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_year_mfr) Then
                    If Aircraft_Data.cliaircraft_year_mfr <> "" Then
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Year Mfr/Dlv:</span> " & Aircraft_Data.cliaircraft_year_mfr
                    End If

                    If Aircraft_Data.cliaircraft_year_mfr <> "" Then
                        Aircraft_Text += " / " & Aircraft_Data.cliaircraft_year_dlv
                    End If

                    If Aircraft_Data.cliaircraft_year_mfr <> "" Then
                        Aircraft_Text += "</span>"
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_year_dlv) Then
                    Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Delivered:</span> " & Aircraft_Data.cliaircraft_year_dlv & "</span>"
                End If

                If CondensedNoteDisplay = False Then
                    If Not IsDBNull(Aircraft_Data.cliaircraft_date_purchased) Then
                        Dim date_purchased As String = datenull(Aircraft_Data.cliaircraft_date_purchased)
                        If date_purchased <> "" Then
                            Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Purchased:</span> " & Aircraft_Data.cliaircraft_date_purchased & "</span>"
                        End If
                    End If
                End If

                If Not IsDBNull(Aircraft_Data.cliaircraft_reg_nbr) Then
                    If Aircraft_Data.cliaircraft_reg_nbr <> "" Then
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Reg #/Prev:</span> " & Aircraft_Data.cliaircraft_reg_nbr
                        If Aircraft_Data.cliaircraft_prev_reg_nbr <> "" Then
                            Aircraft_Text += " / " & Aircraft_Data.cliaircraft_prev_reg_nbr
                        End If
                        Aircraft_Text += "</span>"
                    End If
                End If
                'If Not IsDBNull(Aircraft_Data.cliaircraft_prev_reg_nbr) Then
                '    If Aircraft_Data.cliaircraft_prev_reg_nbr <> "" Then
                '        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Previous Reg #:</span> " & Aircraft_Data.cliaircraft_prev_reg_nbr & "</span>"
                '    End If
                'End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_alt_ser_nbr) Then
                    If Aircraft_Data.cliaircraft_alt_ser_nbr <> "" Then
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Alt. Ser #:</span> " & Aircraft_Data.cliaircraft_alt_ser_nbr & "</span>"
                    End If
                End If
                If HttpContext.Current.Session("localSubscription").crmAerodexFlag = True Then
                Else

                    If Aircraft_Data.cliaircraft_forsale_flag = "N" Then
                        If Not IsDBNull(Aircraft_Data.cliaircraft_value_description) Then
                            If Aircraft_Data.cliaircraft_value_description <> "" Then
                                Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>" & Aircraft_Data.cliaircraft_value_description & "</span>"
                            End If
                        End If
                    ElseIf Aircraft_Data.cliaircraft_forsale_flag = "Y" Then
                        Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'><b class='green'>"

                        If UCase(Aircraft_Data.cliaircraft_status) = "SEE NOTES" Then
                            ' Aircraft_Text += "<span class='help_cursor text_underline' title='" & Replace(Aircraft_Data.cliaircraft_confidential_notes, "'", "&#39;") & "'>" & Aircraft_Data.cliaircraft_status & "</span>"
                        Else
                            Aircraft_Text += Aircraft_Data.cliaircraft_status
                        End If


                        If Not IsDBNull(Aircraft_Data.cliaircraft_asking_wordage) Then
                            If Aircraft_Data.cliaircraft_asking_wordage <> "" Then
                                If Trim(Aircraft_Data.cliaircraft_asking_wordage) = "Price" Then
                                    If Not IsDBNull(Aircraft_Data.cliaircraft_asking_price) Then
                                        Dim asking_price As String = no_zero(Aircraft_Data.cliaircraft_asking_price, "", True)
                                        If asking_price <> "" Then
                                            Aircraft_Text = Aircraft_Text & " Asking: " & asking_price
                                        End If
                                    End If
                                Else
                                    Aircraft_Text = Aircraft_Text & " " & Aircraft_Data.cliaircraft_asking_wordage
                                End If
                            End If
                        End If

                        If Not IsDBNull(Aircraft_Data.cliaircraft_value_description) Then
                            If Aircraft_Data.cliaircraft_value_description <> "" Then
                                Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_value_description
                            End If
                        End If


                        Aircraft_Text = Aircraft_Text & "</b></span></span>"

                        If UCase(Aircraft_Data.cliaircraft_status) = "SEE NOTES" Then
                            Aircraft_Text += "<span class='li'><span class='label green'>" & Aircraft_Data.cliaircraft_confidential_notes & "</span></span>"
                        End If
                    End If
                    If Not IsDBNull(Aircraft_Data.cliaircraft_est_price) Then
                        If Aircraft_Data.cliaircraft_est_price <> 0 Then
                            Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Take Price:</span> " & no_zero(Aircraft_Data.cliaircraft_est_price, "", True) & "</span>"
                        End If
                    End If
                    If Not IsDBNull(Aircraft_Data.cliaircraft_date_listed) Then
                        Dim date_listed As String = datenull(Aircraft_Data.cliaircraft_date_listed)
                        If date_listed <> "" Then
                            Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Listed/DOM:</span> " & date_listed

                            Aircraft_Text += " / " & Replace(trans_date_diff(Now(), Aircraft_Data.cliaircraft_date_listed, 2), "DOM: ", "")
                            Aircraft_Text += "</span> "
                        End If
                    End If




                    If Aircraft_Data.cliaircraft_forsale_flag <> "Y" Then
                        If Not IsDBNull(Aircraft_Data.cliaircraft_status) Then
                            If Aircraft_Data.cliaircraft_status <> "" Then
                                Select Case Aircraft_Data.cliaircraft_status
                                    Case "For Sale"
                                    Case Else
                                        Aircraft_Text = Aircraft_Text & "<span class='li'>" & Aircraft_Data.cliaircraft_status & "</span>"
                                End Select
                            End If
                        End If
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_lifecycle) Then
                    Select Case Aircraft_Data.cliaircraft_lifecycle
                        Case "1"
                            Aircraft_Text = Aircraft_Text & "<span class='li'>In Production</span>"
                        Case "2"
                            Aircraft_Text = Aircraft_Text & "<span class='li'>New</span>"
                        Case "3"
                            Aircraft_Text = Aircraft_Text & "<span class='li'>In Operation</span>"
                        Case "4"
                            Aircraft_Text = Aircraft_Text & "<span class='li'>Retired</span>"
                    End Select
                End If
            ElseIf advanced = True Then
                Aircraft_Text = ""
                Aircraft_Text = Aircraft_Text & "<span class='li'><span class='label'>Airport: </span>"

                'If Not IsDBNull(Aircraft_Data.cliaircraft_aport_private) Then
                '  If Aircraft_Data.cliaircraft_aport_private = "Y" Then
                '    Aircraft_Text = Aircraft_Text & "Private: "
                '  End If
                'End If

                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_iata_code) Then
                    If Aircraft_Data.cliaircraft_aport_iata_code <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_iata_code
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_icao_code) Then
                    If Aircraft_Data.cliaircraft_aport_icao_code <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_icao_code
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_name) Then
                    If Aircraft_Data.cliaircraft_aport_name <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_name
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_state) Then
                    If Aircraft_Data.cliaircraft_aport_state <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_state
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_country) Then
                    If Aircraft_Data.cliaircraft_aport_country <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_country
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_city) Then
                    If Aircraft_Data.cliaircraft_aport_city <> "" Then
                        Aircraft_Text = Aircraft_Text & " - " & Aircraft_Data.cliaircraft_aport_city
                    End If
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_aport_country) Then
                    If Aircraft_Data.cliaircraft_aport_private = "Y" Then
                        Aircraft_Text = Aircraft_Text & "</span><span class='li'><em>(Private Airport)</em></span>"
                    Else
                        Aircraft_Text = Aircraft_Text & "</span><span class='li'><em>(Non-Private Airport)</em></span>"
                    End If
                End If
                If HttpContext.Current.Session("localSubscription").crmAerodexFlag = True Then
                Else
                    Aircraft_Text = Aircraft_Text & yes_no(Aircraft_Data.cliaircraft_exclusive_flag, "previous")
                End If
                If Not IsDBNull(Aircraft_Data.cliaircraft_ownership) Then
                    If Aircraft_Data.cliaircraft_ownership <> "" Then
                        Select Case Aircraft_Data.cliaircraft_ownership
                            Case "W"
                                Aircraft_Text = Aircraft_Text & "<span class='li'>Wholly Owned</span>"
                            Case "F"
                                Aircraft_Text = Aircraft_Text & "<span class='li'>Fractionally Owned</span>"
                            Case "C"
                                Aircraft_Text = Aircraft_Text & "<span class='li'>Co-Owned</span>"
                        End Select
                    End If
                End If
                If HttpContext.Current.Session("localSubscription").crmAerodexFlag = True Then
                Else
                    If Aircraft_Data.cliaircraft_exclusive_flag <> "" Then
                        Aircraft_Text = Aircraft_Text & yes_no(Aircraft_Data.cliaircraft_exclusive_flag, "exclusive")
                    End If
                    If Aircraft_Data.cliaircraft_lease_flag <> "" Then
                        Aircraft_Text = Aircraft_Text & yes_no(Aircraft_Data.cliaircraft_lease_flag, "leased")
                    End If
                End If

            End If
            Build_Aircraft_Display = Aircraft_Text
        End Function
        Public Shared Function Create_Company_Class(ByVal Company_Table As DataTable, ByVal source As String, ByVal Preferences_Table As DataTable) As clsClient_Company
            Dim Company_Data As New clsClient_Company
            Dim Category1 As String = ""
            Dim Category2 As String = ""
            Dim Category3 As String = ""
            Dim Category4 As String = ""
            Dim Category5 As String = ""
            ''Sets the variables for the company display

            'Fields that are shared between Jetnet and Client: 
            Company_Data.clicomp_name = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_name")), Company_Table.Rows(0).Item("comp_name"), "")
            Company_Data.clicomp_state = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_state")), Company_Table.Rows(0).Item("comp_state"), "")
            Company_Data.clicomp_web_address = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_web_address")), Company_Table.Rows(0).Item("comp_web_address"), "")
            Company_Data.clicomp_zip_code = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_zip_code")), Company_Table.Rows(0).Item("comp_zip_code"), "")
            Company_Data.clicomp_email_address = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_email_address")), Company_Table.Rows(0).Item("comp_email_address"), "")
            Company_Data.clicomp_country = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_country")), Company_Table.Rows(0).Item("comp_country"), "")
            Company_Data.clicomp_city = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_city")), Company_Table.Rows(0).Item("comp_city"), "")
            Company_Data.clicomp_address1 = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_address1")), Company_Table.Rows(0).Item("comp_address1"), "")
            Company_Data.clicomp_address2 = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_address2")), Company_Table.Rows(0).Item("comp_address2"), "")
            Company_Data.clicomp_agency_type = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_agency_type")), Company_Table.Rows(0).Item("comp_agency_type"), "")
            Company_Data.clicomp_alternate_name = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_alternate_name")), Company_Table.Rows(0).Item("comp_alternate_name"), "")
            Company_Data.clicomp_alternate_name_type = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_alternate_name_type")), Company_Table.Rows(0).Item("comp_alternate_name_type"), "")
            Company_Data.clicomp_date_updated = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_action_date")), Company_Table.Rows(0).Item("comp_action_date"), Now())
            'Client Only Fields
            If source = "CLIENT" Then
                Company_Data.clicomp_status = IIf(Not IsDBNull(Company_Table.Rows(0).Item("clicomp_status")), Company_Table.Rows(0).Item("clicomp_status"), "")
                Company_Data.clicomp_jetnet_comp_id = IIf(Not IsDBNull(Company_Table.Rows(0).Item("jetnet_comp_id")), Company_Table.Rows(0).Item("jetnet_comp_id"), 0)
                Company_Data.clicomp_user_id = IIf(Not IsDBNull(Company_Table.Rows(0).Item("comp_user_id")), Company_Table.Rows(0).Item("comp_user_id"), 0)
                Company_Data.clicomp_description = IIf(Not IsDBNull(Company_Table.Rows(0).Item("clicomp_description")), Company_Table.Rows(0).Item("clicomp_description"), "")
                Company_Data.clicomp_description = Replace(Company_Data.clicomp_description, vbCrLf, "<br />")

                If Not IsNothing(Preferences_Table) Then
                    If Preferences_Table.Rows.Count > 0 Then
                        ' For Each r As DataRow In aTempTable.Rows
                        If Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category1_use")) Then
                            If Preferences_Table.Rows(0).Item("clipref_category1_use") = "Y" Then
                                Category1 = IIf(Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category1_name")), Preferences_Table.Rows(0).Item("clipref_category1_name"), "")
                            Else

                            End If
                        End If

                        If Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category2_use")) Then
                            If Preferences_Table.Rows(0).Item("clipref_category2_use") = "Y" Then
                                Category2 = IIf(Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category2_name")), Preferences_Table.Rows(0).Item("clipref_category2_name"), "")
                            Else

                            End If
                        End If

                        If Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category3_use")) Then
                            If Preferences_Table.Rows(0).Item("clipref_category3_use") = "Y" Then
                                Category3 = IIf(Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category3_name")), Preferences_Table.Rows(0).Item("clipref_category3_name"), "")
                            Else

                            End If
                        End If

                        If Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category4_use")) Then
                            If Preferences_Table.Rows(0).Item("clipref_category4_use") = "Y" Then
                                Category4 = IIf(Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category4_name")), Preferences_Table.Rows(0).Item("clipref_category4_name"), "")
                            Else

                            End If
                        End If

                        If Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category5_use")) Then
                            If Preferences_Table.Rows(0).Item("clipref_category5_use") = "Y" Then
                                Category5 = IIf(Not IsDBNull(Preferences_Table.Rows(0).Item("clipref_category5_name")), Preferences_Table.Rows(0).Item("clipref_category5_name"), "")
                            Else
                            End If
                        End If
                    End If
                End If

                If Not IsNothing(Preferences_Table) Then
                    If (Not IsDBNull(Company_Table.Rows(0).Item("clicomp_category1"))) Then
                        Company_Data.clicomp_category1 = IIf((Company_Table.Rows(0).Item("clicomp_category1") <> ""), Category1 & ": " & Company_Table.Rows(0).Item("clicomp_category1"), "")
                    End If
                    If (Not IsDBNull(Company_Table.Rows(0).Item("clicomp_category2"))) Then
                        Company_Data.clicomp_category2 = IIf((Company_Table.Rows(0).Item("clicomp_category2") <> ""), Category2 & ": " & Company_Table.Rows(0).Item("clicomp_category2"), "")
                    End If
                    If (Not IsDBNull(Company_Table.Rows(0).Item("clicomp_category3"))) Then
                        Company_Data.clicomp_category3 = IIf((Company_Table.Rows(0).Item("clicomp_category3") <> ""), Category3 & ": " & Company_Table.Rows(0).Item("clicomp_category3"), "")
                    End If
                    If (Not IsDBNull(Company_Table.Rows(0).Item("clicomp_category4"))) Then
                        Company_Data.clicomp_category4 = IIf((Company_Table.Rows(0).Item("clicomp_category4") <> ""), Category4 & ": " & Company_Table.Rows(0).Item("clicomp_category4"), "")
                    End If
                    If (Not IsDBNull(Company_Table.Rows(0).Item("clicomp_category5"))) Then
                        Company_Data.clicomp_category5 = IIf((Company_Table.Rows(0).Item("clicomp_category5") <> ""), Category5 & ": " & Company_Table.Rows(0).Item("clicomp_category5"), "")
                    End If
                End If
            End If
            Company_Table.Dispose()
            Create_Company_Class = Company_Data
        End Function

        Public Shared Function Create_Note_Array_Class(ByVal Note_Table As DataTable) As ArrayList
            Dim Class_Array As New ArrayList(Note_Table.Rows)
            Dim Note_Data As New clsLocal_Notes
            Dim Counter = 0
            Create_Note_Array_Class = Class_Array
            For Each r As DataRow In Note_Table.Rows
                Note_Data = New clsLocal_Notes
                Note_Data.lnote_action_date = IIf(Not IsDBNull(r("lnote_action_date")), r("lnote_action_date"), Now())
                Note_Data.lnote_client_ac_id = IIf(Not IsDBNull(r("lnote_client_ac_id")), r("lnote_client_ac_id"), 0)
                Note_Data.lnote_client_amod_id = IIf(Not IsDBNull(r("lnote_client_amod_id")), r("lnote_client_amod_id"), 0)
                Note_Data.lnote_client_comp_id = IIf(Not IsDBNull(r("lnote_client_comp_id")), r("lnote_client_comp_id"), 0)
                Note_Data.lnote_client_contact_id = IIf(Not IsDBNull(r("lnote_client_contact_id")), r("lnote_client_contact_id"), 0)
                Note_Data.lnote_clipri_ID = IIf(Not IsDBNull(r("lnote_clipri_id")), r("lnote_clipri_id"), 0)
                Note_Data.lnote_document_flag = IIf(Not IsDBNull(r("lnote_document_flag")), r("lnote_document_flag"), "N")
                Note_Data.lnote_entry_date = IIf(Not IsDBNull(r("lnote_entry_date")), r("lnote_entry_date"), Now())

                Note_Data.lnote_id = IIf(Not IsDBNull(r("lnote_id")), r("lnote_id"), 0)
                Note_Data.lnote_jetnet_ac_id = IIf(Not IsDBNull(r("lnote_jetnet_ac_id")), r("lnote_jetnet_ac_id"), 0)
                Note_Data.lnote_jetnet_amod_id = IIf(Not IsDBNull(r("lnote_jetnet_amod_id")), r("lnote_jetnet_amod_id"), 0)
                Note_Data.lnote_jetnet_comp_id = IIf(Not IsDBNull(r("lnote_jetnet_comp_id")), r("lnote_jetnet_comp_id"), 0)

                Note_Data.lnote_jetnet_contact_id = IIf(Not IsDBNull(r("lnote_jetnet_contact_id")), r("lnote_jetnet_contact_id"), 0)
                Note_Data.lnote_note = IIf(Not IsDBNull(r("lnote_note")), r("lnote_note"), "")
                Note_Data.lnote_notecat_key = IIf(Not IsDBNull(r("lnote_notecat_key")), r("lnote_notecat_key"), 0)
                Note_Data.lnote_schedule_end_date = IIf(Not IsDBNull(r("lnote_schedule_end_date")), r("lnote_schedule_end_date"), Now())
                Note_Data.lnote_schedule_start_date = IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), Now())
                Note_Data.lnote_document_name = IIf(Not IsDBNull(r("lnote_document_name")), r("lnote_document_name"), "")
                Note_Data.lnote_status = IIf(Not IsDBNull(r("lnote_status")), r("lnote_status"), "")
                Note_Data.lnote_user_id = IIf(Not IsDBNull(r("lnote_user_id")), r("lnote_user_id"), 0)
                Note_Data.lnote_user_login = IIf(Not IsDBNull(r("lnote_user_login")), r("lnote_user_login"), "")
                Note_Data.lnote_user_name = IIf(Not IsDBNull(r("lnote_user_name")), r("lnote_user_name"), "")

                Class_Array(Counter) = Note_Data
                Counter = Counter + 1
            Next
            Create_Note_Array_Class = Class_Array
        End Function

        Public Shared Function Create_Array_Contact_Class(ByVal Contact_Table As DataTable) As ArrayList
            Dim Class_Array As New ArrayList(Contact_Table.Rows)
            Dim Contact_Data As New clsClient_Contact
            Dim Counter As Integer = 0
            For Each r As DataRow In Contact_Table.Rows
                Contact_Data = New clsClient_Contact
                Contact_Data.clicontact_comp_id = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_comp_id")), Contact_Table.Rows(Counter).Item("contact_comp_id"), 0)
                Contact_Data.clicontact_date_updated = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_action_date")), Contact_Table.Rows(Counter).Item("contact_action_date"), Now())
                Contact_Data.clicontact_email_address = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_email_address")), Contact_Table.Rows(Counter).Item("contact_email_address"), "")
                Contact_Data.clicontact_first_name = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_first_name")), Contact_Table.Rows(Counter).Item("contact_first_name"), "")
                Contact_Data.clicontact_id = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_id")), Contact_Table.Rows(Counter).Item("contact_id"), 0)
                Contact_Data.clicontact_last_name = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_last_name")), Contact_Table.Rows(Counter).Item("contact_last_name"), "")
                Contact_Data.clicontact_middle_initial = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_middle_initial")), Contact_Table.Rows(Counter).Item("contact_middle_initial"), "")
                Contact_Data.clicontact_sirname = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_sirname")), Contact_Table.Rows(Counter).Item("contact_sirname"), "")
                Contact_Data.clicontact_suffix = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_suffix")), Contact_Table.Rows(Counter).Item("contact_suffix"), "")
                Contact_Data.clicontact_title = IIf(Not IsDBNull(Contact_Table.Rows(Counter).Item("contact_title")), Contact_Table.Rows(Counter).Item("contact_title"), "")

                Class_Array(Counter) = Contact_Data
                Counter = Counter + 1
            Next


            Create_Array_Contact_Class = Class_Array
        End Function

        Public Shared Function Create_Array_Phone_Class(ByVal Phone_Table As DataTable) As ArrayList
            Dim Class_Array As New ArrayList(Phone_Table.Rows)
            Dim Phone_Data As New clsClient_Phone_Numbers
            Dim Counter As Integer = 0
            For Each r As DataRow In Phone_Table.Rows
                Phone_Data = New clsClient_Phone_Numbers
                Phone_Data.clipnum_number = IIf(Not IsDBNull(Phone_Table.Rows(Counter).Item("pnum_number")), Phone_Table.Rows(Counter).Item("pnum_number"), "")
                Phone_Data.clipnum_type = IIf(Not IsDBNull(Phone_Table.Rows(Counter).Item("pnum_type")), Phone_Table.Rows(Counter).Item("pnum_type"), "")
                Class_Array(Counter) = Phone_Data
                Counter = Counter + 1
            Next
            Create_Array_Phone_Class = Class_Array
        End Function

        Public Shared Function show_phone_display(ByVal Phone_Data As clsClient_Phone_Numbers) As String
            Dim phone_text As String = ""

            phone_text = phone_text & Phone_Data.clipnum_type & ": " & Phone_Data.clipnum_number & "<br />"
            show_phone_display = phone_text
        End Function


        Public Shared Function Show_Contact_Display(ByVal Contact_Data As clsClient_Contact, Optional LinkContactName As Boolean = False) As String
            Dim contact_text As String = ""
            Dim contact_email_address As String = ""

            contact_text = "<b class=""company_title"">"

            If LinkContactName = False Then
                If Contact_Data.clicontact_first_name <> "" Then
                    contact_text += (Contact_Data.clicontact_first_name & " ")
                End If
                If Contact_Data.clicontact_last_name <> "" Then
                    contact_text += (Contact_Data.clicontact_last_name & " ")
                End If
                If Contact_Data.clicontact_suffix <> "" Then
                    contact_text += (Contact_Data.clicontact_suffix & " ")
                End If
            Else
                Dim ContactTextTemp As String = ""
                If Contact_Data.clicontact_first_name <> "" Then
                    ContactTextTemp += (Contact_Data.clicontact_first_name & " ")
                End If
                If Contact_Data.clicontact_last_name <> "" Then
                    ContactTextTemp += (Contact_Data.clicontact_last_name & " ")
                End If
                If Contact_Data.clicontact_suffix <> "" Then
                    ContactTextTemp += (Contact_Data.clicontact_suffix)
                End If

                'App mode
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                    'This is the only time the details page needs to be different.
                    contact_text += "<a href=""details.aspx?contact_ID=" & Contact_Data.clicontact_id & "&comp_ID=" & Contact_Data.clicontact_comp_id & "&type=1&source=source"">" & ContactTextTemp & "</a>"
                Else
                    contact_text += DisplayFunctions.WriteDetailsLink(0, Contact_Data.clicontact_comp_id, Contact_Data.clicontact_id, 0, True, ContactTextTemp, "", "")
                End If

            End If

            If Contact_Data.clicontact_title <> "" Then
                contact_text += ("<br />(<em>" & Contact_Data.clicontact_title & "</em>)")
            End If
            contact_text += "</b>"
            contact_email_address = IIf(Not IsDBNull(Contact_Data.clicontact_email_address), "<a href='mailto:" & Contact_Data.clicontact_email_address & "' class='non_special_link'>" & Contact_Data.clicontact_email_address & "</a>", "")
            If Contact_Data.clicontact_email_address <> "" Then
                contact_text = contact_text & ("<br />" & contact_email_address & "")
            End If
            Show_Contact_Display = contact_text

        End Function

        Public Shared Function Build_Company_Aircraft_Tab(ByVal aTempTable As DataTable, ByVal show_link As Boolean) As Table
            Build_Company_Aircraft_Tab = Nothing
            Dim lbl As New Label
            Dim tbl As New Table
            Dim tr As New TableRow
            Dim td As New TableCell
            Dim linky As New Label

            Dim ac_id As Integer = 0
            Dim comp_id As Integer = 0
            Dim contact_id As Integer = 0
            Dim idtoshow As Integer = 0
            Dim old_id As Integer = 0

            Dim link As String = ""
            Dim color As String = ""
            Dim act_name As String = ""
            Dim ac_source As String = ""
            Dim amod_make_name As String = ""
            Dim contact_sirname As String = ""
            Dim contact_first_name As String = ""
            Dim contact_last_name As String = ""
            Dim ac_year_mfr As String = ""
            Dim ac_ser_nbr As String = ""
            Dim ac_reg_nbr As String = ""
            Dim ac_forsale_flag As String = ""
            Dim ac_status As String = ""
            Dim ac_exclusive_flag As String = ""
            Dim ac_lease_flag As String = ""
            Dim amod_model_name As String = ""
            Dim ac_asking_price As String = ""
            Dim ac_asking_wordage As String = ""
            Dim ac_foresale_flag As String = ""

            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then

                    tbl = New Table
                    tbl.CellPadding = 5
                    tbl.CellSpacing = 0
                    tbl.CssClass = "data_aircraft_grid"
                    'tbl.Width = 822
                    tbl.Width = Unit.Percentage(100D)
                    ' tbl.BorderColor = Drawing.Color.Gray
                    'tbl.BorderWidth = 1
                    'tbl.Width = 900
                    'Setting up the first row!
                    tr = New TableRow
                    td = New TableCell
                    td.Text = "&nbsp;&nbsp;"
                    tr.Cells.Add(td)

                    td = New TableCell
                    td.Text = "Make"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "Model"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)

                    td = New TableCell

                    td.Text = "Year"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)


                    'tr.Cells.Add(td)
                    td = New TableCell

                    td.Text = "Ser #"
                    tr.Cells.Add(td)
                    ' tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "Reg #"
                    tr.Cells.Add(td)


                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                    Else
                        td = New TableCell
                        td.Text = "Status"
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "Asking $"
                        tr.Cells.Add(td)
                    End If


                    td = New TableCell
                    td.Text = "Relationship"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "Contact"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "&nbsp;&nbsp;"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "&nbsp;&nbsp;"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "&nbsp;&nbsp;"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "&nbsp;&nbsp;"
                    tr.Cells.Add(td)

                    tr.CssClass = "header_row" '"aircraft_list"
                    tbl.Rows.Add(tr)
                    color = ""

                    For Each r As DataRow In aTempTable.Rows
                        act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                        ac_id = IIf(Not IsDBNull(r("ac_id")), r("ac_id"), 0)
                        comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                        contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), 0)

                        ac_source = IIf(Not IsDBNull(r("source")), r("source"), "")
                        amod_make_name = IIf(Not IsDBNull(r("amod_make_name")), r("amod_make_name"), "")
                        contact_sirname = IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), "")
                        contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                        contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")

                        ac_year_mfr = IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), "")

                        ac_ser_nbr = IIf(Not IsDBNull(r("ac_ser_nbr")), r("ac_ser_nbr"), "")
                        ac_reg_nbr = IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), "")
                        ac_forsale_flag = IIf(Not IsDBNull(r("ac_forsale_flag")), r("ac_forsale_flag"), "")
                        ac_status = IIf(Not IsDBNull(r("ac_status")), r("ac_status"), "")
                        ac_exclusive_flag = IIf(Not IsDBNull(r("ac_exclusive_flag")), r("ac_exclusive_flag"), "N")
                        ac_lease_flag = IIf(Not IsDBNull(r("ac_lease_flag")), r("ac_lease_flag"), "N")
                        amod_model_name = IIf(Not IsDBNull(r("amod_model_name")), r("amod_model_name"), "")
                        ac_asking_price = IIf(Not IsDBNull(r("ac_asking_price")), r("ac_asking_price"), "")
                        ac_asking_wordage = IIf(Not IsDBNull(r("ac_asking_wordage")), r("ac_asking_wordage"), "")
                        ac_foresale_flag = IIf(Not IsDBNull(r("ac_forsale_flag")), r("ac_forsale_flag"), "")

                        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And act_name = "Exclusive Broker" Then
                        Else
                            If color = "alt_row" Then
                                color = ""
                            Else
                                color = "alt_row"
                            End If
                            tr = New TableRow
                            tr.CssClass = color

                            If ac_id = old_id Then

                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)



                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                Else
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                End If

                                td = New TableCell
                                td.Text = act_name
                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = contact_sirname & " " & contact_first_name & " " & contact_last_name
                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)

                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                Else
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    tbl.Rows.Add(tr)
                                End If
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)

                                tbl.Rows.Add(tr)
                            Else
                                td = New TableCell
                                td.HorizontalAlign = HorizontalAlign.Left
                                td.Text = WhatAmI(ac_source)
                                tr.Cells.Add(td)
                                td = New TableCell


                                If show_link = True Then
                                    linky.Text = "<a href='details.aspx?ac_ID=" & ac_id & "&source=" & ac_source & "&type=3'>" & amod_make_name & "</a>"
                                    td.Controls.Add(linky)
                                Else
                                    td.Text = amod_make_name
                                End If


                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = "&nbsp;"

                                'mini linkbutton to click on!
                                linky = New Label
                                linky.Text = amod_model_name

                                'AddHandler linky.Click, AddressOf dispDetails
                                td.HorizontalAlign = HorizontalAlign.Left
                                td.Controls.Add(linky)
                                linky.Dispose()

                                tr.Cells.Add(td)


                                td = New TableCell
                                td.Text = ac_year_mfr
                                td.HorizontalAlign = HorizontalAlign.Left
                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)

                                td = New TableCell

                                'mini linkbutton to click on!
                                linky = New Label
                                If show_link = True Then
                                    td.Text = "<a href='details.aspx?ac_ID=" & ac_id & "&source=" & ac_source & "&type=3'>" & ac_ser_nbr & "</a>"
                                Else
                                    td.Text = ac_ser_nbr
                                End If

                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.Text = ac_reg_nbr
                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)


                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                    'blocking out the forsale flag if aerodex flag is set
                                Else
                                    td = New TableCell
                                    td.Text = colormestatus(ac_forsale_flag, ac_status)
                                    tr.Cells.Add(td)
                                    td = New TableCell


                                    If ac_forsale_flag = "Y" Then
                                        'If this first value is for sale, then we basically need to figure out if the wordage is Price.
                                        If Trim(ac_asking_wordage) = "Price" Then
                                            'if the asking wordage is price, then the answer for the first value, is price.
                                            td.Text = no_zero(ac_asking_price, "", True)
                                        Else
                                            'if this is not price, then we need to return asking wordage
                                            td.Text = ac_asking_wordage
                                        End If
                                    End If

                                    tr.Cells.Add(td)
                                End If



                                td = New TableCell
                                td.Text = act_name
                                td.HorizontalAlign = HorizontalAlign.Left
                                tr.Cells.Add(td)
                                td = New TableCell
                                td.HorizontalAlign = HorizontalAlign.Left
                                td.Text = contact_sirname & " " & contact_first_name & " " & contact_last_name

                                tr.Cells.Add(td)
                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then

                                Else
                                    td = New TableCell
                                    td.Text = colorme(ac_forsale_flag)
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = colormeex(ac_exclusive_flag, False)
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = colormelease(ac_lease_flag, False)
                                    tr.Cells.Add(td)
                                End If
                                td = New TableCell
                                td.Text = ""
                                tr.Cells.Add(td)

                                tbl.Rows.Add(tr)


                            End If
                        End If
                        old_id = ac_id
                        tr.Dispose()
                        td.Dispose()
                    Next
                    Build_Company_Aircraft_Tab = tbl
                    tbl.Dispose()
                    aTempTable = Nothing
                Else

                End If
            Else

            End If
        End Function
        Public Shared Function Build_Company_Transaction_Tab(ByVal aTempTable As DataTable) As Table
            Build_Company_Transaction_Tab = Nothing
            Dim lbl As New Label
            Dim tbl As New Table
            Dim tr As New TableRow
            Dim td As New TableCell
            Dim linky As New Label
            Dim trans_date As String = ""
            Dim trans_description As String = ""
            Dim trans_document As String = ""
            Dim trans_id As Integer = 0
            Dim idtoshow As Integer = 0
            Dim old_id As Integer = 0

            Dim color As String = ""


            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then



                    tbl = New Table
                    tbl.CellPadding = 5
                    tbl.CellSpacing = 0
                    'tbl.Width = 822
                    tbl.Width = 820
                    'tbl.BorderColor = Drawing.Color.Gray
                    'tbl.BorderWidth = 1
                    'tbl.Width = 900
                    'Setting up the first row!
                    tr = New TableRow


                    td = New TableCell
                    td.Text = "Date"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "Description"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)

                    td = New TableCell

                    td.Text = "Document"
                    td.HorizontalAlign = HorizontalAlign.Left
                    tr.Cells.Add(td)

                    tr.CssClass = "aircraft_list"
                    tbl.Rows.Add(tr)
                    color = ""
                    If aTempTable.Rows.Count > 0 Then
                        old_id = aTempTable.Rows(0).Item("trans_id")
                    End If
                    color = "alt_row"
                    For Each r As DataRow In aTempTable.Rows


                        trans_id = IIf(Not IsDBNull(r("trans_id")), r("trans_id"), 0)
                        trans_date = IIf(Not IsDBNull(r("trans_date")), r("trans_date"), "")
                        trans_description = IIf(Not IsDBNull(r("trans_subject")), r("trans_subject"), "")
                        trans_document = trans_document & IIf(Not IsDBNull(r("tdoc_doc_type")), r("tdoc_doc_type"), "") & "<br />"

                        If trans_id <> old_id Then
                            If color = "alt_row" Then
                                color = ""
                            Else
                                color = "alt_row"
                            End If
                            tr = New TableRow
                            tr.CssClass = color
                            td = New TableCell
                            td.HorizontalAlign = HorizontalAlign.Left
                            td.VerticalAlign = VerticalAlign.Top
                            td.Text = trans_date
                            tr.Controls.Add(td)

                            td = New TableCell
                            td.HorizontalAlign = HorizontalAlign.Left
                            td.VerticalAlign = VerticalAlign.Top
                            td.Text = trans_description
                            tr.Controls.Add(td)

                            td = New TableCell
                            td.HorizontalAlign = HorizontalAlign.Left
                            td.VerticalAlign = VerticalAlign.Top
                            td.Text = trans_document
                            trans_document = ""
                            tr.Controls.Add(td)
                            tbl.Controls.Add(tr)
                        End If

                        old_id = IIf(Not IsDBNull(r("trans_id")), r("trans_id"), 0)
                    Next
                    Build_Company_Transaction_Tab = tbl
                    tbl.Dispose()
                    aTempTable = Nothing
                Else

                End If
            Else

            End If
        End Function

        Public Shared Function Mobile_Build_Company_Aircraft_Tab(ByVal aTempTable As DataTable, ByVal show_link As Boolean) As Table
            Mobile_Build_Company_Aircraft_Tab = Nothing
            Dim lbl As New Label
            Dim tbl As New Table
            Dim tr As New TableRow
            Dim td As New TableCell
            Dim linky As New Label

            Dim ac_id As Integer = 0
            Dim comp_id As Integer = 0
            Dim contact_id As Integer = 0
            Dim idtoshow As Integer = 0
            Dim old_id As Integer = 0

            Dim link As String = ""
            Dim color As String = ""
            Dim act_name As String = ""
            Dim ac_source As String = ""
            Dim amod_make_name As String = ""
            Dim contact_sirname As String = ""
            Dim contact_first_name As String = ""
            Dim contact_last_name As String = ""
            Dim ac_year_mfr As String = ""
            Dim ac_ser_nbr As String = ""
            Dim ac_reg_nbr As String = ""
            Dim ac_forsale_flag As String = ""
            Dim ac_status As String = ""
            Dim ac_exclusive_flag As String = ""
            Dim ac_lease_flag As String = ""
            Dim amod_model_name As String = ""

            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        tbl = New Table
                        tbl.CellPadding = 5
                        tbl.CellSpacing = 0
                        'tbl.Width = 822
                        tbl.Width = Unit.Percentage(100D)
                        tbl.BorderColor = Drawing.Color.Gray
                        tbl.BorderWidth = 1
                        'tbl.Width = 900
                        'Setting up the first row!
                        tr = New TableRow
                        td = New TableCell
                        td.Text = "&nbsp;&nbsp; "
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "Make"
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "Model"

                        td = New TableCell

                        td.Text = "Year"
                        tr.Cells.Add(td)


                        'tr.Cells.Add(td)
                        td = New TableCell

                        td.Text = "Serial #"
                        tr.Cells.Add(td)
                        ' tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "Reg #"
                        tr.Cells.Add(td)



                        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                        Else
                            td = New TableCell
                            td.Text = "Status "
                            tr.Cells.Add(td)
                        End If


                        td = New TableCell
                        td.Text = "Relationship "
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "Contact"
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "&nbsp;&nbsp; "
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "&nbsp;&nbsp; "
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "&nbsp;&nbsp;"
                        tr.Cells.Add(td)
                        td = New TableCell
                        td.Text = "&nbsp;&nbsp;"
                        tr.Cells.Add(td)

                        tr.CssClass = "aircraft_list"
                        tbl.Rows.Add(tr)
                        color = ""

                        For Each r As DataRow In aTempTable.Rows
                            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                            ac_id = IIf(Not IsDBNull(r("ac_id")), r("ac_id"), 0)
                            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), 0)

                            ac_source = IIf(Not IsDBNull(r("source")), r("source"), "")
                            amod_make_name = IIf(Not IsDBNull(r("amod_make_name")), r("amod_make_name"), "")
                            contact_sirname = IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), "")
                            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")

                            ac_year_mfr = IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), "")


                            ac_ser_nbr = IIf(Not IsDBNull(r("ac_ser_nbr")), r("ac_ser_nbr"), "")
                            ac_reg_nbr = IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), "")
                            ac_forsale_flag = IIf(Not IsDBNull(r("ac_forsale_flag")), r("ac_forsale_flag"), "")
                            ac_status = IIf(Not IsDBNull(r("ac_status")), r("ac_status"), "")
                            ac_exclusive_flag = IIf(Not IsDBNull(r("ac_exclusive_flag")), r("ac_exclusive_flag"), "N")
                            ac_lease_flag = IIf(Not IsDBNull(r("ac_lease_flag")), r("ac_lease_flag"), "N")
                            amod_model_name = IIf(Not IsDBNull(r("amod_model_name")), r("amod_model_name"), "")

                            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And act_name = "Exclusive Broker" Then
                            Else
                                If color = "alt_row" Then
                                    color = ""
                                Else
                                    color = "alt_row"
                                End If
                                tr = New TableRow
                                tr.CssClass = color

                                If ac_id = old_id Then

                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)



                                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                    Else
                                        td = New TableCell
                                        td.Text = ""
                                        tr.Cells.Add(td)
                                    End If

                                    td = New TableCell
                                    td.Text = act_name
                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = contact_sirname & " " & contact_first_name & " " & contact_last_name
                                    tr.Cells.Add(td)

                                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                    Else
                                        td = New TableCell
                                        td.Text = ""
                                        tr.Cells.Add(td)
                                        td = New TableCell
                                        td.Text = ""
                                        tr.Cells.Add(td)
                                        td = New TableCell
                                        td.Text = ""
                                        tr.Cells.Add(td)
                                        tbl.Rows.Add(tr)
                                    End If
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)

                                    tbl.Rows.Add(tr)
                                Else
                                    td = New TableCell
                                    td.Text = WhatAmI(ac_source)
                                    tr.Cells.Add(td)
                                    td = New TableCell


                                    If show_link = True Then
                                        linky.Text = "<a href='details.aspx?ac_ID=" & ac_id & "&source=" & ac_source & "&type=3'>" & amod_make_name & "</a>"
                                        td.Controls.Add(linky)
                                    Else
                                        td.Text = amod_make_name
                                    End If


                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = "&nbsp;"

                                    'mini linkbutton to click on!
                                    linky = New Label
                                    linky.Text = amod_model_name

                                    'AddHandler linky.Click, AddressOf dispDetails
                                    td.Controls.Add(linky)
                                    linky.Dispose()

                                    tr.Cells.Add(td)


                                    td = New TableCell
                                    td.Text = ac_year_mfr
                                    tr.Cells.Add(td)

                                    td = New TableCell
                                    td.Text = "&nbsp;"

                                    'mini linkbutton to click on!
                                    linky = New Label
                                    If show_link = True Then
                                        linky.Text = "<a href='details.aspx?ac_ID=" & ac_id & "&source=" & ac_source & "&type=3'>" & ac_ser_nbr & "</a>"
                                    Else
                                        linky.Text = ac_ser_nbr
                                    End If

                                    'AddHandler linky.Click, AddressOf dispDetails
                                    td.Controls.Add(linky)

                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = ac_reg_nbr

                                    tr.Cells.Add(td)


                                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                                        'blocking out the forsale flag if aerodex flag is set
                                    Else
                                        td = New TableCell
                                        td.Text = colormestatus(ac_forsale_flag, ac_status)
                                        tr.Cells.Add(td)
                                    End If


                                    td = New TableCell
                                    td.Text = act_name

                                    tr.Cells.Add(td)
                                    td = New TableCell
                                    td.Text = "&nbsp;"

                                    'mini linkbutton to click on!
                                    linky = New Label
                                    linky.Text = contact_sirname & " " & contact_first_name & " " & contact_last_name
                                    'AddHandler linky.Click, AddressOf dispDetails
                                    td.Controls.Add(linky)

                                    tr.Cells.Add(td)
                                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then

                                    Else
                                        td = New TableCell
                                        td.Text = colorme(ac_forsale_flag)
                                        tr.Cells.Add(td)
                                        td = New TableCell
                                        td.Text = colormeex(ac_exclusive_flag, False)
                                        tr.Cells.Add(td)
                                        td = New TableCell
                                        td.Text = colormelease(ac_lease_flag, False)
                                        tr.Cells.Add(td)
                                    End If
                                    td = New TableCell
                                    td.Text = ""
                                    tr.Cells.Add(td)

                                    tbl.Rows.Add(tr)


                                End If
                            End If
                            old_id = ac_id
                            tr.Dispose()
                            td.Dispose()
                        Next
                        Mobile_Build_Company_Aircraft_Tab = tbl
                        tbl.Dispose()
                        aTempTable = Nothing
                    ElseIf HttpContext.Current.Session.Item("isMobile") = True Then
                        tbl = New Table
                        old_id = 1
                        tbl.CellPadding = 3
                        tbl.CellSpacing = 3
                        tbl.Width = Unit.Percentage(100D)
                        tbl.BorderColor = Drawing.Color.Gray
                        tbl.BorderWidth = 1
                        ' tr.CssClass = "aircraft_list"
                        'td.Text = "Aircraft List"
                        Dim backcolor As Integer = 0
                        ' td.ColumnSpan = 3
                        'tr.Controls.Add(td)
                        'tbl.Controls.Add(tr)
                        tr = New TableRow
                        td = New TableCell
                        Dim rel As String = ""
                        Dim counter As Integer = 0
                        Dim str As String = ""
                        Dim rowcount As Integer = 0
                        Dim repeated As Boolean = False
                        Dim next_id As Integer = 0
                        For Each r As DataRow In aTempTable.Rows

                            td = New TableCell
                            td.Width = Unit.Percentage(50D)


                            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                            ac_id = IIf(Not IsDBNull(r("ac_id")), r("ac_id"), 0)

                            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), 0)

                            ac_source = IIf(Not IsDBNull(r("source")), r("source"), "")
                            amod_make_name = IIf(Not IsDBNull(r("amod_make_name")), r("amod_make_name"), "")
                            contact_sirname = IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), "")
                            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")

                            ac_year_mfr = IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), "")


                            ac_ser_nbr = IIf(Not IsDBNull(r("ac_ser_nbr")), r("ac_ser_nbr"), "")
                            ac_reg_nbr = IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), "")
                            ac_forsale_flag = IIf(Not IsDBNull(r("ac_forsale_flag")), r("ac_forsale_flag"), "")
                            ac_status = IIf(Not IsDBNull(r("ac_status")), r("ac_status"), "")
                            ac_exclusive_flag = IIf(Not IsDBNull(r("ac_exclusive_flag")), r("ac_exclusive_flag"), "N")
                            ac_lease_flag = IIf(Not IsDBNull(r("ac_lease_flag")), r("ac_lease_flag"), "N")
                            amod_model_name = IIf(Not IsDBNull(r("amod_model_name")), r("amod_model_name"), "")
                            If rowcount + 1 < aTempTable.Rows.Count Then
                                next_id = IIf(Not IsDBNull(aTempTable.Rows(rowcount + 1).Item("ac_id")), aTempTable.Rows(rowcount + 1).Item("ac_id"), 0)
                            Else
                                next_id = 0
                            End If
                            If backcolor = 1 Then
                                td.BackColor = Drawing.Color.Cornsilk
                                'Drawing.Color.LightGray
                            ElseIf backcolor = 2 Then
                                td.BackColor = Drawing.Color.GhostWhite
                            ElseIf backcolor = 3 Then
                                td.BackColor = Drawing.Color.Ivory

                                backcolor = 0
                            End If

                            backcolor = backcolor + 1

                            If rowcount + 1 < aTempTable.Rows.Count Then
                                If ac_id <> next_id Then
                                    repeated = True
                                Else
                                    repeated = False
                                End If
                            Else
                                repeated = True
                            End If

                            'If repeated = False Then
                            str = "<a href='Mobile_Details.aspx?ac_ID=" & ac_id & "&source=" & ac_source & "&type=3'>" & amod_make_name & " " & amod_model_name & " Ser #" & ac_ser_nbr & "</a><br />Reg #:" & ac_reg_nbr
                            'End If

                            rel = rel & contact_first_name & " " & contact_last_name & " (" & act_name & ")<br />"

                            'If repeated = False Then
                            str = str & "<br />" & rel
                            'End If

                            If repeated = True Then
                                str = str & colorme(ac_forsale_flag)
                                str = str & colormeex(ac_exclusive_flag, False)
                                str = str & colormelease(ac_lease_flag, False)
                                td.Text = str
                                tr.Controls.Add(td)
                                str = ""
                                rel = ""
                                counter = counter + 1
                                repeated = False
                            End If





                            old_id = IIf(Not IsDBNull(r("ac_id")), r("ac_id"), 0)
                            If counter = 2 Then
                                tbl.Controls.Add(tr)
                                counter = 0
                                tr = New TableRow
                            End If
                            rowcount = rowcount + 1
                        Next
                        tbl.Controls.Add(tr)
                        Mobile_Build_Company_Aircraft_Tab = tbl
                    End If
                Else

                End If
            Else

            End If

        End Function
        Public Shared Function Company_Listing_Address_Display(ByVal address As Object, ByVal city As Object, ByVal state As Object, ByVal country As Object) As String
            Dim return_string As String = ""

            If Not IsDBNull(address) Then
                If Trim(address.ToString) <> "" Then
                    return_string = return_string & address.ToString & "<br />"
                End If
            End If

            If Not IsDBNull(city) Then
                If Trim(city.ToString) <> "" Then
                    return_string = return_string & city.ToString & " "
                End If
            End If

            If Not IsDBNull(state) Then
                If Trim(state.ToString) <> "" Then
                    return_string = return_string & state & " "
                End If
            End If


            If Not IsDBNull(country) Then
                If Trim(country.ToString) <> "" Then
                    return_string = return_string & "<br />" & country.ToString
                End If
            End If
            Company_Listing_Address_Display = return_string

        End Function
        Public Shared Function DisplayDocuments(ByVal document_name As Object, ByVal document_flag As Object, ByVal clicktoview As Boolean, ByVal document_id As Integer) As String
            document_name = IIf(Not IsDBNull(document_name), document_name.ToString, "")
            document_flag = IIf(Not IsDBNull(document_flag), document_flag.ToString, "")
            document_id = IIf(Not IsDBNull(document_id), document_id.ToString, "")

            DisplayDocuments = ""
            If document_name <> "" Then
                'document_name = Replace(document_name, "Documents\", "")

                If InStr(document_name, "http") Then
                    document_name = document_name
                Else
                    If InStr(document_name, "www") Then
                        document_name = "http://" & document_name & ""
                    Else

                        'document_name = "Documents/" & document_name & ""
                    End If
                End If

                If document_flag = "R" Then
                    DisplayDocuments = DisplayDocuments & "<a href='" & document_name & "' target='blank'><img src='images/remote.png' alt='' border='0'>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "' target='blank'>Click to View Remote Document</a><br /><br />"
                    End If
                ElseIf InStr(document_name, ".pdf") Then
                    DisplayDocuments = DisplayDocuments & "<a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'><img src='images/pdf.png' alt='' border='0'>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'>Click to View PDF</a><br /><br />"
                    End If
                ElseIf InStr(document_name, ".ppt") Or InStr(document_name, ".pps") Or InStr(document_name, ".ppsx") Then
                    DisplayDocuments = DisplayDocuments & "<a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'><img src='images/ppt.png' alt='' border='0'>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'>Click to View Powerpoint</a><br /><br />"
                    End If
                ElseIf InStr(document_name, ".jpg") Or InStr(document_name, ".jpeg") Or InStr(document_name, ".png") Or InStr(document_name, ".gif") Then
                    DisplayDocuments = DisplayDocuments & "<a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'><img src='images/picture.png' alt='' border='0'>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'>Click to View Picture</a><br /><br />"
                    End If
                ElseIf InStr(document_name, ".xls") Or InStr(document_name, ".xlsx") Then
                    DisplayDocuments = DisplayDocuments & "<a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'><img src='images/excel.png' alt='' border='0'>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'>Click to View Excel</a><br /><br />"
                    End If
                Else
                    DisplayDocuments = DisplayDocuments & "<a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'><img src='images/doc.png' alt='' border='0'></a>"
                    If clicktoview = True Then
                        DisplayDocuments = DisplayDocuments & " <a href='edit_note.aspx?type=document_display&file=" & document_name & "&id=" & document_id & "'' target='blank'>Click to View Document</a><br /><br />"
                    End If
                End If
            End If
        End Function
        Public Shared Function DisplayDocumentsDescription(ByVal lnote As Object, ByVal lnote_id As Integer) As String
            DisplayDocumentsDescription = ""
            lnote = IIf(Not IsDBNull(lnote), lnote.ToString, "")
            Dim typed As String = ""
            If (InStr(lnote, " ::: ") > 0) Then
                Dim text As Array = Split(lnote, " ::: ")
                If HttpContext.Current.Session.Item("Listing") = "11" Then
                    typed = "opportunity"
                Else
                    typed = "documents"
                End If
                If HttpContext.Current.Session("isMobile") = False Then
                    DisplayDocumentsDescription = "<strong><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&type=" & typed & "&id=" & lnote_id & "','','scrollbars=no,menubar=no,height=500,width=880,resizable=yes,toolbar=no,location=no,status=no');"">" & text(0) & "</a></strong>"
                Else
                    DisplayDocumentsDescription = "<strong><a href='edit_note.aspx?action=edit&type=" & typed & "&id=" & lnote_id & "'>" & text(0) & "</a></strong>"
                End If
                If text(1) <> "" Then
                    DisplayDocumentsDescription = DisplayDocumentsDescription & ": <br />"
                End If
                DisplayDocumentsDescription = DisplayDocumentsDescription & text(1)
            Else
                DisplayDocumentsDescription = lnote.ToString
            End If

        End Function

        Public Shared Function WelcomeMessageHover() As String
            Dim ReturnString As String = ""

            Select Case HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
                Case True
                    ReturnString = "Aerodex User,"
                Case Else
                    ReturnString = "Marketplace"
            End Select

            'Display Frequency
            ReturnString += " " + HttpContext.Current.Session.Item("localSubscription").crmFrequency + "<br />"

            'Display Helicopter Flag:
            ReturnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, " Helicopter,", "")
            'Display Yacht Flag:
            ReturnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag, " Yacht,", "")

            ReturnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag And HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag, "<br />", "")

            'Display Business Flag:
            ReturnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, " Business ", "")

            'Display Business Tier
            If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag Then ' check for tier level
                Select Case (HttpContext.Current.Session.Item("localPreferences").Tierlevel)
                    Case eTierLevelTypes.JETS
                        ReturnString += "(Jets),"
                    Case eTierLevelTypes.TURBOS
                        ReturnString += "(Turbos),"
                    Case Else
                        ReturnString += "(All),"
                End Select
            End If

            'Display Commercial Flag:
            ReturnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, " Commercial ", "")

            If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag Then ' check for tier level

                Select Case (HttpContext.Current.Session.Item("localPreferences").Tierlevel)
                    Case eTierLevelTypes.JETS
                        ReturnString += "(Jets),"
                    Case eTierLevelTypes.TURBOS
                        ReturnString += "(Turbos),"
                    Case Else
                        ReturnString += "(All),"
                End Select

            End If

            ReturnString = ReturnString.TrimEnd(",")

            Return ReturnString
        End Function
        Public Shared Function SettingWelcomeMessage() As String

            Dim results_table As New DataTable
            results_table = crmWebClient.clsSubscriptionClass.getSessionSubscriptionInfo()

            Dim welcome_string As String = ""

            HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag = False
            HttpContext.Current.Session.Item("localSubscription").crmTurboprops = False

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If HttpContext.Current.Session.Item("localUser").crmEvo = False Then
                            Select Case IIf(r.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y"), True, False)
                                Case True
                                    welcome_string = "Aerodex Manager"
                                Case Else
                                    welcome_string = "Marketplace Manager"
                            End Select
                        Else
                            Select Case IIf(r.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y"), True, False)
                                Case True
                                    welcome_string = "Aerodex User,"
                                Case Else
                                    welcome_string = "Marketplace"
                            End Select
                        End If

                        welcome_string += " " + HttpContext.Current.Session.Item("localSubscription").crmFrequency + "<br />"

                        welcome_string += IIf(r.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y"), " Helicopter,", "")
                        HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = IIf(r.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y"), True, False)


                        welcome_string += IIf(r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), " Yacht,", "")


                        HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag = IIf(r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), True, False)

                        welcome_string += IIf(r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") And r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), "<br />", "")

                        welcome_string += IIf(r.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y"), " Business (", "")
                        HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = IIf(r.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y"), True, False)

                        If r.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then ' check for tier level
                            Select Case (r.Item("sub_busair_tier_level").ToString)
                                Case "1"
                                    HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True
                                    welcome_string += "Jets),"
                                Case "2"
                                    HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True
                                    welcome_string += "Turbos),"
                                Case Else
                                    welcome_string += "All),"
                                    HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = False
                                    HttpContext.Current.Session.Item("localSubscription").crmTurboprops = False
                                    HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag = False
                            End Select
                        End If

                        welcome_string += IIf(r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y"), " Commercial (", "")
                        HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = IIf(r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y"), True, False)

                        If r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") Then ' check for tier level

                            Select Case (r.Item("sub_busair_tier_level").ToString)
                                Case "1"
                                    HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True
                                    welcome_string += "Jets),"
                                Case "2"
                                    HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True
                                    welcome_string += "Turbos),"
                                Case Else
                                    welcome_string += "All),"
                                    HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = False
                                    HttpContext.Current.Session.Item("localSubscription").crmTurboprops = False
                                    HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag = False
                            End Select

                        End If

                        welcome_string = welcome_string.TrimEnd(",")

                    Next

                End If

            End If

            'If HttpContext.Current.Session.Item("localUser").crmEvo = False Then
            '  welcome_string += "<br /><a href='#' onclick=""javascript:window.open('" & IIf((HttpContext.Current.Session.Item("localUser").crmEvo = True), "Preferences.aspx", "myCRM.aspx") & "','','scrollbars=yes,menubar=no,height=800,width=800,resizable=yes,toolbar=no,location=no,status=no');"">" & HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName & "</a>"
            'End If

            Return welcome_string

        End Function

        Public Shared Function Get_Document_File_Name(ByVal nAircraftID As Long,
                                                  ByVal nAircraftJournalID As Long,
                                                  ByVal nAircraftJournSeqNo As Integer,
                                                  ByVal sDocType As String,
                                                  ByVal sDocExtension As String,
                                                  ByRef MyAppState As HttpApplicationState,
                                                  ByRef MySesState As HttpSessionState) As String

            Dim sDestinationFileName As String = ""
            Dim sDirName As String = ""
            Dim sDestinationPath As String = ""
            Dim i As Integer = 0

            ' IDENTIFY THE SUBDIRECTORY WHERE THE DOCUMENT IS BASE ON THE ACID

            Select Case Len(nAircraftID.ToString.Trim)
                Case 1, 2, 3  ' THE AIRCRAFT ID MUST BE LESS THAN 1000 SO JUST SET THE DIRECTORY
                    sDirName = "0-999"
                Case 4 ' AIRCRAFT ID MUST BE IN THE THOUSANDS
                    sDirName = Left(nAircraftID.ToString.Trim, 1)
                    For i = 1 To Len(nAircraftID.ToString.Trim) - 1
                        sDirName = sDirName + "0"
                    Next
                    sDirName = sDirName + "-" + Left(nAircraftID.ToString, 1) + "999"
                Case 5 ' AIRCRAFT ID MUST BE IN THE TENS OF THOUSANDS
                    sDirName = Left(nAircraftID.ToString.Trim, 2)
                    For i = 1 To Len(nAircraftID.ToString.Trim) - 2
                        sDirName = sDirName + "0"
                    Next
                    sDirName = sDirName + "-" + Left(nAircraftID.ToString, 2) + "999"
                Case 6  ' AIRCRAFT ID MUST BE IN THE HUNDREDS OF THOUSANDS
                    sDirName = Left(nAircraftID.ToString.Trim, 3)
                    For i = 1 To Len(nAircraftID.ToString.Trim) - 3
                        sDirName = sDirName + "0"
                    Next
                    sDirName = sDirName + "-" + Left(nAircraftID.ToString.Trim, 3) + "999"
                Case 7  ' AIRCRAFT ID MUST BE IN THE MILLIONS
                    sDirName = Left(nAircraftID.ToString.Trim, 4)
                    For i = 1 To Len(nAircraftID.ToString.Trim) - 4
                        sDirName = sDirName + "0"
                    Next
                    sDirName = sDirName + Constants.cHyphen + Left(nAircraftID.ToString.Trim, 4) + "999"
                Case Else ' RETURN A DIRECTORY NAME OF "0" IF THE NUMBER IS BIGGER THAN 7
                    sDirName = "0"
            End Select

            If Not String.IsNullOrEmpty(sDocType) Then

                ' ASSIGN THE FILE NAME BASED ON AC ID AND JOURN ID AND Extension
                ' IF A SEQUENCE NUMBER IS PASSED THEN ADD THIS TO THE FILE NAME
                ' AS WELL

                If CLng(nAircraftJournSeqNo) > 0 Then
                    sDestinationFileName = nAircraftID.ToString + Constants.cHyphen + nAircraftJournalID.ToString + Constants.cHyphen + nAircraftJournSeqNo.ToString + sDocExtension
                Else
                    sDestinationFileName = nAircraftID.ToString + Constants.cHyphen + nAircraftJournalID.ToString + sDocExtension
                End If

                ' ASSIGN THE DIRECTORY TO BE STORED BASED ON THE FILE TYPE
                ' THE ASSIGN DOCUMENT DIRECTORY FUNCTION IS PASSED AN AIRCRAFT ID
                ' AND RETURNS A SUBDIRECTORY GROUPED INTO THOUSANDS WHERE THE DOCUMENT WILL BE STORED

                'If MySesState.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or MyAppState.Item("webHostObject").evoClientHostName.ToString.ToUpper.Contains("JETNET12") Then
                '    ' depending on the nDocType sDestinationPath will change
                '    Select Case (sDocType.ToUpper)

                '        Case "FAAPDF"
                '            sDestinationPath = MySesState.Item("FAAPDFFolderVirtualPath") + Constants.cSingleForwardSlash + sDirName
                '        Case "NTSB"
                '            sDestinationPath = MySesState.Item("NTSBFolderVirtualPath") + Constants.cSingleForwardSlash + sDirName
                '        Case "337"
                '            sDestinationPath = MySesState.Item("337FolderVirtualPath") + Constants.cSingleForwardSlash + sDirName

                '    End Select
                'Else
                sDestinationPath = MySesState.Item("DocumentFolderVirtualPath") + Constants.cSingleForwardSlash + sDocType.ToUpper + Constants.cSingleForwardSlash + sDirName
                'End If

            End If

            ' CREATE THE FULL FILE NAME
            Return Trim(sDestinationPath + "/" + sDestinationFileName)

        End Function
        ''' <summary>
        '''Check existance of show pic cookies
        ''' </summary>
        ''' <param name="sCookieName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getUserDontShowPicCookies(ByVal sCookieName As String) As Boolean

            Dim nCount As Integer
            Dim tmpDontShowPicFlag As Boolean = False

            nCount = 0

            If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName)) Then
                If HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count > 0 Then
                    For nCount = 0 To HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Count - 1
                        If Not IsNothing(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.GetKey(0)) Then
                            If nCount = 0 Then ' only get first cookie
                                tmpDontShowPicFlag = IIf(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Item(0).ToString.ToLower.Trim = "true", True, False)
                            End If
                        Else
                            tmpDontShowPicFlag = IIf(HttpContext.Current.Request.Cookies.Item(sCookieName).Values.Item(0).ToString.ToLower.Trim = "true", False, True)
                        End If
                    Next ' nCount	
                End If ' if Request.cookies(sCookieName).Count > 0 then
            End If

            Return tmpDontShowPicFlag

        End Function

        ''' <summary>
        ''' Please use something like the function below to store info in this field when you put a client company away for any reason.
        ''' </summary>
        ''' <param name="strTemp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Name_Search_String(ByVal strTemp As String) As String
            Dim strResults As String : strResults = ""
            Dim strWork As String : strWork = strTemp
            Dim iTest As Integer
            Dim iZ1 As Integer

            If Len(strTemp) > 0 Then
                'if the length of the string is greater than 0
                strWork = UCase(strTemp)
                'First take the working variable and transform to uppercase.
                For iZ1 = 1 To Len(strTemp)
                    'for 1 to length of string.
                    iTest = Asc(Mid(strWork, iZ1, 1))
                    'Converts each letter to an ascii value
                    If ((iTest >= 65) And (iTest <= 90)) Or
                       ((iTest >= 48) And (iTest <= 57)) Then
                        '65-90 is A-Z
                        '48-57 is 0-9 
                        'Basically it removes everything except A-Z and 0-9
                        strResults = strResults & Mid(strWork, iZ1, 1)
                    End If
                Next iZ1
            End If    ' Len(strTemp) > 0
            'then returns it.
            Get_Name_Search_String = strResults

        End Function

        Public Shared Function Get_Name_Search_String_Preserve_Percents(ByVal strTemp As String) As String
            Dim strResults As String : strResults = ""
            Dim strWork As String : strWork = strTemp
            Dim iTest As Integer
            Dim iZ1 As Integer

            If Len(strTemp) > 0 Then
                'if the length of the string is greater than 0
                strWork = UCase(strTemp)
                'First take the working variable and transform to uppercase.
                For iZ1 = 1 To Len(strTemp)
                    'for 1 to length of string.
                    iTest = Asc(Mid(strWork, iZ1, 1))
                    'Converts each letter to an ascii value
                    If ((iTest >= 65) And (iTest <= 90)) Or
                       ((iTest >= 48) And (iTest <= 57)) Or (iTest = 37) Then
                        '65-90 is A-Z
                        '48-57 is 0-9 
                        '37 = %
                        'Basically it removes everything except A-Z and 0-9
                        'Or percent sign
                        strResults = strResults & Mid(strWork, iZ1, 1)
                    End If
                Next iZ1
            End If    ' Len(strTemp) > 0
            'then returns it.
            Get_Name_Search_String_Preserve_Percents = strResults

        End Function
        Public Shared Function FilterCompanyNameForCompanyAircraftSearch(ByVal TempCompHold As String) As String
            Dim TempNameHold As String = ""
            Dim TemporaryCompanyArray As Array = Split(TempCompHold, ";")
            For TemporaryCompanyArrayCount = 0 To UBound(TemporaryCompanyArray)

                If InStr(TempCompHold, "*") = 0 Then
                    If TempNameHold <> "" Then
                        TempNameHold += ","
                    End If
                    TempNameHold += Get_Name_Search_String_Preserve_Percents(TemporaryCompanyArray(TemporaryCompanyArrayCount))
                Else
                    'replacing comma if needed.
                    If TempNameHold <> "" Then
                        TempNameHold += ","
                    End If

                    Dim WildcardNameHolder As String = ""
                    Dim PreserveWildcard As Array = Split(TemporaryCompanyArray(TemporaryCompanyArrayCount), "*")
                    For PreserveWildcardCount = 0 To UBound(PreserveWildcard)
                        'If the wildcard was put in the middle.
                        If WildcardNameHolder <> "" Then
                            WildcardNameHolder += "*"
                        End If
                        'Putting the pieces back together.
                        WildcardNameHolder += Get_Name_Search_String_Preserve_Percents(PreserveWildcard(PreserveWildcardCount))
                    Next
                    'Adding a catch in case the wildcard was at the very end.
                    If InStr(WildcardNameHolder, "*") = 0 Then
                        WildcardNameHolder = WildcardNameHolder & "*"
                    End If
                    TempNameHold += WildcardNameHolder
                End If
            Next
            Return TempNameHold
        End Function

        ''' <summary>
        ''' Removing everything except digits
        ''' </summary>
        ''' <param name="strTemp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function RemoveNonDigits(ByVal strTemp As String) As String
            Dim strResults As String : strResults = ""
            Dim strWork As String : strWork = strTemp
            Dim iTest As Integer
            Dim iZ1 As Integer

            If Len(strTemp) > 0 Then
                'if the length of the string is greater than 0
                strWork = UCase(strTemp)
                'First take the working variable and transform to uppercase.
                For iZ1 = 1 To Len(strTemp)
                    'for 1 to length of string.
                    iTest = Asc(Mid(strWork, iZ1, 1))
                    'Converts each letter to an ascii value
                    ' If ((iTest >= 65) And (iTest <= 90)) Or _

                    If (iTest = 45) Or ((iTest >= 48) And (iTest <= 57)) Or ((iTest >= 65) And (iTest <= 90)) Then
                        '65-90 is A-Z
                        '48-57 is 0-9 
                        '45 is -
                        'Basically it removes everything except A-Z and 0-9 and -
                        strResults = strResults & Mid(strWork, iZ1, 1)
                    End If
                Next iZ1
            End If    ' Len(strTemp) > 0
            'then returns it.
            RemoveNonDigits = strResults

        End Function

        Public Shared Function getUserAutoLogin() As Boolean
            Dim nCount As Integer
            Dim tmpUserId As String = "''"
            Dim tmpPassWord As String = "''"
            getUserAutoLogin = False 'initialize as false
            nCount = 0

            If Not IsNothing(HttpContext.Current.Request.Cookies.Item("crmUserName")) Then
                If HttpContext.Current.Request.Cookies.Item("crmUserName").Values.Count > 0 Then
                    For nCount = 0 To HttpContext.Current.Request.Cookies.Item("crmUserName").Values.Count - 1
                        If Not IsNothing(HttpContext.Current.Request.Cookies.Item("crmUserName").Values.GetKey(0)) Then
                            If nCount = 0 Then
                                'only perform check for Evo.
                                If HttpContext.Current.Session.Item("localUser").crmEvo = True Then
                                    If HttpContext.Current.Request.Cookies.Item("AutoLogin").Values.Item(0) = True Then
                                        getUserAutoLogin = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

        End Function

        Public Shared Function Notes_Class_Display(ByVal Note_Array As ArrayList) As String
            'For some parameters.. let's set them up.
            Dim TYPE As String = "note" 'Default Note View
            Dim LNOTE_STATUS As String = "A"
            Dim URL_STRING As String = ""
            Dim CAT_KEY As Integer = 0
            Dim DEFAULT_WIDTH As Integer = 300
            Dim UL_CSS_CLASS As String = "notes_list"
            Dim DIV_CSS_CLASS As String = "notes_list_div"
            Dim NOTES_STRING As String = ""
            Dim TYPE_OF_LISTING As Integer = 1
            Dim USED_ID As Integer = 0
            Dim USED_SOURCE As String = ""
            Dim email_to As String = ""
            Dim email_cc As String = ""
            Dim email_subject As String = ""
            Dim body As String = ""
            Dim document_display As String = ""
            NOTES_STRING = ""
            Select Case TYPE
                Case "note"
                    LNOTE_STATUS = "A"
                Case "email"
                    LNOTE_STATUS = "E"
                    URL_STRING = "email"
                Case "action"
                    LNOTE_STATUS = "P"
                    URL_STRING = "&opp=true"
                Case "opportunity"
                    LNOTE_STATUS = "O"
                    URL_STRING = "action"
            End Select

            For Each Note_Data As clsLocal_Notes In Note_Array

                'Special consideration if the listing is a full notes listing. Meaning the width has to be wider on the note views.
                If CAT_KEY = 0 Then
                    DEFAULT_WIDTH = 800
                    UL_CSS_CLASS = "notes_list_no_width"
                    DIV_CSS_CLASS = "notes_list_div_main"
                End If

                If Note_Data.lnote_notecat_key = CAT_KEY Or CAT_KEY = 0 Then 'If the notes category is equal to the category we're looking at, show the note. 

                    If Note_Data.lnote_status = "E" Then
                        Dim info As Array = Split(HttpUtility.HtmlDecode(Note_Data.lnote_note), ":::")

                        If Not IsNothing(info(0)) Then
                            email_to = info(0)
                        End If
                        If Not IsNothing(info(1)) Then
                            email_cc = info(1)
                        End If
                        If Not IsNothing(info(2)) Then
                            email_subject = info(2)
                            Note_Data.lnote_note = info(2)
                        End If
                        If Not IsNothing(info(3)) Then
                            body = info(3)
                        End If
                    End If

                    If Note_Data.lnote_status = "E" Then
                        TYPE = "email"
                        URL_STRING = "email"
                        DIV_CSS_CLASS = "email_list_div_main"
                    End If
                    If Note_Data.lnote_status = "O" Then
                        TYPE = "opportunity"
                        URL_STRING = "opportunity"
                    End If
                    If Note_Data.lnote_status = "P" Then
                        TYPE = "action"
                        URL_STRING = "action"
                    End If

                    If Note_Data.lnote_status = "E" Or Note_Data.lnote_status = "F" Then
                        document_display = DisplayDocuments(Note_Data.lnote_document_name, Note_Data.lnote_document_flag, False, Note_Data.lnote_id)
                    Else
                        document_display = ""
                    End If

                    NOTES_STRING = NOTES_STRING & "<div class='" & DIV_CSS_CLASS & "'>"

                    If HttpContext.Current.Session.Item("isMobile") = True Then
                        NOTES_STRING = NOTES_STRING & "<b><a href='edit_note.aspx?action=edit&amp;type=" & TYPE & "&amp;id=" & Note_Data.lnote_id & "'"">"
                    Else
                        NOTES_STRING = NOTES_STRING & "<b><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=" & TYPE & "&amp;id=" & Note_Data.lnote_id & "','','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"

                    End If
                    If IsDate(Note_Data.lnote_entry_date) And Note_Data.lnote_status <> "P" Then 'This means it's not an action.
                        NOTES_STRING = NOTES_STRING & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_entry_date)
                        NOTES_STRING = NOTES_STRING & "</a> (<em>Entered by: " & Note_Data.lnote_user_name & ")</em> </b> - "

                        If Note_Data.lnote_status = "E" Then
                            NOTES_STRING = NOTES_STRING & " <b><em>For: " & email_to & "</em> </b>  "
                        End If
                    Else
                        If Note_Data.lnote_status <> "P" And Note_Data.lnote_status <> "O" Then 'This means it's an action.
                            NOTES_STRING = NOTES_STRING & " " & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                        Else
                            NOTES_STRING = NOTES_STRING & " " & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                        End If
                    End If

                    If Note_Data.lnote_status = "F" Or Note_Data.lnote_status = "O" Then
                        If (InStr(Note_Data.lnote_note, " ::: ") > 0) Then
                            Dim text As Array = Split(Note_Data.lnote_note, " ::: ")
                            Note_Data.lnote_note = text(1)
                            NOTES_STRING = NOTES_STRING & "<strong style='color: #023657'>" & text(0) & "</strong> "
                        End If
                    End If

                    'Just displaying the notes text field
                    If Len(Note_Data.lnote_note) > 100 Then
                        NOTES_STRING = NOTES_STRING & HttpContext.Current.Server.HtmlDecode(Left(Note_Data.lnote_note, 100) & "...")
                    Else
                        NOTES_STRING = NOTES_STRING & HttpContext.Current.Server.HtmlDecode(Note_Data.lnote_note)
                    End If
                    NOTES_STRING = NOTES_STRING & document_display
                    If TYPE_OF_LISTING <> 3 Then 'This means that this detailed listing which shows the aircraft information
                        'on the note only shows when the listing type isn't an aircraft.
                        USED_ID = IIf(Note_Data.lnote_jetnet_ac_id <> 0, Note_Data.lnote_jetnet_ac_id, Note_Data.lnote_client_ac_id)
                        USED_SOURCE = IIf(Note_Data.lnote_jetnet_ac_id <> 0, "JETNET", "CLIENT")
                        ' If USED_ID <> 0 Then
                        'NOTES_STRING = NOTES_STRING & "<span class='blue_color'>" & add_ac_name(USED_ID, 2, USED_SOURCE) & "</span>"
                        ' End If
                    End If

                End If

                NOTES_STRING = NOTES_STRING & "</div>"

            Next
            Return NOTES_STRING
        End Function
        Public Shared Function Notes_Class_Display_Company_Or_Aircraft(ByVal Note_Array As ArrayList, ByVal typeOfListing As Integer, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal isView As Boolean, ByVal selectedID As Long) As String
            'For some parameters.. let's set them up.
            Dim TYPE As String = "note" 'Default Note View
            Dim LNOTE_STATUS As String = "A"
            Dim URL_STRING As String = ""
            Dim CAT_KEY As Integer = 0
            Dim DEFAULT_WIDTH As Integer = 300
            Dim UL_CSS_CLASS As String = ""
            Dim DIV_CSS_CLASS As String = ""
            Dim NOTES_STRING As String = ""

            Dim USED_ID As Integer = 0
            Dim USED_SOURCE As String = ""
            Dim email_to As String = ""
            Dim email_cc As String = ""
            Dim email_subject As String = ""
            Dim body As String = ""
            Dim document_display As String = ""
            NOTES_STRING = ""
            Select Case TYPE
                Case "note"
                    LNOTE_STATUS = "A"
                Case "email"
                    LNOTE_STATUS = "E"
                    URL_STRING = "email"
                Case "action"
                    LNOTE_STATUS = "P"
                    URL_STRING = "&opp=true"
                Case "opportunity"
                    LNOTE_STATUS = "O"
                    URL_STRING = "action"
            End Select

            For Each Note_Data As clsLocal_Notes In Note_Array

                'Special consideration if the listing is a full notes listing. Meaning the width has to be wider on the note views.
                If CAT_KEY = 0 Then
                    DEFAULT_WIDTH = 800
                    UL_CSS_CLASS = ""
                    DIV_CSS_CLASS = ""
                End If

                If Note_Data.lnote_notecat_key = CAT_KEY Or CAT_KEY = 0 Then 'If the notes category is equal to the category we're looking at, show the note. 

                    If Note_Data.lnote_status = "E" Then
                        Dim info As Array = Split(HttpUtility.HtmlDecode(Note_Data.lnote_note), ":::")

                        If Not IsNothing(info(0)) Then
                            email_to = info(0)
                        End If
                        If Not IsNothing(info(1)) Then
                            email_cc = info(1)
                        End If
                        If Not IsNothing(info(2)) Then
                            email_subject = info(2)
                            Note_Data.lnote_note = info(2)
                        End If
                        If Not IsNothing(info(3)) Then
                            body = info(3)
                        End If
                    End If

                    If Note_Data.lnote_status = "E" Then
                        TYPE = "email"
                        URL_STRING = "email"
                        DIV_CSS_CLASS = "email_list_div_main"
                    End If
                    If Note_Data.lnote_status = "O" Then
                        TYPE = "opportunity"
                        URL_STRING = "opportunity"
                    End If
                    If Note_Data.lnote_status = "P" Then
                        TYPE = "action"
                        URL_STRING = "action"
                    End If

                    If Note_Data.lnote_status = "E" Or Note_Data.lnote_status = "F" Then
                        document_display = DisplayDocuments(Note_Data.lnote_document_name, Note_Data.lnote_document_flag, False, Note_Data.lnote_id)
                    Else
                        document_display = ""
                    End If

                    NOTES_STRING = NOTES_STRING & "<div class='" & DIV_CSS_CLASS & "'>"

                    If HttpContext.Current.Session.Item("isMobile") = True Then
                        NOTES_STRING = NOTES_STRING & "<b><a href='edit_note.aspx?action=edit&amp;type=" & TYPE & IIf(isView, "&amp;refreshing=prospect&amp;ac_ID=" & selectedID, "") & "&amp;id=" & Note_Data.lnote_id & "'"">"
                    Else
                        NOTES_STRING = NOTES_STRING & "<b><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=" & TYPE & "&amp;id=" & Note_Data.lnote_id & IIf(isView, "&amp;refreshing=prospect&amp;ac_ID=" & selectedID, "") & "','','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"

                    End If
                    If IsDate(Note_Data.lnote_entry_date) And Note_Data.lnote_status <> "P" Then 'This means it's not an action.
                        NOTES_STRING = NOTES_STRING & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_entry_date)
                        NOTES_STRING = NOTES_STRING & "</a> (<em>Entered by: " & Note_Data.lnote_user_name & ")</em> </b> - "

                        If Note_Data.lnote_status = "E" Then
                            NOTES_STRING = NOTES_STRING & " <b><em>For: " & email_to & "</em> </b>  "
                        End If
                    Else
                        If Note_Data.lnote_status <> "P" And Note_Data.lnote_status <> "O" Then 'This means it's an action.
                            NOTES_STRING = NOTES_STRING & " " & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                        Else
                            NOTES_STRING = NOTES_STRING & " " & DateAdd("h", HttpContext.Current.Session.Item("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                        End If
                    End If

                    If Note_Data.lnote_status = "F" Or Note_Data.lnote_status = "O" Then
                        If (InStr(Note_Data.lnote_note, " ::: ") > 0) Then
                            Dim text As Array = Split(Note_Data.lnote_note, " ::: ")
                            Note_Data.lnote_note = text(1)
                            NOTES_STRING = NOTES_STRING & "<strong style='color: #023657'>" & text(0) & "</strong> "
                        End If
                    End If

                    'Just displaying the notes text field
                    If Len(Note_Data.lnote_note) > 100 Then
                        NOTES_STRING = NOTES_STRING & HttpContext.Current.Server.HtmlDecode(Left(Note_Data.lnote_note, 100) & "...")
                    Else
                        NOTES_STRING = NOTES_STRING & HttpContext.Current.Server.HtmlDecode(Note_Data.lnote_note)
                    End If
                    NOTES_STRING = NOTES_STRING & document_display
                    If typeOfListing <> 3 Then 'This means that this detailed listing which shows the aircraft information
                        'on the note only shows when the listing type isn't an aircraft.
                        USED_ID = IIf(Note_Data.lnote_jetnet_ac_id <> 0, Note_Data.lnote_jetnet_ac_id, Note_Data.lnote_client_ac_id)
                        USED_SOURCE = IIf(Note_Data.lnote_jetnet_ac_id <> 0, "JETNET", "CLIENT")
                        If USED_ID <> 0 Then
                            NOTES_STRING = NOTES_STRING & "<span>" & add_ac_name(USED_ID, 2, USED_SOURCE, aclsData_Temp) & "</span>"
                        End If
                    End If

                    If typeOfListing <> 1 Then 'This means that this detailed listing which shows the aircraft information
                        'on the note only shows when the listing type isn't an aircraft.
                        USED_ID = IIf(Note_Data.lnote_jetnet_comp_id <> 0, Note_Data.lnote_jetnet_comp_id, Note_Data.lnote_client_comp_id)
                        USED_SOURCE = IIf(Note_Data.lnote_jetnet_comp_id <> 0, "JETNET", "CLIENT")
                        If USED_ID <> 0 Then
                            NOTES_STRING = NOTES_STRING & "<span>" & add_comp_name(USED_ID, 2, USED_SOURCE, aclsData_Temp) & "</span>"
                        End If
                    End If

                End If

                NOTES_STRING = NOTES_STRING & "</div>"

            Next
            Return NOTES_STRING
        End Function

        Public Shared Function add_comp_name(ByVal q As Integer, ByVal show As Integer, ByVal source As String, ByVal aclsData_Temp As clsData_Manager_SQL)
            Dim aTempTable As New DataTable
            Dim typeoflisting As Integer = 3
            Dim Error_String As String = ""
            'This adds the company name for notes and action display
            add_comp_name = ""
            If typeoflisting <> 1 Then
                '---------------------------Aircraft Contact Information-----------------------------------------------------
                Try
                    Dim strContact As String = ""
                    ' get the contact info
                    Dim compID As Integer = q
                    aTempTable = aclsData_Temp.GetCompanyInfo_ID(compID, source, 0)

                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each r As DataRow In aTempTable.Rows
                                If show = 2 Then
                                    strContact = " (<em>"
                                    If Not (IsDBNull(r("comp_name"))) Then
                                        If r("comp_name") <> "" Then
                                            strContact = strContact & "" & r("comp_name") & " "
                                        End If
                                    End If
                                    If Not (IsDBNull(r("comp_city"))) Then
                                        If r("comp_city") <> "" Then
                                            strContact = strContact & r("comp_city") & " "
                                        End If
                                    End If
                                    If Not (IsDBNull(r("comp_state"))) Then
                                        If r("comp_state") <> "" Then
                                            strContact = strContact & r("comp_state") & " "
                                        End If
                                    End If
                                    If Not (IsDBNull(r("comp_country"))) Then
                                        If r("comp_country") <> "" Then
                                            strContact = strContact & r("comp_country")
                                        End If
                                    End If
                                    strContact = " - " & strContact & "</em>)"
                                End If
                                add_comp_name = strContact
                            Next
                        Else ' 0 rows
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            Error_String = aclsData_Temp.class_error
                            LogError("main_site.Master.vb - add_comp_name() - " & Error_String, aclsData_Temp)
                        End If
                    End If
                Catch ex As Exception
                    Error_String = "main_site.Master.vb - add_comp_name() - " & ex.Message
                    LogError(Error_String, aclsData_Temp)
                End Try
            Else
                If show = 2 Then
                    add_comp_name = " (<em>" & source & " Company</em>)"
                End If
            End If

        End Function

        Public Shared Function addACInfo(ByVal idnum As Integer, ByVal show As Integer, ByVal source As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal crmProspectAircraftID As TextBox, ByVal crmProspectViewACSearchButton As Button, ByVal r As DataRow)
            'Dim aTempTable As New DataTable
            Dim Error_String As String = ""
            'This adds the aircraft name for notes and action display
            Dim aircraft_text As String = ""
            Dim typeoflisting As Integer = 1
            Dim aError As String = ""
            Try


                If source = "JETNET" Then
                    'check for flags

                    aircraft_text = ""

                    aircraft_text = aircraft_text & r("amod_make_name") & " " & r("amod_model_name") & "<br />"

                    If Not IsDBNull(r("ac_ser_nbr")) Then
                        If r("ac_ser_nbr") <> "" Then
                            aircraft_text += "Ser #:" & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_nbr"), "emphasisColor", "") & "<br />"
                            aircraft_text += "<br />"
                        End If
                    End If

                    If Not IsDBNull(r("ac_reg_nbr")) Then
                        If r("ac_reg_nbr") <> "" Then
                            aircraft_text = aircraft_text & "Reg #: " & r("ac_reg_nbr")
                        End If
                    End If

                Else

                    aircraft_text = ""
                    aircraft_text = aircraft_text & r("amod_make_name") & " " & r("amod_model_name") & "<br />"

                    If Not IsDBNull(r("ac_ser_nbr")) Then
                        If r("ac_ser_nbr") <> "" Then
                            aircraft_text = aircraft_text & "Ser #:" & DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_nbr"), "emphasisColor", "&source=CLIENT") & "<br />"
                        End If
                    End If

                    If Not IsDBNull(r("ac_reg_nbr")) Then
                        If r("ac_reg_nbr") <> "" Then
                            aircraft_text = aircraft_text & "Reg #: " & r("ac_reg_nbr")
                        End If
                    End If
                End If
            Catch ex As Exception
                Error_String = "main_site.Master.vb - add_ac_name() - " & ex.Message
                LogError(Error_String, aclsData_Temp)
            End Try
            Return aircraft_text
        End Function

        Public Shared Function add_ac_name(ByVal idnum As Integer, ByVal show As Integer, ByVal source As String, ByVal aclsData_Temp As clsData_Manager_SQL)
            Dim aTempTable As New DataTable
            Dim Error_String As String = ""
            'This adds the aircraft name for notes and action display
            add_ac_name = ""
            Dim typeoflisting As Integer = 1
            Try
                If source = "JETNET" Then

                    If typeoflisting <> 3 Then
                        Dim aircraft_text As String = ""
                        Dim aError As String = ""
                        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(idnum, aError)
                        ' check the state of the DataTable
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                For Each R As DataRow In aTempTable.Rows

                                    'check for flags
                                    aircraft_text = ""
                                    If show = 2 Then

                                        aircraft_text = " (<em>"

                                        If Not IsDBNull(R("ac_year_mfr")) Then
                                            If R("ac_year_mfr") <> "" Then
                                                aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                                            End If
                                        End If
                                        aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & " - "
                                        If Not IsDBNull(R("ac_reg_nbr")) Then
                                            If R("ac_reg_nbr") <> "" Then
                                                aircraft_text = aircraft_text & "R/N: " & R("ac_reg_nbr") & " - "
                                            End If
                                        End If
                                        add_ac_name = aircraft_text & "</em>)"
                                    End If

                                    'If show = 1 Then
                                    If Not IsDBNull(R("ac_ser_nbr")) Then
                                        If R("ac_ser_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "S/N:" & R("ac_ser_nbr") & "</em>)"
                                        End If
                                    End If
                                    'End If
                                    add_ac_name = aircraft_text
                                Next
                            Else ' 0 rows
                            End If
                        Else
                            If aclsData_Temp.class_error <> "" Then
                                Error_String = aclsData_Temp.class_error
                                LogError("main_site.Master.vb - add_ac_name() - " & Error_String, aclsData_Temp)
                            End If
                        End If
                    Else
                        If show = 2 Then
                            add_ac_name = " (<em>" & source & " AC</em>)"
                        End If
                    End If
                Else
                    If typeoflisting <> 3 Then
                        Dim aircraft_text As String = ""
                        Dim aError As String = ""
                        aTempTable = aclsData_Temp.Get_Clients_Aircraft(idnum)
                        ' check the state of the DataTable
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                For Each R As DataRow In aTempTable.Rows
                                    aircraft_text = ""
                                    If show = 2 Then
                                        aircraft_text = " (<em>"

                                        If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                                            If R("cliaircraft_year_mfr") <> "" Then
                                                aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & " "
                                            End If
                                        End If

                                        aircraft_text = aircraft_text & R("cliamod_make_name") & " " & R("cliamod_model_name") & " - "
                                        If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                                            If R("cliaircraft_reg_nbr") <> "" Then
                                                aircraft_text = aircraft_text & "R/N: " & R("cliaircraft_reg_nbr") & "  "
                                            End If
                                        End If
                                    End If

                                    If show = 1 Then
                                        If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                                            If R("cliaircraft_ser_nbr") <> "" Then

                                                aircraft_text = aircraft_text & "S/N:" & R("cliaircraft_ser_nbr") & "</em>)"
                                            End If
                                        End If
                                    End If
                                    add_ac_name = aircraft_text
                                Next
                            Else ' 0 rows
                            End If
                        Else
                            If aclsData_Temp.class_error <> "" Then
                                Error_String = aclsData_Temp.class_error
                                LogError("main_site.Master.vb - add_ac_name() - " & Error_String, aclsData_Temp)
                            End If
                        End If
                    Else
                        If show = 2 Then
                            add_ac_name = " (<em>" & source & " AC</em>)"
                        End If
                    End If
                End If
            Catch ex As Exception
                Error_String = "main_site.Master.vb - add_ac_name() - " & ex.Message
                LogError(Error_String, aclsData_Temp)
            End Try
        End Function
        Public Shared Function DisplayAircraftName(ByVal idnum As Integer, ByVal source As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal endLink As Boolean)
            Dim aTempTable As New DataTable
            Dim Error_String As String = ""
            'This adds the aircraft name for notes and action display
            Dim aircraft_text As String = ""
            Dim typeoflisting As Integer = 1
            Try
                If source = "JETNET" Then
                    Dim aError As String = ""
                    aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(idnum, aError)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In aTempTable.Rows

                                'check for flags
                                aircraft_text = ""

                                If Not IsDBNull(R("ac_year_mfr")) Then
                                    If R("ac_year_mfr") <> "" Then
                                        aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                                    End If
                                End If
                                aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & " - "
                                If Not IsDBNull(R("ac_reg_nbr")) Then
                                    If R("ac_reg_nbr") <> "" Then
                                        aircraft_text = aircraft_text & "R/N: " & R("ac_reg_nbr") & " - "
                                    End If
                                End If

                                If endLink Then
                                    aircraft_text &= "</a>"
                                End If

                                If Not IsDBNull(R("ac_ser_nbr")) Then
                                    If R("ac_ser_nbr") <> "" Then
                                        aircraft_text &= "S/N: "

                                        aircraft_text = aircraft_text & "<span title=""View Aircraft"">" & DisplayFunctions.WriteDetailsLink(idnum, 0, 0, 0, True, UCase(R("ac_ser_nbr")), "noCase emphasisColor text_underline", "&SOURCE=" & source) & "</span>"
                                    End If
                                End If
                            Next
                        End If
                    End If

                Else

                    Dim aError As String = ""
                    aTempTable = aclsData_Temp.Get_Clients_Aircraft(idnum)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In aTempTable.Rows
                                aircraft_text = ""

                                If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                                    If R("cliaircraft_year_mfr") <> "" Then
                                        aircraft_text = aircraft_text & R("cliaircraft_year_mfr") & " "
                                    End If
                                End If

                                aircraft_text = aircraft_text & R("cliamod_make_name") & " " & R("cliamod_model_name") & " - "
                                If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                                    If R("cliaircraft_reg_nbr") <> "" Then
                                        aircraft_text = aircraft_text & "R/N: " & R("cliaircraft_reg_nbr") & "  "
                                    End If
                                End If

                                If endLink Then
                                    aircraft_text &= "</a>"
                                End If
                                If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                                    If R("cliaircraft_ser_nbr") <> "" Then
                                        aircraft_text = aircraft_text & "S/N: <span title=""View Aircraft"">" & DisplayFunctions.WriteDetailsLink(idnum, 0, 0, 0, True, UCase(R("cliaircraft_ser_nbr")), "noCase text_underline emphasisColor", "&SOURCE=" & source) & "</span>"
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                Error_String = "main_site.Master.vb - add_ac_name() - " & ex.Message
                LogError(Error_String, aclsData_Temp)
            End Try
            Return aircraft_text
        End Function
        Public Shared Function Save_Folder_Action(ByVal flist As CheckBoxList, ByVal jetnet_ac_id As Integer, ByVal jetnet_comp_id As Integer, ByVal jetnet_contact_id As Integer, ByVal client_ac_id As Integer, ByVal client_comp_id As Integer, ByVal client_contact_id As Integer, ByVal aclsData_Temp As clsData_Manager_SQL) As Integer
            Dim itemcount As Integer = 0
            Dim ids As Array
            Dim errored As String = ""
            itemcount = flist.Items.Count

            For i = 0 To (itemcount - 1)
                If flist.Items(i).Selected Then
                    ids = Split(flist.Items(i).Value, "|")

                    If UBound(ids) = 1 Then
                        If aclsData_Temp.Delete_Client_Folder_Index(CInt(ids(1)), CInt(ids(0))) = 1 Then
                        End If
                    End If

                    If aclsData_Temp.Insert_Into_Client_Folder_Index(ids(0), jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, 0, errored) = 1 Then
                        Save_Folder_Action = 1
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            Save_Folder_Action = 0
                        End If
                    End If
                Else ' delete the unselected ones if they exist
                    ids = Split(flist.Items(i).Value, "|")

                    If UBound(ids) = 1 Then
                        If aclsData_Temp.Delete_Client_Folder_Index(CInt(ids(1)), CInt(ids(0))) = 1 Then
                        End If
                    End If
                End If
            Next i
        End Function

        Public Shared Function CreateCheckboxList(ByVal aTempTable As DataTable, ByVal id As String, ByVal jetnet_ac_id As Integer, ByVal jetnet_comp_id As Integer, ByVal jetnet_contact_id As Integer, ByVal client_ac_id As Integer, ByVal client_comp_id As Integer, ByVal client_contact_id As Integer, ByVal aclsData_Temp As clsData_Manager_SQL) As CheckBoxList
            CreateCheckboxList = New CheckBoxList
            CreateCheckboxList.TextAlign = TextAlign.Right
            CreateCheckboxList.ID = id
            CreateCheckboxList.CssClass = "CLIENTCRMRowCheckBox"
            Dim fcheck As New ListItem
            Dim fval As String = ""
            Dim aTempTable2 As New DataTable
            For Each r As DataRow In aTempTable.Rows
                If r("cfolder_method").ToString <> "A" Then 'You cannot add to an active folder this way
                    fval = ""
                    fcheck = New ListItem

                    fcheck.Text = r("cfolder_name")
                    fcheck.Selected = False
                    aTempTable2 = aclsData_Temp.Get_ClientFolderIndex_Search(r("cfolder_id"), jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id)
                    If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                            fcheck.Selected = True
                            fval = "|" & aTempTable2.Rows(0).Item("cfoldind_id")
                        End If
                    Else
                        CreateCheckboxList = Nothing
                    End If
                    fcheck.Value = r("cfolder_id") & fval
                    CreateCheckboxList.Items.Add(fcheck)
                End If
            Next
            Return CreateCheckboxList
        End Function
        Public Shared Function Set_Folder_Editing(ByVal crmSource As String, ByVal jetnet_comp_id As Long, ByVal client_comp_id As Long, ByVal jetnet_contact_id As Long, ByVal client_contact_id As Long, ByVal jetnet_ac_id As Long, ByVal client_ac_id As Long, ByVal Sort As Integer, ByVal aclsData_Temp As clsData_Manager_SQL) As Label
            Dim folders_display As New Label
            Try
                'Dim masterpage As Object
                'If Not IsNothing(mob) Then
                '  masterpage = New crmWebClient.Mobile
                '  masterpage = mob
                'Else
                '  masterpage = New crmWebClient.main_site
                '  masterpage = main
                'End If
                Dim folder_display As New Panel
                Dim atemptable As New DataTable
                'Dim jetnet_ac_id As Integer = 0
                'Dim jetnet_comp_id As Integer = 0
                'Dim jetnet_contact_id As Integer = 0
                'Dim client_ac_id As Integer = 0
                'Dim client_comp_id As Integer = 0
                'Dim client_contact_id As Integer = 0
                Dim cfolder_id As Integer = 0
                Dim fval As String = ""

                'Select Case masterpage.TypeOfListing
                '  Case 1
                '    Select Case masterpage.ListingSource
                '      Case "JETNET"
                '        jetnet_comp_id = masterpage.ListingID
                '      Case "CLIENT"
                '        client_comp_id = masterpage.ListingID
                '    End Select
                '    If masterpage.Listing_ContactID <> 0 Then
                '      Select Case masterpage.ListingSource
                '        Case "JETNET"
                '          jetnet_comp_id = 0
                '          jetnet_contact_id = masterpage.Listing_ContactID
                '        Case "CLIENT"
                '          client_comp_id = 0
                '          client_contact_id = masterpage.Listing_ContactID
                '      End Select
                '    End If
                '  Case 3
                '    Select Case masterpage.ListingSource
                '      Case "JETNET"
                '        jetnet_ac_id = masterpage.ListingID
                '      Case "CLIENT"
                '        client_ac_id = masterpage.ListingID
                '    End Select
                'End Select

                'Dim sort As Integer = 0
                'If masterpage.Listing_ContactID = 0 Then
                '  sort = masterpage.TypeOfListing
                'Else
                '  sort = 2
                'End If

                Dim folder_table As New Table
                'folder_table.CssClass = "card_overflow_long"
                Dim folder_row As New TableRow
                Dim folder_cell As New TableCell
                Dim folder_count As Integer = 0
                Dim sharedTable As DataTable
                folder_table.CssClass = "formatTable blue small"
                folder_cell.Text = "<p align='center'>Your folders have been updated.</p>"
                folder_cell.ID = "newly_updated"
                folder_cell.Visible = False
                folder_cell.Font.Bold = True
                folder_cell.ForeColor = Drawing.Color.Red
                folder_cell.ColumnSpan = 2
                folder_row.Controls.Add(folder_cell)
                folder_row.CssClass = " noBorder"
                folder_table.Controls.Add(folder_row)



                folder_row = New TableRow
                folder_cell = New TableCell
                folder_row.CssClass = "header_row noBorder"
                folder_cell.Width = 200
                folder_cell.CssClass = "mediumText uppercase"
                folder_cell.VerticalAlign = VerticalAlign.Top
                folder_cell.Font.Bold = True
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Else
                    folder_cell.Text = "PERSONAL FOLDERS"
                End If

                folder_cell.VerticalAlign = VerticalAlign.Top
                folder_row.Controls.Add(folder_cell)


                folder_cell = New TableCell
                folder_cell.Width = 200
                folder_cell.CssClass = "mediumText uppercase"
                folder_cell.VerticalAlign = VerticalAlign.Top
                folder_cell.Font.Bold = True

                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Else
                    folder_cell.Text = "SHARED FOLDERS"
                End If

                folder_cell.Width = 200

                folder_row.Controls.Add(folder_cell)
                folder_table.Controls.Add(folder_row)


                folder_row = New TableRow
                folder_cell = New TableCell

                sharedTable = aclsData_Temp.Get_Client_Folders_Shared("Y", Sort, False)
                If Not IsNothing(sharedTable) Then
                    If sharedTable.Rows.Count = 0 Then
                        folder_count = folder_count + 1
                    End If
                End If

                'Figuring out the list of folders.
                'personal folders
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                    Dim UserTableCheck As DataTable
                    UserTableCheck = aclsData_Temp.Get_Client_User_By_Email_Address(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress)
                    If Not IsNothing(UserTableCheck) Then
                        atemptable = aclsData_Temp.Get_Client_Folders_NonShared(CInt(UserTableCheck.Rows(0).Item("cliuser_id")), "N", Sort, False)
                    End If
                Else
                    atemptable = aclsData_Temp.Get_Client_Folders_NonShared(CInt(HttpContext.Current.Session.Item("localUser").crmLocalUserID), "N", Sort, False)
                End If


                If Not IsNothing(atemptable) Then
                    If atemptable.Rows.Count > 0 Then
                        folder_cell = New TableCell
                        folder_cell.VerticalAlign = VerticalAlign.Top
                        folder_cell.Controls.Add(CreateCheckboxList(atemptable, "personal_folder_ids", jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, aclsData_Temp))
                    Else
                        Dim flist As New CheckBoxList
                        flist.ID = "personal_folder_ids"
                        folder_cell.Controls.Add(flist)
                        Dim no_folder As New Label
                        no_folder.Text = "<p align='left'>There are no Personal Folders currently.</p>"
                        no_folder.ForeColor = Drawing.Color.Red
                        no_folder.Font.Bold = True
                        folder_cell.Controls.Add(no_folder)
                        folder_count = folder_count + 1
                    End If

                    If folder_count < 2 Then
                        folder_row.Controls.Add(folder_cell)
                        folder_table.Controls.Add(folder_row)
                        folders_display.Controls.Add(folder_table)
                    ElseIf folder_count = 2 Then
                        folders_display.ForeColor = Drawing.Color.Red
                        folders_display.Font.Bold = True
                        If Sort = 1 Then
                            folders_display.Text = "<p align='center'>There are no user Company Folders currently.</p>"
                            If jetnet_contact_id <> 0 Or client_contact_id <> 0 Then
                                folders_display.Text = "<p align='center'>There are no user Contact Folders currently.</p>"
                            End If
                        Else
                            folders_display.Text = "<p align='center'>There are no user Aircraft Folders currently.</p>"
                        End If

                    End If
                End If

                'Figuring out the list of folders.
                'shared folders

                If Not IsNothing(sharedTable) Then
                    If sharedTable.Rows.Count > 0 Then
                        folder_cell = New TableCell
                        folder_cell.VerticalAlign = VerticalAlign.Top
                        folder_cell.Controls.Add(CreateCheckboxList(sharedTable, "folder_ids", jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, aclsData_Temp))
                    Else
                        Dim flist As New CheckBoxList
                        flist.ID = "folder_ids"
                        folder_cell.Controls.Add(flist)
                        Dim no_folder As New Label
                        no_folder.Text = "<p align='left'>There are no Shared Folders currently.</p>"
                        no_folder.ForeColor = Drawing.Color.Red
                        no_folder.Font.Bold = True
                        folder_cell.Controls.Add(no_folder)
                        folder_count = folder_count + 1
                    End If
                End If
                folder_row.Controls.Add(folder_cell)
                ' Set_Folder_Editing = folders_display
            Catch ex As Exception

            End Try
            Set_Folder_Editing = folders_display
        End Function

        Public Shared Function fill_Contact_Info_AC(ByVal idnum As Integer, ByVal source As String, ByVal parent As Integer, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal memory_table As DataTable) As Table
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Dim counter As Integer = 0
            Dim acref_contact_type As String = ""
            Dim act_name As String = ""
            Dim acref_owner_percentage As Double = 0
            Dim comp_id As Integer = 0
            Dim contact_id As Integer = 0
            Dim comp_name As String = ""
            Dim contact_first_name As String = ""
            Dim contact_title As String = ""
            Dim contact_last_name As String = ""
            Dim comp_city As String = ""
            Dim comp_state As String = ""
            Dim comp_country As String = ""
            Dim strContact As String = ""
            Dim cliacref_contact_priority As Integer = 0
            Dim acref_id As Integer = 0
            Dim cell_text As New Label
            Dim ac_contact As New Table
            ac_contact.Width = Unit.Percentage(100)
            Dim atemptable As New DataTable
            '---------------------------Aircraft Contact Information-----------------------------------------------------
            Try
                'Get the added contacts in the client database. 


                If Not IsNothing(memory_table) Then
                    atemptable = DirectCast(memory_table, DataTable)
                Else
                    If source = "CLIENT" Then
                        atemptable = aclsData_Temp.Get_Aircraft_Reference_Client_acID_Full_Details(idnum)
                    Else
                        atemptable = aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(idnum)
                    End If
                End If

                'System.Web.UI.Page()
                'ViewState("Aircraft_Contacts") = atemptable

                If Not IsNothing(atemptable) Then
                    If atemptable.Rows.Count > 0 Then
                        For Each r As DataRow In atemptable.Rows

                            counter = counter + 1
                            strContact = ""
                            acref_contact_type = IIf(Not IsDBNull(r("acref_contact_type")), r("acref_contact_type"), 0)
                            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                            acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
                            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                            comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
                            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), "")
                            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                            contact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
                            comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                            comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                            comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                            cliacref_contact_priority = IIf(Not IsDBNull(r("cliacref_contact_priority")), r("cliacref_contact_priority"), 0)
                            acref_id = IIf(Not IsDBNull(r("acref_id")), r("acref_id"), 0)

                            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And r("acref_contact_type") = "99" Then

                            Else
                                Dim ro As New TableRow
                                Dim itz As New TableCell
                                cell_text = New Label
                                itz.VerticalAlign = VerticalAlign.Top
                                ro.VerticalAlign = VerticalAlign.Top
                                ro.Height = 20

                                strContact = ""

                                If acref_contact_type = "99" Then
                                    strContact = "<span class='small_purple'>Exclusive Broker</span>"
                                ElseIf acref_contact_type = "12" Then
                                    strContact = "<span class='small_orange'>Lessee</span>"
                                Else
                                    strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                                    If acref_contact_type = "8" Or acref_contact_type = "97" Then
                                        If acref_owner_percentage <> 0 Then
                                            strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                                        End If
                                    End If
                                End If

                                strContact = strContact & "<br /><a href='#' onClick=""javascript:var test = confirm('Are you sure you want to delete this contact?');if (test){load('edit.aspx?action=reference&remove=true&id=" & acref_id & "','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');return false;}{return false;};"" style='text-decoration:none;'><span class='tiny_tiny'>(remove)</span></a><br />"

                                cell_text = New Label
                                itz = New TableCell
                                itz.Width = 70
                                cell_text.Text = strContact
                                itz.Controls.Add(cell_text)
                                ro.Cells.Add(itz)

                                cell_text = New Label
                                itz = New TableCell
                                cell_text.Text = "&nbsp;&nbsp;&nbsp;&nbsp;"
                                itz.Controls.Add(cell_text)
                                ro.Cells.Add(itz)

                                strContact = "<a href='mobile_details.aspx?comp_ID=" & comp_id & "&source=CLIENT&type=1' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"


                                If comp_city <> "" Then
                                    strContact = strContact & comp_city & " "
                                End If
                                If comp_state <> "" Then
                                    strContact = strContact & comp_state & " "
                                End If
                                If comp_country <> "" Then
                                    strContact = strContact & comp_country
                                End If
                                strContact = strContact & "</span>"
                                If contact_id <> 0 Then
                                    strContact = strContact & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=CLIENT&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</a>"
                                    If Trim(contact_title) <> "" Then
                                        strContact = strContact & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                    End If
                                End If

                                strContact = strContact & "<br />"
                                If cliacref_contact_priority = 1 Then
                                    '  strContact = strContact & " <strong class='smaller'>PRIMARY</strong> &nbsp;"
                                ElseIf cliacref_contact_priority = 2 Then
                                    ' strContact = strContact & " <strong>SECONDARY</strong> &nbsp;"
                                Else
                                    ' strContact = strContact & " <strong>OTHER</strong> &nbsp;"
                                End If

                                cell_text = New Label
                                itz = New TableCell
                                cell_text.Text = strContact
                                itz.Controls.Add(cell_text)
                                itz.Width = 350
                                ro.Cells.Add(itz)
                                ac_contact.Rows.Add(ro)
                            End If
                        Next

                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = aclsData_Temp.class_error
                        masterpage.LogError("Clsgeneral.vb - Fill_Contact_Info_AC() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If



                ' get the contact info
                If source = "JETNET" Then

                    atemptable = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(idnum, 0)
                    If Not IsNothing(atemptable) Then
                        If atemptable.Rows.Count > 0 Then
                            For Each r As DataRow In atemptable.Rows
                                '
                                counter = counter + 1
                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And r("act_name") = "Exclusive Broker" Then

                                Else
                                    Dim ro As New TableRow
                                    Dim itz As New TableCell
                                    ro.VerticalAlign = VerticalAlign.Top

                                    itz.VerticalAlign = VerticalAlign.Top

                                    cell_text = New Label
                                    act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                                    acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
                                    comp_id = IIf(Not IsDBNull(r("acref_comp_id")), r("acref_comp_id"), 0)
                                    comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
                                    contact_id = IIf(Not IsDBNull(r("acref_contact_id")), r("acref_contact_id"), "")
                                    contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                                    contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                                    comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                                    comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                                    comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                                    strContact = ""

                                    strContact = ""
                                    If act_name = "Exclusive Broker" Then
                                        strContact = strContact & "<b class='small_purple'>" & act_name & "</b>"
                                    ElseIf act_name = "Lessee" Then
                                        strContact = "<span class='small_orange'>Lessee</span>"
                                    ElseIf act_name <> "" Then
                                        strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                                        If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                                            strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                                        End If
                                    End If

                                    cell_text = New Label
                                    itz = New TableCell
                                    itz.Width = 20
                                    cell_text.Text = strContact
                                    itz.Controls.Add(cell_text)
                                    ro.Cells.Add(itz)

                                    cell_text = New Label
                                    itz = New TableCell
                                    itz.Width = 20
                                    cell_text.Text = "&nbsp;&nbsp;"
                                    itz.Controls.Add(cell_text)
                                    ro.Cells.Add(itz)
                                    strContact = ""

                                    If comp_name <> "" Then
                                        strContact = strContact & "<a href='mobile_details.aspx?comp_ID=" & comp_id & "&type=1&source=JETNET' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"
                                    End If

                                    If comp_city <> "" Then
                                        strContact = strContact & comp_city & " "
                                    End If
                                    If comp_state <> "" Then
                                        strContact = strContact & comp_state & " "
                                    End If
                                    If comp_country <> "" Then
                                        strContact = strContact & comp_country
                                    End If

                                    strContact = strContact & "</span>"
                                    If contact_first_name <> "" Then
                                        strContact = strContact & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em>"
                                        If contact_title <> "" Then
                                            strContact = strContact & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                        End If
                                    End If


                                    cell_text = New Label
                                    itz = New TableCell
                                    cell_text.Text = strContact
                                    itz.Controls.Add(cell_text)
                                    ro.Cells.Add(itz)
                                    ac_contact.Rows.Add(ro)

                                End If
                            Next
                        End If
                        ' dump the datatable
                        atemptable.Dispose()
                        atemptable = Nothing
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = aclsData_Temp.class_error
                            masterpage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                End If
            Catch
            End Try

            Return ac_contact
        End Function

        Public Shared Function New_fill_Contact_Info_AC(ByVal idnum As Integer, ByVal source As String, ByVal parent As Integer, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal memory_table As DataTable) As Table
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Dim counter As Integer = 0
            Dim acref_contact_type As String = ""
            Dim act_name As String = ""
            Dim acref_owner_percentage As Double = 0
            Dim comp_id As Integer = 0
            Dim contact_id As Integer = 0
            Dim comp_name As String = ""
            Dim contact_first_name As String = ""
            Dim contact_title As String = ""
            Dim contact_last_name As String = ""
            Dim comp_city As String = ""
            Dim comp_state As String = ""
            Dim comp_country As String = ""
            Dim strContact As String = ""
            Dim cliacref_contact_priority As Integer = 0
            Dim acref_id As Integer = 0
            Dim cell_text As New Label
            Dim ac_contact As New Table
            Dim next_comp As Integer = 0
            Dim repeated As Boolean = False
            ac_contact.Width = Unit.Percentage(100)
            Dim rowcount As Integer = 0
            Dim hold_rel As String = ""
            Dim atemptable As New DataTable
            Dim hold_con As String = ""
            Dim next_id As Integer = 0
            '---------------------------Aircraft Contact Information-----------------------------------------------------
            Try
                'Get the added contacts in the client database. 


                If Not IsNothing(memory_table) Then
                    atemptable = DirectCast(memory_table, DataTable)
                Else
                    If source = "CLIENT" Then
                        atemptable = aclsData_Temp.Get_Aircraft_Reference_Client_acID_Full_Details(idnum)
                    Else
                        atemptable = aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(idnum)
                    End If
                End If

                'System.Web.UI.Page()
                'ViewState("Aircraft_Contacts") = atemptable

                If Not IsNothing(atemptable) Then
                    If atemptable.Rows.Count > 0 Then
                        rowcount = 0

                        Dim afileterd As DataRow()
                        afileterd = atemptable.Select("", "comp_id asc")
                        Dim filteredTable As DataTable = atemptable.Clone
                        ' a single data row for importing to the dalTable
                        ' extract and import
                        For Each atmpDataRow In afileterd
                            filteredTable.ImportRow(atmpDataRow)
                        Next


                        For Each r As DataRow In filteredTable.Rows

                            counter = counter + 1
                            strContact = ""
                            acref_contact_type = IIf(Not IsDBNull(r("acref_contact_type")), r("acref_contact_type"), 0)
                            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                            acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
                            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
                            comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
                            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), "")
                            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                            contact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
                            comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                            comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                            comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                            cliacref_contact_priority = IIf(Not IsDBNull(r("cliacref_contact_priority")), r("cliacref_contact_priority"), 0)
                            acref_id = IIf(Not IsDBNull(r("acref_id")), r("acref_id"), 0)
                            If rowcount + 1 < filteredTable.Rows.Count Then
                                next_id = IIf(Not IsDBNull(filteredTable.Rows(rowcount + 1).Item("comp_id")), filteredTable.Rows(rowcount + 1).Item("comp_id"), 0)
                            Else
                                next_id = 0
                            End If

                            If rowcount + 1 < atemptable.Rows.Count Then
                                If comp_id <> next_id Then
                                    repeated = True
                                Else
                                    repeated = False
                                End If
                            Else
                                repeated = True
                            End If


                            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And r("acref_contact_type") = "99" Then

                            Else
                                If repeated = True Then
                                    Dim ro As New TableRow
                                    Dim itz As New TableCell
                                    'cell_text = New Label
                                    'itz.VerticalAlign = VerticalAlign.Top
                                    'ro.VerticalAlign = VerticalAlign.Top
                                    'ro.Height = 20

                                    'strContact = ""



                                    'If acref_contact_type = "99" Then
                                    '    strContact = "<span class='small_purple'>Exclusive Broker</span>"
                                    'ElseIf acref_contact_type = "12" Then
                                    '    strContact = "<span class='small_orange'>Lessee</span>"
                                    'Else
                                    '    strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                                    '    If acref_contact_type = "8" Or acref_contact_type = "97" Then
                                    '        If acref_owner_percentage <> 0 Then
                                    '            strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                                    '        End If
                                    '    End If
                                    'End If

                                    'strContact = strContact & "<br /><a href='#' onClick=""javascript:var test = confirm('Are you sure you want to delete this contact?');if (test){load('edit.aspx?action=reference&remove=true&id=" & acref_id & "','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');return false;}{return false;};"" style='text-decoration:none;'><span class='tiny_tiny'>(remove)</span></a><br />"

                                    'cell_text = New Label
                                    'itz = New TableCell
                                    'itz.Width = 70
                                    'strContact = strContact & hold_rel
                                    'cell_text.Text = strContact
                                    'itz.Controls.Add(cell_text)
                                    'ro.Cells.Add(itz)

                                    'cell_text = New Label
                                    'itz = New TableCell
                                    'cell_text.Text = "&nbsp;&nbsp;&nbsp;&nbsp;"
                                    'itz.Controls.Add(cell_text)
                                    'ro.Cells.Add(itz)

                                    strContact = "<a href='mobile_details.aspx?comp_ID=" & comp_id & "&source=CLIENT&type=1' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"


                                    If comp_city <> "" Then
                                        strContact = strContact & comp_city & " "
                                    End If
                                    If comp_state <> "" Then
                                        strContact = strContact & comp_state & " "
                                    End If
                                    If comp_country <> "" Then
                                        strContact = strContact & comp_country
                                    End If
                                    strContact = strContact & "</span>"
                                    If contact_id <> 0 Then
                                        strContact = strContact & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=CLIENT&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</a>"
                                        If Trim(contact_title) <> "" Then
                                            strContact = strContact & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                        End If
                                    End If

                                    If acref_contact_type = "99" Then
                                        strContact = strContact & " (<span class='small_purple'>Exclusive Broker</span) "
                                    ElseIf acref_contact_type = "12" Then
                                        strContact = strContact & " (<span class='small_orange'>Lessee</span>) "
                                    Else
                                        strContact = strContact & " (<span class='bold_small'>" & act_name & "</span>) "
                                        If acref_contact_type = "8" Or acref_contact_type = "97" Then
                                            If acref_owner_percentage <> 0 Then
                                                strContact = strContact & " <em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                                            End If
                                        End If
                                    End If

                                    strContact = strContact & "<br /><a href='#' onClick=""javascript:var test = confirm('Are you sure you want to delete this contact?');if (test){load('edit.aspx?action=reference&remove=true&id=" & acref_id & "','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');return false;}{return false;};"" style='text-decoration:none;'><span class='tiny_tiny'>(remove)</span></a><br />"


                                    strContact = strContact & hold_rel
                                    strContact = strContact & "<br />"
                                    If cliacref_contact_priority = 1 Then
                                        '  strContact = strContact & " <strong class='smaller'>PRIMARY</strong> &nbsp;"
                                    ElseIf cliacref_contact_priority = 2 Then
                                        ' strContact = strContact & " <strong>SECONDARY</strong> &nbsp;"
                                    Else
                                        ' strContact = strContact & " <strong>OTHER</strong> &nbsp;"
                                    End If

                                    cell_text = New Label
                                    itz = New TableCell
                                    cell_text.Text = strContact
                                    itz.Controls.Add(cell_text)
                                    ro.Cells.Add(itz)
                                    ac_contact.Rows.Add(ro)
                                    hold_rel = ""
                                    'hold_con = ""
                                Else
                                    If contact_first_name <> "" Then
                                        hold_rel = hold_rel & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em>"
                                        If contact_title <> "" Then
                                            hold_rel = hold_rel & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                        End If
                                    End If

                                    If act_name = "Exclusive Broker" Then
                                        hold_rel = hold_rel & " (<b class='small_purple'>" & act_name & "</b>) <br />"
                                    ElseIf act_name = "Lessee" Then
                                        hold_rel = hold_rel & " (<span class='small_orange'>Lessee</span>) <br />"
                                    ElseIf act_name <> "" Then
                                        hold_rel = hold_rel & " (<span class='bold_small'>" & act_name & "</span>) <br />"
                                        If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                                            hold_rel = hold_rel & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em><br /> "
                                        End If
                                    End If
                                End If
                                rowcount = rowcount + 1
                            End If

                        Next

                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = aclsData_Temp.class_error
                        masterpage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If



                ' get the contact info
                If source = "JETNET" Then

                    atemptable = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(idnum, 0)
                    If Not IsNothing(atemptable) Then
                        If atemptable.Rows.Count > 0 Then

                            Dim afileterd As DataRow()
                            afileterd = atemptable.Select("", "acref_comp_id asc")
                            Dim filteredTable As DataTable = atemptable.Clone
                            ' a single data row for importing to the dalTable
                            ' extract and import
                            For Each atmpDataRow In afileterd
                                filteredTable.ImportRow(atmpDataRow)
                            Next




                            For Each r As DataRow In filteredTable.Rows
                                '
                                counter = counter + 1
                                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True And r("act_name") = "Exclusive Broker" Then

                                Else

                                    comp_id = IIf(Not IsDBNull(r("acref_comp_id")), r("acref_comp_id"), 0)
                                    act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                                    acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)

                                    comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
                                    contact_id = IIf(Not IsDBNull(r("acref_contact_id")), r("acref_contact_id"), "")
                                    contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                                    contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                                    comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                                    comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                                    comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")

                                    If rowcount + 1 < filteredTable.Rows.Count Then
                                        next_id = IIf(Not IsDBNull(filteredTable.Rows(rowcount + 1).Item("acref_comp_id")), filteredTable.Rows(rowcount + 1).Item("acref_comp_id"), 0)
                                    Else
                                        next_id = 0
                                    End If

                                    If rowcount + 1 < atemptable.Rows.Count Then
                                        If comp_id <> next_id Then
                                            repeated = True
                                        Else
                                            repeated = False
                                        End If
                                    Else
                                        repeated = True
                                    End If

                                    If repeated = True Then
                                        Dim ro As New TableRow
                                        Dim itz As New TableCell
                                        ro.VerticalAlign = VerticalAlign.Top

                                        itz.VerticalAlign = VerticalAlign.Top

                                        cell_text = New Label

                                        strContact = ""

                                        'strContact = ""
                                        'If act_name = "Exclusive Broker" Then
                                        '    strContact = strContact & "<b class='small_purple'>" & act_name & "</b><br />"
                                        'ElseIf act_name = "Lessee" Then
                                        '    strContact = "<span class='small_orange'>Lessee</span><br />"
                                        'ElseIf act_name <> "" Then
                                        '    strContact = strContact & "<span class='bold_small'>" & act_name & "</span><br />"
                                        '    If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                                        '        strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em><br />"
                                        '    End If
                                        'End If

                                        'strContact = strContact & hold_rel
                                        'cell_text = New Label
                                        'itz = New TableCell
                                        'itz.Width = 20
                                        'cell_text.Text = strContact
                                        'itz.Controls.Add(cell_text)
                                        'ro.Cells.Add(itz)

                                        'cell_text = New Label
                                        'itz = New TableCell
                                        'itz.Width = 20
                                        'cell_text.Text = "&nbsp;&nbsp;"
                                        'itz.Controls.Add(cell_text)
                                        'ro.Cells.Add(itz)
                                        'strContact = ""

                                        If comp_name <> "" Then
                                            strContact = strContact & "<a href='mobile_details.aspx?comp_ID=" & comp_id & "&type=1&source=JETNET' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"
                                        End If

                                        If comp_city <> "" Then
                                            strContact = strContact & comp_city & " "
                                        End If
                                        If comp_state <> "" Then
                                            strContact = strContact & comp_state & " "
                                        End If
                                        If comp_country <> "" Then
                                            strContact = strContact & comp_country
                                        End If

                                        strContact = strContact & "</span>"
                                        If contact_first_name <> "" Then
                                            strContact = strContact & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em>"
                                            If contact_title <> "" Then
                                                strContact = strContact & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                            End If
                                        End If

                                        If act_name = "Exclusive Broker" Then
                                            strContact = strContact & " (<b class='small_purple'>" & act_name & "</b>) <br />"
                                        ElseIf act_name = "Lessee" Then
                                            strContact = " (<span class='small_orange'>Lessee</span>) <br />"
                                        ElseIf act_name <> "" Then
                                            strContact = strContact & " (<span class='bold_small'>" & act_name & "</span>) <br />"
                                            If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                                                strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em><br />"
                                            End If
                                        End If

                                        strContact = strContact & hold_rel

                                        cell_text = New Label
                                        itz = New TableCell
                                        cell_text.Text = strContact
                                        itz.Controls.Add(cell_text)
                                        ro.Cells.Add(itz)
                                        ac_contact.Rows.Add(ro)
                                        hold_rel = ""
                                        hold_con = ""
                                    Else

                                        If contact_first_name <> "" Then
                                            hold_rel = hold_rel & "<br /><a href='mobile_details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em>"
                                            If contact_title <> "" Then
                                                hold_rel = hold_rel & "<span class='smaller'> (" & contact_title & ")</em></span>&nbsp;"
                                            End If
                                        End If
                                        If act_name = "Exclusive Broker" Then
                                            hold_rel = hold_rel & " (<b class='small_purple'>" & act_name & "</b>) <br />"
                                        ElseIf act_name = "Lessee" Then
                                            hold_rel = hold_rel & " (<span class='small_orange'>Lessee</span>) <br />"
                                        ElseIf act_name <> "" Then
                                            hold_rel = hold_rel & " (<span class='bold_small'>" & act_name & "</span>) <br />"
                                            If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                                                hold_rel = hold_rel & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em><br /> "
                                            End If
                                        End If

                                    End If

                                End If
                                next_comp = r("acref_comp_id")
                            Next
                        End If
                        ' dump the datatable
                        atemptable.Dispose()
                        atemptable = Nothing
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Fill_Contact_Info_AC() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - New_Fill_Contact_Info_AC() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return ac_contact
        End Function

        Public Shared Function saveFolder(ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal folders_display As Label) As Integer
            saveFolder = 0
            Dim masterpage As New Object

            If Not IsNothing(mob) Then
                masterpage = mob
            Else
                masterpage = main
            End If

            Try
                Dim flist As CheckBoxList = folders_display.FindControl("folder_ids")
                Dim personal_flist As CheckBoxList = folders_display.FindControl("personal_folder_ids")
                Dim jetnet_ac_id As Integer = 0
                Dim jetnet_comp_id As Integer = 0
                Dim jetnet_contact_id As Integer = 0
                Dim client_ac_id As Integer = 0
                Dim client_comp_id As Integer = 0
                Dim client_contact_id As Integer = 0
                Dim cfolder_id As Integer = 0
                Dim fval As String = ""
                Dim errored As String = ""

                Select Case masterpage.TypeOfListing
                    Case 1
                        Select Case masterpage.ListingSource
                            Case "JETNET"
                                jetnet_comp_id = masterpage.ListingID
                            Case "CLIENT"
                                client_comp_id = masterpage.ListingID
                        End Select

                        If masterpage.Listing_ContactID <> 0 Then
                            Select Case masterpage.ListingSource
                                Case "JETNET"
                                    jetnet_comp_id = 0
                                    jetnet_contact_id = masterpage.Listing_ContactID
                                Case "CLIENT"
                                    client_comp_id = 0
                                    client_contact_id = masterpage.Listing_ContactID
                            End Select
                        End If
                    Case 3
                        Select Case masterpage.ListingSource
                            Case "JETNET"
                                jetnet_ac_id = masterpage.ListingID
                            Case "CLIENT"
                                client_ac_id = masterpage.ListingID
                        End Select
                End Select

                Save_Folder_Action(flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterpage.aclsData_Temp)
                Save_Folder_Action(personal_flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterpage.aclsData_Temp)

                Dim newly_updated As TableCell = folders_display.FindControl("newly_updated")
                newly_updated.Visible = True
                saveFolder = 1
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - saveFolder() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

        End Function

        Public Shared Function Build_JETNET_Features_Tab(ByVal jetnet_id As Integer, ByVal source As String, ByVal listingID As Integer, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim Atemptable2 As New DataTable
            Dim color As String = ""
            Dim features_text_client As String = ""
            Dim features_text As String = ""
            '--------------------------------Features Information----------------------------------------------------------
            If jetnet_id <> 0 Then
                Try

                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                        features_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br clear='all'/>"
                    End If
                    features_text = features_text & "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"
                    Atemptable2 = masterpage.aclsData_Temp.GetJETNET_Aircraft_Details_Key_Features_AC_ID(jetnet_id, 0)
                    features_text = features_text & "<tr class='dark_blue'><td align='left' valign='top' width='20'><b></b></td>"
                    features_text = features_text & "<td align='left' valign='top' width='50'><b>CODE:</b></td>"
                    features_text = features_text & "<td align='left' valign='top'><b>DESCRIPTION:</b></td>"

                    If Not IsNothing(Atemptable2) Then
                        For Each r As DataRow In Atemptable2.Rows
                            If color = "alt_row" Then
                                color = ""
                            Else
                                color = "alt_row"
                            End If

                            If r("kff_name") = "No" Or r("kff_name") = "N" Then
                                features_text = features_text & "<tr class='" & color & "'><td align='center' valign='middle' class='red'><span class='em' title='No'><img src='images/red_x.gif' alt='No' /></span></td>"
                            ElseIf r("kff_name") = "Yes" Or r("kff_name") = "Y" Then
                                features_text = features_text & "<tr class='" & color & "'><td align='center' valign='middle' class='green'><span class='em' title='Yes'><img src='images/green_check.gif' alt='Yes' /></span></td>"
                            ElseIf r("kff_name") = "Unknown" Or r("kff_name") = "U" Then
                                features_text = features_text & "<tr class='" & color & "'><td align='center' valign='middle' class='blue_color'><span class='em' title='Unknown'><img src='images/blue_dash.gif' alt='Unknown' /></span></td>"
                            Else
                                features_text = features_text & "<tr class='" & color & "'><td align='center' valign='middle'><span title='" & r("kff_name") & "'>" & UCase(Left(r("kff_name"), 1)) & "</span></td>"
                            End If

                            features_text = features_text & "<td align='left' valign='top'>" & r("kfeat_type") & "</td>"
                            features_text = features_text & "<td align='left' valign='top'>" & r("kfeat_name") & "</td>"
                        Next

                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Build_JETNET_Features_Tab - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                    features_text = features_text & "</table>"
                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - Build_JETNET_Features_Tab() Features Tab - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
            End If
            Atemptable2 = Nothing
            Return features_text
        End Function
        Public Shared Function Build_CLIENT_Features_Tab(ByVal client_id As Integer, ByVal source As String, ByVal listingID As Integer, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim Atemptable2 As New DataTable
            Dim color As String = ""
            Dim features_text_client As String = ""
            Dim features_text As String = ""
            '--------------------------------Features Information----------------------------------------------------------

            If client_id <> 0 Then
                Try
                    Atemptable2 = masterpage.aclsData_Temp.Get_Client_Aircraft_Key_Features(client_id)

                    If Not IsNothing(Atemptable2) Then

                        If HttpContext.Current.Session.Item("isMobile") <> True Then
                            If source = "CLIENT" Then
                                features_text_client = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' alt='EDIT CLIENT INFORMATION' class='float_right' style='padding-bottom:3px;' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=features','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all'/>"
                            Else
                                features_text_client = "<img src='images/non_client_info.jpg' class='float_right' style='padding-bottom:3px;' alt='CLIENT INFORMATION' border='0' /><br clear='all'/>"
                            End If
                        End If

                        features_text_client = features_text_client & "<table width='100%' cellpadding='3' cellspacing='0' class='engine_client'>"
                        features_text_client = features_text_client & "<tr class='dark_red'><td align='left' valign='top' width='20'><b></b></td>"
                        features_text_client = features_text_client & "<td align='left' valign='top' width='50'><b>CODE:</b></td>"
                        features_text_client = features_text_client & "<td align='left' valign='top'><b>DESCRIPTION:</b></td>"

                        For Each r As DataRow In Atemptable2.Rows
                            If color = "alt_row_client" Then
                                color = ""
                            Else
                                color = "alt_row_client"
                            End If

                            If r("clikff_name") = "No" Then
                                features_text_client = features_text_client & "<tr class='" & color & "'><td align='center' valign='middle' class='red'><span class='em' title='No'><img src='images/red_x.gif' alt='No' /></span></td>"
                            ElseIf r("clikff_name") = "Yes" Then
                                features_text_client = features_text_client & "<tr class='" & color & "'><td align='center' valign='middle' class='green'><span class='em' title='Yes'><img src='images/green_check.gif' alt='Yes' /></span></td>"
                            ElseIf r("clikff_name") = "Unknown" Then
                                features_text_client = features_text_client & "<tr class='" & color & "'><td align='center' valign='middle' class='blue_color'><span class='em' title='Unknown'><img src='images/blue_dash.gif' alt='Unknown' /></span></td>"
                            Else
                                features_text_client = features_text_client & "<tr class='" & color & "'><td align='center' valign='middle'><span title='" & r("clikfeat_name") & "'>" & UCase(Left(r("clikfeat_name"), 1)) & "</span></td>"
                            End If

                            features_text_client = features_text_client & "<td align='left' valign='top'>" & r("cliafeat_type") & "</td>"
                            features_text_client = features_text_client & "<td align='left' valign='top'>" & r("clikfeat_name") & "</td>"
                        Next

                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Build_CLIENT_Features_Tab() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                    features_text_client = features_text_client & "</table>"

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - Build_CLIENT_Features_Tab() Features Tab - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
            End If
            Atemptable2 = Nothing
            'features_label_client.Text = features_text_client
            Return features_text_client
        End Function
        Public Shared Function Build_JETNET_Engine_Tab(ByVal jetnet_datatable As DataTable, ByVal jetnet_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim engine_text As String = ""
            Dim engine_text_client As String = ""
            If jetnet_id <> 0 Then
                ' ----------------------------------Start Engine Tab-----------------------------------------------------------
                Try
                    If Not IsNothing(jetnet_datatable) Then
                        masterpage.atemptable2 = jetnet_datatable
                    Else
                        masterpage.aTempTable2 = masterpage.aclsData_Temp.GetJETNET_Aircraft_Engine(jetnet_id)
                    End If

                    If Not IsNothing(masterpage.aTempTable2) Then
                        If masterpage.aTempTable2.Rows.Count > 0 Then
                            If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If an EVO user Then
                                engine_text = engine_text & "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br clear='all'/>"
                            End If
                            engine_text = engine_text & "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"


                            For Each R As DataRow In masterpage.aTempTable2.Rows

                                engine_text = engine_text & "<tr class='gray'><td align='left' valign='top' width='100'>Engine Model</td>"
                                engine_text = engine_text & "<td align='left' valign='top' colspan='2'>" & R("ac_engine_name") & "</td>"
                                engine_text = engine_text & "<td align='center' valign='top' colspan='2'>Engine Maintenance Program:<br />"


                                engine_text = engine_text & R("emp_provider_name")

                                If Not IsDBNull(R("emp_program_name")) Then
                                    If UCase(R("emp_program_name")) <> "UNKNOWN" Then
                                        engine_text = engine_text & " - " & R("emp_program_name")
                                    End If
                                End If
                                engine_text = engine_text & "</td>"
                                engine_text = engine_text & "<td align='center' valign='top' colspan='2'>Engine Management Program:<br />"

                                engine_text = engine_text & R("emgp_provider_name")

                                If Not IsDBNull(R("emgp_program_name")) Then
                                    If UCase(R("emgp_program_name")) <> "UNKNOWN" Then
                                        engine_text = engine_text & " - " & R("emgp_program_name")
                                    End If
                                End If

                                engine_text = engine_text & "</td>"
                                engine_text = engine_text & "<td align='center' valign='top' colspan='2'>On Condition TBO:<br />"

                                If Not IsDBNull(R("ac_engine_tbo_oc_flag")) Then
                                    engine_text = engine_text & yes_no(R("ac_engine_tbo_oc_flag"), "else") & "</td></tr>"
                                End If

                                engine_text = engine_text & "<tr><td align='left' valign='top'>&nbsp;</td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>Serial #</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>TTSNEW Hrs</b> <span class='tiny'>(Total Time Since New)</span></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>SOH/SCOR Hrs</b> <span class='tiny'>(Since Overhaul)</span></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>SHI/SMPI Hrs</b> <span class='tiny'>(Since Hot Inspection)</span></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>TBO/TBCI Hrs</b> <span class='tiny'>(Time Between Overhaul)</span></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>Total Cycles Since New</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>Total Cycles Since Overhaul</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top' class='dark_blue'><b>Total Cycles Since Hot</b></td></tr>"
                                engine_text = engine_text & "<tr class='alt_row'><td align='left' valign='top'><b>Eng1:</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_ser_no") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_tot_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_soh_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_shi_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_tbo_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_snew_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_soh_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_1_shs_cycles") & "</td></tr>"
                                engine_text = engine_text & "<tr><td align='left' valign='top'><b>Eng2:</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_ser_no") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_tot_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_soh_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_shi_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_tbo_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_snew_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_soh_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_2_shs_cycles") & "</td></tr>"
                                engine_text = engine_text & "<tr class='alt_row'><td align='left' valign='top'><b>Eng3:</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_ser_no") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_tot_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_soh_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_shi_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_tbo_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_snew_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_soh_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_3_shs_cycles") & "</td></tr>"
                                engine_text = engine_text & "<tr><td align='left' valign='top'><b>Eng4:</b></td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_ser_no") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_tot_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_soh_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_shi_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_tbo_hrs") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_snew_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_soh_cycles") & "</td>"
                                engine_text = engine_text & "<td align='left' valign='top'>" & R("ac_engine_4_shs_cycles") & "</td></tr>"
                            Next
                            engine_text = engine_text & "</table>"
                        Else ' 0 rows
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral - fill_AC_text() Engine Tab - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
            End If
            masterpage.aTempTable2 = Nothing
            Return engine_text
        End Function

        Public Shared Function Build_Both_Engine_Tab_Mobile_Only(ByVal jetnet_id As Integer, ByVal client_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim engine_text As String = ""
            Dim engine_text_client As String = ""
            Dim ext As String = ""
            Dim nbr As String = "no"
            Dim hrs As String = "hrs"
            If jetnet_id <> 0 Then
                ext = "ac"
                nbr = "no"
                hrs = "hrs"
            ElseIf client_id <> 0 Then
                ext = "cliacep"
                nbr = "nbr"
                hrs = "hours"
            End If
            ' ----------------------------------Start Engine Tab-----------------------------------------------------------
            Try
                If jetnet_id <> 0 Then
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.GetJETNET_Aircraft_Engine(jetnet_id)
                ElseIf client_id <> 0 Then
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.Get_Client_Aircraft_Engine(client_id)
                End If
                If Not IsNothing(masterpage.aTempTable2) Then
                    If masterpage.aTempTable2.Rows.Count > 0 Then
                        engine_text = engine_text & "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"


                        For Each R As DataRow In masterpage.aTempTable2.Rows

                            engine_text = engine_text & "<tr class='dark_blue'><td align='left' valign='top' width='100' colspan='2'><b>Engine Model</b></td>"
                            engine_text = engine_text & "<td align='left' valign='top' colspan='2'>" & R(ext & "_engine_name") & "</td>"

                            engine_text = engine_text & "</tr><tr class='dark_blue'>"
                            engine_text = engine_text & "<td align='left' valign='top' colspan='2'><b>Engine Maintenance Program:</b><br />"


                            Dim program As Integer = 0
                            Dim management As Integer = 0
                            Dim program_name As String = ""
                            Dim maint_name As String = ""

                            If client_id <> 0 Then
                                If Not IsDBNull(R("cliacep_engine_maintenance_program")) Then
                                    program = R("cliacep_engine_maintenance_program")
                                End If
                                If Not IsDBNull(R("cliacep_engine_management_program")) Then
                                    management = R("cliacep_engine_management_program")
                                End If
                                Dim atemptable3 As New DataTable
                                atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(program, 0, 0, "Engine", False)
                                If Not IsNothing(atemptable3) Then
                                    If atemptable3.Rows.Count > 0 Then
                                        program_name = atemptable3.Rows(0).Item("emp_provider_name") & " " & atemptable3.Rows(0).Item("emp_program_name")
                                    End If
                                End If
                                atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(0, 0, management, "Engine", False)
                                If Not IsNothing(atemptable3) Then
                                    If atemptable3.Rows.Count > 0 Then
                                        maint_name = atemptable3.Rows(0).Item("emgp_provider_name") & " " & atemptable3.Rows(0).Item("emgp_program_name")
                                    End If
                                End If

                                engine_text = engine_text & program_name & "</td><td align='left' valign='top' colspan='2'><b>Engine Management Program:</b><br />"
                                engine_text = engine_text & maint_name
                            Else
                                engine_text = engine_text & R("emp_provider_name")

                                If Not IsDBNull(R("emp_program_name")) Then
                                    If UCase(R("emp_program_name")) <> "UNKNOWN" Then
                                        engine_text = engine_text & " - " & R("emp_program_name")
                                    End If
                                End If
                                engine_text = engine_text & "</td>"

                                engine_text = engine_text & "<td align='left' valign='top' colspan='2'><b>Engine Management Program:</b><br />"

                                engine_text = engine_text & R("emgp_provider_name")

                                If Not IsDBNull(R("emgp_program_name")) Then
                                    If UCase(R("emgp_program_name")) <> "UNKNOWN" Then
                                        engine_text = engine_text & " - " & R("emgp_program_name")
                                    End If
                                End If

                            End If

                            engine_text = engine_text & "</td>"

                            engine_text = engine_text & "</tr><tr class='dark_blue'>"
                            engine_text = engine_text & "<td align='left' valign='top' colspan='2'><b>On Condition TBO:</b> "


                            If Not IsDBNull(R(ext & "_engine_tbo_oc_flag")) Then
                                engine_text = engine_text & yes_no(R(ext & "_engine_tbo_oc_flag"), "else") & "</td>"
                            End If


                            engine_text = engine_text & "<td align='center' valign='top' colspan='2'></td>"

                            engine_text = engine_text & "</tr>"

                            ' Dim x As Integer = 1
                            Dim answer As String = ""
                            For x As Integer = 1 To 4
                                If Not IsDBNull(R(ext & "_engine_" & x & "_ser_" & nbr)) Then
                                    If R(ext & "_engine_" & x & "_ser_" & nbr) <> "" Then
                                        engine_text = engine_text & "<tr style='background-color:#d4d4d4;'><td align='left' valign='top' colspan='3'><b>Engine #" & x & " "
                                        engine_text = engine_text & "Ser #:</b></td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_ser_" & nbr) & "<br />"
                                        engine_text = engine_text & "</td></tr>"

                                        If jetnet_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_tot_" & hrs)) Then
                                                engine_text = engine_text & "<tr class='gray'><td align='left' valign='top' colspan='3'><span class='tiny'><b>Total Time Since New</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tot_" & hrs) & "</td></tr>"
                                            End If
                                        ElseIf client_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_ttsn_" & hrs)) Then
                                                engine_text = engine_text & "<tr class='gray'><td align='left' valign='top' colspan='3'><span class='tiny'><b>Total Time Since New</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_ttsn_" & hrs) & "</td></tr>"
                                            End If
                                        End If

                                        If jetnet_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_soh_" & hrs)) Then
                                                engine_text = engine_text & "<tr><td align='left' valign='top'  colspan='3'>"
                                                engine_text = engine_text & "<span class='tiny'><b>Since Overhaul</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_soh_" & hrs) & "</td></tr>"
                                            End If
                                        ElseIf client_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_tsoh_" & hrs)) Then
                                                engine_text = engine_text & "<tr><td align='left' valign='top'  colspan='3'>"
                                                engine_text = engine_text & "<span class='tiny'><b>Since Overhaul</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tsoh_" & hrs) & "</td></tr>"
                                            End If
                                        End If
                                        If jetnet_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_shi_" & hrs)) Then
                                                engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                engine_text = engine_text & "<span class='tiny'><b>Since Hot Inspection:</b></span></td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_shi_" & hrs) & "</td></tr>"
                                            End If
                                        ElseIf client_id <> 0 Then
                                            If Not IsDBNull(R(ext & "_engine_" & x & "_tshi_" & hrs)) Then
                                                engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                engine_text = engine_text & "<span class='tiny'><b>Since Hot Inspection</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tshi_" & hrs) & "</td></tr>"
                                            End If
                                        End If
                                        'If jetnet_id <> 0 Then
                                        If Not IsDBNull(R(ext & "_engine_" & x & "_tbo_" & hrs)) Then
                                            engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                            engine_text = engine_text & "<span class='tiny'><b>Time Between Overhaul</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tbo_" & hrs) & "</td></tr>"
                                            '    End If
                                            'ElseIf client_id <> 0 Then
                                            '    If Not IsDBNull(R(ext & "_engine_" & x & "_tbo_" & hrs)) Then
                                            '        engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                            '        engine_text = engine_text & "<b>TBO/TBCI Hrs</b><br /><span class='tiny'>(Time Between Overhaul)</span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tbo_" & hrs) & "</td></tr>"
                                            '    End If
                                            'End If
                                            If jetnet_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_snew_cycles")) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since New</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_1_snew_cycles") & "</td></tr>"
                                                End If
                                            ElseIf client_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_tsn_cycle")) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since New</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_1_tsn_cycle") & "</td></tr>"
                                                End If
                                            End If
                                            If jetnet_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_tbo_" & hrs)) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since Overhaul</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tbo_" & hrs) & "</td></tr>"
                                                End If
                                            ElseIf client_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_tsoh_" & hrs)) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since Overhaul</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tsoh_" & hrs) & "</td></tr>"
                                                End If

                                            End If
                                            If jetnet_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_shs_cycles")) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since Hot</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_shs_cycles")
                                                    engine_text = engine_text & "</td></tr>"
                                                End If
                                            ElseIf client_id <> 0 Then
                                                If Not IsDBNull(R(ext & "_engine_" & x & "_tshi_cycle")) Then
                                                    engine_text = engine_text & "<tr><td align='left' valign='top' colspan='3'>"
                                                    engine_text = engine_text & "<span class='tiny'><b>Total Cycles Since Hot</b></span>:</td><td align='left' valign='top'>" & R(ext & "_engine_" & x & "_tshi_cycle")
                                                    engine_text = engine_text & "</td></tr>"
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next


                        Next
                        engine_text = engine_text & "</table>"
                    Else ' 0 rows
                    End If
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("clsgeneral - fill_AC_text() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If

            Catch ex As Exception
                masterpage.error_string = "clsgeneral - fill_AC_text() Engine Tab - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

            masterpage.aTempTable2 = Nothing
            Return engine_text
        End Function
        Public Shared Function Build_CLIENT_Engine_Tab(ByVal client_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim engine_text As String = ""
            Dim engine_text_client As String = ""
            If client_id <> 0 Then

                '----------------------------------Start Engine Tab-----------------------------------------------------------
                Try
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.Get_Client_Aircraft_Engine(client_id)
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            engine_text_client = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right'  alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=engine','','scrollbars=yes,menubar=no,height=500,width=1200,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all'/>"
                        Else
                            engine_text_client = engine_text_client & "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If

                    If Not IsNothing(masterpage.aTempTable2) Then
                        If masterpage.aTempTable2.Rows.Count > 0 Then
                            engine_text_client = engine_text_client & "<table width='100%' cellpadding='3' cellspacing='0' class='engine_client'>"
                            For Each R As DataRow In masterpage.aTempTable2.Rows
                                engine_text_client = engine_text_client & "<tr class='alt_row_client'><td align='left' valign='top' width='100'>Engine Model</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' colspan='2'>" & R("cliacep_engine_name") & "</td>"
                                engine_text_client = engine_text_client & "<td align='center' valign='top' colspan='2'>Engine Maintenance Program:<br />"
                                Dim atemptable3 As New DataTable
                                Dim program As Integer = 0
                                Dim management As Integer = 0
                                Dim program_name As String = ""
                                Dim maint_name As String = ""
                                If Not IsDBNull(R("cliacep_engine_maintenance_program")) Then
                                    program = R("cliacep_engine_maintenance_program")
                                End If
                                If Not IsDBNull(R("cliacep_engine_management_program")) Then
                                    management = R("cliacep_engine_management_program")
                                End If
                                atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(program, 0, 0, "Engine", False)
                                If Not IsNothing(atemptable3) Then
                                    If atemptable3.Rows.Count > 0 Then
                                        program_name = atemptable3.Rows(0).Item("emp_provider_name")

                                        If Not IsDBNull(atemptable3.Rows(0).Item("emp_program_name")) Then
                                            If UCase(atemptable3.Rows(0).Item("emp_program_name").ToString) <> "UNKNOWN" Then
                                                program_name = program_name & " " & atemptable3.Rows(0).Item("emp_program_name")
                                            End If
                                        End If

                                    End If


                                End If




                                atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(0, 0, management, "Engine", False)
                                If Not IsNothing(atemptable3) Then
                                    If atemptable3.Rows.Count > 0 Then
                                        maint_name = atemptable3.Rows(0).Item("emgp_provider_name")

                                        If Not IsDBNull(atemptable3.Rows(0).Item("emgp_program_name")) Then
                                            If UCase(atemptable3.Rows(0).Item("emgp_program_name").ToString) <> "UNKNOWN" Then
                                                maint_name = maint_name & " " & atemptable3.Rows(0).Item("emgp_program_name")
                                            End If
                                        End If
                                    End If
                                End If
                                If Not IsDBNull(program_name) Then
                                    engine_text_client = engine_text_client & " - " & program_name
                                End If
                                engine_text_client = engine_text_client & "</td>"
                                engine_text_client = engine_text_client & "<td align='center' valign='top' colspan='2'>Engine Management Program:<br />"


                                If Not IsDBNull(maint_name) Then
                                    engine_text_client = engine_text_client & " - " & maint_name
                                End If

                                engine_text_client = engine_text_client & "</td>"
                                engine_text_client = engine_text_client & "<td align='center' valign='top' colspan='2'>On Condition TBO:<br />"

                                If Not IsDBNull(R("cliacep_engine_tbo_oc_flag")) Then
                                    engine_text_client = engine_text_client & yes_no(R("cliacep_engine_tbo_oc_flag"), "else") & "</td></tr>"
                                End If

                                engine_text_client = engine_text_client & "<tr><td align='left' valign='top'>&nbsp;</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>Serial #</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>TTSNEW Hrs</b> <span class='tiny'>(Total Time Since New)</span></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>SOH/SCOR Hrs</b> <span class='tiny'>(Since Overhaul)</span></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>SHI/SMPI Hrs</b> <span class='tiny'>(Since Hot Inspection)</span></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>TBO/TBCI Hrs</b> <span class='tiny'>(Time Between Overhaul)</span></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>Total Cycles Since New</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>Total Cycles Since Overhaul</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top' class='dark_red'><b>Total Cycles Since Hot</b></td></tr>"
                                engine_text_client = engine_text_client & "<tr class='alt_row_client'><td align='left' valign='top'><b>Eng1:</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_ser_nbr") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_ttsn_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tsoh_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tshi_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tbo_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tsn_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tsoh_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_1_tshi_cycle") & "</td></tr>"
                                engine_text_client = engine_text_client & "<tr><td align='left' valign='top'><b>Eng2:</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_ser_nbr") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_ttsn_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tsoh_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tshi_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tbo_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tsn_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tsoh_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_2_tshi_cycle") & "</td></tr>"
                                engine_text_client = engine_text_client & "<tr class='alt_row_client'><td align='left' valign='top'><b>Eng3:</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_ser_nbr") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_ttsn_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tsoh_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tshi_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tbo_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tsn_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tsoh_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_3_tshi_cycle") & "</td></tr>"
                                engine_text_client = engine_text_client & "<tr><td align='left' valign='top'><b>Eng4:</b></td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_ser_nbr") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_ttsn_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tsoh_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tshi_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tbo_hours") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tsn_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tsoh_cycle") & "</td>"
                                engine_text_client = engine_text_client & "<td align='left' valign='top'>" & R("cliacep_engine_4_tshi_cycle") & "</td></tr>"
                            Next
                            engine_text_client = engine_text_client & "</table>"
                        Else ' 0 rows
                        End If

                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Build_CLIENT_Engine_Tab() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - Build_CLIENT_Engine_Tab() Engine Tab - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try

            End If
            masterpage.aTempTable2 = Nothing
            Return engine_text_client
        End Function
        Public Shared Function Build_JETNET_Avionics_Tab(ByVal jetnet_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim none_to_display As Boolean = False
            Dim avionics_text_client As String = ""
            Dim avionics_text As String = ""
            Dim color As String = ""
            '--------------------------------Avionics Information----------------------------------------------------------
            Try
                ' get the avionics
                'Alright let's figure out the id's to pass them. First off.

                If jetnet_id <> 0 Then
                    avionics_text = "<ul class='display_tab'>"
                    'JETNET AVIONICS
                    masterpage.aTempTable2 = masterpage.aclsData_temp.GetJETNET_Aircraft_Avionics_AC_ID(jetnet_id, 0)
                    If Not IsNothing(masterpage.aTempTable2) Then
                        If masterpage.aTempTable2.Rows.Count = 0 Then
                            none_to_display = True
                        End If

                        If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                            If masterpage.aTempTable2.Rows.Count > 0 Then
                                avionics_text = avionics_text & "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br />"
                            End If
                        End If

                        For Each r As DataRow In masterpage.aTempTable2.Rows
                            If color = "alt_row" Then
                                color = ""
                            Else
                                color = "alt_row"
                            End If
                            avionics_text = avionics_text & "<li><b>" & r("av_name") & "</b> - "
                            avionics_text = avionics_text & "" & r("av_description") & "</li>"
                        Next
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                    avionics_text = avionics_text & "</ul>"
                End If

                'If none_to_display = True Then


                '    'props_label_notes = CType(FindControlRecursive(Aircraft_Tabs1, "props_label_notes"), Label)
                '    warning_label.Text = "<p align='center' class='red'><b>There are currently no Avionics Details for this plane.</b></p>"
                'End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Build_JETNET_Avionics_Tab() Engine Tab - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return avionics_text
        End Function


        Public Shared Function Build_CLIENT_Avionics_Tab(ByVal client_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim avionics_text_client As String = ""
            Dim avionics_text As String = ""
            Dim color As String = ""
            Try
                If client_id <> 0 Then
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            avionics_text_client = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' alt='EDIT CLIENT INFORMATION' class='float_right'  border='0' onClick=""javascript:load('edit.aspx?action=edit&type=avionics','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            avionics_text_client = "<img src='images/non_client_info.jpg' alt='CLIENT INFORMATION' class='float_right'  border='0' /><br clear='all' />"
                        End If
                    End If

                    masterpage.aTempTable2 = masterpage.aclsData_Temp.Get_Client_Aircraft_Avionics(client_id)


                    avionics_text_client = avionics_text_client & "<ul class='display_tab_client'>"
                    If Not IsNothing(masterpage.aTempTable2) Then
                        For Each r As DataRow In masterpage.aTempTable2.Rows
                            'CLIENT AVIONICS
                            avionics_text_client = avionics_text_client & "<li><b>" & r("cliav_name") & "</b> - "
                            avionics_text_client = avionics_text_client & "" & r("cliav_description") & "</li>"
                        Next
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                    avionics_text_client = avionics_text_client & "</ul>"

                End If


                masterpage.aTempTable2 = Nothing
            Catch ex As Exception
                masterpage.error_string = "clsgeneral - fill_AC_text() Avionics - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return avionics_text_client
        End Function
        Public Shared Sub Build_Jetnet_Flights_Tab(ByVal flight_summary_label As Label, ByVal flights_warning_text As Label, ByVal flight_dg As DataGrid, ByVal Aircraft_Data As clsClient_Aircraft, ByVal masterpage As main_site, ByVal Flight_Table As DataTable)
            If Aircraft_Data.cliaircraft_reg_nbr <> "" Then
                Flight_Table = masterpage.aclsData_Temp.Aircraft_Flight_Results(Aircraft_Data.cliaircraft_reg_nbr, DateAdd(DateInterval.Day, -90, Now()))
                If Not IsNothing(Flight_Table) Then
                    If Flight_Table.Rows.Count > 0 Then
                        flight_dg.DataSource = Flight_Table
                        flight_dg.DataBind()
                        Dim flight_summary As String = "<table width='100%' cellspacing='0' cellpadding='5'>"
                        flight_summary = flight_summary & "<tr><td align='left' valign='top' width='50%'>"

                        flight_summary = flight_summary & ("<b>Total Flights:</b> " & Flight_Table.Rows.Count & "</td><td align='left' valign='top'>")
                        Dim total_miles As Integer = 0
                        Dim total_flight_time As Integer = 0
                        For Each r As DataRow In Flight_Table.Rows
                            If Not IsDBNull(r("aractivity_distance")) Then
                                If IsNumeric(r("aractivity_distance")) Then
                                    total_miles = total_miles + r("aractivity_distance")
                                End If
                            End If

                            If Not IsDBNull(r("aractivity_flight_time")) Then
                                If IsNumeric(r("aractivity_flight_time")) Then
                                    total_flight_time = total_flight_time + r("aractivity_flight_time")
                                End If
                            End If

                        Next
                        flight_summary = flight_summary & ("<b>Total Miles:</b> " & FormatNumber(total_miles, 0) & "(nm)<br />")
                        flight_summary = flight_summary & ("<b>Average Miles:</b> " & FormatNumber(total_miles / 31, 0) & "(nm)</td>")

                        flight_summary = flight_summary & ("<td align='left' valign='top'><b>Total Flight Time:</b> " & FormatNumber(total_flight_time, 0) & "(min)<br />")


                        flight_summary = flight_summary & ("<b>Average Flight Time:</b> " & FormatNumber(total_flight_time / 31, 0) & "(min)</td></tr></table>")
                        flight_summary = flight_summary & "<br /><br /><p align='center'><b><a href='https://www.traqpak.com/' target='new'>Powered by - ARG/US TRAQPak</a>&nbsp;&nbsp;<a href='https://www.testjetnetevolution.com/argUS.aspx?regnumber=" & Aircraft_Data.cliaircraft_reg_nbr & "' target='new'>View Activity Map</a>&nbsp;&nbsp;<a href='https://www.testjetnetevolution.com/help/TRAQPak_faq.pdf' target='new'>TRAQPak FAQs</a></b></p><br /><br />"
                        flight_summary_label.Text = flight_summary
                    Else
                        flights_warning_text.Text = "<p align='center'>There is no flight activity for this plane.</p>"
                    End If
                Else
                    flights_warning_text.Text = "<p align='center'>There is no flight activity for this plane.</p>"
                End If
            Else
                flights_warning_text.Text = "<p align='center'>There is no flight activity for this plane.</p>"
            End If
        End Sub
        Public Shared Sub Fill_Aircraft_Pictures(ByVal id As Integer, ByVal picture_label As Label, ByVal masterpage As crmWebClient.main_site)
            Dim atemptable As New DataTable
            Dim str As String = ""
            Dim count As Integer = 0
            atemptable = masterpage.aclsData_Temp.AC_Pictures(id)
            str = "<table width='100%' cellpadding='3' cellspacing='0'>"
            str = str & "<tr>"
            If atemptable.Rows.Count > 0 Then
                For Each r As DataRow In atemptable.Rows

                    If count = 2 Then
                        str = str & "</tr><tr>"
                        count = 0
                    End If
                    count = count + 1
                    str = str & "<td align='left' valign='top'><a href='#' onclick=""javascript:window.open('picture.aspx?url=" & r("acpic_ac_id") & "-" & r("acpic_journ_id") & "-" & r("acpic_id") & "." & r("acpic_image_type") & "','unloaded_me','scrollbars=yes,menubar=no,height=800,width=600,resizable=yes,toolbar=no,location=no,status=no');""><img src='" & HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/" & r("acpic_ac_id") & "-" & r("acpic_journ_id") & "-" & r("acpic_id") & "." & r("acpic_image_type") & "' width='185' border='0' /></a></td>"
                Next
            End If
            str = str & "</tr></table>"

            If atemptable.Rows.Count = 0 Then
                str = "<p align='center' class='red'><b>There are no pictures available.</b></p>"
            End If
            picture_label.Text = str
        End Sub
        Public Shared Function Build_Event_Tab(ByVal jetnet_id As Integer, ByVal OtherID As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Dim none_to_display As Boolean = False
            Dim color As String = ""
            Dim event_text As String = ""
            event_text = "<ul class='display_tab'>"
            '--------------------------------Event Tab Information----------------------------------------------------------
            Try
                ' get the events
                If jetnet_id <> 0 Then
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.AC_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, jetnet_id, "", "", "")
                ElseIf OtherID <> 0 Then
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.AC_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, OtherID, "", "", "")
                Else
                    masterpage.aTempTable2 = New DataTable
                End If
                If Not IsNothing(masterpage.aTempTable2) Then
                    If masterpage.aTempTable2.Rows.Count = 0 Then
                        none_to_display = True
                    End If

                    For Each r As DataRow In masterpage.aTempTable2.Rows
                        If color = "alt_row" Then
                            color = ""
                        Else
                            color = "alt_row"
                        End If
                        event_text = event_text & "<li><b>" & r("apev_action_date") & "</b> - "
                        If Not IsDBNull(r("apev_subject")) And Not IsDBNull(r("apev_description")) Then
                            If r("apev_subject") = r("apev_description") Then
                                event_text = event_text & "" & r("apev_subject") & "</li>"
                            Else
                                event_text = event_text & "" & r("apev_subject") & "<br /> (<em>" & r("apev_description") & "</em>)</li>"
                            End If
                        Else
                            If Not IsDBNull(r("apev_subject")) Then
                                event_text = event_text & "" & r("apev_subject") & "<br /> (<em>" & r("apev_description") & "</em>)</li>"
                            Else
                                event_text = event_text & "(<em>" & r("apev_description") & "</em>)</li>"
                            End If
                        End If
                    Next
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If
                '  End If
            Catch ex As Exception
                masterpage.error_string = "details.aspx.vb - fill_AC_text() Events Tab - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            event_text = event_text & "</li></ul>"

            If none_to_display = True Then
                event_text = ""
            End If
            ' End If
            masterpage.aTempTable2 = Nothing
            Return event_text
        End Function

        Public Shared Sub Build_Transaction_Tab(ByVal jetnet_id As Integer, ByVal client_id As Integer, ByVal otherID As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal type As String, ByVal jetnet_label As Label, ByVal client_label As Label)
            '----------------------------------Transactions Information-------------------------------------------------------
            Dim masterpage As Object

            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Dim id_array As String = ""
            Dim client_transactions_hold As New DataTable
            Dim client_transactions_search As New DataTable
            Dim atemptable4 As New DataTable
            Dim trans_text_client As String = ""
            Dim trans_date As String = ""
            Dim trans_subject As String = ""
            ' Dim action_date As String = ""
            Dim asking_price As String = ""
            Dim est_price As String = ""
            Dim sold_price As String = ""
            Dim listing_date As String = ""
            Dim edit_link As String = ""
            Dim trans_text As String = ""
            Dim color As String = ""
            Dim old As Integer = 0
            Dim link As String = ""
            Dim subject_link As String = ""
            Dim doc_list As String = ""
            Dim count As Integer = 0
            Dim compare As Integer = 0
            Dim show_asking As Boolean = False

            If HttpContext.Current.Session("isMobile") = False And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
                If otherID = 0 And source = "JETNET" Then
                    trans_text_client = "&nbsp;&nbsp;<b><a href='#' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&new=true&acID=" & jetnet_id & "&source=JETNET','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');"">Add New Transaction</a></b>"
                Else
                    trans_text_client = "&nbsp;&nbsp;<b><a href='#' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&new=true','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');"">Add New Transaction</a></b>"
                End If
            End If

            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
                trans_text_client = trans_text_client & "<table width='150' class='engine' style='float:right'><tr><td align='left' valign='top' bgcolor='navy'>&nbsp;&nbsp;</td><td align='left' valign='top' class='blue_client'><b>Jetnet Information</b></td></tr>"

                If otherID = 0 And source = "JETNET" Then

                Else
                    trans_text_client = trans_text_client & "<tr><td align='left' valign='top' bgcolor='#7a3733'>&nbsp;&nbsp;</td><td align='left' valign='top' class='red_client'><b>Client Information</b></td></tr>"
                End If

                trans_text_client = trans_text_client & "</table>"
            End If
            'trans_text_client += "<img src='images/transactions.jpg' alt='Transaction Information' border='0' /><br clear='all' /><br /><span class=""emphasis_text red"">*Click on the pencil icon to edit transactions</span><br /><br />"
            trans_text_client += "<table width='" & IIf(HttpContext.Current.Session("isMobile") = False, "100%", "100%") & "' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
            trans_text_client = trans_text_client & "<tr class='header_row'>"


            trans_text_client = trans_text_client & "<td align='left' valign='top'></td>"


            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Date:</b></td>"
            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Description</b></td><td align='left' valign='top'><b>Listing Date</b></td>"
            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Asking Price ($k)</b></td>"
            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
                trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Est. Price ($k)</b></td>"
                trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Sold Price ($k)</b></td>"
            End If
            'If HttpContext.Current.Session("isMobile") = False Then
            '  trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Document</b></td>"
            'End If
            trans_text_client = trans_text_client & "</tr>"
            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
                'If jetnet_id_transaction <> 0 Then
                Try
                    color = ""

                    trans_text = "<tr>"
                    '''''''''''''''''''''''''''''''''''''''''''''Getting the client data ''''''''''''''''''''''''''''''''''''''''
                    'If client_id <> 0 Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Transactions_ACid(client_id, jetnet_id)
                    client_transactions_hold = masterpage.aTempTable
                    'Else
                    'masterpage.atemptable = New DataTable
                    'End If


                    '''''''''''''''''''''''''''''''''''''''''''''getting the jetnet data'''''''''''''''''''''''''''''''''''''''''''
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.GetJETNET_Transactions_acID(jetnet_id)


                    If Not IsNothing(masterpage.aTempTable2) Then
                        If masterpage.aTempTable2.Rows.Count > 0 Then
                            For Each r As DataRow In masterpage.aTempTable2.Rows
                                trans_date = ""
                                trans_subject = ""
                                'action_date = ""
                                asking_price = ""
                                est_price = ""
                                sold_price = ""
                                listing_date = ""
                                edit_link = ""
                                id_array = id_array & r("trans_id") & ","
                                ''''''''''''''''''''''''''''''''''''''initializing these data tables
                                atemptable4 = New DataTable
                                atemptable4 = client_transactions_hold.Clone
                                client_transactions_search = client_transactions_hold

                                '''''''''''''''''''''''''''''''''''''''search through client table to find matching jetnet trans ID. 
                                If client_transactions_search.Rows.Count > 0 Then

                                    Dim afiltered_Client As DataRow() = client_transactions_search.Select("clitrans_jetnet_trans_id = '" & r("trans_id") & "'", "")
                                    ''''''''''''''''''''''''''''''''''store in atemptable4
                                    For Each atmpDataRow_Client In afiltered_Client
                                        atemptable4.ImportRow(atmpDataRow_Client)
                                    Next
                                End If

                                '''''''''''''''''''''''''''''''''''''''if matching client transaction has been found.
                                If Not IsNothing(atemptable4) Then
                                    If atemptable4.Rows.Count > 0 Then
                                        For Each q As DataRow In atemptable4.Rows
                                            If q("clitrans_jetnet_trans_id") = r("trans_id") And q("clitrans_jetnet_trans_id") <> 0 Then
                                                If HttpContext.Current.Session("isMobile") = False Then 'giving an edit link if not mobile.
                                                    edit_link = "<a href='JavaScript:void();' onclick='return false;'><img src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right"" border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                                    If source = "CLIENT" Then 'giving remove link.
                                                        'edit_link = edit_link & "<br /><br /><a onclick=""return confirm('Do you really want remove this transaction?')"" href=""javascript:load('edit.aspx?action=edit&remove=true&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');"" ><img src='images/remove.gif' class='float_right' alt='REMOVE CLIENT INFORMATION' border='0' /></a>"
                                                    End If
                                                End If
                                                ''''''''''''''''''''''transaction date'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_date")) Then
                                                    trans_date = FormatDateTime(q("clitrans_date"), 2)
                                                End If
                                                '''''''''''''''''''''''transaction client'''''''''''''''''''''''''''''
                                                trans_subject = "<b class='red_client'>" & q("clitrans_subject") & "</b>"
                                                '''''''''''''''''''''''transaction action date.'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_date_listed")) Then
                                                    listing_date = FormatDateTime(q("clitrans_date_listed"), 2)
                                                End If
                                                ''''''''''''''''''''''''transaction asking price'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_asking_price")) Then
                                                    If q("clitrans_asking_price") > 0 Then
                                                        asking_price = "<b class='red_client'>" & FormatCurrency((q("clitrans_asking_price") / 1000), 0) & "</b>"
                                                    End If
                                                End If
                                                '''''''''''''''''''''''''transaction estimated price'''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_est_price")) Then
                                                    If q("clitrans_est_price") > 0 Then
                                                        est_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_est_price") / 1000), 0)) & "</b>"
                                                    End If
                                                End If
                                                '''''''''''''''''''''''''transaction sold price'''''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_sold_price")) Then
                                                    If q("clitrans_sold_price") > 0 Then
                                                        ''''''''''''''''''''''transaction sold price type''''''''''''''''''''''''' 
                                                        If q("clitrans_sold_price_type") = "F" Then
                                                            sold_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_sold_price") / 1000), 0)) & " (<em>Firm</em>)" & "</b>"
                                                        Else
                                                            sold_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_sold_price") / 1000), 0)) & " (<em>Estimated</em>)" & "</b>"
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                Else
                                    If masterpage.aclsData_Temp.class_error <> "" Then
                                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                                        masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                                    End If
                                    masterpage.display_error()
                                End If

                                color = "alt_row"

                                'building transaction doc subject links.
                                subject_link = "<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & jetnet_id & "&jid=" & r("trans_id") & "','scrollbars=yes,menubar=no,height=900,width=1180,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                If r("trans_id") = old Then
                                    'if this transaction has already been displayed, just display document link.
                                    'If HttpContext.Current.Session("isMobile") = False Then
                                    '  If Not IsDBNull(r("tdoc_pdf_exist_flag")) Then
                                    '    If r("tdoc_pdf_exist_flag") = "Y" Then
                                    '      trans_text = trans_text & link & "<img src='images/final.jpg' alt='' border='0' /></a>&nbsp;&nbsp;"
                                    '    End If
                                    '  End If
                                    '  trans_text = trans_text & "<b class='blue_jetnet'>" & r("Tdoc_doc_type") & "</b><br />"
                                    'End If
                                Else 'else the transaction row hasn't been displayed yet.
                                    If old <> 0 Then
                                        trans_text = trans_text & "</td></tr>"
                                    End If
                                    trans_text = trans_text & "<tr class='" & color & "'>"
                                    'if the other ID of this transaction doesn't equal zero. Meaning it's a jetnet record, but it has a client ID.
                                    If otherID <> 0 Then
                                        If edit_link = "" Then
                                            trans_text = trans_text & "<td align='left' valign='top'>"
                                            If HttpContext.Current.Session("isMobile") = False Then
                                                trans_text = trans_text & "<a href='JavaScript:void();' onclick='return false;'><img  src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right"" border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & r("trans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1030,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                            End If
                                            trans_text = trans_text & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & edit_link & "</td>"
                                        End If

                                    Else 'elsei t's a jetnet record and doesn't have a client side - the link changes to automatically recreate this aircraft. 
                                        trans_text = trans_text & "<td align='left' valign='top'>"
                                        If HttpContext.Current.Session("isMobile") = False Then
                                            If edit_link <> "" Then
                                                trans_text = trans_text & edit_link
                                                edit_link = ""
                                            Else
                                                trans_text = trans_text & "<a href='JavaScript:void();' onclick='return false;'><img  src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right""  border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & r("trans_id") & "','','scrollbars=yes,menubar=no,height=880,width=900,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                            End If
                                        End If
                                        trans_text = trans_text & "</td>"
                                    End If
                                    ''''''''''''''''''''''''''''''''''''''''''''Transaction Date''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'><b class='blue_jetnet'>" & r("trans_date") & "</b><br />"
                                    If Not IsDBNull(trans_date) Then
                                        If trans_date <> "" Then
                                            If r("Trans_date") <> trans_date Then
                                                trans_text = trans_text & "<b class='red_client'>" & trans_date & "</b>"
                                            End If
                                        End If
                                    End If
                                    trans_text = trans_text & "</td>"
                                    '''''''''''''''''''''''''''''''''''''''''''''Transaction Subject'''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'><b class='blue_jetnet'>" & subject_link & r("trans_subject") & "</a></b><br />"

                                    If r("Trans_subject") <> trans_subject Then
                                        trans_text = trans_text & trans_subject
                                    End If
                                    trans_text = trans_text & "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Transaction Action Date''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'>"


                                    If Not IsDBNull(r("ac_list_date")) Then
                                        trans_text += "<span class='blue_jetnet'>" & r("ac_list_date") & "</span>"
                                    End If
                                    If r("ac_list_date").ToString <> listing_date Then
                                        trans_text += "<br /><b class='red_client'>" & listing_date & "</b>"
                                    End If

                                    trans_text += "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Asking Price''''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'>"

                                    If Not IsDBNull(r("ac_asking_price")) Then

                                        show_asking = False
                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            show_asking = True
                                        ElseIf Not IsDBNull(r("ac_asking")) Then
                                            If Trim(r("ac_asking")) = "Price" Then
                                                show_asking = True
                                            End If
                                        End If

                                        If show_asking = True Then
                                            trans_text += "<span class='blue_jetnet'>"

                                            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                                If Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "" Then
                                                    trans_text += "<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'>"
                                                End If
                                            End If


                                            trans_text += FormatCurrency((r("ac_asking_price") / 1000), 0) & ""

                                            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                                If Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "" Then
                                                    trans_text += "</a>"
                                                End If
                                            End If

                                            trans_text += "</span>"
                                        End If
                                    End If

                                    If r("ac_asking_price").ToString <> asking_price Then
                                        If Trim(asking_price) <> "" Then
                                            If IsNumeric(asking_price) Then
                                                If CInt(asking_price) > 0 Then
                                                    asking_price = (asking_price / 1000)
                                                End If
                                                trans_text += "<br /><span class='red_client'>" & asking_price & "</span>"
                                            Else
                                                If InStr(asking_price, "red_client") > 0 Then
                                                    trans_text += "<br />" & asking_price & ""
                                                Else
                                                    trans_text += "<br /><span class='red_client'>" & asking_price & "</span>"
                                                End If

                                            End If
                                        End If

                                    End If

                                    trans_text += "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Estimated Price'''''''''''''''''''''''''''''''''''''''''''''''
                                    If Trim(est_price) <> "" Then
                                        If IsNumeric(est_price) Then
                                            If CInt(est_price) > 0 Then
                                                est_price = (est_price / 1000)
                                            End If
                                            trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                        End If
                                    Else
                                        trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                    End If



                                    ''''''''''''''''''''''''''''''''''''''''''''''Sold Price''''''''''''''''''''''''''''''''''''''''''''''''''''

                                    ' if there is a sold price, from a client record, then enter ? 
                                    If Trim(sold_price) <> "" Then
                                        If IsNumeric(sold_price) Then
                                            If sold_price > 0 Then
                                                sold_price = (sold_price / 1000)
                                                sold_price = FormatCurrency(sold_price, 0)
                                            End If
                                            trans_text = trans_text & "<td align='left' valign='top'>" & sold_price & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & sold_price & "</td>"
                                        End If
                                    Else
                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_sale_price")) Then
                                                sold_price = r("ac_sale_price")
                                                If CInt(sold_price) > 0 Then
                                                    sold_price = (sold_price / 1000)
                                                    sold_price = FormatCurrency(sold_price, 0)
                                                    trans_text &= "<td align='left' valign='top'>"
                                                    trans_text &= DisplayFunctions.TextToImage(sold_price, 7, "", "40", "Reported Sale Price Displayed with Permission from Source")
                                                    trans_text &= "</td>"
                                                Else
                                                    trans_text = trans_text & "<td align='left' valign='top'></td>"
                                                End If
                                            Else
                                                trans_text = trans_text & "<td align='left' valign='top'></td>"
                                            End If
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'></td>"
                                        End If
                                    End If


                                    ''If this isn't a mobile view, show the documents and document type. 
                                    'If HttpContext.Current.Session("isMobile") = False Then
                                    '  trans_text = trans_text & "<td align='left' valign='top' width='50'>"
                                    '  If Not IsDBNull(r("tdoc_pdf_exist_flag")) Then
                                    '    If r("tdoc_pdf_exist_flag") = "Y" Then
                                    '      trans_text = trans_text & link & "<img src='images/final.jpg' alt='' border='0' /></a>&nbsp;&nbsp;"
                                    '    End If
                                    '  End If

                                    '  trans_text = trans_text & "<b class='blue_jetnet'>" & r("Tdoc_doc_type") & "</b><br />"
                                    'End If
                                End If
                                old = r("trans_id")

                            Next

                        Else ' 0 rows
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                    '''''''''''''''''''''''''''''''''''''''end jetnet / client transaction merged view.'''''''''''''''''''''''''''''''''''
                    id_array = id_array.TrimEnd(",")
                    ''''''''''''''''''''''''''''''''''''''initializing these data tables
                    atemptable4 = New DataTable
                    atemptable4 = client_transactions_hold.Clone
                    client_transactions_search = client_transactions_hold

                    '''''''''''''''''''''''''''''''''''''''search through client table to find matching jetnet trans ID. 
                    If client_transactions_search.Rows.Count > 0 Then
                        Dim afiltered_Client As DataRow() = client_transactions_search.Select("clitrans_jetnet_trans_id not in (" & id_array & ") or clitrans_jetnet_trans_id = 0 ", "")
                        ''''''''''''''''''''''''''''''''''store in atemptable4
                        For Each atmpDataRow_Client In afiltered_Client
                            atemptable4.ImportRow(atmpDataRow_Client)
                        Next
                    End If



                    masterpage.aTempTable = atemptable4


                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In masterpage.aTempTable.Rows
                                '  If q("clitrans_jetnet_trans_id") = 0 Then
                                '''''''''''''''''''''''''''''''''''''''''displaying this data only if there isn't a transID for the jetnet side.''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & "<tr class='alt_row'>"

                                trans_text_client = trans_text_client & "<td align='left' valign='top'>"
                                ''''''''''''''''''''''''''''''''''''if not mobile, allow editing''''''''''''''''''''''''''''''''''''
                                If HttpContext.Current.Session("isMobile") = False Then
                                    trans_text_client = trans_text_client & "<a href='JavaScript:void();' onclick='return false;'><img src='images/edit_icon.png' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1030,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                    If source = "CLIENT" Then
                                        ''''''''''''''''''''''''''''''''''''if client, allow remove''''''''''''''''''''''''''''''''''''
                                        trans_text_client = trans_text_client & "<a onclick=""return confirm('Do you really want remove this transaction?')"" href=""javascript:load('edit.aspx?action=edit&remove=true&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/remove.gif' class='float_left' alt='REMOVE CLIENT INFORMATION' border='0' /></a>"
                                    End If
                                End If
                                trans_text_client = trans_text_client & "</td>"
                                ''''''''''''''''''''''''''''''''''''transaction date''''''''''''''''''''''''''''''''''''
                                If Not IsDBNull(q("clitrans_date")) Then
                                    trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>" & FormatDateTime(q("clitrans_date"), 2) & "</td>"
                                Else
                                    trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'></td>"
                                End If

                                trans_text_client = trans_text_client & "<td align='left' valign='top' width='200'><b class='red_client'>" & q("clitrans_subject") & "<br /></td><td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''transaction date listed''''''''''''''''''''''''''''''''''''
                                If Not IsDBNull(q("clitrans_date_listed")) Then
                                    If q("clitrans_date_listed") <> "0001-01-01 00:00:00" And q("clitrans_date_listed") <> "1/1/1900" Then
                                        trans_text_client = trans_text_client & FormatDateTime(q("clitrans_date_listed"), 2) & "</td>"
                                    Else
                                        trans_text_client = trans_text_client & "</td>"
                                    End If
                                Else
                                    trans_text_client = trans_text_client & "</td>"
                                End If
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''transaction asking price''''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & FormatCurrency(q("clitrans_asking_price"), 0) & "</td>"
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''estimated price''''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & FormatCurrency(q("clitrans_est_price"), 0) & "</td>"
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                If q("clitrans_sold_price_type") = "F" Then
                                    ''''''''''''''''''''''''''''''''''''sold price + type''''''''''''''''''''''''''''''''''''
                                    trans_text_client = trans_text_client & FormatCurrency(q("clitrans_sold_price"), 0) & " (<em>Firm</em>)</td>"
                                Else
                                    trans_text_client = trans_text_client & FormatCurrency(q("clitrans_sold_price"), 0) & " (<em>Estimated</em>)</td>"
                                End If
                                'If HttpContext.Current.Session("isMobile") = False Then
                                '  trans_text_client = trans_text_client & "<td align='left' valign='top'></td>"
                                'End If
                                trans_text_client = trans_text_client & "</tr>"
                                ' End If
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''''''end client transactions where no jetnet trans ID

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - fill_AC_text() Transaction Tab - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
            Else 'evo user

                masterpage.aTempTable2 = masterpage.aclsData_Temp.GetJETNET_Transactions_acID(jetnet_id)


                If Not IsNothing(masterpage.aTempTable2) Then
                    If masterpage.aTempTable2.Rows.Count > 0 Then
                        old = masterpage.aTempTable2.rows(0).item("trans_id")
                        For Each r As DataRow In masterpage.aTempTable2.Rows
                            subject_link = "<a href='/DisplayAircraftDetails.aspx?securityToken=" & IIf(Not IsDBNull(HttpContext.Current.Session.Item("localUser").crmSecurityToken), HttpContext.Current.Session.Item("localUser").crmSecurityToken, "") & "&acid=" & jetnet_id & "&jid=" & r("trans_id") & "' target='new'>"
                            compare = 0
                            count = count + 1

                            If count < masterpage.atemptable2.rows.count Then
                                compare = masterpage.atemptable2.rows(count).item("trans_id")
                            Else
                                compare = 0
                            End If
                            If old = r("trans_id") And old = compare Then
                                'doc_list = doc_list & link & r("tdoc_doc_type") & "</a><br />"
                            Else
                                'doc_list = doc_list & link & r("tdoc_doc_type") & "</a><br />"
                                trans_text = trans_text & "<tr><td align='left' valign='top'></td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("trans_date") & "</td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & subject_link & r("trans_subject") & " " & r("journ_customer_note") & "</a></td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("ac_list_date") & "</td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("ac_asking_price") & "</td>"
                                'trans_text = trans_text & "<td align='left' valign='top'>" & doc_list & "</td>"
                                trans_text += "</tr>"
                                doc_list = ""
                            End If

                            old = r("trans_id")

                        Next
                    Else
                        trans_text = trans_text & "<tr><td colspan='6' align='left' valign='top'><p align='center'>There is no historical transactions for this aircraft.</p></td>"
                    End If
                End If


            End If

            If type = "both" Then
                trans_text = trans_text_client & trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

            ElseIf type = "jetnet" Then
                trans_text = trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

                trans_text_client = trans_text_client & "</td></tr></table>"
                client_label.Text = trans_text_client
            ElseIf type = "client" Then
                trans_text = trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

                trans_text_client = trans_text_client & "</td></tr></table>"
                client_label.Text = trans_text_client

            End If
            'Return trans_text
            'trans_label.Text = trans_text_client & trans_text & "</td></tr></table>"
            trans_text = ""
            masterpage.aTempTable2 = Nothing
            masterpage.aTempTable = Nothing
        End Sub


        Public Shared Sub Build_Transaction_Tab_Company(ByVal jetnet_id As Integer, ByVal client_id As Integer, ByVal otherID As Integer, ByVal listingID As Integer, ByVal source As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal type As String, ByVal jetnet_label As Label, ByVal client_label As Label, ByVal viewAll As Boolean)
            '----------------------------------Transactions Information-------------------------------------------------------
            Dim masterpage As Object

            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Dim id_array As String = ""
            Dim client_transactions_hold As New DataTable
            Dim client_transactions_search As New DataTable
            Dim atemptable4 As New DataTable
            Dim trans_text_client As String = ""
            Dim trans_date As String = ""
            Dim trans_subject As String = ""
            ' Dim action_date As String = ""
            Dim asking_price As String = ""
            Dim est_price As String = ""
            Dim sold_price As String = ""
            Dim listing_date As String = ""
            Dim edit_link As String = ""
            Dim trans_text As String = ""
            Dim color As String = ""
            Dim old As Integer = 0
            Dim link As String = ""
            Dim subject_link As String = ""
            Dim doc_list As String = ""
            Dim count As Integer = 0
            Dim compare As Integer = 0
            Dim show_asking As Boolean = False
            Dim trans_aircraft As String = ""
            Dim fullClient_AC As String = ""

            Dim ErrorField As String = ""
            If HttpContext.Current.Session("isMobile") = False And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
                If otherID = 0 And source = "JETNET" Then
                    trans_text_client = "&nbsp;&nbsp;<b class=""float_left""><a href='#' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&new=true&acID=" & jetnet_id & "&source=JETNET','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');"">Add New Transaction</a></b>"
                Else
                    trans_text_client = "&nbsp;&nbsp;<b class=""float_left""><a href='#' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&new=true','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');"">Add New Transaction</a></b>"
                End If
            End If

            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
                trans_text_client = trans_text_client & "<table width='150' class='engine' style='float:right'><tr><td align='left' valign='top' bgcolor='navy'>&nbsp;&nbsp;</td><td align='left' valign='top' class='blue_client'><b>Jetnet Information</b></td></tr>"

                If otherID = 0 And source = "JETNET" Then

                Else
                    trans_text_client = trans_text_client & "<tr><td align='left' valign='top' bgcolor='#7a3733'>&nbsp;&nbsp;</td><td align='left' valign='top' class='red_client'><b>Client Information</b></td></tr>"
                End If

                trans_text_client = trans_text_client & "</table>"
            End If
            'trans_text_client += "<img src='images/transactions.jpg' alt='Transaction Information' border='0' /><br clear='all' /><br /><span class=""emphasis_text red"">*Click on the pencil icon to edit transactions</span>"
            If viewAll = False Then
                trans_text_client += "<a href=""details.aspx?trans=all&type=1&comp_id=" & HttpContext.Current.Session("ListingID") & "&source=" & HttpContext.Current.Session("ListingSource") & """ class=""float_left"" style=""padding-top:15px;clear:left;"" onclick=""javascript:  ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"">View All Transactions</a>"
            Else
                trans_text_client += "<a href=""details.aspx?trans=year&type=1&comp_id=" & HttpContext.Current.Session("ListingID") & "&source=" & HttpContext.Current.Session("ListingSource") & """ class=""float_left"" style=""padding-top:15px;clear:left;"" onclick=""javascript:  ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"">View Last Year's Transactions</a>"
            End If
            'trans_text_client += "<br />"
            trans_text_client += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
            trans_text_client = trans_text_client & "<tr class='header_row'>"


            trans_text_client = trans_text_client & "<td align='left' valign='top'></td>"


            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Date:</b></td>"
            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Description</b></td>"
            trans_text_client += "<td align='left' valign='top'><b>Aircraft</b></td>"
            trans_text_client += "<td align='left' valign='top'><b>Listing Date</b></td>"
            trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Asking Price ($k)</b></td>"
            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
                trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Est. Price ($k)</b></td>"
                trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Sold Price ($k)</b></td>"
            End If
            'If HttpContext.Current.Session("isMobile") = False Then
            '  trans_text_client = trans_text_client & "<td align='left' valign='top'><b>Document</b></td>"
            'End If
            trans_text_client = trans_text_client & "</tr>"
            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
                'If jetnet_id_transaction <> 0 Then
                Try
                    color = ""

                    trans_text = "<tr>"
                    '''''''''''''''''''''''''''''''''''''''''''''Getting the client data ''''''''''''''''''''''''''''''''''''''''
                    If client_id <> 0 Then
                        masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Transactions_CompID(client_id, viewAll)
                        client_transactions_hold = masterpage.aTempTable
                    Else
                        masterpage.atemptable = New DataTable
                    End If


                    '''''''''''''''''''''''''''''''''''''''''''''getting the jetnet data'''''''''''''''''''''''''''''''''''''''''''
                    masterpage.aTempTable2 = masterpage.aclsData_Temp.TransactionsCompanyDetailsByJetnetID(jetnet_id, False, viewAll)


                    If Not IsNothing(masterpage.aTempTable2) Then
                        If masterpage.aTempTable2.Rows.Count > 0 Then
                            For Each r As DataRow In masterpage.aTempTable2.Rows
                                trans_date = ""
                                trans_subject = ""
                                'action_date = ""
                                asking_price = ""
                                est_price = ""
                                sold_price = ""
                                listing_date = ""
                                edit_link = ""
                                trans_aircraft = ""
                                fullClient_AC = ""
                                id_array = id_array & r("trans_id") & ","
                                ''''''''''''''''''''''''''''''''''''''initializing these data tables
                                atemptable4 = New DataTable
                                atemptable4 = client_transactions_hold.Clone
                                client_transactions_search = client_transactions_hold

                                '''''''''''''''''''''''''''''''''''''''search through client table to find matching jetnet trans ID. 
                                If client_transactions_search.Rows.Count > 0 Then

                                    Dim afiltered_Client As DataRow() = client_transactions_search.Select("clitrans_jetnet_trans_id = '" & r("trans_id") & "'", "")
                                    ''''''''''''''''''''''''''''''''''store in atemptable4
                                    For Each atmpDataRow_Client In afiltered_Client
                                        atemptable4.ImportRow(atmpDataRow_Client)
                                    Next
                                End If

                                '''''''''''''''''''''''''''''''''''''''if matching client transaction has been found.
                                If Not IsNothing(atemptable4) Then
                                    If atemptable4.Rows.Count > 0 Then
                                        For Each q As DataRow In atemptable4.Rows
                                            If q("clitrans_jetnet_trans_id") = r("trans_id") And q("clitrans_jetnet_trans_id") <> 0 Then
                                                If HttpContext.Current.Session("isMobile") = False Then 'giving an edit link if not mobile.
                                                    edit_link = "<a href='JavaScript:void();' onclick='return false;'><img src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right"" border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1130,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                                    If source = "CLIENT" Then 'giving remove link.
                                                        'edit_link = edit_link & "<br /><br /><a onclick=""return confirm('Do you really want remove this transaction?')"" href=""javascript:load('edit.aspx?action=edit&remove=true&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');"" ><img src='images/remove.gif' class='float_right' alt='REMOVE CLIENT INFORMATION' border='0' /></a>"
                                                    End If
                                                End If
                                                ''''''''''''''''''''''transaction date'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_date")) Then
                                                    trans_date = FormatDateTime(q("clitrans_date"), 2)
                                                End If
                                                '''''''''''''''''''''''transaction client'''''''''''''''''''''''''''''
                                                trans_subject = "<b class='red_client'>" & q("clitrans_subject") & "</b>"
                                                fullClient_AC += q("cliamod_make_name") & " " & q("cliamod_model_name") & " "
                                                fullClient_AC += "<a href='details.aspx?type=3&source=CLIENT&ac_id=" & q("clitrans_cliac_id") & "'>"

                                                If Not IsDBNull(q("clitrans_ser_nbr")) Then
                                                    trans_aircraft += q("clitrans_ser_nbr").ToString
                                                    fullClient_AC += "<br />Ser #: " & q("clitrans_ser_nbr").ToString
                                                End If

                                                fullClient_AC += "</a>"

                                                '''''''''''''''''''''''transaction action date.'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_date_listed")) Then
                                                    listing_date = FormatDateTime(q("clitrans_date_listed"), 2)
                                                End If
                                                ''''''''''''''''''''''''transaction asking price'''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_asking_price")) Then
                                                    If q("clitrans_asking_price") > 0 Then
                                                        asking_price = "<b class='red_client'>" & FormatCurrency((q("clitrans_asking_price") / 1000), 0) & "</b>"
                                                    End If
                                                End If
                                                '''''''''''''''''''''''''transaction estimated price'''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_est_price")) Then
                                                    If q("clitrans_est_price") > 0 Then
                                                        est_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_est_price") / 1000), 0)) & "</b>"
                                                    End If
                                                End If
                                                '''''''''''''''''''''''''transaction sold price'''''''''''''''''''''''''''''''
                                                If Not IsDBNull(q("clitrans_sold_price")) Then
                                                    If q("clitrans_sold_price") > 0 Then
                                                        ''''''''''''''''''''''transaction sold price type''''''''''''''''''''''''' 
                                                        If q("clitrans_sold_price_type") = "F" Then
                                                            sold_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_sold_price") / 1000), 0)) & " (<em>Firm</em>)" & "</b>"
                                                        Else
                                                            sold_price = "<b class='red_client'>" & CStr(FormatCurrency((q("clitrans_sold_price") / 1000), 0)) & " (<em>Estimated</em>)" & "</b>"
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                Else
                                    If masterpage.aclsData_Temp.class_error <> "" Then
                                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                                        masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                                    End If
                                    masterpage.display_error()
                                End If

                                color = "alt_row"

                                'building transaction doc subject links.
                                subject_link = "<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & r("ac_id") & "&jid=" & r("trans_id") & "','scrollbars=yes,menubar=no,height=900,width=1180,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                                If r("trans_id") = old Then
                                    'if this transaction has already been displayed, just display document link.
                                    'If HttpContext.Current.Session("isMobile") = False Then
                                    '  If Not IsDBNull(r("tdoc_pdf_exist_flag")) Then
                                    '    If r("tdoc_pdf_exist_flag") = "Y" Then
                                    '      trans_text = trans_text & link & "<img src='images/final.jpg' alt='' border='0' /></a>&nbsp;&nbsp;"
                                    '    End If
                                    '  End If
                                    '  trans_text = trans_text & "<b class='blue_jetnet'>" & r("Tdoc_doc_type") & "</b><br />"
                                    'End If
                                Else 'else the transaction row hasn't been displayed yet.
                                    If old <> 0 Then
                                        trans_text = trans_text & "</td></tr>"
                                    End If
                                    trans_text = trans_text & "<tr class='" & color & "'>"
                                    'if the other ID of this transaction doesn't equal zero. Meaning it's a jetnet record, but it has a client ID.
                                    If otherID <> 0 Then
                                        If edit_link = "" Then
                                            trans_text = trans_text & "<td align='left' valign='top'>"
                                            If HttpContext.Current.Session("isMobile") = False Then
                                                trans_text = trans_text & "<a href='JavaScript:void();' onclick='return false;'><img  src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right"" border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & r("trans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1030,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                            End If
                                            trans_text = trans_text & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & edit_link & "</td>"
                                        End If

                                    Else 'elsei t's a jetnet record and doesn't have a client side - the link changes to automatically recreate this aircraft. 
                                        trans_text = trans_text & "<td align='left' valign='top'>"
                                        If HttpContext.Current.Session("isMobile") = False Then
                                            If edit_link <> "" Then
                                                trans_text = trans_text & edit_link
                                                edit_link = ""
                                            Else
                                                trans_text = trans_text & "<a href='JavaScript:void();' onclick='return false;'><img  src=""images/edit_icon.png"" alt=""Edit Transaction"" title=""Edit this Transaction"" class=""help_cursor float_right""  border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & r("trans_id") & "','','scrollbars=yes,menubar=no,height=880,width=900,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                            End If
                                        End If
                                        trans_text = trans_text & "</td>"
                                    End If
                                    ''''''''''''''''''''''''''''''''''''''''''''Transaction Date''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'><b class='blue_jetnet'>" & FormatDateTime(r("trans_date"), 2) & "</b><br />"
                                    ErrorField = "date"
                                    If Not IsDBNull(trans_date) Then
                                        If trans_date <> "" Then
                                            If r("Trans_date") <> trans_date Then
                                                trans_text = trans_text & "<b class='red_client'>" & trans_date & "</b>"
                                            End If
                                        End If
                                    End If
                                    ErrorField = "date2"
                                    trans_text = trans_text & "</td>"
                                    '''''''''''''''''''''''''''''''''''''''''''''Transaction Subject'''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'><b class='blue_jetnet'>" & subject_link & r("trans_subject") & "</a></b><br />"
                                    ErrorField = "subject"
                                    If r("Trans_subject") <> trans_subject Then
                                        trans_text = trans_text & trans_subject
                                    End If
                                    ErrorField = "subject2"
                                    trans_text = trans_text & "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''AC Info''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text += "<td align='left' valign='top'><b class='blue_jetnet'>" & r("amod_make_name") & " " & r("amod_model_name") & "<br /><a href='details.aspx?source=JETNET&type=3&ac_id=" & r("ac_id") & "'>Ser #: " & r("ac_ser_no_full") & "</a></b><br />"
                                    If r("ac_ser_no_full") <> trans_aircraft Then
                                        trans_text = trans_text & trans_aircraft
                                    End If
                                    trans_text += "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Transaction Action Date''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'>"


                                    ErrorField = "ldate"
                                    If Not IsDBNull(r("ac_list_date")) Then
                                        trans_text += "<span class='blue_jetnet'>" & FormatDateTime(r("ac_list_date"), 2) & "</span>"
                                    End If

                                    If r("ac_list_date").ToString <> listing_date Then
                                        trans_text += "<br /><b class='red_client'>" & listing_date & "</b>"
                                    End If
                                    ErrorField = "ldate2"
                                    trans_text += "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Asking Price''''''''''''''''''''''''''''''''''''''''''''''''''
                                    trans_text = trans_text & "<td align='left' valign='top'>"
                                    ErrorField = "askingprice"
                                    If Not IsDBNull(r("ac_asking_price")) Then


                                        show_asking = False
                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            show_asking = True
                                        ElseIf Not IsDBNull(r("ac_asking")) Then
                                            If Trim(r("ac_asking")) = "Price" Then
                                                show_asking = True
                                            End If
                                        End If

                                        If show_asking = True Then
                                            trans_text += "<span class='blue_jetnet'>"

                                            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                                If Not IsDBNull(r("ac_asking")) Then
                                                    If Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "" Then
                                                        trans_text += "<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'>"
                                                    End If
                                                End If
                                            End If



                                            trans_text += FormatCurrency((r("ac_asking_price") / 1000), 0) & ""

                                            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                                If Not IsDBNull(r("ac_asking")) Then
                                                    If Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "" Then
                                                        trans_text += "</a>"
                                                    End If
                                                End If
                                            End If

                                            trans_text += "</span>"
                                        End If
                                    End If

                                    If Not IsDBNull(r("ac_asking_price")) Then
                                        If r("ac_asking_price").ToString <> asking_price Then
                                            If Trim(asking_price) <> "" Then
                                                If IsNumeric(asking_price) Then
                                                    If CInt(asking_price) > 0 Then
                                                        asking_price = (asking_price / 1000)
                                                    End If
                                                    trans_text += "<br /><span class='red_client'>" & asking_price & "</span>"
                                                Else
                                                    If InStr(asking_price, "red_client") > 0 Then
                                                        trans_text += "<br />" & asking_price & ""
                                                    Else
                                                        trans_text += "<br /><span class='red_client'>" & asking_price & "</span>"
                                                    End If

                                                End If
                                            End If

                                        End If
                                    End If
                                    ErrorField = "askingprice2"
                                    trans_text += "</td>"
                                    ''''''''''''''''''''''''''''''''''''''''''''''Estimated Price'''''''''''''''''''''''''''''''''''''''''''''''
                                    ErrorField = "eprice"
                                    If Trim(est_price) <> "" Then
                                        If IsNumeric(est_price) Then
                                            If CInt(est_price) > 0 Then
                                                est_price = (est_price / 1000)
                                            End If
                                            trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                        End If
                                    Else
                                        trans_text = trans_text & "<td align='left' valign='top'>" & est_price & "</td>"
                                    End If
                                    ErrorField = "eprice2"


                                    ''''''''''''''''''''''''''''''''''''''''''''''Sold Price''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    ErrorField = "sprice"
                                    ' if there is a sold price, from a client record, then enter ? 
                                    If Trim(sold_price) <> "" Then
                                        If IsNumeric(sold_price) Then
                                            If sold_price > 0 Then
                                                sold_price = (sold_price / 1000)
                                                sold_price = FormatCurrency(sold_price, 0)
                                            End If
                                            trans_text = trans_text & "<td align='left' valign='top'>" & sold_price & "</td>"
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'>" & sold_price & "</td>"
                                        End If
                                    Else
                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_sale_price")) Then
                                                sold_price = r("ac_sale_price")
                                                If CInt(sold_price) > 0 Then
                                                    sold_price = (sold_price / 1000)
                                                    sold_price = FormatCurrency(sold_price, 0)
                                                    trans_text &= "<td align='left' valign='top'>"
                                                    trans_text &= DisplayFunctions.TextToImage(sold_price, 7, "", "40", "Reported Sale Price Displayed with Permission from Source")
                                                    trans_text &= "</td>"
                                                Else
                                                    trans_text = trans_text & "<td align='left' valign='top'></td>"
                                                End If
                                            Else
                                                trans_text = trans_text & "<td align='left' valign='top'></td>"
                                            End If
                                        Else
                                            trans_text = trans_text & "<td align='left' valign='top'></td>"
                                        End If
                                    End If
                                    ErrorField = "sprice2"

                                    ''If this isn't a mobile view, show the documents and document type. 
                                    'If HttpContext.Current.Session("isMobile") = False Then
                                    '  trans_text = trans_text & "<td align='left' valign='top' width='50'>"
                                    '  If Not IsDBNull(r("tdoc_pdf_exist_flag")) Then
                                    '    If r("tdoc_pdf_exist_flag") = "Y" Then
                                    '      trans_text = trans_text & link & "<img src='images/final.jpg' alt='' border='0' /></a>&nbsp;&nbsp;"
                                    '    End If
                                    '  End If

                                    '  trans_text = trans_text & "<b class='blue_jetnet'>" & r("Tdoc_doc_type") & "</b><br />"
                                    'End If
                                End If
                                old = r("trans_id")

                            Next

                        Else ' 0 rows
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                    '''''''''''''''''''''''''''''''''''''''end jetnet / client transaction merged view.'''''''''''''''''''''''''''''''''''
                    id_array = id_array.TrimEnd(",")
                    ''''''''''''''''''''''''''''''''''''''initializing these data tables
                    atemptable4 = New DataTable
                    atemptable4 = client_transactions_hold.Clone
                    client_transactions_search = client_transactions_hold

                    '''''''''''''''''''''''''''''''''''''''search through client table to find matching jetnet trans ID. 
                    If client_transactions_search.Rows.Count > 0 Then
                        Dim afiltered_Client As DataRow() = client_transactions_search.Select("clitrans_jetnet_trans_id not in (" & id_array & ") or clitrans_jetnet_trans_id = 0 ", "")
                        ''''''''''''''''''''''''''''''''''store in atemptable4
                        For Each atmpDataRow_Client In afiltered_Client
                            atemptable4.ImportRow(atmpDataRow_Client)
                        Next
                    End If



                    masterpage.aTempTable = atemptable4


                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In masterpage.aTempTable.Rows
                                '  If q("clitrans_jetnet_trans_id") = 0 Then
                                '''''''''''''''''''''''''''''''''''''''''displaying this data only if there isn't a transID for the jetnet side.''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & "<tr class='alt_row'>"

                                trans_text_client = trans_text_client & "<td align='left' valign='top'>"
                                ''''''''''''''''''''''''''''''''''''if not mobile, allow editing''''''''''''''''''''''''''''''''''''
                                If HttpContext.Current.Session("isMobile") = False Then
                                    trans_text_client = trans_text_client & "<a href='JavaScript:void();' onclick='return false;'><img src='images/edit_icon.png' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=880,width=1030,resizable=yes,toolbar=no,location=no,status=no');""/></a>"
                                    If source = "CLIENT" Then
                                        ''''''''''''''''''''''''''''''''''''if client, allow remove''''''''''''''''''''''''''''''''''''
                                        trans_text_client = trans_text_client & "<a onclick=""return confirm('Do you really want remove this transaction?')"" href=""javascript:load('edit.aspx?action=edit&remove=true&type=transaction&trans=" & q("clitrans_jetnet_trans_id") & "&cli_trans=" & q("clitrans_id") & "','','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/remove.gif' class='float_left' alt='REMOVE CLIENT INFORMATION' border='0' /></a>"
                                    End If
                                End If
                                trans_text_client = trans_text_client & "</td>"
                                ''''''''''''''''''''''''''''''''''''transaction date''''''''''''''''''''''''''''''''''''
                                If Not IsDBNull(q("clitrans_date")) Then
                                    trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>" & FormatDateTime(q("clitrans_date"), 2) & "</td>"
                                Else
                                    trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'></td>"
                                End If

                                trans_text_client = trans_text_client & "<td align='left' valign='top' width='200'><b class='red_client'>" & q("clitrans_subject") & "<br /></td><td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''transaction date listed''''''''''''''''''''''''''''''''''''
                                If Not IsDBNull(q("clitrans_date_listed")) Then
                                    If q("clitrans_date_listed") <> "0001-01-01 00:00:00" And q("clitrans_date_listed") <> "1/1/1900" Then
                                        trans_text_client = trans_text_client & FormatDateTime(q("clitrans_date_listed"), 2) & "</td>"
                                    Else
                                        trans_text_client = trans_text_client & "</td>"
                                    End If
                                Else
                                    trans_text_client = trans_text_client & "</td>"
                                End If
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''transaction asking price''''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & FormatCurrency(q("clitrans_asking_price"), 0) & "</td>"
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                ''''''''''''''''''''''''''''''''''''estimated price''''''''''''''''''''''''''''''''''''
                                trans_text_client = trans_text_client & FormatCurrency(q("clitrans_est_price"), 0) & "</td>"
                                trans_text_client = trans_text_client & "<td align='left' valign='top'><b class='red_client'>"
                                If q("clitrans_sold_price_type") = "F" Then
                                    ''''''''''''''''''''''''''''''''''''sold price + type''''''''''''''''''''''''''''''''''''
                                    trans_text_client = trans_text_client & FormatCurrency(q("clitrans_sold_price"), 0) & " (<em>Firm</em>)</td>"
                                Else
                                    trans_text_client = trans_text_client & FormatCurrency(q("clitrans_sold_price"), 0) & " (<em>Estimated</em>)</td>"
                                End If
                                'If HttpContext.Current.Session("isMobile") = False Then
                                '  trans_text_client = trans_text_client & "<td align='left' valign='top'></td>"
                                'End If
                                trans_text_client = trans_text_client & "</tr>"
                                ' End If
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("details.aspx.vb - fill_AC_text() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''''''end client transactions where no jetnet trans ID

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - fill_AC_text() Transaction Tab - " & ErrorField & " " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
            Else 'evo user

                masterpage.aTempTable2 = masterpage.aclsData_Temp.GetJETNET_Transactions_acID(jetnet_id)


                If Not IsNothing(masterpage.aTempTable2) Then
                    If masterpage.aTempTable2.Rows.Count > 0 Then
                        old = masterpage.aTempTable2.rows(0).item("trans_id")
                        For Each r As DataRow In masterpage.aTempTable2.Rows
                            subject_link = "<a href='/DisplayAircraftDetails.aspx?securityToken=" & IIf(Not IsDBNull(HttpContext.Current.Session.Item("localUser").crmSecurityToken), HttpContext.Current.Session.Item("localUser").crmSecurityToken, "") & "&acid=" & jetnet_id & "&jid=" & r("trans_id") & "' target='new'>"
                            compare = 0
                            count = count + 1

                            If count < masterpage.atemptable2.rows.count Then
                                compare = masterpage.atemptable2.rows(count).item("trans_id")
                            Else
                                compare = 0
                            End If
                            If old = r("trans_id") And old = compare Then
                                'doc_list = doc_list & link & r("tdoc_doc_type") & "</a><br />"
                            Else
                                'doc_list = doc_list & link & r("tdoc_doc_type") & "</a><br />"
                                trans_text = trans_text & "<tr><td align='left' valign='top'></td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("trans_date") & "</td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & subject_link & r("trans_subject") & " " & r("journ_customer_note") & "</a></td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("ac_list_date") & "</td>"
                                trans_text = trans_text & "<td align='left' valign='top'>" & r("ac_asking_price") & "</td>"
                                'trans_text = trans_text & "<td align='left' valign='top'>" & doc_list & "</td>"
                                trans_text += "</tr>"
                                doc_list = ""
                            End If

                            old = r("trans_id")

                        Next
                    Else
                        trans_text = trans_text & "<tr><td colspan='6' align='left' valign='top'><p align='center'>There is no historical transactions for this aircraft.</p></td>"
                    End If
                End If


            End If

            If type = "both" Then
                trans_text = trans_text_client & trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

            ElseIf type = "jetnet" Then
                trans_text = trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

                trans_text_client = trans_text_client & "</td></tr></table>"
                client_label.Text = trans_text_client
            ElseIf type = "client" Then
                trans_text = trans_text & "</td></tr></table>"
                jetnet_label.Text = trans_text

                trans_text_client = trans_text_client & "</td></tr></table>"
                client_label.Text = trans_text_client

            End If
            'Return trans_text
            'trans_label.Text = trans_text_client & trans_text & "</td></tr></table>"
            trans_text = ""
            masterpage.aTempTable2 = Nothing
            masterpage.aTempTable = Nothing
        End Sub

        Public Shared Function Build_JETNET_Equipment_Table_Tabs(ByVal jetnet_id As Integer, ByVal source As String, ByVal Aircraft_Data As clsClient_Aircraft, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal returned As String, ByVal Equipment_Table As DataTable) As String

            Dim aircraft_text As String = ""
            Dim cockpit_text As String = ""
            Dim equipment_text As String = ""
            Dim interior_text As String = ""
            Dim exterior_text As String = ""
            Dim maintenance_text As String = ""
            Dim color As String = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If


            If jetnet_id <> 0 Then

                Try
                    If Not IsNothing(Equipment_Table) Then
                        If Equipment_Table.Rows.Count > 0 Then
                            cockpit_text = ""
                            equipment_text = ""
                            interior_text = ""
                            exterior_text = ""
                            maintenance_text = ""
                            If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                cockpit_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br clear='all'/>"
                                equipment_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br clear='all'/>"
                            End If
                            cockpit_text = cockpit_text & "<ul class='display_tab'>"
                            equipment_text = equipment_text & "<ul class='display_tab'>"
                            For c As Integer = 0 To Equipment_Table.Rows.Count - 1
                                If Trim(Equipment_Table.Rows(c).Item("adet_data_type")) = "Interior" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    interior_text = interior_text & "<li><b>" & Equipment_Table.Rows(c).Item("adet_data_name") & ":</b> - "
                                    interior_text = interior_text & Equipment_Table.Rows(c).Item("adet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("adet_data_type")) = "Exterior" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    exterior_text = exterior_text & "<li><b>" & Equipment_Table.Rows(c).Item("adet_data_name") & ":</b> - "
                                    exterior_text = exterior_text & Equipment_Table.Rows(c).Item("adet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("adet_data_type")) = "Addl Cockpit Equipment" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    cockpit_text = cockpit_text & "<li><b>" & Equipment_Table.Rows(c).Item("adet_data_name") & ":</b> - "
                                    cockpit_text = cockpit_text & Equipment_Table.Rows(c).Item("adet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("adet_data_type")) = "Equipment" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    equipment_text = equipment_text & "<li><b>" & Equipment_Table.Rows(c).Item("adet_data_name") & ":</b> - "
                                    equipment_text = equipment_text & Equipment_Table.Rows(c).Item("adet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("adet_data_type")) = "Maintenance" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    maintenance_text = maintenance_text & "<li><b>" & Equipment_Table.Rows(c).Item("adet_data_name") & ":</b> - "
                                    maintenance_text = maintenance_text & Equipment_Table.Rows(c).Item("adet_data_description") & "</li>"
                                End If
                            Next
                            ' dump the datatable
                            Equipment_Table.Dispose()
                            Equipment_Table = Nothing
                        Else '0 rows
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Build Jetnet Equipment Table Tabs - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If

                    maintenance_text = maintenance_text & "</ul>"
                    equipment_text = equipment_text & "</ul>"
                    cockpit_text = cockpit_text & "</ul>"
                    interior_text = interior_text & "</ul>"
                    exterior_text = exterior_text & "</ul>"

                    If jetnet_id <> 0 Then
                        ' check the state of the DataTable
                        If Not IsNothing(Aircraft_Data) Then
                            Select Case returned
                                Case "cockpit"
                                    aircraft_text = cockpit_text
                                Case "equipment"
                                    aircraft_text = equipment_text
                                Case "interior"
                                    aircraft_text = ""
                                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                        aircraft_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br />"
                                    End If
                                    aircraft_text = aircraft_text & "<ul class='display_tab'><b>INTERIOR</b>"
                                    'set up the first five interior
                                    aircraft_text = aircraft_text & "<li><b>AC Interior Rating:</b> - " & Aircraft_Data.cliaircraft_interior_rating & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>Done By:</b> - " & Aircraft_Data.cliaircraft_interior_doneby_name & "</li>"
                                    If Len(Trim(Aircraft_Data.cliaircraft_interior_month_year)) > 4 Then
                                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Left(Aircraft_Data.cliaircraft_interior_month_year, 2) & "/" & Right(Aircraft_Data.cliaircraft_interior_month_year, 4) & "</li>"
                                    Else
                                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Aircraft_Data.cliaircraft_interior_month_year & "</li>"
                                    End If
                                    aircraft_text = aircraft_text & "<li><b>Passengers:</b> - " & Aircraft_Data.cliaircraft_passenger_count & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>Configuration:</b> - " & Aircraft_Data.cliaircraft_interior_config_name & "</li>"
                                    aircraft_text = aircraft_text & interior_text
                                Case "exterior"
                                    aircraft_text = ""
                                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                        aircraft_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br />"
                                    End If

                                    aircraft_text = aircraft_text & "<ul class='display_tab'><b>EXTERIOR</b>"
                                    'set up the first three exterior
                                    aircraft_text = aircraft_text & "<li><b>AC Exterior Rating:</b> - " & Aircraft_Data.cliaircraft_exterior_rating & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>Done By:</b> - " & Aircraft_Data.cliaircraft_exterior_doneby_name & "</li>"

                                    If Len(Trim(Aircraft_Data.cliaircraft_exterior_month_year)) > 4 Then
                                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Left(Aircraft_Data.cliaircraft_exterior_month_year, 2) & "/" & Right(Aircraft_Data.cliaircraft_exterior_month_year, 4) & "</li>"
                                    Else
                                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Aircraft_Data.cliaircraft_exterior_month_year & "</li>"
                                    End If

                                    aircraft_text = aircraft_text & exterior_text
                                Case "maintenance"
                                    aircraft_text = ""
                                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                        aircraft_text = "<b><img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /></b>"
                                    End If
                                    aircraft_text = aircraft_text & "<ul class='display_tab'>"
                                    aircraft_text = aircraft_text & "<li><b>Airframe Maintenance Program:</b> - "
                                    Dim program As Integer = 0
                                    Dim program_name As String = ""
                                    If Not IsDBNull(Aircraft_Data.cliaircraft_airframe_maintenance_program) Then
                                        program = Aircraft_Data.cliaircraft_airframe_maintenance_program
                                    End If
                                    Dim atemptable3 As New DataTable
                                    atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(program, 0, 0, "Airframe", False)
                                    If Not IsNothing(atemptable3) Then
                                        If atemptable3.Rows.Count > 0 Then
                                            If Not IsDBNull(atemptable3.Rows(0).Item("amp_provider_name")) Then
                                                If LCase(atemptable3.Rows(0).Item("amp_provider_name").ToString) = "unknown" Then
                                                    program_name = "Unknown"
                                                Else
                                                    program_name = atemptable3.Rows(0).Item("amp_provider_name") & " " & atemptable3.Rows(0).Item("amp_program_name")
                                                End If
                                            Else
                                                program_name = "Unknown"
                                            End If

                                        End If
                                    End If

                                    aircraft_text = aircraft_text & program_name & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>Airframe Maintenance Tracking Program:</b> - "

                                    program = 0
                                    program_name = ""
                                    If Not IsDBNull(Aircraft_Data.cliaircraft_airframe_maintenance_tracking_program) Then
                                        program = Aircraft_Data.cliaircraft_airframe_maintenance_tracking_program
                                    End If
                                    atemptable3 = New DataTable
                                    atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(0, program, 0, "Airframe", False)
                                    If Not IsNothing(atemptable3) Then
                                        If atemptable3.Rows.Count > 0 Then
                                            If Not IsDBNull(atemptable3.Rows(0).Item("amtp_program_name")) Then
                                                If LCase(atemptable3.Rows(0).Item("amtp_provider_name").ToString) = "unknown" Then
                                                    program_name = "Unknown"
                                                Else
                                                    program_name = atemptable3.Rows(0).Item("amtp_provider_name") & " " & atemptable3.Rows(0).Item("amtp_program_name")
                                                End If
                                            Else
                                                program_name = "Unknown"
                                            End If

                                        End If
                                    End If

                                    aircraft_text = aircraft_text & program_name & "</li>"
                                    aircraft_text = aircraft_text & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>AC Damage History Notes:</b> - " & Aircraft_Data.cliaircraft_damage_history_notes & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>AC Maintained:</b> - " & Aircraft_Data.cliaircraft_ac_maintained & "</li>"
                                    aircraft_text = aircraft_text & maintenance_text
                                Case "apu"
                                    aircraft_text = ""
                                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                        aircraft_text = "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' />"
                                    End If
                                    aircraft_text = aircraft_text & "<ul class='display_tab'>"
                                    ' setup the APU info
                                    aircraft_text = aircraft_text & "<li><b>APU Model Name:</b> - " & Aircraft_Data.cliaircraft_apu_model_name & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>APU Serial #:</b> - " & Aircraft_Data.cliaircraft_apu_ser_nbr & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>APU Maintenance Plan:</b> - " & Aircraft_Data.cliaircraft_apu_maintance_program & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>APU Total Time (Hours) Since New:</b> - " & Aircraft_Data.cliaircraft_apu_ttsn_hours & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>Since Overhaul (SOH) Hours:</b> - " & Aircraft_Data.cliaircraft_apu_tsoh_hours & "</li>"
                                    aircraft_text = aircraft_text & "<li><b>APU Since Hot Inspection (SHI) Hours:</b> - " & Aircraft_Data.cliaircraft_apu_tshi_hours & "</li></ul>"

                                Case "usage"
                                    aircraft_text = ""
                                    If HttpContext.Current.Session.Item("isMobile") <> True And HttpContext.Current.Session.Item("localUser").crmEvo <> True Then
                                        aircraft_text = "<b><img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /></b>"
                                    End If

                                    'If Trim(HttpContext.Current.Session.Item("useFAAFlightData")) <> "" And Trim(HttpContext.Current.Session.Item("useFAAFlightData")) <> "ARGUS" Then
                                    Dim flight_data_temp As New flightDataFunctions
                                    Dim FAATable As New DataTable

                                    flight_data_temp.serverConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
                                    flight_data_temp.clientConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase"



                                    FAATable = flight_data_temp.getAllFAAFlightData(Aircraft_Data.cliaircraft_reg_nbr, Aircraft_Data.cliaircraft_id, IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_engine_times_as_of), Aircraft_Data.cliaircraft_date_engine_times_as_of, ""))
                                    aircraft_text = aircraft_text & flight_data_temp.displayAirframeTimesData(FAATable, IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_engine_times_as_of), Aircraft_Data.cliaircraft_date_engine_times_as_of, ""), IIf(Not IsNothing(Aircraft_Data.cliaircraft_airframe_total_hours), Aircraft_Data.cliaircraft_airframe_total_hours, 0), IIf(Not IsNothing(Aircraft_Data.cliaircraft_airframe_total_landings), Aircraft_Data.cliaircraft_airframe_total_landings, 0), True, IIf(Aircraft_Data.cliaircraft_new_flag = "Y", "N", "Y"), IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_purchased), Aircraft_Data.cliaircraft_date_purchased, ""))
                                    'Else
                                    'aircraft_text = aircraft_text & "<ul class='display_tab'>"
                                    'aircraft_text = aircraft_text & "<li><b>Times/Values Current As Of:</b> - " & Aircraft_Data.cliaircraft_date_engine_times_as_of & "</li>"
                                    'aircraft_text = aircraft_text & "<li><b>Air Frame Total Time (AFTT):</b> - " & Aircraft_Data.cliaircraft_airframe_total_hours & "</li>"
                                    'aircraft_text = aircraft_text & "<li><b>Landings/Cycles:</b> - " & Aircraft_Data.cliaircraft_airframe_total_landings & "</li></ul>"
                                    'End If
                            End Select


                        End If

                    End If
                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - Build Jetnet Equipment Table Tabs - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try

            End If

            Return aircraft_text
        End Function

        Public Shared Function Build_CLIENT_Equipment_Table_Tabs(ByVal client_id As Integer, ByVal source As String, ByVal Aircraft_Data As clsClient_Aircraft, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal returned As String, ByVal Equipment_Table As DataTable) As String
            Dim aircraft_text As String = ""
            Dim cockpit_text_client As String = ""
            Dim equipment_text_client As String = ""
            Dim interior_text_client As String = ""
            Dim exterior_text_client As String = ""
            Dim maintenance_text_client As String = ""
            Dim apu_text_client As String = ""
            Dim usage_text_client As String = ""
            Dim color As String = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If


            If client_id <> 0 Then
                Try
                    If Not IsNothing(Equipment_Table) Then
                        If Equipment_Table.Rows.Count > 0 Then

                            For c As Integer = 0 To Equipment_Table.Rows.Count - 1
                                If Trim(Equipment_Table.Rows(c).Item("cliadet_data_type")) = "Interior" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    interior_text_client = interior_text_client & "<li><b>" & Equipment_Table.Rows(c).Item("cliadet_data_name") & ":</b> - "
                                    interior_text_client = interior_text_client & Equipment_Table.Rows(c).Item("cliadet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("cliadet_data_type")) = "Exterior" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    exterior_text_client = exterior_text_client & "<li><b>" & Equipment_Table.Rows(c).Item("cliadet_data_name") & ":</b> - "
                                    exterior_text_client = exterior_text_client & Equipment_Table.Rows(c).Item("cliadet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("cliadet_data_type")) = "Addl Cockpit Equipment" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    cockpit_text_client = cockpit_text_client & "<li><b>" & Equipment_Table.Rows(c).Item("cliadet_data_name") & ":</b> - "
                                    cockpit_text_client = cockpit_text_client & Equipment_Table.Rows(c).Item("cliadet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("cliadet_data_type")) = "Equipment" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    equipment_text_client = equipment_text_client & "<li><b>" & Equipment_Table.Rows(c).Item("cliadet_data_name") & ":</b> - "
                                    equipment_text_client = equipment_text_client & Equipment_Table.Rows(c).Item("cliadet_data_description") & "</li>"
                                ElseIf Trim(Equipment_Table.Rows(c).Item("cliadet_data_type")) = "Maintenance" Then
                                    If color = "alt_row" Then
                                        color = ""
                                    Else
                                        color = "alt_row"
                                    End If
                                    maintenance_text_client = maintenance_text_client & "<li><b>" & Equipment_Table.Rows(c).Item("cliadet_data_name") & ":</b> - "
                                    maintenance_text_client = maintenance_text_client & Equipment_Table.Rows(c).Item("cliadet_data_description") & "</li>"
                                End If
                            Next
                        Else '0 rows
                        End If
                    End If

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - build_client_equipment_Table_tabs - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try
                maintenance_text_client = maintenance_text_client & "</ul>"
                equipment_text_client = equipment_text_client & "</ul>"
                cockpit_text_client = cockpit_text_client & "</ul>"
                interior_text_client = interior_text_client & "</ul>"
                exterior_text_client = exterior_text_client & "</ul>"
            End If
            '------------------------------------------CLIENT TAB GENERAL INFO---------------------------------------------

            Select Case returned
                Case "interior"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=details&typeofdetails=interior','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'><b>INTERIOR</b><br />"
                    'set up the first five interior
                    aircraft_text = aircraft_text & "<li><b>AC Interior Rating:</b> - " & Aircraft_Data.cliaircraft_interior_rating & "</li>"
                    aircraft_text = aircraft_text & "<li><b>Done By:</b> - " & Aircraft_Data.cliaircraft_interior_doneby_name & "</li>"
                    If Len(Trim(Aircraft_Data.cliaircraft_interior_month_year)) > 4 Then
                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Left(Aircraft_Data.cliaircraft_interior_month_year, 2) & "/" & Right(Aircraft_Data.cliaircraft_interior_month_year, 4) & "</li>"
                    Else
                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Aircraft_Data.cliaircraft_interior_month_year & "</li>"
                    End If

                    aircraft_text = aircraft_text & "<li><b>Passengers:</b> - " & Aircraft_Data.cliaircraft_passenger_count & "</li>"
                    aircraft_text = aircraft_text & "<li><b>Configuration:</b> - " & Aircraft_Data.cliaircraft_interior_config_name & "</li>"
                    aircraft_text = aircraft_text & interior_text_client
                Case "exterior"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=details&typeofdetails=exterior','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'><b>EXTERIOR</b><br />"
                    'set up the first three exterior
                    aircraft_text = aircraft_text & "<li><b>AC Exterior Rating:</b> - " & Aircraft_Data.cliaircraft_exterior_rating & "</li>"
                    aircraft_text = aircraft_text & "<li><b>Done By:</b> - " & Aircraft_Data.cliaircraft_exterior_doneby_name & "</li>"
                    If Len(Trim(Aircraft_Data.cliaircraft_exterior_month_year)) > 4 Then
                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Left(Aircraft_Data.cliaircraft_exterior_month_year, 2) & "/" & Right(Aircraft_Data.cliaircraft_exterior_month_year, 4) & "</li>"
                    Else
                        aircraft_text = aircraft_text & "<li><b>MM/YYYY:</b> - " & Aircraft_Data.cliaircraft_exterior_month_year & "</li>"
                    End If



                    aircraft_text = aircraft_text & exterior_text_client
                Case "maintenance"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=details&typeofdetails=main','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            aircraft_text = "<img src='images/client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'>"
                    aircraft_text = aircraft_text & "<li><b>Airframe Maintenance Program:</b> - "
                    Dim program As Integer = 0
                    Dim program_name As String = ""
                    If Not IsDBNull(Aircraft_Data.cliaircraft_airframe_maintenance_program) Then
                        program = Aircraft_Data.cliaircraft_airframe_maintenance_program
                    End If
                    Dim atemptable3 As New DataTable
                    atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(program, 0, 0, "Airframe", False)
                    If Not IsNothing(atemptable3) Then

                        If atemptable3.Rows.Count > 0 Then
                            If Not IsDBNull(atemptable3.Rows(0).Item("amp_provider_name")) And Not IsDBNull(atemptable3.Rows(0).Item("amp_program_name")) Then
                                If LCase(atemptable3.Rows(0).Item("amp_provider_name").ToString) = "unknown" Or LCase(atemptable3.Rows(0).Item("amp_program_name").ToString) = "unknown" Then
                                    program_name = "Unknown"
                                Else
                                    program_name = atemptable3.Rows(0).Item("amp_provider_name") & " " & atemptable3.Rows(0).Item("amp_program_name")
                                End If
                            Else
                                program_name = "Unknown"
                            End If
                        End If
                    End If

                    aircraft_text = aircraft_text & program_name & "</li>"
                    aircraft_text = aircraft_text & "<li><b>Airframe Maintenance Tracking Program:</b> - "
                    program = 0
                    program_name = ""
                    If Not IsDBNull(Aircraft_Data.cliaircraft_airframe_maintenance_tracking_program) Then
                        program = Aircraft_Data.cliaircraft_airframe_maintenance_tracking_program
                    End If
                    atemptable3 = New DataTable
                    atemptable3 = masterpage.aclsData_Temp.lookupAirframeEngine_Mait(0, program, 0, "Airframe", False)
                    If Not IsNothing(atemptable3) Then
                        If atemptable3.Rows.Count > 0 Then
                            If Not IsDBNull(atemptable3.Rows(0).Item("amtp_provider_name")) And Not IsDBNull(atemptable3.Rows(0).Item("amtp_program_name")) Then
                                If LCase(atemptable3.Rows(0).Item("amtp_provider_name").ToString) = "unknown" Or LCase(atemptable3.Rows(0).Item("amtp_program_name").ToString) = "unknown" Then
                                    program_name = "Unknown"
                                Else
                                    program_name = atemptable3.Rows(0).Item("amtp_provider_name") & " " & atemptable3.Rows(0).Item("amtp_program_name")
                                End If
                            Else
                                program_name = "Unknown"
                            End If

                        End If
                    End If
                    aircraft_text = aircraft_text & program_name & "</li>"
                    aircraft_text = aircraft_text & "<li><b>AC Damage History Notes:</b> - " & Aircraft_Data.cliaircraft_damage_history_notes & "</li>"

                    aircraft_text = aircraft_text & "<li><b>AC Maintained:</b> - " & Aircraft_Data.cliaircraft_ac_maintained & "</li>"
                    aircraft_text = aircraft_text & maintenance_text_client
                Case "apu"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=apu','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'>"
                    ' setup the APU info
                    aircraft_text = aircraft_text & "<li><b>APU Model Name:</b> - " & Aircraft_Data.cliaircraft_apu_model_name & "</li>"
                    aircraft_text = aircraft_text & "<li><b>APU Serial #:</b> - " & Aircraft_Data.cliaircraft_apu_ser_nbr & "</li>"
                    aircraft_text = aircraft_text & "<li><b>APU Maintenance Plan:</b> - " & Aircraft_Data.cliaircraft_apu_maintance_program & "</li>"
                    aircraft_text = aircraft_text & "<li><b>APU Total Time (Hours) Since New:</b> - " & Aircraft_Data.cliaircraft_apu_ttsn_hours & "</li>"
                    aircraft_text = aircraft_text & "<li><b>Since Overhaul (SOH) Hours:</b> - " & Aircraft_Data.cliaircraft_apu_tsoh_hours & "</li>"
                    aircraft_text = aircraft_text & "<li><b>APU Since Hot Inspection (SHI) Hours:</b> - " & Aircraft_Data.cliaircraft_apu_tshi_hours & "</li></ul>"
                    aircraft_text = aircraft_text & apu_text_client
                Case "usage"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=usage','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all' />"
                        End If
                    End If


                    'If Trim(HttpContext.Current.Session.Item("useFAAFlightData")) <> "" And Trim(HttpContext.Current.Session.Item("useFAAFlightData")) <> "ARGUS" And Aircraft_Data.cliaircraft_jetnet_ac_id > 0 Then
                    Dim flight_data_temp As New flightDataFunctions
                    Dim FAATable As New DataTable

                    flight_data_temp.serverConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
                    flight_data_temp.clientConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")


                    FAATable = flight_data_temp.getAllFAAFlightData(Aircraft_Data.cliaircraft_reg_nbr, Aircraft_Data.cliaircraft_jetnet_ac_id, IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_engine_times_as_of), Aircraft_Data.cliaircraft_date_engine_times_as_of, ""))
                    aircraft_text = aircraft_text & flight_data_temp.displayAirframeTimesData(FAATable, IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_engine_times_as_of), Aircraft_Data.cliaircraft_date_engine_times_as_of, ""), IIf(Not IsNothing(Aircraft_Data.cliaircraft_airframe_total_hours), Aircraft_Data.cliaircraft_airframe_total_hours, 0), IIf(Not IsNothing(Aircraft_Data.cliaircraft_airframe_total_landings), Aircraft_Data.cliaircraft_airframe_total_landings, 0), True, IIf(Aircraft_Data.cliaircraft_new_flag = "Y", "N", "Y"), IIf(Not IsNothing(Aircraft_Data.cliaircraft_date_purchased), Aircraft_Data.cliaircraft_date_purchased, ""))
          'Else
          'aircraft_text = aircraft_text & "<ul class='display_tab_client'>"

          'aircraft_text = aircraft_text & "<li><b>Times/Values Current As Of:</b> - " & Aircraft_Data.cliaircraft_date_engine_times_as_of & "</li>"
          'aircraft_text = aircraft_text & "<li><b>Air Frame Total Time (AFTT):</b> - " & Aircraft_Data.cliaircraft_airframe_total_hours & "</li>"
          'aircraft_text = aircraft_text & "<li><b>Landings/Cycles:</b> - " & Aircraft_Data.cliaircraft_airframe_total_landings & "</li></ul>"
          'aircraft_text = aircraft_text & usage_text_client
          'End If
                Case "equipment"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=details&typeofdetails=equip','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all'/>"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all'/>"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'>"

                    aircraft_text = aircraft_text & equipment_text_client
                Case "cockpit"
                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                        If source = "CLIENT" Then
                            aircraft_text = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right' alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=details&typeofdetails=cockpit','','scrollbars=yes,menubar=no,height=500,width=500,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all'/>"
                        Else
                            aircraft_text = "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all'/>"
                        End If
                    End If
                    aircraft_text = aircraft_text & "<ul class='display_tab_client'>"
                    aircraft_text = aircraft_text & cockpit_text_client
            End Select


            Return aircraft_text

        End Function
        Public Shared Function Get_Maintenance_By_ID_Client(ByVal cliacID As Long) As DataTable
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim TempTable As New DataTable

            Try
                'Opening Connection 

                sql = "SELECT cliacmaint_id as acmaint_id, cliacmaint_name as acmaint_name, cliacmaint_complied_date as acmaint_complied_date, cliacmaint_date_type as acmaint_date_type, cliacmaint_complied_hrs as acmaint_complied_hrs, cliacmaint_due_hrs as acmaint_due_hrs, "
                sql = sql & " cliacmaint_due_date as acmaint_due_date, cliacmaint_notes as acmaint_notes, 0 as mitem_duration from client_aircraft_maintenance   "
                ' sql = sql & " INNER JOIN Maintenance_Item with (NOLOCK) ON aircraft_maintenance.acmaint_name = maintenance_item.mitem_name"
                sql = sql & " WHERE "
                sql = sql & " cliacmaint_cliac_id = " & cliacID & "  "
                sql = sql & " order by cliacmaint_complied_date asc "


                Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "", sql.ToString)


                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60
                MySqlCommand.CommandText = sql

                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Try
                    TempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
                End Try

                Get_Maintenance_By_ID_Client = TempTable

                MySqlReader.Close()

            Catch ex As Exception
                Get_Maintenance_By_ID_Client = Nothing
            Finally
                MySqlReader = Nothing

                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing

            End Try

        End Function

        Public Shared Sub populate_models(ByVal model_list As ListBox, ByVal default_vis As Boolean, ByVal c As Control, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal selected As Boolean)

            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try


                Dim models As Array = Split("", "")
                Dim models_string As String = ""
                Dim market_date As String = ""

                If default_vis = True Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Preferences()
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            market_date = CStr(IIf(Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_activity_default_days")), masterpage.aTempTable.Rows(0).Item("clipref_activity_default_days"), ""))
                        End If
                    End If

                    If Not IsDBNull(HttpContext.Current.Session.Item("localUser").crmUserDefaultModels) Then
                        models = Split(HttpContext.Current.Session.Item("localUser").crmUserDefaultModels, ",")
                    End If
                    models_string = CStr(IIf(Not IsDBNull(HttpContext.Current.Session.Item("localUser").crmUserDefaultModels), HttpContext.Current.Session.Item("localUser").crmUserDefaultModels, ""))


                    If market_date <> "" Then
                        If c.ID = "market_search" Then
                            Dim start_date As DropDownList = c.FindControl("market_time")
                            start_date.SelectedValue = market_date
                        End If
                    End If
                End If


                masterpage.aTempTable = masterpage.aclsData_Temp.Get_Combination_Models(HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops)
                ' check the state of the DataTable
                Dim val As String = ""
                model_list.Items.Clear()
                ' If models_string = "" Then
                model_list.Items.Add(New ListItem("All", "All"))
                ' End If
                If Not IsNothing(masterpage.aTempTable) Then
                    If masterpage.aTempTable.Rows.Count > 0 Then
                        For Each r As DataRow In masterpage.aTempTable.Rows
                            val = r("amod_id") & "|" & r("amod_make_name") & "|" & r("amod_model_name") & "|" & r("source") & "|" & r("client_id")
                            If models_string <> "" Then
                                If InStr(UCase(models_string), UCase(val)) > 0 Then
                                    model_list.Items.Add(New ListItem(CStr(r("amod_make_name") & " " & r("amod_model_name")), val))
                                End If
                            Else
                                model_list.Items.Add(New ListItem(CStr(r("amod_make_name") & " " & r("amod_model_name")), val))
                            End If
                        Next
                    End If
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("clsgeneral.vb - populatemodels() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If


                For x = 0 To UBound(models)
                    For j As Integer = 0 To model_list.Items.Count() - 1
                        Dim mode As String = UCase(model_list.Items(j).Value)
                        Dim et As String = UCase(models(x))
                        If UCase(model_list.Items(j).Value) = UCase(models(x)) Then
                            If selected = True Then
                                model_list.Items(j).Selected = True
                            End If
                        Else

                        End If
                    Next
                Next
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - populatemodels() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try


        End Sub

        Public Shared Sub Populate_State(ByVal state As ListBox, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            masterpage.atemptable = masterpage.aclsData_Temp.Get_Jetnet_State()
            If Not IsNothing(masterpage.atemptable) Then
                If masterpage.atemptable.Rows.Count > 0 Then
                    For Each r As DataRow In masterpage.atemptable.Rows
                        state.Items.Add(New ListItem(CStr(r("client_state")), CStr(r("client_state_abbr"))))
                    Next
                End If
            Else
                If masterpage.aclsData_Temp.class_error <> "" Then
                    masterpage.error_string = masterpage.aclsData_Temp.class_error
                    masterpage.LogError("clsgeneral.aspx.vb - populate state() - " & masterpage.error_string)
                End If
                masterpage.display_error()
            End If
        End Sub
        Public Shared Sub Populate_Country(ByVal country As DropDownList, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            masterpage.atemptable = masterpage.aclsData_Temp.Get_Jetnet_Country()
            If Not IsNothing(masterpage.atemptable) Then
                If masterpage.atemptable.Rows.Count > 0 Then
                    For Each r As DataRow In masterpage.atemptable.Rows
                        If Not IsDBNull(r("clicountry_name")) And Trim(r("clicountry_name")) <> "" Then
                            country.Items.Add(New ListItem(CStr(r("clicountry_name")), CStr(r("clicountry_name"))))
                        End If
                    Next
                End If
            Else
                If masterpage.aclsData_Temp.class_error <> "" Then
                    masterpage.error_string = masterpage.aclsData_Temp.class_error
                    masterpage.LogError("clsgeneral.aspx.vb - populate country() - " & masterpage.error_string)
                End If
                masterpage.display_error()
            End If
            country.Items.Add(New ListItem("ALL", ""))
            country.SelectedValue = ""
        End Sub

        Public Shared Sub Populate_Listbox(ByVal tempTable As DataTable, ByVal lb As ListBox, ByVal fieldtext As String, ByVal fieldvalue As String, ByVal quotes As Boolean, Optional ByVal bIsAerodex As Boolean = False, Optional ByVal bPrefixCode As Boolean = False)
            lb.Items.Clear()
            lb.Items.Add(New ListItem("All", ""))

            If Not IsNothing(tempTable) Then

                If tempTable.Rows.Count > 0 Then

                    For Each r As DataRow In tempTable.Rows

                        If Not IsDBNull(r(fieldtext)) And Trim(r(fieldvalue)) <> "" Then

                            If Not IsDBNull(r(fieldtext)) And Trim(r(fieldtext)) = "End User" Then
                                lb.Items.Add(New ListItem("Retail/End User", "EU"))
                            ElseIf bIsAerodex And Not (r.Item(fieldvalue).ToString.Contains("99") Or r.Item(fieldvalue).ToString.Contains("93")) Then
                                lb.Items.Add(New ListItem(IIf(bPrefixCode, "(" + r(fieldvalue).ToString + ") " + r(fieldtext).ToString, r(fieldtext).ToString), IIf(quotes, "'" + r(fieldvalue).ToString + "'", r(fieldvalue).ToString)))
                            ElseIf Not bIsAerodex Then
                                lb.Items.Add(New ListItem(IIf(bPrefixCode, "(" + r(fieldvalue).ToString + ") " + r(fieldtext).ToString, r(fieldtext).ToString), IIf(quotes, "'" + r(fieldvalue).ToString + "'", r(fieldvalue).ToString)))
                            End If

                        End If


                    Next

                End If

            End If

            lb.SelectedValue = ""

        End Sub

        ''' <summary>
        ''' This function and above need to be combined into one that takes either object. However I'm still working on implementing the one below so I don't want to combine both yet.
        ''' </summary>
        ''' <param name="tempTable"></param>
        ''' <param name="lb"></param>
        ''' <param name="fieldtext"></param>
        ''' <param name="fieldvalue"></param>
        ''' <param name="quotes"></param>
        ''' <remarks></remarks>
        Public Shared Sub Populate_Dropdown(ByVal tempTable As DataTable, ByVal lb As DropDownList, ByVal fieldtext As String, ByVal fieldvalue As String, ByVal quotes As Boolean)
            lb.Items.Clear()
            lb.Items.Add(New ListItem("All", ""))
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then
                    For Each r As DataRow In tempTable.Rows
                        If Not IsDBNull(r(fieldtext)) And Trim(r(fieldvalue)) <> "" Then
                            lb.Items.Add(New ListItem(CStr(r(fieldtext)), IIf(quotes = True, "'" & CStr(r(fieldvalue)) & "'", CStr(r(fieldvalue)))))
                        End If
                    Next
                End If
            End If
            lb.SelectedValue = ""
        End Sub

        Public Shared Sub Populate_Company_Category(ByVal category As DropDownList, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            category.Items.Add(New ListItem("NONE", ""))
            masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Preferences()
            If Not IsNothing(masterpage.aTempTable) Then
                If masterpage.aTempTable.Rows.Count > 0 Then
                    ' For Each r As DataRow In aTempTable.Rows
                    If Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_category1_use")) Then
                        If masterpage.aTempTable.Rows(0).Item("clipref_category1_use") = "Y" Then
                            category.Items.Add(New ListItem(masterpage.aTempTable.Rows(0).Item("clipref_category1_name"), "clicomp_category1"))
                        Else

                        End If
                    End If

                    If Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_category2_use")) Then
                        If masterpage.aTempTable.Rows(0).Item("clipref_category2_use") = "Y" Then
                            category.Items.Add(New ListItem(masterpage.aTempTable.Rows(0).Item("clipref_category2_name"), "clicomp_category2"))
                        Else

                        End If
                    End If

                    If Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_category3_use")) Then
                        If masterpage.aTempTable.Rows(0).Item("clipref_category3_use") = "Y" Then
                            category.Items.Add(New ListItem(masterpage.aTempTable.Rows(0).Item("clipref_category3_name"), "clicomp_category3"))
                        Else

                        End If
                    End If

                    If Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_category4_use")) Then
                        If masterpage.aTempTable.Rows(0).Item("clipref_category4_use") = "Y" Then
                            category.Items.Add(New ListItem(masterpage.aTempTable.Rows(0).Item("clipref_category4_name"), "clicomp_category4"))
                        Else

                        End If
                    End If

                    If Not IsDBNull(masterpage.aTempTable.Rows(0).Item("clipref_category5_use")) Then
                        If masterpage.aTempTable.Rows(0).Item("clipref_category5_use") = "Y" Then
                            category.Items.Add(New ListItem(masterpage.aTempTable.Rows(0).Item("clipref_category5_name"), "clicomp_category5"))
                        Else

                        End If
                    End If
                    'Next
                End If
            Else
                If masterpage.aclsData_Temp.class_error <> "" Then
                    masterpage.error_string = masterpage.aclsData_Temp.class_error
                    masterpage.LogError("listing.aspx.vb - fill_CBO() - " & masterpage.error_string)
                End If
                masterpage.display_error()
            End If
        End Sub

        Public Shared Function Upcoming_ActionItems(ByVal main_calendar_txt As Label, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal DateString As String) As Integer
            Dim masterpage As Object
            Dim pagelink As String = ""
            If Not IsNothing(mob) Then
                pagelink = "mobile_details.aspx"
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                pagelink = "details.aspx"
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try

                'Weekday Calendar Header Setup
                Dim OutputString As String = ""
                Dim today As Date = FormatDateTime(Now(), 2)
                Dim week As Integer = Weekday(today)
                Dim monthint As Integer = Month(today)
                Dim monthdis As String = MonthName(monthint)
                Dim weekdis As String = WeekdayName(week)
                Dim yeardis As Integer = Year(today)
                Dim daydis As Integer = Day(today)
                'today_date.Text = weekdis & ", " & monthdis & " " & daydis & ", " & yeardis
                If String.IsNullOrEmpty(DateString) Then
                    DateString = FormatDateTime(DateAdd(DateInterval.Day, 8, Now()), 2) 'default to a week out
                End If

                ' create a datarow to filter in the rows by make_name
                Try
                    If HttpContext.Current.Session.Item("localUser").crmLocalUserID <> 0 Then
                        masterpage.aTempTable2 = masterpage.aclsData_Temp.Get_Local_Notes_GetByUserIDStatusLessThanDate(HttpContext.Current.Session.Item("localUser").crmLocalUserID, DateString, "P")
                    Else
                        masterpage.aTempTable2 = Nothing
                    End If

                Catch ex As Exception
                    masterpage.error_string = "clsgeneral.vb - Upcoming_ActionItems() - Get_Local_Notes_GetByUserIDStatusLessThanDate - " & ex.Message
                    masterpage.LogError(masterpage.error_string)
                End Try

                'Dim calendar_string As String = ""
                Dim oldweekdis As Integer = 0
                Dim oldmonthint As Integer = 0
                Dim olddaydis As Integer = 0
                Dim CompanyString As String = ""
                Dim ACString As String = ""
                'Dim cal_header As New Label
                'Dim cal_table As New Table
                'Dim caltr As New TableRow
                'Dim caltd As New TableCell
                'Dim caltd2 As New TableCell
                'cal_table.CellPadding = 3
                OutputString = "<table width='100%' cellpadding='3' cellspacing='0'>"

                If Not IsNothing(masterpage.aTempTable2) Then
                    If masterpage.aTempTable2.Rows.Count > 0 Then
                        For Each r As DataRow In masterpage.aTempTable2.Rows
                            ACString = ""
                            CompanyString = ""
                            If r("lnote_status").ToString = "P" Then
                                today = IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), Now())
                                week = Weekday(today)
                                daydis = Day(today)
                                weekdis = WeekdayName(week)
                                monthint = Month(today)
                                monthdis = Left(MonthName(monthint), 3)
                                Dim timeofday = TimeValue(today)
                                If daydis <> olddaydis Or week <> oldweekdis Or monthint <> oldmonthint Then
                                    OutputString += ("<tr><td align='left' valign='top' colspan='3'><div class='cal_header'>" & weekdis & ", " & monthdis & " " & daydis & " " & Year(today) & "<br /></div></td></tr>")
                                End If

                                'Figure out the company
                                Dim ds As New DataTable

                                Try
                                    If r("lnote_jetnet_comp_id") <> 0 Then
                                        ds = masterpage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_jetnet_comp_id"), "JETNET", 0)
                                    Else
                                        ds = masterpage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_client_comp_id"), "CLIENT", 0)
                                    End If
                                Catch ex As Exception
                                    masterpage.error_string = "clsgeneral.vb - Upcoming_ActionItems() - GetLimited_CompanyInfo_ID() - " & ex.Message
                                    masterpage.LogError(masterpage.error_string)
                                End Try

                                OutputString += "<tr>" '<td align='left' valign='top' nowrap>"

                                If ds.Rows.Count > 0 Then

                                    CompanyString = "<a href='" & pagelink & "?comp_ID=" & ds.Rows(0).Item("comp_id") & "&type=1&source=" & ds.Rows(0).Item("source") & "'>(<em style='color:#5b5e65;'>" & ds.Rows(0).Item("comp_name").ToString & ", " & ds.Rows(0).Item("comp_city").ToString & " " & ds.Rows(0).Item("comp_state").ToString & "</em>)</a>"
                                    If HttpContext.Current.Session.Item("isMobile") <> True Then
                                        Dim temporaryText As String = clsGeneral.stripHTML(Replace(Replace(masterpage.createAnAddressPopOut(r("lnote_client_comp_id"), "CLIENT"), "<br />", vbNewLine), "<BR />", vbNewLine))
                                        CompanyString += "<img src='images/magnify.png' alt='" & temporaryText & "' title='" & temporaryText & "' />"
                                    End If
                                    CompanyString += "<br />"
                                End If

                                If r("lnote_client_ac_id") <> 0 Or r("lnote_jetnet_ac_id") <> 0 Then
                                    If r("lnote_jetnet_ac_id") <> 0 Then
                                        Dim TempString As String = ""
                                        TempString += what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2, mob, main)
                                        ACString = "<br />(<em style='color:#5b5e65;'>" & Replace(TempString, "<br />", " - ") & "</em>)"
                                    Else
                                        Dim TempString As String = ""
                                        TempString += what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2, mob, main)
                                        ACString = "<br />(<em style='color:#5b5e65;'>" & Replace(TempString, "<br />", " - ") & "</em>)"
                                    End If
                                End If
                                OutputString += "<td align='left' valign='top' nowrap class='date_cal'>"

                                OutputString += "<a href='#' style='text-decoration:none;' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=action&amp;id=" & r("lnote_id") & "','','scrollbars=yes,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no');""><b>" & Format(timeofday, "hh:mm tt") & " - " & Format(DateAdd(DateInterval.Minute, 30, timeofday), "hh:mm tt") & "</b></a>"

                                OutputString += "</td><td align='left' valign='top' width='15'>&nbsp;</td><td align='left' valign='top'><a href='#' style='text-decoration:none;' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=action&amp;id=" & r("lnote_id") & "','','scrollbars=yes,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no');"">" & HttpContext.Current.Server.HtmlDecode(Left(r("lnote_note").ToString, 150)) & "</a> " & CompanyString
                                OutputString += ACString

                                oldweekdis = week
                                oldmonthint = monthint
                                olddaydis = daydis
                                OutputString += "</td></tr>"
                            End If
                        Next
                        OutputString += "</table>"
                        main_calendar_txt.Text = OutputString
                        Return 1

                    Else
                        OutputString = "<p class='red' align='center'>You have no action items scheduled in the upcoming week.</p>"
                        main_calendar_txt.Text = OutputString
                        Return 0
                    End If
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("clsgeneral.vb - upcoming_action_items() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Page Load() - " & ex.Message & " - " & masterpage.aclsData_Temp.class_error
                masterpage.LogError(masterpage.error_string)
            End Try
        End Function

        Public Shared Sub Market_Categories(ByVal categories As ListBox, ByVal types As ListBox, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal EventType As String)
            Dim aTempTable As New DataTable
            categories.Items.Clear()
            If EventType = "" Or UCase(EventType) = "AIRCRAFT" Then
                categories.Items.Add(New ListItem("All", ""))
                aTempTable = aclsData_Temp.Market_Search_Category()
                If Not IsNothing(aTempTable) Then
                    If Not aTempTable.Rows.Count < 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            categories.Items.Add(New ListItem(r("apecat_category_group"), r("apecat_category_group")))
                        Next
                    End If
                    categories.SelectedValue = ""
                    types.Items.Add(New ListItem("Please Select a Category", ""))
                    types.SelectedValue = ""
                End If
            Else
                categories.Items.Add(New ListItem("Company/Contact", "Company/Contact"))
                categories.SelectedValue = "Company/Contact"
            End If

        End Sub
        Public Shared Sub Market_Type(ByVal categories As ListBox, ByVal types As ListBox, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal typeOfEvent As String)
            Dim aTempTable As New DataTable
            'Dim masterpage As Object
            'If Not IsNothing(mob) Then
            '    masterpage = New crmWebClient.Mobile
            '    masterpage = mob
            'Else
            '    masterpage = New crmWebClient.main_site
            '    masterpage = main
            'End If

            Dim list As String = ""
            For i = 0 To categories.Items.Count - 1
                If categories.Items(i).Selected Then
                    If categories.Items(i).Value <> "" Then
                        list = list & "'" & categories.Items(i).Value & "',"
                    End If
                End If
            Next

            If list <> "" Then
                list = UCase(list.TrimEnd(","))
            End If
            If list <> "" Then
                types.Items.Clear()

                If typeOfEvent = "" Or UCase(typeOfEvent) = "AIRCRAFT" Then
                    types.Items.Add(New ListItem("All", ""))
                End If

                aTempTable = aclsData_Temp.Market_Search_Type(list, typeOfEvent)
                If Not IsNothing(aTempTable) Then
                    If Not aTempTable.Rows.Count < 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            types.Items.Add(New ListItem(r("apecat_category_name"), r("apecat_category_name")))
                        Next
                    End If
                End If
                aTempTable = Nothing

                If typeOfEvent = "" Or UCase(typeOfEvent) = "AIRCRAFT" Then
                    types.SelectedValue = ""
                Else
                    types.SelectedIndex = 0
                End If
            Else
                types.Items.Clear()
                types.Items.Add(New ListItem("All", ""))
            End If
        End Sub
        Public Shared Function what_ac(ByVal jetnet As Integer, ByVal client As Integer, ByVal show As Integer, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            Dim masterpage As Object
            Dim pagelink As String = ""
            If Not IsNothing(mob) Then
                pagelink = "mobile_details.aspx"
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                pagelink = "details.aspx"
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            'This function takes what AC and determines what ac is associated with this ID. 
            what_ac = ""

            Try
                Dim aircraft_text As String = ""
                If jetnet <> 0 Then
                    Dim aError As String = ""
                    masterpage.aTempTable = masterpage.aclsData_Temp.GetJETNET_AC_NAME(jetnet, aError)
                    ' check the state of the DataTable
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In masterpage.aTempTable.Rows
                                If show = 2 Then
                                    aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & "<br />"
                                    If Not IsDBNull(R("ac_ser_nbr")) Then
                                        If R("ac_ser_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "<a href='" & pagelink & "?ac_ID=" & jetnet & "&type=3&source=JETNET'>Ser #: " & R("ac_ser_nbr") & "</a><br />"
                                        End If
                                    End If
                                    If Not IsDBNull(R("ac_year_mfr")) Then
                                        If R("ac_year_mfr") <> "" Then
                                            aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                                        End If
                                    End If
                                    If Not IsDBNull(R("ac_reg_nbr")) Then
                                        If R("ac_reg_nbr") <> "" Then
                                            aircraft_text = aircraft_text & " Reg #: " & R("ac_reg_nbr")
                                        End If
                                    End If
                                End If
                                what_ac = aircraft_text
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneralmain_site.Master.vb - what_ac() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                ElseIf client <> 0 Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Clients_Aircraft_Ser_Model(client)
                    ' check the state of the DataTable
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In masterpage.aTempTable.Rows

                                If show = 2 Then


                                    If Not IsDBNull(R("cliamod_make_name")) And Not IsDBNull(R("cliamod_model_name")) Then
                                        aircraft_text = R("cliamod_make_name") & " " & R("cliamod_model_name") & "<br />"
                                    End If

                                    If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                                        If R("cliaircraft_ser_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "<a href='" & pagelink & ".aspx?ac_ID=" & client & "&type=3&source=CLIENT'>Ser #: " & R("cliaircraft_ser_nbr") & "</a><br />"
                                        End If
                                    End If

                                    If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                                        If R("cliaircraft_year_mfr") <> "" Then
                                            aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & "<br />"
                                        End If
                                    End If
                                    If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                                        If R("cliaircraft_reg_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "<br />"
                                        End If
                                    End If
                                End If
                                what_ac = aircraft_text
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - what_ac() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - what_ac() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

        End Function

        Public Shared Function Fill_Company(ByVal subnode As Boolean, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal status_cbo As String, ByVal subset As String, ByVal country As String, ByVal operator_type As String, ByVal show_all As String, ByVal special_field As String, ByVal special_field_text As String, ByVal special_field_view As Boolean, ByVal special_field_column As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal SubNodeOfListing As Integer, ByVal state As ListBox, ByVal client_IDS As String, ByVal jetnet_IDS As String, ByVal companyCity As String, ByVal mergeLists As Boolean) As DataTable
            'x is the search string.
            'Subnode is whether or not we're going to display the information for a subfolder. Example:
            'Under Company - there is a folder called Hot Leads. If subnode is true - hot leads information will display.
            'If it's false, the regular search will take place. 
            'Y is a determining factor. More often than not it will be 1. That means
            'When things are searched - they'll be a % sign on either side. Example: where comp_name like "%test%"
            'When y is 2 - they'll only be a % sign at the end. Example: where comp_name like "A%". This is for company
            'Letter/number buttons.

            'Event that's handled on the Master Page.
            Dim states As String = ""
            For i = 0 To state.Items.Count - 1
                If state.Items(i).Selected Then
                    If state.Items(i).Value <> "" Then
                        states = states & "'" & state.Items(i).Value & "',"
                    End If
                End If
            Next
            If status_cbo = "N" Then
                subset = "C"
            End If

            If states <> "" Then
                states = UCase(states.TrimEnd(","))
            End If


            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If


            Dim arComp_ids_JETNET As String = ""
            Dim arComp_ids_CLIENT As String = ""
            If subnode = True Then
                masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Folder_Index(SubNodeOfListing)
                If Not IsNothing(masterpage.aTempTable) Then
                    If masterpage.aTempTable.Rows.Count > 0 Then
                        ' build an string of comp_ids
                        For count As Integer = 0 To masterpage.aTempTable.Rows.Count - 1
                            arComp_ids_JETNET = arComp_ids_JETNET & masterpage.aTempTable.Rows(count).Item("cfoldind_jetnet_comp_id")
                            arComp_ids_CLIENT = arComp_ids_CLIENT & masterpage.aTempTable.Rows(count).Item("cfoldind_client_comp_id")
                            If count <> masterpage.aTempTable.Rows.Count - 1 Then
                                arComp_ids_JETNET = arComp_ids_JETNET & ","
                                arComp_ids_CLIENT = arComp_ids_CLIENT & ","
                            End If
                        Next
                        masterpage.aTempTable.Dispose()
                    End If
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("main_site.Master.vb - Fill_Company() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If
            End If

            If jetnet_IDS <> "" Then
                arComp_ids_JETNET = jetnet_IDS
                subnode = True
            End If
            If client_IDS <> "" Then
                arComp_ids_CLIENT = client_IDS
                subnode = True
            End If

            If search_for = "" Then
                If show_all <> "" Then
                    If show_all = True And subset = "C" Then
                        search_for = "%"
                    End If
                End If
            End If
            Dim atemptable As New DataTable
            Try
                If subnode <> True Then
                    If search_for <> "" Or (special_field_text <> "" And special_field <> "") Or (country <> "") Or (states <> "") Or companyCity <> "" Then
                        If search_where = 1 Then 'This means that parentheses is on both sides, search feature. 
                            atemptable = masterpage.aclsData_Temp.Company_Search("%" & search_for & "%", status_cbo, subset, country, states, operator_type, HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, special_field, special_field_text, "", "", "", companyCity, "")

                        ElseIf search_where = 2 Then
                            atemptable = masterpage.aclsData_Temp.Company_Search("" & search_for & "%", status_cbo, subset, country, states, operator_type, HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, special_field, special_field_text, "", "", "", companyCity, "")

                        End If
                    End If
                Else
                    If arComp_ids_JETNET = "" And arComp_ids_CLIENT = "" Then
                        atemptable = Nothing
                    Else
                        If arComp_ids_CLIENT = "" Then
                            subset = "J"
                        ElseIf arComp_ids_JETNET = "" Then
                            subset = "C"
                        End If
                        atemptable = masterpage.aclsData_Temp.Company_Search("" & search_for & "%", status_cbo, subset, country, states, operator_type, HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, special_field, special_field_text, arComp_ids_CLIENT, arComp_ids_JETNET, "", companyCity, "")
                    End If
                End If

                Return atemptable
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Fill_Company() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return atemptable
        End Function

        Public Shared Function parseUserAgentString(ByVal inUserAgentString As String)
            Dim osString As String = ""
            Dim browserString As String = ""

            osString = "other  "
            browserString = "unknown"
            If Not String.IsNullOrEmpty(inUserAgentString) Then
                inUserAgentString = inUserAgentString.ToLower
                If inUserAgentString.Contains("windows") Then
                    osString = "win    "
                ElseIf inUserAgentString.Contains("ipad") Then
                    osString = "ipad   "
                ElseIf inUserAgentString.Contains("iphone") Then
                    osString = "iphone "
                ElseIf inUserAgentString.Contains("mac") Then
                    osString = "mac    "
                ElseIf inUserAgentString.Contains("android") Then
                    osString = "droid  "
                ElseIf inUserAgentString.Contains("blackberry") Then
                    osString = "blackb "
                ElseIf inUserAgentString.Contains("red hat") Then
                    osString = "linux  "
                ElseIf inUserAgentString.Contains("linux") Then
                    osString = "linux  "
                End If
                If Trim(osString) = "" Then
                    If inUserAgentString.Contains("mobile") Then
                        osString = "mobile "
                    ElseIf inUserAgentString.Contains("tablet") Then
                        osString = "tablet "
                    Else
                        osString = "other  "
                    End If
                End If
                If inUserAgentString.Contains("chrome") Then
                    browserString = "chrome"
                ElseIf inUserAgentString.Contains("opera") Then
                    browserString = "opera"
                ElseIf inUserAgentString.Contains("safari") Then
                    browserString = "safari"
                ElseIf inUserAgentString.Contains("mobile safari") Then
                    browserString = "safarim"
                ElseIf inUserAgentString.Contains("firefox") Then
                    browserString = "firefox"
                ElseIf inUserAgentString.Contains("fennec") Then
                    browserString = "firefoxm"
                ElseIf inUserAgentString.Contains("applewebkit") Then
                    browserString = "mozilla"
                ElseIf inUserAgentString.Contains("msie") Then
                    browserString = "msie"
                Else
                    browserString = "unknown"
                End If
            Else
                inUserAgentString = "Blank User Agent String"
            End If
            Return osString + browserString
        End Function


        ''' <summary>
        ''' Fill Contact for the Mobile/Regular Version
        ''' </summary>
        ''' <param name="mob">Mobile Masterpage</param>
        ''' <param name="main">Main site masterpage</param>
        ''' <param name="contact_first_name">First Name</param>
        ''' <param name="contact_last_name">Last Name</param>
        ''' <param name="comp_name">Company Name</param>
        ''' <param name="contact_status">Contact Status</param>
        ''' <param name="search_where">Begins With/Anywhere</param>
        ''' <param name="contact_ordered_by">Order By</param>
        ''' <param name="subset">Data subset</param>
        ''' <param name="subnode">Subnode Folder</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Fill_Contact(ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal contact_first_name As String, ByVal contact_last_name As String, ByVal comp_name As String, ByVal contact_status As String, ByVal search_where As String, ByVal contact_ordered_by As String, ByVal subset As String, ByVal subnode As Integer, ByVal email_address As String, ByVal phone As String) As DataTable
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try
                If contact_status = "N" Then
                    subset = "C"
                End If
                Dim arComp_ids_JETNET As String = ""
                Dim arComp_ids_CLIENT As String = ""

                If subnode <> 0 Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Folder_Index(masterpage.SubNodeOfListing)
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For count As Integer = 0 To masterpage.aTempTable.Rows.Count - 1
                                arComp_ids_JETNET = arComp_ids_JETNET & "" & masterpage.aTempTable.Rows(count).Item("cfoldind_jetnet_contact_id") & ""
                                arComp_ids_CLIENT = arComp_ids_CLIENT & "" & masterpage.aTempTable.Rows(count).Item("cfoldind_client_contact_id") & ""
                                If count <> masterpage.aTempTable.Rows.Count - 1 Then
                                    arComp_ids_JETNET = arComp_ids_JETNET & ","
                                    arComp_ids_CLIENT = arComp_ids_CLIENT & ","
                                End If
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.aspx.vb - Fill_Contact() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                End If

                ''''''Search Phone #
                If phone <> "" Then
                    Dim PhoneTable As New DataTable
                    phone = Replace(phone, ".", "%")
                    phone = Replace(phone, "(", "%")
                    phone = Replace(phone, ")", "%")
                    phone = "%" & phone & "%"
                    Try
                        PhoneTable = masterpage.aclsData_Temp.SearchPhoneNumbers(phone)
                        If Not IsNothing(PhoneTable) Then
                            If PhoneTable.Rows.Count > 0 Then
                                For Each r As DataRow In PhoneTable.Rows
                                    If r("pnum_contact_id") > 0 Then
                                        If r("source") = "CLIENT" Then
                                            arComp_ids_CLIENT += r("pnum_contact_id") & ","
                                        Else
                                            arComp_ids_JETNET += r("pnum_contact_id") & ","
                                        End If
                                    End If
                                Next
                            Else
                            End If
                        Else
                            If masterpage.aclsData_Temp.class_error <> "" Then
                                masterpage.error_string = masterpage.aclsData_Temp.class_error
                                masterpage.LogError("clsGeneral Fill_Contact - " & masterpage.error_string)
                            End If
                            masterpage.display_error()
                        End If


                        subnode = True

                        If arComp_ids_JETNET <> "" Then
                            arComp_ids_JETNET = UCase(arComp_ids_JETNET.TrimEnd(","))
                        Else
                            arComp_ids_JETNET = "0"
                        End If
                        If arComp_ids_CLIENT <> "" Then
                            arComp_ids_CLIENT = UCase(arComp_ids_CLIENT.TrimEnd(","))
                        Else
                            arComp_ids_CLIENT = "0"
                        End If
                    Catch ex As Exception
                        masterpage.error_string = "clsGeneral Fill_Contact - " & ex.Message
                        masterpage.LogError(masterpage.error_string)
                    End Try
                End If



                '''''''End search Phone #
                Select Case contact_ordered_by
                    Case "1"
                        contact_ordered_by = "contact_first_name asc, contact_last_name asc"
                    Case "2"
                        contact_ordered_by = "comp_name asc"
                    Case Else
                        contact_ordered_by = "contact_last_name asc, contact_first_name asc"
                End Select

                If contact_ordered_by = "" Then
                    contact_ordered_by = "contact_first_name ASC"
                End If


                If (subnode <> 0 And (arComp_ids_CLIENT <> "" Or arComp_ids_JETNET <> "")) Or (subnode = 0 And (contact_first_name <> "" Or contact_last_name <> "" Or comp_name <> "" Or email_address <> "")) Then
                    If search_where = "2" Then
                        masterpage.aTempTable = masterpage.aclsData_Temp.Search_Contacts("" & contact_first_name & "%", "" & contact_last_name & "%", comp_name & "%", contact_status, "2", contact_ordered_by, subset, arComp_ids_JETNET, arComp_ids_CLIENT, email_address & "%")
                    Else
                        masterpage.aTempTable = masterpage.aclsData_Temp.Search_Contacts("%" & contact_first_name & "%", "%" & contact_last_name & "%", "%" & comp_name & "%", contact_status, "2", contact_ordered_by, subset, arComp_ids_JETNET, arComp_ids_CLIENT, "%" & email_address & "%")
                    End If
                End If

                If IsNothing(masterpage.atemptable) Then
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("clsgeneral.aspx.vb - Fill_Contact() - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Fill_Contact() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return masterpage.atemptable
        End Function
        Public Shared Function Fill_Aircraft(ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal ac_sort As String, ByVal ac_subset As String, ByVal ac_types_of_owners As String, ByVal aircraft_search_for As String, ByVal market_status_cbo As String, ByVal airport_name As String, ByVal icao_code As String, ByVal iata_code As String, ByVal city As String, ByVal country As String, ByVal on_exclusive As String, ByVal on_lease As String, ByVal year_start As String, ByVal year_end As String, ByVal aircraft_search_where As String, ByVal model_cbo As ListBox, ByVal subnode As Boolean, ByVal states As String, ByVal search_field As String, ByVal lifecycle As String, ByVal ownership As String, ByVal CustomField1 As String, ByVal CustomField2 As String, ByVal CustomField3 As String, ByVal CustomField4 As String, ByVal CustomField5 As String, ByVal CustomField6 As String, ByVal CustomField7 As String, ByVal CustomField8 As String, ByVal CustomField9 As String, ByVal CustomField10 As String, ByVal AircraftNotesSearch As Integer, ByVal AircraftNoteDate As String, ByVal MergeLists As Boolean) As DataTable
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            'Figuring out models:
            Dim models As String = ""
            For i = 0 To model_cbo.Items.Count - 1
                If model_cbo.SelectedValue = "All" Then
                    If model_cbo.Items(i).Value <> "All" Then
                        models = models & "'" & model_cbo.Items(i).Value & "',"
                    End If
                Else
                    If model_cbo.Items(i).Selected Then
                        If model_cbo.Items(i).Value <> "" Then
                            models = models & "'" & model_cbo.Items(i).Value & "',"
                        End If
                    End If
                End If
            Next

            If models <> "" Then
                models = UCase(models.TrimEnd(","))
            End If
            HttpContext.Current.Session.Item("models_export") = models

            Dim jetnet_model_id As String = ""
            Dim client_model_id As String = ""
            If models <> "" Then
                models = Replace(models, "'", "")
                Dim model_sets As Array = Split(models, ",")
                For x = 0 To UBound(model_sets)
                    Dim model_info As Array = Split(model_sets(x), "|")

                    If x = 0 Then
                        jetnet_model_id = "'"
                        client_model_id = "'"
                    End If

                    jetnet_model_id = jetnet_model_id & model_info(0)
                    client_model_id = client_model_id & model_info(4)


                    If x <> UBound(model_sets) Then
                        jetnet_model_id = jetnet_model_id & "','"
                        client_model_id = client_model_id & "','"
                    Else
                        jetnet_model_id = jetnet_model_id & "'"
                        client_model_id = client_model_id & "'"
                    End If


                Next
            End If


            Dim arComp_ids_Jetnet As String = ""
            Dim arComp_ids_CLIENT As String = ""

            If HttpContext.Current.Session.Item("localUser").crmEvo = False Then
                'This is if you're looking at a subfolder
                If subnode = True Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Folder_Index(masterpage.SubNodeOfListing)
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            ' build an string of comp_ids
                            For count As Integer = 0 To masterpage.aTempTable.Rows.Count - 1
                                arComp_ids_Jetnet = arComp_ids_Jetnet & masterpage.aTempTable.Rows(count).Item("cfoldind_jetnet_ac_id")
                                arComp_ids_CLIENT = arComp_ids_CLIENT & masterpage.aTempTable.Rows(count).Item("cfoldind_client_ac_id")
                                If count <> masterpage.aTempTable.Rows.Count - 1 Then
                                    arComp_ids_Jetnet = arComp_ids_Jetnet & ","
                                    arComp_ids_CLIENT = arComp_ids_CLIENT & ","
                                End If
                            Next
                        Else
                            arComp_ids_Jetnet = "0"
                            arComp_ids_CLIENT = "0"
                        End If
                    End If
                ElseIf subnode = False Then 'This means you aren't looking at a subfolder. Next thing we need to check for is.. what's your
                    'AircraftNotesSearch variable? If it is 0, you can bypass this and ignore it. However if it is 1 or 2, we need to fill up those IDs anyhow.
                    If AircraftNotesSearch = 1 Or AircraftNotesSearch = 2 Then
                        Dim NoteJetnetModels As String = ""
                        Dim NoteClientModels As String = ""
                        SetUpSpecialModels(models, NoteClientModels, NoteJetnetModels)
                        arComp_ids_CLIENT = BuildClientACString(NoteClientModels, NoteJetnetModels, AircraftNoteDate)

                        arComp_ids_Jetnet = BuildJetnetCompIds(masterpage.aclsData_Temp, AircraftNoteDate, NoteClientModels, NoteJetnetModels, ac_subset, AircraftNotesSearch)

                    End If
                End If
            End If



            If aircraft_search_where = "1" Then
                aircraft_search_for = "" & aircraft_search_for & "%"
            ElseIf aircraft_search_where = "2" Then
                aircraft_search_for = "%" & aircraft_search_for & "%"
            End If

            If ac_sort = "COMP_NAME ASC" Or ac_sort = "COMP_NAME DESC" Then
                masterpage.AircraftSort_Company = True
                masterpage.aTempTable = masterpage.aclsData_Temp.AC_Search_new(ac_sort, ac_subset, ac_types_of_owners, arComp_ids_Jetnet, arComp_ids_CLIENT, "" & aircraft_search_for & "", market_status_cbo, airport_name, icao_code, iata_code, city, country, states, client_model_id, jetnet_model_id, HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, on_exclusive, on_lease, year_start, year_end, search_field, aircraft_search_where, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, MergeLists)
            Else
                masterpage.AircraftSort_Company = False
                masterpage.aTempTable = masterpage.aclsData_Temp.AC_Search(ac_sort, ac_subset, ac_types_of_owners, arComp_ids_Jetnet, arComp_ids_CLIENT, "" & aircraft_search_for & "", market_status_cbo, airport_name, icao_code, iata_code, city, country, states, client_model_id, jetnet_model_id, HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, on_exclusive, on_lease, year_start, year_end, search_field, aircraft_search_where, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, MergeLists)
            End If
            Return masterpage.atemptable
        End Function
        Public Shared Sub SetUpSpecialModels(ByVal NoteModels As String, ByRef NoteClientModels As String, ByRef NoteJetnetModels As String)
            If NoteModels <> "" Then
                NoteModels = Replace(NoteModels, "'", "")
                Dim model_sets As Array = Split(NoteModels, ",")
                For x = 0 To UBound(model_sets)
                    Dim model_info As Array = Split(model_sets(x), "|")

                    'Setting up the CLIENT Model.
                    If model_info(4) <> "0" Then
                        If NoteClientModels <> "" Then
                            NoteClientModels += ","
                        End If
                        NoteClientModels += "'" & model_info(4) & "'"
                    End If


                    'Setting up the JETNET Model.
                    If model_info(0) <> "0" Then
                        If NoteJetnetModels <> "" Then
                            NoteJetnetModels += ","
                        End If

                        NoteJetnetModels += "'" & model_info(0) & "'"
                    End If
                Next
            End If

        End Sub
        Public Shared Function BuildClientACString(ByVal client_model_id As String, ByVal jetnet_model_id As String, ByRef AircraftNoteDate As String) As String
            Dim arComp_ids_CLIENT As String = ""

            arComp_ids_CLIENT = "select distinct lnote_client_ac_id from local_notes where lnote_client_ac_id > 0 and lnote_status = 'A' "
            arComp_ids_CLIENT += " and ((lcase(lnote_note) not like ""no answer%"") and (lcase(lnote_note) not like ""message left%"") and (lcase(lnote_note) not like ""left voicemail%""))"

            If HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly Then
                arComp_ids_CLIENT += " and ( lnote_user_id = " & HttpContext.Current.Session.Item("localUser").crmLocalUserID.ToString & ") "
            End If

            If Not String.IsNullOrEmpty(Trim(client_model_id)) Or Not String.IsNullOrEmpty(Trim(jetnet_model_id)) Then
                arComp_ids_CLIENT += " and ("

                If Not String.IsNullOrEmpty(Trim(client_model_id)) Then
                    arComp_ids_CLIENT += "lnote_client_amod_id in (" & Trim(client_model_id) & ") "
                    If Not String.IsNullOrEmpty(Trim(jetnet_model_id)) Then
                        arComp_ids_CLIENT += " or "
                    End If
                End If

                If Not String.IsNullOrEmpty(jetnet_model_id) Then
                    arComp_ids_CLIENT += " lnote_jetnet_amod_id in (" & Trim(jetnet_model_id) & ")"
                End If

                arComp_ids_CLIENT += ")"
            End If


            If AircraftNoteDate <> "" Then
                AircraftNoteDate = Year(AircraftNoteDate) & "-" & Month(AircraftNoteDate) & "-" & Day(AircraftNoteDate)
                arComp_ids_CLIENT += " and (lnote_entry_date >= """ & AircraftNoteDate & """)"
            End If


            Return arComp_ids_CLIENT
        End Function

        Public Shared Function BuildJetnetCompIds(ByRef aclsData_Temp As clsData_Manager_SQL, ByVal AircraftNoteDate As String, ByVal client_model_id As String, ByVal jetnet_model_id As String, ByRef ac_subset As String, ByVal AircraftNotesSearch As Integer) As String
            Dim arComp_ids_Jetnet As String = ""
            Dim TemporaryTable As New DataTable

            TemporaryTable = aclsData_Temp.SelectDistinctJetnetAircraftIDFromNotes(AircraftNoteDate, client_model_id, jetnet_model_id)
            If Not IsNothing(TemporaryTable) Then
                If TemporaryTable.Rows.Count > 0 Then
                    For Each r As DataRow In TemporaryTable.Rows
                        If arComp_ids_Jetnet <> "" Then
                            arComp_ids_Jetnet += ","
                        End If
                        arComp_ids_Jetnet += r("lnote_jetnet_ac_id").ToString
                    Next
                End If
            End If


            If AircraftNotesSearch = 1 Then
                If arComp_ids_Jetnet = "" Then
                    ac_subset = "C"
                End If
            End If


            Return arComp_ids_Jetnet
        End Function

        Public Shared Sub Transaction_Contact_Type(ByVal relationships As ListBox, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try
                masterpage.aTempTable = masterpage.aclsData_temp.Get_CRM_Client_Aircraft_Contact_Type

                For Each r As DataRow In masterpage.aTempTable.Rows
                    relationships.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
                Next
                relationships.SelectionMode = ListSelectionMode.Multiple
                For i As Integer = 0 To relationships.Items.Count - 1
                    If relationships.Items(i).Value = "95" Or relationships.Items(i).Value = "96" Or relationships.Items(i).Value = "62" Then
                        relationships.Items(i).Selected = True
                    End If
                Next i

            Catch ex As Exception
                masterpage.error_string = "transactionSearch - fill_CBO() Get_Client_Aircraft_Contact_Type Filling - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub
        Public Shared Sub Transaction_Category(ByVal trans_type_cbo As DropDownList, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            masterpage.aTempTable = masterpage.aclsData_temp.Get_Client_Transactions_Category

            If Not IsNothing(masterpage.aTempTable) Then
                If masterpage.aTempTable.Rows.Count > 0 Then
                    ' Dim distinctTable As DataTable = masterpage.aTempTable.DefaultView.ToTable(True, "clitcat_type")
                    For Each q As DataRow In masterpage.aTempTable.Rows
                        If Not IsDBNull(q("clitcat_type")) Then
                            trans_type_cbo.Items.Add(New ListItem(q("clitcat_type"), q("clitcat_code")))
                        End If
                    Next
                End If
            Else
                If masterpage.aclsData_Temp.class_error <> "" Then
                    masterpage.error_string = masterpage.aclsData_Temp.class_error
                    masterpage.LogError("transactionSearch - fill_CBO() - " & masterpage.error_string)
                End If
                masterpage.display_error()
            End If
        End Sub
        Public Shared Function Fill_Transactions(ByVal start_date As String, ByVal end_date As String, ByVal transaction_model As ListBox, ByVal search As String, ByVal search_where As Integer, ByVal internal As String, ByVal awaiting As String, ByVal trans_type As String, ByVal subset As String, ByVal year_start As String, ByVal year_end As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)

            Dim awaiting_value As String = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Dim models As String = ""
            For i = 0 To transaction_model.Items.Count - 1
                If transaction_model.SelectedValue = "All" Then
                    If transaction_model.Items(i).Value <> "All" Then
                        models = models & "'" & transaction_model.Items(i).Value & "',"
                    End If
                Else
                    If transaction_model.Items(i).Selected Then
                        If transaction_model.Items(i).Value <> "" Then
                            models = models & "'" & transaction_model.Items(i).Value & "',"
                        End If
                    End If
                End If
            Next

            If models <> "" Then
                models = UCase(models.TrimEnd(","))
            End If


            HttpContext.Current.Session.Item("models_export") = models

            If start_date <> "" Then
                start_date = Year(start_date) & "-" & Month(start_date) & "-" & Day(start_date)
            End If
            If end_date <> "" Then
                end_date = Year(end_date) & "-" & Month(end_date) & "-" & Day(end_date)
            Else
                end_date = Year(Now()) & "-" & Month(Now()) & "-" & Day(Now())
            End If
            Try
                Dim jetnet_model_id As String = ""
                Dim client_model_id As String = ""
                If models <> "" Then
                    models = Replace(models, "'", "")
                    Dim model_sets As Array = Split(models, ",")
                    For x = 0 To UBound(model_sets)
                        Dim model_info As Array = Split(model_sets(x), "|")
                        If x = 0 Then
                            jetnet_model_id = "'"
                            client_model_id = "'"
                        End If
                        jetnet_model_id = jetnet_model_id & model_info(0)
                        client_model_id = client_model_id & model_info(4)
                        If x <> UBound(model_sets) Then
                            jetnet_model_id = jetnet_model_id & "','"
                            client_model_id = client_model_id & "','"
                        Else
                            jetnet_model_id = jetnet_model_id & "'"
                            client_model_id = client_model_id & "'"
                        End If
                    Next
                End If


                If search_where <> 2 Then
                    search = "%" & search & "%"
                Else
                    search = "" & search & "%"
                End If

                If awaiting = True Then
                    awaiting_value = "Y"
                Else
                    awaiting_value = "N"
                End If

                masterpage.aTempTable = masterpage.aclsData_Temp.Transaction_Search(start_date, end_date, trans_type, client_model_id, jetnet_model_id, search, subset, year_start, year_end, internal, awaiting_value)
                Return masterpage.atemptable
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - Fill_transation() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return masterpage.atemptable
        End Function
        Public Shared Function Fill_Notes_Actions_Documents(ByVal notes_start_date As String, ByVal notes_end_date As String, ByVal notes_search As String, ByVal search_where As String, ByVal model_cbo As ListBox, ByVal category As String, ByVal user As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal opp_status As String, ByVal clientIds As String, ByVal jetnetIds As String, ByVal acSearchField As Integer, ByVal acSearchOperator As Integer, ByVal acSearchText As String, ByVal OnlyModel As Boolean, ByVal OnlyAircraft As Boolean, ByVal FolderType As Long) As DataTable
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If


            Dim models As String = ""
            For i = 0 To model_cbo.Items.Count - 1
                If model_cbo.SelectedValue = "All" Then
                    If model_cbo.Items(i).Value <> "All" Then
                        models = models & "" & model_cbo.Items(i).Value & ","
                    End If
                Else
                    If model_cbo.Items(i).Selected Then
                        If model_cbo.Items(i).Value <> "" Then
                            models = models & "" & model_cbo.Items(i).Value & ","
                        End If
                    End If
                End If
            Next

            If models <> "" Then
                models = UCase(models.TrimEnd(","))
            End If

            HttpContext.Current.Session.Item("models_export") = models

            If notes_start_date <> "" Then
                notes_start_date = Year(notes_start_date) & "-" & Month(notes_start_date) & "-" & Day(notes_start_date)
            End If

            If notes_end_date <> "" Then
                notes_end_date = Year(notes_end_date) & "-" & Month(notes_end_date) & "-" & Day(notes_end_date)
            Else
                'If the type of listing isn't an opportunity, default to end date of now if it's blank.
                If masterpage.typeoflisting <> 11 Then
                    notes_end_date = Year(Now()) & "-" & Month(Now()) & "-" & Day(Now())
                End If
            End If
            Dim jetnet_model_id As String = ""
            Dim client_model_id As String = ""
            If models <> "" Then
                models = Replace(models, "'", "")
                Dim model_sets As Array = Split(models, ",")
                For x = 0 To UBound(model_sets)
                    Dim model_info As Array = Split(model_sets(x), "|")
                    If x = 0 Then
                        jetnet_model_id = "'"
                        client_model_id = "'"
                    End If
                    jetnet_model_id = jetnet_model_id & model_info(0)
                    client_model_id = client_model_id & model_info(4)
                    If x <> UBound(model_sets) Then
                        jetnet_model_id = jetnet_model_id & "','"
                        client_model_id = client_model_id & "','"
                    Else
                        jetnet_model_id = jetnet_model_id & "'"
                        client_model_id = client_model_id & "'"
                    End If
                Next
            End If

            Dim search As String = "%" & notes_search & "%" 'Default to this type of search
            If search_where = 2 Then 'This means that parentheses is on both sides, search feature. 
                search = "%" & notes_search & "%"
            ElseIf search_where = 1 Then
                search = notes_search & "%"
            End If

            Dim status As String = "A"

            Select Case masterpage.typeoflisting
                Case 4
                    status = "P"
                Case 7
                    status = "F"
                Case 6
                    status = "A"
                Case 11
                    status = "O"
                Case 16
                    status = "B"
            End Select

            masterpage.aTempTable = masterpage.aclsData_Temp.Notes_Search(search, notes_start_date, notes_end_date, status, category, jetnet_model_id, client_model_id, user, opp_status, clientIds, jetnetIds, acSearchField, acSearchOperator, acSearchText, OnlyModel, OnlyAircraft, FolderType)


            Return masterpage.atemptable

        End Function
        Public Shared Function Fill_Market(ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal market_model As ListBox, ByVal categories As ListBox, ByVal market_types As ListBox, ByVal market_time As Integer, ByVal start_date As String, ByVal end_date As String) As DataTable
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try


                Dim models_cbo As String = ""
                For i = 0 To market_model.Items.Count - 1
                    If market_model.SelectedValue = "All" Then
                        If market_model.Items(i).Value <> "All" Then
                            models_cbo = models_cbo & "" & market_model.Items(i).Value & ","
                        End If
                    Else
                        If market_model.Items(i).Selected Then
                            If market_model.Items(i).Value <> "" Then
                                models_cbo = models_cbo & "" & market_model.Items(i).Value & ","
                            End If
                        End If
                    End If
                Next

                If models_cbo <> "" Then
                    models_cbo = UCase(models_cbo.TrimEnd(","))
                End If

                Dim cat As String = ""
                For i = 0 To categories.Items.Count - 1
                    If categories.Items(i).Selected Then
                        cat = cat & "'" & categories.Items(i).Value & "',"
                    End If
                Next

                If cat <> "" Then
                    cat = cat.TrimEnd(",")
                End If

                Dim market_type As String = ""
                For i = 0 To market_types.Items.Count - 1
                    If market_types.Items(i).Selected Then
                        market_type = market_type & "'" & market_types.Items(i).Value & "',"
                    End If
                Next

                Dim start_date_field As String = ""
                Dim end_date_field As String = ""
                'let's figure out if we're using market time or all the date differences.
                Dim difference_in_days As Integer = 10

                If start_date <> "" And end_date <> "" Then
                    end_date_field = CStr(Format(CDate(end_date), "MM/dd/yyyy hh:mm:ss tt"))
                    start_date_field = CStr(Format(CDate(start_date), "MM/dd/yyyy hh:mm:ss tt"))
                ElseIf market_time <> 0 Then

                    If market_time > 0 And market_time < 365 Then
                        market_time = market_time * -1
                        start_date_field = DateAdd(DateInterval.Day, market_time, Now())
                        start_date_field = CStr(Format(CDate(start_date_field), "MM/dd/yyyy hh:mm:ss tt"))
                    Else
                        start_date_field = DateAdd(DateInterval.Month, -12, Now())
                        start_date_field = CStr(Format(CDate(start_date_field), "MM/dd/yyyy hh:mm:ss tt"))
                    End If


                    '  start_date_field = Month(start_date_field) & "/" & Day(start_date_field) & "/" & Year(start_date_field) & " " & "1:59:29 PM"
                End If


                If market_type <> "" Then
                    market_type = market_type.TrimEnd(",")
                End If
                Dim jetnet_model_id_hold As String = ""
                Dim jetnet_model_id As Integer = 0
                If models_cbo <> "" Then
                    Dim arrayed As Array = Split(models_cbo, ",")

                    For counted = 0 To UBound(arrayed)
                        Dim model_info As Array = Split(arrayed(counted), "|")
                        jetnet_model_id_hold = jetnet_model_id_hold & "'" & model_info(0) & "',"
                    Next

                    If jetnet_model_id_hold <> "" Then
                        jetnet_model_id_hold = UCase(jetnet_model_id_hold.TrimEnd(","))
                    End If

                Else
                    jetnet_model_id_hold = ""
                End If


                masterpage.aTempTable = masterpage.aclsData_Temp.Market_Search(jetnet_model_id_hold, start_date_field, end_date_field, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, 0, cat, market_type)
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - Fill_Market() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return masterpage.atemptable
        End Function

        Public Shared Function Fill_Wanteds(ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, ByVal market_model As ListBox, ByVal start_date_field As String, ByVal end_date_field As String, ByVal interested_party As String, ByVal subset As String) As DataTable
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try

                Dim models_cbo As String = ""
                For i = 0 To market_model.Items.Count - 1
                    If market_model.SelectedValue = "All" Then
                        If market_model.Items(i).Value <> "All" Then
                            models_cbo = models_cbo & "" & market_model.Items(i).Value & ","
                        End If
                    Else
                        If market_model.Items(i).Selected Then
                            If market_model.Items(i).Value <> "" Then
                                models_cbo = models_cbo & "" & market_model.Items(i).Value & ","
                            End If
                        End If
                    End If
                Next

                If models_cbo <> "" Then
                    models_cbo = UCase(models_cbo.TrimEnd(","))
                End If

                If IsDate(start_date_field) Then
                    start_date_field = Year(start_date_field) & "-" & Month(start_date_field) & "-" & Day(start_date_field)
                End If
                If IsDate(end_date_field) Then
                    end_date_field = Year(end_date_field) & "-" & Month(end_date_field) & "-" & Day(end_date_field)
                End If

                Dim jetnet_model_id_hold As String = ""
                Dim jetnet_model_id As Integer = 0
                If models_cbo <> "" Then
                    Dim arrayed As Array = Split(models_cbo, ",")

                    For counted = 0 To UBound(arrayed)
                        Dim model_info As Array = Split(arrayed(counted), "|")
                        jetnet_model_id_hold = jetnet_model_id_hold & "'" & model_info(0) & "',"
                    Next

                    If jetnet_model_id_hold <> "" Then
                        jetnet_model_id_hold = UCase(jetnet_model_id_hold.TrimEnd(","))
                    End If

                Else
                    jetnet_model_id_hold = ""
                End If


                masterpage.aTempTable = masterpage.aclsData_Temp.Return_Wanted(0, "", 0, jetnet_model_id_hold, start_date_field, end_date_field, interested_party, subset, 0)
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - Fill_Market() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
            Return masterpage.atemptable
        End Function

        Public Shared Function what_opportunity_cat(ByVal x As Integer, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            what_opportunity_cat = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try
                If Not IsDBNull(x) Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Opportunity_Categories_ID(x)
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In masterpage.aTempTable.Rows
                                'notecat_key, notecat_name
                                If x = R("oppcat_key") Then
                                    what_opportunity_cat = R("oppcat")
                                End If
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("main_site.Master.vb - what_opportunity_cat() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                        what_opportunity_cat = x
                    End If
                End If
            Catch ex As Exception
                masterpage.error_string = "main_site.Master.vb - what_opportunity_cat() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

        End Function

        Public Shared Sub LogUser(ByVal mob As Object, ByVal status As String)
            Dim masterpage As New Object

            masterpage = mob

            Try
                masterpage.error_string = "clsgeneral.Master.vb - LogUser() - "
                If HttpContext.Current.Session.Item("localUser").crmEvo = True Then
                    Dim returned As Integer = masterpage.aclsData_Temp.Update_Evo_Sub_Dates("logout", Now(), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)
                    If returned = 0 Then
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.Master.vb - LogUser() - LOGIN WAS NOT UPDATED, THIS IS WHY " & masterpage.error_string)
                        End If
                    End If
                Else
                    Dim action As String = "login"
                    If status = "N" Then
                        action = "logout"
                    End If

                    Dim returned As Integer = masterpage.aclsData_Temp.CRM_Central_Update_Client_User_Dates(HttpContext.Current.Session.Item("localUser").crmLocalUserID, CLng(HttpContext.Current.Session.Item("masterRecordID")), status, action, Now())

                    If returned = 0 Then
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.Master.vb - LogUser() - LOGIN WAS NOT UPDATED, THIS IS WHY " & masterpage.error_string)
                        End If
                    End If

                End If

            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - LogUser() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub
        Public Shared Function FindCRMUser_Homebase(ByVal crmTestMaster As main_site, ByVal user_id As String) As String
            FindCRMUser_Homebase = ""
            Try

                Dim atempTable As DataTable

                atempTable = crmTestMaster.aclsData_Temp.Get_User_List_Homebase(Trim(user_id))
                If Trim(user_id) <> "" Then
                    If Not IsNothing(atempTable) Then
                        If atempTable.Rows.Count > 0 Then
                            For Each q As DataRow In atempTable.Rows
                                FindCRMUser_Homebase = q("user_first_name") & " " & q("user_last_name")
                            Next
                        End If
                    End If
                End If

            Catch ex As Exception

            End Try

        End Function

        Public Shared Sub FillCRMUser_Homebase(ByVal crmTestMaster As main_site, ByVal type_string As String, ByVal display_cbo As DropDownList, ByVal include_inactives As Boolean, ByVal user_id As String)
            Dim ExistsTable As New DataTable
            Dim selected As Long = 0
            Dim atempTable As DataTable
            Try



                atempTable = crmTestMaster.aclsData_Temp.Get_User_List_Homebase("", type_string)

                If Trim(user_id) <> "" Then
                    If Not IsNothing(atempTable) Then
                        If atempTable.Rows.Count > 0 Then
                            For Each q As DataRow In atempTable.Rows
                                If Trim(q("user_id")) = Trim(user_id) Then
                                    display_cbo.Items.Add(New ListItem(q("user_first_name") & " " & q("user_last_name") & "", q("user_id")))
                                End If
                            Next
                        End If
                    End If
                End If

                display_cbo.Items.Add(New ListItem("ALL", "0"))

                If Not IsNothing(atempTable) Then
                    If atempTable.Rows.Count > 0 Then
                        For Each q As DataRow In atempTable.Rows
                            If Trim(q("user_id")) <> Trim(user_id) Then
                                display_cbo.Items.Add(New ListItem(q("user_first_name") & " " & q("user_last_name") & "", q("user_id")))
                            End If
                        Next
                    End If
                End If





            Catch ex As Exception
                crmTestMaster.error_string = "clsgeneral.Master.vb - FillCRMUser_Homebase - " & ex.Message
                crmTestMaster.LogError(crmTestMaster.error_string)
            End Try
        End Sub

        Public Shared Sub FillCRMUserOnEvol(ByVal crmTestMaster As main_site, ByVal type_string As String, ByVal display_cbo As DropDownList, ByVal include_inactives As Boolean)
            Dim ExistsTable As New DataTable
            Dim selected As Long = 0
            Dim atempTable As DataTable
            Try
                ExistsTable = crmTestMaster.aclsData_Temp.Get_Client_User_By_Email_Address(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress)
                If Not IsNothing(ExistsTable) Then
                    If ExistsTable.Rows.Count > 0 Then
                        selected = ExistsTable.Rows(0).Item("cliuser_id")
                    End If
                End If

                display_cbo.Items.Add(New ListItem("My " & type_string, selected))

                If HttpContext.Current.Session.Item("localUser").crmUserType <> eUserTypes.MyNotesOnly Then
                    If include_inactives = True Then
                        atempTable = crmTestMaster.aclsData_Temp.Get_AllClientUser_Active("A")
                    Else
                        atempTable = crmTestMaster.aclsData_Temp.Get_AllClientUser_Active("Y")
                    End If

                    If Not IsNothing(atempTable) Then
                        If atempTable.Rows.Count > 0 Then
                            For Each q As DataRow In atempTable.Rows
                                If (q("cliuser_id") <> HttpContext.Current.Session.Item("localUser").crmLocalUserID) Then
                                    display_cbo.Items.Add(New ListItem(q("cliuser_first_name") & " " & q("cliuser_last_name") & " " & type_string & "", q("cliuser_id")))
                                End If
                            Next
                        End If
                    Else
                        If crmTestMaster.aclsData_Temp.class_error <> "" Then
                            crmTestMaster.error_string = crmTestMaster.aclsData_Temp.class_error
                            crmTestMaster.LogError("clsgeneral.vb - Fill_User_Dropdown - " & crmTestMaster.error_string)
                        End If
                        crmTestMaster.display_error()
                    End If
                    display_cbo.Items.Add(New ListItem("All " & type_string, "0"))
                End If
            Catch ex As Exception
                crmTestMaster.error_string = "clsgeneral.Master.vb - FillCRMUserOnEvol - " & ex.Message
                crmTestMaster.LogError(crmTestMaster.error_string)
            End Try
        End Sub

        Public Shared Sub Fill_User_Dropdown(ByVal display_cbo As DropDownList, ByVal type_string As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, Optional ByVal include_inactives As Boolean = False)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try


                display_cbo.Items.Add(New ListItem("My " & type_string, HttpContext.Current.Session.Item("localUser").crmLocalUserID))

                If HttpContext.Current.Session.Item("localUser").crmUserType <> eUserTypes.MyNotesOnly Then
                    If include_inactives = True Then
                        masterpage.aTempTable = masterpage.aclsData_Temp.Get_AllClientUser_Active("A")
                    Else
                        masterpage.aTempTable = masterpage.aclsData_Temp.Get_AllClientUser_Active("Y")
                    End If

                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In masterpage.aTempTable.Rows
                                If (q("cliuser_id") <> HttpContext.Current.Session.Item("localUser").crmLocalUserID) Then
                                    display_cbo.Items.Add(New ListItem(q("cliuser_first_name") & " " & q("cliuser_last_name") & " " & type_string & "", q("cliuser_id")))
                                End If
                            Next
                        Else
                            '0 rows
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("clsgeneral.vb - Fill_User_Dropdown - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                    End If
                    display_cbo.Items.Add(New ListItem("All " & type_string, "0"))
                End If

            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - Fill_User_Dropdown - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub

        Public Shared Sub Fill_Note_Category(ByVal notes_cat As DropDownList, ByVal document As String, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site, Optional ByVal SortBy As String = "")
            Dim masterpage As Object

            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try
                'Filling Note Category Up. 
                masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Note_Document_Category(document, SortBy)
                If Not IsNothing(masterpage.aTempTable) Then
                    If masterpage.aTempTable.Rows.Count > 0 Then
                        For Each z As DataRow In masterpage.aTempTable.Rows
                            notes_cat.Items.Add(New ListItem(z("notecat_name"), z("notecat_key")))
                        Next
                    End If
                Else
                    If masterpage.aclsData_Temp.class_error <> "" Then
                        masterpage.error_string = masterpage.aclsData_Temp.class_error
                        masterpage.LogError("opportunities page load - " & masterpage.error_string)
                    End If
                    masterpage.display_error()
                End If
                notes_cat.Items.Add(New ListItem("All", 0))
                notes_cat.SelectedValue = 0

            Catch ex As Exception
                masterpage.error_string = "clsgeneral.Master.vb - Fill_Note_Category - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub

        Public Shared Function what_cat(ByVal x As Integer, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            what_cat = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try
                If Not IsDBNull(x) Then
                    masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_Note_Category
                    If Not IsNothing(masterpage.aTempTable) Then
                        If masterpage.aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In masterpage.aTempTable.Rows
                                'notecat_key, notecat_name
                                If x = R("notecat_key") Then
                                    what_cat = R("notecat_name")
                                End If
                            Next
                        End If
                    Else
                        If masterpage.aclsData_Temp.class_error <> "" Then
                            masterpage.error_string = masterpage.aclsData_Temp.class_error
                            masterpage.LogError("main_site.Master.vb - what_cat() - " & masterpage.error_string)
                        End If
                        masterpage.display_error()
                        what_cat = x
                    End If
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - what_cat() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

        End Function

        Public Shared Function what_user(ByVal x As Object, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site) As String
            what_user = ""
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try
                If IsDBNull(x) Then
                Else
                    If IsNumeric(x) Then
                        masterpage.aTempTable = masterpage.aclsData_Temp.Get_Client_User(CInt(x))
                        If Not IsNothing(masterpage.aTempTable) Then
                            If masterpage.aTempTable.Rows.Count > 0 Then
                                For Each r As DataRow In masterpage.aTempTable.Rows
                                    what_user = r("cliuser_first_name") & " " & Left(r("cliuser_last_name"), 15)
                                Next
                            End If
                        Else
                            If masterpage.aclsData_Temp.class_error <> "" Then
                                masterpage.error_string = masterpage.aclsData_Temp.class_error
                                masterpage.LogError("main_site.Master.vb - what_user() - " & masterpage.error_string)
                            End If
                            masterpage.display_error()
                        End If
                    Else
                        Return x.ToString
                    End If
                End If
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - what_user() - " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try

        End Function

        Public Shared Sub Set_IDS(ByVal aclsData_Temp As clsData_Manager_SQL)
            Try
                Dim atemptable2 As New DataTable
                If Not IsNothing(HttpContext.Current.Request.Item("ac_ID")) Then
                    If IsNumeric(HttpContext.Current.Request.Item("ac_ID").Trim) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("ac_ID").ToString) Then
                            HttpContext.Current.Session("ListingID") = HttpContext.Current.Request.Item("ac_ID").Trim
                            HttpContext.Current.Session("OtherID") = 0
                            If Not IsNothing(HttpContext.Current.Request.Item("source")) Then
                                If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("source").ToString) Then
                                    HttpContext.Current.Session("ListingSource") = HttpContext.Current.Request.Item("source").Trim

                                    If HttpContext.Current.Session("ListingSource") = "JETNET" Then
                                        atemptable2 = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(HttpContext.Current.Session("ListingID"))
                                    Else
                                        atemptable2 = aclsData_Temp.Get_Clients_Aircraft(HttpContext.Current.Session("ListingID"))
                                    End If

                                    If Not IsNothing(atemptable2) Then
                                        If atemptable2.Rows.Count > 0 Then
                                            'Raise event to set Other ID for Aircraft.This would be the client ID for a jetnet Aircraft if one exists. 
                                            If HttpContext.Current.Session("ListingSource") = "JETNET" Then
                                                HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("cliaircraft_id")
                                            Else
                                                HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("cliaircraft_jetnet_ac_id")
                                            End If
                                        End If
                                    Else
                                        If aclsData_Temp.class_error <> "" Then
                                            aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, aclsData_Temp.class_error, DateTime.Now.ToString())
                                        End If
                                    End If
                                Else

                                End If
                            End If
                        End If
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Request.Item("comp_ID")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("comp_ID").ToString) Then

                        If IsNumeric(HttpContext.Current.Request.Item("comp_ID").Trim) Then
                            HttpContext.Current.Session("ListingID") = HttpContext.Current.Request.Item("comp_ID").Trim
                            If Not IsNothing(HttpContext.Current.Request.Item("source")) Then
                                If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("source").ToString) Then
                                    HttpContext.Current.Session("ListingSource") = HttpContext.Current.Request.Item("source").Trim
                                    HttpContext.Current.Session("OtherID") = 0
                                    If HttpContext.Current.Session("ListingSource") = "JETNET" Then
                                        atemptable2 = aclsData_Temp.GetCompanyInfo_JETNET_ID(HttpContext.Current.Session("ListingID"), "")
                                    Else
                                        atemptable2 = aclsData_Temp.GetCompanyInfo_ID(HttpContext.Current.Session("ListingID"), "CLIENT", 0)
                                    End If
                                    If Not IsNothing(atemptable2) Then
                                        If atemptable2.Rows.Count > 0 Then
                                            'Raise event to set Other ID for Aircraft.This would be the client ID for a jetnet Aircraft if one exists. 
                                            If HttpContext.Current.Session("ListingSource") = "JETNET" Then
                                                'HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("clicomp_jetnet_comp_id")
                                                HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("comp_id")
                                            Else
                                                HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("jetnet_comp_id")
                                                'HttpContext.Current.Session("OtherID") = atemptable2.Rows(0).Item("comp_id")
                                            End If
                                        End If
                                    Else
                                        If aclsData_Temp.class_error <> "" Then
                                            aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, aclsData_Temp.class_error, DateTime.Now.ToString())
                                        End If
                                    End If
                                Else

                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, "clsgeneral.vb - set_Ids() - " & ex.ToString, DateTime.Now.ToString())
            End Try
        End Sub
        Public Shared Function RemoveHTML(ByVal strText)
            If Not IsDBNull(strText) Then
                Dim nPos1
                Dim nPos2

                nPos1 = InStr(strText, "<")
                Do While nPos1 > 0
                    nPos2 = InStr(nPos1 + 1, strText, ">")
                    If nPos2 > 0 Then
                        strText = Left(strText, nPos1 - 1) & Mid(strText, nPos2 + 1)
                    Else
                        Exit Do
                    End If
                    nPos1 = InStr(strText, "<")
                Loop
            End If
            RemoveHTML = strText
        End Function

        Public Shared Function Display_Listing_Note_Email_Text(ByVal lnote As Object, ByVal status As Object) As String
            Display_Listing_Note_Email_Text = ""
            lnote = IIf(Not IsDBNull(lnote), lnote, "")
            status = IIf(Not IsDBNull(status), status, "")
            If status = "E" Then
                Dim info As Array = Split(HttpUtility.HtmlDecode(lnote), ":::")
                If Not IsNothing(info(2)) Then
                    lnote = ("<b><em>To: " & IIf(Not IsNothing(info(0)), info(0), "") & "</em></b><br />Subject: " & info(2))
                End If
            Else
                lnote = IIf((Len(lnote) > 255), Left(lnote, 255) & "...", lnote)
            End If

            Return lnote
        End Function

        Public Shared Sub Company_Last_Search_Selection(ByVal search_for_txt As TextBox, ByVal subset As DropDownList, ByVal search_where As DropDownList, ByVal search_for_cbo As TextBox, ByVal status_cbo As DropDownList, ByVal special_field_cbo As DropDownList, ByVal state As ListBox, ByVal country As DropDownList, ByVal types_of_owners As DropDownList, ByVal show_all As CheckBox, ByVal special_field_txt As TextBox, ByVal special_field_view As CheckBox, ByVal search_pnl As Panel, ByVal company_phone_number As TextBox, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If

            Try
                Dim comp_search As Array = Split(HttpContext.Current.Session("search_company"), "@")
                Dim states As Array = Split(comp_search(7), ",")
                Dim statestr As String = ""

                subset.SelectedValue = comp_search(5)
                country.SelectedValue = comp_search(6)
                types_of_owners.SelectedValue = comp_search(8)
                If IsNothing(comp_search(9)) Then
                    show_all.Checked = False
                Else
                    show_all.Checked = comp_search(9)
                End If
                special_field_cbo.SelectedValue = comp_search(10)
                special_field_txt.Text = Trim(comp_search(11))
                special_field_view.Checked = comp_search(12)
                special_field_cbo.Text = comp_search(13)
                company_phone_number.Text = comp_search(14)
                search_for_txt.Text = Trim(comp_search(1))
                search_where.Text = comp_search(2)
                'search_for_cbo.Text = comp_search(3)
                status_cbo.Text = comp_search(4)
                special_field_cbo.SelectedValue = comp_search(10)
                If comp_search(6) <> "" Or comp_search(7) <> "" Or comp_search(8) <> "" Or comp_search(9) <> "" Or comp_search(10) <> "" Or comp_search(11) <> "" Or comp_search(12) <> "" Or comp_search(13) <> "" Then
                    'Advanced_Search_Company_Fill_In() 'make advanced search visible
                    'is the special field selected?
                    'Select_Special_Field(comp_search(10))
                End If

                If Replace(comp_search(7), "'", "") <> "" Then
                    state.Visible = True
                    search_pnl.Height = 170
                    For x = 0 To UBound(states)
                        'state.Items(2).Selected = True
                        For j As Integer = 0 To state.Items.Count - 1
                            Dim mode As String = UCase(state.Items(j).Value)
                            Dim et As String = Replace(UCase(states(x)), "'", "")
                            If mode = et Then
                                state.Items(j).Selected = True
                            Else
                            End If
                        Next
                    Next
                End If

                subset.SelectedValue = comp_search(5)
                country.SelectedValue = comp_search(6)
                types_of_owners.SelectedValue = comp_search(8)
                show_all.Checked = comp_search(9)
                special_field_cbo.SelectedValue = comp_search(10)
                special_field_txt.Text = Trim(comp_search(11))
                special_field_view.Checked = comp_search(12)
                special_field_cbo.Text = comp_search(13)

                ' RaiseEvent Searched_Me(Me, False, clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True), search_where.Text, search_for_cbo.Text, status_cbo.Text, subset.SelectedValue, country.SelectedValue, comp_search(7), types_of_owners.SelectedValue, show_all.Checked, special_field_cbo.SelectedValue, clsGeneral.clsGeneral.StripChars(special_field_txt.Text, True), special_field_view.Checked, special_field_cbo.Text)
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Company_Last_Search_Selection() " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub

        Public Shared Sub Contact_Last_Search_Selection(ByVal first_name As TextBox, ByVal last_name As TextBox, ByVal search_where As DropDownList, ByVal search_for_cbo As DropDownList, ByVal comp_name_txt As TextBox, ByVal status_cbo As DropDownList, ByVal ordered_by As DropDownList, ByVal subset As DropDownList, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try
                Dim contact_search As Array = Split(HttpContext.Current.Session("search_contact"), "@")
                first_name.Text = contact_search(0)
                last_name.Text = contact_search(1)
                search_where.Text = contact_search(2)
                search_for_cbo.SelectedValue = contact_search(3)
                comp_name_txt.Text = contact_search(4)
                status_cbo.SelectedValue = contact_search(5)
                ordered_by.SelectedValue = contact_search(6)
                subset.SelectedValue = contact_search(7)

                Dim ord As String = contact_search(6)
                Dim x As String = contact_search(5)
                Dim z As String = contact_search(4)

            Catch ex As Exception
                masterpage.error_string = "clsGeneral.vb - contact_last_search_selection " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub

        Public Shared Sub Aircraft_Last_Search_Selection(ByVal state As ListBox, ByVal state_text As TableCell, ByVal search_pnl As Panel, ByVal search_for_txt As TextBox, ByVal search_where As DropDownList, ByVal search_for_cbo As DropDownList, ByVal model_cbo As ListBox, ByVal market_status_cbo As DropDownList, ByVal sort_method_cbo As DropDownList, ByVal sort_by_cbo As DropDownList, ByVal subset As DropDownList, ByVal airport_name As TextBox, ByVal icao_code As TextBox, ByVal iata_code As TextBox, ByVal city As TextBox, ByVal country As DropDownList, ByVal types_of_owners As DropDownList, ByVal on_lease As DropDownList, ByVal on_exclusive As DropDownList, ByVal year_start As DropDownList, ByVal year_end As DropDownList, ByVal aftt As CheckBox, ByVal mob As crmWebClient.Mobile, ByVal main As crmWebClient.main_site)
            Dim masterpage As Object
            If Not IsNothing(mob) Then
                masterpage = New crmWebClient.Mobile
                masterpage = mob
            Else
                masterpage = New crmWebClient.main_site
                masterpage = main
            End If
            Try

                Dim ac_search As Array = Split(HttpContext.Current.Session("search_aircraft"), "@")
                ac_search(3) = Replace(ac_search(3), "'", "")
                ac_search(13) = Replace(ac_search(13), "'", "")
                Dim models As Array = Split(ac_search(3), ",")
                Dim states As Array = Split(ac_search(13), ",")
                If ac_search(8) <> "" Or ac_search(9) <> "" Or ac_search(10) <> "" Or ac_search(11) <> "" Or ac_search(12) <> "" Then
                    'set the advanced search clicky option.
                    'Advanced_Search_Fill_In_Click()
                End If
                If ac_search(13) <> "" Then
                    state.Visible = True
                    state_text.Visible = True
                    search_pnl.Height = 305
                End If

                search_for_txt.Text = ac_search(0)
                search_where.SelectedValue = ac_search(1)
                search_for_cbo.SelectedValue = ac_search(2)


                'refil the models 
                For x = 0 To UBound(models)
                    '  Response.Write(models(x) & "<br />")
                    For j As Integer = 0 To model_cbo.Items.Count() - 1
                        If model_cbo.Items(0).Selected = True Then
                            model_cbo.Items(0).Selected = False
                        End If
                        Dim mode As String = UCase(model_cbo.Items(j).Value)
                        Dim et As String = UCase(models(x))
                        If UCase(model_cbo.Items(j).Value) = UCase(models(x)) Then
                            model_cbo.Items(j).Selected = True
                        Else
                        End If
                    Next
                Next
                'refil the states
                For x = 0 To UBound(states)
                    For j As Integer = 0 To state.Items.Count() - 1
                        Dim mode As String = UCase(state.Items(j).Value)
                        Dim et As String = UCase(states(x))
                        If UCase(state.Items(j).Value) = UCase(states(x)) Then
                            state.Items(j).Selected = True
                        Else
                        End If
                    Next
                Next

                market_status_cbo.SelectedValue = ac_search(4)
                sort_method_cbo.SelectedValue = ac_search(5)
                sort_by_cbo.SelectedValue = ac_search(6)
                subset.SelectedValue = ac_search(7)
                airport_name.Text = ac_search(8)
                icao_code.Text = ac_search(9)
                iata_code.Text = ac_search(10)
                city.Text = ac_search(11)
                country.SelectedValue = ac_search(12)
                types_of_owners.SelectedValue = ac_search(14)
                on_lease.SelectedValue = ac_search(15)
                on_exclusive.SelectedValue = ac_search(16)
                year_start.SelectedValue = ac_search(17)
                year_end.SelectedValue = ac_search(18)
                Try
                    aftt.Checked = ac_search(19)
                Catch
                    aftt.Checked = False
                End Try

                'raise search event!
                'Click_Search()
            Catch ex As Exception
                masterpage.error_string = "clsgeneral.vb - Aircraft_Last_Search_Selection() " & ex.Message
                masterpage.LogError(masterpage.error_string)
            End Try
        End Sub
        Public Shared Sub Fill_Opportunity_Category(ByVal notes_opp As DropDownList, ByVal atemptable As DataTable, ByVal aclsdata_temp As clsData_Manager_SQL)
            atemptable = aclsdata_temp.Get_Opportunity_Categories()
            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each q As DataRow In atemptable.Rows
                        notes_opp.Items.Add(New ListItem(q(1), q(0)))
                    Next
                End If
            End If
        End Sub
        Public Shared Sub Fill_Type_Category(ByVal notes_opp As DropDownList, ByVal atemptable As DataTable, ByVal aclsdata_temp As clsData_Manager_SQL)
            atemptable = aclsdata_temp.Get_Opportunity_Categories()
            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each q As DataRow In atemptable.Rows
                        notes_opp.Items.Add(New ListItem(q(1), q(0)))
                    Next
                End If
            End If
        End Sub
        Public Shared Function AddNextPreviousToNotesTable(ByVal notesTable As DataTable) As DataTable
            Dim ResultsTable As New DataTable
            Dim nextColumn As New DataColumn
            Dim previousColumn As New DataColumn
            Dim RowCount As Integer = 0
            Dim sortTable As New DataTable
            sortTable = notesTable.Clone
            ResultsTable = notesTable.Clone
            nextColumn.DataType = System.Type.GetType("System.Int64")
            nextColumn.DefaultValue = 0
            nextColumn.Unique = False
            nextColumn.ColumnName = "lnote_next_id"
            ResultsTable.Columns.Add(nextColumn)



            previousColumn.DataType = System.Type.GetType("System.Int64")
            previousColumn.DefaultValue = 0
            previousColumn.Unique = False
            previousColumn.ColumnName = "lnote_previous_id"
            ResultsTable.Columns.Add(previousColumn)

            Dim afileterd As DataRow()
            afileterd = notesTable.Select("", "lnote_entry_date desc")
            For Each atmpDataRow As DataRow In afileterd
                sortTable.ImportRow(atmpDataRow)
            Next

            For Each drRow As DataRow In sortTable.Rows
                Dim newRow As DataRow = ResultsTable.NewRow()

                newRow.Item("lnote_id") = drRow("lnote_id")
                newRow.Item("lnote_jetnet_ac_id") = drRow("lnote_jetnet_ac_id")
                newRow.Item("lnote_opportunity_status") = drRow("lnote_opportunity_status")
                newRow.Item("lnote_jetnet_comp_id") = drRow("lnote_jetnet_comp_id")
                newRow.Item("lnote_client_ac_id") = drRow("lnote_client_ac_id")
                newRow.Item("lnote_client_comp_id") = drRow("lnote_client_comp_id")
                newRow.Item("lnote_jetnet_contact_id") = drRow("lnote_jetnet_contact_id")
                newRow.Item("lnote_client_contact_id") = drRow("lnote_client_contact_id")
                newRow.Item("lnote_note") = drRow("lnote_note")
                newRow.Item("lnote_entry_date") = drRow("lnote_entry_date")
                newRow.Item("lnote_action_date") = drRow("lnote_action_date")
                newRow.Item("lnote_user_login") = drRow("lnote_user_login")
                newRow.Item("lnote_user_name") = drRow("lnote_user_name")
                newRow.Item("lnote_notecat_key") = drRow("lnote_notecat_key")
                newRow.Item("lnote_status") = drRow("lnote_status")
                newRow.Item("lnote_schedule_start_date") = drRow("lnote_schedule_start_date")
                newRow.Item("lnote_schedule_end_date") = drRow("lnote_schedule_end_date")
                newRow.Item("lnote_user_id") = drRow("lnote_user_id")
                newRow.Item("lnote_clipri_ID") = drRow("lnote_clipri_ID")
                newRow.Item("lnote_document_flag") = drRow("lnote_document_flag")
                newRow.Item("lnote_jetnet_amod_id") = drRow("lnote_jetnet_amod_id")
                newRow.Item("lnote_jetnet_yacht_id") = drRow("lnote_jetnet_yacht_id")
                newRow.Item("lnote_client_amod_id") = drRow("lnote_client_amod_id")
                newRow.Item("lnote_document_name") = drRow("lnote_document_name")


                If RowCount = 0 Then
                    newRow.Item("lnote_previous_id") = 0
                Else
                    newRow.Item("lnote_previous_id") = sortTable.Rows(RowCount - 1).Item("lnote_id")
                End If

                If RowCount + 1 = notesTable.Rows.Count Then
                    newRow.Item("lnote_next_id") = 0
                Else
                    newRow.Item("lnote_next_id") = sortTable.Rows(RowCount + 1).Item("lnote_id")
                End If


                ResultsTable.Rows.Add(newRow)
                ResultsTable.AcceptChanges()

                RowCount += 1

            Next


            Return ResultsTable
        End Function
        Public Shared Function limit_rows(ByVal data As DataTable, ByVal start_count As Integer, ByVal end_count As Integer) As DataTable
            'Grab all of the silly client records without jetnet trans ID
            Dim returnTable As New DataTable
            returnTable = data.Clone
            Dim afileterd As DataRow()
            afileterd = data.Select("", "lnote_entry_date desc")

            ' 'a single data row for importing to the dalTable
            Dim atmpDataRow As DataRow
            ' extract and import
            For Each atmpDataRow In afileterd
                returnTable.ImportRow(atmpDataRow)
            Next


            Dim myTable As DataTable = returnTable.Clone()
            Dim rowcount As Integer = returnTable.Rows.Count
            If end_count < rowcount Then
                rowcount = end_count
            Else
                rowcount = rowcount
            End If
            ' in standard RSS, the feed items are here
            Dim myRows As DataRow() = returnTable.[Select]()
            For i As Integer = start_count To rowcount - 1
                If i < myRows.Length Then
                    myTable.ImportRow(myRows(i))
                    myTable.AcceptChanges()
                End If
            Next
            Return myTable
        End Function
        Public Shared Sub Model_Type_Selected_Index_Changed(ByVal type As ListBox, ByVal model_type As CheckBoxList)
            type.Items.Clear()
            type.Items.Add(New ListItem("All", "All"))
            Dim StartTable As New DataTable
            Dim ResultFilter As New DataTable
            Dim Commercial As Boolean = False
            Dim Business As Boolean = False
            Dim Helicopter As Boolean = False
            Dim selectString As String = ""
            Dim selectString_No As String = ""
            For Each i As ListItem In model_type.Items
                If i.Selected = True Then
                    If i.Value = "Helicopter" Then
                        Helicopter = True
                    ElseIf i.Value = "Commercial" Then
                        Commercial = True
                    ElseIf i.Value = "Business" Then
                        Business = True
                    End If
                End If
            Next


            If Business = True Then
                selectString += " ( amod_product_business_flag='Y' "
                selectString += " )"
            Else
                selectString_No += " ( amod_product_business_flag='N' )"
            End If

            If Business = True And Helicopter = True Then
                selectString += " or amod_product_helicopter_flag='Y' "
            ElseIf Helicopter = True Then
                selectString += "  amod_product_helicopter_flag='Y' "
            Else
                If selectString_No <> "" Then
                    selectString_No += " and "
                End If
                selectString_No += "  amod_product_helicopter_flag='N' "
            End If
            If (Business = True And Commercial = True) Or (Helicopter = True And Commercial = True) Then
                selectString += " or amod_product_commercial_flag='Y' "
            ElseIf Commercial = True Then
                selectString += "  amod_product_commercial_flag='Y' "
            Else
                If selectString_No <> "" Then
                    selectString_No += " and "
                End If
                selectString_No += "  amod_product_commercial_flag='N' "
            End If

            If selectString_No <> "" Then
                selectString_No = " and ( " & selectString_No & " )"
            End If
            selectString = selectString & selectString_No
            If Not IsNothing(HttpContext.Current.Session.Item("TypeTable")) Then
                StartTable = HttpContext.Current.Session.Item("TypeTable")
                ResultFilter = StartTable.Clone
                Dim afiltered As DataRow() = StartTable.Select(selectString & "", "")
                ' extract and import
                For Each atmpDataRow_Client In afiltered
                    ResultFilter.ImportRow(atmpDataRow_Client)
                Next

                For Each q As DataRow In ResultFilter.Rows 'HttpContext.Current.Session.Item("TypeTable").Rows

                    If Not IsDBNull(q("atype_name")) Then
                        If (type.Items.FindByValue(q("amod_type_code") & "|" & q("amod_airframe_type_code")) Is Nothing) Then
                            type.Items.Add(New ListItem(q("atype_name"), q("amod_type_code") & "|" & q("amod_airframe_type_code")))
                        End If
                    End If
                Next
            End If
        End Sub
        Public Shared Sub Make_Selected_Index_Changed(ByVal model As ListBox, ByVal make As ListBox, ByVal type As ListBox, Optional ByVal RunEvent As Boolean = False, Optional ByVal UseDefault As Boolean = False)
            Dim temptable As New DataTable
            If Not IsNothing(HttpContext.Current.Session.Item("table")) Then
                temptable = HttpContext.Current.Session.Item("table")
                HttpContext.Current.Session.Item("MakeSelection") = ""
                model.Items.Clear()
                model.Items.Add(New ListItem("All", "All"))
                Dim ModelDataTable As DataTable = temptable.DefaultView.ToTable(True, "amod_model_name", "MakeAbbrev", "amod_make_name", "amod_id", "atype_code", "amod_airframe_type_code")
                For Each q As DataRow In ModelDataTable.Rows
                    If Not IsDBNull(q("amod_model_name")) Then
                        If Not make.Items.FindByValue("" & q("atype_code") & "|" & q("amod_make_name")) Is Nothing Then
                            If make.Items.FindByValue("" & q("atype_code") & "|" & q("amod_make_name")).Selected = True Or make.Items.FindByValue("All").Selected = True Then
                                If type.Items.FindByValue(q("atype_code") & "|" & q("amod_airframe_type_code")).Selected = True Or type.Items.FindByValue("All").Selected = True Then
                                    model.Items.Add(New ListItem("[" & q("MakeAbbrev") & "] " & q("amod_model_name"), q("amod_id") & "|" & q("amod_make_name") & "|" & q("amod_model_name") & "|JETNET|0"))
                                    HttpContext.Current.Session.Item("MakeSelection") += q("atype_code") & "|" & q("amod_make_name") & "##"
                                End If
                            End If
                        End If

                    End If
                Next
                model.SelectedValue = "All"


                If UseDefault = True Then
                    'loop through the model
                    model.SelectedIndex = -1
                    For ListBoxCount As Integer = 0 To model.Items.Count() - 1
                        If UCase(model.Items(ListBoxCount).Value) <> "ALL" Then
                            model.Items(ListBoxCount).Selected = True
                        End If
                    Next
                End If



                'HttpContext.Current.Session.Item("ModelSelection")
                If Not IsNothing(HttpContext.Current.Session.Item("ModelSelection")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("ModelSelection")) Then
                        If Not IsNothing(model) Then
                            Dim MultipleSelection As Array
                            'We split the answer.
                            MultipleSelection = HttpContext.Current.Session.Item("ModelSelection").Split("##")
                            If model.SelectionMode = ListSelectionMode.Multiple Then
                                model.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                                'that the page defaults to.
                                For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                                    For ListBoxCount As Integer = 0 To model.Items.Count() - 1
                                        If UCase(model.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                            model.Items(ListBoxCount).Selected = True
                                        End If
                                    Next
                                Next
                            Else
                                model.SelectedValue = HttpContext.Current.Session.Item("ModelSelection")
                            End If

                        End If

                    End If

                End If

            End If

        End Sub
        Public Shared Sub Type_Selected_Index_Changed(ByVal make As ListBox, ByVal type As ListBox, ByVal pageisPostback As Boolean, Optional ByVal model As ListBox = Nothing, Optional ByVal RunEvent As Boolean = False, Optional ByVal UseDefault As Boolean = False)
            Dim temptable As New DataTable
            Dim page As New System.Web.UI.Page
            'Clearing out session variable.
            'Working process/Not finished by any means.
            HttpContext.Current.Session.Item("TypeSelection") = ""


            If Not IsNothing(HttpContext.Current.Session.Item("table")) Then
                temptable = HttpContext.Current.Session.Item("table")
                make.Items.Clear()
                Dim MakeDataTable As DataTable = temptable.DefaultView.ToTable(True, "amod_make_name", "amod_type_code", "amod_airframe_type_code")

                make.Items.Add(New ListItem("All", "All"))
                If pageisPostback Then
                    For Each q As DataRow In MakeDataTable.Rows
                        If Not IsDBNull(q("amod_make_name")) Then
                            If Not (type.Items.FindByValue(q("amod_type_code") & "|" & q("amod_airframe_type_code")) Is Nothing) Then
                                If type.Items.FindByValue(q("amod_type_code") & "|" & q("amod_airframe_type_code")).Selected = True Or type.Items.FindByValue("All").Selected = True Then
                                    'added 8/13
                                    HttpContext.Current.Session.Item("TypeSelection") += q("amod_type_code") & "|" & q("amod_airframe_type_code") & "##"
                                    If (make.Items.FindByValue(q("amod_make_name")) Is Nothing) Then
                                        make.Items.Add(New ListItem("[" & q("amod_airframe_type_code") & "][" & q("amod_type_code") & "] " & q("amod_make_name"), "" & q("amod_type_code") & "|" & q("amod_make_name")))
                                    End If
                                End If
                            End If

                        End If
                    Next
                    make.SelectedValue = "All"

                    'use default code
                    If UseDefault = True Then
                        RunEvent = True
                        make.SelectedIndex = -1
                        For ListBoxCount As Integer = 0 To make.Items.Count() - 1
                            If UCase(make.Items(ListBoxCount).Value) <> "ALL" Then
                                make.Items(ListBoxCount).Selected = True
                            End If
                        Next
                    End If

                    'Make Selection
                    'Working on this, added to test some things
                    If Not IsNothing(HttpContext.Current.Session.Item("MakeSelection")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MakeSelection")) Then
                            If Not IsNothing(model) Then
                                Dim MultipleSelection As Array
                                'We split the answer.
                                MultipleSelection = HttpContext.Current.Session.Item("MakeSelection").Split("##")
                                If make.SelectionMode = ListSelectionMode.Multiple Then
                                    make.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                                    'that the page defaults to.
                                    For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                                        For ListBoxCount As Integer = 0 To make.Items.Count() - 1
                                            If UCase(make.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                                make.Items(ListBoxCount).Selected = True
                                            End If
                                        Next
                                    Next
                                Else
                                    make.SelectedValue = HttpContext.Current.Session.Item("MakeSelection")
                                End If

                            End If

                        End If

                    End If

                    'This means that the model listbox has been passed to the function
                    'And that the runevent has been passed as well. This means that
                    'The types have been selected up top and they need to be ran.
                    If RunEvent = True Then
                        Make_Selected_Index_Changed(model, make, type, True, True)
                    End If

                End If

            End If
        End Sub

        ''' <summary>
        ''' This is used for the evo search boxes on the ac page and where the model is used
        ''' </summary>
        ''' <param name="TypeDataTable"></param>
        ''' <param name="model_type"></param>
        ''' <param name="masterpage"></param>
        ''' <param name="temptable"></param>
        ''' <param name="TypeDataHold"></param>
        ''' <param name="type"></param>
        ''' <remarks></remarks>
        Public Shared Sub Getting_Type_Listbox_Set(ByVal TypeDataTable As DataTable, ByVal model_type As CheckBoxList, ByVal masterpage As Object, ByVal temptable As DataTable, ByVal TypeDataHold As DataTable, ByVal type As ListBox, Optional ByVal Model As ListBox = Nothing, Optional ByVal Make As ListBox = Nothing, Optional ByVal RunEvent As Boolean = False, Optional ByVal UseDefault As Boolean = False)
            'Now we're sorting based on checkboxes
            'This doesn't need on post back because it needs to filter the table every postback.
            'Here we go, this is going to build the select statement, it's kinda weird.
            Dim helicopter As Boolean = False
            Dim business As Boolean = False
            Dim commercial As Boolean = False

            For i = 0 To model_type.Items.Count - 1
                If model_type.Items(i).Selected Then
                    If model_type.Items(i).Value = "Helicopter" Then
                        helicopter = True
                    ElseIf model_type.Items(i).Value = "Business" Then
                        business = True
                    ElseIf model_type.Items(i).Value = "Commercial" Then
                        commercial = True
                    End If
                End If
            Next
            SettingUpTypeListbox(helicopter, business, commercial, TypeDataTable, masterpage, temptable, TypeDataHold, type, Model, Make, RunEvent, UseDefault)
        End Sub


        Public Shared Sub SettingUpTypeListbox(ByVal helicopter As Boolean, ByVal business As Boolean, ByVal commercial As Boolean, ByVal TypeDataTable As DataTable, ByVal masterpage As Object, ByVal temptable As DataTable, ByVal TypeDataHold As DataTable, ByVal type As ListBox, Optional ByVal Model As ListBox = Nothing, Optional ByVal Make As ListBox = Nothing, Optional ByVal RunEvent As Boolean = False, Optional ByVal UseDefault As Boolean = False)
            Dim select_string As String = ""
            Dim afiltered As DataRow()
            TypeDataTable = masterpage.aclsData_Temp.Create_Type_DataTable
            TypeDataHold = masterpage.aclsData_Temp.Create_Type_DataTable

            TypeDataTable = masterpage.aclsData_Temp.GetAircraft_Type(helicopter, business, commercial, IIf(UseDefault = True, HttpContext.Current.Session.Item("localUser").crmSelectedModels, ""))
            Dim t As Integer = TypeDataTable.Rows.Count
            HttpContext.Current.Session.Item("TypeTable") = TypeDataTable
            HttpContext.Current.Session.Item("OriginalTypeTable") = TypeDataTable

            If Not IsNothing(HttpContext.Current.Session.Item("OriginalTypeTable")) Then
                TypeDataTable = HttpContext.Current.Session.Item("OriginalTypeTable")
            End If



            temptable = masterpage.aclsData_Temp.GetAircraft_MakeModels("", "", helicopter, business, commercial, HttpContext.Current.Session.Item("localSubscription").crmJets_Flag, HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag, HttpContext.Current.Session.Item("localSubscription").crmTurboprops, IIf(UseDefault = True, HttpContext.Current.Session.Item("localUser").crmSelectedModels, ""))
            HttpContext.Current.Session.Item("table") = temptable


            If helicopter = True Then
                select_string = "amod_product_helicopter_flag = 'Y'"
            Else
                select_string = "amod_product_helicopter_flag = 'N'"
            End If

            If business = True Then
                select_string = select_string & " or amod_product_business_flag= 'Y' "
            Else
                select_string = select_string & " and amod_product_business_flag= 'N' "
            End If

            If commercial = True Then
                select_string = select_string & " or amod_product_commercial_flag= 'Y' "
            Else
                select_string = select_string & " and amod_product_commercial_flag= 'N' "
            End If

            TypeDataHold = TypeDataTable.Clone

            If TypeDataTable.Columns.Contains("amod_airframe_type_code") Then
                afiltered = TypeDataTable.Select(select_string, "amod_airframe_type_code asc, amod_type_code asc")
            Else
                afiltered = TypeDataTable.Select("", "")
            End If

            For Each atmpDataRow In afiltered
                TypeDataHold.ImportRow(atmpDataRow)
            Next
            TypeDataTable = masterpage.aclsData_Temp.Create_Type_DataTable
            TypeDataTable = TypeDataHold
            TypeDataHold = masterpage.aclsData_Temp.Create_Type_DataTable

            HttpContext.Current.Session.Item("TypeTable") = TypeDataTable

            For Each q As DataRow In TypeDataTable.Rows
                If Not IsDBNull(q("atype_name")) Then
                    If (type.Items.FindByValue(q("amod_type_code") & "|" & q("amod_airframe_type_code")) Is Nothing) Then
                        type.Items.Add(New ListItem(q("atype_name"), q("amod_type_code") & "|" & q("amod_airframe_type_code")))
                    End If
                End If
            Next

            'use default selection:
            If UseDefault = True Then
                RunEvent = True
                type.SelectedIndex = -1
                For ListBoxCount As Integer = 0 To type.Items.Count() - 1
                    If UCase(type.Items(ListBoxCount).Text) <> "ALL" Then
                        type.Items(ListBoxCount).Selected = True
                    End If
                Next
            End If

            'Type Selection 

            If Not IsNothing(HttpContext.Current.Session.Item("TypeSelection")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("TypeSelection")) Then
                    Dim MultipleSelection As Array
                    'We split the answer.
                    MultipleSelection = HttpContext.Current.Session.Item("TypeSelection").Split("##")
                    If type.SelectionMode = ListSelectionMode.Multiple Then
                        type.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                            For ListBoxCount As Integer = 0 To type.Items.Count() - 1
                                If UCase(type.Items(ListBoxCount).Value) = UCase(MultipleSelection(MultipleSelectionCount)) Then
                                    type.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    Else
                        type.SelectedValue = HttpContext.Current.Session.Item("TypeSelection")
                    End If
                End If
            End If


            If Not IsNothing(Model) Then
                If RunEvent = True Then
                    Type_Selected_Index_Changed(Make, type, True, Model, True, UseDefault)
                End If
            End If

        End Sub
        Public Shared Function MobileDisplayStatus(ByVal ac_forsale_flag As Object, ByVal ac_status As Object, ByVal ac_delivery As Object, ByVal ac_asking_price As Object, ByVal acListDate As Object, ByVal acAskingWordage As Object, ByVal History As Boolean, ByVal DOM As Object, Optional ByVal is_homebase As String = "N") As String
            Dim returnString As String = ""
            acAskingWordage = acAskingWordage.ToString



            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                returnString = "<span class=""div_clear display_block"">"
                If ac_forsale_flag = "Y" Then
                    returnString += "<span class=' " & IIf(History = True, "green_background_height", "") & "'>"
                    If acAskingWordage = "Price" Then
                        If Not IsDBNull(ac_asking_price) Then
                            returnString += "<span class=""green_text float_left"">" & crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(ac_asking_price)
                            returnString += "</span>"
                        End If
                    Else
                        returnString += "<span class=""green_text  float_left mobileAlignLeft"">" & acAskingWordage.ToString
                        returnString += "</span>"
                    End If
                    returnString += " <span class=""float_right mobileAlignRight"">DOM: " & DateDiff(DateInterval.Day, acListDate, DOM) & "</span></span>"
                Else
                    returnString += "<span class='" & IIf(History = True, "green_background_height", "display_block div_clear") & "'>" & ac_status & "</span>"
                End If
                returnString += "</span>"
            End If
            Return returnString
        End Function
        Public Shared Function DisplayStatusListingDateEvoACListing(ByVal ac_forsale_flag As Object, ByVal ac_status As Object, ByVal ac_delivery As Object, ByVal ac_asking_price As Object, ByVal acListDate As Object, ByVal acAskingWordage As Object, ByVal History As Boolean, ByVal DOM As Object, Optional ByVal is_homebase As String = "N") As String
            Dim returnString As String = ""
            acAskingWordage = acAskingWordage.ToString


            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                If ac_forsale_flag = "Y" Then
                    returnString = "<span class='green_background greenText " & IIf(History = True, "green_background_height", "") & "'><span class=""li"">"
                    returnString += "<span "
                    If ac_status.ToString.ToUpper = "FOR SALE" Then
                        returnString += " class=""forsaleBold"" "
                    End If
                    returnString += ">"
                    returnString += ac_status
                    returnString += "</span>"

                    If Not IsDBNull(ac_delivery) Then
                        If Trim(ac_delivery) <> "" Then
                            returnString += ", Delivery " & ac_delivery
                        End If
                    End If


                    If acAskingWordage = "Price" Then
                        If Not IsDBNull(ac_asking_price) Then
                            returnString += " <span class="""">(" & crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(ac_asking_price)
                            returnString += ")</span>"
                        End If
                    Else
                        returnString += " <span class=""tiny_text"">(" & acAskingWordage.ToString
                        returnString += ")</span>"
                    End If
                    returnString += " </span>"
                    If Not IsDBNull(acListDate) Then
                        If IsDate(acListDate) Then
                            returnString += "<span class=""li"">"
                            If Trim(is_homebase) = "Y" Then
                            Else
                                returnString += "Listed: " & Month(acListDate) & "/" & Day(acListDate) & "/" & Right(Year(acListDate), 2)
                            End If

                            'ADD MSW - taken out for homebase exports - 4/22/16
                            If Trim(is_homebase) = "N" Then
                                If Not IsDBNull(DOM) Then
                                    If IsDate(DOM) Then
                                        returnString += " <span class=""tiny_text"">(DOM: " & DateDiff(DateInterval.Day, acListDate, DOM) & ")</span>"
                                    End If
                                End If
                            End If
                        End If
                    End If
                    returnString += "</span></span>"
                Else
                    returnString += "<span class='li " & IIf(History = True, "green_background_height", "") & "'>" & ac_status & "</span>"
                End If
            End If
            Return returnString
        End Function

        Public Shared Function TwoPlaceYear(ByVal dateTransformed As Object) As String
            Dim returnYear As String = ""
            If Not IsDBNull(dateTransformed) Then
                If IsDate(dateTransformed) Then
                    returnYear = Month(dateTransformed) & "/" & Day(dateTransformed) & "/" & Right(Year(dateTransformed), 2)
                End If
            End If
            Return returnYear
        End Function


        Public Shared Function DisplayStatusListingDateEvoYachtListing(ByVal forsale_flag As Object, ByVal status As Object, ByVal asking_price As Object, ByVal ListDate As Object, ByVal Gallery As Boolean, ByVal is_lease As Object, ByVal is_charter As Object, ByVal wordage As Object, ByVal yachtID As Long) As String
            Dim returnString As String = ""
            forsale_flag = forsale_flag.ToString
            is_lease = is_lease.ToString
            is_charter = is_charter.ToString
            wordage = wordage.ToString
            status = status.ToString
            asking_price = asking_price.ToString
            ListDate = ListDate.ToString

            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                If forsale_flag = "Y" Or is_lease = "Y" Or is_charter = "Y" Then
                    returnString = "<span class='green_background " & IIf(Gallery = False, "padding", "") & "'><span class=""li"">" & status
                    returnString += " " & display_yacht_status(forsale_flag, is_charter, is_lease, yachtID)

                    If Trim(wordage) = "Inquire" Then
                        returnString += "<span class=""emphasis_text"">(Inquire)</span>"
                    Else
                        If Not IsDBNull(asking_price) Then
                            If Trim(asking_price) <> "" And Trim(asking_price) <> "0" Then
                                returnString += " <span class=""emphasis_text"">(" & Trim(crmWebClient.clsGeneral.clsGeneral.no_zero(asking_price, "", True))
                                returnString += ")</span>"
                            End If
                        End If
                    End If


                    returnString += " </span>"
                    If Not IsDBNull(ListDate) Then
                        If IsDate(ListDate) Then
                            returnString += "<span class=""li"">Date Listed: " & FormatDateTime(ListDate, DateFormat.ShortDate) & "</span>"
                        End If
                    End If
                    returnString += "</span>"
                Else
                    returnString += "<span class='li'>" & status & "</span>"
                End If
            End If
            Return returnString
        End Function

        Public Shared Function display_yacht_status(ByVal for_sale As String, ByVal for_charter As String, ByVal for_lease As String, Optional ByVal YachtID As Long = 0) As String
            Dim TitleText As String = ""
            Dim SpanTag As String = "<span>"
            display_yacht_status = ""

            If YachtID > 0 Then
                If Trim(for_sale) = "Y" Or Trim(for_charter) = "Y" Then
                    TitleText = YachtFunctions.DisplayYachtConfidentialNotes(for_charter, YachtID, "", True, for_sale)
                    If Not String.IsNullOrEmpty(Trim(TitleText)) Then
                        SpanTag = "<span class=""help_cursor underline"" title=""" & TitleText & """>"
                    End If
                End If
            End If


            If Trim(for_sale) = "Y" And Trim(for_charter) = "Y" And Trim(for_lease) = "Y" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Sale/Lease/Charter"
                display_yacht_status += "</span>"
            ElseIf Trim(for_sale) = "Y" And Trim(for_charter) = "Y" And Trim(for_lease) = "N" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Sale/Charter"
                display_yacht_status += "</span>"
            ElseIf Trim(for_sale) = "Y" And Trim(for_charter) = "N" And Trim(for_lease) = "Y" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Sale/Lease"
                display_yacht_status += "</span>"
            ElseIf Trim(for_sale) = "N" And Trim(for_charter) = "Y" And Trim(for_lease) = "Y" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Lease/Charter"
                display_yacht_status += "</span>"
            ElseIf Trim(for_sale) = "Y" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Sale"
                display_yacht_status += "</span>"
            ElseIf Trim(for_charter) = "Y" Then
                display_yacht_status = SpanTag
                display_yacht_status += "For Charter"
                display_yacht_status += "</span>"
            ElseIf Trim(for_lease) = "Y" Then
                display_yacht_status = "For Lease"
            End If

        End Function
        Public Shared Function make_yacht_link_get_request() As String
            make_yacht_link_get_request = HttpContext.Current.Request.Item("compid").ToString
        End Function


        'Create a Model.
        Public Shared Function Create_Model(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal cliamod_airframe_type As String, ByVal cliamod_make_name As String, ByVal cliamod_make_type As String, ByVal cliamod_manufacturer_name As String, ByVal cliamod_model_name As String, ByVal jetnet_amod_id As Integer) As Integer
            Dim aclsInsert_Client_Aircraft_Model As New clsClient_Aircraft_Model
            Dim aTempTable As New DataTable
            Dim model_id As Integer = 0

            aclsInsert_Client_Aircraft_Model.cliamod_airframe_type = cliamod_airframe_type
            aclsInsert_Client_Aircraft_Model.cliamod_make_name = cliamod_make_name
            aclsInsert_Client_Aircraft_Model.cliamod_make_type = cliamod_make_type
            aclsInsert_Client_Aircraft_Model.cliamod_manufacturer_name = cliamod_manufacturer_name
            aclsInsert_Client_Aircraft_Model.cliamod_model_name = cliamod_model_name
            aclsInsert_Client_Aircraft_Model.cliamod_jetnet_amod_id = jetnet_amod_id

            aTempTable = aclsData_Temp.Get_Clients_Aircraft_Model_Make_Model(cliamod_make_name, cliamod_model_name)

            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    For Each t As DataRow In aTempTable.Rows
                        model_id = t("cliamod_id")
                    Next
                Else
                    model_id = aclsData_Temp.Insert_Client_Aircraft_Model(aclsInsert_Client_Aircraft_Model) 'model doesn't exist - insert it
                End If

            Else
                'If aclsData_Temp.class_error <> "" Then
                '    LogError("Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & aclsData_Temp.class_error)
                'End If
            End If
            Return model_id
        End Function

        'Create the treeview
        Public Shared Sub Create_Tree_Nav(ByVal left_nav_tv As TreeView)

        End Sub


        Public Shared Sub LogError(ByVal ex As String, ByVal aclsData_Temp As clsData_Manager_SQL)
            aclsData_Temp.LogError(HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
        End Sub

        Public Shared Function GenerateProductCodeSelectionQuery_YachtsIncluded(ByRef crmSubScriptionCls As crmSubscriptionClass, ByVal Is_Operator_Flag As Boolean, ByVal Is_Aircraft_Flag As Boolean)
            '------------------------------------------ 
            ' Function: GenerateProductCodeSelectionQuery_CRM
            '
            ' This function takes in the local subscription class. 
            'If then names that crm subscription class and references it that way
            '
            '
            '
            ' This function take in two flags. One for Operator and one for Aircraft.
            ' Currently there is no way to do both a model/aircraft selection and an operator selection.
            '
            ' If Operator flag is true, then it runs the operator section query
            ' If Operator Flag is flase, it runs the model selection. It then checks the aircraft flag.
            ' If the aircraft flag is true, and the operator flag is false it will then run model and aircraft.

            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False) - This would run Just Model Selection Code 
            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True) -  This would run The Model Selection and the Aircraft Selection Code
            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), True, False) -  This would run Just the Operator Selection Code 
            '
            '
            '------------------------------------------
            Dim sSelectionClause As String = " "
            Dim nloop As Integer = 0
            Dim bSingleProduct As Boolean = True
            Dim string_for_type As String = ""
            Dim cAndClause As String = " AND "
            Dim cOrClause As String = " OR "
            Dim cSingleOpen As String = "("
            Dim cSingleClose As String = ")"


            'This has to be added.
            'In case a session is dropped, if this isn't there, it will cause an sql error
            'added 4-27-2012
            If (crmSubScriptionCls.crmHelicopter_Flag = True Or crmSubScriptionCls.crmCommercial_Flag = True Or crmSubScriptionCls.crmBusiness_Flag = True Or crmSubScriptionCls.crmYacht_Flag = True) Then

                If Is_Operator_Flag = False Then ' if it is not an operator, then run the model

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------

                    sSelectionClause &= "AND amod_customer_flag = 'Y' "


                    sSelectionClause &= cAndClause
                    sSelectionClause &= cSingleOpen



                    If crmSubScriptionCls.crmBusiness_Flag = True Then
                        sSelectionClause &= "( amod_product_business_flag = 'Y'"

                        If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
                        ElseIf crmSubScriptionCls.crmJets_Flag = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
                        ElseIf crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
                        Else
                            sSelectionClause &= ")"
                        End If
                    End If

                    If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= "( amod_product_commercial_flag = 'Y')"


                        'If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
                        '    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
                        'ElseIf crmSubScriptionCls.crmJets_Flag = True Then
                        '    sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
                        'ElseIf crmSubScriptionCls.crmTurboprops = True Then
                        '    sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
                        'Else
                        '    sSelectionClause &= ")"
                        'End If
                    End If


                    If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If


                    If crmSubScriptionCls.crmHelicopter_Flag = True Then
                        ' sSelectionClause &= "(amod_type_code IN ('T','P') and amod_product_helicopter_flag = 'Y')"
                        sSelectionClause &= "(amod_product_helicopter_flag = 'Y')"
                    End If



                    sSelectionClause &= cSingleClose

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------




                    If Is_Aircraft_Flag = True Then ' if it is aircraf, then run aircraft 

                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        sSelectionClause &= cAndClause
                        sSelectionClause &= cSingleOpen


                        If crmSubScriptionCls.crmBusiness_Flag = True Then
                            sSelectionClause &= " ac_product_business_flag = 'Y' "
                        End If

                        If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                            sSelectionClause &= cOrClause
                        End If

                        If crmSubScriptionCls.crmCommercial_Flag = True Then
                            sSelectionClause &= " ac_product_commercial_flag = 'Y' "
                        End If

                        If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                            sSelectionClause &= cOrClause
                        End If

                        If crmSubScriptionCls.crmHelicopter_Flag = True Then
                            sSelectionClause &= " ac_product_helicopter_flag = 'Y'"
                        End If

                        sSelectionClause &= cSingleClose


                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                    End If

                ElseIf Is_Operator_Flag = True Then  ' if operator is true, then run it
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------

                    sSelectionClause &= cAndClause
                    sSelectionClause &= cSingleOpen


                    If crmSubScriptionCls.crmBusiness_Flag = True Then
                        sSelectionClause &= " comp_product_business_flag = 'Y' "
                    End If

                    If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= " comp_product_commercial_flag = 'Y' "
                    End If

                    If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmHelicopter_Flag = True Then
                        sSelectionClause &= " comp_product_helicopter_flag = 'Y'"
                    End If

                    '  If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then 'only crm'

                    If (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Or (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmHelicopter_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmYacht_Flag = True Then
                        sSelectionClause &= " comp_product_yacht_flag = 'Y'"
                    End If

                    'End If

                    sSelectionClause &= cSingleClose

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                End If
            End If

            Return sSelectionClause.Trim

        End Function

        Public Shared Function GenerateProductCodeSelectionQuery_CRM(ByRef crmSubScriptionCls As crmSubscriptionClass, ByVal Is_Operator_Flag As Boolean, ByVal Is_Aircraft_Flag As Boolean)
            '------------------------------------------ 
            ' Function: GenerateProductCodeSelectionQuery_CRM
            '
            ' This function takes in the local subscription class. 
            'If then names that crm subscription class and references it that way
            '
            '
            '
            ' This function take in two flags. One for Operator and one for Aircraft.
            ' Currently there is no way to do both a model/aircraft selection and an operator selection.
            '
            ' If Operator flag is true, then it runs the operator section query
            ' If Operator Flag is flase, it runs the model selection. It then checks the aircraft flag.
            ' If the aircraft flag is true, and the operator flag is false it will then run model and aircraft.

            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False) - This would run Just Model Selection Code 
            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True) -  This would run The Model Selection and the Aircraft Selection Code
            '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), True, False) -  This would run Just the Operator Selection Code 
            '
            '
            '------------------------------------------
            Dim sSelectionClause As String = " "
            Dim nloop As Integer = 0
            Dim bSingleProduct As Boolean = True
            Dim string_for_type As String = ""
            Dim cAndClause As String = " AND "
            Dim cOrClause As String = " OR "
            Dim cSingleOpen As String = "("
            Dim cSingleClose As String = ")"


            'This has to be added.
            'In case a session is dropped, if this isn't there, it will cause an sql error
            'added 4-27-2012
            If (crmSubScriptionCls.crmHelicopter_Flag = True Or crmSubScriptionCls.crmCommercial_Flag = True Or crmSubScriptionCls.crmBusiness_Flag = True) Then

                If Is_Operator_Flag = False Then ' if it is not an operator, then run the model

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------

                    sSelectionClause &= "AND amod_customer_flag = 'Y' "


                    sSelectionClause &= cAndClause
                    sSelectionClause &= cSingleOpen



                    If crmSubScriptionCls.crmBusiness_Flag = True Then
                        sSelectionClause &= "( amod_product_business_flag = 'Y'"

                        If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
                        ElseIf crmSubScriptionCls.crmJets_Flag = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
                        ElseIf crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
                        Else
                            sSelectionClause &= ")"
                        End If
                    End If

                    If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= "( amod_product_commercial_flag = 'Y'"

                        ' UN COMMENTED THIS SECTION - 11/8/18 after consulting RTW --- MSW - so that there was no dataleak in a company spec/ac display
                        If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
                        ElseIf crmSubScriptionCls.crmJets_Flag = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
                        ElseIf crmSubScriptionCls.crmTurboprops = True Then
                            sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
                        Else
                            sSelectionClause &= ")"
                        End If
                    End If


                    If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If


                    If crmSubScriptionCls.crmHelicopter_Flag = True Then
                        ' sSelectionClause &= "(amod_type_code IN ('T','P') and amod_product_helicopter_flag = 'Y')"
                        sSelectionClause &= "(amod_product_helicopter_flag = 'Y')"
                    End If



                    sSelectionClause &= cSingleClose

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------




                    If Is_Aircraft_Flag = True Then ' if it is aircraf, then run aircraft 

                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        sSelectionClause &= cAndClause
                        sSelectionClause &= cSingleOpen


                        If crmSubScriptionCls.crmBusiness_Flag = True Then
                            sSelectionClause &= " ac_product_business_flag = 'Y' "
                        End If

                        If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                            sSelectionClause &= cOrClause
                        End If

                        If crmSubScriptionCls.crmCommercial_Flag = True Then
                            sSelectionClause &= " ac_product_commercial_flag = 'Y' "
                        End If

                        If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                            sSelectionClause &= cOrClause
                        End If

                        If crmSubScriptionCls.crmHelicopter_Flag = True Then
                            sSelectionClause &= " ac_product_helicopter_flag = 'Y'"
                        End If

                        sSelectionClause &= cSingleClose


                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                        '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
                    End If

                ElseIf Is_Operator_Flag = True Then  ' if operator is true, then run it
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                    sSelectionClause &= "AND comp_active_flag = 'Y' "

                    sSelectionClause &= cAndClause
                    sSelectionClause &= cSingleOpen


                    If crmSubScriptionCls.crmBusiness_Flag = True Then
                        sSelectionClause &= " comp_product_business_flag = 'Y' "
                    End If

                    If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmCommercial_Flag = True Then
                        sSelectionClause &= " comp_product_commercial_flag = 'Y' "
                    End If

                    If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmHelicopter_Flag = True Then
                        sSelectionClause &= " comp_product_helicopter_flag = 'Y'"
                    End If

                    ' If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then 'only crm'

                    If (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Or (crmSubScriptionCls.crmYacht_Flag = True And crmSubScriptionCls.crmHelicopter_Flag = True) Then
                        sSelectionClause &= cOrClause
                    End If

                    If crmSubScriptionCls.crmYacht_Flag = True Then
                        sSelectionClause &= " comp_product_yacht_flag = 'Y'"
                    End If

                    'End If

                    sSelectionClause &= cSingleClose

                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                    '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
                End If
            End If

            Return sSelectionClause.Trim

        End Function
        'Public Shared Function GenerateProductCodeSelectionQuery_CRM(ByRef crmSubScriptionCls As crmSubscriptionClass, ByVal Is_Operator_Flag As Boolean, ByVal Is_Aircraft_Flag As Boolean)
        '    '------------------------------------------
        '    ' Function: GenerateProductCodeSelectionQuery_CRM
        '    '
        '    ' This function takes in the local subscription class. 
        '    'If then names that crm subscription class and references it that way
        '    '
        '    '
        '    '
        '    ' This function take in two flags. One for Operator and one for Aircraft.
        '    ' Currently there is no way to do both a model/aircraft selection and an operator selection.
        '    '
        '    ' If Operator flag is true, then it runs the operator section query
        '    ' If Operator Flag is flase, it runs the model selection. It then checks the aircraft flag.
        '    ' If the aircraft flag is true, and the operator flag is false it will then run model and aircraft.
        '    '
        '    '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False) - This would run Just Model Selection Code 
        '    '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, True) -  This would run The Model Selection and the Aircraft Selection Code
        '    '  GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), True, False) -  This would run Just the Operator Selection Code 
        '    '
        '    '
        '    '------------------------------------------
        '    Dim sSelectionClause As String = ""
        '    Dim nloop As Integer = 0
        '    Dim bSingleProduct As Boolean = True
        '    Dim string_for_type As String = ""
        '    Dim cAndClause As String = " AND "
        '    Dim cOrClause As String = " OR "
        '    Dim cSingleOpen As String = "("
        '    Dim cSingleClose As String = ")"




        '    If Is_Operator_Flag = False Then ' if it is not an operator, then run the model

        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------

        '        sSelectionClause &= "AND amod_customer_flag = 'Y' "

        '        sSelectionClause &= cAndClause

        '        sSelectionClause &= cSingleOpen

        '        If crmSubScriptionCls.crmBusiness_Flag = True Then
        '            sSelectionClause &= "( amod_product_business_flag = 'Y'"

        '            If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
        '            ElseIf crmSubScriptionCls.crmJets_Flag = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
        '            ElseIf crmSubScriptionCls.crmTurboprops = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
        '            Else
        '                sSelectionClause &= ")"
        '            End If
        '        End If

        '        If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
        '            sSelectionClause &= cOrClause
        '        End If

        '        If crmSubScriptionCls.crmCommercial_Flag = True Then
        '            sSelectionClause &= "( amod_product_commercial_flag = 'Y'"


        '            If crmSubScriptionCls.crmJets_Flag = True And crmSubScriptionCls.crmTurboprops = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('J','E', 'T','P'))"
        '            ElseIf crmSubScriptionCls.crmJets_Flag = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('J','E'))"
        '            ElseIf crmSubScriptionCls.crmTurboprops = True Then
        '                sSelectionClause &= cAndClause & "amod_type_code IN ('T','P'))"
        '            Else
        '                sSelectionClause &= ")"
        '            End If
        '        End If


        '        If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
        '            sSelectionClause &= cOrClause
        '        End If


        '        If crmSubScriptionCls.crmHelicopter_Flag = True Then
        '            sSelectionClause &= "(amod_type_code IN ('T','P') and amod_product_helicopter_flag = 'Y')"
        '        End If




        '        sSelectionClause &= cSingleClose

        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE MODEL SELECTION---------------------------------------------------------




        '        If Is_Aircraft_Flag = True Then ' if it is aircraf, then run aircraft 

        '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
        '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
        '            sSelectionClause &= cAndClause

        '            sSelectionClause &= cSingleOpen

        '            If crmSubScriptionCls.crmBusiness_Flag = True Then
        '                sSelectionClause &= " ac_product_business_flag = 'Y' "
        '            End If

        '            If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
        '                sSelectionClause &= cOrClause
        '            End If

        '            If crmSubScriptionCls.crmCommercial_Flag = True Then
        '                sSelectionClause &= " ac_product_commercial_flag = 'Y' "
        '            End If

        '            If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
        '                sSelectionClause &= cOrClause
        '            End If

        '            If crmSubScriptionCls.crmHelicopter_Flag = True Then
        '                sSelectionClause &= " ac_product_helicopter_flag = 'Y'"
        '            End If

        '            sSelectionClause &= cSingleClose

        '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
        '            '-------------------------- THIS IS THE SECTION OF CODE FOR THE AIRCRAFT SELECTION---------------------------------------------------------
        '        End If

        '    ElseIf Is_Operator_Flag = True Then  ' if operator is true, then run it
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
        '        sSelectionClause &= "AND comp_active_flag = 'Y' "

        '        sSelectionClause &= cAndClause

        '        sSelectionClause &= cSingleOpen

        '        If crmSubScriptionCls.crmBusiness_Flag = True Then
        '            sSelectionClause &= " comp_product_business_flag = 'Y' "
        '        End If

        '        If crmSubScriptionCls.crmBusiness_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True Then
        '            sSelectionClause &= cOrClause
        '        End If

        '        If crmSubScriptionCls.crmCommercial_Flag = True Then
        '            sSelectionClause &= " comp_product_commercial_flag = 'Y' "
        '        End If

        '        If (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmCommercial_Flag = True) Or (crmSubScriptionCls.crmHelicopter_Flag = True And crmSubScriptionCls.crmBusiness_Flag = True) Then
        '            sSelectionClause &= cOrClause
        '        End If

        '        If crmSubScriptionCls.crmHelicopter_Flag = True Then
        '            sSelectionClause &= " comp_product_helicopter_flag = 'Y'"
        '        End If

        '        sSelectionClause &= cSingleClose
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
        '        '-------------------------- THIS IS THE SECTION OF CODE FOR THE OPERATOR ---------------------------------------------------------
        '    End If

        '    Return sSelectionClause.Trim

        'End Function

        Public Shared Sub Display_Jetnet_Aircraft_Label(ByVal aircraft_info As Label, ByVal aircraft_Data As clsClient_Aircraft, ByVal ac_ID As Integer, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal jetnet_mod As TextBox)
            Dim aTempTable As New DataTable
            Dim Aircraft_Model As String = ""
            aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(ac_ID, "")
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    jetnet_mod.Text = aTempTable.Rows(0).Item("ac_amod_id")
                    Aircraft_Model = (aTempTable.Rows(0).Item("amod_make_name") & " " & aTempTable.Rows(0).Item("amod_model_name"))
                    aircraft_Data = Create_Aircraft_Class(aTempTable, "ac")
                    aircraft_Data.cliaircraft_id = aTempTable.Rows(0).Item("ac_id")
                    aircraft_info.Text = Aircraft_Model & "<br />" & Build_Aircraft_Display(aircraft_Data, True, False, False)
                End If
            End If
        End Sub


        Public Shared Sub SendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String)

            ' Instantiate a new instance of MailMessage
            Dim mMailMessage As New MailMessage()

            ' Set the sender address of the mail message
            mMailMessage.From = New MailAddress(from)
            ' Set the recepient address of the mail message
            mMailMessage.To.Add(New MailAddress(recepient))

            ' Check if the bcc value is nothing or an empty string
            If Not bcc Is Nothing And bcc <> String.Empty Then
                ' Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(New MailAddress(bcc))
            End If

            ' Check if the cc value is nothing or an empty value
            If Not cc Is Nothing And cc <> String.Empty Then
                ' Set the CC address of the mail message
                mMailMessage.CC.Add(New MailAddress(cc))
            End If

            ' Set the subject of the mail message
            mMailMessage.Subject = subject
            ' Set the body of the mail message
            mMailMessage.Body = body

            ' Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = True
            ' Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal

            ' Instantiate a new instance of SmtpClient
            Dim mSmtpClient As New SmtpClient("localhost", 25)
            ' Send the mail message
            mSmtpClient.Send(mMailMessage)


        End Sub

#Region "SMS Event Listbox"
        Public Shared Sub Fill_SMS_Events(ByVal myListBoxControl As ListBox, ByVal subscriberEvents As String)
            Dim tmpEventArr() As String = Nothing
            myListBoxControl.Items.Clear()
            tmpEventArr = subscriberEvents.Split(Constants.cCommaDelim) 'splitting the events if they exist.
            myListBoxControl.Items.Add(New ListItem("Newly Available", "MA"))
            myListBoxControl.Items.Add(New ListItem("Change in Asking Price", "CA"))
            myListBoxControl.Items.Add(New ListItem("Off Market", "OM"))
            myListBoxControl.Items.Add(New ListItem("Off Market Due To Sale", "OMNS"))

            For x = 0 To UBound(tmpEventArr)
                For j As Integer = 0 To myListBoxControl.Items.Count() - 1
                    If UCase(myListBoxControl.Items(j).Value) = UCase(tmpEventArr(x)) Then
                        myListBoxControl.Items(j).Selected = True
                    End If
                Next
            Next


        End Sub
#End Region

#Region "My Models Listbox moving"
        Public Shared Sub AddBtn_Click(ByVal selected_models As ListBox, ByVal market_pref_models As ListBox)
            Dim lasset As New ArrayList()
            Dim lsubordinate As New ArrayList()
            If market_pref_models.SelectedIndex >= 0 Then
                Dim i As Integer
                For i = 0 To market_pref_models.Items.Count - 1
                    If market_pref_models.Items(i).Selected Then
                        If Not lasset.Contains(market_pref_models.Items(i)) Then
                            lasset.Add(market_pref_models.Items(i))
                        End If
                    End If
                Next i
                Dim fiel As New ListItem
                For i = 0 To lasset.Count - 1
                    If Not selected_models.Items.Contains(CType(lasset(i), ListItem)) Then
                        selected_models.Items.Add(CType(lasset(i), ListItem))
                        fiel = CType(lasset(i), ListItem)
                    End If
                    market_pref_models.Items.Remove(CType(lasset(i), ListItem))
                Next i
            End If
        End Sub

        Public Shared Sub AddAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs, ByVal selected_models As ListBox, ByVal market_pref_models As ListBox)
            Dim lasset As New ArrayList()
            Dim lsubordinate As New ArrayList()
            While market_pref_models.Items.Count <> 0
                Dim i As Integer
                For i = 0 To market_pref_models.Items.Count - 1
                    If Not lasset.Contains(market_pref_models.Items(i)) Then
                        lasset.Add(market_pref_models.Items(i))
                    End If
                Next i
                For i = 0 To lasset.Count - 1
                    If Not selected_models.Items.Contains(CType(lasset(i), ListItem)) Then
                        selected_models.Items.Add(CType(lasset(i), ListItem))
                    End If
                    market_pref_models.Items.Remove(CType(lasset(i), ListItem))
                Next i
            End While
        End Sub

        Public Shared Sub RemoveBtn_Click(ByVal Src As [Object], ByVal E As EventArgs, ByVal selected_models As ListBox, ByVal market_pref_models As ListBox)
            Dim lasset As New ArrayList()
            Dim lsubordinate As New ArrayList()
            If Not (selected_models.SelectedItem Is Nothing) Then
                Dim i As Integer
                For i = 0 To selected_models.Items.Count - 1
                    If selected_models.Items(i).Selected Then
                        If Not lsubordinate.Contains(selected_models.Items(i)) Then
                            lsubordinate.Add(selected_models.Items(i))
                        End If
                    End If
                Next i
                Dim fiel As New ListItem
                For i = 0 To lsubordinate.Count - 1
                    If Not market_pref_models.Items.Contains(CType(lsubordinate(i), ListItem)) Then
                        market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
                        fiel = CType(lsubordinate(i), ListItem)
                    End If
                    selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
                    fiel = CType(lsubordinate(i), ListItem)


                    lasset.Add(lsubordinate(i))
                    market_pref_models.SelectedValue = fiel.Value
                Next i
            End If
        End Sub

        Public Shared Sub RemoveAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs, ByVal selected_models As ListBox, ByVal market_pref_models As ListBox)
            Dim lasset As New ArrayList()
            Dim lsubordinate As New ArrayList()
            While selected_models.Items.Count <> 0
                Dim i As Integer
                For i = 0 To selected_models.Items.Count - 1
                    If Not lsubordinate.Contains(selected_models.Items(i)) Then
                        lsubordinate.Add(selected_models.Items(i))
                    End If
                Next i
                Dim fiel As New ListItem
                For i = 0 To lsubordinate.Count - 1
                    If Not market_pref_models.Items.Contains(CType(lsubordinate(i), ListItem)) Then
                        market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
                        fiel = CType(lsubordinate(i), ListItem)
                    End If
                    selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
                    lasset.Add(lsubordinate(i))
                Next i
            End While
        End Sub

#End Region
#Region "Evo Reload Subscription"
        Public Shared Function Reload_Evolution_Subscription(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal tempTable As DataTable) As Boolean
            Try
                Dim product_code As String = ""
                Dim ClientDatabaseInformation As New DataTable
                Dim client_dbhost As String = ""
                Dim client_dbDatabase As String = ""
                Dim client_dbUID As String = ""
                Dim client_dbPWD As String = ""


                If tempTable.Rows.Count > 0 Then
                    'Subscriber ID, Also defaulting CRM user ID to 1
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_sub_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUserID = 1
                        HttpContext.Current.Session.Item("localUser").crmSubSubID = tempTable.Rows(0).Item("sublogin_sub_id")
                    End If

                    'Subscriber Parent ID
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_parent_sub_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmSubParentID = tempTable.Rows(0).Item("sub_parent_sub_id")
                    End If

                    'Login:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_login")) Then
                        HttpContext.Current.Session.Item("localUser").crmUserLogin = tempTable.Rows(0).Item("subins_login").ToString
                    End If

                    'Seq #
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_seq_no")) Then
                        HttpContext.Current.Session.Item("localUser").crmSubSeqNo = tempTable.Rows(0).Item("subins_seq_no")
                    End If

                    'added 9/25/15
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_default_airports")) Then
                        HttpContext.Current.Session.Item("localUser").crmUserDefaultAirports = tempTable.Rows(0).Item("subins_default_airports")
                    End If

                    'Added 4/7/15
                    'A session variable on the Evolution side to name the files in the temporary folder.
                    HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString & "_" & HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim & "_" & HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString & "_"

                    'Platform OS:
                    If Not IsNothing(HttpContext.Current.Request.Item("whatBrowser")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("whatBrowser").ToString.Trim) Then
                            HttpContext.Current.Session.Item("localUser").crmPlatformOS = HttpContext.Current.Request.Item("whatBrowser").ToString.Trim
                        End If
                    End If

                    'don't show pictures
                    'If Not getUserDontShowPicCookies("DoNotShowPictures") Then
                    '    HttpContext.Current.Response.Cookies.Item("DoNotShowPictures").Item("") = "true"
                    '    HttpContext.Current.Response.Cookies.Item("DoNotShowPictures").Expires = DateTime.Now.AddDays(300)
                    '    HttpContext.Current.Session.Item("localUser").crmDontShowPics = True
                    'Else
                    '    HttpContext.Current.Response.Cookies.Item("DoNotShowPictures").Item("") = "false"
                    '    HttpContext.Current.Response.Cookies.Item("DoNotShowPictures").Expires = DateTime.Now.AddDays(300)
                    '    HttpContext.Current.Session.Item("localUser").crmDontShowPics = False
                    'End If



                    'Default Model ID:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_default_amod_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmUserSelectedModel = tempTable.Rows(0).Item("subins_default_amod_id")
                    End If

                    'Last install date
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_install_date")) Then
                        HttpContext.Current.Session.Item("localUser").crmSubInstallDate = tempTable.Rows(0).Item("subins_install_date")
                    End If

                    'Service Code:
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_serv_code")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmServiceCode = tempTable.Rows(0).Item("sub_serv_code").ToString
                    End If

                    'Default View:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_evoview_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmSelectedView = tempTable.Rows(0).Item("subins_evoview_id")
                    End If

                    'Background Image ID:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_background_image_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUser_Background_ID = tempTable.Rows(0).Item("subins_background_image_id")
                    End If

                    '# of Records:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_nbr_rec_per_page")) Then
                        If IsNumeric(tempTable.Rows(0).Item("subins_nbr_rec_per_page")) Then
                            If tempTable.Rows(0).Item("subins_nbr_rec_per_page") > 0 Then
                                'If for whatever reason, their record rows are less than 10,
                                'Since this system doesn't allow it, we move them up to 10.
                                If tempTable.Rows(0).Item("subins_nbr_rec_per_page") < 10 Then
                                    HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage = 10
                                Else
                                    HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage = tempTable.Rows(0).Item("subins_nbr_rec_per_page")
                                End If
                            End If
                        End If
                    End If

                    'Mobile Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_evo_mobile_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmMobileFlag = IIf(UCase(tempTable.Rows(0).Item("subins_evo_mobile_flag")) = "Y", True, False)
                    End If

                    'Marketing Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_marketing_flag")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmMarketingFlag = IIf(UCase(tempTable.Rows(0).Item("sub_marketing_flag")) = "Y", True, False)
                    End If

                    'Demo Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_demo_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = IIf(UCase(tempTable.Rows(0).Item("sublogin_demo_flag")) = "Y", True, False)
                        If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                            HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.GUEST
                        Else
                            HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.USER
                        End If
                    End If

                    'Administrator Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_admin_flag")) Then
                        If UCase(tempTable.Rows(0).Item("subins_admin_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR
                        End If
                    End If


                    'Mobile #:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_cell_number")) Then
                        HttpContext.Current.Session.Item("localUser").crmMobileNumber = tempTable.Rows(0).Item("subins_cell_number").ToString
                    End If

                    'Email Replyname:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_email_replyname")) Then
                        HttpContext.Current.Session.Item("localUser").crmEmailReplyname = tempTable.Rows(0).Item("subins_email_replyname").ToString
                    End If

                    'Email Replyname:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_email_replyaddress")) Then
                        HttpContext.Current.Session.Item("localUser").crmEmailReplyAddress = tempTable.Rows(0).Item("subins_email_replyaddress").ToString
                    End If

                    'Email default format:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_email_default_format")) Then
                        HttpContext.Current.Session.Item("localUser").crmEmailFormat = tempTable.Rows(0).Item("subins_email_default_format")
                    End If

                    'Cell Service:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_cell_service")) Then
                        HttpContext.Current.Session.Item("localUser").crmCellService = tempTable.Rows(0).Item("subins_cell_service").ToString
                    End If

                    'Cell Carrier ID:

                    If Not IsDBNull(tempTable.Rows(0).Item("subins_cell_carrier_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmCellCarrierID = tempTable.Rows(0).Item("subins_cell_carrier_id")
                    End If

                    'SMS Events:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_sms_events")) Then
                        HttpContext.Current.Session.Item("localUser").crmCellEvents = tempTable.Rows(0).Item("subins_sms_events").ToString
                    End If

                    'SMS Models:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_smstxt_models")) Then
                        HttpContext.Current.Session.Item("localUser").crmSMSSelectedModels = tempTable.Rows(0).Item("subins_smstxt_models")
                    End If

                    'SMS Status:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_smstxt_active_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmSMSStatus = tempTable.Rows(0).Item("subins_smstxt_active_flag").ToString
                    End If

                    'Local Notes DB:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_local_db_file")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalDbFile = tempTable.Rows(0).Item("subins_local_db_file").ToString
                    End If

                    'Local Notes:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_local_db_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalSideNotes_Flag = IIf(UCase(tempTable.Rows(0).Item("subins_local_db_flag")) = "Y", True, False)
                    End If

                    'Enable Notes:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_allow_local_notes_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmEnableNotes = IIf(UCase(tempTable.Rows(0).Item("sublogin_allow_local_notes_flag")) = "Y", True, False)
                    End If

                    'Active X:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_activex_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmActiveX = IIf(UCase(tempTable.Rows(0).Item("subins_activex_flag")) = "Y", True, False)
                    End If

                    'Display Note Tag on AC:
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_display_note_tag_on_aclist_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmDisplayNoteTag = IIf(UCase(tempTable.Rows(0).Item("subins_display_note_tag_on_aclist_flag")) = "Y", True, False)
                    End If

                    ''user
                    'Allow Export Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_allow_export_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmAllowExport_Flag = IIf(UCase(tempTable.Rows(0).Item("sublogin_allow_export_flag")) = "Y", True, False)
                    End If

                    'Allow Projects Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_allow_projects_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmAllowProjects_Flag = IIf(UCase(tempTable.Rows(0).Item("sublogin_allow_projects_flag")) = "Y", True, False)
                    End If

                    'Email Request Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_allow_email_request_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmAllowEmailRequest = IIf(UCase(tempTable.Rows(0).Item("sublogin_allow_email_request_flag")) = "Y", True, False)
                    End If

                    'Allow Text Message Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_allow_text_message_flag")) Then
                        HttpContext.Current.Session.Item("localUser").crmAllowTextMessage = IIf(UCase(tempTable.Rows(0).Item("sublogin_allow_text_message_flag")) = "Y", True, False)
                    End If


                    'Aerodex Flag:
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_aerodex_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_aerodex_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False
                        End If
                    End If

                    'Last Login
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_last_login_date")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_login_date = tempTable.Rows(0).Item("subins_last_login_date")
                    End If

                    'Last Logout
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_last_logout_date")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_logout_date = tempTable.Rows(0).Item("subins_last_logout_date")
                    End If

                    'Last Session
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_last_session_date")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_session_date = tempTable.Rows(0).Item("subins_last_session_date")
                    End If

                    'Star Reports
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_starreports_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_starreports_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmStar_Reports_Flag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmStar_Reports_Flag = False
                        End If
                    End If

                    'Business Flag?
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_business_aircraft_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_business_aircraft_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True
                            product_code = "B,"
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False
                        End If
                    End If

                    'Business Tier
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_busair_tier_level")) Then
                        If tempTable.Rows(0).Item("sub_busair_tier_level") = "1" Then
                            HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "J"
                            HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True
                        ElseIf tempTable.Rows(0).Item("sub_busair_tier_level") = "2" Then
                            HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "T"
                            HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True
                        ElseIf tempTable.Rows(0).Item("sub_busair_tier_level") = "3" Then
                            HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "ALL"
                            HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = False
                            HttpContext.Current.Session.Item("localSubscription").crmTurboprops = False
                            HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag = False
                        End If
                    End If


                    'sub_yacht_flag='Y'
                    'Yacht Flag
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_yacht_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_yacht_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag = True
                            ' product_code = "Y,"
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag = False
                        End If
                    End If

                    'Helicopter Flag?
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_helicopters_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_helicopters_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True
                            product_code = product_code & "H,"
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False
                        End If
                    End If
                    'Commercial Flag
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_commerical_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_commerical_flag")) = "Y" Then
                            product_code = product_code & "C,"
                            HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False
                        End If
                    End If

                    product_code = product_code.Trim(",")
                    'Just a handy way to store all product codes.
                    HttpContext.Current.Session.Item("localSubscription").crmProductCode = product_code

                    'SPI Flag
                    If Not IsDBNull(tempTable.Rows(0).Item("sublogin_values_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sublogin_values_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = False
                        End If
                    End If

                    'Cloud Notes +?
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_server_side_notes_flag")) Then
                        If UCase(tempTable.Rows(0).Item("sub_server_side_notes_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = False
                        End If
                    End If

                    'Cloud Notes + DB?
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_server_side_dbase_name")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmServerSideDBName = tempTable.Rows(0).Item("sub_server_side_dbase_name")
                    End If
                    'Cloud Notes + Reg #
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_server_side_crm_regid")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmRegID = tempTable.Rows(0).Item("sub_server_side_crm_regid")
                    End If

                    'Cloud Notes?
                    If Not IsDBNull(tempTable.Rows(0).Item("Sub_cloud_notes_flag")) Then
                        If UCase(tempTable.Rows(0).Item("Sub_cloud_notes_flag")) = "Y" Then
                            HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True
                        Else
                            HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = False
                        End If
                    End If

                    'Share Type
                    Dim ShareByCompFlag As String = "N"
                    Dim ShareByParentFlag As String = "N"
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_share_by_comp_id_flag")) Then
                        ShareByCompFlag = tempTable.Rows(0).Item("sub_share_by_comp_id_flag")
                    End If
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_share_by_parent_sub_id_flag")) Then
                        ShareByParentFlag = tempTable.Rows(0).Item("sub_share_by_parent_sub_id_flag")
                    End If

                    If ShareByCompFlag = "N" And ShareByParentFlag = "N" Then
                        '--    A. IF SUB_BY_COMP_ID_FLAG='N' AND SUB_SHARE_BY_PARENT_SUB_ID_FLAG='N' THEN SHARE STATUS = MYSUBSCRIPTION
                        HttpContext.Current.Session.Item("localSubscription").crmSubscriptionShareType = eSubscriptionShareType.MY_SUBSCRIPTION
                    ElseIf ShareByParentFlag = "Y" Then
                        '--    B. IF SUB_SHARE_BY_PARENT_SUB_ID_FLAG='Y' THEN SHARE STATUS = MYPARENTSUBSCRIPTION
                        HttpContext.Current.Session.Item("localSubscription").crmSubscriptionShareType = eSubscriptionShareType.MY_PARENT_SUBSCRIPTION
                    ElseIf ShareByCompFlag = "Y" Then
                        '--    C. IF SUB_BY_COMP_ID_FLAG='Y' THEN SHARE STATUS = MYPARENTCOMPANY
                        HttpContext.Current.Session.Item("localSubscription").crmSubscriptionShareType = eSubscriptionShareType.MY_PARENT_COMPANY
                    End If

                    'Cloud Notes DB?
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_cloud_notes_database")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmCloudNotesDBName = tempTable.Rows(0).Item("sub_cloud_notes_database")
                    End If

                    'Company ID
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_comp_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmUserCompanyID = tempTable.Rows(0).Item("sub_comp_id")
                    End If
                    'Contact ID
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_contact_id")) Then
                        HttpContext.Current.Session.Item("localUser").crmUserContactID = tempTable.Rows(0).Item("subins_contact_id")
                    End If
                    'Frequency
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_frequency")) Then
                        HttpContext.Current.Session.Item("localSubscription").crmFrequency = tempTable.Rows(0).Item("sub_frequency")
                    End If
                    'Contact First Name
                    If Not IsDBNull(tempTable.Rows(0).Item("contact_first_name")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName = tempTable.Rows(0).Item("contact_first_name")
                    End If
                    'Contact Last Name
                    If Not IsDBNull(tempTable.Rows(0).Item("contact_last_name")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUserLastName = tempTable.Rows(0).Item("contact_last_name")
                    End If
                    'Email Address
                    If Not IsDBNull(tempTable.Rows(0).Item("contact_email_address")) Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress = tempTable.Rows(0).Item("contact_email_address")
                    End If
                    'Documents Flag
                    HttpContext.Current.Session.Item("localSubscription").crmDocumentsFlag = False

                    'SubscriptionInstallModels = aclsData_Temp.EvoSubscription_GetSubscription_Install_Models(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
                    'If Not IsNothing(SubscriptionInstallModels) Then
                    '    If SubscriptionInstallModels.Rows.Count > 0 Then
                    '        For Each r As DataRow In SubscriptionInstallModels.Rows
                    '            HttpContext.Current.Session.Item("localUser").crmSelectedModels += r("sim_amod_id").ToString & ","
                    '        Next
                    '        HttpContext.Current.Session.Item("localUser").crmSelectedModels = HttpContext.Current.Session.Item("localUser").crmSelectedModels.TrimEnd(",")
                    '    End If
                    'End If
                    'Preferred model list
                    If Not IsDBNull(tempTable.Rows(0).Item("subins_default_models")) Then
                        HttpContext.Current.Session.Item("localUser").crmSelectedModels = tempTable.Rows(0).Item("subins_default_models").ToString
                    End If

                    'Let's set the session variable for the aircraft listing/gallery datagrid
                    Dim _ListingView As HttpCookie = HttpContext.Current.Request.Cookies("ACListingView")
                    If _ListingView IsNot Nothing Then
                        If _ListingView("USER") = HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString Then
                            Dim x As String = _ListingView("VIEW")
                            HttpContext.Current.Session.Item("localUser").crmACListingView = _ListingView("VIEW")
                        Else
                            HttpContext.Current.Session.Item("localUser").crmACListingView = eListingView.GALLERY
                        End If
                    Else
                        HttpContext.Current.Session.Item("localUser").crmACListingView = eListingView.GALLERY
                    End If

                    'Let's set the session variable for the Company listing/gallery datagrid
                    Dim _CompListingView As HttpCookie = HttpContext.Current.Request.Cookies("CompanyListingView")
                    If _CompListingView IsNot Nothing Then
                        If _CompListingView("USER") = HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString Then
                            Dim x As String = _CompListingView("VIEW")
                            HttpContext.Current.Session.Item("localUser").crmCompanyListingView = _CompListingView("VIEW")
                        Else
                            HttpContext.Current.Session.Item("localUser").crmCompanyListingView = eListingView.LISTING
                        End If
                    Else
                        HttpContext.Current.Session.Item("localUser").crmCompanyListingView = eListingView.LISTING
                    End If

                    'We're going to default this to 2000.
                    HttpContext.Current.Session.Item("localUser").crmMaxClientExport = 2000
                    If Not IsDBNull(tempTable.Rows(0).Item("sub_max_allowed_custom_export")) Then
                        If tempTable.Rows(0).Item("sub_max_allowed_custom_export") > 0 Then
                            HttpContext.Current.Session.Item("localUser").crmMaxClientExport = tempTable.Rows(0).Item("sub_max_allowed_custom_export")
                        End If
                    End If

                    If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                        If HttpContext.Current.Session.Item("localUser").crmUser_CRM_Database_Not_Available = False Then
                            If HttpContext.Current.Session.Item("localSubscription").crmRegID <> 0 Then
                                ClientDatabaseInformation = aclsData_Temp.Get_Server_Notes_DBInfo_Evo_Side(HttpContext.Current.Session.Item("localSubscription").crmRegID)
                                If Not IsNothing(ClientDatabaseInformation) Then
                                    If ClientDatabaseInformation.Rows.Count > 0 Then
                                        If Not IsDBNull(ClientDatabaseInformation.Rows(0).Item("client_dbhost")) Then
                                            If (HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL) Or (HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNET12")) Then
                                                'Rick: one approach is to read the IP from the database and if it says localhost or 172.30.5.38 then point to 192.69.4.159 and if it says 172.30.5.47 then point to 192.69.4.165 - Skype - 5/27/2015
                                                If ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString = "172.30.5.38" Or ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString = "localhost" Then
                                                    client_dbhost = "192.69.4.159"
                                                ElseIf ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString = "172.30.5.47" Then
                                                    client_dbhost = "192.69.4.165"
                                                Else
                                                    client_dbhost = ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString
                                                End If

                                            Else
                                                If ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString = "localhost" Then
                                                    client_dbhost = "172.30.5.38"
                                                Else
                                                    client_dbhost = ClientDatabaseInformation.Rows(0).Item("client_dbhost").ToString
                                                End If
                                            End If

                                        End If
                                        If Not IsDBNull(ClientDatabaseInformation.Rows(0).Item("client_dbDatabase")) Then
                                            client_dbDatabase = ClientDatabaseInformation.Rows(0).Item("client_dbDatabase")
                                        End If
                                        If Not IsDBNull(ClientDatabaseInformation.Rows(0).Item("client_dbUID")) Then
                                            client_dbUID = ClientDatabaseInformation.Rows(0).Item("client_dbUID")
                                        End If
                                        If Not IsDBNull(ClientDatabaseInformation.Rows(0).Item("client_dbPWD")) Then
                                            client_dbPWD = ClientDatabaseInformation.Rows(0).Item("client_dbPWD")
                                        End If

                                        HttpContext.Current.Application.Item("crmJetnetServerNotes") = "Connect Timeout=45;Allow User Variables=True;Default Command Timeout=3600;Persist Security Info=True;server=" + client_dbhost + ";User Id=" + client_dbUID + ";password=" + client_dbPWD + ";database=" + client_dbDatabase
                                        HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = HttpContext.Current.Application.Item("crmJetnetServerNotes")
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region
#Region "Recent Cookies Functions"
        '''
        ''' <summary>
        ''' This function is meant to combine both the EVO cookies AC store and the CRM AC store. CRM is on details.aspx.vb starts at line 226.
        ''' </summary>
        ''' <param name="ac_ID"></param>
        ''' <param name="source"></param>
        ''' <remarks></remarks>
        ''' ''''''''''''''
        Public Shared Sub Recent_Cookies(ByVal CookieName As String, ByVal ac_ID As Long, ByVal source As String)
            Dim _aircraftCookies As HttpCookie = HttpContext.Current.Request.Cookies(CookieName)
            Dim stored_id As String = ""
            Dim stored_source As String = ""
            Dim AmountNumber As Integer = 4
            Dim UserID As String = HttpContext.Current.Session.Item("localUser").crmLocalUserID.ToString

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                AmountNumber = 9
                UserID = HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString

            End If

            If _aircraftCookies IsNot Nothing Then
                If _aircraftCookies("USER") = UserID Then
                    stored_id = _aircraftCookies("ID")
                    stored_source = _aircraftCookies("SOURCE")

                    'Let's do one thing at a time. First we need to only store 5 companies. 
                    'Also no duplicates.. 
                    Dim id_array As Array = Split(stored_id, "|")
                    Dim source_array As Array = Split(stored_source, "|")

                    Dim exists As Integer = InStr(stored_id, CStr(ac_ID))

                    If UBound(id_array) < AmountNumber Then

                        If exists = 0 Then
                            HttpContext.Current.Response.Cookies(CookieName).Values("ID") = ac_ID & "|" & stored_id
                            HttpContext.Current.Response.Cookies(CookieName).Values("SOURCE") = source & "|" & stored_source
                            HttpContext.Current.Response.Cookies(CookieName).Values("USER") = UserID
                            HttpContext.Current.Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(10)
                        Else
                            Dim topnumber As Integer = UBound(id_array)
                            stored_id = ""
                            stored_source = ""

                            For i As Integer = 0 To topnumber
                                If id_array(i) <> CStr(ac_ID) Then
                                    stored_id = stored_id & id_array(i) & "|"
                                    stored_source = stored_source & source_array(i) & "|"
                                End If
                            Next


                            If stored_id <> "" Then
                                stored_id = UCase(stored_id.TrimEnd("|"))
                            End If

                            If stored_source <> "" Then
                                stored_source = UCase(stored_source.TrimEnd("|"))
                            End If

                            HttpContext.Current.Response.Cookies(CookieName).Values("ID") = ac_ID & "|" & stored_id
                            HttpContext.Current.Response.Cookies(CookieName).Values("SOURCE") = source & "|" & stored_source
                            HttpContext.Current.Response.Cookies(CookieName).Values("USER") = UserID
                            HttpContext.Current.Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(10)
                        End If

                    Else
                        'Store the ubound of the array.
                        Dim topnumber As Integer = UBound(id_array)
                        'rewrite the cookie with the last 5 in array.
                        stored_id = ""
                        stored_source = ""
                        If exists = 0 Then
                            For x As Integer = AmountNumber To 1 Step -1
                                stored_id += id_array(topnumber - x) & "|"
                                stored_source += source_array(topnumber - x) & "|"
                            Next
                            stored_id = stored_id.TrimEnd("|")
                            stored_source = stored_source.TrimEnd("|")
                            'stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1)
                            'stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1)
                        Else
                            For x As Integer = AmountNumber To 0 Step -1
                                stored_id = id_array(topnumber - x) & "|"
                                stored_source = source_array(topnumber - x) & "|"
                            Next
                            stored_id += stored_id.TrimEnd("|")
                            stored_source += stored_source.TrimEnd("|")
                            'stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1) & "|" & id_array(topnumber)
                            'stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1) & "|" & source_array(topnumber)

                        End If


                        id_array = Split(stored_id, "|")
                        source_array = Split(stored_source, "|")
                        topnumber = UBound(id_array)
                        stored_id = ""
                        stored_source = ""

                        For i As Integer = 0 To topnumber
                            If id_array(i) <> CStr(ac_ID) Then
                                stored_id = stored_id & id_array(i) & "|"
                                stored_source = stored_source & source_array(i) & "|"
                            End If
                        Next

                        If stored_id <> "" Then
                            stored_id = UCase(stored_id.TrimEnd("|"))
                        End If

                        If stored_source <> "" Then
                            stored_source = UCase(stored_source.TrimEnd("|"))
                        End If

                        HttpContext.Current.Response.Cookies(CookieName).Values("ID") = ac_ID & "|" & stored_id
                        HttpContext.Current.Response.Cookies(CookieName).Values("SOURCE") = source & "|" & stored_source
                        HttpContext.Current.Response.Cookies(CookieName).Values("USER") = UserID
                        HttpContext.Current.Response.Cookies(CookieName).Expires = DateTime.Now.AddDays(10)

                    End If
                Else
                    CreateNonExistingCookie(CookieName, ac_ID, source, UserID)
                    'Dim aCookie As New HttpCookie(CookieName)
                    'aCookie.Values("ID") = ac_ID

                    'aCookie.Values("SOURCE") = source
                    'aCookie.Values("USER") = UserID
                    'aCookie.Expires = DateTime.Now.AddDays(10)
                    'HttpContext.Current.Response.Cookies.Add(aCookie)
                End If
            Else
                CreateNonExistingCookie(CookieName, ac_ID, source, UserID)
                'Dim aCookie As New HttpCookie(CookieName)
                'aCookie.Values("ID") = ac_ID

                'aCookie.Values("SOURCE") = source
                'aCookie.Values("USER") = UserID
                'aCookie.Expires = DateTime.Now.AddDays(10)
                'HttpContext.Current.Response.Cookies.Add(aCookie)
            End If
        End Sub

        Public Shared Sub CreateNonExistingCookie(ByVal cookieName As String, ByVal ac_ID As Long, ByVal source As String, ByVal userID As String)
            Dim aCookie As New HttpCookie(cookieName)
            aCookie.Values("ID") = ac_ID

            aCookie.Values("SOURCE") = source
            aCookie.Values("USER") = userID
            aCookie.Expires = DateTime.Now.AddDays(10)
            HttpContext.Current.Response.Cookies.Add(aCookie)
        End Sub
#End Region
#Region "Master Page Evo Side"
        ''' <summary>
        ''' Sets the page title for three evo masterpages
        ''' </summary>
        ''' <param name="page_title"></param>
        ''' <remarks></remarks>
        Public Shared Function Set_Page_Title(ByVal page_title As String) As String
            Set_Page_Title = page_title
            If page_title <> "" Then
                Set_Page_Title += " - "
            End If
            Select Case HttpContext.Current.Session.Item("jetnetAppVersion")
                Case Constants.ApplicationVariable.EVO
                    Set_Page_Title += " JETNET Evolution - "
                Case Constants.ApplicationVariable.CUSTOMER_CENTER
                    Set_Page_Title += " Evolution Admin - "
                Case Constants.ApplicationVariable.YACHT
                    Set_Page_Title += " Yacht Spot - "
                Case Else
                    Set_Page_Title += " JETNET CRM - "
            End Select
            Set_Page_Title += WeekdayName(Weekday(FormatDateTime(Now(), 2))) & ", " & MonthName(Month(FormatDateTime(Now(), 2))) & " " & Day(FormatDateTime(Now(), 2)) & ", " & Year(FormatDateTime(Now(), 2))


            Return Set_Page_Title
        End Function
#End Region
        ''' <summary>
        ''' Function that returns an event title if the event has occurred within a week. Data is recieved date|title from AircraftListing.aspx
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Show_Evo_Event_Ac_Listing(ByVal NormaleventText As Object, ByVal AerodexEventText As Object) As String
            Show_Evo_Event_Ac_Listing = ""
            NormaleventText = NormaleventText.ToString
            AerodexEventText = AerodexEventText.ToString

            Dim EventText As String = ""

            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Then
                EventText = AerodexEventText
            Else
                EventText = NormaleventText
            End If

            If InStr(EventText, "|") > 0 Then 'checking to make sure event is piped correctly
                Dim event_Information() As String = Split(EventText, "|") 'splitting into array
                If UBound(event_Information) = 1 Then 'making sure array is right size
                    If IsDate(event_Information(0)) Then 'making sure first array item is date
                        If DateDiff(DateInterval.Day, CDate(event_Information(0)), Now()) <= 7 Then 'making sure first array item is within 1 week of today
                            Show_Evo_Event_Ac_Listing = "<i class=""fa fa-lightbulb-o"" alt=""" & event_Information(0) & " " & event_Information(1) & """ title='" & event_Information(0) & " " & event_Information(1) & "'></i>"
                        End If
                    End If
                End If
            End If

        End Function
#Region "CRM ACTIVE FOLDERS" '
        ''' <summary>
        ''' This function is just some common code that's going to go ahead and grab the folder data from the aircraft/contact/company search box control.
        ''' </summary>
        ''' <param name="masterpage"></param>
        ''' <param name="FolderTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReturnCfolderData(ByRef masterpage As main_site, ByRef FolderTable As DataTable) As String
            Dim cFolderData As String = ""
            FolderTable = masterpage.aclsData_Temp.Get_Client_Folders_ByID(CInt(HttpContext.Current.Session.Item("localUser").crmLocalUserID), masterpage.SubNodeOfListing)
            If FolderTable.Rows.Count > 0 Then
                masterpage.NameOfSubnode = FolderTable.Rows(0).Item("cfolder_name").ToString

                If Not IsDBNull(FolderTable.Rows(0).Item("cfolder_data")) Then
                    If Not String.IsNullOrEmpty(FolderTable.Rows(0).Item("cfolder_data")) Then
                        cFolderData = Trim(FolderTable.Rows(0).Item("cfolder_data").ToString)
                    End If
                End If
            End If

            Return cFolderData
        End Function
#End Region
#Region "Encode/Decode"

        Public Shared Function EncodeBase64(ByVal input As String) As String
            Dim strBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(input)
            Return System.Convert.ToBase64String(strBytes)
        End Function

        ' Returns the input string decoded from base64
        Public Shared Function DecodeBase64(ByVal input As String) As String
            Dim strBytes() As Byte = System.Convert.FromBase64String(input)
            Return System.Text.Encoding.UTF8.GetChars(strBytes)
        End Function

#End Region

#Region "Outlook Functions"
        Public Shared Function Create_VCard(ByVal CompanyInformation As DataTable, ByVal CompanyPhoneInformation As DataTable, ByVal ContactPhoneInformation As DataTable, ByVal ContactInformation As DataTable) As Integer
            Try
                Dim writer As System.IO.StreamWriter

                writer = New System.IO.StreamWriter(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "\contact.vcf"))

                If Not IsNothing(CompanyInformation) Then
                    If CompanyInformation.Rows.Count > 0 Then

                        ' write all information from SQL statement to the vCard file
                        writer.WriteLine("BEGIN:VCARD")
                        writer.WriteLine("VERSION:2.1")


                        If ContactInformation.Rows.Count > 0 Then
                            writer.WriteLine("N:" & ContactInformation.Rows(0).Item("contact_first_name") & " " & ContactInformation.Rows(0).Item("contact_last_name"))
                            writer.WriteLine("FN:" & ContactInformation.Rows(0).Item("contact_sirname") & " " & ContactInformation.Rows(0).Item("contact_first_name") & " " & ContactInformation.Rows(0).Item("contact_middle_initial") & " " & ContactInformation.Rows(0).Item("contact_last_name"))
                            writer.WriteLine("TITLE:" & ContactInformation.Rows(0).Item("contact_title"))
                        End If

                        writer.WriteLine("ORG:" & CompanyInformation.Rows(0).Item("comp_name").ToString & "")


                        '' check the state of the DataTable for Company phone Numbers
                        If Not IsNothing(CompanyPhoneInformation) Then
                            If CompanyPhoneInformation.Rows.Count > 0 Then
                                ' set it to the datagrid 
                                For Each q As DataRow In CompanyPhoneInformation.Rows
                                    If q("pnum_type") = "Office" Then
                                        writer.WriteLine("TEL;WORK;VOICE:" & q("pnum_number"))
                                    End If
                                    If q("pnum_type") = "Fax" Then
                                        writer.WriteLine("TEL;WORK;FAX:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Mobile" Then
                                        writer.WriteLine("TEL;CELL:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Residence" Then
                                        writer.WriteLine("TEL;HOME;VOICE:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Residence Fax" Then
                                        writer.WriteLine("TEL;HOME;FAX:" & q("pnum_number"))
                                    End If
                                Next
                            End If
                        End If


                        '' check the state of the DataTable for contact phone numbers
                        If Not IsNothing(ContactPhoneInformation) Then
                            If ContactPhoneInformation.Rows.Count > 0 Then
                                ' set it to the datagrid 
                                For Each q As DataRow In ContactPhoneInformation.Rows
                                    If q("pnum_type") = "Office" Then
                                        writer.WriteLine("TEL;WORK;VOICE:" & q("pnum_number"))
                                    End If
                                    If q("pnum_type") = "Fax" Then
                                        writer.WriteLine("TEL;WORK;FAX:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Mobile" Then
                                        writer.WriteLine("TEL;CELL:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Residence" Then
                                        writer.WriteLine("TEL;HOME;VOICE:" & q("pnum_number"))
                                    End If

                                    If q("pnum_type") = "Residence Fax" Then
                                        writer.WriteLine("TEL;HOME;FAX:" & q("pnum_number"))
                                    End If
                                Next
                            End If
                        End If


                        writer.WriteLine("EMAIL;WORK:" & CompanyInformation.Rows(0).Item("comp_email_address").ToString)
                        writer.WriteLine("ADR;HOME:;;" & CompanyInformation.Rows(0).Item("comp_address1").ToString & " " & CompanyInformation.Rows(0).Item("comp_address2").ToString & ";" & CompanyInformation.Rows(0).Item("comp_city").ToString & ";" & CompanyInformation.Rows(0).Item("comp_state").ToString & ";" & CompanyInformation.Rows(0).Item("comp_zip_code").ToString & ";" & CompanyInformation.Rows(0).Item("comp_country").ToString)
                        writer.WriteLine("END:VCARD")

                        ContactInformation.Dispose()
                        CompanyPhoneInformation.Dispose()
                        ContactPhoneInformation.Dispose()
                        CompanyInformation.Dispose()

                        HttpContext.Current.Response.ContentType = "text/x-vcard"
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=contact.vcf")
                        writer.Close()
                        HttpContext.Current.Response.TransmitFile(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "\contact.vcf"))
                        Create_VCard = 1
                    Else
                        Create_VCard = 0
                    End If
                Else
                    Create_VCard = 0
                End If
            Catch ex As Exception
                Create_VCard = 0
            End Try
        End Function
#End Region

#Region "Listbox Extraction"
        Public Shared Function ExtractSelectedStringFromListboxDropdown(ByVal LB As Object, ByVal surroundedByQuotes As Boolean, ByVal SelectedField As Integer, ByVal NoArray As Boolean) As String
            Dim answer_array() As String
            Dim answer_string As String = ""
            'Model String Building.
            For i = 0 To LB.Items.Count - 1
                If LB.Items(i).Selected Then
                    If LB.Items(i).Value <> "" Then 'Here we check to see if there is a value, meaning there's no selection
                        If UCase(LB.items(i).value) <> "ALL" Then 'Checking to make sure ALL isn't checked, if it is, we don't need to search
                            If NoArray = False Then
                                answer_array = Split(LB.Items(i).Value, "|")

                                If UBound(answer_array) >= SelectedField Then
                                    If surroundedByQuotes = True Then
                                        answer_string += "'"
                                    End If

                                    answer_string += answer_array(SelectedField)

                                    If surroundedByQuotes = True Then
                                        answer_string += "'"
                                    End If
                                    answer_string += ","
                                End If
                            ElseIf NoArray = True Then
                                If surroundedByQuotes = True Then
                                    answer_string += "'"
                                End If

                                answer_string += LB.Items(i).Value

                                If surroundedByQuotes = True Then
                                    answer_string += "'"
                                End If
                                answer_string += ","

                            End If
                        End If
                    End If
                End If
            Next

            If answer_string <> "" Then
                answer_string = UCase(answer_string.TrimEnd(","))
            End If
            Return answer_string
        End Function
#End Region

#Region "SaveStaticFolders"
        ''' <summary>
        ''' This runs on aircraft details/company details
        ''' </summary>
        ''' <param name="folders_label"></param>
        ''' <param name="aclsData_Temp"></param>
        ''' <param name="AircraftID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SaveStaticFolders(ByVal folders_label As Label, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal WantedID As Long, ByVal ContactID As Long, ByVal JournalID As Long, ByVal YachtID As Long) As Label
            Dim AttentionLabel As New Label
            AttentionLabel.ForeColor = Drawing.Color.Red
            AttentionLabel.Font.Bold = True




            Dim SharedCheckboxList As New CheckBoxList
            Dim NonSharedCheckboxList As New CheckBoxList
            If Not IsNothing(folders_label.FindControl("SharedCheckboxList")) Then
                SharedCheckboxList = folders_label.FindControl("SharedCheckboxList")
            End If
            If Not IsNothing(folders_label.FindControl("NonSharedCheckboxList")) Then
                NonSharedCheckboxList = folders_label.FindControl("NonSharedCheckboxList")
            End If


            LoopThroughFolder(NonSharedCheckboxList, folders_label, aclsData_Temp, AircraftID, CompanyID, WantedID, ContactID, JournalID, YachtID)
            LoopThroughFolder(SharedCheckboxList, folders_label, aclsData_Temp, AircraftID, CompanyID, WantedID, ContactID, JournalID, YachtID)
            AttentionLabel.Text = "<br /><br /><p align='center'>Your Folder Information has been saved.</p>"

            folders_label.Controls.Add(AttentionLabel)
            Return folders_label
        End Function

        Public Shared Sub LoopThroughFolder(ByVal TempCheckBoxList As CheckBoxList, ByVal folders_label As Label, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal AircraftID As Long, ByVal CompanyID As Long, ByVal WantedID As Long, ByVal ContactID As Long, ByVal JournalID As Long, ByVal yachtID As Long)
            Dim CfolderData As String = ""
            Dim OldFolderData As New DataTable
            Dim OldCfolderData As String = ""
            Dim TempCheckTable As New DataTable

            For Each li As ListItem In TempCheckBoxList.Items

                If li.Selected Then
                    OldFolderData = New DataTable
                    'Need an add to static folders.
                    'This means first we have to figure out the old cfolder_data.
                    OldFolderData = aclsData_Temp.GetEvolutionFolderssBySubscription(li.Value, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "S")
                    If Not IsNothing(OldFolderData) Then
                        If OldFolderData.Rows.Count > 0 Then
                            'The Folder Actually Exists
                            'Go ahead and get ready for an update.
                            If CompanyID > 0 Then
                                OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "comp_id=", "")
                            ElseIf WantedID > 0 Then
                                OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "amwant_id=", "")
                            ElseIf JournalID > 0 Then
                                OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "journ_id=", "")
                            ElseIf yachtID > 0 Then
                                OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "yt_id=", "")
                            ElseIf ContactID > 0 Then
                                OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "company_contact_info=true!~!contact_id=", "")
                            Else
                                If InStr(OldFolderData.Rows(0).Item("cfolder_data").ToString, "COMPARE_ac_id=Equals!~!ac_id=") > 0 Then
                                    OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "COMPARE_ac_id=Equals!~!ac_id=", "")
                                Else
                                    OldCfolderData = Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "ac_id=", "")
                                End If
                            End If

                            TempCheckTable = aclsData_Temp.GetEvolutionFoldersIndex(li.Value, AircraftID, CompanyID, WantedID, ContactID, JournalID, yachtID)

                            If TempCheckTable.Rows.Count = 0 Then
                                CfolderData = OldCfolderData

                                'Go Ahead and Insert it into the evolution index table.
                                aclsData_Temp.Insert_Into_Evolution_Folder_Index(li.Value, 0, AircraftID, JournalID, CompanyID, ContactID, WantedID, 0, yachtID)

                                If CfolderData <> "" Then
                                    CfolderData += ","
                                End If

                                If CompanyID > 0 Then
                                    CfolderData += "" & CompanyID & ""
                                ElseIf WantedID > 0 Then
                                    CfolderData += "" & WantedID & ""
                                ElseIf ContactID > 0 Then
                                    CfolderData += "" & ContactID & ""
                                ElseIf yachtID > 0 Then
                                    CfolderData += "" & yachtID & ""
                                ElseIf JournalID > 0 Then
                                    CfolderData += "" & JournalID & ""
                                Else
                                    CfolderData += "" & AircraftID & ""
                                End If

                            Else
                                CfolderData = OldCfolderData
                            End If


                        End If
                    End If

                Else
                    'Not selected, meaning we might have to remove it 
                    OldFolderData = New DataTable

                    Dim ACArray As Array
                    'Need an add to static folders.
                    'This means first we have to figure out the old cfolder_data.
                    OldFolderData = aclsData_Temp.GetEvolutionFolderssBySubscription(li.Value, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "S")
                    If Not IsNothing(OldFolderData) Then
                        If OldFolderData.Rows.Count > 0 Then
                            CfolderData = ""
                            If CompanyID > 0 Then
                                ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "comp_id=", ""), ",")
                            ElseIf WantedID > 0 Then
                                ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "amwant_id=", ""), ",")
                            ElseIf yachtID > 0 Then
                                ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "yt_id=", ""), ",")
                            ElseIf JournalID > 0 Then
                                ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "journ_id=", ""), ",")
                            ElseIf ContactID > 0 Then
                                ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "company_contact_info=true!~!contact_id=", ""), ",")
                            Else
                                If InStr(OldFolderData.Rows(0).Item("cfolder_data").ToString, "COMPARE_ac_id=Equals!~!ac_id=") > 0 Then
                                    ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "COMPARE_ac_id=Equals!~!ac_id=", ""), ",")
                                Else
                                    ACArray = Split(Replace(OldFolderData.Rows(0).Item("cfolder_data").ToString, "ac_id=", ""), ",")
                                End If
                            End If


                            TempCheckTable = aclsData_Temp.GetEvolutionFoldersIndex(li.Value, AircraftID, CompanyID, WantedID, ContactID, JournalID, yachtID)
                            If TempCheckTable.Rows.Count > 0 Then
                                'Remove the index entry. 
                                aclsData_Temp.Remove_Evolution_Folder_Index(TempCheckTable.Rows(0).Item("cfoldind_id"), li.Value, AircraftID, CompanyID, WantedID, ContactID, JournalID, yachtID)
                            End If

                            'rebuild the cdata
                            If CompanyID > 0 Then
                                For x = 0 To UBound(ACArray)
                                    If IsNumeric(ACArray(x)) Then
                                        If ACArray(x) <> CompanyID Then
                                            If CfolderData <> "" Then
                                                CfolderData += ","
                                            End If
                                            CfolderData += ACArray(x)
                                        End If
                                    End If
                                Next
                            ElseIf WantedID > 0 Then
                                For x = 0 To UBound(ACArray)
                                    If IsNumeric(ACArray(x)) Then
                                        If ACArray(x) <> WantedID Then
                                            If CfolderData <> "" Then
                                                CfolderData += ","
                                            End If
                                            CfolderData += ACArray(x)
                                        End If
                                    End If
                                Next
                            ElseIf JournalID > 0 Then
                                For x = 0 To UBound(ACArray)
                                    If IsNumeric(ACArray(x)) Then
                                        If ACArray(x) <> JournalID Then
                                            If CfolderData <> "" Then
                                                CfolderData += ","
                                            End If
                                            CfolderData += ACArray(x)
                                        End If
                                    End If
                                Next
                            ElseIf ContactID > 0 Then
                                For x = 0 To UBound(ACArray)
                                    If IsNumeric(ACArray(x)) Then
                                        If ACArray(x) <> ContactID Then
                                            If CfolderData <> "" Then
                                                CfolderData += ","
                                            End If
                                            CfolderData += ACArray(x)
                                        End If
                                    End If
                                Next
                            Else
                                For x = 0 To UBound(ACArray)
                                    If IsNumeric(ACArray(x)) Then
                                        If ACArray(x) <> AircraftID Then
                                            If CfolderData <> "" Then
                                                CfolderData += ","
                                            End If
                                            CfolderData += ACArray(x)
                                        End If
                                    End If
                                Next
                            End If



                        End If
                    End If
                End If


                If CompanyID > 0 Then
                    CfolderData = "comp_id=" & CfolderData
                ElseIf WantedID > 0 Then
                    CfolderData = "amwant_id=" & CfolderData
                ElseIf yachtID > 0 Then
                    CfolderData = "yt_id=" & CfolderData
                ElseIf JournalID > 0 Then
                    CfolderData = "journ_id=" & CfolderData
                ElseIf ContactID > 0 Then
                    CfolderData = "company_contact_info=true!~!contact_id=" & CfolderData
                Else
                    CfolderData = "COMPARE_ac_id=Equals!~!ac_id=" & CfolderData
                End If

                ' MSW - this is an error that seems to be re-created thro certain steps, cant find where, so replacing here to fix - 4/8/19
                If InStr(CfolderData, "COMPARE_ac_id=Equals!~!ac_id=ac_id=") > 0 Then
                    CfolderData = Replace(CfolderData, "COMPARE_ac_id=Equals!~!ac_id=ac_id=", "COMPARE_ac_id=Equals!~!ac_id=")
                End If


                'Running the update statement after either removed or added:
                aclsData_Temp.Edit_Fields_Evolution_Folders(CfolderData, OldFolderData.Rows(0).Item("cfolder_hide_flag").ToString, OldFolderData.Rows(0).Item("cfolder_name").ToString, OldFolderData.Rows(0).Item("cfolder_share").ToString, OldFolderData.Rows(0).Item("cfolder_description").ToString, li.Value, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", "", "", "", 0, OldFolderData.Rows(0).Item("cfolder_operator_flag").ToString)
            Next
        End Sub
#End Region

        Public Shared Sub Figure_Out_Note_Search_Fields(ByRef ac_search_field As DropDownList, ByRef ac_search_field_operator As DropDownList, ByRef ac_search_field_text As TextBox, ByRef acSearchField As Integer, ByRef acSearchOperator As Integer, ByRef acSearchText As String)
            'We're going to go ahead and verify that the aircraft search field is either 2,3,4 or it's going to default to 1.
            If IsNumeric(ac_search_field.SelectedValue) Then
                Select Case ac_search_field.SelectedValue
                    Case 2, 3, 4
                        acSearchField = ac_search_field.SelectedValue
                    Case Else
                        acSearchField = 1
                End Select
            Else 'If this isn't passed as a number, we're automatically changing it to 1.
                acSearchField = 1
            End If

            'The same goes for the search field operator.
            If IsNumeric(ac_search_field_operator.SelectedValue) Then
                Select Case ac_search_field_operator.SelectedValue
                    Case 2, 3
                        acSearchOperator = ac_search_field_operator.SelectedValue
                    Case Else
                        acSearchOperator = 1
                End Select
            Else 'if on the off chance somehow a variable gets passed through the dropdown that isn't numeric, we're
                'just going to go ahead and turn it into 1
                acSearchOperator = 1
            End If

            If Not String.IsNullOrEmpty(ac_search_field_text.Text) Then
                'If this isn't empty, we go ahead and grab the information.
                acSearchText = Trim(StripChars(ac_search_field_text.Text, True))
            Else 'However if it is empty, then we're going to clear the other variables, because at this point, they're useless without
                'search text.
                acSearchField = 0
                acSearchOperator = 0
                acSearchText = ""
            End If
        End Sub

#Region "Evo - Advanced Search Prep"

        ''' <summary>
        ''' This function preps the query string for the advanced search.
        ''' </summary>
        ''' <param name="OperatorChoice"></param>
        ''' <param name="SearchVal"></param>
        ''' <param name="DataType"></param>
        ''' <param name="ConvertDateTime">If this is true, we're going to add a small convert sql date time that basically treats the date like a date, not datetime</param>
        ''' <returns></returns>
        ''' <remarks></remarks> 
        Public Shared Function PrepQueryString(ByVal OperatorChoice As String, ByVal SearchVal As String, ByVal DataType As String, ByVal ConvertDateTime As Boolean, ByVal DataName As String, ByVal CommasAsDelimeter As Boolean)
            Dim Quotes As String = "'"
            Dim ReturnString As String = ""
            Dim ConvertTimeStart As String = ""
            Dim ConvertTimeEnd As String = ""
            Dim ACArray As Array
            Dim SplittableInClause As Boolean = False


            SearchVal = StripChars(SearchVal, False)
            If CommasAsDelimeter = False Then
                SearchVal = Replace(SearchVal, "&apos;", "''")
            End If

            Select Case DataType
                Case "Char", "String", "Year"
                    Quotes = "'"
                Case "Date"
                    Quotes = "'"
                    If ConvertDateTime = True Then
                        ConvertTimeStart = "CONVERT (DATETIME,"
                        ConvertTimeEnd = ",102)"
                    End If
                Case "Numeric"
                    Quotes = ""
            End Select


            If CommasAsDelimeter = True Then
                If InStr(SearchVal, ",") > 0 Or InStr(SearchVal, "*") > 0 Then
                    SplittableInClause = True
                End If
            End If


            If SplittableInClause = True And (Trim(DataName) = "comp_id" Or Trim(DataName) = "clicomp_id") Then   ' added MSW -  10/17/19
                ACArray = Split(SearchVal, ",")

                If UBound(ACArray) = 0 Then
                    If InStr(SearchVal, "*") > 0 Then
                        ReturnString = DataName & " = " & ConvertTimeStart & Quotes & Replace(SearchVal, "*", "%") & Quotes & ConvertTimeEnd
                    End If
                Else
                    ReturnString = " " & DataName & "  in ("
                    For x = 0 To UBound(ACArray)
                        If x > 0 Then
                            ReturnString += ","
                        End If
                        ReturnString += ConvertTimeStart & Quotes & Trim(Replace(ACArray(x), "*", "")) & Quotes & ConvertTimeEnd
                    Next

                    ReturnString += ")"
                End If
            ElseIf SplittableInClause = True Then
                ACArray = Split(SearchVal, ",")

                If UBound(ACArray) = 0 Then
                    If InStr(SearchVal, "*") > 0 Then
                        ReturnString = DataName & " like " & ConvertTimeStart & Quotes & Replace(SearchVal, "*", "%") & Quotes & ConvertTimeEnd
                    End If
                ElseIf Trim(DataName) = "comp_name_search" Or Trim(DataName) = "comp_altname_search" Then  'if the data is splittable, then we should always do it with  a like for comp name search
                    ' ADDED IN MSW - 4/13/20
                    ' dont check to see if there is a *, nomatter what use like a star was in there. 
                    ReturnString = " ( "
                    For x = 0 To UBound(ACArray)
                        If x > 0 Then
                            ReturnString += " or "
                        End If
                        If InStr(Trim(ACArray(x)), "*") = 0 Then
                            ACArray(x) = Trim(ACArray(x)) & "*"
                        End If
                        ReturnString += "(" & DataName & " like " & ConvertTimeStart & Quotes & Replace(Trim(ACArray(x)), "*", "%") & Quotes & ConvertTimeEnd & ")"
                    Next

                    ReturnString += " )"

                Else
                    If InStr(SearchVal, "*") = 0 Then
                        ReturnString = " in ("
                        For x = 0 To UBound(ACArray)
                            If x > 0 Then
                                ReturnString += ","
                            End If
                            ReturnString += ConvertTimeStart & Quotes & Trim(ACArray(x)) & Quotes & ConvertTimeEnd
                        Next

                        ReturnString += ")"
                    Else
                        'This has asterisks
                        ReturnString = " ( "
                        For x = 0 To UBound(ACArray)
                            If x > 0 Then
                                ReturnString += " or "
                            End If
                            If InStr(Trim(ACArray(x)), "*") = 0 Then
                                ACArray(x) = Trim(ACArray(x)) & "*"
                            End If
                            ReturnString += "(" & DataName & " like " & ConvertTimeStart & Quotes & Replace(Trim(ACArray(x)), "*", "%") & Quotes & ConvertTimeEnd & ")"
                        Next

                        ReturnString += " )"
                    End If

                End If
            Else
                'A small catch that is put here to force the operator choice to be between
                'if the data type is Date or Numeric.
                If DataType = "Date" Or DataType = "Numeric" Or DataType = "Year" Then
                    If InStr(SearchVal, ":") > 0 Then
                        OperatorChoice = "Between"
                    End If
                End If
                Select Case OperatorChoice
                    Case "Equals"
                        ReturnString = " = " & ConvertTimeStart & Quotes & SearchVal & Quotes & ConvertTimeEnd
                    Case "Includes"
                        ReturnString = " like " & ConvertTimeStart & Quotes & "%" & SearchVal & "%" & Quotes & ConvertTimeEnd
                    Case "Begins With"
                        ReturnString = " like " & ConvertTimeStart & Quotes & "" & SearchVal & "%" & Quotes & ConvertTimeEnd
                    Case "Less Than"
                        ReturnString = " < " & ConvertTimeStart & Quotes & SearchVal & Quotes & ConvertTimeEnd
                    Case "Between"
                        SearchVal = Replace(SearchVal, ";", ":")
                        Dim ar As Array = Split(SearchVal, ":")
                        If UBound(ar) = 1 Then
                            ReturnString = "between " & ConvertTimeStart & Quotes & Trim(ar(0)) & Quotes & ConvertTimeEnd & " and " & ConvertTimeStart & Quotes & Trim(ar(1)) & Quotes & ConvertTimeEnd
                        End If
                    Case Else

                        If DataType = "Year" And String.IsNullOrEmpty(OperatorChoice) Then  ' added in msw
                            ReturnString = " = " + SearchVal
                        Else
                            ReturnString = " > " + ConvertTimeStart + Quotes + SearchVal + Quotes + ConvertTimeEnd
                        End If

                End Select
            End If
            Return ReturnString
        End Function

        ''' <summary>
        ''' This is currently being worked on and only used on the company listing page for now.
        ''' </summary>
        ''' <param name="OperatorChoice"></param>
        ''' <param name="SearchVal"></param>
        ''' <param name="DataType"></param>
        ''' <param name="ConvertDateTime"></param>
        ''' <param name="DataName"></param>
        ''' <param name="CommasAsDelimeter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ZipCodePrepQueryString(ByVal OperatorChoice As String, ByVal SearchVal As String, ByVal DataType As String, ByVal ConvertDateTime As Boolean, ByVal DataName As String, ByVal CommasAsDelimeter As Boolean)
            Dim Quotes As String = "'"
            Dim ReturnString As String = ""
            Dim ACArray As Array
            Dim SplittableInClause As Boolean = False
            'First we're going to pass this through a function that's
            'going to get rid of unnecessary characters that shouldn't show up in a 
            'postal/zip code.
            SearchVal = StripChars(SearchVal, False)
            'Added 12/05/17 to replace underscore with comma.
            SearchVal = SearchVal.Replace(Constants.cImbedComa, Constants.cCommaDelim)


            'If we pass the option to allow commas to be a delimiter, which in the case of a zip code,
            'will be, then we go ahead and check and see if there isa comma or an asterisk.
            'If there is one, then we go ahead and say it's a splittable clause.
            If CommasAsDelimeter = True Then
                If InStr(SearchVal, ",") > 0 Or InStr(SearchVal, "*") > 0 Then
                    SplittableInClause = True
                End If
            End If
            'If this has be determined to be a splittable clause
            If SplittableInClause = True Then
                ACArray = Split(SearchVal, ",")
                'Then we go ahead and split by comma.
                If UBound(ACArray) = 0 Then
                    'If the array only has one 
                    If InStr(SearchVal, "*") > 0 Then 'Then we're going to check to see if there's an asterisk.
                        Dim TemporaryHold As String = Trim(Replace(SearchVal, "*", "")) 'Now we have to declare a temporary variable used to check the length of the item without an asterisk.
                        TemporaryHold = Replace(TemporaryHold, " ", "") 'Replacing spaces for length check.

                        Dim TemporaryDataName As String = DataName 'If there is an asterisk, we're going to set a temporary variable that holds the data name.
                        OperatorChoice = " like " 'The operator choice is going to be defaulted to like, because an asterisk is wildcard.

                        If TemporaryHold.Length = 6 And InStr(SearchVal, " ") > 0 Then 'We need to check for a canadian zip code - 6 characters that includes a space.
                            'This is a canadian zip code.
                            OperatorChoice = " = " 'This sets the operator to be = instead of like.
                            TemporaryDataName = "SUBSTRING(" & DataName & ",1,7) "
                            SearchVal = Replace(Trim(SearchVal), "*", "") 'This removes the asterisk.
                        ElseIf SearchVal.Length = 5 And InStr(SearchVal, " ") = 0 Then 'Five characters with no space.
                            TemporaryDataName = "SUBSTRING(" & DataName & ",1,5) "
                            OperatorChoice = " = " 'This sets the operator to be = instead of like.
                            SearchVal = Replace(Trim(SearchVal), "*", "") 'This removes the asterisk.
                        Else
                            'That the like is performed on comp_zip_code instead of substring(comp_zip_code, 1, 5)
                            If InStr(SearchVal, "*") > 0 Then
                                TemporaryDataName = DataName
                            End If
                        End If

                        'However we can't forget to replace the asterisk with percentage.
                        ReturnString = TemporaryDataName & " " & OperatorChoice & " " & Quotes & Replace(SearchVal, "*", "%") & Quotes
                    End If
                Else 'This code runs of  the array is more than 1 
                    Dim ZipCodeLike As String = "" 'we're going to initialize three seperate strings here.
                    Dim ZipCodeSub As String = "" 'I found this an easy way to figure out the ors vs the likes.
                    Dim ZipCodeFull As String = "" 'One is for likes, one is for 5 digit zip codes, one is for the full zip codes.
                    Dim ZipCodeCanadian As String = "" 'This one is set up for Canadian zip codes.

                    ReturnString = " ( "

                    For x = 0 To UBound(ACArray) 'For each item in the zip code array.
                        If InStr(Trim(ACArray(x)), "*") > 0 Then 'First we check and see if there's an asterisk.
                            Dim TemporaryHold As String = Trim(Replace(ACArray(x), "*", "")) 'Now we have to declare a temporary variable used to check the length of the item without an asterisk.
                            TemporaryHold = Replace(TemporaryHold, " ", "") 'Let's replace the spaces while we're at it so we can properly check for length.

                            If TemporaryHold.Length = 6 And InStr(Trim(ACArray(x)), " ") > 0 Then 'We need to check for canadian zip codes.
                                If ZipCodeCanadian <> "" Then
                                    ZipCodeCanadian += ","
                                End If
                                ZipCodeCanadian += Replace(ACArray(x), "*", "")

                            ElseIf InStr(Trim(ACArray(x)), "-") > 0 Then 'Is the zip code a full zip code? This is going to check for the hyphen.
                                If ZipCodeFull <> "" Then
                                    ZipCodeFull += ","
                                End If
                                ZipCodeFull += Replace(ACArray(x), "*", "") 'Now if they have an extended zip code, we're going to ignore the * and set up the zip code full variable.

                            ElseIf TemporaryHold.Length = 5 And InStr(Trim(ACArray(x)), " ") = 0 Then 'A length of five and no spaces
                                If ZipCodeSub <> "" Then
                                    ZipCodeSub += ","
                                End If
                                ZipCodeSub += Replace(ACArray(x), "*", "") 'Here we set up the zip code sub variable after ignoring the asterisk.
                            Else 'This is the blanket like statement - if nothing else matches, we use a like.
                                If ZipCodeLike <> "" Then 'we need to go ahead and set up the wildcard.
                                    ZipCodeLike += ","
                                End If

                                Dim Place As Integer = ACArray(x).ToString.IndexOf("*") 'This code is going to search for the first instance of an * and 
                                ACArray(x) = ACArray(x).ToString.Remove(Place, 1).Insert(Place, "%") 'then it replaces them with a percentage sign.
                                ZipCodeLike += Replace(ACArray(x), "*", "") 'And then it goes ahead and replaces any other asterisks with nothi
                            End If

                        Else 'This means there is no asterisk.
                            If InStr(Trim(ACArray(x)), "-") > 0 Then 'Once again we check for that hyphen. If it has one, then it's a full zip code.
                                If ZipCodeFull <> "" Then
                                    ZipCodeFull += ","
                                End If
                                ZipCodeFull += ACArray(x)
                            Else 'This means the zipcode is not an extended one.
                                'Let's check and see if it's 5 letters?
                                Dim TemporaryHold As String = Trim(ACArray(x)) 'Since there is no asterisk here (earlier check) - we don't need to replace it.
                                TemporaryHold = Replace(TemporaryHold, " ", "")
                                If TemporaryHold.Length = 5 And InStr(Trim(ACArray(x)), " ") = 0 Then 'is the length equal to 5? Is there a space? If there is a space, but 5 length, it should be treated like a wildcard. The space indicates that it's a canadian postal code.
                                    If ZipCodeSub <> "" Then
                                        ZipCodeSub += ","
                                    End If
                                    ZipCodeSub += ACArray(x) 'This will set up the zip code canadian variable. It goes ahead and checks length and for a space.
                                ElseIf TemporaryHold.Length = 6 And InStr(ACArray(x), " ") > 0 Then
                                    If ZipCodeCanadian <> "" Then
                                        ZipCodeCanadian += ","
                                    End If
                                    ZipCodeCanadian += ACArray(x)
                                Else 'This means it's not 5, or it's 5 with a space - which means it's a like statement. It could be a canadian zip code if there's a space so it should still be a like
                                    If ZipCodeLike <> "" Then
                                        ZipCodeLike += ","
                                    End If
                                    ZipCodeLike += ACArray(x) + "%" 'We need to go ahead and append an asterisk since earlier checks indicate that it has none.
                                End If
                            End If
                        End If

                    Next


                    Dim ZipCodeLikeString As String = "" 'I set up these four string variables so that I could build sub clauses easily as well as check
                    Dim ZipCodeFullString As String = "" 'and see if they're already holding something. It makes it a little bit easier to manage.
                    Dim ZipCodeSubString As String = "" 'One for the like statement, the full is the equals statement, the canadian postal codes substring(x,1,7), and the sub substring(x, 1,5) statments.
                    Dim ZipCodeCanadianString As String = ""

                    If ZipCodeLike <> "" Then 'If there are zip codes needing a like statement
                        Dim ZipCodeLikeArray As Array = Split(ZipCodeLike, ",") 'I'm going to create a special array
                        For z = 0 To UBound(ZipCodeLikeArray) 'Loop through them.
                            If ZipCodeLikeString <> "" Then 'Check if this is the first time through, if not add an or statement.
                                ZipCodeLikeString += " or "
                            End If
                            ZipCodeLikeString += "(" + DataName + " like " + Quotes + Trim(ZipCodeLikeArray(z)) + Quotes + ")" 'And finally build the statement. Seperated by parentheses for easier reading.
                        Next
                    End If

                    If ZipCodeFull <> "" Then 'If there are zip codes that are considered extended (hyphen included)
                        Dim ZipCodeFullArray As Array = Split(ZipCodeFull, ",") 'I'm going to build a special array for them.
                        For z = 0 To UBound(ZipCodeFullArray) 'Loop through them
                            If ZipCodeFullString <> "" Then 'Check for the first time through and add an or statement if it is.
                                ZipCodeFullString += ","
                            End If
                            ZipCodeFullString += Quotes + Trim(ZipCodeFullArray(z)) + Quotes 'Then go ahead and build the zip code full string.
                        Next

                        If ZipCodeFullString <> "" Then 'If the full string has been built
                            If InStr(ZipCodeFullString, ",") > 0 Then 'We're going to check for a comma (one time through)
                                ZipCodeFullString = " (" + DataName + " in (" + ZipCodeFullString + "))" 'If it has a comma, it's a like statement.
                            Else 'If it has no comma, then it's an equals statement
                                ZipCodeFullString = " (" + DataName + " = " + ZipCodeFullString + ")"
                            End If
                        End If
                    End If

                    If ZipCodeSub <> "" Then 'This is set up much like the other ones. If there are sub (5 digit) zip codes (no wildcard)
                        ZipCodeSubString = SetUpSubZipCode(ZipCodeSub, Quotes, DataName, 5)
                    End If

                    If ZipCodeCanadian <> "" Then 'This is set up much like sub string, except it's a canadian zip code so the substring is 7, not five.
                        ZipCodeCanadianString = SetUpSubZipCode(ZipCodeCanadian, Quotes, DataName, 7)
                    End If

                    ReturnString = "" 'Here is where we actually build the string returned.

                    If ZipCodeFullString <> "" Then 'First we go through the full zip codes and append them to the return string if necessary.
                        ReturnString += ZipCodeFullString
                    End If

                    If ZipCodeLikeString <> "" Then 'Then we go through the like strings and append them (and an or) if necessary.
                        If ReturnString <> "" Then
                            ReturnString += " or "
                        End If
                        ReturnString += ZipCodeLikeString
                    End If

                    If ZipCodeSubString <> "" Then 'Then the sub strings and append them as well as an or if it's required.
                        If ReturnString <> "" Then
                            ReturnString += " or "
                        End If
                        ReturnString += ZipCodeSubString
                    End If

                    If ZipCodeCanadianString <> "" Then 'Finally we go ahead and build the canadian zip code.
                        If ReturnString <> "" Then
                            ReturnString += " or "
                        End If
                        ReturnString += ZipCodeCanadianString
                    End If


                    ReturnString = "(" + ReturnString + " )" 'This finally move seperates the zip code with parentheses, for easier reading.

                End If
            Else 'This means that there's no asterisk and there's no comma. Basically a search on a single zip code.
                If InStr(SearchVal, "-") = 0 And SearchVal.Length = 5 Then 'There's no hyphen and the length is 5.
                    DataName = "SUBSTRING(" & DataName & ",1,5) " 'We need to change the field name to be a substring, basically searching on the first 5 letters.
                ElseIf SearchVal.Length = 7 And InStr(SearchVal, " ") > 0 Then
                    DataName = "SUBSTRING(" & DataName & ",1,7) " 'We need to change the field name to be a substring, basically searching on the first 7 letters. This is a small catch for canadian postal codes.
                End If
                If SearchVal.Length = 5 Or SearchVal.Length = 7 And InStr(SearchVal, " ") > 0 Then 'If the search value length is equal to 5, or if it's 7 and includes a space.
                    ReturnString = "(" + DataName + " = " + Quotes + SearchVal + Quotes + ")" 'We use an equals.
                Else
                    ReturnString = "(" + DataName + " like " + Quotes + SearchVal + "%" + Quotes + ")" 'Otherwise we're going to force a like because it's not a 5 digit zip code
                End If
            End If
            Return ReturnString
        End Function

        Public Shared Function SetUpSubZipCode(ByVal ZipCodeSub As String, ByVal Quotes As String, ByVal DataName As String, ByVal CountofSubString As Integer) As String
            Dim ZipCodeSubString As String = ""
            If ZipCodeSub <> "" Then 'This is set up much like the other ones. If there are sub (5 digit) zip codes (no wildcard)
                Dim ZipCodeSubArray As Array = Split(ZipCodeSub, ",") 'Then we build a special array for them.

                For z = 0 To UBound(ZipCodeSubArray) 'loop through the array
                    If ZipCodeSubString <> "" Then 'add a comma if needed
                        ZipCodeSubString += ","
                    End If
                    ZipCodeSubString += Quotes + Trim(ZipCodeSubArray(z)) + Quotes 'Build the string
                Next
                If ZipCodeSubString <> "" Then 'If the string has been built
                    If InStr(ZipCodeSubString, ",") > 0 Then 'Check for the existence of a comma
                        ZipCodeSubString = " (SUBSTRING(" + DataName + ",1," & CountofSubString & ") in (" + ZipCodeSubString + "))" 'If there is one, we need to use an in clause.
                    Else
                        ZipCodeSubString = " (SUBSTRING(" + DataName + ",1," & CountofSubString & ")  = " + ZipCodeSubString + ")" 'If there isn't one, it's just a single or.
                    End If
                End If
            End If
            Return ZipCodeSubString
        End Function
        ''' <summary>
        ''' Used below in clean user data finds a string in another string
        ''' </summary>
        ''' <param name="sInputString"></param>
        ''' <param name="sDelimiter"></param>
        ''' <returns>boolean</returns>
        ''' <remarks></remarks>
        Public Shared Function FindItemInData(ByVal sInputString As String, ByVal sDelimiter As String) As Boolean

            Dim nPos As Integer = 0

            If Not String.IsNullOrEmpty(sInputString.Trim) Then
                nPos = sInputString.IndexOfAny(sDelimiter, 0)
            End If

            If nPos > 0 Then
                Return True
            Else
                Return False
            End If

        End Function
        ''' <summary>
        ''' Used to replace an item on string or parse "cut and paste" column into a "sReplace value" delimited string
        ''' </summary>
        ''' <param name="sInputString"></param>
        ''' <param name="sFind"></param>
        ''' <param name="sReplace"></param>
        ''' <param name="bIsTextAreaInput"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CleanUserData(ByVal sInputString As String, ByVal sFind As String, ByVal sReplace As String, ByVal bIsTextAreaInput As Boolean)

            Dim CRLF = Chr(13) + Chr(10)

            Dim n_loop As Integer = 0
            Dim n_offset As Integer = 0
            Dim n_offset1 As Integer = 0
            Dim sTmpData As String = ""
            Dim sOutputString As String = ""

            If Not String.IsNullOrEmpty(sInputString.Trim) Then

                If Not bIsTextAreaInput Then

                    sOutputString = sInputString.Replace(sFind, sReplace).Trim

                Else

                    Do While n_loop < sInputString.Length + 1

                        ' find first CRLF
                        'n_offset = InStr(n_loop, inputString, CRLF, CompareMethod.Binary)
                        n_offset = sInputString.IndexOfAny(CRLF, n_loop)

                        ' find seccond CRLF
                        'n_offset1 = InStr(n_offset + 1, inputString, CRLF, CompareMethod.Binary)
                        n_offset1 = sInputString.IndexOfAny(CRLF, n_offset + 1)

                        If n_offset > 0 And n_offset1 = -1 Then
                            n_offset1 = sInputString.Length
                        End If

                        ' grab first item from n_loop to n_offset
                        If (n_offset > n_loop) Then

                            sTmpData = sInputString.Substring(n_loop, n_offset)

                            ' clean out any "GA" Garbage also zero length data
                            If Not sTmpData.ToUpper.Contains("GA") And sTmpData.Length > 0 Then

                                ' I also need to preserve any commas in the data
                                If sTmpData.Contains(Constants.cCommaDelim) Then
                                    ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                                    sTmpData = sTmpData.Replace(Constants.cCommaDelim, Constants.cImbedComa)
                                End If

                                ' clean out the EXCEL 03 Character
                                sTmpData = sTmpData.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

                                If String.IsNullOrEmpty(sOutputString.Trim) Then
                                    sOutputString = sTmpData
                                Else
                                    sOutputString += sReplace + sTmpData.Trim
                                End If

                            End If

                        End If

                        If (n_offset = -1 Or n_offset1 = -1) And n_offset = n_offset1 Then
                            sOutputString = Trim(sInputString)
                            Exit Do
                        End If

                        sTmpData = ""

                        If (n_offset1 > n_loop) Then  ' found second CRLF after our start

                            ' find next CRLF start 1 chars ahead of our first CRLF pair
                            If (n_offset1 > n_offset) Then ' found next CRLF the data is between the two offsets

                                If (n_offset1 - n_offset) > 1 Then ' ok we have at least one char between the two

                                    sTmpData = sInputString.Substring(n_offset + 1, ((n_offset1 - n_offset) - 1)) ' ok get the data

                                    ' clean out any "GA" Garbage also zero length data
                                    If Not sTmpData.ToUpper.Contains("GA") And sTmpData.Length > 0 Then

                                        ' I also need to preserve any commas in the data
                                        If sTmpData.Contains(Constants.cCommaDelim) Then
                                            ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                                            sTmpData = sTmpData.Replace(Constants.cCommaDelim, Constants.cImbedComa)
                                        End If

                                        ' clean out the EXCEL 03 Character
                                        sTmpData = sTmpData.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

                                        If String.IsNullOrEmpty(sOutputString.Trim) Then
                                            sOutputString = sTmpData
                                        Else
                                            sOutputString += sReplace + sTmpData.Trim
                                        End If

                                    End If
                                End If
                            End If

                        Else
                            Exit Do
                        End If

                        ' jump ahead n_offset1 to look for the next chunk of data
                        If n_offset1 > 0 Then
                            n_loop = n_offset1
                        End If

                        n_offset = 0
                        n_offset1 = 0
                        sTmpData = ""

                    Loop ' While n_loop < sInputString.Length + 1

                    If Not String.IsNullOrEmpty(sOutputString.Trim) Then

                        ' chop off the last comma if there is one
                        If Right(sOutputString, 1) = Constants.cCommaDelim Or
                          Right(sOutputString, 1) = Constants.cColonDelim Or
                          Right(sOutputString, 1) = Constants.cSemiColonDelim Then
                            sOutputString = Left(sOutputString, sOutputString.Length - 1)

                        End If

                    Else

                        ' I also need to preserve any commas in the data
                        If sInputString.Contains(Constants.cCommaDelim) Then
                            ' change imbedded commas to underscores  ie XYZ, LLC to XYZ_ LLC
                            sInputString = sInputString.Replace(Constants.cCommaDelim, Constants.cImbedComa)
                        End If

                        ' clean out the EXCEL 03 Character
                        sInputString = sInputString.Replace(Constants.EXCEL2003CHAR, Constants.cEmptyString)

                        ' clean out the CRLF
                        sInputString = sInputString.Replace(CRLF, Constants.cEmptyString)

                        sOutputString = sInputString.Trim

                    End If

                End If ' not bIsTextAreaInput

                ' chop off the last comma if there is one
                If Right(sOutputString, 1) = Constants.cCommaDelim Or
                  Right(sOutputString, 1) = Constants.cColonDelim Or
                  Right(sOutputString, 1) = Constants.cSemiColonDelim Then
                    sOutputString = sOutputString.Substring(1, sOutputString.Length - 1)
                End If

            End If ' Not String.IsNullOrEmpty(sInputString.Trim)

            Return sOutputString

        End Function

#End Region
#Region "Clear Evo Search Class"
        Public Shared Sub ClearSavedSelection()
            'Clear out the Type/Make/Model Boxes Properly on Reset:
            HttpContext.Current.Session.Item("tabAircraftType") = ""
            HttpContext.Current.Session.Item("tabAircraftMake") = ""
            HttpContext.Current.Session.Item("tabAircraftModel") = ""
            HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
            HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
            HttpContext.Current.Session.Item("tabAircraftSize") = ""
            HttpContext.Current.Session.Item("hasModelFilter") = False

            HttpContext.Current.Session.Item("chkHelicopterFilter") = False
            HttpContext.Current.Session.Item("chkBusinessFilter") = False
            HttpContext.Current.Session.Item("chkCommercialFilter") = False

            HttpContext.Current.Session.Item("tabYachtCategory") = ""
            HttpContext.Current.Session.Item("tabYachtModel") = ""
            HttpContext.Current.Session.Item("tabYachtBrand") = ""

            HttpContext.Current.Session.Item("companyRegion") = ""
            HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent"
            HttpContext.Current.Session.Item("companyTimeZone") = ""
            HttpContext.Current.Session.Item("companyCountry") = ""
            HttpContext.Current.Session.Item("companyState") = ""
            'HttpContext.Current.Session.Item("translatedCompanyStates") = ""

            HttpContext.Current.Session.Item("baseRegion") = ""
            HttpContext.Current.Session.Item("baseRegionOrContinent") = "continent"
            HttpContext.Current.Session.Item("baseCountry") = ""
            HttpContext.Current.Session.Item("baseState") = ""
            'HttpContext.Current.Session.Item("translatedBaseStates") = ""



            Dim I As Integer = 0
            Dim L As Integer = HttpContext.Current.Session.Contents.Count
            Dim keyName As String

            For I = L - 1 To 0 Step -1
                If TypeOf (HttpContext.Current.Session.Contents.Item(I)) Is String Then
                    If InStr(HttpContext.Current.Session.Contents.Keys(I).ToString(), "Advanced-") > 0 Then

                        keyName = HttpContext.Current.Session.Contents.Keys(I).ToString()
                        HttpContext.Current.Session.Remove(keyName)
                    End If
                End If
            Next

            HttpContext.Current.Session.Item("searchCriteria") = New SearchSelectionCriteria
        End Sub
#End Region

        Public Shared Function BuildCustomFieldsString(ByRef data_subset As String, ByRef CustomField1 As String, ByRef CustomField2 As String, ByRef CustomField3 As String, ByRef CustomField4 As String, ByRef CustomField5 As String, ByRef CustomField6 As String, ByRef CustomField7 As String, ByRef CustomField8 As String, ByRef CustomField9 As String, ByRef CustomField10 As String) As String
            Dim ReturnString As String = ""
            Dim andQ As String = ""

            'Starting with Custom Field 1
            If CustomField1 <> "" Then
                If CustomField1 = "_" Then
                    CustomField1 = "%"
                End If
                data_subset = "C"

                ReturnString += " cliaircraft_custom_1 like '" & CustomField1 & "' "
            End If

            'Custom Field 2
            If CustomField2 <> "" Then
                If CustomField2 = "_" Then
                    CustomField2 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_2 like '" & CustomField2 & "' "
            End If

            'Custom Field 3
            If CustomField3 <> "" Then
                If CustomField3 = "_" Then
                    CustomField3 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_3 like '" & CustomField3 & "' "
            End If

            'Custom Field 4
            If CustomField4 <> "" Then
                If CustomField4 = "_" Then
                    CustomField4 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_4 like '" & CustomField4 & "' "
            End If

            'Custom Field 5
            If CustomField5 <> "" Then
                If CustomField5 = "_" Then
                    CustomField5 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_5 like '" & CustomField5 & "' "
            End If

            'Custom Field 6
            If CustomField6 <> "" Then
                If CustomField6 = "_" Then
                    CustomField6 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_6 like '" & CustomField6 & "' "
            End If

            'Custom Field 7
            If CustomField7 <> "" Then
                If CustomField7 = "_" Then
                    CustomField7 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_7 like '" & CustomField7 & "' "
            End If

            'Custom Field 8
            If CustomField8 <> "" Then
                If CustomField8 = "_" Then
                    CustomField8 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_8 like '" & CustomField8 & "' "
            End If

            'Custom Field 9
            If CustomField9 <> "" Then
                If CustomField9 = "_" Then
                    CustomField9 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_9 like '" & CustomField9 & "' "
            End If

            'Custom Field 10
            If CustomField10 <> "" Then
                If CustomField10 = "_" Then
                    CustomField10 = "%"
                End If
                data_subset = "C"
                If ReturnString <> "" Then
                    andQ = " and "
                Else
                    andQ = ""
                End If
                ReturnString += andQ & " cliaircraft_custom_10 like '" & CustomField10 & "' "
            End If

            Return ReturnString
        End Function


        Public Shared Function SaveMarketComparables(ByRef NoteInformation As clsLocal_Notes, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef savedAFTT As String) As Integer
            Dim AircraftDataTable As New DataTable
            Dim CurrentData As New DataTable
            Dim AircraftData As New clsClient_Aircraft
            AircraftDataTable = aclsData_Temp.Get_Client_Current_Market_Comparables(NoteInformation.lnote_id)

            'Remove up above:
            aclsData_Temp.Remove_Client_Comparables(NoteInformation.lnote_id, "C")

            If Not IsNothing(AircraftDataTable) Then
                If AircraftDataTable.Rows.Count > 0 Then
                    For Each r As DataRow In AircraftDataTable.Rows
                        'clival_client_ac_id
                        CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(r("AC ID"))
                        If Not IsNothing(CurrentData) Then
                            If CurrentData.Rows.Count > 0 Then
                                AircraftData = Create_Aircraft_Class(CurrentData, "cliaircraft")
                                aclsData_Temp.Insert_Client_Comparables(NoteInformation.lnote_id, AircraftData, IIf(Not IsDBNull(CurrentData.Rows(0).Item("cliaircraft_ser_nbr_sort")), CurrentData.Rows(0).Item("cliaircraft_ser_nbr_sort"), ""), CurrentData.Rows(0).Item("cliaircraft_cliamod_id"), CurrentData.Rows(0).Item("cliamod_jetnet_amod_id"), r("clival_ac_type"))
                            End If
                        End If
                    Next
                End If
            End If

            AircraftDataTable = New DataTable
            AircraftDataTable = aclsData_Temp.Get_Client_Primary_Comparable(NoteInformation.lnote_id)

            'Remove up above:
            aclsData_Temp.Remove_Client_Comparables(NoteInformation.lnote_id, "P")

            If Not IsNothing(AircraftDataTable) Then
                If AircraftDataTable.Rows.Count > 0 Then
                    For Each r As DataRow In AircraftDataTable.Rows
                        'clival_client_ac_id
                        CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(r("AC ID"))
                        If Not IsNothing(CurrentData) Then
                            If CurrentData.Rows.Count > 0 Then
                                AircraftData = Create_Aircraft_Class(CurrentData, "cliaircraft")
                                savedAFTT = AircraftData.cliaircraft_airframe_total_hours
                                aclsData_Temp.Insert_Client_Comparables(NoteInformation.lnote_id, AircraftData, IIf(Not IsDBNull(CurrentData.Rows(0).Item("cliaircraft_ser_nbr_sort")), CurrentData.Rows(0).Item("cliaircraft_ser_nbr_sort"), ""), CurrentData.Rows(0).Item("cliaircraft_cliamod_id"), CurrentData.Rows(0).Item("cliamod_jetnet_amod_id"), r("clival_ac_type"))
                            End If
                        End If
                    Next
                End If
            End If




            Return 1
        End Function

        Public Shared Function CleanTemporaryFilesWithPrefix() As Boolean

            If String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix.ToString.Trim) Then
                Return False
            Else
                'Temporary Prefix is not empty.
                Try

                    Dim sFileNames As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath"))
                    Dim fileList As String() = System.IO.Directory.GetFiles(sFileNames, HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix.ToString + "*.*")

                    For Each f As String In fileList
                        System.IO.File.Delete(f)
                    Next

                    Return True
                Catch ex As Exception
                    Return False
                End Try

            End If
        End Function


        Public Shared Sub WriteJqueryForAircraftEditBlocks(ByVal MeRef As System.Web.UI.Page, ByRef ac_sale As RadioButtonList, ByRef ac_status_not_for_sale As DropDownList, ByRef ac_status_for_sale As DropDownList, ByRef CompareValidator1 As CompareValidator, ByRef date_listed_panel As Panel, ByRef date_listed As TextBox, ByRef DOMlisted As Label, ByRef DOMWord As Label, ByRef est_label As Label, ByRef cliaircraft_value_description_text As TextBox, ByRef est_price As TextBox, ByRef broker_price As TextBox, ByRef broker_lbl As Label, ByRef asking_price As TextBox, ByRef asking_wordage As DropDownList, ByRef ask_lbl As Label, ByRef ac_exclusive As RadioButtonList)
            Dim asking_wordageIndexChangedScript As StringBuilder = New StringBuilder()
            asking_wordageIndexChangedScript.Append(vbCrLf & " function askingWordageChange()  {")


            asking_wordageIndexChangedScript.Append(vbCrLf & " if ($('#" & asking_wordage.ClientID & "').val() == 'Price' )  {")

            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & asking_price.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & ask_lbl.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & est_label.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & est_price.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & broker_lbl.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & broker_price.ClientID & "').css('display','block');")

            asking_wordageIndexChangedScript.Append(vbCrLf & " } else { ")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & est_label.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & est_price.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & broker_lbl.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & broker_price.ClientID & "').css('display','block');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & asking_price.ClientID & "').css('display','none');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & ask_lbl.ClientID & "').css('display','none');")
            asking_wordageIndexChangedScript.Append(vbCrLf & "$('#" & asking_price.ClientID & "').val('0.00');")

            asking_wordageIndexChangedScript.Append(vbCrLf & " } ")
            asking_wordageIndexChangedScript.Append(vbCrLf & " } ")

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(MeRef, MeRef.GetType(), "askingWordage", asking_wordageIndexChangedScript.ToString, True)

            If Not MeRef.ClientScript.IsClientScriptBlockRegistered("acSaleChange") Then
                Dim acSaleChangeScript As StringBuilder = New StringBuilder()
                acSaleChangeScript.Append(vbCrLf & "  function acSaleChanged() {")

                acSaleChangeScript.Append(vbCrLf & "if($('#" & ac_sale.ClientID & " input:checked').val() == 'Y') {")
                acSaleChangeScript.Append(vbCrLf & "$('#" & CompareValidator1.ClientID & "').prop('disabled', false);")
                acSaleChangeScript.Append(vbCrLf & "$('#" & date_listed_panel.ClientID & "').css('display', 'block');")
                acSaleChangeScript.Append(vbCrLf & "acStatusLoad();")
                acSaleChangeScript.Append(vbCrLf & "$('#" & est_label.ClientID & "').css('display', 'block');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & est_price.ClientID & "').css('display', 'block');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & broker_lbl.ClientID & "').css('display', 'block');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & broker_price.ClientID & "').css('display', 'block');")
                acSaleChangeScript.Append(vbCrLf & "} else {")
                acSaleChangeScript.Append(vbCrLf & "$('#" & CompareValidator1.ClientID & "').prop('disabled', true);")
                acSaleChangeScript.Append(vbCrLf & "$('#" & date_listed_panel.ClientID & "').css('display', 'none');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & date_listed_panel.ClientID & "').val('');")
                acSaleChangeScript.Append(vbCrLf & "acStatusLoad();")
                acSaleChangeScript.Append(vbCrLf & "$('#" & asking_price.ClientID & "').val('0.00');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & est_price.ClientID & "').val('0.00');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & broker_price.ClientID & "').val('0.00');")
                acSaleChangeScript.Append(vbCrLf & "$('#" & ac_exclusive.ClientID & "').val('N');")
                acSaleChangeScript.Append(vbCrLf & "}")

                acSaleChangeScript.Append(vbCrLf & "}")

                MeRef.ClientScript.RegisterClientScriptBlock(MeRef.GetType(), "acSaleChanged()", acSaleChangeScript.ToString, True)
            End If

        End Sub

        Public Shared Function TransSetRetailFlag(ByVal SubcatCode3 As String) As String
            Dim CodeFound As Boolean = False
            TransSetRetailFlag = "N"
            Dim stringArray As String() = {"CC", "DB", "DS", "FI", "FY", "IT", "LS", "MF", "RE", "RM"}
            For Each x As String In stringArray
                If x.Equals(SubcatCode3) Then
                    CodeFound = True
                End If
            Next

            If CodeFound = False Then
                TransSetRetailFlag = "Y"
            Else
                TransSetRetailFlag = "N"
            End If

        End Function

        Public Shared Function ParseOutPasswordForDBDisplay(ByVal dbDisplayString As String) As String
            Dim database_display As Array = Split(LCase(dbDisplayString), ";password")
            Dim ReturnString As String = ""
            If UBound(database_display) > 0 Then
                ReturnString = database_display(0).ToString
            End If

            Return ReturnString
        End Function

        Public Shared Function CreateEvoHelpLink(ByVal sectionName As String, Optional returnJustURL As Boolean = False) As String
            Dim localDataLayer As New helpListsDataLayer
            Dim ResultsTable As New DataTable
            Dim URLReturn As String = ""
            Dim ReturnString As String = ""
            localDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            ResultsTable = localDataLayer.GetHelpTopicBySection(sectionName)

            If Not IsNothing(ResultsTable) Then
                If ResultsTable.Rows.Count > 0 Then
                    URLReturn = "/help.aspx?t=2&section=" & ResultsTable.Rows(0).Item("evotop_id")
                    ReturnString = "<a href=""/help.aspx?t=2&section=" & ResultsTable.Rows(0).Item("evotop_id") & """ target=""_blank"" class=""help_cursor""><img src=""images/help-circle.svg"" alt=""Help"" /></a>"
                Else
                    URLReturn = "/help.aspx?t=2"
                    ReturnString = "<a href=""/help.aspx?t=2"" target=""_blank"" class=""help_cursor""><img src=""images/help-circle.svg"" alt=""Help"" /></a>"

                End If
            End If

            If returnJustURL Then
                ReturnString = URLReturn
            End If
            Return ReturnString

        End Function

        Public Shared Function LookupDefaultFolder(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef FolderID As Long) As String
            'Hardcoded here for now.
            Dim ReturnString As String = ""
            Dim TempTable As New DataTable
            TempTable = aclsData_Temp.GetEvolutionFolderssBySubscription(FolderID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")

            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    For Each r As DataRow In TempTable.Rows
                        Dim FolderDataString As Array
                        FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

                        Select Case r("cfttpe_name").ToString
                            Case "Yacht History"
                                ReturnString = "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',true,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Yacht Events"
                                ReturnString = "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',false,true,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Market Summaries"
                                ReturnString = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,false,true,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Operating Costs"
                                ReturnString = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,true,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Performance Specs"
                                ReturnString = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',true,false,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Aircraft"
                                'Then adding them to the Aircraft Main Node:
                                ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false, false, false, '" & Replace(FolderDataString(0), "'", "\'") & "');"
                            Case "Yacht"
                                If Replace(r("cfolder_data").ToString, "yt_id=", "") <> "" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false,false,true, '" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                            Case "Company"
                                If Replace(r("cfolder_data").ToString, "comp_id=", "") <> "" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,true,false,false, '" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                            Case "Contact"
                                If Replace(r("cfolder_data").ToString, "company_contact_info=true!~!contact_id=", "") <> "" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,true,false,false, '" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                            Case "History"
                                If Replace(r("cfolder_data").ToString, "journ_id=", "") <> "" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "',true" & ",false,false, false,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                            Case "Events"
                                'Then adding them to the Event Main Node:
                                If r("cfolder_method") = "A" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "',false, true,false, false,false, " & " '" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                            Case "Wanteds"
                                If Replace(r("cfolder_data").ToString, "amwant_id=", "") <> "" Then
                                    ReturnString = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false,true,false, '" & Replace(FolderDataString(0), "'", "\'") & "');"
                                End If
                        End Select
                    Next
                End If
            End If
            Return ReturnString

        End Function


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Method name: Get_Client_Aircraft_Key_Features_As_Jetnet_Fields
        ' Purpose: to get a clients aircrfaft propeller
        ' Parameters: aircraftID
        ' Return: 
        '       DataTable
        ' Needs to be moved to clsDataManager.
        ' Change Log
        '           12/21/2015   - Created By: Amanda Vaughn
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Shared Function Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(ByVal aircraftID As Long) As DataTable
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim aTempTable As New DataTable
            Try

                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                'Building Query
                sql = " SELECT client_aircraft_key_features.cliafeat_cliac_id as afeat_ac_id, "
                sql += " client_aircraft_key_features.cliafeat_type as kfeat_type, "
                sql += " client_aircraft_key_features.cliafeat_type as kfeat_code, "
                sql += " client_aircraft_key_features.cliafeat_flag as afeat_flag, "
                sql += " client_aircraft_key_features.cliafeat_seq_nbr as afeat_seq_nbr, "
                sql += " client_aircraft_key_features.cliafeat_flag as kff_name, "
                sql += " client_key_features.clikfeat_name as kfeat_name"
                sql += " FROM client_aircraft_key_features"
                sql += " INNER JOIN client_key_features_flag ON client_aircraft_key_features.cliafeat_flag = client_key_features_flag.clikff_flag"
                sql += " INNER JOIN client_key_features ON client_aircraft_key_features.cliafeat_type = client_key_features.clikfeat_type"
                sql += " WHERE  client_aircraft_key_features.cliafeat_cliac_id = @aircraftID"
                sql += " and clikfeat_active_flag = 'Y' "
                sql += " ORDER BY client_aircraft_key_features.cliafeat_seq_nbr"

                'Writing Query to session
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(ByVal aircraftID As Long) As DataTable</b><br />" & sql

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sql, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("aircraftID", aircraftID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return aTempTable
            Catch ex As Exception
                Get_Client_Aircraft_Key_Features_As_Jetnet_Fields = Nothing
                'Me.class_error = "Error in SQL Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(ByVal aircraftID As Long) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing

            End Try

        End Function


        ''' <summary>
        ''' To be moved to the clsData layer.
        ''' </summary>
        ''' <param name="aircraftID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Aircraft_Reference_Client_acID_As_JetnetFields(ByVal aircraftID As Long) As DataTable
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim aTempTable As New DataTable
            Try
                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                sql = "SELECT client_aircraft_reference.cliacref_id AS acref_id, clicomp_zip_code as comp_zip_code, clicomp_alternate_name as comp_name_alt, clicomp_alternate_name_type as comp_name_alt_type, client_aircraft_reference.cliacref_cliac_id as acref_ac_id, "
                sql = sql & " client_aircraft_reference.cliacref_comp_id AS comp_id, client_aircraft_reference.cliacref_contact_id AS acref_contact_id, "
                sql = sql & " client_aircraft_reference.cliacref_contact_id AS contact_id, client_aircraft_reference.cliacref_contact_type AS acref_contact_type, "
                sql = sql & " client_aircraft_reference.cliacref_owner_percentage AS acref_owner_percentage, client_aircraft_reference.cliacref_jetnet_ac_id, "
                sql = sql & " client_aircraft_reference.cliacref_date_fraction_purchased AS acref_date_fraction_purchased, "
                sql = sql & " '' AS acref_fraction_expires_date,client_aircraft_reference.cliacref_comp_id as acref_comp_id, "
                sql = sql & " client_aircraft_reference.cliacref_business_type AS acref_business_type, 0 as acref_transmit_seq_no, "
                sql = sql & " client_aircraft_reference.cliacref_operator_flag AS acref_operator_flag, "
                sql = sql & " client_aircraft_reference.cliacref_jetnet_contact_type AS acref_jetnet_contact_type, "
                sql = sql & " client_aircraft_reference.cliacref_contact_priority , 'CLIENT' as source, clicontact_sirname as contact_sirname, clicontact_middle_initial as contact_middle_initial, clicontact_suffix as contact_suffix, clicontact_email_address as contact_email_address, '' as contact_phone_office, '' as contact_phone_mobile, '' as contact_phone_fax, "
                sql = sql & " client_company.clicomp_name AS comp_name, clicomp_web_address as comp_web_address, clicomp_email_address as comp_email_address, client_company.clicomp_city AS comp_city, clicomp_country AS comp_country, clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, "
                sql = sql & " client_company.clicomp_state AS comp_state, client_contact.clicontact_first_name AS contact_first_name, "
                sql = sql & " client_contact.clicontact_last_name AS contact_last_name, 0 as comp_journ_id,"
                sql = sql & " client_contact.clicontact_title AS contact_title, client_aircraft_contact_type.cliact_name AS act_name"
                sql = sql & " FROM  client_aircraft_reference left outer JOIN"
                sql = sql & " client_aircraft_contact_type ON client_aircraft_reference.cliacref_contact_type = client_aircraft_contact_type.cliact_type "
                sql = sql & " LEFT OUTER JOIN"
                sql = sql & "  client_company ON client_aircraft_reference.cliacref_comp_id = client_company.clicomp_id LEFT OUTER JOIN"
                sql = sql & " client_contact ON client_aircraft_reference.cliacref_contact_id = client_contact.clicontact_id"
                sql = sql & " WHERE (client_aircraft_reference.cliacref_cliac_id = @AircraftID)"
                sql = sql & " ORDER BY  cliacref_contact_priority, cliacref_contact_type"

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Aircraft_Reference_Client_acID_As_JetnetFields(ByVal aircraftID As Long) As DataTable</b><br />" & sql

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sql, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("aircraftID", aircraftID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return aTempTable
            Catch ex As Exception
                Get_Aircraft_Reference_Client_acID_As_JetnetFields = Nothing
                'Me.class_error = "Error in  SQL Get_Aircraft_Reference_Client_acID_As_JetnetFields(ByVal aircraftID As Long) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try



        End Function

        ''' <summary>
        ''' To be moved to the clsData layer.
        ''' </summary>
        ''' <param name="contactID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Aircraft_Contact_As_JetnetFields(ByVal contactID As Long) As DataTable
            Dim sQuery As String = ""
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim aTempTable As New DataTable
            Try
                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                sql = " clicontact_comp_id as contact_comp_id, "
                sql += " clicontact_sirname as contact_sirname, "
                sql += " clicontact_first_name as contact_first_name, "
                sql += " clicontact_last_name as contact_last_name, "
                sql += " clicontact_middle_initial as contact_middle_initial, "
                sql += " clicontact_suffix as contact_suffix, "
                sql += " clicontact_title as contact_title, "
                sql += " clicontact_email_address as contact_email_address, "
                sql += " clicontact_id as contact_id, "
                sql += " 0 as contact_journ_id "
                sQuery = "SELECT " + sql
                sQuery += " FROM Client_Contact "
                sQuery &= " WHERE clicontact_id = @contactID "

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>    Public Shared Function Get_Aircraft_Contact_As_JetnetFields(ByVal contactID As Long) As DataTable</b><br />" & sQuery

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sQuery, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("contactID", contactID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return aTempTable
            Catch ex As Exception
                Get_Aircraft_Contact_As_JetnetFields = Nothing
                'Me.class_error = "Error in  SQL  Get_Aircraft_Contact_As_JetnetFields(ByVal contactID As Long) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try



        End Function
        ''' <summary>
        ''' To be moved to the clsData layer.
        ''' </summary>
        ''' <param name="companyID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Aircraft_Company_As_JetnetFields(ByVal companyID As Long) As DataTable
            Dim sQuery As String = ""
            Dim sql As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim aTempTable As New DataTable
            Try
                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                sql = " clicomp_id as comp_id, "
                sql += " clicomp_name as comp_name, "
                sql += " clicomp_alternate_name_type as comp_name_alt_type, "
                sql += " clicomp_alternate_name as comp_name_alt, "
                sql += " clicomp_address1 as comp_address1, "
                sql += " clicomp_address2 as comp_address2, "
                sql += " clicomp_city as comp_city, "
                sql += " clicomp_state as comp_state, "
                sql += " clicomp_zip_code as comp_zip_code, "
                sql += " clicomp_country as comp_country, "
                sql += " clicomp_agency_type as comp_agency_type, "
                sql += " clicomp_web_address as comp_web_address, "
                sql += " clicomp_email_address as comp_email_address, "
                sql += " clicomp_action_date as comp_action_date, "
                sql += " clicomp_jetnet_comp_id as comp_jetnet_comp_id, "
                sql += " clicomp_user_id as comp_user_id, "
                sql += " clicomp_status as comp_status, "
                sql += " clicomp_description as comp_description, "
                sql += " clicomp_product_helicopter_flag as comp_product_helicopter_flag, "
                sql += " clicomp_product_business_flag as comp_product_business_flag, "
                sql += " clicomp_product_commercial_flag as comp_product_commercial_flag, "
                sql += " clicomp_name_search as comp_name_search, "
                sql += " '' as comp_fractowr_notes, 0 as comp_journ_id "

                sQuery = "SELECT " + sql
                sQuery += " FROM Client_Company "
                sQuery &= " INNER JOIN Client_Aircraft_Reference ON (cliacref_comp_id = clicomp_id)"
                sQuery &= " WHERE (clicomp_id = @CompanyID "



                ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users
                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Then
                    sQuery &= " AND cliacref_contact_type NOT IN ('93','98','99','71')"
                Else
                    sQuery &= " AND cliacref_contact_type NOT IN ('71')"
                End If

                sQuery &= " AND clicomp_status = 'Y')"


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Aircraft_Company_As_JetnetFields(ByVal companyID As Long, ByVal aerodex As Boolean) As DataTable</b><br />" & sQuery

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sQuery, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("CompanyID", companyID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return aTempTable
            Catch ex As Exception
                Get_Aircraft_Company_As_JetnetFields = Nothing
                'Me.class_error = "Error in  SQL Get_Aircraft_Company_As_JetnetFields(ByVal companyID As Long, ByVal aerodex As Boolean) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try



        End Function

        ''' <summary>
        ''' To be moved to the clsData layer.
        ''' </summary>
        ''' <param name="companyID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Client_Phone_As_JetnetFields(ByVal companyID As Long, ByVal contactID As Long) As DataTable
            Dim sQuery As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim aTempTable As New DataTable
            Try
                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                sQuery = "SELECT clipnum_comp_id AS pnum_comp_id, clipnum_contact_id AS pnum_contact_id, clipnum_id, "
                sQuery += " clipnum_number AS pnum_number, clipnum_number AS pnum_number_full, clipnum_type AS pnum_type, 'CLIENT' AS source FROM client_phone_numbers "
                sQuery += " INNER JOIN client_phone_type ON cliptype_name = clipnum_type"
                sQuery += " WHERE (clipnum_comp_id = @CompanyID) AND (clipnum_contact_id = @ContactID)"
                sQuery += " ORDER BY cliptype_seq_no"

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Client_Phone_As_JetnetFields(ByVal companyID As Long, ByVal contactID As Long) As DataTable</b><br />" & sQuery

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sQuery, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("CompanyID", companyID)
                MySqlCommand.Parameters.AddWithValue("ContactID", contactID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    aTempTable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return aTempTable
            Catch ex As Exception
                Get_Client_Phone_As_JetnetFields = Nothing
                'Me.class_error = "Error in  SQL Get_Client_Phone_As_JetnetFields(ByVal companyID As Long, ByVal contactID As Long) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try



        End Function
        ''' <summary>
        ''' To be moved to the clsData layer.
        ''' </summary>
        ''' <param name="aerodex"></param>
        ''' <param name="aircraftID"></param>
        ''' <param name="CompanyTypeNbr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_ContactReferences_Client_ACDetails(ByVal aircraftID As Long, ByVal companyID As Long, ByVal CompanyTypeNbr As String, ByVal aerodex As Boolean) As DataTable
            Dim sQuery As String = ""
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim atemptable As New DataTable
            Try
                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'To be changed when in clsDataManager.
                MySqlConn.Open()

                sQuery = "SELECT DISTINCT cliacref_contact_id as cref_contact_id, 0 as cref_transmit_seq_no FROM Client_Aircraft_Reference "
                sQuery &= " WHERE cliacref_cliac_id = @aircraftID"
                sQuery &= " AND cliacref_comp_id = @companyID"
                sQuery &= " AND cliacref_contact_type IN (" & CompanyTypeNbr & ")"

                ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users
                If aerodex Then
                    sQuery &= " AND cliacref_contact_type NOT IN ('93','98','99','71')"
                Else
                    sQuery &= " AND cliacref_contact_type NOT IN ('71')"
                End If

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_ContactReferences_Client_ACDetails(ByVal aircraftID As Long, ByVal companyID As Long, ByVal CompanyTypeNbr As String, ByVal aerodex As Boolean) As DataTable</b><br />" & sQuery

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sQuery, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("companyID", companyID)
                MySqlCommand.Parameters.AddWithValue("aircraftID", aircraftID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                End Try

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return atemptable

            Catch ex As Exception
                Get_ContactReferences_Client_ACDetails = Nothing
                'Me.class_error = "Error in Get_ContactReferences_Client_ACDetails(ByVal aircraftID As Long, ByVal companyID As Long, ByVal CompanyTypeNbr As String, ByVal aerodex As Boolean) As DataTable: SQL VERSION " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try

        End Function
        ''' <summary>
        ''' To be moved to the data layer.
        ''' </summary>
        ''' <param name="aircraftID"></param>
        ''' <param name="aerodex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get_Client_Transactions_as_JetnetFields(ByVal aircraftID As Long, ByVal aerodex As Boolean) As DataTable
            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
            Dim atemptable As New DataTable
            Dim excludeONOFFmarket As String = ""
            Dim sql As String = ""
            Try

                'Opening Connection.
                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")'To be changed when in clsDataManager.
                MySqlConn.Open()
                If aerodex Then excludeONOFFmarket = ",'OM','MA'"

                sql = "SELECT "
                sql += " clitrans_cliac_id as ac_id, clitrans_jetnet_trans_id as client_jetnet_trans_id, "
                sql += " clitrans_customer_note as journ_customer_note, "
                sql += " clitrans_date as journ_date, "
                sql += " clitrans_id as journ_id, "
                sql += " clitrans_jetnet_ac_id as journ_ac_id, 'CLIENT' as source, "
                sql += " clitrans_subject as journ_subject, "
                sql += " clitrans_subcategory_code as journ_subcategory_code, "
                sql += " clitcat_name as jcat_subcategory_name, "
                sql += " 'N' as jcat_auto_subject_flag "
                sql += " FROM client_transactions "
                sql += " left outer join client_transaction_category on clitcat_code = clitrans_subcategory_code "
                sql += " WHERE "
                sql += " clitrans_jetnet_ac_id = @aircraftID "
                sql += " AND clitrans_subcategory_code NOT IN ('IN','DM','EXOFF','EXON'" & excludeONOFFmarket & ")"


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Get_Client_Transactions_as_JetnetFields(ByVal aircraftID As Long, ByVal aerodex As Boolean) As DataTable</b><br />" & sql

                'Setting up Command
                Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand(sql, MySqlConn)

                'Adding Parameters.
                MySqlCommand.Parameters.AddWithValue("aircraftID", aircraftID)

                'Executing with close command after read
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                'Fill Table
                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                End Try

                atemptable.Constraints.Clear()

                'Close
                MySqlCommand.Dispose()
                MySqlCommand = Nothing

                'Return table.
                Return atemptable

            Catch ex As Exception
                Get_Client_Transactions_as_JetnetFields = Nothing
                'Me.class_error = "Error in Get_Client_Transactions_as_JetnetFields(ByVal aircraftID As Long, ByVal aerodex As Boolean) As DataTable: " & ex.Message
            Finally
                MySqlConn.Dispose()
                MySqlConn.Close()
                MySqlConn = Nothing
            End Try

        End Function

        Public Shared Function UpdateScheduleFolder(ByVal minutesVal As Long, ByVal cfolder_id As Integer, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As Integer
            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim sql As String = ""
            UpdateScheduleFolder = 0
            Try
                'make sure there's a session set

                If HttpContext.Current.Session.Item("crmUserLogon") = True Then
                    If subID <> 0 And userLogin <> "" And seqNO <> 0 And cfolder_id <> 0 Then
                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        SqlConn.Open()

                        sql = " UPDATE client_folder "
                        sql = sql & " SET  cfolder_jetnet_run_freq_in_mins = @minutesVal "
                        sql = sql & " WHERE (cfolder_id = @cfolderID and cfolder_sub_id = @subID and cfolder_login = @userLogin and cfolder_seq_no = @seqNo) "



                        Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)
                        SqlCommand.Parameters.AddWithValue("minutesVal", minutesVal)
                        SqlCommand.Parameters.AddWithValue("cfolderID", cfolder_id)
                        SqlCommand.Parameters.AddWithValue("subID", subID)
                        SqlCommand.Parameters.AddWithValue("userLogin", userLogin)
                        SqlCommand.Parameters.AddWithValue("seqNo", seqNO)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsgeneral", sql.ToString)
                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing

                        UpdateScheduleFolder = 1
                        sql = ""
                    End If
                End If

            Catch ex As Exception
                UpdateScheduleFolder = 0
                Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral.vb", ex.Message)
            Finally
                SqlConn.Close()
                SqlConn.Dispose()
            End Try
        End Function

        Public Shared Function UpdateAutoRunFlag(ByVal AutoRunFlag As String, ByVal cfolder_id As Integer, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As Integer
            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlCommand As New SqlClient.SqlCommand
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing
            Dim sql As String = ""
            UpdateAutoRunFlag = 0
            Try
                'make sure there's a session set.
                If HttpContext.Current.Session.Item("crmUserLogon") = True Then
                    If subID <> 0 And userLogin <> "" And seqNO <> 0 And cfolder_id <> 0 Then

                        sql = " UPDATE client_folder "
                        sql = sql & " SET  cfolder_jetnet_run_flag = '" & AutoRunFlag & "' "

                        If AutoRunFlag = "Y" Then
                            sql = sql & ", cfolder_jetnet_run_last_process_date = GETDATE() "
                        End If
                        sql = sql & " WHERE (cfolder_id = '" & cfolder_id & "' and cfolder_sub_id = " & subID & " and cfolder_login = '" & Trim(userLogin) & "' and cfolder_seq_no = " & seqNO & ") "

                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        SqlConn.Open()
                        SqlCommand.Connection = SqlConn

                        SqlCommand.CommandText = sql
                        SqlCommand.ExecuteNonQuery()

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral.vb", sql.ToString)

                        UpdateAutoRunFlag = 1
                        sql = ""
                    End If
                End If

            Catch ex As Exception
                UpdateAutoRunFlag = 0
                Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral.vb", ex.Message)
            Finally
                SqlCommand.Dispose()
                SqlConn.Close()
                SqlConn.Dispose()
            End Try
        End Function
        Public Shared Function RemoveProject(ByVal cfolder_id As Integer, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As Integer
            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlException As SqlClient.SqlException : SqlException = Nothing

            Dim sql As String = ""

            RemoveProject = 0
            Try
                'make sure there's a session set.
                If HttpContext.Current.Session.Item("crmUserLogon") = True Then
                    If subID <> 0 And userLogin <> "" And seqNO <> 0 And cfolder_id <> 0 Then
                        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        SqlConn.Open()
                        'That table does not really have a run flag I guess – therefore set the sissc_reply_username and sissc_reply_email equal to “NULL” instead.
                        sql = " update Subscription_Install_Saved_Search_Criteria set sissc_reply_username = NULL, sissc_reply_email = NULL "
                        sql += " where sissc_id = @projectID "
                        sql += " and sissc_sub_id  = @subID"
                        sql += " and sissc_login = @userLogin "
                        sql += " and sissc_seq_no = @seqNo "


                        Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)
                        SqlCommand.Parameters.AddWithValue("projectID", cfolder_id)
                        SqlCommand.Parameters.AddWithValue("subID", subID)
                        SqlCommand.Parameters.AddWithValue("userLogin", userLogin)
                        SqlCommand.Parameters.AddWithValue("seqNo", seqNO)

                        Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsgeneral", sql.ToString)
                        SqlCommand.ExecuteNonQuery()

                        SqlCommand.Dispose()
                        SqlCommand = Nothing


                        sql = ""
                    End If
                End If

            Catch ex As Exception
                RemoveProject = 0
                Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "clsGeneral.vb", ex.Message)
            Finally

                SqlConn.Close()
                SqlConn.Dispose()
            End Try
        End Function
        Public Shared Function FindNextPreviousButtonsCRMACDetails(ByVal BrowseTable As Label, ByVal RecordsOf As Panel, ByVal BrowseLabel As Label, ByVal CurrentRecordLabel As Label, ByVal TotalRecordLabel As Label, ByVal AircraftID As Long, ByRef PreviousACSwap As Label, ByRef NextACSwap As Label) As Boolean
            Dim ACIDNext As Long = 0
            Dim ACIDPrev As Long = 0
            Dim ACIDNextSource As String = ""
            Dim ACIDPrevSource As String = ""
            Dim SplitNext() As String
            Dim SplitPrev() As String
            Dim CurrentRecord As Long = 0
            Dim TotalRecord As Long = 0
            If Not IsNothing(HttpContext.Current.Session("my_ids")) Then
                For i = LBound(HttpContext.Current.Session("my_ids")) To UBound(HttpContext.Current.Session("my_ids"))
                    Dim session_var() As String = Split((HttpContext.Current.Session("my_ids")(i)), "|")

                    If UBound(session_var) >= 1 Then
                        TotalRecord = UBound(HttpContext.Current.Session("my_ids")) + 1
                        If session_var(0) = AircraftID Then
                            CurrentRecord = i + 1
                            'Figure out Next 
                            If UBound(HttpContext.Current.Session("my_ids")) = i Then 'No Next
                            Else
                                SplitNext = Split((HttpContext.Current.Session("my_ids")(i + 1)), "|")
                                ACIDNext = SplitNext(0)
                                ACIDNextSource = SplitNext(1)
                            End If

                            'Figure out Previous
                            If LBound(HttpContext.Current.Session("my_ids")) = i Then 'Nothing previous
                            Else
                                SplitPrev = Split((HttpContext.Current.Session("my_ids")(i - 1)), "|")
                                ACIDPrev = SplitPrev(0)
                                ACIDPrevSource = SplitPrev(1)
                            End If
                        End If
                    End If
                Next

                If ACIDPrev > 0 Then
                    PreviousACSwap.Text = "<a href=""#"" id=""previousAC"" class='gray_button float_left noBefore' value="" < Previous Aircraft"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDPrev & "" & IIf(ACIDPrevSource = "CLIENT", "&source=CLIENT", "") & "';"" tooltip = ""Click to View the Previous Aircraft"">&#9668; <strong>Previous</strong></a>"
                    PreviousACSwap.Visible = True
                Else
                    PreviousACSwap.Visible = False
                End If

                CurrentRecordLabel.Text = CurrentRecord
                TotalRecordLabel.Text = TotalRecord 'nTotalRecordCount.ToString

                If TotalRecord = 1 Then
                    BrowseLabel.Visible = False
                    BrowseTable.Text = ""
                    RecordsOf.Visible = False
                ElseIf TotalRecord = 0 Then
                    RecordsOf.Visible = False
                End If

                If ACIDNext > 0 Then
                    NextACSwap.Text = "<a href=""#"" id=""nextAC"" class='gray_button' onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDNext & "" & IIf(ACIDNextSource = "CLIENT", "&source=CLIENT", "") & "';"" value=""Next Aircraft &#9658 "" tooltip = ""Click to View the Next Aircraft""><strong>Next</strong> &#9658</a>"
                    NextACSwap.Visible = True
                Else
                    NextACSwap.Visible = False
                End If
            Else
                BrowseLabel.Visible = False
                BrowseTable.Text = ""
                RecordsOf.Visible = False
            End If

            Return True

        End Function

#Region "Dealing with Caching"
        Public Shared Sub FillCachedHelpMenu(ByRef DataLayer As clsData_Manager_SQL)
            If IsNothing(HttpContext.Current.Cache("CachedHelpMenu")) Then
                HttpContext.Current.Cache.Insert("CachedHelpMenu", LoadHelpMenu(DataLayer), Nothing, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration)
            End If
        End Sub


        ''' <summary>
        ''' This runs the queries to store the help menu into cache for the CRM
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function LoadHelpMenu(ByRef DataLayer As clsData_Manager_SQL) As DataSet
            Dim HelpMenuDataSet As New DataSet
            Dim HelpTopics As New DataTable
            Dim HelpReleases As New DataTable

            'Datatable #1 is the topics from the help menu
            HelpTopics = DataLayer.get_help_topics()
            HelpTopics.TableName = "TOPICS"

            'Datatable #2 is the releases from the help menu
            HelpReleases = DataLayer.get_latest_releases()
            HelpReleases.TableName = "RELEASES"


            'Adding to the dataset.
            HelpMenuDataSet.Tables.Add(HelpTopics)
            HelpMenuDataSet.Tables.Add(HelpReleases)

            Return HelpMenuDataSet

        End Function
#End Region

        Public Shared Function CheckForBotActivity(aclsData_Temp As clsData_Manager_SQL, pageIsPostBack As Boolean) As Boolean
            Dim returnBoolean As Boolean = True

            'If Evo Application
            If Not pageIsPostBack Then
                If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                    'If Test
                    'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                    'Check to make sure you're not on the user verification page, if you are, this doesn't need to run.
                    If Not UCase(HttpContext.Current.Request.RawUrl.ToString()).Contains("/USERVERIFICATION.ASPX") Then
                        'If Query returns row
                        Dim tempTable As New DataTable
                        tempTable = aclsData_Temp.CheckForBotActivity(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
                        If Not IsNothing(tempTable) Then
                            If tempTable.Rows.Count > 0 Then
                                If Not IsDBNull(tempTable.Rows(0).Item("tcount")) Then
                                    If tempTable.Rows(0).Item("tcount") > 0 Then
                                        Return False
                                    End If
                                End If
                            End If
                            'End If
                        End If
                    End If
                End If
            End If
            Return returnBoolean
        End Function
    End Class
End Namespace

