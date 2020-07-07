Partial Public Class aviationrss
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim abiDataManager As New abi_functions
    abiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")

    abi_functions.BuildAircraftFeed("Latest Aircraft for Sale at JETNET Global", "Latest Aircraft For Sale by Aircraft Dealers and Brokers", abiDataManager, "", "")
  End Sub

End Class