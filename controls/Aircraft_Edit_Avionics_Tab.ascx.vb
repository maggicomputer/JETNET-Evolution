Imports System.IO
Partial Public Class Aircraft_Edit_Avionics_Tab
  Inherits System.Web.UI.UserControl
  'going away
  Dim aclsData_Temp As New Object 'Class Managers used
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Dim AircraftID As Long = 0
#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


      If Not IsNothing(Request.Item("ac_ID")) Then
        If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
          AircraftID = CLng(Request.Item("ac_ID").ToString.Trim)
        End If
      End If

      If AircraftID = 0 Then
        AircraftID = Session.Item("ListingID")
      End If


      Try
        If Not Page.IsPostBack Then
          Dim aircraftTable As New DataTable
          aircraftTable = CommonAircraftFunctions.BuildReusableTable(AircraftID, 0, "CLIENT", "", aclsData_Temp, True, 0, "CLIENT")

          If Not IsNothing(aircraftTable) Then
            If aircraftTable.Rows.Count > 0 Then
              title_change.Text = CommonAircraftFunctions.CreateHeaderLine(aircraftTable.Rows(0).Item("amod_make_name"), aircraftTable.Rows(0).Item("amod_model_name"), aircraftTable.Rows(0).Item("ac_ser_nbr"), "")
            End If
          End If

          bind_data()
          aTempTable = aclsData_Temp.Get_Client_Aircraft_Data_Type("avionics")
          If Not IsNothing(aTempTable) Then
            For Each r As DataRow In aTempTable.Rows
              cliav_name.Items.Add(New ListItem(r("cliadt_data_name"), r("cliadt_data_name")))
            Next
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("Aircraft_Edit_Avionics_Tab.ascx.vb - Page_Load() " & error_string, aclsData_Temp)
            End If
          End If
        End If
      Catch ex As Exception
        error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - Page_Load() " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region
#Region "DataGrid Events"
  Private Sub datagrid_avionics_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles datagrid_avionics.ItemDataBound
    Try
      Dim sel As TextBox = e.Item.FindControl("name_hidden")
      If Not IsNothing(e.Item.FindControl("name_type")) Then
        Dim ddl As DropDownList = e.Item.FindControl("name_type")
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Data_Type("avionics")

        For Each r As DataRow In aTempTable.Rows
          ddl.Items.Add(New ListItem(r("cliadt_data_name"), r("cliadt_data_name")))
        Next
        ddl.SelectedValue = sel.Text
      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - datagrid_avionics_ItemDataBound - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try

  End Sub
  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim name As TextBox = e.Item.FindControl("name_hidden")
      Dim id As TextBox = e.Item.FindControl("id_hidden")
      Dim description As TextBox = e.Item.FindControl("description_hidden")

      Dim name_new As DropDownList = e.Item.FindControl("name_type")
      Dim description_new As TextBox = e.Item.FindControl("description")

      If aclsData_Temp.Update_Client_Aircraft_Avionics(id.Text, name_new.SelectedValue, description_new.Text, id.Text, name.Text, description.Text) = 1 Then
        datagrid_avionics.EditItemIndex = -1
        bind_data()

        updated.Text = "<p align=""center"">Your information has been edited.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - MyDataGrid_Update - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim name As TextBox = e.Item.FindControl("name_delete")
      Dim id As TextBox = e.Item.FindControl("id_delete")
      Dim description As TextBox = e.Item.FindControl("description_delete")
      If aclsData_Temp.Delete_Client_Aircraft_Avionics(id.Text, name.Text, description.Text) = 1 Then
        datagrid_avionics.EditItemIndex = -1
        bind_data()

        updated.Text = "<p align=""center"">Your information has been removed.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - MyDataGrid_Delete - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      datagrid_avionics.EditItemIndex = -1
      bind_data()

      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - MyDataGrid_Cancel - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_avionics.EditItemIndex = CInt(E.Item.ItemIndex)
      bind_data()
      datagrid_avionics.DataBind()
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - MyDataGrid_Edit - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Insert/Add New Row Event"
  Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    Try
      new_row.Visible = True
      add_new.Visible = False
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - add_new_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert.Click
    Try
      If aclsData_Temp.Insert_Client_Aircraft_Avionics(AircraftID, cliav_name.Text, cliav_description.Text) = 1 Then
        bind_data()
        new_row.Visible = False
        add_new.Visible = True
        datagrid_avionics.EditItemIndex = -1
        updated.Text = "<p align=""center"">Your information has been added.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - insert_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Function to Bind Data"
  Public Sub bind_data()
    Try
      aTempTable = aclsData_Temp.Get_Client_Aircraft_Avionics(AircraftID)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          datagrid_avionics.DataSource = aTempTable
          datagrid_avionics.DataBind()
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - bind_data() - " & error_string
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Avionics_Tab.ascx.vb - bind_data() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region


End Class