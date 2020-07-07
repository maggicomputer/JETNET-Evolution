Partial Public Class macShell
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Response.Buffer = True
    Response.ClearContent()
    Response.ClearHeaders()
    Response.Clear()
    Response.AddHeader("content-disposition", "attachment;filename=export.xls")
    Response.Charset = ""
    Response.Cache.SetCacheability(HttpCacheability.NoCache)
    Response.ContentType = "application/vnd.xls"

        Dim DisplayString As String = Uri.UnescapeDataString(Trim(Request("data")))

        Response.Write("<table>" & DisplayString & "</table>")

    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "self.close();", True)
  End Sub

End Class