
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/FolderMaintenance.aspx.vb $
'$$Author: Matt $
'$$Date: 7/06/20 1:51p $
'$$Modtime: 7/06/20 1:46p $
'$$Revision: 6 $
'$$Workfile: FolderMaintenance.aspx.vb $
'
' ********************************************************************************

Partial Public Class FolderMaintenance
    Inherits System.Web.UI.Page
    Dim QueryRebuild As String = ""
    Dim nFolderType As Integer = 3
    Dim URLRedirect As String = ""
    Public bRefreshPreferences As Boolean = False
    Public bRefreshHome As Boolean = False
    Dim bFromPreferences As Boolean = False
    Dim bFromHome As Boolean = False
    Dim bDeleteFolder As Boolean = False
    Dim bNewStaticFolder As Boolean = False
    Dim bDefaultFolder As Boolean = False

    Dim nReportID As Long = 0
    Dim sTypeOfFolder As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request.Item("fromPreferences")) Then
            If Not String.IsNullOrEmpty(Request.Item("fromPreferences").ToString.Trim) Then
                bFromPreferences = CBool(Request.Item("fromPreferences").ToString.ToLower.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("fromHome")) Then
            If Not String.IsNullOrEmpty(Request.Item("fromHome").ToString.Trim) Then
                bFromHome = CBool(Request.Item("fromHome").ToString.ToLower.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("newStaticFolder")) Then
            If Not String.IsNullOrEmpty(Request.Item("newStaticFolder").ToString.Trim) Then
                bNewStaticFolder = CBool(Request.Item("newStaticFolder").ToString.ToLower.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("default")) Then
            If Not String.IsNullOrEmpty(Request.Item("default").ToString.Trim) Then
                bDefaultFolder = CBool(Request.Item("default").ToString.ToLower.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("t")) Then
            If Not String.IsNullOrEmpty(Request.Item("t").ToString.Trim) Then
                If IsNumeric(Request.Item("t").ToString) Then
                    nFolderType = CInt(Request.Item("t").ToString)
                End If
            End If
        End If




        If nFolderType = "17" Then
            cfolder_default.Enabled = True
        End If

        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            Dim FolderDefaultEdit As Boolean = False

            If Not IsNothing(Trim(Request("DEFAULT_FOLDER_EDIT"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("DEFAULT_FOLDER_EDIT"))) Then
                    If Trim(Request("DEFAULT_FOLDER_EDIT")) = "true" Then
                        FolderDefaultEdit = True
                    End If
                End If
            End If

            If bFromPreferences Then

                If Not IsNothing(Request.Item("deleteFolder")) Then
                    If Not String.IsNullOrEmpty(Request.Item("deleteFolder").ToString.Trim) Then
                        bDeleteFolder = CBool(Request.Item("deleteFolder").ToString.ToLower.Trim)
                    End If
                End If

                If Not IsNothing(Request.Item("REPORT_ID")) Then
                    If Not String.IsNullOrEmpty(Request.Item("REPORT_ID").ToString.Trim) Then
                        If IsNumeric(Request.Item("REPORT_ID").ToString.ToLower.Trim) Then
                            nReportID = CLng(Request.Item("REPORT_ID").ToString.ToLower.Trim)
                        End If
                    End If
                End If

                If Not IsNothing(Request.Item("TYPE_OF_FOLDER")) Then
                    If Not String.IsNullOrEmpty(Request.Item("TYPE_OF_FOLDER").ToString.Trim) Then
                        sTypeOfFolder = Request.Item("TYPE_OF_FOLDER").ToString
                    End If
                End If

                add_folder_text.Text = " The purpose of this form is to edit this Folder for the [" + sTypeOfFolder.Trim + "] search tab. " +
                               "Click 'Save Folder' to save any changes you have made. "

                If bDeleteFolder Then

                    Dim is_admin As String = "N"

                    Call commonLogFunctions.Log_User_Event_Data("UserFolderDelete", "Folder DELETED: (" + nReportID.ToString + ")", Nothing, 0, 0, 0, 0, 0, 0, 0)

                    Master.aclsData_Temp.Remove_Evolution_Folder_Index(0, nReportID, 0, 0, 0, 0, 0, 0)

                    If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                        is_admin = "Y"
                    Else
                        is_admin = "N"
                    End If

                    If Master.aclsData_Temp.Remove_Evolution_Folders(nReportID, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, is_admin) = 1 Then
                        attention.Text = "<p align='center'><b>Your folder has been removed.</b></p>"
                        If bFromPreferences Then
                            bRefreshPreferences = True
                        End If

                        If bFromHome Then
                            bRefreshHome = True
                        End If

                    Else
                        attention.Text = "<p align='center'><b>We're sorry, there was a problem removing your data.</b></p>"
                    End If

                End If

            Else

                If Not IsNothing(Request.Form("REPORT_ID")) Then
                    If Not String.IsNullOrEmpty(Request.Form("REPORT_ID").ToString.Trim) Then
                        If IsNumeric(Request.Form("REPORT_ID").ToString.ToLower.Trim) Then
                            nReportID = CLng(Request.Form("REPORT_ID").ToString.ToLower.Trim)
                        End If
                    End If
                End If

                If Not IsNothing(Request.Form("TYPE_OF_FOLDER")) Then
                    If Not String.IsNullOrEmpty(Request.Form("TYPE_OF_FOLDER").ToString.Trim) Then
                        sTypeOfFolder = Request.Form("TYPE_OF_FOLDER").ToString
                    End If
                End If

            End If

            If Not FolderDefaultEdit Then

                Dim folderID As Long = 0
                Dim FolderTableData As New DataTable
                Dim ActionString As String = "Create"
                Dim temp_type As String = ""


                If Not IsPostBack Then

                    temp_type = Trim(Request("type"))

                    If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                        Me.export_types.Items.Clear()
                        Me.export_types.Items.Add(New ListItem("My Personal & Shared Folders", "MY"))
                        Me.export_types.Items.Add(New ListItem("All Folders", "ALL"))
                        Me.export_types.Items.Add(New ListItem("All Shared Folders", "ALS"))
                        Me.export_types.Items.Add(New ListItem("All Personal Folders", "ALP"))

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
                                Me.bottom_label_text.Text = "<br /><br />As an administrator you can view, edit, and delete folders for all users for your Company. Use the drop down above the folder list to select your desired folder list"
                            Case eSubscriptionShareType.MY_PARENT_SUBSCRIPTION
                                Me.bottom_label_text.Text = "<br /><br />As an administrator you can view, edit, and delete folders for all users for your Subscription. Use the drop down above the folder list to select your desired folder list"
                            Case Else
                                Me.bottom_label_text.Text = "<br /><br />As an administrator you can view, edit, and delete folders for all users for your Company. Use the drop down above the folder list to select your desired folder list"
                        End Select

                    Else

                        Me.export_types.Items.Clear()
                        Me.export_types.Items.Add(New ListItem("My Personal & Shared Folders", "MY"))
                        Me.export_types.Items.Add(New ListItem("My Personal Folders", "MYP"))
                        Me.export_types.Items.Add(New ListItem("Shared Folders", "ALLS"))

                        If Trim(temp_type) = "MY" Then
                            Me.export_types.SelectedIndex = 0
                        ElseIf Trim(temp_type) = "MYP" Then
                            Me.export_types.SelectedIndex = 1
                        ElseIf Trim(temp_type) = "ALLS" Then
                            Me.export_types.SelectedIndex = 2
                        End If

                        Me.bottom_label_text.Text = "<br /><br />You are only able to edit/delete folders that you have created.  If you wish to edit any folder from another user you will either need to contact the other user or your account administrator. "

                    End If
                End If

                If Not IsPostBack Then

                    If bNewStaticFolder Then

                        Add_Folder_Mode_Click(sender, e)

                    End If

                    If Not String.IsNullOrEmpty(sTypeOfFolder.Trim) Then
                        Dim TheRealQueryString As String = Session.Item("Master" + IIf(sTypeOfFolder.ToUpper <> "HISTORY", sTypeOfFolder.Trim, "Aircraft"))

                        Dim requesttype As String = sTypeOfFolder.Trim
                        TheRealQueryString = Replace(TheRealQueryString, "'", "''")
                        Add_Folder_Mode.Visible = False

                        'Response.Write(TheRealQueryString)

                        If sTypeOfFolder = "COMPANY" Then
                            operatorAnalysisRow.CssClass = ""
                            cfolder_operator_flag.Enabled = True
                            If Trim(ActionString) = "Create" Then
                                cfolder_operator_flag.Checked = True
                            End If
                        End If


                        If nReportID > 0 Then
                            ActionString = "Edit"
                            folderID = nReportID

                            FolderTableData = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, IIf(bFromPreferences = True, "", "A"))

                            If Not IsNothing(FolderTableData) Then
                                If FolderTableData.Rows.Count > 0 Then

                                    If sTypeOfFolder = "COMPANY" Then
                                        cfolder_operator_flag.Checked = IIf(FolderTableData.Rows(0).Item("cfolder_operator_flag").ToString = "Y", True, False)
                                    End If

                                    cfolder_name.Text = FolderTableData.Rows(0).Item("cfolder_name").ToString
                                    cfolder_description.Text = FolderTableData.Rows(0).Item("cfolder_description").ToString
                                    cfolder_share.Checked = IIf(FolderTableData.Rows(0).Item("cfolder_share").ToString = "Y", True, False)
                                    cfolder_hide.Checked = IIf(FolderTableData.Rows(0).Item("cfolder_hide_flag").ToString = "Y", True, False)

                                    cfolder_method.Text = IIf(bFromPreferences = True, FolderTableData.Rows(0).Item("cfolder_method").ToString, "A")

                                    cfolder_id.Text = folderID

                                    cfolder_jetnet_run_reply_username.Text = FolderTableData.Rows(0).Item("cfolder_jetnet_run_reply_username").ToString
                                    cfolder_jetnet_run_flag.Checked = IIf(FolderTableData.Rows(0).Item("cfolder_jetnet_run_flag").ToString = "Y", True, False)
                                    cfolder_jetnet_run_reply_email.Text = FolderTableData.Rows(0).Item("cfolder_jetnet_run_reply_email").ToString

                                    cfolder_default.Checked = IIf(FolderTableData.Rows(0).Item("cfolder_default_flag").ToString = "Y", True, False)

                                    folder_submit_button.Text = "Save Folder"

                                    If bFromPreferences And Not bDeleteFolder Then

                                        folder_delete_button.Visible = True
                                        folder_delete_button.PostBackUrl = "~/FolderMaintenance.aspx?fromPreferences=true&deleteFolder=true&type=MY&REPORT_ID=" + folderID.ToString + "&TYPE_OF_FOLDER=" + sTypeOfFolder.Trim + "&t=" + nFolderType.ToString + """"

                                    End If

                                End If
                            Else
                                Master.LogError(Master.aclsData_Temp.class_error)
                            End If
                        End If


                        Select Case sTypeOfFolder.Trim.ToUpper
                            Case "HISTORY"
                                Master.SetPageTitle(ActionString & " History Folder")
                                cfolder_type_of_folder.Text = "8"
                                nFolderType = 8
                                foldertypeStringLabel.Text = "History"
                            Case "EVENTS"
                                Dim EventTiming As Integer = 0 'A counter, just to basically tell how many parameters are set for time (hour/day/etc). Every one that's not zero, this gets incremented. Just to make the display a little nicer, so I can tell if we need ands, commas, etc
                                Master.SetPageTitle(ActionString & " Event Folder")
                                cfolder_type_of_folder.Text = "5"
                                nFolderType = 5

                                If UCase(Session.Item("localSubscription").crmFrequency) = "LIVE" Then
                                    EventTable.Visible = True
                                    eventsBox.Visible = True
                                    foldertypeStringLabel.Text = "Events"
                                    cfolder_jetnet_run_flag.Text = "Check to have JETNET process and email results every: {"

                                    Dim TotalMinutes As Long = 0
                                    Dim TotalDays As Integer = 0
                                    Dim TotalHours As Integer = 0

                                    If Session.Item("searchCriteria").SearchCriteriaEventMonths <> 0 Then
                                        cfolder_jetnet_run_flag.Text += Session.Item("searchCriteria").SearchCriteriaEventMonths.ToString & " Month"
                                        If Session.Item("searchCriteria").SearchCriteriaEventMonths > 1 Then
                                            cfolder_jetnet_run_flag.Text += "s"
                                        End If
                                        EventTiming += 1
                                        TotalMinutes += Session.Item("searchCriteria").SearchCriteriaEventMonths * 43829
                                    End If


                                    If Session.Item("searchCriteria").SearchCriteriaEventDays <> 0 Then
                                        If EventTiming > 0 Then
                                            cfolder_jetnet_run_flag.Text += ", "
                                        End If
                                        cfolder_jetnet_run_flag.Text += Session.Item("searchCriteria").SearchCriteriaEventDays.ToString & " Day"
                                        If Session.Item("searchCriteria").SearchCriteriaEventDays > 1 Then
                                            cfolder_jetnet_run_flag.Text += "s"
                                        End If
                                        EventTiming += 1
                                        TotalMinutes += Session.Item("searchCriteria").SearchCriteriaEventDays * 1440
                                    End If

                                    If Session.Item("searchCriteria").SearchCriteriaEventHours <> 0 Then
                                        If EventTiming > 0 Then
                                            cfolder_jetnet_run_flag.Text += ", "
                                        End If
                                        cfolder_jetnet_run_flag.Text += Session.Item("searchCriteria").SearchCriteriaEventHours.ToString & " Hour"
                                        If Session.Item("searchCriteria").SearchCriteriaEventHours > 1 Then
                                            cfolder_jetnet_run_flag.Text += "s"
                                        End If
                                        EventTiming += 1
                                        TotalMinutes += Session.Item("searchCriteria").SearchCriteriaEventHours * 60
                                    End If

                                    If Session.Item("searchCriteria").SearchCriteriaEventMinutes <> 0 Then
                                        If EventTiming >= 2 And EventTiming > 0 Then
                                            cfolder_jetnet_run_flag.Text += " and "
                                        ElseIf EventTiming > 0 Then
                                            cfolder_jetnet_run_flag.Text += " , "
                                        End If
                                        cfolder_jetnet_run_flag.Text += Session.Item("searchCriteria").SearchCriteriaEventMinutes.ToString & " Minute"

                                        If Session.Item("searchCriteria").SearchCriteriaEventMinutes > 1 Then
                                            cfolder_jetnet_run_flag.Text += "s"
                                        End If
                                        EventTiming += 1
                                        TotalMinutes += Session.Item("searchCriteria").SearchCriteriaEventMinutes
                                    End If
                                    'If EventTiming = 1 Then
                                    '    cfolder_jetnet_run_flag.Text = Replace(cfolder_jetnet_run_flag.Text, "1 ", " ")
                                    'End If
                                    cfolder_jetnet_run_flag.Text += "}"

                                    'This is really just a catch all. I wanted to put some error catching in to make it to where 
                                    'if they had no default time, they couldn't check the box.
                                    If EventTiming = 0 Then
                                        cfolder_jetnet_run_flag.Checked = False
                                        cfolder_jetnet_run_flag.Enabled = False
                                    End If

                                    emptyBox.Text = TotalMinutes


                                    'TotalMinutes
                                    If folderID = 0 Then
                                        'Filling up the user's name, first name first.
                                        If Not IsNothing(Session.Item("localUser").crmLocalUserFirstName) Then
                                            If Not String.IsNullOrEmpty(Session.Item("localUser").crmLocalUserFirstName) Then
                                                cfolder_jetnet_run_reply_username.Text = Session.Item("localUser").crmLocalUserFirstName.ToString
                                            End If
                                        End If

                                        'Filling up the user's name, last name next.
                                        If Not IsNothing(Session.Item("localUser").crmLocalUserLastName) Then
                                            If Not String.IsNullOrEmpty(Session.Item("localUser").crmLocalUserLastName) Then
                                                cfolder_jetnet_run_reply_username.Text += " " & Session.Item("localUser").crmLocalUserLastName.ToString
                                            End If
                                        End If

                                        'trimming the result
                                        cfolder_jetnet_run_reply_username.Text = Trim(cfolder_jetnet_run_reply_username.Text)


                                        'Filling up the email address:
                                        If Not IsNothing(Session.Item("localUser").crmLocalUserName) Then
                                            If Not String.IsNullOrEmpty(Session.Item("localUser").crmLocalUserName) Then
                                                cfolder_jetnet_run_reply_email.Text = Session.Item("localUser").crmLocalUserName.ToString
                                            End If
                                        End If
                                    End If
                                End If
                            Case "COMPANY"
                                foldertypeStringLabel.Text = "Company"
                                Master.SetPageTitle(ActionString & " Company Folder")
                                cfolder_type_of_folder.Text = "1"
                                nFolderType = 1

                                If bFromHome = False Then
                                    folder_submit_button.Text = "Save and Return to Company List"
                                    folder_submit_button_flight.Visible = True
                                    If cfolder_operator_flag.Checked = False Then
                                        folder_submit_button_flight.Attributes.Add("class", "display_none")
                                    End If

                                End If
                            Case "WANTED"
                                foldertypeStringLabel.Text = "Wanted"
                                Master.SetPageTitle(ActionString & " Wanted Folder")
                                cfolder_type_of_folder.Text = "9"
                                nFolderType = 9
                            Case "YACHT"
                                foldertypeStringLabel.Text = "Yachts"
                                Master.SetPageTitle(ActionString & " Yacht Folder")
                                cfolder_type_of_folder.Text = "10"
                                nFolderType = 10
                            Case "YACHT HISTORY"
                                foldertypeStringLabel.Text = "Yacht History"
                                Master.SetPageTitle(ActionString & " Yacht History Folder")
                                cfolder_type_of_folder.Text = "14"
                                nFolderType = 14
                            Case "YACHT EVENTS"
                                foldertypeStringLabel.Text = "Yacht Events"
                                Master.SetPageTitle(ActionString & " Yacht Events Folder")
                                cfolder_type_of_folder.Text = "15"
                                nFolderType = 15
                            Case "PERFORMANCE SPECS"
                                foldertypeStringLabel.Text = "Performance Specs"
                                Master.SetPageTitle(ActionString & " Performance Specs Folder")
                                cfolder_type_of_folder.Text = "12"
                                nFolderType = 12
                            Case "OPERATING COSTS"
                                foldertypeStringLabel.Text = "Operating Costs"
                                Master.SetPageTitle(ActionString & " Operating Costs Folder")
                                cfolder_type_of_folder.Text = "11"
                                nFolderType = 11
                            Case "MARKET SUMMARIES"
                                foldertypeStringLabel.Text = "Market Summaries"
                                Master.SetPageTitle(ActionString & " Market Summary Folder")
                                cfolder_type_of_folder.Text = "13"
                                nFolderType = 13
                            Case "VALUE"
                                foldertypeStringLabel.Text = "Value View"
                                Master.SetPageTitle(ActionString & " Value View Folder")
                                cfolder_type_of_folder.Text = "16"
                                nFolderType = 16
                            Case "AIRPORT"
                                foldertypeStringLabel.Text = "Airport View"
                                Master.SetPageTitle(ActionString & " Airport View Folder")
                                cfolder_type_of_folder.Text = "17"
                                nFolderType = 17
                            Case Else
                                foldertypeStringLabel.Text = "Aircraft"
                                Master.SetPageTitle(ActionString & " Aircraft Folder")
                                cfolder_type_of_folder.Text = "3"
                                nFolderType = 3

                                If bFromHome = False Then
                                    folder_submit_button.Text = "Save and Return to Aircraft List"
                                    folder_submit_button_flight.Visible = True
                                End If
                        End Select

                        'folder_query_list.Text = "<br /><p>" & HttpContext.Current.Session.Item("SearchString") & "</p><hr /><em class='tiny_text'>(Debug display at the moment:<br />"
                        For Each name As String In Request.Form.AllKeys
                            Dim value As String = Request.Form(name)
                            If name <> "TYPE_OF_FOLDER" And name <> "REPORT_ID" Then
                                If name = "cboAircraftTypeID" Or name = "cboAircraftMakeID" Or name = "cboAircraftModelID" Or name = "cboYachtCategoryID" Or name = "cboYachtBrandID" Or name = "cboYachtModelID" Then

                                    Dim TypeValues As Array
                                    Dim ValueList As String = ""
                                    value = value.ToString.Trim
                                    TypeValues = value.Split("##")
                                    Dim sAirframeType As String = ""
                                    Dim sAirType As String = ""
                                    Dim sMake As String = ""
                                    Dim sModel As String = ""
                                    Dim sUsage As String = ""
                                    Dim sMotor As String = ""
                                    Dim sCategory As String = ""
                                    Dim sBrand As String = ""
                                    For j = 0 To UBound(TypeValues)
                                        If TypeValues(j) <> "" Then
                                            If IsNumeric(TypeValues(j)) Then
                                                Dim CurrentModelCount As Long = CLng(TypeValues(j))

                                                If ValueList <> "" Then
                                                    ValueList += "##"
                                                End If

                                                If name = "cboAircraftTypeID" Then
                                                    commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)
                                                    ValueList += "" & sAirType & "|" & sAirframeType
                                                ElseIf name = "cboAircraftMakeID" Then
                                                    commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)
                                                    ValueList += "" & sMake & "|" & commonEvo.ReturnAmodIDForItemIndex(CurrentModelCount).ToString
                                                ElseIf name = "cboAircraftModelID" Then
                                                    ValueList += commonEvo.ReturnAmodIDForItemIndex(CurrentModelCount).ToString
                                                ElseIf name = "cboYachtCategoryID" Then 'The category needs to be saved with the motor type, otherwise you'll get the wrong category selected in the selectbox
                                                    commonEvo.ReturnYachtModelDataFromIndex(CurrentModelCount, sMotor, sCategory, sBrand, sModel)
                                                    ValueList += "" & sCategory & "|" & sMotor
                                                ElseIf name = "cboYachtBrandID" Then
                                                    commonEvo.ReturnYachtModelDataFromIndex(CurrentModelCount, sMotor, sCategory, sBrand, sModel)
                                                    ValueList += "" & sBrand & "|" & sCategory
                                                ElseIf name = "cboYachtModelID" Then
                                                    ValueList += commonEvo.ReturnYachtModelIDForItemIndex(CurrentModelCount).ToString
                                                End If
                                            End If
                                        End If
                                    Next
                                    If ValueList <> "" Then
                                        ' folder_query_list.Text += name & ": " & ValueList & "<br />"
                                        QueryRebuild += name & "=" & ValueList & "!~!"
                                    End If
                                Else
                                    '  folder_query_list.Text += name & ": " & value & "<br />"
                                    QueryRebuild += name & "=" & value & "!~!"
                                End If
                            End If
                        Next
                        'Response.Write(QueryRebuild)
                        'QueryRebuild = QueryRebuild.TrimEnd("!~!")

                        'folder_query_list.Text += ")</em>"

                        cfolder_data.Text = QueryRebuild & "THEREALSEARCHQUERY=" & TheRealQueryString
                        cfolder_data.Enabled = False

                        EditFolder.Visible = False
                        AddFolder.Visible = True
                        cfolder_method.Text = "A"

                    ElseIf Not bNewStaticFolder Then

                        EditFolder.Visible = True
                        AddFolder.Visible = False

                        Select Case nFolderType
                            Case 3
                                Master.SetPageTitle("Edit Aircraft Folders")
                                foldertypeStringLabel.Text = "Aircraft"
                            Case 1
                                Master.SetPageTitle("Edit Company Folders")
                                foldertypeStringLabel.Text = "Company"
                            Case 2
                                Master.SetPageTitle("Edit Contact Folders")
                                foldertypeStringLabel.Text = "Contact"
                            Case 5
                                Master.SetPageTitle("Edit Event Folders")
                                foldertypeStringLabel.Text = "Event"
                                Add_Folder_Mode.Visible = False
                            Case 9
                                Master.SetPageTitle("Edit Wanteds Folders")
                                foldertypeStringLabel.Text = "Wanted"
                            Case 10
                                Master.SetPageTitle("Edit Yacht Folders")
                                foldertypeStringLabel.Text = "Yacht"
                            Case 14
                                Master.SetPageTitle("Edit Yacht History Folders")
                                foldertypeStringLabel.Text = "Yacht History"
                                Add_Folder_Mode.Visible = False
                            Case 15
                                Master.SetPageTitle("Edit Yacht Events Folders")
                                foldertypeStringLabel.Text = "Yacht Events"
                                Add_Folder_Mode.Visible = False
                            Case 16
                                Master.SetPageTitle("Edit Value View Folders")
                                foldertypeStringLabel.Text = "Value"
                                Add_Folder_Mode.Visible = False
                            Case 8
                                Master.SetPageTitle("Edit History Folders")
                                foldertypeStringLabel.Text = "History"
                ' Add_Folder_Mode.Visible = False
                            Case 12
                                Master.SetPageTitle("Edit Performance Specs Folders")
                                foldertypeStringLabel.Text = "Performance Specs"
                                Add_Folder_Mode.Visible = False
                            Case 13
                                Master.SetPageTitle("Edit Market Summary Folders")
                                foldertypeStringLabel.Text = "Market Summaries"
                                Add_Folder_Mode.Visible = False
                            Case 11
                                Master.SetPageTitle("Edit Operating Costs Folders")
                                foldertypeStringLabel.Text = "Operating Costs"
                                Add_Folder_Mode.Visible = False
                            Case 17
                                Master.SetPageTitle("Edit Airport View Folders")
                                foldertypeStringLabel.Text = "Airport View"
                                Add_Folder_Mode.Visible = False

                        End Select


                        'Needs to check for incorrect sort:
                        StartCheckingForIncorrectSortOnLoad(nFolderType)

                        BindFolderData(True, True, nFolderType)

                    End If
                End If

                'Figure out URL Redirect
                URLRedirect = Replace(parent_path.Text, "/", "")

                If Session.Item("localUser").crmAllowProjects_Flag = False Then
                    folder_submit_button.Visible = False
                    Add_Folder_Mode.Visible = False
                    AddFolder.CssClass = "display_disable"
                    AddFolder.Enabled = False
                    add_folder_table.CssClass = "data_aircraft_grid float_left display_disable"
                    add_folder_text.Text = "Your subscription does not have projects enabled."
                    add_text_panel.CssClass = "nonflyout_info_box_red remove_margin"
                End If

            ElseIf FolderDefaultEdit Then
                'Figuring Out DefaultFolder Setting:
                FigureOutDefaultFolder()

            End If

        End If


        ' if its aerodex and not live, then get rid of the button 7/6/2020 
        If Session.Item("localSubscription").crmFrequency <> "Live" And Session.Item("localPreferences").AerodexFlag = True Then
            folder_submit_button_flight.Attributes.Add("class", "display_none") ' remove the flight activity button 
        End If

    End Sub

    Private Sub FigureOutDefaultFolder()
        Add_Folder_Mode.Visible = False
        EditFolder.Visible = False
        AddFolder.Visible = False

        'We need to figure out if we're clearing our default or adding one (clearing the old)

        Dim AddDefault As Boolean = False
        Dim ClearDefault As Boolean = False
        Dim FolderID As Long = 0
        Dim cfolderData As String = ""
        Dim FolderData As New DataTable
        Dim TypeOfFolder As String = ""

        If nReportID > 0 Then
            FolderID = nReportID
        End If

        If Request.Form("REMOVE") <> Nothing Then
            If Not String.IsNullOrEmpty(Request.Form("REMOVE")) Then
                If Request.Form("REMOVE") = "true" Then
                    'remove just wants to be cleared.
                    ClearDefault = True
                End If
            End If
        End If

        If Request.Form("SAVE") <> Nothing Then
            If Not String.IsNullOrEmpty(Request.Form("SAVE")) Then
                If Request.Form("SAVE") = "true" Then
                    'Add Needs old to be cleared cleared and new to be Added
                    AddDefault = True
                    ClearDefault = True
                End If
            End If
        End If

        If ClearDefault Then
            'Clear Default Here.
            Master.aclsData_Temp.ClearDefaultFolderFlag(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo)
        End If

        If AddDefault Then
            'Add Default Here.
            Master.aclsData_Temp.UpdateDefaultFolderFlag("Y", FolderID, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo)
        End If

        'It is going to make a lot more sense and be a lot more efficient to lookup the cfolder data to rerun the folder
        'Then try to pass all of the data to this page through javascript.
        'Since it's already saved in the database and we're not really updating the actual folder information 
        'This seems like a better option. Passing it through javascript to rerun could end up introducing bugs,
        'This way no matter what - we're running what's in the database.
        FolderData = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(FolderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "A")
        If Not IsNothing(FolderData) Then
            If FolderData.Rows.Count > 0 Then
                cfolderData = FolderData.Rows(0).Item("cfolder_data").ToString
            End If
        End If


        'After we finished, closed and refresh.
        Dim booleanString As String = ""
        Dim javascriptFunction As String = "ParseForm"
        Select Case sTypeOfFolder.Trim.ToUpper
            Case "AIRCRAFT"
                booleanString = "false,false,false, false, false,"
            Case "COMPANY"
                booleanString = "false,false,true,false, false,"
            Case "CONTACT"
                booleanString = "false,false,true,false, false,"
            Case "YACHTS"
                booleanString = "false,false,false,false, true,"
            Case "EVENTS"
                booleanString = "false, true,false, false, false,"
            Case "WANTED"
                booleanString = "false,false,false,true, false,"
            Case "HISTORY"
                booleanString = "true,false,false, false, false,"
            Case "YACHT HISTORY"
                javascriptFunction = "ParseYachtSpecialFolders"
                booleanString = "true, false,"
            Case "YACHT EVENTS"
                javascriptFunction = "ParseYachtSpecialFolders"
                booleanString = "false, true,"
            Case "MARKET SUMMARIES"
                javascriptFunction = "ParseSpecsOperatingMarketForm"
                booleanString = "false,false,true,"
            Case "OPERATING COSTS"
                javascriptFunction = "ParseSpecsOperatingMarketForm"
                booleanString = "false,true,false,"
            Case "PERFORMANCE SPECS"
                javascriptFunction = "ParseSpecsOperatingMarketForm"
                booleanString = "true,false,false,"
        End Select
        Dim javascript As String = javascriptFunction & "('" & FolderID & "'," & booleanString & "'" & Replace(cfolderData, "'", "\'") & "');"

        javascript = " window.onload = function() {" & javascript & "; self.close();};"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ParseFormScript", javascript, True)

    End Sub

    ''' <summary>
    ''' This button click saves an add folder and inserts
    ''' </summary>
    ''' <param name="sender"></param> 
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub folder_submit_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles folder_submit_button.Click
        If Page.IsValid Then
            Dim checkTable As New DataTable
            Dim cfolderProcessDate As String = ""
            Dim cfolderProcessTime As Integer = 60
            If cfolder_type_of_folder.Text = "5" Then
                If IsNumeric(emptyBox.Text) Then
                    cfolderProcessTime = emptyBox.Text
                End If
                cfolderProcessDate = FormatDateTime(Now(), vbGeneralDate)
            End If
            Dim FolderID As Long = 0
            Dim aport_split_list As Array

            If cfolder_default.Checked = True And CInt(cfolder_type_of_folder.Text) = 17 Then
                bDefaultFolder = True
            End If

            checkTable = CheckForUniqueFolderName(clsGeneral.clsGeneral.PrepFolderNameForSave(cfolder_name.Text, True), IIf(IsNumeric(cfolder_id.Text), cfolder_id.Text, 0), cfolder_share.Checked, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

            If Not IsNothing(checkTable) Then
                If checkTable.Rows.Count = 0 Then
                    If Not IsNumeric(cfolder_id.Text) Then

                        If bDefaultFolder And CInt(cfolder_type_of_folder.Text) = 17 Then

                            Dim sQuery = New StringBuilder()
                            Dim SqlConn As New SqlClient.SqlConnection
                            Dim SqlCommand As New SqlClient.SqlCommand
                            Dim SqlReader As SqlClient.SqlDataReader
                            Dim SqlException As SqlClient.SqlException : SqlException = Nothing

                            Try


                                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                                SqlConn.Open()

                                SqlCommand.Connection = SqlConn
                                SqlCommand.CommandType = CommandType.Text
                                SqlCommand.CommandTimeout = 60

                                Try
                                    sQuery.Append("UPDATE Client_Folder SET cfolder_default_flag = 'N' WHERE cfolder_cftype_id = 17")
                                    sQuery.Append(" AND cfolder_sub_id = " + Session.Item("localUser").crmSubSubID.ToString + " AND cfolder_login = '" + Session.Item("localUser").crmUserLogin.ToString.Trim + "'  AND cfolder_seq_no = " + Session.Item("localUser").crmSubSeqNo.ToString)

                                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />folder_submit_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles folder_submit_button.Click</b><br />" + sQuery.ToString

                                    SqlCommand.CommandText = sQuery.ToString
                                    SqlCommand.ExecuteNonQuery()


                                Catch SqlException
                                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in saveAsDefaultAirportFolder ExecuteNonQuery : " + SqlException.Message
                                End Try

                            Catch ex As Exception

                                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in saveAsDefaultAirportFolder(ByVal AportFolderID As Long)) As Boolean " + ex.Message

                            Finally
                                SqlReader = Nothing

                                SqlConn.Dispose()
                                SqlConn.Close()
                                SqlConn = Nothing

                                SqlCommand.Dispose()
                                SqlCommand = Nothing
                            End Try

                        End If

                        If Trim(Request("id_list")) <> "" Then
                            cfolder_data.Text = "ac_aport_id=" & Trim(Request("id_list"))
                        End If


                        FolderID = Master.aclsData_Temp.Insert_Into_Evolution_Folders(CInt(cfolder_type_of_folder.Text), IIf(cfolder_hide.Checked, "Y", "N"), clsGeneral.clsGeneral.PrepFolderNameForSave(cfolder_name.Text, True), IIf(cfolder_share.Checked, "Y", "N"), cfolder_method.Text, clsGeneral.clsGeneral.StripChars(cfolder_description.Text, True), Replace(cfolder_data.Text, "'", "''"), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, IIf(cfolder_jetnet_run_flag.Checked, "Y", "N"), cfolder_jetnet_run_reply_username.Text, Replace(cfolder_jetnet_run_reply_email.Text, ";", ","), cfolderProcessDate, cfolderProcessTime, IIf(cfolder_operator_flag.Checked, "Y", "N"), bDefaultFolder)

                        If Trim(Request("id_list")) <> "" Then
                            If Trim(cfolder_data.Text) <> "" Then
                                aport_split_list = Split(cfolder_data.Text, ",")
                                aport_split_list(0) = Replace(aport_split_list(0), "ac_aport_id=", "")

                                For i = 0 To UBound(aport_split_list)
                                    If Trim(aport_split_list(0)) <> "" Then
                                        Call Master.aclsData_Temp.Insert_Into_Evolution_Folder_Index(FolderID, 0, 0, 0, 0, 0, 0, 0, 0, CLng(aport_split_list(i)))
                                    End If
                                Next
                            End If
                        End If

                        attention.Text = "<p align='center'><b>Your folder has been added</b></p>"
                    Else
                        If cfolder_id.Text <> 0 Then
                            FolderID = cfolder_id.Text
                            If (Master.aclsData_Temp.Edit_Fields_Evolution_Folders(Replace(cfolder_data.Text, "'", "''"), IIf(cfolder_hide.Checked, "Y", "N"), cfolder_name.Text, IIf(cfolder_share.Checked, "Y", "N"), cfolder_description.Text, cfolder_id.Text, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, IIf(cfolder_jetnet_run_flag.Checked, "Y", "N"), cfolder_jetnet_run_reply_username.Text, Replace(cfolder_jetnet_run_reply_email.Text, ";", ","), cfolderProcessDate, cfolderProcessTime, IIf(cfolder_operator_flag.Checked, "Y", "N"), bDefaultFolder)) = 1 Then
                                attention.Text = "<p align='center'><b>Your folder has been edited.</b></p>"

                                If bFromPreferences Then
                                    bRefreshPreferences = True
                                    Exit Sub
                                End If

                                If bFromHome Then
                                    bRefreshHome = True
                                    Exit Sub
                                End If

                            End If
                        End If
                    End If

                    If EditFolder.Visible = True Then
                        BindFolderData(True, True, nFolderType)

                    Else

                        If cfolder_method.Text = "A" Then
                            Dim booleanString As String = ""
                            Dim javascriptFunction As String = "ParseForm"
                            Dim additionalParameters As String = ""
                            Select Case CInt(cfolder_type_of_folder.Text)
                                Case 3
                                    booleanString = "false,false,false, false, false,"
                                Case 1
                                    booleanString = "false,false,true,false, false,"
                                Case 2
                                    booleanString = "false,false,true,false, false,"
                                Case 10
                                    booleanString = "false,false,false,false, true,"
                                Case 5
                                    booleanString = "false, true,false, false, false,"
                                Case 9
                                    booleanString = "false,false,false,true, false,"
                                Case 8
                                    booleanString = "true,false,false, false, false,"
                                Case 13
                                    javascriptFunction = "ParseSpecsOperatingMarketForm"
                                    booleanString = "false,false,true,"
                                Case 11
                                    javascriptFunction = "ParseSpecsOperatingMarketForm"
                                    booleanString = "false,true,false,"
                                Case 12
                                    javascriptFunction = "ParseSpecsOperatingMarketForm"
                                    booleanString = "true,false,false,"
                                Case 14
                                    javascriptFunction = "ParseYachtSpecialFolders"
                                    booleanString = "true,false,"
                                Case 15
                                    javascriptFunction = "ParseYachtSpecialFolders"
                                    booleanString = "false,true,"
                                Case 16
                                    javascriptFunction = "ParseViewFolders"
                                    booleanString = "27,"
                                    additionalParameters = ",''"

                            End Select

                            Dim javascript As String = javascriptFunction & "('" & FolderID & "'," & booleanString & "'" & Replace(cfolder_data.Text, "'", "\'") & "'" & additionalParameters & ");"
                            javascript = " window.onload = function() {" & javascript & "; setTimeout(function(){ window.close(); }, 2000);};"

                            If launchFlight.Checked = True Then
                                If CInt(cfolder_type_of_folder.Text) = 3 Then
                                    javascript = "window.onload = function() {"
                                    javascript += javascriptFunction & "('" & FolderID & "'," & booleanString & "'" & Replace(cfolder_data.Text, "'", "\'") & "'" & additionalParameters & ");"
                                    javascript += " setFlightActivityView('" & FolderID.ToString & "', '" & Replace(cfolder_name.Text, "'", "\'") & "'); setTimeout(function(){ window.close(); }, 2000);};"
                                ElseIf CInt(cfolder_type_of_folder.Text) = 1 Then
                                    javascript = " window.onload = function() {setOperatorAnalysisView('" & FolderID.ToString & "', '" & Replace(cfolder_name.Text, "'", "\'") & "'); setTimeout(function(){ window.close(); }, 2000);};"
                                End If
                            End If

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Parent_Window", javascript, True)

                        Else

                            If bNewStaticFolder And CInt(cfolder_type_of_folder.Text) = 17 Then

                                Response.Redirect("staticFolderEditor.aspx?folderID=" + FolderID.ToString + "&airport=true" + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", ""), True)

                                'This does not work in Firefox.
                                'Dim javascript As String = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + FolderID.ToString + "&airport=true" + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", "") + """,""StaticFolderEditor""); window.opener.location = '" + URLRedirect + "';self.close();"
                                'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Load_Airport_Folder_Refresh_Parent_Window", javascript, True)

                            ElseIf bNewStaticFolder And CInt(cfolder_type_of_folder.Text) = 1 Then

                                Response.Redirect("staticFolderEditor.aspx?folderID=" + FolderID.ToString + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", ""), True)
                                'This does not work in firefox.
                                'Dim javascript As String = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + FolderID.ToString + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", "") + """,""StaticFolderEditor""); window.opener.location = '" + URLRedirect + "';self.close();"
                                'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Load_Company_Folder_Refresh_Parent_Window", javascript, True)

                            ElseIf bNewStaticFolder And CInt(cfolder_type_of_folder.Text) = 3 Then

                                Response.Redirect("staticFolderEditor.aspx?folderID=" + FolderID.ToString + "&aircraft=true" + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", ""), True)
                                'This does not work in firefox.
                                'Dim javascript As String = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + FolderID.ToString + "&aircraft=true" + IIf(bFromHome, "&fromHome=true", "") + IIf(bDefaultFolder, "&default=true", "") + """,""StaticFolderEditor""); window.opener.location = '" + URLRedirect + "';self.close();"
                                'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Load_Company_Folder_Refresh_Parent_Window", javascript, True)

                            Else

                                RefreshPage()

                            End If

                        End If

                    End If

                Else
                    attention.Text = "<br /><br /><p align='center'><b>In order to " & IIf(IsNumeric(cfolder_id.Text), "edit a folder name", "insert a new folder") & ", your" & IIf(cfolder_share.Checked, " shared", "") & " folder name must be unique" & IIf(cfolder_share.Checked, " within your subscription", "") & ". Please pick another name and try again.</b></p>"
                End If
            Else
                attention.Text = "<br /><br /><p align='center'><b>There was a problem editing your folder.</b></p>"
            End If



        End If
    End Sub

    Public Sub RefreshPage()
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" + URLRedirect.Trim + "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    End Sub

    ''' <summary>
    ''' cancel for reorder list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub Cancel(ByVal sender As Object)

        'Reseting the Show Insert Items / Insert Index to false.
        If UCase(sender.id.ToString) = "SHARED_REORDER_LIST" Then
            Shared_Reorder_List.ShowInsertItem = False
            Shared_Reorder_List.EditItemIndex = -1
            'Rebinding
            BindFolderData(True, False, nFolderType)
        Else
            NonShared_Reorder_List.ShowInsertItem = False
            NonShared_Reorder_List.EditItemIndex = -1
            'Rebinding
            BindFolderData(False, True, nFolderType)
        End If


    End Sub

    ''' <summary>
    ''' delete for reorder list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        Try
            Dim id As TextBox = e.Item.FindControl("id")
            Dim is_admin As String = "N"
            'Response.Write("remove id: " & id.Text)

            Call commonLogFunctions.Log_User_Event_Data("UserFolderDelete", "Folder DELETED: (" & CLng(id.Text) & ")", Nothing, 0, 0, 0, 0, 0, 0, 0)

            Master.aclsData_Temp.Remove_Evolution_Folder_Index(0, CLng(id.Text), 0, 0, 0, 0, 0, 0)

            If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                is_admin = "Y"
            Else
                is_admin = "N"
            End If

            If Master.aclsData_Temp.Remove_Evolution_Folders(CInt(id.Text), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, is_admin) = 1 Then


                If UCase(sender.id.ToString) = "SHARED_REORDER_LIST" Then

                    Shared_Reorder_List.ShowInsertItem = False
                    Shared_Reorder_List.EditItemIndex = -1
                    BindFolderData(True, False, nFolderType)

                Else

                    NonShared_Reorder_List.ShowInsertItem = False
                    NonShared_Reorder_List.EditItemIndex = -1
                    BindFolderData(False, True, nFolderType)

                End If
                attention.Text = "<p align='center'><b>Your folder has been removed.</b></p>"
            Else
                attention.Text = "<p align='center'><b>We're sorry, there was a problem removing your data.</b></p>"
            End If

        Catch ex As Exception
            Master.LogError("Error on FolderMaintenance.aspx.vb: Delete() " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Save for folder add reorder list. To be looked at
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Sub Save_Row(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        attention.Text = ""
        If e.CommandName = "Save" Then 'Save the edits
            Save(sender, e)
        ElseIf e.CommandName = "Edit" Then
            Edit(sender, e) 'Sets up the edit form.
        ElseIf e.CommandName = "Delete" Then
            Delete(sender, e) 'Sets up the deletion
        ElseIf e.CommandName = "Cancel" Then
            Cancel(sender) 'Cancelling.
        End If

    End Sub
    ''' <summary>
    ''' Save for both reorder lists
    ''' </summary>
    ''' <param name="Sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Sub Save(ByVal Sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        Dim id As TextBox = e.Item.FindControl("id")
        Dim cfolder_new_name As TextBox = e.Item.FindControl("new_name")
        Dim cfolder_new_share As CheckBox = e.Item.FindControl("new_share")
        Dim cfolder_new_hide As CheckBox = e.Item.FindControl("new_hide")
        Dim cfolder_new_operator As CheckBox = e.Item.FindControl("new_operator")
        Dim cfolder_new_description As TextBox = e.Item.FindControl("new_description")
        Dim CheckTable As New DataTable

        CheckTable = CheckForUniqueFolderName(clsGeneral.clsGeneral.PrepFolderNameForSave(cfolder_new_name.Text, True), IIf(IsNumeric(id.Text), id.Text, 0), cfolder_new_share.Checked, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

        If Not IsNothing(CheckTable) Then
            If CheckTable.Rows.Count = 0 Then


                If (Master.aclsData_Temp.Edit_Fields_Evolution_Folders("", IIf(cfolder_new_hide.Checked, "Y", "N"), cfolder_new_name.Text, IIf(cfolder_new_share.Checked, "Y", "N"), cfolder_new_description.Text, id.Text, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", "", "", "", 0, IIf(cfolder_new_operator.Checked, "Y", "N"))) = 1 Then
                    If UCase(Sender.id.ToString) = "SHARED_REORDER_LIST" Then

                        Shared_Reorder_List.ShowInsertItem = False
                        Shared_Reorder_List.EditItemIndex = -1
                        BindFolderData(True, False, nFolderType)

                    Else

                        NonShared_Reorder_List.ShowInsertItem = False
                        NonShared_Reorder_List.EditItemIndex = -1
                        BindFolderData(False, True, nFolderType)

                    End If
                    attention.Text = "<p align='center'><b>Your folder has been edited.</b></p>"
                Else
                    attention.Text = "<p align='center'><b>We're sorry, there was a problem saving your data.</b></p>"
                End If

            Else
                attention.Text = "<br /><br /><p align='center'><b>In order to " & IIf(IsNumeric(id.Text), "edit a folder name", "insert a new folder") & ", your" & IIf(cfolder_new_share.Checked, " shared", "") & " folder name must be unique" & IIf(cfolder_new_share.Checked, " within your subscription", "") & ". Please pick another name and try again.</b></p>"
            End If
        Else
            attention.Text = "<br /><br /><p align='center'><b>There was a problem editing your folder.</b></p>"
        End If
    End Sub
    ''' <summary>
    ''' Edit for reorder list
    ''' </summary>
    ''' <param name="Sender"></param>
    ''' <remarks></remarks>
    Sub Edit(ByVal Sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        If UCase(Sender.id.ToString) = "SHARED_REORDER_LIST" Then

            Shared_Reorder_List.ShowInsertItem = False
            Shared_Reorder_List.EditItemIndex = CInt(e.Item.ItemIndex)
            BindFolderData(True, False, nFolderType)

        Else

            NonShared_Reorder_List.ShowInsertItem = False
            NonShared_Reorder_List.EditItemIndex = CInt(e.Item.ItemIndex)
            BindFolderData(False, True, nFolderType)

        End If
    End Sub


    Public Function CheckForUniqueFolderName(ByVal FolderName As String, ByVal FolderID As Long, ByVal SharedFolder As Boolean, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable

        Try
            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sql = "select cfolder_id, cfolder_sub_id, cfolder_login, cfolder_seq_no, cfolder_name, cfolder_cftype_id "
            sql += " from Client_Folder with (NOLOCK) where "

            sql += " cfolder_sub_id = @cfolder_sub_id"

            If FolderID > 0 Then
                'This means it's an edited folder and we're editing the name.
                'If we're checking for an existance of a name, exclude the folder ID that you pass.
                'Otherwise every time they edit a name, they'll need a new name.
                sql += " and cfolder_id <> @cfolder_id"
            End If

            If Not SharedFolder Then
                sql += " and cfolder_login = @cfolder_login"
                sql += " and cfolder_seq_no = @cfolder_seq_no"
            End If

            sql += " and upper(cfolder_name) = @cfolder_name"

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>CheckForUniqueFolderName(ByVal FolderName As String, ByVal SharedFolder As Boolean, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As DataTable</b><br />" & sql

            Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


            SqlCommand.Parameters.AddWithValue("cfolder_sub_id", subID)

            If FolderID > 0 Then
                SqlCommand.Parameters.AddWithValue("cfolder_id", FolderID)
            End If

            SqlCommand.Parameters.AddWithValue("cfolder_login", userLogin)
            SqlCommand.Parameters.AddWithValue("cfolder_seq_no", seqNO)
            SqlCommand.Parameters.AddWithValue("cfolder_name", Trim(UCase(FolderName)))



            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                TempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
            End Try

            CheckForUniqueFolderName = TempTable

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception
            CheckForUniqueFolderName = Nothing
            ' Me.class_error = "Error in CheckForUniqueFolderName(ByVal FolderName As String, ByVal SharedFolder As Boolean, ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As DataTable: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

    End Function

    ''' <summary>
    ''' Reorder the reordered list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub reorderSort_ItemReorder(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs) Handles Shared_Reorder_List.ItemReorder, NonShared_Reorder_List.ItemReorder
        Dim dataTable As New DataTable

        Select Case UCase(sender.id.ToString)
            Case "SHARED_REORDER_LIST"
                dataTable = DirectCast(Session("SharedFolderList"), DataTable)
                ReorderFolderList(dataTable, e, True)
            Case Else
                dataTable = DirectCast(Session("NonSharedFolderList"), DataTable)
                ReorderFolderList(dataTable, e, False)
        End Select


    End Sub


    ''' <summary>
    ''' Function that runs to reorder the folder list, it goes ahead and accepts a datatable
    ''' </summary>
    ''' <param name="datatable"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Sub ReorderFolderList(ByVal datatable As DataTable, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs, ByVal sharedFolders As Boolean)
        Try
            Dim oldIndex As Integer = e.OldIndex
            Dim newIndex As Integer = e.NewIndex
            Dim newPriorityOrder As Integer = CInt(datatable.Rows(newIndex)("cfolder_sort"))

            If newIndex > oldIndex Then
                'item moved down
                For i As Integer = oldIndex + 1 To newIndex
                    Dim propertyId As Integer = CInt(datatable.Rows(i)("cfolder_id"))
                    If propertyId <> -1 Then
                        datatable.Rows(i)("cfolder_sort") = CInt(datatable.Rows(i)("cfolder_sort")) - 1
                        UpdateSortOrderField(propertyId, CInt(datatable.Rows(i)("cfolder_sort")), 2)
                    End If
                Next
            Else
                'item moved up
                For i As Integer = oldIndex - 1 To newIndex Step -1
                    Dim propertyId As Integer = datatable.Rows(i)("cfolder_id")
                    If propertyId <> -1 Then
                        datatable.Rows(i)("cfolder_sort") = CInt(datatable.Rows(i)("cfolder_sort")) + 1
                        UpdateSortOrderField(propertyId, CInt(datatable.Rows(i)("cfolder_sort")), 2)
                    End If
                Next
            End If

            'Finally, update the priority for origional row            
            Dim id As Integer = CInt(datatable.Rows(oldIndex)("cfolder_id"))
            If id <> -1 Then
                UpdateSortOrderField(id, newPriorityOrder, 2)
            End If

            'Sets the session variable accordingly.
            If sharedFolders = True Then
                Session("SharedFolderList") = datatable
                BindFolderData(True, False, nFolderType)
            Else
                Session("NonSharedFolderList") = datatable
                BindFolderData(False, True, nFolderType)
            End If

            'Rebinds the Folder Data


        Catch ex As Exception
            Master.LogError("Error on FolderMaintenance.aspx.vb: ReorderFolderList() " & Master.aclsData_Temp.class_error)
            Master.aclsData_Temp.class_error = ""
        End Try
    End Sub
    ''' <summary>
    ''' Updates sort order field with new sort
    ''' </summary>
    ''' <param name="pId"></param>
    ''' <param name="newPriorityOrder"></param>
    ''' <param name="sort_field"></param>
    ''' <remarks></remarks>
    Private Sub UpdateSortOrderField(ByVal pId As Integer, ByVal newPriorityOrder As Integer, ByVal sort_field As Integer)
        Try
            Master.aclsData_Temp.Edit_Sort_Evolution_Folders(Math.Abs(newPriorityOrder), pId, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
        Catch ex As Exception
            Master.LogError("Error on FolderMaintenance.aspx.vb: BindFolderData() " & ex.Message)
            Master.aclsData_Temp.class_error = ""
        End Try
    End Sub


    ''' <summary>
    ''' This is a placeholder function for right now.
    ''' Eventually it will need to be shelled out to warn users that the static folders they create are linked to static items. A warning before
    ''' Delete.
    ''' </summary>
    ''' <param name="tcount"></param>
    ''' <remarks></remarks>
    Public Sub Display_Popup(ByVal tcount As Object)
        'Display_Popup = ""
        'If Not IsDBNull(tcount) Then
        '    Display_Popup = "if(!confirm('The folder that you wish to delete contains " & tcount & " record(s). Do you still want to delete?'))return false;"
        'End If
    End Sub

    ''' <summary>
    ''' Bind the reorder list data.
    ''' </summary>
    ''' <remarks></remarks>
    Sub BindFolderData(ByVal BindShared As Boolean, ByVal BindNonShared As Boolean, ByVal FolderType As Integer)
        'I am going to query twice. Once to get the list of shared folder for subscription
        'One to get a list of non-shared folders (personal folders)
        'After this (and loading to a datatable, we will go ahead and bind them to a reorder list.
        'The administrator only will be able to sort shared folders.
        Dim SharedFoldersTable As New DataTable
        Dim NonSharedFoldersTable As New DataTable

        Try

            'This is the main shared folder Query, it grabs all the folders by a subscription.
            'Eventually it will need to be passed a type of folder as well (based on what we're editing).
            'We're not there yet.

            If Trim(Me.export_types.SelectedValue) = "ALS" Then
                Me.personal_folders_panel.Visible = False
            Else
                Me.personal_folders_panel.Visible = True
            End If



            If BindShared = True And Me.export_types.SelectedValue <> "ALP" Then
                SharedFoldersTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "Y", FolderType, Nothing, "", Me.export_types.SelectedValue)

                If Not IsNothing(SharedFoldersTable) Then
                    If SharedFoldersTable.Rows.Count > 0 Then
                        'Setting the Reorder List
                        Shared_Reorder_List.DataSource = SharedFoldersTable
                        Session("SharedFolderList") = SharedFoldersTable
                        'This toggles off the ability for non Administrators to reorder the shared folders.
                        'NOTE: FOR RIGHT NOW, I'M KEEPING THE REORDER TO BE ON FOR TESTING.
                        'If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                        Shared_Reorder_List.AllowReorder = True
                        'Else
                        '    Shared_Reorder_List.AllowReorder = False
                        'End If
                        Shared_Reorder_List.DataBind()
                        shared_folders_panel.Visible = True
                    Else
                        'There are no shared folders at this time.
                        Shared_Reorder_List.DataSource = New DataTable
                        Shared_Reorder_List.DataBind()
                        shared_folders_panel.Visible = True

                    End If


                Else
                    Master.LogError("Error on FolderMaintenance.aspx.vb: BindFolderData() " & Master.aclsData_Temp.class_error)
                    Master.aclsData_Temp.class_error = ""
                End If
            ElseIf Me.export_types.SelectedValue = "ALP" Then
                Shared_Reorder_List.DataSource = New DataTable
                Shared_Reorder_List.DataBind()
                shared_folders_panel.Visible = False
            End If

            If BindNonShared = True And Me.export_types.SelectedValue <> "ALS" Then
                'Non Shared Folder Reorder List Binding
                NonSharedFoldersTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "N", FolderType, Nothing, "", Me.export_types.SelectedValue)
                If Not IsNothing(NonSharedFoldersTable) Then
                    If NonSharedFoldersTable.Rows.Count > 0 Then
                        'Setting the Non Shared Reorder list
                        NonShared_Reorder_List.DataSource = NonSharedFoldersTable
                        Session("NonSharedFolderList") = NonSharedFoldersTable
                        NonShared_Reorder_List.DataBind()
                        NonShared_Reorder_List.Visible = True
                    Else
                        NonShared_Reorder_List.DataSource = New DataTable
                        NonShared_Reorder_List.DataBind()
                        NonShared_Reorder_List.Visible = True
                    End If
                End If
            ElseIf Me.export_types.SelectedValue = "ALS" Then
                NonShared_Reorder_List.DataSource = New DataTable
                NonShared_Reorder_List.DataBind()
                NonShared_Reorder_List.Visible = False
            End If


            SharedFoldersTable.Dispose()
            SharedFoldersTable = Nothing

            NonSharedFoldersTable.Dispose()
            NonSharedFoldersTable = Nothing
        Catch ex As Exception
            Master.LogError("Error on FolderMaintenance.aspx.vb: Page Load " & ex.Message)
            Master.aclsData_Temp.class_error = ""
        End Try
    End Sub

    Protected Sub Item_Bound(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemEventArgs)

        Dim can_see_table As New DataTable
        Dim edit_button As LinkButton = CType(e.Item.FindControl("edit"), LinkButton)
        Dim delete_button As LinkButton = CType(e.Item.FindControl("delete"), LinkButton)
        Dim temp_id As TextBox = CType(e.Item.FindControl("ID"), TextBox)

        Dim new_share As CheckBox = CType(e.Item.FindControl("new_share"), CheckBox)

        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
            edit_button.Visible = True
            delete_button.Visible = True

        Else
            ' check is user isnt admin if its their folder
            can_see_table = Master.aclsData_Temp.Check_Permission_BySubscription(temp_id.Text, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "Y", nFolderType, Nothing, "")
            If Not IsNothing(can_see_table) Then
                If can_see_table.Rows.Count > 0 Then
                    edit_button.Visible = True
                    delete_button.Visible = True
                Else
                    edit_button.Visible = False
                    delete_button.Visible = False
                End If
            Else
                edit_button.Visible = False
                delete_button.Visible = False
            End If
        End If

    End Sub

    Private Sub export_types_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_types.SelectedIndexChanged

        If IsNumeric(Trim(Request("t"))) Then
            nFolderType = Trim(Request("t"))
        End If

        Call BindFolderData(True, True, nFolderType)


    End Sub

    ''' <summary>
    ''' The Goal of this function is to check and see if the cfolder sort is incorrect. 
    ''' Meaning no duplicate #'s
    ''' unfortunately this has to be run first before the binding, otherwise we wouldn't have to query twice.
    ''' </summary>
    ''' <remarks></remarks>
    Sub StartCheckingForIncorrectSortOnLoad(ByVal folderType As Integer)
        Dim BadSortTable As New DataTable
        Dim fixed_sort As Integer = 0

        BadSortTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "Y", folderType, Nothing, "")
        If Not IsNothing(BadSortTable) Then
            If BadSortTable.Rows.Count > 0 Then
                Checking_For_Bad_Sort(BadSortTable)
            End If
        End If

        BadSortTable = New DataTable
        BadSortTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "N", folderType, Nothing, "")
        If Not IsNothing(BadSortTable) Then
            If BadSortTable.Rows.Count > 0 Then
                Checking_For_Bad_Sort(BadSortTable)
            End If
        End If

        BadSortTable.Dispose()
        BadSortTable = Nothing
    End Sub

    ''' <summary>
    ''' We need to parse through the table looking at each sort ID to make sure unique.
    ''' </summary>
    ''' <param name="BadTable"></param>
    ''' <remarks></remarks>
    Sub Checking_For_Bad_Sort(ByVal BadTable As DataTable)
        Dim old_sort As Integer = -1
        Dim bad_data As Boolean = False
        If Not IsNothing(BadTable) Then
            If BadTable.Rows.Count > 0 Then
                For Each r As DataRow In BadTable.Rows
                    If old_sort = r("cfolder_sort") Then
                        bad_data = True
                    End If
                    old_sort = r("cfolder_sort")
                Next
            End If
        Else
            If Master.aclsData_Temp.class_error <> "" Then
                Master.LogError("Error on FolderMaintenance.aspx.vb: Checking For Bad Sort " & Master.aclsData_Temp.class_error)
                Master.aclsData_Temp.class_error = ""
            End If
        End If
        'We've found some bad sorting data, meaning the structure is compromised and needs to be redone.
        If bad_data = True Then
            ' Finally_Fixing_Data_Sort(BadTable)
        End If
    End Sub

    ''' <summary>
    ''' Subroutine of checking for bad sort. This finally updates the fields after bad sort as been identified.
    ''' </summary>
    ''' <param name="BadTable"></param>
    ''' <remarks></remarks>
    Sub Finally_Fixing_Data_Sort(ByVal BadTable As DataTable)
        Dim x As Integer = 0
        If Not IsNothing(BadTable) Then
            If BadTable.Rows.Count > 0 Then
                For Each r As DataRow In BadTable.Rows
                    UpdateSortOrderField(r("cfolder_id"), x, 2)
                    x += 1
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' This function runs when the page is opened in edit mode (not add mode), and the add folder button is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Add_Folder_Mode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_Folder_Mode.Click

        AddFolder.Visible = True

        If nFolderType = 1 Then
            cfolder_operator_flag.Enabled = True
            operatorAnalysisRow.CssClass = ""
            If Not IsNothing(Trim(Request("opChecked"))) Then
                If Trim(Request("opChecked")) = "true" Then
                    cfolder_operator_flag.Checked = True
                End If
            End If
        End If

        If Not bNewStaticFolder Then
            add_folder_table.Width = Unit.Percentage(53)
            EditFolder.Width = Unit.Percentage(46)
            Shared_Reorder_List.Width = Unit.Percentage(100)
            NonShared_Reorder_List.Width = Unit.Percentage(100)
        Else
            Add_Folder_Mode.Visible = False
            EditFolder.Visible = False
            add_folder_table.Width = Unit.Percentage(100)
            If nFolderType = 17 Then
                foldertypeStringLabel.Text = "Airport"
            ElseIf nFolderType = 3 Then
                foldertypeStringLabel.Text = "Aircrft"
            Else
                foldertypeStringLabel.Text = "Company"
            End If
            cfolder_share.Enabled = True
        End If

        If bDefaultFolder And nFolderType = 17 Then
            cfolder_default.Checked = True
        End If

        add_folder_table.CssClass = "data_aircraft_grid float_right"

        cancel_add_folder_button.Visible = True
        cfolder_method.Text = "S"
        cfolder_type_of_folder.Text = nFolderType.ToString
        add_folder_text.Text = " The purpose of this form is to create a Static Folder. " &
                        "Please name and describe the folder you are creating for easy " &
                        "reference and then click 'Add Folder' to save the folder for future use. "

        Master.SetPageTitle("Add a Static " & foldertypeStringLabel.Text & " Folder")

    End Sub

    ''' <summary>
    ''' This function runs whenever the page is opened in edit mode (not add mode), the "Add Folder button is clicked, bringing up the add form
    ''' Then the cancel add folder button is clicked, running this function.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cancel_add_folder_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_add_folder_button.Click

        If Not bNewStaticFolder Then

            AddFolder.Visible = False
            add_folder_table.CssClass = "data_aircraft_grid float_left"


            EditFolder.Width = Unit.Percentage(100)
            Shared_Reorder_List.Width = Unit.Pixel(590)
            NonShared_Reorder_List.Width = Unit.Pixel(590)


            Add_Folder_Mode.Visible = True
            cancel_add_folder_button.Visible = False
            add_folder_text.Text = " The purpose of this form is to create a Folder for the [Aircraft] search that you " &
                           "have just completed. Please name and describe the folder you are creating for easy " &
                           "reference and then click 'Add Folder' to save the folder for future use. "
            Master.SetPageTitle("Edit Folders")

        Else

            Add_Folder_Mode.Visible = False

            Dim javascript As String = " window.onload = function() { self.close(); };"
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "CloseWindowScript", javascript, True)

        End If


    End Sub


    ''' <summary>
    ''' This small function runs in the reorder list to determine what class is used to display the draghandle icon.
    ''' </summary>
    ''' <param name="share"></param>
    ''' <param name="hide"></param>
    ''' <param name="method"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FolderClassDisplay(ByVal share As String, ByVal hide As String, ByVal method As String) As String
        Dim ReturnClass As String = ""
        Dim SharedString As String = ""
        Dim HideString As String = ""
        Dim MethodString As String = ""

        If method = "S" Then
            MethodString = "Static"
        End If

        If hide = "Y" Then
            HideString = "Hide"
        End If

        If share = "Y" Then
            SharedString = "Share"
        End If


        ReturnClass = "dragHandle" & SharedString & HideString & MethodString

        Return "<div class=""" & ReturnClass & """></div>"
    End Function

    Public Sub checkEmail(ByVal sender As Object, ByVal args As ServerValidateEventArgs)
        If cfolder_jetnet_run_flag.Checked = True Then
            If cfolder_jetnet_run_reply_email.Text = "" Then
                args.IsValid = False
                Exit Sub
            End If
        End If
        args.IsValid = True
    End Sub

    Private Sub FolderMaintenance_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Page.IsPostBack Then
            Master.SetPageTitle("Folder Maintenance")
        End If
    End Sub
End Class