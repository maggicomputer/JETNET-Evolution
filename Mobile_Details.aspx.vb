Imports System.IO
Imports System
Partial Public Class Mobile_Details
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' If Master.Edit = False Then
            If Master.TypeOfListing = 1 Then
                features_visibility.Visible = False
                engine_visibility.Visible = False
                avionics_visibility.Visible = False
                usage_visibility.Visible = False
                maintenance_visibility.Visible = False
                equipment_visibility.Visible = False
                int_visibility.Visible = False
                cockpit_visibility.Visible = False
                apu_visibility.Visible = False
                transaction_visibility.Visible = False
                events_visibility.Visible = False
                Company_Details_View()

                content.Visible = True
            ElseIf Master.TypeOfListing = 3 Then
                opp_visibility.Visible = False
                Ac_Details_View()
                content.Visible = True
                aircraft_information.Visible = False
                aircraft_visibility.Visible = False
            End If
      Dim masterPage As Mobile = DirectCast(Page.Master, Mobile)

      Select Case masterPage.TypeOfListing
        Case 1
          'Company
          If masterPage.Listing_ContactID <> 0 Then
            Select Case masterPage.ListingSource
              Case "JETNET"
                folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
              Case "CLIENT"
                folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
            End Select
          Else 'No Contact 
            Select Case masterPage.ListingSource
              Case "JETNET"
                folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
              Case "CLIENT"
                folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
            End Select
          End If
        Case 3
          Select Case masterPage.ListingSource
            Case "JETNET"
              folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
            Case "CLIENT"
              folders_display.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
          End Select
      End Select



            If Session.Item("localSubscription").crmDocumentsFlag = False Then
                documents_visibility.Visible = False
            End If
        Catch ex As Exception
            Master.error_string = "mobile_details.aspx.vb - Page Load" & ex.Message
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End Try
    End Sub
    Private Sub Company_Details_View()
        Dim jetnet_comp_id As Integer = 0
        Dim Company_Results As New DataTable
        Dim Preferences_Table As New DataTable
        Dim contact_text As String = ""
        Dim Company_Phone_Array As New ArrayList



        'First we'd like to display the client preferences special field with our company Data.
        'So this is call #1 to the database. 
        Preferences_Table = Nothing

        Company_Results = Master.aclsData_Temp.GetCompanyInfo_ID(Master.ListingID, Master.ListingSource, 0)
        ' check the state of the DataTable
        If Not IsNothing(Company_Results) Then
            If Company_Results.Rows.Count > 0 Then
                For Each R As DataRow In Company_Results.Rows
                    'Sets the variables for the company display
                    If Master.ListingSource = "CLIENT" Then
                        jetnet_comp_id = IIf(Not IsDBNull(R("jetnet_comp_id")), R("jetnet_comp_id"), 0)
                        switched.CssClass = "client_block"
                        Master.Set_Edit_Button(IIf(Session.Item("search_company") <> "", "<a href='mobile_listing.aspx?type=" & IIf(Master.Listing_ContactID <> 0, 2, 1) & "&redo_search=true'>Back to Listing</a>", "") & "<a href='edit.aspx?type=company&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='edit_links'>Edit</a> " & IIf(jetnet_comp_id <> 0, "(<a href='mobile_details.aspx?type=1&comp_ID=" & jetnet_comp_id & "&source=JETNET' class='edit_links'>View Jetnet Company</a>)", ""))
                        Master.OtherID = jetnet_comp_id
                    Else
                        Master.aTempTable = Master.aclsData_Temp.GetCompanyInfo_JETNET_ID(Master.ListingID, "")
                        If Not IsNothing(Master.aTempTable) Then 'not nothing
                            If Master.aTempTable.Rows.Count > 0 Then
                                Master.OtherID = Master.aTempTable.Rows(0).Item("comp_id")
                            End If
                        Else
                        End If
                        Master.Set_Edit_Button(IIf(Session.Item("search_company") <> "", "<a href='mobile_listing.aspx?type=" & IIf(Master.Listing_ContactID <> 0, 2, 1) & "&redo_search=true'>Back to Listing</a>", "") & IIf(Master.OtherID <> 0, "(<a href='mobile_details.aspx?type=1&comp_ID=" & Master.OtherID & "&source=CLIENT' class='edit_links'>View Client Company</a>)", "<a href='edit.aspx?type=company&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='edit_links'>Create Client Company</a>"))

                    End If

                    Dim Company_Data As New clsClient_Company
                    Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, Master.ListingSource, Preferences_Table)
                    'Builds the company Display
                    contact_text = "<h2>" & Company_Data.clicomp_name & "</h2>" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)
                Next
            Else
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('There was an error with the url you've clicked. Please hit back and try again.');", True)
            End If
        Else
            If Master.aclsData_Temp.class_error <> "" Then
                Master.error_string = "ListingsMaster.vb - Fill_Company_Info() - " & Master.aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
            End If
        End If
        information.Text = contact_text

        '------Phone Company Information Left Card Display----------------------------------------------------------------------
        contact_text = ""

        Try
            Master.aTempTable = Master.aclsData_Temp.GetPhoneNumbers(Master.ListingID, 0, Master.ListingSource, 0)
            '' check the state of the DataTable
            If Not IsNothing(Master.aTempTable) Then
                If Master.aTempTable.Rows.Count > 0 Then
                    ' set it to the datagrid 
                    Company_Phone_Array = clsGeneral.clsGeneral.Create_Array_Phone_Class(Master.aTempTable)

                    For i = 0 To Company_Phone_Array.Count - 1
                        contact_text = contact_text & clsGeneral.clsGeneral.show_phone_display(Company_Phone_Array(i))
                    Next
                End If
            Else
                If Master.aclsData_Temp.class_error <> "" Then
                    Master.error_string = "listings.master.vb - Phone Numbers Array Company() - " & Master.aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
                End If
            End If
        Catch ex As Exception
            Master.error_string = "listings.master.vb - Phone Numbers Array Company" & ex.Message
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End Try
        phone.Text = contact_text

        Dim counter As Integer = 0
        Dim contact_id As Integer = 0
        Dim contact_first_name As String = ""
        Dim contact_title As String = ""
        Dim contact_last_name As String = ""
        Dim contact_email_address As String = ""
        Dim strContact As String = ""
        Dim cliacref_contact_priority As Integer = 0
        Dim acref_id As Integer = 0
        Dim cell_text As New Label
        Dim ac_contact As New Table
        Dim color As String = "eaeaea"
        strContact = ""
        '  If Master.Listing_ContactID = 0 Then
        'fill contacts
        Master.aTempTable = Master.aclsData_Temp.GetContacts(Master.ListingID, Master.ListingSource, "Y", 0)
        'strContact = 

        ' check the state of the DataTable
        If Not IsNothing(Master.aTempTable) Then
            If Master.aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In Master.aTempTable.Rows
                    contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), "")
                    If Master.Listing_ContactID = contact_id Or Master.Listing_ContactID = 0 Then


                        contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                        contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                        contact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
                        contact_email_address = IIf(Not IsDBNull(r("contact_email_address")), r("contact_email_address"), "")

                        If color = "alt_contact_row" Then
                            color = "contact_row"
                        Else
                            color = "alt_contact_row"
                        End If
                        strContact = strContact & "<span class='" & color & "'>"

                        If Master.Listing_ContactID <> 0 Then
                            If Master.ListingSource = "CLIENT" Then
                                Master.Set_Edit_Button("<a href='edit.aspx?type=company&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>Edit Company</a>  <a href='edit.aspx?type=contact&contact_ID=" & contact_id & "&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>Edit Contact</a>  <a href='mobile_details.aspx?type=1&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>View All Contacts</a>")
                            Else
                                Master.Set_Edit_Button("<a href='mobile_details.aspx?type=1&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>View All Contacts</a>")
                            End If
                            strContact = strContact & "" & contact_first_name & " " & contact_last_name & ""

                        Else
                            If Master.ListingSource = "JETNET" Then
                                strContact = strContact & "<a href='mobile_details.aspx?type=1&contact_ID=" & contact_id & "&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>" & contact_first_name & " " & contact_last_name & "</a> " & IIf(Master.Listing_ContactID <> 0, "<a href='mobile_listing.aspx?type=1&redo_search=true'>View All Contacts</a>", "")
                            Else
                                strContact = strContact & "<a href='edit.aspx?type=contact&contact_id=" & contact_id & "&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='bold'>Edit " & contact_first_name & " " & contact_last_name & "</a>" & IIf(Master.Listing_ContactID <> 0, "<a href='mobile_listing.aspx?type=1&redo_search=true'>View All Contacts</a>", "")
                            End If
                        End If

                        If Trim(contact_title) <> "" Then
                            strContact = strContact & " <b>(" & contact_title & ")</b>"
                        End If
                        If contact_email_address <> "" Then
                            strContact = strContact & " <Br /><a href='mailto:" & contact_email_address & "'>" & contact_email_address & "</a>"
                        End If
                        strContact = strContact & "<br />"
                        Try
                            Master.aTempTable = Master.aclsData_Temp.GetPhoneNumbers(Master.ListingID, contact_id, Master.ListingSource, 0)
                            '' check the state of the DataTable
                            If Not IsNothing(Master.aTempTable) Then
                                If Master.aTempTable.Rows.Count > 0 Then
                                    ' set it to the datagrid 
                                    Company_Phone_Array = clsGeneral.clsGeneral.Create_Array_Phone_Class(Master.aTempTable)

                                    For i = 0 To Company_Phone_Array.Count - 1
                                        strContact = strContact & clsGeneral.clsGeneral.show_phone_display(Company_Phone_Array(i))
                                    Next
                                Else
                                    'rows = 0
                                    'Phone_Text = ""
                                End If
                            Else
                                If Master.aclsData_Temp.class_error <> "" Then
                                    Master.error_string = "listings.master.vb - Phone Numbers Array Company() - " & Master.aclsData_Temp.class_error
                                    clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
                                End If

                            End If
                        Catch ex As Exception
                            Master.error_string = "listings.master.vb - Phone Numbers Array Company" & ex.Message
                            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
                        End Try
                        strContact = strContact & "</span>"
                    End If
                Next

            End If
        End If
        ' End If
        contact_information.Text = strContact

        Try
            Dim tbl As New Table
            '-------------------Aircraft Tab Listing-------------------------------------------------------------------------------------------
            If Master.ListingSource = "CLIENT" Then
                Master.aTempTable = Master.aclsData_Temp.Get_Client_JETNET_AC(Master.ListingID, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
            Else
                Master.aTempTable = Master.aclsData_Temp.GetAircraft_Listing_compid(Master.ListingID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
            End If

            If Not IsNothing(Master.aTempTable) Then
                If Master.aTempTable.Rows.Count > 0 Then

                    tbl = clsGeneral.clsGeneral.Mobile_Build_Company_Aircraft_Tab(Master.aTempTable, True)
                    aircraft_information.Controls.Clear()
                    aircraft_information.Controls.Add(tbl)
                    Master.aTempTable = Nothing
                Else
                    aircraft_information.Text = "<p align='center' class='attention'><b>No aircraft associated with this company.</b></p>"
                    aircraft_information.Controls.Clear()

                End If
            Else
                aircraft_information.Text = "<p align='center' class='attention'><b>No aircraft associated with this company.</b></p>"
                aircraft_information.Controls.Clear()
                If Master.aclsData_Temp.class_error <> "" Then
                    Master.error_string = "listings.master.vb - fill_comp_AC() - " & Master.aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
                End If
            End If


            If Master.ListingSource = "CLIENT" Then 'If notes are a client Aircraft
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Master.ListingID, 0, "A", False, True) 'Datahook for client/note aircraft
            Else
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Master.ListingID, "A", False, True) 'Datahook for jetnet/note aircraft
            End If

            Dim Note_Array As ArrayList
            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                notes_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            If Master.ListingSource = "CLIENT" Then 'If notes are a client Aircraft
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Master.ListingID, 0, "O", False, True) 'Datahook for client/note aircraft
            Else
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Master.ListingID, "O", False, True) 'Datahook for jetnet/note aircraft
            End If

            Note_Array = New ArrayList
            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                opp_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            If Master.ListingSource = "CLIENT" Then 'If notes are a client Aircraft
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Master.ListingID, 0, "P", False, True) 'Datahook for client/note aircraft
            Else
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Master.ListingID, "P", False, True) 'Datahook for jetnet/note aircraft
            End If

            Note_Array = New ArrayList
            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                actions_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            'Sharing Document Data
            If Master.ListingSource = "CLIENT" Then 'If notes are a client Aircraft
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Master.ListingID, 0, "F", False, True) 'Datahook for client/note aircraft
            Else
                Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Master.ListingID, "F", False, True) 'Datahook for jetnet/note aircraft
            End If

            Note_Array = New ArrayList
            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                documents_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

        Catch ex As Exception
            Master.error_string = "listings.master.vb - fill_comp_AC() - " & ex.Message
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End Try

    End Sub
    Private Sub Ac_Details_View()

        ' Menu.Height = 38
        Dim CLIENT_Aircraft_Data As New clsClient_Aircraft
        Dim JETNET_Aircraft_Data As New clsClient_Aircraft
        Dim Aircraft_Data As New clsClient_Aircraft

        Dim Aircraft_Model As String = ""
        'Dim Aircraft_Model_Data As New clsClient_Aircraft_Model
        Dim Price_Status As Integer = 0
        Dim jetnet_ac_id As Integer = 0
        Dim client_ac_id As Integer = 0
        '--------Basic Aircraft Left Card Display for the AC Information------------------------------------------------
        Try
            If UCase(Master.ListingSource) = "CLIENT" Then
                Master.aTempTable = Master.aclsData_Temp.Get_Clients_Aircraft(Master.ListingID)
                If Master.aTempTable.Rows.Count > 0 Then
                    switched.CssClass = "client_block"
                    Aircraft_Model = (Master.aTempTable.Rows(0).Item("cliamod_make_name") & " " & Master.aTempTable.Rows(0).Item("cliamod_model_name"))
                    CLIENT_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Master.aTempTable, "cliaircraft")
                    CLIENT_Aircraft_Data.cliaircraft_id = Master.ListingID
                    jetnet_ac_id = Master.aTempTable.Rows(0).Item("cliaircraft_jetnet_ac_id")
                    client_ac_id = Master.aTempTable.Rows(0).Item("cliaircraft_id")
                    Master.OtherID = jetnet_ac_id
                    Master.Set_Edit_Button(IIf(Session.Item("search_aircraft") <> "", "<a href='mobile_listing.aspx?type=3&redo_search=true'>Back to Listing</a> ", "") & "<a href='edit.aspx?type=aircraft&ac_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='edit_links'>Edit</a> " & IIf(jetnet_ac_id <> 0, "(<a href='mobile_details.aspx?type=3&ac_ID=" & jetnet_ac_id & "&source=JETNET' class='edit_links'>View Jetnet Aircraft</a>)", ""))

                    'NO WE HAVE TO POLL THE DATABASE FOR THIS AC INFO AGAIN.
                    Master.aTempTable = Master.aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_ac_id, "")
                    JETNET_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Master.aTempTable, "ac")
                    Master.aTempTable.Dispose()

                    Aircraft_Data = CLIENT_Aircraft_Data
                End If
                Master.aTempTable.Dispose()
            ElseIf UCase(Master.ListingSource) = "JETNET" Then
                Master.aTempTable = Master.aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(Master.ListingID, "")
                If Master.aTempTable.Rows.Count > 0 Then
                    JETNET_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Master.aTempTable, "ac")
                    Aircraft_Model = (Master.aTempTable.Rows(0).Item("amod_make_name") & " " & Master.aTempTable.Rows(0).Item("amod_model_name"))
                    JETNET_Aircraft_Data.cliaircraft_id = Master.ListingID
                    jetnet_ac_id = Master.ListingID

                    'other id for jetnet listing
                    Master.aTempTable2 = Master.aclsData_Temp.Get_Client_Aircraft_JETNET_AC(JETNET_Aircraft_Data.cliaircraft_id)
                    If Not IsNothing(Master.aTempTable2) Then
                        If Master.aTempTable2.Rows.Count > 0 Then
                            Master.OtherID = Master.aTempTable2.Rows(0).Item("cliaircraft_id")
                            client_ac_id = Master.aTempTable2.Rows(0).Item("cliaircraft_id")
                            CLIENT_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Master.aTempTable2, "cliaircraft")
                            Master.aTempTable.Dispose()
                        End If
                    Else
                        If Master.aclsData_Temp.class_error <> "" Then
                            Master.error_string = "Mobile_Details.ascx.vb - Ac_Details_View() - " & Master.aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
                        End If
                    End If
                    Master.OtherID = client_ac_id
                    Master.Set_Edit_Button(IIf(Session.Item("search_aircraft") <> "" And Session.Item("FromTypeOfListing") = 3, "<a href='mobile_listing.aspx?type=3&redo_search=true'>Back to Listing</a>", "") & IIf(client_ac_id <> 0, "", "<a href='edit.aspx?type=aircraft&ac_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "' class='edit_links'>Create Client Aircraft</a> ") & IIf(client_ac_id <> 0, "(<a href='mobile_details.aspx?type=3&ac_ID=" & client_ac_id & "&source=CLIENT' class='edit_links'>View Client Aircraft</a>)", ""))

                    Aircraft_Data = JETNET_Aircraft_Data
                End If
            End If

            Dim aircraft_text As String = ""
            information.Text = "<h2> " & Aircraft_Model & "</h2>" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, False) & "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, False, True, False) & ""
            If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                If Master.ListingSource = "JETNET" Then
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Aircraft_Data.cliaircraft_id, "A", True, False)
                Else
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, 0, "A", True, False)
                End If
            End If

            Dim Note_Array As ArrayList
            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                notes_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            'Action Items!
            If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                If Master.ListingSource = "JETNET" Then
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Aircraft_Data.cliaircraft_id, "P", True, False)
                Else
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, 0, "P", True, False)
                End If
            End If

            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                actions_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            'Documents!
            If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                If Master.ListingSource = "JETNET" Then
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(0, Aircraft_Data.cliaircraft_id, "F", True, False)
                Else
                    Master.aTempTable = Master.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, 0, "F", True, False)
                End If
            End If

            If Master.aTempTable.Rows.Count > 0 Then
                Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(Master.aTempTable)
                documents_display.Text = clsGeneral.clsGeneral.Notes_Class_Display(Note_Array)
            End If

            If Master.ListingSource = "JETNET" Then
                clsGeneral.clsGeneral.Build_Transaction_Tab(jetnet_ac_id, client_ac_id, Master.OtherID, Master.ListingID, Master.ListingSource, Master, Nothing, "both", jetnet_transactions, Nothing)
                jetnet_features_display.Text = clsGeneral.clsGeneral.Build_JETNET_Features_Tab(jetnet_ac_id, "JETNET", Master.ListingID, Master, Nothing)
                jetnet_engine_display.Text = clsGeneral.clsGeneral.Build_Both_Engine_Tab_Mobile_Only(jetnet_ac_id, 0, Master.ListingID, "JETNET", Master, Nothing)
                jetnet_avionics_display.Text = clsGeneral.clsGeneral.Build_JETNET_Avionics_Tab(jetnet_ac_id, Master.ListingID, "JETNET", Master, Nothing)
                Master.aTempTable2 = Master.aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID(jetnet_ac_id)
                jetnet_apu_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "apu", Master.aTempTable2)
                jetnet_usage_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "usage", Master.aTempTable2)
                jetnet_interior_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "interior", Master.aTempTable2)
                jetnet_exterior_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "exterior", Master.aTempTable2)
                jetnet_maintenance_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "maintenance", Master.aTempTable2)
                jetnet_cockpit_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "cockpit", Master.aTempTable2)
                jetnet_equipment_display.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_ac_id, "JETNET", JETNET_Aircraft_Data, Master, Nothing, "equipment", Master.aTempTable2)
                jetnet_event_display.Text = clsGeneral.clsGeneral.Build_Event_Tab(jetnet_ac_id, Master.OtherID, Master.ListingID, Master.ListingSource, Master, Nothing)
            ElseIf Master.ListingSource = "CLIENT" Then
                clsGeneral.clsGeneral.Build_Transaction_Tab(jetnet_ac_id, client_ac_id, Master.OtherID, Master.ListingID, Master.ListingSource, Master, Nothing, "both", jetnet_transactions, Nothing)
                client_engine_display.Text = clsGeneral.clsGeneral.Build_Both_Engine_Tab_Mobile_Only(0, client_ac_id, Master.ListingID, "CLIENT", Master, Nothing)
                client_features_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Features_Tab(client_ac_id, "CLIENT", Master.ListingID, Master, Nothing)
                client_avionics_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Avionics_Tab(client_ac_id, Master.ListingID, "CLIENT", Master, Nothing)
                Master.aTempTable2 = Master.aclsData_Temp.Get_Client_Aircraft_Details(client_ac_id)
                client_apu_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "apu", Master.aTempTable2)
                client_usage_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "usage", Master.aTempTable2)
                client_interior_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "interior", Master.aTempTable2)
                client_exterior_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "exterior", Master.aTempTable2)
                client_maintenance_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "maintenance", Master.aTempTable2)
                client_cockpit_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "cockpit", Master.aTempTable2)
                client_equipment_display.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_ac_id, "CLIENT", CLIENT_Aircraft_Data, Master, Nothing, "equipment", Master.aTempTable2)
            End If

            contact_information.Controls.Add(clsGeneral.clsGeneral.New_fill_Contact_Info_AC(Aircraft_Data.cliaircraft_id, Master.ListingSource, 3, Master.aclsData_Temp, Master, Nothing, Nothing))

        Catch ex As Exception
            Master.error_string = "Mobile_Details.aspx.vb - Fill_AC_Info() " & ex.Message
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End Try
    End Sub
    Private Sub Toggle_Visibility(ByVal Type As String)
        Select Case Type
            Case "all_switch"
                If Master.TypeOfListing = 1 Then
                    aircraft_visibility.Visible = True
                    notes_visibility.Visible = True
                    actions_visibility.Visible = True
                    documents_visibility.Visible = True
                    folders_visibility.Visible = True

                    events_visibility.Visible = False
                    transaction_visibility.Visible = False
                    equipment_visibility.Visible = False
                    apu_visibility.Visible = False
                    engine_visibility.Visible = False
                    features_visibility.Visible = False
                    usage_visibility.Visible = False
                    int_visibility.Visible = False
                    avionics_visibility.Visible = False
                    cockpit_visibility.Visible = False
                    maintenance_visibility.Visible = False
                Else
                    aircraft_visibility.Visible = False
                    notes_visibility.Visible = False
                    actions_visibility.Visible = False
                    documents_visibility.Visible = False
                    folders_visibility.Visible = False

                    events_visibility.Visible = True
                    transaction_visibility.Visible = True
                    equipment_visibility.Visible = True
                    apu_visibility.Visible = True
                    engine_visibility.Visible = True
                    features_visibility.Visible = True
                    usage_visibility.Visible = True
                    int_visibility.Visible = True
                    avionics_visibility.Visible = True
                    cockpit_visibility.Visible = True
                    maintenance_visibility.Visible = True
                End If
            Case "aircraft_switch"

                If Master.TypeOfListing = 1 Then
                    aircraft_visibility.Visible = True
                    notes_visibility.Visible = False
                    actions_visibility.Visible = False
                    documents_visibility.Visible = False
                    folders_visibility.Visible = False

                    events_visibility.Visible = False
                    transaction_visibility.Visible = False
                    equipment_visibility.Visible = False
                    apu_visibility.Visible = False
                    engine_visibility.Visible = False
                    features_visibility.Visible = False
                    usage_visibility.Visible = False
                    int_visibility.Visible = False
                    avionics_visibility.Visible = False
                    cockpit_visibility.Visible = False
                    maintenance_visibility.Visible = False
                ElseIf Master.TypeOfListing = 3 Then
                    aircraft_visibility.Visible = False
                    notes_visibility.Visible = False
                    actions_visibility.Visible = False
                    documents_visibility.Visible = False
                    folders_visibility.Visible = False

                    events_visibility.Visible = True
                    transaction_visibility.Visible = True
                    equipment_visibility.Visible = True
                    apu_visibility.Visible = True
                    engine_visibility.Visible = True
                    features_visibility.Visible = True
                    usage_visibility.Visible = True
                    int_visibility.Visible = True
                    avionics_visibility.Visible = True
                    cockpit_visibility.Visible = True
                    maintenance_visibility.Visible = True

                End If

            Case "notes_switch"
                aircraft_visibility.Visible = False
                notes_visibility.Visible = True
                actions_visibility.Visible = False
                documents_visibility.Visible = False
                folders_visibility.Visible = False

                events_visibility.Visible = False
                transaction_visibility.Visible = False
                equipment_visibility.Visible = False
                apu_visibility.Visible = False
                engine_visibility.Visible = False
                features_visibility.Visible = False
                usage_visibility.Visible = False
                int_visibility.Visible = False
                avionics_visibility.Visible = False
                cockpit_visibility.Visible = False
                maintenance_visibility.Visible = False
            Case "actions_switch"
                aircraft_visibility.Visible = False
                notes_visibility.Visible = False
                actions_visibility.Visible = True
                documents_visibility.Visible = False
                folders_visibility.Visible = False

                events_visibility.Visible = False
                transaction_visibility.Visible = False
                equipment_visibility.Visible = False
                apu_visibility.Visible = False
                engine_visibility.Visible = False
                features_visibility.Visible = False
                usage_visibility.Visible = False
                int_visibility.Visible = False
                avionics_visibility.Visible = False
                cockpit_visibility.Visible = False
                maintenance_visibility.Visible = False
            Case "documents_switch"
                aircraft_visibility.Visible = False
                notes_visibility.Visible = False
                actions_visibility.Visible = False
                documents_visibility.Visible = True
                folders_visibility.Visible = False

                events_visibility.Visible = False
                transaction_visibility.Visible = False
                equipment_visibility.Visible = False
                apu_visibility.Visible = False
                engine_visibility.Visible = False
                features_visibility.Visible = False
                usage_visibility.Visible = False
                int_visibility.Visible = False
                avionics_visibility.Visible = False
                cockpit_visibility.Visible = False
                maintenance_visibility.Visible = False
            Case "folders_switch"
                aircraft_visibility.Visible = False
                notes_visibility.Visible = False
                actions_visibility.Visible = False
                documents_visibility.Visible = False
                folders_visibility.Visible = True

                events_visibility.Visible = False
                transaction_visibility.Visible = False
                equipment_visibility.Visible = False
                apu_visibility.Visible = False
                engine_visibility.Visible = False
                features_visibility.Visible = False
                usage_visibility.Visible = False
                int_visibility.Visible = False
                avionics_visibility.Visible = False
                cockpit_visibility.Visible = False
                maintenance_visibility.Visible = False

        End Select

        If Session.Item("localSubscription").crmDocumentsFlag = False Then
            documents_visibility.Visible = False
        End If
    End Sub
    Private Sub all_switch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles all_switch.Click, aircraft_switch.Click, notes_switch.Click, actions_switch.Click, documents_switch.Click, folders_switch.Click


        Dim handle As LinkButton = CType(sender, LinkButton)
        all_switch.Font.Underline = False
        aircraft_switch.Font.Underline = False
        notes_switch.Font.Underline = False
        actions_switch.Font.Underline = False
        documents_switch.Font.Underline = False
        folders_switch.Font.Underline = False
        handle.Font.Underline = True
        Toggle_Visibility(handle.ID)
    End Sub
    Private Sub save_folder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_folder.Click
        clsGeneral.clsGeneral.saveFolder(Master, Nothing, folders_display)
    End Sub
End Class