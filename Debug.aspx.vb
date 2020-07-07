Partial Public Class Debug
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'This is being protected from display for jetnet and mvintech accounts.
    'I added a third where clause with a check for a nonsense request variable just in case for whatever reason
    'We absolutely need to see this page on another account not mvintech/jetnet.
    If Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Or Trim(Request("unlocked")) = "@123Sesame" Then
      Dim database_display As Array = Split(Session.Item("jetnetClientDatabase").ToString, ";Password")
      Session.Item("localUser").crmUser_DebugText += "<br /><br />"
      If UBound(database_display) > 0 Then
        Session.Item("localUser").crmUser_DebugText += "<b>Jetnet Database Connection <em>Not displaying password</em></b>: " + database_display(0).ToString + "<br />"
      End If

      Dim database_display2 As Array = Split(Session.Item("jetnetAdminDatabase").ToString, ";Password")

      If UBound(database_display2) > 0 Then
        Session.Item("localUser").crmUser_DebugText += "<b>Jetnet ADMIN Database Connection <em>Not displaying password</em></b>: " + database_display2(0).ToString + "<br />"
      End If


      Response.Write("<h1>Debug Text</h1>" + Session.Item("localUser").crmUser_DebugText)
      Response.Write("<br /><br /><hr /><br /><br />")

      Response.Write("Server Notes: " + Session.Item("jetnetServerNotesDatabase").ToString + "<br /><br />")
      Response.Write("isEVOLOGGING: " + Session.Item("isEVOLOGGING").ToString + "<br /><br />")

      Response.Write("<br /><br /><hr /><br /><br />")
      Response.Write("<h2>LAST IN SESSION LISTING SQL QUERIES</h2><br /><br />")
      If Not IsNothing(Session.Item("MasterAircraft")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterAircraft")) Then
          Response.Write("Session.Item(""MasterAircraft"") = " + Session.Item("MasterAircraft").ToString + " (Applicable for History/Aircraft)<br /><br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterCompany")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterCompany")) Then
          Response.Write("Session.Item(""MasterCompany"") = " + Session.Item("MasterCompany").ToString + "<br /><br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterEvents")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterEvents")) Then
          Response.Write("Session.Item(""MasterEvents"") = " + Session.Item("MasterEvents").ToString + "<br /><br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterWanted")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterWanted")) Then
          Response.Write("Session.Item(""MasterWanted"") = " + Session.Item("MasterWanted").ToString + "<br /><br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterYacht")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterYacht")) Then
          Response.Write("Session.Item(""MasterYacht"") = " + Session.Item("MasterYacht").ToString + "<br /><br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterYachtEvents")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterYachtEvents")) Then
          Response.Write("Session.Item(""MasterYachtEvents"") = " + Session.Item("MasterYachtEvents").ToString + "<br /><br /><br />")
        End If
      End If
      Response.Write("<br /><br /><hr /><br /><br />")
      Response.Write("Session.Item(""MasterAircraftSelect"") = " + Session.Item("MasterAircraftSelect").ToString + "<br /><br />")
      Response.Write("Session.Item(""MasterAircraftFrom"") = " + Session.Item("MasterAircraftFrom").ToString + "<br /><br />")
      Response.Write("Session.Item(""MasterAircraftWhere"") = " + Session.Item("MasterAircraftWhere").ToString + "<br /><br />")
      Response.Write("Session.Item(""MasterAircraftSort"") = " + Session.Item("MasterAircraftSort").ToString + "<br /><br />")

      'Yacht Session Flags
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtSelect")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtSelect")) Then
          Response.Write("Session.Item(""MasterYachtSelect"") = " + Session.Item("MasterYachtSelect").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtFrom")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtFrom")) Then
          Response.Write("Session.Item(""MasterYachtFrom"") = " + Session.Item("MasterYachtFrom").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtWhere")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtWhere")) Then
          Response.Write("Session.Item(""MasterYachtWhere"") = " + Session.Item("MasterYachtWhere").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtSort")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtSort")) Then
          Response.Write("Session.Item(""MasterYachtSort"") = " + Session.Item("MasterYachtSort").ToString + "<br /><br />")
        End If
      End If

      'Yacht Event Session Flags
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtEventsWhere")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtEventsWhere")) Then
          Response.Write("Session.Item(""MasterYachtEventsWhere"") = " + Session.Item("MasterYachtEventsWhere").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(HttpContext.Current.Session.Item("MasterYachtEventsFrom")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterYachtEventsFrom")) Then
          Response.Write("Session.Item(""MasterYachtEventsFrom"") = " + Session.Item("MasterYachtEventsFrom").ToString + "<br /><br />")
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("MasterAircraftCompany")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("MasterAircraftCompany")) Then
          Response.Write("Session.Item(""MasterAircraftCompany"")  = " + Session.Item("MasterAircraftCompany").ToString + "<br /><br />")
        End If
      End If
      Response.Write("<br /><br /><hr /><br /><br />")
      If Not IsNothing(Session.Item("MasterCompanyFrom")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterCompanyFrom")) Then
          Response.Write("Session.Item(""MasterCompanyFrom"") = " + Session.Item("MasterCompanyFrom").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterCompanyWhere")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterCompanyWhere")) Then
          Response.Write("Session.Item(""MasterCompanyWhere"") = " + Session.Item("MasterCompanyWhere").ToString + "<br /><br />")
        End If
      End If
      Response.Write("<br /><br /><hr /><br /><br />")
      If Not IsNothing(Session.Item("MasterAircraftEventsWhere")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterAircraftEventsWhere")) Then
          Response.Write("Session.Item(""MasterAircraftEventsWhere"") = " + Session.Item("MasterAircraftEventsWhere").ToString + "<br /><br />")
        End If
      End If
      If Not IsNothing(Session.Item("MasterAircraftEventsFrom")) Then
        If Not String.IsNullOrEmpty(Session.Item("MasterAircraftEventsFrom")) Then
          Response.Write("Session.Item(""MasterAircraftEventsFrom"") = " + Session.Item("MasterAircraftEventsFrom").ToString + "<br /><br />")
        End If
      End If


      Dim I As Integer = 0
      Dim L As Integer = Session.Contents.Count
      Dim Value(L) As String
      Dim keyName(L) As String

      'How many session variables are there?
      Response.Write("<h4>There are <b>" & Session.Contents.Count & "</b> Session variables</h4>")

      Response.Write("<div class='content_padding six columns'><strong>Advanced Search Items:</strong><br />")
      'Use a For Each ... Next to loop through the entire collection
      For Each strName In Session.Contents
        'Is this session variable an array?
        If TypeOf Session.Contents(strName) Is String Then
          If InStr(strName, "Advanced-") > 0 Then
            'We aren't dealing with an array, so just display the variable
            Response.Write("Session.Item(""" & strName & """): " & Session.Contents(strName) & "<br />")
          End If
        End If
      Next


            Response.Write("</div><br /><br />")


      Response.Write("Session.Item(""useFAAFlightData"") As String: " + Session.Item("useFAAFlightData").ToString + "<br /><br />")

      Response.Write("<div class='content_padding six columns'><strong>User Information:</strong><br />" + Session.Item("localUser").DisplayUser() + "</div><br /><br />")
      Response.Write("<div class='content_padding six columns'><strong>Subscription Information:</strong><br />" + Session.Item("localSubscription").DisplaySubscription() + "</div><br /><br />")
      Response.Write("<div class='content_padding six columns'><strong>Preferences Information:</strong><br />" + Session.Item("localPreferences").DisplayPreferences() + "</div><br /><br />")
            Response.Write("<div class='content_padding six columns'><strong>Search Information:</strong><br />" + Session.Item("searchCriteria").DisplaySearchClass() + "</div><br /><br />")
            Response.Write("<div class='content_padding six columns'><strong>Homebase User Class Information:</strong><br />" + Session.Item("homebaseUserClass").DisplayHomeBaseClass() + "</div><br /><br />")
        Else
      Response.Redirect("default.aspx")
    End If
  End Sub

End Class