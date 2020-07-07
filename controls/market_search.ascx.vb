Public Partial Class market_search
    Inherits System.Web.UI.UserControl
    Dim error_string As String = ""
    Dim atemptable, temptable As New DataTable
    Public Event Market_Searched_me(ByVal sender As Object, ByVal model_cbo As ListBox, ByVal start_date As Integer, ByVal cat As ListBox, ByVal market_type As ListBox, ByVal start_date As String, ByVal end_date As String)
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event check_changed(ByVal sender As Object)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            If Session.Item("crmUserLogon") = True Then
                market_time.Focus()
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Dim TypeDataTable As New DataTable
                Dim TypeDataHold As New DataTable
                Dim default_vis As Boolean = True
                Dim research As Boolean = False

                Try

                    'Querying the Database and keeping this information so we only have to do it once. 
                    If Not Page.IsPostBack Then
                        If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                            evo_swap.Visible = True
                            market_time.Visible = False
                            clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                            ''''''
                        Else
                            evo_swap.Visible = False
                            market_time.Visible = True
                            Try
                                clsGeneral.clsGeneral.populate_models(model_cbo, default_vis, Me, Nothing, masterPage, default_vis)
                            Catch ex As Exception
                                error_string = "wantedSearch - fill_CBO() Trans Model Dropdown Filling - " & ex.Message
                                masterPage.LogError(error_string)
                            End Try

                            If model_cbo.SelectedValue <> "" Then
                                default_models.Checked = True
                            Else
                                default_models.Checked = False
                            End If
                            model_cbo.Visible = True
                            model_evo_swap.Visible = False
                            model_type.Visible = False
                            default_models.Visible = True
                            ' Else
                            '    clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                            'End If
                        End If
                    End If

                    If Not Page.IsPostBack Then
                        ' If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                        'Just a simple check to get rid of all the checkboxes we can't have.
                        If Session.Item("localSubscription").crmHelicopter_Flag <> True Then
                            model_type.Items.Remove(model_type.Items.FindByValue("Helicopter"))
                        End If
                        If Session.Item("localSubscription").crmBusiness_Flag <> True Then
                            model_type.Items.Remove(model_type.Items.FindByValue("Business"))
                        End If
                        If Session.Item("localSubscription").crmCommercial_Flag <> True Then
                            model_type.Items.Remove(model_type.Items.FindByValue("Commercial"))
                        End If
                        '    Else
                        If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                            If Not IsNothing(Trim(Request("redo_search"))) Then
                                If Trim(Request("redo_search")) = "true" Then
                                    research = True
                                End If
                            End If
                        End If
                        ' End If

                        clsGeneral.clsGeneral.Market_Categories(categories, types, masterPage.aclsData_Temp, "")


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
                                search_for_cbo.SelectedValue = 10
                            Catch
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    error_string = "MarketSearch.ascx.vb - Page_Load() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
    Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        'Event that's handled on the Master Page.
        'Response.Write(search_for_cbo.SelectedItem.Value)
        Try
            RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
        Catch ex As Exception
            error_string = "MarketSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Private Sub search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search.Click
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            market_search_attention.Text = ""
            market.Height = 250
            'Dim masterPage As main_site = DirectCast(Page.Master, main_site)
            'Dim models As String = ""
            'For i = 0 To model_cbo.Items.Count - 1
            '    If model_cbo.Items(i).Selected Then
            '        models = models & "" & model_cbo.Items(i).Value & ","
            '    End If
            'Next

            'If models <> "" Then
            '    models = UCase(models.TrimEnd(","))
            'End If

            'Dim cat As String = ""
            'For i = 0 To categories.Items.Count - 1
            '    If categories.Items(i).Selected Then
            '        cat = cat & "'" & categories.Items(i).Value & "',"
            '    End If
            'Next

            'If cat <> "" Then
            '    cat = UCase(cat.TrimEnd(","))
            'End If

            'Dim market_type As String = ""
            'For i = 0 To types.Items.Count - 1
            '    If types.Items(i).Selected Then
            '        market_type = market_type & "'" & types.Items(i).Value & "',"
            '    End If
            'Next

            'If market_type <> "" Then
            '    market_type = UCase(market_type.TrimEnd(","))
            'End If


            


            'masterPage.PerformDatabaseAction = True
            If model.SelectedValue = "" And model_cbo.SelectedValue = "" And market_time.SelectedValue = "" And (ad_start_date.Text = "" And ad_end_date.Text = "") Then
                market_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
                market.Height = 250
            Else
                If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                    RaiseEvent Market_Searched_me(e, model, CInt(market_time.SelectedValue), categories, types, ad_start_date.Text, ad_end_date.Text)
                Else
                    If model_cbo.Visible = True Then
                        RaiseEvent Market_Searched_me(e, model_cbo, CInt(market_time.SelectedValue), categories, types, ad_start_date.Text, ad_end_date.Text)
                    Else
                        RaiseEvent Market_Searched_me(e, model, CInt(market_time.SelectedValue), categories, types, ad_start_date.Text, ad_end_date.Text)
                    End If
                End If
            End If

            'masterPage.PerformDatabaseAction = False
        Catch ex As Exception
            error_string = "MarketSearch.ascx.vb - search_button_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub

    Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
        RaiseEvent check_changed(Me)
    End Sub

    Private Sub categories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles categories.SelectedIndexChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        clsGeneral.clsGeneral.Market_Type(categories, types, masterPage.aclsData_Temp, "")
    End Sub
    Private Sub type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type.SelectedIndexChanged
        clsGeneral.clsGeneral.Type_Selected_Index_Changed(make, type, Page.IsPostBack)
    End Sub


    Private Sub make_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles make.SelectedIndexChanged
        clsGeneral.clsGeneral.Make_Selected_Index_Changed(model, make, type)
    End Sub



    Private Sub model_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_type.SelectedIndexChanged
        clsGeneral.clsGeneral.Model_Type_Selected_Index_Changed(type, model_type)
    End Sub
End Class