Imports System.IO
Partial Public Class Company_Tabs
  Inherits System.Web.UI.UserControl
  Public aTempTable, aTempTable2 As New DataTable
  Public Event Notes(ByVal text As String, ByVal cat_name As String, ByVal main_id As Integer, ByVal cat_id As Integer, ByVal action As Boolean, ByVal label As Label, ByVal Notes_Data As DataTable)
  Dim error_string As String = ""
  Dim Notes_Data As DataTable
  Dim Action_Data As DataTable
  Dim Document_Data As DataTable
  Dim Opp_Data As DataTable
  Dim Prospect_Data As DataTable
  Dim ShowAll As Boolean = False
#Region "Page Events"

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'Session.Item("transaction_table") = Nothing
    If Me.Visible = True Then
      If Session.Item("crmUserLogon") = True Then
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try

          If Session.Item("localSubscription").crmDocumentsFlag = True Then
            opportunities_tab.Visible = True
          Else
            opportunities_tab.Visible = False
          End If
          If Session.Item("localUser").crmEvo = True Then 'If an EVO user
            notes_tab.Visible = False
            opp_tab.Visible = False
            action_tab.Visible = False
            opportunities_tab.Visible = False
            job_tab.Visible = False
          Else

          End If

          Select Case masterPage.ListingSource
            Case "JETNET"
              add_wanted.Visible = False
            Case Else

              'certification_tab.Visible = False
          End Select

          '---------------------------------------------End Database Connection Stuff---------------------------------------------
          'Grabbing the Tabs Data from View State so we don't have to query the database except once. This stuff gets
          'Pulled from Company Card control. 
          If Not IsNothing(ViewState("Notes_Data")) Then
            Notes_Data = DirectCast(ViewState("Notes_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Action_Data")) Then
            Action_Data = DirectCast(ViewState("Action_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Document_Data")) Then
            Document_Data = DirectCast(ViewState("Document_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Opp_Data")) Then
            Opp_Data = DirectCast(ViewState("Opp_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Prospect_Data")) Then
            Prospect_Data = DirectCast(ViewState("Prospect_Data"), DataTable)
          End If

          'An Event to Fill The Notes, Email, Action, Documents. Datatable and control it's supposed to be filling
          'Is sent to the control. 
          If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
            Dim data As New DataTable

            'Only load data if it exists, otherwise default to new/empty datatable.
            If Not IsNothing(Notes_Data) Then
              data = Notes_Data.Clone
            End If

            Dim startCount As Integer = 0
            Dim endCount As Integer = 10

            If Not IsNothing(Trim(Request("startCount"))) Then
              If IsNumeric(Trim(Request("startCount"))) Then
                startCount = Trim(Request("startCount"))
                endCount = startCount + 10
              End If
            End If

            If startCount = 0 Then 'We're only checking on the session item existing if there's no request variable passed.
              If Not IsNothing(Trim(Session.Item("startCount"))) Then 'We check existence of session item.
                If IsNumeric(Trim(Session.Item("startCount"))) Then 'Check for numeric
                  If Session.Item("startCount") > 0 Then 'Then make sure it's greater than 0.
                    startCount = Session.Item("startCount") 'We set the start count to the session item that's set on the notes control.
                    endCount = startCount + 10 'have to set an end count of + 10
                    'Important note: On the company tab/aircraft tab pages, we will not clear this session variable.
                    'That's because we need it on the details.aspx page and that loads after. 
                    'We will clear it on that.
                  End If
                End If
              End If
            End If

            Try
              If Not IsNothing(Notes_Data) Then

                Notes_Data = clsGeneral.clsGeneral.AddNextPreviousToNotesTable(Notes_Data)

                If Notes_Data.Rows.Count > 0 Then
                  data = clsGeneral.clsGeneral.limit_rows(Notes_Data, startCount, endCount)
                End If

                RaiseEvent Notes("", "NOTES", masterPage.ListingID, 0, False, notes_list, data)
                RaiseEvent Notes("", "ACTION", masterPage.ListingID, 0, True, action_label, Action_Data)
                RaiseEvent Notes("", "DOCUMENTS", masterPage.ListingID, 0, False, document_label, Document_Data)
                RaiseEvent Notes("", "OPPORTUNITIES", masterPage.ListingID, 0, False, opp_list, Opp_Data)
                RaiseEvent Notes("", "PROSPECT", masterPage.ListingID, 0, False, prospect_list, Prospect_Data)
              End If
            Catch ex As Exception
              error_string = "Company_Tabs.ascx.vb - Page Load - Notes Loading " & ex.Message & " C ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
              masterPage.LogError(error_string)
            End Try

            'Fill up the Tabs Function. 


            'Setting the Job Tab to be invisible if not Jet Advisors. 
            If Application.Item("crmClientSiteData").crmClientHostName <> "WWW.JETADVISORSCRM.COM" Or Application.Item("crmClientSiteData").crmClientHostName <> "JETADVISORSCRM.COM" Then
              job_tab.Visible = False
            Else
              If Session.Item("IsJob") = False Then
                job_tab.Visible = False
              Else
                job_tab.Visible = True
              End If
            End If
          End If

          Try 'This is only an attempt to set the active tab index. 
            If Not Page.IsPostBack Then 'meaning this only happens on a real page refresh and not on a tab post back
              If Not IsNothing(Session.Item("company_active_tab")) Then
                tabs_container.ActiveTabIndex = Session.Item("company_active_tab")
              End If
            End If
          Catch
            'just an attempt
          End Try

          If Not IsNothing(Trim(Request("trans"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("trans"))) Then
              If Not Page.IsPostBack Then
                tabs_container.ActiveTabIndex = 1
              End If
              If Trim(Request("trans")) = "all" Then
                ShowAll = True
              End If
            End If
          End If

          Try
            If Not Page.IsPostBack Then
              tabs_container_ActiveTabChanged(tabs_container, e)
            End If
          Catch ex As Exception
            error_string = "Company_Tabs.ascx.vb - Page Load() Error in TabsContainerChange() " & ex.Message & " C ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
            masterPage.LogError(error_string)
          End Try

          'Dim _selected As HttpCookie = Request.Cookies("ppkcookie")

          'If Not IsNothing(_selected) Then
          '    Try
          '        tabs_container.ActiveTab.ID = _selected.Value
          '    Catch

          '    End Try
          'End If

          Try 'This is only an attempt to set the active tab index. 
            If IsNumeric(Trim(Request("startCount"))) Then
              tabs_container.ActiveTabIndex = 3
            End If
          Catch

          End Try

        

        Catch ex As Exception
          error_string = "Company_Tabs.ascx.vb - Page Load() " & ex.Message & " C ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub

#End Region
#Region "Job Tab"
  Function fill_job_tab(ByVal x As String) As String
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    'This fills up the job tab with information on the contact/job page.
    Dim color As String = ""
    fill_job_tab = ""
    '--------------------------------Resume Information----------------------------------------------------------
    Try
      Dim resume_text As String = "<table width='90%' cellpadding='2' cellspacing='0'>"
      Dim aError As String = ""
      ' get the avionics

      Dim ac_experience As String = ""
      aTempTable = masterPage.aclsData_Temp.GetClient_JobSeeker_contactID(x)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          ac_experience = ac_experience & "<tr class='alt_row'><td colspan='2' valign='top' align='left'><hr /><b>Experience:</b></td></tr>"
          For Each r As DataRow In aTempTable.Rows
            ac_experience = ac_experience & "<tr class='alt_row'><td align='left' valign='top' colspan='2'>"
            ac_experience = ac_experience & IIf(Not IsDBNull(r("jobsind_model_name")), r("jobsind_model_name") & " ", "") & " - "
            ac_experience = ac_experience & IIf(Not IsDBNull(r("jobsind_model_experience")), r("jobsind_model_experience") & " ", "")
            If Not IsDBNull(r("jobsind_model_experience_type")) Then
              If r("jobsind_model_experience_type") = "H" Then
                ac_experience = ac_experience & " " & "Hours"
              Else
                ac_experience = ac_experience & "Years"
              End If
            End If
            ac_experience = ac_experience & " Exp.</td></tr>"
          Next
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("Company_Tabs.ascx.vb - Fill_Job_Tab() - " & error_string)
        End If
        masterPage.display_error()
      End If

      aTempTable = masterPage.aclsData_Temp.GetClient_JobSeeker_Details(x, aError)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If color = "alt_row" Then
              color = ""
            Else
              color = "alt_row"
            End If
            resume_text = resume_text & "<tr class='" & color & "'><td align='left' valign='top' width='250'><b>General Information:</b></td>"
            resume_text = resume_text & "<td align='left' valign='top'>" & IIf(Not IsDBNull(r("jobseek_general")), r("jobseek_general") & " ", "") & "</td>"
            resume_text = resume_text & "<tr class='" & color & "'><td align='left' valign='top' width='250'><b>Special Information:</b></td>"
            resume_text = resume_text & "<td align='left' valign='top'>" & IIf(Not IsDBNull(r("jobseek_special")), r("jobseek_special") & " ", "") & "</td>"
            resume_text = resume_text & "<tr class='" & color & "'><td align='left' valign='top' width='250'><b>Employment Information:</b></td>"
            resume_text = resume_text & "<td align='left' valign='top'>" & IIf(Not IsDBNull(r("jobseek_employment")), r("jobseek_employment") & " ", "") & "</td>"
            resume_text = resume_text & "<tr class='" & color & "'><td align='left' valign='top' width='250'><b>Education Information:</b></td>"
            resume_text = resume_text & "<td align='left' valign='top'>" & IIf(Not IsDBNull(r("jobseek_education")), r("jobseek_education") & " ", "") & "</td></tr>"
          Next
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("Company_Tabs.ascx.vb - Fill_Job_Tab() - " & error_string)
        End If
        masterPage.display_error()
      End If
      resume_text = resume_text & ac_experience
      resume_label.Text = resume_text & "</table>"

    Catch ex As Exception
      error_string = "Company_Tabs.ascx.vb - Fill_Job_Tab() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Function
#End Region
#Region "Company Transactions"
  Public Sub transaction(ByVal idnum As Integer, ByVal source As String)

    Dim tbl As New Table
    Dim jetnet_id As Integer = 0
    Dim client_id As Integer = 0
    Dim jetnet_id_transaction As Integer = 0
    Dim client_id_transaction As Integer = 0
    Dim trans_text As String = ""
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    If source = "CLIENT" Then 'this will ensure that if a client company has a jetnet side that the transactions will show
      If masterPage.OtherID <> 0 Then
        idnum = masterPage.OtherID
        source = "JETNET"
      End If
    End If

  

    Select Case masterPage.ListingSource
      Case "JETNET"
        jetnet_id_transaction = masterPage.ListingID
        client_id = masterPage.OtherID
        client_id_transaction = masterPage.OtherID
      Case "CLIENT"
        jetnet_id_transaction = masterPage.OtherID
        jetnet_id = masterPage.OtherID
        client_id_transaction = masterPage.ListingID
        client_id = masterPage.ListingID
    End Select

    Try
      masterPage.PerformDatabaseAction = True
      If masterPage.ListingSource = "JETNET" Then
        '--------------Transaction Listings-----------------------------------------------------------------------------------
        If Not IsDate(trans_tab_time.Text) Then
          clsGeneral.clsGeneral.Build_Transaction_Tab_Company(masterPage.ListingID, masterPage.OtherID, masterPage.OtherID, masterPage.ListingID, masterPage.ListingSource, Nothing, masterPage, "both", trans_label_table_text, Nothing, ShowAll)
          trans_tab_time.Text = Now()
        End If

      ElseIf masterPage.ListingSource = "CLIENT" Then
        '--------------Transaction Listings-----------------------------------------------------------------------------------

        If Not IsDate(trans_tab_time.Text) Then
          clsGeneral.clsGeneral.Build_Transaction_Tab_Company(masterPage.OtherID, masterPage.ListingID, masterPage.OtherID, masterPage.ListingID, masterPage.ListingSource, Nothing, masterPage, "both", trans_label_table_text, Nothing, ShowAll)
          trans_tab_time.Text = Now()
        End If
      Else
        trans_warning_text.Text = "<p align='center'>No Current Transactions for this Company.</p>"
        trans_text = "<table width='100%' cellpadding='3' cellspacing='0'class='engine'>"
      End If

      RaiseEvent Notes(trans_text, "TRANSACTIONS", idnum, masterPage.what_cat(0, "TRANSACTIONS", True), False, trans_label, Notes_Data)
      masterPage.PerformDatabaseAction = False
    Catch ex As Exception
      error_string = "Company_Tabs.ascx.vb - Transaction() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub transaction_gv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles transaction_gv.PageIndexChanged
    'controls paging for the company transaction datagrid
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      If Not IsNothing(Session.Item("transaction_table")) Then
        transaction_gv.CurrentPageIndex = e.NewPageIndex
        transaction_gv.DataSource = Session.Item("transaction_table")
        transaction_gv.DataBind()
        tabs_container.ActiveTab = transaction_tab
      End If
    Catch ex As Exception
      error_string = "Company_Tabs.ascx.vb - Transaction_gv_PageIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Company AC Tab"
  Sub Company_Aircraft_Tab(ByVal idnum As Integer, ByVal source As String)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim tbl As New Table
      Dim lbl As New Label

      '-------------------Aircraft Tab Listing-------------------------------------------------------------------------------------------

      If Not IsNothing(ViewState("Aircraft_List")) Then
        aTempTable = DirectCast(ViewState("Aircraft_List"), DataTable)
      Else
        If source = "CLIENT" Then
          aTempTable = masterPage.aclsData_Temp.Get_Client_JETNET_AC(idnum, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
          'this is just a small and simple catch.
          'if this temptable has zero rows.. check and see if there's an other id.
          'If there is, try to get the jetnet ac.
          If aTempTable.Rows.Count = 0 Then
            If masterPage.OtherID <> 0 Then
              aTempTable = masterPage.aclsData_Temp.GetAircraft_Listing_compid(masterPage.OtherID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
            End If
          End If
        Else
          aTempTable = masterPage.aclsData_Temp.GetAircraft_Listing_compid(idnum, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
        End If
      End If

      ViewState("Aircraft_List") = aTempTable

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count = 0 Then
          aircraft_label.Controls.Clear()
          lbl = New Label
          lbl.Text = "<p align='center'>No aircraft associated with this company</p>"
          'tabs_container.ActiveTabIndex = 4
          lbl.ForeColor = Drawing.Color.Red
          lbl.Font.Bold = True
          aircraft.Controls.Add(lbl)
          lbl.Dispose()
        End If

        If aTempTable.Rows.Count > 0 Then
          If Not IsNumeric(Trim(Request("startCount"))) Then
            tabs_container.ActiveTabIndex = 0
          Else
            tabs_container.ActiveTabIndex = 4
          End If

          tbl = clsGeneral.clsGeneral.Build_Company_Aircraft_Tab(aTempTable, True)
          aircraft_label.Controls.Clear()


          tbl.ID = "ac_listing"
          aircraft_label.Controls.Add(tbl)
          aircraft_label.EnableViewState = True
          tbl.Dispose()
          aTempTable = Nothing

          If Not IsNothing(aircraft_label.FindControl("ac_listing")) Then
            Dim stringwrite As System.IO.StringWriter = New System.IO.StringWriter
            Dim htmlwrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringwrite)
            Dim mycontrol As Control = Parent.FindControl("companyCard")
            If Not IsNothing(mycontrol) Then
              Dim info As Label = mycontrol.FindControl("comp_name")
              Dim space As New Label
              space.Text = "<br /><br />"
              If Not IsNothing(info) Then
                info.RenderControl(htmlwrite)
                space.RenderControl(htmlwrite)
              End If
              info = mycontrol.FindControl("contact_info")
              If Not IsNothing(info) Then
                info.RenderControl(htmlwrite)
              End If
              info = mycontrol.FindControl("contact_right")
              If Not IsNothing(info) Then
                info.RenderControl(htmlwrite)

              End If
              space.RenderControl(htmlwrite)
            End If

            aircraft_label.FindControl("ac_listing").RenderControl(htmlwrite)
            Session("export_info") = "<p align='center'><b style='font-size:19px;'>Company Aircraft List</b></p>" & stringwrite.ToString()
          End If

        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("company_tabs.ascx.vb  - Company_Aircraft_Tab(" & masterPage.ListingID & ") - " & error_string)
          End If
          masterPage.display_error()
        End If
      Else
        aircraft_label.Controls.Clear()
        lbl = New Label
        lbl.Text = "<p align='center'>No aircraft associated with this company</p>"
        'tabs_container.ActiveTabIndex = 4
        lbl.ForeColor = Drawing.Color.Red
        lbl.Font.Bold = True
        aircraft_label.Controls.Add(lbl)
        lbl.Dispose()
      End If
    Catch ex As Exception
      error_string = "company_tabs.ascx.vb  - Company_Aircraft_Tab(" & idnum & ") - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

#End Region

#Region "Fill_Parent_Company_Info"
  Public Sub Fill_Parent(ByVal company_ID As Integer, ByVal Company_Source As String, ByVal masterPage As main_site)
    Try
      If Company_Source = "JETNET" Then 'If an EVO user
        aTempTable2 = masterPage.aclsData_Temp.Get_Company_Relationships(company_ID, 0)
        If Not (IsNothing(aTempTable2)) Then
          If aTempTable2.Rows.Count > 0 Then
            Dim return_string As String = "<table width='100%' cellpadding='4' cellspacing='0' border='0'><tr class='aircraft_list'><td align='left' valign='top'>Relationship</td><td align='left' valign='top'>Company</td><td align='left' valign='top'>Contact</td></tr>"
            Dim tempComp As New clsClient_Company
            Dim tempData As New DataTable
            Dim tempContact As New DataTable
            Dim bgcolor As String = "#ffffff"
            For Each q As DataRow In aTempTable2.Rows

              tempContact = New DataTable
              Dim Contact_Class_Array As New ArrayList
              Dim company_one As Integer = 0
              Dim contact_one As Integer = 0
              Dim contact_display As String = ""
              Dim company_two As Integer = 0
              Dim contact_two As Integer = 0
              If bgcolor = "" Then
                bgcolor = "alt_row"
              Else
                bgcolor = ""
              End If
              return_string = return_string & "<tr class='" & bgcolor & "'>"
              company_one = IIf(Not IsDBNull(q("compref_rel_comp_id")), q("compref_rel_comp_id"), 0)
              contact_one = IIf(Not IsDBNull(q("compref_rel_contact_id")), q("compref_rel_contact_id"), 0)

              company_two = IIf(Not IsDBNull(q("compref_comp_id")), q("compref_comp_id"), 0)
              If company_one = company_ID Then
                contact_one = IIf(Not IsDBNull(q("compref_contact_id")), q("compref_contact_id"), 0)
              End If

              'Get the contact information
              If contact_one <> 0 Then
                tempContact = masterPage.aclsData_Temp.GetContacts_Details(contact_one, "JETNET")
                If Not IsNothing(tempContact) Then
                  If tempContact.Rows.Count > 0 Then
                    Contact_Class_Array = clsGeneral.clsGeneral.Create_Array_Contact_Class(tempContact)
                    For Each Con As clsClient_Contact In Contact_Class_Array
                      contact_display = clsGeneral.clsGeneral.Show_Contact_Display(Con)
                    Next
                  End If
                Else
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("company tabs -masterPage.aclsData_Temp.GetContacts_Details(" & contact_one & ", JETNET"") - " & error_string)
                  End If
                  masterPage.display_error()
                End If
              End If


              If company_one <> company_ID Then
                tempData = masterPage.aclsData_Temp.GetCompanyInfo_ID(company_one, "JETNET", 0)
                tempComp = clsGeneral.clsGeneral.Create_Company_Class(tempData, "JETNET", Nothing)
                return_string = return_string & "<td align='left' valign='top'><b>" & q("actype_compref_name2") & "</b></td><td align='left' valign='top'><b><a href='details.aspx?comp_ID=" & company_one & "&source=JETNET&type=1'>" & tempComp.clicomp_name & "</a></b><br />" & clsGeneral.clsGeneral.Show_Company_Display(tempComp, False) & "</td><td align='left' valign='top'>" & contact_display & "</td>"
              Else
                tempData = masterPage.aclsData_Temp.GetCompanyInfo_ID(company_two, "JETNET", 0)
                tempComp = clsGeneral.clsGeneral.Create_Company_Class(tempData, "JETNET", Nothing)
                return_string = return_string & "<td align='left' valign='top'><b>" & q("actype_name") & "</b></td><td align='left' valign='top'><b><a href='details.aspx?comp_ID=" & company_two & "&source=JETNET&type=1'>" & tempComp.clicomp_name & "</a></b><br />" & clsGeneral.clsGeneral.Show_Company_Display(tempComp, False) & "</td><td align='left' valign='top'>" & contact_display & "</td>"
              End If
              return_string = return_string & "</tr>"

            Next
            relationship_text.Text = return_string & "</tr></table>"
          Else
            relationship_warning.Text = "<p align='center'>No Current Relationships for this Company.</p>"
          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("company_tabs.ascx.vb  -Fill_Parent(" & masterPage.ListingID & ") - " & error_string)
          End If
          masterPage.display_error()
        End If

      ElseIf Company_Source = "CLIENT" Then

        Dim contact_text As String = "<a href='#' onClick=""javascript:load('edit.aspx?type=company&main_location=true&comp_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1100,resizable=yes,toolbar=no,location=no,status=no');"">Identify Main Location</a>"
        Dim contact_phone As String = ""

        If masterPage.MainLocID <> 0 Then
          '------Phone Company Information Left Card Display----------------------------------------------------------------------
          contact_text = "<tr><td align='left' valign='top'><h4>Main Location</h4>"
          contact_text = contact_text & Related_Info(masterPage.MainLocID, "CLIENT", masterPage) & "</td>"


          contact_text = contact_text & "<td align='left' valign='top'><h4>Phone Numbers</h4>"
          contact_text = contact_text & Related_Phones(masterPage.MainLocID, "CLIENT", masterPage) & "</td></tr>"
        End If

        If masterPage.MainLocID = 0 Then
          contact_text = ""
        End If

        aTempTable2 = masterPage.aclsData_Temp.GetCompanyMainLoc_ID(masterPage.ListingID)

        If aTempTable2.Rows.Count > 0 Then
          If masterPage.MainLocID <> 0 Then
            contact_text = contact_text & "<tr><td align='left' valign='top' colspan='2'><hr class='light_divider' /></td></tr>"
          End If
          contact_text = contact_text & "<tr><td align='left' valign='top' colspan='2'><h4>Additional Locations</h4></td></tr>"
        Else
          If masterPage.MainLocID = 0 Then
            relationship_warning.Text = "<p align='center'>No Current Relationships for this Company.</p>"
          End If
        End If
        For Each r As DataRow In aTempTable2.Rows
          contact_text = contact_text & "<tr><td align='left' valign='top'>"
          contact_text = contact_text & Related_Info(r("comp_id"), "CLIENT", masterPage)
          contact_text = contact_text & "</td>"

          contact_text = contact_text & "<td align='left' valign='top'><h4>Phone Numbers</h4>"
          contact_text = contact_text & Related_Phones(r("comp_id"), "CLIENT", masterPage)
          contact_text = contact_text & "</td></tr>"
        Next


        ''''''''''''''''''''''


        contact_text = "<table width='100%' cellspacing='0' cellpadding='3'>" & contact_text & "</table>"
        relationship_text.Text = contact_text



        '''''''''''''''''''''''''''''
      End If
    Catch ex As Exception
      error_string = "CompanyTabs.ascx.vb - Phone Numbers Array Company" & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region

#Region "Fill Certifications"
  Public Sub Fill_Certifications(ByVal companyID As Integer)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim certification_text As String = ""
    Dim count As Integer = 0
    Try
      aTempTable = masterPage.aclsData_Temp.Return_Certifications(companyID, 0)
      If Not IsNothing(aTempTable) Then
        If (aTempTable.Rows.Count > 0) Then
          certification_text = "<table cellpadding='3' cellspacing='0'><tr>"

          For Each q As DataRow In aTempTable.Rows
            If count = 3 Then
              certification_text = certification_text & "</tr><tr>"
              count = 0
            End If
            certification_text = certification_text & "<td align='center' valign='top' width='150'><img src=""" & HttpContext.Current.Session.Item("jetnetFullHostName").ToString + Session.Item("ImagesVirtualPath").ToString + Constants.cSingleForwardSlash + q("ccerttype_logo_image") & """ alt=""" & q("ccerttype_type") & """/>" & q("ccerttype_type") & "</td>"
          Next
          certification_text = certification_text & "</tr></table>"
          cert_text.Text = certification_text
        Else
          cert_text.Text = "<p align='center'>No Current Operating Certification(s) for this Company.</p>"
          cert_text.ForeColor = Drawing.Color.Red
          cert_text.Font.Bold = True
          ' certification_tab.Visible = False
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("CompanyTabs.ascx.vb -Fill_Certifications() - " & error_string)
        End If
        masterPage.display_error()
      End If
    Catch ex As Exception
      error_string = "CompanyTabs.ascx.vb - Fill_Certifications() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Fill Wanted"
  Public Sub Fill_Wanted(ByVal companyID As Integer, ByVal companySource As String)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    If masterPage.OtherID = 0 And masterPage.ListingSource = "JETNET" Then
      add_wanted.Text = "<p align=""left"">&nbsp;<a href=""#"" onclick=""javascript:create_comp_wanted('edit_note.aspx?action=new&amp;type=wanted&amp;cat_key=23','edit.aspx?type=company&auto=true&note_type=wanted&comp_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "');"">Add Wanted</a>&nbsp;</p>"
    Else
      add_wanted.Text = "<p align=""left"">&nbsp;<a href=""#"" onclick=""javascript:load('edit_note.aspx?type=wanted&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"">Add Wanted</a>&nbsp;</p>"
    End If

    clsGeneral.clsGeneral.Fill_Wanteds_Tab(masterPage.aclsData_Temp, wanted_label, wanted_dg, companyID, companySource, masterPage.OtherID)

  End Sub
#End Region
#Region "Functions That Grab The Notes/Actions/Emails/Documents Data from Details.aspx"
  Public Sub Consume_Action_Data(ByVal Action_Table As DataTable)
    ViewState("Action_Data") = Action_Table
    Notes_Data = DirectCast(ViewState("Action_Data"), DataTable)
  End Sub
  Public Sub Consume_Opp_Data(ByVal Email_Table As DataTable)
    ViewState("Opp_Data") = Email_Table
    Notes_Data = DirectCast(ViewState("Opp_Data"), DataTable)
  End Sub
  Public Sub Consume_Notes_Data(ByVal Notes_Table As DataTable)
    ViewState("Notes_Data") = Notes_Table
    Notes_Data = DirectCast(ViewState("Notes_Data"), DataTable)
  End Sub
  Public Sub Consume_Document_Data(ByVal Document_Table As DataTable)
    ViewState("Document_Data") = Document_Table
    Document_Data = DirectCast(ViewState("Document_Data"), DataTable)
  End Sub
  Public Sub Consume_Prospect_Data(ByVal Prospect_Table As DataTable)
    ViewState("Prospect_Data") = Prospect_Table
    Prospect_Data = DirectCast(ViewState("Prospect_Data"), DataTable)
  End Sub
  Private Function Related_Info(ByVal company_id As Integer, ByVal company_source As String, ByVal masterpage As main_site) As String
    Dim jetnet_comp_id As Integer = 0
    Dim Company_Results As New DataTable
    Dim Preferences_Table As New DataTable
    Dim contact_text As String = ""
    Dim Company_Phone_Array As New ArrayList

    'First we'd like to display the client preferences special field with our company Data.
    'So this is call #1 to the database. 
    Preferences_Table = Nothing
    Company_Results = masterpage.aclsData_Temp.GetCompanyInfo_ID(company_id, company_source, 0)
    ' check the state of the DataTable
    If Not IsNothing(Company_Results) Then
      If Company_Results.Rows.Count > 0 Then
        For Each R As DataRow In Company_Results.Rows
          'Sets the variables for the company display
          If company_source = "CLIENT" Then
            jetnet_comp_id = IIf(Not IsDBNull(R("jetnet_comp_id")), R("jetnet_comp_id"), 0)
          End If
          Dim Company_Data As New clsClient_Company
          Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, company_source, Preferences_Table)
          'Builds the company Display
          contact_text = "<a href='details.aspx?comp_ID=" & R("comp_id") & "&type=1&source=CLIENT'>" & Company_Data.clicomp_name & "</a><br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)
        Next
      End If
    Else 'This means that the datalayer errored. Display the Error. 
      If masterpage.aclsData_Temp.class_error <> "" Then
        error_string = masterpage.aclsData_Temp.class_error
        masterpage.LogError("CompanyTabs.ascx.vb - Fill_Company_Info() - " & error_string)
      End If
      masterpage.display_error()
    End If
    Return contact_text
  End Function
  Private Function Related_Phones(ByVal company_id As Integer, ByVal company_source As String, ByVal masterpage As main_site) As String
    Dim contact_text As String = ""
    Try
      Dim jetnet_comp_id As Integer = 0
      Dim Company_Results As New DataTable
      Dim Preferences_Table As New DataTable
      Dim Company_Phone_Array As New ArrayList

      aTempTable = masterpage.aclsData_Temp.GetPhoneNumbers(company_id, 0, company_source, 0)
      '' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          ' set it to the datagrid 
          Company_Phone_Array = clsGeneral.clsGeneral.Create_Array_Phone_Class(aTempTable)
          For i = 0 To Company_Phone_Array.Count - 1
            contact_text = contact_text & clsGeneral.clsGeneral.show_phone_display(Company_Phone_Array(i))
          Next
        End If
      Else 'This means that the datalayer errored. Display the Error. 
        If masterpage.aclsData_Temp.class_error <> "" Then
          error_string = masterpage.aclsData_Temp.class_error
          masterpage.LogError("CompanyTabs.ascx.vb - Related_Phones() - " & error_string)
        End If
        masterpage.display_error()
      End If
    Catch ex As Exception
      error_string = "CompanyTabs.ascx.vb - Related_Phones" & ex.Message
      masterpage.LogError(error_string)
    End Try
    Return contact_text
  End Function
#End Region
#Region "Function That Fills Tabs"
  Private Sub tabs_container_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabs_container.ActiveTabChanged
    '  If Not Page.IsPostBack Then
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim id As Integer = 0
    Select Case masterPage.ListingSource
      Case "JETNET"
        id = masterPage.ListingID
      Case "CLIENT"
        id = masterPage.ListingID
    End Select
    If Not IsDate(aircraft_tab_time.Text) Then
      aircraft_warning_text.Text = "<p align='center'>Please wait while the aircraft information loads.</p>"
    End If
    If Not IsDate(cert_tab_time.Text) Then
      cert_warning_text.Text = "<p align='center'>Please wait while the certification information loads.</p>"
    End If
    If Not IsDate(trans_tab_time.Text) Then
      trans_warning_text.Text = "<p align='center'>Please wait while the transaction information loads.</p>"
    End If
    If Not IsDate(wanted_tab_time.Text) Then
      wanted_warning_text.Text = "<p align='center'>Please wait while the wanted(s) information loads.</p>"
    End If
    If Not IsDate(rel_tab_time.Text) Then
      rel_warning_text.Text = "<p align='center'>Please wait while the relationship information loads.</p>"
    End If
    If Not IsDate(job_tab_time.Text) Then
      job_warning_text.Text = "<p align='center'>Please wait while the job information loads.</p>"
    End If

    aircraft_warning_text.Text = ""

    Select Case tabs_container.ActiveTab.ID
      Case "aircraft_tab"
        'Fill the aircraft tab
        ' If Not IsDate(aircraft_tab_time.Text) Then
        'doesn't matter for this one, query saved in vs
        Company_Aircraft_Tab(id, masterPage.ListingSource)
        'aircraft_tab_time.Text = Now()
        'End If

      Case "wanted_tab"
        'Only fill these if EVO.
        If Not IsDate(wanted_tab_time.Text) Then
          Fill_Wanted(id, masterPage.ListingSource)
          add_wanted.Visible = True

          wanted_tab_time.Text = Now()
        End If
        wanted_warning_text.Text = ""
      Case "certification_tab"
        If Not IsDate(cert_tab_time.Text) Then
          If masterPage.ListingSource = "JETNET" Then 'If an EVO user
            'Fill Certification Tab
            Fill_Certifications(id)
          ElseIf masterPage.OtherID <> 0 Then
            Fill_Certifications(masterPage.OtherID)
          End If
          cert_tab_time.Text = Now()
        End If
        cert_warning_text.Text = ""
      Case "transaction_tab"
        'Fill the transaction tab
        If Not IsDate(trans_tab_time.Text) Then
          transaction(id, masterPage.ListingSource)
          trans_tab_time.Text = Now()
        End If
        trans_warning_text.Text = ""
      Case "relationship_tab"
        'Fill relationship Tab
        If Not IsDate(rel_tab_time.Text) Then
          Fill_Parent(id, masterPage.ListingSource, masterPage)
          rel_tab_time.Text = Now()
        End If
        rel_warning_text.Text = ""
      Case "job_tab"
        'Fill relationship Tab
        If Not IsDate(job_tab_time.Text) Then
          If Application.Item("crmClientSiteData").crmClientHostName = "WWW.JETADVISORSCRM.COM" Or Application.Item("crmClientSiteData").crmClientHostName = "JETADVISORSCRM.COM" Then
            fill_job_tab(masterPage.Listing_ContactID)
          End If
          job_tab_time.Text = Now()
        End If
        job_warning_text.Text = ""
    End Select


    '  End If
  End Sub

#End Region

End Class