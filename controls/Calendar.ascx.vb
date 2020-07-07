Imports System.IO
Partial Public Class Calendar
    Inherits System.Web.UI.UserControl
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Public Event Changed_Date(ByVal ActionDate As String)
    Dim error_string As String = ""
#Region "Page Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.Visible Then
            Dim masterPage As main_site = DirectCast(Page.Master, main_site)
            Try
                If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                    Me.Visible = False
                End If

            Catch ex As Exception
                error_string = "Calendar.ascx.vb - Page Load() " & ex.Message
                masterPage.LogError(error_string)
            End Try
        End If

    End Sub
#End Region
#Region "Calendar Events"
    Public counter As Integer = 0
    Public Schedule_Table As DataTable
    Private Sub Calendar_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar.DayRender
        If Session.Item("crmUserLogon") = True Then
            If Session.Item("localUser").crmEvo <> True Then
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Try

                    Dim currently As String = CStr(e.Day.Date)
                    Dim onemonth As String = CStr(e.Day.Date)
                    counter = counter + 1
                    If counter = 1 Then
                        'currently = Month(DateAdd(DateInterval.Month, 1, e.Day.Date)) & "/1/" & e.Day.Date.Year
                        'onemonth = CStr(DateAdd(DateInterval.Month, 1, CDate(currently)))
                        ' Response.Write("touched database " & currently & " " & counter)
                        currently = Month(e.Day.Date) & "/1/" & Year(e.Day.Date)
                        currently = CStr(DateAdd(DateInterval.Month, 1, CDate(currently)))
                        onemonth = CStr(DateAdd(DateInterval.Month, 1, CDate(currently)))

                        Schedule_Table = masterPage.aclsData_Temp.Get_Local_Notes_Schedule_DateLimitedReturn(currently, onemonth, Session("timezone_offset"), "P")
                        'Response.Write(Schedule_Table.Rows(0).Item("lnote_status"))
                    End If

                    If Not IsNothing(Schedule_Table) Then
                        If Schedule_Table.Rows.Count > 0 Then
                            For Each r As DataRow In Schedule_Table.Rows
                                'If r("lnote_status") = "P" Then
                                If Not IsDBNull(r("lnote_schedule_start_date")) Then
                                    If Day(r("lnote_schedule_start_date")) = e.Day.Date.Day Then
                                        If Not IsNothing(Session.Item("localUser").crmLocalUserID) Then
                                            Dim hold_id As Integer = 0
                                            If Not IsDBNull(r("lnote_user_id")) Then
                                                hold_id = r("lnote_user_id")
                                            End If
                                            Dim z As Integer = Session.Item("localUser").crmLocalUserID
                                            If Session.Item("localUser").crmLocalUserID = hold_id Then
                                                e.Cell.Font.Bold = True
                                            End If
                                        End If
                                    End If
                                End If
                                'End If
                            Next
                        End If
                    Else
                        If masterPage.aclsData_Temp.class_error <> "" Then
                            error_string = masterPage.aclsData_Temp.class_error
                            masterPage.LogError("Calendar.ascx.vb - Calendar_DayRender() - " & error_string)
                        End If
                        masterPage.display_error()
                    End If
                Catch ex As Exception
                    error_string = "Calendar.ascx.vb - Calendar_SelectionChanged() " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
    Private Sub Calendar_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar.SelectionChanged
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
            Session.Item("DaySelected") = "true"
            RaiseEvent Changed_Date(Calendar.SelectedDate.ToString("MM/dd/yyyy"))
        Catch ex As Exception
            error_string = "Calendar.ascx.vb - Calendar_SelectionChanged() " & ex.Message
            MasterPage.LogError(error_string)
        End Try
    End Sub
#End Region
   
End Class