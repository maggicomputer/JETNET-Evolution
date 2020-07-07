Partial Public Class View_Template
    Inherits System.Web.UI.Page
    Dim masterPage As New Object
    Private Sub View_Template_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim ViewID As Integer = 0

        If Not IsNothing(Request.Item("ViewID")) Then
            If Not String.IsNullOrEmpty(Request.Item("ViewID").ToString) Then
                ViewID = CInt(Request.Item("ViewID"))
            End If
        End If

        If ViewID = 27 Then
            View_Master1.Visible = False
            Value_View1.Visible = True
            MobileView1.Visible = False
            'masterPage.MenuBarVisibility(False)
            Call commonLogFunctions.Log_User_Event_Data("UserDisplayView", "User Entered View " & Replace(commonEvo.Get_Default_User_View_Name(ViewID), "&nbsp;", " "), Nothing, ViewID, 0, 0, 0, 0, 0, 0)

        ElseIf Session.Item("isMobile") = True Then
            View_Master1.Visible = False
            Value_View1.Visible = False
            MobileView1.Visible = True
            Call commonLogFunctions.Log_User_Event_Data("UserDisplayView", "User Entered View " & Replace(commonEvo.Get_Default_User_View_Name(ViewID), "&nbsp;", " "), Nothing, ViewID, 0, 0, 0, 0, 0, 0)

        Else
            MobileView1.Visible = False
            'This is a check. We need to first of all determine if they're passing an amod ID in the url and second of all figure out if that model ID is a valid one
            'for their subscription.
            Dim ModelID As Long = 0
            Dim TemporaryModelCheckTable As New DataTable
            If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                If Not IsNothing(Request.Item("amod_id")) Then 'It does exist.
                    If Not String.IsNullOrEmpty(Request.Item("amod_id").ToString) Then 'It isn't empty.
                        If IsNumeric(Request.Item("amod_id")) Then 'It is numeric.
                            If CLng(Request.Item("amod_id")) <> -1 Then 'It doesn't equal -1
                                If CLng(Request.Item("amod_id")) > 0 Then 'Greater than zero.
                                    ModelID = CLng(Request.Item("amod_id"))

                                    'The model ID passed inspection, let's go ahead and run a very minor check.

                                    TemporaryModelCheckTable = masterPage.aclsData_Temp.GetJetnetModelInfo(ModelID, False, "View_Template.aspx.vb")

                                    If Not IsNothing(TemporaryModelCheckTable) Then
                                        If TemporaryModelCheckTable.Rows.Count = 0 Then

                                            'This means that the model ID isn't found.
                                            Response.Redirect("view_template.aspx?noMaster=" + Request.Item("noMaster").ToString.Trim + "&ViewID=" + Request.Item("ViewID").ToString.Trim + "&ViewName=" + Request.Item("ViewName").ToString.Trim + "&amod_id=-1", True)
                                            Context.ApplicationInstance.CompleteRequest()

                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            If Trim(Request("ViewID")) = "18" Then
                masterPage.setPageText("PROSPECT MANAGEMENT")
            End If
            'Else
            '    masterPage.Set_Active_Tab(1)
        End If


    End Sub


    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
                masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
                If Session.Item("isMobile") Then
                    Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
                    masterPage = DirectCast(Page.Master, MobileTheme)

                End If
            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If
        End Try

    End Sub
End Class