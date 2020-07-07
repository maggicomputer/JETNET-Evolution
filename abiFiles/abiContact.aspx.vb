Imports System.Net.Mail
Imports System.Net
Imports System.IO

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/abiFiles/abiContact.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:42a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: abiContact.aspx.vb $
'
' ********************************************************************************

Partial Public Class abiContact
  Inherits System.Web.UI.Page
  Dim acID As Long = 0

  Public errorString As String = ""
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim aircraftData As New DataTable

    If Not Page.IsPostBack Then
      Dim CountryTable As New DataTable
      CountryTable = Master.AbiDataManager.GetCountry()

      If Not IsNothing(CountryTable) Then
        If CountryTable.Rows.Count > 0 Then
          clsGeneral.clsGeneral.Populate_Dropdown(CountryTable, country, "country_name", "country_name", False)
          country.Items.RemoveAt(0)
          country.Items.Insert(0, New ListItem("Please Select One", ""))
          country.SelectedValue = "United States"
        End If
      End If

    End If

    If Not IsNothing(Trim(Request("inquiry"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("inquiry"))) Then
        interest.SelectedValue = "Dealer Aircraft Listing"
        interest.Enabled = False
      End If
    End If
    If Not IsNothing(Trim(Request("acID"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("acID"))) Then
        acID = Trim(Request("acID"))
        acID = Server.UrlDecode(acID)
        interest.SelectedValue = "Dealer Aircraft Listing"
        interest.Enabled = False
        aircraftData = Master.AbiDataManager.GetABIACDetails(acID, 0, 0, "")
        If Not IsNothing(aircraftData) Then
          If aircraftData.Rows.Count > 0 Then
            dealerInformation.Text = "<div class=""items-row col-2 row-fluid"">"
            dealerInformation.Text += Master.AbiDataManager.DisplayRightHandColumn(aircraftData, False)
            dealerInformation.Text += "</div>"
            companyID.Text = aircraftData.Rows(0).Item("comp_id")

            modelInformation.Text = ""

            'Ser #
            If Not IsDBNull(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
              If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("ac_ser_no_full")) Then
                modelInformation.Text += "SER #"
                modelInformation.Text += aircraftData.Rows(0).Item("ac_ser_no_full").ToString & "&nbsp;&nbsp;|&nbsp;&nbsp;"
              End If
            End If

            If Not IsDBNull(aircraftData.Rows(0).Item("amod_make_name")) Then
              If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_make_name")) Then
                modelInformation.Text += aircraftData.Rows(0).Item("amod_make_name").ToString & " "
              End If
            End If

            If Not IsDBNull(aircraftData.Rows(0).Item("amod_model_name")) Then
              If Not String.IsNullOrEmpty(aircraftData.Rows(0).Item("amod_model_name")) Then
                modelInformation.Text += aircraftData.Rows(0).Item("amod_model_name").ToString
              End If
            End If

          End If
        End If
      End If
    End If


  End Sub

  

  Private Sub CreateEmail()
    Dim ResponseVar As Boolean = False
    Dim HTMLStr As String = ""

    'Variables.
    Dim abiemail_category As String = ""
    Dim abiemail_comp_name As String = ""
    Dim abiemail_first_name As String = ""
    Dim abiemail_last_name As String = ""
    Dim abiemail_address1 As String = ""
    Dim abiemail_address2 As String = ""
    Dim abiemail_city As String = ""
    Dim abiemail_state As String = ""
    Dim abiemail_zip_code As String = ""
    Dim abiemail_phone As String = ""
    Dim abiemail_email_address As String = ""
    Dim abiemail_notes As String = ""
    Dim abiemail_comp_id As Long = 0
    Dim abiemail_ac_id As Long = 0
    Dim abiemail_want_id As Long = 0
    Dim abiemail_to As String = ""
    Dim abiemail_from As String = ""
    Dim abiemail_subject As String = ""
    Dim abiemail_body As String = ""


    HTMLStr += "<html><body bgcolor=""white"" style=""background-image: none;""><table width=""100%"" cellpadding=""3"" cellspacing=""0"" style=""font-size:11px;"">"
    HTMLStr += "<tr>"
    HTMLStr += "<td align=""left"" valign=""top"" width=""20%"">&nbsp;</td>"
    HTMLStr += "<td align=""left"" valign=""top"">&nbsp;</td>"
    HTMLStr += "</tr>"

    If companyID.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"" width=""20%"">Inquiring About:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & modelInformation.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_comp_id = companyID.Text
      abiemail_ac_id = acID
    End If

    If interest.SelectedValue <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Interest:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & interest.SelectedValue & "</td>"
      HTMLStr += "</tr>"
      abiemail_category = interest.SelectedValue
    End If

    If company_name.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Company Name:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & company_name.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_comp_name = company_name.Text
    End If

    If first_name.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">First Name:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & first_name.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_first_name = first_name.Text
    End If

    If last_name.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Last Name:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & last_name.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_last_name = last_name.Text
    End If

    If address.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Address:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & address.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_address1 = address.Text
    End If

    If address_cont.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Address (cont):</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & address_cont.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_address2 = address_cont.Text
    End If

    If phone.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Phone:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & phone.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_phone = phone.Text
    End If

    If city.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">City:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & city.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_city = city.Text
    End If

    If state.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">State:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & state.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_state = state.Text
    End If

    If zip.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Zip:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & zip.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_zip_code = zip.Text
    End If

    If country.SelectedValue <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Country:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & country.SelectedValue & "</td>"
      HTMLStr += "</tr>"
    End If

    If email.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"">Email:</td>"
      HTMLStr += "<td align=""left"" valign=""top"">" & email.Text & "</td>"
      HTMLStr += "</tr>"
      abiemail_email_address = email.Text
      abiemail_from = email.Text
    End If

    If message.Text <> "" Then
      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"" colspan=""2"">Message:</td>"
      HTMLStr += "</tr>"

      HTMLStr += "<tr>"
      HTMLStr += "<td align=""left"" valign=""top"" colspan=""2"">" & message.Text & "</td>"
      HTMLStr += "</tr>"

      abiemail_notes = message.Text
    End If

    HTMLStr += "</table></body></html>"

    abiemail_body = HTMLStr

    If companyID.Text <> "" Then
      abiemail_subject = "JETNET Global Aircraft Inquiry"
      abiemail_to = Master.AbiDataManager.getCompanyEmail(companyID.Text)
      ResponseVar = Master.AbiDataManager.Create_Email_Record(abiemail_category, abiemail_comp_name, abiemail_first_name, abiemail_last_name, abiemail_address1, abiemail_address2, abiemail_city, abiemail_state, abiemail_zip_code, abiemail_phone, abiemail_email_address, abiemail_notes, abiemail_comp_id, abiemail_ac_id, abiemail_want_id, abiemail_to, abiemail_from, abiemail_subject, abiemail_body)

      'ResponseVar = Send_Email(email.Text, Master.AbiDataManager.getCompanyEmail(companyID.Text), "", "", "Jetnet GLOBAL Contact Request", HTMLStr)
      If ResponseVar Then
        attention.Text = "<p>Your message has been sent to the Dealer down below, along with your contact information.</p>"
      Else
        attention.Text = "<p>Your message has not been sent. <br /><br />Please contact us with this error message: <u>" & errorString & "</u></p>"
      End If
    Else
      abiemail_subject = "JETNET Global " & interest.SelectedValue & " Inquiry"
      abiemail_to = "jetnetglobal@jetnet.com"
      ResponseVar = Master.AbiDataManager.Create_Email_Record(abiemail_category, abiemail_comp_name, abiemail_first_name, abiemail_last_name, abiemail_address1, abiemail_address2, abiemail_city, abiemail_state, abiemail_zip_code, abiemail_phone, abiemail_email_address, abiemail_notes, abiemail_comp_id, abiemail_ac_id, abiemail_want_id, abiemail_to, abiemail_from, abiemail_subject, abiemail_body)

      ' ResponseVar = Send_Email(email.Text, "jetnetglobal@jetnet.com", "", "", "Jetnet GLOBAL Contact Request", HTMLStr)

      If ResponseVar Then
        attention.Text = "<p>Thank you for contacting us. Your Message has been sent.</p>"
      Else
        attention.Text = "<p>Your message has not been sent. <br /><br />Please contact us with this error message: <u>" & errorString & "</u></p>"
      End If
    End If


      attention.Visible = True
      textBody.Visible = False

  End Sub

  Private Sub submitForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitForm.Click
    'Validate recaptcha
    Dim valid As Boolean = False

    If Not IsNothing(Trim(Request("g-recaptcha-response"))) Then
      Dim ResponseVar As String = Trim(Request("g-recaptcha-response"))
      Dim req As HttpWebRequest = DirectCast(WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LdLCgUTAAAAAMSiL5NRxn5MY44PhRuz1HAkSTwF&response=" + ResponseVar), HttpWebRequest)

      Try
        'Google recaptcha Response
        Using wResponse As WebResponse = req.GetResponse()

          Using readStream As New StreamReader(wResponse.GetResponseStream())
            Dim jsonResponse As String = readStream.ReadToEnd()

            Dim jss = New Script.Serialization.JavaScriptSerializer()
            Dim data = jss.Deserialize(Of Object)(jsonResponse)

            valid = Convert.ToBoolean(data("success"))

          End Using
        End Using

      Catch ex As WebException
        Throw ex
      End Try

    End If

    If valid Then
      RecaptchaFail.Visible = False

      CreateEmail()
    Else
      RecaptchaFail.Visible = True
      attention.Text = ("<p>Invalid Recaptcha. Please check your submission and try again.</p>")
      attention.Visible = True
    End If
  End Sub


  Public Function Send_Email(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String) As Boolean
    Try
      ' Instantiate a new instance of MailMessage
      Dim mMailMessage As New MailMessage()

      ' Set the sender address of the mail message
      mMailMessage.From = New MailAddress(from)
      ' Set the recepient address of the mail message
      mMailMessage.To.Add(New MailAddress(recepient))

      ' Check if the bcc value is nothing or an empty string
      If Not bcc Is Nothing And bcc <> String.Empty Then
        ' Set the Bcc address of the mail message
        mMailMessage.Bcc.Add(New MailAddress(bcc))
      End If

      ' Check if the cc value is nothing or an empty value
      If Not cc Is Nothing And cc <> String.Empty Then
        ' Set the CC address of the mail message
        mMailMessage.CC.Add(New MailAddress(cc))
      End If

      ' Set the subject of the mail message
      mMailMessage.Subject = subject
      ' Set the body of the mail message
      mMailMessage.Body = body


      ' Set the format of the mail message body as HTML
      mMailMessage.IsBodyHtml = True
      ' Set the priority of the mail message to normal
      mMailMessage.Priority = MailPriority.Normal

      ' Instantiate a new instance of SmtpClient
      Dim mSmtpClient As New SmtpClient("localhost", 25)
      ' Send the mail message
      mSmtpClient.Send(mMailMessage)
      Return True
    Catch ex As Exception
      errorString = ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " Send_Email ABIContact.aspx.vb -" & ex.Message
      Return False
    End Try
  End Function
End Class