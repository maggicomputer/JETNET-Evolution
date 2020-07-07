Partial Public Class validateUser
  Inherits System.Web.UI.UserControl

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case Session.Item("jetnetAppVersion")
            Case Constants.ApplicationVariable.YACHT
                Label1.Text = "Yacht Spot User Validation Failure." ' - Please check your username and password and try again."
            Case Constants.ApplicationVariable.CRM
                Label1.Text = "Jetnet CRM User Validation Failure." ' - Please check your username and password and try again."
            Case Constants.ApplicationVariable.EVO
                Label1.Text = "Jetnet Validation Failure." ' - Please check your username and password and try again."
            Case Constants.ApplicationVariable.CUSTOMER_CENTER
                Label1.Text = "Jetnet Validation Failure." ' - Please check your username and password and try again."
        End Select

    End Sub

  Public Sub setValidationText(ByVal in_sErrorText As String)
        Me.FailureText1.Text = in_sErrorText.Trim
  End Sub

End Class