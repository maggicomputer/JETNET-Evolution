' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/eventCategoryType.ascx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:46a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: eventCategoryType.ascx.vb $
'
' ********************************************************************************

Partial Public Class eventCategoryType
  Inherits System.Web.UI.UserControl

  Public eventCatString As String = ""

  Public eventCatTypeCboName As String = ""
  Public eventCatTypeCodeCboName As String = ""

  Public eventCatTypeValue As String = ""
  Public eventCatTypeCodeValue As String = ""

  Public sHTMLSelectSize As String = ""

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Me.Visible Then
      commonEvo.fillEventCategoryArray(eventCatString)
    End If

    If Not IsPostBack Then
      If String.IsNullOrEmpty(eventCatTypeCboName) And String.IsNullOrEmpty(eventCatTypeCodeCboName) Then
        setControlName("Event")
        setListSize(4)
      End If
    End If

    add_MultiListEnsureItemVisible_Script(eventCatTypeCodeCboName)

  End Sub

  Public Sub setListSize(ByVal nSize As Integer)
    sHTMLSelectSize = nSize.ToString
  End Sub

  Public Sub setControlName(ByVal sControlBaseName As String)
    eventCatTypeCboName = "cbo" + sControlBaseName + "Categories"
    eventCatTypeCodeCboName = "cbo" + sControlBaseName + "TypeCodes"
  End Sub

  Public Sub add_MultiListEnsureItemVisible_Script(ByVal sControlName As String)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("eiv-lb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function MultiListEnsureItemVisible() {")
      sScptStr.Append(vbCrLf & "    var list = document.getElementById(""" + sControlName.Trim + "ID"");")
      sScptStr.Append(vbCrLf & "    var wasDisabled = false;")
      sScptStr.Append(vbCrLf & "    if (list.disabled) { // cant set selected items on disabled list")
      sScptStr.Append(vbCrLf & "      wasDisabled = true;")
      sScptStr.Append(vbCrLf & "      list.disabled = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    if (!list || !list.multiple || list.length == 0) return;")
      sScptStr.Append(vbCrLf & "    var lastItem = list[list.length - 1];")
      sScptStr.Append(vbCrLf & "    if (lastItem.selected) {")
      sScptStr.Append(vbCrLf & "      lastItem.selected = true;")
      sScptStr.Append(vbCrLf & "      return;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    else {")
      sScptStr.Append(vbCrLf & "      lastItem.selected = true;")
      sScptStr.Append(vbCrLf & "      lastItem.selected = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    for (var i = 0; i < list.length; i++) {")
      sScptStr.Append(vbCrLf & "     if (list[i].selected) {")
      sScptStr.Append(vbCrLf & "       list[i].selected = true;")
      sScptStr.Append(vbCrLf & "         if (wasDisabled) {")
      sScptStr.Append(vbCrLf & "           list.disabled = true;")
      sScptStr.Append(vbCrLf & "         }")
      sScptStr.Append(vbCrLf & "       return;")
      sScptStr.Append(vbCrLf & "      }")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "eiv-lb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

End Class