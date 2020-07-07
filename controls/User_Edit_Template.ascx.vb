Imports System.IO
Imports System
Imports System.Net.Mail
Partial Public Class User_Edit_Template
  Inherits System.Web.UI.UserControl
  Public Event Attention(ByVal text As String)
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
#Region "Page Loads"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try


        aclsData_Temp = New clsData_Manager_SQL

        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

        aclsData_Temp.class_error = ""


        If InStr(UCase(Application.Item("crmClientSiteData").crmClientHostName), "WWW") = 0 Then
          If UCase(Application.Item("crmClientSiteData").crmClientHostName) <> "LOCALHOST" Then
            Application.Item("crmClientSiteData").crmClientHostName = UCase("WWW." & Application.Item("crmClientSiteData").crmClientHostName)
          End If
        End If


        If Trim(Request("support")) <> "" Then
          support_email.Visible = True
          user_edit.Visible = False
          aTempTable = aclsData_Temp.Get_Client_User(CInt(Session.Item("localUser").crmLocalUserID))
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              If Not IsDBNull(aTempTable.Rows(0).Item("cliuser_first_name")) Or Not IsDBNull(aTempTable.Rows(0).Item("cliuser_last_name")) Then
                email_name.Text = aTempTable.Rows(0).Item("cliuser_first_name") & " " & aTempTable.Rows(0).Item("cliuser_last_name")
              End If
              If Not IsDBNull(Application.Item("crmClientSiteData").crmClientHostName) Then
                email_client.Text = Application.Item("crmClientSiteData").crmClientHostName
              End If
              If Not IsDBNull(aTempTable.Rows(0).Item("cliuser_email_address")) Then
                email_email.Text = aTempTable.Rows(0).Item("cliuser_email_address")
              End If
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "User_Edit_Template.ascx.vb - Page Load() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If

          End If

        ElseIf Trim(Request("support")) = "" Then
          support_email.Visible = False
          If Not Page.IsPostBack Then
            If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
              bind_users()
              '
            End If
          End If
        End If
      Catch ex As Exception
        error_string = "User_Edit_Template.ascx.vb - Page Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If

  End Sub
