Imports System.IO
Partial Public Class JobsSearch
    Inherits System.Web.UI.UserControl
    Public Event Searched_Me(ByVal sender As Object, ByVal subnode As String, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String)
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event Redirect(ByVal sender As Object)
    Dim error_string As String = ""

#Region "Events"
    Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
        'Event that's handled on the Master Page.
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
        Catch ex As Exception
            error_string = "JobsSearch.ascx.vb - search_button_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
        'Event that's handled on the Master Page.
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
        Catch ex As Exception
            error_string = "JobsSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() - " & ex.Message
            MasterPage.LogError(error_string)
        End Try
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            If Session.Item("crmUserLogon") = True Then
                search_for_txt.Focus()
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Try
                    If Not Page.IsPostBack Then
                        search_where.Items.Clear()
                        search_for_cbo.Items.Clear()
                        search_for_cbo.Items.Add(New ListItem("COMPANY", "1"))
                        search_for_cbo.Items.Add(New ListItem("CONTACT", "2"))
                        search_for_cbo.Items.Add(New ListItem("AIRCRAFT", "3"))
                        search_for_cbo.Items.Add(New ListItem("ACTION ITEMS", "4"))

                        search_for_cbo.Items.Add(New ListItem("JOBS", "5"))
                        search_for_cbo.Items.Add(New ListItem("NOTES", "6"))
                        search_for_cbo.Items.Add(New ListItem("OPPORTUNITIES", "7"))
                        search_for_cbo.Items.Add(New ListItem("TRANSACTIONS", "8"))
                        search_for_cbo.Items.Add(New ListItem("MARKET", "10"))
                        search_for_cbo.Items.Add(New ListItem("Begins With", "2"))

                        search_where.Items.Add(New ListItem("Anywhere", "1"))
                        If Not Page.IsPostBack Then
                            Try
                                search_for_cbo.SelectedValue = 5
                            Catch
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    error_string = "JobSearch.ascx.vb - Page_Load() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
End Class