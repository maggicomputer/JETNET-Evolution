
Partial Public Class abiLinks
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim LinkTable As New DataTable
    Dim TopicName As String = ""

    If Not IsNothing(Trim(Request("topic"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("topic"))) Then
        TopicName = Trim(Request("topic"))
        TopicName = Server.UrlDecode(TopicName)
        links_header.InnerHtml = TopicName & " Links"
        viewAllDiv.Visible = True
      End If
    End If

    FillLinkList(LinkTable, TopicName)
    FillLinkTopicList(LinkTable)

    Master.Set_Page_Title("Aviation Links at JETNET Global")

  End Sub

  Private Sub FillLinkTopicList(ByRef LinkTable As DataTable)
    Dim LinkTopicTable As New DataTable
    LinkTopicTable = Master.AbiDataManager.GetABILinkTopicList

    If Not IsNothing(LinkTopicTable) Then
      If LinkTopicTable.Rows.Count > 0 Then
        linksTopics.DataSource = LinkTopicTable
        linksTopics.DataBind()
      End If
    End If

  End Sub
  Private Sub FillLinkList(ByRef LinkTable As DataTable, ByRef TopicName As String)
    LinkTable = Master.AbiDataManager.GetABILinkList(TopicName)
    Dim DisplayString As String = ""
    Dim cbusName As String = ""
    Dim count As Integer = 1
    Dim float As String = "pull-left"

    If Not IsNothing(LinkTable) Then
      If LinkTable.Rows.Count > 0 Then

        For Each r As DataRow In LinkTable.Rows


          If Not IsDBNull(r("cbus_name")) Then
            If cbusName <> r("cbus_name") Then

              'close span3 (two column layout)
              If count = 1 Then
                DisplayString += "<div class=""span8 " & float & """>"
              Else
                DisplayString += "</div>"

                DisplayString += "<div class=""span8 " & float & """>"
              End If


              If float = "pull-left" Then
                float = "pull-right"
              Else
                float = "pull-left"
              End If


              If TopicName = "" Then
                DisplayString += "<br /><h4>" & r("cbus_name") & "</h4>"
              End If

            End If
          End If

          'Displaying correct link syntax even if link is blank so page renders correctly.
          DisplayString += "<a href="""

          'Web Address
          If Not IsDBNull(r("comp_web_address")) Then
            If InStr("http://", r("comp_web_address")) = 0 Then
              DisplayString += "http://" & r("comp_web_address")
            Else
              DisplayString += r("comp_web_address")
            End If
          Else
            DisplayString += "#"
          End If

          'Ending link begining tag no matter what.
          DisplayString += """ target=""new"">"

          'Company Name
          If Not IsDBNull(r("comp_name")) Then
            DisplayString += r("comp_name")
          End If

          'end link tag
          DisplayString += "</a><br />"

          'Resetting topic name to see if we need another header
          If Not IsDBNull(r("cbus_name")) Then
            cbusName = r("cbus_name")
          End If
          count += 1
        Next

        linkListLiteral.Text = DisplayString
      End If
    End If
  End Sub

End Class