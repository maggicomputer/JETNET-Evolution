' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Attributes.aspx.vb $
'$$Author: Mike $
'$$Date: 6/15/20 9:11p $
'$$Modtime: 6/15/20 8:31p $
'$$Revision: 14 $
'$$Workfile: Attributes.aspx.vb $
'
' ********************************************************************************

Partial Public Class Attributes

  Inherits System.Web.UI.Page

  Public Shared masterPage As New Object

  Private Sub Attributes_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
      Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, HomebaseTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
      masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Page.Title = "Attribute Management"

    If Not Page.IsPostBack Then
      close_window_only.Text += ("<a class='underline cursor' onclick=""javascript:window.close();return false;"" class=""close_button"" style=""padding-right:15px;""><img src='images/x.svg' alt='Close' /></a>")
    End If

    Dim temp_id As Long = 0
    Dim rule_id As Long = 0


    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Attribute Management")

    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then

      masterPage.Set_Active_Tab(5)
      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase Edit Attributes - Home")
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Attribute Management")

      masterPage.SetPageTitle("Attribute Management")
    End If

    If Not IsPostBack Then

      If Not IsNothing(Trim(Request("add"))) Then
        If Trim(Request("add")) = "true" Then
          add_attention.Text = "<p>Your attribute has been added.</p>"
        End If
      End If

      If Not IsNothing(Trim(Request("update"))) Then
        If Trim(Request("update")) = "true" Then
          add_attention.Text = "<p>Your attribute has been updated.</p>"
        End If
      End If
      If Not IsNothing(Trim(Request("asset"))) Then
        If Trim(Request("asset")) = "true" Then
          add_attention.Text = "<p>Your attribute has been linked.</p>"
          viewStatus.SelectedValue = "AS"
        End If
      End If
      If Trim(Request("rule_id")) <> "" Then

        Me.attributes_panel.TabIndex = 2
        edit_rules_panel.Visible = True
        attributes_tab.ActiveTab = edit_rules_panel

        rule_id = Trim(Request("rule_id"))

        fillUpAreaBlock("")

        Me.rule_operator.Items.Add("AND")
        Me.rule_operator.Items.Add("OR")
        Call fill_action_options(rule_action_drop)


        get_rule_data_top(rule_id)

      ElseIf IsNumeric(selectedAsset.Text) Then
        displaySelectedAsset(selectedAsset.Text)

      ElseIf IsNumeric(selectedAttribute.Text) Then
        If selectedAttribute.Text > 0 Then
          Me.attributes_panel.TabIndex = 1
          attributes_tab.ActiveTab = edit_tab

          fillUpAreaBlock("")

          Me.related_attributes.Text = get_related_attributes_function(temp_id, "S")
          Me.components_label.Text = get_related_attributes_function(temp_id, "C")
          '   Me.rules_label.Text = get_rules_function(temp_id)


          get_attributes_data_top(temp_id)

        End If
      Else
        Me.mainMenuAdd.Text = CreateTreeMenu()
      End If



    End If



  End Sub
  Private Sub displaySelectedAsset(ByVal assetID As Long)
    Try


      Dim attributesTable As New DataTable
      attributesTable = getLinkableAttributes()
      linkedAttribute.Items.Add(New ListItem("NONE", ""))
      If Not IsNothing(attributesTable) Then
        If attributesTable.Rows.Count > 0 Then
          For Each r As DataRow In attributesTable.Rows
            linkedAttribute.Items.Add(New ListItem(r("acatt_name"), r("acatt_id")))
          Next
        End If
      End If

      Dim displayTable As New DataTable
      displayTable = getAssetAttributes(0, assetID)
      If Not IsNothing(displayTable) Then
        If displayTable.Rows.Count > 0 Then
          If Not IsDBNull(displayTable.Rows(0).Item("ASSETNAME")) Then
            asset_Name.Text = displayTable.Rows(0).Item("ASSETNAME")
          End If
          If Not IsDBNull(displayTable.Rows(0).Item("acatt_id")) Then
            linkedAttribute.SelectedValue = displayTable.Rows(0).Item("acatt_id")
          End If

        End If
      End If
    Catch ex As Exception
      Response.Write(ex.Message)
    End Try
  End Sub

  Private Sub editUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editUpdateButton.Click
    ShowAttributeEditForm()
    editTabUpdate.Update()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("swapAttribute") Then '
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.editTabUpdate, Me.GetType, "swapAttribute", "$find('" & edit_tab.ClientID & "')._show();var tabTopL = $find('" & attributes_tab.ClientID & "');tabTopL.set_activeTabIndex(1);tabTopL.get_tabs()[1]._header.innerHTML = 'Edit Attribute';ChangeTheMouseCursorOnItemParentDocument('cursor_default standalone_page');", True)
    End If

  End Sub

  Private Sub editAssetUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editAssetUpdateButton.Click
    displaySelectedAsset(selectedAsset.Text)

    If Not Page.ClientScript.IsClientScriptBlockRegistered("swapAsset") Then '
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.asset_update_panel, Me.GetType, "swapAsset", "$find('" & asset_edit_panel.ClientID & "')._show();var tabTopL = $find('" & attributes_tab.ClientID & "');tabTopL.set_activeTabIndex(2);tabTopL.get_tabs()[2]._header.innerHTML = 'Edit Asset';ChangeTheMouseCursorOnItemParentDocument('cursor_default standalone_page');", True)
    End If

  End Sub

  Private Sub ShowAttributeEditForm()
    If IsNumeric(selectedAttribute.Text) Then
      Dim temp_id As Long = 0
      Dim rule_id As Long = 0
      edit_tab.HeaderText = "Edit Attribute"
      'Me.attributes_panel.TabIndex = 1
      attributes_tab.ActiveTab = edit_tab
      bottom_tab_container.ActiveTabIndex = 0

      temp_id = IIf(Not String.IsNullOrEmpty(selectedAttribute.Text.Trim), selectedAttribute.Text.Trim, "0")
      Temp_ID_New.Text = temp_id.ToString

      related_attributes.Text = get_related_attributes_function(temp_id, "S")
      components_label.Text = get_related_attributes_function(temp_id, "C")

      model_relationships_label.Text = getModelRelationships(temp_id)

      getAssetInsight(temp_id)
      getModelWithAttributesTab(temp_id)

      Dim SynonymTable As New DataTable
      SynonymTable = getSynonymList()

      ' ADDED IN TO TAB_8 MSW - 2/28/20 
      synonyms_label.Text = get_synonyms_function(temp_id)

      If Not IsNothing(SynonymTable) Then
        If SynonymTable.Rows.Count > 0 Then

          clsGeneral.clsGeneral.Populate_Dropdown(SynonymTable, synonym_id, "acatt_name", "acatt_id", False)
          synonym_id.Items.FindByText("All").Value = "0"
          synonym_id.Items.FindByText("All").Text = ""
          synonym_id.Items.Remove(synonym_id.Items.FindByValue(temp_id))
        End If
      End If

      get_attributes_data_top(temp_id)
    End If

  End Sub
  Protected Sub getAssetInsight(ByVal attID As Long)
    Dim returnTable As New DataTable
    Dim returnString As String = ""
    asset_attributes.Text = returnString
    returnTable = getAssetAttributes(attID, 0)


    returnString = "<table width=""100%"" id=""assetAttributeData"">"

    returnString += "<thead><tr class=""noBorder"">"
    returnString += "<th align=""left"" valign=""top"" width=""225""><span class=""subHeader"">NAME</span></th>"
    returnString += "<th align=""left"" valign=""top""><span class=""subHeader"">ASSET NAME</span></th>"
    returnString += "</tr></thead><tbody>"
    If Not IsNothing(returnTable) Then
      If returnTable.Rows.Count > 0 Then
        For Each r As DataRow In returnTable.Rows

          returnString += "<tr>"
          returnString += "<td align=""left"" valign=""top"">"
          If Not IsDBNull(r("ATTNAME")) Then
            returnString += r("ATTNAME")
          End If
          returnString += "</td>"
          returnString += "<td align=""left"" valign=""top"">"
          If Not IsDBNull(r("ASSETNAME")) Then
            returnString += r("ASSETNAME")
          End If


          'returnString += "</td>"
          'returnString += "<td align=""left"" valign=""top"">"
          'If Not IsDBNull(r("JETNETQUERY")) Then
          '  returnString += r("JETNETQUERY").ToString
          'End If
          'returnString += "</td>"
          'returnString += "<td align=""left"" valign=""top"">"
          'If Not IsDBNull(r("AIRCRAFT")) Then
          '  returnString += r("AIRCRAFT").ToString
          'End If
          'returnString += "</td>"

          returnString += "</tr>"
        Next

      End If
    End If


    returnString += "</tbody></table>"

    asset_attributes.Text = returnString
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "assetAttributeData", "$(document).ready( function () {$('#assetAttributeData').DataTable();} );", True)

  End Sub
  Protected Sub getModelWithAttributesTab(ByVal attID As Long)
    Dim modelsTable As New DataTable
    modelsTable = getModelWithAttributes(attID)
    If Not IsNothing(modelsTable) Then
      If modelsTable.Rows.Count > 0 Then
        'sQuery.Append("select amod_id as MODID, amod_make_name as MAKENAME, amod_model_name as MODELNAME, ")
        'sQuery.Append(" case when sum(case when attmod_standard_equip = 'Y' then 1 else 0 end) > 0 then 'Y' else '-' end as STANDARD, ")
        'sQuery.Append(" case when sum(case when attmod_standard_equip = 'N' then 1 else 0 end) > 0 then 'Y' else '-' end as MAPPED, ")
        'sQuery.Append(" count(distinct ac_id) as NUMAIRCRAFT, ")
        'sQuery.Append(" SUM( case when acattind_status_flag = 'Y' then 1 else 0 end ) as HAVE, ")
        'sQuery.Append(" SUM( case when acattind_status_flag = 'N' then 1 else 0 end ) as DONOTHAVE, ")
        'sQuery.Append(" SUM( case when acattind_status_flag = 'U' then 1 else 0 end ) as UNKNOWN ")
        models_with_attributes_label.Text = "<table width=""100%"" id=""modelsWithAttributesData"">"
        models_with_attributes_label.Text += "<thead><tr>"
        models_with_attributes_label.Text += "<th>MAKE</th>"
        models_with_attributes_label.Text += "<th>MODEL</th>"
        models_with_attributes_label.Text += "<th>STANDARD</th>"
        models_with_attributes_label.Text += "<th>MAPPED</th>"
        models_with_attributes_label.Text += "<th>NUMAIRCRAFT</th>"
        models_with_attributes_label.Text += "<th>HAVE</th>"
        models_with_attributes_label.Text += "<th>DO NOT HAVE</th>"
        models_with_attributes_label.Text += "<th>UNKNOWN</th>"
        models_with_attributes_label.Text += "</tr></thead><tbody>"
        For Each r As DataRow In modelsTable.Rows
          models_with_attributes_label.Text += "<tr>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("MAKENAME")) Then
            models_with_attributes_label.Text += r("MAKENAME").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("MODELNAME")) Then
            models_with_attributes_label.Text += r("MODELNAME").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("STANDARD")) Then
            models_with_attributes_label.Text += r("STANDARD").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("MAPPED")) Then
            models_with_attributes_label.Text += r("MAPPED").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("NUMAIRCRAFT")) Then
            models_with_attributes_label.Text += r("NUMAIRCRAFT").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("HAVE")) Then
            models_with_attributes_label.Text += r("HAVE").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("DONOTHAVE")) Then
            models_with_attributes_label.Text += r("DONOTHAVE").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "<td>"
          If Not IsDBNull(r("UNKNOWN")) Then
            models_with_attributes_label.Text += r("UNKNOWN").ToString
          End If
          models_with_attributes_label.Text += "</td>"
          models_with_attributes_label.Text += "</tr>"
        Next
        models_with_attributes_label.Text += "</tbody></table>"

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "modeslWithAttributesTab", "$(document).ready( function () {$('#modelsWithAttributesData').DataTable();} );", True)

      End If
    End If
  End Sub
  Protected Sub add_new_attribute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles add_new_attribute.Click
    selectedAttribute.Text = 0
    fillUpAreaBlock("")
    'rules_label.Text = get_rules_function(0)
    area_drop.Text = ""
    block_drop.Text = ""
    name_text.Text = ""
    acatt_average.Text = "0"
    acatt_high.Text = "0"
    acatt_low.Text = "0"
    synonym_id.SelectedValue = 0
    last_action_date.Text = ""
    last_refresh_date.Text = ""
    description.Text = ""
    acatt_count.Text = ""
    business_check.Checked = True
    commercial_check.Checked = True
    heli_check.Checked = True
    aerodex_check.Checked = False
    model_dependent.Checked = False
    acatt_glossary.Checked = False
    howToFindRule.Text = ""
    attention_label.Text = ""
    code_text.Text = ""

    queryRule.Text = ""
    autoGenerateRule.Checked = False
    edit_tab.HeaderText = "Add Attribute"
    editTabUpdate.Update()
    If Not Page.ClientScript.IsClientScriptBlockRegistered("swapAttribute") Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.editTabUpdate, Me.GetType, "swapAttribute", "$('#" & selectedAttribute.ClientID & "').val('0');$('#" & tab_2.ClientID & "_tab').addClass('display_none');$('#" & attention_label.ClientID & "').html('');$('#" & tab_3.ClientID & "_tab').addClass('display_none');$('#" & tab_4.ClientID & "_tab').addClass('display_none');$find('" & edit_tab.ClientID & "')._show();var tabTopL = $find('" & attributes_tab.ClientID & "');tabTopL.get_tabs()[1]._header.innerHTML = 'Add Attribute';tabTopL.set_activeTabIndex(1);ChangeTheMouseCursorOnItemParentDocument('cursor_default standalone_page');", True)
      EditUpdate.Update()
    End If

  End Sub
  Protected Sub add_rule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles add_rule.Click

    ' Response.Redirect("attributes.aspx?rule_id=0&id=" & real_id.Text & "")

  End Sub

  Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cancel_button.Click

    attributes_tab.TabIndex = 0

  End Sub

  Protected Sub submit_button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles submit_button.Click

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim update_string As String = ""
    Dim temp_id As Long = 0


    Try

      If IsNumeric(selectedAttribute.Text) Then
        If selectedAttribute.Text > 0 Then
          temp_id = selectedAttribute.Text

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
            update_string = " Update [Homebase].jetnet_ra.dbo.aircraft_attribute set  "
          Else
            update_string = " Update aircraft_attribute set  "
          End If


          If Me.acatt_glossary.Checked = True Then
            update_string &= " acatt_glossary = 'Y', "
          Else
            update_string &= " acatt_glossary = 'N', "
          End If

          update_string &= " acatt_low_value = @acatt_low_value, "
          update_string &= " acatt_average_value = @acatt_average_value, "
          update_string &= " acatt_high_value = @acatt_high_value, "

          update_string &= " acatt_status = @acatt_status, "
          update_string &= " acatt_area = @acatt_area, "
          update_string &= " acatt_action_date = @acatt_action_date "
          update_string &= ", acatt_block = @acatt_block "
          update_string &= ", acatt_name = @acatt_name "
          update_string &= ", acatt_query = @acatt_query "
          update_string &= ", acatt_howtofind = @acatt_howtofind "
          update_string &= ", acatt_auto_generate = @acatt_auto_generate "
          update_string &= ", acatt_description = @acatt_description, acatt_abbrev = @acatt_abbrev, acatt_synonym_id = @acatt_synonym_id "

          If Me.business_check.Checked = True Then
            update_string &= ", acatt_product_business_flag = 'Y' "
          Else
            update_string &= ", acatt_product_business_flag = 'N' "
          End If

          If Me.commercial_check.Checked = True Then
            update_string &= ", acatt_product_commercial_flag = 'Y' "
          Else
            update_string &= ", acatt_product_commercial_flag = 'N' "
          End If

          If Me.heli_check.Checked = True Then
            update_string &= ", acatt_product_helicopter_flag = 'Y' "
          Else
            update_string &= ", acatt_product_helicopter_flag = 'N' "
          End If

          If Me.aerodex_check.Checked = True Then
            update_string &= ", acatt_aerodex_flag = 'Y' "
          Else
            update_string &= ", acatt_aerodex_flag = 'N' "
          End If

          If Me.model_dependent.Checked = True Then
            update_string &= ", acatt_model_dependent_flag = 'Y' "
          Else
            update_string &= ", acatt_model_dependent_flag = 'N' "
          End If

          update_string &= ", acatt_definition_url = @acatt_definition_url "


          update_string &= " where acatt_id = " & temp_id.ToString

        ElseIf selectedAttribute.Text = 0 Then

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
            update_string = " Insert into [Homebase].jetnet_ra.dbo.aircraft_attribute "
          Else
            update_string = " Insert into aircraft_attribute "
          End If


          update_string &= " (acatt_low_value, acatt_average_value, acatt_high_value, acatt_glossary, acatt_status, acatt_area, acatt_action_date, acatt_block, acatt_name, acatt_description, acatt_abbrev, acatt_synonym_id "
          update_string &= ", acatt_product_business_flag, acatt_product_commercial_flag, acatt_product_helicopter_flag, acatt_aerodex_flag, acatt_model_dependent_flag "
          update_string &= " , acatt_query, acatt_howtofind, acatt_auto_generate, acatt_definition_url) VALUES (@acatt_low_value, @acatt_average_value, @acatt_high_value,"

          If Me.acatt_glossary.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          update_string &= "@acatt_status, "
          update_string &= " @acatt_area, "
          update_string &= " @acatt_action_date, "
          update_string &= " @acatt_block, "
          update_string &= " @acatt_name, "
          update_string &= " @acatt_description, "
          update_string &= " @acatt_abbrev, "
          update_string &= " @acatt_synonym_id, "

          If Me.business_check.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          If Me.commercial_check.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          If Me.heli_check.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          If Me.aerodex_check.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          If Me.model_dependent.Checked = True Then
            update_string &= "'Y', "
          Else
            update_string &= "'N', "
          End If

          update_string &= "@acatt_query, @acatt_howtofind, @acatt_auto_generate, @acatt_definition_url )"
        End If
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()


      Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)

      SqlCommand.Parameters.AddWithValue("acatt_low_value", acatt_low.Text)
      SqlCommand.Parameters.AddWithValue("acatt_average_value", acatt_average.Text)
      SqlCommand.Parameters.AddWithValue("acatt_high_value", acatt_high.Text)

      SqlCommand.Parameters.AddWithValue("acatt_status", acatt_status.SelectedValue)
      SqlCommand.Parameters.AddWithValue("acatt_area", area_drop.SelectedValue)
      SqlCommand.Parameters.AddWithValue("acatt_action_date", FormatDateTime(Now(), vbGeneralDate))
      SqlCommand.Parameters.AddWithValue("acatt_block", block_drop.SelectedValue)
      SqlCommand.Parameters.AddWithValue("acatt_name", name_text.Text)
      SqlCommand.Parameters.AddWithValue("acatt_query", queryRule.Text)
      SqlCommand.Parameters.AddWithValue("acatt_howtofind", howToFindRule.Text)
      SqlCommand.Parameters.AddWithValue("acatt_auto_generate", IIf(autoGenerateRule.Checked, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("acatt_description", description.Text)
      SqlCommand.Parameters.AddWithValue("acatt_abbrev", code_text.Text)
      SqlCommand.Parameters.AddWithValue("acatt_synonym_id", synonym_id.SelectedValue)
      SqlCommand.Parameters.AddWithValue("acatt_definition_url", def_url.Text)

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, update_string.ToString)
      SqlCommand.ExecuteNonQuery()


      SqlCommand.Dispose()
      SqlCommand = Nothing

      '  If selectedAttribute.Text = 0 Then
      Call Table_Action_log("aircraft_attribute") ' always insert  
      ' Else
      '  If Trim(queryRule.ToolTip) <> Trim(queryRule.Text) Then
      '  Call Table_Action_log("aircraft_attribute")
      '  End If
      '  End If




    Catch ex As Exception
    Finally
      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      If selectedAttribute.Text > 0 Then
        Response.Redirect("attributes.aspx?update=true")
      Else
        Response.Redirect("attributes.aspx?add=true")
      End If

    End Try



  End Sub
  Public Sub Table_Action_log(ByVal table_name As String)

    If check_action_log(table_name) = False Then
      Call insert_into_Table_Action_Log(table_name)
    End If

  End Sub

  Public Sub insert_into_Table_Action_Log(ByVal Passed_Table_Name As String)

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection

    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()


      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" Insert Into  [Homebase].jetnet_ra.dbo.Table_Action_Log (tact_table_name,tact_action_date) ")
      Else
        sQuery.Append(" Insert Into  Table_Action_Log (tact_table_name,tact_action_date) ")
      End If



      sQuery.Append("  VALUES ('" & Passed_Table_Name & "',NULL) ")

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      SqlCommand.ExecuteNonQuery()


    Catch ex As Exception
      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message 
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try
  End Sub
  Public Function check_action_log(ByVal Passed_Table_Name As String) As Boolean
    check_action_log = False

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection

    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing


    Dim sQuery = New StringBuilder()

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sQuery.Append(" SELECT tact_table_name")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append("  FROM [Homebase].jetnet_ra.dbo.Table_Action_Log ")
      Else
        sQuery.Append("  FROM Table_Action_Log ")
      End If

      sQuery.Append("  WHERE tact_table_name = '" & Passed_Table_Name & "'")
      sQuery.Append(" AND tact_action_date IS NULL ")

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      If Not IsNothing(temptable) Then
        If temptable.Rows.Count > 0 Then
          check_action_log = True
        End If
      End If

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try


  End Function

  Sub createAlphaTreeMenu(ByRef ReturnString As String)
    Dim returnTable As New DataTable
    Dim htmlout As New StringBuilder
    htmlout.Append("<table width=""100%"" id=""alphabetTree"" cellpadding=""3"" cellspacing=""0"">")
    returnTable = getAlphaTopics()

    htmlout.Append("<thead><tr class=""noBorder"">")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">NAME</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">ABBR</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">DEPEND</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">KEY</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">GLOSS.</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">SYNM.</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">DESC</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">AREA</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">BLOCK</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">AUTO</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">#</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">VALUE</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">REFRESH</span></th>")
    htmlout.Append("</tr></thead><tbody>")

    If Not IsNothing(returnTable) Then
      If returnTable.Rows.Count > 0 Then
        For Each r As DataRow In returnTable.Rows

          htmlout.Append("<tr>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("NAME")) Then
            htmlout.Append(attributeClickButton(r("acatt_id"), r("NAME")))
          End If
          htmlout.Append("&nbsp;</td>")


          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("acatt_abbrev")) Then
            htmlout.Append(r("acatt_abbrev"))
          End If
          htmlout.Append("&nbsp;</td>")

          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("ONMODELSONLY")) Then
            If r("ONMODELSONLY") = "YES" Then
              htmlout.Append("MODELS")
            End If
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("MODELSMAPPED")) Then
            htmlout.Append(r("MODELSMAPPED").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left""  valign=""top"">")
          If Not IsDBNull(r("GLOSSARY")) Then
            htmlout.Append(r("GLOSSARY").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("SYNONYM")) Then
            htmlout.Append(r("SYNONYM").ToString)
          End If
          htmlout.Append("&nbsp;</td>")

          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("DESCRIPTION")) Then
            htmlout.Append(r("DESCRIPTION").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("AREA")) Then
            htmlout.Append(r("AREA").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("BLOCK")) Then
            htmlout.Append(r("BLOCK").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("acatt_count")) Then
            htmlout.Append(r("acatt_count").ToString)
          End If
          htmlout.Append("&nbsp;</td>")


          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("acatt_auto_generate")) Then
            htmlout.Append(r("acatt_auto_generate"))
          End If
          htmlout.Append("&nbsp;</td>")


          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("acatt_average_value")) Then
            htmlout.Append(r("acatt_average_value").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("acatt_refresh_date")) Then
            htmlout.Append(r("acatt_refresh_date").ToString)
          End If
          htmlout.Append("&nbsp;</td>")
          htmlout.Append("</tr>")
        Next
      End If
    End If

    htmlout.Append("</tbody></table>")

    ReturnString = htmlout.ToString
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alphabetTreeTab", "$('#alphabetTree').DataTable({""paging"":false}); ", True)

  End Sub


  Sub createAssetInsight(ByRef ReturnString As String)

    Dim returnTable As New DataTable
    Dim htmlout As New StringBuilder

    htmlout.Append("<table width=""100%"" id=""AssetTree"" cellpadding=""3"" cellspacing=""0"">")
    returnTable = getAssetAttributes(0, 0)

    htmlout.Append("<thead><tr class=""noBorder"">")
    htmlout.Append("<th align=""left"" valign=""top"" width=""225""><span class=""subHeader"">NAME</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">ASSET NAME</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">JETNETQUERY</span></th>")
    htmlout.Append("<th align=""left"" valign=""top""><span class=""subHeader"">#</span></th>")
    htmlout.Append("</tr></thead><tbody>")
    If Not IsNothing(returnTable) Then
      If returnTable.Rows.Count > 0 Then
        For Each r As DataRow In returnTable.Rows

          htmlout.Append("<tr>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("ATTNAME")) Then
            htmlout.Append(attributeClickButton(r("acatt_id"), r("ATTNAME")))
          End If
          htmlout.Append("</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("ASSETNAME")) Then
            htmlout.Append(assetClickButton(r("aimodif_item_id"), r("ASSETNAME")))
          End If

          htmlout.Append("</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("JETNETQUERY")) Then
            htmlout.Append(r("JETNETQUERY").ToString)
          End If
          htmlout.Append("</td>")
          htmlout.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("AIRCRAFT")) Then
            htmlout.Append(r("AIRCRAFT").ToString)
          End If
          htmlout.Append("</td>")

          htmlout.Append("</tr>")
        Next
      End If
    End If

    htmlout.Append("</tbody></table>")


    ReturnString = htmlout.ToString
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetTreeTab", "$('#AssetTree').DataTable({""paging"":false}); ", True)

  End Sub
  Function CreateTreeMenu() As String
    Dim returnString As String = ""
    Dim StartingArea As String = ""
    Dim ReturnTable As New DataTable
    Dim PreviousArea As String = ""
    Dim htmlOut As New StringBuilder

    mainMenuAdd.Visible = True
    If viewStatus.SelectedValue = "AS" Then
      createAssetInsight(returnString)
      htmlOut.Append(returnString)
    ElseIf viewStateShow.SelectedValue = "alpha" Or viewStateShow.SelectedValue = "glossary" Then
      createAlphaTreeMenu(returnString)
      htmlOut.Append(returnString)
    ElseIf viewStatus.SelectedValue <> "AS" Then

      If Not IsNothing(Trim(Request("area"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("area"))) Then
          StartingArea = Trim(Request("area"))
        End If
      End If
      htmlOut.Append("<ul class=""sortable list-unstyled"" id=""sortable"">")

      ReturnTable = getTopLevelTopics()

      If Not IsNothing(ReturnTable) Then


        If StartingArea <> "" Then

          htmlOut.Append("<li>")

          htmlOut.Append("<div class=""block block-title""><div class=""isFolder"">")
          htmlOut.Append("<A href='attributes.aspx?area=" & StartingArea & "'>")
          htmlOut.Append(StartingArea)
          htmlOut.Append("</a></div></div><ul class=""sortable list-unstyled"" id=""sortable"">")
        End If



        Dim Area_Distinct_table_view As New DataView
        Dim Area_Distinct_table As New DataTable

        Area_Distinct_table_view = ReturnTable.DefaultView
        Area_Distinct_table_view.Sort = "acatt_area"

        ''actually get the distinct values.
        Area_Distinct_table = Area_Distinct_table_view.ToTable(True, "acatt_area")

        For Each areaRow As DataRow In Area_Distinct_table.Rows

          htmlOut.Append("<li>")

          htmlOut.Append("<div class=""block block-title mainBackground""><div class=""isFolder""><strong class=""emphasisColor"">")
          ' returnString += "<A href='attributes.aspx?area=" & areaRow("acatt_area") & "'>"
          htmlOut.Append(UCase(areaRow("acatt_area")))
          'returnString += "</a>"
          htmlOut.Append("</strong></div></div>")

          'Dim BlockTable As New DataTable
          'BlockTable = getSecondLevelTopics(areaRow("acatt_area"))

          Dim Block_Distinct_table_view As New DataView
          Dim Block_Distinct_table As New DataTable

          Block_Distinct_table_view = ReturnTable.DefaultView
          Block_Distinct_table_view.Sort = "acatt_block"
          Block_Distinct_table_view.RowFilter = "acatt_area = '" & areaRow("acatt_area") & "'"

          ''actually get the distinct values.
          Block_Distinct_table = Area_Distinct_table_view.ToTable(True, "acatt_block")

          If Not IsNothing(Block_Distinct_table) Then
            If Block_Distinct_table.Rows.Count > 0 Then
              htmlOut.Append("<ul class=""sortable list-unstyled"">")
              For Each blockRow As DataRow In Block_Distinct_table.Rows
                htmlOut.Append("<div class=""block block-title""><div class=""isFolder""><strong>")
                htmlOut.Append(UCase(blockRow("acatt_block")))
                htmlOut.Append("</strong></div></div>")

                Dim Name_Distinct_table_view As New DataView
                Dim Name_Distinct_table As New DataTable

                Name_Distinct_table_view = ReturnTable.DefaultView
                Name_Distinct_table_view.Sort = "acatt_block"
                Name_Distinct_table_view.RowFilter = "acatt_area = '" & areaRow("acatt_area") & "' and acatt_block = '" & blockRow("acatt_block") & "'"

                ''actually get the distinct values.
                Name_Distinct_table = Name_Distinct_table_view.ToTable(True, "acatt_name", "acatt_id", "itemSubCount")
                If Not IsNothing(Name_Distinct_table) Then
                  If Name_Distinct_table.Rows.Count > 0 Then
                    For Each nameRow As DataRow In Name_Distinct_table.Rows
                      htmlOut.Append("<ul class=""sortable list-unstyled"">")
                      htmlOut.Append("<div class=""block block-title""><div class=""notFolder""><span></span>")
                      htmlOut.Append(attributeClickButton(nameRow("acatt_id"), nameRow("acatt_name")))

                      htmlOut.Append("</div></div>")

                      If nameRow("itemSubCount") > 0 Then
                        CreateChildren(htmlOut, nameRow("itemSubCount"), 0)
                      Else
                        htmlOut.Append("<ul class=""sortable list-unstyled"">")

                        htmlOut.Append("</ul>")
                      End If

                      htmlOut.Append("</ul>")
                    Next
                  End If
                End If
              Next
              htmlOut.Append("</ul>")
            Else
              ' No(blocks)
              htmlOut.Append("<ul class=""sortable list-unstyled""><li></li></ul>")
            End If
          End If

          'BlockTable.Dispose()
          'BlockTable = Nothing

          htmlOut.Append("</li>")

        Next
        If StartingArea <> "" Then
          htmlOut.Append("</li></ul>")
        End If

      End If
      htmlOut.Append("</ul>")

    End If

    If Not Page.ClientScript.IsClientScriptBlockRegistered("DisableCursor") Then '
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Attribute_UpdatePanel, Me.GetType, "DisableCursor", "ChangeTheMouseCursorOnItemParentDocument('cursor_default standalone_page');", True)
    End If

    Return htmlOut.ToString

  End Function



  Public Function CreateChildren(ByRef htmlOut As StringBuilder, ByVal parentID As Long, ByRef ChildCount As Integer) As String
    Dim ReturnTable As New DataTable
    htmlOut.Append("<ul class=""sortable list-unstyled"">")

    ReturnTable = get_topics(parentID)
    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows
        htmlOut.Append("<li>")
        htmlOut.Append("<div class=""block block-title italic""><div class=""notFolder""><span></span>")

        htmlOut.Append(r("acatt_name"))

        htmlOut.Append("</div></div>")
        If r("itemSubCount") > 0 Then
          CreateChildren(htmlOut, r("acatt_id"), 0)
        Else
          htmlOut.Append("<ul class=""sortable list-unstyled""><li></li></ul>")
        End If
        htmlOut.Append("</li>")
      Next
    End If
    htmlOut.Append("</ul>")

    Return htmlOut.ToString
  End Function

  Function attributeClickButton(ByVal acattID As Long, ByVal acattName As String)
    Dim returnString As String = ""

    returnString = "<a href=""javascript:Void(0);"" class=""text_underline"" onclick=""$('#" & add_attention.ClientID & "').html('');$('#" & selectedAttribute.ClientID & "').val('" & acattID & "');$('#" & editUpdateButton.ClientID & "').click();ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page');$('#" & attention_label.ClientID & "').html('');"">"
    returnString += acattName
    returnString += "</a>"
    Return returnString
  End Function

  Function assetClickButton(ByVal assetID As Long, ByVal assetName As String)
    Dim returnString As String = ""

    returnString = "<a href=""javascript:void(0);"" class=""text_underline"" onclick=""$('#" & add_attention.ClientID & "').html('');$('#" & selectedAsset.ClientID & "').val('" & assetID & "');$('#" & editAssetUpdateButton.ClientID & "').click();ChangeTheMouseCursorOnItemParentDocument('cursor_wait standalone_page');$('#" & attention_label.ClientID & "').html('');"">"
    returnString += assetName
    returnString += "</a>"
    Return returnString
  End Function
  Public Function getModelWithAttributes(ByVal attID As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection

    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      '-- GET A SUMMARY OF MODELS ATTACHED TO A GIVEN ATTRIBUTE

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sQuery.Append("select amod_id as MODID, amod_make_name as MAKENAME, amod_model_name as MODELNAME, ")
      sQuery.Append(" case when sum(case when attmod_standard_equip = 'Y' then 1 else 0 end) > 0 then 'Y' else '-' end as STANDARD, ")
      sQuery.Append(" case when sum(case when attmod_standard_equip = 'N' then 1 else 0 end) > 0 then 'Y' else '-' end as MAPPED, ")
      sQuery.Append(" count(distinct ac_id) as NUMAIRCRAFT, ")
      sQuery.Append(" SUM( case when acattind_status_flag = 'Y' then 1 else 0 end ) as HAVE, ")
      sQuery.Append(" SUM( case when acattind_status_flag = 'N' then 1 else 0 end ) as DONOTHAVE, ")
      sQuery.Append(" SUM( case when acattind_status_flag = 'U' then 1 else 0 end ) as UNKNOWN ")


      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.Aircraft_Attribute with (NOLOCK) ")
        sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Index with (NOLOCK) on acattind_acatt_id = acatt_id ")
        sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft with (NOLOCK) on ac_id = acattind_ac_id  and acattind_journ_id = ac_journ_id ")
        sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model with (NOLOCK) on attmod_amod_id = ac_amod_id and attmod_att_id=acatt_id")
        sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.aircraft_model with (NOLOCK) on amod_id = ac_amod_id  ")
      Else
        sQuery.Append(" from Aircraft_Attribute with (NOLOCK) ")
        sQuery.Append(" inner join Aircraft_Attribute_Index with (NOLOCK) on acattind_acatt_id = acatt_id ")
        sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = acattind_ac_id  and acattind_journ_id = ac_journ_id ")
        sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on attmod_amod_id = ac_amod_id and attmod_att_id=acatt_id")
        sQuery.Append(" left outer join aircraft_model with (NOLOCK) on amod_id = ac_amod_id  ")
      End If



      sQuery.Append(" where acatt_id = @attID")
      sQuery.Append(" and ac_journ_id = 0 group by amod_id, amod_make_name, amod_model_name, acatt_model_dependent_flag order by amod_id,amod_make_name asc, amod_model_name asc")


      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


      SqlCommand.Parameters.AddWithValue("attID", attID)



      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try


      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

    Return temptable

  End Function

  Public Function get_topics(ByVal parentID As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If parentID > 0 Then

        sQuery.Append(" select achild.acatt_name as acatt_name, achild.acatt_id as acatt_id, ")


        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
          sQuery.Append("( SELECT count(*)")
          sQuery.Append(" FROM [Homebase].jetnet_ra.dbo.aircraft_attribute_reference with (NOLOCK)")
          sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
          sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
          sQuery.Append(" where acatr_parent_id = achild.acatt_id ")
          sQuery.Append(" ) AS itemSubCount ")
          sQuery.Append(" from [Homebase].jetnet_ra.dbo.aircraft_attribute_reference with (NOLOCK) ")
          sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
          sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
        Else
          sQuery.Append("( SELECT count(*)")
          sQuery.Append(" FROM aircraft_attribute_reference with (NOLOCK)")
          sQuery.Append(" inner join Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
          sQuery.Append(" inner join Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
          sQuery.Append(" where acatr_parent_id = achild.acatt_id ")
          sQuery.Append(" ) AS itemSubCount ")
          sQuery.Append(" from aircraft_attribute_reference with (NOLOCK) ")
          sQuery.Append(" inner join Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
          sQuery.Append(" inner join Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
        End If


        sQuery.Append(" where acatr_parent_id = " & parentID.ToString)
        sQuery.Append(" order by achild.acatt_name ")



        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        SqlConn.Open()
        SqlCommand.Connection = SqlConn


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 600

        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          temptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
        End Try
      End If
    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function getAreaBlock() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append("select acattarea_area_name as AREA, acattarea_block_name as BLOCK ")
      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append("from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Area with (NOLOCK) ")
      Else
        sQuery.Append("from Aircraft_Attribute_Area with (NOLOCK) ")
      End If



      sQuery.Append("order by acattarea_area_name, acattarea_block_name ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function getBlock(ByVal AreaTable As DataTable) As DataTable

    Dim temptable As New DataTable

    Dim sQuery = New StringBuilder()
    Dim AvionicsOnly As Boolean = False
    Dim OtherOnly As Boolean = False
    Try



      Dim distinct_table_view As New DataView
      Dim distinct_table As New DataTable

      distinct_table_view = AreaTable.DefaultView
      distinct_table_view.Sort = "BLOCK"

      If area_drop.SelectedValue <> "" Then
        distinct_table_view.RowFilter = "AREA = '" & area_drop.SelectedValue & "'"
      End If

      ''actually get the distinct values.
      distinct_table = distinct_table_view.ToTable()
      temptable = distinct_table



    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally


    End Try

    Return temptable

  End Function
  Public Function getAlphaTopics() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append("select acatt_name as NAME, acatt_average_value, acatt_id, acatt_count, acatt_refresh_date, 	case when acatt_glossary='Y' then 'YES' else '' end as GLOSSARY, case when acatt_model_dependent_flag='Y' then 'YES' else '' end as ONMODELSONLY, ")
        sQuery.Append("case when acatt_synonym_id=0 then ' ' else (select top 1 b.acatt_name from [Homebase].jetnet_ra.dbo.Aircraft_Attribute b with (NOLOCK) where b.acatt_id = Aircraft_Attribute.acatt_synonym_id) end as SYNONYM, ")
        sQuery.Append("acatt_refresh_date as LASTREFRESH, acatt_count AS AIRCRAFT, ")
        sQuery.Append("(select COUNT(*) from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model with (NOLOCK) where attmod_att_id = Aircraft_Attribute.acatt_id) as MODELSMAPPED, ")
        sQuery.Append("case when LEN(acatt_description) > 0 then 'YES' else '' end as DESCRIPTION, ")
        sQuery.Append(" acatt_abbrev, acatt_auto_generate,")

        sQuery.Append("acatt_area as AREA, acatt_block as BLOCK  from [Homebase].jetnet_ra.dbo.Aircraft_Attribute with (NOLOCK) ")
      Else
        sQuery.Append("select acatt_name as NAME, acatt_average_value, acatt_id, acatt_count, acatt_refresh_date, 	case when acatt_glossary='Y' then 'YES' else '' end as GLOSSARY, case when acatt_model_dependent_flag='Y' then 'YES' else '' end as ONMODELSONLY, ")
        sQuery.Append("case when acatt_synonym_id=0 then ' ' else (select top 1 b.acatt_name from Aircraft_Attribute b with (NOLOCK) where b.acatt_id = Aircraft_Attribute.acatt_synonym_id) end as SYNONYM, ")
        sQuery.Append("acatt_refresh_date as LASTREFRESH, acatt_count AS AIRCRAFT, ")
        sQuery.Append("(select COUNT(*) from Aircraft_Attribute_Model with (NOLOCK) where attmod_att_id = Aircraft_Attribute.acatt_id) as MODELSMAPPED, ")
        sQuery.Append("case when LEN(acatt_description) > 0 then 'YES' else '' end as DESCRIPTION, ")
        sQuery.Append(" acatt_abbrev, acatt_auto_generate,")
        sQuery.Append("acatt_area as AREA, acatt_block as BLOCK  from Aircraft_Attribute with (NOLOCK) ")
      End If



      If viewStatus.SelectedValue <> "" Then
        If viewStatus.SelectedValue = "Y" Then
          sQuery.Append(" where acatt_status = 'Y' ")
        ElseIf viewStatus.SelectedValue = "N" Then
          sQuery.Append(" where acatt_status = 'N' ")
        End If
      End If

      If viewStateShow.SelectedValue <> "" Then
        If viewStateShow.SelectedValue = "glossary" Then
          sQuery.Append(" where acatt_glossary = 'Y' ")
        End If
      End If


      sQuery.Append("order by acatt_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function getTopLevelTopics() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select *  ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(", ( SELECT count(*) FROM [Homebase].jetnet_ra.dbo.aircraft_attribute_reference with (NOLOCK)  ")
        sQuery.Append(" inner Join [Homebase].jetnet_ra.dbo.Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
        sQuery.Append(" inner Join [Homebase].jetnet_ra.dbo.Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id  ")
        sQuery.Append(" where acatr_parent_id = a1.acatt_id  ) AS itemSubCount  ")

        sQuery.Append(" from  [Homebase].jetnet_ra.dbo.aircraft_attribute a1 with (NOLOCK) ")
      Else
        sQuery.Append(", ( SELECT count(*) FROM aircraft_attribute_reference with (NOLOCK)  ")
        sQuery.Append(" inner Join Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
        sQuery.Append(" inner Join Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id  ")
        sQuery.Append(" where acatr_parent_id = a1.acatt_id   ) AS itemSubCount  ")

        sQuery.Append(" from  aircraft_attribute a1 with (NOLOCK) ")
      End If



      If viewStatus.SelectedValue <> "" Then
        If viewStatus.SelectedValue = "Y" Then
          sQuery.Append(" where acatt_status = 'Y' ")
        ElseIf viewStatus.SelectedValue = "N" Then
          sQuery.Append(" where acatt_status = 'N' ")
        End If
      End If

      sQuery.Append(" order by acatt_area, acatt_block, acatt_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function getLinkableAttributes() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct acatt_name, acatt_id ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" From [Homebase].jetnet_ra.dbo.aircraft_attribute with (NOLOCK) ")
      Else
        sQuery.Append(" From aircraft_attribute with (NOLOCK) ")
      End If


      sQuery.Append(" Where acatt_auto_generate ='Y' and len(acatt_query) > 0 ")

      sQuery.Append(" order by acatt_name")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function getAssetAttributes(ByVal acattID As Long, ByVal assetID As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection

    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try



      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        SqlConn.ConnectionString = "server=www.jetnetsql1.com;initial catalog=jetnet_ra;Persist Security Info=False;User Id=evolution;Password=vbs73az8;"
      Else
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      End If

      SqlConn.Open()


      sQuery.Append("SELECT distinct aimodif_description as ASSETNAME, aimodif_item_id, acatt_id,  acatt_name as ATTNAME,  ")
      sQuery.Append("case when len(aimodif_jetnet_query) > 0 then 'YES' else ' ' end as JETNETQUERY, COUNT(distinct acattind_ac_id) as AIRCRAFT  ")
      sQuery.Append("FROM Asset_Insight_Modifications with (NOLOCK)  ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append("left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute with (NOLOCK) on aimodif_acatt_id = acatt_id  ")
        sQuery.Append("left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Index with (NOLOCK) on acatt_id = acattind_acatt_id ")
      Else
        sQuery.Append("left outer join Aircraft_Attribute with (NOLOCK) on aimodif_acatt_id = acatt_id  ")
        sQuery.Append("left outer join Aircraft_Attribute_Index with (NOLOCK) on acatt_id = acattind_acatt_id ")
      End If




      'sQuery.Append(" where ")
      Dim sqlWhere As String = ""
      Dim andString As String = ""
      If acattID > 0 Then
        sqlWhere = " aimodif_acatt_id = @acattID "
        andString = " and "
      End If

      If assetID > 0 Then
        sqlWhere += andString + " aimodif_item_id = @assetID "
        andString = " and "
      End If


      If assetID = 0 And acattID = 0 Then
        If viewStateShow.SelectedValue <> "" Then
          If viewStateShow.SelectedValue = "glossary" Then
            sqlWhere += andString + " acatt_glossary = 'Y' "
            andString = " and "
          End If
        End If
      End If

      If sqlWhere <> "" Then
        sQuery.Append(" where " & sqlWhere)
      End If

      sQuery.Append(" group by aimodif_description, aimodif_item_id,  acatt_id, acatt_name, aimodif_jetnet_query    ")

      Dim orderBy As String = " order by aimodif_description asc "

      If assetID = 0 And acattID = 0 Then
        If viewStateShow.SelectedValue <> "" Then
          If viewStateShow.SelectedValue = "alpha" Then
            orderBy = " order by acatt_name asc "
          End If
        End If
      End If

      sQuery.Append(orderBy)


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      If acattID > 0 Then
        SqlCommand.Parameters.AddWithValue("acattID", acattID)
      End If

      If assetID > 0 Then
        SqlCommand.Parameters.AddWithValue("assetID", assetID)
      End If

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try

        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing



    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return temptable

  End Function
  Public Function getModelRelationshipsTable(ByVal attmod_att_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sQuery.Append("Select amod_make_name, amod_model_name,attmod_value, attmod_standard_equip, attmod_stdeq_start_ser_no_value, attmod_stdeq_start_ser_no_value, attmod_seq_no, attmod_notes, amod_id ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model  with (NOLOCK) inner join [Homebase].jetnet_ra.dbo.Aircraft_Model with (NOLOCK) on attmod_amod_id = amod_id ")
      Else
        sQuery.Append(" from Aircraft_Attribute_Model  with (NOLOCK) inner join Aircraft_Model with (NOLOCK) on attmod_amod_id = amod_id ")
      End If


      sQuery.Append(" where attmod_att_id = @attmodAttID")
      sQuery.Append(" order by amod_make_name, amod_model_name")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      SqlCommand.Parameters.AddWithValue("attmodAttID", attmod_att_id)


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return temptable

  End Function

  Public Function getRelatedAircraft(ByVal att_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sQuery.Append("select amod_make_name as MAKE, amod_model_name as MODEL, ac_mfr_year, ac_year, ac_ser_no_full as SERNBR, ac_reg_no as REGNBR, ac_id ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Index  with (NOLOCK) ")
        sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft with (NOLOCK) on ac_id = acattind_ac_id  and acattind_journ_id = ac_journ_id ")
        sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.aircraft_model with (NOLOCK) on amod_id = ac_amod_id  ")
      Else
        sQuery.Append(" from Aircraft_Attribute_Index  with (NOLOCK) ")
        sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = acattind_ac_id  and acattind_journ_id = ac_journ_id ")
        sQuery.Append(" left outer join aircraft_model with (NOLOCK) on amod_id = ac_amod_id  ")
      End If





      sQuery.Append(" where acattind_acatt_id = @AttID")
      sQuery.Append(" order by amod_make_name, amod_model_name, ac_ser_no_full ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      SqlCommand.Parameters.AddWithValue("AttID", att_id)


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return temptable

  End Function
  Public Function getSynonymList() As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      sQuery.Append(" select distinct acatt_name, acatt_id ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" From [Homebase].jetnet_ra.dbo.aircraft_attribute with (NOLOCK) ")
      Else
        sQuery.Append(" From aircraft_attribute with (NOLOCK) ")
      End If

      'sQuery.Append(" Where acatt_area = @acatt_area")
      sQuery.Append(" Order By acatt_name")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

      'SqlCommand.Parameters.AddWithValue("acatt_area", acatt_area)


      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception

      'aError = "Error In ac_dealer_get_relationship_sales_main_comp_id() As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return temptable

  End Function
  Public Sub fill_action_options(ByRef drop_to_fill As DropDownList)

    drop_to_fill.Items.Clear()
    drop_to_fill.Items.Add("")
    drop_to_fill.Items.Add("Like")
    drop_to_fill.Items.Add("Not Like")
    drop_to_fill.Items.Add("EQUAL")
    drop_to_fill.Items.Add("Not EQUAL")

  End Sub

  Public Sub fillUpAreaBlock(ByVal overriddenAreaType As String)

    Dim ReturnTable As New DataTable
    Dim BlockTable As New DataTable

    ReturnTable = getAreaBlock()

    FillUpAreaDropdown(ReturnTable)

    If Not String.IsNullOrEmpty(overriddenAreaType) Then
      area_drop.SelectedValue = overriddenAreaType
    End If


    BlockTable = getBlock(ReturnTable)
    block_drop.Items.Clear()
    block_drop.Items.Add("")

    If Not IsNothing(BlockTable) Then
      For Each r As DataRow In BlockTable.Rows
        block_drop.Items.Add(New System.Web.UI.WebControls.ListItem(r("BLOCK"), r("BLOCK")))
      Next
    End If

  End Sub

  Private Sub FillUpAreaDropdown(ByVal ReturnTable As DataTable)
    area_drop.Items.Clear()
    area_drop.Items.Add("")

    Dim distinct_table_view As New DataView
    Dim distinct_table As New DataTable
    ''create the view to get the distinct values.
    distinct_table_view = ReturnTable.DefaultView
    distinct_table_view.Sort = "AREA"


    ''actually get the distinct values.
    distinct_table = distinct_table_view.ToTable(True, "AREA")

    If Not IsNothing(distinct_table) Then
      For Each r As DataRow In distinct_table.Rows
        area_drop.Items.Add(New System.Web.UI.WebControls.ListItem(r("AREA"), r("AREA")))
      Next
    End If

  End Sub
  Public Function get_rules_function(ByVal att_id As Long) As String

    Dim ReturnTable As New DataTable
    get_rules_function = "<table id= ""AttributesRules"">"
    get_rules_function &= "<thead>"
    get_rules_function &= "<tr>"
    get_rules_function &= "<th>Operator</th>"
    get_rules_function &= "<th>Area</th>"
    get_rules_function &= "<th>Block</th>"
    get_rules_function &= "<th>Action</th>"
    get_rules_function &= "<th>Phrases</th>"
    get_rules_function &= "</tr>"
    get_rules_function &= "</thead>"
    get_rules_function &= "<tbody>"

    If att_id > 0 Then 'otherwise empty table
      ReturnTable = get_my_rules(att_id)

      If Not IsNothing(ReturnTable) Then
        For Each r As DataRow In ReturnTable.Rows
          get_rules_function &= "<tr>"
          get_rules_function &= "<td>" & r("acatrule_order") & "</td>"
          get_rules_function &= "<td>" & r("acatrule_area") & "</td>"
          get_rules_function &= "<td>" & r("acatrule_block") & "</td>"
          get_rules_function &= "<td><A href='attributes.aspx?id=" & att_id & "&rule_id=" & r("acatrule_id") & "'>" & r("acatrule_action") & "</td>"
          get_rules_function &= "<td>" & r("acatrule_phrases") & "</td>"
          get_rules_function &= "</td></tr>"
        Next
      End If
    End If

    get_rules_function &= "</tbody>"
    get_rules_function &= "</table>"

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AttributesRulesTable", "$(document).ready( function () {$('#AttributesRules').DataTable();} );", True)

  End Function

  Public Function getModelRelationships(ByVal att_id As String) As String
    Dim returnString As String = ""
    Dim ReturnTable As New DataTable
    returnString = "<table id=""modelRelationships"">"
    returnString &= "<thead>"
    returnString &= "<tr>"
    returnString &= "<th>Edit</th>"
    returnString &= "<th>Model</th>"
    returnString &= "<th>Std Equip</th>"
    returnString &= "<th>S/N Range</th>"
    returnString &= "<th>Est. Value Impact</th>"
    returnString &= "<th>Importance</th>"
    returnString &= "<th width='200'>Notes</th>"
    returnString &= "</tr>"
    returnString &= "</thead>"
    returnString &= "<tbody>"
    ReturnTable = getModelRelationshipsTable(att_id)

    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows
        returnString &= "<tr>"
        returnString &= "<td>"
        If Not IsDBNull(r("amod_id")) Then
          returnString &= "<A href='www.evolutionadmin.com/home_Model.aspx?modelID=" & r("amod_id") & "&attIndex=" & att_id & "' target='_blank'>Edit</a>"
        End If
        returnString &= "</td>"

        returnString &= "<td>"
        If Not IsDBNull(r("amod_make_name")) Then
          returnString &= r("amod_make_name")
        End If
        If Not IsDBNull(r("amod_model_name")) Then
          returnString &= " " & r("amod_model_name")
        End If
        returnString &= "</td>"
        returnString &= "<td class=""text_align_center"">"
        If Not IsDBNull(r("attmod_standard_equip")) Then
          If UCase(r("attmod_standard_equip")) = "Y" Then
            returnString &= "YES"
          End If
        End If
        returnString &= "</td>"

        returnString &= "<td>"
        If Not IsDBNull(r("attmod_stdeq_start_ser_no_value")) Then
          returnString &= r("attmod_stdeq_start_ser_no_value")
        End If
        returnString &= "</td>"
        returnString &= "<td>"
        If Not IsDBNull(r("attmod_value")) Then
          returnString &= r("attmod_value")
        End If
        returnString &= "</td>"
        returnString &= "<td>"
        If Not IsDBNull(r("attmod_seq_no")) Then
          returnString &= r("attmod_seq_no")
        End If
        returnString &= "</td>"

        returnString &= "<td>"
        If Not IsDBNull(r("attmod_notes")) Then
          returnString &= r("attmod_notes")
        End If
        returnString &= "</td>"

        returnString &= "</tr>"
      Next
    End If
    returnString &= "</tbody>"
    returnString &= "</table>"


    Return returnString
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "modelRelationshipsTable", "$(document).ready( function () {$('#modelRelationships').DataTable();} );", True)

  End Function



  Public Function getAircraftAttribute(ByVal att_id As String) As String
    Dim returnString As String = ""
    Dim ReturnTable As New DataTable
    Dim html As New StringBuilder

    returnString = "<table id=""aircraftTable"">"
    ReturnTable = getRelatedAircraft(att_id)



    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows

        If Not String.IsNullOrEmpty(html.ToString) Then
          html.Append(",")
        End If

        html.Append("{")
        html.Append("""SEL"": """",")

        html.Append("""Make"": """)
        If Not IsDBNull(r("MAKE")) Then
          html.Append(r("MAKE"))
        End If
        html.Append(""",")

        html.Append("""Model"": """)
        If Not IsDBNull(r("MODEL")) Then
          html.Append(" " & r("MODEL"))
        End If
        html.Append(""",")


        If Not IsDBNull(r("SERNBR")) Then
          'htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")

          html.Append("""Serial Number"":""<a href='DisplayAircraftDetail.aspx?acid=" + r("ac_id").ToString + "&jid=0' target='_blank'>" & r("SERNBR") & "</a>"",")
        Else
          html.Append("""Serial Number"": """",")
        End If

        If Not IsDBNull(r("REGNBR")) Then
          html.Append("""Registration Number"": """ & r("REGNBR") & """,")
        Else
          html.Append("""Registration Number"": """",")
        End If

        If Not IsDBNull(r("ac_year")) Then
          html.Append("""YEAR"": """ & r("ac_year") & """,")
        Else
          html.Append("""YEAR"": """",")
        End If

        If Not IsDBNull(r("ac_mfr_year")) Then
          html.Append("""MFR YEAR"": """ & r("ac_mfr_year") & """")
        Else
          html.Append("""MFR YEAR"": """"")
        End If

        html.Append("}")

      Next
    End If
    'returnString &= "</tbody>"
    returnString &= "</table>"



    Dim jsString As String = ""
    jsString = "$('#aircraftTable').DataTable({ data: aircraftDataSet,"
    jsString += " columns: ["
    jsString += "  { title: ""Make"", data: ""Make"" },"
    jsString += "  { title: ""Model"", data: ""Model"" },"
    jsString += " { title: ""MFR Year"", data: ""MFR YEAR"" },"
    jsString += " { title: ""Year"", data: ""YEAR"" },"
    jsString += " { title: ""Ser #"", data: ""Serial Number"" },"
    jsString += " { title: ""Reg #"", data: ""Registration Number"" }"
    jsString += " ]"
    jsString += " });"

    System.Web.UI.ScriptManager.RegisterClientScriptBlock(acRelated, acRelated.GetType(), "relatedACTable", "var aircraftDataSet;aircraftDataSet = [ " & html.ToString & " ];" & jsString & ";$('#" & acRelatedRan.ClientID & "').val('true');ChangeTheMouseCursorOnItemParentDocument('cursor_default standalone_page');", True)
    Return returnString
  End Function

  Public Function get_synonyms_function(ByVal att_id As String) As String

    Dim ReturnTable As New DataTable
    get_synonyms_function = "<table id=""related_syn"">"
    get_synonyms_function &= "<thead>"
    get_synonyms_function &= "<tr>"
    get_synonyms_function &= "<th>ID</th>"
    get_synonyms_function &= "<th>Name</th>"
    get_synonyms_function &= "</tr>"
    get_synonyms_function &= "</thead>"
    get_synonyms_function &= "<tbody>"
    ReturnTable = get_my_synonyms(att_id)

    known_as_label.Visible = True
    synonym_id.Visible = True

    If Not IsNothing(ReturnTable) Then
      If ReturnTable.Rows.Count > 0 Then
        For Each r As DataRow In ReturnTable.Rows
          get_synonyms_function &= "<tr><td>" & r("acatt_id") & "</td><td><A href='attributes.aspx?id=" & r("acatt_id") & "'>"
          get_synonyms_function &= r("acatt_name")
          get_synonyms_function &= "</a></td></tr>"
        Next
        synonym_id.Visible = False
        known_as_label.Visible = False
      End If

    End If
    get_synonyms_function &= "</tbody>"
    get_synonyms_function &= "</table>"

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "related_syn", "$(document).ready( function () {$('#related_syn').DataTable();} );", True)

  End Function

  Public Function get_related_attributes_function(ByVal att_id As String, ByVal relation_type As String) As String

    Dim ReturnTable As New DataTable
    get_related_attributes_function = "<table id=""relatedAttributes_" & Trim(relation_type) & """>"
    get_related_attributes_function &= "<thead>"
    get_related_attributes_function &= "<tr>"
    get_related_attributes_function &= "<th>ID</th>"
    get_related_attributes_function &= "<th>Name</th>"
    get_related_attributes_function &= "</tr>"
    get_related_attributes_function &= "</thead>"
    get_related_attributes_function &= "<tbody>"

    ReturnTable = get_my_attributes(att_id, relation_type)

    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows
        get_related_attributes_function &= "<tr><td>" & r("child_id") & "</td><td><A href='attributes.aspx?id=" & r("child_id") & "'>"
        get_related_attributes_function &= r("child_name")
        get_related_attributes_function &= "</a></td></tr>"
      Next
    End If
    get_related_attributes_function &= "</tbody>"
    get_related_attributes_function &= "</table>"

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "relatedAttributesTable_" & Trim(relation_type) & "", "$(document).ready( function () {$('#relatedAttributes_" & Trim(relation_type) & "').DataTable();} );", True)

  End Function

  Public Function get_rule_data_top(ByVal rule_id As String) As String

    get_rule_data_top = ""

    Dim ReturnTable As New DataTable

    ReturnTable = get_rule_data(rule_id)
    If Not IsNothing(ReturnTable) Then
      For Each r As DataRow In ReturnTable.Rows

        Me.rule_operator.Text = r("acatrule_operator")
        Me.rule_area.Text = r("acatrule_area")
        Me.rule_block.Text = r("acatrule_block")
        Me.rule_action_drop.Text = r("acatrule_action")

        If Not IsDBNull(r("acatrule_phrases")) Then
          Me.rule_phrases_textbox.Text = r("acatrule_phrases")
        Else
          Me.rule_phrases_textbox.Text = ""
        End If

      Next
    End If

  End Function

  Public Function get_attributes_data_top(ByVal att_id As String) As String

    get_attributes_data_top = ""

    Dim ReturnTable As New DataTable

    attention_label.Text = ""

    ReturnTable = get_attributes_data(att_id)
    If Not IsNothing(ReturnTable) Then
      fillUpAreaBlock(ReturnTable.Rows(0).Item("acatt_area"))

      For Each r As DataRow In ReturnTable.Rows

        Try
          Me.area_drop.SelectedValue = r("acatt_area")
        Catch
          area_drop.SelectedValue = ""
        End Try
        Try
          Me.block_drop.SelectedValue = r("acatt_block")
        Catch
          block_drop.SelectedValue = ""
        End Try
        Me.name_text.Text = r("acatt_name")

        If Not IsDBNull(r("acatt_howtofind")) Then
          howToFindRule.Text = r("acatt_howtofind")
        End If
        If Not IsDBNull(r("acatt_query")) Then
          queryRule.Text = r("acatt_query")
          queryRule.ToolTip = queryRule.Text
        End If

        If Not IsDBNull(r("acatt_auto_generate")) Then
          If r("acatt_auto_generate") = "Y" Then
            autoGenerateRule.Checked = True
          Else
            autoGenerateRule.Checked = False
          End If
        End If

        If Not IsDBNull(r("acatt_low_value")) Then
          acatt_low.Text = r("acatt_low_value").ToString
        End If
        If Not IsDBNull(r("acatt_average_value")) Then
          acatt_average.Text = r("acatt_average_value").ToString
        End If
        If Not IsDBNull(r("acatt_high_value")) Then
          acatt_high.Text = r("acatt_high_value").ToString
        End If
        If Not IsDBNull(r("acatt_synonym_id")) Then
          synonym_id.SelectedValue = r("acatt_synonym_id")
        End If
        If Not IsDBNull(r("acatt_action_date")) Then
          last_action_date.Text = r("acatt_action_date")
        End If

        If Not IsDBNull(r("acatt_refresh_date")) Then
          last_refresh_date.Text = r("acatt_refresh_date")
        End If

        If Not IsDBNull(r("acatt_synonym_id")) Then
          synonym_id.SelectedValue = r("acatt_synonym_id")
        End If

        If Not IsDBNull(r("acatt_abbrev")) Then
          code_text.Text = r("acatt_abbrev")
        End If
        If Not IsDBNull(r("acatt_description")) Then
          Me.description.Text = r("acatt_description")
        Else
          Me.description.Text = ""
        End If

        If Not IsDBNull(r("acatt_definition_url")) Then
          Me.def_url.Text = r("acatt_definition_url")
        Else
          Me.def_url.Text = ""
        End If


        If Not IsDBNull(r("acatt_count")) Then

          If r("acatt_count") = 0 Then
            tab_5.Visible = False
            acatt_count.Text = "Attribute Not Yet Indexed."
          Else
            acatt_count.Text = r("acatt_count") & " Aircraft Associated."
            tab_5.Visible = True
          End If
        Else
          acatt_count.Text = "Attribute Not Yet Indexed."
        End If

        If Not IsDBNull(r("acatt_status")) Then
          Try
            acatt_status.SelectedValue = r("acatt_status")
          Catch ex As Exception
            acatt_status.SelectedValue = "Y"
          End Try
        End If

        If Trim(r("acatt_glossary")) = "Y" Then
          Me.acatt_glossary.Checked = True
        Else
          Me.acatt_glossary.Checked = False
        End If

        If Trim(r("acatt_product_business_flag")) = "Y" Then
          Me.business_check.Checked = True
        Else
          Me.business_check.Checked = False
        End If

        If Trim(r("acatt_product_commercial_flag")) = "Y" Then
          Me.commercial_check.Checked = True
        Else
          Me.commercial_check.Checked = False
        End If

        model_dependent.Checked = False 'Default to false.

        If Not IsDBNull(r("acatt_model_dependent_flag")) Then
          If r("acatt_model_dependent_flag") = "Y" Then
            model_dependent.Checked = True
          End If
        End If

        If Trim(r("acatt_product_helicopter_flag")) = "Y" Then
          Me.heli_check.Checked = True
        Else
          Me.heli_check.Checked = False
        End If

        If Trim(r("acatt_aerodex_flag")) = "Y" Then
          Me.aerodex_check.Checked = True
        Else
          Me.aerodex_check.Checked = False
        End If


      Next
    End If

  End Function

  'Public Function get_distinct_list(ByVal field_to_use As String, ByVal value_to_use As String) As DataTable

  '  Dim temptable As New DataTable
  '  Dim SqlConn As New SqlClient.SqlConnection
  '  Dim SqlCommand As New SqlClient.SqlCommand
  '  Dim SqlReader As SqlClient.SqlDataReader
  '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing

  '  Dim sQuery = New StringBuilder()

  '  Try

  '    If Trim(field_to_use) = Trim(value_to_use) Then
  '      sQuery.Append(" select distinct " & field_to_use & " ")
  '    Else
  '      sQuery.Append(" select distinct " & field_to_use & ", " & value_to_use & " ")
  '    End If


  '    sQuery.Append(" from aircraft_attribute  with (NOLOCK) ")
  '    If Trim(field_to_use) = Trim(value_to_use) Then
  '      sQuery.Append(" group by " & field_to_use & " ")
  '    Else
  '      sQuery.Append(" group by " & field_to_use & ", " & value_to_use & "  ")
  '    End If

  '    sQuery.Append(" order by " & field_to_use & " asc  ")


  '    SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
  '    SqlConn.Open()
  '    SqlCommand.Connection = SqlConn


  '    ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

  '    SqlCommand.CommandText = sQuery.ToString
  '    SqlCommand.CommandType = CommandType.Text
  '    SqlCommand.CommandTimeout = 600

  '    SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

  '    Try
  '      temptable.Load(SqlReader)
  '    Catch constrExc As System.Data.ConstraintException
  '      Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
  '    End Try

  '  Catch ex As Exception

  '    'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

  '  Finally
  '    SqlReader = Nothing

  '    SqlConn.Dispose()
  '    SqlConn.Close()
  '    SqlConn = Nothing

  '    SqlCommand.Dispose()
  '    SqlCommand = Nothing
  '  End Try

  '  Return temptable

  'End Function

  Public Function get_my_rules(ByVal att_id As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select * ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.aircraft_attribute_rules with (NOLOCK) ")
      Else
        sQuery.Append(" from aircraft_attribute_rules with (NOLOCK) ")
      End If


      sQuery.Append(" where acatrule_parent_id = " & att_id & "  ")
      sQuery.Append(" order by acatrule_order asc ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function


  Public Function get_my_synonyms(ByVal att_id As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select acatt_id, acatt_name ")


      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" From [Homebase].jetnet_ra.dbo.Aircraft_Attribute with (NOLOCK) ")
      Else
        sQuery.Append(" From Aircraft_Attribute with (NOLOCK) ")
      End If


      sQuery.Append(" Where acatt_synonym_id = " & att_id & " Order By acatt_name ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function


  Public Function get_my_attributes(ByVal att_id As String, ByVal relation_type As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select aparent.acatt_name as parent_name, achild.acatt_name as child_name ")
      sQuery.Append(" , aparent.acatt_id as parent_id, achild.acatt_id as child_id ")
      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.aircraft_attribute_reference with (NOLOCK) ")
        sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
        sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
      Else
        sQuery.Append(" from aircraft_attribute_reference with (NOLOCK) ")
        sQuery.Append(" inner join Aircraft_Attribute aparent with (NOLOCK) on acatr_parent_id = aparent.acatt_id ")
        sQuery.Append(" inner join Aircraft_Attribute achild with (NOLOCK) on acatr_child_id = achild.acatt_id ")
      End If




      sQuery.Append(" where acatr_parent_id = " & att_id & "  ")

      If Trim(relation_type) <> "" Then
        sQuery.Append(" and acatr_type = '" & Trim(relation_type) & "'  ")
      End If


      sQuery.Append(" order by achild.acatt_name ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function
  Public Function get_rule_data(ByVal rule_id As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select * ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.aircraft_attribute_rules with (NOLOCK) ")
      Else
        sQuery.Append(" from aircraft_attribute_rules with (NOLOCK) ")
      End If


      sQuery.Append(" where acatrule_id = " & rule_id & "  ")
      sQuery.Append(" order by acatrule_order asc ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Function get_attributes_data(ByVal att_id As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select * ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" from [Homebase].jetnet_ra.dbo.aircraft_attribute with (NOLOCK) ")
      Else
        sQuery.Append(" from aircraft_attribute with (NOLOCK) ")
      End If



      sQuery.Append(" where acatt_id = " & att_id & "  ")


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      ' clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      'aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function



  Private Sub viewStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles viewStatus.SelectedIndexChanged
    Me.mainMenuAdd.Text = CreateTreeMenu()
  End Sub

  Private Sub area_drop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles area_drop.SelectedIndexChanged
    Dim BlockTable As New DataTable

    BlockTable = getBlock(getAreaBlock())

    block_drop.Items.Clear()
    block_drop.Items.Add("")

    If Not IsNothing(BlockTable) Then
      For Each r As DataRow In BlockTable.Rows
        block_drop.Items.Add(New System.Web.UI.WebControls.ListItem(r("BLOCK"), r("BLOCK")))
      Next
    End If
  End Sub

  Private Sub viewState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles viewStateShow.SelectedIndexChanged
    Me.mainMenuAdd.Text = CreateTreeMenu()
  End Sub

  Private Sub runACButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles runACButton.Click
    ac_related_label.Text = getAircraftAttribute(selectedAttribute.Text)

  End Sub

  Private Sub addModelRelationship_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles addModelRelationship.Click
    'Dim masterPage As New Object
    'Try
    '  If Not IsNothing(Session.Item("isMobile")) Then
    '    If Session.Item("isMobile") Then
    '      masterPage = DirectCast(Page.Master, MobileTheme)
    '    Else 
    '      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
    '        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    '      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
    '        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    '      End If
    '    End If
    '  Else
    '    masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    '  End If
    'Catch ex As Exception
    '  masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    'End Try

    ModelRelationshipAddPanel.Visible = True
    DisplayFunctions.SingleModelLookupAndFill(models_makeModel, masterPage)

    System.Web.UI.ScriptManager.RegisterClientScriptBlock(modelUpdate, modelUpdate.GetType(), "modelRelationshipsTable", "$(document).ready( function () {$('#modelRelationships').DataTable();} );", True)
    modelUpdate.Update()
  End Sub

  Private Sub saveAssetAttribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveAssetAttribute.Click
    'CREATE TABLE [dbo].[Asset_Insight_Modifications](
    '[aimodif_id] [int] IDENTITY(1,1) NOT NULL,
    '[aimodif_description] [varchar](100) NULL,
    '[aimodif_jetnet_area] [varchar](100) NULL,
    '[aimodif_jetnet_name] [varchar](100) NULL,
    '[aimodif_jetnet_query] [varchar](2000) NULL,
    '[aimodif_acatt_id] [int] NULL
    ') ON [PRIMARY]

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim update_string As String = ""


    Try

      If IsNumeric(selectedAsset.Text) Then
        If selectedAsset.Text > 0 Then
          If IsNumeric(linkedAttribute.SelectedValue) Then

            update_string = " Update Asset_Insight_Modifications set  "
            update_string &= " aimodif_acatt_id = @aimodif_acatt_id "
            update_string &= " where aimodif_item_id = " & selectedAsset.Text.ToString
            update_string = update_string

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
              SqlConn.ConnectionString = "server=www.jetnetsql1.com;initial catalog=jetnet_ra;Persist Security Info=False;User Id=evolution;Password=vbs73az8;"
            Else
              SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            End If


            SqlConn.Open()


            Dim SqlCommand As New SqlClient.SqlCommand(update_string, SqlConn)

            SqlCommand.Parameters.AddWithValue("aimodif_acatt_id", linkedAttribute.SelectedValue)


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, update_string.ToString)
            SqlCommand.ExecuteNonQuery()

            SqlCommand.Dispose()
            SqlCommand = Nothing
          End If
        End If
      End If
    Catch ex As Exception
    Finally
      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      Response.Redirect("attributes.aspx?asset=true")


    End Try

  End Sub


End Class