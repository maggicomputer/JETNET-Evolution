Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class _contactCard
  Inherits System.Web.UI.UserControl
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event Change_ContactID(ByVal x As Integer)
  Public Event Change_Display(ByVal x As String, ByVal type As Integer)
  Public Event SetUpDisplay()
  Dim error_string As String = ""
  Dim contact_text As String = ""
  Dim Aircraft_Data As New clsClient_Aircraft
#Region "Page Events"
  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    If Me.Visible Then
      Dim masterPage As main_site = DirectCast(Page.Master, main_site)
      Try

        If Not Page.IsPostBack Then
          masterPage.OtherID = masterPage.OtherID
        Else
          masterPage.OtherID = Session.Item("OtherID")
        End If
        If Not IsNothing(Request.Item("comp_ID")) Then
          If Not String.IsNullOrEmpty(Request.Item("comp_ID").ToString) Then
            If IsNumeric(Request.Item("comp_ID").Trim) Then
              masterPage.ListingID = Request.Item("comp_ID").Trim
            Else
              Response.Redirect("home.aspx")
            End If
          Else
            Response.Redirect("home.aspx")
          End If
        Else
          masterPage.ListingID = Session("ListingID")
        End If



        If Not IsNothing(Request.Item("ac_ID")) Then
          If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
            If IsNumeric(Request.Item("ac_ID").Trim) Then
              masterPage.ListingID = Request.Item("ac_ID").Trim
            Else
              Response.Redirect("home.aspx")
            End If
          End If
        Else
          masterPage.ListingID = Session("ListingID")
        End If

        If Not IsNothing(Request.Item("contact_ID")) Then
          If Not String.IsNullOrEmpty(Request.Item("contact_ID").ToString) Then
            If IsNumeric(Request.Item("contact_ID").Trim) Then
              masterPage.Listing_ContactID = Request.Item("contact_ID").Trim
            Else
              Response.Redirect("home.aspx")
            End If
          End If
        Else
          masterPage.Listing_ContactID = 0
        End If
        If Not IsNothing(Request.Item("source")) Then
          If Not String.IsNullOrEmpty(Request.Item("source").ToString) Then
            masterPage.ListingSource = Request.Item("source").Trim
          End If
        Else
          masterPage.ListingSource = Session("ListingSource")
        End If
        If Not IsNothing(Request.Item("type")) Then
          If Not String.IsNullOrEmpty(Request.Item("type").ToString) Then
            masterPage.TypeOfListing = Request.Item("type").Trim
          End If
        Else
          masterPage.TypeOfListing = Session("Listing")
        End If
        masterPage.Listing_ContactID = Session.Item("ContactID")
        masterPage.FromTypeOfListing = Session.Item("FromTypeOfListing")
        save_folder_bottom.Visible = False
        save_folder_top.Visible = False

        If masterPage.ListingID = 0 Then 'no listing ID
          Response.Redirect("home.aspx", False)
        End If
      Catch ex As Exception
        error_string = "ContactCard.ascx.vb - Page Init() - " & ex.Message
        masterPage.LogError(error_string)
      End Try
    End If
  End Sub
#End Region

