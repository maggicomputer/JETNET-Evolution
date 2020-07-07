Partial Public Class myCRM
  Inherits System.Web.UI.Page
  Dim aTempTable As New DataTable
  Dim error_string As String
  Public aclsData_Temp As New clsData_Manager_SQL
  Private lasset As New ArrayList()
  Private lsubordinate As New ArrayList()
  Dim bHasNoBlankAcFieldsCookie As Boolean = False
  Dim bShowBlankAcFields As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim picture_string As String = ""

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
      aclsData_Temp.class_error = ""

      'Fill My Account Information!!

      'check active tab
      If Not Page.IsPostBack Then
        If Not IsNothing(Trim(Request("tab"))) Then
          If Trim(Request("tab")) = "support" Then
            tab_container_ID.ActiveTab = my_support
          End If
        End If
        'CRM Background image?

      End If

      bShowBlankAcFields = commonEvo.getUserShowBlankACFields(Session.Item("ShowCondensedAcFormat"), bHasNoBlankAcFieldsCookie)

      'crmLocalUserName
      'Subscriber User Name
      If Not IsDBNull(Session.Item("localUser").crmLocalUserName) Then
        subscription_username.Text = "User Name: <em>" & Session.Item("localUser").crmLocalUserName & "</em>"
        'supportinfo_subscriber_information_username.Text = Session.Item("localUser").crmLocalUserName
      End If

      'Subscriber First Name/Last Name
      If Not IsDBNull(Session.Item("localUser").crmLocalUserFirstName) And Not IsDBNull(Session.Item("localUser").crmLocalUserLastName) Then
        actinfo_contact_name.Text = Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName
      End If

      'Subscriber Email Address
      If Not IsDBNull(Session.Item("localUser").crmLocalUserEmailAddress) Then
        subscription_email.Text = "<a href='mailto:" & Session.Item("localUser").crmLocalUserEmailAddress & "'>" & Session.Item("localUser").crmLocalUserEmailAddress & "</a>"
        actinfo_subscriber_information_email.Text = Session.Item("localUser").crmLocalUserEmailAddress
      End If

      'Subscriber has a demo account?
      If Session.Item("localUser").crmUserType = eUserTypes.GUEST Then
        subscription_demo_account.Text = "Demo Account: <em>True</em>"
      Else

        password_txt.Attributes.Add("onblur", "validatePassword();")
        add_ValidatePassword_Script(password_txt)

        subscription_demo_account.Text = "Demo Account: <em>False</em>"
      End If

      'client name
      If Not IsDBNull(Application.Item("crmClientSiteData").crmClientHostName) Then
        actinfo_client_name.Text = Application.Item("crmClientSiteData").crmClientHostName
      End If

      'Subscriber has an admin account?
      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        adminLink.Visible = True
        subscription_admin_account.Text = "Administrator Account: <em>True</em>"
        model_toggle.Enabled = True
        maximum_export.Enabled = True
      Else
        subscription_admin_account.Text = "Administrator Account: <em>False</em>"
        'model_attention.Text = "<p align='center' class='attention'>This is only accessible for Administrators.</p>"
        model_toggle.Enabled = True
        aircraft_preference_toggle.Enabled = False
        maximum_export.Enabled = False
        preference_toggle.Enabled = False
        market_time.Enabled = False
      End If

      If Session.Item("localSubscription").crmBusiness_Flag Then
        subscription_business.Text = "Business: <em>True</em>"
      Else
        subscription_business.Text = "Business: <em>False</em>"
      End If

      If Session.Item("localSubscription").crmCommercial_Flag Then
        subscription_commercial.Text = "Commercial: <em>True</em>"
      Else
        subscription_commercial.Text = "Commercial: <em>False</em>"
      End If

      If Session.Item("localSubscription").crmHelicopter_Flag Then
        subscription_commercial.Text = "Helicopter: <em>True</em>"
      Else
        subscription_commercial.Text = "Helicopter: <em>False</em>"
      End If


      If Not IsDBNull(Session.Item("localSubscription").crmTierlevel) Then
        subscription_tier.Text = "Tier Levels: <em>" & Session.Item("localSubscription").crmTierlevel & "</em>"
      End If

      'Fill the changeable, only on !post back.
      If Not Page.IsPostBack Then
        fill_Timezones() ' fill timezones
        fill_Models() ' fill models
        Fill_Preferences() 'fill market model, fill company categories, fill market default days
        fill_feature_code() 'fill feature codes

        If Not IsDBNull(Session("timezone")) Then
          If IsNumeric(Session("timezone")) Then
            actinfo_timezone.SelectedValue = Session("timezone")
          End If
        End If

        'Filling up display blank fields cookie
        If bShowBlankAcFields Then
          display_no_blank_fields_on_aircraft_ddl.SelectedValue = "EF"
        Else
          display_no_blank_fields_on_aircraft_ddl.SelectedValue = "CF"
        End If

        aTempTable = aclsData_Temp.GetRepresentative(Session.Item("localUser").crmUserCompanyID)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            If Not IsDBNull(aTempTable.Rows(0).Item("user_first_name")) Then
              jetnet_rep_name.Text = aTempTable.Rows(0).Item("user_first_name")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_middle_initial")) Then
              jetnet_rep_name.Text = jetnet_rep_name.Text & " " & aTempTable.Rows(0).Item("user_middle_initial")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_last_name")) Then
              jetnet_rep_name.Text = jetnet_rep_name.Text & " " & aTempTable.Rows(0).Item("user_last_name")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_phone_no")) Then
              jetnet_rep_phone.Text = aTempTable.Rows(0).Item("user_phone_no")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_phone_no_ext")) Then
              jetnet_rep_phone.Text = jetnet_rep_phone.Text & " Ext. " & aTempTable.Rows(0).Item("user_phone_no_ext")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_email_address")) Then
              jetnet_rep_email.Text = "<a href='mailto:" & aTempTable.Rows(0).Item("user_email_address") & "'>" & aTempTable.Rows(0).Item("user_email_address") & "</a>"
            End If

            picture_string = "/pictures/accountrep/"

            'If Not IsDBNull(aTempTable.Rows(0).Item("user_first_name")) Then
            '    picture_string = picture_string & Left(aTempTable.Rows(0).Item("user_first_name"), 1)
            'End If

            'If Not IsDBNull(aTempTable.Rows(0).Item("user_middle_initial")) Then
            '    picture_string = picture_string & Left(aTempTable.Rows(0).Item("user_middle_initial"), 1)
            'End If

            If Not IsDBNull(aTempTable.Rows(0).Item("user_id")) Then
              picture_string = picture_string & aTempTable.Rows(0).Item("user_id")
            End If

            picture_string = picture_string & ".jpg"
            jetnet_rep.ImageUrl = picture_string
          End If
        End If

        mydisplay_records_per_page_txt.Text = "25" 'default to 25
        If Not IsDBNull(Session.Item("localUser").crmUserRecsPerPage) Then
          If IsNumeric(Session.Item("localUser").crmUserRecsPerPage) Then
            If (Session.Item("localUser").crmUserRecsPerPage) <> 0 Then
              mydisplay_records_per_page_txt.Text = Session.Item("localUser").crmUserRecsPerPage
            End If
          End If
        End If

        types_of_owners.SelectedValue = "All Owners" 'default to all owners
        If Not IsDBNull(Session.Item("localUser").crmUserAircraftRelationship) Then
          If Session.Item("localUser").crmUserAircraftRelationship <> "" Then
            types_of_owners.SelectedValue = Session.Item("localUser").crmUserAircraftRelationship
          End If
        End If


        If Session.Item("localUser").crmUser_Autolog_Flag Then
          automaticNoteLog.Checked = True
        Else
          automaticNoteLog.Checked = False
        End If


        maximum_records_export.Text = "0" 'default to 0.
        If Not IsDBNull(Session.Item("localUser").crmMaxClientExport) Then
          If IsNumeric(Session.Item("localUser").crmMaxClientExport) Then
            If Session.Item("localUser").crmMaxClientExport <> 0 Then
              maximum_records_export.Text = Session.Item("localUser").crmMaxClientExport
            End If
          End If
        End If
      End If
    End If

    actinfo_password_mouseover_img.AlternateText = "Change Subscriber Password:" + vbCrLf + vbCrLf + "New password should be a minimum of 8 characters " + vbCrLf + _
                                               "must contain at least one number and one character" + vbCrLf + vbCrLf + "All characters will be stored in lower case"
    actinfo_password_mouseover_img.ToolTip = actinfo_password_mouseover_img.AlternateText

  End Sub

  Public Sub add_ValidatePassword_Script(ByVal tbSource As TextBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("cvp-tb-onblur") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function validatePassword() {")
      sScptStr.Append(vbCrLf & "    var txttext = document.getElementById(""" + tbSource.ClientID.ToString + """).value;")
      sScptStr.Append(vbCrLf & "    var regex = /^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,15}$/;")

      sScptStr.Append(vbCrLf & "    if (eval(regex.test(txttext)) == false && txttext != """") {")
      sScptStr.Append(vbCrLf & "      alert('Your new password should be a minimum of 8 characters in length and must contain at least ""one number"" and ""one character""');")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + tbSource.ClientID.ToString + """).focus();")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cvp-tb-onblur", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Sub run_value_label(ByVal sender As Object, ByVal e As EventArgs)
    Try

      If Me.mydisplay_value_format.SelectedValue = "T" Then ' thousands
        Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000"
      ElseIf Me.mydisplay_value_format.SelectedValue = "M" Then ' millions
        Me.format_label.Text = "i.e. 10,000,000 displayed as 10M"
      ElseIf Me.mydisplay_value_format.SelectedValue = "F" Then ' full number
        Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000,000"
      Else
        Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000"
      End If



    Catch ex As Exception

    End Try
  End Sub

#Region "Model Commands - Market Models"
  Public Sub fill_Models()
    Try
      'Changed 10/27/10 by Amanda to Take into Consideration Flags.
      aTempTable = aclsData_Temp.Get_Combination_Models(Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
      ' check the state of the DataTable
      Dim val As String = ""
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each r As DataRow In aTempTable.Rows
            val = r("amod_id") & "|" & r("amod_make_name") & "|" & r("amod_model_name") & "|" & r("source") & "|" & r("client_id")

            market_pref_models.Items.Add(New ListItem(CStr(r("amod_make_name") & " " & r("amod_model_name")), val))
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "myCRM.aspx.vb - fill_Models() - " & aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If

      End If
    Catch ex As Exception
      error_string = "myCRM.aspx.vb - fill_Models() market Model Dropdown Filling - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub AddBtn_Click()
    Try
      clsGeneral.clsGeneral.AddBtn_Click(selected_models, market_pref_models)
      'If market_pref_models.SelectedIndex >= 0 Then
      '    Dim i As Integer
      '    For i = 0 To market_pref_models.Items.Count - 1
      '        If market_pref_models.Items(i).Selected Then
      '            If Not lasset.Contains(market_pref_models.Items(i)) Then
      '                lasset.Add(market_pref_models.Items(i))
      '            End If
      '        End If
      '    Next i
      '    Dim fiel As New ListItem
      '    For i = 0 To lasset.Count - 1
      '        If Not selected_models.Items.Contains(CType(lasset(i), ListItem)) Then
      '            selected_models.Items.Add(CType(lasset(i), ListItem))
      '            fiel = CType(lasset(i), ListItem)
      '        End If
      '        market_pref_models.Items.Remove(CType(lasset(i), ListItem))
      '    Next i
      'End If
    Catch ex As Exception
      error_string = "myCRM.aspx.vb -  UpdateFields() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub AddAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    Try
      clsGeneral.clsGeneral.AddAllBtn_Click(Src, E, selected_models, market_pref_models)
      'While market_pref_models.Items.Count <> 0
      '    Dim i As Integer
      '    For i = 0 To market_pref_models.Items.Count - 1
      '        If Not lasset.Contains(market_pref_models.Items(i)) Then
      '            lasset.Add(market_pref_models.Items(i))
      '        End If
      '    Next i
      '    For i = 0 To lasset.Count - 1
      '        If Not selected_models.Items.Contains(CType(lasset(i), ListItem)) Then
      '            selected_models.Items.Add(CType(lasset(i), ListItem))
      '        End If
      '        market_pref_models.Items.Remove(CType(lasset(i), ListItem))
      '    Next i
      'End While
    Catch ex As Exception
      error_string = "myCRM.aspx.vb -  AddAllBtn_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub RemoveBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    Try
      clsGeneral.clsGeneral.RemoveBtn_Click(Src, E, selected_models, market_pref_models)
      'If Not (selected_models.SelectedItem Is Nothing) Then
      '    Dim i As Integer
      '    For i = 0 To selected_models.Items.Count - 1
      '        If selected_models.Items(i).Selected Then
      '            If Not lsubordinate.Contains(selected_models.Items(i)) Then
      '                lsubordinate.Add(selected_models.Items(i))
      '            End If
      '        End If
      '    Next i
      '    Dim fiel As New ListItem
      '    For i = 0 To lsubordinate.Count - 1
      '        If Not market_pref_models.Items.Contains(CType(lsubordinate(i), ListItem)) Then
      '            market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
      '            fiel = CType(lsubordinate(i), ListItem)
      '        End If
      '        selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
      '        fiel = CType(lsubordinate(i), ListItem)


      '        lasset.Add(lsubordinate(i))
      '        market_pref_models.SelectedValue = fiel.Value
      '    Next i
      'End If
    Catch ex As Exception
      error_string = "myCRM.aspx.vb - RemoveBtn_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub RemoveAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    Try
      clsGeneral.clsGeneral.RemoveAllBtn_Click(Src, E, selected_models, market_pref_models)
      'While selected_models.Items.Count <> 0
      '    Dim i As Integer
      '    For i = 0 To selected_models.Items.Count - 1
      '        If Not lsubordinate.Contains(selected_models.Items(i)) Then
      '            lsubordinate.Add(selected_models.Items(i))
      '        End If
      '    Next i
      '    Dim fiel As New ListItem
      '    For i = 0 To lsubordinate.Count - 1
      '        If Not market_pref_models.Items.Contains(CType(lsubordinate(i), ListItem)) Then
      '            market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
      '            fiel = CType(lsubordinate(i), ListItem)
      '        End If
      '        selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
      '        lasset.Add(lsubordinate(i))
      '    Next i
      'End While
    Catch ex As Exception
      error_string = "myCRM.aspx.vb - RemoveAllBtn_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  '' ---- ButtonMoveUp_Click --------------------------
  ''
  '' Move listbox item up one

  'Protected Sub ButtonMoveUp_Click(ByVal sender As Object, ByVal e As EventArgs)
  '    Try
  '        Dim SelectedIndex As Integer = selected_models.SelectedIndex

  '        If SelectedIndex = -1 Then
  '            ' nothing selected
  '            Return
  '        End If
  '        If SelectedIndex = 0 Then
  '            ' already at top of list  
  '            Return
  '        End If

  '        Dim Temp As ListItem
  '        Temp = selected_models.SelectedItem

  '        selected_models.Items.Remove(selected_models.SelectedItem)
  '        selected_models.Items.Insert(SelectedIndex - 1, Temp)
  '    Catch ex As Exception
  '        error_string = "myCRM.aspx.vb - ButtonMoveUp_Click() - " & ex.Message
  '        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
  '    End Try
  'End Sub

  '' ---- ButtonMoveDown_Click -------------------------------
  ''
  '' Move listbox item down one

  'Protected Sub ButtonMoveDown_Click(ByVal sender As Object, ByVal e As EventArgs)
  '    Try
  '        Dim SelectedIndex As Integer = selected_models.SelectedIndex

  '        If SelectedIndex = -1 Then
  '            ' nothing selected
  '            Return
  '        End If
  '        If SelectedIndex = selected_models.Items.Count - 1 Then
  '            ' already at top of list            
  '            Return
  '        End If

  '        Dim Temp As ListItem
  '        Temp = selected_models.SelectedItem

  '        selected_models.Items.Remove(selected_models.SelectedItem)
  '        selected_models.Items.Insert(SelectedIndex + 1, Temp)
  '    Catch ex As Exception
  '        error_string = "myCRM.aspx.vb - ButtonMoveDown_Click() - " & ex.Message
  '        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
  '    End Try
  'End Sub
#End Region
#Region "Timezones"
  Public Sub fill_Timezones()
    aTempTable = aclsData_Temp.Get_Client_Timezone()
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          actinfo_timezone.Items.Add(New ListItem(r(1), r(0)))
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "myCRM.aspx.vb - Page_Load() - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End If
    End If
  End Sub
#End Region
#Region "Fill Preferences"
  Public Sub Fill_Preferences()

    aTempTable = aclsData_Temp.Get_Client_Preferences()
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          'Aircraft Preferences.
          ac_category_1.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), ""))
          ac_category_2.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), ""))
          ac_category_3.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), ""))
          ac_category_4.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), ""))
          ac_category_5.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), ""))
          ac_category_6.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), ""))
          ac_category_7.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), ""))
          ac_category_8.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), ""))
          ac_category_9.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), ""))
          ac_category_10.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), ""))

          If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
            If r("clipref_ac_custom_1_use") = "Y" Then
              ac_category_1_use.Checked = True
            Else
              ac_category_1_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
            If r("clipref_ac_custom_2_use") = "Y" Then
              ac_category_2_use.Checked = True
            Else
              ac_category_2_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
            If r("clipref_ac_custom_3_use") = "Y" Then
              ac_category_3_use.Checked = True
            Else
              ac_category_3_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
            If r("clipref_ac_custom_4_use") = "Y" Then
              ac_category_4_use.Checked = True
            Else
              ac_category_4_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
            If r("clipref_ac_custom_5_use") = "Y" Then
              ac_category_5_use.Checked = True
            Else
              ac_category_5_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
            If r("clipref_ac_custom_6_use") = "Y" Then
              ac_category_6_use.Checked = True
            Else
              ac_category_6_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
            If r("clipref_ac_custom_7_use") = "Y" Then
              ac_category_7_use.Checked = True
            Else
              ac_category_7_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
            If r("clipref_ac_custom_8_use") = "Y" Then
              ac_category_8_use.Checked = True
            Else
              ac_category_8_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
            If r("clipref_ac_custom_9_use") = "Y" Then
              ac_category_9_use.Checked = True
            Else
              ac_category_9_use.Checked = False
            End If
          End If
          If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
            If r("clipref_ac_custom_10_use") = "Y" Then
              ac_category_10_use.Checked = True
            Else
              ac_category_10_use.Checked = False
            End If
          End If


          'Company Preferences
          pref_1.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name"), ""))
          pref_2.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name"), ""))
          pref_3.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name"), ""))
          pref_4.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name"), ""))
          pref_5.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name"), ""))

          pref_id.Text = CStr(IIf(Not IsDBNull(r("clipref_id")), r("clipref_id"), ""))

          If Not IsDBNull(r("clipref_category1_use")) Then
            If r("clipref_category1_use") = "Y" Then
              pref_1_use.Checked = True
            Else
              pref_1_use.Checked = False
            End If
          End If

          If Not IsDBNull(r("clipref_category2_use")) Then
            If r("clipref_category2_use") = "Y" Then
              pref_2_use.Checked = True
            Else
              pref_2_use.Checked = False
            End If
          End If

          If Not IsDBNull(r("clipref_category3_use")) Then
            If r("clipref_category3_use") = "Y" Then
              pref_3_use.Checked = True
            Else
              pref_3_use.Checked = False
            End If
          End If

          If Not IsDBNull(r("clipref_category4_use")) Then
            If r("clipref_category4_use") = "Y" Then
              pref_4_use.Checked = True
            Else
              pref_4_use.Checked = False
            End If
          End If

          If Not IsDBNull(r("clipref_category5_use")) Then
            If r("clipref_category5_use") = "Y" Then
              pref_5_use.Checked = True
            Else
              pref_5_use.Checked = False
            End If
          End If


          If Not IsDBNull(Session.Item("localUser").crmUserDefaultModels) Then
            Dim models As Array = Split(Session.Item("localUser").crmUserDefaultModels, ",")
            For x = 0 To UBound(models)
              For j As Integer = 0 To market_pref_models.Items.Count() - 1
                If UCase(market_pref_models.Items(j).Value) = UCase(models(x)) Then
                  market_pref_models.Items(j).Selected = True
                End If
              Next
            Next
            AddBtn_Click()
          End If

          If Not IsDBNull(r("clipref_activity_default_days")) Then
            market_time.SelectedValue = CStr(r("clipref_activity_default_days"))
          End If


          If Not IsDBNull(r("clipref_value_format")) Then
            mydisplay_value_format.SelectedValue = CStr(r("clipref_value_format"))

            If Me.mydisplay_value_format.SelectedValue = "T" Then ' thousands
              Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000"
            ElseIf Me.mydisplay_value_format.SelectedValue = "M" Then ' millions
              Me.format_label.Text = "i.e. 10,000,000 displayed as 10M"
            ElseIf Me.mydisplay_value_format.SelectedValue = "F" Then ' full number
              Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000,000"
            Else
              Me.format_label.Text = "i.e. 10,000,000 displayed as 10,000"
            End If
          End If



        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "myCRM.ascx.vb - Fill_Preferences() - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)

      End If
    End If



  End Sub
#End Region


#Region "Fill Feature Codes"
  Protected Sub fill_feature_code()
    aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_List()
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        datagrid_feature_code.DataSource = aTempTable
        datagrid_feature_code.DataBind()
      End If
    End If
  End Sub
  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id_hidden")

      Dim type_hidden As TextBox = e.Item.FindControl("type_hidden")

      If aclsData_Temp.Delete_Client_Aircraft_Key_Features_List(id.Text, type_hidden.Text) = 1 Then
        main_attention.Text = "<p align='center'>Your code has been removed.</p>"
        fill_feature_code()
      End If
    Catch ex As Exception
      error_string = "Preference_Edit_Template.ascx.vb - MyDataGrid_Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      'bind_data()
      datagrid_feature_code.EditItemIndex = -1
    Catch ex As Exception
      error_string = "Preference_Edit_Template.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_feature_code.EditItemIndex = CInt(E.Item.ItemIndex)
      main_attention.Text = ""
      fill_feature_code()
    Catch ex As Exception
      error_string = "Preference_Edit_Template.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert.Click

    If aclsData_Temp.Insert_Client_Aircraft_Key_Features_List(clickfeat_name.Text, clikfeat_type.Text, "") = 1 Then
      fill_feature_code()
      new_row.Visible = False
      add_new.Visible = True
      main_attention.Text = "<p align='center'>Your code has been saved.</p>"
    Else
      error_string = "Preference_Edit_Template.ascx.vb - Insert_Client_Aircraft_Key_Features_List() - " & aclsData_Temp.class_error
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End If

  End Sub
  Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    new_row.Visible = True
    add_new.Visible = False
    main_attention.Text = ""
  End Sub
#End Region
#Region "Support Tab"


  Private Sub myact_email_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles myact_email_button.Click
    'submit_email_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles myact_email_button.Click
    Try

      Dim Bcc As String = ""
      Dim Cc As String = ""
      Dim Subject As String = "JETNET CRM Support Request – " & actinfo_client_name.Text
      Dim Body As String = ""
      Dim From As String = "support@aerowebtech.com"

      Body = "<html>"
      Body += "<head></head>"
      Body += "<body>"
      Body += "<table width=""500"" cellpadding=""3"" cellspacing=""0"">"
      Body += "<tr><td align=""left"" valign=""top"">Name:</td><td align=""left"" valign=""top"">"
      Body += actinfo_contact_name.Text.Trim + "</td></tr>"
      Body += "<tr><td align=""left"" valign=""top"">Email:</td><td align=""left"" valign=""top"">"
      Body += actinfo_subscriber_information_email.Text.Trim + "</td></tr>"
      Body += "<tr><td align=""left"" valign=""top"">Phone:</td><td align=""left"" valign=""top"">"
      Body += actinfo_phone_textbox.Text.Trim + "</td></tr>"
      Body += "<tr><td align=""left"" valign=""top"">Client:</td><td align=""left"" valign=""top"">"
      Body += actinfo_client_name.Text.Trim + "</td></tr>"
      Body += "<tr><td align=""left"" valign=""top"">Description:</td><td align=""left"" valign=""top"">"
      Body += actinfo_email_textbox.Text.Trim + "</td></tr>"
      Body += "</table>"
      Body += "</body>"
      Body += "</html>"



      Dim wasSent As Integer = aclsData_Temp.InsertMailQueue(Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmSubSubID, From, Body.Trim)

      If wasSent > 0 Then
        mysupport_attention.Text = "<p align='center'>Your Support Email has been Sent.</p>"
      Else
        mysupport_attention.Text = "<p align='center'>Your Support Email has NOT been Sent.</p>"
      End If
    Catch ex As Exception
      error_string = "myCRM.ascx.vb - Submit_Email_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region

  Private Function UpdateCRMBlankFieldCookie(ByVal displayBlankAcVal As String) As Boolean
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
      error_string = "Error in UpdateCRMBlankFieldCookie(ByVal displayBlankAcVal As String) As Boolean " + ex.Message
    End Try
    Return bResult

  End Function

  Private Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
    'Response.Write("Verify/Save Password!<br /><br />")
    Dim go_ahead As Boolean = False
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim sql As String = ""
    Dim i As Integer = 0
    Dim temp_checked As Boolean = False
    Dim temp_text As String = ""


    Try
      main_attention.Text = ""
      password_attention.Text = ""
      If old_password_txt.Text = Session.Item("localUser").crmLocalUserPswd Then
        go_ahead = True
      ElseIf old_password_txt.Text <> Session.Item("localUser").crmLocalUserPswd Then
        If password_txt.Text <> "" And password_confirm_txt.Text <> "" Then
          If password_txt.Text = password_confirm_txt.Text Then
            'sorry cannot save your password, old password doesn't match
            password_attention.Text = "<p align='center'>We're sorry, your old password is not correct.</p>"
            go_ahead = False
          End If
        Else
          go_ahead = True 'not even trying to update password
        End If
      End If

      If go_ahead = True Then

        If password_txt.Text <> "" And password_confirm_txt.Text <> "" Then
          aclsData_Temp.Update_Client_User_Password(password_txt.Text, mydisplay_records_per_page_txt.Text, types_of_owners.SelectedValue, actinfo_timezone.SelectedValue, Session.Item("localUser").crmLocalUserID, IIf(automaticNoteLog.Checked, "Y", "N"))
          Session.Item("localUser").crmLocalUserPswd = password_txt.Text
        Else
          aclsData_Temp.Update_Client_User_Password("", mydisplay_records_per_page_txt.Text, types_of_owners.SelectedValue, actinfo_timezone.SelectedValue, Session.Item("localUser").crmLocalUserID, IIf(automaticNoteLog.Checked, "Y", "N"))
        End If

        Session.Item("localUser").crmUser_Autolog_Flag = automaticNoteLog.Checked

        'Saving cookie to show blank fields on aircraft page.
        If Not String.IsNullOrEmpty(display_no_blank_fields_on_aircraft_ddl.SelectedValue.ToString) Then
          UpdateCRMBlankFieldCookie(display_no_blank_fields_on_aircraft_ddl.SelectedValue.Trim)
        End If

        If actinfo_timezone.SelectedValue <> 0 Then
          Session("timezone") = actinfo_timezone.SelectedValue
        End If
        If IsNumeric(mydisplay_records_per_page_txt.Text) Then
          If mydisplay_records_per_page_txt.Text <> 0 Then
            Session.Item("localUser").crmUserRecsPerPage = mydisplay_records_per_page_txt.Text
          End If
        End If
        'If types_of_owners.SelectedValue <> "" Then
        Session.Item("localUser").crmUserAircraftRelationship = types_of_owners.SelectedValue
        'End If
        Dim models As String = ""
        Dim UpdateJetnetModelID As String = ""
        Dim UpdateJetnetModelArray As Array = Split("", "")
        For i = 0 To selected_models.Items.Count - 1
          If selected_models.Items(i).Value <> "" Then
            models = models & "" & selected_models.Items(i).Value & ","
            UpdateJetnetModelArray = Split(selected_models.Items(i).Value, "|")
            If UBound(UpdateJetnetModelArray) > 0 Then
              If UpdateJetnetModelID <> "" Then
                UpdateJetnetModelID += ","
              End If
              UpdateJetnetModelID += UpdateJetnetModelArray(0)
            End If

          End If
        Next

        Session.Item("localUser").crmSelectedModels = UpdateJetnetModelID
        If models <> "" Then
          models = UCase(models.TrimEnd(","))
        End If

        'We can add the update for the user table for the default models since now we go ahead and save them there.
        If aclsData_Temp.Update_Client_User_Default_Models(models, Session.Item("localUser").crmLocalUserID) = 0 Then
          'warn user of problem.
          main_attention.Text = "<p align='center'>There was a problem saving your default Models.</p>"
          'log error
          error_string = "myCRM.ascx.vb - Save Default Models - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        Else
          Session.Item("localUser").crmUserDefaultModels = models 'update the session variable
        End If




        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
          'these can only be modified by an administrator!!
          'Subscriber has an admin account?
          If pref_id.Text = "" Then
            If aclsData_Temp.Insert_Client_Preferences(ac_category_1.Text, ac_category_2.Text, ac_category_3.Text, ac_category_4.Text, ac_category_5.Text, ac_category_6.Text, ac_category_7.Text, ac_category_8.Text, ac_category_9.Text, ac_category_10.Text, _
                                                       IIf(ac_category_1_use.Checked = True, "Y", ""), IIf(ac_category_2_use.Checked = True, "Y", ""), IIf(ac_category_3_use.Checked = True, "Y", ""), IIf(ac_category_4_use.Checked = True, "Y", ""), IIf(ac_category_5_use.Checked = True, "Y", ""), IIf(ac_category_6_use.Checked = True, "Y", ""), IIf(ac_category_7_use.Checked = True, "Y", ""), IIf(ac_category_8_use.Checked = True, "Y", ""), IIf(ac_category_9_use.Checked = True, "Y", ""), IIf(ac_category_10_use.Checked = True, "Y", ""), _
                                                       pref_1.Text, pref_2.Text, pref_3.Text, pref_4.Text, pref_5.Text, _
                                                       IIf(pref_1_use.Checked = True, "Y", ""), IIf(pref_2_use.Checked = True, "Y", ""), IIf(pref_3_use.Checked = True, "Y", ""), IIf(pref_4_use.Checked = True, "Y", ""), IIf(pref_5_use.Checked = True, "Y", ""), _
                                                       market_time.SelectedValue, IIf(IsNumeric(maximum_records_export.Text), maximum_records_export.Text, 0), mydisplay_value_format.SelectedValue) = 1 Then
              'saved
            End If
          Else
            If aclsData_Temp.Update_Client_Preferences(ac_category_1.Text, ac_category_2.Text, ac_category_3.Text, ac_category_4.Text, ac_category_5.Text, ac_category_6.Text, ac_category_7.Text, ac_category_8.Text, ac_category_9.Text, ac_category_10.Text, _
                                                       IIf(ac_category_1_use.Checked = True, "Y", ""), IIf(ac_category_2_use.Checked = True, "Y", ""), IIf(ac_category_3_use.Checked = True, "Y", ""), IIf(ac_category_4_use.Checked = True, "Y", ""), IIf(ac_category_5_use.Checked = True, "Y", ""), IIf(ac_category_6_use.Checked = True, "Y", ""), IIf(ac_category_7_use.Checked = True, "Y", ""), IIf(ac_category_8_use.Checked = True, "Y", ""), IIf(ac_category_9_use.Checked = True, "Y", ""), IIf(ac_category_10_use.Checked = True, "Y", ""), _
                                                       pref_1.Text, pref_2.Text, pref_3.Text, pref_4.Text, pref_5.Text, _
                                                       IIf(pref_1_use.Checked = True, "Y", ""), IIf(pref_2_use.Checked = True, "Y", ""), IIf(pref_3_use.Checked = True, "Y", ""), IIf(pref_4_use.Checked = True, "Y", ""), IIf(pref_5_use.Checked = True, "Y", ""), _
                                                       market_time.SelectedValue, IIf(IsNumeric(maximum_records_export.Text), maximum_records_export.Text, 0), pref_id.Text, mydisplay_value_format.SelectedValue) = 1 Then
              'saved
            End If
          End If


          ' GOING TO COMMENT OUT THIS SECTION -- CHANGE TO DO ONE AT A TIME 
          ' delete all company pref 
          If aclsData_Temp.Delete_Client_Custom_Exports("Company") = True Then

            ' if any are checked, then go in 
            If pref_1_use.Checked = True Or pref_2_use.Checked = True Or pref_3_use.Checked = True Or pref_4_use.Checked = True Or pref_5_use.Checked = True Then
              Try

                MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                For i = 1 To 5
                  temp_text = ""
                  temp_checked = False
                  If i = 1 Then
                    temp_text = pref_1.Text
                    temp_checked = pref_1_use.Checked
                  ElseIf i = 2 Then
                    temp_text = pref_2.Text
                    temp_checked = pref_2_use.Checked
                  ElseIf i = 3 Then
                    temp_text = pref_3.Text
                    temp_checked = pref_3_use.Checked
                  ElseIf i = 4 Then
                    temp_text = pref_4.Text
                    temp_checked = pref_4_use.Checked
                  ElseIf i = 5 Then
                    temp_text = pref_5.Text
                    temp_checked = pref_5_use.Checked
                  End If

                  If Trim(temp_text) <> "" And temp_checked = True Then
                    sql = "Insert into client_custom_export (clicexp_type, clicexp_display, clicexp_client_db_name, clicexp_jetnet_db_name, clicexp_sort, clicexp_aerodex_flag "
                    sql &= " , clicexp_header_field_name, clicexp_field_type, clicexp_field_length) "
                    sql &= " VALUES ( "
                    sql &= "'Company', "
                    sql &= "'" & Trim(temp_text) & "', "
                    sql &= "'clicomp_category" & i & " as ''" & Trim(temp_text) & "''', "
                    sql &= "''''' as ''" & Trim(temp_text) & "''', "
                    sql &= "'10" & i & "', "
                    sql &= "'Y', "
                    sql &= "'" & Trim(temp_text) & "', "
                    sql &= "'String', "
                    sql &= "'150' "
                    sql &= " ) "

                    MySqlCommand.CommandText = sql
                    MySqlCommand.ExecuteNonQuery()
                    MySqlCommand.Dispose()
                  End If
                Next

              Catch ex As Exception

              Finally
                MySqlConn.Close()
                MySqlConn.Dispose()

                MySqlCommand.Dispose()
              End Try

            End If
          End If

          Session.Item("localUser").crmMaxClientExport = IIf(IsNumeric(maximum_records_export.Text), maximum_records_export.Text, 0)
          main_attention.Text = "<p align='center'>Your preferences have been saved.</p>"
        Else
          main_attention.Text = "<p align='center'>Your individual preferences have been saved.</p>"
        End If
      End If
    Catch ex As Exception
      main_attention.Text = "<p align='center'>There was a problem saving your preferences.</p>"

      error_string = "myCRM.ascx.vb - save_button() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    Finally
      MySqlConn = Nothing
      MySqlCommand = Nothing
    End Try
  End Sub

  Public Sub insert_into_custom_export(ByVal temp_text As String, ByVal i As Integer, ByVal temp_type As String)

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim Sql As String = ""

    Try

      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      Sql = "Insert into client_custom_export (clicexp_type, clicexp_display, clicexp_client_db_name, clicexp_jetnet_db_name, clicexp_sort, clicexp_aerodex_flag "
      Sql &= " , clicexp_header_field_name, clicexp_field_type, clicexp_field_length) "
      Sql &= " VALUES ( "
      Sql &= "'" & Trim(temp_type) & "', "
      Sql &= "'" & Trim(temp_text) & "', "
      Sql &= "'clicomp_category" & i & " as ''" & Trim(temp_text) & "''', "
      Sql &= "''''' as ''" & Trim(temp_text) & "''', "
      Sql &= "'10" & i & "', "
      Sql &= "'Y', "
      Sql &= "'" & Trim(temp_text) & "', "
      Sql &= "'String', "
      Sql &= "'150' "
      Sql &= " ) "

      MySqlCommand.CommandText = Sql
      MySqlCommand.ExecuteNonQuery()
      MySqlCommand.Dispose()



    Catch ex As Exception

    Finally
      MySqlConn.Close()
      MySqlConn.Dispose()

      MySqlCommand.Dispose()
    End Try
  End Sub

  Public Sub update_client_custom_export(ByVal temp_text As String, ByVal temp_type As String, ByVal i As Integer, ByVal temp_id As Long)

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim Sql As String = ""

    Try

      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      Sql = " Update client_custom_export set "
      Sql &= " clicexp_display = '" & Trim(temp_text) & "' "
      Sql &= ", clicexp_header_field_name = '" & Trim(temp_text) & "' "
      Sql &= ", clicexp_client_db_name = 'clicomp_category" & i & " as ''" & Trim(temp_text) & "''' "
      Sql &= ", clicexp_jetnet_db_name = ''''' as ''" & Trim(temp_text) & "''' "

      ' Sql &= " where clicexp_sort = '10" & Trim(i) & "' and clicexp_type = '" & Trim(temp_type) & "' "
      Sql &= " where clicexp_id = '" & temp_id & "'"



      MySqlCommand.CommandText = Sql
      MySqlCommand.ExecuteNonQuery()
      MySqlCommand.Dispose()



    Catch ex As Exception

    Finally
      MySqlConn.Close()
      MySqlConn.Dispose()

      MySqlCommand.Dispose()
    End Try
  End Sub

  Public Sub delete_client_custom_export(ByVal temp_text As String, ByVal temp_type As String, ByVal i As Integer)

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim Sql As String = ""

    Try

      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      Sql = " delete from client_custom_export  "
      Sql &= " where clicexp_sort = '10" & Trim(i) & "' and clicexp_type = '" & Trim(temp_type) & "' "

      MySqlCommand.CommandText = Sql
      '  MySqlCommand.ExecuteNonQuery()
      '  MySqlCommand.Dispose()



    Catch ex As Exception

    Finally
      MySqlConn.Close()
      MySqlConn.Dispose()

      MySqlCommand.Dispose()
    End Try
  End Sub

  Private Sub myCRM_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    background_image_style.Text = "<style type=""text/css"">"
    background_image_style.Text = background_image_style.Text & "body {background-image: url('" & HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + Session.Item("ImagesVirtualPath") + "/background/" + Session.Item("localUser").crmLocalUser_background & ".jpg' ); }"
    background_image_style.Text = background_image_style.Text & "</style>"
  End Sub

  Private Sub changeq_ac_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_1.Click 
    Call edit_click(1)
  End Sub
  Private Sub deleteq_ac_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_1.Click
    Call delete_click(1)
  End Sub
  Private Sub updateq_ac_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_1.Click  ' save
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_1.Text, IIf(ac_category_1_use.Checked = True, "Y", ""), 1, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 1)
    If temp_id = 0 Then
      If ac_category_1_use.Checked = True Then
        Call insert_into_custom_export(ac_category_1.Text, 1, "Aircraft")
      End If
    Else

      If ac_category_1_use.Checked = True Then
        Call update_client_custom_export(ac_category_1.Text, "Aircraft", 1, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 1)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If

    Call back_to_normal(1)
  End Sub
  Private Sub yes_delete1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete1.Click 
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 1, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 1)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 1)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_1.Text = ""
    Call back_to_normal(1)
  End Sub
  Private Sub no_delete1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete1.Click
    Call back_to_normal(1)
  End Sub
  Private Sub cancel_ac_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_1.Click
    Call back_to_normal(1)
  End Sub
   
  Private Sub changeq_ac_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_2.Click
    Call edit_click(2)
  End Sub
  Private Sub deleteq_ac_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_2.Click
    Call delete_click(2)
  End Sub
  Private Sub updateq_ac_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_2.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_2.Text, IIf(ac_category_2_use.Checked = True, "Y", ""), 2, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 2)
    If temp_id = 0 Then
      If ac_category_2_use.Checked = True Then
        Call insert_into_custom_export(ac_category_2.Text, 2, "Aircraft")
      End If
    Else

      If ac_category_2_use.Checked = True Then
        Call update_client_custom_export(ac_category_2.Text, "Aircraft", 2, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 2)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(2)
  End Sub
  Private Sub yes_delete2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete2.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 2, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 2)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 2)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_2.Text = ""
    Call back_to_normal(2)
  End Sub
  Private Sub no_delete2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete2.Click
    Call back_to_normal(2)
  End Sub
  Private Sub cancel_ac_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_2.Click
    Call back_to_normal(2)
  End Sub

  Private Sub changeq_ac_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_3.Click
    Call edit_click(3)
  End Sub
  Private Sub deleteq_ac_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_3.Click
    Call delete_click(3)
  End Sub
  Private Sub updateq_ac_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_3.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_3.Text, IIf(ac_category_3_use.Checked = True, "Y", ""), 3, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 3)
    If temp_id = 0 Then
      If ac_category_3_use.Checked = True Then
        Call insert_into_custom_export(ac_category_3.Text, 3, "Aircraft")
      End If
    Else

      If ac_category_3_use.Checked = True Then
        Call update_client_custom_export(ac_category_3.Text, "Aircraft", 3, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 3)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(3)
  End Sub
  Private Sub yes_delete3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete3.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 3, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 3)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 3)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_3.Text = ""
    Call back_to_normal(3)
  End Sub
  Private Sub no_delete3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete3.Click
    Call back_to_normal(3)
  End Sub
  Private Sub cancel_ac_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_3.Click
    Call back_to_normal(3)
  End Sub

  Private Sub changeq_ac_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_4.Click
    Call edit_click(4)
  End Sub
  Private Sub deleteq_ac_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_4.Click
    Call delete_click(4)
  End Sub
  Private Sub updateq_ac_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_4.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_4.Text, IIf(ac_category_4_use.Checked = True, "Y", ""), 4, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 4)
    If temp_id = 0 Then
      If ac_category_4_use.Checked = True Then
        Call insert_into_custom_export(ac_category_4.Text, 4, "Aircraft")
      End If
    Else

      If ac_category_4_use.Checked = True Then
        Call update_client_custom_export(ac_category_4.Text, "Aircraft", 4, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 1)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(4)
  End Sub
  Private Sub yes_delete4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete4.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 4, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 4)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 4)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_4.Text = ""
    Call back_to_normal(4)
  End Sub
  Private Sub no_delete4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete4.Click
    Call back_to_normal(4)
  End Sub
  Private Sub cancel_ac_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_4.Click
    Call back_to_normal(4)
  End Sub

  Private Sub changeq_ac_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_5.Click
    Call edit_click(5)
  End Sub
  Private Sub deleteq_ac_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_5.Click
    Call delete_click(5)
  End Sub
  Private Sub updateq_ac_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_5.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_5.Text, IIf(ac_category_5_use.Checked = True, "Y", ""), 5, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 5)
    If temp_id = 0 Then
      If ac_category_5_use.Checked = True Then
        Call insert_into_custom_export(ac_category_5.Text, 5, "Aircraft")
      End If
    Else

      If ac_category_5_use.Checked = True Then
        Call update_client_custom_export(ac_category_5.Text, "Aircraft", 5, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 5)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(5)
  End Sub
  Private Sub yes_delete5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete5.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 5, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 5)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 5)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_5.Text = ""
    Call back_to_normal(5)
  End Sub
  Private Sub no_delete5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete5.Click
    Call back_to_normal(5)
  End Sub
  Private Sub cancel_ac_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_5.Click
    Call back_to_normal(5)
  End Sub

  Private Sub changeq_ac_6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_6.Click
    Call edit_click(6)
  End Sub
  Private Sub deleteq_ac_6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_6.Click
    Call delete_click(6)
  End Sub
  Private Sub updateq_ac_6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_6.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_6.Text, IIf(ac_category_6_use.Checked = True, "Y", ""), 6, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 6)
    If temp_id = 0 Then
      If ac_category_6_use.Checked = True Then
        Call insert_into_custom_export(ac_category_6.Text, 6, "Aircraft")
      End If
    Else

      If ac_category_6_use.Checked = True Then
        Call update_client_custom_export(ac_category_6.Text, "Aircraft", 6, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 6)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(6)
  End Sub
  Private Sub yes_delete6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete6.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 6, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 6)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 6)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_6.Text = ""
    Call back_to_normal(6)
  End Sub
  Private Sub no_delete6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete6.Click
    Call back_to_normal(6)
  End Sub
  Private Sub cancel_ac_6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_6.Click
    Call back_to_normal(6)
  End Sub

  Private Sub changeq_ac_7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_7.Click
    Call edit_click(7)
  End Sub
  Private Sub deleteq_ac_7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_7.Click
    Call delete_click(7)
  End Sub
  Private Sub updateq_ac_7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_7.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_7.Text, IIf(ac_category_7_use.Checked = True, "Y", ""), 7, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 7)
    If temp_id = 0 Then
      If ac_category_7_use.Checked = True Then
        Call insert_into_custom_export(ac_category_7.Text, 7, "Aircraft")
      End If
    Else

      If ac_category_7_use.Checked = True Then
        Call update_client_custom_export(ac_category_7.Text, "Aircraft", 7, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 7)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(7)
  End Sub
  Private Sub yes_delete7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete7.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 7, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 7)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 7)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_7.Text = ""
    Call back_to_normal(7)
  End Sub
  Private Sub no_delete7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete7.Click
    Call back_to_normal(7)
  End Sub
  Private Sub cancel_ac_7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_7.Click
    Call back_to_normal(7)
  End Sub

  Private Sub changeq_ac_8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_8.Click
    Call edit_click(8)
  End Sub
  Private Sub deleteq_ac_8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_8.Click
    Call delete_click(1)
  End Sub
  Private Sub updateq_ac_8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_8.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_8.Text, IIf(ac_category_8_use.Checked = True, "Y", ""), 8, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 8)
    If temp_id = 0 Then
      If ac_category_8_use.Checked = True Then
        Call insert_into_custom_export(ac_category_8.Text, 8, "Aircraft")
      End If
    Else

      If ac_category_8_use.Checked = True Then
        Call update_client_custom_export(ac_category_8.Text, "Aircraft", 8, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 8)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(8)
  End Sub
  Private Sub yes_delete8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete8.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 8, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 8)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 8)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_8.Text = ""
    Call back_to_normal(8)
  End Sub
  Private Sub no_delete8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete8.Click
    Call back_to_normal(8)
  End Sub
  Private Sub cancel_ac_8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_8.Click
    Call back_to_normal(8)
  End Sub

  Private Sub changeq_ac_9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_9.Click
    Call edit_click(9)
  End Sub
  Private Sub deleteq_ac_9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_9.Click
    Call delete_click(9)
  End Sub
  Private Sub updateq_ac_9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_9.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_9.Text, IIf(ac_category_9_use.Checked = True, "Y", ""), 9, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 9)
    If temp_id = 0 Then
      If ac_category_9_use.Checked = True Then
        Call insert_into_custom_export(ac_category_9.Text, 9, "Aircraft")
      End If
    Else

      If ac_category_9_use.Checked = True Then
        Call update_client_custom_export(ac_category_9.Text, "Aircraft", 9, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 9)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(9)
  End Sub
  Private Sub yes_delete9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete9.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 9, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 9)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 9)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_9.Text = ""
    Call back_to_normal(9)
  End Sub
  Private Sub no_delete9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete9.Click
    Call back_to_normal(9)
  End Sub
  Private Sub cancel_ac_9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_9.Click
    Call back_to_normal(9)
  End Sub

  Private Sub changeq_ac_10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_ac_10.Click
    Call edit_click(10)
  End Sub
  Private Sub deleteq_ac_10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteq_ac_10.Click
    Call delete_click(10)
  End Sub
  Private Sub updateq_ac_10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateq_ac_10.Click
    Dim temp_id As Long = 0

    Call aclsData_Temp.Update_Single_Client_Preferences(ac_category_10.Text, IIf(ac_category_10_use.Checked = True, "Y", ""), 10, pref_id.Text)

    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 10)
    If temp_id = 0 Then
      If ac_category_10_use.Checked = True Then
        Call insert_into_custom_export(ac_category_10.Text, 10, "Aircraft")
      End If
    Else

      If ac_category_10_use.Checked = True Then
        Call update_client_custom_export(ac_category_10.Text, "Aircraft", 10, temp_id)
      Else
        'if its no longer in the custom exports 
        Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 10)
        Call aclsData_Temp.Delete_client_project_reference(temp_id)
      End If

    End If
    Call back_to_normal(10)
  End Sub
  Private Sub yes_delete10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete10.Click
    Dim temp_id As Long = 0
    Call aclsData_Temp.Update_Single_Client_Preferences("", "", 10, pref_id.Text)
    'delete the custom exoprt refereance items
    temp_id = aclsData_Temp.Find_Client_Custom_Export_id("Aircraft", 10)
    Call aclsData_Temp.Delete_Client_Custom_Exports("Aircraft", 10)
    Call aclsData_Temp.Delete_client_project_reference(temp_id)
    Me.ac_category_10.Text = ""
    Call back_to_normal(10)
  End Sub
  Private Sub no_delete10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete10.Click
    Call back_to_normal(10)
  End Sub
  Private Sub cancel_ac_10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_ac_10.Click
    Call back_to_normal(10)
  End Sub

  Public Sub delete_click(ByVal spot As Integer)
    If spot = 1 Then
      Me.yes_delete1.Visible = True
      Me.no_delete1.Visible = True
      Me.deleteq_ac_1.Visible = False
      Me.updateq_ac_1.Visible = False
      Me.edit_ac_1.Visible = False
      Me.deleteq_label1.Visible = True
    ElseIf spot = 2 Then
      Me.yes_delete2.Visible = True
      Me.no_delete2.Visible = True
      Me.deleteq_ac_2.Visible = False
      Me.updateq_ac_2.Visible = False
      Me.edit_ac_2.Visible = False
      Me.deleteq_label2.Visible = True
    ElseIf spot = 3 Then
      Me.yes_delete3.Visible = True
      Me.no_delete3.Visible = True
      Me.deleteq_ac_3.Visible = False
      Me.updateq_ac_3.Visible = False
      Me.edit_ac_3.Visible = False
      Me.deleteq_label3.Visible = True
    ElseIf spot = 4 Then
      Me.yes_delete4.Visible = True
      Me.no_delete4.Visible = True
      Me.deleteq_ac_4.Visible = False
      Me.updateq_ac_4.Visible = False
      Me.edit_ac_4.Visible = False
      Me.deleteq_label4.Visible = True
    ElseIf spot = 5 Then
      Me.yes_delete5.Visible = True
      Me.no_delete5.Visible = True
      Me.deleteq_ac_5.Visible = False
      Me.updateq_ac_5.Visible = False
      Me.edit_ac_5.Visible = False
      Me.deleteq_label5.Visible = True
    ElseIf spot = 6 Then
      Me.yes_delete6.Visible = True
      Me.no_delete6.Visible = True
      Me.deleteq_ac_6.Visible = False
      Me.updateq_ac_6.Visible = False
      Me.edit_ac_6.Visible = False
      Me.deleteq_label6.Visible = True
    ElseIf spot = 7 Then
      Me.yes_delete7.Visible = True
      Me.no_delete7.Visible = True
      Me.deleteq_ac_7.Visible = False
      Me.updateq_ac_7.Visible = False
      Me.edit_ac_7.Visible = False
      Me.deleteq_label7.Visible = True
    ElseIf spot = 8 Then
      Me.yes_delete8.Visible = True
      Me.no_delete8.Visible = True
      Me.deleteq_ac_8.Visible = False
      Me.updateq_ac_8.Visible = False
      Me.edit_ac_8.Visible = False
      Me.deleteq_label8.Visible = True
    ElseIf spot = 9 Then
      Me.yes_delete9.Visible = True
      Me.no_delete9.Visible = True
      Me.deleteq_ac_9.Visible = False
      Me.updateq_ac_9.Visible = False
      Me.edit_ac_9.Visible = False
      Me.deleteq_label9.Visible = True
    ElseIf spot = 10 Then
      Me.yes_delete10.Visible = True
      Me.no_delete10.Visible = True
      Me.deleteq_ac_10.Visible = False
      Me.updateq_ac_10.Visible = False
      Me.edit_ac_10.Visible = False
      Me.deleteq_label10.Visible = True
    End If

  End Sub

  Public Sub edit_click(ByVal spot As Integer)

    If spot = 1 Then
      Me.ac_category_1.Enabled = True
      Me.updateq_ac_1.Visible = True
      Me.deleteq_ac_1.Visible = False
      Me.edit_ac_1.Visible = False
      Me.cancel_ac_1.Visible = True
    ElseIf spot = 2 Then
      Me.ac_category_2.Enabled = True
      Me.updateq_ac_2.Visible = True
      Me.deleteq_ac_2.Visible = False
      Me.edit_ac_2.Visible = False
      Me.cancel_ac_2.Visible = True
    ElseIf spot = 3 Then
      Me.ac_category_3.Enabled = True
      Me.updateq_ac_3.Visible = True
      Me.deleteq_ac_3.Visible = False
      Me.edit_ac_3.Visible = False
      Me.cancel_ac_3.Visible = True
    ElseIf spot = 4 Then
      Me.ac_category_4.Enabled = True
      Me.updateq_ac_4.Visible = True
      Me.deleteq_ac_4.Visible = False
      Me.edit_ac_4.Visible = False
      Me.cancel_ac_4.Visible = True
    ElseIf spot = 5 Then
      Me.ac_category_5.Enabled = True
      Me.updateq_ac_5.Visible = True
      Me.deleteq_ac_5.Visible = False
      Me.edit_ac_5.Visible = False
      Me.cancel_ac_5.Visible = True
    ElseIf spot = 6 Then
      Me.ac_category_6.Enabled = True
      Me.updateq_ac_6.Visible = True
      Me.deleteq_ac_6.Visible = False
      Me.edit_ac_6.Visible = False
      Me.cancel_ac_6.Visible = True
    ElseIf spot = 7 Then
      Me.ac_category_7.Enabled = True
      Me.updateq_ac_7.Visible = True
      Me.deleteq_ac_7.Visible = False
      Me.edit_ac_7.Visible = False
      Me.cancel_ac_7.Visible = True
    ElseIf spot = 8 Then
      Me.ac_category_8.Enabled = True
      Me.updateq_ac_8.Visible = True
      Me.deleteq_ac_8.Visible = False
      Me.edit_ac_8.Visible = False
      Me.cancel_ac_8.Visible = True
    ElseIf spot = 9 Then
      Me.ac_category_9.Enabled = True
      Me.updateq_ac_9.Visible = True
      Me.deleteq_ac_9.Visible = False
      Me.edit_ac_9.Visible = False
      Me.cancel_ac_9.Visible = True
    ElseIf spot = 10 Then
      Me.ac_category_10.Enabled = True
      Me.updateq_ac_10.Visible = True
      Me.deleteq_ac_10.Visible = False
      Me.edit_ac_10.Visible = False
      Me.cancel_ac_10.Visible = True
    End If

  End Sub

  Private Sub back_to_normal(ByVal spot As Integer)

    If spot = 1 Then
      Me.yes_delete1.Visible = False
      Me.no_delete1.Visible = False
      Me.deleteq_ac_1.Visible = True
      Me.edit_ac_1.Visible = True
      Me.updateq_ac_1.Visible = False
      Me.cancel_ac_1.Visible = False
      Me.deleteq_label1.Visible = False
      Me.ac_category_1.Enabled = False
    ElseIf spot = 2 Then
      Me.yes_delete2.Visible = False
      Me.no_delete2.Visible = False
      Me.deleteq_ac_2.Visible = True
      Me.edit_ac_2.Visible = True
      Me.updateq_ac_2.Visible = False
      Me.cancel_ac_2.Visible = False
      Me.deleteq_label2.Visible = False
      Me.ac_category_2.Enabled = False
    ElseIf spot = 3 Then
      Me.yes_delete3.Visible = False
      Me.no_delete3.Visible = False
      Me.deleteq_ac_3.Visible = True
      Me.edit_ac_3.Visible = True
      Me.updateq_ac_3.Visible = False
      Me.cancel_ac_3.Visible = False
      Me.deleteq_label3.Visible = False
      Me.ac_category_4.Enabled = False
    ElseIf spot = 4 Then
      Me.yes_delete4.Visible = False
      Me.no_delete4.Visible = False
      Me.deleteq_ac_4.Visible = True
      Me.edit_ac_4.Visible = True
      Me.updateq_ac_4.Visible = False
      Me.cancel_ac_4.Visible = False
      Me.deleteq_label4.Visible = False
      Me.ac_category_4.Enabled = False
    ElseIf spot = 5 Then
      Me.yes_delete5.Visible = False
      Me.no_delete5.Visible = False
      Me.deleteq_ac_5.Visible = True
      Me.edit_ac_5.Visible = True
      Me.updateq_ac_5.Visible = False
      Me.cancel_ac_5.Visible = False
      Me.deleteq_label5.Visible = False
      Me.ac_category_5.Enabled = False
    ElseIf spot = 6 Then
      Me.yes_delete6.Visible = False
      Me.no_delete6.Visible = False
      Me.deleteq_ac_6.Visible = True
      Me.edit_ac_6.Visible = True
      Me.updateq_ac_6.Visible = False
      Me.cancel_ac_6.Visible = False
      Me.deleteq_label6.Visible = False
      Me.ac_category_6.Enabled = False
    ElseIf spot = 7 Then
      Me.yes_delete7.Visible = False
      Me.no_delete7.Visible = False
      Me.deleteq_ac_7.Visible = True
      Me.edit_ac_7.Visible = True
      Me.updateq_ac_7.Visible = False
      Me.cancel_ac_7.Visible = False
      Me.deleteq_label7.Visible = False
      Me.ac_category_7.Enabled = False
    ElseIf spot = 8 Then
      Me.yes_delete8.Visible = False
      Me.no_delete8.Visible = False
      Me.deleteq_ac_8.Visible = True
      Me.edit_ac_8.Visible = True
      Me.updateq_ac_8.Visible = False
      Me.cancel_ac_8.Visible = False
      Me.deleteq_label8.Visible = False
      Me.ac_category_8.Enabled = False
    ElseIf spot = 9 Then
      Me.yes_delete9.Visible = False
      Me.no_delete9.Visible = False
      Me.deleteq_ac_9.Visible = True
      Me.edit_ac_9.Visible = True
      Me.updateq_ac_9.Visible = False
      Me.cancel_ac_9.Visible = False
      Me.deleteq_label9.Visible = False
      Me.ac_category_9.Enabled = False
    ElseIf spot = 10 Then
      Me.yes_delete10.Visible = False
      Me.no_delete10.Visible = False
      Me.deleteq_ac_10.Visible = True
      Me.edit_ac_10.Visible = True
      Me.updateq_ac_10.Visible = False
      Me.cancel_ac_10.Visible = False
      Me.deleteq_label10.Visible = False
      Me.ac_category_10.Enabled = False
    End If

  End Sub

End Class