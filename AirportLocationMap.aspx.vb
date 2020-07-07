' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/AirportLocationMap.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:37a $
'$$Modtime: 6/18/19 6:11p $
'$$Revision: 2 $
'$$Workfile: AirportLocationMap.aspx.vb $
'
' ********************************************************************************

Partial Public Class AirportLocationMap
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    Dim airport_lat As String = ""
    Dim airport_long As String = ""
    Dim airport_title As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load flight data : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      If Not IsNothing(Request.Item("aportLat")) Then
        If Not String.IsNullOrEmpty(Request.Item("aportLat").ToString.Trim) Then
          airport_lat = Request("aportLat").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("aportLong")) Then
        If Not String.IsNullOrEmpty(Request.Item("aportLong").ToString.Trim) Then
          airport_long = Request("aportLong").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("aportTitle")) Then
        If Not String.IsNullOrEmpty(Request.Item("aportTitle").ToString.Trim) Then
          airport_title = Request("aportTitle").ToString.Trim
        End If
      End If

      Master.SetPageTitle(airport_title)  ' sets the page title and page.text

      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "initializeMap", "initialize(" + airport_lat.Trim + "," + airport_long.Trim + ",""" + airport_title.Trim + """);", True)

    End If

  End Sub

End Class