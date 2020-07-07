Public Partial Class EmptySessionRefresh
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Write the header - Refresh the page every 10 minutes. 
        Response.AddHeader("Refresh", 600)
    End Sub

End Class