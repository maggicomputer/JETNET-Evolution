Public Partial Class ViewLogs
    Inherits System.Web.UI.UserControl
    Dim aclsData_Temp As New Object
    Dim error_string As String = ""
    Dim tempTable As New DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        End If
        If Me.Visible Then
            aclsData_Temp = New clsData_Manager_SQL
            aclsData_Temp.class_error = ""


            If Not Page.IsPostBack Then



                tempTable = aclsData_Temp.View_CRM_Event("CRM EXPORT", Application.Item("crmClientSiteData").crmClientHostName, "")
                If Not IsNothing(tempTable) Then
                    If tempTable.Rows.Count > 0 Then
                        event_log.DataSource = tempTable
                        event_log.DataBind()
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = "ViewLogs.ascx.vb - Page Load() - " & aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                    End If
                End If
            End If
        End If
    End Sub


End Class