' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/home_Model.aspx.vb $
'$$Author: Matt $
'$$Date: 4/27/20 2:40p $
'$$Modtime: 4/27/20 2:40p $
'$$Revision: 11 $
'$$Workfile: home_Model.aspx.vb $
'
' ********************************************************************************

Partial Public Class home_Model
    Inherits System.Web.UI.Page
    Dim ModelID As Long = 272
    Dim attIndex As Long = 0

    Public Shared masterPage As New Object


    Private Sub home_Model_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.Master"
            masterPage = DirectCast(Page.Master, EmptyEvoTheme)
        ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
            Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
            masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
        ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
            masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request("modelID")) Then
            If Not String.IsNullOrEmpty(Trim(Request("modelID"))) Then
                If IsNumeric(Trim(Request("modelID"))) Then
                    ModelID = Trim(Request("modelID"))
                End If
            End If
        End If

        If Not IsNothing(Request("attIndex")) Then
            If Not String.IsNullOrEmpty(Trim(Request("attIndex"))) Then
                If IsNumeric(Trim(Request("attIndex"))) Then
                    attIndex = Trim(Request("attIndex"))
                End If
            End If
        End If

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase - Assign Model Attributes")
            masterPage.SetPageTitle("Homebase - Assign Model Attributes")
        Else
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution - Assign Model Attributes")
            masterPage.SetPageTitle("Evolution - Assign Model Attributes")
        End If


        If attIndex = 0 Then
            Dim modelTable As New DataTable
            Dim modelTable_available As New DataTable
            Dim modelTable_assigned As New DataTable

            modelTable = getModelAttributes()
            modelTable_available = getAvailableModelAttributes()    'right 
            modelTable_assigned = getAssignedModelAttributes()      'left

            If Not IsNothing(modelTable) Then
                If modelTable.Rows.Count > 0 Then

                    BuildSortables(modelTable, modelTable_available, modelTable_assigned)
                End If
            End If



            'Dim allTable As New DataTable
            'allTable = getAllModelAttribute()

            ' Response.Write("model ID: " & ModelID.ToString)
            Dim InformationTable As New DataTable
            InformationTable = masterPage.aclsData_Temp.GetJetnetModelInfo(ModelID, True, "home_Model.aspx.vb")
            If Not IsNothing(InformationTable) Then
                If InformationTable.Rows.Count > 0 Then
                    If Not IsDBNull(InformationTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(InformationTable.Rows(0).Item("amod_model_name")) Then
                        informationText.InnerText = InformationTable.Rows(0).Item("amod_make_name").ToString + " " + InformationTable.Rows(0).Item("amod_model_name").ToString + " Model Attributes"
                        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(InformationTable.Rows(0).Item("amod_make_name").ToString + " " + InformationTable.Rows(0).Item("amod_model_name").ToString)
                        masterPage.SetPageTitle(InformationTable.Rows(0).Item("amod_make_name").ToString + " " + InformationTable.Rows(0).Item("amod_model_name").ToString + " Model Attributes")
                    End If
                End If
            End If
        Else
            'Look up the Model:
            Dim ModelName As String = ""
            Dim MakeName As String = ""
            Dim InformationTable As New DataTable
            InformationTable = masterPage.aclsData_Temp.GetJetnetModelInfo(ModelID, True, "home_Model.aspx.vb")
            If Not IsNothing(InformationTable) Then
                If InformationTable.Rows.Count > 0 Then
                    If Not IsDBNull(InformationTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(InformationTable.Rows(0).Item("amod_model_name")) Then
                        MakeName = InformationTable.Rows(0).Item("amod_make_name").ToString
                        ModelName = InformationTable.Rows(0).Item("amod_model_name").ToString
                    End If
                End If
            End If

            informationText.InnerText = "Edit Model Attribute"
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Edit Model Attribute")
            masterPage.SetPageTitle("Edit Model Attribute")
            defaultText.Visible = False
            Dim attInfoTable As New DataTable
            attInfoTable = clsGeneral.clsGeneral.SelectModelAttributeByID(attIndex)

            If Not IsNothing(attInfoTable) Then
                If attInfoTable.Rows.Count > 0 Then
                    panelUpdateAtt.Visible = True
                    ReturnToList.Text = "<a href=""/home_Model.aspx?modelID=" & ModelID.ToString & """>Return to List</a>"
                    informationText.InnerText = MakeName & " " & ModelName & " " & attInfoTable.Rows(0).Item("acatt_name").ToString
                    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(MakeName & " " & ModelName & " " & attInfoTable.Rows(0).Item("acatt_name").ToString)
                    masterPage.SetPageTitle(MakeName & " " & ModelName & " " & attInfoTable.Rows(0).Item("acatt_name").ToString)

                    If Not Page.IsPostBack Then
                        'attmod_value
                        If Not IsDBNull(attInfoTable.Rows(0).Item("attmod_standard_equip")) Then
                            standardEquipUpdate.Checked = IIf(attInfoTable.Rows(0).Item("attmod_standard_equip") = "Y", True, False)
                        End If
                        'attmod_standard_equip()
                        If Not IsDBNull(attInfoTable.Rows(0).Item("attmod_value")) Then
                            valueAttUpdate.Text = attInfoTable.Rows(0).Item("attmod_value")
                        End If
                        'attmod_notes
                        If Not IsDBNull(attInfoTable.Rows(0).Item("attmod_notes")) Then
                            valueAttmod_notes.Text = attInfoTable.Rows(0).Item("attmod_notes")
                        End If

                        'attmod_stdeq_start_ser_no_value
                        If Not IsDBNull(attInfoTable.Rows(0).Item("attmod_stdeq_start_ser_no_value")) Then
                            valueAttmod_stdeq_start_ser_no.Text = attInfoTable.Rows(0).Item("attmod_stdeq_start_ser_no_value")
                        End If
                        'attmod_stdeq_end_ser_no_value
                        If Not IsDBNull(attInfoTable.Rows(0).Item("attmod_stdeq_end_ser_no_value")) Then
                            valueAttmod_stdeq_end_ser_no.Text = attInfoTable.Rows(0).Item("attmod_stdeq_end_ser_no_value")
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Public Sub BuildSortables(ByVal modelTable As DataTable, ByVal Available_modelTable As DataTable, ByVal assigned_modelTable As DataTable)

        Dim DistinctTableUnassignedView As New DataView
        Dim DistinctTableUnassigned As New DataTable

        'DistinctTableUnassignedView = Available_modelTable.DefaultView

        'DistinctTableUnassignedView.Sort = " ATTRIBUTE, attmod_seq_no"
        ' DistinctTableUnassignedView.Sort = "AREA, BLOCK, ATTRIBUTE, attmod_seq_no"

        ' DistinctTableUnassignedView.RowFilter = "ASSIGNED = ''"
        DistinctTableUnassigned = Available_modelTable

        sort1.Text = "<div class=""six columns""><h3>Available Attributes</h3>"
        sort1.Text += "<input type = ""text"" id=""searchValueCheck"" onkeyup=""searchTheSortable()"" placeholder=""Type to search for attributes.."" title=""Type in an Attribute"" />"
        sort1.Text += "<ul id=""sortable1"" class=""connectedSortable"">"
        Dim oldArea As String = ""
        Dim oldBlock As String = ""
        Dim cssClass As String = ""
        For Each r As DataRow In DistinctTableUnassigned.Rows
            Dim value As Long = 0
            'If Not IsDBNull(r("attmod_standard_equip")) Then
            '    If r("attmod_standard_equip") = "Y" Then
            '        cssClass = "defaultModel"
            '    End If
            'End If
            'If oldArea <> r("AREA") Then
            '  sort1.Text += "<li class=""ui-state-default ui-state-disabled area"">" & r("AREA") & "</li>"
            'End If
            'If oldBlock <> r("BLOCK") Then
            '  sort1.Text += "<li class=""ui-state-default ui-state-disabled"">" & r("BLOCK") & "</li>"
            'End If
            If Not IsDBNull(r("VALUE")) Then
                If r("VALUE") > 0 Then
                    value = r("VALUE")
                End If
            End If

            sort1.Text += "<li class=""ui-state-default indent " & cssClass & """ id=""catRow_" & r("acatt_id").ToString & """ model=""" & ModelID.ToString & """>"
            sort1.Text += r("ATTRIBUTE")

            If value > 0 Then
                sort1.Text += " (" & clsGeneral.clsGeneral.ConvertIntoThousands(value).ToString & ")"
            End If


            sort1.Text += "</li>"
            'oldArea = r("AREA")
            '  oldBlock = r("BLOCK")
            cssClass = ""
        Next
        sort1.Text += "</ul>"
        sort1.Text += "</div></div>"

        Dim DistinctTableAssignedView As New DataView
        Dim DistinctTableAssigned As New DataTable

        'DistinctTableAssignedView = assigned_modelTable.DefaultView
        '   DistinctTableAssignedView.Sort = "attmod_seq_no"
        '  DistinctTableAssignedView.RowFilter = "ASSIGNED <> ''"
        DistinctTableAssigned = assigned_modelTable

        oldArea = ""
        oldBlock = ""
        Dim Count As Integer = 1
        sort2.Text = "<div class=""row""><div class=""six columns""><h3>Attributes Assigned</h3><ul id=""sortable2"" class=""connectedSortable"">"
        For Each r As DataRow In DistinctTableAssigned.Rows
            Dim value As Long = 0
            cssClass = ""
            If Not IsDBNull(r("STANDARD")) Then
                If r("STANDARD") = "Y" Then
                    cssClass = "defaultModel"
                End If
            End If

            'If oldArea <> r("AREA") Then
            '  sort2.Text += "<li class=""ui-state-default ui-state-disabled area"">" & r("AREA") & "</li>"
            'End If
            'If oldBlock <> r("BLOCK") Then
            '  sort2.Text += "<li class=""ui-state-default ui-state-disabled"">" & r("BLOCK") & "</li>"
            'End If
            If Not IsDBNull(r("VALUE")) Then
                If r("VALUE") > 0 Then
                    value = r("VALUE")
                End If
            End If

            sort2.Text += "<li class=""ui-state-default " & cssClass & """ id=""catRow_" & r("acatt_id").ToString & """ model=""" & ModelID.ToString & """>" & Count.ToString & ".) <a href=""/home_Model.aspx?modelID=" & ModelID.ToString & "&attIndex=" & r("attmod_id") & """>" & r("ATTRIBUTE") & "</a>"

            If value > 0 Then
                sort2.Text += " (" & clsGeneral.clsGeneral.ConvertIntoThousands(value).ToString & ")"
            End If

            sort2.Text += "</li>"
            'oldArea = r("AREA")
            'oldBlock = r("BLOCK")
            Count += 1
        Next
        sort2.Text += "</ul>"

        sort2.Text += "<h3>Asset Insight Attributes (Not Mapped)</h3>"
        sort2.Text += "<p Class='large' id='defaultText'>"
        sort2.Text += "The list below shows assets not mapped to this specific model.  The <span class='defaultModel'>blue</span> assets have at least been mapped to a JETNET attribute."
        sort2.Text += "</p>"

        sort2.Text += "<ul id = ""sortable3"" Class=""connectedSortable"">"

        Dim unassignedTable As New DataTable
        unassignedTable = getUnassignedModelAttributes()
        If Not IsNothing(unassignedTable) Then
            If unassignedTable.Rows.Count > 0 Then
                For Each q As DataRow In unassignedTable.Rows

                    If IsDBNull(q("acatt_id")) Then
                        sort2.Text += "<li>" & q("aimodif_description") & "</li>"
                    ElseIf Not IsDBNull(q("acatt_id")) Then
                        If q("acatt_id") > 0 Then
                            sort2.Text += "<li><span class='defaultModel'>" & q("aimodif_description") & "</span></li>"
                        Else
                            sort2.Text += "<li>" & q("aimodif_description") & "</li>"
                        End If
                    End If
                Next
            End If
        End If
        sort2.Text += "</ul>"


        sort2.Text += "<h3>Model Features (Not Mapped)</h3><ul id=""sortable3"" Class=""connectedSortable"">"

        Dim unassignedTable2 As New DataTable
        unassignedTable2 = getUnassignedAttributes()
        If Not IsNothing(unassignedTable2) Then
            If unassignedTable2.Rows.Count > 0 Then
                For Each q As DataRow In unassignedTable2.Rows
                    sort2.Text += "<li>" & q("kfeat_name") & "</li>"
                Next
            End If
        End If
        sort2.Text += "</ul>"


        sort2.Text += "</div>"

    End Sub

    Public Function getAllModelAttribute() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append("Select * from [Homebase].jetnet_ra.dbo.aircraft_attribute_model order by attmod_amod_id ")
            Else
                sQuery.Append("Select * from aircraft_attribute_model order by attmod_amod_id ")
            End If



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function

    Public Function getModelAttributes() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append("Select acatt_area As AREA,attmod_standard_equip, attmod_id, acatt_block As BLOCK, acatt_name As ATTRIBUTE, acatt_abbrev As ABBREV, ")
            sQuery.Append(" Case When attmod_value > 0 Then attmod_value Else acatt_average_value End As VALUE, ")
            sQuery.Append(" Case When Not attmod_amod_id Is NULL Then 'YES' else ' ' end as ASSIGNED,attmod_seq_no, acatt_id, COUNT(distinct ac_id) as AIRCRAFT ")


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Index with (NOLOCK)  ")
                sQuery.Append(" INNER JOIN [Homebase].jetnet_ra.dbo.Aircraft_Attribute with (NOLOCK) on acatt_id = acattind_acatt_id ")
                sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft with (NOLOCK) on ac_id = acattind_ac_id and ac_journ_id = 0 ")
                sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model with (NOLOCK) on ac_amod_id = attmod_amod_id and acatt_id = attmod_att_id ")
            Else
                sQuery.Append(" from Aircraft_Attribute_Index with (NOLOCK)  ")
                sQuery.Append(" INNER JOIN Aircraft_Attribute with (NOLOCK) on acatt_id = acattind_acatt_id ")
                sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = acattind_ac_id and ac_journ_id = 0 ")
                sQuery.Append(" left outer join Aircraft_Attribute_Model with (NOLOCK) on ac_amod_id = attmod_amod_id and acatt_id = attmod_att_id ")
            End If

            sQuery.Append(" where ac_amod_id = @acAmodID ")
            sQuery.Append(" and acattind_status_flag in ('Y','N') ")
            sQuery.Append(" group by acatt_area,acatt_block, acatt_name, attmod_id,  acatt_abbrev, case when attmod_value > 0 then attmod_value else acatt_average_value end,attmod_amod_id, attmod_standard_equip, attmod_seq_no, acatt_id ")
            sQuery.Append(" order by case when not attmod_amod_id is NULL then 'YES' else ' ' end desc, acatt_area, acatt_block,acatt_name, attmod_seq_no ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("acAmodID", ModelID)




            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function

    Public Function getAvailableModelAttributes() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append("  Select distinct acatt_name As ATTRIBUTE,  acatt_id,  ")
            sQuery.Append("  acatt_average_value as VALUE, count(distinct ac_id) as TCOUNT ")

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append("  From [Homebase].jetnet_ra.dbo.Aircraft_Attribute WITH(NOLOCK)  ")
                sQuery.Append("  iNNER Join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Index with (NOLOCK) on acatt_id=acattind_acatt_id ")
                sQuery.Append("  inner Join [Homebase].jetnet_ra.dbo.aircraft with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id =0 ")
                sQuery.Append("  Left outer join [Homebase].jetnet_ra.dbo.aircraft_attribute_model with (NOLOCK) on ac_amod_id = attmod_amod_id And acatt_id = attmod_att_id ")
            Else
                sQuery.Append("  From Aircraft_Attribute WITH(NOLOCK)  ")
                sQuery.Append("  iNNER Join Aircraft_Attribute_Index with (NOLOCK) on acatt_id=acattind_acatt_id ")
                sQuery.Append("  inner Join aircraft with (NOLOCK) on acattind_ac_id = ac_id And ac_journ_id =0 ")
                sQuery.Append("  Left outer join aircraft_attribute_model with (NOLOCK) on ac_amod_id = attmod_amod_id And acatt_id = attmod_att_id ")
            End If

            sQuery.Append("  WHERE ac_amod_id = @acAmodID  ")
            sQuery.Append("  And attmod_amod_id Is NULL ")
            sQuery.Append("  Group by acatt_name, acatt_id, acatt_average_value ")
            sQuery.Append("  ORDER BY acatt_name  ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("acAmodID", ModelID)


            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function

    Public Function getAssignedModelAttributes() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append("  Select acatt_name As ATTRIBUTE, acatt_abbrev As ABBREV, acatt_id, attmod_id, ")
            sQuery.Append(" attmod_standard_equip as STANDARD, ")
            sQuery.Append(" CASE WHEN attmod_value > 0 THEN attmod_value ELSE acatt_average_value END AS VALUE  ")

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" From [Homebase].jetnet_ra.dbo.Aircraft_Attribute WITH(NOLOCK)  ")
                sQuery.Append(" INNER Join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model WITH(NOLOCK) ON acatt_id = attmod_att_id ")
            Else
                sQuery.Append(" From Aircraft_Attribute WITH(NOLOCK)  ")
                sQuery.Append(" INNER Join Aircraft_Attribute_Model WITH(NOLOCK) ON acatt_id = attmod_att_id ")
            End If

            sQuery.Append(" WHERE attmod_amod_id = @acAmodID  ")
            sQuery.Append(" ORDER BY attmod_standard_equip, attmod_seq_no ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("acAmodID", ModelID)




            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function

    Public Function getUnassignedModelAttributes() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            '' IF WE ARE ON HOMEBASE - WE NEED TO GO TO SQL1 , IF WE ARE ON EVOADMIN, USE NORMAL CONNECTION 
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                SqlConn.ConnectionString = "server=www.jetnetsql1.com;initial catalog=jetnet_ra;Persist Security Info=False;User Id=evolution;Password=vbs73az8;"
            Else
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            End If

            SqlConn.Open()


            sQuery.Append("select aimodif_description, aimodif_item_id, acatt_name, acatt_id")

            sQuery.Append(" from Asset_Insight_Model_Modifications with (NOLOCK)")
            sQuery.Append(" inner join Asset_Insight_Model wtih (NOLOCK) on aimodmod_asset_id = aimodel_asset_id and aimodel_jetnet_amod_id > 0")
            sQuery.Append(" inner join Asset_Insight_Modifications with (NOLOCK) on aimodif_item_id = aimodmod_item_id")


            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                sQuery.Append(" left outer join aircraft_attribute with (NOLOCK) on acatt_id = aimodif_acatt_id ")
            Else
                sQuery.Append(" left outer join  [Homebase].jetnet_ra.dbo.aircraft_attribute with (NOLOCK) on acatt_id = aimodif_acatt_id ")
            End If


            sQuery.Append(" where aimodel_jetnet_amod_id = @acAmodID ")

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                sQuery.Append(" and aimodif_acatt_id not in (select distinct attmod_att_id from Aircraft_Attribute_Model with (NOLOCK)")
            Else
                sQuery.Append(" and aimodif_acatt_id not in (select distinct attmod_att_id from [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model with (NOLOCK)")
            End If



            sQuery.Append(" where attmod_amod_id = aimodel_jetnet_amod_id)")
            sQuery.Append(" order by aimodif_description")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("acAmodID", ModelID)


            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function


    Public Function getUnassignedAttributes() As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection

        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try

            '' goes external msw - 6/15/19 --
            ' If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
            '  SqlConn.ConnectionString = "server=www.jetnetsql1.com;initial catalog=jetnet_ra;Persist Security Info=False;User Id=evolution;Password=vbs73az8;"
            '  Else
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            '   End If



            SqlConn.Open()


            sQuery.Append("select kfeat_code, kfeat_name,  ")
            sQuery.Append(" case when acatt_name is NULL then 'NOT MAPPED TO ATTRIBUTE'  ")
            sQuery.Append(" when attmod_att_id is NULL then 'NOT MAPPED TO MODEL' else 'MAPPED' end AS MODELSTATUS ")
            '--acatt_name, attmod_att_id ")


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" from [Homebase].jetnet_ra.dbo.Key_Feature with (NOLOCK) ")
                sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.Aircraft_Model_Key_Feature with (NOLOCK) on kfeat_code = amfeat_feature_code ")
                sQuery.Append(" inner join [Homebase].jetnet_ra.dbo.aircraft_model wtih (NOLOCK) On amfeat_amod_id = amod_id ")
                sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute With (NOLOCK) On kfeat_code=acatt_abbrev ")
                sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.Aircraft_Attribute_Model With (NOLOCK) On amod_id = attmod_amod_id And acatt_id = attmod_att_id ")
            Else
                sQuery.Append(" from Key_Feature with (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Model_Key_Feature with (NOLOCK) on kfeat_code = amfeat_feature_code ")
                sQuery.Append(" inner join aircraft_model wtih (NOLOCK) On amfeat_amod_id = amod_id ")
                sQuery.Append(" left outer join Aircraft_Attribute With (NOLOCK) On kfeat_code=acatt_abbrev ")
                sQuery.Append(" left outer join Aircraft_Attribute_Model With (NOLOCK) On amod_id = attmod_amod_id And acatt_id = attmod_att_id ")
            End If



            sQuery.Append(" where amod_id = " & ModelID & " ")
            sQuery.Append(" And kfeat_inactive_date Is NULL ")
            sQuery.Append(" And attmod_att_id Is NULL ")
            sQuery.Append(" order by kfeat_name ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return temptable

    End Function

    Private Sub submitAttribute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitAttribute.Click
        If Page.IsValid Then
            'amod_id = attIndex 
            'amod_value valueAttUpdate.Text 
            'amod_standard_default_equip iif(standardEquipUpdate.Checked, "Y","N")
            'amod_amod_id = ModelID


            ''attmod_notes = valueAttmod_notes.Text 
            ''attmod_stdeq_start_ser_no_value = valueAttmod_stdeq_start_ser_no.Text 
            ''attmod_stdeq_end_ser_no_value = valueAttmod_stdeq_end_ser_no.Text 

            clsGeneral.clsGeneral.UpdateModelAttribute(IIf(standardEquipUpdate.Checked, "Y", "N"), valueAttUpdate.Text, attIndex, ModelID, valueAttmod_notes.Text, valueAttmod_stdeq_start_ser_no.Text, valueAttmod_stdeq_end_ser_no.Text)
            attention.Text = "<p align=""center"">Your attribute has been updated.</p>"
        End If
    End Sub


End Class