Public Partial Class genericError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT) Then
            crm_error_message.Visible = False
            evo_error_message.Visible = True
            evo_title.Text = "Yacht Spot"
            Me.Page.Title = "Yacht Spot has experienced an error."
        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Then
            crm_error_message.Visible = False
            evo_error_message.Visible = True
            evo_title.Text = "Jetnet Evolution"
            Me.Page.Title = "Jetnet Evolution has experienced an error."
        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
            crm_error_message.Visible = False
            evo_error_message.Visible = True
            evo_title.Text = "Evolution Customer Center"
            Me.Page.Title = "Evolution Customer Center has experienced an error."
        Else
            crm_error_message.Visible = True
            evo_error_message.Visible = False
            Me.Page.Title = "Jetnet CRM has experienced an error."
        End If
    End Sub

End Class