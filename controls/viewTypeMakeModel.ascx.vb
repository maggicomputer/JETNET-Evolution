
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/viewTypeMakeModel.ascx.vb $
'$$Author: Mike $
'$$Date: 6/23/20 3:44p $
'$$Modtime: 6/23/20 2:24p $
'$$Revision: 7 $
'$$Workfile: viewTypeMakeModel.ascx.vb $
'
' ********************************************************************************

Partial Public Class viewTypeMakeModelCtrl

  Inherits System.Web.UI.UserControl

  Public productCodeCount As Integer = 0
  Public isHeliOnlyProduct As Boolean = False
  Public makeModelString As String = ""
  Public typeLableString As String = ""
  Public defaultMakeModelString As String = ""
  Public mfrNamesString As String = ""
  Public sizeString As String = ""

  Public controlAcType As String = ""
  Public controlAcMake As String = ""
  Public controlAcModel As String = ""
  Public controlAcMfrNames As String = ""
  Public controlAcSize As String = ""

  Public controlAcTypeName As String = ""
  Public controlAcMakeName As String = ""
  Public controlAcModelName As String = ""

  Private bIsView As Boolean = False ' determines if its multi-select single select
  Private bShowWeightClass As Boolean = False ' determines if Weight Class dropdown is shown
  Private bShowMfrNames As Boolean = False ' determines if Manufacturer Name dropdown is shown
  Private bShowSize As Boolean = False ' determines if size dropdown is shown
  Private bOverideDefaultModel As Boolean = False ' determines if "default model" check box is shown
  Private bOverideMultiSelect As Boolean = False ' enables overide of single select with multi-select for list boxes(view only)

  Private controlAcModelWeightClass As String = ""

  Public sHTMLSelectText As String = ""
  Public sHTMLSelectSize As String = ""
  Public script_version As String = ""

  Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    script_version = My.Settings.SCRIPT_VERSION.ToString

    Dim header As LiteralControl = New LiteralControl
    header.Text = "<script type=""text/javascript"" src=""/common/rebuildClientArray.js" + script_version + """></script>"
    Page.Header.Controls.Add(header)

    Dim header1 As LiteralControl = New LiteralControl
    header1.Text = "<script type=""text/javascript"" src=""/common/TypeMakeModelDropdown.js" + script_version + """></script>"
    Page.Header.Controls.Add(header1)

    Dim header2 As LiteralControl = New LiteralControl
    header2.Text = "<script type=""text/javascript"" src=""/common/refreshTMMDropdowns.js" + script_version + """></script>"
    Page.Header.Controls.Add(header2)

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim sErrorString As String = ""

    Try

      If Not IsNothing(Session.Item("localPreferences")) Then

        If IsArray(Session.Item("localPreferences").ProductCode) Then

          For nloop As Integer = 0 To UBound(Session.Item("localPreferences").ProductCode)

            Select Case Session.Item("localPreferences").ProductCode(nloop)
              Case eProductCodeTypes.H
                productCodeCount += 1
              Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
                productCodeCount += 1
              Case eProductCodeTypes.R
              Case eProductCodeTypes.C
                productCodeCount += 1
              Case eProductCodeTypes.P
              Case eProductCodeTypes.A
              Case eProductCodeTypes.Y

            End Select

          Next

        Else
          productCodeCount = 3
        End If

        isHeliOnlyProduct = HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct

      Else
        productCodeCount = 3
      End If

      ' if our controls are not filled in then use these default values
      If Not IsPostBack Then

        If String.IsNullOrEmpty(controlAcTypeName) And String.IsNullOrEmpty(controlAcMakeName) And String.IsNullOrEmpty(controlAcModelName) Then
          setControlName("Aircraft")
          setListSize(4)
          bIsView = False
        End If

      End If

      If bIsView Then
        controlAcType = Session.Item("viewAircraftType").ToString
        controlAcMake = Session.Item("viewAircraftMake").ToString
        controlAcModel = Session.Item("viewAircraftModel").ToString
        controlAcModelWeightClass = Session.Item("viewAircraftModelWeightClass")
        controlAcMfrNames = Session.Item("viewAircraftMfrNames")
        controlAcSize = Session.Item("viewAircraftSize")

        If bOverideMultiSelect Then
          sHTMLSelectText = "multiple=""multiple"" "
        Else
          sHTMLSelectText = ""
        End If

      Else

        controlAcType = Session.Item("tabAircraftType").ToString
        controlAcMake = Session.Item("tabAircraftMake").ToString
        controlAcModel = Session.Item("tabAircraftModel").ToString
        controlAcModelWeightClass = Session.Item("tabAircraftModelWeightClass")
        controlAcMfrNames = Session.Item("tabAircraftMfrNames")
        controlAcSize = Session.Item("tabAircraftSize")

        sHTMLSelectText = "multiple=""multiple"" "

      End If

      Dim sepArry(1) As Char
      sepArry(0) = Constants.cDymDataSeperator.Substring(0, 1)
      sepArry(1) = Constants.cDymDataSeperator.Substring(1, 1)

      ddlSizeCatDiv.Visible = bShowSize
      ddlMfrNameDiv.Visible = bShowMfrNames

      If bShowWeightClass Then
        tableCellWeightClass.Visible = True
        tableCellFilter.ColumnSpan = 2

        If bIsView Then
          controlAcModelWeightClass = Session.Item("viewAircraftModelWeightClass").ToString
        Else
          controlAcModelWeightClass = Session.Item("tabAircraftModelWeightClass").ToString
        End If

        If Not String.IsNullOrEmpty(controlAcModelWeightClass.Trim) Then

          If controlAcModelWeightClass.Contains(Constants.cCommaDelim) Then

            Dim tmpWeightClsArry As Array = controlAcModelWeightClass.Split(Constants.cCommaDelim)

            For x As Integer = 0 To UBound(tmpWeightClsArry)

              ddlWeightClass.SelectedValue = tmpWeightClsArry(x)
              ddlWeightClass.SelectedIndex = x

            Next

          ElseIf controlAcModelWeightClass.Contains(Constants.cDymDataSeperator) Then

            Dim tmpWeightClsArry As Array = controlAcModelWeightClass.Split(sepArry, StringSplitOptions.RemoveEmptyEntries)

            For x As Integer = 0 To UBound(tmpWeightClsArry)

              ddlWeightClass.SelectedValue = tmpWeightClsArry(x)
              ddlWeightClass.SelectedIndex = x

            Next

          Else
            ddlWeightClass.SelectedValue = IIf(controlAcModelWeightClass.ToUpper.Contains("ALL"), "", controlAcModelWeightClass)
          End If

        Else
          ddlWeightClass.SelectedValue = ""
        End If

      Else
        tableCellWeightClass.Visible = False
        tableCellFilter.ColumnSpan = 3
      End If

      If Not IsNothing(Request.Item("hasModelFilter")) Then
        If Not String.IsNullOrEmpty(Request.Item("hasModelFilter")) Then
          If Request.Item("hasModelFilter").ToString.ToLower = "true" Then
            HttpContext.Current.Session.Item("hasModelFilter") = True
          Else
            HttpContext.Current.Session.Item("hasModelFilter") = False
          End If
        End If
      End If

      If Not IsNothing(Request.Item("hasHelicopterFilter")) Then
        ' this should toggle the state of the Helicopter filter  
        If Request.Item("hasHelicopterFilter").ToString.ToLower = "true" Then
          HttpContext.Current.Session.Item("hasHelicopterFilter") = True
        ElseIf Request.Item("hasHelicopterFilter").ToString.ToLower = "false" Then
          HttpContext.Current.Session.Item("hasHelicopterFilter") = False
        End If
      End If

      If Not IsNothing(Request.Item("hasBusinessFilter")) Then
        ' this should toggle the state of the Business filter  
        If Request.Item("hasBusinessFilter").ToString.ToLower = "true" Then
          HttpContext.Current.Session.Item("hasBusinessFilter") = True
        ElseIf Request.Item("hasBusinessFilter").ToString.ToLower = "false" Then
          HttpContext.Current.Session.Item("hasBusinessFilter") = False
        End If
      End If

      If Not IsNothing(Request.Item("hasCommercialFilter")) Then
        ' this should toggle the state of the Commercial filter  
        If Request.Item("hasCommercialFilter").ToString.ToLower = "true" Then
          HttpContext.Current.Session.Item("hasCommercialFilter") = True
        ElseIf Request.Item("hasCommercialFilter").ToString.ToLower = "false" Then
          HttpContext.Current.Session.Item("hasCommercialFilter") = False
        End If
      End If

      If Not IsNothing(Request.Item("hasRegionalFilter")) Then
        ' this should toggle the state of the Regional filter  
        If Request.Item("hasRegionalFilter").ToString.ToLower = "true" Then
          HttpContext.Current.Session.Item("hasRegionalFilter") = True
        ElseIf Request.Item("hasRegionalFilter").ToString.ToLower = "false" Then
          HttpContext.Current.Session.Item("hasRegionalFilter") = False
        End If
      End If

      If Not IsNothing(Request.Item("lastModelFilter")) Then
        If Not String.IsNullOrEmpty(Request.Item("lastModelFilter")) Then
          HttpContext.Current.Session.Item("lastModelFilter") = Request.Item("lastModelFilter").ToString.ToUpper.Trim
        Else
          HttpContext.Current.Session.Item("lastModelFilter") = ""
        End If
      Else
        HttpContext.Current.Session.Item("lastModelFilter") = ""
      End If

      commonEvo.fillAirframeArray(makeModelString)
      commonEvo.fillAircraftTypeLableArray(typeLableString)

      If bShowMfrNames Then
        ' fill list of Manufactureres (based on product code)
        commonEvo.fillMfrNamesArray(mfrNamesString)
      End If

      If bShowSize Then
        ' fill list of sizes 
        commonEvo.fillAircraftSizeArray(sizeString)
      End If

      If Not bOverideDefaultModel Then
        If Not String.IsNullOrEmpty(Session.Item("localPreferences").UserDefaultModelList) Then
          commonEvo.fillDefaultAirframeArray(defaultMakeModelString)
        End If
        If CBool(Session.Item("UserDefaultFlag").ToString) Then
          tableRowDefaultModelsCheck.Visible = True
        Else
          tableRowDefaultModelsCheck.Visible = True
        End If
      Else
        Session.Item("UserDefaultFlag") = False
        tableRowDefaultModelsCheck.Visible = False
      End If

      displayProductCodeCheckBoxes()

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in viewTypeMakeModelCtrl (load)"
    End Try

  End Sub

  Public Sub setIsView(ByVal inSetIsView As Boolean)
    bIsView = inSetIsView
  End Sub

  Public Sub setShowWeightClass(ByVal inShowWeightClass As Boolean)
    bShowWeightClass = inShowWeightClass
  End Sub

  Public Sub setShowMfrNames(ByVal inShowMfrNames As Boolean)
    bShowMfrNames = inShowMfrNames
  End Sub

  Public Sub setShowAcSize(ByVal inShowAcSize As Boolean)
    bShowSize = inShowAcSize
  End Sub

  Public Sub setListSize(ByVal nSize As Integer)
    sHTMLSelectSize = nSize.ToString
  End Sub

  Public Sub setControlName(ByVal sControlBaseName As String)
    controlAcTypeName = "cbo" + sControlBaseName + "Type"
    controlAcMakeName = "cbo" + sControlBaseName + "Make"
    controlAcModelName = "cbo" + sControlBaseName + "Model"
  End Sub

  Public Sub setOverideDefaultModel(ByVal inOverideDefaultModel As Boolean)
    bOverideDefaultModel = inOverideDefaultModel
  End Sub

  Public Sub setOverideMultiSelect(ByVal inOverideMultiSelect As Boolean)
    bOverideMultiSelect = inOverideMultiSelect
  End Sub

  Public Function getWeightClass() As String
    Dim displayWeightClassString As String = ""

    For Each li In ddlWeightClass.Items
      If li.Selected Then

        If String.IsNullOrEmpty(displayWeightClassString.Trim) Then
          displayWeightClassString = li.Value.ToString.Trim
        Else
          displayWeightClassString += Constants.cCommaDelim + li.Value.ToString.Trim
        End If

      End If
    Next

    Return displayWeightClassString.Trim

  End Function

  Private Sub displayProductCodeCheckBoxes()

    Dim htmlOut As New StringBuilder

    htmlOut.Append("<table id=""typeMakeModelOuterTable"" cellpadding=""2"" cellspacing=""0"" style=""width: 100%;"">")
    htmlOut.Append("<tr><td style=""text-align: left; vertical-align: middle; width: 25%; padding: 2px;"" align=""left"" valign=""middle"" nowrap=""nowrap"">")

    ' if user has this product and and any other product code then show this product code check box
    If Session.Item("localPreferences").UserHelicopterFlag And (Session.Item("localPreferences").UserRegionalFlag Or Session.Item("localPreferences").UserBusinessFlag Or Session.Item("localPreferences").UserCommercialFlag) Then
      htmlOut.Append("<input type=""checkbox"" value=""true"" name=""chkHelicopterFilter"" id=""chkHelicopterFilterID"" onclick='JavaScript:refreshTypeMakeModelByCheckBox(""onClick"",""filter""," + isHeliOnlyProduct.ToString.ToLower + "," + productCodeCount.ToString + ");'")

      If Session.Item("hasModelFilter") Then
        Session.Item("chkHelicopterFilter") = Session.Item("hasHelicopterFilter")
      ElseIf Not Session.Item("chkHelicopterFilter") Then
        Session.Item("chkHelicopterFilter") = True
      End If

      If Session.Item("chkHelicopterFilter") Then
        htmlOut.Append(" checked=""checked"" />&nbsp;Helicopter")
      Else
        htmlOut.Append(" />&nbsp;Helicopter")
      End If

    Else
      htmlOut.Append("&nbsp;<input type=""checkbox"" value=""" + Session.Item("localPreferences").UserHelicopterFlag.ToString.ToLower + """ name=""chkHelicopterFilter"" id=""chkHelicopterFilterID"" " + IIf(Session.Item("localPreferences").UserHelicopterFlag, "checked=""checked""", "") + "/>")
      htmlOut.Append(vbCrLf + "<script type=""text/javascript"" language=""JavaScript"">")
      htmlOut.Append(vbCrLf + "  document.getElementById(""chkHelicopterFilterID"").style.visibility = ""hidden"";")
      htmlOut.Append(vbCrLf + "</script>" + vbCrLf)
    End If

    htmlOut.Append("</td><td style=""text-align: left; vertical-align: middle; width: 25%; padding: 2px;"" align=""left"" valign=""middle"" nowrap=""nowrap"">")


    If Session.Item("localPreferences").UserBusinessFlag And (Session.Item("localPreferences").UserRegionalFlag Or Session.Item("localPreferences").UserHelicopterFlag Or Session.Item("localPreferences").UserCommercialFlag) Then

      htmlOut.Append("<input type=""checkbox"" value=""true"" name=""chkBusinessFilter"" id=""chkBusinessFilterID"" onclick='JavaScript:refreshTypeMakeModelByCheckBox(""onClick"",""filter""," + isHeliOnlyProduct.ToString.ToLower + "," + productCodeCount.ToString + ");'")

      If Session.Item("hasModelFilter") Then
        Session.Item("chkBusinessFilter") = Session.Item("hasBusinessFilter")
      ElseIf Not Session.Item("chkBusinessFilter") Then
        Session.Item("chkBusinessFilter") = True
      End If

      If Session.Item("chkBusinessFilter") Then
        htmlOut.Append(" checked=""checked"" />&nbsp;Business")
      Else
        htmlOut.Append(" />&nbsp;Business")
      End If

    Else
      htmlOut.Append("&nbsp;<input type=""checkbox"" value=""" + Session.Item("localPreferences").UserBusinessFlag.ToString.ToLower + """ name=""chkBusinessFilter"" id=""chkBusinessFilterID"" " + IIf(Session.Item("localPreferences").UserBusinessFlag, "checked=""checked""", "") + "/>")
      htmlOut.Append(vbCrLf + "<script type=""text/javascript"" language=""JavaScript"">")
      htmlOut.Append(vbCrLf + "  document.getElementById(""chkBusinessFilterID"").style.visibility = ""hidden"";")
      htmlOut.Append(vbCrLf + "</script>" + vbCrLf)
    End If

    htmlOut.Append("</td><td style=""text-align: left; vertical-align: middle; width: 25%; padding: 2px;"" align=""left"" valign=""middle"" nowrap=""nowrap"">")


    If Session.Item("localPreferences").UserCommercialFlag And (Session.Item("localPreferences").UserBusinessFlag Or Session.Item("localPreferences").UserHelicopterFlag Or Session.Item("localPreferences").UserRegionalFlag) Then

      htmlOut.Append("<input type=""checkbox"" value=""true"" name=""chkCommercialFilter"" id=""chkCommercialFilterID"" onclick='JavaScript:refreshTypeMakeModelByCheckBox(""onClick"",""filter""," + isHeliOnlyProduct.ToString.ToLower + "," + productCodeCount.ToString + ");'")

      If Session.Item("hasModelFilter") Then
        Session.Item("chkCommercialFilter") = Session.Item("hasCommercialFilter")
      ElseIf Not Session.Item("chkCommercialFilter") Then
        Session.Item("chkCommercialFilter") = True
      End If

      If Session.Item("chkCommercialFilter") Then
        htmlOut.Append(" checked=""checked"" />&nbsp;Commercial")
      Else
        htmlOut.Append(" />&nbsp;Commercial")
      End If

    Else
      htmlOut.Append("&nbsp;<input type=""checkbox"" value=""" + Session.Item("localPreferences").UserCommercialFlag.ToString.ToLower + """ name=""chkCommercialFilter"" id=""chkCommercialFilterID"" " + IIf(Session.Item("localPreferences").UserCommercialFlag, "checked=""checked""", "") + "/>")
      htmlOut.Append(vbCrLf + "<script type=""text/javascript"" language=""JavaScript"">")
      htmlOut.Append(vbCrLf + "  document.getElementById(""chkCommercialFilterID"").style.visibility = ""hidden"";")
      htmlOut.Append(vbCrLf + "</script>" + vbCrLf)
    End If

    htmlOut.Append("</td><td style=""text-align: left; vertical-align: middle; width: 25%; padding: 2px;"" align=""left"" valign=""middle"" nowrap=""nowrap"">")

    If Session.Item("localPreferences").UserRegionalFlag And (Session.Item("localPreferences").UserBusinessFlag Or Session.Item("localPreferences").UserHelicopterFlag Or Session.Item("localPreferences").UserCommercialFlag) Then

      htmlOut.Append("<input type=""checkbox"" value=""true"" name=""chkRegionalFilter"" id=""chkRegionalFilterID"" onclick='JavaScript:refreshTypeMakeModelByCheckBox(""onClick"",""filter""," + isHeliOnlyProduct.ToString.ToLower + "," + productCodeCount.ToString + ");'")

      If Session.Item("hasModelFilter") Then
        Session.Item("chkRegionalFilter") = Session.Item("hasRegionalFilter")
      ElseIf Not Session.Item("chkRegionalFilter") Then
        Session.Item("chkRegionalFilter") = True
      End If

      If Session.Item("chkRegionalFilter") Then
        htmlOut.Append(" checked=""checked"" />&nbsp;Regional")
      Else
        htmlOut.Append(" />&nbsp;Regional")
      End If

    Else
      htmlOut.Append("&nbsp;<input type=""checkbox"" value=""" + Session.Item("localPreferences").UserRegionalFlag.ToString.ToLower + """ name=""chkRegionalFilter"" id=""chkRegionalFilterID"" " + IIf(Session.Item("localPreferences").UserRegionalFlag, "checked=""checked""", "") + "/>")
      htmlOut.Append(vbCrLf + "<script type=""text/javascript"" language=""JavaScript"">")
      htmlOut.Append(vbCrLf + "  document.getElementById(""chkRegionalFilterID"").style.visibility = ""hidden"";")
      htmlOut.Append(vbCrLf + "</script>" + vbCrLf)
    End If

    htmlOut.Append("</td></tr>")
    htmlOut.Append("</table>" + vbCrLf)

    productCodeFilter.Text = htmlOut.ToString()

    htmlOut = Nothing

  End Sub

End Class