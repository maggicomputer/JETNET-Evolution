Partial Public Class picture
  Inherits System.Web.UI.Page
  Public aclsData_Temp As New clsData_Manager_SQL
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim url As String = Trim(Request("url"))
    Dim ACID As Long = 12798
    Dim JOURNALID As Long = 0
    Dim SEQNO As Long = 0
    Dim fAcpic_image_type As String = ""
    Dim fAcpic_id As String = ""
    Dim fAcpic_subject As String = ""

    Dim imgFolder As String = ""
    Dim theImgFile As String = ""
    Dim sTransDocHtml As String = ""
    Dim Journ_Subject As String = ""
    Dim fAdoc_doc_date As String = ""
    Dim hDocumentFile As String = ""
    Dim PictureTable As New DataTable
    Dim refTable As New DataTable

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      'display Single Picture
      If url <> "/picture.aspx" Then
        url = Trim(Request("url"))
        Dim websitePath As String = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + Session.Item("AircraftPicturesFolderVirtualPath")
        If Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
          websitePath = "https://www.testjetnetevolution.com/pictures/aircraft/"
        End If
        Master.SetPageTitle("Picture")
        Master.SetPageText("")

        picture_plain.Text = ("<img src='" & websitePath.ToString.Trim & "/" & url & "' /><br /><p align=""center""><a href=""#"" onclick=""javascript:window.close();"">Close Window</a></p>")
      ElseIf Page.Request.Form("document") <> "" Then
        'initialize connection string for the document page.
        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
        aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")


        ACID = Page.Request.Form("acID")
        JOURNALID = Page.Request.Form("journalID")
        SEQNO = Page.Request.Form("document")

        If CommonAircraftFunctions.displayTransactionDocuments(ACID, JOURNALID, SEQNO, True, False, False, False, Me.Application, Me.Session, sTransDocHtml, hDocumentFile, Journ_Subject, fAdoc_doc_date, aclsData_Temp, True, Nothing, "") Then

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
            hDocumentFile = "https://www.jetnetevolution.com/" + hDocumentFile
          End If

          Master.SetPageTitle(Page.Request.Form("make") & " " & Page.Request.Form("model") & " S/N " & Page.Request.Form("serial") & " Document")

          picture_plain.Text = ("<div class=""DetailsBrowseTable""><div class=""backgroundShade""><a href=""#"" onclick=""javascript:window.close();"" class=""gray_button float_right""><strong>Close</strong></a><a href=""" & hDocumentFile & """ target=""new"" class=""gray_button float_right noBefore""><strong>Open Document In New Window</strong></a></div></div><div>") & vbNewLine
          picture_plain.Text += ("<div id=""container"">") & vbNewLine
          picture_plain.Text += "<table width='100%' cellspacing='6' align='center' cellpadding='6' class='float_left medium_text'>"
          picture_plain.Text += "<tr>"
          picture_plain.Text += "<td align='left' valign='top'><b class='title'>AIRCRAFT DOCUMENTATION DETAILS</b></td>"
          picture_plain.Text += "</tr>"
          picture_plain.Text += "<tr>"
          picture_plain.Text += "<td align='left' valign='top'>" & fAdoc_doc_date & " - " & Journ_Subject & "</td>"
          picture_plain.Text += "</tr>"
          picture_plain.Text += "<tr>"
          picture_plain.Text += "<td align='left' valign='top'>" & sTransDocHtml & "</td>"
          picture_plain.Text += "</tr>"
          picture_plain.Text += "</table><br /><br clear='all' /><br />"
          picture_plain.Text += "<iframe src='" & hDocumentFile & "' width='100%' height='600'></iframe></div></div>"

        End If

      Else 'Display List of Pictures.

        ACID = Page.Request.Form("acID")
        JOURNALID = Page.Request.Form("journalID")
        'initialize connection string for the picture page.
        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
        aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

        PictureTable = aclsData_Temp.GetJETNET_AC_pictures(ACID, JOURNALID)

        If Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
          imgFolder = "https://www.testjetnetevolution.com/pictures/aircraft/"
        Else
          imgFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath")
        End If

        'picture.Text = ("<table cellspacing='0' cellpadding='0' width='520'>")
        'picture.Text += ("<tr><td align='center' valign='top'>")

        '<!-- Start Advanced Gallery Html Containers -->
        controls_buttons.Visible = True
        picture.Text = ("<div id=""page"">") & vbNewLine
        picture.Text += ("<div id=""container"">") & vbNewLine
        picture.Text += ("<div id=""controls"" class=""controls""></div>") & vbNewLine
        picture.Text += ("<div id=""gallery"" class=""content"">") & vbNewLine

        picture.Text += ("<div class=""slideshow-container"">") & vbNewLine
        picture.Text += ("<div id=""loading"" class=""loader""></div>") & vbNewLine
        picture.Text += ("<div id=""slideshow"" class=""slideshow""></div>") & vbNewLine
        picture.Text += ("<div id=""caption"" class=""caption-container"" style='opacity:0.7'></div>") & vbNewLine
        picture.Text += ("</div>") & vbNewLine
        ' picture.Text += ("<div id=""captionToggle"" >") & vbNewLine
        ' picture.Text += ("<a href=""#toggleCaption"" class=""off"" title=""Show Caption"">Show Caption</a>") & vbNewLine
        ' picture.Text += ("</div>") & vbNewLine
        picture.Text += ("</div>") & vbNewLine
        picture.Text += ("<div id=""thumbs"" class=""navigation"">") & vbNewLine
        picture.Text += ("<ul class=""thumbs noscript"">") & vbNewLine

        picture_plain.Text = ("<div id=""page"">") & vbNewLine
        picture_plain.Text += ("<div id=""container"">") & vbNewLine
        Master.SetPageTitle(Page.Request.Form("make") & " " & Page.Request.Form("model") & " S/N " & Page.Request.Form("serial") & " Pictures")

        ' picture_plain.Text += "<h2><span class='float_left'>Make: " & Page.Request.Form("make") & " Model: " & Page.Request.Form("model") & "</span><span class='float_right'>Serial Number: " & Page.Request.Form("serial") & "</span></h2><br />"
        If Not IsNothing(PictureTable) Then
          If PictureTable.Rows.Count > 0 Then
            For Each r As DataRow In PictureTable.Rows
              fAcpic_image_type = ""
              fAcpic_id = ""
              fAcpic_subject = ""

              If Not (IsDBNull(r("acpic_image_type"))) Then
                If Not String.IsNullOrEmpty(r("acpic_image_type").ToString) Then
                  fAcpic_image_type = r("acpic_image_type").ToString.ToLower.Trim
                End If
              End If

              If Not (IsDBNull(r("acpic_id"))) Then
                If Not String.IsNullOrEmpty(r("acpic_id").ToString) Then
                  fAcpic_id = r("acpic_id").ToString.Trim
                End If
              End If

              If Not (IsDBNull(r("acpic_subject"))) Then
                If Not String.IsNullOrEmpty(r("acpic_subject").ToString) Then
                  fAcpic_subject = r("acpic_subject").ToString.Trim
                End If
              End If


              theImgFile = imgFolder + "/" + ACID.ToString + Constants.cHyphen + JOURNALID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type

              picture.Text += ("<li>") & vbNewLine
              picture.Text += ("<a class=""thumb"" name=""leaf"" href=""" + imgFolder + "/" + ACID.ToString + Constants.cHyphen + JOURNALID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """ title=""" + fAcpic_subject + """>") & vbNewLine
              picture.Text += ("<img src=""" + imgFolder + "/" + ACID.ToString + Constants.cHyphen + JOURNALID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """ alt=""" + fAcpic_subject + " width='550px' "" />") & vbNewLine
              picture.Text += ("</a>") & vbNewLine
              picture.Text += ("<div class=""caption"">") & vbNewLine
              picture.Text += ("<div class=""download"">")
              picture.Text += ("<a href=""" + imgFolder + "/" + ACID.ToString + Constants.cHyphen + JOURNALID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """ target='new'>View Original</a>") & vbNewLine
              picture.Text += ("</div>") & vbNewLine
              picture.Text += ("<div class=""image-title"">" + fAcpic_subject + "</div>") & vbNewLine
              picture.Text += ("<div class=""image-desc"">&nbsp;</div>") & vbNewLine
              picture.Text += ("</div>") & vbNewLine
              picture.Text += ("</li>") & vbNewLine

              picture_plain.Text += ("<img border='0' src='" + imgFolder + "/" + ACID.ToString + Constants.cHyphen + JOURNALID.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "'  width='650' style='padding-bottom:3px;' /><h3 class='caption_text_large'>" & fAcpic_subject & "</h3><br /><br />")

            Next
          End If
        End If

        picture.Text += ("</ul>") & vbNewLine
        picture.Text += ("</div>") & vbNewLine
        picture.Text += ("<!-- End Advanced Gallery Html Containers -->") & vbNewLine
        picture.Text += ("<div style=""clear: both;""></div>") & vbNewLine
        picture.Text += ("</div>") & vbNewLine
        picture.Text += ("</div>") & vbNewLine

        picture_plain.Text += ("</div>") & vbNewLine
        picture_plain.Text += ("</div>") & vbNewLine

        javascript_text.Text += ("<script type=""text/javascript"">") & vbNewLine
        javascript_text.Text += ("jQuery(document).ready(function($) {") & vbNewLine
        javascript_text.Text += ("// We only want these styles applied when javascript is enabled") & vbNewLine
        javascript_text.Text += ("$('div.navigation').css({'width' : '300px', 'float' : 'left'});") & vbNewLine
        javascript_text.Text += ("$('div.content').css('display', 'block');") & vbNewLine

        javascript_text.Text += ("// Initially set opacity on thumbs and add") & vbNewLine
        javascript_text.Text += ("// additional styling for hover effect on thumbs") & vbNewLine
        javascript_text.Text += ("var onMouseOutOpacity = 0.67;") & vbNewLine
        javascript_text.Text += ("jQuery('#thumbs ul.thumbs li').opacityrollover({") & vbNewLine
        javascript_text.Text += ("mouseOutOpacity:   onMouseOutOpacity,") & vbNewLine
        javascript_text.Text += ("mouseOverOpacity:  1.0,") & vbNewLine
        javascript_text.Text += ("fadeSpeed:  'fast',") & vbNewLine
        javascript_text.Text += ("exemptionSelector:  '.selected'") & vbNewLine
        javascript_text.Text += ("});") & vbNewLine

        'javascript_text.Text += ("// Enable toggling of the caption") & vbNewLine
        'javascript_text.Text += ("var captionOpacity = 0.0;") & vbNewLine
        'javascript_text.Text += ("$('#captionToggle a').click(function(e) {") & vbNewLine
        'javascript_text.Text += ("var link = $(this);") & vbNewLine

        'javascript_text.Text += ("var isOff = link.hasClass('off');") & vbNewLine
        'javascript_text.Text += ("var removeClass = isOff ? 'off' : 'on';") & vbNewLine
        'javascript_text.Text += ("var addClass = isOff ? 'on' : 'off';") & vbNewLine
        'javascript_text.Text += ("var linkText = isOff ? 'Hide Caption' : 'Show Caption';") & vbNewLine
        'javascript_text.Text += ("captionOpacity = isOff ? 0.7 : 0.0;") & vbNewLine

        'javascript_text.Text += ("link.removeClass(removeClass).addClass(addClass).text(linkText).attr('title', linkText);") & vbNewLine
        'javascript_text.Text += ("$('#caption span.image-caption').fadeTo(1000, captionOpacity);") & vbNewLine

        'javascript_text.Text += ("e.preventDefault();") & vbNewLine
        'javascript_text.Text += ("});") & vbNewLine


        javascript_text.Text += ("// Initialize Advanced Galleriffic Gallery") & vbNewLine
        javascript_text.Text += ("var gallery = $('#thumbs').galleriffic({") & vbNewLine
        javascript_text.Text += ("delay:                     2500,") & vbNewLine
        javascript_text.Text += ("numThumbs:                 15,") & vbNewLine
        javascript_text.Text += ("preloadAhead:              10,") & vbNewLine
        javascript_text.Text += ("enableTopPager:            true,") & vbNewLine
        javascript_text.Text += ("enableBottomPager:         true,") & vbNewLine
        javascript_text.Text += ("maxPagesToShow:            7,") & vbNewLine
        javascript_text.Text += ("imageContainerSel:  '#slideshow',") & vbNewLine
        javascript_text.Text += ("controlsContainerSel:  '#controls',") & vbNewLine
        javascript_text.Text += ("captionContainerSel:  '#caption',") & vbNewLine
        javascript_text.Text += ("loadingContainerSel:  '#loading',") & vbNewLine
        javascript_text.Text += ("renderSSControls:          true,") & vbNewLine
        javascript_text.Text += ("renderNavControls:         true,") & vbNewLine
        javascript_text.Text += ("playLinkText:  'Play Slideshow',") & vbNewLine
        javascript_text.Text += ("pauseLinkText:  'Pause Slideshow',") & vbNewLine
        javascript_text.Text += ("prevLinkText:  '&lsaquo; Previous Photo',") & vbNewLine
        javascript_text.Text += ("nextLinkText:  'Next Photo &rsaquo;',") & vbNewLine
        javascript_text.Text += ("nextPageLinkText:  'Next &rsaquo;',") & vbNewLine
        javascript_text.Text += ("prevPageLinkText:  '&lsaquo; Prev',") & vbNewLine
        javascript_text.Text += ("enableHistory:             true,") & vbNewLine
        javascript_text.Text += ("autoStart:                 false,") & vbNewLine
        javascript_text.Text += ("syncTransitions:           true,") & vbNewLine
        javascript_text.Text += ("defaultTransitionDuration: 900,") & vbNewLine
        javascript_text.Text += ("onSlideChange:             function(prevIndex, nextIndex) {") & vbNewLine
        javascript_text.Text += ("// 'this' refers to the gallery, which is an extension of $('#thumbs')") & vbNewLine
        javascript_text.Text += ("this.find('ul.thumbs').children()") & vbNewLine
        javascript_text.Text += (".eq(prevIndex).fadeTo('fast', onMouseOutOpacity).end()") & vbNewLine
        javascript_text.Text += (".eq(nextIndex).fadeTo('fast', 1.0);") & vbNewLine
        javascript_text.Text += ("},") & vbNewLine
        javascript_text.Text += ("onPageTransitionOut:       function(callback) {") & vbNewLine
        javascript_text.Text += ("this.fadeTo('fast', 0.0, callback);") & vbNewLine
        javascript_text.Text += ("},") & vbNewLine
        javascript_text.Text += ("onPageTransitionIn:        function() {") & vbNewLine
        javascript_text.Text += ("this.fadeTo('fast', 1.0);") & vbNewLine
        javascript_text.Text += ("}") & vbNewLine
        javascript_text.Text += ("});") & vbNewLine

        javascript_text.Text += ("/**** Functions to support integration of galleriffic with the jquery.history plugin ****/") & vbNewLine

        javascript_text.Text += ("// PageLoad function") & vbNewLine
        javascript_text.Text += ("// This function is called when:") & vbNewLine
        javascript_text.Text += ("// 1. after calling $.historyInit();") & vbNewLine
        javascript_text.Text += ("// 2. after calling $.historyLoad();") & vbNewLine
        javascript_text.Text += ("// 3. after pushing ""Go Back"" button of a browser") & vbNewLine
        javascript_text.Text += ("function pageload(hash) {") & vbNewLine
        javascript_text.Text += ("	// alert(""pageload: "" + hash);") & vbNewLine
        javascript_text.Text += ("	// hash doesn't contain the first # character.") & vbNewLine
        javascript_text.Text += ("	if(hash) {") & vbNewLine
        javascript_text.Text += ("		$.galleriffic.gotoImage(hash);") & vbNewLine
        javascript_text.Text += ("} else {") & vbNewLine
        javascript_text.Text += ("	gallery.gotoIndex(0);") & vbNewLine
        javascript_text.Text += ("}") & vbNewLine
        javascript_text.Text += ("}") & vbNewLine

        javascript_text.Text += ("// Initialize history plugin.") & vbNewLine
        javascript_text.Text += ("// The callback is called at once by present location.hash. ") & vbNewLine
        javascript_text.Text += ("$.historyInit(pageload, ""advanced.html"");") & vbNewLine

        javascript_text.Text += ("// set onlick event for buttons using the jQuery 1.3 live method") & vbNewLine
        javascript_text.Text += ("$(""a[rel='history']"").live('click', function(e) {") & vbNewLine
        javascript_text.Text += ("if (e.button != 0) return true;") & vbNewLine

        javascript_text.Text += ("var hash = this.href;") & vbNewLine
        javascript_text.Text += ("hash = hash.replace(/^.*#/, '');") & vbNewLine

        javascript_text.Text += ("// moves to a new page. ") & vbNewLine
        javascript_text.Text += ("// pageload is called at once. ") & vbNewLine
        javascript_text.Text += ("// hash don't contain ""#"", ""?""") & vbNewLine
        javascript_text.Text += ("$.historyLoad(hash);") & vbNewLine

        javascript_text.Text += ("return false;") & vbNewLine
        javascript_text.Text += ("});") & vbNewLine

        javascript_text.Text += ("/****************************************************************************************/") & vbNewLine
        javascript_text.Text += ("});") & vbNewLine
        javascript_text.Text += ("</script>") & vbNewLine
      End If
      End If



  End Sub

End Class