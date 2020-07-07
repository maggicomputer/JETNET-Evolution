' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Notify.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:40a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: Notify.aspx.vb $
'
' ********************************************************************************

Partial Public Class Notify
  Inherits System.Web.UI.Page
  Dim aircraftID As Long = 0
  Dim JournalID As Long = 0
  Dim companyID As Long = 0
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load user session : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      Master.RemoveAllStyleElements(True)

      'We need the Aircraft ID:
      If Not IsNothing(Request.Item("acid")) Then
        If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
          aircraftID = CLng(Request.Item("acid").ToString.Trim)
        End If
      End If

      'We need the Aircraft ID:
      If Not IsNothing(Request.Item("compid")) Then
        If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
          companyID = CLng(Request.Item("compid").ToString.Trim)
          tellChangesText.InnerText = "TELL JETNET ABOUT CHANGES TO THIS COMPANY"
          tellTypeText.InnerText = "company"
        End If
      End If

      'We need the Journal ID:
      If Not IsNothing(Request.Item("jID")) Then
        If Not String.IsNullOrEmpty(Request.Item("jID").ToString) Then
          JournalID = CLng(Request.Item("jID").ToString.Trim)
        End If
      End If


    End If

  End Sub
  Public Sub validateLength(ByVal sender As Object, ByVal args As ServerValidateEventArgs)
    responseText.Text = responseText.Text.TrimEnd().TrimStart()
    Dim lenAnswer As Integer = Len(responseText.Text)
    If lenAnswer > 2000 Or lenAnswer = 0 Then 'I am using the text straight out of the textbox
      'instead of args.value because we're trimming it up above to remove trailing/leading spaces.
      args.IsValid = False
      Exit Sub
    End If
    args.IsValid = True
  End Sub
  Private Sub submitNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitNotify.Click
    If Page.IsValid Then

      'We very much need to input the information. We are only inputting a couple of variables. One is the messageText. It 
      'needs to be a length of 2000 and should be validated client side. However since client side validation can fail, we're going to go ahead and 
      'validate server side as well.
      If Len(responseText.Text) <= 2000 And Len(responseText.Text) > 0 Then 'This means we're hopefully good to go and submit

        'Time to run the submission.
        'Below are the fields that we need:
        'o	Subscriber information (sub/login/seq/contact id/etc) filled in
        'o(Subislog_msg_type = "Subscriber Feedback")
        'o	aircraft id filled in.
        'o	Set webaction_date = '1/1/1900'
        'o	Fill in subislog_message – with what they type in the form.

        Call commonLogFunctions.Log_User_Event_Data("Submitted Data", Replace(responseText.Text, "'", "''"), Nothing, 0, JournalID, 0, companyID, 0, aircraftID, 0, 0, "1/1/1900")
        pre_submittal_form.Visible = False
        post_submittal_form.Visible = True
      ElseIf Len(responseText.Text) = 0 Then 'We need to warn the user, this field is required.
        attention.Text = "*Text is required"
      Else 'We must send a warning back to the user.
        attention.Text = "*Text must be no longer than 2000 characters."
      End If
    End If
  End Sub


End Class