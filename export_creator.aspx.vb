
Partial Public Class export_creator
  Inherits System.Web.UI.Page
  Dim aclsData_Temp As New clsData_Manager_SQL
  Private localDatalayer As viewsDataLayer
  Dim aTempTable, aTempTable2, final_table As New DataTable
  Dim error_string As String = ""
  Dim subset_string As String = ""
  Dim column As New DataColumn 'Column to Add Source to jetnet data.
  Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
  Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
  Dim column4 As New DataColumn
  Dim column5 As New DataColumn
  Dim column6 As New DataColumn
  Dim column7 As New DataColumn
  Dim column8 As New DataColumn
  Dim column9 As New DataColumn
  Dim column10 As New DataColumn

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim listing As Integer = Session.Item("Listing")
    Dim table As DataTable = CType(Session("Results"), DataTable)
    Dim runExport As Boolean = True
    Dim jetnet_model_string As String = ""

    If IsNothing(Session("EXPORT_CHECKS")) Then
      Session("EXPORT_CHECKS") = ""
    End If

    If Trim(Request("project_id")) <> "" Then
      listing = 3
    End If

    If Session.Item("crmUserLogon") <> True Then
      'error_string = "export_creator.aspx.vb - Page Load() - " & Request.ServerVariables("SCRIPT_NAME").ToString() & " - Session Timeout"
      'LogError(error_string)
      Response.Redirect("Default.aspx", False)
    End If


    aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
    aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

    localDatalayer = New viewsDataLayer
    localDatalayer.adminConnectStr = Application.Item("crmClientSiteData").AdminDatabaseConn
    HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = Application.Item("crmClientDatabase")

    Session("export_info") = ""

    'Response.Write("Cliuser Max Records: " & Session.Item("localUser").crmMaxClientExport & "<br />")
    'Response.Write("Cliuser Type: " & Session.Item("localUser").crmUserType & "<br />")
    'Response.Write("Cliuser rows exported: " & table.Rows.Count & "<br />")
    'Response.Write("Export Type: " & Server.UrlDecode(Trim(Request("su"))) & "<br />")


    Me.export_now.Visible = True
    ' commented out MSW -4/23/19 - this block was trying to protect against mass exports we believe, but we are shutting it off to allow ac and othher exports that it 
    ' was accidently protecting from 
    'If Trim(Request("su")) = "J" Or Trim(Request("su")) = "B" Then  ' if its both client and jetnet or just jetnet 
    '  If Trim(Request("fn")) = "%" Or Trim(Request("fn")) = "" Then       ' first name blank or using %
    '    If Trim(Request("ln")) = "%" Or Trim(Request("ln")) = "" Then   ' last name blank or using %
    '      If Trim(Request("cn")) = "%" Or Trim(Request("cn")) = "" Then   ' company name blank or using %
    '        If Trim(Request("cem")) = "%" Or Trim(Request("cem")) = "" Then   ' company email address blank or using %
    '          If Trim(Request("cphn")) = "" Then 'contact phone
    '            Me.export_now.Visible = False
    '          End If
    '        End If
    '      End If
    '    End If
    '  End If
    'End If








    runExport = AllowExport(table)
    'This is a function that runs on load and when a project is deleted.
    'used to tell whether the load export needs to be there.
    If runExport = True Then
      Show_Load_Export()

      If Not Page.IsPostBack Then 'Only do this once.

        Select Case listing
          Case 14
            If Not Page.IsPostBack Then
              company_new.Visible = True
              form1.Visible = True
              export_title.Text = "Performance/Operating Export"
              type_of_info.Items.Add(New ListItem("Performance Specs", "Performance Specs"))
              type_of_info.Items.Add(New ListItem("Operating Costs", "Operating"))
              type_of_info.SelectedValue = "Performance Specs"
              add()
            End If
          Case 6
            If Not Page.IsPostBack Then
              company_new.Visible = True
              form1.Visible = True
              export_title.Text = "Note Export"
              type_of_info.Items.Add(New ListItem("Note Information", "Note"))
              type_of_info.Items.Add(New ListItem("Company Information", "Company"))
              type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
              type_of_info.Items.Add(New ListItem("Aircraft Information", "Aircraft"))
              type_of_info.SelectedValue = "Note"
              add()
            End If
          Case 16
            If Not Page.IsPostBack Then
              company_new.Visible = True
              form1.Visible = True
              export_title.Text = "Note Export"
              type_of_info.Items.Add(New ListItem("Prospect Information", "Prospect"))
              type_of_info.Items.Add(New ListItem("Company Information", "Company"))
              type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
              type_of_info.Items.Add(New ListItem("Aircraft Information", "Aircraft"))
              type_of_info.SelectedValue = "Prospect"
              add()
            End If
          Case 4
            If Not Page.IsPostBack Then
              company_new.Visible = True
              form1.Visible = True
              export_title.Text = "Action Item Export"
              type_of_info.Items.Add(New ListItem("Action Item Information", "Action"))
              type_of_info.Items.Add(New ListItem("Company Information", "Company"))
              type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
              type_of_info.Items.Add(New ListItem("Aircraft Information", "Aircraft"))
              type_of_info.SelectedValue = "Action"
              add()
            End If
          Case 11
            If Not Page.IsPostBack Then
              company_new.Visible = True
              form1.Visible = True
              export_title.Text = "Opportunity Export"
              type_of_info.Items.Add(New ListItem("Opportunity Information", "Opportunity"))
              type_of_info.Items.Add(New ListItem("Company Information", "Company"))
              type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
              'type_of_info.Items.Add(New ListItem("Aircraft Information", "Aircraft"))
              type_of_info.SelectedValue = "Opportunity"
              add()
            End If
          Case 8
            If Not Page.IsPostBack Then
              Try
                company_new.Visible = True
                form1.Visible = True
                export_title.Text = "Transaction Export"

                type_of_info.Items.Add(New ListItem("Transaction Information", "Transaction"))
                type_of_info.Items.Add(New ListItem("Company Information", "Transaction Company"))
                type_of_info.Items.Add(New ListItem("Contact Information", "Transaction Contact"))
                type_of_info.Items.Add(New ListItem("Aircraft Information", "Transaction Aircraft"))
                type_of_info.SelectedValue = "Transaction"
                add_default_transaction_columns()
              Catch ex As Exception
                error_string = "export_creator.aspx.vb - Trans Display - " & ex.Message
                LogError(error_string)
              End Try
            End If

          Case 1
            Try

              If Not Page.IsPostBack Then
                company_new.Visible = True
                form1.Visible = True
                export_title.Text = "Company List Export"
                type_of_info.Items.Add(New ListItem("Company Information", "Company"))
                type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
                type_of_info.SelectedValue = "Company"
                add()
              End If

            Catch ex As Exception
              error_string = "export_creator.aspx.vb - Company Display - " & ex.Message
              LogError(error_string)
            End Try
          Case 2
            Try
              If Not Page.IsPostBack Then
                company_new.Visible = True
                form1.Visible = True
                export_title.Text = "Contact List Export"
                type_of_info.Items.Add(New ListItem("Company Information", "Company"))
                type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
                type_of_info.SelectedValue = "Contact"
                add()
              End If
            Catch ex As Exception
              error_string = "export_creator.aspx.vb - Contact Display - " & ex.Message
              LogError(error_string)
            End Try
          Case 3
            Try
              If Not Page.IsPostBack Then


                company_new.Visible = True
                form1.Visible = True
                export_title.Text = "Aircraft List Export"
                type_of_info.Items.Add(New ListItem("Company Information", "Company"))
                type_of_info.Items.Add(New ListItem("Contact Information", "Contact"))
                type_of_info.Items.Add(New ListItem("Aircraft Information", "Aircraft"))
                type_of_info.Items.Add(New ListItem("Aircraft Feature Codes", "Feature Code"))
                type_of_info.SelectedValue = "Aircraft"
                add()
                Me.merge_lists.Visible = True
              End If
            Catch ex As Exception
              error_string = "export_creator.aspx.vb - Contact Display - " & ex.Message
              LogError(error_string)
            End Try
        End Select


        Me.export_type_drop.Visible = True
        Call add_in_type_dropdown()
        Session("Last_Export_Type") = "All"

      ElseIf IsPostBack = True Then

        ' if the type drop down has changed since it was changed last, otherwise, its a different click
        If Trim(Session("Last_Export_Type")) <> Trim(Me.export_type_drop.Text) Then
          If Trim(Session("Last_Export_Type")) = "" And Trim(Me.export_type_drop.Text) = "All" Then
            Call add_in_type_dropdown()
            '  Call add()
          Else
            Call add() ' ADD IS THE FUNCTION THAT WILL CHANGE THE LISTBOX SELECTION OPTIONS 
            Session("Last_Export_Type") = Trim(Me.export_type_drop.Text)
          End If
        Else
          Call add_in_type_dropdown()
          ' Call add()
        End If

      End If
    ElseIf runExport = False Then
      no_export_error.Visible = True

      If Session.Item("localUser").crmAllowExport_Flag = False Then
        no_export_error.Text = "<p align='center'>Demo Accounts are not allowed to perform data exports. Contact your JETNET marketing representative for more information</p>"
      Else
        no_export_error.Text = "<p align='center'>Exceeded maximum records allowed for export.  Contact your CRM/Administrator to complete larger exports.</p>"
      End If

    End If


    If Not Page.IsPostBack Then
      If Trim(Request("project_id")) <> "" And Trim(Request("project_id")) <> "0" Then
        Call LOAD_Export_function()
        Call open_project_function()
        Me.export_now.Visible = False
        Me.create_export_template.Visible = False
      ElseIf Trim(Request("new_project")) <> "" Then
        Call LOAD_Export_function()
        Call open_project_function()
        Call create_click()
        Me.file_save.Visible = False
        Me.export_now.Visible = False
        Me.create_export_template.Visible = False
      End If
    End If
  End Sub
  Public Sub add_in_type_dropdown()

    Dim type_table As New DataTable
    Dim export_checks As String = ""

    Try
      type_table = localDatalayer.Create_Distinct_Export_Type(type_of_info, export_checks)


      If (Trim(export_checks) <> Trim(Session("EXPORT_CHECKS"))) Or Me.export_type_drop.Items.Count = 0 Then ' also re-load if its blank
        Session("EXPORT_CHECKS") = export_checks
        Session("Last_Export_Type") = "" '  reset since we are picking a new checkbox

        Me.export_type_drop.Items.Clear()
        Me.export_type_drop.Items.Add(New ListItem("All", "All"))
        If Not IsNothing(type_table) Then
          If type_table.Rows.Count > 0 Then
            'choice_to_export.Items.Clear()
            For Each r As DataRow In type_table.Rows
              Me.export_type_drop.Items.Add(New ListItem(r("cliexp_sub_group"), r("cliexp_sub_group")))
            Next
          End If
        End If
      End If


    Catch ex As Exception

    End Try
  End Sub
  Public Function AllowExport(ByVal table As DataTable) As Boolean
    If (Server.UrlDecode(Trim(Request("su"))) = "C") Then
      subset_string = "CLIENT"
      'i.	if the customer has selected CLIENT only data sets 
      'ii.	and the user is NOT user type admin
      'iii.	and “clipref_max_client_export” value > 0
      'iv.	and the result list count is > the “clipref_max_client_export” value
      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        AllowExport = True 'These people can run whatever export.
      Else
        If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
          'these people are restricted depending
          If Session.Item("localUser").crmMaxClientExport > 0 Then
            'this is further restricted.
            If table.Rows.Count > Session.Item("localUser").crmMaxClientExport Then
              AllowExport = False 'if table row is greater then the max export variable, they need to stop.
            Else
              AllowExport = True
            End If
          Else
            AllowExport = True ' this has no restriction, 0 means unlimited export.
          End If
        End If
      End If
    Else
      'check just for subset string
      If (Server.UrlDecode(Trim(Request("su"))) = "J") Then
        subset_string = "JETNET"
      Else
        subset_string = "JETNET/CLIENT"
      End If



      'jetnet results
      'i.	if the customer has selected BOTH JETNET/CLIENT data sets 
      'ii.	and the user is NOT user type admin
      'iii.	and the result list count is 5,000 value
      'iv.	then display this message when they click on export to excel and do not allow them into it - “Exceeded maximum records allowed for export.  Contact your CRM/Administrator to complete larger exports.”

      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        AllowExport = True 'These people can run whatever export.
      Else
        If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
          'this is further restricted.
          If IsNothing(table) Then
            AllowExport = True
          Else
            If table.Rows.Count > 5000 Then
              AllowExport = False 'if table row is greater then the max export variable, they need to stop.
            Else
              AllowExport = True 'this is an okay record.
            End If
          End If
        End If
        End If
    End If


    If Session.Item("localUser").crmUserType = eUserTypes.GUEST Then
      Session.Item("localUser").crmAllowExport_Flag = False
      Session.Item("localUser").crmDemoUserFlag = True
      AllowExport = False
    End If

  End Function
  Public Sub add_phone()
    add()
  End Sub
  Public Sub add_default_transaction_columns()
    Try
      add()
      export_label.Visible = True
      custom_export.Visible = True
      choice_to_export.ForeColor = Drawing.Color.LightGray
      type_of_info.Enabled = False
      Button1.Enabled = False
      Button2.Enabled = False
      Button3.Enabled = False
      Button4.Enabled = False
      choice_to_export.Enabled = False
      info_to_export.Items.Clear()
      company_new.Visible = True
      form1.Visible = True

      aTempTable = aclsData_Temp.export_default_transaction_columns()

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          'choice_to_export.Items.Clear()
          For Each r As DataRow In aTempTable.Rows
            info_to_export.Items.Add(New ListItem(r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
          Next
        End If
      End If



    Catch ex As Exception
      error_string = "export_creator.aspx.vb -  add_default_transaction_columns() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub

  Public Sub add()
    Try
      phone.Visible = False
      contact_phone.Visible = False
      choice_to_export.Items.Clear()
      Dim strchklist As String = ""
      Dim li As ListItem
      Dim in_box As Boolean = False
      Dim temp_group_name As String = ""

      column.DataType = System.Type.GetType("System.Int64")
      column.DefaultValue = 0
      column.Unique = False
      column.ColumnName = "cliexp_id"
      final_table.Columns.Add(column)

      column2.DataType = System.Type.GetType("System.String")
      column2.DefaultValue = 0
      column2.Unique = False
      column2.ColumnName = "cliexp_type"
      final_table.Columns.Add(column2)

      column3.DataType = System.Type.GetType("System.String")
      column3.DefaultValue = 0
      column3.AllowDBNull = True
      column3.Unique = False
      column3.ColumnName = "cliexp_display"
      final_table.Columns.Add(column3)

      column4.DataType = System.Type.GetType("System.String")
      column4.AllowDBNull = True
      column4.Unique = False
      column4.ColumnName = "cliexp_client_db_name"
      final_table.Columns.Add(column4)

      column5.DataType = System.Type.GetType("System.String")
      column5.AllowDBNull = True
      column5.Unique = False
      column5.ColumnName = "cliexp_jetnet_db_name"
      final_table.Columns.Add(column5)


      column6.DataType = System.Type.GetType("System.String")
      column6.AllowDBNull = True
      column6.Unique = False
      column6.ColumnName = "source"
      final_table.Columns.Add(column6)


      column7.DataType = System.Type.GetType("System.String")
      column7.AllowDBNull = True
      column7.Unique = False
      column7.ColumnName = "cliexp_header_field_name"
      final_table.Columns.Add(column7)

      column8.DataType = System.Type.GetType("System.String")
      column8.AllowDBNull = True
      column8.Unique = False
      column8.ColumnName = "cliexp_field_type"
      final_table.Columns.Add(column8)

      column9.DataType = System.Type.GetType("System.String")
      column9.AllowDBNull = True
      column9.Unique = False
      column9.ColumnName = "cliexp_field_length"
      final_table.Columns.Add(column9)

      column10.DataType = System.Type.GetType("System.String")
      column10.AllowDBNull = True
      column10.Unique = False
      column10.ColumnName = "cliexp_sub_group"
      final_table.Columns.Add(column10)


      For Each li In type_of_info.Items
        If li.Selected Then
          Dim type As String = li.Value

          aTempTable.Clear()
          aTempTable = aclsData_Temp.Build_Export(type, Me.export_type_drop.Text, Session.Item("localSubscription").crmAerodexFlag)

          If Trim(type) = "Feature Code" Or Trim(type) = "Company" Or Trim(type) = "Aircraft" Then
            aTempTable2.Clear()
            aTempTable2 = aclsData_Temp.Build_Custom_Export(type, Me.export_type_drop.Text)

            final_table.Rows.Clear()
            For Each drRow As DataRow In aTempTable.Rows
              final_table.ImportRow(drRow)
            Next


            If Not IsNothing(aTempTable2) Then
              For Each drRow As DataRow In aTempTable2.Rows
                final_table.ImportRow(drRow)
              Next
            End If

            If Not IsNothing(final_table) Then
              If final_table.Rows.Count > 0 Then
                'choice_to_export.Items.Clear()
                For Each r As DataRow In final_table.Rows

                  in_box = False
                  For Each xx In info_to_export.Items
                    If Trim(xx.value) = (r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")) Then
                      in_box = True
                    End If
                  Next

                  If Not IsDBNull(r("cliexp_sub_group")) Then
                    If Trim(r("cliexp_sub_group")) <> "" Then
                      temp_group_name = r("cliexp_sub_group") & " - "
                    ElseIf Trim(type) = "Feature Code" Then ' if its a client feature code 
                      temp_group_name = "Features - "
                    ElseIf Trim(type) = "Company" Then ' if its a client feature code 
                      temp_group_name = "Company - "
                    ElseIf Trim(type) = "Aircraft" Then ' if its a client feature code 
                      temp_group_name = "Aircraft - "
                    Else
                      temp_group_name = ""
                    End If
                  ElseIf Trim(type) = "Feature Code" Then ' if its a client feature code 
                    temp_group_name = "Features - "
                  ElseIf Trim(type) = "Company" Then ' if its a client feature code 
                    temp_group_name = "Company - "
                  ElseIf Trim(type) = "Aircraft" Then ' if its a client feature code 
                    temp_group_name = "Aircraft - "
                  Else
                    temp_group_name = ""
                  End If

                  If in_box = False Then
                    choice_to_export.Items.Add(New ListItem(temp_group_name & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
                  End If

                Next
              End If
            End If



          Else
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                'choice_to_export.Items.Clear()
                For Each r As DataRow In aTempTable.Rows

                  in_box = False
                  For Each xx In info_to_export.Items
                    If Trim(xx.value) = (r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")) Then
                      in_box = True
                    End If
                  Next

                  If Not IsDBNull(r("cliexp_sub_group")) Then
                    If Trim(r("cliexp_sub_group")) <> "" Then
                      temp_group_name = r("cliexp_sub_group") & " - "
                    Else
                      temp_group_name = ""
                    End If
                  Else
                    temp_group_name = ""
                  End If


                  If in_box = False Then
                    choice_to_export.Items.Add(New ListItem(temp_group_name & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
                  End If
                Next
              End If
            End If
          End If



          ' COMMENTED OUT ... now using custom export
          '  If li.Value = "Aircraft" Then
          'get_fields_for_add_comparable(0, choice_to_export)
          ' End If



          If li.Value = "Company" Or li.Value = "Transaction Company" Then
            phone.Visible = True
            If phone.Checked = True Then
              If li.Value = "Company" Then
                aTempTable = aclsData_Temp.Build_Export("Company Phone", Me.export_type_drop.Text)
              Else
                aTempTable = aclsData_Temp.Build_Export("Transaction Company Phone", Me.export_type_drop.Text)
              End If
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  'choice_to_export.Items.Clear()
                  For Each r As DataRow In aTempTable.Rows

                    in_box = False
                    For Each xx In info_to_export.Items
                      If Trim(xx.value) = (r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")) Then
                        in_box = True
                      End If
                    Next

                    If Not IsDBNull(r("cliexp_sub_group")) Then
                      If Trim(r("cliexp_sub_group")) <> "" Then
                        temp_group_name = r("cliexp_sub_group") & " - "
                      Else
                        temp_group_name = ""
                      End If
                    Else
                      temp_group_name = ""
                    End If


                    If in_box = False Then
                      choice_to_export.Items.Add(New ListItem(temp_group_name & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
                    End If
                  Next
                End If
              Else

                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export_creator.aspx.vb - add_phone() - " & error_string)
                End If
                display_error()

              End If
            End If
            If Session("Listing") = 3 Then
              If type_of_info.Items.FindByValue("Aircraft").Selected = True Then
                aTempTable = aclsData_Temp.Build_Export("Company Aircraft", Me.export_type_drop.Text)

                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    'choice_to_export.Items.Clear()
                    For Each r As DataRow In aTempTable.Rows

                      in_box = False
                      For Each xx In info_to_export.Items
                        If Trim(xx.value) = (r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")) Then
                          in_box = True
                        End If
                      Next


                      If Not IsDBNull(r("cliexp_sub_group")) Then
                        If Trim(r("cliexp_sub_group")) <> "" Then
                          temp_group_name = r("cliexp_sub_group") & " - "
                        Else
                          temp_group_name = ""
                        End If
                      Else
                        temp_group_name = ""
                      End If

                      If in_box = False Then
                        choice_to_export.Items.Add(New ListItem(temp_group_name & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
                      End If
                    Next
                  End If
                Else

                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("export_creator.aspx.vb - add_phone() - " & error_string)
                  End If
                  display_error()

                End If
              End If
            End If
          End If
          If li.Value = "Contact" Or li.Value = "Transaction Contact" Then
            contact_phone.Visible = True
            If contact_phone.Checked = True Then
              If li.Value = "Contact" Then
                aTempTable = aclsData_Temp.Build_Export("Contact Phone", Me.export_type_drop.Text)
              Else
                aTempTable = aclsData_Temp.Build_Export("Transaction Contact Phone", Me.export_type_drop.Text)
              End If
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  'choice_to_export.Items.Clear()
                  For Each r As DataRow In aTempTable.Rows


                    in_box = False
                    For Each xx In info_to_export.Items
                      If Trim(xx.value) = (r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")) Then
                        in_box = True
                      End If
                    Next

                    If Not IsDBNull(r("cliexp_sub_group")) Then
                      If Trim(r("cliexp_sub_group")) <> "" Then
                        temp_group_name = r("cliexp_sub_group") & " - "
                      Else
                        temp_group_name = ""
                      End If
                    Else
                      temp_group_name = ""
                    End If


                    If in_box = False Then
                      choice_to_export.Items.Add(New ListItem(temp_group_name & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & r("source") & "|" & r("cliexp_header_field_name")))
                    End If
                  Next
                End If
              Else

                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export_creator.aspx.vb - add_phone() - " & error_string)
                End If
                display_error()

              End If
            End If
          End If
        End If
      Next

      If phone.Visible = False Then
        phone.Checked = False
      End If
      If contact_phone.Visible = False Then
        contact_phone.Checked = False
      End If
      'info_to_export.Items.Clear()
    Catch ex As Exception
      error_string = "export_creator.aspx.vb - Add() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub


  Private lasset As New ArrayList()
  Private lsubordinate As New ArrayList()

  Public Sub AddBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)

    attention.Text = ""
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

          End If
          choice_to_export.Items.Remove(CType(lasset(i), ListItem))
        Next i
      Else
        attention.Text = "<p align='center'>Please select fields to move over</p>"
      End If
      'info_to_export.SelectedValue = ""
    Catch ex As Exception
      error_string = "Export_creator.ascx.vb - AddBtn_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub AddAllBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
    attention.Text = ""
    Try
      While choice_to_export.Items.Count <> 0
        export_label.Visible = True
        Dim i As Integer
        For i = 0 To choice_to_export.Items.Count - 1
          If Not lasset.Contains(choice_to_export.Items(i)) Then
            lasset.Add(choice_to_export.Items(i))
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
      error_string = "Export_creator.ascx.vb - AddAllBtn_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  'Public Sub VerifyNoBadFields()
  '    Try
  '        Dim GoodFields As New ArrayList()
  '        Dim ExportFields As New ArrayList()

  '        'lsubordinate is the list of fields allowed. 
  '        'First we fill it with this list 
  '        Dim i As Integer
  '        For i = 0 To choice_to_export.Items.Count - 1
  '            If Not GoodFields.Contains(choice_to_export.Items(i)) Then
  '                GoodFields.Add(choice_to_export.Items(i))
  '            End If
  '        Next i

  '        For i = 0 To info_to_export.Items.Count - 1
  '            If Not ExportFields.Contains(info_to_export.Items(i)) Then
  '                ExportFields.Add(info_to_export.Items(i))
  '            End If
  '        Next i

  '        'You cannot have it in info to export if it's not in choice to export. 
  '        Dim fiel As New ListItem
  '        Dim safety_number As Integer = GoodFields.Count
  '        For i = 0 To info_to_export.Items.Count - 1
  '            If i < safety_number Then
  '                If info_to_export.Items.Contains(CType(GoodFields(i), ListItem)) Then
  '                    'then it's fine!!!
  '                Else
  '                    info_to_export.Items.Remove(CType(GoodFields(i), ListItem))
  '                End If
  '            Else
  '                info_to_export.Items.Remove(CType(ExportFields(i), ListItem))
  '            End If

  '            ' fiel = CType(GoodFields(i), ListItem)


  '            ' lasset.Add(GoodFields(i))
  '            'choice_to_export.SelectedValue = fiel.Value
  '        Next i
  '    Catch ex As Exception
  '        error_string = "export_creator.aspx.vb - VerifyBadData() - " & ex.Message
  '        LogError(error_string)
  '    End Try

  'End Sub
  Public Sub RemoveBtn_Click(ByVal Src As [Object], ByVal E As EventArgs)
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
        Next i
      Else
        attention.Text = "<p align='center'>Please select fields to move over</p>"
      End If

      If info_to_export.Items.Count = 0 Then
        export_label.Visible = False
      End If
    Catch ex As Exception
      error_string = "Export_creator.ascx.vb - RemoveBtn_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
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

    Catch ex As Exception
      error_string = "Export_creator.ascx.vb - RemoveAllBtn_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  ' ---- ButtonMoveUp_Click --------------------------
  '
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
      error_string = "Export_creator.ascx.vb - ButtonMoveUp_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  ' ---- ButtonMoveDown_Click -------------------------------
  '
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
      error_string = "Export_creator.ascx.vb - ButtonMoveDown_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

#Region "Error Handling for datamanager"
  Function display_error()
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(Replace(aclsData_Temp.class_error, "'", ""), vbNewLine, "") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub
#End Region

  Protected Sub export_now_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles export_now.Click
    Dim listing_id As Integer = Session.Item("Listing")
    Dim field_save As String = ""
    Dim company As Boolean = False
    Dim contact As Boolean = False
    Dim aircraft As Boolean = False
    Dim transaction As Boolean = False
    Dim note As Boolean = False
    Dim include_phone As Boolean = False
    Dim performance_specs As Boolean = False
    Dim operating_costs As Boolean = False
    Dim order_by_string As String = ""


    attention.Text = ""

    '279', 'Prospect', 'Prospect Description', 'lnote_note as prospectnote', '', '2', 'Y', 'Prospect Description', 'String', NULL, 'Prospect'

    Try
      Dim jetnet As String = ""
      Dim client As String = ""
      Dim column_list As String = ""
      Dim first_column As String = ""

      For i = 0 To info_to_export.Items.Count - 1
        If Not lasset.Contains(info_to_export.Items(i)) Then
          Dim t As String = info_to_export.Items(i).Value
          Dim splitstring As Array = Split(info_to_export.Items(i).Value, "|")

          Dim ignore_jetnet = False
          Dim ignore_client = False
          Dim bother As String = info_to_export.Items(i).Value
          Select Case splitstring(3)
            Case "Performance Specs"
              performance_specs = True
              ignore_client = True
            Case "Company", "Transaction Company"
              company = True
            Case "Contact", "Transaction Contact"
              contact = True
            Case "Aircraft"
              aircraft = True
            Case "Transaction"
              transaction = True
            Case "Note"
              note = True
            Case "Company Phone", "Contact Phone", "Transaction Company Phone", "Transaction Contact Phone"
              include_phone = True
            Case Else
              Dim test As String = splitstring(3)
          End Select



          If i = 0 Then
            first_column = splitstring(5)
          End If

          If i < 5 Then
            If Trim(order_by_string) <> "" Then
              order_by_string &= " asc,"
            End If
            order_by_string &= splitstring(5)
          End If

          If i = info_to_export.Items.Count - 1 Then
            column_list = column_list & splitstring(3) & "&&&" & splitstring(5)
          Else
            column_list = column_list & splitstring(3) & "&&&" & splitstring(5) & "|"
          End If

          If listing_id = 6 Or listing_id = 11 Or listing_id = 4 Or listing_id = 16 Then
            If InStr(splitstring(3), "Aircraft") = 0 Then
              ignore_jetnet = True
            Else
              ignore_jetnet = False
            End If
          End If

          If ignore_jetnet = True Then
          Else
            jetnet = jetnet & splitstring(2)
          End If

          If ignore_client = False Then
            client = client & splitstring(1)
          End If

          If InStr(UCase(info_to_export.Items(i).Value), "AS ") > 0 Then
            If ignore_client = False Then
              client = client & ","
            End If

            If ignore_jetnet = True Then
            Else
              jetnet = jetnet & ","
            End If
          Else
            If ignore_client = False Then
              client = client & " as """ & splitstring(5) & ""","
            End If
            If ignore_jetnet = True Then
            Else
              jetnet = jetnet & " as """ & splitstring(5) & ""","
            End If
          End If





          'If i = 0 Then
          '  first_column = info_to_export.Items(i).Text
          '  first_column = Replace(first_column, "Feature ", "")
          'End If

          'If i < 5 Then
          '  If Trim(order_by_string) <> "" Then
          '    order_by_string &= " asc,"
          '  End If
          '  order_by_string &= info_to_export.Items(i).Text
          '  order_by_string = Replace(order_by_string, "Feature ", "")
          'End If

          'If i = info_to_export.Items.Count - 1 Then
          '  column_list = column_list & info_to_export.Items(i).Text
          'Else
          '  column_list = column_list & info_to_export.Items(i).Text & "|"
          'End If

          'If listing_id = 6 Or listing_id = 11 Or listing_id = 4 Or listing_id = 16 Then
          '  If InStr(info_to_export.Items(i).Text, "Aircraft") = 0 Then
          '    ignore_jetnet = True
          '  Else
          '    ignore_jetnet = False
          '  End If
          'End If

          'If ignore_jetnet = True Then
          'Else
          '  jetnet = jetnet & splitstring(2)
          'End If

          'If ignore_client = False Then
          '  client = client & splitstring(1)
          'End If

          'If InStr(UCase(info_to_export.Items(i).Value), "AS ") > 0 Then
          '  If ignore_client = False Then
          '    client = client & ","
          '  End If

          '  If ignore_jetnet = True Then
          '  Else
          '    jetnet = jetnet & ","
          '  End If
          'Else
          '  If ignore_client = False Then
          '    client = client & " as """ & info_to_export.Items(i).Text & ""","
          '  End If
          '  If ignore_jetnet = True Then
          '  Else
          '    jetnet = jetnet & " as """ & info_to_export.Items(i).Text & ""","
          '  End If
          'End If



        End If

      Next i


      If jetnet = "" And client = "" Then
        attention.Text = "<p align='center'>Please select fields to export.</p>"
      Else
        If client <> "" Then
          client = UCase(client.TrimEnd(","))
        End If

        If jetnet <> "" Then
          jetnet = UCase(jetnet.TrimEnd(","))
        End If

        Dim returned As New DataTable
        Dim subnode As String = Trim(Request("sn"))
        Dim subnode_exists As String = Trim(Request("snt"))
        Dim arComp_ids_JETNET As String = ""
        Dim arComp_ids_Client As String = ""

        If subnode_exists <> "" Then
          aTempTable = aclsData_Temp.Get_Client_Folder_Index(subnode)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              ' build an string of comp_ids
              For count As Integer = 0 To aTempTable.Rows.Count - 1
                If listing_id = 1 Then
                  If aTempTable.Rows(count).Item("cfoldind_jetnet_comp_id") <> 0 Then
                    arComp_ids_JETNET = arComp_ids_JETNET & aTempTable.Rows(count).Item("cfoldind_jetnet_comp_id") & ","
                  End If
                  If aTempTable.Rows(count).Item("cfoldind_client_comp_id") <> 0 Then
                    arComp_ids_Client = arComp_ids_Client & aTempTable.Rows(count).Item("cfoldind_client_comp_id") & ","
                  End If
                ElseIf listing_id = 2 Then
                  If aTempTable.Rows(count).Item("cfoldind_jetnet_contact_id") <> 0 Then
                    arComp_ids_JETNET = arComp_ids_JETNET & "" & aTempTable.Rows(count).Item("cfoldind_jetnet_contact_id") & ","
                  End If
                  If aTempTable.Rows(count).Item("cfoldind_client_contact_id") <> 0 Then
                    arComp_ids_Client = arComp_ids_Client & "" & aTempTable.Rows(count).Item("cfoldind_client_contact_id") & ","
                  End If
                ElseIf listing_id = 3 Then
                  If aTempTable.Rows(count).Item("cfoldind_jetnet_ac_id") <> 0 Then
                    arComp_ids_JETNET = arComp_ids_JETNET & aTempTable.Rows(count).Item("cfoldind_jetnet_ac_id") & ","
                  End If
                  If aTempTable.Rows(count).Item("cfoldind_client_ac_id") <> 0 Then
                    arComp_ids_Client = arComp_ids_Client & aTempTable.Rows(count).Item("cfoldind_client_ac_id") & ","
                  End If
                End If

              Next
              aTempTable.Dispose()
              arComp_ids_Client = UCase(arComp_ids_Client.TrimEnd(","))
              arComp_ids_JETNET = UCase(arComp_ids_JETNET.TrimEnd(","))
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("export_creator.aspx.vb - Fill_Company() - " & error_string)
            End If
            display_error()
          End If
        End If


        Dim type As Integer = 1
        ''''''''''''''''''''''Let's figure out the model strings once, for airplanes, transactions or notes.'''''''''''''''''''''''''''''''''''
        Dim model_cbo As String = ""
        Dim jetnet_model_id As String = ""
        Dim client_model_id As String = ""

        If (listing_id = 3 And subnode_exists = "") Or listing_id = 8 Or listing_id = 6 Or listing_id = 14 Or listing_id = 16 Then
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

              'If model_info(4) <> 0 Then
              If x = 0 Then
                client_model_id = "'"
              End If
              client_model_id = client_model_id & model_info(4)
              If x <> UBound(model_sets) Then
                client_model_id = client_model_id & "','"
              Else
                client_model_id = client_model_id & "'"
              End If
              'End If


              'If model_info(0) <> 0 Then
              If x = 0 Then
                jetnet_model_id = "'"
              End If
              jetnet_model_id = jetnet_model_id & model_info(0)
              If x <> UBound(model_sets) Then
                jetnet_model_id = jetnet_model_id & "','"
              Else
                jetnet_model_id = jetnet_model_id & "'"
              End If
              'End If

            Next
            If client_model_id <> "" Then
              client_model_id = UCase(client_model_id.TrimEnd(","))
            End If
            If jetnet_model_id <> "" Then
              jetnet_model_id = UCase(jetnet_model_id.TrimEnd(","))
            End If
          End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''end model figure out'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If listing_id = 14 Then
          Dim retur As String
          retur = aclsData_Temp.View_Exports(first_column, jetnet, jetnet_model_id, performance_specs, operating_costs)
          Response.Write(retur)
        ElseIf listing_id = 1 Then
          Dim state As String = Server.UrlDecode(Trim(Request("state")))
          Dim owners As String = Server.UrlDecode(Trim(Request("owners")))
          Dim country As String = Server.UrlDecode(Trim(Request("country")))
          Dim subset As String = Server.UrlDecode(Trim(Request("su")))
          Dim search As String = Server.UrlDecode(Trim(Request("search")))
          Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))
          Dim special_field As String = Server.UrlDecode(Trim(Request("sp_cbo")))
          Dim special_field_txt As String = Server.UrlDecode(Trim(Request("sp_txt")))
          Dim show_all As Boolean = Server.UrlDecode(Trim(Request("all")))
          Dim status As String = Server.UrlDecode(Trim(Request("st")))
          Dim companyCity As String = Server.UrlDecode(Trim(Request("ccs")))
          Dim companyPhone As String = Server.UrlDecode(Trim(Request("ccp")))

          If search_where = 2 Then
            search = search & "%"
          Else
            search = "%" & search & "%"
          End If

          If companyPhone <> "" Then
            Try
              companyPhone = Replace(companyPhone, ".", "%")
              companyPhone = Replace(companyPhone, "(", "%")
              companyPhone = Replace(companyPhone, ")", "%")
              companyPhone = "%" & companyPhone & "%"

              aTempTable = aclsData_Temp.SearchPhoneNumbers(companyPhone)
              '' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If r("source") = "CLIENT" Then
                      arComp_ids_Client = arComp_ids_Client & r("pnum_comp_id") & ","
                    Else
                      arComp_ids_JETNET = arComp_ids_JETNET & r("pnum_comp_id") & ","
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export_creator.aspx.vb - export_now_click() Phone # Company Export - " & error_string)

                  display_error()
                End If
              End If


              If arComp_ids_JETNET <> "" Then
                arComp_ids_JETNET = UCase(arComp_ids_JETNET.TrimEnd(","))
                subnode_exists = "True"
              End If
              If arComp_ids_Client <> "" Then
                arComp_ids_Client = UCase(arComp_ids_Client.TrimEnd(","))
                subnode_exists = "True"
              End If
            Catch ex As Exception
              error_string = ex.Message
              LogError("export_creator.aspx.vb - export_now_click() Phone # Company Export - " & error_string)

              display_error()
            End Try
          End If

          'adding a catch not to poll the database if it's a folder with no client or no jetnet
          If subnode_exists <> "" Then
            If arComp_ids_Client <> "" Or arComp_ids_JETNET <> "" Then
              If arComp_ids_JETNET = "" Then
                subset = "C"
              ElseIf arComp_ids_Client = "" Then
                subset = "J"
              End If
            End If
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

          returned = aclsData_Temp.Export_All(first_column, client, jetnet, company, contact, aircraft, transaction, subset, special_field, special_field_txt, search, status, country, state_string, owners, include_phone, "%", "%", arComp_ids_JETNET, arComp_ids_Client, listing_id, "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", companyCity, order_by_string)
        ElseIf listing_id = 2 Then


          Dim search_first As String = Server.UrlDecode(Trim(Request("fn")))
          'Last Name/Single Quote Replacement
          Dim search_last As String = Server.UrlDecode(Trim(Request("ln")))
          search_last = Replace(search_last, "'", "''")

          Dim search_where As String = IIf(IsNumeric(Server.UrlDecode(Trim(Request("sw")))), Server.UrlDecode(Trim(Request("sw"))), 2)
          'Company Name/Single Quote Replacement
          Dim company_name As String = Server.UrlDecode(Trim(Request("cn")))
          company_name = Replace(company_name, "'", "''")
          Dim contactPhone As String = Server.UrlDecode(Trim(Request("cphn")))
          Dim status_cbo As String = Server.UrlDecode(Trim(Request("st")))
          Dim subset As String = Server.UrlDecode(Trim(Request("su")))
          'Email Address/Single Quote Replacement
          Dim contact_email_address As String = Server.UrlDecode(Trim(Request("cem")))
          contact_email_address = Replace(contact_email_address, "'", "''")


          If search_where = 2 Then
            company_name = company_name & "%"
            search_last = search_last & "%"
            search_first = search_first & "%"
            contact_email_address = contact_email_address & "%"
          Else
            company_name = "%" & company_name & "%"
            search_last = search_last & "%"
            search_first = search_first & "%"
            contact_email_address = "%" & contact_email_address & "%"
          End If


          If contactPhone <> "" Then
            Try
              contactPhone = Replace(contactPhone, ".", "%")
              contactPhone = Replace(contactPhone, "(", "%")
              contactPhone = Replace(contactPhone, ")", "%")
              contactPhone = "%" & contactPhone & "%"

              aTempTable = aclsData_Temp.SearchPhoneNumbers(contactPhone)
              '' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If r("pnum_contact_id") > 0 Then
                      If r("source") = "CLIENT" Then
                        arComp_ids_Client = arComp_ids_Client & r("pnum_contact_id") & ","
                      Else
                        arComp_ids_JETNET = arComp_ids_JETNET & r("pnum_contact_id") & ","
                      End If
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export_creator.aspx.vb - export_now_click() Phone # Company Export - " & error_string)

                  display_error()
                End If
              End If


              If arComp_ids_JETNET <> "" Then
                arComp_ids_JETNET = UCase(arComp_ids_JETNET.TrimEnd(","))
                subnode_exists = "True"
              End If
              If arComp_ids_Client <> "" Then
                arComp_ids_Client = UCase(arComp_ids_Client.TrimEnd(","))
                subnode_exists = "True"
              End If
            Catch ex As Exception
              error_string = ex.Message
              LogError("export_creator.aspx.vb - export_now_click() Phone # Company Export - " & error_string)

              display_error()
            End Try
          End If


          'adding a catch not to poll the database if it's a folder with no client or no jetnet
          If subnode_exists <> "" Then
            If arComp_ids_Client <> "" Or arComp_ids_JETNET <> "" Then
              If arComp_ids_JETNET = "" Then
                subset = "C"
              ElseIf arComp_ids_Client = "" Then
                subset = "J"
              End If
            End If
          End If


          returned = aclsData_Temp.Export_All(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "", company_name, status_cbo, "", "", "", include_phone, search_first, search_last, arComp_ids_JETNET, arComp_ids_Client, listing_id, "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", contact_email_address, "", "", "", "", "", order_by_string)
        ElseIf listing_id = 3 Then

          Dim search As String = Server.UrlDecode(Trim(Request("se")))
          Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))

          Dim ownership As String = Server.UrlDecode(Trim(Request("ot")))
          Dim lifecycle As String = Server.UrlDecode(Trim(Request("lcs")))
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
          Dim CustomField1 As String = Server.UrlDecode(Trim(Request("c1")))
          Dim CustomField2 As String = Server.UrlDecode(Trim(Request("c2")))
          Dim CustomField3 As String = Server.UrlDecode(Trim(Request("c3")))
          Dim CustomField4 As String = Server.UrlDecode(Trim(Request("c4")))
          Dim CustomField5 As String = Server.UrlDecode(Trim(Request("c5")))
          Dim CustomField6 As String = Server.UrlDecode(Trim(Request("c6")))
          Dim CustomField7 As String = Server.UrlDecode(Trim(Request("c7")))
          Dim CustomField8 As String = Server.UrlDecode(Trim(Request("c8")))
          Dim CustomField9 As String = Server.UrlDecode(Trim(Request("c9")))
          Dim CustomField10 As String = Server.UrlDecode(Trim(Request("c10")))
          Dim customFieldString As String = ""
          Dim country_string As String = ""
          Dim state_string As String = ""
          Dim AircraftNotesSearch As Integer = 0
          Dim AircraftNoteDate As String = ""

          'If this is 0, basically ignore this parameter.
          'If this is 1 or 2, we need to use it to get a list of with or without notes aircraft 
          If Not IsNothing(Trim(Request("nss"))) Then
            If IsNumeric(Trim(Request("nss"))) Then
              AircraftNotesSearch = Server.UrlDecode(Trim(Request("nss")))
            End If
          End If

          If Not IsNothing(Trim(Request("and"))) Then
            AircraftNoteDate = Server.UrlDecode(Trim(Request("and")))
          End If

          If AircraftNotesSearch = 1 Or AircraftNotesSearch = 2 Then
            Dim NoteJetnetModels As String = ""
            Dim NoteClientModels As String = ""
            clsGeneral.clsGeneral.SetUpSpecialModels(model_cbo, NoteClientModels, NoteJetnetModels)

            arComp_ids_Client = clsGeneral.clsGeneral.BuildClientACString(NoteClientModels, NoteJetnetModels, AircraftNoteDate)

            arComp_ids_JETNET = clsGeneral.clsGeneral.BuildJetnetCompIds(aclsData_Temp, AircraftNoteDate, NoteClientModels, NoteJetnetModels, subset, AircraftNotesSearch)
          End If

          customFieldString = clsGeneral.clsGeneral.BuildCustomFieldsString(subset, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10)


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

          'Setting up Country String
          If country_cbo <> "" Then
            Dim countries As Array = Split(country_cbo, ",")
            For x = 0 To UBound(countries)
              If Trim(countries(x)) <> "" Then
                country_string += "'" & countries(x) & "',"
              End If
            Next
          End If

          If country_string <> "" Then
            country_string = UCase(country_string.TrimEnd(","))
          End If


          If Trim(search_where) <> "" Then
            If search_where = 1 Then
              search = "%" & search & "%"
            Else
              search = "" & search & "%"
            End If
          Else
            search = "" & search & "%"
          End If

          'adding a catch not to poll the database if it's a folder with no client or no jetnet
          If subnode_exists <> "" Then
            If arComp_ids_Client <> "" Or arComp_ids_JETNET <> "" Then
              If arComp_ids_JETNET = "" Then
                subset = "C"
              ElseIf arComp_ids_Client = "" Then
                subset = "J"
              End If
            End If
          End If


          returned = aclsData_Temp.Export_All(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "", "", "Y", "", "", "", include_phone, "%", "%", arComp_ids_JETNET, arComp_ids_Client, listing_id, search, market_status_cbo, airport_name, icao_code, iata_code, city, country_string, state_string, types_of_owners, client_model_id, jetnet_model_id, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, on_exclusive, on_lease, "", "", "", "", "", "", "", "", "", "", year_start, year_end, "", "", "", "", "", lifecycle, ownership, "", order_by_string, Me.merge_lists.Checked, AircraftNotesSearch)
        ElseIf listing_id = 8 Then
          Dim internal As String = Server.UrlDecode(Trim(Request("in")))
          Dim awaiting As String = Server.UrlDecode(Trim(Request("ad")))
          Dim year_start As String = Server.UrlDecode(Trim(Request("tys")))
          Dim year_end As String = Server.UrlDecode(Trim(Request("tye")))
          Dim trans_search As String = Server.UrlDecode(Trim(Request("se")))
          Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))
          Dim subset As String = Server.UrlDecode(Trim(Request("d")))
          Dim trans_trans_type As String = Server.UrlDecode(Trim(Request("t")))
          Dim trans_start_date As String = Server.UrlDecode(Trim(Request("s")))
          If IsDate(trans_start_date) Then
            trans_start_date = Year(trans_start_date) & "-" & Month(trans_start_date) & "-" & Day(trans_start_date)
          End If
          Dim trans_end_date As String = Server.UrlDecode(Trim(Request("e")))
          If IsDate(trans_end_date) Then
            trans_end_date = Year(trans_end_date) & "-" & Month(trans_end_date) & "-" & Day(trans_end_date)
          Else
            trans_end_date = Year(Now()) & "-" & Month(Now()) & "-" & Day(Now())
          End If
          If search_where = 1 Then
            trans_search = "%" & trans_search & "%"
          Else
            trans_search = "" & trans_search & "%"
          End If

          returned = aclsData_Temp.Export_All(first_column, client, jetnet, company, contact, aircraft, transaction, subset, "", "", "", "", "", "", "", include_phone, "%%", "%%", arComp_ids_JETNET, arComp_ids_Client, listing_id, "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", trans_search, jetnet_model_id, client_model_id, trans_trans_type, trans_start_date, trans_end_date, "", "", "", "", year_start, year_end, internal, awaiting, "", "", "", "", "", "", order_by_string)

        ElseIf listing_id = 6 Or listing_id = 4 Or listing_id = 11 Or listing_id = 16 Then
          'Aircraft search related items
          Dim acSearchOperator As Integer = 0
          Dim acSearchField As Integer = 0
          Dim acSearchText As String = ""
          Dim AircraftSearch As Boolean = False 'This only needs to be set if the searchText for ac is something
          Dim temporaryTable As New DataTable
          Dim noteIds As String = ""

          'Other request variables:
          Dim note_search As String = Server.UrlDecode(Trim(Request("sf")))
          Dim search_where As String = Server.UrlDecode(Trim(Request("sw")))
          Dim note_start_date As String = Server.UrlDecode(Trim(Request("st")))
          Dim note_user As String = Server.UrlDecode(Trim(Request("us")))
          Dim note_cat As String = Server.UrlDecode(Trim(Request("ca")))
          Dim opp_status As String = Server.UrlDecode(Trim(Request("opp")))
          Dim lnote_order As String = Server.UrlDecode(Trim(Request("no")))
          Dim note_end_date As String = Server.UrlDecode(Trim(Request("en")))
          Dim note_type As String = Server.UrlDecode(Trim(Request("nt")))

          'Folder Types
          Dim FolderType As Long = 3
          If Not IsNothing(Server.UrlDecode(Trim(Request("pft")))) Then
            If IsNumeric(Server.UrlDecode(Trim(Request("pft")))) Then
              FolderType = Server.UrlDecode(Trim(Request("pft")))
            End If
          End If
          Dim FolderID As Long = 0
          If Not IsNothing(Server.UrlDecode(Trim(Request("pfi")))) Then
            If IsNumeric(Server.UrlDecode(Trim(Request("pfi")))) Then
              FolderID = Server.UrlDecode(Trim(Request("pfi")))
            End If
          End If

          Dim prospectType As Integer = 0
          Dim clientIDs As String = ""
          Dim jetnetIDs As String = ""


          ' addded in MSW 12/30/15 - to include other kinds of notes in the export, hubbard crm

          If listing_id = 16 Then
            If ac_prospect.Text <> "" Then
              Dim ac_sets As Array = Split(ac_prospect.Text, ",")

              For x = 0 To UBound(ac_sets)

                Dim ac_info As Array = Split(ac_sets(x), "|")

                If x = 0 Then
                  clientIDs = "'"
                End If

                clientIDs = clientIDs & ac_info(0)
                If x <> UBound(ac_sets) Then
                  clientIDs += "','"
                Else
                  clientIDs += "'"
                End If


                If x = 0 Then
                  jetnetIDs += "'"
                End If

                jetnetIDs += ac_info(1)
                If x <> UBound(ac_sets) Then
                  jetnetIDs += "','"
                Else
                  jetnetIDs += "'"
                End If


              Next
            End If
          End If

          If IsNumeric(Trim(Request("pty"))) Then
            prospectType = Server.UrlDecode(Trim(Request("pty")))
          End If

          'Setting the search operator, but only if it's numeric/set
          If Not IsNothing(Server.UrlDecode(Trim(Request("acOp")))) Then
            If IsNumeric(Server.UrlDecode(Trim(Request("acOp")))) Then
              acSearchOperator = Server.UrlDecode(Trim(Request("acOp")))
            End If
          End If

          'Setting the search field operator, but only if it's numeric/set
          If Not IsNothing(Server.UrlDecode(Trim(Request("acSF")))) Then
            If IsNumeric(Server.UrlDecode(Trim(Request("acSF")))) Then
              acSearchField = Server.UrlDecode(Trim(Request("acSF")))
            End If
          End If

          If Not IsNothing(Server.UrlDecode(Trim(Request("acST")))) Then
            If Not String.IsNullOrEmpty(Server.UrlDecode(Trim(Request("acST")))) Then
              acSearchText = Server.UrlDecode(Trim(Request("acST")))
            End If
          End If

          'Let's figure out if we're running an aircraft search
          If Not String.IsNullOrEmpty(acSearchText) Then
            AircraftSearch = True
            acSearchText = Trim(clsGeneral.clsGeneral.StripChars(acSearchText, True))
          Else
            acSearchField = 0
            acSearchOperator = 0
            acSearchText = ""
          End If

          If FolderType > 0 Then
            If FolderID > 0 Then
              Dim FolderDataTable As New DataTable
              Dim FieldToPoll As String = "ac"
              jetnetIDs = ""
              clientIDs = ""

              Select Case FolderType
                Case 2
                  FieldToPoll = "contact"
                Case 1
                  FieldToPoll = "comp"
              End Select

              FolderDataTable = aclsData_Temp.Get_Client_Folder_Index(CLng(FolderID))
              If Not IsNothing(FolderDataTable) Then
                If FolderDataTable.Rows.Count > 0 Then
                  ' build an string of ac_ids
                  For Each r As DataRow In FolderDataTable.Rows
                    If Not IsDBNull(r("cfoldind_jetnet_" & FieldToPoll & "_id")) Then
                      If IsNumeric(r("cfoldind_jetnet_" & FieldToPoll & "_id")) Then
                        If r("cfoldind_jetnet_" & FieldToPoll & "_id") > 0 Then
                          If jetnetIDs <> "" Then
                            jetnetIDs += ","
                          End If
                          jetnetIDs += r("cfoldind_jetnet_" & FieldToPoll & "_id").ToString
                        End If
                      End If
                    End If

                    If Not IsDBNull(r("cfoldind_client_" & FieldToPoll & "_id")) Then
                      If IsNumeric(r("cfoldind_client_" & FieldToPoll & "_id")) Then
                        If r("cfoldind_client_" & FieldToPoll & "_id") > 0 Then
                          If clientIDs <> "" Then
                            clientIDs += ","
                          End If
                          clientIDs += r("cfoldind_client_" & FieldToPoll & "_id").ToString
                        End If
                      End If
                    End If
                  Next
                Else
                  jetnetIDs += ""
                  clientIDs += ""
                End If
              End If
            End If
          End If

          If IsDate(note_start_date) Then
            note_start_date = Year(note_start_date) & "-" & Month(note_start_date) & "-" & Day(note_start_date)
          End If

          If IsDate(note_end_date) Then
            note_end_date = Year(note_end_date) & "-" & Month(note_end_date) & "-" & Day(note_end_date)
          End If

          If IsNumeric(search_where) Then
            If search_where = 2 Then
              note_search = "%" & note_search & "%"
            Else
              note_search = "" & note_search & "%"
            End If
          End If


          'Clear the debugging
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText = ""

          'What if here - if we determine it's an aircraft based search - then we go ahead and transfer all of our data to the 
          'Notes_Search function
          'Return a list of IDs of applicable notes and then pass those note IDs to the export notes function.
          'That way we wouldn't care about joins or filtering on the export level. We'd already have the note IDs we need.

          'We only need to run this if aircraftsearch = true
          'Otherwise we can just carry on.
          If AircraftSearch = True Then
            temporaryTable = aclsData_Temp.Notes_Search(note_search, note_start_date, note_end_date, note_type, IIf(IsNumeric(note_cat), note_cat, 0), jetnet_model_id, client_model_id, note_user, opp_status, clientIDs, jetnetIDs, acSearchField, acSearchOperator, acSearchText, False, False, FolderType)

            If Not IsNothing(temporaryTable) Then
              If temporaryTable.Rows.Count > 0 Then
                For Each r As DataRow In temporaryTable.Rows
                  If noteIds <> "" Then
                    noteIds += ","
                  End If
                  noteIds += r("lnote_id")
                Next
                returned = aclsData_Temp.Export_Notes(first_column, client, jetnet, company, contact, aircraft, transaction, listing_id, note_search, note_start_date, note_end_date, note_type, client_model_id, jetnet_model_id, note_user, note_cat, opp_status, lnote_order, column_list, noteIds, IIf(prospectType = 2, True, False), IIf(prospectType = 1, True, False), clientIDs, jetnetIDs, FolderType)
              Else
                returned = New DataTable 'No applicable notes to show, so no results.
              End If
            End If
          Else
            returned = aclsData_Temp.Export_Notes(first_column, client, jetnet, company, contact, aircraft, transaction, listing_id, note_search, note_start_date, note_end_date, note_type, client_model_id, jetnet_model_id, note_user, note_cat, opp_status, lnote_order, column_list, noteIds, IIf(prospectType = 2, True, False), IIf(prospectType = 1, True, False), clientIDs, jetnetIDs, FolderType)
          End If

        End If


        If Not IsNothing(returned) Then
          If returned.Columns.Count > 0 Then

            aclsData_Temp.Insert_CRM_Event("CRM EXPORT", Application.Item("crmClientSiteData").crmClientHostName, "This user has performed a " & subset_string & " export returning " & returned.Rows.Count & " rows.", Session.Item("localUser").crmLocalUserName)
            'Dim runExport As Boolean = AllowExport(returned)
            'If runExport = True Then
            'Dim test As Integer = returned.Rows.Count
            '  Response.Write(field_save)
            Dim stringwrite As System.IO.StringWriter = New System.IO.StringWriter
            Dim htmlwrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringwrite)
            gridview1.AllowPaging = False

            gridview1.DataSource = returned
            gridview1.DataBind()


            gridview1.RenderControl(htmlwrite)
            ' Session("export_info") = "<p align='center'><b style='font-size:19px;'>Custom Company/Contact Export</b></p>" & stringwrite.ToString()
            Session("export_info") = stringwrite.ToString()
            gridview1.Visible = False

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.open('export.aspx','_blank','width=400,height=400,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "self.close();", True)
            Response.Redirect("export.aspx", False)
            'ElseIf runExport = False Then
            '    no_export_error.Visible = True
            '    no_export_error.Text = "<p align='center'>Exceeded maximum records allowed for export.  Contact your CRM/Administrator to complete larger exports.</p>"

            'End If


          Else
            ' Response.Write("<h1>Debug Text</h1>" & Session.Item("localUser").crmUser_DebugText)
            attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
          End If

        Else

          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("export_creator.aspx.vb - export_now_click() - " & error_string)

            display_error()
          Else
            attention.Text = "<p align='center'>None of the selected information exists for this dataset.</p>"
          End If

        End If
      End If
    Catch ex As Exception
      error_string = "export_creator.aspx.vb - export_now_click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub custom_export_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles custom_export.CheckedChanged
    If custom_export.Checked = True Then
      Try
        choice_to_export.Items.Clear()
        export_label.Visible = False
        info_to_export.Items.Clear()
        choice_to_export.ForeColor = Drawing.Color.Black
        company_new.Visible = True
        form1.Visible = True
        add()
        type_of_info.Enabled = True
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        choice_to_export.Enabled = True
      Catch ex As Exception
        error_string = "export_creator.aspx.vb - Trans Display - " & ex.Message
        LogError(error_string)
      End Try
    Else
      add_default_transaction_columns()
    End If

  End Sub

  Private Sub Fill_Open_Box_old()
    Dim jetnet_model_string As String = ""

    aTempTable = aclsData_Temp.Client_Project_Details_By_User_ID(Session.Item("localUser").crmLocalUserID)
    open_project_ddl.Items.Clear()
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each q As DataRow In aTempTable.Rows
          If Session.Item("Listing") = q("cliproj_source") Then
            If q("cliproj_shared") = "N" Then
              open_project_ddl.Items.Add(New ListItem(q("cliproj_name"), q("cliproj_name") & "++++++" & q("cliproj_id")))
            End If

          End If
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("export_creator.aspx.vb -file_menu_MenuItemClick() - " & error_string)
      End If
      display_error()
    End If

    'Projects this person can access via sharing.
    aTempTable = aclsData_Temp.Client_Project_Details_By_Shared_Flag("Y")
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each q As DataRow In aTempTable.Rows
          ' If q("cliproj_user_id") <> Session.Item("localUser").crmLocalUserID Then
          If Session.Item("Listing") = q("cliproj_source") Then
            open_project_ddl.Items.Add(New ListItem(q("cliproj_name"), q("cliproj_name") & "++++++" & q("cliproj_id")))
          End If
          'End If
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("export_creator.aspx.vb -file_menu_MenuItemClick() - " & error_string)
      End If
      display_error()
    End If




  End Sub

  Private Sub file_save_as_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles file_save_as.Click
    attention.Text = ""
    Try
      Save_Project(0)


    Catch ex As Exception
      error_string = "export_creator.aspx.vb - file_save_as_Click - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub file_save_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles file_save.Click
    attention.Text = ""
    Try
      Save_Project(CInt(file_id.Text))
    Catch ex As Exception
      error_string = "export_creator.aspx.vb - file_save_Click - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Show_Load_Export()
    load_export_label.Visible = False
    'Setting up the file menu - Does Open Project exist? 
    aTempTable = aclsData_Temp.Client_Project_Details_By_User_ID(Session.Item("localUser").crmLocalUserID)

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        'This means that there's a project you can open
        For Each z As DataRow In aTempTable.Rows
          If Not IsDBNull(z("cliproj_source")) Then
            If Session.Item("Listing") = z("cliproj_source") Then
              load_export_label.Visible = True
              'file_menu.Items(0).ChildItems.Add(New MenuItem("Open Project", 1))
            End If
          End If
        Next
      End If
      'However if there isn't one, you still need to check for one that you can share.
      aTempTable = aclsData_Temp.Client_Project_Details_By_Shared_Flag("Y")

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then 'We found shared project
          For Each z As DataRow In aTempTable.Rows

            If Not IsDBNull(z("cliproj_source")) Then
              If Session.Item("Listing") = z("cliproj_source") Then
                load_export_label.Visible = True
                'file_menu.Items(0).ChildItems.Add(New MenuItem("Open Project", 1))
              End If
            End If
          Next
        Else
          'sorry, there's really no projects you can open.
        End If
      End If

      If Session("show_open") = True Then
        file_menu.Items(0).ChildItems.Add(New MenuItem("Open Export Template", 1))
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("export_creator.aspx.vb - Page Load() - " & error_string)
      End If
      display_error()
    End If

  End Sub
  Private Sub Save_Project(ByVal id As Integer)
    Dim zerofix As String = ""
    Dim model_defaut_update As Boolean = False

    Try
      'check to see if the file name is clear

      aTempTable = aclsData_Temp.Client_Project_Details_By_Name(id)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count = 0 Or id <> 0 Then

          'First I need to save the project data. So here we go.
          'I need to figure out what type of project this is going to be.
          Dim project_source As String = Session.Item("Listing")
          Dim project_id As Integer = 0

          attention.Text = ""

          Dim aclsClient_Project As New clsClient_Project
          aclsClient_Project.cliproj_action_date = Now()
          aclsClient_Project.cliproj_description = CStr(Replace(file_description.Text, "'", "''"))
          aclsClient_Project.cliproj_name = CStr(Replace(file_name.Text, "'", "''"))
          aclsClient_Project.cliproj_shared = CStr(IIf(file_shared.Checked, "Y", "N"))
          aclsClient_Project.cliproj_market_default = CStr(IIf(check_market_default.Checked, "Y", "N"))
          If Trim(Request("new_project")) <> "" Then
            aclsClient_Project.cliproj_source = "3"  ' make it an AC folder
          ElseIf project_source > 0 Then   ' added in MSW  - was defaulting to 0 - 11/7/18
            aclsClient_Project.cliproj_source = project_source
          Else ' added in MSW  - was defaulting to 0   11/7/18
            aclsClient_Project.cliproj_source = "3"
          End If
 
          aclsClient_Project.cliproj_type = "E" 'default E for export!
          aclsClient_Project.cliproj_user_id = CStr(Session.Item("localUser").crmLocalUserID)


          If Trim(Me.model_list_source.Items(Me.model_list.SelectedIndex).Text) = "client" Then
            zerofix = Me.model_list.SelectedValue
            zerofix = Left(zerofix, Len(zerofix) - 3)
            aclsClient_Project.cliproj_jetnet_model = aclsData_Temp.Get_JETNET_Aircraft_ModelID_BY_CLIENT_MODEL(zerofix)
            aclsClient_Project.cliproj_client_model = zerofix
          Else
            aclsClient_Project.cliproj_jetnet_model = Me.model_list.SelectedValue
            aclsClient_Project.cliproj_client_model = 0
          End If

          If Me.default_model_export.Checked Then
            aclsClient_Project.cliproj_model_default = "Y"
            model_defaut_update = True
          Else
            aclsClient_Project.cliproj_model_default = "N"
          End If



          If id = 0 Then
            project_id = aclsData_Temp.Client_Project_Insert(aclsClient_Project)
          Else
            project_id = id
            aclsClient_Project.cliproj_id = id
            aclsData_Temp.Client_Project_Update(aclsClient_Project)
            'delete previous references. 
            aclsData_Temp.Client_Project_Reference_Delete(id)
          End If
          If project_id <> 0 Then
            If info_to_export.Items.Count <> 0 Then
              For i = 0 To info_to_export.Items.Count - 1
                If Not lasset.Contains(info_to_export.Items(i)) Then
                  Dim splitstring As Array = Split(info_to_export.Items(i).Value, "|")
                  Dim aclsClient_Project_Reference As New clsClient_Project_Reference
                  aclsClient_Project_Reference.clipref_cliproj_id = project_id
                  aclsClient_Project_Reference.clipref_exp_id = splitstring(0)
                  aclsClient_Project_Reference.clipref_sort_order = i
                  aclsClient_Project_Reference.clipref_source = splitstring(4)
                  aclsData_Temp.Client_Project_Reference_Insert(aclsClient_Project_Reference)
                End If
              Next i
            End If
            attention.Text = "<p align='center'>Your Format was saved.</p>"
          Else
            attention.Text = "<p align='center'>There was a problem saving your information.</p>"
          End If
          'file_open_dialog.Visible = False
          'open_project.Visible = True
          'Fill_Open_Box()


          If model_defaut_update = True Then
            Call aclsData_Temp.UPDATE_DEFAULT_MODELS(aclsClient_Project.cliproj_jetnet_model, project_source.ToString, project_id, aclsClient_Project.cliproj_user_id)
          End If



          Better_Open_Project(project_id)
          file_save_as.Visible = True
          file_delete.Visible = True
          load_export_label.Visible = True
        Else
          attention.Text = "<p align='center'>Please choose a different name. This one is already in use.</p>"
        End If
      End If
    Catch ex As Exception
      error_string = "export_creator.aspx.vb - file_save_Click - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub market_defualt_check_changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles check_market_default.CheckedChanged

    If check_market_default.Checked = True Then
      Me.warning_label.Text = "Saving this template as the Market Default will set this template as the default for all users under this subscription.  Are you sure you want to continue?"
    Else
      Me.warning_label.Text = "Removing this template as the Market Default will remove this template as the default for all users under this subscription.  Are you sure you want to continue?"
    End If 
    Me.warning_label.Visible = True
    '  MsgBox("Saving this template as the Market Default will set this template as the default for all users under this subscription.  Are you sure you want to continue?", MsgBoxStyle.OkCancel)

    '  MsgBox("Removing this template as the Market Default will remove this template as the default for all users under this subscription.  Are you sure you want to continue?", MsgBoxStyle.OkCancel)

  End Sub



  Private Sub Open_Project_Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Open_Project_Btn.Click

    Call open_project_function()

  End Sub

  Public Sub open_project_function()

    Dim temp_id As Long = 0

    Try
      attention.Text = ""
      'change the button to save as.
      ' file_save.ImageUrl = "images/save_as.gif"
      open_project.Visible = False
      export_now.Visible = True
      export_label.Visible = True


      If Trim(Request("project_id")) <> "" Then
        temp_id = Trim(Request("project_id"))
        Me.export_now.Visible = True
      Else
        Dim info As Array = Split(open_project_ddl.SelectedValue, "++++++")
        'export_title.Text = info(0) 
        temp_id = info(1)
      End If

      info_to_export.Items.Clear()
      file_open_dialog.Visible = True

      Better_Open_Project(temp_id)

    Catch ex As Exception
      error_string = "export_creator.aspx.vb - Open_Project_Btn_Click() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub

  Private Sub Better_Open_Project(ByVal id As Integer)

    Dim jetnet_model_id As Integer = 0
    Dim client_model_id As Integer = 0
    Dim temp_group As String = ""



    Me.default_model_export.Visible = True
    Me.default_label.Visible = True
    Me.warning_label.Visible = False

    file_id.Text = "0"

    info_to_export.Items.Clear()
    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Resize", "window.resizeTo(910,560);", True)
    'Let's fill in the open dialog boxes. 
    aTempTable = aclsData_Temp.Client_Project_Details_By_Project_ID(id)
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each q In aTempTable.Rows
          file_id.Text = id
          file_name.Text = CStr(IIf(Not IsDBNull(q("cliproj_name")), q("cliproj_name"), ""))
          file_description.Text = CStr(IIf(Not IsDBNull(q("cliproj_description")), q("cliproj_description"), ""))
          If Not IsDBNull(q("cliproj_shared")) Then
            file_shared.Checked = IIf((q("cliproj_shared") = "Y"), True, False)
          End If

          If Not IsDBNull(q("cliproj_market_default")) Then
            check_market_default.Checked = IIf((q("cliproj_market_default") = "Y"), True, False)
          End If

          file_delete.Visible = False
          If Not IsDBNull(q("cliproj_market_default")) Then
            If Trim(q("cliproj_user_id")) = Trim(Session.Item("localUser").crmLocalUserID) Then
              file_delete.Visible = True  ' if its u, u can delete 
            End If
          End If

          If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
            file_delete.Visible = True
          End If

          If CDbl(q("cliproj_jetnet_model")) <> 0 Then
            jetnet_model_id = CDbl(q("cliproj_jetnet_model"))
          End If

          If CDbl(q("cliproj_client_model")) <> 0 Then
            client_model_id = CDbl(q("cliproj_client_model"))
          End If

          If Trim(q("cliproj_model_default")) = "Y" Then
            Me.default_model_export.Checked = True
          Else
            Me.default_model_export.Checked = False
          End If


        Next
      End If
    End If

    aTempTable = aclsData_Temp.Client_Project_Reference_Details_By_Project_ID(id)
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each q As DataRow In aTempTable.Rows
          'Response.Write(q("clipref_exp_id") & "<br />")

          If Trim(q("clipref_source")) = "JETNET" Then
            aTempTable2 = aclsData_Temp.Build_Export_byID(q("clipref_exp_id"))
          Else
            aTempTable2 = aclsData_Temp.Build_Custom_Export_byID(q("clipref_exp_id"))
          End If

          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable2.Rows


                If Not IsDBNull(r("cliexp_sub_group")) Then
                  If Trim(r("cliexp_sub_group")) <> "" Then
                    temp_group = r("cliexp_sub_group") & " - "
                  ElseIf Trim(r("cliexp_type")) = "Feature Code" Then ' if its a client feature code 
                    temp_group = "Features - "
                  ElseIf Trim(q("clipref_source")) <> "JETNET" Then
                    If Not IsDBNull(r("cliexp_type")) Then
                      temp_group = r("cliexp_type") & " - "
                    Else
                      temp_group = ""
                    End If
                  Else
                    temp_group = ""
                  End If
                ElseIf Trim(r("cliexp_sub_group")) = "Feature Code" Then ' if its a client feature code 
                  temp_group = "Features - "
                Else
                  temp_group = ""
                End If

                ' info_to_export.Items.Add(New ListItem(r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name")))
                info_to_export.Items.Add(New ListItem(temp_group & r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name") & "|" & r("cliexp_type") & "|" & q("clipref_source") & "|" & r("cliexp_header_field_name")))
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("export_creator.aspx.vb - Open_Project_Btn_Click() - " & error_string)
            End If
            display_error()
          End If
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("export_creator.aspx.vb - Open_Project_Btn_Click() - " & error_string)
      End If
      display_error()
    End If


    Call make_model_list_dropdowns(jetnet_model_id, client_model_id)


  End Sub
  Public Sub make_model_list_dropdowns(ByVal jetnet_model_id As Integer, ByVal client_model_id As Integer)


    Dim jetnet_model_string As String = ""
    Dim jetnet_data As New DataTable
    Dim client_data As New DataTable
    Dim final_Table2 As New DataTable
    Dim icolumn As New DataColumn 'Column to Add Source to jetnet data.
    Dim icolumn2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
    Dim icolumn3 As New DataColumn 'Column to add take price to jetnet data (null)
    Dim icolumn4 As New DataColumn
    Dim icolumn5 As New DataColumn 


    client_data = aclsData_Temp.Get_Client_Models_For_Drop(jetnet_model_string, client_model_id)
    jetnet_data = aclsData_Temp.Get_JETNET_Models_For_Drop(jetnet_model_string, jetnet_model_id, False)

    Me.model_list.Items.Clear()
    Me.model_list_source.Items.Clear()
    Me.model_list.Items.Add(New System.Web.UI.WebControls.ListItem("Not Model Based", 0))
    Me.model_list_source.Items.Add(New System.Web.UI.WebControls.ListItem("Not Model Based", 0))

    final_table.Clear()
    final_table.Columns.Clear()
    icolumn.DataType = System.Type.GetType("System.String")
    icolumn.DefaultValue = 0
    icolumn.Unique = False
    icolumn.ColumnName = "amod_make_name"
    final_table.Columns.Add(icolumn)

    icolumn2.DataType = System.Type.GetType("System.String")
    icolumn2.DefaultValue = 0
    icolumn2.Unique = False
    icolumn2.ColumnName = "amod_model_name"
    final_table.Columns.Add(icolumn2)

    icolumn3.DataType = System.Type.GetType("System.Int64")
    icolumn3.DefaultValue = 0
    icolumn3.AllowDBNull = True
    icolumn3.Unique = False
    icolumn3.ColumnName = "cliamod_id"
    final_table.Columns.Add(icolumn3)

    icolumn4.DataType = System.Type.GetType("System.Int64")
    icolumn4.DefaultValue = 0
    icolumn4.AllowDBNull = True
    icolumn4.Unique = False
    icolumn4.ColumnName = "amod_id"
    final_table.Columns.Add(icolumn4)

    icolumn5.DataType = System.Type.GetType("System.String")
    icolumn5.AllowDBNull = True
    icolumn5.Unique = False
    icolumn5.ColumnName = "source"
    final_table.Columns.Add(icolumn5)

    final_Table2 = final_table.Clone

    If Not IsNothing(client_data) Then
      For Each drRow As DataRow In client_data.Rows
        final_table.ImportRow(drRow)
      Next
    End If

    If Not IsNothing(jetnet_data) Then
      For Each drRow As DataRow In jetnet_data.Rows
        final_table.ImportRow(drRow)
      Next
    End If


    Dim afiltered_BOTH As DataRow() = final_table.Select("", "amod_make_name asc, amod_model_name asc")
    ' extract and import

    For Each atmpDataRow_JETNET In afiltered_BOTH
      final_Table2.ImportRow(atmpDataRow_JETNET)
    Next


    For Each r As DataRow In final_Table2.Rows
      If Trim(r("source")) = "client" Then
        model_list.Items.Add(New System.Web.UI.WebControls.ListItem((r("amod_make_name") & " " & r("amod_model_name")), CInt(r("cliamod_id") & "000")))
        model_list_source.Items.Add(New System.Web.UI.WebControls.ListItem(r("source"), 0))
        If CDbl(client_model_id) = CDbl(r("cliamod_id")) Then
          model_list.SelectedIndex = model_list.Items.Count - 1
          model_list_source.SelectedIndex = model_list.Items.Count - 1
        End If
      Else
        model_list.Items.Add(New System.Web.UI.WebControls.ListItem((r("amod_make_name") & " " & r("amod_model_name")), r("amod_id")))
        model_list_source.Items.Add(New System.Web.UI.WebControls.ListItem(r("source"), 0))
        If CDbl(jetnet_model_id) = CDbl(r("amod_id")) Then
          model_list.SelectedIndex = model_list.Items.Count - 1
          model_list_source.SelectedIndex = model_list.Items.Count - 1
        End If
      End If

    Next



  End Sub
  Private Sub load_export_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles load_export.Click

    Call LOAD_Export_function() 

  End Sub
  Public Sub LOAD_Export_function()
    'open
    attention.Text = ""
    'file_save.ImageUrl = "images/update.gif"
    export_info_box.Visible = False
    selected_export_template.Text = "Selected Export Template:"
    file_open_dialog.Visible = False
    file_save_as.Visible = True
    open_project.Visible = True
    open_project_ddl.Items.Clear()

    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Resize", "window.resizeTo(770,660);", True)
    'Projects for this person.
    ' Fill_Open_Box()
    open_project_ddl.Items.Clear()
    aclsData_Temp.Fill_Open_Box(open_project_ddl, Session.Item("localUser").crmLocalUserID, Session.Item("Listing"), 0, 0, "")

  End Sub

  Private Sub create_export_template_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles create_export_template.Click

    Call create_click()
    
  End Sub
  Public Sub create_click()
    Dim jetnet_model_string As String = ""

    'First thing make sure there's stuff to save
    If info_to_export.Items.Count <> 0 Then
      file_id.Text = "0"
      selected_export_template.Text = "Create Export Template:"
      export_info_box.Visible = False
      'Make boxes disabled.
      'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Resize", "window.resizeTo(910,560);", True)
      'open_project.Visible = False
      'choice_to_export.Items.Clear()
      'choice_to_export.Enabled = False
      'type_of_info.Enabled = False
      'Button1.Enabled = False
      'Button2.Enabled = False
      'Button3.Enabled = False
      'Button4.Enabled = False
      'Make file save dialog show up.
      file_open_dialog.Visible = True
      file_name.Text = ""
      file_description.Text = ""
      file_shared.Checked = False
      file_save_as.Visible = False
      open_project.Visible = False

      Call make_model_list_dropdowns(0, 0)
    Else
      open_project.Visible = False
      attention.Text = "<p align='center' valign='top'>Please select fields to save first.</p>"
    End If
  End Sub
  Private Sub cancel_file_save_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cancel_file_save.Click
    file_open_dialog.Visible = False
    info_to_export.Items.Clear()
    attention.Text = ""
    export_info_box.Visible = True
  End Sub

  Private Sub file_delete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles file_delete.Click

    If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
      aclsData_Temp.Client_Project_Delete(file_id.Text, 0) 
      aclsData_Temp.Client_Project_Reference_Delete(file_id.Text) 
    Else
      ' if not admin, try to delete the project
      aclsData_Temp.Client_Project_Delete(file_id.Text, CStr(Session.Item("localUser").crmLocalUserID))
 
      'check to make sure it has been deleted
      aTempTable = aclsData_Temp.Client_Project_EXISTS_By_Project_ID(file_id.Text)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          ' then dotn delete
        Else
          aclsData_Temp.Client_Project_Reference_Delete(file_id.Text)
        End If
      Else
        aclsData_Temp.Client_Project_Reference_Delete(file_id.Text)
      End If 
    End If



    file_open_dialog.Visible = False
    open_project.Visible = False
    export_now.Visible = False
    export_label.Visible = False
    load_export_label.Visible = False
    info_to_export.Items.Clear()
    add()
    export_info_box.Visible = True
    attention.Text = "<p align='center'>Your export template has been removed.</p>"
    Show_Load_Export()
  End Sub



  Private Sub type_of_info_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type_of_info.SelectedIndexChanged
    'info_to_export.Items.Clear()
    add()
    'VerifyNoBadFields()
  End Sub


  Public Sub get_fields_for_add_comparable(ByVal NOTE_ID As Long, ByRef drop_list As ListBox)

    Dim Query As String = ""
    Dim results_table As New DataTable
    Dim temp_name As String = ""
    Dim temp_val As String = ""
    Dim found_spot As Boolean = False
    Dim selected_fields As String = ""
    Dim i As Integer = 0

    Query = " SELECT * "
    Query &= " from client_preference "
    Query &= " where clipref_ac_custom_1_use = 'Y' "

    results_table = localDatalayer.Get_Compare_Query(Query, "get_fields_for_add_comparable")

    If Not IsNothing(results_table) Then

      If results_table.Rows.Count > 0 Then

        For Each r As DataRow In results_table.Rows

          For i = 1 To 10
            If Not IsDBNull(r("clipref_ac_custom_" & i & "_use")) Then
              If Trim(r("clipref_ac_custom_" & i & "_use")) = "Y" Then
                If Not IsDBNull(r("clipref_ac_custom_" & i & "")) Then
                  If Trim(r("clipref_ac_custom_" & i & "")) <> "" Then
                    If Trim(selected_fields) <> "" Then
                      selected_fields &= ","
                    End If
                    selected_fields &= "'" & r("clipref_ac_custom_" & i & "") & "'"
                  End If
                End If
              End If
            End If
          Next

        Next

      End If

    End If

    Query = " SELECT clivalch_id, clivalch_order, clivalch_name, clivalch_db_name, clivalch_trans_db_name, clivalch_closed_db_name,  "
    Query &= " clivalch_description " '
    Query &= "  from client_value_field_choice "
    Query &= " where clivalch_name in (" & Trim(selected_fields) & ") "
    Query &= " and clivalch_db_name like 'cliaircraft_custom_%' "
    Query &= " order by clivalch_order"

    results_table = localDatalayer.Get_Compare_Query(Query, "get_fields_for_add_comparable")

    If Not IsNothing(results_table) Then

      If results_table.Rows.Count > 0 Then


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

          drop_list.Items.Add(New ListItem(temp_name, r("clivalch_id") & "| if(" & temp_val & " IS NULL ,' '," & temp_val & " )  as '" & temp_name & "' |' ' as '" & temp_name & "'|" & temp_name))

        Next


        If found_spot = False Then
          drop_list.SelectedIndex = 0
        End If

      End If
    End If
  End Sub
End Class