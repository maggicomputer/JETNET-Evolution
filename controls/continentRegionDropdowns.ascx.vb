
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/continentRegionDropdowns.ascx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:46a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: continentRegionDropdowns.ascx.vb $
'
' ********************************************************************************

Partial Public Class continentRegionDropdownsCtrl

  Inherits System.Web.UI.UserControl

  Public regionString As String = ""
  Public countryString As String = ""
  Public timeZoneString As String = ""

  Public bIsView As Boolean = False ' determines if dropdowns are set for documents view
  Public bIsBase As Boolean = False ' determines if dropdowns are set for aircraft base search
  Public bShowInactiveCountries As Boolean = False ' determines if inactive countries are shown

  Public bFirstControl As Boolean = False ' runs the scripts for page arrays(only needed once per page)
  ' no matter how many controls are shown

  Public sHTMLSelectSize As String = ""

  Public sControlType As String = ""

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim localDataLayer As New viewsDataLayer

    If Me.Visible Then
      If bFirstControl Then
        commonEvo.fillContinentArray(countryString)
        commonEvo.fillRegionArray(regionString)
        commonEvo.fillTimeZoneArray(timeZoneString)
      Else
        commonEvo.fillContinentArray("")
        commonEvo.fillRegionArray("")
        commonEvo.fillTimeZoneArray("")
      End If
    End If
    ' set the values in the control to show based on type
    If Not bIsBase And bIsView Then
      sControlType = "view"
    ElseIf bIsBase And Not bIsView Then
      sControlType = "base"
    ElseIf Not bIsBase And Not bIsView Then
      sControlType = "company"
    End If

    displayContinentRegionDropdowns()

  End Sub

  Public Sub setIsView(ByVal inSetIsView As Boolean)
    bIsView = inSetIsView
  End Sub

  Public Sub setIsBase(ByVal inSetIsBase As Boolean)
    bIsBase = inSetIsBase
  End Sub

  Public Sub setFirstControl(ByVal inFirstControl As Boolean)
    'used for only adding hidden values once per page
    bFirstControl = inFirstControl
  End Sub

  Public Sub setShowInactiveCountries(ByVal inShowInactiveCountries As Boolean)
    'used for historical seraches
    bShowInactiveCountries = inShowInactiveCountries
  End Sub

  Public Sub setListSize(ByVal nSize As Integer)
    sHTMLSelectSize = nSize.ToString
  End Sub

  Private Sub displayContinentRegionDropdowns()

    Dim htmlOut As New StringBuilder

    htmlOut.Append("<table id=""contentRegionOuterTable"" cellpadding=""4"" cellspacing=""0"">")
    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

    If (bIsBase And Not bIsView) Then ' for base searches

      htmlOut.Append("<input type=""radio"" name='radBaseContinentRegion' id='radBaseContinentRegionID' value='Continent' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Continent&nbsp;")
      htmlOut.Append("<input type=""radio"" name='radBaseContinentRegion' id='radBaseContinentRegionID1' value='Region' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Region<br />")

      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboBaseRegion' id='cboBaseRegionID' onchange='javascript:refreshRegionsJS(""onChange"",""region"", bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:145px;""><option selected='selected' value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">Country:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboBaseCountry' id='cboBaseCountryID' onchange='javascript:refreshRegionsJS(""onChange"",""country"", bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:165px;""><option value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">State/Province:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboBaseState' id='cboBaseStateID' onchange='javascript:refreshRegionsJS(""onChange"",""state"", bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:155px;""><option value='All'>All</option></select>")

    ElseIf (Not bIsBase And Not bIsView) Then  ' for company searches

      htmlOut.Append("<input type=""radio"" name='radContinentRegion' id='radContinentRegionID' value='Continent' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Continent&nbsp;")
      htmlOut.Append("<input type=""radio"" name='radContinentRegion' id='radContinentRegionID1' value='Region' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Region<br />")

      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboCompanyRegion' id='cboCompanyRegionID' onchange='javascript:refreshRegionsJS(""onChange"",""region"", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:145px;""><option selected='selected' value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">Country:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboCompanyCountry' id='cboCompanyCountryID' onchange='javascript:refreshRegionsJS(""onChange"",""country"", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:165px;""><option value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">State/Province:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboCompanyState' id='cboCompanyStateID' onchange='javascript:refreshRegionsJS(""onChange"",""state"", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:155px;""><option value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left""><div class='TimeZone' id='cboCompanyTimeZoneLabelID' name='cboCompanyTimeZoneLabel'>Time&nbsp;Zone:</div>")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboCompanyTimeZone' id='cboCompanyTimeZoneID' onchange='javascript:refreshRegionsJS(""onChange"",""timeZone"", bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:95px;""><option value='All'>All</option></select>")

    Else  ' for view searches

      htmlOut.Append("<input type=""radio"" name='radViewContinentRegion' id='radViewContinentRegionID' value='Continent' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Continent&nbsp;")
      htmlOut.Append("<input type=""radio"" name='radViewContinentRegion' id='radViewContinentRegionID1' value='Region' onclick='javascript:refreshRegionsJS(""onClick"","""", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' />Region<br />")

      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboViewRegion' id='cboViewRegionID' onchange='javascript:refreshRegionsJS(""onChange"",""region"", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:145px;""><option selected='selected' value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">Country:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboViewCountry' id='cboViewCountryID' onchange='javascript:refreshRegionsJS(""onChange"",""country"", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:165px;""><option value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left"">State/Province:<br />")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboViewState' id='cboViewStateID' onchange='javascript:refreshRegionsJS(""onChange"",""state"", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:155px;""><option value='All'>All</option></select>")

      htmlOut.Append("</td><td valign=""bottom"" align=""left""><div class='TimeZone' id='cboViewTimeZoneLabelID' name='cboViewTimeZoneLabel'>Time&nbsp;Zone:</div>")
      htmlOut.Append("<select multiple='multiple' size='" + sHTMLSelectSize.Trim + "' name='cboViewTimeZone' id='cboViewTimeZoneID' onchange='javascript:refreshRegionsJS(""onChange"",""timeZone"", bIsBaseView, bIsViewView, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);' style=""width:95px;""><option value='All'>All</option></select>")

    End If

    htmlOut.Append("</td></tr>")
    htmlOut.Append("</table>" + vbCrLf)

    continentRegionDropdownsID.Text = htmlOut.ToString

    htmlOut = Nothing

  End Sub

End Class