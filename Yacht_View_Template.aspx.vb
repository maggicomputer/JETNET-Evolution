Partial Public Class Yacht_View_Template
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '   Master.Set_Page_Title("View Page Title")
    'Master.Set_Active_Tab(1)
  End Sub



  Private Sub Yacht_View_Template_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    If Session.Item("localUser").crmEvo = True Then
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        Me.MasterPageFile = "~/EvoStyles/YachtTheme.master"
      End If
    End If
  End Sub
End Class