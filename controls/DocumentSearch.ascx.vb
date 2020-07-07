Public Partial Class DocumentSearch
    Inherits System.Web.UI.UserControl
    Dim error_string As String = ""
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
    Public Event check_changed(ByVal sender As Object)
    Public Event Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal display_cbo As String, ByVal orderby As String, ByVal category As Integer, ByVal start_date As String, ByVal end_date As String)
#Region "Custom Events"
    Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            'Dim filter_me As Boolean = False
            Dim notes_model As New ListBox

            If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                notes_model = model
            Else
                If model_cbo.Visible = True Then
                    notes_model = model_cbo
                Else
                    notes_model = model
                End If
            End If


            Dim models As String = ""
            For i = 0 To notes_model.Items.Count - 1
                If notes_model.Items(i).Selected Then
                    If notes_model.Items(i).Value <> "" Then
                        models = models & "'" & notes_model.Items(i).Value & "',"
                    End If
                End If
            Next

            If models <> "" Then
                models = UCase(models.TrimEnd(","))
            End If

            'Event that's handled on the Master Page.
            masterPage.PerformDatabaseAction = True
            RaiseEvent Searched_Me(e, Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, search_for_cbo.SelectedValue, models, display_cbo.SelectedValue, "", notes_cat.SelectedValue, ad_start_date.Text, ad_end_date.Text)
            masterPage.PerformDatabaseAction = False
        Catch ex As Exception
            error_string = "OpportunitiesSearch.ascx.vb - search_button_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            'Event that's handled on the Master Page.
            RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
        Catch ex As Exception
            error_string = "OpportunitiesSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            If Session.Item("crmUserLogon") = True Then
                search_for_txt.Focus()
                Dim TypeDataTable As New DataTable
                Dim TypeDataHold As New DataTable
                Dim temptable As New DataTable
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Try
                    'Querying the Database and keeping this information so we only have to do it once. 
                    If Not Page.IsPostBack Then
                        If Session.Item("localUser").crmEvo = True Then 'If an EVO user

                            clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                            ''''''
                        Else
                            Dim model As ListBox = model_cbo
                            Try
                                clsGeneral.clsGeneral.populate_models(model, True, Me, Nothing, masterPage, True)
                            Catch ex As Exception
                                error_string = "OpportunitiesSearch.ascx.vb - fill_CBO() Error in Aircraft Dropdown Filling - " & ex.Message
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
                            'Else
                            'clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                            'End If
                        End If
                    End If


                    '---------------------------------------------End Database Connection Stuff---------------------------------------------
                    If Not Page.IsPostBack Then

                        'If Session.Item("localUser").crmEvo = True Then 'If an EVO user
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
                        'End If


                        'Filling Note Category Up. 
                        aTempTable = masterPage.aclsData_Temp.Get_Client_Note_Document_Category("Y")
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                For Each z As DataRow In aTempTable.Rows
                                    notes_cat.Items.Add(New ListItem(z("notecat_name"), z("notecat_key")))
                                Next
                            End If
                        Else
                            If masterPage.aclsData_Temp.class_error <> "" Then
                                error_string = masterPage.aclsData_Temp.class_error
                                masterPage.LogError("opportunities page load - " & error_string)
                            End If
                            masterPage.display_error()
                        End If
                        notes_cat.Items.Add(New ListItem("All", 0))
                        notes_cat.SelectedValue = 0
                        'My Opportunities or all Opportunities
                        display_cbo.Items.Add(New ListItem("My Documents", Session.Item("localUser").crmLocalUserID))
                        display_cbo.Items.Add(New ListItem("All Documents", "0"))
                        search_where.Items.Clear()
                        search_for_cbo.Items.Clear()
                        search_for_cbo.Items.Add(New ListItem("COMPANY", "1"))
                        search_for_cbo.Items.Add(New ListItem("CONTACT", "2"))
                        search_for_cbo.Items.Add(New ListItem("AIRCRAFT", "3"))
                        search_for_cbo.Items.Add(New ListItem("ACTION ITEMS", "4"))
                        search_for_cbo.Items.Add(New ListItem("NOTES", "6"))
                        search_for_cbo.Items.Add(New ListItem("DOCUMENTS", "7"))
                        search_for_cbo.Items.Add(New ListItem("TRANSACTIONS", "8"))
                        search_for_cbo.Items.Add(New ListItem("MARKET", "10"))
                        search_where.Items.Add(New ListItem("Begins With", "2"))

                        search_where.Items.Add(New ListItem("Anywhere", "1"))
                        If Not Page.IsPostBack Then
                            Try
                                search_for_cbo.SelectedValue = 7
                            Catch
                            End Try
                        End If

                    End If
                Catch ex As Exception
                    error_string = "OpportunitiesSearch.ascx.vb - Page Init() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
    Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
        RaiseEvent check_changed(Me)
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