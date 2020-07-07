Partial Public Class rss_turbine
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim abiDataManager As New abi_functions
    abiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")

    abi_functions.BuildAircraftFeed("Latest Turbines for Sale at JETNET Global", "Latest Turbines For Sale by Aircraft Dealers and Brokers", abiDataManager, "F", "T")

  End Sub

End Class