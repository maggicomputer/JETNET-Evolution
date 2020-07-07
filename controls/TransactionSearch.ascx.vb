Imports System.IO
Imports crmWebClient.clsGeneral

Partial Public Class TransactionSearch
  Inherits System.Web.UI.UserControl
  Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
  Public Event Market_Searched_me(ByVal sender As Object, ByVal model_cbo As String, ByVal start_date As String)
  Public Event Searched_me(ByVal sender As Object, ByVal search As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal subset As String, ByVal trans_type As String, ByVal start_date As String, ByVal end_date As String, ByVal relationships As String, ByVal year_start As String, ByVal year_end As String, ByVal internal As String, ByVal awaiting As Boolean)
  Public Event check_changed(ByVal sender As Object)
  Dim aTempTable, aTempTable2, temptable As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      If Session.Item("crmUserLogon") = True Then

        Dim _afttCookies As HttpCookie = Request.Cookies("aftt")
        If Not IsNothing(_afttCookies) Then
          If _afttCookies.Value = True Then
            aftt.Checked = True
          Else
            aftt.Checked = False
          End If
        Else
          aftt.Checked = True
        End If

        search_for_txt.Focus()
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Dim TypeDataTable As New DataTable
        Dim TypeDataHold As New DataTable
        Dim default_vis As Boolean = True
        Dim research As Boolean = False
        Dim helicopter As Boolean = False
        Dim business As Boolean = False
        Dim commercial As Boolean = False
        Dim select_string As String = ""

        Try
          If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
            If Not IsNothing(Trim(Request("redo_search"))) Then
              If Trim(Request("redo_search")) = "true" Then
                research = True
              End If
            End If
          End If

          'Querying the Database and keeping this information so we only have to do it once. 
          If Not Page.IsPostBack Then
            If Session.Item("localUser").crmEvo = True Then 'If an EVO user

              clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
              ''''''
            Else
              If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                Try
                  clsGeneral.clsGeneral.populate_models(model_cbo, IIf(research = True, False, True), Me, Nothing, masterPage, IIf(research = True, False, True))
                Catch ex As Exception
                  error_string = "aircraft - fill_CBO() Model Dropdown Filling - " & ex.Message
                  masterPage.LogError(error_string)
                End Try
              End If


              If model_cbo.SelectedValue <> "" Then
                default_models.Checked = True
              Else
                default_models.Checked = False
              End If
              model_cbo.Visible = True
              model_evo_swap.Visible = False
              model_type.Visible = False
              default_models.Visible = True
              '    Else

              '    clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)

              'End If

            End If
          End If
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

            'clsGeneral.clsGeneral.Transaction_Contact_Type(relationships, Nothing, masterPage)

            Try
              clsGeneral.clsGeneral.Transaction_Contact_Type(relationships, Nothing, masterPage)
              clsGeneral.clsGeneral.Transaction_Category(trans_type_cbo, Nothing, masterPage)

              If Not Page.IsPostBack Then
                trans_type_cbo.SelectedValue = "Full Sale"
              End If

              year_start.Items.Add(New ListItem("All", ""))
              year_end.Items.Add(New ListItem("All", ""))
              For i As Integer = 2015 To 1957 Step -1
                year_start.Items.Add(New ListItem(i, i))
                year_end.Items.Add(New ListItem(i, i))
              Next

              year_start.SelectedValue = ""
              year_end.SelectedValue = ""

              Dim search_Where As DropDownList = Me.FindControl("search_where")
              Dim search_in As DropDownList = Me.FindControl("search_for_cbo")
              search_Where.Items.Clear()
              search_in.Items.Clear()
              search_in.Items.Add(New ListItem("COMPANY", "1"))
              search_in.Items.Add(New ListItem("CONTACT", "2"))
              search_in.Items.Add(New ListItem("AIRCRAFT", "3"))
              search_in.Items.Add(New ListItem("ACTION ITEMS", "4"))
              search_in.Items.Add(New ListItem("NOTES", "6"))
              search_in.Items.Add(New ListItem("OPPORTUNITIES", "7"))
              search_in.Items.Add(New ListItem("TRANSACTIONS", "8"))
              search_in.Items.Add(New ListItem("MARKET", "10"))
              search_Where.Items.Add(New ListItem("Begins With", "2"))
              search_Where.Items.Add(New ListItem("Anywhere", "1"))
              search_in.SelectedValue = 8



              If Not IsNothing(Session("search_transaction")) Then
                If Not String.IsNullOrEmpty(Session("search_transaction").ToString) And research = True Then
                  default_vis = False
                End If
              End If


              Dim FolderTable As New DataTable
              Dim cfolderData As String = ""
              Dim AlreadyRanSearch As Boolean = False
              If masterPage.IsSubNode = True Then
                default_models.Checked = False
                cfolderData = clsGeneral.clsGeneral.ReturnCfolderData(masterPage, FolderTable)
                'This is going to populate the model box for those who are viewing a folder.
                clsGeneral.clsGeneral.populate_models(model_cbo, False, Me, Nothing, masterPage, False)

                If cfolderData <> "" Then

                  'Fills up the applicable folder Information pulled from the cfolder data field
                  DisplayFunctions.FillUpFolderInformation(New Table, New Label, cfolderData, New Label, FolderTable, True, False, False, False, False, search_pnl, New BulletedList, Nothing, Nothing, Nothing)


                  'Automatically running the search
                  Click_Search()
                  AlreadyRanSearch = True
                  masterPage.IsSubNode = False
                  masterPage.SubNodeOfListing = 0
                  masterPage.NameOfSubnode = ""
                End If
              End If


              If Session.Item("localUser").crmEvo <> True Then 'If an EVO user


                If research = True Then
                  If Not IsNothing(Session("search_transaction")) Then
                    If Not String.IsNullOrEmpty(Session("search_transaction").ToString) Then
                      Last_Search() 'fill last search and perform
                      default_vis = False
                      default_models.Checked = False
                    End If
                  End If
                End If
              End If
            Catch ex As Exception
              error_string = "transactionSearch - fill_CBO() Trans Type Dropdown Filling - " & ex.Message
              masterPage.LogError(error_string)
            End Try

          End If

        Catch ex As Exception
          error_string = "TransactionSearch.ascx.vb - Page_Load() - " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub
