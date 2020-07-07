' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminBackground.aspx.vb $
'$$Author: Mike $
'$$Date: 7/11/19 2:23p $
'$$Modtime: 7/11/19 1:20p $
'$$Revision: 3 $
'$$Workfile: adminBackground.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminBackground
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Private sBackgroundTask As String = ""
  Public nBackgroundID As Integer = 0
  Private sBackgroundActive As String = ""
  Private sBackgroundProduct As String = ""
  Public bShowBackgroundDetails As Boolean = False

  Public Shared masterPage As New Object

  Private Sub adminBackground_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
      masterPage = DirectCast(Page.Master, CustomerAdminTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
      Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, HomebaseTheme)
    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim bAddNewBackground As Boolean = False
    Dim bShowActive As Boolean = False
    Dim sUploadFileLink As String = ""

    Dim sDisplayBackgroundList As String = ""

    Dim sErrorString As String = ""
    Dim FileError As String = ""

    Dim oBackgroundObject As New adminBackgroundCriteriaClass

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        masterPage.Set_Active_Tab(9)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution Background Management - Home")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        masterPage.Set_Active_Tab(8)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase Background Management - Home")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sBackgroundTask = Request.Item("task").ToString.ToUpper.Trim

          If sBackgroundTask.ToLower.Contains("add") Then
            bAddNewBackground = True
          End If

          If sBackgroundTask.ToLower.Contains("details") Then
            bShowBackgroundDetails = True
          End If

        End If
      End If

      If Not IsNothing(Request.Item("product")) Then
        If Not String.IsNullOrEmpty(Request.Item("product").ToString.Trim) Then
          sBackgroundProduct = Request.Item("product").ToString.ToUpper.Trim
          If Not IsPostBack Then
            backgroundByProduct.SelectedValue = sBackgroundProduct
          End If
        End If
      End If

      If Not IsNothing(Request.Item("active")) Then
        If Not String.IsNullOrEmpty(Request.Item("active").ToString.Trim) Then
          sBackgroundActive = Request.Item("active").ToString.ToUpper.Trim
          If Not IsPostBack Then
            backgroundByStatus.SelectedValue = sBackgroundActive
          End If
        End If
      End If

      If Not IsNothing(Request.Item("backID")) Then
        If Not String.IsNullOrEmpty(Request.Item("backID").ToString.Trim) Then
          If IsNumeric(Request.Item("backID").ToString.Trim) Then
            nBackgroundID = CInt(Request.Item("backID").ToString.Trim)
          End If
        End If
      End If

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      If nBackgroundID = 0 Then

        TableCell_add_background_table.Visible = False

        localDatalayer.displayBackgroundList(IIf(Not String.IsNullOrEmpty(backgroundByProduct.SelectedValue.Trim), backgroundByProduct.SelectedValue.Trim, ""), IIf(Not String.IsNullOrEmpty(backgroundByStatus.SelectedValue.Trim), True, False), sDisplayBackgroundList)
        backgroundDisplayLbl.Text = sDisplayBackgroundList.Trim

      ElseIf IsPostBack And sBackgroundTask.ToLower.Contains("submit") Then

        Dim backgroundImageLink As String = ""
        Dim backgroundImageFile As String = ""
        Dim TheFile As System.IO.FileInfo

        Dim objfilestream As IO.Stream = Nothing

        Dim imgOrigional As Drawing.Image = Nothing
        Dim imgResize As Drawing.Image = Nothing

        oBackgroundObject.BkndCriteriaItemID = nBackgroundID

        oBackgroundObject.BkndCriteriaItemTitle = background_title.Text.Trim

        oBackgroundObject.BkndCriteriaItemStatus = background_statusChk.Checked
        oBackgroundObject.BkndCriteriaBusinessFlag = background_busChk.Checked
        oBackgroundObject.BkndCriteriaCommercialFlag = background_commChk.Checked
        oBackgroundObject.BkndCriteriaHelicopterFlag = background_heliChk.Checked
        oBackgroundObject.BkndCriteriaAerodexFlag = background_aeroChk.Checked
        oBackgroundObject.BkndCriteriaYachtFlag = background_yachtsChk.Checked
        oBackgroundObject.BkndCriteriaFeatureFlag = background_featurChk.Checked
        oBackgroundObject.BkndCriteriaItemNew = True

        ' grab the file from the upload loaction and place in "../images/background/" + nBackgroundID.ToString + ".jpg"

        If (background_fileLink.HasFile) Then

          Try

            objfilestream = background_fileLink.PostedFile.InputStream
            imgOrigional = Drawing.Image.FromStream(objfilestream)

            'jpg check
            If imgOrigional.RawFormat.Equals(Drawing.Imaging.ImageFormat.Jpeg) Then

              'Check that image does not exceed maximum dimension settings
              If imgOrigional.Width > 1600 Then
                imgResize = resizeBackgroundImage(imgOrigional, New Drawing.Size(1600, 900))
              End If

              ' delete any "previous" background image before saving new image
              backgroundImageLink = "images/background/" + nBackgroundID.ToString + ".jpg"
              backgroundImageFile = HttpContext.Current.Server.MapPath(backgroundImageLink)

              oBackgroundObject.BkndCriteriaItemLink = "../" + backgroundImageLink

              TheFile = New System.IO.FileInfo(backgroundImageFile)

              If TheFile.Exists Then 'is the file actually there?
                System.IO.File.Delete(backgroundImageFile) 'remove the file.
              End If

              If Not localDatalayer.insertOrUpdateBackground(0, "", "", False, oBackgroundObject) Then
                FileError = "There was a problem saving your background image information to the database."
              End If

              If String.IsNullOrEmpty(FileError.Trim) Then

                If Not IsNothing(imgResize) Then
                  imgResize.Save(backgroundImageFile, imgOrigional.RawFormat)
                ElseIf Not IsNothing(imgOrigional) Then
                  imgOrigional.Save(backgroundImageFile, Drawing.Imaging.ImageFormat.Jpeg)
                End If

                backgroundImage.ImageUrl = "../" + backgroundImageLink
                backgroundImage.AlternateText = backgroundImageLink
                backgroundImage.ToolTip = backgroundImageLink

                FileError = "* New Background Added *"

              End If

            Else
              FileError = "Your background image must be a .jpg"
            End If

            objfilestream.Close()

          Catch ex As Exception
            FileError = "File Error: " + ex.Message.ToString()
          End Try
        Else
          FileError = "You have not specified a background image file."
        End If

        addNewBackgroundLbl.Text = "<p align=""center"">" + FileError + "</p>"
        addNewBackgroundLbl.Visible = True

        Me.Form.Action = "adminbackground.aspx"

        updateBackgroundBtn.Visible = True
        insertBackgroundBtn.Visible = False

      ElseIf IsPostBack And sBackgroundTask.ToLower.Contains("edit") Then

        oBackgroundObject.BkndCriteriaItemID = nBackgroundID

        oBackgroundObject.BkndCriteriaItemTitle = background_title.Text.Trim
        oBackgroundObject.BkndCriteriaItemLink = background_link.Text.Trim

        oBackgroundObject.BkndCriteriaItemStatus = background_statusChk.Checked
        oBackgroundObject.BkndCriteriaBusinessFlag = background_busChk.Checked
        oBackgroundObject.BkndCriteriaCommercialFlag = background_commChk.Checked
        oBackgroundObject.BkndCriteriaHelicopterFlag = background_heliChk.Checked
        oBackgroundObject.BkndCriteriaAerodexFlag = background_aeroChk.Checked
        oBackgroundObject.BkndCriteriaYachtFlag = background_yachtsChk.Checked
        oBackgroundObject.BkndCriteriaFeatureFlag = background_featurChk.Checked

        oBackgroundObject.BkndCriteriaItemNew = False

        If Not localDatalayer.insertOrUpdateBackground(0, "", "", False, oBackgroundObject) Then
          FileError = "There was a problem saving your background image information to the database."
        Else
          FileError = "* Background Updated *"
        End If

        addNewBackgroundLbl.Text = "<p align=""center"">" + FileError + "</p>"
        addNewBackgroundLbl.Visible = True

        Me.Form.Action = "adminbackground.aspx"

        updateBackgroundBtn.Visible = True
        insertBackgroundBtn.Visible = False

      Else

        If sBackgroundTask.ToLower.Contains("update") And nBackgroundID > 0 Then

          Dim bBackgroundProductValue As Boolean = False

          If Not IsNothing(Request.Item("prodvalue")) Then
            If Not String.IsNullOrEmpty(Request.Item("prodvalue").ToString.Trim) Then
              bBackgroundProductValue = CBool(Request.Item("prodvalue").ToString.Trim)
            End If
          End If

          TableCell_add_background_table.Visible = False

          localDatalayer.insertOrUpdateBackground(nBackgroundID, sBackgroundActive, sBackgroundProduct, bBackgroundProductValue, Nothing)

          localDatalayer.displayBackgroundList(IIf(Not String.IsNullOrEmpty(backgroundByProduct.SelectedValue.Trim), backgroundByProduct.SelectedValue.Trim, ""), IIf(Not String.IsNullOrEmpty(backgroundByStatus.SelectedValue.Trim), True, False), sDisplayBackgroundList)
          backgroundDisplayLbl.Text = sDisplayBackgroundList.Trim

          Me.Form.Action = "adminbackground.aspx"

        End If

      End If

      If bAddNewBackground And nBackgroundID = 0 Then

        backgroundTableTitle.Text = "ADD NEW BACKGROUND"
        TableCell_add_background_table.Visible = True

        TableCell0.Visible = False
        TableCell01.Visible = False

        TableCell_background_list.Visible = False


        ' get the next background id
        nBackgroundID = localDatalayer.getMaxBackgroundID + 1

        background_link.Text = "../images/background/" + nBackgroundID.ToString + ".jpg"

        backgroundImage.ImageUrl = "../images/background/0.jpg"
        backgroundImage.AlternateText = "../images/background/0.jpg"
        backgroundImage.ToolTip = "../images/background/0.jpg"

        updateBackgroundBtn.Visible = False
        insertBackgroundBtn.PostBackUrl = "~/adminBackground.aspx?task=submit&backID=" + nBackgroundID.ToString

      End If

      If bShowBackgroundDetails And nBackgroundID > 0 Then

        backgroundTableTitle.Text = "UPDATE BACKGROUND"
        TableCell_add_background_table.Visible = True

        TableCell0.Visible = False
        TableCell01.Visible = False

        TableCell_background_list.Visible = False


        background_link.Text = "../images/background/" + nBackgroundID.ToString + ".jpg"

        backgroundImage.ImageUrl = "../images/background/" + nBackgroundID.ToString + ".jpg"
        backgroundImage.AlternateText = "../images/background/" + nBackgroundID.ToString + ".jpg"
        backgroundImage.ToolTip = "../images/background/" + nBackgroundID.ToString + ".jpg"

        background_fileLink.Visible = False

        insertBackgroundBtn.Visible = False
        updateBackgroundBtn.PostBackUrl = "~/adminBackground.aspx?task=edit&backID=" + nBackgroundID.ToString

        Dim results_table As New DataTable

        results_table = localDatalayer.getBackground(nBackgroundID)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              If Not IsDBNull(r.Item("evoback_title")) Then
                If Not String.IsNullOrEmpty(r.Item("evoback_title").ToString.Trim) Then
                  background_title.Text = HttpContext.Current.Server.HtmlEncode(r.Item("evoback_title").ToString.Trim)
                End If

              End If

              If Not IsDBNull(r.Item("evoback_active_flag")) Then
                If r.Item("evoback_active_flag").ToString.Trim.Contains("Y") Then
                  background_statusChk.Checked = True
                Else
                  background_statusChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_aerodex_flag")) Then
                If r.Item("evoback_aerodex_flag").ToString.Trim.Contains("Y") Then
                  background_aeroChk.Checked = True
                Else
                  background_aeroChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_product_business_flag")) Then
                If r.Item("evoback_product_business_flag").ToString.Trim.Contains("Y") Then
                  background_busChk.Checked = True
                Else
                  background_busChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_product_commercial_flag")) Then
                If r.Item("evoback_product_commercial_flag").ToString.Trim.Contains("Y") Then
                  background_commChk.Checked = True
                Else
                  background_commChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_product_helicopter_flag")) Then
                If r.Item("evoback_product_helicopter_flag").ToString.Trim.Contains("Y") Then
                  background_heliChk.Checked = True
                Else
                  background_heliChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_feature_flag")) Then
                If r.Item("evoback_feature_flag").ToString.Trim.Contains("Y") Then
                  background_featurChk.Checked = True
                Else
                  background_featurChk.Checked = False
                End If
              End If

              If Not IsDBNull(r.Item("evoback_product_yacht_flag")) Then
                If r.Item("evoback_product_yacht_flag").ToString.Trim.Contains("Y") Then
                  background_yachtsChk.Checked = True
                Else
                  background_yachtsChk.Checked = False
                End If
              End If

            Next

            results_table = Nothing

          End If

        End If

      End If

    End If

  End Sub

  Private Function resizeBackgroundImage(ByVal image As System.Drawing.Image, ByVal size As System.Drawing.Size, Optional ByVal preserveAspectRatio As Boolean = True) As System.Drawing.Image

    Dim newWidth As Integer
    Dim newHeight As Integer

    If preserveAspectRatio Then
      Dim originalWidth As Integer = image.Width
      Dim originalHeight As Integer = image.Height
      Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
      Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
      Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
      newWidth = CInt(originalWidth * percent)
      newHeight = CInt(originalHeight * percent)
    Else
      newWidth = size.Width
      newHeight = size.Height
    End If

    Try

      Dim newImage As System.Drawing.Image = New Drawing.Bitmap(newWidth, newHeight)

      Using graphicsHandle As Drawing.Graphics = Drawing.Graphics.FromImage(newImage)
        graphicsHandle.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
        graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
      End Using

      Return newImage

    Catch ex As Exception

      addNewBackgroundLbl.Text = "<p align=""center"">Resize File Error: " + ex.Message.ToString() + "</p>"
      addNewBackgroundLbl.Visible = True

      Return Nothing

    End Try

  End Function

End Class