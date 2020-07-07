' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/UserVerification.aspx.vb $
'$$Author: Amanda $
'$$Date: 2/03/20 3:11p $
'$$Modtime: 2/03/20 2:30p $
'$$Revision: 4 $
'$$Workfile: UserVerification.aspx.vb $
'
' ********************************************************************************

Imports System.IO
Imports System.Net

Public Class UserVerification

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Call commonLogFunctions.Log_User_Event_Data("UserAbuse", "User Displayed Captcha for Potential Abuse", Nothing, 0, 0, 0, 0, 0, 0, 0)
        End If
    End Sub


    Private Sub SUBMIT_BUTTON_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitButton.Click
        If Page.IsValid Then
            'Validate recaptcha
            Dim valid As Boolean = False

            Try


                If Not IsNothing(Trim(Request("g-recaptcha-response"))) Then
                    Dim ResponseVar As String = Trim(Request("g-recaptcha-response"))
                    valid = True
                    Dim req As HttpWebRequest = DirectCast(WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LfsWdUUAAAAAGDaTJGgTQa1SglOwIPtuvViiLft&response=" + ResponseVar), HttpWebRequest)

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

                    Dim aclsData_Temp As New clsData_Manager_SQL
                    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
                    aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

                    RecaptchaFail.Visible = False
                    'We need to mark it as complete: 

                    aclsData_Temp.Update_Subscription_Heavy_User_Log(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
                    Call commonLogFunctions.Log_User_Event_Data("UserAbuse", "User Cleared Captcha for Potential Abuse", Nothing, 0, 0, 0, 0, 0, 0, 0)
                    Response.Redirect("/home.aspx")
                Else
                    RecaptchaFail.Visible = True
                    attention_text.Text = ("<p>Invalid Recaptcha. Please check your submission and try again.</p>")
                    attention_text.Visible = True
                End If
            Catch ex As Exception
                attention_text.Text = ("<p>Recaptcha Error. Please check your submission and try again.</p>")
                attention_text.Visible = True
            Finally
                ' validationUpdate.Update()
            End Try

        End If
    End Sub


End Class