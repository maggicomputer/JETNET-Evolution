' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Mobile.Master.vb $
'$$Author: Mike $
'$$Date: 6/14/20 8:38a $
'$$Modtime: 6/14/20 8:12a $
'$$Revision: 2 $
'$$Workfile: Mobile.Master.vb $
'
' ********************************************************************************

Partial Public Class Mobile
  Inherits System.Web.UI.MasterPage
  Public aTempTable As New DataTable 'Data Tables used
  Public aclsData_Temp As New clsData_Manager_SQL 'Test DataObject!!
  Public aTempTable2 As New DataTable 'Data Tables used 
  Public error_string As String
  'Public Event Search_Click()
  Private intTypeOfListing As Integer
  Private strSource As String
  Private intID As Integer
  Private intOtherID As Integer
  Private intContactID As Integer
  Private intSubNodeOfListing As Integer
  Private boolEdit As Boolean
  Private bAircraftSort_Company As Boolean

  Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
    error_string = "mobile_master.aspx.vb - Page Load (Mobile Master Page) " & sender.ToString
    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    If Not Page.IsPostBack Then
      Session.Item("TypeOfListing") = 0
    End If

    Try

      'Right now.. if the EVO flag is set to true, I'm going to change the datalayer.
      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

      Session.Item("isMobile") = True
      aclsData_Temp.class_error = ""
      Sub_Menu_Visibility(False)
      'Let's set the welcome Message
      If Not Page.IsPostBack Then
        user_welcome_label.Text = clsGeneral.clsGeneral.SettingWelcomeMessage()
        'user_database_label.Text = clsGeneral.clsGeneral.SettingFrequency
      End If
      'What Type is this?
      What_Type_Is_This()



      If Session.Item("crmUserLogon") <> True Then
        'error_string = "crmMobile master page - " & Request.ServerVariables("SCRIPT_NAME").ToString() & " - Session Timeout"
        'clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        Response.Redirect("Default.aspx", False)
      End If


    Catch ex As Exception
      error_string = "listings.master.vb - Page Load() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub Sub_Menu_Visibility(ByVal visible As Boolean)
    home_link.Visible = visible
    companies_link.Visible = visible
    contacts_link.Visible = visible
    aircraft_link.Visible = visible
    ' notes_link.Visible = visible
    ' actions_link.Visible = visible
    ' docs_link.Visible = visible
    transaction_link.Visible = visible
    ' market_link.Visible = visible
  End Sub
  Public Sub What_Type_Is_This()
    If Not IsNothing(Request.Item("type")) Then
      If Not String.IsNullOrEmpty(Request.Item("type").ToString) Then
        If IsNumeric(Request.Item("type").Trim) Then
          TypeOfListing = Request.Item("type").Trim
        End If
      End If
    Else
      TypeOfListing = Session("TypeOfListing")
    End If

    folder_search.Text = "<a href='mobile_listing.aspx?type=" & TypeOfListing & "&show=folder'>Folder Search</a>"

    search_new.Text = "<a href='mobile_listing.aspx?type=" & TypeOfListing & "'>New Search</a>"
    If Not IsNothing(Request.Item("sub")) Then
      If Not String.IsNullOrEmpty(Request.Item("sub").ToString) Then
        If IsNumeric(Request.Item("sub").Trim) Then
          SubNodeOfListing = Request.Item("sub").Trim
        End If
      End If
    End If

    If Not IsNothing(Request.Item("comp_ID")) Then
      If Not String.IsNullOrEmpty(Request.Item("comp_ID").ToString) Then
        If IsNumeric(Request.Item("comp_ID").Trim) Then
          ListingID = Request.Item("comp_ID").Trim
        End If
      End If
    Else
      ListingID = Session("ListingID")
    End If

    If Not IsNothing(Request.Item("ac_ID")) Then
      If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
        If IsNumeric(Request.Item("ac_ID").Trim) Then
          ListingID = Request.Item("ac_ID").Trim
        End If
      End If
    Else
      ListingID = Session("ListingID")
    End If

    If Not IsNothing(Request.Item("contact_ID")) Then
      If Not String.IsNullOrEmpty(Request.Item("contact_ID").ToString) Then
        If IsNumeric(Request.Item("contact_ID").Trim) Then
          Listing_ContactID = Request.Item("contact_ID").Trim
        End If
      End If
    Else
      Listing_ContactID = 0
    End If


    If Not IsNothing(Request.Item("source")) Then
      If Not String.IsNullOrEmpty(Request.Item("source").ToString) Then
        ListingSource = Request.Item("source").Trim
      End If
    Else
      ListingSource = Session("ListingSource")
    End If


    If TypeOfListing <> 0 Then
      Sub_Menu_Visibility(True)
      If TypeOfListing = 1 Then
        companies_link.BackColor = Drawing.Color.FromName("#8cc7dd")
        folder_search.Visible = True
      ElseIf TypeOfListing = 2 Then
        contacts_link.BackColor = Drawing.Color.FromName("#8cc7dd")
        folder_search.Visible = True
      ElseIf TypeOfListing = 3 Then
        aircraft_link.BackColor = Drawing.Color.FromName("#8cc7dd")
        folder_search.Visible = True
      ElseIf TypeOfListing = 4 Then
        new_action.Visible = True
        actions_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      ElseIf TypeOfListing = 6 Then
        new_note.Visible = True
        notes_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      ElseIf TypeOfListing = 7 Then
        'new_document.Visible = True
        docs_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      ElseIf TypeOfListing = 8 Then
        transaction_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      ElseIf TypeOfListing = 10 Then
        market_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      ElseIf TypeOfListing = 11 Then
        new_opportunity.Visible = True
      End If
    End If


  End Sub
  Public Property AircraftSort_Company() As Boolean
    Get
      Return bAircraftSort_Company
    End Get
    Set(ByVal value As Boolean)
      bAircraftSort_Company = value
      Session.Item("AircraftSort_Company") = value
    End Set
  End Property
  Public Property TypeOfListing() As Integer
    Get
      Return intTypeOfListing
    End Get
    Set(ByVal value As Integer)
      intTypeOfListing = value
      Session.Item("Listing") = value
    End Set
  End Property
  Public Property SubNodeOfListing() As Integer
    Get
      Return intSubNodeOfListing
    End Get
    Set(ByVal value As Integer)
      intSubNodeOfListing = value
    End Set
  End Property
  Public Property ListingID() As Integer 'ID of listing
    Get
      Return intID
    End Get
    Set(ByVal value As Integer)
      intID = value
      Session.Item("ListingID") = value
    End Set
  End Property
  Public Property OtherID() As Integer 'Other ID of listing
    Get
      Return intOtherID
    End Get
    Set(ByVal value As Integer)
      intOtherID = value
      Session.Item("OtherID") = value
    End Set
  End Property
  Public Property ListingSource() As String 'Jetnet/Client source
    Get
      Return strSource
    End Get
    Set(ByVal value As String)
      strSource = UCase(value)
      Session.Item("ListingSource") = UCase(value)
    End Set
  End Property
  Public Property Listing_ContactID() As Integer
    Get
      Return intContactID
    End Get
    Set(ByVal value As Integer)
      intContactID = value
      Session.Item("ContactID") = value
    End Set
  End Property
#Region "Error Handling for datamanager"

#End Region
  Public Sub Set_Edit_Button(ByVal text As String)
    edit_link.Text = text
    edit_link.Visible = True
  End Sub
  Public Function display_error()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function

  Private Sub user_logout_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles user_logout_button.Click
    Dim masterPage As Mobile = DirectCast(Page.Master, Mobile)
    If Session.Item("localUser").crmLocalUserID <> 0 Then
      clsGeneral.clsGeneral.LogUser(masterPage, "N")
    End If

    Session.Contents.Clear()
    Session.Abandon()
    Session.Item("Listing") = ""
    Session.Item("Subnode") = ""
    Session.Item("ID") = ""

    Response.Redirect("default.aspx?mobile=1", False)
  End Sub

  'Private Sub folder_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles folder_search.Click
  '    RaiseEvent Folder_Click()
  'End Sub

  'Private Sub new_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_new.Click
  '    RaiseEvent Search_Click()
  'End Sub
End Class