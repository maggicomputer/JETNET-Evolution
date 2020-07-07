
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/searchSummary.aspx.vb $
'$$Author: Matt $
'$$Date: 1/03/20 8:57a $
'$$Modtime: 1/02/20 9:09a $
'$$Revision: 6 $
'$$Workfile: searchSummary.aspx.vb $
'
' ********************************************************************************

Partial Public Class searchSummary
  Inherits System.Web.UI.Page


  Private Sub searchSummary_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    'Stepwise Refinement:
    'Check for login
    'If Not Postback:
    'Build Summary Option Folders
    'If Postback:
    'Build Summary Table.
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else
      'I programmed two ways to build the folders.
      'The first option is basic tables, however being able to collapse the folders won't work
      'I worked on the second option.
      'The 2nd option is commented out below but you can uncomment to see it.
      'basically you build tree views dynamically based on what you're displaying.
      'however you have to build it on every load, otherwise if you click an option, the tree view will
      'never do anything because it disappears on initial postback. It has to be build in init and each subsequent load.
      'if you choose the 1st option, you won't need to rebuild on each postback, it's a much more simple version -
      'just building it in a table that looks like a treeview.

      Me.Server.ScriptTimeout = 1200

      Dim sub_type As String = ""
      Dim FilterPage As Boolean = False

      If Trim(Request("sub_type")) <> "" Then
        sub_type = Trim(Request("sub_type"))
      End If

      If Not IsNothing(Trim(Request("filter"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("filter"))) Then
          FilterPage = True
        End If
      End If

      ' if its not postback, so its first time in or re-loaded, and there is no search session.. 
      'which means if its the first time any summary page has been loaded
      ' or 
      ' if the current summary type is not the same as the last summary type.

      If FilterPage = True Then
        'Response.Write("page should be filtered")
        folder_tab.HeaderText = "Topic Areas"
        summary_tab.HeaderText = "Topics - Alphabetic"
        second_summary_tab.HeaderText = "Topics - By Area"
        BuildTopicAreaDisplay(second_summary_table_label, False, True, "AREA", "TOPIC")
        BuildTopicAreaDisplay(summary_table_label, True, False, "LETTER", "TOPIC")
        leftTabContainerCell.Visible = False
        second_summary_tab.Visible = True
      Else 'if trim(request("filter")) isn't passed, the whole page should carry on as normal.

        If (Not IsPostBack And Trim(Session("SearchSummaryTreeHTML")) = "") Or (Trim(Session("SearchSummaryLastSub")) <> sub_type) Then
          holder_for_new_table.Text = get_folder_list(sub_type)
        ElseIf Trim(Session("SearchSummaryTreeHTML")) <> "" Then
          holder_for_new_table.Text = Session("SearchSummaryTreeHTML")
        End If


        If Trim(Request("h")) = "1" Then
          Me.folder_tab.HeaderText = "Historical Summary Options"
        Else
          Me.folder_tab.HeaderText = "Summary Options"
        End If

        If Trim(Request("sub_type")) <> "" Then
          Session("SearchSummaryLastSub") = Trim(sub_type)
        Else
          If Trim(Request("h")) = "1" Then
            Session("SearchSummaryLastSub") = "ACH"
          Else
            Session("SearchSummaryLastSub") = "AC"
          End If
        End If

        ' BuildSummaryOptionFolders()
        'If Page.IsPostBack Then
        BuildSummaryTable()
        'End If
      End If 'if filter page = false
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'Master.SetStatusText(HttpContext.Current.Session.Item("SearchString"))
    Master.SetPageTitle("Search Summary")
  End Sub

  ''' <summary>
  ''' Placeholder Function for Building Summary Option Folders.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub BuildSummaryOptionFolders()
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '1ST WAY
    Dim str As String = ""
    str = "<table width='250' cellspacing='0' cellpadding='3' class='data_aircraft_grid'>"
    str += "<tr class='header_row'>"
    str += "<td align='left' valign='bottom' width='35'><img src='images/expanded.png' alt='' /></td><td align='left' valign='middle'><b class='title'>General</b></td>"
    str += "</tr>"
    str += "<tr>"
    str += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td><td align='left' valign='middle'>Airframe Type (F/R)</td>"
    str += "</tr>"
    str += "<tr>"
    str += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td><td align='left' valign='middle'>Make Type Name</td>"
    str += "</tr>"
    str += "<tr>"
    str += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td><td align='left' valign='middle'>Make</td>"
    str += "</tr>"
    str += "<tr>"
    str += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td><td align='left' valign='middle'>Model</td>"
    str += "</tr>"
    str += "<tr class='header_row'>"
    str += "<td align='left' valign='bottom'><img src='images/expanded.png' alt='' /></td><td align='left' valign='middle'><b class='title'>Location</b></td>"
    str += "</tr>"
    str += "<tr>"
    str += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td><td align='left' valign='middle'>Base Airport IATA Code</td>"
    str += "</tr>"
    str += "</table>"
    holder_for_new_table.Text = str 'add table

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '2ND WAY
    general_tree.Visible = False 'in order to see the second way, toggle visibility to true.

    Dim TempNode As New TreeNode
    Dim TempSubNode As New TreeNode

    'Let's fill up the tree view with some static Data.
    TempNode.Text = "<b class='upperHeader'>General</b>"


    TempSubNode = New TreeNode
    TempSubNode.Text = "Airframe Type (F/R)"
    TempSubNode.ImageUrl = DisplayFunctions.ReturnFolderImage("", "N", "N")

    TempNode.ChildNodes.Add(TempSubNode) 'Add the Subnode to the Parent node.

    TempSubNode = New TreeNode
    TempSubNode.Text = "Make Type Name"
    TempSubNode.ImageUrl = DisplayFunctions.ReturnFolderImage("", "N", "N")
    TempNode.ChildNodes.Add(TempSubNode) 'Add the Subnode to the Parent node.

    TempSubNode = New TreeNode
    TempSubNode.Text = "Make"
    TempSubNode.ImageUrl = DisplayFunctions.ReturnFolderImage("", "N", "N")
    TempNode.ChildNodes.Add(TempSubNode) 'Add the Subnode to the Parent node.

    TempSubNode = New TreeNode
    TempSubNode.Text = "Model"
    TempSubNode.ImageUrl = DisplayFunctions.ReturnFolderImage("", "N", "N")
    TempNode.ChildNodes.Add(TempSubNode) 'Add the Subnode to the Parent node.

    general_tree.Nodes.Add(TempNode) 'Add Parent Node


    'New TreeView
    Dim location_Tree As New TreeView
    location_Tree.NodeIndent = "-15"
    location_Tree.ExpandImageUrl = "~/images/expandable.png"
    location_Tree.CollapseImageUrl = "~/images/expanded.png"
    location_Tree.RootNodeStyle.ChildNodesPadding = Unit.Pixel(3)
    location_Tree.RootNodeStyle.CssClass = "header_row"
    location_Tree.RootNodeStyle.ForeColor = Drawing.Color.AliceBlue
    location_Tree.RootNodeStyle.Font.Size = New FontUnit(10)
    location_Tree.NodeStyle.Font.Size = New FontUnit(8)
    location_Tree.NodeStyle.ForeColor = Drawing.Color.Black
    location_Tree.NodeStyle.HorizontalPadding = Unit.Pixel(2)
    location_Tree.NodeStyle.NodeSpacing = Unit.Pixel(0)
    location_Tree.NodeStyle.VerticalPadding = Unit.Pixel(0)

    location_Tree.Width = Unit.Percentage(100)
    location_Tree.CssClass = "aircraft_folder"

    'New Parent Node:
    TempNode = New TreeNode
    TempNode.Text = "<b class='upperHeader'>Location</b>"

    TempSubNode = New TreeNode
    TempSubNode.Text = "Base Airport IATA Code"
    TempSubNode.ImageUrl = DisplayFunctions.ReturnFolderImage("", "N", "N")

    TempNode.ChildNodes.Add(TempSubNode) 'Add the Subnode to the Parent node.
    location_Tree.Nodes.Add(TempNode) 'Add Parent Node


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'UNCOMMENT OUT FOR 2ND WAY
    'holder_for_new_tree_views.Controls.Add(location_Tree) 'Add New TreeView


  End Sub

  ''' <summary>
  ''' Placeholder for Building Summary Table:
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub BuildSummaryTable()


    If Trim(Request("type")) <> "" Then
      summary_table_label.Text = Sum_AC_Based_On_Criteria(Request("type"), Trim(Request("display")))
    Else
      If Trim(Request("sub_type")) = "C" Then
        summary_tab.HeaderText = "Company Summary"
      ElseIf Trim(Request("trans")) = "1" Then
        summary_tab.HeaderText = "Summary By Year/Month"
        Make_Transaction_History()
      ElseIf Trim(Request("h")) = "1" Then
        summary_tab.HeaderText = "Historical Market Summary"
        Make_AC_Mini_Market()
      Else
        summary_tab.HeaderText = "Mini Market Summary"
        Make_AC_Mini_Market()
        summary_table_label.Text += make_exec_summary()
      End If
    End If


  End Sub

  Public Function get_AC_Based_On_Criteria(ByVal temp_type As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bIsCompany As Boolean = False
    Dim bIsHistory As Boolean = False
    Dim bIsEvents As Boolean = False
    Dim bIsAircraft As Boolean = False

    Try

      sQuery.Append(" SELECT distinct ")

      sQuery.Append(temp_type)

      If temp_type.ToLower.Contains("amod_model_name") Then
        sQuery.Append(", amod_make_name, amod_id ")
      ElseIf temp_type.ToLower.Contains("acot_name") Then
        sQuery.Append(", ac_ownership_type ")
      End If

      If Not IsNothing(Request("sub_type")) Then
        If Not String.IsNullOrEmpty(Request("sub_type").ToString.Trim) Then
          If Request("sub_type").ToString.ToUpper.Contains("C") Then
            sQuery.Append(", count(distinct comp_id) as tcount ")
            bIsCompany = True
          End If
        End If
      End If

      If Not IsNothing(Request("h")) Then
        If Not String.IsNullOrEmpty(Request("h").ToString.Trim) Then
          If Request("h").ToString.ToUpper.Contains("1") Then
            sQuery.Append(", count(distinct journ_id) as tcount ")
            bIsHistory = True
          End If
        End If
      End If

      If Not IsNothing(Request("e")) Then
        If Not String.IsNullOrEmpty(Request("e").ToString.Trim) Then
          If Request("e").ToString.ToUpper.Contains("1") Then
            bIsEvents = True
          End If
        End If
      End If

      If Not bIsCompany And Not bIsHistory And Not bIsEvents Then
        bIsAircraft = True
        sQuery.Append(", count(distinct ac_id) as tcount ")
      End If

      If bIsCompany Then

        If Not IsNothing(HttpContext.Current.Session.Item("MasterCompanyFrom")) Then

          Dim temp_comp_where As String = HttpContext.Current.Session.Item("MasterCompanyFrom").ToString
          If temp_comp_where.ToLower.Contains("left outer join contact with(nolock) on (comp_id = contact_comp_id and comp_journ_id = contact_journ_id)") Then
            temp_comp_where = temp_comp_where.ToLower.Replace("left outer join contact with(nolock) on (comp_id = contact_comp_id and comp_journ_id = contact_journ_id)", "")
          End If

          sQuery.Append(temp_comp_where)

          temp_comp_where = ""
          temp_comp_where = HttpContext.Current.Session.Item("MasterCompanyWhere").ToString

          If Not temp_comp_where.ToLower.Contains("country") And Not temp_comp_where.ToLower.Contains("view_aircraft_company_flat") Then
            sQuery.Append(" INNER JOIN Country WITH(NOLOCK) on comp_country = country_name ")
          End If

          If Not temp_comp_where.ToLower.Contains("where") Then
            sQuery.Append(" WHERE ")
          End If

          sQuery.Append(temp_comp_where)
        Else

          sQuery.Append(" from View_Aircraft_Company_Flat WITH(NOLOCK) ")

          If InStr(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString, "WHERE") = 0 Then
            sQuery.Append(" WHERE ")
          End If

          sQuery.Append(HttpContext.Current.Session.Item("MasterCompanyWhere").ToString.Replace("comp_journ_id", "cref_journ_id"))

        End If

      ElseIf bIsEvents Then

        If HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString.ToLower.Contains("left outer join company") Then

          sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString.Trim)

        Else

          sQuery.Append(" FROM Priority_Events WITH(NOLOCK) ")
          sQuery.Append(" inner join Priority_Events_Category WITH(NOLOCK) on priorev_category_code=priorevcat_category_code")
          sQuery.Append(" LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) ")

        End If

        sQuery.Append(" WHERE ")
        sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftEventsWhere").ToString.Trim)

      ElseIf bIsHistory Then

        If HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("comp_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("contact_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("cref_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("state_") Then
          sQuery.Append(" FROM View_Aircraft_Company_History_Flat WITH(NOLOCK) ")
        Else
          sQuery.Append(" FROM View_Aircraft_History_Flat WITH(NOLOCK) ")
        End If

        sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.Trim)

      ElseIf bIsAircraft Then

        If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftFrom")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterAircraftFrom").ToString.Trim) Then
            sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftFrom").ToString.Trim)
          Else
            sQuery.Append(" FROM View_Aircraft_Flat WITH(NOLOCK) ")
          End If
        Else
          sQuery.Append(" FROM View_Aircraft_Flat WITH(NOLOCK) ")
        End If
        sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.Trim)

      End If

      If temp_type.ToLower.Contains("amod_model_name") Then
        sQuery.Append(" GROUP BY amod_make_name, amod_id, amod_model_name ")
      ElseIf temp_type.ToLower.Contains("acot_name") Then
        sQuery.Append(" GROUP BY " + temp_type.Trim + ", ac_ownership_type")
      Else
        sQuery.Append(" GROUP BY " + temp_type.Trim)
      End If

      If temp_type.ToLower.Contains("amod_model_name") Then
        sQuery.Append(" ORDER BY amod_make_name, amod_id, amod_model_name ")
      Else
        sQuery.Append(" ORDER BY " + temp_type.Trim + " ASC")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_AC_Based_On_Criteria(ByVal temp_type As String) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 180

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_AC_Based_On_Criteria load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_AC_Based_On_Criteria(ByVal temp_type As String, ByVal display1 As String) As DataTable " + ex.Message

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

  Public Function Sum_AC_Based_On_Criteria(ByVal summary_field As String, ByVal summary_field_display As String) As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim bIsCompany As Boolean = False
    Dim bIsHistory As Boolean = False
    Dim bIsEvents As Boolean = False
    Dim bIsAircraft As Boolean = False

    Dim count1 As Integer = 0
    Dim sum_total As Integer = 0

    Dim sumFieldValue As String = ""
    Dim summField As String = ""

    Dim sRefTitle As String = ""
    Dim sRefLink As String = ""
    Dim maintenanceTable As New DataTable

    Try

      If Not IsNothing(Request("sub_type")) Then
        If Not String.IsNullOrEmpty(Request("sub_type").ToString.Trim) Then
          If Request("sub_type").ToString.ToUpper.Contains("C") Then
            summary_tab.HeaderText = "Summary of Company by " + summary_field_display.Trim
            bIsCompany = True
          End If
        End If
      End If

      If Not IsNothing(Request("h")) Then
        If Not String.IsNullOrEmpty(Request("h").ToString.Trim) Then
          If Request("h").ToString.ToUpper.Contains("1") Then
            summary_tab.HeaderText = "Summary of Aircraft History by " + summary_field_display.Trim
            bIsHistory = True
          End If
        End If
      End If

      If Not IsNothing(Request("e")) Then
        If Not String.IsNullOrEmpty(Request("e").ToString.Trim) Then
          If Request("e").ToString.ToUpper.Contains("1") Then
            summary_tab.HeaderText = "Summary of Aircraft Events by " + summary_field_display.Trim
          End If
        End If
      End If

      If Not bIsCompany And Not bIsHistory And Not bIsEvents Then
        bIsAircraft = True
        summary_tab.HeaderText = "Summary of Aircraft by " + summary_field_display.Trim
      End If

      If Not String.IsNullOrEmpty(summary_field.Trim) Then

        results_table = get_AC_Based_On_Criteria(summary_field)

        If summary_field = "ac_maintained" Then
          'We need this table to compare whether us/foreign.
          maintenanceTable = Master.aclsData_Temp.MaintenanceRegulationUSOrForeign(False, False)
        End If

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            htmlOut.Append("<table id=""summaryDataTable"" width=""30%"" cellpadding=""2"" cellspacing=""0"" class=""data_aircraft_grid"">")

            For Each r As DataRow In results_table.Rows

              sumFieldValue = ""
              summField = ""

              If count1 = 0 Then
                htmlOut.Append("<tr class=""header_row"">")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><strong>" + summary_field_display.Trim + "</strong></td>")
                htmlOut.Append("<td align=""right"" valign=""middle""><strong>Count</strong></td>")
                htmlOut.Append("</tr>")
              End If

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

              If summary_field.ToLower.Contains("amod_model_name") Then

                If Not IsDBNull(r.Item("amod_id")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then

                    sumFieldValue = r.Item("amod_id").ToString.Trim

                    If Not IsDBNull(r.Item(summary_field)) Then
                      If Not String.IsNullOrEmpty(r.Item(summary_field).ToString.Trim) Then
                        sRefLink = "javascript:ParseForm('0', " + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false, false,'COMPARE_amod_id=Equals!~!amod_id"
                      End If
                    End If

                  End If
                End If
              ElseIf summary_field.ToLower.Contains("ac_maintained") Then

                If Not IsDBNull(r.Item(summary_field)) Then
                  If Not String.IsNullOrEmpty(r.Item(summary_field).ToString.Trim) Then
                    summField = r.Item(summary_field).ToString.Trim
                  Else
                    summField = "Blank"
                    sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_us_" + summary_field.Trim + "=Equals!~!us_" + summary_field.Trim
                    sRefLink += "=BLANK!~!');"
                  End If
                Else
                  summField = "Blank/Unknown"
                  sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_us_" + summary_field.Trim + "=Equals!~!us_" + summary_field.Trim
                  sRefLink += "=IS NULL!~!');"
                End If




                If Not IsNothing(maintenanceTable) Then
                  If maintenanceTable.Rows.Count > 0 Then
                    Dim Distinct_View As New DataView
                    Dim Distinct_Table As New DataTable

                    Distinct_View = maintenanceTable.DefaultView
                    Distinct_View.RowFilter = "certification_name = '" & summField & "'"

                    ''actually get the distinct values.
                    Distinct_Table = Distinct_View.ToTable()

                    If Not IsNothing(Distinct_Table) Then
                      If Distinct_Table.Rows.Count > 0 Then
                        If Distinct_Table.Rows(0).Item("certification_usa_flag") = "B" Or Distinct_Table.Rows(0).Item("certification_usa_flag") = "U" Then
                          sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_us_" + summary_field.Trim + "=Equals!~!us_" + summary_field.Trim
                        Else
                          sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_foreign_" + summary_field.Trim + "=Equals!~!foreign_" + summary_field.Trim
                        End If
                        sRefLink += "=\'" + summField.Trim + "\'!~!');"
                      End If
                    End If

                  End If
                End If


              ElseIf summary_field.ToLower.Contains("acot_name") Then

                If Not IsDBNull(r.Item("ac_ownership_type")) Then
                  If Not String.IsNullOrEmpty(r.Item("ac_ownership_type").ToString.Trim) Then

                    sumFieldValue = r.Item("ac_ownership_type").ToString.Trim

                    If Not IsDBNull(r.Item(summary_field)) Then
                      If Not String.IsNullOrEmpty(r.Item(summary_field).ToString.Trim) Then
                        sRefLink = "javascript:ParseForm('0', " + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_ac_ownership_type=Equals!~!ac_ownership_type"
                      End If
                    End If

                  End If
                End If

              Else

                If Not IsDBNull(r.Item(summary_field)) Then
                  If Not String.IsNullOrEmpty(r.Item(summary_field).ToString.Trim) Then
                    sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_" + summary_field.Trim + "=Equals!~!" + summary_field.Trim
                  End If
                End If

              End If

              If summary_field.ToLower.Contains("amod_model_name") Then

                If Not IsDBNull(r.Item("amod_model_name")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then

                    summField = r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim
                    sRefLink += "=" + sumFieldValue.Trim + "!~!');"

                  Else
                    summField = "Blank"
                    sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_" + summary_field.Trim + "=Equals!~!" + summary_field.Trim
                    sRefLink += "=BLANK!~!');"
                  End If
                Else
                  summField = "Blank/Unknown"
                  sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_" + summary_field.Trim + "=Equals!~!" + summary_field.Trim
                  sRefLink += "=IS NULL!~!');"
                End If
              ElseIf summary_field.ToLower.Contains("ac_maintained") Then

              Else

                If Not IsDBNull(r.Item(summary_field)) Then
                  If Not String.IsNullOrEmpty(r.Item(summary_field).ToString.Trim) Then

                    summField = r.Item(summary_field).ToString.Trim

                    If String.IsNullOrEmpty(sumFieldValue.Trim) Then
                      sumFieldValue = r.Item(summary_field).ToString.Trim
                    End If

                    sRefLink += "=" + sumFieldValue.Trim + "!~!');"
                  Else

                    summField = "Blank"
                    sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_" + summary_field.Trim + "=Equals!~!" + summary_field.Trim
                    sRefLink += "=BLANK!~!');"
                  End If
                Else
                  summField = "Blank/Unknown"
                  sRefLink = "javascript:ParseForm('0'," + bIsHistory.ToString.ToLower + ", false," + bIsCompany.ToString.ToLower + ", false,false,'COMPARE_" + summary_field.Trim + "=Equals!~!" + summary_field.Trim
                  sRefLink += "=IS NULL!~!');"
                End If
              End If

              sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view Aircraft List""")

              htmlOut.Append("<a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + summField.Trim + "</a></td><td align=""right"" valign=""middle"">" + FormatNumber(r.Item("tcount").ToString, 0).ToString + "</td></tr>")

              count1 += 1
              sum_total += CInt(r.Item("tcount").ToString)

            Next

            htmlOut.Append("<tr><td align=""left"" valign=""middle""><strong>Total : </strong></td>")
            htmlOut.Append("<td align=""right"" valign=""middle""><strong>" + FormatNumber(sum_total, 0).ToString + "</strong></td></tr>")

            htmlOut.Append("</table>")

          Else
            htmlOut.Append("<table id=""summaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No " + summary_field_display.Trim + " Summary Found</td></tr></table>")
          End If
        Else
          htmlOut.Append("<table id=""summaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No " + summary_field_display.Trim + " Summary Found</td></tr></table>")
        End If

      End If 'Not String.IsNullOrEmpty(temp_type.Trim) Then

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = "<font color=""red**> error in : Sum_AC_Based_On_Criteria(ByVal temp_type As String, ByVal display1 As String) As String</font><br/>" + ex.Message
    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Function get_folder_list(ByVal sub_type As String) As String
    get_folder_list = ""

    Dim Query As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoTempRS2 As System.Data.SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim last_display As String = ""
    Dim is_history As String = "0"

    Try
      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase") 'Application.Item("crmJetnetDatabase")
      ' SqlConn.ConnectionString = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=evolution;Password=vbs73az8"

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 180


      If Trim(Request("h")) = "1" Then
        is_history = "1"
      Else
        is_history = "0"
      End If

      Query = " select cefstab_sub_name, cef_display, cef_evo_field_name"
      Query += " from Custom_Export_Fields"
      Query += " inner join Custom_Export_Tab with (NOLOCK) on cef_export_tab_id = cefstab_id"
      Query += " where cef_sub_total_flag='Y'"

      Query += " and cefstab_main_name='Aircraft' "
      If Trim(sub_type) = "C" Then
        Query += "and cefstab_sub_name = 'Company/Contact'"
      Else
        Query += "and cefstab_sub_name <> 'Company/Contact'"
      End If

      If Session.Item("localPreferences").AerodexFlag = True Then
        Query = Query & " and cef_product_aerodex_flag='Y' "
      End If

      Query += " order by cefstab_order, cefstab_sub_name,cef_sort, cef_display, cef_evo_field_name"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>get_folder_list</b><br />" & Query


      SqlCommand.CommandText = Query
      adoTempRS2 = SqlCommand.ExecuteReader()

      If Not IsNothing(adoTempRS2) Then

        get_folder_list = "<table width='250' cellspacing='0' cellpadding='3' class='data_aircraft_grid'>"


        If Trim(Request("sub_type")) = "C" Then

        Else
          get_folder_list += "<tr class='header_row'>"
          get_folder_list += "<td align='left' valign='bottom' width='35'><img src='images/expanded.png' alt='' /></td>"
          get_folder_list += "<td align='left' valign='middle'><b class='title'><a href='searchsummary.aspx?h=" & is_history & "'>"
          If is_history = "1" Then
            get_folder_list += "Historical Market Summary"
          Else
            get_folder_list += "Mini Market Summary"
          End If
          get_folder_list += "</a></b></td></tr>"
        End If

        If Trim(Request("h")) = "1" Then
          get_folder_list += "<tr class='header_row'>"
          get_folder_list += "<td align='left' valign='bottom' width='35'><img src='images/expanded.png' alt='' /></td>"
          get_folder_list += "<td align='left' valign='middle'><b class='title'><a href='searchsummary.aspx?trans=" & is_history & "&h=1'>"
          get_folder_list += "Summary By Year/Month"
          get_folder_list += "</a></b></td></tr>"
        End If


        Do While adoTempRS2.Read

          If Trim(last_display) <> Trim(adoTempRS2("cefstab_sub_name")) Then
            get_folder_list += "<tr class='header_row'>"
            get_folder_list += "<td align='left' valign='bottom' width='35'><img src='images/expanded.png' alt='' /></td>"
            get_folder_list += "<td align='left' valign='middle'><b class='title'>" & adoTempRS2("cefstab_sub_name") & "</b></td>"
            get_folder_list += "</tr>"
          End If

          get_folder_list += "<tr>"
          get_folder_list += "<td align='right' valign='bottom'>&nbsp;&nbsp;<img src='" & DisplayFunctions.ReturnFolderImage("", "N", "N") & "' alt='' /></td>"
          get_folder_list += "<td align='left' valign='middle' nowrap='nowrap'>"
          get_folder_list += "<a href='searchsummary.aspx?type=" & adoTempRS2("cef_evo_field_name") & "&display=" & adoTempRS2("cef_display") & "&h=" & is_history & "&sub_type=" & sub_type & "'>"
          get_folder_list += adoTempRS2("cef_display")
          get_folder_list += "</a>"
          get_folder_list += "</td>"
          get_folder_list += "</tr>"

          last_display = adoTempRS2("cefstab_sub_name")
        Loop

        get_folder_list += "</table>"

      End If

      Session("SearchSummaryTreeHTML") = get_folder_list

      adoTempRS2.Dispose()
      adoTempRS2 = Nothing


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = "<font color=""red**> error in : get_folder_list(ByVal sub_type As String) As String</font><br/>" + ex.Message
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function

  'Private Sub BuildTopicArea()
  '  Dim ResultsTable As New DataTable
  '  Dim ResultsString As String = ""
  '  Dim HoldMainArea As String = ""
  '  Dim areaString As String = ""

  '  If Not String.IsNullOrEmpty(Trim(Request("area"))) Then
  '    areaString = Trim(Request("area"))
  '  End If


  '  ResultsTable = GetTopicListQueryByArea("", Session.Item("MasterAircraftWhere"), Session.Item("MasterAircraftFrom"))

  '  If Not IsNothing(ResultsTable) Then
  '    If ResultsTable.Rows.Count > 0 Then

  '      ResultsString = "<table width='250' cellspacing='0' cellpadding='3' class='data_aircraft_grid'>"

  '      For Each r As DataRow In ResultsTable.Rows

  '        If HoldMainArea <> r("AREA").ToString Then
  '          ResultsString += "<tr class='header_row'>"
  '          ResultsString += "<td align='left' valign='bottom' width='35'><img src='images/expand" & IIf(areaString = r("AREA"), "ed", "able") & ".png' alt='' /></td><td align='left' valign='middle'>" & IIf(areaString = r("AREA"), "", "<a href=""searchSummary.aspx?filter=true&area=" & r("AREA") & """>") & "<span class=""upper_header size_12"">" & r("AREA").ToString & "</span>" & IIf(areaString = r("AREA"), "", "</a>") & "</td>"
  '          ResultsString += "</tr>"
  '        End If

  '        HoldMainArea = r("AREA").ToString
  '      Next

  '      If areaString <> "" Then
  '        ResultsString += "<tr><td align=""right"" valign=""bottom"" colspan=""2""><a href=""searchSummary.aspx?filter=true"">View All</a></td></tr>"
  '      End If
  '      ResultsString += "</table>"

  '      holder_for_new_table.Text = ResultsString
  '    End If
  '  End If
  'End Sub
  '  -- ******************************************
  '-- TOPIC LIST QUERY - FOR ALL AIRCRAFT - BY AREA
  'select distinct actop_id as TOPID, actop_area as AREA, actop_name as TOPIC, COUNT(*) as tcount from aircraft_topic with (NOLOCK)
  'inner join aircraft_topic_index with (NOLOCK) on actopind_actop_id = actop_id
  'where actopind_ac_id in (select distinct ac_id from Aircraft_Flat with (NOLOCK) 
  'where ac_journ_id = 0 )
  'group by actop_id, actop_area,actop_name
  'order by actop_area,actop_name

  Private Sub BuildTopicAreaDisplay(ByVal displayLabel As Label, ByVal DisplayLetter As Boolean, ByVal DisplayArea As Boolean, ByVal displayFieldOne As String, ByVal displayFieldTwo As String)
    Dim ResultsTable As New DataTable
    Dim ResultsString As String = ""
    Dim HoldMainArea As String = ""
    Dim letter As String = ""
    Dim topicCount As Integer = 0
    Dim cssClass As String = ""
    Dim areaString As String = ""
    Dim ItemUnderCount As Integer = 0

    If Not String.IsNullOrEmpty(Trim(Request("area"))) Then
      areaString = Trim(Request("area"))
      summary_tab.HeaderText = areaString & " Topics"
    End If


    If DisplayLetter Then
      ResultsTable = Master.aclsData_Temp.GetTopicListQueryByLetter(areaString, letter, Session.Item("MasterAircraftWhere"), Session.Item("MasterAircraftFrom"))
    ElseIf DisplayArea Then
      ResultsTable = Master.aclsData_Temp.GetTopicListQueryByArea("", Session.Item("MasterAircraftWhere"), Session.Item("MasterAircraftFrom"))
    End If

    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then

        ResultsString = "<table width='100%' cellspacing='0' cellpadding='3' class='data_aircraft_grid medium_text'>"

        For Each r As DataRow In ResultsTable.Rows

          If HoldMainArea <> r(displayFieldOne).ToString Then

            If ItemUnderCount = 2 Then
              ResultsString += "<td align='left' valign='top' width=""33%"" class=""override_borders""></td>"
            End If

            ItemUnderCount = 0

            If cssClass = "" Then
              cssClass = "dataListGray"
            Else
              cssClass = ""
            End If

            If HoldMainArea <> "" Then
              ResultsString += "</table></td></tr>"
            End If

            ResultsString += "<tr>"
            topicCount = 0
            ResultsString += "<td align='left' valign='top' class=""" & cssClass & " override_borders""><span class=""upper_header medium_text""><b>" & r(displayFieldOne).ToString & "</b></span></td>"
            ResultsString += "</tr>"

            ResultsString += "<tr><td align=""left"" valign=""top"" class=""" & cssClass & " override_borders""><table width=""100%"" cellpadding=""0"" cellspacing=""0"">"
          End If


          If topicCount = 3 Then
            ResultsString += "</tr><tr>"
            topicCount = 0
          End If

          ResultsString += "<td align='left' valign='top' width=""33%"" class=""override_borders"">"
          ResultsString += "<span class=""padding_topic_list""><input type=""checkbox"">&nbsp;" & r(displayFieldTwo).ToString & " (" & r("tcount").ToString & ")</span></td>"

          HoldMainArea = r(displayFieldOne).ToString
          topicCount += 1
          ItemUnderCount += 1
        Next

        ResultsString += "</table></td></tr>"

        ResultsString += "</table>"
        displayLabel.Text = ResultsString
      End If
    End If
  End Sub

  Public Function Make_Transaction_History() As String
    Make_Transaction_History = ""

    Dim Query As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoTempRS As New DataTable
    Dim adoTempRS2 As System.Data.SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim last_display As String = ""
    Dim count1 As Integer = 0
    Dim sum_total As Integer = 0
    Dim fleetHtmlOut As New StringBuilder
    Dim alt_row As Boolean = False
    Dim temp_date As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 180

      Query = "SELECT DISTINCT year(journ_date) AS tyear, month(journ_date) AS tmonth, count(*) AS tcount "

      If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftFrom")) Then
        If Trim(HttpContext.Current.Session.Item("MasterAircraftFrom")) <> "" Then
          Query += HttpContext.Current.Session.Item("MasterAircraftFrom").ToString
          Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
        Else
          Query += " from View_Aircraft_History_Flat WITH(NOLOCK) "
          Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
        End If
      Else
        Query += " from View_Aircraft_History_Flat WITH(NOLOCK) "
        Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
      End If

      Query += " group by year(journ_date), month(journ_date) "
      Query += " order by year(journ_date) asc , month(journ_date) asc "

      SqlCommand.CommandText = Query
      adoTempRS2 = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        adoTempRS.Load(adoTempRS2)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = adoTempRS.GetErrors()
      End Try


      If Not IsNothing(adoTempRS) Then

        fleetHtmlOut.Append("<table id='fleetTable' cellpadding='2' cellspacing='0' width='40%'>")
        fleetHtmlOut.Append("<tr>")
        fleetHtmlOut.Append("<td align='right' valign='top' class='FleetMarket_Left_TD' width='50%'><table id='ownershipTable' cellspacing='0' cellpadding='2' width='100%' class='data_aircraft_grid'>")
        fleetHtmlOut.Append("<tr class='header_row'><td valign='middle' align='center' colspan='2'><strong>&nbsp;Summary By Year/Month&nbsp;</strong></td></tr>")

        fleetHtmlOut.Append("<tr class='header_row'>")
        fleetHtmlOut.Append("<td align='center' valign='top' nowrap='nowrap'><strong>Month / Year</strong></td><td align='right' valign='middle' width='100'><strong>Count</strong></td>")
        fleetHtmlOut.Append("</tr>")


        If adoTempRS.Rows.Count > 0 Then
          For Each r As DataRow In adoTempRS.Rows

            If alt_row = True Then
              fleetHtmlOut.Append("<tr class='alt_row'>")
              alt_row = False
            Else
              fleetHtmlOut.Append("<tr>")
              alt_row = True
            End If

            temp_date = r("tmonth") & "/01/" & r("tyear")

            fleetHtmlOut.Append("<td valign='top' align='right'>")
            fleetHtmlOut.Append("<a  onclick=""javascript:ParseForm('0', 1, false, 0, false, false,'COMPARE_journ_date_operator=Between!~!ac_ownership_type!~!journ_date=" & temp_date & ":" & CStr(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(temp_date)))) & "')"">")
            fleetHtmlOut.Append(r("tmonth") & "/" & r("tyear"))
            fleetHtmlOut.Append("</a>")
            fleetHtmlOut.Append("&nbsp;</td><td align='right'>")
            fleetHtmlOut.Append(r("tcount"))
            fleetHtmlOut.Append("&nbsp;</td></tr>")

          Next
        End If


        fleetHtmlOut.Append("</table>")
        fleetHtmlOut.Append("</td></tr></table>")


        summary_table_label.Text = fleetHtmlOut.ToString
      End If


      fleetHtmlOut = Nothing
      adoTempRS2.Dispose()
      adoTempRS2 = Nothing

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = "<font color=""red**> error in : Make_Transaction_History() As String</font><br/>" + ex.Message
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
  End Function

  Public Sub Make_AC_Mini_Market()

    Dim Query As String = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoTempRS As New DataTable
    Dim adoTempRS2 As System.Data.SqlClient.SqlDataReader : adoTempRS2 = Nothing
    Dim last_display As String = ""
    Dim count1 As Integer = 0
    Dim sum_total As Integer = 0

    Dim bIsCompany As Boolean = False
    Dim bIsHistory As Boolean = False
    Dim bIsEvents As Boolean = False
    Dim bIsAircraft As Boolean = False

    Try

      If Not IsNothing(Request("sub_type")) Then
        If Not String.IsNullOrEmpty(Request("sub_type").ToString.Trim) Then
          If Request("sub_type").ToString.ToUpper.Contains("C") Then
            bIsCompany = True
          End If
        End If
      End If

      If Not IsNothing(Request("h")) Then
        If Not String.IsNullOrEmpty(Request("h").ToString.Trim) Then
          If Request("h").ToString.ToUpper.Contains("1") Then
            bIsHistory = True
          End If
        End If
      End If

      If Not IsNothing(Request("e")) Then
        If Not String.IsNullOrEmpty(Request("e").ToString.Trim) Then
          If Request("e").ToString.ToUpper.Contains("1") Then
            bIsEvents = True
          End If
        End If
      End If

      If Not bIsCompany And Not bIsHistory And Not bIsEvents Then
        bIsAircraft = True
      End If

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 180

      Query += ("SELECT ac_id, ac_ownership_type, ac_lifecycle_stage, ac_forsale_flag, ac_exclusive_flag,")
      Query += (" ac_lease_flag, ac_asking, ac_asking_price, ac_list_date, ac_mfr_year, DATEDIFF(d,ac_list_date,getdate()) AS daysonmarket")

      If bIsHistory Then

        If HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("comp_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("contact_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("cref_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("state_") Then
          Query += " FROM View_Aircraft_Company_History_Flat WITH(NOLOCK)"
        Else
          Query += " FROM View_Aircraft_History_Flat WITH(NOLOCK)"
        End If

        Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString

      ElseIf bIsEvents Then

        If (InStr(UCase(HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString()), "LEFT OUTER JOIN COMPANY") > 0) Then
          Query += HttpContext.Current.Session.Item("MasterAircraftEventsFrom").ToString()
        Else
          Query += " FROM Priority_Events WITH(NOLOCK) "
          Query += " INNER JOIN Priority_Events_Category WITH(NOLOCK) ON priorev_category_code=priorevcat_category_code"
          Query += " LEFT OUTER JOIN View_Aircraft_Flat WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0) "

        End If

        Query += " WHERE "
        Query += HttpContext.Current.Session.Item("MasterAircraftEventsWhere").ToString()

      Else

        If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftFrom")) Then
          If Trim(HttpContext.Current.Session.Item("MasterAircraftFrom")) <> "" Then
            Query += HttpContext.Current.Session.Item("MasterAircraftFrom").ToString
            Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
          Else
            Query += " from View_Aircraft_Flat WITH(NOLOCK) "
            Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
          End If
        Else
          Query += " from View_Aircraft_Flat WITH(NOLOCK) "
          Query += HttpContext.Current.Session.Item("MasterAircraftWhere").ToString
        End If

      End If

      SqlCommand.CommandText = Query
      adoTempRS2 = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        adoTempRS.Load(adoTempRS2)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = adoTempRS.GetErrors()
      End Try

      If Not IsNothing(adoTempRS2) Then
        summary_table_label.Text = views_display_fleet_market_summary(adoTempRS)
      End If


      adoTempRS2.Dispose()
      adoTempRS2 = Nothing

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<font color=""red**> error in : Make_AC_Mini_Market() As String</font><br/>" + ex.Message
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try

  End Sub

  Public Function views_display_fleet_market_summary(ByVal results_table As DataTable) As String
    views_display_fleet_market_summary = ""

    Dim fleetHtmlOut As New StringBuilder
    Dim marketHtmlOut As New StringBuilder


    Dim string_for_op_percentage = ""

    Dim avgyear As Integer = 0
    Dim avgyearcount As Integer = 0

    Dim totalcount As Integer = 0
    Dim totalInOpcount As Integer = 0
    Dim ac_for_sale As Integer = 0
    Dim ac_exclusive_sale As Integer = 0
    Dim ac_lease As Integer = 0

    Dim w_owner As Integer = 0
    Dim s_owner As Integer = 0
    Dim f_owner As Integer = 0
    Dim o_stage As Integer = 0
    Dim t_stage As Integer = 0
    Dim th_stage As Integer = 0
    Dim f_stage As Integer = 0

    Dim daysonmarket As Integer = 0
    Dim daysonmarket2 As Integer = 0
    Dim days As Integer = 0

    Dim allhigh As Integer = 0
    Dim alllow As Integer = 0

    Dim forsaleavghigh As Double = 0.0
    Dim forsaleavlow As Double = 0.0

    Dim per As Double = 0
    Dim per2 As Double = 0
    Dim per3 As Double = 0

    Try

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("daysonmarket")) Then
              If CLng(r.Item("daysonmarket").ToString) > 0 Then
                daysonmarket += 1
                daysonmarket2 += CLng(r.Item("daysonmarket").ToString)
              End If
            End If

            If Not IsDBNull(r("ac_mfr_year")) Then
              If IsNumeric(r("ac_mfr_year").ToString) Then

                If CInt(r("ac_mfr_year").ToString) > 0 Then

                  If allhigh = 0 Or CInt(r.Item("ac_mfr_year").ToString) > allhigh Then
                    allhigh = CInt(r.Item("ac_mfr_year").ToString)
                  End If

                  If alllow = 0 Or CInt(r.Item("ac_mfr_year").ToString) < alllow Then
                    alllow = CInt(r.Item("ac_mfr_year").ToString)
                  End If

                End If
              End If
            End If

            totalcount += 1

            If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
              totalInOpcount += 1
            End If

            If r.Item("ac_ownership_type").ToString.ToUpper = "W" And r.Item("ac_lifecycle_stage").ToString = "3" Then
              w_owner += 1
            End If

            If r.Item("ac_ownership_type").ToString.ToUpper = "F" And r.Item("ac_lifecycle_stage").ToString = "3" Then
              f_owner += 1
            End If

            If r.Item("ac_ownership_type").ToString.ToUpper = "S" And r.Item("ac_lifecycle_stage").ToString = "3" Then
              s_owner += 1
            End If

            If r.Item("ac_lifecycle_stage").ToString = "1" Then
              o_stage += 1
            End If

            If r.Item("ac_lifecycle_stage").ToString = "2" Then
              t_stage += 1
            End If

            If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
              th_stage += 1
            End If

            If r.Item("ac_lifecycle_stage").ToString = "4" Then
              f_stage += 1
            End If

            If r.Item("ac_forsale_flag").ToString.ToUpper = "Y" Then

              ac_for_sale += 1

              If Not IsDBNull(r("ac_asking_price")) Then
                If Not String.IsNullOrEmpty(r.Item("ac_asking_price").ToString) Then

                  If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                    If forsaleavghigh = 0 Or CDbl(r.Item("ac_asking_price").ToString) > forsaleavghigh Then
                      forsaleavghigh = CDbl(r.Item("ac_asking_price").ToString)
                    End If

                    If forsaleavlow = 0 Or (CDbl(r.Item("ac_asking_price").ToString) < forsaleavlow) Then
                      forsaleavlow = CDbl(r.Item("ac_asking_price").ToString)
                    End If

                  End If

                End If
              End If
            End If

            If Not IsDBNull(r("ac_exclusive_flag")) Then
              If r.Item("ac_exclusive_flag").ToString.ToUpper = "Y" Then
                ac_exclusive_sale += 1
              End If
            End If

            If Not IsDBNull(r("ac_lease_flag")) Then
              If r.Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                ac_lease += 1
              End If
            End If

          Next

        End If
      End If

      If (forsaleavlow > 0) Then
        forsaleavlow = CDbl(forsaleavlow / 1000)
      End If

      If (forsaleavghigh > 0) Then
        forsaleavghigh = CDbl(forsaleavghigh / 1000)
      End If

      If (ac_for_sale > 0 And th_stage > 0) Then

        per = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
        per2 = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100), 1)
        per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

        If daysonmarket > 0 Then
          days = System.Math.Round(CLng(daysonmarket2) / CLng(daysonmarket))
        Else
          days = System.Math.Round(CLng(daysonmarket2))
        End If

      End If

      If (alllow >= 0 And allhigh > 0) Then
        For i As Integer = alllow To allhigh
          avgyear += i
          avgyearcount += 1
        Next
      End If

      If avgyear > 0 And avgyearcount > 0 Then
        avgyear = CLng(avgyear / avgyearcount)
      End If

      string_for_op_percentage = "&nbsp;<span class='tiny'>(" + FormatNumber(per, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span>"

      ' start outer table
      fleetHtmlOut.Append("<table id='fleetTable' cellpadding='2' cellspacing='0' width='100%'" + IIf(HttpContext.Current.Session.Item("lastView") <> 16, " class='module'", "") + ">")
      fleetHtmlOut.Append("<tr>")

      ' Ownership table
      fleetHtmlOut.Append("<td align='right' valign='top' class='FleetMarket_Left_TD' width='50%'><table id='ownershipTable' cellspacing='0' cellpadding='2' width='100%' class='data_aircraft_grid'>")
      fleetHtmlOut.Append("<tr class='header_row'><td valign='middle' align='center' colspan='2'><strong>&nbsp;Ownership&nbsp;(In&nbsp;Operation)&nbsp;</strong></td></tr>")

      If w_owner > 0 Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>" + FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If s_owner > 0 Then
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If f_owner > 0 Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If totalInOpcount > 0 Then
        fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If (alllow > 0) And (allhigh > 0) And (allhigh <> CInt(Now().Year)) Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - " + allhigh.ToString + "</td></tr>")
      ElseIf (alllow > 0) And (allhigh = CInt(Now().Year)) Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - To Present</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range&nbsp;:&nbsp;N/A</td></tr>")
      End If

      fleetHtmlOut.Append("</table></td>")

      ' Fleet Info
      fleetHtmlOut.Append("<td align='left' width='50%' valign='top'>")
      fleetHtmlOut.Append("<table id='lifeCycleTable' width='100%' cellspacing='0' cellpadding='2' class='data_aircraft_grid'>")
      fleetHtmlOut.Append("<tr class='header_row'><td valign='top' align='center' colspan='2'><strong>Fleet By Life Cycle</strong></td></tr>")

      If o_stage > 0 Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(o_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If t_stage > 0 Then
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(t_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If th_stage > 0 Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(th_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If f_stage > 0 Then
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
      End If

      If totalcount > 0 Then
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td class='border_bottom' align='right'>&nbsp;" + FormatNumber(totalcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
      Else
        fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td class='border_bottom' align='right'>&nbsp;0</td></tr>")
      End If

      fleetHtmlOut.Append("</table>")
      fleetHtmlOut.Append("</td></tr></table>")


      If Session.Item("localPreferences").AerodexFlag = False Then
        marketHtmlOut.Append("<table  width='100%' cellspacing='0' cellpadding='4' valign='top' class='data_aircraft_grid'>")
        marketHtmlOut.Append("<tr class='header_row'><td valign='top' align='center' colspan='2'><strong>Market Status</strong></td></tr>")

        If ac_for_sale > 0 Then
          marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>For Sale&nbsp;</td><td valign='top' align='left' class='rightside'>" + FormatNumber(ac_for_sale, 0, TriState.False, TriState.False, TriState.True).ToString + string_for_op_percentage + "</td></tr>")
        Else
          marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>For Sale:&nbsp;</td><td align='left' class='rightside'>0&nbsp;<span class='tiny'>(0% of For Sale)</span></td></tr>")
        End If

        If forsaleavlow > 0 Or forsaleavghigh > 0 Then
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Asking Price Range:&nbsp;</td><td align='left' nowrap='nowrap' class='rightside'>$" + FormatNumber(forsaleavlow.ToString, 0) + "k - $" + FormatNumber(forsaleavghigh.ToString, 0) + "k</td></tr>")
        Else
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Asking Price Range:&nbsp;</td><td align='left' class='rightside'>No Asking Prices</td></tr>")
        End If

        If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
          If CLng(ac_exclusive_sale) > 0 Then
            marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>On Exclusive:&nbsp;</td><td align='left' class='rightside'>" + FormatNumber(ac_exclusive_sale, 0, TriState.False, TriState.False, TriState.True).ToString + " <span class='tiny'>(" + FormatNumber(per2, 1, TriState.False, TriState.False, TriState.True).ToString + "% For Sale on Exclusive)</span></td></tr>")
          Else
            marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>On Exclusive:&nbsp;</td><td align='left' class='rightside'>0&nbsp;<span class='tiny'>(0% For Sale on Exclusive)</span></td></tr>")
          End If
        End If

        If avgyear > 0 Then
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Avg MFG Year:&nbsp;</td><td align='left' class='rightside'>" + avgyear.ToString + "</td></tr>")
        Else
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Avg MFG Year:&nbsp;</td><td align='left' class='rightside'>N/A</td></tr>")
        End If

        If days > 0 Then
          marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Avg Days on Market:&nbsp;</td><td align='left' class='rightside'>" + days.ToString + "</td></tr>")
        Else
          marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Avg Days on Market:&nbsp;</td><td align='left' class='rightside'>N/A</td></tr>")
        End If

        If ac_lease > 0 Then
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='border_bottom'>Leased:&nbsp;</td><td align='left' class='border_bottom'>" + FormatNumber(ac_lease, 0, TriState.False, TriState.False, TriState.True).ToString + "&nbsp;<span class='tiny'>(" + FormatNumber(per3, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span></td></tr>")
        Else
          marketHtmlOut.Append("<tr><td valign='top' align='left' class='border_bottom'>Leased:&nbsp;</td><td align='left' class='border_bottom'>0&nbsp;<span class='tiny'>(0% of In Operation)</span></td></tr>")
        End If

        marketHtmlOut.Append("</table>")
      End If

      views_display_fleet_market_summary = marketHtmlOut.ToString.Trim & "<br>"
      views_display_fleet_market_summary += fleetHtmlOut.ToString.Trim


    Catch ex As Exception

      ' aError = "Error in views_Build_FleetMarketSummary(ByVal in_nModelID As Long, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

    Finally

    End Try

    fleetHtmlOut = Nothing
    marketHtmlOut = Nothing
    results_table = Nothing

  End Function

  Public Function make_exec_summary() As String

    Dim htmlOut As New StringBuilder
    Dim sQuery As New StringBuilder()

    Dim Total_Leased As Integer = 0
    Dim Total_Available As Integer = 0
    Dim Total_Fractional As Integer = 0
    Dim Total_Whole As Integer = 0
    Dim Total_Shared As Integer = 0
    Dim Total_Other As Integer = 0

    Dim Total_Dealer_Owned As Integer = 0
    Dim Total_User_Owned As Integer = 0
    Dim Total_Exclusive As Integer = 0
    Dim Total_Domestic As Integer = 0
    Dim Total_International As Integer = 0

    Dim Total_ForSale_Whole As Integer = 0
    Dim Total_ForSale_Shared As Integer = 0
    Dim Total_ForSale_Fractional As Integer = 0

    Dim Total_Leased_Whole As Integer = 0
    Dim Total_Leased_Shared As Integer = 0
    Dim Total_Leased_Fractional As Integer = 0

    Dim Total_Domestic_Whole As Integer = 0
    Dim Total_Domestic_Shared As Integer = 0
    Dim Total_Domestic_Fractional As Integer = 0

    Dim Total_International_Whole As Integer = 0
    Dim Total_International_Shared As Integer = 0
    Dim Total_International_Fractional As Integer = 0

    Dim Total_EndUser_Whole As Integer = 0
    Dim Total_EndUser_Shared As Integer = 0
    Dim Total_EndUser_Fractional As Integer = 0

    Dim Total_EndUserExclusive_Whole As Integer = 0
    Dim Total_EndUserExclusive_Shared As Integer = 0
    Dim Total_EndUserExclusive_Fractional As Integer = 0

    Dim Total_Dealer_Whole As Integer = 0
    Dim Total_Dealer_Shared As Integer = 0
    Dim Total_Dealer_Fractional As Integer = 0

    Dim Total_Other_Whole As Integer = 0
    Dim Total_Other_Shared As Integer = 0
    Dim Total_Other_Fractional As Integer = 0

    Dim AFTT_Low As Long = 0
    Dim AFTT_High As Long = 0
    Dim AFTT_Count As Long = 0
    Dim AFTT_Total As Long = 0
    Dim AFTT_Avg As Long = 0

    Dim YearMFG_Low As Long = 0
    Dim YearMFG_High As Long = 0
    Dim YearMFG_Count As Long = 0
    Dim YearMFG_Total As Long = 0
    Dim YearMFG_Avg As Long = 0

    Dim Asking_Low As Long = 0
    Dim Asking_High As Long = 0
    Dim Asking_Count As Long = 0
    Dim Asking_Total As Long = 0
    Dim Asking_Avg As Long = 0

    Dim Landings_Low As Long = 0
    Dim Landings_High As Long = 0
    Dim Landings_Count As Long = 0
    Dim Landings_Total As Long = 0
    Dim Landings_Avg As Long = 0

    Dim EngTT_Low As Long = 0
    Dim EngTT_High As Long = 0
    Dim EngTT_Count As Long = 0
    Dim EngTT_Total As Long = 0
    Dim EngTT_Avg As Long = 0

    Dim DaysOnMarket_Low As Long = 0
    Dim DaysOnMarket_High As Long = 0
    Dim DaysOnMarket_Count As Long = 0
    Dim DaysOnMarket_Total As Long = 0
    Dim DaysOnMarket_Avg As Long = 0

    Dim nCount As Integer = 0
    Dim tempTitle As String = ""

    Dim strRegNbr As String = ""
    Dim strExclusiveFlag As String = ""
    Dim strOwnershipType As String = ""
    Dim strCompCountry As String = ""
    Dim strCrefBusinessType As String = ""

    Dim tempDaysOnMarket As Integer = 0

    Dim dtListDate As DateTime = Now()

    Dim tempEngCount As Integer = 0
    Dim tempEngTotal As Double = 0.0

    Dim EngTotalHrs_1 As Double = 0.0
    Dim EngTotalHrs_2 As Double = 0.0
    Dim EngTotalHrs_3 As Double = 0.0
    Dim EngTotalHrs_4 As Double = 0.0

    Dim nAFTT As Double = 0.0
    Dim nYearMFG As Double = 0.0
    Dim nLandings As Double = 0.0
    Dim nAsking As Double = 0.0

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim execSummary_datatable As New DataTable
    Dim adoRS_execSummary As System.Data.SqlClient.SqlDataReader : adoRS_execSummary = Nothing

    Dim bIsCompany As Boolean = False
    Dim bIsHistory As Boolean = False
    Dim bIsEvents As Boolean = False
    Dim bIsAircraft As Boolean = False

    Try

      If Not IsNothing(Request("sub_type")) Then
        If Not String.IsNullOrEmpty(Request("sub_type").ToString.Trim) Then
          If Request("sub_type").ToString.ToUpper.Contains("C") Then
            bIsCompany = True
          End If
        End If
      End If

      If Not IsNothing(Request("h")) Then
        If Not String.IsNullOrEmpty(Request("h").ToString.Trim) Then
          If Request("h").ToString.ToUpper.Contains("1") Then
            bIsHistory = True
          End If
        End If
      End If

      If Not IsNothing(Request("e")) Then
        If Not String.IsNullOrEmpty(Request("e").ToString.Trim) Then
          If Request("e").ToString.ToUpper.Contains("1") Then
            bIsEvents = True
          End If
        End If
      End If

      If Not bIsCompany And Not bIsHistory And Not bIsEvents Then
        bIsAircraft = True
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 180

      sQuery.Append("SELECT ac_id, ac_reg_no, ac_lease_flag, ac_forsale_flag, ac_ownership_type, ac_list_date, ac_forsale_flag, ac_airframe_tot_hrs, ac_mfr_year ,ac_asking_price,")
      sQuery.Append(" ac_airframe_tot_landings, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_exclusive_flag, cref_business_type, comp_country")

      sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftFrom").ToString)

      If bIsHistory Then

        If HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("comp_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("contact_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("cref_") Or _
           HttpContext.Current.Session.Item("MasterAircraftWhere").ToString.ToLower.Contains("state_") Then
          sQuery.Append(" FROM View_Aircraft_Company_History_Flat WITH(NOLOCK) ")
        Else
          sQuery.Append(" FROM View_Aircraft_History_Flat WITH(NOLOCK) ")
        End If

      End If

      sQuery.Append(" INNER LOOP JOIN Aircraft_Reference WITH(NOLOCK) ON (cref_ac_id = ac_id) AND (cref_journ_id = 0) AND cref_contact_type IN ('00','97','17','08','56')")
      sQuery.Append(" INNER LOOP JOIN Company WITH(NOLOCK) ON cref_comp_id = comp_id AND cref_journ_id = comp_journ_id ")

      sQuery.Append(HttpContext.Current.Session.Item("MasterAircraftWhere").ToString)

      sQuery.Append(" ORDER BY amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />make_exec_summary()<br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      adoRS_execSummary = SqlCommand.ExecuteReader()

      Try
        execSummary_datatable.Load(adoRS_execSummary)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = execSummary_datatable.GetErrors()
      End Try

      adoRS_execSummary.Close()
      adoRS_execSummary = Nothing

      If execSummary_datatable.Rows.Count > 0 Then

        For Each Row As DataRow In execSummary_datatable.Rows

          nCount += 1

          strOwnershipType = ""
          If Not IsDBNull(Row.Item("ac_ownership_type")) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_ownership_type").ToString.Trim) Then
              strOwnershipType = Row.Item("ac_ownership_type").ToString.Trim
            End If
          End If

          If Not (IsDBNull(Row.Item("ac_lease_flag"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_lease_flag").ToString.Trim) Then

              If Row.Item("ac_lease_flag").ToString.ToUpper.Contains("Y") Then

                Total_Leased += 1

                Select Case strOwnershipType.ToUpper.Trim
                  Case "W"
                    Total_Leased_Whole += 1
                  Case "S"
                    Total_Leased_Shared += 1
                  Case "F"
                    Total_Leased_Fractional += 1
                End Select

              End If

            Else
              Total_Leased = 0
            End If
          Else
            Total_Leased = 0
          End If

          If Not (IsDBNull(Row.Item("ac_forsale_flag"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_forsale_flag").ToString.Trim) Then

              If Row.Item("ac_forsale_flag").ToString.ToUpper.Contains("Y") Then

                Total_Available += 1

                Select Case strOwnershipType.ToUpper.Trim
                  Case "W"
                    Total_ForSale_Whole += 1
                  Case "S"
                    Total_ForSale_Shared += 1
                  Case "F" ' we keep this in here for consistancy but its N/A for display
                    Total_ForSale_Fractional += 1
                End Select

                tempDaysOnMarket = 0

                If Not (IsDBNull(Row.Item("ac_list_date"))) Then
                  If Not String.IsNullOrEmpty(Row.Item("ac_list_date").ToString.Trim) And IsDate(Row.Item("ac_list_date").ToString) Then

                    dtListDate = CDate(Row.Item("ac_list_date").ToString.Trim)

                    tempDaysOnMarket = DateDiff("d", dtListDate, Now())
                    DaysOnMarket_Count += 1
                    DaysOnMarket_Total += CLng(tempDaysOnMarket)

                    If DaysOnMarket_Low = 0 Or CLng(tempDaysOnMarket) < DaysOnMarket_Low Then
                      DaysOnMarket_Low = CLng(tempDaysOnMarket)
                    End If

                    If DaysOnMarket_High = 0 Or CLng(tempDaysOnMarket) > DaysOnMarket_High Then
                      DaysOnMarket_High = CLng(tempDaysOnMarket)
                    End If

                  End If
                End If

              End If

            Else
              Total_Available = 0
            End If
          Else
            Total_Available = 0
          End If


          If strOwnershipType.ToUpper.Contains("W") Then
            Total_Whole += 1
          End If

          If strOwnershipType.ToUpper.Contains("F") Then
            Total_Fractional += 1
          End If

          If strOwnershipType.ToUpper.Contains("S") Then
            Total_Shared += 1
          End If

          ' ************* AFTT ***************************
          If Not (IsDBNull(Row.Item("ac_airframe_tot_hrs"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_airframe_tot_hrs").ToString.Trim) Then

              nAFTT = CDbl(Row.Item("ac_airframe_tot_hrs").ToString)

              If nAFTT > 0 Then
                AFTT_Count += 1
                AFTT_Total += CLng(nAFTT)

                If AFTT_Low = 0 Or CLng(nAFTT) < AFTT_Low Then
                  AFTT_Low = CLng(nAFTT)
                End If

                If AFTT_High = 0 Or CLng(nAFTT) > AFTT_High Then
                  AFTT_High = CLng(nAFTT)
                End If

              End If
            End If
          End If

          ' ************* YEAR MFG *************************** 
          If Not (IsDBNull(Row.Item("ac_mfr_year"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_mfr_year").ToString.Trim) Then

              nYearMFG = CDbl(Row.Item("ac_mfr_year").ToString)

              If nYearMFG > 0 Then
                YearMFG_Count += 1
                YearMFG_Total += CLng(nYearMFG)

                If YearMFG_Low = 0 Or CLng(nYearMFG) < YearMFG_Low Then
                  YearMFG_Low = CLng(nYearMFG)
                End If

                If YearMFG_High = 0 Or CLng(nYearMFG) > YearMFG_High Then
                  YearMFG_High = CLng(nYearMFG)
                End If

              End If
            End If
          End If

          ' ************* ASKING PRICE  *************************** 
          If Not (IsDBNull(Row.Item("ac_asking_price"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_asking_price").ToString.Trim) Then

              nAsking = CDbl(Row.Item("ac_asking_price").ToString)

              If nAsking > 0 Then
                Asking_Count += 1
                Asking_Total += CInt(nAsking)

                If Asking_Low = 0 Or CLng(nAsking) < Asking_Low Then
                  Asking_Low = CLng(nAsking)
                End If

                If Asking_High = 0 Or CLng(nAsking) > Asking_High Then
                  Asking_High = CLng(nAsking)
                End If

              End If
            End If
          End If

          ' ************* LANDINGS  ***************************    
          If Not (IsDBNull(Row.Item("ac_airframe_tot_landings"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_airframe_tot_landings").ToString.Trim) Then

              nLandings = CDbl(Row.Item("ac_airframe_tot_landings").ToString)

              If nLandings > 0 Then
                Landings_Count += 1
                Landings_Total += CLng(nLandings)

                If Landings_Low = 0 Or CLng(nLandings) < Landings_Low Then
                  Landings_Low = CInt(nLandings)
                End If

                If Landings_High = 0 Or CLng(nLandings) > Landings_High Then
                  Landings_High = CLng(nLandings)
                End If

              End If
            End If
          End If

          ' ************* ENG_TT  ***************************
          EngTotalHrs_1 = 0
          EngTotalHrs_2 = 0
          EngTotalHrs_3 = 0
          EngTotalHrs_4 = 0

          tempEngCount = 0
          tempEngTotal = 0

          If Not (IsDBNull(Row.Item("ac_engine_1_tot_hrs"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_engine_1_tot_hrs").ToString.Trim) Then

              EngTotalHrs_1 = CDbl(Row.Item("ac_engine_1_tot_hrs").ToString)
              If EngTotalHrs_1 > 0 Then
                tempEngCount += 1
                tempEngTotal += EngTotalHrs_1
              End If

            End If
          End If

          If Not (IsDBNull(Row.Item("ac_engine_2_tot_hrs"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_engine_2_tot_hrs").ToString.Trim) Then

              EngTotalHrs_2 = CDbl(Row.Item("ac_engine_2_tot_hrs").ToString)
              If EngTotalHrs_2 > 0 Then
                tempEngCount += 1
                tempEngTotal += EngTotalHrs_2
              End If

            End If
          End If

          If Not (IsDBNull(Row.Item("ac_engine_3_tot_hrs"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_engine_3_tot_hrs").ToString.Trim) Then

              EngTotalHrs_3 = CDbl(Row.Item("ac_engine_3_tot_hrs").ToString)
              If EngTotalHrs_3 > 0 Then
                tempEngCount += 1
                tempEngTotal += EngTotalHrs_3
              End If

            End If
          End If

          If Not (IsDBNull(Row.Item("ac_engine_4_tot_hrs"))) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_engine_4_tot_hrs").ToString.Trim) Then

              EngTotalHrs_4 = CDbl(Row.Item("ac_engine_4_tot_hrs").ToString)
              If EngTotalHrs_4 > 0 Then
                tempEngCount += 1
                tempEngTotal += EngTotalHrs_4
              End If

            End If
          End If

          If tempEngCount > 0 Then
            tempEngTotal = tempEngTotal / tempEngCount

            EngTT_Count += 1
            EngTT_Total += CLng(tempEngTotal)

            If (EngTT_Low = 0 Or CLng(EngTotalHrs_1) < EngTT_Low And EngTotalHrs_1 > 0) Then
              EngTT_Low = CLng(EngTotalHrs_1)
            End If

            If (EngTT_Low = 0 Or CLng(EngTotalHrs_2) < EngTT_Low And EngTotalHrs_2 > 0) Then
              EngTT_Low = CLng(EngTotalHrs_2)
            End If

            If (EngTT_Low = 0 Or CLng(EngTotalHrs_3) < EngTT_Low And EngTotalHrs_3 > 0) Then
              EngTT_Low = CLng(EngTotalHrs_3)
            End If

            If (EngTT_Low = 0 Or CLng(EngTotalHrs_4) < EngTT_Low And EngTotalHrs_4 > 0) Then
              EngTT_Low = CLng(EngTotalHrs_4)
            End If

            If (EngTT_High = 0 Or CLng(EngTotalHrs_1) > EngTT_High) Then
              EngTT_High = CLng(EngTotalHrs_1)
            End If

            If (EngTT_High = 0 Or CLng(EngTotalHrs_2) > EngTT_High) Then
              EngTT_High = CLng(EngTotalHrs_2)
            End If

            If (EngTT_High = 0 Or CLng(EngTotalHrs_3) > EngTT_High) Then
              EngTT_High = CLng(EngTotalHrs_3)
            End If

            If (EngTT_High = 0 Or CLng(EngTotalHrs_4) > EngTT_High) Then
              EngTT_High = CLng(EngTotalHrs_4)
            End If

          End If ' tempEngCount > 0     

          strRegNbr = ""
          If Not IsDBNull(Row.Item("ac_reg_no")) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_reg_no").ToString.Trim) Then
              strRegNbr = Row.Item("ac_reg_no").ToString.Trim
            End If
          End If

          strExclusiveFlag = ""
          If Not IsDBNull(Row.Item("ac_exclusive_flag")) Then
            If Not String.IsNullOrEmpty(Row.Item("ac_exclusive_flag").ToString.Trim) Then
              strExclusiveFlag = Row.Item("ac_exclusive_flag").ToString.Trim
            End If
          End If

          strCompCountry = ""
          If Not IsDBNull(Row.Item("comp_country")) Then
            If Not String.IsNullOrEmpty(Row.Item("comp_country").ToString.Trim) Then
              strCompCountry = Row.Item("comp_country").ToString.Trim
            End If
          End If

          strCrefBusinessType = ""
          If Not IsDBNull(Row.Item("cref_business_type")) Then
            If Not String.IsNullOrEmpty(Row.Item("cref_business_type").ToString.Trim) Then
              strCrefBusinessType = Row.Item("cref_business_type").ToString.Trim
            End If
          End If

          findOwnerInfo(CLng(Row.Item("ac_id").ToString), strOwnershipType, strExclusiveFlag, strRegNbr, strCompCountry, strCrefBusinessType, _
                          Total_Domestic_Whole, Total_Domestic_Shared, Total_Domestic_Fractional, _
                          Total_International_Whole, Total_International_Shared, Total_International_Fractional, _
                          Total_Dealer_Whole, Total_Dealer_Shared, Total_Dealer_Fractional, _
                          Total_Other_Whole, Total_Other_Shared, Total_Other_Fractional, _
                          Total_EndUser_Whole, Total_EndUser_Shared, Total_EndUser_Fractional, _
                          Total_EndUserExclusive_Whole, Total_EndUserExclusive_Shared, Total_EndUserExclusive_Fractional)

        Next

      End If

      Total_Dealer_Owned = Total_Dealer_Whole + Total_Dealer_Shared + Total_Dealer_Fractional
      Total_User_Owned = Total_EndUser_Whole + Total_EndUser_Shared + Total_EndUser_Fractional
      Total_Exclusive = Total_EndUserExclusive_Whole + Total_EndUserExclusive_Shared + Total_EndUserExclusive_Fractional
      Total_Other = Total_Other_Whole + Total_Other_Shared + Total_Other_Fractional

      Total_Domestic = Total_Domestic_Whole + Total_Domestic_Shared + Total_Domestic_Fractional
      Total_International = Total_International_Whole + Total_International_Shared + Total_International_Fractional

      If AFTT_Count > 0 Then
        AFTT_Avg = AFTT_Total / AFTT_Count
      Else
        AFTT_Avg = 0
      End If

      If YearMFG_Count > 0 Then
        YearMFG_Avg = YearMFG_Total / YearMFG_Count
      Else
        YearMFG_Avg = 0
      End If

      If Asking_Count > 0 Then
        Asking_Avg = Asking_Total / Asking_Count
      Else
        Asking_Avg = 0
      End If

      If Landings_Count > 0 Then
        Landings_Avg = Landings_Total / Landings_Count
      Else
        Landings_Avg = 0
      End If

      If EngTT_Count > 0 Then
        EngTT_Avg = EngTT_Total / EngTT_Count
      Else
        EngTT_Avg = 0
      End If

      If DaysOnMarket_Count > 0 Then
        DaysOnMarket_Avg = DaysOnMarket_Total / DaysOnMarket_Count
      Else
        DaysOnMarket_Avg = 0
      End If

      htmlOut.Append("<table id=""outterExecSummaryTable"" width=""720"" cellspacing=""0"" cellpadding=""2"" border=""0"">")
      htmlOut.Append("<tr><td align=""center""><br />")

      htmlOut.Append("<table id=""innerExecSummaryTable1"" cellspacing=""0"" cellpadding=""2"" width=""100%"" class=""data_aircraft_grid"">")
      htmlOut.Append("<tr class=""header_row""><td colspan=""10"" valign=""middle"" align=""center""><strong>Ownership(Summary)</strong></td></tr>")

      htmlOut.Append("<tr class=""header_row""><td></td>")
            htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total All Airframes""><strong>Totals</strong></td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total 'For Sale' Airframes""><strong>For Sale</strong></td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total 'Leased' Airframes""><strong>Leased</strong></td>")
      htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total 'Dealer Broker/Manufacturer/Distributor' Owned Airframes""><strong>Dealer(Owned)</strong></td>")
            htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total 'End User' Airframes""><strong>End User Owned</strong></td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"" title=""Total 'End User w/Exclusive' Airframes""><strong>End User w/Exclusive</strong></td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"" title=""ONLY 'United States'""><strong>Owned(Domestic)</strong></td>")
      htmlOut.Append("<td valign=""middle"" align=""right"" title=""Other than 'United States'""><strong>Owned(International)</strong></td>")
      htmlOut.Append("<td valign=""middle"" align=""right"" title=""All Financial Institution/Fractional Leasing Company/Ferrying Company/Leasing Company/Program Holder/Reverse Exchange Company""><strong>Misc.Owned</strong></td>")

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr class=""alt_row"">")

      htmlOut.Append("<td valign=""middle"" align=""right"">Wholly&nbsp;Owned</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Whole, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_ForSale_Whole, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Leased_Whole, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Dealer_Whole, 0, True, False, True).ToString + "</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUser_Whole, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUserExclusive_Whole, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Domestic_Whole, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_International_Whole, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Other_Whole, 0, True, False, True).ToString + "</td>")

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr>")

      htmlOut.Append("<td valign=""middle"" align=""right"">Fractionally&nbsp;Owned</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Fractional, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">N/A</td>") ' + FormatNumber(Total_ForSale_Fractional, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Leased_Fractional, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Dealer_Fractional, 0, True, False, True).ToString + "</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUser_Fractional, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUserExclusive_Fractional, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Domestic_Fractional, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_International_Fractional, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Other_Fractional, 0, True, False, True).ToString + "</td>")

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr class=""alt_row"">")

      htmlOut.Append("<td valign=""middle"" align=""right"">Shared&nbsp;Ownership</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Shared, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_ForSale_Shared, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Leased_Shared, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Dealer_Shared, 0, True, False, True).ToString + "</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUser_Shared, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_EndUserExclusive_Shared, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Domestic_Shared, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_International_Shared, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Other_Shared, 0, True, False, True).ToString + "</td>")

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr>")

      htmlOut.Append("<td valign=""middle"" align=""right"">Totals</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(nCount, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Available, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Leased, 0, True, False, True).ToString + "</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Dealer_Owned, 0, True, False, True).ToString + "</td>")
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_User_Owned, 0, True, False, True).ToString + "</td>")
            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Exclusive, 0, True, False, True).ToString + "</td>")
            End If
            htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Domestic, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_International, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Total_Other, 0, True, False, True).ToString + "</td>")

      htmlOut.Append("</tr>")
      htmlOut.Append("</table><br />")

      htmlOut.Append("<table id=""innerExecSummaryTable2"" cellspacing=""0"" cellpadding=""2"" width=""80%"" class=""data_aircraft_grid"">")
      htmlOut.Append("<tr class=""header_row""><td colspan=""4"" valign=""middle"" align=""center""><strong>Aircraft(Summary)</strong></td></tr>")
      htmlOut.Append("<tr class=""header_row""><td valign=""middle"" align=""right""></td>")
      htmlOut.Append("<td valign=""middle"" align=""right""><strong>Low</strong></td>")
      htmlOut.Append("<td valign=""middle"" align=""right""><strong>High</strong></td>")
      htmlOut.Append("<td valign=""middle"" align=""right""><strong>Average</strong></td></tr>")

            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<tr class=""alt_row""><td valign=""middle"" align=""right"">Asking Price*</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">$" + FormatNumber(Asking_Low, 0, True, False, True).ToString + "</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">$" + FormatNumber(Asking_High, 0, True, False, True).ToString + "</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">$" + FormatNumber(Asking_Avg, 0, True, False, True).ToString + "</td></tr>")
            End If


            htmlOut.Append("<tr><td valign=""middle"" align=""right"">AFTT</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(AFTT_Low, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(AFTT_High, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(AFTT_Avg, 0, True, False, True).ToString + "</td></tr>")

      htmlOut.Append("<tr class=""alt_row""><td valign=""middle"" align=""right"">Year(Mfr)</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(YearMFG_Low, 0, True, False, False).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(YearMFG_High, 0, True, False, False).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(YearMFG_Avg, 0, True, False, False).ToString + "</td></tr>")

      htmlOut.Append("<tr><td valign=""middle"" align=""right"">Landings</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Landings_Low, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Landings_High, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(Landings_Avg, 0, True, False, True).ToString + "</td></tr>")

      htmlOut.Append("<tr class=""alt_row""><td valign=""middle"" align=""right"">Engine Total Time</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(EngTT_Low, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(EngTT_High, 0, True, False, True).ToString + "</td>")
      htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(EngTT_Avg, 0, True, False, True).ToString + "</td></tr>")


            If Session.Item("localPreferences").AerodexFlag = False Then
                htmlOut.Append("<tr><td valign=""middle"" align=""right"">Days On Market**</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(DaysOnMarket_Low, 0, True, False, True).ToString + "</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(DaysOnMarket_High, 0, True, False, True).ToString + "</td>")
                htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(DaysOnMarket_Avg, 0, True, False, True).ToString + "</td></tr>")

                htmlOut.Append("<tr class=""alt_row""><td valign=""middle"" align=""center"" colspan=""4""> * Based On: " + FormatNumber(Asking_Count, 0, True, False, True).ToString + " Aircraft Prices &nbsp;&nbsp;&nbsp;&nbsp; ** Based On: " + FormatNumber(Total_Available, 0, True, False, True).ToString + " For Sale Aircraft</td></tr>")
            End If

            htmlOut.Append("</table>")
            htmlOut.Append("<br /></td></tr></table>")

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in make_exec_summary()" + ex.Message
    Finally

      execSummary_datatable = Nothing

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return htmlOut.ToString

  End Function

  Public Sub findOwnerInfo(ByVal inAircraftID As Long, _
                            ByVal inOwnershipType As String, _
                            ByVal inExclusiveFlag As String, _
                            ByVal inRegNbr As String, _
                            ByVal inCompCountry As String, _
                            ByVal inCrefBusinessType As String, _
                            ByRef Total_Domestic_Whole As Integer, _
                            ByRef Total_Domestic_Shared As Integer, _
                            ByRef Total_Domestic_Fractional As Integer, _
                            ByRef Total_International_Whole As Integer, _
                            ByRef Total_International_Shared As Integer, _
                            ByRef Total_International_Fractional As Integer, _
                            ByRef Total_Dealer_Whole As Integer, _
                            ByRef Total_Dealer_Shared As Integer, _
                            ByRef Total_Dealer_Fractional As Integer, _
                            ByRef Total_Other_Whole As Integer, _
                            ByRef Total_Other_Shared As Integer, _
                            ByRef Total_Other_Fractional As Integer, _
                            ByRef Total_EndUser_Whole As Integer, _
                            ByRef Total_EndUser_Shared As Integer, _
                            ByRef Total_EndUser_Fractional As Integer, _
                            ByRef Total_EndUserExclusive_Whole As Integer, _
                            ByRef Total_EndUserExclusive_Shared As Integer, _
                            ByRef Total_EndUserExclusive_Fractional As Integer)

    Try

      '--------------------------------------------
      ' check domestic vs international owner
      ' if country is blank check regnbr for 'N'
      '--------------------------------------------
      If (inCompCountry.ToLower.Contains("united states")) Or (String.IsNullOrEmpty(inCompCountry.Trim) And Left(inRegNbr, 1).ToUpper.Contains("N")) Then

        Select Case inOwnershipType.ToUpper
          Case "W"
            Total_Domestic_Whole += 1
          Case "S"
            Total_Domestic_Shared += 1
          Case "F"
            Total_Domestic_Fractional += 1
        End Select

      Else

        Select Case inOwnershipType.ToUpper
          Case "W"
            Total_International_Whole += 1
          Case "S"
            Total_International_Shared += 1
          Case "F"
            Total_International_Fractional += 1
        End Select

      End If ' if (inCompCountry = "United States") Or (inCompCountry = "" AND Left(inRegNbr,1) = "N") Then

      ' check business type
      Select Case inCrefBusinessType.ToUpper

        ' Counts As Dealer
        ' Dealer, Manufacturer, Distributor 
        Case "DB", "MF", "DS"

          Select Case UCase(inOwnershipType)
            Case "W"
              Total_Dealer_Whole += 1
            Case "S"
              Total_Dealer_Shared += 1
            Case "F"
              Total_Dealer_Fractional += 1
          End Select

          ' Counts As Other
          ' Financial Institution, Leasing Company
          ' Fractional Leasing Company, Ferrying Company
          ' Program Holder, Reverse Exchange Company
        Case "FI", "LS", "FS", "FY", "PH", "RE"

          Select Case inOwnershipType.ToUpper
            Case "W"
              Total_Other_Whole += 1
            Case "S"
              Total_Other_Shared += 1
            Case "F"
              Total_Other_Fractional += 1
          End Select

          ' Everything Else Counts As End User
          ' Anything NOT DB, MF, DS, FI, LS, FS, FY, PH, RE
        Case Else

          Select Case inOwnershipType.ToUpper
            Case "W"
              Total_EndUser_Whole += 1
            Case "S"
              Total_EndUser_Shared += 1
            Case "F"
              Total_EndUser_Fractional += 1
          End Select

          If inExclusiveFlag.ToUpper.Contains("Y") Then

            Select Case inOwnershipType.ToUpper
              Case "W"
                Total_EndUserExclusive_Whole += 1
              Case "S"
                Total_EndUserExclusive_Shared += 1
              Case "F"
                Total_EndUserExclusive_Fractional += 1
            End Select

          End If ' if UCase(inExclusiveFlag) = "Y" then

      End Select ' Select case FACcref_business_type

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in findOwnerInfo(...)" + ex.Message
    Finally

    End Try

  End Sub ' FindOwnerInfo

End Class