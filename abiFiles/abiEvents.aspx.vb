Partial Public Class abiEvents
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Master.Set_Meta_Information("The ABI aviation industry events calendar provides the aircraft industry with lists of trade shows, seminars, conferences, and workshops world wide.", "aviation industry events, aviation industry calendar, Aircraft, jetnet global, business directory, aviation, aircraft, mail list, email list, business list, aircraft for sale, aircraft classified, purchase mail list, fbo, dealer, dealers, news, aviation links, aviation events, aviation products, plane, airplane, airline, airport, pilot, pilots, sale, transportation, charter, jetnet")
    Master.Set_Page_Title("Aviation Industry Events at JETNET Global")

    FillEventsList()
  End Sub

  Private Sub FillEventsList()
    Dim eventTable As New DataTable
    eventTable = Master.AbiDataManager.GetABIEventList(0)

    eventRepeater.DataSource = eventTable
    eventRepeater.DataBind()

  End Sub
End Class