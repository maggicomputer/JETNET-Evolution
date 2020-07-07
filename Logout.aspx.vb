Partial Public Class Logout
  Inherits System.Web.UI.Page

  Private bIsBadIP As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim CheckForRedirect As Boolean = False

    If Not IsNothing(Request.Item("badip")) Then
      If Not String.IsNullOrEmpty(Request.Item("badip").ToString.Trim) Then
        bIsBadIP = CBool(Request.Item("badip").ToString)
      End If
    End If

    If Not bIsBadIP Then

      If Session.Item("localUser").crmEvo = True Then

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
          CheckForRedirect = True
        End If

        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT) Then
          logo.ImageUrl = "~/images/JETNET_YachtSpot.png" 'swap logo
          background_image.ImageUrl = "https://www.jetnetevolution.com/images/background/31.jpg"
          Page.Header.Title = "Yacht Spot Logout"
          logo.CssClass = "evolution_logo"

          welcome_to_text.Text = "You have been logged out of Yacht Spot Online"
        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Then
          logo.ImageUrl = "~/images/JETNET_EvoMarketplace.png" 'swap logo
          logo.CssClass = "evolution_logo"
          If Session.Item("isMobile") = True Then
            logo.CssClass += " home logoMobileOffset"
            logo.ImageUrl = "~/images/JETNET_EvoMarketplace_Mobile.png"
            background_image.Visible = False
            fixedBar.Visible = True
            mobile_styles.Visible = True
          End If
          Page.Header.Title = "JETNET Logout"
          background_image.ImageUrl = "https://www.jetnetevolution.com/images/background/59.jpg"
        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
          logo.ImageUrl = "~/images/JETNET_EvoAdmin_Outlines.png" 'swap logo
          Page.Header.Title = "Evolution Admin Logout"
          background_image.ImageUrl = "https://www.jetnetevolution.com/images/background/59.jpg"
          welcome_to_text.Text = "You have been logged out of Evolution Admin Online"
        End If
      Else
        logo.ImageUrl = "~/images/JETNET_MarketplaceMan.png" 'swap logo out
        Page.Header.Title = "Marketplace Manager Logout"
        logo.CssClass = "evolution_logo"
        background_image.ImageUrl = "https://www.jetnetevolution.com/images/background/10.jpg"
        welcome_to_text.Text = "You have been logged out of the Marketplace Manager."
      End If

      Dim bEnableChat As Boolean = False

      ChatManager.CheckAndInitChat(False, bEnableChat)

      If bEnableChat Then
        ChatManager.LogMySessionOff(HttpContext.Current)
      End If

      If Session.Item("isEVOLOGGING") Then
        Dim aclsTemp As New clsData_Manager_SQL
        aclsTemp.Update_Evo_Sub_Dates("logout", Now(), HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID, HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)
      End If

      If CheckForRedirect Then
        If Not IsNothing(Trim(Request("swap"))) Then
          If Not String.IsNullOrEmpty(Trim(Request("swap"))) Then
            If Trim(Request("swap")) = "true" Then
              Dim URLString As String = ""
              Dim Parameters As String = ""
              URLString = Trim(Request("url"))
              Parameters = "?swap=true&2=" & Trim(Request("2")) & "&1=" & Trim(Request("1"))
              Response.Redirect(URLString & Parameters, True)
            End If
          End If
        End If
      End If

      'This is for the API auto login.
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        If Not IsNothing(Trim(Request("apiLog"))) Then
          If Not String.IsNullOrEmpty(Trim(Request("apiLog"))) Then
            If Trim(Request("apiLog")) = "true" Then
              Dim URLString As String = ""
              URLString = "/default.aspx?apiLog=true&2=" & Trim(Request("2")) & "&1=" & Trim(Request("1")) & "&type=" & Trim(Request("type")) & "&id=" & Trim(Request("id"))

              If Not IsNothing(Trim(Request("compid"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("compid"))) Then
                  URLString += "&compid=" & Trim(Request("compid"))
                End If
              End If

              If Not IsNothing(Trim(Request("jid"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("jid"))) Then
                  URLString += "&jid=" & Trim(Request("jid"))
                End If
              End If

              If Not IsNothing(Trim(Request("source"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
                  URLString += "&source=" & Trim(Request("source"))
                End If
              End If

              Response.Redirect(URLString)
            End If
          End If
        End If
      End If

    Else

      logo.ImageUrl = "~/images/JETNET_EvoMarketplace.png"
      logo.CssClass = "evolution_logo"
      Page.Header.Title = "JETNET EVOLUTION UNABLE TO ACCESS"
      background_image.ImageUrl = "https://www.jetnetevolution.com/images/background/59.jpg"
      welcome_to_text.ForeColor = Drawing.Color.Red
      welcome_to_text.Text = "UNABLE TO ACCESS CURRENT SYSTEM.<br /><br />PLEASE CONTACT JETNET’S CUSTOMER SERVICE FOR ADDITIONAL HELP!"

      welcome_paragraph.Text = "<p><a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or <a href='tel:800-553-8638'>(800)-553-8638</a></p>"

    End If

    Session.Contents.Clear()
    Session.Abandon()
    Session.Item("Listing") = ""
    Session.Item("Subnode") = ""
    Session.Item("ID") = ""

  End Sub

End Class