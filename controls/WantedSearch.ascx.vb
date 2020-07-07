Public Partial Class WantedSearch
    Inherits System.Web.UI.UserControl
    Dim error_string As String = ""
    Dim atemptable, temptable As New DataTable
    Public Event Wanted_Searched_me(ByVal sender As Object, ByVal model_cbo As ListBox, ByVal start_date As String, ByVal end_date As String, ByVal interested_party As String, ByVal subset As String)
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event check_changed(ByVal sender As Object)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            If Session.Item("crmUserLogon") = True Then
                interested_party.Focus()
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Dim TypeDataTable As New DataTable
                Dim TypeDataHold As New DataTable
                Dim default_vis As Boolean = True
                Dim research As Boolean = False

                Try

                    'Querying the Database and keeping this information so we only have to do it once. 
                    If Not Page.IsPostBack Then
                        If Trim(Request("redo_search")) = "true" Then
                            research = True
                        End If

                        If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                            datasubset_label.Text = ""
                            subset.Visible = False
                            clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                            ''''''
                        Else

                            Try
                                clsGeneral.clsGeneral.populate_models(model_cbo, IIf(research = True, False, True), Me, Nothing, masterPage, default_vis)
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
                            'clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
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



                        search_where.Items.Clear()
                        search_for_cbo.Items.Clear()
                        search_for_cbo.Items.Add(New ListItem("WANTEDS", "12"))

                        search_where.Items.Add(New ListItem("Begins With", "2"))

                        search_where.Items.Add(New ListItem("Anywhere", "1"))
                        If Not Page.IsPostBack Then
                            Try
                                search_for_cbo.SelectedValue = 12
                                If research = True Then
                                    RecallSessionForSearch()
                                End If
                            Catch
                            End Try
                        End If
                    End If

                Catch ex As Exception
                    error_string = "WantedSearch.ascx.vb - Page_Load() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
    Private Sub search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search.Click
        SearchClickFunction()
    End Sub
    Private Sub type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type.SelectedIndexChanged
        clsGeneral.clsGeneral.Type_Selected_Index_Changed(make, type, Page.IsPostBack)
    End Sub
    Private Sub SearchClickFunction()
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            wanted_search_attention.Text = ""


            Dim model_list As New ListBox

            If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                model_list = model
            Else
                If model_cbo.Visible = True Then
                    model_list = model_cbo
                Else
                    model_list = model
                End If
            End If

            If model.SelectedValue <> "" Or interested_party.Text <> "" Or model_cbo.SelectedValue <> "" Then
                SaveSessionForRecall()
                RaiseEvent Wanted_Searched_me(search, model_list, start_date.Text, end_date.Text, interested_party.Text, subset.SelectedValue)
            Else
                wanted_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
                wanted.Height = 250
            End If

        Catch ex As Exception
            error_string = "WantedSearch.ascx.vb - search_button_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub

    Private Sub make_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles make.SelectedIndexChanged
        clsGeneral.clsGeneral.Make_Selected_Index_Changed(model, make, type)
    End Sub

    Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
        RaiseEvent check_changed(Me)
    End Sub

    Private Sub model_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_type.SelectedIndexChanged
        clsGeneral.clsGeneral.Model_Type_Selected_Index_Changed(type, model_type)
    End Sub


    Private Sub SaveSessionForRecall()
        Dim ModelsForSave As String = ""

        For i = 0 To model_cbo.Items.Count - 1
            If model_cbo.Items(i).Selected Then
                If model_cbo.Items(i).Value <> "" Then
                    ModelsForSave += "'" & model_cbo.Items(i).Value & "',"
                End If
            End If
        Next

        'First we save the interested party
        Session("search_wanted") = Trim(clsGeneral.clsGeneral.StripChars(interested_party.Text, True)) & "@"
        'Then we save the type of search
        Session("search_wanted") += search_for_cbo.SelectedValue & "@"
        'Start date
        Session("search_wanted") += start_date.Text & "@"
        'End Date
        Session("search_wanted") += end_date.Text & "@"
        'Data Subset
        Session("search_wanted") += subset.SelectedValue & "@"
        'models
        Session("search_wanted") += ModelsForSave


    End Sub

    Private Sub RecallSessionForSearch()
        If Not IsNothing(Session("search_wanted")) Then
            If Not String.IsNullOrEmpty(Session("search_wanted")) Then
                Dim SearchText As Array = Split(Session("search_wanted"), "@")

                'The first variable that's been saved is the 
                'Interested party
                If UBound(SearchText) >= 0 Then
                    interested_party.Text = SearchText(0)
                End If

                'Type of 
                If UBound(SearchText) >= 1 Then
                    search_for_cbo.SelectedValue = SearchText(1)
                End If

                'Start date
                If UBound(SearchText) >= 2 Then
                    start_date.Text = SearchText(2)
                End If

                'End date
                If UBound(SearchText) >= 3 Then
                    end_date.Text = SearchText(3)
                End If

                'Subset
                If UBound(SearchText) >= 4 Then
                    subset.SelectedValue = SearchText(4)
                End If

                'models
                If UBound(SearchText) >= 5 Then
                    If Not String.IsNullOrEmpty(SearchText(5)) Then

                        'Replacing single quotes first
                        SearchText(5) = Replace(SearchText(5), "'", "")

                        Dim ListBoxToFill As New ListBox
                        Dim models As Array = Split(SearchText(5), ",")

                        'deselect all previous
                        model_cbo.SelectedValue = -1

                        For x = 0 To UBound(models)
                            For j As Integer = 0 To model_cbo.Items.Count() - 1
                                If UCase(model_cbo.Items(j).Value) = UCase(models(x)) Then
                                    model_cbo.Items(j).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If

                SearchClickFunction()
            End If
        End If
    End Sub
End Class