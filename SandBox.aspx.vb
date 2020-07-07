Partial Public Class sandbox
  Inherits System.Web.UI.Page
  Dim aTempTable As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Private Sub sandbox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try

      If clsGeneral.clsGeneral.Upcoming_ActionItems(action_items, Master, Nothing, FormatDateTime(DateAdd(DateInterval.Day, 7, Now()), 2)) = 0 Then
        'don't show, nothing here by default.
        action_items.Visible = False
        main_menu.Visible = True
        today_date.Visible = False
        display_table.Visible = False
      Else
        'show!            
        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        today_date.Text = weekdis & ", " & monthdis & " " & daydis & ", " & yeardis
        action_items.Visible = True
        main_menu.Visible = False
      End If


    Catch ex As Exception
      error_string = "sandbox.aspx.vb - Page Load()  - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try


  End Sub





  Private Sub skip_main_menu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles skip_main_menu.Click
    If skip_main_menu.Text = "Skip to Main Menu" Then
      action_items.Visible = False
      main_menu.Visible = True
      skip_main_menu.Text = "View Action Items"
    Else
      skip_main_menu.Text = "Skip to Main Menu"
      action_items.Visible = True
      main_menu.Visible = False

    End If
  End Sub
End Class