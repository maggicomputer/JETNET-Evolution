Imports System.IO
Imports System.Runtime.CompilerServices
Imports crmWebClient.clsGeneral
Partial Public Class _details
  Inherits System.Web.UI.Page
  Dim table As DataTable

  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used 
  Dim back_button As Boolean = False
  Public Aircraft_Data As New clsClient_Aircraft

#Region "Handles Sync Date Displays"

  Private Sub companyCard_Synch_Date(ByVal Synch_Type As String, ByVal sync_display As Label) Handles companyCard.Synch_Date
    Try
      sync_display.Text = (Date_Sync_Display(Synch_Type))
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - companyCard_Synch_Date() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub aircraftCard_Synch_Date(ByVal Synch_Type As String, ByVal sync_display As Label) Handles aircraftCard.Synch_Date
    Try

      sync_display.Text = (Date_Sync_Display(Synch_Type))
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - aircraftCard_Synch_Date() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Public Function Date_Sync_Display(ByVal x As String) As String
    Date_Sync_Display = ""
    Dim standalone As Boolean = False
    '------------------------This is for the Date Synch Display------------------------------------------------------
    Try
      'If frequency is Live the system will work exactly as it does today. If monthly or weekly we should not display the last synchronized date stuff since this will not be accurate.
      If Session.Item("localUser").crmEvo = True And Master.ListingSource = "JETNET" Then 'If an EVO user
        Date_Sync_Display = "<table class='float_left'><tr><td align='left' valign='top'><em class='tiny'><img src='images/evo.png' alt='JETNET RECORD' /></td><td align='left' valign='middle'><b><em class='tiny'>Evolution Live</em></b></em></td></tr></table>"
      Else

        If Session.Item("ListingSource") = "JETNET" Then
          Date_Sync_Display = "<em class='tiny'><img src='images/evo.png' alt='JETNET RECORD' class='float_left'/>" 'Last Synchronized:<br />" & (aTempTable.Rows(0).Item("jsync_date")) & "</em>"
        Else
          Date_Sync_Display = "<img src='images/client.png' alt='CLIENT RECORD' class='float_left'/>"
        End If

        aTempTable = Nothing
      End If
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - Date_Sync_Display() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
    '--------------------------------------------End Synch Display--------------------------------------------
  End Function
#End Region
#Region "Customized Control Events"
  Private Sub companyCard_Next_Prev_Btn(ByVal Command As String) Handles companyCard.Next_Prev_Btn, aircraftCard.Next_Prev_Btn
    Try

      Dim split_me As Array = Split(Command, "|")

      If split_me(2) = "" Then
        Master.Listing_ContactID = 0
      Else
        Master.Listing_ContactID = split_me(2)
      End If

      Master.ListingID = split_me(0)
      Master.ListingSource = split_me(1)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = 'details.aspx';", True)
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - companyCard_Next_Prev_Btn() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try

  End Sub
  Private Sub contactCard_Change_ContactID(ByVal x As Integer) Handles contactCard.Change_ContactID
    Try
      Session.Item("ContactID") = x
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = 'details.aspx';", True)
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - contactCard_Change_ContactID() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub contactCard_SetUpDisplay() Handles contactCard.SetUpDisplay, aircraftCard.SetUpDisplay, companyCard.SetUpDisplay
    set_up_display()
    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = 'details.aspx';", True)
  End Sub
  Private Sub companyCard_SetOtherID(ByVal id As Integer) Handles companyCard.SetOtherID, aircraftCard.SetOtherID
    Try
      Master.OtherID = id
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - companyCard_SetOtherID() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub contactCard_Change_Display(ByVal x As String, ByVal type As Integer) Handles contactCard.Change_Display
    Try
      Select Case type
        Case 1
          Dim list As Array = Split(x, "|")
          Master.TypeOfListing = 1
          Master.ListingID = list(0)
          Master.ListingSource = list(1)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = 'details.aspx';", True)
        Case 2
          Dim list As Array = Split(x, "|")
          Master.TypeOfListing = 1
          Master.ListingID = list(0)
          Master.Listing_ContactID = list(1)
          Master.ListingSource = list(2)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location = 'details.aspx';", True)
      End Select
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - contactCard_Change_Display - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
#End Region
#Region "Page Events"

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Try

      Response.Cache.SetCacheability(HttpCacheability.NoCache)
      Response.Cache.SetNoStore()
      Response.Expires = -1

      Session("export_info") = ""
      Master.Header_Bar = False

      set_up_display()
      'make_recently_viewed_Cookies()

    Catch ex As Exception
      Master.error_string = "details.aspx.vb - export_Load() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Private Sub export_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
    Session("export_info") = ""
    AddHandler aircraftCard.ShareAircraftDataTable, AddressOf Share_Aircraft_Data_Between_Controls
    AddHandler aircraftCard.ShareNotesDataTable, AddressOf Share_AC_Notes_Data_Between_Controls ' 
    AddHandler aircraftCard.ShareActionDataTable, AddressOf Share_AC_Action_Data_Between_Controls
    AddHandler aircraftCard.ShareDocumentDataTable, AddressOf Share_AC_Document_Data_Between_Controls
    AddHandler aircraftCard.ShareProspectDataTable, AddressOf Share_AC_Prospect_Data_Between_Controls
    AddHandler aircraftCard.ShareValueDataTable, AddressOf Share_AC_Value_Data_Between_Controls
    AddHandler Master.SendToTabs, AddressOf ShareWithActionTabShowJETNETCLIENT
    AddHandler companyCard.ShareNotesDataTable, AddressOf Share_Company_Notes_Data_Between_Controls ' 
    AddHandler companyCard.ShareActionDataTable, AddressOf Share_Company_Action_Data_Between_Controls
    AddHandler companyCard.ShareDocumentDataTable, AddressOf Share_Company_Document_Data_Between_Controls
    AddHandler companyCard.ShareOppDataTable, AddressOf Share_Company_Opp_Data_Between_Controls
    AddHandler companyCard.ShareProspectDataTable, AddressOf Share_Company_Prospect_Data_Between_Controls
  End Sub


