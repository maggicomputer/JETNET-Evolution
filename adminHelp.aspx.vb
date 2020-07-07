' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminHelp.aspx.vb $
'$$Author: Mike $
'$$Date: 7/12/19 2:01p $
'$$Modtime: 7/12/19 1:58p $
'$$Revision: 4 $
'$$Workfile: adminHelp.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminHelp
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Private sHelpArea As String = ""
  Private sDisplayOrder As String = ""
  Private sSortOrder As String = "DESC"

  Private nHelpItemID As Integer = -1
  Public bEditHelpItem As Boolean = False
  Public bHasHelpModelItem As Boolean = False
  Public bAddHelpItem As Boolean = False
  Public bHasHelpHintItem As Boolean = False
  Private bDeleteHelpItem As Boolean = False
  Private bSubmitHelpItem As Boolean = False
  Private bReleaseHelpItem As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sDisplayAdminHelpList As String = ""
    Dim sDisplayAdminDetailHelpList As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Not IsNothing(Request.Item("helpArea")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpArea").ToString.Trim) Then
          sHelpArea = Request.Item("helpArea").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("displayOrder")) Then
        If Not String.IsNullOrEmpty(Request.Item("displayOrder").ToString.Trim) Then
          sDisplayOrder = Request.Item("displayOrder").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("sortOrder")) Then
        If Not String.IsNullOrEmpty(Request.Item("sortOrder").ToString.Trim) Then
          If Request.Item("sortOrder").ToString.ToUpper.Contains("DESC") Then
            sSortOrder = "ASC"
          Else
            sSortOrder = "DESC"
          End If
        End If
      End If

      If Not IsNothing(Request.Item("helpId")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpId").ToString.Trim) Then
          If IsNumeric(Request.Item("helpId").ToString) Then
            nHelpItemID = CInt(Request.Item("helpId").ToString)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("add")) Then
        If Not String.IsNullOrEmpty(Request.Item("add").ToString.Trim) Then
          bAddHelpItem = CBool(Request.Item("add").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("edit")) Then
        If Not String.IsNullOrEmpty(Request.Item("edit").ToString.Trim) Then
          bEditHelpItem = CBool(Request.Item("edit").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("delete")) Then
        If Not String.IsNullOrEmpty(Request.Item("delete").ToString.Trim) Then
          bDeleteHelpItem = CBool(Request.Item("delete").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("submit")) Then
        If Not String.IsNullOrEmpty(Request.Item("submit").ToString.Trim) Then
          bSubmitHelpItem = CBool(Request.Item("submit").ToString.Trim)
        End If
      End If

      If Not IsNothing(Request.Item("release")) Then
        If Not String.IsNullOrEmpty(Request.Item("release").ToString.Trim) Then
          bReleaseHelpItem = CBool(Request.Item("release").ToString.Trim)
        End If
      End If


      Dim sErrorString As String = ""

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      Master.Set_Active_Tab(6) '

      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution Help Center - Home")

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      localDatalayer.displayAdminHelpList(bAddHelpItem, sDisplayAdminHelpList)
      adminHelpListLbl.Text = sDisplayAdminHelpList.Trim

      adminDetailHelpListPnl.Visible = True
      adminDetailHelpItemPnl.Visible = False

      submitItemBtn.Visible = False
      deleteItemBtn.Visible = False

      deleteItemBtn.PostBackUrl = "~/adminHelp.aspx?helpId=" + nHelpItemID.ToString + "&delete=true"

      Dim helpCriteria As New helpAdminSelectionCriteriaClass

      If Not String.IsNullOrEmpty(sHelpArea.Trim) Or Not String.IsNullOrEmpty(sDisplayOrder.Trim) Then

        localDatalayer.displayAdminDetailHelpList(sHelpArea, sDisplayOrder, sSortOrder, sDisplayAdminDetailHelpList)
        adminDetailHelpListLbl.Text = sDisplayAdminDetailHelpList.Trim

        adminDetailHelpListPnl.Visible = True

      ElseIf bEditHelpItem Or bAddHelpItem Then

        adminDetailHelpListLbl.Text = ""

        helpItemReleaseDescription.config.toolbar = New Object() {New Object() {"Source"},
                                                                  New Object() {"Cut", "Copy", "Paste", "PasteText", "PasteFromWord", "-", "Undo", "Redo"},
                                                                  New Object() {"Bold", "Italic", "Underline", "Strike", "-", "Subscript", "Superscript"},
                                                                  New Object() {"NumberedList", "BulletedList", "-", "Outdent", "Indent"}, "/",
                                                                  New Object() {"Styles", "Format", "Font", "FontSize", "TextColor", "BGColor", "-", "About"}}
        helpItemReleaseDescription.config.extraPlugins = "pastefromword"

        helpItemTypeDDl.Attributes.Add("onclick", "ListEnableItem();")
        add_ListEnableItem_Script(helpItemTypeDDl)

        If bAddHelpItem Then

          deleteItemBtn.Visible = False
          submitItemBtn.Visible = True

          submitItemBtn.PostBackUrl = "~/adminHelp.aspx?helpId=-1&submit=true"

          helpDetailsLabel.Text = "ADD NEW HELP ITEM"

          If bReleaseHelpItem Then

            localDatalayer.fill_help_release_type_dropdown("R", 0, helpItemTypeDDl, bReleaseHelpItem)

            rel_label.Visible = False
            helpItemReleaseDescription.Visible = False
            video_panel.Visible = False

            helpItemReleaseDescription_R.Visible = True
            help_panel.Visible = True
            image_url_label.Visible = True

          Else
            localDatalayer.fill_help_release_type_dropdown("", 0, helpItemTypeDDl)
          End If


          localDatalayer.fill_help_release_topic_checkbox_dropdown(0, 0, helpItemTopicCBL)
          commonEvo.fillMakeModelDropDown(MakeModelDDL, Nothing, 0, "", "", False, False, False, True, False, False) ' fill dropdownlist with models

          helpItemStatusChk.Checked = True
          helpItemReleaseDate.Text = Now().ToLocalTime

        Else

          helpDetailsLabel.Text = "HELP ITEM DETAILS"

          deleteItemBtn.Visible = True
          submitItemBtn.Visible = True

          submitItemBtn.PostBackUrl = "~/adminHelp.aspx?helpId=" + nHelpItemID.ToString + "&submit=true"

          localDatalayer.getAdminDetailHelpItem(nHelpItemID, helpCriteria)

          helpItemStatusChk.Checked = helpCriteria.HelpCriteriaItemStatus

          If helpCriteria.HelpCriteriaSubID > -1 Then
            helpItemSubID.Text = helpCriteria.HelpCriteriaSubID.ToString
          End If

          If helpCriteria.HelpCriteriaCompanyID > -1 Then
            helpItemCompanyID.Text = helpCriteria.HelpCriteriaCompanyID.ToString
          End If

          helpItemAdminOnly.Checked = helpCriteria.HelpCriteriaAdminOnly

          If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemReleaseDate.Trim) Then
            helpItemReleaseDate.Text = FormatDateTime(helpCriteria.HelpCriteriaItemReleaseDate.Trim, DateFormat.GeneralDate)
          End If

          If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTitle.Trim) Then
            helpItemReleaseTitle.Text = helpCriteria.HelpCriteriaItemTitle.Trim
          End If

          ' fill in release type dropdown
          localDatalayer.fill_help_release_type_dropdown(helpCriteria.HelpCriteriaItemReleaseType.Trim, 0, helpItemTypeDDl, bReleaseHelpItem)

          If helpCriteria.HelpCriteriaModelID > -1 Then
            bHasHelpModelItem = True
            commonEvo.fillMakeModelDropDown(MakeModelDDL, Nothing, 0, "", helpCriteria.HelpCriteriaModelID.ToString, False, False, False, True, False, False) ' fill dropdownlist with models
          End If

          If bReleaseHelpItem Then

            helpItemReleaseDescription.Visible = False
            video_panel.Visible = False
            rel_label.Visible = False

            helpItemReleaseDescription_R.Visible = True
            help_panel.Visible = True
            image_url_label.Visible = True

            If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim) Then

              Dim tDesc As String = helpCriteria.HelpCriteriaItemDiscription.Trim
              Dim firstItem As Integer = 0
              Dim firstQuote As Integer = 0
              Dim secondQuote As Integer = 0
              Dim tUrl As String = ""
              Dim tHeight As String = ""
              Dim tWidth As String = ""

              ' check if discription has "image source"
              Try

                If tDesc.ToUpper.Contains("IMG SRC=") Then

                  ' pull out "URL" from description
                  firstItem = tDesc.ToUpper.IndexOf("<IMG SRC=")

                  If firstItem > 0 Then
                    firstQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstItem + 1)
                    secondQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstQuote + 1)

                    tUrl = tDesc.Substring(firstQuote + 1, secondQuote - (firstQuote + 1))
                  End If

                  firstItem = 0
                  firstQuote = 0
                  secondQuote = 0

                  ' pull out "height" from description
                  firstItem = tDesc.ToUpper.IndexOf("HEIGHT=")

                  If firstItem > 0 Then
                    firstQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstItem + 1)
                    secondQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstQuote + 1)

                    tHeight = tDesc.Substring(firstQuote + 1, secondQuote - (firstQuote + 1))
                  End If

                  firstItem = 0
                  firstQuote = 0
                  secondQuote = 0

                  ' pull out "width" from description
                  firstItem = tDesc.ToUpper.IndexOf("WIDTH=")

                  If firstItem > 0 Then
                    firstQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstItem + 1)
                    secondQuote = tDesc.ToUpper.IndexOf(Constants.QUOTE, firstQuote + 1)

                    tWidth = tDesc.Substring(firstQuote + 1, secondQuote - (firstQuote + 1))
                  End If

                  helpItemReleaseDescription_R.Text = tUrl
                  pop_height.Text = tHeight
                  pop_width.Text = tWidth

                Else ' this is a release item with just a description not an image link

                  help_panel.Visible = False
                  image_url_label.Visible = False
                  helpItemReleaseDescription_R.Visible = False

                  helpItemReleaseDescription.Visible = True
                  video_panel.Visible = True
                  rel_label.Visible = True

                  If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim) Then
                    helpItemReleaseDescription.Text = helpCriteria.HelpCriteriaItemDiscription.Trim
                  End If

                End If

              Catch ex As Exception

                help_panel.Visible = False
                image_url_label.Visible = False
                helpItemReleaseDescription_R.Visible = False

                helpItemReleaseDescription.Visible = True
                video_panel.Visible = True
                rel_label.Visible = True

                If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim) Then
                  helpItemReleaseDescription.Text = helpCriteria.HelpCriteriaItemDiscription.Trim
                End If

              End Try

            End If

          Else

            help_panel.Visible = False
            image_url_label.Visible = False
            helpItemReleaseDescription_R.Visible = False

            helpItemReleaseDescription.Visible = True
            video_panel.Visible = True
            rel_label.Visible = True

            If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim) Then
              helpItemReleaseDescription.Text = helpCriteria.HelpCriteriaItemDiscription.Trim
            End If

          End If


          If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemViewNumber.Trim) Then
            helpItemViewNumber.Text = helpCriteria.HelpCriteriaItemViewNumber.Trim
            bHasHelpHintItem = True
          End If

            If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTabName.Trim) Then
            helpItemTabName.Text = helpCriteria.HelpCriteriaItemTabName.Trim
            bHasHelpHintItem = True
          End If

          If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemVideoLink.Trim) Then
            helpItemVideoLink.Text = helpCriteria.HelpCriteriaItemVideoLink.Trim
          End If

          ' jetnet product "check boxes"                  
          helpItemBusChk.Checked = helpCriteria.HelpCriteriaBusinessFlag
          helpItemHeliChk.Checked = helpCriteria.HelpCriteriaHelicopterFlag
          helpItemComChk.Checked = helpCriteria.HelpCriteriaCommercialFlag
          ' helpItemYchtChk.Checked = helpCriteria.HelpCriteriayYachtFlag

          helpItemNewEvoOnlyChk.Checked = helpCriteria.HelpCriteriaNewEvoOnlyFlag
          helpItemNewEvoChk.Checked = helpCriteria.HelpCriteriaNewEvoFlag
          helpItemOldEvoChk.Checked = helpCriteria.HelpCriteriaOldEvoFlag
          helpItemCRMChk.Checked = helpCriteria.HelpCriteriaCRMFlag

          If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDocumentLink.Trim) Then
            helpItemDocumentLink.Text = helpCriteria.HelpCriteriaItemDocumentLink.Trim
          End If

          ' check the "topic" check boxes
          localDatalayer.fill_help_release_topic_checkbox_dropdown(nHelpItemID, 0, helpItemTopicCBL)

          helpItemDocumentFileLink.Visible = False

        End If

        adminDetailHelpItemPnl.Visible = True

      ElseIf bSubmitHelpItem Or bDeleteHelpItem Then

        helpCriteria.HelpCriteriaItemID = nHelpItemID

        If bSubmitHelpItem Then

          helpCriteria.HelpCriteriaItemStatus = helpItemStatusChk.Checked

          If helpCriteria.HelpCriteriaSubID > -1 Then
            helpItemSubID.Text = helpCriteria.HelpCriteriaSubID.ToString
          End If

          If Not String.IsNullOrEmpty(helpItemSubID.Text.Trim) Then
            If IsNumeric(helpItemSubID.Text) Then
              helpCriteria.HelpCriteriaSubID = CLng(helpItemSubID.Text)
            End If
          End If

          If Not String.IsNullOrEmpty(helpItemCompanyID.Text.Trim) Then
            If IsNumeric(helpItemCompanyID.Text) Then
              helpCriteria.HelpCriteriaCompanyID = CLng(helpItemCompanyID.Text)
            End If
          End If

          helpCriteria.HelpCriteriaAdminOnly = helpItemAdminOnly.Checked

          helpCriteria.HelpCriteriaItemReleaseDate = helpItemReleaseDate.Text
          helpCriteria.HelpCriteriaItemTitle = helpItemReleaseTitle.Text.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote)

          helpCriteria.HelpCriteriaItemReleaseType = helpItemTypeDDl.SelectedValue

          If MakeModelDDL.Enabled Then
            If Not String.IsNullOrEmpty(MakeModelDDL.SelectedValue.Trim) Then
              helpCriteria.HelpCriteriaModelID = CLng(MakeModelDDL.SelectedValue)
            End If
          End If

          If Not bReleaseHelpItem And Not String.IsNullOrEmpty(helpItemReleaseDescription.Text.Trim) Then
            helpCriteria.HelpCriteriaItemDiscription = helpItemReleaseDescription.Text
          Else
            helpCriteria.HelpCriteriaItemDiscription = "<a href=""" + helpItemDocumentLink.Text.Trim + """ target=""_blank"">"
            helpCriteria.HelpCriteriaItemDiscription += "<img src=""" + helpItemReleaseDescription_R.Text.Trim + """ width=""" + pop_width.Text + """ height=""" + pop_height.Text + """ border=""0""></a>"
          End If

          helpCriteria.HelpCriteriaItemViewNumber = helpItemViewNumber.Text.Trim

          helpCriteria.HelpCriteriaItemTabName = helpItemTabName.Text.Trim


          helpCriteria.HelpCriteriaItemVideoLink = helpItemVideoLink.Text.Trim

          helpCriteria.HelpCriteriaBusinessFlag = helpItemBusChk.Checked
          helpCriteria.HelpCriteriaHelicopterFlag = helpItemHeliChk.Checked
          helpCriteria.HelpCriteriaCommercialFlag = helpItemComChk.Checked

          'helpCriteria.HelpCriteriaYachtFlag = helpItemYchtChk.Checked

          helpCriteria.HelpCriteriaNewEvoOnlyFlag = helpItemNewEvoOnlyChk.Checked
          helpCriteria.HelpCriteriaNewEvoFlag = helpItemNewEvoChk.Checked
          helpCriteria.HelpCriteriaOldEvoFlag = helpItemOldEvoChk.Checked
          helpCriteria.HelpCriteriaCRMFlag = helpItemCRMChk.Checked

          For Each l As ListItem In helpItemTopicCBL.Items

            If l.Selected Then
              If String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTopicList.Trim) Then
                helpCriteria.HelpCriteriaItemTopicList = l.Value
              Else
                helpCriteria.HelpCriteriaItemTopicList += Constants.cCommaDelim + l.Value
              End If
            End If

          Next

          If Not String.IsNullOrEmpty(helpItemDocumentLink.Text.Trim) Then
            helpCriteria.HelpCriteriaItemDocumentLink = helpItemDocumentLink.Text
          End If

          If helpCriteria.HelpCriteriaModelID = -1 And helpCriteria.HelpCriteriaItemReleaseType.ToUpper.Contains("ML") Then

            adminDetailHelpListLbl.Text = "There was an error. Please select a model ID for this Help Item. Item was NOT " + IIf(nHelpItemID > -1, "UPDATED", "INSERTED")

          Else

            If localDatalayer.insertOrUpdateHelpItem(IIf(nHelpItemID > -1, True, False), helpCriteria, helpItemDocumentFileLink) Then
              adminDetailHelpListLbl.Text = "Item was " + IIf(nHelpItemID > -1, "UPDATED", "INSERTED")
            Else
              adminDetailHelpListLbl.Text = "There was an error. Item was NOT " + IIf(nHelpItemID > -1, "UPDATED", "INSERTED")
            End If

          End If


        Else

          If localDatalayer.deleteHelpItem(nHelpItemID) Then
            adminDetailHelpListLbl.Text = "Item was Deleted"
          Else
            adminDetailHelpListLbl.Text = "Item was NOT Deleted"
          End If

        End If

        adminDetailHelpListPnl.Visible = True

      End If

    End If

  End Sub

  Public Sub add_ListEnableItem_Script(ByVal lbSource As DropDownList)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("lei-lb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf + "  function ListEnableItem() {")
      sScptStr.Append(vbCrLf + "    var list = document.getElementById(""" + lbSource.ClientID.ToString + """);")
      sScptStr.Append(vbCrLf + "    for (var i = 0; i < list.length; i++) {")
      sScptStr.Append(vbCrLf + "      if (list[i].selected) {")
      sScptStr.Append(vbCrLf + "        if (list[i].value == ""ML"") {")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblModelListID).show();")
      sScptStr.Append(vbCrLf + "          $(""#"" + dllModelListID).show();")
      sScptStr.Append(vbCrLf + "        } else {")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblModelListID).hide();")
      sScptStr.Append(vbCrLf + "          $(""#"" + dllModelListID).hide();")
      sScptStr.Append(vbCrLf + "        }")
      sScptStr.Append(vbCrLf + "        if (list[i].value == ""EH"") {")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblViewID).show();")
      sScptStr.Append(vbCrLf + "          $(""#"" + tbxViewID).show();")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblTabID).show();")
      sScptStr.Append(vbCrLf + "          $(""#"" + tbxTabID).show();")
      sScptStr.Append(vbCrLf + "          return;")
      sScptStr.Append(vbCrLf + "        } else {")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblViewID).hide();")
      sScptStr.Append(vbCrLf + "          $(""#"" + tbxViewID).hide();")
      sScptStr.Append(vbCrLf + "          $(""#"" + lblTabID).hide();")
      sScptStr.Append(vbCrLf + "          $(""#"" + tbxTabID).hide();")
      sScptStr.Append(vbCrLf + "          return;")
      sScptStr.Append(vbCrLf + "        }")
      sScptStr.Append(vbCrLf + "      }")
      sScptStr.Append(vbCrLf + "    }")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "lei-lb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

End Class