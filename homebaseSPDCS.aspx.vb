' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebaseSPDCS.aspx.vb $
'$$Author: Mike $
'$$Date: 7/19/19 10:24a $
'$$Modtime: 7/19/19 6:01a $
'$$Revision: 3 $
'$$Workfile: homebaseSPDCS.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseSPDCS
  Inherits System.Web.UI.Page

  Private sTask As String = ""
  Public Shared masterPage As New Object


  Private Sub Homebase_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try
      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then 
        Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.master"
        masterPage = DirectCast(Page.Master, HomebaseTheme)


      Else
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
      End If

    Catch ex As Exception
      Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
      masterPage = DirectCast(Page.Master, EvoTheme)
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                            HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then

        Response.Redirect("Default.aspx", True)
      End If

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        'masterPage.Set_Active_Tab(4)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin SPD Collection Summary - Home")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        masterPage.Set_Active_Tab(9)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase SPD Collection Summary - Home")
      End If

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sTask = Request.Item("task").ToString.ToUpper.Trim
        End If
      End If

      'If IsPostBack And sTask.ToLower.Contains("run") And Not String.IsNullOrEmpty(reg_no.Text.Trim) Then

      'Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(reg_no.Text.Trim + " - Homebase Flight Research - " + WeekdayName(Weekday(Today)).ToString + ", " + MonthName(Month(Today)).ToString + " " + Day(Today).ToString + ", " + Year(Today).ToString)

      SPDCSDetailsLbl.Text = "<a href='homebaseSPDCS.aspx?original=Y'>Show Original</a><Br><br>"

      If Trim(Request("original")) = "Y" Then
        SPDCSDetailsLbl.Text += generateSPDCS_Original()
      Else
        SPDCSDetailsLbl.Text += generateSPDCS()
      End If



      'End If

    End If

  End Sub

  Public Function getSPDCSDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()
    Dim sQuery2 = New StringBuilder()
    Dim sQuery3 = New StringBuilder()
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing 'sqlrep_level = 'JETNET' 

    Try


      sQuery2.Append(" and ( ")

      sQuery2.Append(" (journ_date > ")
      sQuery2.Append(" (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
      sQuery2.Append(" Where af2.ac_id = Aircraft.ac_id ")
      sQuery2.Append(" and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
      sQuery2.Append(" order by af2.journ_date asc))")

      sQuery2.Append(" or  ")

      sQuery2.Append(" (journ_date = ")
      sQuery2.Append(" (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
      sQuery2.Append(" Where af2.ac_id =  Aircraft.ac_id and  Aircraft.ac_journ_id  >  af2.ac_journ_id ")
      sQuery2.Append(" and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
      sQuery2.Append(" order by af2.journ_date asc))")

      sQuery2.Append(" ) ")


      sQuery3.Append(" and ( ")

      sQuery3.Append(" (journ_date > ")
      sQuery3.Append(" (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
      sQuery3.Append(" Where af2.ac_id = journal.journ_ac_id ")
      sQuery3.Append(" and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
      sQuery3.Append(" order by af2.journ_date asc))")

      sQuery3.Append(" or  ")

      sQuery3.Append(" (journ_date = ")
      sQuery3.Append(" (select top 1 journ_date from View_Aircraft_History_Flat af2 with (NOLOCK) ")
      sQuery3.Append(" Where af2.ac_id =  journal.journ_ac_id and  journal.journ_id  >  af2.ac_journ_id ")
      sQuery3.Append(" and (af2.journ_newac_flag = 'Y' or ac_previously_owned_flag = 'Y') ")
      sQuery3.Append(" order by af2.journ_date asc))")

      sQuery3.Append(" ) ")





      sQuery.Append("SELECT top 250 amod_class_code as class_code, amod_weight_class as WEIGHT, amod_make_name as MAKE, amod_model_name AS MODEL, COUNT(*) AS CLICKS,")

      sQuery.Append(" (SELECT COUNT(*) FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = journ_id AND ac_id = journ_ac_id")
      sQuery.Append(" AND journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') AND c.amod_id = ac_amod_id AND YEAR(journ_date) = " & Year(Date.Now) & " ")

      sQuery.Append(sQuery2.ToString)

      sQuery.Append(" ) AS TRANSYTD,")




      sQuery.Append(" (SELECT COUNT(*) FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = journ_id AND ac_id = journ_ac_id")
      sQuery.Append(" AND journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') AND c.amod_id = ac_amod_id AND journ_date >= '" & DateAdd(DateInterval.Month, -6, Date.Now) & "'")

      sQuery.Append(sQuery2.ToString)

      sQuery.Append(" ) AS TRANS_LAST_6_MONTHS,")


      sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      sQuery.Append(" WHERE acval_amod_id = c.amod_id ")
      sQuery.Append(" AND journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')")

      sQuery.Append(" and journ_newac_flag = 'N' AND journ_internal_trans_flag = 'N' ")
      sQuery.Append(sQuery3.ToString)

      sQuery.Append(" AND journ_date >= '" & DateAdd(DateInterval.Month, -6, Date.Now) & "') AS  PRICES_LAST_6_MONTHS ")


      '  sQuery.Append(" (SELECT COUNT(*) FROM Journal WITH(NOLOCK)")
      '  sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = journ_id AND ac_id = journ_ac_id")
      '  sQuery.Append(" and journ_subcat_code_part1='WS' AND c.amod_id = ac_amod_id AND YEAR(journ_date) = 2015 ) AS TRANS2015,")

      ' sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      '  sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      ' sQuery.Append(" WHERE acval_amod_id = c.amod_id AND YEAR(journ_date) = 2015) AS PRICES2015,")

      ' sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      '  sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      '  sQuery.Append(" WHERE acval_amod_id = c.amod_id AND YEAR(journ_date) < 2015) AS PRICEPRIOR,")

      '  sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      '  sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      '  sQuery.Append(" WHERE acval_amod_id = c.amod_id ) AS PRICETOT")

      sQuery.Append(" FROM Subscription_Install_Log a WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft b WITH(NOLOCK) ON subislog_ac_id = ac_id AND ac_journ_id = 0")
      sQuery.Append(" INNER JOIN Aircraft_Model c WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE subislog_amod_id > 0 AND subislog_msg_type = 'UserStatistics'")
      sQuery.Append(" AND subislog_subid NOT IN (SELECT DISTINCT sub_id FROM Subscription WHERE sub_comp_id = 135887)")


      If Me.show_15_year.Checked = True Then
        sQuery.Append(" and (amod_end_year = '' or amod_end_year > 2001) and amod_start_year <> '' ")
      End If 

      If Trim(Request("date_search")) <> "" Then
        sQuery.Append(" AND subislog_date >= '" & Trim(Request("date_search")) & "' ")
      Else
        sQuery.Append(" AND subislog_date >= '1/1/" & Year(Now()) & "' ")
      End If

      If Trim(ddl_model_type.SelectedValue.ToString) <> "All" Then
        sQuery.Append(" AND amod_type_code IN ('" & ddl_model_type.SelectedValue.ToString & "') ")
      End If 

      sQuery.Append(" and (b.ac_product_business_flag = 'Y') ")
      sQuery.Append(" GROUP BY amod_class_code, amod_weight_class, amod_make_name, amod_model_name, amod_id")


      If Trim(Request("order_by")) = "class" Then
        sQuery.Append(" ORDER BY amod_class_code, COUNT(*) DESC")
      ElseIf Trim(Request("order_by")) = "weight" Then
        sQuery.Append(" ORDER BY amod_class_code, COUNT(*) DESC")
      ElseIf Trim(Request("order_by")) = "make_model" Then
        sQuery.Append(" ORDER BY amod_make_name, amod_model_name, COUNT(*) DESC")
      ElseIf Trim(Request("order_by")) = "clicks" Then
        sQuery.Append(" ORDER BY COUNT(*) DESC")
      Else
        sQuery.Append(" ORDER BY amod_class_code, COUNT(*) DESC")
      End If


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getSPDCSDataTable() As DataTable</b><br />" + sQuery.ToString

      Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

      If Not useBackupSQL Then
        SqlConn.ConnectionString = My.Settings.TEST_LOCAL_MSSQL
      Else
        SqlConn.ConnectionString = My.Settings.TEST_LOCAL_MSSQL_BK
      End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getSPDCSDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getSPDCSDataTable() As DataTable</b><br />" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Function generateSPDCS() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim htmlOut_return As New StringBuilder
    Dim percent_have As Double = 0.0
    Dim percent_good As Double = 30.0
    Dim post_models As String = ""
    Dim post_models_replace As String = ""
    Dim temp_count As Integer = 0
    Dim total_good As Integer = 0
    Dim total_percent As Double = 0.0
    Dim show_row As Boolean = False

    Try

      post_models = ",   Challenger 604 , Challenger 605 , Challenger 650 , Citation Sovereign , Citation Sovereign+ , Citation X , Citation X+ , Citation XLS  , "
      post_models &= " Citation XLS+ , Falcon 2000 , Falcon 2000EX EASy  , Falcon 2000S , Falcon 50 , Falcon 50EX , Falcon 7X , Falcon 900B , "
      post_models &= " Falcon 900C  ,  Falcon 900DX , Falcon 900EX , Falcon 900EX EASy , Global 5000 , Global 6000 , Global Express , Global Express XRS  , "
      post_models &= " Gulfstream G-150 , Gulfstream G-200 , Gulfstream G-280 , Gulfstream G-350 ,  "
      post_models &= " Gulfstream G-450 , Gulfstream G-500 , Gulfstream G-550 , Gulfstream G-650 ,  "
      post_models &= " Gulfstream G-IV , Gulfstream G-IVSP , Gulfstream G-V , Hawker 4000 ,  "
      post_models &= " Hawker 800XP , Hawker 850XP , Hawker 900XP , "
      post_models &= " Learjet 40  ,  Learjet 45 , Learjet 45XR , Learjet 60 , Learjet 60XR   ,  Learjet 70 , Learjet 75    ."
      post_models = UCase(post_models)
      post_models_replace = post_models

      If Trim(Me.percent_good.Text) <> "" Then
        percent_good = CDbl(Me.percent_good.Text)
      End If

      If Trim(Request("order_by")) <> "" And Not IsPostBack Then
 
        If Trim(Request("percent_good")) <> "" Then
          Me.percent_good.Text = Trim(Request("percent_good"))
          percent_good = Trim(Request("percent_good"))
        End If 

        If Trim(Request("show_15")) <> "" Then
          Me.show_15_year.Checked = Trim(Request("show_15"))
        End If
 
        If Trim(Request("ddl_is_good")) <> "" Then
          Me.ddl_is_good.SelectedValue = Trim(Request("ddl_is_good"))
        End If

        If Trim(Request("ddl_model_type")) <> "" And Trim(Request("ddl_model_type")) <> "All" Then
           ddl_model_type.SelectedValue = Trim(Request("ddl_model_type"))
        ElseIf Trim(Request("ddl_model_type")) <> "All" Then ' if its not all, default to Jets
          ddl_model_type.SelectedValue = "J"
        Else
          ddl_model_type.SelectedValue = "All"
        End If 

        Dim Filtered_DV As New DataView(HttpContext.Current.Session.Item("SP_Table"))

        If Trim(Request("order_by")) = "class" Then
          Filtered_DV.Sort = " class_code asc, CLICKS DESC"
        ElseIf Trim(Request("order_by")) = "weight" Then
          Filtered_DV.Sort = " WEIGHT asc, CLICKS DESC"
        ElseIf Trim(Request("order_by")) = "make_model" Then
          Filtered_DV.Sort = " MAKE asc, MODEL, CLICKS DESC"
        ElseIf Trim(Request("order_by")) = "clicks" Then
          Filtered_DV.Sort = " CLICKS DESC"
        Else
          Filtered_DV.Sort = " class_code, CLICKS DESC"
        End If

        HttpContext.Current.Session.Item("SP_Table") = Filtered_DV.ToTable

      Else
        Session("SP_Table") = getSPDCSDataTable()
      End If

      results_table = Session("SP_Table")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Sale Price Data Collection Summary</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"" valign='bottom'>")
          htmlOut.Append("<td align=""left"">ROW</td>")
          htmlOut.Append("<td align=""left""><a href='homebaseSPDCS.aspx?order_by=class&percent_good=" & Me.percent_good.Text & "&show_15=" & Me.show_15_year.Checked & "&ddl_is_good= " & Me.ddl_is_good.SelectedValue & "&ddl_model_type=" & Me.ddl_model_type.SelectedValue.ToString & "'>CLASS</a></td>")
          htmlOut.Append("<td align=""left""><a href='homebaseSPDCS.aspx?order_by=weight&percent_good=" & Me.percent_good.Text & "&show_15=" & Me.show_15_year.Checked & "&ddl_is_good= " & Me.ddl_is_good.SelectedValue & "&ddl_model_type=" & Me.ddl_model_type.SelectedValue.ToString & "'>WEIGHT</a></td>")
          htmlOut.Append("<td align=""left""><a href='homebaseSPDCS.aspx?order_by=acpost&percent_good=" & Me.percent_good.Text & "&show_15=" & Me.show_15_year.Checked & "&ddl_is_good= " & Me.ddl_is_good.SelectedValue & "&ddl_model_type=" & Me.ddl_model_type.SelectedValue.ToString & "' title='Aircraft Post' tag='Aircraft Post' name='Aircraft Post'>AP</a></td>")
          htmlOut.Append("<td align=""left""><a href='homebaseSPDCS.aspx?order_by=make_model&percent_good=" & Me.percent_good.Text & "&show_15=" & Me.show_15_year.Checked & "&ddl_is_good= " & Me.ddl_is_good.SelectedValue & "&ddl_model_type=" & Me.ddl_model_type.SelectedValue.ToString & "'>MAKE/MODEL</a></td>")
          htmlOut.Append("<td align=""left""><a href='homebaseSPDCS.aspx?order_by=clicks&percent_good=" & Me.percent_good.Text & "&show_15=" & Me.show_15_year.Checked & "&ddl_is_good= " & Me.ddl_is_good.SelectedValue & "&ddl_model_type=" & Me.ddl_model_type.SelectedValue.ToString & "'>CLICKS</a></td>")
          htmlOut.Append("<td align=""left"">TRANS<br/>YTD</td>")
          htmlOut.Append("<td align=""left"">TRANS<br/>6 MONTHS</td>")
          htmlOut.Append("<td align=""left"">PRICES<br/>6 MONTHS</td>")
          htmlOut.Append("<td align=""left"">PERCENT</td>")
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows
            percent_have = 0.0


            If Not IsDBNull(r.Item("PRICES_LAST_6_MONTHS")) Then
              If r.Item("PRICES_LAST_6_MONTHS") <> 0 Then
                percent_have = (r.Item("PRICES_LAST_6_MONTHS") / r.Item("TRANS_LAST_6_MONTHS"))
                percent_have = FormatNumber((percent_have * 100), 0)
              End If
            End If

            show_row = False
            If ddl_is_good.SelectedValue = "Y" Then
              If percent_have >= percent_good Then
                show_row = True
              End If
            ElseIf ddl_is_good.SelectedValue = "N" Then
              If percent_have >= percent_good Then
              Else
                show_row = True
              End If
            Else
              show_row = True
            End If

            If show_row = True Then
              If percent_have >= percent_good Then
                htmlOut.Append("<tr bgcolor='#b3ff99'>")
                total_good = total_good + 1
              Else
                htmlOut.Append("<tr bgcolor='#ffcccc'>")
              End If

              temp_count = temp_count + 1
              htmlOut.Append("<td align=""left"" valign=""top"">" & temp_count & "</td>")

              If Not IsDBNull(r.Item("class_code")) Then
                htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("class_code") & "</td>")
              End If

              If Not IsDBNull(r.Item("WEIGHT")) Then
                htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("WEIGHT") & "</td>")
              End If

              If Not IsDBNull(r.Item("MAKE")) Then

                If InStr(Trim(post_models), " " & Trim(r.Item("MAKE")) & " " & Trim(r.Item("MODEL")) & " ") > 0 Then
                  post_models_replace = Replace(Trim(post_models_replace), " " & Trim(r.Item("MAKE")) & " " & Trim(r.Item("MODEL")) & " ", "")
                  htmlOut.Append("<td align=""right"" valign=""top"">*&nbsp;</td>")
                  htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("MAKE") & " / " & r.Item("MODEL") & "&nbsp;</td>")
                Else
                  htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
                  htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("MAKE") & " / " & r.Item("MODEL") & "&nbsp;</td>")
                End If


              End If

              If Not IsDBNull(r.Item("CLICKS")) Then
                htmlOut.Append("<td align=""right"" valign=""top"">" & FormatNumber(r.Item("CLICKS"), 0) & "&nbsp;</td>")
              Else
                htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
              End If

              If Not IsDBNull(r.Item("TRANSYTD")) Then
                htmlOut.Append("<td align=""right"" valign=""top"">" & r.Item("TRANSYTD") & "&nbsp;</td>")
              Else
                htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
              End If


              If Not IsDBNull(r.Item("TRANS_LAST_6_MONTHS")) Then
                htmlOut.Append("<td align=""right"" valign=""top"">" & r.Item("TRANS_LAST_6_MONTHS") & "&nbsp;</td>")
              Else
                htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
              End If


              If Not IsDBNull(r.Item("PRICES_LAST_6_MONTHS")) Then
                htmlOut.Append("<td align=""right"" valign=""top"">" & r.Item("PRICES_LAST_6_MONTHS") & "&nbsp;</td>")
              Else
                htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
              End If

              htmlOut.Append("<td align=""right"" valign=""top"">" & percent_have & "%&nbsp;</td>")


              htmlOut.Append("</tr>")
            End If

          Next

          htmlOut.Append("</table>")

          post_models_replace = Replace(post_models_replace, ",", "")


          htmlOut_return.Append("* Aircraft Post Models ")

          post_models_replace = Replace(post_models_replace, "EASY XRS", "")
          post_models_replace = Replace(post_models_replace, "EASY", "")
          post_models_replace = Replace(post_models_replace, ".", "")


          If ddl_model_type.SelectedValue <> "J" And ddl_model_type.SelectedValue <> "All" Then

          Else
            If ddl_is_good.SelectedValue = "All" Then
              If Me.show_15_year.Checked = True Then

              Else
                If Trim(post_models_replace) <> "" Then
                  htmlOut_return.Append(" ( Models Not Included: " & Trim(post_models_replace) & " )")
                End If
              End If
            End If
          End If

          If ddl_is_good.SelectedValue = "All" Then
            If temp_count > 0 Then
              total_percent = FormatNumber(((total_good / temp_count) * 100), 0)
              htmlOut_return.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
              htmlOut_return.Append("&nbsp;&nbsp;&nbsp;&nbsp;Models Meets Percentage: " & total_percent & "% ")
            End If
          End If



          htmlOut_return.Append("<br/>")

          htmlOut_return.Append(htmlOut.ToString)

        End If

      End If


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateSPDCS() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut_return.ToString
    htmlOut = Nothing
    htmlOut_return = Nothing
    results_table = Nothing

  End Function






























































  Public Function getSPDCSDataTable_original() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing 'sqlrep_level = 'JETNET'

    Try

      sQuery.Append("SELECT top 250 amod_make_name as MAKE, amod_model_name AS MODEL, amod_id AS MODID, COUNT(*) AS CLICKS,")

      sQuery.Append(" (SELECT COUNT(*) FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = journ_id AND ac_id = journ_ac_id")
      sQuery.Append(" AND journ_subcat_code_part1='WS' AND c.amod_id = ac_amod_id AND YEAR(journ_date) = 2016 ) AS TRANS2016,")

      sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      sQuery.Append(" WHERE acval_amod_id = c.amod_id AND YEAR(journ_date) = 2016) AS PRICES2016,")

      sQuery.Append(" (SELECT COUNT(*) FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_journ_id = journ_id AND ac_id = journ_ac_id")
      sQuery.Append(" and journ_subcat_code_part1='WS' AND c.amod_id = ac_amod_id AND YEAR(journ_date) = 2015 ) AS TRANS2015,")

      sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      sQuery.Append(" WHERE acval_amod_id = c.amod_id AND YEAR(journ_date) = 2015) AS PRICES2015,")

      sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      sQuery.Append(" WHERE acval_amod_id = c.amod_id AND YEAR(journ_date) < 2015) AS PRICEPRIOR,")

      sQuery.Append(" (SELECT COUNT(distinct acval_journ_id) FROM Aircraft_Value WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON acval_journ_id = journ_id")
      sQuery.Append(" WHERE acval_amod_id = c.amod_id ) AS PRICETOT")

      sQuery.Append(" FROM Subscription_Install_Log a WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft b WITH(NOLOCK) ON subislog_ac_id = ac_id AND ac_journ_id = 0")
      sQuery.Append(" INNER JOIN Aircraft_Model c WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE subislog_amod_id > 0 AND subislog_msg_type = 'UserStatistics'")
      sQuery.Append(" AND subislog_subid NOT IN (SELECT DISTINCT sub_id FROM Subscription WHERE sub_comp_id = 135887)")

      sQuery.Append(" AND subislog_date >= '1/1/2016' AND amod_type_code IN('E','J')")

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")

      sQuery.Append(" ORDER BY COUNT(*) DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getSPDCSDataTable() As DataTable</b><br />" + sQuery.ToString

      Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

      If Not useBackupSQL Then
        SqlConn.ConnectionString = My.Settings.TEST_LOCAL_MSSQL
      Else
        SqlConn.ConnectionString = My.Settings.TEST_LOCAL_MSSQL_BK
      End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getSPDCSDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getSPDCSDataTable() As DataTable</b><br />" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Function generateSPDCS_Original() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getSPDCSDataTable_original()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Sale Price Data Collection Summary</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns
            htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns
              htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
            Next

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateSPDCS() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

End Class