Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class ContactSearch
  Inherits System.Web.UI.UserControl
  Dim error_string As String = ""
  Dim aTempTable As New DataTable
  Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
  Public Event Searched_me(ByVal sender As Object, ByVal search_first As String, ByVal search_last As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal company_name As String, ByVal status_cbo As String, ByVal ordered_by As String, ByVal subset As String, ByVal email_address As String, ByVal phoneText As String)
#Region "Page Load"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      If Session.Item("crmUserLogon") = True Then
        first_name.Focus()

        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
          If Session.Item("localUser").crmEvo = True Then 'if person is EVO
            second_advanced.Visible = False
            search_pnl.Height = 67
            search_pnl_table.Height = 67
            status_cbo.Enabled = False
          Else
            search_pnl.Height = 130
            search_pnl_table.Height = 130
          End If
          If Not Page.IsPostBack Then
            ordered_by.Items.Add(New ListItem("First Name, Last Name", "1"))
            ordered_by.Items.Add(New ListItem("Company Name", "2"))
            ordered_by.Items.Add(New ListItem("Last Name, First Name", "3"))
            status_cbo.Items.Add(New ListItem("All", "B"))
            status_cbo.Items.Add(New ListItem("Active", "Y"))
            status_cbo.Items.Add(New ListItem("Inactive", "N"))
            status_cbo.SelectedValue = "Y"

            search_where.Items.Clear()
            search_for_cbo.Items.Clear()
            search_for_cbo.Items.Add(New ListItem("COMPANY", "1"))
            search_for_cbo.Items.Add(New ListItem("CONTACT", "2"))
            search_for_cbo.Items.Add(New ListItem("AIRCRAFT", "3"))
            search_for_cbo.Items.Add(New ListItem("ACTION ITEMS", "4"))
            search_for_cbo.Items.Add(New ListItem("NOTES", "6"))
            search_for_cbo.Items.Add(New ListItem("OPPORTUNITIES", "7"))
            search_for_cbo.Items.Add(New ListItem("TRANSACTIONS", "8"))
            search_for_cbo.Items.Add(New ListItem("MARKET", "10"))
            search_where.Items.Add(New ListItem("Begins With", "2"))

            search_where.Items.Add(New ListItem("Anywhere", "1"))
            If Not Page.IsPostBack Then
              Try
                search_for_cbo.SelectedValue = 2
              Catch
              End Try
            End If

            Dim research As Boolean = False
            If Not IsNothing(Trim(Request("redo_search"))) Then
              If Trim(Request("redo_search")) = "true" Then
                research = True
              End If
            End If

            'Let's try to refill up the Company folders.
            Dim cfolderData As String = ""
            Dim FolderTable As New DataTable
            If masterPage.IsSubNode = True Then
              cfolderData = clsGeneral.clsGeneral.ReturnCfolderData(masterPage, FolderTable)

              If cfolderData = "" Then
                masterPage.Fill_Contact(True, "", "", 2, "", "B", "Date Scheduled", "B", "", "")
              ElseIf cfolderData <> "" Then
                'Fills up the applicable folder Information pulled from the cfolder data field
                DisplayFunctions.FillUpFolderInformation(New Table, New Label, cfolderData, New Label, FolderTable, True, False, False, False, False, search_pnl, New BulletedList, Nothing, Nothing, Nothing)
                'Automatically running the search
                search_Click()
              End If
            End If

            If research = True Then
              If Not IsNothing(Session("search_contact")) Then
                If Not String.IsNullOrEmpty(Session("search_contact").ToString) Then
                  Last_Search() 'fill last search and perform
                End If
              End If
            End If
          End If
        Catch ex As Exception
          error_string = "ContactSearch.ascx.vb - Page_Load() - " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub
#End Region

#Region "Custom Events"
  Public Sub search_Click()


    Dim masterPage As main_site = DirectCast(Page.Master, main_site)

    'Event that's handled on the Master Page.
    'Clicking the button, so clear the subfolder.
    masterPage.NameOfSubnode = ""

    Try
      contact_search_attention.Text = ""
      search_pnl.Height = 130
      If first_name.Text <> "" Or last_name.Text <> "" Or comp_name_txt.Text <> "" Or comp_email_address.Text <> "" Or phone.Text <> "" Then

        masterPage.PerformDatabaseAction = True

        Session("search_contact") = Trim(clsGeneral.clsGeneral.StripChars(first_name.Text, True)) & "@" & clsGeneral.clsGeneral.StripChars(last_name.Text, True) & "@" & search_where.Text & "@" & search_for_cbo.SelectedValue & "@" & comp_name_txt.Text & "@" & status_cbo.SelectedValue & "@" & ordered_by.SelectedValue & "@" & subset.SelectedValue & "@" & Replace(comp_email_address.Text, "@", "***") & "@" & clsGeneral.clsGeneral.StripChars(phone.Text, True)

        RaiseEvent Searched_me(search_button, Trim(Replace(first_name.Text, "'", "''")), Trim(Replace(last_name.Text, "'", "''")), search_where.Text, search_for_cbo.SelectedValue, Replace(comp_name_txt.Text, "'", "''"), status_cbo.SelectedValue, ordered_by.SelectedValue, subset.SelectedValue, Replace(comp_email_address.Text, "'", "''"), phone.Text)
        masterPage.PerformDatabaseAction = False
      Else
        contact_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
        search_pnl.Height = 140
      End If

      masterPage.Write_Javascript_Out()
    Catch ex As Exception
      error_string = "ContactSearch.ascx.vb - search_button_Click() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      'Event that's handled on the Master Page.
      RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
    Catch ex As Exception
      error_string = "ContactSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
  Private Sub Last_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim contact_search As Array = Split(Session("search_contact"), "@")
      first_name.Text = contact_search(0)
      last_name.Text = contact_search(1)
      search_where.Text = contact_search(2)
      search_for_cbo.SelectedValue = contact_search(3)
      comp_name_txt.Text = contact_search(4)
      status_cbo.SelectedValue = contact_search(5)
      ordered_by.SelectedValue = contact_search(6)
      subset.SelectedValue = contact_search(7)
      comp_email_address.Text = Replace(contact_search(8), "***", "@")
      phone.Text = contact_search(9)
      RaiseEvent Searched_me(Me, Replace(first_name.Text, "'", "''"), Replace(last_name.Text, "'", "''"), search_where.Text, search_for_cbo.SelectedValue, Replace(comp_name_txt.Text, "'", "''"), status_cbo.SelectedValue, ordered_by.SelectedValue, subset.SelectedValue, Replace(comp_email_address.Text, "'", "''"), phone.Text)

    Catch ex As Exception
      error_string = "ContactSearch.ascx.vb - Last_Search() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub
End Class