Partial Public Class MapItems
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            'For right now, we're going to pass some hard coded values
            Dim ResultsTable As New DataTable
            Dim counter As Integer = 1
            Master.SetPageTitle("Map of Selected Aircraft")
            'This registers the javascript (reused on different pages)
            DisplayFunctions.BuildJavascriptMap(Me, Me.GetType, False, "map_canvas", 0, False, False)
            'Temporary to build a layout design
            If Not Page.IsPostBack Then
                Dim portfolioID As Boolean = False
                If Not IsNothing(Trim(Request("id"))) Then
                    If Trim(Request("id")) = "true" Then
                        portfolioID = True
                    End If
                End If

                If portfolioID = False Then
                    If Not IsNothing(Session.Item("Aircraft_Master")) Then
                        ResultsTable = CType(Session.Item("Aircraft_Master"), DataTable)
                    End If
                Else
                    If Not IsNothing(HttpContext.Current.Session.Item("portfolioAircraft")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("portfolioAircraft").ToString.Trim) Then
                            Dim ac_list As String = ""
                            ac_list = HttpContext.Current.Session.Item("portfolioAircraft").ToString.Trim
                            ResultsTable = GenerateAircraftListTableFromID(ac_list)
                        End If
                    End If
                End If

                Master.SetContainerClass("container MaxWidthRemove") 'set full width page
                If Not IsNothing(ResultsTable) Then
                    If ResultsTable.Rows.Count > 0 Then
                        locations_generated.Text = "<script type=""text/javascript"">" & vbNewLine
                        locations_generated.Text += "locations = [" & vbNewLine

                        For Each r As DataRow In ResultsTable.Rows
                            If counter < 501 Then
                                If Not IsDBNull(r("aport_latitude_decimal")) And Not IsDBNull(r("aport_longitude_decimal")) Then
                                    Dim AirportTitleString As String = counter & ".) " & r("amod_make_name").ToString & " " & r("amod_model_name").ToString & " S/N # " & crmWebClient.DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "text_underline", "") & "<br />"
                                    AirportTitleString += "<em class=""tiny_text"">IATA/ICAO: " & r("ac_aport_iata_code") & "-" & r("ac_aport_icao_code") & "</em>"

                                    aircraft_list.Text += "<tr><td>" & AirportTitleString & "</td></tr>"
                                    locations_generated.Text += "['" & Replace(AirportTitleString, "'", """") & "<br /><br /><em class=""tiny_text red_text"">Click Marker to View Aircraft</em>', " & r("aport_latitude_decimal").ToString & ", " & r("aport_longitude_decimal").ToString & ", " & r("ac_id").ToString & "]," & vbNewLine
                                    counter += 1
                                End If
                            End If
                        Next

                        If counter > 500 Then
                            warning.Visible = True
                            aircraftWarningBox.Visible = True
                        End If

                        If (counter - 1) < ResultsTable.Rows.Count Then
                            warningAircraftNotShow.Visible = True
                            aircraftWarningBox.Visible = True
                        End If



                        locations_generated.Text = locations_generated.Text.TrimEnd(locations_generated.Text, ",")
                        locations_generated.Text += "]" & vbNewLine
                        locations_generated.Text += "</script>" & vbNewLine
                    End If
                End If

                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Draw Map", "BuildPoints();", True)
                ResultsTable.Dispose()
            End If
        End If

    End Sub

    Public Function GenerateAircraftListTableFromID(ByVal aclist As String) As DataTable

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing
        Dim count As Integer = 0
        Dim sQuery As New StringBuilder()

        Try

            If Not String.IsNullOrEmpty(aclist) Then

                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

                SqlConn.Open()

                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = System.Data.CommandType.Text
                SqlCommand.CommandTimeout = 90

                sQuery.Append("SELECT DISTINCT amod_id, amod_make_name, amod_model_name, aport_latitude_decimal, aport_longitude_decimal, ac_ser_no_full, ac_id, ac_aport_icao_code, ac_aport_iata_code")
                sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) WHERE ac_id IN (")
                sQuery.Append(aclist)
                sQuery.Append(") AND ac_journ_id = 0 ")
                sQuery.Append(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))

                sQuery.Append(" GROUP BY amod_id, amod_make_name, amod_model_name, aport_latitude_decimal, aport_longitude_decimal, ac_ser_no_full, ac_id, ac_aport_icao_code, ac_aport_iata_code")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GenerateAircraftListTableFromID(ByVal aclist As String) As Datatable<br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                _recordSet = SqlCommand.ExecuteReader()

                Try
                    _dataTable.Load(_recordSet)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                End Try

                _recordSet.Close()
                _recordSet = Nothing

            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_tab7_results_table(ByVal aclist As String, ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        End Try

        Return _dataTable

    End Function


End Class