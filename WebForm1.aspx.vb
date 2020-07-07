Partial Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
        End If
    End Sub

 
End Class