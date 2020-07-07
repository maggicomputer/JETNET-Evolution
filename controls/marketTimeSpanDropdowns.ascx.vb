' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /CRM Evolution/controls/marketTimeSpanDropdowns.ascx.vb $
'$$Author: Mike $
'$$Date: 5/02/14 11:37a $
'$$Modtime: 4/30/14 1:08p $
'$$Revision: 1 $
'$$Workfile: marketTimeSpanDropdowns.ascx.vb $
'
' ********************************************************************************

Partial Public Class marketTimeSpanDropdownsCtrl
  Inherits System.Web.UI.UserControl

  Public timeScaleCboName As String = ""
  Public startDateCboName As String = ""
  Public displayRangeCboName As String = ""

  Public timeScaleValue As String = ""
  Public startDateValue As String = ""
  Public displayRangeValue As String = ""

  Private bIsView As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try

      ' if our controls are not filled in then use these default values
      If Not IsPostBack Then
        If String.IsNullOrEmpty(timeScaleCboName.Trim) And String.IsNullOrEmpty(startDateCboName.Trim) And String.IsNullOrEmpty(displayRangeCboName.Trim) Then
          setControlName("View")
          bIsView = True
        End If
      End If

      If bIsView Then

        If Not IsNothing(Session.Item("viewTimeScale")) Then
          If Not String.IsNullOrEmpty(Session.Item("viewTimeScale").ToString.Trim) Then
            timeScaleValue = Session.Item("viewTimeScale").ToString
          End If
        End If

        If Not IsNothing(Session.Item("viewStartDate")) Then
          If Not String.IsNullOrEmpty(Session.Item("viewStartDate").ToString.Trim) Then
            startDateValue = Session.Item("viewStartDate").ToString
          End If
        End If

        If Not IsNothing(Session.Item("viewScaleSets")) Then
          If Not String.IsNullOrEmpty(Session.Item("viewScaleSets").ToString.Trim) Then
            displayRangeValue = Session.Item("viewScaleSets").ToString
          End If
        End If

      Else

        If Not IsNothing(Session.Item("marketTimeScale")) Then
          If Not String.IsNullOrEmpty(Session.Item("marketTimeScale").ToString.Trim) Then
            timeScaleValue = Session.Item("marketTimeScale").ToString
          End If
        End If

        If Not IsNothing(Session.Item("marketStartDate")) Then
          If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) Then
            startDateValue = Session.Item("marketStartDate").ToString
          End If
        End If

        If Not IsNothing(Session.Item("marketScaleSets")) Then
          If Not String.IsNullOrEmpty(Session.Item("marketScaleSets").ToString.Trim) Then
            displayRangeValue = Session.Item("marketScaleSets").ToString
          End If
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) : marketTimeSpanDropdowns.ascx : " + ex.Message

    End Try

  End Sub

  Public Sub setIsView(ByVal inSetIsView As Boolean)
    bIsView = inSetIsView
  End Sub

  Public Sub setControlName(ByVal sControlBaseName As String)
    timeScaleCboName = "cbo" + sControlBaseName + "TimeScale"
    startDateCboName = "cbo" + sControlBaseName + "StartDate"
    displayRangeCboName = "cbo" + sControlBaseName + "RangeSpan"
  End Sub

  Public Sub setValues(ByVal sTimeScale As String, ByVal sStartDate As String, ByVal sRangeSpan As Integer)
    timeScaleValue = sTimeScale
    startDateValue = sStartDate
    displayRangeValue = sRangeSpan.ToString
  End Sub

End Class