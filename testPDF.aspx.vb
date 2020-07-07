Partial Public Class testPDF
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

  End Sub

  Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

    Dim reportFolder As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString)
    Dim bReturnValue As Boolean = False
    Dim varURL As String = ""
    Dim varTimeout As Integer = 0

    Dim filename As String = "3769_BFarb_1_aircraft_full_spec__standard_53_348031_7210_4_29_2019_11_31_7_664_AM.html"

    Dim htmlToPdfConverter As New EvoPdf.HtmlToPdfConverter()

    Try

      varURL = reportFolder + "\" + filename.Trim
      varTimeout = 60

      ' Set license key received after purchase to use the converter in licensed mode
      ' Leave it not set to use the converter in demo mode
      ' htmlToPdfConverter.LicenseKey = "" '"31FCUEVAUEJCSFBGXkBQQ0FeQUJeSUlJSVBA" old key
      htmlToPdfConverter.LicenseKey = "9Xtoem9qemp6bXRqemlrdGtodGNjY2N6ag=="

      ' Set HTML Viewer width in pixels which is the equivalent in converter of the browser window width
      htmlToPdfConverter.HtmlViewerWidth = 1024

      ' Set HTML viewer height in pixels to convert the top part of a HTML page 
      ' Leave it not set to convert the entire HTML
      htmlToPdfConverter.HtmlViewerHeight = 0

      ' Set PDF page size which can be a predefined size like A4 or a custom size in points 
      ' Leave it not set to have a default A4 PDF page
      htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = EvoPdf.PdfPageSize.A4

      ' Set PDF page orientation to Portrait or Landscape
      ' Leave it not set to have a default Portrait orientation for PDF page
      htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = EvoPdf.PdfPageOrientation.Portrait

      ' Set the maximum time in seconds to wait for HTML page to be loaded 
      ' Leave it not set for a default 60 seconds maximum wait time
      htmlToPdfConverter.NavigationTimeout = varTimeout

      ' Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
      ' Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
      htmlToPdfConverter.ConversionDelay = 0

      htmlToPdfConverter.ConvertUrlToFile(varURL, reportFolder + "\" + commonEvo.GenerateFileName(filename.Trim, ".pdf", True))

      bReturnValue = True

    Catch ex As Exception

      TextBox1.Text = "error in convert_to_pdf: " + ex.Message

    Finally

      ' Clear Objects
      htmlToPdfConverter = Nothing

    End Try
  End Sub

End Class