#Region "Fill Contact Card based on AC or Company"
  Public Sub saveFolder()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim flist As CheckBoxList = FindControl("folder_ids")
      Dim personal_flist As CheckBoxList = FindControl("personal_folder_ids")
      Dim jetnet_ac_id As Integer = 0
      Dim jetnet_comp_id As Integer = 0
      Dim jetnet_contact_id As Integer = 0
      Dim client_ac_id As Integer = 0
      Dim client_comp_id As Integer = 0
      Dim client_contact_id As Integer = 0
      Dim cfolder_id As Integer = 0
      Dim fval As String = ""
      Dim errored As String = ""

      Select Case masterPage.TypeOfListing
        Case 1
          Select Case masterPage.ListingSource
            Case "JETNET"
              jetnet_comp_id = masterPage.ListingID
            Case "CLIENT"
              client_comp_id = masterPage.ListingID
          End Select

          If masterPage.Listing_ContactID <> 0 Then
            Select Case masterPage.ListingSource
              Case "JETNET"
                jetnet_contact_id = masterPage.ListingID
              Case "CLIENT"
                client_contact_id = masterPage.ListingID
            End Select
          End If
        Case 3
          Select Case masterPage.ListingSource
            Case "JETNET"
              jetnet_ac_id = masterPage.ListingID
            Case "CLIENT"
              client_ac_id = masterPage.ListingID
          End Select
      End Select

      clsGeneral.clsGeneral.Save_Folder_Action(flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterPage.aclsData_Temp)
      clsGeneral.clsGeneral.Save_Folder_Action(personal_flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterPage.aclsData_Temp)

    Catch ex As Exception
      error_string = "ContactCard.ascx.vb - Save_Folder() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub
  Public Sub fill_Contact_Info_AC(ByVal idnum As Integer, ByVal source As String, ByVal parent As Integer)


    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    aircraft_contact_details.Controls.Clear()
    'ac_picture_tab.Visible = True
    aircraft_flight_tab.Visible = True
    company_contact_tab.Visible = False
    ac_picture_tab.Visible = True
    company_profile_tabs.Visible = False
    aircraft_contact_tab.Visible = True


    folders.Visible = True

    Dim counter As Integer = 0
    Dim acref_contact_type As String = ""
    Dim act_name As String = ""
    Dim acref_owner_percentage As Double = 0
    Dim comp_id As Integer = 0
    Dim contact_id As Integer = 0
    Dim comp_name As String = ""
    Dim contact_first_name As String = ""
    Dim contact_title As String = ""
    Dim contact_last_name As String = ""
    Dim comp_city As String = ""
    Dim comp_state As String = ""
    Dim comp_country As String = ""
    Dim strContact As String = ""
    Dim cliacref_contact_priority As Integer = 0
    Dim acref_id As Integer = 0
    Dim cell_text As New Label
    Dim ac_contact As New Table

    'added 1/16/2013
    Dim phone_number As String = ""
    Dim PhoneTable As New DataTable

    ac_contact.Width = Unit.Percentage(100)

    '---------------------------Aircraft Contact Information-----------------------------------------------------
    Try
      'Get the added contacts in the client database. 


      If Not IsNothing(ViewState("Aircraft_Contacts")) Then
        aTempTable = DirectCast(ViewState("Aircraft_Contacts"), DataTable)
      Else
        If Not Page.IsPostBack Then
          If source = "CLIENT" Then
            aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Client_acID_Full_Details(idnum)
            If masterPage.OtherID <> 0 Then
              clsGeneral.clsGeneral.Fill_Aircraft_Pictures(masterPage.OtherID, picture_label, masterPage)
            End If
          Else
            If Session.Item("localUser").crmEvo = True Then
              aTempTable = New DataTable
            Else
              aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(idnum)
            End If
            'pictures jetnet
            clsGeneral.clsGeneral.Fill_Aircraft_Pictures(idnum, picture_label, masterPage)

          End If

        End If
        '15806
      End If

      masterPage.PerformDatabaseAction = True

      ViewState("Aircraft_Contacts") = aTempTable

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows

            counter = counter + 1
            strContact = ""
            acref_contact_type = IIf(Not IsDBNull(r("acref_contact_type")), r("acref_contact_type"), 0)
            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
            acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
            comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), "")
            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
            contact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
            comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
            comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
            comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
            cliacref_contact_priority = IIf(Not IsDBNull(r("cliacref_contact_priority")), r("cliacref_contact_priority"), 0)
            acref_id = IIf(Not IsDBNull(r("acref_id")), r("acref_id"), 0)

            If Session.Item("localSubscription").crmAerodexFlag = True And r("acref_contact_type") = "99" Then

            Else
              Dim ro As New TableRow
              Dim itz As New TableCell
              cell_text = New Label
              itz.VerticalAlign = VerticalAlign.Top
              ro.VerticalAlign = VerticalAlign.Top
              ro.Height = 20

              ro.CssClass = "client"

              strContact = "<a href='#' onclick=""javascript:load('company_spec.aspx?source=CLIENT&company_ID=" & comp_id & "','','scrollbars=yes,menubar=no,height=700,width=930,resizable=yes,toolbar=no,location=no,status=no');"">" & DisplayRecentCompanyNote(comp_id, "CLIENT") & "</a>"


              cell_text = New Label
              cell_text.Text = strContact
              itz.Width = 30
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)

              strContact = ""

              If acref_contact_type = "99" Then
                strContact = "<span class='small_purple'>Exclusive Broker</span>"
              ElseIf acref_contact_type = "12" Then
                strContact = "<span class='small_orange'>Lessee</span>"
              Else
                strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                If acref_contact_type = "8" Or acref_contact_type = "97" Then
                  If acref_owner_percentage <> 0 Then
                    strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                  End If
                End If
              End If

              strContact = strContact & "<br /><a href='#' onClick=""javascript:var test = confirm('Are you sure you want to delete this contact?');if (test){load('edit.aspx?action=reference&remove=true&id=" & acref_id & "','','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');return false;}{return false;};"" style='text-decoration:none;'><span class='tiny_tiny'>(remove)</span></a><br />"

              cell_text = New Label
              itz = New TableCell
              itz.Width = 70
              cell_text.Text = strContact
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)

              cell_text = New Label
              itz = New TableCell
              cell_text.Text = "&nbsp;&nbsp;&nbsp;&nbsp;"
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)

              strContact = "<a href='details.aspx?comp_ID=" & comp_id & "&source=CLIENT&type=1' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"


              If comp_city <> "" Then
                strContact = strContact & comp_city & " "
              End If
              If comp_state <> "" Then
                strContact = strContact & comp_state & " "
              End If
              If comp_country <> "" Then
                strContact = strContact & comp_country
              End If
              strContact = strContact & "</span>"

              If contact_id <> 0 Then
                'Changed on 1/16/2013. 
                'This small block is going to basically figure out your phone number for a contact or a 
                'and display it on the aircraft contact card.
                'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, contact_id, source, 0)

                If Not IsNothing(PhoneTable) Then
                  If PhoneTable.Rows.Count > 0 Then
                    'your phone number was found here.
                    phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                  Else
                    'no phone number was found here.
                    phone_number = ""
                  End If
                Else 'error logging.
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                  End If
                End If
              End If



              If contact_id <> 0 Then
                strContact = strContact & "<br /><a href='details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=CLIENT&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em></a>"
                If contact_title <> "" Then
                  strContact = strContact & "&nbsp;<span class='smaller'> (" & contact_title & ")</em></span>"
                End If
                If phone_number <> "" Then
                  If contact_title <> "" Then
                    strContact = strContact & "<br />"
                  Else
                    strContact = strContact & "&nbsp;"
                  End If
                  strContact = strContact & "<span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
                End If
              End If

              strContact = strContact & "<br />"

              cell_text = New Label
              itz = New TableCell
              cell_text.Text = strContact
              itz.Controls.Add(cell_text)
              itz.Width = 350
              ro.Cells.Add(itz)
              ac_contact.Rows.Add(ro)

            End If
          Next


        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & error_string)
        End If
        masterPage.display_error()
      End If

      ' get the contact info
      If source = "JETNET" Then

        aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(idnum, 0)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              counter = counter + 1
              If Session.Item("localSubscription").crmAerodexFlag = True And r("act_name") = "Exclusive Broker" Then

              Else
                Dim ro As New TableRow
                Dim itz As New TableCell
                ro.VerticalAlign = VerticalAlign.Top

                itz.VerticalAlign = VerticalAlign.Top

                cell_text = New Label
                act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
                comp_id = IIf(Not IsDBNull(r("acref_comp_id")), r("acref_comp_id"), 0)
                comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")

                contact_id = IIf(Not IsDBNull(r("acref_contact_id")), r("acref_contact_id"), 0)

                contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                strContact = ""

                strContact = strContact & "<a href='#' onclick=""javascript:load('company_spec.aspx?source=JETNET&company_ID=" & comp_id & "','','scrollbars=yes,menubar=no,height=700,width=930,resizable=yes,toolbar=no,location=no,status=no');"">" & DisplayRecentCompanyNote(comp_id, "JETNET") & "</a>"

                cell_text = New Label
                cell_text.Text = strContact
                itz.Width = 30
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)

                strContact = ""

                If act_name = "Exclusive Broker" Then
                  strContact = strContact & "<b class='small_purple'>" & act_name & "</b>"
                ElseIf act_name = "Lessee" Then
                  strContact = "<span class='small_orange'>Lessee</span>"
                ElseIf act_name <> "" Then
                  strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                  If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                    strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                  End If
                End If

                cell_text = New Label
                itz = New TableCell
                itz.Width = 70
                cell_text.Text = strContact
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)

                cell_text = New Label
                itz = New TableCell
                itz.Width = 20
                cell_text.Text = "&nbsp;&nbsp;&nbsp;&nbsp;"
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)
                strContact = ""

                If comp_name <> "" Then
                  strContact = strContact & "<a href='details.aspx?comp_ID=" & comp_id & "&type=1&source=JETNET' class='bold_small'>" & comp_name & "</a><br /><span class='smaller'>"
                End If

                If comp_city <> "" Then
                  strContact = strContact & comp_city & " "
                End If
                If comp_state <> "" Then
                  strContact = strContact & comp_state & " "
                End If
                If comp_country <> "" Then
                  strContact = strContact & comp_country
                End If

                strContact = strContact & "</span>"

                If contact_id <> 0 Then
                  'Changed on 1/16/2013. 
                  'This small block is going to basically figure out your phone number for a contact or a 
                  'and display it on the aircraft contact card.
                  'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                  PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, contact_id, source, 0)

                  If Not IsNothing(PhoneTable) Then
                    If PhoneTable.Rows.Count > 0 Then
                      'your phone number was found here.
                      phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                    Else
                      'no phone number was found here.
                      phone_number = ""
                    End If
                  Else 'error logging.
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                    End If
                  End If
                End If


                If contact_first_name <> "" Then
                  strContact = strContact & "<br /><a href='details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><em class='small'>" & contact_first_name & " " & contact_last_name & "</em></a>"

                  If contact_title <> "" Then
                    strContact = strContact & "&nbsp;<span class='smaller'> (" & contact_title & ")</em></span>"
                  End If
                  If phone_number <> "" Then
                    If contact_title <> "" Then
                      strContact = strContact & "<br />"
                    Else
                      strContact = strContact & "&nbsp;"
                    End If
                    strContact = strContact & "<span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
                  End If
                End If



                cell_text = New Label
                itz = New TableCell
                cell_text.Text = strContact
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)
                ac_contact.Rows.Add(ro)

              End If
            Next
          End If
          ' dump the datatable
          aTempTable.Dispose()
          aTempTable = Nothing
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & error_string)
          End If
          masterPage.display_error()
        End If
      End If

      Dim con As New Panel
      ' con.Height = 170
      ' con.CssClass = "card_overflow" '"card_overflow_long"
      aircraft_contact.Visible = True
      aircraft_contact_details.Visible = True
      contact_add.Visible = False
      ac_contact.CssClass = "seperator"

      ac_contact.CellPadding = 0
      ac_contact.CellSpacing = 0
      ac_contact.EnableViewState = True
      con.Controls.Add(ac_contact)
      con.EnableViewState = True
      aircraft_contact_details.Controls.Clear()
      aircraft_contact_details.Controls.Add(con)

      If Session.Item("localUser").crmEvo = False Then

        folders.Controls.Clear()


        Select Case masterPage.TypeOfListing
          Case 1
            'Company
            If masterPage.Listing_ContactID <> 0 Then
              Select Case masterPage.ListingSource
                Case "JETNET"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
                Case "CLIENT"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
              End Select
            Else 'No Contact 
              Select Case masterPage.ListingSource
                Case "JETNET"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                Case "CLIENT"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
              End Select
            End If
          Case 3
            Select Case masterPage.ListingSource
              Case "CLIENT"
                folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
              Case "JETNET"
                folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
            End Select
        End Select

        folders_tab.Visible = True
        folders.Visible = True
        Dim check As CheckBoxList = folders.FindControl("personal_folder_ids")

        If Not IsNothing(check) And folders.Visible = True Then
          save_folder_top.Visible = True
          save_folder_bottom.Visible = True
        End If

        check = New CheckBoxList
        check = folders.FindControl("folder_ids")
        If Not IsNothing(check) And folders.Visible = True Then
          save_folder_top.Visible = True
          save_folder_bottom.Visible = True
        End If
        folders_tab.Visible = True
      Else
        aircraft_contact_add.Visible = False
      End If




      '  aircraft_contact_tab.Visible = True


    Catch ex As Exception
      error_string = "ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub
  Public Sub fill_Contact_Info_Company(ByVal contact_id As Integer, ByVal idnum As Integer, ByVal source As String, ByVal parent As Integer, ByVal newpage As Boolean)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim show_contact As Boolean = False 'this variable is set with the request variable SC. If it's set, display all contacts.
    'This is a request variable that's set if the view all button is clicked on a contact to display all contacts.
    'Generally it works okay without this, but when there is a profile on the company, that gets precedence.
    'This is supposed to halt that. 
    If Not IsNothing(Trim(Request("sc"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("sc"))) Then
        If (Trim(Request("sc"))) = "true" Then
          show_contact = True
        End If
      End If
    End If
    'turn on vis on tabs
    company_contact_tab.Visible = True
    ac_picture_tab.Visible = False
    company_profile_tabs.Visible = False
    aircraft_flight_tab.Visible = False
    tab_info_container.ActiveTab = company_contact_tab
    'turn off ac tabs
    aircraft_contact_tab.Visible = False
    Try

      Dim view_one As String = ""
      If Not IsNothing(Request.Item("view")) Then
        If Not String.IsNullOrEmpty(Request.Item("view").ToString) Then
          view_one = Request.Item("view").Trim
        End If
      End If

      If Not IsNothing(Request.Item("folders")) Then
        If Not String.IsNullOrEmpty(Request.Item("folders").ToString) Then
          folders_saved_message.Visible = True
        End If
      End If


      '------Contact Information (if a contact ID is clicked display)----------------------------------------------------------------------
      If contact_id <> 0 Then
        Try
          'this just resets the other ID so nothing gets held
          masterPage.Listing_ContactID_Other = 0
          masterPage.OtherID = masterPage.OtherID
          clsGeneral.clsGeneral.Recent_Cookies("contacts", contact_id, UCase(masterPage.ListingSource))
          'Only for non evo!
          'toggle the tabs.
          view_all_contacts.Visible = True
          If Session.Item("localUser").crmEvo <> True Then 'If an EVO user

            Select Case masterPage.TypeOfListing
              Case 1
                'Company
                If masterPage.Listing_ContactID <> 0 Then
                  Select Case masterPage.ListingSource
                    Case "JETNET"
                      folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
                    Case "CLIENT"
                      folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
                  End Select
                Else 'No Contact 
                  Select Case masterPage.ListingSource
                    Case "JETNET"
                      folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                    Case "CLIENT"
                      folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                  End Select
                End If
              Case 3
                Select Case masterPage.ListingSource
                  Case "CLIENT"
                    folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
                  Case "JETNET"
                    folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
                End Select
            End Select

            folders_tab.Visible = True
            folders.Visible = True
            Dim check As CheckBoxList = folders.FindControl("personal_folder_ids")

            If Not IsNothing(check) And folders.Visible = True Then
              save_folder_top.Visible = True
              save_folder_bottom.Visible = True
            End If

            check = New CheckBoxList
            check = folders.FindControl("folder_ids")
            If Not IsNothing(check) And folders.Visible = True Then
              save_folder_top.Visible = True
              save_folder_bottom.Visible = True
            End If
            folders_tab.Visible = True
          End If

          If Session.Item("ListingSource") = "CLIENT" And Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
            contact_edit_btn.Visible = True
            contact_edit.Visible = True
            ImageButton1.Visible = True
          ElseIf Session.Item("OtherID") <> 0 And Session.Item("ListingSource") = "JETNET" Or Session.Item("localUser").crmEvo = True Then 'If an EVO user Then
            contact_edit_btn.Visible = False
            ImageButton1.Visible = False
          End If
          ' used this call to get the contacts
          aTempTable = masterPage.aclsData_Temp.GetContacts_Details(contact_id, source)
          Dim comp_id As Integer = 0
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                Dim linky As New Label
                company_contact_tab.HeaderText = "" & R("contact_first_name") & " " & R("contact_middle_initial") & " " & R("contact_last_name")

                If source = "CLIENT" Then
                  masterPage.Listing_ContactID_Other = R("contact_jetnet_contact_id")
                  masterPage.OtherID = masterPage.OtherID
                  If Not (IsDBNull(R("contact_preferred_name"))) Then
                    If R("contact_preferred_name") <> "" Then
                      contact_text = "Preferred Name: " & R("contact_preferred_name") & "<br />"
                    End If
                  End If
                Else
                  aTempTable2 = masterPage.aclsData_Temp.GetContactInfo_JETNET_ID(contact_id, "Y")
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      masterPage.Listing_ContactID_Other = aTempTable2.Rows(0).Item("contact_id")
                      masterPage.OtherID = masterPage.OtherID
                    End If
                  End If
                  If masterPage.Listing_ContactID_Other = 0 And masterPage.OtherID > 0 Then
                    'This doesn't exist on Jetnet Side
                    compJetnetAddToClient.Visible = True
                  End If
                End If


                contact_text = contact_text & R("contact_title") & " <br />"
                If Not (IsDBNull(R("contact_email_address"))) Then
                  If R("contact_email_address") <> "" Then
                    contact_text = contact_text & "<a href='mailto:" & R("contact_email_address") & "' class='non_special_link'>" & R("contact_email_address") & "</a>"

                    If Session.Item("localUser").crmEvo = False Then
                      contact_text = contact_text & "<a href='#' onclick=""javascript:load('edit_note.aspx?action=new&type=email&source=" & Session.Item("ListingSource") & "&comp_ID=" & R("contact_comp_id") & "contact_ID=" & R("contact_id") & "&cat_key=0','','scrollbars=yes,menubar=no,height=805,width=860,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/mail_compose.png' alt='Email Company' width='24' border='0' /></a>"
                    End If
                  End If
                End If
                If UCase(source) = "CLIENT" Then
                  '  contact_text = contact_text & "Email List: " & clsGeneral.clsGeneral.yes_no(R("contact_email_list"), "else") & "<br />"
                  contact_text = contact_text & "<br /><em>" & R("contact_notes") & "</em>"
                End If
              Next
            Else
              'zero rows
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - fill_Contact_Info_Company() - " & error_string)
            End If
            masterPage.display_error()
          End If
          aircraft_right_panel.Visible = True
          contact_details.Visible = True
          contact_details.Text = contact_text

        Catch ex As Exception
          error_string = "ContactCard.ascx.vb - fill_Contact_Info_Company() Individual Contact - " & ex.Message
          masterPage.LogError(error_string)
        End Try
        '------Contact Information Phone Numbers(if a contact ID is clicked display)----------------------------------------------------------------------
        Try
          contact_text = "<h4>Phone Numbers</h4>"
          aTempTable = masterPage.aclsData_Temp.GetPhoneNumbers(idnum, contact_id, source, 0)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              ' set it to the datagrid 
              For Each q As DataRow In aTempTable.Rows
                contact_text = contact_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
              Next
            Else
              '0 rows
              contact_phone_details.Visible = False
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - fill_Contact_Info_Company() - " & error_string)
            End If
            masterPage.display_error()
          End If
          contact_phone_details.Text = contact_text
        Catch ex As Exception
          error_string = "ContactCard.ascx.vb - fill_Contact_Info_Company() Phone# - " & ex.Message
          masterPage.LogError(error_string)
        End Try
        'make add contact invisible
        comp_contact_add.Visible = False
      Else
        '------Contact Information (view all contacts)----------------------------------------------------------------------

        If Session.Item("ListingSource") = "JETNET" Then
          comp_contact_add.Visible = False
        Else
          comp_contact_add.Visible = True
        End If

        Try
          'This is going to fill the contacts gridview on the right.
          aTempTable = masterPage.aclsData_Temp.GetContacts(idnum, source, "Y", 0)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              ' set it to the datagrid 
              contacts_gv.DataSource = aTempTable
              contacts_gv.DataBind()
              contacts_gv.Visible = True

              If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                Select Case masterPage.TypeOfListing
                  Case 1
                    'Company
                    If masterPage.Listing_ContactID <> 0 Then
                      Select Case masterPage.ListingSource
                        Case "JETNET"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
                        Case "CLIENT"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
                      End Select
                    Else 'No Contact 
                      Select Case masterPage.ListingSource
                        Case "JETNET"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                        Case "CLIENT"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                      End Select
                    End If
                  Case 3
                    Select Case masterPage.ListingSource
                      Case "CLIENT"
                        folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
                      Case "JETNET"
                        folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
                    End Select
                End Select
                folders_tab.Visible = True
                folders.Visible = True
                Dim check As CheckBoxList = folders.FindControl("personal_folder_ids")

                If Not IsNothing(check) And folders.Visible = True Then
                  save_folder_top.Visible = True
                  save_folder_bottom.Visible = True
                End If

                check = New CheckBoxList
                check = folders.FindControl("folder_ids")
                If Not IsNothing(check) And folders.Visible = True Then
                  save_folder_top.Visible = True
                  save_folder_bottom.Visible = True
                End If

              End If
            Else
              contact_no_results.Visible = True
              contacts_gv.Visible = False
              contact_no_results.Text = "<p align='center'>No Contacts Currently</p>"
              contact_no_results.ForeColor = Drawing.Color.Red
              contact_no_results.Font.Bold = True

              Dim check As CheckBoxList = folders.FindControl("personal_folder_ids")
              folders.Controls.Clear()
              If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                Select Case masterPage.TypeOfListing
                  Case 1
                    'Company
                    If masterPage.Listing_ContactID <> 0 Then
                      Select Case masterPage.ListingSource
                        Case "JETNET"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
                        Case "CLIENT"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
                      End Select
                    Else 'No Contact 
                      Select Case masterPage.ListingSource
                        Case "JETNET"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                        Case "CLIENT"
                          folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                      End Select
                    End If
                  Case 3
                    Select Case masterPage.ListingSource
                      Case "CLIENT"
                        folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
                      Case "JETNET"
                        folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
                    End Select
                End Select

                folders_tab.Visible = True
                folders.Visible = True
                If Not IsNothing(check) And folders.Visible = True Then
                  save_folder_top.Visible = True
                  save_folder_bottom.Visible = True
                  'linky2.Visible = True
                End If

                check = New CheckBoxList
                check = folders.FindControl("folder_ids")
                If Not IsNothing(check) And folders.Visible = True Then
                  save_folder_top.Visible = True
                  save_folder_bottom.Visible = True
                  'linky2.Visible = True
                End If
              End If
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - fill_Contact_Info_Company() - " & error_string)
            End If
            masterPage.display_error()
          End If
        Catch ex As Exception
          error_string = "ContactCard.ascx.vb - fill_Contact_Info_Company() ContactsGrid - " & ex.Message
          masterPage.LogError(error_string)
        End Try

        'air_comp_head.Controls.Add(lab4)
        If Session.Item("localUser").crmEvo <> True Then 'If an EVO user do not display the /folders link
          contact_edit_btn.Visible = False
          'air_comp_head.Controls.Add(new_link)
        End If
        '---Added for Profile Information! This'll only show up if there's profile information associated with the company. This includes category,
        '---Company descriptions. These only show up for client companies so if the source is jetnet, you can automatically ignore it. 

        If source = "CLIENT" And contact_id = 0 Then
          aTempTable = masterPage.aclsData_Temp.GetCompanyInfo_ID(idnum, source, 0)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows

                'This is storing the description/categories to be further maniuplated.  
                Dim description As String = IIf(Not IsDBNull(R("clicomp_description")), R("clicomp_description"), "")
                Dim cat1 As String = CStr(IIf(Not IsDBNull(R("clicomp_category1")), R("clicomp_category1"), ""))
                Dim cat2 As String = CStr(IIf(Not IsDBNull(R("clicomp_category2")), R("clicomp_category2"), ""))
                Dim cat3 As String = CStr(IIf(Not IsDBNull(R("clicomp_category3")), R("clicomp_category3"), ""))
                Dim cat4 As String = CStr(IIf(Not IsDBNull(R("clicomp_category4")), R("clicomp_category4"), ""))
                Dim cat5 As String = CStr(IIf(Not IsDBNull(R("clicomp_category5")), R("clicomp_category5"), ""))

                If description <> "" Or cat1 <> "" Or cat2 <> "" Or cat3 <> "" Or cat4 <> "" Or cat5 <> "" Then 'Don't display profile if no info
                  company_profile_tabs.Visible = True
                End If

                If description <> "" And view_one = "" Or cat1 <> "" And view_one = "" Or cat2 <> "" And view_one = "" Or cat3 <> "" And view_one = "" Or cat4 <> "" And view_one = "" Or cat5 <> "" And view_one = "" Or view_one = "profile" Then 'Don't display profile if no info
                  Dim profile As String = ""
                  If newpage = True Then
                    If show_contact = False Then
                      tab_info_container.ActiveTab = company_profile_tabs
                    End If
                  End If

                  If description <> "" Then
                    profile = "<p align='left' class='no_pad'>Description: " & description & "</p>"
                    profile = Replace(profile, vbCrLf, "<br />")
                  Else
                    company_profile.Visible = False
                  End If

                  company_profile.Text = profile

                  profile = "<p align='left'>"

                  aTempTable2 = masterPage.aclsData_Temp.Get_Client_Preferences()
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      For Each z As DataRow In aTempTable2.Rows

                        If Not IsDBNull(z("clipref_category1_use")) Then
                          If z("clipref_category1_use") = "Y" Then
                            If Not IsDBNull(cat1) Then
                              If cat1 <> "" Then
                                profile = profile & z("clipref_category1_name") & ": " & cat1 & "<br />"
                              End If
                            End If
                          End If
                        End If

                        If Not IsDBNull(z("clipref_category2_use")) Then
                          If z("clipref_category2_use") = "Y" Then
                            If Not IsDBNull(cat2) Then
                              If cat2 <> "" Then
                                profile = profile & z("clipref_category2_name") & ": " & cat2 & "<br />"
                              End If
                            End If
                          End If
                        End If

                        If Not IsDBNull(z("clipref_category3_use")) Then
                          If z("clipref_category3_use") = "Y" Then
                            If Not IsDBNull(cat3) Then
                              If cat3 <> "" Then
                                profile = profile & z("clipref_category3_name") & ": " & cat3 & "<br />"
                              End If
                            End If
                          End If
                        End If

                        If Not IsDBNull(z("clipref_category4_use")) Then
                          If z("clipref_category4_use") = "Y" Then
                            If Not IsDBNull(cat4) Then
                              If cat4 <> "" Then
                                profile = profile & z("clipref_category4_name") & ": " & cat4 & "<br />"
                              End If
                            End If
                          End If
                        End If

                        If Not IsDBNull(z("clipref_category5_use")) Then
                          If z("clipref_category5_use") = "Y" Then
                            If Not IsDBNull(cat5) Then
                              If cat5 <> "" Then
                                profile = profile & z("clipref_category5_name") & ": " & cat5 & "<br />"
                              End If
                            End If
                          End If
                        End If

                        profile = profile & "</p>"
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("ContactCard.ascx.vb - fill_Contact_Info_Company() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                  company_categories.Text = profile

                End If

              Next

            End If
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "ContactCard.ascx.vb - fill_Contact_Info_Company() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub

  Public Sub fill_Contact_Info_AC_New(ByVal idnum As Integer, ByVal source As String, ByVal parent As Integer)


    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    aircraft_contact_details.Controls.Clear()
    'ac_picture_tab.Visible = True

    company_contact_tab.Visible = False
    ac_picture_tab.Visible = True
    company_profile_tabs.Visible = False
    aircraft_contact_tab.Visible = True


    folders.Visible = True

    Dim counter As Integer = 0
    Dim acref_contact_type As String = ""
    Dim act_name As String = ""
    Dim acref_owner_percentage As Double = 0
    Dim comp_id As Integer = 0
    Dim contact_id As Integer = 0
    Dim comp_name As String = ""
    Dim contact_first_name As String = ""
    Dim contact_title As String = ""
    Dim contact_last_name As String = ""
    Dim comp_city As String = ""
    Dim comp_state As String = ""
    Dim comp_country As String = ""
    Dim strContact As String = ""
    Dim cliacref_contact_priority As Integer = 0
    Dim acref_id As Integer = 0
    Dim cell_text As New Label
    'Dim ac_contact As New Table
    Dim contact_client As New Table
    Dim contact_jetnet As New Table
    Dim ContactSource As String = "JETNET"
    Dim reorderClient As Boolean = False
    'added 1/16/2013
    Dim phone_number As String = ""
    Dim PhoneTable As New DataTable

    'ac_contact.Width = Unit.Percentage(100)
    contact_client.Width = Unit.Percentage(100)
    contact_jetnet.Width = Unit.Percentage(100)
    '---------------------------Aircraft Contact Information-----------------------------------------------------
    Try
      'Get the added contacts in the client database. 


      If Not IsNothing(ViewState("Aircraft_Contacts")) Then
        aTempTable = DirectCast(ViewState("Aircraft_Contacts"), DataTable)
      Else
        If Not Page.IsPostBack Then
          If source = "CLIENT" Then
            aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Client_acID_Full_Details(idnum)
            If masterPage.OtherID <> 0 Then
              clsGeneral.clsGeneral.Fill_Aircraft_Pictures(masterPage.OtherID, picture_label, masterPage)
            End If
          Else
            If Session.Item("localUser").crmEvo = True Then
              aTempTable = New DataTable
            Else
              aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(idnum)
            End If
            'pictures jetnet
            clsGeneral.clsGeneral.Fill_Aircraft_Pictures(idnum, picture_label, masterPage)

            If masterPage.OtherID = 0 Then
              reorderClient = True
            End If
          End If

        End If
        '15806
      End If

      masterPage.PerformDatabaseAction = True

      ViewState("Aircraft_Contacts") = aTempTable

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows

            counter = counter + 1
            strContact = ""
            acref_contact_type = IIf(Not IsDBNull(r("acref_contact_type")), r("acref_contact_type"), 0)
            act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
            acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
            comp_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0)
            comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
            contact_id = IIf(Not IsDBNull(r("contact_id")), r("contact_id"), "")
            contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
            contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
            contact_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
            comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
            comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
            comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
            cliacref_contact_priority = IIf(Not IsDBNull(r("cliacref_contact_priority")), r("cliacref_contact_priority"), 0)
            acref_id = IIf(Not IsDBNull(r("acref_id")), r("acref_id"), 0)
            ContactSource = IIf(Not IsDBNull(r("source")), r("source"), "")
            If Session.Item("localSubscription").crmAerodexFlag = True And r("acref_contact_type") = "99" Then

            Else
              Dim ro As New TableRow
              Dim itz As New TableCell

              cell_text = New Label
              itz.VerticalAlign = VerticalAlign.Top
              ro.VerticalAlign = VerticalAlign.Top
              ro.Height = 20

              ro.CssClass = "client"



              strContact = "<br /><a href='#' onclick=""javascript:load('company_spec.aspx?source=CLIENT&company_ID=" & comp_id & "','','scrollbars=yes,menubar=no,height=700,width=930,resizable=yes,toolbar=no,location=no,status=no');"">" & DisplayRecentCompanyNote(comp_id, "CLIENT") & "</a>"


              cell_text = New Label
              cell_text.Text = strContact
              itz.Width = 20
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)

              strContact = ""

              If acref_contact_type = "99" Then
                strContact = "<span class='small_purple'>Exclusive Broker</span>"
              ElseIf acref_contact_type = "12" Then
                strContact = "<span class='small_orange'>Lessee</span>"
              Else
                strContact = "<span class='bold_small'>" & act_name & "</span>"
                If acref_contact_type = "8" Or acref_contact_type = "97" Then
                  If acref_owner_percentage <> 0 Then
                    strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                  End If
                End If
              End If

              strContact = strContact & "&nbsp;<a href='#' onClick=""javascript:var test = confirm('Are you sure you want to delete this contact?');if (test){load('edit.aspx?action=reference&remove=true&id=" & acref_id & "','','scrollbars=yes,menubar=no,height=100,width=100,resizable=yes,toolbar=no,location=no,status=no');return false;}{return false;};"" style='text-decoration:none;'><span class='tiny_tiny'>(remove)</span></a><br />"

              'ac_contact.Rows.Add(ro)

              'ro = New TableRow
              'cell_text = New Label
              'itz = New TableCell
              'itz.Width = 50
              'cell_text.Text = strContact
              'itz.Controls.Add(cell_text)
              'ro.Cells.Add(itz)

              'cell_text = New Label
              'itz = New TableCell
              'cell_text.Text = "&nbsp;&nbsp;"
              'itz.Controls.Add(cell_text)
              'ro.Cells.Add(itz)

              strContact = strContact & "<a href='details.aspx?comp_ID=" & comp_id & "&source=CLIENT&type=1' class='small'>" & comp_name & "</a><br /><span class='smaller'>"


              If comp_city <> "" Then
                strContact = strContact & comp_city & " "
              End If
              If comp_state <> "" Then
                strContact = strContact & comp_state & " "
              End If
              If comp_country <> "" Then
                strContact = strContact & comp_country
              End If

              If comp_id <> 0 Then
                'Changed on 1/16/2013. 
                'This small block is going to basically figure out your phone number for a contact or a 
                'and display it on the aircraft contact card.
                'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, 0, ContactSource, 0)

                If Not IsNothing(PhoneTable) Then
                  If PhoneTable.Rows.Count > 0 Then
                    'your phone number was found here.
                    phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                  Else
                    'no phone number was found here.
                    phone_number = ""
                  End If
                Else 'error logging.
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                  End If
                End If
              End If

              If phone_number <> "" Then
                strContact = strContact & "<br /><span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
              End If

              strContact = strContact & "</span>"

              If contact_id <> 0 Then
                'Changed on 1/16/2013. 
                'This small block is going to basically figure out your phone number for a contact or a 
                'and display it on the aircraft contact card.
                'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, contact_id, ContactSource, 0)

                If Not IsNothing(PhoneTable) Then
                  If PhoneTable.Rows.Count > 0 Then
                    'your phone number was found here.
                    phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                  Else
                    'no phone number was found here.
                    phone_number = ""
                  End If
                Else 'error logging.
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                  End If
                End If
              End If

              cell_text = New Label
              itz = New TableCell
              cell_text.Text = strContact
              itz.Controls.Add(cell_text)
              itz.Width = 225
              ro.Cells.Add(itz)
              contact_client.Rows.Add(ro)

              cell_text = New Label
              itz = New TableCell
              itz.Width = 10
              cell_text.Text = "&nbsp;"
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)
              strContact = ""

              If contact_id <> 0 Then
                strContact = strContact & "<br /><a href='details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=CLIENT&type=1'><span class='small'>" & contact_first_name & " " & contact_last_name & "</span></a>"
                If contact_title <> "" Then
                  strContact = strContact & "<br /><span class='smaller'>" & contact_title & "</em></span>"
                End If
                If phone_number <> "" Then
                  If contact_title <> "" Then
                    strContact = strContact & "<br />"
                  Else
                    strContact = strContact & "&nbsp;"
                  End If
                  strContact = strContact & "<span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
                End If
              End If

              strContact = strContact & "<br />"

              cell_text = New Label
              itz = New TableCell
              cell_text.Text = strContact
              itz.Controls.Add(cell_text)
              ro.Cells.Add(itz)
              ro.CssClass = "client seperator_tr"
              contact_client.Rows.Add(ro)

            End If
          Next


        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & error_string)
        End If
        masterPage.display_error()
      End If

      ' get the contact info
      If source = "JETNET" Then
        Dim AppendingCounter As Integer = 0
        aTempTable = masterPage.aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(idnum, 0)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              counter = counter + 1
              If Session.Item("localSubscription").crmAerodexFlag = True And r("act_name") = "Exclusive Broker" Then

              Else
                Dim ro As New TableRow
                Dim itz As New TableCell
                ro.VerticalAlign = VerticalAlign.Top
                itz.VerticalAlign = VerticalAlign.Top

                cell_text = New Label
                act_name = IIf(Not IsDBNull(r("act_name")), r("act_name"), "")
                acref_owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
                comp_id = IIf(Not IsDBNull(r("acref_comp_id")), r("acref_comp_id"), 0)
                comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")

                contact_id = IIf(Not IsDBNull(r("acref_contact_id")), r("acref_contact_id"), 0)

                contact_first_name = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
                contact_last_name = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                comp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
                comp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                comp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                strContact = ""


                ro = New TableRow
                itz = New TableCell
                ro.VerticalAlign = VerticalAlign.Top
                itz.VerticalAlign = VerticalAlign.Top
                strContact = ""
                strContact = strContact & "<br /><a href='#' onclick=""javascript:load('company_spec.aspx?source=JETNET&company_ID=" & comp_id & "','','scrollbars=yes,menubar=no,height=700,width=930,resizable=yes,toolbar=no,location=no,status=no');"">" & DisplayRecentCompanyNote(comp_id, "JETNET") & "</a>"

                cell_text = New Label
                cell_text.Text = strContact
                itz.Width = 20
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)

                strContact = ""

                If act_name = "Exclusive Broker" Then
                  strContact = strContact & "<b class='small_purple'>" & act_name & "</b>"
                ElseIf act_name = "Lessee" Then
                  strContact = "<span class='small_orange'>Lessee</span>"
                ElseIf act_name <> "" Then
                  strContact = strContact & "<span class='bold_small'>" & act_name & "</span>"
                  If act_name = "Co-Owner" Or act_name = "Fractional Owner" Then
                    strContact = strContact & "<em class='small'>(" & FormatNumber(acref_owner_percentage, 2) & "%)</em> "
                  End If
                End If


                strContact = strContact & "<br />"

                If comp_name <> "" Then
                  strContact = strContact & "<a href='details.aspx?comp_ID=" & comp_id & "&type=1&source=JETNET' class='small'>" & comp_name & "</a><br /><span class='smaller'>"
                End If

                If comp_city <> "" Then
                  strContact = strContact & comp_city & " "
                End If
                If comp_state <> "" Then
                  strContact = strContact & comp_state & " "
                End If
                If comp_country <> "" Then
                  strContact = strContact & comp_country
                End If


                If comp_id <> 0 Then
                  'Changed on 1/16/2013. 
                  'This small block is going to basically figure out your phone number for a contact or a 
                  'and display it on the aircraft contact card.
                  'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                  PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, 0, source, 0)

                  If Not IsNothing(PhoneTable) Then
                    If PhoneTable.Rows.Count > 0 Then
                      'your phone number was found here.
                      phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                    Else
                      'no phone number was found here.
                      phone_number = ""
                    End If
                  Else 'error logging.
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                    End If
                  End If
                End If

                If phone_number <> "" Then
                  strContact = strContact & "<br /><span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
                End If
                strContact = strContact & "</span>"

                If contact_id <> 0 Then
                  'Changed on 1/16/2013. 
                  'This small block is going to basically figure out your phone number for a contact or a 
                  'and display it on the aircraft contact card.
                  'If there is a contact ID, it's going to call the function based on a contact, if not - a company.
                  PhoneTable = masterPage.aclsData_Temp.GetPhoneNumbers(comp_id, contact_id, source, 0)

                  If Not IsNothing(PhoneTable) Then
                    If PhoneTable.Rows.Count > 0 Then
                      'your phone number was found here.
                      phone_number = PhoneTable.Rows(0).Item("pnum_type") & ": " & PhoneTable.Rows(0).Item("pnum_number")
                    Else
                      'no phone number was found here.
                      phone_number = ""
                    End If
                  Else 'error logging.
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() Phone Number - " & error_string)
                    End If
                  End If
                End If

                cell_text = New Label
                itz = New TableCell
                cell_text.Text = strContact
                itz.Controls.Add(cell_text)
                itz.Width = "225"
                ro.Cells.Add(itz)
                contact_jetnet.Rows.Add(ro)

                cell_text = New Label
                itz = New TableCell
                itz.Width = 10
                cell_text.Text = "&nbsp;"
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)
                strContact = ""

                strContact = "<br />"
                If contact_first_name <> "" Then
                  strContact = strContact & "<a href='details.aspx?comp_ID=" & comp_id & "&contact_ID=" & contact_id & "&source=JETNET&type=1'><span class='small'>" & contact_first_name & " " & contact_last_name & "</span></a>"

                  If contact_title <> "" Then
                    strContact = strContact & "<br /><span class='smaller'>" & contact_title & "</em></span>"
                  End If
                  If phone_number <> "" Then
                    'If contact_title <> "" Then
                    strContact = strContact & "<br />"
                    'Else
                    '    strContact = strContact & "&nbsp;"
                    'End If
                    strContact = strContact & "<span class='smaller' style='color:#852020;'>" & phone_number & "</span>&nbsp;"
                  End If
                End If



                cell_text = New Label
                itz = New TableCell
                cell_text.Text = strContact
                itz.Controls.Add(cell_text)
                ro.Cells.Add(itz)
                ro.CssClass = "seperator_tr"

                contact_jetnet.Rows.Add(ro)
              End If
            Next
          End If
          ' dump the datatable
          aTempTable.Dispose()
          aTempTable = Nothing
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & error_string)
          End If
          masterPage.display_error()
        End If
      End If

      Dim con As New Panel
      'con.Height = 170
      ' con.CssClass = "card_overflow" '"card_overflow_long"
      aircraft_contact.Visible = True
      aircraft_contact_details.Visible = True
      contact_add.Visible = False
      contact_jetnet.CssClass = "seperator"
      contact_client.CssClass = "seperator"
      contact_jetnet.CellPadding = 0
      contact_client.CellPadding = 0
      contact_jetnet.CellSpacing = 0
      contact_jetnet.CellSpacing = 0
      contact_jetnet.EnableViewState = True
      contact_client.EnableViewState = True

      If reorderClient Then
        con.Controls.Add(contact_jetnet)
        con.Controls.Add(contact_client)
      Else
        con.Controls.Add(contact_client)
        con.Controls.Add(contact_jetnet)
      End If

      con.EnableViewState = True
      aircraft_contact_details.Controls.Clear()
      aircraft_contact_details.Controls.Add(con)

      If Session.Item("localUser").crmEvo = False Then

        folders.Controls.Clear()

        Select Case masterPage.TypeOfListing
          Case 1
            'Company
            If masterPage.Listing_ContactID <> 0 Then
              Select Case masterPage.ListingSource
                Case "JETNET"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, masterPage.Listing_ContactID, 0, 0, 0, 2, masterPage.aclsData_Temp))
                Case "CLIENT"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, masterPage.Listing_ContactID, 0, 0, 2, masterPage.aclsData_Temp))
              End Select
            Else 'No Contact 
              Select Case masterPage.ListingSource
                Case "JETNET"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", masterPage.ListingID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
                Case "CLIENT"
                  folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, masterPage.ListingID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
              End Select
            End If
          Case 3
            Select Case masterPage.ListingSource
              Case "JETNET"
                folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", 0, 0, 0, 0, masterPage.ListingID, 0, 3, masterPage.aclsData_Temp))
              Case "CLIENT"
                folders.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, 0, 0, 0, 0, masterPage.ListingID, 3, masterPage.aclsData_Temp))
            End Select
        End Select
        folders_tab.Visible = True
        folders.Visible = True
        Dim check As CheckBoxList = folders.FindControl("personal_folder_ids")

        If Not IsNothing(check) And folders.Visible = True Then
          save_folder_top.Visible = True
          save_folder_bottom.Visible = True
        End If

        check = New CheckBoxList
        check = folders.FindControl("folder_ids")
        If Not IsNothing(check) And folders.Visible = True Then
          save_folder_top.Visible = True
          save_folder_bottom.Visible = True
        End If
        folders_tab.Visible = True
      Else
        aircraft_contact_add.Visible = False
      End If




      '  aircraft_contact_tab.Visible = True


    Catch ex As Exception
      error_string = "ContactCard.ascx.vb - Fill_Contact_Info_AC() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub
