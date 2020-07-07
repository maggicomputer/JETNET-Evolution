Partial Public Class abiDealer
  Inherits System.Web.UI.Page

  Private Sub abiDealer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim CountryName As String = ""

    If Not IsNothing(Trim(Request("country"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("country"))) Then
        CountryName = Trim(Request("country"))
      End If
    End If

    'If Not IsNothing(Trim(Request("toggle"))) Then
    '  If Not String.IsNullOrEmpty(Trim(Request("toggle"))) Then
    '    aside_right.Visible = False
    '    component.Attributes.Remove("class")
    '    component.Attributes.Add("class", "span9")
    '  End If
    'End If


    FillABIDealers(CountryName)
    FillABIDealerCountries()


  End Sub

  Private Sub FillABIDealerCountries()
    Dim CountryTable As New DataTable
    CountryTable = Master.AbiDataManager.GetABIDealersCountry()

    If Not IsNothing(CountryTable) Then
      dealerCountry.DataSource = CountryTable
      dealerCountry.DataBind()
    End If
  End Sub
  Private Sub FillABIDealers(ByRef CountryName As String)

    Dim DealersTable As New DataTable
    DealersTable = Master.AbiDataManager.GetABIDealers(False, CountryName)

    If Not IsNothing(DealersTable) Then
      dealersRepeater.DataSource = DealersTable
      dealersRepeater.DataBind()
    End If

  End Sub


  Public Function setImagePath(ByVal comp_id As String) As String

    If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
      Return "https://www.jetnetevolution.com/pictures/company/" + comp_id + ".jpg"
      'Return "" & clsData_Manager_SQL.get_site_name & "/pictures/company/" + comp_id + ".jpg"
    Else
      Return HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/company/" + comp_id + ".jpg" '"http://www.jetnetGlobal.com/photos/company"
    End If

  End Function
End Class