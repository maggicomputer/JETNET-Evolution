Imports System.IO
Partial Public Class _companyCard
  Inherits System.Web.UI.UserControl
  Public Event Synch_Date(ByVal Synch_Type As String, ByVal sync_display As Label)
  Public Event Next_Prev_Btn(ByVal Command As String)
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim contact_text As String = ""
  Public Event SetUpDisplay()
  Public Event SetOtherID(ByVal id As Integer)
  Public Event AddOtherIDToPage(ByVal c As ImageButton)
  Public Event ShareNotesDataTable(ByVal t As DataTable)
  Public Event ShareActionDataTable(ByVal t As DataTable)
  Public Event ShareOppDataTable(ByVal t As DataTable)
  Public Event ShareProspectDataTable(ByVal t As DataTable)
  Public Event ShareDocumentDataTable(ByVal t As DataTable)
  Dim error_string As String = ""
  Dim jetnet_comp_id As Integer

#Region "Page Events"
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      If Session.Item("crmUserLogon") = True Then
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try

          If Session.Item("localUser").crmEvo = True Then 'if person is EVO
            edit_company.Visible = False
            email_company.Visible = False
          End If

          ' If Not Page.IsPostBack Then
          If masterPage.TypeOfListing = 1 Then
            Fill_Company_Info(masterPage.ListingID, masterPage.ListingSource, masterPage)
            clsGeneral.clsGeneral.Recent_Cookies("companies", masterPage.ListingID, UCase(masterPage.ListingSource))
            RaiseEvent Synch_Date("Company_Sync", synch_date_comp)
            set_next_prev()
          End If
          'End If
        Catch ex As Exception
          error_string = "CompanyCard.ascx.vb - Page Load() " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub
