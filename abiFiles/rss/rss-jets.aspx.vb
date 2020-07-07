
Partial Public Class rss_jets
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim abiDataManager As New abi_functions
    abiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")

    abi_functions.BuildAircraftFeed("Latest Jets for Sale at JETNET Global", "Latest Jets For Sale by Aircraft Dealers and Brokers", abiDataManager, "F", "J")

  End Sub

End Class