#End Region


#Region "Display Button Handlers - Contacts, Companies, AC, View All, etc"
  'Sub details(ByVal sender As Object, ByVal e As System.EventArgs)
  '    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
  '    Try
  '        RaiseEvent Change_Display(sender.commandName, 1)
  '    Catch ex As Exception
  '        error_string = "ContactCard.ascx.vb - details() - " & ex.Message
  '        masterPage.LogError(error_string)
  '    End Try
  'End Sub
  'Sub details_con(ByVal sender As Object, ByVal e As System.EventArgs)
  '    Try
  '        RaiseEvent Change_Display(sender.commandName, 2)
  '    Catch ex As Exception
  '        error_string = "ContactCard.ascx.vb - details_con() - " & ex.Message
  '        LogError(error_string)
  '    End Try
  'End Sub
  Sub dispDetails(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Select Case (e.CommandName)
        Case "details"
          Dim id As Integer = Convert.ToInt32(e.Item.Cells(0).Text)
          RaiseEvent Change_ContactID(id)
        Case "remove"
          Dim id As Integer = Convert.ToInt32(e.Item.Cells(0).Text)
          'check to see if it has notes attached.
          aTempTable = masterPage.aclsData_Temp.Get_Local_Notes_Client_Contact(id)

          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows
                Dim aclsLocal_Notes As New clsLocal_Notes
                'if there are references, update them to zero
                aclsLocal_Notes.lnote_jetnet_ac_id = r("lnote_jetnet_ac_id")
                aclsLocal_Notes.lnote_client_ac_id = r("lnote_client_ac_id")
                aclsLocal_Notes.lnote_jetnet_comp_id = r("lnote_jetnet_comp_id")
                aclsLocal_Notes.lnote_client_comp_id = r("lnote_client_comp_id")
                aclsLocal_Notes.lnote_client_contact_id = 0
                aclsLocal_Notes.lnote_jetnet_contact_id = r("lnote_jetnet_contact_id")
                aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")
                aclsLocal_Notes.lnote_document_flag = r("lnote_document_flag")
                aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
                aclsLocal_Notes.lnote_status = r("lnote_status")
                aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")

                aclsLocal_Notes.lnote_note = r("lnote_note")
                aclsLocal_Notes.lnote_id = r("lnote_id")
                aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
                aclsLocal_Notes.lnote_action_date = Now() ' DB requires some value
                aclsLocal_Notes.lnote_user_login = r("lnote_user_login") ' DB requires a string value greater than 0
                aclsLocal_Notes.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)
                aclsLocal_Notes.lnote_notecat_key = r("lnote_notecat_key")
                aclsLocal_Notes.lnote_user_id = r("lnote_user_id")
                aclsLocal_Notes.lnote_schedule_start_date = r("lnote_schedule_start_date")
                aclsLocal_Notes.lnote_schedule_end_date = r("lnote_schedule_end_date")

                If masterPage.aclsData_Temp.update_localNote(aclsLocal_Notes) = True Then
                Else
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
                  End If
                  masterPage.display_error()
                End If
              Next

            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
            End If
            masterPage.display_error()
          End If

          'Check on AC References.
          aTempTable = masterPage.aclsData_Temp.Get_Client_Aircraft_Reference_ContactID(id)

          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows

                'Response.Write(r("cliacref_id") & "<br />")

                Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
                aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = r("cliacref_comp_id")
                aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")

                aclsInsert_Client_Aircraft_Reference.cliacref_id = r("cliacref_id")
                aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = r("cliacref_contact_type")
                aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
                aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = r("cliacref_jetnet_ac_id")
                aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")
                aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = r("cliacref_operator_flag")
                aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = r("cliacref_owner_percentage")
                aclsInsert_Client_Aircraft_Reference.cliacref_business_type = r("cliacref_business_type")
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

                If masterPage.aclsData_Temp.Update_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                Else
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
                  End If
                  masterPage.display_error()
                End If
              Next
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
            End If
            masterPage.display_error()
          End If

          If masterPage.aclsData_Temp.Delete_Client_Contact(id) = 1 Then

            Try
              'This is going to fill the contacts gridview on the right.
              aTempTable = masterPage.aclsData_Temp.GetContacts(Session("ListingID"), Session("ListingSource"), "Y", 0)
              ' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  ' set it to the datagrid 
                  contacts_gv.DataSource = aTempTable
                  contacts_gv.DataBind()
                Else
                  contact_no_results.Visible = True
                  contacts_gv.Visible = False
                  contact_no_results.Text = "<p align='center'>No Contacts Currently</p>"
                  contact_no_results.ForeColor = Drawing.Color.Red
                  contact_no_results.Font.Bold = True
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
                End If
                masterPage.display_error()
              End If
            Catch ex As Exception
              error_string = "ContactCard.ascx.vb - dispDetails() Remove - " & ex.Message
              masterPage.LogError(error_string)
            End Try

          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("ContactCard.ascx.vb - dispDetails() Remove - " & error_string)
            End If
            masterPage.display_error()
          End If

      End Select
    Catch ex As Exception
      error_string = "ContactCard.ascx.vb - dispDetails() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

