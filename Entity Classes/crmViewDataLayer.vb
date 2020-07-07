Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/crmViewDataLayer.vb $
'$$Author: Amanda $
'$$Date: 6/26/20 4:19p $
'$$Modtime: 6/26/20 3:28p $
'$$Revision: 25 $  


'$$Workfile: crmViewDataLayer.vb $
' 
' ****************************************************************** **************  

<System.Serializable()> Public Class crmViewDataLayer

    Private Shared aError As String

    Private jetClientConnectString As String
    Private crmClientConnectString As String



    Private jetAdminConnectString As String

    Private jetStarConnectString As String
    Private jetCloudConnectString As String

    Private jetServerConnectString As String

    Private crmActiveConnectString As String
    Private crmHistoryConnectString As String

#Region "Constructor"
    Sub New()
        aError = ""

        jetClientConnectString = ""
        crmClientConnectString = ""

        jetAdminConnectString = ""
        jetStarConnectString = ""
        jetCloudConnectString = ""
        jetServerConnectString = ""

        crmActiveConnectString = ""
        crmHistoryConnectString = ""

    End Sub
#End Region
#Region "Properties"
    Public Shared Property class_error() As String
        Get
            class_error = aError
        End Get
        Set(ByVal value As String)
            aError = value
        End Set
    End Property

    Public Property adminConnectStr() As String
        Get
            adminConnectStr = jetAdminConnectString
        End Get
        Set(ByVal value As String)
            jetAdminConnectString = value
        End Set
    End Property

    Public Property jetClientConnectStr() As String
        Get
            jetClientConnectStr = jetClientConnectString
        End Get
        Set(ByVal value As String)
            jetClientConnectString = value
        End Set
    End Property

    Public Property crmClientConnectStr() As String
        Get
            crmClientConnectStr = crmClientConnectString
        End Get
        Set(ByVal value As String)
            crmClientConnectString = value
        End Set
    End Property

    Public Property jetStarConnectStr() As String
        Get
            jetStarConnectStr = jetStarConnectString
        End Get
        Set(ByVal value As String)
            jetStarConnectString = value
        End Set
    End Property

    Public Property jetCloudConnectStr() As String
        Get
            jetCloudConnectStr = jetCloudConnectString
        End Get
        Set(ByVal value As String)
            jetCloudConnectString = value
        End Set
    End Property

    Public Property jetServerConnectStr() As String
        Get
            jetServerConnectStr = jetServerConnectString
        End Get
        Set(ByVal value As String)
            jetServerConnectString = value
        End Set
    End Property

    Public Property crmActiveConnectStr() As String
        Get
            crmActiveConnectStr = crmActiveConnectString
        End Get
        Set(ByVal value As String)
            crmActiveConnectString = value
        End Set
    End Property

    Public Property crmHistoryConnectStr() As String
        Get
            crmHistoryConnectStr = crmHistoryConnectString
        End Get
        Set(ByVal value As String)
            crmHistoryConnectString = value
        End Set
    End Property
#End Region

#Region "PROSPECTOR VIEW - View #18"
#Region "Filling Controls"
    ''' <summary>
    ''' Fills up the common upgrade dropdown in the CRM View
    ''' </summary>
    ''' <param name="UpgradeModels">Datatable of upgrade models</param>
    ''' <param name="selectedModelID">Model ID of the model we're on</param>
    ''' <param name="Applicable_Dropdown">Dropdown we're filling</param>
    ''' <param name="applicableReportText">The report tab we're on</param>
    ''' <param name="model_reports_tab">Tab panel on view page</param>
    ''' <remarks></remarks>
    Public Shared Sub CRM_VIEW_Fill_Common_Upgrade_Dropdown(ByRef UpgradeModels As DataTable, ByRef selectedModelID As Long, ByRef Applicable_Dropdown As DropDownList, ByRef applicableReportText As String, ByRef model_reports_tab As AjaxControlToolkit.TabPanel, ByVal applicableControlToSaveSession As Control, ByVal PageIsPostBack As Boolean, Optional ByVal make_model_name As String = "")



        Select Case UCase(applicableReportText)
            Case "PROSPECTS"
                Applicable_Dropdown.Items.Clear()
                Applicable_Dropdown.Items.Add(New ListItem("My Aircraft", "AC"))
                Applicable_Dropdown.Items.Add(New ListItem("My Aircraft or " & make_model_name & " (but not other Aircraft)", "ACMODEL"))
                Applicable_Dropdown.Items.Add(New ListItem("All " & make_model_name & " Prospects", "MODEL"))

                ' Applicable_Dropdown.SelectedValue = "AC"
                Applicable_Dropdown.Enabled = True
                '  Applicable_Dropdown.CssClass = "display_disable"
            Case Else
                If UCase(model_reports_tab.HeaderText) <> "REPORT SELECTION: " & UCase(applicableReportText) Then
                    Applicable_Dropdown.Items.Clear()
                    If UpgradeModels.Rows.Count > 0 Then
                        Applicable_Dropdown.Items.Add(New ListItem("My Model", selectedModelID))
                        Applicable_Dropdown.Items.Add(New ListItem("Common Upgrade Models", 0))
                        'Filling up the dropdown.
                        For Each r As DataRow In UpgradeModels.Rows
                            If selectedModelID <> r("AModId") Then
                                Applicable_Dropdown.Items.Add(New ListItem(r("Make") & " " & r("Model"), r("AModId")))
                            End If
                        Next

                        'if we re-loaded the dropdown, and we have a session that we had changed, we need to change it to that
                        If IsNothing(HttpContext.Current.Session.Item("Prospector_Model")) Then
                            HttpContext.Current.Session.Item("Prospector_Model") = 0
                        End If

                        If HttpContext.Current.Session.Item("Prospector_Model") > 0 Then
                            Applicable_Dropdown.SelectedValue = HttpContext.Current.Session.Item("Prospector_Model")
                        End If
                    End If
                Else    ' if we have not changed tabs, then we need to not re-load the dropdown, but we need to set the session
                    If IsNothing(HttpContext.Current.Session.Item("Prospector_Model")) Then
                        HttpContext.Current.Session.Item("Prospector_Model") = 0
                    End If

                    HttpContext.Current.Session.Item("Prospector_Model") = Applicable_Dropdown.SelectedValue
                End If

        End Select

        'Go ahead and Fill Session, but only if not postback
        If Not PageIsPostBack Then
            crmViewDataLayer.FillSessionSearchOnProspector(applicableControlToSaveSession)
        End If

    End Sub

    ''' <summary>
    ''' Grabs common upgrade models datatable
    ''' </summary>
    ''' <param name="ModelID"></param>
    ''' <param name="UpgradeModelDataTable"></param>
    ''' <param name="localDatalayer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Find_Upgrade_Models_From_Records(ByRef ModelID As Long, ByRef UpgradeModelDataTable As DataTable, ByRef localDatalayer As viewsDataLayer) As String()
        Dim ReturnArray(1) As String 'ReturnArray(0) stores list of model IDs, ReturnArray(1) returns a list of model names. 
        Dim sqlQuery As String = ""
        Try
            sqlQuery = "SELECT TOP 5 "
            sqlQuery += "upg_modelid As AModId, "
            sqlQuery += "upg_make As Make, "
            sqlQuery += "upg_model As Model, "
            sqlQuery += "upg_totalsoldfrommodel As TotSold, "
            sqlQuery += "CAST(upg_totalupgradestomodel AS INT) As TotUpgrades, "
            sqlQuery += "upg_percentupgradestomodel As PercentWUpgrade "
            sqlQuery += "FROM star_reports.dbo.upgrade_data WITH (NOLOCK) "
            sqlQuery += " inner Join aircraft_model on amod_id = upg_modelid "
            sqlQuery += "WHERE (upg_upgradedtomodelid = " & ModelID & ") "
            sqlQuery += "AND (upg_database = (SELECT TOP 1 MAX(upg_database) "
            sqlQuery += "FROM star_reports.dbo.upgrade_data WITH (NOLOCK) "
            sqlQuery += ") "
            sqlQuery += "AND (upg_database <> 'jetnet_ra_') "
            sqlQuery += ") "
            sqlQuery += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, False)
            sqlQuery += "ORDER BY CAST(upg_totalupgradestomodel AS INT) DESC, upg_make, upg_model "

            UpgradeModelDataTable = localDatalayer.Get_CRM_VIEW_Function(sqlQuery, "Time to Buy Tab")

            If Not IsNothing(UpgradeModelDataTable) Then
                If UpgradeModelDataTable.Rows.Count > 0 Then
                    For Each r As DataRow In UpgradeModelDataTable.Rows
                        If ReturnArray(0) <> "" Then
                            ReturnArray(0) += ","
                            ReturnArray(1) += ", "
                        End If

                        ReturnArray(0) += r("AModId").ToString
                        ReturnArray(1) += r("Make").ToString & " " & r("Model").ToString
                    Next
                End If
            End If
        Catch ex As Exception
            Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(HttpContext.Current.Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex.Message, "'", "''"), Nothing, 0, 0, 0, 0, 0)
        End Try

        Return ReturnArray
    End Function

    Public Shared Sub FillSessionSearchOnProspector(ByRef parent As Control)
        Try
            For Each c As Control In parent.Controls
                If TypeOf c Is TextBox Then
                    Dim temporaryTextBox As TextBox = c
                    'Fill it back up if it exists
                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                            temporaryTextBox.Text = HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)
                        End If
                    End If
                ElseIf TypeOf c Is CheckBox Then
                    Dim temporaryCheckbox As CheckBox = c
                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID)) Then
                            temporaryCheckbox.Checked = HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID)
                        End If
                    End If
                ElseIf TypeOf c Is DropDownList Then
                    Dim temporaryDropdownList As DropDownList = c
                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                            temporaryDropdownList.SelectedValue = HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            ' Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "):  " & ex.Message)
        End Try
    End Sub

    Public Shared Sub ParseSaveSessionSearchOnProspector(ByRef parent As Control)
        Try
            For Each c As Control In parent.Controls
                If TypeOf c Is TextBox Then
                    Dim temporaryTextBox As TextBox = c

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID)) Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID) = temporaryTextBox.Text
                        End If
                    Else
                        If temporaryTextBox.Text <> "" Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryTextBox.ID) = temporaryTextBox.Text
                        End If
                    End If

                    temporaryTextBox.Dispose()
                ElseIf TypeOf c Is CheckBox Then
                    Dim temporaryCheckbox As CheckBox = c
                    HttpContext.Current.Session.Item("Advanced-" & temporaryCheckbox.ID) = temporaryCheckbox.Checked.ToString

                ElseIf TypeOf c Is DropDownList Then
                    Dim temporaryDropdownList As DropDownList = c

                    If Not IsNothing(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID)) Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID) = temporaryDropdownList.SelectedValue
                        End If
                    Else
                        If temporaryDropdownList.SelectedValue <> "" Then
                            HttpContext.Current.Session.Item("Advanced-" & temporaryDropdownList.ID) = temporaryDropdownList.SelectedValue
                        End If
                    End If
                    temporaryDropdownList.Dispose()
                End If

            Next
        Catch ex As Exception
            ' Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "):  " & ex.Message)
        End Try

    End Sub
#End Region
#Region "Dealing with Display"
    ''' <summary>
    ''' Builds the Company/Title Display
    ''' </summary>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="r"></param>
    ''' <remarks></remarks>
    Public Shared Sub CRM_VIEW_Build_Company_Title_Location(ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByVal r As DataRow)
        CompanyLocation = "" 'Display company Location for row. Clears initially
        CompanyTitle = "" 'Display company mouseover.  Clears initially

        CompanyTitle = IIf(Not IsDBNull(r("comp_name")), r("comp_name") & vbNewLine, vbNewLine)
        CompanyTitle += IIf(Not IsDBNull(r("comp_address1")), r("comp_address1") & " ", "")
        CompanyTitle += IIf(Not IsDBNull(r("comp_address2")), r("comp_address2") & vbNewLine, vbNewLine)
        CompanyLocation += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
        CompanyLocation += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")
        CompanyLocation += IIf(Not IsDBNull(r("comp_country")), r("comp_country") & " ", " ")

        CompanyLocation = Replace(CompanyLocation, "United States", "U.S.")
        CompanyTitle += IIf(Not IsDBNull(r("comp_city")), r("comp_city") & ", ", "")
        CompanyTitle += IIf(Not IsDBNull(r("comp_state")), r("comp_state") & " ", " ")

        CompanyTitle += IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")

        If CompanyTitle <> "" Then
            CompanyTitle += vbNewLine
        End If

        CompanyTitle += IIf(Not IsDBNull(r("comp_phone_office")), vbNewLine & "Office: " & r("comp_phone_office"), "")
        CompanyTitle += IIf(Not IsDBNull(r("comp_phone_fax")), vbNewLine & "Fax: " & r("comp_phone_fax"), "")

        If CompanyTitle <> "" Then
            CompanyTitle += vbNewLine
        End If

        CompanyTitle += IIf(Not IsDBNull(r("comp_email_address")), vbNewLine & "Email: " & r("comp_email_address"), "")
        CompanyTitle += IIf(Not IsDBNull(r("comp_web_address")), vbNewLine & "Website: " & r("comp_web_address"), "")

        CompanyTitle = Trim(CompanyTitle)
    End Sub
    ''' <summary>
    ''' Just a test function I've been using to export any datatable to excel
    ''' </summary>
    ''' <param name="dtdata"></param>
    ''' <remarks></remarks>
    Public Shared Sub ExportTableData(ByVal dtdata As DataTable)
        If dtdata IsNot Nothing Then
            Dim attach As String = "attachment;filename=Export.xls"
            HttpContext.Current.Response.Buffer = True
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.AddHeader("content-disposition", attach)
            HttpContext.Current.Response.ContentType = "application/ms-excel"


            For Each dc As DataColumn In dtdata.Columns
                'sep = ";";
                HttpContext.Current.Response.Write(dc.ColumnName + vbTab)
            Next
            HttpContext.Current.Response.Write(System.Environment.NewLine)
            For Each dr As DataRow In dtdata.Rows
                For i As Integer = 0 To dtdata.Columns.Count - 1
                    HttpContext.Current.Response.Write(dr(i).ToString() & vbTab)
                Next
                HttpContext.Current.Response.Write(vbLf)
            Next
            HttpContext.Current.Response.End()
        End If
    End Sub
#End Region
#Region "Previous Owner Functions - CRM VIEW"
    ''' <summary>
    ''' Builds the Previous Owner Display Table
    ''' </summary>
    ''' <param name="Temporary_Table"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="TabContainer1"></param>
    ''' <param name="count_label"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Previous_Owner_Table(ByRef Temporary_Table As DataTable, ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByRef count_label As Label, ByRef crm_view_view_all As LinkButton, ByVal activeTabID As String) As String
        Dim HtmlOut As New StringBuilder
        Dim CheckDataTable As New DataTable
        Dim ToggleRowColor As Boolean = False

        Dim CheckNote As Boolean = False
        Dim CheckAction As Boolean = False
        Dim CheckProspect As Boolean = False

        HtmlOut.Append("<div class='prospectDataTableContainer'><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")

        If Not IsNothing(Temporary_Table) Then
            count_label.Text = Temporary_Table.Rows.Count & " Previous Owners Found."

            If Temporary_Table.Rows.Count = 25 Then
                crm_view_view_all.Visible = True
            End If

            If Temporary_Table.Rows.Count > 0 Then

                HtmlOut.Append("<table width=""100%""  id=""" & activeTabID & "Copy"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")
                HtmlOut.Append("<thead><tr class='header'><!--<th valign='top' align='left'>SEL</th>--><th valign='top' align='left'>Previous Owner</th>")


                HtmlOut.Append("<th valign='top' align='left'>Make/Model</th>")
                HtmlOut.Append("<th valign='top' align='left'>Date Sold</th>")
                HtmlOut.Append("<th valign='top' align='left'>Serial No</th>")
                HtmlOut.Append("<th valign='top' align='left'>Reg No</th>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Notes
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Actions
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Dates
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                End If
                '----------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(True, Temporary_Table.Rows(0)))
                HtmlOut.Append("</tr></thead><tbody>")

                For Each r As DataRow In Temporary_Table.Rows
                    Dim NoteDisplay As String = "" 'Deals with Note Display
                    Dim ActionDisplay As String = "" 'Action Display Text.
                    Dim ProspectDisplay As String = "" 'Deals with Prospect Display


                    If Not ToggleRowColor Then
                        HtmlOut.Append("<tr class='alt_row'>")
                        ToggleRowColor = True
                    Else
                        HtmlOut.Append("<tr bgcolor='white'>")
                        ToggleRowColor = False
                    End If
                    HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")
                    'This function builds the company title/location
                    crmViewDataLayer.CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                    'Display's company field.
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("comp_name")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                    End If
                    HtmlOut.Append("</td>")


                    'Display Aircraft Field.
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amod_make_name")) Then
                        HtmlOut.Append(Trim(r("amod_make_name")) & "/")
                    End If
                    If Not IsDBNull(r("amod_model_name")) Then
                        HtmlOut.Append(Trim(r("amod_model_name")))
                    End If
                    HtmlOut.Append("</td>")


                    'Display Journal Date Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("journ_date")) Then
                        HtmlOut.Append(Trim(r("journ_date")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Serial #/Owner Percent Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_ser_no_full")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, Trim(r("ac_ser_no_full")), "", ""))
                    End If

                    If Not IsDBNull(r("cref_owner_percent")) Then
                        If Trim(Trim(r("cref_owner_percent"))) <> "0" Then
                            HtmlOut.Append(" (" & Trim(r("cref_owner_percent")) & "%)")
                        End If
                    End If
                    HtmlOut.Append("</td>")

                    'Display Reg # Field
                    HtmlOut.Append("<td align= ""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_reg_no")) Then
                        HtmlOut.Append(Trim(r("ac_reg_no")))
                    End If
                    HtmlOut.Append("</td>")

                    'Crm Toggle On
                    '----------------------------------------------------------------------------------------------------------
                    If clsGeneral.clsGeneral.isCrmDisplayMode Then
                        'Let's go ahead and run a check on all three:
                        Run_Check_On_Applicable_Notes(CheckDataTable, AircraftID, r("comp_id"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)

                        'Display Note Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If NoteDisplay <> "" Then
                            HtmlOut.Append(NoteDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "A", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Display Action Field
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If CheckAction Then
                            ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                        End If

                        If ActionDisplay <> "" Then
                            HtmlOut.Append(ActionDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "P", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Display Prospect Field
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If CheckProspect Then
                            ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                        End If

                        If Trim(ProspectDisplay) <> "" Then
                            HtmlOut.Append(ProspectDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "B", TabContainer1.ActiveTabIndex)) '"<a href='#' onclick=""javascript:load('edit.aspx?prospectACID=" & temp_ac_id & "&comp_ID=" & r("comp_id") & "&source=JETNET&type=company&action=checkforcreation&note_type=B','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/gold_plus_sign.png' /></a>" 'DisplayFunctions.WriteNotesRemindersLinks_Action(0, temp_ac_id, Trim(r("comp_id")), 0, True, "&b=1", "<img src='images/gold_plus_sign.png'>")
                        End If
                        HtmlOut.Append("</td>")
                    End If
                    '----------------------------------------------------------------------------------------------------------
                    HtmlOut.Append(DisplayCompanyFields(False, r))
                    HtmlOut.Append("</tr>")

                Next

                HtmlOut.Append("</tbody></table>")

            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If
        Else 'Error reporting needs to go here.
            HtmlOut.Append("<div class='red_text'><p align='center'>An error as occurred.</p>")
        End If

        HtmlOut.Append("</div>")
        Return HtmlOut.ToString
    End Function

    Public Shared Function DisplayCompanyFields(ByVal header As Boolean, ByVal r As DataRow) As String

        Dim HtmlOut2 As New StringBuilder

        If header = False Then
            HtmlOut2.Append("<td align=""left"" valign=""top"">")
            If Not IsDBNull(r("comp_address1")) Then
                HtmlOut2.Append(r("comp_address1"))
                If Not IsDBNull(r("comp_address2")) Then
                    HtmlOut2.Append(" " & r("comp_address2"))
                End If
            End If
            HtmlOut2.Append("</td>")

            'City
            HtmlOut2.Append("<td valign='top' align='left'>")
            If Not IsDBNull(r("comp_city")) Then
                HtmlOut2.Append(r("comp_city") & ", ")
            End If
            HtmlOut2.Append("</td>")

            'State
            HtmlOut2.Append("<td valign='top' align='left'>")
            If Not IsDBNull(r("comp_state")) Then
                HtmlOut2.Append(r("comp_state") & " ")
            End If
            HtmlOut2.Append("</td>")

            'Country
            HtmlOut2.Append("<td valign='top' align='left'>")
            If Not IsDBNull(r("comp_country")) Then
                HtmlOut2.Append(Replace(r("comp_country"), "United States", "U.S."))
            End If
            HtmlOut2.Append("</td>")

            'Office
            HtmlOut2.Append("<td align=""left"" valign=""top"">")
            If Not IsDBNull(r("comp_phone_office")) Then
                HtmlOut2.Append(r("comp_phone_office"))
            End If
            HtmlOut2.Append("</td>")

            'Email
            HtmlOut2.Append("<td align=""left"" valign=""top"">")
            If Not IsDBNull(r("comp_email_address")) Then
                HtmlOut2.Append("<a href='mailto:" & r("comp_email_address") & "'>" & r("comp_email_address") & "</a>")
            End If
            HtmlOut2.Append("</td>")

            'Web
            HtmlOut2.Append("<td align=""left"" valign=""top"">")
            If Not IsDBNull(r("comp_web_address")) Then
                HtmlOut2.Append("<a target=""blank"" href='" & IIf(InStr(r("comp_web_address"), "http://") > 0, r("comp_web_address"), "http://" & r("comp_web_address")) & "'>" & r("comp_web_address") & "</a>")
            End If
            HtmlOut2.Append("</td>")
        Else
            HtmlOut2.Append("<th valign='top' align='left'>Address</th>")

            HtmlOut2.Append("<th valign='top' align='left'>City</th>")
            HtmlOut2.Append("<th valign='top' align='left'>State</th>")
            HtmlOut2.Append("<th valign='top' align='left'>Country</th>")

            HtmlOut2.Append("<th valign='top' align='left'>Office</th>")
            HtmlOut2.Append("<th valign='top' align='left'>Email</th>")
            HtmlOut2.Append("<th valign='top' align='left'>Web</th>")
        End If
        Return HtmlOut2.ToString
    End Function
    ''' <summary>
    ''' Builds the Previous Owner Tab Query
    ''' </summary>
    ''' <param name="MakeSearch"></param>
    ''' <param name="YearDateVariable"></param>
    ''' <param name="Aircraft_Make_Name"></param>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="OrderByVariable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Build_Previous_Owner_Build_Run_Query(ByRef MakeSearch As Boolean, ByRef YearDateVariable As String, ByRef Aircraft_Make_Name As String, ByRef ModelSearchVariable As String, ByRef ViewAll As Boolean, ByRef UpgradeModels() As String, ByRef OrderByVariable As String, ByRef localDataLayer As viewsDataLayer, ByRef ExportEmail As Boolean) As DataTable
        Dim Query As String = ""
        Dim OwnerTable As New DataTable
        '-- **********************************************************
        '-- PREVIOUS OWNERS TAB
        '-- SELECT LIST OF COMPANIES (END USERS)
        '-- THAT SOLD AIRCRAFT (CHALLENGE 300S - BY MODEL ID)
        '-- IN PAST 5 YEARS
        '-- THAT NO LONGER OWN AN AIRCRAFT
        '-- THAT ARE NOT INTERNAL TRANSACTIONS
        Query = " select distinct " & IIf(ViewAll = False, " top 25", "")

        Query = Query & " comp_name, comp_city, comp_state, comp_country, comp_id, "
        Query = Query & "comp_address1, comp_address2, comp_phone_office, comp_phone_fax, comp_email_address, "
        Query = Query & " comp_web_address, cref_owner_percent, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no, journ_date "
        Query = Query & " , contact_first_name, contact_last_name, contact_phone_office, contact_phone_fax, contact_phone_mobile, contact_email_address "
        Query = Query & " from View_Aircraft_Company_History_Flat a with (NOLOCK) "
        Query = Query & " where journ_subcat_code_part1 in ('WS','FS','SS') "

        Query = Query & " and ( cref_contact_type in ('95','69')  ) AND amod_customer_flag = 'Y' "
        Query = Query & " AND (( amod_product_business_flag = 'Y' AND amod_type_code IN ('J','E', 'T','P')) "
        Query = Query & " OR ( amod_product_commercial_flag = 'Y') OR (amod_product_helicopter_flag = 'Y')) "
        Query = Query & " AND ( ac_product_business_flag = 'Y' OR ac_product_commercial_flag = 'Y' "
        Query = Query & " OR ac_product_helicopter_flag = 'Y') "
        Query = Query & " and journ_date > = '" & YearDateVariable & "'"
        Query = Query & " and journ_internal_trans_flag ='N'"
        Query = Query & " and comp_name not like 'Awaiting Doc%'"

        If MakeSearch = True Then
            Query = Query & " and amod_make_name = '" & Trim(Aircraft_Make_Name) & "'"
        ElseIf MakeSearch = False Then
            If ModelSearchVariable <> 0 Then
                Query = Query & " and amod_id = " & ModelSearchVariable & " "
            Else
                Query = Query & " and amod_id in (" & UpgradeModels(0) & ") "
            End If
        End If

        Query = Query & " and cref_business_type in ('EU','AU','A3','AY','AA','AX','AT','FF','FL','SR','TS') "


        ''-- ----------
        ''-- MAKE SURE THAT THE CURRENT COMPANY HAS NOT PURCHASED ANY AIRCRAFT AFTER THEIR LAST SALE
        'Query &= " and not exists ( "
        'Query &= " select distinct comp_id from View_Aircraft_Company_History_Flat with (NOLOCK) "
        'Query &= " where journ_date > DateAdd(d, -30, a.journ_date) "
        'Query &= " and ((comp_id = a.comp_id) "

        ''-- ----------
        ''-- ALSO MAKE SURE THAT ONE OF THEIR RELATED COMPANIES (BUSINESS NAMES, ADDITIONAL LOCATIONS, SUBSIDIARIES)
        'Query &= " or (comp_id in (select distinct compref_rel_comp_id "
        'Query &= " from Company_Reference where compref_comp_id = a.comp_id and compref_journ_id = 0 "
        'Query &= " and compref_contact_type in ('82','59','84')))) "
        'Query &= " and journ_subcat_code_part1 in ('WS','FS','SS') "
        'Query &= " and ( cref_contact_type in ('96','70') )  "
        'Query &= " ) "



        Query &= " And Not exists( "
        Query &= " Select distinct comp_id from View_Aircraft_Company_History_Flat With (NOLOCK)  "
        Query &= " where journ_date > DateAdd(d, -30, a.journ_date) And "
        Query &= " ((comp_id = a.comp_id) "
        Query &= " And journ_subcat_code_part1 In ('WS','FS','SS') and ( cref_contact_type in ('96','70')) )) "

        Query &= " And Not exists (  "
        Query &= " Select distinct comp_id from View_Aircraft_Company_History_Flat With (NOLOCK)  "
        Query &= " where journ_date > DateAdd(d, -30, a.journ_date) And "
        Query &= "(comp_id in (select distinct compref_rel_comp_id from Company_Reference where compref_comp_id = a.comp_id And compref_journ_id = 0 "
        Query &= " And compref_contact_type in ('82','59','84'))) "
        Query &= " And journ_subcat_code_part1 in ('WS','FS','SS') and ( cref_contact_type in ('96','70') )) "



        Query = Query & OrderByVariable

        'Run the Query
        OwnerTable = localDataLayer.Get_CRM_VIEW_Function(Query, "PREVIOUS OWNERS TAB")

        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = OwnerTable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

        ''actually get the distinct values.
        distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            OwnerTable = distinct_table
        End If

        Return OwnerTable
    End Function
#End Region
#Region "Expiring Leases Functions - Crm View"
    ''' <summary>
    ''' Builds the display table for expiring lease functions
    ''' </summary>
    ''' <param name="Temporary_Table"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="count_label"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <param name="TabContainer1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Expiring_Leases_Table(ByRef Temporary_Table As DataTable, ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef count_label As Label, ByRef crm_view_view_all As LinkButton, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByVal activeTabID As String)
        Dim HtmlOut As New StringBuilder
        Dim ToggleRowColor As Boolean = False
        HtmlOut.Append("<div class='prospectDataTableContainer'><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")

        If Not IsNothing(Temporary_Table) Then
            count_label.Text = Temporary_Table.Rows.Count & " Expiring Leases Found."

            If Temporary_Table.Rows.Count = 25 Then
                crm_view_view_all.Visible = True
            End If

            If Temporary_Table.Rows.Count > 0 Then

                HtmlOut.Append("<table width=""100%"" id=""" & activeTabID & "Copy"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")
                HtmlOut.Append("<thead><tr class='header' valign='top'><!--<th valign='top' align='left'>SEL</th>-->")
                HtmlOut.Append("<th valign='top' align='left' width='250'>Lessee</th>")

                HtmlOut.Append("<th valign='top' align='left' width='80'>Lease<br>Expiration</th>")

                HtmlOut.Append("<th valign='top' align='left' width='120'>Make/Model</th>")
                HtmlOut.Append("<th valign='top' align='left'>Ser No</th>")
                HtmlOut.Append("<th valign='top' align='left'>Reg No</th>")
                HtmlOut.Append("<th valign='top' align='left' width='270'>Subject</th>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Note display
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Action item display
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Prospect Display
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                End If
                '-----------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(True, Temporary_Table.Rows(0)))
                HtmlOut.Append("</tr></thead><tbody>")

                For Each r As DataRow In Temporary_Table.Rows
                    Dim NoteDisplay As String = ""
                    Dim ActionDisplay As String = ""
                    Dim ProspectDisplay As String = ""

                    Dim CheckDataTable As New DataTable
                    Dim CheckNote As Boolean = False
                    Dim CheckProspect As Boolean = False
                    Dim CheckAction As Boolean = False

                    If Not ToggleRowColor Then
                        HtmlOut.Append("<tr class='alt_row'>")
                        ToggleRowColor = True
                    Else
                        HtmlOut.Append("<tr bgcolor='white'>")
                        ToggleRowColor = False
                    End If

                    HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")

                    HtmlOut.Append("<td align=""left"" valign=""top"">")

                    'Builds the company location/title display
                    crmViewDataLayer.CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                    'Display Company Field
                    If Not IsDBNull(r("comp_name")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                    End If
                    HtmlOut.Append("</td>")


                    'Display Journal Date Field/Lease Expiration
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("journ_date")) Then
                        HtmlOut.Append(Trim(r("journ_date")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Make/Model Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amod_make_name")) Then
                        HtmlOut.Append(Trim(r("amod_make_name")) & "/")
                    End If
                    If Not IsDBNull(r("amod_model_name")) Then
                        HtmlOut.Append(Trim(r("amod_model_name")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Ser # field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_ser_no_full")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, Trim(r("ac_ser_no_full")), "", ""))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Reg # Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_reg_no")) Then
                        HtmlOut.Append(Trim(r("ac_reg_no")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Subject Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("journ_subject")) Then
                        HtmlOut.Append(Trim(r("journ_subject")))
                    End If
                    HtmlOut.Append("</td>")

                    'Crm Toggle On
                    '----------------------------------------------------------------------------------------------------------
                    If clsGeneral.clsGeneral.isCrmDisplayMode Then
                        'Let's go ahead and run a check on all three:
                        Run_Check_On_Applicable_Notes(CheckDataTable, AircraftID, r("comp_id"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)

                        'Note Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If NoteDisplay <> "" Then
                            HtmlOut.Append(NoteDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "A", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Action Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If CheckAction Then
                            ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), 0, aclsData_Temp, "COMP", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                        End If

                        If ActionDisplay <> "" Then
                            HtmlOut.Append(ActionDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "P", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")


                        'Prospect Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckProspect Then
                            ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                        End If

                        If Trim(ProspectDisplay) <> "" Then
                            HtmlOut.Append(ProspectDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "B", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                    End If
                    '----------------------------------------------------------------------------------------------------------
                    HtmlOut.Append(DisplayCompanyFields(False, r))


                    HtmlOut.Append("</tr>")
                Next


                HtmlOut.Append("</tbody></table>")
            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If

        End If

        HtmlOut.Append("</div>")

        Return HtmlOut.ToString
    End Function
    ''' <summary>
    ''' Builds the expiring leases query
    ''' </summary>
    ''' <param name="MakeSearch"></param>
    ''' <param name="YearVariable"></param>
    ''' <param name="Aircraft_Make_Name"></param>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="OrderByVariable"></param>
    ''' <param name="DoNotIncludeExpired"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Build_Expiring_Leases_Build_Run_Query(ByRef MakeSearch As Boolean, ByRef YearVariable As Long, ByRef Aircraft_Make_Name As String, ByRef ModelSearchVariable As String, ByRef ViewAll As Boolean, ByRef UpgradeModels() As String, ByRef OrderByVariable As String, ByRef DoNotIncludeExpired As Boolean, ByRef localdatalayer As viewsDataLayer, ByRef ExportEmail As Boolean) As DataTable
        Dim Query As String = ""
        Dim ResultsTable As New DataTable
        '-- =============================================================================
        '-- EXPIRING LEASES TAB
        '-- LEASES DUE TO EXPIRE FOR END USERS OVER NEXT 2 YEARS
        Query = " SELECT " & IIf(ViewAll = False, " top 25", "") & " comp_name, comp_city,  comp_state, comp_country, "
        Query = Query & " aclease_expiration_date as journ_date, comp_id, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, "
        Query = Query & " amod_id, journ_subject, journ_id"
        Query = Query & ",comp_address1, comp_address2, comp_email_address, comp_web_address, "
        Query = Query & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
        Query = Query & " and pnum_type='Office') as comp_phone_office,"
        Query = Query & " (select top 1 pnum_number_full from phone_numbers where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 "
        Query = Query & " and pnum_type='Fax') as comp_phone_fax"
        Query = Query & " FROM aircraft WITH(NOLOCK) "
        Query = Query & " INNER JOIN Aircraft_Lease WITH(NOLOCK) ON ac_id = aclease_ac_id and ac_journ_id = aclease_journ_id "
        Query = Query & " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id "
        Query = Query & " INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id=cref_ac_id and ac_journ_id = cref_journ_id "
        Query = Query & " LEFT OUTER JOIN company WITH(NOLOCK) ON cref_comp_id = comp_id and cref_journ_id = comp_journ_id "
        Query = Query & " INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id "
        Query = Query & " WHERE ac_journ_id > 0 "
        Query = Query & " AND aclease_expired = 'N' "
        Query = Query & " AND cref_contact_type in ('12', '39') " '-- LESSEE OR SUBLESSEE"

        Query = Query & " AND ( "

        ' if there is a not in there, then put today date on
        If DoNotIncludeExpired = True Then
            Query = Query & "  (aclease_expiration_date >= GETDATE()) AND "
        End If

        Query = Query & "  (aclease_expiration_date < GETDATE()+" & (YearVariable * 365) & ")) "
        Query = Query & " and cref_business_type in ('EU','AU','A3','AY','AA','AX','AT','FF','FL','SR','TS') "


        'If make search is on
        If MakeSearch = True Then
            Query = Query & " and amod_make_name = '" & Trim(Aircraft_Make_Name) & "'"
        ElseIf MakeSearch = False Then 'Otherwise we're doing a model search
            If ModelSearchVariable <> 0 Then
                Query = Query & " and amod_id = " & ModelSearchVariable & " " 'Single model search
            Else
                Query = Query & " and amod_id in (" & UpgradeModels(0) & ") " 'Common upgrade model search
            End If
        End If

        Query = Query & " and comp_name <> 'Awaiting Documentaton'"
        Query = Query & " AND amod_airframe_type_code='F' AND amod_type_code = 'J' AND ac_product_business_flag = 'Y' "
        Query = Query & " AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y' AND amod_airframe_type_code = 'R' "
        Query = Query & " AND ac_product_helicopter_flag = 'Y') OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' "
        Query = Query & " AND ac_product_business_flag = 'Y') OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F' "
        Query = Query & " AND ac_product_commercial_flag = 'Y')) "
        Query = Query & OrderByVariable

        ResultsTable = localdatalayer.Get_CRM_VIEW_Function(Query, "EXPIRING LEASES TAB")


        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = ResultsTable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            ResultsTable = distinct_table
        End If


        Return ResultsTable
    End Function
#End Region
#Region "Expiring Fractional Owners Functions - CRM View"
    ''' <summary>
    ''' Builds the query for the Expiring Fractional Owners
    ''' </summary>
    ''' <param name="MakeSearch"></param>
    ''' <param name="YearVariable"></param>
    ''' <param name="Aircraft_Make_Name"></param>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="OrderByVariable"></param>
    ''' <param name="DoNotIncludeExpired"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Build_Expiring_Fractional_Owner_Build_Run_Query(ByRef MakeSearch As Boolean, ByRef YearVariable As Long, ByRef Aircraft_Make_Name As String, ByRef ModelSearchVariable As String, ByRef ViewAll As Boolean, ByRef UpgradeModels() As String, ByRef OrderByVariable As String, ByRef DoNotIncludeExpired As Boolean, ByRef LocalDataLayer As viewsDataLayer, ByRef ExportEmail As Boolean)
        Dim Query As String = ""
        Dim ResultsTable As New DataTable
        '-- ======================================================================================
        '-- EXPIRING FRACTIONAL OWNERS
        '-- FRACTIONAL OWNERS WITH EXPIRING AGREEMENTS
        Query = " select distinct " & IIf(ViewAll = False, " top 25", "") & " comp_name, comp_city,comp_state, comp_country, comp_id, amod_make_name, cref_owner_percent, "
        Query = Query & " amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, cref_fraction_expires_date as journ_date "
        Query = Query & ",comp_address1, comp_address2, comp_phone_office, comp_phone_fax, comp_email_address, comp_web_address"
        Query = Query & " from View_Aircraft_Company_Flat with (NOLOCK) "

        ' if there is a not in there 
        If DoNotIncludeExpired Then
            Query = Query & " where cref_fraction_expires_date between '" & Date.Now.Date & "' and '" & DateAdd(DateInterval.Year, CInt(YearVariable), Date.Now.Date) & "' "
        Else ' include previous dates
            Query = Query & " where cref_fraction_expires_date < '" & DateAdd(DateInterval.Year, CInt(YearVariable), Date.Now.Date) & "' "
        End If

        Query = Query & " and ( ( cref_contact_type = '97' ) ) "
        Query = Query & " and cref_business_type in ('EU','AU','A3','AY','AA','AX','AT','FF','FL','SR','TS') "

        'If make search is on
        If MakeSearch = True Then
            Query = Query & " and amod_make_name = '" & Trim(Aircraft_Make_Name) & "'"
        ElseIf MakeSearch = False Then 'Otherwise we're doing a model search
            If ModelSearchVariable <> 0 Then
                Query = Query & " and amod_id = " & ModelSearchVariable & " " 'Single model search
            Else
                Query = Query & " and amod_id in (" & UpgradeModels(0) & ") " 'Common upgrade model search
            End If
        End If

        Query = Query & " AND amod_customer_flag = 'Y' AND (( amod_product_business_flag = 'Y' "
        Query = Query & " AND amod_type_code IN ('J','E', 'T','P')) OR ( amod_product_commercial_flag = 'Y') "
        Query = Query & " OR (amod_product_helicopter_flag = 'Y')) AND ( ac_product_business_flag = 'Y' "
        Query = Query & " OR ac_product_commercial_flag = 'Y' OR ac_product_helicopter_flag = 'Y') "
        Query = Query & OrderByVariable

        ResultsTable = LocalDataLayer.Get_CRM_VIEW_Function(Query, "EXPIRING FRACTIONAL OWNERS")

        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = ResultsTable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            ResultsTable = distinct_table
        End If

        Return ResultsTable
    End Function
    ''' <summary>
    ''' Builds the table display for Expiring Fractional Owners
    ''' </summary>
    ''' <param name="Temporary_Table"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="count_label"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <param name="TabContainer1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Expiring_Fractional_Owner_Table(ByRef Temporary_Table As DataTable, ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef count_label As Label, ByRef crm_view_view_all As LinkButton, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByVal activeTabID As String)
        Dim HtmlOut As New StringBuilder
        Dim ToggleRowColor As Boolean = False

        HtmlOut.Append("<div class=""prospectDataTableContainer""><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")
        If Not IsNothing(Temporary_Table) Then
            count_label.Text = Temporary_Table.Rows.Count & " Expiring Fractional Owners Found."

            If Temporary_Table.Rows.Count = 25 Then
                crm_view_view_all.Visible = True
            End If

            If Temporary_Table.Rows.Count > 0 Then

                HtmlOut.Append("<table width=""100%"" id=""" & activeTabID & "Copy"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")
                HtmlOut.Append("<thead><tr class='header'><!--<th valign='top' align='left'>SEL</th>-->")
                HtmlOut.Append("<th valign='top' align='left'>Fractional Owner</em></th>")

                HtmlOut.Append("<th valign='top' align='left'>Make/Model</th>")
                HtmlOut.Append("<th valign='top' align='left'>Frac Expires Date</th>")
                HtmlOut.Append("<th valign='top' align='left'>Ser No</th>")
                HtmlOut.Append("<th valign='top' align='left'>Reg No</th>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Note Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Action Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Prospect Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                End If
                '----------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(True, Temporary_Table.Rows(0)))
                HtmlOut.Append("</tr></thead><tbody>")

                For Each r As DataRow In Temporary_Table.Rows
                    Dim NoteDisplay As String = ""
                    Dim ActionDisplay As String = ""
                    Dim ProspectDisplay As String = ""

                    Dim CheckDataTable As New DataTable
                    Dim CheckNote As Boolean = False
                    Dim CheckProspect As Boolean = False
                    Dim CheckAction As Boolean = False

                    If Not ToggleRowColor Then
                        HtmlOut.Append("<tr class='alt_row'>")
                        ToggleRowColor = True
                    Else
                        HtmlOut.Append("<tr bgcolor='white'>")
                        ToggleRowColor = False
                    End If
                    HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")

                    'Build the company title/location
                    crmViewDataLayer.CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                    'Display company field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("comp_name")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Make/Model field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amod_make_name")) Then
                        HtmlOut.Append(Trim(r("amod_make_name")) & "/")
                    End If

                    If Not IsDBNull(r("amod_model_name")) Then
                        HtmlOut.Append(Trim(r("amod_model_name")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Journal Date Field (Frac Expires Date)
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("journ_date")) Then
                        HtmlOut.Append(Trim(r("journ_date")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display Ser/Owner Percent Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_ser_no_full")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, Trim(r("ac_ser_no_full")), "", ""))
                    End If

                    If Not IsDBNull(r("cref_owner_percent")) Then
                        HtmlOut.Append(" (" & Trim(r("cref_owner_percent")) & "%)")
                    End If
                    HtmlOut.Append("</td>")

                    'Display Reg #  Field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_reg_no")) Then
                        HtmlOut.Append(Trim(r("ac_reg_no")))
                    End If

                    HtmlOut.Append("&nbsp;</td>")

                    'Crm Toggle On
                    '----------------------------------------------------------------------------------------------------------
                    If clsGeneral.clsGeneral.isCrmDisplayMode Then
                        'Let's go ahead and run a check on all three:
                        Run_Check_On_Applicable_Notes(CheckDataTable, AircraftID, r("comp_id"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)


                        'Note Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If NoteDisplay <> "" Then
                            HtmlOut.Append(NoteDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "A", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Action Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckAction Then
                            ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), 0, aclsData_Temp, "COMP", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                        End If

                        If ActionDisplay <> "" Then
                            HtmlOut.Append(ActionDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "P", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")


                        'Prospect Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckProspect Then
                            ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                        End If

                        If Trim(ProspectDisplay) <> "" Then
                            HtmlOut.Append(ProspectDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "B", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")
                    End If
                    '--------------------------------------------------------------------------------------------------------------------------

                    HtmlOut.Append(DisplayCompanyFields(False, r))
                    HtmlOut.Append("</tr>")
                Next
                HtmlOut.Append("</tbody></table>")
            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If
        End If

        HtmlOut.Append("</div>")
        Return HtmlOut.ToString
    End Function
#End Region
#Region "Recent Sales Tab Functions - CRM VIEW"
    ''' <summary>
    ''' Build Recent Sales Tab Query
    ''' </summary>
    ''' <param name="MakeSearch"></param>
    ''' <param name="YearDateVariable"></param>
    ''' <param name="Aircraft_Make_Name"></param>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="OrderByVariable"></param>
    ''' <param name="localDatalayer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Recent_Sales_Build_Run_Query(ByRef MakeSearch As Boolean, ByRef YearDateVariable As String, ByRef Aircraft_Make_Name As String, ByRef ModelSearchVariable As String, ByRef ViewAll As Boolean, ByRef UpgradeModels() As String, ByRef OrderByVariable As String, ByRef localDatalayer As viewsDataLayer, ByRef ExportEmail As Boolean) As DataTable
        Dim Query As String = ""
        Dim ResultsTable As New DataTable

        '-- ==========================================================================
        '-- RECENT SALES TAB
        '-- GET DEALER COMPANIES THAT RECENTLY SOLD AIRCRAFT
        Query = " select distinct " & IIf(ViewAll = False, " top 25", "") & " comp_name, comp_city, comp_state, comp_country, comp_id "
        Query = Query & ", comp_address1, comp_address2, comp_phone_office, comp_phone_fax, comp_email_address, comp_web_address, contact_first_name, contact_last_name,contact_title, contact_id, contact_email_address, "
        Query = Query & " (select top 1 pnum_number_full from Phone_Numbers where pnum_comp_id = comp_id "
        Query = Query & " and pnum_contact_id = contact_id and pnum_journ_id = 0 and pnum_type='Office') as contact_office_phone, "
        Query = Query & " (select top 1 pnum_number_full from Phone_Numbers where pnum_comp_id = comp_id "
        Query = Query & "and pnum_contact_id = contact_id and pnum_journ_id = 0 and pnum_type='Mobile') as contact_mobile_phone, journ_subcat_code_part1, journ_date, ac_id, amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no  "
        Query = Query & " from View_Aircraft_Company_History_Flat with (NOLOCK) "
        Query = Query & " where journ_subcat_code_part1 in ('WS','SS','FS') "
        Query = Query & "  and journ_date > = '" & YearDateVariable & "' "

        'If make search is on
        If MakeSearch = True Then
            Query = Query & " and amod_make_name = '" & Trim(Aircraft_Make_Name) & "'"
        ElseIf MakeSearch = False Then 'Otherwise we're doing a model search
            If ModelSearchVariable <> 0 Then
                Query = Query & " and amod_id = " & ModelSearchVariable & " " 'Single model search
            Else
                Query = Query & " and amod_id in (" & UpgradeModels(0) & ") " 'Common upgrade model search
            End If
        End If

        Query = Query & " and ( cref_contact_type = '99' ) "
        Query = Query & " AND amod_customer_flag = 'Y' "
        Query = Query & " AND (( amod_product_business_flag = 'Y' AND amod_type_code IN ('J','E', 'T','P')) "
        Query = Query & " OR ( amod_product_commercial_flag = 'Y') OR (amod_product_helicopter_flag = 'Y')) "
        Query = Query & " AND ( ac_product_business_flag = 'Y' OR ac_product_commercial_flag = 'Y' "
        Query = Query & " OR ac_product_helicopter_flag = 'Y') "
        Query = Query & OrderByVariable

        'Run Query 
        ResultsTable = localDatalayer.Get_CRM_VIEW_Function(Query, "RECENT SALES TAB")

        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = ResultsTable.DefaultView
            distinct_table_view.Sort = "contact_email_address"

            distinct_table_view.RowFilter = "contact_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "contact_email_address")
            ResultsTable = distinct_table
        End If

        Return ResultsTable
    End Function
    ''' <summary>
    ''' Build Recent Sales Datatable
    ''' </summary>
    ''' <param name="Temporary_Table"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="count_label"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <param name="TabContainer1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Recent_Sales_Table(ByRef Temporary_Table As DataTable, ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef count_label As Label, ByRef crm_view_view_all As LinkButton, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByVal activeTabID As String) As String
        Dim HtmlOut As New StringBuilder
        Dim ToggleRowColor As Boolean = False

        HtmlOut.Append("<div class=""prospectDataTableContainer""><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")

        If Not IsNothing(Temporary_Table) Then
            count_label.Text = Temporary_Table.Rows.Count & " Recent Sales Found."
            If Temporary_Table.Rows.Count = 25 Then
                crm_view_view_all.Visible = True
            End If

            If Temporary_Table.Rows.Count > 0 Then

                HtmlOut.Append("<table width=""100%"" id=""" & activeTabID & "Copy"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")
                HtmlOut.Append("<thead><tr class='header'><!--<th align='left' valign='top'>SEL</th>--><th valign='top' align='left'>Exclusive Broker</em></th>")

                HtmlOut.Append("<th align='left' valign='top'>Contact</th>")
                HtmlOut.Append("<th align='left' valign='top'>Title</th>")

                HtmlOut.Append("<th align='left' valign='top'>Make/Model</th>")
                HtmlOut.Append("<th align='left' valign='top'>Date Sold</th>")
                HtmlOut.Append("<th align='left' valign='top'>Ser No</th>")
                HtmlOut.Append("<th align='left' valign='top'>Reg No</th>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Note Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Action Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Prospect Field
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                End If
                '----------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(True, Temporary_Table.Rows(0)))
                HtmlOut.Append("</tr></thead><tbody>")

                For Each r As DataRow In Temporary_Table.Rows
                    Dim ContactInformation As String = ""
                    Dim NoteDisplay As String = ""
                    Dim ActionDisplay As String = ""
                    Dim ProspectDisplay As String = ""

                    Dim CheckDataTable As New DataTable
                    Dim CheckNote As Boolean = False
                    Dim CheckProspect As Boolean = False
                    Dim CheckAction As Boolean = False

                    If Not ToggleRowColor Then
                        HtmlOut.Append("<tr class='alt_row'>")
                        ToggleRowColor = True
                    Else
                        HtmlOut.Append("<tr bgcolor='white'>")
                        ToggleRowColor = False
                    End If

                    'Build Contact Information Display.
                    If Not IsDBNull(r("contact_first_name")) Then
                        ContactInformation = r("contact_first_name").ToString
                    End If

                    If Not IsDBNull(r("contact_last_name")) Then
                        If ContactInformation <> "" Then
                            ContactInformation += " "
                        End If
                        ContactInformation += r("contact_last_name").ToString
                    End If
                    'Build Contact Link
                    If Not IsDBNull(r("contact_id")) Then
                        ContactInformation = DisplayFunctions.WriteDetailsLink(0, r("comp_id"), r("contact_id"), 0, True, ContactInformation, "", "")
                    End If

                    HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")


                    'Build company location/title string
                    crmViewDataLayer.CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                    'Company 
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("comp_name")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                    End If
                    HtmlOut.Append("</td>")


                    'Display Contact Field:
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    HtmlOut.Append(ContactInformation)
                    HtmlOut.Append("</td>")

                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    'Append Contact Title.
                    If Not IsDBNull(r("contact_title")) Then
                        HtmlOut.Append(r("contact_title"))
                    End If
                    HtmlOut.Append("</td>")


                    'The model field.
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amod_make_name")) Then
                        HtmlOut.Append(Trim(r("amod_make_name")) & "/")
                    End If
                    If Not IsDBNull(r("amod_model_name")) Then
                        HtmlOut.Append(Trim(r("amod_model_name")))
                    End If
                    HtmlOut.Append("</td>")

                    Dim dateSort As String = ""
                    If Not IsDBNull(r.Item("journ_date")) Then
                        If IsDate(r.Item("journ_date").ToString) Then
                            dateSort = Format(r.Item("journ_date"), "yyyy/MM/dd")
                        End If
                    End If

                    'Display Journal Date.
                    HtmlOut.Append("<td align=""left"" valign=""top"" data-sort=""" & dateSort & """>")
                    If Not IsDBNull(r("journ_date")) Then
                        HtmlOut.Append(Trim(r("journ_date")))
                    End If
                    HtmlOut.Append("</td>")

                    'Serial #/Share/Fraction
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_ser_no_full")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, Trim(r("ac_ser_no_full")), "", ""))
                    End If
                    If Not IsDBNull(r("journ_subcat_code_part1")) Then
                        If Trim(r("journ_subcat_code_part1")) = "SS" Then
                            HtmlOut.Append(" (Share)")
                        ElseIf Trim(r("journ_subcat_code_part1")) = "FS" Then
                            HtmlOut.Append(" (Fraction)")
                        End If
                    End If
                    HtmlOut.Append("</td>")

                    'Reg #
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("ac_reg_no")) Then
                        HtmlOut.Append(Trim(r("ac_reg_no")))
                    End If
                    HtmlOut.Append("&nbsp;</td>")

                    'Crm Toggle On
                    '----------------------------------------------------------------------------------------------------------
                    If clsGeneral.clsGeneral.isCrmDisplayMode Then
                        'Let's go ahead and run a check on all three:
                        Run_Check_On_Applicable_Notes(CheckDataTable, AircraftID, r("comp_id"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)

                        'Note Field
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If NoteDisplay <> "" Then
                            HtmlOut.Append(NoteDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "A", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Action Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckAction Then
                            ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), 0, aclsData_Temp, "COMP", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                        End If

                        If ActionDisplay <> "" Then
                            HtmlOut.Append(ActionDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "P", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")

                        'Prospect Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckProspect Then
                            ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                        End If

                        If Trim(ProspectDisplay) <> "" Then
                            HtmlOut.Append(ProspectDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "B", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")
                    End If
                    '----------------------------------------------------------------------------------------------------------------
                    HtmlOut.Append(DisplayCompanyFields(False, r))
                    HtmlOut.Append("</tr>")
                Next


                HtmlOut.Append("</tbody></table>")

            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If

        End If

        HtmlOut.Append("</div>")

        Return HtmlOut.ToString
    End Function
#End Region
#Region "Wanted Tab Function - CRM VIEW"
    ''' <summary>
    ''' Builds wanted tab query for CRM View
    ''' </summary>
    ''' <param name="MakeSearch"></param>
    ''' <param name="YearDateVariable"></param>
    ''' <param name="Aircraft_Make_Name"></param>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="OrderByVariable"></param>
    ''' <param name="DoNotIncludeBroker"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Wanted_Build_Run_Query(ByRef MakeSearch As Boolean, ByRef YearDateVariable As String, ByRef Aircraft_Make_Name As String, ByRef ModelSearchVariable As String, ByRef ViewAll As Boolean, ByRef UpgradeModels() As String, ByRef OrderByVariable As String, ByRef DoNotIncludeBroker As Boolean, ByRef LocalDataLayer As viewsDataLayer, ByRef ExportEmail As Boolean) As DataTable
        Dim Query As String = ""
        Dim ResultsTable As New DataTable
        ''-- ===========================================================
        '-- WANTEDS TAB 
        '-- MODELS WANTED - DEFAULT TO THOSE WANTED BY END USERS
        Query = Query & " select distinct " & IIf(ViewAll = False, " top 25", "") & " comp_name,comp_city, comp_state, comp_country, comp_id, amwant_listed_date as journ_date, "
        Query = Query & " amod_make_name,   amod_model_name, amwant_amod_id, amwant_id,"
        Query = Query & " amwant_notes, amwant_start_year, amwant_end_year, "
        Query = Query & " amwant_max_price, amwant_max_aftt, contact_first_name, contact_last_name, contact_id  "
        'JETNET' as source 
        Query = Query & ",comp_address1, comp_address2, comp_phone_office, comp_phone_fax, comp_email_address, comp_web_address"
        Query = Query & " from view_aircraft_model_wanted WITH(NOLOCK)"
        Query = Query & "  where(amwant_journ_id = 0)"
        Query = Query & " AND (amwant_verified_date IS NOT NULL) "

        If DoNotIncludeBroker Then
            Query = Query & " and comp_business_type='EU' "
        End If


        'If make search is on
        If MakeSearch = True Then
            Query = Query & " and amod_make_name = '" & Trim(Aircraft_Make_Name) & "'"
        ElseIf MakeSearch = False Then 'Otherwise we're doing a model search
            If ModelSearchVariable <> 0 Then
                Query = Query & " and amwant_amod_id = " & ModelSearchVariable & " " 'Single model search
            Else
                Query = Query & " and amwant_amod_id in (" & UpgradeModels(0) & ") " 'Common upgrade model search
            End If
        End If


        Query = Query & " and amwant_listed_date > = '" & YearDateVariable & "' "

        Query = Query & " and amod_type_code in ('J') and amod_airframe_type_code in ('F') "
        Query = Query & "AND amod_customer_flag = 'Y' "
        Query = Query & " AND (( amod_product_business_flag = 'Y' AND amod_type_code IN ('J','E', 'T','P')) "
        Query = Query & " OR ( amod_product_commercial_flag = 'Y') "
        Query = Query & " OR (amod_product_helicopter_flag = 'Y')) "
        Query = Query & OrderByVariable

        'Run Query for tab
        ResultsTable = LocalDataLayer.Get_CRM_VIEW_Function(Query, "WANTEDS TAB")

        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = ResultsTable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            ResultsTable = distinct_table
        End If

        Return ResultsTable
    End Function
    ''' <summary>
    ''' Displays Wanted Tab Table
    ''' </summary>
    ''' <param name="Temporary_Table"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="count_label"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <param name="TabContainer1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Wanted_Table(ByRef Temporary_Table As DataTable, ByRef CompanyTitle As String, ByRef CompanyLocation As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef count_label As Label, ByRef crm_view_view_all As LinkButton, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByVal activeTabID As String)
        Dim HtmlOut As New StringBuilder
        Dim ToggleRowColor As Boolean = False

        HtmlOut.Append("<div class=""prospectDataTableContainer""><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")
        If Not IsNothing(Temporary_Table) Then
            count_label.Text = Temporary_Table.Rows.Count & " Wanteds Found."
            If Temporary_Table.Rows.Count = 25 Then
                crm_view_view_all.Visible = True
            End If

            If Temporary_Table.Rows.Count > 0 Then

                HtmlOut.Append("<table width=""100%"" id=""" & activeTabID & "Copy"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")
                HtmlOut.Append("<thead><tr class='header'><!--<th valign='top' align='left'>SEL</th>--><th valign='top' align='left'>Company</em></th>")

                HtmlOut.Append("<th valign='top' align='left'>Make/Model</th>")

                HtmlOut.Append("<th valign='top' align='left'>List Date</th>")
                HtmlOut.Append("<th valign='top' align='left'>Notes</th>")
                HtmlOut.Append("<th valign='top' align='left'>Start Year</th>")
                HtmlOut.Append("<th valign='top' align='left'>End Year</th>")
                HtmlOut.Append("<th valign='top' align='left'>Max Price</th>")
                HtmlOut.Append("<th valign='top' align='left'>Max Aftt</th>")
                HtmlOut.Append("<th valign='top' align='left'>Contact</th>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Note Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Action Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                    'Prospect Field.
                    HtmlOut.Append("<th valign='top' align='left' width=""17"">&nbsp;</th>")
                End If
                '--------------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(True, Temporary_Table.Rows(0)))
                HtmlOut.Append("</tr></thead>")

                For Each r As DataRow In Temporary_Table.Rows
                    Dim ProspectDisplay As String = ""
                    Dim NoteDisplay As String = ""
                    Dim ActionDisplay As String = ""

                    Dim CheckDataTable As New DataTable
                    Dim CheckNote As Boolean = False
                    Dim CheckProspect As Boolean = False
                    Dim CheckAction As Boolean = False

                    If Not ToggleRowColor Then
                        HtmlOut.Append("<tr class='alt_row'>")
                        ToggleRowColor = True
                    Else
                        HtmlOut.Append("<tr bgcolor='white'>")
                        ToggleRowColor = False
                    End If

                    HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")
                    'build's company title/location
                    crmViewDataLayer.CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                    'Display company field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("comp_name")) Then
                        HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                    End If
                    HtmlOut.Append("</td>")



                    'Display make/model
                    HtmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(r("amod_make_name")) Then
                        HtmlOut.Append(Trim(r("amod_make_name")) & "/")
                    End If

                    If Not IsDBNull(r("amod_model_name")) Then
                        HtmlOut.Append(Trim(r("amod_model_name")))
                    End If

                    HtmlOut.Append("</td>")

                    'Display date
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("journ_date")) Then
                        HtmlOut.Append(Trim(r("journ_date")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display wanted notes
                    HtmlOut.Append("<td align=""left"" valign=""top"" width='250'>")
                    If Not IsDBNull(r("amwant_notes")) Then
                        HtmlOut.Append(Trim(r("amwant_notes")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display start year
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amwant_start_year")) Then
                        HtmlOut.Append(Trim(r("amwant_start_year")))
                    End If
                    HtmlOut.Append("</td>")

                    'Display end year
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amwant_end_year")) Then
                        HtmlOut.Append(Trim(r("amwant_end_year")))
                    End If
                    HtmlOut.Append("</td>")

                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amwant_max_price")) Then
                        HtmlOut.Append(Trim(r("amwant_max_price")))
                    End If
                    HtmlOut.Append("</td>")

                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("amwant_max_aftt")) Then
                        HtmlOut.Append(Trim(r("amwant_max_aftt")))
                    End If
                    HtmlOut.Append("</td>")

                    'Contact field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r("contact_first_name")) Then
                        HtmlOut.Append(Trim(r("contact_first_name")) & " ")
                    End If

                    If Not IsDBNull(r("contact_last_name")) Then
                        HtmlOut.Append(Trim(r("contact_last_name")))
                    End If
                    HtmlOut.Append("</td>")

                    'Crm Toggle On
                    '----------------------------------------------------------------------------------------------------------
                    If clsGeneral.clsGeneral.isCrmDisplayMode Then
                        'Let's go ahead and run a check on all three:
                        Run_Check_On_Applicable_Notes(CheckDataTable, AircraftID, r("comp_id"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)


                        'Note Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        If NoteDisplay <> "" Then
                            HtmlOut.Append(NoteDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "A", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")


                        'Action Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckAction Then
                            ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), 0, aclsData_Temp, "COMP", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                        End If

                        If ActionDisplay <> "" Then
                            HtmlOut.Append(ActionDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "P", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")


                        'Prospect Field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        If CheckProspect Then
                            ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("comp_id")), AircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                        End If

                        If Trim(ProspectDisplay) <> "" Then
                            HtmlOut.Append(ProspectDisplay)
                        Else
                            HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(AircraftID, r("comp_id"), "B", TabContainer1.ActiveTabIndex))
                        End If
                        HtmlOut.Append("</td>")
                    End If
                    '-----------------------------------------------------------------------------------------------------------------
                    HtmlOut.Append(DisplayCompanyFields(False, r))
                    HtmlOut.Append("</tr>")
                Next


                HtmlOut.Append("</table>")

            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If
        End If

        HtmlOut.Append("</div>")
        Return HtmlOut.ToString
    End Function
#End Region
#Region "Time To Buy Tab Functions - CRM VIEW"
    ''' <summary>
    ''' Display/Build Query for Tab
    ''' </summary>
    ''' <param name="ModelID"></param>
    ''' <param name="ForSaleStatus"></param>
    ''' <param name="CommonUpgradeModels"></param>
    ''' <param name="PreviouslyOwned"></param>
    ''' <param name="LifecyclePercentage"></param>
    ''' <param name="CompanySort"></param>
    ''' <param name="ViewAll"></param>
    ''' <param name="LocalDataLayer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Find_Owners_From_Upgrade_Based_On_Parameters(ByRef ModelID As Long, ByRef ForSaleStatus As String, ByRef CommonUpgradeModels As Long, ByRef PreviouslyOwned As String, ByRef LifecyclePercentage As Decimal, ByRef CompanySort As String, ByRef ViewAll As Boolean, ByRef LocalDataLayer As viewsDataLayer, ByRef ExportEmail As Boolean) As DataTable
        Dim OwnersDatatable As New DataTable
        Dim sqlQuery As String = ""

        '-- Now Find Owners Of Those Aircraft Models From The Upgrade From Report
        '-- That Have Owned That Aircraft Longer Than The Length Of Ownership Report States
        '-- Based on Used

        sqlQuery += "SELECT " & IIf(ViewAll = False, " top 25", "")

        sqlQuery += " comp_id As CompId, comp_name As Company,  comp_address1, comp_address2, comp_web_address,"
        sqlQuery += " comp_country, comp_state, comp_city, "
        sqlQuery += " comp_email_address,"
        sqlQuery += " (select top 1 pnum_number_full from Phone_Numbers where pnum_comp_id = comp_id"
        sqlQuery += " and pnum_contact_id = 0 and pnum_journ_id = 0 and pnum_type='Office') as comp_phone_office, "
        sqlQuery += " (select top 1 pnum_number_full from Phone_Numbers where pnum_comp_id = comp_id "
        sqlQuery += " and pnum_contact_id = 0 and pnum_journ_id = 0 and pnum_type='Fax') as comp_phone_fax, "


        sqlQuery += "(SELECT TOP 1 ac4_new_days_owned FROM star_reports.dbo.Aircraft_4 WITH (NOLOCK) WHERE  (ac4_new_days_owned > 0) AND (ac_amod_id = ac4_amod_id) ORDER BY ac4_start_date DESC ) as looavgnew, "
        sqlQuery += "(SELECT TOP 1 ac4_used_days_owned FROM star_reports.dbo.Aircraft_4 WITH (NOLOCK) WHERE (ac4_used_days_owned > 0) AND (ac_amod_id = ac4_amod_id) ORDER BY ac4_start_date DESC ) as looavgused, "

        sqlQuery += "amod_id As AModId, amod_airframe_type_code As AirframeType, amod_type_code As MakeType, amod_make_name As Make, amod_model_name As Model, "
        sqlQuery += "ac_id As ACId, ac_ser_no_full As SerNbr, ac_reg_no As RegNbr, ac_forsale_flag as ForSale, "
        sqlQuery += "CAST(ac_purchase_date AS DATE) As PurchaseDate, DATEDIFF(DAY,ac_purchase_date,GETDATE()) As NbrDaysOwned, "
        sqlQuery += " ac_previously_owned_flag As PreviouslyOwned "

        sqlQuery += "FROM jetnet_ra.dbo.Aircraft_Model WITH (NOLOCK) "
        sqlQuery += "INNER JOIN jetnet_ra.dbo.Aircraft WITH (NOLOCK) ON ac_amod_id = amod_id "
        sqlQuery += "INNER JOIN jetnet_ra.dbo.Aircraft_Reference WITH (NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id "
        sqlQuery += "INNER JOIN jetnet_ra.dbo.Company WITH (NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id "
        '  sqlQuery += "LEFT OUTER JOIN jetnet_ra.dbo.Contact WITH (NOLOCK) ON contact_id = cref_contact_id AND contact_journ_id = cref_journ_id " ' added MSW 7/20/15



        sqlQuery += "WHERE (amod_customer_flag = 'Y') "
        'sqlQuery += "AND (amod_product_business_flag = 'Y') "
        sqlQuery += "AND (ac_journ_id = 0) "
        'sqlQuery += "AND (ac_product_business_flag = 'Y') "
        sqlQuery += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)
        sqlQuery += "AND (ac_lifecycle_stage = 3) "
        sqlQuery += "AND (cref_transmit_seq_no = 1) "

        Select Case ForSaleStatus
            Case "FOR SALE"
                sqlQuery += " and ac_forsale_flag='Y' "
            Case "NOT FOR SALE"
                sqlQuery += " and ac_forsale_flag='N' "
        End Select


        '--------------------------------------------
        '-- No Awaiting Documentation

        sqlQuery += "AND (comp_name <> 'Awaiting Documentation') "
        sqlQuery += "AND (comp_awaitdoc_flag = 'N') "

        '--------------------------------------------
        '-- No Government 

        sqlQuery += "AND (comp_agency_type <> 'G') "

        '--------------------------------------------
        '-- Company Can NOT Have Business Type

        sqlQuery += "AND ( EXISTS (SELECT NULL FROM jetnet_ra.dbo.Business_Type_Reference WITH (NOLOCK) "
        sqlQuery += " WHERE (bustypref_comp_id = comp_id) "
        sqlQuery += " AND (bustypref_journ_id = comp_journ_id) "
        sqlQuery += " AND (bustypref_type IN ('EU','AU','A3','AY','AA','AX','AT','FF','FL','SR','TS')) "
        sqlQuery += " ) "
        sqlQuery += " ) "

        '---------------------------------------
        '-- Find Upgrade From Model Aircraft
        '-- Or Base it on the model we pass

        If CommonUpgradeModels = 0 Then
            sqlQuery += "AND (amod_id IN (SELECT TOP 5 upg_modelid "
            sqlQuery += "FROM star_reports.dbo.upgrade_data (NOLOCK) "
            sqlQuery += "WHERE (upg_upgradedtomodelid = " & ModelID & ") "
            sqlQuery += "AND (upg_database = (SELECT TOP 1 MAX(upg_database) "
            sqlQuery += "FROM star_reports.dbo.upgrade_data WITH (NOLOCK) "
            sqlQuery += ") "
            sqlQuery += " )"
            sqlQuery += "AND (upg_database <> 'jetnet_ra_') "
            sqlQuery += "ORDER BY CAST(upg_totalupgradestomodel AS INT) DESC, upg_make, upg_model  "
            sqlQuery += ") "
            sqlQuery += ") "
        Else
            sqlQuery += " and (amod_id = " & CommonUpgradeModels & ")"
        End If

        '---------------------------------------
        '-- Find Length Of Ownership

        sqlQuery += "AND ( "


        sqlQuery += " ( "

        '----------------------
        '-- New Aircraft
        If PreviouslyOwned = "PREVIOUS" Or PreviouslyOwned = "ALL" Then
            sqlQuery += "(DATEDIFF(DAY,ac_purchase_date,GETDATE()) >= "

            Select Case LifecyclePercentage
                Case 1.5 '150%
                    sqlQuery += " 1.5 * "
                Case 1.25 '125%
                    sqlQuery += " 1.25 * "
                Case 0.9 '90%
                    sqlQuery += " .9 * "
                Case 0.85
                    sqlQuery += " .85 * "
                Case 0.8
                    sqlQuery += " .8 * "
                Case 0.75
                    sqlQuery += " .75 * "
            End Select

            sqlQuery += "(SELECT TOP 1 ac4_used_days_owned "
            sqlQuery += "FROM star_reports.dbo.Aircraft_4 WITH (NOLOCK) "
            sqlQuery += "WHERE "
            sqlQuery += " (ac4_new_days_owned > 0) "

            sqlQuery += "AND (ac_previously_owned_flag = 'Y') "
            sqlQuery += "AND (ac_amod_id = ac4_amod_id) "
            sqlQuery += "ORDER BY ac4_start_date DESC "
            sqlQuery += ")"
            sqlQuery += ") "
        End If

        If PreviouslyOwned = "ALL" Then
            sqlQuery += " OR "
        End If

        If PreviouslyOwned = "NEW" Or PreviouslyOwned = "ALL" Then
            sqlQuery += "(DATEDIFF(DAY,ac_purchase_date,GETDATE()) >= "

            Select Case LifecyclePercentage
                Case 1.5 '150%
                    sqlQuery += " 1.5 * "
                Case 1.25 '125%
                    sqlQuery += " 1.25 * "
                Case 0.9 '90%
                    sqlQuery += " .9 * "
            End Select

            sqlQuery += "(SELECT TOP 1 ac4_new_days_owned "
            sqlQuery += "FROM star_reports.dbo.Aircraft_4 WITH (NOLOCK) "
            sqlQuery += "WHERE "
            sqlQuery += " (ac4_new_days_owned > 0) "

            sqlQuery += "AND (ac_previously_owned_flag = 'N') "
            sqlQuery += "AND (ac_amod_id = ac4_amod_id) "
            sqlQuery += "ORDER BY ac4_start_date DESC "
            sqlQuery += ")"
            sqlQuery += ") "
        End If

        sqlQuery += ")" 'end or

        sqlQuery += ")"
        sqlQuery += "ORDER BY "
        sqlQuery += CompanySort
        'sqlQuery += " comp_name, comp_country, comp_state, comp_city "

        OwnersDatatable = LocalDataLayer.Get_CRM_VIEW_Function(sqlQuery, "Time to Buy Tab")

        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = OwnersDatatable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            OwnersDatatable = distinct_table
        End If

        Return OwnersDatatable
    End Function
    ''' <summary>
    ''' Display Table for the time to buy tab. Special notes are that it's running a pretty big query and it uses a lot of
    ''' special field names. For instance - comp_id is CompID - it's best to look at the query up above to see what's returned.
    ''' </summary>
    ''' <param name="AllOwnersDatatable"></param>
    ''' <param name="OwnersToDisplay"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="aircraftID"></param>
    ''' <param name="count_label"></param>
    ''' <param name="TabContainer1"></param>
    ''' <param name="crm_view_view_all"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Owner_Table_Time_To_Buy(ByRef AllOwnersDatatable As DataTable, ByRef OwnersToDisplay As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef aircraftID As Long, ByRef count_label As Label, ByRef TabContainer1 As AjaxControlToolkit.TabContainer, ByRef crm_view_view_all As LinkButton, ByVal activeTabID As String) As String

        Dim CompanyDisplay As String = "" 'Holds Company Display Information.
        Dim ProspectDisplay As String = "" 'Holds Prospect Display Information.
        Dim NoteDisplay As String = "" 'Holds Note Display Information.
        Dim ActionDisplay As String = "" 'Holds Action Display Information.
        Dim ForSale As String = "" 'This holds the forsale class information if it's needed (green background)
        Dim AvgNewDays As Double = 0
        Dim AvgUsedDays As Double = 0

        Dim CheckDataTable As New DataTable
        Dim CheckNote As Boolean = False
        Dim CheckProspect As Boolean = False
        Dim CheckAction As Boolean = False
        Dim HtmlOut As New StringBuilder
        count_label.Text = AllOwnersDatatable.Rows.Count & " Time To Buy Prospects Found."

        If AllOwnersDatatable.Rows.Count > 0 Then
            Dim cssClass As String = ""
            'Set up the start of the table.
            HtmlOut.Append("<div  class=""prospectDataTableContainer""><div id=""" & activeTabID & "InnerTable"" style=""width: 100%;""></div>")
            HtmlOut.Append("<table width='100%' id=""" & activeTabID & "Copy"" cellpadding='3' cellspacing='0' class=""prospectDataTable"">")
            HtmlOut.Append("<thead><tr class='header'>")
            HtmlOut.Append("<!--<th align='left' valign='top'>SEL</th>-->")
            HtmlOut.Append("<th align='left' valign='top' width='330'>Company</th>")
            HtmlOut.Append("<th align='left' valign='top' width='150'>Make/Model</th>")
            HtmlOut.Append("<th align='left' valign='top' width='80'>Ser No</th>")
            HtmlOut.Append("<th align='left' valign='top' width='80'>Reg No</th>")
            HtmlOut.Append("<th align='right' valign='top'>Purchased</th>")

            HtmlOut.Append("<th align='right' valign='top' width='110'><span class='help_cursor' title='Length of Ownership'>Length of Ownership</span><br /><span class='smaller_text'>Avg Years</span></th>")
            HtmlOut.Append("<th align='right' valign='top' width='110'><span class='help_cursor' title='Years Owned'>Years Owned</span></th>")
            HtmlOut.Append("<th align='center' valign='top' width='80'>Previously Owned</th>")
            'Crm Toggle On
            '----------------------------------------------------------------------------------------------------------
            If clsGeneral.clsGeneral.isCrmDisplayMode Then
                HtmlOut.Append("<th align='right' valign='top'><!--Notes--></th>")
                HtmlOut.Append("<th align='right' valign='top'><!--Actions--></th>")
                HtmlOut.Append("<th align='right' valign='top'><!--Prospects--></th>")
            End If
            '-------------------------------------------------------------------------------------------------------------

            HtmlOut.Append(DisplayCompanyFields(True, AllOwnersDatatable.Rows(0)))
            HtmlOut.Append("</tr></thead><tbody>")

            For Each r As DataRow In AllOwnersDatatable.Rows
                CompanyDisplay = "" 'Holds Company Display Information.
                ProspectDisplay = "" 'Holds Prospect Display Information.
                NoteDisplay = "" 'Holds Note Display Information.
                ActionDisplay = "" 'Holds Action Display Information.
                ForSale = "" 'This holds the forsale class information if it's needed (green background)
                AvgNewDays = 0
                AvgUsedDays = 0
                CheckNote = False
                CheckProspect = False
                CheckAction = False

                CheckDataTable.Clear()

                If cssClass = "" Then
                    HtmlOut.Append("<tr>")
                    cssClass = "alt_row"
                Else
                    HtmlOut.Append("<tr class='alt_row'>")
                    cssClass = ""
                End If

                If Not IsDBNull(r("looavgnew")) Then
                    AvgNewDays = r("looavgnew")
                End If

                If Not IsDBNull(r("looavgused")) Then
                    AvgUsedDays = r("looavgused")
                End If

                If Not IsDBNull(r("ForSale")) Then
                    If r("ForSale").ToString = "Y" Then
                        ForSale = r("ForSale").ToString
                    End If
                End If

                HtmlOut.Append("<!--<td align='left' valign='top'></td>-->")
                'Display the Company Field.
                HtmlOut.Append("<td align='left' valign='top'>" & DisplayFunctions.WriteDetailsLink(0, r("CompId"), 0, 0, True, r("Company").ToString, "", "") & "</td>") 'Display Company Name Link.

                'Display the Make/Model Field.
                HtmlOut.Append("<td align='left' valign='top'>" & r("Make").ToString & " " & r("Model").ToString & "</td>")
                'Display the Ser# Field.
                HtmlOut.Append("<td align='left' valign='top' " & IIf(ForSale = "Y", "class='light_green_background_no_block'", "") & ">" & DisplayFunctions.WriteDetailsLink(r("ACId"), 0, 0, 0, True, r("SerNbr").ToString, "", "") & "</td>")
                'Display the Reg# Field.
                HtmlOut.Append("<td align='left' valign='top' " & IIf(ForSale = "Y", "class='light_green_background_no_block'", "") & ">" & IIf(Not IsDBNull(r("RegNbr")), r("RegNbr").ToString, "") & "</td>")

                'Display the Purchase Date Field.
                HtmlOut.Append("<td align='right' valign='top'>" & FormatDateTime(r("PurchaseDate"), DateFormat.ShortDate) & "</td>")

                'Display Average Length of Ownership in years
                HtmlOut.Append("<td align='right' valign='top'>" & IIf(r("PreviouslyOwned").ToString = "Y", FormatNumber(AvgUsedDays / 365, 1), FormatNumber(AvgNewDays / 365, 1)) & "</td>")

                'Display Number of Days Owned in years Field.
                HtmlOut.Append("<td align='right' valign='top'>" & FormatNumber(r("NbrDaysOwned") / 365, 1) & "</td>")

                'Display Previously Owned Field.
                HtmlOut.Append("<td align='center' valign='top'>" & IIf(r("PreviouslyOwned").ToString = "Y", "&#10003;", "") & "</td>")

                'Crm Toggle On
                '----------------------------------------------------------------------------------------------------------
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    'Let's go ahead and run a check on all three:
                    Run_Check_On_Applicable_Notes(CheckDataTable, aircraftID, r("CompId"), CheckNote, CheckProspect, CheckAction, NoteDisplay, ProspectDisplay, ActionDisplay)

                    'Display Notes Field.
                    HtmlOut.Append("<td align='right' valign='top'>")

                    If NoteDisplay <> "" Then
                        HtmlOut.Append(NoteDisplay)
                    Else
                        HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(aircraftID, r("CompId"), "A", TabContainer1.ActiveTabIndex))
                    End If

                    HtmlOut.Append("</td>")

                    'Display Action Field.
                    HtmlOut.Append("<td align='right' valign='top'>")
                    If CheckAction Then
                        ActionDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("CompId")), aircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "P")
                    End If

                    If ActionDisplay <> "" Then
                        HtmlOut.Append(ActionDisplay)
                    Else
                        HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(aircraftID, r("CompId"), "P", TabContainer1.ActiveTabIndex))
                    End If
                    HtmlOut.Append("</td>")


                    'Display Prospects Field. 
                    HtmlOut.Append("<td align='right' valign='top'>")
                    If CheckProspect Then
                        ProspectDisplay = crmWebClient.DisplayFunctions.BuildNote_ProspectView(Trim(r("CompId")), aircraftID, aclsData_Temp, "COMP_AC", HttpContext.Current.Application.Item("crmClientDatabase"), "B")
                    End If
                    'Checks to see whether or not there is a prospect. If there is, it displays the icon.
                    If Not String.IsNullOrEmpty(ProspectDisplay) Then
                        HtmlOut.Append(ProspectDisplay)
                    Else 'Otherwise it displays a link that calls the note page to check and see if the company needs to be created. 
                        HtmlOut.Append(DisplayFunctions.WriteCRMNoteLinksForProspectViewFORCLIENTCOMPANYCREATION(aircraftID, r("CompId"), "B", TabContainer1.ActiveTabIndex))
                    End If

                    HtmlOut.Append("</td>")
                End If
                '--------------------------------------------------------------------------------------------------------------------
                HtmlOut.Append(DisplayCompanyFields(False, r))
                HtmlOut.Append("</tr>")
            Next
            HtmlOut.Append("</tbody></table>")

            If AllOwnersDatatable.Rows.Count = 25 Then
                'For now we're going to assume that there's more:
                crm_view_view_all.Visible = True
            End If

            HtmlOut.Append("</div>")
        Else
            HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
        End If

        Return HtmlOut.ToString
    End Function
#End Region
#Region "Prospects Tab Functions - CRM VIEW"
    ''' <summary>
    ''' Run/Build Prospects Query
    ''' </summary>
    ''' <param name="ModelSearchVariable"></param>
    ''' <param name="UpgradeModels"></param>
    ''' <param name="InactiveProspects"></param>
    ''' <param name="localDataLayer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Prospects_Notes_Build_Run_Query(ByRef ModelSearchVariable As Long, ByRef UpgradeModels() As String, ByRef InactiveProspects As Boolean, ByVal localDataLayer As viewsDataLayer, ByRef Export As Boolean, ByRef ExportEmail As Boolean, ByRef ACID As Long, ByRef NoteSearch As Boolean, ByRef OrderBy As String, ByRef DisplayCompanyNotesOnly As Boolean, ByRef MyAircraftSearch As Boolean) As DataTable
        Dim Query As String = ""
        Dim ResultsTable As New DataTable

        'OrderBy = "clicomp_name"
        Query = " SELECT "
        Query = Query & " clicomp_name as comp_name, clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, "
        Query = Query & " clicomp_city as comp_city, clicomp_state as comp_state, clicomp_country as comp_country, "
        Query = Query & " clicomp_email_address as comp_email_address, clicomp_web_address as comp_web_address, clicontact_email_address as contact_email_address, "
        Query = Query & " cliaircraft_year_mfr, cliamod_make_name, cliamod_model_name, cliaircraft_ser_nbr, "


        'Edited to include both Notes Search/Prospects Search
        ' If NoteSearch Then
        Query = Query & " clicontact_first_name as contact_first_name, clicontact_last_name as contact_last_name, clicontact_title as contact_title,"
        'End If

        Query = Query & " (select clipnum_number from client_phone_numbers where clipnum_type='Office' and clipnum_comp_id = clicomp_id and clipnum_contact_id = 0 limit 1) as comp_phone_office, "
        Query = Query & " (select clipnum_number from client_phone_numbers where clipnum_type='Fax' and clipnum_comp_id = clicomp_id and clipnum_contact_id = 0 limit 1) as comp_phone_fax, "

        ' Query = Query & " (select concat(cliaircraft_year_mfr, ' ', cliamod_make_name, '/', cliamod_model_name, ' ' , cliaircraft_ser_nbr) from client_aircraft inner join client_aircraft_model on cliamod_id = cliaircraft_cliamod_id  where (cliaircraft_jetnet_ac_id = lnote_jetnet_ac_id and lnote_jetnet_ac_id > 0)  limit 1) as ac_details, "
        ' Query = Query & " (select concat(cliamod_make_name, '/', cliamod_model_name)  from client_aircraft_model where cliamod_id = lnote_client_amod_id  limit 1) as model_details, "


        If Export = False And ExportEmail = False Then
            Query = Query & " lnote_jetnet_amod_id, lnote_jetnet_ac_id, lnote_id, lnote_status, lnote_opportunity_status,"
        End If

        Query = Query & " lnote_note, lnote_entry_date, lnote_user_name,clicomp_id as comp_id, lnote_jetnet_comp_id, lnote_jetnet_contact_id, lnote_client_contact_id "


        Query = Query & " FROM local_notes "

        'If DisplayCompanyNotesOnly Then
        '  Query = Query & " inner join client_company on lnote_client_comp_id = clicomp_id "
        'Else
        Query = Query & " left outer join client_company on lnote_client_comp_id = clicomp_id "
        'End If

        'Adding these to the notes only search.
        'If NoteSearch Then
        Query = Query & " left outer join client_contact on clicontact_comp_id = clicomp_id and lnote_client_contact_id = clicontact_id "
        Query = Query & " left outer join client_aircraft_model on cliamod_id = lnote_client_amod_id "
        Query = Query & " left outer join client_aircraft on cliaircraft_jetnet_ac_id = lnote_jetnet_ac_id  and lnote_jetnet_ac_id > 0 "
        'End If

        Query = Query & " WHERE "
        If NoteSearch = False Then
            Query = Query & " lnote_status='B' "
        Else
            Query = Query & " lnote_status = 'A' "
        End If

        If DisplayCompanyNotesOnly Then
            Query = Query & " and ((lnote_client_comp_id > 0 or lnote_jetnet_comp_id > 0)) "
        End If

        If MyAircraftSearch = True And ModelSearchVariable > 0 Then
            Query = Query & " and (( lnote_jetnet_amod_id = " & ModelSearchVariable & "  and lnote_jetnet_ac_id = 0)" 'Single model search
            Query = Query & " or lnote_jetnet_ac_id = " & ACID & " ) " 'Single ac search 
        ElseIf MyAircraftSearch = False And ModelSearchVariable > 0 Then
            Query = Query & " and lnote_jetnet_amod_id = " & ModelSearchVariable & " " 'Single model search
        Else
            If ACID = 0 Then
                If ModelSearchVariable <> 0 Then
                    Query = Query & " and lnote_jetnet_amod_id = " & ModelSearchVariable & " " 'Single model search
                Else
                    Query = Query & " and lnote_jetnet_amod_id in (" & UpgradeModels(0) & ") " 'Common upgrade model search
                End If
            Else
                Query = Query & " and lnote_jetnet_ac_id = " & ACID & " " 'Single ac search
            End If
        End If




        If NoteSearch = False Then
            If Not InactiveProspects Then
                Query = Query & " and lnote_opportunity_status = 'A' "
            End If
            Query = Query & " order by " & OrderBy 'lnote_opportunity_status asc, clicomp_name "

        Else
            Query = Query & " order by " & OrderBy 'lnote_entry_date desc, clicomp_name "
        End If

        ResultsTable = localDataLayer.Get_CRM_VIEW_Prospects(Query)

        Dim EditedTable As New DataTable
        EditedTable = ResultsTable.Clone

        'This needs to be added because we have to actually loop through each table row to re-add if we need a jetnet company.
        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                For Each r As DataRow In ResultsTable.Rows
                    Dim newCustomersRow As DataRow = EditedTable.NewRow()

                    If Not IsDBNull(r("comp_id")) Then
                        newCustomersRow("comp_id") = r("comp_id")
                        newCustomersRow("comp_name") = r("comp_name")
                        newCustomersRow("comp_address1") = r("comp_address1")
                        newCustomersRow("comp_address2") = r("comp_address2")
                        newCustomersRow("comp_city") = r("comp_city")
                        newCustomersRow("comp_state") = r("comp_state")
                        newCustomersRow("comp_country") = r("comp_country")
                        newCustomersRow("comp_email_address") = r("comp_email_address")
                        newCustomersRow("comp_web_address") = r("comp_web_address")
                        newCustomersRow("contact_email_address") = r("contact_email_address")

                        newCustomersRow("contact_first_name") = r("contact_first_name")
                        newCustomersRow("contact_last_name") = r("contact_last_name")
                        newCustomersRow("contact_title") = r("contact_title")
                        newCustomersRow("comp_phone_office") = r("comp_phone_office")
                        newCustomersRow("comp_phone_fax") = r("comp_phone_fax")
                    ElseIf r.Item("lnote_jetnet_comp_id") > 0 Then
                        Dim TemporaryCompanyInformation As New DataTable
                        Dim TemporaryContactInformation As New DataTable

                        TemporaryCompanyInformation = localDataLayer.get_comp_id_by_name(r.Item("lnote_jetnet_comp_id"))

                        If Not IsNothing(TemporaryCompanyInformation) Then
                            If TemporaryCompanyInformation.Rows.Count > 0 Then
                                For Each q As DataRow In TemporaryCompanyInformation.Rows
                                    newCustomersRow("comp_name") = q("comp_name")
                                    newCustomersRow("comp_address1") = q("comp_address1")
                                    newCustomersRow("comp_address2") = q("comp_address2")
                                    newCustomersRow("comp_city") = q("comp_city")
                                    newCustomersRow("comp_state") = q("comp_state")
                                    newCustomersRow("comp_country") = q("comp_country")
                                    newCustomersRow("comp_email_address") = q("comp_email_address")
                                    newCustomersRow("comp_web_address") = q("comp_web_address")
                                    newCustomersRow("comp_phone_office") = q("comp_phone_office")
                                    newCustomersRow("comp_phone_fax") = q("comp_phone_fax")
                                    newCustomersRow("comp_id") = r.Item("lnote_jetnet_comp_id")
                                    If r.Item("lnote_jetnet_contact_id") > 0 Then
                                        TemporaryContactInformation = commonEvo.get_contact_info_fromID_returnDatatable(r.Item("lnote_jetnet_comp_id"), r.Item("lnote_jetnet_contact_id"), 0, False)
                                        If Not IsNothing(TemporaryContactInformation) Then
                                            If TemporaryContactInformation.Rows.Count > 0 Then
                                                For Each m As DataRow In TemporaryContactInformation.Rows
                                                    newCustomersRow("contact_email_address") = m("contact_email_address")

                                                    newCustomersRow("contact_first_name") = m("contact_first_name")
                                                    newCustomersRow("contact_last_name") = m("contact_last_name")
                                                    newCustomersRow("contact_title") = m("contact_title")
                                                Next
                                            End If
                                        End If

                                    End If
                                Next

                                TemporaryCompanyInformation.Dispose()
                                TemporaryContactInformation.Dispose()
                            End If
                        End If
                    End If

                    newCustomersRow("cliaircraft_year_mfr") = r("cliaircraft_year_mfr")
                    newCustomersRow("cliamod_make_name") = r("cliamod_make_name")
                    newCustomersRow("cliaircraft_year_mfr") = r("cliaircraft_year_mfr")
                    newCustomersRow("cliamod_model_name") = r("cliamod_model_name")
                    newCustomersRow("cliaircraft_ser_nbr") = r("cliaircraft_ser_nbr")

                    If Export = False And ExportEmail = False Then
                        newCustomersRow("lnote_jetnet_amod_id") = r("lnote_jetnet_amod_id")
                        newCustomersRow("lnote_jetnet_ac_id") = r("lnote_jetnet_ac_id")
                        newCustomersRow("lnote_jetnet_comp_id") = r("lnote_jetnet_comp_id")
                        newCustomersRow("lnote_id") = r("lnote_id")
                        newCustomersRow("lnote_status") = r("lnote_status")
                        newCustomersRow("lnote_opportunity_status") = r("lnote_opportunity_status")
                    End If

                    newCustomersRow("lnote_note") = r("lnote_note")
                    newCustomersRow("lnote_entry_date") = r("lnote_entry_date")
                    newCustomersRow("lnote_user_name") = r("lnote_user_name")

                    EditedTable.Rows.Add(newCustomersRow)
                    EditedTable.AcceptChanges()

                Next
            End If
        End If


        If ExportEmail Then
            Dim distinct_table_view As New DataView
            Dim distinct_table As New DataTable
            ''create the view to get the distinct values.
            distinct_table_view = EditedTable.DefaultView
            distinct_table_view.Sort = "comp_email_address"

            distinct_table_view.RowFilter = "comp_email_address <> ''"

            ''actually get the distinct values.
            distinct_table = distinct_table_view.ToTable(True, "comp_email_address")
            EditedTable = distinct_table
        End If



        Return EditedTable
    End Function
    ''' <summary>
    ''' Display Prospects Tab
    ''' </summary>
    ''' <param name="TemporaryTable"></param>
    ''' <param name="count_label"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="CompanyLocation"></param>
    ''' <param name="CompanyTitle"></param>
    ''' <param name="localDatalayer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CRM_VIEW_Display_Prospects_Notes_Table(ByRef TemporaryTable As DataTable, ByRef count_label As Label, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef AircraftID As Long, ByRef CompanyLocation As String, ByRef CompanyTitle As String, ByRef localDatalayer As viewsDataLayer, ByRef isNoteView As Boolean, ByVal selected_dropdown_value As String, ByVal ActiveTabID As String) As String
        Dim HtmlOut As New StringBuilder
        Dim ToggleRowColor As Boolean = False

        If Not IsNothing(TemporaryTable) Then
            count_label.Text = TemporaryTable.Rows.Count & IIf(isNoteView, " Notes", " Prospects") & " Found."

            If TemporaryTable.Rows.Count > 0 Then

                HtmlOut.Append("<div class=""prospectDataTableContainer""><div id=""" & ActiveTabID & "InnerTable"" style=""width: 100%;""></div><table  id=""" & ActiveTabID & "Copy"" width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""prospectDataTable"">")


                HtmlOut.Append("<thead><tr class='header'>")


                If isNoteView Then
                    HtmlOut.Append("<!--<th valign='top' align='left'>SEL</th>-->")
                    HtmlOut.Append("<th valign='top' align='left'>Date</th>")
                    HtmlOut.Append("<th valign='top' align='left'>Entered By</th>")
                    HtmlOut.Append("<th valign='top' align='left'>Company</th>")

                    HtmlOut.Append("<th valign='top' align='left'>Note</th>")
                    'htmlout.Append( "<td valign='top' align='left'>&nbsp;</td><td valign='top' align='left'>&nbsp;</td><td>&nbsp;</td>"
                    HtmlOut.Append("<th valign='top' align='left'>&nbsp;</th>")
                    HtmlOut.Append(DisplayCompanyFields(True, TemporaryTable.Rows(0)))
                Else
                    HtmlOut.Append("<!--<th valign='top' align='left'>SEL</th>-->")
                    HtmlOut.Append("<th valign='top' align='left'>Prospect</th>")

                    HtmlOut.Append("<th valign='top' align='left'>Prospect Notes</th>")
                    HtmlOut.Append("<th valign='top' align='left'>&nbsp;</th><th valign='top' align='left'>&nbsp;</th>")
                    HtmlOut.Append("<th valign='top' align='left'>&nbsp;</th>")
                    HtmlOut.Append(DisplayCompanyFields(True, TemporaryTable.Rows(0)))
                End If

                HtmlOut.Append("</tr></thead><tbody>")


                For Each r As DataRow In TemporaryTable.Rows
                    Dim TemporaryCompanyInformation As New DataTable

                    If isNoteView Then
                        If Not ToggleRowColor Then
                            HtmlOut.Append("<tr class='alt_row'>")
                            ToggleRowColor = True
                        Else
                            HtmlOut.Append("<tr bgcolor='white'>")
                            ToggleRowColor = False
                        End If

                        HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")

                        'Display Entry Date
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        HtmlOut.Append(r("lnote_entry_date"))
                        HtmlOut.Append("</td>")
                        'Display User Name
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        HtmlOut.Append(r("lnote_user_name"))
                        HtmlOut.Append("</td>")

                    Else
                        If Not IsDBNull(r.Item("lnote_opportunity_status")) Then
                            If Trim(r.Item("lnote_opportunity_status")) = "A" Then
                                If Not ToggleRowColor Then
                                    HtmlOut.Append("<tr class='alt_row'>")
                                    ToggleRowColor = True
                                Else
                                    HtmlOut.Append("<tr bgcolor='white'>")
                                    ToggleRowColor = False
                                End If
                            Else
                                HtmlOut.Append("<tr bgcolor='#989898'>")
                                ToggleRowColor = False
                            End If
                        Else
                            If Not ToggleRowColor Then
                                HtmlOut.Append("<tr class='alt_row'>")
                                ToggleRowColor = True
                            Else
                                HtmlOut.Append("<tr bgcolor='white'>")
                                ToggleRowColor = False
                            End If
                        End If

                        HtmlOut.Append("<!--<td align=""left"" valign=""top""></td>-->")
                    End If

                    'Display company field
                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    'If Not IsDBNull(r.Item("comp_id")) Then
                    If Not IsDBNull(r.Item("comp_id")) Then
                        CRM_VIEW_Build_Company_Title_Location(CompanyTitle, CompanyLocation, r)

                        If Not IsDBNull(r("comp_name")) Then
                            If r("lnote_jetnet_comp_id") > 0 Then
                                HtmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r("lnote_jetnet_comp_id"), 0, 0, True, Trim(r("comp_name")), "", ""))
                            Else
                                'This only displays if you have just a client company, not based off of a jetnet record. Only then does it link you to your client record.
                                HtmlOut.Append("<a href='#' onclick=""javascript:load('details.aspx?source=CLIENT&type=1&comp_ID=" & r("comp_id") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">" & Trim(r("comp_name")) & "</a>")
                            End If
                        End If
                        'ElseIf (r.Item("lnote_jetnet_comp_id")) > 0 Then
                        '  TemporaryCompanyInformation = localDatalayer.get_comp_id_by_name(r.Item("lnote_jetnet_comp_id"))

                        '  If Not IsNothing(TemporaryCompanyInformation) Then
                        '    If TemporaryCompanyInformation.Rows.Count > 0 Then
                        '      For Each q As DataRow In TemporaryCompanyInformation.Rows
                        '        If Not IsDBNull(q("comp_name")) Then
                        '          htmlout.Append( "<span><span class='label'><span class='magnify_bullet' title='" & CompanyTitle & "'>" & DisplayFunctions.WriteDetailsLink(0, r("lnote_jetnet_comp_id"), 0, 0, True, Trim(q("comp_name")), "", "") & " <span class='tiny'>" & CompanyLocation & "</span></span></span>"
                        '        End If
                        '      Next
                        '    End If
                        '  End If

                    End If
                    'End If
                    HtmlOut.Append("</td>")


                    HtmlOut.Append("<td align=""left"" valign=""top"">")
                    If Not IsDBNull(r.Item("lnote_note")) Then
                        HtmlOut.Append(r.Item("lnote_note"))
                    End If



                    If Trim(selected_dropdown_value) <> "AC" And Trim(selected_dropdown_value) <> "" Then
                        If Not IsDBNull(r.Item("cliaircraft_ser_nbr")) Then
                            If Not IsDBNull(r.Item("cliaircraft_ser_nbr")) Then
                                HtmlOut.Append(" - <a href=""details.aspx?ac_ID=" & r.Item("lnote_jetnet_ac_id") & "&source=JETNET&type=3"" target='_blank'><b>")
                                HtmlOut.Append(r.Item("cliaircraft_year_mfr") & " " & r.Item("cliamod_make_name") & "/" & r.Item("cliamod_model_name") & " " & r.Item("cliaircraft_ser_nbr"))
                                HtmlOut.Append("</b></a>")
                            Else
                                HtmlOut.Append(" - <b>")
                                HtmlOut.Append(r.Item("cliamod_make_name") & "/" & r.Item("cliamod_model_name"))
                                HtmlOut.Append("</b>")
                            End If
                        End If
                    End If



                    'cliamod_make_name, cliamod_model_name, cliaircraft_year_mfr, cliaircraft_ser_nbr

                    HtmlOut.Append("</td>")

                    If isNoteView Then
                        'Display edit field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")

                        HtmlOut.Append("<a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;from=view&amp;rememberTab=7&amp;type=note&amp;refreshing=prospect&amp;ac_ID=" & AircraftID & "&amp;id=" & r.Item("lnote_id") & "','','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/edit_icon.png'></a>") 'DisplayFunctions.WriteNotesRemindersLinks_Action(r.Item("lnote_id"), AircraftID, r("lnote_jetnet_comp_id"), 0, True, "&n=1", "<img src='images/edit_icon.png'>")
                        HtmlOut.Append("&nbsp;</td>")
                    Else

                        'Display note field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        HtmlOut.Append(crmWebClient.DisplayFunctions.BuildNote(Trim(r("lnote_jetnet_comp_id")), aclsData_Temp, "COMP"))
                        HtmlOut.Append("&nbsp;</td>")
                        'Display action field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        HtmlOut.Append(crmWebClient.DisplayFunctions.BuildNote_Action(Trim(r("lnote_jetnet_comp_id")), aclsData_Temp, "COMP"))
                        HtmlOut.Append("&nbsp;</td>")
                        'Display prospect field.
                        HtmlOut.Append("<td align=""left"" valign=""top"">")
                        HtmlOut.Append("<a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;from=view&amp;rememberTab=6&amp;type=prospect&amp;refreshing=prospect&amp;ac_ID=" & AircraftID & "&amp;id=" & r.Item("lnote_id") & "','','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/edit_icon.png'></a>") 'DisplayFunctions.WriteNotesRemindersLinks_Action(r.Item("lnote_id"), AircraftID, r("lnote_jetnet_comp_id"), 0, True, "&n=1", "<img src='images/edit_icon.png'>")
                        ' htmlout.Append( DisplayFunctions.WriteNotesRemindersLinks_Action(r.Item("lnote_id"), AircraftID, r("lnote_jetnet_comp_id"), 0, True, "&b=1", "<img src='images/edit_icon.png'>")
                        HtmlOut.Append("&nbsp;</td>")
                    End If
                    HtmlOut.Append(DisplayCompanyFields(False, r))
                    HtmlOut.Append("</tr>")
                Next

                HtmlOut.Append("</tbody></table></div>")

            Else 'results = 0
                HtmlOut.Append("<p align='center'>There are no applicable results.</p>")
            End If

        End If

        Return HtmlOut.ToString
    End Function

#End Region
#Region "Check All Notes Status - CRM VIEW"
    ''' <summary>
    ''' This goes and runs the multiple status query to check for an action item, note and prospect, orders it by status (note comes first, etc) and limits by one.
    ''' </summary>
    ''' <param name="show_type"></param>
    ''' <param name="companyID"></param>
    ''' <param name="aircraftID"></param>
    ''' <param name="source"></param>
    ''' <param name="schedule_date"></param>
    ''' <param name="limit"></param>
    ''' <param name="new_connection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function Check_For_Applicable_Notes_LIMIT_CRM(ByRef show_type As String, ByVal companyID As Long, ByVal aircraftID As Long, ByVal source As String, ByVal schedule_date As String, ByVal limit As Integer, ByVal new_connection As String) As DataTable
        Dim aTempTable As New DataTable
        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            Dim qry As String = ""


            qry = "SELECT  local_notes.lnote_id, local_notes.lnote_jetnet_ac_id, local_notes.lnote_jetnet_comp_id, local_notes.lnote_client_ac_id, local_notes.lnote_client_comp_id,"
            qry = qry & "local_notes.lnote_jetnet_contact_id, local_notes.lnote_client_contact_id, LEFT(local_notes.lnote_note, 21000) AS lnote_note, local_notes.lnote_entry_date,"
            qry = qry & "local_notes.lnote_action_date, local_notes.lnote_user_login, local_notes.lnote_user_name, local_notes.lnote_notecat_key, local_notes.lnote_status, "
            qry = qry & "local_notes.lnote_schedule_start_date, local_notes.lnote_schedule_end_date, local_notes.lnote_user_id, local_notes.lnote_clipri_ID, "
            qry = qry & "local_notes.lnote_document_flag, local_notes.lnote_jetnet_amod_id, local_notes.lnote_client_amod_id, local_notes.lnote_document_name "
            qry = qry & " FROM  local_notes WHERE (local_notes.lnote_status in ('A','P','B')) "

            If HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly Then
                qry = qry & " and local_notes.lnote_user_id = '" & HttpContext.Current.Session.Item("localUser").crmLocalUserID.ToString & "' "
            End If

            If show_type = "AC" Then
                If source = "CLIENT" Then
                    qry = qry & "AND (local_notes.lnote_client_ac_id = '" & aircraftID & "') "
                Else
                    qry = qry & "AND (local_notes.lnote_jetnet_ac_id = '" & aircraftID & "') "
                End If
            ElseIf show_type = "COMP" Then
                If source = "CLIENT" Then
                    qry = qry & " and (local_notes.lnote_client_comp_id = '" & companyID & "')"
                Else
                    qry = qry & "and (local_notes.lnote_jetnet_comp_id = '" & companyID & "') "
                End If
            ElseIf show_type = "COMP_AC" Then
                If source = "CLIENT" Then
                    qry = qry & " and (local_notes.lnote_client_comp_id = '" & companyID & "')"
                    qry = qry & "AND (local_notes.lnote_client_ac_id = '" & aircraftID & "') "
                Else
                    qry = qry & "and (local_notes.lnote_jetnet_comp_id = '" & companyID & "') "
                    qry = qry & "and (local_notes.lnote_jetnet_ac_id = '" & aircraftID & "') "
                End If

            End If

            qry = qry & "ORDER BY lnote_status, lnote_entry_date desc "

            If limit <> 5000 Then
                qry = qry & " limit " & limit
            End If
            qry = qry

            MySqlConn.ConnectionString = new_connection
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            Dim sQuery As String = qry
            MySqlCommand.CommandText = sQuery

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                aTempTable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
            End Try


            Check_For_Applicable_Notes_LIMIT_CRM = aTempTable

        Catch ex As Exception
            Check_For_Applicable_Notes_LIMIT_CRM = Nothing
            'class_error = "Error in Check_For_Applicable_Notes_LIMIT_CRM((ByVal acid " & aircraftID & " As long, compid " & companyID & " As long, ByVal ac_source " & source & " As String, ByVal schedule_date " & schedule_date & " As String) As DataTable: " & ex.Message
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
    ''' <summary>
    ''' This is a small check that runs in every prospector tab table build. It checks for the last note of the three important types (note/action/prospect) - 
    ''' then it sets some view booleans based on what we're going to show.
    ''' </summary>
    ''' <param name="CheckDataTable"></param>
    ''' <param name="AircraftID"></param>
    ''' <param name="CompanyID"></param>
    ''' <param name="CheckNote"></param>
    ''' <param name="CheckProspect"></param>
    ''' <param name="CheckAction"></param>
    ''' <param name="NoteDisplay"></param>
    ''' <param name="ProspectDisplay"></param>
    ''' <param name="ActionDisplay"></param>
    ''' <remarks></remarks>
    Public Shared Sub Run_Check_On_Applicable_Notes(ByRef CheckDataTable As DataTable, ByRef AircraftID As Long, ByRef CompanyID As Long, ByRef CheckNote As Boolean, ByRef CheckProspect As Boolean, ByRef CheckAction As Boolean, ByRef NoteDisplay As String, ByRef ProspectDisplay As String, ByRef ActionDisplay As String)
        Dim First_Note_Status As String = ""
        Dim First_Note_Text As String = ""

        CheckDataTable = Check_For_Applicable_Notes_LIMIT_CRM("COMP_AC", CompanyID, AircraftID, "JETNET", "", 1, HttpContext.Current.Application.Item("crmClientDatabase"))

        '1.	If we returned nothing, then all the plus signs would be displayed and we would be done. In this case, First_Note_Status would be blank.
        '2.	If we returned an ‘A’ note then we can display the note icon and do our other 2 checks as normal. In this case, First_Note_Status would be A. 
        '3.	If we return a ‘B’, then we already have the prospect and we would put a +sign for the note and only have to do the action item check. In This Case, First Note Status would be B
        '4.) If we return P, then it's an action item And we don't have to search for anything
        If Not IsNothing(CheckDataTable) Then
            If CheckDataTable.Rows.Count > 0 Then
                First_Note_Status = IIf(Not IsDBNull(CheckDataTable.Rows(0).Item("lnote_status")), CheckDataTable.Rows(0).Item("lnote_status"), "")
                First_Note_Text = IIf(Not IsDBNull(CheckDataTable.Rows(0).Item("lnote_note")), CheckDataTable.Rows(0).Item("lnote_note"), "")

                Select Case First_Note_Status
                    Case "A"
                        CheckNote = False
                        CheckAction = True
                        CheckProspect = True
                        NoteDisplay = "<img src=""images/document.png"" class=""float_left"" height=""20"" alt='" & First_Note_Text & "' title='" & First_Note_Text & "'/>"
                    Case "B"
                        CheckNote = False
                        CheckProspect = False
                        CheckAction = True
                        ProspectDisplay = "<img src=""images/gold_prospect_icon.png"" class=""float_left"" height=""20"" alt='" & First_Note_Text & "' title='" & First_Note_Text & "'/>"
                    Case "P"
                        CheckNote = False
                        CheckProspect = False
                        CheckAction = False
                        ActionDisplay = "<img src=""images/red_pin.png"" class=""float_left"" height=""20"" alt='" & First_Note_Text & "' title='" & First_Note_Text & "'/>"
                End Select
            End If
        End If
    End Sub
#End Region
#End Region

#Region "FULL MODEL VIEW"
#Region "Fleet/Market Summary"
    Public Shared Sub Combined_views_display_fleet_market_summary(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef JetnetDataLayer As viewsDataLayer, ByRef IDSToExclude As String, ByRef CRMViewActive As Boolean, ByVal DisplayLink As Boolean, ByRef avg_asking As Long, ByRef avg_take As Long, ByRef avg_sold As Long, Optional ByRef values_for_sale As Long = 0, Optional ByRef values_dom As Long = 0, Optional ByRef values_in_op As Double = 0, Optional ByRef values_total_inop As Long = 0, Optional ByRef ac_exclusive_sale As Integer = 0, Optional ByRef ac_lease As Integer = 0, Optional ByRef absorp_rate As Double = 0.0, Optional ByRef per As Double = 0, Optional ByRef per2 As Double = 0, Optional ByRef per3 As Double = 0, Optional ByRef forsaleavghigh As Double = 0.0, Optional ByRef forsaleavlow As Double = 0.0, Optional ByRef values_mfr_avg_low As Integer = 0, Optional ByRef values_mfr_avg_high As Integer = 0, Optional ByRef values_mfr_avg As Integer = 0, Optional ByRef daysonmarket_low As Integer = 0, Optional ByRef daysonmarket_high As Integer = 0, Optional ByRef days_avg As Integer = 0, Optional ByRef values_aftt_low As Long = 0, Optional ByRef values_aftt_high As Long = 0, Optional ByRef values_aftt_avg As Long = 0, Optional ByRef values_avg_asking_display As Double = 0, Optional ByVal modelImage As String = "", Optional ByRef landings_high As Double = 0, Optional ByRef landings_low As Double = 0, Optional ByRef landings_avg As Double = 0, Optional ByRef landings_sum As Double = 0, Optional ByRef landings_count As Long = 0)

        Dim fleetHtmlOut As New StringBuilder
        Dim marketHtmlOut As New StringBuilder

        Dim results_table As New DataTable


        'values_forsale_avg_low, values_forsale_avg_high, values_mfr_avg_low, values_mfr_avg_high, values_mfr_avg, values_days_low, values_days_high, 
        'values_days_avg, 
        'values_aftt_low, values_aftt_high, values_aftt_avg
        '
        Dim string_for_op_percentage = ""

        Dim avgyear As Integer = 0
        Dim avgyearcount As Integer = 0

        Dim totalcount As Integer = 0
        Dim totalInOpcount As Integer = 0
        Dim ac_for_sale As Integer = 0
        'Dim ac_exclusive_sale As Integer = 0
        'Dim ac_lease As Integer = 0

        Dim w_owner As Integer = 0
        Dim s_owner As Integer = 0
        Dim f_owner As Integer = 0
        Dim o_stage As Integer = 0
        Dim t_stage As Integer = 0
        Dim th_stage As Integer = 0
        Dim f_stage As Integer = 0

        'Dim daysonmarket As Integer = 0
        'Dim daysonmarket2 As Integer = 0
        'Dim days As Integer = 0

        Dim allhigh As Integer = 0
        Dim alllow As Integer = 0
        Dim values_mfr_count As Integer = 0
        'Dim forsaleavghigh As Double = 0.0
        'Dim forsaleavlow As Double = 0.0
        Dim values_aftt_count As Integer = 0
        Dim all_aftt_high As Integer = 0
        Dim all_aftt_low As Integer = 0
        Dim forsaletakehigh As Double = 0
        Dim forsaletakelow As Double = 0
        Dim us_reg As Integer
        'Dim per As Double = 0
        'Dim per2 As Double = 0
        'Dim per3 As Double = 0

        Dim ClientTable As New DataTable
        Dim JetnetTable As New DataTable

        Dim avg_asking_count As Integer = 0
        Dim avg_take_count As Integer = 0
        Dim avg_sold_count As Integer = 0
        Dim forsalesoldhigh As Integer = 0
        Dim forsalesoldlow As Integer = 0
        Dim total_days As Integer = 0
        Dim days_total_sum As Integer = 0

        'Dim absorp_rate As Double = 0.0
        Dim number_of_months_divide As Integer = 6

        If daysonmarket_low = 0 Then
            daysonmarket_low = 10000
        End If

        Try

            If searchCriteria.ViewCriteriaTimeSpan > 0 Then
                number_of_months_divide = searchCriteria.ViewCriteriaTimeSpan
            End If

            'Get the client market summary data.
            ClientTable = CLIENT_get_fleet_market_summary_info(searchCriteria, False, number_of_months_divide)
            'Get the jetnet market summary data

            JetnetTable = commonEvo.get_fleet_market_summary_info(searchCriteria, False, number_of_months_divide)
            'Combine then and set the IDs to exclude to be used later on the forsale tab


            IDSToExclude = CombineTwoAircraftDatatables(ClientTable, JetnetTable, results_table, "", False)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r("daysonmarket")) Then
                            If CLng(r.Item("daysonmarket").ToString) > 0 Then
                                total_days += 1

                                days_total_sum = days_total_sum + CLng(r.Item("daysonmarket").ToString)

                                If CLng(r.Item("daysonmarket").ToString) > daysonmarket_high Then
                                    daysonmarket_high = CLng(r.Item("daysonmarket").ToString)
                                End If

                                If CLng(r.Item("daysonmarket").ToString) < daysonmarket_low Then
                                    daysonmarket_low = CLng(r.Item("daysonmarket").ToString)
                                End If

                            End If
                        End If


                        If Not IsDBNull(r("SalesPerTimeframe")) Then
                            If IsNumeric(r("SalesPerTimeframe").ToString) Then
                                If r("SalesPerTimeframe") > 0 Then
                                    absorp_rate = r("SalesPerTimeframe")
                                End If
                            End If
                        End If

                        If r("ac_lifecycle_stage") = "3" Then
                            If Not IsDBNull(r("ac_country_of_registration")) Then
                                If Trim(r("ac_country_of_registration")) = "United States" Then
                                    us_reg = us_reg + 1
                                End If
                            End If
                        End If



                        If Not IsDBNull(r("ac_airframe_tot_landings")) Then
                            If IsNumeric(r("ac_airframe_tot_landings").ToString) Then

                                If CInt(r("ac_airframe_tot_landings").ToString) > 0 Then

                                    If landings_high = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) > landings_high Then
                                        landings_high = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    If landings_low = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) < landings_low Then
                                        landings_low = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    landings_sum += CInt(r.Item("ac_airframe_tot_landings").ToString)

                                    landings_count += 1
                                End If
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

                        If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                            If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                    If CInt(r("ac_airframe_tot_Hrs")) > CInt(all_aftt_high) Then
                                        all_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                    End If

                                    If CInt(r("ac_airframe_tot_Hrs")) < CInt(all_aftt_low) Or all_aftt_low = 0 Then
                                        all_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                    End If
                                End If
                            End If
                        End If

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

                                        avg_asking = avg_asking + CDbl(r.Item("ac_asking_price").ToString)
                                        avg_asking_count = avg_asking_count + 1

                                    End If

                                End If
                            End If



                            If Not IsDBNull(r("ac_mfr_year")) Then
                                If IsNumeric(r("ac_mfr_year")) Then
                                    If CInt(r("ac_mfr_year").ToString) > 0 Then
                                        If CInt(r("ac_mfr_year")) > CInt(values_mfr_avg_high) Then
                                            values_mfr_avg_high = CInt(r("ac_mfr_year"))
                                        End If

                                        If CInt(r("ac_mfr_year")) < CInt(values_mfr_avg_low) Or values_mfr_avg_low = 0 Then
                                            values_mfr_avg_low = CInt(r("ac_mfr_year"))
                                        End If

                                        values_mfr_avg = values_mfr_avg + CInt(r("ac_mfr_year"))
                                        values_mfr_count = values_mfr_count + 1
                                    End If
                                End If
                            End If

                            If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                                If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                    If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                        If CInt(r("ac_airframe_tot_Hrs")) > CInt(values_aftt_high) Then
                                            values_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                        End If

                                        If CInt(r("ac_airframe_tot_Hrs")) < CInt(values_aftt_low) Or values_aftt_low = 0 Then
                                            values_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                        End If

                                        values_aftt_avg = values_aftt_avg + CInt(r("ac_airframe_tot_Hrs"))
                                        values_aftt_count = values_aftt_count + 1
                                    End If
                                End If
                            End If


                            'Edited to add the take price, done in the same way as the asking price above:
                            If Not IsDBNull(r("ac_take_price")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_take_price").ToString) Then

                                    If CDbl(r.Item("ac_take_price").ToString) > 0 Then

                                        If forsaletakehigh = 0 Or CDbl(r.Item("ac_take_price").ToString) > forsaletakehigh Then
                                            forsaletakehigh = CDbl(r.Item("ac_take_price").ToString)
                                        End If

                                        If forsaletakelow = 0 Or (CDbl(r.Item("ac_take_price").ToString) < forsaletakelow) Then
                                            forsaletakelow = CDbl(r.Item("ac_take_price").ToString)
                                        End If

                                        avg_take = avg_take + CDbl(r.Item("ac_take_price").ToString)
                                        avg_take_count = avg_take_count + 1

                                    End If

                                End If
                            End If



                            'Edited to add the take price, done in the same way as the asking price above:
                            If Not IsDBNull(r("sold_price")) Then
                                If Not String.IsNullOrEmpty(r.Item("sold_price").ToString) Then

                                    If CDbl(r.Item("sold_price").ToString) > 0 Then

                                        If forsalesoldhigh = 0 Or CDbl(r.Item("sold_price").ToString) > forsalesoldhigh Then
                                            forsalesoldhigh = CDbl(r.Item("sold_price").ToString)
                                        End If

                                        If forsalesoldlow = 0 Or (CDbl(r.Item("sold_price").ToString) < forsalesoldlow) Then
                                            forsalesoldlow = CDbl(r.Item("sold_price").ToString)
                                        End If

                                        avg_sold = avg_sold + CDbl(r.Item("sold_price").ToString)
                                        avg_sold_count = avg_sold_count + 1

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

            If landings_sum > 0 Then
                landings_avg = (landings_sum / landings_count)
            End If


            If (forsaleavlow > 0) Then
                forsaleavlow = CDbl(forsaleavlow / 1000)
            End If

            If (forsaleavghigh > 0) Then
                forsaleavghigh = CDbl(forsaleavghigh / 1000)
            End If

            'Same calculations added for take price
            If (forsaletakelow > 0) Then
                forsaletakelow = CDbl(forsaletakelow / 1000)
            End If

            If (forsaletakehigh > 0) Then
                forsaletakehigh = CDbl(forsaletakehigh / 1000)
            End If

            If (forsalesoldlow > 0) Then
                forsalesoldlow = CDbl(forsalesoldlow / 1000)
            End If

            If (forsalesoldhigh > 0) Then
                forsalesoldhigh = CDbl(forsalesoldhigh / 1000)
            End If


            If (ac_for_sale > 0 And th_stage > 0) Then

                per = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
                per2 = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100), 1)
                per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

                If total_days > 0 Then
                    days_avg = System.Math.Round(CLng(days_total_sum) / CLng(total_days))
                End If


            ElseIf th_stage > 0 And ac_lease > 0 Then
                per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)
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


            If avg_asking_count > 0 Then
                avg_asking = CDbl(avg_asking / avg_asking_count)
                avg_asking = CDbl(avg_asking / 1000)
                values_avg_asking_display = avg_asking
            End If

            If values_aftt_count > 0 Then
                values_aftt_avg = CDbl(values_aftt_avg / values_aftt_count)
            End If

            If values_mfr_count > 0 Then
                values_mfr_avg = CDbl(values_mfr_avg / values_mfr_count)
            End If


            If avg_take_count > 0 Then
                avg_take = CDbl(avg_take / avg_take_count)
                avg_take = CDbl(avg_take / 1000)
            End If

            If avg_sold_count > 0 Then
                avg_sold = CDbl(avg_sold / avg_sold_count)
                avg_sold = CDbl(avg_sold / 1000)
            End If



            string_for_op_percentage = "&nbsp;<span class='tiny'>(" + FormatNumber(per, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span>"

            ' start outer table
            'fleetHtmlOut.Append("<table id='fleetTable' cellpadding='2' cellspacing='0' width='100%'" + IIf(HttpContext.Current.Session.Item("lastView") <> 16, " class='module'", "") + ">")
            'fleetHtmlOut.Append("<tr>")

            '' Ownership table

            'fleetHtmlOut.Append("<td align='right' valign='top' class='FleetMarket_Left_TD' width='50%'><table id='ownershipTable' cellspacing='0' cellpadding='2' width='100%' class='sub_table'>")
            'fleetHtmlOut.Append("<tr class='aircraft_list'><td valign='middle' align='center' colspan='2'><strong>&nbsp;Ownership&nbsp;(In&nbsp;Operation)&nbsp;</strong></td></tr>")

            'If w_owner > 0 Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>" + FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If s_owner > 0 Then
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >Shared:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If f_owner > 0 Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If totalInOpcount > 0 Then
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If
            values_in_op = totalInOpcount

            'If (alllow > 0) And (allhigh > 0) And (allhigh <> CInt(Now().Year)) Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - " + allhigh.ToString + "</td></tr>")
            'ElseIf (alllow > 0) And (allhigh = CInt(Now().Year)) Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - To Present</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range&nbsp;:&nbsp;N/A</td></tr>")
            'End If

            'fleetHtmlOut.Append("</table>")
            'fleetHtmlOut.Append("</td>")

            '' Fleet Info
            'fleetHtmlOut.Append("<td align='left' width='50%' valign='top'>")
            'fleetHtmlOut.Append("<table id='lifeCycleTable' width='100%' cellspacing='0' cellpadding='2' class='sub_table'>")
            'fleetHtmlOut.Append("<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong>Fleet By Life Cycle</strong></td></tr>")

            'If o_stage > 0 Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(o_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'>In Production:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If t_stage > 0 Then
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(t_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >At MFR:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If th_stage > 0 Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(th_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' >In Operation:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If f_stage > 0 Then
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;" + FormatNumber(f_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr><td valign='top' align='left' >Retired:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
            'End If

            'If totalcount > 0 Then
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td class='border_bottom' align='right'>&nbsp;" + FormatNumber(totalcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            'Else
            '  fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'>Total Aircraft:&nbsp;</td><td class='border_bottom' align='right'>&nbsp;0</td></tr>")
            'End If

            'fleetHtmlOut.Append("</table>")
            'fleetHtmlOut.Append("</td></tr></table>")




            marketHtmlOut.Append("<table  width='95%' cellspacing='0' cellpadding='4' valign='top' class='formatTable  datagrid blue'>")
            marketHtmlOut.Append("<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong class=""subHeader"">Market Status</strong></td></tr>")

            If ac_for_sale > 0 Then
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>For Sale&nbsp;</td><td valign='top' align='left' class='rightside'>" + FormatNumber(ac_for_sale, 0, TriState.False, TriState.False, TriState.True).ToString + string_for_op_percentage + "</td></tr>")
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>For Sale:&nbsp;</td><td align='left' class='rightside'>0&nbsp;<span class='tiny'>(0% of For Sale)</span></td></tr>")
            End If
            values_for_sale = ac_for_sale

            If forsaleavlow > 0 Or forsaleavghigh > 0 Then
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Asking Price Range:&nbsp;</td><td valign='top' align='left' nowrap='nowrap' class='rightside'>" + FormatCurrency(forsaleavlow, 0, TriState.False, TriState.True, TriState.True).ToString + "k - " + FormatCurrency(forsaleavghigh, 0, TriState.False, TriState.True, TriState.True).ToString + "k")
                If avg_asking_count > 0 Then
                    marketHtmlOut.Append("&nbsp;&nbsp;&nbsp;(Avg: $" & FormatNumber(avg_asking, 0) & "k)")
                End If
                marketHtmlOut.Append("</td></tr>")
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Asking Price Range:&nbsp;</td><td align='left' class='rightside'>No Asking Prices</td></tr>")
            End If

            If CRMViewActive Then
                If forsaletakelow > 0 Or forsaletakehigh > 0 Then
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Take Price Range:&nbsp;</td><td valign='top' align='left' nowrap='nowrap' class='rightside'>" + FormatCurrency(forsaletakelow, 0, TriState.False, TriState.True, TriState.True).ToString + "k - " + FormatCurrency(forsaletakehigh, 0, TriState.False, TriState.True, TriState.True).ToString + "k")
                    If avg_take_count > 0 Then
                        marketHtmlOut.Append("&nbsp;&nbsp;&nbsp;(Avg: $" & FormatNumber(avg_take, 0) & "k)")
                    End If
                    marketHtmlOut.Append("</td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Take Price Range:&nbsp;</td><td valign='top' align='left' class='rightside'>No Take Prices</td></tr>")
                End If
            End If


            If CRMViewActive Then
                If forsalesoldlow > 0 Or forsalesoldhigh > 0 Then
                    marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Estimated Value Range:&nbsp;</td><td valign='top' align='left' nowrap='nowrap' class='rightside'>" + FormatCurrency(forsalesoldlow, 0, TriState.False, TriState.True, TriState.True).ToString + "k - " + FormatCurrency(forsalesoldhigh, 0, TriState.False, TriState.True, TriState.True).ToString + "k")
                    If avg_sold_count > 0 Then
                        marketHtmlOut.Append("&nbsp;&nbsp;&nbsp;(Avg: $" & FormatNumber(avg_sold, 0) & "k)")
                    End If
                    marketHtmlOut.Append("</td></tr>")
                Else
                    marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Estimated Value Range:&nbsp;</td><td valign='top' align='left' class='rightside'>No Estimated Values</td></tr>")
                End If
            End If




            If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                If CLng(ac_exclusive_sale) > 0 Then
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>On Exclusive:&nbsp;</td><td valign='top' align='left' class='rightside'>" + FormatNumber(ac_exclusive_sale, 0, TriState.False, TriState.False, TriState.True).ToString + " <span class='tiny'>(" + FormatNumber(per2, TriState.False, TriState.False, TriState.True).ToString + "% For Sale on Exclusive)</span></td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>On Exclusive:&nbsp;</td><td align='left' class='rightside'>0&nbsp;<span class='tiny'>(0% For Sale on Exclusive)</span></td></tr>")
                End If
            End If

            If avgyear > 0 Then
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Avg MFG Year:&nbsp;</td><td valign='top' align='left' class='rightside'>" + FormatNumber(avgyear, 0, TriState.False, TriState.False, TriState.False).ToString + "</td></tr>")
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Avg MFG Year:&nbsp;</td><td align='left' class='rightside'>N/A</td></tr>")
            End If

            If days_avg > 0 Then
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Avg Days on Market:&nbsp;</td><td valign='top' align='left' class='rightside'>" + FormatNumber(days_avg, 0, TriState.False, TriState.False, TriState.True).ToString + "</td></tr>")
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Avg Days on Market:&nbsp;</td><td align='left' class='rightside'>N/A</td></tr>")
            End If
            values_dom = days_avg

            If ac_lease > 0 Then
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='border_bottom'>Leased:&nbsp;</td><td valign='top' align='left' class='border_bottom'>" + FormatNumber(ac_lease, 0, TriState.False, TriState.False, TriState.True).ToString + "&nbsp;<span class='tiny'>(" + FormatNumber(per3, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span></td></tr>")
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='border_bottom'>Leased:&nbsp;</td><td align='left' class='border_bottom'>0&nbsp;<span class='tiny'>(0% of In Operation)</span></td></tr>")
            End If

            If absorp_rate > 0 Then
                absorp_rate = FormatNumber((FormatNumber(absorp_rate, 2) / number_of_months_divide), 2)
                absorp_rate = (FormatNumber(ac_for_sale, 2) / FormatNumber(absorp_rate, 2))
                If absorp_rate > 0 Then
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">" & FormatNumber(absorp_rate, 1) & "&nbsp;Months (Based on 6 Months of Sales)</span></td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">-</span></td></tr>")
                End If
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">-</span></td></tr>")
            End If


            marketHtmlOut.Append("</table>")

            fleetHtmlOut = New StringBuilder


            'If searchCriteria.ViewID = 16 Then
            '    fleetHtmlOut.Append("<div class=""valueSpec aircraftListing Simplistic aircraftSpec gray_background viewBoxMargin""><div class=""row""><div class=""eleven columns enableMarginColumn"">")
            '    fleetHtmlOut.Append(DisplayFunctions.BuildViewOwnershipBox("", w_owner, s_owner, f_owner, totalInOpcount, alllow, allhigh))
            '    fleetHtmlOut.Append("</div><div class=""eleven columns enableMarginColumn"">")
            '    fleetHtmlOut.Append(DisplayFunctions.BuildViewLifecycleBox("", o_stage, t_stage, th_stage, f_stage, totalcount))
            '    fleetHtmlOut.Append("</div>")


            '    fleetHtmlOut.Append("<div class=""eleven columns enableMarginColumn"">")
            '    fleetHtmlOut.Append(DisplayFunctions.BuildViewFleetCompBox("", alllow.ToString & " - " & allhigh.ToString, FormatNumber(all_aftt_low, 0).ToString & " - " & FormatNumber(all_aftt_high, 0).ToString, IIf(us_reg > 0, us_reg & "/" & (th_stage - us_reg), "")))
            '    fleetHtmlOut.Append("</div>")

            '    If Not String.IsNullOrEmpty(modelImage) Then
            '        fleetHtmlOut.Append("<div class=""eleven columns enableMarginColumn""><img src=""" & modelImage & """ width=""235"" style=""margin-top:-3px;""/></div>")
            '    End If

            '    fleetHtmlOut.Append("</div></div>")
            'Else
            fleetHtmlOut.Append("<div class=""row""><div class=""" & IIf(String.IsNullOrEmpty(modelImage), "four", "three") & " columns enableMarginColumn"">")
            fleetHtmlOut.Append(DisplayFunctions.BuildViewOwnershipBox("", w_owner, s_owner, f_owner, totalInOpcount, alllow, allhigh))
            fleetHtmlOut.Append("</div><div class=""" & IIf(String.IsNullOrEmpty(modelImage), "four", "three") & " columns enableMarginColumn"">")
            fleetHtmlOut.Append(DisplayFunctions.BuildViewLifecycleBox("", o_stage, t_stage, th_stage, f_stage, totalcount))
            fleetHtmlOut.Append("</div>")

            If searchCriteria.ViewID = 1 Or searchCriteria.ViewID = 11 Or searchCriteria.ViewID = 16 Then
                fleetHtmlOut.Append("<div class=""" & IIf(String.IsNullOrEmpty(modelImage), "four", "three") & " columns enableMarginColumn"">")
                fleetHtmlOut.Append(DisplayFunctions.BuildViewFleetCompBox("", alllow.ToString & " - " & allhigh.ToString, FormatNumber(all_aftt_low, 0).ToString & " - " & FormatNumber(all_aftt_high, 0).ToString, IIf(us_reg > 0, us_reg & "/" & (th_stage - us_reg), "")))
                fleetHtmlOut.Append("</div>")
            End If

            If Not String.IsNullOrEmpty(modelImage) Then
                fleetHtmlOut.Append("<div class=""three columns enableMarginColumn""><img src=""" & modelImage & """ width=""235"" style=""margin-top:-3px;""/></div>")
            End If

            fleetHtmlOut.Append("</div>")
            ' End If


            values_total_inop = totalInOpcount
            values_in_op = ((ac_for_sale / totalInOpcount) * 100)


            out_Build_FleetMarketSummary_text = fleetHtmlOut.ToString.Trim
            out_GetMarketStatus = marketHtmlOut.ToString.Trim

        Catch ex As Exception

            class_error = "Error in Combined_views_Build_FleetMarketSummary(ByVal in_nModelID As Long, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        fleetHtmlOut = Nothing
        marketHtmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Shared Function CLIENT_get_fleet_market_summary_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean, Optional ByVal number_of_months_divide As Integer = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()


        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim sqlWhere As String = ""

        Try

            If Not bUseCharterQuery Then

                sQuery.Append("SELECT cliaircraft_country_of_registration as ac_country_of_registration, cliaircraft_id as ac_id, cliaircraft_ownership as ac_ownership_type, cliaircraft_est_price as ac_take_price, 'CLIENT' as source, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, cliaircraft_lifecycle as ac_lifecycle_stage, cliaircraft_airframe_total_hours as ac_airframe_tot_Hrs, cliaircraft_forsale_flag as ac_forsale_flag, cliaircraft_exclusive_flag as ac_exclusive_flag,")
                sQuery.Append(" cliaircraft_lease_flag as ac_lease_flag, cliaircraft_asking_wordage as ac_asking, cliaircraft_broker_price as sold_price, cliaircraft_asking_price as ac_asking_price, cliaircraft_date_listed as ac_list_date, cliaircraft_year_mfr as ac_mfr_year, DATEDIFF(cliaircraft_date_listed,NOW()) AS daysonmarket, cliaircraft_airframe_total_landings as ac_airframe_tot_landings ")
                If number_of_months_divide > 0 Then
                    sQuery.Append(", 0 as SalesPerTimeframe ")
                End If

                sQuery.Append(" FROM client_aircraft INNER JOIN Client_aircraft_model ON cliamod_id = cliaircraft_cliamod_id")


                If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                    Dim tmpStr As String = ""

                    ' flatten out amodID array ...
                    For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                        If String.IsNullOrEmpty(tmpStr) Then
                            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                        Else
                            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                        End If
                    Next

                    sqlWhere = "cliamod_jetnet_amod_id IN (" + tmpStr.Trim + ")"
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sqlWhere = "cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString
                ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
                    sqlWhere = "cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString
                ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
                    sqlWhere = "cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString
                ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
                    sqlWhere = "cliamod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')"
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sqlWhere = "cliamod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'"
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sqlWhere = "cliamod_make_type = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'"
                End If

                If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                    If sqlWhere <> "" Then
                        sqlWhere += " and "
                    End If
                    sqlWhere += " ((cliaircraft_airframe_total_hours >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (cliaircraft_airframe_total_hours IS NULL))"
                End If

                If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                    If sqlWhere <> "" Then
                        sqlWhere += " and "
                    End If
                    sqlWhere += " ((cliaircraft_airframe_total_hours <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (cliaircraft_airframe_total_hours IS NULL))"
                End If

                If searchCriteria.ViewCriteriaYearStart > 0 Then
                    If sqlWhere <> "" Then
                        sqlWhere += " and "
                    End If
                    sqlWhere += " cliaircraft_year_mfr >= " & searchCriteria.ViewCriteriaYearStart
                End If

                If searchCriteria.ViewCriteriaYearEnd > 0 Then
                    If sqlWhere <> "" Then
                        sqlWhere += " and "
                    End If
                    sqlWhere += " cliaircraft_year_mfr <=  " & searchCriteria.ViewCriteriaYearEnd
                End If

                Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                    Case crmWebClient.Constants.VIEW_EXECUTIVE
                        If sqlWhere <> "" Then
                            sqlWhere += " and "
                        End If
                        sqlWhere += "cliamod_airframe_type='F' AND cliamod_make_type = 'E'"
                    Case crmWebClient.Constants.VIEW_JETS
                        If sqlWhere <> "" Then
                            sqlWhere += " and "
                        End If
                        sqlWhere += "cliamod_airframe_type='F' AND cliamod_make_type = 'J'"
                    Case crmWebClient.Constants.VIEW_TURBOPROPS
                        If sqlWhere <> "" Then
                            sqlWhere += " and "
                        End If
                        sqlWhere += "cliamod_airframe_type='F' AND cliamod_make_type = 'T'"
                    Case crmWebClient.Constants.VIEW_PISTONS
                        If sqlWhere <> "" Then
                            sqlWhere += " and "
                        End If
                        sqlWhere += "cliamod_airframe_type='F' AND cliamod_make_type = 'P'"
                    Case crmWebClient.Constants.VIEW_HELICOPTERS
                        If sqlWhere <> "" Then
                            sqlWhere += " and "
                        End If
                        sqlWhere += "cliamod_airframe_type='R' AND cliamod_make_type in ('T','P')"
                End Select

                If sqlWhere <> "" Then
                    sQuery.Append(" where " & sqlWhere)
                End If

            Else


                sQuery.Append("SELECT cliaircraft_id as ac_id, cliaircraft_est_price as ac_take_price, 'CLIENT' as source, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, cliaircraft_ownership as ac_ownership_type, cliaircraft_lifecycle as ac_lifecycle_stage, cliaircraft_forsale_flag as ac_forsale_flag, cliaircraft_exclusive_flag as ac_exclusive_flag,")
                sQuery.Append(" cliaircraft_lease_flag as ac_lease_flag, cliaircraft_asking_wordage as ac_asking, cliaircraft_asking_price as ac_asking_price, cliaircraft_date_listed as ac_list_date, cliaircraft_year_mfr as ac_mfr_year, DATEDIFF(cliaircraft_date_listed,NOW()) AS daysonmarket")

                sQuery.Append(" FROM client_aircraft INNER JOIN client_aircraft_model WITH(NOLOCK) ON cliamod_id = cliaircraft_cliamod_id WHERE ")

                If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                    Dim tmpStr As String = ""

                    ' flatten out amodID array ...
                    For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                        If String.IsNullOrEmpty(tmpStr) Then
                            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                        Else
                            tmpStr += crmWebClient.Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                        End If
                    Next

                    sQuery.Append("cliamod_jetnet_amod_id IN (" + tmpStr.Trim + ")")
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append("cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
                    sQuery.Append("cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
                ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
                    sQuery.Append("cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
                ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
                    sQuery.Append("cliamod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(crmWebClient.Constants.cCommaDelim, crmWebClient.Constants.cValueSeperator).Trim + "')")
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append("cliamod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append("cliamod_make_type = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
                End If

                Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                    Case crmWebClient.Constants.VIEW_EXECUTIVE
                        sQuery.Append(crmWebClient.Constants.cAndClause + "cliamod_airframe_type='F' AND cliamod_make_type = 'E'")
                    Case crmWebClient.Constants.VIEW_JETS
                        sQuery.Append(crmWebClient.Constants.cAndClause + "cliamod_airframe_type='F' AND cliamod_make_type = 'J'")
                    Case crmWebClient.Constants.VIEW_TURBOPROPS
                        sQuery.Append(crmWebClient.Constants.cAndClause + "cliamod_airframe_type='F' AND cliamod_make_type = 'T'")
                    Case crmWebClient.Constants.VIEW_PISTONS
                        sQuery.Append(crmWebClient.Constants.cAndClause + "cliamod_airframe_type='F' AND cliamod_make_type = 'P'")
                    Case crmWebClient.Constants.VIEW_HELICOPTERS
                        sQuery.Append(crmWebClient.Constants.cAndClause + "cliamod_airframe_type='R' AND cliamod_make_type in ('T','P')")
                End Select


                sQuery.Append(" AND EXISTS (SELECT NULL FROM client_aircraft_reference WITH(NOLOCK)")
                sQuery.Append(" WHERE cliacref_cliac_id = cliaircraft_id ")
                sQuery.Append(" AND (cliacref_contact_type IN ('94','33') OR cliacref_business_type = 'CH'))")


            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'HttpContext.Current.Application.Item("crmClientDatabase") 'clientConnectString
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                class_error = "Error in CLIENT_get_fleet_market_summary_info load datatable " + constrExc.Message
            End Try


        Catch ex As Exception
            Return Nothing

            class_error = "Error in CLIENT_get_fleet_market_summary_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function
#End Region
#Region "Used to Combine Client/Jetnet"





    Public Shared Function CombineTwoAircraftDatatables_Custom(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable, ByRef FullClientIDsToExclude As String, ByRef UseFullClientIDs As Boolean, ByVal order_by As String, ByVal is_retail As String) As String
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column5 As New DataColumn
        Dim Column6 As New DataColumn
        Dim Column7 As New DataColumn
        Dim IDsToExclude As String = ""

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        column.DataType = System.Type.GetType("System.String")
        column.DefaultValue = "JETNET"
        column.Unique = False
        column.ColumnName = "source"
        JetnetTable.Columns.Add(column)

        Column6.DataType = System.Type.GetType("System.Double")
        Column6.DefaultValue = 0
        Column6.AllowDBNull = True
        Column6.Unique = False
        Column6.ColumnName = "EVALUE"
        ClientTable.Columns.Add(Column6)

        Column7.DataType = System.Type.GetType("System.Double")
        Column7.DefaultValue = 0
        Column7.AllowDBNull = True
        Column7.Unique = False
        Column7.ColumnName = "AVGMODYREVALUE"
        ClientTable.Columns.Add(Column7)

        column2.DataType = System.Type.GetType("System.Int64")
        column2.DefaultValue = 0
        column2.Unique = False
        column2.ColumnName = "client_jetnet_ac_id"
        JetnetTable.Columns.Add(column2)

        column3.DataType = System.Type.GetType("System.Double")
        column3.AllowDBNull = True
        column3.Unique = False
        column3.ColumnName = "ac_take_price"
        JetnetTable.Columns.Add(column3)

        column4.DataType = System.Type.GetType("System.Double")
        column4.AllowDBNull = True
        column4.Unique = False
        column4.ColumnName = "ac_sold_price"
        JetnetTable.Columns.Add(column4)

        column5.DataType = System.Type.GetType("System.Double")
        column5.AllowDBNull = True
        column5.Unique = False
        column5.ColumnName = "retail_flag"
        JetnetTable.Columns.Add(column5)


        'First we need to loop through the client data to get a list for our not in statement on the jetnet side.
        If UseFullClientIDs Then
            IDsToExclude = FullClientIDsToExclude
        Else
            For Each drRow As DataRow In ClientTable.Rows
                If IDsToExclude <> "" Then
                    IDsToExclude += ", "
                End If
                IDsToExclude += drRow("client_jetnet_ac_id").ToString
            Next
            IDsToExclude = IDsToExclude
        End If
        'First we copy the Client data. This allows the return table to have
        'The Client Data In it.
        ReturnTable = ClientTable.Copy
        If Trim(is_retail) = "Y" Then
            ReturnTable.Rows.Clear()
        End If

        ReturnTable.Constraints.Clear()


        For i = 0 To ReturnTable.Columns.Count - 1
            If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
                ReturnTable.Columns(i).MaxLength = 1000
            End If
        Next

        If IDsToExclude <> "" Then
            Dim afiltered_Jetnet As DataRow() = JetnetTable.Select(" ac_id not in (" & IDsToExclude & ") ", "")
            For Each drJetnet In afiltered_Jetnet
                ReturnTable.ImportRow(drJetnet)
            Next


            If Trim(is_retail) = "Y" Then
                Dim afiltered_Jetnet2 As DataRow() = ClientTable.Select(" retail_flag = 'Y' ", "")
                For Each drJetnet2 In afiltered_Jetnet2
                    ReturnTable.ImportRow(drJetnet2)
                Next
            End If

        Else
            'Nothing to exclude, so we go ahead and import the jetnet data as is.
            For Each drRow As DataRow In JetnetTable.Rows
                ReturnTable.ImportRow(drRow)
            Next
        End If

        Return IDsToExclude
    End Function

    ''' <summary>
    ''' A generic function that will merge the jetnet/client table together, excluding either the client IDs in the table or the
    ''' optional parameter of IDs that you send it.
    ''' </summary>
    ''' <param name="ClientTable"></param>
    ''' <param name="JetnetTable"></param>
    ''' <param name="ReturnTable"></param>
    ''' <param name="FullClientIDsToExclude"></param>
    ''' <param name="UseFullClientIDs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CombineTwoAircraftDatatables(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable, ByRef FullClientIDsToExclude As String, ByRef UseFullClientIDs As Boolean) As String
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column5 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column6 As New DataColumn
        Dim column7 As New DataColumn
        Dim IDsToExclude As String = ""

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        column.DataType = System.Type.GetType("System.String")
        column.DefaultValue = "JETNET"
        column.Unique = False
        column.ColumnName = "source"
        JetnetTable.Columns.Add(column)

        column2.DataType = System.Type.GetType("System.Int64")
        column2.DefaultValue = 0
        column2.Unique = False
        column2.ColumnName = "client_jetnet_ac_id"
        JetnetTable.Columns.Add(column2)

        column3.DataType = System.Type.GetType("System.Double")
        column3.AllowDBNull = True
        column3.Unique = False
        column3.ColumnName = "ac_take_price"
        JetnetTable.Columns.Add(column3)

        column4.DataType = System.Type.GetType("System.Double")
        column4.AllowDBNull = True
        column4.Unique = False
        column4.ColumnName = "ac_sold_price"
        JetnetTable.Columns.Add(column4)


        column5.DataType = System.Type.GetType("System.Int64")
        column5.DefaultValue = 0
        column5.ColumnName = "client_model_id"
        JetnetTable.Columns.Add(column5)

        column6.DataType = System.Type.GetType("System.Double")
        column6.DefaultValue = 0
        column6.AllowDBNull = True
        column6.Unique = False
        column6.ColumnName = "EVALUE"
        ClientTable.Columns.Add(column6)

        column7.DataType = System.Type.GetType("System.Double")
        column7.DefaultValue = 0
        column7.AllowDBNull = True
        column7.Unique = False
        column7.ColumnName = "AVGMODYREVALUE"
        ClientTable.Columns.Add(column7)

        'First we need to loop through the client data to get a list for our not in statement on the jetnet side.
        If UseFullClientIDs Then
            IDsToExclude = FullClientIDsToExclude
        Else
            For Each drRow As DataRow In ClientTable.Rows
                If IDsToExclude <> "" Then
                    IDsToExclude += ", "
                End If
                IDsToExclude += drRow("client_jetnet_ac_id").ToString
            Next
            IDsToExclude = IDsToExclude
        End If
        'First we copy the Client data. This allows the return table to have
        'The Client Data In it.
        ReturnTable = ClientTable.Copy

        ReturnTable.Constraints.Clear()

        For i = 0 To ReturnTable.Columns.Count - 1
            If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
                ReturnTable.Columns(i).MaxLength = 1000
            End If
        Next


        If IDsToExclude <> "" Then
            Dim afiltered_Jetnet As DataRow() = JetnetTable.Select(" ac_id not in (" & IDsToExclude & ") ", "")
            For Each drJetnet In afiltered_Jetnet
                Try
                    ReturnTable.ImportRow(drJetnet)
                Catch ex As Exception
                End Try
            Next
        Else
            'Nothing to exclude, so we go ahead and import the jetnet data as is.
            For Each drRow As DataRow In JetnetTable.Rows
                Try
                    ReturnTable.ImportRow(drRow)
                Catch ex As Exception
                End Try
            Next
        End If

        Return IDsToExclude
    End Function
    ''' <summary>
    ''' This preps/sorts the merged datatable for the for sale tab of the model view.
    ''' </summary>
    ''' <param name="ClientTable"></param>
    ''' <param name="JetnetTable"></param>
    ''' <param name="searchCriteria"></param>
    ''' <param name="FullClientIDsToExclude"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ModifyAndCombineJetnetClientDataForSale(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef FullClientIDsToExclude As String, ByVal order_by_string As String, ByVal is_retail As String) As DataTable
        Dim ReturnTable As New DataTable 'Datatable to be returned.
        Dim DefaultSort As String = ""

        'This will go ahead and merge the two datatables together, adding  fields that we need to merge schemas.
        'If needed, this could return the IDS we excluded running through the table. That's used more on the market status tab function though
        'Since it will return all the IDs, that way we can pass it along down here as well

        If Trim(order_by_string) = "" Then
            CombineTwoAircraftDatatables(ClientTable, JetnetTable, ReturnTable, FullClientIDsToExclude, True)
        Else
            CombineTwoAircraftDatatables_Custom(ClientTable, JetnetTable, ReturnTable, FullClientIDsToExclude, False, order_by_string, is_retail)
        End If

        Try


            Dim Filtered_DV As New DataView(ReturnTable)


            If Trim(order_by_string) = "" Then
                'But we're not done yet, because we have to sort the datatable. 
                Select Case (searchCriteria.ViewCriteriaSortBy.ToLower)
                    Case "serno"
                        Filtered_DV.Sort = "ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc"

                    Case "aftt"
                        Filtered_DV.Sort = "ac_airframe_tot_hrs, ac_ser_no_sort, ac_list_date, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc"

                    Case "mfryear"
                        Filtered_DV.Sort = "ac_mfr_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_year, ac_asking_price desc, ac_asking asc"

                    Case "acyear"
                        Filtered_DV.Sort = "ac_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_asking_price desc, ac_asking asc"

                    Case "listdate"
                        Filtered_DV.Sort = "ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc"

                    Case "asking"
                        Filtered_DV.Sort = "ac_asking_price desc, ac_asking asc, ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year"

                    Case Else
                        Filtered_DV.Sort = "ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc"

                End Select
            Else
                order_by_string = Replace(order_by_string, "'Transaction Date'", "'Transaction Date' DESC")
                Filtered_DV.Sort = Replace(order_by_string, "'", "")
            End If

            ReturnTable = Filtered_DV.ToTable

        Catch ex As Exception

        End Try

        Return ReturnTable
    End Function


    Public Shared Sub CombineTwoWantedDatatables(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable)
        Dim column As New DataColumn 'Column to Add Source to jetnet data.

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        column.DataType = System.Type.GetType("System.String")
        column.DefaultValue = "JETNET"
        column.Unique = False
        column.ColumnName = "source"
        JetnetTable.Columns.Add(column)

        'First we copy the jetnet data. This allows the return table to have
        'The Client Data In it.
        ReturnTable = JetnetTable.Copy

        'Nothing to exclude, so we go ahead and import the client data as is.
        For Each drRow As DataRow In ClientTable.Rows
            ReturnTable.ImportRow(drRow)
        Next

        'Sorting
        Dim Filtered_DV As New DataView(ReturnTable)

        Filtered_DV.Sort = "amwant_listed_date DESC"

        ReturnTable = Filtered_DV.ToTable
    End Sub


    Public Shared Function CombineTwoHistoryDatatables(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable, ByVal is_retail As String) As String
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn 'Column to add sold price
        Dim column5 As New DataColumn 'Column to add sold price type
        Dim column6 As New DataColumn 'Column to add sold price type
        Dim column7 As New DataColumn 'Column to add sold price type
        Dim column8 As New DataColumn
        Dim IDsToExclude As String = ""

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        column.DataType = System.Type.GetType("System.String")
        column.DefaultValue = "JETNET"
        column.Unique = False
        column.ColumnName = "source"
        JetnetTable.Columns.Add(column)

        column2.DataType = System.Type.GetType("System.Int64")
        column2.DefaultValue = 0
        column2.Unique = False
        column2.ColumnName = "client_jetnet_trans_id"
        JetnetTable.Columns.Add(column2)

        column3.DataType = System.Type.GetType("System.Double")
        column3.AllowDBNull = True
        column3.Unique = False
        column3.ColumnName = "ac_take_price"
        JetnetTable.Columns.Add(column3)

        'added in now 
        'column4.DataType = System.Type.GetType("System.Double")
        'column4.AllowDBNull = True
        'column4.Unique = False
        'column4.ColumnName = "ac_sold_price"
        'JetnetTable.Columns.Add(column4)

        column5.DataType = System.Type.GetType("System.String")
        column5.DefaultValue = ""
        column5.Unique = False
        column5.ColumnName = "ac_sold_price_type"
        JetnetTable.Columns.Add(column5)


        column6.DataType = System.Type.GetType("System.Int64")
        column6.DefaultValue = 0
        column6.Unique = False
        column6.ColumnName = "jetnet_ac_id"
        JetnetTable.Columns.Add(column6)

        column7.DataType = System.Type.GetType("System.String")
        column7.DefaultValue = is_retail
        column7.Unique = False
        column7.ColumnName = "retail_flag"
        JetnetTable.Columns.Add(column7)


        'column8.DataType = System.Type.GetType("System.String")
        'column8.DefaultValue = is_retail
        'column8.Unique = False
        'column8.ColumnName = "ac_sale_price_display_flag"
        'JetnetTable.Columns.Add(column8)



        For Each drRow As DataRow In ClientTable.Rows
            If IDsToExclude <> "" Then
                IDsToExclude += ", "
            End If
            IDsToExclude += drRow("client_jetnet_trans_id").ToString
        Next
        IDsToExclude = IDsToExclude

        'First we copy the Client data. This allows the return table to have
        'The Client Data In it.
        ReturnTable = ClientTable.Copy

        If Trim(is_retail) = "Y" Then
            ReturnTable.Rows.Clear()
        End If

        ReturnTable.Constraints.Clear()

        For i = 0 To ReturnTable.Columns.Count - 1
            If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
                ReturnTable.Columns(i).MaxLength = 1000
            End If
        Next


        If IDsToExclude <> "" Then

            If Trim(is_retail) = "Y" Then
                Dim afiltered_Jetnet2 As DataRow() = ClientTable.Select(" retail_flag = 'Y' ", "")
                For Each drJetnet2 In afiltered_Jetnet2
                    ReturnTable.ImportRow(drJetnet2)
                Next
            End If

            Dim afiltered_Jetnet As DataRow() = JetnetTable.Select(" journ_id not in (" & IDsToExclude & ") ", "")
            For Each drJetnet In afiltered_Jetnet
                ReturnTable.ImportRow(drJetnet)
            Next
        Else
            'Nothing to exclude, so we go ahead and import the jetnet data as is.
            For Each drRow As DataRow In JetnetTable.Rows
                ReturnTable.ImportRow(drRow)
            Next
        End If

        'Let's reorder this:
        'Sorting
        Dim Filtered_DV As New DataView(ReturnTable)

        Filtered_DV.Sort = "journ_date DESC"

        ReturnTable = Filtered_DV.ToTable

        Return IDsToExclude
    End Function


    Public Shared Function CombineTwoHistoryDatatables_Empty_Client(ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable, ByVal is_retail As String) As String
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn 'Column to add sold price
        Dim column5 As New DataColumn 'Column to add sold price type
        Dim column6 As New DataColumn 'Column to add sold price type
        Dim column7 As New DataColumn 'Column to add sold price type
        Dim column8 As New DataColumn
        Dim IDsToExclude As String = ""

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        column.DataType = System.Type.GetType("System.String")
        column.DefaultValue = "JETNET"
        column.Unique = False
        column.ColumnName = "source"
        JetnetTable.Columns.Add(column)

        column2.DataType = System.Type.GetType("System.Int64")
        column2.DefaultValue = 0
        column2.Unique = False
        column2.ColumnName = "client_jetnet_trans_id"
        JetnetTable.Columns.Add(column2)

        column3.DataType = System.Type.GetType("System.Double")
        column3.AllowDBNull = True
        column3.Unique = False
        column3.ColumnName = "ac_take_price"
        JetnetTable.Columns.Add(column3)

        'added in now 
        'column4.DataType = System.Type.GetType("System.Double")
        'column4.AllowDBNull = True
        'column4.Unique = False
        'column4.ColumnName = "ac_sold_price"
        'JetnetTable.Columns.Add(column4)

        column5.DataType = System.Type.GetType("System.String")
        column5.DefaultValue = ""
        column5.Unique = False
        column5.ColumnName = "ac_sold_price_type"
        JetnetTable.Columns.Add(column5)


        column6.DataType = System.Type.GetType("System.Int64")
        column6.DefaultValue = 0
        column6.Unique = False
        column6.ColumnName = "jetnet_ac_id"
        JetnetTable.Columns.Add(column6)

        column7.DataType = System.Type.GetType("System.String")
        column7.DefaultValue = is_retail
        column7.Unique = False
        column7.ColumnName = "retail_flag"
        JetnetTable.Columns.Add(column7)


        'column8.DataType = System.Type.GetType("System.String")
        'column8.DefaultValue = is_retail
        'column8.Unique = False
        'column8.ColumnName = "ac_sale_price_display_flag"
        'JetnetTable.Columns.Add(column8)


        ReturnTable = JetnetTable.Copy

        If Trim(is_retail) = "Y" Then
            ReturnTable.Rows.Clear()
        End If

        ReturnTable.Constraints.Clear()

        For i = 0 To ReturnTable.Columns.Count - 1
            If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
                ReturnTable.Columns(i).MaxLength = 1000
            End If
        Next


        'Let's reorder this:
        'Sorting
        Dim Filtered_DV As New DataView(ReturnTable)

        Filtered_DV.Sort = "journ_date DESC"

        ReturnTable = Filtered_DV.ToTable

        Return IDsToExclude
    End Function
#End Region
#Region "For Sale Tab Functions"
    '''' <summary>
    '''' I'm doing this on purpose.
    '''' I need to edit this function, and I need to heavily edit it to add CRM capability.
    '''' It is my hope and intention to edit this function by making a copy of the one in the ViewsDataLayer
    '''' And then go ahead and replace the old one, that way there's not two of the same function
    '''' However since we have to keep old cability and I'm breaking this one, there's a need for two.
    '''' </summary>
    '''' <param name="searchCriteria"></param>
    '''' <param name="out_htmlString"></param>
    '''' <param name="is_extra_criteria"></param>
    '''' <param name="JetnetViewData"></param>
    '''' <param name="CRMViewActive"></param>
    '''' <param name="FullClientIDstoExclude"></param>
    '''' <remarks></remarks>
    'Public Shared Sub views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal is_extra_criteria As Boolean, ByRef JetnetViewData As viewsDataLayer, ByRef CRMViewActive As Boolean, ByRef FullClientIDstoExclude As String, ByVal DisplayLink As Boolean, ByRef table_to_add As DataTable, Optional ByVal NOTE_ID As Long = 0, Optional ByVal REAL_AC_ID As Long = 0, Optional ByVal COMPLETED_DATE As String = "", Optional ByVal page_break_after As Integer = 0, Optional ByVal header_text As String = "", Optional ByVal is_word As Boolean = True, Optional ByVal jetnet_string As String = "", Optional ByVal client_string As String = "", Optional ByVal order_by_string As String = "", Optional ByVal fields_name As String = "", Optional ByVal type_string As String = "", Optional ByVal size_string As String = "", Optional ByVal page_break_header As String = "", Optional ByRef string_for_export As String = "", Optional ByRef ActiveTabIndex As Long = 0, Optional ByVal run_market_insert As Boolean = False)
    '  Dim aclsData_Temp As New clsData_Manager_SQL
    '  Dim arrFeatCodes() As String = Nothing

    '  Dim arrStdFeatCodes(,) As String = Nothing

    '  Dim strOut As New StringBuilder
    '  Dim htmlOut As New StringBuilder
    '  Dim HTML_NOTE As String = ""

    '  Dim results_table As New DataTable
    '  Dim toggleRowColor As Boolean = False

    '  Dim bHadStatus As Boolean = False
    '  Dim cellWidth As Integer = 20

    '  Dim nFeatureCountForSpan As Integer = 0
    '  Dim table_height As Integer = 0
    '  Dim sCompanyPhone As String = ""
    '  Dim orig_view As Boolean = False
    '  Dim font_shrink As String = ""
    '  Dim start_text As String = ""
    '  Dim row_count As Integer = 0
    '  Dim pages_made As Integer = 1
    '  Dim order_by_string_break() As String
    '  Dim db_fields_names() As String

    '  Dim temp_field As String = ""
    '  Dim temp_val As String = ""
    '  Dim format_me As Boolean = False
    '  Dim temp_field_name As String = ""
    '  Dim type_string_names() As String
    '  Dim size_string_names() As String

    '  Dim temp_type As String = ""
    '  Dim temp_size As String = ""

    '  Dim strOut_Export As New StringBuilder
    '  Dim htmlOut_Export As New StringBuilder
    '  Dim start_text_export As String = ""
    '  Dim new_note_id As Long = 0
    '  Dim aclsLocal_Notes As New clsLocal_Notes

    '  Dim t_ask As Long = 0
    '  Dim t_take As Long = 0
    '  Dim t_sold As Long = 0


    '  Dim ac_temp As New clsClient_Aircraft

    '  Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
    '  Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    '  Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing
    '  Dim JetnetTable As New DataTable 'Table that stores Jetnet Data.
    '  Dim clientTable As New DataTable 'Table that stores Client Data.

    '  Dim CurrentData As New DataTable
    '  Dim delete_string As String = ""


    '  orig_view = searchCriteria.ViewCriteriaIsReport
    '  searchCriteria.ViewCriteriaIsReport = is_extra_criteria

    '  'Setting up the CRM/Evolution Connections to clsDataManager
    '  If CRMViewActive Then
    '    'Set up connection
    '    aclsData_Temp.client_DB = HttpContext.Current.Application.Item("crmClientDatabase")
    '    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
    '  Else
    '    'Setting up with the correct connections.
    '    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
    '    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
    '  End If


    '  Try


    '    If DisplayLink Then
    '      font_shrink = "<font>"
    '    Else
    '      If is_word Then
    '        font_shrink = "<font size='-3'>"
    '      Else
    '        font_shrink = "<font size='-2'>"
    '      End If


    '    End If

    '    If DisplayLink Or Trim(order_by_string) = "" Then
    '      strOut.Append("<table id='forSaleOuterTable' cellspacing='0' cellpadding='0' width=""100%"">")
    '    End If


    '    If CRMViewActive = True Then
    '      If DisplayLink Or Trim(order_by_string) = "" Then
    '        strOut.Append("<tr valign='top'><td valign='middle' align='center'  bgcolor='#C0C0C0' style='padding-left:3px;'>")
    '      End If
    '    Else
    '      If Not searchCriteria.ViewCriteriaIsReport And DisplayLink Then
    '        strOut.Append("<tr><td valign='middle' align='center' class='header' style='padding-left:3px;'>")
    '        strOut.Append("&nbsp;&nbsp;Sort List By&nbsp;<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=serno&activetab=1' class='White'><b>Serial&nbsp;#</b></a>&nbsp;or&nbsp;")
    '        strOut.Append("<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=aftt&activetab=1' class='White'><b>AFTT</b></a>&nbsp;or&nbsp;")
    '        strOut.Append("<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=mfryear&activetab=1' class='White'><b>Aircraft&nbsp;MFR&nbsp;Year</b></a>&nbsp;or&nbsp;")
    '        strOut.Append("<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=acyear&activetab=1' class='White'><b>Aircraft&nbsp;DLV&nbsp;Year</b></a>&nbsp;or&nbsp;")
    '        strOut.Append("<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=listdate&activetab=1' class='White'><b>Date&nbsp;Listed</b></a>&nbsp;or&nbsp;")
    '        strOut.Append("<a href='view_template.aspx?ViewID=" & Trim(HttpContext.Current.Request("ViewID")) & "&ViewName=" & Trim(HttpContext.Current.Request("ViewName")) & "&sortBy=asking&activetab=1' class='White'><b>Asking&nbsp;Price</b></a>")
    '      Else
    '        If page_break_after > 0 Then
    '          strOut.Append("<tr bgcolor='#CCCCCC'><td valign='middle' align='center' bgcolor='#C0C0C0' style='padding-left:3px;'><strong>")
    '        Else
    '          strOut.Append("<tr bgcolor='#CCCCCC'><td valign='middle' align='center'  bgcolor='#C0C0C0' style='padding-left:3px;'><strong>")
    '        End If
    '      End If
    '    End If



    '    If run_market_insert = True Then
    '      aclsLocal_Notes.lnote_jetnet_comp_id = 0
    '      aclsLocal_Notes.lnote_client_comp_id = 0
    '      aclsLocal_Notes.lnote_client_contact_id = 0
    '      aclsLocal_Notes.lnote_jetnet_contact_id = 0
    '      aclsLocal_Notes.lnote_document_flag = "N"
    '      aclsLocal_Notes.lnote_note = ""
    '      aclsLocal_Notes.lnote_id = 0
    '      aclsLocal_Notes.lnote_action_date = CDate(Date.Now()).Date
    '      aclsLocal_Notes.lnote_clipri_ID = 1
    '      aclsLocal_Notes.lnote_user_login = HttpContext.Current.Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
    '      aclsLocal_Notes.lnote_user_name = Left(HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName, 15)
    '      aclsLocal_Notes.lnote_user_id = HttpContext.Current.Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
    '      aclsLocal_Notes.lnote_status = "S"



    '      CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(HttpContext.Current.Session.Item("CLIENT_AC_ID"))
    '      If Not IsNothing(CurrentData) Then
    '        If CurrentData.Rows.Count > 0 Then
    '          ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "cliaircraft")

    '          aclsLocal_Notes.lnote_client_amod_id = ac_temp.cliaircraft_cliamod_id
    '          aclsLocal_Notes.lnote_jetnet_amod_id = searchCriteria.ViewCriteriaAmodID
    '        End If
    '      End If

    '      '--------------------- INSERT INTO NOTE RECORD if one doesnt exist---------- 

    '      new_note_id = aclsData_Temp.Check_If_Note_Exists(Date.Now, searchCriteria.ViewCriteriaAmodID, ac_temp.cliaircraft_cliamod_id)
    '      If new_note_id = 0 Then
    '        new_note_id = aclsData_Temp.Insert_Note(aclsLocal_Notes)
    '      Else
    '        Try
    '          delete_string = " delete from client_value_comparables where clival_note_id = " & new_note_id & " "
    '          If NOTE_ID > 0 And Trim(delete_string) <> "" Then
    '            SqlConn.ConnectionString = aclsData_Temp.client_DB
    '            SqlConn.Open()
    '            SqlCommand.Connection = SqlConn

    '            SqlCommand.CommandText = delete_string
    '            SqlCommand.ExecuteNonQuery()
    '          End If
    '        Catch ex As Exception
    '        Finally
    '          SqlCommand.Dispose()
    '          SqlConn.Close()
    '          SqlConn.Dispose()
    '        End Try
    '      End If
    '      '--------------------- INSERT INTO NOTE RECORD if one doesnt exist---------- 
    '    End If




    '    If Trim(order_by_string) = "" Then

    '      'This grabs the data for the jetnet aircraft for sale
    '      JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string)

    '      'This grabs the client side aircraft for sale
    '      clientTable = get_client_model_forsale_info(searchCriteria, client_string, order_by_string)

    '      'This takes those two datatables, excludes the jetnet ones we have client aircraft for, adds the extra fields to make
    '      'The schemas match and then merges them into results table.
    '      results_table = ModifyAndCombineJetnetClientDataForSale(clientTable, JetnetTable, searchCriteria, FullClientIDstoExclude, order_by_string, "N")

    '      If Not IsNothing(table_to_add) Then
    '        table_to_add = results_table
    '      End If

    '      If Not IsNothing(results_table) Then

    '        If results_table.Rows.Count > 0 Then


    '          If Not searchCriteria.ViewCriteriaIsReport Then
    '            strOut.Append("" & font_shrink & "&nbsp;&nbsp;&nbsp;&nbsp;AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
    '            strOut_Export.Append("" & font_shrink & "&nbsp;&nbsp;&nbsp;&nbsp;AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
    '          Else
    '            strOut.Append("" & font_shrink & "AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></strong></td></tr>")
    '            strOut_Export.Append("" & font_shrink & "AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></strong></td></tr>")
    '          End If

    '          If Not searchCriteria.ViewCriteriaIsReport Then

    '            If is_word Then
    '              If page_break_after > 0 Then
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1' valign='top'  width=""100%""><thead><tr valign='top'>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1' valign='top'><thead><tr valign='top'>")
    '              Else
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0' valign='top'  width=""100%""><thead><tr valign='top'>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0' valign='top'><thead><tr valign='top'>")
    '              End If
    '            Else
    '              If page_break_after > 0 Then
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1' valign='top'  width=""100%""><thead><tr valign='top'>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1' valign='top'><thead><tr valign='top'>")
    '              Else
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0' valign='top'  width=""100%""><thead><tr valign='top'>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0' valign='top'><thead><tr valign='top'>")
    '              End If
    '            End If




    '            If DisplayLink Then
    '              htmlOut.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'>&nbsp;</th><th class='forSaleCellBorder'>&nbsp;</th>")
    '            End If

    '            If DisplayLink Then
    '              If CRMViewActive Then
    '                htmlOut.Append("<th class='forSaleCellBorder'>&nbsp;</th>")
    '              End If

    '              If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
    '                If NOTE_ID > 0 Then
    '                  htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>$</strong>")
    '                  htmlOut.Append("</th>")
    '                End If
    '                htmlOut.Append("<th class='forSaleCellBorder'>&nbsp;</td>") ' blue plus 
    '              End If
    '            End If

    '            If is_word Then
    '              htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SER<br />NUM</font></strong></th>")
    '              htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SER<br />NUM</font></strong></th>")
    '            Else
    '              htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SERIAL<br />NUMBER</font></strong></th>")
    '              htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SERIAL<br />NUMBER</font></strong></th>")
    '            End If


    '          Else

    '            If is_word Then
    '              If page_break_after > 0 Then
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1' ><thead>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1'><thead>")
    '              Else
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0'><tr>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0'><thead>")
    '              End If
    '            Else
    '              If page_break_after > 0 Then
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1'  width=""100%""><thead>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1'><tr>")
    '              Else
    '                htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0'  width=""100%""><thead>")
    '                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0'><thead>")
    '              End If
    '            End If



    '            htmlOut.Append("<tr>")
    '            htmlOut_Export.Append("<tr>")

    '            If DisplayLink Then
    '              htmlOut.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'>&nbsp;</th><th class='forSaleCellBorder'>&nbsp;</th>")
    '            End If

    '            If DisplayLink Then
    '              If NOTE_ID > 0 Then
    '                If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
    '                  htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>$</strong>")
    '                  htmlOut.Append("</th>")
    '                End If
    '              Else
    '                If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
    '                  htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>&nbsp;")
    '                  htmlOut.Append("</th>")
    '                End If
    '              End If
    '            End If

    '            If is_word Then
    '              htmlOut.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SER<br />NUM</strong></th>")
    '              htmlOut_Export.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SER<br />NUM</strong></th>")
    '            Else
    '              htmlOut.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SERIAL<br />NUMBER</strong></th>")
    '              htmlOut_Export.Append("<th align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SERIAL<br />NUMBER</strong></th>")
    '            End If

    '          End If



    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR MFR</font></strong></th>")
    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR DLV</font></strong></th>")


    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR MFR</font></strong></th>")
    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR DLV</font></strong></th>")



    '          If Not searchCriteria.ViewCriteriaIsReport Then
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "OWNER</font></strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "OWNER</font></strong></th>")

    '            If DisplayLink Then
    '              htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></th>")
    '              htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></th>")
    '            End If
    '          Else
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OWNER</strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OWNERPHONE</strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATOR</strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATORPHONE</strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKERPHONE</strong></th>")

    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OWNER</strong></h>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OWNERPHONE</strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATOR</strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATORPHONE</strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>BROKERPHONE</strong></th>")
    '          End If

    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ASKING</font></strong></th>")

    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ASKING</font></strong></th>")


    '          'Take Price Added
    '          If CRMViewActive Then
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "TAKE PRICE</font></strong></th>")
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EST VALUE</font></strong></th>")

    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "TAKE PRICE</font></strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EST VALUE</font></strong></th>")
    '          End If

    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "DATE LISTED</font></strong></th>")
    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "AFTT</font></strong></th>")
    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ENGINE&nbsp;TT</font></strong></th>")

    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "DATE LISTED</font></strong></th>")
    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "AFTT</font></strong></th>")
    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ENGINE&nbsp;TT</font></strong></th>")



    '          If DisplayLink Then
    '            'htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>FEATURES</strong><br />")
    '            'htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>FEATURES</strong><br />")


    '            'htmlOut.Append("<table id='featureHeadingTable' width='100%' cellpadding='1' cellspacing='0' border='0'><tr>")
    '            'htmlOut_Export.Append("<table id='featureHeadingTable' width='100%' cellpadding='1' cellspacing='0' border='0'><tr>")

    '            JetnetViewData.load_standard_ac_features(searchCriteria, arrStdFeatCodes)

    '            Dim sNonStandardAcFeature As String = ""
    '            JetnetViewData.display_nonstandard_feature_code_headings(searchCriteria, arrFeatCodes, arrStdFeatCodes, cellWidth, sNonStandardAcFeature)

    '            htmlOut.Append(sNonStandardAcFeature) '+ "</tr></table>")
    '            htmlOut_Export.Append(sNonStandardAcFeature) ' + "</tr></table>")

    '            'htmlOut.Append("</th>")
    '            'htmlOut_Export.Append("</th>")
    '          End If


    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder' title='Number Of Passengers'><strong>" & font_shrink & "PAX</font></strong></th>")
    '          htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "INT<br />YEAR</font></strong></th>")

    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder' title='Number Of Passengers'><strong>" & font_shrink & "PAX</font></strong></th>")
    '          htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "INT<br />YEAR</font></strong></th>")


    '          If (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName)) Then
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></th>")
    '          Else
    '            htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorderNoNotes'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></th>")
    '            htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorderNoNotes'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></th>")
    '          End If

    '          If DisplayLink Then
    '            If Not searchCriteria.ViewCriteriaIsReport Then
    '              If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName)) Or CRMViewActive = True) Then
    '                htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>NOTES</strong>")
    '                htmlOut.Append("</th>")
    '              End If
    '            Else
    '              htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'><strong>NOTES</strong>")
    '              htmlOut.Append("</th>")
    '            End If
    '          End If



    '          htmlOut.Append("</tr></thead><tbody>")
    '          htmlOut_Export.Append("</tr></thead><tbody>")

    '          start_text = htmlOut.ToString
    '          start_text_export = htmlOut_Export.ToString


    '          For Each r As DataRow In results_table.Rows


    '            ' set the ac_id for this listing
    '            searchCriteria.ViewCriteriaAircraftID = CLng(r.Item("ac_id").ToString)


    '            row_count = row_count + 1

    '            If run_market_insert = True Then

    '              Select Case UCase(r("source").ToString)
    '                Case "JETNET"

    '                  CurrentData = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(r("ac_id"), 0)
    '                  If Not IsNothing(CurrentData) Then
    '                    If CurrentData.Rows.Count > 0 Then
    '                      ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "ac")
    '                    End If
    '                  End If

    '                  ac_temp.cliaircraft_jetnet_ac_id = ac_temp.cliaircraft_id
    '                  ac_temp.cliaircraft_id = 0
    '                  aclsData_Temp.Insert_Client_Comparables(new_note_id, ac_temp, IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), ""), 0, searchCriteria.ViewCriteriaAmodID, "C")

    '                Case "CLIENT"

    '                  CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(r("ac_id"))
    '                  If Not IsNothing(CurrentData) Then
    '                    If CurrentData.Rows.Count > 0 Then
    '                      ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "cliaircraft")
    '                    End If
    '                  End If

    '                  aclsData_Temp.Insert_Client_Comparables(new_note_id, ac_temp, IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), ""), r("client_model_id"), searchCriteria.ViewCriteriaAmodID, "C")
    '              End Select
    '            End If


    '            ' only should be for spec page break 
    '            If page_break_after > 0 Then
    '              If row_count > page_break_after Then

    '                If pages_made = 1 Then ' if its the first time in, then made the page break after much larger
    '                  page_break_after = page_break_after + 10
    '                End If

    '                pages_made = pages_made + 1

    '                htmlOut.Append("</table>")  '  for the row/tables made in this function 
    '                htmlOut.Append("</td></tr></table></td></tr></table></td></tr></table>")  '  for the 3 table row columns made before this function
    '                htmlOut.Append("</table></td></tr></table>") ' for the header
    '                htmlOut.Append(Insert_Page_Break(is_word))
    '                htmlOut.Append("<table align='center' id='fleetTable'  cellpadding='1' cellspacing='0' width='95%'>")
    '                htmlOut.Append("<tr id='trInner_Content_AC_PIC'>")
    '                htmlOut.Append("<td  id='tdInner_Content_AC_PIC' align='center' colspan='3'>")

    '                htmlOut.Append(Replace(header_text, "Market Value Analysis", "Value Analysis - Market Survery (" & pages_made & ")"))
    '                ' htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module""><tr><td align=""left"" valign=""top"">")



    '                htmlOut.Append("<table id='modelForsaleViewTopTable' width=""100%"" cellpadding=""4"" cellspacing=""0""><tr><td align=""left"" valign=""top"">")

    '                htmlOut.Append(page_break_header)
    '                htmlOut.Append(start_text)
    '                row_count = 0
    '              End If
    '            End If


    '            If Not toggleRowColor Then
    '              htmlOut.Append("<tr class='alt_row'>")
    '              htmlOut_Export.Append("<tr class='alt_row'>")
    '              toggleRowColor = True
    '            Else
    '              htmlOut.Append("<tr bgcolor='white'>")
    '              htmlOut_Export.Append("<tr bgcolor='white'>")
    '              toggleRowColor = False
    '            End If

    '            If DisplayLink Then
    '              htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")


    '              If (searchCriteria.ViewCriteriaNoLocalNotes = False And Not searchCriteria.ViewCriteriaIsReport) Then

    '                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")  ' Note ICON
    '                htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a href='javascript:displayLocalAircraftNoteJS(" + r.Item("ac_id").ToString + ",0,0);'><img src='images/Notes.gif' border='0'></a></div>")
    '                htmlOut.Append("</td>")

    '              ElseIf (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) And Not searchCriteria.ViewCriteriaIsReport) Then

    '                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")  ' Note ICON
    '                htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a class='underline' onclick='javascript:callNoteViewImg" + r.Item("ac_id").ToString + "();'><img src='images/Notes.gif' border='0'></a></div>")
    '                htmlOut.Append("</td>")

    '              Else

    '                If Not searchCriteria.ViewCriteriaIsReport Then
    '                  htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;")  ' NO NOTES No Note ICON
    '                  htmlOut.Append("</td>")
    '                End If

    '              End If
    '            End If

    '            If DisplayLink Then
    '              If CRMViewActive Then
    '                htmlOut.Append("<td class='forSaleCellBorder'>")

    '                If NOTE_ID > 0 Then
    '                  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
    '                Else
    '                  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
    '                End If

    '                htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
    '                htmlOut.Append("</a>")
    '                htmlOut.Append("</td>")
    '              End If
    '            End If

    '            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '            'OWNER LOOKUP MOVED TO BEFORE NOTES ICON SO QUERY HAD TO BE DONE ONLY ONCE.
    '            searchCriteria.ViewCriteriaGetExclusive = False
    '            searchCriteria.ViewCriteriaGetOperator = False

    '            Dim ownerDataTable As New DataTable

    '            Select Case UCase(r("source").ToString)
    '              Case "JETNET"
    '                ownerDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
    '              Case "CLIENT"
    '                ownerDataTable = Get_Client_Owner_Info(searchCriteria, 0)
    '            End Select


    '            If DisplayLink Then

    '              If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
    '                If NOTE_ID > 0 Then
    '                  htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' PROSPECTS

    '                  'This appends the notes on the table.
    '                  htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "F", REAL_AC_ID, 0, COMPLETED_DATE, 0))

    '                  htmlOut.Append("</td>")
    '                End If

    '                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' NOTE ADD 
    '                If Not IsNothing(ownerDataTable) Then
    '                  If ownerDataTable.Rows.Count > 0 Then
    '                    Dim TemporaryCompanyID As Long = 0
    '                    Dim CheckNoteTable As New DataTable

    '                    htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit.aspx?prospectACID=" & IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")) & "&comp_ID=")

    '                    'Need to send jetnet company ID
    '                    If UCase(r("source")) = "JETNET" Then
    '                      htmlOut.Append(ownerDataTable.Rows(0).Item("comp_id"))
    '                      TemporaryCompanyID = ownerDataTable.Rows(0).Item("comp_id")
    '                    Else
    '                      htmlOut.Append(ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id"))
    '                      TemporaryCompanyID = ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id")
    '                    End If

    '                    htmlOut.Append("&source=JETNET&type=company&action=checkforcreation&note_type=A&from=view&rememberTab=" & ActiveTabIndex & "&returnView=" & searchCriteria.ViewID & IIf(NOTE_ID > 0, "&NoteID=" & NOTE_ID, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">")


    '                    HTML_NOTE = CheckForNotesForSaleTab(CRMViewActive, r.Item("source").ToString, r.Item("ac_id"), aclsData_Temp)

    '                    If HTML_NOTE = "" Then
    '                      htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
    '                    Else
    '                      htmlOut.Append(Replace(HTML_NOTE, "images/document.png", "images/note_pin_add.png"))
    '                    End If


    '                    htmlOut.Append("</a>")
    '                  Else
    '                    Dim CheckNoteTable As New DataTable

    '                    htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit_note.aspx?source=JETNET&from=view&ac_ID=" & IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")) & "&type=note&action=new&ViewID=19&refreshing=prospect&rememberTab=" & ActiveTabIndex & "&NoteID=" & NOTE_ID & "');"">")

    '                    HTML_NOTE = CheckForNotesForSaleTab(CRMViewActive, r.Item("source").ToString, r.Item("ac_id"), aclsData_Temp)

    '                    If html_NOTE = "" Then
    '                      htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
    '                    Else
    '                      htmlOut.Append(Replace(HTML_NOTE, "images/document.png", "images/note_pin_add.png"))
    '                    End If


    '                    htmlOut.Append("</a>")

    '                  End If
    '                End If
    '                htmlOut.Append("</td>")

    '              End If
    '              ' End If
    '            End If



    '            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")  ' SERIAL NUMBER

    '            htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")  ' SERIAL NUMBER


    '            If (Not searchCriteria.ViewCriteriaIsReport And DisplayLink) Or DisplayLink Then
    '              If Not IsDBNull(r("ac_ser_no_full")) Then

    '                If r.Item("source").ToString = "JETNET" Then
    '                  htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
    '                Else
    '                  Dim JetnetForSaleCheck As New DataTable
    '                  Dim NotForSaleJetnetSide As Boolean = False
    '                  'This is where we need to add a check for client off market aircraft. 
    '                  'On both the market summary view and the value view need to have a way of showing that an aircraft is an off market.
    '                  'Recommend the following: on display of every client record in the listing check to see if there is a 
    '                  'corresponding jetnet for sale record 
    '                  '(select count(*) from aircraft where ac_id = #### and ac_journ_id = 0 and ac_forsale_flag=’Y’), 
    '                  'if not then color the serial number red and bold it and modify the alt tag/mouseover to read as 
    '                  '“Display Aircraft Details: JETNET shows this aircraft as off market.
    '                  JetnetForSaleCheck = JetnetViewData.Check_Jetnet_Off_Market_Aircraft(r.Item("client_jetnet_ac_id"))
    '                  If Not IsNothing(JetnetForSaleCheck) Then
    '                    If JetnetForSaleCheck.Rows.Count > 0 Then
    '                      If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
    '                        NotForSaleJetnetSide = True
    '                      End If
    '                    End If
    '                  End If

    '                  htmlOut.Append("<a onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0&source=" & r.Item("source").ToString & """,""AircraftDetails"");'")

    '                  If NotForSaleJetnetSide Then
    '                    htmlOut.Append(" class='underline red_text' title='Display Aircraft Details: JETNET shows this aircraft as off market.'>")
    '                  Else
    '                    htmlOut.Append(" class='underline' title='Display Aircraft Details'>")
    '                  End If

    '                End If



    '                htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

    '                htmlOut_Export.Append(r.Item("ac_ser_no_full").ToString + "</a>")

    '              Else
    '                htmlOut.Append("&nbsp;")
    '              End If
    '            Else

    '              If Not IsDBNull(r("ac_ser_no_full")) Then
    '                htmlOut.Append(font_shrink & "" & r.Item("ac_ser_no_full").ToString & "</font>")
    '                htmlOut_Export.Append(font_shrink & "" & r.Item("ac_ser_no_full").ToString & "</font>")
    '              Else
    '                htmlOut.Append("&nbsp;")
    '                htmlOut_Export.Append("&nbsp;")
    '              End If


    '            End If





    '            htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR MFG
    '            htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR MFG

    '            If Not IsDBNull(r("ac_mfr_year")) Then
    '              If Not String.IsNullOrEmpty(r.Item("ac_mfr_year")) Then
    '                If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
    '                  htmlOut.Append("0")
    '                  htmlOut_Export.Append("0")
    '                Else
    '                  htmlOut.Append(r.Item("ac_mfr_year").ToString)
    '                  htmlOut_Export.Append(r.Item("ac_mfr_year").ToString)
    '                End If
    '              End If
    '            Else
    '              htmlOut.Append("U")
    '              htmlOut_Export.Append("U")
    '            End If

    '            htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR DLV
    '            htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR DLV

    '            If Not IsDBNull(r("ac_year")) Then
    '              If Not String.IsNullOrEmpty(r.Item("ac_year")) Then
    '                If CDbl(r.Item("ac_year").ToString) = 0 Then
    '                  htmlOut.Append("0")
    '                  htmlOut_Export.Append("0")
    '                Else
    '                  htmlOut.Append(r.Item("ac_year").ToString)
    '                  htmlOut_Export.Append(r.Item("ac_year").ToString)
    '                End If
    '              End If
    '            Else
    '              htmlOut.Append("U")
    '              htmlOut_Export.Append("U")
    '            End If

    '            If DisplayLink Then
    '              htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OWNER

    '              htmlOut_Export.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' >" & font_shrink) ' OWNER
    '            Else
    '              htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' >" & font_shrink) ' OWNER
    '            End If

    '            'Owner table has been moved up above the notes icon. So it doesn't have to be ran twice.
    '            If Not IsNothing(ownerDataTable) Then

    '              If ownerDataTable.Rows.Count > 0 Then
    '                For Each vr_owner As DataRow In ownerDataTable.Rows

    '                  Select Case UCase(r("source").ToString)
    '                    Case "JETNET"
    '                      sCompanyPhone = ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), vr_owner("comp_phone_fax"))
    '                    Case "CLIENT"
    '                      sCompanyPhone = ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(vr_owner.Item("comp_id").ToString), True)
    '                  End Select

    '                  If String.IsNullOrEmpty(sCompanyPhone) Then
    '                    sCompanyPhone = "Not listed"
    '                  End If

    '                  If Not searchCriteria.ViewCriteriaIsReport And DisplayLink Then
    '                    If r.Item("source").ToString = "JETNET" Then
    '                      htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
    '                    Else
    '                      htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
    '                    End If

    '                    htmlOut.Append(" title='PH : " + sCompanyPhone + "'>" + vr_owner.Item("comp_name").ToString.Trim + "</a>")
    '                    htmlOut_Export.Append("" + font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font>") ' OWNER
    '                  Else

    '                    If is_word Then
    '                      htmlOut.Append(font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font>") ' OWNER 
    '                    Else
    '                      htmlOut.Append(font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font>") ' OWNER 
    '                    End If


    '                    If DisplayLink Then
    '                      htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' OWNERPHONE  
    '                    End If
    '                  End If
    '                Next
    '              Else
    '                If Not searchCriteria.ViewCriteriaIsReport Then
    '                  htmlOut.Append("None")
    '                  htmlOut_Export.Append("None")
    '                Else
    '                  htmlOut.Append("None</td>") ' OWNER
    '                  htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE

    '                  htmlOut_Export.Append("None</td>") ' OWNER
    '                  htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE
    '                End If
    '              End If
    '            Else
    '              If Not searchCriteria.ViewCriteriaIsReport Then
    '                htmlOut.Append("None")
    '                htmlOut_Export.Append("None")
    '              Else
    '                htmlOut.Append("None</td>") ' OWNER
    '                htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE  

    '                htmlOut_Export.Append("None</td>") ' OWNER
    '                htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE  
    '              End If
    '            End If

    '            ownerDataTable = Nothing

    '            If searchCriteria.ViewCriteriaIsReport Then

    '              searchCriteria.ViewCriteriaGetExclusive = False
    '              searchCriteria.ViewCriteriaGetOperator = True

    '              Dim operatorDataTable As New DataTable

    '              Select Case UCase(r("source").ToString)
    '                Case "JETNET"
    '                  operatorDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
    '                Case "CLIENT"
    '                  operatorDataTable = Get_Client_Owner_Info(searchCriteria, 0)
    '              End Select


    '              If Not IsNothing(operatorDataTable) Then

    '                If operatorDataTable.Rows.Count > 0 Then
    '                  For Each r_operator As DataRow In operatorDataTable.Rows
    '                    sCompanyPhone = ""
    '                    htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                    htmlOut.Append(r_operator.Item("comp_name").ToString.Trim + "</td>")
    '                    htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

    '                    htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                    htmlOut_Export.Append(r_operator.Item("comp_name").ToString.Trim + "</td>")
    '                    htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

    '                    Select Case UCase(r("source").ToString)
    '                      Case "JETNET"
    '                        sCompanyPhone = ReturnCompanyPhoneFax(r_operator("comp_phone_office"), r_operator("comp_phone_fax"))
    '                      Case "CLIENT"
    '                        sCompanyPhone = ReturnCompanyPhoneFax(r_operator("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(r_operator.Item("comp_id").ToString), True)
    '                    End Select

    '                    htmlOut.Append(sCompanyPhone)
    '                    htmlOut_Export.Append(sCompanyPhone)
    '                    '+ 
    '                  Next
    '                Else
    '                  htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                  htmlOut.Append("None</td>")
    '                  htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE  

    '                  htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                  htmlOut_Export.Append("None</td>")
    '                  htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE   
    '                End If
    '              Else
    '                htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                htmlOut.Append("None</td>")
    '                htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE 

    '                htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
    '                htmlOut_Export.Append("None</td>")
    '                htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE 
    '              End If

    '              operatorDataTable = Nothing

    '            End If



    '            If DisplayLink Then
    '              htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' BROKER
    '              htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' BROKER

    '              searchCriteria.ViewCriteriaGetExclusive = True
    '              searchCriteria.ViewCriteriaGetOperator = False

    '              Dim exclusiveDataTable As New DataTable

    '              'We only need to try filling this up if the aircraft Exclusive flag is Y.
    '              If r("ac_exclusive_flag") = "Y" Then
    '                Select Case UCase(r("source").ToString)
    '                  Case "JETNET"
    '                    exclusiveDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
    '                  Case "CLIENT"
    '                    exclusiveDataTable = Get_Client_Owner_Info(searchCriteria, 0)
    '                End Select
    '              End If

    '              If Not IsNothing(exclusiveDataTable) Then

    '                If exclusiveDataTable.Rows.Count > 0 Then
    '                  For Each vr_exclusive As DataRow In exclusiveDataTable.Rows

    '                    Select Case UCase(r("source").ToString)
    '                      Case "JETNET"
    '                        sCompanyPhone = ReturnCompanyPhoneFax(vr_exclusive("comp_phone_office"), vr_exclusive("comp_phone_fax"))
    '                      Case "CLIENT"
    '                        sCompanyPhone = ReturnCompanyPhoneFax(vr_exclusive("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(vr_exclusive.Item("comp_id").ToString), True)
    '                    End Select


    '                    If String.IsNullOrEmpty(sCompanyPhone) Then
    '                      sCompanyPhone = "Not listed"
    '                    End If

    '                    If Not searchCriteria.ViewCriteriaIsReport Then
    '                      If r.Item("source").ToString = "JETNET" Then
    '                        htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
    '                        'htmlOut_Export.Append("<strong>")
    '                      Else
    '                        htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
    '                        'htmlOut_Export.Append("<strong>")
    '                      End If

    '                      ' htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
    '                      htmlOut.Append(" title='PH : " + sCompanyPhone + "'><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></a>")
    '                      htmlOut_Export.Append("" + vr_exclusive.Item("comp_name").ToString.Trim)
    '                    Else
    '                      htmlOut.Append("<font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></td>")
    '                      htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' BROKERPHONE  
    '                      htmlOut_Export.Append("<font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></td>")
    '                      htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' BROKERPHONE  
    '                    End If
    '                  Next
    '                Else
    '                  If Not searchCriteria.ViewCriteriaIsReport Then
    '                    htmlOut.Append("None")
    '                    htmlOut_Export.Append("None")
    '                  Else
    '                    htmlOut.Append("None</td>")
    '                    htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
    '                    htmlOut_Export.Append("None</td>")
    '                    htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
    '                  End If
    '                End If
    '              Else
    '                If Not searchCriteria.ViewCriteriaIsReport Then
    '                  htmlOut.Append("None")
    '                  htmlOut_Export.Append("None")
    '                Else
    '                  htmlOut.Append("None</td>")
    '                  htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
    '                  htmlOut_Export.Append("None</td>")
    '                  htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
    '                End If
    '              End If

    '              exclusiveDataTable = Nothing
    '            End If



    '            htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" & font_shrink) ' ASKING
    '            htmlOut_Export.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" & font_shrink) ' ASKING

    '            'bHadStatus = False
    '            'If Not IsDBNull(r("ac_Status")) Then
    '            '    If Not String.IsNullOrEmpty(r.Item("ac_Status").ToString) Then
    '            '        If r.Item("ac_Status").ToString.ToLower.Trim.Contains("for sale") Then
    '            '            'htmlOut.Append(JetnetViewData.forsale_status(r.Item("ac_Status").ToString.Trim))
    '            '            ' bHadStatus = True
    '            '        End If
    '            '    End If
    '            'End If

    '            'If bHadStatus Then
    '            '    htmlOut.Append("&nbsp;")
    '            'End If




    '            If Not IsDBNull(r("ac_asking")) Then
    '              If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
    '                If Not IsDBNull(r("ac_asking_price")) Then
    '                  If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
    '                    htmlOut.Append("$" + (CDbl(r.Item("ac_asking_price").ToString) / 1000).ToString + "k")
    '                    htmlOut_Export.Append("$" + (CDbl(r.Item("ac_asking_price").ToString) / 1000).ToString + "k")
    '                  End If
    '                End If
    '              Else
    '                htmlOut.Append(JetnetViewData.forsale_status(r.Item("ac_asking").ToString.Trim))
    '                htmlOut_Export.Append(JetnetViewData.forsale_status(r.Item("ac_asking").ToString.Trim))
    '              End If
    '            End If

    '            htmlOut.Append("&nbsp;</td>")
    '            htmlOut_Export.Append("&nbsp;</td>")



    '            'Take Price Added 
    '            If CRMViewActive Then
    '              htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
    '              htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
    '              If Not IsDBNull(r("ac_take_price")) Then
    '                If CDbl(r.Item("ac_take_price").ToString) > 0 Then
    '                  htmlOut.Append("$" + (CDbl(r.Item("ac_take_price").ToString) / 1000).ToString + "k")
    '                  htmlOut_Export.Append("$" + (CDbl(r.Item("ac_take_price").ToString) / 1000).ToString + "k")
    '                End If
    '              End If
    '              htmlOut.Append("</font></td>")
    '              htmlOut_Export.Append("</font></td>")
    '            End If


    '            'sold_price  Added 
    '            If CRMViewActive Then
    '              htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
    '              htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
    '              If Not IsDBNull(r("sold_price")) Then
    '                If CDbl(r.Item("sold_price").ToString) > 0 Then
    '                  htmlOut.Append("$" + (CDbl(r.Item("sold_price").ToString) / 1000).ToString + "k")
    '                  htmlOut_Export.Append("$" + (CDbl(r.Item("sold_price").ToString) / 1000).ToString + "k")
    '                End If
    '              End If
    '              htmlOut.Append("</font></td>")
    '              htmlOut_Export.Append("</font></td>")
    '            End If

    '            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AC LIST DATE
    '            htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AC LIST DATE

    '            If Not IsDBNull(r.Item("ac_list_date")) Then
    '              If IsDate(r.Item("ac_list_date").ToString) Then
    '                htmlOut.Append(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate))
    '                htmlOut_Export.Append(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate))
    '              Else
    '                htmlOut.Append("&nbsp;")
    '                htmlOut_Export.Append("&nbsp;")
    '              End If
    '            Else
    '              htmlOut.Append("&nbsp;")
    '              htmlOut_Export.Append("&nbsp;")
    '            End If

    '            htmlOut.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AFTT
    '            htmlOut_Export.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AFTT


    '            If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
    '              If CDbl(r.Item("ac_airframe_tot_hrs").ToString) = 0 Then
    '                htmlOut.Append("0")
    '                htmlOut_Export.Append("0")
    '              Else
    '                htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString)
    '                htmlOut_Export.Append(r.Item("ac_airframe_tot_hrs").ToString)
    '              End If
    '            Else
    '              htmlOut.Append("U")
    '              htmlOut_Export.Append("U")
    '            End If

    '            htmlOut.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' Engine Times
    '            htmlOut_Export.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' Engine Times

    '            If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
    '              If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
    '                htmlOut.Append("[0]&nbsp;")
    '                htmlOut_Export.Append("[0]&nbsp;")
    '              Else
    '                htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
    '                htmlOut_Export.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
    '              End If
    '            Else
    '              htmlOut.Append("[U]&nbsp;")
    '              htmlOut_Export.Append("[U]&nbsp;")
    '            End If

    '            If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
    '              If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
    '                htmlOut.Append("[0]&nbsp;")
    '                htmlOut_Export.Append("[0]&nbsp;")
    '              Else
    '                htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
    '                htmlOut_Export.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
    '              End If
    '            Else
    '              htmlOut.Append("[U]&nbsp;")
    '              htmlOut_Export.Append("[U]&nbsp;")
    '            End If

    '            If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
    '              If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
    '                htmlOut.Append("[0]&nbsp;")
    '                htmlOut_Export.Append("[0]&nbsp;")
    '              Else
    '                htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
    '                htmlOut_Export.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
    '              End If
    '            End If

    '            If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
    '              If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
    '                htmlOut.Append("[0]&nbsp;")
    '                htmlOut_Export.Append("[0]&nbsp;")
    '              Else
    '                htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
    '                htmlOut_Export.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
    '              End If
    '            End If



    '            If DisplayLink Then
    '              htmlOut.Append("</font></td>") '<td align='center' valign='middle' class='forSaleCellBorder'>") ' Feature Codes
    '              htmlOut_Export.Append("</font></td>") '<td align='center' valign='middle' class='forSaleCellBorder'>") ' Feature Codes

    '              Dim sAcFeatureCodes As String = ""
    '              '''''''''''''''''''''''''''''''''''''''''''

    '              If Not IsDBNull(r.Item("source").ToString) Then
    '                If Trim(r.Item("source").ToString) = "CLIENT" Then
    '                  JetnetViewData.display_client_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
    '                Else
    '                  JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
    '                End If
    '              Else
    '                JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
    '              End If


    '              htmlOut.Append(sAcFeatureCodes)

    '              sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "height='15'", "")
    '              sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "vertical-align: middle;", "")
    '              sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "'>No features", "' colspan='4'>No features")


    '              htmlOut_Export.Append(sAcFeatureCodes)
    '            End If



    '            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' PASSENGERS
    '            htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)

    '            If Not IsDBNull(r("ac_passenger_count")) Then
    '              If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
    '                htmlOut.Append("0&nbsp;")
    '                htmlOut_Export.Append("0&nbsp;")
    '              Else
    '                htmlOut.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
    '                htmlOut_Export.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
    '              End If
    '            Else
    '              htmlOut.Append("U&nbsp;")
    '              htmlOut_Export.Append("U&nbsp;")
    '            End If

    '            htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' INT YEAR
    '            htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)

    '            If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
    '              htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)
    '              htmlOut_Export.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

    '              If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
    '                htmlOut.Append("/")
    '                htmlOut_Export.Append("/")
    '              End If
    '              htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
    '              htmlOut_Export.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
    '            Else
    '              htmlOut.Append("&nbsp;")
    '              htmlOut_Export.Append("&nbsp;")
    '            End If



    '            'If HttpContext.Current.Session.Item("localPreferences").HasLocalNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").NotesDatabaseName) And searchCriteria.ViewCriteriaNoLocalNotes = False Then
    '            ' htmlOut.Append("</td><td align='center' valign='middle' class='forSaleCellBorder'>") ' EXT YEAR
    '            '  Else
    '            htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorderNoNotes'>" & font_shrink) ' EXT YEAR
    '            htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorderNoNotes'>" & font_shrink) ' EXT YEAR

    '            '   End If

    '            If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
    '              htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
    '              htmlOut_Export.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
    '              If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
    '                htmlOut.Append("/")
    '                htmlOut_Export.Append("/")
    '              End If
    '              htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
    '              htmlOut_Export.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
    '            Else
    '              htmlOut.Append("&nbsp;")
    '              htmlOut_Export.Append("&nbsp;")
    '            End If

    '            If DisplayLink Then
    '              If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

    '                htmlOut.Append("</td><td align='center' valign='middle' class='forSaleCellBorder' title='Most Recent Local Note'>") ' NOTES

    '                'This appends the notes on the table.
    '                htmlOut.Append(HTML_NOTE)

    '              End If
    '            End If

    '            htmlOut.Append("</font></td></tr>")
    '            htmlOut_Export.Append("</font></td></tr>")
    '          Next

    '        Else
    '          htmlOut.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
    '          htmlOut_Export.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
    '        End If

    '      Else
    '        htmlOut.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
    '        htmlOut_Export.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
    '      End If

    '    Else
    '      '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
    '      '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
    '      '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
    '      'This grabs the data for the jetnet aircraft for sale
    '      JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string)

    '      'This grabs the client side aircraft for sale
    '      clientTable = get_client_model_forsale_info(searchCriteria, client_string, order_by_string)

    '      'This takes those two datatables, excludes the jetnet ones we have client aircraft for, adds the extra fields to make
    '      'The schemas match and then merges them into results table.
    '      FullClientIDstoExclude = ""
    '      results_table = ModifyAndCombineJetnetClientDataForSale(clientTable, JetnetTable, searchCriteria, FullClientIDstoExclude, order_by_string, "N")

    '      ' If Not IsNothing(table_to_add) Then
    '      'table_to_add = results_table
    '      ' End If


    '      order_by_string_break = Split(order_by_string, ",")

    '      db_fields_names = Split(fields_name, ",")

    '      type_string_names = Split(type_string, ",")

    '      size_string_names = Split(size_string, ",")

    '      htmlOut.Append("<table id='forSaleInnerTable' cellpadding='4' cellspacing='0' border='1'   width=""100%"">")
    '      htmlOut.Append("<tr bgcolor='#CCCCCC'>")

    '      htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='4' cellspacing='0' border='1'>")
    '      htmlOut_Export.Append("<tr bgcolor='#CCCCCC'>")


    '      If DisplayLink Then ' dont display these for export to excel
    '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;</td>")
    '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;</td>")
    '        If NOTE_ID > 0 Then
    '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;</td>")
    '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;</td>") ' blue plus 
    '        End If
    '      End If

    '      For i = 0 To order_by_string_break.Length - 1
    '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")
    '        htmlOut.Append(Trim(Replace(order_by_string_break(i), "'", "")) & "")
    '        htmlOut.Append("</td>")

    '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")
    '        htmlOut_Export.Append(Trim(Replace(order_by_string_break(i), "'", "")) & "")
    '        htmlOut_Export.Append("</td>")
    '      Next
    '      htmlOut.Append("</tr>")
    '      htmlOut_Export.Append("</tr>")

    '      ' clientTable to be changed to combined one later
    '      If Not IsNothing(results_table) Then
    '        If results_table.Rows.Count > 0 Then
    '          For Each r As DataRow In results_table.Rows

    '            If Not toggleRowColor Then
    '              htmlOut.Append("<tr class='alt_row'>")
    '              htmlOut_Export.Append("<tr class='alt_row'>")
    '              toggleRowColor = True
    '            Else
    '              htmlOut.Append("<tr bgcolor='white'>")
    '              htmlOut_Export.Append("<tr bgcolor='white'>")
    '              toggleRowColor = False
    '            End If

    '            For i = 0 To order_by_string_break.Length - 1

    '              If i = 0 And DisplayLink = True Then
    '                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")

    '                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

    '                If NOTE_ID > 0 Then
    '                  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
    '                Else
    '                  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
    '                End If

    '                htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
    '                htmlOut.Append("</a>")
    '                htmlOut.Append("</td>")

    '                If NOTE_ID > 0 Then
    '                  If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

    '                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' PROSPECTS

    '                    'This appends the notes on the table.
    '                    htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "F", REAL_AC_ID, 0, COMPLETED_DATE, 0))

    '                    htmlOut.Append("</td>")

    '                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' NOTE ADD 
    '                    htmlOut.Append("<A href='#' title='Add Note' alt='Add Note'><img src='images/blue_plus_sign.png' width='16'></a>")
    '                    htmlOut.Append("</td>")
    '                  End If
    '                End If
    '              End If


    '              format_me = True

    '              temp_field = Trim(Replace(order_by_string_break(i), "'", ""))
    '              temp_field_name = Trim(Replace(db_fields_names(i), "'", ""))

    '              temp_type = Trim(Replace(type_string_names(i), "'", ""))
    '              temp_size = Trim(Replace(size_string_names(i), "'", ""))

    '              If Not IsDBNull(r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")) Then
    '                temp_val = r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")
    '              Else
    '                temp_val = ""
    '              End If

    '              If Trim(temp_type) <> "" Then
    '                If Trim(temp_type) = "String" Then

    '                ElseIf Trim(temp_type) = "Char" Then

    '                ElseIf Trim(temp_type) = "Value" Then
    '                  temp_val = FormatNumber(temp_val, 0)
    '                ElseIf Trim(temp_type) = "Date" Then

    '                Else

    '                End If
    '              End If

    '              'Else
    '              '  If Trim(temp_field_name) = "cliaircraft_ser_nbr" Then
    '              '    format_me = False
    '              '  ElseIf Trim(temp_field_name) = "cliaircraft_reg_nbr" Then
    '              '    format_me = False
    '              '  ElseIf Trim(temp_field_name) = "cliamod_make_name" Then
    '              '    format_me = False
    '              '  ElseIf Trim(temp_field_name) = "cliamod_model_name" Then
    '              '    format_me = False
    '              '  ElseIf Trim(temp_field_name) = "cliaircraft_year_mfr" Then
    '              '    format_me = False
    '              '  End If
    '              'End If




    '              'If IsNumeric(temp_field) And (format_me) Then
    '              If Trim(temp_type) = "Value" Then
    '                htmlOut.Append("<td align='right'>")
    '                htmlOut_Export.Append("<td align='right'>")
    '              Else
    '                htmlOut.Append("<td align='left'>")
    '                htmlOut_Export.Append("<td align='left'>")
    '              End If


    '              If DisplayLink = True Then
    '                If Trim(temp_field_name) = "cliaircraft_ser_nbr" Then
    '                  If r.Item("source").ToString = "JETNET" Then
    '                    htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
    '                  Else
    '                    htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("client_jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
    '                  End If
    '                End If
    '              End If

    '              'If Trim(temp_field_name) = "cliaircraft_asking_price" Or Trim(temp_field_name) = "cliaircraft_est_price" Or Trim(temp_field_name) = "cliaircraft_broker_price" Then
    '              If Trim(temp_type) = "Value" Then
    '                If IsNumeric(temp_val) Then
    '                  If CDbl(temp_val) > 0 Then
    '                    htmlOut.Append("$")
    '                    htmlOut_Export.Append("$")
    '                  End If
    '                End If
    '              End If

    '              'If IsNumeric(temp_val) And (format_me) Then
    '              If Trim(temp_type) = "Value" Then
    '                If CDbl(temp_val) > 0 Then
    '                  htmlOut.Append(FormatNumber(temp_val, 0))
    '                  htmlOut_Export.Append(FormatNumber(temp_val, 0))
    '                End If
    '              Else
    '                htmlOut.Append(temp_val)
    '                htmlOut_Export.Append(temp_val)
    '              End If


    '              If Trim(temp_field_name) = "cliaircraft_ser_nbr" And DisplayLink = True Then
    '                htmlOut.Append("</a>")
    '              End If

    '              htmlOut.Append("&nbsp;</td>")
    '              htmlOut_Export.Append("&nbsp;</td>")

    '            Next
    '            htmlOut.Append("</tr>")
    '            htmlOut_Export.Append("</tr>")
    '          Next
    '        End If
    '      End If


    '    End If

    '    htmlOut.Append("</tbody></table>")
    '    htmlOut_Export.Append("</tbody></table>")

    '    If DisplayLink Or Trim(order_by_string) = "" Then
    '      strOut.Append("<tr valign='top'><td>" + htmlOut.ToString() + "</td></tr></table>")

    '      strOut_Export.Append("<tr valign='top'><td>" + htmlOut_Export.ToString() + "</td></tr></table>")
    '    Else
    '      strOut.Append("" + htmlOut.ToString() + "")
    '    End If


    '    string_for_export = strOut_Export.ToString
    '    searchCriteria.ViewCriteriaIsReport = orig_view

    '  Catch ex As Exception

    '    class_error = "Error in views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    '  Finally

    '  End Try


    '  out_htmlString = strOut.ToString
    '  htmlOut = Nothing
    '  strOut = Nothing
    '  results_table = Nothing

    'End Sub

    ''' <summary>
    ''' I'm doing this on purpose.
    ''' I need to edit this function, and I need to heavily edit it to add CRM capability.
    ''' It is my hope and intention to edit this function by making a copy of the one in the ViewsDataLayer
    ''' And then go ahead and replace the old one, that way there's not two of the same function
    ''' However since we have to keep old cability and I'm breaking this one, there's a need for two.
    ''' </summary>
    ''' <param name="searchCriteria"></param>
    ''' <param name="out_htmlString"></param>
    ''' <param name="is_extra_criteria"></param>
    ''' <param name="JetnetViewData"></param>
    ''' <param name="CRMViewActive"></param>
    ''' <param name="FullClientIDstoExclude"></param>
    ''' <remarks></remarks>
    Public Shared Sub views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String,
                                                     ByVal is_extra_criteria As Boolean, ByRef JetnetViewData As viewsDataLayer,
                                                     ByRef CRMViewActive As Boolean, ByRef FullClientIDstoExclude As String,
                                                     ByVal DisplayLink As Boolean, ByRef table_to_add As DataTable,
                                                     Optional ByVal NOTE_ID As Long = 0, Optional ByVal REAL_AC_ID As Long = 0,
                                                     Optional ByVal COMPLETED_DATE As String = "", Optional ByVal page_break_after As Integer = 0,
                                                     Optional ByVal header_text As String = "", Optional ByVal is_word As Boolean = True,
                                                     Optional ByVal jetnet_string As String = "", Optional ByVal client_string As String = "",
                                                     Optional ByVal order_by_string As String = "", Optional ByVal fields_name As String = "",
                                                     Optional ByVal type_string As String = "", Optional ByVal size_string As String = "",
                                                     Optional ByVal page_break_header As String = "", Optional ByRef string_for_export As String = "",
                                                     Optional ByRef ActiveTabIndex As Long = 0, Optional ByVal run_market_insert As Boolean = False,
                                                     Optional ByVal CurrentForSaleIDs As String = "", Optional ByVal runModelOnly As Boolean = False,
                                                     Optional ByRef PassBackDataTable As DataTable = Nothing, Optional ByVal displayEValues As Boolean = False)
        Dim aclsData_Temp As New clsData_Manager_SQL
        Dim arrFeatCodes() As String = Nothing
        Dim arrStdFeatCodes(,) As String = Nothing

        Dim strOut As New StringBuilder
        Dim htmlOut As New StringBuilder
        Dim HTML_NOTE As String = ""

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False


        Dim bHadStatus As Boolean = False
        Dim cellWidth As Integer = 20

        Dim nFeatureCountForSpan As Integer = 0
        Dim table_height As Integer = 0
        Dim sCompanyPhone As String = ""
        Dim orig_view As Boolean = False
        Dim font_shrink As String = ""
        Dim start_text As String = ""
        Dim row_count As Integer = 0
        Dim pages_made As Integer = 1
        Dim order_by_string_break() As String
        Dim db_fields_names() As String

        Dim temp_field As String = ""
        Dim temp_val As String = ""
        Dim format_me As Boolean = False
        Dim temp_field_name As String = ""
        Dim type_string_names() As String
        Dim size_string_names() As String

        Dim temp_type As String = ""
        Dim temp_size As String = ""

        Dim strOut_Export As New StringBuilder
        Dim start_text_export As String = ""
        Dim new_note_id As Long = 0
        Dim aclsLocal_Notes As New clsLocal_Notes
        Dim VariantListStr As String = ""
        Dim t_ask As Long = 0
        Dim t_take As Long = 0
        Dim t_sold As Long = 0


        Dim ac_temp As New clsClient_Aircraft

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing
        Dim JetnetTable As New DataTable 'Table that stores Jetnet Data.
        Dim clientTable As New DataTable 'Table that stores Client Data.

        Dim CurrentData As New DataTable
        Dim JetnetExtraCriteria As String = ""
        Dim ClientExtraCriteria As String = ""

        Dim delete_string As String = ""
        Dim arrayOfIDs() As String
        ReDim arrayOfIDs(0)
        arrayOfIDs(0) = ""
        Dim arrCounter As Integer = 0
        Dim temp_last_price As Long = 0
        Dim temp_last_date As String = ""
        Dim htmlOut_trans As String = ""
        Dim use_looked_up As Boolean = False
        Dim font_text As String = "<font size='-3'>"
        Dim efont_text As String = "</font>"

        If DisplayLink = True Then
            font_text = ""
            efont_text = ""
        Else
            font_text = "<font size='-2'>"
            efont_text = "</font>"
        End If


        If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
            For i = 0 To searchCriteria.ViewCriteriaAmodIDArray.Length - 1
                If Trim(VariantListStr) <> "" Then
                    VariantListStr += ", "
                End If
                VariantListStr += searchCriteria.ViewCriteriaAmodIDArray(i).ToString
            Next
        End If

        orig_view = searchCriteria.ViewCriteriaIsReport
        searchCriteria.ViewCriteriaIsReport = is_extra_criteria

        'Setting up the CRM/Evolution Connections to clsDataManager
        'If CRMViewActive Then
        '  ''Set up connection
        '  'aclsData_Temp.client_DB = HttpContext.Current.Application.Item("crmClientDatabase")
        '  'aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
        'Else
        'Setting up with the correct connections.
        aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
        'End If

        'If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
        '  If aclsData_Temp.client_DB = "" Then
        '    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
        '  End If
        'End If

        Try

            'Setting up array if needed of IDs to include
            Dim JetnetACIDsToInclude As String = ""
            Dim ClientACIDsToInclude As String = ""

            If CurrentForSaleIDs <> "" Then 'This is the only time we really care about filling these in.
                'They should originate from a textbox on the view that gets filled in as the datatable control fills it up/removes it
                Dim BreakableIDArray As Array
                BreakableIDArray = Split(CurrentForSaleIDs, ",")
                If UBound(BreakableIDArray) > 0 Then
                    For BreakableIDArrayCount = 0 To UBound(BreakableIDArray)
                        'This means that we have some pairs. 
                        'Since we're only ever going to be expecting |CLIENT or |JETNET, we don't need to split them into another array. 
                        'Let's just remove spaces:
                        Dim TemporaryHoldingID As String = BreakableIDArray(BreakableIDArrayCount)
                        TemporaryHoldingID = UCase(Replace(TemporaryHoldingID, " ", ""))
                        If InStr(TemporaryHoldingID, "|JETNET") Then 'This is a jetnet ID:
                            TemporaryHoldingID = Replace(TemporaryHoldingID, "|JETNET", "")
                            If IsNumeric(TemporaryHoldingID) Then
                                If JetnetACIDsToInclude <> "" Then
                                    JetnetACIDsToInclude += ", "
                                End If
                                JetnetACIDsToInclude += TemporaryHoldingID
                            End If
                        ElseIf InStr(TemporaryHoldingID, "|CLIENT") Then 'This is a Client ID:
                            TemporaryHoldingID = Replace(TemporaryHoldingID, "|CLIENT", "")
                            If IsNumeric(TemporaryHoldingID) Then
                                If ClientACIDsToInclude <> "" Then
                                    ClientACIDsToInclude += ", "
                                End If
                                ClientACIDsToInclude += TemporaryHoldingID
                            End If
                        End If
                    Next
                End If
            End If


            If ClientACIDsToInclude <> "" Then
                'We need to set the extra_sold_criteria here.
                ClientExtraCriteria = " and cliaircraft_id in (" & ClientACIDsToInclude & ")"
            End If


            If JetnetACIDsToInclude <> "" Then
                'We need to set the extra_sold_criteria here.
                JetnetExtraCriteria = " and ac_id in (" & JetnetACIDsToInclude & ")"
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                ClientExtraCriteria += " and ((cliaircraft_airframe_total_hours >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (cliaircraft_airframe_total_hours IS NULL))"
                JetnetExtraCriteria += " and ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))"
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                ClientExtraCriteria += " and ((cliaircraft_airframe_total_hours <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (cliaircraft_airframe_total_hours IS NULL))"
                JetnetExtraCriteria += " and ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))"
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                ClientExtraCriteria += " and cliaircraft_year_mfr >= " & searchCriteria.ViewCriteriaYearStart
                JetnetExtraCriteria += " and ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                ClientExtraCriteria += " and cliaircraft_year_mfr <=  " & searchCriteria.ViewCriteriaYearEnd
                JetnetExtraCriteria += " and ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd
            End If

            If run_market_insert = True Then
                aclsLocal_Notes.lnote_jetnet_comp_id = 0
                aclsLocal_Notes.lnote_client_comp_id = 0
                aclsLocal_Notes.lnote_client_contact_id = 0
                aclsLocal_Notes.lnote_jetnet_contact_id = 0
                aclsLocal_Notes.lnote_document_flag = "N"
                aclsLocal_Notes.lnote_note = ""
                aclsLocal_Notes.lnote_id = 0
                aclsLocal_Notes.lnote_action_date = CDate(Date.Now()).Date
                aclsLocal_Notes.lnote_clipri_ID = 1
                aclsLocal_Notes.lnote_user_login = HttpContext.Current.Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
                aclsLocal_Notes.lnote_user_name = Left(HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName, 15)
                aclsLocal_Notes.lnote_user_id = HttpContext.Current.Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
                aclsLocal_Notes.lnote_status = "S"

                CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(HttpContext.Current.Session.Item("CLIENT_AC_ID"))
                If Not IsNothing(CurrentData) Then
                    If CurrentData.Rows.Count > 0 Then
                        ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "cliaircraft")

                        aclsLocal_Notes.lnote_client_amod_id = ac_temp.cliaircraft_cliamod_id
                        aclsLocal_Notes.lnote_jetnet_amod_id = searchCriteria.ViewCriteriaAmodID
                    End If
                End If

                '--------------------- INSERT INTO NOTE RECORD if one doesnt exist---------- 

                new_note_id = aclsData_Temp.Check_If_Note_Exists(Date.Now, searchCriteria.ViewCriteriaAmodID, ac_temp.cliaircraft_cliamod_id)
                If new_note_id = 0 Then
                    new_note_id = aclsData_Temp.Insert_Note(aclsLocal_Notes)
                Else
                    Try
                        delete_string = " delete from client_value_comparables where clival_note_id = " & new_note_id & " "
                        If NOTE_ID > 0 And Trim(delete_string) <> "" Then
                            SqlConn.ConnectionString = aclsData_Temp.client_DB
                            SqlConn.Open()
                            SqlCommand.Connection = SqlConn

                            SqlCommand.CommandText = delete_string
                            SqlCommand.ExecuteNonQuery()
                        End If
                    Catch ex As Exception
                    Finally
                        SqlCommand.Dispose()
                        SqlConn.Close()
                        SqlConn.Dispose()
                    End Try
                End If
                '--------------------- INSERT INTO NOTE RECORD if one doesnt exist---------- 
            End If


            If Trim(order_by_string) = "" Then

                'This grabs the data for the jetnet aircraft for sale
                JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string, JetnetExtraCriteria, displayEValues)

                'This grabs the client side aircraft for sale
                clientTable = get_client_model_forsale_info(searchCriteria, client_string, order_by_string, ClientExtraCriteria, displayEValues)

                'This takes those two datatables, excludes the jetnet ones we have client aircraft for, adds the extra fields to make
                'The schemas match and then merges them into results table.
                results_table = ModifyAndCombineJetnetClientDataForSale(clientTable, JetnetTable, searchCriteria, FullClientIDstoExclude, order_by_string, "N")

                If Not IsNothing(table_to_add) Then
                    table_to_add = results_table
                End If

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        htmlOut.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0' align='center'><thead><tr>")



                        If DisplayLink Then
                            htmlOut.Append(" <th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")
                            If CRMViewActive Then
                                htmlOut.Append(" <th>HIDDEN IDs</th>")
                            End If
                            htmlOut.Append("<th><span  class=""help_cursor"" title=""Source of data as JETNET or Client"">SRC</span></th><th><span  class=""help_cursor"" title=""Click on the pencil icon to edit this aircraft record"">EDT</span></th>")
                        End If



                        htmlOut.Append("<th>SER<br />NUM</th>")
                        htmlOut.Append("<th>REG<br />NUM</th>")

                        If DisplayLink = True Then
                            htmlOut.Append("<th>YEAR MFR</th>")
                            htmlOut.Append("<th>YEAR DLV</th>")
                        Else
                            htmlOut.Append("<th>YEAR<br />MFR/DLV</th>")
                            ' htmlOut.Append("<th>YEAR<br />DLV</th>")
                        End If

                        htmlOut.Append("<th>ASKING ($k)</th>")


                        'Take Price Added
                        If CRMViewActive Then
                            htmlOut.Append("<th>TAKE  ($k)</th>")
                            htmlOut.Append("<th>EST VALUE</th>")
                        End If

                        If displayEValues Then 'evalues 
                            ' eValue and Model Year Avg eValue
                            htmlOut.Append("<th class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>" & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</th>")
                            htmlOut.Append("<th class=""" & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>MODEL YEAR AVG " & Left(Constants.eValues_Refer_Name, Constants.eValues_Refer_Name.Length - 1) & "</th>")
                        End If

                        If DisplayLink = True Then
                            htmlOut.Append("<th>DATE LISTED</th>")
                        Else
                            htmlOut.Append("<th>LIST<br />DATE</th>")
                        End If


                        If DisplayLink = True Then
                            If (CRMViewActive And HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True) Then ' Or HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                                htmlOut.Append("<th class=""text_align_center"">LAST SALE PRICE ($k)</th>")
                                htmlOut.Append("<th class=""text_align_center"">SALE PRICE DATE</th>")
                                ' htmlOut.Append("<th class=""text_align_center"">EST AFTT</th>")
                            End If
                        End If

                        htmlOut.Append("<th>AFTT</th>")
                        htmlOut.Append("<th>ENGINE TT</th>")

                        htmlOut.Append("<th>ENG 1</br>SOH</th>")
                        htmlOut.Append("<th>ENG 2</br>SOH</th>")


                        'Feature Codes:
                        If DisplayLink Then
                            JetnetViewData.load_standard_ac_features(searchCriteria, arrStdFeatCodes)

                            Dim sNonStandardAcFeature As String = ""
                            JetnetViewData.display_nonstandard_feature_code_headings(searchCriteria, arrFeatCodes, arrStdFeatCodes, cellWidth, sNonStandardAcFeature)
                            htmlOut.Append(sNonStandardAcFeature)
                        End If

                        htmlOut.Append("<th title='Number Of Passengers'>PAX</th>")
                        If DisplayLink = True Then
                            htmlOut.Append("<th>INT<br />YEAR</th>")
                            htmlOut.Append("<th>EXT<br />YEAR</th>")
                        Else
                            htmlOut.Append("<th>INT/EXT<br />YEAR</th>")
                        End If

                        htmlOut.Append("<th>ENGINE MAINTENANCE PROGRAM</th>")

                        If CRMViewActive = True Then
                            htmlOut.Append("<th>VALUE<br />NOTE</th>")
                        End If

                        If DisplayLink Then
                            If NOTE_ID > 0 Then
                                If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
                                    If runModelOnly = False Then
                                        htmlOut.Append("<th>$</th>")
                                    End If
                                End If
                            End If
                            If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
                                htmlOut.Append("<th><span  class=""help_cursor"" title=""Note indicator. Use the mouse over to see the latest note or click to add a note."">NTE</span></th>")
                            End If

                        End If

                        If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                            htmlOut.Append("<th>Status</th>")
                        End If

                        htmlOut.Append("<th>Based</th>")

                        If Not searchCriteria.ViewCriteriaIsReport Then
                            htmlOut.Append("<th width=""250"">OWNER</th>")
                            If DisplayLink Then
                                htmlOut.Append("<th width=""250"">BROKER</th>")
                            End If
                        Else
                            htmlOut.Append("<th width=""250"">OWNER</th>")
                            htmlOut.Append("<th>OWNERPHONE</th>")
                            htmlOut.Append("<th width=""250"">OPERATOR</th>")
                            htmlOut.Append("<th>OPERATORPHONE</th>")
                            htmlOut.Append("<th width=""250"">BROKER</th>")
                            htmlOut.Append("<th>BROKERPHONE</th>")
                        End If




                        htmlOut.Append("</tr></thead><tbody>")

                        start_text = htmlOut.ToString


                        For Each r As DataRow In results_table.Rows
                            'Setting up Next/Previous
                            ReDim Preserve arrayOfIDs(arrCounter)
                            arrayOfIDs(arrCounter) = r.Item("ac_id").ToString & "|" & r.Item("source")
                            arrCounter += 1
                            ' set the ac_id for this listing
                            searchCriteria.ViewCriteriaAircraftID = CLng(r.Item("ac_id").ToString)


                            row_count = row_count + 1

                            If run_market_insert = True Then

                                Select Case UCase(r("source").ToString)
                                    Case "JETNET"

                                        CurrentData = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(r("ac_id"), 0)
                                        If Not IsNothing(CurrentData) Then
                                            If CurrentData.Rows.Count > 0 Then
                                                ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "ac")
                                            End If
                                        End If

                                        ac_temp.cliaircraft_jetnet_ac_id = ac_temp.cliaircraft_id
                                        ac_temp.cliaircraft_id = 0
                                        aclsData_Temp.Insert_Client_Comparables(new_note_id, ac_temp, IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), ""), 0, searchCriteria.ViewCriteriaAmodID, "C")

                                    Case "CLIENT"

                                        CurrentData = aclsData_Temp.Get_Clients_Aircraft_For_Comparable(r("ac_id"))
                                        If Not IsNothing(CurrentData) Then
                                            If CurrentData.Rows.Count > 0 Then
                                                ac_temp = clsGeneral.clsGeneral.Create_Aircraft_Class(CurrentData, "cliaircraft")
                                            End If
                                        End If

                                        aclsData_Temp.Insert_Client_Comparables(new_note_id, ac_temp, IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), ""), r("client_model_id"), searchCriteria.ViewCriteriaAmodID, "C")
                                End Select
                            End If


                            htmlOut.Append("<tr bgcolor='white' class='" & IIf(CRMViewActive, r.Item("source").ToString, "") & "CRMRow'>")

                            If DisplayLink Then
                                htmlOut.Append("<td></td>")
                                If CRMViewActive Then
                                    htmlOut.Append(" <td>" & r.Item("ac_id").ToString & "|" & r.Item("source").ToString & "</td>")
                                End If
                                htmlOut.Append("<td class=""text_align_center"">" + IIf(r.Item("source").ToString = "JETNET", "<span id='src_text_replace' style=""display:none;"" title='JETNET'>JETNET</span><img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<span id='src_text_replace' style=""display:none;"" title='CLIENT'>CLIENT</span><img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")
                            End If

                            If DisplayLink Then
                                If CRMViewActive Then
                                    htmlOut.Append("<td class=""text_align_center"">")

                                    If NOTE_ID > 0 Then
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1" & IIf(Not String.IsNullOrEmpty(VariantListStr), "&extra_amod=" & VariantListStr, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    Else
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1" & IIf(Not String.IsNullOrEmpty(VariantListStr), "&extra_amod=" & VariantListStr, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    End If

                                    htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
                                    htmlOut.Append("</a>")
                                    htmlOut.Append("</td>")
                                End If
                            End If


                            htmlOut.Append("<td class=""text_align_center"" data-sort=""" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), "") & """><span class=""padding"">")  ' SERIAL NUMBER
                            htmlOut.Append(font_text)
                            If (Not searchCriteria.ViewCriteriaIsReport And DisplayLink) Or DisplayLink Then
                                If Not IsDBNull(r("ac_ser_no_full")) Then

                                    If r.Item("source").ToString = "JETNET" Then
                                        'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                    Else
                                        Dim JetnetForSaleCheck As New DataTable
                                        Dim NotForSaleJetnetSide As Boolean = False
                                        'This is where we need to add a check for client off market aircraft. 
                                        'On both the market summary view and the value view need to have a way of showing that an aircraft is an off market.
                                        'Recommend the following: on display of every client record in the listing check to see if there is a 
                                        'corresponding jetnet for sale record 
                                        '(select count(*) from aircraft where ac_id = #### and ac_journ_id = 0 and ac_forsale_flag=’Y’), 
                                        'if not then color the serial number red and bold it and modify the alt tag/mouseover to read as 
                                        '“Display Aircraft Details: JETNET shows this aircraft as off market.
                                        JetnetForSaleCheck = JetnetViewData.Check_Jetnet_Off_Market_Aircraft(r.Item("client_jetnet_ac_id"))
                                        If Not IsNothing(JetnetForSaleCheck) Then
                                            If JetnetForSaleCheck.Rows.Count > 0 Then
                                                If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
                                                    NotForSaleJetnetSide = True
                                                End If
                                            End If
                                        End If

                                        ' htmlOut.Append("<a onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0&source=" & r.Item("source").ToString & """,""AircraftDetails"");'")
                                        htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, False, "", "underline", "&source=" & r.Item("source").ToString))

                                        If NotForSaleJetnetSide Then
                                            htmlOut.Append(" class='underline red_text' title='Display Aircraft Details: JETNET shows this aircraft as off market.'>")
                                        Else
                                            htmlOut.Append(" class='underline' title='Display Aircraft Details'>")
                                        End If

                                    End If



                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                            Else

                                If Not IsDBNull(r("ac_ser_no_full")) Then
                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString)
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If

                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td>")
                            htmlOut.Append("<td class=""text_align_center""><span class=""padding"">")
                            htmlOut.Append(font_text)
                            If Not IsDBNull(r("ac_reg_no")) Then
                                htmlOut.Append(r.Item("ac_reg_no").ToString)
                            Else
                                htmlOut.Append("&nbsp;")
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td>")



                            If DisplayLink = True Then

                                htmlOut.Append("<td class=""text_align_center""><span class=""padding"">") ' YR MFG


                                If Not IsDBNull(r("ac_mfr_year")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_mfr_year")) Then
                                        If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
                                            htmlOut.Append("0")
                                        Else
                                            htmlOut.Append(r.Item("ac_mfr_year").ToString)
                                        End If
                                    End If
                                Else
                                    htmlOut.Append("U")

                                End If

                                htmlOut.Append("</span></td><td class=""text_align_center""><span class=""padding"">") ' YR DLV

                                If Not IsDBNull(r("ac_year")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_year")) Then
                                        If CDbl(r.Item("ac_year").ToString) = 0 Then
                                            htmlOut.Append("0")
                                        Else
                                            htmlOut.Append(r.Item("ac_year").ToString)
                                        End If
                                    End If
                                Else
                                    htmlOut.Append("U")
                                End If

                                htmlOut.Append("</span></td>")
                            Else
                                htmlOut.Append("<td class=""text_align_center""><span class=""padding"">")
                                htmlOut.Append(font_text)
                                If Not IsDBNull(r("ac_mfr_year")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_mfr_year")) Then
                                        If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
                                            htmlOut.Append("0")
                                        Else
                                            htmlOut.Append(r.Item("ac_mfr_year").ToString)
                                        End If
                                    End If
                                Else
                                    htmlOut.Append("U")

                                End If

                                htmlOut.Append("/")

                                If Not IsDBNull(r("ac_year")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_year")) Then
                                        If CDbl(r.Item("ac_year").ToString) = 0 Then
                                            htmlOut.Append("0")
                                        Else
                                            htmlOut.Append(r.Item("ac_year").ToString)
                                        End If
                                    End If
                                Else
                                    htmlOut.Append("U")
                                End If
                                htmlOut.Append(efont_text)
                                htmlOut.Append("</span></td>")
                            End If


                            htmlOut.Append("<td class=""text_align_center"">") ' ASKING

                            htmlOut.Append("<span class=""padding"">")
                            htmlOut.Append(font_text)

                            If Not IsDBNull(r("ac_asking")) Then
                                If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                                    If Not IsDBNull(r("ac_asking_price")) Then
                                        If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                                            htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_asking_price").ToString) / 1000), 0).ToString + "")
                                        End If
                                    End If
                                Else
                                    htmlOut.Append(JetnetViewData.forsale_status(r.Item("ac_asking").ToString.Trim))
                                End If
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td>")


                            'Take Price Added 
                            If CRMViewActive Then
                                htmlOut.Append("<td class=""text_align_center"">")
                                htmlOut.Append("<span class=""padding"">")
                                htmlOut.Append(font_text)
                                If Not IsDBNull(r("ac_take_price")) Then
                                    If CDbl(r.Item("ac_take_price").ToString) > 0 Then
                                        htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_take_price").ToString) / 1000), 0).ToString + "")
                                    End If
                                End If
                                htmlOut.Append(efont_text)
                                htmlOut.Append("</span></td>")
                            End If


                            'sold_price  Added 
                            If CRMViewActive Then
                                htmlOut.Append("<td class=""text_align_center"">")
                                htmlOut.Append("<span class=""padding"">")
                                htmlOut.Append(font_text)
                                If Not IsDBNull(r("sold_price")) Then
                                    If CDbl(r.Item("sold_price").ToString) > 0 Then
                                        htmlOut.Append("$" + FormatNumber((CDbl(r.Item("sold_price").ToString) / 1000), 0).ToString + "")
                                    End If
                                End If
                                htmlOut.Append(efont_text)
                                htmlOut.Append("</span></td>")
                            End If

                            If displayEValues Then 'evalues 
                                ' eValue and Model Year Avg eValue
                                htmlOut.Append("<td class=""text_align_center  " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>")
                                If Not IsDBNull(r("EVALUE")) Then
                                    If IsNumeric(r("EVALUE")) Then
                                        If r("EVALUE") > 0 Then
                                            If r("SOURCE") = "JETNET" Then
                                                htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("EVALUE")))
                                            End If
                                        Else
                                            If r("SOURCE") = "CLIENT" Then
                                                JetnetTable.Clear()
                                                JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string, " and ac_id = " & r.Item("client_jetnet_ac_id").ToString, displayEValues)
                                                If Not IsNothing(JetnetTable) Then
                                                    If JetnetTable.Rows.Count > 0 Then
                                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(JetnetTable.Rows(0).Item("EVALUE")))
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                htmlOut.Append("</td>")
                                htmlOut.Append("<td class=""text_align_center " & HttpContext.Current.Session.Item("localUser").crmUser_Evalues_CSS & """>")
                                If Not IsDBNull(r("AVGMODYREVALUE")) Then
                                    If IsNumeric(r("AVGMODYREVALUE")) Then
                                        If r("AVGMODYREVALUE") > 0 Then
                                            If r("SOURCE") = "JETNET" Then
                                                htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("AVGMODYREVALUE")))
                                            End If
                                        Else
                                            If r("SOURCE") = "CLIENT" Then
                                                If Not IsNothing(JetnetTable) Then
                                                    If JetnetTable.Rows.Count > 0 Then
                                                        htmlOut.Append(clsGeneral.clsGeneral.ConvertIntoThousands(JetnetTable.Rows(0).Item("AVGMODYREVALUE")))
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                                htmlOut.Append("</td>")
                            End If

                            htmlOut.Append("<td class=""text_align_center""")
                            Dim dateSort As String = ""
                            If Not IsDBNull(r.Item("ac_list_date")) Then
                                If IsDate(r.Item("ac_list_date").ToString) Then
                                    dateSort = Format(r.Item("ac_list_date"), "yyyy/MM/dd")
                                End If
                            End If

                            htmlOut.Append(" data-sort=""" & dateSort & """>") ' AC LIST DATE
                            htmlOut.Append("<span class=""padding"">")
                            htmlOut.Append(font_text)

                            If Not IsDBNull(r.Item("ac_list_date")) Then
                                If IsDate(r.Item("ac_list_date").ToString) Then
                                    htmlOut.Append(CommonAircraftFunctions.trim_out_year_start(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate)))
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                            Else
                                htmlOut.Append("&nbsp;")
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td>")



                            '------------------------------------------------------------------------------- 
                            If DisplayLink = True Then
                                If (CRMViewActive And HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True) Then ' Or HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                                    temp_last_price = 0
                                    temp_last_date = ""
                                    htmlOut_trans = ""
                                    use_looked_up = False
                                    ' If r.Item("source").ToString = "JETNET" Then
                                    Dim LookupID As Long = 0
                                    Dim LookupString As String = ""
                                    If r.Item("source").ToString = "JETNET" Then
                                        LookupString = " and clitrans_jetnet_ac_id =  " & If(r.Item("client_jetnet_ac_id") > 0, r.Item("client_jetnet_ac_id").ToString, r.Item("ac_id").ToString)
                                    Else
                                        If r.Item("client_jetnet_ac_id") > 0 Then
                                            LookupString = " and clitrans_jetnet_ac_id =  " & r.Item("client_jetnet_ac_id").ToString
                                        Else
                                            LookupString = " and clitrans_cliac_id  =  " & r.Item("ac_id").ToString
                                        End If
                                    End If


                                    clientTable.Clear()
                                    If LookupString <> "" Then
                                        clientTable = get_client_model_forsale_info_single_ac(searchCriteria, client_string, order_by_string, LookupString)

                                        If Not IsNothing(clientTable) Then
                                            If clientTable.Rows.Count > 0 Then
                                                For Each c As DataRow In clientTable.Rows

                                                    If Not IsDBNull(c("LASTSALEPRICE")) Then
                                                        If Not IsDBNull(c("LASTSALEPRICE")) Then
                                                            If CDbl(c.Item("LASTSALEPRICE").ToString) > 0 Then
                                                                temp_last_price = CDbl(c.Item("LASTSALEPRICE").ToString)

                                                                htmlOut_trans &= (DisplayFunctions.TextToImage("$" & FormatNumber((c.Item("LASTSALEPRICE").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))

                                                            End If
                                                        End If
                                                    End If

                                                    If Not IsDBNull(c("LASTSALEPRICEDATE")) Then
                                                        If Not IsDBNull(c("LASTSALEPRICEDATE")) Then
                                                            temp_last_date = FormatDateTime(c.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate)
                                                            htmlOut.Append(font_text)
                                                            htmlOut_trans &= ("" & FormatDateTime(c.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate))
                                                            htmlOut.Append(efont_text)
                                                        End If
                                                    End If

                                                Next
                                            End If
                                        End If
                                        'End If

                                        '---------------- COMPARE THE DATES, IF IT IS GREATER THAN CURRENT, THEN USE IT --------- 
                                        If Trim(temp_last_date) <> "" Then
                                            If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                    If CDate(temp_last_date) > CDate(r.Item("LASTSALEPRICEDATE")) Then
                                                        use_looked_up = True
                                                    End If
                                                End If
                                            Else
                                                use_looked_up = True 'the jetnet transaction price isn't there but we can still use the client side.
                                            End If
                                        End If
                                        '---------------- COMPARE THE DATES----------------------------------

                                        clientTable.Clear()

                                        htmlOut.Append("<td class=""text_align_center"">") ' AC LIST DATE
                                        htmlOut.Append(font_text)
                                        If use_looked_up = False Then

                                            If Not IsDBNull(r("LASTSALEPRICE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICE")) Then
                                                    If CDbl(r.Item("LASTSALEPRICE").ToString) > 0 Then
                                                        htmlOut.Append(DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("LASTSALEPRICE").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))
                                                    End If
                                                End If
                                            End If
                                            htmlOut.Append(" </td><td class=""text_align_center"">") ' AC LIST DATE

                                            If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                    htmlOut.Append("" & FormatDateTime(r.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate))
                                                End If
                                            End If

                                        Else
                                            Call show_last_items(htmlOut, temp_last_price, temp_last_date, "CLIENT")
                                        End If
                                        htmlOut.Append(efont_text)
                                        htmlOut.Append(" </td>")

                                    Else
                                        JetnetTable.Clear()
                                        JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string, " and ac_id = " & r.Item("ac_id").ToString, displayEValues)


                                        If Not IsNothing(JetnetTable) Then
                                            If JetnetTable.Rows.Count > 0 Then
                                                For Each j As DataRow In JetnetTable.Rows

                                                    If Not IsDBNull(j("LASTSALEPRICE")) Then
                                                        If Not IsDBNull(j("LASTSALEPRICE")) Then
                                                            If CDbl(j.Item("LASTSALEPRICE").ToString) > 0 Then
                                                                temp_last_price = CDbl(j.Item("LASTSALEPRICE").ToString)

                                                                htmlOut_trans &= (DisplayFunctions.TextToImage("$" & FormatNumber((j.Item("LASTSALEPRICE").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))

                                                            End If
                                                        End If
                                                    End If

                                                    If Not IsDBNull(j("LASTSALEPRICEDATE")) Then
                                                        If Not IsDBNull(j("LASTSALEPRICEDATE")) Then
                                                            temp_last_date = FormatDateTime(j.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate)
                                                            htmlOut_trans &= ("" & FormatDateTime(j.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate))
                                                        End If
                                                    End If

                                                Next
                                            End If
                                        End If

                                        '---------------- COMPARE THE DATES, IF IT IS GREATER THAN CURRENT, THEN USE IT --------- 
                                        If Trim(temp_last_date) <> "" Then
                                            If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                    If CDate(temp_last_date) > CDate(r.Item("LASTSALEPRICEDATE")) Then
                                                        use_looked_up = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                        '---------------- COMPARE THE DATES----------------------------------

                                        JetnetTable.Clear()

                                        If use_looked_up = False Then
                                            htmlOut.Append("<td class=""text_align_center"">") ' AC LIST DATE
                                            htmlOut.Append(font_text)
                                            If Not IsDBNull(r("LASTSALEPRICE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICE")) Then
                                                    If CDbl(r.Item("LASTSALEPRICE").ToString) > 0 Then
                                                        htmlOut.Append(DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("LASTSALEPRICE").ToString / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))
                                                    End If
                                                End If
                                            End If
                                            htmlOut.Append(" ")
                                            htmlOut.Append(efont_text)
                                            htmlOut.Append("</td><td class=""text_align_center"">") ' AC LIST DATE
                                            htmlOut.Append(font_text)
                                            If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                If Not IsDBNull(r("LASTSALEPRICEDATE")) Then
                                                    htmlOut.Append("" & FormatDateTime(r.Item("LASTSALEPRICEDATE"), DateFormat.ShortDate))
                                                End If
                                            End If
                                        Else
                                            Call show_last_items(htmlOut, temp_last_price, temp_last_date, "JETNET")
                                        End If
                                        htmlOut.Append(efont_text)
                                        htmlOut.Append(" </td>")

                                    End If

                                End If
                            End If
                            '-------------------------------------------------------------------------------

                            '  htmlOut.Append("<td class=""text_align_center"">") ' AC LIST DATE

                            'If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                            '  If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                            '    htmlOut.Append("" + (r.Item("ac_est_airframe_hrs").ToString + ""))
                            '  End If
                            'End If


                            htmlOut.Append("<td class=""text_align_center"">") ' AFTT
                            htmlOut.Append("<span class=""padding"">")
                            htmlOut.Append(font_text)

                            If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                                If CDbl(r.Item("ac_airframe_tot_hrs").ToString) = 0 Then
                                    htmlOut.Append("0")
                                Else
                                    htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString)
                                End If
                            Else
                                htmlOut.Append("U")
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td><td class=""text_align_center"">") ' Engine Times
                            htmlOut.Append("<span class=""padding"">")
                            htmlOut.Append(font_text)
                            If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                                If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                                    htmlOut.Append("[0] ")
                                Else
                                    htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "] ")
                                End If
                            Else
                                htmlOut.Append("[U] ")
                            End If

                            If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                                If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                                    htmlOut.Append("[0] ")
                                Else
                                    htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "] ")
                                End If
                            Else
                                htmlOut.Append("[U] ")
                            End If

                            If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                                If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                                    htmlOut.Append("[0] ")
                                Else
                                    htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "] ")
                                End If
                            End If

                            If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                                If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                                    htmlOut.Append("[0] ")
                                Else
                                    htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "] ")
                                End If
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span>")


                            htmlOut.Append("</td>") 'BASED 
                            htmlOut.Append("<td>")
                            If Not IsDBNull(r.Item("ac_engine_1_soh_hrs")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_engine_1_soh_hrs")) Then
                                    htmlOut.Append("" + r.Item("ac_engine_1_soh_hrs").ToString.Trim + "")
                                End If
                            End If
                            htmlOut.Append("</td>") 'BASED

                            htmlOut.Append("<td>")
                            If Not IsDBNull(r.Item("ac_engine_2_soh_hrs")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_engine_2_soh_hrs")) Then
                                    htmlOut.Append("" + r.Item("ac_engine_2_soh_hrs").ToString.Trim + "")
                                End If
                            End If


                            If DisplayLink Then
                                htmlOut.Append("</td>") '<td align='center' valign='middle' class='forSaleCellBorder'>") ' Feature Codes

                                Dim sAcFeatureCodes As String = ""
                                '''''''''''''''''''''''''''''''''''''''''''

                                If Not IsDBNull(r.Item("source").ToString) Then
                                    If Trim(r.Item("source").ToString) = "CLIENT" Then
                                        JetnetViewData.display_client_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                                    Else
                                        JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                                    End If
                                Else
                                    JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                                End If


                                htmlOut.Append(sAcFeatureCodes)

                            End If



                            htmlOut.Append("<td class=""text_align_center"">") ' PASSENGERS
                            htmlOut.Append("<span class=""padding"">")
                            htmlOut.Append(font_text)
                            If Not IsDBNull(r("ac_passenger_count")) Then
                                If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
                                    htmlOut.Append("0 ")
                                Else
                                    htmlOut.Append(r.Item("ac_passenger_count").ToString + " ")
                                End If
                            Else
                                htmlOut.Append("U ")
                            End If
                            htmlOut.Append(efont_text)
                            htmlOut.Append("</span></td><td class=""text_align_center"">") ' INT YEAR


                            If DisplayLink = True Then
                                htmlOut.Append("<span class=""padding"">")
                                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")
                                    End If
                                    htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If
                                htmlOut.Append("</span></td><td class=""text_align_center"">") ' EXT YEAR
                                htmlOut.Append("<span class=""padding"">")
                                '   End If

                                If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")

                                    End If
                                    htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If
                            Else
                                htmlOut.Append("<span class=""padding"">")
                                htmlOut.Append(font_text)
                                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")
                                    End If
                                    htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
                                Else
                                    htmlOut.Append(" ")
                                End If

                                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                        htmlOut.Append("/")
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")

                                    End If
                                    htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If
                                htmlOut.Append(efont_text)
                            End If

                            htmlOut.Append("<td>")
                            If Not IsDBNull(r("emp_program_name")) Then
                                htmlOut.Append(r("emp_program_name"))
                            End If
                            htmlOut.Append("</td>")
                            'If DisplayLink Then
                            '  If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

                            '    htmlOut.Append("</td><td>") ' NOTES

                            '    'This appends the notes on the table.
                            '    htmlOut.Append(HTML_NOTE)

                            '  End If
                            'End If



                            If CRMViewActive = True Then
                                htmlOut.Append("</span></td><td class=""text_align_center"">") ' EXT YEAR
                                htmlOut.Append("<span class=""padding"">")
                                htmlOut.Append(font_text)
                                If Not String.IsNullOrEmpty(r.Item("cliaircraft_value_description").ToString) Then
                                    If Trim(r.Item("cliaircraft_value_description")) <> "" Then
                                        htmlOut.Append(r.Item("cliaircraft_value_description"))
                                    Else
                                        htmlOut.Append(" ")
                                    End If
                                Else
                                    htmlOut.Append(" ")
                                End If
                                htmlOut.Append(efont_text)
                            End If

                            htmlOut.Append("</span></td>")

                            'newly moved:
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'OWNER LOOKUP MOVED TO BEFORE NOTES ICON SO QUERY HAD TO BE DONE ONLY ONCE.
                            searchCriteria.ViewCriteriaGetExclusive = False
                            searchCriteria.ViewCriteriaGetOperator = False

                            Dim ownerDataTable As New DataTable

                            Select Case UCase(r("source").ToString)
                                Case "JETNET"
                                    ownerDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
                                Case "CLIENT"
                                    ownerDataTable = Get_Client_Owner_Info(searchCriteria, 0)
                            End Select


                            If DisplayLink Then

                                If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
                                    If NOTE_ID > 0 Then
                                        If runModelOnly = False Then
                                            htmlOut.Append("<td class=""text_align_center"">") ' PROSPECTS

                                            'This appends the notes on the table.
                                            htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "F", REAL_AC_ID, 0, COMPLETED_DATE, 0))

                                            htmlOut.Append("</td>")
                                        End If
                                    End If

                                    htmlOut.Append("<td class=""text_align_center"">") ' NOTE ADD 
                                    If Not IsNothing(ownerDataTable) Then
                                        If ownerDataTable.Rows.Count > 0 Then
                                            Dim TemporaryCompanyID As Long = 0
                                            Dim CheckNoteTable As New DataTable

                                            htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit.aspx?prospectACID=" & IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")) & "&comp_ID=")

                                            'Need to send jetnet company ID
                                            If UCase(r("source")) = "JETNET" Then
                                                htmlOut.Append(ownerDataTable.Rows(0).Item("comp_id"))
                                                TemporaryCompanyID = ownerDataTable.Rows(0).Item("comp_id")
                                            Else
                                                htmlOut.Append(ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id"))
                                                TemporaryCompanyID = ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id")
                                            End If

                                            htmlOut.Append("&source=JETNET&type=company&action=checkforcreation&note_type=A&from=view&rememberTab=" & ActiveTabIndex & "&returnView=" & searchCriteria.ViewID & IIf(NOTE_ID > 0, "&NoteID=" & NOTE_ID, "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">")


                                            HTML_NOTE = CheckForNotesForSaleTab(CRMViewActive, r.Item("source").ToString, r.Item("ac_id"), aclsData_Temp)

                                            If HTML_NOTE = "" Then
                                                htmlOut.Append("<span class=""notePlusBlock"" style=""background-image:url('images/blue_plus_sign.png');""> </span>")
                                            Else
                                                htmlOut.Append(Replace(HTML_NOTE, "images/document.png", "images/note_pin_add.png"))
                                            End If


                                            htmlOut.Append("</a>")
                                        Else
                                            Dim CheckNoteTable As New DataTable

                                            htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit_note.aspx?source=" & r("source") & "&from=view&ac_ID=" & IIf(r("source") = "JETNET", r("client_jetnet_ac_id"), r("ac_id")) & "&type=note&action=new&ViewID=" & searchCriteria.ViewID & "&refreshing=prospect&rememberTab=" & ActiveTabIndex & IIf(NOTE_ID > 0, "&NoteID=" & NOTE_ID, "") & "');return false;"">")

                                            HTML_NOTE = CheckForNotesForSaleTab(CRMViewActive, r.Item("source").ToString, r.Item("ac_id"), aclsData_Temp)

                                            If HTML_NOTE = "" Then
                                                htmlOut.Append("<span class=""notePlusBlock"" style=""background-image:url('images/blue_plus_sign.png');""> </span>")
                                            Else
                                                htmlOut.Append(Replace(HTML_NOTE, "images/document.png", "images/note_pin_add.png"))
                                            End If


                                            htmlOut.Append("</a>")

                                        End If
                                    End If
                                    htmlOut.Append("</td>")

                                End If
                            End If


                            'NEW FIELDS 6/12/17

                            'Aircraft Status
                            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                                htmlOut.Append("<td>") 'STATUS

                                ' Don't show status for Aerodex Users
                                If Not IsDBNull(r.Item("ac_status")) And Not String.IsNullOrEmpty(r.Item("ac_status").ToString) Then
                                    htmlOut.Append(UCase(r.Item("ac_status").ToString.Trim + " "))
                                End If
                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                'Aircraft Delivery / Delivery Date
                                If Not IsDBNull(r.Item("ac_delivery")) And Not String.IsNullOrEmpty(r.Item("ac_delivery").ToString) Then
                                    If r.Item("ac_delivery").ToString.ToLower.Contains("date") Then
                                        JetnetTable.Clear()
                                        JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string, " and ac_id = " & r.Item("ac_id").ToString)

                                        If Not IsNothing(JetnetTable) Then
                                            If JetnetTable.Rows.Count > 0 Then
                                                If Not IsDBNull(JetnetTable.Rows(0).Item("ac_delivery_date")) And Not String.IsNullOrEmpty(JetnetTable.Rows(0).Item("ac_delivery_date").ToString) Then
                                                    htmlOut.Append(UCase(("" + FormatDateTime(JetnetTable.Rows(0).Item("ac_delivery_date").ToString, DateFormat.ShortDate) + "")))
                                                End If
                                            End If
                                        End If
                                    Else
                                        htmlOut.Append(UCase(("" + r.Item("ac_delivery").ToString.Trim + "")))
                                    End If
                                End If
                                htmlOut.Append("</td>")
                            End If


                            htmlOut.Append("<td>")
                            Dim AportInfo As String = ""
                            If Not IsDBNull(r.Item("ac_aport_city")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_city")) Then
                                    AportInfo = (("" + r.Item("ac_aport_city").ToString.Trim + ""))
                                End If
                            End If
                            If Not IsDBNull(r.Item("ac_aport_country")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_country")) Then
                                    If AportInfo <> "" Then
                                        AportInfo += ", "
                                    End If
                                    AportInfo += ((" " + Replace(r.Item("ac_aport_country").ToString.Trim, "United States", "US") + ""))
                                End If
                            End If
                            If Not IsDBNull(r.Item("country_continent_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("country_continent_name")) Then
                                    If AportInfo <> "" Then
                                        AportInfo += " - "
                                    End If
                                    AportInfo += ((" " + Replace(r.Item("country_continent_name").ToString.Trim, "United States", "US") + ""))
                                End If
                            End If

                            htmlOut.Append(AportInfo)
                            htmlOut.Append("</td>") 'BASED




                            htmlOut.Append("<td width=""250"">") ' OWNER

                            'Owner table has been moved up above the notes icon. So it doesn't have to be ran twice.
                            If Not IsNothing(ownerDataTable) Then

                                If ownerDataTable.Rows.Count > 0 Then
                                    For Each vr_owner As DataRow In ownerDataTable.Rows
                                        htmlOut.Append("<span class=""padding"">")
                                        htmlOut.Append(font_text)
                                        Select Case UCase(r("source").ToString)
                                            Case "JETNET"
                                                sCompanyPhone = ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), vr_owner("comp_phone_fax"))
                                            Case "CLIENT"
                                                sCompanyPhone = ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(vr_owner.Item("comp_id").ToString), True)
                                        End Select

                                        'If String.IsNullOrEmpty(sCompanyPhone) Then
                                        '  sCompanyPhone = "Not listed"
                                        'End If

                                        If Not searchCriteria.ViewCriteriaIsReport And DisplayLink Then
                                            If r.Item("source").ToString = "JETNET" Then
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            Else
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("clicomp_jetnet_comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                ' htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            End If

                                            htmlOut.Append(" title='PH : " + sCompanyPhone + "'>" + vr_owner.Item("comp_name").ToString.Trim + "</a>")
                                        Else

                                            If r.Item("source").ToString = "JETNET" Then
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            Else
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("clicomp_jetnet_comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            End If

                                            htmlOut.Append(" title='PH : " + sCompanyPhone + "'>" + vr_owner.Item("comp_name").ToString.Trim + "</a>")

                                            htmlOut.Append(efont_text)
                                            htmlOut.Append("</span>")
                                            If DisplayLink Then
                                                htmlOut.Append("<td><span class=""padding"">" + sCompanyPhone & "</span></td>") ' OWNERPHONE  
                                            End If
                                        End If
                                    Next
                                Else
                                    If Not searchCriteria.ViewCriteriaIsReport Then
                                        'htmlOut.Append("None")
                                    Else
                                        htmlOut.Append("</td><td>")
                                    End If
                                End If
                            Else
                                If Not searchCriteria.ViewCriteriaIsReport Then
                                Else
                                    htmlOut.Append("</td><td width=""250"">")
                                End If
                            End If

                            ownerDataTable = Nothing

                            If searchCriteria.ViewCriteriaIsReport Then

                                searchCriteria.ViewCriteriaGetExclusive = False
                                searchCriteria.ViewCriteriaGetOperator = True

                                Dim operatorDataTable As New DataTable

                                Select Case UCase(r("source").ToString)
                                    Case "JETNET"
                                        operatorDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
                                    Case "CLIENT"
                                        operatorDataTable = Get_Client_Owner_Info(searchCriteria, 0)
                                End Select


                                If Not IsNothing(operatorDataTable) Then

                                    If operatorDataTable.Rows.Count > 0 Then
                                        For Each r_operator As DataRow In operatorDataTable.Rows
                                            sCompanyPhone = ""

                                            Select Case UCase(r("source").ToString)
                                                Case "JETNET"
                                                    sCompanyPhone = ReturnCompanyPhoneFax(r_operator("comp_phone_office"), r_operator("comp_phone_fax"))
                                                Case "CLIENT"
                                                    sCompanyPhone = ReturnCompanyPhoneFax(r_operator("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(r_operator.Item("comp_id").ToString), True)
                                            End Select

                                            htmlOut.Append("</td><td><span class=""padding"">") ' OPERATOR

                                            If r.Item("source").ToString = "JETNET" Then
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, r_operator.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r_operator.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            Else
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, r_operator.Item("clicomp_jetnet_comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r_operator.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            End If

                                            htmlOut.Append(" title='PH : " + sCompanyPhone + "'>" + r_operator.Item("comp_name").ToString.Trim + "</a>")


                                            htmlOut.Append("</span></td>")
                                            htmlOut.Append("<td><span class=""padding"">")

                                            htmlOut.Append(font_text)
                                            htmlOut.Append(sCompanyPhone)
                                            htmlOut.Append(efont_text)
                                            htmlOut.Append("</span>")

                                        Next
                                    Else
                                        htmlOut.Append("</td><td>") ' OPERATOR
                                        ' htmlOut.Append("None</td>")
                                        htmlOut.Append("</td><td> ") ' OPERATORPHONE  
                                    End If
                                Else
                                    htmlOut.Append("</td><td>") ' OPERATOR
                                    'htmlOut.Append("None</td>")
                                    htmlOut.Append("</td><td> ") ' OPERATORPHONE 
                                End If

                                operatorDataTable = Nothing

                            End If





                            If DisplayLink Then
                                htmlOut.Append("</td><td width=""250"">") ' BROKER

                                searchCriteria.ViewCriteriaGetExclusive = True
                                searchCriteria.ViewCriteriaGetOperator = False

                                Dim exclusiveDataTable As New DataTable

                                'We only need to try filling this up if the aircraft Exclusive flag is Y.
                                If r("ac_exclusive_flag") = "Y" Then
                                    Select Case UCase(r("source").ToString)
                                        Case "JETNET"
                                            exclusiveDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
                                        Case "CLIENT"
                                            exclusiveDataTable = Get_Client_Owner_Info(searchCriteria, 0)
                                    End Select
                                End If

                                If Not IsNothing(exclusiveDataTable) Then

                                    If exclusiveDataTable.Rows.Count > 0 Then
                                        For Each vr_exclusive As DataRow In exclusiveDataTable.Rows
                                            htmlOut.Append("<span class=""padding"">")
                                            Select Case UCase(r("source").ToString)
                                                Case "JETNET"
                                                    sCompanyPhone = ReturnCompanyPhoneFax(vr_exclusive("comp_phone_office"), vr_exclusive("comp_phone_fax"))
                                                Case "CLIENT"
                                                    sCompanyPhone = ReturnCompanyPhoneFax(vr_exclusive("comp_phone_office"), "") 'Get_Client_Company_Phone(CLng(vr_exclusive.Item("comp_id").ToString), True)
                                            End Select


                                            If String.IsNullOrEmpty(sCompanyPhone) Then
                                                ' sCompanyPhone = "Not listed"
                                            End If

                                            If r.Item("source").ToString = "JETNET" Then
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_exclusive.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            Else
                                                htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_exclusive.Item("clicomp_jetnet_comp_id").ToString.ToString, 0, 0, False, "", "underline", "&journid=0"))
                                                'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                                            End If

                                            htmlOut.Append(" title='PH : " + sCompanyPhone + "'><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></a>")
                                            htmlOut.Append("</span>")

                                            If Not searchCriteria.ViewCriteriaIsReport Then
                                            Else
                                                htmlOut.Append("</td>")
                                                htmlOut.Append("<td><span class=""padding"">" + sCompanyPhone + "</span>") ' BROKERPHONE  
                                            End If

                                        Next
                                    Else
                                        If Not searchCriteria.ViewCriteriaIsReport Then
                                            ' htmlOut.Append("None")
                                        Else
                                            'htmlOut.Append("None</td>")
                                            htmlOut.Append("</td><td> ") ' BROKERPHONE  
                                        End If
                                    End If
                                Else
                                    If Not searchCriteria.ViewCriteriaIsReport Then
                                        'htmlOut.Append("None")
                                    Else
                                        'htmlOut.Append("None</td>")
                                        htmlOut.Append("</td><td> ") ' BROKERPHONE  
                                    End If
                                End If

                                exclusiveDataTable = Nothing
                            End If

                            htmlOut.Append("</td>")



                            htmlOut.Append("</tr>")
                        Next

                    Else
                        htmlOut.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
                    End If

                Else
                    htmlOut.Append("<tr><td>Nothing For Sale at this time, for this Make/Model ...</td></tr>")
                End If

            Else
                '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
                '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
                '-----------------------CUSTOM ---------------------------------------------------------------------------------------------------
                'This grabs the data for the jetnet aircraft for sale
                JetnetTable = JetnetViewData.get_model_forsale_info2(searchCriteria, jetnet_string, order_by_string, JetnetExtraCriteria)

                'This grabs the client side aircraft for sale
                clientTable = get_client_model_forsale_info(searchCriteria, client_string, order_by_string, ClientExtraCriteria)

                'This takes those two datatables, excludes the jetnet ones we have client aircraft for, adds the extra fields to make
                'The schemas match and then merges them into results table.
                FullClientIDstoExclude = ""
                results_table = ModifyAndCombineJetnetClientDataForSale(clientTable, JetnetTable, searchCriteria, FullClientIDstoExclude, order_by_string, "N")

                ' If Not IsNothing(table_to_add) Then
                'table_to_add = results_table
                ' End If


                order_by_string_break = Split(order_by_string, ",")

                db_fields_names = Split(fields_name, ",")

                type_string_names = Split(type_string, ",")

                size_string_names = Split(size_string, ",")


                htmlOut.Append("<table  id='tableCopy' cellpadding='0' cellspacing='0' border='0' width=""100%"" >")
                htmlOut.Append("<thead bgcolor='#CCCCCC'>")
                htmlOut.Append("<th>SEL</th>") 'We need a buffer for the checkbox.

                If DisplayLink Then ' dont display these for export to excel
                    If CRMViewActive Then
                        htmlOut.Append("<th>HIDDEN IDs</th>")
                    End If
                    htmlOut.Append("<th>SRC</th>")

                    htmlOut.Append("<th>EDT</th>")
                    If NOTE_ID > 0 Then
                        htmlOut.Append("<th>$</th>")
                        htmlOut.Append("<th>NTE</th>") ' blue plus 
                    End If
                End If

                For i = 0 To order_by_string_break.Length - 1
                    htmlOut.Append("<th>")
                    If Trim(order_by_string_break(i)) = "'ASKING$'" Or Trim(order_by_string_break(i)) = "'TAKE$'" Or Trim(order_by_string_break(i)) = "'EST$'" Then
                        htmlOut.Append(Trim(Replace(Replace(order_by_string_break(i), "'", ""), "$", "($k)")) & "")
                    Else
                        htmlOut.Append(Trim(Replace(order_by_string_break(i), "'", "")) & "")
                    End If

                    htmlOut.Append("</th>")
                Next
                htmlOut.Append("</thead><tbody>")



                ' clientTable to be changed to combined one later
                If Not IsNothing(results_table) Then
                    If results_table.Rows.Count > 0 Then
                        For Each r As DataRow In results_table.Rows

                            htmlOut.Append("<tr class='alt_row " & IIf(CRMViewActive, r.Item("source").ToString, "") & "CRMRow'>")
                            htmlOut.Append("<td></td>") 'Empty checkbox cell
                            If CRMViewActive Then
                                htmlOut.Append(" <td>" & r.Item("ac_id").ToString & "|" & r.Item("source").ToString & "</td>")
                            End If
                            For i = 0 To order_by_string_break.Length - 1


                                If i = 0 And DisplayLink = True Then

                                    htmlOut.Append("<td class=""text_align_center"">" + IIf(r.Item("source").ToString = "JETNET", "<span id='src_text_replace' style=""display:none;"" title='JETNET'>JETNET</span><img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<span id='src_text_replace' style=""display:none;"" title='CLIENT'>CLIENT</span><img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")

                                    htmlOut.Append("<td class=""text_align_center"">")

                                    If NOTE_ID > 0 Then
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    Else
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    End If

                                    htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
                                    htmlOut.Append("</a>")
                                    htmlOut.Append("</td>")

                                    If NOTE_ID > 0 Then
                                        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

                                            htmlOut.Append("<td class=""text_align_center"">") ' PROSPECTS

                                            'This appends the notes on the table.
                                            htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "F", REAL_AC_ID, 0, COMPLETED_DATE, 0))

                                            htmlOut.Append("</td>")

                                            htmlOut.Append("<td class=""text_align_center"">") ' NOTE ADD 
                                            htmlOut.Append("<A href='#' title='Add Note' alt='Add Note'><img src='images/blue_plus_sign.png' width='16'></a>")
                                            htmlOut.Append("</td>")
                                        End If
                                    End If
                                End If


                                format_me = True
                                '32|cliaircraft_airframe_total_hours|ac_airframe_tot_hrs|Aircraft|JETNET|AFTT
                                temp_field = Trim(Replace(order_by_string_break(i), "'", ""))
                                temp_field_name = Trim(Replace(db_fields_names(i), "'", ""))

                                temp_type = Trim(Replace(type_string_names(i), "'", ""))
                                temp_size = Trim(Replace(size_string_names(i), "'", ""))

                                If Not IsDBNull(r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")) Then
                                    temp_val = r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")
                                Else
                                    temp_val = ""
                                End If

                                If Trim(temp_type) <> "" Then
                                    If Trim(temp_type) = "String" Then

                                    ElseIf Trim(temp_type) = "Char" Then

                                    ElseIf Trim(temp_type) = "Value" Then
                                        temp_val = FormatNumber(temp_val, 0)
                                    ElseIf Trim(temp_type) = "Date" Then

                                    Else

                                    End If
                                End If

                                htmlOut.Append("<td class=""text_align_center"" ")

                                If Trim(temp_field_name) = "cliaircraft_ser_nbr" Then
                                    htmlOut.Append("data-sort=""" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), "") & """")
                                End If
                                htmlOut.Append(">")

                                'Setting up Next/Previous
                                ReDim Preserve arrayOfIDs(arrCounter)
                                arrayOfIDs(arrCounter) = r.Item("ac_id").ToString & "|" & r.Item("source")
                                arrCounter += 1

                                If DisplayLink = True Then
                                    If Trim(temp_field_name) = "cliaircraft_ser_nbr" Then
                                        'If r.Item("source").ToString = "JETNET" Then
                                        '  htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                    'Else
                                    '  htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("client_jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                    'End If

                                    If r.Item("source").ToString = "JETNET" Then
                                            htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id").ToString, 0, 0, 0, False, "", "underline", "&jid=0") & ">")
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        Else
                                            Dim JetnetForSaleCheck As New DataTable
                                            Dim NotForSaleJetnetSide As Boolean = False
                                            'This is where we need to add a check for client off market aircraft. 
                                            'On both the market summary view and the value view need to have a way of showing that an aircraft is an off market.
                                            'Recommend the following: on display of every client record in the listing check to see if there is a 
                                            'corresponding jetnet for sale record 
                                            '(select count(*) from aircraft where ac_id = #### and ac_journ_id = 0 and ac_forsale_flag=’Y’), 
                                            'if not then color the serial number red and bold it and modify the alt tag/mouseover to read as 
                                            '“Display Aircraft Details: JETNET shows this aircraft as off market.
                                            JetnetForSaleCheck = JetnetViewData.Check_Jetnet_Off_Market_Aircraft(r.Item("client_jetnet_ac_id"))
                                            If Not IsNothing(JetnetForSaleCheck) Then
                                                If JetnetForSaleCheck.Rows.Count > 0 Then
                                                    If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
                                                        NotForSaleJetnetSide = True
                                                    End If
                                                End If
                                            End If
                                            htmlOut.Append("<a " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id").ToString, 0, 0, 0, False, "", "underline", "&jid=0&source=" & r.Item("source").ToString))
                                            'htmlOut.Append("<a onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0&source=" & r.Item("source").ToString & """,""AircraftDetails"");'")

                                            If NotForSaleJetnetSide Then
                                                htmlOut.Append(" class='underline red_text' title='Display Aircraft Details: JETNET shows this aircraft as off market.'>")
                                            Else
                                                htmlOut.Append(" class='underline' title='Display Aircraft Details'>")
                                            End If

                                        End If
                                    End If
                                End If

                                If Trim(temp_field) = "AFTT" Then
                                    htmlOut.Append(temp_val)
                                ElseIf Trim(temp_field) = "ASKING$" Or Trim(temp_field) = "TAKE$" Or Trim(temp_field) = "EST$" Then
                                    If IsNumeric(temp_val) Then
                                        If CDbl(temp_val) > 0 Then
                                            Dim tempHold As Long = FormatNumber((CDbl(temp_val) / 1000), 0)
                                            htmlOut.Append("$")
                                            htmlOut.Append((FormatNumber(tempHold, 0).ToString))
                                        End If
                                    End If
                                Else
                                    If Trim(temp_type) = "Value" Then
                                        If IsNumeric(temp_val) Then
                                            If CDbl(temp_val) > 0 Then
                                                htmlOut.Append("$")
                                            End If
                                        End If
                                    End If

                                    'If IsNumeric(temp_val) And (format_me) Then
                                    If Trim(temp_type) = "Value" Then
                                        If CDbl(temp_val) > 0 Then
                                            htmlOut.Append(FormatNumber(temp_val, 0))
                                        End If
                                    Else
                                        htmlOut.Append(temp_val)
                                    End If

                                End If

                                If Trim(temp_field_name) = "cliaircraft_ser_nbr" And DisplayLink = True Then
                                    htmlOut.Append("</a>")
                                End If

                                htmlOut.Append("&nbsp;</td>")

                            Next
                            htmlOut.Append("</tr>")
                        Next
                    End If
                End If


            End If

            htmlOut.Append("</tbody></table>")

            'If searchCriteria.ViewCriteriaIsReport = False Then

            'End If
            If Not IsNothing(PassBackDataTable) Then 'we're passing this information back to view master. 
                PassBackDataTable = results_table
            End If
            'If DisplayLink Or Trim(order_by_string) = "" Then
            strOut.Append("<span id=""openNewWindowContents"">" + htmlOut.ToString() + "</span><div class=""resizeCW""><div id=""forSaleInnerTable"" style=""width:100%;""></div></div>")
            'Else

            'End If
            HttpContext.Current.Session("my_ids") = arrayOfIDs
            HttpContext.Current.Session("crmPagingParent") = "VIEW_TEMPLATE"

            string_for_export = strOut_Export.ToString
            searchCriteria.ViewCriteriaIsReport = orig_view

        Catch ex As Exception

            class_error = "Error in views_display_aircraft_forsale(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try


        out_htmlString = strOut.ToString
        htmlOut = Nothing
        strOut = Nothing
        results_table = Nothing

    End Sub

    Public Shared Function GetEstimates(ByRef ac_id As Long, ByVal REPORT_TYPE As String, ByVal get_details As Boolean, ByVal jetnet_ac_id As Long, ByVal note_id As Long, ByVal completed_or_open As String, ByVal include_snapshot As Boolean, ByVal localCriteria As viewSelectionCriteriaClass, Optional ByVal extra_criteria As String = "") As DataTable
        Dim atemptable As New DataTable
        Dim Query As String = ""

        Dim SqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim SqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim SqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim SqlException As MySql.Data.MySqlClient.MySqlException : SqlException = Nothing

        Dim sSeperator As String = ""

        Try

            Query = Query & "SELECT distinct clival_asking_price as asking_price, clival_est_price as take_price, clival_broker_price as sold_price, clival_entry_date as date_of   "


            If get_details Then
                If Trim(completed_or_open) <> "C" Then
                    Query = Query & ", clival_type as description, clival_jetnet_ac_id as ac_id, clival_aftt_hours, clival_total_landings "
                Else
                    Query = Query & ", clival_type as description "
                End If
            End If


            Query = Query & " , 'CLIENT' as Data_Source, '' as ac_asking "
            Query = Query & " , '" & Trim(LCase(REPORT_TYPE)) & "' as data_type "
            Query = Query & " , clival_id as clival_id "
            Query = Query & " , lnote_id as lnote_id "
            Query = Query & " FROM client_value_comparables "

            If include_snapshot = True Then
                Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('D') "
            Else
                Query = Query & " INNER JOIN LOCAL_NOTES on lnote_id = clival_note_id and lnote_status in ('D') "
            End If


            If Not IsNothing(localCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(localCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = localCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += crmWebClient.Constants.cCommaDelim + localCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next
                Query = Query & " WHERE (clival_jetnet_amod_id in (" & tmpStr & ")) "
            ElseIf localCriteria.ViewCriteriaAmodID > 0 Then
                Query = Query & " WHERE (clival_jetnet_amod_id = " & localCriteria.ViewCriteriaAmodID & ") "
            End If

            If Trim(extra_criteria) <> "" Then
                Query = Query & Trim(extra_criteria)
            End If



            Query = Query & " and lnote_Status = 'D' "
            Query = Query & " order by lnote_entry_date asc "


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", Query.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_my_ac_value_history load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in (ByRef ac_id As Long, ByVal REPORT_TYPE As String, ByVal get_details As Boolean, ByVal jetnet_ac_id As Long, ByVal note_id As Long, ByVal completed_or_open As String, ByVal include_snapshot As Boolean, ByVal localCriteria As viewSelectionCriteriaClass, Optional ByVal extra_criteria As String = "") As DataTable" + ex.Message

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

    Public Shared Sub show_last_items(ByRef htmlout1 As StringBuilder, ByVal price As Long, ByVal dateof As String, ByVal type_of As String)

        If Trim(type_of) = "JETNET" Then
            htmlout1.Append(DisplayFunctions.TextToImage("$" & FormatNumber((price / 1000), 0) & "k", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))
            htmlout1.Append("&nbsp;</td><td class=""text_align_center"">")
            htmlout1.Append("" & FormatDateTime(dateof, DateFormat.ShortDate))
        Else
            htmlout1.Append("$" & FormatNumber((price / 1000), 0) & "k")
            htmlout1.Append("&nbsp;</td><td class=""text_align_center"">")
            htmlout1.Append("" & FormatDateTime(dateof, DateFormat.ShortDate))
        End If

    End Sub

    Public Shared Function get_client_comp_id_by_name(ByVal comp_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery As String = ""

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing


        Try

            sQuery = "Select client_company.*,  "
            sQuery = sQuery & " (select clipnum_number from client_phone_numbers where clipnum_comp_id = clicomp_id and clipnum_contact_id = 0 "
            sQuery = sQuery & " and clipnum_type='Office' LIMIT 1) as comp_phone_office,  "
            sQuery = sQuery & " (select clipnum_number from client_phone_numbers where clipnum_comp_id = clicomp_id and clipnum_contact_id = 0  "
            sQuery = sQuery & " and clipnum_type='Fax' LIMIT 1) as comp_phone_fax  "
            sQuery = sQuery & " from client_company "
            sQuery = sQuery & " where clicomp_id = " & comp_id.ToString & " "

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
            MySqlConnection.Open()
            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_client_comp_id_by_name load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_ac_based_on_location(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing


        Return atemptable

    End Function
    Public Shared Function get_client_company_name_by_id(ByVal comp_id As Long, Optional ByVal no_columns As Boolean = False) As String
        get_client_company_name_by_id = ""
        Dim CompanyName As String = ""
        Dim CompanyPhone As String = ""
        Dim results_table As New DataTable
        Dim searchCriteria As New viewSelectionCriteriaClass


        results_table = Get_Client_Owner_Info(searchCriteria, comp_id)

        If Not IsNothing(results_table) Then
            If results_table.Rows.Count > 0 Then



                For Each r As DataRow In results_table.Rows


                    CompanyName = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
                    CompanyPhone = IIf(Not IsDBNull(r("comp_phone_office")), "PH: " & r("comp_phone_office"), "PH: Not Listed")

                    If no_columns = False Then
                        get_client_company_name_by_id += "<td align=""left"" valign=""top"">"
                    End If


                    get_client_company_name_by_id += "<span><span class='label'><span title='" & CompanyPhone & "'>" & DisplayFunctions.WriteDetailsLink(0, r("clicomp_jetnet_comp_id"), 0, 0, True, CompanyName, "help_cursor", "") & "</span></span>" ' <span class='tiny'>" & CompanyLocation & "</span>"

                    If no_columns = False Then
                        get_client_company_name_by_id += "</td>"
                    End If


                Next

            Else
                If no_columns = False Then
                    get_client_company_name_by_id += "<td align=""left"" valign=""top"">"
                    get_client_company_name_by_id += "&nbsp;</td>"
                End If

            End If
        Else
            If no_columns = False Then
                get_client_company_name_by_id += "<td align=""left"" valign=""top"">"
                get_client_company_name_by_id += "&nbsp;</td>"
            End If
        End If







    End Function

    Public Shared Function Get_Market_Snapshot_Count(ByVal date_start As String, ByRef google_map_string As String, ByVal jetnet_amod_id As Long) As Boolean

        Get_Market_Snapshot_Count = False

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim sQuery As New StringBuilder
        Dim temp_data As String = ""
        Dim temp_count As String = ""
        Dim added_first As Boolean = False


        sQuery.Append("SELECT DISTINCT date( lnote_action_date ) as tdate , count( * ) AS tcount ")
        sQuery.Append("FROM local_notes ")
        sQuery.Append("INNER JOIN client_value_comparables ON lnote_id = clival_note_id ")
        sQuery.Append("WHERE lnote_status = 'S' ")
        sQuery.Append(" AND lnote_action_date >= '" & date_start & "' ")
        sQuery.Append(" AND lnote_jetnet_amod_id = '" & jetnet_amod_id & "' ")
        sQuery.Append("GROUP BY date( lnote_action_date ) ")
        sQuery.Append("ORDER BY date( lnote_action_date ) ")

        Try

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                google_map_string = " data1.addColumn('string', 'Date'); "
                google_map_string &= " data1.addColumn('number', '# For Sale'); "
                google_map_string &= " data1.addRows(["
                Get_Market_Snapshot_Count = True

                Do While MySqlReader.Read()

                    temp_data = ""
                    If Not IsDBNull(MySqlReader.Item("tdate")) Then
                        temp_data = MySqlReader.Item("tdate")
                    End If

                    If Not IsDBNull(MySqlReader.Item("tcount")) Then
                        temp_count = MySqlReader.Item("tcount")
                    End If


                    If added_first = True Then
                        google_map_string &= ",['" & temp_data & "'," & temp_count & "]"
                    Else
                        google_map_string &= "['" & temp_data & "'," & temp_count & "]"
                    End If

                    added_first = True
                Loop

            End If

            MySqlReader.Close()

        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

            class_error = "Error in Get_Client_Company_Phone(ByVal in_CompanyID As Long, ByVal bGetFirstNumber As Boolean) As String " + ex.Message
        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing

    End Function

    Public Shared Function Get_Market_Snapshot_Datatable(ByVal date_start As String, ByVal jetnet_amod_id As Long) As DataTable

        Get_Market_Snapshot_Datatable = Nothing

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim sQuery As New StringBuilder
        Dim temp_data As String = ""
        Dim temp_count As String = ""
        Dim added_first As Boolean = False
        Dim atemptable As New DataTable


        sQuery.Append("select distinct Date(lnote_action_date) as tdate, clival_ser_nbr,  ")
        sQuery.Append(" clival_asking_price, ")
        sQuery.Append(" clival_est_price, clival_broker_price ")
        sQuery.Append(" from local_notes  ")
        sQuery.Append(" inner join client_value_comparables on lnote_id = clival_note_id ")
        sQuery.Append(" where lnote_status='S' ")
        sQuery.Append(" and lnote_action_date >= '" & date_start & "'")
        sQuery.Append(" and  clival_jetnet_amod_id = " & jetnet_amod_id & "  ")
        sQuery.Append(" order by Date(lnote_action_date), clival_ser_nbr_sort, clival_ser_nbr  ")


        Try

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                ' aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

            Get_Market_Snapshot_Datatable = atemptable

        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

            class_error = "Error in Get_Market_Snapshot_Datatable(ByVal date_start As String, ByVal jetnet_amod_id As Long) As DataTable " + ex.Message
        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing

    End Function


    ''' <summary>
    ''' This function is used within the for sale tab of the model view to get the company phone numbers.
    ''' </summary>
    ''' <param name="in_CompanyID"></param>
    ''' <param name="bGetFirstNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Get_Client_Company_Phone(ByVal in_CompanyID As Long, ByVal bGetFirstNumber As Boolean) As String

        Dim MySqlConnection As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim sCompanyPhone As String = ""

        Dim sQuery As New StringBuilder

        If Not bGetFirstNumber Then
            sQuery.Append("SELECT clipnum_number as pnum_number_full, clipnum_type as pnum_type FROM client_phone_numbers INNER JOIN client_Phone_Type ON cliptype_name = clipnum_type")
            sQuery.Append(" WHERE clipnum_comp_id = " + in_CompanyID.ToString)
            sQuery.Append(" AND clipnum_contact_id = 0 ")
        Else
            sQuery.Append("SELECT clipnum_number as pnum_number_full FROM client_phone_numbers INNER JOIN client_Phone_Type ON cliptype_name = clipnum_type")
            sQuery.Append(" WHERE clipnum_comp_id = " + in_CompanyID.ToString)
            sQuery.Append(" AND clipnum_contact_id = 0 ")
            sQuery.Append(" ORDER BY cliptype_seq_no ASC limit 1")
        End If

        Try

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConnection.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            MySqlConnection.Open()

            MySqlCommand.Connection = MySqlConnection
            MySqlCommand.CommandTimeout = 1000
            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                Do While MySqlReader.Read()

                    If Not bGetFirstNumber Then

                        If Not String.IsNullOrEmpty(sCompanyPhone) Then
                            sCompanyPhone += "<br />"
                        End If

                        If Not IsDBNull(MySqlReader.Item("pnum_type")) Then
                            If Not String.IsNullOrEmpty(MySqlReader.Item("pnum_type").ToString.Trim) Then
                                sCompanyPhone = MySqlReader.Item("pnum_type").ToString.Trim + " : "
                            End If
                        End If
                        If Not IsDBNull(MySqlReader.Item("pnum_number_full")) Then
                            If Not String.IsNullOrEmpty(MySqlReader.Item("pnum_number_full").ToString.Trim) Then
                                sCompanyPhone += MySqlReader.Item("pnum_number_full").ToString.Trim
                            End If
                        End If

                    Else

                        If Not IsDBNull(MySqlReader.Item("pnum_number_full")) Then
                            If Not String.IsNullOrEmpty(MySqlReader.Item("pnum_number_full").ToString.Trim) Then
                                sCompanyPhone += MySqlReader.Item("pnum_number_full").ToString.Trim
                            End If
                        End If

                    End If


                Loop

            End If

            MySqlReader.Close()

        Catch ex As Exception
            MySqlConnection.Dispose()
            MySqlCommand.Dispose()

            Return Nothing

            class_error = "Error in Get_Client_Company_Phone(ByVal in_CompanyID As Long, ByVal bGetFirstNumber As Boolean) As String " + ex.Message
        Finally

            MySqlCommand.Dispose()
            MySqlConnection.Close()
            MySqlConnection.Dispose()

        End Try

        MySqlReader = Nothing
        MySqlCommand = Nothing
        MySqlConnection = Nothing

        Return sCompanyPhone

    End Function

    ''' <summary>
    ''' This function is used within the for sale tab of the model view to get the owner/operator/exclusive broker information.
    ''' </summary>
    ''' <param name="searchCriteria"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function Get_Client_Owner_Info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal byID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT clicomp_id as comp_id, clicomp_jetnet_comp_id , clicomp_name as comp_name, clipnum_number as comp_phone_office FROM client_company ")
            sQuery.Append(" INNER JOIN client_aircraft_reference ON (clicomp_id = cliacref_comp_id)")
            sQuery.Append(" left outer join client_phone_numbers on clicomp_id = clipnum_comp_id and clipnum_contact_id = 0 ")
            sQuery.Append(" left outer join client_Phone_Type ON cliptype_name = clipnum_type ")

            '  sQuery.Append(" LEFT OUTER JOIN client_contact ON (cliacref_contact_id = clicontact_id )")

            sQuery.Append("WHERE ")
            If byID = 0 Then
                sQuery.Append(" (cliacref_cliac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + ")")
                If searchCriteria.ViewCriteriaGetExclusive Then
                    sQuery.Append(crmWebClient.Constants.cAndClause + "((cliacref_contact_type = '99') OR (cliacref_contact_type = '93') )")
                ElseIf searchCriteria.ViewCriteriaGetOperator Then
                    sQuery.Append(crmWebClient.Constants.cAndClause + "(cliacref_operator_flag in ('Y','O'))")
                Else
                    sQuery.Append(crmWebClient.Constants.cAndClause + " (cliacref_contact_type = '00') ")
                End If
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(crmWebClient.Constants.cAndClause + "clicomp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If
            Else
                sQuery.Append("clicomp_id = " + byID.ToString)
            End If




            sQuery.Append(crmWebClient.Constants.cAndClause + "clicomp_status = 'Y' ORDER BY cliptype_seq_no limit 1")



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)


            MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                ' aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            class_error = "Error in Get_Client_Owner_Info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Shared Function ReturnCompanyPhoneFax(ByVal companyOffice As Object, ByVal companyFax As Object) As String
        Dim sCompanyPhone As String = ""

        'Checking for office phone.
        If Not IsDBNull(companyOffice) Then
            If Not String.IsNullOrEmpty(companyOffice) Then
                sCompanyPhone = companyOffice.ToString
            End If
        End If
        'checking for fax.
        If String.IsNullOrEmpty(sCompanyPhone) Then
            If Not IsDBNull(companyFax) Then
                If Not String.IsNullOrEmpty(companyFax) Then
                    sCompanyPhone = companyFax.ToString
                End If
            End If
        End If

        Return sCompanyPhone
    End Function
    Public Shared Function CLIENTGetOwnerExclusiveOperatorInformation(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing


        Try

            sQuery.Append("SELECT clitcomp_name AS comp_name, clitcomp_id AS comp_id, clitpnum_number AS comp_phone_office, '' AS comp_phone_fax ")
            sQuery.Append("FROM client_transactions ")
            sQuery.Append("INNER JOIN client_transactions_aircraft_reference ON clitcref_client_trans_id = clitrans_id ")
            sQuery.Append("INNER JOIN client_transactions_company ON clitcomp_id = clitcref_client_comp_id ")
            sQuery.Append("LEFT OUTER JOIN client_transactions_phone_numbers ON clitcomp_id = clitpnum_comp_id ")
            sQuery.Append("AND clitrans_id = clitpnum_trans_id and clitpnum_type = 'Office' ")
            sQuery.Append("WHERE clitrans_jetnet_trans_id = " + searchCriteria.ViewCriteriaJournalID.ToString)
            If searchCriteria.ViewCriteriaGetExclusive Then
                sQuery.Append(Constants.cAndClause + " ((clitcref_contact_type = '99') OR (clitcref_contact_type = '93'))")
            ElseIf searchCriteria.ViewCriteriaGetOperator Then
                sQuery.Append(Constants.cAndClause + " (clitcref_operator_flag in ('Y', 'O'))")
            Else
                sQuery.Append(Constants.cAndClause + " (clitcref_contact_type in ('00','97','08','56','95'))")
            End If




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'clientConnectString

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                ' aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in CLIENTGetOwnerExclusiveOperatorInformation(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function
    Public Shared Function GetOwnerExclusiveOperatorInformation(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT TOP 1 comp_name, comp_id, comp_phone_office, comp_phone_fax FROM Aircraft_Company_Flat WITH(NOLOCK) ")
            sQuery.Append(" WHERE (cref_ac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + " AND cref_journ_id = " + searchCriteria.ViewCriteriaJournalID.ToString)

            If searchCriteria.ViewCriteriaGetExclusive Then
                sQuery.Append(Constants.cAndClause + "((cref_contact_type = '99') OR (cref_contact_type = '93') OR (cref_transmit_seq_no = 4))")
            ElseIf searchCriteria.ViewCriteriaGetOperator Then
                sQuery.Append(Constants.cAndClause + "(cref_operator_flag in ('Y', 'O'))")
            Else
                sQuery.Append(Constants.cAndClause + "cref_transmit_seq_no = 1 AND cref_contact_type <> '71'")
            End If

            If searchCriteria.ViewCriteriaJournalID = 0 Then
                sQuery.Append(Constants.cAndClause + "comp_active_flag = 'Y'")
            End If

            sQuery.Append(")")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
                aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in GetOwnerExclusiveOperatorInformation(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Shared Function GetOwnerExclusiveOperatorInformation_Multiple(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT distinct top 1 comp_name, comp_id, comp_phone_office, comp_phone_fax FROM Aircraft_Company_Flat WITH(NOLOCK) ")
            sQuery.Append(" WHERE (cref_ac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString + " AND cref_journ_id = " + searchCriteria.ViewCriteriaJournalID.ToString)

            If searchCriteria.ViewCriteriaGetExclusive Then
                sQuery.Append(Constants.cAndClause + "((cref_contact_type in ('99','93')) OR (cref_transmit_seq_no in (4,5)))")
            ElseIf searchCriteria.ViewCriteriaGetOperator Then
                sQuery.Append(Constants.cAndClause + "(cref_operator_flag in ('Y', 'O'))")
            Else
                sQuery.Append(Constants.cAndClause + "cref_transmit_seq_no = 1 AND cref_contact_type <> '71'")
            End If

            If searchCriteria.ViewCriteriaJournalID = 0 Then
                sQuery.Append(Constants.cAndClause + "comp_active_flag = 'Y'")
            End If

            sQuery.Append(")")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
                aError = "Error in get_owner_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in GetOwnerExclusiveOperatorInformation(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Shared Function get_client_model_forsale_info_single_ac(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal client_string As String, ByVal order_by_string As String, Optional ByRef ClientExtraCriteria As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing


        Try


            sQuery.Append("  select clitrans_sold_price as LASTSALEPRICE, clitrans_date as LASTSALEPRICEDATE from client_transactions ")
            sQuery.Append("  where  clitrans_sold_price > 0 ")
            sQuery.Append(" and clitrans_type = 'Full Sale' ")
            sQuery.Append(" AND clitrans_internal_trans_flag= 'N'  ")
            sQuery.Append(" and clitrans_retail_flag='Y' ")
            sQuery.Append(ClientExtraCriteria)
            sQuery.Append(" order by clitrans_date desc LIMIT 1 ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            'This will need to be changed to crmClientConnectString, as will all the other references in this class, however, not at that stage yet
            MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                class_error = "Error in get_client_model_forsale_info_single_ac load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            class_error = "Error in get_client_model_forsale_info_single_ac(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function
    ''' <summary>
    ''' This function gets the client for sale information for the for sale tab of the model view
    ''' </summary>
    ''' <param name="searchCriteria"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function get_client_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal client_string As String, ByVal order_by_string As String, Optional ByRef ClientExtraCriteria As String = "", Optional ByVal displayEValues As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing


        Try

            If Trim(client_string) = "" Then
                sQuery.Append("SELECT ")
                'If displayEValues Then
                '  sQuery.Append("0 as EVALUE, 0 as AVGMODYREVALUE, ")
                'End If
                sQuery.Append("cliaircraft_id as ac_id,  emp_program_name, cliaircraft_aport_country as ac_aport_country, cliaircraft_aport_city as ac_aport_city, cliaircraft_reg_nbr as ac_reg_no, cliaircraft_delivery as ac_delivery, cliaircraft_exclusive_flag as ac_exclusive_flag, cliaircraft_est_price as ac_take_price, cliaircraft_ser_nbr as ac_ser_no_full, cliaircraft_ser_nbr_sort as ac_ser_no_sort, ")
                sQuery.Append(" cliaircraft_year_dlv as ac_year, cliaircraft_year_mfr as ac_mfr_year, cliaircraft_airframe_total_hours as ac_airframe_tot_hrs, ")
                sQuery.Append(" cliacep_engine_1_ttsn_hours as ac_engine_1_tot_hrs, cliacep_engine_2_ttsn_hours as ac_engine_2_tot_hrs, ")
                sQuery.Append(" cliacep_engine_3_ttsn_hours as ac_engine_3_tot_hrs, cliacep_engine_4_ttsn_hours as ac_engine_4_tot_hrs, ")
                sQuery.Append(" cliaircraft_interior_month_year as ac_interior_moyear,  cliaircraft_exterior_month_year as ac_exterior_moyear,")
                sQuery.Append(" cliaircraft_date_listed as ac_list_date, cliaircraft_status as ac_status, cliaircraft_asking_wordage as ac_asking, ")
                sQuery.Append(" cliaircraft_asking_price as  ac_asking_price, cliaircraft_passenger_count as ac_passenger_count, 0 as ac_journ_id, cliamod_make_name as amod_make_name, 'CLIENT' as source, ")
                sQuery.Append(" cliamod_model_name as amod_model_name, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, ")
                sQuery.Append(" cliaircraft_broker_price as sold_price, cliaircraft_cliamod_id as client_model_id ")

                sQuery.Append(", Case when cliacep_engine_1_tsoh_hours Is null then 0 else cliacep_engine_1_tsoh_hours end as ac_engine_1_soh_hrs ")
                sQuery.Append(", Case when cliacep_engine_2_tsoh_hours Is null then 0 else cliacep_engine_2_tsoh_hours end as ac_engine_2_soh_hrs ")
                sQuery.Append(", '' as country_continent_name ")
                sQuery.Append(", cliaircraft_value_description ")
                '  If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True   Then

                sQuery.Append(", (select  clicomp_jetnet_comp_id  FROM  client_company ")
                sQuery.Append(" INNER JOIN client_aircraft_reference ON  clicomp_id =  cliacref_comp_id  ")
                sQuery.Append(" WHERE  cliacref_cliac_id =  cliaircraft_id  AND  cliacref_contact_type IN ('99','93','00', '08')  ")
                sQuery.Append(" ORDER BY cliacref_contact_type DESC LIMIT 1 ) AS displaycompany")




                sQuery.Append(", (select clitrans_sold_price from client_transactions ")
                sQuery.Append("  where(clitrans_cliac_id = cliaircraft_id And clitrans_sold_price > 0) ")
                sQuery.Append(" and clitrans_type = 'FUll Sale' ")
                sQuery.Append(" AND clitrans_internal_trans_flag='N'  ")
                sQuery.Append(" and clitrans_retail_flag='Y' ")
                sQuery.Append(" order by clitrans_date desc limit 1) as LASTSALEPRICE, ")

                sQuery.Append(" (select clitrans_date from client_transactions ")
                sQuery.Append(" where(clitrans_cliac_id = cliaircraft_id And clitrans_sold_price > 0) ")
                sQuery.Append(" and clitrans_type = 'Full Sale' ")
                sQuery.Append(" AND clitrans_internal_trans_flag='N'  ")
                sQuery.Append(" and clitrans_retail_flag='Y' ")
                sQuery.Append(" order by clitrans_date desc limit 1) as LASTSALEPRICEDATE ")


                ' sQuery.Append(", ac_est_airframe_hrs  ")
                'End If

                sQuery.Append(" FROM client_aircraft inner join client_aircraft_model ON cliaircraft_cliamod_id = cliamod_id ")

                sQuery.Append(" LEFT OUTER JOIN client_aircraft_engine ON client_aircraft.cliaircraft_id = client_aircraft_engine.cliacep_cliac_id")
                sQuery.Append(" LEFT OUTER JOIN engine_maintenance_program ON client_aircraft_engine.cliacep_engine_maintenance_program = engine_maintenance_program.emp_id ")

                'sQuery.Append(" left outer join client_aircraft_engine on cliaircraft_id = cliacep_cliac_id")

            Else

                sQuery.Append("SELECT distinct cliaircraft_id as ac_id, cliaircraft_est_price as ac_take_price, cliaircraft_ser_nbr as ac_ser_no_full, cliaircraft_ser_nbr_sort as ac_ser_no_sort, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, ")

                If displayEValues Then
                    sQuery.Append("0 as EVALUE, 0 as AVGMODYREVALUE, ")
                End If

                'If InStr(client_string, "cliaircraft_id as ac_id") > 0 Then
                '  client_string = Replace(Trim(client_string), "cliaircraft_id as ac_id", "")
                'ElseIf InStr(client_string, "cliaircraft_est_price as ac_take_price") > 0 Then
                '  client_string = Replace(Trim(client_string), "cliaircraft_est_price as ac_take_price", "")
                'ElseIf InStr(client_string, "cliaircraft_ser_nbr as ac_ser_no_full") > 0 Then
                '  client_string = Replace(Trim(client_string), "cliaircraft_ser_nbr as ac_ser_no_full", "")
                'ElseIf InStr(client_string, "cliaircraft_jetnet_ac_id as client_jetnet_ac_id") > 0 Then
                '  client_string = Replace(Trim(client_string), "cliaircraft_jetnet_ac_id as client_jetnet_ac_id", "")
                'End If

                'If InStr(Trim(client_string), ",,") > 0 Or InStr(Trim(client_string), ", ,") > 0 Then
                '  client_string = Replace(Trim(client_string), ",,", "")
                '  client_string = Replace(Trim(client_string), ", ,", "")
                'End If


                sQuery.Append(client_string)
                sQuery.Append(", 'CLIENT' as source ")
                sQuery.Append(" from client_aircraft ")
                sQuery.Append(" inner JOIN client_aircraft_model ON client_aircraft.cliaircraft_cliamod_id = client_aircraft_model.cliamod_id ")
                sQuery.Append(" LEFT OUTER JOIN client_aircraft_reference ON client_aircraft_reference.cliacref_cliac_id = client_aircraft.cliaircraft_id ")
                sQuery.Append(" left outer join client_aircraft_engine on client_aircraft.cliaircraft_id=cliacep_cliac_id ")
                sQuery.Append(" left outer join client_contact on client_aircraft_reference.cliacref_contact_id=clicontact_id ")
                sQuery.Append(" LEFT OUTER JOIN client_company on client_aircraft_reference.cliacref_comp_id = client_company.clicomp_id ")
                sQuery.Append(" LEFT OUTER JOIN client_aircraft_contact_type ON client_aircraft_reference.cliacref_contact_type = client_aircraft_contact_type.cliact_type")

            End If


            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE cliamod_jetnet_amod_id IN (" + tmpStr.Trim + ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " ")
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(" WHERE cliamod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' ")
                End If
            End If

            If ClientExtraCriteria <> "" Then
                sQuery.Append(ClientExtraCriteria)
            End If

            sQuery.Append(" AND cliaircraft_forsale_flag = 'Y'")

            If Trim(client_string) = "" Then
                Select Case (searchCriteria.ViewCriteriaSortBy.ToLower)
                    Case "serno"
                        sQuery.Append(" ORDER BY cliaircraft_ser_nbr_sort, cliaircraft_date_listed, cliaircraft_airframe_total_hours, cliaircraft_year_mfr, cliaircraft_year_dlv, cliaircraft_asking_price desc, cliaircraft_asking_wordage asc")

                    Case "aftt"
                        sQuery.Append(" ORDER BY cliaircraft_airframe_total_hours, cliaircraft_ser_nbr_sort, cliaircraft_date_listed, cliaircraft_year_mfr, cliaircraft_year_dlv, cliaircraft_asking_price desc, cliaircraft_asking_wordage asc")

                    Case "mfryear"
                        sQuery.Append(" ORDER BY cliaircraft_year_mfr, cliaircraft_ser_nbr_sort, cliaircraft_date_listed, cliaircraft_airframe_total_hours, cliaircraft_year_dlv, cliaircraft_asking_price desc, cliaircraft_asking_wordage asc")

                    Case "acyear"
                        sQuery.Append(" ORDER BY cliaircraft_year_dlv, cliaircraft_ser_nbr_sort, cliaircraft_date_listed, cliaircraft_airframe_total_hours, cliaircraft_year_mfr, cliaircraft_asking_price desc, cliaircraft_asking_wordage asc")

                    Case "listdate"
                        sQuery.Append(" ORDER BY cliaircraft_date_listed, cliaircraft_ser_nbr_sort, cliaircraft_airframe_total_hours, cliaircraft_year_mfr, cliaircraft_year_dlv, cliaircraft_asking_price, cliaircraft_asking_wordage asc")

                    Case "asking"
                        sQuery.Append(" ORDER BY cliaircraft_asking_price desc, cliaircraft_asking_wordage asc, cliaircraft_date_listed, cliaircraft_ser_nbr_sort, cliaircraft_airframe_total_hours, cliaircraft_year_mfr, cliaircraft_year_dlv")

                    Case Else
                        sQuery.Append(" ORDER BY cliaircraft_ser_nbr_sort, cliaircraft_date_listed, cliaircraft_airframe_total_hours,cliaircraft_year_mfr, cliaircraft_year_dlv, cliaircraft_asking_price desc, cliaircraft_asking_wordage asc")

                End Select
            Else
                sQuery.Append(" order by " & order_by_string)
            End If


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

            'If Trim(HttpContext.Current.Application.Item("crmClientDatabase")) = "" Then
            '  If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            '    MySqlConn.ConnectionString = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
            '  Else
            '    MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
            '  End If
            'Else
            '  'This will need to be changed to crmClientConnectString, as will all the other references in this class, however, not at that stage yet
            '  MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'crmClientConnectString
            'End If

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                class_error = "Error in Client_get_model_forsale_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            class_error = "Error in Client_get_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function
    Public Shared Function CheckForProspectorsTab(ByVal AircraftID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal NOTE_ID As Long, ByVal source As String, ByVal open_or_closed As String, ByVal COMPARE_BASE_AC_ID As Long, ByVal trans_id As Long, ByVal completed_date As String, ByVal jetnet_Ac_id_ifclient As Long) As String

        Dim TemporaryTable As New DataTable
        Dim client_ac_id As Long = 0
        Dim ReturnString As String = ""
        Dim temp_data_layer As New viewsDataLayer
        'If the crm View is active (viewing the full model view from the 


        If client_ac_id = 70 Or AircraftID = 70 Then
            client_ac_id = client_ac_id
        End If


        client_ac_id = AircraftID


        'Set up connection

        If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            If Trim(HttpContext.Current.Application.Item("crmClientDatabase")) = "" Then
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
            Else
                aclsData_Temp.client_DB = HttpContext.Current.Application.Item("crmClientDatabase")
                'crmClientConnectString
            End If
        Else
            aclsData_Temp.client_DB = HttpContext.Current.Application.Item("crmClientDatabase")
            'crmClientConnectString
        End If




        If UCase(Trim(source)) <> "CLIENT" Then
            ' right now only showing for client records
            ' client_ac_id = temp_data_layer.Get_JETNET_AC_ID_FROM_CLIENT(AircraftID, True)

            ' ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=8&type_of=add&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&ac_type=JETNET&direct=Y' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"
            ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=add&compare_ac_id=0&jac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&ac_type=JETNET&direct=Y' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"
            ReturnString += "<img src='images/addcompare.png' width='16' border='0'>"
            ReturnString += "</a>"
            'ReturnString += "&nbsp;"

        Else
            client_ac_id = AircraftID

            ' if its completed --------------

            'Use CRM Connection
            If client_ac_id = 0 Then
                TemporaryTable = aclsData_Temp.Find_Client_Analysis_Note(client_ac_id, NOTE_ID, open_or_closed, jetnet_Ac_id_ifclient)

                If Not IsNothing(TemporaryTable) Then
                    If TemporaryTable.Rows.Count > 0 Then
                        If Trim(completed_date) = "" Then
                            ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&id=" & TemporaryTable.Rows(0).Item("clival_id") & "&type_of=remove&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & TemporaryTable.Rows(0).Item("clival_clitrans_id") & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Delete this client aircraft as a current market comparable' title='Delete this client aircraft as a current market comparable'>"
                        End If
                        'ReturnString += "<span style=""display:block;background-image:url('images/value.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;"" title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "'>&nbsp;</span>"
                        ReturnString += "<img src='images/value.png' width='16' title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "'>"
                        If Trim(completed_date) = "" Then
                            ReturnString += "</a>"
                        End If
                    Else
                        If Trim(completed_date) = "" Then
                            ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=add&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"

                            ReturnString += "<img src='images/addcompare.png' width='16' border='0'>"
                            ' ReturnString += "<span style=""display:block;background-image:url('images/addcompare.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;""'>&nbsp;</span>"

                            ReturnString += "</a>"
                        End If
                    End If
                Else
                    If Trim(completed_date) = "" Then
                        ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=add&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"

                        ReturnString += "<img src='images/addcompare.png' width='16' border='0'>"
                        ' ReturnString += "<span style=""display:block;background-image:url('images/addcompare.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;""'>&nbsp;</span>"

                        ReturnString += "</a>"
                    End If
                End If


            Else
                TemporaryTable = aclsData_Temp.Find_Client_Analysis_Note(client_ac_id, NOTE_ID, open_or_closed, 0)


                If Not IsNothing(TemporaryTable) Then
                    If TemporaryTable.Rows.Count > 0 Then

                        If CDbl(COMPARE_BASE_AC_ID) = CDbl(TemporaryTable.Rows(0).Item("clival_client_ac_id")) Then
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("clival_value_description")) Then
                                ReturnString += "<table cellspacing='0' cellpadding='0' border='0' align=""center""><tr valign='top'><td class=""override_borders"">"
                                If Trim(completed_date) = "" Then
                                    ReturnString += "<a href='#' alt='My current aircraft - already part of this analysis.' title='My current aircraft - already part of this analysis.'>"
                                End If
                                ReturnString += "<img src='images/value.png' width='16' title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "' >"
                                ReturnString += "</a></td>" '<td>"
                                ' ReturnString += "<a href='#' alt='My current aircraft - already part of this analysis.' title='My current aircraft - already part of this analysis.'>"
                                'ReturnString += "<img src='images/value.png' width='15' title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "' >"
                                'ReturnString += "</a></td>
                                ReturnString += "</tr></table>"
                                ReturnString += "</a>"
                            Else
                                ReturnString += "<table cellspacing='0' cellpadding='0' border='0' align=""center""><tr valign='top'><td class=""override_borders"">"
                                If Trim(completed_date) = "" Then
                                    ReturnString += "<a href='#' alt='My current aircraft - already part of this analysis.' title='My current aircraft - already part of this analysis.'>"
                                End If
                                ReturnString += "<img src='images/current_value.png' width='16' title='' >"
                                ReturnString += "</a></td>" '<td>"
                                ' ReturnString += "<a href='#' alt='My current aircraft - already part of this analysis.' title='My current aircraft - already part of this analysis.'>"
                                ' ReturnString += "<img src='images/value.png' width='15' title='' >"
                                'ReturnString += "</a></td>"
                                ReturnString += "</tr></table>"
                                ReturnString += "</a>"
                            End If
                        Else
                            If Not IsDBNull(TemporaryTable.Rows(0).Item("clival_value_description")) Then
                                If Trim(completed_date) = "" Then
                                    ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&id=" & TemporaryTable.Rows(0).Item("clival_id") & "&type_of=remove&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & TemporaryTable.Rows(0).Item("clival_clitrans_id") & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Delete this client aircraft as a current market comparable' title='Delete this client aircraft as a current market comparable'>"
                                End If
                                'ReturnString += "<span style=""display:block;background-image:url('images/value.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;"" title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "'>&nbsp;</span>"
                                ReturnString += "<img src='images/value.png' width='16' title='" & TemporaryTable.Rows(0).Item("clival_value_description") & "'>"
                                If Trim(completed_date) = "" Then
                                    ReturnString += "</a>"
                                End If
                            Else
                                If Trim(completed_date) = "" Then
                                    ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=remove&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & TemporaryTable.Rows(0).Item("clival_clitrans_id") & "' alt='Delete this client aircraft as a current market comparable' title='Delete this client aircraft as a current market comparable'>"
                                End If
                                ReturnString += "<img src='images/value.png' width='16'>"
                                If Trim(completed_date) = "" Then
                                    ReturnString += "</a>"
                                End If
                            End If
                        End If
                    Else
                        If Trim(completed_date) = "" Then
                            ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=add&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"

                            ReturnString += "<img src='images/addcompare.png' width='16' border='0'>"
                            ' ReturnString += "<span style=""display:block;background-image:url('images/addcompare.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;""'>&nbsp;</span>"

                            ReturnString += "</a>"
                        End If
                    End If
                Else
                    If Trim(completed_date) = "" Then
                        ReturnString += "<a href='view_template.aspx?ViewID=19&ViewName=Market Value Analysis&extra=false&noteID=" & NOTE_ID & "&activetab=9&type_of=add&compare_ac_id=" & AircraftID & "&sold_current=" & open_or_closed & "&trans_id=" & trans_id & "&jac_id=" & jetnet_Ac_id_ifclient & "' alt='Add this client aircraft as a current market comparable' title='Add this client aircraft as a current market comparable'>"

                        ReturnString += "<img src='images/addcompare.png' width='16' border='0'>"
                        ' ReturnString += "<span style=""display:block;background-image:url('images/addcompare.png');background-repeat:no-repeat;background-position:top center;padding:5px;width:20px;cursor:help;"">&nbsp;</span>"

                        ReturnString += "</a>"
                    End If
                End If
            End If

            TemporaryTable.Dispose()
        End If




        Return ReturnString
    End Function

    ''' <summary>
    ''' This function checks for notes on the forsale tab.
    ''' </summary>
    ''' <param name="CRMViewActive"></param>
    ''' <param name="source"></param>
    ''' <param name="AircraftID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckForNotesForSaleTab(ByVal CRMViewActive As Boolean, ByVal source As String, ByVal AircraftID As Long, ByVal aclsData_Temp As clsData_Manager_SQL) As String

        Dim TemporaryTable As New DataTable
        Dim ReturnString As String = ""
        'If the crm View is active (viewing the full model view from the 
        If CRMViewActive Then
            'Set up connection
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")

            'Use CRM Connection
            TemporaryTable = aclsData_Temp.DUAL_Notes_LIMIT("AC", AircraftID, "A", source, "", "lnote_entry_date desc", 1)

            If Not IsNothing(TemporaryTable) Then
                If TemporaryTable.Rows.Count > 0 Then
                    'This will naturally show up on the export, as styles are dropped in excel.
                    ReturnString = "<span id='note_text_replace' style=""display:none;"" title='" & TemporaryTable.Rows(0).Item("lnote_note") & "'>" & TemporaryTable.Rows(0).Item("lnote_note").ToString & "</span>"
                    'This will not show up on the export, as styles are dropped in excel.
                    ReturnString += "<span class=""notePlusBlock"" style=""background-image:url('images/document.png');"" title='" & TemporaryTable.Rows(0).Item("lnote_note") & "'> </span>"
                End If
            End If
        Else
            'Evolution view, check for server notes
            'check for standard notes
            'check for cloud notes, doesn't run the function if not.
            If (HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True) Then
                ReturnString = crmWebClient.DisplayFunctions.BuildNote(AircraftID, aclsData_Temp, "AC")
            End If

        End If
        TemporaryTable.Dispose()

        Return ReturnString
    End Function


#End Region

#Region "Retail Sales Tab"

    Public Shared Sub Combined_views_display_recent_retail_sales(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef JetnetDataLayer As viewsDataLayer, ByRef CRMViewActive As Boolean, ByVal DisplayLink As Boolean, ByRef table_to_return As DataTable, ByVal is_internal As String, ByVal is_retail As String, ByVal is_excel As Boolean, ByVal NOTE_ID As Long, ByVal REAL_AC_ID As Long, ByVal LAST_SAVE_DATE As String, Optional ByRef ActiveTabIndexInteger As Integer = 0, Optional ByVal page_break_after As Integer = 0, Optional ByVal header_text As String = "", Optional ByVal is_word As Boolean = True, Optional ByVal is_company_logo As String = "", Optional ByVal jetnet_string As String = "", Optional ByVal client_string As String = "", Optional ByVal order_by_string As String = "", Optional ByVal fields_name As String = "", Optional ByVal type_string As String = "", Optional ByVal size_string As String = "", Optional ByVal page_break_header As String = "", Optional ByRef estimated_value_label2 As String = "", Optional ByVal months_to_show As Integer = 0, Optional ByRef export_string As String = "", Optional ByRef years_of As Integer = 0, Optional ByRef aftt_within As Long = 0, Optional ByRef years_current As String = "", Optional ByRef aftt_current As String = "", Optional ByRef estimates_export As String = "", Optional ByRef spi_bottom_label As String = "", Optional ByRef spi_graph_string As String = "", Optional ByRef sold_avg_asking_text As String = "", Optional ByRef sold_avg_sold_text As String = "", Optional ByRef sold_percent_asking_text As String = "", Optional ByRef sold_variance_text As String = "", Optional ByRef sold_dom_text As String = "", Optional ByRef sold_aftt_text As String = "", Optional ByVal use_only_used_data As Boolean = False, Optional ByVal use_jetnet_data As Boolean = True, Optional ByVal extra_sold_criteria As String = "", Optional ByVal extra_client_sold_criteria As String = "", Optional ByVal is_jetnet_spi As Boolean = False, Optional ByRef first_asking_vs_selling_graph As String = "", Optional ByVal JournalIDsToIncludeString As String = "", Optional ByVal runModelonlyValueView As Boolean = False, Optional ByRef PassBackTable As DataTable = Nothing)
        Dim ClientTable As New DataTable
        Dim JetnetTable As New DataTable

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim temp_header As String = ""
        Dim rows_to_show As Integer = 0
        Dim font_shrink As String = ""
        Dim title_section As String = ""
        Dim add_to_table As Boolean = False
        Dim dr As DataRow
        Dim temp_asking As Long = 0
        Dim temp_take As Long = 0
        Dim temp_sold As Long = 0
        Dim temp_date As String = ""
        Dim aclsData_Temp As New clsData_Manager_SQL
        Dim temp_details As String = ""
        Dim row_count As Integer = 0
        Dim pages_made As Integer = 1
        Dim orig_view As Boolean = False
        Dim start_text As String = ""
        Dim order_by_string_break() As String
        Dim db_fields_names() As String

        Dim temp_field As String = ""
        Dim temp_val As String = ""
        Dim format_me As Boolean = False
        Dim temp_field_name As String = ""
        Dim type_string_names() As String
        Dim size_string_names() As String

        Dim temp_type As String = ""
        Dim temp_size As String = ""
        Dim COMPLETED_DATE As String = ""

        Dim asking_with_sold_total As Double = 0
        Dim sold_with_asking_total As Double = 0

        Dim asking_with_sale_count As Integer = 0
        Dim asking_with_sale_percent As Double = 0

        Dim asking_total As Double = 0
        Dim sold_total As Double = 0
        Dim asking_count As Integer = 0
        Dim sold_count As Integer = 0

        Dim has_asking As Boolean = False
        Dim has_sold As Boolean = False

        Dim current_real_asking As Double = 0
        Dim percent_of_current_asking As Double = 0
        Dim percent_of_avg_asking As Double = 0


        Dim title_section_export As String = ""
        Dim htmlOut_Export As New StringBuilder
        Dim temp_header_export As String = ""
        Dim year_range As String = ""
        Dim aftt_range As String = ""
        Dim year_range_client As String = ""
        Dim aftt_range_client As String = ""
        Dim htmlOut_Export_estimates As New StringBuilder
        Dim use_only_used As String = ""

        Dim arrFeatCodes() As String = Nothing
        Dim arrStdFeatCodes(,) As String = Nothing
        Dim cellWidth As Integer = 20

        'Setting up array if needed of IDs to include
        Dim JetnetJournalIDsToInclude As String = ""
        Dim ClientJournalIDsToInclude As String = ""

        If JournalIDsToIncludeString <> "" Then 'This is the only time we really care about filling these in.
            'They should originate from a textbox on the view that gets filled in as the datatable control fills it up/removes it
            Dim BreakableIDArray As Array
            BreakableIDArray = Split(JournalIDsToIncludeString, ",")
            If UBound(BreakableIDArray) > 0 Then
                For BreakableIDArrayCount = 0 To UBound(BreakableIDArray)
                    'This means that we have some pairs. 
                    'Since we're only ever going to be expecting |CLIENT or |JETNET, we don't need to split them into another array. 
                    'Let's just remove spaces:
                    Dim TemporaryHoldingID As String = BreakableIDArray(BreakableIDArrayCount)
                    TemporaryHoldingID = UCase(Replace(TemporaryHoldingID, " ", ""))
                    If InStr(TemporaryHoldingID, "|JETNET") Then 'This is a jetnet ID:
                        TemporaryHoldingID = Replace(TemporaryHoldingID, "|JETNET", "")
                        If IsNumeric(TemporaryHoldingID) Then
                            If JetnetJournalIDsToInclude <> "" Then
                                JetnetJournalIDsToInclude += ", "
                            End If
                            JetnetJournalIDsToInclude += TemporaryHoldingID
                        End If
                    ElseIf InStr(TemporaryHoldingID, "|CLIENT") Then 'This is a Client ID:
                        TemporaryHoldingID = Replace(TemporaryHoldingID, "|CLIENT", "")
                        If IsNumeric(TemporaryHoldingID) Then
                            If ClientJournalIDsToInclude <> "" Then
                                ClientJournalIDsToInclude += ", "
                            End If
                            ClientJournalIDsToInclude += TemporaryHoldingID
                        End If
                    End If
                Next
            End If
        End If


        If ClientJournalIDsToInclude <> "" Then
            'We need to set the extra_sold_criteria here.
            extra_client_sold_criteria &= " and clitrans_id in (" & ClientJournalIDsToInclude & ")"
        End If

        If JetnetJournalIDsToInclude <> "" Then
            'We need to set the extra_sold_criteria here.
            extra_sold_criteria &= " and journ_id in (" & JetnetJournalIDsToInclude & ")"
        End If


        If searchCriteria.ViewCriteriaAFTTStart > 0 Then
            aftt_range_client += " and ((clitrans_airframe_total_hours >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (clitrans_airframe_total_hours IS NULL))"
            aftt_range += " and  ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))"
        End If

        If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
            aftt_range_client += " and ((clitrans_airframe_total_hours <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (clitrans_airframe_total_hours IS NULL))"
            aftt_range += " and  ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))"
        End If


        If searchCriteria.ViewCriteriaYearStart > 0 Then
            year_range_client += " and clitrans_year_mfr >= " & searchCriteria.ViewCriteriaYearStart
            year_range += " and ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
            year_range_client += " and clitrans_year_mfr <=  " & searchCriteria.ViewCriteriaYearEnd
            year_range += " and ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd
        End If

        ' - SOLD TRENDS TAB--------------------------------------------------
        Dim start_date As String = "'"
        Dim startring_quarter As Integer = 0
        Dim starting_year As Integer = 0
        Dim array_of_quarters(20) As String
        Dim array_of_end_dates(20) As String
        Dim asking_sum_quarter(20) As Double
        Dim asking_count_quarter(20) As Double
        Dim sold_sum_quarter(20) As Double
        Dim sold_count_quarter(20) As Double
        Dim sold_dom_quarter(20) As Double
        Dim sold_dom_count_quarter(20) As Double
        Dim sold_aftt_quarter(20) As Double
        Dim sold_aftt_count_quarter(20) As Double
        Dim sold_with_asking_quarter(20) As Double
        Dim sold_with_asking_count(20) As Double
        Dim asking_with_sold_quarter(20) As Double
        Dim asking_with_sold_count(20) As Double

        Dim quarters_to_use As Integer = 0
        Dim date_first_quarter_end As String = ""
        Dim temp_date_sold As String = ""
        Dim temp_year As Integer = 0
        Dim temp_quarter As Integer = 0
        Dim use_this_quarter As Integer = 0
        Dim temp_val1 As Double = 0
        Dim last_val As Double = 0
        Dim temp_vala As String = ""
        Dim temp_vale As String = ""
        Dim temp_dom As String = ""
        Dim used_sale As Boolean = False
        Dim show_asking As Boolean = False
        Dim temp_date2 As String = ""
        Dim temp_end As String = ""
        Dim resulta As String = ""
        Dim is_crm_evo As Boolean = False
        Dim temp_sale As String = ""
        Dim aftt_number As String = ""
        Dim soh1 As String = ""
        Dim soh2 As String = ""

        ' - SOLD TRENDS TAB--------------------------------------------------


        Try

            'If CRMViewActive Then
            '  'Set up connection
            '  aclsData_Temp.client_DB = HttpContext.Current.Application.Item("crmClientDatabase")
            '  aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            'Else
            'Setting up with the correct connections.
            aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
            'End If


            'If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            '  If aclsData_Temp.client_DB = "" Then
            '    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
            '    is_crm_evo = True
            '  End If
            'End If


            ' - SOLD TRENDS TAB--------------------------------------------------

            For i = 0 To 19
                asking_sum_quarter(i) = 0
                asking_count_quarter(i) = 0
                sold_sum_quarter(i) = 0
                sold_count_quarter(i) = 0
                sold_dom_quarter(i) = 0
                sold_dom_count_quarter(i) = 0
                sold_aftt_quarter(i) = 0
                sold_aftt_count_quarter(i) = 0
            Next


            start_date = DateAdd(DateInterval.Month, -months_to_show, Date.Now())

            If Month(start_date) = 12 Or Month(start_date) = 11 Or Month(start_date) = 10 Then '10/16/2016 - 1/1/2017 - 1/1/2014
                start_date = "1/1/" & (Year(start_date) + 1)
            ElseIf Month(start_date) = 7 Or Month(start_date) = 8 Or Month(start_date) = 9 Then '8/16/2016 - 10/1/2016 - 10/1/2013
                start_date = "10/1/" & Year(start_date)
            ElseIf Month(start_date) = 4 Or Month(start_date) = 5 Or Month(start_date) = 6 Then '5/16/2016 - 7/1/2016 - 7/1/2013
                start_date = "7/1/" & Year(start_date)
            Else '1,2,3     '3/16/2016 - 3/1/2016 - 4/1/2013
                start_date = "4/1/" & Year(start_date)
            End If


            starting_year = Year(CDate(start_date))

            If months_to_show = 6 Then
                quarters_to_use = 2
            ElseIf months_to_show = 12 Then
                quarters_to_use = 4
            ElseIf months_to_show = 18 Then
                quarters_to_use = 6
            ElseIf months_to_show = 24 Then
                quarters_to_use = 8
            ElseIf months_to_show = 36 Then
                quarters_to_use = 12
            ElseIf months_to_show = 48 Then
                quarters_to_use = 16
            ElseIf months_to_show = 60 Then
                quarters_to_use = 20
            End If

            If Month(CDate(start_date)) < 4 Then
                startring_quarter = 1
                date_first_quarter_end = CDate("4/1/" & starting_year)
            ElseIf Month(CDate(start_date)) < 7 Then
                startring_quarter = 2
                date_first_quarter_end = CDate("7/1/" & starting_year)
            ElseIf Month(CDate(start_date)) < 10 Then
                startring_quarter = 3
                date_first_quarter_end = CDate("10/1/" & starting_year)
            Else
                startring_quarter = 4
                date_first_quarter_end = CDate("1/1/" & starting_year + 1)
            End If

            array_of_end_dates(0) = date_first_quarter_end
            array_of_quarters(0) = (starting_year & " - Q" & startring_quarter)

            temp_date_sold = CDate(date_first_quarter_end)

            For i = 1 To quarters_to_use
                temp_date_sold = DateAdd(DateInterval.Quarter, 1, CDate(temp_date_sold))

                If Month(CDate(temp_date_sold)) = 1 Then ' if end is month 1 then its quarter 4
                    temp_quarter = 4
                    temp_year = Year(temp_date_sold) - 1
                ElseIf Month(CDate(temp_date_sold)) = 4 Then
                    temp_quarter = 1
                    temp_year = Year(temp_date_sold)
                ElseIf Month(CDate(temp_date_sold)) = 7 Then
                    temp_quarter = 2
                    temp_year = Year(temp_date_sold)
                Else
                    temp_quarter = 3
                    temp_year = Year(temp_date_sold)
                End If


                array_of_end_dates(i) = temp_date_sold
                array_of_quarters(i) = (temp_year & " - Q" & temp_quarter)
            Next





            ' - SOLD TRENDS TAB--------------------------------------------------


            If Trim(years_of) <> "" And Trim(years_of) <> "0" Then
                year_range = " and (ac_mfr_year >= " & (years_current - years_of) & " and ac_mfr_year <= " & (years_current + years_of) & " ) "
                year_range_client = " and (clitrans_year_mfr >= " & (years_current - years_of) & " and clitrans_year_mfr <= " & (years_current + years_of) & " ) "
            End If

            If Trim(aftt_within) <> "" And Trim(aftt_within) <> "0" Then
                aftt_range = " and (ac_airframe_tot_hrs >= " & (aftt_current - aftt_within) & " and ac_airframe_tot_hrs <= " & (aftt_current + aftt_within) & " ) "
                aftt_range_client = " and (clitrans_airframe_total_hours >= " & (aftt_current - aftt_within) & " and clitrans_airframe_total_hours <= " & (aftt_current + aftt_within) & " ) "
            End If



            If DisplayLink Then
                font_shrink = "<font>"
            Else
                font_shrink = "<font size='-2'>"
            End If

            If use_only_used_data = True Then
                use_only_used = " and clitrans_newac_flag = 'N' "
            Else
                use_only_used = ""
            End If
            'Grabbing client data

            If is_jetnet_spi = True Then
                ' for new spi view
                'If use_only_used_data = True Then
                use_only_used = " and journ_newac_flag = 'N' "
                'End If

                'ADDED IN MSW  - 5/17/2016
                If Trim(first_asking_vs_selling_graph) = "Y" Then
                    first_asking_vs_selling_graph = ""
                    extra_sold_criteria &= " and ac_asking_price is not NULL and ac_asking_price > 0 "
                    extra_sold_criteria &= " and ac_sale_price is not NULL and ac_sale_price > 0 "
                End If

                'Grabbing jetnet data
                JetnetTable = CRMget_retail_sales_info(searchCriteria, is_internal, is_retail, LAST_SAVE_DATE, jetnet_string, months_to_show, year_range, aftt_range, use_only_used, extra_sold_criteria)
                'Combining/Altering data

                CombineTwoHistoryDatatables_Empty_Client(ClientTable, JetnetTable, results_table, is_retail)
            Else

                If Trim(first_asking_vs_selling_graph) = "Y" Then
                    extra_client_sold_criteria &= " and clitrans_asking_price is not NULL and clitrans_asking_price > 0 "
                    extra_client_sold_criteria &= " and clitrans_sold_price is not NULL and clitrans_sold_price > 0 "
                End If
                ''''''''''''''extra_client_sold_criteria = " and clitrans_id = 16044478 "'test
                ClientTable = Client_get_retail_sales_info(searchCriteria, "", LAST_SAVE_DATE, client_string, months_to_show, year_range_client, aftt_range_client, use_only_used, extra_client_sold_criteria, is_retail)

                If use_only_used_data = True Then
                    use_only_used = " and journ_newac_flag = 'N' "
                End If

                If Trim(first_asking_vs_selling_graph) = "Y" Then
                    first_asking_vs_selling_graph = ""
                    extra_sold_criteria &= " and ac_asking_price is not NULL and ac_asking_price > 0 "
                    extra_sold_criteria &= " and ac_sale_price is not NULL and ac_sale_price > 0 "
                End If

                '''''''''extra_sold_criteria = " and journ_id = 16044478 " 'test
                'Grabbing jetnet data
                JetnetTable = CRMget_retail_sales_info(searchCriteria, is_internal, is_retail, LAST_SAVE_DATE, jetnet_string, months_to_show, year_range, aftt_range, use_only_used, extra_sold_criteria)
                'Combining/Altering data


                If Trim(client_string) = "" Then
                    CombineTwoHistoryDatatables(ClientTable, JetnetTable, results_table, is_retail)
                Else
                    results_table = ModifyAndCombineJetnetClientDataForSale(ClientTable, JetnetTable, searchCriteria, "", order_by_string, is_retail)
                End If
            End If

            If clsGeneral.clsGeneral.isCrmDisplayMode() = True And is_crm_evo = True Then
                If IsNothing(table_to_return) Then
                    table_to_return = results_table
                End If
            End If


            If clsGeneral.clsGeneral.isCrmDisplayMode() = True And is_crm_evo = True Then

            ElseIf Trim(client_string) = "" Then
                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        If is_word Then
                            If page_break_after > 0 Then
                                title_section = ("<table border='0' cellpadding='1' cellspacing='0' border='1' id=""retailSalesCopy"">")
                            Else
                                title_section = ("<table border='0' cellpadding='1' cellspacing='0' id=""retailSalesCopy"">")
                            End If
                        Else
                            If page_break_after > 0 Then
                                title_section = ("<table border='0' cellpadding='2' cellspacing='0' border='1' id=""retailSalesCopy"">")
                            Else
                                title_section = ("<table border='0' cellpadding='2' cellspacing='0' id=""retailSalesCopy"">")
                            End If
                        End If



                        If DisplayLink = True Then
                            title_section = title_section & ("<thead><tr><th align='center' class='seperator'>SEL</th>")
                        End If

                        ' If NOTE_ID > 0 Then
                        If DisplayLink = False And NOTE_ID = 0 Then
                        Else
                            title_section &= ("<th align='center' class='seperator'>HIDDEN ID</th>")
                        End If

                        'End If
                        title_section_export = title_section      ' added here

                        If DisplayLink Then
                            title_section = title_section & ("<th align='center' class='seperator'>SRC</th>")
                        End If

                        If DisplayLink Then
                            If CRMViewActive Then
                                title_section = title_section & ("<th align='center' class='seperator'><strong>EDT</strong></th>")
                                If NOTE_ID > 0 Then
                                    If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
                                        If runModelonlyValueView = False Then
                                            title_section = title_section & ("<th align='center' class='seperator'><strong>$</strong></th>")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        title_section = title_section & ("<th align='left' nowrap='nowrap' class='seperator'><strong>" & font_shrink & "Serial#</font></strong></th><th align='left' class='seperator'><strong>" & font_shrink & "Reg#</font></strong></th>")
                        title_section = title_section & ("<th align='center' class='seperator'><strong>" & font_shrink & "Date</font></strong></th>")

                        title_section = title_section & ("<th class='seperator' align='center'><strong>" & font_shrink & "Year<br/>MFR</font></strong></th>")


                        title_section = title_section & ("<th class='seperator' align='center'><strong><span class='help_cursor' title='Asking Price applies to both Jetnet and Client Records.'>" & font_shrink & "Asking($k)</font></span></strong></th>")

                        If CRMViewActive Then
                            title_section = title_section & ("<th class='seperator' align='center'><strong><span class='help_cursor' title='Take Price applies to only Client Records.'>" & font_shrink & "Take<br/>($k)</font></span></strong></th>")
                            title_section = title_section & ("<th class='seperator' align='center'><strong><span class='help_cursor' title='Sold Price applies to only Client Records.'>" & font_shrink & "Sold<br/>($k)</font></span></strong></th>")
                            title_section = title_section & ("<th class='seperator' align='center'><strong>" & font_shrink & "Value Note</font></strong></th>")
                        End If

                        title_section = title_section & ("<th class='seperator' align='center'><strong>" & font_shrink & "Transaction Info</font></strong></th>")
                        title_section = title_section & ("<th class='seperator' align='center'><strong>" & font_shrink & "Year<br/>DLV</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "AFTT</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "ENGINE TT</font></strong></th>")

                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "ENG 1<br/>SOH</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "ENG 2<br/>SOH</font></strong></th>")


                        'Feature Codes:

                        JetnetDataLayer.load_standard_ac_features(searchCriteria, arrStdFeatCodes)

                        Dim sNonStandardAcFeature As String = ""
                        JetnetDataLayer.display_nonstandard_feature_code_headings(searchCriteria, arrFeatCodes, arrStdFeatCodes, cellWidth, sNonStandardAcFeature)

                        title_section = title_section & (sNonStandardAcFeature)


                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "PAX</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "INT YEAR</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "EXT YEAR</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "ENGINE MAINTENANCE PROGRAM</font></strong></th>")
                        title_section = title_section & ("<th align='left' class='seperator'><strong>" & font_shrink & "BROKER</font></strong></th>")



                        title_section = title_section & "</tr></thead>" + vbCrLf


                        '-------------------------- for export-----------------------------------------
                        title_section_export = title_section_export & ("<td align='left' nowrap='nowrap' class='seperator'><strong>" & font_shrink & "Serial #</font></strong></td><td align='left' class='seperator'><strong>" & font_shrink & "Reg #</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='center' class='seperator'><strong>" & font_shrink & "Date</font></strong></td>")
                        title_section_export = title_section_export & ("<td class='seperator' align='center'><strong>" & font_shrink & "Year<br/>MFR</font></strong></td>")

                        title_section_export = title_section_export & ("<td class='seperator' align='center'><strong><span class='help_cursor' title='Asking Price applies to both Jetnet and Client Records.'>" & font_shrink & "Asking($k)</font></span></strong></td>")

                        If CRMViewActive Then
                            title_section_export = title_section_export & ("<td class='seperator' align='center'><strong><span class='help_cursor' title='Take Price applies to only Client Records.'>" & font_shrink & "Take<br/>($k)</font></span></strong></td>")
                            title_section_export = title_section_export & ("<td class='seperator' align='center'><strong><span class='help_cursor' title='Sold Price applies to only Client Records.'>" & font_shrink & "Sold<br/>($k)</font></span></strong></td>")
                            title_section_export = title_section_export & ("<td class='seperator' align='center'><strong>" & font_shrink & "Value Note</font></strong></td>")
                        End If

                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "Transaction Info</font></strong></td>")
                        title_section_export = title_section_export & ("<td class='seperator' align='center'><strong>" & font_shrink & "Year<br/>DLV</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "AFTT</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "ENGINE TT</font></strong></td>")


                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "ENG 1<br/>SOH</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "ENG 2<br/>SOH</font></strong></td>")

                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "INT<br/>YEAR</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "EXT<br/>YEAR</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "ENGINE MAINTENANCE PROGRAM</font></strong></td>")
                        title_section_export = title_section_export & ("<td align='left' class='seperator'><strong>" & font_shrink & "BROKER</font></strong></td>")

                        '-------------------------- for export-----------------------------------------


                        htmlOut.Append(title_section)
                        ' htmlOut.Append(title_section & "</tr>")

                        htmlOut_Export.Append(title_section_export & "</tr>")

                        title_section_export = title_section_export & ("<td class='seperator' align='center'><strong><span class='help_cursor' title='Sold Price applies to only Client Records.'>" & font_shrink & "Date Listed</font></span></strong></td>")

                        title_section_export = title_section_export & ("<td class='seperator' align='center'><strong><span class='help_cursor' title='Sold Price applies to only Client Records.'>" & font_shrink & "AFTT</font></span></strong></td>")

                        htmlOut_Export_estimates.Append(title_section_export & "</tr>")


                        soh1 = ""
                        soh2 = ""

                        For Each r As DataRow In results_table.Rows
                            'We may need to look for a jetnet sale price/jetnet data:
                            Dim JetnetSalePrice As New DataTable

                            If is_excel = False Then
                                If page_break_after = 0 Then   ' if not for spec----
                                    If (rows_to_show > 15 And (Not DisplayLink)) Then ' only do top 4- recent for pdf
                                        rows_to_show = 0
                                        htmlOut.Append("</td></tr>")
                                        htmlOut.Append("</table>")
                                        htmlOut.Append("</td></tr></table>")
                                        htmlOut.Append("</td></tr></table>")
                                        htmlOut.Append(Insert_Page_Break(is_word))

                                        htmlOut.Append("<table align='center' id='fleetTable'  cellpadding='1' cellspacing='0' width='95%'>")

                                        htmlOut.Append("<tr id='trInner_Content_AC_PIC'>")
                                        htmlOut.Append("<td  id='tdInner_Content_AC_PIC' align='center' colspan='3'>")

                                        If Trim(is_company_logo) <> "" Then
                                            htmlOut.Append(is_company_logo)
                                        Else
                                            htmlOut.Append("<img src='/images/marketpdfheader.jpg' />")
                                        End If


                                        htmlOut.Append("</td></tr>")
                                        htmlOut.Append("<tr><Td>")

                                        htmlOut.Append("<table border='0' width='100%' cellpadding='2' cellspacing='0'>")
                                        'htmlOut.Append("<tr><td valign='middle'  bgcolor='#C0C0C0' align='center' colspan='5'>" & font_shrink & "RECENT RETAIL SALES <em>(Last " & months_to_show & " Months)</em></font></td></tr>")

                                        htmlOut.Append("<tr><td align='left' colspan='5'>")

                                        htmlOut.Append(title_section)

                                    End If
                                End If
                            End If


                            row_count = row_count + 1


                            ' only should be for spec page break 
                            If page_break_after > 0 Then
                                If row_count > page_break_after Then

                                    If pages_made = 1 Then ' if its the first time in, then made the page break after much larger
                                        ' page_break_after = page_break_after + 10
                                        htmlOut.Append("</table>")  '  for the row/tables made in this function
                                        htmlOut.Append("</td></tr></table>")
                                    Else
                                        htmlOut.Append("</table>")  '  for the row/tables made in this function
                                        htmlOut.Append("</td></tr></table>")
                                        htmlOut.Append("</td></tr></table>")
                                    End If

                                    pages_made = pages_made + 1



                                    htmlOut.Append("</table></td></tr></table>") ' for the header
                                    htmlOut.Append(Insert_Page_Break(is_word))
                                    htmlOut.Append(Replace(header_text, "Market Value Analysis", "Value Analysis - Sold Survey (" & pages_made & ")"))
                                    htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module""><tr><td align=""left"" valign=""top"">")


                                    htmlOut.Append("<table id='modelForsaleViewTopTable' width=""95%"" cellpadding=""4"" cellspacing=""0""><tr><td align=""left"" valign=""top"">")
                                    htmlOut.Append(title_section)
                                    row_count = 0
                                End If
                            End If



                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class='alt_row " & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export.Append("<tr class='alt_row " & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export_estimates.Append("<tr class='alt_row " & r.Item("source").ToString & "CRMRow'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr class='" & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export.Append("<tr class='" & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export_estimates.Append("<tr class='" & r.Item("source").ToString & "CRMRow'>")
                                toggleRowColor = False
                            End If

                            If DisplayLink = True Then
                                htmlOut.Append("<td align='center' class='seperator'> </td>")
                            End If

                            ' If NOTE_ID > 0 Then
                            If DisplayLink = False And NOTE_ID = 0 Then
                            Else
                                htmlOut.Append("<td align='center' class='seperator'>" & r.Item("journ_id").ToString & "|" & r.Item("source").ToString & "</td>")
                            End If

                            'End If
                            If DisplayLink Then
                                htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap' data-sort=""" & r.Item("source").ToString & """>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")
                                If CRMViewActive Then
                                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

                                    'If r("source") = "CLIENT" Then
                                    '  'If CDbl(r("client_jetnet_trans_id")) > 0 Then
                                    '  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=transaction&cli_trans=" & r("journ_id") & "&trans=" & r("client_jetnet_trans_id") & "&acID=" & r.Item("ac_id") & "&source=" & r.Item("source") & "&from=view&activetab=" & ActiveTabIndexInteger & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    'Else
                                    '  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=transaction&trans=" & r("journ_id") & "&acID=" & r.Item("ac_id") & "&source=" & r.Item("source") & "&from=view&activetab=" & ActiveTabIndexInteger & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    'End If
                                    resulta = ""
                                    If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                                        For i = 0 To searchCriteria.ViewCriteriaAmodIDArray.Length - 1
                                            If Trim(resulta) <> "" Then
                                                resulta = resulta & ", "
                                            End If
                                            resulta = resulta & searchCriteria.ViewCriteriaAmodIDArray(i).ToString
                                        Next
                                        If r("source") = "CLIENT" Then
                                            searchCriteria.ViewCriteriaAircraftID = r.Item("jetnet_ac_id")
                                            searchCriteria.ViewCriteriaJournalID = r("client_jetnet_trans_id")
                                            htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?" & IIf(Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request("viewType"))), "viewType=" & Trim(HttpContext.Current.Request("viewType")) & "&", "") & "action=edit&viewID=" & searchCriteria.ViewID.ToString & "&type=transaction&amod_id=" & searchCriteria.ViewCriteriaAmodID.ToString & "&cli_trans=" & r("journ_id") & "&trans=" & r("client_jetnet_trans_id") & "&acID=" & IIf(r.Item("jetnet_ac_id") > 0, r.Item("jetnet_ac_id").ToString & "&source=JETNET", r.Item("ac_id").ToString & "&source=" & r("source").ToString) & "&from=view&activetab=" & ActiveTabIndexInteger & "&extra_amod=" & resulta & IIf(NOTE_ID > 0, "&noteID=" & NOTE_ID.ToString, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                        Else
                                            searchCriteria.ViewCriteriaAircraftID = r.Item("ac_id")
                                            searchCriteria.ViewCriteriaJournalID = r.Item("journ_id")
                                            htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?" & IIf(Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request("viewType"))), "viewType=" & Trim(HttpContext.Current.Request("viewType")) & "&", "") & "action=edit&viewID=" & searchCriteria.ViewID.ToString & "&type=transaction&amod_id=" & searchCriteria.ViewCriteriaAmodID.ToString & "&trans=" & r("journ_id") & "&acID=" & IIf(r.Item("jetnet_ac_id") > 0, r.Item("jetnet_ac_id").ToString & "&source=JETNET", r.Item("ac_id").ToString & "&source=" & r("source").ToString) & "&from=view&activetab=" & ActiveTabIndexInteger & "&extra_amod=" & resulta & IIf(NOTE_ID > 0, "&noteID=" & NOTE_ID.ToString, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                        End If
                                    Else
                                        If r("source") = "CLIENT" Then
                                            searchCriteria.ViewCriteriaAircraftID = r.Item("jetnet_ac_id")
                                            searchCriteria.ViewCriteriaJournalID = r("client_jetnet_trans_id")
                                            htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?" & IIf(Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request("viewType"))), "viewType=" & Trim(HttpContext.Current.Request("viewType")) & "&", "") & "action=edit&viewID=" & searchCriteria.ViewID.ToString & "&type=transaction&amod_id=" & searchCriteria.ViewCriteriaAmodID.ToString & "&cli_trans=" & r("journ_id") & "&trans=" & r("client_jetnet_trans_id") & "&acID=" & IIf(r.Item("jetnet_ac_id") > 0, r.Item("jetnet_ac_id").ToString & "&source=JETNET", r.Item("ac_id").ToString & "&source=" & r("source").ToString) & "&from=view&activetab=" & ActiveTabIndexInteger & IIf(NOTE_ID > 0, "&noteID=" & NOTE_ID.ToString, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                        Else
                                            searchCriteria.ViewCriteriaAircraftID = r.Item("ac_id")
                                            searchCriteria.ViewCriteriaJournalID = r.Item("journ_id")
                                            htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?" & IIf(Not String.IsNullOrEmpty(Trim(HttpContext.Current.Request("viewType"))), "viewType=" & Trim(HttpContext.Current.Request("viewType")) & "&", "") & "action=edit&viewID=" & searchCriteria.ViewID.ToString & "&type=transaction&amod_id=" & searchCriteria.ViewCriteriaAmodID.ToString & "&trans=" & r("journ_id") & "&acID=" & IIf(r.Item("jetnet_ac_id") > 0, r.Item("jetnet_ac_id").ToString & "&source=JETNET", r.Item("ac_id").ToString & "&source=" & r("source").ToString) & "&from=view&activetab=" & ActiveTabIndexInteger & IIf(NOTE_ID > 0, "&noteID=" & NOTE_ID.ToString, "") & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                        End If
                                    End If



                                    htmlOut.Append("<img src='images/edit_icon.png' alt='Edit this Transaction' title='Edit this Transaction' border='0'>")
                                    htmlOut.Append("</a>")
                                    htmlOut.Append("</td>")
                                End If
                            Else
                                If r("source") = "CLIENT" Then
                                    searchCriteria.ViewCriteriaAircraftID = r.Item("jetnet_ac_id")
                                    searchCriteria.ViewCriteriaJournalID = r("client_jetnet_trans_id")
                                Else
                                    searchCriteria.ViewCriteriaAircraftID = r.Item("ac_id")
                                    searchCriteria.ViewCriteriaJournalID = r.Item("journ_id")
                                End If
                            End If

                            If DisplayLink Then
                                If CRMViewActive And NOTE_ID > 0 Then
                                    If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
                                        If runModelonlyValueView = False Then
                                            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' PROSPECTS

                                            'This appends the notes on the table.
                                            If Trim(searchCriteria.ViewCriteriaAircraftID) <> "" Then
                                                If r.Item("ac_id") = 0 Then
                                                    htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", REAL_AC_ID, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("jetnet_ac_id").ToString))
                                                Else
                                                    htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", REAL_AC_ID, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("jetnet_ac_id").ToString))
                                                End If
                                            Else
                                                htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", 0, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("jetnet_ac_id").ToString))
                                            End If

                                            htmlOut.Append("</td>")
                                        End If
                                    End If
                                End If
                            End If


                            temp_details = ""
                            If Not IsDBNull(r("ac_ser_no_full")) Then
                                temp_details = r("ac_ser_no_full")
                                Dim SerSort As String = ""
                                SerSort = r("ac_ser_no_full")
                                SerSort = Regex.Replace(SerSort, "[^0-9]", "")


                                If Not DisplayLink Then
                                    htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'  data-sort=""" & SerSort & """>" + font_shrink + r.Item("ac_ser_no_full").ToString + "</font></td>")
                                ElseIf r.Item("source").ToString = "JETNET" Then

                                    htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'  data-sort=""" & SerSort & """>")


                                    If r.Item("ac_id") = 0 Then
                                        If Not IsDBNull(r.Item("jetnet_ac_id")) Then
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("jetnet_ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        End If
                                    Else
                                        htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                        ' htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                    End If

                                    ' htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'><a target='_blank' href='DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "'>" + r.Item("ac_ser_no_full").ToString + "</a></td>")

                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a></td>")

                                    htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_ser_no_full").ToString + "</font></td>")
                                    htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_ser_no_full").ToString + "</font></td>")

                                Else
                                    htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'  data-sort=""" & SerSort & """>")
                                    If r.Item("ac_id") = 0 Then
                                        If Not IsDBNull(r.Item("jetnet_ac_id")) Then
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("jetnet_ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        End If
                                    Else
                                        htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                        'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                    End If
                                    ' <a target='_blank' href='/details.aspx?type=3&source=JETNET&ac_ID=" + r.Item("ac_id").ToString + "'>" +
                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a></td>")

                                    htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_ser_no_full").ToString + "</font></td>")
                                    htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_ser_no_full").ToString + "</font></td>")

                                End If



                            Else
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                            End If

                            If Not IsDBNull(r("ac_reg_no")) Then
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_reg_no").ToString + "</font></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_reg_no").ToString + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_reg_no").ToString + "</font></td>")
                            Else
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                            End If

                            temp_date = ""
                            If Not IsDBNull(r("journ_date")) Then
                                Dim dateSort As String = ""
                                dateSort = Format(r.Item("journ_date"), "yyyy/MM/dd")
                                temp_date2 = FormatDateTime(r.Item("journ_date").ToString, DateFormat.GeneralDate)
                                temp_end = Right(Trim(temp_date2), 2)
                                temp_date2 = Left(Trim(temp_date2), Len(Trim(temp_date2)) - 2)

                                If Right(Trim(temp_date2), 2) = "20" Or Right(Trim(temp_date2), 2) = "19" Then
                                    temp_date2 = Left(Trim(temp_date2), Len(Trim(temp_date2)) - 2) & temp_end
                                End If

                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'  data-sort='" & dateSort & "'><em>" & font_shrink + temp_date2 & "</font></em></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em>" + font_shrink & temp_date2 & "</font></em></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em>" + font_shrink & temp_date2 & "</font></em></td>")


                                temp_date = FormatDateTime(r.Item("journ_date").ToString, DateFormat.GeneralDate)

                                For i = 0 To quarters_to_use
                                    If CDate(temp_date) < CDate(array_of_end_dates(i)) Then
                                        use_this_quarter = i
                                        i = quarters_to_use + 1
                                    End If
                                Next


                            Else
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em> </em></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em> </em></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'><em> </em></td>")
                            End If



                            If Not IsDBNull(r("ac_mfr_year")) Then
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_mfr_year").ToString + "</font></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_mfr_year").ToString + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" + font_shrink + r.Item("ac_mfr_year").ToString + "</font></td>")
                            Else
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'> </td>")
                            End If



                            has_sold = False
                            has_asking = False
                            add_to_table = False
                            temp_asking = 0
                            temp_take = 0
                            temp_sold = 0
                            'Asking Price
                            htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                            htmlOut_Export.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                            htmlOut_Export_estimates.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)

                            If Not IsDBNull(r("ac_asking_price")) Then
                                If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                                    show_asking = False
                                    If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                        show_asking = True
                                    ElseIf r("source") = "CLIENT" Then
                                        show_asking = True
                                    ElseIf r("ac_asking") = "Price" Then
                                        show_asking = True
                                    End If

                                    If show_asking = True Then

                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_asking")) Then
                                                If (Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "") And Trim(r("source")) = "JETNET" Then
                                                    htmlOut.Append("<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'>")
                                                End If
                                            Else
                                                show_asking = show_asking
                                            End If
                                        End If

                                        htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_asking_price").ToString) / 1000), 0).ToString + "")

                                        If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                            If Not IsDBNull(r("ac_asking")) Then
                                                If (Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "") And Trim(r("source")) = "JETNET" Then
                                                    htmlOut.Append("</a>")
                                                End If
                                            End If
                                        End If

                                        htmlOut_Export.Append("$" + FormatNumber((CDbl(r.Item("ac_asking_price").ToString) / 1000), 0).ToString + "")


                                        htmlOut_Export_estimates.Append(r.Item("ac_asking_price").ToString)
                                        temp_asking = CDbl(r.Item("ac_asking_price").ToString)
                                        add_to_table = True
                                        has_asking = True



                                        asking_sum_quarter(use_this_quarter) = CDbl(CDbl(asking_sum_quarter(use_this_quarter)) + CDbl(temp_asking))
                                        asking_count_quarter(use_this_quarter) = asking_count_quarter(use_this_quarter) + 1


                                        asking_total = CDbl(CDbl(asking_total) + CDbl(temp_asking))
                                        asking_count = asking_count + 1
                                    End If
                                Else
                                End If
                            End If

                            If has_asking = False Then
                                If Not IsDBNull(r("ac_asking")) Then
                                    If Trim(r("ac_asking")) = "Make Offer" Then
                                        htmlOut.Append("M/O ")
                                        htmlOut_Export.Append("M/O ")
                                        htmlOut_Export_estimates.Append("M/O ")
                                    Else
                                        htmlOut.Append("OFFMKT ")
                                        htmlOut_Export.Append("OFFMKT ")
                                        htmlOut_Export_estimates.Append("OFFMKT ")
                                    End If
                                Else
                                    htmlOut.Append("M/O ")
                                    htmlOut_Export.Append("M/O ")
                                    htmlOut_Export_estimates.Append("M/O ")
                                End If
                            End If


                            If CRMViewActive Or HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")
                                'Take Price Column
                                htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export_estimates.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)

                                If Not IsDBNull(r("ac_take_price")) Then
                                    If CDbl(r.Item("ac_take_price").ToString) > 0 Then
                                        htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_take_price").ToString) / 1000), 0).ToString + "")
                                        htmlOut_Export.Append("$" + FormatNumber((CDbl(r.Item("ac_take_price").ToString) / 1000), 0).ToString + "")
                                        htmlOut_Export_estimates.Append(r.Item("ac_take_price").ToString)
                                        add_to_table = True
                                        temp_take = CDbl(r.Item("ac_take_price").ToString)
                                    End If
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")
                                'Sold Price Column
                                htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export_estimates.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)


                                'If Not IsDBNull(r("ac_sold_price")) Then
                                '  If CDbl(r.Item("ac_sold_price").ToString) > 0 Then

                                If r("source") = "CLIENT" Then
                                    If Not IsDBNull(r("ac_sold_price")) Then
                                        If CDbl(r.Item("ac_sold_price").ToString) > 0 Then
                                            htmlOut.Append("$" + FormatNumber((CDbl(r.Item("ac_sold_price").ToString) / 1000), 0).ToString + "")
                                            htmlOut_Export.Append("$" + FormatNumber((CDbl(r.Item("ac_sold_price").ToString) / 1000), 0).ToString + "")
                                            htmlOut_Export_estimates.Append(r.Item("ac_sold_price").ToString)

                                            add_to_table = True
                                            temp_sold = CDbl(r.Item("ac_sold_price").ToString)
                                            has_sold = True

                                            sold_sum_quarter(use_this_quarter) = CDbl(CDbl(sold_sum_quarter(use_this_quarter)) + CDbl(temp_sold))
                                            sold_count_quarter(use_this_quarter) = sold_count_quarter(use_this_quarter) + 1

                                            sold_total = CDbl(CDbl(sold_total) + CDbl(temp_sold))
                                            sold_count = sold_count + 1
                                        Else


                                            If r.Item("client_jetnet_trans_id") > 0 Then
                                                JetnetSalePrice = CRMget_retail_sales_info(searchCriteria, is_internal, is_retail, LAST_SAVE_DATE, jetnet_string, months_to_show, year_range, aftt_range, use_only_used, " and journ_id = " & r.Item("client_jetnet_trans_id").ToString)
                                                If Not IsNothing(JetnetSalePrice) Then
                                                    If JetnetSalePrice.Rows.Count > 0 Then
                                                        If Not IsDBNull(JetnetSalePrice.Rows(0).Item("ac_sale_price_display_flag")) Then
                                                            If Trim(JetnetSalePrice.Rows(0).Item("ac_sale_price_display_flag").ToString) = "Y" Then
                                                                If Not IsDBNull(JetnetSalePrice.Rows(0).Item("ac_sold_price")) Then
                                                                    If CDbl(JetnetSalePrice.Rows(0).Item("ac_sold_price").ToString) > 0 Then
                                                                        used_sale = True
                                                                        add_to_table = True

                                                                        htmlOut.Append("<A href='' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>")
                                                                        htmlOut.Append("<p unselectable='on' style='display:inline'>")
                                                                        htmlOut.Append(DisplayFunctions.TextToImage("$" & FormatNumber((JetnetSalePrice.Rows(0).Item("ac_sold_price").ToString / 1000), 0) & "", 7, "", "42", "Reported Sale Price Displayed with Permission from Source."))
                                                                        htmlOut.Append("</p>")
                                                                        htmlOut.Append("</a>")
                                                                    End If
                                                                End If
                                                            End If
                                                        End If

                                                        If Not String.IsNullOrEmpty(JetnetSalePrice.Rows(0).Item("ac_engine_1_soh_hrs").ToString) Then
                                                            soh1 = (JetnetSalePrice.Rows(0).Item("ac_engine_1_soh_hrs").ToString.Trim + " ")
                                                        Else
                                                            soh1 = (" ")
                                                        End If

                                                        If Not String.IsNullOrEmpty(JetnetSalePrice.Rows(0).Item("ac_engine_2_soh_hrs").ToString) Then
                                                            soh2 = (JetnetSalePrice.Rows(0).Item("ac_engine_2_soh_hrs").ToString.Trim + " ")
                                                        Else
                                                            soh2 = (" ")
                                                        End If
                                                    End If
                                                End If
                                            End If

                                        End If
                                    End If
                                Else ' if its a jetnet record, add it into the export for estimates

                                    If Not IsDBNull(r("ac_sold_price")) Then
                                        If CDbl(r.Item("ac_sold_price").ToString) > 0 Then

                                            If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                                If Not IsDBNull(r("ac_sale_price_display_flag")) Then
                                                    If Trim(r.Item("ac_sale_price_display_flag").ToString) = "Y" Then
                                                        used_sale = True
                                                        add_to_table = True

                                                        htmlOut.Append("<A href='' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>")

                                                        ' tweaked MSW - 1/21/19 - take make it non displayable only when it shouldnt be
                                                        temp_sale = DisplayFunctions.TextToImage("$" & FormatNumber((r.Item("ac_sold_price").ToString / 1000), 0) & "", 7, "", "42", "Reported Sale Price Displayed with Permission from Source.")

                                                        If InStr(temp_sale, "img") > 0 Then
                                                            htmlOut.Append("<p unselectable='on' style='display:inline'>")
                                                            htmlOut.Append(temp_sale)
                                                            htmlOut.Append("</p>")
                                                        Else
                                                            htmlOut.Append(temp_sale)
                                                        End If

                                                        htmlOut.Append("</a>")

                                                        htmlOut_Export.Append("$" + FormatNumber((CDbl(r.Item("ac_sold_price").ToString) / 1000), 0).ToString + "")
                                                        htmlOut_Export_estimates.Append(r.Item("ac_sold_price").ToString)

                                                        temp_sold = CDbl(r.Item("ac_sold_price").ToString)
                                                        has_sold = True

                                                        sold_sum_quarter(use_this_quarter) = CDbl(CDbl(sold_sum_quarter(use_this_quarter)) + CDbl(temp_sold))
                                                        sold_count_quarter(use_this_quarter) = sold_count_quarter(use_this_quarter) + 1

                                                        sold_total = CDbl(CDbl(sold_total) + CDbl(temp_sold))
                                                        sold_count = sold_count + 1
                                                    Else
                                                        used_sale = False
                                                    End If
                                                Else
                                                    used_sale = False
                                                End If

                                            Else
                                                used_sale = False
                                            End If
                                        End If
                                    End If

                                    If used_sale = False Then
                                        If use_jetnet_data = True Then
                                            htmlOut_Export_estimates.Append(r.Item("ac_sold_price").ToString)

                                            temp_sold = CDbl(r.Item("ac_sold_price").ToString)
                                            has_sold = True

                                            sold_sum_quarter(use_this_quarter) = CDbl(CDbl(sold_sum_quarter(use_this_quarter)) + CDbl(temp_sold))
                                            sold_count_quarter(use_this_quarter) = sold_count_quarter(use_this_quarter) + 1

                                            sold_total = CDbl(CDbl(sold_total) + CDbl(temp_sold))
                                            sold_count = sold_count + 1
                                        Else
                                            ' do not use in anything 
                                        End If
                                    End If

                                End If

                                'End If
                                '  End If

                                'if there is both 
                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If Not IsDBNull(r("ac_sold_price")) Then
                                        If CDbl(r.Item("ac_asking_price").ToString) > 0 And CDbl(r.Item("ac_sold_price").ToString) > 0 Then
                                            asking_with_sold_quarter(use_this_quarter) = CDbl(CDbl(asking_with_sold_quarter(use_this_quarter)) + CDbl(temp_asking))
                                            sold_with_asking_quarter(use_this_quarter) = CDbl(CDbl(sold_with_asking_quarter(use_this_quarter)) + CDbl(temp_sold))

                                            asking_with_sold_count(use_this_quarter) = asking_with_sold_count(use_this_quarter) + 1
                                            sold_with_asking_count(use_this_quarter) = sold_with_asking_count(use_this_quarter) + 1
                                        End If
                                    End If
                                End If



                                aftt_number = "0"
                                If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                                    If r.Item("ac_airframe_tot_hrs").ToString <> "" Then
                                        aftt_number = r.Item("ac_airframe_tot_hrs").ToString
                                        aftt_number = Replace(aftt_number, ",", "")

                                        If Len(aftt_number) = 6 Then
                                            aftt_number = aftt_number
                                        ElseIf Len(aftt_number) = 5 Then
                                            aftt_number = "0" & aftt_number
                                        ElseIf Len(aftt_number) = 4 Then
                                            aftt_number = "00" & aftt_number
                                        ElseIf Len(aftt_number) = 3 Then
                                            aftt_number = "000" & aftt_number
                                        ElseIf Len(aftt_number) = 2 Then
                                            aftt_number = "0000" & aftt_number
                                        ElseIf Len(aftt_number) = 1 Then
                                            aftt_number = "00000" & aftt_number
                                        End If

                                    End If
                                End If



                                htmlOut_Export_estimates.Append("</td><td align='right' valign='top' nowrap='nowrap' class='seperator' data-sort='" & aftt_number & "'>" & font_shrink)


                                'If Not IsDBNull(r("ac_list_date")) Then
                                '  If r.Item("ac_list_date").ToString <> "" Then
                                '    htmlOut_Export_estimates.Append(r.Item("ac_list_date").ToString)

                                '    temp_dom = DateDiff(DateInterval.Day, CDate(r.Item("ac_list_date").ToString), CDate(r.Item("journ_date").ToString))

                                '    sold_dom_quarter(use_this_quarter) = CDbl(sold_aftt_quarter(use_this_quarter) + temp_dom)
                                '    sold_dom_count_quarter(use_this_quarter) = sold_dom_count_quarter(use_this_quarter) + 1

                                '  End If
                                'End If
                                'htmlOut_Export_estimates.Append("&nbsp;")

                                'htmlOut_Export_estimates.Append("</td><td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)

                                If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                                    If r.Item("ac_airframe_tot_hrs").ToString <> "" Then
                                        htmlOut_Export_estimates.Append(r.Item("ac_airframe_tot_hrs").ToString)
                                        'datediff(d,ac_list_date,journ_date) as DOM
                                        sold_aftt_quarter(use_this_quarter) = CDbl(sold_aftt_quarter(use_this_quarter) + CDbl(r.Item("ac_airframe_tot_hrs")))
                                        sold_aftt_count_quarter(use_this_quarter) = sold_aftt_count_quarter(use_this_quarter) + 1
                                    End If
                                End If
                                htmlOut_Export_estimates.Append(" ")

                                htmlOut_Export_estimates.Append("</td><td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)

                                If Not IsDBNull(r("source")) Then
                                    If r.Item("source").ToString <> "" Then
                                        htmlOut_Export_estimates.Append(r.Item("source").ToString)
                                    End If
                                End If

                            End If

                            If has_asking And has_sold Then
                                asking_with_sold_total = CDbl(CDbl(asking_with_sold_total) + CDbl(temp_asking))
                                sold_with_asking_total = CDbl(CDbl(sold_with_asking_total) + CDbl(temp_sold))

                                asking_with_sale_count = asking_with_sale_count + 1

                                'added MSW 5/11/16 - if they have spi, do not automatically clear
                                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                                Else
                                    If r("source") = "CLIENT" Then
                                    Else ' if its jetnet, clear
                                        temp_sold = 0 ' after adding to estimates, re set = 0 
                                    End If
                                End If

                            End If




                            If CRMViewActive = True Then
                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")

                                htmlOut.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)
                                htmlOut_Export_estimates.Append("<td align='right' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink)

                                If Not IsDBNull(r("clitrans_value_description")) Then
                                    If r.Item("clitrans_value_description").ToString <> "" Then
                                        htmlOut.Append(r.Item("clitrans_value_description").ToString)
                                        htmlOut_Export.Append(r.Item("clitrans_value_description").ToString)
                                        ' htmlOut_Export_estimates.Append(r.Item("clitrans_value_description").ToString)
                                    Else
                                        htmlOut.Append(" ")
                                        htmlOut_Export.Append(" ")
                                        ' htmlOut_Export_estimates.Append("&nbsp;")
                                    End If
                                Else
                                    htmlOut_Export.Append(" ")
                                    htmlOut.Append(" ")
                                End If


                            End If


                            htmlOut.Append("</font></td>")
                            htmlOut_Export.Append("</font></td>")
                            htmlOut_Export_estimates.Append("</font></td>")

                            Dim JournalSubjectNote As String = ""
                            Dim JournalSubjectNoteDisplay As String = ""
                            'Set up the subject
                            If Not IsDBNull(r("journ_subject")) Then
                                If Not String.IsNullOrEmpty(r("journ_subject")) Then
                                    JournalSubjectNote = Left(r.Item("journ_subject").ToString, 90).ToString
                                    JournalSubjectNoteDisplay = Left(r.Item("journ_subject").ToString, 90).ToString
                                End If
                            End If
                            'The note
                            If Not IsDBNull(r("journ_customer_note")) Then
                                If Not String.IsNullOrEmpty(r.Item("journ_customer_note")) Then
                                    JournalSubjectNote += " (<span class=""help_cursor error_text no_text_underline"" title=""" + r.Item("journ_customer_note").ToString + """>Note</span>)"
                                    JournalSubjectNoteDisplay += " (<span class=""help_cursor error_text no_text_underline"" title=""" + r.Item("journ_customer_note").ToString + """>" + r.Item("journ_customer_note").ToString + "</span>)"
                                End If
                            End If

                            If Not DisplayLink Then  ' if its the pdf
                                If Not String.IsNullOrEmpty(JournalSubjectNote) Then
                                    htmlOut.Append("<td align='left' valign='top'  class='seperator'>" + font_shrink + JournalSubjectNote + "</font></td>")
                                    htmlOut_Export.Append("<td align='left' valign='top'  class='seperator'>" + font_shrink + JournalSubjectNoteDisplay + "</font></td>")
                                    htmlOut_Export_estimates.Append("<td align='left' valign='top'  class='seperator'>" + font_shrink + JournalSubjectNoteDisplay + "</font></td>")
                                Else
                                    htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                    htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                    htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                End If
                            ElseIf Not String.IsNullOrEmpty(JournalSubjectNote) Then
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + JournalSubjectNote + "</td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + JournalSubjectNoteDisplay + "</td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" + JournalSubjectNoteDisplay + "</td>")
                            Else
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'> </td>")
                            End If

                            ' Feature Codes

                            Dim sAcFeatureCodes As String = ""
                            '''''''''''''''''''''''''''''''''''''''''''

                            ' If Not IsDBNull(r.Item("source").ToString) Then
                            '  If Trim(r.Item("source").ToString) = "CLIENT" Then
                            '    JetnetDataLayer.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                            '  Else
                            '    JetnetDataLayer.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                            '  End If
                            'Else
                            ' JetnetDataLayer.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                            'End If

                            If Not IsDBNull(r.Item("source").ToString) Then
                                If Trim(r.Item("source").ToString) = "CLIENT" Then
                                    JetnetDataLayer.display_client_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                                Else
                                    JetnetDataLayer.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                                End If
                            Else
                                JetnetDataLayer.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
                            End If

                            If InStr(sAcFeatureCodes, "No features available for this") > 0 Then
                                'sAcFeatureCodes = Replace(sAcFeatureCodes, "<td ", "<td colspan='" & arrFeatCodes.Length & "' ")
                                For x = 1 To arrFeatCodes.Length - 1
                                    sAcFeatureCodes += "<td> </td>"
                                Next
                            End If


                            ''Owner lookup:
                            'searchCriteria.ViewCriteriaGetExclusive = False
                            'searchCriteria.ViewCriteriaGetOperator = False

                            'Dim ownerDataTable As New DataTable
                            'Dim OwnerString As New StringBuilder

                            'If r.Item("source") = "JETNET" Then
                            '  ownerDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
                            'Else
                            '  ownerDataTable = CLIENTGetOwnerExclusiveOperatorInformation(searchCriteria)
                            'End If

                            'BuildCompanyStringBuilder(DisplayLink, ownerDataTable, OwnerString, searchCriteria)


                            searchCriteria.ViewCriteriaGetExclusive = True
                            searchCriteria.ViewCriteriaGetOperator = False
                            Dim brokerDataTable As New DataTable
                            Dim brokerString As New StringBuilder
                            brokerDataTable = GetOwnerExclusiveOperatorInformation(searchCriteria)
                            BuildCompanyStringBuilder(DisplayLink, brokerDataTable, brokerString, searchCriteria)
                            If r.Item("source") = "CLIENT" Then
                                Dim ACYear As String = ""
                                Dim ACAirframe As String = ""
                                Dim PAX As String = ""
                                Dim IntYear As String = ""
                                Dim ExtYear As String = ""
                                Dim EngTime As String = ""
                                Dim EMPGString As String = ""

                                If r.Item("client_jetnet_trans_id") > 0 Then
                                    If JetnetSalePrice.Rows.Count = 0 Then 'Fill up table, hasn't been already:
                                        JetnetSalePrice = CRMget_retail_sales_info(searchCriteria, is_internal, is_retail, LAST_SAVE_DATE, jetnet_string, months_to_show, year_range, aftt_range, use_only_used, " and journ_id = " & r.Item("client_jetnet_trans_id").ToString)
                                    End If

                                    If Not IsNothing(JetnetSalePrice) Then
                                        If JetnetSalePrice.Rows.Count > 0 Then

                                            If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                                IntYear = (Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

                                                If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                                    IntYear += ("/")
                                                End If
                                                IntYear += Right(r.Item("ac_interior_moyear").ToString, 4).Trim + " "
                                            End If


                                            If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                                ExtYear = (Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

                                                If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                                    ExtYear += ("/")

                                                End If
                                                ExtYear += (Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + " ")

                                            End If

                                            If Not IsDBNull(JetnetSalePrice.Rows(0).Item("emp_program_name")) Then
                                                EMPGString = JetnetSalePrice.Rows(0).Item("emp_program_name")
                                            End If

                                            If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                                                If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                                                    EngTime = ("[0] ")
                                                Else
                                                    EngTime = ("[" + r.Item("ac_engine_1_tot_hrs").ToString + "] ")
                                                End If
                                            Else
                                                EngTime = ("[U] ")
                                            End If

                                            If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                                                If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                                                    EngTime += ("[0] ")
                                                Else
                                                    EngTime += ("[" + r.Item("ac_engine_2_tot_hrs").ToString + "] ")
                                                End If
                                            Else
                                                EngTime += ("[U] ")
                                            End If

                                            If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                                                If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                                                    EngTime += ("[0] ")
                                                Else
                                                    EngTime += ("[" + r.Item("ac_engine_3_tot_hrs").ToString + "] ")
                                                End If
                                            End If

                                            If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                                                If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                                                    EngTime += ("[0] ")
                                                Else
                                                    EngTime += ("[" + r.Item("ac_engine_4_tot_hrs").ToString + "] ")
                                                End If
                                            End If

                                            If Not IsDBNull(JetnetSalePrice.Rows(0).Item("ac_year")) Then
                                                ACYear = JetnetSalePrice.Rows(0).Item("ac_year").ToString
                                            End If
                                            If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
                                                If IsNumeric(r.Item("ac_airframe_tot_hrs")) Then
                                                    If r.Item("ac_airframe_tot_hrs") > 0 Then
                                                        ACAirframe = r.Item("ac_airframe_tot_hrs")
                                                    End If
                                                End If
                                            End If


                                            If Not IsDBNull(JetnetSalePrice.Rows(0).Item("ac_passenger_count")) Then
                                                If CDbl(JetnetSalePrice.Rows(0).Item("ac_passenger_count").ToString) = 0 Then
                                                    PAX = "0 "
                                                Else
                                                    PAX = JetnetSalePrice.Rows(0).Item("ac_passenger_count").ToString + " "
                                                End If
                                            Else
                                                PAX = "U "
                                            End If

                                            If Trim(soh1) = "" Then
                                                If Not String.IsNullOrEmpty(r.Item("ac_engine_1_soh_hrs").ToString) Then
                                                    soh1 = (r.Item("ac_engine_1_soh_hrs").ToString.Trim + " ")
                                                Else
                                                    soh1 = (" ")
                                                End If
                                            End If

                                            If Trim(soh2) = "" Then
                                                If Not String.IsNullOrEmpty(r.Item("ac_engine_2_soh_hrs").ToString) Then
                                                    soh2 = (r.Item("ac_engine_2_soh_hrs").ToString.Trim + " ")
                                                Else
                                                    soh2 = (" ")
                                                End If
                                            End If
                                        End If
                                    End If
                                End If


                                'year DLV:
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACYear + "</font></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACYear + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACYear + "</font></td>")

                                'AFTT
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACAirframe + "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACAirframe + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + ACAirframe + "</font></td>")
                                'Engine TT
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & EngTime & "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & EngTime & "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & EngTime & ";</font></td>")

                                ' --------------------- CLIENT RECODS _ NEED TO LOOK UP JETNET ---------------------------------

                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh1 & "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh1 & "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh1 & "</font></td>")

                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh2 & "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh2 & "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & soh2 & "</font></td>")


                                htmlOut.Append(sAcFeatureCodes)

                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + PAX + "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + PAX + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + PAX + "</font></td>")

                                'Ext Year
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & ExtYear & "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & ExtYear & "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & ExtYear & "</font></td>")
                                'Int Year
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & IntYear & "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & IntYear & "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & IntYear & "</font></td>")
                                ''EMPG
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")

                                'Broker
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")




                            Else

                                'year DLV:
                                htmlOut.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_year").ToString + "</font></td>")
                                htmlOut_Export.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_year").ToString + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='center' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_year").ToString + "</font></td>")

                                'AFTT
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_airframe_tot_hrs").ToString + "</font></td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_airframe_tot_hrs").ToString + "</font></td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" + r.Item("ac_airframe_tot_hrs").ToString + "</font></td>")

                                'Engine TT
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
                                    If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
                                        htmlOut.Append("[0] ")
                                    Else
                                        htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "] ")
                                    End If
                                Else
                                    htmlOut.Append("[U] ")
                                End If

                                If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
                                    If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
                                        htmlOut.Append("[0] ")
                                    Else
                                        htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "] ")
                                    End If
                                Else
                                    htmlOut.Append("[U] ")
                                End If

                                If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
                                    If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
                                        htmlOut.Append("[0] ")
                                    Else
                                        htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "] ")
                                    End If
                                End If

                                If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
                                    If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
                                        htmlOut.Append("[0] ")
                                    Else
                                        htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "] ")
                                    End If
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")


                                'ac_engine_1_soh_hrs
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")

                                If Not String.IsNullOrEmpty(r.Item("ac_engine_1_soh_hrs").ToString) Then
                                    htmlOut.Append(r.Item("ac_engine_1_soh_hrs").ToString.Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")

                                'ac_engine_2_soh_hrs
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")

                                If Not String.IsNullOrEmpty(r.Item("ac_engine_2_soh_hrs").ToString) Then
                                    htmlOut.Append(r.Item("ac_engine_2_soh_hrs").ToString.Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")

                                htmlOut.Append(sAcFeatureCodes)

                                'PAX
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")

                                If Not IsDBNull(r("ac_passenger_count")) Then
                                    If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
                                        htmlOut.Append("0 ")
                                        htmlOut_Export.Append("0 ")
                                        htmlOut_Export_estimates.Append("0 ")
                                    Else
                                        htmlOut.Append(r.Item("ac_passenger_count").ToString + " ")
                                        htmlOut_Export.Append(r.Item("ac_passenger_count").ToString + " ")
                                        htmlOut_Export_estimates.Append(r.Item("ac_passenger_count").ToString + " ")
                                    End If
                                Else
                                    htmlOut.Append("U ")
                                    htmlOut_Export.Append("U ")
                                    htmlOut_Export_estimates.Append("U ")
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")


                                'Ext Year
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")

                                If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")

                                    End If
                                    htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 2).Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")

                                'Int Year
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "")

                                If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
                                    htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

                                    If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
                                        htmlOut.Append("/")
                                    End If
                                    htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 2).Trim + " ")
                                Else
                                    htmlOut.Append(" ")
                                End If

                                htmlOut.Append("</font></td>")
                                htmlOut_Export.Append("</font></td>")
                                htmlOut_Export_estimates.Append("</font></td>")

                                Dim EMPGString As String = ""
                                If Not IsDBNull(r.Item("emp_program_name")) Then
                                    EMPGString = r.Item("emp_program_name")
                                End If

                                ''EMPG
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & EMPGString.ToString & "</td>")

                                'Broker
                                htmlOut.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")
                                htmlOut_Export.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")
                                htmlOut_Export_estimates.Append("<td align='left' valign='top' nowrap='nowrap' class='seperator'>" & font_shrink & "" & brokerString.ToString & "</font></td></tr>")


                            End If


                            If clsGeneral.clsGeneral.isCrmDisplayMode() = True And is_crm_evo = True Then ' if we are on evo/crm 
                            Else
                                If Not IsNothing(table_to_return) Then
                                    If add_to_table = True Then
                                        dr = table_to_return.NewRow()
                                        dr.Item("asking_price") = temp_asking
                                        dr.Item("take_price") = temp_take
                                        dr.Item("sold_price") = temp_sold
                                        dr.Item("date_of") = temp_date
                                        dr.Item("ac_details") = temp_details
                                        table_to_return.Rows.Add(dr)
                                    End If
                                End If
                            End If

                            rows_to_show = rows_to_show + 1
                        Next

                        htmlOut.Append("</table>")
                        htmlOut_Export.Append("</table>")
                        htmlOut_Export_estimates.Append("</table>")

                    Else
                        htmlOut.Append("No Retail Sales at this time, for this Make/Model ...")
                        htmlOut_Export.Append("No Retail Sales at this time, for this Make/Model ...")
                        htmlOut_Export_estimates.Append("No Retail Sales at this time, for this Make/Model ...")
                    End If

                Else
                    htmlOut.Append("No Retail Sales at this time, for this Make/Model ...")
                    htmlOut_Export.Append("No Retail Sales at this time, for this Make/Model ...")
                    htmlOut_Export_estimates.Append("No Retail Sales at this time, for this Make/Model ...")
                End If

                ' htmlOut.Append("</td></tr></table>")
                htmlOut_Export.Append("</td></tr></table>")
                htmlOut_Export_estimates.Append("</td></tr></table>")

                If sold_with_asking_total > 0 And asking_with_sold_total > 0 Then
                    asking_with_sale_percent = CDbl(CDbl(sold_with_asking_total) / CDbl(asking_with_sold_total))
                End If




                estimated_value_label2 = "<table width='90%' cellpadding='1'  border='1' cellspacing='0' class=""data_view_grid"">"
                estimated_value_label2 &= "<tr class='header_row'>"

                If Trim(HttpContext.Current.Request("estimates")) = "Y" Then
                    estimated_value_label2 &= "<td><a href='/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & NOTE_ID.ToString & "_estimates.xls' target='_blank'>Estimates</a> (Based on Last " & Trim(months_to_show) & " Months of Sales)</td>"
                Else
                    ' estimated_value_label2 &= "<td>Estimates (Based on Last " & Trim(months_to_show) & " Months of Sales)</td>"
                End If


                estimated_value_label2 &= "<td align='right'>Values</td>"
                estimated_value_label2 &= "</tr>"

                estimated_value_label2 &= "<tr>"
                estimated_value_label2 &= "<td class='alt_color'>Average Asking Price</td>"

                If CDbl(asking_count) = 0 Then
                    estimated_value_label2 &= "<td align='right'>-&nbsp;</td>"
                Else
                    estimated_value_label2 &= "<td align='right'>$" & FormatNumber(CDbl(asking_total) / CDbl(asking_count), 0) & "&nbsp;</td>"
                End If


                estimated_value_label2 &= "</tr>"

                estimated_value_label2 &= "<tr>"
                estimated_value_label2 &= "<td class='alt_color'>Average Sale Price</td>"

                If CDbl(sold_count) = 0 Then
                    estimated_value_label2 &= "<td align='right'>-&nbsp;</td>"
                Else
                    estimated_value_label2 &= "<td align='right'>$" & FormatNumber(CDbl(sold_total) / CDbl(sold_count), 0) & "&nbsp;</td>"
                End If



                estimated_value_label2 &= "</tr>"

                estimated_value_label2 &= "<tr>"


                If CDbl(asking_with_sale_percent) = 0 Then
                    estimated_value_label2 &= "<td class='alt_color'>Sale Price Avg (Not Available) of Asking</td>"
                Else
                    estimated_value_label2 &= "<td class='alt_color'>Sale Prices Avg (" & FormatNumber(CDbl(asking_with_sale_percent * 100), 1) & "%) of Asking</td>"
                End If

                estimated_value_label2 &= "<td>&nbsp;</td>"
                estimated_value_label2 &= "</tr>"


                If Not IsNothing(HttpContext.Current.Session.Item("Current_Asking")) Then
                    If Trim(HttpContext.Current.Session.Item("Current_Asking")) <> "" And IsNumeric(HttpContext.Current.Session.Item("Current_Asking")) Then
                        percent_of_current_asking = FormatNumber(CDbl(CDbl(HttpContext.Current.Session.Item("Current_Asking")) * CDbl(asking_with_sale_percent)), 0)
                    End If
                End If


                estimated_value_label2 &= "<tr>"
                estimated_value_label2 &= "<td class='alt_color'>Estimate Based % Current Asking</td>"
                If CDbl(percent_of_current_asking) = 0 Then
                    estimated_value_label2 &= "<td align='right'>-&nbsp;</td>"
                Else
                    estimated_value_label2 &= "<td align='right'>$" & FormatNumber(percent_of_current_asking, 0) & "&nbsp;</td>"
                End If


                estimated_value_label2 &= "</tr>"

                If CDbl(asking_count) > 0 And CDbl(asking_with_sale_percent) > 0 Then
                    percent_of_avg_asking = CDbl(CDbl(CDbl(asking_total) / CDbl(asking_count)) * asking_with_sale_percent)
                Else
                    percent_of_avg_asking = 0
                End If


                estimated_value_label2 &= "<tr>"
                estimated_value_label2 &= "<td class='alt_color'>Estimate Based % Avg Asking</td>"


                If CDbl(percent_of_avg_asking) = 0 Then
                    estimated_value_label2 &= "<td align='right'>-&nbsp;</td>"
                Else
                    estimated_value_label2 &= "<td align='right'>$" & FormatNumber(percent_of_avg_asking, 0) & "&nbsp;</td>"
                End If


                estimated_value_label2 &= "</tr>"

                estimated_value_label2 &= "</table>"


                spi_bottom_label = ""


                For i = 0 To quarters_to_use
                    temp_sold = 0
                    temp_asking = 0
                    temp_val1 = 0

                    spi_bottom_label &= array_of_quarters(i) & " :: "

                    If CDbl(asking_count_quarter(i)) > 0 Then
                        spi_bottom_label &= " ASKING (" & CDbl(asking_count_quarter(i)).ToString & ") = "
                        temp_asking = CDbl(CDbl(asking_sum_quarter(i)) / CDbl(asking_count_quarter(i)))
                        temp_asking = CDbl(temp_asking / 1000)
                        spi_bottom_label &= temp_asking
                    End If

                    If CDbl(sold_count_quarter(i)) > 0 Then
                        spi_bottom_label &= "; SOLD (" & CDbl(sold_count_quarter(i)).ToString & ") = "
                        temp_sold = CDbl(CDbl(sold_sum_quarter(i)) / CDbl(sold_count_quarter(i)))
                        temp_sold = CDbl(temp_sold / 1000)
                        spi_bottom_label &= temp_sold
                    End If

                    spi_bottom_label &= "<br><br>"
                    spi_bottom_label = ""

                    If IsNothing(HttpContext.Current.Session.Item("Current_Estimated")) Then
                        HttpContext.Current.Session.Item("Current_Estimated") = 0
                        temp_vale = "null"
                    ElseIf CDbl(HttpContext.Current.Session.Item("Current_Estimated")) = 0 Then
                        temp_vale = "null"
                    Else
                        temp_vale = Replace(CDbl(HttpContext.Current.Session.Item("Current_Estimated") / 1000), ",", "")
                    End If

                    If IsNothing(HttpContext.Current.Session.Item("Current_Asking")) Then
                        HttpContext.Current.Session.Item("Current_Asking") = 0
                        temp_vala = "null"
                    ElseIf CDbl(HttpContext.Current.Session.Item("Current_Asking")) = 0 Then
                        temp_vala = "null"
                    Else
                        temp_vala = Replace(CDbl(HttpContext.Current.Session.Item("Current_Asking") / 1000), ",", "")
                    End If

                    ' GETS STRING FOR FIRST TIME THROUGH, FOR BOTH ONLY ASKING AND SELLING
                    Call build_spi_string(first_asking_vs_selling_graph, True, temp_asking, is_jetnet_spi, array_of_quarters(i), temp_vala, temp_vale, temp_sold)

                    ' GETS STRING FOR EVERYTHING - ASKING, SELLING OR BOTH 
                    Call build_spi_string(spi_graph_string, False, temp_asking, is_jetnet_spi, array_of_quarters(i), temp_vala, temp_vale, temp_sold)



                    If Trim(sold_avg_asking_text) = "" Then
                        sold_avg_asking_text = " data2.addColumn('string', 'Quarter'); "
                        sold_avg_asking_text &= " data2.addColumn('number', 'Avg Asking Price ($k)'); "
                        ' sold_avg_asking_text &= " data2.addColumn('number', 'x1'); "
                        ' sold_avg_asking_text &= " data2.addColumn('number', 'x2'); "
                        ' sold_avg_asking_text &= " data2.addColumn('number', 'x3'); "
                        ' sold_avg_asking_text &= " data2.addColumn('number', 'x4'); "
                        ' sold_avg_asking_text &= " data2.addColumn('number', 'x5'); "
                        sold_avg_asking_text &= " data2.addRows(["

                        'If CDbl(temp_asking) > 0 Then
                        '  sold_avg_asking_text &= "['" & array_of_quarters(i) & "'," & temp_asking & ", null, null, null, null, null]"
                        'Else
                        '  sold_avg_asking_text &= "['" & array_of_quarters(i) & "', null, null, null, null, null, null]"
                        'End If

                        If CDbl(temp_asking) > 0 Then
                            sold_avg_asking_text &= "['" & array_of_quarters(i) & "'," & temp_asking & "]"
                        Else
                            sold_avg_asking_text &= "['" & array_of_quarters(i) & "', null]"
                        End If

                    Else
                        'If CDbl(temp_asking) > 0 Then
                        '  sold_avg_asking_text &= ",['" & array_of_quarters(i) & "'," & temp_asking & ", null, null, null, null, null]"
                        'Else
                        '  sold_avg_asking_text &= ",['" & array_of_quarters(i) & "',null, null, null, null, null, null]"
                        'End If

                        If CDbl(temp_asking) > 0 Then
                            sold_avg_asking_text &= ",['" & array_of_quarters(i) & "'," & temp_asking & "]"
                        Else
                            sold_avg_asking_text &= ",['" & array_of_quarters(i) & "',null]"
                        End If
                    End If


                    If Trim(sold_avg_sold_text) = "" Then
                        sold_avg_sold_text = " data3.addColumn('string', 'Quarter'); "
                        sold_avg_sold_text &= " data3.addColumn('number', 'Avg Selling Price ($k)'); "
                        sold_avg_sold_text &= " data3.addRows(["

                        If CDbl(temp_sold) > 0 Then
                            sold_avg_sold_text &= "['" & array_of_quarters(i) & "', " & temp_sold & "]"
                        Else
                            sold_avg_sold_text &= "['" & array_of_quarters(i) & "', null]"
                        End If

                    Else
                        If CDbl(temp_sold) > 0 Then
                            sold_avg_sold_text &= ",['" & array_of_quarters(i) & "'," & temp_sold & "]"
                        Else
                            sold_avg_sold_text &= ",['" & array_of_quarters(i) & "', null]"
                        End If
                    End If


                    If CDbl(temp_sold) <> 0 Then
                        temp_val1 = CDbl(CDbl(sold_with_asking_quarter(i)) / CDbl(asking_with_sold_quarter(i)) * 100)
                        'temp_val1 = CDbl(CDbl(temp_sold) / CDbl(temp_asking) * 100)
                        temp_val1 = Replace(FormatNumber(temp_val1, 1), ",", "")
                    End If

                    If Trim(sold_percent_asking_text) = "" Then
                        sold_percent_asking_text = " data4.addColumn('string', 'Quarter'); "
                        sold_percent_asking_text &= " data4.addColumn('number', 'Percent of Asking'); "
                        sold_percent_asking_text &= " data4.addRows(["

                        If CDbl(temp_val1) <> 0 Then
                            sold_percent_asking_text &= "['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                        Else
                            sold_percent_asking_text &= "['" & array_of_quarters(i) & "', null]"
                        End If

                    Else
                        If CDbl(temp_val1) <> 0 Then
                            sold_percent_asking_text &= ",['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                        Else
                            sold_percent_asking_text &= ",['" & array_of_quarters(i) & "', null]"
                        End If
                    End If


                    If CDbl(temp_sold) <> 0 Then
                        temp_val1 = CDbl(100 - CDbl(temp_val1))
                        temp_val1 = Replace(FormatNumber(temp_val1, 1), ",", "")
                    End If

                    If Trim(sold_variance_text) = "" Then
                        sold_variance_text = " data5.addColumn('string', 'Quarter'); "
                        sold_variance_text &= " data5.addColumn('number', 'Variance on Asking'); "
                        sold_variance_text &= " data5.addRows(["

                        If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                            sold_variance_text &= "['" & array_of_quarters(i) & "',null]"
                        Else
                            sold_variance_text &= "['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                        End If
                    Else
                        If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                            sold_variance_text &= ",['" & array_of_quarters(i) & "',null]"
                        Else
                            sold_variance_text &= ",['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                        End If

                    End If


                    '-----------------------------------------------------------------------------
                    'If Trim(sold_dom_text) = "" Then
                    '  sold_dom_text = " data6.addColumn('string', 'Quarter'); "
                    '  sold_dom_text &= " data6.addColumn('number', 'Average Days on Market'); "
                    '  sold_dom_text &= " data6.addRows(["

                    '  If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                    '    sold_dom_text &= "['" & array_of_quarters(i) & "',null]"
                    '  Else
                    '    sold_dom_text &= "['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                    '  End If
                    'Else
                    '  If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                    '    sold_dom_text &= ",['" & array_of_quarters(i) & "',null]"
                    '  Else
                    '    sold_dom_text &= ",['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                    '  End If

                    'End If

                    'If Trim(sold_aftt_text) = "" Then
                    '  sold_aftt_text = " data7.addColumn('string', 'Quarter'); "
                    '  sold_aftt_text &= " data7.addColumn('number', 'Average Airframe Total Time'); "
                    '  sold_aftt_text &= " data7.addRows(["

                    '  If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                    '    sold_aftt_text &= "['" & array_of_quarters(i) & "',null]"
                    '  Else
                    '    sold_aftt_text &= "['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                    '  End If
                    'Else
                    '  If CDbl(temp_val1) = 0 And CDbl(temp_val1) = 0 Then
                    '    sold_aftt_text &= ",['" & array_of_quarters(i) & "',null]"
                    '  Else
                    '    sold_aftt_text &= ",['" & array_of_quarters(i) & "'," & temp_val1 & "]"
                    '  End If

                    'End If
                    '-----------------------------------------------------------------------------



                    last_val = temp_asking

                Next




                ' temp_header = temp_header & ("<table border='0' width='100%' cellpadding='2' cellspacing='0'>")

                'If page_break_after > 0 Then ' for spec
                '  If DisplayLink Then
                '    temp_header = temp_header & ("<tr><td bgcolor='#C0C0C0' valign='middle' class='header_text' align='center' colspan='5'>RECENT SALES <em>(Last " & months_to_show & " Months)</em></td></tr>")
                '  Else
                '    temp_header = temp_header & ("<tr><td bgcolor='#C0C0C0' valign='middle' align='center' colspan='5'>" & font_shrink & "RECENT RETAIL SALES <em>(Last " & months_to_show & " Months)</em></font></td></tr>")
                '  End If
                'Else
                '  If DisplayLink Then
                '    temp_header = temp_header & ("<tr><td valign='middle' bgcolor='#C0C0C0' class='header' align='center' colspan='5'>RECENT SALES <em>(Last " & months_to_show & " Months)</em></td></tr>")
                '  Else
                '    temp_header = temp_header & ("<tr><td valign='middle' bgcolor='#C0C0C0' align='center' colspan='5'>" & font_shrink & "RECENT RETAIL SALES <em>(Last " & months_to_show & " Months)</em></font></td></tr>")
                '  End If
                'End If

                '------------------------------ FOR EXPORT SECTION-----------------------------
                temp_header_export = temp_header_export & ("<table border='0' width='100%' cellpadding='2' cellspacing='0'>")

                If page_break_after > 0 Then ' for spec
                    If DisplayLink Then
                        temp_header_export = temp_header_export & ("<tr><td bgcolor='#C0C0C0' valign='middle' class='header_text' align='center' colspan='5'>RECENT SALES <em>(Last " & months_to_show & " Months)</em></td></tr>")
                    Else
                        temp_header_export = temp_header_export & ("<tr><td bgcolor='#C0C0C0' valign='middle' align='center' colspan='5'>" & font_shrink & "RECENT RETAIL SALES <em>(Last " & months_to_show & " Months)</em></font></td></tr>")
                    End If
                Else
                    If DisplayLink Then
                        temp_header_export = temp_header_export & ("<tr><td valign='middle' bgcolor='#C0C0C0' class='header' align='center' colspan='5'>RECENT SALES <em>(Last " & months_to_show & " Months)</em></td></tr>")
                    Else
                        temp_header_export = temp_header_export & ("<tr><td valign='middle' bgcolor='#C0C0C0' align='center' colspan='5'>" & font_shrink & "RECENT RETAIL SALES <em>(Last " & months_to_show & " Months)</em></font></td></tr>")
                    End If
                End If
                '------------------------------ FOR EXPORT SECTION-----------------------------




            Else

                order_by_string_break = Split(order_by_string, ",")

                db_fields_names = Split(fields_name, ",")

                type_string_names = Split(type_string, ",")

                size_string_names = Split(size_string, ",")

                htmlOut.Append("<table id=""retailSalesCopy"" cellpadding='4' cellspacing='0' border='1'>")
                htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='4' cellspacing='0' border='1'>")

                htmlOut.Append("<thead><tr bgcolor='#CCCCCC'>")
                htmlOut_Export.Append("<tr bgcolor='#CCCCCC'>")

                If DisplayLink Then ' dont display these for export to excel
                    htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>SEL</th>")
                    'If NOTE_ID > 0 Then
                    htmlOut.Append("<th>HIDDEN ID</th>")
                    'End If
                    htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>SRC</th>")
                    If NOTE_ID > 0 Then
                        htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>EDT</th>")
                    End If
                    htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>$</th>")
                End If

                For i = 0 To order_by_string_break.Length - 1
                    htmlOut.Append("<th align='center' valign='middle' class='forSaleCellBorder'>")
                    htmlOut.Append(Trim(Replace(order_by_string_break(i), "'", "")) & "")
                    htmlOut.Append("</th>")

                    htmlOut_Export.Append("<th align='center' valign='middle' class='forSaleCellBorder'>")
                    htmlOut_Export.Append(Trim(Replace(order_by_string_break(i), "'", "")) & "")
                    htmlOut_Export.Append("</th>")
                Next

                htmlOut.Append("</tr></thead>")
                htmlOut_Export.Append("</tr>")

                ' clientTable to be changed to combined one later
                If Not IsNothing(results_table) Then
                    If results_table.Rows.Count > 0 Then
                        For Each r As DataRow In results_table.Rows

                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class='alt_row " & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export.Append("<tr class='alt_row " & r.Item("source").ToString & "CRMRow'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr  class='" & r.Item("source").ToString & "CRMRow'>")
                                htmlOut_Export.Append("<tr  class='" & r.Item("source").ToString & "CRMRow'>")
                                toggleRowColor = False
                            End If

                            For i = 0 To order_by_string_break.Length - 1

                                If i = 0 And DisplayLink = True Then
                                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'></td>")
                                    'If NOTE_ID > 0 Then
                                    htmlOut.Append("<td align='center' class='seperator'>" & r.Item("journ_id").ToString & "|" & r.Item("source").ToString & "</td>")
                                    'End If
                                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")

                                    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

                                    'If NOTE_ID > 0 Then
                                    '  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=6','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    'Else
                                    '  htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=" & NOTE_ID & "&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    'End If

                                    If r("source") = "CLIENT" Then
                                        'If CDbl(r("client_jetnet_trans_id")) > 0 Then
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=transaction&cli_trans=" & r("journ_id") & "&trans=" & r("client_jetnet_trans_id") & "&acID=" & r.Item("ac_id") & "&source=" & r.Item("source") & "&from=view&activetab=" & ActiveTabIndexInteger & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    Else
                                        htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=transaction&trans=" & r("journ_id") & "&acID=" & r.Item("ac_id") & "&source=" & r.Item("source") & "&from=view&activetab=" & ActiveTabIndexInteger & "','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                                    End If



                                    htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
                                    htmlOut.Append("</a>")
                                    htmlOut.Append("</td>")

                                    If NOTE_ID > 0 Then
                                        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

                                            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' PROSPECTS

                                            If Trim(searchCriteria.ViewCriteriaAircraftID) <> "" Then
                                                If r.Item("ac_id") = 0 Then
                                                    htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", REAL_AC_ID, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("client_jetnet_ac_id").ToString))
                                                Else
                                                    htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", REAL_AC_ID, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("client_jetnet_ac_id").ToString))
                                                End If
                                            Else
                                                htmlOut.Append(CheckForProspectorsTab(r.Item("ac_id"), aclsData_Temp, NOTE_ID, r.Item("source").ToString, "S", 0, r.Item("journ_id").ToString, LAST_SAVE_DATE, r.Item("client_jetnet_ac_id").ToString))
                                            End If

                                            htmlOut.Append("</td>")
                                        End If
                                    End If
                                End If


                                format_me = True

                                temp_field = Trim(Replace(order_by_string_break(i), "'", ""))
                                temp_field_name = Trim(Replace(db_fields_names(i), "'", ""))

                                temp_type = Trim(Replace(type_string_names(i), "'", ""))
                                temp_size = Trim(Replace(size_string_names(i), "'", ""))

                                If Not IsDBNull(r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")) Then
                                    temp_val = r("" & Trim(Replace(order_by_string_break(i), "'", "")) & "")
                                Else
                                    temp_val = ""
                                End If

                                If Trim(temp_type) <> "" Then
                                    If Trim(temp_type) = "String" Then

                                    ElseIf Trim(temp_type) = "Char" Then

                                    ElseIf Trim(temp_type) = "Value" Then
                                        temp_val = FormatNumber(temp_val, 0)
                                    ElseIf Trim(temp_type) = "Date" Then

                                    Else

                                    End If
                                End If




                                'If IsNumeric(temp_field) And (format_me) Then
                                If Trim(temp_type) = "Value" Then
                                    htmlOut.Append("<td align='right'>")
                                    htmlOut_Export.Append("<td align='right'>")
                                Else
                                    htmlOut.Append("<td align='left'>")
                                    htmlOut_Export.Append("<td align='left'>")
                                End If


                                If DisplayLink = True Then
                                    If Trim(temp_field_name) = "cliaircraft_ser_nbr" Then
                                        If r.Item("source").ToString = "JETNET" Then
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                            ' htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        Else
                                            htmlOut.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(r.Item("client_jetnet_ac_id"), 0, 0, 0, False, "", "underline", "") & ">")
                                            'htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("client_jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
                                        End If
                                    End If
                                End If

                                'If Trim(temp_field_name) = "cliaircraft_asking_price" Or Trim(temp_field_name) = "cliaircraft_est_price" Or Trim(temp_field_name) = "cliaircraft_broker_price" Then
                                If Trim(temp_type) = "Value" Then
                                    If IsNumeric(temp_val) Then
                                        If CDbl(temp_val) > 0 Then
                                            htmlOut.Append("$")
                                            htmlOut_Export.Append("$")
                                        End If
                                    End If
                                End If

                                'If IsNumeric(temp_val) And (format_me) Then
                                If Trim(temp_type) = "Value" Then
                                    If CDbl(temp_val) > 0 Then
                                        htmlOut.Append(FormatNumber(temp_val, 0))
                                        htmlOut_Export.Append(FormatNumber(temp_val, 0))
                                    End If
                                Else
                                    htmlOut.Append(temp_val)
                                    htmlOut_Export.Append(temp_val)
                                End If


                                If Trim(temp_field_name) = "cliaircraft_ser_nbr" And DisplayLink = True Then
                                    htmlOut.Append("</a>")
                                End If


                                htmlOut.Append("&nbsp;</td>")
                                htmlOut_Export.Append("&nbsp;</td>")

                            Next
                            htmlOut.Append("</tr>")
                            htmlOut_Export.Append("</tr>")
                        Next
                    End If
                End If


            End If

            If Not IsNothing(PassBackTable) Then
                PassBackTable = results_table
            End If
            temp_header = temp_header '& ("<tr><td align='left' colspan='5'>")

            temp_header_export = temp_header_export & ("<tr><td align='left' colspan='5'>")

        Catch ex As Exception

            class_error = "Error in views_display_recent_retail_sales(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = "<span id=""RetailNewWindowContents"">" & temp_header & htmlOut.ToString & "</span><div class=""resizeCWRetail""><div id=""RetailInnerTable"" style=""width: 100%;""></div></div>"

        export_string = temp_header_export & htmlOut_Export.ToString

        estimates_export = temp_header_export & htmlOut_Export_estimates.ToString

        htmlOut = Nothing
        results_table = Nothing

    End Sub


    Public Shared Sub BuildCompanyStringBuilder(ByRef displayLink As Boolean, ByRef OwnerDataTable As DataTable, ByRef ownerString As StringBuilder, ByRef searchCriteria As viewSelectionCriteriaClass)

        If Not IsNothing(OwnerDataTable) Then
            If OwnerDataTable.Rows.Count > 0 Then
                Dim sCompanyPhone As String = ""
                For Each vr_owner As DataRow In OwnerDataTable.Rows
                    ownerString.Append("<span class=""padding"">")

                    sCompanyPhone = ReturnCompanyPhoneFax(vr_owner("comp_phone_office"), vr_owner("comp_phone_fax"))

                    If Not searchCriteria.ViewCriteriaIsReport And displayLink Then
                        'If r.Item("source").ToString = "JETNET" Then
                        ownerString.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0") & "")
                        'ownerString.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                        'Else
                        '  ownerString.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                        'End If

                        If Not String.IsNullOrEmpty(sCompanyPhone) Then
                            ownerString.Append(" title='PH : " + sCompanyPhone + "'")
                        End If
                        ownerString.Append(">" + vr_owner.Item("comp_name").ToString.Trim + "</a>")
                    Else

                        'If r.Item("source").ToString = "JETNET" Then
                        ownerString.Append("<a class='underline' " & DisplayFunctions.WriteDetailsLink(0, vr_owner.Item("comp_id").ToString, 0, 0, False, "", "underline", "&journid=0") & "")
                        'ownerString.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                        'Else
                        '  ownerString.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
                        'End If

                        ownerString.Append(" title='PH : " + sCompanyPhone + "'>" + vr_owner.Item("comp_name").ToString.Trim + "</a>")

                        ownerString.Append("</span>")
                        If displayLink Then
                            ownerString.Append("<td><span class=""padding"">" + sCompanyPhone & "</span></td>") ' OWNERPHONE  
                        End If
                    End If
                Next
            End If
        End If
    End Sub


    Public Shared Sub build_spi_string(ByRef spi_graph_string As String, ByVal show_only_both_asking_selling As Boolean, ByVal temp_asking As Long, ByVal is_jetnet_spi As Boolean, ByVal array_spot As String, ByVal temp_vala As String, ByVal temp_vale As String, ByVal temp_sold As Long)

        ' only add if we have both asking and selling and we say "TRUE" that we need both, or its not new view, or we dont need both
        If (temp_asking > 0 And temp_sold > 0 And show_only_both_asking_selling = True) Or is_jetnet_spi = False Or show_only_both_asking_selling = False Then

            If Trim(spi_graph_string) = "" Then

                If show_only_both_asking_selling = True And CDbl(temp_asking) > 0 And CDbl(temp_sold) > 0 Then

                    spi_graph_string = " data1.addColumn('string', 'Quarter'); "
                    spi_graph_string &= " data1.addColumn('number', 'Asking'); "

                    If is_jetnet_spi = True Or show_only_both_asking_selling = True Then
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Estimate'); "
                        spi_graph_string &= " data1.addColumn('number', 'x1'); "
                        spi_graph_string &= " data1.addColumn('number', 'x2'); "
                    Else
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Asking'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Estimate'); "
                        spi_graph_string &= " data1.addColumn('number', 'x1'); "
                        spi_graph_string &= " data1.addColumn('number', 'x2'); "
                    End If


                    spi_graph_string &= " data1.addRows(["
                ElseIf show_only_both_asking_selling = False Then

                    spi_graph_string = " data1.addColumn('string', 'Quarter'); "
                    spi_graph_string &= " data1.addColumn('number', 'Asking'); "

                    If is_jetnet_spi = True Or show_only_both_asking_selling = True Then
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Estimate'); "
                        spi_graph_string &= " data1.addColumn('number', 'x1'); "
                        spi_graph_string &= " data1.addColumn('number', 'x2'); "
                    Else
                        spi_graph_string &= " data1.addColumn('number', 'Sold'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Asking'); "
                        spi_graph_string &= " data1.addColumn('number', 'My Estimate'); "
                        spi_graph_string &= " data1.addColumn('number', 'x1'); "
                        spi_graph_string &= " data1.addColumn('number', 'x2'); "
                    End If


                    spi_graph_string &= " data1.addRows(["
                Else
                    ' if show only both and there isnt both 
                End If




                If is_jetnet_spi = True Or show_only_both_asking_selling = True Then
                    If CDbl(temp_asking) > 0 And CDbl(temp_sold) > 0 Then
                        spi_graph_string &= "['" & array_spot & "'," & temp_asking & ", " & temp_vala & ", " & temp_sold & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_asking) > 0 Then
                        spi_graph_string &= "['" & array_spot & "'," & temp_asking & ", " & temp_vala & ", null, " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_sold) > 0 Then
                        spi_graph_string &= "['" & array_spot & "',null, " & temp_vala & ", " & temp_sold & ", " & temp_vale & ", null, null]"
                    ElseIf show_only_both_asking_selling = False Then
                        spi_graph_string &= "['" & array_spot & "',null, " & temp_vala & ", null, " & temp_vale & ", null, null]"
                    End If
                Else
                    If CDbl(temp_asking) > 0 And CDbl(temp_sold) > 0 Then
                        spi_graph_string &= "['" & array_spot & "'," & temp_asking & ", " & temp_sold & ", " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_asking) > 0 Then
                        spi_graph_string &= "['" & array_spot & "'," & temp_asking & ", null, " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_sold) > 0 Then
                        spi_graph_string &= "['" & array_spot & "',null, " & temp_sold & ", " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf show_only_both_asking_selling = False Then
                        spi_graph_string &= "['" & array_spot & "',null, null, " & temp_vala & ", " & temp_vale & ", null, null]"
                    End If
                End If



            Else
                If is_jetnet_spi = True Or show_only_both_asking_selling = True Then
                    If CDbl(temp_asking) > 0 And CDbl(temp_sold) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "'," & temp_asking & ", " & temp_vala & ", " & temp_sold & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_asking) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "'," & temp_asking & ", " & temp_vala & ", null, " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_sold) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "',null, " & temp_vala & ", " & temp_sold & ", " & temp_vale & ", null, null]"
                    ElseIf show_only_both_asking_selling = False Then
                        spi_graph_string &= ",['" & array_spot & "',null, " & temp_vala & ", null, " & temp_vale & ", null, null]"
                    End If
                Else
                    If CDbl(temp_asking) > 0 And CDbl(temp_sold) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "'," & temp_asking & ", " & temp_sold & ", " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_asking) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "'," & temp_asking & ", null, " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf CDbl(temp_sold) > 0 Then
                        spi_graph_string &= ",['" & array_spot & "',null, " & temp_sold & ", " & temp_vala & ", " & temp_vale & ", null, null]"
                    ElseIf show_only_both_asking_selling = False Then
                        spi_graph_string &= ",['" & array_spot & "',null, null, " & temp_vala & ", " & temp_vale & ", null, null]"
                    End If
                End If
            End If

        End If


    End Sub

    Public Shared Function Client_get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal sale_type_string As String, ByVal last_date As String, Optional ByVal client_string As String = "", Optional ByVal months_to_show As Integer = 0, Optional ByVal years_of As String = "", Optional ByVal aftt_within As String = "", Optional ByVal use_only_used As String = "", Optional ByVal extra_criteria As String = "", Optional ByVal isRetail As String = "") As DataTable
        Dim YearDateVariable As String = ""
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            If CDbl(months_to_show) > 0 Then
                YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(months_to_show), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(months_to_show), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(months_to_show), Now()))
            ElseIf Trim(last_date) = "" Then
                YearDateVariable = Year(DateAdd(DateInterval.Year, -1, Now())) & "-" & Month(DateAdd(DateInterval.Year, -1, Now())) & "-" & Day(DateAdd(DateInterval.Year, -1, Now()))
            Else
                YearDateVariable = Year(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, -1, CDate(last_date)))
            End If

            If Trim(client_string) <> "" Then

                sQuery.Append("SELECT distinct cliaircraft_id as ac_id, '' as emp_program_name, cliaircraft_est_price as ac_take_price,cliaircraft_ser_nbr_sort as ac_ser_no_sort, cliaircraft_ser_nbr as ac_ser_no_full, cliaircraft_jetnet_ac_id as client_jetnet_ac_id,  clitrans_id as journ_id, clitrans_jetnet_trans_id as client_jetnet_trans_id, ")

                sQuery.Append(", case when cliacep_engine_1_tsoh_hours is null then 0 else cliacep_engine_1_tsoh_hours end as ac_engine_1_soh_hrs, case when cliacep_engine_2_tsoh_hours is null then 0 else cliacep_engine_2_tsoh_hours end as ac_engine_2_soh_hrs ")

                sQuery.Append(client_string)
                sQuery.Append(", 'CLIENT' as source, cliaircraft_asking_wordage as ac_asking ")
                sQuery.Append(", cliaircraft_date_listed as ac_list_date, cliaircraft_airframe_total_hours as ac_airframe_tot_hrs, clitrans_customer_note as journ_customer_note, clitrans_retail_flag as retail_flag ")
                sQuery.Append(", 'Y' as ac_sale_price_display_flag, clitrans_value_description ")
                sQuery.Append(" FROM client_transactions  ")
                sQuery.Append(" inner JOIN client_aircraft on clitrans_cliac_id = cliaircraft_id ")
                sQuery.Append(" inner JOIN client_aircraft_model ON client_aircraft.cliaircraft_cliamod_id = client_aircraft_model.cliamod_id ")

                sQuery.Append(" LEFT OUTER JOIN client_transactions_company on clitcomp_trans_id = clitrans_id ")
                sQuery.Append(" LEFT OUTER JOIN client_transactions_contact on clitcontact_trans_id = clitrans_id ")

                sQuery.Append(" LEFT OUTER JOIN client_aircraft_reference ON client_aircraft_reference.cliacref_cliac_id = client_aircraft.cliaircraft_id ")
                sQuery.Append(" left outer join client_aircraft_engine on client_aircraft.cliaircraft_id=cliacep_cliac_id ")
                sQuery.Append(" LEFT OUTER JOIN client_aircraft_contact_type ON client_aircraft_reference.cliacref_contact_type = client_aircraft_contact_type.cliact_type")

            Else
                sQuery.Append("SELECT 'CLIENT' as source, '' as emp_program_name, clitrans_asking_price as ac_asking_price, clitrans_sold_price as ac_sale_price, NULL as ac_year,  clitrans_airframe_total_hours as ac_airframe_tot_hrs,  NULL as ac_engine_1_tot_hrs,   NULL as ac_engine_2_tot_hrs, NULL as ac_engine_3_tot_hrs, NULL as ac_engine_4_tot_hrs,  NULL as  ac_interior_moyear,  NULL as ac_exterior_moyear, NULL as ac_passenger_count, clitrans_jetnet_trans_id as client_jetnet_trans_id, clitrans_id as journ_id, clitrans_sold_price as ac_sold_price, clitrans_sold_price_type as ac_sold_price_type, clitrans_est_price as ac_take_price, clitrans_type as journ_subcategory_code, clitrans_date as journ_date, clitrans_subject as journ_subject")
                ', cliaircraft_ser_nbr_sort as ac_ser_no_sort, ")
                sQuery.Append(", clitrans_ser_nbr as ac_ser_no_sort")

                sQuery.Append(", 0 as ac_engine_1_soh_hrs, 0 as ac_engine_2_soh_hrs ")

                sQuery.Append(", clitrans_ser_nbr as ac_ser_no_full, clitrans_reg_nbr as ac_reg_no, clitrans_year_mfr as ac_mfr_year, clitrans_cliac_id as ac_id, cliamod_make_name as amod_make_name, clitrans_jetnet_ac_id as jetnet_ac_id")
                sQuery.Append(", clitrans_date_listed as ac_list_date, clitrans_airframe_total_hours as ac_airframe_tot_hrs, clitrans_customer_note as journ_customer_note, clitrans_retail_flag as retail_flag ")
                sQuery.Append(", 'Y' as ac_sale_price_display_flag, clitrans_asking_wordage  as ac_asking, clitrans_value_description ")
                sQuery.Append(" FROM client_transactions ")
                sQuery.Append("  INNER JOIN client_aircraft_model ON clitrans_cliamod_id = cliamod_id ")

                'sQuery.Append(" LEFT OUTER JOIN client_aircraft on clitrans_cliac_id = cliaircraft_id ")
            End If





            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE cliamod_jetnet_amod_id IN (" + tmpStr.Trim + ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE cliamod_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" WHERE cliamod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If Trim(use_only_used) <> "" Then
                sQuery.Append(use_only_used)
            End If

            If Trim(years_of) <> "" And Trim(years_of) <> "0" Then
                sQuery.Append(years_of)
            End If

            If Trim(aftt_within) <> "" And Trim(aftt_within) <> "0" Then
                sQuery.Append(aftt_within)
            End If

            If Trim(extra_criteria) <> "" Then
                sQuery.Append(extra_criteria)
            End If

            sQuery.Append(" and (clitrans_type='Full Sale') ")

            ' moved into combine function
            ' If isRetail = "Y" Then
            'sQuery.Append(" and clitrans_retail_flag = 'Y' ")
            ' End If

            ' only do if there is no extra ( only do if not from pdf) 
            If Trim(extra_criteria) = "" Then
                sQuery.Append(" AND clitrans_date >= '" & YearDateVariable & "'")


                If Trim(last_date) <> "" Then
                    YearDateVariable = Year(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, 0, CDate(last_date)))
                    sQuery.Append(" AND clitrans_date <= '" & YearDateVariable & "' ")
                End If
            End If


            sQuery.Append(" ORDER BY clitrans_date DESC")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)


            'If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
            '  If Trim(HttpContext.Current.Application.Item("crmClientDatabase")) = "" Then
            MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
            '  Else
            '    MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'crmClientConnectString
            '  End If
            'Else
            '  MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase") 'crmClientConnectString
            'End If



            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                class_error = "Error in Client_get_model_forsale_info load datatable " + constrExc.Message
            End Try



        Catch ex As Exception
            Return Nothing

            aError = "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"

        Finally

            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing

        End Try

        Return atemptable

    End Function

#End Region

#Region "Wanted Tab"
    Public Shared Sub Combined_views_display_model_wanteds(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef JetnetDataLayer As viewsDataLayer, ByVal DisplayLink As Boolean, Optional ByRef isCrmViewActive As Boolean = False)
        Dim ClientTable As New DataTable
        Dim JetnetTable As New DataTable

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Dim fAmwant_listed_date As String = ""
        Dim fInterested_party As String = ""
        Dim fAmwant_start_year As String = ""
        Dim fAmwant_end_year As String = ""
        Dim fAmwant_notes As String = ""
        Dim fAmwant_id As Long = 0
        Dim fComp_id As Long = 0
        Dim font_shrink As String = ""

        Try



            If DisplayLink Then
                font_shrink = "<font>"
            Else
                font_shrink = "<font size='-2'>"
            End If


            htmlOut.Append("<table border='0' width=""100%"" cellpadding='2' cellspacing='0'>")
            'Grabbing Client Data
            ClientTable = CLIENT_get_model_wanteds_info(searchCriteria)
            'Grabbing Jetnet Data
            JetnetTable = JetnetDataLayer.get_model_wanteds_info(searchCriteria, isCrmViewActive)
            'Merging/Sorting
            CombineTwoWantedDatatables(ClientTable, JetnetTable, results_table)

            If Not IsNothing(results_table) Then

                htmlOut.Append("<tr><td valign='top' bgcolor='#C0C0C0' align='center' colspan='2'>" & font_shrink & "WANTED MODELS <em>(" + results_table.Rows.Count.ToString + ")</em></font></td></tr>")
                htmlOut.Append("<tr><td align=""center"" colspan=""2"">")

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table width=""100%"" border='0' cellpadding='4' cellspacing='0'>")

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r("amwant_listed_date")) Then
                            fAmwant_listed_date = r.Item("amwant_listed_date").ToString.Trim
                        Else
                            fAmwant_listed_date = ""
                        End If

                        If Not IsDBNull(r("interested_party")) Then
                            fInterested_party = r.Item("interested_party").ToString.Trim
                        Else
                            fInterested_party = ""
                        End If

                        If Not IsDBNull(r("amwant_start_year")) Then
                            fAmwant_start_year = r.Item("amwant_start_year").ToString.Trim
                        Else
                            fAmwant_start_year = ""
                        End If

                        If Not IsDBNull(r("amwant_end_year")) Then
                            fAmwant_end_year = r.Item("amwant_end_year").ToString.Trim
                        Else
                            fAmwant_end_year = ""
                        End If

                        If Not IsDBNull(r("amwant_notes")) Then
                            fAmwant_notes = r.Item("amwant_notes").ToString.Trim
                        Else
                            fAmwant_notes = ""
                        End If

                        If Not IsDBNull(r("amwant_id")) Then
                            fAmwant_id = CLng(r.Item("amwant_id").ToString)
                        Else
                            fAmwant_id = 0
                        End If

                        If Not IsDBNull(r("comp_id")) Then
                            fComp_id = CLng(r.Item("comp_id").ToString)
                        Else
                            fComp_id = 0
                        End If

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        If DisplayLink Then
                            'htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='amwantid : " + r.Item("amwant_id").ToString + "' /></td>")
                            htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")
                        End If

                        If Not DisplayLink Then
                            htmlOut.Append("<td align='left' valign='top' class='seperator'>" & font_shrink & "<em>" & FormatDateTime(fAmwant_listed_date.ToString, DateFormat.ShortDate) + "</em> | ")
                        ElseIf UCase(r.Item("source")) = "JETNET" Then
                            htmlOut.Append("<td align='left' valign='top' class='seperator'><em><a href=""javascript:load('WantedDetails.aspx?id=" + fAmwant_id.ToString + "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">" + FormatDateTime(fAmwant_listed_date.ToString, DateFormat.ShortDate) + "</a></em> | ")
                        Else
                            htmlOut.Append("<td align='left' valign='top' class='seperator'><em><a href=""javascript:load('edit_note.aspx?action=edit&amp;type=wanted&amp;ViewID=1&amp;refreshing=prospect&amp;viewModelID=" & r.Item("amwant_amod_id").ToString & "&amp;id=" + fAmwant_id.ToString + "','','scrollbars=yes,menubar=no,height=600,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">" + FormatDateTime(fAmwant_listed_date.ToString, DateFormat.ShortDate) + "</a></em> | ")
                        End If

                        If DisplayLink Then
                            htmlOut.Append("<a target='_blank' href='DisplayCompanyDetail.aspx?compid=" + fComp_id.ToString + "'><strong>" + fInterested_party.Trim + "</strong></a><br />")
                        Else
                            htmlOut.Append("" & font_shrink & "<strong>" + fInterested_party.Trim + "</strong><br />")
                        End If

                        If Not String.IsNullOrEmpty(fAmwant_start_year) And Not String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : " + fAmwant_start_year.Trim)
                            htmlOut.Append(" - " + fAmwant_end_year.Trim)
                        ElseIf Not String.IsNullOrEmpty(fAmwant_start_year) And String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : " + fAmwant_start_year.Trim)
                        ElseIf String.IsNullOrEmpty(fAmwant_start_year) And Not String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("End Year : " + fAmwant_end_year.Trim)
                        ElseIf String.IsNullOrEmpty(fAmwant_start_year) And String.IsNullOrEmpty(fAmwant_end_year) Then
                            htmlOut.Append("Year : Open")
                        End If

                        If Not String.IsNullOrEmpty(fAmwant_notes) Then
                            htmlOut.Append(" " + Left(fAmwant_notes, 250))
                        End If

                        htmlOut.Append("</font></td></tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("No Wanteds at this time, for this Make/Model ...")
                End If

            Else
                htmlOut.Append("No Wanteds at this time, for this Make/Model ...")
            End If

            htmlOut.Append("</td></tr></table>" & vbCrLf)


        Catch ex As Exception

            class_error = "Error in views_display_model_wanteds(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Shared Function CLIENT_get_model_wanteds_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("select lnote_id as amwant_id, clicomp_name as interested_party, clicomp_jetnet_comp_id as comp_id, lnote_wanted_start_year as amwant_start_year, ")
            sQuery.Append(" lnote_wanted_end_year as amwant_end_year, lnote_wanted_max_price as amwant_max_price, lnote_wanted_max_aftt as ")
            sQuery.Append(" amwant_max_aftt, lnote_schedule_start_date as amwant_listed_date, cliamod_make_name as amod_make_name, ")
            sQuery.Append(" cliamod_model_name as amod_model_name, lnote_jetnet_amod_id as amwant_amod_id, lnote_note as amwant_notes, ")
            sQuery.Append(" NULL as amwant_year_note, NULL as amwant_amount_note, 0 as amwant_journ_id, '' as amwant_auto_distribute_replyname, ")
            sQuery.Append(" lnote_entry_date as amwant_entry_date, lnote_entry_date as amwant_verified_date, lnote_action_date as amwant_web_action_date, lnote_action_date as amwant_action_date, ")
            sQuery.Append(" NULL as amwant_auto_unsubscribe_date, '' as amwant_auto_distribute_email, NULL as auto_distribute_flag, lnote_jetnet_contact_id as amwant_contact_id, clicomp_jetnet_comp_id as amwant_comp_id, ")
            sQuery.Append(" '' as amwant_entry_user_id, lnote_wanted_damage_hist as amwant_accept_damage_hist, lnote_wanted_damage_cur as amwant_accept_damage_cur, ")
            sQuery.Append(" 'CLIENT' as source ")
            sQuery.Append(" from local_notes inner join client_company on lnote_client_comp_id = clicomp_id  ")
            sQuery.Append(" left outer join client_aircraft_model ")
            sQuery.Append(" on lnote_client_amod_id = cliamod_id where lnote_status = 'W' ")


            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" AND (lnote_jetnet_amod_id IN (" + tmpStr.Trim + ")) ")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" AND (lnote_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + ")")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" AND (cliamod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "')")
            End If


            sQuery.Append(" ORDER BY lnote_schedule_start_date DESC, cliamod_make_name, cliamod_model_name")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientDatabase")
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                class_error = "Error in CLIENT_get_model_wanteds_info load datatable " + constrExc.Message
            End Try


        Catch ex As Exception
            Return Nothing

            class_error = "Error in CLIENT_get_model_wanteds_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function
#End Region
#End Region

#Region "Get_Correct_Model"
    Public Shared Function CheckForCorrectModel(ByRef localCriteria As viewSelectionCriteriaClass, ByRef localDataLayer As viewsDataLayer, ByRef sharedModelTable As DataTable)
        Dim returnID As Long = 0
        'Dim aTempTable As New DataTable
        'Here we need to test the model to see if it's in the subscription
        If localCriteria.ViewCriteriaAmodID > 0 Then
            If Not IsNothing(sharedModelTable) Then
                If sharedModelTable.Rows.Count = 0 Then 'Refill up the table just in case it wasn't really there.
                    sharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
                End If
                If Not IsNothing(sharedModelTable) Then
                    If sharedModelTable.Rows.Count > 0 Then
                        'Model ID is fine
                        'No need to return anything except 0 here.
                        'returnID = aTempTable.Rows(0).Item("amod_id")
                    Else
                        'Model ID isn't fine. We need a new one.
                        returnID = 272
                        'Return_Correct_Model_Suggestion(localCriteria)
                    End If
                End If
            Else
                returnID = 272
            End If
        End If
        Return returnID
    End Function

    Public Shared Function Return_Correct_Model_Suggestion(ByRef localCriteria As viewSelectionCriteriaClass)
        Dim ReturnID As Long = 0
        If localCriteria.ViewCriteriaAmodID <= 0 And localCriteria.ViewID = 1 Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSelectedModels) Then
                Dim SeperateModels As String() = Split(HttpContext.Current.Session.Item("localUser").crmSelectedModels, ",")
                ReturnID = SeperateModels(0)
            End If

            If ReturnID = 0 Then
                'Let's look at Business First.
                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag Then
                    If HttpContext.Current.Session.Item("localSubscription").crmJets_Flag Then
                        ReturnID = 272
                    ElseIf HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag Then
                        ReturnID = 272
                    ElseIf HttpContext.Current.Session.Item("localSubscription").crmTurboprops Then
                        ReturnID = 175
                    End If
                ElseIf HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag Then
                    'Then we look at Helicopter
                    ReturnID = 400
                ElseIf HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag Then
                    'Finally we look at Commercial
                    ReturnID = 28
                End If
            End If
        End If

        Return ReturnID

    End Function
#End Region



    Public Shared Function Build_Compare_Graphs(ByVal ac_id As Long, ByVal NOTE_ID As Long, ByVal is_just_ac_spec As Boolean, ByVal internal As String, ByVal retail As String, ByRef localDatalayer As viewsDataLayer, ByRef ANALYTICS_HISTORY As System.Web.UI.DataVisualization.Charting.Chart, ByVal map_path As String, ByVal COMPLETED_OR_OPEN As String, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef Page1 As Page, ByRef bottom_tab_update_panel As UpdatePanel, ByVal recent_sales As Boolean, ByVal market_survey As Boolean, ByVal market_status As Boolean, ByVal sold_comparable As Boolean, ByVal current_market As Boolean, ByVal is_solo_pdf As Boolean, ByVal header_text_for_spec As String, Optional ByVal is_word As Boolean = True, Optional ByVal do_only_number As Integer = 0, Optional ByVal show_less_details As Boolean = False, Optional ByVal extra_sold_criteria As String = "", Optional ByVal extra_client_sold_criteria As String = "", Optional ByVal show_sales_in_last_months As Integer = 0, Optional ByVal amod_id_no_ac_id As Integer = 0) As String
        Build_Compare_Graphs = ""


        Dim charting_string As String = ""
        Dim google_map_array_list As String = ""
        Dim aircraft_history_string As String = ""
        Dim jetnet_ac_id As Long = 0
        Dim avg_asking As Long = 0
        Dim avg_take As Long = 0
        Dim avg_sold As Long = 0
        Dim temp_table As New DataTable
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
        Dim column4 As New DataColumn
        Dim column5 As New DataColumn
        Dim ClientIDSToExclude As String = ""   ' TEMP HOLD MSW
        Dim graph_per_page As Integer = 0
        Dim real_jetnet_ac_id As Long = 0
        Dim make_name As String = ""
        Dim model_name As String = ""
        Dim temp_amod_id As Long = 0
        Dim rest_of_ac_string As String = ""
        Dim location_string As String = ""


        Try
            ' If Trim(header_text_for_spec) <> "" Then
            '     location_string = "http://www.jetnet.com/"
            ' Else

            If HttpContext.Current.Request.IsSecureConnection = True Then
                location_string = "https://"
            Else
                location_string = "http://"
            End If
            location_string = HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim()



            ' If InStr(location_string, "jetnetcrmtest") > 0 Then
            '  location_string = "www.jetnetevolution.com"
            '  End If
            ' End If




            'If Trim(Request("internal")) <> "" Then
            '    internal = Trim(Request("internal"))
            'End If

            'If Trim(Request("retail")) <> "" Then
            '    retail = Trim(Request("retail"))
            'End If

            column.DataType = System.Type.GetType("System.Double")
            column.DefaultValue = 0
            column.Unique = False
            column.ColumnName = "asking_price"
            temp_table.Columns.Add(column)

            column2.DataType = System.Type.GetType("System.Double")
            column2.DefaultValue = 0
            column2.Unique = False
            column2.ColumnName = "take_price"
            temp_table.Columns.Add(column2)

            column3.DataType = System.Type.GetType("System.Double")
            column3.DefaultValue = 0
            column3.AllowDBNull = True
            column3.Unique = False
            column3.ColumnName = "sold_price"
            temp_table.Columns.Add(column3)


            column4.DataType = System.Type.GetType("System.DateTime")
            column4.AllowDBNull = True
            column4.Unique = False
            column4.ColumnName = "date_of"
            temp_table.Columns.Add(column4)


            column5.DataType = System.Type.GetType("System.String")
            column5.AllowDBNull = True
            column5.Unique = False
            column5.ColumnName = "ac_details"
            temp_table.Columns.Add(column5)




            If Not IsNothing(localDatalayer) Then
                real_jetnet_ac_id = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(ac_id, False)
            Else
                localDatalayer = New viewsDataLayer
                real_jetnet_ac_id = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(ac_id, False)
            End If

            localDatalayer.Get_AC_MAKE_MODEL(real_jetnet_ac_id, make_name, model_name, temp_amod_id, rest_of_ac_string, "")
            searchCriteria.ViewCriteriaAircraftID = real_jetnet_ac_id
            searchCriteria.ViewCriteriaAircraftMake = make_name
            searchCriteria.ViewCriteriaAircraftModel = model_name
            searchCriteria.ViewCriteriaAmodID = temp_amod_id

            '-------------------------------------
            jetnet_ac_id = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(ac_id, False)
            ANALYTICS_HISTORY.Series.Clear()
            ANALYTICS_HISTORY.Titles.Clear()

            If do_only_number = 0 Or do_only_number = 1 Then

                If is_solo_pdf = False Then

                    ANALYTICS_HISTORY.Width = 600

                    ANALYTICS_HISTORY.Titles.Add("My Aircraft Value History")
                    localDatalayer.views_analytics_graph_1(ac_id, ANALYTICS_HISTORY, aircraft_history_string, jetnet_ac_id, google_map_array_list, "O", NOTE_ID, False, "", True, show_sales_in_last_months)
                    ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                    ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_HISTORY.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

                    DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "Value History", "Aircraft Value ($k)", "chart_div_value_history", 600, 400, "POINTS", 1, charting_string, Page1, bottom_tab_update_panel, False, False)

                    'chart_div_value_history.InnerHtml = "<a href=" & chart_div_value_history.InnerHtml.ToString & ">Printable version</a>"
                    '  chart_div_value_history.Visible = True
                    Build_Compare_Graphs &= header_text_for_spec

                    Build_Compare_Graphs &= "<img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_HISTORY.jpg' width='600' height='400'>"

                    Build_Compare_Graphs &= Replace(aircraft_history_string, "table border='1' cellpadding='3' cellspacing='0'", "table border='1' cellpadding='3' cellspacing='0' align='center'")


                    Build_Compare_Graphs &= "</table>"
                    Build_Compare_Graphs &= "</td></tr></table>"
                    graph_per_page = 1

                End If
            End If

            '---------------------------------------------

            If is_just_ac_spec = False Then

                If do_only_number = 0 Or do_only_number = 2 Then       ' if its all, or its first one 

                    If current_market Then
                        ANALYTICS_HISTORY.Titles.Clear()
                        ANALYTICS_HISTORY.Series.Clear()
                        ANALYTICS_HISTORY.Titles.Add("Current Market Comparables")
                        ANALYTICS_HISTORY.Width = 550
                        aircraft_history_string = localDatalayer.views_analytics_graph_completed_current(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, COMPLETED_OR_OPEN, google_map_array_list)
                        'localDatalayer.views_analytics_graph_completed_current(Session.Item("CLIENT_AC_ID"), Me.ANALYTICS_CURRENT_MARKET, note_id, searchCriteria, "COMPLETED")

                        ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_CURRENT_MARKET.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

                        DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "Current Market Comparables", "Aircraft Value ($k)", "chart_div_survey", 600, 350, "POINTS", 2, charting_string, Page1, bottom_tab_update_panel, False, False)

                        '    Build_Compare_Graphs &= "<table width='10' height='100'><tr><td>&nbsp;</td></tr></table>"

                        If Trim(header_text_for_spec) <> "" Then
                            Build_Compare_Graphs &= Insert_Page_Break(is_word)
                            Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Market Comparables")
                        End If

                        Build_Compare_Graphs &= "<img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_CURRENT_MARKET.jpg' width='550' >"

                        Build_Compare_Graphs &= Replace(aircraft_history_string, "table border='1' cellpadding='3' cellspacing='0'", "table border='1' cellpadding='3' cellspacing='0' align='center'")
                        Build_Compare_Graphs &= "</table>"
                        Build_Compare_Graphs &= "</td></tr></table>"
                        graph_per_page = graph_per_page + 1
                    End If
                End If

                If do_only_number = 0 Or do_only_number = 3 Then
                    If sold_comparable Then
                        ANALYTICS_HISTORY.Titles.Clear()
                        ANALYTICS_HISTORY.Series.Clear()
                        ANALYTICS_HISTORY.Titles.Add("Sold Comparables")
                        ANALYTICS_HISTORY.Width = 600
                        aircraft_history_string = ""
                        aircraft_history_string = localDatalayer.views_analytics_graph_2(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, "TRANS", Nothing, google_map_array_list, COMPLETED_OR_OPEN)
                        ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_SOLD_COMPARABLES.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

                        DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "Sold Comparables", "Aircraft Value ($k)", "chart_div_survey", 600, 230, "POINTS", 3, charting_string, Page1, bottom_tab_update_panel, False, False)


                        If Trim(header_text_for_spec) <> "" Then
                            Build_Compare_Graphs &= Insert_Page_Break(is_word)
                            Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Sold Comparables")
                        End If

                        'If graph_per_page = 2 Then
                        '  Build_Compare_Graphs &= Insert_Page_Break()
                        '    graph_per_page = 0
                        'Else
                        '    Build_Compare_Graphs &= "<table width='10' height='100'><tr><td>&nbsp;</td></tr></table>"
                        'End If

                        Build_Compare_Graphs &= "<Tr><td align='center'><img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_SOLD_COMPARABLES.jpg' width='600' ></td></tr>"

                        If Trim(header_text_for_spec) <> "" Then
                            Build_Compare_Graphs &= "<Tr><td align='center'>" & aircraft_history_string & "</td></tr>"
                        End If
                        Build_Compare_Graphs &= "</table>"
                        Build_Compare_Graphs &= "</td></tr></table>"
                        graph_per_page = graph_per_page + 1
                    End If
                End If


                If do_only_number = 0 Or do_only_number = 4 Then
                    If Trim(COMPLETED_OR_OPEN) <> "C" Then


                        If market_status Then
                            ' set the title and load the data into the chart control 
                            ANALYTICS_HISTORY.Titles.Clear()
                            ANALYTICS_HISTORY.Titles.Add("Market Status")

                            aircraft_history_string = ""
                            crmViewDataLayer.Combined_views_display_fleet_market_summary(searchCriteria, "", aircraft_history_string, localDatalayer, ClientIDSToExclude, True, False, avg_asking, avg_take, avg_sold)


                            localDatalayer.views_analytics_graph_market_status(ac_id, ANALYTICS_HISTORY, avg_asking, avg_take, avg_sold, google_map_array_list, COMPLETED_OR_OPEN, amod_id_no_ac_id)
                            ANALYTICS_HISTORY.Width = 600

                            ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                            ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_MARKET_STATUS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

                            DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "", "Aircraft Value ($k)", "chart_div_survey", 600, 350, "POINTS", 4, charting_string, Page1, bottom_tab_update_panel, False, False)


                            If Trim(header_text_for_spec) <> "" Then
                                Build_Compare_Graphs &= Insert_Page_Break(is_word)
                                Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Market Status")
                            End If

                            'If graph_per_page = 2 Then
                            '  Build_Compare_Graphs &= Insert_Page_Break()
                            '    graph_per_page = 0
                            'Else
                            '    Build_Compare_Graphs &= "<table width='10' height='100'><tr><td>&nbsp;</td></tr></table>"
                            'End If

                            Build_Compare_Graphs &= "<img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_MARKET_STATUS.jpg' width='600'>"

                            If Trim(header_text_for_spec) <> "" Then
                                aircraft_history_string = Replace(aircraft_history_string, "<td valign='top'", "<td valign='top' class='small_header_text'")
                                aircraft_history_string = Replace(aircraft_history_string, "class='seperator'", "")
                                aircraft_history_string = Replace(aircraft_history_string, "class='rightside'", "")
                                aircraft_history_string = Replace(aircraft_history_string, "class='border_bottom'", "class='small_header_text'")
                                aircraft_history_string = Replace(aircraft_history_string, "class='sub_table'", "align='center' border='1'")
                                '
                                Build_Compare_Graphs &= "<Tr><td align='center'>" & aircraft_history_string & "</td></tr>"
                            End If
                            Build_Compare_Graphs &= "</table>"
                            Build_Compare_Graphs &= "</td></tr></table>"
                            graph_per_page = graph_per_page + 1
                        End If

                    End If
                End If


                If do_only_number = 0 Or do_only_number = 5 Then
                    If market_survey Then

                        '-------------------- ONLY FOR THE SPEC--------------------------
                        If Trim(header_text_for_spec) <> "" Then
                            aircraft_history_string = ""
                            If Trim(ClientIDSToExclude) = "" Then
                                crmViewDataLayer.Combined_views_display_fleet_market_summary(searchCriteria, "", "", localDatalayer, ClientIDSToExclude, True, False, 0, 0, 0)
                            End If

                            crmViewDataLayer.Build_For_sale_tab(searchCriteria, aircraft_history_string, NOTE_ID, "", 19, True, "", False, "", localDatalayer, "", ClientIDSToExclude, ac_id, False, 16, header_text_for_spec, is_word)
                        End If
                        '-------------------- ONLY FOR THE SPEC--------------------------

                        ANALYTICS_HISTORY.Titles.Clear()
                        ANALYTICS_HISTORY.Series.Clear()
                        ANALYTICS_HISTORY.Titles.Add("Market Survey")
                        If amod_id_no_ac_id > 0 Then
                            localDatalayer.views_analytics_graph_2_Survey(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, ClientIDSToExclude, google_map_array_list, COMPLETED_OR_OPEN, True)
                        Else
                            localDatalayer.views_analytics_graph_2_Survey(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, ClientIDSToExclude, google_map_array_list, COMPLETED_OR_OPEN, False)
                        End If
                        ANALYTICS_HISTORY.Width = 700
                        ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_MARKET_SURVEY.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)


                        DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "Market Survey", "Aircraft Value ($k)", "chart_div_survey", 700, 400, "POINTS", 5, charting_string, Page1, bottom_tab_update_panel, True, False)



                        If Trim(header_text_for_spec) <> "" Then
                            Build_Compare_Graphs &= Insert_Page_Break(is_word)
                            Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Market Survey")
                        End If

                        'If graph_per_page = 2 Then
                        '  Build_Compare_Graphs &= Insert_Page_Break()
                        '    graph_per_page = 0
                        'Else
                        '    Build_Compare_Graphs &= "<table width='10' height='100'><tr><td>&nbsp;</td></tr></table>"
                        'End If

                        Build_Compare_Graphs &= "<img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_MARKET_SURVEY.jpg' width='700'>"

                        If do_only_number = 0 Then   ' if running by itself, then we ran text seperately
                            If Trim(header_text_for_spec) <> "" Then
                                aircraft_history_string = Replace(aircraft_history_string, "class='forSaleCellBorder'", "class='small_header_text'")
                                aircraft_history_string = Replace(aircraft_history_string, "class='forSaleCellBorderNoNotes'", "class='small_header_text'")
                                Build_Compare_Graphs &= "" & aircraft_history_string & ""
                            End If
                        End If

                        Build_Compare_Graphs &= "</table>"
                        Build_Compare_Graphs &= "</td></tr></table>"

                        graph_per_page = graph_per_page + 1
                    End If
                End If


                If do_only_number = 0 Or do_only_number = 6 Then

                    If recent_sales Then

                        ANALYTICS_HISTORY.Titles.Clear()
                        ANALYTICS_HISTORY.Series.Clear()
                        ANALYTICS_HISTORY.Titles.Add("Sold Survey")
                        ANALYTICS_HISTORY.Width = 725


                        If Trim(header_text_for_spec) <> "" Then ' display links should be false 
                            crmViewDataLayer.Combined_views_display_recent_retail_sales(searchCriteria, rest_of_ac_string, localDatalayer, True, False, temp_table, internal, retail, False, NOTE_ID, ac_id, "", 0, 30, header_text_for_spec, is_word, "", "", "", "", "", "", "", "", "", 0, "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", False, True, extra_sold_criteria, extra_client_sold_criteria)
                        Else
                            crmViewDataLayer.Combined_views_display_recent_retail_sales(searchCriteria, rest_of_ac_string, localDatalayer, True, True, temp_table, internal, retail, False, NOTE_ID, ac_id, "", 0, 0, "", is_word, "", "", "", "", "", "", "", "", "", 0, "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", False, True, extra_sold_criteria, extra_client_sold_criteria)
                        End If

                        If amod_id_no_ac_id > 0 Then
                            localDatalayer.views_analytics_graph_2(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, "RECENT", temp_table, google_map_array_list, COMPLETED_OR_OPEN, show_less_details, True)
                        Else
                            localDatalayer.views_analytics_graph_2(ac_id, ANALYTICS_HISTORY, NOTE_ID, searchCriteria, "RECENT", temp_table, google_map_array_list, COMPLETED_OR_OPEN, show_less_details)
                        End If

                        ANALYTICS_HISTORY.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        ANALYTICS_HISTORY.SaveImage(map_path + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_RECENT_SALES.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

                        DisplayFunctions.load_google_chart(Nothing, google_map_array_list, "Sold Survey", "Aircraft Value ($k)", "chart_div_survey", 700, 700, "POINTS", 6, charting_string, Page1, bottom_tab_update_panel, True, False)


                        If Trim(header_text_for_spec) <> "" Then
                            Build_Compare_Graphs &= Insert_Page_Break(is_word)
                            Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Sold Survey")
                        End If

                        'If graph_per_page = 2 Then
                        ' Build_Compare_Graphs &= Insert_Page_Break()
                        '    graph_per_page = 0
                        'Else
                        '    Build_Compare_Graphs &= "<table width='10' height='100'><tr><td>&nbsp;</td></tr></table>"
                        'End If



                        Build_Compare_Graphs &= "<img src='" & location_string & "/TempFiles/" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & ac_id & "_ANALYTICS_RECENT_SALES.jpg' width='725'>"




                        If Trim(header_text_for_spec) <> "" Then
                            rest_of_ac_string = Replace(rest_of_ac_string, "class='seperator'", "class='small_header_text'")
                            Build_Compare_Graphs &= "</td></tr></table>"
                            Build_Compare_Graphs &= "</table></td></tr></table>"
                            Build_Compare_Graphs &= Insert_Page_Break(is_word)
                            Build_Compare_Graphs &= Replace(header_text_for_spec, "Market Value Analysis", "Value Analysis - Sold Survey")
                            Build_Compare_Graphs &= rest_of_ac_string
                        End If
                        Build_Compare_Graphs &= "</td></tr></table>"
                        Build_Compare_Graphs &= "</table></td></tr></table>"


                        graph_per_page = graph_per_page + 1
                    End If
                End If


            Else
                ' in a closed analysis, look up pics
                ' Build_Compare_Graphs &= "<img src='http://" & location_string & "/TempFiles/" & ac_id & "_" & NOTE_ID & "_COMPLETE_ANALYTICS_MARKET_STATUS.jpg' width='500'>"

                ' Build_Compare_Graphs &= "<img src='http://" & location_string & "/TempFiles/" & ac_id & "_" & NOTE_ID & "_COMPLETE_ANALYTICS_MARKET_SURVEY.jpg' width='700'>"

                '  Build_Compare_Graphs &= "<img src='http://" & location_string & "/TempFiles/" & ac_id & "_" & NOTE_ID & "_COMPLETE_ANALYTICS_RECENT_SALES.jpg' width='700'>"

            End If



        Catch ex As Exception

        End Try

    End Function
    Public Shared Sub get_valuation_fields_from_layout(ByVal NOTE_ID As Long, ByRef db_fields As Array, ByRef name_fields As Array, ByRef field_count As Integer, ByRef select_string As String, ByVal REPORT_TYPE As String, ByVal primary_or_compare As String, ByVal export_id As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef order_by_string As String, ByRef primary_custom_fields As String, ByVal get_primary As Boolean)


        Dim Query As String = ""
        Dim results_table As New DataTable
        Dim results_table_inner As New DataTable
        Dim temp_string As String = ""
        Dim temp_note As String = ""
        Dim Query2 As String = ""
        Dim aTempTable As New DataTable
        Dim aTempTable2 As New DataTable
        Dim size_string As String = ""
        Dim type_string As String = ""
        Dim spot1 As Long = 0
        Dim fields_string As String = ""
        Dim client_fields_from_custom As String = ""
        Dim temp1 As String = ""
        Dim temp1_org As String = ""
        Dim match_found As Boolean = True
        Dim temp_header As String = ""

        Try

            aTempTable = aclsData_Temp.Client_Project_Reference_Details_By_Project_ID(export_id)
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
                                    ' info_to_export.Items.Add(New ListItem(r("cliexp_display"), r("cliexp_id") & "|" & r("cliexp_client_db_name") & "|" & r("cliexp_jetnet_db_name")))


                                    If get_primary = True Then
                                        If Not IsDBNull(r("cliexp_client_primary_db_name")) Then
                                            If Trim(r("cliexp_client_primary_db_name")) <> "" Then
                                                temp1 = Trim(r("cliexp_client_primary_db_name"))
                                            Else
                                                temp1 = ""
                                            End If
                                        Else
                                            temp1 = ""
                                        End If
                                    Else
                                        If Not IsDBNull(r("cliexp_client_db_name")) Then
                                            temp1 = Trim(r("cliexp_client_db_name"))
                                        Else
                                            temp1 = ""
                                        End If
                                    End If


                                    If Trim(temp1) <> "" Then
                                        temp1_org = temp1

                                        temp_header = ""
                                        If Not IsDBNull(r("cliexp_header_field_name")) Then
                                            If Trim(r("cliexp_header_field_name")) <> "" Then
                                                temp_header = Trim(r("cliexp_header_field_name"))
                                            End If
                                        End If

                                        match_found = False
                                        For i = 0 To field_count - 1
                                            If Trim(db_fields(i)) = Trim(temp1) Then
                                                match_found = True
                                            ElseIf (Trim(temp1) = "cliamod_make_name" Or Trim(temp1) = "cliamod_model_name" Or Trim(temp1) = "cliaircraft_ser_nbr") Then
                                                match_found = True
                                            ElseIf (Trim(temp_header) = "MAKE" Or Trim(temp_header) = "MODEL" Or Trim(temp_header) = "SERNBR") Then
                                                match_found = True
                                            End If
                                        Next

                                        If match_found = True Then
                                            match_found = match_found
                                        Else
                                            If InStr(Trim(temp1), " as ") > 0 Then
                                                temp1 = Right(temp1, (Len(temp1) - InStr(Trim(temp1), " as ") - 3))
                                            End If

                                            ' if its a sub select, then put it in the header, which will put it in the order by 
                                            If InStr(Trim(temp1_org), "SELECT ") > 0 Then
                                                temp1_org = temp1_org & " as " & Trim(temp_header)
                                                temp1 = Trim(temp_header)
                                            End If

                                            db_fields(field_count) = Replace(temp1, "'", "")


                                            If Trim(temp_header) <> "" Then
                                                name_fields(field_count) = Trim(temp_header)
                                            Else
                                                name_fields(field_count) = temp1
                                            End If


                                            If Trim(db_fields(field_count)) <> "" Then
                                                If field_count = 0 Then
                                                    select_string = temp1_org & " "
                                                    order_by_string = temp1 & " asc "
                                                Else
                                                    If Trim(select_string) <> "" Then
                                                        select_string = select_string & ", " & temp1_org & " "
                                                        order_by_string = order_by_string & ", " & temp1 & " asc "
                                                    Else
                                                        select_string = temp1_org & " "
                                                        order_by_string = temp1 & " asc "
                                                    End If
                                                End If
                                            End If


                                            field_count = field_count + 1
                                        End If
                                    Else
                                        temp1 = ""
                                    End If


                                    'If Not IsDBNull(r("cliexp_header_field_name")) Then
                                    '  If Trim(client_fields_from_custom) <> "" Then
                                    '    client_fields_from_custom = client_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                    '  Else
                                    '    client_fields_from_custom = client_fields_from_custom & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                    '  End If
                                    'Else
                                    '  If Trim(client_fields_from_custom) <> "" Then
                                    '    client_fields_from_custom = client_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_display") & "' "
                                    '  Else
                                    '    client_fields_from_custom = client_fields_from_custom & temp_string & " as '" & r("cliexp_display") & "' "
                                    '  End If
                                    'End If


                                Next
                            End If
                        End If
                    Next
                End If
            End If







        Catch ex As Exception
        Finally
        End Try


    End Sub

    Public Shared Sub Build_For_sale_tab(ByRef searchCriteria As viewSelectionCriteriaClass,
                                          ByRef out_htmlString As String,
                                          ByVal NOTE_ID As Long,
                                          ByRef EXCEL_FILE_NAME As String,
                                          ByVal View_ID As Integer,
                                          ByVal CRMViewActive As Boolean,
                                          ByVal extra_criteria As Boolean,
                                          ByVal allow_export As String,
                                          ByVal JetnetClientSourceText As String,
                                          ByRef localDatalayer As crmWebClient.viewsDataLayer,
                                          ByRef LAST_SAVE_DATE As String,
                                          ByRef ClientIDSToExclude As String,
                                          ByRef CLIENT_AC_ID As Long,
                                          ByVal display_link As Boolean,
                                          ByVal page_break_after As Integer,
                                          ByVal header_text As String,
                                          Optional ByVal is_word As Boolean = True,
                                          Optional ByVal export_id As Integer = 0,
                                          Optional ByVal aclsData_Temp As clsData_Manager_SQL = Nothing,
                                          Optional ByRef ActiveTabIndex As Long = 1,
                                          Optional ByVal run_comparable_insert As Boolean = False, Optional ByVal CurrentForSaleIDs As String = "", Optional ByVal UseModelValueOnly As Boolean = False, Optional ByRef PassBackDataTable As DataTable = Nothing, Optional ByVal displayEValues As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim cur_date_string As String = ""
        Dim temp_for_sale_text As String = ""
        Dim htmlForSale As String = ""
        Dim jetnet_fields_from_custom As String = ""
        Dim client_fields_from_custom As String = ""
        Dim order_by_string As String = ""
        Dim fields_string As String = ""

        Dim type_string As String = ""
        Dim size_string As String = ""

        Dim aTempTable As New DataTable
        Dim aTempTable2 As New DataTable
        Dim spot1 As Integer = 0
        Dim temp_string As String = ""
        Dim abrevs As String = ""
        Dim key_text As String = ""
        Dim abrevs_count As Integer = 0

        Dim htmlOut_Export As New StringBuilder
        Dim htmlForSale_export As String = ""

        Try


            If clsGeneral.clsGeneral.isCrmDisplayMode() = True Then
                If aclsData_Temp.client_DB = "" Then
                    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
                End If
            End If

            cur_date_string &= Replace(Now.Hour, "/", "_") + "_"
            cur_date_string &= Replace(Now.Minute, "/", "_") + "_"
            cur_date_string &= Replace(Now.Second, "/", "_")
            EXCEL_FILE_NAME = HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & cur_date_string & "_REPORT_ForSale.xls"

            If CRMViewActive = True Then
                If display_link = True Then
                    'If allow_export = True Then
                    '  htmlOut.Append("&nbsp;&nbsp;<a href='/TempFiles/" & EXCEL_FILE_NAME & "' target='blank'>Export To Excel</a>")
                    'End If

                    If NOTE_ID > 0 Then
                        htmlOut.Append("<br><font size='-6'>" & JetnetClientSourceText)
                        If UseModelValueOnly = False Then
                            htmlOut.Append("&nbsp;Click on the green (+) and ($) icons to add and remove current market comparables from the current market value analysis.</font>")
                        End If
                    Else
                        htmlOut.Append("&nbsp;<font size='-6'>" & JetnetClientSourceText)
                    End If
                End If
            End If

            If (View_ID < 2) Or View_ID = 11 Or View_ID = 19 Then
                If page_break_after = 0 Then        ' dont show for spec
                    ' htmlOut.Append("<div style='height:370px; width:970px; overflow: auto;'><p>")
                End If
            End If

            htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
            htmlOut.Append("<tr><td align=""left"" valign=""top"">")

            htmlOut_Export.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
            htmlOut_Export.Append("<tr><td align=""left"" valign=""top""><table id='modelForsaleViewTopTable' width=""100%"" cellpadding=""3"" cellspacing=""0"">")


            If display_link Then

                If Not CRMViewActive Then

                    htmlOut.Append("<tr valign='top'><td align=""left"" valign=""top"">")

                    'If allow_export Then
                    '  htmlOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;<a href='/TempFiles/" + EXCEL_FILE_NAME + "' target='blank'>Export To Excel</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                    'End If
                    If HttpContext.Current.Session.Item("isMobile") = False Then
                        If View_ID = 19 Then

                            If extra_criteria Then
                                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&noteID=" + NOTE_ID.ToString + "&activetab=4"" class=""padding"">Hide Extra Criteria</a>")
                            Else
                                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&extra=true&noteID=" + NOTE_ID.ToString + "&activetab=4"" class=""padding"">View Extra Criteria</a>")
                            End If

                        Else

                            If extra_criteria Then
                                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaSortBy.Trim), "&sortby=" + searchCriteria.ViewCriteriaSortBy.Trim, "") + "&activetab=1"" class=""padding"">Hide Extra Criteria</a>")
                            Else
                                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + IIf(Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaSortBy.Trim), "&sortby=" + searchCriteria.ViewCriteriaSortBy.Trim, "") + "&extra=true&activetab=1"" class=""padding"">View Extra Criteria</a>")
                            End If

                        End If
                    End If
                    htmlOut.Append("</td></tr><tr><td align=""left"" valign=""top"">")

                End If

            End If

            If CDbl(export_id) = 0 Or CDbl(export_id) = -1 Then

                If IsNothing(HttpContext.Current.Session.Item("COMPARE_FOR_SALE_TABLE")) Then
                    HttpContext.Current.Session.Item("COMPARE_FOR_SALE_TABLE") = New DataTable
                End If

                If CRMViewActive Then
                    'We need to use a special view for this
                    crmViewDataLayer.views_display_aircraft_forsale(searchCriteria, htmlForSale, extra_criteria, localDatalayer, CRMViewActive, ClientIDSToExclude, display_link, HttpContext.Current.Session.Item("COMPARE_FOR_SALE_TABLE"), NOTE_ID, CLIENT_AC_ID, LAST_SAVE_DATE, page_break_after, header_text, is_word, "", "", "", "", "", "", "", htmlForSale_export, ActiveTabIndex, run_comparable_insert, CurrentForSaleIDs, UseModelValueOnly, PassBackDataTable, displayEValues)
                Else
                    localDatalayer.views_display_aircraft_forsale(searchCriteria, htmlForSale, extra_criteria, displayEValues)
                End If

                ''If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                'htmlForSale = "<div style=""width:1100px;overflow:none;"">" & htmlForSale & "</div>"
                ''End If

                htmlOut.Append(htmlForSale)

            Else

                '-------------------------------------FOR A PROJECT--------------------------------- --------------------------------------
                If Not IsNothing(aclsData_Temp) Then
                    jetnet_fields_from_custom = ""
                    client_fields_from_custom = ""


                    aTempTable = aclsData_Temp.Client_Project_Reference_Details_By_Project_ID(export_id)
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In aTempTable.Rows

                                If Trim(q("clipref_source")) = "JETNET" Then
                                    aTempTable2 = aclsData_Temp.Build_Export_byID(q("clipref_exp_id"))
                                Else
                                    aTempTable2 = aclsData_Temp.Build_Custom_Export_byID(q("clipref_exp_id"))
                                End If

                                If Not IsNothing(aTempTable2) Then
                                    If aTempTable2.Rows.Count > 0 Then
                                        For Each r As DataRow In aTempTable2.Rows

                                            If Not IsDBNull(r("cliexp_field_length")) Then
                                                temp_string = Trim(r("cliexp_field_length"))
                                            Else
                                                temp_string = " "
                                            End If

                                            If Trim(size_string) <> "" Then
                                                size_string = size_string & ", '" & temp_string & "' "
                                            Else
                                                size_string = size_string & "'" & temp_string & "' "
                                            End If


                                            If Not IsDBNull(r("cliexp_field_type")) Then
                                                temp_string = Trim(r("cliexp_field_type"))
                                            Else
                                                temp_string = " "
                                            End If

                                            If Trim(type_string) <> "" Then
                                                type_string = type_string & ", '" & temp_string & "' "
                                            Else
                                                type_string = type_string & "'" & temp_string & "' "
                                            End If




                                            temp_string = Trim(r("cliexp_client_db_name"))

                                            If InStr(Trim(temp_string), " as ") > 0 Then
                                                spot1 = InStr(Trim(temp_string), " as ")
                                                temp_string = Left(Trim(temp_string), spot1)
                                            End If

                                            If Trim(fields_string) <> "" Then
                                                fields_string = fields_string & ", '" & temp_string & "' "
                                            Else
                                                fields_string = fields_string & "'" & temp_string & "' "
                                            End If


                                            If Not IsDBNull(r("cliexp_header_field_name")) Then
                                                If Trim(client_fields_from_custom) <> "" Then
                                                    client_fields_from_custom = client_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                                Else
                                                    client_fields_from_custom = client_fields_from_custom & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                                End If
                                            Else
                                                If Trim(client_fields_from_custom) <> "" Then
                                                    client_fields_from_custom = client_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_display") & "' "
                                                Else
                                                    client_fields_from_custom = client_fields_from_custom & temp_string & " as '" & r("cliexp_display") & "' "
                                                End If
                                            End If

                                            temp_string = Trim(r("cliexp_jetnet_db_name"))

                                            If InStr(Trim(temp_string), " as ") > 0 Then
                                                spot1 = InStr(Trim(temp_string), " as ")
                                                temp_string = Left(Trim(temp_string), spot1)
                                            End If


                                            If Not IsDBNull(r("cliexp_header_field_name")) Then
                                                If Trim(jetnet_fields_from_custom) <> "" Then
                                                    jetnet_fields_from_custom = jetnet_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                                Else
                                                    jetnet_fields_from_custom = jetnet_fields_from_custom & temp_string & " as '" & r("cliexp_header_field_name") & "' "
                                                End If


                                                If abrevs_count = 0 Then
                                                    abrevs &= ("<tr bgcolor='white'>")
                                                    abrevs_count = 1
                                                Else
                                                    abrevs &= ("<tr class='alt_row'>")
                                                    abrevs_count = 0
                                                End If

                                                abrevs &= ("<td align='left' valign='middle' class='seperator'>")
                                                abrevs &= (r("cliexp_header_field_name"))
                                                abrevs &= ("</td><td align='left' valign='middle' class='seperator'>")
                                                abrevs &= (r("cliexp_display"))
                                                abrevs &= ("</td></tr>")

                                            Else
                                                If Trim(jetnet_fields_from_custom) <> "" Then
                                                    jetnet_fields_from_custom = jetnet_fields_from_custom & ", " & temp_string & " as '" & r("cliexp_display") & "' "
                                                Else
                                                    jetnet_fields_from_custom = jetnet_fields_from_custom & temp_string & " as '" & r("cliexp_display") & "' "
                                                End If
                                            End If


                                            If Not IsDBNull(r("cliexp_header_field_name")) Then
                                                If Trim(order_by_string) <> "" Then
                                                    order_by_string = order_by_string & ", '" & r("cliexp_header_field_name") & "' "
                                                Else
                                                    order_by_string = order_by_string & "'" & r("cliexp_header_field_name") & "' "
                                                End If
                                            Else
                                                If Trim(order_by_string) <> "" Then
                                                    order_by_string = order_by_string & ", '" & r("cliexp_display") & "' "
                                                Else
                                                    order_by_string = order_by_string & "'" & r("cliexp_display") & "' "
                                                End If
                                            End If

                                        Next
                                    End If
                                End If
                            Next
                        End If
                    End If


                    crmViewDataLayer.views_display_aircraft_forsale(searchCriteria, htmlForSale, Trim(extra_criteria), localDatalayer, CRMViewActive, ClientIDSToExclude, display_link, Nothing, NOTE_ID, CLIENT_AC_ID, LAST_SAVE_DATE, page_break_after, header_text, is_word, jetnet_fields_from_custom, client_fields_from_custom, order_by_string, fields_string, type_string, size_string, "", htmlForSale_export, 0, False, CurrentForSaleIDs, UseModelValueOnly, PassBackDataTable)
                    '-------------------------------------FOR A PROJECT--------------------------------- --------------------------------------

                    'If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                    '  htmlForSale = "<div style=""width:1100px;overflow:auto;"">" & htmlForSale & "</div>"
                    'End If

                    htmlOut.Append(htmlForSale)

                    htmlForSale = htmlForSale_export



                End If

            End If



            key_text = ""
            'If CDbl(export_id) > 0 Then
            '  key_text &= ("<table id='modelForsaleViewBottomTable' width=""40%"" cellpadding=""4"" cellspacing=""0"">")
            '  key_text &= ("<tr><td align=""left"" valign=""top"">")
            '  key_text &= ("<table id='modelForsaleViewStatusTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")
            '  key_text &= ("<tr><td align='center' valign='middle' class='header' bgcolor='#EEEEEE' colspan='2' style='padding-left:3px;'>HEADING KEY</td></tr>")
            '  key_text &= (abrevs)
            '  key_text &= ("</table>")
            '  key_text &= ("</td></tr></table>")
            '  htmlOut.Append(key_text)
            '  htmlForSale &= key_text
            'End If


            If Trim(htmlForSale_export) <> "" Then
                temp_for_sale_text = Trim(htmlForSale_export)
            Else
                temp_for_sale_text = Trim(htmlForSale)
            End If




            ' MSW, replace the styles with colors for excel
            temp_for_sale_text = Replace(temp_for_sale_text, "width='100%'", "width='500'")
            temp_for_sale_text = Replace(temp_for_sale_text, "class='alt_row'", "bgcolor='#EEEEEE'")
            temp_for_sale_text = Replace(temp_for_sale_text, "<img src='images/Notes.gif' border='0'>", "") ' added mse replace bad image


            temp_for_sale_text = Replace(temp_for_sale_text, "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "JETNET")
            temp_for_sale_text = Replace(temp_for_sale_text, "<img src='images/client.png' alt='CLIENT RECORD' width='15' />", "CLIENT")

            CommonAircraftFunctions.Build_String_To_HTML(temp_for_sale_text, EXCEL_FILE_NAME)

            ' if you have picked first one or there 
            If CDbl(export_id) = 0 Or CDbl(export_id) = -1 Then
                ' if its for spec---------------------
                If page_break_after > 0 Then
                    'htmlOut.Append("</td></tr></table></td></tr></table></td></tr></table>")  '  for the 3 table row columns made before this function
                    'htmlOut.Append("</table></td></tr></table>") ' for the header
                    'htmlOut.Append(Insert_Page_Break())
                    'htmlOut.Append(Replace(header_text, "Market Value Analysis", "Value Analysis - Market Survery (FEAT)"))
                    'htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module""><tr><td align=""left"" valign=""top"">")
                    'htmlOut.Append("<table id='modelForsaleViewTopTable' width=""100%"" cellpadding=""4"" cellspacing=""0""><tr><td align=""left"" valign=""top"">")

                    '  htmlOut.Append("<table id='modelForsaleViewBottomTable' width=""70%"" cellpadding=""4"" cellspacing=""0"">")
                    ' htmlOut.Append("<tr><td align=""left"" valign=""top"">")
                Else

                    'htmlOut.Append("<table id='modelForsaleViewBottomTable' width=""70%"" cellpadding=""4"" cellspacing=""0"">")
                    'htmlOut.Append("<tr><td align=""left"" valign=""top"">")


                    Dim htmlStandardFeatures As String = ""
                    localDatalayer.display_standard_model_features(searchCriteria, htmlStandardFeatures, display_link, False)
                    htmlOut.Append("<br clear=""all"" /><p>" & htmlStandardFeatures & "</p>")

                    'htmlOut.Append("</td>")
                    'Removed the model image on the bottom of the forsale tab per Rick's instructions on 4/28/2014
                    '<td align=""left"" valign=""top"">")

                    'Dim htmlModelPic As String = ""
                    'localDatalayer.views_display_model_pic(searchCriteria, htmlModelPic)
                    'htmlOut.Append(htmlModelPic)

                    'htmlOut.Append("</td>"
                    'htmlOut.Append("<td align=""left"" valign=""top"">")
                    If display_link = True Then
                        '  htmlOut.Append("<table id='modelForsaleViewStatusTable' width='100%' cellpadding='4' cellspacing='0' class='module'>")
                        '  htmlOut.Append("<tr><td align='center' valign='middle' class='seperator' colspan='2' style='padding-left:3px;'>ASKING ABBREVIATIONS</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Price</td><td align='left' valign='middle' class='seperator'>1,800K</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Make Offer</td><td align='left' valign='middle' class='seperator'>M/O</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Share</td><td align='left' valign='middle' class='seperator'>SHARE</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Lease</td><td align='left' valign='middle' class='seperator'>LS</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Sale/Lease</td><td align='left' valign='middle' class='seperator'>FS/LS</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Lease Only</td><td align='left' valign='middle' class='seperator'>LS/O</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Sealed Bid</td><td align='left' valign='middle' class='seperator'>BID</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Trade</td><td align='left' valign='middle' class='seperator'>TRD</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Sale/Trade</td><td align='left' valign='middle' class='seperator'>FS/TRD</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Sale/Share</td><td align='left' valign='middle' class='seperator'>FS/SH</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>No Engines</td><td align='left' valign='middle' class='seperator'>NO/ENG</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Auction</td><td align='left' valign='middle' class='seperator'>AUC</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Unconfirmed</td><td align='left' valign='middle' class='seperator'>UNC</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Confidential</td><td align='left' valign='middle' class='seperator'>CONF</td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='seperator'>Sale Pending</td><td align='left' valign='middle' class='seperator'>SP</td></tr>")
                        '  htmlOut.Append("</table>") ' modelForsaleViewStatusTable
                    Else
                        '  htmlOut.Append("<table id='modelForsaleViewStatusTable' width='100%' cellpadding='4' cellspacing='0'>")
                        '  htmlOut.Append("<tr><td align='center' valign='middle' class='forSaleCellBorder' colspan='2' style='padding-left:3px;'><font size='1'><font size='1'>ASKING ABBREVIATIONS</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Price</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>1,800K</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Make Offer</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>M/O</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Share</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>SHARE</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Lease</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>LS</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Sale/Lease</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>FS/LS</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Lease Only</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>LS/O</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Sealed Bid</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>BID</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Trade</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>TRD</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Sale/Trade</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>FS/TRD</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Sale/Share</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>FS/SH</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>No Engines</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>NO/ENG</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Auction</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>AUC</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Unconfirmed</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>UNC</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Confidential</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>CONF</font></td></tr>")
                        '  htmlOut.Append("<tr><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>Sale Pending</font></td><td align='left' valign='middle' class='forSaleCellBorder'><font size='1'>SP</font></td></tr>")
                        'htmlOut.Append("</table>") ' modelForsaleViewStatusTable


                        'htmlOut.Append("</td>")
                        'htmlOut.Append("</tr></table>")   ' for the first one 
                    End If
                End If

            End If


            If searchCriteria.ViewCriteriaIsReport Or export_id > 0 Then
                htmlOut.Append("</td></tr></table>") ' modelForsaleViewBottomTable
            End If

            If (View_ID < 2) Or View_ID = 11 Or View_ID = 19 Then
                If page_break_after = 0 Then
                    'htmlOut.Append("</p></div>")
                End If
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [Build_For_sale_tab] : " + ex.Message
        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing

    End Sub

    Public Shared Function Insert_Page_Break(ByVal is_word As Boolean) As String
        Insert_Page_Break = ""
        Try

            Insert_Page_Break = "</td></tr></table></td></tr></table></td></tr></table></td></tr></table>"

            If is_word Then
                Insert_Page_Break &= "<br style=""page-break-before: always"">"
            Else
                Insert_Page_Break &= "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
            End If

            'Insert_Page_Break = "div style=""page-break-after: always;""><span style=""display: none;"">&nbsp;</span></div>'

        Catch ex As Exception
            ' Response.Write("Error " & ex.Message & " in Insert_Page_Break() As String")
            'clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Insert_Page_Break() As String", aclsData_Temp)
        End Try
    End Function

    Public Shared Function convert_to_pdf(ByVal report_name As String) As Boolean

        Dim reportFolder As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString)
        Dim bReturnValue As Boolean = False
        Dim varURL As String = ""
        Dim varTimeout As Integer = 0

        Dim htmlToPdfConverter As New EvoPdf.HtmlToPdfConverter()

        Try

            varURL = reportFolder + "\" + report_name
            varTimeout = 60

            ' Set license key received after purchase to use the converter in licensed mode
            ' Leave it not set to use the converter in demo mode
            ' htmlToPdfConverter.LicenseKey = "" '"4W9+bn19bn5ue2B+bn1/YH98YHd3d3c="
            htmlToPdfConverter.LicenseKey = "9Xtoem9qemp6bXRqemlrdGtodGNjY2N6ag=="

            ' Set HTML Viewer width in pixels which is the equivalent in converter of the browser window width
            htmlToPdfConverter.HtmlViewerWidth = 1024

            ' Set HTML viewer height in pixels to convert the top part of a HTML page 
            ' Leave it not set to convert the entire HTML
            htmlToPdfConverter.HtmlViewerHeight = 0

            ' Set PDF page size which can be a predefined size like A4 or a custom size in points 
            ' Leave it not set to have a default A4 PDF page
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = EvoPdf.PdfPageSize.A4

            ' Set PDF page orientation to Portrait or Landscape
            ' Leave it not set to have a default Portrait orientation for PDF page
            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = EvoPdf.PdfPageOrientation.Portrait

            ' Set the maximum time in seconds to wait for HTML page to be loaded 
            ' Leave it not set for a default 60 seconds maximum wait time
            htmlToPdfConverter.NavigationTimeout = varTimeout

            ' Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
            ' Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 0

            htmlToPdfConverter.ConvertUrlToFile(varURL, reportFolder + "\" + commonEvo.GenerateFileName(report_name, ".pdf", True))

            bReturnValue = True

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in convert_to_pdf: " + ex.Message

        Finally

            ' Clear Objects
            htmlToPdfConverter = Nothing

        End Try

        Return bReturnValue

    End Function

    Public Shared Function write_report_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean

        Try

            Dim f As System.IO.StreamWriter

            f = System.IO.File.CreateText(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sReportname.Trim)

            ' write to the file
            f.WriteLine(sOutoutString)

            'close the streamwriter
            f.Close()
            f.Dispose()
            f = Nothing

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in write_report_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean " + ex.Message
            Return False
        End Try

        Return True

    End Function

    Public Shared Function get_valuation_details(ByVal NOTE_ID As Long, ByRef localDatalayer As viewsDataLayer, ByRef client_ac_session As Long, ByRef jetnet_ac_session As Long, ByRef LAST_SAVE_DATE As String, ByRef completed_or_open As String, ByVal is_for_spec As Boolean, ByRef temp_string_bottom_spec As String, Optional ByVal amod_id_no_ac_id As Long = 0) As String
        get_valuation_details = ""

        Dim Query As String = ""
        Dim results_table As New DataTable
        Dim results_table_inner As New DataTable
        Dim temp_string As String = ""
        Dim action_date As String = ""
        Dim temp_note As String = ""
        Dim ac_info As String = ""
        Dim comp_info As String = ""
        Dim contact_info As String = ""
        Dim Query2 As String = ""
        Dim string_open_closed As String = ""

        Query = ""
        Query = Query & " SELECT lnote_id, lnote_action_date, lnote_note, lnote_opportunity_status, "
        Query = Query & " lnote_client_ac_id, lnote_jetnet_ac_id, lnote_client_comp_id, lnote_client_contact_id, "
        Query = Query & " cliamod_make_name, cliamod_model_name, cliaircraft_year_mfr, cliaircraft_ser_nbr, "
        Query = Query & " clicomp_name, clicomp_address1, clicomp_address2, clicomp_city, clicomp_state, "
        Query = Query & " clicomp_zip_code, clicomp_country, clicontact_first_name, clicontact_last_name, "
        Query = Query & " clicontact_title, clicontact_email_address " ', clicomp_email_address, clicomp_web_address "
        Query = Query & " FROM local_notes "
        Query = Query & " inner join client_aircraft on lnote_client_ac_id=cliaircraft_id "
        Query = Query & " inner join client_aircraft_model on cliaircraft_cliamod_id = cliamod_id "
        Query = Query & " left outer join client_company on lnote_client_comp_id=clicomp_id "
        Query = Query & " left outer join client_contact on lnote_client_contact_id=clicontact_id "
        Query = Query & " WHERE lnote_id = " & NOTE_ID & " "

        Try

            results_table = localDatalayer.Get_Compare_Query(Query, "GET VALUATION DETAILS")

            If Not IsNothing(results_table) Then

                If Not is_for_spec Then
                    temp_string &= "<div style='height:200px; width:355px; overflow: auto;'>"
                    temp_string &= "<table width=""100%"" cellpadding=""3"" cellspacing=""5"" bgcolor='#E8E8E8' valign='top'>"
                Else
                    temp_string &= "<table width=""100%"" cellpadding=""3"" cellspacing=""5"" valign='top'>"
                End If



                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Trim(ac_info) = "" Then
                            client_ac_session = r("lnote_client_ac_id")
                            jetnet_ac_session = r("lnote_jetnet_ac_id")
                        End If

                        If Not IsDBNull(r("lnote_action_date")) Then
                            action_date = Trim(r("lnote_action_date"))
                            action_date = FormatDateTime(action_date, DateFormat.ShortDate)
                        End If


                        If amod_id_no_ac_id > 0 Then
                        Else
                            If Not IsDBNull(r("lnote_note")) Then
                                If Trim(r("lnote_note")) <> "" Then
                                    temp_note = "<b>Description: </b>" & Trim(r("lnote_note"))
                                End If
                            End If
                        End If


                        '------------------- AC INFO SECTION----------------------
                        'If Not IsDBNull(r("lnote_client_ac_id")) Then
                        ' action_date = Trim(r("lnote_client_ac_id"))
                        ' End If
                        If amod_id_no_ac_id > 0 Then
                            ac_info = "<font class='header_text'><b>Aircraft Model: </b></font><font class='small_header_text'>"
                            If Not IsDBNull(r("cliamod_make_name")) Then
                                ac_info &= Trim(r("cliamod_make_name")) & " "
                            End If

                            If Not IsDBNull(r("cliamod_model_name")) Then
                                ac_info &= Trim(r("cliamod_model_name")) & " "
                            End If
                            ac_info &= "</font>"
                        Else
                            ac_info = "<font class='header_text'><b>Aircraft: </b></font><font class='small_header_text'>"
                            If Not IsDBNull(r("cliaircraft_year_mfr")) Then
                                ac_info &= Trim(r("cliaircraft_year_mfr")) & " "
                            End If

                            If Not IsDBNull(r("cliamod_make_name")) Then
                                ac_info &= Trim(r("cliamod_make_name")) & " "
                            End If

                            If Not IsDBNull(r("cliamod_model_name")) Then
                                ac_info &= Trim(r("cliamod_model_name")) & " "
                            End If

                            If Not IsDBNull(r("cliaircraft_ser_nbr")) Then
                                ac_info &= ", Ser# " & Trim(r("cliaircraft_ser_nbr"))
                            End If
                            ac_info &= "</font>"
                        End If


                        '------------------- AC INFO SECTION----------------------


                        '--------------- COMP INFO-------------------------
                        If Not IsDBNull(r("clicomp_name")) Then

                            If is_for_spec = True Then
                                comp_info = "<table width='100%'><tr valign='top'><td align='center'><font class='header_text'><b>Company/Customer: </b></font><br><font class='small_header_text'>"
                            Else
                                comp_info = "<table width='100%'><tr valign='top'><td><font class='header_text'><b>Company/Customer: </b></font><br><font class='small_header_text'>"
                            End If



                            comp_info &= Trim(r("clicomp_name")) & "<Br>"


                            If Not IsDBNull(r("clicomp_address1")) Then
                                If Trim(r("clicomp_address1")) <> "" Then
                                    comp_info &= Trim(r("clicomp_address1")) & "<Br>"
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_address2")) Then
                                If Trim(r("clicomp_address2")) <> "" Then
                                    comp_info &= Trim(r("clicomp_address2")) & "<Br>"
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_city")) Then
                                comp_info &= Trim(r("clicomp_city")) & " "
                            End If

                            If Not IsDBNull(r("clicomp_state")) Then
                                If Not IsDBNull(r("clicomp_city")) Then
                                    comp_info &= ", "
                                End If
                                comp_info &= Trim(r("clicomp_state")) & " "
                                If Not IsDBNull(r("clicomp_zip_code")) Then
                                    comp_info &= Trim(r("clicomp_zip_code")) & " "
                                End If
                            End If

                            If Not IsDBNull(r("clicomp_country")) Then
                                comp_info &= Trim(r("clicomp_country")) & ""
                            End If

                            If Not IsDBNull(r("clicomp_state")) Then
                                comp_info &= "<br>"
                            ElseIf Not IsDBNull(r("clicomp_city")) Then
                                comp_info &= "<br>"
                            ElseIf Not IsDBNull(r("clicomp_country")) Then
                                comp_info &= "<br>"
                            End If



                            If Not IsDBNull(r("lnote_client_comp_id")) Then
                                If r("lnote_client_comp_id") > 0 Then
                                    Query2 = " select  clipnum_number from client_phone_numbers where clipnum_comp_id = " & r("lnote_client_comp_id") & " and clipnum_contact_id = 0 and clipnum_type = 'Office' "
                                    results_table_inner = localDatalayer.Get_Compare_Query(Query2, "GET VALUATION DETAILS - COMPANY PHONE")
                                    If Not IsNothing(results_table_inner) Then
                                        If results_table_inner.Rows.Count > 0 Then
                                            For Each x As DataRow In results_table_inner.Rows
                                                comp_info &= "<b>Office: </b>" & x.Item("clipnum_number") & "<Br>"
                                            Next
                                        End If
                                    End If
                                End If
                            End If

                            comp_info &= "</font></td></tr></table>"
                        End If
                        '--------------- COMP INFO-------------------------


                        '------------- CONTACT INFO-------------------------

                        contact_info = ""

                        If Not IsDBNull(r("clicontact_first_name")) Then
                            contact_info &= Trim(r("clicontact_first_name")) & " "
                        End If

                        If Not IsDBNull(r("clicontact_last_name")) Then
                            contact_info &= Trim(r("clicontact_last_name")) & "<br>"
                        End If

                        If Not IsDBNull(r("clicontact_title")) Then
                            contact_info &= Trim(r("clicontact_title")) & "<br>"
                        End If

                        If Not IsDBNull(r("clicontact_email_address")) Then
                            contact_info &= Trim(r("clicontact_email_address")) & "<br>"
                        End If

                        If Not IsDBNull(r("lnote_client_contact_id")) Then
                            If r("lnote_client_contact_id") > 0 Then
                                Query2 = " select  clipnum_number from client_phone_numbers where  clipnum_contact_id = " & r("lnote_client_contact_id") & " and clipnum_comp_id = 0 and clipnum_type = 'Office' "
                                results_table_inner = localDatalayer.Get_Compare_Query(Query2, "GET VALUATION DETAILS - CONTACT PHONE")
                                If Not IsNothing(results_table_inner) Then
                                    If results_table_inner.Rows.Count > 0 Then
                                        For Each x As DataRow In results_table_inner.Rows
                                            comp_info &= "<b>Office: </b>" & x.Item("clipnum_number") & "<Br>"
                                        Next
                                    End If
                                End If
                            End If
                        End If

                        If Trim(contact_info) <> "" Then
                            If is_for_spec = True Then
                                contact_info = "<table width='100%'><tr valign='top'><td align='center'><font class='header_text'><b>Contact: </b></font><font class='small_header_text'><br>" & contact_info
                            Else
                                contact_info = "<table width='100%'><tr valign='top'><td><font class='header_text'><b>Contact: </b></font><font class='small_header_text'><br>" & contact_info
                            End If

                            contact_info &= "</font></td></tr></table>"
                        End If

                        '------------- CONTACT INFO-------------------------





                        '" , , , , "
                        ' , , , , , "
                        '  , , , , "
                        '   "


                        If Not IsDBNull(r("lnote_opportunity_status")) Then
                            string_open_closed = Trim(r("lnote_opportunity_status"))
                            completed_or_open = string_open_closed
                        End If


                        If Not IsDBNull(r("lnote_action_date")) Then
                            If is_for_spec = False Then

                                action_date = "<b>Last Update: </b>" & Trim(r("lnote_action_date"))
                            Else
                                If completed_or_open = "O" Then
                                    action_date = FormatDateTime(Now, DateFormat.ShortDate)
                                Else
                                    action_date = Trim(r("lnote_action_date"))
                                End If
                            End If

                            If Trim(completed_or_open) = "C" Then
                                LAST_SAVE_DATE = Trim(r("lnote_action_date"))
                            End If
                        End If

                    Next
                End If










                If is_for_spec = True Then
                    temp_string &= "<tr valign='top'><td align='center' class='header_text'><b><font size='+1'>MARKET VALUE REPORT</font></b></td></tr>"
                    temp_string &= "<tr valign='top'><td align='center'>" & Replace(ac_info, "</b>", "</b><br>") & "</td></tr>"
                    temp_string &= "<tr valign='top'><td align='center'><font class='header_text'><b>Date:</b></font><br><font class='small_header_text'>" & action_date & "</font></td></tr>"

                    If Trim(string_open_closed) = "O" Then
                        string_open_closed = "<b>Status:</b> Open"
                    ElseIf Trim(string_open_closed) = "C" Then
                        string_open_closed = "<b>Status:</b> Closed"
                    End If


                    '   temp_string &= "<tr valign='top'><td align='center'>" & string_open_closed & "</td></tr>"
                    temp_string &= "</table>"


                    temp_string_bottom_spec = ""
                    temp_string_bottom_spec = "<table width='100%' align='center'>"
                    temp_string_bottom_spec &= "<tr valign='top'>"
                    temp_string_bottom_spec &= "<td align='center' colspan='2'>" & comp_info & "</td>"
                    temp_string_bottom_spec &= "</tr>"

                    If Trim(contact_info) <> "" Then
                        temp_string_bottom_spec &= "<tr valign='top'>"
                        temp_string_bottom_spec &= "<td align='center' colspan='2'><font class='small_header_text'>" & contact_info & "</font></td>"
                        temp_string_bottom_spec &= "</tr>"
                    End If


                    ' temp_string_bottom_spec &= "<tr valign='top'>"
                    '  temp_string_bottom_spec &= "<td align='center' colspan='2' class='small_header_text'>" & temp_note & "</td>"
                    '  temp_string_bottom_spec &= "</tr>"

                    temp_string_bottom_spec &= "</table>"


                Else


                    temp_string &= "<tr valign='top'>"
                    ' temp_string &= "<td align='left'>" & ac_info & "</td>"
                    temp_string &= "<td align='left'>" & action_date & "</td>"



                    If Trim(string_open_closed) = "O" Then
                        string_open_closed = "<b>Status:</b> Open"
                        '   Me.update_compare2.Visible = True
                    ElseIf Trim(string_open_closed) = "C" Then
                        string_open_closed = "<b>Status:</b> Closed"
                        '  Me.update_compare2.Visible = False
                    End If



                    temp_string &= "<td align='left'>" & string_open_closed & "</td>"
                    temp_string &= "</tr>"


                    temp_string &= "<tr valign='top'>"
                    temp_string &= "<td align='left' colspan='2'>" & comp_info & "</td>"
                    temp_string &= "</tr>"

                    If Trim(contact_info) <> "" Then
                        temp_string &= "<tr valign='top'>"
                        temp_string &= "<td align='left' colspan='2'>" & contact_info & "</td>"
                        temp_string &= "</tr>"
                    End If


                    temp_string &= "<tr valign='top'>"
                    temp_string &= "<td align='left' colspan='2'>" & temp_note & "</td>"
                    temp_string &= "</tr>"




                    temp_string &= "</table>"
                    temp_string &= "</div>"

                    temp_string &= "<br><table cellspacing='0' cellpadding='0'>"
                    temp_string &= "<tr valign='top'>"
                    temp_string &= "<td align='left' colspan='2'>"

                    temp_string &= "<a href='#' onclick=""window.open('/edit_note.aspx?action=edit&type=valuation&id=" & NOTE_ID & "&refreshing=view&nWin=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">"

                    '<a href='edit_note.aspx?action=edit&type=valuation&id=" & NOTE_ID & "' target='_blank'>
                    temp_string &= "Edit"

                    temp_string &= "</a>"

                    temp_string &= "</td></tr></table>"



                End If


            End If

            get_valuation_details = temp_string

        Catch ex As Exception
        Finally
        End Try


    End Function


    Public Shared Function CRMget_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal internal_flag As String, ByVal retail_flag As String, ByVal last_date As String, Optional ByVal jetnet_string As String = "", Optional ByVal months_to_Show As Integer = 0, Optional ByVal years_of As String = "", Optional ByVal aftt_within As String = "", Optional ByVal use_only_used As String = "", Optional ByVal extra_criteria As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim YearDateVariable As String = ""
        Dim start_date As String = ""
        Dim AclsData_Temp As New clsData_Manager_SQL

        Try

            'Query = "SELECT TOP 20 journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, amod_make_name"
            'sQuery.Append(" FROM Journal_Summary WITH(NOLOCK)"
            'sQuery.Append(" WHERE ac_amod_id = " & inModelID
            'sQuery.Append(" AND (journ_date BETWEEN (getdate()-90) AND (getdate()+1))"
            'sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') "
            'sQuery.Append(" AND (journ_subcat_code_part3 NOT IN ('DB','DS','FI','MF','FY','RE','IT','RR')) "
            'sQuery.Append(" AND (journ_subcategory_code NOT LIKE '%IT%')"
            'sQuery.Append(" AND (journ_internal_trans_flag = 'N')"
            'sQuery.Append(MakeAircraftProductCodeClause(session("Product_Code"), False, False)
            'sQuery.Append(" ORDER BY journ_date DESC"
            If Trim(jetnet_string) <> "" Then

                sQuery.Append("SELECT distinct ")
                sQuery.Append(" ac_id,  emp_program_name, ac_ser_no_full, journ_id, 0 as client_jetnet_trans_id, journ_customer_note, ac_ser_no_sort ")

                If InStr(jetnet_string, "ac_list_date") = 0 Then
                    sQuery.Append(" ,ac_list_date ")
                End If

                If InStr(jetnet_string, "ac_airframe_tot_hrs") = 0 Then
                    sQuery.Append(" ,ac_airframe_tot_hrs ")
                End If

                If InStr(jetnet_string, "ac_asking_price") = 0 Then
                    sQuery.Append(" ,ac_asking_price ")
                End If

                If Trim(jetnet_string) <> "" Then
                    sQuery.Append(", ")
                    sQuery.Append(jetnet_string)
                End If
                sQuery.Append(", ac_sale_price_display_flag, case  when ac_asking IS NULL  then '' else ac_asking end as ac_asking, journ_id")
                sQuery.Append(", case when ac_engine_1_soh_hrs is null then 0 else ac_engine_1_soh_hrs end as ac_engine_1_soh_hrs, case when ac_engine_2_soh_hrs is null then 0 else ac_engine_2_soh_hrs end as ac_engine_2_soh_hrs ")

                sQuery.Append(" FROM aircraft WITH (NOLOCK) ")

                sQuery.Append(" inner JOIN aircraft_reference WITH (NOLOCK) ON aircraft_reference.cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                sQuery.Append(" inner JOIN company with (NOLOCK) on  aircraft_reference.cref_comp_id = comp_id and cref_journ_id = comp_journ_id ")
                sQuery.Append(" inner JOIN aircraft_model WITH (NOLOCK) ON aircraft.ac_amod_id = aircraft_model.amod_id ")
                sQuery.Append(" inner JOIN aircraft_contact_type WITH (NOLOCK) ON aircraft_reference.cref_contact_type = aircraft_contact_type.actype_code ")
                sQuery.Append(" INNER Join Engine_Maintenance_Program WITH(NOLOCK) ON aircraft.ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id ")

                sQuery.Append(" left outer join contact with (NOLOCK) on cref_contact_id = contact_id and cref_journ_id = contact_journ_id and contact_active_flag='Y' and contact_hide_flag='N' ")
                sQuery.Append(" left outer join Journal on journ_id = ac_journ_id  ")
                sQuery.Append(" left outer join Journal_Category on jcat_subcategory_code  = journ_subcategory_code ")
            Else
                sQuery.Append("SELECT  journ_id, emp_program_name, journ_subcategory_code, journ_date,ac_year,  journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, ac_asking_price, ac_ser_no_sort, amod_make_name")
                sQuery.Append(", ac_list_date, ac_airframe_tot_hrs, journ_customer_note, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear,ac_passenger_count ")

                sQuery.Append(", case when ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' ")
                sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
                sQuery.Append(" then ac_sale_price else '' end as ac_sold_price, ac_airframe_tot_hrs, '' as clitrans_value_description ")
                sQuery.Append(", (SELECT TOP 1 comp_name FROM Aircraft_Company_Flat WITH(NOLOCK) WHERE cref_ac_id =ac_id AND cref_journ_id = ac_journ_id AND ((cref_contact_type = '99') OR (cref_contact_type = '93')) ) as BROKER ")


                sQuery.Append(" , ac_sale_price_display_flag, case  when ac_asking IS NULL  then '' else ac_asking end as ac_asking, ac_status ")
                sQuery.Append(", case when ac_engine_1_soh_hrs is null then 0 else ac_engine_1_soh_hrs end as ac_engine_1_soh_hrs, case when ac_engine_2_soh_hrs is null then 0 else ac_engine_2_soh_hrs end as ac_engine_2_soh_hrs ")

                sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
                sQuery.Append(" INNER Join Engine_Maintenance_Program WITH(NOLOCK) ON aircraft.ac_engine_maintenance_prog_EMP = Engine_Maintenance_Program.emp_id ")
                sQuery.Append(" INNER JOIN journal_category WITH(NOLOCK) ON journ_subcategory_code = jcat_subcategory_code")

            End If



            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                Dim tmpStr As String = ""
                For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
                    If String.IsNullOrEmpty(tmpStr) Then
                        tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
                    Else
                        tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
                    End If
                Next

                sQuery.Append(" WHERE amod_id IN (" + tmpStr.Trim + ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If Trim(use_only_used) <> "" Then
                sQuery.Append(use_only_used)
            End If

            If Trim(years_of) <> "" And Trim(years_of) <> "0" Then
                sQuery.Append(years_of)
            End If

            If Trim(aftt_within) <> "" And Trim(aftt_within) <> "0" Then
                sQuery.Append(aftt_within)
            End If

            If Trim(extra_criteria) <> "" Then
                sQuery.Append(extra_criteria)
            End If

            'subcat code part3 and date modified/removed per Rick on 4/30/2014
            '  sQuery.Append(" AND ((jcat_category_code = 'AH') and (journ_subcat_code_part1='WS') )") 'AND (journ_subcat_code_part3 NOT IN ('DB','DS','FI','MF','FY','RE','IT','RR'))
            '  sQuery.Append(" and NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')) ")

            'added MSW - 5/17/2016 - 
            ' Dim AclsData_Temp As New clsData_Manager_SQL
            'sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())

            sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') ")      '-- Whole Sales Only 


            ' only do if there is no extra ( only do if not from pdf) 
            If Trim(extra_criteria) = "" Or InStr(Trim(extra_criteria), "and ac_asking_price is not NULL") > 0 Then
                If CDbl(months_to_Show) > 0 Then
                    start_date = Date.Now()


                    'If Month(start_date) = 12 Or Month(start_date) = 11 Or Month(start_date) = 10 Then '10/16/2016 - 1/1/2017 - 1/1/2014
                    '  start_date = "1/1/" & (Year(start_date) + 1)
                    'ElseIf Month(start_date) = 7 Or Month(start_date) = 8 Or Month(start_date) = 9 Then '8/16/2016 - 10/1/2016 - 10/1/2013
                    '  start_date = "10/1/" & Year(start_date)
                    'ElseIf Month(start_date) = 4 Or Month(start_date) = 5 Or Month(start_date) = 6 Then '5/16/2016 - 7/1/2016 - 7/1/2013
                    '  start_date = "7/1/" & Year(start_date)
                    'Else '1,2,3     '3/16/2016 - 3/1/2016 - 4/1/2013
                    '  start_date = "4/1/" & Year(start_date)
                    'End If

                    'YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date))) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date))) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(months_to_Show), CDate(start_date)))

                    YearDateVariable = Year(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now())) & "-" & Month(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now())) & "-" & Day(DateAdd(DateInterval.Month, -CDbl(months_to_Show), Now()))
                ElseIf Trim(last_date) = "" Then
                    YearDateVariable = Year(DateAdd(DateInterval.Year, -1, Now())) & "-" & Month(DateAdd(DateInterval.Year, -1, Now())) & "-" & Day(DateAdd(DateInterval.Year, -1, Now()))
                Else
                    YearDateVariable = Year(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, -1, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, -1, CDate(last_date)))
                End If

                sQuery.Append(" AND journ_date >= '" & YearDateVariable & "' ")

                If Trim(last_date) <> "" Then
                    YearDateVariable = Year(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Month(DateAdd(DateInterval.Year, 0, CDate(last_date))) & "-" & Day(DateAdd(DateInterval.Year, 0, CDate(last_date)))
                    sQuery.Append(" AND journ_date <= '" & YearDateVariable & "' ")
                End If
            End If




            If Trim(internal_flag) = "N" Then
                sQuery.Append(" AND  journ_internal_trans_flag = 'N' ")
            End If

            If Trim(retail_flag) = "Y" Then
                sQuery.Append(" AND NOT (journ_subcat_code_part3 IN ('DB', 'DS', 'FI', 'FY', 'IT', 'MF', 'RE', 'CC', 'LS', 'RM')) ")
                ' sQuery.Append(" and jcat_used_retail_sales_flag = 'Y' ")
            End If


            If Trim(last_date) <> "" Then

            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))





            sQuery.Append(" ORDER BY journ_date DESC")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "crmViewDataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase")
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
                aError = "Error in get_retail_sales_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

End Class

