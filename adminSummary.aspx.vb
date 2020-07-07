
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminSummary.aspx.vb $
'$$Author: Matt $
'$$Date: 4/28/20 10:33a $
'$$Modtime: 4/28/20 10:33a $
'$$Revision: 6 $
'$$Workfile: adminSummary.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminSummary

  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Private nReportID As Integer = 0
  Public Shared masterPage As New Object

  Private Sub adminSummary_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    Try

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, HomebaseTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (adminSummary_PreInit): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (adminSummary_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sDisplayAdminReportList As String = ""
    Dim sAdminReportString As String = ""
    Dim sAdminReportFileName As String = ""
    Dim sReportOutputFilename As String = ""
    Dim sErrorString As String = ""
    Dim type_of As String = ""

    Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Not IsNothing(Request.Item("rid")) Then
        If Not String.IsNullOrEmpty(Request.Item("rid").ToString.Trim) Then
          If IsNumeric(Request.Item("rid")) Then
            nReportID = CInt(Request.Item("rid").ToString)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("rtype")) Then
        If Not String.IsNullOrEmpty(Request.Item("rtype").ToString.Trim) Then
          type_of = Request.Item("rtype").ToString
        End If
      End If

      reportErrorLbl.Visible = False


      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        masterPage.Set_Active_Tab(7)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Administrative Reports Center")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        masterPage.Set_Active_Tab(7)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase Reports Center")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      If nReportID > 0 Then

                localDatalayer.generateAdminReport(nReportID, sAdminReportString, 0, "", True, False)

                If Not String.IsNullOrEmpty(sAdminReportString.Trim) Then

          Dim sReportTitle = subscriptionInfo + "adminReport_" + nReportID.ToString

          sAdminReportFileName = commonEvo.GenerateFileName(sReportTitle, ".xls", False)

          If write_report_string_to_file(sAdminReportString, sAdminReportFileName) Then
            sReportOutputFilename = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "/" + sAdminReportFileName.Trim
          Else
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in sending string to file"
          End If

          If String.IsNullOrEmpty(sReportOutputFilename.Trim) Then
            SetReportErrorText("<b><font color=""red"">There was an error generateing your report! Please check your selections and try running again.</font></b>")
          Else

            Dim reportURL As String = "openReportWindow(""" + sReportOutputFilename.Trim + """,""" + nReportID.ToString + """);"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PopUpSelectedReportWindow", reportURL, True)

          End If

        Else
          SetReportErrorText("<b><font color=""red"">There was an error generateing your report! Please check your selections and try running again.</font></b>")
        End If

      End If

      localDatalayer.displayAdminReportList(sDisplayAdminReportList, , , True, type_of)
      adminReportsListLbl.Text = sDisplayAdminReportList.Trim

    End If


  End Sub

  Private Sub SetReportErrorText(ByVal errorText As String)
    reportErrorLbl.Visible = True
    reportErrorLbl.Text = errorText
  End Sub

  Private Function write_report_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean

    Try

      Dim f As System.IO.StreamWriter

      f = System.IO.File.CreateText(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sReportname.Trim)

      ' write to the file
      f.WriteLine(sOutoutString)

      'close the streamwriter
      f.Close()
      f.Dispose()
      f = Nothing

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in write_report_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean " + ex.Message
      Return False
    End Try

    Return True

  End Function

End Class