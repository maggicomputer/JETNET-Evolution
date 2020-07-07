Imports System.IO
Partial Public Class RecordNavBar
  Inherits System.Web.UI.UserControl
  Dim error_string As String = ""

#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'Dim sub_string As String = ""
    'sub_string = "<table width='100%' cellspacing='0' cellpadding='0'><tr>"
    'If Not Page.IsPostBack Then
    '  If Me.Visible Then
    '    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    '    Try

    '      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
    '        sub_string = sub_string & "<td align='left' valign='middle'><a href=""#"" rel=""anylinkmenu_sub2"" class=""menuanchorclass"">Admin</a></td>" '<td align='left' valign='middle'><a href=""#"" rel=""anylinkmenu_sub1"" class=""menuanchorclass"">Folders</a></td>"
    '      End If
    '      sub_string = sub_string & "</tr></table>"

    '      sub_menu_text.Text = sub_string
    '    Catch ex As Exception
    '      error_string = "RecordNavBar.ascx.vb - Page_Load() - " & ex.Message
    '      masterPage.LogError(error_string)
    '    End Try
    '  End If
    'End If
  End Sub
#End Region

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub
End Class