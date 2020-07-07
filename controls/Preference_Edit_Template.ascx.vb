Imports System.Collections
Imports System.IO
Partial Public Class Preference_Edit_Template
    Inherits System.Web.UI.UserControl
    Public Event attention(ByVal text As String)
    Public Event close_me()
    Dim aclsData_Temp As New clsData_Manager_SQL 'Class Managers used
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Private lasset As New ArrayList()
    Private lsubordinate As New ArrayList()
    Dim error_string As String = ""


#Region "Datagrid Events"


#End Region
#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            Try
           
                aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


                aclsData_Temp.class_error = ""
                If Request.QueryString.Item("type") = "fcpreferences" Then
                    'preferences_panel.Visible = False
                    'features_code_panel.Visible = True
                    'client_preferences.Visible = False
                    'market_pref.Visible = False
                    'If Not Page.IsPostBack Then
                    '    fill_feature_code()
                    'End If
                ElseIf Request.QueryString.Item("type") = "mpreferences" Then
                    preferences_panel.Visible = False
                    'features_code_panel.Visible = False
                    client_preferences.Visible = False
                    market_pref.Visible = True
                    If Not Page.IsPostBack Then
                        fillpreferences()
                    End If


                ElseIf Request.QueryString.Item("type") = "apreferences" Then
                    If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then

                        Try
                            '--------------------------------These are the ADMIN Preferences--------------------------------
                            preferences_panel.Visible = False
                            client_preferences.Visible = True
                            'features_code_panel.Visible = False
                            company_atten.Text = "<p align='center' class='info_box'>The following preferences will be applied to all users of this CRM and should only be modified by a system administrator.</p>"
                            If Not Page.IsPostBack Then
                                fillpreferences()
                            End If
                        Catch ex As Exception
                            error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & ex.Message
                            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                        End Try
                    End If
                ElseIf Request.QueryString.Item("type") = "cpreferences" Then
                    '-----------------------------------These are personal preferences--------------------------------
                    preferences_panel.Visible = True
                    client_preferences.Visible = False
                    'features_code_panel.Visible = False
                    If Not Page.IsPostBack Then
                        aTempTable = aclsData_Temp.Get_Client_Timezone()
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                For Each r As DataRow In aTempTable.Rows
                                    cliuser_time_zone.Items.Add(New ListItem(r(1), r(0)))
                                Next
                            End If
                        Else
                            If aclsData_Temp.class_error <> "" Then
                                error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                            End If

                        End If

                        aTempTable = aclsData_Temp.Get_Client_User(CInt(Session.Item("localUser").crmLocalUserID))
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then

                                For Each r As DataRow In aTempTable.Rows
                                    cliuser_first_name.Text = CStr(IIf(Not IsDBNull(r("cliuser_first_name")), r("cliuser_first_name"), ""))
                                    cliuser_last_name.Text = CStr(IIf(Not IsDBNull(r("cliuser_last_name")), r("cliuser_last_name"), ""))
                                    cliuser_login.Text = CStr(IIf(Not IsDBNull(r("cliuser_login")), r("cliuser_login"), ""))
                                    cliuser_password.Text = CStr(IIf(Not IsDBNull(r("cliuser_password")), r("cliuser_password"), ""))
                                    cliuser_admin_flag.Text = CStr(IIf(Not IsDBNull(r("cliuser_admin_flag")), r("cliuser_admin_flag"), ""))
                                    cliuser_email_address.Text = CStr(IIf(Not IsDBNull(r("cliuser_email_address")), r("cliuser_email_address"), ""))
                                    cliuser_user_id.Text = CStr(IIf(Not IsDBNull(r("cliuser_user_id")), r("cliuser_user_id"), ""))
                                    cliuser_end_date.Text = CStr(IIf(Not IsDBNull(r("cliuser_end_date")), r("cliuser_end_date"), ""))
                                    Try
                                        If Not IsDBNull(r("cliuser_timezone")) Then
                                            cliuser_time_zone.SelectedValue = r("cliuser_timezone")
                                        End If
                                    Catch
                                    End Try
                                    cliuser_id.Text = CStr(IIf(Not IsDBNull(r("cliuser_id")), r("cliuser_id"), ""))
                                Next
                            End If
                        Else
                            If aclsData_Temp.class_error <> "" Then
                                error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                            End If

                        End If
                    End If
                ElseIf Request.QueryString.Item("type") = "fpreferences" Then
                    '----------------------------------Aircraft Field Preferences----------------------------------------------
                    aircraft_atten.Text = "<p align='center' class='info_box'>The following preferences will be applied to all users of this CRM and should only be modified by a system administrator.</p>"
                    preferences_panel.Visible = False
                    ac_fields.Visible = True
                    client_preferences.Visible = False
                    'features_code_panel.Visible = False



                    aTempTable = aclsData_Temp.GetClientListPreferenceSelected("Aircraft", "N")
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each r As DataRow In aTempTable.Rows

                                If Not Page.IsPostBack Then
                                    all_fields.Items.Add(New ListItem(r("clilistpref_name"), r("clilstpref_id")))
                                    'client_fields.Items.Add(New ListItem(r("clilistpref_name"), r("clilstpref_id")))
                                End If
                            Next
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                        End If
                    End If

                End If

                Dim field_text As String = ""
                field_text = "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"
                field_text = field_text & "<tr class='gray'>"

                aTempTable = aclsData_Temp.GetClientListPreferenceSelected("Aircraft", "Y")
                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            field_text = field_text & "<td align='left' valign='top'>"
                            field_text = field_text & r("clilistpref_name")
                            field_text = field_text & "</td>"
                            If Not Page.IsPostBack Then
                                client_fields.Items.Add(New ListItem(r("clilistpref_name"), r("clilstpref_id")))
                            End If
                        Next
                    End If
                End If

                field_text = field_text & "</tr>"
                field_text = field_text & "</table>"

                table_columns.Text = field_text
            Catch ex As Exception
                error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & ex.Message
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try
        End If
    End Sub
    Public Sub fillpreferences()
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
                    error_string = "preference_edit_template.ascx.vb - load() - " & aclsData_Temp.class_error

                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                End If

            End If
        Catch ex As Exception
            error_string = "preference_edit_template.ascx.vb - load() market Model Dropdown Filling - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try

        aTempTable = aclsData_Temp.Get_Client_Preferences()
        If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
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



                    If Not IsDBNull(r("clipref_activity_default_days")) Then
                        market_time.SelectedValue = CStr(r("clipref_activity_default_days"))
                    End If
                Next
            End If
        Else
            If aclsData_Temp.class_error <> "" Then
                error_string = "Preference_Edit_Template.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)

            End If
        End If
    End Sub

    Public Sub UpdateFields()
        Try
            Dim updated_string As String = ""

            updated_string = "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"
            updated_string = updated_string & "<tr class='gray'>"
            Dim x As Integer = 1
            For Each Item As ListItem In client_fields.Items


                '  Response.Write("Aircraft" & " " & Item.Text & " " & "Y" & " " & x & " " & Item.Value & "<br />")
                If aclsData_Temp.UpdateClientListPreference("Aircraft", Item.Text, "Y", x, Item.Value) = 1 Then
                    'Response.Write("updated")
                    updated_string = updated_string & "<td align='left' valign='top'>" & Item.Text & "</td>"
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = "Preference_Edit_Template.ascx.vb - UpdateFields() - " & aclsData_Temp.class_error

                        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                    End If
                End If



                x = x + 1
            Next
            updated_string = updated_string & "</tr>"
            updated_string = updated_string & "</table>"
            table_columns.Text = updated_string
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - UpdateFields() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Public Sub AddBtn_Click()
        Try
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
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - UpdateFields() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    Public Sub AddAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
        Try
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
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - AddAllBtn_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    Public Sub RemoveBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
        Try
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
                    If Not all_fields.Items.Contains(CType(lsubordinate(i), ListItem)) Then
                        market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
                        fiel = CType(lsubordinate(i), ListItem)
                    End If
                    selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
                    fiel = CType(lsubordinate(i), ListItem)


                    lasset.Add(lsubordinate(i))
                    market_pref_models.SelectedValue = fiel.Value
                Next i
            End If
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - RemoveBtn_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    Public Sub RemoveAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
        Try

            While selected_models.Items.Count <> 0
                Dim i As Integer
                For i = 0 To selected_models.Items.Count - 1
                    If Not lsubordinate.Contains(selected_models.Items(i)) Then
                        lsubordinate.Add(selected_models.Items(i))
                    End If
                Next i
                Dim fiel As New ListItem
                For i = 0 To lsubordinate.Count - 1
                    If Not all_fields.Items.Contains(CType(lsubordinate(i), ListItem)) Then
                        market_pref_models.Items.Add(CType(lsubordinate(i), ListItem))
                        fiel = CType(lsubordinate(i), ListItem)
                    End If
                    selected_models.Items.Remove(CType(lsubordinate(i), ListItem))
                    lasset.Add(lsubordinate(i))
                Next i
            End While
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - RemoveAllBtn_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    ' ---- ButtonMoveUp_Click --------------------------
    '
    ' Move listbox item up one

    Protected Sub ButtonMoveUp_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim SelectedIndex As Integer = client_fields.SelectedIndex

            If SelectedIndex = -1 Then
                ' nothing selected
                Return
            End If
            If SelectedIndex = 0 Then
                ' already at top of list  
                Return
            End If

            Dim Temp As ListItem
            Temp = client_fields.SelectedItem

            client_fields.Items.Remove(client_fields.SelectedItem)
            client_fields.Items.Insert(SelectedIndex - 1, Temp)
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - ButtonMoveUp_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    ' ---- ButtonMoveDown_Click -------------------------------
    '
    ' Move listbox item down one

    Protected Sub ButtonMoveDown_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim SelectedIndex As Integer = client_fields.SelectedIndex

            If SelectedIndex = -1 Then
                ' nothing selected
                Return
            End If
            If SelectedIndex = client_fields.Items.Count - 1 Then
                ' already at top of list            
                Return
            End If

            Dim Temp As ListItem
            Temp = client_fields.SelectedItem

            client_fields.Items.Remove(client_fields.SelectedItem)
            client_fields.Items.Insert(SelectedIndex + 1, Temp)
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - ButtonMoveDown_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

    'Protected Sub fill_feature_code()
    '    aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_List()
    '    If Not IsNothing(aTempTable) Then
    '        If aTempTable.Rows.Count > 0 Then
    '            datagrid_feature_code.DataSource = aTempTable
    '            datagrid_feature_code.DataBind()
    '        End If
    '    End If
    'End Sub