#End Region
#Region "Displays in Details View"
  Sub dispDetails(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Select Case (e.CommandName)
        Case "details"
          add_new_user.Visible = False
          DetailsView1.Visible = True
          'add_new_user.Visible = True
          Dim id As Integer = Convert.ToInt32(e.Item.Cells(0).Text)

          aTempTable = aclsData_Temp.Get_Client_User(id)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows
                DetailsView1.DataSource = aTempTable
                DetailsView1.ChangeMode(DetailsViewMode.Edit)
                DetailsView1.DataBind()

                If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETNETCRM.COM" Then
                Else
                  Dim demo As RadioButtonList = DetailsView1.FindControl("cliuser_admin_flag")
                  demo.Items.RemoveAt(1)
                  Dim demo_btn As ImageButton = DetailsView1.FindControl("ResetDemo")
                  Dim demo_end_date As Panel = DetailsView1.FindControl("demo_end_date")
                  demo_btn.Visible = False
                  demo_end_date.Visible = False

                End If

                Dim cliuser_time As DropDownList = (DetailsView1.FindControl("cliuser_time"))
                aTempTable = aclsData_Temp.Get_Client_Timezone()
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable.Rows
                      cliuser_time.Items.Add(New ListItem(q("clitzone_name"), q("clitzone_id")))
                    Next
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = "User_Edit_Template.ascx.vb - dispDetails() - " & aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                  End If

                End If

                cliuser_time.SelectedValue = r("cliuser_timezone")
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "User_Edit_Template.ascx.vb - dispDetails() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
          End If
      End Select
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - dispDetails() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Details View Events"


  Protected Sub DetailsView1_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewModeEventArgs) Handles DetailsView1.ModeChanging
    Try
      If e.CancelingEdit Then
        DetailsView1.Visible = False
        error_max_users.Text = ("")
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - DetailsView1_ModeChanging() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Protected Sub DetailsView1_ItemDeleting1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewDeleteEventArgs) Handles DetailsView1.ItemDeleting
    Try
      Dim userid As Integer = DetailsView1.Rows(0).Cells(1).Text
      Dim first_name As TextBox = (DetailsView1.FindControl("cliuser_first_name"))
      Dim last_name As TextBox = (DetailsView1.FindControl("cliuser_last_name"))
      Dim email_address As TextBox = (DetailsView1.FindControl("cliuser_email_address"))

      If aclsData_Temp.Update_Client_User_Active_Flag("N", userid) = 1 Then
        error_max_users.Text = ("<p align='center'>Your User has been removed.</p>")
        DetailsView1.Visible = False
        'Refill the datagrid
        bind_users()

        If Session.Item("isEVOLOGGING") Then
          Call commonLogFunctions.Log_User_Event_Data("UserDelete", " Delete User " + first_name.Text.Trim + " " + last_name.Text.Trim + "/" + email_address.Text.Trim)
        End If

      Else

        If aclsData_Temp.class_error <> "" Then
          error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemDeleting1() - " & aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If

        error_max_users.Text = ("<p align='center'>There was a problem removing your User.</p>")
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemDeleting1() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub test()
    Dim admin_flag As RadioButtonList = (DetailsView1.FindControl("cliuser_admin_flag"))
    Dim demo_end_date As Panel = (DetailsView1.FindControl("demo_end_date"))
    If admin_flag.SelectedValue = "D" Then
      demo_end_date.Visible = True
    Else
      demo_end_date.Visible = False
    End If
  End Sub
  Protected Sub DetailsView1_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdateEventArgs) Handles DetailsView1.ItemUpdating
    Try
      Dim first_name As TextBox = (DetailsView1.FindControl("cliuser_first_name"))
      Dim last_name As TextBox = (DetailsView1.FindControl("cliuser_last_name"))
      Dim login As TextBox = (DetailsView1.FindControl("cliuser_login"))
      Dim password As TextBox = (DetailsView1.FindControl("cliuser_password"))
      Dim admin_flag As RadioButtonList = (DetailsView1.FindControl("cliuser_admin_flag"))

      Dim admin_flag_value As String


      admin_flag_value = admin_flag.SelectedValue


      Dim email_address As TextBox = (DetailsView1.FindControl("cliuser_email_address"))
      Dim timezone As DropDownList = (DetailsView1.FindControl("cliuser_time"))
      Dim demo_time As TextBox = (DetailsView1.FindControl("demo_time"))
      Dim userid As Integer = DetailsView1.Rows(0).Cells(1).Text
      Dim test_date As New Nullable(Of System.DateTime)
      If demo_time.Text <> "" Then
        test_date = demo_time.Text
      End If
      If aclsData_Temp.Update_Client_User(first_name.Text, last_name.Text, login.Text, password.Text, admin_flag_value, email_address.Text, Now(), userid, timezone.SelectedValue, test_date, userid) = 1 Then
        error_max_users.Text = ("<p align='center'>Your User has been saved.</p>")
        'refill datagrid
        bind_users()
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemUpdating() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
        error_max_users.Text = ("<p align='center'>There was a problem saving your User.</p>")
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemUpdating() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub AddUser(ByVal addUser As Boolean, ByVal updateUser As Boolean)
    Dim cliuserExistingID As TextBox = DetailsView1.FindControl("cliuserExistingID")
    Dim first_name As TextBox = (DetailsView1.FindControl("cliuser_first_name"))
    Dim last_name As TextBox = (DetailsView1.FindControl("cliuser_last_name"))
    Dim login As TextBox = (DetailsView1.FindControl("cliuser_login"))
    Dim password As TextBox = (DetailsView1.FindControl("cliuser_password"))
    Dim admin_flag As RadioButtonList = (DetailsView1.FindControl("cliuser_admin_flag"))

    Dim admin_flag_value As String

    admin_flag_value = admin_flag.SelectedValue


    Dim email_address As TextBox = (DetailsView1.FindControl("cliuser_email_address"))
    Dim timezone As DropDownList = (DetailsView1.FindControl("cliuser_time"))
    Dim demo_time As TextBox = (DetailsView1.FindControl("demo_time"))
    Dim test_date As New Nullable(Of System.DateTime)
    If demo_time.Text <> "" Then
      test_date = demo_time.Text
    End If

    If addUser Then
      Dim return_id = aclsData_Temp.Insert_Client_User_Return(first_name.Text, last_name.Text, login.Text, password.Text, admin_flag_value, email_address.Text, Now(), CInt(Session.Item("localUser").crmLocalUserID), timezone.SelectedValue, test_date)
      If return_id <> 0 Then
        Dim BodyText As String = ""
        BodyText = "<p>This email is to alert JETNET technical staff that MPM client at " & Replace(UCase(Application.Item("crmClientSiteData").crmClientHostName), "WWW.", "") & " has added a new user. </p>"
        BodyText += "<strong>User Details are as follows:</strong><br />"
        BodyText += "<table cellpadding=""3"" cellspacing=""0"">"
        BodyText += "<tr>"
        BodyText += "<td align=""left"" width=""160"">Email Address:</td><td align=""left"">" & email_address.Text & "</td>"
        BodyText += "</tr>"
        BodyText += "<tr>"
        BodyText += "<td align=""left"">First Name:</td><td align=""left"">" & first_name.Text & "</td>"
        BodyText += "</tr>"
        BodyText += "<tr>"
        BodyText += "<td align=""left"">Last Name:</td><td align=""left"">" & last_name.Text & "</td>"
        BodyText += "</tr>"
        BodyText += "</table>"
        aclsData_Temp.InsertCRMMailQueue("Evolution", Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmLocalUserEmailAddress, "smtp.jetnet.com", "customerservice@jetnet.com", "cservice123", "stephanie@jetnet.com", "", "", "New User Has been Added to " & Replace(UCase(Application.Item("crmClientSiteData").crmClientHostName), "WWW.", "") & " MPM.", BodyText, Session.Item("localUser").crmUserCompanyID, Session.Item("localSubscription").crmSubscriptionID, "MPM", Replace(UCase(Application.Item("crmClientSiteData").crmClientHostName), "WWW", ""))

        If aclsData_Temp.Update_Client_User(first_name.Text, last_name.Text, login.Text, password.Text, admin_flag_value, email_address.Text, Now(), return_id, timezone.SelectedValue, test_date, return_id) = 1 Then
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = "User_Edit_Template.ascx.vb - AddUser(Add) - " & aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
          End If
          error_max_users.Text = ("<p align='center'>There was a problem adding your User.</p>")
        End If

        DetailsView1.Visible = False
        DetailsView1.ChangeMode(DetailsViewMode.Insert)
        DetailsView1.DataBind()
        bind_users()

        If Session.Item("isEVOLOGGING") Then
          Call commonLogFunctions.Log_User_Event_Data("UserAdd", " Added User " + first_name.Text.Trim + " " + last_name.Text.Trim + "/" + email_address.Text.Trim)
        End If

        error_max_users.Text = ("<p align='center'>Your User has been added.</p>")

      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "User_Edit_Template.ascx.vb - AddUser(Add) - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
        error_max_users.Text = ("<p align='center'>There was a problem adding your User.</p>")
      End If
    ElseIf updateUser Then
      Dim return_id As Long = 0

      If Not IsNothing(cliuserExistingID) Then
        return_id = cliuserExistingID.Text

        If return_id <> 0 Then
          If aclsData_Temp.Update_Client_User_Active_Flag("Y", return_id) = 1 Then
            If aclsData_Temp.Update_Client_User(first_name.Text, last_name.Text, login.Text, password.Text, admin_flag_value, email_address.Text, Now(), return_id, timezone.SelectedValue, test_date, return_id) = 1 Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = "User_Edit_Template.ascx.vb - AddUser (Update) - " & aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
              End If
              error_max_users.Text = ("<p align='center'>There was a problem updating your User.</p>")
            End If


            DetailsView1.Visible = False
            DetailsView1.ChangeMode(DetailsViewMode.Insert)
            DetailsView1.DataBind()
            bind_users()

            If Session.Item("isEVOLOGGING") Then
              Call commonLogFunctions.Log_User_Event_Data("UserUpdate", " Updated User " + first_name.Text.Trim + " " + last_name.Text.Trim + "/" + email_address.Text.Trim)
            End If

            error_max_users.Text = ("<p align='center'>Your User has been updated.</p>")

          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "User_Edit_Template.ascx.vb - AddUser (Update) - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
            error_max_users.Text = ("<p align='center'>There was a problem updating your User.</p>")
          End If

        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = "User_Edit_Template.ascx.vb - AddUser (Update) - " & aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
          End If
          error_max_users.Text = ("<p align='center'>There was a problem updating your User.</p>")
        End If
      End If

    End If
  End Sub

  ''' <summary>
  ''' ''' Okay button on Modal Popup. This is what fires when you click it. 
  ''' </summary>
  Public Sub Okay_Button_ModalPopup() 'Handles OkButton.Click

    MPE.Hide()
    AddUser(False, True)
  End Sub

  Protected Sub DetailsView1_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertEventArgs) Handles DetailsView1.ItemInserting
    Try
      Dim login As TextBox = (DetailsView1.FindControl("cliuser_login"))
      Dim password As TextBox = (DetailsView1.FindControl("cliuser_password"))
      Dim cpassword As TextBox = (DetailsView1.FindControl("cliuser_confirm"))
      Dim cliuserExistingID As TextBox = DetailsView1.FindControl("cliuserExistingID")

      password.Attributes.Add("value", password.Text)
      cpassword.Attributes.Add("value", cpassword.Text)

      If Not IsNothing(login) Then

        If Not String.IsNullOrEmpty(login.Text) Then
          Dim UserReturn As New DataTable
          UserReturn = Get_Client_User_By_UserLogin(login.Text)
          If Not IsNothing(UserReturn) Then
            If UserReturn.Rows.Count > 0 Then
              MPE.Show()
              cliuserExistingID.Text = UserReturn.Rows(0).Item("cliuser_id")
            Else 'insert as normal
              AddUser(True, False)
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemInserting() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
            error_max_users.Text = ("<p align='center'>There was a problem saving your User.</p>")
          End If
        End If
      End If

    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - DetailsView1_ItemInserting() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub


  ''' <summary>
  ''' Searches users for user login to see if a user with this login exists.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function Get_Client_User_By_UserLogin(ByVal cliuser_login As String) As DataTable
    Dim sql As String = ""
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
    Dim aTempTable As New DataTable

    Try
      sql = "SELECT cliuser_action_date, cliuser_active_flag, cliuser_admin_flag, cliuser_email_address, cliuser_end_date, cliuser_first_name, cliuser_id, cliuser_last_login, cliuser_last_login_date, cliuser_last_logout_date, cliuser_last_name, cliuser_last_session_date, cliuser_loggedin_flag, cliuser_login, cliuser_password, cliuser_timezone, cliuser_user_id FROM client_user WHERE (cliuser_login = '" & cliuser_login & "' and (cliuser_password is NULL or cliuser_active_flag = 'N'))"

      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sql


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Client_User_By_UserLogin(ByVal cliuser_login As String) As DataTable</b><br />" & sql


      MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


      Try
        aTempTable.Load(MySqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
      End Try

      Return aTempTable
    Catch ex As Exception
      Get_Client_User_By_UserLogin = Nothing
      aclsData_Temp.class_error = "Error in Get_Client_User_By_UserLogin(ByVal cliuser_login As String) As DataTable: " & ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br />Error in Get_Client_User_By_UserLogin(ByVal cliuser_login As String) As DataTable: " & ex.Message
    Finally
      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing
    End Try



  End Function


#End Region



  Sub bind_users()
    error_max_users.Text = ""
    aTempTable = aclsData_Temp.Get_AllClientUser_Active("Y")
    If Not IsNothing(aTempTable) Then
      'Response.Write(aTempTable.Rows.Count & "!!!!")
      If CInt(Session.Item("localSubscription").crmMaxUserCount) <= aTempTable.Rows.Count Then
        add_new_user.Visible = False
        error_max_users.Text = "<p align='center'>Your maximum amount of users (" & CInt(Session.Item("localSubscription").crmMaxUserCount) & ") has been reached. Please contact Jetnet to add more users.</p>"
      Else
        add_new_user.Visible = True
        error_max_users.Text = ""
      End If

      If aTempTable.Rows.Count > 0 Then

        Datagrid1.DataSource = aTempTable
        Datagrid1.DataBind()

        If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETNETCRM.COM" Then
          Datagrid1.Columns(5).Visible = True
        Else
          Datagrid1.Columns(5).Visible = False
        End If

      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "User_Edit_Template.ascx.vb - Page Load() - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End If
    End If
  End Sub

  Public Function demo_expiration(ByVal x As Object, ByVal y As Object) As String
    demo_expiration = ""
    Try

      If Not IsDBNull(x) And Not IsDBNull(y) Then
        If FormatDateTime(Now(), 2) <= FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(y)), 2) And FormatDateTime(Now(), 2) >= FormatDateTime(DateAdd(DateInterval.Day, -1, CDate(y)), 2) Then
          demo_expiration = "<span class='green'>&#10004</span>"
        Else
          demo_expiration = "<span class='red'>&ndash;</span>"
        End If
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - demo_expiration() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Private Sub add_new_user_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_new_user.Click
    Try
      DetailsView1.Visible = True
      DetailsView1.ChangeMode(DetailsViewMode.Insert)
      add_new_user.Visible = False
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - add_new_user_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Function set_demo_flags() As String
    set_demo_flags = ""
    Try
      If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETNETCRM.COM" Then
      Else
        Dim demo As RadioButtonList = DetailsView1.FindControl("cliuser_admin_flag")
        demo.Items.RemoveAt(1)
        Dim demo_btn As ImageButton = DetailsView1.FindControl("ResetDemo")
        demo_btn.Visible = False
        Dim demo_end_date As Panel = DetailsView1.FindControl("demo_end_date")
        demo_end_date.Visible = False

      End If
      Dim cliuser_time As DropDownList = (DetailsView1.FindControl("cliuser_time"))
      aTempTable = aclsData_Temp.Get_Client_Timezone()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            cliuser_time.Items.Add(New ListItem(q("clitzone_name"), q("clitzone_id")))
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "User_Edit_Template.ascx.vb - set_demo_flags() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - set_demo_flags() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Function whatTimeZone(ByVal x As Object) As String
    whatTimeZone = ""
    Try
      If Not IsDBNull(x) Then
        If IsNumeric(x) Then
          aTempTable = aclsData_Temp.Get_Client_Timezone(CInt(x))
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows
                whatTimeZone = r("clitzone_name")
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "User_Edit_Template.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - whatTimeZone() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function

  Public Sub SendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String)
    Try
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

    Catch ex As Exception
      error_string = "user_edit_template.aspx.vb - SendMailMessage() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try

  End Sub

  Private Sub submit_email_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles submit_email.Click
    Try

      Dim Bcc As String = ""
      Dim Cc As String = ""
      Dim Subject As String = "JETNET CRM Support Request – " & email_client.Text
      Dim Body As String = ""

      Body = "<html>"
      Body = Body & "<head></head>"
      Body = Body & "<body>"
      Body = Body & "<table width='500' cellpadding='3' cellspacing='0'>"
      Body = Body & "<tr><td align='left' valign='top'>Name:</td><td align='left' valign='top'>"
      Body = Body & email_name.Text & "</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'>Email:</td><td align='left' valign='top'>"
      Body = Body & email_email.Text & "</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'>Phone:</td><td align='left' valign='top'>"
      Body = Body & email_phone.Text & "</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'>Client:</td><td align='left' valign='top'>"
      Body = Body & email_client.Text & "</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'>Description:</td><td align='left' valign='top'>"
      Body = Body & email_description.Text & "</td></tr>"
      Body = Body & "</table>"
      Body = Body & "</body>"
      Body = Body & "</html>"
      Dim From As String = "support@aerowebtech.com"
      Dim Recepient As String = "support@aerowebtech.com"
      If email_email.Text <> "" Then
        From = email_email.Text
      End If


      SendMailMessage(From, Recepient, Bcc, Cc, Subject, Body)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

    Catch ex As Exception
      error_string = "User_Edit_Template.ascx.vb - Submit_Email_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
End Class
