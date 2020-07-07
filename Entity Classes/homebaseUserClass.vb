' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/homebaseUserClass.vb $
'$$Author: Amanda $
'$$Date: 11/21/19 3:02p $
'$$Modtime: 11/20/19 2:59p $
'$$Revision: 3 $
'$$Workfile: homebaseUserClass.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class homebaseUserClass
    Private _home_user_id As String = ""
    Private _home_account_id As String = ""
    Private _home_user_type As String = ""
    Public Property home_user_id() As String
        Get
            Return _home_user_id
        End Get
        Set(ByVal value As String)
            _home_user_id = value
        End Set
    End Property

    Public Property home_account_id() As String
        Get
            Return _home_account_id
        End Get
        Set(ByVal value As String)
            _home_account_id = value
        End Set
    End Property

    Public Property home_user_type() As String
        Get
            Return _home_user_type
        End Get
        Set(ByVal value As String)
            _home_user_type = value
        End Set
    End Property


    Public Sub New(homeUserId As String, homeAccountId As String, homeUserType As String)

        Try

            _home_user_id = homeUserId
            _home_account_id = homeAccountId
            _home_user_type = homeUserType

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        End Try

    End Sub

    '•	Home_user_id – initials of user used to put on all types of records indicating who Is responsible. 
    '•	Home_account_id – 4 char code of the account rep
    '•	Home_user_type – String indicating if user Is a research manager, administrator, etc.

    Public Function DisplayHomeBaseClass() As String

        Dim sOutputString = New StringBuilder()

        sOutputString.Append("Session.Item(""homebaseUserClass"").home_user_id As string: " + _home_user_id.ToString + "<br />")
        sOutputString.Append("Session.Item(""homebaseUserClass"").home_account_id As string: " + _home_account_id.ToString + "<br />")
        sOutputString.Append("Session.Item(""homebaseUserClass"").home_user_type As string: " + home_user_type + "<br />")

        Return sOutputString.ToString
    End Function
    Public Sub New()

        Try

            _home_user_id = ""
            _home_account_id = ""
            _home_user_type = ""

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        End Try

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim [class] = TryCast(obj, homebaseUserClass)
        Return [class] IsNot Nothing AndAlso
               home_user_id = [class].home_user_id AndAlso
               home_account_id = [class].home_account_id AndAlso
               home_user_type = [class].home_user_type
    End Function

    Public Shared Operator =(class1 As homebaseUserClass, class2 As homebaseUserClass) As Boolean
        Return EqualityComparer(Of homebaseUserClass).Default.Equals(class1, class2)
    End Operator

    Public Shared Operator <>(class1 As homebaseUserClass, class2 As homebaseUserClass) As Boolean
        Return Not class1 = class2
    End Operator

End Class
