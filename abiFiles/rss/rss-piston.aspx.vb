
Partial Public Class rss_piston
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim abiDataManager As New abi_functions
    abiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")

    abi_functions.BuildAircraftFeed("Latest Pistons for Sale at JETNET Global", "Latest Pistons For Sale by Aircraft Dealers and Brokers", abiDataManager, "F", "P")

  End Sub

End Class