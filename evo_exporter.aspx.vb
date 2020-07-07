Partial Public Class evo_exporter
  Inherits System.Web.UI.Page
  Dim aclsData_Temp As New clsData_Manager_SQL
  Private localDatalayer As viewsDataLayer
  Dim aTempTable As New DataTable 'Datatable
  Dim atemptable2 As New DataTable
  Private lasset As New ArrayList()
  Private lsubordinate As New ArrayList()
  Dim error_string As String = ""
  Dim current_history As String = ""
  Public order_by_string As String
  Dim Summary As Boolean = False
  Dim bFromPreferences As Boolean = False

  Private nExportID As Long = 0
  Private sExportType As String = ""

  Public bRefreshPreferences As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ''response redirect if not logged in.
    ''If Session.Item("crmUserLogon") <> True Then
    ''    Response.Redirect("Default.aspx", False)
    ''End If
    'Setting up databases. Both Jetnet and Client. 
    ' aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
    aclsData_Temp.JETNET_DB = Session.Item("jetnetAdminDatabase")
    aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

    aclsData_Temp.class_error = "" 'reset error to nothing
    Master.SetContainerClass("container MaxWidthRemove") 'set full width page
    localDatalayer = New viewsDataLayer
    localDatalayer.adminConnectStr = Application.Item("crmClientSiteData").AdminDatabaseConn

    'sets up the tab answer. 
    Dim warning_label As String = ""
    Dim can_export As Boolean = False

    If Not IsNothing(Request.Item("fromPreferences")) Then
      If Not String.IsNullOrEmpty(Request.Item("fromPreferences").ToString.Trim) Then
        bFromPreferences = CBool(Request.Item("fromPreferences").ToString.ToLower.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("id")) Then
      If Not String.IsNullOrEmpty(Request.Item("id").ToString.Trim) Then
        If IsNumeric(Request.Item("id").ToString.ToLower.Trim) Then
          nExportID = CLng(Request.Item("id").ToString.ToLower.Trim)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("type")) Then
      If Not String.IsNullOrEmpty(Request.Item("type").ToString.Trim) Then
        sExportType = Request.Item("type").ToString
      End If
    End If

    If bFromPreferences Then
      sExportType = sExportType.Replace("|", "#")
    End If

    If CDbl(Session.Item("localUser").crmLatestRecordCount) > CDbl(Session.Item("localUser").crmMaxClientExport) Then
      Me.run_export.Visible = False
      'Me.label_text.Text = "<div class=""Box"">"
      Me.label_text.Text = "<p>Note that custom exports are limited to a maximum of 255 fields and " & Session.Item("localUser").crmMaxClientExport & " records at a time."
      Me.label_text.Text += "Your current selection of " & Session.Item("localUser").crmLatestRecordCount & " Records has exceeded your current export limits.</p>"
      ' Me.label_text.Text += "</div>"
    Else
      Me.run_export.Visible = True
      ' Me.label_text.Text = "<div class=""Box"">"
      Me.label_text.Text = "<p>Please select from the list of available fields using the arrows below the list."
      Me.label_text.Text += "Once you have your desired fields in the 'Fields to Export' list then click "
      Me.label_text.Text += "on 'Run Export' to generate the desired export.</p>"
      ' Me.label_text.Text += "</p>" 

    End If



    'warning_label = "<p align='left' class='info_box float_export'>"
    ' warning_label += "<b>Export/Report Data Usage:</b>"
    ' warning_label += "<br>Subscribers agree that:"
    ' warning_label += "<br>&nbsp;&nbsp;&nbsp;&#149;ALL JETNET data is available to users on a subscription only basis."
    ' warning_label += "<br>&nbsp;&nbsp;&nbsp;&#149;ALL JETNET data is the property of JETNET and may not be used or otherwise published without the express written permission of JETNET."
    'warning_label += "<br>&nbsp;&nbsp;&nbsp;&#149;ALL JETNET data may not be exported for any purposes that directly or indirectly compete with JETNET’s services."
    'warning_label += "<br>&nbsp;&nbsp;&nbsp;&#149;ALL JETNET data may not be exported for use in 3rd party systems or CRMs for the purpose of reducing subscriptions to JETNET services."
    ' warning_label += "<br>Any breach of the data usage agreement above by the subscribing company and/or their respective employees, agents, or representatives will be subject to legal action.</font>"
    warning_label += "<p><strong>Export/Report Data Usage:</strong> Subscribers agree that:   (1) ALL JETNET data is available to users on a subscription only basis. (2) ALL JETNET data is the property of JETNET and may not be used or otherwise published without the express written permission of JETNET (3) No JETNET data shall be exported for any purposes that directly or indirectly compete with JETNET’s services.  (4) No JETNET data shall be exported for use in 3rd party systems or CRMs for the purpose of reducing subscriptions to JETNET services. - Any breach of the data usage agreement above by the subscribing company and/or their respective employees, agents, or representatives will be subject to legal action."


    warning_label += "</p>"

    Me.warning1.Text = warning_label
    ' Me.warning2.Text = warning_label

    If Trim(UCase(Request("export_type"))) <> "" Then
      Session("tab") = Trim(UCase(Request("export_type")))
    End If


    If sExportType.ToLower.Contains("summary") Then
      Summary = True
    End If
    Try

      Master.SetPageTitle(Session("tab") & " Data Export")

      If Summary = True Then
        export_type.SelectedValue = "summary"
      End If

      If Not Page.IsPostBack Then 'this needs to not be ran on subsequent postbacks, so it's important to keep inside of here. This way the 

        If Not String.IsNullOrEmpty(sExportType.Trim) Then
          Me.export_types.SelectedValue = sExportType.Trim
          Fill_Available_Templates(sExportType.Trim, nExportID.ToString)
        Else
          Fill_Available_Templates("", nExportID.ToString)
        End If

        'These set up the form for the custom export. Makes the form visible, changes title text, selects default value for 1st listbox and fills the 
        'values of the second listbox
        company_new.Visible = True
        container.Visible = True

        add_sub()

        If nExportID > 0 Then
          Call selected_export_function(0)
        End If

        If Trim(Session("tab")) = "YACHT COMPANY EXPORT" Then
          ' IF YOU ARE AUTO-EXPORTING THIS FOR YACHT CROSSOVER EXPORTS, THEN EXPORT AUTOMATICALLY
          If Not IsNothing(Session("Yacht_Crossover_Select")) Then
            If Trim(Session("Yacht_Crossover_Select")) <> "" Then
              Export_Information(export_now_btn)
            End If
          End If
        ElseIf Trim(Session("tab")) = "YACHT COMPANY MODEL EXPORT" Then
          ' IF YOU ARE AUTO-EXPORTING THIS FOR YACHT CROSSOVER EXPORTS, THEN EXPORT AUTOMATICALLY
          If Not IsNothing(Session("Yacht_Crossover_Model_Select")) Then
            If Trim(Session("Yacht_Crossover_Model_Select")) <> "" Then
              Export_Information(export_now_btn)
            End If
          End If
        ElseIf Trim(Session("tab")) = "YACHT COMPANY YACHT EXPORT" Then
          ' IF YOU ARE AUTO-EXPORTING THIS FOR YACHT CROSSOVER EXPORTS, THEN EXPORT AUTOMATICALLY
          If Not IsNothing(Session("Yacht_Crossover_Yacht_Select")) Then
            If Trim(Session("Yacht_Crossover_Yacht_Select")) <> "" Then
              Export_Information(export_now_btn)
            End If
          End If
        ElseIf Trim(Session("tab")) = "YACHT COMPANY NO YACHT EXPORT" Then
          ' IF YOU ARE AUTO-EXPORTING THIS FOR YACHT CROSSOVER EXPORTS, THEN EXPORT AUTOMATICALLY
          If Not IsNothing(Session("Yacht_Crossover_AC_Select")) Then
            If Trim(Session("Yacht_Crossover_AC_Select")) <> "" Then
              Export_Information(export_now_btn)
            End If
          End If
        End If

      End If

      Me.help_link.Visible = True
      Me.help_button_label.Visible = True

      Me.help_link.Text = "&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;<span class=""red_text"">" & Me.my_export_list_box.Items.Count & " templates selected.</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

      check_if_can_export()

      If Trim(Request("delete")) = "Y" And nExportID > 0 And Not IsPostBack Then
        Call clicked_modify_export(Nothing, Nothing)
        TabContainer1.ActiveTabIndex = 1
        tabs_container.ActiveTabIndex = 1

      ElseIf bFromPreferences And nExportID > 0 And Not IsPostBack Then

        warning1.Visible = False
        customize_tab.HeaderText = "Edit Template"

        sender.id = "modify_export"

        Call clicked_modify_export(sender, e)
        TabContainer1.ActiveTabIndex = 1
        tabs_container.ActiveTabIndex = 1

        template_tab.Visible = False

      Else

        If Not IsPostBack Then
          If Not IsNothing(Session("View_ID")) Then
            If Trim(Session("Tab")) = "YACHT COMPANY" And Trim(Session("View_ID")) = 21 Then
              TabContainer1.ActiveTabIndex = 1
              tabs_container.ActiveTabIndex = 1
            End If
          End If
        End If
      End If

    Catch ex As Exception
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, "evo_exporter.aspx.vb Page_Load():" & ex.Message, DateTime.Now.ToString())
    End Try
  End Sub

  Public Function check_if_can_export() As Boolean
    check_if_can_export = False

    If Session.Item("localUser").crmAllowExport_Flag = False Then
      check_if_can_export = False
      '  ElseIf Session.Item("localUser").crmDemoUserFlag = True Then
      '      check_if_can_export = False
      '  ElseIf Session.Item("localSubscription").crmMarketingFlag = True Then
      '     check_if_can_export = False
    Else
      check_if_can_export = True
    End If

    If check_if_can_export = False Then
      Me.warning1.Text = "<p><font color='red'>You are currently on a Demo Account and are not able to use this capability.</font></p>"
      Me.export_now_btn.Visible = False
    End If

  End Function




    Public Sub Fill_Available_Templates(ByVal temp_type As String, ByVal selected_id As String, Optional ByVal bFromPreferences As Boolean = False)
    Dim temp_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim abrevs_count As Integer = 0


        aTempTable = aclsData_Temp.ListAll_Subscription_Install_Saved_Exports(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, "N", Session("tab"), temp_type)



        '-----------------------------------------
        my_export_list_box.Items.Clear()
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          my_export_list_box.Items.Add(New ListItem(r("sise_subject"), r("sise_id")))

          If Trim(selected_id) <> "" Then
            If Trim(r("sise_id")) = Trim(selected_id) Then
              my_export_list_box.SelectedIndex = my_export_list_box.Items.Count - 1
              Me.selected_name.Text = r("sise_subject")
            End If
          End If
        Next
        TabContainer1.ActiveTabIndex = 1
        tabs_container.ActiveTab = template_tab
      End If
    End If
    my_export_list_box.Visible = False


    '-----------------------------------------

    If Not IsPostBack Then
      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR = True Then
        Me.export_types.Items.Clear()
        Me.export_types.Items.Add(New ListItem("My Personal & Shared Templates", "MY"))
        Me.export_types.Items.Add(New ListItem("All Templates", "ALL"))
        Me.export_types.Items.Add(New ListItem("All Shared Templates", "ALS"))
        Me.export_types.Items.Add(New ListItem("All Personal Templates", "ALP"))

        If Trim(temp_type) = "MY" Then
          Me.export_types.SelectedIndex = 0
        ElseIf Trim(temp_type) = "ALL" Then
          Me.export_types.SelectedIndex = 1
        ElseIf Trim(temp_type) = "ALS" Then
          Me.export_types.SelectedIndex = 2
        ElseIf Trim(temp_type) = "ALP" Then
          Me.export_types.SelectedIndex = 3
        End If

        Select Case HttpContext.Current.Session.Item("localSubscription").crmSubscriptionShareType
          Case eSubscriptionShareType.MY_PARENT_COMPANY
            Me.bottom_label_text.Text = "As an administrator you can view, edit, and delete templates for all users for your Company. Use the drop down above the template list to select your desired template list"
          Case eSubscriptionShareType.MY_PARENT_SUBSCRIPTION
            Me.bottom_label_text.Text = "As an administrator you can view, edit, and delete templates for all users for your Subscription. Use the drop down above the template list to select your desired template list"
          Case Else
            Me.bottom_label_text.Text = "As an administrator you can view, edit, and delete templates for all users for your Company. Use the drop down above the template list to select your desired template list"
        End Select

      Else
        Me.export_types.Items.Clear()
        Me.export_types.Items.Add(New ListItem("My Personal & Shared Templates", "MY"))
        Me.export_types.Items.Add(New ListItem("My Personal Templates", "MYP"))
        Me.export_types.Items.Add(New ListItem("Shared Templates", "ALLS"))

        If Trim(temp_type) = "MY" Then
          Me.export_types.SelectedIndex = 0
        ElseIf Trim(temp_type) = "MYP" Then
          Me.export_types.SelectedIndex = 1
        ElseIf Trim(temp_type) = "ALLS" Then
          Me.export_types.SelectedIndex = 2
        End If

        Me.bottom_label_text.Text = "You are only able to edit/delete templates that you have created.  If you wish to edit any template from another user you will either need to contact the other user or your account administrator. "

      End If
    End If

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then

        htmlOut.Append("<table border='0' width='100%' cellpadding='3' cellspacing='0' class=""formatTable blue"" >")

        For Each r As DataRow In aTempTable.Rows

          If Not String.IsNullOrEmpty(selected_id.Trim) Then

            If r.Item("sise_id").ToString.Contains(selected_id.Trim) Then

              htmlOut.Append("<tr>")

              If abrevs_count = 0 Then
                abrevs_count = 1
              Else
                abrevs_count = 0
              End If
            Else
              If abrevs_count = 0 Then
                htmlOut.Append("<tr>")
                abrevs_count = 1
              Else
                htmlOut.Append("<tr>")
                abrevs_count = 0
              End If
            End If
          Else
            If abrevs_count = 0 Then
              htmlOut.Append("<tr>")
              abrevs_count = 1
            Else
              htmlOut.Append("<tr>")
              abrevs_count = 0
            End If
          End If

          If Trim(r("sise_share_flag")) = "Y" Then
            htmlOut.Append("<td valign='top' align='left' width='16'>")
            htmlOut.Append("<img src='images/shared_folder.png' title='Shared Template' alt='Shared Template'>")
            htmlOut.Append("</td>")
          Else
            htmlOut.Append("<td valign='top' align='left' width='16'>")
            htmlOut.Append("<img src='images/regular_folder.png' title='Personal Template' alt='Personal Template'>")
            htmlOut.Append("</td>")
          End If

          htmlOut.Append("<td valign='top' align='left' width='400'>")
          htmlOut.Append("<a name=""" + r.Item("sise_id").ToString + """ href=""evo_exporter.aspx?id=" + r.Item("sise_id").ToString + "&type=" + Me.export_types.SelectedValue & "#" + r.Item("sise_id").ToString + """>")

          If Not (IsDBNull(r.Item("sise_subject"))) And Not String.IsNullOrEmpty(r.Item("sise_subject").ToString.Trim) Then
            htmlOut.Append(r.Item("sise_subject").ToString.Trim)
          Else
            htmlOut.Append(" blank name ")
          End If

          htmlOut.Append("</a></td>")

          htmlOut.Append("<td valign='top' align='left' width='130'>")
          If Not IsDBNull(r("contact_first_name")) Then
            htmlOut.Append(r("contact_first_name") & " ")
          End If
          If Not IsDBNull(r("contact_last_name")) Then
            htmlOut.Append(r("contact_last_name"))
          End If
          htmlOut.Append("&nbsp;</td>")

          If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR = True Then

            htmlOut.Append("<td valign='top' align='left' width='12'>")
            htmlOut.Append("<a href='evo_exporter.aspx?id=" & r("sise_id") & "&type=" & Me.export_types.SelectedValue & "#" & r("sise_id") & "'><img src='images/edit_icon.png' alt='Edit Template' title='Edit Template'></a>")
            htmlOut.Append("</td>")

            htmlOut.Append("<td valign='top' align='left' width='12'>")
            htmlOut.Append("<a href='evo_exporter.aspx?id=" & r("sise_id") & "&type=" & Me.export_types.SelectedValue & "&delete=Y#" & r("sise_id") & "'>")
            htmlOut.Append("<img src='images/delete_icon.png' title='Delete Template' alt='Delete Template'>")
            htmlOut.Append("</a></td>")
          Else
            If Trim(r("sise_id")) = Trim(CStr(Trim(Session.Item("localUser").crmUserLogin))) Then
              htmlOut.Append("<td valign='top' align='left' width='12'>")
              htmlOut.Append("<a href='evo_exporter.aspx?id=" & r("sise_id") & "&type=" & Me.export_types.SelectedValue & "#" & r("sise_id") & "'><img src='images/edit_icon.png' alt='Edit Template' title='Edit Template'></a>")
              htmlOut.Append("</td>")

              htmlOut.Append("<td valign='top' align='left' width='12'>")
              htmlOut.Append("<a href='evo_exporter.aspx?id=" & r("sise_id") & "&type=" & Me.export_types.SelectedValue & "&delete=Y#" & r("sise_id") & "'>")
              htmlOut.Append("<img src='images/delete_icon.png' title='Delete Template' alt='Delete Template'>")
              htmlOut.Append("</a></td>")
            Else
              htmlOut.Append("<td valign='top' align='left' width='12'>&nbsp;</td>")
              htmlOut.Append("<td valign='top' align='left' width='12'>&nbsp;</td>")
            End If
          End If

          htmlOut.Append("</tr>")
        Next

        htmlOut.Append("</table>")

        Me.templates_panel.Visible = True
        Me.export_list.Visible = True
        Me.export_list.Text = htmlOut.ToString

        TabContainer1.ActiveTabIndex = 1
        tabs_container.ActiveTab = template_tab
      End If
    End If

  End Sub

  ''' <summary>
  ''' This function fills the second listbox on the custom exporter.
  ''' 'It takes the selected from the first group and queries the database to get the second listbox selections.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub add_sub()
    Dim type As String = ""
    Dim summary As Boolean = False
    Try

      sub_selections.Items.Clear()
            If export_type.SelectedValue = "summary" Then
                summary = True
            Else
                summary = False
            End If

            '     For Each li As ListItem In available_data_types.Items
            'If li.Selected Then
            'type = li.Value

            If Trim(UCase(Session("tab"))) = "OPERATING COST" Then
                operating_radio.Visible = True
                If Not IsPostBack Then  ' page load 
                    operating_radio.SelectedIndex = 0
                End If
            End If 

            aTempTable = aclsData_Temp.Fill_Available_Data_Fields_Based_On_Main_Group(Session("tab"), summary, Session.Item("localSubscription").crmAerodexFlag)

            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    For Each r As DataRow In aTempTable.Rows
                        'cefstab_sub_name, cefstab_id
                        If Not IsDBNull(r("cefstab_sub_name")) Then
                            'have to use a | to split the subgroup and the maingroup. You'll need both types to get the answers in the third listbox
                            'sub_selections.Items.Add(New ListItem(r("cef_sub_group"), r("cef_sub_group") & "|" & type))
                            sub_selections.Items.Add(New ListItem(r("cefstab_sub_name"), r("cefstab_id")))
                        End If
                    Next

                End If
            Else
                'Dataquery returned nothing, an error has occurred. 
                If aclsData_Temp.class_error <> "" Then
                    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, "evo_exporter.aspx.vb Add_Sub(): " & aclsData_Temp.class_error, DateTime.Now.ToString())
                    aclsData_Temp.class_error = ""
                End If
            End If

            ' sub_selections.Items.Add(New ListItem("Custom Fields", "Custom"))

            '  End If
            '  Next

        Catch ex As Exception
      error_string = "evo_exporter.aspx.vb - add_sub() - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  ''' <summary>
  ''' This fills the third listbox.
  ''' In order to do this correctly, you'll need the corresponding first box and second box entries. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub add_actual_fields()
    Dim type As String = ""
    Dim cef_display As String = ""
    Dim cef_id As Integer = 0
    Dim cef_evo_field_name As String = ""
    'Dim cef_main_group As String = ""
    Dim cef_header_field_name As String = ""
    Dim summary As Boolean = False
    Dim rename_field_name As String = ""

    Try
      choice_to_export.Items.Clear()

      For Each li As ListItem In sub_selections.Items
        If li.Selected Then
          type = li.Value
          'This is the piped value we saved in the second listbox.
          '  Dim split_string As Array = Split(type, "|")
          'Splitting it to get  the answer to run the query
          ' If UBound(split_string) = 1 Then 'make sure that you have the correct amount in the array.
          ' If choic Then
          If export_type.SelectedValue = "summary" Then
            summary = True
          Else
            summary = False
          End If

          If Trim(type) = "Custom" Then
            get_fields_for_add_comparable(0, choice_to_export, "")
          Else
            aTempTable = aclsData_Temp.Fill_Available_Data_Fields_Based_On_Sub_Group(type, summary, Session("tab"), "", 0, Session.Item("localSubscription").crmAerodexFlag) 'run the function

            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
                  If Not IsDBNull(r("cef_display")) Then
                    cef_display = r("cef_display")
                  End If
                  If Not IsDBNull(r("cef_id")) Then
                    cef_id = r("cef_id")
                  End If
                  ' this is the evo field name 

                  ' this is the header field name
                  cef_header_field_name = ""
                  If Not IsDBNull(r("cef_header_field_name")) Then
                    cef_header_field_name = r("cef_header_field_name")
                  End If

                  cef_evo_field_name = ""
                  If Not IsDBNull(r("cef_client_field_name")) Then
                    cef_evo_field_name = r("cef_client_field_name")
                  Else
                    If Not IsDBNull(r("cef_evo_field_name")) Then
                      cef_evo_field_name = r("cef_evo_field_name")
                    End If
                  End If



                  'cef_id, cef_display, cef_evo_field_name
                  'choice_to_export.Items.Add(New ListItem(cef_display, cef_id & "|" & cef_evo_field_name & "|" & cef_main_group & "|" & cef_header_field_name))
                  choice_to_export.Items.Add(New ListItem(cef_display, cef_id & "|" & cef_evo_field_name & "|" & cef_header_field_name))
                Next
              End If
            Else
              'Dataquery returned nothing, an error has occurred. 
              If aclsData_Temp.class_error <> "" Then
                aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, "evo_exporter.aspx.vb Add_Actual_Fields(): " & aclsData_Temp.class_error, DateTime.Now.ToString())
                aclsData_Temp.class_error = ""
              End If
            End If
            'End If
          End If
        End If
      Next

    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb - add_actual_fields() - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  'all this does is add from 1 listbox to the other.
  Public Sub AddBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    attention.Text = ""
    Dim splitstring As Array
    Dim orig_order As String = ""
    Dim temp_name As String = ""

    If Trim(Me.order_by.Text) <> "" Then
      orig_order = Me.order_by.Text & ", "
    End If

    Try
      If choice_to_export.SelectedIndex >= 0 Then
        export_label.Visible = True
        Dim i As Integer
        For i = 0 To choice_to_export.Items.Count - 1
          If choice_to_export.Items(i).Selected Then
            If Not lasset.Contains(choice_to_export.Items(i)) Then
              lasset.Add(choice_to_export.Items(i))
            End If

          End If
        Next i

        Dim fiel As New ListItem
        For i = 0 To lasset.Count - 1
          If Not info_to_export.Items.Contains(CType(lasset(i), ListItem)) Then
            info_to_export.Items.Add(CType(lasset(i), ListItem))
            fiel = CType(lasset(i), ListItem)

            splitstring = Split(lasset(i).Value, "|")

            If InStr(UCase(splitstring(1)), "SELECT") = 0 Then
              If InStr(Trim(splitstring(1)), "rename_") > 0 Then
                order_by_string = order_by_string & splitstring(2) & ", "
              Else
                order_by_string = order_by_string & splitstring(1) & ", "
              End If

            End If


            If Trim(order_by_string) <> "" Then
              Me.order_by.Text = orig_order & Left(Trim(order_by_string), Len(Trim(order_by_string)) - 1)
            End If

          End If
          choice_to_export.Items.Remove(CType(lasset(i), ListItem))



        Next i
      Else
        attention.Text = "<p align='center'>Please select fields to move over</p>"
      End If
    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb - AddBtn_Click() - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  'This adds all of one listbox to another.
  Public Sub AddAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    attention.Text = ""
    Dim splitstring As Array
    Dim orig_order As String = ""
    Dim fiel As New ListItem


    Try
      While choice_to_export.Items.Count <> 0
        export_label.Visible = True
        Dim i As Integer
        For i = 0 To choice_to_export.Items.Count - 1
          If Not lasset.Contains(choice_to_export.Items(i)) Then
            lasset.Add(choice_to_export.Items(i))

            fiel = CType(lasset(i), ListItem)

            splitstring = Split(lasset(i).Value, "|")

            If InStr(UCase(splitstring(1)), "SELECT") = 0 Then
              If InStr(Trim(splitstring(1)), "rename_") > 0 Then
                order_by_string = order_by_string & splitstring(2) & ", "
              Else
                order_by_string = order_by_string & splitstring(1) & ", "
              End If
            Else
              order_by_string = order_by_string & splitstring(1) & ", "
            End If

            If Trim(order_by_string) <> "" Then
              Me.order_by.Text = orig_order & Left(Trim(order_by_string), Len(Trim(order_by_string)) - 1)
            End If
          End If
        Next i


        For i = 0 To lasset.Count - 1
          If Not info_to_export.Items.Contains(CType(lasset(i), ListItem)) Then
            info_to_export.Items.Add(CType(lasset(i), ListItem))

          End If
          choice_to_export.Items.Remove(CType(lasset(i), ListItem))
        Next i
      End While

    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb -AddAllBtn_Click - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  'This removes one entry from a listbox.
  Public Sub RemoveBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)

    Dim temp_field As String = ""
    Dim array_split() As String

    Try
      attention.Text = ""
      If Not (info_to_export.SelectedItem Is Nothing) Then
        Dim i As Integer
        For i = 0 To info_to_export.Items.Count - 1
          If info_to_export.Items(i).Selected Then
            If Not lsubordinate.Contains(info_to_export.Items(i)) Then
              lsubordinate.Add(info_to_export.Items(i))
            End If
          End If
        Next i
        Dim fiel As New ListItem
        For i = 0 To lsubordinate.Count - 1
          If Not choice_to_export.Items.Contains(CType(lsubordinate(i), ListItem)) Then
            choice_to_export.Items.Add(CType(lsubordinate(i), ListItem))
          End If




          info_to_export.Items.Remove(CType(lsubordinate(i), ListItem))


          fiel = CType(lsubordinate(i), ListItem)


          lasset.Add(lsubordinate(i))
          choice_to_export.SelectedValue = fiel.Value


          array_split = Split(fiel.Value, "|")

          If InStr(Trim(array_split(1)), "rename_") > 0 Then
            temp_field = array_split(2)
          Else
            temp_field = array_split(1)
          End If

          If Trim(Me.order_by.Text) <> "" Then
            Me.order_by.Text = Replace(Me.order_by.Text, "," & temp_field, "")
            Me.order_by.Text = Replace(Me.order_by.Text, ", " & temp_field, "")
            Me.order_by.Text = Replace(Me.order_by.Text, "" & temp_field, "")
            Me.order_by.Text = Replace(Me.order_by.Text, "" & temp_field, "")
            ' if we deleted first, then get rid of 
            If Left(Trim(Me.order_by.Text), 1) = "," Then
              Me.order_by.Text = Right(Trim(Me.order_by.Text), Len(Trim(Me.order_by.Text)) - 1)
            End If
          End If

        Next i
      Else
        attention.Text = "<p align='center'>Please select fields to move over</p>"
      End If

      If info_to_export.Items.Count = 0 Then
        export_label.Visible = False
      End If



    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb -RemoveBtn_Click - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  'This removes all from listbox.
  Public Sub RemoveAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    Try
      While info_to_export.Items.Count <> 0
        export_label.Visible = False
        Dim i As Integer
        For i = 0 To info_to_export.Items.Count - 1
          If Not lsubordinate.Contains(info_to_export.Items(i)) Then
            lsubordinate.Add(info_to_export.Items(i))
          End If
        Next i
        Dim fiel As New ListItem
        For i = 0 To lsubordinate.Count - 1
          If Not choice_to_export.Items.Contains(CType(lsubordinate(i), ListItem)) Then
            choice_to_export.Items.Add(CType(lsubordinate(i), ListItem))

            fiel = CType(lsubordinate(i), ListItem)

          End If
          info_to_export.Items.Remove(CType(lsubordinate(i), ListItem))
          lasset.Add(lsubordinate(i))
        Next i
      End While

      Me.order_by.Text = ""


    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb -RemoveAllBtn_Click - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  ' Move listbox item up one
  Protected Sub ButtonMoveUp_Click(ByVal sender As Object, ByVal e As EventArgs)
    Try
      Dim SelectedIndex As Integer = info_to_export.SelectedIndex

      If SelectedIndex = -1 Then
        ' nothing selected
        Return
      End If
      If SelectedIndex = 0 Then
        ' already at top of list  
        Return
      End If

      Dim Temp As ListItem
      Temp = info_to_export.SelectedItem

      info_to_export.Items.Remove(info_to_export.SelectedItem)
      info_to_export.Items.Insert(SelectedIndex - 1, Temp)

    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb -ButtonMoveUp_Click - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  ' Move listbox item down one
  Protected Sub ButtonMoveDown_Click(ByVal sender As Object, ByVal e As EventArgs)
    Try
      Dim SelectedIndex As Integer = info_to_export.SelectedIndex
      If SelectedIndex = -1 Then
        ' nothing selected
        Return
      End If
      If SelectedIndex = info_to_export.Items.Count - 1 Then
        ' already at top of list            
        Return
      End If

      Dim Temp As ListItem
      Temp = info_to_export.SelectedItem

      info_to_export.Items.Remove(info_to_export.SelectedItem)
      info_to_export.Items.Insert(SelectedIndex + 1, Temp)

    Catch ex As Exception
      error_string = "evo_exporter.aspx.vb -ButtonMoveDown_Click - " & ex.Message
      aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
    End Try
  End Sub

  ''' <summary>
  ''' This runs whenever an available datatype is picked. 
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub available_data_types_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles available_data_types.SelectedIndexChanged
    choice_to_export.Items.Clear() 'clear listbox.
    add_sub() 'refill second listbox.
  End Sub

  'runs whenever second listbox is picked
  Private Sub sub_selections_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sub_selections.SelectedIndexChanged
    add_actual_fields() 'refill third listbox
  End Sub

  'Export Now button.
  Private Sub export_now_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_now_btn.Click, run_export.Click, export_now_csv.Click, run_csv_export.Click

    If Trim(Request("view_id")) = "19" Then
      Export_Information_View_19(sender)
    Else
      ' If my_export_list_box.SelectedValue <> "" Then
      'Call selected_export_function(0) ' ADDED MSW - reselect the export before run 
      ' End If

      Export_Information(sender)
    End If

  End Sub

  Private Sub Export_Information_View_19(ByVal sender As Object)

    Dim can_export As Boolean = True
    Dim too_many_fields As Boolean = False
    Dim export_list_box As New ListBox
    Dim column_list As String = ""
    Dim first_column As String = ""
    Dim jetnet As String = ""
    Dim group_by As String = ""
    Dim count_comma_array As String()
    Dim temp_link As String = ""
    Dim temp_name As String = ""


    If sender.id = "export_now_btn" Or sender.id = "save_run_export_template" Then
      export_list_box = info_to_export
    ElseIf sender.id = "run_export" Then
      export_list_box = export_field_list_box
    End If

    For i = 0 To export_list_box.Items.Count - 1
      If Not lasset.Contains(export_list_box.Items(i)) Then

        Dim splitstring As Array = Split(export_list_box.Items(i).Value, "|")


        If i = export_list_box.Items.Count - 1 Then
          column_list = column_list & export_list_box.Items(i).Text
        Else
          column_list = column_list & export_list_box.Items(i).Text & "|"
        End If

        'setting up the actual field list.  
        If InStr(Trim(splitstring(1)), "rename_") > 0 Then
          temp_name = find_rename_field_for_export(splitstring(1))
          jetnet = jetnet & temp_name
        Else
          jetnet = jetnet & splitstring(1)
        End If



        group_by = group_by & splitstring(1) & ","


        If Trim(first_column) = "" Then 'This just holds the first column in a variable for sorting purposes. 
          ' If UBound(splitstring) >= 3 Then
          first_column = splitstring(2)
          'ElseIf UBound(splitstring) >= 2 Then
          '    first_column = splitstring(2)
          ' End If
        End If

        jetnet = jetnet & ","

      End If
    Next i

    too_many_fields = False
    If Trim(jetnet) <> "" Then
      count_comma_array = Split(jetnet, ",")
      If UBound(count_comma_array) > 254 Then
        too_many_fields = True
      End If
    End If


    temp_link = "view_template.aspx?ViewID=19&ViewName=Model Compare&view_items=" & jetnet
    Response.Redirect(temp_link)
  End Sub

  Private Sub Export_Information(ByVal sender As Object)
    Dim listing_id As Integer = 3 'Session.Item("Listing") 'aircraft hard coded for now.
    Dim field_save As String = ""
    Dim company As Boolean = False
    Dim contact As Boolean = False
    Dim aircraft As Boolean = False
    Dim transaction As Boolean = False
    Dim include_phone As Boolean = False
    Dim jetnet As String = ""
    Dim client As String = ""
    Dim column_list As String = ""
    Dim first_column As String = ""
    Dim jetnet_model_id As String = ""
    Dim model_cbo As String = ""
    Dim returned As New DataTable
    Dim group_by As String = ""
    Dim summary As Boolean = False
    attention.Text = "" 'clear error label.
    Dim export_list_box As New ListBox
    Dim is_too_many As Boolean = False
    Dim count_comma_array As String()
    Dim too_many_fields As Boolean = False
    Dim is_jetnet As Boolean = False
    Dim can_export As Boolean = True
    Dim temp_name As String = ""
    Dim OrderByString As String = ""
    Dim temp_string As String = ""

    HttpContext.Current.Session.Item("export_type") = ""
    HttpContext.Current.Session.Item("export_type") = sender.id
    If sender.id = "export_now_btn" Or sender.id = "export_now_csv" Or sender.id = "save_run_export_template" Or sender.id = "run_csv_export" Then
      export_list_box = info_to_export
    ElseIf sender.id = "run_export" Then
      export_list_box = export_field_list_box
    End If

    For i = 0 To export_list_box.Items.Count - 1
      If Not lasset.Contains(export_list_box.Items(i)) Then


        'Dim splitstring As Array = Split(export_list_box.Items(i).Value, "|")
        ''This figures out the type of database field that it is. 
        ''This basically tells the query that we send all this information to
        ''what tables we need. 
        'Select Case splitstring(2)
        '    Case "Company", "Transaction Company"
        '        company = True
        '    Case "Contact", "Transaction Contact"
        '        contact = True
        '    Case "Aircraft", "Aircraft Companies"
        '        aircraft = True
        '    Case "Transaction"
        '        transaction = True
        '    Case "Company Phone", "Contact Phone", "Transaction Company Phone", "Transaction Contact Phone"
        '        include_phone = True
        'End Select


        'If i = 0 Then 'This just holds the first column in a variable for sorting purposes.
        '    first_column = splitstring(3)
        '    'first_column = Replace(first_column, "Feature ", "")
        'End If
        ''setting up the column list.
        'If i = export_list_box.Items.Count - 1 Then
        '    column_list = column_list & export_list_box.Items(i).Text
        'Else
        '    column_list = column_list & export_list_box.Items(i).Text & "|"
        'End If
        ''setting up the actual field list. 
        'jetnet = jetnet & splitstring(1)
        'group_by = group_by & splitstring(1) & ","
        'If InStr(UCase(export_list_box.Items(i).Value), "AS ") > 0 Then
        '    jetnet = jetnet & ","
        'Else
        '    jetnet = jetnet & " as """ & splitstring(3) & """," 'field name as header name.
        'End If



        Dim splitstring As Array = Split(export_list_box.Items(i).Value, "|")


        If i = export_list_box.Items.Count - 1 Then
          column_list = column_list & export_list_box.Items(i).Text
        Else
          column_list = column_list & export_list_box.Items(i).Text & "|"
        End If

        If Trim(column_list) <> "Please Select an Export" Then


          If OrderByString <> "" Then
            OrderByString += ", "
          End If
          OrderByString += splitstring(2)

          'setting up the actual field list.  
          If InStr(Trim(splitstring(1)), "rename_") > 0 Then
            temp_name = find_rename_field_for_export(splitstring(1))
            jetnet = jetnet & temp_name

            group_by = group_by & splitstring(2) & ","

            If Trim(first_column) = "" Then
              first_column = splitstring(2)
            End If

            If InStr(UCase(export_list_box.Items(i).Value), "AS ") > 0 Then
              jetnet = jetnet & ","
            Else
              jetnet = jetnet & " as """ & splitstring(2) & """," 'field name as header name. 
            End If
          Else

            If Trim(first_column) = "" Then
              first_column = splitstring(2)
            End If

            jetnet = jetnet & splitstring(1)
            group_by = group_by & splitstring(1) & ","

            If InStr(UCase(export_list_box.Items(i).Value), "AS ") > 0 Then
              jetnet = jetnet & ","
            Else
              jetnet = jetnet & " as """ & splitstring(2) & """," 'field name as header name. 
            End If
          End If




          '  If Trim(first_column) = "" Then 'This just holds the first column in a variable for sorting purposes. 
          ' If UBound(splitstring) >= 3 Then
          '  first_column = splitstring(2)
          'ElseIf UBound(splitstring) >= 3 Then
          '  first_column = splitstring(3)
          ' ElseIf UBound(splitstring) >= 2 Then
          '  first_column = splitstring(2)
          'End If
          ' End If


          '  If InStr(UCase(export_list_box.Items(i).Value), "AS ") > 0 Then
          ' jetnet = jetnet & ","
          ' Else
          '  If UBound(splitstring) >= 3 Then
          '  jetnet = jetnet & " as """ & splitstring(2) & """," 'field name as header name.
          ' ElseIf UBound(splitstring) >= 2 Then
          '   jetnet = jetnet & " as """ & splitstring(2) & """," 'field name as header name.
          ' End If 
          ' End If

        End If

      End If
    Next i


    If Trim(column_list) <> "Please Select an Export" Then


      Me.order_by.Text = OrderByString

      too_many_fields = False
      If Trim(jetnet) <> "" Then
        count_comma_array = Split(jetnet, ",")
        If UBound(count_comma_array) > 254 Then
          too_many_fields = True
        End If
      End If

      If jetnet <> "" Then 'trims the ending comma.
        jetnet = UCase(jetnet.TrimEnd(","))
      End If
      If group_by <> "" Then
        group_by = UCase(group_by.TrimEnd(","))
      End If

      ' can_export = clsData_Manager_SQL.Check_Sub_Info_For_Export(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)))

      can_export = check_if_can_export()

      ' if its jetnet and not a demo or if its mvintech, then dont put any limitations on
      If InStr(Session.Item("localUser").crmLocalUserEmailAddress, "jetnet.com") > 0 Then
        If InStr(Session.Item("localUser").crmLocalUserEmailAddress, "demo@jetnet.com") = 0 Then
          is_jetnet = True    ' is a jetnet account  - not demo
        Else          ' - is a demo account 
          is_jetnet = False
        End If
      ElseIf InStr(Session.Item("localUser").crmLocalUserEmailAddress, "mvintech.com") > 0 Then
        is_jetnet = True
      End If



      If Not can_export Then  ' added MSW  - 1/29/2014 - to stop demo users from exporting
        attention.Text = "<p align='center'>You are currently on a Demo Account and are not able to use this capability.</p>"
      Else
        If (too_many_fields = True) And (is_jetnet = False) Then
          attention.Text = "<p align='center'>Your exported exceeded the maximum number of fields allowed for custom exports (255 fields).</p>"
        Else
          'If the type is aircraft - defaulted to this type, then we have to get the models from somewhere.
          If (listing_id = 3) Then
            If Not IsNothing(Session.Item("models_export")) Then
              If Not String.IsNullOrEmpty(Session.Item("models_export").ToString) Then
                model_cbo = Session.Item("models_export")
              End If
            End If


            If model_cbo <> "" Then
              model_cbo = Replace(model_cbo, "'", "")
              Dim model_sets As Array = Split(model_cbo, ",")

              For x = 0 To UBound(model_sets)

                Dim model_info As Array = Split(model_sets(x), "|")
                If x = 0 Then
                  jetnet_model_id = "'"
                End If
                jetnet_model_id = jetnet_model_id & model_info(0)
                If x <> UBound(model_sets) Then
                  jetnet_model_id = jetnet_model_id & "','"
                Else
                  jetnet_model_id = jetnet_model_id & "'"
                End If

              Next
              If jetnet_model_id <> "" Then
                jetnet_model_id = UCase(jetnet_model_id.TrimEnd(","))
              End If
            End If
          End If
          ''''''''''''''''''''''''''''''''''''''''''''''''''''end model figure out'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


          If listing_id = 3 Then
            'just url variables. You can get the search fields from anywhere though.
            '
            Dim search As String = Server.UrlDecode(Trim(Request("se")))
            Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))

            Dim market_status_cbo As String = Server.UrlDecode(Trim(Request("ms")))
            Dim subset As String = Server.UrlDecode(Trim(Request("su")))
            Dim airport_name As String = Server.UrlDecode(Trim(Request("an")))
            Dim icao_code As String = Server.UrlDecode(Trim(Request("ic")))
            Dim iata_code As String = Server.UrlDecode(Trim(Request("ia")))
            Dim city As String = Server.UrlDecode(Trim(Request("ci")))
            Dim country_cbo As String = Server.UrlDecode(Trim(Request("co")))
            Dim state As String = Server.UrlDecode(Trim(Request("sta")))
            Dim types_of_owners As String = Server.UrlDecode(Trim(Request("ow")))
            Dim on_exclusive As String = Server.UrlDecode(Trim(Request("ex")))
            Dim on_lease As String = Server.UrlDecode(Trim(Request("le")))
            Dim year_start As String = Server.UrlDecode(Trim(Request("ys")))
            Dim year_end As String = Server.UrlDecode(Trim(Request("ye")))
            Dim state_string As String = ""
            Dim company_table_needed As String = "N"
            'Dim answer As Array

            'If state <> "" Then
            '    Dim states As Array = Split(state, ",")
            '    For x = 0 To UBound(states)

            '        If x = 0 Then
            '            state_string = "'"
            '        End If
            '        state_string = state_string & states(x)
            '        If x <> UBound(states) Then
            '            state_string = state_string & "','"
            '        Else
            '            state_string = state_string & "'"
            '        End If

            '    Next
            'End If

            'If search_where = 2 Then
            'search = "%" & search & "%"
            ' Else
            search = "" & search & "%"
            'End If

            subset = "J"
            Session.Item("localUser").crmUser_DebugText = ""

            If jetnet <> "" Then ' figure out if summary level or not
              If export_type.SelectedValue = "summary" Then
                jetnet = jetnet & " , count(*) as totrecs"
                summary = True
              End If
            End If


            'For i = 0 To info_to_export.Items.Count - 1
            '    answer = Split(info_to_export.Items(i).Value, "|")
            '    If answer(2) = "Aircraft Company" Then 
            '        company_table_needed = "Y"
            '    End If
            'Next

            company_table_needed = "N"


            order_by_string = Me.order_by.Text




                        If is_jetnet Then
                            returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "Y", "", "", "", include_phone, "%", "%", listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_cbo, state_string, types_of_owners, "", jetnet_model_id, on_exclusive, on_lease, "", "", "", "", "", "", year_start, year_end, "", "", group_by, summary, company_table_needed, order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))
                        Else
                            returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "Y", "", "", "", include_phone, "%", "%", listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_cbo, state_string, types_of_owners, "", jetnet_model_id, on_exclusive, on_lease, "", "", "", "", "", "", year_start, year_end, "", "", group_by, summary, company_table_needed, order_by_string, Me.info_to_export, Session("tab"), Session("current_history"), True)
                            ' Response.Write(Session.Item("localUser").crmUser_DebugText)

                            ' moved - MSW - to count the rows returned -7/6/2020 
                            If Not IsNothing(returned) Then
                                If returned.Columns.Count > Session.Item("localUser").crmMaxClientExport Then
                                    is_too_many = True
                                    returned = Nothing
                                Else
                                    ' then we are ok to go 
                                End If
                            End If



                            'If Not IsNothing(returned) Then
                            '    If returned.Columns.Count = 1 Then
                            '        If Not IsDBNull(returned.Rows(0).Item("tcount")) Then
                            '            If CDbl(returned.Rows(0).Item("tcount")) > Session.Item("localUser").crmMaxClientExport Then
                            '                is_too_many = True
                            '                returned = Nothing
                            '            Else
                            '                returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "Y", "", "", "", include_phone, "%", "%", listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_cbo, state_string, types_of_owners, "", jetnet_model_id, on_exclusive, on_lease, "", "", "", "", "", "", year_start, year_end, "", "", group_by, summary, company_table_needed, order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))

                            '                If Not IsNothing(returned) Then
                            '                    If returned.Rows.Count > Session.Item("localUser").crmMaxClientExport Then
                            '                        is_too_many = True
                            '                        returned = Nothing
                            '                    End If
                            '                End If

                            '            End If
                            '        End If
                            '    End If
                            'Else
                            '    returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "Y", "", "", "", include_phone, "%", "%", listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_cbo, state_string, types_of_owners, "", jetnet_model_id, on_exclusive, on_lease, "", "", "", "", "", "", year_start, year_end, "", "", group_by, summary, company_table_needed, order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))
                            'End If
                        End If

                            If Not IsNothing(returned) Then
              Call commonLogFunctions.Log_User_Event_Data("UserExport", "Records: " & returned.Rows.Count & ", Export: " & jetnet, Nothing)
              Call submit_report_request(0, returned.Rows.Count, "Custom")
            Else
              Call commonLogFunctions.Log_User_Event_Data("UserExport", "Export: " & jetnet, Nothing)
            End If


          ElseIf listing_id = 1 Then
            Dim state As String = Server.UrlDecode(Trim(Request("state")))
            Dim owners As String = Server.UrlDecode(Trim(Request("owners")))
            Dim country As String = Server.UrlDecode(Trim(Request("country")))
            Dim subset As String = Server.UrlDecode(Trim(Request("subset")))
            Dim search As String = Server.UrlDecode(Trim(Request("search")))
            Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))
            Dim show_all As Boolean = Server.UrlDecode(Trim(Request("all")))
            Dim status As String = Server.UrlDecode(Trim(Request("st")))
            If search_where = 2 Then
              search = search & "%"
            Else
              search = "%" & search & "%"
            End If

            Dim state_string As String = ""
            If state <> "" Then
              Dim states As Array = Split(state, ",")
              For x = 0 To UBound(states)

                If x = 0 Then
                  state_string = "'"
                End If
                state_string = state_string & states(x)
                If x <> UBound(states) Then
                  state_string = state_string & "','"
                Else
                  state_string = state_string & "'"
                End If

              Next
            End If

            order_by_string = Me.order_by.Text

            If is_jetnet Then
              returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, search, "Y", country, state_string, owners, include_phone, "%", "%", listing_id, "%", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", group_by, summary, "", order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))
            Else
              returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, search, "Y", country, state_string, owners, include_phone, "%", "%", listing_id, "%", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", group_by, summary, "", order_by_string, Me.info_to_export, Session("tab"), Session("current_history"), True)

              If Not IsNothing(returned) Then
                If returned.Columns.Count = 1 Then
                  If Not IsDBNull(returned.Rows(0).Item("tcount")) Then
                    If CDbl(returned.Rows(0).Item("tcount")) > Session.Item("localUser").crmMaxClientExport Then
                      is_too_many = True
                      returned = Nothing
                    Else
                      returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, search, "Y", country, state_string, owners, include_phone, "%", "%", listing_id, "%", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", group_by, summary, "", order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))
                    End If
                  End If
                End If
              Else
                returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, search, "Y", country, state_string, owners, include_phone, "%", "%", listing_id, "%", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", group_by, summary, "", order_by_string, Me.info_to_export, Session("tab"), Session("current_history"))
              End If

            End If

            If Not IsNothing(returned) Then
              Call commonLogFunctions.Log_User_Event_Data("UserExport", "Records: " & returned.Rows.Count & ", Export: " & jetnet, Nothing)
            Else
              Call commonLogFunctions.Log_User_Event_Data("UserExport", "Export: " & jetnet, Nothing)
            End If


          End If

          If Not IsNothing(returned) Then
            If returned.Columns.Count > 0 Then
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - STREAMWRITER</b><br />"
              Dim stringwrite As System.IO.StringWriter = New System.IO.StringWriter
              Dim htmlwrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringwrite)
              Dim htmlOut As New StringBuilder
              gridview1.AllowPaging = False

              If Trim(HttpContext.Current.Session.Item("export_type")) = "export_now_csv" Or Trim(HttpContext.Current.Session.Item("export_type")) = "run_csv_export" Then
                Dim i As Integer = 0
                HttpContext.Current.Session.Item("CSV_Text") = ""
                If Not IsNothing(returned) Then
                  If returned.Rows.Count > 0 Then
                    For Each r As DataRow In returned.Rows

                      For i = 0 To returned.Columns.Count - 1
                        If i > 0 Then
                          htmlOut.Append(",")
                        End If
                        If Not IsDBNull(r.Item(i)) Then
                          htmlOut.Append("""" & Replace(r.Item(i).ToString, "'", "") & """")
                        Else
                          htmlOut.Append("")
                        End If
                      Next

                      htmlOut.Append(Chr(10))
                    Next
                  End If
                End If

              End If

              HttpContext.Current.Session.Item("CSV_Text") = htmlOut.ToString

              gridview1.DataSource = returned
              gridview1.DataBind()
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - Data Bound</b><br />"

              gridview1.RenderControl(htmlwrite)
              ' 
              '  Session("export_info") = "<p align='center'><b style='font-size:19px;'>Custom Export</b></p>" & stringwrite.ToString()
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - Control Rendered</b><br />"

              temp_string = stringwrite.ToString()

              ' THIS IS PUT IN TO FORMAT THE SERIAL NUMBER COLUMN, ;.; IS ADDED TO THE FORM
              ' ADDED MSW - 10/13/17
              temp_string = Replace(temp_string, "<td>;.;", "<td style='mso-number-format:""\@""'>")

              If Not IsNothing(HttpContext.Current.Session.Item("CSV_Text")) Then
                'added MSW - 3/27/19
                HttpContext.Current.Session.Item("CSV_Text") = Replace(HttpContext.Current.Session.Item("CSV_Text"), ";.;", "")
              End If

              If Trim(HttpContext.Current.Session.Item("export_type")) = "export_now_csv" Or sender.id = "run_csv_export" Then
                Session("export_info") = HttpContext.Current.Session.Item("CSV_Text")
              Else
                Session("export_info") = temp_string
              End If
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - String Written</b><br />"
              gridview1.Visible = False
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.open('export.aspx','_blank','width=400,height=400,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');", True)
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - Start Script</b><br />"

              If Application.Item("DebugFlag") = False Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "self.close();", True)
                Response.Redirect("export.aspx", False)
              Else
                Response.Write(Session.Item("localUser").crmUser_DebugText)
              End If
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export_All() as DataTable - Redirect Run</b><br />"

            Else
              Response.Write("<h1>Debug Text</h1>" & Session.Item("localUser").crmUser_DebugText)
              attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
            End If

          Else

            If is_too_many Then
              attention.Text = "<p align='center'>Your exported exceeded the maximum number of records allowed for custom exports (" & Session.Item("localUser").crmMaxClientExport & " records).</p>"
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                'aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, "evo_exporter.aspx.vb Page_Load():" & error_string, DateTime.Now.ToString())
                aclsData_Temp.class_error = ""
                attention.Text = "<p align='center'><font color='red'>You Selected Fields that were not in your original dataset.</font></p>"
              Else
                attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
              End If
            End If
          End If
        End If

      End If

    Else
      attention.Text = "<p align='center'>Please Select a Template</p>"
    End If

  End Sub
  Private Function submit_report_request(ByVal reportNumber As Integer, ByVal reportCount As Integer, ByVal reportName As String) As Boolean

    Dim bReturnValue As Boolean = False

    Dim sQuery = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim results_table As New DataTable

    Dim reportRecCount As Long = 0



    Try

      sQuery.Append("INSERT INTO Report_Request_DotNet (rrdn_sub_comp_id, rrdn_sub_contact_id, rrdn_sub_id, rrdn_login, rrdn_seq_no,")
      sQuery.Append(" rrdn_report_nbr, rrdn_report_query, rrdn_html_flag, rrdn_excel_flag, rrdn_textcomma_flag, rrdn_texttab_flag, rrdn_showheaderrow_flag,")
      sQuery.Append(" rrdn_web_action_date, rrdn_processed_date, rrdn_action_date, rrdn_ftp_file_delete_date,")
      sQuery.Append(" rrdn_reply_username, rrdn_reply_email, rrdn_report_filename, rrdn_zip_filename,")
      sQuery.Append(" rrdn_on_hold, rrdn_status, rrdn_total_query_records, rrdn_host_name, rrdn_app_name,")
      sQuery.Append(" rrdn_include_confidential, rrdn_include_contacts, rrdn_include_all_contacts, rrdn_include_all_base_aircraft,")
      sQuery.Append(" rrdn_include_asking_price, rrdn_add_to_asking_price, rrdn_company_subquery, rrdn_contact_subquery, rrdn_progholder_id ")

      sQuery.Append(", rrdn_total_report_records ")

      sQuery.Append(") VALUES (")

      sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + ",") ' rrdn_sub_comp_id
      sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + ",") ' rrdn_sub_contact_id
      sQuery.Append(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + ",") ' rrdn_sub_id
      sQuery.Append("'" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "',") ' rrdn_login
      sQuery.Append(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + ",") ' rrdn_seq_no

      sQuery.Append("0,")   '  rrdn_report_no

      sQuery.Append("'" + clsData_Manager_SQL.FormatForSQL(HttpContext.Current.Session.Item("MasterAircraft").ToString.ToLower) + "',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'1900-01-01',NULL,NULL,NULL,") ' rrdn_web_action_date, rrdn_processed_date, rrdn_action_date, rrdn_ftp_file_delete_date

      sQuery.Append("'david@jetnet.com',") ' rrdn_reply_username
      sQuery.Append("'david@jetnet.com',") ' rrdn_reply_email

      sQuery.Append("'',") ' rrdn_report_filename 
      sQuery.Append("'',") ' rrdn_zip_filename 
      sQuery.Append("'Y',") ' rrdn_on_hold 

      sQuery.Append("'Started .NET Report (#" + reportNumber.ToString + ") " + reportName + " - AT " + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "',")

      sQuery.Append(reportCount.ToString + ",") ' rrdn_total_query_records 

      sQuery.Append("'" + HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString.Trim + "',") ' rrdn_host_name 
      sQuery.Append("'" + HttpContext.Current.Application.Item("crmClientSiteData").webSiteHostName(HttpContext.Current.Session.Item("jetnetWebHostType")).ToString + "',") ' rrdn_app_name 


      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("'N',")

      sQuery.Append("0,")

      sQuery.Append("NULL,")

      sQuery.Append("NULL,")


      sQuery.Append("0 ") ' rrdn_progholder_id  


      sQuery.Append(", " & reportCount.ToString & "  ")


      sQuery.Append(")")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>AddRecordToReportRequest(ByVal reportNumber As Integer, ByVal reportName As String, ByVal reportRecCount As Integer) As Boolean<br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
        bReturnValue = True
      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in AddRecordToReportRequest SqlCommand.ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in AddRecordToReportRequest(ByVal reportNumber As Integer, ByVal reportName As String, ByVal reportRecCount As Integer) As Boolean" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return bReturnValue

  End Function
  Private Sub Export_Information_Original(ByVal sender As Object)
    Dim listing_id As Integer = Session.Item("Listing") 'aircraft hard coded for now.
    Dim field_save As String = ""
    Dim company As Boolean = False
    Dim contact As Boolean = False
    Dim aircraft As Boolean = False
    Dim transaction As Boolean = False
    Dim include_phone As Boolean = False
    Dim jetnet As String = ""
    Dim client As String = ""
    Dim column_list As String = ""
    Dim first_column As String = ""
    Dim jetnet_model_id As String = ""
    Dim model_cbo As String = ""
    Dim returned As New DataTable
    Dim group_by As String = ""
    Dim summary As Boolean = False
    attention.Text = "" 'clear error label.
    Dim export_list_box As New ListBox

    If sender.id = "export_now_btn" Or sender.id = "save_run_export_template" Then
      export_list_box = info_to_export
    ElseIf sender.id = "run_export" Then
      export_list_box = export_field_list_box
    End If

    For i = 0 To export_list_box.Items.Count - 1
      If Not lasset.Contains(export_list_box.Items(i)) Then
        Dim splitstring As Array = Split(export_list_box.Items(i).Value, "|")
        'This figures out the type of database field that it is. 
        'This basically tells the query that we send all this information to
        'what tables we need. 
        Select Case splitstring(2)
          Case "Company", "Transaction Company"
            company = True
          Case "Contact", "Transaction Contact"
            contact = True
          Case "Aircraft", "Aircraft Companies"
            aircraft = True
          Case "Transaction"
            transaction = True
          Case "Company Phone", "Contact Phone", "Transaction Company Phone", "Transaction Contact Phone"
            include_phone = True
        End Select

        If i = 0 Then 'This just holds the first column in a variable for sorting purposes.
          first_column = splitstring(3)
          'first_column = Replace(first_column, "Feature ", "")
        End If

        'setting up the column list.
        If i = export_list_box.Items.Count - 1 Then
          column_list = column_list & export_list_box.Items(i).Text
        Else
          column_list = column_list & export_list_box.Items(i).Text & "|"
        End If
        'setting up the actual field list. 
        jetnet = jetnet & splitstring(1)
        group_by = group_by & splitstring(1) & ","
        If InStr(UCase(export_list_box.Items(i).Value), "AS ") > 0 Then
          jetnet = jetnet & ","
        Else
          jetnet = jetnet & " as """ & splitstring(2) & """," 'field name as header name.
        End If

      End If
    Next i


    If jetnet <> "" Then 'trims the ending comma.
      jetnet = UCase(jetnet.TrimEnd(","))
    End If
    If group_by <> "" Then
      group_by = UCase(group_by.TrimEnd(","))
    End If

    'If the type is aircraft - defaulted to this type, then we have to get the models from somewhere.
    If (listing_id = 3) Then
      If Not IsNothing(Session.Item("models_export")) Then
        If Not String.IsNullOrEmpty(Session.Item("models_export").ToString) Then
          model_cbo = Session.Item("models_export")
        End If
      End If


      If model_cbo <> "" Then
        model_cbo = Replace(model_cbo, "'", "")
        Dim model_sets As Array = Split(model_cbo, ",")

        For x = 0 To UBound(model_sets)

          Dim model_info As Array = Split(model_sets(x), "|")
          If x = 0 Then
            jetnet_model_id = "'"
          End If
          jetnet_model_id = jetnet_model_id & model_info(0)
          If x <> UBound(model_sets) Then
            jetnet_model_id = jetnet_model_id & "','"
          Else
            jetnet_model_id = jetnet_model_id & "'"
          End If

        Next
        If jetnet_model_id <> "" Then
          jetnet_model_id = UCase(jetnet_model_id.TrimEnd(","))
        End If
      End If
    End If
    ''''''''''''''''''''''''''''''''''''''''''''''''''''end model figure out'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    If listing_id = 3 Then
      'just url variables. You can get the search fields from anywhere though.
      '
      Dim search As String = Server.UrlDecode(Trim(Request("se")))
      Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))

      Dim market_status_cbo As String = Server.UrlDecode(Trim(Request("ms")))
      Dim subset As String = Server.UrlDecode(Trim(Request("su")))
      Dim airport_name As String = Server.UrlDecode(Trim(Request("an")))
      Dim icao_code As String = Server.UrlDecode(Trim(Request("ic")))
      Dim iata_code As String = Server.UrlDecode(Trim(Request("ia")))
      Dim city As String = Server.UrlDecode(Trim(Request("ci")))
      Dim country_cbo As String = Server.UrlDecode(Trim(Request("co")))
      Dim state As String = Server.UrlDecode(Trim(Request("sta")))
      Dim types_of_owners As String = Server.UrlDecode(Trim(Request("ow")))
      Dim on_exclusive As String = Server.UrlDecode(Trim(Request("ex")))
      Dim on_lease As String = Server.UrlDecode(Trim(Request("le")))
      Dim year_start As String = Server.UrlDecode(Trim(Request("ys")))
      Dim year_end As String = Server.UrlDecode(Trim(Request("ye")))
      Dim state_string As String = ""

      If state <> "" Then
        Dim states As Array = Split(state, ",")
        For x = 0 To UBound(states)

          If x = 0 Then
            state_string = "'"
          End If
          state_string = state_string & states(x)
          If x <> UBound(states) Then
            state_string = state_string & "','"
          Else
            state_string = state_string & "'"
          End If

        Next
      End If

      If search_where = 2 Then
        search = "%" & search & "%"
      Else
        search = "" & search & "%"
      End If

      subset = "J"
      Session.Item("localUser").crmUser_DebugText = ""

      If jetnet <> "" Then ' figure out if summary level or not
        If export_type.SelectedValue = "summary" Then
          jetnet = jetnet & " , count(*) as totrecs"
          summary = True
        End If
      End If
      returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "Y", "", "", "", include_phone, "%", "%", listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_cbo, state_string, types_of_owners, "", jetnet_model_id, on_exclusive, on_lease, "", "", "", "", "", "", year_start, year_end, "", "", group_by, summary, "", "", Me.info_to_export, Session("tab"), Session("current_history"))
      ' Response.Write(Session.Item("localUser").crmUser_DebugText)

    ElseIf listing_id = 1 Then
      Dim state As String = Server.UrlDecode(Trim(Request("state")))
      Dim owners As String = Server.UrlDecode(Trim(Request("owners")))
      Dim country As String = Server.UrlDecode(Trim(Request("country")))
      Dim subset As String = Server.UrlDecode(Trim(Request("subset")))
      Dim search As String = Server.UrlDecode(Trim(Request("search")))
      Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))
      Dim show_all As Boolean = Server.UrlDecode(Trim(Request("all")))
      Dim status As String = Server.UrlDecode(Trim(Request("st")))
      If search_where = 2 Then
        search = search & "%"
      Else
        search = "%" & search & "%"
      End If

      Dim state_string As String = ""
      If state <> "" Then
        Dim states As Array = Split(state, ",")
        For x = 0 To UBound(states)

          If x = 0 Then
            state_string = "'"
          End If
          state_string = state_string & states(x)
          If x <> UBound(states) Then
            state_string = state_string & "','"
          Else
            state_string = state_string & "'"
          End If

        Next
      End If

      returned = Export_Evo(first_column, client, jetnet, company, contact, aircraft, transaction, subset, search, "Y", country, state_string, owners, include_phone, "%", "%", listing_id, "%", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", group_by, summary, "", "", Me.info_to_export, Session("tab"), Session("current_history"))
    End If

    If Not IsNothing(returned) Then
      If returned.Columns.Count > 0 Then
        Dim stringwrite As System.IO.StringWriter = New System.IO.StringWriter
        Dim htmlwrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringwrite)
        gridview1.AllowPaging = False

        gridview1.DataSource = returned
        gridview1.DataBind()


        gridview1.RenderControl(htmlwrite)
        ' 
        ' Session("export_info") = "<p align='center'><b style='font-size:19px;'>Custom Export</b></p>" & stringwrite.ToString()
        Session("export_info") = stringwrite.ToString()
        gridview1.Visible = False
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.open('export.aspx','_blank','width=400,height=400,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');", True)

        If Application.Item("DebugFlag") = False Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "self.close();", True)
          Response.Redirect("export.aspx", False)
        Else
          Response.Write(Session.Item("localUser").crmUser_DebugText)
        End If


      Else
        Response.Write("<h1>Debug Text</h1>" & Session.Item("localUser").crmUser_DebugText)
        attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
      End If

    Else

      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, "evo_exporter.aspx.vb Page_Load():" & error_string, DateTime.Now.ToString())
        aclsData_Temp.class_error = ""
      Else
        attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
      End If

    End If

  End Sub

  'clears all fields in info_to_export.
  Private Sub clearselectedfields_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles clearselectedfields.Click
    info_to_export.Items.Clear()
    Me.order_by.Text = ""
  End Sub

  Public Sub clicked_modify_export(ByVal sender As Object, ByVal e As System.EventArgs)

    Dim cssClass As String = "export_save_options"
    Dim answer As Array = Split(my_export_list_box.SelectedValue, "|||")
    available_fields_swap.Visible = False
    Me.save_as_export_btn.Visible = True
    Me.delete_warning.Visible = False

    save_export_form.Visible = True
    subject.Text = ""
    description.Value = ""
    save_export_template.Text = "Save"
    Me.shared_panel.Visible = True

    If IsNothing(sender) = True And IsNothing(e) = True Then
      ' for the delete ----------------------------------------------------------
      save_as_export_buttons.Visible = True
      save_export_template.Visible = False
      Me.save_as_export_btn.Visible = False
      Me.delete_warning.Visible = True

      info_to_export.Items.Clear()
      available_data_types.SelectedValue = "Aircraft"
      subject.Text = my_export_title.Text

      If Trim(my_export_title.Text) <> "" Then
        delete_warning.Text = "<table><tr><td nowrap='nowrap'>Are You Sure You Want to Delete The Template '" & my_export_title.Text & "'?</td></tr></table>"
      Else
        delete_warning.Text = "<table><tr><td nowrap='nowrap'>Are You Sure You Want to Delete This Template?</td></tr></table>"
      End If




      description.Value = my_export_description.Text
      export_id.Text = export_id_hold.Text

      For i = 0 To export_field_list_box.Items.Count - 1
        ' If export_field_list_box.Items(i).Selected Then
        info_to_export.Items.Add(New ListItem(export_field_list_box.Items(i).Text, export_field_list_box.Items(i).Value))
        'End If
      Next

      cssClass = "export_modify_options"
      If UBound(answer) = 1 Then
        description.Value = answer(0)
        export_id.Text = answer(1)
        subject.Text = my_export_list_box.SelectedItem.Text
      End If
      ' for the delete ----------------------------------------------------------
    ElseIf sender.id = "modify_export" Then

      If TabContainer1.ActiveTabIndex = 0 Then
        ' you cant modift a common template 
        subject.Text = ""
      Else
        save_export_template.Text = "Save As.."
        save_as_export_buttons.Visible = True


        info_to_export.Items.Clear()
        ' subject.Text = "Special Export Form at Aircraft Locations"
        ' description.Text = "Another export designed to showing where aircraft are physically located."
        available_data_types.SelectedValue = "Aircraft"
        'info_to_export.Items.Add(New ListItem("Model", "amod_model_name"))
        'info_to_export.Items.Add(New ListItem("Serial #", "ac_ser_no"))
        'info_to_export.Items.Add(New ListItem("Registration #", "ac_reg_no"))
        'info_to_export.Items.Add(New ListItem("Aircraft Base City", "ac_aport_city"))
        'info_to_export.Items.Add(New ListItem("Aircraft Base State", "ac_aport_state"))
        'info_to_export.Items.Add(New ListItem("Aircraft Base Country", "ac_aport_country"))

        subject.Text = my_export_title.Text
        description.Value = my_export_description.Text
        export_id.Text = export_id_hold.Text

        For i = 0 To export_field_list_box.Items.Count - 1
          ' If export_field_list_box.Items(i).Selected Then
          info_to_export.Items.Add(New ListItem(export_field_list_box.Items(i).Text, export_field_list_box.Items(i).Value))
          'End If
        Next


        cssClass = "export_modify_options"
        If UBound(answer) = 1 Then
          description.Value = answer(0)
          export_id.Text = answer(1)
          subject.Text = my_export_list_box.SelectedItem.Text
        End If

      End If

    Else
      If export_id.Text <> "" Then
        subject.Text = my_export_title.Text
        description.Value = my_export_description.Text
        export_id.Text = export_id_hold.Text
      End If
    End If

    If IsNothing(sender) = True And IsNothing(e) = True Then
    ElseIf sender.id = "modify_export" And TabContainer1.ActiveTabIndex = 0 Then

    Else

      'save_export_form.CssClass = cssClass
      title_panel.Visible = True

      tabs_container.ActiveTab = customize_tab

      'make the buttons invisible that we do not need.
      clearselectedfields.Visible = False
      edit_selected_fields.Visible = True
      export_label.Visible = False
      move_up.Visible = False
      move_down.Visible = False
      save_export_buttons.Visible = True
      export_type.Enabled = False
      format_options.Enabled = False


      attention.Text = ""
      label_text.Text = "<p> " &
                           "Please enter or modify your subject and description and press the Save Button to save your Template, or the Save/Run button to save and then run your template."

    End If

  End Sub

  Private Sub modify_export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles modify_export.Click, create_new_export.Click

    Call clicked_modify_export(sender, e)

  End Sub

  'This just writes the query to the screen if you're going to save the export template.
  Private Sub save_export_template_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_export_template.Click

    SaveExport()

  End Sub

  Private Sub SaveExport()
    Dim returned As Integer = 0
    Dim return_string As String = ""
    Dim tab_type As String = ""
    Dim items_string As String = ""
    Dim answer2 As Array

    ' look through the items in the list 
    For i = 0 To info_to_export.Items.Count - 1
      answer2 = Split(info_to_export.Items(i).Value, "|")
      items_string = items_string & answer2(1)
    Next

    'see what table we are going to be using 
    If InStr(UCase(items_string), "COMP_") > 0 Or InStr(UCase(items_string), "CREF_") > 0 Or InStr(UCase(items_string), "CONTACT_") > 0 Then
      tab_type = "View_Aircraft_Company_Flat"
    ElseIf Trim(UCase(Session("tab"))) = "AIRCRAFT" And Trim(UCase(current_history)) = "HISTORY" Then
      tab_type = "from View_Aircraft_History_Flat"
    ElseIf Trim(UCase(Session("tab"))) = "AIRCRAFT" And Trim(UCase(current_history)) = "" Then
      tab_type = "from View_Aircraft_Flat"
    ElseIf Trim(UCase(Session("tab"))) = "COMPANY" Then
      tab_type = "from View_Aircraft_Company_Flat"
    Else
      tab_type = "from View_Aircraft_Flat"
    End If



    If info_to_export.Items.Count = 0 Then
      attention.Text = "<p align='center'>Please choose information to save first.</p>"
    Else
      attention.Text = ""
      If subject.Text.Length < 50 Then
        If description.Value.Length < 500 Then
        Else
          attention.Text = "<p align='center'><font color='red'>Your Export Description Can't Exceed 500 Characters. It has been truncated.</font></p>"
        End If
      Else
        attention.Text = "<p align='center'><font color='red'>Your Export Subject Can't Exceed 50 Characters. It has been truncated.</font></p>"
      End If

      description.Value = Left(description.Value, 499)
      subject.Text = Left(subject.Text, 49)

      ' if its save as, make run_export button invisible
      Me.run_export.Visible = False

      returned = aclsData_Temp.Insert_Subscription_Install_Saved_Exports(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, subject.Text, description.Value, "N", Session("tab"), tab_type, Me.export_type.SelectedValue, Me.shared_check.Checked)
      'returned = 1
      If Not IsNothing(returned) Then
        If returned <> 0 Then
          export_id.Text = returned
          For i = 0 To info_to_export.Items.Count - 1
            Dim answer As Array = Split(info_to_export.Items(i).Value, "|")

            If UBound(answer) >= 4 Then
              aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(returned, info_to_export.Items(i).Value, i)
            ElseIf UBound(answer) >= 3 Then
              aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(returned, info_to_export.Items(i).Value, i)
            ElseIf UBound(answer) >= 2 Then
              'Response.Write(answer(3) & "!!")
              '  aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(returned, answer(3), i) ' changed from 3 to 2, there is only 3 items in 0 spot array
              aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(returned, info_to_export.Items(i).Value, i)
            End If

            ' Response.Write("insert into Subscription_Install_Saved_Export_Fields (sisef_header_field_name, sisef_seq_no) values('" & info_to_export.Items(i).Value & "','" & i & "')<br />")

          Next

          If attention.Text.ToString.Trim <> "" Then
            attention.Text &= "<p align='center'>Your Template has been Saved.</p>"
          Else
            attention.Text = "<p align='center'>Your Template has been Saved.</p>"
          End If

          Fill_Available_Templates("", "")
          tabs_container.ActiveTabIndex = 0

          If bFromPreferences Then

            bRefreshPreferences = True
            TabContainer1.ActiveTabIndex = 1
            tabs_container.ActiveTabIndex = 1

          End If

        End If
      End If

    End If
  End Sub

  Private Sub export_type_rad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_type.SelectedIndexChanged
    info_to_export.Items.Clear()
    choice_to_export.Items.Clear()
    'add_actual_fields()
    sub_selections.Items.Clear()
    add_sub()
  End Sub

  Private Sub my_common_export_list_box_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles common_export_list_box.SelectedIndexChanged

    Session.Item("list_type") = "common"

    my_export_list_box_SelectedIndexChanged(sender, e)

  End Sub

  Public Function selected_export_function(ByVal clicked_id As Integer)
    selected_export_function = ""
    Dim cef_display As String = ""
    Dim cef_id As Integer = 0
    Dim cef_evo_field_name As String = ""
    ' Dim cef_main_group As String = ""
    export_field_list_box.Items.Clear()
    info_to_export.Items.Clear()
    Dim cef_header_field_name As String = ""
    Dim id_array() As String
    Dim user_temp As String = ""
    Dim sise_description As String = ""
    Dim sise_id As Long = 0
    Dim sise_subject As String = ""
    Dim sise_share_flag As String = ""
    Dim Is_admin As Boolean = False
    Dim rename_field_name As String = ""
    Dim temp_count As Integer = 0
    Dim temp_user_login As String = ""

    'common_export_list_box

    If Trim(UCase(Request("export_type"))) <> "" Then
      Session("tab") = Trim(UCase(Request("export_type")))
    End If

    If clicked_id <> 0 Then
      For temp_count = 0 To my_export_list_box.Items.Count - 1
        If my_export_list_box.Items(temp_count).Value = clicked_id Then
          my_export_list_box.SelectedIndex = temp_count
        End If
      Next
    End If

    If my_export_list_box.SelectedValue <> "" Or common_export_list_box.SelectedValue <> "" Then
      export_info.Visible = True
      export_instructions.Visible = False

      Me.no_permission.Visible = False

      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        Is_admin = True
      End If

      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        Me.modify_export.Visible = True
        Me.no_permission.Visible = False
        aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, False, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, temp_user_login)

        my_export_description.Text = sise_description
        my_export_title.Text = sise_subject
        export_id_hold.Text = sise_id

        If Trim(sise_share_flag) = "Y" Then
          Me.shared_check.Checked = True
        Else
          Me.shared_check.Checked = False
        End If

        If Trim(temp_user_login) <> Trim(CStr(Trim(Session.Item("localUser").crmUserLogin))) Then
          Me.edit_warning.Visible = True
        End If

      Else
        If Trim(Session.Item("list_type")) = "" Then
          aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, False, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          If IsNothing(aTempTable) Then
            If Is_admin = False Then
              Me.modify_export.Visible = False ' its not urs to edit, but re-run to show details
              Me.no_permission.Visible = True
            Else
              Me.modify_export.Visible = True
            End If
            aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, True, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          ElseIf aTempTable.Rows.Count = 0 Then
            If Is_admin = False Then
              Me.modify_export.Visible = False ' its not urs to edit, but re-run to show details
              Me.no_permission.Visible = True
            Else
              Me.modify_export.Visible = True
            End If
            aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, True, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          Else
            Me.modify_export.Visible = True
          End If
          common_export_list_box.SelectedIndex = -1
        Else
          aTempTable = aclsData_Temp.Select_Subscription_Install(6266, CStr(Trim("exporttemplates")), 1, common_export_list_box.SelectedValue, Me.export_type, False, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          If IsNothing(aTempTable) Then
            If Is_admin = False Then
              Me.modify_export.Visible = False ' its not urs to edit, but re-run to show details
              Me.no_permission.Visible = True
            Else
              Me.modify_export.Visible = True
            End If
            aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, True, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          ElseIf aTempTable.Rows.Count = 0 Then
            If Is_admin = False Then
              Me.modify_export.Visible = False ' its not urs to edit, but re-run to show details
              Me.no_permission.Visible = True
            Else
              Me.modify_export.Visible = True
            End If
            aTempTable = aclsData_Temp.Select_Subscription_Install(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, my_export_list_box.SelectedValue, Me.export_type, True, user_temp, sise_description, sise_subject, sise_id, sise_share_flag, "")
          End If
          my_export_list_box.SelectedIndex = -1
        End If

        my_export_description.Text = sise_description
        my_export_title.Text = sise_subject
        export_id_hold.Text = sise_id


        ' if both = Y then share 
        ' If Trim(aTempTable.Rows(0).Item("sub_share_by_comp_id_flag")) = "Y" Then ' if your company shares, 


        If Trim(sise_share_flag) = "Y" Then
          Me.shared_check.Checked = True
        Else
          Me.shared_check.Checked = False
        End If
        'nd If

        Me.no_permission.Text = "No Permission to Modify this Template.<br/>The creator [" & user_temp & "] has permission to edit.<br>"
      End If



      Me.order_by.Text = ""

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then


          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("sisef_header_field_name")) Then
              order_by_string = ""

              If InStr(r("sisef_header_field_name"), "|") = 0 Then
                ' old way of displaying 
                export_id_hold.Text = export_id_hold.Text
              Else

                id_array = Split(r("sisef_header_field_name"), "|")
                If Trim(id_array(0)) <> "" Then

                  atemptable2 = aclsData_Temp.Fill_Available_Data_Fields_Based_On_Sub_Group("", False, Session("tab"), r("sisef_header_field_name"), id_array(0), Session.Item("localSubscription").crmAerodexFlag) 'run the function

                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each q As DataRow In atemptable2.Rows
                        If Not IsDBNull(q("cef_display")) Then
                          cef_display = q("cef_display")
                        End If
                        If Not IsDBNull(q("cef_id")) Then
                          cef_id = q("cef_id")
                        End If

                        'If Not IsDBNull(q("cef_main_group")) Then
                        '  cef_main_group = q("cef_main_group")
                        'End If

                        cef_header_field_name = ""
                        If Not IsDBNull(q("cef_header_field_name")) Then
                          cef_header_field_name = q("cef_header_field_name")
                        End If

                        cef_evo_field_name = ""
                        If Not IsDBNull(q("cef_client_field_name")) Then
                          cef_evo_field_name = q("cef_client_field_name")
                        Else
                          If Not IsDBNull(q("cef_evo_field_name")) Then
                            cef_evo_field_name = q("cef_evo_field_name")
                          End If
                        End If


                        export_field_list_box.Items.Add(New ListItem(q("cef_display"), cef_id & "|" & cef_evo_field_name & "|" & cef_header_field_name))

                        info_to_export.Items.Add(New ListItem(q("cef_display"), cef_id & "|" & cef_evo_field_name & "|" & cef_header_field_name))


                        If InStr(UCase(id_array(1)), "SELECT") = 0 Then
                          If InStr(Trim(id_array(1)), "rename_") > 0 Then
                            order_by_string = id_array(2)
                          Else
                            order_by_string = id_array(1)
                          End If
                        Else
                          order_by_string = id_array(1)
                        End If


                        If InStr(UCase(order_by_string), "SELECT") = 0 Then
                          If Trim(Me.order_by.Text) <> "" Then
                            Me.order_by.Text = Me.order_by.Text & " ," & order_by_string
                          Else
                            Me.order_by.Text = order_by_string
                          End If
                        End If
                      Next
                    End If
                  End If
                End If



              End If
            End If
          Next

        End If
      End If

      Session.Item("list_type") = ""

    End If
  End Function

  Private Sub export_types_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_types.SelectedIndexChanged

    Call Fill_Available_Templates(Me.export_types.SelectedValue, "")

    Me.help_link.Text = "&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;<span class=""red_text"">" & Me.my_export_list_box.Items.Count & " Templates Selected.</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

  End Sub

  Private Sub my_export_list_box_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles my_export_list_box.SelectedIndexChanged

    Call selected_export_function(0)

  End Sub

  Private Sub tabs_container_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabs_container.ActiveTabChanged
    If tabs_container.ActiveTabIndex = 0 Then
      label_text.Text = "  <p>Please select from the list of previously stored export templates to the left or " &
                        " click on 'Create New Export'. If you have selected a previously stored template then you can click on either ""Modify Export"" if you wish to change the template " &
                        " or ""Run Export"" to create your export file. "
    Else
      label_text.Text = "<p>" & "Please select from the list of available fields using the arrows below the list. " &
                                  "Once you have your desired fields in the ""Fields to Export"" list then click " &
                                  "on ""Run Export"" to generate the desired export."

    End If
  End Sub

  Private Sub cancel_save_export_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles edit_selected_fields.Click
    save_export_form.Visible = False

    create_new_export.Enabled = True

    create_new_export.Text = "Edit Form"
    move_up.Visible = True
    move_down.Visible = True
    clearselectedfields.Visible = True
    edit_selected_fields.Visible = False
    export_label.Visible = True

    available_fields_swap.Visible = True

    export_type.Enabled = True
    format_options.Enabled = True

    Me.edit_selected_fields.Visible = False

  End Sub

  Private Sub delete_custom_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delete_custom_button.Click

    Call delete_template(CInt(export_id.Text))

  End Sub

  Private Sub delete_template(ByVal temp_id As Integer)
    Dim returned As Integer = 0

    If export_id.Text <> "" Then
      Call commonLogFunctions.Log_User_Event_Data("UserCustomExportDelete", "CUSTOM EXPORT DELETED: " & Me.selected_name.Text & "(" & temp_id & ")", Nothing, 0, 0, 0, 0, 0, 0, 0)
      returned = aclsData_Temp.Delete_Saved_Export(temp_id)

      If returned = 1 Then
        Call Fill_Available_Templates(sExportType, "")
      End If
    End If

    If bFromPreferences Then

      subject.Text = ""
      description.Value = ""
      info_to_export.Items.Clear()
      bRefreshPreferences = True

    End If

  End Sub

  Private Sub save_as_export_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_as_export_btn.Click
    'Response.Write("update!! #" & export_id.Text)
    Dim returned As Integer = 0

    If export_id.Text <> "" Then

      attention.Text = ""
      If subject.Text.Length < 50 Then
        If description.Value.Length < 500 Then
        Else
          attention.Text = "<p align='center'><font color='red'>Your Export Description Can't Exceed 500 Characters. It has been truncated.</font></p>"
        End If
      Else
        attention.Text = "<p align='center'><font color='red'>Your Export Subject Can't Exceed 50 Characters. It has been truncated.</font></p>"
      End If

      description.Value = Left(description.Value, 499)
      subject.Text = Left(subject.Text, 49)

      If Trim(subject.Text) = "" Then
        attention.Text = "<p align='center'><font color='red'>Your Export Cannot Have a Blank Subject. It has not been saved.</font></p>"
      Else
        returned = aclsData_Temp.Update_Saved_Export_Fields(Session.Item("localUser").crmSubSubID, CStr(Trim(Session.Item("localUser").crmUserLogin)), Session.Item("localUser").crmSubSeqNo, export_id.Text, subject.Text, description.Value, Me.export_type.SelectedValue, Me.shared_check.Checked)

        If returned <> 0 Then
          For i = 0 To info_to_export.Items.Count - 1
            Dim answer As Array = Split(info_to_export.Items(i).Value, "|")



            If UBound(answer) >= 3 Then
              aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(export_id.Text, info_to_export.Items(i).Value, i)
            ElseIf UBound(answer) >= 2 Then
              'Response.Write(answer(3) & "!!")
              aclsData_Temp.Insert_Subscription_Install_Saved_Export_Fields(export_id.Text, info_to_export.Items(i).Value, i)
            End If
          Next

          If attention.Text.ToString.Trim <> "" Then
            attention.Text &= "<p align='center'>Your Template has been Saved.</p>"
          Else
            attention.Text = "<p align='center'>Your Template has been Saved.</p>"
          End If

          Fill_Available_Templates("", "")
          selected_export_function(export_id.Text)

          If bFromPreferences Then

            bRefreshPreferences = True

          End If

        Else

        End If
      End If
    End If

  End Sub

  Private Sub save_run_export_template_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_run_export_template.Click
    SaveExport()
    attention.Text = "<p align='center'>Your Information has been Saved</p>"
    Export_Information(sender)


  End Sub

  Private Sub create_new_export_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles create_new_export_btn.Click
    tabs_container.ActiveTab = customize_tab

    ' if you hit the creat new button
    subject.Text = ""
    description.Value = ""
    export_id.Text = ""
    Me.info_to_export.Items.Clear()
    Me.save_as_export_btn.Visible = False
    Me.save_export_template.Text = "Save"

  End Sub

  Public Sub get_fields_for_add_comparable(ByVal NOTE_ID As Long, ByRef drop_list As ListBox, ByVal selected_field As String)

    Dim Query As String = ""
    Dim results_table As New DataTable
    Dim temp_name As String = ""
    Dim temp_val As String = ""
    Dim found_spot As Boolean = False

    Query = " SELECT clivalch_id, clivalch_order, clivalch_name, clivalch_db_name, clivalch_trans_db_name, clivalch_closed_db_name,  "
    Query &= " clivalch_description " '
    Query &= "  from client_value_field_choice "
    If Trim(selected_field) = "" Then
      Query &= " where clivalch_id not in (SELECT distinct clivalfld_choice_id "
      Query &= "  from client_value_fields "
      Query &= " where clivalfld_val_id = " & NOTE_ID & " ) "
    Else
      Query &= " where clivalch_name = '" & Trim(selected_field) & "' "
    End If

    Query &= " and clivalch_db_name like 'cliaircraft_custom_%' "
    Query &= " order by clivalch_order"

    results_table = localDatalayer.Get_Compare_Query(Query, "get_fields_for_add_comparable")

    If Not IsNothing(results_table) Then

      If results_table.Rows.Count > 0 Then

        drop_list.Items.Clear()

        For Each r As DataRow In results_table.Rows


          If Not IsDBNull(r("clivalch_name")) Then
            temp_name = r("clivalch_name")
          Else
            temp_name = ""
          End If

          If Not IsDBNull(r("clivalch_db_name")) Then
            temp_val = r("clivalch_db_name")
          Else
            temp_val = " "
          End If

          drop_list.Items.Add(New ListItem(temp_name, r("clivalch_id") & "|" & temp_val & "|" & temp_name))

        Next


        If found_spot = False Then
          drop_list.SelectedIndex = 0
        End If

      End If
    End If



  End Sub

  Public Function Export_Evo(ByVal first_column As String, ByVal client_string As String, ByVal jetnet_string As String,
                             ByVal company As Boolean, ByVal contact As Boolean, ByVal aircraft As Boolean, ByVal transaction As Boolean,
                             ByVal data_subset As String, ByVal comp_name As String, ByVal status As String, ByVal country As String, ByVal state As String,
                             ByVal operator_type As String, ByVal include_phone As Boolean, ByVal f_name As String, ByVal l_name As String, ByVal originating_type As Integer,
                             ByVal ac_search As String, ByVal market_status As String, ByVal airport_name As String, ByVal icao_code As String, ByVal iata_code As String, ByVal ac_city As String,
                             ByVal ac_country As String, ByVal ac_state As String, ByVal ac_owners As String, ByVal client_models As String, ByVal jetnet_models As String,
                             ByVal on_exclusive As String, ByVal on_lease As String, ByVal trans_search As String, ByVal trans_jetnet_model As String, ByVal trans_client_model As String,
                             ByVal trans_trans_type As String, ByVal trans_start_date As String, ByVal trans_end_date As String, ByVal year_start As String, ByVal year_end As String,
                             ByVal internal As String, ByVal awaiting As String, ByVal group_by As String, ByVal summary As Boolean, ByVal is_comp_needed As String,
                             ByVal order_by_string As String, ByVal info_to_export As ListBox, ByVal tab As String, ByVal current_history As String, Optional ByVal is_count As Boolean = False) As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim replace_string As String = ""

    Try
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = ""
      ' create the Select Strings

      Dim aSQL_String_JETNET_Select As String = ""
      ' create the Where string
      Dim aSQL_String_JETNET_Where As String = ""
      ' create a data table to hold the company info
      Dim dalTable As New DataTable
      Dim atemptable As New DataTable
      Dim atemptable2 As New DataTable
      Dim dataSet As DataSet = New DataSet("dataSet")
      Dim i As Integer = 0
      dataSet.Tables.Add(atemptable)
      dataSet.Tables.Add(atemptable2)
      dataSet.Tables.Add(dalTable)
      dataSet.EnforceConstraints = False
      Dim splitstring As Array
      Dim transaction_category As Array = Split(trans_trans_type, "|")
      Dim jetnet_transaction_category As String = ""
      Dim lease_start_date_sub As String = ""
      Dim must_be_leased As Boolean = False

      If UBound(transaction_category) = 1 Then
        jetnet_transaction_category = transaction_category(1)
      End If

      If Trim(UCase(tab)) = "HISTORY" Then
        If InStr(jetnet_string, "DATEDIFF(D,AC_LIST_DATE,GETDATE())") > 0 Then
          jetnet_string = Replace(jetnet_string, "DATEDIFF(D,AC_LIST_DATE,GETDATE())", "DATEDIFF(D,AC_LIST_DATE, JOURN_DATE)")
        End If
      End If

      ' HttpContext.Current.Session.Item("MasterAircraftSelect").ToString = ""
      '  HttpContext.Current.Session.Item("MasterAircraftFrom").ToString = ""
      '  HttpContext.Current.Session.Item("MasterAircraftWhere").ToString = ""
      '  HttpContext.Current.Session.Item("MasterAircraftSort").ToString = ""


      If IsNothing(HttpContext.Current.Session.Item("MasterAircraftEventsWhere")) Then
        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") = ""
      End If

      If InStr(jetnet_string, "AC_SER_NO_FULL AS") > 0 Then
        jetnet_string = Replace(jetnet_string, "AC_SER_NO_FULL AS", "';.;' + AC_SER_NO_FULL AS")
      End If

      If Trim(UCase(tab)) = "HISTORY" Then
        jetnet_string = Replace(jetnet_string, "LEASE_START_DATE", "journ_date")
      ElseIf InStr(jetnet_string, "LEASE_START_DATE") > 0 Then
        must_be_leased = True
        lease_start_date_sub &= " (select top 1 journ_date from Journal with (NOLOCK) "
        lease_start_date_sub &= " inner join Journal_Category with (NOLOCK) on jcat_subcategory_code  = journ_subcategory_code "
        lease_start_date_sub &= " where journ_ac_id = ac_id "
        lease_start_date_sub &= " and jcat_subcategory_transtype in ('Lease','Lease Internal', 'Helo Lease', 'Commercial A/C Lease') order by journ_date desc) "

        jetnet_string = Replace(jetnet_string, "LEASE_START_DATE", lease_start_date_sub)
      End If

      If InStr(jetnet_string, "EXTERNAL_LAV ") > 0 Then
        jetnet_string = Replace(jetnet_string, "EXTERNAL_LAV ", "(case when  (select top 1 adet_data_description FROM Aircraft_Details with (NOLOCK) WHERE (((Aircraft_Details.adet_data_description LIKE '%external%service%') AND (Aircraft_Details.adet_data_name = 'Lavatory')) or  (Aircraft_Details.adet_data_description LIKE '%external lav service%') ) AND (Aircraft_Details.adet_journ_id = 0) and (Aircraft_Details.adet_ac_id = VIEW_AIRCRAFT_FLAT.ac_id)) is not null then 'Yes' else 'No' end)")
      End If

      If InStr(jetnet_string, "AC_LIST_DATE ") > 0 Then
        jetnet_string = Replace(jetnet_string, "AC_LIST_DATE ", "  ('  ' + cast(CAST(AC_LIST_DATE AS DATE) as varchar(20))) ")
      End If

      If jetnet_string.ToUpper.Contains("AMOD_WEIGHT_CLASS AS") Then

        jetnet_string = Replace(jetnet_string, "AMOD_WEIGHT_CLASS AS ""WEIGHTCLASS""", "(SELECT DISTINCT acwgtcls_name FROM Aircraft_Weight_Class WITH(NOLOCK) WHERE amod_type_code = acwgtcls_maketype AND amod_weight_class = acwgtcls_code AND amod_airframe_type_code = acwgtcls_airframe_type_code) AS ""WEIGHTCLASS""")

      End If
      ' aSQL_String_JETNET_Select = HttpContext.Current.Session.Item("MasterAircraftSelect").ToString()

      If Trim(UCase(tab)) = "YACHT" Then
                If is_count Then
                    ' if only counting, replace the subselect for manufacturer
                    jetnet_string = Replace(UCase(jetnet_string), UCase("(select comp_name from Company where comp_id = ym_mfr_comp_id and comp_journ_id = 0)"), "")

                    If InStr(UCase(jetnet_string), "COMP") > 0 Then
                        aSQL_String_JETNET_Select = "SELECT  count(distinct yr_id) as tcount "
                    Else
                        aSQL_String_JETNET_Select = "SELECT  count(distinct yt_id) as tcount "
                    End If
                Else
                    aSQL_String_JETNET_Select = "SELECT DISTINCT "

          aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & jetnet_string
        End If
      Else
        If is_count Then
          aSQL_String_JETNET_Select = "SELECT "
                    'If Trim(UCase(tab)) = "HISTORY" And (InStr(UCase(jetnet_string), "COMP_") > 0 Or InStr(UCase(jetnet_string), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CONTACT_") > 0 Or InStr(UCase(jetnet_string), "CBUS_") > 0 Or InStr(UCase(jetnet_string), "ACTYPE_NAME") > 0) And (Trim(UCase(tab)) = "HISTORY" Or Trim(UCase(tab)) = "AIRCRAFT") Then
                    '          aSQL_String_JETNET_Select += " count(distinct ac_journ_id) as tcount "
                    ' Else
                    aSQL_String_JETNET_Select += " count(*) as tcount "
                        ' End If
                        Else
          aSQL_String_JETNET_Select = "SELECT DISTINCT "

          aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & jetnet_string
        End If
      End If


            ' If Trim(is_comp_needed) = "Y" Then
            'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_Flat "
            ' Else
            '     aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Flat "
            ' End If
            'View_Aircraft_History_Flat




            '   ' client 
            '   HttpContext.Current.Session.Item("CLIENT_AC_LIST") = HttpContext.Current.Session.Item("CLIENT_AC_LIST")
            '   HttpContext.Current.Session.Item("Company_Master") = HttpContext.Current.Session.Item("Company_Master")



            'aircraft_company_history_flat
            If Trim(UCase(tab)) = "YACHT" Or Trim(UCase(tab)) = "YACHTHISTORY" Then


                If InStr(aSQL_String_JETNET_Select, "YT_HULL_MFR_NBR ") > 0 Then
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "YT_HULL_MFR_NBR ", "'&nbsp;' + YT_HULL_MFR_NBR + ''")
                End If


                If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtFrom")) Then
                    If Trim(HttpContext.Current.Session.Item("MasterYachtFrom").ToString()) <> "" Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Trim(HttpContext.Current.Session.Item("MasterYachtFrom").ToString())
                    Else
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from Yacht WITH(NOLOCK) "
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join Yacht_Model WITH(NOLOCK) on ym_model_id = yt_model_id "
                    End If

                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterYachtWhere").ToString()
                End If

                'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join yacht_category_size on  ycs_motor_type = ym_motor_type and ycs_category_size = ym_category_size "
                'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join yacht_motor_type on  ymt_motor_type = ym_motor_type  "
                'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join yacht_classification_society_types on ycst_code = yt_class_id  "



            ElseIf Trim(UCase(tab)) = "YACHTEVENTS" Then

                If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtEventsFrom")) Then
                    If Trim(HttpContext.Current.Session.Item("MasterYachtEventsFrom").ToString()) <> "" Then
                        If Trim(HttpContext.Current.Session.Item("MasterYachtEventsFrom").ToString()) <> "" Then
                            aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Trim(HttpContext.Current.Session.Item("MasterYachtEventsFrom").ToString())
                            aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterYachtEventsWhere").ToString()
                        End If
                    End If
                End If



                'else if there is a comp_ cref_ or contact_ in the select list then there must be View_Aircraft_Company_Flat
            ElseIf (InStr(UCase(jetnet_string), "COMP_") > 0 Or InStr(UCase(jetnet_string), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CONTACT_") > 0 Or InStr(UCase(jetnet_string), "CBUS_") > 0 Or InStr(UCase(jetnet_string), "ACTYPE_NAME") > 0) And (Trim(UCase(tab)) = "HISTORY" Or Trim(UCase(tab)) = "AIRCRAFT") Then



                If Trim(UCase(tab)) = "HISTORY" Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_History_Flat WITH(NOLOCK) "

                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_FLAT", "View_Aircraft_Company_History_Flat")
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_COMPANY_FLAT", "View_Aircraft_Company_History_Flat")
                    ' this is to make sure the subselects like last fractional purchase date work correctly for historical 

                    ' check to see if there is a this in the where clause, if so, its using out used search
                    If InStr(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString, "View_Aircraft_History_Flat") > 0 Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Replace(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString(), "View_Aircraft_History_Flat", "View_Aircraft_Company_History_Flat")
                    Else
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()
                    End If


                ElseIf Trim(UCase(tab)) = "AIRCRAFT" Then


                    replace_string = ""

                    If InStr(UCase(aSQL_String_JETNET_Select), "(SELECT TOP 1 ADOC_DOC_DATE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCDATE""") Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, ",(SELECT TOP 1 ADOC_DOC_DATE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCDATE""", "")
                        If Trim(replace_string) <> "" Then
                            replace_string &= ", "
                        End If
                        replace_string &= "(SELECT TOP 1 ADOC_DOC_DATE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCDATE"""
                    End If

                    If InStr(UCase(aSQL_String_JETNET_Select), "(SELECT TOP 1 ADOC_DOC_TYPE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCTYPE""") Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, ",(SELECT TOP 1 ADOC_DOC_TYPE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCTYPE""", "")
                        If Trim(replace_string) <> "" Then
                            replace_string &= ", "
                        End If
                        replace_string &= "(SELECT TOP 1 ADOC_DOC_TYPE FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC) AS ""DOCTYPE"""
                    End If

                    If InStr(UCase(aSQL_String_JETNET_Select), "(SELECT TOP 1 ADOC_DOC_AMOUNT FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCAMOUNT""") Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, ",(SELECT TOP 1 ADOC_DOC_AMOUNT FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCAMOUNT""", "")
                        If Trim(replace_string) <> "" Then
                            replace_string &= ", "
                        End If
                        replace_string &= "(SELECT TOP 1 ADOC_DOC_AMOUNT FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCAMOUNT"""
                    End If

                    If InStr(UCase(aSQL_String_JETNET_Select), "(SELECT TOP 1 COMP_NAME FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCINFAVOR""") Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, ",(SELECT TOP 1 COMP_NAME FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCINFAVOR""", "")
                        If Trim(replace_string) <> "" Then
                            replace_string &= ", "
                        End If
                        replace_string &= "(SELECT TOP 1 COMP_NAME FROM COMPANY WITH (NOLOCK)INNER JOIN AIRCRAFT_DOCUMENT WITH (NOLOCK) ON ADOC_INFAVOR_COMP_ID = COMP_ID AND ADOC_JOURN_ID = COMP_JOURN_ID WHERE (ADOC_AC_ID = AC_ID) AND (ADOC_INFAVOR_COMP_ID IS NOT NULL) AND (ADOC_DOC_DATE >= AC_PURCHASE_DATE) AND (ADOC_ONBEHALF_COMP_ID IN (SELECT CREF_COMP_ID FROM AIRCRAFT_REFERENCE WITH (NOLOCK)WHERE (CREF_AC_ID = AC_ID) AND (CREF_JOURN_ID = AC_JOURN_ID) AND (CREF_TRANSMIT_SEQ_NO = 1) )) ORDER BY ADOC_DOC_DATE DESC)  AS ""DOCINFAVOR"""
                    End If




                    ' if if is still in there 
                    If (InStr(UCase(aSQL_String_JETNET_Select), "COMP_") > 0 Or InStr(UCase(aSQL_String_JETNET_Select), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CBUS_") > 0 Or InStr(UCase(aSQL_String_JETNET_Select), "CONTACT_") > 0 Or InStr(UCase(aSQL_String_JETNET_Select), "ACTYPE_NAME") > 0) And (Trim(UCase(tab)) = "HISTORY" Or Trim(UCase(tab)) = "AIRCRAFT") Then
                        If Trim(replace_string) <> "" And Trim(aSQL_String_JETNET_Select) <> "SELECT DISTINCT" Then
                            aSQL_String_JETNET_Select &= ", " & replace_string
                        ElseIf Trim(replace_string) <> "" Then
                            aSQL_String_JETNET_Select &= " " & replace_string
                        End If
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_FLAT", "View_Aircraft_Company_Flat")
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_Flat WITH(NOLOCK) "
                        ' replace the fields in the sub select that reference the fields in the select. I did the dot in case is a sub select that uses one of those tables
                        ' 5/14/20 - did the ucase - to make sure it got them all 
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Replace(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "VIEW_AIRCRAFT_FLAT.", "VIEW_AIRCRAFT_COMPANY_FLAT.")

                    Else
                        If Trim(replace_string) <> "" And Trim(aSQL_String_JETNET_Select) <> "SELECT DISTINCT" Then
                            aSQL_String_JETNET_Select &= ", " & replace_string
                        ElseIf Trim(replace_string) <> "" Then
                            aSQL_String_JETNET_Select &= " " & replace_string
                        End If
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Flat WITH(NOLOCK) "
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()
                    End If


                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                        If (InStr(UCase(aSQL_String_JETNET_Select), "COMP_") > 0 Or InStr(UCase(aSQL_String_JETNET_Select), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CBUS_") > 0 Or InStr(UCase(aSQL_String_JETNET_Select), "CONTACT_") > 0) Then
                            If InStr(aSQL_String_JETNET_Select, "View_Aircraft_Company_Flat") > 0 Then
                                aSQL_String_JETNET_Select &= " and cref_contact_type not in ('99','98','38','IV') "
                            End If
                        End If
                    End If

                    ' commented out msw - 6/22/17
                    '  If must_be_leased = True Then
                    '    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " and ac_lease_flag = 'Y' "
                    '  End If
                End If


            ElseIf Trim(UCase(tab)) = "HISTORY" Then

                If InStr(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString(), "comp_") > 0 Or InStr(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString(), "cref_") > 0 Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_History_Flat WITH(NOLOCK) "
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_FLAT", "View_Aircraft_Company_History_Flat")
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()
                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_History_Flat WITH(NOLOCK) "
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_FLAT", "View_Aircraft_History_Flat")
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()
                End If


            ElseIf Trim(UCase(tab)) = "AIRCRAFT" Then

                If (InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "COMP_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "CREF_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "CONTACT_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "STATE_NAME") > 0) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_Flat WITH(NOLOCK) "
                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Flat WITH(NOLOCK) "
                End If

                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()

                'exclusive broker, dealer broker, sellers broker, sales company contact, 
                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                    If (InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "COMP_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "CREF_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "CONTACT_") > 0 Or InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString()), "STATE_NAME") > 0) Then
                        aSQL_String_JETNET_Select &= " and cref_contact_type not in ('99','98','38','IV') "
                    End If
                End If





            ElseIf Trim(UCase(tab)) = "YACHT COMPANY EXPORT" Then

                If Not IsNothing(HttpContext.Current.Session.Item("Yacht_Crossover_Select")) Then
                    If Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Select")) <> "" Then
                        aSQL_String_JETNET_Select = Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Select"))
                    End If
                End If

            ElseIf Trim(UCase(tab)) = "YACHT COMPANY MODEL EXPORT" Then

                If Not IsNothing(HttpContext.Current.Session.Item("Yacht_Crossover_Model_Select")) Then
                    If Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Model_Select")) <> "" Then
                        aSQL_String_JETNET_Select = Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Model_Select"))
                    End If
                End If
            ElseIf Trim(UCase(tab)) = "YACHT COMPANY YACHT EXPORT" Then

                If Not IsNothing(HttpContext.Current.Session.Item("Yacht_Crossover_Yacht_Select")) Then
                    If Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Yacht_Select")) <> "" Then
                        aSQL_String_JETNET_Select = Trim(HttpContext.Current.Session.Item("Yacht_Crossover_Yacht_Select"))
                    End If
                End If

            ElseIf Trim(UCase(tab)) = "YACHT COMPANY NO YACHT EXPORT" Then

                If Not IsNothing(HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select")) Then
                    If Trim(HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select")) <> "" Then
                        aSQL_String_JETNET_Select = Trim(HttpContext.Current.Session.Item("Yacht_Crossover_AC_Select"))
                    End If
                End If

            ElseIf Trim(UCase(tab)) = "YACHT COMPANY" Then


                If Not IsNothing(HttpContext.Current.Session.Item("MasterCompanyFrom")) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterCompanyFrom").ToString
                    If InStr(UCase(Left(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString, 10)), "WHERE") = 0 Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                    End If
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterCompanyWhere").ToString()

                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_Flat WITH(NOLOCK) "
                    If InStr(UCase(Left(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString, 10)), "WHERE") = 0 Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                    End If
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Replace(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString(), "comp_journ_id", "cref_journ_id")
                End If


                If Not IsNothing(HttpContext.Current.Session.Item("MasterContactWhere")) Then
                    If Trim(HttpContext.Current.Session.Item("MasterContactWhere")) <> "" Then
                        aSQL_String_JETNET_Select &= " and contact_id in (" & HttpContext.Current.Session.Item("MasterContactWhere") & ") "
                    End If
                End If


                If InStr(aSQL_String_JETNET_Select, "(SELECT TOP 1 YCT_NAME FROM YACHT_REFERENCE WITH (NOLOCK) INNER JOIN YACHT_CONTACT_TYPE WITH (NOLOCK) ON YCT_CODE = YR_CONTACT_TYPE WHERE YR_COMP_ID = COMPANY.COMP_ID ORDER BY YCT_SEQ_NO ASC)") > 0 Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " and (SELECT TOP 1 YCT_NAME FROM YACHT_REFERENCE WITH (NOLOCK) INNER JOIN YACHT_CONTACT_TYPE WITH (NOLOCK) ON YCT_CODE = YR_CONTACT_TYPE WHERE YR_COMP_ID = COMPANY.COMP_ID ORDER BY YCT_SEQ_NO ASC) <> 'Research Only' "
                End If

                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " and contact_research_flag = 'N' "


            ElseIf Trim(UCase(tab)) = "COMPANY" Then

                If Not IsNothing(HttpContext.Current.Session.Item("MasterCompanyFrom")) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterCompanyFrom").ToString

                    If InStr(UCase(Left(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString, 10)), "WHERE") = 0 Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                    End If
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterCompanyWhere").ToString()

                    If InStr(Trim(aSQL_String_JETNET_Select), "CONTACT_COMP_ID = PNUM_COMP_ID") > 0 Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "CONTACT_COMP_ID = PNUM_COMP_ID", "COMP_ID = PNUM_COMP_ID")
                    End If

                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " from View_Aircraft_Company_Flat WITH(NOLOCK) "
                    If InStr(UCase(Left(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString, 10)), "WHERE") = 0 Then
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                    End If
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & Replace(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString(), "comp_journ_id", "cref_journ_id")
                End If


                If Not IsNothing(HttpContext.Current.Session.Item("MasterContactWhere")) Then
                    If Trim(HttpContext.Current.Session.Item("MasterContactWhere")) <> "" Then
                        aSQL_String_JETNET_Select &= " and contact_id in (" & HttpContext.Current.Session.Item("MasterContactWhere") & ") "
                    End If
                End If



            ElseIf Trim(UCase(tab)) = "EVENTS" Then
                'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " FROM Priority_Events WITH(NOLOCK) "
                'aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join Priority_Events_Category WITH(NOLOCK) on priorev_category_code=priorevcat_category_code"
                'If (InStr(UCase(jetnet_string), "COMP_") > 0 Or InStr(UCase(jetnet_string), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CONTACT_") > 0) Then
                '    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " LEFT OUTER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) "
                'Else
                '    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) "
                'End If

                If (InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString()), "LEFT OUTER JOIN COMPANY") > 0) Or (InStr(LCase(HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString()), "view_aircraft_company_flat") > 0) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString()
                Else



                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " FROM Priority_Events WITH(NOLOCK) "
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " inner join Priority_Events_Category WITH(NOLOCK) on priorev_category_code=priorevcat_category_code"
                    If (InStr(UCase(jetnet_string), "COMP_") > 0 Or InStr(UCase(jetnet_string), "CREF_") > 0 Or InStr(UCase(jetnet_string), "CONTACT_") > 0) Then
                        aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, "VIEW_AIRCRAFT_FLAT.", "View_Aircraft_Company_Flat.")
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " LEFT OUTER JOIN View_Aircraft_Company_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) "
                    Else
                        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) "
                    End If
                End If





                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftEventsWhere").ToString()



            ElseIf Trim(UCase(tab)) = "WANTED" Then

                ' added in MSW = 5/14/20 so that columns r not ambigious 
                If InStr(UCase(jetnet_string), " COMP_") > 0 Then
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, " COMP_", " view_aircraft_model_wanted.COMP_")
                End If

                If InStr(UCase(jetnet_string), ",COMP_") > 0 Then
                    aSQL_String_JETNET_Select = Replace(aSQL_String_JETNET_Select, ",COMP_", ",view_aircraft_model_wanted.COMP_")
                End If

                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWantedFrom").ToString()
                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWantedWhere").ToString()


            ElseIf Trim(UCase(tab)) = "PERFORMANCE SPECS" Then
                If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftPerformanceSpecsFrom")) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftPerformanceSpecsFrom").ToString()
                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " FROM Aircraft_Model WITH(NOLOCK) "
                End If
                If InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftPerformanceSpecsWhere").ToString), "WHERE") = 0 Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                End If
                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftPerformanceSpecsWhere").ToString()
            ElseIf Trim(UCase(tab)) = "OPERATING COST" Then
                If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftOperatingCostFrom")) Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftOperatingCostFrom").ToString()
                Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " FROM Aircraft_Model WITH(NOLOCK) "
                End If
                If InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftOperatingCostWhere").ToString), "WHERE") = 0 Then
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
                End If
                aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftOperatingCostWhere").ToString()

                If operating_radio.Visible = True Then
                    If Trim(operating_radio.SelectedValue) <> "" Then
                        If Trim(operating_radio.SelectedValue) = "0" Then
                            ' get rid of the duplicates, show nautical miles 
                            '  ConvertStatuteMileToNauticalMile
                            '  nmMile = CDbl(stMile) * 0.86898

                            ' ConvertNauticalMileToStatuteMile
                            ' stMile = CDbl(nmMile) * 1.1515


                            'avgBlockSpeed    =  * 86898 statuate to nautical 
                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_avg_block_speed", " ROUND((amod_avg_block_speed * 0.86898), 2)  ")    ' BLOCKSPD	    ' as amod_avg_block_speed 

                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_tot_stat_mile_cost", "  ROUND(((amod_tot_direct_cost / amod_annual_hours) / (amod_avg_block_speed * 0.86898)), 2)  ")   ' TOTCOSTSTMILE    ' as amod_tot_stat_mile_cost

                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_annual_miles", " ROUND((amod_annual_miles * 0.86898), 2)  ")  ' ANNMILES    ' as amod_annual_miles


                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_tot_df_statmile_cost", " ROUND((amod_tot_df_annual_cost / (amod_annual_miles * 0.86898) ), 2) ") ' TOTSTATMILECOST   ' as  amod_tot_df_annual_cost 
                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_tot_df_seat_cost", " ROUND(((amod_tot_df_annual_cost / (amod_annual_miles * 0.86898)) / amod_number_of_passengers ), 2) ")   ' TOTSEATCOST  '  as  amod_tot_df_seat_cost


                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_tot_nd_statmile_cost", " ROUND((amod_tot_nd_annual_cost / (amod_annual_miles * 0.86898) ), 2)  ") ' TOTSMILENODEP     ' as  amod_tot_nd_statmile_cost
                            aSQL_String_JETNET_Select = Replace(LCase(aSQL_String_JETNET_Select), "amod_tot_nd_seat_cost", " ROUND(((amod_tot_nd_annual_cost / (amod_annual_miles * 0.86898)) / amod_number_of_passengers ), 2) ")   'TOTSEATNODEP    '  as  amod_tot_nd_seat_cost

                        ElseIf Trim(operating_radio.SelectedValue) = "1" Then
                            ' get rid of the duplicates, show statuate miles
                        End If
                    End If
                End If


            Else
                    aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftFrom").ToString()
        If InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString), "WHERE") = 0 Then
          aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " WHERE "
        End If
        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftWhere").ToString() '
      End If


      ' if the where clause has View_Aircraft_Company_Flat in it then we must use View_Aircraft_Company_Flat 




      If is_count Then
        ' if its count then dont do any order by 

      ElseIf InStr(jetnet_string, "count") > 0 Then
        order_by_string = ""
        'order_by_string = Left(Trim(order_by_string), Len(Trim(order_by_string)) - 1)
        For i = 0 To info_to_export.Items.Count - 1

          splitstring = Split(info_to_export.Items(i).Value, "|")

          order_by_string = order_by_string & splitstring(1) & ", "
        Next

        If Trim(order_by_string) <> "" Then
          order_by_string = Left(Trim(order_by_string), Len(Trim(order_by_string)) - 1)
        End If

        'TBD NEED TO ADD BACK IN FOR SUMMARY
        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " group by " & order_by_string
        aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " order by " & order_by_string
        ' aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & HttpContext.Current.Session.Item("MasterAircraftSort").ToString()
      Else
        If Trim(UCase(tab)) = "YACHT" Then

        ElseIf Trim(order_by_string) <> "" Then
          aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " order by " & order_by_string
        ElseIf Trim(UCase(tab)) = "YACHT COMPANY EXPORT" Then
        ElseIf Trim(UCase(tab)) = "YACHT COMPANY MODEL EXPORT" Then
        ElseIf Trim(UCase(tab)) = "YACHT COMPANY YACHT EXPORT" Then
        ElseIf Trim(UCase(tab)) = "YACHT COMPANY NO YACHT EXPORT" Then
        Else
          If Trim(UCase(tab)) = "EVENTS" Then
            aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & "  "
          ElseIf Trim(is_comp_needed) = "Y" Then
            aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " order by AMOD_MAKE_NAME, AMOD_MODEL_NAME, AC_SER_NO_FULL "
          Else
            aSQL_String_JETNET_Select = aSQL_String_JETNET_Select & " order by AMOD_MAKE_NAME, AMOD_MODEL_NAME, AC_SER_NO_FULL "
          End If
        End If

      End If



      '  HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Export_All() as DataTable - Jetnet Side</b><br />" & sQuery
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Export_All() as DataTable - Jetnet Side</b><br />" & aSQL_String_JETNET_Select

      ' changed MSW - 4/16/19
      '  If originating_type <> 1 Then
      ' Dim test As String = ""
      'SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase")

      SqlConn.Open()
      SqlCommand.Connection = SqlConn



      ' SqlCommand.CommandText = sQuery  ' commented out msw 3/8/2013
      SqlCommand.CommandText = aSQL_String_JETNET_Select
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


      dataSet.EnforceConstraints = False
      atemptable.PrimaryKey = Nothing
      atemptable.Constraints.Clear()

      ' if you are counting then just count, otherwise, load in 


      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Export_All() as DataTable - Datatable Loaded</b><br />"


      dataSet.EnforceConstraints = False
      SqlReader.Close()
      SqlReader = Nothing


      If Trim(UCase(tab)) = "YACHT COMPANY EXPORT" Then
        Export_Evo = atemptable
      ElseIf Trim(UCase(tab)) = "YACHT COMPANY MODEL EXPORT" Then
        Export_Evo = atemptable
      ElseIf Trim(UCase(tab)) = "YACHT COMPANY YACHT EXPORT" Then
        Export_Evo = atemptable
      ElseIf Trim(UCase(tab)) = "YACHT COMPANY NO YACHT EXPORT" Then
        Export_Evo = atemptable
      ElseIf is_count Or Trim(UCase(tab)) = "YACHT" Then
        Export_Evo = atemptable
      Else
        dalTable = atemptable.Clone
        dataSet.EnforceConstraints = False
        dalTable.PrimaryKey = Nothing
        dalTable.Constraints.Clear()

        '  If originating_type <> 1 Then
        Dim afiltered_BOTH As DataRow() = atemptable.Select("", first_column & " asc")
        ' extract and import
        For Each atmpDataRow_JETNET In afiltered_BOTH
          dalTable.ImportRow(atmpDataRow_JETNET)
        Next

        If dalTable.Columns.Count = 1 Then
          Export_Evo = CheckDataTableColumn(dalTable)
        Else
          Export_Evo = dalTable
        End If
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Export_All() as DataTable - Exiting Function/b><br />"

      'Else
      ''If originating_type = 8 Then
      'Export_Evo = New DataTable
      ''End If
      'End If
    Catch ex As Exception
      Export_Evo = Nothing
      Call commonLogFunctions.Log_User_Event_Data("UserError", "User Error During Export:" & Trim(ex.Message), Nothing)
    Finally
      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try
  End Function

  Public Function find_rename_field_for_export(ByVal rename_field_string As String) As String

    find_rename_field_for_export = ""

    Dim jetnet_query As String = ""
    Dim strDate As System.DateTime = Now()
    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim return_id As Integer = 0
    Try
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      jetnet_query = ""
      jetnet_query = "select cef_evo_field_name from Custom_Export_Fields where cef_client_field_name = '" & rename_field_string & "' "
      SqlCommand.CommandText = jetnet_query
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If temptable.Rows.Count > 0 Then
        find_rename_field_for_export = temptable.Rows(0).Item("cef_evo_field_name")
      End If

      SqlReader.Close()
      SqlReader = Nothing

      Return find_rename_field_for_export
    Catch ex As Exception
      find_rename_field_for_export = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in find_rename_field_for_export(ByVal rename_field_string As String) As String " & ex.Message
    Finally
      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Private Function CheckDataTableColumn(ByVal dt As DataTable) As DataTable
    Dim flag As Boolean = False
    Dim counter As Integer = 0
[EXIT]:
    Try
      For i As Integer = counter To dt.Columns.Count - 1
        For x As Integer = 0 To dt.Rows.Count - 1
          If String.IsNullOrEmpty(dt.Rows(x)(i).ToString()) Then
            'means there is an empty value
            flag = True
          Else
            'means if it found non null or empty in rows of a particular column
            flag = False
            counter = i + 1
            GoTo [EXIT]
          End If
        Next

        If flag = True Then
          dt.Columns.Remove(dt.Columns(i))
          i -= 1
        End If
      Next
    Catch
      Return dt
    End Try
    Return dt
  End Function

End Class