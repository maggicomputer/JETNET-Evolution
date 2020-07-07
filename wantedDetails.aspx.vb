Partial Public Class wantedDetails
    Inherits System.Web.UI.Page
    Dim WantedID As Long


    Public cHTMLnbsp As String = "&nbsp;"
    Public CRLF = Chr(13) & Chr(10)
    Public EXCEL2003CHAR = Chr(160)
    Public QUOTE = Chr(34)

    Const cMultiDelim = ", "
    Const cCommaDelim = ","
    Const cColonDelim = ":"
    Const cSemiColonDelim = ";"
    Const cWildCard = "*"
    Const cImbedComa = "_"
    Const cHyphen = "-"
    Const cSpaceDelim = " "

    Const cEmptyString = ""
    Const cSingleSpace = " "
    Const cSingleQuote = "'"
    Const cDoubbleSingleQuote = "''"

    Private Sub wantedDetails_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            If Not IsNothing(Request.Item("id")) Then
                If Not String.IsNullOrEmpty(Request.Item("id").ToString) Then
                    WantedID = CLng(Request.Item("id").ToString.Trim)
                End If
            End If

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                view_folders.Visible = False
            End If

            '----------------------------------------------------------------------------------------
            'NOTE: I want to put this in only on postback, but because it's being built dynamically,
            'and controls are being added with handlers, it faces the same problem as the aircraft listing page advanced search. 
            'Meaning that if not built on every init, they won't exist.
            'So this is a note to take a look at this whenever the aircraft listing page is 
            'worked through and use the same approach that was decided on there.
            '----------------------------------------------------------------------------------------
            'This Function Builds the Dynamic Table for Static Folders. This will allow them to add 
            'Aircraft to folders and this will only be built once. This is also built on page initialization because
            'It's adding dynamic controls to the page. These have to be put in at the very begining of the page lifecycle of the viewstate
            'will not be set.
            Build_Dynamic_Folder_Table()

            Dim ChartJavascript As String = ""
            ChartJavascript = "function loadMasonry() {" & vbNewLine
            ChartJavascript += "var grid = document.querySelector('.grid');" & vbNewLine
            ChartJavascript += "var msnry = new Masonry(grid, {" & vbNewLine
            ChartJavascript += "itemSelector: '.grid-item'," & vbNewLine
            ChartJavascript += "columnWidth: '.grid-item'," & vbNewLine
            ChartJavascript += "gutter: 10," & vbNewLine
            ChartJavascript += "horizontalOrder: true," & vbNewLine
            ChartJavascript += "percentPosition: true" & vbNewLine
            ChartJavascript += "});" & vbNewLine
            ChartJavascript += "}" & vbNewLine
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StartupScript", ChartJavascript, True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            Master.SetPageTitle("Wanted Details")
        End If

        Me.content_wanted.Text = select_wanted_details()

    End Sub

    Public Sub ViewWantedFolders(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_folders.Click
        If InStr(view_folders.CssClass, "blue_button") > 0 Then
            ToggleButtons(False)
        Else
            ToggleButtons(True)
        End If
        folders_update_panel.Update()
    End Sub

    Public Sub ToggleButtons(ByVal FoldersVis As Boolean)
        If FoldersVis Then
            closeFolders.Visible = True
            folders_tab.Visible = True
            'folders_container.CssClass = "blue-theme"
            view_folders.CssClass = "blue_button float_left noBefore"
            'view_folders.Text = "Close Folders"
        Else
            closeFolders.Visible = False
            folders_tab.Visible = False
            'folders_container.CssClass = "dark-theme"
            view_folders.CssClass = "gray_button float_left noBefore"
            ' view_folders.Text = "View Folders"
            folders_update_panel.Update()
        End If

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If
    End Sub

    Public Function select_wanted_details() As String
        select_wanted_details = ""

        Dim nAircraftModelID As Long = 0
        Dim staticBkgroundImage As String = ""
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlConn2 As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlCommand2 As New System.Data.SqlClient.SqlCommand
        Dim adoTempRS As New DataTable
        Dim adoRS As System.Data.SqlClient.SqlDataReader : adoRS = Nothing
        Dim adoRSAircraft As System.Data.SqlClient.SqlDataReader : adoRSAircraft = Nothing
        Dim Query As String = ""
        Dim amod_id As Long = 0
        Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

        Try

            If Trim(Request("homebase")) = "Y" Then
                SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase")
                SqlConn2.ConnectionString = Session.Item("jetnetAdminDatabase")
            Else
                SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
                SqlConn2.ConnectionString = Session.Item("jetnetClientDatabase")
            End If

            SqlConn.Open()
            SqlConn2.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand2.Connection = SqlConn2
            SqlCommand2.CommandType = System.Data.CommandType.Text
            SqlCommand2.CommandTimeout = 60



            nAircraftModelID = 0


            'Me.top_section.Text += "<head>"
            'Me.top_section.Text += "<meta http-equiv='Content-Language' content='en-us' />"
            'Me.top_section.Text += "<meta http-equiv='Content-Type' content='text/html; charset=windows-1252' />"
            'Me.top_section.Text += "<meta name='apple-mobile-web-app-capable' content='yes' />"
            'Me.top_section.Text += "<meta name='format-detection' content='telephone=no' /> "
            'Me.top_section.Text += "<link href='common/jetnet.css' type='text/css' rel='stylesheet' />"
            'Me.top_section.Text += "<link href='css/i -landscape.css' rel='stylesheet' media='all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)' />"
            'Me.top_section.Text += "<link href='css/ipad-landscape.css' rel='stylesheet' media='all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)' />"
            'Me.top_section.Text += "<link href='css/regular.css' rel='stylesheet' media='all and (min-device-width: 1024px)' />"
            'Me.top_section.Text += "<title>JETNET - Display Wanted Details</title>"
            'Me.top_section.Text += "</head>"



            'Me.top_section.Text += "<body id='bodyID' class='bg_image_ie' style='background-image: url(" & staticBkgroundImage & "); margin-top:0px; margin-left:10px;'>"
            'Me.top_section.Text += "<div style='text-align:center;'>"


            'Me.top_section.Text += (vbCrLf & "<table class='centerTable' border='0' cellspacing='0' cellpadding='2' width='80%'><tr><td style='vertical-align:bottom;'>")

            'if session("Aerodex") then
            '  if session("isHeliOnlyProduct") then
            '    select_wanted_details +=  "<img id='evoProductImgID' src='/images/rotodex_red.png' style='width: 300px; height: 100px; text-align:left; vertical-align:bottom;' border='0' alt='' /></td>"
            '    select_wanted_details +=  "<td><div class='pageHeaderRoto'>Display Helicopter History</div>"
            '  else
            '    select_wanted_details +=  "<img id='evoProductImgID' src='/images/JN_AerodexMarketplace_Logo2.png' style='width: 300px; height: 100px; text-align:left; vertical-align:bottom;' border='0' alt='' /></td>"
            '    select_wanted_details +=  "<td><div class='pageHeaderAero'>Display Aircraft History</div>"
            '  end if
            'else
            '  if session("isHeliOnlyProduct") then
            '    select_wanted_details +=  "<img id='evoProductImgID' src='/images/helidex_blue.png' style='width: 300px; height: 100px; text-align:left; vertical-align:bottom;' border='0' alt='' /></td>"
            '    select_wanted_details +=  "<td><div class='pageHeaderHelo'>Display Helicopter History</div>"
            '  else

            ' commented out msw
            'select_wanted_details += ("<img id='evoProductImgID' src='/images/generic_EVO_logo.png' style='width: 300px; text-align:left; vertical-align:bottom;' border='0' alt='' /></td>")
            'Me.top_section.Text += ("<td style='vertical-align:bottom;'><div class='pageHeaderEvo'>Display Wanted Details</div>")
            '  end if
            'end if 
            Master.SetPageTitle("Wanted Details")

            'Me.top_section.Text += ("</td></tr></table>")

            If Request("id") <> "" Then

                nAircraftModelID = Request("id")

                ' Query = "SELECT * FROM Aircraft_Model_Wanted WITH(NOLOCK) WHERE amwant_id = " & nAircraftModelID

                Query = "Select distinct  amwant_listed_date, amwant_comp_id, amwant_start_year, amwant_end_year, amwant_max_price, amwant_max_aftt, amwant_accept_damage_hist, "
                Query += " amwant_accept_dam_cur, amwant_notes, amwant_year_note, amwant_amount_note, amod_make_name, amod_model_name, amod_id "
                Query += " FROM Aircraft_Model_Wanted WITH(NOLOCK) "
                Query += " INNER JOIN Aircraft_model on amod_id = amwant_amod_id"
                Query += "  WHERE amwant_id = " & nAircraftModelID


                SqlCommand2.CommandText = Query
                adoRS = SqlCommand2.ExecuteReader()


                select_wanted_details += ("<table class='formatTable blue' border='0' cellspacing='0' cellpadding='2' width='100%'><tr><td style='vertical-align:top; text-align:left;'>")


                If adoRS.HasRows Then
                    adoRS.Read()

                    amod_id = adoRS("amod_id")


                    Me.make_model.Text = adoRS("amod_make_name") & " " & adoRS("amod_model_name")


                    select_wanted_details += ("<br /><table  width='100%' cellspacing='2' cellpadding='2' border='0'>")

                    'select_wanted_details += "<div class=""row"">"
                    'select_wanted_details += "<div class=""four columns"">"

                    If Not IsDBNull(adoRS("amwant_listed_date")) Then
                        select_wanted_details += build_column_section("Date Listed", adoRS("amwant_listed_date"))
                    End If


                    If Not IsDBNull(adoRS("amwant_start_year")) Then
                        select_wanted_details += build_column_section("Start Year", adoRS("amwant_start_year"))
                    End If


                    If Not IsDBNull(adoRS("amwant_end_year")) Then
                        select_wanted_details += build_column_section("End Year", adoRS("amwant_end_year"))
                    End If

                    If Not IsDBNull(adoRS("amwant_max_price")) Then
                        select_wanted_details += build_column_section("Max Price", "$ " & clsGeneral.clsGeneral.ConvertIntoThousands(adoRS("amwant_max_price")))
                    End If

                    If Not IsDBNull(adoRS("amwant_max_aftt")) Then
                        select_wanted_details += build_column_section("Max AFTT", adoRS("amwant_max_aftt"))
                    End If


                    ' If Not IsDBNull(adoRS("amwant_max_aftt")) Then
                    '   select_wanted_details += build_column_section("Max AFTT", adoRS("amwant_max_aftt"))
                    ' End If


                    If Not IsDBNull(adoRS("amwant_accept_damage_hist")) Then
                        Select Case UCase(adoRS("amwant_accept_damage_hist"))
                            Case "Y"
                                select_wanted_details += build_column_section("Accept Hist Damage?", "Yes")
                            Case "N"
                                select_wanted_details += build_column_section("Accept Hist Damage?", "No")
                            Case "U"
                                select_wanted_details += build_column_section("Accept Hist Damage?", "Unknown")
                        End Select
                    End If

                    If Not IsDBNull(adoRS("amwant_accept_dam_cur")) Then
                        Select Case UCase(adoRS("amwant_accept_dam_cur"))
                            Case "Y"
                                select_wanted_details += build_column_section("Accept Curr Damage?", "Yes")
                            Case "N"
                                select_wanted_details += build_column_section("Accept Curr Damage?", "No")
                            Case "U"
                                select_wanted_details += build_column_section("Accept Curr Damage?", "Unknown")
                        End Select
                    End If

                    'select_wanted_details += "</div>"
                    'select_wanted_details += "<div class=""four columns"">"
                    select_wanted_details += "</table></td><td width='50%'>"
                    select_wanted_details += FillModelPictureAndVideo(amod_id)
                    select_wanted_details += "</td></tr>"
                    'select_wanted_details += "</div>"
                    ' select_wanted_details += "</div>"

                    If Not IsDBNull(adoRS("amwant_notes")) Then
                        select_wanted_details += ("<tr><td colspan=""2"">" & adoRS("amwant_notes") & "</td></tr>")
                    End If

                    If Not IsDBNull(adoRS("amwant_year_note")) Then
                        select_wanted_details += ("<tr><td colspan=""2"">Year Note: " & adoRS("amwant_year_note") & "</td></tr>")
                    End If


                    If Not IsDBNull(adoRS("amwant_amount_note")) Then
                        select_wanted_details += ("<tr><td colspan=""2"">Amount Note: " & adoRS("amwant_amount_note") & "</td></tr>")
                    End If



                    crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, company_label, Master, adoRS("amwant_comp_id"), 0, "", about_label, New AjaxControlToolkit.TabContainer, company_address_label, company_name_label, False)


                Else
                    select_wanted_details += ("<tr><td colspan=""2"">No Wanted Record Found</td></tr>")
                End If

                select_wanted_details += ("</table><br />")

            End If

            adoRS.Close()

            select_wanted_details += "</td>"
            select_wanted_details += "</tr>"
            select_wanted_details += "</table> "
            'select_wanted_details += "</div></body></html>	"

            '  adoRSAircraft.Dispose()
            adoTempRS.Dispose()
            adoRS.Dispose()
            adoRSAircraft = Nothing
            adoTempRS = Nothing
            adoRS = Nothing


        Catch ex As Exception
            ' aCommonEvo.DisplayAlert("Error in btnRunReport_Click: " & ex.Message)
        Finally
            SqlConn.Close()
            SqlConn.Dispose()
            SqlConn = Nothing

            SqlConn2.Close()
            SqlConn2.Dispose()
            SqlConn2 = Nothing
        End Try
    End Function

    Public Function build_column_section(ByVal name1 As String, ByVal value As String) As String
        build_column_section = ""

        build_column_section += ("<tr><td>" & name1 & "</td><td align='left' width='65%'>")
        build_column_section += (value)
        build_column_section += "</td></tr>"

    End Function

    Public Function FillModelPictureAndVideo(ByVal ModelID As Long) As String

        If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
            FillModelPictureAndVideo = "<img src=""https://www.testjetnetevolution.com/pictures/model/" + ModelID.ToString + ".jpg"" width=""200"" class=""border padding"" />"
        Else
            FillModelPictureAndVideo = "<img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ModelPicturesFolderVirtualPath") + "/" + ModelID.ToString + ".jpg"" width=""200"" class=""border padding"" />"
        End If

        'video 
        'Session("ModelVideosFolderVirtualPath") & "/" & ModelID & ".mpeg"
    End Function

    ''' <summary>
    ''' This function is running to build the dynamic folder list to allow adding to static folders.
    ''' It's built dynamically in page init
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Build_Dynamic_Folder_Table()
        'Dim FoldersTable As New DataTable
        Dim ContainerTable As New Table
        Dim TR As New TableRow
        Dim TDHold As New TableCell
        Dim SubmitButton As New LinkButton


        ContainerTable = DisplayFunctions.CreateStaticFoldersTable(0, 0, 0, WantedID, 0, Master.aclsData_Temp, 0)
        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)

        SubmitButton.Text = "Save Folders"
        SubmitButton.ID = "SaveStaticFoldersButton"
        ContainerTable.CssClass = "formatTable blue small"
        AddHandler SubmitButton.Click, AddressOf SaveStaticFolders

        TDHold.Controls.Add(SubmitButton)
        TR.Controls.Add(TDHold)

        ContainerTable.Controls.Add(TR)

        folders_label.Controls.Clear()
        folders_label.Controls.Add(ContainerTable)

        folders_update_panel.Update()
    End Sub
    ''' <summary>
    ''' This function allows saving of static folders.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveStaticFolders()
        folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, Master.aclsData_Temp, 0, 0, WantedID, 0, 0, 0)
        folders_update_panel.Update()
    End Sub
End Class