#End Region
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible = True Then
      If Session.Item("crmUserLogon") = True Then
        'If Not Page.IsPostBack Then
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Select Case masterPage.TypeOfListing
          Case 1
            fill_Contact_Info_Company(masterPage.Listing_ContactID, masterPage.ListingID, masterPage.ListingSource, masterPage.TypeOfListing, True)
          Case 3
            fill_Contact_Info_AC_New(masterPage.ListingID, masterPage.ListingSource, masterPage.TypeOfListing)
        End Select
        'End If
        masterPage.Write_Javascript_Out()
      End If
    End If
  End Sub

  'This fills up the flight table.
  Public Sub Consume_Aircraft_Data(ByVal Aircraft_Table As clsClient_Aircraft, ByVal tempTable As DataTable)
    Aircraft_Data = Aircraft_Table
    If Not IsDate(flight_tab_time.Text) Then
      Dim Flight_Table As New DataTable
      Dim masterPage As main_site = DirectCast(Page.Master, main_site)

      Dim flight_data_temp As New flightDataFunctions
      flight_data_temp.serverConnectStr = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
      flight_data_temp.clientConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")


      Dim CurrentACJetnetID As Long = 0
      Dim FAARegNo As String = ""
      Dim TemporaryRegTable As New DataTable
      Dim ErrorString As String = ""

      If masterPage.ListingSource = "CLIENT" Then
        CurrentACJetnetID = Aircraft_Data.cliaircraft_jetnet_ac_id
        TemporaryRegTable = masterPage.aclsData_Temp.GetJETNET_AC_NAME(CurrentACJetnetID, ErrorString)

        'Grab Jetnet's Reg #
        If Not IsNothing(TemporaryRegTable) Then
          If TemporaryRegTable.Rows.Count > 0 Then
            If Not IsDBNull(TemporaryRegTable.Rows(0).Item("ac_reg_nbr")) Then
              If Not String.IsNullOrEmpty(TemporaryRegTable.Rows(0).Item("ac_reg_nbr")) Then
                FAARegNo = TemporaryRegTable.Rows(0).Item("ac_reg_nbr")
              End If
            End If
          End If
        End If

      Else
        CurrentACJetnetID = Aircraft_Data.cliaircraft_id
        FAARegNo = Aircraft_Data.cliaircraft_reg_nbr
      End If

      If CurrentACJetnetID > 0 Then
        Dim tmpFlightDataTable As New DataTable




        ' checks for cleaned flight data
        If flight_data_temp.checkForFAAFlightData(FAARegNo, CurrentACJetnetID, False, True) Then

          tmpFlightDataTable = flight_data_temp.getFAAFlightData(FAARegNo, CurrentACJetnetID, Nothing, Nothing)

          If Not IsNothing(tmpFlightDataTable) Then
            flight_summary_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, aircraft_flight_tab.HeaderText, "last_year", "", CurrentACJetnetID, DateAdd(DateInterval.Day, -90, Date.Now.Date), True, FAARegNo, False)
          End If
        ElseIf flight_data_temp.IS_ON_BLOCKED_LIST(FAARegNo) = True Then
          'if its blocked, try again , for unclean
          If flight_data_temp.checkForFAAFlightData(FAARegNo, CurrentACJetnetID, True, True) Then

            tmpFlightDataTable = flight_data_temp.getFAAFlightData(FAARegNo, CurrentACJetnetID, Nothing, Nothing, True)

            If Not IsNothing(tmpFlightDataTable) Then
              flight_summary_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, aircraft_flight_tab.HeaderText, "last_year", "", CurrentACJetnetID, DateAdd(DateInterval.Day, -90, Date.Now.Date), True, FAARegNo, True)
            End If

          End If

        ElseIf Trim(FAARegNo) = "" Then
          tmpFlightDataTable = flight_data_temp.getFAAFlightData(FAARegNo, CurrentACJetnetID, Nothing, Nothing, True)
          If Not IsNothing(tmpFlightDataTable) Then
            flight_summary_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, aircraft_flight_tab.HeaderText, "last_year", "", CurrentACJetnetID, DateAdd(DateInterval.Day, -90, Date.Now.Date), True, FAARegNo, True)
          End If
        End If

        aircraft_flight_tab.HeaderText = "FLIGHTS"
        flight_summary_label.CssClass = "FAA_Data_Table"

        If InStr(flight_summary_label.Text, "</table></table>") > 0 Then
          flight_summary_label.Text = "<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border=""0""><tr><td align='left' valign='top'>" & flight_summary_label.Text
        End If

      End If
    End If
  End Sub

  Private Sub save_folder_bottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_folder_bottom.Click, save_folder_top.Click
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    clsGeneral.clsGeneral.saveFolder(Nothing, masterPage, folders)
    'If masterPage.Listing_ContactID <> 0 Then
    '  Response.Redirect("details.aspx?contact_ID=" & masterPage.Listing_ContactID & "&comp_ID=" & masterPage.ListingID & "&type=" & masterPage.TypeOfListing & "&source=" & masterPage.ListingSource, False)
    'Else
    '  Response.Redirect("details.aspx?comp_ID=" & masterPage.ListingID & "&type=" & masterPage.TypeOfListing & "&source=" & masterPage.ListingSource, False)
    'End If
    save_folder_bottom.Visible = False
    save_folder_top.Visible = False
    tab_info_container.ActiveTab = folders_tab
    'folders_saved_message.Visible = True
  End Sub

  'Private Sub tab_info_container_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tab_info_container.ActiveTabChanged
  '    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
  '    Select Case tab_info_container.ActiveTab.ID
  '        Case "ac_picture_tab"
  '            clsGeneral.clsGeneral.Fill_Aircraft_Pictures(masterPage.ListingID, picture_label, masterPage)
  '            tab_info_container.ActiveTab.ID = "ac_picture_tab"
  '    End Select
  'End Sub

  Public Function DisplayRecentCompanyNote(ByVal companyID As Long, ByVal source As String) As String
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim NotesTable As New DataTable


    Dim ReturnString As String = "<img src=""images/binoculars.png"" alt=""View More Information"" border=""0"" title=""View More Information"" />"
    NotesTable = masterPage.aclsData_Temp.DUAL_Notes_LIMIT("COMP", companyID, "A", source, Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()), "lnote_entry_date desc", 1)
    Try
      If Not IsNothing(NotesTable) Then
        If NotesTable.Rows.Count > 0 Then
          Dim dateOfNote As New Date
          Dim lnote_user_name As String = ""
          Dim lnote_entry_date As New Date

          'Set up the entry date.
          If Not IsDBNull(NotesTable.Rows(0).Item("lnote_entry_date")) Then
            dateOfNote = NotesTable.Rows(0).Item("lnote_entry_date")
            lnote_entry_date = NotesTable.Rows(0).Item("lnote_entry_date")
          Else
            dateOfNote = Now()
            lnote_entry_date = Now()
          End If

          'Set up the timespan to check how many days old: 
          Dim ts As TimeSpan = Now().Subtract(dateOfNote)

          'set up the username: 
          If Not IsDBNull(NotesTable.Rows(0).Item("lnote_user_name")) Then
            lnote_user_name = "(By: " & NotesTable.Rows(0).Item("lnote_user_name") & ") "
          End If

          'Timezone offset. 
          lnote_entry_date = DateAdd("h", Session("timezone_offset"), lnote_entry_date)

          If ts.Days <= 180 Then
            ReturnString = "<img src=""images/binoculars_green.png"" alt=""" & lnote_entry_date & " - " & lnote_user_name & NotesTable.Rows(0).Item("lnote_note").ToString & """ border=""0"" title=""" & lnote_entry_date & " - " & lnote_user_name & NotesTable.Rows(0).Item("lnote_note").ToString & """ />"
          Else
            ReturnString = "<img src=""images/binoculars.png"" alt=""" & lnote_entry_date & " - " & lnote_user_name & NotesTable.Rows(0).Item("lnote_note").ToString & """ border=""0"" title=""" & lnote_entry_date & " - " & lnote_user_name & NotesTable.Rows(0).Item("lnote_note").ToString & """ />"
          End If

        End If
      Else 'Datalayer error:
        If masterPage.aclsData_Temp.class_error <> "" Then
          masterPage.LogError("1. ContactCard.ascx.vb - DisplayRecentCompanyNote(ByVal companyID: " & companyID.ToString & " As Long, ByVal source: " & source & " As String) As String - " & masterPage.aclsData_Temp.class_error)
        End If
        masterPage.display_error()
      End If
    Catch ex As Exception
      error_string = "2. ContactCard.ascx.vb - DisplayRecentCompanyNote(ByVal companyID: " & companyID.ToString & " As Long, ByVal source: " & source & " As String) As String - " & ex.Message
      masterPage.LogError(error_string)
    Finally
      NotesTable.Dispose()
      NotesTable = Nothing
    End Try

    Return ReturnString
  End Function
End Class