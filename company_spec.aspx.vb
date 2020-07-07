Partial Public Class company_spec
    Inherits System.Web.UI.Page
    Dim LookupTable As New DataTable
    Dim TempTable As New DataTable
    Dim TempTable2 As New DataTable
    Dim aTempTable As New DataTable
    Dim aTempTable2 As New DataTable
    Dim aclsData_Temp As New Object 'Class Managers used
    Dim error_string As String = ""
    Dim m_bIsTerminating As Boolean = False

    Sub Company_Aircraft_Tab(ByVal idnum As Integer, ByVal source As String)
        Try
            Dim tbl As New Table
            '-------------------Aircraft Tab Listing-------------------------------------------------------------------------------------------
            If source = "CLIENT" Then
                aTempTable = aclsData_Temp.Get_Client_JETNET_AC(idnum, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
            Else
                aTempTable = aclsData_Temp.GetAircraft_Listing_compid(idnum, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
            End If

            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then

                    tbl = clsGeneral.clsGeneral.Build_Company_Aircraft_Tab(aTempTable, False)
                    Aircraft_Information.Controls.Clear()
                    Aircraft_Information.Controls.Add(tbl)
                    aTempTable = Nothing
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("company_spec.aspx.vb - fill_comp_AC(" & idnum & "/" & source & ") - " & error_string)
                    End If
                    display_error()
                End If
            Else

            End If
        Catch ex As Exception
            error_string = "company_spec.aspx.vb - fill_comp_AC(" & idnum & "/" & source & ") -  " & ex.Message
            LogError(error_string)
        End Try
    End Sub

    Public Function display_error()
        '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
        display_error = ""
        If aclsData_Temp.class_error <> "" Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
        End If
        aclsData_Temp.class_error = ""
    End Function

    Public Sub LogError(ByVal ex As String)
        aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
    End Sub

    Private Sub company_spec_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            aclsData_Temp = New clsData_Manager_SQL

      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
      aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")
            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            End If

            'Declaring the variables
            Dim Company_Data As New clsClient_Company
            Dim Company_ID As Integer = 0
            Dim Other_ID As Integer = 0
            Dim Company_Source As String = "JETNET"
            Dim Company_Results As New DataTable
            Dim Preferences_Table As New DataTable
            Dim Contact_Class_Array As New ArrayList
            Dim Contact_Display As String = ""
            Dim Company_Phone_Array As New ArrayList
            Dim Phone_Text As String = ""
            Dim Count As Integer = 0
            Dim color As String = "E3EDF8"
            Dim daystr As String = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now())
            Dim lnote_order As String = "lnote_schedule_start_date asc "
            Dim Note_Array As New ArrayList

            'Grabbing the Querystring variables
            If Not IsNothing(Request.Item("company_ID")) Then
                If Not String.IsNullOrEmpty(Request.Item("company_ID").ToString) Then
                    Company_ID = Request.Item("company_ID").Trim
                End If
            Else
                Company_ID = Session("ListingID")
            End If

            If Not IsNothing(Request.Item("source")) Then
                If Not String.IsNullOrEmpty(Request.Item("source").ToString) Then
                    Company_Source = Request.Item("source").Trim
                End If
            Else
                Company_Source = Session("ListingSource")
            End If

            If Company_ID <> 0 Then
                'First we'd like to display the client preferences special field with our company Data.
                'So this is call #1 to the database. 
                Preferences_Table = aclsData_Temp.Get_Client_Preferences()

                'Alright, so let's create a company class.
                Company_Results = aclsData_Temp.GetCompanyInfo_ID(Company_ID, Company_Source, 0)
                If Not IsNothing(Company_Results) Then
                    If Company_Results.Rows.Count > 0 Then
                        'Build a Company Class and Send back.
                        Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, Company_Source, Preferences_Table)


                        If Company_Source = "CLIENT" Then
                            Other_ID = Company_Data.clicomp_jetnet_comp_id
                        Else
                            aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(Company_ID, "")
                            If Not IsNothing(aTempTable) Then 'not nothing
                                If aTempTable.Rows.Count > 0 Then
                                    Other_ID = aTempTable.Rows(0).Item("comp_id")
                                End If
                            End If
                        End If


                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("company_spec.ascx.vb - aclsData_Temp.GetCompanyInfo_ID(" & Company_ID & ", " & Company_Source & ",0) - " & error_string)
                    End If
                    display_error()
                End If
                Company_Results.Dispose()

                'Alright so now I have my Company Class. We should probably be displaying it.
                Phone_Text = clsGeneral.clsGeneral.Show_Company_Display(Company_Data, True)
                Try
                    aTempTable = aclsData_Temp.GetPhoneNumbers(Company_ID, 0, Company_Source, 0)
                    '' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            ' set it to the datagrid 
                            Company_Phone_Array = clsGeneral.clsGeneral.Create_Array_Phone_Class(aTempTable)

                            For i = 0 To Company_Phone_Array.Count - 1
                                Phone_Text = Phone_Text & clsGeneral.clsGeneral.show_phone_display(Company_Phone_Array(i))
                            Next
                        Else
                            'rows = 0
                            'Phone_Text = ""
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            LogError("Sandbox.ascx.vb - Phone Numbers Company() - " & error_string)
                        End If
                        display_error()
                    End If
                Catch ex As Exception
                    error_string = "Sandbox.ascx.vb - Phone Numbers Company" & ex.Message
                    LogError(error_string)
                End Try
                Company_Information.Text = "<table width='100%' cellpadding='3'><tr><td align='left' valign='top'>" & Phone_Text & "</td></tr></table>"

                aTempTable = aclsData_Temp.GetContacts(Company_ID, Company_Source, "Y", 0)
                ' check the state of the DataTable
                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        Contact_Class_Array = clsGeneral.clsGeneral.Create_Array_Contact_Class(aTempTable)
                    Else
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("aclsData_Temp.GetContacts(" & Company_ID & ", " & Company_Source & ", ""Y"",0) - " & error_string)
                    End If
                    display_error()
                End If

                Contact_Display = "<table width='100%' cellpadding='3'><tr bgcolor='#F2F2F2'>"
                For Each Con As clsClient_Contact In Contact_Class_Array
                    If Count = 2 Then
                        If color = "#E3EDF8" Then
                            color = "#F2F2F2"
                        Else
                            color = "#E3EDF8"
                        End If
                        Contact_Display = Contact_Display & "</tr><tr bgcolor='" & color & "'>"
                        Count = 0
                    End If

                    Count = Count + 1
                    Contact_Display = Contact_Display & "<td align='left' valign='top'>" & clsGeneral.clsGeneral.Show_Contact_Display(Con)

                    Phone_Text = "<br />"
                    Try
                        aTempTable = aclsData_Temp.GetPhoneNumbers(Company_ID, Con.clicontact_id, Company_Source, 0)
                        '' check the state of the DataTable
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                ' set it to the datagrid
                                Company_Phone_Array = New ArrayList
                                Company_Phone_Array = clsGeneral.clsGeneral.Create_Array_Phone_Class(aTempTable)

                                For i = 0 To Company_Phone_Array.Count - 1
                                    Phone_Text = Phone_Text & clsGeneral.clsGeneral.show_phone_display(Company_Phone_Array(i))
                                Next
                            Else
                                'rows = 0
                                Phone_Text = ""
                            End If
                        Else
                            If aclsData_Temp.class_error <> "" Then
                                error_string = aclsData_Temp.class_error
                                LogError("Sandbox.ascx.vb - Phone Numbers Company() - " & error_string)
                            End If
                            display_error()
                        End If
                    Catch ex As Exception
                        error_string = "Sandbox.ascx.vb - Phone Numbers Company" & ex.Message
                        LogError(error_string)
                    End Try

                    Contact_Display = Contact_Display & Phone_Text & "</td>"
                Next
                Contact_Display = Contact_Display & "</table>"
                Contact_Information.Text = Contact_Display

                'This creates the company aircraft tab.
                Company_Aircraft_Tab(Company_ID, Company_Source)

                'This creates company wanted.

                clsGeneral.clsGeneral.Fill_Wanteds_Tab(aclsData_Temp, wanted_label, wanted_dg, Company_ID, Company_Source, Other_ID)


                'Going to create an array of notes for display purposes. First we need the query.  
                'First I need to run a query to get the notes. 
                'This will be the limited query. It only queries for 5 notes.
                aTempTable = aclsData_Temp.DUAL_Notes_LIMIT("COMP", Company_ID, "A", Company_Source, daystr, lnote_order, 5000)

                If aTempTable.Rows.Count > 0 Then
                    Note_Array = clsGeneral.clsGeneral.Create_Note_Array_Class(aTempTable)


                    'For some parameters.. let's set them up.
                    Dim TYPE As String = "note" 'Default Note View
                    Dim LNOTE_STATUS As String = "A"
                    Dim URL_STRING As String = ""
                    Dim CAT_KEY As Integer = 0
                    Dim DEFAULT_WIDTH As Integer = 300
                    Dim UL_CSS_CLASS As String = "notes_list"
                    Dim DIV_CSS_CLASS As String = "notes_list_div"
                    Dim NOTES_STRING As String = ""
                    Dim TYPE_OF_LISTING As Integer = 1
                    Dim USED_ID As Integer = 0
                    Dim USED_SOURCE As String = ""
                    NOTES_STRING = ""
                    Select Case TYPE
                        Case "note"
                            LNOTE_STATUS = "A"
                        Case "action"
                            LNOTE_STATUS = "P"
                            URL_STRING = "&opp=true"
                        Case "opportunity"
                            LNOTE_STATUS = "O"
                            URL_STRING = "action"
                    End Select

                    For Each Note_Data As clsLocal_Notes In Note_Array

                        'Special consideration if the listing is a full notes listing. Meaning the width has to be wider on the note views.
                        If CAT_KEY = 0 Then
                            DEFAULT_WIDTH = 800
                            UL_CSS_CLASS = "notes_list_no_width"
                            DIV_CSS_CLASS = "notes_list_div_main"
                        End If

                        If Note_Data.lnote_notecat_key = CAT_KEY Or CAT_KEY = 0 Then 'If the notes category is equal to the category we're looking at, show the note. 
                            NOTES_STRING = NOTES_STRING & "<div class='" & DIV_CSS_CLASS & "'>"
                            NOTES_STRING = NOTES_STRING & "<b><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=" & TYPE & "&amp;id=" & Note_Data.lnote_id & "','','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"
                            If IsDate(Note_Data.lnote_entry_date) And Note_Data.lnote_status <> "P" Then 'This means it's not an action.
                                NOTES_STRING = NOTES_STRING & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_entry_date)
                                NOTES_STRING = NOTES_STRING & "</a> (<em>Entered by: " & Note_Data.lnote_user_name & ")</em> </b> - "
                            Else
                                If Note_Data.lnote_status <> "P" And Note_Data.lnote_status <> "O" Then 'This means it's an action.
                                    NOTES_STRING = NOTES_STRING & " " & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                                Else
                                    NOTES_STRING = NOTES_STRING & " " & DateAdd("h", Session("timezone_offset"), Note_Data.lnote_schedule_start_date) & "</a></b> - "
                                End If
                            End If

                            'Just displaying the notes text field
                            If Len(Note_Data.lnote_note) > 100 Then
                                NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Left(Note_Data.lnote_note, 100) & "...")
                            Else
                                NOTES_STRING = NOTES_STRING & Server.HtmlEncode(Note_Data.lnote_note)
                            End If

                            If TYPE_OF_LISTING <> 3 Then 'This means that this detailed listing which shows the aircraft information
                                'on the note only shows when the listing type isn't an aircraft.
                                USED_ID = IIf(Note_Data.lnote_jetnet_ac_id <> 0, Note_Data.lnote_jetnet_ac_id, Note_Data.lnote_client_ac_id)
                                USED_SOURCE = IIf(Note_Data.lnote_jetnet_ac_id <> 0, "JETNET", "CLIENT")
                                If USED_ID <> 0 Then
                                    NOTES_STRING = NOTES_STRING & "<span class='blue_color'>" & add_ac_name(USED_ID, 2, USED_SOURCE) & "</span>"
                                End If
                            End If

                        End If

                        NOTES_STRING = NOTES_STRING & "</div>"

                    Next
                    Notes_Information.Text = NOTES_STRING 'Server.HtmlEncode(NOTES_STRING)
                End If
            End If
        Catch ex As Exception
            error_string = "company_spec.aspx.vb - fill_comp_AC() -  " & ex.Message
            LogError(error_string)
        End Try
    End Sub

    'THESE TWO FUNCTIONS ARE TEMPORARY. I AM WRITING A REPLACEMENT IN CLSGENERAL.
    Function add_comp_name(ByVal q As Integer, ByVal show As Integer, ByVal source As String)
        Dim typeoflisting As Integer = 3
        'This adds the company name for notes and action display
        add_comp_name = ""
        If typeoflisting <> 1 Then
            '---------------------------Aircraft Contact Information-----------------------------------------------------
            Try
                Dim strContact As String = ""
                ' get the contact info
                Dim compID As Integer = q
                aTempTable = aclsData_Temp.GetCompanyInfo_ID(compID, source, 0)

                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            If show = 2 Then
                                strContact = " (<em>"
                                If Not (IsDBNull(r("comp_name"))) Then
                                    If r("comp_name") <> "" Then
                                        strContact = strContact & "" & r("comp_name") & " "
                                    End If
                                End If
                                If Not (IsDBNull(r("comp_city"))) Then
                                    If r("comp_city") <> "" Then
                                        strContact = strContact & r("comp_city") & " "
                                    End If
                                End If
                                If Not (IsDBNull(r("comp_state"))) Then
                                    If r("comp_state") <> "" Then
                                        strContact = strContact & r("comp_state") & " "
                                    End If
                                End If
                                If Not (IsDBNull(r("comp_country"))) Then
                                    If r("comp_country") <> "" Then
                                        strContact = strContact & r("comp_country")
                                    End If
                                End If
                                strContact = " - " & strContact & "</em>)"
                            End If
                            add_comp_name = strContact
                        Next
                    Else ' 0 rows
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("main_site.Master.vb - add_comp_name() - " & error_string)
                    End If
                    display_error()
                End If
            Catch ex As Exception
                error_string = "main_site.Master.vb - add_comp_name() - " & ex.Message
                LogError(error_string)
            End Try
        Else
            If show = 2 Then
                add_comp_name = " (<em>" & source & " Company</em>)"
            End If
        End If

    End Function
    Function add_ac_name(ByVal idnum As Integer, ByVal show As Integer, ByVal source As String)
        'This adds the aircraft name for notes and action display
        add_ac_name = ""
        Dim typeoflisting As Integer = 1
        Try
            If source = "JETNET" Then
                If typeoflisting <> 3 Then
                    Dim aircraft_text As String = ""
                    Dim aError As String = ""
                    aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(idnum, aError)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In aTempTable.Rows

                                'check for flags
                                aircraft_text = ""
                                If show = 2 Then
                                    aircraft_text = " (<em>"
                                    If Not IsDBNull(R("ac_year_mfr")) Then
                                        If R("ac_year_mfr") <> "" Then
                                            aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                                        End If
                                    End If
                                    aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & " - "
                                    If Not IsDBNull(R("ac_reg_nbr")) Then
                                        If R("ac_reg_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "Reg #: " & R("ac_reg_nbr") & " - "
                                        End If
                                    End If
                                    add_ac_name = aircraft_text & "</em>)"
                                End If

                                'If show = 1 Then
                                If Not IsDBNull(R("ac_ser_nbr")) Then
                                    If R("ac_ser_nbr") <> "" Then
                                        aircraft_text = aircraft_text & "Ser #:" & R("ac_ser_nbr") & "</em>)"
                                    End If
                                End If
                                'End If
                                add_ac_name = aircraft_text
                            Next
                        Else ' 0 rows
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            LogError("main_site.Master.vb - add_ac_name() - " & error_string)
                        End If
                        display_error()
                    End If
                Else
                    If show = 2 Then
                        add_ac_name = " (<em>" & source & " AC</em>)"
                    End If
                End If
            Else
                If typeoflisting <> 3 Then
                    Dim aircraft_text As String = ""
                    Dim aError As String = ""
                    aTempTable = aclsData_Temp.Get_Clients_Aircraft(idnum)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each R As DataRow In aTempTable.Rows
                                aircraft_text = ""
                                If show = 2 Then
                                    aircraft_text = " (<em>"
                                    If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                                        If R("cliaircraft_year_mfr") <> "" Then
                                            aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & " "
                                        End If
                                    End If
                                    If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                                        If R("cliaircraft_reg_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "  "
                                        End If
                                    End If
                                End If

                                If show = 1 Then
                                    If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                                        If R("cliaircraft_ser_nbr") <> "" Then
                                            aircraft_text = aircraft_text & "Ser #:" & R("cliaircraft_ser_nbr") & "</em>)"
                                        End If
                                    End If
                                End If
                                add_ac_name = aircraft_text
                            Next
                        Else ' 0 rows
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            LogError("main_site.Master.vb - add_ac_name() - " & error_string)
                        End If
                        display_error()
                    End If
                Else
                    If show = 2 Then
                        add_ac_name = " (<em>" & source & " AC</em>)"
                    End If
                End If
            End If
        Catch ex As Exception
            error_string = "main_site.Master.vb - add_ac_name() - " & ex.Message
            LogError(error_string)
        End Try
    End Function
End Class