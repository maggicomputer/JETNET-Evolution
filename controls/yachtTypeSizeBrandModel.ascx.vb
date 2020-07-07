' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/yachtTypeSizeBrandModel.ascx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:48a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: yachtTypeSizeBrandModel.ascx.vb $
'
' ********************************************************************************

Partial Public Class yachtTypeSizeBrandModel
  Inherits System.Web.UI.UserControl

  Public controlYachtType As String = ""
  Public controlYachtSize As String = ""
  Public controlYachtBrand As String = ""
  Public controlYachtModel As String = ""

  Public controlYachtTypeName As String = ""
  Public controlYachtSizeName As String = ""
  Public controlYachtBrandName As String = ""
  Public controlYachtModelName As String = ""

  Public yachtBrandModelString As String = ""
  Public yachtMotorCategoryString As String = ""

  Public sHTMLSelectText As String = ""
  Public sHTMLSelectSize As String = ""

  Private bOverideMultiSelect As Boolean = False ' enables overide of single select with multi-select for list boxes(view only)
  Private bIsView As Boolean = False ' determines if its multi-select single select

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try

      ' if our controls are not filled in then use these default values
      If Not IsPostBack Then
        If String.IsNullOrEmpty(controlYachtTypeName) And String.IsNullOrEmpty(controlYachtSizeName) And _
           String.IsNullOrEmpty(controlYachtBrandName) And String.IsNullOrEmpty(controlYachtModelName) Then
          setControlName("Yacht")
          setListSize(4)
          bIsView = False
        End If
      End If

      If bIsView Then

        controlYachtType = Session.Item("viewYachtType").ToString
        controlYachtSize = Session.Item("viewYachtSize").ToString
        controlYachtBrand = Session.Item("viewYachtBrand").ToString
        controlYachtModel = Session.Item("viewYachtModel").ToString

        If bOverideMultiSelect Then
          sHTMLSelectText = "multiple=""multiple"" "
        Else
          sHTMLSelectText = ""
        End If

      Else

        controlYachtType = Session.Item("tabYachtType").ToString
        controlYachtSize = Session.Item("tabYachtSize").ToString
        controlYachtBrand = Session.Item("tabYachtBrand").ToString
        controlYachtModel = Session.Item("tabYachtModel").ToString
        sHTMLSelectText = "multiple=""multiple"" "

      End If

      commonEvo.fillYachtArray(yachtBrandModelString)
      commonEvo.fillYachtCategoryLableArray(yachtMotorCategoryString)

    Catch ex As Exception

    End Try
  End Sub

  Public Sub setListSize(ByVal nSize As Integer)
    sHTMLSelectSize = nSize.ToString
  End Sub

  Public Sub setControlName(ByVal sControlBaseName As String)
    controlYachtTypeName = "cbo" + sControlBaseName + "Type"
    controlYachtSizeName = "cbo" + sControlBaseName + "Size"
    controlYachtBrandName = "cbo" + sControlBaseName + "Brand"
    controlYachtModelName = "cbo" + sControlBaseName + "Model"
  End Sub
  Public Sub setIsView(ByVal inSetIsView As Boolean)
    bIsView = inSetIsView
  End Sub

  Public Sub setOverideMultiSelect(ByVal inOverideMultiSelect As Boolean)
    bOverideMultiSelect = inOverideMultiSelect
  End Sub

  Public Sub add_MultiListEnsureItemVisible_Script(ByVal lbSource As ListBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("tsbm-lb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function MultiListEnsureItemVisible() {")
      sScptStr.Append(vbCrLf & "    var list = document.getElementById(""" + lbSource.ClientID.ToString + """);")
      sScptStr.Append(vbCrLf & "    if ((typeof(list.name) != ""undefined"") && (list != null)) {")
      sScptStr.Append(vbCrLf & "      var wasDisabled = false;")
      sScptStr.Append(vbCrLf & "      if (list.disabled) { // cant set selected items on disabled list")
      sScptStr.Append(vbCrLf & "        wasDisabled = true;")
      sScptStr.Append(vbCrLf & "        list.disabled = false;")
      sScptStr.Append(vbCrLf & "      }")
      sScptStr.Append(vbCrLf & "      if (!list || !list.multiple || list.length == 0) return;")
      sScptStr.Append(vbCrLf & "      var lastItem = list[list.length - 1];")
      sScptStr.Append(vbCrLf & "      if (lastItem.selected) {")
      sScptStr.Append(vbCrLf & "        lastItem.selected = true;")
      sScptStr.Append(vbCrLf & "        return;")
      sScptStr.Append(vbCrLf & "      }")
      sScptStr.Append(vbCrLf & "      else {")
      sScptStr.Append(vbCrLf & "        lastItem.selected = true;")
      sScptStr.Append(vbCrLf & "        lastItem.selected = false;")
      sScptStr.Append(vbCrLf & "      }")
      sScptStr.Append(vbCrLf & "      for (var i = 0; i < list.length; i++) {")
      sScptStr.Append(vbCrLf & "       if (list[i].selected) {")
      sScptStr.Append(vbCrLf & "         list[i].selected = true;")
      sScptStr.Append(vbCrLf & "           if (wasDisabled) {")
      sScptStr.Append(vbCrLf & "             list.disabled = true;")
      sScptStr.Append(vbCrLf & "           }")
      sScptStr.Append(vbCrLf & "         return;")
      sScptStr.Append(vbCrLf & "        }")
      sScptStr.Append(vbCrLf & "      }")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tsbm-lb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub
End Class