#Region "Custom Events"
  Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      'Event that's handled on the Master Page.
      RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
    Catch ex As Exception
      error_string = "TransactionSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
    Click_Search()
  End Sub

  Private Sub Click_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim model_list As New ListBox
    Try
      trans_search_attention.Text = ""
      search_pnl.Height = 270

      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
        model_list = model
      Else
        If model_cbo.Visible = True Then
          model_list = model_cbo
        Else
          model_list = model
        End If
      End If

      Dim models As String = ""
      For i = 0 To model_list.Items.Count - 1
        If model_list.Items(i).Selected Then
          If model_list.Items(i).Value <> "" Then
            models = models & "'" & model_list.Items(i).Value & "',"
          End If
        End If
      Next

      If models <> "" Then
        models = UCase(models.TrimEnd(","))
      End If
      Session.Item("models_export") = models


      Dim rel As String = ""
      For i = 0 To relationships.Items.Count - 1
        If relationships.Items(i).Selected Then
          rel = rel & "" & relationships.Items(i).Value & ","
        End If
      Next

      If rel <> "" Then
        rel = UCase(rel.TrimEnd(","))
      End If

      If start_date_txt.Text <> "" Or end_date_txt.Text <> "" Or model.SelectedValue <> "" Or model_cbo.SelectedValue <> "" Or search_for_txt.Text <> "" Then
        Session.Item("transaction_owners") = rel
        masterPage.PerformDatabaseAction = True
        Session("search_transaction") = clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True) & "@" & search_where.SelectedValue & "@" & search_for_cbo.SelectedValue & "@" & models & "@" & subset.SelectedValue & "@" & trans_type_cbo.SelectedValue & "@" & start_date_txt.Text & "@" & end_date_txt.Text & "@" & rel & "@" & year_start.SelectedValue & "@" & year_end.SelectedValue & "@" & internal_trans.SelectedValue & "@" & awaiting.Checked & "@" & aftt.Checked
        RaiseEvent Searched_me(Me, Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, search_for_cbo.SelectedValue, models, subset.SelectedValue, trans_type_cbo.SelectedValue, start_date_txt.Text, end_date_txt.Text, rel, year_start.SelectedValue, year_end.SelectedValue, internal_trans.SelectedValue, awaiting.Checked)
        masterPage.PerformDatabaseAction = False
      Else
        trans_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
        search_pnl.Height = 240
      End If


    Catch ex As Exception
      error_string = "TransactionSearch.ascx.vb - search_button_Click() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
    RaiseEvent check_changed(Me)
  End Sub
  Private Sub Last_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      '0 clsGeneral.clsGeneral.StripChars(search_for_txt.Text) 
      '1 search_where.SelectedValue
      '2 search_for_cbo.SelectedValue 
      '3 models 
      '4 subset.SelectedValue 
      '5 trans_type_cbo.SelectedValue 
      '6 start_date_txt.Text 
      '7 end_date_txt.Text 
      '8 rel 
      '9 year_start.SelectedValue 
      '10 year_end.SelectedValue

      Dim transaction_search As Array = Split(Session("search_transaction"), "@")
      search_for_txt.Text = transaction_search(0)
      search_where.SelectedValue = transaction_search(1)
      search_for_cbo.SelectedValue = transaction_search(2)
      Dim test As String = transaction_search(3)
      Dim models As Array = Split(Replace(transaction_search(3), "'", ""), ",")
      subset.SelectedValue = transaction_search(4)
      trans_type_cbo.SelectedValue = transaction_search(5)
      start_date_txt.Text = transaction_search(6)
      end_date_txt.Text = transaction_search(7)
      Dim rel As Array = Split(transaction_search(8), ",")
      year_start.SelectedValue = transaction_search(9)
      year_end.SelectedValue = transaction_search(10)
      Try
        internal_trans.SelectedValue = transaction_search(11)
      Catch
        internal_trans.SelectedValue = ""
      End Try

      'Awaiting on/off
      Try
        awaiting.Checked = transaction_search(12)
      Catch
        awaiting.Checked = False
      End Try


      'AFTT on/off
      Try
        aftt.Checked = transaction_search(13)
      Catch
        aftt.Checked = False
      End Try

      'Try
      '    awaiting.Checked = transaction_search(12)
      'Catch
      '    awaiting.Checked = False
      'End Try

      'refil the models 
      For x = 0 To UBound(models)
        For j As Integer = 0 To model_cbo.Items.Count() - 1
          If model_cbo.Items(0).Selected = True Then
            model_cbo.Items(0).Selected = False
          End If
          Dim ModelsSplit As String() = Split(UCase(model_cbo.Items(j).Value), "|")
          Dim ModelsSelection As String() = Split(UCase(models(x)), "|")
          'Comparing only the jetnet ID to the selection of the jetnet ID.
          'This is because on the transaction page you can edit transactions which in turn create models
          'And then refresh the page.
          If (UBound(ModelsSelection) >= 0 And UBound(ModelsSplit) >= 0) Then
            If ModelsSplit(0) = ModelsSelection(0) Then
              model_cbo.Items(j).Selected = True
            ElseIf UCase(model_cbo.Items(j).Value) = UCase(models(x)) Then
              model_cbo.Items(j).Selected = True
            End If
          End If
        Next
      Next


      'refil the relationships
      For x = 0 To UBound(rel)
        '  Response.Write(models(x) & "<br />")
        For j As Integer = 0 To relationships.Items.Count() - 1
          If relationships.Items(0).Selected = True Then
            relationships.Items(0).Selected = False
          End If
          Dim mode As String = UCase(relationships.Items(j).Value)
          Dim et As String = UCase(rel(x))
          If UCase(relationships.Items(j).Value) = UCase(rel(x)) Then
            relationships.Items(j).Selected = True
          Else
          End If
        Next
      Next
      Click_Search()
    Catch ex As Exception
      error_string = "transactionSearch.ascx.vb - click search() " & ex.Message
      masterPage.LogError(error_string)
    End Try
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
#End Region

End Class