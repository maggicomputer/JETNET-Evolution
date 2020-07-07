Imports System.IO
Partial Public Class Aircraft_Edit_Propeller_Tab
    Inherits System.Web.UI.UserControl
    Public aclsData_Temp As New Object
    Public aTempTable, aTempTable2 As New DataTable 'Data Tables used 
    Dim error_string As String = ""
#Region "Page Events/Includes Filling Textboxes"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            Try
              
                If Session.Item("crmUserLogon") <> True Then
                    Response.Redirect("Default.aspx", False)
                End If

                aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

                Session("export_info") = ""
               
                '---------------------------------------------End Database Connection Stuff---------------------------------------------


                If Not Page.IsPostBack Then
                    aTempTable2 = aclsData_Temp.Get_Client_Aircraft_Propeller(Session.Item("ListingID"))
                    If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                            For Each R As DataRow In aTempTable2.Rows

                                If Not IsDBNull(R("cliacpr_prop_1_ser_nbr")) Then
                                    prop_1_ser.Text = R("cliacpr_prop_1_ser_nbr")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_2_ser_nbr")) Then
                                    prop_2_ser.Text = R("cliacpr_prop_2_ser_nbr")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_3_ser_nbr")) Then
                                    prop_3_ser.Text = R("cliacpr_prop_3_ser_nbr")
                                End If

                                If Not IsDBNull(R("cliacpr_prop_1_ttsn_hours")) Then
                                    prop_1_ttsnew.Text = R("cliacpr_prop_1_ttsn_hours")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_2_ttsn_hours")) Then
                                    prop_2_ttsnew.Text = R("cliacpr_prop_2_ttsn_hours")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_3_ttsn_hours")) Then
                                    prop_3_ttsnew.Text = R("cliacpr_prop_3_ttsn_hours")
                                End If

                                If Not IsDBNull(R("cliacpr_prop_1_tsoh_hours")) Then
                                    prop_1_soh.Text = R("cliacpr_prop_1_tsoh_hours")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_2_tsoh_hours")) Then
                                    prop_2_soh.Text = R("cliacpr_prop_2_tsoh_hours")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_3_tsoh_hours")) Then
                                    prop_3_soh.Text = R("cliacpr_prop_3_tsoh_hours")
                                End If

                                If Not IsDBNull(R("cliacpr_prop_1_month_year_oh")) Then
                                    prop_1_sohyrs.Text = R("cliacpr_prop_1_month_year_oh")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_2_month_year_oh")) Then
                                    prop_2_sohyrs.Text = R("cliacpr_prop_2_month_year_oh")
                                End If
                                If Not IsDBNull(R("cliacpr_prop_3_month_year_oh")) Then
                                    prop_3_sohyrs.Text = R("cliacpr_prop_3_month_year_oh")
                                End If


                            Next
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = "Aircraft_Edit_Propeller_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                        End If
                    End If
                End If
            Catch ex As Exception
                error_string = "Aircraft_Edit_Propeller_Tabs.ascx.vb - Page Load() - " & ex.Message
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try
        End If
    End Sub
#End Region
#Region "Update Event"
    Private Sub update_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles update.Click
        Try
            Dim aclsClient_Aircraft_Propeller As New clsClient_Aircraft_Propeller

            aclsClient_Aircraft_Propeller.cliacpr_cliac_id = Session.Item("ListingID")

            If Not IsDBNull(prop_1_ser.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_1_ser_nbr = prop_1_ser.Text
            End If
            If Not IsDBNull(prop_2_ser.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_2_ser_nbr = prop_2_ser.Text
            End If
            If Not IsDBNull(prop_3_ser.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_3_ser_nbr = prop_3_ser.Text
            End If
            If Not IsDBNull(prop_1_ttsnew.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_1_ttsn_hours = prop_1_ttsnew.Text
            End If
            If Not IsDBNull(prop_2_ttsnew.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_2_ttsn_hours = prop_2_ttsnew.Text
            End If
            If Not IsDBNull(prop_3_ttsnew.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_3_ttsn_hours = prop_3_ttsnew.Text
            End If
            If Not IsDBNull(prop_1_soh.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_1_tsoh_hours = prop_1_soh.Text
            End If
            If Not IsDBNull(prop_2_soh.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_2_tsoh_hours = prop_2_soh.Text
            End If
            If Not IsDBNull(prop_3_soh.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_3_tsoh_hours = prop_3_soh.Text
            End If

            If Not IsDBNull(prop_1_sohyrs.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_1_month_year_oh = prop_1_sohyrs.Text
            End If
            If Not IsDBNull(prop_2_sohyrs.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_2_month_year_oh = prop_2_sohyrs.Text
            End If
            If Not IsDBNull(prop_3_sohyrs.Text) Then
                aclsClient_Aircraft_Propeller.cliacpr_prop_3_month_year_oh = prop_3_sohyrs.Text
            End If

            aclsData_Temp.Update_Client_Aircraft_Propeller(aclsClient_Aircraft_Propeller)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Catch ex As Exception
            error_string = "Aircraft_Edit_Propeller_Tabs.ascx.vb - update_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
#End Region

End Class