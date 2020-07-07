Imports crmWebClient.clsGeneral

Partial Public Class ActionItemsSearch
    Inherits System.Web.UI.UserControl
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal document_status As String, ByVal type_notes As String, ByVal orderby As String, ByVal start_date As String, ByVal reg_start_date As String, ByVal reg_end_date As String)
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Dim error_string As String = ""
#Region "Custom Events"
    Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
        Search()
    End Sub
    Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            'Event that's handled on the Master Page.
            RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
        Catch ex As Exception
            error_string = "ActionItemsSearch.ascx.vb - search_for_cbo_SelectedIndexChanged()" & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Private Sub Search()
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            'Event that's handled on the Master Page.
            Session("DayPilotCalendar1_startDate") = ""
            masterPage.PerformDatabaseAction = True

            'Saving this search to session variable
            SaveSessionForRecall()

            RaiseEvent Searched_Me(search_button, Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, search_for_cbo.SelectedValue, view_cbo.SelectedValue, display_cbo.SelectedValue, order_bo.SelectedValue, start_date.Text, Trim(ad_start_date.Text), Trim(ad_end_date.Text))
            masterPage.PerformDatabaseAction = False
            If view_cbo.SelectedValue = "Day" Or view_cbo.SelectedValue = "Week" Or view_cbo.SelectedValue = "Month" Then
                start_date_lbl.Visible = True
                start_date.Visible = True
                cal_image2.Visible = True
                order_bo.Visible = False
                order_lbl.Visible = False
            Else
                start_date_lbl.Visible = False
                start_date.Visible = False
                cal_image2.Visible = False
                order_bo.Visible = True
                order_lbl.Visible = True
            End If
        Catch ex As Exception
            error_string = "ActionItemsSearch.ascx.vb - search_button_Click() " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
#End Region
    Private Sub display_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles display_cbo.SelectedIndexChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            If view_cbo.SelectedValue = "Day" Or view_cbo.SelectedValue = "Week" Then
                Session("DayPilotCalendar1_startDate") = Now()
            End If
        Catch ex As Exception
            error_string = "ActionItemsSearch.ascx.vb - display_cbo_SelectedIndexChanged()" & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            search_for_txt.Focus()
            Dim masterPage As main_site = DirectCast(Page.Master, main_site)
            Try
                If Not (String.IsNullOrEmpty(Session.Item("DaySelected"))) Then
                    view_cbo.SelectedValue = "Day"
                    Session.Item("DaySelected") = ""
                End If

                If Not Page.IsPostBack Then
                    Dim document_status As DropDownList = view_cbo
                    'View Type for Action Items
                    document_status.Items.Add(New ListItem("List", "List"))
                    document_status.Items.Add(New ListItem("Day", "Day"))
                    document_status.Items.Add(New ListItem("Week", "Week"))
                    document_status.Items.Add(New ListItem("Month", "Month"))
                    If Not (String.IsNullOrEmpty(Session.Item("DayPilotCalendar1_startDate"))) Then
                        document_status.SelectedValue = "Day"
                    End If
                    'My action items or all
                    clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, "Action Items", Nothing, masterPage)

                    'Order by
                    order_bo.Items.Add(New ListItem("Date Scheduled", "DATE"))
                    order_bo.Items.Add(New ListItem("Priority", "PRI_NAME"))


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
                            search_for_cbo.SelectedValue = 4
                            If Trim(Request("redo_search")) = "true" Then
                                RecallSessionForSearch()
                            End If
                        Catch
                        End Try
                    End If

                End If
            Catch ex As Exception
                error_string = "ActionItemsSearch.ascx.vb - Page_Load()" & ex.Message
                masterPage.LogError(error_string)
            End Try
        End If
    End Sub



    Private Sub SaveSessionForRecall()
        'First we save the search text
        Session("search_action") = Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)) & "@"

        'Then we save the search text operator (begins with, etc)
        Session("search_action") += search_where.SelectedValue & "@"

        'Then we save the type of search
        Session("search_action") += search_for_cbo.SelectedValue & "@"

        'Calendar type
        Session("search_action") += view_cbo.SelectedValue.ToString & "@"

        'Then we save the my notes/person's notes
        Session("search_action") += display_cbo.SelectedValue.ToString & "@"

        'Then we save the order by 
        Session("search_action") += order_bo.SelectedValue & "@"

        'Start date
        Session("search_action") += ad_start_date.Text & "@"

        'End Date
        Session("search_action") += ad_end_date.Text & "@"
    End Sub

    Private Sub RecallSessionForSearch()
        If Not IsNothing(Session("search_action")) Then
            If Not String.IsNullOrEmpty(Session("search_action")) Then
                Dim SearchText As Array = Split(Session("search_action"), "@")

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

                'active status
                If UBound(SearchText) >= 3 Then
                    view_cbo.SelectedValue = SearchText(3)
                End If

                'Person's notes/your notes
                If UBound(SearchText) >= 4 Then
                    display_cbo.SelectedValue = SearchText(4)
                End If

                'Order by
                If UBound(SearchText) >= 5 Then
                    order_bo.SelectedValue = SearchText(5)
                End If

                'Start date
                If UBound(SearchText) >= 6 Then
                    ad_start_date.Text = SearchText(6)
                End If

                'End date
                If UBound(SearchText) >= 7 Then
                    ad_end_date.Text = SearchText(7)
                End If


            End If
        End If
        Search()
    End Sub
End Class