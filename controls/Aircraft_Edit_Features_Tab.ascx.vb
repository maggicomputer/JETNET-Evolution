Imports System.IO
Partial Public Class Aircraft_Edit_Features_Tab
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New Object 'Class Managers used
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Dim aircraftID As Long = 0
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try


        If Not IsNothing(Request.Item("ac_ID")) Then
          If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
            AircraftID = CLng(Request.Item("ac_ID").ToString.Trim)
          End If
        End If

        If AircraftID = 0 Then
          AircraftID = Session.Item("ListingID")
        End If



        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        'going away

        If Not Page.IsPostBack Then
          Dim aircraftTable As New DataTable
          aircraftTable = CommonAircraftFunctions.BuildReusableTable(aircraftID, 0, "CLIENT", "", aclsData_Temp, True, 0, "CLIENT")

          If Not IsNothing(aircraftTable) Then
            If aircraftTable.Rows.Count > 0 Then
              title_change.Text = CommonAircraftFunctions.CreateHeaderLine(aircraftTable.Rows(0).Item("amod_make_name"), aircraftTable.Rows(0).Item("amod_model_name"), aircraftTable.Rows(0).Item("ac_ser_nbr"), "")
            End If
          End If

          bind_data()
          aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_List()

          If Not IsNothing(aTempTable) Then
            For Each r As DataRow In aTempTable.Rows
              clikfeat_name.Items.Add(New ListItem(r("clikfeat_name"), r(1)))
            Next
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
          End If

          aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_Flag()
          If Not IsNothing(aTempTable) Then
            For Each r As DataRow In aTempTable.Rows
              status.Items.Add(New ListItem(r(1), r(0)))
            Next
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End If
          End If
        End If
      Catch ex As Exception
        error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - Page_Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
  Public Sub bind_data()
    Dim temp_seq_no As Integer = 0
    Dim FeaturesTable As New DataTable

    FeaturesTable = aclsData_Temp.Get_Client_Aircraft_Key_Features(aircraftID)
    If Not IsNothing(FeaturesTable) Then
      If FeaturesTable.Rows.Count > 0 Then
        datagrid_features.DataSource = FeaturesTable
        datagrid_features.DataBind()

        For Each r As DataRow In FeaturesTable.Rows
          temp_seq_no = r("cliafeat_seq_nbr")
        Next

        Me.seq_no.Text = (temp_seq_no + 1)
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End If
    End If
  End Sub
  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      datagrid_features.EditItemIndex = -1
      bind_data()

    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_features.EditItemIndex = CInt(E.Item.ItemIndex)
      bind_data()
      datagrid_features.DataBind()
    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    Try
      new_row.Visible = True
      add_new.Visible = False
    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - add_new_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id_delete")
      Dim type_hidden As TextBox = e.Item.FindControl("name_delete")
      If aclsData_Temp.Delete_Client_Aircraft_Key_Features(id.Text, type_hidden.Text) = 1 Then
        bind_data()
        message.Text = "<p align='center'>Your information has been removed.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - MyDataGrid_Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id_delete")
      Dim typed As TextBox = e.Item.FindControl("name_delete")
      Dim description_new As DropDownList = e.Item.FindControl("description")
      Dim type_new As DropDownList = e.Item.FindControl("name_type")
      Dim seq As TextBox = e.Item.FindControl("seq")

      Dim id_int As Integer = CInt(id.Text)
      Dim seq_int As Integer = CInt(seq.Text)
      If aclsData_Temp.Update_Client_Aircraft_Key_Features(id_int, type_new.SelectedValue, description_new.SelectedValue, seq_int, typed.Text, id_int) = 1 Then
        datagrid_features.EditItemIndex = -1
        bind_data()
        message.Text = "<p align='center'>Your information has been updated.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - MyDataGrid_Update() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Private Sub insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert.Click
    Try
      If aclsData_Temp.Insert_Client_Aircraft_Key_Features(aircraftID, clikfeat_name.SelectedValue, status.SelectedValue, seq_no.Text) = 1 Then
        datagrid_features.EditItemIndex = -1
        bind_data()
        message.Text = "<p align='center'>Your information has been added.</p>"
        new_row.Visible = False
        add_new.Visible = True
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - MyDataGrid_Update() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub datagrid_features_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles datagrid_features.ItemDataBound
    Try
      Dim sel As TextBox = e.Item.FindControl("name_delete")
      If Not IsNothing(e.Item.FindControl("name_type")) Then
        Dim ddl As DropDownList = e.Item.FindControl("name_type")
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_List


        For Each r As DataRow In aTempTable.Rows
          ddl.Items.Add(New ListItem(r("clikfeat_name"), r(1)))
        Next

        ddl.SelectedValue = sel.Text
      End If
      aTempTable.Dispose()

      If Not IsNothing(e.Item.FindControl("description")) Then
        Dim ddl2 As DropDownList = e.Item.FindControl("description")
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Key_Features_Flag()

        For Each r As DataRow In aTempTable.Rows
          ddl2.Items.Add(New ListItem(r(1), r(0)))
        Next
        Dim sel2 As TextBox = e.Item.FindControl("description_delete")

        ddl2.SelectedValue = sel2.Text
      End If


    Catch ex As Exception
      error_string = "Aircraft_Edit_Features_Tabs.ascx.vb - datagrid_features_ItemDataBound() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try

  End Sub

End Class