#End Region
  Public Sub ShareWithActionTabShowJETNETCLIENT(ByVal show_jetnet As CheckBox)

    Aircraft_Tabs1.Show_Jetnet_Tabs(show_jetnet)
  End Sub

  Public Sub Share_Aircraft_Data_Between_Controls(ByVal Returned_Aircraft_Data As clsClient_Aircraft, ByVal aircraft_Table As DataTable)
    ViewState("Aircraft_Data") = aircraft_Table
    Aircraft_Data = Returned_Aircraft_Data
    Aircraft_Tabs1.Consume_Aircraft_Data(Returned_Aircraft_Data, aircraft_Table)
    contactCard.Consume_Aircraft_Data(Returned_Aircraft_Data, aircraft_Table)
  End Sub
  Public Sub Share_AC_Notes_Data_Between_Controls(ByVal Notes_Table)
    ViewState("Notes_Data") = Notes_Table
    Aircraft_Tabs1.Consume_Notes_Data(Notes_Table)
  End Sub
  Public Sub Share_AC_Prospect_Data_Between_Controls(ByVal Prospect_Table)
    ViewState("Prospect_Data") = Prospect_Table
    Aircraft_Tabs1.Consume_Prospect_Data(Prospect_Table)
  End Sub
  Public Sub Share_AC_Value_Data_Between_Controls(ByVal Value_Table)
    ViewState("Value_Data") = Value_Table
    Aircraft_Tabs1.Consume_Value_Data(Value_Table)
  End Sub
  Public Sub Share_AC_Action_Data_Between_Controls(ByVal Action_Table)
    ViewState("Action_Data") = Action_Table
    Aircraft_Tabs1.Consume_Action_Data(Action_Table)
  End Sub
  Public Sub Share_AC_Document_Data_Between_Controls(ByVal Document_Table)
    ViewState("Document_Data") = Document_Table
    Aircraft_Tabs1.Consume_Document_Data(Document_Table)
  End Sub
  Public Sub Share_Company_Notes_Data_Between_Controls(ByVal Notes_Table)
    ViewState("Notes_Data") = Notes_Table
    Company_Tabs1.Consume_Notes_Data(Notes_Table)
  End Sub
  Public Sub Share_Company_Action_Data_Between_Controls(ByVal Action_Table)
    ViewState("Action_Data") = Action_Table
    Company_Tabs1.Consume_Action_Data(Action_Table)
  End Sub
  Public Sub Share_Company_Document_Data_Between_Controls(ByVal Document_Table)
    ViewState("Document_Data") = Document_Table
    Company_Tabs1.Consume_Document_Data(Document_Table)
  End Sub
  Public Sub Share_Company_Opp_Data_Between_Controls(ByVal Document_Table)
    ViewState("Opp_Data") = Document_Table
    Company_Tabs1.Consume_Opp_Data(Document_Table)
  End Sub
  Public Sub Share_Company_Prospect_Data_Between_Controls(ByVal Prospect_Table)
    ViewState("Prospect_Data") = Prospect_Table
    Company_Tabs1.Consume_Prospect_Data(Prospect_Table)
  End Sub
  'Public Sub Share_Company_Email_Data_Between_Controls(ByVal Email_Table)
  '    ViewState("Email_Data") = Email_Table
  '    Company_Tabs1.Consume_Email_Data(Email_Table)
  'End Sub

  '#Region "Function to Make Recently Viewed Company/Aircraft/Contact Cookies!"
  '    Sub make_recently_viewed_Cookies()
  '        Try
  '            If Not Page.IsPostBack Then
  '                Dim _companiesCookies As HttpCookie = Request.Cookies("companies")
  '                Dim _aircraftCookies As HttpCookie = Request.Cookies("aircraft")
  '                Dim _contactsCookies As HttpCookie = Request.Cookies("contacts")
  '                Dim stored_id As String = ""
  '                Dim stored_source As String = ""

  '                Select Case Master.TypeOfListing
  '                    Case 1
  '                        If _companiesCookies IsNot Nothing Then
  '                            stored_id = _companiesCookies("ID")
  '                            stored_source = _companiesCookies("SOURCE")

  '                            'Let's do one thing at a time. First we need to only store 5 companies. 
  '                            'Also no duplicates.. 

  '                            Dim id_array As Array = Split(stored_id, "|")
  '                            Dim source_array As Array = Split(stored_source, "|")
  '                            'ubound needs to be less than 4 to have 5 companies stored.

  '                            Dim exists As Integer = InStr(stored_id, CStr(Master.ListingID))

  '                            If UBound(id_array) < 4 Then

  '                                If exists = 0 Then
  '                                    Response.Cookies("companies").Values("ID") = Master.ListingID & "|" & stored_id
  '                                    Response.Cookies("companies").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                    Response.Cookies("companies").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                Else
  '                                    Dim topnumber As Integer = UBound(id_array)


  '                                    stored_id = ""
  '                                    stored_source = ""

  '                                    For i As Integer = 0 To topnumber
  '                                        If id_array(i) <> CStr(Master.ListingID) Then
  '                                            stored_id = stored_id & id_array(i) & "|"
  '                                            stored_source = stored_source & source_array(i) & "|"
  '                                        End If
  '                                    Next


  '                                    If stored_id <> "" Then
  '                                        stored_id = UCase(stored_id.TrimEnd("|"))
  '                                    End If

  '                                    If stored_source <> "" Then
  '                                        stored_source = UCase(stored_source.TrimEnd("|"))
  '                                    End If

  '                                    Response.Cookies("companies").Values("ID") = Master.ListingID & "|" & stored_id
  '                                    Response.Cookies("companies").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                    Response.Cookies("companies").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                End If

  '                            Else
  '                                'Store the ubound of the array.
  '                                Dim topnumber As Integer = UBound(id_array)
  '                                'rewrite the cookie with the last 5 in array.

  '                                If exists = 0 Then
  '                                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1)
  '                                    stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1)
  '                                Else
  '                                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1) & "|" & id_array(topnumber)
  '                                    stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1) & "|" & source_array(topnumber)

  '                                End If


  '                                id_array = Split(stored_id, "|")
  '                                source_array = Split(stored_source, "|")
  '                                topnumber = UBound(id_array)
  '                                stored_id = ""
  '                                stored_source = ""

  '                                For i As Integer = 0 To topnumber
  '                                    If id_array(i) <> CStr(Master.ListingID) Then
  '                                        stored_id = stored_id & id_array(i) & "|"
  '                                        stored_source = stored_source & source_array(i) & "|"
  '                                    End If
  '                                Next

  '                                If stored_id <> "" Then
  '                                    stored_id = UCase(stored_id.TrimEnd("|"))
  '                                End If

  '                                If stored_source <> "" Then
  '                                    stored_source = UCase(stored_source.TrimEnd("|"))
  '                                End If

  '                                Response.Cookies("companies").Values("ID") = Master.ListingID & "|" & stored_id
  '                                Response.Cookies("companies").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                Response.Cookies("companies").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                            End If

  '                        Else
  '                            Dim aCookie As New HttpCookie("companies")
  '                            aCookie.Values("ID") = Master.ListingID
  '                            aCookie.Values("SOURCE") = Master.ListingSource
  '                            aCookie.Values("USER") = Session.Item("localUser").crmLocalUserID
  '                            aCookie.Expires = DateTime.Now.AddDays(10)
  '                            Response.Cookies.Add(aCookie)
  '                        End If


  '                        If Master.Listing_ContactID <> 0 Then
  '                            If _contactsCookies IsNot Nothing Then
  '                                stored_id = _contactsCookies("ID")
  '                                stored_source = _contactsCookies("SOURCE")

  '                                'Let's do one thing at a time. First we need to only store 5 companies. 
  '                                'Also no duplicates.. 

  '                                Dim id_array As Array = Split(stored_id, "|")
  '                                Dim source_array As Array = Split(stored_source, "|")
  '                                'ubound needs to be less than 4 to have 5 companies stored.

  '                                Dim exists As Integer = InStr(stored_id, CStr(Master.Listing_ContactID))

  '                                If UBound(id_array) < 4 Then

  '                                    If exists = 0 Then
  '                                        Response.Cookies("contacts").Values("ID") = Master.ListingID & "," & Master.Listing_ContactID & "|" & stored_id
  '                                        Response.Cookies("contacts").Values("SOURCE") = Master.ListingSource & "," & Master.ListingSource & "|" & stored_source
  '                                        Response.Cookies("contacts").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                    Else
  '                                        Dim topnumber As Integer = UBound(id_array)


  '                                        stored_id = ""
  '                                        stored_source = ""

  '                                        For i As Integer = 0 To topnumber
  '                                            If id_array(i) <> CStr(Master.ListingID) & "," & CStr(Master.Listing_ContactID) Then
  '                                                stored_id = stored_id & id_array(i) & "|"
  '                                                stored_source = stored_source & source_array(i) & "|"
  '                                            End If
  '                                        Next


  '                                        If stored_id <> "" Then
  '                                            stored_id = UCase(stored_id.TrimEnd("|"))
  '                                        End If

  '                                        If stored_source <> "" Then
  '                                            stored_source = UCase(stored_source.TrimEnd("|"))
  '                                        End If

  '                                        Response.Cookies("contacts").Values("ID") = Master.ListingID & "," & Master.Listing_ContactID & "|" & stored_id
  '                                        Response.Cookies("contacts").Values("SOURCE") = Master.ListingSource & "," & Master.ListingSource & "|" & stored_source
  '                                        Response.Cookies("contacts").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                    End If

  '                                Else
  '                                    'Store the ubound of the array.
  '                                    Dim topnumber As Integer = UBound(id_array)
  '                                    'rewrite the cookie with the last 5 in array.

  '                                    If exists = 0 Then
  '                                        stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1)
  '                                        stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1)
  '                                    Else
  '                                        stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1) & "|" & id_array(topnumber)
  '                                        stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1) & "|" & source_array(topnumber)

  '                                    End If


  '                                    id_array = Split(stored_id, "|")
  '                                    source_array = Split(stored_source, "|")
  '                                    topnumber = UBound(id_array)
  '                                    stored_id = ""
  '                                    stored_source = ""

  '                                    For i As Integer = 0 To topnumber
  '                                        If id_array(i) <> CStr(Master.ListingID) & "," & CStr(Master.Listing_ContactID) Then
  '                                            stored_id = stored_id & id_array(i) & "|"
  '                                            stored_source = stored_source & source_array(i) & "|"
  '                                        End If
  '                                    Next

  '                                    If stored_id <> "" Then
  '                                        stored_id = UCase(stored_id.TrimEnd("|"))
  '                                    End If

  '                                    If stored_source <> "" Then
  '                                        stored_source = UCase(stored_source.TrimEnd("|"))
  '                                    End If

  '                                    Response.Cookies("contacts").Values("ID") = Master.ListingID & "," & Master.Listing_ContactID & "|" & stored_id
  '                                    Response.Cookies("contacts").Values("SOURCE") = Master.ListingSource & "," & Master.ListingSource & "|" & stored_source
  '                                    Response.Cookies("contacts").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                End If

  '                            Else
  '                                Dim aCookie As New HttpCookie("contacts")
  '                                aCookie.Values("ID") = Master.ListingID & "," & Master.Listing_ContactID
  '                                aCookie.Values("SOURCE") = Master.ListingSource & "," & Master.ListingSource
  '                                aCookie.Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                aCookie.Expires = DateTime.Now.AddDays(10)
  '                                Response.Cookies.Add(aCookie)
  '                            End If
  '                            Dim _contactsCookiesDIS As HttpCookie = Request.Cookies("contacts")
  '                            'Response.Write(_contactsCookiesDIS("ID") & "<---- IDs<br />")
  '                            'Response.Write(_contactsCookiesDIS("SOURCE") & "<---- SOURCEs<br />")
  '                        End If
  '                    Case 3
  '                        If _aircraftCookies IsNot Nothing Then
  '                            stored_id = _aircraftCookies("ID")
  '                            stored_source = _aircraftCookies("SOURCE")

  '                            'Let's do one thing at a time. First we need to only store 5 companies. 
  '                            'Also no duplicates.. 

  '                            Dim id_array As Array = Split(stored_id, "|")
  '                            Dim source_array As Array = Split(stored_source, "|")
  '                            'ubound needs to be less than 4 to have 5 companies stored.

  '                            Dim exists As Integer = InStr(stored_id, CStr(Master.ListingID))

  '                            If UBound(id_array) < 4 Then

  '                                If exists = 0 Then
  '                                    Response.Cookies("aircraft").Values("ID") = Master.ListingID & "|" & stored_id
  '                                    Response.Cookies("aircraft").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                    Response.Cookies("aircraft").Values("USER") = Session.Item("localUser").crmLocalUserID
  '                                Else
  '                                    Dim topnumber As Integer = UBound(id_array)


  '                                    stored_id = ""
  '                                    stored_source = ""

  '                                    For i As Integer = 0 To topnumber
  '                                        If id_array(i) <> CStr(Master.ListingID) Then
  '                                            stored_id = stored_id & id_array(i) & "|"
  '                                            stored_source = stored_source & source_array(i) & "|"
  '                                        Else

  '                                        End If
  '                                    Next


  '                                    If stored_id <> "" Then
  '                                        stored_id = UCase(stored_id.TrimEnd("|"))
  '                                    End If

  '                                    If stored_source <> "" Then
  '                                        stored_source = UCase(stored_source.TrimEnd("|"))
  '                                    End If

  '                                    Response.Cookies("aircraft").Values("ID") = Master.ListingID & "|" & stored_id
  '                                    Response.Cookies("aircraft").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                    Response.Cookies("aircraft").Values("USER") = Session.Item("localUser").crmLocalUserID


  '                                End If

  '                            Else
  '                                'Store the ubound of the array.
  '                                Dim topnumber As Integer = UBound(id_array)
  '                                'rewrite the cookie with the last 5 in array.

  '                                If exists = 0 Then
  '                                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1)
  '                                    stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1)
  '                                Else
  '                                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1) & "|" & id_array(topnumber)
  '                                    stored_source = source_array(topnumber - 4) & "|" & source_array(topnumber - 3) & "|" & source_array(topnumber - 2) & "|" & source_array(topnumber - 1) & "|" & source_array(topnumber)

  '                                End If


  '                                id_array = Split(stored_id, "|")
  '                                source_array = Split(stored_source, "|")
  '                                topnumber = UBound(id_array)
  '                                stored_id = ""
  '                                stored_source = ""

  '                                For i As Integer = 0 To topnumber
  '                                    If id_array(i) <> CStr(Master.ListingID) Then
  '                                        stored_id = stored_id & id_array(i) & "|"
  '                                        stored_source = stored_source & source_array(i) & "|"
  '                                    End If
  '                                Next

  '                                If stored_id <> "" Then
  '                                    stored_id = UCase(stored_id.TrimEnd("|"))
  '                                End If

  '                                If stored_source <> "" Then
  '                                    stored_source = UCase(stored_source.TrimEnd("|"))
  '                                End If

  '                                Response.Cookies("aircraft").Values("ID") = Master.ListingID & "|" & stored_id
  '                                Response.Cookies("aircraft").Values("SOURCE") = Master.ListingSource & "|" & stored_source
  '                                Response.Cookies("aircraft").Values("USER") = Session.Item("localUser").crmLocalUserID

  '                            End If

  '                        Else
  '                            Dim aCookie As New HttpCookie("aircraft")
  '                            aCookie.Values("ID") = Master.ListingID
  '                            aCookie.Values("SOURCE") = Master.ListingSource
  '                            aCookie.Values("USER") = Session.Item("localUser").crmLocalUserID
  '                            aCookie.Expires = DateTime.Now.AddDays(10)
  '                            Response.Cookies.Add(aCookie)
  '                        End If

  '                End Select



  '            End If
  '            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '        Catch ex As Exception
  '            Master.error_string = "details.aspx.vb - make_recently_viewed_cookies() - " & ex.Message
  '            Master.LogError(Master.error_string)
  '        End Try
  '    End Sub
  '#End Region
