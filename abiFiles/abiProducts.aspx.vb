' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiProducts.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:43a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: abiProducts.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiProducts
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim TopicName As String = ""
    If Not IsNothing(Trim(Request("topic"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("topic"))) Then
        TopicName = Trim(Request("topic"))
        TopicName = Server.UrlDecode(TopicName)
        product_header.InnerHtml = "<h1>" & TopicName & "</h1>"
      End If
    End If


    Master.Set_Meta_Information("Aircraft for sale, planes for sale, & helicopters for sale including Cessna, Gulfstream, Challenger, Hawker, and Learjet aircraft by Aircraft Dealers & Brokers.", "Aircraft, JETNET Global, business directory, aviation, aircraft, mail list, email list, business list, aircraft for sale, aircraft classified, purchase mail list, fbo, dealer, dealers, news, aviation links, aviation events, aviation products, plane, airplane, airline, airport, pilot, pilots, sale, transportation, charter, jetnet")
    Master.Set_Page_Title(IIf(TopicName <> "", TopicName & " ", "") & "Aviation Industry Products from JETNET Global")

    DisplayProducts(TopicName)
    DisplayProductCategories()
  End Sub
  Private Sub DisplayProductCategories()
    Dim CategoryTable As New DataTable
    CategoryTable = Master.AbiDataManager.GetABIProductsCategoriesList()

    productsCategories.DataSource = CategoryTable
    productsCategories.DataBind()
  End Sub

  Private Sub DisplayProducts(ByVal topicName As String)
    Dim ProductTable As New DataTable
    Dim DisplayString As String = ""
    Dim abiSubGroup As String = ""

    ProductTable = Master.AbiDataManager.GetABIProducts(topicName)

    If Not IsNothing(ProductTable) Then
      For Each r As DataRow In ProductTable.Rows

        If topicName = "" Then
          If abiSubGroup <> r("abiserv_subgroup") Then
            DisplayString += "<span class=""span9""><h4 class='subTitle'>" & r("abiserv_subgroup") & "</h4></span><br /><br />"
          End If
        End If

        DisplayString += "<article class=""item column-1"">"
        DisplayString += "<!-- Intro image -->"
        DisplayString += "<span class=""span3"">"
        DisplayString += "<figure class=""item_img img-intro"">"
        DisplayString += "<a href=""#"">"
        DisplayString += "<img width=""250"" src=""images/blank.gif"" class=""lazy"" data-src=""/images/background/11.jpg"" alt=""""/></span>"
        DisplayString += "</a>"
        DisplayString += "</figure>"
        DisplayString += "</span>"
        DisplayString += "<span class=""span9"">"
        'DisplayString += "<figcaption>" & r("abiserv_subgroup") & "</figcaption>"
        DisplayString += "<!--  title/author -->"
        DisplayString += "<header class=""item_header"">"
        DisplayString += "<h4 class=""item_title"">		"
        DisplayString += "<a href=""#"">"
        DisplayString += "<span>" & r("abiserv_name") & "</span> "
        DisplayString += "</a>"
        DisplayString += "</h4>"
        DisplayString += "</header>"
        DisplayString += "<!-- Introtext -->"
        DisplayString += "<div class=""item_introtext"">"
        DisplayString += r("abiserv_description")
        DisplayString += IIf(r("abiserv_amount") > 0, " <strong>[" & FormatCurrency(r("abiserv_amount"), 2) & "]</strong>", "")
        DisplayString += "</div>"

        DisplayString += "<!-- info TOP -->"
        DisplayString += "<div class=""item_info"">"

        DisplayString += "</div>"
        DisplayString += "</span>"

        DisplayString += "</article><!-- end item -->"
        abiSubGroup = r("abiserv_subgroup")
      Next
    End If

    productText.Text = DisplayString
  End Sub

End Class