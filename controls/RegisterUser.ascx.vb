
Partial Public Class _RegisterUser

  Inherits System.Web.UI.UserControl

  Public Event RegisterClientStatus As EventHandler
  Public Event RegisterClientFailed As EventHandler

  Protected Overridable Sub OnRegisterClientStatus(ByVal e As EventArgs)
    RaiseEvent RegisterClientStatus(Me, e)
  End Sub

  Protected Overridable Sub OnRegisterClientFailed(ByVal e As EventArgs)
    RaiseEvent RegisterClientFailed(Me, e)
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Me.TextSubID.Focus()

  End Sub

  Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RegisterButton.Click

    Try

      If Me.Register_Client() Then
        Session.Item("localSubscription").crmSubStatusCode = eObjStatusCode.SUCCESS
        Session.Item("localUser").crmUserStatusCode = eObjStatusCode.SUCCESS
        Call OnRegisterClientStatus(New EventArgs())
      Else
        Session.Item("localSubscription").crmSubStatusCode = eObjStatusCode.FAILURE
        Session.Item("localUser").crmUserStatusCode = eObjStatusCode.FAILURE
        Call OnRegisterClientFailed(New EventArgs())
      End If

    Catch ex As Exception
      Me.FailureText1.Text = "RegisterUser.ascx > RegisterButton_Click Error : " & ex.Message

    End Try

    Return

  End Sub

  Private Function generate_subscription_code(ByVal nSubID As Long, ByVal sUserID As String, ByVal nSeqNo As Long) As String

    Return Trim("Subscription Code - " + nSubID.ToString + "," + sUserID + "," + nSeqNo.ToString)

  End Function

  Private Function Register_Client() As Boolean

    ' register local crm install on local client

    'Dim slocalInstallValue As String = ""
    'Dim slocalSubscriptionCode As String = ""

    'Dim objCrmService As New com.jetnet.jetnetcrm.crmWebService
    'Dim objReturnStatus As New com.jetnet.jetnetcrm.returnStatus
    'Dim objReturnLogonInfo As New com.jetnet.jetnetcrm.returnExtraLogonData

    'Dim securityTokenLocal As String = ""
    'Dim sQuery As String = ""

    'objReturnStatus.Status_code = com.jetnet.jetnetcrm.eStatusCode.NULL
    'objReturnStatus.Description = ""

    'Try

    '  If Not (String.IsNullOrEmpty(Me.TextSubID.Text.Trim) And String.IsNullOrEmpty(Me.TextUserID.Text.Trim) And String.IsNullOrEmpty(Me.TextPswd.Text.Trim)) Then

    '    Session.Item("localUser").crmSubSubID = CLng(Me.TextSubID.Text)
    '    Session.Item("localUser").crmSubUserID = Me.TextUserID.Text
    '    Session.Item("localUser").crmSubPswdID = Me.TextPswd.Text
    '    Session.Item("localUser").crmUserLogin = Session.Item("localUser").crmSubUserID + "|" + Session.Item("localUser").crmSubPswdID

    '    objCrmService.Timeout = -1 ' infinite time out.

    '    If objCrmService.RegisterClientInstall(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubUserID, Session.Item("localUser").crmSubPswdID, objReturnStatus, objReturnLogonInfo) Then

    '      If objReturnStatus.Status_code = com.jetnet.jetnetcrm.eStatusCode.SUCCESS Then

    '        If Not IsNothing(objReturnLogonInfo) Then

    '          Session.Item("localUser").crmSubStartDate = CDate(objReturnLogonInfo.LogonSubStartDate)

    '          If objReturnLogonInfo.LogonNoEndDate Then
    '            Session.Item("localUser").crmSubNoEndDate = objReturnLogonInfo.LogonNoEndDate
    '          Else
    '            Session.Item("localUser").crmSubEndDate = CDate(objReturnLogonInfo.LogonSubEndDate)
    '          End If

    '          Session.Item("localUser").crmSubSeqNo = objReturnLogonInfo.LogonSubSeqNumber
    '          Session.Item("localUser").crmUserCompanyID = objReturnLogonInfo.LogonSubCompanyID
    '          Session.Item("localUser").crmUserContactID = objReturnLogonInfo.LogonSubContactID

    '          Session.Item("localSubscription").crmMaxUserCount = objReturnLogonInfo.LogonSubTotalInstalCount

    '          Session.Item("localSubscription").crmTierlevel = objReturnLogonInfo.LogonSubTierlevel
    '          Session.Item("localSubscription").crmProductCode = objReturnLogonInfo.LogonSubProductCode
    '          Session.Item("localSubscription").crmFrequency = objReturnLogonInfo.LogonSubFrequency

    '          If Not String.IsNullOrEmpty(objReturnLogonInfo.LogonSubAerodexFlag) Then

    '            If objReturnLogonInfo.LogonSubAerodexFlag = "Y" Then
    '              Session.Item("localSubscription").crmAerodexFlag = True
    '            Else
    '              Session.Item("localSubscription").crmAerodexFlag = False
    '            End If

    '          Else
    '            Session.Item("localSubscription").crmAerodexFlag = False
    '          End If

    '        Else

    '          Session.Item("localUser").crmSubStartDate = Today().ToString
    '          Session.Item("localUser").crmSubNoEndDate = True
    '          Session.Item("localUser").crmSubSeqNo = 0

    '        End If

    '        Session.Item("localUser").crmSubscriptionCode = generate_subscription_code(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubUserID, Session.Item("localUser").crmSubSeqNo)
    '        Session.Item("localUser").crmSecurityToken = objCrmService.CreateSecurityToken(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubUserID, Session.Item("localUser").crmSubPswdID, CInt(Session.Item("localUser").crmSubSeqNo), objReturnStatus)

    '        If objReturnStatus.Status_code = com.jetnet.jetnetcrm.eStatusCode.SUCCESS And Not String.IsNullOrEmpty(Session.Item("localUser").crmSecurityToken) Then

    '          Session.Item("localUser").crmSubInstallDate = Today().ToString
    '          Session.Item("localUser").crmSubAccessDate = Now().ToString

    '          Select Case Session.Item("localSubscription").crmLogonType ' if saveInCookie true then use encrypted cookies to check for install

    '            Case eLogonTypes.COOKIE

    '            Case eLogonTypes.DATABASE ' save the user info as encrypted strings in the local client database

    '              If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
    '                ' connect to local client database for data connections for this host
    '                sQuery = "UPDATE Client_Register SET client_regSubscriptionCode = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubscriptionCode) + "',"
    '                sQuery = sQuery + " client_regSecurityToken = '" + Session.Item("localUser").crmSecurityToken + "',"
    '                sQuery = sQuery + " client_regInstallDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubInstallDate) + "',"
    '                sQuery = sQuery + " client_regAccessDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubAccessDate) + "',"
    '                sQuery = sQuery + " client_webUserLimit = " + Session.Item("localSubscription").crmMaxUserCount.ToString + ","
    '                sQuery = sQuery + " client_webInstanceID = " + Application.Item("crmClientSiteData").crmWebInstanceID.ToString + ","
    '                sQuery = sQuery + " client_regStatus = 'Y',"
    '                If Session.Item("localSubscription").crmAerodexFlag Then
    '                  sQuery = sQuery + " client_regAerodexFlag = 'Y',"
    '                Else
    '                  sQuery = sQuery + " client_regAerodexFlag = 'N',"
    '                End If
    '                sQuery = sQuery + " client_regFrequency = '" + Session.Item("localSubscription").crmFrequency + "',"
    '                sQuery = sQuery + " client_regTierLevel = '" + Session.Item("localSubscription").crmTierlevel + "',"
    '                sQuery = sQuery + " client_regProductCode = '" + Session.Item("localSubscription").crmProductCode + "',"
    '                sQuery = sQuery + " client_regJetnetCompanyID = " + Session.Item("localUser").crmUserCompanyID.ToString
    '                sQuery = sQuery + " WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
    '                sQuery = sQuery + "' and client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
    '                sQuery = sQuery + Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "'"
    '                sQuery = sQuery + " AND client_regType = 'C' AND client_regStatus = 'N'"

    '              Else
    '                ' if NOT in standalone mode connect to master database for data connections for this host 
    '                sQuery = "UPDATE Client_Register_Master SET client_regSubscriptionCode = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubscriptionCode) + "',"
    '                sQuery = sQuery + " client_regSecurityToken = '" + Session.Item("localUser").crmSecurityToken + "',"
    '                sQuery = sQuery + " client_regInstallDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubInstallDate) + "',"
    '                sQuery = sQuery + " client_regAccessDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubAccessDate) + "',"
    '                sQuery = sQuery + " client_webUserLimit = " + Session.Item("localSubscription").crmMaxUserCount.ToString + ","
    '                sQuery = sQuery + " client_webInstanceID = " + Application.Item("crmClientSiteData").crmWebInstanceID.ToString + ","
    '                sQuery = sQuery + " client_regStatus = 'Y',"
    '                If Session.Item("localSubscription").crmAerodexFlag Then
    '                  sQuery = sQuery + " client_regAerodexFlag = 'Y',"
    '                Else
    '                  sQuery = sQuery + " client_regAerodexFlag = 'N',"
    '                End If
    '                sQuery = sQuery + " client_regFrequency = '" + Session.Item("localSubscription").crmFrequency + "',"
    '                sQuery = sQuery + " client_regTierLevel = '" + Session.Item("localSubscription").crmTierlevel + "',"
    '                sQuery = sQuery + " client_regProductCode = '" + Session.Item("localSubscription").crmProductCode + "',"
    '                sQuery = sQuery + " client_regJetnetCompanyID = " + Session.Item("localUser").crmUserCompanyID.ToString
    '                sQuery = sQuery + " WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
    '                sQuery = sQuery + "' and client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
    '                sQuery = sQuery + Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "'"
    '                sQuery = sQuery + " AND client_regType = 'C' AND client_regStatus = 'N'"
    '              End If

    '              Select Case Session.Item("localSubscription").crmDataLayerType
    '                Case eDatalayerTypes.ACCESS
    '                  Dim AccessConn As New OleDb.OleDbConnection
    '                  Dim AccessCommand As New OleDb.OleDbCommand
    '                  Dim AccessException As OleDb.OleDbException : AccessException = Nothing

    '                  If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
    '                    AccessConn.ConnectionString = Application.Item("crmClientDatabase")
    '                  Else
    '                    AccessConn.ConnectionString = My.Settings.DEFAULT_LIVE_ACCESS.ToString
    '                  End If

    '                  Try

    '                    AccessConn.Open()

    '                    AccessCommand.Connection = AccessConn
    '                    AccessCommand.CommandType = CommandType.Text
    '                    AccessCommand.CommandTimeout = 60

    '                    AccessCommand.CommandText = sQuery
    '                    AccessCommand.ExecuteNonQuery()

    '                  Catch AccessException

    '                    AccessConn.Dispose()
    '                    AccessCommand.Dispose()

    '                    Me.FailureText1.Text = "Security Token Not Saved Properly to Database : " + AccessException.Message
    '                    Return False

    '                  Finally

    '                    AccessCommand.Dispose()
    '                    AccessConn.Close()
    '                    AccessConn.Dispose()

    '                  End Try


    '                Case eDatalayerTypes.MSSQL

    '                  Dim SqlConn As New SqlClient.SqlConnection
    '                  Dim SqlCommand As New SqlClient.SqlCommand
    '                  Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    '                  If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
    '                    SqlConn.ConnectionString = Application.Item("crmClientDatabase")
    '                  Else
    '                    If CBool(My.Settings.IsDebugMode) = True Then
    '                      SqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MSSQL_DEBUG.ToString
    '                    Else
    '                      SqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MSSQL.ToString
    '                    End If
    '                  End If

    '                  Try

    '                    SqlConn.Open()

    '                    SqlCommand.Connection = SqlConn
    '                    SqlCommand.CommandType = CommandType.Text
    '                    SqlCommand.CommandTimeout = 60

    '                    SqlCommand.CommandText = sQuery
    '                    SqlCommand.ExecuteNonQuery()


    '                  Catch SqlException

    '                    SqlConn.Dispose()
    '                    SqlCommand.Dispose()

    '                    Me.FailureText1.Text = "Security Token Not Saved Properly to Database : " + SqlException.Message
    '                    Return False

    '                  Finally

    '                    SqlCommand.Dispose()
    '                    SqlConn.Close()
    '                    SqlConn.Dispose()

    '                  End Try

    '                Case eDatalayerTypes.MYSQL

    '                  Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    '                  Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    '                  Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    '                  If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
    '                    MySqlConn.ConnectionString = Application.Item("crmClientDatabase")
    '                  Else
    '                    If CBool(My.Settings.IsDebugMode) = True Then
    '                      MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL_DEBUG.ToString
    '                    Else
    '                      MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL.ToString
    '                    End If
    '                  End If

    '                  Try

    '                    MySqlConn.Open()

    '                    MySqlCommand.Connection = MySqlConn
    '                    MySqlCommand.CommandType = CommandType.Text
    '                    MySqlCommand.CommandTimeout = 60

    '                    MySqlCommand.CommandText = sQuery
    '                    MySqlCommand.ExecuteNonQuery()


    '                  Catch MySqlException

    '                    MySqlConn.Dispose()
    '                    MySqlCommand.Dispose()

    '                    Me.FailureText1.Text = "Security Token Not Saved Properly to Database : " + MySqlException.Message
    '                    Return False

    '                  Finally

    '                    MySqlCommand.Dispose()
    '                    MySqlConn.Close()
    '                    MySqlConn.Dispose()

    '                  End Try

    '              End Select

    '            Case eLogonTypes.REGISTRY ' else use crmRegisterAssmbly.crmRegisterClass

    '          End Select

    '        Else

    '          Me.FailureText1.Text = objReturnStatus.Description
    '          Return False

    '        End If

    '      End If
    '    Else

    '      Me.FailureText1.Text = objReturnStatus.Description
    '      Return False

    '    End If

    '  End If

    'Catch ex As Exception

    '  Me.FailureText1.Text = "RegisterUser.ascx > Register_Client Error: " + ex.Message
    '  Return False

    'End Try

    Return True

  End Function

End Class