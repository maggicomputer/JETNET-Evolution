Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class OpportunitesSearch
    Inherits System.Web.UI.UserControl
    Dim error_string As String = ""
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event check_changed(ByVal sender As Object)
    Public Event Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal display_cbo As String, ByVal start_date As String, ByVal notecat As Integer, ByVal end_date As String, ByVal status As String)
#Region "Custom Events"
    Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
        Search()
    End Sub
#End Region
    Private Sub Search()
        'Event that's handled on the Master Page.
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            masterPage.PerformDatabaseAction = True
            RaiseEvent Searched_Me(search_button, Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, search_for_cbo.SelectedValue, display_cbo.SelectedValue, ad_start_date.Text, notes_cat.SelectedValue, ad_end_date.Text, opportunity_status.SelectedValue)
            SaveSessionForRecall()
            masterPage.PerformDatabaseAction = False
        Catch ex As Exception
            error_string = "OpportunitiesSearch.ascx.vb - Search_Button_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            If Session.Item("crmUserLogon") = True Then
                search_for_txt.Focus()
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Try

                    '---------------------------------------------End Database Connection Stuff---------------------------------------------
                    If Not Page.IsPostBack Then

                        clsGeneral.clsGeneral.Fill_Opportunity_Category(notes_cat, aTempTable, masterPage.aclsData_Temp)
                        'My notes or all notes 
                        clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, "Opportunities", Nothing, masterPage)
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
                        search_for_cbo.Items.Add(New ListItem("OPPORTUNITIES", "11"))
                        search_where.Items.Add(New ListItem("Begins With", "2"))

                        search_where.Items.Add(New ListItem("Anywhere", "1"))
                        If Not Page.IsPostBack Then
                            Try
                                search_for_cbo.SelectedValue = 11
                                If Trim(Request("redo_search")) = "true" Then
                                    RecallSessionForSearch()
                                End If
                            Catch
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    error_string = "notesSearch.ascx.vb - Page Init() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub


    Private Sub SaveSessionForRecall()
    'First we save the search text

        Session("search_opportunity") = Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)) & "@"

        'Then we save the search text operator (begins with, etc)
        Session("search_opportunity") += search_where.SelectedValue & "@"

        'Then we save the type of search
        Session("search_opportunity") += search_for_cbo.SelectedValue & "@"

        'Category
        Session("search_opportunity") += notes_cat.SelectedValue.ToString & "@"

        'Then we save the my notes/person's notes
        Session("search_opportunity") += display_cbo.SelectedValue.ToString & "@"

        'Then we save the status
        Session("search_opportunity") += opportunity_status.SelectedValue & "@"

        'Start date
        Session("search_opportunity") += ad_start_date.Text & "@"

        'End Date
        Session("search_opportunity") += ad_end_date.Text & "@"

    End Sub


    Private Sub RecallSessionForSearch()
        If Not IsNothing(Session("search_opportunity")) Then
            If Not String.IsNullOrEmpty(Session("search_opportunity")) Then
                Dim SearchText As Array = Split(Session("search_opportunity"), "@")

                'The first variable that's been saved is the 
                'Search for txt.
                If UBound(SearchText) >= 0 Then
                    search_for_txt.Text = SearchText(0)
                End If

                'Then we fill in the search where.
                If UBound(SearchText) >= 1 Then
                    search_where.SelectedValue = SearchText(1)
                End If

                'Type of 
                If UBound(SearchText) >= 2 Then
                    search_for_cbo.SelectedValue = SearchText(2)
                End If

                'category
                If UBound(SearchText) >= 3 Then
                    notes_cat.SelectedValue = SearchText(3)
                End If

                'Person's notes/your notes
                If UBound(SearchText) >= 4 Then
                    display_cbo.SelectedValue = SearchText(4)
                End If

                'Status
                If UBound(SearchText) >= 5 Then
                    opportunity_status.SelectedValue = SearchText(5)
                End If

                'Start date
                If UBound(SearchText) >= 6 Then
                    ad_start_date.Text = SearchText(6)
                End If

                'End date
                If UBound(SearchText) >= 7 Then
                    ad_end_date.Text = SearchText(7)
                End If

                Search()
            End If
        End If

    End Sub
End Class