Imports System.IO
Partial Public Class export
  Inherits System.Web.UI.Page
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable
  Dim error_string As String = ""
#Region "Page Events"

  Private Sub export_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Try
      Dim outlook As Boolean = False

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      End If

      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = Application.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")

      If Not IsNothing(Trim(Request("outlook"))) Then
        outlook = IIf(Trim(Request("outlook") = "true"), True, False)
      End If


      If Session.Item("localUser").crmEvo = False Then
        If Session("export_info") <> "" And outlook = False Then

          Session("ignore") = ""
          Response.Buffer = True
          Response.ClearContent()
          Response.ClearHeaders()
          Response.Clear()
          Response.AddHeader("content-disposition", "attachment;filename=export.xls")
          Response.Charset = ""
          Response.Cache.SetCacheability(HttpCacheability.NoCache)
          Response.ContentType = "application/vnd.xls"

          Session("export_info") = Replace(Session("export_info"), "<img src='images/client.png' alt='CLIENT RECORD' class='ico_padding'/>", "CLIENT")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/client.png' alt='CLIENT RECORD' title='CLIENT RECORD' class='ico_padding'/>", "CLIENT")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding'/>", "CLIENT")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding' title='JETNET RECORD' />", "JETNET")
          Session("export_info") = Replace(Session("export_info"), "<a href=", "<span class=")
          Session("export_info") = Replace(Session("export_info"), "</a>", "</span>")

          Session("export_info") = Replace(Session("export_info"), "<img src='images/red_arrow.gif' alt='For Sale' width='25' />", "FOR SALE")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/purple_arrow.gif' alt='Exclusive' width='25' />", "EXCLUSIVE")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/orange_arrow.gif' alt='Lease' width='25' />", "LEASED")

          Session("export_info") = Replace(Session("export_info"), "<input type=""checkbox"" />", "")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/pilot.png' alt='PILOT' width='20' align='center' />", "PILOT")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/wrench.png' alt='MECHANIC' align='center'/>", "MECHANIC")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>", "EXCLUSIVE")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/orange_arrow.gif' alt='Lease' width='25'/>", "LEASE")
          Session("export_info") = Replace(Session("export_info"), "<img src='images/red_arrow.gif' alt='For Sale' width='25'/>", "FOR SALE")




          Response.Write(Session("export_info"))

          'Session("export_info") = ""
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "self.close();", True)
        Else

          Select Case Trim(Request("parent"))
            Case "1"

              ' USE THE FILE-EXTENSION .VCF !!!!
              Dim idnum As Integer = Session.Item("ListingID")
              Dim source As String = Session.Item("ListingSource")

              ' create the FileSystemObject
              'fso = CreateObject("Scripting.FileSystemObject")

              ' open the file for overwriting
              'fVCardFile = fso.CreateTextFile(sFileName, True)

              Dim writer As System.IO.StreamWriter

              writer = New System.IO.StreamWriter(Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString & "\" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf"))


              aTempTable = aclsData_Temp.GetCompanyInfo_ID(idnum, source, 0)
              ' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each R As DataRow In aTempTable.Rows

                    ' write all information from SQL statement to the vCard file
                    writer.WriteLine("BEGIN:VCARD")
                    writer.WriteLine("VERSION:2.1")
                    writer.WriteLine("ORG:" & R("comp_name") & "")
                    writer.WriteLine("TITLE:")

                    aTempTable = aclsData_Temp.GetPhoneNumbers(idnum, 0, source, 0)
                    '' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                      If aTempTable.Rows.Count > 0 Then
                        ' set it to the datagrid 
                        For Each q As DataRow In aTempTable.Rows
                          If q("pnum_type") = "Office" Then
                            writer.WriteLine("TEL;WORK;VOICE:" & q("pnum_number"))
                          End If
                          If q("pnum_type") = "Fax" Then
                            writer.WriteLine("TEL;WORK;FAX:" & q("pnum_number"))
                          End If

                          If q("pnum_type") = "Mobile" Then
                            writer.WriteLine("TEL;CELL:" & q("pnum_number"))
                          End If

                          If q("pnum_type") = "Residence" Then
                            writer.WriteLine("TEL;HOME;VOICE:" & q("pnum_number"))
                          End If

                          If q("pnum_type") = "Residence Fax" Then
                            writer.WriteLine("TEL;HOME;FAX:" & q("pnum_number"))
                          End If
                        Next
                      End If

                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("export.aspx.vb - Page Load() - " & error_string)
                      End If
                      display_error()
                    End If

                    writer.WriteLine("EMAIL;WORK:" & R("comp_email_address"))
                    writer.WriteLine("ADR;HOME:;;" & R("comp_address1") & " " & R("comp_address2") & ";" & R("comp_city") & ";" & R("comp_state") & ";" & R("comp_zip_code") & ";" & R("comp_country"))
                    writer.WriteLine("END:VCARD")

                  Next
                Else
                  ' Response.Write("no rows")
                End If

              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export.aspx.vb - Page Load() - " & error_string)
                End If
                display_error()
              End If


              ' close the file and set the FileSystemObject to nothing
              'fVCardFile.Close()
              'fso = Nothing

              Response.ContentType = "text/x-vcard"
              Response.AppendHeader("Content-Disposition", "attachment; filename=" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf")
              writer.Close()
              Response.TransmitFile(Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString & "\" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf"))
            Case "2"


              Dim idnum As Integer = Session.Item("ContactID")
              Dim source As String = Session.Item("ListingSource")

              Dim writer As System.IO.StreamWriter

              writer = New System.IO.StreamWriter(Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString & "\" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf"))


              aTempTable = aclsData_Temp.GetContacts_Details(idnum, source)
              ' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each R As DataRow In aTempTable.Rows

                    ' write all information from SQL statement to the vCard file
                    writer.WriteLine("BEGIN:VCARD")
                    writer.WriteLine("VERSION:2.1")
                    writer.WriteLine("N:" & R("contact_first_name") & " " & R("contact_last_name"))
                    writer.WriteLine("FN:" & R("contact_sirname") & " " & R("contact_first_name") & " " & R("contact_middle_initial") & " " & R("contact_last_name"))
                    writer.WriteLine("TITLE:" & R("contact_title"))

                    aTempTable2 = aclsData_Temp.GetPhoneNumbers(0, idnum, source, 0)
                    '' check the state of the DataTable
                    If Not IsNothing(aTempTable2) Then
                      If aTempTable2.Rows.Count > 0 Then
                        ' set it to the datagrid 
                        For Each f As DataRow In aTempTable2.Rows
                          If Not IsDBNull(f("pnum_type")) Then
                            If f("pnum_type") = "Office" Then
                              writer.WriteLine("TEL;WORK;VOICE:" & f("pnum_number"))
                            End If
                            If f("pnum_type") = "Fax" Then
                              writer.WriteLine("TEL;WORK;FAX:" & f("pnum_number"))
                            End If

                            If f("pnum_type") = "Mobile" Then
                              writer.WriteLine("TEL;CELL:" & f("pnum_number"))
                            End If

                            If f("pnum_type") = "Residence" Then
                              writer.WriteLine("TEL;HOME;VOICE:" & f("pnum_number"))
                            End If

                            If f("pnum_type") = "Residence Fax" Then
                              writer.WriteLine("TEL;HOME;FAX:" & f("pnum_number"))
                            End If
                          End If
                        Next
                      End If

                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("export.aspx.vb - Page Load() - " & error_string)
                      End If
                      display_error()
                    End If


                    If Not IsDBNull(R("contact_comp_id")) Then
                      aTempTable2 = aclsData_Temp.GetPhoneNumbers(R("contact_comp_id"), idnum, source, 0)
                      '' check the state of the DataTable
                      If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                          ' set it to the datagrid 
                          For Each f As DataRow In aTempTable2.Rows
                            If Not IsDBNull(f("pnum_type")) Then
                              If f("pnum_type") = "Office" Then
                                writer.WriteLine("TEL;WORK;VOICE:" & f("pnum_number"))
                              End If
                              If f("pnum_type") = "Fax" Then
                                writer.WriteLine("TEL;WORK;FAX:" & f("pnum_number"))
                              End If

                              If f("pnum_type") = "Mobile" Then
                                writer.WriteLine("TEL;CELL:" & f("pnum_number"))
                              End If

                              If f("pnum_type") = "Residence" Then
                                writer.WriteLine("TEL;HOME;VOICE:" & f("pnum_number"))
                              End If

                              If f("pnum_type") = "Residence Fax" Then
                                writer.WriteLine("TEL;HOME;FAX:" & f("pnum_number"))
                              End If
                            End If
                          Next
                        End If

                      Else
                        If aclsData_Temp.class_error <> "" Then
                          error_string = aclsData_Temp.class_error
                          LogError("export.aspx.vb - Page Load() - " & error_string)
                        End If
                        display_error()
                      End If
                    End If

                    writer.WriteLine("EMAIL;WORK:" & R("contact_email_address"))
                    If Not IsDBNull(R("contact_comp_id")) Then
                      aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(R("contact_comp_id"), source, 0)
                      ' check the state of the DataTable
                      If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                          For Each q As DataRow In aTempTable2.Rows
                            writer.WriteLine("ORG:" & q("comp_name") & "")
                            writer.WriteLine("ADR;HOME:;;" & q("comp_address1") & " " & q("comp_address2") & ";" & q("comp_city") & ";" & q("comp_state") & ";" & q("comp_zip_code") & ";" & q("comp_country"))

                          Next
                        End If
                      End If
                    End If
                    writer.WriteLine("END:VCARD")

                  Next
                Else
                  ' Response.Write("no rows")
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("export.aspx.vb - Page Load() - " & error_string)
                End If
                display_error()
              End If


              Response.ContentType = "text/x-vcard"
              Response.AppendHeader("Content-Disposition", "attachment; filename=" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf")
              writer.Close()
              Response.TransmitFile(Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString & "\" & Session.Item("localUser").crmUserTemporaryFilePrefix & "contact.vcf"))
            Case Else
              Response.Write("<p style='font-family:verdana;font-size:11px;'><em>There are no aircraft to export.</em></p>")

          End Select
        End If
      ElseIf Session.Item("localUser").crmEvo = True Then
        'This is the evolution side. This link will come from the home page

        'This happens if the report ID is nothing anything. If it isn't,
        'This means the link is coming from the home page tab.
        If Not IsNothing(Request.Item("repID")) Then
          If Not String.IsNullOrEmpty(Request.Item("repID").ToString.Trim) Then
            If IsNumeric(Request.Item("repID")) Then
              'Setting up the admin center datalayer.
              Dim localDatalayer As New admin_center_dataLayer

              'Naming variables for the report name. Following the
              'same schema as the admin side - with one difference - instead of adminReport it'll be Report.
              Dim sReportString As String = ""
              Dim sReportFileName As String = ""
              Dim sReportOutputFilename As String = ""
              Dim nReportID As Integer = CInt(Request.Item("repID").ToString)
              Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"
              Dim sReportTitle = subscriptionInfo + "Report_" + nReportID.ToString

              'Setting up the datalayer connections.
              localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
              localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
              localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
              localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
              localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

              'Function that calls the generate report.
              localDatalayer.generateAdminReport(nReportID, sReportString, Session.Item("localUser").crmSubSubID, "Subscription,All", False, Session.Item("localPreferences").AerodexFlag)

              If Not String.IsNullOrEmpty(sReportString.Trim) Then
  
                Dim f As System.IO.StreamWriter

                sReportFileName = commonEvo.GenerateFileName(sReportTitle, ".xls", False)

                sReportOutputFilename = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "/" + sReportFileName.Trim

                f = System.IO.File.CreateText(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sReportFileName.Trim)

                ' write to the file
                f.WriteLine(sReportString)

                'close the streamwriter
                f.Close()
                f.Dispose()
                f = Nothing

                Response.Redirect(sReportOutputFilename, False)
              End If
            End If
          End If
        Else
          'Okay, if the report ID isn't passed correctly, then we use this page as a blank page to write out headers
          'for the evolution export. So if the session variable export_info isn't blank, go ahead
          'and write those xls headers and direct the user along.
          If Session("export_info") <> "" And outlook = False Then

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export.aspx - SETUP<b><br />"


            Session("ignore") = ""
            Response.Buffer = True
            Response.ClearContent()
            Response.ClearHeaders()
            Response.Clear() 

            If Trim(HttpContext.Current.Session.Item("export_type")) = "export_now_csv" Then
              Response.AddHeader("content-disposition", "attachment;filename=export.csv")
              Response.Charset = ""
              Response.Cache.SetCacheability(HttpCacheability.NoCache)
              Response.ContentType = "application/vnd.csv"
            Else
              Response.AddHeader("content-disposition", "attachment;filename=export.xls")
              Response.Charset = ""
              Response.Cache.SetCacheability(HttpCacheability.NoCache)
              Response.ContentType = "application/vnd.xls"
            End If


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export.aspx - REPLACES<b><br />"

            Session("export_info") = Replace(Session("export_info"), "<img src='images/client.png' alt='CLIENT RECORD' class='ico_padding'/>", "CLIENT")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/client.png' alt='CLIENT RECORD' title='CLIENT RECORD' class='ico_padding'/>", "CLIENT")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding'/>", "CLIENT")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/evo.png' alt='JETNET RECORD' class='ico_padding' title='JETNET RECORD' />", "JETNET")
            Session("export_info") = Replace(Session("export_info"), "<a href=", "<span class=")
            Session("export_info") = Replace(Session("export_info"), "</a>", "</span>")

            Session("export_info") = Replace(Session("export_info"), "<img src='images/red_arrow.gif' alt='For Sale' width='25' />", "FOR SALE")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/purple_arrow.gif' alt='Exclusive' width='25' />", "EXCLUSIVE")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/orange_arrow.gif' alt='Lease' width='25' />", "LEASED")

            Session("export_info") = Replace(Session("export_info"), "<input type=""checkbox"" />", "")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/pilot.png' alt='PILOT' width='20' align='center' />", "PILOT")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/wrench.png' alt='MECHANIC' align='center'/>", "MECHANIC")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>", "EXCLUSIVE")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/orange_arrow.gif' alt='Lease' width='25'/>", "LEASE")
            Session("export_info") = Replace(Session("export_info"), "<img src='images/red_arrow.gif' alt='For Sale' width='25'/>", "FOR SALE")
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export.aspx - PRINT<b><br />"
            Response.Write(Session("export_info"))
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Export.aspx - AFTER PRINT<b><br />"
          End If
        End If

        End If
    Catch ex As Exception
      error_string = "export.aspx.vb - Page Load() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

  End Sub
#End Region
#Region "Error Handling for datamanager"
  Function display_error()
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(Replace(aclsData_Temp.class_error, "'", ""), vbNewLine, "") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub
#End Region


End Class