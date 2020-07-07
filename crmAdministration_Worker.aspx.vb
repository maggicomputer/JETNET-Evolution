Public Partial Class crmAdministration_Worker
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim CLIENT_ID As Integer = 0
        Dim JETNET_ID As Integer = 0
        Master.TypeOfListing = 9
        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        Else
            Response.Redirect("home.aspx")
        End If


        If Not IsNothing(Request.Item("client_ID")) Then
            If Not String.IsNullOrEmpty(Request.Item("client_ID").ToString) Then
                If IsNumeric(Request.Item("client_ID").Trim) Then
                    CLIENT_ID = Request.Item("client_ID").Trim
                End If
            End If
        End If
        If Not IsNothing(Request.Item("jetnet_ID")) Then
            If Not String.IsNullOrEmpty(Request.Item("jetnet_ID").ToString) Then
                If IsNumeric(Request.Item("jetnet_ID").Trim) Then
                    JETNET_ID = Request.Item("jetnet_ID").Trim
                End If
            End If
        End If
        If Not IsNothing(Request.Item("update_orphan")) Then
            If Not String.IsNullOrEmpty(Request.Item("update_orphan").ToString) Then
                If Request.Item("update_orphan") = "true" Then
                    If JETNET_ID <> 0 And CLIENT_ID <> 0 Then
                        Try
                            Dim counter As String = Master.aclsData_Temp.FIX_Potential_Orphaned_Client_Aircraft_Records(JETNET_ID, CLIENT_ID)
                            If counter <> "" Then
                                'fixed, let's  close.
                                '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='crmAdministration.aspx';", True)
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

                            Else
                                If Master.aclsData_Temp.class_error <> "" Then
                                    Master.error_string = Master.aclsData_Temp.class_error
                                    Master.LogError("crmAdministration_Worker.aspx.vb  FIX_Potential_Orphaned_Client_Aircraft_Records() - " & Master.error_string)
                                End If
                                Master.display_error()
                            End If
                        Catch ex As Exception
                            Master.error_string = "crmAdministration_Worker.aspx.vb - FIX_Potential_Orphaned_Client_Aircraft_Records() - " & ex.Message
                            Master.LogError(Master.error_string)
                        End Try
                    End If
                End If
            End If
        End If

    End Sub

End Class