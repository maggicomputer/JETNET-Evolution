' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/ChatBox.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: ChatBox.aspx.vb $
'
' ********************************************************************************

Partial Public Class _ChatBox
  Inherits System.Web.UI.Page
  Public txtAlias As String = ""
  Public txtAliasID As Long = 0
  Public communityAliasID As Long = 0
  Public communityAlias As String = ""
  Public bIsAdd As Boolean = False
  Public localRoomID As String = ""
  Public chatWithFriendlyName As String = ""

  Public fullHostname As String = ""

  Private roomID As Guid

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try

      HttpContext.Current.Response.ClearHeaders()
      HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate") ' HTTP 1.1.
      HttpContext.Current.Response.AddHeader("Pragma", "no-cache") ' HTTP 1.0.
      HttpContext.Current.Response.AddHeader("Expires", "-1") ' Proxies.

      If Not IsPostBack Then

        Dim CompanyID As Long = 0
        Dim JournalID As Long = 0
        Dim ContactID As Long = 0

        Dim ContactTable As New DataTable

        Dim friendlyName As String = ""
        Dim sDoingBusinessAs As String = ""

        fullHostname = Session.Item("jetnetFullHostName")

        If Not IsNothing(Request.Item("rid")) Then
          If Not String.IsNullOrEmpty(Request.Item("rid").ToString.Trim) Then
            roomID = New Guid(Request.Item("rid").ToString)
            localRoomID = roomID.ToString
          End If
        End If

        If Not IsNothing(roomID) Then
          If roomID <> Guid.Empty Then

            Dim notifyInfo = ChatManager.GetNotificationInfo(roomID)

            If Not IsNothing(notifyInfo) Then

              Dim currentSession = ChatManager.GetMySession(HttpContext.Current)

              txtAlias = currentSession.UserAlias
              txtAliasID = currentSession.UID

              If currentSession.UID = notifyInfo.ToUserUID Then

                ContactID = notifyInfo.FromUserContactID
                CompanyID = notifyInfo.FromUserCompanyID
                If Not ChatManager.IsUserOnCommunityList(HttpContext.Current, notifyInfo.FromAlias, notifyInfo.FromUserUID) Then
                  bIsAdd = True
                End If

                communityAlias = notifyInfo.FromAlias
                communityAliasID = notifyInfo.FromUserUID

                chatWithFriendlyName = notifyInfo.FromUserName.Trim

              ElseIf currentSession.UID = notifyInfo.FromUserUID Then

                ContactID = notifyInfo.ToUserContactID
                CompanyID = notifyInfo.ToUserCompanyID

                If Not ChatManager.IsUserOnCommunityList(HttpContext.Current, notifyInfo.ToAlias, notifyInfo.ToUserUID) Then
                  bIsAdd = True
                End If
                communityAlias = notifyInfo.ToAlias
                communityAliasID = notifyInfo.ToUserUID
                chatWithFriendlyName = notifyInfo.ToUserName.Trim

              End If

            End If

          End If
        End If

        If ContactID > 0 And CompanyID > 0 Then

          'Fills Company Information Tab
          crmWebClient.CompanyFunctions.Fill_Information_Tab_ChatBox(company_name, company_information_label, CompanyID, JournalID, sDoingBusinessAs, company_address)

          ContactTable = commonEvo.get_contact_info_fromID_returnDatatable(CompanyID, ContactID, 0, True)

          If Not IsNothing(ContactTable) Then

            If ContactTable.Rows.Count > 0 Then

              friendlyName = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_sirname").ToString.Trim), ContactTable.Rows(0).Item("contact_sirname").ToString.Trim + " ", ""), "")
              friendlyName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_first_name")), ContactTable.Rows(0).Item("contact_first_name").ToString.Trim + " ", "")
              friendlyName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim), ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")
              friendlyName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_last_name")), ContactTable.Rows(0).Item("contact_last_name").ToString, "")

              Page.Title = "JETNET Community Chat - " + friendlyName.Trim

              Dim imgDisplayFolder As String = HttpContext.Current.Session.Item("jetnetFullHostName") + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")
              Dim TheFile As System.IO.FileInfo
              Dim contactImageLink As String = ""
              Dim contactImageFile As String = ""

              Dim temp_height As Integer = 0
              Dim temp_width As Integer = 0
              Dim zimage2 As System.Drawing.Image
              Dim desired_width As Integer = 0
              Dim desired_height As Integer = 0
              Dim returnString As String = ""

              If Not IsDBNull(ContactTable.Rows(0).Item("conpic_contact_id")) Then

                contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString
                contactImageFile = HttpContext.Current.Server.MapPath("/" + contactImageLink)

                TheFile = New System.IO.FileInfo(contactImageFile)

                If TheFile.Exists Then 'is the file actually there?

                  zimage2 = System.Drawing.Image.FromFile(contactImageFile)
                  temp_width = zimage2.Width
                  temp_height = zimage2.Height
                  desired_width = 100
                  desired_height = 70

                  DisplayFunctions.Resize_Image(temp_width, temp_height, desired_width, desired_height, returnString, imgDisplayFolder.Trim + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString, friendlyName.Trim, "margin_4 float_right border")
                  contact_picture.Text = returnString
                Else
                  contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "-" + ContactTable.Rows(0).Item("conpic_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString
                  contactImageFile = HttpContext.Current.Server.MapPath("/" + contactImageLink)

                  TheFile = New System.IO.FileInfo(contactImageFile)

                  If TheFile.Exists Then 'is the file actually there?

                    zimage2 = System.Drawing.Image.FromFile(contactImageFile)
                    temp_width = zimage2.Width
                    temp_height = zimage2.Height
                    desired_width = 100
                    desired_height = 70

                    DisplayFunctions.Resize_Image(temp_width, temp_height, desired_width, desired_height, returnString, imgDisplayFolder.Trim + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "-" + ContactTable.Rows(0).Item("conpic_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString, friendlyName.Trim, "margin_4 float_right border")
                    contact_picture.Text = returnString
                  End If

                End If

              End If

              ContactFunctions.Display_Contact_Details_ChatBox(ContactTable, followUpUserInformation, CompanyID, JournalID)

            End If

          End If

          If Not IsNothing(ContactTable) Then
            ContactTable = Nothing
          End If

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Page_Load : ChatBox.aspx</b><br />" + ex.Message
    End Try

  End Sub

End Class

