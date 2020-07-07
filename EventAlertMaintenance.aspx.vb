
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/EventAlertMaintenance.aspx.vb $
'$$Author: Matt $
'$$Date: 6/02/20 12:34p $
'$$Modtime: 6/02/20 10:41a $
'$$Revision: 9 $
'$$Workfile: EventAlertMaintenance.aspx.vb $
'
' ********************************************************************************

Public Class EventAlertMaintenance

    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.SetContainerClass("container MaxWidthRemove")
        Master.SetPageTitle("Schedule Event Alert")
        Dim temp_text As String = ""

        If Not IsNothing(HttpContext.Current.Session.Item("SearchString")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("SearchString")) Then
                'Regex replace Type string on search filter
                Dim strReplacement As String = ""
                Dim strPattern As String = "^Type[\W\w]+?<br \/>"
                Dim rgx As Regex = New Regex(strPattern)
                Dim strResult As String = rgx.Replace(HttpContext.Current.Session.Item("SearchString"), strReplacement)
                'Regex replace Airframe Type string on search filter
                strPattern = "Airframe Type[\W\w]+?<br \/>"
                rgx = New Regex(strPattern)
                strResult = rgx.Replace(strResult, strReplacement)
                'Regex replace Start Date string on search filter
                strPattern = "Start Date[\W\w]+?<br \/>"
                rgx = New Regex(strPattern)
                strResult = rgx.Replace(strResult, strReplacement)
                'Regex replace End date string on search filter
                strPattern = "End Date[\W\w]+?<br \/>"
                rgx = New Regex(strPattern)
                strResult = rgx.Replace(strResult, strReplacement)
                searchFilterText.Text = strResult

            End If
        End If


        If Not Page.IsPostBack Then
            '1.	Default Schedule – If the user is live – which all are right now, then default to live clicked.  
            'If easy To plan For, make it so If weekly Then Default To weekly And monthly Default To monthly since I anticipate JETNET 
            'deciding To go this direction soon.

            reoccurence_radio.SelectedValue = UCase(Session.Item("localSubscription").crmFrequency)
            reoccurence_radio_SelectedIndexChanged(reoccurence_radio, EventArgs.Empty)

            cfolder_jetnet_run_reply_username.Text = Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName
            cfolder_jetnet_run_reply_username.Enabled = False
            cfolder_jetnet_run_reply_email.Text = Session.Item("localUser").crmLocalUserEmailAddress
            cfolder_jetnet_run_reply_email.Enabled = False


            If Trim(Request("id")) <> "" And Trim(Request("id")) <> "0" Then
                Dim data1 As New DataTable
                Dim aclsData_Manager_SQL As New clsData_Manager_SQL


                Try


                    aclsData_Manager_SQL.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")

                    data1 = aclsData_Manager_SQL.GetEvolutionFolders(Trim(Request("id")))

                    If Not IsNothing(data1) Then
                        If data1.Rows.Count > 0 Then
                            For Each r As DataRow In data1.Rows
                                cfolder_name.Text = r.Item("cfolder_name")
                                cfolder_description.Text = r.Item("cfolder_description")


                                ' cfolder_description.Text = r.Item("cfolder_schedule_run_date")
                                '  cfolder_description.Text = r.Item("cfolder_schedule_type")
                                '  cfolder_description.Text = r.Item("cfolder_schedule_day")
                                '  cfolder_description.Text = r.Item("cfolder_schedule_hour")


                                If Trim(r.Item("cfolder_schedule_type")) = "LIVE" Then
                                    reoccurence_radio.SelectedIndex = 0
                                ElseIf Trim(r.Item("cfolder_schedule_type")) = "DAILY" Then
                                    reoccurence_radio.SelectedIndex = 1
                                ElseIf Trim(r.Item("cfolder_schedule_type")) = "WEEKLY" Then
                                    reoccurence_radio.SelectedIndex = 2
                                ElseIf Trim(r.Item("cfolder_schedule_type")) = "MONTHLY" Then
                                    reoccurence_radio.SelectedIndex = 3
                                End If

                                daily_toggle.Visible = False
                                weekly_toggle.Visible = False
                                live_toggle.Visible = False
                                monthly_toggle.Visible = False

                                Select Case UCase(reoccurence_radio.SelectedValue)
                                    Case "DAILY"
                                        daily_toggle.Visible = True

                                        temp_text = r.Item("cfolder_schedule_hour")

                                        daily_time_hour.SelectedIndex = temp_text

                                        '  daily_next_run.Text = Date.Now.Date() & " " & daily_time_hour.SelectedItem.ToString
                                        '    daily_next_run.Text = 
                                        Call FigureOutDateTime(daily_time_hour, daily_next_run, False)

                                    Case "WEEKLY"
                                        weekly_toggle.Visible = True

                                        weekly_time_hour.SelectedValue = r.Item("cfolder_schedule_hour")

                                        weekly_time_day.SelectedValue = r.Item("cfolder_schedule_day")

                                        '    weekly_next_run.Text = 
                                        Call FigureOutDateTime(weekly_time_hour, weekly_next_run, True)
                                    Case "MONTHLY"
                                        monthly_toggle.Visible = True

                                        monthly_time_hour.SelectedValue = r.Item("cfolder_schedule_hour")
                                        monthly_day_of_month.SelectedValue = r.Item("cfolder_schedule_day")

                                        'get next month 
                                        monthly_next_run.Text = (Month(Date.Now()) + 1) & "/" & monthly_day_of_month.SelectedValue & "/" & Year(DateAdd(DateInterval.Month, 1, Date.Now()))


                                    Case Else
                                        live_toggle.Visible = True
                                        live_toggle_next_run.Text = DateAdd(DateInterval.Minute, 15, Now())
                                End Select

                            Next
                        End If
                    End If

                Catch ex As Exception

                End Try
            End If

        End If

    End Sub
    Public Sub checkEmail(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        If cfolder_jetnet_run_reply_email.Text = "" Then
            args.IsValid = False
            Exit Sub
        End If

        args.IsValid = True
    End Sub

    Private Sub reoccurence_radio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles reoccurence_radio.SelectedIndexChanged
        daily_toggle.Visible = False
        weekly_toggle.Visible = False
        live_toggle.Visible = False
        monthly_toggle.visible = False
        Select Case UCase(reoccurence_radio.SelectedValue)
            Case "DAILY"
                daily_toggle.Visible = True
                daily_time_hour.SelectedValue = (Hour(Now()))
                Dim modifiedDate As Date = Now()
                modifiedDate = DateAdd(DateInterval.Day, 1, modifiedDate)
                modifiedDate = DateAdd(DateInterval.Second, -Second(Now), modifiedDate)
                daily_next_run.Text = DateAdd(DateInterval.Minute, -Minute(Now), modifiedDate)
            Case "WEEKLY"
                weekly_toggle.Visible = True
                weekly_time_hour.SelectedValue = (Hour(Now()))
                weekly_time_day.SelectedValue = Weekday(Now()) - 1
                Dim modifiedDate As Date = Now()
                modifiedDate = DateAdd(DateInterval.WeekOfYear, 1, modifiedDate)
                modifiedDate = DateAdd(DateInterval.Second, -Second(Now), modifiedDate)
                weekly_next_run.Text = DateAdd(DateInterval.Minute, -Minute(Now), modifiedDate)
            Case "MONTHLY"
                monthly_toggle.Visible = True

                monthly_time_hour.SelectedValue = (Hour(DateAdd(DateInterval.Hour, 1, Now())))
                monthly_day_of_month.SelectedValue = Day(DateAdd(DateInterval.Hour, 1, Now()))
                monthly_next_run.Text = Month(DateAdd(DateInterval.Hour, 1, Now())) & "/" & Day(DateAdd(DateInterval.Hour, 1, Now())) & "/" & Year(DateAdd(DateInterval.Hour, 1, Now())) & " " & Replace(Replace(monthly_time_hour.SelectedItem.Text, " AM", ""), " PM", "") & ":00:00 " & DateAndTime.DateAdd(DateInterval.Hour, 1, Now()).ToString("tt", Globalization.CultureInfo.InvariantCulture)

            Case Else
                live_toggle.Visible = True
                live_toggle_next_run.Text = DateAdd(DateInterval.Minute, 15, Now())
        End Select
    End Sub

    Private Sub daily_time_hour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles daily_time_hour.SelectedIndexChanged
        FigureOutDateTime(daily_time_hour, daily_next_run, False)
    End Sub



    Private Sub FigureOutDateTime(timeHour As DropDownList, nextDailyRun As Label, considerWeek As Boolean)
        Dim modifiedDate As Date = Now()
        modifiedDate = DateAdd(DateInterval.Day, 1, modifiedDate)
        modifiedDate = DateAdd(DateInterval.Second, -Second(Now), modifiedDate)
        daily_next_run.Text = DateAdd(DateInterval.Minute, -Minute(Now), modifiedDate)
        Dim splitHour As String() = IIf(InStr(timeHour.SelectedItem.Text, " AM") > 0, Split(timeHour.SelectedItem.Text, " "), Split(timeHour.SelectedItem.Text, " "))

        If UBound(splitHour) = 1 Then
            Dim passedDate As New Date
            If considerWeek = False Then
                passedDate = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)

                Dim ts As TimeSpan = Now.Subtract(passedDate)

                If ts.Ticks <= 0 Then
                    nextDailyRun.Text = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                Else
                    nextDailyRun.Text = Month(Now()) & "/" & Day(DateAdd(DateInterval.Day, 1, Now())) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                End If

            Else
                passedDate = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                Dim ts As TimeSpan = Now.Subtract(passedDate)


                If ts.Ticks <= 0 Then
                    passedDate = Month(Now()) & "/" & Day(GetNextWeekday(Now(), weekly_time_day.SelectedValue)) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                Else
                    nextDailyRun.Text = Month(Now()) & "/" & Day(DateAdd(DateInterval.Day, 1, Now())) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                    passedDate = Month(Now()) & "/" & Day(GetNextWeekday(DateTime.Today.AddDays(1), weekly_time_day.SelectedValue)) & "/" & Year(Now()) & " " & splitHour(0) & ":00:00 " & splitHour(1)
                    '
                End If



                nextDailyRun.Text = passedDate
            End If


        End If
    End Sub

    Private Sub weekly_time_hour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles weekly_time_hour.SelectedIndexChanged, weekly_time_day.SelectedIndexChanged
        FigureOutDateTime(weekly_time_hour, weekly_next_run, True)
    End Sub

    Public Function GetNextWeekday(start As DateTime, day As DayOfWeek) As DateTime

        Dim daysToAdd As Integer = 0
        daysToAdd = (Int(day) - Int(start.DayOfWeek) + 7) Mod 7

        Return start.AddDays(daysToAdd)

    End Function

    Private Sub saveAndSchedule_Click(sender As Object, e As EventArgs) Handles saveAndSchedule.Click
        Dim cfolder_schedule_run_date As New Date 'This will be filled In With the Next run Date For the Event report based On the other selections below. 
        Dim cfolder_schedule_type As String = "Live" 'This will indicate whether the schedule For the Event Is Live, Hourly, Daily, Weekly, Or Monthly.  At present, these will be the only options.

        'This will be a number that means a different thing depending On the schedule type. Examples include the day field will be set To 0 the day would be set To 1.
        'For Weekly – the day field will indicate the day Of the week that the Event should be scheduled On 1-7, meaning Monday thru Sunday.
        'For Monthly – the day field will represent the day of the month starting with the first day of the month (1-31).  It Is Not recommended that day 31 Not be used since it does Not work for every month.
        Dim cfolder_schedule_day As Integer = 0

        Dim cfolder_schedule_hour As Integer = 12 ' Entered As a time (AM/PM), but stored As a number from 1 To 24. 
        Dim aclsData_Manager_SQL As New clsData_Manager_SQL


        Try
            aclsData_Manager_SQL.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")

            cfolder_schedule_type = UCase(reoccurence_radio.SelectedValue)
            Select Case UCase(reoccurence_radio.SelectedValue)
                Case "LIVE"
                    cfolder_schedule_run_date = live_toggle_next_run.Text
                    cfolder_schedule_day = 0
                    cfolder_schedule_hour = 12
                Case "WEEKLY"
                    cfolder_schedule_run_date = weekly_next_run.Text
                    cfolder_schedule_day = weekly_time_day.SelectedValue
                    cfolder_schedule_hour = weekly_time_hour.SelectedValue
                Case "DAILY"
                    cfolder_schedule_run_date = daily_next_run.Text
                    cfolder_schedule_day = 0
                    cfolder_schedule_hour = daily_time_hour.SelectedValue
                Case "MONTHLY"
                    cfolder_schedule_run_date = monthly_next_run.Text
                    cfolder_schedule_day = monthly_day_of_month.SelectedValue
                    cfolder_schedule_hour = monthly_time_hour.SelectedValue
            End Select

            Page.Validate()

            If Page.IsValid = False Then
                ' dont know how to turn it on to say it needs to show 
                If RequiredFieldValidator1.IsValid = False Then
                    RequiredFieldValidator1.ErrorMessage = True  ' doesnt work, doesnt show any validating yet 
                End If
            Else
                If Trim(Request("id")) <> "" And Trim(Request("id")) <> "0" Then
                    Call aclsData_Manager_SQL.Edit_Fields_Evolution_Folders("", "N", cfolder_name.Text, "N", cfolder_description.Text, Trim(Request("id")), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", "", "", "", 0, "", False, cfolder_schedule_run_date, cfolder_schedule_type, cfolder_schedule_day, cfolder_schedule_hour)
                Else
                    Call aclsData_Manager_SQL.Insert_Into_Evolution_Folders(5, "N", cfolder_name.Text, "N", "A", cfolder_description.Text, "", HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, "", "", "", "", 0, "", False, cfolder_schedule_run_date, cfolder_schedule_type, cfolder_schedule_day, cfolder_schedule_hour)
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub




End Class