#End Region
#Region "Update Preferences"
    Private Sub update_preferences_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles update_preferences.Click
        Try
            Dim end_date As New Nullable(Of System.DateTime)
            If cliuser_end_date.Text <> "" Then
                end_date = cliuser_end_date.Text
            End If
            If aclsData_Temp.Update_Client_User(cliuser_first_name.Text, cliuser_last_name.Text, cliuser_login.Text, cliuser_password.Text, cliuser_admin_flag.Text, cliuser_email_address.Text, Now(), CInt(Session.Item("localUser").crmLocalUserID), cliuser_time_zone.SelectedValue, end_date, cliuser_id.Text) = 1 Then
                personal_atten.Text = "<p align='center'>Your preferences have been saved.</p>"
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
                If aclsData_Temp.class_error <> "" Then
                    error_string = "Preference_Edit_Template.ascx.vb - update_preferences_Click() - " & aclsData_Temp.class_error

                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                End If
                personal_atten.Text = "<p align='center'>There was a problem saving your preferences.</p>"
            End If
        Catch ex As Exception
            error_string = "Preference_Edit_Template.ascx.vb - update_preferences_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub

#End Region

    'Private Sub update_client_pref_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles update_client_pref.Click, market_pref_btn.Click
    '    Try
    '        Dim models As String = ""
    '        For i = 0 To selected_models.Items.Count - 1
    '            If selected_models.Items(i).Value <> "" Then
    '                models = models & "" & selected_models.Items(i).Value & ","
    '            End If
    '        Next

    '        If models <> "" Then
    '            models = UCase(models.TrimEnd(","))
    '        End If

    '        If pref_id.Text = "" Then
    '            If aclsData_Temp.Insert_Client_Preferences(pref_1.Text, pref_2.Text, pref_3.Text, pref_4.Text, pref_5.Text, IIf(pref_1_use.Checked = True, "Y", ""), IIf(pref_2_use.Checked = True, "Y", ""), IIf(pref_3_use.Checked = True, "Y", ""), IIf(pref_4_use.Checked = True, "Y", ""), IIf(pref_5_use.Checked = True, "Y", ""), market_time.SelectedValue, models) = 1 Then
    '                company_atten.Text = "<p align='center'>Your preferences have been saved.</p>"
    '            Else
    '                company_atten.Text = "<p align='center'>There was a problem saving your preferences.</p>"
    '            End If
    '        Else
    '            If aclsData_Temp.Update_Client_Preferences(pref_1.Text, pref_2.Text, pref_3.Text, pref_4.Text, pref_5.Text, IIf(pref_1_use.Checked = True, "Y", ""), IIf(pref_2_use.Checked = True, "Y", ""), IIf(pref_3_use.Checked = True, "Y", ""), IIf(pref_4_use.Checked = True, "Y", ""), IIf(pref_5_use.Checked = True, "Y", ""), market_time.SelectedValue, models, pref_id.Text) = 1 Then
    '                company_atten.Text = "<p align='center'>Your preferences have been saved.</p>"
    '            Else
    '                company_atten.Text = "<p align='center'>There was a problem saving your preferences.</p>"
    '            End If
    '        End If
    '        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    '    Catch ex As Exception
    '        error_string = "Preference_Edit_Template.ascx.vb - update_client_pref_Click() - " & ex.Message
    '        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    '    End Try
    'End Sub


    'Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    '    new_row.Visible = True
    '    add_new.Visible = False
    '    feature_code_atten.Text = ""
    'End Sub


End Class