#End Region
#Region "Fill Company Card Information"
  Public Sub Fill_Company_Info(ByVal company_ID As Integer, ByVal Company_Source As String, ByVal masterPage As main_site)
    Try
      Dim jetnet_comp_id As Integer = 0
      Dim Company_Results As New DataTable
      Dim Preferences_Table As New DataTable
      Dim contact_text As String = ""
      Dim Company_Phone_Array As New ArrayList
      Dim Business_Type As String = ""
      Dim Company_Data As New clsClient_Company
      'First we'd like to display the client preferences special field with our company Data.
      'So this is call #1 to the database. 
      Preferences_Table = Nothing
      If Not IsNothing(ViewState("Company_Info")) Then
        Company_Results = DirectCast(ViewState("Company_Info"), DataTable)
      Else
        Company_Results = masterPage.aclsData_Temp.GetCompanyInfo_ID(company_ID, Company_Source, 0)
      End If

      ViewState("Company_Info") = Company_Results

      ' check the state of the DataTable
      If Not IsNothing(Company_Results) Then
        If Company_Results.Rows.Count > 0 Then
          For Each R As DataRow In Company_Results.Rows
            'Sets the variables for the company display
            If Company_Source = "CLIENT" Then
              jetnet_comp_id = IIf(Not IsDBNull(R("jetnet_comp_id")), R("jetnet_comp_id"), 0)
              masterPage.MainLocID = IIf(Not IsDBNull(R("clicomp_mainloc_comp_id")), R("clicomp_mainloc_comp_id"), 0)
            End If
            Company_Data = New clsClient_Company
            Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, Company_Source, Preferences_Table)
            'Company Name Display
            'comp_name.Text = Company_Data.clicomp_name
            company_info_tab.HeaderText = Company_Data.clicomp_name
            'This is going to have to be done for just EVO based clients.
            'Basically it calls a function to get the business type and then sets it in the class!
            If Company_Source = "JETNET" Then 'If an EVO user only
              Preferences_Table = Nothing
              Preferences_Table = masterPage.aclsData_Temp.Return_Business_Type(company_ID, 0)
              If Not IsNothing(Preferences_Table) Then
                If Preferences_Table.Rows.Count > 0 Then
                  For Each z As DataRow In Preferences_Table.Rows
                    Business_Type = Business_Type & IIf(Not IsDBNull(z("cbus_name")), " " & z("cbus_name") & ",", "")
                  Next
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("CompanyCard.ascx.vb - Return Business Type() - " & error_string)
                End If
                masterPage.display_error()
              End If
              Preferences_Table = Nothing
            End If
            If Business_Type <> "" Then
              Business_Type = Business_Type.TrimEnd(",")
            End If
            Company_Data.clicomp_business_type_name = Business_Type
            full_page.Text = "<a href='#' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & CLng(Session("ListingID")) & "&jid=0&source=" & Session("ListingSource") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" ><img src='images/full_view.jpg' alt='Full Page View' border='0' /></a>"

            'Builds the company Display
            contact_text = clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)
            'Sets the Other ID clicky changey jetnet/client link.
            If Company_Source = "CLIENT" Then
              OtherIDSetting(jetnet_comp_id, Company_Source)
            Else
              OtherIDSetting(company_ID, Company_Source)
              masterPage.fill_bar()
            End If
            If Company_Data.clicomp_email_address <> "" Then
              email_company.Text = "<a href='#' onclick=""javascript:load('edit_note.aspx?action=new&type=email&comp_ID=" & company_ID & "&source=" & Company_Source & "&cat_key=0','','scrollbars=yes,menubar=no,height=805,width=860,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/mail_compose.png' alt='Email Company' width='24' border='0'/></a>"
            End If
          Next
        End If
      Else 'This means that the datalayer errored. Display the Error. 
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("CompanyCard.ascx.vb - Fill_Company_Info() - " & error_string)
        End If
        masterPage.display_error()
      End If
      contact_info.Text = contact_text
      '------Phone Company Information Left Card Display----------------------------------------------------------------------
      contact_text = "<h4>Phone Numbers</h4>"
      Try
        If Not IsNothing(ViewState("Phone_Info")) Then
          aTempTable = DirectCast(ViewState("Phone_Info"), DataTable)
        Else
          aTempTable = masterPage.aclsData_Temp.GetPhoneNumbers(company_ID, 0, Company_Source, 0)
        End If

        ViewState("Phone_Info") = aTempTable

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
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("CompanyCard.ascx.vb - Phone Numbers Array Company() - " & error_string)
          End If
          masterPage.display_error()
        End If
      Catch ex As Exception
        error_string = "CompanyCard.ascx.vb - Phone Numbers Array Company" & ex.Message
        masterPage.LogError(error_string)
      End Try
      contact_right.Text = contact_text & "<br />" & IIf(Company_Data.clicomp_business_type_name <> "", "<h4>Business Type(s): </h4><em>" & Company_Data.clicomp_business_type_name & "</em>", "")
      'Sharing Note Data
      contact_info.EnableViewState = True
      contact_right.EnableViewState = True
      aTempTable = New DataTable


      Dim Notes_Hold As New DataTable
      Dim Notes_Search As New DataTable
      If Not Page.IsPostBack Then
        If company_ID <> 0 Then
          If masterPage.ListingSource = "CLIENT" Then 'If notes are a client Aircraft
            Notes_Hold = masterPage.aclsData_Temp.Dual_NotesOnlyOne(company_ID, 0, "", False, True) 'Datahook for client/note aircraft
          Else
            Notes_Hold = masterPage.aclsData_Temp.Dual_NotesOnlyOne(0, company_ID, "", False, True) 'Datahook for jetnet/note aircraft
          End If
        End If


        If Session.Item("localUser").crmEvo <> True Then
          If company_ID <> 0 Then
            aTempTable = Notes_Hold.Clone
            Notes_Search = Notes_Hold
            Dim afiltered_Client As DataRow() = Notes_Search.Select("lnote_status in ('A','E')", "")
            ' extract and import
            For Each atmpDataRow_Client In afiltered_Client
              aTempTable.ImportRow(atmpDataRow_Client)
            Next
          End If
        End If
        'share datatable
        'Disposing Data Table
        RaiseEvent ShareNotesDataTable(aTempTable)
        aTempTable.Dispose()

        'Sharing Opp Data
        aTempTable = New DataTable
        If Session.Item("localUser").crmEvo <> True Then
          If company_ID <> 0 Then
            aTempTable = Notes_Hold.Clone
            Notes_Search = Notes_Hold
            Dim afiltered_Client As DataRow() = Notes_Search.Select("lnote_status = 'O'", "")
            ' extract and import
            For Each atmpDataRow_Client In afiltered_Client
              aTempTable.ImportRow(atmpDataRow_Client)
            Next
          End If
        End If
        'share datatable
        'Disposing Data Table
        RaiseEvent ShareOppDataTable(aTempTable)
        aTempTable.Dispose()


        'Sharing Prospect Table Data
        aTempTable = New DataTable
        If Session.Item("localUser").crmEvo <> True Then
          If company_ID <> 0 Then
            aTempTable = New DataTable
            If masterPage.ListingSource = "JETNET" Then
              aTempTable = masterPage.aclsData_Temp.ChangeProspectNotesByParameters(0, company_ID, 0, 0, 0, 0, False, False, False)
            Else
              aTempTable = masterPage.aclsData_Temp.ChangeProspectNotesByParameters(company_ID, 0, 0, 0, 0, 0, False, False, False)
            End If
          End If
        End If
        'share datatable
        'Disposing Data Table
        RaiseEvent ShareProspectDataTable(aTempTable)
        aTempTable.Dispose()


        'Sharing Action Data
        aTempTable = New DataTable
        If Session.Item("localUser").crmEvo <> True Then
          If company_ID <> 0 Then
            aTempTable = Notes_Hold.Clone
            Notes_Search = Notes_Hold
            Dim afiltered_Client As DataRow() = Notes_Search.Select("lnote_status = 'P'", "")
            ' extract and import
            For Each atmpDataRow_Client In afiltered_Client
              aTempTable.ImportRow(atmpDataRow_Client)
            Next
          End If
        End If
        RaiseEvent ShareActionDataTable(aTempTable) 'share datatable
        'Disposing Data Table
        aTempTable.Dispose()
        'Sharing Document Data
        aTempTable = New DataTable
        If Session.Item("localUser").crmEvo <> True Then
          If Session.Item("localSubscription").crmDocumentsFlag = True Then
            If company_ID <> 0 Then
              aTempTable = Notes_Hold.Clone
              Notes_Search = Notes_Hold
              Dim afiltered_Client As DataRow() = Notes_Search.Select("lnote_status = 'F'", "")
              ' extract and import
              For Each atmpDataRow_Client In afiltered_Client
                aTempTable.ImportRow(atmpDataRow_Client)
              Next
            End If
          End If
        End If
        'share datatable
        'Disposing Data Table
        RaiseEvent ShareDocumentDataTable(aTempTable)
        aTempTable.Dispose()
      End If
      aTempTable.Dispose()
    Catch ex As Exception
      error_string = "CompanyCard.ascx.vb - Fill_Company_Info() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Functions For Categories, switching between jetnet/client, show next-previous buttons and next/prev buttons events"
  'Sub switch(ByVal sender As Object, ByVal e As System.EventArgs)
  '    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
  '    Try
  '        Session.Item("ListingID") = sender.commandName
  '        Session.Item("ContactID") = 0
  '        If Session.Item("ListingSource") = "CLIENT" Then
  '            Session.Item("ListingSource") = "JETNET"
  '        Else
  '            Session.Item("ListingSource") = "CLIENT"
  '        End If
  '        RaiseEvent SetUpDisplay()
  '    Catch ex As Exception
  '        error_string = "CompanyCard.ascx.vb - switch() " & ex.Message
  '        masterPage.LogError(error_string)
  '    End Try
  'End Sub
  Private Sub set_next_prev()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim next_prev As String = ""
    Dim next_id As String = ""
    Dim prev_id As String = ""
    Dim prev_comp As String = ""
    Dim next_type As String = ""
    Dim next_comp As String = ""
    Dim prev_type As String = ""
    Dim session_var_next() As String
    Dim session_var_prev() As String

    If Session.Item("IsJob") <> True Then
      Try
        If Not IsNothing(Session("my_ids")) Then
          For i = LBound(Session("my_ids")) To UBound(Session("my_ids"))
            'Response.Write(Session("my_ids")(i) & "!<br />")
            Dim session_var() As String = Split((Session("my_ids")(i)), "|")
            Dim compare_id As String = Trim(Request("contact_id"))
            If compare_id = "" Then
              compare_id = Session("ListingID")
            End If
            If session_var(0) = compare_id Then
              'Try
              If UBound(Session("my_ids")) = i Then
                'No Next
              Else
                session_var_next = Split((Session("my_ids")(i + 1)), "|")
                next_id = session_var_next(0)
                next_type = session_var_next(1)

                If InStr((Session("my_ids")(i + 1)), "|comp") > 0 Then
                  next_comp = Replace(session_var_next(2), "comp:", "")
                End If
              End If

              If LBound(Session("my_ids")) = i Then
                'Nothing previous
              Else
                session_var_prev = Split((Session("my_ids")(i - 1)), "|")
                prev_id = session_var_prev(0)
                prev_type = session_var_prev(1)

                If InStr((Session("my_ids")(i - 1)), "|comp") > 0 Then
                  prev_comp = Replace(session_var_prev(2), "comp:", "")
                End If
              End If
            End If
          Next

          Dim next_prev_str As String = "<table width='50' cellpadding='0' cellspacing='0' border='0'><tr>"

          If prev_comp <> "" Then
            next_prev_str = next_prev_str & "<td align='left' valign='top'><a href='details.aspx?source=" & prev_type & "&contact_id=" & prev_id & "&comp_ID=" & prev_comp & "&type=1'><img src='images/previous.png' alt='Previous'  border='0' /></a></td>"
          ElseIf prev_id <> "" Then
            next_prev_str = next_prev_str & "<td align='left' valign='top'><a href='details.aspx?source=" & prev_type & "&comp_ID=" & prev_id & "&type=1'><img src='images/previous.png' alt='Previous'  border='0' /></a></td>"
          Else
            next_prev_str = next_prev_str & "<td align='left' valign='top'><img src='images/spacer.gif' alt='' width='25' height='25' /></td>"
          End If


          If next_comp <> "" Then
            If next_prev_str <> "" Then
              next_prev_str = next_prev_str & "  "
            End If
            next_prev_str = next_prev_str & "<td align='left' valign='top'><a href='details.aspx?source=" & next_type & "&contact_id=" & next_id & "&&comp_ID=" & next_comp & "&type=1'><img src='images/next.png' alt='Next' border='0'  /></a></td>"
          ElseIf next_id <> "" Then
            If next_prev_str <> "" Then
              next_prev_str = next_prev_str & "  "
            End If
            next_prev_str = next_prev_str & "<td align='left' valign='top'><a href='details.aspx?source=" & next_type & "&comp_ID=" & next_id & "&type=1'><img src='images/next.png' alt='Next' border='0'  /></a></td>"
          Else
            next_prev_str = next_prev_str & "<td align='left' valign='top'><img src='images/spacer.gif' alt='' width='25' height='25' /></td>"
          End If
          next_prev_str = next_prev_str & "</tr></table>"
          Dim lab As New Label
          lab.Text = next_prev_str
          next_prev_text.Controls.Add(lab)
        End If
      Catch ex As Exception
        error_string = "CompanyCard.ascx.vb - set_next_prev() " & ex.Message
        masterPage.LogError(error_string)
      End Try
    End If
  End Sub
  Private Sub show(ByVal sender As Object, ByVal e As System.EventArgs)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      RaiseEvent Next_Prev_Btn(sender.commandname)
    Catch ex As Exception
      error_string = "CompanyCard.ascx.vb - show() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub OtherIDSetting(ByVal id As Integer, ByVal source As String) 'Function that will let me set the other ID (jetnet/client) on the masterpage
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    If source = "CLIENT" Then
      If Not IsDBNull(id) Then
        If id <> 0 Then
          RaiseEvent SetOtherID(id)
          masterPage.ShowJetnetClientOption = True
        End If
      End If
    Else
      aTempTable = masterPage.aclsData_Temp.CheckforCompanyBy_JETNET_ID(id, "")
      If Not IsNothing(aTempTable) Then 'not nothing
        If aTempTable.Rows.Count > 0 Then
          RaiseEvent SetOtherID(aTempTable.Rows(0).Item("comp_id"))
          masterPage.ShowJetnetClientOption = True
          If Session.Item("OtherID") <> 0 Then
            edit_company.Visible = False
          End If
        End If
      Else
      End If

    End If
  End Sub
#End Region
End Class