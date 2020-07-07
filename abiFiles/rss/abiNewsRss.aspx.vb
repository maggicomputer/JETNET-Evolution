Imports System.IO

Partial Public Class abiNewsRss
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim abiDataManager As New abi_functions
    Dim ArticleTable As New DataTable

    abiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")

    ArticleTable = abiDataManager.GetLatestJetnetNews(5)

    Response.Buffer = True
    Response.CacheControl = "no-cache"
    Response.ContentType = "text/xml"

    Response.Write(vbCrLf & "<rss version=" & Chr(34) & "2.0" & Chr(34) & ">")
    Response.Write(vbCrLf & "  <channel>")
    Response.Write(vbCrLf & "    <title>Latest News at JETNET Global</title>")
    Response.Write(vbCrLf & "    <link>http://www.jetnetglobal.com/</link>")
    Response.Write(vbCrLf & "    <description>Latest news from JETNET Global</description>")
    Response.Write(vbCrLf & "    <language>en-us</language>")
    Response.Write(vbCrLf & "    <copyright>" & Year(Now()) & " JETNET Global - Jetnet (All Rights Reserved)</copyright>")
    Response.Write(vbCrLf & "    <lastBuildDate>" & Now() & "</lastBuildDate>")


    If Not IsNothing(ArticleTable) Then
      For Each q As DataRow In ArticleTable.Rows

        'the date
        Dim CurrHour As String = ""
        Dim CurrMin As String = ""
        Dim CurrSec As String = ""
        Dim CurrDateT As String = ""

        CurrHour = Hour(q("evonot_release_date"))
        If CurrHour < 10 Then CurrHour = "0" & CurrHour

        CurrMin = Minute(q("evonot_release_date"))
        If CurrMin < 10 Then CurrMin = "0" & CurrMin

        CurrSec = Second(q("evonot_release_date"))
        If CurrSec < 10 Then CurrSec = "0" & CurrSec

        CurrDateT = WeekdayName(Weekday(q("evonot_release_date")), True) & ", " & Day(q("evonot_release_date")) & " " & _
        MonthName(Month(q("evonot_release_date")), True) & " " & Year(q("evonot_release_date")) & " " & _
        CurrHour & ":" & CurrMin & ":" & CurrSec & " GMT"

        Response.Write(vbCrLf & "    <item>")
        Response.Write(vbCrLf & "      <title>" & DisplayFunctions.ApplyXMLFormatting(q("evonot_title")) & "</title>")
        Response.Write(vbCrLf & "      <link>" & DisplayFunctions.ApplyXMLFormatting(IIf(InStr(q("evonot_doc_link"), "http://") > 0, q("evonot_doc_link"), "http://" & q("evonot_doc_link"))) & "</link>")
        Response.Write(vbCrLf & "      <description><![CDATA[" & IIf(Not IsDBNull(q("evonot_announcement")), q("evonot_announcement"), "") & "]]></description>")
        Response.Write(vbCrLf & "      <pubDate>" & CurrDateT & "</pubDate>")
        Response.Write(vbCrLf & "    </item>")

      Next
    End If


    Response.Write(vbCrLf & "     </channel>")
    Response.Write(vbCrLf & "  </rss>")
  End Sub

End Class