#Region "Notes/Events for Tabs"
  'Event for displaying notes from company/aircraft tabs.
  Private Sub Company_Tabs1_Notes(ByVal text As String, ByVal cat_name As String, ByVal main_id As Integer, ByVal cat_id As Integer, ByVal action As Boolean, ByVal label As System.Web.UI.WebControls.Label, ByVal Notes_Data As DataTable) Handles Company_Tabs1.Notes, Aircraft_Tabs1.Notes
    Try

      notes(text, cat_name, main_id, cat_id, action, label, Notes_Data)
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - Company_Tabs1_Notes() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Sub notes(ByVal x As String, ByVal y As String, ByVal idnum As Integer, ByVal cat_key As Integer, ByVal action As Boolean, ByVal c As Label, ByVal Notes_Data As DataTable)
    Try
      If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
        '---------------------------------------------------paging for notes!!-------------------------------------------------
        Dim NotesRowCount As New DataTable

        If Not IsNothing(ViewState("Notes_Data")) Then
          NotesRowCount = DirectCast(ViewState("Notes_Data"), DataTable)
        End If
        Dim fullURL As String = Request.ServerVariables("URL") & "?"
        If Master.TypeOfListing = 1 Then
          fullURL = fullURL & "type=1&comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource
          If Master.Listing_ContactID <> 0 Then
            fullURL = fullURL & "&contactID=" & Master.Listing_ContactID
          End If
        ElseIf Master.TypeOfListing = 3 Then
          fullURL = fullURL & "type=3&ac_ID=" & Master.ListingID & "&source=" & Master.ListingSource
        End If

        Dim startCount As Integer = 0
        Dim endCount As Integer = 10
        Dim showPrevious As Boolean = False
        Dim showNext As Boolean = False
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
              End If
            End If
          End If
        End If

        If startCount <> 0 Then
          showPrevious = True
        End If
        If endCount < NotesRowCount.Rows.Count Then
          showNext = True
        Else
          endCount = NotesRowCount.Rows.Count
        End If

        If cat_key <> 0 Then
          showNext = False
          showPrevious = False
        End If

        'x is part of the other side of the table on the right of notes.
        'Y is text for category
        'Idnum is id number of comp/ac
        'Cat key is cat key
        'Action is true false whether the notes are action planned notes
        'This writes out the notes or actions. 
        Dim actioned As String = ""
        Dim pnl As New Panel
        Dim trans_key As Integer = 99 'Master.what_cat(0, "TRANSACTIONS", True) 'Getting the category key for transactions. 
        Dim typed As String = ""
        Dim linky As New LinkButton
        Dim notes_string As String = "" 'Strings to build the notes
        Dim notes_front As String = "" 'Strings to build the notes
        c.Controls.Clear() 'clearing the control that I'm putting the notes on.
        Dim view As String
        Dim used_id As Integer = 0
        Dim size As String = "height=435,width=860"
        Dim used_source As String = ""
        Dim aError As String = ""
        Dim aircraft_text As String = ""
        Dim frlbl2 As New Label
        Dim beg As New Label
        Dim entry_date As Date = Now()
        Dim schedule_start As Date = Now()
        Dim status As String = "P"
        Dim lnote_category As Integer = 0
        Dim lnote_user_name As String = ""
        Dim lnote_document_name As String = ""
        Dim lnote_client_ac_id As Integer = 0
        Dim lnote_jetnet_ac_id As Integer = 0
        Dim lnote_jetnet_comp_id As Integer = 0
        Dim lnote_client_comp_id As Integer = 0
        Dim lnote_jetnet_amod_id As Integer = 0
        Dim lnote_client_amod_id As Integer = 0
        Dim lnote_client_contact_id As Long = 0
        Dim lnote_jetnet_contact_id As Long = 0
        Dim lnote_opportunity_status As String = ""
        Dim lnote_user_id As Long = 0

        Dim lnote_id As Integer = 0
        Dim lnote_text As String = ""
        Dim lnote_title As String = ""
        Dim lnote_document_flag As String = ""
        Dim frlbl As New Label
        Dim bklbl As New Label
        Dim ending As New Label
        Dim document_display As String = ""
        Dim email_to As String = ""
        Dim email_cc As String = ""
        Dim email_subject As String = ""
        Dim body As String = ""
        Dim NextNote As Integer = 0
        Dim PreviousNote As Integer = 0

        If action = False And (y <> "DOCUMENTS" And y <> "EMAIL" And y <> "OPPORTUNITIES" And y <> "PROSPECT") Then
          typed = "A"
          view = "note"
        ElseIf y = "PROSPECT" Then
          typed = "B"
          view = "prospect"
        ElseIf y = "EMAIL" Then
          typed = "E"
          view = "email"
        ElseIf y = "DOCUMENTS" Then
          typed = "F"
          actioned = "&doc=true"
          view = "documents"
        ElseIf y = "OPPORTUNITIES" Then
          typed = "O"
          view = "opportunity"
        Else
          actioned = "action"
          typed = "P"
          view = "action"
        End If
        If view <> "note" Then
          showPrevious = False
          showNext = False
        End If
        Dim ending_str As String = ""

        If IsNothing(Notes_Data) Then 'Check for error on datahook table
          If Master.aclsData_Temp.class_error <> "" Then
            Master.error_string = Master.aclsData_Temp.class_error 'Save/Log/Display Error
            Master.LogError("details.aspx.vb - Notes() - " & Master.error_string)
          End If
          Master.display_error()
        End If
        Dim width As String = "300"
        Dim css As String = "notes_list"
        Dim div As String = "notes_list_div"
        If cat_key = 0 Then
          width = "800"
          css = "notes_list_no_width"
          div = "notes_list_div_main"
        End If

        If Not IsNothing(Notes_Data) Then
          notes_string = ""
          If cat_key <> 0 Then
            If cat_key = trans_key And Master.TypeOfListing = 1 Then 'This has to be added. The transaction display for the company uses a datagrid. In order to get side by side - need this
            Else
              notes_string = notes_string & "<table width='100%' cellpadding='3' cellspacing='0' align='left'>"
              notes_string = notes_string & "<td align='left' valign='top' width='1%'>&nbsp;</td><td align='right' valign='top' class='border_left' width='300'>"
            End If
          End If

          If cat_key = trans_key And Master.TypeOfListing = "1" Then 'This has to be added. The transaction display for the company uses a datagrid. In order to get side by side - need this
          Else
            notes_string = notes_string & "<table width='100%' align='right' cellspacing='0' cellpadding='0'>"
            notes_string = notes_string & "<tr><td align='left' valign='top'>"
          End If

          If Master.TypeOfListing = 3 And cat_key > 0 Then 'If listing type is an aircraft
            If view = "action" Then 'If action items
              notes_string = notes_string & "<img src='images/action.jpg' alt='Action Items' class='float_right' /><br clear='all' />"
            ElseIf view = "prospect" Then
              notes_string = notes_string & "<img src='images/prospect.jpg' alt='Prospects' class='float_right' /><br clear='all' />"
            Else 'If notes
              Dim temporaryCategory As String = ""
              temporaryCategory = UCase(clsGeneral.clsGeneral.what_cat(cat_key, Nothing, Master))
              'We need to display a seperate image if it's interior or exterior notes.
              If temporaryCategory = "INTERIOR" Then
                notes_string = notes_string & "<img src='images/interior_notes.jpg' alt='Interior Notes' class='float_right' /><br clear='all' />"
              ElseIf temporaryCategory = "EXTERIOR" Then
                notes_string = notes_string & "<img src='images/exterior_notes.jpg' alt='Exterior Notes' class='float_right' /><br clear='all' />"
              Else
                ' notes_string = notes_string & "<img src='images/notes.jpg' alt='Notes' class='float_right' /><br clear='all' />"
              End If

            End If
          End If


          If view <> "email" Then 'And view <> "prospect" Then ' no add new button for email/prospect (for now)
            If Master.ListingSource = "CLIENT" Then 'If source is client, don't call javascript function asking for creation of company. 
              notes_string = notes_string & "<ul class='" & css & "' ><li><a href='#' onclick=""window.open('edit_note.aspx?action=new&amp;type=" & view & "&amp;cat_key=" & cat_key & "" & IIf(view = "note", IIf(startCount > 0, "&startCount=" & startCount, ""), "") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"
            Else
              'If Master.OtherID = 0 Then 'If there's no corresponding client record, force creation of client company if a company record.
              '  If Master.TypeOfListing = 1 Then
              '    notes_string = notes_string & "<ul class='" & css & "'><li><a href='#' onclick=""javascript:create_comp('edit_note.aspx?action=new&amp;type=" & view & "&amp;cat_key=" & cat_key & "','edit.aspx?comp_ID=" & Master.ListingID & "&source=" & Master.ListingSource & "&type=company&auto=true&note_type=" & view & "');"">"
              '  Else 'just suggest if jetnet record
              '    notes_string = notes_string & "<ul class='" & css & "'><li><a href='#' onclick=""javascript:test('edit_note.aspx?action=new&amp;type=" & view & "&amp;cat_key=" & cat_key & "','edit.aspx?action=edit&type=aircraft&run_auto=true');"">"
              '  End If
              'Else 'if corresponding record exists, keep going. 
              '  If Master.TypeOfListing = 1 Then
              '    notes_string = notes_string & "<ul class='" & css & "'><li><a href='#' onclick=""javascript:warning('edit_note.aspx?action=new&amp;type=" & view & "&amp;cat_key=" & cat_key & "');"">"
              '  Else
              notes_string = notes_string & "<ul class='" & css & "'><li><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=new&amp;type=" & view & "&amp;cat_key=" & cat_key & "" & IIf(view = "note", IIf(startCount > 0, "&startCount=" & startCount, ""), "") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"
              'End If

              'End If
            End If
          End If



          'Just figuring out the string for the add button. 
          If view = "action" Then
            notes_string = notes_string & "Add Action</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          ElseIf view = "documents" Then
            notes_string = notes_string & "Add Document</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          ElseIf view = "note" Then
            notes_string = notes_string & "Add Note</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          ElseIf view = "opportunity" Then
            notes_string = notes_string & "Add Opportunity</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          ElseIf view = "prospect" Then
            notes_string = notes_string & "Add Aircraft Prospect</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          End If


          If cat_key = 17 Then
            notes_string = notes_string & "<ul class='" & css & "'><li><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=new&amp;type=value_analysis&amp;cat_key=" & cat_key & "" & IIf(view = "note", IIf(startCount > 0, "&startCount=" & startCount, ""), "") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">"
            notes_string = notes_string & "Enter Aircraft Value Estimate</a>&nbsp;&nbsp;&nbsp;&nbsp;</li></ul>"
          End If


          If view = "note" And cat_key = 0 Then

            notes_string = notes_string & "<table width='50%' cellpadding='4' cellspacing='0' align='center'><tr>"

            If showPrevious = True Then
              notes_string = notes_string & "<td align='left' valign='top' width='50'><img src='images/spacer.gif' width='50' height='1' alt='' /><br /><a href='" & fullURL & "&startCount=" & startCount - 10 & "' class='bold_small'>Previous</a></td>"
            Else
              notes_string = notes_string & "<td align='left' valign='top' width='50'><img src='images/spacer.gif' width='50' height='1' alt='' /></td>"
            End If
            If showPrevious = True Or showNext = True Then
              notes_string = notes_string & "<td align='center' valign='top' width='180'><img src='images/spacer.gif' width='180' height='1' alt='' /><br /><i class='bold_small'>(Now Viewing Notes " & startCount + 1 & " to " & endCount & " of " & NotesRowCount.Rows.Count & ")</i></td>"
            End If

            NotesRowCount.Dispose()
            If showNext = True Then
              notes_string = notes_string & "<td align='right' valign='top' width='50'><img src='images/spacer.gif' width='50' height='1' alt='' /><br /><a href='" & fullURL & "&startCount=" & startCount + 10 & "' class='bold_small'>Next</a></td>"
            Else
              notes_string = notes_string & "<td align='left' valign='top' width='50'><img src='images/spacer.gif' width='50' height='1' alt='' /></td>"
            End If

            notes_string = notes_string & "</tr></table>"

          End If
          For Each q As DataRow In Notes_Data.Rows


            'Setting variables for notes display
            entry_date = IIf(Not IsDBNull(q("lnote_entry_date")), q("lnote_entry_date"), Now())
            lnote_document_name = IIf(Not IsDBNull(q("lnote_document_name")), q("lnote_document_name"), "")
            lnote_document_flag = IIf(Not IsDBNull(q("lnote_document_flag")), q("lnote_document_flag"), "")
            lnote_text = IIf(Not IsDBNull(q("lnote_note")), q("lnote_note"), "")

            lnote_opportunity_status = IIf(Not IsDBNull(q("lnote_opportunity_status")), q("lnote_opportunity_status"), "")

            If view = "documents" Or view = "opportunity" Then
              If (InStr(lnote_text, " ::: ") > 0) Then
                Dim text As Array = Split(lnote_text, " ::: ")
                lnote_text = text(1)
                lnote_title = text(0)
              End If
            End If
            schedule_start = IIf(Not IsDBNull(q("lnote_schedule_start_date")), q("lnote_schedule_start_date"), Now())
            status = IIf(Not IsDBNull(q("lnote_status")), q("lnote_status"), "P")

            If status = "E" Then
              typed = "E"
              view = "email"
              div = "email_list_div_main"
              size = "height=805,width=860"
            Else
              div = ""
              If y <> "DOCUMENTS" And y <> "ACTION" And y <> "OPPORTUNITIES" And y <> "PROSPECT" Then
                typed = "A"
                view = "note"
                If cat_key = 0 Then
                  NextNote = IIf(Not IsDBNull(q("lnote_next_id")), q("lnote_next_id"), "0")
                  PreviousNote = IIf(Not IsDBNull(q("lnote_previous_id")), q("lnote_previous_id"), "0")
                End If
              ElseIf y = "PROSPECT" Then
                typed = "B"
                view = "prospect"
                If lnote_opportunity_status = "I" Then
                  div = "display_disabled_prospect "
                End If
              ElseIf y = "ACTION" Then
                typed = "P"
                view = "action"
              ElseIf y = "OPPORTUNITIES" Then
                typed = "O"
                view = "opportunity"
              Else
                typed = "F"
                actioned = "&doc=true"
                view = "documents"
              End If
              If cat_key = 0 Then
                div += "notes_list_div_main"
              Else
                div += "notes_list_div"
              End If
              size = "height=435,width=860"
            End If
            lnote_category = IIf(Not IsDBNull(q("lnote_notecat_key")), q("lnote_notecat_key"), "0")
            lnote_jetnet_ac_id = IIf(Not IsDBNull(q("lnote_jetnet_ac_id")), q("lnote_jetnet_ac_id"), 0)
            lnote_client_ac_id = IIf(Not IsDBNull(q("lnote_client_ac_id")), q("lnote_client_ac_id"), 0)
            lnote_jetnet_comp_id = IIf(Not IsDBNull(q("lnote_jetnet_comp_id")), q("lnote_jetnet_comp_id"), 0)
            lnote_client_comp_id = IIf(Not IsDBNull(q("lnote_client_comp_id")), q("lnote_client_comp_id"), 0)
            lnote_jetnet_amod_id = IIf(Not IsDBNull(q("lnote_jetnet_amod_id")), q("lnote_jetnet_amod_id"), 0)
            lnote_client_amod_id = IIf(Not IsDBNull(q("lnote_client_amod_id")), q("lnote_client_amod_id"), 0)
            lnote_jetnet_contact_id = IIf(Not IsDBNull(q("lnote_jetnet_contact_id")), q("lnote_jetnet_contact_id"), 0)
            lnote_client_contact_id = IIf(Not IsDBNull(q("lnote_client_contact_id")), q("lnote_client_contact_id"), 0)
            lnote_id = IIf(Not IsDBNull(q("lnote_id")), q("lnote_id"), 0)
            lnote_user_id = IIf(Not IsDBNull(q("lnote_user_id")), q("lnote_user_id"), 0)
            lnote_user_name = IIf(Not IsDBNull(q("lnote_user_name")), q("lnote_user_name"), "")

            If view = "email" Then
              Dim info As Array = Split(HttpUtility.HtmlDecode(lnote_text), ":::")

              If Not IsNothing(info(0)) Then
                email_to = info(0)
              End If
              If Not IsNothing(info(1)) Then
                email_cc = info(1)
              End If
              If Not IsNothing(info(2)) Then
                email_subject = info(2)
                lnote_text = info(2)
              End If
              If Not IsNothing(info(3)) Then
                body = info(3)
              End If
              'If Not IsNothing(info(4)) Then
              '    lnote_text = info(4)
              'End If
            ElseIf view = "prospect" Then
              lnote_user_name = Master.what_user(lnote_user_id)
            End If

            If view = "email" Or view = "documents" Then
              document_display = clsGeneral.clsGeneral.DisplayDocuments(lnote_document_name, lnote_document_flag, False, lnote_id)
            End If
            If lnote_category = cat_key Or cat_key = 0 Then 'If the notes category is equal to the category we're looking at, show the note. 
              notes_string = notes_string & "<div class='" & div & "'>"

              If document_display <> "" Then
                If view <> "email" Then
                  notes_string = notes_string & "<table width='98%' cellpadding='0' cellspacing='0'><tr><td align='left' valign='top'>"
                End If
              End If
              notes_string = notes_string & "<b><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=" & view & "&amp;id=" & q("lnote_id") & "" & IIf(view = "note", IIf(NextNote > 0, "&nextNote=" & NextNote.ToString, "") & IIf(PreviousNote > 0, "&previousNote=" & PreviousNote.ToString, "") & IIf(startCount > 0, "&startCount=" & startCount, ""), "") & "','','scrollbars=yes,menubar=no," & size & ",resizable=yes,toolbar=no,location=no,status=no');"">"
              If IsDate(entry_date) And status <> "P" Then 'This means it's not an action.
                notes_string = notes_string & DateAdd("h", Session("timezone_offset"), entry_date)
                notes_string = notes_string & "</a> (<em>By: " & lnote_user_name & " </em> </b> "
                If status = "E" Then
                  notes_string = notes_string & " <b><em>For: " & email_to & "</em> </b>  "
                End If
                notes_string = notes_string & ") "

                'Adding the category if it's needed:
                If view = "note" And cat_key = 0 Then
                  Dim temporaryCategory As String = ""
                  temporaryCategory = UCase(clsGeneral.clsGeneral.what_cat(lnote_category, Nothing, Master))
                  'We need to show the category here:
                  If temporaryCategory <> "GENERAL" Then
                    notes_string += " [<em>" & temporaryCategory & "</em>] "
                  End If
                End If
              Else
                If status <> "P" And status <> "O" Then 'This means it's an action.
                  notes_string = notes_string & " " & DateAdd("h", Session("timezone_offset"), schedule_start) & "</a></b> - "
                Else
                  notes_string = notes_string & " " & DateAdd("h", Session("timezone_offset"), schedule_start) & "</a></b> - "
                End If
              End If

              If lnote_title <> "" Then
                notes_string = notes_string & "<b><a href='#' onclick=""javascript:window.open('edit_note.aspx?action=edit&type=" & view & "&id=" & q("lnote_id") & "" & IIf(view = "note", IIf(NextNote > 0, "&nextNote=" & NextNote.ToString, "") & IIf(PreviousNote > 0, "&previousNote=" & PreviousNote.ToString, "") & IIf(startCount > 0, "&startCount=" & startCount, ""), "") & "','','scrollbars=no,menubar=no," & size & ",resizable=yes,toolbar=no,location=no,status=no');"" style='text-decoration:none;'>" & lnote_title & "</a>"
                If lnote_text <> "" Then
                  notes_string = notes_string & ": "
                End If
                notes_string = notes_string & "</b>"
              End If
              'Just displaying the notes text field
              If Len(lnote_text) > 100 Then
                notes_string = notes_string & Server.HtmlDecode(Left(lnote_text, 100) & "...")
              Else
                notes_string = notes_string & Server.HtmlDecode(lnote_text)
              End If

              If Master.TypeOfListing <> 3 Then 'This means that this detailed listing which shows the aircraft information
                'on the note only shows when the listing type isn't an aircraft.
                used_id = IIf(lnote_jetnet_ac_id <> 0, lnote_jetnet_ac_id, lnote_client_ac_id)
                used_source = IIf(lnote_jetnet_ac_id <> 0, "JETNET", "CLIENT")
                If used_id <> 0 Then
                  notes_string = notes_string & "<b><a href='details.aspx?ac_ID=" & used_id & "&source=" & used_source & "&type=3'>" & Master.add_ac_name(used_id, 2, used_source) & "</a></b>"
                End If
              ElseIf status = "B" Then
                'Display the aircraft information only if the aircraft ID showing is different from what was selected.
                If (Master.ListingSource = "JETNET" And Master.ListingID <> lnote_jetnet_ac_id) Or (Master.ListingSource = "CLIENT" And Master.ListingID <> lnote_client_ac_id) Then
                  used_id = IIf(lnote_jetnet_ac_id <> 0, lnote_jetnet_ac_id, lnote_client_ac_id)
                  used_source = IIf(lnote_jetnet_ac_id <> 0, "JETNET", "CLIENT")
                  Dim DisplayTable As New DataTable
                  Dim SerNo As String = ""
                  Dim RegNo As String = ""
                  Dim ACYear As String = ""

                  If used_id <> 0 Then
                    If used_source = "JETNET" Then
                      DisplayTable = Master.aclsData_Temp.GetJETNET_AC_NAME(lnote_jetnet_ac_id, "")
                      If Not IsNothing(DisplayTable) Then
                        If DisplayTable.Rows.Count > 0 Then
                          If Not IsDBNull(DisplayTable.Rows(0).Item("ac_ser_nbr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ac_ser_nbr")) Then
                              SerNo = " S/N# " & DisplayTable.Rows(0).Item("ac_ser_nbr").ToString
                            End If
                          End If
                          If Not IsDBNull(DisplayTable.Rows(0).Item("ac_year_mfr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ac_year_mfr")) Then
                              ACYear = DisplayTable.Rows(0).Item("ac_year_mfr").ToString
                            End If
                          End If
                          If Not IsDBNull(DisplayTable.Rows(0).Item("ac_reg_nbr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("ac_reg_nbr")) Then
                              If SerNo <> "" Then
                                RegNo += ","
                              End If
                              RegNo = " Reg# " & DisplayTable.Rows(0).Item("ac_reg_nbr").ToString
                            End If
                          End If

                        End If 'Rows not zero
                      End If 'Display table not nothing
                    Else 'Not jetnet source
                      DisplayTable = Master.aclsData_Temp.Get_Clients_Aircraft_Ser_Model(lnote_client_ac_id)
                      If Not IsNothing(DisplayTable) Then
                        If DisplayTable.Rows.Count > 0 Then
                          If Not IsDBNull(DisplayTable.Rows(0).Item("cliaircraft_ser_nbr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("cliaircraft_ser_nbr")) Then
                              SerNo = " S/N# " & DisplayTable.Rows(0).Item("cliaircraft_ser_nbr").ToString
                            End If
                          End If
                          If Not IsDBNull(DisplayTable.Rows(0).Item("cliaircraft_year_mfr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("cliaircraft_year_mfr")) Then
                              ACYear = DisplayTable.Rows(0).Item("cliaircraft_year_mfr").ToString
                            End If
                          End If
                          If Not IsDBNull(DisplayTable.Rows(0).Item("cliaircraft_reg_nbr")) Then
                            If Not String.IsNullOrEmpty(DisplayTable.Rows(0).Item("cliaircraft_reg_nbr")) Then
                              If SerNo <> "" Then
                                RegNo += ","
                              End If
                              RegNo = " Reg# " & DisplayTable.Rows(0).Item("cliaircraft_reg_nbr").ToString
                            End If
                          End If
                        End If 'Rows not zero
                      End If 'Display table not nothing
                    End If 'if/then for source.
                    notes_string = notes_string & "<b> - <a href='details.aspx?ac_ID=" & used_id & "&source=" & used_source & "&type=3'>" & ACYear & SerNo & RegNo & "</a></b>"

                  End If 'ID doesn't equal zero

                End If 'Special check to display AC
              End If 'status = B

              'Adding the contact name
              If view = "note" Or view = "prospect" Then
                If lnote_jetnet_contact_id > 0 Or lnote_client_contact_id > 0 Then
                  If Master.Listing_ContactID <> IIf(lnote_jetnet_contact_id <> 0, lnote_jetnet_contact_id, lnote_client_contact_id) Then
                    notes_string += " [<em>" & DisplayContactNote(lnote_jetnet_contact_id, lnote_client_contact_id, IIf(lnote_jetnet_comp_id <> 0, lnote_jetnet_comp_id, lnote_client_comp_id)) & "</em>]"
                  End If
                End If
              End If


              If Master.TypeOfListing <> 1 Then
                used_id = IIf(lnote_jetnet_comp_id <> 0, lnote_jetnet_comp_id, lnote_client_comp_id)
                used_source = IIf(lnote_jetnet_comp_id <> 0, "JETNET", "CLIENT")

                'Get the jetnet company information. 
                If used_id <> 0 Then
                  notes_string = notes_string & "<b><a href='details.aspx?comp_ID=" & used_id & "&source=" & used_source & "&type=1'>" & Master.add_comp_name(used_id, 2, used_source) & "</a></b>"
                End If

              End If

              'Not showing the model if there's an aircraft ID, otherwise this information is already repeated.
              'We also do a check to make sure that if we show the model, we show it if the aircraft is different from what's being listed.
              If (status <> "B" And lnote_jetnet_ac_id = 0 And lnote_client_ac_id = 0) Or (status = "B" And (Master.ListingSource = "JETNET" And Master.ListingID <> lnote_jetnet_ac_id) Or (Master.ListingSource = "CLIENT" And Master.ListingID <> lnote_client_ac_id)) Then
                If lnote_jetnet_amod_id <> 0 Or lnote_client_amod_id <> 0 Then
                  notes_string = notes_string & " (<em>" & Master.what_model(lnote_jetnet_amod_id, lnote_client_amod_id) & "</em>)"
                End If
              End If

              If status = "B" Then
                If lnote_category > 0 Then
                  'This is a prospect, show the category:
                  Master.PerformDatabaseAction = True
                  Dim CategoryDisplay As String = Master.what_opportunity_cat(lnote_category, True)
                  If CategoryDisplay <> "" Then
                    notes_string += " <em>" & CategoryDisplay & "</em>"
                  End If
                End If
              End If

              If document_display <> "" Then
                If view <> "email" Then
                  notes_string = notes_string & "</td><td width='25' align='left' valign='top'>"
                  notes_string = notes_string & document_display & "</table>"
                Else
                  notes_string = notes_string & document_display
                End If
              End If

            End If



            notes_string = notes_string & "</div>"
          Next

          If cat_key = trans_key And Master.TypeOfListing = 1 Then
          Else 'This has to be added. The transaction display for the company uses a datagrid. In order to get side by side - need this
            notes_string = notes_string & "</td></tr>"
            notes_string = notes_string & "</table>"
          End If
          If cat_key <> 0 Then
            If cat_key = trans_key And Master.TypeOfListing = 1 Then
            Else
              notes_string = notes_string & "</td></tr></table>"
            End If
          End If

          ending = New Label
          ending.Text = notes_string
          pnl.Controls.Add(ending)
          ending.Dispose()
          c.Controls.Add(pnl)
          pnl.Dispose()

        Else
          If Master.aclsData_Temp.class_error <> "" Then
            Master.error_string = Master.aclsData_Temp.class_error
            Master.LogError("details.aspx.vb - Notes() - " & Master.error_string)
          End If
          Master.display_error()
        End If
      End If
      aTempTable = Nothing

    Catch ex As Exception
      Master.error_string = "details.aspx.vb - notes() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  Function DisplayContactNote(ByVal jetnet As Long, ByVal client As Long, ByVal companyID As Long) As String
    'This function takes the contact id/source and displays what contact the number is associated with.
    DisplayContactNote = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = Master.aclsData_Temp.GetContacts_Details(idnum, source)
      Dim comp_id As Integer = 0
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows

            DisplayContactNote = "<a href=""details.aspx?contact_ID=" & idnum & "&comp_ID=" & companyID & "&source=" & source & "&type=1"">"
            If Not IsDBNull(R("contact_first_name")) Then
              If Not String.IsNullOrEmpty(R("contact_first_name")) Then
                DisplayContactNote += R("contact_first_name") & " "
              End If
            End If
            If Not IsDBNull(R("contact_middle_initial")) Then
              If Not String.IsNullOrEmpty(R("contact_middle_initial")) Then
                DisplayContactNote += R("contact_middle_initial") & " "
              End If
            End If
            If Not IsDBNull(R("contact_last_name")) Then
              If Not String.IsNullOrEmpty(R("contact_last_name")) Then
                DisplayContactNote += R("contact_last_name")
              End If
            End If
            DisplayContactNote += "</a>"

            If Not IsDBNull(R("contact_title")) Then
              If Not String.IsNullOrEmpty(R("contact_title")) Then
                DisplayContactNote += ", " & R("contact_title").ToString
              End If
            End If
            If Not IsDBNull(R("contact_email_address")) Then
              If Not String.IsNullOrEmpty(R("contact_email_address")) Then
                DisplayContactNote += ", " & "<a href='mailto:" & R("contact_email_address") & "'>" & R("contact_email_address") & "</a>"
              End If
            End If
          Next
        End If
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = Master.aclsData_Temp.class_error
          Master.LogError("details.aspx.vb - what_contact() - " & Master.error_string)
        End If
        Master.display_error()
      End If
      Return DisplayContactNote
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - what_contact() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Function
#End Region
  '#Region "Fill Company/Contact/Job Tabs"
  '    'Sub dispDetails(ByVal s As LinkButton, ByVal e As EventArgs)
  '    '    Select Case (s.CommandName)
  '    '        Case "details"
  '    '            Dim split_me As Array = Split(s.CommandArgument, "|")
  '    '            Try
  '    '                Master.TypeOfListing = CInt(split_me(0))
  '    '                If CInt(split_me(0)) = 1 Then
  '    '                    Master.ListingID = CInt(split_me(1))
  '    '                    Try
  '    '                        Master.Listing_ContactID = CInt(split_me(2))
  '    '                    Catch
  '    '                        Master.Listing_ContactID = 0
  '    '                    End Try
  '    '                    Try
  '    '                        Master.ListingSource = split_me(3)
  '    '                    Catch ex As Exception
  '    '                        Master.ListingSource = "JETNET"
  '    '                    End Try
  '    '                Else
  '    '                    Master.ListingID = CInt(split_me(1))
  '    '                    Master.Listing_ContactID = 0
  '    '                    Master.ListingSource = split_me(2)
  '    '                End If


  '    '            Catch ex As Exception
  '    '                error_string = "details.aspx.vb - dispDetails() - " & ex.Message
  '    '                LogError(error_string)
  '    '            End Try
  '    '    End Select
  '    '    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location='details.aspx';", True)
  '    'End Sub
  '#End Region
#Region "Set Up Display Function/Fill Listing/View  Details"
  Private Sub set_up_display()
    Try
      Select Case Master.TypeOfListing
        Case 1
          companyCard.Visible = True
          contactCard.Visible = True
          aircraftCard.Visible = False
          Aircraft_Tabs1.Visible = False
          Company_Tabs1.Visible = True

        Case 3
          Aircraft_Tabs1.Visible = True
          Company_Tabs1.Visible = False
          companyCard.Visible = False
          contactCard.Visible = True
          aircraftCard.Visible = True

      End Select
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - set_up_display() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
  'Private Sub Fill_Listing()
  '    Try
  '        If back_button = True Then
  '            If Not IsNothing(Session("Results")) Then
  '                table = Session("Results")
  '                Master.Table_List = table
  '            Else
  '                table = Master.Table_List
  '            End If
  '        Else
  '            table = Master.Table_List
  '        End If
  '        If Not IsNothing(table) Then
  '            Session("FromDetails") = True
  '            Session("Results") = Master.Table_List
  '            Master.Redirect_Based_On_Type()
  '            HttpContext.Current.ApplicationInstance.CompleteRequest()
  '            Master.m_bIsTerminating = True
  '        End If
  '    Catch ex As Exception
  '        error_string = "details.aspx.vb - Fill_Listing() - " & ex.Message
  '        LogError(error_string)
  '    End Try
  'End Sub
  'Private Sub Fill_ListingFromBack()
  '    Try
  '        If Not IsNothing(Session("Results")) Then
  '            table = Session("Results")
  '            Master.Table_List = table
  '        Else
  '            table = Master.Table_List
  '        End If

  '        If Not IsNothing(table) Then
  '            Session("FromDetails") = True
  '            Session("Results") = Master.Table_List
  '            Master.TypeOfListing = Master.FromTypeOfListing
  '            Master.Redirect_Based_On_Type()
  '        End If
  '    Catch ex As Exception
  '        error_string = "details.aspx.vb - Fill_Listing() - " & ex.Message
  '        LogError(error_string)
  '    End Try
  'End Sub
  Sub details(ByVal sender As Object, ByVal e As System.EventArgs)
    Try
      Select Case sender.commandargument
        Case "ac"
          Dim arrayed As Array = Split(sender.commandname, "|")
          Master.TypeOfListing = 3
          Master.ListingID = arrayed(0)
          Master.ListingSource = arrayed(1)
        Case "comp"
          Dim arrayed As Array = Split(sender.commandname, "|")
          Master.TypeOfListing = 1
          Master.ListingID = arrayed(0)
          Master.ListingSource = arrayed(1)
      End Select
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location='details.aspx';", True)
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - details() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub
#End Region

#Region "Fill Page Listing from Back command"
  'Private Sub go_back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles go_back.Click
  '    Fill_ListingFromBack()
  'End Sub
#End Region

  ''' <summary>
  ''' Moved this to it's own function so that way I could put in in the page event loadComplete. This allows me to get the OtherID session variable that gets initialized in
  ''' The different controls.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub LogForEvo()
    Try
      Select Case Master.TypeOfListing
        Case 1

          'Changes 4/14:
          'anything that has a jetnet id - so i would guess if it is a client company with a jetnet id - then log with the jetnet id
          If Session.Item("isEVOLOGGING") = True Then
            If Master.ListingSource.ToUpper.Contains("JETNET") Then
              Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Company Details: Company_ID = " + Master.ListingID.ToString, Nothing, 0, 0, 0, Master.ListingID)
            ElseIf Master.ListingSource.ToUpper.Contains("CLIENT") Then
              If Master.OtherID > 0 Then
                Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Company Details: Company_ID = " + Master.OtherID.ToString, Nothing, 0, 0, 0, Master.OtherID)
              End If
            End If
          End If

        Case 3
          Dim ac_amod_id As String = ""

          If Master.ListingSource.ToUpper.Contains("JETNET") Then
            ac_amod_id = commonEvo.GetAircraftInfo(Master.ListingID, True, False)
          ElseIf Master.ListingSource.ToUpper.Contains("CLIENT") Then
            If Master.OtherID > 0 Then
              ac_amod_id = commonEvo.GetAircraftInfo(Master.OtherID, True, False)
            End If
          End If


          If Not String.IsNullOrEmpty(ac_amod_id.Trim) Then
            If Not IsNumeric(ac_amod_id) Then
              ac_amod_id = "0"
            End If
          End If
          If Session.Item("isEVOLOGGING") = True Then
            If Master.ListingSource.ToUpper.Contains("JETNET") Then
              Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Aircraft Details: AC_ID = " + Master.ListingID.ToString, Nothing, 0, 0, 0, 0, 0, Master.ListingID, CInt(ac_amod_id))
            ElseIf Master.ListingSource.ToUpper.Contains("CLIENT") Then
              Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Aircraft Details: AC_ID = " + Master.OtherID.ToString, Nothing, 0, 0, 0, 0, 0, Master.OtherID, CInt(ac_amod_id))
            End If
          End If
      End Select
    Catch ex As Exception
      Master.error_string = "details.aspx.vb - LogForEvo() - " & ex.Message
      Master.LogError(Master.error_string)
    End Try
  End Sub

  Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    LogForEvo()

    If Not IsNothing(Trim(Session.Item("startCount"))) Then 'We check existence of session item.
      If IsNumeric(Trim(Session.Item("startCount"))) Then 'Check for numeric
        If Session.Item("startCount") > 0 Then 'Then make sure it's greater than 0.
          Session.Item("startCount") = 0 'And then we clear it so that it doesn't cause the page to get "stuck".
        End If
      End If
    End If
  End Sub
End Class
